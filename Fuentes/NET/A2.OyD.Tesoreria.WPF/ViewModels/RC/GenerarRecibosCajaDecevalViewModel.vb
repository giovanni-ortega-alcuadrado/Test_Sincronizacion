Imports Telerik.Windows.Controls
'Codigo Creado Por: Rafael Cordero
'Archivo: GenerarRecibosCajaDecevalViewModel.vb
'Generado el : 08/08/2011 
'Propiedad de Alcuadrado S.A. 2011

'Se cambia la lógica para que cuando se maneja Maker and Cheker todo el proceso de inserción se haga desde la base de datos
'Santiago Alexander Vergara Orrego
'Enero 31/2014

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
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.Runtime.InteropServices.Automation
Imports System.Text
Imports Microsoft.VisualBasic.CompilerServices
Imports A2ComunesImportaciones

Public Class GenerarRecibosCajaDecevalViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Const CSTR_NOMBREPROCESO_RECIBOSDECAJA = "RECIBOSDECAJA"
    Private Shared SEPARATOR_FORMAT_CVS As String = System.Globalization.CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator
    Dim Pagina As Integer = 1
    Const NroDeRegistro As Integer = 5000  'JFSB 20160915 Se ponen 10000 registros por recibo
    Dim NroFilas As Integer
    Dim NroRegistrosTotal As Integer
    Dim strResultado As String = String.Empty
    Public Const CSTR_NOMBREPROCESO_CSVREPORTE = "GeneracionCSVReportes"
    Private intNroDetallesPermitidos As Integer   'CORREC_CITI_SV_2014
    Private strFormaPagoPorDefecto As String = "E"
    Dim objServicios As A2VisorReportes.A2.Visor.Servicios.GeneralesClient
    Dim logLlevarValorNroDetalles As Boolean = True
    Dim logLanzarCodigoSeleccionarTodos As Boolean = True
    Dim CantidadRegistros As Integer
    Dim logConsultar As Boolean = True
    Private CantidadRegistrosOriginales As Integer
    Dim DesmarcarTodos As String = Nothing

#Region "Declaraciones"
    Private dcProxy As TesoreriaDomainContext   'CORREC_CITI_SV_2014
    Private dcProxy1 As ImportacionesDomainContext
    Private objProxyUtil As UtilidadesDomainContext

    'Constantes para indicar el tipo de filtro
    Private Const ALGUNOS_REGISTROS As Integer = 1
    Private Const TODOS_LOS_REGISTROS As Integer = 2
    Private Const REGISTRO_INICIAL As Integer = 3

    'Constante para indicar el Codigo de Comicol
    Private Const Comicol As Integer = 23

    Private Const LLAMADO_DESDE_ACEPTAR As String = "ACEPTAR"

    'NIT DECEVAL
    Private Const Nitdeceval As Decimal = 8001820912


    Private Const STRVALORESNUMERICOS As String = "Debe ingresar valores Numéricos"
    Private Const BANCO_NO_VALIDO As String = "Debe seleccionar un banco"
    Private Const SUCURSAL_NO_VALIDA As String = "Debe seleccionar una Sucursal"
    Private Const CONSECUTIVO_NO_VALIDA As String = "Debe seleccionar un Consecutivo de Tesorería"
    Private Const CUENTACONTABLE_NO_VALIDA As String = "Debe seleccionar una Cuenta Contable"
    Private Const FECHARECIBO_NO_VALIDA As String = "Debe seleccionar una Fecha para el Recibo"
    Private Const RECIBI_NO_VALIDO As String = "El Campo Recibí no puede estar Vacio"
    Private Const TIPOID_NO_VALIDO As String = "Debe seleccionar el Tipo de Documento"
    Private Const NROID_NO_VALIDO As String = "Debe digitar el Nro de Documento"
    Private Const BANCOGIRADOR_NO_VALIDO As String = "El banco girador no puede estar vacío"
    Private Const CHEQUE_NO_VALIDO As String = "El número de Cheque no puede estar Vacío"
    Private Const FECHACONSIGNACION_NO_VALIDA As String = "Debe seleccionar una Fecha para la consignación del cheque "
    Private Const NRODETALLES_NO_VALIDO As String = "Debe digitar un número de detalles por Recibo de Caja."
    Private Const NRODETALLES_POR_RECIBO As String = "El número de detalles por Recibo de Caja no puede superar los "
    Private Const USUARIO_NO_VALIDO As String = "Debe seleccionar el usuario al cual quedarán asociados los Recibos"
    Private Const NO_EXISTEN_DETALLES As String = "No existen detalles para hacer Recibos"
    Private Const NO_SELECCIONADOS As String = "No existen detalles Seleccionados para hacer Recibos"
    Private Const NO_CONTRAPARTIDA As String = "No existen datos de sucursales diferentes a la escogida." & vbCrLf & "No puede hacerse la contrapartida"
    Private Const DETALLE_CONTRAPARTIDA As String = "Sucursales y Agencias "
    'CORREC_CITI_SV_2014
    Private Const NRO_DETALLES_MAYOR As String = "El número de detalles no puede ser mayor al parametrizado ({0})."

    Private mdtmFechaFinVigencia As Date
    Private intSecuencia As Integer = 0
    Private intNroDetalles As Integer = 0
    Private TotalRecibo As Double = 0
    Private TotalDetallesGRID As Long = 0
    Private RegistrosInsertados As Long = 0
    Private ValorTotalRegistrosInsertados As Double = 0
    Private logSeleccionatodos As Boolean
    Public Event salvar()
    Public Event mostrarmensaje()
    Public Event mostrar()

    Public Enum EnumVersionAplicacionCliente
        G 'Genérico
        C 'City
    End Enum

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
#End Region

#End Region

#Region "Inicialización"

    Public Sub New()
        Try
            IsBusy = True
            IsBusyDetalles = True

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext
                dcProxy1 = New ImportacionesDomainContext
                objProxyUtil = New UtilidadesDomainContext   'CORREC_CITI_SV_2014
            Else
                objProxyUtil = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))   'CORREC_CITI_SV_2014

            End If
            'JFSB 20160915 Se alarga el tiempo de espera para el proxy del módulo a 300 minutos
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 300, 0)
            DirectCast(dcProxy1.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 300, 0)

            clsEstablecerComunicacionVisorReportes.ServicioGenerales(objServicios, Application.Current.Resources("A2VServicioParam").ToString())

            If Not Program.IsDesignMode() Then
                VisibleContraPartida = Visibility.Collapsed
                RegistroTesoreriaSeleccionado.NroDocumento = Nitdeceval
                RegistroTesoreriaSeleccionado.FormaPagoCE = String.Empty
                IsBusy = True
                IsBusyDetalles = True
                'CORREC_CITI_SV_2014
                RegistroTesoreriaSeleccionado.FormaPagoCE = strFormaPagoPorDefecto
                objProxyUtil.Verificaparametro("Top_Consulta_Detalle_Tesoreria", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "Top_Consulta_Detalle_Tesoreria")
                objProxyUtil.Verificaparametro("CANTIDAD_REGISTROS_DETALLE", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "CANTIDAD_REGISTROS_DETALLE")

                'JCM20160216
                ValoresSelected = RegistroTesoreriaSeleccionado
                logConsultar = False
                Sucursal = "0"
                logConsultar = True
                Tipo = "T"
                FechaConsignar = Now
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "GenerarRecibosCajaDecevalViewModel.New", Program.TituloSistema, Program.Maquina, ex)
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

                    DiccionarioCarga = dicListaCombos
                    IsBusyDetalles = True
                    Sucursal = "0"

                    If DiccionarioCarga.ContainsKey("LoginUsuario") Then
                        If DiccionarioCarga("LoginUsuario").Where(Function(i) i.Descripcion = Program.Usuario).Count > 0 Then
                            RegistroTesoreriaSeleccionado.Usuario = Program.Usuario
                        End If
                    End If

                    If Not IsNothing(dcProxy.ItemCombos) Then
                        objProxyUtil.ItemCombos.Clear()
                    End If
                    objProxyUtil.Load(objProxyUtil.cargarCombosCondicionalQuery("CONSECUTIVOS_DEMOCRATIZADOS", "NO", 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, Nothing)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos",
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos",
                 Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Public Sub CargarListaPagosDeceval(ByVal pobjEstado As Boolean)
        Try
            IsBusyDetalles = True

            If pobjEstado Then
                Pagina = 1
                NroRegistrosTotal = 0
                logLanzarCodigoSeleccionarTodos = False
                SeleccionarTodos = False
                logSeleccionatodos = False
                logLanzarCodigoSeleccionarTodos = True
                _ListaRegDetalle = New ObservableCollection(Of ListaDetalleTesoreria)
                _numTotalSeleccionados = 0
                _numTotalGeneral = 0

                CargarTotalesPagosDeceval()
            End If

            dcProxy.ListaDetalleTesorerias.Clear()
            dcProxy.Load(dcProxy.GetListaDetalleTesoreriasQuery(Pagina, NroDeRegistro, Democratizados, IDEspecie, Sucursal, Tipo, True, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListaPagosDeceval, pobjEstado)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargara la lista del detalle",
                                 Me.ToString(), "CargarListaPagosDeceval", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CargarTotalesPagosDeceval()
        Try
            Dim strNoSeleccionados As String = String.Empty
            dcProxy.PagosDECEVALTotales.Clear()
            If logSeleccionatodos Then

                For Each objLista In ListaDetallePaged
                    If objLista.Seleccionado = False Then
                        strNoSeleccionados += objLista.intIDPagosDeceval & ","
                    End If
                Next
                If Not String.IsNullOrEmpty(strNoSeleccionados) Then
                    strNoSeleccionados = strNoSeleccionados.Substring(0, strNoSeleccionados.Length - 1)
                End If

                dcProxy.Load(dcProxy.PagosDeceval_TotalesRegistrosQuery(Democratizados, IDEspecie, Sucursal, Tipo, "Todos", strNoSeleccionados, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTotales, String.Empty)

            Else

                For Each Lista In (From l In _ListaRegDetalle)
                    If Not Lista.VlrAPagar Is Nothing Then
                        If Lista.Seleccionado Then
                            _numTotalSeleccionados = _numTotalSeleccionados + Lista.VlrAPagar
                        End If
                    End If
                Next

                dcProxy.Load(dcProxy.PagosDeceval_TotalesRegistrosQuery(Democratizados, IDEspecie, Sucursal, Tipo, "Todos", strNoSeleccionados, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTotales, String.Empty)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargara la lista del detalle",
                                 Me.ToString(), "CargarTotalesPagosDeceval", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    Public Sub SeleccionarTodosChecked(ByVal pbolCheked As System.Nullable(Of Boolean), ByVal plogllamadoViejo As Boolean)
        Try
            If Not pbolCheked Is Nothing Then
                IsBusy = True
                If plogllamadoViejo = False Then
                    logSeleccionatodos = True

                    _numTotalSeleccionados = 0

                    dcProxy.PagosDECEVALTotales.Clear()
                    If pbolCheked Then
                        dcProxy.Load(dcProxy.PagosDeceval_TotalesRegistrosQuery(Democratizados, IDEspecie, Sucursal, Tipo, "Todos", String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTotales, String.Empty)
                    Else
                        dcProxy.Load(dcProxy.PagosDeceval_TotalesRegistrosQuery(Democratizados, IDEspecie, Sucursal, Tipo, String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTotales, String.Empty)
                    End If

                    For Each Lista In _ListaRegDetalle
                        Lista.Seleccionado = pbolCheked
                    Next
                Else
                    For Each Lista In _ListaRegDetalle
                        If Lista.intIDPagosDeceval = SelectedDetalle.intIDPagosDeceval Then
                            Lista.Seleccionado = SelectedDetalle.Seleccionado
                        End If
                    Next
                End If

                MyBase.CambioItem("TotalSeleccionados")
                MyBase.CambioItem("TotalGeneral")
                IsBusy = False
            Else
                If plogllamadoViejo = False Then
                    SeleccionarTodos = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar los registros",
                                 Me.ToString(), "SeleccionarTodosChecked", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub CalcularValorSeleccionado()
        Try

            If Not IsNothing(_ListaRegDetalle) Then
                If NroDetalles = CantidadRegistros Then
                    _numTotalSeleccionados = 0

                    For Each Lista In (From l In _ListaRegDetalle)
                        If Not Lista.VlrAPagar Is Nothing Then
                            If Lista.Seleccionado Then
                                _numTotalSeleccionados = _numTotalSeleccionados + Lista.VlrAPagar
                            End If
                        End If
                    Next

                Else
                    For Each li In _ListaRegDetalle
                        If SelectedDetalle.intIDPagosDeceval = li.intIDPagosDeceval Then
                            li.Seleccionado = SelectedDetalle.Seleccionado
                            If li.Seleccionado = False Then
                                _numTotalSeleccionados = _numTotalSeleccionados - SelectedDetalle.VlrAPagar
                                DesmarcarTodos = "SI"
                                SeleccionarTodos = False
                                logSeleccionatodos = False
                            Else
                                _numTotalSeleccionados = _numTotalSeleccionados + SelectedDetalle.VlrAPagar
                            End If
                        End If
                    Next
                    If _ListaRegDetalle.Where(Function(i) i.Seleccionado = False).Count = 0 Then
                        DesmarcarTodos = "NO"
                        SeleccionarTodos = True
                    End If
                    'Else
                    '    If SelectedDetalle.Seleccionado = False Then
                    '        _numTotalSeleccionados = _numTotalSeleccionados - SelectedDetalle.VlrAPagar
                    '        DesmarcarTodos = "SI"
                    '        SeleccionarTodos = False
                    '        logSeleccionatodos = False
                    '    Else
                    '        _numTotalSeleccionados = _numTotalSeleccionados + SelectedDetalle.VlrAPagar
                    '    End If
                    'End If
                End If
                DesmarcarTodos = Nothing
                MyBase.CambioItem("TotalGeneralOriginal")
                MyBase.CambioItem("TotalSeleccionadosOriginal")
                MyBase.CambioItem("TotalSeleccionados")
                MyBase.CambioItem("TotalGeneral")

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar los registros",
                                 Me.ToString(), "CalcularValorSeleccionado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub Aceptar()
        Try
            If Validaciones(LLAMADO_DESDE_ACEPTAR) Then
                IsBusy = True
                IsBusyDetalles = True
                Dim xmlSeleccionados As String = String.Empty
                Dim objRet As LoadOperation(Of RespuestaProcesosGenericos)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()


                If dcProxy Is Nothing Then
                    dcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                End If

                dcProxy.RespuestaProcesosGenericos.Clear()
                xmlSeleccionados = "<PagosDeceval>"

                'JFSB 20160911 Se valida si la opción de Seleccionar todos está marcada
                If SeleccionarTodos = False Then
                    For Each objLista In ListaDetallePaged
                        If objLista.Seleccionado Then
                            If Not IsNothing(objLista.lngIDComitenteAnterior) Then
                                If objLista.lngIDComitenteAnterior <> objLista.lngIDComitente Then
                                    Dim strSeleccionados = <Detalle intIDPagosDeceval=<%= objLista.intIDPagosDeceval %>
                                                               IDComitente=<%= objLista.lngIDComitente %>>
                                                           </Detalle>

                                    xmlSeleccionados = xmlSeleccionados & strSeleccionados.ToString

                                Else
                                    Dim strSeleccionados = <Detalle intIDPagosDeceval=<%= objLista.intIDPagosDeceval %>>
                                                           </Detalle>

                                    xmlSeleccionados = xmlSeleccionados & strSeleccionados.ToString
                                End If
                            Else
                                Dim strSeleccionados = <Detalle intIDPagosDeceval=<%= objLista.intIDPagosDeceval %>>
                                                       </Detalle>

                                xmlSeleccionados = xmlSeleccionados & strSeleccionados.ToString
                            End If
                        End If
                    Next

                    xmlSeleccionados = xmlSeleccionados & "</PagosDeceval>"

                    Dim strXMLDetTesoreriaSeguro As String = System.Web.HttpUtility.HtmlEncode(xmlSeleccionados)

                    objRet = Await dcProxy.Load(dcProxy.Generar_RC_DecevalSyncQuery(strXMLDetTesoreriaSeguro, NroDetalles, RegistroTesoreriaSeleccionado.NombreConsecutivo, RegistroTesoreriaSeleccionado.TipoIdentificacion, RegistroTesoreriaSeleccionado.NroDocumento,
                                   RegistroTesoreriaSeleccionado.Nombre, RegistroTesoreriaSeleccionado.Creacion, RegistroTesoreriaSeleccionado.Usuario, CuentaContable,
                                   CentroCostos, IDBancoGirador, RegistroTesoreriaSeleccionado.NumCheque, RegistroTesoreriaSeleccionado.IDBanco,
                                   FechaConsignar, Observaciones, RegistroTesoreriaSeleccionado.FormaPagoCE, Democratizados, IDEspecie, Sucursal, Tipo, strXMLDetTesoreriaSeguro, Program.Usuario, Program.HashConexion)).AsTask()

                Else

                    For Each objLista In ListaDetallePaged
                        If Not IsNothing(objLista.lngIDComitenteAnterior) Then
                            If objLista.lngIDComitenteAnterior <> objLista.lngIDComitente Then
                                Dim strSeleccionados = <Detalle intIDPagosDeceval=<%= objLista.intIDPagosDeceval %>
                                                           IDComitente=<%= objLista.lngIDComitente %>>
                                                       </Detalle>

                                xmlSeleccionados = xmlSeleccionados & strSeleccionados.ToString
                            End If
                        End If
                    Next

                    xmlSeleccionados = xmlSeleccionados & "</PagosDeceval>"

                    Dim strXMLDetTesoreriaSeguro As String = System.Web.HttpUtility.HtmlEncode(xmlSeleccionados)

                    objRet = Await dcProxy.Load(dcProxy.Generar_RC_DecevalSyncQuery("Todos", NroDetalles, RegistroTesoreriaSeleccionado.NombreConsecutivo, RegistroTesoreriaSeleccionado.TipoIdentificacion, RegistroTesoreriaSeleccionado.NroDocumento,
                       RegistroTesoreriaSeleccionado.Nombre, RegistroTesoreriaSeleccionado.Creacion, RegistroTesoreriaSeleccionado.Usuario, CuentaContable,
                       CentroCostos, IDBancoGirador, RegistroTesoreriaSeleccionado.NumCheque, RegistroTesoreriaSeleccionado.IDBanco,
                       FechaConsignar, Observaciones, RegistroTesoreriaSeleccionado.FormaPagoCE, Democratizados, IDEspecie, Sucursal, Tipo, strXMLDetTesoreriaSeguro, Program.Usuario, Program.HashConexion)).AsTask()

                End If

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al validar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        If objRet.Entities.Count > 0 Then
                            Dim objListaRespuesta As List(Of RespuestaProcesosGenericos)
                            Dim objListaMensajes As New List(Of String)

                            objListaRespuesta = objRet.Entities.ToList

                            If objListaRespuesta.Count > 0 Then
                                For Each li In objListaRespuesta
                                    objListaMensajes.Add(li.Mensaje)
                                Next
                                objViewImportarArchivo.ListaMensajes = objListaMensajes
                            End If

                            IsBusy = False
                            IsBusyDetalles = IsBusyDetalles = True
                            Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                            objViewImportarArchivo.ShowDialog()

                            If objListaRespuesta.Where(Function(i) i.Exitoso = True).Count > 0 Then
                                _numTotalSeleccionados = 0
                                _numTotalGeneral = 0
                                logConsultar = False
                                Sucursal = "0"
                                Tipo = "T"
                                Democratizados = False
                                IDEspecie = Nothing
                                logLlevarValorNroDetalles = True
                                CargarTotalesPagosDeceval()
                                IsBusyDetalles = True
                                logConsultar = True
                                CargarListaPagosDeceval(True)
                            End If

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llemar el proceso para generar los recibos",
                                 Me.ToString(), "Aceptar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'JFSB 20171114 Se agrega porque se utiliza en la pantalla GenerarRecibosCajaDeceval_611View
    Public Sub NombreSucursal_Click()
        If Not String.IsNullOrEmpty(_intSucursal) Then
            If _ListaRegDetalle.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No hay detalles", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Call OpcionSeleccion()
        End If
    End Sub
    'JFSB 20171114 Se agrega porque se utiliza en la pantalla GenerarRecibosCajaDeceval_611View
    Public Sub OpcionSeleccion()
        Try
            Dim intNroSecuencia As Long = 0
            _numTotalSeleccionados = 0
            _numTotalGeneral = 0
            Dim intTotalReg As Integer = _ListaRegDetalle.Count
            Dim intTotalSeleccionados As Integer = 0
            logSeleccionatodos = True

            For Each Lista In _ListaRegDetalle

                If Lista.CodSucursal = _intSucursal Or _SeleccionarTodos Then
                    Lista.Seleccionado = True

                    intTotalSeleccionados = intTotalSeleccionados + 1

                    If Not Lista.VlrAPagar Is Nothing Then
                        _numTotalSeleccionados = _numTotalSeleccionados + Lista.VlrAPagar
                    End If
                Else
                    Lista.Seleccionado = False
                End If

                _numTotalGeneral = _numTotalGeneral + Lista.VlrAPagar
            Next

            If intTotalSeleccionados = intTotalReg Then
                _SeleccionarTodos = True
            End If
            logSeleccionatodos = False
            MyBase.CambioItem("SeleccionarTodos")
            MyBase.CambioItem("ListaDetallePaged")
            MyBase.CambioItem("TotalSeleccionados")
            MyBase.CambioItem("TotalGeneral")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, " Se presentó un error al actualizar el total",
                                 Me.ToString(), "OpcionSeleccion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoGenerar_RC(ByVal lo As InvokeOperation(Of String))
        Try
            IsBusy = False
            If Not lo.HasError Then
                strResultado = String.Empty
                Dim resultado = lo.Value.Split("*")
                strResultado = "Número de Recibos de caja insertados : " & CStr(resultado(0)) & vbCrLf & "Valor de : $ " & resultado(1)
                A2Utilidades.Mensajes.mostrarMensaje(strResultado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                IsBusy = True
                _numTotalSeleccionados = 0
                _numTotalGeneral = 0
                ListaRegDetalle.Clear()
                CargarListaPagosDeceval(False)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Ultimos Valores Sugeridos",
                         Me.ToString(), "TerminoGenerar_RC", Application.Current.ToString(), Program.Maquina, lo.Error)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llemar el proceso para generar los recibos",
                                 Me.ToString(), "Aceptar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Asincronos"

    Private Sub TerminoTraerListaPagosDeceval(ByVal lo As LoadOperation(Of ListaDetalleTesoreria))
        Try
            If Not lo.HasError Then
                IsBusyDetalles = True
                ListaDetalle.Clear()
                _ListaDetalle.Clear()
                ListaRegDetalle.Clear()
                ListaDetalle = dcProxy.ListaDetalleTesorerias.ToList()

                If ListaDetalle.Count > 0 Then

                    CantidadRegistrosOriginales = ListaDetalle.Count

                    For Each i In ListaDetalle
                        NroFilas = i.NroFilas
                        NroRegistrosTotal = NroRegistrosTotal + i.RegistrosporPaginaTotal
                        Exit For
                    Next

                    For Each Lista In _ListaDetalle
                        _ListaRegDetalle.Add(New ListaDetalleTesoreria With {.Seleccionado = False,
                                                                             .Especie = Lista.Especie,
                                                                             .VlrAPagar = Lista.VlrAPagar,
                                                                             .Cliente = Lista.Cliente,
                                                                             .DocumentoDCVAL = Lista.DocumentoDCVAL,
                                                                             .Sucursal = Lista.Sucursal,
                                                                             .Receptor = Lista.Receptor,
                                                                             .Detalle = Lista.Detalle,
                                                                             .Emisor = Lista.Emisor,
                                                                             .strISIN = Lista.strISIN,
                                                                             .lngFungible = Lista.lngFungible,
                                                                             .lngCuentaInversionista = Lista.lngCuentaInversionista,
                                                                             .strIDEspecie = Lista.strIDEspecie,
                                                                             .lngIdEmisor = Lista.lngIdEmisor,
                                                                             .Tipo = Lista.Tipo,
                                                                             .CodSucursal = Lista.CodSucursal,
                                                                             .strIDReceptor = Lista.strIDReceptor,
                                                                             .DocumentoOyD = Lista.DocumentoOyD,
                                                                             .curSaldoContable = Lista.curSaldoContable,
                                                                             .curRecaudoRendimientos = Lista.curRecaudoRendimientos,
                                                                             .curRetencionFuente = Lista.curRetencionFuente,
                                                                             .lngIDComitente = Lista.lngIDComitente,
                                                                             .curImpuestoCree = Lista.curImpuestoCree,
                                                                             .curImpuestoIca = Lista.curImpuestoIca,
                                                                             .curOtrosImpuestos = Lista.curOtrosImpuestos,
                                                                             .NroCodigosOyDxCliente = Lista.NroCodigosOyDxCliente,
                                                                             .intIDPagosDeceval = Lista.intIDPagosDeceval,
                                                                             .strCuentaContableCodigoInactivoCR = Lista.strCuentaContableCodigoInactivoCR,
                                                                             .strCuentaContableCodigoInactivoDB = Lista.strCuentaContableCodigoInactivoDB,
                                                                             .strCuentaContableCodigoInexistenteCR = Lista.strCuentaContableCodigoInexistenteCR,
                                                                             .strCuentaContableCodigoInexistenteDB = Lista.strCuentaContableCodigoInexistenteDB,
                                                                             .strCuentaContableCodigoSinPortafolio = Lista.strCuentaContableCodigoSinPortafolio,
                                                                             .Consecutivo = Lista.Consecutivo})
                        '_numTotalGeneral = _numTotalGeneral + Lista.VlrAPagar

                        If _ListaRegDetalle.Count = CantidadRegistros Then
                            Exit For
                        End If
                    Next

                    MyBase.CambioItem("ListaDetalle")
                    MyBase.CambioItem("ListaDetallePaged")
                    MyBase.CambioItem("RegistroTesoreriaSeleccionado")
                    MyBase.CambioItem("TotalSeleccionados")
                    MyBase.CambioItem("TotalGeneral")
                Else
                    MyBase.CambioItem("ListaDetalle")
                    MyBase.CambioItem("ListaDetallePaged")
                    MyBase.CambioItem("RegistroTesoreriaSeleccionado")
                    MyBase.CambioItem("TotalSeleccionados")
                    MyBase.CambioItem("TotalGeneral")
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(lo.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            If NroRegistrosTotal > 0 Then
                If NroFilas > NroDeRegistro Then
                    Pagina = Pagina + 1
                    If NroFilas = NroRegistrosTotal Then
                        Pagina = 1
                        Exit Sub
                    ElseIf NroRegistrosTotal < NroFilas Then
                        CargarListaPagosDeceval(False)
                    End If
                End If
            End If


            IsBusyDetalles = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarMensaje(ex.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End Try
    End Sub

    Private Sub TerminoConsultarTotales(ByVal lo As LoadOperation(Of PagosDECEVALTotales))
        Try
            If Not lo.HasError Then
                IsBusyDetalles = True
                If lo.Entities.Count > 0 Then
                    If logLlevarValorNroDetalles Then
                        NroDetalles = lo.Entities.First.intCantidadRegistros
                        logLlevarValorNroDetalles = False
                    End If
                    If NroDetalles > CantidadRegistros Then
                        NroDetalles = NroDeRegistro
                    End If
                    ' JFSB 20161003 Se agrega validacion para cuando no existan detalles para procesar
                    If IsNothing(lo.Entities.First.dblValorGeneral) Then
                        TotalGeneral = 0
                        TotalGeneralOriginal = 0
                        TotalSeleccionadosOriginal = 0
                    Else
                        TotalGeneral = lo.Entities.First.dblValorGeneral
                        TotalGeneralOriginal = lo.Entities.First.dblValorGeneral
                    End If
                    If logSeleccionatodos Then
                        If Not IsNothing(lo.Entities.First.dblValorSeleccionado) Then
                            _numTotalSeleccionados = lo.Entities.First.dblValorSeleccionado
                            _numTotalSeleccionadosOriginal = lo.Entities.First.dblValorSeleccionado
                        Else
                            _numTotalSeleccionados = 0
                            _numTotalSeleccionadosOriginal = 0

                        End If
                    Else
                        _numTotalSeleccionados = 0
                        _numTotalSeleccionadosOriginal = 0
                    End If
                End If

                MyBase.CambioItem("TotalGeneralOriginal")
                MyBase.CambioItem("TotalSeleccionadosOriginal")
                MyBase.CambioItem("TotalSeleccionados")
                MyBase.CambioItem("TotalGeneral")

                IsBusyDetalles = False
            Else
                A2Utilidades.Mensajes.mostrarMensaje(lo.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarMensaje(ex.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End Try
    End Sub

    Private Sub TerminoTraerrecibos(ByVal lo As InvokeOperation(Of System.Nullable(Of Boolean)))
        Try
            If Not lo.HasError Then
                TerminoCrearArchivo()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los datos para el reporte titulos",
                                 Me.ToString(), "TerminoTraerTitulos", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los datos para el reporte titulos",
                                 Me.ToString(), "TerminoTraerTitulos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Sub TerminoCrearArchivo()
        Try
            Dim cwCar As New A2ComunesImportaciones.ListarArchivosDirectorioView(CSTR_NOMBREPROCESO_CSVREPORTE) '(CSTR_NOMBREPROCESO_RECIBOSDECAJA)
            Program.Modal_OwnerMainWindowsPrincipal(cwCar)
            cwCar.ShowDialog()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al levantar la ventana de visualización de los archivos",
                                 Me.ToString(), "TerminoCrearArchivo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    'JCM20160216
    Private Sub TerminoTraerConsecutivos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If objProxyUtil.ItemCombos.Count > 0 Then
                    listConsecutivos = objProxyUtil.ItemCombos.Where(Function(y) y.Categoria = "CAJA").ToList
                Else
                    listConsecutivos = Nothing
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'CORREC_CITI_SV_2014
    Private Sub Terminotraerparametro(ByVal obj As InvokeOperation(Of String))
        Try
            If obj.HasError Then
                obj.MarkErrorAsHandled()
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el parametro", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                Select Case obj.UserState
                    Case "Top_Consulta_Detalle_Tesoreria"
                        intNroDetallesPermitidos = 100
                        intNroDetallesPermitidos = IIf(Versioned.IsNumeric(obj.Value), obj.Value, 100)

                        objProxyUtil.Verificaparametro("FORMAPAGO_DEFECTO_RC_DVAL", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "FORMAPAGO_DEFECTO_RC_DVAL")
                    Case "FORMAPAGO_DEFECTO_RC_DVAL"
                        strFormaPagoPorDefecto = obj.Value
                        If Not IsNothing(RegistroTesoreriaSeleccionado) Then
                            RegistroTesoreriaSeleccionado.FormaPagoCE = strFormaPagoPorDefecto
                        End If

                        objProxyUtil.Verificaparametro("COLOR_RC_DVAL_CODIGOSPORCLIENTE", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "COLOR_RC_DVAL_CODIGOSPORCLIENTE")
                    Case "COLOR_RC_DVAL_CODIGOSPORCLIENTE"
                        If Application.Current.Resources.Contains("HabilitarResaltoColorDeceval") Then
                            Application.Current.Resources.Remove("HabilitarResaltoColorDeceval")
                        End If
                        Application.Current.Resources.Add("HabilitarResaltoColorDeceval", obj.Value)

                        objProxyUtil.Verificaparametro("PAGOSDECEVAL_GENERARRECIBOSANULADOS", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "PAGOSDECEVAL_GENERARRECIBOSANULADOS")
                    Case "PAGOSDECEVAL_GENERARRECIBOSANULADOS"
                        If obj.Value = "SI" Then
                            MostrarCamposDemocratizados = Visibility.Visible
                        Else
                            MostrarCamposDemocratizados = Visibility.Collapsed
                        End If

                        If Not IsNothing(dcProxy.ItemCombos) Then
                            objProxyUtil.ItemCombos.Clear()
                        End If
                        objProxyUtil.Load(objProxyUtil.cargarCombosEspecificosQuery("Tesoreria_RecibosCaja", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, Nothing)
                    Case "CANTIDAD_REGISTROS_DETALLE"
                        If Not IsNothing(obj.Value) Then
                            CantidadRegistros = Convert.ToInt32(obj.Value)
                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el parametro", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _DiccionarioCarga As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCarga() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCarga
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCarga = value
            MyBase.CambioItem("DiccionarioCarga")
        End Set
    End Property

    Private WithEvents _Valoreselected As Tesoreri
    Public Property ValoresSelected() As Tesoreri
        Get
            Return _Valoreselected
        End Get
        Set(value As Tesoreri)
            _Valoreselected = value
            MyBase.CambioItem("ValoresSelected")
        End Set
    End Property

    Private _RegistroTesoreriaSeleccionado As New Tesoreri
    Public Property RegistroTesoreriaSeleccionado() As Tesoreri
        Get

            If _RegistroTesoreriaSeleccionado.TipoIdentificacion Is Nothing Then
                _RegistroTesoreriaSeleccionado.TipoIdentificacion = "N"
            End If

            'DEMC20171017 Se comenta linea ya que la fecha doc debe guardar con la fecha parametrizada.
            'If IsDate(_RegistroTesoreriaSeleccionado.Creacion) Then
            '    _RegistroTesoreriaSeleccionado.Creacion = Now.Date
            'End If

            If _RegistroTesoreriaSeleccionado.Nombre Is Nothing Or String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.Nombre) Then
                _RegistroTesoreriaSeleccionado.Nombre = "Deceval S.A."
            End If

            Return _RegistroTesoreriaSeleccionado
        End Get
        Set(ByVal value As Tesoreri)
            _RegistroTesoreriaSeleccionado = value
            MyBase.CambioItem("RegistroTesoreriaSeleccionado")
        End Set
    End Property

    Private _strCuentaContable As String
    Public Property CuentaContable() As String
        Get
            Return _strCuentaContable
        End Get
        Set(ByVal value As String)
            _strCuentaContable = value
            MyBase.CambioItem("CuentaContable")
        End Set
    End Property

    Private _strNombreBanco As String
    Public Property NombreBanco() As String
        Get
            Return _strNombreBanco
        End Get
        Set(ByVal value As String)
            _strNombreBanco = value
            MyBase.CambioItem("NombreBanco")
        End Set
    End Property

    Private _strIdBancoGirador As String
    Public Property IDBancoGirador() As String
        Get
            Return _strIdBancoGirador
        End Get
        Set(ByVal value As String)
            _strIdBancoGirador = value
            MyBase.CambioItem("IDBancoGirador")
        End Set
    End Property

    Private _strNombreBancoGirador As String
    Public Property NombreBancoGirador() As String
        Get
            Return _strNombreBancoGirador
        End Get
        Set(ByVal value As String)
            _strNombreBancoGirador = value
            MyBase.CambioItem("NombreBancoGirador")
        End Set
    End Property

    Private _strObservaciones As String
    Public Property Observaciones() As String
        Get
            Return _strObservaciones
        End Get
        Set(ByVal value As String)
            _strObservaciones = value
            MyBase.CambioItem("Observaciones")
        End Set
    End Property

    Private _strCentroCostos As String
    Public Property CentroCostos() As String
        Get
            Return _strCentroCostos
        End Get
        Set(ByVal value As String)
            _strCentroCostos = value
            MyBase.CambioItem("CentroCostos")
        End Set
    End Property

    Private _ListaDetalle As New List(Of ListaDetalleTesoreria)
    Public Property ListaDetalle() As List(Of ListaDetalleTesoreria)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of ListaDetalleTesoreria))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
        End Set
    End Property


    Private _ListaRegDetalle As New ObservableCollection(Of ListaDetalleTesoreria)
    Public Property ListaRegDetalle() As ObservableCollection(Of ListaDetalleTesoreria)
        Get
            Return _ListaRegDetalle
        End Get
        Set(ByVal value As ObservableCollection(Of ListaDetalleTesoreria))
            _ListaRegDetalle = value
            MyBase.CambioItem("ListaRegDetalle")
        End Set
    End Property


    Public ReadOnly Property ListaDetallePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaRegDetalle) Then
                Dim view = New PagedCollectionView(_ListaRegDetalle)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _FechaConsignar As Date
    Public Property FechaConsignar() As Date
        Get
            Return _FechaConsignar
        End Get
        Set(ByVal value As Date)
            _FechaConsignar = value
            MyBase.CambioItem("FechaConsignar")
        End Set
    End Property

    Private _SeleccionarTodos As System.Nullable(Of Boolean)
    Public Property SeleccionarTodos() As System.Nullable(Of Boolean)
        Get
            If _SeleccionarTodos Is Nothing Then
                _SeleccionarTodos = False
            End If
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As System.Nullable(Of Boolean))
            _SeleccionarTodos = value
            If logLanzarCodigoSeleccionarTodos Then
                If IsNothing(DesmarcarTodos) Then
                    SeleccionarTodosChecked(_SeleccionarTodos, False)
                End If
            End If
            MyBase.CambioItem("SeleccionarTodos")
        End Set
    End Property

    Private _numTotalSeleccionados As Decimal
    Public Property TotalSeleccionados() As Decimal
        Get
            Return _numTotalSeleccionados
        End Get
        Set(ByVal value As Decimal)
            _numTotalSeleccionados = value
            MyBase.CambioItem("TotalSeleccionados")
        End Set
    End Property

    Private _numTotalSeleccionadosOriginal As Decimal
    Public Property TotalSeleccionadosOriginal() As Decimal
        Get
            Return _numTotalSeleccionadosOriginal
        End Get
        Set(ByVal value As Decimal)
            _numTotalSeleccionadosOriginal = value
            MyBase.CambioItem("TotalSeleccionadosOriginal")
        End Set
    End Property

    Private _numTotalGeneral As Decimal
    Public Property TotalGeneral() As Decimal
        Get
            Return _numTotalGeneral
        End Get
        Set(ByVal value As Decimal)
            _numTotalGeneral = value
            MyBase.CambioItem("TotalGeneral")
        End Set
    End Property

    Private _numTotalGeneralOriginal As Decimal
    Public Property TotalGeneralOriginal() As Decimal
        Get
            Return _numTotalGeneralOriginal
        End Get
        Set(ByVal value As Decimal)
            _numTotalGeneralOriginal = value
            MyBase.CambioItem("TotalGeneralOriginal")
        End Set
    End Property

    Private _bolVisibleContraPartida As Visibility
    Public Property VisibleContraPartida() As Visibility
        Get
            Return _bolVisibleContraPartida
        End Get
        Set(ByVal value As Visibility)
            _bolVisibleContraPartida = value
            MyBase.CambioItem("VisibleContraPartida")
        End Set
    End Property

    Private _intNroDetalles As System.Nullable(Of Integer) = 100
    Public Property NroDetalles() As System.Nullable(Of Integer)
        Get
            Return _intNroDetalles
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intNroDetalles = value
            MyBase.CambioItem("NroDetalles")
        End Set
    End Property
    Private WithEvents _SelectedDetalle As ListaDetalleTesoreria
    Public Property SelectedDetalle() As ListaDetalleTesoreria
        Get
            Return _SelectedDetalle
        End Get
        Set(ByVal value As ListaDetalleTesoreria)
            _SelectedDetalle = value
            MyBase.CambioItem("SelectedDetalle")
        End Set
    End Property


    'JCM20150216
    'Propiedad para cargar los consecutivos de los documentos
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


    ' Compañía asociada al consecutivo seleccionado
    Private _strCompaniaConsecutivo As String = String.Empty
    Public Property strCompaniaConsecutivo() As String
        Get
            Return _strCompaniaConsecutivo
        End Get
        Set(ByVal value As String)
            _strCompaniaConsecutivo = value
            MyBase.CambioItem("strCompaniaConsecutivo")
        End Set
    End Property

    Public Property strCompaniaBanco As String = String.Empty

    Private _intSucursal As String = String.Empty
    Public Property Sucursal() As String
        Get
            Return _intSucursal
        End Get
        Set(ByVal value As String)
            _intSucursal = value
            If Not IsNothing(_intSucursal) Then
                If logConsultar Then
                    IsBusyDetalles = True
                    CargarListaPagosDeceval(True)
                End If
            End If
            MyBase.CambioItem("Sucursal")
        End Set
    End Property

    Private _strTipo As String
    Public Property Tipo() As String
        Get
            Return _strTipo
        End Get
        Set(ByVal value As String)
            _strTipo = value
            If Not IsNothing(_strTipo) Then
                If logConsultar Then
                    IsBusyDetalles = True
                    CargarListaPagosDeceval(True)
                End If
            End If
            MyBase.CambioItem("Tipo")
        End Set
    End Property

    Private _Democratizados As Boolean
    Public Property Democratizados() As Boolean
        Get
            Return _Democratizados
        End Get
        Set(ByVal value As Boolean)
            _Democratizados = value
            If Not IsNothing(_Democratizados) Then
                If Not IsNothing(dcProxy.ItemCombos) Then
                    objProxyUtil.ItemCombos.Clear()
                End If
                objProxyUtil.Load(objProxyUtil.cargarCombosCondicionalQuery("CONSECUTIVOS_DEMOCRATIZADOS", IIf(_Democratizados, "SI", "NO"), 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, Nothing)
                If logConsultar Then
                    IsBusyDetalles = True
                    CargarListaPagosDeceval(True)
                End If
            End If
            MyBase.CambioItem("Democratizados")
        End Set
    End Property

    Private _IDEspecie As String
    Public Property IDEspecie() As String
        Get
            Return _IDEspecie
        End Get
        Set(ByVal value As String)
            _IDEspecie = value
            If Not IsNothing(_IDEspecie) Then
                If logConsultar Then
                    IsBusyDetalles = True
                    CargarListaPagosDeceval(True)
                End If

            End If
            MyBase.CambioItem("IDEspecie")
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

    Private _MostrarCamposDemocratizados As Visibility = Visibility.Visible
    Public Property MostrarCamposDemocratizados() As Visibility
        Get
            Return _MostrarCamposDemocratizados
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposDemocratizados = value
            MyBase.CambioItem("MostrarCamposDemocratizados")
        End Set
    End Property
#End Region

    Private Sub _ValoresSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _Valoreselected.PropertyChanged
        If e.PropertyName = "NombreConsecutivo" Then
            If Not IsNothing(_Valoreselected.NombreConsecutivo) Then
                strCompaniaConsecutivo = (From c In listConsecutivos Where c.ID = _Valoreselected.NombreConsecutivo Select c).FirstOrDefault.Retorno
            Else
                strCompaniaConsecutivo = String.Empty
            End If
            LimpiarBanco()
        End If
    End Sub

#Region "Funciones y Mtodos Generales"

    Private Function Validaciones(ByVal LlamadaDesde As String) As Boolean
        '/*****************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function Validaciones  As Boolean
        '/* Alcance     : Private
        '/* Descripción : Valida el ingreso de los campos que son obligatorios en la forma
        '/* Parámetros  :
        '/* Por             Nombre      Tipo        Descripción
        '/* LlamadaDesde    Permite Saber el evento desde el cual se llamo
        '/* Valores de retorno:
        '/* devuelve  TRUE si todos los datos fueron correctos
        '/*                  FALSE si falto algun campo obligatorio
        '/* FIN DOCUMENTO
        '/******************************************************************************************
        Dim seleccionados As Boolean
        Dim strMsg As String = String.Empty
        Dim bolPresentaInconsistencia As Boolean = False
        Dim nroFila As Integer = 0

        Try

            Validaciones = False

            If IsBusyDetalles Then
                strMsg = strMsg & "* " & "No se han cargado todos los detalles, por favor espere para ejecutar la generación de los documentos." & vbCrLf
                bolPresentaInconsistencia = True
            End If

            If String.IsNullOrEmpty(RegistroTesoreriaSeleccionado.FormaPagoCE) Then
                strMsg = strMsg & "* " & "Debe de seleccionar la forma de pago. " & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Consecutivo
            If String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.NombreConsecutivo) Then
                strMsg = strMsg & "* " & CONSECUTIVO_NO_VALIDA & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Sucursal
            'If String.IsNullOrEmpty(Sucursal) Then
            '    strMsg = strMsg & "* " & SUCURSAL_NO_VALIDA & vbCrLf
            '    bolPresentaInconsistencia = True
            'ElseIf Sucursal = 0 Then
            '    strMsg = strMsg & "* " & SUCURSAL_NO_VALIDA & vbCrLf
            '    bolPresentaInconsistencia = True
            'End If

            'Cuenta Contable
            'If String.IsNullOrEmpty(_strCuentaContable) Then
            '    strMsg = strMsg & "* " & CUENTACONTABLE_NO_VALIDA & vbCrLf
            '    bolPresentaInconsistencia = True
            'End If

            'Fecha de Corte
            If Not IsDate(_RegistroTesoreriaSeleccionado.Creacion) Then
                strMsg = strMsg & "* " & FECHARECIBO_NO_VALIDA & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Se recibe de.... Por defecto Deceval S.A.
            If String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.Nombre) Then
                strMsg = strMsg & "* " & RECIBI_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Tipo de Identificacion
            If String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.TipoIdentificacion) Then
                strMsg = strMsg & "* " & TIPOID_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Valida Nro de Documento que no llegue vacio.
            If _RegistroTesoreriaSeleccionado.NroDocumento Is Nothing Then
                strMsg = strMsg & "* " & NROID_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Codigo del Banco
            If _RegistroTesoreriaSeleccionado.IDBanco Is Nothing Then
                strMsg = strMsg & "* " & BANCO_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Codigo banco girador
            'If String.IsNullOrEmpty(_strIdBancoGirador) Then
            '    strMsg = strMsg & "* " & BANCOGIRADOR_NO_VALIDO & vbCrLf
            '    bolPresentaInconsistencia = True
            'End If

            'Valida Cheque
            'If IsNothing(_RegistroTesoreriaSeleccionado.NumCheque) Then
            '    strMsg = strMsg & "* " & CHEQUE_NO_VALIDO & vbCrLf
            '    bolPresentaInconsistencia = True
            'End If

            'Fecha a consignar
            If Not IsDate(_FechaConsignar) Then
                strMsg = strMsg & "* " & FECHACONSIGNACION_NO_VALIDA & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Nro de Detalles
            If NroDetalles <= 0 Then
                strMsg = strMsg & "* " & NRODETALLES_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            If NroDetalles > 5000 Then
                strMsg = strMsg & "* " & NRODETALLES_POR_RECIBO & 5000 & vbCrLf
                bolPresentaInconsistencia = True
            End If

            Dim intCantidadSeleccionados As Integer = 0

            If Not IsNothing(_ListaRegDetalle) Then
                intCantidadSeleccionados = _ListaRegDetalle.Where(Function(i) i.Seleccionado).Count
            End If

            'CORREC_CITI_SV_2014
            If intCantidadSeleccionados > intNroDetallesPermitidos Then
                strMsg = strMsg & "* " & String.Format(NRO_DETALLES_MAYOR, intNroDetallesPermitidos) & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Valida que se haya seleccionado un usuario
            If String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.Usuario) Then
                strMsg = strMsg & "* " & USUARIO_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            If (_strObservaciones & "") = "" Then
                _strObservaciones = ""
            End If

            With _ListaRegDetalle

                If .Count = 0 Then
                    strMsg = strMsg & "* " & NRODETALLES_NO_VALIDO & vbCrLf
                    bolPresentaInconsistencia = True
                End If

                seleccionados = False

                For Each lista In _ListaRegDetalle
                    nroFila = nroFila + 1
                    If SeleccionarTodos() Then lista.Seleccionado = True

                    If lista.Seleccionado Then
                        seleccionados = True
                    End If

                    If lista.VlrAPagar > 0 Then
                        If String.IsNullOrEmpty(lista.Detalle) Then
                            strMsg = strMsg & "* No existe detalle" & vbCrLf
                            bolPresentaInconsistencia = True
                            Exit For
                        End If
                    Else
                        If IsNothing(lista.VlrAPagar) Then
                            strMsg = strMsg & "* No existe Valor a pagar en la linea " + nroFila.ToString & vbCrLf
                            bolPresentaInconsistencia = True
                            Exit For
                        End If
                    End If
                Next
            End With

            If Not seleccionados And LlamadaDesde = LLAMADO_DESDE_ACEPTAR Then
                strMsg = strMsg & "* " & NO_SELECCIONADOS & vbCrLf
                bolPresentaInconsistencia = True
            End If

            If bolPresentaInconsistencia Then
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            Return Not bolPresentaInconsistencia

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación de campos",
                                 Me.ToString(), "GenerarRecibosCajaDecevalViewModel.Validaciones", Program.TituloSistema, Program.Maquina, ex)
            Return False
        End Try

    End Function

    Public Sub EjecutarConsulta()
        Try
            'SLB20131108 Se genera el informe desde el Visor de Reportes
            IsBusy = True

            Dim strParametros As String
            Dim strRegistrosSeleccionados As String = String.Empty
            Dim objListaIDSeleccionados As List(Of Integer) = New List(Of Integer)

            For Each Lista In _ListaRegDetalle
                If Lista.Seleccionado Then
                    If Not objListaIDSeleccionados.Contains(Lista.intIDPagosDeceval) Then
                        objListaIDSeleccionados.Add(Lista.intIDPagosDeceval)
                    End If
                End If
            Next

            If SeleccionarTodos Then
                strRegistrosSeleccionados = "Todos"
            Else
                For Each li In objListaIDSeleccionados
                    If String.IsNullOrEmpty(strRegistrosSeleccionados) Then
                        strRegistrosSeleccionados = li
                    Else
                        strRegistrosSeleccionados = String.Format("{0},{1}", strRegistrosSeleccionados, li)

                    End If
                Next
            End If

            If String.IsNullOrEmpty(strRegistrosSeleccionados) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se ha seleccionado ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            strParametros = " '@Pagina'**'" & 0 & "'**'INTEGER'|" _
                            & " '@RegistrosporPagina'**'" & 0 & "'**'INTEGER'|" _
                            & " '@plogDemocratizados'**'" & IIf(Democratizados = True, 1, 0) & "'**'BOOLEAN'|" _
                            & " '@pstrIDEspecie'**'" & IIf(IDEspecie Is Nothing, Nothing, IDEspecie) & "'**'STRING'|" _
                            & " '@pintSucursal'**'" & Sucursal & "'**'INTEGER'|" _
                            & " '@pstrRegistrosSeleccionados'**'" & strRegistrosSeleccionados & "'**'STRING'|" _
                            & " '@pstrTipo'**'" & Tipo & "'**'STRING'|"

            Dim objRespuesta = objServicios.GenerarArchivo("Recibos Caja Deceval", Program.Usuario, Application.Current.Resources("A2RutaFisicaGeneracion").ToString, strParametros, "SpConsultarPagosDecevalOyDNet", Program.Maquina, A2VisorReportes.A2.Visor.Servicios.TipoArchivo.EXCEL.EXCEL, "Recibos Caja Deceval", Program.HashConexion("Recibos Caja Deceval"))
            TerminoGenerarArchivo(objRespuesta)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al ejecutar la consulta.",
                                 Me.ToString(), "ReporteRecibosdeCajaViewModel.EjecutarConsulta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoGenerarArchivo(ByVal e As A2VisorReportes.A2.Visor.Servicios.RespuestaArchivo)
        Try
            IsBusy = False
            If e.EjecucionExitosa Then
                TerminoCrearArchivo()
            Else
                Dim ex1 As New Exception(e.Mensaje)
                A2Utilidades.Mensajes.mostrarMensaje("Se presentó un error en el momento de generar el archivo plano." & ex1.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error en el momento de generar el archivo plano.",
                                 Me.ToString(), "TerminoGenerarArchivoPlano", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _SelectedDetalle_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _SelectedDetalle.PropertyChanged
        Try
            If e.PropertyName = "Seleccionado" Then
                CalcularValorSeleccionado()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la edición del detalle registro",
                                 Me.ToString(), "_SelectedDetalle_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CrearColumnasTitulos(ByVal pgrid As DataGrid)
        '<--
        Dim Especie As New DataGridTextColumn
        Especie.Header = "Especie"
        pgrid.Columns.Add(Especie)

        Dim VlrAPagar As New DataGridTextColumn
        VlrAPagar.Header = "Valor A Pagar"
        pgrid.Columns.Add(VlrAPagar)

        Dim Cliente As New DataGridTextColumn
        Cliente.Header = "Cliente"
        pgrid.Columns.Add(Cliente)

        Dim DocumentoDCVAL As New DataGridTextColumn
        DocumentoDCVAL.Header = "Documento DECEVAL"
        pgrid.Columns.Add(DocumentoDCVAL)

        Dim Sucursal As New DataGridTextColumn
        Sucursal.Header = "Sucursal"
        pgrid.Columns.Add(Sucursal)

        Dim Receptor As New DataGridTextColumn
        Receptor.Header = "Receptor"
        pgrid.Columns.Add(Receptor)

        Dim lngIDComitente As New DataGridTextColumn
        lngIDComitente.Header = "IDComitente"
        pgrid.Columns.Add(lngIDComitente)

        Dim Detalle As New DataGridTextColumn
        Detalle.Header = "Detalle"
        pgrid.Columns.Add(Detalle)

        Dim Emisor As New DataGridTextColumn
        Emisor.Header = "Emisor"
        pgrid.Columns.Add(Emisor)

        Dim strISIN As New DataGridTextColumn
        strISIN.Header = "ISIN"
        pgrid.Columns.Add(strISIN)

        Dim lngFungible As New DataGridTextColumn
        lngFungible.Header = "Fungible"
        pgrid.Columns.Add(lngFungible)

        Dim lngCuentaInversionista As New DataGridTextColumn
        lngCuentaInversionista.Header = "Cuenta Inversionista"
        pgrid.Columns.Add(lngCuentaInversionista)

        Dim strIDEspecie As New DataGridTextColumn
        strIDEspecie.Header = "IDEspecie"
        pgrid.Columns.Add(strIDEspecie)

        Dim lngIdEmisor As New DataGridTextColumn
        lngIdEmisor.Header = "IdEmisor"
        pgrid.Columns.Add(lngIdEmisor)

        Dim Tipo As New DataGridTextColumn
        Tipo.Header = "Tipo"
        pgrid.Columns.Add(Tipo)

        Dim CodSucursal As New DataGridTextColumn
        CodSucursal.Header = "Codigo Sucursal"
        pgrid.Columns.Add(CodSucursal)

        Dim strIDReceptor As New DataGridTextColumn
        strIDReceptor.Header = "IDReceptor"
        pgrid.Columns.Add(strIDReceptor)

        Dim DocumentoOyD As New DataGridTextColumn
        DocumentoOyD.Header = "Documento OyD"
        pgrid.Columns.Add(DocumentoOyD)

        Dim CurSaldoContable As New DataGridTextColumn
        CurSaldoContable.Header = "Saldo Contable"
        pgrid.Columns.Add(CurSaldoContable)

        Dim curRecaudoRendimientos As New DataGridTextColumn
        curRecaudoRendimientos.Header = "Recaudo Rendimientos"
        pgrid.Columns.Add(curRecaudoRendimientos)

        Dim curRecaudoCapital As New DataGridTextColumn
        curRecaudoCapital.Header = "Recaudo Capital"
        pgrid.Columns.Add(curRecaudoCapital)

        Dim curRetencionFuente As New DataGridTextColumn
        curRetencionFuente.Header = "Retencion Fuente"
        pgrid.Columns.Add(curRetencionFuente)
        '-->
    End Sub

    Private Sub exportDataGrid(ByVal dGrid As DataGrid, ByVal strFormat As String)

        Dim strBuilder As New StringBuilder()
        Dim strLineas As New List(Of String)

        If IsNothing(dGrid) Then Return
        Dim lstFields As List(Of String) = New List(Of String)()
        If dGrid.HeadersVisibility = DataGridHeadersVisibility.Column OrElse dGrid.HeadersVisibility = DataGridHeadersVisibility.All Then
            For Each dgcol As DataGridColumn In dGrid.Columns
                lstFields.Add(formatField(dgcol.Header.ToString(), strFormat, True))
            Next
            buildStringOfRow(strBuilder, lstFields, strFormat)
            strLineas.Add(strBuilder.ToString())
        End If
        dGrid.ItemsSource = _ListaRegDetalle

        For a = 0 To _ListaRegDetalle.Count - 1
            lstFields.Clear()
            strBuilder.Clear()
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Especie), String.Empty, _ListaRegDetalle(a).Especie)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).VlrAPagar), String.Empty, _ListaRegDetalle(a).VlrAPagar)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Cliente), String.Empty, _ListaRegDetalle(a).Cliente)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).DocumentoDCVAL), String.Empty, _ListaRegDetalle(a).DocumentoDCVAL)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Sucursal), String.Empty, _ListaRegDetalle(a).Sucursal)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Receptor), String.Empty, _ListaRegDetalle(a).Receptor)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).lngIDComitente), String.Empty, _ListaRegDetalle(a).lngIDComitente)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Detalle), String.Empty, _ListaRegDetalle(a).Detalle)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Emisor), String.Empty, _ListaRegDetalle(a).Emisor)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).strISIN), String.Empty, _ListaRegDetalle(a).strISIN)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).lngFungible), String.Empty, _ListaRegDetalle(a).lngFungible)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).lngCuentaInversionista), String.Empty, _ListaRegDetalle(a).lngCuentaInversionista)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).strIDEspecie), String.Empty, _ListaRegDetalle(a).strIDEspecie)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).lngIdEmisor), String.Empty, _ListaRegDetalle(a).lngIdEmisor)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Tipo), String.Empty, _ListaRegDetalle(a).Tipo)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).CodSucursal), String.Empty, _ListaRegDetalle(a).CodSucursal)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).strIDReceptor), String.Empty, _ListaRegDetalle(a).strIDReceptor)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).DocumentoOyD), String.Empty, _ListaRegDetalle(a).DocumentoOyD)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).curSaldoContable), String.Empty, _ListaRegDetalle(a).curSaldoContable)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).curRecaudoRendimientos), String.Empty, _ListaRegDetalle(a).curRecaudoRendimientos)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).curRecaudoCapital), String.Empty, _ListaRegDetalle(a).curRecaudoCapital)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).curRetencionFuente), String.Empty, _ListaRegDetalle(a).curRetencionFuente)), strFormat, False))


            buildStringOfRow(strBuilder, lstFields, strFormat)
            strLineas.Add(strBuilder.ToString())
        Next

        dcProxy1.Guardar_ArchivoServidor(CSTR_NOMBREPROCESO_RECIBOSDECAJA, Program.Usuario, String.Format("ReporteTitulosActivos{0}.csv", Now.ToString("yyyy-mm-dd")), strLineas, Program.HashConexion, AddressOf TerminoCrearArchivo, True)

    End Sub

    Private Shared Function formatField(ByVal data As String, ByVal format As String, ByVal encabezado As Boolean) As String
        Select Case format
            Case "XML"
                Return String.Format("<Cell><Data ss:Type=""String" & """>{0}</Data></Cell>", data)
            Case "CSV"
                If encabezado = True Then
                    Return String.Format("""   {0}   """, data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                Else
                    Return String.Format("""{0}""", data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                End If
        End Select
        Return data

    End Function

    Private Shared Sub buildStringOfRow(ByVal strBuilder As StringBuilder, ByVal lstFields As List(Of String), ByVal strFormat As String)
        Select Case strFormat
            Case "XML"
                strBuilder.AppendLine("<Row>")
                strBuilder.AppendLine(String.Join("" & vbCrLf & "", lstFields.ToArray()))
                strBuilder.AppendLine("</Row>")
                ' break;
            Case "CSV"
                strBuilder.AppendLine(String.Join(SEPARATOR_FORMAT_CVS, lstFields.ToArray()))
                ' break;
        End Select
    End Sub

    

    'JCM20160217
    Public Sub LimpiarBanco()

        RegistroTesoreriaSeleccionado.IDBanco = Nothing
        NombreBanco = Nothing
        strCompaniaBanco = String.Empty
    End Sub



#End Region


End Class