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


'Modificado Por : Jhon Fredy Gonzalez JFGB20160511
'Fecha          : 11 de Mayo del 2016
'Descripción    : Se realiza implementación de selección de concepto en todos los detalles de tesoreria.
'                 la selección de la cuenta contable dependera de ese concepto, como la selección del comitente y del banco


#End Region

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports Microsoft.VisualBasic.CompilerServices
Imports A2Utilidades.Mensajes

Public Class TesoreriaViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"

    'Public Property cb As New CamposBusquedaTesoreri
    Private TesoreriPorDefecto As Tesoreri
    Private TesoreriAnterior As Tesoreri
    Private DetalleTesoreriPorDefecto As DetalleTesoreri
    Const CUENTA_4_POR_MIL_DE_BANCO As String = "Cuenta4pormilDeBanco"
    Const STR_SI As String = "SI"
    Const STR_NO As String = "NO"
    Dim mstrlogCuenta_4_pormil_DeBanco As String
    Dim mstrNroCuentaContableBanco As String
    Dim dcProxy As TesoreriaDomainContext
    Dim dcProxy1 As TesoreriaDomainContext
    Dim dcProxy2 As TesoreriaDomainContext
    Dim dcProxy3 As MaestrosDomainContext
    Dim dcProxy4 As TesoreriaDomainContext
    Dim dcProxyTransaccion As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim objProxy1 As UtilidadesDomainContext
    Dim objProxyCarga As ImportacionesDomainContext
    Dim contador As New A2.OyD.OYDServer.RIA.Web.OyDTesoreria.Tesoreri
    Dim A2Util As A2UtilsViewModel
    Private mlogDuplicarTesoreria As Boolean = False
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim mensaje As String
    Dim MakeAndCheck As Integer = 0
    Dim Make As Boolean
    Dim esVersion As Boolean = False
    Dim codigo As Integer
    Dim fechaCierre As Nullable(Of DateTime)
    Public moduloTesoreria As String
    Dim sw As Integer
    Dim Filtro As Integer = 0
    Dim estadoMC As String
    Dim ModificacionFormaPago, msobregiro, logValidacuentasuperval As Boolean
    Dim strNombreConsecutivo As String = String.Empty
    Dim strNombreConsecutivoTodos As String = String.Empty
    Dim ValidaSobregiro, mlogVentanaGmf, mlogCobroGmf, logCobrarGMF, mlogCalculoGMFPorEncima, mlogCalculoGMFPorDebajo, mlogclienteGMF, mlogBancoGMF, ValidaCobroMGF As Boolean
    Dim Moneda As Integer = 0
    Dim Moneda2 As Integer = 0
    'Dim autorizaciones As Autorizaciones
    Private WithEvents autorizaciones As Autorizaciones
    Private WithEvents Mensajegmf As MensajeGMF
    Dim IndexPosicion As Integer
    Dim comitente, mstrNombreConsecutivoNota As String
    Dim mstrctacontableMGF, mstrccostosMGF, mstrctacontablecontrapMGF, mstrccostocontrapMGF As String
    Dim mdblGMFInferior, msnGMF, dblvalortotalConGMF As Double
    Dim strservidor, strbasedatos, strowner, strcia, strCuenta, mstrCtacontraparteNotasCXC As String
    Dim mlogctaordensuper, mlogctaordensuperbanco As Boolean
    Dim mcurTotalDebitoSV, mcurTotalCreditoSV, mcurTotalDebitos, mcurTotalCreditos As Decimal
    Dim dblvalordigitado, dblvalorsaldodetalle As Double
    Dim mcurTotalDebitoNOSV, mcurTotalCreditoNOSV As Decimal 'SV20151005
    Dim intBanco As Integer
    Dim ControlMensaje As Boolean = False
    Dim TotalComprobantesAnterior As Double

    'SLB Declaraciones de variables
    Dim cwOrdenesPendientesTesoreria As cwOrdenesPendientesTesoreria
    Dim cwAnularDocmuento As cwMotivoAnulacion
    Public _mlogBuscarCliente As Boolean = True
    Public _mlogBuscarBancos As Boolean = True
    Public _mlogBuscarCentroCostos As Boolean = True
    Public _mlogBuscarFormaEntrega As Boolean = True
    Public _mlogBuscarEntidad As Boolean = True
    Public _mlogBuscarTipoCuenta As Boolean = True
    Public _mlogBuscarTipoIdTitular As Boolean = True
    Public _mlogBuscarTipoIdentBeneficiario As Boolean = True
    Public _mlogBuscarCuentaContable As Boolean = True
    Public _mlogBuscarNIT As Boolean = True
    Private _mlogAplicaBancoDestino As Boolean = False
    Private _mstrActivarBancoTraslado As String = "NO"
    Private _msMostrarValorDebito As String = "NO"
    Private _mlogActivarListaClinton As Boolean = False
    Private _mlogGrabarListaClinton As Boolean = False
    Private _mlogCanjeCheque As Boolean = False
    Private DatosListaClinton As New OYDUtilidades.ClienteInhabilitado
    Private _mlogSeleccioNoCliente As Boolean
    Private _mstrConsecutivoTrasladoBancos As String = String.Empty
    Dim intNroDetallesValidarSuperBanco As Integer = 0
    Private _mlogAnular As Boolean = False
    Private _strCausalInactivadad As String = String.Empty
    Private _mstrAnularNotaGMF As String = String.Empty
    Dim mstrCtaContableTrasladoDB, mstrCtaContableTrasladoCR, mstrTipoNotasCxC As String
    Dim mcurTotalRC As Double
    Public DIGITAR_CUENTAS_BENEFICIARIO_CE As String = ""
    Private VersionAplicacionCliente As String = String.Empty
    Private _mInhabilitar_Edicion_Registro As String = String.Empty
    ''' <summary>
    ''' Se valida la NC si tiene un GMF asociado: contra un CE (intNroValidacionGMF_NC = 1) y contra otra NC (intNroValidacionGMF_NC = 0)
    ''' </summary>
    ''' <remarks>SLB20130313</remarks>
    Dim intNroValidacionGMF_NC As Byte = 0
    Dim SoSumitChanges As SubmitOperation
    'Dim _mlogPermiteGenerarGMF As Boolean = False
    Dim IndexGlobal As Integer = 0

    Dim ListaCombosEspecificos As List(Of OYDUtilidades.ItemCombo)
    ' JFSB 20160812 - Se crea variable para lamacenar el comitente seleccionado cuando se maneja cuenta bancaria
    Dim ClienteAnterior As OYDUtilidades.BuscadorClientes
    Dim ListaDetalleAnterior As List(Of DetalleTesoreri)
    Dim ListaChequeTesoreriaAnterior As List(Of Cheque)
    Dim CambiarPropiedades As Boolean = True
    Dim Edicion As Boolean = False
    Dim cuentaContable As String = String.Empty
    Dim TipoCuenta As String = String.Empty
    Dim SplitCuenta As Boolean = False
    Dim NotaGMFManual As Boolean = False
    Dim logReasignar As Boolean = False
    Dim EjecutarCobroGMF As Boolean = False
    Dim logEditar As Boolean = False
    Dim logCambiarCuentaBancaria As Boolean = True

    ''' <summary>
    ''' Permite controlar el valor por cliente registrado en el comprobante.
    ''' </summary>
    ''' <remarks>
    ''' Creado por       : Juan Carlos Soto Cruz.
    ''' Fecha            : Octubre 20/2011
    ''' </remarks>
    Private Structure TAcumuladorXCliente
        Dim lngIDComitente As String
        Dim VlrDB As Double
        Dim VlrCR As Double
        Dim VlrSaldo As Double
    End Structure

    ''' <summary>
    ''' Define si en el momento de los comprobantes a los usuarios se les autoriza el sobregiro.
    ''' </summary>
    ''' <remarks></remarks>
    Public glogAutorizaPago As Boolean

    ''' <summary>
    ''' Esta variable permite definir desde que modulo se esta llamando la pantalla para autorizar. 
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Public gstrModuloAutorizar As String

    ''' <summary>
    ''' Esta variable define quien autoriza un proceso especial.
    ''' </summary>
    ''' <remarks></remarks>
    Public gstrUsuarioqueAutoriza As String

    ''' <summary>
    ''' Variable que determina si al grabar un comporbante se valida si el cliente tiene saldo.
    ''' </summary>
    ''' <remarks></remarks>
    Public glogValSobregiroCE As Boolean = False

    Public glogValSobregiroNC As Boolean = False

    Private glogConceptoDetalleTesoreriaManual As Boolean = True

    Dim SaldoClientesTesoreria As Double

    ''' <summary>
    ''' Variable que determina si la opción de transferencia electrónica se encuentra disponible para un cliente
    ''' </summary>
    ''' <remarks>
    ''' Creado por       : Juan Carlos Soto Cruz.
    ''' Fecha            : Octubre 25/2011
    ''' </remarks>
    Dim TransferenciaDisponible As Boolean

    Dim vlrTotalXCliente() As TAcumuladorXCliente

    'SLB20140620 Manejo de la Configuración de Consecutivos
    Dim ListaConfiguracionConsecutivos As List(Of ConfiguracionConsecutivos)
    Dim ConfiguracionConsecutivosSelected As ConfiguracionConsecutivos
    Dim mlogClientes, mlogCuenta As Boolean
    'JFGB20160511
    Public plogEsFondosOYD As Boolean = False
    Public intIDCompaniaFirma As Integer = 0
    Public strNroDocumentoCompaniaFirma As String = String.Empty
    Public strNombreCortoCompaniaFirma As String = String.Empty
    Public strNombreCompaniaFirma As String = String.Empty
    Public logConsultarCompania As Boolean = True
    'JFSB 20160922
    Dim objRetornoCalculo As List(Of OYDUtilidades.TempRetornoCalculo)
    'JFSB 20171204
    Dim objRetornoReasignarCuenta As List(Of OyDTesoreria.RetornoReasignarCuentas)
    Dim objRetornoTesoreria As List(Of OyDTesoreria.RespuestaDatosTesoreria)

    'VENTANAEMERGENTE
    Public logVentanaEmergente As Boolean = False
    Public logEsEdicionRegistroEmergente As Boolean = False
    Public intIDDocumentoEmergente As Nullable(Of Integer) = 0
    Public strNombreConsecutivoEmergente As String = String.Empty
    Public objTesoreriaEmergente As TesoreriaEmergenteEncabezado = Nothing
    Public objVentanaEmergente As TesoreriaVentanaEmergenteView = Nothing

    Private logCuentasTrasladoInstalacionActivas As Boolean = False
    Dim logPermiteValorCeroRC As Boolean = False 'DEMC20180925

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
    End Enum

    'SLB20130228
    Private Enum TabsTesoreriaRC As Byte
        Tesoreria
        Cheques
    End Enum

    Private Enum ACCIONES_ESTADODOCUMENTO As Byte
        EDITAR
        BORRAR
    End Enum

#End Region

#Region "Inicializacion"

    Public Sub New()
    End Sub

    Public Sub inicializar(Optional ByVal plogConsultar As Boolean = True)
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext()
                dcProxy1 = New TesoreriaDomainContext()
                dcProxy2 = New TesoreriaDomainContext()
                dcProxy3 = New MaestrosDomainContext()
                dcProxy4 = New TesoreriaDomainContext()
                dcProxyTransaccion = New TesoreriaDomainContext()
                objProxy = New UtilidadesDomainContext()
                objProxy1 = New UtilidadesDomainContext()
                objProxyCarga = New ImportacionesDomainContext()
            Else
                dcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxy1 = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxy2 = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxy3 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioMaestros))
                dcProxy4 = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyTransaccion = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                objProxy1 = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                objProxyCarga = New ImportacionesDomainContext(New System.Uri((Program.RutaServicioImportaciones)))

            End If
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxyTransaccion.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(objProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If Not Program.IsDesignMode() Then
                moduloTesoreria = Application.Current.Resources("moduloTesoreria").ToString

                Application.Current.Resources.Remove("moduloTesoreria")
                If moduloTesoreria = "RC" Then
                    strNombreConsecutivo = "ConsecutivosTesoreriaRCComp"
                    strNombreConsecutivoTodos = "ConsecutivosTesoreriaRCTOD"
                ElseIf moduloTesoreria = "N" Then
                    strNombreConsecutivo = "ConsecutivosTesoreriaNotasComp"
                    strNombreConsecutivoTodos = "ConsecutivosTesoreriaNotasTOD"
                ElseIf moduloTesoreria = "CE" Then
                    strNombreConsecutivo = "NombreConsecutivoCEComp"
                    strNombreConsecutivoTodos = "NombreConsecutivoCETOD"
                End If

                '************************************ OYD.NET *********************************************
                IsBusy = True

                If plogConsultar Then
                    dcProxy.Load(dcProxy.TesoreriaFiltrarQuery("", moduloTesoreria, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "FiltroInicial")
                Else
                    MakeAndCheck = 0
                    ColumnasVisibles = Visibility.Collapsed
                    MyBase.CambioItem("ColumnasVisibles")
                    HabilitarImpresion = True
                    content = "aprobada"
                    visible = False
                    visibleap = False
                End If

                dcProxy1.Load(dcProxy1.TraerTesoreriPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreriaPorDefecto_Completed, "Default")

                If Not IsNothing(_TesoreriSelected) Then
                    ConsultarFechaCierreSistema()
                End If

                'Se ejecuta la operación encargada de consultar variables para validar que el cliente tenga saldo disponible en comprobantes de egreso.
                dcProxy.Load(dcProxy.TraerBolsaQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsa, Nothing)

                CargarCombosViewModel()
                ConsultarParametros()
                Make = True
            End If

            VersionAplicacionCliente = Program.VersionAplicacionCliente

            If VersionAplicacionCliente = EnumVersionAplicacionCliente.G.ToString Then 'Si es Genérico
                VisibleEspecificoCity = Visibility.Collapsed

            ElseIf VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then 'Si es City
                VisibleEspecificoCity = Visibility.Visible
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "TesoreriaViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerTesoreriaPorDefecto_Completed(ByVal lo As LoadOperation(Of Tesoreri))
        Try
            If Not lo.HasError Then
                TesoreriPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la tesorería por defecto",
                                                 Me.ToString(), "TerminoTraerTesoreriPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la tesorería por defecto",
                                 Me.ToString(), "TerminoTraerTesoreriaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerTesoreria(ByVal lo As LoadOperation(Of Tesoreri))
        Try
            If Not lo.HasError Then
                ListaTesoreria = dcProxy.Tesoreris
                If Not IsNothing(dcProxy.Tesoreris) Then
                    If dcProxy.Tesoreris.Count >= 0 Then
                        If Not IsNothing(TesoreriSelected) Then
                            If Not IsNothing(TesoreriSelected.MakeAndCheck) Then
                                MakeAndCheck = TesoreriSelected.MakeAndCheck
                            Else
                                MakeAndCheck = 0
                            End If

                            'Se incluye codigo para el manejo del Maker and Checker
                            If MakeAndCheck = 1 Then
                                'Se pregunta si tiene MakeAndCheck para ocultar las columnas de Aprobar que se encuentran en el Formulario JRP 13/07/2012
                                ColumnasVisibles = Visibility.Visible
                                MyBase.CambioItem("ColumnasVisibles")
                                HabilitarImpresion = False
                                If lo.UserState = Nothing Then
                                    TesoreriSelected = ListaTesoreria.Where(Function(ic) ic.IDDocumento = codigo).First
                                    CuentaContableCliente = TesoreriSelected.CuentaCliente
                                End If
                            Else
                                ColumnasVisibles = Visibility.Collapsed
                                MyBase.CambioItem("ColumnasVisibles")
                                HabilitarImpresion = True
                            End If

                            If lo.UserState = "insert" Then
                                If IsNothing(TesoreriSelected) Then
                                    TesoreriSelected = ListaTesoreria.First
                                End If

                                CuentaContableCliente = TesoreriSelected.CuentaCliente
                            End If

                            'Se incluye codigo para el manejo del Maker and Checker
                            If MakeAndCheck = 1 Then
                                If TesoreriSelected.Por_Aprobar <> Nothing And TesoreriSelected.Por_Aprobar <> "Rechazado" Then
                                    If Not (TesoreriSelected.EstadoMC Is Nothing) Then
                                        If TesoreriSelected.EstadoMC.Equals("Ingreso") Then
                                            visible = False
                                        Else
                                            visible = True
                                        End If
                                        visibleap = True
                                        content = "por aprobada"
                                    End If
                                Else
                                    content = "aprobada"
                                    If (TesoreriSelected.Por_Aprobar = Nothing Or TesoreriSelected.Por_Aprobar = "Rechazado") And lo.UserState <> Nothing Then
                                        visible = False
                                        visibleap = False
                                    End If
                                End If
                            Else
                                content = "aprobada"
                                If (TesoreriSelected.Por_Aprobar = Nothing Or TesoreriSelected.Por_Aprobar = "Rechazado") And lo.UserState <> Nothing Then
                                    visible = False
                                    visibleap = False
                                End If
                            End If
                            'SLB20130925 Se comenta ya que puede sacar error si el servidor esta muy lento
                            'If _ListConsecutivosConsola.Count = 0 Then
                            '    _ListConsecutivosConsola = CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(strNombreConsecutivo)
                            'End If
                            'listConsecutivos = _ListConsecutivosConsola
                        End If
                    Else
                        If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Then
                            A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            visNavegando = "Collapsed"
                            MyBase.CambioItem("visNavegando")
                            'MyBase.CambiarALista()
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Tesoreria",
                                                 Me.ToString(), "TerminoTraerTesoreri", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Tesoreria",
                                                             Me.ToString(), "TerminoTraerTesoreri", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Tablas padres
    Private Sub TerminoTraerDetalleTesoreria(ByVal lo As LoadOperation(Of DetalleTesoreri))
        Try
            If Not lo.HasError Then
                ListaDetalleTesoreria = dcProxy.DetalleTesoreris.ToList     'JFSB 20160926
                For Each li In ListaDetalleTesoreria
                    ListaDetalleTesoreriaAnt.Add(New secuenciadetalletesoreira With {.Secuencia = li.Secuencia})
                Next
                'En el procedimiento almacenado se esta devolviendo el valor positivo para 
                'mostrarlo en pantalla.

                'For Each ld In ListaDetalleTesoreria
                '    If Not IsNothing(ld.Credito) Then
                '        If ld.Credito < 0 Then
                '            ld.Credito = ld.Credito * -1
                '        End If
                '    End If
                'Next
                If logVentanaEmergente And logEsEdicionRegistroEmergente Then
                    logEsEdicionRegistroEmergente = False
                    EditarRegistro()
                End If

                If moduloTesoreria = "RC" Then
                    SumarTotales()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleTesoreria",
                                                 Me.ToString(), "TerminoTraerDetalleTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusyDetalles = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleTesoreria",
                                                             Me.ToString(), "TerminoTraerDetalleTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoTraerChequesTesoreria(ByVal lo As LoadOperation(Of Cheque))
        Try
            If Not lo.HasError Then
                ListaChequeTesoreria = dcProxy.Cheques.ToList
                For Each li In ListaChequeTesoreria
                    ListaChequeTesoreriaAnt.Add(New secuenciachequestesoreria With {.Secuencia = li.Secuencia})
                Next
                If moduloTesoreria = "RC" Then
                    SumarTotales()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Cheques",
                                                 Me.ToString(), "TerminoTraerChequesTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Cheques",
                                                            Me.ToString(), "TerminoTraerChequesTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoTraerObservacionesTesoreria(ByVal lo As LoadOperation(Of TesoreriaAdicionale))
        Try
            If Not lo.HasError Then
                ListaObservacionesTesoreria = dcProxy.TesoreriaAdicionales.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TesoreriaAdicionales",
                                                 Me.ToString(), "TerminoTraerObservacionesTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TesoreriaAdicionales",
                                                 Me.ToString(), "TerminoTraerObservacionesTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoTraerBolsa(ByVal lo As LoadOperation(Of TraerBolsa))
        Try
            If Not lo.HasError Then
                ListaTraerBolsas = dcProxy.TraerBolsas
                'SLB20130305 Esto solo se debe obtener una vez
                glogValSobregiroCE = ListaTraerBolsas.First.logValSobregiroCE
                glogValSobregiroNC = ListaTraerBolsas.First.logValSobregiroNC
                glogConceptoDetalleTesoreriaManual = ListaTraerBolsas.First.logConceptoDetalleTesoreriaManual
                'JFGB20160511
                If Not IsNothing(ListaTraerBolsas.First.lngIDCiaPrincipal) Then
                    intIDCompaniaFirma = ListaTraerBolsas.First.lngIDCiaPrincipal
                    strNroDocumentoCompaniaFirma = ListaTraerBolsas.First.strNumeroDocumentoCiaPrincipal
                    strNombreCortoCompaniaFirma = ListaTraerBolsas.First.strNombreCorto
                    strNombreCompaniaFirma = ListaTraerBolsas.First.strNombreCiaPrincipal
                End If

                dcProxy.Load(dcProxy.VerificarGMFQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoverificarGMF, Nothing)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TraerBolsa",
                                                 Me.ToString(), "TerminoTraerBolsa", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TraerBolsa",
                                                 Me.ToString(), "TerminoTraerBolsa", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoConsutarConsecutivoBanco(ByVal lo As LoadOperation(Of ConsecutivoBanco))
        Try
            If Not lo.HasError Then
                ListaConsecutivoBanco = dcProxy.ConsecutivoBancos
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Consecutivo Banco",
                                                 Me.ToString(), "TerminoConsutarConsecutivoBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Consecutivo Banco",
                                                 Me.ToString(), "TerminoConsutarConsecutivoBanco", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub ConsutarCuentaTransferenciaCompleted(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se realizó la verificación de que el cliente especificado tenga una cuenta con el indicativo logTransferirA.",
                                                             Me.ToString(), "ConsutarCuentaTransferenciaCompleted", Program.TituloSistema, Program.Maquina, lo.Error,
                                                           Program.RutaServicioLog)
            Else
                TransferenciaDisponible = lo.Value
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se realizó la Validación del saldo del cliente.",
                                                 Me.ToString(), "ConsutarCuentaTransferenciaCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub consultarFechaCierreCompaniaCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        Try
            If obj.HasError Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre de la compañia", Me.ToString(), "consultarFechaCierreCompaniaCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                fechaCierre = obj.Value

                If obj.UserState = "EDITAR" Then
                    ContinuarEdicionDocumento()
                ElseIf obj.UserState = "GUARDAR" Then
                    ContinuarGuardadoDocumento()
                ElseIf obj.UserState = "BORRAR" Then
                    ContinuarBorradoDocumento()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre de la compañia",
                                                 Me.ToString(), "consultarFechaCierreCompaniaCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        Try
            If obj.HasError Then
                If obj.UserState = "EDITAR" Then
                    MyBase.RetornarValorEdicionNavegacion()
                End If
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                fechaCierre = obj.Value

                If obj.UserState = "EDITAR" Then
                    ContinuarEdicionDocumento()
                ElseIf obj.UserState = "GUARDAR" Then
                    ContinuarGuardadoDocumento()
                ElseIf obj.UserState = "BORRAR" Then
                    ContinuarBorradoDocumento()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema",
                                                 Me.ToString(), "consultarFechaCierreCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Metodo para asincronico para recargar los combos de tipo de acuerdo a los permisos del usuario.
    ''' </summary>
    ''' <remarks>
    ''' Funcion          : TerminoCargarCombosEspecificos()
    ''' Creado por       : Juan David Correa Perez.
    ''' Descripción      : Llama al metodo de Nuevo(), Editar() o Borrar siempre y cuando el usuario tenga los permisos sobre los consecutivos. 
    ''' Fecha            : Agosto 16/2011 11:53 am
    ''' Pruebas CB       : Juan David Correa Perez- Agosto 16/2011 - Resultado Ok    
    ''' </remarks>
    Private Sub TerminoValidarEdicion(ByVal lo As LoadOperation(Of Tesoreri))
        Try
            If Not lo.HasError Then
                If MakeAndCheck = 1 Then
                    If dcProxy2.Tesoreris.Count > 0 Then
                        IsBusy = False
                        MyBase.RetornarValorEdicionNavegacion()
                        A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede editar porque tiene una versión pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        Editar()
                    End If
                Else
                    Editar()
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los documentos por aprobar.",
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los documentos por aprobar.",
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoatraerCuentasbancarias(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If objProxy1.ItemCombos.Count > 0 Then
                    'listCuentasbancarias = objProxy1.ItemCombos.Where(Function(y) y.Categoria = "CuentasBancarias").ToList
                    CuentaContableCliente = String.Empty

                    listCuentasbancarias.Clear()
                    For Each li In objProxy1.ItemCombos.Where(Function(y) y.Categoria = "CuentasBancarias").ToList
                        listCuentasbancarias.Add(li)
                    Next
                    'CORREC_CITI_SV_2014 - si la lista de cuentas trae solo una, esta se selecciona por defecto
                    If Editando = True And listCuentasbancarias.Count = 1 Then
                        'TesoreriSelected.CuentaCliente = listCuentasbancarias.First.ID
                        'JFSB 20171019
                        CuentaContableCliente = listCuentasbancarias.First.ID
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las cuentas bancarias.",
                                     Me.ToString(), "TerminoatraerCuentasbancarias", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar las cuentas bancarias.",
                                       Me.ToString(), "TerminoatraerCuentasbancarias", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoTraerConsecutivos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                Dim objListaConsecutivos As New List(Of OYDUtilidades.ItemCombo)
                If lo.UserState = "CONSECUTIVOSCOMPANIA" Then
                    objListaConsecutivos = objProxy.ItemCombos.Where(Function(y) y.Categoria = strNombreConsecutivo).ToList
                    If Not IsNothing(TesoreriSelected) Then
                        If Not IsNothing(_TesoreriSelected.IDCompania) Then
                            If Editando Then
                                TesoreriSelected.NombreConsecutivo = String.Empty
                            End If

                            listConsecutivos = objListaConsecutivos.Where(Function(i) i.Retorno = _TesoreriSelected.IDCompania.ToString).ToList

                            If Editando Then
                                If listConsecutivos.Count = 1 Then
                                    TesoreriSelected.NombreConsecutivo = listConsecutivos.First.ID
                                End If
                            End If
                        Else
                            listConsecutivos = Nothing
                            If Not IsNothing(_TesoreriSelected) Then
                                TesoreriSelected.NombreConsecutivo = Nothing
                            End If
                        End If
                    End If
                ElseIf lo.UserState = "EDITARREGISTRO" Then
                    objListaConsecutivos = objProxy.ItemCombos.Where(Function(y) y.Categoria = strNombreConsecutivo).ToList

                    If Not IsNothing(_TesoreriSelected) Then
                        Dim strConsecutivoSeleccionado As String = _TesoreriSelected.NombreConsecutivo
                        listConsecutivos = objListaConsecutivos
                        _TesoreriSelected.NombreConsecutivo = strConsecutivoSeleccionado
                    Else
                        listConsecutivos = objListaConsecutivos
                    End If


                Else
                    If objProxy.ItemCombos.Count > 0 Then
                        Dim hayConsecutivos As Boolean = False

                        objListaConsecutivos = objProxy.ItemCombos.Where(Function(y) y.Categoria = strNombreConsecutivo).ToList
                        If objListaConsecutivos.Count <> 0 Then
                            If lo.UserState <> "Nuevo" And lo.UserState <> "Duplicar" Then
                                For Each Consecutivo In objListaConsecutivos
                                    If Consecutivo.ID = TesoreriSelected.NombreConsecutivo Then
                                        hayConsecutivos = True
                                    End If
                                Next
                            Else
                                hayConsecutivos = True
                            End If
                        Else
                            hayConsecutivos = False
                        End If
                        If hayConsecutivos = True Then
                            If lo.UserState = "Nuevo" Then
                                dcProxy.Load(dcProxy.TraerBolsaQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsa, Nothing)
                                Nuevo()
                            ElseIf lo.UserState = "Editar" Then
                                ConsultarConsecutivosCompania("EDITARREGISTRO")
                                'SV20160203
                                ValidarEdicion()
                                dcProxy.Load(dcProxy.TraerBolsaQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsa, Nothing)
                            ElseIf lo.UserState = "Borrar" Then
                                Borrar()
                            ElseIf lo.UserState = "Duplicar" Then
                                'SV20160203
                                ConsultarConsecutivosCompania("EDITARREGISTRO")
                                dcProxy.Load(dcProxy.TraerBolsaQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsa, Nothing)
                                ContinuarDuplicarRegistro()
                            End If
                        Else
                            IsBusy = False
                            A2Utilidades.Mensajes.mostrarMensaje("Señor usuario, usted no posee permisos para realizar esta operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            'listConsecutivos = CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(strNombreConsecutivo)
                            listConsecutivos = _ListConsecutivosConsola
                        End If
                    Else
                        IsBusy = False
                        A2Utilidades.Mensajes.mostrarMensaje("Señor usuario, usted no posee permisos para realizar esta operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'listConsecutivos = CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(strNombreConsecutivo)
                        listConsecutivos = _ListConsecutivosConsola
                    End If
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los documentos por aprobar.",
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerConsecutivosBusqueda(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If Not IsNothing(CamposBusquedaTesoreria.IDCompania) Then
                    CamposBusquedaTesoreria.NombreConsecutivo = String.Empty
                    Dim objListaConsecutivos As New List(Of OYDUtilidades.ItemCombo)
                    Dim strTipoConsecutivo As String = String.Empty

                    If moduloTesoreria = "RC" Then
                        strTipoConsecutivo = "ConsecutivosTesoreriaRC"
                    ElseIf moduloTesoreria = "N" Then
                        strTipoConsecutivo = "ConsecutivosTesoreriaNotas"
                    ElseIf moduloTesoreria = "CE" Then
                        strTipoConsecutivo = "NombreConsecutivoCE"
                    End If

                    objListaConsecutivos = objProxy.ItemCombos.Where(Function(y) y.Categoria = strTipoConsecutivo).ToList

                    listConsecutivosBusqueda = objListaConsecutivos.Where(Function(i) i.Retorno = CamposBusquedaTesoreria.IDCompania.ToString).ToList

                    If listConsecutivosBusqueda.Count = 1 Then
                        CamposBusquedaTesoreria.NombreConsecutivo = listConsecutivosBusqueda.First.ID
                    End If
                Else
                    listConsecutivosBusqueda = Nothing
                    If Not IsNothing(CamposBusquedaTesoreria) Then
                        CamposBusquedaTesoreria.NombreConsecutivo = Nothing
                    End If
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los documentos por aprobar.",
                                     Me.ToString(), "TerminoTraerConsecutivosBusqueda", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivosBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo que carga la Configuración de los Consecutivos.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20140620</remarks>
    Private Sub TerminoTraerConfiguracionConsecutivos(ByVal lo As LoadOperation(Of ConfiguracionConsecutivos))
        If Not lo.HasError Then
            ListaConfiguracionConsecutivos = dcProxy.ConfiguracionConsecutivos.ToList

            objProxy.Verificaparametro("CF_UTILIZAPASIVA_A2", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "CF_UTILIZAPASIVA_A2")
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TraerConfiguracionConsecutivos.",
                                             Me.ToString(), "TerminoTraerConfiguracionConsecutivos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Propiedades"

#Region "Propiedades para presentación del ordenante"

    Private _Ordenantes As List(Of OYDUtilidades.BuscadorOrdenantes)
    Public Property Ordenantes As List(Of OYDUtilidades.BuscadorOrdenantes)
        Get
            Return (_Ordenantes)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorOrdenantes))
            _Ordenantes = value
            MyBase.CambioItem("Ordenantes")
        End Set
    End Property

    Private _mobjOrdenanteSeleccionado As OYDUtilidades.BuscadorOrdenantes
    Public Property OrdenanteSeleccionado() As OYDUtilidades.BuscadorOrdenantes
        Get
            Return (_mobjOrdenanteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorOrdenantes)
            _mobjOrdenanteSeleccionado = value
            MyBase.CambioItem("OrdenanteSeleccionado")
        End Set
    End Property

#End Region

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            Dim logIgual As Boolean = False

            If Not _ComitenteSeleccionado Is Nothing Then
                If _ComitenteSeleccionado.Equals(value) Then
                    logIgual = True
                End If
            Else
                Ordenantes = Nothing
                _ComitenteSeleccionado = value
                MyBase.CambioItem("ComitenteSeleccionado")
            End If
        End Set
    End Property

    Private _AnulandoOrden As New OrdenesTesoreria
    Public Property AnulandoOrden() As OrdenesTesoreria
        Get
            Return _AnulandoOrden
        End Get
        Set(ByVal value As OrdenesTesoreria)
            _AnulandoOrden = value
            MyBase.CambioItem("AnulandoOrden")
        End Set
    End Property

    Private _OrdenPendienteTesoreria As New OrdenesTesoreri

    'Se Crea esta Propiedad para Ocultar las Columnas dependiendo de si tienen o no MakeAndChecker JRP 13/07/2012
    Private _ColumnasVisibles As Visibility
    Public Property ColumnasVisibles() As Visibility
        Get
            Return _ColumnasVisibles
        End Get
        Set(ByVal value As Visibility)
            _ColumnasVisibles = value
            MyBase.CambioItem("ColumnasVisibles")
        End Set
    End Property

    Private _MostrarConcepto As Visibility = Visibility.Collapsed
    Public Property MostrarConcepto As Visibility
        Get
            Return _MostrarConcepto
        End Get
        Set(ByVal value As Visibility)
            _MostrarConcepto = value
            MyBase.CambioItem("MostrarConcepto")
        End Set
    End Property

    Private _VerValorCredito As Visibility = Visibility.Visible
    Public Property VerValorCredito As Visibility
        Get
            Return _VerValorCredito
        End Get
        Set(ByVal value As Visibility)
            _VerValorCredito = value
            MyBase.CambioItem("VerValorCredito")
        End Set
    End Property

    'JFGB20160511
    Private _MostrarConceptoFondosOYD As Visibility = Visibility.Collapsed
    Public Property MostrarConceptoFondosOYD() As Visibility
        Get
            Return _MostrarConceptoFondosOYD
        End Get
        Set(ByVal value As Visibility)
            _MostrarConceptoFondosOYD = value
            MyBase.CambioItem("MostrarConceptoFondosOYD")
        End Set
    End Property

    Private _HabilitarCreacionDetallesNotas As Boolean = False
    Public Property HabilitarCreacionDetallesNotas() As Boolean
        Get
            Return _HabilitarCreacionDetallesNotas
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCreacionDetallesNotas = value
            MyBase.CambioItem("HabilitarCreacionDetallesNotas")
        End Set
    End Property

    Private _ConceptoSeleccionadoDetalleNota As Nullable(Of Integer)
    Public Property ConceptoSeleccionadoDetalleNota() As Nullable(Of Integer)
        Get
            Return _ConceptoSeleccionadoDetalleNota
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ConceptoSeleccionadoDetalleNota = value
            MyBase.CambioItem("ConceptoSeleccionadoDetalleNota")
        End Set
    End Property

    Private _DetalleIngresadoDetalleNota As String
    Public Property DetalleIngresadoDetalleNota() As String
        Get
            Return _DetalleIngresadoDetalleNota
        End Get
        Set(ByVal value As String)
            _DetalleIngresadoDetalleNota = value
            MyBase.CambioItem("DetalleIngresadoDetalleNota")
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


    Private _EditarColDetalle As Boolean = False
    ''' <summary>
    ''' Propiedad para permitir digitar o inhabilitar el campo Detalle dependiendo del parametro glogConceptoDetalleTesoreriaManual
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>SLB20130711</remarks>
    Public Property EditarColDetalle As Boolean
        Get
            Return _EditarColDetalle
        End Get
        Set(ByVal value As Boolean)
            _EditarColDetalle = value
            MyBase.CambioItem("EditarColDetalle")
        End Set
    End Property

    Private _ListaTesoreria As EntitySet(Of Tesoreri)
    Public Property ListaTesoreria() As EntitySet(Of Tesoreri)
        Get
            Return _ListaTesoreria
        End Get
        Set(ByVal value As EntitySet(Of Tesoreri))
            _ListaTesoreria = value
            MyBase.CambioItem("ListaTesoreria")
            MyBase.CambioItem("ListaTesoreriaPaged")
            If Not IsNothing(value) Then
                If IsNothing(TesoreriAnterior) Then
                    If value.Count > 0 Then
                        If _ListaTesoreria.FirstOrDefault.Por_Aprobar = "Por Aprobar" Then
                            Filtro = 0
                        ElseIf _ListaTesoreria.FirstOrDefault.Por_Aprobar = "Rechazado" Then
                            Filtro = 2
                        Else
                            Filtro = 1
                        End If
                    End If
                    TesoreriSelected = _ListaTesoreria.FirstOrDefault
                    'JFSB 20171019
                    If Not IsNothing(TesoreriSelected) Then
                        CuentaContableCliente = TesoreriSelected.CuentaCliente
                    End If
                Else
                    If value.Count > 0 Then
                        If Not TesoreriAnterior.IDDocumento.Equals(_ListaTesoreria.FirstOrDefault.IDDocumento) Then
                            TesoreriSelected = _ListaTesoreria.FirstOrDefault
                            'JFSB 20171019
                            If Not IsNothing(TesoreriSelected) Then
                                CuentaContableCliente = TesoreriSelected.CuentaCliente
                            End If
                        Else
                            TesoreriSelected = TesoreriAnterior
                            'JFSB 20171019
                            If Not IsNothing(TesoreriSelected) Then
                                CuentaContableCliente = TesoreriSelected.CuentaCliente
                            End If
                        End If
                    End If

                    'TesoreriSelected = TesoreriAnterior
                End If
            End If
            'MyBase.CambioItem("ListaTesoreria")
            'MyBase.CambioItem("ListaTesoreriaPaged")
        End Set
    End Property
    Public ReadOnly Property ListaTesoreriaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTesoreria) Then
                Dim view = New PagedCollectionView(_ListaTesoreria)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Dim loDT As LoadOperation(Of DetalleTesoreri)
    Dim loCH As LoadOperation(Of Cheque)
    Dim loTA As LoadOperation(Of TesoreriaAdicionale)

    Private WithEvents _TesoreriSelected As Tesoreri
    Public Property TesoreriSelected() As Tesoreri
        Get
            Return _TesoreriSelected
        End Get
        Set(ByVal value As Tesoreri)
            If Not value Is Nothing Then
                _TesoreriSelected = value
                If esVersion = False Then
                    'If Not TesoreriSelected.IDDocumento = contador.IDDocumento And Not mlogDuplicarTesoreria Then
                    If Not mlogDuplicarTesoreria Then

                        If Not IsNothing(loDT) Then
                            If Not loDT.IsComplete Then
                                loDT.Cancel()
                            End If
                        End If

                        dcProxy.DetalleTesoreris.Clear()
                        If Not IsNothing(ListaDetalleTesoreriaAnt) Then
                            ListaDetalleTesoreriaAnt.Clear()
                        End If

                        If Not String.IsNullOrEmpty(TesoreriSelected.NombreConsecutivo) Then
                            IsBusyDetalles = True
                            loDT = dcProxy.Load(dcProxy.Traer_DetalleTesoreria_TesoreriQuery(Filtro, moduloTesoreria, TesoreriSelected.NombreConsecutivo, TesoreriSelected.IDDocumento, TesoreriSelected.EstadoMC,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleTesoreria, Nothing)
                        End If

                        If moduloTesoreria = ClasesTesoreria.RC.ToString Then
                            If Not IsNothing(loCH) Then
                                If Not loCH.IsComplete Then
                                    loCH.Cancel()
                                End If
                            End If

                            If Not IsNothing(loTA) Then
                                If Not loTA.IsComplete Then
                                    loTA.Cancel()
                                End If
                            End If

                            dcProxy.Cheques.Clear()
                            dcProxy.TesoreriaAdicionales.Clear()
                            ListaObservacionesTesoreria.Clear()
                            ListaDetalleTesoreriaAnt.Clear()
                            ListaChequeTesoreriaAnt.Clear()

                            loCH = dcProxy.Load(dcProxy.Traer_Cheques_TesoreriQuery(Filtro, TesoreriSelected.IDDocumento, TesoreriSelected.NombreConsecutivo, TesoreriSelected.EstadoMC, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerChequesTesoreria, Nothing)
                            loTA = dcProxy.Load(dcProxy.Traer_Observaciones_TesoreriQuery(Filtro, TesoreriSelected.IDDocumento, TesoreriSelected.NombreConsecutivo, TesoreriSelected.Tipo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerObservacionesTesoreria, Nothing)



                        ElseIf moduloTesoreria = ClasesTesoreria.CE.ToString Then
                            CuentaTrasferencia()

                            logCambiarCuentaBancaria = False
                            'JFSB 20180228 Se ajusta la asociacion de la cuenta bancaria del cliente
                            If String.IsNullOrEmpty(TesoreriSelected.CuentaCliente) Then
                                CuentaContableCliente = Nothing
                            Else
                                CuentaContableCliente = TesoreriSelected.CuentaCliente
                            End If
                            logCambiarCuentaBancaria = True

                            If TesoreriSelected.FormaPagoCE = "B" Then

                                MostarFormaEntrega = Visibility.Visible
                                MostarBeneficiario = Visibility.Visible
                                TipoIdentBeneficiario = Visibility.Visible
                                IdentificacionBeneficiario = Visibility.Visible
                                MostrarNombrePersonaRecoge = Visibility.Visible
                                MostrarIdentificacion = Visibility.Visible
                                MostrarOficina = Visibility.Visible

                                MostrarEntidad = Visibility.Collapsed
                                MostrarTipoCuenta = Visibility.Collapsed
                                MostrarNroCuenta = Visibility.Collapsed
                                MostrarTipoIdTitular = Visibility.Collapsed
                                MostrarIdTitular = Visibility.Collapsed
                                MostrarTitular = Visibility.Collapsed

                                MostrarCodCartera = Visibility.Collapsed
                                MostrarOficinaCuentaInversion = Visibility.Collapsed
                                MostrarNombreCarteraColectiva = Visibility.Collapsed

                            ElseIf TesoreriSelected.FormaPagoCE = "N" Then

                                MostrarEntidad = Visibility.Visible
                                MostrarTipoCuenta = Visibility.Visible
                                MostrarNroCuenta = Visibility.Visible
                                MostrarTipoIdTitular = Visibility.Visible
                                MostrarIdTitular = Visibility.Visible
                                MostrarTitular = Visibility.Visible

                                MostarFormaEntrega = Visibility.Collapsed
                                MostarBeneficiario = Visibility.Collapsed
                                TipoIdentBeneficiario = Visibility.Collapsed
                                IdentificacionBeneficiario = Visibility.Collapsed
                                MostrarNombrePersonaRecoge = Visibility.Collapsed
                                MostrarIdentificacion = Visibility.Collapsed
                                MostrarOficina = Visibility.Collapsed

                                MostrarCodCartera = Visibility.Collapsed
                                MostrarOficinaCuentaInversion = Visibility.Collapsed
                                MostrarNombreCarteraColectiva = Visibility.Collapsed

                            ElseIf TesoreriSelected.FormaPagoCE = "L" Then

                                MostrarEntidad = Visibility.Visible
                                MostrarTipoCuenta = Visibility.Visible
                                MostrarNroCuenta = Visibility.Visible
                                MostrarTipoIdTitular = Visibility.Visible
                                MostrarIdTitular = Visibility.Visible
                                MostrarTitular = Visibility.Visible
                                MostrarCodCartera = Visibility.Visible
                                MostrarOficinaCuentaInversion = Visibility.Visible
                                MostrarNombreCarteraColectiva = Visibility.Visible

                                MostarFormaEntrega = Visibility.Collapsed
                                MostarBeneficiario = Visibility.Collapsed
                                TipoIdentBeneficiario = Visibility.Collapsed
                                IdentificacionBeneficiario = Visibility.Collapsed
                                MostrarNombrePersonaRecoge = Visibility.Collapsed
                                MostrarIdentificacion = Visibility.Collapsed
                                MostrarOficina = Visibility.Collapsed

                            Else

                                MostarFormaEntrega = Visibility.Collapsed
                                MostarBeneficiario = Visibility.Collapsed
                                TipoIdentBeneficiario = Visibility.Collapsed
                                IdentificacionBeneficiario = Visibility.Collapsed
                                MostrarNombrePersonaRecoge = Visibility.Collapsed
                                MostrarIdentificacion = Visibility.Collapsed
                                MostrarOficina = Visibility.Collapsed

                                MostrarEntidad = Visibility.Collapsed
                                MostrarTipoCuenta = Visibility.Collapsed
                                MostrarNroCuenta = Visibility.Collapsed
                                MostrarTipoIdTitular = Visibility.Collapsed
                                MostrarIdTitular = Visibility.Collapsed
                                MostrarTitular = Visibility.Collapsed

                                MostrarCodCartera = Visibility.Collapsed
                                MostrarOficinaCuentaInversion = Visibility.Collapsed
                                MostrarNombreCarteraColectiva = Visibility.Collapsed

                            End If

                        End If

                        If Not (TesoreriSelected.EstadoMC Is Nothing) Then
                            If TesoreriSelected.EstadoMC.Equals("Ingreso") Then
                                visible = False
                            End If

                        End If
                        contador = value

                        'JFGB20160511
                        If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                            If plogEsFondosOYD Then
                                If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                                    MostrarConcepto = Visibility.Visible
                                    MostrarConceptoFondosOYD = Visibility.Collapsed
                                Else
                                    MostrarConcepto = Visibility.Collapsed
                                    MostrarConceptoFondosOYD = Visibility.Visible
                                End If
                            Else
                                MostrarConceptoFondosOYD = Visibility.Collapsed
                                If Not glogConceptoDetalleTesoreriaManual Then
                                    MostrarConcepto = Visibility.Visible
                                Else
                                    MostrarConcepto = Visibility.Collapsed
                                End If
                            End If
                        Else
                            MostrarConceptoFondosOYD = Visibility.Collapsed
                            If Not glogConceptoDetalleTesoreriaManual Then
                                MostrarConcepto = Visibility.Visible
                            Else
                                MostrarConcepto = Visibility.Collapsed
                            End If
                        End If
                    End If
                End If

                esVersion = False
            End If
            MyBase.CambioItem("TesoreriSelected")
            'MyBase.CambioItem("ColumnasVisibles")
        End Set
    End Property

    Private _visible As Boolean
    Public Property visible As Boolean
        Get
            Return _visible
        End Get
        Set(ByVal value As Boolean)
            _visible = value
            MyBase.CambioItem("visible")
        End Set
    End Property

    Private _visibilidad As Visibility = Visibility.Collapsed
    Public Property visibilidad As Visibility
        Get
            Return _visibilidad
        End Get
        Set(ByVal value As Visibility)
            _visibilidad = value
            MyBase.CambioItem("visibilidad")
        End Set
    End Property

    Private _visibilidadSubEstados As Visibility = Visibility.Collapsed
    Public Property visibilidadSubEstados As Visibility
        Get
            Return _visibilidadSubEstados
        End Get
        Set(ByVal value As Visibility)
            _visibilidadSubEstados = value
            MyBase.CambioItem("visibilidadSubEstados")
        End Set
    End Property
    Private _visibleap As Boolean
    Public Property visibleap As Boolean
        Get
            Return _visibleap
        End Get
        Set(ByVal value As Boolean)
            _visibleap = value
            MyBase.CambioItem("visibleap")
        End Set
    End Property
    Private _content As String
    Public Property content As String
        Get
            Return _content
        End Get
        Set(ByVal value As String)
            _content = value
            MyBase.CambioItem("content")
        End Set
    End Property
    Private _cmbNombreConsecutivoHabilitado As Boolean
    Public Property cmbNombreConsecutivoHabilitado() As Boolean
        Get
            Return _cmbNombreConsecutivoHabilitado
        End Get
        Set(ByVal value As Boolean)
            _cmbNombreConsecutivoHabilitado = value
            MyBase.CambioItem("cmbNombreConsecutivoHabilitado")
        End Set
    End Property

    'Propiedad para realizar la suma de todos los valores del detalle de Cheques
    Private _TotalCheques As Double
    Public Property TotalCheques() As Double
        Get
            Return _TotalCheques
        End Get
        Set(ByVal value As Double)
            _TotalCheques = value
            MyBase.CambioItem("TotalCheques")
        End Set
    End Property

    'Propiedad para realizar la suma de todos los valores del detalle de recibos
    Private _TotalRecibos As Double
    Public Property TotalRecibos() As Double
        Get
            Return _TotalRecibos
        End Get
        Set(ByVal value As Double)
            _TotalRecibos = value
            MyBase.CambioItem("TotalRecibos")
        End Set

    End Property

    'Propiedad para realizar la suma de todos los valores del detalle de los comprobantes
    Private _TotalComprobantes As Double
    Public Property TotalComprobantes() As Double
        Get
            Return _TotalComprobantes
        End Get
        Set(ByVal value As Double)
            _TotalComprobantes = value
            MyBase.CambioItem("TotalComprobantes")
        End Set

    End Property

    'Propiedad para deshabilitar los campos Nombre, NroDocumento y Tipo de Documento
    Private _HabilitarEdicion As Boolean
    Public Property HabilitarEdicion() As Boolean
        Get
            Return _HabilitarEdicion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicion = value
            MyBase.CambioItem("HabilitarEdicion")
        End Set
    End Property

    'Propiedad para poner de solo lectura los grid
    Private _Read As Boolean = True
    Public Property Read() As Boolean
        Get
            Return _Read
        End Get
        Set(ByVal value As Boolean)
            _Read = value
            MyBase.CambioItem("Read")
        End Set
    End Property

    'JFSB 20171019 Se agrega propiedad para el tipo de cuenta
    Private _CuentaContableCliente As String
    Public Property CuentaContableCliente() As String
        Get
            Return _CuentaContableCliente
        End Get
        Set(ByVal value As String)
            _CuentaContableCliente = value
            If logCambiarCuentaBancaria And Editando Then
                TesoreriSelected.CuentaCliente = _CuentaContableCliente
            End If
            MyBase.CambioItem("CuentaContableCliente")
        End Set
    End Property

    Private WithEvents _CamposBusquedaTesoreria As CamposBusquedaTesoreri = New CamposBusquedaTesoreri
    Public Property CamposBusquedaTesoreria() As CamposBusquedaTesoreri
        Get
            Return _CamposBusquedaTesoreria
        End Get
        Set(ByVal value As CamposBusquedaTesoreri)
            If Not value Is Nothing Then
                _CamposBusquedaTesoreria = value
                MyBase.CambioItem("CamposBusquedaTesoreria")
            End If
        End Set
    End Property

    Private _MostarBancoDestino As Visibility = Visibility.Collapsed
    Public Property MostarBancoDestino As Visibility
        Get
            Return _MostarBancoDestino
        End Get
        Set(ByVal value As Visibility)
            _MostarBancoDestino = value
            MyBase.CambioItem("MostarBancoDestino")
        End Set
    End Property

    Private _MostarFormaEntrega As Visibility = Visibility.Collapsed
    Public Property MostarFormaEntrega As Visibility
        Get
            Return _MostarFormaEntrega
        End Get
        Set(ByVal value As Visibility)
            _MostarFormaEntrega = value
            MyBase.CambioItem("MostarFormaEntrega")
        End Set
    End Property

    Private _MostrarNombrePersonaRecoge As Visibility = Visibility.Collapsed
    Public Property MostrarNombrePersonaRecoge As Visibility
        Get
            Return _MostrarNombrePersonaRecoge
        End Get
        Set(ByVal value As Visibility)
            _MostrarNombrePersonaRecoge = value
            MyBase.CambioItem("MostrarNombrePersonaRecoge")
        End Set
    End Property


    Private _MostarBeneficiario As Visibility = Visibility.Collapsed
    Public Property MostarBeneficiario As Visibility
        Get
            Return _MostarBeneficiario
        End Get
        Set(ByVal value As Visibility)
            _MostarBeneficiario = value
            MyBase.CambioItem("MostarBeneficiario")
        End Set
    End Property

    Private _IdentificacionBeneficiario As Visibility = Visibility.Collapsed
    Public Property IdentificacionBeneficiario As Visibility
        Get
            Return _IdentificacionBeneficiario
        End Get
        Set(ByVal value As Visibility)
            _IdentificacionBeneficiario = value
            MyBase.CambioItem("IdentificacionBeneficiario")
        End Set
    End Property

    Private _TipoIdentBeneficiario As Visibility = Visibility.Collapsed
    Public Property TipoIdentBeneficiario As Visibility
        Get
            Return _TipoIdentBeneficiario
        End Get
        Set(ByVal value As Visibility)
            _TipoIdentBeneficiario = value
            MyBase.CambioItem("TipoIdentBeneficiario")
        End Set
    End Property

    Private _MostrarIdentificacion As Visibility = Visibility.Collapsed
    Public Property MostrarIdentificacion As Visibility
        Get
            Return _MostrarIdentificacion
        End Get
        Set(ByVal value As Visibility)
            _MostrarIdentificacion = value
            MyBase.CambioItem("MostrarIdentificacion")
        End Set
    End Property
    Private _MostrarOficina As Visibility = Visibility.Collapsed
    Public Property MostrarOficina As Visibility
        Get
            Return _MostrarOficina
        End Get
        Set(ByVal value As Visibility)
            _MostrarOficina = value
            MyBase.CambioItem("MostrarOficina")
        End Set
    End Property

    Private _MostrarEntidad As Visibility = Visibility.Collapsed
    Public Property MostrarEntidad As Visibility
        Get
            Return _MostrarEntidad
        End Get
        Set(ByVal value As Visibility)
            _MostrarEntidad = value
            MyBase.CambioItem("MostrarEntidad")
        End Set
    End Property

    Private _MostrarTipoCuenta As Visibility = Visibility.Collapsed
    Public Property MostrarTipoCuenta As Visibility
        Get
            Return _MostrarTipoCuenta
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoCuenta = value
            MyBase.CambioItem("MostrarTipoCuenta")
        End Set
    End Property
    Private _MostrarNroCuenta As Visibility = Visibility.Collapsed
    Public Property MostrarNroCuenta As Visibility
        Get
            Return _MostrarNroCuenta
        End Get
        Set(ByVal value As Visibility)
            _MostrarNroCuenta = value
            MyBase.CambioItem("MostrarNroCuenta")
        End Set
    End Property
    Private _MostrarTipoIdTitular As Visibility = Visibility.Collapsed
    Public Property MostrarTipoIdTitular As Visibility
        Get
            Return _MostrarTipoIdTitular
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoIdTitular = value
            MyBase.CambioItem("MostrarTipoIdTitular")
        End Set
    End Property

    Private _MostrarIdTitular As Visibility = Visibility.Collapsed
    Public Property MostrarIdTitular As Visibility
        Get
            Return _MostrarIdTitular
        End Get
        Set(ByVal value As Visibility)
            _MostrarIdTitular = value
            MyBase.CambioItem("MostrarIdTitular")
        End Set
    End Property
    Private _MostrarTitular As Visibility = Visibility.Collapsed
    Public Property MostrarTitular As Visibility
        Get
            Return _MostrarTitular
        End Get
        Set(ByVal value As Visibility)
            _MostrarTitular = value
            MyBase.CambioItem("MostrarTitular")
        End Set
    End Property

    Private _MostrarCodCartera As Visibility = Visibility.Collapsed
    Public Property MostrarCodCartera As Visibility
        Get
            Return _MostrarCodCartera
        End Get
        Set(ByVal value As Visibility)
            _MostrarCodCartera = value
            MyBase.CambioItem("MostrarCodCartera")
        End Set
    End Property
    Private _MostrarOficinaCuentaInversion As Visibility = Visibility.Collapsed
    Public Property MostrarOficinaCuentaInversion As Visibility
        Get
            Return _MostrarOficinaCuentaInversion
        End Get
        Set(ByVal value As Visibility)
            _MostrarOficinaCuentaInversion = value
            MyBase.CambioItem("MostrarOficinaCuentaInversion")
        End Set
    End Property

    Private _MostrarNombreCarteraColectiva As Visibility = Visibility.Collapsed
    Public Property MostrarNombreCarteraColectiva As Visibility
        Get
            Return _MostrarNombreCarteraColectiva
        End Get
        Set(ByVal value As Visibility)
            _MostrarNombreCarteraColectiva = value
            MyBase.CambioItem("MostrarNombreCarteraColectiva")
        End Set
    End Property


    Private _MostrarBotonDuplicar As Visibility = Visibility.Collapsed
    Public Property MostrarBotonDuplicar As Visibility
        Get
            Return _MostrarBotonDuplicar
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotonDuplicar = value
            MyBase.CambioItem("MostrarBotonDuplicar")
        End Set
    End Property

    ''' <summary>
    ''' Permite controlar si el botón para duplicar una Tesoreria está o no activo
    ''' </summary>
    Private _ActivarDuplicarTesoreria As Boolean = True
    Public Property ActivarDuplicarTesoreria As Boolean
        Get
            Return (_ActivarDuplicarTesoreria)
        End Get
        Set(ByVal value As Boolean)
            _ActivarDuplicarTesoreria = value
            MyBase.CambioItem("ActivarDuplicarTesoreria")
        End Set
    End Property

    'Esta Propiedad es para Ocultar los botones de las operaciones de Ordenes Tesorería  JRP 13/07/2012
    Private _BotonesOrdenes As Boolean = False
    Public Property BotonesOrdenes() As Boolean
        Get
            Return _BotonesOrdenes
        End Get
        Set(ByVal value As Boolean)
            _BotonesOrdenes = value
            MyBase.CambioItem("BotonesOrdenes")
        End Set
    End Property


    ''' <summary>
    ''' Permite controlar si el nombre del consecutivo está o no activo
    ''' </summary>
    Private _nombreConsecutivovisible As Boolean = True
    Public Property nombreConsecutivovisible As Boolean
        Get
            Return (_nombreConsecutivovisible)
        End Get
        Set(ByVal value As Boolean)
            _nombreConsecutivovisible = value
            MyBase.CambioItem("nombreConsecutivovisible")
        End Set
    End Property
    Private _DeshabilitarSucursal As Boolean = False
    Public Property DeshabilitarSucursal() As Boolean
        Get
            Return _DeshabilitarSucursal
        End Get
        Set(ByVal value As Boolean)
            _DeshabilitarSucursal = value
            MyBase.CambioItem("DeshabilitarSucursal")
        End Set
    End Property
    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")
        End Set
    End Property
    Private _ListaTraerBolsas As EntitySet(Of TraerBolsa)
    Public Property ListaTraerBolsas() As EntitySet(Of TraerBolsa)
        Get
            Return _ListaTraerBolsas
        End Get
        Set(ByVal value As EntitySet(Of TraerBolsa))
            _ListaTraerBolsas = value
            MyBase.CambioItem("ListaTraerBolsas")
        End Set
    End Property
    Private _ListaConsecutivoBanco As EntitySet(Of ConsecutivoBanco)
    Public Property ListaConsecutivoBanco() As EntitySet(Of ConsecutivoBanco)
        Get
            Return _ListaConsecutivoBanco
        End Get
        Set(ByVal value As EntitySet(Of ConsecutivoBanco))
            _ListaConsecutivoBanco = value
            MyBase.CambioItem("ListaConsecutivoBanco")
        End Set
    End Property
    Private _VisibilidadNumCheque As Boolean = False
    Public Property VisibilidadNumCheque As Boolean
        Get
            Return _VisibilidadNumCheque
        End Get
        Set(ByVal value As Boolean)
            _VisibilidadNumCheque = value
            MyBase.CambioItem("VisibilidadNumCheque")
        End Set
    End Property

    Private _ListConsecutivosConsola As New List(Of OYDUtilidades.ItemCombo)
    'Propiedad para cargar los Consecutivos de los usuarios.
    Private _listConsecutivos As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property listConsecutivos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listConsecutivos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listConsecutivos = value
            MyBase.CambioItem("listConsecutivos")
        End Set
    End Property

    Private _listConsecutivosBusqueda As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property listConsecutivosBusqueda() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listConsecutivosBusqueda
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listConsecutivosBusqueda = value
            MyBase.CambioItem("listConsecutivosBusqueda")
        End Set
    End Property

    Private _VisibleEspecificoCity As Visibility = Visibility.Collapsed
    Public Property VisibleEspecificoCity As Visibility
        Get
            Return _VisibleEspecificoCity
        End Get
        Set(value As Visibility)
            _VisibleEspecificoCity = value
            MyBase.CambioItem("VisibleEspecificoCity")
        End Set
    End Property

    'Propiedad para cargar las cuentas bancarias
    Private _listCuentasbancarias As ObservableCollection(Of OYDUtilidades.ItemCombo) = New ObservableCollection(Of OYDUtilidades.ItemCombo)
    Public Property listCuentasbancarias() As ObservableCollection(Of OYDUtilidades.ItemCombo)
        Get
            Return _listCuentasbancarias
        End Get
        Set(ByVal value As ObservableCollection(Of OYDUtilidades.ItemCombo))
            _listCuentasbancarias = value
            MyBase.CambioItem("listCuentasbancarias")
        End Set
    End Property

    Private _TabSeleccionadoGeneral As Integer = 0
    ''' <summary>
    ''' SLB20130228 Propiedad para controlar el tab activo del tab control que contiene los datos generales de la Tesoreria
    ''' </summary>
    Public Property TabSeleccionadoGeneral As Integer
        Get
            Return _TabSeleccionadoGeneral
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionadoGeneral = value
            MyBase.CambioItem("TabSeleccionadoGeneral")
        End Set
    End Property

    Private _EditandoRegistro As Boolean = False
    Public Property EditandoRegistro() As Boolean
        Get
            Return _EditandoRegistro
        End Get
        Set(ByVal value As Boolean)
            _EditandoRegistro = value
            MyBase.CambioItem("EditandoRegistro")
        End Set
    End Property

    'CORREC_CITI_SV_2014
    Private _EditandoClienteC As Boolean = False
    Public Property EditandoClienteC() As Boolean
        Get
            Return _EditandoClienteC
        End Get
        Set(ByVal value As Boolean)
            _EditandoClienteC = value
            MyBase.CambioItem("EditandoClienteC")
        End Set
    End Property

    Private _HabilitarRCImpreso As Boolean = False
    Public Property HabilitarRCImpreso() As Boolean
        Get
            Return _HabilitarRCImpreso
        End Get
        Set(ByVal value As Boolean)
            _HabilitarRCImpreso = value
            MyBase.CambioItem("HabilitarRCImpreso")
        End Set
    End Property

    Private _HabilitarImpresion As Boolean = False
    Public Property HabilitarImpresion As Boolean
        Get
            Return _HabilitarImpresion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarImpresion = value
            MyBase.CambioItem("HabilitarImpresion")
        End Set
    End Property

    Private _HabilitaTipocheque As Boolean
    Public Property HabilitaTipocheque As Boolean
        Get
            Return _HabilitaTipocheque
        End Get
        Set(ByVal value As Boolean)
            _HabilitaTipocheque = value
            MyBase.CambioItem("HabilitaTipocheque")
        End Set
    End Property
    Private _FechaActual As DateTime = Now.Date
    Public Property FechaActual As DateTime
        Get
            Return _FechaActual
        End Get
        Set(ByVal value As DateTime)
            _FechaActual = value
            MyBase.CambioItem("FechaActual")
        End Set
    End Property

    ' JFSB 20160810 Se crea propiedad a la que se va a hacer referencia en el nombre de la columna valor en los maestros de comprobantes de egreso y recibo de caja
    Private Property _NombreColumnaValor As String
    Public Property NombreColumnaValor As String
        Get
            Return _NombreColumnaValor
        End Get
        Set(ByVal value As String)
            _NombreColumnaValor = value
            MyBase.CambioItem("NombreColumnaValor")
        End Set
    End Property

    'JFSB 20160817 Se crea propiedad para mostrar los campos número de intermedia e instrucción
    Private _VerCamposIntermedia As Visibility = Visibility.Collapsed
    Public Property VerCamposIntermedia As Visibility
        Get
            Return _VerCamposIntermedia
        End Get
        Set(value As Visibility)
            _VerCamposIntermedia = value
            MyBase.CambioItem("VerCamposIntermedia")
        End Set
    End Property

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

    Private _CantidadDecimalesPantalla As String = "2"
    Public Property CantidadDecimalesPantalla() As String
        Get
            Return _CantidadDecimalesPantalla
        End Get
        Set(ByVal value As String)
            _CantidadDecimalesPantalla = value
            MyBase.CambioItem("CantidadDecimalesPantalla")
        End Set
    End Property


#End Region

#Region "Métodos"

#Region "Métodos para controlar cambio de campos asociados a buscadores"
    Public Sub actualizarComitenteOrden(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Me.ComitenteSeleccionado = pobjComitente
    End Sub
#End Region

    ''' <summary>
    ''' Metodo para validar que tipos de consecutivos estan permitidos para el usuario.
    ''' </summary>
    ''' <remarks>
    ''' Funcion          : ValidarUsuario()
    ''' Se encarga de    : Carga los combos por usuario.
    ''' Modificado por   : Juan David Correa Perez.
    ''' Descripción      : Se incluye codigo para cargar los tipos de consecutivos permitidos para el usuario.
    ''' Fecha            : Agosto 16/2011 2:40 pm
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Agosto 16/2011 - Resultado Ok    
    ''' </remarks>
    Public Sub ValidarUsuario(ByVal pstrClaseCombos As String, ByVal pstrTipo As String)
        Try
            '// Ejecutar la carga de los combos y luego valida los datos con el metodo de ValidarConsecutivos
            If Not IsNothing(objProxy.ItemCombos) Then
                objProxy.ItemCombos.Clear()
            End If
            objProxy.Load(objProxy.cargarCombosEspecificosQuery(pstrClaseCombos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, pstrTipo)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar al consultar los consecutivos.", Me.ToString(), "ValidarUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ConsultarConsecutivosCompania(Optional ByVal pstrUserState As String = "CONSECUTIVOSCOMPANIA")
        Try
            '// Ejecutar la carga de los combos y luego valida los datos con el metodo de ValidarConsecutivos
            Dim pstrClaseCombos As String = String.Empty
            If moduloTesoreria = "N" Then
                pstrClaseCombos = "Tesoreria_Notas"
            ElseIf moduloTesoreria = "CE" Then
                pstrClaseCombos = "Tesoreria_ComprobantesEgreso"
            ElseIf moduloTesoreria = "RC" Then
                pstrClaseCombos = "Tesoreria_RecibosCaja"
            End If

            If Not IsNothing(objProxy.ItemCombos) Then
                objProxy.ItemCombos.Clear()
            End If
            objProxy.Load(objProxy.cargarCombosEspecificosQuery(pstrClaseCombos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar al consultar los consecutivos.", Me.ToString(), "ConsultarConsecutivosCompania", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ConsultarConsecutivosCompaniaBusqueda(Optional ByVal pstrUserState As String = "CONSECUTIVOSCOMPANIA")
        Try
            '// Ejecutar la carga de los combos y luego valida los datos con el metodo de ValidarConsecutivos
            Dim pstrClaseCombos As String = String.Empty
            If moduloTesoreria = "N" Then
                pstrClaseCombos = "Tesoreria_Notas"
            ElseIf moduloTesoreria = "CE" Then
                pstrClaseCombos = "Tesoreria_ComprobantesEgreso"
            ElseIf moduloTesoreria = "RC" Then
                pstrClaseCombos = "Tesoreria_RecibosCaja"
            End If

            If Not IsNothing(objProxy.ItemCombos) Then
                objProxy.ItemCombos.Clear()
            End If
            objProxy.Load(objProxy.cargarCombosEspecificosQuery(pstrClaseCombos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosBusqueda, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar al consultar los consecutivos.", Me.ToString(), "ConsultarConsecutivosCompaniaBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub LimpiarConsecutivoCompania()
        Try
            If Not IsNothing(_TesoreriSelected) Then
                _TesoreriSelected.NombreConsecutivo = String.Empty
            End If
            listConsecutivos = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar al limpiar la compañia.", Me.ToString(), "LimpiarConsecutivoCompania", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub LimpiarConsecutivoCompaniaBusqueda()
        Try
            If Not IsNothing(CamposBusquedaTesoreria) Then
                CamposBusquedaTesoreria.NombreConsecutivo = String.Empty
            End If
            listConsecutivosBusqueda = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar al limpiar la compañia.", Me.ToString(), "LimpiarConsecutivoCompaniaBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            dcProxy.Tesoreris.Clear()
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.TesoreriaFiltrarQuery(TextoFiltroSeguro, moduloTesoreria, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.TesoreriaFiltrarQuery("", moduloTesoreria, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub RefrescarTesoreria()
        Try
            If Not IsNothing(_TesoreriSelected) Then
                CamposBusquedaTesoreria = New CamposBusquedaTesoreri()
                CamposBusquedaTesoreria.Tipo = _TesoreriSelected.Tipo
                CamposBusquedaTesoreria.NombreConsecutivo = _TesoreriSelected.NombreConsecutivo
                CamposBusquedaTesoreria.IDDocumento = _TesoreriSelected.IDDocumento

                'If RegistroActivoPorAprobar Then
                '    cb.EstadoMakerChecker = EstadoMakerChecker.PA.ToString()
                'Else
                '    cb.EstadoMakerChecker = EstadoMakerChecker.A.ToString()
                'End If

                ConfirmarBuscar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de actualización de la orden", Me.ToString(), "refrescarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            If MakeAndCheck = 1 Then
                visibilidad = Visibility.Visible
            Else
                visibilidad = Visibility.Collapsed

            End If

            CamposBusquedaTesoreria = New CamposBusquedaTesoreri

            'If moduloTesoreria = "RC" Then
            '    CamposBusquedaTesoreria.NombreConsecutivo = TesoreriSelected.NombreConsecutivo
            'End If
            CamposBusquedaTesoreria.IDCompania = intIDCompaniaFirma
            CamposBusquedaTesoreria.NombreCompania = strNombreCompaniaFirma
            ConsultarConsecutivosCompaniaBusqueda()

            CamposBusquedaTesoreria.Aprobados = 1
            If Not IsNothing(_TesoreriSelected) Then
                CamposBusquedaTesoreria.Tipo = TesoreriSelected.Tipo
            Else
                CamposBusquedaTesoreria.Tipo = moduloTesoreria
            End If
            CamposBusquedaTesoreria.DisplayDate = Now.Date
            CamposBusquedaTesoreria.IDDocumento = Nothing
            CamposBusquedaTesoreria.Documento = Nothing

            'cb.Filtro = 1
            'cb.Tipo = TesoreriSelected.Tipo
            'cb.NombreConsecutivo = TesoreriSelected.NombreConsecutivo
            'cb.DisplayDate = Now
            'cb.Documento = Now

            visibilidadSubEstados = Visibility.Collapsed

            MyBase.Buscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Buscar los registros",
                                 Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            'If cb.Tipo <> 0 Or cb.NombreConsecutivo <> 0 Or cb.IDDocumento <> 0 Or cb.TipoIdentificacion <> 0 Or cb.NroDocumento <> 0 Or cb.Nombre <> 0 Or cb.IDBanco <> 0 Or cb.NumCheque <> 0 Or cb.Valor <> 0 Or cb.Detalle <> 0 Or Not IsNothing(cb.Documento) Or cb.Estado <> 0 Or cb.Estado <> 0 Or cb.Impresiones <> 0 Or cb.FormaPagoCE <> 0 Or cb.NroLote <> 0 Or cb.Contabilidad <> 0 Or Not IsNothing(cb.Actualizacion) Or cb.Usuario <> 0 Or cb.ParametroContable <> 0 Or cb.ImpresionFisica <> 0 Or cb.MultiCliente <> 0 Or cb.CuentaCliente <> 0 Or cb.CodComitente <> 0 Or cb.ArchivoTransferencia <> 0 Or cb.IdNumInst <> 0 Or cb.DVP <> 0 Or cb.Instruccion <> 0 Or cb.IdNroOrden <> 0 Or cb.DetalleInstruccion <> 0 Or cb.EstadoNovedadContabilidad <> 0 Or cb.eroComprobante_Contabilidad <> 0 Or cb.hadecontabilizacion_Contabilidad <> 0 Or Not IsNothing(cb.haProceso_Contabilidad) Or cb.EstadoNovedadContabilidad_Anulada <> 0 Or cb.EstadoAutomatico <> 0 Or cb.eroLote_Contabilidad <> 0 Or cb.MontoEscrito <> 0 Or cb.TipoIntermedia <> 0 Or cb.Concepto <> 0 Or cb.RecaudoDirecto <> 0 Or Not IsNothing(cb.ContabilidadEncuenta) Or cb.Sobregiro <> 0 Or cb.IdentificacionAutorizadoCheque <> 0 Or cb.NombreAutorizadoCheque <> 0 Or cb.IDTesoreria <> 0 Or Not IsNothing(cb.ContabilidadENC) Or cb.NroLoteAntENC <> 0 Or Not IsNothing(cb.ContabilidadAntENC) Or cb.IdSucursalBancaria <> 0 Or Not IsNothing(cb.Creacion) <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
            If Not IsNothing(CamposBusquedaTesoreria.Tipo) Or
                Not IsNothing(CamposBusquedaTesoreria.NombreConsecutivo) Or
                CamposBusquedaTesoreria.IDDocumento <> 0 Or
                Not IsNothing(CamposBusquedaTesoreria.Documento) Or
                CamposBusquedaTesoreria.Aprobados <> 0 Or
                CamposBusquedaTesoreria.NoAprobados <> 0 Or
                CamposBusquedaTesoreria.PorAprobar <> 0 Then

                If CamposBusquedaTesoreria.Aprobados <> 0 Then
                    Filtro = 1
                ElseIf CamposBusquedaTesoreria.NoAprobados <> 0 Then
                    Filtro = 2
                    If CamposBusquedaTesoreria.Ingreso <> 0 Then
                        estadoMC = "Ingreso"
                    ElseIf CamposBusquedaTesoreria.Modificacion <> 0 Then
                        estadoMC = "Modificacion"
                    ElseIf CamposBusquedaTesoreria.Retiro <> 0 Then
                        estadoMC = "Retiro"
                    ElseIf CamposBusquedaTesoreria.Todos <> 0 Then
                        estadoMC = "Todos"
                    End If
                ElseIf CamposBusquedaTesoreria.PorAprobar <> 0 Then
                    Filtro = 0
                    If CamposBusquedaTesoreria.Ingreso <> 0 Then
                        estadoMC = "Ingreso"
                    ElseIf CamposBusquedaTesoreria.Modificacion <> 0 Then
                        estadoMC = "Modificacion"
                    ElseIf CamposBusquedaTesoreria.Retiro <> 0 Then
                        estadoMC = "Retiro"
                    ElseIf CamposBusquedaTesoreria.Todos <> 0 Then
                        estadoMC = "Todos"
                    End If
                End If

                ErrorForma = ""
                dcProxy.Tesoreris.Clear()
                TesoreriAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Tipo = " & cb.Tipo.ToString() & " NombreConsecutivo = " & cb.NombreConsecutivo.ToString() & " IDDocumento = " & cb.IDDocumento.ToString() & " TipoIdentificacion = " & cb.TipoIdentificacion.ToString() & " NroDocumento = " & cb.NroDocumento.ToString() & " Nombre = " & cb.Nombre.ToString() & " IDBanco = " & cb.IDBanco.ToString() & " NumCheque = " & cb.NumCheque.ToString() & " Valor = " & cb.Valor.ToString() & " Detalle = " & cb.Detalle.ToString() & " Documento = " & cb.Documento.ToString() & " Estado = " & cb.Estado.ToString() & " Estado = " & cb.Estado.ToString() & " Impresiones = " & cb.Impresiones.ToString() & " FormaPagoCE = " & cb.FormaPagoCE.ToString() & " NroLote = " & cb.NroLote.ToString() & " Contabilidad = " & cb.Contabilidad.ToString() & " Actualizacion = " & cb.Actualizacion.ToString() & " Usuario = " & cb.Usuario.ToString() & " ParametroContable = " & cb.ParametroContable.ToString() & " ImpresionFisica = " & cb.ImpresionFisica.ToString() & " MultiCliente = " & cb.MultiCliente.ToString() & " CuentaCliente = " & cb.CuentaCliente.ToString() & " CodComitente = " & cb.CodComitente.ToString() & " ArchivoTransferencia = " & cb.ArchivoTransferencia.ToString() & " IdNumInst = " & cb.IdNumInst.ToString() & " DVP = " & cb.DVP.ToString() & " Instruccion = " & cb.Instruccion.ToString() & " IdNroOrden = " & cb.IdNroOrden.ToString() & " DetalleInstruccion = " & cb.DetalleInstruccion.ToString() & " EstadoNovedadContabilidad = " & cb.EstadoNovedadContabilidad.ToString() & " eroComprobante_Contabilidad = " & cb.eroComprobante_Contabilidad.ToString() & " hadecontabilizacion_Contabilidad = " & cb.hadecontabilizacion_Contabilidad.ToString() & " haProceso_Contabilidad = " & cb.haProceso_Contabilidad.ToString() & " EstadoNovedadContabilidad_Anulada = " & cb.EstadoNovedadContabilidad_Anulada.ToString() & " EstadoAutomatico = " & cb.EstadoAutomatico.ToString() & " eroLote_Contabilidad = " & cb.eroLote_Contabilidad.ToString() & " MontoEscrito = " & cb.MontoEscrito.ToString() & " TipoIntermedia = " & cb.TipoIntermedia.ToString() & " Concepto = " & cb.Concepto.ToString() & " RecaudoDirecto = " & cb.RecaudoDirecto.ToString() & " ContabilidadEncuenta = " & cb.ContabilidadEncuenta.ToString() & " Sobregiro = " & cb.Sobregiro.ToString() & " IdentificacionAutorizadoCheque = " & cb.IdentificacionAutorizadoCheque.ToString() & " NombreAutorizadoCheque = " & cb.NombreAutorizadoCheque.ToString() & " IDTesoreria = " & cb.IDTesoreria.ToString() & " ContabilidadENC = " & cb.ContabilidadENC.ToString() & " NroLoteAntENC = " & cb.NroLoteAntENC.ToString() & " ContabilidadAntENC = " & cb.ContabilidadAntENC.ToString() & " IdSucursalBancaria = " & cb.IdSucursalBancaria.ToString() & " Creacion = " & cb.Creacion.ToString()
                'dcProxy.Load(dcProxy.TesoreriaConsultarQuery(Filtro, CamposBusquedaTesoreria.Tipo, CamposBusquedaTesoreria.NombreConsecutivo, CamposBusquedaTesoreria.IDDocumento, IIf(CamposBusquedaTesoreria.Documento.Value.Year < 1900, Nothing, CamposBusquedaTesoreria.Documento.Value), estadoMC, CamposBusquedaTesoreria.IDBanco,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "Busqueda")
                dcProxy.Load(dcProxy.TesoreriaConsultarQuery(Filtro, CamposBusquedaTesoreria.Tipo, CamposBusquedaTesoreria.NombreConsecutivo, CamposBusquedaTesoreria.IDDocumento, CamposBusquedaTesoreria.Documento, estadoMC, CamposBusquedaTesoreria.IDBanco, CamposBusquedaTesoreria.IDCompania, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "Busqueda")
                MyBase.ConfirmarBuscar()
                CamposBusquedaTesoreria = New CamposBusquedaTesoreri
                CambioItem("CamposBusquedaTesoreria")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConfirmarBuscarEnter(ByVal NombreConsecutivo As String, ByVal IDDocumento As Integer, ByVal Documento As Date?,
                                    ByVal Aprobados As Integer, ByVal NoAprobados As Integer, ByVal PorAprobar As Integer, ByVal Ingreso As Integer,
                                    ByVal Modificacion As Integer, ByVal Retiro As Integer, ByVal Todos As Integer, ByVal IDBanco As Integer)
        Try
            'If cb.Tipo <> 0 Or cb.NombreConsecutivo <> 0 Or cb.IDDocumento <> 0 Or cb.TipoIdentificacion <> 0 Or cb.NroDocumento <> 0 Or cb.Nombre <> 0 Or cb.IDBanco <> 0 Or cb.NumCheque <> 0 Or cb.Valor <> 0 Or cb.Detalle <> 0 Or Not IsNothing(cb.Documento) Or cb.Estado <> 0 Or cb.Estado <> 0 Or cb.Impresiones <> 0 Or cb.FormaPagoCE <> 0 Or cb.NroLote <> 0 Or cb.Contabilidad <> 0 Or Not IsNothing(cb.Actualizacion) Or cb.Usuario <> 0 Or cb.ParametroContable <> 0 Or cb.ImpresionFisica <> 0 Or cb.MultiCliente <> 0 Or cb.CuentaCliente <> 0 Or cb.CodComitente <> 0 Or cb.ArchivoTransferencia <> 0 Or cb.IdNumInst <> 0 Or cb.DVP <> 0 Or cb.Instruccion <> 0 Or cb.IdNroOrden <> 0 Or cb.DetalleInstruccion <> 0 Or cb.EstadoNovedadContabilidad <> 0 Or cb.eroComprobante_Contabilidad <> 0 Or cb.hadecontabilizacion_Contabilidad <> 0 Or Not IsNothing(cb.haProceso_Contabilidad) Or cb.EstadoNovedadContabilidad_Anulada <> 0 Or cb.EstadoAutomatico <> 0 Or cb.eroLote_Contabilidad <> 0 Or cb.MontoEscrito <> 0 Or cb.TipoIntermedia <> 0 Or cb.Concepto <> 0 Or cb.RecaudoDirecto <> 0 Or Not IsNothing(cb.ContabilidadEncuenta) Or cb.Sobregiro <> 0 Or cb.IdentificacionAutorizadoCheque <> 0 Or cb.NombreAutorizadoCheque <> 0 Or cb.IDTesoreria <> 0 Or Not IsNothing(cb.ContabilidadENC) Or cb.NroLoteAntENC <> 0 Or Not IsNothing(cb.ContabilidadAntENC) Or cb.IdSucursalBancaria <> 0 Or Not IsNothing(cb.Creacion) <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
            If Not IsNothing(CamposBusquedaTesoreria.Tipo) Or
                Not IsNothing(NombreConsecutivo) Or
                IDDocumento <> 0 Or
                Not IsNothing(Documento) Or
                Aprobados <> 0 Or
                NoAprobados <> 0 Or
                PorAprobar <> 0 Then

                If Aprobados <> 0 Then
                    Filtro = 1
                ElseIf NoAprobados <> 0 Then
                    Filtro = 2
                    If Ingreso <> 0 Then
                        estadoMC = "Ingreso"
                    ElseIf Modificacion <> 0 Then
                        estadoMC = "Modificacion"
                    ElseIf Retiro <> 0 Then
                        estadoMC = "Retiro"
                    ElseIf Todos <> 0 Then
                        estadoMC = "Todos"
                    End If
                ElseIf PorAprobar <> 0 Then
                    Filtro = 0
                    If Ingreso <> 0 Then
                        estadoMC = "Ingreso"
                    ElseIf Modificacion <> 0 Then
                        estadoMC = "Modificacion"
                    ElseIf Retiro <> 0 Then
                        estadoMC = "Retiro"
                    ElseIf Todos <> 0 Then
                        estadoMC = "Todos"
                    End If
                End If

                ErrorForma = ""
                dcProxy.Tesoreris.Clear()
                TesoreriAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Tipo = " & cb.Tipo.ToString() & " NombreConsecutivo = " & cb.NombreConsecutivo.ToString() & " IDDocumento = " & cb.IDDocumento.ToString() & " TipoIdentificacion = " & cb.TipoIdentificacion.ToString() & " NroDocumento = " & cb.NroDocumento.ToString() & " Nombre = " & cb.Nombre.ToString() & " IDBanco = " & cb.IDBanco.ToString() & " NumCheque = " & cb.NumCheque.ToString() & " Valor = " & cb.Valor.ToString() & " Detalle = " & cb.Detalle.ToString() & " Documento = " & cb.Documento.ToString() & " Estado = " & cb.Estado.ToString() & " Estado = " & cb.Estado.ToString() & " Impresiones = " & cb.Impresiones.ToString() & " FormaPagoCE = " & cb.FormaPagoCE.ToString() & " NroLote = " & cb.NroLote.ToString() & " Contabilidad = " & cb.Contabilidad.ToString() & " Actualizacion = " & cb.Actualizacion.ToString() & " Usuario = " & cb.Usuario.ToString() & " ParametroContable = " & cb.ParametroContable.ToString() & " ImpresionFisica = " & cb.ImpresionFisica.ToString() & " MultiCliente = " & cb.MultiCliente.ToString() & " CuentaCliente = " & cb.CuentaCliente.ToString() & " CodComitente = " & cb.CodComitente.ToString() & " ArchivoTransferencia = " & cb.ArchivoTransferencia.ToString() & " IdNumInst = " & cb.IdNumInst.ToString() & " DVP = " & cb.DVP.ToString() & " Instruccion = " & cb.Instruccion.ToString() & " IdNroOrden = " & cb.IdNroOrden.ToString() & " DetalleInstruccion = " & cb.DetalleInstruccion.ToString() & " EstadoNovedadContabilidad = " & cb.EstadoNovedadContabilidad.ToString() & " eroComprobante_Contabilidad = " & cb.eroComprobante_Contabilidad.ToString() & " hadecontabilizacion_Contabilidad = " & cb.hadecontabilizacion_Contabilidad.ToString() & " haProceso_Contabilidad = " & cb.haProceso_Contabilidad.ToString() & " EstadoNovedadContabilidad_Anulada = " & cb.EstadoNovedadContabilidad_Anulada.ToString() & " EstadoAutomatico = " & cb.EstadoAutomatico.ToString() & " eroLote_Contabilidad = " & cb.eroLote_Contabilidad.ToString() & " MontoEscrito = " & cb.MontoEscrito.ToString() & " TipoIntermedia = " & cb.TipoIntermedia.ToString() & " Concepto = " & cb.Concepto.ToString() & " RecaudoDirecto = " & cb.RecaudoDirecto.ToString() & " ContabilidadEncuenta = " & cb.ContabilidadEncuenta.ToString() & " Sobregiro = " & cb.Sobregiro.ToString() & " IdentificacionAutorizadoCheque = " & cb.IdentificacionAutorizadoCheque.ToString() & " NombreAutorizadoCheque = " & cb.NombreAutorizadoCheque.ToString() & " IDTesoreria = " & cb.IDTesoreria.ToString() & " ContabilidadENC = " & cb.ContabilidadENC.ToString() & " NroLoteAntENC = " & cb.NroLoteAntENC.ToString() & " ContabilidadAntENC = " & cb.ContabilidadAntENC.ToString() & " IdSucursalBancaria = " & cb.IdSucursalBancaria.ToString() & " Creacion = " & cb.Creacion.ToString()
                'dcProxy.Load(dcProxy.TesoreriaConsultarQuery(Filtro, CamposBusquedaTesoreria.Tipo, CamposBusquedaTesoreria.NombreConsecutivo, CamposBusquedaTesoreria.IDDocumento, IIf(CamposBusquedaTesoreria.Documento.Value.Year < 1900, Nothing, CamposBusquedaTesoreria.Documento.Value), estadoMC, CamposBusquedaTesoreria.IDBanco,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "Busqueda")
                dcProxy.Load(dcProxy.TesoreriaConsultarQuery(Filtro, CamposBusquedaTesoreria.Tipo, NombreConsecutivo, IDDocumento, Documento, estadoMC, IDBanco, CamposBusquedaTesoreria.IDCompania, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "Busqueda")
                MyBase.ConfirmarBuscar()
                CamposBusquedaTesoreria = New CamposBusquedaTesoreri
                CambioItem("CamposBusquedaTesoreria")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ImportarArchivo()
        Try
            If String.IsNullOrEmpty(_NombreArchivo) Then
                mostrarMensaje("Debe de seleccionar un archivo para la importación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            If Not IsNothing(objProxyCarga.RespuestaArchivoImportacions) Then
                objProxyCarga.RespuestaArchivoImportacions.Clear()
            End If

            objProxyCarga.Load(objProxyCarga.CargaMasivaDetalleTesoreria_SubirArchivoQuery("ImpTesoreria", moduloTesoreria, Program.Usuario, Program.UsuarioWindows, _NombreArchivo, Program.Maquina, Program.HashConexion), AddressOf TerminoCargarArchivo, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al importar el archivo.", Me.ToString(), "ImportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub QuitarFiltro()
        MyBase.QuitarFiltro()
        Filtro = 0
    End Sub

    ''' <summary>
    ''' Metodo para actualizacion de Tesoreria y detalles por tesoreria.
    ''' </summary>
    ''' <remarks>
    ''' Funcion          : ActualizarRegistro()
    ''' Se encarga de    : Actualizacion de Tesoreria y detalles por tesoreria.
    ''' Modificado por   : Juan Carlos Soto Cruz.
    ''' Descripción      : Se incluye codigo para controlar el orden de actualizacion del encabezado y los detalles.
    ''' Fecha            : Agosto 07/2011
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Agosto 07/2011 - Resultado Ok    
    ''' </remarks>
    ''' 
    Public Overrides Sub ActualizarRegistro()
        Try
            ControlMensaje = False
            If String.IsNullOrEmpty(_TesoreriSelected.NombreConsecutivo) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el tipo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            ' JFSB 20160831 Se agrega validación para cuando el banco vaya vacío.
            If moduloTesoreria = ClasesTesoreria.CE.ToString Then
                If IsNothing(_TesoreriSelected.IDBanco) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el banco.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If moduloTesoreria = ClasesTesoreria.N.ToString And NotaGMFManual = False Then
                If TesoreriSelected.NombreConsecutivo = _ListaTraerBolsas.First.strTipo Or TesoreriSelected.NombreConsecutivo = _ListaTraerBolsas.First.strtipoNotasCxC Then
                    NotaGMFManual = True
                    mostrarMensajePregunta("¿Desea generar ésta nota de GMF manual ?, recuerde que aplicará la funcionalidad de registros adicionales de GMF(si se tiene configurado)",
                                  Program.TituloSistema,
                                  "GUARDARTESORERIA",
                                  AddressOf TerminaPregunta, False)
                    Exit Sub
                End If
            End If

            _TesoreriSelected.Sobregiro = False
            _TesoreriSelected.TrasladoEntreBancos = False
            _TesoreriSelected.BancoGMF = False
            _TesoreriSelected.ClienteGMF = False
            If moduloTesoreria = ClasesTesoreria.CE.ToString Then
                SumarComprobantes()
            ElseIf moduloTesoreria = ClasesTesoreria.RC.ToString Then
                SumarTotales()
            End If

            ConsultarFechaCierreSistema("GUARDAR")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
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
                    Dim logContinuarConsultando As Boolean = False
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count = 0 Then
                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next
                        logContinuarConsultando = True
                    Else
                        Dim objTipo As String = String.Empty

                        If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
                            objListaMensajes.Add("El archivo generó algunas inconsistencias al intentar subirlo:")
                        End If

                        For Each li In objListaRespuesta.OrderBy(Function(o) o.Exitoso)
                            If objTipo <> li.Tipo And li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                                'objTipo = li.Tipo
                                'objListaMensajes.Add(String.Format("Hoja {0}", li.Tipo))
                            ElseIf li.Tipo = "REGISTROSIMPORTADOS" Then
                                'If li.Columna > 0 Then
                                logContinuar = True
                                logContinuarConsultando = True
                                'End If
                            End If

                            If li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                                objListaMensajes.Add(String.Format("Fila: {0} - Campo {1} - Validación: {2}", li.Fila, li.Campo, li.Mensaje))
                            Else
                                objListaMensajes.Add(li.Mensaje)
                            End If
                        Next
                    End If

                    ViewImportarArchivo.ListaMensajes = objListaMensajes
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo"

                    If logContinuarConsultando Then
                        IsBusyDetalles = True
                        If Not IsNothing(dcProxy4.DetalleTesoreris) Then
                            dcProxy4.DetalleTesoreris.Clear()
                        End If
                        dcProxy4.Load(dcProxy4.CargaMasivaDetalleTesoreria_ConsultarQuery(moduloTesoreria, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarDetalleTesoreria, String.Empty)
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo"
                End If

                Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
                ViewImportarArchivo.ShowDialog()

                'If logContinuar Then
                '    If TipoDocumentoSeleccionado.ToUpper = "NOTAS" Then
                '        CargarContenido(TipoOpcionCargar.RESULTADO)
                '    Else
                '        VerAtras = Visibility.Visible
                '        CargarContenido(TipoOpcionCargar.CAMPOS)
                '        VerGrabar = Visibility.Visible
                '    End If
                'Else
                '    VerGrabar = Visibility.Collapsed
                'End If
                IsBusy = False

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de libranzas.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de libranzas.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoConsultarDetalleTesoreria(ByVal lo As LoadOperation(Of DetalleTesoreri))
        Try
            If Not lo.HasError Then
                Dim objListaDetalle As New List(Of OyDTesoreria.DetalleTesoreri)
                Dim intContadorSecuencia As Integer = 0

                objListaDetalle = lo.Entities.ToList

                If Not IsNothing(ListaDetalleTesoreria) Then
                    If ListaDetalleTesoreria.Count > 0 Then
                        For Each li In ListaDetalleTesoreria
                            If li.Secuencia > intContadorSecuencia Then
                                intContadorSecuencia = li.Secuencia
                            End If

                        Next
                    End If
                End If

                intContadorSecuencia += 1

                Dim HabilitarSeleccionBanco As Boolean = False
                Dim HabilitarSeleccionCliente As Boolean = False
                Dim HabilitarSeleccionCuentaContable As Boolean = False

                'JFGB20160511
                If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                    If plogEsFondosOYD Then
                        If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                            HabilitarSeleccionBanco = True
                            HabilitarSeleccionCliente = True
                            HabilitarSeleccionCuentaContable = True
                        Else
                            HabilitarSeleccionBanco = False
                            HabilitarSeleccionCliente = False
                            HabilitarSeleccionCuentaContable = False
                        End If
                    Else
                        HabilitarSeleccionBanco = True
                        HabilitarSeleccionCliente = True
                        HabilitarSeleccionCuentaContable = True
                    End If
                Else
                    HabilitarSeleccionBanco = True
                    HabilitarSeleccionCliente = True
                    HabilitarSeleccionCuentaContable = True
                End If

                Dim objListaDetalleNuevo As New List(Of DetalleTesoreri)

                If Not IsNothing(ListaDetalleTesoreria) Then
                    For Each li In ListaDetalleTesoreria
                        objListaDetalleNuevo.Add(li)
                    Next
                End If

                For Each li In objListaDetalle
                    objListaDetalleNuevo.Add(New OyDTesoreria.DetalleTesoreri With {.Secuencia = intContadorSecuencia,
                                                                                     .Actualizacion = Now.Date,
                                                                                     .Aprobacion = li.Aprobacion,
                                                                                     .BancoDestino = li.BancoDestino,
                                                                                     .Beneficiario = li.Beneficiario,
                                                                                     .CentroCosto = li.CentroCosto,
                                                                                     .CompaniaBanco = li.CompaniaBanco,
                                                                                     .ConsecutivoConsignacionOPT = li.ConsecutivoConsignacionOPT,
                                                                                     .Credito = li.Credito,
                                                                                     .CuentaDestino = li.CuentaDestino,
                                                                                     .Debito = li.Debito,
                                                                                     .Detalle = li.Detalle,
                                                                                     .estadoMC = li.EstadoMC,
                                                                                     .EstadoTransferencia = li.EstadoTransferencia,
                                                                                     .Exportados = li.Exportados,
                                                                                     .FormaEntrega = li.FormaEntrega,
                                                                                     .HabilitarSeleccionBanco = li.HabilitarSeleccionBanco,
                                                                                     .HabilitarSeleccionCliente = li.HabilitarSeleccionCliente,
                                                                                     .HabilitarSeleccionCuentaContable = li.HabilitarSeleccionCuentaContable,
                                                                                     .IDBanco = li.IDBanco,
                                                                                     .IDComisionista = li.IDComisionista,
                                                                                     .IDComitente = li.IDComitente,
                                                                                     .IDConcepto = li.IDConcepto,
                                                                                     .IDCuentaContable = li.IDCuentaContable,
                                                                                     .IDDetalleTesoreria = -(li.IDDetalleTesoreria),
                                                                                     .IDDocumento = TesoreriSelected.IDDocumento,
                                                                                     .IDDocumentoNotaGMF = li.IDDocumentoNotaGMF,
                                                                                     .IdentificacionBenficiciario = li.IdentificacionBenficiciario,
                                                                                     .IdentificacionPerRecoge = li.IdentificacionPerRecoge,
                                                                                     .IdentificacionTitular = li.IdentificacionTitular,
                                                                                     .IDSucComisionista = li.IDSucComisionista,
                                                                                     .InfoSesion = li.InfoSesion,
                                                                                     .intClave_PorAprobar = li.intClave_PorAprobar,
                                                                                     .intIDTesoreria = -(intContadorSecuencia),
                                                                                     .ManejaCliente = li.ManejaCliente,
                                                                                     .NIT = li.NIT,
                                                                                     .Nombre = li.Nombre,
                                                                                     .NombreConsecutivo = TesoreriSelected.NombreConsecutivo,
                                                                                     .NombreConsecutivoNotaGMF = li.NombreConsecutivoNotaGMF,
                                                                                     .NombrePersonaRecoge = li.NombrePersonaRecoge,
                                                                                     .OficinaEntrega = li.OficinaEntrega,
                                                                                     .Por_Aprobar = li.Por_Aprobar,
                                                                                     .Retencion = li.Retencion,
                                                                                     .SaldoCliente = li.SaldoCliente,
                                                                                     .Tesoreri = li.Tesoreri,
                                                                                     .Tipo = li.Tipo,
                                                                                     .TipoCuenta = li.TipoCuenta,
                                                                                     .TipoIdentBeneficiario = li.TipoIdentBeneficiario,
                                                                                     .TipoIdTitular = li.TipoIdTitular,
                                                                                     .TipoMovimientoTesoreria = li.TipoMovimientoTesoreria,
                                                                                     .Titular = li.Titular,
                                                                                     .Usuario = Program.Usuario,
                                                                                     .Valor = li.Valor,
                                                                                     .NombreBancoDestino = li.NombreBancoDestino,
                                                                                     .DescripcionTipoCuenta = li.DescripcionTipoCuenta,
                                                                                     .DescripcionTipoIdTitular = li.DescripcionTipoIdTitular,
                                                                                     .DescripcionFormaEntrega = li.DescripcionFormaEntrega,
                                                                                     .DescripcionTipoIdentBeneficiario = li.DescripcionTipoIdentBeneficiario,
                                                                                     .OficinaCuentaInversion = li.OficinaCuentaInversion,
                                                                                     .NombreCarteraColectiva = li.NombreCarteraColectiva,
                                                                                     .NombreAsesor = li.NombreAsesor})
                    intContadorSecuencia += 1
                Next

                ListaDetalleTesoreria = objListaDetalleNuevo

                MyBase.CambioItem("ListaDetalleTesoreria")

                If ListaDetalleTesoreria.Count > 0 Then
                    DetalleTesoreriSelected = ListaDetalleTesoreria.First
                End If

                If moduloTesoreria = "RC" Then
                    SumarTotales()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleTesoreria",
                                                 Me.ToString(), "TerminoTraerDetalleTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleTesoreria",
                                                             Me.ToString(), "TerminoTraerDetalleTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusyDetalles = False
    End Sub
    ''' Modificado por   : Yessid Andrés Paniagua Pabón
    ''' Descripción      : Se cambio el IF para preguntar por el objeto si viene NULL ya que antes ocasionaba una exepcion
    ''' Fecha            : Abril 04/2016 
    ''' Pruebas CB       : Yessid Andrés Paniagua Pabón - Abril 04/2016 - Resultado Ok   
    ''' ID del Cambio    : YAPP20160404
    Private Sub ContinuarGuardadoDocumento()
        Try
            If Not IsNothing(fechaCierre) Then
                If TesoreriSelected.Documento.ToShortDateString <= fechaCierre Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha del documento no puede ser menor o igual a la fecha de cierre (" & fechaCierre.Value.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If


            If Not IsNothing(ListaDetalleTesoreria) Then  'YAPP20160404 If ListaDetalleTesoreria.Count <> 0 Then 
                If ListaDetalleTesoreria.Count <> 0 Then
                    'Dim numeroErrores = (From lr In ListaDetalleTesoreria Where lr.HasValidationErrors = True).Count
                    'If numeroErrores <> 0 Then
                    '    A2Utilidades.Mensajes.mostrarMensaje("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    '    Exit Sub
                    'End If

                    ConfiguracionConsecutivosSelected = (From obj In ListaConfiguracionConsecutivos Where obj.NombreConsecutivo = _TesoreriSelected.NombreConsecutivo).FirstOrDefault
                    mlogClientes = ConfiguracionConsecutivosSelected.logCliente
                    mlogCuenta = ConfiguracionConsecutivosSelected.logCuentaContable

                    'Realiza las validaciones especificas por modulo.
                    Dim PermitirGuardar As Boolean

                    If moduloTesoreria = "N" Then
                        PermitirGuardar = ValidarNotas()
                        If PermitirGuardar Then
                            ValidacionesServidorNC()
                        End If
                        Exit Sub
                    ElseIf moduloTesoreria = "RC" Then
                        PermitirGuardar = ValidarRecibosCaja()
                        If PermitirGuardar Then
                            ValidacionesServidorRC()
                        End If
                        Exit Sub
                    ElseIf moduloTesoreria = "CE" Then
                        PermitirGuardar = DatosValidosCE()
                        If PermitirGuardar Then
                            ValidacionesServidorCE()
                        End If
                        Exit Sub
                    End If
                Else
                    If moduloTesoreria = "RC" Then
                        If IsNothing(TesoreriSelected.Nombre) Or TesoreriSelected.Nombre = String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el nombre del cliente para el recibo en el encabezado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el detalle del Recibo de Caja.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        Dim encabezado = IIf(moduloTesoreria = "N", "de las Notas contables.", "del Comprobante.")
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los detalles " + encabezado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                If moduloTesoreria = "RC" Then
                    If IsNothing(TesoreriSelected.Nombre) Or TesoreriSelected.Nombre = String.Empty Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el nombre del cliente para el recibo en el encabezado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el detalle del Recibo de Caja.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    Dim encabezado = IIf(moduloTesoreria = "N", "de las Notas contables.", "del Comprobante.")
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los detalles " + encabezado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ContinuarGuardadoDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub guardar()
        Try
            Dim origen = "update"
            ValidaSobregiro = True
            ErrorForma = ""
            TesoreriAnterior = TesoreriSelected
            Dim Secuencia As Integer = 1

            'If Not ListaTesoreria.Contains(TesoreriSelected) Then
            If ListaTesoreria.Where(Function(li) li.IDTesoreria = TesoreriSelected.IDTesoreria).Count = 0 Then
                'If (From lt In ListaTesoreria Where lt.IDDocumento = TesoreriSelected.IDDocumento).Count = 0 Then
                origen = "insert"

                'SLB20130123 Si se cambia el consecutivo del encabezado tambien se cambia el del detalle
                Secuencia = 1

                'con.Numbers.ForEach(Sub(n) n.Primary = False)

                For Each objLista In ListaDetalleTesoreria
                    objLista.NombreConsecutivo = _TesoreriSelected.NombreConsecutivo
                    If Not objLista.Secuencia = Secuencia Then
                        objLista.Secuencia = Secuencia
                    End If
                    Secuencia = Secuencia + 1
                Next

                'For Each detalleTesoreria In ListaDetalleTesoreria
                '    TesoreriSelected.DetalleTesoreris.Add(detalleTesoreria)
                'Next
                'SLB20130304
                If moduloTesoreria = "RC" Then
                    'For Each cheque In ListaChequeTesoreria

                    '    TesoreriSelected.Cheques.Add(cheque)
                    'Next
                    For Each observacion In ListaObservacionesTesoreria

                        TesoreriSelected.TesoreriaAdicionales.Add(observacion)
                    Next

                    Secuencia = 1
                    For Each objLista In ListaChequeTesoreria
                        objLista.NombreConsecutivo = _TesoreriSelected.NombreConsecutivo
                        If Not objLista.Secuencia = Secuencia Then
                            objLista.Secuencia = Secuencia
                        End If
                        Secuencia = Secuencia + 1
                    Next

                End If

                mlogDuplicarTesoreria = False

                ListaTesoreria.Add(TesoreriSelected)
            Else
                'SLB20130304 Cuando se toma un registro ingresado desde VB.6 se le debe adicionar el intIDTesoreria
                If ListaDetalleTesoreria.Count > 0 Then    'ListaDetalleTesoreria.HasChanges
                    Secuencia = 1
                    For Each objLista In ListaDetalleTesoreria
                        If IsNothing(objLista.intIDTesoreria) Then objLista.intIDTesoreria = _TesoreriSelected.IDTesoreria

                        If Not objLista.Secuencia = Secuencia Then
                            objLista.Secuencia = Secuencia
                        End If
                        Secuencia = Secuencia + 1
                    Next
                End If
                If moduloTesoreria = ClasesTesoreria.RC.ToString Then
                    If ListaChequeTesoreria.Count > 0 Then
                        Secuencia = 1
                        For Each objLista In ListaChequeTesoreria
                            If IsNothing(objLista.intIDTesoreria) Then objLista.intIDTesoreria = _TesoreriSelected.IDTesoreria

                            If Not objLista.Secuencia = Secuencia Then
                                objLista.Secuencia = Secuencia
                            End If
                            Secuencia = Secuencia + 1
                        Next
                    End If
                End If
            End If

            If mlogCobroGmf Then
                'SLB20130214
                If moduloTesoreria = ClasesTesoreria.CE.ToString Then
                    mcurTotalDebitos = 0
                    mcurTotalCreditos = 0
                    For Each li In ListaDetalleTesoreria
                        mcurTotalDebitos = mcurTotalDebitos + IIf(li.Debito Is Nothing, 0, li.Debito)
                        mcurTotalCreditos = mcurTotalCreditos + IIf(li.Credito Is Nothing, 0, li.Credito)
                        dblvalordigitado = IIf(li.Debito Is Nothing, 0, li.Debito) - IIf(li.Credito Is Nothing, 0, li.Credito)
                        dblvalorsaldodetalle = dblvalordigitado
                        If mlogCobroGmf And mlogCalculoGMFPorDebajo Then
                            dblvalorsaldodetalle = dblvalordigitado * (1 - mdblGMFInferior)
                            li.Valor = dblvalorsaldodetalle
                        End If
                    Next
                End If
            End If

            'SLB20130802 Manejo control Canje Cheques
            If moduloTesoreria = "RC" Then
                If _mlogCanjeCheque And _ListaChequeTesoreria.First.FormaPagoRC = "C" Then
                    For Each objCheque In ListaChequeTesoreria
                        objCheque.ChequeHizoCanje = False
                    Next
                Else
                    For Each objCheque In ListaChequeTesoreria
                        If Not objCheque.ChequeHizoCanje Then
                            objCheque.ChequeHizoCanje = True
                        End If

                    Next
                End If
            End If

            'Sí el nombre del consecutivo es diferente al del encabezado se lo adiciona
            If Not IsNothing(_ListaDetalleTesoreria) Then
                For Each li In _ListaDetalleTesoreria
                    If li.NombreConsecutivo <> _TesoreriSelected.NombreConsecutivo Then
                        li.NombreConsecutivo = _TesoreriSelected.NombreConsecutivo
                    End If
                Next
            End If

            If moduloTesoreria = "RC" Then
                If Not IsNothing(_ListaChequeTesoreria) Then
                    For Each li In _ListaChequeTesoreria
                        If li.NombreConsecutivo <> _TesoreriSelected.NombreConsecutivo Then
                            li.NombreConsecutivo = _TesoreriSelected.NombreConsecutivo
                        End If
                    Next
                End If
                If Not IsNothing(_ListaObservacionesTesoreria) Then
                    For Each li In _ListaObservacionesTesoreria
                        If li.NombreConsecutivo <> _TesoreriSelected.NombreConsecutivo Then
                            li.NombreConsecutivo = _TesoreriSelected.NombreConsecutivo
                        End If
                    Next
                End If
            End If

            Dim strXMLDetTesoreria As String, strXMLDetCheques As String

            If Not IsNothing(_ListaDetalleTesoreria) AndAlso _ListaDetalleTesoreria.Count > 0 Then

                strXMLDetTesoreria = "<DetTesoreria>  "

                If IsNothing(DetalleTesoreriSelected) Then
                    DetalleTesoreriSelected = _ListaDetalleTesoreria.FirstOrDefault
                End If

                For Each objDet In _ListaDetalleTesoreria

                    Dim strXMLDetalle = <Detalle IDComisionista=<%= objDet.IDComisionista %>
                                            IDSucComisionista=<%= objDet.IDSucComisionista %>
                                            Tipo=<%= objDet.Tipo %>
                                            NombreConsecutivo=<%= objDet.NombreConsecutivo %>
                                            IDDocumento=<%= objDet.IDDocumento %>
                                            Secuencia=<%= objDet.Secuencia %>
                                            IDComitente=<%= objDet.IDComitente %>
                                            Valor=<%= objDet.Valor %>
                                            IDCuentaContable=<%= objDet.IDCuentaContable %>
                                            Detalle=<%= objDet.Detalle %>
                                            IDBanco=<%= objDet.IDBanco %>
                                            CompaniaBanco=<%= objDet.CompaniaBanco %>
                                            NIT=<%= objDet.NIT %>
                                            CentroCosto=<%= objDet.CentroCosto %>
                                            Actualizacion=<%= objDet.Actualizacion %>
                                            Usuario=<%= objDet.Usuario %>
                                            EstadoTransferencia=<%= objDet.EstadoTransferencia %>
                                            BancoDestino=<%= objDet.BancoDestino %>
                                            CuentaDestino=<%= objDet.CuentaDestino %>
                                            TipoCuenta=<%= objDet.TipoCuenta %>
                                            IdentificacionTitular=<%= objDet.IdentificacionTitular %>
                                            Titular=<%= objDet.Titular %>
                                            TipoIdTitular=<%= objDet.TipoIdTitular %>
                                            FormaEntrega=<%= objDet.FormaEntrega %>
                                            Beneficiario=<%= objDet.Beneficiario %>
                                            TipoIdentBeneficiario=<%= objDet.TipoIdentBeneficiario %>
                                            IdentificacionBenficiciario=<%= objDet.IdentificacionBenficiciario %>
                                            NombrePersonaRecoge=<%= objDet.NombrePersonaRecoge %>
                                            IdentificacionPerRecoge=<%= objDet.IdentificacionPerRecoge %>
                                            OficinaEntrega=<%= objDet.OficinaEntrega %>
                                            NombreConsecutivoNotaGMF=<%= objDet.NombreConsecutivoNotaGMF %>
                                            IDDocumentoNotaGMF=<%= objDet.IDDocumentoNotaGMF %>
                                            Exportados=<%= objDet.Exportados %>
                                            IdConcepto=<%= objDet.IDConcepto %>
                                            CodigoCartera=<%= objDet.CodigoCartera %>
                                            OficinaCuentaInversion=<%= objDet.OficinaCuentaInversion %>
                                            NombreCarteraColectiva=<%= objDet.NombreCarteraColectiva %>
                                            NombreAsesor=<%= objDet.NombreAsesor %>
                                            Aprobacion=<%= objDet.Aprobacion %>
                                            IDDetalleTesoreria=<%= objDet.IDDetalleTesoreria %>
                                            EstadoAprobacion=<%= String.Empty %>
                                            EstadoMC=<%= objDet.EstadoMC %>
                                            Clave_PorAprobar=<%= objDet.intClave_PorAprobar %>
                                            CobroGMF=<%= objDet.CobroGMF %>
                                            >
                                        </Detalle>

                    strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString

                Next

                strXMLDetTesoreria = strXMLDetTesoreria & " </DetTesoreria>"

            End If

            Dim strXMLDetTesoreriaSeguro As String = System.Web.HttpUtility.HtmlEncode(strXMLDetTesoreria)

            If Not IsNothing(_ListaChequeTesoreria) AndAlso _ListaChequeTesoreria.Count > 0 Then
                strXMLDetCheques = "<DetCheques>  "

                For Each objCheque In _ListaChequeTesoreria

                    Dim strXMLCheque = <Detalle IDComisionista=<%= objCheque.IDComisionista %>
                                           IDSucComisionista=<%= objCheque.IDSucComisionista %>
                                           NombreConsecutivo=<%= objCheque.NombreConsecutivo %>
                                           IDDocumento=<%= objCheque.IDDocumento %>
                                           Secuencia=<%= objCheque.Secuencia %>
                                           Efectivo=<%= objCheque.Efectivo %>
                                           BancoGirador=<%= objCheque.BancoGirador %>
                                           NumCheque=<%= objCheque.NumCheque %>
                                           Valor=<%= objCheque.Valor %>
                                           BancoConsignacion=<%= objCheque.BancoConsignacion %>
                                           Consignacion=<%= objCheque.Consignacion %>
                                           FormaPagoRC=<%= objCheque.FormaPagoRC %>
                                           Actualizacion=<%= objCheque.Actualizacion %>
                                           Usuario=<%= objCheque.Usuario %>
                                           Comentario=<%= objCheque.Comentario %>
                                           IdProducto=<%= objCheque.IdProducto %>
                                           SucursalesBancolombia=<%= objCheque.SucursalesBancolombia %>
                                           ChequeHizoCanje=<%= objCheque.ChequeHizoCanje %>
                                           IDCheques=<%= objCheque.IDCheques %>
                                           Aprobacion=<%= objCheque.Aprobacion %>
                                           >
                                       </Detalle>

                    strXMLDetCheques = strXMLDetCheques & strXMLCheque.ToString
                Next

                strXMLDetCheques = strXMLDetCheques & " </DetCheques>"
            End If

            Dim strXMLDetChequesSeguro As String = System.Web.HttpUtility.HtmlEncode(strXMLDetCheques)

            TesoreriSelected.XMLDetalle = strXMLDetTesoreriaSeguro
            TesoreriSelected.XMLDetalleCheques = strXMLDetChequesSeguro

            IsBusy = True
            IsBusyDetalles = True
            Dim objRet As LoadOperation(Of OyDTesoreria.RespuestaDatosTesoreria)
            dcProxy.RespuestaDatosTesorerias.Clear()

            objRet = Await dcProxy.Load(dcProxy.TesoreriaActualizarSyncQuery(TesoreriSelected.Aprobacion, TesoreriSelected.Tipo, TesoreriSelected.NombreConsecutivo,
                                                                             TesoreriSelected.IDDocumento, TesoreriSelected.TipoIdentificacion, TesoreriSelected.NroDocumento,
                                                                             TesoreriSelected.Nombre, TesoreriSelected.IDBanco, TesoreriSelected.NumCheque, TesoreriSelected.Valor,
                                                                             TesoreriSelected.Detalle, TesoreriSelected.Documento, TesoreriSelected.Estado, TesoreriSelected.FecEstado,
                                                                             TesoreriSelected.Impresiones, TesoreriSelected.FormaPagoCE, TesoreriSelected.NroLote, TesoreriSelected.Contabilidad,
                                                                             TesoreriSelected.Actualizacion, TesoreriSelected.Usuario, TesoreriSelected.ParametroContable, TesoreriSelected.ImpresionFisica,
                                                                             TesoreriSelected.MultiCliente, TesoreriSelected.CuentaCliente, TesoreriSelected.CodComitente, TesoreriSelected.ArchivoTransferencia,
                                                                             TesoreriSelected.IdNumInst, TesoreriSelected.DVP, TesoreriSelected.Instruccion, TesoreriSelected.IdNroOrden, TesoreriSelected.DetalleInstruccion,
                                                                             TesoreriSelected.EstadoNovedadContabilidad, TesoreriSelected.eroComprobante_Contabilidad, TesoreriSelected.hadecontabilizacion_Contabilidad,
                                                                             TesoreriSelected.haProceso_Contabilidad, TesoreriSelected.EstadoNovedadContabilidad_Anulada, TesoreriSelected.EstadoAutomatico, TesoreriSelected.eroLote_Contabilidad,
                                                                             TesoreriSelected.MontoEscrito, TesoreriSelected.TipoIntermedia, TesoreriSelected.Concepto, TesoreriSelected.RecaudoDirecto, TesoreriSelected.ContabilidadEncuenta,
                                                                             TesoreriSelected.Sobregiro, TesoreriSelected.IdentificacionAutorizadoCheque, TesoreriSelected.NombreAutorizadoCheque, TesoreriSelected.IDTesoreria,
                                                                             TesoreriSelected.ContabilidadENC, TesoreriSelected.NroLoteAntENC, TesoreriSelected.ContabilidadAntENC, TesoreriSelected.IdSucursalBancaria, TesoreriSelected.Creacion,
                                                                             TesoreriSelected.CobroGMF, TesoreriSelected.TrasladoEntreBancos, TesoreriSelected.lngBancoConsignacion, TesoreriSelected.ClienteGMF, TesoreriSelected.BancoGMF,
                                                                             TesoreriSelected.Tipocheque, TesoreriSelected.TipoCuenta, strXMLDetTesoreriaSeguro, strXMLDetChequesSeguro, True, TesoreriSelected.IDComisionista)).AsTask()

            If Not IsNothing(objRet) Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al insertar el registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Exit Sub
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al insertar el registro.", Me.ToString(), "guardar", Program.TituloSistema, Program.Maquina, objRet.Error)
                        Exit Sub
                    End If
                Else
                    dcProxyTransaccion.SubmitChanges(AddressOf TerminoSubmitChanges, origen)

                    objRetornoTesoreria = objRet.Entities.ToList

                    If objRetornoTesoreria.Where(Function(i) i.logExitoso = False).Count > 1 Then
                        A2Utilidades.Mensajes.mostrarMensaje(mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    Else
                        TerminoGrabar(origen)

                        '_TesoreriSelected.IDTesoreria = objRetornoTesoreria.FirstOrDefault.intIDTesoreria
                        '_TesoreriSelected.IDDocumento = objRetornoTesoreria.FirstOrDefault.lngIDocumento

                    End If
                End If

            End If

            Program.VerificarCambiosProxyServidor(dcProxy)
            'dcProxyTransaccion.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            ActivarDuplicarTesoreria = True
            Filtro = 0
            EjecutarCobroGMF = False
            logEditar = False
        Catch ex As Exception
            IsBusy = False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar la validación de los comprobantes de egreso",
                                 Me.ToString(), "guardar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Overrides Async Sub EditarRegistro()
        Try
            If logVentanaEmergente = False Then
                If dcProxy.IsLoading Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If Not IsNothing(_TesoreriSelected) And Not ListaTesoreria.Count = 0 Then
                'CORREC_CITI_SV_2014
                If ListaDetalleTesoreria.Count > TesoreriSelected.TopRegistros Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("El registro no se puede editar debido a que el número de detalles (" & ListaDetalleTesoreria.Count & "), es mayor al número de detalles parametrizado (" & TesoreriSelected.TopRegistros & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    If _TesoreriSelected.Estado = "I" Then
                        MyBase.RetornarValorEdicionNavegacion()
                        A2Utilidades.Mensajes.mostrarMensaje("El estado del documento es Impreso no se puede editar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        'JFSB 20161122
                        If moduloTesoreria = ClasesTesoreria.CE.ToString Then
                            Dim strFormasPago = Split(_mInhabilitar_Edicion_Registro, ",")
                            For Each i In strFormasPago

                                If i = TesoreriSelected.FormaPagoCE Then
                                    Editando = False
                                    EditandoRegistro = False
                                    cmbNombreConsecutivoHabilitado = False
                                    VisibilidadNumCheque = False
                                    EditandoClienteC = False
                                    DeshabilitarSucursal = False
                                    HabilitaTipocheque = False

                                    MyBase.CambioItem("Editando")
                                    MyBase.CambioItem("EditandoRegistro")
                                    MyBase.CambioItem("cmbNombreConsecutivoHabilitado")
                                    MyBase.CambioItem("VisibilidadNumCheque")
                                    MyBase.CambioItem("EditandoClienteC")
                                    MyBase.CambioItem("DeshabilitarSucursal")
                                    MyBase.CambioItem("HabilitaTipocheque")

                                    MyBase.RetornarValorEdicionNavegacion()
                                    IsBusy = False
                                    A2Utilidades.Mensajes.mostrarMensaje("El registro no se puede editar porque la forma de pago está configurada para no dejar modificar el registro. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                                    Exit Sub

                                End If
                            Next
                        End If

                        ListaDetalleAnterior = New List(Of DetalleTesoreri)
                        TotalComprobantesAnterior = TotalComprobantes

                        IsBusy = True
                        Edicion = True
                        ControlMensaje = False
                        logEditar = True
                        EjecutarCobroGMF = True

                        For Each li In ListaDetalleTesoreria
                            ListaDetalleAnterior.Add(li)
                        Next

                        If moduloTesoreria = ClasesTesoreria.RC.ToString Then
                            ListaChequeTesoreriaAnterior = New List(Of Cheque)

                            For Each li In ListaChequeTesoreria
                                ListaChequeTesoreriaAnterior.Add(li)
                            Next
                        End If


                        If Await ValidarEstadoDocumento(ACCIONES_ESTADODOCUMENTO.EDITAR) Then
                            ConsultarFechaCierreSistema("EDITAR")
                        Else
                            IsBusy = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro",
                                 Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub ContinuarEdicionDocumento()
        Try
            If Not IsNothing(fechaCierre) Then
                If _TesoreriSelected.Documento.ToShortDateString <= fechaCierre Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("El registro no se puede editar porque la fecha del documento es menor o igual a la fecha de cierre (" & fechaCierre.Value.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If moduloTesoreria = "N" Then
                ValidarUsuario("Tesoreria_Notas", "Editar")
            ElseIf moduloTesoreria = "CE" Then
                ValidarUsuario("Tesoreria_ComprobantesEgreso", "Editar")
            ElseIf moduloTesoreria = "RC" Then
                ValidarUsuario("Tesoreria_RecibosCaja", "Editar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro",
                                 Me.ToString(), "ContinuarEdicionDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ValidarEstado(ByVal strBusqueda As String)
        Try
            IsBusy = True
            dcProxy.TesoreriaModificables.Clear()
            dcProxy.Load(dcProxy.Validar_EstadoDocumentoTesoreriaQuery(_TesoreriSelected.Tipo, _TesoreriSelected.NombreConsecutivo, _TesoreriSelected.IDDocumento,
                                                      Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarEstado, strBusqueda)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el estado del documento",
                                 Me.ToString(), "ValidarEstado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoValidarEstado(ByVal lo As LoadOperation(Of TesoreriaModificable))
        Try
            IsBusy = False
            If Not lo.HasError Then
                Dim objDatos As TesoreriaModificable
                objDatos = lo.Entities.FirstOrDefault

                If Not IsNothing(objDatos) Then

                    If objDatos.Estado <> _TesoreriSelected.Estado Then
                        _TesoreriSelected.Estado = objDatos.Estado
                    End If

                    If Not objDatos.UltimaModificacion.Equals(_TesoreriSelected.Actualizacion) Then
                        mostrarMensajePregunta("El documento de tesorería fue modificado después que se realizó la consulta para desplegarla en pantalla. Debe actualizarla para poder modificarla." & vbNewLine & vbNewLine & "¿Desea refrescar los datos del documento de tesorería actual?",
                                                           Program.TituloSistema,
                                                           "TERMINOVERIFICARTESORERIANMODIFICABLE",
                                                           AddressOf validarRefrescarTesoreria, False)
                        Exit Sub
                    End If
                End If

                Select Case lo.UserState.ToString
                    Case "editar"
                        Editar(True)
                    Case "borrar"
                        Borrar(True)
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar el estado del documento de tesorería.",
                                         Me.ToString(), "TerminoValidarEstado", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar el estado del documento de tesorería.",
                                     Me.ToString(), "TerminoValidarEstado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que permite resfrecar el documento de tesorería.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub validarRefrescarTesoreria(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                RefrescarTesoreria()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar si se refrescan los datos de las órdenes en pantalla", Me.ToString(), "validarRefrescarOrdenes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Function puedeEditarClienteC(logEditarCliente As Boolean) As Boolean
        If logEditarCliente = False Or VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then
            Return False
        Else
            Return True
        End If
    End Function


    ''' <summary>
    ''' Metodo para permitir la edición del documento.
    ''' </summary>
    ''' <remarks>
    ''' Funcion          : Editar()
    ''' Se encarga de    : Editar un registro validando si el usuario tiene permisos y si el registro se puede editar.
    ''' Modificado por   : Juan David Correa Perez.
    ''' Descripción      : Se incluye codigo para habilitar la edición del registro.
    ''' Fecha            : Agosto 16/2011 2:40 pm
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Agosto 16/2011 - Resultado Ok    
    ''' </remarks>
    Public Sub Editar(Optional ByVal _mlogValidarEstado As Boolean = False, Optional ByVal _mlogValidarRelacionGMF As Boolean = False)
        Try
            If Not IsNothing(_TesoreriSelected) Then
                If Not _mlogValidarEstado Then
                    ValidarEstado("editar")
                    Exit Sub
                End If

                If TesoreriSelected.Por_Aprobar = "Rechazado" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede editar porque ya fue Rechazado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else

                    'Se agrega condición para mostrar mensaje si el documento esta pendiente por aprobar.
                    If (IsNothing(TesoreriSelected.EstadoMC) Or TesoreriSelected.EstadoMC = "Aprobado") And visible = False Then

                        If TesoreriSelected.Estado = "P" Then
                            'SLB20130201 Validar si tiene GMF asociado
                            If Not _mlogValidarRelacionGMF Then
                                intNroValidacionGMF_NC = 0
                                ValidarRelacionGMF("modificar")
                                Exit Sub
                            End If

                            Editando = True
                            EditandoClienteC = puedeEditarClienteC(True) 'CORREC_CITI_SV_2014
                            EditandoRegistro = True
                            HabilitarRCImpreso = True
                            DeshabilitarSucursal = True
                            'JFGB20160511
                            If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                                If plogEsFondosOYD = False Then
                                    If glogConceptoDetalleTesoreriaManual Then
                                        EditarColDetalle = True
                                    End If
                                End If
                            Else
                                If glogConceptoDetalleTesoreriaManual Then
                                    EditarColDetalle = True
                                End If
                            End If


                            Read = False
                            If TesoreriSelected.FormaPagoCE = "C" Then
                                HabilitaTipocheque = True
                            End If

                            If IsNothing(TesoreriSelected.IdComitente) Then
                                HabilitarEdicion = True
                            Else
                                HabilitarEdicion = False
                            End If
                            'SLB20130820 Habilitar o deshabilitar el campo cheque
                            If moduloTesoreria = ClasesTesoreria.CE.ToString Then
                                If (TesoreriSelected.NumCheque = 0 Or IsNothing(_TesoreriSelected.NumCheque)) Then
                                    VisibilidadNumCheque = False
                                Else
                                    buscarBancos(_TesoreriSelected.IDBanco, "consultarbancos")
                                End If
                            End If

                            MyBase.CambioItem("Editando")
                            MyBase.CambioItem("DeshabilitarSucursal")
                            MyBase.CambioItem("Read")
                            MyBase.CambioItem("HabilitarEdicion")
                            cmbNombreConsecutivoHabilitado = False
                            MyBase.CambioItem("cmbNombreConsecutivoHabilitado")
                            'Se agrega la modificación de la actualización para que cuando 
                            'se agregue un detalle o se retire un detalle se dispare el procedimiento 
                            'de actualización del papa.
                            TesoreriSelected.Actualizacion = Now.Date
                            TesoreriSelected.Usuario = Program.Usuario
                            For Each li In ListaDetalleTesoreria
                                li.Usuario = Program.Usuario
                            Next
                            'JFGB20160511
                            If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                                If plogEsFondosOYD Then
                                    If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                                        If Not IsNothing(ListaDetalleTesoreria) Then
                                            For Each li In ListaDetalleTesoreria
                                                li.HabilitarSeleccionCliente = True
                                                li.HabilitarSeleccionBanco = True
                                                li.HabilitarSeleccionCuentaContable = True
                                            Next
                                        End If
                                        HabilitarCreacionDetallesNotas = True
                                    Else
                                        If Not IsNothing(ListaDetalleTesoreria) Then
                                            For Each li In ListaDetalleTesoreria
                                                If li.ManejaCliente = "S" Or li.ManejaCliente = "O" Then
                                                    li.HabilitarSeleccionCliente = True
                                                Else
                                                    li.HabilitarSeleccionCliente = False
                                                End If

                                                li.HabilitarSeleccionBanco = False
                                                li.HabilitarSeleccionCuentaContable = False
                                            Next
                                        End If
                                        HabilitarCreacionDetallesNotas = False
                                    End If
                                Else
                                    If Not IsNothing(ListaDetalleTesoreria) Then
                                        For Each li In ListaDetalleTesoreria
                                            li.HabilitarSeleccionCliente = True
                                            li.HabilitarSeleccionBanco = True
                                            li.HabilitarSeleccionCuentaContable = True
                                        Next
                                    End If
                                    HabilitarCreacionDetallesNotas = True
                                End If
                            Else
                                If Not IsNothing(ListaDetalleTesoreria) Then
                                    For Each li In ListaDetalleTesoreria
                                        li.HabilitarSeleccionCliente = True
                                        li.HabilitarSeleccionBanco = True
                                        li.HabilitarSeleccionCuentaContable = True
                                    Next
                                End If
                                HabilitarCreacionDetallesNotas = True
                            End If

                        ElseIf TesoreriSelected.Estado = "I" Then
                            If moduloTesoreria = ClasesTesoreria.RC.ToString Then
                                TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                                Editando = True
                                EditandoClienteC = puedeEditarClienteC(True) 'CORREC_CITI_SV_2014
                                HabilitarRCImpreso = True
                                MyBase.CambioItem("Editando")
                            Else
                                Editando = False
                                EditandoClienteC = puedeEditarClienteC(False) 'CORREC_CITI_SV_2014
                                EditandoRegistro = False
                                HabilitarRCImpreso = False
                                DeshabilitarSucursal = False
                                'JFGB20160511
                                If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                                    If plogEsFondosOYD = False Then
                                        If glogConceptoDetalleTesoreriaManual Then
                                            EditarColDetalle = False
                                        End If
                                    End If
                                Else
                                    If glogConceptoDetalleTesoreriaManual Then
                                        EditarColDetalle = False
                                    End If
                                End If

                                cmbNombreConsecutivoHabilitado = False
                                Read = True
                                MyBase.CambioItem("DeshabilitarSucursal")
                                MyBase.CambioItem("Editando")
                                MyBase.CambioItem("Read")
                                A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede editar porque ya fue impreso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        Else
                            Editando = False
                            EditandoClienteC = puedeEditarClienteC(False) 'CORREC_CITI_SV_2014
                            EditandoRegistro = False
                            HabilitarRCImpreso = False
                            DeshabilitarSucursal = False
                            'JFGB20160511
                            If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                                If plogEsFondosOYD = False Then
                                    If glogConceptoDetalleTesoreriaManual Then
                                        EditarColDetalle = False
                                    End If
                                End If
                            Else
                                If glogConceptoDetalleTesoreriaManual Then
                                    EditarColDetalle = False
                                End If
                            End If


                            cmbNombreConsecutivoHabilitado = False
                            Read = True
                            MyBase.CambioItem("DeshabilitarSucursal")
                            MyBase.CambioItem("Editando")
                            MyBase.CambioItem("Read")
                            A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede editar porque ya fue anulado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                        nombreConsecutivovisible = False
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede editar hasta no ser aprobado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
                'RecargarSaldoClientes()
                ActivarDuplicarTesoreria = False
                HabilitarImpresion = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro",
                                 Me.ToString(), "Editar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Al momento de editar y duplicar se validan los saldos de detalles.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RecargarSaldoClientes()
        Try
            If moduloTesoreria <> ClasesTesoreria.RC.ToString Then
                Dim secuencia As Integer = 0
                For Each objListaDet In ListaDetalleTesoreria
                    If Not String.IsNullOrEmpty(objListaDet.IDComitente) And Editando Then
                        'dcProxy.ValidarSaldoClientesTesoreria_Generico(objListaDet.IDComisionista,
                        '                                  objListaDet.IDSucComisionista,
                        '                                  objListaDet.IDComitente,
                        '                                  _TesoreriSelected.Documento,Program.Usuario, Program.HashConexion, AddressOf ValidarSaldoClientesTesoreriaCompleted, secuencia)
                    End If
                    secuencia = secuencia + 1
                Next
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición del registro",
                                 Me.ToString(), "RecargarSaldoClientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para consultar si el registro tiene una versión pendiente por aprobar.
    ''' </summary>
    ''' <remarks>
    ''' Funcion          : ValidarEdicion()
    ''' Se encarga de    : Verifica si el registro se puede editar.
    ''' Modificado por   : Juan David Correa Perez.
    ''' Descripción      : Se incluye codigo para controlar la edición del registro.
    ''' Fecha            : Agosto 16/2011 3:19 pm
    ''' Pruebas CB       : Juan David Correa Perez - Agosto 16/2011 - Resultado Ok    
    ''' </remarks>
    Public Sub ValidarEdicion()
        Try
            If Not IsNothing(dcProxy2.Tesoreris) Then
                dcProxy2.Tesoreris.Clear()
            End If

            dcProxy2.Load(dcProxy2.TesoreriaConsultarQuery(0, TesoreriSelected.Tipo, TesoreriSelected.NombreConsecutivo, TesoreriSelected.IDDocumento, Nothing, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarEdicion, "ValidarEdicion")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar validar la edición",
                                 Me.ToString(), "ValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub CambiarAForma()
        Try
            If MakeAndCheck = 1 Then
                If Not IsNothing(TesoreriSelected.EstadoMC) And TesoreriSelected.EstadoMC <> "Aprobado" And TesoreriSelected.EstadoMC <> "Rechazado" Then
                    If TesoreriSelected.EstadoMC.Equals("Ingreso") Then
                        visible = False
                    Else
                        visible = True
                    End If
                End If
                MyBase.CambiarAForma()
            Else
                MyBase.CambiarAForma()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar a la forma",
                                 Me.ToString(), "CambiarAForma", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Maker and Checker"

    Public Overrides Sub AprobarRegistro()
        Try
            'ActivarDuplicarTesoreria = True
            'esVersion = True
            'Dim numeroErrores = (From lr In ListaDetalleTesoreria Where lr.HasValidationErrors = True).Count
            'If numeroErrores <> 0 Then
            '    MessageBox.Show("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", "Alerta", MessageBoxButton.OK)
            '    Exit Sub
            'End If
            'Dim origen = "update"
            'ErrorForma = ""
            'TesoreriAnterior = TesoreriSelected
            'If (TesoreriSelected.EstadoMC.Equals("Modificacion") Or TesoreriSelected.EstadoMC.Equals("Ingreso")) Then
            '    TesoreriSelected.Aprobacion = 2
            '    TesoreriSelected.Usuario = Program.Usuario
            '    DetalleTesoreriSelected = ListaDetalleTesoreria.FirstOrDefault
            '    If Not IsNothing(DetalleTesoreriSelected) Then
            '        If Not IsNothing(DetalleTesoreriSelected) Then
            '            If Not IsNothing(DetalleTesoreriSelected.EstadoMC) Then
            '                If (DetalleTesoreriSelected.EstadoMC.Equals("Modificacion") Or DetalleTesoreriSelected.EstadoMC.Equals("Ingreso") Or DetalleTesoreriSelected.EstadoMC.Equals("Retiro")) Then
            '                    For Each li In ListaDetalleTesoreria
            '                        'DetalleTesoreriSelected = li
            '                        'DetalleTesoreriSelected.Aprobacion = 2
            '                        li.Aprobacion = 2
            '                    Next
            '                End If
            '            End If
            '        End If
            '    End If
            '    'Para guardar la relación de cheques
            '    If moduloTesoreria = "RC" Then
            '        ChequeTesoreriSelected = ListaChequeTesoreria.FirstOrDefault
            '        If Not IsNothing(ChequeTesoreriSelected) Then
            '            If Not IsNothing(ChequeTesoreriSelected) Then
            '                If Not IsNothing(ChequeTesoreriSelected.Estado) Then
            '                    If (ChequeTesoreriSelected.Estado.Equals("Modificacion") Or ChequeTesoreriSelected.Estado.Equals("Ingreso") Or ChequeTesoreriSelected.Estado.Equals("Retiro")) Then
            '                        For Each ch In ListaChequeTesoreria
            '                            'ChequeTesoreriSelected = ch
            '                            'ChequeTesoreriSelected.Aprobacion = 2
            '                            ch.Aprobacion = 2
            '                        Next
            '                    End If
            '                End If
            '            End If
            '        End If
            '    End If

            'ElseIf (TesoreriSelected.EstadoMC.Equals("Retiro")) Then
            '    origen = "BorrarRegistro"
            '    TesoreriSelected.Aprobacion = 2
            '    TesoreriSelected.Usuario = Program.Usuario
            'End If
            'IsBusy = True
            'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            IsBusy = True
            dcProxy.AprobarTesoreria(_TesoreriSelected.IDTesoreria, Program.Usuario, Program.HashConexion, AddressOf TerminoAprobar, "aprobó")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        MyBase.AprobarRegistro()
    End Sub

    Private Sub TerminoAprobar(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        If Not lo.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje("Se " & lo.UserState.ToString & " correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            TesoreriAnterior = Nothing
            dcProxy.Tesoreris.Clear()
            dcProxy.Load(dcProxy.TesoreriaFiltrarQuery("", moduloTesoreria, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "insert") ' Recarga la lista para que carguen los include
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoAprobar" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub


    Public Overrides Sub RechazarRegistro()
        Try
            'ActivarDuplicarTesoreria = True
            'esVersion = True
            'Dim numeroErrores = (From lr In ListaDetalleTesoreria Where lr.HasValidationErrors = True).Count
            'If numeroErrores <> 0 Then
            '    MessageBox.Show("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", "Alerta", MessageBoxButton.OK)
            '    Exit Sub
            'End If
            'Dim origen = "update"
            'ErrorForma = ""
            'TesoreriAnterior = TesoreriSelected
            'If (TesoreriSelected.EstadoMC.Equals("Modificacion") Or TesoreriSelected.EstadoMC.Equals("Ingreso")) Then
            '    TesoreriSelected.Aprobacion = 1
            '    TesoreriSelected.Usuario = Program.Usuario
            '    DetalleTesoreriSelected = ListaDetalleTesoreria.FirstOrDefault
            '    If Not IsNothing(DetalleTesoreriSelected) Then
            '        For Each li In ListaDetalleTesoreria
            '            'DetalleTesoreriSelected = li
            '            'DetalleTesoreriSelected.Aprobacion = 1
            '            li.Aprobacion = 1
            '        Next

            '    End If

            '    'Para guardar la relación de cheques
            '    If moduloTesoreria = "RC" Then
            '        ChequeTesoreriSelected = ListaChequeTesoreria.FirstOrDefault
            '        If Not IsNothing(ChequeTesoreriSelected) Then
            '            For Each ch In ListaChequeTesoreria
            '                'ChequeTesoreriSelected = ch
            '                'ChequeTesoreriSelected.Aprobacion = 1
            '                ch.Aprobacion = 1
            '            Next
            '        End If
            '    End If

            '    'ElseIf (TesoreriSelected.EstadoMC.Equals("RetiroDetalle")) Then
            '    '    origen = "BorrarRegistro"

            '    '    DetalleTesoreriSelected = ListaDetalleTesoreria.FirstOrDefault
            '    '    If Not IsNothing(DetalleTesoreriSelected) Then
            '    '        If Not IsNothing(DetalleTesoreriSelected.EstadoMC) Then
            '    '            If (DetalleTesoreriSelected.EstadoMC.Equals("RetiroDetalle")) Then
            '    '                For Each li In ListaDetalleTesoreria
            '    '                    'DetalleTesoreriSelected = li
            '    '                    'DetalleTesoreriSelected.Aprobacion = 1
            '    '                    li.Aprobacion = 1
            '    '                Next
            '    '            End If
            '    '        End If
            '    '    End If

            '    '    'Para guardar la relación de cheques
            '    '    If moduloTesoreria = "RC" Then
            '    '        ChequeTesoreriSelected = ListaChequeTesoreria.FirstOrDefault
            '    '        If Not IsNothing(ChequeTesoreriSelected) Then
            '    '            If Not IsNothing(ChequeTesoreriSelected.Estado) Then
            '    '                If (ChequeTesoreriSelected.Estado.Equals("RetiroDetalle")) Then
            '    '                    For Each ch In ListaChequeTesoreria
            '    '                        'ChequeTesoreriSelected = ch
            '    '                        'ChequeTesoreriSelected.Aprobacion = 1
            '    '                        ch.Aprobacion = 1
            '    '                    Next
            '    '                End If
            '    '            End If
            '    '        End If
            '    '    End If

            'ElseIf (TesoreriSelected.EstadoMC.Equals("Retiro")) Then
            '    origen = "BorrarRegistro"
            '    TesoreriSelected.Aprobacion = 1
            '    TesoreriSelected.Usuario = Program.Usuario

            '    'DetalleTesoreriSelected = ListaDetalleTesoreria.FirstOrDefault
            '    'If Not IsNothing(DetalleTesoreriSelected) Then
            '    '    If Not IsNothing(DetalleTesoreriSelected.EstadoMC) Then
            '    '        If (DetalleTesoreriSelected.EstadoMC.Equals("Retiro")) Then
            '    '            For Each li In ListaDetalleTesoreria
            '    '                'DetalleTesoreriSelected = li
            '    '                'DetalleTesoreriSelected.Aprobacion = 1
            '    '                li.Aprobacion = 1
            '    '            Next
            '    '        End If
            '    '    End If
            '    'End If

            '    ''Para guardar la relación de cheques
            '    'If moduloTesoreria = "RC" Then
            '    '    ChequeTesoreriSelected = ListaChequeTesoreria.FirstOrDefault
            '    '    If Not IsNothing(ChequeTesoreriSelected) Then
            '    '        If Not IsNothing(ChequeTesoreriSelected.Estado) Then
            '    '            If (ChequeTesoreriSelected.Estado.Equals("Retiro")) Then
            '    '                For Each ch In ListaChequeTesoreria
            '    '                    'ChequeTesoreriSelected = ch
            '    '                    'ChequeTesoreriSelected.Aprobacion = 1
            '    '                    ch.Aprobacion = 1
            '    '                Next
            '    '            End If
            '    '        End If
            '    '    End If
            '    'End If
            'End If

            'IsBusy = True
            'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)

            IsBusy = True
            dcProxy.RechazarTesoreria(_TesoreriSelected.IDTesoreria, Program.Usuario, Program.HashConexion, AddressOf TerminoAprobar, "rechazo")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        MyBase.RechazarRegistro()
    End Sub
    Public Overrides Sub VersionRegistro()
        Try
            esVersion = True
            codigo = TesoreriSelected.IDDocumento
            If TesoreriSelected.Por_Aprobar Is Nothing Then
                dcProxy.Tesoreris.Clear()
                IsBusy = True
                dcProxy.Load(dcProxy.TesoreriaConsultarQuery(0, TesoreriSelected.Tipo, TesoreriSelected.NombreConsecutivo, TesoreriSelected.IDDocumento, Nothing, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, Nothing)
                Filtro = 0
            Else
                dcProxy.Tesoreris.Clear()
                IsBusy = True
                dcProxy.Load(dcProxy.TesoreriaConsultarQuery(1, TesoreriSelected.Tipo, TesoreriSelected.NombreConsecutivo, TesoreriSelected.IDDocumento, Nothing, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, Nothing)
                Filtro = 1
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        MyBase.VersionRegistro()
    End Sub

#End Region

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            IsBusy = True
            IsBusyDetalles = True
            ControlMensaje = False

            If logVentanaEmergente Then
                If Not IsNothing(objVentanaEmergente) Then
                    objVentanaEmergente.DocumentoCreado = False
                    objVentanaEmergente.Close()
                    Exit Sub
                End If
            End If

            If Not IsNothing(_TesoreriSelected) Then
                Editando = False
                EditandoClienteC = puedeEditarClienteC(False) 'CORREC_CITI_SV_2014
                dcProxy.RejectChanges()
                EditandoRegistro = False
                HabilitarRCImpreso = False
                DeshabilitarSucursal = False
                VisibilidadNumCheque = False
                EjecutarCobroGMF = False
                logEditar = False
                'JFGB20160511
                If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                    If plogEsFondosOYD = False Then
                        If glogConceptoDetalleTesoreriaManual Then
                            EditarColDetalle = False
                        End If
                    End If
                Else
                    If glogConceptoDetalleTesoreriaManual Then
                        EditarColDetalle = False
                    End If
                End If


                cmbNombreConsecutivoHabilitado = False
                Read = True
                HabilitaTipocheque = False
                MyBase.CambioItem("Editando")
                MyBase.CambioItem("DeshabilitarSucursal")
                MyBase.CambioItem("Read")
                If _TesoreriSelected.EntityState = EntityState.Detached Then
                    TesoreriSelected = TesoreriAnterior
                End If
                ActivarDuplicarTesoreria = True
                If MakeAndCheck = 1 Then
                    HabilitarImpresion = False
                Else
                    HabilitarImpresion = True
                End If

                CuentaTrasferencia()

                logCambiarCuentaBancaria = False
                'JFSB 20180228 Se ajusta la asociacion de la cuenta bancaria del cliente
                If String.IsNullOrEmpty(TesoreriSelected.CuentaCliente) Then
                    CuentaContableCliente = Nothing
                Else
                    CuentaContableCliente = TesoreriSelected.CuentaCliente
                End If
                logCambiarCuentaBancaria = True

            End If
            If Not IsNothing(ListaDetalleTesoreriaAnt) Then
                ListaDetalleTesoreriaAnt.Clear()
            End If
            If Not IsNothing(ListaChequeTesoreriaAnt) Then
                ListaChequeTesoreriaAnt.Clear()
            End If

            Dim objlista As New List(Of DetalleTesoreri)
            Dim objlistaCheque As New List(Of Cheque)

            If Not IsNothing(ListaDetalleAnterior) Then
                For Each li In ListaDetalleAnterior
                    objlista.Add(li)
                Next
            End If

            ListaDetalleTesoreria = objlista

            If Not IsNothing(ListaChequeTesoreriaAnterior) Then
                For Each li In ListaChequeTesoreriaAnterior
                    objlistaCheque.Add(li)

                Next
            End If

            ListaChequeTesoreria = objlistaCheque

            If Not IsNothing(ListaDetalleTesoreria) Then
                For Each li In ListaDetalleTesoreria
                    ListaDetalleTesoreriaAnt.Add(New secuenciadetalletesoreira With {.Secuencia = li.Secuencia})
                Next
            End If
            If Not IsNothing(ListaChequeTesoreria) Then
                For Each li In ListaChequeTesoreria
                    ListaChequeTesoreriaAnt.Add(New secuenciachequestesoreria With {.Secuencia = li.Secuencia})
                Next
            End If
            'listConsecutivos = CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(strNombreConsecutivo)
            MyBase.CambioItem("ListaDetalleTesoreria")
            MyBase.CambioItem("ListaChequeTesoreria")

            Dim strConsecutivoSeleccionado As String = _TesoreriSelected.NombreConsecutivo
            listConsecutivos = _ListConsecutivosConsola
            _TesoreriSelected.NombreConsecutivo = strConsecutivoSeleccionado

            BotonesOrdenes = False
            'JFGB20160511
            HabilitarCreacionDetallesNotas = False
            IsBusy = False
            IsBusyDetalles = False
            Edicion = False
            TotalComprobantesAnterior = 0
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConsultarConsecutivoBanco()
        dcProxy.ConsecutivoBancos.Clear()
        dcProxy.Load(dcProxy.ConsutarConsecutivoBancoQuery(TesoreriSelected.IDBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoConsutarConsecutivoBanco, Nothing)
    End Sub

#Region "Búsqueda de Bancos desde el control de la vista"

    ''' <summary>
    ''' Buscar los datos del banco seleccionado en el encabezado y en el detalle de Tesoreria
    ''' </summary>
    ''' <param name="plngIdBanco">Codigo del banco el cual se va a realizar la búsqueda</param>
    ''' <remarks>SLB20130312</remarks>
    Friend Sub buscarBancos(Optional ByVal plngIdBanco As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy.BuscadorGenericos.Clear()
            '  If moduloTesoreria = "CE" Then
            objProxy.Load(objProxy.buscarItemsQuery(plngIdBanco, "cuentasbancarias", "T", "BANCOSXCONSECUTIVO", _TesoreriSelected.IDCompania, _TesoreriSelected.NombreConsecutivo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, pstrBusqueda)
            ' Else
            '  objProxy.Load(objProxy.buscarItemEspecificoQuery("cuentasbancarias", plngIdBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, pstrBusqueda)
            '   End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método recibe la respuesta si el banco existe o no
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>20130201</remarks>
    ''' JFSB 20170801 - Se ajusta la lógica para que concuerde con el dato a buscar
    Private Sub TerminoTraerCuentasBancarias(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                _mlogBuscarBancos = False
                If lo.Entities.ToList.Count > 0 Then
                    Select Case lo.UserState.ToString
                        Case "consultarbancos"
                            If lo.Entities.Where(Function(i) i.InfoAdicional02 = "1" And i.IdItem = TesoreriSelected.IDBanco).Count > 0 Then
                                CuentaBancaria = lo.Entities.Where(Function(i) i.InfoAdicional02 = "1" And i.IdItem = TesoreriSelected.IDBanco).First
                                If CuentaBancaria.InfoAdicional03 = _TesoreriSelected.IDCompania.ToString Then 'SV20160203
                                    _TesoreriSelected.IDBanco = CuentaBancaria.IdItem 'lo.Entities.First.IdItem
                                    _TesoreriSelected.NombreBco = CuentaBancaria.Nombre 'lo.Entities.First.Nombre
                                    _TesoreriSelected.ChequeraAutomatica = CuentaBancaria.Estado 'lo.Entities.First.Estado
                                    If Not String.IsNullOrEmpty(CuentaBancaria.InfoAdicional03) Then
                                        _TesoreriSelected.CompaniaBanco = CuentaBancaria.InfoAdicional03
                                    Else
                                        _TesoreriSelected.CompaniaBanco = Nothing
                                    End If

                                    ConsultarConsecutivoBanco()
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra asociado a una compañia diferente a la del consecutivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    LimpiarBanco()
                                End If
                            ElseIf lo.Entities.Where(Function(i) i.InfoAdicional02 = "0" And i.IdItem = TesoreriSelected.IDBanco).Count > 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                LimpiarBanco()
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                LimpiarBanco()
                            End If
                        Case "consultarBancoDestino"
                            If lo.Entities.Where(Function(i) i.InfoAdicional02 = "1" And i.IdItem = TesoreriSelected.lngBancoConsignacion).Count > 0 Then
                                CuentaBancaria = lo.Entities.Where(Function(i) i.InfoAdicional02 = "1" And i.IdItem = TesoreriSelected.lngBancoConsignacion).First

                                _TesoreriSelected.lngBancoConsignacion = CuentaBancaria.IdItem 'lo.Entities.First.IdItem
                                _TesoreriSelected.strBancoDestino = CuentaBancaria.Nombre 'lo.Entities.First.Nombre
                            ElseIf lo.Entities.Where(Function(i) i.InfoAdicional02 = "0" And i.IdItem = TesoreriSelected.lngBancoConsignacion).Count > 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                _TesoreriSelected.lngBancoConsignacion = Nothing
                                _TesoreriSelected.strBancoDestino = String.Empty
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                _TesoreriSelected.lngBancoConsignacion = Nothing
                                _TesoreriSelected.strBancoDestino = String.Empty
                            End If
                        Case "consultarBancoDetalle"
                            If moduloTesoreria = ClasesTesoreria.N.ToString Then
                                If lo.Entities.Where(Function(i) i.InfoAdicional02 = "1" And i.IdItem = DetalleTesoreriSelected.IDBanco).Count > 0 Then
                                    CuentaBancaria = lo.Entities.Where(Function(i) i.InfoAdicional02 = "1" And i.IdItem = DetalleTesoreriSelected.IDBanco).First
                                    BancoSeleccionadoDetalle(CuentaBancaria)
                                ElseIf lo.Entities.Where(Function(i) i.InfoAdicional02 = "0" And i.IdItem = DetalleTesoreriSelected.IDBanco).Count > 0 Then
                                    A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    BancoSeleccionadoDetalle(Nothing)
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    BancoSeleccionadoDetalle(Nothing)
                                End If
                            Else
                                If lo.Entities.Where(Function(i) i.InfoAdicional02 = "1" And i.IdItem = ChequeTesoreriSelected.BancoConsignacion).Count > 0 Then
                                    CuentaBancaria = lo.Entities.Where(Function(i) i.InfoAdicional02 = "1" And i.IdItem = ChequeTesoreriSelected.BancoConsignacion).First
                                    BancoSeleccionadoDetalle(CuentaBancaria)
                                ElseIf lo.Entities.Where(Function(i) i.InfoAdicional02 = "0" And i.IdItem = ChequeTesoreriSelected.BancoConsignacion).Count > 0 Then
                                    A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    BancoSeleccionadoDetalle(Nothing)
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    BancoSeleccionadoDetalle(Nothing)
                                End If
                            End If
                    End Select
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Select Case lo.UserState.ToString
                        Case "consultarbancos"
                            LimpiarBanco()
                        Case "consultarBancoDestino"
                            _TesoreriSelected.lngBancoConsignacion = Nothing
                            _TesoreriSelected.strBancoDestino = String.Empty
                        Case "consultarBancoDetalle"
                            BancoSeleccionadoDetalle(Nothing)
                    End Select
                End If
                _mlogBuscarBancos = True
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos",
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                _mlogBuscarBancos = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(),
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Método para asignar los valores correspodientes del banco seleccionado en el detalle de NC y RC
    ''' </summary>
    ''' <param name="pobjBanco"></param>
    ''' <remarks>SLB20130312</remarks>
    Public Sub BancoSeleccionadoDetalle(ByVal pobjBanco As OYDUtilidades.BuscadorGenerico)
        If moduloTesoreria = ClasesTesoreria.N.ToString Then
            _DetalleTesoreriSelected.IDComitente = Nothing

            If Not IsNothing(pobjBanco) Then
                _DetalleTesoreriSelected.IDBanco = pobjBanco.IdItem
                _DetalleTesoreriSelected.Nombre = pobjBanco.Nombre
                _mlogBuscarCuentaContable = False
                _DetalleTesoreriSelected.IDCuentaContable = pobjBanco.CodigoAuxiliar
                _mlogBuscarCuentaContable = True
                If Not String.IsNullOrEmpty(pobjBanco.InfoAdicional03) Then
                    _DetalleTesoreriSelected.CompaniaBanco = pobjBanco.InfoAdicional03 'SV20160203
                Else
                    _DetalleTesoreriSelected.CompaniaBanco = Nothing 'SV20160203
                End If

            Else
                _DetalleTesoreriSelected.IDBanco = Nothing
                _DetalleTesoreriSelected.Nombre = String.Empty
                _DetalleTesoreriSelected.IDCuentaContable = String.Empty
                _DetalleTesoreriSelected.CompaniaBanco = Nothing 'SV20160203
            End If
        End If

        If moduloTesoreria = ClasesTesoreria.RC.ToString Then
            If Not IsNothing(pobjBanco) Then
                _ChequeTesoreriSelected.BancoConsignacion = pobjBanco.IdItem
                _ChequeTesoreriSelected.Consignacion = Now.Date

                If Not String.IsNullOrEmpty(pobjBanco.InfoAdicional03) Then
                    _ChequeTesoreriSelected.CompaniaBanco = pobjBanco.InfoAdicional03 'SV20160203
                Else
                    _ChequeTesoreriSelected.CompaniaBanco = Nothing 'SV20160203
                End If
            Else
                _ChequeTesoreriSelected.BancoConsignacion = Nothing
                _ChequeTesoreriSelected.Consignacion = Nothing
            End If
        End If
    End Sub

    'SV20160203
    Public Sub LimpiarBanco()
        _TesoreriSelected.IDBanco = Nothing
        _TesoreriSelected.NombreBco = String.Empty
        _TesoreriSelected.NumCheque = Nothing
        _TesoreriSelected.CompaniaBanco = Nothing 'SV20160203
    End Sub


#End Region

    ''' <summary>
    ''' Metodo para realizar el calculo de totales de debitos, creditos y cheques del recibo de caja.
    ''' </summary>
    ''' <remarks>
    ''' Funcion          : SumarTotales()
    ''' Se encarga de    : Sumar los totales de los cheques y los recibos de caja.
    ''' Modificado por   : Juan David Correa Perez.
    ''' Descripción      : Se incluye codigo para comprobar que la resta de los recibos de caja y de los cheques sea 
    '''                    igual a 0.
    ''' Fecha            : Agosto 08/2011
    ''' Pruebas CB       : Juan David Correa Perez- Agosto 08/2011 - Resultado Ok    
    ''' </remarks>
    Public Sub SumarTotales()
        Try
            'SUMAR LOS TOTALES DE RECIBOS Y CHEQUES

            TotalRecibos = 0

            If Not (ListaDetalleTesoreria Is Nothing) Then
                For Each T In ListaDetalleTesoreria
                    TotalRecibos = TotalRecibos + IIf(T.Debito Is Nothing, 0, T.Debito) - IIf(T.Credito Is Nothing, 0, T.Credito)
                Next
            End If

            TotalCheques = 0

            If Not (ListaChequeTesoreria Is Nothing) Then
                For Each C In ListaChequeTesoreria
                    TotalCheques = TotalCheques + C.Valor
                Next
            End If

            MyBase.CambioItem("TotalRecibos")
            MyBase.CambioItem("TotalCheques")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el calculo de los totales",
                                 Me.ToString(), "SumarTotales", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Public Sub SumarComprobantes()
        Try
            TotalComprobantes = 0
            If Not IsNothing(ListaDetalleTesoreria) Then
                For Each Lista In ListaDetalleTesoreria
                    TotalComprobantes = TotalComprobantes + IIf(IsNothing(Lista.Debito), 0, Lista.Debito) - IIf(IsNothing(Lista.Credito), 0, Lista.Credito)
                Next
            End If
            MyBase.CambioItem("TotalComprobantes")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el calculo de los totales",
                                 Me.ToString(), "SumarComprobantes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Private Sub CerroVentana()
    '    If autorizaciones.DialogResult = True Then
    '        If autorizaciones.autorizapago = True Then
    '            IsBusy = True

    '            If moduloTesoreria = "N" Then
    '                dcProxy.LogAutorizacion(TesoreriSelected.NombreConsecutivo, IIf(TesoreriSelected.IDDocumento = -1, 0, TesoreriSelected.IDDocumento), IIf(comitente = String.Empty, "0", comitente), "AUTORIZACION DE GIRO EN COMPROBANTE DE EGRESO", TesoreriSelected.Documento, autorizaciones.validausuario.usuario, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerLoginsertar, Nothing)
    '            ElseIf moduloTesoreria = "CE" Then
    '                dcProxy.LogAutorizacion(TesoreriSelected.NombreConsecutivo, IIf(TesoreriSelected.IDDocumento = -1, 0, TesoreriSelected.IDDocumento), IIf(comitente = String.Empty, "0", comitente), "AUTORIZACION DE SOBREGIRO EN NOTAS DE TESORERIA", TesoreriSelected.Documento, autorizaciones.validausuario.usuario, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerLoginsertar, Nothing)
    '            End If


    '        End If
    '    End If

    'End Sub

    'Private Sub TerminoTraerLoginsertar(ByVal obj As InvokeOperation(Of Integer))
    '    'IsBusy = False
    '    If obj.HasError Then
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminoTraervalidar", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
    '        ValidaSobregiro = False
    '    Else
    '        ValidaSobregiro = True
    '        Dim origen = "update"
    '        ErrorForma = ""
    '        TesoreriAnterior = TesoreriSelected
    '        'If Not ListaTesoreria.Contains(TesoreriSelected) Then
    '        If (From lt In ListaTesoreria Where lt.IDTesoreria = TesoreriSelected.IDTesoreria).Count = 0 Then
    '            origen = "insert"
    '            For Each detalleTesoreria In ListaDetalleTesoreria
    '                TesoreriSelected.DetalleTesoreris.Add(detalleTesoreria)
    '            Next

    '            ListaTesoreria.Add(TesoreriSelected)
    '        End If

    '        IsBusy = True
    '        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
    '        ActivarDuplicarTesoreria = True
    '        Filtro = 0
    '    End If
    'End Sub

    Private Sub Terminotraervalorparametrocontroldual(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "Terminotraervalorparametrocontroldual", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            ValidaSobregiro = False
        Else

            If IIf(Not Versioned.IsNumeric(obj.UserState), obj.UserState, "0") = "ValidaCobroGmfNotas" And obj.Value = "SI" Then
                logCobrarGMF = True
                Exit Sub
            End If
            If msobregiro Or obj.Value = "SI" Then
                dcProxy.AprobarCompEgreso(TesoreriSelected.NombreConsecutivo, TesoreriSelected.IDDocumento, "Aplazar", msobregiro, IIf(Versioned.IsNumeric(obj.UserState), obj.UserState, 0), True, Program.Usuario, Program.HashConexion, AddressOf TerminoactualizartesoreriaCE, Nothing)
            End If
            'SI EL COMPROBANTE QUEDA EN ESTADO A POR EL SOBREGIRO,LA NOTA GMF ASOCIADA TAMBIEN 
            If msobregiro And mlogCobroGmf And Versioned.IsNumeric(obj.UserState) Then
                dcProxy.AprobarCompEgreso(mstrNombreConsecutivoNota, obj.UserState, "Aplazar", msobregiro, 0, False, Program.Usuario, Program.HashConexion, AddressOf TerminoactualizartesoreriaNOTA, Nothing) 'CONSECUTIVO DE LA NOTA
            End If
            mlogCobroGmf = False
            msobregiro = False
            mstrNroCuentaContableBanco = ""
            mstrlogCuenta_4_pormil_DeBanco = STR_NO
            IsBusy = True
            TesoreriAnterior = Nothing
            dcProxy.Tesoreris.Clear()
            dcProxy.Load(dcProxy.TesoreriaFiltrarQuery("", moduloTesoreria, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "insert") ' Recarga la lista para que carguen los include
        End If
    End Sub
    Private Sub TerminoactualizartesoreriaCE(ByVal obj As InvokeOperation(Of Integer))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminoactualizartesoreriaCE", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        End If
    End Sub
    Private Sub TerminoactualizartesoreriaNOTA(ByVal obj As InvokeOperation(Of Integer))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminoactualizartesoreriaNOTA", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        End If
    End Sub

    'Public Sub ValidarTransferencia()
    '    If String.IsNullOrEmpty(TesoreriSelected.TipoCuenta) Then

    '        If Not (TransferenciaDisponible) Then
    '            'If Not String.IsNullOrEmpty(TesoreriSelected.IdComitente) Then
    '            TesoreriSelected.FormaPagoCE = Nothing
    '            TesoreriSelected.FormaPagoCE = "C"
    '            MyBase.CambioItem("TesoreriSelected")
    '            A2Utilidades.Mensajes.mostrarMensaje("La Transferencia Electronica no se encuentra disponible para este cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '        Else
    '            TesoreriSelected.SucursalBancaria = Nothing
    '            TesoreriSelected.IdSucursalBancaria = 109
    '            DeshabilitarSucursal = False
    '            MyBase.CambioItem("DeshabilitarSucursal")
    '        End If
    '    Else
    '        TesoreriSelected.SucursalBancaria = Nothing
    '        TesoreriSelected.IdSucursalBancaria = 109
    '        DeshabilitarSucursal = False
    '        MyBase.CambioItem("DeshabilitarSucursal")
    '    End If

    'End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub


#Region "Métodos para cuando se termina de grabar un documento de Tesoreria"

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                Dim strMsg As String = String.Empty
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                        Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                        So.MarkErrorAsHandled()
                    End If
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                Exit Try
            End If
            'SLB20130208 Se traslada la Información de las tablas temporales a la originales de Tesoreria
            IsBusy = True
            IsBusyDetalles = True
            SoSumitChanges = So
            ''' JFSB 20180102 Se agrega el parámetro de la compañía
            'dcProxy.TrasladarTesoreria_TMP_TBL(_TesoreriSelected.IDTesoreria, Program.Usuario, _TesoreriSelected.IDCompania, Program.HashConexion, AddressOf TerminoTrasladarTesoreria, So.UserState.ToString)
            HabilitaTipocheque = False
            EditandoClienteC = puedeEditarClienteC(False) 'CORREC_CITI_SV_2014
            Edicion = False
            'mlogCobroGmf = False
            'logCobrarGMF = False
            mlogCalculoGMFPorEncima = False
            mlogCalculoGMFPorDebajo = False
            mlogclienteGMF = False
            mlogBancoGMF = False
            ValidaCobroMGF = False
            NotaGMFManual = False
            ControlMensaje = False

            IsBusy = False
            IsBusyDetalles = False
            If Not So.HasError Then
                MyBase.TerminoSubmitChanges(SoSumitChanges)
                EditandoRegistro = False
                HabilitarRCImpreso = False
                VisibilidadNumCheque = False
                'JFGB20160511
                If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                    If plogEsFondosOYD = False Then
                        If glogConceptoDetalleTesoreriaManual Then
                            EditarColDetalle = False
                        End If
                    End If
                Else
                    If glogConceptoDetalleTesoreriaManual Then
                        EditarColDetalle = False
                    End If
                End If

                'JFGB20160511
                HabilitarCreacionDetallesNotas = False
                cmbNombreConsecutivoHabilitado = False

                If logVentanaEmergente Then
                    If Not IsNothing(objVentanaEmergente) Then
                        objVentanaEmergente.DocumentoCreado = True
                        objVentanaEmergente.FechaDocumentoActualizado = _TesoreriSelected.Documento
                        objVentanaEmergente.TipoRegistroActualizado = moduloTesoreria
                        ' objVentanaEmergente.IDDocumentoActualizado = _TesoreriSelected.IDDocumento
                        Dim Documento As Integer
                        For Each li In objRetornoTesoreria
                            If Not IsNothing(li.lngIDocumento) Then
                                Documento = li.lngIDocumento
                            End If
                        Next
                        objVentanaEmergente.IDDocumentoActualizado = Documento
                        objVentanaEmergente.NombreConsecutivoActualizado = _TesoreriSelected.NombreConsecutivo
                        Dim dblValorTotal As Double = 0
                        For Each li In ListaDetalleTesoreria
                            If Not IsNothing(li.Debito) Then
                                dblValorTotal += li.Debito
                            End If
                        Next
                        If dblValorTotal = 0 Then
                            For Each li In ListaDetalleTesoreria
                                If Not IsNothing(li.Credito) Then
                                    dblValorTotal += li.Credito
                                End If
                            Next
                        End If

                        objVentanaEmergente.ValorTotalActualizado = dblValorTotal
                        objVentanaEmergente.Close()
                    End If
                End If
            Else
                dcProxy.BorrarTesoreria_TMP_TBL(_TesoreriSelected.IDTesoreria, Program.Usuario, Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarTMP, "borrar")
                If SoSumitChanges.UserState = "insert" Then
                    configurarNuevaTesoreria(_TesoreriSelected, True)
                End If
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                                   Me.ToString(), "TerminoTrasladarTesoreria" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
            End If '
            'TerminoGrabar(So)
            ' MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            BotonesOrdenes = False
        End Try
    End Sub

    ''' <summary>
    ''' Se recibe la respuesta si se traslado correctamente la información de la tablas temporales a las tablas de Tesoreria
    ''' </summary>
    ''' <param name="lo">SLB20130208</param>
    ''' <remarks></remarks>
    Private Sub TerminoTrasladarTesoreria(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        IsBusyDetalles = False
        If Not lo.HasError Then
            MyBase.TerminoSubmitChanges(SoSumitChanges)
            EditandoRegistro = False
            HabilitarRCImpreso = False
            VisibilidadNumCheque = False
            'JFGB20160511
            If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                If plogEsFondosOYD = False Then
                    If glogConceptoDetalleTesoreriaManual Then
                        EditarColDetalle = False
                    End If
                End If
            Else
                If glogConceptoDetalleTesoreriaManual Then
                    EditarColDetalle = False
                End If
            End If

            'JFGB20160511
            HabilitarCreacionDetallesNotas = False
            cmbNombreConsecutivoHabilitado = False

            If logVentanaEmergente Then
                If Not IsNothing(objVentanaEmergente) Then
                    objVentanaEmergente.DocumentoCreado = True
                    objVentanaEmergente.FechaDocumentoActualizado = _TesoreriSelected.Documento
                    objVentanaEmergente.TipoRegistroActualizado = moduloTesoreria
                    objVentanaEmergente.IDDocumentoActualizado = _TesoreriSelected.IDDocumento
                    objVentanaEmergente.NombreConsecutivoActualizado = _TesoreriSelected.NombreConsecutivo
                    Dim dblValorTotal As Double = 0
                    For Each li In ListaDetalleTesoreria
                        If Not IsNothing(li.Debito) Then
                            dblValorTotal += li.Debito
                        End If
                    Next
                    If dblValorTotal = 0 Then
                        For Each li In ListaDetalleTesoreria
                            If Not IsNothing(li.Credito) Then
                                dblValorTotal += li.Credito
                            End If
                        Next
                    End If

                    objVentanaEmergente.ValorTotalActualizado = dblValorTotal
                    objVentanaEmergente.Close()
                End If
            End If
        Else
            dcProxy.BorrarTesoreria_TMP_TBL(_TesoreriSelected.IDTesoreria, Program.Usuario, Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarTMP, "borrar")
            If SoSumitChanges.UserState = "insert" Then
                configurarNuevaTesoreria(_TesoreriSelected, True)
            End If
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoTrasladarTesoreria" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If '
    End Sub

    ''' <summary>
    ''' Se recibe la respuesta si se borro correctamente la información de la tablas temporales
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130208</remarks>
    Private Sub TerminoEliminarTMP(ByVal lo As InvokeOperation(Of Integer))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoEliminarTMP" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If '
    End Sub

    ''' <summary>
    ''' Método para grabar el log en la lista Clinton y para recargar la lista de Tesoereria
    ''' </summary>
    ''' <param name="So"></param>
    ''' <remarks>SLB2013022</remarks>
    Sub TerminoGrabar(ByVal So As String) '(ByVal So As SubmitOperation)
        'SLB20130208 Se graba el Log de la Lista Clinton si aplica
        If _mlogActivarListaClinton Then
            If _mlogGrabarListaClinton Then
                If _TesoreriSelected.Nombre <> "" And DatosListaClinton.Nombre <> "" And DatosListaClinton.porcentaje > 0 Then
                    Dim strForma As String = ""
                    Select Case moduloTesoreria
                        Case ClasesTesoreria.CE.ToString
                            strForma = "frmCompEgreso"
                        Case ClasesTesoreria.RC.ToString
                            strForma = "frmRecibosCaja"
                    End Select
                    objProxy.GrabarListaClinton(strForma, _TesoreriSelected.IdComitente, _TesoreriSelected.NroDocumento,
                                               _TesoreriSelected.Nombre, DatosListaClinton.Nombre, DatosListaClinton.porcentaje,
                                               Program.Usuario, Program.HashConexion, AddressOf TerminoGrabarListaClinton, "grabar")
                End If
                _mlogGrabarListaClinton = False
            End If
        End If

        'Se incluye codigo para el manejo del Maker and Checker
        'If Not IsNothing(MakeAndCheck) Then
        '    If MakeAndCheck = 1 Then
        'If Not IsNothing(So.UserState) Then
        If Not String.IsNullOrEmpty(So) Then 'And (Not So = "update" And MakeAndCheck = 0)
            TesoreriAnterior = Nothing
            dcProxy.Tesoreris.Clear()
            dcProxy.Load(dcProxy.TesoreriaFiltrarQuery("", moduloTesoreria, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "insert") ' Recarga la lista para que carguen los include
        End If
        '    End If
        'End If
        'Fin Maker and Checker
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Se recibe la respuesta la insercción en Lista Clinton
    ''' </summary>
    ''' <param name="lo">SLB20130208</param>
    ''' <remarks></remarks>
    Private Sub TerminoGrabarListaClinton(ByVal lo As InvokeOperation(Of Boolean))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar el Log de la Lista Clinton",
                                               Me.ToString(), "TerminoGrabarListaClinton" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

#End Region

#Region "Validación de la asociación a un documento GMF"

    ''' <summary>
    ''' Método para validar si el documento de Tesoreria esta asociado a una Nota GMF
    ''' </summary>
    ''' <param name="strEstado"></param>
    ''' <remarks></remarks>
    Private Sub ValidarRelacionGMF(ByVal strEstado As String)
        Try
            Select Case moduloTesoreria
                Case ClasesTesoreria.CE.ToString
                    'SLB20130703 Se permite editar los CE que tienen una nota GMF Relacionada.
                    If IsNothing(_TesoreriSelected.lngBancoConsignacion) Then
                        Select Case strEstado
                            Case "modificar"
                                Editar(True, True)
                            Case "anular"
                                Borrar(True, True)
                        End Select
                    Else
                        IsBusy = True
                        dcProxy.ValidarDocumentoGMFs.Clear()
                        dcProxy.Load(dcProxy.ValidarRelacion_GMFQuery(_TesoreriSelected.NombreConsecutivo, _TesoreriSelected.IDDocumento, "0", Program.Usuario, Program.HashConexion), AddressOf TerminaValidarRelacionGMF, strEstado)
                        Exit Sub
                    End If
                Case ClasesTesoreria.N.ToString
                    'SLB20130305 Se valida la NC si tiene un GMF contra un CE (intNroValidacionGMF_NC = 1) y tambien contra otra NC (intNroValidacionGMF_NC = 0)
                    'SLB20130704 Solo se debe validar la NC que sea GMF intNroValidacionGMF_NC = 1
                    intNroValidacionGMF_NC = 1
                    IsBusy = True
                    dcProxy.ValidarDocumentoGMFs.Clear()
                    dcProxy.Load(dcProxy.ValidarRelacion_GMFQuery(_TesoreriSelected.NombreConsecutivo, _TesoreriSelected.IDDocumento, intNroValidacionGMF_NC, Program.Usuario, Program.HashConexion), AddressOf TerminaValidarRelacionGMF, strEstado)
                    Exit Sub
                Case ClasesTesoreria.RC.ToString
                    IsBusy = True
                    dcProxy.ValidarDocumentoGMFs.Clear()
                    dcProxy.Load(dcProxy.ValidarComprobanteAsociadoQuery(_TesoreriSelected.NombreConsecutivo, _TesoreriSelected.IDDocumento, Program.Usuario, Program.HashConexion), AddressOf TerminaValidarRelacionGMF, strEstado)
                    Exit Sub
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación del GMF",
                                 Me.ToString(), "ValidarGMF", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Recibe la respuesta si el CE seleccionado tiene un GMF relacionado.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130201</remarks>
    Private Sub TerminaValidarRelacionGMF(ByVal lo As LoadOperation(Of ValidarDocumentoGMF))
        IsBusy = False
        If Not lo.HasError Then
            If lo.Entities.Count > 0 Then
                Select Case moduloTesoreria
                    Case ClasesTesoreria.CE.ToString
                        'A2Utilidades.Mensajes.mostrarMensaje("El comprobante de egreso no se puede modificar, esta relacionado con una nota GMF" & vbCr & "Documento: " _
                        '                             & lo.Entities.First.strDescripcion & "  Número: " & lo.Entities.First.lngIDDocumentoNotaGMF & vbCr & "Solo se puede anular el documento y automaticamente se anula la nota GMF", _
                        '                             Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        A2Utilidades.Mensajes.mostrarMensaje("No se puede modificar el registro. Hay un recibo de caja asociado al comprobante" & vbCr & "Documento: " _
                                                     & lo.Entities.First.strDescripcion & "  Número: " & lo.Entities.First.lngIDDocumentoNotaGMF & vbCr & "Solo se puede anular el documento y automaticamente se anula el recibo de caja.",
                                                     Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Case ClasesTesoreria.RC.ToString
                        A2Utilidades.Mensajes.mostrarMensaje("El recibo de caja no se puede " & lo.UserState.ToString & ", está asociado al comprobante de egreso con consecutivo " _
                                                             & lo.Entities.First.strDescripcion & " y número de documento " & lo.Entities.First.lngIDDocumentoNotaGMF, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Case ClasesTesoreria.N.ToString
                        'If intNroValidacionGMF_NC = 0 Then
                        '    A2Utilidades.Mensajes.mostrarMensaje("La nota contable no se puede modificar, esta relacionado con una nota GMF" & vbCr & "Documento: " _
                        '                             & lo.Entities.First.strDescripcion & "  Número: " & lo.Entities.First.lngIDDocumentoNotaGMF & vbCr & "Solo se puede anular el documento y automaticamente se anula la nota GMF.", _
                        '                             Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'Else
                        A2Utilidades.Mensajes.mostrarMensaje("La nota contable no se puede " & lo.UserState.ToString & ", es una nota GMF, está asociada con el:" & vbCr & " - Documento: " _
                                                         & lo.Entities.First.strDescripcion & vbCr & " - Número: " & lo.Entities.First.lngIDDocumentoNotaGMF, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'End If
                End Select
            Else
                'If moduloTesoreria = ClasesTesoreria.N.ToString Then
                '    If intNroValidacionGMF_NC = 0 Then
                '        intNroValidacionGMF_NC = 1
                '        ValidarRelacionGMF("modificar")
                '        Exit Sub
                '    End If
                'End If

                Select Case lo.UserState.ToString
                    Case "modificar"
                        Editar(True, True)
                    Case "anular"
                        Borrar(True, True)
                End Select
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación del GMF",
                                             Me.ToString(), "TerminaValidarRelacionGMF", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Anular documento de tesorería (CE,NC y RC)"

    Public Overrides Async Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_TesoreriSelected) And Not ListaTesoreria.Count = 0 Then
                IsBusy = True
                ControlMensaje = False
                If Await ValidarEstadoDocumento(ACCIONES_ESTADODOCUMENTO.BORRAR) Then
                    ConsultarFechaCierreSistema("BORRAR")
                Else
                    IsBusy = False
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
                                 Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarBorradoDocumento()
        Try
            If Not IsNothing(fechaCierre) Then
                If _TesoreriSelected.Documento.ToShortDateString <= fechaCierre Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("El registro no se puede borrar porque la fecha del documento es menor o igual a la fecha de cierre (" & fechaCierre.Value.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If moduloTesoreria = "N" Then
                ValidarUsuario("Tesoreria_Notas", "Borrar")
            ElseIf moduloTesoreria = "CE" Then
                ValidarUsuario("Tesoreria_ComprobantesEgreso", "Borrar")
            ElseIf moduloTesoreria = "RC" Then
                ValidarUsuario("Tesoreria_RecibosCaja", "Borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
                                 Me.ToString(), "ContinuarBorradoDocumento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para permitir que un documento quede nulo.
    ''' </summary>
    ''' <remarks>
    ''' Funcion          : Borrar()
    ''' Se encarga de    : Anular un registro validando si el usuario tiene permisos y el documento se pueda anular.
    ''' Modificado por   : Juan David Correa Perez.
    ''' Descripción      : Se incluye codigo para habilitar anular un registro.
    ''' Fecha            : Agosto 16/2011 2:40 pm
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Agosto 16/2011 - Resultado Ok    
    ''' </remarks>
    Public Sub Borrar(Optional ByVal _mlogValidarEstado As Boolean = False, Optional ByVal _mlogValidarRelacionGMF As Boolean = False)
        Try
            If Not _mlogValidarEstado Then
                ValidarEstado("borrar")
                Exit Sub
            End If

            If Not _mlogValidarRelacionGMF Then
                Select Case moduloTesoreria
                    Case ClasesTesoreria.RC.ToString
                        ValidarRelacionGMF("anular")
                        Exit Sub
                    Case ClasesTesoreria.N.ToString
                        If _mstrAnularNotaGMF.Equals("SI") Then
                            intNroValidacionGMF_NC = 1
                            ValidarRelacionGMF("anular")
                            Exit Sub
                        End If
                End Select
            End If

            If TesoreriSelected.Por_Aprobar = "Rechazado" Then
                A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede editar porque ya fue Rechazado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            ElseIf TesoreriSelected.Por_Aprobar = Nothing Then
                Try
                    If Not IsNothing(_TesoreriSelected) Then
                        If TesoreriSelected.Estado = "I" Then
                            cwAnularDocmuento = New cwMotivoAnulacion()
                            AddHandler cwAnularDocmuento.Closed, AddressOf CerroVentanaAnularDocumento
                            Program.Modal_OwnerMainWindowsPrincipal(cwAnularDocmuento)
                            cwAnularDocmuento.ShowDialog()
                            'TesoreriSelected.Estado = "A"
                            'TesoreriSelected.Aprobacion = 0
                            'IsBusy = True
                            'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "update")
                        ElseIf TesoreriSelected.Estado = "A" Then
                            A2Utilidades.Mensajes.mostrarMensaje("El documento ya se encuentra anulado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede anular porque se encuentra pendiente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                Catch ex As Exception
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular el documento",
                     Me.ToString(), "AnularDocumento", Application.Current.ToString(), Program.Maquina, ex)
                End Try
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede anular porque está pendiente de una aprobación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar el registro",
                                 Me.ToString(), "Borrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Resultado del Child Windows de la Causal de Anulación.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB20120228</remarks>
    Private Sub CerroVentanaAnularDocumento(sender As Object, e As EventArgs)
        Try
            If cwAnularDocmuento.DialogResult.Value Then
                _strCausalInactivadad = cwAnularDocmuento.CausalInactividad
                Anular()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana del las Ordenes Pendientes de Tesoreria",
                     Me.ToString(), "CerroVentanaOrdenesPendientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para anular el documento de Tesoreria
    ''' </summary>
    ''' <remarks>SLB20130228</remarks>
    Private Sub Anular()
        Try
            IsBusy = True
            dcProxy.AnularTesoreria(_TesoreriSelected.IDTesoreria, _strCausalInactivadad, Program.Usuario, Program.HashConexion, AddressOf TerminoAnularTesoreria, "anular")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Anular el registro",
                                 Me.ToString(), "Anular", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Recibe el resultado de la Anulación del documente de Tesorería
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130228</remarks>
    Private Sub TerminoAnularTesoreria(ByVal lo As InvokeOperation(Of Boolean))
        IsBusy = False
        If Not lo.HasError Then
            If lo.Value = "1" Then
                TesoreriAnterior = Nothing
                dcProxy.Tesoreris.Clear()
                dcProxy.Load(dcProxy.TesoreriaFiltrarQuery("", moduloTesoreria, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "insert") ' Recarga la lista para que carguen los include
            Else
                _TesoreriSelected.Estado = "A"
                _TesoreriSelected.CausalInactividad = _strCausalInactivadad
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoAnularTesoreria" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

#End Region

#Region "Busqueda de Comitente desde el control de la vista"

    ''' <summary>
    ''' Buscar los datos del comitente que tiene asignada la Tesoreria.
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks>SLB20130122</remarks>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")

        Dim strIdComitente As String = String.Empty

        Try
            If Not Me.TesoreriSelected Is Nothing Then
                If Not strIdComitente.Equals(Me.TesoreriSelected.IdComitente) Then
                    If pstrIdComitente.Trim.Equals(String.Empty) Then
                        strIdComitente = Me.TesoreriSelected.IdComitente
                    Else
                        strIdComitente = pstrIdComitente
                    End If

                    If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorClientes.Clear()
                        objProxy.Load(objProxy.buscarClienteEspecificoCompaniaQuery(strIdComitente, Program.Usuario, IIf(pstrBusqueda = "encabezado", "IdComitente_condigito", "IdComitente"), True, _TesoreriSelected.IDCompania, Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130122</remarks>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Select Case lo.UserState.ToString
                        Case "encabezado"
                            _TesoreriSelected.IdComitente = String.Empty
                            listCuentasbancarias.Clear()
                            _TesoreriSelected.CuentaCliente = String.Empty
                            'JFSB 20171019
                            CuentaContableCliente = String.Empty

                        Case "detalle"
                            Me.ComitenteSeleccionadoDetalle(Nothing)
                    End Select
                ElseIf lo.Entities.ToList.Item(0).TipoVinculacion = "O" And lo.UserState.ToString = "detalle" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente en el detalle no puede ser un Ordenante ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Me.ComitenteSeleccionadoDetalle(Nothing)
                Else
                    Select Case lo.UserState.ToString
                        Case "encabezado"
                            If Me.logReasignar Then
                                Me.ComitenteSeleccionadoM(lo.Entities.ToList.Item(0), Me.logReasignar)
                            Else
                                Me.ComitenteSeleccionadoM(lo.Entities.ToList.Item(0))
                            End If
                        Case "detalle"
                            Me.ComitenteSeleccionadoDetalle(lo.Entities.ToList.FirstOrDefault)
                    End Select
                End If
            Else
                'Me.ComitenteSeleccionado(Nothing)
                A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Select Case lo.UserState.ToString
                    Case "encabezado"
                        _TesoreriSelected.IdComitente = String.Empty
                        listCuentasbancarias.Clear()
                        _TesoreriSelected.CuentaCliente = String.Empty
                        'JFSB 20171019
                        CuentaContableCliente = String.Empty
                    Case "detalle"
                        Me.ComitenteSeleccionadoDetalle(Nothing)
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Property objComitenteEncabezado As OYDUtilidades.BuscadorClientes

    ''' <summary>
    ''' Método para asignar los valores correspodientes del comitente seleccionado
    ''' </summary>
    ''' <param name="pobjComitente"></param>
    ''' <remarks>SLB20130122</remarks>
    ''' Modificado por   : Yessid Andrés Paniagua Pabón
    ''' Descripción      : Se verifica que ListaDetalleTesoreria no venga en NULL.
    ''' Fecha            : Abril 04/2016 
    ''' Pruebas CB       : Yessid Andrés Paniagua Pabón - Abril 04/2016 - Resultado Ok   
    ''' ID del Cambio    : YAPP20160404

    Public Sub ComitenteSeleccionadoM(ByVal pobjComitente As OYDUtilidades.BuscadorClientes, Optional logReasignar As Boolean = False)

        'If logReasignar Then
        '    pobjComitente = objComitenteEncabezado
        'End If

        If Not IsNothing(pobjComitente) Then

            If logReasignar = 0 Then
                objComitenteEncabezado = pobjComitente
            End If
            Dim Existe As Boolean = False

            If Not IsNothing(ListaDetalleTesoreria) Then 'YAPP20160404 If ListaDetalleTesoreria.Count = 0 Then
                If ListaDetalleTesoreria.Count > 0 Then
                    Existe = True
                End If
            End If

            CambiarPropiedades = False
            _TesoreriSelected.IdComitente = pobjComitente.IdComitente
            _TesoreriSelected.Nombre = pobjComitente.Nombre
            _TesoreriSelected.NroDocumento = pobjComitente.NroDocumento
            _TesoreriSelected.TipoIdentificacion = pobjComitente.CodTipoIdentificacion
            _TesoreriSelected.TipoVinculacion = pobjComitente.TipoVinculacion
            CuentaContableCliente = String.Empty
            listCuentasbancarias.Clear()
            _TesoreriSelected.CuentaCliente = Nothing
            'SLB20130611 Forma de Pago y Tipo Cuenta solo para CE
            If moduloTesoreria = ClasesTesoreria.CE.ToString And logReasignar = False Then
                _TesoreriSelected.TipoCuenta = pobjComitente.TipoCuenta
                _TesoreriSelected.FormaPagoCE = pobjComitente.CodFormaPago
                'JFSB 20161005 Ajuste para cuando la forma de pago del cliente seleccionado sea Cheque
                If pobjComitente.CodFormaPago = "C" Then
                    HabilitaTipocheque = True
                Else
                    HabilitaTipocheque = False
                    If Not String.IsNullOrEmpty(_TesoreriSelected.Tipocheque) Then
                        _TesoreriSelected.Tipocheque = Nothing
                    End If
                End If

                If _TesoreriSelected.FormaPagoCE = "T" Then

                    HabilitaTipocheque = False
                    TesoreriSelected.Tipocheque = Nothing
                    objProxy1.ItemCombos.Clear()
                    objProxy1.Load(objProxy1.cargarCombosEspecificos_ConUsuarioQuery("Tesoreria_CuentasBancarias", TesoreriSelected.IdComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoatraerCuentasbancarias, Nothing)
                    MyBase.CambioItem("TesoreriSelected")

                End If

            End If
            CambiarPropiedades = True
            If moduloTesoreria <> ClasesTesoreria.N.ToString Then

                'JFSB 20160812 Se llena la variable con lo seleccionado en el encabezado del comitente del encabezado
                ClienteAnterior = pobjComitente
            End If

            HabilitarEdicion = False

            If Not Existe Then
                'NombreColeccionDetalle = "cmDetalleTesoreri"
                NuevoDetalle()
            End If

            AdicionarCuentaContable_Detalle()
            Me.logReasignar = False
        End If

    End Sub

    ''' <summary>
    ''' Método para asignar los valores correspodientes del comitente seleccionado en el detalle
    ''' </summary>
    ''' <param name="pobjComitente"></param>
    ''' <remarks>SLB20130312</remarks>
    Public Sub ComitenteSeleccionadoDetalle(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                _DetalleTesoreriSelected.IDComitente = pobjComitente.IdComitente
                _DetalleTesoreriSelected.Nombre = pobjComitente.Nombre
                _mlogBuscarNIT = False
                _DetalleTesoreriSelected.NIT = pobjComitente.NroDocumento
                _DetalleTesoreriSelected.TipoVinculacion = pobjComitente.TipoVinculacion
                _mlogBuscarNIT = True
                Edicion = False
                'ValidarSaldoCliente()
            Else
                _DetalleTesoreriSelected.IDComitente = String.Empty
                _DetalleTesoreriSelected.Nombre = String.Empty
                _DetalleTesoreriSelected.NIT = String.Empty
            End If
            If moduloTesoreria = ClasesTesoreria.N.ToString Then _DetalleTesoreriSelected.IDBanco = Nothing
            AdicionarCuentaContable_Detalle()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al actualizar los datos del cliente.", Me.ToString(), "ComitenteSeleccionadoDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejo de la cuenta contable desde Instalación.
    ''' </summary>
    ''' <remarks>SLB</remarks>
    Public Sub AdicionarCuentaContable_Detalle()
        Try
            If Not String.IsNullOrEmpty(strCuenta) And Not IsNothing(_DetalleTesoreriSelected) Then
                _mlogBuscarCuentaContable = False
                If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                    If _TesoreriSelected.IDCompania <> intIDCompaniaFirma Then
                        _DetalleTesoreriSelected.IDCuentaContable = String.Empty
                    Else
                        _DetalleTesoreriSelected.IDCuentaContable = strCuenta
                    End If
                Else
                    _DetalleTesoreriSelected.IDCuentaContable = strCuenta
                End If

                _mlogBuscarCuentaContable = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar la cuenta contable.", Me.ToString(), "AdicionarCuentaContable_Detalle", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Validaciones en el cliente de NC, RC y CE"
    'Función de validación del Detalle de Notas Contables
    'Devuelve un Boolean. True=Permitir Guardar - False=Permitir Guardar
    Public Function ValidarNotas() As Boolean
        Try
            Dim Index As Integer
            Dim Existe As Boolean
            ReDim vlrTotalXCliente(0)
            'Se realizan las validaciones especificas de Notas Tesoreria
            Dim sumaDebitos As Decimal
            Dim sumaCreditos As Decimal
            Dim CountBancosDetalle As Integer = 0
            Dim logtienecuentaPYG As Boolean = False
            Dim logtienecuentaBalance As Boolean = False

            intBanco = -1
            mlogclienteGMF = False

            For Each ld In ListaDetalleTesoreria
                If IsNothing(ld.Detalle) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el detalle del documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    NotaGMFManual = False
                    Return False
                End If
                'Modificado por Natalia Andrea Otalvaro 31 Agosto 2015
                'Se quita la validación para que permita grabar detalles con el debito y credito en cero como en vb. toro
                '********************************************************************************************************
                If (IsNothing(ld.Credito) Or ld.Credito = 0) And (IsNothing(ld.Debito) Or ld.Debito = 0) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un valor Débito o Crédito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    NotaGMFManual = False
                    Return False
                End If
                '********************************************************************************************************
                'SLB20130712 En VB.6 esta dejando grabar detalles sin comitente ni código.
                'If IsNothing(ld.IDComitente) And IsNothing(ld.IDBanco) Then
                '    A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar código de cliente o código de banco.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    Return False
                'End If
                If (String.IsNullOrEmpty(ld.IDComitente) Or String.IsNullOrEmpty(ld.Nombre)) And mlogClientes Then 'SLB20140626 Se adiciona validación contra la variable mlogClientes
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el cliente en el detalle de la nota.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                    NotaGMFManual = False
                    Return False
                End If

                If (ld.TipoVinculacion = "O") Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente " + ld.IDComitente + " en el detalle no puede ser un Ordenante ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                    NotaGMFManual = False
                    Return False
                End If

                'Validación del Centro de Costos y la Cuenta Contable JRP 2012/18/07                
                If String.IsNullOrEmpty(ld.IDCuentaContable) And mlogCuenta Then 'SLB20140626 Se adiciona validación contra la variable mlogCuenta
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar una Cuenta Contable en el detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    NotaGMFManual = False
                    Return False
                End If

                'JABG 20151026l 
                ld.NombreConsecutivo = _TesoreriSelected.NombreConsecutivo

                'Modificado por Natalia Andrea Otalvaro 31 Agosto 2015
                'Se quita la validación para que permita grabar detalles con el debito y credito en cero como en vb.
                '********************************************************************************************************
                'Lleva el valor debito o credito al valor del detalle
                If (IsNothing(ld.Debito) Or ld.Debito = 0) And (IsNothing(ld.Credito) Or ld.Credito = 0) Then
                    ld.Valor = 0
                    ld.Tipo = "ND"
                Else
                    If Not IsNothing(ld.Debito) Then
                        If ld.Debito <> 0 Then
                            ld.Valor = ld.Debito
                            sumaDebitos += ld.Debito
                            ld.Tipo = "ND"
                        End If
                    End If
                    If Not IsNothing(ld.Credito) Then
                        If ld.Credito <> 0 Then
                            ld.Valor = ld.Credito
                            sumaCreditos += ld.Credito
                            ld.Tipo = "NC"
                        End If
                    End If
                End If

                'validar cobro gmf JBT
                If logCobrarGMF Then
                    'Evaluar si hay un cliente al que se le cobre GMF
                    If Not IsNothing(ld.IDComitente) And Not IsNothing(ld.Debito) And Not IsNothing(ld.Nombre) Then
                        mlogclienteGMF = True
                        _TesoreriSelected.ClienteGMF = True
                    End If
                    'Evaluar si hay un banco al que se le cobre 
                    If Not IsNothing(ld.IDBanco) And Not IsNothing(ld.Nombre) Then
                        mlogBancoGMF = True
                        CountBancosDetalle = CountBancosDetalle + 1
                        _TesoreriSelected.BancoGMF = True
                        If intBanco = -1 Then
                            intBanco = ld.IDBanco
                        End If
                    End If

                    If (mlogclienteGMF) And CountBancosDetalle > 1 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Solo se puede tener un solo banco cuando se cobra GMF", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        mlogBancoGMF = False
                        mlogclienteGMF = False
                        _TesoreriSelected.BancoGMF = False
                        _TesoreriSelected.ClienteGMF = False
                        intBanco = -1
                        NotaGMFManual = False
                        Return False
                    End If
                    If (Not IsNothing(ld.IDBanco) And IsNothing(ld.Nombre)) Or (Not IsNothing(ld.IDComitente) And IsNothing(ld.Nombre)) Then
                        mlogclienteGMF = False
                        mlogBancoGMF = False
                        _TesoreriSelected.BancoGMF = False
                        _TesoreriSelected.ClienteGMF = False
                    End If

                    'JABG 20151022
                    If (IsNothing(ld.IDBanco) And (Trim(ld.IDComitente)) = "" And Not IsNothing(ld.IDCuentaContable) And Not IsNothing(ld.Debito)) Then
                        mlogclienteGMF = False
                        mlogBancoGMF = False
                        _TesoreriSelected.BancoGMF = False
                        _TesoreriSelected.ClienteGMF = False
                    End If

                    'JABG 20151022
                    If Not glogConceptoDetalleTesoreriaManual And Not IsNothing(ld.IDConcepto) And IsNothing(ld.IDBanco) And Trim(ld.IDComitente) = "" And Not IsNothing(ld.Debito) Then
                        mlogclienteGMF = False
                        mlogBancoGMF = False
                        _TesoreriSelected.BancoGMF = False
                        _TesoreriSelected.ClienteGMF = False
                    End If

                    'JABG 20151022
                    If mstrNombreConsecutivoNota.Trim = ld.NombreConsecutivo Or mstrTipoNotasCxC.Trim = ld.NombreConsecutivo Then
                        mlogclienteGMF = False
                        mlogBancoGMF = False
                        _TesoreriSelected.BancoGMF = False
                        _TesoreriSelected.ClienteGMF = False
                    End If

                End If

                Existe = False 'Permite evaluar los registros del arreglo.
                For Index = 0 To UBound(vlrTotalXCliente) 'Recorre el arreglo
                    'Si el cliente existe acumula lo que tiene en DB o CR
                    If Not IsNothing(ld.IDComitente) Then
                        If vlrTotalXCliente(Index).lngIDComitente = Trim(ld.IDComitente) Then
                            vlrTotalXCliente(Index).VlrCR = vlrTotalXCliente(Index).VlrCR + Val(ld.Credito.ToString())
                            vlrTotalXCliente(Index).VlrDB = vlrTotalXCliente(Index).VlrDB + Val(ld.Debito.ToString())
                            vlrTotalXCliente(Index).VlrSaldo = IIf(IsNothing(ld.SaldoCliente), 0, ld.SaldoCliente) 'SLB20130522
                            Existe = True
                            Exit For
                        End If
                    End If
                Next
                If Not Existe Then 'Si no encontro el cliente agrega un nuevo item al arreglo
                    ReDim Preserve vlrTotalXCliente(UBound(vlrTotalXCliente) + 1)
                    Index = UBound(vlrTotalXCliente)
                    IndexPosicion = Index
                    'Llena los valores en el cliente
                    If Not IsNothing(ld.IDComitente) Then
                        vlrTotalXCliente(Index).lngIDComitente = Trim(ld.IDComitente.ToString())
                    Else
                        vlrTotalXCliente(Index).lngIDComitente = String.Empty
                    End If
                    vlrTotalXCliente(Index).VlrCR = Val(ld.Credito.ToString())
                    vlrTotalXCliente(Index).VlrDB = Val(ld.Debito.ToString())
                    vlrTotalXCliente(Index).VlrSaldo = IIf(IsNothing(ld.SaldoCliente), 0, ld.SaldoCliente) 'SLB20130522
                End If

            Next

            'Se comparan las Notas Debito y Credito para segurarse de que las sumas de las mismas sean iguales.
            If sumaDebitos <> sumaCreditos And TesoreriSelected.Contabilidad Then
                Dim strTemp As String
                strTemp = "La suma de los débitos es diferente a la suma de los Créditos. " & vbCrLf & vbCrLf
                strTemp = strTemp & "ND= " & Format(sumaDebitos, "$##,##0.00") & vbCrLf
                strTemp = strTemp & "NC= " & Format(sumaCreditos, "$##,##0.00") & vbCrLf
                strTemp = strTemp & "________" & vbCrLf
                strTemp = strTemp & "DIF= " & Format(sumaCreditos - sumaDebitos, "$##,##0.00")
                A2Utilidades.Mensajes.mostrarMensaje(strTemp, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            'For Each ltb In ListaTraerBolsas
            '    glogValSobregiroNC = IIf(ltb.logValSobregiroNC, True, False)
            'Next

            'If glogValSobregiroNC Then
            '    'Se valida que el cliente tenga saldo disponible
            '    validarparametrosGMF()
            '    Return False
            'End If

            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar la validación de las notas contables",
                                 Me.ToString(), "ValidarNotas", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    'Metodo de validación del Detalle de Recibos de Caja
    'Devuelve un Boolean. True=Permitir Guardar - False=Permitir Guardar
    Public Function ValidarRecibosCaja() As Boolean
        Try
            Dim sumarDebitos As Double
            Dim sumarCreditos As Double

            'Valida el Recibo de tesoreria
            If IsNothing(TesoreriSelected.Nombre) Or TesoreriSelected.Nombre = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el nombre del beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            'SLB2013028 
            If IsNothing(_TesoreriSelected.TipoIdentificacion) Or _TesoreriSelected.TipoIdentificacion = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el tipo de identificación del beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If
            If IsNothing(_TesoreriSelected.NroDocumento) Or _TesoreriSelected.NroDocumento = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número del documento del beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            'Valida la lista de Detalle tesoreria
            For Each ld In ListaDetalleTesoreria

                If (String.IsNullOrEmpty(ld.IDComitente) Or String.IsNullOrEmpty(ld.Nombre)) And mlogClientes Then 'SLB20140626 Se adiciona validación contra la variable mlogClientes
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el cliente en el detalle del recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                    Return False
                End If

                If (ld.TipoVinculacion = "O") Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente " + ld.IDComitente + " en el detalle no puede ser un Ordenante ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                    Return False
                End If
                'JFGB20160511
                If plogEsFondosOYD Then
                    If _TesoreriSelected.IDCompania <> intIDCompaniaFirma Then
                        If IsNothing(ld.IDConcepto) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el concepto en el detalle del recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return False
                            Exit Function
                        End If
                    End If
                End If
                'JFGB20160511
                If plogEsFondosOYD = False Then
                    If (String.IsNullOrEmpty(ld.IDComitente) Or String.IsNullOrEmpty(ld.Nombre)) And mlogClientes Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un cliente en el detalle del recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return False
                        Exit Function
                    End If
                Else
                    If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                        'SLB20140626 Se habilita esta validación y queda como VB.6
                        If (String.IsNullOrEmpty(ld.IDComitente) Or String.IsNullOrEmpty(ld.Nombre)) And mlogClientes Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un cliente en el detalle del recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return False
                            Exit Function
                        End If
                    Else
                        If ld.ManejaCliente = "S" Then
                            If (String.IsNullOrEmpty(ld.IDComitente) Or String.IsNullOrEmpty(ld.Nombre)) Then
                                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un cliente en el detalle del recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Return False
                                Exit Function
                            End If
                        ElseIf ld.ManejaCliente = "N" Then
                            If Not String.IsNullOrEmpty(ld.IDComitente) Or Not String.IsNullOrEmpty(ld.Nombre) Then
                                A2Utilidades.Mensajes.mostrarMensaje("No se debe ingresar un cliente en el detalle del recibo por la configuración que tiene el concepto seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Return False
                                Exit Function
                            End If
                        End If

                    End If
                End If

                If IsNothing(ld.Detalle) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el detalle del recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                    Return False
                End If
                'Modificado por Natalia Andrea Otalvaro 31 Agosto 2015
                'Se quita la validación para que permita grabar detalles con el debito y credito en cero como en vb.
                '********************************************************************************************************
                'DEMC20180925 INICIO: Se consulta el parametro "PERMITIRVALORESCERORC" que permite validar si se realiza la validacion de valores en cero en RC.
                If logPermiteValorCeroRC = False Then
                    If VerValorCredito = Visibility.Collapsed Then
                        If IsNothing(ld.Debito) Or ld.Debito = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un valor para el detalle del recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                            Return False
                        End If
                    Else
                        If (IsNothing(ld.Debito) And IsNothing(ld.Credito)) Or (ld.Debito = 0 And ld.Credito = 0) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un valor débito o crédito para el detalle del recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                            Return False
                        End If
                    End If
                End If
                'DEMC20180925 FIN
                '********************************************************************************************************

                'Validación del Centro de Costos y la Cuenta Contable JRP 2012/18/07
                If String.IsNullOrEmpty(ld.IDCuentaContable) And mlogCuenta Then 'SLB20140626 Se adiciona validación contra la variable mlogCuenta
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar una Cuenta Contable en el detalle del recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                    Return False
                End If

                'Modificado por Natalia Andrea Otalvaro 31 Agosto 2015
                'Se quita la validación para que permita grabar detalles con el debito y credito en cero como en vb.
                '********************************************************************************************************
                'Lleva el valor debito o credito al valor del detalle
                If (IsNothing(ld.Debito) Or ld.Debito = 0) And (IsNothing(ld.Credito) Or ld.Credito = 0) Then
                    ld.Valor = 0
                Else
                    If Not IsNothing(ld.Debito) Then
                        If ld.Debito <> 0 Then
                            ld.Valor = ld.Debito
                            sumarDebitos += ld.Debito
                        End If
                    End If
                    If Not IsNothing(ld.Credito) Then
                        If ld.Credito <> 0 Then
                            ld.Valor = -ld.Credito
                            sumarCreditos += ld.Credito
                        End If
                    End If
                End If
                '********************************************************************************************************
            Next

            'Modificado por Natalia Andrea Otalvaro 31 Agosto 2015
            'Se quita la validación para que permita grabar detalles con el debito y credito en cero como en vb. toro
            '********************************************************************************************************
            If IsNothing(TotalRecibos) Or TotalRecibos = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El valor total del recibo no debe de ser 0.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If
            '********************************************************************************************************

            If TotalRecibos < 0 Then
                Dim strTemp As String
                strTemp = "El valor total del detalle del recibo no debe de ser negativo." & vbCrLf
                strTemp = strTemp & "Valor total Débito = " & Format(sumarDebitos, "$##,##0.00") & vbCrLf
                strTemp = strTemp & "Valor total Crédito = " & Format(sumarCreditos, "$##,##0.00") & vbCrLf
                strTemp = strTemp & "Valor total = " & Format(TotalRecibos, "$##,##0.00") & vbCrLf
                A2Utilidades.Mensajes.mostrarMensaje(strTemp, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            'Valida la lista de Cheques Tesoreria
            If Not IsNothing(ListaChequeTesoreria) AndAlso ListaChequeTesoreria.Count > 0 Then

                For Each ch In ListaChequeTesoreria
                    'Modificado por Natalia Andrea Otalvaro 31 Agosto 2015
                    'Se quita la validación para que permita grabar detalles con el debito y credito en cero como en vb.
                    '********************************************************************************************************
                    'If IsNothing(ch.Valor) Or ch.Valor = 0 Then
                    '    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar valor para el detalle del cheque.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    '    TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                    '    Return False
                    'End If
                    '********************************************************************************************************
                    If IsNothing(ch.FormaPagoRC) Or ch.FormaPagoRC = String.Empty Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar una forma de pago para el detalle del cheque.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                        Return False
                    End If

                    If Program.VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then 'Si es City
                        If ch.FormaPagoRC <> "C" And ch.FormaPagoRC <> "T" And ch.FormaPagoRC <> "E" Then
                            Dim strTemp As String
                            strTemp = "En el detalle de cheques solo se permiten las siguientes formas de pago. " & vbCrLf
                            strTemp = strTemp & "1. CHEQUE" & vbCrLf
                            strTemp = strTemp & "2. TRANSFERENCIA ELECTRÓNICA" & vbCrLf
                            strTemp = strTemp & "3. EFECTIVO" & vbCrLf
                            A2Utilidades.Mensajes.mostrarMensaje(strTemp, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return False
                        End If
                    End If

                    'SLB20130301 Validación de campos dependiedo si son obligatorios o no
                    CamposObligatorioSelected = ListaCamposObligatorios.Where(Function(l) l.lngID = ch.FormaPagoRC).FirstOrDefault

                    If Not IsNothing(CamposObligatorioSelected) Then
                        If ch.BancoGirador = String.Empty And CamposObligatorioSelected.strBancoGirador Then
                            A2Utilidades.Mensajes.mostrarMensaje("La forma de pago [" & CamposObligatorioSelected.strFormadePago & "] obliga a digitar el Banco Girador", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                            Return False
                        End If

                        If (ch.NumCheque = 0 Or IsNothing(ch.NumCheque)) And CamposObligatorioSelected.lngNumCheque Then
                            A2Utilidades.Mensajes.mostrarMensaje("La forma de pago [" & CamposObligatorioSelected.strFormadePago & "] obliga a digitar el número de cheque", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                            Return False
                        End If

                        If (IsNothing(ch.BancoConsignacion) Or ch.BancoConsignacion = 0) And CamposObligatorioSelected.lngBancoConsignacion Then
                            A2Utilidades.Mensajes.mostrarMensaje("La forma de pago [" & CamposObligatorioSelected.strFormadePago & "] obliga a elegir el banco de consignación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                            Return False
                        End If

                        If IsNothing(ch.Consignacion) And CamposObligatorioSelected.dtmConsignacion Then
                            A2Utilidades.Mensajes.mostrarMensaje("La forma de pago [" & CamposObligatorioSelected.strFormadePago & "] obliga digitar la fecha de consignación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                            Return False
                        End If

                        If ch.Comentario = String.Empty And CamposObligatorioSelected.strComentario Then
                            A2Utilidades.Mensajes.mostrarMensaje("La forma de pago [" & CamposObligatorioSelected.strFormadePago & "] obliga digitar las observaciones", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                            Return False
                        End If

                        If (IsNothing(ch.IdProducto) Or ch.IdProducto = 0) And CamposObligatorioSelected.lngidproducto Then
                            A2Utilidades.Mensajes.mostrarMensaje("La forma de pago [" & CamposObligatorioSelected.strFormadePago & "] obliga a elegir el tipo de producto", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                            Return False
                        End If
                    End If

                    'SLB20130131 El consecutivo del detalle debe ser igual al del Encabezado
                    ch.NombreConsecutivo = _TesoreriSelected.NombreConsecutivo
                Next

            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el detalle del cheque.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                Return False
            End If
            'CORREC_CITI_SV_2014
            'Modificado por Natalia Andrea Otalvaro 31 Agosto 2015
            'Se quita la validación para que permita grabar detalles con el debito y credito en cero como en vb.
            'IsNothing(TotalCheques) Or 
            '********************************************************************************************************
            If Math.Round(TotalCheques, 2) <> Math.Round(TotalRecibos, 2) Then
                Dim strTemp As String
                strTemp = "El valor total del Detalle del Cheque debe ser igual que el valor total de los recibos." & vbCrLf
                strTemp = strTemp & "Valor total recibos = " & Format(TotalRecibos, "$##,##0.00") & vbCrLf
                strTemp = strTemp & "Valor total cheques = " & Format(TotalCheques, "$##,##0.00") & vbCrLf
                A2Utilidades.Mensajes.mostrarMensaje(strTemp, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If
            '********************************************************************************************************

            'SLB20130802 - Jorge Arango Sept 4/2013: Busca si al menos hay una forma de pago cheque, si la hay se valida el control de canje si este esta habilitado, de lo contrario no
            Dim logValidarCanje As Boolean = False
            logValidarCanje = False
            For Each ld In ListaChequeTesoreria
                If ld.FormaPagoRC = "C" Then
                    logValidarCanje = True
                    Exit For
                End If
            Next

            'JAG 20150423 Se comenta debido a que con el ajuste del prorrateo de valores en canje ya no son necesarias estas validaciones
            ''SLB20130802 Control Canje Cheques
            If _mlogCanjeCheque And logValidarCanje Then

                If ListaDetalleTesoreria.Count > 1 Then 'Detalle Recibos de Caja
                    Dim strIDComitente = ListaDetalleTesoreria.First.IDComitente
                    For Each ld In ListaDetalleTesoreria
                        If LTrim(strIDComitente) <> LTrim(ld.IDComitente) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Cuando está habilitado el control de canje de cheques, no es posible ingresar códigos de clientes diferentes. Por Favor Verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                            Return False
                        End If
                    Next
                End If

                If ListaChequeTesoreria.Count > 1 Then 'Cheques
                    Dim dtmConsignacion = ListaChequeTesoreria.First.Consignacion
                    Dim strFormaPago = ListaChequeTesoreria.First.FormaPagoRC

                    For Each ld In ListaChequeTesoreria
                        If dtmConsignacion <> ld.Consignacion Or strFormaPago <> ld.FormaPagoRC Then
                            A2Utilidades.Mensajes.mostrarMensaje("Cuando está habilitado el control de canje de cheques, la fecha de consignación de los cheques y la forma de pago deben ser iguales. Por Favor Verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                            Return False
                        End If
                        'If strFormaPago <> ld.FormaPagoRC Then
                        '    A2Utilidades.Mensajes.mostrarMensaje("Cuando esta habilitado el control de canje de cheques, la fecha de consignacion de los cheques y la forma de pago deben ser iguales. Por Favor Verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        '    TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                        '    Return False
                        'End If
                    Next
                End If

            End If
            'JAG 20150423

            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar la validación de las recibos de caja",
                                 Me.ToString(), "ValidarRecibosCaja", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Valida el ingreso de los datos que son obligatorios en Comprobantes de Egreso.
    ''' </summary>
    ''' <returns>True = Si todos los datos son correctos - False = Si algun dato no es correcto</returns>
    ''' <remarks>
    ''' Desarrollado por : Juan Carlos Soto Cruz.
    ''' Fecha            : Agosto 06/2011
    ''' </remarks>
    Private Function DatosValidosCE() As Boolean
        Try
            Dim Index As Integer
            Dim Existe As Boolean
            ReDim vlrTotalXCliente(0)

            DatosValidosCE = True

            'Se valida fecha de cierre
            If Not IsNothing(fechaCierre) Then
                If TesoreriSelected.Documento.ToShortDateString <= fechaCierre Then
                    A2Utilidades.Mensajes.mostrarMensaje("El documento con fecha " & fechaCierre.Value.ToShortDateString() & " no puede ser ingresado o modificado porque la fecha es inferior a la fecha de cierre registrada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    DatosValidosCE = False
                    Exit Function
                End If
            End If

            'Se valida tipo (Esta validacion quedo en metadata)
            'Se valida que sea valida la fecha (Esta validacion quedo en metadata)

            'Se valida banco
            'JFSB 20171006 Se ajusta validación
            If IsNothing(TesoreriSelected.IDBanco) Or String.IsNullOrEmpty(TesoreriSelected.NombreBco) Then
                DatosValidosCE = False
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un Banco en el encabezado del documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            'Se valida Cheque
            If (TesoreriSelected.NumCheque = 0 Or IsNothing(_TesoreriSelected.NumCheque)) And Not TesoreriSelected.ChequeraAutomatica And TesoreriSelected.FormaPagoCE = "C" Then
                DatosValidosCE = False
                VisibilidadNumCheque = True
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un número de cheque en el encabezado del documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            'Santiago Vergara - Octubre 24/2013 - Se adiciona validacion del documento del beneficiario
            If _TesoreriSelected.NroDocumento = "" Then
                DatosValidosCE = False
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número del documento de identificación del beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            'SLB20130211 Validaciones del Banco de Destino
            If _mstrActivarBancoTraslado = "SI" Then
                _mlogSeleccioNoCliente = False
                'Se verifica si se seleccionó un cliente en el detalle
                For Each ld In ListaDetalleTesoreria
                    If (ld.TipoVinculacion = "O") Then
                        A2Utilidades.Mensajes.mostrarMensaje("El comitente " + ld.IDComitente + " en el detalle no puede ser un Ordenante ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                        Return False
                    End If
                    If Not String.IsNullOrEmpty(ld.IDComitente) Then
                        _mlogSeleccioNoCliente = True
                    End If
                Next

                'JFGB20160511
                If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                    If IsNothing(_TesoreriSelected.lngBancoConsignacion) And Not _mlogSeleccioNoCliente Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un cliente en el detalle del comprobante o un banco de destino.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DatosValidosCE = False
                        Exit Function
                        'Si no se seleccionó el banco destino solo se validan los datos del cliente
                    ElseIf IsNothing(_TesoreriSelected.lngBancoConsignacion) Then
                        _mlogSeleccioNoCliente = True
                    End If
                End If

                'Valida si se seleccionó un bando de destino
                If Not IsNothing(_TesoreriSelected.lngBancoConsignacion) Then

                    'Se evalúa si ya esta configurado el consecutivo para el recibo de caja cuando se hace un traslado 
                    If _mstrConsecutivoTrasladoBancos.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado el consecutivo para Traslado entre bancos. Configure el valor del parámetro 'CONSECUTIVOTRASLADOBANCOS' ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DatosValidosCE = False
                        Exit Function
                    End If

                    'Se evalúa ademas de haber seleccionado el banco de destino tambien se selecciono un cliente en el detalle
                    If _mlogSeleccioNoCliente Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se puede ingresar al mismo tiempo el cliente y el banco de destino.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DatosValidosCE = False
                        Exit Function
                    End If

                    'Se evalúa si el banco girador es el mismo banco de destino
                    If _TesoreriSelected.IDBanco = _TesoreriSelected.lngBancoConsignacion Then
                        A2Utilidades.Mensajes.mostrarMensaje("El banco de destino no puede ser el mismo banco girador.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DatosValidosCE = False
                        Exit Function
                    End If

                    'Se valida que la forma de pago sea cheque
                    If _TesoreriSelected.FormaPagoCE <> "C" Then
                        A2Utilidades.Mensajes.mostrarMensaje("Para hacer un traslado entre bancos la forma de pago debe ser cheque.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DatosValidosCE = False
                        Exit Function
                    End If
                    _TesoreriSelected.TrasladoEntreBancos = True

                End If
            End If



            'Se valida Beneficiario
            If String.IsNullOrEmpty(TesoreriSelected.Nombre) Then
                DatosValidosCE = False
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el nombre del beneficiario del cheque en el encabezado del documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            'Se valida forma de pago
            If IsNothing(TesoreriSelected.FormaPagoCE) Then
                DatosValidosCE = False
                A2Utilidades.Mensajes.mostrarMensaje(" Debe seleccionar una forma de pago en el encabezado del documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            If Program.VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then
                If TesoreriSelected.FormaPagoCE = "T" Then
                    If (Not TransferenciaDisponible) Then
                        DatosValidosCE = False
                        A2Utilidades.Mensajes.mostrarMensaje("La opción de transferencia electrónica no se encuentra disponible para este cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Function
                    End If
                ElseIf TesoreriSelected.FormaPagoCE = "C" Then
                    'Se valida sucursal
                    If Program.VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then 'Si es City
                        If String.IsNullOrEmpty(TesoreriSelected.SucursalBancaria) And TesoreriSelected.FormaPagoCE = "C" Then
                            DatosValidosCE = False
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la sucursal bancaria en el encabezado del documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Function
                        End If
                    End If

                Else
                    DatosValidosCE = False
                    A2Utilidades.Mensajes.mostrarMensaje("El Cliente elegido debe tener, como forma de pago, Cheque o Transferencia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
            End If
            If _TesoreriSelected.FormaPagoCE = "C" And String.IsNullOrEmpty(_TesoreriSelected.Tipocheque) Then
                DatosValidosCE = False
                A2Utilidades.Mensajes.mostrarMensaje("El tipo de cheque es un dato requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If
            If VisibleEspecificoCity = Visibility.Collapsed And _TesoreriSelected.FormaPagoCE = "T" And String.IsNullOrEmpty(_TesoreriSelected.CuentaCliente) Then
                DatosValidosCE = False
                A2Utilidades.Mensajes.mostrarMensaje("La cuenta bancaria del cliente es un dato requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If



            'Se valida Detalle (Esta validacion quedo en otro metodo)

            'Se valida que el numero del cheque no sea menor que el asignado en el consecutivo del banco.
            If Not (TesoreriSelected.NumCheque = 0 Or IsNothing(_TesoreriSelected.NumCheque)) And Not TesoreriSelected.ChequeraAutomatica Then
                If Not IsNothing(ListaConsecutivoBanco) Then
                    If ListaConsecutivoBanco.Count > 0 Then
                        For Each lcb In ListaConsecutivoBanco
                            If (TesoreriSelected.NumCheque <= lcb.Actual) Then
                                DatosValidosCE = False
                                A2Utilidades.Mensajes.mostrarMensaje("El número del cheque no puede ser menor que el asignado en el consecutivo del banco.",
                                                                     Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Function
                            End If
                        Next
                    End If
                End If
            End If

            If ListaDetalleTesoreria.Count > 4 And TesoreriSelected.NumCheque = 0 And TesoreriSelected.ChequeraAutomatica And TesoreriSelected.FormaPagoCE = "C" Then
                DatosValidosCE = False
                A2Utilidades.Mensajes.mostrarMensaje("Existen más de 4 detalles para la chequera automática.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            'VALIDA QUE UNO DE LOS DETALLES TENGA UN VALOR EN LA PARTE DEL DEBITO SÍ ESTA HABILITADO LOS CAMPOS CRETIDO Y DEBITO
            If VerValorCredito = Visibility.Visible Then
                Dim logContieneValorDebito As Boolean = False

                For Each ld In ListaDetalleTesoreria
                    If ld.Debito > 0 Then
                        logContieneValorDebito = True
                        Exit For
                    End If
                Next

                If logContieneValorDebito = False Then
                    DatosValidosCE = False
                    A2Utilidades.Mensajes.mostrarMensaje("Debe de existir al menos un detalle con el valor en Débito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
            End If


            For Each ld In ListaDetalleTesoreria
                'JFGB20160511
                If plogEsFondosOYD Then
                    If _TesoreriSelected.IDCompania <> intIDCompaniaFirma Then
                        If IsNothing(ld.IDConcepto) Then
                            DatosValidosCE = False
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el concepto en el detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Function
                        End If
                    End If
                End If
                'JFGB20160511
                If plogEsFondosOYD = False Then
                    If (String.IsNullOrEmpty(ld.IDComitente) Or String.IsNullOrEmpty(ld.Nombre)) And mlogClientes Then
                        DatosValidosCE = False
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un cliente en el detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Function
                    End If
                    If (ld.TipoVinculacion = "O") Then
                        A2Utilidades.Mensajes.mostrarMensaje("El comitente " + ld.IDComitente + " en el detalle no puede ser un Ordenante ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                        Return False
                    End If
                Else
                    If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                        'SLB20140626 Se habilita esta validación y queda como VB.6
                        If (String.IsNullOrEmpty(ld.IDComitente) Or String.IsNullOrEmpty(ld.Nombre)) And mlogClientes Then
                            DatosValidosCE = False
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un cliente en el detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Function
                        End If
                        If (ld.TipoVinculacion = "O") Then
                            A2Utilidades.Mensajes.mostrarMensaje("El comitente " + ld.IDComitente + " en el detalle no puede ser un Ordenante ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                            Return False
                        End If
                    Else
                        If ld.ManejaCliente = "S" Then
                            If (String.IsNullOrEmpty(ld.IDComitente) Or String.IsNullOrEmpty(ld.Nombre)) Then
                                DatosValidosCE = False
                                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un cliente en el detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Function
                            End If
                            If (ld.TipoVinculacion = "O") Then
                                A2Utilidades.Mensajes.mostrarMensaje("El comitente " + ld.IDComitente + " en el detalle no puede ser un Ordenante ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                                Return False
                            End If

                        ElseIf ld.ManejaCliente = "N" Then
                            If Not String.IsNullOrEmpty(ld.IDComitente) Or Not String.IsNullOrEmpty(ld.Nombre) Then
                                DatosValidosCE = False
                                A2Utilidades.Mensajes.mostrarMensaje("No se debe ingresar un cliente en el detalle del comprobante por la configuración que tiene el concepto seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Function
                            End If
                            If (ld.TipoVinculacion = "O") Then
                                A2Utilidades.Mensajes.mostrarMensaje("El comitente " + ld.IDComitente + " en el detalle no puede ser un Ordenante ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                TabSeleccionadoGeneral = TabsTesoreriaRC.Tesoreria
                                Return False
                            End If
                        End If

                    End If
                End If


                'Modificado por Natalia Andrea Otalvaro 31 Agosto 2015
                'Se quita la validación para que permita grabar detalles con el debito y credito en cero como en vb.
                '********************************************************************************************************
                If VerValorCredito = Visibility.Collapsed Then
                    If IsNothing(ld.Debito) Or ld.Debito = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un valor para el detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DatosValidosCE = False
                        Exit Function
                    End If
                Else
                    If (IsNothing(ld.Debito) And IsNothing(ld.Credito)) Or (ld.Debito = 0 And ld.Credito = 0) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un valor débito o crédito para el detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DatosValidosCE = False
                        Exit Function
                    End If
                End If
                '********************************************************************************************************

                If IsNothing(ld.Detalle) Then
                    DatosValidosCE = False
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el campo detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
                'JFGB20160511
                If plogEsFondosOYD = False Then
                    If String.IsNullOrEmpty(ld.IDCuentaContable) And mlogCuenta Then 'SLB20140626 Se adicona el mlogCuenta para validar de Igual Forma que VB.6
                        DatosValidosCE = False
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar una cuenta contable en el detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Function
                    End If
                Else
                    If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                        If String.IsNullOrEmpty(ld.IDCuentaContable) And mlogCuenta Then 'SLB20140626 Se adicona el mlogCuenta para validar de Igual Forma que VB.6
                            DatosValidosCE = False
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar una cuenta contable en el detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Function
                        End If
                    Else
                        If String.IsNullOrEmpty(ld.IDCuentaContable) Then
                            DatosValidosCE = False
                            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar una cuenta contable en el detalle del comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Function
                        End If
                    End If
                End If

                'Modificado por Natalia Andrea Otalvaro 31 Agosto 2015
                'Se quita la validación para que permita grabar detalles con el debito y credito en cero como en vb.
                '********************************************************************************************************
                'Lleva el valor debito o credito al valor del detalle
                If (IsNothing(ld.Debito) Or ld.Debito = 0) And (IsNothing(ld.Credito) Or ld.Credito = 0) Then
                    ld.Valor = 0
                Else
                    If Not IsNothing(ld.Debito) Then
                        If ld.Debito <> 0 Then
                            ld.Valor = ld.Debito
                        End If
                    End If
                    If Not IsNothing(ld.Credito) Then
                        If ld.Credito <> 0 Then
                            ld.Valor = -ld.Credito
                        End If
                    End If
                End If

                Existe = False 'Permite evaluar los registros del arreglo.
                For Index = 0 To UBound(vlrTotalXCliente) 'Recorre el arreglo
                    'Si el cliente existe acumula lo que tiene en DB o CR
                    If Not IsNothing(ld.IDComitente) Then
                        If vlrTotalXCliente(Index).lngIDComitente = Trim(ld.IDComitente.ToString()) Then
                            vlrTotalXCliente(Index).VlrCR = vlrTotalXCliente(Index).VlrCR + Val(ld.Credito.ToString())
                            vlrTotalXCliente(Index).VlrDB = vlrTotalXCliente(Index).VlrDB + Val(ld.Debito.ToString())
                            vlrTotalXCliente(Index).VlrSaldo = IIf(IsNothing(ld.SaldoCliente), 0, ld.SaldoCliente) 'SLB20130522
                            Existe = True
                            Exit For
                        End If
                    End If
                Next
                If Not Existe Then 'Si no encontro el cliente agrega un nuevo item al arreglo
                    ReDim Preserve vlrTotalXCliente(UBound(vlrTotalXCliente) + 1)
                    Index = UBound(vlrTotalXCliente)
                    IndexPosicion = Index
                    'Llena los valores en el cliente
                    If Not IsNothing(ld.IDComitente) Then
                        vlrTotalXCliente(Index).lngIDComitente = Trim(ld.IDComitente.ToString())
                    Else
                        vlrTotalXCliente(Index).lngIDComitente = String.Empty
                    End If
                    vlrTotalXCliente(Index).VlrCR = Val(ld.Credito.ToString())
                    vlrTotalXCliente(Index).VlrDB = Val(ld.Debito.ToString())
                    vlrTotalXCliente(Index).VlrSaldo = IIf(IsNothing(ld.SaldoCliente), 0, ld.SaldoCliente) 'SLB20130522
                End If

            Next

            'For Each ltb In ListaTraerBolsas
            '    glogValSobregiroCE = IIf(ltb.logValSobregiroCE, True, False)
            'Next

            'If glogValSobregiroCE Then
            '    validarparametrosGMF()
            '    DatosValidosCE = False
            '    Exit Function
            'End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar la validación de los comprobantes de egreso",
                                 Me.ToString(), "ValidarComprobantesEgreso", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

#End Region

#Region "Consultar parametros y campos obligatorios"

    ''' <summary>
    ''' Método para consultar los parámetros que se Utilizan en Tesoreria
    ''' </summary>
    ''' <remarks>SLB20130204</remarks>
    Private Sub ConsultarParametros()
        Try
            dcProxy.ValidarUsuario_DuplicarTesoreria(Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "VALIDARUSUARIODUPLICAR")
            objProxy.Verificaparametro("OCULTAR_COLUMNA_CREDITO", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "OCULTAR_COLUMNA_CREDITO")
            'JFSB Parametro para habilitar las columnas instruccion y numero de orden
            objProxy.Verificaparametro("HABILITAR_CAMPOS_INTERMEDIA", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "HABILITAR_CAMPOS_INTERMEDIA")
            objProxy.Verificaparametro("TESORERIA_CANTIDADDECIMALES", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "TESORERIA_CANTIDADDECIMALES")

            Select Case moduloTesoreria
                Case ClasesTesoreria.CE.ToString
                    'Jorge Andrés Bedoya 2012/08/13
                    'mlogActivarListaClinton = IIf(ValorParametro("ACTIVARLISTACLINTON", "SI") = "SI", True, False)
                    objProxy.Verificaparametro("ACTIVARLISTACLINTON", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "ACTIVARLISTACLINTON")

                    'se verifica si se debe activar el banco de destino - Santiago Vergara - Junio 2012
                    'mstrActivarBancoTraslado = ValorParametro("TRASLADOENTREBANCOS", "NO")
                    objProxy.Verificaparametro("TRASLADOENTREBANCOS", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "TRASLADOENTREBANCOS")

                    objProxy.Verificaparametro("CONSECUTIVOTRASLADOBANCOS", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "CONSECUTIVOTRASLADOBANCOS")
                    'JBT Parametro para cuando no tenga cuenta bancaria se deje digitar una
                    objProxy.Verificaparametro("DIGITAR_CUENTAS_BENEFICIARIO_CE", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "DIGITAR_CUENTAS_BENEFICIARIO_CE")

                    'JFSB 20161122 - Se agrega llamado al parametro 
                    objProxy.Verificaparametro("INHABILITAR_EDICION_REGISTRO", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "INHABILITAR_EDICION_REGISTRO")

                Case ClasesTesoreria.RC.ToString
                    objProxy.Verificaparametro("CONTROL_CANJE_CHEQUES", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "CONTROL_CANJE_CHEQUES")
                    objProxy.Verificaparametro("ACTIVARLISTACLINTON", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "ACTIVARLISTACLINTON")
                    objProxy.Verificaparametro("PERMITIRVALORESCERORC", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "PERMITIRVALORESCERORC") 'DEMC20180925 Se crea parametro que permite validar si se pueden guardar detalles en cero en la pantalla de RC.
                    'SLB20130925 Se lanza cuando termina de CargarCombosViewModel
                    'objProxy1.Load(objProxy1.ConsularCamposObligatoriosQuery("tblCheques", "(Todos)", "strFormaPagoRC", "(Todos)", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCamposObligatorios, "")
                Case ClasesTesoreria.N.ToString
                    objProxy.Verificaparametro("ANULAR_NOTA_GMF", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "ANULAR_NOTA_GMF")
                    'SE REUTILIZA EL MISMO METODO Terminotraervalorparametrocontroldual PARA NO CREAR MAS CODIGO JBT
                    dcProxy.Verifica4pormil("NOTAS_COBRO_GMF", Program.Usuario, Program.HashConexion, AddressOf Terminotraervalorparametrocontroldual, "ValidaCobroGmfNotas")

                    dcProxy.Verifica4pormil("UTILIZARCUENTASTRASLADOGMF", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "UTILIZARCUENTASTRASLADOGMF")
            End Select

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los parametros de Tesorería",
                                 Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados de Tesoreria
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    ''' <remarks>SLB20130204</remarks>
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState
                Case "ACTIVARLISTACLINTON"
                    _mlogActivarListaClinton = IIf(lo.Value.ToString.Equals("SI"), True, False)
                Case "TRASLADOENTREBANCOS"
                    _mstrActivarBancoTraslado = lo.Value.ToString
                    If _mstrActivarBancoTraslado.Equals("SI") Then
                        MostarBancoDestino = Visibility.Visible
                    End If
                Case "CONSECUTIVOTRASLADOBANCOS"
                    _mstrConsecutivoTrasladoBancos = lo.Value.ToString
                Case "ANULAR_NOTA_GMF"
                    _mstrAnularNotaGMF = lo.Value.ToString.ToUpper
                Case "VALIDARUSUARIODUPLICAR"
                    If lo.Value.ToString <> String.Empty Then
                        MostrarBotonDuplicar = Visibility.Visible
                    End If
                Case "CONTROL_CANJE_CHEQUES"
                    _mlogCanjeCheque = IIf(lo.Value.ToString.Equals("SI"), True, False)
                Case "DIGITAR_CUENTAS_BENEFICIARIO_CE"
                    DIGITAR_CUENTAS_BENEFICIARIO_CE = lo.Value
                Case "CF_UTILIZAPASIVA_A2"
                    plogEsFondosOYD = IIf(lo.Value.ToString.Equals("SI"), True, False)

                    If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                        If plogEsFondosOYD Then
                            If Not IsNothing(_TesoreriSelected) Then
                                If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                                    MostrarConcepto = Visibility.Visible
                                    MostrarConceptoFondosOYD = Visibility.Collapsed
                                Else
                                    MostrarConcepto = Visibility.Collapsed
                                    MostrarConceptoFondosOYD = Visibility.Visible
                                End If
                            End If
                        Else
                            MostrarConceptoFondosOYD = Visibility.Collapsed
                            If Not glogConceptoDetalleTesoreriaManual Then
                                MostrarConcepto = Visibility.Visible
                            Else
                                MostrarConcepto = Visibility.Collapsed
                            End If
                        End If
                    Else
                        MostrarConceptoFondosOYD = Visibility.Collapsed
                        If Not glogConceptoDetalleTesoreriaManual Then
                            MostrarConcepto = Visibility.Visible
                        Else
                            MostrarConcepto = Visibility.Collapsed
                        End If
                    End If

                Case "OCULTAR_COLUMNA_CREDITO"
                    _msMostrarValorDebito = lo.Value.ToString
                    If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                        If _msMostrarValorDebito.Equals("SI") Then
                            VerValorCredito = Visibility.Collapsed
                        End If
                        ' JFSB 20160810 Se agrega valor a la propiedad a la que va a hacer referencia la columna valor debito
                        If lo.Value.ToString.Equals("SI") Then
                            NombreColumnaValor = "Valor"
                        Else
                            If moduloTesoreria = ClasesTesoreria.RC.ToString Then
                                NombreColumnaValor = "Valor Débito"
                            Else
                                NombreColumnaValor = "Débito"
                            End If
                        End If

                    End If
                Case "HABILITAR_CAMPOS_INTERMEDIA"
                    If lo.Value.ToString = "SI" Then
                        VerCamposIntermedia = Visibility.Visible
                    Else
                        VerCamposIntermedia = Visibility.Collapsed
                    End If
                Case "TESORERIA_CANTIDADDECIMALES"
                    If Not String.IsNullOrEmpty(lo.Value) Then
                        Dim intCantidadDecimales As Integer
                        Try
                            intCantidadDecimales = CInt(lo.Value)
                        Catch ex As Exception
                            intCantidadDecimales = 2
                        End Try

                        CantidadDecimalesPantalla = intCantidadDecimales.ToString
                    End If
                Case "INHABILITAR_EDICION_REGISTRO"
                    If Not String.IsNullOrEmpty(lo.Value) Then
                        If moduloTesoreria = ClasesTesoreria.CE.ToString Then

                            _mInhabilitar_Edicion_Registro = lo.Value
                            'Editando = False
                            'EditandoRegistro = False
                            'cmbNombreConsecutivoHabilitado = False
                            'VisibilidadNumCheque = False
                            'EditandoClienteC = False
                            'DeshabilitarSucursal = False
                            'HabilitaTipocheque = False
                        End If
                    End If
                Case "UTILIZARCUENTASTRASLADOGMF"
                    If lo.Value.ToString = "SI" Then
                        logCuentasTrasladoInstalacionActivas = True
                    Else
                        logCuentasTrasladoInstalacionActivas = False
                    End If

                    'DEMC20180925 INICIO
                Case "PERMITIRVALORESCERORC"
                    If lo.Value.ToString = "SI" Then
                        logPermiteValorCeroRC = True
                    Else
                        logPermiteValorCeroRC = False
                    End If
                    'DEMC20180925 FIN
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los paramétros",
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' Método para consultar los combos especificos 
    ''' </summary>
    ''' <remarks>SLB20130925</remarks>
    Private Sub CargarCombosViewModel()
        Try
            Dim strClaseCombos As String = String.Empty
            Select Case moduloTesoreria
                Case ClasesTesoreria.CE.ToString
                    strClaseCombos = "Tesoreria_ComprobantesEgreso"
                Case ClasesTesoreria.RC.ToString
                    strClaseCombos = "Tesoreria_RecibosCaja"
                Case ClasesTesoreria.N.ToString
                    strClaseCombos = "Tesoreria_Notas"
            End Select

            objProxy.Load(objProxy.cargarCombosEspecificosQuery(strClaseCombos, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, strClaseCombos)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los combos especificos en ViewModel de Tesorería",
                                 Me.ToString(), "CargarCombosViewModel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Método encargado de recibir la lista de las Combos Especificos de PagosDeceval.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130830</remarks>
    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If logVentanaEmergente = False Then
                IsBusy = False
            Else
                NuevoRegistroVentanaEmergente(intIDDocumentoEmergente, strNombreConsecutivoEmergente)
            End If

            If Not lo.HasError Then
                ListaCombosEspecificos = objProxy.ItemCombos.ToList()

                _ListConsecutivosConsola = ListaCombosEspecificos.Where(Function(obj) obj.Categoria = strNombreConsecutivoTodos).ToList

                listConsecutivos = _ListConsecutivosConsola

                If moduloTesoreria = ClasesTesoreria.RC.ToString Then
                    objProxy1.Load(objProxy1.ConsularCamposObligatoriosQuery("tblCheques", "(Todos)", "strFormaPagoRC", "(Todos)", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCamposObligatorios, "")
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos",
                                                 Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos",
                                             Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de definir los campos obligatorios de la forma de pago en cheques. Solo aplica para RC.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130301</remarks>
    Private Sub TerminoConsultarCamposObligatorios(ByVal lo As LoadOperation(Of OYDUtilidades.CamposObligatorios))
        Try
            If Not lo.HasError Then
                'Dim ListaFormaPago = CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("FormaPagoRC")
                Dim ListaFormaPago = ListaCombosEspecificos.Where(Function(obj) obj.Categoria = "FormaPagoRC").ToList
                _ListaCamposObligatoriosCheques = objProxy1.CamposObligatorios

                For Each Lista In ListaFormaPago
                    CamposObligatorioSelected = New CamposObligatorios

                    CamposObligatorioSelected.lngID = Lista.ID
                    CamposObligatorioSelected.strFormadePago = Lista.Descripcion

                    For Each campo In (From ld In _ListaCamposObligatoriosCheques Where ld.ValorCampoCondicionante = Lista.ID)
                        If campo.NombreCampoObligado.Equals("strBancoGirador") Then CamposObligatorioSelected.strBancoGirador = True

                        If campo.NombreCampoObligado.Equals("lngNumCheque") Then CamposObligatorioSelected.lngNumCheque = True

                        If campo.NombreCampoObligado.Equals("lngBancoConsignacion") Then CamposObligatorioSelected.lngBancoConsignacion = True

                        If campo.NombreCampoObligado.Equals("dtmConsignacion") Then CamposObligatorioSelected.dtmConsignacion = True

                        If campo.NombreCampoObligado.Equals("strComentario") Then CamposObligatorioSelected.strComentario = True

                        If campo.NombreCampoObligado.Equals("lngidproducto") Then CamposObligatorioSelected.lngidproducto = True
                    Next
                    ListaCamposObligatorios.Add(CamposObligatorioSelected)
                Next

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los campos obligatorios",
                                                     Me.ToString(), "TerminoConsultarCamposObligatorios", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar al terminar de consultar los campos obligatorios",
                                 Me.ToString(), "TerminoConsultarCamposObligatorios", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Propiedades"

    Private _ListaCamposObligatoriosCheques As EntitySet(Of OYDUtilidades.CamposObligatorios)
    Property CamposObligatorioSelected As CamposObligatorios
    Property ListaCamposObligatorios As New List(Of CamposObligatorios)

#End Region

#End Region

#Region "Proceso de Anular Ordenes de Tesorería"

    ''' <summary>
    ''' Realiza el proceso de Anular la Orden de Tesorería.
    ''' </summary>
    ''' <remarks>
    ''' Nombre           : AnularOrden    
    ''' Desarrollado por : Jeison Ramírez Pino.
    ''' Fecha            : Julio 23/2012
    ''' Pruebas CB       : Jeison Ramírez Pino - Julio 23/2012 - Resultado Ok
    ''' </remarks>
    Public Sub AnularOrden()
        Try
            'C1.Silverlight.C1MessageBox.Show("¿Desea Anular esta Orden de Tesorería?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPreguntaOrden)
            mostrarMensajePregunta("¿Desea Anular esta Orden de Tesorería?",
                                   Program.TituloSistema,
                                   "ANULARORDEN",
                                   AddressOf TerminaPreguntaOrden, False)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Anular la Orden de Tesoreria", Me.ToString(), "AnularOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminaPreguntaOrden(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            'configurarNuevaTesoreria(_TesoreriSelected, True)
            'dcProxy.EliminarTarifas(TarifaSelected.Aprobacion, TarifaSelected.Nombre, TarifaSelected.Usuario, TarifaSelected.ID, String.Empty, TarifaSelected.IDTarifas,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")        
            dcProxy.OrdenesTesoreriaEditar(3,
                                           AnulandoOrden.CodCliente,
                                           AnulandoOrden.NombreCliente,
                                           AnulandoOrden.Tipo,
                                           AnulandoOrden.ConsecutivoConsignacion,
                                           AnulandoOrden.Detalle,
                                           AnulandoOrden.ValorSaldo,
                                           String.Empty,
                                           AnulandoOrden.IDBancoGirador,
                                           AnulandoOrden.NroCheque,
                                           AnulandoOrden.FechaConsignacion,
                                           AnulandoOrden.IDBancoRec,
                                           AnulandoOrden.FormaPago,
                                           String.Empty,
                                           String.Empty,
                                           0,
                                           0,
                                           String.Empty,
                                           String.Empty,
                                           Now.Date,
                                           String.Empty,
                                           Now.Date, Program.Usuario, Program.HashConexion)
        Else
            Exit Sub
        End If
    End Sub

#End Region

#Region "Nuevo y duplicar"

    Public Overrides Sub NuevoRegistro()
        Try
            If logVentanaEmergente = False Then
                If dcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            IsBusy = True
            ListaDetalleTesoreria = Nothing
            ControlMensaje = False

            If moduloTesoreria = "N" Then
                ValidarUsuario("Tesoreria_Notas", "Nuevo")
            ElseIf moduloTesoreria = "CE" Then
                ValidarUsuario("Tesoreria_ComprobantesEgreso", "Nuevo")
            ElseIf moduloTesoreria = "RC" Then
                ValidarUsuario("Tesoreria_RecibosCaja", "Nuevo")
            End If
            BotonesOrdenes = True 'JRP se cambia el valor a la variable para que muestre los botones de las operaciones de Ordenes
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para crear un nuevo registro.
    ''' </summary>
    ''' <remarks>
    ''' Funcion          : Nuevo()
    ''' Se encarga de    : Agrega un nuevo registro a la lista de Tesoreria, DetalleTesoreria, ChequeTesoreria, ObservacionesTesoreria.
    ''' Modificado por   : Juan David Correa Perez.
    ''' Descripción      : Se incluye codigo para crear un nuevo documento de tesoreria.
    ''' Fecha            : Agosto 16/2011 2:40 pm
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Agosto 16/2011 - Resultado Ok    
    ''' </remarks>
    Public Sub Nuevo()
        Try
            ActivarDuplicarTesoreria = False
            HabilitarImpresion = False
            Dim NewTesoreri As New Tesoreri
            ''TODO: Verificar cuales son los campos que deben inicializarse
            'NewTesoreri.IDComisionista = TesoreriPorDefecto.IDComisionista
            'NewTesoreri.IDSucComisionista = TesoreriPorDefecto.IDSucComisionista

            'Se le asigna el modulo que lo llama
            NewTesoreri.Tipo = moduloTesoreria
            If moduloTesoreria = "RC" Then
                If (From c In listConsecutivos Where strNombreConsecutivo = "CAJACLI").Count >= 1 Then
                    NewTesoreri.NombreConsecutivo = "CAJACLI"
                End If
            ElseIf moduloTesoreria = "CE" Then
                If (From c In listConsecutivos Where strNombreConsecutivo = "EGRECLI").Count >= 1 Then
                    NewTesoreri.NombreConsecutivo = "EGRECLI"
                End If
                NewTesoreri.FormaPagoCE = "C"
            Else
                If (From c In listConsecutivos Where strNombreConsecutivo = "NOTACLI").Count >= 1 Then
                    NewTesoreri.NombreConsecutivo = "NOTACLI"
                End If
            End If

            NewTesoreri.IDDocumento = TesoreriPorDefecto.IDDocumento
            NewTesoreri.TipoIdentificacion = TesoreriPorDefecto.TipoIdentificacion
            'NewTesoreri.NroDocumento = TesoreriPorDefecto.NroDocumento
            'NewTesoreri.Nombre = TesoreriPorDefecto.Nombre
            'NewTesoreri.IDBanco = TesoreriPorDefecto.IDBanco
            'NewTesoreri.NumCheque = TesoreriPorDefecto.NumCheque
            'NewTesoreri.Valor = TesoreriPorDefecto.Valor
            'NewTesoreri.Detalle = TesoreriPorDefecto.Detalle
            NewTesoreri.Documento = TesoreriPorDefecto.Documento
            NewTesoreri.Estado = TesoreriPorDefecto.Estado
            'NewTesoreri.Estado = TesoreriPorDefecto.Estado
            'NewTesoreri.Impresiones = TesoreriPorDefecto.Impresiones
            NewTesoreri.FormaPagoCE = TesoreriPorDefecto.FormaPagoCE
            'NewTesoreri.NroLote = TesoreriPorDefecto.NroLote
            'NewTesoreri.Contabilidad = TesoreriPorDefecto.Contabilidad
            NewTesoreri.Actualizacion = Now
            'NewTesoreri.Usuario = Program.Usuario
            'NewTesoreri.ParametroContable = TesoreriPorDefecto.ParametroContable
            'NewTesoreri.ImpresionFisica = TesoreriPorDefecto.ImpresionFisica
            'NewTesoreri.MultiCliente = TesoreriPorDefecto.MultiCliente
            'NewTesoreri.CuentaCliente = TesoreriPorDefecto.CuentaCliente
            'NewTesoreri.CodComitente = TesoreriPorDefecto.CodComitente
            'NewTesoreri.ArchivoTransferencia = TesoreriPorDefecto.ArchivoTransferencia
            'NewTesoreri.IdNumInst = TesoreriPorDefecto.IdNumInst
            'NewTesoreri.DVP = TesoreriPorDefecto.DVP
            'NewTesoreri.Instruccion = TesoreriPorDefecto.Instruccion
            'NewTesoreri.IdNroOrden = TesoreriPorDefecto.IdNroOrden
            'NewTesoreri.DetalleInstruccion = TesoreriPorDefecto.DetalleInstruccion
            NewTesoreri.EstadoNovedadContabilidad = TesoreriPorDefecto.EstadoNovedadContabilidad
            'NewTesoreri.eroComprobante_Contabilidad = TesoreriPorDefecto.eroComprobante_Contabilidad
            'NewTesoreri.hadecontabilizacion_Contabilidad = TesoreriPorDefecto.hadecontabilizacion_Contabilidad
            'NewTesoreri.haProceso_Contabilidad = TesoreriPorDefecto.haProceso_Contabilidad
            NewTesoreri.EstadoNovedadContabilidad_Anulada = TesoreriPorDefecto.EstadoNovedadContabilidad_Anulada
            NewTesoreri.EstadoAutomatico = TesoreriPorDefecto.EstadoAutomatico
            NewTesoreri.Contabilidad = True
            'NewTesoreri.eroLote_Contabilidad = TesoreriPorDefecto.eroLote_Contabilidad
            'NewTesoreri.MontoEscrito = TesoreriPorDefecto.MontoEscrito
            'NewTesoreri.TipoIntermedia = TesoreriPorDefecto.TipoIntermedia
            'NewTesoreri.Concepto = TesoreriPorDefecto.Concepto
            'NewTesoreri.RecaudoDirecto = TesoreriPorDefecto.RecaudoDirecto
            'NewTesoreri.ContabilidadEncuenta = TesoreriPorDefecto.ContabilidadEncuenta
            'NewTesoreri.Sobregiro = TesoreriPorDefecto.Sobregiro
            'NewTesoreri.Sobregiro = False
            'NewTesoreri.IdentificacionAutorizadoCheque = TesoreriPorDefecto.IdentificacionAutorizadoCheque
            'NewTesoreri.NombreAutorizadoCheque = TesoreriPorDefecto.NombreAutorizadoCheque
            'NewTesoreri.IDTesoreria = TesoreriPorDefecto.IDTesoreria
            'NewTesoreri.ContabilidadENC = TesoreriPorDefecto.ContabilidadENC
            'NewTesoreri.NroLoteAntENC = TesoreriPorDefecto.NroLoteAntENC
            'NewTesoreri.ContabilidadAntENC = TesoreriPorDefecto.ContabilidadAntENC
            'NewTesoreri.IdSucursalBancaria = TesoreriPorDefecto.IdSucursalBancaria
            'NewTesoreri.Creacion = TesoreriPorDefecto.Creacion
            NewTesoreri.lngBancoConsignacion = Nothing
            NewTesoreri.strBancoDestino = String.Empty
            NewTesoreri.FecEstado = TesoreriPorDefecto.FecEstado
            NewTesoreri.Usuario = Program.Usuario
            NewTesoreri.IDCompania = intIDCompaniaFirma
            NewTesoreri.NombreCompania = strNombreCompaniaFirma

            TesoreriAnterior = TesoreriSelected
            TesoreriSelected = NewTesoreri
            MyBase.CambioItem("Tesoreria")
            Editando = True
            EditandoClienteC = puedeEditarClienteC(True) 'CORREC_CITI_SV_2014
            EditandoRegistro = True
            HabilitarRCImpreso = True
            DeshabilitarSucursal = True
            CuentaContableCliente = Nothing
            'JFGB20160511
            If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                If plogEsFondosOYD = False Then
                    If glogConceptoDetalleTesoreriaManual Then
                        EditarColDetalle = True
                    End If
                End If
            Else
                If glogConceptoDetalleTesoreriaManual Then
                    EditarColDetalle = True
                End If
            End If


            '_mlogPermiteGenerarGMF = True
            Read = False
            MyBase.CambioItem("Editando")
            MyBase.CambioItem("DeshabilitarSucursal")
            MyBase.CambioItem("Read")
            cmbNombreConsecutivoHabilitado = True
            nombreConsecutivovisible = True
            PropiedadTextoCombos = ""
            MyBase.CambioItem("cmbNombreConsecutivoHabilitado")

            If moduloTesoreria = "RC" Then
                Dim NewObservacionTesoreria As New TesoreriaAdicionale
                NewObservacionTesoreria.IDDocumento = TesoreriSelected.IDDocumento
                NewObservacionTesoreria.NombreConsecutivo = TesoreriSelected.NombreConsecutivo
                NewObservacionTesoreria.Tipo = moduloTesoreria
                NewObservacionTesoreria.Actualizacion = TesoreriSelected.Actualizacion
                NewObservacionTesoreria.Observacion = String.Empty
                NewObservacionTesoreria.Usuario = Program.Usuario
                dcProxy.TesoreriaAdicionales.Add(NewObservacionTesoreria)
                ListaObservacionesTesoreria = dcProxy.TesoreriaAdicionales.ToList
                ObservacionTesoreriSelected = NewObservacionTesoreria
                HabilitarEdicion = True
                MyBase.CambioItem("HabilitarEdicion")
                MyBase.CambioItem("ObservacionTesoreriSelected")
                MyBase.CambioItem("ListaObservacionesTesoreria")
            End If

            ConsultarConsecutivosCompania()

            If logVentanaEmergente Then
                DatosDefectoVentanaEmergente()
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "Nuevo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Realiza el proceso de duplicar Tesoreria.
    ''' </summary>
    ''' <remarks>
    ''' Nombre           : duplicarTesoreria    
    ''' Desarrollado por : Juan Carlos Soto Cruz.
    ''' Fecha            : Septiembre 06/2011
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Septiembre 06/2011 - Resultado Ok
    ''' </remarks>
    Public Sub duplicarTesoreria()
        Try
            If moduloTesoreria = "N" Then
                ValidarUsuario("Tesoreria_Notas", "Duplicar")
            ElseIf moduloTesoreria = "CE" Then
                ValidarUsuario("Tesoreria_ComprobantesEgreso", "Duplicar")
            ElseIf moduloTesoreria = "RC" Then
                ValidarUsuario("Tesoreria_RecibosCaja", "Duplicar")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la duplicación de Tesoreria", Me.ToString(), "duplicarTesoreria", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub ContinuarDuplicarRegistro()
        Try
            'C1.Silverlight.C1MessageBox.Show("¿Está seguro de duplicar?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPregunta)
            mostrarMensajePregunta("¿Está seguro de duplicar?",
                                   Program.TituloSistema,
                                   "DUPLICARTESORERIA",
                                   AddressOf TerminaPregunta, False)
        Catch ex As Exception

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la duplicación de Tesoreria", Me.ToString(), "ContinuarDuplicarRegistro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If NotaGMFManual Then
                ActualizarRegistro()
            Else
                configurarNuevaTesoreria(_TesoreriSelected, True)
                BotonesOrdenes = True 'JRP se cambia el valor a la variable para que muestre los botones de las operaciones de Ordenes
            End If
        Else
            NotaGMFManual = False
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' Habilita el formularion para modificar los datos en pantalla pero con un numero de documento diferente.
    ''' </summary>
    ''' <param name="pobjDatosTesoreria"></param>
    ''' <param name="plogDuplicar"></param>
    ''' <remarks>
    ''' Nombre           : configurarNuevaTesoreria    
    ''' Desarrollado por : Juan Carlos Soto Cruz.
    ''' Fecha            : Septiembre 06/2011
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Septiembre 06/2011 - Resultado Ok    
    ''' </remarks>
    Private Sub configurarNuevaTesoreria(ByVal pobjDatosTesoreria As OyDTesoreria.Tesoreri, ByVal plogDuplicar As Boolean)
        Try
            Dim NewTesoreri As New Tesoreri
            Edicion = True
            NewTesoreri.Tipo = pobjDatosTesoreria.Tipo
            NewTesoreri.IDCompania = pobjDatosTesoreria.IDCompania
            NewTesoreri.NombreCompania = pobjDatosTesoreria.NombreCompania
            If plogDuplicar Then
                If Not IsNothing(listConsecutivos) Then
                    If listConsecutivos.Where(Function(i) i.ID = pobjDatosTesoreria.NombreConsecutivo).Count > 0 Then
                        NewTesoreri.NombreConsecutivo = pobjDatosTesoreria.NombreConsecutivo
                    End If
                End If
            Else
                NewTesoreri.NombreConsecutivo = pobjDatosTesoreria.NombreConsecutivo
            End If

            NewTesoreri.IDDocumento = TesoreriPorDefecto.IDDocumento
            NewTesoreri.TipoIdentificacion = pobjDatosTesoreria.TipoIdentificacion
            'If plogDuplicar = True Then
            NewTesoreri.Documento = Now
            'Else
            'NewTesoreri.Documento = Now
            'End If
            NewTesoreri.Estado = "P"
            'NewTesoreri.Estado = pobjDatosTesoreria.Estado
            'NewTesoreri.Actualizacion = pobjDatosTesoreria.Actualizacion
            NewTesoreri.Actualizacion = Now
            NewTesoreri.EstadoNovedadContabilidad = pobjDatosTesoreria.EstadoNovedadContabilidad
            NewTesoreri.EstadoNovedadContabilidad_Anulada = pobjDatosTesoreria.EstadoNovedadContabilidad_Anulada
            NewTesoreri.EstadoAutomatico = pobjDatosTesoreria.EstadoAutomatico
            NewTesoreri.FecEstado = Now
            NewTesoreri.Contabilidad = pobjDatosTesoreria.Contabilidad
            If moduloTesoreria = "CE" Then
                NewTesoreri.IDBanco = pobjDatosTesoreria.IDBanco
                ConsultarConsecutivoBanco()
                NewTesoreri.NombreBco = pobjDatosTesoreria.NombreBco
                NewTesoreri.NumCheque = pobjDatosTesoreria.NumCheque
                NewTesoreri.IdComitente = pobjDatosTesoreria.IdComitente
                NewTesoreri.Nombre = pobjDatosTesoreria.Nombre
                NewTesoreri.NroDocumento = pobjDatosTesoreria.NroDocumento
                NewTesoreri.IdSucursalBancaria = pobjDatosTesoreria.IdSucursalBancaria
                NewTesoreri.SucursalBancaria = pobjDatosTesoreria.SucursalBancaria
                NewTesoreri.Impresiones = pobjDatosTesoreria.Impresiones
                NewTesoreri.FormaPagoCE = pobjDatosTesoreria.FormaPagoCE
                'Santiago Vergara - Octubre 02/2014 - Se corrige para que habilite el tipo cheque cuando se duplica un regustro
                If pobjDatosTesoreria.FormaPagoCE = "C" Then
                    HabilitaTipocheque = True
                End If
                'SLB20130215 Traslado entre bancos
                NewTesoreri.lngBancoConsignacion = pobjDatosTesoreria.lngBancoConsignacion
                NewTesoreri.strBancoDestino = pobjDatosTesoreria.strBancoDestino
                NewTesoreri.Tipocheque = pobjDatosTesoreria.Tipocheque
                'Santiago Vergara - Octubre 08/2014 - Se corrige ya que no estaba duplicacndo la cuenta del cliente 
                NewTesoreri.CuentaCliente = pobjDatosTesoreria.CuentaCliente
                NewTesoreri.TipoCuenta = pobjDatosTesoreria.TipoCuenta
            ElseIf moduloTesoreria = "RC" Then
                NewTesoreri.IdComitente = pobjDatosTesoreria.IdComitente
                NewTesoreri.Nombre = pobjDatosTesoreria.Nombre
                NewTesoreri.NroDocumento = pobjDatosTesoreria.NroDocumento
                NewTesoreri.Impresiones = pobjDatosTesoreria.Impresiones
            End If

            NewTesoreri.Usuario = Program.Usuario 'SLB20140627

            'Se activa este indicador para no borrar los detalles de la tesoreria
            mlogDuplicarTesoreria = plogDuplicar

            TesoreriAnterior = TesoreriSelected
            TesoreriSelected = NewTesoreri
            MyBase.CambioItem("Tesoreria")
            Editando = True
            EditandoClienteC = puedeEditarClienteC(True) 'CORREC_CITI_SV_2014
            EditandoRegistro = True
            HabilitarRCImpreso = True
            DeshabilitarSucursal = True
            'JFGB20160511
            If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                If plogEsFondosOYD = False Then
                    If glogConceptoDetalleTesoreriaManual Then
                        EditarColDetalle = True
                    End If
                End If
            Else
                If glogConceptoDetalleTesoreriaManual Then
                    EditarColDetalle = True
                End If
            End If


            '_mlogPermiteGenerarGMF = True
            Read = False
            MyBase.CambioItem("Editando")
            MyBase.CambioItem("DeshabilitarSucursal")
            MyBase.CambioItem("Read")
            cmbNombreConsecutivoHabilitado = True
            MyBase.CambioItem("cmbNombreConsecutivoHabilitado")

            If moduloTesoreria = "RC" Then
                Dim NewObservacionTesoreria As New TesoreriaAdicionale
                NewObservacionTesoreria.IDDocumento = TesoreriSelected.IDDocumento
                NewObservacionTesoreria.NombreConsecutivo = TesoreriSelected.NombreConsecutivo
                NewObservacionTesoreria.Tipo = moduloTesoreria
                NewObservacionTesoreria.Actualizacion = TesoreriSelected.Actualizacion
                NewObservacionTesoreria.Observacion = String.Empty
                NewObservacionTesoreria.Usuario = Program.Usuario
                dcProxy.TesoreriaAdicionales.Add(NewObservacionTesoreria)
                ListaObservacionesTesoreria = dcProxy.TesoreriaAdicionales.ToList
                ObservacionTesoreriSelected = NewObservacionTesoreria
                HabilitarEdicion = True
                MyBase.CambioItem("HabilitarEdicion")
                MyBase.CambioItem("ObservacionTesoreriSelected")
                MyBase.CambioItem("ListaObservacionesTesoreria")
            End If

            ' Se desactiva el indicador de duplicar tesoreria
            mlogDuplicarTesoreria = False
            ' Se inhabilita el boton de duplicar
            ActivarDuplicarTesoreria = False
            HabilitarImpresion = False

            Editando = True
            EditandoClienteC = puedeEditarClienteC(True) 'CORREC_CITI_SV_2014
            EditandoRegistro = True
            HabilitarRCImpreso = True
            DeshabilitarSucursal = True
            'JFGB20160511
            If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                If plogEsFondosOYD = False Then
                    If glogConceptoDetalleTesoreriaManual Then
                        EditarColDetalle = True
                    End If
                End If
            Else
                If glogConceptoDetalleTesoreriaManual Then
                    EditarColDetalle = True
                End If
            End If

            cmbNombreConsecutivoHabilitado = True
            '_mlogPermiteGenerarGMF = True
            MyBase.CambioItem("DeshabilitarSucursal")
            MyBase.CambioItem("Editando")
            ListaDetalleduplica.Clear()
            For Each li In ListaDetalleTesoreria
                nuevoduplicadetalle(li)
            Next
            Dim objListaDetalleTesoreria As New List(Of DetalleTesoreri)

            For Each I In ListaDetalleduplica
                objListaDetalleTesoreria.Add(I)
            Next

            ListaDetalleTesoreria = objListaDetalleTesoreria

            If moduloTesoreria = "RC" Then
                ListaChequeduplica.Clear()
                For Each li In ListaChequeTesoreria
                    nuevoduplicacheque(li)
                Next
                Dim objListaDetalleCheque As New List(Of Cheque)

                For Each li In ListaChequeduplica
                    objListaDetalleCheque.Add(li)
                Next

                ListaChequeTesoreria = objListaDetalleCheque
            End If

            If moduloTesoreria = "CE" Then buscarBancos(_TesoreriSelected.IDBanco, "consultarbancos")
            'RecargarSaldoClientes()

            'JFGB20160511
            If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
                If plogEsFondosOYD Then
                    If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                        If Not IsNothing(ListaDetalleTesoreria) Then
                            For Each li In ListaDetalleTesoreria
                                li.HabilitarSeleccionCliente = True
                                li.HabilitarSeleccionBanco = True
                                li.HabilitarSeleccionCuentaContable = True
                            Next
                        End If
                        HabilitarCreacionDetallesNotas = True
                    Else
                        If Not IsNothing(ListaDetalleTesoreria) Then
                            For Each li In ListaDetalleTesoreria
                                If li.ManejaCliente = "S" Or li.ManejaCliente = "O" Then
                                    li.HabilitarSeleccionCliente = True
                                Else
                                    li.HabilitarSeleccionCliente = False
                                End If

                                li.HabilitarSeleccionBanco = False
                                li.HabilitarSeleccionCuentaContable = False
                            Next
                        End If
                        HabilitarCreacionDetallesNotas = False
                    End If
                Else
                    If Not IsNothing(ListaDetalleTesoreria) Then
                        For Each li In ListaDetalleTesoreria
                            li.HabilitarSeleccionCliente = True
                            li.HabilitarSeleccionBanco = True
                            li.HabilitarSeleccionCuentaContable = True
                        Next
                    End If
                    HabilitarCreacionDetallesNotas = True
                End If
            Else
                If Not IsNothing(ListaDetalleTesoreria) Then
                    For Each li In ListaDetalleTesoreria
                        li.HabilitarSeleccionCliente = True
                        li.HabilitarSeleccionBanco = True
                        li.HabilitarSeleccionCuentaContable = True
                    Next
                End If
                HabilitarCreacionDetallesNotas = True
            End If

            If Not IsNothing(dcProxy.DetalleTesoreris) Then
                dcProxy.DetalleTesoreris.Clear()
            End If

            If Not IsNothing(dcProxy.Cheques) Then
                dcProxy.Cheques.Clear()
            End If

            If Not IsNothing(ListaDetalleTesoreria) Then
                If ListaDetalleTesoreria.Count > 0 Then
                    DetalleTesoreriSelected = ListaDetalleTesoreria.First
                End If
            End If

            If Not IsNothing(ListaChequeTesoreria) Then
                If ListaChequeTesoreria.Count > 0 Then
                    ChequeTesoreriSelected = ListaChequeTesoreria.First
                End If
            End If

        Catch ex As Exception
            mlogDuplicarTesoreria = False
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en configurar la nueva Tesoreria", Me.ToString(), "configurarNuevaTesoreria", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub nuevoduplicadetalle(ByVal nuevodetalle As DetalleTesoreri)
        Dim NewDetalleTesoreri As New DetalleTesoreri
        NewDetalleTesoreri.IDComitente = nuevodetalle.IDComitente
        NewDetalleTesoreri.Nombre = nuevodetalle.Nombre
        NewDetalleTesoreri.NIT = nuevodetalle.NIT
        'NewDetalleTesoreri.IDDocumento = nuevodetalle.IDDocumento
        NewDetalleTesoreri.IDDocumento = TesoreriSelected.IDDocumento
        NewDetalleTesoreri.Tipo = nuevodetalle.Tipo
        NewDetalleTesoreri.NombreConsecutivo = nuevodetalle.NombreConsecutivo
        NewDetalleTesoreri.Usuario = Program.Usuario
        NewDetalleTesoreri.Actualizacion = Now
        NewDetalleTesoreri.Secuencia = nuevodetalle.Secuencia
        NewDetalleTesoreri.Debito = nuevodetalle.Debito
        NewDetalleTesoreri.Credito = nuevodetalle.Credito
        NewDetalleTesoreri.Detalle = nuevodetalle.Detalle
        NewDetalleTesoreri.IDCuentaContable = nuevodetalle.IDCuentaContable
        NewDetalleTesoreri.CentroCosto = nuevodetalle.CentroCosto
        NewDetalleTesoreri.IDBanco = nuevodetalle.IDBanco

        'SLB20140620 Concepto
        NewDetalleTesoreri.IDConcepto = nuevodetalle.IDConcepto
        NewDetalleTesoreri.CompaniaBanco = nuevodetalle.CompaniaBanco
        NewDetalleTesoreri.EstadoTransferencia = nuevodetalle.EstadoTransferencia
        NewDetalleTesoreri.BancoDestino = nuevodetalle.BancoDestino
        NewDetalleTesoreri.NombreBancoDestino = nuevodetalle.NombreBancoDestino
        NewDetalleTesoreri.CuentaDestino = nuevodetalle.CuentaDestino
        NewDetalleTesoreri.TipoCuenta = nuevodetalle.TipoCuenta
        NewDetalleTesoreri.DescripcionTipoCuenta = nuevodetalle.DescripcionTipoCuenta
        NewDetalleTesoreri.IdentificacionTitular = nuevodetalle.IdentificacionTitular
        NewDetalleTesoreri.Titular = nuevodetalle.Titular
        NewDetalleTesoreri.TipoIdTitular = nuevodetalle.TipoIdTitular
        NewDetalleTesoreri.DescripcionTipoIdTitular = nuevodetalle.DescripcionTipoIdTitular
        NewDetalleTesoreri.FormaEntrega = nuevodetalle.FormaEntrega
        NewDetalleTesoreri.DescripcionFormaEntrega = nuevodetalle.DescripcionFormaEntrega
        NewDetalleTesoreri.Beneficiario = nuevodetalle.Beneficiario
        NewDetalleTesoreri.TipoIdentBeneficiario = nuevodetalle.TipoIdentBeneficiario
        NewDetalleTesoreri.DescripcionTipoIdentBeneficiario = nuevodetalle.DescripcionTipoIdentBeneficiario
        NewDetalleTesoreri.IdentificacionBenficiciario = nuevodetalle.IdentificacionBenficiciario
        NewDetalleTesoreri.NombrePersonaRecoge = nuevodetalle.NombrePersonaRecoge
        NewDetalleTesoreri.IdentificacionPerRecoge = nuevodetalle.IdentificacionPerRecoge
        NewDetalleTesoreri.OficinaEntrega = nuevodetalle.OficinaEntrega
        NewDetalleTesoreri.Exportados = nuevodetalle.Exportados
        NewDetalleTesoreri.Nombre = nuevodetalle.Nombre
        NewDetalleTesoreri.ManejaCliente = nuevodetalle.ManejaCliente
        NewDetalleTesoreri.TipoMovimientoTesoreria = nuevodetalle.TipoMovimientoTesoreria
        NewDetalleTesoreri.Retencion = nuevodetalle.Retencion
        NewDetalleTesoreri.HabilitarSeleccionCliente = nuevodetalle.HabilitarSeleccionCliente
        NewDetalleTesoreri.HabilitarSeleccionBanco = nuevodetalle.HabilitarSeleccionBanco
        NewDetalleTesoreri.HabilitarSeleccionCuentaContable = nuevodetalle.HabilitarSeleccionCuentaContable
        NewDetalleTesoreri.OficinaCuentaInversion = nuevodetalle.OficinaCuentaInversion
        NewDetalleTesoreri.NombreCarteraColectiva = nuevodetalle.NombreCarteraColectiva
        NewDetalleTesoreri.NombreAsesor = nuevodetalle.NombreAsesor
        NewDetalleTesoreri.CodigoCartera = nuevodetalle.CodigoCartera

        NewDetalleTesoreri.intClave_PorAprobar = -1

        'DEMC20160114 Apoyo con JEPM. Asignar nuevo IDDetalleTesoreria negativo
        If Not IsNothing(ListaDetalleTesoreria) AndAlso ListaDetalleTesoreria.Count >= 1 Then
            If IsNothing(ListaDetalleduplica) OrElse ListaDetalleduplica.Count < 1 Then
                NewDetalleTesoreri.IDDetalleTesoreria = -1
            Else
                NewDetalleTesoreri.IDDetalleTesoreria = (From c In ListaDetalleduplica Select c.IDDetalleTesoreria).Min - 1
            End If
        End If

        ListaDetalleduplica.Add(NewDetalleTesoreri)
    End Sub

    Public Sub nuevoduplicacheque(ByVal nuevocheque As Cheque)
        Dim newChequeTesoreria As New Cheque
        'newChequeTesoreria.IDDocumento = nuevocheque.IDDocumento
        newChequeTesoreria.IDDocumento = TesoreriSelected.IDDocumento
        newChequeTesoreria.NombreConsecutivo = nuevocheque.NombreConsecutivo
        newChequeTesoreria.Secuencia = nuevocheque.Secuencia
        newChequeTesoreria.BancoGirador = nuevocheque.BancoGirador
        newChequeTesoreria.NumCheque = nuevocheque.NumCheque
        newChequeTesoreria.Valor = nuevocheque.Valor
        newChequeTesoreria.Usuario = Program.Usuario
        newChequeTesoreria.BancoConsignacion = nuevocheque.BancoConsignacion
        newChequeTesoreria.Consignacion = Now.Date
        newChequeTesoreria.Actualizacion = Now
        'SLB20130403 Se deben mantener los estados de la forma de pago, observaciones y tipo producto
        'newChequeTesoreria.FormaPagoRC = "C"
        newChequeTesoreria.FormaPagoRC = nuevocheque.FormaPagoRC
        newChequeTesoreria.Comentario = nuevocheque.Comentario
        newChequeTesoreria.IdProducto = nuevocheque.IdProducto
        ListaChequeduplica.Add(newChequeTesoreria)
    End Sub

    Public Sub NuevoRegistroVentanaEmergente(ByVal pintIDDocumento As Nullable(Of Integer),
                                                   ByVal pstrNombreConsecutivo As String)
        Try
            If pintIDDocumento = 0 Or IsNothing(pintIDDocumento) Then
                NuevoRegistro()
            Else
                logEsEdicionRegistroEmergente = True
                IsBusy = True
                dcProxy.Tesoreris.Clear()
                TesoreriAnterior = Nothing
                dcProxy.Load(dcProxy.TesoreriaConsultarQuery(1, moduloTesoreria, pstrNombreConsecutivo, pintIDDocumento, Nothing, "Todos", Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "VentanaEmergente")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistroVentanaEmergente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub DatosDefectoVentanaEmergente()
        Try
            If IsNothing(ListaTesoreria) Then
                ListaTesoreria = dcProxy.Tesoreris
            End If
            If IsNothing(ListaDetalleTesoreria) Then
                ListaDetalleTesoreria = dcProxy.DetalleTesoreris.ToList
            End If
            If IsNothing(ListaChequeTesoreria) Then
                ListaChequeTesoreria = dcProxy.Cheques.ToList
            End If

            If Not IsNothing(objTesoreriaEmergente) Then

                If Not IsNothing(objTesoreriaEmergente.IDCompania) Then
                    _TesoreriSelected.IDCompania = objTesoreriaEmergente.IDCompania
                    _TesoreriSelected.NombreCompania = objTesoreriaEmergente.NombreCompania
                End If
                If Not IsNothing(objTesoreriaEmergente.FechaDocumento) Then
                    _TesoreriSelected.Documento = objTesoreriaEmergente.FechaDocumento
                End If

                If moduloTesoreria = "RC" Or moduloTesoreria = "CE" Then
                    If Not String.IsNullOrEmpty(objTesoreriaEmergente.NroDocumentoBeneficiario) Then
                        _TesoreriSelected.NroDocumento = objTesoreriaEmergente.NroDocumentoBeneficiario
                    End If
                    If Not String.IsNullOrEmpty(objTesoreriaEmergente.NombreBeneficiario) Then
                        _TesoreriSelected.Nombre = objTesoreriaEmergente.NombreBeneficiario
                    End If
                    If Not String.IsNullOrEmpty(objTesoreriaEmergente.TipoIdentificacionBeneficiario) Then
                        _TesoreriSelected.TipoIdentificacion = objTesoreriaEmergente.TipoIdentificacionBeneficiario
                    End If
                End If

                If moduloTesoreria = "RC" Then
                    If Not IsNothing(objTesoreriaEmergente.ListaCheque) Then
                        For Each li In objTesoreriaEmergente.ListaCheque
                            Dim newChequeTesoreria As New Cheque
                            newChequeTesoreria.IDDocumento = TesoreriSelected.IDDocumento
                            newChequeTesoreria.NombreConsecutivo = TesoreriSelected.NombreConsecutivo

                            Dim resultChequeRC As Integer = 1
                            For Each value In ListaChequeTesoreriaAnt
                                resultChequeRC = Math.Max(resultChequeRC, value.Secuencia) + 1
                            Next
                            newChequeTesoreria.Secuencia = resultChequeRC
                            newChequeTesoreria.BancoGirador = ""
                            newChequeTesoreria.NumCheque = 0
                            newChequeTesoreria.Valor = 0
                            newChequeTesoreria.Usuario = Program.Usuario
                            newChequeTesoreria.BancoConsignacion = 0
                            newChequeTesoreria.Consignacion = Now.Date
                            newChequeTesoreria.Actualizacion = Now.Date
                            newChequeTesoreria.FormaPagoRC = "C"
                            newChequeTesoreria.ChequeHizoCanje = True 'SLB20130802 Manejo de cheques de canje

                            If Not IsNothing(li.BancoGirador) Then
                                newChequeTesoreria.BancoGirador = li.BancoGirador.ToString
                            End If
                            If Not IsNothing(li.NroCheque) Then
                                newChequeTesoreria.NumCheque = li.NroCheque
                            End If
                            If Not IsNothing(li.Valor) Then
                                newChequeTesoreria.Valor = li.Valor
                            End If
                            If Not IsNothing(li.BancoConsignacion) Then
                                newChequeTesoreria.BancoConsignacion = li.BancoConsignacion
                            End If
                            If Not IsNothing(li.FechaConsignacion) Then
                                newChequeTesoreria.Consignacion = li.FechaConsignacion
                            End If
                            If Not IsNothing(li.FormaPago) Then
                                newChequeTesoreria.FormaPagoRC = li.FormaPago
                            End If
                            If Not IsNothing(li.TipoProducto) Then
                                newChequeTesoreria.IdProducto = li.TipoProducto
                            End If

                            AdicionarRegistroEnDetalleCheque(ListaChequeTesoreria, newChequeTesoreria)
                            ListaChequeTesoreriaAnt.Add(New secuenciachequestesoreria With {.Secuencia = resultChequeRC})
                            ChequeTesoreriSelected = newChequeTesoreria

                            MyBase.CambioItem("ChequeTesoreriSelected")
                            MyBase.CambioItem("ListaChequeTesoreria")
                        Next
                    End If
                ElseIf moduloTesoreria = "CE" Then
                    If Not String.IsNullOrEmpty(objTesoreriaEmergente.FormaPago) Then
                        _TesoreriSelected.FormaPagoCE = objTesoreriaEmergente.FormaPago
                    End If
                    If _TesoreriSelected.FormaPagoCE = "C" Then
                        HabilitaTipocheque = True
                        If Not String.IsNullOrEmpty(objTesoreriaEmergente.TipoCheque) Then
                            _TesoreriSelected.Tipocheque = objTesoreriaEmergente.TipoCheque
                        End If
                    Else
                        HabilitaTipocheque = False
                    End If
                    If _TesoreriSelected.FormaPagoCE = "T" Then
                        If Not String.IsNullOrEmpty(objTesoreriaEmergente.CuentaCliente) Then
                            _TesoreriSelected.CuentaCliente = objTesoreriaEmergente.CuentaCliente
                            'JFSB 20171019
                            CuentaContableCliente = objTesoreriaEmergente.CuentaCliente
                        End If
                    End If
                End If

                If Not IsNothing(objTesoreriaEmergente.ListaDetalle) Then
                    For Each li In objTesoreriaEmergente.ListaDetalle
                        Dim NewDetalleTesoreri As New DetalleTesoreri

                        If Not String.IsNullOrEmpty(li.IDComitente) Then
                            NewDetalleTesoreri.IDComitente = li.IDComitente
                            NewDetalleTesoreri.Nombre = li.NombreComitente
                        End If

                        If Not String.IsNullOrEmpty(li.Nit) Then
                            NewDetalleTesoreri.NIT = li.Nit
                        End If

                        If Not IsNothing(ListaDetalleTesoreria) AndAlso ListaDetalleTesoreria.Count >= 1 Then
                            NewDetalleTesoreri.IDDetalleTesoreria = (From c In _ListaDetalleTesoreria Select c.IDDetalleTesoreria).Min - 1
                        Else
                            NewDetalleTesoreri.IDDetalleTesoreria = -1
                        End If
                        NewDetalleTesoreri.IDDocumento = TesoreriSelected.IDDocumento
                        NewDetalleTesoreri.Tipo = TesoreriSelected.Tipo
                        NewDetalleTesoreri.NombreConsecutivo = TesoreriSelected.NombreConsecutivo
                        NewDetalleTesoreri.Usuario = Program.Usuario

                        Dim resultDetalleRC As Integer = 1
                        For Each value In ListaDetalleTesoreriaAnt
                            resultDetalleRC = Math.Max(resultDetalleRC, value.Secuencia) + 1
                        Next
                        NewDetalleTesoreri.Secuencia = resultDetalleRC
                        NewDetalleTesoreri.intClave_PorAprobar = -1

                        If IsNothing(ListaDetalleTesoreria) Then
                            ListaDetalleTesoreria = dcProxy.DetalleTesoreris.ToList
                        End If

                        NewDetalleTesoreri.Actualizacion = Now.Date

                        If plogEsFondosOYD Then
                            If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                                NewDetalleTesoreri.HabilitarSeleccionBanco = True
                                NewDetalleTesoreri.HabilitarSeleccionCliente = True
                                NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True
                            Else
                                NewDetalleTesoreri.HabilitarSeleccionBanco = False
                                NewDetalleTesoreri.HabilitarSeleccionCliente = False
                                NewDetalleTesoreri.HabilitarSeleccionCuentaContable = False
                            End If
                        Else
                            NewDetalleTesoreri.HabilitarSeleccionBanco = True
                            NewDetalleTesoreri.HabilitarSeleccionCliente = True
                            NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True
                        End If

                        If Not IsNothing(li.IDConcepto) Then
                            NewDetalleTesoreri.IDConcepto = li.IDConcepto
                        End If
                        If Not String.IsNullOrEmpty(li.Detalle) Then
                            NewDetalleTesoreri.Detalle = li.Detalle
                        End If
                        If Not String.IsNullOrEmpty(li.CuentaContable) Then
                            NewDetalleTesoreri.IDCuentaContable = li.CuentaContable
                        End If
                        If Not IsNothing(li.ValorDebito) Then
                            NewDetalleTesoreri.Debito = li.ValorDebito
                        End If
                        If Not IsNothing(li.ValorCredito) Then
                            NewDetalleTesoreri.Credito = li.ValorCredito
                        End If

                        AdicionarRegistroEnDetalle(ListaDetalleTesoreria, NewDetalleTesoreri)
                        DetalleTesoreriSelected = NewDetalleTesoreri
                        If Not String.IsNullOrEmpty(DetalleTesoreriSelected.IDComitente) Then
                            AdicionarCuentaContable_Detalle()
                        End If

                        MyBase.CambioItem("DetalleTesoreriSelected")
                        MyBase.CambioItem("ListaDetalleTesoreria")
                    Next
                End If
            End If

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "DatosDefectoVentanaEmergente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Generación del GMF"

    Public Sub validarparametrosGMF()
        Try
            Dim MensajeCobroGMF As String = String.Format("El banco para este comprobante tiene configurado{0}el manejo del Gravamen al Movimiento Financiero{0}(tanto por cada mil). Por favor elija la opción mas{0}indicada para grabar el comprobante de Egreso{0}y......", vbCrLf)

            'If mstrNombreConsecutivoNota.Trim <> "" And _mlogPermiteGenerarGMF Then
            If moduloTesoreria = "N" Then
                If ValidarParametrosInstalacionGMF() Then
                    Mensajegmf = New MensajeGMF
                    Mensajegmf.RdbDebajo.Visibility = Visibility.Collapsed 'No se puede cobrar GMF por debajo
                    AddHandler Mensajegmf.Closed, AddressOf CerroVentanagmf


                    If objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "PREGUNTAR").Count > 0 Then
                        If objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "PREGUNTAR" And i.Confirmacion = False).Count > 0 Then
                            MensajeCobroGMF = objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "PREGUNTAR" And i.Confirmacion = False).First().MensajeCobroGMF
                        End If

                        Mensajegmf.txtMensajeGMF.Text = MensajeCobroGMF
                        Program.Modal_OwnerMainWindowsPrincipal(Mensajegmf)
                        Mensajegmf.ShowDialog()
                        Exit Sub
                    ElseIf objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "ENCIMA").Count > 0 Then
                        Mensajegmf.RdbEncima.IsChecked = True
                        ContinuarCobroGMF()
                        Exit Sub
                    ElseIf objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "DEBAJO").Count > 0 Then
                        Mensajegmf.RdbDebajo.IsChecked = True
                        ContinuarCobroGMF()
                        Exit Sub
                    End If
                    Exit Sub
                Else
                    Exit Sub
                End If
            Else
                Mensajegmf = New MensajeGMF
                AddHandler Mensajegmf.Closed, AddressOf CerroVentanagmf


                If objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "PREGUNTAR").Count > 0 Then
                    If objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "PREGUNTAR" And i.Confirmacion = False).Count > 0 Then
                        MensajeCobroGMF = objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "PREGUNTAR" And i.Confirmacion = False).First().MensajeCobroGMF
                    End If

                    Mensajegmf.txtMensajeGMF.Text = MensajeCobroGMF
                    Program.Modal_OwnerMainWindowsPrincipal(Mensajegmf)
                    Mensajegmf.ShowDialog()
                    Exit Sub
                ElseIf objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "ENCIMA").Count > 0 Then
                    Mensajegmf.RdbEncima.IsChecked = True
                    ContinuarCobroGMF()
                    Exit Sub
                ElseIf objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "DEBAJO").Count > 0 Then
                    Mensajegmf.RdbDebajo.IsChecked = True
                    ContinuarCobroGMF()
                    Exit Sub
                End If
            End If

            'SLB20130305 Organizar la secuencia de los eventos
            'Select Case moduloTesoreria
            '    Case ClasesTesoreria.N.ToString, ClasesTesoreria.CE.ToString
            '        recorresobregiro()
            '    Case Else
            '        'aca va la logica de la validacion
            '        'recorrerarreglo()
            'End Select

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en validarparametrosGMF",
                     Me.ToString(), "validarparametrosGMF", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    'Private Sub Terminotraergravamen(ByVal lo As LoadOperation(Of Bancogravamen))
    '    Try
    '        If Not lo.HasError Then
    '            If dcProxy.Bancogravamens.Count > 0 Then
    '                If IsNothing(dcProxy.Bancogravamens.First.stridcuentactble) Then
    '                    mstrNroCuentaContableBanco = String.Empty
    '                Else
    '                    mstrNroCuentaContableBanco = dcProxy.Bancogravamens.First.stridcuentactble.Trim
    '                End If
    '                If dcProxy.Bancogravamens.First.logcobrogmf Then
    '                    If moduloTesoreria = "N" Then
    '                        'If logCobrarGMF Then
    '                        If ValidarParametrosInstalacionGMF() Then
    '                            Mensajegmf = New MensajeGMF
    '                            Mensajegmf.RdbDebajo.Visibility = Visibility.Collapsed 'No se puede cobrar GMF por debajo
    '                            AddHandler Mensajegmf.Closed, AddressOf CerroVentanagmf
    '                            

    '                            If objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "PREGUNTAR").Count > 0 Then
    '                                Mensajegmf.ShowDialog()
    '                                Exit Sub
    '                            ElseIf objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "ENCIMA").Count > 0 Then
    '                                Mensajegmf.RdbEncima.IsChecked = True
    '                                ContinuarCobroGMF()
    '                            ElseIf objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "DEBAJO").Count > 0 Then
    '                                Mensajegmf.RdbDebajo.IsChecked = True
    '                                ContinuarCobroGMF()
    '                            End If
    '                            Exit Sub
    '                        End If
    '                        'End If
    '                    Else
    '                        Mensajegmf = New MensajeGMF
    '                        AddHandler Mensajegmf.Closed, AddressOf CerroVentanagmf
    '                        

    '                        If objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "PREGUNTAR").Count > 0 Then
    '                            Mensajegmf.ShowDialog()
    '                            Exit Sub
    '                        ElseIf objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "ENCIMA").Count > 0 Then
    '                            Mensajegmf.RdbEncima.IsChecked = True
    '                            ContinuarCobroGMF()
    '                        ElseIf objRetornoCalculo.Where(Function(i) i.TipoCobroGMF = "DEBAJO").Count > 0 Then
    '                            Mensajegmf.RdbDebajo.IsChecked = True
    '                            ContinuarCobroGMF()
    '                        End If
    '                    End If
    '                Else
    '                    mlogCobroGmf = False
    '                End If
    '            End If


    '            'SLB20130305 Organizar la secuencia de los eventos
    '            Select Case moduloTesoreria
    '                Case ClasesTesoreria.N.ToString, ClasesTesoreria.CE.ToString
    '                    recorresobregiro()
    '                Case Else
    '                    'aca va la logica de la validacion
    '                    'recorrerarreglo()
    '            End Select
    '        Else
    '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en PoseeCobrogravamen", _
    '                                             Me.ToString(), "Terminotraergravamen", Application.Current.ToString(), Program.Maquina, lo.Error)
    '            lo.MarkErrorAsHandled()   '????
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en PoseeCobrogravamen", _
    '                             Me.ToString(), "Terminotraergravamen", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Se valida los parametros del GMF, si falta alguno no se puede generar el GMF
    ''' </summary>
    ''' <returns>True - Existen todos los parametros</returns>
    ''' <remarks>SLB20130306</remarks>
    Private Function ValidarParametrosInstalacionGMF() As Boolean
        ValidarParametrosInstalacionGMF = False

        'If Not (logCobrarGMF = True And ((mlogBancoGMF = True And mlogclienteGMF = True) Or (mlogBancoGMF = False And mlogclienteGMF = True))) Then
        '    Exit Function
        'End If

        If mstrNombreConsecutivoNota = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado el nombre del consecutivo de la nota GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrctacontableMGF = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrccostosMGF = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado el centro de costos para el GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrctacontablecontrapMGF = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable de la contraparte para el GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrccostocontrapMGF = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado el centro de costos de la contraparte para el GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrTipoNotasCxC = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado el tipo de Notas CxC en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrCtacontraparteNotasCXC = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable de la contraparte para Notas CxC en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If logCuentasTrasladoInstalacionActivas Then
            If mstrCtaContableTrasladoDB = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable de traslado DB en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            If mstrCtaContableTrasladoCR = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable de traslado CR en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If
        End If

        ValidarParametrosInstalacionGMF = True
    End Function

    Private Sub CerroVentanagmf()
        If Mensajegmf.DialogResult = True Then
            ContinuarCobroGMF()
        Else
            'JFSB 20170711 Se agrega el control para que no se quede la pantalla cargando
            IsBusy = False
            IsBusyDetalles = False
            NotaGMFManual = False
            ControlMensaje = False
            Exit Sub
        End If


    End Sub

    Public Sub ContinuarCobroGMF()
        'JFSB 20160922
        If Mensajegmf.generargmf.NoGenera Then
            mlogCobroGmf = False
            intBanco = -1
            _TesoreriSelected.CobroGMF = 0
        Else
            mlogCobroGmf = True
            If Mensajegmf.generargmf.Encima Then
                mlogCalculoGMFPorEncima = True
                mlogCalculoGMFPorDebajo = False
                _TesoreriSelected.CobroGMF = 1
            Else
                mlogCalculoGMFPorDebajo = True
                mlogCalculoGMFPorEncima = False
                _TesoreriSelected.CobroGMF = -1
            End If
        End If

        'If plogRecorrerSobregriro Then
        'SLB20130305 Organizar la secuencia de los eventos
        Select Case moduloTesoreria
            Case ClasesTesoreria.N.ToString, ClasesTesoreria.CE.ToString
                recorresobregiro()
            Case Else
                'aca va la logica de la validacion
                'recorrerarreglo()
        End Select
        'End If

    End Sub

    Private Sub TerminoverificarGMF(ByVal lo As LoadOperation(Of Instalacion))
        Try
            If Not lo.HasError Then

                Dim objInstalacion As New Instalacion

                objInstalacion = dcProxy.Instalacions.FirstOrDefault

                With objInstalacion
                    mstrNombreConsecutivoNota = .STRTIPO
                    mstrctacontableMGF = .STRCTACONTABLE
                    mstrccostosMGF = .strCCosto
                    mstrctacontablecontrapMGF = .STRCTACONTABLECONTRAPARTE
                    mstrccostocontrapMGF = .STRCCOSTOCONTRAPARTE
                    mstrCtacontraparteNotasCXC = .strCtaContableContraparteNotasCxC
                    msnGMF = .INTGMS
                    mdblGMFInferior = .DBLGMFINFERIOR
                    If mdblGMFInferior = 0 Then
                        mdblGMFInferior = msnGMF
                    End If
                    strservidor = .strServidor
                    strbasedatos = .strBaseDatos
                    strowner = .strOwner
                    If Not IsNothing(.lngCompania) Then
                        strcia = .lngCompania
                    End If
                    strCuenta = .strCtaContableClientes
                    logValidacuentasuperval = .logValidaCuentaSuperVal
                    mstrCtaContableTrasladoDB = .strCtaContableTrasladoDB
                    mstrCtaContableTrasladoCR = .strCtaContableTrasladoCR
                    mstrTipoNotasCxC = .strtipoNotasCxC
                End With

                dcProxy.Load(dcProxy.ConfiguracionConsecutivos_ConsultarQuery(moduloTesoreria, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfiguracionConsecutivos, "FiltroInicial")
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en VerificarGMF",
                                                 Me.ToString(), "TerminoverificarGMF", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en VerificarGMF",
                                 Me.ToString(), "TerminoverificarGMF", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "SuperVal_Comentadas"

    'Private Sub recorrerarreglo()
    '    If logValidacuentasuperval Then
    '        validarTotalctasupervalores()
    '        validarCtasupervaloresdelbanco()
    '    Else 'SLB para poder guardar el Comprabante de Egreso
    '        If moduloTesoreria = "CE" Then
    '            recorresobregiro()
    '        End If
    '    End If
    'End Sub
    'Private Sub validarTotalctasupervalores()
    '    Try
    '        Dim consecutivo As Integer = 0
    '        mcurTotalDebitos = 0
    '        mcurTotalCreditos = 0
    '        For Each li In ListaDetalleTesoreria
    '            mcurTotalDebitos = mcurTotalDebitos + IIf(li.Debito Is Nothing, 0, li.Debito)
    '            mcurTotalCreditos = mcurTotalCreditos + IIf(li.Credito Is Nothing, 0, li.Credito)
    '            dblvalordigitado = IIf(li.Debito Is Nothing, 0, li.Debito) - IIf(li.Debito Is Nothing, 0, li.Credito)
    '            dblvalorsaldodetalle = dblvalordigitado
    '            If mlogCobroGmf And Not mlogCalculoGMFPorEncima Then
    '                dblvalorsaldodetalle = dblvalordigitado * (1 - mdblGMFInferior)
    '                li.Valor = dblvalorsaldodetalle
    '            End If
    '            If mlogCobroGmf Then
    '                li.NombreConsecutivoNotaGMF = mstrNombreConsecutivoNota
    '                li.IDDocumentoNotaGMF = 0 'aqui va el numero de documento de la nota que se creo
    '            End If
    '            dcProxy.DatosEnCuentas.Clear()
    '            dcProxy.Load(dcProxy.ValidaDatosencuentaQuery(li.IDCuentaContable, li.NIT, li.CentroCosto, 0, 0,Program.Usuario, Program.HashConexion), AddressOf Terminotraerdatosencuenta, consecutivo)
    '            consecutivo = consecutivo + 1
    '        Next
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en validarTotalctasupervalores", _
    '                 Me.ToString(), "validarTotalctasupervalores", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try

    'End Sub
    'Private Sub Terminotraerdatosencuenta(ByVal lo As LoadOperation(Of DatosEnCuenta))
    '    Try
    '        If Not lo.HasError Then
    '            If dcProxy.DatosEnCuentas.Count = 0 Then
    '                mlogctaordensuper = False
    '            Else
    '                mlogctaordensuper = dcProxy.DatosEnCuentas.First.logCtaOrdenSuperValores
    '                If mlogctaordensuper Then
    '                    'If Not IsNothing(ListaDetalleTesoreria(lo.UserState).Debito) And ListaDetalleTesoreria(lo.UserState).Debito <> 0 Then
    '                    mcurTotalDebitoSV = mcurTotalDebitoSV + IIf(ListaDetalleTesoreria(lo.UserState).Debito Is Nothing, 0, ListaDetalleTesoreria(lo.UserState).Debito)
    '                    mcurTotalCreditoSV = mcurTotalCreditoSV + IIf(ListaDetalleTesoreria(lo.UserState).Credito Is Nothing, 0, ListaDetalleTesoreria(lo.UserState).Credito)
    '                    'End If
    '                End If
    '            End If
    '        Else
    '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerdatosencuenta", _
    '                              Me.ToString(), "Terminotraerdatosencuenta", Application.Current.ToString(), Program.Maquina, lo.Error)
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerdatosencuenta", _
    '                 Me.ToString(), "Terminotraerdatosencuenta", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub
    'Private Sub validarCtasupervaloresdelbanco()
    '    If moduloTesoreria = "CE" Then
    '        dcProxy.CuentaBancoEnCuentas.Clear()
    '        dcProxy.Load(dcProxy.TesoreriacuentabancoencuentaQuery(TesoreriSelected.IDBanco,Program.Usuario, Program.HashConexion), AddressOf Terminotraerbancoencuenta, Nothing)
    '    Else
    '        Try
    '            If mcurTotalDebitoSV <> mcurTotalCreditoSV Then
    '                A2Utilidades.Mensajes.mostrarMensaje("la suma de los débitos y los créditos no da igual para la cuenta de supervalores.Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '                Exit Sub
    '            End If
    '            recorresobregiro()
    '        Catch ex As Exception
    '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerbancoencuenta", _
    '                     Me.ToString(), "Terminotraerbancoencuenta", Application.Current.ToString(), Program.Maquina, ex)
    '        End Try
    '    End If

    'End Sub
    'Private Sub Terminotraerbancoencuenta(ByVal lo As LoadOperation(Of CuentaBancoEnCuenta))
    '    Try
    '        If Not lo.HasError Then
    '            If dcProxy.CuentaBancoEnCuentas.Count > 0 Then
    '                mlogctaordensuperbanco = dcProxy.CuentaBancoEnCuentas.First.logCtaOrdenSuperValores
    '            Else
    '                mlogctaordensuperbanco = False
    '            End If
    '            If mlogctaordensuperbanco Then
    '                If mcurTotalDebitoSV <> ((mcurTotalCreditoSV + mcurTotalDebitos - mcurTotalCreditos)) Then
    '                    A2Utilidades.Mensajes.mostrarMensaje("la suma de los débitos y los créditos no da igual para la cuenta de supervalores.Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '                    Exit Sub
    '                ElseIf mcurTotalDebitoSV <> mcurTotalCreditoSV Then
    '                    A2Utilidades.Mensajes.mostrarMensaje("la suma de los débitos y los créditos no da igual para la cuenta de supervalores.Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '                    Exit Sub
    '                End If
    '            End If
    '            recorresobregiro()
    '        Else
    '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerbancoencuenta", _
    '                              Me.ToString(), "Terminotraerbancoencuenta", Application.Current.ToString(), Program.Maquina, lo.Error)
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerbancoencuenta", _
    '                 Me.ToString(), "Terminotraerbancoencuenta", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

#End Region

#Region "Sobregiro"

    Private Async Sub recorresobregiro(Optional ByVal ItemRecorrigo As Integer = 0)
        If intIDCompaniaFirma = _TesoreriSelected.IDCompania Then
            Dim ValorNotaGMF As Double

            Select Case moduloTesoreria
                Case ClasesTesoreria.CE.ToString
                    DatosValidosCE()
                Case ClasesTesoreria.N.ToString
                    ValidarNotas()
            End Select

            Dim objRet As LoadOperation(Of OyDTesoreria.RespuetaConsultarNotaGMF)

            If EjecutarCobroGMF Then
                For Each li In ListaDetalleAnterior
                    If Not IsNothing(li.IDDocumentoNotaGMF) And EjecutarCobroGMF Then
                        dcProxy.RespuetaConsultarNotaGMFs.Clear()
                        objRet = Await dcProxy.Load(dcProxy.Tesoreria_ConsultarNotaGFMQuery(li.IDDocumentoNotaGMF, li.NombreConsecutivoNotaGMF, Program.Usuario, Program.HashConexion)).AsTask()
                        If Not IsNothing(objRet) Then
                            If objRet.HasError Then
                                If objRet.Error Is Nothing Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar la nota asociada de GMF.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                                Else
                                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar la nota asociada de GMF.", Me.ToString(), "recorresobregiro", Program.TituloSistema, Program.Maquina, objRet.Error)
                                End If
                            Else
                                Dim objListaRespuesta As List(Of OyDTesoreria.RespuetaConsultarNotaGMF)
                                Dim objRespuesta As OyDTesoreria.RespuetaConsultarNotaGMF

                                objListaRespuesta = objRet.Entities.ToList()
                                objRespuesta = objListaRespuesta.FirstOrDefault
                                ValorNotaGMF = objRespuesta.ValorNota
                                EjecutarCobroGMF = False
                            End If
                        End If
                    End If
                Next
            End If

            Dim objViewConfirmacion As New A2ComunesControl.PoPupConfirmarGeneracionProcesosMasivos()
            Dim objListaMensaje As New List(Of String)
            'Se valida que el cliente tenga saldo disponible
            'For Index = 0 To UBound(vlrTotalXCliente) 'Recorre el arreglo
            For Index = ItemRecorrigo To UBound(vlrTotalXCliente) 'Recorre el arreglo
                IndexGlobal = Index
                'Si hay cliente se valida que tenga saldo o si no se presenta el mensaje de autorizacion.
                If Not String.IsNullOrEmpty(Trim(vlrTotalXCliente(Index).lngIDComitente)) Then
                    ''Se ejecuta la operacion encargada de consultar el saldo de un cliente a la fecha dada

                    With vlrTotalXCliente(Index)
                        If ValorNotaGMF > 0 Then
                            .VlrSaldo = .VlrSaldo - TotalComprobantesAnterior - ValorNotaGMF
                        Else
                            .VlrSaldo = .VlrSaldo - TotalComprobantesAnterior
                        End If

                        If mlogCobroGmf And mlogCalculoGMFPorEncima Then
                            'hallar el valor del gmf de una cantidad
                            dblvalortotalConGMF = (.VlrDB - .VlrCR) + ((.VlrDB - .VlrCR) * msnGMF)

                            'End If
                            If (((.VlrSaldo) * -1) < dblvalortotalConGMF) And (.VlrDB) <> 0 And Not IsNothing(.VlrDB) Then
                                If moduloTesoreria = "N" Then

                                    objListaMensaje.Add("El cliente : " & .lngIDComitente _
                                                            & " No tiene saldo disponible, el pago incluyendo el GMF es por valor de " & Format(dblvalortotalConGMF, "$##,##0.00") & " y quedaría negativo en " & Format(Math.Abs(.VlrSaldo * -1 - dblvalortotalConGMF), "$##,##0.00") & ".")

                                Else

                                    objListaMensaje.Add("El cliente : " & .lngIDComitente _
                                                          & " No tiene saldo disponible, el pago incluyendo el GMF es por valor de " & Format(dblvalortotalConGMF, "$##,##0.00") & " y quedaría negativo en " & Format(Math.Abs(.VlrSaldo * -1 - dblvalortotalConGMF), "$##,##0.00") & ".")

                                End If
                            End If
                        ElseIf mlogCobroGmf And mlogCalculoGMFPorDebajo Then
                            'hallar el valor del gmf de una cantidad
                            dblvalortotalConGMF = (.VlrDB - .VlrCR)
                            Dim dblvalortotalConGMFTotal = (.VlrDB + .VlrCR)

                            'End If
                            If (((.VlrSaldo) * -1) < ((.VlrDB) - (.VlrCR))) And (.VlrDB) <> 0 And Not IsNothing(.VlrDB) Then
                                If moduloTesoreria = "N" Then

                                    objListaMensaje.Add("El cliente : " & .lngIDComitente _
                                                        & " No tiene saldo disponible, el pago incluyendo el GMF es por valor de " & Format(dblvalortotalConGMF, "$##,##0.00") & " y quedaría negativo en " & Format(Math.Abs(.VlrSaldo * -1 - dblvalortotalConGMFTotal), "$##,##0.00") & ".")

                                Else

                                    objListaMensaje.Add("El cliente : " & .lngIDComitente _
                                                        & " No tiene saldo disponible, el pago incluyendo el GMF es por valor de " & Format(dblvalortotalConGMF, "$##,##0.00") & " y quedaría negativo en " & Format(Math.Abs(.VlrSaldo * -1 - dblvalortotalConGMFTotal), "$##,##0.00") & ".")

                                End If
                            End If
                        Else

                            If (((.VlrSaldo) * -1) < ((.VlrDB) - (.VlrCR))) And (.VlrDB) <> 0 And Not IsNothing(.VlrDB) Then
                                If moduloTesoreria = "N" Then

                                    objListaMensaje.Add("El cliente : " & .lngIDComitente _
                                                            & " No tiene saldo disponible, el pago es por valor de " & Format(.VlrDB - .VlrCR, "$##,##0.00") & " y quedaría negativo en " & Format(Math.Abs(.VlrSaldo * -1 - (.VlrDB - .VlrCR)), "$##,##0.00") & ".")

                                Else

                                    objListaMensaje.Add("El cliente : " & .lngIDComitente _
                                                           & " No tiene saldo disponible, el pago es por valor de " & Format(.VlrDB - .VlrCR, "$##,##0.00") & " y quedaría negativo en " & Format(Math.Abs(.VlrSaldo * -1 - (.VlrDB - .VlrCR)), "$##,##0.00") & ".")

                                End If

                            End If

                        End If
                    End With

                End If
            Next

            If objListaMensaje.Count > 0 Then

                objViewConfirmacion.MensajeConfirmacion = "¿ Desea dejar anulado el documento para ser autorizado posteriormente ? "
                objViewConfirmacion.listaMensajes = objListaMensaje
                AddHandler objViewConfirmacion.Closed, AddressOf TerminaPreguntaSobregiroMasivo

                Program.Modal_OwnerMainWindowsPrincipal(objViewConfirmacion)
                objViewConfirmacion.ShowDialog()

                Exit Sub

            End If
        End If

        'SLB20130305 Organizar la secuencia de los eventos
        Select Case moduloTesoreria
            Case ClasesTesoreria.N.ToString
                ValidacionesServidorNC(True, True, True, True, True, True, True)
            Case ClasesTesoreria.CE.ToString
                ValidacionesServidorCE(True, True, True, True, True, True, True, True)
            Case Else
                guardar()
        End Select

    End Sub

    Private Sub TerminaPreguntaSobregiroMasivo(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2ComunesControl.PoPupConfirmarGeneracionProcesosMasivos = CType(sender, A2ComunesControl.PoPupConfirmarGeneracionProcesosMasivos)

        If objResultado.DialogResult Then
            'autorizaciones = New Autorizaciones
            'AddHandler autorizaciones.Closed, AddressOf CerroVentana
            'autorizaciones.CenterOnScreen()
            'autorizaciones.ShowModal()
            'autorizaciones.Show()
            'SBL20130213 Se asigna el sobregiro a la entidad
            _TesoreriSelected.Sobregiro = True
            msobregiro = True

            'SLB20130708 Debe recorrer el sobregiro para cada uno de los detalles de Tesorería.
            recorresobregiro(IndexGlobal + 1)
        Else
            IsBusy = False
            ValidaSobregiro = False
            NotaGMFManual = False
            ControlMensaje = False
            If logEditar = True Then
                EjecutarCobroGMF = True
            End If
        End If
    End Sub

    Private Sub TerminaPreguntaSobregiro(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

        If objResultado.CodigoLlamado = "VALIDACIONGMF" Then

            If objResultado.DialogResult Then
                validarparametrosGMF()
                Exit Sub
            Else
                IsBusy = False
                NotaGMFManual = False
                ControlMensaje = False
                Exit Sub
            End If
        End If

        If objResultado.DialogResult Then
            'autorizaciones = New Autorizaciones
            'AddHandler autorizaciones.Closed, AddressOf CerroVentana
            'autorizaciones.CenterOnScreen()
            'autorizaciones.ShowModal()
            'autorizaciones.Show()
            'SBL20130213 Se asigna el sobregiro a la entidad
            _TesoreriSelected.Sobregiro = True
            msobregiro = True

            'SLB20130708 Debe recorrer el sobregiro para cada uno de los detalles de Tesorería.
            recorresobregiro(IndexGlobal + 1)
        Else
            ValidaSobregiro = False
            NotaGMFManual = False
            ControlMensaje = False
        End If
    End Sub

#End Region

#Region "Cuenta Transferencia CE al Editar"

    Private Sub CuentaTrasferencia()
        Try
            'listCuentasbancarias = objProxy1.ItemCombos.ToList
            listCuentasbancarias.Clear()
            For Each li In objProxy1.ItemCombos.ToList
                listCuentasbancarias.Add(li)
            Next
            If Not String.IsNullOrEmpty(_TesoreriSelected.CuentaCliente) Then
                listCuentasbancarias.Add(New OYDUtilidades.ItemCombo With {.Categoria = "CuentasBancarias", .Descripcion = _TesoreriSelected.CuentaCliente, .ID = _TesoreriSelected.CuentaCliente})
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en CuentaTrasferencia",
                               Me.ToString(), "CuentaTrasferencia", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub llenarcuentacombo(Optional ByVal strcuentabancaria As String = "")
        Try
            If TesoreriSelected.FormaPagoCE = "T" Then
                If DIGITAR_CUENTAS_BENEFICIARIO_CE = "SI" Then
                    listCuentasbancarias.Clear()
                    'If listCuentasbancarias.Count = 0 Then
                    listCuentasbancarias.Add(New OYDUtilidades.ItemCombo With {.Categoria = "CuentasBancarias", .Descripcion = strcuentabancaria.Trim, .ID = strcuentabancaria.Trim})
                        'TesoreriSelected.CuentaCliente = listCuentasbancarias.First.ID
                        'JFSB 20171019
                        CuentaContableCliente = listCuentasbancarias.First.ID

                    'End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en llenarcuentacombo",
                             Me.ToString(), "llenarcuentacombo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Limpiar el combo de cuenta en transferncia electronica.
    ''' </summary>
    ''' <remarks>SLB20140606</remarks>
    Private Sub LimpiarComboCuentaTransferencia()
        listCuentasbancarias.Clear()
        _TesoreriSelected.CuentaCliente = Nothing
        'JFSB 20171019
        CuentaContableCliente = Nothing
    End Sub

#End Region

    Private Sub ConsultarFechaCierreSistema(Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(_TesoreriSelected) Then
                If _TesoreriSelected.IDCompania <> intIDCompaniaFirma Then
                    'JFSB 20180103 Se envía el parametro al llamar al método
                    ValidadorEncargos(pstrUserState)
                    'objProxy.consultarFechaCierreCompania(_TesoreriSelected.IDCompania, Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompaniaCompleted, pstrUserState)
                Else
                    objProxy.consultarFechaCierre(moduloTesoreria.Substring(0, 1), Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, pstrUserState)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre del sistema.",
                               Me.ToString(), "ConsultarFechaCierreSistema", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ValidarEstadoDocumento(ByVal pobjAccion As ACCIONES_ESTADODOCUMENTO) As System.Threading.Tasks.Task(Of Boolean)
        Dim logLlamadoExitoso As Boolean = True
        Try
            If Not IsNothing(_TesoreriSelected) Then

                Dim objRet As LoadOperation(Of OyDTesoreria.RespuestaValidacionEstadoDocumento)
                Dim objProxy As TesoreriaDomainContext

                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    objProxy = New TesoreriaDomainContext()
                Else
                    objProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                End If

                'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
                DirectCast(objProxy.DomainClient, WebDomainClient(Of TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

                ErrorForma = String.Empty

                If Not IsNothing(objProxy.RespuestaValidacionEstadoDocumentos) Then
                    objProxy.RespuestaValidacionEstadoDocumentos.Clear()
                End If

                objRet = Await objProxy.Load(objProxy.Tesoreria_ValidarEstadoDocumentoSyncQuery(pobjAccion.ToString,
                                                                                                _TesoreriSelected.IDTesoreria,
                                                                                                Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al calcular los valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "ObtenerCalculosMotor", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        Dim objListaRespuestaValidacion As List(Of RespuestaValidacionEstadoDocumento) = objRet.Entities.ToList

                        If objListaRespuestaValidacion.Where(Function(i) i.Exitoso = False).Count > 0 Then
                            logLlamadoExitoso = False
                            Dim strMensajeValidacion As String = String.Empty

                            For Each li In objListaRespuestaValidacion
                                If li.Exitoso = False Then
                                    If String.IsNullOrEmpty(strMensajeValidacion) Then
                                        strMensajeValidacion = li.Mensaje
                                    Else
                                        strMensajeValidacion = String.Format("{0}{1}{2}", strMensajeValidacion, vbCrLf, li.Mensaje)
                                    End If
                                End If
                            Next

                            mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el estado del documento.", Me.ToString(), "ValidarEstadoDocumento", Program.TituloSistema, Program.Maquina, ex)
        End Try

        Return logLlamadoExitoso
    End Function

    Public Sub RecibirRespuestaBuscadorConceptos(ByVal pintIDConcepto As Nullable(Of Integer),
                                                 ByVal pstrDetalleConcepto As String,
                                                 ByVal pstrPermiteSeleccionCliente As String,
                                                 ByVal pstrTipoMovimiento As String,
                                                 ByVal pstrCuentaContable As String,
                                                 ByVal pstrTipoRetencion As String,
                                                 ByVal pstrDetalleConceptoCompleto As String,
                                                 ByVal plngIDComitente As String,
                                                 ByVal pstrNombreCliente As String,
                                                 ByVal pstrNroDocumento As String)
        Try
            If moduloTesoreria = "RC" Then
                If Not IsNothing(_DetalleTesoreriSelected) Then
                    If Not String.IsNullOrEmpty(plngIDComitente) Then
                        _mlogBuscarCliente = False
                        _DetalleTesoreriSelected.IDComitente = plngIDComitente
                        _DetalleTesoreriSelected.Nombre = pstrNombreCliente
                        _mlogBuscarCliente = True
                        _mlogBuscarNIT = False
                        _DetalleTesoreriSelected.NIT = pstrNroDocumento
                        _mlogBuscarNIT = True
                    End If
                    _DetalleTesoreriSelected.IDConcepto = pintIDConcepto
                    _DetalleTesoreriSelected.Detalle = pstrDetalleConceptoCompleto
                    _mlogBuscarCuentaContable = False
                    _DetalleTesoreriSelected.IDCuentaContable = pstrCuentaContable
                    _mlogBuscarCuentaContable = True
                    _DetalleTesoreriSelected.ManejaCliente = pstrPermiteSeleccionCliente
                    If pstrPermiteSeleccionCliente = "N" Then
                        _DetalleTesoreriSelected.HabilitarSeleccionCliente = False
                    Else
                        If pstrPermiteSeleccionCliente = "O" Then
                            _DetalleTesoreriSelected.HabilitarSeleccionCliente = True
                        Else
                            _DetalleTesoreriSelected.HabilitarSeleccionCliente = True
                        End If
                    End If
                    cmbNombreConsecutivoHabilitado = False
                End If
            ElseIf moduloTesoreria = "CE" Then
                If Not IsNothing(_DetalleTesoreriSelected) Then
                    If Not String.IsNullOrEmpty(plngIDComitente) Then
                        _mlogBuscarCliente = False
                        _DetalleTesoreriSelected.IDComitente = plngIDComitente
                        _DetalleTesoreriSelected.Nombre = pstrNombreCliente
                        _mlogBuscarCliente = True
                        _mlogBuscarNIT = False
                        _DetalleTesoreriSelected.NIT = pstrNroDocumento
                        _mlogBuscarNIT = True
                    End If
                    _DetalleTesoreriSelected.IDConcepto = pintIDConcepto
                    _DetalleTesoreriSelected.Detalle = pstrDetalleConceptoCompleto
                    _mlogBuscarCuentaContable = False
                    _DetalleTesoreriSelected.IDCuentaContable = pstrCuentaContable
                    _mlogBuscarCuentaContable = True
                    _DetalleTesoreriSelected.ManejaCliente = pstrPermiteSeleccionCliente
                    If pstrPermiteSeleccionCliente = "N" Then
                        _DetalleTesoreriSelected.HabilitarSeleccionCliente = False
                    Else
                        If pstrPermiteSeleccionCliente = "O" Then
                            _DetalleTesoreriSelected.HabilitarSeleccionCliente = True
                        Else
                            _DetalleTesoreriSelected.HabilitarSeleccionCliente = True
                        End If
                    End If
                    cmbNombreConsecutivoHabilitado = False
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta de los conceptos.", Me.ToString(), "RecibirRespuestaBuscadorConceptos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Friend Sub buscarCompania(Optional ByVal pintCompania As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Dim intCompania As Integer = 1

        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemsQuery(pintCompania, "compania", "T", "incluircompaniasclasestodasconfirma", String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf buscarCompaniaCompleted, pstrBusqueda)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la compañia", Me.ToString(), "buscarCompania", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Friend Sub buscarCompaniaBusqueda(Optional ByVal pintCompania As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Dim intCompania As Integer = 1

        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemsQuery(pintCompania, "compania", "T", "incluircompaniasclasestodasconfirma", String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf buscarCompaniaCompletedBusqueda, pstrBusqueda)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la compañia", Me.ToString(), "buscarCompania", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarCompaniaCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.UserState.ToString = "buscarCompaniaEncabezado" Then
                    If Not IsNothing(_TesoreriSelected.IDCompania) Then
                        If lo.Entities.Where(Function(i) i.IdItem = _TesoreriSelected.IDCompania).Count > 0 Then
                            _TesoreriSelected.IDCompania = lo.Entities.Where(Function(i) i.IdItem = _TesoreriSelected.IDCompania.ToString).First.IdItem
                            _TesoreriSelected.NombreCompania = lo.Entities.Where(Function(i) i.IdItem = _TesoreriSelected.IDCompania.ToString).First.Nombre
                            ConsultarConsecutivosCompania()
                        Else
                            _TesoreriSelected.IDCompania = Nothing
                            _TesoreriSelected.NombreCompania = String.Empty
                            LimpiarConsecutivoCompania()
                            A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        _TesoreriSelected.IDCompania = Nothing
                        _TesoreriSelected.NombreCompania = String.Empty
                        LimpiarConsecutivoCompania()
                        A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                If lo.UserState.ToString = "buscarCompaniaEncabezado" Then
                    _TesoreriSelected.IDCompania = Nothing
                    _TesoreriSelected.NombreCompania = String.Empty
                    LimpiarConsecutivoCompania()
                End If
                A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia del banco", Me.ToString(), "buscarCompaniaCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarCompaniaCompletedBusqueda(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If Not IsNothing(CamposBusquedaTesoreria.IDCompania) Then
                    If lo.Entities.Where(Function(i) i.IdItem = CamposBusquedaTesoreria.IDCompania).Count > 0 Then
                        CamposBusquedaTesoreria.IDCompania = lo.Entities.Where(Function(i) i.IdItem = CamposBusquedaTesoreria.IDCompania.ToString).First.IdItem
                        CamposBusquedaTesoreria.NombreCompania = lo.Entities.Where(Function(i) i.IdItem = CamposBusquedaTesoreria.IDCompania.ToString).First.Nombre
                        ConsultarConsecutivosCompaniaBusqueda()
                    Else
                        CamposBusquedaTesoreria.IDCompania = Nothing
                        CamposBusquedaTesoreria.NombreCompania = String.Empty
                        LimpiarConsecutivoCompaniaBusqueda()
                        A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    CamposBusquedaTesoreria.IDCompania = Nothing
                    CamposBusquedaTesoreria.NombreCompania = String.Empty
                    LimpiarConsecutivoCompaniaBusqueda()
                    A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                CamposBusquedaTesoreria.IDCompania = Nothing
                CamposBusquedaTesoreria.NombreCompania = String.Empty
                LimpiarConsecutivoCompaniaBusqueda()
                A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia", Me.ToString(), "buscarCompaniaCompletedBusqueda", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Buscador Generico"

    ''' <summary>
    ''' Buscar el centro de costos, cuenta contable y Nits seleccionado.
    ''' </summary>
    ''' <param name="pstrCentroCostos"></param>
    ''' <param name="pstrBusqueda"></param>
    ''' <remarks>SLB20130710</remarks>
    Friend Sub buscarGenerico(Optional ByVal pstrCentroCostos As String = "", Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy.BuscadorGenericos.Clear()

            Dim objTipoBusqueda As New ControlBusqueda

            objTipoBusqueda.CodigoDetalle = _DetalleTesoreriSelected.IDDetalleTesoreria
            objTipoBusqueda.TopicoBusqueda = pstrBusqueda

            objProxy.Load(objProxy.buscarItemEspecificoQuery(pstrBusqueda, pstrCentroCostos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBuscadorGenerico, objTipoBusqueda)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método recibe del buscador generico.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130710</remarks>
    Private Sub TerminoTraerBuscadorGenerico(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case CType(lo.UserState, ControlBusqueda).TopicoBusqueda.ToString
                    Case "CentrosCosto"
                        If lo.Entities.ToList.Count > 0 Then
                            For Each li In _ListaDetalleTesoreria
                                If li.IDDetalleTesoreria = CType(lo.UserState, ControlBusqueda).CodigoDetalle Then
                                    li.CentroCosto = lo.Entities.First.IdItem
                                End If
                            Next
                            '_DetalleTesoreriSelected.CentroCosto = lo.Entities.First.IdItem
                        Else
                            sw = 1
                            A2Utilidades.Mensajes.mostrarMensaje("El centro de costos ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            For Each li In _ListaDetalleTesoreria
                                If li.IDDetalleTesoreria = CType(lo.UserState, ControlBusqueda).CodigoDetalle Then
                                    li.CentroCosto = Nothing
                                End If
                            Next
                            '_DetalleTesoreriSelected.CentroCosto = Nothing
                        End If
                    Case "CuentasContables", "CuentasContables_BALPYG"
                        If lo.Entities.ToList.Count > 0 Then
                            For Each li In _ListaDetalleTesoreria
                                If li.IDDetalleTesoreria = CType(lo.UserState, ControlBusqueda).CodigoDetalle Then
                                    li.IDCuentaContable = lo.Entities.First.IdItem
                                End If
                            Next
                            '_DetalleTesoreriSelected.IDCuentaContable = lo.Entities.First.IdItem
                        Else
                            sw = 1
                            A2Utilidades.Mensajes.mostrarMensaje("La cuenta contable ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                            '_DetalleTesoreriSelected.IDCuentaContable = Nothing
                            For Each li In _ListaDetalleTesoreria
                                If li.IDDetalleTesoreria = CType(lo.UserState, ControlBusqueda).CodigoDetalle Then
                                    li.IDCuentaContable = Nothing
                                End If
                            Next
                        End If
                    Case "NITS"
                        If lo.Entities.ToList.Count > 0 Then
                            For Each li In _ListaDetalleTesoreria
                                If li.IDDetalleTesoreria = CType(lo.UserState, ControlBusqueda).CodigoDetalle Then
                                    li.NIT = lo.Entities.First.CodItem
                                End If
                            Next
                            '_DetalleTesoreriSelected.NIT = lo.Entities.First.CodItem
                        Else
                            sw = 1
                            A2Utilidades.Mensajes.mostrarMensaje("El NIT ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            For Each li In _ListaDetalleTesoreria
                                If li.IDDetalleTesoreria = CType(lo.UserState, ControlBusqueda).CodigoDetalle Then
                                    li.NIT = Nothing
                                End If
                            Next
                            '_DetalleTesoreriSelected.NIT = Nothing
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos",
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(),
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

#End Region

#Region "Mensajes"

    Private Sub TerminoMostrarMensajeUsuario(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado = CType(sender, A2Utilidades.wcpMensajes)

            Select Case objResultado.CodigoLlamado.ToUpper
                Case "FORMADEPAGO"
                    TesoreriSelected.FormaPagoCE = "C"
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del mensaje usuario.", Me.ToString(), "TerminoMostrarMensajeUsuario", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "PropertyChanged"

    Private Sub _TesoreriSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _TesoreriSelected.PropertyChanged
        Try
            If Editando And CambiarPropiedades Then
                Select Case moduloTesoreria
                    Case "CE"
                        'SV20160203
                        If e.PropertyName = "NombreConsecutivo" Then
                            If Not IsNothing(TesoreriSelected.NombreConsecutivo) Then
                                If _TesoreriSelected.CompaniaBanco <> _TesoreriSelected.IDCompania Then
                                    LimpiarBanco()
                                End If

                                'JFGB20160511
                                If plogEsFondosOYD Then
                                    If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                                        MostrarConcepto = Visibility.Visible
                                        MostrarConceptoFondosOYD = Visibility.Collapsed
                                    Else
                                        MostrarConcepto = Visibility.Collapsed
                                        MostrarConceptoFondosOYD = Visibility.Visible
                                    End If
                                Else
                                    MostrarConceptoFondosOYD = Visibility.Collapsed
                                    If Not glogConceptoDetalleTesoreriaManual Then
                                        MostrarConcepto = Visibility.Visible
                                    Else
                                        MostrarConcepto = Visibility.Collapsed
                                    End If
                                End If
                            End If
                        End If

                        If e.PropertyName = "FormaPagoCE" Or e.PropertyName = "IdComitente" Or e.PropertyName = "TipoCuenta" Then

                            If Not String.IsNullOrEmpty(TesoreriSelected.IdComitente) Then
                                dcProxy.ConsutarCuentaTransferencia(TesoreriSelected.IdComitente, Program.Usuario, Program.HashConexion, AddressOf ConsutarCuentaTransferenciaCompleted, "")
                                If SplitCuenta = False Then
                                    LimpiarComboCuentaTransferencia()
                                End If
                            End If
                            If e.PropertyName = "FormaPagoCE" Then
                                If TesoreriSelected.FormaPagoCE = "T" Then
                                    'If Program.VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then 'SLB20130711 Si es City
                                    '    ValidarTransferencia()
                                    'End If
                                    HabilitaTipocheque = False
                                    TesoreriSelected.Tipocheque = Nothing
                                    objProxy1.ItemCombos.Clear()
                                    objProxy1.Load(objProxy1.cargarCombosEspecificos_ConUsuarioQuery("Tesoreria_CuentasBancarias", TesoreriSelected.IdComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoatraerCuentasbancarias, Nothing)
                                    MyBase.CambioItem("TesoreriSelected")
                                ElseIf TesoreriSelected.FormaPagoCE = "C" Then
                                    If TesoreriSelected.IdSucursalBancaria = 109 Then
                                        TesoreriSelected.SucursalBancaria = Nothing
                                        TesoreriSelected.IdSucursalBancaria = Nothing
                                    End If
                                    DeshabilitarSucursal = True
                                    HabilitaTipocheque = True
                                    If e.PropertyName = "FormaPagoCE" Then
                                        If Not String.IsNullOrEmpty(_TesoreriSelected.IdComitente) Then
                                            logReasignar = True
                                            buscarComitente(_TesoreriSelected.IdComitente, "encabezado")
                                        End If
                                    End If
                                    'objComitenteEncabezado = Nothing
                                    'ComitenteSeleccionadoM(objComitenteEncabezado, True)
                                    MyBase.CambioItem("DeshabilitarSucursal")
                                    MyBase.CambioItem("TesoreriSelected")
                                ElseIf TesoreriSelected.FormaPagoCE <> "C" And TesoreriSelected.FormaPagoCE <> "T" Then
                                    If Program.VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then 'SLB20130214 Si es City
                                        Dim strTemp As String
                                        strTemp = "Solo se permiten las siguientes formas de pago en el encabezado del documento. " & vbCrLf
                                        strTemp = strTemp & "1. CHEQUE" & vbCrLf
                                        strTemp = strTemp & "2. TRANSFERENCIA ELECTRÓNICA" & vbCrLf
                                        A2Utilidades.Mensajes.mostrarMensajeResultadoAsincronico(strTemp, Program.TituloSistema, AddressOf TerminoMostrarMensajeUsuario, "FORMADEPAGO", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        'ModificacionFormaPago = True

                                        'DeshabilitarSucursal = True
                                        'HabilitaTipocheque = True
                                        'MyBase.CambioItem("DeshabilitarSucursal")
                                        'MyBase.CambioItem("TesoreriSelected")
                                    Else
                                        TesoreriSelected.Tipocheque = Nothing
                                        HabilitaTipocheque = False
                                        TesoreriSelected.TipoCuenta = ""

                                        If e.PropertyName = "FormaPagoCE" Then
                                            If Not String.IsNullOrEmpty(_TesoreriSelected.IdComitente) Then
                                                logReasignar = True
                                                buscarComitente(_TesoreriSelected.IdComitente, "encabezado")
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            If TesoreriSelected.FormaPagoCE = "B" Then

                                'JFSB 20160903 Se ocultan las otras columnas
                                MostrarEntidad = Visibility.Collapsed
                                MostrarTipoCuenta = Visibility.Collapsed
                                MostrarNroCuenta = Visibility.Collapsed
                                MostrarTipoIdTitular = Visibility.Collapsed
                                MostrarIdTitular = Visibility.Collapsed
                                MostrarTitular = Visibility.Collapsed

                                'JFSB 20160903 Se ocultan las otras columnas
                                MostrarCodCartera = Visibility.Collapsed
                                MostrarOficinaCuentaInversion = Visibility.Collapsed
                                MostrarNombreCarteraColectiva = Visibility.Collapsed

                                MostarFormaEntrega = Visibility.Visible
                                MostarBeneficiario = Visibility.Visible
                                TipoIdentBeneficiario = Visibility.Visible
                                IdentificacionBeneficiario = Visibility.Visible
                                MostrarNombrePersonaRecoge = Visibility.Visible
                                MostrarIdentificacion = Visibility.Visible
                                MostrarOficina = Visibility.Visible
                                TesoreriSelected.TipoCuenta = ""

                            ElseIf TesoreriSelected.FormaPagoCE = "N" Then

                                MostrarEntidad = Visibility.Visible
                                MostrarTipoCuenta = Visibility.Visible
                                MostrarNroCuenta = Visibility.Visible
                                MostrarTipoIdTitular = Visibility.Visible
                                MostrarIdTitular = Visibility.Visible
                                MostrarTitular = Visibility.Visible

                                'JFSB 20160903 Se ocultan las otras columnas
                                MostarFormaEntrega = Visibility.Collapsed
                                MostarBeneficiario = Visibility.Collapsed
                                TipoIdentBeneficiario = Visibility.Collapsed
                                IdentificacionBeneficiario = Visibility.Collapsed
                                MostrarNombrePersonaRecoge = Visibility.Collapsed
                                MostrarIdentificacion = Visibility.Collapsed
                                MostrarOficina = Visibility.Collapsed

                                'JFSB 20160903 Se ocultan las otras columnas
                                MostrarCodCartera = Visibility.Collapsed
                                MostrarOficinaCuentaInversion = Visibility.Collapsed
                                MostrarNombreCarteraColectiva = Visibility.Collapsed
                                TesoreriSelected.TipoCuenta = ""

                            ElseIf TesoreriSelected.FormaPagoCE = "L" Then

                                MostrarEntidad = Visibility.Visible
                                MostrarTipoCuenta = Visibility.Visible
                                MostrarNroCuenta = Visibility.Visible
                                MostrarTipoIdTitular = Visibility.Visible
                                MostrarIdTitular = Visibility.Visible
                                MostrarTitular = Visibility.Visible
                                MostrarCodCartera = Visibility.Visible
                                MostrarOficinaCuentaInversion = Visibility.Visible
                                MostrarNombreCarteraColectiva = Visibility.Visible

                                'JFSB 20160903 Se ocultan las otras columnas
                                MostarFormaEntrega = Visibility.Collapsed
                                MostarBeneficiario = Visibility.Collapsed
                                TipoIdentBeneficiario = Visibility.Collapsed
                                IdentificacionBeneficiario = Visibility.Collapsed
                                MostrarNombrePersonaRecoge = Visibility.Collapsed
                                MostrarIdentificacion = Visibility.Collapsed
                                MostrarOficina = Visibility.Collapsed
                                TesoreriSelected.TipoCuenta = ""

                            Else

                                MostarFormaEntrega = Visibility.Collapsed
                                MostarBeneficiario = Visibility.Collapsed
                                TipoIdentBeneficiario = Visibility.Collapsed
                                IdentificacionBeneficiario = Visibility.Collapsed
                                MostrarNombrePersonaRecoge = Visibility.Collapsed
                                MostrarIdentificacion = Visibility.Collapsed
                                MostrarOficina = Visibility.Collapsed

                                MostrarEntidad = Visibility.Collapsed
                                MostrarTipoCuenta = Visibility.Collapsed
                                MostrarNroCuenta = Visibility.Collapsed
                                MostrarTipoIdTitular = Visibility.Collapsed
                                MostrarIdTitular = Visibility.Collapsed
                                MostrarTitular = Visibility.Collapsed

                                MostrarCodCartera = Visibility.Collapsed
                                MostrarOficinaCuentaInversion = Visibility.Collapsed
                                MostrarNombreCarteraColectiva = Visibility.Collapsed

                                If SplitCuenta = False Then
                                    TesoreriSelected.TipoCuenta = ""
                                End If

                            End If
                        End If

                        'Se ejecuta la operación que verifica que el cliente especificado tenga una cuenta con el indicativo “logTransferirA”
                        If e.PropertyName = "IdComitente" And Not String.IsNullOrEmpty(TesoreriSelected.IdComitente) Then
                            dcProxy.ConsutarCuentaTransferencia(TesoreriSelected.IdComitente, Program.Usuario, Program.HashConexion, AddressOf ConsutarCuentaTransferenciaCompleted, "")
                            If _mlogBuscarCliente Then
                                buscarComitente(_TesoreriSelected.IdComitente, "encabezado")
                            End If
                        End If

                        'Se ejecuta la operación encargada de consultar el consecutivo de un banco en la tabla tblConsecutivos
                        If e.PropertyName = "IDBanco" And Not IsNothing(TesoreriSelected.IDBanco) Then
                            If _mlogBuscarBancos Then
                                buscarBancos(_TesoreriSelected.IDBanco, "consultarbancos")
                                Exit Sub
                            End If
                            ConsultarConsecutivoBanco()
                        End If

                        If e.PropertyName = "lngBancoConsignacion" And Not IsNothing(TesoreriSelected.lngBancoConsignacion) Then
                            If _mlogBuscarBancos Then
                                buscarBancos(_TesoreriSelected.lngBancoConsignacion, "consultarBancoDestino")
                                Exit Sub
                            End If
                        End If

                        'SLB20130214
                        If e.PropertyName = "ChequeraAutomatica" Then
                            If _TesoreriSelected.ChequeraAutomatica Then
                                VisibilidadNumCheque = False
                                _TesoreriSelected.NumCheque = Nothing
                            Else
                                VisibilidadNumCheque = True
                            End If
                        End If

                        'SLB20130214
                        If e.PropertyName = "FormaPagoCE" Then
                            If _TesoreriSelected.FormaPagoCE = "T" Then
                                VisibilidadNumCheque = False
                                _TesoreriSelected.NumCheque = Nothing
                            Else
                                VisibilidadNumCheque = True
                                'Inicio SLB20130611
                                LimpiarComboCuentaTransferencia()
                                'Fin SLB20130611
                            End If
                        End If

                        'SLB20130611
                        If e.PropertyName = "CuentaCliente" Then

                            If SplitCuenta = False Then

                                If Not String.IsNullOrEmpty(_TesoreriSelected.CuentaCliente) Then
                                    If _TesoreriSelected.CuentaCliente.Contains("|") = True Then
                                        Dim Parte = Split(_TesoreriSelected.CuentaCliente, "|")
                                        cuentaContable = Parte.First
                                        TipoCuenta = Parte.Last
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(cuentaContable) And Not String.IsNullOrEmpty(TipoCuenta) Then
                                    SplitCuenta = True
                                    _TesoreriSelected.CuentaCliente = cuentaContable

                                End If
                            Else
                                If Not String.IsNullOrEmpty(TipoCuenta) Then
                                    _TesoreriSelected.TipoCuenta = TipoCuenta
                                    SplitCuenta = False
                                End If
                            End If


                            If Not String.IsNullOrEmpty(_TesoreriSelected.CuentaCliente) And Not String.IsNullOrEmpty(TesoreriSelected.IdComitente) Then
                                'IsBusy = True
                                dcProxy.CuentasClientes_Tesorerias.Clear()
                                dcProxy.Load(dcProxy.CuentasClientesTesoreria_ConsultarQuery(_TesoreriSelected.IdComitente, _TesoreriSelected.CuentaCliente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasClientes, "consultar")
                            End If

                            cuentaContable = String.Empty
                            TipoCuenta = String.Empty
                        End If

                    Case "RC"
                        'SV20160203
                        If e.PropertyName = "NombreConsecutivo" Then
                            If Not IsNothing(TesoreriSelected.NombreConsecutivo) Then
                                If Not IsNothing(_ChequeTesoreriSelected) Then
                                    If _ChequeTesoreriSelected.CompaniaBanco <> _TesoreriSelected.IDCompania Then
                                        _ChequeTesoreriSelected.BancoConsignacion = Nothing
                                        _ChequeTesoreriSelected.Consignacion = Nothing
                                        _ChequeTesoreriSelected.CompaniaBanco = Nothing
                                    End If
                                End If

                                'JFGB20160511
                                If plogEsFondosOYD Then
                                    If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                                        MostrarConcepto = Visibility.Visible
                                        MostrarConceptoFondosOYD = Visibility.Collapsed
                                    Else
                                        MostrarConcepto = Visibility.Collapsed
                                        MostrarConceptoFondosOYD = Visibility.Visible
                                    End If
                                Else
                                    MostrarConceptoFondosOYD = Visibility.Collapsed
                                    If Not glogConceptoDetalleTesoreriaManual Then
                                        MostrarConcepto = Visibility.Visible
                                    Else
                                        MostrarConcepto = Visibility.Collapsed
                                    End If
                                End If
                            End If
                        End If

                        If e.PropertyName = "IdComitente" And Not String.IsNullOrEmpty(TesoreriSelected.IdComitente) And _mlogBuscarCliente Then
                            buscarComitente(_TesoreriSelected.IdComitente, "encabezado")
                        End If

                    Case "N"
                        'SV20160203
                        If e.PropertyName = "NombreConsecutivo" Then
                            If Not IsNothing(TesoreriSelected.NombreConsecutivo) Then
                                If Not IsNothing(_DetalleTesoreriSelected) Then
                                    If _DetalleTesoreriSelected.CompaniaBanco <> _TesoreriSelected.IDCompania Then
                                        _DetalleTesoreriSelected.IDBanco = Nothing
                                        _DetalleTesoreriSelected.Nombre = String.Empty
                                        _DetalleTesoreriSelected.IDCuentaContable = String.Empty
                                        _DetalleTesoreriSelected.CompaniaBanco = Nothing
                                    End If
                                End If

                                'JFGB20160511
                                MostrarConceptoFondosOYD = Visibility.Collapsed
                                If Not glogConceptoDetalleTesoreriaManual Then
                                    MostrarConcepto = Visibility.Visible
                                Else
                                    MostrarConcepto = Visibility.Collapsed
                                End If
                                HabilitarCreacionDetallesNotas = True
                            End If
                        End If

                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_TesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _DetalleTesoreriSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleTesoreriSelected.PropertyChanged
        Try

            If moduloTesoreria = "N" Then

                If Editando And sw = 0 Then
                    'If e.PropertyName = "Debito" And Not IsNothing(DetalleTesoreriSelected.Credito) Then
                    '    sw = 1
                    '    DetalleTesoreriSelected.Credito = Nothing
                    '    Exit Sub
                    'End If
                    'If e.PropertyName = "Credito" And Not IsNothing(DetalleTesoreriSelected.Debito) Then
                    '    sw = 1
                    '    DetalleTesoreriSelected.Debito = Nothing
                    'End If

                    'SLB20130312 Se adiciona la busqueda del comitente desde el control 
                    If e.PropertyName = "IDComitente" And _mlogBuscarCliente Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.IDComitente) Then
                            sw = 1
                            buscarComitente(_DetalleTesoreriSelected.IDComitente, "detalle")
                        Else
                            sw = 1
                            _DetalleTesoreriSelected.Nombre = String.Empty
                            _DetalleTesoreriSelected.NIT = String.Empty
                        End If
                    End If

                    'SLB20130312 Se adiciona la busqueda del banco desde el control 
                    If e.PropertyName = "IDBanco" And Not IsNothing(_DetalleTesoreriSelected.IDBanco) Then
                        If _mlogBuscarBancos Then
                            buscarBancos(_DetalleTesoreriSelected.IDBanco, "consultarBancoDetalle")
                            Exit Sub
                        End If
                    End If

                    'SLB20130710 Se adiciona la busqueda del centro de costos desde el control 
                    If e.PropertyName = "CentroCosto" And _mlogBuscarCentroCostos Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.CentroCosto) Then
                            If ControlMensaje = False Then
                                buscarGenerico(_DetalleTesoreriSelected.CentroCosto, "CentrosCosto")
                            End If
                        End If
                    End If

                    'SLB20130710 Se adiciona la busqueda de la cuenta contable desde el control 
                    If e.PropertyName = "IDCuentaContable" And _mlogBuscarCuentaContable Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.IDCuentaContable) Then
                            If ControlMensaje = False Then
                                buscarGenerico(_DetalleTesoreriSelected.IDCuentaContable, "CuentasContables")
                            End If
                        End If
                    End If

                    'SLB20130710 Se adiciona la busqueda de la NITS desde el control 
                    If e.PropertyName = "NIT" And _mlogBuscarNIT Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.NIT) Then
                            If ControlMensaje = False Then
                                buscarGenerico(_DetalleTesoreriSelected.NIT, "NITS")
                            End If
                        End If
                    End If

                    'RBP20160708 Se adiciona la busqueda de Forma de entrega desde el control 
                    If e.PropertyName = "FormaEntrega" And _mlogBuscarFormaEntrega Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.DescripcionFormaEntrega) Then
                            buscarGenerico(_DetalleTesoreriSelected.FormaEntrega, "FormaEntrega")
                        End If
                    End If
                    If e.PropertyName = "TipoIdentBeneficiario" And _mlogBuscarTipoIdentBeneficiario Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.DescripcionTipoIdentBeneficiario) Then
                            buscarGenerico(_DetalleTesoreriSelected.TipoIdentBeneficiario, "TipoIdentBeneficiario")
                        End If
                    End If
                    If e.PropertyName = "Entidad" And _mlogBuscarEntidad Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.NombreBancoDestino) Then
                            buscarGenerico(_DetalleTesoreriSelected.NombreBancoDestino, "EntidadBancoDestino")
                        End If
                    End If
                    If e.PropertyName = "TipoCuenta" And _mlogBuscarTipoCuenta Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.DescripcionTipoCuenta) Then
                            buscarGenerico(_DetalleTesoreriSelected.DescripcionTipoCuenta, "TipoCuenta")
                        End If
                    End If
                    If e.PropertyName = "TipoIdTitular" And _mlogBuscarTipoIdTitular Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.DescripcionTipoCuenta) Then
                            buscarGenerico(_DetalleTesoreriSelected.DescripcionTipoIdTitular, "TipoIdTitular")
                        End If
                    End If

                Else
                    sw = 0
                End If
            ElseIf moduloTesoreria = "RC" Then
                If Editando And sw = 0 Then
                    'If e.PropertyName = "Debito" And Not IsNothing(DetalleTesoreriSelected.Credito) Then
                    '    sw = 1
                    '    DetalleTesoreriSelected.Credito = Nothing
                    '    Exit Sub
                    'End If
                    'If e.PropertyName = "Credito" And Not IsNothing(DetalleTesoreriSelected.Debito) Then
                    '    sw = 1
                    '    DetalleTesoreriSelected.Debito = Nothing
                    'End If
                    SumarTotales()
                    'SLB20130312 Se adiciona la busqueda del comitente desde el control 
                    If e.PropertyName = "IDComitente" And _mlogBuscarCliente Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.IDComitente) Then
                            sw = 1
                            buscarComitente(_DetalleTesoreriSelected.IDComitente, "detalle")
                        Else
                            sw = 1
                            _DetalleTesoreriSelected.Nombre = String.Empty
                            _DetalleTesoreriSelected.NIT = String.Empty
                        End If
                    End If

                    'SLB20130711 Se adiciona la busqueda del centro de costos desde el control 
                    If e.PropertyName = "CentroCosto" And _mlogBuscarCentroCostos Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.CentroCosto) Then
                            If ControlMensaje = False Then
                                buscarGenerico(_DetalleTesoreriSelected.CentroCosto, "CentrosCosto")
                            End If
                        End If
                    End If

                    'SLB20130711 Se adiciona la busqueda de la cuenta contable desde el control 
                    If e.PropertyName = "IDCuentaContable" And _mlogBuscarCuentaContable Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.IDCuentaContable) Then
                            If ControlMensaje = False Then
                                buscarGenerico(_DetalleTesoreriSelected.IDCuentaContable, "CuentasContables")
                            End If
                        End If
                    End If

                    'SLB20130711 Se adiciona la busqueda de la NITS desde el control 
                    If e.PropertyName = "NIT" And _mlogBuscarNIT Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.NIT) Then
                            If ControlMensaje = False Then
                                buscarGenerico(_DetalleTesoreriSelected.NIT, "NITS")
                            End If
                        End If
                    End If

                Else
                    sw = 0
                End If

            ElseIf moduloTesoreria = "CE" Then
                If Editando And sw = 0 Then
                    'If e.PropertyName = "Debito" And Not IsNothing(DetalleTesoreriSelected.Credito) Then
                    '    sw = 1
                    '    DetalleTesoreriSelected.Credito = Nothing
                    '    Exit Sub
                    'End If
                    'If e.PropertyName = "Credito" And Not IsNothing(DetalleTesoreriSelected.Debito) Then
                    '    sw = 1
                    '    DetalleTesoreriSelected.Debito = Nothing
                    'End If
                    SumarComprobantes()
                    'SLB20130312 Se adiciona la busqueda del comitente desde el control 
                    If e.PropertyName = "IDComitente" And _mlogBuscarCliente Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.IDComitente) Then
                            sw = 1
                            buscarComitente(_DetalleTesoreriSelected.IDComitente, "detalle")
                        Else
                            sw = 1
                            _DetalleTesoreriSelected.Nombre = String.Empty
                            _DetalleTesoreriSelected.NIT = String.Empty
                        End If
                    End If

                    'SLB20130711 Se adiciona la busqueda del centro de costos desde el control 
                    If e.PropertyName = "CentroCosto" And _mlogBuscarCentroCostos Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.CentroCosto) Then
                            If ControlMensaje = False Then
                                buscarGenerico(_DetalleTesoreriSelected.CentroCosto, "CentrosCosto")
                            End If
                        End If
                    End If

                    'SLB20130711 Se adiciona la busqueda de la cuenta contable desde el control 
                    If e.PropertyName = "IDCuentaContable" And _mlogBuscarCuentaContable Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.IDCuentaContable) Then
                            If ControlMensaje = False Then
                                buscarGenerico(_DetalleTesoreriSelected.IDCuentaContable, "CuentasContables")
                            End If
                        End If
                    End If

                    'SLB20130711 Se adiciona la busqueda de la NITS desde el control 
                    If e.PropertyName = "NIT" And _mlogBuscarNIT Then
                        If Not String.IsNullOrEmpty(_DetalleTesoreriSelected.NIT) Then
                            If ControlMensaje = False Then
                                buscarGenerico(_DetalleTesoreriSelected.NIT, "NITS")
                            End If
                        End If
                    End If

                Else
                    sw = 0
                End If
            End If

            'If e.PropertyName.Equals("IDComitente") And (moduloTesoreria.Equals("CE") Or moduloTesoreria.Equals("N")) And sw = 0 Then
            '    'Se ejecuta la operacion encargada de consultar el saldo de un cliente a la fecha dada
            '    ValidarSaldoCliente()
            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_DetalleTesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _ChequeTesoreriSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ChequeTesoreriSelected.PropertyChanged
        Try
            If moduloTesoreria = "RC" Then
                If e.PropertyName = "Valor" Then
                    SumarTotales()
                End If
                If e.PropertyName = "BancoConsignacion" Then
                    If _mlogBuscarBancos Then
                        If Not IsNothing(_ChequeTesoreriSelected.BancoConsignacion) Then
                            buscarBancos(_ChequeTesoreriSelected.BancoConsignacion, "consultarBancoDetalle")
                            Exit Sub
                        End If
                    End If
                End If
                If e.PropertyName = "NumCheque" Or e.PropertyName = "BancoGirador" Then
                    If _ChequeTesoreriSelected.NumCheque.ToString = "NaN" Then
                        _ChequeTesoreriSelected.NumCheque = Nothing
                    End If
                    If _ChequeTesoreriSelected.BancoGirador = "NaN" Then
                        _ChequeTesoreriSelected.BancoGirador = String.Empty
                    End If
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_DetalleTesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _CamposBusquedaTesoreria_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CamposBusquedaTesoreria.PropertyChanged
        If e.PropertyName.ToLower <> "NombreConsecutivo" And e.PropertyName.ToLower <> "IDDocumento" And e.PropertyName <> "Documento" And e.PropertyName <> "DisplayDate" Then
            Select Case e.PropertyName
                Case "Aprobados"
                    visibilidadSubEstados = Visibility.Collapsed
                Case Else
                    If MakeAndCheck = 1 Then
                        visibilidadSubEstados = Visibility.Visible
                    End If
            End Select
        End If
    End Sub

#End Region

#Region "Validaciones en el Servidor de CE, RC y NC"

#Region "Propiedades"

    Private _DatosEnCuentaSelected As New DatosEnCuenta
    Private _ListaDatosEnCuenta As New List(Of DatosEnCuenta)

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Método encargado de realizar todas la validaciones de un Comprobante de Egreso en el Servidor
    ''' </summary>
    ''' <param name="YaValidoMoneda"></param>
    ''' <param name="YaValidoClienteInhabil"></param>
    ''' <remarks>SLB20130208</remarks>
    Async Sub ValidacionesServidorCE(Optional ByVal YaValidoSaldo As Boolean = False, Optional ByVal YaValidoMoneda As Boolean = False,
                                     Optional ByVal YaValidoClienteInhabil As Boolean = False, Optional ByVal YaValidoCuenta As Boolean = False,
                                     Optional ByVal YaValidoDatosEnCuenta As Boolean = False, Optional ByVal YaValidoSuperVal As Boolean = False,
                                     Optional ByVal YaValidoTesoreria As Boolean = False, Optional ByVal YaValidoGMF As Boolean = False)
        Try
            If Not YaValidoSaldo Then
                ValidarSaldoCliente()
                Exit Sub
            End If
            'SLB20130208 Valida Si la Moneda del Consecutivo de Tesoreria corresponden con la Moneda del Banco
            If Not YaValidoMoneda Then
                Valida_MonedaConsecutivo_MonedaBanco()
                Exit Sub
            End If

            'SLB 20130207 Se Valida si el beneficiaro esta inhabilitado
            If Not YaValidoClienteInhabil Then
                If _TesoreriSelected.NroDocumento <> "" And _mlogActivarListaClinton Then
                    ValidaClienteInhabil()
                    Exit Sub
                End If
            End If

            'JFSB 20171201
            If Not YaValidoCuenta Then
                ReasignarCuenta()
                Exit Sub
            End If

            'SLB20130214 Se valida los datos de EnCuenta
            If Not YaValidoDatosEnCuenta Then
                ExistenDatosEnCuenta()
                Exit Sub
            End If

            'SLB20130306 Se valida la Cuenta de la Super Valores
            If Not YaValidoSuperVal Then
                If logValidacuentasuperval Then
                    ValidacionCuentaSuperVal()
                    Exit Sub
                End If
            End If

            'SV20160203
            If Not YaValidoTesoreria Then
                ValidarTesoreria()
                Exit Sub
            End If

            'CORREC_CITI_SV_2014
            'Santiago Vergara - Diciembre 05/2013 -- Si es CITI no se valida GMF
            If VersionAplicacionCliente <> EnumVersionAplicacionCliente.C.ToString AndAlso Not YaValidoGMF AndAlso glogValSobregiroCE AndAlso Not _TesoreriSelected.TrasladoEntreBancos Then 'Si no es City
                'JFSB 20160926 Se envia el detalle del registro en una variable XML para el llamado al motor de calculos.
                IsBusy = True
                Dim strXMLDetTesoreria As String

                If Not IsNothing(_ListaDetalleTesoreria) AndAlso _ListaDetalleTesoreria.Count > 0 Then

                    strXMLDetTesoreria = "<DetTesoreria>  "

                    If IsNothing(DetalleTesoreriSelected) Then
                        DetalleTesoreriSelected = _ListaDetalleTesoreria.FirstOrDefault
                    End If

                    For Each objDet In _ListaDetalleTesoreria

                        Dim strXMLDetalle = <Detalle ID=<%= objDet.IDDetalleTesoreria %>
                                                IdConcepto=<%= objDet.IDConcepto %>
                                                IDComitente=<%= objDet.IDComitente %>
                                                Documento=<%= objDet.NIT %>
                                                CuentaContable=<%= objDet.IDCuentaContable %>
                                                IDBanco=<%= objDet.IDBanco %>
                                                IDTitular=<%= objDet.IdentificacionTitular %>
                                                FormaEntrega=<%= objDet.FormaEntrega %>
                                                Tipo=<%= objDet.Tipo %>
                                                NroDocumentoBeneficiario=<%= objDet.IdentificacionBenficiciario %>>
                                            </Detalle>

                        strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString

                    Next

                    strXMLDetTesoreria = strXMLDetTesoreria & " </DetTesoreria>"

                End If

                Dim strXMLDetTesoreriaSeguro As String = System.Web.HttpUtility.HtmlEncode(strXMLDetTesoreria)
                'JFSB 20160922
                Dim objRet As LoadOperation(Of OYDUtilidades.TempRetornoCalculo)

                objRetornoCalculo = Nothing
                objProxy.TempRetornoCalculos.Clear()

                objRet = Await objProxy.Load(objProxy.VerificarCobroGMFSyncQuery(TesoreriSelected.NombreConsecutivo, TesoreriSelected.IDBanco, TesoreriSelected.Nombre, TesoreriSelected.NroDocumento, TesoreriSelected.TipoIdentificacion, TesoreriSelected.FormaPagoCE, TesoreriSelected.Tipocheque, DetalleTesoreriSelected.IDCuentaContable, TesoreriSelected.DetalleInstruccion, strXMLDetTesoreriaSeguro, TesoreriSelected.Tipo, Program.Usuario, Program.HashConexion)).AsTask()
                If Not IsNothing(objRet) Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar la generación del proceso de GMF.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar la generación del proceso de GMF.", Me.ToString(), "ConsultarLiquidacionesCompra", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If
                    Else
                        objRetornoCalculo = objRet.Entities.ToList

                        Dim objMensajeRetorno As OYDUtilidades.TempRetornoCalculo

                        objMensajeRetorno = objRetornoCalculo.FirstOrDefault

                        If objRetornoCalculo.Where(Function(i) i.Confirmacion = True And i.CobraGMF = "SI" And i.Exitoso = True And Not _
                                                       String.IsNullOrEmpty(i.MensajeCobroGMF)).Count > 0 Then

                            mostrarMensajePregunta(objMensajeRetorno.MensajeCobroGMF,
                                                        Program.TituloSistema,
                                                        "VALIDACIONGMF",
                                                        AddressOf TerminaPreguntaSobregiro, False)

                            Exit Sub

                        ElseIf objRetornoCalculo.Where(Function(i) i.Exitoso = True And i.CobraGMF = "SI").Count > 0 Then
                            validarparametrosGMF()
                            Exit Sub

                        ElseIf objRetornoCalculo.Where(Function(i) i.Exitoso = True And i.CobraGMF = "NO").Count > 0 Then
                            mlogCobroGmf = False
                            recorresobregiro()
                            Exit Sub
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objMensajeRetorno.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            IsBusyDetalles = False
                            Exit Sub
                        End If

                    End If

                End If
                IsBusy = False
                Exit Sub
            ElseIf glogValSobregiroCE AndAlso Not YaValidoGMF Then
                recorresobregiro()
                Exit Sub
            End If

            guardar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ValidacionesServidorCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de realizar todas la validaciones de un Recibo de Caja en el Servidor
    ''' </summary>
    ''' <param name="YaValidoMoneda"></param>
    ''' <param name="YaValidoClienteInhabil"></param>
    ''' <remarks>SLB20130227</remarks>
    Sub ValidacionesServidorRC(Optional ByVal YaValidoMoneda As Boolean = False, Optional ByVal YaValidoClienteInhabil As Boolean = False,
                               Optional ByVal YaValidoCuenta As Boolean = False, Optional ByVal YaValidoDatosEnCuenta As Boolean = False,
                               Optional ByVal YaValidoSuperVal As Boolean = False, Optional ByVal YaValidoTesoreria As Boolean = False)
        Try
            'SLB20130304 Valida Si la Moneda del Consecutivo de Tesoreria corresponden con la Moneda del Banco
            If Not YaValidoMoneda Then
                Valida_MonedaConsecutivo_MonedaBanco()
                Exit Sub
            End If

            'SLB 20130207 Se Valida si el beneficiaro esta inhabilitado
            If Not YaValidoClienteInhabil Then
                If _TesoreriSelected.NroDocumento <> "" And _mlogActivarListaClinton Then
                    ValidaClienteInhabil()
                    Exit Sub
                End If
            End If

            'JFSB 20171201
            If Not YaValidoCuenta Then
                ReasignarCuenta()
                Exit Sub
            End If

            'SLB20130214 Se valida los datos de EnCuenta
            If Not YaValidoDatosEnCuenta Then
                ExistenDatosEnCuenta()
                Exit Sub
            End If

            'SLB20130306 Se valida la Cuenta de la Super Valores
            If Not YaValidoSuperVal Then
                If logValidacuentasuperval Then
                    ValidacionCuentaSuperVal()
                    Exit Sub
                End If
            End If

            'SV20160203
            If Not YaValidoTesoreria Then
                ValidarTesoreria()
                Exit Sub
            End If

            guardar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ValidacionesServidorRC", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de realizar todas la validaciones de las Notas Contables en el Servidor
    ''' </summary>
    ''' <param name="YaValidoMoneda"></param>
    ''' <param name="YaValidoDatosEnCuenta"></param>
    ''' <remarks>SLB20130227</remarks>
    Async Sub ValidacionesServidorNC(Optional ByVal YaValidoSaldo As Boolean = False, Optional ByVal YaValidoMoneda As Boolean = False,
                                     Optional YaValidoCuenta As Boolean = False, Optional YaValidoDatosEnCuenta As Boolean = False,
                                     Optional ByVal YaValidoSuperVal As Boolean = False, Optional ByVal YaValidoTesoreria As Boolean = False,
                                     Optional ByVal YaValidoGMF As Boolean = False)
        Try
            'SLB20130305 Valida Si la Moneda del Consecutivo de Tesoreria corresponden con la Moneda del Banco
            If Not YaValidoSaldo Then
                ValidarSaldoCliente()
                Exit Sub
            End If

            If Not YaValidoMoneda Then
                Valida_MonedaConsecutivo_MonedaBanco()
                Exit Sub
            End If

            'JFSB 20171201
            If Not YaValidoCuenta Then
                ReasignarCuenta()
                Exit Sub
            End If

            'SLB20130214 Se valida los datos de EnCuenta
            If Not YaValidoDatosEnCuenta Then
                ExistenDatosEnCuenta()
                Exit Sub
            End If

            'SLB20130305 Se valida la Cuenta de la Super Valores
            If Not YaValidoSuperVal Then
                If logValidacuentasuperval Then
                    ValidacionCuentaSuperVal()
                    Exit Sub
                End If
            End If

            'SV20160203
            If Not YaValidoTesoreria Then
                ValidarTesoreria()
                Exit Sub
            End If

            'CORREC_CITI_SV_2014
            'Santiago Vergara - Diciembre 05/2013 -- Si es CITI no se valida GMF
            If VersionAplicacionCliente <> EnumVersionAplicacionCliente.C.ToString AndAlso Not YaValidoGMF AndAlso glogValSobregiroNC Then 'Si no es City
                IsBusy = True
                Dim strXMLDetTesoreria As String

                If Not IsNothing(_ListaDetalleTesoreria) AndAlso _ListaDetalleTesoreria.Count > 0 Then

                    strXMLDetTesoreria = "<DetTesoreria>  "

                    If IsNothing(DetalleTesoreriSelected) Then
                        DetalleTesoreriSelected = _ListaDetalleTesoreria.FirstOrDefault
                    End If

                    For Each objDet In _ListaDetalleTesoreria

                        Dim strXMLDetalle = <Detalle ID=<%= objDet.IDDetalleTesoreria %>
                                                IdConcepto=<%= objDet.IDConcepto %>
                                                IDComitente=<%= objDet.IDComitente %>
                                                Documento=<%= objDet.NIT %>
                                                CuentaContable=<%= objDet.IDCuentaContable %>
                                                IDBanco=<%= objDet.IDBanco %>
                                                IDTitular=<%= objDet.IdentificacionTitular %>
                                                FormaEntrega=<%= objDet.FormaEntrega %>
                                                Tipo=<%= objDet.Tipo %>>
                                            </Detalle>

                        strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString

                    Next

                    strXMLDetTesoreria = strXMLDetTesoreria & " </DetTesoreria>"

                End If

                Dim strXMLDetTesoreriaSeguro As String = System.Web.HttpUtility.HtmlEncode(strXMLDetTesoreria)
                'JFSB 20160922
                Dim objRet As LoadOperation(Of OYDUtilidades.TempRetornoCalculo)

                objRetornoCalculo = Nothing
                IsBusy = True
                objProxy.TempRetornoCalculos.Clear()

                objRet = Await objProxy.Load(objProxy.VerificarCobroGMFSyncQuery(TesoreriSelected.NombreConsecutivo, TesoreriSelected.IDBanco, TesoreriSelected.Nombre, TesoreriSelected.NroDocumento, TesoreriSelected.TipoIdentificacion, TesoreriSelected.FormaPagoCE, TesoreriSelected.Tipocheque, DetalleTesoreriSelected.IDCuentaContable, TesoreriSelected.DetalleInstruccion, strXMLDetTesoreriaSeguro, TesoreriSelected.Tipo, Program.Usuario, Program.HashConexion)).AsTask()

                If Not IsNothing(objRet) Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar la generación del proceso de GMF.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar la generación del proceso de GMF.", Me.ToString(), "ConsultarLiquidacionesCompra", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If
                    Else

                        objRetornoCalculo = objRet.Entities.ToList

                        Dim objMensajeRetorno As OYDUtilidades.TempRetornoCalculo

                        objMensajeRetorno = objRetornoCalculo.FirstOrDefault

                        If objRetornoCalculo.Where(Function(i) i.Confirmacion = True And i.CobraGMF = "SI" And i.Exitoso = True).Count > 0 Then

                            mostrarMensajePregunta(objMensajeRetorno.MensajeCobroGMF,
                                                        Program.TituloSistema,
                                                        "VALIDACIONGMF",
                                                        AddressOf TerminaPreguntaSobregiro, False)

                            Exit Sub

                        ElseIf objRetornoCalculo.Where(Function(i) i.Exitoso = True And i.CobraGMF = "SI").Count > 0 Then
                            validarparametrosGMF()
                            Exit Sub
                        ElseIf objRetornoCalculo.Where(Function(i) i.Exitoso = True And i.CobraGMF = "NO").Count > 0 Then
                            recorresobregiro()
                            Exit Sub
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objMensajeRetorno.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            IsBusyDetalles = False
                            Exit Sub
                        End If

                    End If

                End If
                IsBusy = False

                Exit Sub
            ElseIf glogValSobregiroNC AndAlso Not YaValidoGMF Then
                recorresobregiro()
                Exit Sub
            End If

            guardar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ValidacionesServidorNC", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para validar los datos de EnCuenta: Cuenta Contable, Centros de Costos y Nits
    ''' </summary>
    ''' <remarks>SLB20130214</remarks>
    Private Sub ExistenDatosEnCuenta()
        Try
            IsBusy = True

            Dim strXMLDetTesoreria As String = String.Empty

            If Not IsNothing(_ListaDetalleTesoreria) AndAlso _ListaDetalleTesoreria.Count > 0 Then

                strXMLDetTesoreria = "<DetTesoreria>  "

                Dim strXMLDetalle As Xml.Linq.XElement

                For Each objDet In _ListaDetalleTesoreria

                    strXMLDetalle = <Detalle CuentaContable=<%= objDet.IDCuentaContable %>
                                        Nit=<%= objDet.NIT %>
                                        CentroCosto=<%= objDet.CentroCosto %>
                                        Banco=<%= Nothing %>
                                        >
                                    </Detalle>

                    strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString
                Next

                'If moduloTesoreria = ClasesTesoreria.CE.ToString Then
                '    strXMLDetalle = <Detalle CuentaContable=<%= Nothing %>
                '                        Nit=<%= Nothing %>
                '                        CentroCosto=<%= Nothing %>
                '                        Banco=<%= _TesoreriSelected.IDBanco %>
                '                        >
                '                    </Detalle>

                '    strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString
                'End If

                'If Not IsNothing(_ListaChequeTesoreria) AndAlso _ListaChequeTesoreria.Count > 0 Then
                '    For Each li In _ListaChequeTesoreria
                '        strXMLDetalle = <Detalle CuentaContable=<%= Nothing %>
                '                            Nit=<%= Nothing %>
                '                            CentroCosto=<%= Nothing %>
                '                            Banco=<%= li.BancoConsignacion %>
                '                            >
                '                        </Detalle>

                '        strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString
                '    Next
                'End If

                strXMLDetTesoreria = strXMLDetTesoreria & " </DetTesoreria>"
            End If

            If Not String.IsNullOrEmpty(strXMLDetTesoreria) Then
                strXMLDetTesoreria = HttpUtility.HtmlEncode(strXMLDetTesoreria)

                dcProxy.DatosEnCuentas.Clear()
                ''' JFSB 20180102 Se agrega el parámetro de la compañía
                dcProxy.Load(dcProxy.ValidaDatosencuentaQuery(Nothing,
                                                              Nothing,
                                                              Nothing,
                                                              0, 0, strXMLDetTesoreria, True, TesoreriSelected.IDCompania, Program.Usuario, Program.HashConexion), AddressOf TerminoVerificarDatosEnCuenta, "Verificar")
            Else
                IsBusy = False
                Select Case moduloTesoreria
                    Case ClasesTesoreria.CE.ToString
                        ValidacionesServidorCE(True, True, True, True, True)
                    Case ClasesTesoreria.N.ToString
                        ValidacionesServidorNC(True, True, True, True)
                    Case ClasesTesoreria.RC.ToString
                        ValidacionesServidorRC(True, True, True, True)
                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerdatosencuenta",
                     Me.ToString(), "ExistenDatosEnCuenta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para validar si la moneda del consecutivo corresponde a la moneda del banco
    ''' </summary>
    ''' <remarks>SLB20130304</remarks>
    Private Sub Valida_MonedaConsecutivo_MonedaBanco()
        Try
            IsBusy = True
            Dim strXMLDetTesoreria As String = String.Empty

            Select Case moduloTesoreria
                Case ClasesTesoreria.CE.ToString
                    If Not IsNothing(_ListaDetalleTesoreria) AndAlso _ListaDetalleTesoreria.Count > 0 Then

                        strXMLDetTesoreria = "<DetTesoreria>  "
                        Dim strXMLDetalle = <Detalle IDBanco=<%= _TesoreriSelected.IDBanco %>
                                                NombreConsecutivo=<%= _TesoreriSelected.NombreConsecutivo %>
                                                >
                                            </Detalle>
                        strXMLDetTesoreria = strXMLDetTesoreria & " </DetTesoreria>"
                    End If
                Case ClasesTesoreria.N.ToString
                    If Not IsNothing(_ListaDetalleTesoreria) AndAlso _ListaDetalleTesoreria.Count > 0 Then

                        strXMLDetTesoreria = "<DetTesoreria>  "

                        For Each objDet In _ListaDetalleTesoreria

                            Dim strXMLDetalle = <Detalle IDBanco=<%= objDet.IDBanco %>
                                                    NombreConsecutivo=<%= _TesoreriSelected.NombreConsecutivo %>
                                                    >
                                                </Detalle>

                            strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString
                        Next
                        strXMLDetTesoreria = strXMLDetTesoreria & " </DetTesoreria>"
                    End If
                Case ClasesTesoreria.RC.ToString
                    If Not IsNothing(_ListaChequeTesoreria) AndAlso _ListaChequeTesoreria.Count > 0 Then

                        strXMLDetTesoreria = "<DetTesoreria>  "

                        For Each objDet In _ListaChequeTesoreria

                            Dim strXMLDetalle = <Detalle IDBanco=<%= objDet.BancoConsignacion %>
                                                    NombreConsecutivo=<%= _TesoreriSelected.NombreConsecutivo %>
                                                    >
                                                </Detalle>

                            strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString
                        Next
                        strXMLDetTesoreria = strXMLDetTesoreria & " </DetTesoreria>"
                    End If
            End Select

            If Not String.IsNullOrEmpty(strXMLDetTesoreria) Then
                strXMLDetTesoreria = HttpUtility.HtmlEncode(strXMLDetTesoreria)

                dcProxy.DatosMonedaBancoConsecutivos.Clear()
                dcProxy.Load(dcProxy.MonedaConsecutivo_Corresponde_MonedaBancoQuery(Nothing, Nothing, True, strXMLDetTesoreria, moduloTesoreria, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarMoneda, "validacion")
            Else
                IsBusy = False
                Select Case moduloTesoreria
                    Case ClasesTesoreria.CE.ToString
                        ValidacionesServidorCE(True, True)
                    Case ClasesTesoreria.N.ToString
                        ValidacionesServidorNC(True, True)
                    Case ClasesTesoreria.RC.ToString
                        ValidacionesServidorRC(True)
                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método Valida_MonedaConsecutivo_MonedaBanco",
                     Me.ToString(), "Valida_MonedaConsecutivo_MonedaBanco", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida la existencia en la base de datos de un cliente inhabil con un número de identificación o el cdigo OyD(Valores de método: Si EXISTE un cliente con el mismo Número de identificación o con un nombre semejante a un porcentaje parametrizado.)
    ''' </summary>
    ''' <remarks>SLB20130204</remarks>
    Private Sub ValidaClienteInhabil(Optional ByVal logYaValidoNroDocumento As Boolean = False)
        Try
            If Not logYaValidoNroDocumento Then
                If _TesoreriSelected.NroDocumento.Equals("") And _TesoreriSelected.Nombre.Equals("") Then Exit Sub

                If _TesoreriSelected.NroDocumento <> "" Then
                    IsBusy = True
                    objProxy.ClienteInhabilitados.Clear()
                    objProxy.Load(objProxy.ValidarClienteInhabilitadoQuery(_TesoreriSelected.NroDocumento, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteInhabilitado, "CINroDocumento")
                    Exit Sub
                End If
            End If

            If _TesoreriSelected.Nombre <> "" Then
                IsBusy = True
                objProxy.ClienteInhabilitados.Clear()
                objProxy.Load(objProxy.ValidarClienteInhabilitadoQuery("", _TesoreriSelected.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteInhabilitado, "CIidComitente")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los Clientes Inhabilitados",
                                 Me.ToString(), "ValidaClienteInhabil", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Si el beneficiario tiene alguna semejanza con un cliente Inhabilitado, si es True la respuesta
    ''' se debe grabar el Log en tblListaClinton con el parámetro _mlogGrabarListaClinton
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TerminoPreguntaSemejanza(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            _mlogGrabarListaClinton = True
            Select Case moduloTesoreria
                Case ClasesTesoreria.CE.ToString
                    ValidacionesServidorCE(True, True, True)
                Case ClasesTesoreria.RC.ToString
                    ValidacionesServidorRC(True, True)
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Método para realizar la Validación de la Cuenta de la Super Valores
    ''' </summary>
    ''' <remarks>SLB20130306</remarks>
    Private Sub ValidacionCuentaSuperVal()
        Try
            IsBusy = True
            mcurTotalDebitos = 0
            mcurTotalCreditos = 0
            mcurTotalCreditoSV = 0
            mcurTotalDebitoSV = 0
            mcurTotalCreditoNOSV = 0
            mcurTotalDebitoNOSV = 0
            mcurTotalRC = 0
            Select Case moduloTesoreria
                Case ClasesTesoreria.N.ToString
                    ValidarTotalCtaSuperValoresMod()
                    If mcurTotalDebitoSV <> mcurTotalCreditoSV Then
                        IsBusy = False
                        ControlMensaje = False
                        A2Utilidades.Mensajes.mostrarMensaje("La suma de los débitos y los créditos no da igual para la cuenta de supervalores. Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                    ValidacionesServidorNC(True, True, True, True, True)

                Case ClasesTesoreria.CE.ToString
                    'ValidarTotalCtaSuperValoresMod()
                    IsBusy = True
                    dcProxy.CuentaBancoEnCuentas.Clear()
                    ''' JFSB 20180102 Se agrega el parámetro de la compañía
                    dcProxy.Load(dcProxy.TesoreriacuentabancoencuentaQuery(TesoreriSelected.IDBanco, TesoreriSelected.IDCompania, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancoEnCuentaSuperValores, Nothing)

                Case ClasesTesoreria.RC.ToString
                    ValidarTotalCtaSuperValoresMod()
                    intNroDetallesValidarSuperBanco = 0
                    ValidarTotalCtaSuperVBanco()
            End Select

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Validacion Cuenta SuperValores",
                     Me.ToString(), "ValidacionCuentaSuperVal", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para validar el Total de la cuenta de Super Valores
    ''' </summary>
    ''' <remarks>SLB20130306</remarks>
    Private Sub ValidarTotalCtaSuperValoresMod()
        Try
            Dim consecutivo As Integer = 0
            For Each objLista In _ListaDatosEnCuenta
                If objLista.logCtaOrdenSuperValores Then
                    mcurTotalDebitoSV = mcurTotalDebitoSV + IIf(ListaDetalleTesoreria(consecutivo).Debito Is Nothing, 0, ListaDetalleTesoreria(consecutivo).Debito)
                    mcurTotalCreditoSV = mcurTotalCreditoSV + IIf(ListaDetalleTesoreria(consecutivo).Credito Is Nothing, 0, ListaDetalleTesoreria(consecutivo).Credito)
                ElseIf moduloTesoreria = ClasesTesoreria.CE.ToString Then
                    'SV20151005
                    mcurTotalDebitoNOSV = mcurTotalDebitoNOSV + IIf(ListaDetalleTesoreria(consecutivo).Debito Is Nothing, 0, ListaDetalleTesoreria(consecutivo).Debito)
                    mcurTotalCreditoNOSV = mcurTotalCreditoNOSV + IIf(ListaDetalleTesoreria(consecutivo).Credito Is Nothing, 0, ListaDetalleTesoreria(consecutivo).Credito)
                End If

                consecutivo = consecutivo + 1
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en validarTotalctasupervalores",
                     Me.ToString(), "validarTotalctasupervalores", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Método en el se validan si la suma de los débitos y créditos no dan igual para la cuenta de supervalores, tomando el cuenta si el banco de consignación del
    ''' detalle de cheques
    ''' </summary>
    ''' <remarks>SLB20130306</remarks>
    Private Sub ValidarTotalCtaSuperVBanco()
        Try
            If intNroDetallesValidarSuperBanco < ListaChequeTesoreria.Count Then
                If ListaChequeTesoreria.ElementAt(intNroDetallesValidarSuperBanco).BancoConsignacion = 0 Or IsNothing(ListaChequeTesoreria.ElementAt(intNroDetallesValidarSuperBanco).BancoConsignacion) Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("En la relación de cheques para cuentas supervalores debe ingresar el banco y fecha de consignación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadoGeneral = TabsTesoreriaRC.Cheques
                    ControlMensaje = False
                    Exit Sub
                End If
                IsBusy = True
                dcProxy.CuentaBancoEnCuentas.Clear()
                ''' JFSB 20180102 Se agrega el parámetro de la compañía
                dcProxy.Load(dcProxy.TesoreriacuentabancoencuentaQuery(ListaChequeTesoreria.ElementAt(intNroDetallesValidarSuperBanco).BancoConsignacion, TesoreriSelected.IDCompania, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancoEnCuentaSuperValoresRC, Nothing)
            Else
                If mcurTotalDebitoSV <> (mcurTotalCreditoSV + mcurTotalRC) Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("La suma de los débitos y los créditos no da igual para la cuenta de supervalores. Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ControlMensaje = False
                    Exit Sub
                End If
                ValidacionesServidorRC(True, True, True, True, True)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Validacion Cuenta SuperValores",
                     Me.ToString(), "ValidacionCuentaSuperVal", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Envía todos los datos de la orden para validarla en el servidor de sql
    ''' </summary>
    ''' <remarks>SV20160203</remarks>
    Private Sub ValidarTesoreria()
        Try
            IsBusy = True
            Dim strXMLDetTesoreria As String, strXMLDetCheques As String

            'Formar el xml con el detalle de tesorería
            If Not IsNothing(_ListaDetalleTesoreria) AndAlso _ListaDetalleTesoreria.Count > 0 Then
                strXMLDetTesoreria = "<DetTesoreria>  "

                For Each objDet In _ListaDetalleTesoreria

                    Dim strXMLDetalle = <Detalle Secuencia=<%= objDet.Secuencia %>
                                            IDBanco=<%= objDet.IDBanco %>>
                                        </Detalle>

                    strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString

                Next

                strXMLDetTesoreria = strXMLDetTesoreria & " </DetTesoreria>"
            End If

            Dim strXMLDetTesoreriaSeguro As String = System.Web.HttpUtility.HtmlEncode(strXMLDetTesoreria)

            'Formar el xml con el detalle de cheques
            If Not IsNothing(_ListaChequeTesoreria) AndAlso _ListaChequeTesoreria.Count > 0 Then
                strXMLDetCheques = "<DetCheques>  "

                For Each objCheque In _ListaChequeTesoreria

                    Dim strXMLCheque = <Detalle Secuencia=<%= objCheque.Secuencia %>
                                           BancoConsignacion=<%= objCheque.BancoConsignacion %>>
                                       </Detalle>

                    strXMLDetCheques = strXMLDetCheques & strXMLCheque.ToString
                Next

                strXMLDetCheques = strXMLDetCheques & " </DetCheques>"
            End If

            Dim strXMLDetChequesSeguro As String = System.Web.HttpUtility.HtmlEncode(strXMLDetCheques)


            dcProxy.tblRespuestaValidacionesTesorerias.Clear()
            IsBusy = True
            dcProxy.Load(dcProxy.Tesoreria_ValidacionesQuery(_TesoreriSelected.Tipo,
                                _TesoreriSelected.NombreConsecutivo,
                                _TesoreriSelected.IDDocumento,
                                _TesoreriSelected.TipoIdentificacion,
                                _TesoreriSelected.NroDocumento,
                                _TesoreriSelected.Nombre,
                                _TesoreriSelected.IDBanco,
                                strXMLDetTesoreriaSeguro,
                                strXMLDetChequesSeguro,
                                Program.Usuario, Program.HashConexion), AddressOf TerminoValidarTesoreria, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el registro de tesorería",
                                 Me.ToString(), "ValidarTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'JFSB 20171210 Se agrega metodo para reasignar las cuentas contables desde las fuentes
    Async Sub ReasignarCuenta()
        Try
            IsBusy = True
            ControlMensaje = True
            Dim strXMLDetTesoreria As String

            If Not IsNothing(_ListaDetalleTesoreria) AndAlso _ListaDetalleTesoreria.Count > 0 Then

                strXMLDetTesoreria = "<root>  "

                If IsNothing(DetalleTesoreriSelected) Then
                    DetalleTesoreriSelected = _ListaDetalleTesoreria.FirstOrDefault
                End If

                For Each objDet In _ListaDetalleTesoreria
                    Dim strXMLCuentasContables = <Datos intID=<%= objDet.IDDetalleTesoreria %>
                                                     strIDCuentaContable=<%= objDet.IDCuentaContable %>
                                                     strNombreConsecutivo=<%= _TesoreriSelected.NombreConsecutivo %>
                                                     LngIDConcepto=<%= objDet.IDConcepto %>
                                                     lngIdComitente=<%= objDet.IDComitente %>
                                                     lngIdBanco=<%= IIf(moduloTesoreria = ClasesTesoreria.N.ToString, objDet.IDBanco, Nothing) %>
                                                     strTipo=<%= IIf(moduloTesoreria = ClasesTesoreria.CE.ToString, "EGRESOS", IIf(moduloTesoreria = ClasesTesoreria.RC.ToString, "CAJA", "NOTAS")) %>
                                                     lngIdBancoDoc=<%= IIf(moduloTesoreria = ClasesTesoreria.CE.ToString, _TesoreriSelected.IDBanco, IIf(moduloTesoreria = ClasesTesoreria.N.ToString, objDet.IDBanco, Nothing)) %>
                                                     lngSecuencia=<%= objDet.Secuencia %>>
                                                 </Datos>

                    strXMLDetTesoreria = strXMLDetTesoreria & strXMLCuentasContables.ToString

                Next

                strXMLDetTesoreria = strXMLDetTesoreria & " </root>"

                Dim strXMLDetTesoreriaSeguro As String = System.Web.HttpUtility.HtmlEncode(strXMLDetTesoreria)
                Dim objRet As OpenRiaServices.DomainServices.Client.LoadOperation(Of OyDTesoreria.RetornoReasignarCuentas)

                dcProxy.RetornoReasignarCuentas.Clear()
                objRet = Await dcProxy.Load(dcProxy.Tesoreria_ReasignarCuentaContableSyncQuery(strXMLDetTesoreriaSeguro, Program.Usuario, Program.HashConexion)).AsTask()

                If Not IsNothing(objRet) Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al reasignar la cuenta contable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al reasignar la cuenta contable.", Me.ToString(), "ReasignarCuenta", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If
                    Else
                        objRetornoReasignarCuenta = objRet.Entities.ToList

                        Dim objListaTesoreria As New List(Of DetalleTesoreri)

                        For Each li In ListaDetalleTesoreria
                            objListaTesoreria.Add(li)
                        Next

                        For Each li In objRetornoReasignarCuenta

                            For Each obj In objListaTesoreria.Where(Function(i) i.IDCuentaContable = li.strIDCuentaContable)
                                obj.IDCuentaContable = li.strIDCuentaContableNueva
                            Next
                        Next

                        ListaDetalleTesoreria = objListaTesoreria

                        Select Case moduloTesoreria
                            Case ClasesTesoreria.CE.ToString
                                ValidacionesServidorCE(True, True, True, True)
                            Case ClasesTesoreria.N.ToString
                                ValidacionesServidorNC(True, True, True)
                            Case ClasesTesoreria.RC.ToString
                                ValidacionesServidorRC(True, True, True)
                        End Select
                    End If
                End If

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reasignar la cuenta",
                                 Me.ToString(), "ReasignarCuenta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos"

    ''' <summary>
    ''' Recibe la respuesta de la verificación de Encuenta
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130214</remarks>
    Private Sub TerminoVerificarDatosEnCuenta(ByVal lo As LoadOperation(Of DatosEnCuenta))
        Try
            If Not lo.HasError Then

                Dim logExitoso As Boolean = True
                Dim objListaMensajes As List(Of String) = New List(Of String)
                If logValidacuentasuperval Then
                    _ListaDatosEnCuenta = New List(Of DatosEnCuenta)
                End If

                For Each li In lo.Entities.ToList
                    If li.logExitoso = False Then
                        Dim strMensajeValidar = li.strMensaje.Split("&")
                        For Each objMensaje In strMensajeValidar
                            objListaMensajes.Add(objMensaje)
                        Next
                        logExitoso = False
                        NotaGMFManual = False
                    End If

                    If logValidacuentasuperval Then
                        _ListaDatosEnCuenta.Add(li)
                    End If
                Next

                If logExitoso Then
                    'IsBusy = False
                    Select Case moduloTesoreria
                        Case ClasesTesoreria.CE.ToString
                            ValidacionesServidorCE(True, True, True, True, True)
                        Case ClasesTesoreria.N.ToString
                            ValidacionesServidorNC(True, True, True, True)
                        Case ClasesTesoreria.RC.ToString
                            ValidacionesServidorRC(True, True, True, True)
                    End Select
                Else
                    Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                    objViewImportarArchivo.ListaMensajes = objListaMensajes

                    objViewImportarArchivo.Title = "Validaciones contables"
                    Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                    objViewImportarArchivo.ShowDialog()
                    IsBusy = False
                    ControlMensaje = False
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerdatosencuenta",
                                  Me.ToString(), "Terminotraerdatosencuenta", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerdatosencuenta",
                     Me.ToString(), "Terminotraerdatosencuenta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que recibe la respuesta Si la Moneda del Consecutivo de Tesoreria corresponden con la Moneda del Banco
    ''' </summary>
    ''' <param name="lo">
    ''' Si la Respuesta es True continua con la validaciones del CE
    ''' </param>
    ''' <remarks>SLB20130208</remarks>
    Private Sub TerminoValidarMoneda(ByVal lo As LoadOperation(Of DatosMonedaBancoConsecutivo))
        IsBusy = False
        If Not lo.HasError Then
            Dim logExitoso As Boolean = True
            Dim objListaMensajes As List(Of String) = New List(Of String)

            For Each li In lo.Entities.ToList
                If li.logExitoso = False Then
                    objListaMensajes.Add(li.strMensaje)
                    logExitoso = False
                End If
            Next

            If logExitoso Then
                Select Case moduloTesoreria
                    Case ClasesTesoreria.CE.ToString
                        ValidacionesServidorCE(True, True)
                    Case ClasesTesoreria.N.ToString
                        ValidacionesServidorNC(True, True)
                    Case ClasesTesoreria.RC.ToString
                        ValidacionesServidorRC(True)
                End Select
            Else
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                objViewImportarArchivo.ListaMensajes = objListaMensajes

                objViewImportarArchivo.Title = "Validaciones consecutivo"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()
                IsBusy = False
                ControlMensaje = False
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoTrasladarTesoreria" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    ''' <summary>
    ''' Recibe la respuesta si el beneficiario de tesoreria tiene semejanzas con algún cliente inhabilitado o si esta inhabilitado
    ''' </summary>
    ''' <param name="lo">SLB20130204</param>
    ''' <remarks></remarks>
    Private Sub TerminoConsultarClienteInhabilitado(ByVal lo As LoadOperation(Of OYDUtilidades.ClienteInhabilitado))
        IsBusy = False
        If Not lo.HasError Then
            Select Case lo.UserState
                Case "CINroDocumento"
                    If lo.Entities.Count > 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("El documento " & _TesoreriSelected.NroDocumento.ToString & " ya existe como Inhabilitado." & vbCr &
                                                             "Pertenece a: " & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                                             "Fecha: " & lo.Entities.First.Ingreso,
                                                             Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        ValidaClienteInhabil(True)
                    End If
                Case "CIidComitente"
                    If lo.Entities.Count > 0 Then
                        'C1.Silverlight.C1MessageBox.Show("El Cliente: " & _TesoreriSelected.IdComitente & " Tiene semejanza con el cliente Inhabilitado " _
                        '                                 & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?", _
                        '                                 Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaSemejanza)
                        mostrarMensajePregunta("El Cliente: " & _TesoreriSelected.IdComitente & " Tiene semejanza con el cliente Inhabilitado " _
                                               & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%",
                                               Program.TituloSistema,
                                               "CLIENTEINHABILITADO",
                                               AddressOf TerminoPreguntaSemejanza, True, "¿Desea continuar?")
                        DatosListaClinton = lo.Entities.First
                    Else
                        Select Case moduloTesoreria
                            Case ClasesTesoreria.CE.ToString
                                ValidacionesServidorCE(True, True, True)
                            Case ClasesTesoreria.RC.ToString
                                ValidacionesServidorRC(True, True)
                        End Select
                    End If
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los paramétros",
                                                 Me.ToString(), "TerminoConsultarClienteInhabilitado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' Método en el se verifica si el banco del CE tiene cuenta de Orden de Super Valores
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130306</remarks>
    Private Sub TerminoTraerBancoEnCuentaSuperValores(ByVal lo As LoadOperation(Of CuentaBancoEnCuenta))
        Try
            IsBusy = False
            If Not lo.HasError Then
                If dcProxy.CuentaBancoEnCuentas.Count > 0 Then
                    mlogctaordensuperbanco = dcProxy.CuentaBancoEnCuentas.First.logCtaOrdenSuperValores
                Else
                    mlogctaordensuperbanco = False
                End If

                ValidarTotalCtaSuperValoresMod()
                'SV20151005 - Si la cuenta del banco es de supervalores los debitos y los creditos de las cuentas del detalle que no son
                'de supervalores deben ser iguales ya que lo demas afeca al banco.
                If mlogctaordensuperbanco And mcurTotalDebitoNOSV <> mcurTotalCreditoNOSV Then
                    A2Utilidades.Mensajes.mostrarMensaje("La suma de los débitos y los créditos no da igual para la cuenta de supervalores. Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ControlMensaje = False
                    Exit Sub
                ElseIf mlogctaordensuperbanco = False And mcurTotalDebitoSV <> mcurTotalCreditoSV Then
                    'SV20151005 - Si la cuenta del banco no es de supervalores los debitos y los creditos de las cuentas del detalle que son de supervalores deben ser iguales
                    A2Utilidades.Mensajes.mostrarMensaje("La suma de los débitos y los créditos no da igual para la cuenta de supervalores. Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ControlMensaje = False
                    Exit Sub
                End If

                ValidacionesServidorCE(True, True, True, True, True, True)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TerminoTraerBancoEnCuentaSuperValores",
                                  Me.ToString(), "TerminoTraerBancoEnCuentaSuperValores", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerbancoencuenta",
                     Me.ToString(), "Terminotraerbancoencuenta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método en el se verifica si los bancos del detalle de cheques de RC tiene cuenta de Orden de Super Valores
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130306</remarks>
    Private Sub TerminoTraerBancoEnCuentaSuperValoresRC(ByVal lo As LoadOperation(Of CuentaBancoEnCuenta))
        Try
            IsBusy = False
            If Not lo.HasError Then
                If dcProxy.CuentaBancoEnCuentas.Count > 0 Then
                    mlogctaordensuperbanco = dcProxy.CuentaBancoEnCuentas.First.logCtaOrdenSuperValores
                Else
                    mlogctaordensuperbanco = False
                End If
                If mlogctaordensuperbanco Then
                    mcurTotalRC = mcurTotalRC + ListaChequeTesoreria.ElementAt(intNroDetallesValidarSuperBanco).Valor
                End If
                intNroDetallesValidarSuperBanco = intNroDetallesValidarSuperBanco + 1
                ValidarTotalCtaSuperVBanco()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TerminoTraerBancoEnCuentaSuperValores",
                                  Me.ToString(), "TerminoTraerBancoEnCuentaSuperValores", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Terminotraerbancoencuenta",
                     Me.ToString(), "Terminotraerbancoencuenta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Recibe la respuesta devuelta por el servidor en lal consulta de validaciones
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20160203</remarks>
    Private Sub TerminoValidarTesoreria(ByVal lo As LoadOperation(Of tblRespuestaValidacionesTesoreria))
        Try
            If Not lo.HasError Then
                Dim strMensajeError As String, logErrores As Boolean
                strMensajeError = "El registro no se pugo grabar:" & vbCrLf
                For Each obj In dcProxy.tblRespuestaValidacionesTesorerias.ToList
                    If obj.Exitoso = False Then
                        logErrores = True
                        strMensajeError = strMensajeError & " - " & obj.Mensaje & vbCrLf
                    End If
                Next

                If logErrores Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMensajeError, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    ControlMensaje = False
                    Exit Sub
                End If

                Select Case moduloTesoreria
                    Case ClasesTesoreria.CE.ToString
                        ValidacionesServidorCE(True, True, True, True, True, True, True)
                    Case ClasesTesoreria.RC.ToString
                        ValidacionesServidorRC(True, True, True, True, True, True)
                    Case ClasesTesoreria.N.ToString
                        ValidacionesServidorNC(True, True, True, True, True, True)
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de validar los datos de tesorería",
                        Me.ToString(), "TerminoValidarTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de validar los datos de tesorería",
                     Me.ToString(), "TerminoValidarTesoreria", Application.Current.ToString(), Program.Maquina, ex)
            'Finally
            '    IsBusy = False
        End Try
    End Sub

#End Region

#End Region

#Region "Imprimir Reportes"

#Region "Declaraciones"

    Dim intContadorVerificacion As Integer

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Metodo para imprimir reportes de los formularios de tesoreria
    ''' </summary>
    ''' <remarks>SLB20130228</remarks>
    Public Sub ImprimirReporte(ByVal strNombreReporte As String)
        Try
            Dim strParametros As String = String.Empty
            Dim strReporte As String = String.Empty
            Dim strNroVentana As String = String.Empty

            If Not IsNothing(_TesoreriSelected) Then
                If _TesoreriSelected.Estado = "P" Then
                    Select Case strNombreReporte
                        Case "ImprimirRecibosCaja"
                            If Application.Current.Resources.Contains("A2VReporteImprimirRecibosCaja") = False Then
                                A2Utilidades.Mensajes.mostrarMensaje("El reporte para imprimir el recibo de caja no está configurado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            strReporte = Application.Current.Resources("A2VReporteImprimirRecibosCaja").ToString.Trim()
                            strParametros = "&pstrEstado=" & _TesoreriSelected.Estado & "&pstrConsecutivoCaja=" & _TesoreriSelected.NombreConsecutivo & "&pintDesde=" & _TesoreriSelected.IDDocumento & "&pintHasta=" & _TesoreriSelected.IDDocumento & "&pstrFormLlamado=IMPRIMIR"
                        Case "ImprimirRecibosCajaProvi"
                            If Application.Current.Resources.Contains("A2VReporteImprimirRecibosCajaProvi") = False Then
                                A2Utilidades.Mensajes.mostrarMensaje("El reporte para imprimir el recibo de caja provisional no está configurado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            strReporte = Application.Current.Resources("A2VReporteImprimirRecibosCajaProvi").ToString.Trim()
                            strParametros = "&pstrEstado=" & _TesoreriSelected.Estado & "&pstrConsecutivoCaja=" & _TesoreriSelected.NombreConsecutivo & "&pintDesde=" & _TesoreriSelected.IDDocumento & "&pintHasta=" & _TesoreriSelected.IDDocumento
                        Case "ImprimirNotaContable"
                            If Application.Current.Resources.Contains("A2VReporteImprimirNotaContable") = False Then
                                A2Utilidades.Mensajes.mostrarMensaje("El reporte para imprimir la nota contable no está configurado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            strReporte = Application.Current.Resources("A2VReporteImprimirNotaContable").ToString.Trim()
                            strParametros = "&plngReciboDesde=" & _TesoreriSelected.IDDocumento & "&plngReciboHasta=" & _TesoreriSelected.IDDocumento & "&pstrTipoConsecutivo=" & _TesoreriSelected.NombreConsecutivo & "&pstrEstado=" & _TesoreriSelected.Estado
                    End Select

                    MostrarReporte(strParametros, Me.ToString, strReporte)

                    If Not strNombreReporte = "ImprimirRecibosCajaProvi" Then
                        System.Threading.Thread.Sleep(5000)
                        IsBusy = True
                        intContadorVerificacion = 0
                        'Procedimiento para evaluar si ya se lanzo el reporte
                        dcProxy.Verificar_EstadoImpresion(_TesoreriSelected.IDTesoreria, Program.Usuario, Program.HashConexion, AddressOf TeminoVericarEstadoImpresion, "verificar")
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Solo se pueden imprimir documentos pendientes", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la impresión del reporte", Me.ToString(), "imprimirReporte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos"

    ''' <summary>
    ''' Método para verificar si el reporte se imprimio correctamente, si se imprimio correctamente se cambia el estado del documento de Tesorería a Impreso y en
    ''' caso contrario se vuelve a verificar si ya se imprimio el reporte, si ha pasado un numero de veces predeterminado quiere decir que ocurrio un error con
    ''' el reporte.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130312</remarks>
    Private Sub TeminoVericarEstadoImpresion(ByVal lo As InvokeOperation(Of Boolean))
        Try
            IsBusy = False
            If Not lo.HasError Then
                If lo.Value Then
                    _TesoreriSelected.Estado = "I"
                Else
                    If intContadorVerificacion < 20 Then
                        System.Threading.Thread.Sleep(3000)
                        IsBusy = True
                        intContadorVerificacion = intContadorVerificacion + 1
                        dcProxy.Verificar_EstadoImpresion(_TesoreriSelected.IDTesoreria, Program.Usuario, Program.HashConexion, AddressOf TeminoVericarEstadoImpresion, "verificar")
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un error con el reporte de impresión del documento de tesorería", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TeminoVericarEstadoImpresion",
                                  Me.ToString(), "TeminoVericarEstadoImpresion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TeminoVericarEstadoImpresion",
                     Me.ToString(), "TeminoVericarEstadoImpresion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#End Region

#Region "Ordenes pendientes de Tesorería"

    ''' <summary>
    ''' Se abre el Child Window el cual trae las Ordenes de Tesoreria Pendientes
    ''' </summary>
    ''' <remarks>SLB20120118</remarks>
    Public Sub OrdenesPendientesTesoreria_Mostar()
        Try
            cwOrdenesPendientesTesoreria = New cwOrdenesPendientesTesoreria(moduloTesoreria)
            AddHandler cwOrdenesPendientesTesoreria.Closed, AddressOf CerroVentanaOrdenesPendientes
            Program.Modal_OwnerMainWindowsPrincipal(cwOrdenesPendientesTesoreria)
            cwOrdenesPendientesTesoreria.ShowDialog()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar un nuevo detalle de los Pendientes",
                                 Me.ToString(), "OrdenesPendientesTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Resultado del Child Windows de la Ordenes de Tesoreria Pendientes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB20120118</remarks>
    Private Sub CerroVentanaOrdenesPendientes(sender As Object, e As EventArgs)
        Try
            If cwOrdenesPendientesTesoreria.DialogResult.Value Then
                If Not IsNothing(cwOrdenesPendientesTesoreria.OrdenesTesoreriaSelected) Then
                    _OrdenPendienteTesoreria = cwOrdenesPendientesTesoreria.OrdenesTesoreriaSelected
                    OrdenesPendientesTesoreria_Adicionar()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana del las Ordenes Pendientes de Tesoreria",
                     Me.ToString(), "CerroVentanaOrdenesPendientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para crear la NC, CE o RC dependiendo de la Orden de Tesoreria Pendientes Seleccionada.
    ''' </summary>
    ''' <remarks>SLB20120118</remarks>
    Sub OrdenesPendientesTesoreria_Adicionar()
        Try
            _mlogBuscarCliente = False
            Select Case _OrdenPendienteTesoreria.Tipo
                Case "CE"
                    If _OrdenPendienteTesoreria.Beneficiario <> "" Then
                        _TesoreriSelected.Nombre = _OrdenPendienteTesoreria.Beneficiario
                    Else
                        _TesoreriSelected.Nombre = _OrdenPendienteTesoreria.NombreCliente
                        _TesoreriSelected.IdComitente = _OrdenPendienteTesoreria.CodCliente
                    End If

                    If _OrdenPendienteTesoreria.IDBancoGirador <> 0 Then
                        _TesoreriSelected.IDBanco = _OrdenPendienteTesoreria.IDBancoGirador
                    End If

                    If _OrdenPendienteTesoreria.NroCheque <> 0 Then
                        TesoreriSelected.NumCheque = _OrdenPendienteTesoreria.NroCheque
                    End If
                    _TesoreriSelected.FormaPagoCE = _OrdenPendienteTesoreria.RetornoFormaPago

                    If _OrdenPendienteTesoreria.NroDocumentoCliente <> 0 AndAlso Not IsNothing(_OrdenPendienteTesoreria.NroDocumentoCliente) Then
                        _TesoreriSelected.NroDocumento = _OrdenPendienteTesoreria.NroDocumentoCliente
                    End If

                    If _OrdenPendienteTesoreria.TipoIdentificacionCliente <> "" AndAlso Not IsNothing(_OrdenPendienteTesoreria.TipoIdentificacionCliente) Then
                        _TesoreriSelected.TipoIdentificacion = _OrdenPendienteTesoreria.TipoIdentificacionCliente
                    End If

                    If ListaDetalleTesoreria.Count = 0 Then
                        Dim NewDetalleTesoreri As New DetalleTesoreri

                        NewDetalleTesoreri.Usuario = Program.Usuario
                        NewDetalleTesoreri.Actualizacion = Now.Date
                        Dim result As Integer = 1
                        For Each value In ListaDetalleTesoreriaAnt
                            result = Math.Max(result, value.Secuencia) + 1
                        Next
                        NewDetalleTesoreri.Secuencia = result

                        'JFGB20160511
                        If plogEsFondosOYD Then
                            If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                                NewDetalleTesoreri.HabilitarSeleccionBanco = True
                                NewDetalleTesoreri.HabilitarSeleccionCliente = True
                                NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True
                            Else
                                NewDetalleTesoreri.HabilitarSeleccionBanco = False
                                NewDetalleTesoreri.HabilitarSeleccionCliente = False
                                NewDetalleTesoreri.HabilitarSeleccionCuentaContable = False
                            End If
                        Else
                            NewDetalleTesoreri.HabilitarSeleccionBanco = True
                            NewDetalleTesoreri.HabilitarSeleccionCliente = True
                            NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True
                        End If

                        AdicionarRegistroEnDetalle(ListaDetalleTesoreria, NewDetalleTesoreri)
                        ListaDetalleTesoreriaAnt.Add(New secuenciadetalletesoreira With {.Secuencia = result})
                        DetalleTesoreriSelected = NewDetalleTesoreri
                    Else
                        DetalleTesoreriSelected = ListaDetalleTesoreria.First
                    End If

                    _DetalleTesoreriSelected.Tipo = TesoreriSelected.Tipo
                    _DetalleTesoreriSelected.IDComitente = _OrdenPendienteTesoreria.CodCliente
                    _DetalleTesoreriSelected.Nombre = _OrdenPendienteTesoreria.NombreCliente
                    _DetalleTesoreriSelected.Detalle = _OrdenPendienteTesoreria.Detalle
                    _DetalleTesoreriSelected.IDCuentaContable = _OrdenPendienteTesoreria.CtaContable
                    If _OrdenPendienteTesoreria.ValorSaldo < 0 Then
                        _DetalleTesoreriSelected.Credito = _OrdenPendienteTesoreria.ValorSaldo
                    Else
                        _DetalleTesoreriSelected.Debito = _OrdenPendienteTesoreria.ValorSaldo
                    End If
                    _DetalleTesoreriSelected.Valor = _OrdenPendienteTesoreria.ValorSaldo
                    _DetalleTesoreriSelected.ConsecutivoConsignacionOPT = _OrdenPendienteTesoreria.ConsecutivoConsignacion

                    _DetalleTesoreriSelected.NombreConsecutivo = TesoreriSelected.NombreConsecutivo

                    MyBase.CambioItem("DetalleTesoreriSelected")
                    MyBase.CambioItem("ListaDetalleTesoreria")

                Case "RC"
                    If _OrdenPendienteTesoreria.Beneficiario <> "" Then
                        _TesoreriSelected.Nombre = _OrdenPendienteTesoreria.Beneficiario
                    Else
                        _TesoreriSelected.Nombre = _OrdenPendienteTesoreria.NombreCliente
                        _TesoreriSelected.IdComitente = _OrdenPendienteTesoreria.CodCliente
                    End If

                    If _OrdenPendienteTesoreria.NroDocumentoCliente <> 0 AndAlso Not IsNothing(_OrdenPendienteTesoreria.NroDocumentoCliente) Then
                        _TesoreriSelected.NroDocumento = _OrdenPendienteTesoreria.NroDocumentoCliente
                    End If

                    If _OrdenPendienteTesoreria.TipoIdentificacionCliente <> "" AndAlso Not IsNothing(_OrdenPendienteTesoreria.TipoIdentificacionCliente) Then
                        _TesoreriSelected.TipoIdentificacion = _OrdenPendienteTesoreria.TipoIdentificacionCliente
                    End If

                    'Detalle de Recibos
                    Dim result As Integer = 1
                    For Each value In ListaDetalleTesoreriaAnt
                        result = Math.Max(result, value.Secuencia) + 1
                    Next

                    If ListaDetalleTesoreria.Count = 0 Then
                        Dim NewDetalleTesoreri As New DetalleTesoreri

                        NewDetalleTesoreri.Tipo = _OrdenPendienteTesoreria.Tipo
                        NewDetalleTesoreri.NombreConsecutivo = TesoreriSelected.NombreConsecutivo
                        NewDetalleTesoreri.Usuario = Program.Usuario
                        NewDetalleTesoreri.Actualizacion = Now.Date

                        NewDetalleTesoreri.Secuencia = result

                        'JFGB20160511
                        If plogEsFondosOYD Then
                            If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                                NewDetalleTesoreri.HabilitarSeleccionBanco = True
                                NewDetalleTesoreri.HabilitarSeleccionCliente = True
                                NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True
                            Else
                                NewDetalleTesoreri.HabilitarSeleccionBanco = False
                                NewDetalleTesoreri.HabilitarSeleccionCliente = False
                                NewDetalleTesoreri.HabilitarSeleccionCuentaContable = False
                            End If
                        Else
                            NewDetalleTesoreri.HabilitarSeleccionBanco = True
                            NewDetalleTesoreri.HabilitarSeleccionCliente = True
                            NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True
                        End If

                        AdicionarRegistroEnDetalle(ListaDetalleTesoreria, NewDetalleTesoreri)
                        ListaDetalleTesoreriaAnt.Add(New secuenciadetalletesoreira With {.Secuencia = result})
                        DetalleTesoreriSelected = NewDetalleTesoreri
                    Else
                        DetalleTesoreriSelected = ListaDetalleTesoreria.First
                    End If

                    _DetalleTesoreriSelected.IDComitente = _OrdenPendienteTesoreria.CodCliente
                    _DetalleTesoreriSelected.Nombre = _OrdenPendienteTesoreria.NombreCliente
                    _DetalleTesoreriSelected.Detalle = _OrdenPendienteTesoreria.Detalle
                    _DetalleTesoreriSelected.IDCuentaContable = _OrdenPendienteTesoreria.CtaContable
                    _DetalleTesoreriSelected.Debito = _OrdenPendienteTesoreria.ValorSaldo
                    _DetalleTesoreriSelected.ConsecutivoConsignacionOPT = _OrdenPendienteTesoreria.ConsecutivoConsignacion

                    MyBase.CambioItem("DetalleTesoreriSelected")
                    MyBase.CambioItem("ListaDetalleTesoreria")

                    'Detalle de Cheques
                    If ListaChequeTesoreria.Count = 0 Then
                        Dim newChequeTesoreria As New Cheque

                        Dim result2 As Integer = 1
                        For Each value In ListaChequeTesoreriaAnt
                            result2 = Math.Max(result, value.Secuencia) + 1
                        Next
                        newChequeTesoreria.Secuencia = result2
                        newChequeTesoreria.Usuario = Program.Usuario
                        newChequeTesoreria.Actualizacion = Now.Date
                        newChequeTesoreria.ChequeHizoCanje = True 'SLB20130802 Manejo de cheques de canje
                        AdicionarRegistroEnDetalleCheque(ListaChequeTesoreria, newChequeTesoreria)
                        ListaChequeTesoreriaAnt.Add(New secuenciachequestesoreria With {.Secuencia = result})
                        ChequeTesoreriSelected = newChequeTesoreria
                    Else
                        ChequeTesoreriSelected = ListaChequeTesoreria.First
                    End If

                    _ChequeTesoreriSelected.FormaPagoRC = _OrdenPendienteTesoreria.RetornoFormaPago
                    _ChequeTesoreriSelected.Valor = _OrdenPendienteTesoreria.ValorSaldo
                    _ChequeTesoreriSelected.Consignacion = _OrdenPendienteTesoreria.FechaConsignacion
                    _ChequeTesoreriSelected.BancoGirador = _OrdenPendienteTesoreria.IDBancoGirador
                    _ChequeTesoreriSelected.NumCheque = _OrdenPendienteTesoreria.NroCheque
                    _ChequeTesoreriSelected.BancoConsignacion = _OrdenPendienteTesoreria.IDBancoRec
                    _ChequeTesoreriSelected.NombreConsecutivo = TesoreriSelected.NombreConsecutivo

                    MyBase.CambioItem("ChequeTesoreriSelected")
                    MyBase.CambioItem("ListaChequeTesoreria")

                Case "NC", "ND"

                    If ListaDetalleTesoreria.Count = 0 Then
                        Dim NewDetalleTesoreri As New DetalleTesoreri

                        NewDetalleTesoreri.Usuario = Program.Usuario
                        NewDetalleTesoreri.Actualizacion = Now.Date
                        Dim result As Integer = 1
                        For Each value In ListaDetalleTesoreriaAnt
                            result = Math.Max(result, value.Secuencia) + 1
                        Next
                        NewDetalleTesoreri.Secuencia = result

                        'JFGB20160511
                        NewDetalleTesoreri.HabilitarSeleccionBanco = True
                        NewDetalleTesoreri.HabilitarSeleccionCliente = True
                        NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True

                        AdicionarRegistroEnDetalle(ListaDetalleTesoreria, NewDetalleTesoreri)
                        ListaDetalleTesoreriaAnt.Add(New secuenciadetalletesoreira With {.Secuencia = result})
                        DetalleTesoreriSelected = NewDetalleTesoreri
                    Else
                        DetalleTesoreriSelected = ListaDetalleTesoreria.First
                    End If

                    _DetalleTesoreriSelected.Nombre = _OrdenPendienteTesoreria.NombreCliente
                    _DetalleTesoreriSelected.IDComitente = _OrdenPendienteTesoreria.CodCliente
                    _DetalleTesoreriSelected.Detalle = _OrdenPendienteTesoreria.Detalle
                    _DetalleTesoreriSelected.IDCuentaContable = _OrdenPendienteTesoreria.CtaContable

                    If _OrdenPendienteTesoreria.Tipo = "ND" Then
                        _DetalleTesoreriSelected.Debito = _OrdenPendienteTesoreria.ValorSaldo
                    Else
                        _DetalleTesoreriSelected.Credito = _OrdenPendienteTesoreria.ValorSaldo
                    End If

                    If _OrdenPendienteTesoreria.IDBancoGirador <> 0 Then
                        _DetalleTesoreriSelected.IDBanco = _OrdenPendienteTesoreria.IDBancoGirador
                    End If
                    _DetalleTesoreriSelected.NombreConsecutivo = TesoreriSelected.NombreConsecutivo

                    _DetalleTesoreriSelected.ConsecutivoConsignacionOPT = _OrdenPendienteTesoreria.ConsecutivoConsignacion

                    MyBase.CambioItem("DetalleTesoreriSelected")
                    MyBase.CambioItem("ListaDetalleTesoreria")

            End Select

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar un nuevo detalle",
                     Me.ToString(), "OrdenesPendientesTesoreria_Adicionar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            _mlogBuscarCliente = True
        End Try
    End Sub

#End Region

#Region "Validar Saldo Cliente"

    ''' <summary>
    ''' Validar el Saldo del cliente seleccionado en el detalle
    ''' </summary>
    ''' <remarks>SLB20130326</remarks>
    Private Sub ValidarSaldoCliente()
        Try
            IsBusy = True
            If Editando Then

                Dim strXMLClientes As String
                Dim Comitente As String = ""

                If Not IsNothing(_ListaDetalleTesoreria) AndAlso _ListaDetalleTesoreria.Count > 0 Then

                    strXMLClientes = "<root>  "

                    For Each objDet In _ListaDetalleTesoreria
                        If Not String.IsNullOrEmpty(objDet.IDComitente) Then
                            If Comitente <> objDet.IDComitente Then

                                Dim strXMLDetalle = <Clientes lngIDComitente=<%= objDet.IDComitente %>>
                                                    </Clientes>

                                strXMLClientes = strXMLClientes & strXMLDetalle.ToString
                                Comitente = objDet.IDComitente
                            End If
                        End If
                    Next

                    strXMLClientes = strXMLClientes & " </root>"

                End If

                Dim strXMLDetTesoreriaSeguro As String = System.Web.HttpUtility.HtmlEncode(strXMLClientes)

                dcProxy.SaldosClientesMasivos.Clear()
                dcProxy.Load(dcProxy.ValidarSaldoClientesTesoreria_Masivo_GenericoQuery(_TesoreriSelected.IDComisionista, _TesoreriSelected.IDSucComisionista,
                                                                                        strXMLDetTesoreriaSeguro,
                                                                                        _TesoreriSelected.NombreConsecutivo,
                                                                                        _TesoreriSelected.Documento,
                                                                                        Program.Usuario, Program.HashConexion), AddressOf ValidarSaldoClientesTesoreriaCompleted, "")
            Else

                IsBusy = False
                Select Case moduloTesoreria
                    Case ClasesTesoreria.CE.ToString
                        ValidacionesServidorCE(True)
                    Case ClasesTesoreria.N.ToString
                        ValidacionesServidorNC(True)
                End Select

                'dcProxy.ValidarSaldoClientesTesoreria_Generico(_TesoreriSelected.IDComisionista,
                '                                  _TesoreriSelected.IDSucComisionista,
                '                                  _DetalleTesoreriSelected.IDComitente,
                '                                  _TesoreriSelected.Documento,Program.Usuario, Program.HashConexion, AddressOf ValidarSaldoClientesTesoreriaCompleted, "")
            End If

            'If Program.VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then 'SLB20130214 Si es City
            'dcProxy.ValidarSaldoClientesTesoreria(TesoreriSelected.IDComisionista,
            '                                      TesoreriSelected.IDSucComisionista,
            '                                      DetalleTesoreriSelected.IDComitente,
            '                                      TesoreriSelected.Documento,Program.Usuario, Program.HashConexion, AddressOf ValidarSaldoClientesTesoreriaCompleted, "")


            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar el saldo del cliente",
                                 Me.ToString(), "ValidarSaldoCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ValidarSaldoClientesTesoreriaCompleted(ByVal lo As LoadOperation(Of SaldosClientesMasivo))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se realizó la Validación del saldo del cliente.", Me.ToString(), "ValidarSaldoClientesTesoreriaCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
            Else
                ListaSaldosClientes = lo.Entities.ToList
                Dim objSaldoCliente As New SaldosClientesMasivo
                If lo.UserState.ToString = String.Empty Then
                    For Each saldo In ListaSaldosClientes
                        objSaldoCliente = saldo

                        For Each li In _ListaDetalleTesoreria
                            If Trim(saldo.lngIDComitente) = Trim(li.IDComitente) Then
                                li.SaldoCliente = saldo.curSaldoCorte
                            End If
                        Next
                    Next
                End If

                Select Case moduloTesoreria
                    Case ClasesTesoreria.CE.ToString
                        ValidacionesServidorCE(True)
                    Case ClasesTesoreria.N.ToString
                        ValidacionesServidorNC(True)
                End Select

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se realizó la Validación del saldo del cliente.",
                                                 Me.ToString(), "ValidarSaldoClientesTesoreriaCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Tablas hijas"
    'JFSB 20170801 Se crea la propiedad para utilizar el control de busqueda de bancos sin necesidad de hacerlo por el buscador
    Private _CuentaBancaria As OYDUtilidades.BuscadorGenerico
    Public Property CuentaBancaria() As OYDUtilidades.BuscadorGenerico
        Get
            Return _CuentaBancaria
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorGenerico)
            _CuentaBancaria = value
            MyBase.CambioItem("CuentaBancaria")
        End Set
    End Property

    '******************************************************** DetalleTesoreria 
    Private _ListaDetalleTesoreria As List(Of DetalleTesoreri)
    Public Property ListaDetalleTesoreria() As List(Of DetalleTesoreri)
        Get
            Return _ListaDetalleTesoreria
        End Get
        Set(ByVal value As List(Of DetalleTesoreri))
            _ListaDetalleTesoreria = value
            If Not IsNothing(value) Then
                If moduloTesoreria = "RC" Then
                    SumarTotales()
                ElseIf moduloTesoreria = "CE" Then
                    SumarComprobantes()
                End If
            End If
            MyBase.CambioItem("ListaDetalleTesoreria")
            MyBase.CambioItem("DetalleTesoreriaPaged")
        End Set
    End Property
    Private _ListaDetalleduplica As New List(Of DetalleTesoreri)
    Public Property ListaDetalleduplica() As List(Of DetalleTesoreri)
        Get
            Return _ListaDetalleduplica
        End Get
        Set(ByVal value As List(Of DetalleTesoreri))
            _ListaDetalleduplica = value
            MyBase.CambioItem("ListaDetalleduplica")
        End Set
    End Property
    Private _ListaSaldosClientes As List(Of SaldosClientesMasivo)
    Public Property ListaSaldosClientes() As List(Of SaldosClientesMasivo)
        Get
            Return _ListaSaldosClientes
        End Get
        Set(ByVal value As List(Of SaldosClientesMasivo))
            _ListaSaldosClientes = value
            MyBase.CambioItem("listaSaldosClientes")
        End Set
    End Property


    Private _ListaDetalleTesoreriaAnt As New List(Of secuenciadetalletesoreira)
    Public Property ListaDetalleTesoreriaAnt() As List(Of secuenciadetalletesoreira)
        Get
            Return _ListaDetalleTesoreriaAnt
        End Get
        Set(ByVal value As List(Of secuenciadetalletesoreira))
            _ListaDetalleTesoreriaAnt = value
            MyBase.CambioItem("ListaDetalleTesoreriaAnt")
        End Set
    End Property

    Public ReadOnly Property DetalleTesoreriaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleTesoreria) Then
                Dim view = New PagedCollectionView(_ListaDetalleTesoreria)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _DetalleTesoreriSelected As DetalleTesoreri
    Public Property DetalleTesoreriSelected() As DetalleTesoreri
        Get
            Return _DetalleTesoreriSelected
        End Get
        Set(ByVal value As DetalleTesoreri)
            If Not value Is Nothing Then
                _DetalleTesoreriSelected = value

                If moduloTesoreria = "RC" Then
                    SumarTotales()
                ElseIf moduloTesoreria = "CE" Then
                    SumarComprobantes()
                End If
                'If moduloTesoreria = "CE" Or moduloTesoreria = "N" Then
                '    If (Not IsNothing(TesoreriSelected.IDComisionista) And Not IsNothing(TesoreriSelected.IDSucComisionista)) Then
                '        ValidarSaldoCliente()
                '    End If
                'End If

                MyBase.CambioItem("DetalleTesoreriSelected")
            End If
        End Set
    End Property



    ''' Modificado por   : Yessid Andrés Paniagua Pabón
    ''' Descripción      : Se verifica que ListaDetalleTesoreria no venga en NULL, si viene en NULL se le asigna el contenido del proxy (1).
    '''                  : Se le coloca la hora al detalle de tesoreria ya que estaba enviado NULL y causaba una exepcion (2)
    ''' Fecha            : Abril 04/2016 
    ''' Pruebas CB       : Yessid Andrés Paniagua Pabón - Abril 04/2016 - Resultado Ok   
    ''' ID del Cambio    : YAPP20160404
    Public Overrides Sub NuevoRegistroDetalle()
        Try

            If Editando = True Then
                Select Case NombreColeccionDetalle
                    Case "cmDetalleTesoreri"

                        If IsNothing(TesoreriSelected.NombreConsecutivo) Or TesoreriSelected.NombreConsecutivo = String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el Tipo en el encabezado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If

                        NuevoDetalle()

                    Case "cmDetalleCheque"
                        Dim newChequeTesoreria As New Cheque
                        newChequeTesoreria.IDDocumento = TesoreriSelected.IDDocumento
                        newChequeTesoreria.NombreConsecutivo = TesoreriSelected.NombreConsecutivo
                        'newChequeTesoreria.Secuencia = ListaChequeTesoreria.Count + 1
                        Dim result As Integer = 1
                        For Each value In ListaChequeTesoreriaAnt
                            result = Math.Max(result, value.Secuencia) + 1
                        Next
                        newChequeTesoreria.Secuencia = result
                        newChequeTesoreria.BancoGirador = ""
                        newChequeTesoreria.NumCheque = 0
                        newChequeTesoreria.Valor = 0
                        newChequeTesoreria.Usuario = Program.Usuario
                        newChequeTesoreria.BancoConsignacion = Nothing '0
                        newChequeTesoreria.Consignacion = Nothing 'Now.Date
                        newChequeTesoreria.Actualizacion = Now.Date
                        newChequeTesoreria.FormaPagoRC = "C"
                        newChequeTesoreria.ChequeHizoCanje = True 'SLB20130802 Manejo de cheques de canje

                        AdicionarRegistroEnDetalleCheque(ListaChequeTesoreria, newChequeTesoreria)

                        ListaChequeTesoreriaAnt.Add(New secuenciachequestesoreria With {.Secuencia = result})
                        ChequeTesoreriSelected = newChequeTesoreria

                        MyBase.CambioItem("ChequeTesoreriSelected")
                        MyBase.CambioItem("ListaChequeTesoreria")


                End Select
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El detalle del documento no se puede editar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar un nuevo detalle",
                                 Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub NuevoDetalle()
        Dim NewDetalleTesoreri As New DetalleTesoreri

        If IsNothing(TesoreriSelected.NombreConsecutivo) Or TesoreriSelected.NombreConsecutivo = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el Tipo en el encabezado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If

        If Not String.IsNullOrEmpty(TesoreriSelected.IdComitente) Then
            NewDetalleTesoreri.IDComitente = TesoreriSelected.IdComitente
            If moduloTesoreria <> ClasesTesoreria.N.ToString Then
                NewDetalleTesoreri.Nombre = ClienteAnterior.Nombre
            Else
                NewDetalleTesoreri.Nombre = TesoreriSelected.Nombre
            End If

            'Santiago Vergara - Octubre 08/2014 -  Se hace cambio para cuando sea un nit se haga el substring para quitar el dígito de verificación
            If TesoreriSelected.TipoIdentificacion = "N" Then

                'JFSB 20160812 - Se ajusta el llenado del nit del detalle de comprobantes
                NewDetalleTesoreri.NIT = ClienteAnterior.NroDocumento.Substring(0, ClienteAnterior.NroDocumento.Length - 1)

            Else

                ' JFSB 20160812 - Se ajusta el llenado del número de documento en el detalle de comprobantes
                NewDetalleTesoreri.NIT = ClienteAnterior.NroDocumento
            End If
        End If

        'DEMC20160114 Apoyo con JEPM. Asignar nuevo IDDetalleTesoreria negativo
        If Not IsNothing(ListaDetalleTesoreria) AndAlso ListaDetalleTesoreria.Count >= 1 Then
            NewDetalleTesoreri.IDDetalleTesoreria = (From c In _ListaDetalleTesoreria Select c.IDDetalleTesoreria).Min - 1
        Else
            NewDetalleTesoreri.IDDetalleTesoreria = -1
        End If
        NewDetalleTesoreri.IDDocumento = TesoreriSelected.IDDocumento
        NewDetalleTesoreri.Tipo = TesoreriSelected.Tipo
        NewDetalleTesoreri.NombreConsecutivo = TesoreriSelected.NombreConsecutivo
        NewDetalleTesoreri.Usuario = Program.Usuario
        NewDetalleTesoreri.TipoVinculacion = TesoreriSelected.TipoVinculacion


        'NewDetalleTesoreri.Secuencia = ListaDetalleTesoreria.First.Secuencia.Ma
        'SLB20130222 
        Dim result As Integer = 1
        For Each value In ListaDetalleTesoreriaAnt
            result = Math.Max(result, value.Secuencia) + 1
        Next
        NewDetalleTesoreri.Secuencia = result
        'NewDetalleTesoreri.Secuencia = ListaDetalleTesoreria.Count + 1
        NewDetalleTesoreri.intClave_PorAprobar = -1


        'YAPP20160404 (1).
        If IsNothing(ListaDetalleTesoreria) Then
            ListaDetalleTesoreria = dcProxy.DetalleTesoreris.ToList     'JFSB 20160926
        End If
        'YAPP20160404

        NewDetalleTesoreri.Actualizacion = Now.Date  'YAPP20160404  (2)

        'JFGB20160511
        If moduloTesoreria = ClasesTesoreria.RC.ToString Or moduloTesoreria = ClasesTesoreria.CE.ToString Then
            If plogEsFondosOYD Then
                If _TesoreriSelected.IDCompania = intIDCompaniaFirma Then
                    NewDetalleTesoreri.HabilitarSeleccionBanco = True
                    NewDetalleTesoreri.HabilitarSeleccionCliente = True
                    NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True
                Else
                    NewDetalleTesoreri.HabilitarSeleccionBanco = False
                    NewDetalleTesoreri.HabilitarSeleccionCliente = False
                    NewDetalleTesoreri.HabilitarSeleccionCuentaContable = False
                End If
            Else
                NewDetalleTesoreri.HabilitarSeleccionBanco = True
                NewDetalleTesoreri.HabilitarSeleccionCliente = True
                NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True
            End If
        Else
            NewDetalleTesoreri.HabilitarSeleccionBanco = True
            NewDetalleTesoreri.HabilitarSeleccionCliente = True
            NewDetalleTesoreri.HabilitarSeleccionCuentaContable = True
        End If

        AdicionarRegistroEnDetalle(ListaDetalleTesoreria, NewDetalleTesoreri)



        'ListaDetalleTesoreriaAnt.Add(New secuenciadetalletesoreira With {.Secuencia = result})
        DetalleTesoreriSelected = NewDetalleTesoreri
        'cmbNombreConsecutivoHabilitado = False
        If Not String.IsNullOrEmpty(DetalleTesoreriSelected.IDComitente) Then
            AdicionarCuentaContable_Detalle()
        End If

        MyBase.CambioItem("DetalleTesoreriSelected")
        MyBase.CambioItem("ListaDetalleTesoreria")
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            If Editando = True Then
                Select Case NombreColeccionDetalle
                    Case "cmDetalleTesoreri"
                        If Not IsNothing(_ListaDetalleTesoreria) Then
                            If Not IsNothing(_DetalleTesoreriSelected) Then

                                Dim objListaRetorno As New List(Of DetalleTesoreri)
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DetalleTesoreriSelected, ListaDetalleTesoreria)

                                For Each li In ListaDetalleTesoreria
                                    objListaRetorno.Add(li)
                                Next

                                If _DetalleTesoreriSelected.IDDetalleTesoreria > 0 Then
                                    If objListaRetorno.Where(Function(i) i.IDDetalleTesoreria = _DetalleTesoreriSelected.IDDetalleTesoreria).Count > 0 Then
                                        objListaRetorno.Remove(ListaDetalleTesoreria.Where(Function(i) i.IDDetalleTesoreria = _DetalleTesoreriSelected.IDDetalleTesoreria).First())
                                    End If

                                    If Not IsNothing(dcProxy.DetalleTesoreris) Then
                                        If dcProxy.DetalleTesoreris.Where(Function(i) i.IDDetalleTesoreria = _DetalleTesoreriSelected.IDDetalleTesoreria).Count > 0 Then
                                            dcProxy.DetalleTesoreris.Remove(dcProxy.DetalleTesoreris.Where(Function(i) i.IDDetalleTesoreria = _DetalleTesoreriSelected.IDDetalleTesoreria).First)
                                        End If
                                    End If
                                Else
                                    If objListaRetorno.Where(Function(i) i.IDDetalleTesoreria = _DetalleTesoreriSelected.IDDetalleTesoreria And i.Secuencia = _DetalleTesoreriSelected.Secuencia).Count > 0 Then
                                        objListaRetorno.Remove(ListaDetalleTesoreria.Where(Function(i) i.IDDetalleTesoreria = _DetalleTesoreriSelected.IDDetalleTesoreria And i.Secuencia = _DetalleTesoreriSelected.Secuencia).First())
                                    End If
                                End If

                                ListaDetalleTesoreria = objListaRetorno

                                If ListaDetalleTesoreria.Count > 0 Then
                                    Program.PosicionarItemLista(DetalleTesoreriSelected, ListaDetalleTesoreria, intRegistroPosicionar)

                                    If plogEsFondosOYD Then
                                        If intIDCompaniaFirma <> _TesoreriSelected.IDCompania Then
                                            cmbNombreConsecutivoHabilitado = False
                                        End If
                                    End If
                                Else
                                    DetalleTesoreriSelected = Nothing
                                    If plogEsFondosOYD Then
                                        If intIDCompaniaFirma <> _TesoreriSelected.IDCompania Then
                                            cmbNombreConsecutivoHabilitado = True
                                        End If
                                    End If
                                End If

                                If moduloTesoreria = "RC" Then
                                    SumarTotales()
                                End If

                                MyBase.CambioItem("DetalleTesoreriSelected")
                                MyBase.CambioItem("ListaDetalleTesoreria")
                            End If
                        End If
                    Case "cmDetalleCheque"
                        If Not IsNothing(_ListaChequeTesoreria) Then
                            If Not IsNothing(_ChequeTesoreriSelected) Then

                                Dim objListaRetorno As New List(Of Cheque)
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ChequeTesoreriSelected, ListaChequeTesoreria)

                                For Each li In ListaChequeTesoreria
                                    objListaRetorno.Add(li)
                                Next
                                If _ChequeTesoreriSelected.IDCheques > 0 Then
                                    If objListaRetorno.Where(Function(i) i.IDCheques = _ChequeTesoreriSelected.IDCheques).Count > 0 Then
                                        objListaRetorno.Remove(ListaChequeTesoreria.Where(Function(i) i.IDCheques = _ChequeTesoreriSelected.IDCheques).First())
                                    End If

                                    If Not IsNothing(dcProxy.Cheques) Then
                                        If dcProxy.Cheques.Where(Function(i) i.IDCheques = _ChequeTesoreriSelected.IDCheques).Count > 0 Then
                                            dcProxy.Cheques.Remove(dcProxy.Cheques.Where(Function(i) i.IDCheques = _ChequeTesoreriSelected.IDCheques).First)
                                        End If
                                    End If
                                Else
                                    If objListaRetorno.Where(Function(i) i.IDCheques = _ChequeTesoreriSelected.IDCheques And i.Secuencia = _ChequeTesoreriSelected.Secuencia).Count > 0 Then
                                        objListaRetorno.Remove(ListaChequeTesoreria.Where(Function(i) i.IDCheques = _ChequeTesoreriSelected.IDCheques And i.Secuencia = _ChequeTesoreriSelected.Secuencia).First())
                                    End If
                                End If

                                ListaChequeTesoreria = objListaRetorno

                                If ListaChequeTesoreria.Count > 0 Then
                                    Program.PosicionarItemLista(ChequeTesoreriSelected, ListaChequeTesoreria, intRegistroPosicionar)
                                Else
                                    ChequeTesoreriSelected = Nothing
                                End If

                                If moduloTesoreria = "RC" Then
                                    SumarTotales()
                                End If

                                MyBase.CambioItem("ChequeTesoreriSelected")
                                MyBase.CambioItem("ListaChequeTesoreria")
                            End If
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El detalle del documento no se puede eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un detalle",
                                 Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub AdicionarRegistroEnDetalle(ByRef pobjListaDetalle As List(Of OyDTesoreria.DetalleTesoreri), ByVal pobjRegistroAdicionar As OyDTesoreria.DetalleTesoreri)
        Try
            Dim objListaNueva As New List(Of OyDTesoreria.DetalleTesoreri)

            If Not IsNothing(pobjListaDetalle) Then
                For Each li In pobjListaDetalle
                    objListaNueva.Add(li)
                Next
            End If

            objListaNueva.Add(pobjRegistroAdicionar)

            pobjListaDetalle = objListaNueva

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al adicionar un detalle",
                                 Me.ToString(), "AdicionarRegistroEnDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub AdicionarRegistroEnDetalleCheque(ByRef pobjListaDetalle As List(Of OyDTesoreria.Cheque), ByVal pobjRegistroAdicionar As OyDTesoreria.Cheque)
        Try
            Dim objListaNueva As New List(Of OyDTesoreria.Cheque)

            If Not IsNothing(pobjListaDetalle) Then
                For Each li In pobjListaDetalle
                    objListaNueva.Add(li)
                Next
            End If

            objListaNueva.Add(pobjRegistroAdicionar)

            pobjListaDetalle = objListaNueva

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al adicionar un detalle",
                                 Me.ToString(), "AdicionarRegistroEnDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    '******************************************************** ChequesTesoreria 
    Private _ListaChequeTesoreria As List(Of Cheque)
    Public Property ListaChequeTesoreria() As List(Of Cheque)
        Get
            Return _ListaChequeTesoreria
        End Get
        Set(ByVal value As List(Of Cheque))
            _ListaChequeTesoreria = value
            If Not value Is Nothing Then
                If moduloTesoreria = "RC" Then
                    SumarTotales()
                End If
            End If
            MyBase.CambioItem("ListaChequeTesoreria")
            MyBase.CambioItem("ChequesTesoreriaPaged")
        End Set
    End Property
    Private _ListaChequeduplica As New List(Of Cheque)
    Public Property ListaChequeduplica() As List(Of Cheque)
        Get
            Return _ListaChequeduplica
        End Get
        Set(ByVal value As List(Of Cheque))
            _ListaChequeduplica = value
            MyBase.CambioItem("ListaChequeduplica")
        End Set
    End Property
    Private _ListaChequeTesoreriaAnt As New List(Of secuenciachequestesoreria)
    Public Property ListaChequeTesoreriaAnt() As List(Of secuenciachequestesoreria)
        Get
            Return _ListaChequeTesoreriaAnt
        End Get
        Set(ByVal value As List(Of secuenciachequestesoreria))
            _ListaChequeTesoreriaAnt = value
            MyBase.CambioItem("ListaChequeTesoreriaAnt")
        End Set
    End Property

    Public ReadOnly Property ChequesTesoreriaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaChequeTesoreria) Then
                Dim view = New PagedCollectionView(_ListaChequeTesoreria)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ChequeTesoreriSelected As Cheque
    Public Property ChequeTesoreriSelected() As Cheque
        Get
            Return _ChequeTesoreriSelected
        End Get
        Set(ByVal value As Cheque)
            _ChequeTesoreriSelected = value
            If Not value Is Nothing Then
                If moduloTesoreria = "RC" Then
                    SumarTotales()
                End If
            End If
            MyBase.CambioItem("ChequeTesoreriSelected")
        End Set
    End Property

    '******************************************************** ObservacionesTesoreria 
    Private _ListaObservacionesTesoreria As List(Of TesoreriaAdicionale) = New List(Of TesoreriaAdicionale) '= New EntitySet(Of TesoreriaAdicionale)
    Public Property ListaObservacionesTesoreria() As List(Of TesoreriaAdicionale)
        Get
            Return _ListaObservacionesTesoreria
        End Get
        Set(ByVal value As List(Of TesoreriaAdicionale))

            If Not (value Is Nothing) Then
                _ListaObservacionesTesoreria = value

                If value.Count <> 0 Then
                    ObservacionTesoreriSelected = value.Last
                End If

            End If

            MyBase.CambioItem("ListaObservacionesTesoreria")
        End Set
    End Property

    Private WithEvents _ObservacionTesoreriSelected As TesoreriaAdicionale
    Public Property ObservacionTesoreriSelected() As TesoreriaAdicionale
        Get
            Return _ObservacionTesoreriSelected
        End Get
        Set(ByVal value As TesoreriaAdicionale)
            _ObservacionTesoreriSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("ObservacionTesoreriSelected")
            End If
        End Set
    End Property
#End Region

#Region "Cuentas Clientes"

    Private Sub TerminoTraerCuentasClientes(ByVal lo As LoadOperation(Of CuentasClientes_Tesoreria))
        IsBusy = False
        If Not lo.HasError Then
            If lo.Entities.ToList.Count > 0 Then
                _TesoreriSelected.Nombre = lo.Entities.First.Titular
                _TesoreriSelected.TipoIdentificacion = lo.Entities.First.Retorno
                _TesoreriSelected.NroDocumento = lo.Entities.First.NumeroID
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Cuentas de los Clientes",
                                             Me.ToString(), "TerminoTraerCuentasClientes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Verificar Anulación Encargos"
    Private Sub ValidadorEncargos(Optional ByVal pstrUserState As String = "")
        Try

            'If Not IsNothing(_TesoreriSelected) And Not ListaTesoreria.Count = 0 Then
            dcProxy.consultarCompaniaTesoreriaValidarAnular(_TesoreriSelected.IDCompania, _TesoreriSelected.Documento, _TesoreriSelected.NombreConsecutivo, _TesoreriSelected.IDDocumento, Program.Usuario, Program.HashConexion, AddressOf consultarCompaniaValidaEncargosCompleted, pstrUserState)
            'End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar al validar los encargos.",
                                 Me.ToString(), "ValidadorEncargos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub consultarCompaniaValidaEncargosCompleted(ByVal obj As InvokeOperation(Of String))
        Try
            Dim strMsg As String = String.Empty
            If obj.HasError Then
                MyBase.RetornarValorEdicionNavegacion()
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validación para los encargos de la compañía", Me.ToString(), "consultarCompaniaValidaEncargosCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else

                If Not String.IsNullOrEmpty(obj.Value.ToString()) Then
                    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, obj.Value.ToString())
                    IsBusy = False
                    If Not String.IsNullOrEmpty(strMsg) Then
                        MyBase.RetornarValorEdicionNavegacion()
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    'JFSB 20171005 Se agrega momentaneamente este ajuste
                    If obj.UserState = "EDITAR" Then
                        objProxy.consultarFechaCierreCompania(_TesoreriSelected.IDCompania, Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompaniaCompleted, "EDITAR")
                    ElseIf obj.UserState = "GUARDAR" Then
                        objProxy.consultarFechaCierreCompania(_TesoreriSelected.IDCompania, Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompaniaCompleted, "GUARDAR")
                    ElseIf obj.UserState = "BORRAR" Then
                        objProxy.consultarFechaCierreCompania(_TesoreriSelected.IDCompania, Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompaniaCompleted, "BORRAR")
                    End If
                End If
            End If

            'If ValidarEncargos = True Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre de la compañia",
                                                 Me.ToString(), "consultarFechaCierreCompaniaCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class




'Clase base para forma de búsquedas
Public Class CamposBusquedaTesoreri
    Implements INotifyPropertyChanged
    Private _Tipo As String
    <StringLength(2, ErrorMessage:="La longitud máxima es de 2")>
    <Display(Name:="Tipo")>
    Public Property Tipo As String

        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _Tipo = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tipo"))
        End Set
    End Property
    Private _IDCompania As Nullable(Of Integer)
    <Display(Name:="Compañia")>
    Public Property IDCompania() As Nullable(Of Integer)
        Get
            Return _IDCompania
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDCompania = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDCompania"))
        End Set
    End Property
    Private _NombreCompania As String
    <Display(Name:="Compañia")>
    Public Property NombreCompania() As String
        Get
            Return _NombreCompania
        End Get
        Set(ByVal value As String)
            _NombreCompania = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCompania"))
        End Set
    End Property
    Private _NombreConsecutivo As String
    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")>
    <Display(Name:="Tipo")>
    Public Property NombreConsecutivo As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _NombreConsecutivo = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreConsecutivo"))
        End Set
    End Property
    Private _IDDocumento As Integer
    <Display(Name:="Número")>
    Public Property IDDocumento As Integer
        Get
            Return _IDDocumento
        End Get
        Set(ByVal value As Integer)
            'If Not IsNothing(value) Then
            _IDDocumento = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDDocumento"))
        End Set
    End Property

    <StringLength(1, ErrorMessage:="La longitud máxima es de 1")>
    <Display(Name:="TipoIdentificacion")>
    Public Property TipoIdentificacion As String

    <Display(Name:="NroDocumento")>
    Public Property NroDocumento As String

    <StringLength(80, ErrorMessage:="La longitud máxima es de 80")>
    <Display(Name:="Nombre")>
    Public Property Nombre As String


    Private _IDBanco As Integer
    <Display(Name:="Banco")>
    Public Property IDBanco As Integer
        Get
            Return _IDBanco
        End Get
        Set(ByVal value As Integer)
            'If Not IsNothing(value) Then
            _IDBanco = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDBanco"))
        End Set
    End Property


    Private _NombreBanco As String
    <Display(Name:=" ")>
    Public Property NombreBanco As String
        Get
            Return _NombreBanco
        End Get
        Set(ByVal value As String)
            If Not IsNothing(value) Then
                _NombreBanco = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreBanco"))
        End Set
    End Property

    <Display(Name:="NumCheque")>
    Public Property NumCheque As Double

    <Display(Name:="Valor")>
    Public Property Valor As Decimal

    <StringLength(80, ErrorMessage:="La longitud máxima es de 80")>
    <Display(Name:="Detalle")>
    Public Property Detalle As String

    Private _visibilidadSubEstados As Visibility = Visibility.Collapsed
    Public Property visibilidadSubEstados As Visibility
        Get
            Return _visibilidadSubEstados
        End Get
        Set(ByVal value As Visibility)
            _visibilidadSubEstados = value
            'MyBase.CambioItem("visibilidadSubEstados")
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("visibilidadSubEstados"))
        End Set
    End Property

    Private _Documento As System.Nullable(Of DateTime)
    <Display(Name:="Fecha")>
    Public Property Documento As System.Nullable(Of DateTime)
        Get
            Return _Documento
        End Get
        Set(ByVal value As System.Nullable(Of DateTime))
            'If Not IsNothing(value) Then
            _Documento = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Documento"))
        End Set
    End Property

    '<Display(Name:="Fecha", Description:="Documento")> _
    'Public Property Documento As DateTime

    <StringLength(1, ErrorMessage:="La longitud máxima es de 1")>
    <Display(Name:="Estado")>
    Public Property Estado As String

    <Display(Name:="Estado")>
    Public Property FecEstado As DateTime

    <Display(Name:="Impresiones")>
    Public Property Impresiones As Integer

    <StringLength(1, ErrorMessage:="La longitud máxima es de 1")>
    <Display(Name:="FormaPagoCE")>
    Public Property FormaPagoCE As String

    <Display(Name:="NroLote")>
    Public Property NroLote As Integer

    <Display(Name:="Contabilidad")>
    Public Property Contabilidad As Boolean

    <Display(Name:="Actualizacion", Description:="Actualizacion")>
    Public Property Actualizacion As DateTime

    <StringLength(60, ErrorMessage:="La longitud máxima es de 60")>
    <Display(Name:="Usuario")>
    Public Property Usuario As String

    <StringLength(2, ErrorMessage:="La longitud máxima es de 2")>
    <Display(Name:="ParametroContable")>
    Public Property ParametroContable As String

    <Display(Name:="ImpresionFisica")>
    Public Property ImpresionFisica As Boolean

    <Display(Name:="MultiCliente")>
    Public Property MultiCliente As Boolean

    <StringLength(25, ErrorMessage:="La longitud máxima es de 25")>
    <Display(Name:="CuentaCliente")>
    Public Property CuentaCliente As String

    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")>
    <Display(Name:="CodComitente")>
    Public Property CodComitente As String

    <StringLength(2, ErrorMessage:="La longitud máxima es de 2")>
    <Display(Name:="ArchivoTransferencia")>
    Public Property ArchivoTransferencia As String

    <Display(Name:="IdNumInst")>
    Public Property IdNumInst As Integer

    <StringLength(10, ErrorMessage:="La longitud máxima es de 10")>
    <Display(Name:="DVP")>
    Public Property DVP As String

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Instruccion")>
    Public Property Instruccion As String

    <Display(Name:="IdNroOrden")>
    Public Property IdNroOrden As Decimal

    <StringLength(450, ErrorMessage:="La longitud máxima es de 450")>
    <Display(Name:="DetalleInstruccion")>
    Public Property DetalleInstruccion As String

    <StringLength(1, ErrorMessage:="La longitud máxima es de 1")>
    <Display(Name:="EstadoNovedadContabilidad")>
    Public Property EstadoNovedadContabilidad As String

    <StringLength(10, ErrorMessage:="La longitud máxima es de 10")>
    <Display(Name:="eroComprobante_Contabilidad")>
    Public Property eroComprobante_Contabilidad As String

    <StringLength(10, ErrorMessage:="La longitud máxima es de 10")>
    <Display(Name:="hadecontabilizacion_Contabilidad")>
    Public Property hadecontabilizacion_Contabilidad As String

    <Display(Name:="haProceso_Contabilidad")>
    Public Property haProceso_Contabilidad As DateTime

    <StringLength(1, ErrorMessage:="La longitud máxima es de 1")>
    <Display(Name:="EstadoNovedadContabilidad_Anulada")>
    Public Property EstadoNovedadContabilidad_Anulada As String

    <StringLength(1, ErrorMessage:="La longitud máxima es de 1")>
    <Display(Name:="EstadoAutomatico")>
    Public Property EstadoAutomatico As String

    <StringLength(10, ErrorMessage:="La longitud máxima es de 10")>
    <Display(Name:="eroLote_Contabilidad")>
    Public Property eroLote_Contabilidad As String

    <StringLength(256, ErrorMessage:="La longitud máxima es de 256")>
    <Display(Name:="MontoEscrito")>
    Public Property MontoEscrito As String

    <StringLength(25, ErrorMessage:="La longitud máxima es de 25")>
    <Display(Name:="TipoIntermedia")>
    Public Property TipoIntermedia As String

    <StringLength(200, ErrorMessage:="La longitud máxima es de 200")>
    <Display(Name:="Concepto")>
    Public Property Concepto As String

    <Display(Name:="RecaudoDirecto")>
    Public Property RecaudoDirecto As Boolean

    <Display(Name:="ContabilidadEncuenta")>
    Public Property ContabilidadEncuenta As DateTime

    <Display(Name:="Sobregiro")>
    Public Property Sobregiro As Boolean

    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")>
    <Display(Name:="IdentificacionAutorizadoCheque")>
    Public Property IdentificacionAutorizadoCheque As String

    <StringLength(80, ErrorMessage:="La longitud máxima es de 80")>
    <Display(Name:="NombreAutorizadoCheque")>
    Public Property NombreAutorizadoCheque As String

    <Display(Name:="IDTesoreria")>
    Public Property IDTesoreria As Integer

    <Display(Name:="ContabilidadENC")>
    Public Property ContabilidadENC As DateTime

    <Display(Name:="NroLoteAntENC")>
    Public Property NroLoteAntENC As Integer

    <Display(Name:="ContabilidadAntENC")>
    Public Property ContabilidadAntENC As DateTime

    <Display(Name:="IdSucursalBancaria")>
    Public Property IdSucursalBancaria As Integer

    <Display(Name:="Creacion")>
    Public Property Creacion As DateTime

    '<Display(Name:="")> _
    'Public Property Filtro As Byte

    Private _Aprobados As Integer
    <Display(Name:=" ")>
    Public Property Aprobados As Integer
        Get
            Return _Aprobados
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(value) Then
                _Aprobados = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Aprobados"))
        End Set
    End Property

    '<Display(Name:=" ")> _
    'Public Property Aprobados As Integer

    Private _PorAprobar As Integer
    <Display(Name:=" ")>
    Public Property PorAprobar As Integer
        Get
            Return _PorAprobar
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(value) Then
                _PorAprobar = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PorAprobar"))
        End Set
    End Property

    '<Display(Name:=" ")> _
    'Public Property PorAprobar As Integer

    Private _NoAprobados As Integer
    <Display(Name:=" ")>
    Public Property NoAprobados As Integer
        Get
            Return _NoAprobados
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(value) Then
                _NoAprobados = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NoAprobados"))
        End Set
    End Property

    '<Display(Name:=" ")> _
    'Public Property NoAprobados As Integer

    '<Display(Name:="Fecha")> _
    'Public Property DisplayDate As DateTime

    Private _DisplayDate As Date
    <Display(Name:="Fecha")>
    Public Property DisplayDate As Date
        Get
            Return _DisplayDate
        End Get
        Set(ByVal value As Date)
            _DisplayDate = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DisplayDate"))
        End Set
    End Property


    Private _Ingreso As Integer
    <Display(Name:=" ")>
    Public Property Ingreso As Integer
        Get
            Return _Ingreso
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(value) Then
                _Ingreso = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ingreso"))
        End Set
    End Property

    Private _Modificacion As Integer
    <Display(Name:=" ")>
    Public Property Modificacion As Integer
        Get
            Return _Modificacion
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(value) Then
                _Modificacion = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modificacion"))
        End Set
    End Property

    Private _Retiro As Integer
    <Display(Name:=" ")>
    Public Property Retiro As Integer
        Get
            Return _Retiro
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(value) Then
                _Retiro = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Retiro"))
        End Set
    End Property

    Private _Todos As Integer
    <Display(Name:=" ")>
    Public Property Todos As Integer
        Get
            Return _Todos
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(value) Then
                _Todos = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Todos"))
        End Set
    End Property


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
Public Class secuenciadetalletesoreira
    Public Property Secuencia As Integer
End Class
Public Class secuenciachequestesoreria
    Public Property Secuencia As Integer
End Class
Public Class OrdenesTesoreria
    Implements INotifyPropertyChanged

    <Display(Name:="Consecutivo Consignación")>
    Public Property ConsecutivoConsignacion As String

    <Display(Name:="Nombre Cliente")>
    Public Property NombreCliente As String

    <Display(Name:="Valor Saldo")>
    Public Property ValorSaldo As Decimal

    <Display(Name:="Código Cliente")>
    Public Property CodCliente As String

    <Display(Name:="Detalle")>
    Public Property Detalle As String

    <Display(Name:="Forma de Pago")>
    Public Property FormaPago As String

    <Display(Name:="Cuenta Contable")>
    Public Property CtaContable As String

    <Display(Name:="Fecha Consignación")>
    Public Property FechaConsignacion As DateTime

    <Display(Name:="Procesado")>
    Public Property Procesado As Int32

    <Display(Name:="Logín")>
    Public Property Login As String

    <Display(Name:="Fecha de Actualización")>
    Public Property FechaActualizacion As DateTime = Now.Date

    <Display(Name:="Banco Girador")>
    Public Property IDBancoGirador As Int32

    <Display(Name:="Número Cheque")>
    Public Property NroCheque As Decimal

    <Display(Name:="Bano Rec.")>
    Public Property IDBancoRec As Int32

    <Display(Name:="Tipo")>
    Public Property Tipo As String

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

''' <summary>
''' Clase creada para la validaciónes en el detalle de cheques de RC dependiendo de la forma de pago
''' </summary>
''' <remarks>SLB20130304</remarks>
Public Class CamposObligatorios
    Public Property lngID As String = String.Empty
    Public Property strFormadePago As String = String.Empty
    Public Property strBancoGirador As Boolean = False
    Public Property lngNumCheque As Boolean = False
    Public Property lngBancoConsignacion As Boolean = False
    Public Property dtmConsignacion As Boolean = False
    Public Property strComentario As Boolean = False
    Public Property lngidproducto As Boolean = False
End Class

Public Class TesoreriaEmergenteEncabezado
    Public Property FechaDocumento As Nullable(Of DateTime)
    Public Property IDCompania As Nullable(Of Integer)
    Public Property NombreCompania As String
    Public Property NroDocumentoBeneficiario As String
    Public Property NombreBeneficiario As String
    Public Property TipoIdentificacionBeneficiario As String
    Public Property FormaPago As String
    Public Property TipoCheque As String
    Public Property CuentaCliente As String
    Public Property ListaDetalle As List(Of TesoreriaEmergenteDetalle)
    Public Property ListaCheque As List(Of TesoreriaEmergenteCheque)
End Class

Public Class TesoreriaEmergenteDetalle
    Public Property IDComitente As String
    Public Property NombreComitente As String
    Public Property IDBanco As Nullable(Of Integer)
    Public Property NombreBanco As String
    Public Property IDConcepto As Nullable(Of Integer)
    Public Property Detalle As String
    Public Property CuentaContable As String
    Public Property Nit As String
    Public Property CentroCostos As String
    Public Property ValorDebito As Double
    Public Property ValorCredito As Double
End Class

Public Class TesoreriaEmergenteCheque
    Public Property NroCheque As Nullable(Of Double)
    Public Property BancoGirador As Nullable(Of Integer)
    Public Property BancoConsignacion As Nullable(Of Integer)
    Public Property NombreBancoGirador As String
    Public Property FechaConsignacion As Nullable(Of DateTime)
    Public Property Valor As Double
    Public Property Observaciones As String
    Public Property FormaPago As String
    Public Property TipoProducto As Nullable(Of Integer)
End Class

Public Class ControlBusqueda
    Public Property CodigoDetalle As Integer
    Public Property TopicoBusqueda As String
End Class