Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports A2.Notificaciones.Cliente
Imports GalaSoft.MvvmLight.Messaging
Imports System.Globalization

'JDCP20170124
'Se realizan ajustes de codigo para adaptar el tipo de compañia tesoreria para el desarrollo omnibus de Citi.

''' <summary>
''' ViewModel para la pantalla Compañías perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' <history>
''' Creado por       : Javier Pardo  - Alcuadrado S.A.
''' Descripción      : Creación.
''' Fecha            : Julio 29/2015
''' Pruebas CB       : Javier Pardo - Julio 29/2015 - Resultado Ok 
''' </history>

Public Class CompaniasViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Proxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As Companias
    Private mdcProxy As CalculosFinancierosDomainContext    ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mobjVM As CompaniasViewModel

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As Companias
    Private mobjDetallePorDefecto As CompaniasCodigos
    Private mobjDetallePenalidadPorDefecto As CompaniasPenalidades
    Private mobjDetalleTesoreriaPorDefecto As CompaniasTesoreria
    Private mobjDetalleLimitesPorDefecto As CompaniasLimites
    Private mobjDetalleAutorizacionesPorDefecto As CompaniasAutorizaciones
    Private mobjDetalleCondicionesTesoreriaPorDefecto As CompaniasPenalidades
    Private mobjDetalleTesoreriaCondicionesPorDefecto As CompaniasCondicionesTesoreria
    Private mobjDetalleParametrosCompaniaPorDefecto As ParametrosCompania
    Private mobjDetalleAcumuladosComisionesPorDefecto As CompaniasAcumuladoComisiones
    Public _mlogBuscarClienteEncabezado As Boolean = True
    Public _mLogBuscarClienteDetalle As Boolean = True
    Public objProxy As UtilidadesDomainContext
    Private strNroDto As String
    Private strNroDocumentoActual As String
    Private logPreguntarCambio As Boolean
    Private strComitenteActual As String
    Private logParametrosPorDefecto As Boolean


    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la variable de cambio
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjTipoParticipacion As String
    Private mobjCambioParticipacion As Boolean
    Private NoValidarCambio As Boolean

    'JCM20160209
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private Const PARAM_STR_CALCULOS_FINANCIEROS As String = "CF_UTILIZACALCULOSFINANCIEROS"
    Public strCodigoOYDEncabezado As String = String.Empty
    Private strCodigoSeleccionado As String = String.Empty
    Private strCodOyDPartidas As String = String.Empty

    'JCM20180329
    Private intCantidadDecimales As Integer


    'JDCP20170124
    Public logHabilitarCompaniaPasivaTesoreria As Boolean = False
    Private Const PARAM_CF_UTILIZAPASIVA_TESORERIA_A2 As String = "CF_UTILIZAPASIVA_TESORERIA_A2"
    Private Const TIPOCOMPANIA_TESORERIA As String = "TES"
    Private Const PARTICIPACION_CONSOLIDACLASES As String = "CC"

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = New UtilidadesDomainContext()
        Else
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        If Not Program.IsDesignMode() Then
            DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)

            IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
            logPreguntarCambio = True
        End If



    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <history>
    ''' Creado por       : Javier Pardo (Alcuadrado S.A.)
    ''' Descripción      : Creación.
    ''' Fecha            : Julio 29/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Julio 29/2015 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then


                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()
                ConsultarDetallePorDefectoSync()
                ConsultarPenalidadPorDefectoSync()
                ConsultarTesoreriaPorDefectoSync()
                ConsultarLimitesPorDefectoSync()
                ConsultarDetalleAutorizacionesPorDefectoSync()
                ConsultarCondicionesTesoreriaPorDefectoSync()
                ConsultarParametrosCompaniaPorDefectoSync()
                ConsultarAcumuladoComisionesPorDefectoSync()

                '------------------------------------------------------------------------------------------------------------------------------------------------
                '-- Se valida los valores del parametro de calculos financieros
                'ID caso de prueba:  CP051
                '------------------------------------------------------------------------------------------------------------------------------------------------

                Dim Proxyparametro As UtilidadesDomainContext
                Proxyparametro = inicializarProxyUtilidadesOYD()

                'JDCP20170124
                '************************************************************************************************************************************
                If Not IsNothing(objProxy.ItemCombos) Then
                    objProxy.ItemCombos.Clear()
                End If
                objProxy.Load(objProxy.cargarCombosCondicionalQuery("LISTA_TIPOCOMPANIA", String.Empty, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosCondicional, "TIPOCOMPANIA")
                objProxy.Load(objProxy.cargarCombosCondicionalQuery("TIPOPLAZO_PASIVA", String.Empty, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosCondicional, "TIPOPLAZO_PASIVA")
                objProxy.Load(objProxy.cargarCombosCondicionalQuery("TIPOPARTICIPACION_PASIVA", String.Empty, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosCondicional2, "TIPOPARTICIPACION_PASIVA")


                Proxyparametro.Verificaparametro(PARAM_STR_CALCULOS_FINANCIEROS, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroCF, PARAM_STR_CALCULOS_FINANCIEROS)
                Proxyparametro.Verificaparametro(PARAM_CF_UTILIZAPASIVA_TESORERIA_A2, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroCF, PARAM_CF_UTILIZAPASIVA_TESORERIA_A2)
                '************************************************************************************************************************************
                'mdcProxyUtilidad01 = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext()
                'mdcProxyUtilidad01.Verificaparametro(PARAM_STR_CALCULOS_FINANCIEROS, AddressOf TerminotraerparametroCF, Nothing)


                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)



            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de Companias que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of Companias)
    Public Property ListaEncabezado() As EntitySet(Of Companias)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of Companias))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Companias para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezado) Then
                If IsNothing(_ListaEncabezadoPaginada) Then
                    Dim view = New PagedCollectionView(_ListaEncabezado)
                    _ListaEncabezadoPaginada = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Elemento de la lista de Companias que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As Companias
    Public Property EncabezadoSeleccionado() As Companias
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As Companias)

            Dim logIncializarDet As Boolean = False

            _EncabezadoSeleccionado = value

            'If _EncabezadoSeleccionado.strParticipacion <> "" Then
            '    mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
            'Else
            '    mobjTipoParticipacion = Nothing
            'End If


            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intIDCompania > 0 Then
                    ConsultarDetalle(_EncabezadoSeleccionado.intIDCompania)
                    ConsultarDetallePenalidad(_EncabezadoSeleccionado.intIDCompania)
                    ConsultarDetalleTesoreria(_EncabezadoSeleccionado.intIDCompania)
                    ConsultarDetalleLimite(_EncabezadoSeleccionado.intIDCompania)
                    ConsultarDetalleAutorizaciones(_EncabezadoSeleccionado.intIDCompania)
                    ConsultarDetalleCondicionesTesoreria(_EncabezadoSeleccionado.intIDCompania)
                    ConsultarDetalleParametrosCompania(_EncabezadoSeleccionado.intIDCompania)
                    ConsultarDetalleAcumuladoComisiones(_EncabezadoSeleccionado.intIDCompania)

                    'JDCP20170124
                    If _EncabezadoSeleccionado.strTipoCompania = TIPOCOMPANIA_TESORERIA Then
                        MostrarControlesTipoCompania = Visibility.Collapsed
                    Else
                        MostrarControlesTipoCompania = Visibility.Visible
                    End If

                    'If _EncabezadoSeleccionado.strParticipacion <> PARTICIPACION_CONSOLIDACLASES Then
                    '    HabilitarTabTesoreria = Visibility.Visible
                    'Else
                    '    HabilitarTabTesoreria = Visibility.Collapsed
                    'End If
                Else
                    logIncializarDet = True
                End If
                'InicializarDetalle()
                InicializarEncabezado()
            End If

            If logIncializarDet Then
                ListaDetalle = Nothing
                ListaDetalleTesoreria = Nothing
                ListaPenalidad = Nothing
                ListaDetalleLimites = Nothing
                ListaDetalleAutorizaciones = Nothing
                ListaDetalleCondicionesTesoreria = Nothing
                ListaDetalleParametrosCompania = Nothing

            End If

            MyBase.CambioItem("EncabezadoSeleccionado")
            MyBase.CambioItem("MostrarComboTipoTabla")
        End Set
    End Property

    ''' <summary>
    ''' Eventos del property changed del encabezado seleccionado.
    ''' </summary>

    Private Async Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try
            If Editando Then


                Select Case e.PropertyName
                    Case "strNumeroDocumento"
                        _mlogBuscarClienteEncabezado = True
                        _mLogBuscarClienteDetalle = False

                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strNumeroDocumento) Then
                            ObtenerCodigoOyDCliente(_EncabezadoSeleccionado.strNumeroDocumento, False)
                            ' buscarComitente(_EncabezadoSeleccionado.strNumeroDocumento)

                            'buscarComitente(_strCodigoOyD)
                        End If
                    Case "strCompaniaContable"
                        'JEPM20150728 
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strCompaniaContable) Then
                            VerificarExistenciaCompaniaEnCuenta(_EncabezadoSeleccionado.strCompaniaContable)
                        End If
                    Case "logManejaValorUnidad"

                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Si se selecciona el check de valor unidad deben aparecer los tab dependientes en caso contrario solo se muestra el tab de Detalles
                        '-- se asigna el foco al tab Detalle o al tab Estrucura
                        'ID caso de prueba:  CP018
                        '------------------------------------------------------------------------------------------------------------------------------------------------



                        If _EncabezadoSeleccionado.logManejaValorUnidad = False Then
                            HabilitarTabEstructura = Visibility.Collapsed
                            HabilitarTabComisiones = Visibility.Collapsed
                            HabilitarTabTesoreria = Visibility.Collapsed
                            HabilitarTabClientesAutorizados = Visibility.Collapsed
                            HabilitarTabLimites = Visibility.Collapsed
                            HabilitarTabDetalle = Visibility.Visible
                            FocoTabDetalle = True


                        ElseIf _EncabezadoSeleccionado.logManejaValorUnidad = True Then
                            HabilitarTabEstructura = Visibility.Visible
                            HabilitarTabComisiones = Visibility.Visible
                            'If _EncabezadoSeleccionado.strParticipacion <> PARTICIPACION_CONSOLIDACLASES Then
                            HabilitarTabTesoreria = Visibility.Visible
                            'Else
                            '    HabilitarTabTesoreria = Visibility.Collapsed
                            'End If
                            VerificarHabilitacionCuentasOmnibus()
                            HabilitarTabLimites = Visibility.Visible
                            HabilitarTabDetalle = Visibility.Collapsed
                            FocoTabEstructura = True
                        Else
                            HabilitarTabEstructura = Visibility.Collapsed
                            HabilitarTabComisiones = Visibility.Collapsed
                            HabilitarTabTesoreria = Visibility.Collapsed
                            HabilitarTabClientesAutorizados = Visibility.Collapsed
                            HabilitarTabLimites = Visibility.Collapsed
                            HabilitarTabDetalle = Visibility.Visible
                            FocoTabDetalle = True

                        End If


                        If logParametrosPorDefecto = True Then
                            Await ConsultarDetalleParametrosCompania(0)
                            Await ConsultarDetalleCondicionesOperativasPorDefecto(0)
                        End If

                        'MyBase.CambioItem("HabilitarTabEstructura")
                    Case "strIdentificadorCuenta"
                        If _EncabezadoSeleccionado.strIdentificadorCuenta = "O" Then
                            MostrarOtroPrefijo = Visibility.Visible
                        Else
                            MostrarOtroPrefijo = Visibility.Collapsed
                        End If
                        'MyBase.CambioItem("MostrarOtroPrefijo")

                    Case "strTipoPlazo"


                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Se valida que debe suceder cuando se escojan cada uno de de los valores para Tipo de plazo
                        '-- 
                        'ID caso de prueba:  CP019, CP026, CP029
                        '------------------------------------------------------------------------------------------------------------------------------------------------


                        If _EncabezadoSeleccionado.strTipoPlazo = "A" Then
                            MostrarPactoPermanencia = Visibility.Visible
                            MostrarFCP = Visibility.Collapsed
                            MostrarInscritoRNVE = Visibility.Collapsed
                            MostrarFechaVencimiento = Visibility.Collapsed
                            MostrarTipoParticipacion = Visibility.Visible
                            MostrarCompartimentos = Visibility.Collapsed
                            MostrarISIN = Visibility.Collapsed
                            MostrarCamposGrid = Visibility.Visible


                            If _EncabezadoSeleccionado.strPactoPermanencia = "S" Then
                                MostrarDatosPermanencia = Visibility.Visible

                                If _EncabezadoSeleccionado.strPenalidad = "F" Then
                                    MostrarCamposPenalidadFactor = Visibility.Visible
                                    MostrarCamposPenalidadTabla = Visibility.Collapsed
                                ElseIf _EncabezadoSeleccionado.strPenalidad = "T" Then
                                    MostrarCamposPenalidadFactor = Visibility.Collapsed
                                    MostrarCamposPenalidadTabla = Visibility.Visible
                                Else
                                    MostrarCamposPenalidadFactor = Visibility.Collapsed
                                    MostrarCamposPenalidadTabla = Visibility.Collapsed
                                End If

                            Else
                                MostrarDatosPermanencia = Visibility.Collapsed
                                MostrarCamposPenalidadFactor = Visibility.Collapsed
                                MostrarCamposPenalidadTabla = Visibility.Collapsed
                            End If



                        ElseIf _EncabezadoSeleccionado.strTipoPlazo = "C" Then
                            MostrarFCP = Visibility.Visible
                            MostrarInscritoRNVE = Visibility.Visible
                            MostrarPactoPermanencia = Visibility.Collapsed
                            MostrarFechaVencimiento = Visibility.Visible
                            MostrarTipoParticipacion = Visibility.Collapsed
                            MostrarDatosPermanencia = Visibility.Collapsed
                            MostrarCamposPenalidadFactor = Visibility.Collapsed
                            MostrarCamposPenalidadTabla = Visibility.Collapsed
                            MostrarCamposGrid = Visibility.Collapsed

                            If _EncabezadoSeleccionado.strFondoCapitalPrivado = "S" Then
                                MostrarCompartimentos = Visibility.Visible
                            Else
                                MostrarCompartimentos = Visibility.Collapsed
                            End If

                            If _EncabezadoSeleccionado.strInscritoRNVE = "S" Then
                                MostrarISIN = Visibility.Visible
                            Else
                                MostrarISIN = Visibility.Collapsed
                            End If



                        ElseIf _EncabezadoSeleccionado.strTipoPlazo = "CF" Then
                            MostrarPactoPermanencia = Visibility.Collapsed
                            MostrarFCP = Visibility.Collapsed
                            MostrarInscritoRNVE = Visibility.Visible
                            MostrarFechaVencimiento = Visibility.Collapsed
                            MostrarDatosPermanencia = Visibility.Visible
                            MostrarTipoParticipacion = Visibility.Visible
                            MostrarISIN = Visibility.Collapsed
                            MostrarCompartimentos = Visibility.Collapsed
                            MostrarCamposGrid = Visibility.Collapsed

                            If _EncabezadoSeleccionado.strPenalidad = "F" Then
                                MostrarCamposPenalidadFactor = Visibility.Visible
                                MostrarCamposPenalidadTabla = Visibility.Collapsed
                            ElseIf _EncabezadoSeleccionado.strPenalidad = "T" Then
                                MostrarCamposPenalidadFactor = Visibility.Collapsed
                                MostrarCamposPenalidadTabla = Visibility.Visible
                            Else
                                MostrarCamposPenalidadFactor = Visibility.Collapsed
                                MostrarCamposPenalidadTabla = Visibility.Collapsed
                            End If



                        End If
                    Case "strPactoPermanencia"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Se valida que debe suceder cuando se escoja el pacto de permanencia
                        'ID caso de prueba:  CP020
                        '------------------------------------------------------------------------------------------------------------------------------------------------

                        If _EncabezadoSeleccionado.strPactoPermanencia = "S" Then
                            MostrarDatosPermanencia = Visibility.Visible


                            If _EncabezadoSeleccionado.strPenalidad = "F" Then
                                MostrarCamposPenalidadFactor = Visibility.Visible
                                MostrarCamposPenalidadTabla = Visibility.Collapsed
                            ElseIf _EncabezadoSeleccionado.strPenalidad = "T" Then
                                MostrarCamposPenalidadFactor = Visibility.Collapsed
                                MostrarCamposPenalidadTabla = Visibility.Visible
                            Else
                                MostrarCamposPenalidadFactor = Visibility.Collapsed
                                MostrarCamposPenalidadTabla = Visibility.Collapsed
                            End If


                        Else
                            MostrarDatosPermanencia = Visibility.Collapsed
                            MostrarCamposPenalidadFactor = Visibility.Collapsed
                            MostrarCamposPenalidadTabla = Visibility.Collapsed
                        End If

                    Case "strPenalidad"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Se valida que debe suceder cuando se elija algún valor para el pacto de permanencia
                        'ID caso de prueba:  CP021
                        '------------------------------------------------------------------------------------------------------------------------------------------------


                        If _EncabezadoSeleccionado.strPenalidad = "F" Then
                            MostrarCamposPenalidadFactor = Visibility.Visible
                            MostrarCamposPenalidadTabla = Visibility.Collapsed
                        ElseIf _EncabezadoSeleccionado.strPenalidad = "T" Then
                            MostrarCamposPenalidadFactor = Visibility.Collapsed
                            MostrarCamposPenalidadTabla = Visibility.Visible
                        Else
                            MostrarCamposPenalidadFactor = Visibility.Collapsed
                            MostrarCamposPenalidadTabla = Visibility.Collapsed
                        End If
                    Case "strFondoCapitalPrivado"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Se valida que debe suceder cuando se escoja un valor para el Fondo capital Privado 
                        '-- 
                        'ID caso de prueba:  CP027
                        '------------------------------------------------------------------------------------------------------------------------------------------------

                        If _EncabezadoSeleccionado.strFondoCapitalPrivado = "S" Then
                            MostrarCompartimentos = Visibility.Visible
                        Else
                            MostrarCompartimentos = Visibility.Collapsed
                        End If
                    Case "strInscritoRNVE"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Se valida que debe suceder cuando se escoja un valor en el combo Inscrito registro nacional de valores y emisores
                        '-- 
                        'ID caso de prueba:  CP028
                        '------------------------------------------------------------------------------------------------------------------------------------------------

                        If _EncabezadoSeleccionado.strInscritoRNVE = "S" Then
                            MostrarISIN = Visibility.Visible
                        Else
                            MostrarISIN = Visibility.Collapsed
                        End If
                    Case "strComisionAdministracion"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Valida que dependiendo de la comisión administración se habilite o se deshabilite el Tipo comisón
                        'ID caso de prueba:  CP032
                        '------------------------------------------------------------------------------------------------------------------------------------------------

                        If _EncabezadoSeleccionado.strComisionAdministracion = "S" Then
                            MostrarTipoComision = Visibility.Visible
                        Else
                            MostrarTipoComision = Visibility.Collapsed
                            '_EncabezadoSeleccionado.strTipoComision = Nothing
                        End If

                        If MostrarTipoComision = Visibility.Visible And _EncabezadoSeleccionado.strTipoCompania = "APT" Then
                            MostrarIvaComisionAdmon = Visibility.Visible
                        Else
                            MostrarIvaComisionAdmon = Visibility.Collapsed
                        End If

                        If MostrarTipoComision = Visibility.Visible Then
                            If _EncabezadoSeleccionado.strTipoComision = "TE" Or _EncabezadoSeleccionado.strTipoComision = "TN" Then
                                MostrarTasaComision = Visibility.Visible
                                MostrarValorComision = Visibility.Collapsed
                                MostrarTablaAcumuladoComision = Visibility.Collapsed
                                MostrarGridAcumuladoComisiones = Visibility.Collapsed
                                MostrarDatosComisionAdmon = Visibility.Visible
                            ElseIf _EncabezadoSeleccionado.strTipoComision = "V" Then
                                MostrarTasaComision = Visibility.Collapsed
                                MostrarValorComision = Visibility.Visible
                                MostrarTablaAcumuladoComision = Visibility.Collapsed
                                MostrarGridAcumuladoComisiones = Visibility.Collapsed
                                MostrarDatosComisionAdmon = Visibility.Visible
                            ElseIf _EncabezadoSeleccionado.strTipoComision = "TR" Then
                                MostrarValorComision = Visibility.Collapsed
                                MostrarTasaComision = Visibility.Collapsed
                                MostrarTablaAcumuladoComision = Visibility.Visible
                                If _EncabezadoSeleccionado.strTipoAcumuladorComision = "D" Or _EncabezadoSeleccionado.strTipoAcumuladorComision = "A" Or _EncabezadoSeleccionado.strTipoAcumuladorComision = "AD" Then
                                    MostrarGridAcumuladoComisiones = Visibility.Visible
                                Else
                                    MostrarGridAcumuladoComisiones = Visibility.Collapsed
                                End If
                                MostrarDatosComisionAdmon = Visibility.Visible
                            End If
                        Else
                            MostrarValorComision = Visibility.Collapsed
                            MostrarTasaComision = Visibility.Collapsed
                            MostrarTablaAcumuladoComision = Visibility.Collapsed
                            MostrarGridAcumuladoComisiones = Visibility.Collapsed
                            MostrarDatosComisionAdmon = Visibility.Collapsed
                        End If



                    Case "strTipoComision"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Valida que dependiendo del Tipo de Comisón se muestren o se oculten los campos Tasa Comisión, valor comisión y Periodo
                        'ID caso de prueba:  CP033
                        '------------------------------------------------------------------------------------------------------------------------------------------------


                        If _EncabezadoSeleccionado.strTipoComision = "TE" Or _EncabezadoSeleccionado.strTipoComision = "TN" Then
                            MostrarTasaComision = Visibility.Visible
                            MostrarValorComision = Visibility.Collapsed
                            MostrarTablaAcumuladoComision = Visibility.Collapsed
                            MostrarGridAcumuladoComisiones = Visibility.Collapsed
                        ElseIf _EncabezadoSeleccionado.strTipoComision = "V" Then
                            MostrarTasaComision = Visibility.Collapsed
                            MostrarValorComision = Visibility.Visible
                            MostrarGridAcumuladoComisiones = Visibility.Collapsed
                            MostrarTablaAcumuladoComision = Visibility.Collapsed
                        ElseIf _EncabezadoSeleccionado.strTipoComision = "TR" Then
                            MostrarTablaAcumuladoComision = Visibility.Visible
                            MostrarTasaComision = Visibility.Collapsed
                            MostrarValorComision = Visibility.Collapsed
                            If _EncabezadoSeleccionado.strTipoAcumuladorComision = "D" Or _EncabezadoSeleccionado.strTipoAcumuladorComision = "A" Or _EncabezadoSeleccionado.strTipoAcumuladorComision = "AD" Then
                                MostrarGridAcumuladoComisiones = Visibility.Visible
                            Else
                                MostrarGridAcumuladoComisiones = Visibility.Collapsed
                            End If
                        Else
                            MostrarValorComision = Visibility.Collapsed
                            MostrarTasaComision = Visibility.Collapsed
                            MostrarTablaAcumuladoComision = Visibility.Collapsed
                            MostrarGridAcumuladoComisiones = Visibility.Collapsed
                        End If

                        MostrarDatosComisionAdmon = Visibility.Visible
                    Case "strOtrasComisiones"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Valida que dependiendo del Tipo de Comisón se muestren o se oculten los campos De Exito, Entrada y Salida
                        'ID caso de prueba:  CP034
                        '------------------------------------------------------------------------------------------------------------------------------------------------


                        If _EncabezadoSeleccionado.strOtrasComisiones = "S" Then
                            MostrarOpcionesOtrasComisiones = Visibility.Visible
                        Else
                            MostrarOpcionesOtrasComisiones = Visibility.Collapsed
                        End If

                        If MostrarOpcionesOtrasComisiones = Visibility.Visible Then
                            If _EncabezadoSeleccionado.strEntrada = "F" Then
                                MostrarFactorEntrada = Visibility.Visible
                                MostrarValorEntrada = Visibility.Collapsed
                            ElseIf _EncabezadoSeleccionado.strEntrada = "V" Then
                                MostrarFactorEntrada = Visibility.Collapsed
                                MostrarValorEntrada = Visibility.Visible
                            Else
                                MostrarFactorEntrada = Visibility.Collapsed
                                MostrarValorEntrada = Visibility.Collapsed
                            End If


                            If _EncabezadoSeleccionado.strSalida = "F" Then
                                MostrarFactorSalida = Visibility.Visible
                                MostrarValorSalida = Visibility.Collapsed
                            ElseIf _EncabezadoSeleccionado.strSalida = "V" Then
                                MostrarFactorSalida = Visibility.Collapsed
                                MostrarValorSalida = Visibility.Visible
                            Else
                                MostrarFactorSalida = Visibility.Collapsed
                                MostrarValorSalida = Visibility.Collapsed
                            End If
                        Else
                            MostrarFactorEntrada = Visibility.Collapsed
                            MostrarValorEntrada = Visibility.Collapsed
                            MostrarFactorSalida = Visibility.Collapsed
                            MostrarValorSalida = Visibility.Collapsed
                        End If

                    Case "strEntrada"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Valida que dependiendo del combo Entrada se muestren o se oculten los campos Factor y Valor
                        'ID caso de prueba:  CP035
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        If _EncabezadoSeleccionado.strEntrada = "F" Then
                            MostrarFactorEntrada = Visibility.Visible
                            MostrarValorEntrada = Visibility.Collapsed
                        ElseIf _EncabezadoSeleccionado.strEntrada = "V" Then
                            MostrarFactorEntrada = Visibility.Collapsed
                            MostrarValorEntrada = Visibility.Visible
                        Else
                            MostrarFactorEntrada = Visibility.Collapsed
                            MostrarValorEntrada = Visibility.Collapsed
                        End If
                    Case "strSalida"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Valida que dependiendo del combo Salida se muestren o se oculten los campos Factor y Valor
                        'ID caso de prueba:  CP036
                        '------------------------------------------------------------------------------------------------------------------------------------------------

                        If _EncabezadoSeleccionado.strSalida = "F" Then
                            MostrarFactorSalida = Visibility.Visible
                            MostrarValorSalida = Visibility.Collapsed
                        ElseIf _EncabezadoSeleccionado.strSalida = "V" Then
                            MostrarFactorSalida = Visibility.Collapsed
                            MostrarValorSalida = Visibility.Visible
                        Else
                            MostrarFactorSalida = Visibility.Collapsed
                            MostrarValorSalida = Visibility.Collapsed
                        End If
                    Case "strTipoCompania"

                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Valida que dependiendo del Tipo de compañia que se seleccione se habilite el check de maneja valor unidad,
                        '-- de igual manera se ocultan ocultan o se muestran los tab incluidos los campos principales de cada tab
                        'ID caso de prueba:  CP017
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        'JDCP20170124
                        If _EncabezadoSeleccionado.strTipoCompania = TIPOCOMPANIA_TESORERIA Then
                            MostrarControlesTipoCompania = Visibility.Collapsed
                            _EncabezadoSeleccionado.strCuentaOmnibus = "NO"
                        Else
                            MostrarControlesTipoCompania = Visibility.Visible

                            If _EncabezadoSeleccionado.strTipoCompania = "PP" Then
                                MostrarManejaValorUnidad = Visibility.Collapsed
                                MostrarManejaOmnibus = Visibility.Collapsed
                                HabilitarTabDetalle = Visibility.Visible
                                HabilitarTabEstructura = Visibility.Collapsed
                                HabilitarTabComisiones = Visibility.Collapsed
                                HabilitarTabLimites = Visibility.Collapsed
                                HabilitarTabTesoreria = Visibility.Collapsed
                                HabilitarTabClientesAutorizados = Visibility.Collapsed
                                MostrarCamposPrincipales = Visibility.Collapsed
                                FocoTabDetalle = True

                                _EncabezadoSeleccionado.strCuentaOmnibus = "NO"
                                ValidarCodigosCompaniaTipoCompania()

                            ElseIf _EncabezadoSeleccionado.strTipoCompania = Nothing Then
                                MostrarManejaValorUnidad = Visibility.Collapsed
                                MostrarManejaOmnibus = Visibility.Collapsed
                                MostrarCamposPrincipales = Visibility.Collapsed
                            Else

                                If TieneParametro = True Then
                                    MostrarManejaValorUnidad = Visibility.Visible
                                    MostrarManejaOmnibus = Visibility.Visible
                                    MostrarCamposPrincipales = Visibility.Visible
                                End If

                                If _EncabezadoSeleccionado.logManejaValorUnidad = False Then
                                    HabilitarTabEstructura = Visibility.Collapsed
                                    HabilitarTabComisiones = Visibility.Collapsed
                                    HabilitarTabTesoreria = Visibility.Collapsed
                                    HabilitarTabClientesAutorizados = Visibility.Collapsed
                                    HabilitarTabLimites = Visibility.Collapsed
                                    HabilitarTabDetalle = Visibility.Visible
                                    FocoTabDetalle = True


                                ElseIf _EncabezadoSeleccionado.logManejaValorUnidad = True Then
                                    HabilitarTabEstructura = Visibility.Visible
                                    HabilitarTabComisiones = Visibility.Visible
                                    'If _EncabezadoSeleccionado.strParticipacion <> PARTICIPACION_CONSOLIDACLASES Then
                                    HabilitarTabTesoreria = Visibility.Visible
                                    'Else
                                    '    HabilitarTabTesoreria = Visibility.Collapsed
                                    'End If
                                    VerificarHabilitacionCuentasOmnibus()
                                    HabilitarTabLimites = Visibility.Visible
                                    HabilitarTabDetalle = Visibility.Collapsed
                                    FocoTabEstructura = True
                                    If _EncabezadoSeleccionado.strTipoPlazo = "A" Then
                                        MostrarCamposGrid = Visibility.Visible
                                    Else
                                        MostrarCamposGrid = Visibility.Collapsed
                                    End If

                                    If MostrarTipoComision = Visibility.Visible And _EncabezadoSeleccionado.strTipoCompania = "APT" Then
                                        MostrarIvaComisionAdmon = Visibility.Visible
                                    Else
                                        MostrarIvaComisionAdmon = Visibility.Collapsed
                                    End If

                                    If MostrarTipoComisionGestor = Visibility.Visible And _EncabezadoSeleccionado.strTipoCompania = "FIC" Then
                                        MostrarIvaComisionGestor = Visibility.Visible
                                    Else
                                        MostrarIvaComisionGestor = Visibility.Collapsed
                                    End If

                                Else
                                    _EncabezadoSeleccionado.logManejaValorUnidad = False
                                    HabilitarTabEstructura = Visibility.Collapsed
                                    HabilitarTabComisiones = Visibility.Collapsed
                                    HabilitarTabTesoreria = Visibility.Collapsed
                                    HabilitarTabClientesAutorizados = Visibility.Collapsed
                                    HabilitarTabLimites = Visibility.Collapsed
                                    HabilitarTabDetalle = Visibility.Visible
                                    FocoTabDetalle = True
                                End If

                            End If
                            'CP060: Mostrar el cliente partidas cuando la compañias sea FIC
                            If _EncabezadoSeleccionado.strTipoCompania = "FIC" Then
                                MostrarClientePartidas = Visibility.Visible
                            Else
                                MostrarClientePartidas = Visibility.Collapsed
                            End If
                        End If


                    Case "strParticipacion"

                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Valida que dependiendo del Tipo de Participación se muestre el buscador seleccionado, las opciones son:No Particionado (Solo buscadores OyD), con clases
                        '--(Solo buscador companias), Estrategia General (Los dos buscadores), Estrategia individual (Ninguno)
                        'ID caso de prueba:  CP030
                        '------------------------------------------------------------------------------------------------------------------------------------------------



                        'If NoValidarCambio = False Then
                        '    mobjCambioParticipacion = False
                        '    ValidarCambioParticipacion(_EncabezadoSeleccionado.strParticipacion, "1")
                        'Else
                        '    NoValidarCambio = False
                        'End If

                        If _EncabezadoSeleccionado.strParticipacion = "C" Or _EncabezadoSeleccionado.strParticipacion = "I" Or _EncabezadoSeleccionado.strParticipacion = "CC" Then
                            MostrarBuscadorOyd = Visibility.Visible
                            MostrarBuscadorCompanias = Visibility.Collapsed
                        ElseIf _EncabezadoSeleccionado.strParticipacion = "CI" Then
                            MostrarBuscadorOyd = Visibility.Collapsed
                            MostrarBuscadorCompanias = Visibility.Visible
                            ListaDetalle = New List(Of CFCalculosFinancieros.CompaniasCodigos)
                        End If

                        'If _EncabezadoSeleccionado.strParticipacion <> PARTICIPACION_CONSOLIDACLASES Then
                        '    HabilitarTabTesoreria = Visibility.Visible
                        'Else
                        '    HabilitarTabTesoreria = Visibility.Collapsed
                        '    ListaDetalleTesoreria = Nothing
                        '    DetalleTesoreriaSeleccionado = Nothing
                        'End If

                        If mobjCambioParticipacion = True Then
                            mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                            mobjCambioParticipacion = False
                        End If

                        ''JCM20160125()
                        If _EncabezadoSeleccionado.strParticipacion = "CC" Then
                            strTipoAgrupacion = "todoslosreceptores&**&" & _EncabezadoSeleccionado.strNumeroDocumento
                            If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strNumeroDocumento) And IsNothing(_EncabezadoSeleccionado.dtmUltimoCierre) Then
                                ValidarCodigosOyDExistentes(_EncabezadoSeleccionado.strNumeroDocumento)
                            End If
                        Else
                            strTipoAgrupacion = "todoslosreceptores"
                        End If
                    Case "strTipoAutorizados"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Valida que dependiendo del Tipo Autorizados, se oculte el Grid de Autorizados o se muestre el Grid
                        'ID caso de prueba:  CP041
                        '------------------------------------------------------------------------------------------------------------------------------------------------


                        If _EncabezadoSeleccionado.strTipoAutorizados = "T" Then
                            MostrarGridAutorizaciones = Visibility.Collapsed
                        ElseIf _EncabezadoSeleccionado.strTipoAutorizados = "C" Then
                            MostrarGridAutorizaciones = Visibility.Visible
                        Else
                            MostrarBuscadorCompanias = Visibility.Collapsed
                        End If

                    Case "strNumeroDocumentoClientePartidas"
                        If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strNumeroDocumentoClientePartidas) Then
                            ObtenerCodigoOyDClientePartidas(_EncabezadoSeleccionado.strNumeroDocumentoClientePartidas)
                            'buscarComitentePartidas(_EncabezadoSeleccionado.strNumeroDocumentoClientePartidas)
                        End If


                    Case "strComisionGestor"
                        If _EncabezadoSeleccionado.strComisionGestor = "S" Then
                            MostrarTipoComisionGestor = Visibility.Visible
                        Else
                            MostrarTipoComisionGestor = Visibility.Collapsed
                        End If

                        If _EncabezadoSeleccionado.strComisionGestor = "S" And _EncabezadoSeleccionado.strTipoCompania = "FIC" Then
                            MostrarIvaComisionGestor = Visibility.Visible
                        Else
                            MostrarIvaComisionGestor = Visibility.Collapsed
                        End If

                        If MostrarTipoComisionGestor = Visibility.Visible Then
                            If _EncabezadoSeleccionado.strTipoComisionGestor = "TE" Or _EncabezadoSeleccionado.strTipoComisionGestor = "TN" Then
                                MostrarTasaComisionGestor = Visibility.Visible
                                MostrarValorComisionGestor = Visibility.Collapsed
                            ElseIf _EncabezadoSeleccionado.strTipoComisionGestor = "V" Then
                                MostrarTasaComisionGestor = Visibility.Collapsed
                                MostrarValorComisionGestor = Visibility.Visible
                            Else
                                MostrarValorComisionGestor = Visibility.Collapsed
                                MostrarTasaComisionGestor = Visibility.Collapsed
                            End If
                        Else
                            MostrarValorComisionGestor = Visibility.Collapsed
                            MostrarTasaComisionGestor = Visibility.Collapsed
                        End If


                    Case "strTipoComisionGestor"
                        '------------------------------------------------------------------------------------------------------------------------------------------------
                        '-- Valida que dependiendo del Tipo de Comisón se muestren o se oculten los campos Tasa Comisión, valor comisión y Periodo
                        'ID caso de prueba:  CP033
                        '------------------------------------------------------------------------------------------------------------------------------------------------


                        If _EncabezadoSeleccionado.strTipoComisionGestor = "TE" Or _EncabezadoSeleccionado.strTipoComisionGestor = "TN" Then
                            MostrarTasaComisionGestor = Visibility.Visible
                            MostrarValorComisionGestor = Visibility.Collapsed
                        ElseIf _EncabezadoSeleccionado.strTipoComisionGestor = "V" Then
                            MostrarTasaComisionGestor = Visibility.Collapsed
                            MostrarValorComisionGestor = Visibility.Visible
                        Else
                            MostrarValorComisionGestor = Visibility.Collapsed
                            MostrarTasaComisionGestor = Visibility.Collapsed
                        End If


                    Case "strComisionMinima"
                        If _EncabezadoSeleccionado.strComisionMinima = "S" Then
                            MostrarManejaComisionMin = Visibility.Visible
                        Else
                            MostrarManejaComisionMin = Visibility.Collapsed
                        End If

                    Case "strTipoAcumuladorComision"
                        If _EncabezadoSeleccionado.strTipoAcumuladorComision = "D" Or _EncabezadoSeleccionado.strTipoAcumuladorComision = "A" Or _EncabezadoSeleccionado.strTipoAcumuladorComision = "AD" Then
                            MostrarGridAcumuladoComisiones = Visibility.Visible
                        Else
                            MostrarGridAcumuladoComisiones = Visibility.Collapsed
                        End If

                    Case "strFondoRenovable"
                        If MostrarDatosPermanencia = Visibility.Visible And __EncabezadoSeleccionado.strFondoRenovable <> "N" Then
                            MostrarPeriodoGracia = Visibility.Visible
                        Else
                            MostrarPeriodoGracia = Visibility.Collapsed
                        End If

                    Case "strCuentaOmnibus"
                        VerificarHabilitacionCuentasOmnibus()

                    Case Else

                End Select

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_CustodiSelected.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub VerificarHabilitacionCuentasOmnibus()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strCuentaOmnibus) And _EncabezadoSeleccionado.strCuentaOmnibus <> "NO" Then
                    HabilitarCamposOmnibus = True
                    HabilitarTabClientesAutorizados = Visibility.Collapsed
                Else
                    HabilitarCamposOmnibus = False
                    If _EncabezadoSeleccionado.logManejaValorUnidad Then
                        HabilitarTabClientesAutorizados = Visibility.Visible
                    Else
                        HabilitarTabClientesAutorizados = Visibility.Collapsed
                    End If
                End If
            Else
                HabilitarCamposOmnibus = False
                HabilitarTabClientesAutorizados = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar o deshabilitar campos omnibus.",
                                 Me.ToString(), "VerificarHabilitacionCuentasOmnibus", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private _strTipoTabla As String = Nothing
    Public Property strTipoTabla() As String
        Get
            Return _strTipoTabla
        End Get
        Set(ByVal value As String)
            _strTipoTabla = value
            MyBase.CambioItem("strTipoTabla")
            MyBase.CambioItem("ListaAcumulacionComisiones")
            MyBase.CambioItem("ListaDetallePaginadaListaAcumulacionComisiones")
        End Set
    End Property


#Region "Métodos para Detalles"

    ''' <summary>
    ''' Lista de detalles de la compañía en este caso los códigos de clientes relacionados a la compañía
    ''' </summary>
    Private _ListaDetalle As List(Of CompaniasCodigos)
    Public Property ListaDetalle() As List(Of CompaniasCodigos)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of CompaniasCodigos))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    ''' <summary>
    ''' Pagina la lista detalles. Se presenta en el grid del detalle 
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalle) Then
                Dim view = New PagedCollectionView(_ListaDetalle)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As CompaniasCodigos
    Public Property DetalleSeleccionado() As CompaniasCodigos
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As CompaniasCodigos)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    'Private _ListaDetalleTesoreria As ObservableCollection(Of DetalleTesoreria)
    'Public Property ListaDetalleTesoreria() As ObservableCollection(Of DetalleTesoreria)
    '    Get
    '        Return _ListaDetalleTesoreria
    '    End Get
    '    Set(ByVal value As ObservableCollection(Of DetalleTesoreria))
    '        _ListaDetalleTesoreria = value
    '        MyBase.CambioItem("ListaDetalleTesoreria")
    '    End Set
    'End Property


    ''' <summary>
    ''' Lista de detalles de la Tesoreria en este caso los valores de tesorería para la compañía
    ''' </summary>
    Private _ListaDetalleTesoreria As List(Of CompaniasTesoreria)
    Public Property ListaDetalleTesoreria() As List(Of CompaniasTesoreria)
        Get
            Return _ListaDetalleTesoreria
        End Get
        Set(ByVal value As List(Of CompaniasTesoreria))
            _ListaDetalleTesoreria = value
            MyBase.CambioItem("ListaDetalleTesoreria")
            MyBase.CambioItem("ListaDetallePaginadaTesoreria")
        End Set
    End Property

    ''' <summary>
    ''' Lista de detalle de Penalidades, valores del grid de las penalidades
    ''' </summary>
    Private _ListaPenalidad As List(Of CompaniasPenalidades)
    Public Property ListaPenalidad() As List(Of CompaniasPenalidades)
        Get
            Return _ListaPenalidad
        End Get
        Set(ByVal value As List(Of CompaniasPenalidades))
            _ListaPenalidad = value
            MyBase.CambioItem("ListaPenalidad")
            MyBase.CambioItem("ListaDetallePaginadaPenalidad")
        End Set
    End Property

    ''' <summary>
    ''' Lista de detalle de Limites, para capturar los valores del grid de limites de la compañía.
    ''' </summary>
    Private _ListaDetalleLimites As List(Of CompaniasLimites)
    Public Property ListaDetalleLimites() As List(Of CompaniasLimites)
        Get
            Return _ListaDetalleLimites
        End Get
        Set(ByVal value As List(Of CompaniasLimites))
            _ListaDetalleLimites = value
            MyBase.CambioItem("ListaDetalleLimites")
            MyBase.CambioItem("ListaDetalleLimitesPaginada")
        End Set
    End Property


    ''' <summary>
    ''' Lista de detalle de Acumulacion Comisiones, valores del grid de Acumulacion Comisiones
    ''' </summary>
    Private _ListaAcumulacionComisiones As List(Of CompaniasAcumuladoComisiones)
    Public Property ListaAcumulacionComisiones() As List(Of CompaniasAcumuladoComisiones)
        Get
            Return _ListaAcumulacionComisiones
        End Get
        Set(ByVal value As List(Of CompaniasAcumuladoComisiones))
            _ListaAcumulacionComisiones = value
            MyBase.CambioItem("ListaAcumulacionComisiones")
            MyBase.CambioItem("ListaDetallePaginadaListaAcumulacionComisiones")
        End Set
    End Property

    Private _ListaAcumulacionComisionesPorTipoTabla As List(Of CompaniasAcumuladoComisiones)
    Public Property ListaAcumulacionComisionesPorTipoTabla() As List(Of CompaniasAcumuladoComisiones)
        Get
            Return _ListaAcumulacionComisionesPorTipoTabla
        End Get
        Set(ByVal value As List(Of CompaniasAcumuladoComisiones))
            _ListaAcumulacionComisionesPorTipoTabla = value
            MyBase.CambioItem("ListaAcumulacionComisiones")
            MyBase.CambioItem("ListaAcumulacionComisionesPorTipoTabla")
            MyBase.CambioItem("ListaDetallePaginadaListaAcumulacionComisiones")
        End Set
    End Property



    ''' <summary>
    ''' Lista de detalles de la Tesoreria en este caso los valores de condiciones tesorería para la compañía
    ''' </summary>
    Private _ListaDetalleCondicionesTesoreria As List(Of CompaniasCondicionesTesoreria)
    Public Property ListaDetalleCondicionesTesoreria() As List(Of CompaniasCondicionesTesoreria)
        Get
            Return _ListaDetalleCondicionesTesoreria
        End Get
        Set(ByVal value As List(Of CompaniasCondicionesTesoreria))
            _ListaDetalleCondicionesTesoreria = value
            MyBase.CambioItem("ListaDetalleCondicionesTesoreria")
            MyBase.CambioItem("ListaDetalleCondicionesPaginadaTesoreria")
        End Set
    End Property

    ''' <summary>
    ''' Lista de detalles de los parametros compañia
    ''' </summary>
    Private _ListaParametrosSeleccioando As List(Of ParametrosCompania)
    Public Property ListaParametrosSeleccioando() As List(Of ParametrosCompania)
        Get
            Return _ListaParametrosSeleccioando
        End Get
        Set(ByVal value As List(Of ParametrosCompania))
            _ListaParametrosSeleccioando = value
        End Set
    End Property




    ''' <summary>
    ''' Lista de detalles de los parametros compañia
    ''' </summary>
    Private _ListaDetalleParametrosCompania As List(Of ParametrosCompania)
    Public Property ListaDetalleParametrosCompania() As List(Of ParametrosCompania)
        Get
            Return _ListaDetalleParametrosCompania
        End Get
        Set(ByVal value As List(Of ParametrosCompania))
            _ListaDetalleParametrosCompania = value
            MyBase.CambioItem("ListaDetalleParametrosCompania")
            MyBase.CambioItem("ListaDetalleParametrosPaginadaCompania")
        End Set
    End Property



    ''' <summary>
    ''' Pagina la lista detalle Acumulacion Comisiones. Se presenta en el grid del detalle de Acumulacion Comisiones 
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginadaListaAcumulacionComisiones() As PagedCollectionView
        Get
            If Not IsNothing(_ListaAcumulacionComisionesPorTipoTabla) Then
                Dim view = New PagedCollectionView(_ListaAcumulacionComisionesPorTipoTabla)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    ''' <summary>
    ''' Pagina la lista detalle de tesorería. Se presenta en el grid del detalle de condiciones tesorería 
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginadaTesoreria() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleTesoreria) Then
                Dim view = New PagedCollectionView(_ListaDetalleTesoreria)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property


    ''' <summary>
    ''' Pagina la lista detalle de Condiciones tesorería. Se presenta en el grid del detalle de tesorería 
    ''' </summary>
    Public ReadOnly Property ListaDetalleParametrosPaginadaCompania() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleParametrosCompania) Then
                Dim view = New PagedCollectionView(_ListaDetalleParametrosCompania)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Pagina la lista detalle de parametrosn compañia. Se presenta en el grid del detalle de parametros compañia 
    ''' </summary>
    Public ReadOnly Property ListaDetalleCondicionesPaginadaTesoreria() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleCondicionesTesoreria) Then
                Dim view = New PagedCollectionView(_ListaDetalleCondicionesTesoreria)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property



    ''' <summary>
    ''' Pagina la lista detalle de penalidades. Se presenta en el grid del detalle de penalidades 
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginadaPenalidad() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPenalidad) Then
                Dim view = New PagedCollectionView(_ListaPenalidad)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado en el Grid de Penalidades
    ''' </summary>
    Private WithEvents _DetallePenalidadSeleccionado As CompaniasPenalidades
    Public Property DetallePenalidadSeleccionado() As CompaniasPenalidades
        Get
            Return _DetallePenalidadSeleccionado
        End Get
        Set(ByVal value As CompaniasPenalidades)
            _DetallePenalidadSeleccionado = value
            MyBase.CambioItem("DetallePenalidadSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado en el Grid de Tesorería
    ''' </summary>
    Private WithEvents _DetalleTesoreriaSeleccionado As CompaniasTesoreria
    Public Property DetalleTesoreriaSeleccionado() As CompaniasTesoreria
        Get
            Return _DetalleTesoreriaSeleccionado
        End Get
        Set(ByVal value As CompaniasTesoreria)
            _DetalleTesoreriaSeleccionado = value
            MyBase.CambioItem("DetalleTesoreriaSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado en el Grid de Límites
    ''' </summary>
    Private WithEvents _DetalleLimitesSeleccionado As CompaniasLimites
    Public Property DetalleLimitesSeleccionado() As CompaniasLimites
        Get
            Return _DetalleLimitesSeleccionado
        End Get
        Set(ByVal value As CompaniasLimites)
            _DetalleLimitesSeleccionado = value
            MyBase.CambioItem("DetalleLimitesSeleccionado")
        End Set
    End Property


    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado en el Grid de Tesorería Condiciones
    ''' </summary>
    Private WithEvents _DetalleTesoreriaCondicioneSeleccionado As CompaniasCondicionesTesoreria
    Public Property DetalleTesoreriaCondicioneSeleccionado() As CompaniasCondicionesTesoreria
        Get
            Return _DetalleTesoreriaCondicioneSeleccionado
        End Get
        Set(ByVal value As CompaniasCondicionesTesoreria)
            _DetalleTesoreriaCondicioneSeleccionado = value
            MyBase.CambioItem("DetalleTesoreriaCondicioneSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado en el Grid de Tesorería Condiciones
    ''' </summary>
    Private WithEvents _DetalleParametrosCompaniaSeleccionado As ParametrosCompania
    Public Property DetalleParametrosCompaniaSeleccionado() As ParametrosCompania
        Get
            Return _DetalleParametrosCompaniaSeleccionado
        End Get
        Set(ByVal value As ParametrosCompania)
            _DetalleParametrosCompaniaSeleccionado = value
            MyBase.CambioItem("DetalleParametrosCompaniaSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado en el Grid de Acumulacion Comisiones
    ''' </summary>
    Private WithEvents _DetalleAcumuladoComisionesSeleccionado As CompaniasAcumuladoComisiones
    Public Property DetalleAcumuladoComisionesSeleccionado() As CompaniasAcumuladoComisiones
        Get
            Return _DetalleAcumuladoComisionesSeleccionado
        End Get
        Set(ByVal value As CompaniasAcumuladoComisiones)
            _DetalleAcumuladoComisionesSeleccionado = value
            MyBase.CambioItem("DetalleAcumuladoComisionesSeleccionado")
        End Set
    End Property


    ''' <summary>
    ''' Pagina la lista detalle de limites. Se presenta en el grid del detalle de limites 
    ''' </summary>
    Public ReadOnly Property ListaDetalleLimitesPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleLimites) Then
                Dim view = New PagedCollectionView(_ListaDetalleLimites)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Lista de detalle de autorizaciones, para capturar los valores de las autorizaciones en el Grid
    ''' </summary>
    Private _ListaDetalleAutorizaciones As List(Of CompaniasAutorizaciones)
    Public Property ListaDetalleAutorizaciones() As List(Of CompaniasAutorizaciones)
        Get
            Return _ListaDetalleAutorizaciones
        End Get
        Set(ByVal value As List(Of CompaniasAutorizaciones))
            _ListaDetalleAutorizaciones = value
            MyBase.CambioItem("ListaDetalleAutorizaciones")
            MyBase.CambioItem("ListaDetalleAutorizacionesPaginada")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado en el Grid de autorizaciones
    ''' </summary>
    Private WithEvents _DetalleAutorizacionesSeleccionado As CompaniasAutorizaciones
    Public Property DetalleAutorizacionesSeleccionado() As CompaniasAutorizaciones
        Get
            Return _DetalleAutorizacionesSeleccionado
        End Get
        Set(ByVal value As CompaniasAutorizaciones)
            _DetalleAutorizacionesSeleccionado = value
            MyBase.CambioItem("DetalleAutorizacionesSeleccionado")
        End Set
    End Property
    ''' <summary>
    ''' Pagina la lista detalle de Autorizaciones. Se presenta en el grid del detalle de Autorizaciones 
    ''' </summary>
    Public ReadOnly Property ListaDetalleAutorizacionesPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleAutorizaciones) Then
                Dim view = New PagedCollectionView(_ListaDetalleAutorizaciones)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property



#End Region

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaCompanias
    Public Property cb() As CamposBusquedaCompanias
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaCompanias)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para habilitar o deshabilitar campos del encabezado
    ''' </summary>
    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            MyBase.CambioItem("HabilitarEncabezado")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para habilitar o deshabilitar campos del encabezado que se pueden editar con continuidad
    ''' </summary>
    Private _HabilitarCamposEdicion As Boolean = False
    Public Property HabilitarCamposEdicion() As Boolean
        Get
            Return _HabilitarCamposEdicion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposEdicion = value
            MyBase.CambioItem("HabilitarCamposEdicion")
        End Set
    End Property


    ''' <summary>
    ''' Propiedad para habilitar o deshabilitar el buscador de Gestor
    ''' </summary>
    Private _HabilitarCamposGestor As Boolean = False
    Public Property HabilitarCamposGestor() As Boolean
        Get
            Return _HabilitarCamposGestor
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposGestor = value
            MyBase.CambioItem("HabilitarCamposGestor")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para habilitar o deshabilitar el buscador de monedas
    ''' </summary>
    Private _HabilitarCamposMoneda As Boolean = False
    Public Property HabilitarCamposMoneda() As Boolean
        Get
            Return _HabilitarCamposMoneda
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposMoneda = value
            MyBase.CambioItem("HabilitarCamposMoneda")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para Mostrar u ocultar el tab de Detalle
    ''' </summary>
    Private _HabilitarTabDetalle As Visibility = Visibility.Collapsed
    Public Property HabilitarTabDetalle() As Visibility
        Get
            Return _HabilitarTabDetalle

        End Get
        Set(value As Visibility)
            _HabilitarTabDetalle = value
            MyBase.CambioItem("HabilitarTabDetalle")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para Mostrar u ocultar el tab de estructura
    ''' </summary>
    Private _HabilitarTabEstructura As Visibility = Visibility.Collapsed
    Public Property HabilitarTabEstructura() As Visibility
        Get
            Return _HabilitarTabEstructura
        End Get
        Set(ByVal value As Visibility)
            _HabilitarTabEstructura = value
            MyBase.CambioItem("HabilitarTabEstructura")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para Mostrar u ocultar el tab de comisiones
    ''' </summary>
    Private _HabilitarTabComisiones As Visibility = Visibility.Collapsed
    Public Property HabilitarTabComisiones() As Visibility
        Get
            Return _HabilitarTabComisiones

        End Get
        Set(value As Visibility)
            _HabilitarTabComisiones = value
            MyBase.CambioItem("HabilitarTabComisiones")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para Mostrar u ocultar el tab de Tesorería
    ''' </summary>
    Private _HabilitarTabTesoreria As Visibility = Visibility.Collapsed
    Public Property HabilitarTabTesoreria() As Visibility
        Get
            Return _HabilitarTabTesoreria

        End Get
        Set(value As Visibility)
            _HabilitarTabTesoreria = value
            MyBase.CambioItem("HabilitarTabTesoreria")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para Mostrar u ocultar el tab de clientes autorizados
    ''' </summary>
    Private _HabilitarTabClientesAutorizados As Visibility = Visibility.Collapsed
    Public Property HabilitarTabClientesAutorizados() As Visibility
        Get
            Return _HabilitarTabClientesAutorizados

        End Get
        Set(value As Visibility)
            _HabilitarTabClientesAutorizados = value
            MyBase.CambioItem("HabilitarTabClientesAutorizados")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para Mostrar u ocultar el tab de limites
    ''' </summary>
    Private _HabilitarTabLimites As Visibility = Visibility.Collapsed
    Public Property HabilitarTabLimites() As Visibility
        Get
            Return _HabilitarTabLimites

        End Get
        Set(value As Visibility)
            _HabilitarTabLimites = value
            MyBase.CambioItem("HabilitarTabLimites")
        End Set
    End Property



    ''' <summary>
    ''' Propiedad para Mostrar u ocultar el campo fecha del grid de parametros
    ''' </summary>
    Private _HabilitarFechaParametro As Visibility = Visibility.Collapsed
    Public Property HabilitarFechaParametro() As Visibility
        Get
            Return _HabilitarFechaParametro

        End Get
        Set(value As Visibility)
            _HabilitarFechaParametro = value
            MyBase.CambioItem("HabilitarFechaParametro")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para asignar el foco o no sobre el tab de detalle
    ''' </summary>
    Private _FocoTabDetalle As Boolean = False
    Public Property FocoTabDetalle() As Boolean
        Get
            Return _FocoTabDetalle

        End Get
        Set(value As Boolean)
            _FocoTabDetalle = value
            MyBase.CambioItem("FocoTabDetalle")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para asignar el foco o no sobre el tab de estructura
    ''' </summary>
    Private _FocoTabEstructura As Boolean = False
    Public Property FocoTabEstructura() As Boolean
        Get
            Return _FocoTabEstructura

        End Get
        Set(value As Boolean)
            _FocoTabEstructura = value
            MyBase.CambioItem("FocoTabEstructura")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u oculta la opción de maneja de valor de unidad
    ''' </summary>
    Private _MostrarManejaValorUnidad As Visibility = Visibility.Collapsed
    Public Property MostrarManejaValorUnidad() As Visibility
        Get
            Return _MostrarManejaValorUnidad

        End Get
        Set(value As Visibility)
            _MostrarManejaValorUnidad = value
            MyBase.CambioItem("MostrarManejaValorUnidad")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u oculta la opción de maneja de valor de unidad
    ''' </summary>
    Private _MostrarManejaOmnibus As Visibility = Visibility.Collapsed
    Public Property MostrarManejaOmnibus() As Visibility
        Get
            Return _MostrarManejaOmnibus

        End Get
        Set(value As Visibility)
            _MostrarManejaOmnibus = value
            MyBase.CambioItem("MostrarManejaOmnibus")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u oculta la opción de maneja comision minima
    ''' </summary>
    Private _MostrarManejaComisionMin As Visibility = Visibility.Collapsed
    Public Property MostrarManejaComisionMin() As Visibility
        Get
            Return _MostrarManejaComisionMin

        End Get
        Set(value As Visibility)
            _MostrarManejaComisionMin = value
            MyBase.CambioItem("MostrarManejaComisionMin")
        End Set
    End Property


    ''' <summary>
    ''' Propiedad Mostrar u oculta el textbob de Otro Prefijo
    ''' </summary>
    Private _MostrarOtroPrefijo As Visibility = Visibility.Collapsed
    Public Property MostrarOtroPrefijo() As Visibility
        Get
            Return _MostrarOtroPrefijo
        End Get
        Set(ByVal value As Visibility)
            _MostrarOtroPrefijo = value
            MyBase.CambioItem("MostrarOtroPrefijo")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u ocultar el combo del pacto de permanencia
    ''' </summary>
    Private _MostrarPactoPermanencia As Visibility = Visibility.Collapsed
    Public Property MostrarPactoPermanencia() As Visibility
        Get
            Return _MostrarPactoPermanencia

        End Get
        Set(value As Visibility)
            _MostrarPactoPermanencia = value
            MyBase.CambioItem("MostrarPactoPermanencia")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar los campos dependientes del combo de pacto de permanencia
    ''' </summary>
    Private _MostrarDatosPermanencia As Visibility = Visibility.Collapsed
    Public Property MostrarDatosPermanencia() As Visibility
        Get
            Return _MostrarDatosPermanencia

        End Get
        Set(value As Visibility)
            _MostrarDatosPermanencia = value
            MyBase.CambioItem("MostrarDatosPermanencia")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar los campos dependientes del factor de penalidad
    ''' </summary>
    Private _MostrarCamposPenalidadFactor As Visibility = Visibility.Collapsed
    Public Property MostrarCamposPenalidadFactor() As Visibility
        Get
            Return _MostrarCamposPenalidadFactor

        End Get
        Set(value As Visibility)
            _MostrarCamposPenalidadFactor = value
            MyBase.CambioItem("MostrarCamposPenalidadFactor")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u ocultar la tabla en la que se visualiza el grid de penalidades
    ''' </summary>
    Private _MostrarCamposPenalidadTabla As Visibility = Visibility.Collapsed
    Public Property MostrarCamposPenalidadTabla() As Visibility
        Get
            Return _MostrarCamposPenalidadTabla

        End Get
        Set(value As Visibility)
            _MostrarCamposPenalidadTabla = value
            MyBase.CambioItem("MostrarCamposPenalidadTabla")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u ocultar el combo del fondo del capital privado
    ''' </summary>
    Private _MostrarFCP As Visibility = Visibility.Collapsed
    Public Property MostrarFCP() As Visibility
        Get
            Return _MostrarFCP

        End Get
        Set(value As Visibility)
            _MostrarFCP = value
            MyBase.CambioItem("MostrarFCP")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u ocultar el combo del InscritoRNVE
    ''' </summary>
    Private _MostrarInscritoRNVE As Visibility = Visibility.Collapsed
    Public Property MostrarInscritoRNVE() As Visibility
        Get
            Return _MostrarInscritoRNVE

        End Get
        Set(value As Visibility)
            _MostrarInscritoRNVE = value
            MyBase.CambioItem("MostrarInscritoRNVE")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u ocultar la fecha de vencimiento del fondo
    ''' </summary>
    Private _MostrarFechaVencimiento As Visibility = Visibility.Collapsed
    Public Property MostrarFechaVencimiento() As Visibility
        Get
            Return _MostrarFechaVencimiento

        End Get
        Set(value As Visibility)
            _MostrarFechaVencimiento = value
            MyBase.CambioItem("MostrarFechaVencimiento")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar el combo de compartimientos
    ''' </summary>
    Private _MostrarCompartimentos As Visibility = Visibility.Collapsed
    Public Property MostrarCompartimentos() As Visibility
        Get
            Return _MostrarCompartimentos

        End Get
        Set(value As Visibility)
            _MostrarCompartimentos = value
            MyBase.CambioItem("MostrarCompartimentos")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar el buscador de especies que traera el Isin referente a la especie
    ''' </summary>
    Private _MostrarISIN As Visibility = Visibility.Collapsed
    Public Property MostrarISIN() As Visibility
        Get
            Return _MostrarISIN

        End Get
        Set(value As Visibility)
            _MostrarISIN = value
            MyBase.CambioItem("MostrarISIN")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u ocultar el combo de Tipo de participación
    ''' </summary>
    Private _MostrarTipoParticipacion As Visibility = Visibility.Collapsed
    Public Property MostrarTipoParticipacion() As Visibility
        Get
            Return _MostrarTipoParticipacion

        End Get
        Set(value As Visibility)
            _MostrarTipoParticipacion = value
            MyBase.CambioItem("MostrarTipoParticipacion")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar el combo de Tipo de comisión que depende de la comisión de administracion
    ''' </summary>
    Private _MostrarTipoComision As Visibility = Visibility.Collapsed
    Public Property MostrarTipoComision() As Visibility
        Get
            Return _MostrarTipoComision

        End Get
        Set(value As Visibility)
            _MostrarTipoComision = value
            MyBase.CambioItem("MostrarTipoComision")
        End Set
    End Property

    Private _MostrarTipoComisionGestor As Visibility = Visibility.Collapsed
    Public Property MostrarTipoComisionGestor() As Visibility
        Get
            Return _MostrarTipoComisionGestor

        End Get
        Set(value As Visibility)
            _MostrarTipoComisionGestor = value
            MyBase.CambioItem("MostrarTipoComisionGestor")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u ocultar el combo de Tasa de comisión, que depende del Tipo de comisión
    Private _MostrarTasaComision As Visibility = Visibility.Collapsed
    Public Property MostrarTasaComision() As Visibility
        Get
            Return _MostrarTasaComision

        End Get
        Set(value As Visibility)
            _MostrarTasaComision = value
            MyBase.CambioItem("MostrarTasaComision")
        End Set
    End Property


    Private _DecimalesComision As String = ""
    Public Property DecimalesComision() As String
        Get
            Return _DecimalesComision

        End Get
        Set(value As String)
            _DecimalesComision = value
            MyBase.CambioItem("DecimalesComision")
        End Set
    End Property


    Private _MostrarTasaComisionGestor As Visibility = Visibility.Collapsed
    Public Property MostrarTasaComisionGestor() As Visibility
        Get
            Return _MostrarTasaComisionGestor

        End Get
        Set(value As Visibility)
            _MostrarTasaComisionGestor = value
            MyBase.CambioItem("MostrarTasaComisionGestor")
        End Set
    End Property


    ''' <summary>
    ''' Propiedad Mostrar u ocultar el textbox para ingresar el valor de comisión
    ''' </summary>
    Private _MostrarValorComision As Visibility = Visibility.Collapsed
    Public Property MostrarValorComision() As Visibility
        Get
            Return _MostrarValorComision

        End Get
        Set(value As Visibility)
            _MostrarValorComision = value
            MyBase.CambioItem("MostrarValorComision")
        End Set
    End Property

    Private _MostrarValorComisionGestor As Visibility = Visibility.Collapsed
    Public Property MostrarValorComisionGestor() As Visibility
        Get
            Return _MostrarValorComisionGestor

        End Get
        Set(value As Visibility)
            _MostrarValorComisionGestor = value
            MyBase.CambioItem("MostrarValorComisionGestor")
        End Set
    End Property

    Private _MostrarTablaAcumuladoComision As Visibility = Visibility.Collapsed
    Public Property MostrarTablaAcumuladoComision() As Visibility
        Get
            Return _MostrarTablaAcumuladoComision

        End Get
        Set(value As Visibility)
            _MostrarTablaAcumuladoComision = value
            MyBase.CambioItem("MostrarTablaAcumuladoComision")
        End Set
    End Property

    Private _MostrarComboTipoTabla As Visibility = Visibility.Collapsed
    Public Property MostrarComboTipoTabla() As Visibility
        Get
            Return _MostrarComboTipoTabla

        End Get
        Set(value As Visibility)
            _MostrarComboTipoTabla = value
            MyBase.CambioItem("MostrarComboTipoTabla")
        End Set
    End Property

    Private _MostrarGridAcumuladoComisiones As Visibility = Visibility.Collapsed
    Public Property MostrarGridAcumuladoComisiones() As Visibility
        Get
            Return _MostrarGridAcumuladoComisiones

        End Get
        Set(value As Visibility)
            _MostrarGridAcumuladoComisiones = value
            MyBase.CambioItem("MostrarGridAcumuladoComisiones")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u ocultar los combos que depende de las otras comisiones
    ''' </summary>
    Private _MostrarOpcionesOtrasComisiones As Visibility = Visibility.Collapsed
    Public Property MostrarOpcionesOtrasComisiones() As Visibility
        Get
            Return _MostrarOpcionesOtrasComisiones

        End Get
        Set(value As Visibility)
            _MostrarOpcionesOtrasComisiones = value
            MyBase.CambioItem("MostrarOpcionesOtrasComisiones")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar el textbox para ingresar el factor de entrada
    ''' </summary>
    Private _MostrarFactorEntrada As Visibility = Visibility.Collapsed
    Public Property MostrarFactorEntrada() As Visibility
        Get
            Return _MostrarFactorEntrada

        End Get
        Set(value As Visibility)
            _MostrarFactorEntrada = value
            MyBase.CambioItem("MostrarFactorEntrada")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar el textbox para ingresar el valor de entrada
    ''' </summary>
    Private _MostrarValorEntrada As Visibility = Visibility.Collapsed
    Public Property MostrarValorEntrada() As Visibility
        Get
            Return _MostrarValorEntrada

        End Get
        Set(value As Visibility)
            _MostrarValorEntrada = value
            MyBase.CambioItem("MostrarValorEntrada")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar el textbox para ingresar el Fator de salida
    ''' </summary>
    Private _MostrarFactorSalida As Visibility = Visibility.Collapsed
    Public Property MostrarFactorSalida() As Visibility
        Get
            Return _MostrarFactorSalida

        End Get
        Set(value As Visibility)
            _MostrarFactorSalida = value
            MyBase.CambioItem("MostrarFactorSalida")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar el textbox para ingresar el valor de salida
    ''' </summary>
    Private _MostrarValorSalida As Visibility = Visibility.Collapsed
    Public Property MostrarValorSalida() As Visibility
        Get
            Return _MostrarValorSalida

        End Get
        Set(value As Visibility)
            _MostrarValorSalida = value
            MyBase.CambioItem("MostrarValorSalida")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar los campos principales de cada uno de las tabs (Estructura, Comisiones, Tesorería, Limites, Clientes autoriados)
    ''' </summary>
    Private _MostrarCamposPrincipales As Visibility = Visibility.Collapsed
    Public Property MostrarCamposPrincipales() As Visibility
        Get
            Return _MostrarCamposPrincipales

        End Get
        Set(value As Visibility)
            _MostrarCamposPrincipales = value
            MyBase.CambioItem("MostrarCamposPrincipales")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar los campos del grid???
    ''' </summary>
    Private _MostrarCamposGrid As Visibility = Visibility.Collapsed
    Public Property MostrarCamposGrid() As Visibility
        Get
            Return _MostrarCamposGrid

        End Get
        Set(value As Visibility)
            _MostrarCamposGrid = value
            MyBase.CambioItem("MostrarCamposGrid")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad para mostrar u ocultar el buscador de códigos Oyd, dependiendo del tipo de Participación
    ''' </summary>
    Private _MostrarBuscadorOyd As Visibility = Visibility.Collapsed
    Public Property MostrarBuscadorOyd() As Visibility
        Get
            Return _MostrarBuscadorOyd

        End Get
        Set(value As Visibility)
            _MostrarBuscadorOyd = value
            MyBase.CambioItem("MostrarBuscadorOyd")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad para mostrar u ocultar el buscador de compañias, dependiendo del tipo de Participación
    ''' </summary>
    Private _MostrarBuscadorCompanias As Visibility = Visibility.Collapsed
    Public Property MostrarBuscadorCompanias() As Visibility
        Get
            Return _MostrarBuscadorCompanias

        End Get
        Set(value As Visibility)
            _MostrarBuscadorCompanias = value
            MyBase.CambioItem("MostrarBuscadorCompanias")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar los campos referentes al iva para la comisión de adminsitración
    ''' </summary>
    Private _MostrarIvaComisionAdmon As Visibility = Visibility.Collapsed
    Public Property MostrarIvaComisionAdmon() As Visibility
        Get
            Return _MostrarIvaComisionAdmon

        End Get
        Set(value As Visibility)
            _MostrarIvaComisionAdmon = value
            MyBase.CambioItem("MostrarIvaComisionAdmon")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar los campos referentes al iva para la comisión de adminsitración
    ''' </summary>
    Private _MostrarIvaComisionGestor As Visibility = Visibility.Collapsed
    Public Property MostrarIvaComisionGestor() As Visibility
        Get
            Return _MostrarIvaComisionGestor

        End Get
        Set(value As Visibility)
            _MostrarIvaComisionGestor = value
            MyBase.CambioItem("MostrarIvaComisionGestor")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Mostrar u ocultar el campo periodo de gracia de acuerdo a la configuración del pacto de permanencia y la viabilidad si el fondo es renovable o no
    ''' </summary>
    Private _MostrarPeriodoGracia As Visibility = Visibility.Collapsed
    Public Property MostrarPeriodoGracia() As Visibility
        Get
            Return _MostrarPeriodoGracia

        End Get
        Set(value As Visibility)
            _MostrarPeriodoGracia = value
            MyBase.CambioItem("MostrarPeriodoGracia")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para limpiar los datos del cliente
    ''' </summary>
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
    ''' <summary>
    ''' Propiedad para listar los campos del tipo compañias
    ''' </summary>
    Private _Companias As List(Of Companias)
    Public Property Companias() As List(Of Companias)
        Get
            Return _Companias
        End Get
        Set(ByVal value As List(Of Companias))
            _Companias = value
            MyBase.CambioItem("Companias")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Habilitar la edicion del detalle
    ''' </summary>
    Private _HabilitarEdicionDetalle As Boolean
    Public Property HabilitarEdicionDetalle() As Boolean
        Get
            Return _HabilitarEdicionDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionDetalle = value
            MyBase.CambioItem("HabilitarEdicionDetalle")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Habilitar el boton??
    ''' </summary>
    Private _HabilitarBoton As System.Boolean = True
    Public Property HabilitarBoton() As System.Boolean
        Get
            Return _HabilitarBoton
        End Get
        Set(ByVal value As System.Boolean)
            _HabilitarBoton = value
            MyBase.CambioItem("HabilitarBoton")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad Habilitar campos activa??
    ''' </summary>
    Private _HabilitarCampoActiva As Boolean
    Public Property HabilitarCampoActiva() As Boolean
        Get
            Return _HabilitarCampoActiva
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCampoActiva = value
            MyBase.CambioItem("HabilitarCampoActiva")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad para validar si, se muestra o no el boton de penalidades (Depende si es o no un nuevo registro)
    ''' </summary>
    Private _HabilitarBotonImportar As Visibility
    Public Property HabilitarBotonImportar() As Visibility
        Get
            Return _HabilitarBotonImportar
        End Get
        Set(ByVal value As Visibility)
            _HabilitarBotonImportar = value
            MyBase.CambioItem("HabilitarBotonImportar")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad para almacenar el indice seleccionado del grid de Tesorería
    ''' </summary>
    Private _IndiceListaTesoreria As Integer
    Public Property IndiceListaTesoreria() As Integer
        Get
            Return _IndiceListaTesoreria
        End Get
        Set(ByVal value As Integer)
            _IndiceListaTesoreria = value
            MyBase.CambioItem("IndiceListaTesoreria")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad para Habilitar o deshabilitar los campos de los tabs
    ''' </summary>
    Private _HabilitarCamposlectura As Boolean
    Public Property HabilitarCamposlectura() As Boolean
        Get
            Return _HabilitarCamposlectura
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposlectura = value
            MyBase.CambioItem("HabilitarCamposlectura")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad para mostrar u ocultar el grid de autorizaciones (Tab Clientes autorizados)
    ''' </summary>
    Private _MostrarGridAutorizaciones As Visibility
    Public Property MostrarGridAutorizaciones() As Visibility
        Get
            Return _MostrarGridAutorizaciones
        End Get
        Set(ByVal value As Visibility)
            _MostrarGridAutorizaciones = value
            MyBase.CambioItem("MostrarGridAutorizaciones")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad para verificar si el parametro se encunetra "prendido" o no.
    ''' </summary>
    Private _TieneParametro As Boolean
    Public Property TieneParametro() As Boolean
        Get
            Return _TieneParametro
        End Get
        Set(ByVal value As Boolean)
            _TieneParametro = value
            MyBase.CambioItem("TieneParametro")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad para mostrar en texto el estado de la compañia, se utiliza con un converter ya que el campo en la bd es Bit
    ''' </summary>
    Private _strEstadoCompania As String
    Public Property strEstadoCompania() As String
        Get
            Return _strEstadoCompania
        End Get
        Set(ByVal value As String)
            _strEstadoCompania = value
            MyBase.CambioItem("strEstadoCompania")
        End Set
    End Property


    Private _logValorMaximo As Boolean
    Public Property logValorMaximo() As Boolean
        Get
            Return _logValorMaximo
        End Get
        Set(ByVal value As Boolean)
            _logValorMaximo = value
            If value Then
                _DetalleAcumuladoComisionesSeleccionado.dblRangoFinal = Program.ValorMaximoRangoAcumuladoComisiones
            Else
                _DetalleAcumuladoComisionesSeleccionado.dblRangoFinal = 0
            End If
            'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logValorMaximo"))
        End Set
    End Property




    'Private _strCodigoOyD As String
    'Public Property strCodigoOyD() As String
    '    Get
    '        Return _strCodigoOyD
    '    End Get
    '    Set(value As String)
    '        _strCodigoOyD = value
    '        MyBase.CambioItem("strCodigoOyD")
    '    End Set
    'End Property

    ''' <summary>
    ''' Propiedad para mostrar u ocultar el buscador de cliente partidas
    ''' </summary>
    Private _MostrarClientePartidas As Visibility = Visibility.Collapsed
    Public Property MostrarClientePartidas() As Visibility
        Get
            Return _MostrarClientePartidas
        End Get
        Set(ByVal value As Visibility)
            _MostrarClientePartidas = value
            MyBase.CambioItem("MostrarClientePartidas")
        End Set
    End Property
    ''' <summary>
    ''' Propiedad para indicar el estado busy del buscador de operación
    ''' </summary>
    Private _IsBusyTipoOperacion As Boolean
    Public Property IsBusyTipoOperacion() As Boolean
        Get
            Return _IsBusyTipoOperacion
        End Get
        Set(ByVal value As Boolean)
            _IsBusyTipoOperacion = value
            MyBase.CambioItem("IsBusyTipoOperacion")
        End Set
    End Property

    'Private _StrIDCuentaContable As String
    'Public Property StrIDCuentaContable() As String
    '    Get
    '        Return _StrIDCuentaContable
    '    End Get
    '    Set(ByVal value As String)
    '        _StrIDCuentaContable = value
    '        MyBase.CambioItem("StrIDCuentaContable")
    '    End Set
    'End Property

    'JDCP20170124
    Private ListaTipoCompaniaCompleta As List(Of OYDUtilidades.ItemCombo)
    Private ListaTipoCompaniaEdicion As List(Of OYDUtilidades.ItemCombo)

    'JCM20170213
    Private ListaTipoPlazoCompleta As List(Of OYDUtilidades.ItemCombo)


    Private _ListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoCompania() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoCompania
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoCompania = value
            MyBase.CambioItem("ListaTipoCompania")
        End Set
    End Property
    Private _ListaTipoCompaniaBusqueda As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoCompaniaBusqueda() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoCompaniaBusqueda
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoCompaniaBusqueda = value
            MyBase.CambioItem("ListaTipoCompaniaBusqueda")
        End Set
    End Property
    Private _ListaTipoPlazoBusqueda As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoPlazoBusqueda() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoPlazoBusqueda
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoPlazoBusqueda = value
            MyBase.CambioItem("ListaTipoPlazoBusqueda")
        End Set
    End Property

    Private _ListaTipoParticipacion As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoParticipacion() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoParticipacion
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoParticipacion = value
            MyBase.CambioItem("ListaTipoParticipacion")
        End Set
    End Property


    Private _MostrarControlesTipoCompania As Visibility = Visibility.Visible
    Public Property MostrarControlesTipoCompania() As Visibility
        Get
            Return _MostrarControlesTipoCompania
        End Get
        Set(ByVal value As Visibility)
            _MostrarControlesTipoCompania = value
            MyBase.CambioItem("MostrarControlesTipoCompania")
        End Set
    End Property

    Private _strTipoAgrupacion As String = "todoslosreceptores"
    Public Property strTipoAgrupacion() As String
        Get
            Return _strTipoAgrupacion
        End Get
        Set(ByVal value As String)
            _strTipoAgrupacion = value
            MyBase.CambioItem("strTipoAgrupacion")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para habilitar o deshabilitar el campo estado
    ''' </summary>
    Private _HabilitarEstado As Boolean = False
    Public Property HabilitarEstado() As Boolean
        Get
            Return _HabilitarEstado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEstado = value
            MyBase.CambioItem("HabilitarEstado")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para habilitar o deshabilitar el campo estado
    ''' </summary>
    Private _HabilitarTipoCompania As Boolean = False
    Public Property HabilitarTipoCompania() As Boolean
        Get
            Return _HabilitarTipoCompania
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoCompania = value
            MyBase.CambioItem("HabilitarTipoCompania")
        End Set
    End Property


    Private _MostrarDatosComisionAdmon As Visibility = Visibility.Collapsed
    Public Property MostrarDatosComisionAdmon() As Visibility
        Get
            Return _MostrarDatosComisionAdmon

        End Get
        Set(value As Visibility)
            _MostrarDatosComisionAdmon = value
            MyBase.CambioItem("MostrarDatosComisionAdmon")
        End Set
    End Property

    Private _HabilitarCamposOmnibus As Boolean
    Public Property HabilitarCamposOmnibus() As Boolean
        Get
            Return _HabilitarCamposOmnibus
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposOmnibus = value
            MyBase.CambioItem("HabilitarCamposOmnibus")
        End Set
    End Property


#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' Descripción      : Actualización. Nuevos campos strContabDividendos, dtmApertura, dtmUltimoCierre, strProveedorPrecios.
    ''' Fecha            : Agosto 12/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Agosto 12/2015 - Resultado Ok 
    ''' 'JEPM201500812
    ''' </history>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New Companias

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of Companias)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIDCompania = -1
                objNvoEncabezado.strTipoDocumento = String.Empty
                objNvoEncabezado.strNumeroDocumento = String.Empty
                objNvoEncabezado.strNombre = String.Empty
                objNvoEncabezado.strTipoDocumento = String.Empty
                objNvoEncabezado.strCompaniaContable = String.Empty
                objNvoEncabezado.strCodigoSuper = String.Empty
                objNvoEncabezado.strTipoCompania = String.Empty
                objNvoEncabezado.logActiva = True
                objNvoEncabezado.strContabDividendos = String.Empty
                objNvoEncabezado.dtmApertura = Nothing
                objNvoEncabezado.dtmUltimoCierre = Nothing
                objNvoEncabezado.strProveedorPrecios = String.Empty
                objNvoEncabezado.strTipoEntidadSuper = String.Empty
                objNvoEncabezado.intIdDinamicaContableConfig = Nothing
                objNvoEncabezado.intIdGestor = Nothing
                objNvoEncabezado.strNombreGestor = String.Empty
                '' ''JCM 2015/12/28
                objNvoEncabezado.logManejaValorUnidad = False
                objNvoEncabezado.strNombreMoneda = String.Empty
                objNvoEncabezado.intIDMoneda = Nothing
                objNvoEncabezado.strNombreMonedaComision = String.Empty
                objNvoEncabezado.intIDMonedaComision = Nothing
                objNvoEncabezado.dblValorUnidadInicial = Nothing
                objNvoEncabezado.dblValorUnidadVigente = Nothing
                objNvoEncabezado.strIdentificadorCuenta = "N"
                objNvoEncabezado.dtmFechaInicio = Nothing
                objNvoEncabezado.dtmFechaVencimiento = Nothing
                objNvoEncabezado.strTipoPlazo = String.Empty
                objNvoEncabezado.strPactoPermanencia = String.Empty
                objNvoEncabezado.intDiasPlazo = Nothing
                objNvoEncabezado.intDiasGracia = Nothing
                objNvoEncabezado.strPenalidad = String.Empty
                objNvoEncabezado.dblFactorPenalidad = Nothing
                objNvoEncabezado.strFondoCapitalPrivado = String.Empty
                objNvoEncabezado.strCompartimentos = String.Empty
                objNvoEncabezado.dblLimiteMontoInversion = Nothing
                objNvoEncabezado.strISIN = String.Empty
                objNvoEncabezado.strParticipacion = String.Empty
                objNvoEncabezado.strComisionAdministracion = "N"
                objNvoEncabezado.strTipoComision = String.Empty
                objNvoEncabezado.dblTasaComision = Nothing
                objNvoEncabezado.dblValorComision = Nothing
                objNvoEncabezado.strPeriodoComision = String.Empty
                objNvoEncabezado.strOtrasComisiones = "N"
                objNvoEncabezado.strDeExito = String.Empty
                objNvoEncabezado.dblComisionExito = Nothing
                objNvoEncabezado.strEntrada = String.Empty
                objNvoEncabezado.dblComisionEntrada = Nothing
                objNvoEncabezado.strSalida = String.Empty
                objNvoEncabezado.dblComisionSalida = Nothing
                objNvoEncabezado.strNombreCorto = String.Empty
                objNvoEncabezado.strTipoAutorizados = "T"
                objNvoEncabezado.strNumeroDocumentoClientePartidas = String.Empty
                objNvoEncabezado.strNombreclientepartidas = String.Empty
                objNvoEncabezado.lngIDBancos = Nothing
                objNvoEncabezado.strBancosDescripcion = String.Empty
                objNvoEncabezado.strSubtipoNegocio = String.Empty
                objNvoEncabezado.strCategoria = String.Empty
                objNvoEncabezado.strComisionGestor = String.Empty
                objNvoEncabezado.strTipoComisionGestor = String.Empty
                objNvoEncabezado.dblTasaComisionGestor = Nothing
                objNvoEncabezado.strComisionMinima = String.Empty
                objNvoEncabezado.dblValorComisionMin = Nothing
                objNvoEncabezado.strTipoAcumuladorComision = Nothing
                objNvoEncabezado.strIvaComisionAdmon = Nothing
                objNvoEncabezado.strIvaComisionGestor = Nothing
                objNvoEncabezado.strFondoRenovable = String.Empty
                objNvoEncabezado.strCuentaOmnibus = "NO"
                strTipoAgrupacion = "todoslosreceptores"



            End If

            ListaAcumulacionComisiones.Clear()

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            _mlogBuscarClienteEncabezado = True
            _mLogBuscarClienteDetalle = True


            HabilitarEncabezado = True
            HabilitarEstado = False

            If logHabilitarCompaniaPasivaTesoreria Then
                HabilitarTipoCompania = False
            Else
                HabilitarTipoCompania = True
            End If

            HabilitarCamposEdicion = True
            HabilitarCamposGestor = True
            HabilitarEdicionDetalle = True
            HabilitarCampoActiva = False
            HabilitarCamposMoneda = True
            HabilitarBotonImportar = Visibility.Collapsed
            logParametrosPorDefecto = True
            VerificarHabilitarDeshabilitar()

            'JDCP20170124
            CargarTipoCompaniaValidas(True)
            If Not IsNothing(ListaTipoCompania) Then
                If logHabilitarCompaniaPasivaTesoreria Then
                    If ListaTipoCompania.Where(Function(i) i.ID = "TES").Count > 0 Then
                        _EncabezadoSeleccionado.strTipoCompania = ListaTipoCompania.Where(Function(i) i.ID = "TES").First.ID
                        _EncabezadoSeleccionado.strTipoCompaniaDescripcion = ListaTipoCompania.Where(Function(i) i.ID = "TES").First.Descripcion
                    End If
                Else
                    If ListaTipoCompania.Count = 1 Then
                        _EncabezadoSeleccionado.strTipoCompania = _ListaTipoCompania.First.ID
                        _EncabezadoSeleccionado.strTipoCompaniaDescripcion = _ListaTipoCompania.First.Descripcion
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' TODO CP
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado(True, FiltroVM)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    ''' TODO CP
    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.strTipoDocumento) Or Not IsNothing(cb.strNumeroDocumento) Or
                Not IsNothing(cb.strNombre) Or Not IsNothing(cb.strTipoCompania) Then 'Validar que ingresó algo en los campos de búsqueda

                Await ConsultarEncabezado(False, String.Empty, cb.strTipoDocumento, cb.strNumeroDocumento, cb.strNombre, cb.strTipoCompania, cb.strTipoPlazo, cb.strParticipacion)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' ID caso de prueba:  CP008 Guardar Compañía
    ''' </summary>
    ''' <history>
    ''' Descripción      : Actualización. Nuevos campos strContabDividendos, dtmApertura, dtmUltimoCierre, strProveedorPrecios.
    ''' Fecha            : Agosto 12/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Agosto 12/2015 - Resultado Ok 
    ''' 'JEPM201500812
    ''' </history>
    Public Overrides Async Sub ActualizarRegistro()

        '------------------------------------------------------------------------------------------------------------------------------------------------
        '-- Valida que no existan una compañia repetida en el detalle
        'ID caso de prueba:  CP048
        '------------------------------------------------------------------------------------------------------------------------------------------------



        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            Dim xmlCompleto As String
            Dim xmlDetalle As String
            Dim xmlCompletoPenalidad As String
            Dim xmlDetallePenalidad As String
            Dim xmlCompletoTesoreria As String
            Dim xmlDetalleTesoreria As String
            Dim xmlDetalleLimites As String
            Dim xmlCompletoLimites As String
            Dim xmlDetalleAutorizaciones As String
            Dim xmlCompletoAutorizaciones As String
            Dim xmlDetalleCondicionesTesoreria As String
            Dim xmlCompletoCondicionesTesoreria As String
            Dim xmlDetalleParametrosCompania As String
            Dim xmlCompletoParametrosCompania As String
            Dim xmlDetalleAcumladoComisiones As String
            Dim xmlCompletoAcumladoComisiones As String


            ErrorForma = String.Empty
            IsBusy = True

            'JDCP20170124
            If _EncabezadoSeleccionado.strTipoCompania <> TIPOCOMPANIA_TESORERIA Then
                ValidarAutorizacion()
            End If

            If ValidarRegistro() Then

                'JDCP20170124
                If (ValidarDetalle() And ValidarDetallePenalidad() And ValidarDetalleAcumuladoComisiones() And ValidarDetalleLimitesActualizar()) Or _EncabezadoSeleccionado.strTipoCompania = TIPOCOMPANIA_TESORERIA Then

                    ' Incializar los mensajes de validación
                    _EncabezadoSeleccionado.strMsgValidacion = String.Empty

                    'Construir el XML de los detalles
                    xmlCompleto = "<Compania>"

                    If Not IsNothing(ListaDetalle) Then
                        For Each objeto In (From c In ListaDetalle)

                            xmlDetalle = "<Detalle intIDCompania=""" & objeto.intIDCompania &
                                         """ lngIDComitente=""" & objeto.lngIDComitente &
                                         """ logCompania=""" & objeto.logCompania & """></Detalle>"

                            xmlCompleto = xmlCompleto & xmlDetalle
                        Next
                    End If
                    xmlCompleto = xmlCompleto & "</Compania>"

                    'Construir el XML de los detalles Penalidades
                    xmlCompletoPenalidad = "<Compania>"

                    If Not IsNothing(ListaPenalidad) Then
                        For Each objeto In (From c In ListaPenalidad)

                            xmlDetallePenalidad = "<DetallePenalidad intIDCompania=""" & objeto.intIDCompania &
                                                  """ intDiasInicial=""" & objeto.intDiasInicial &
                                                  """ intDiasFinal=""" & objeto.intDiasFinal &
                                                  """ dblFactor=""" & objeto.dblFactor & """></DetallePenalidad>"

                            xmlCompletoPenalidad = xmlCompletoPenalidad & xmlDetallePenalidad
                        Next

                    End If

                    xmlCompletoPenalidad = xmlCompletoPenalidad & "</Compania>"


                    ''Construir el XML de los detalles Tesoreria
                    xmlCompletoTesoreria = "<Compania>"

                    If Not IsNothing(ListaDetalleTesoreria) Then
                        For Each objeto In (From c In ListaDetalleTesoreria)
                            xmlDetalleTesoreria = "<DetalleTesoreria intIDCompania=""" & objeto.intIDCompania &
                             """ strTipoOperacion=""" & objeto.strTipoOperacion &
                             """ intDiasAplicaTransaccion=""" & objeto.intDiasAplicaTransaccion &
                            """ lngIDConcepto=""" & objeto.lngIDConcepto &
                            """ intDiasPago=""" & objeto.intDiasPago & """></DetalleTesoreria>"
                            xmlCompletoTesoreria = xmlCompletoTesoreria & xmlDetalleTesoreria
                        Next
                    End If
                    xmlCompletoTesoreria = xmlCompletoTesoreria & "</Compania>"


                    'Construir el XML de los detalles Limites
                    xmlCompletoLimites = "<Compania>"

                    If Not IsNothing(ListaDetalleLimites) Then
                        For Each objeto In (From c In ListaDetalleLimites)
                            xmlDetalleLimites = "<DetalleLimites intIDCompania=""" & objeto.intIDCompania &
                            """ strTipoConcepto=""" & objeto.strTipoConcepto &
                            """ logValor=""" & objeto.logValor &
                            """ dblValor=""" & objeto.dblValor &
                            """ logPorcentaje=""" & objeto.logPorcentaje &
                            """ dblPorcentaje=""" & objeto.dblPorcentaje & """></DetalleLimites>"
                            xmlCompletoLimites = xmlCompletoLimites & xmlDetalleLimites
                        Next
                    End If
                    xmlCompletoLimites = xmlCompletoLimites & "</Compania>"


                    'Construir el XML de los detalles Autorizaciones
                    xmlCompletoAutorizaciones = "<Compania>"

                    If Not IsNothing(ListaDetalleAutorizaciones) Then
                        For Each objeto In (From c In ListaDetalleAutorizaciones)
                            xmlDetalleAutorizaciones = "<DetalleAutorizaciones intIDCompania=""" & objeto.intIDCompania &
                            """ lngIDComitente=""" & objeto.lngIDComitente & """></DetalleAutorizaciones>"
                            xmlCompletoAutorizaciones = xmlCompletoAutorizaciones & xmlDetalleAutorizaciones
                        Next
                    End If
                    xmlCompletoAutorizaciones = xmlCompletoAutorizaciones & "</Compania>"

                    'Construir el XML de los detalles de condiciones Tesoreria, JCM20160816
                    xmlCompletoCondicionesTesoreria = "<Compania>"
                    If Not IsNothing(ListaDetalleCondicionesTesoreria) Then
                        For Each objeto In (From c In ListaDetalleCondicionesTesoreria)
                            xmlDetalleCondicionesTesoreria = "<DetalleCondicionesTesoreria intIDCompania=""" & objeto.intIDCompania &
                            """ strTipoOperacion=""" & objeto.strTipoOperacionCT &
                            """ lngIDConcepto=""" & objeto.lngIDConceptoCT & """></DetalleCondicionesTesoreria>"
                            xmlCompletoCondicionesTesoreria = xmlCompletoCondicionesTesoreria & xmlDetalleCondicionesTesoreria
                        Next
                    End If
                    xmlCompletoCondicionesTesoreria = xmlCompletoCondicionesTesoreria & "</Compania>"



                    If ListaParametrosSeleccioando.Count > 0 Then
                        FiltrarInformacion("")
                    End If

                    'Construir el XML de los detalles de los parametros de la compania, JCM20160910
                    xmlCompletoParametrosCompania = "<Compania>"
                    If Not IsNothing(ListaDetalleParametrosCompania) Then
                        For Each objeto In (From c In ListaDetalleParametrosCompania)
                            If objeto.dtmFechaInicialParametro.ToString = "" Then
                                xmlDetalleParametrosCompania = "<ParametrosAdicionales intIDParametrizacion=""" & objeto.intID &
                                """ strValor=""" & ConvertirTexto(objeto.strValorParametro) &
                                """ logManejaFecha=""" & objeto.logManejaFechaParametro &
                                """ dtmFechaInicial=""" & objeto.dtmFechaInicialParametro & """></ParametrosAdicionales>"
                                xmlCompletoParametrosCompania = xmlCompletoParametrosCompania & xmlDetalleParametrosCompania
                            Else
                                xmlDetalleParametrosCompania = "<ParametrosAdicionales intIDParametrizacion=""" & objeto.intID &
                                """ strValor=""" & ConvertirTexto(objeto.strValorParametro) &
                                """ logManejaFecha=""" & objeto.logManejaFechaParametro &
                                """ dtmFechaInicial=""" & objeto.dtmFechaInicialParametro.Value.ToString("yyyy-MM-dd") & """></ParametrosAdicionales>"
                                xmlCompletoParametrosCompania = xmlCompletoParametrosCompania & xmlDetalleParametrosCompania

                            End If
                        Next
                    End If
                    xmlCompletoParametrosCompania = xmlCompletoParametrosCompania & "</Compania>"


                    'Construir el XML de detalla de Acumulado Comisiones, JCM20170511
                    xmlCompletoAcumladoComisiones = "<Compania>"
                    If Not IsNothing(ListaAcumulacionComisiones) Then
                        For Each Objeto In (From c In ListaAcumulacionComisiones)
                            xmlDetalleAcumladoComisiones = "<DetalleAcumladoComisiones intIDCompania=""" & Objeto.intIDCompania &
                            """ dblRangoInicio=""" & Objeto.dblRangoInicio &
                            """ dblRangoFinal=""" & Objeto.dblRangoFinal &
                            """ dblPorcentaje=""" & Objeto.dblPorcentaje &
                            """ logValorMaximo=""" & Objeto.logValorMaximo &
                            """ strTipoTabla=""" & Objeto.strTipoTabla &
                            """ strTipoComision=""" & Objeto.strComisionOrigen &
                            """></DetalleAcumladoComisiones>"
                            xmlCompletoAcumladoComisiones = xmlCompletoAcumladoComisiones & xmlDetalleAcumladoComisiones
                        Next
                    End If
                    xmlCompletoAcumladoComisiones = xmlCompletoAcumladoComisiones & "</Compania>"




                    'JCM20161024
                    'Se realiza un Replace del xmlCompletoParametrosCompania, los campo < por {##}
                    'xmlCompletoParametrosCompania = Replace(xmlCompletoParametrosCompania, "<", "{##}")



                    Dim strMsg As String = String.Empty
                    Dim objRet As InvokeOperation(Of String)
                    Dim strCodigoNuevo As Integer = EncabezadoSeleccionado.intIDCompania




                    objRet = Await mdcProxy.ActualizarCompaniasSync(EncabezadoSeleccionado.intIDCompania, EncabezadoSeleccionado.strTipoDocumento, EncabezadoSeleccionado.strNumeroDocumento,
                                                                    EncabezadoSeleccionado.strNombre, EncabezadoSeleccionado.strCompaniaContable, EncabezadoSeleccionado.strTipoCompania,
                                                                    EncabezadoSeleccionado.strCodigoSuper, EncabezadoSeleccionado.logActiva, EncabezadoSeleccionado.strContabDividendos,
                                                                    EncabezadoSeleccionado.dtmApertura, EncabezadoSeleccionado.dtmUltimoCierre, EncabezadoSeleccionado.strProveedorPrecios,
                                                                    EncabezadoSeleccionado.strTipoEntidadSuper, EncabezadoSeleccionado.intIdDinamicaContableConfig, EncabezadoSeleccionado.intIdGestor,
                                                                    EncabezadoSeleccionado.strNombreGestor, EncabezadoSeleccionado.logManejaValorUnidad, EncabezadoSeleccionado.intIDMoneda,
                                                                    EncabezadoSeleccionado.strNombreMoneda, EncabezadoSeleccionado.dblValorUnidadInicial, EncabezadoSeleccionado.dblValorUnidadVigente,
                                                                    EncabezadoSeleccionado.strIdentificadorCuenta, EncabezadoSeleccionado.strOtroPrefijo, EncabezadoSeleccionado.dtmFechaInicio, EncabezadoSeleccionado.dtmFechaVencimiento,
                                                                    EncabezadoSeleccionado.strTipoPlazo, EncabezadoSeleccionado.strPactoPermanencia, EncabezadoSeleccionado.intDiasPlazo, EncabezadoSeleccionado.intDiasGracia,
                                                                    EncabezadoSeleccionado.strPenalidad, EncabezadoSeleccionado.dblFactorPenalidad, EncabezadoSeleccionado.strFondoCapitalPrivado, EncabezadoSeleccionado.strCompartimentos,
                                                                    EncabezadoSeleccionado.dblLimiteMontoInversion, EncabezadoSeleccionado.strInscritoRNVE, EncabezadoSeleccionado.strIdEspecie, EncabezadoSeleccionado.strISIN, EncabezadoSeleccionado.strParticipacion, EncabezadoSeleccionado.strComisionAdministracion,
                                                                    EncabezadoSeleccionado.strTipoComision, EncabezadoSeleccionado.dblTasaComision, EncabezadoSeleccionado.dblValorComision, EncabezadoSeleccionado.strPeriodoComision,
                                                                    EncabezadoSeleccionado.strOtrasComisiones, EncabezadoSeleccionado.strDeExito, EncabezadoSeleccionado.dblComisionExito, EncabezadoSeleccionado.dblValorExito, EncabezadoSeleccionado.strEntrada,
                                                                    EncabezadoSeleccionado.dblComisionEntrada, EncabezadoSeleccionado.dblValorComisionEntrada, EncabezadoSeleccionado.strSalida, EncabezadoSeleccionado.dblComisionSalida, EncabezadoSeleccionado.dblValorComisionSalida,
                                                                    EncabezadoSeleccionado.strNombreCorto, EncabezadoSeleccionado.strTipoAutorizados, EncabezadoSeleccionado.strNumeroDocumentoClientePartidas, EncabezadoSeleccionado.strNombreclientepartidas, EncabezadoSeleccionado.lngIDBancos.Value, EncabezadoSeleccionado.strSubtipoNegocio,
                                                                    EncabezadoSeleccionado.strCategoria, EncabezadoSeleccionado.strComisionGestor, EncabezadoSeleccionado.strTipoComisionGestor, EncabezadoSeleccionado.dblTasaComisionGestor, EncabezadoSeleccionado.strComisionMinima, EncabezadoSeleccionado.dblValorComisionMin, EncabezadoSeleccionado.lngIDComitenteClientePartidas, EncabezadoSeleccionado.strTipoAcumuladorComision, EncabezadoSeleccionado.strIvaComisionAdmon, EncabezadoSeleccionado.strIvaComisionGestor,
                                                                    EncabezadoSeleccionado.strFondoRenovable, EncabezadoSeleccionado.strBaseComisionAdmon, EncabezadoSeleccionado.strPeriodoCobroComisionAdmon, xmlCompleto, xmlCompletoPenalidad, xmlCompletoTesoreria, xmlCompletoLimites, xmlCompletoAutorizaciones, xmlCompletoCondicionesTesoreria, xmlCompletoParametrosCompania, xmlCompletoAcumladoComisiones, Program.Usuario, Program.HashConexion, EncabezadoSeleccionado.strCuentaOmnibus,
                                                                    EncabezadoSeleccionado.intIDMonedaComision, EncabezadoSeleccionado.strNombreMonedaComision).AsTask()


                    If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                        strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                        If Not String.IsNullOrEmpty(strMsg) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        Editando = False
                        HabilitarEncabezado = False
                        HabilitarEstado = False
                        HabilitarTipoCompania = False
                        HabilitarCamposEdicion = False
                        HabilitarCamposGestor = False
                        HabilitarCamposMoneda = False
                        HabilitarEdicionDetalle = False
                        HabilitarCampoActiva = False
                        HabilitarBotonImportar = Visibility.Visible
                        logParametrosPorDefecto = False
                        VerificarHabilitarDeshabilitar()

                        'limpio cache
                        If strCodigoNuevo = 0 Or strCodigoNuevo = -1 Then
                            Dim objDebug As New clsDebug With {.strMensajeDebug = "Elimino recurso combos"}
                            Messenger.Default.Send(Of clsDebug)(objDebug)
                            If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                                Application.Current.Resources.Remove(Program.NOMBRE_LISTA_COMBOS)
                            End If
                            objDebug = New clsDebug With {.strMensajeDebug = "Inicia recarga combos"}
                            Messenger.Default.Send(Of clsDebug)(objDebug)
                            Dim recursos As New A2UtilsViewModel()
                            Await recursos.inicializarCombos("", "", True)
                        End If


                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        Await ConsultarEncabezado(True, String.Empty)
                    End If 'If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                End If 'If ValidarDetalle() Then

            Else
                HabilitarEncabezado = True
                If logHabilitarCompaniaPasivaTesoreria Then
                    HabilitarTipoCompania = False
                Else
                    HabilitarTipoCompania = True
                End If
                HabilitarCamposEdicion = True
                HabilitarCamposGestor = True
                HabilitarCamposMoneda = True
                HabilitarEdicionDetalle = True
                HabilitarBotonImportar = Visibility.Collapsed
                logParametrosPorDefecto = True
                VerificarHabilitarDeshabilitar()
            End If

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' ID caso de prueba:  CP009 No habilitar los campos tipo documento ni numero documento en edición
    ''' ID caso de prueba:  CP012 que no permita modificar si esta inactiva
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not EncabezadoSeleccionado.logActiva Then 'CP012
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("La Compañía está inactiva, no se permite modificar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            _EncabezadoSeleccionado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            If String.IsNullOrEmpty(_EncabezadoSeleccionado.strCuentaOmnibus) Then
                _EncabezadoSeleccionado.strCuentaOmnibus = "NO"
            End If

            Editando = True
            MyBase.CambioItem("Editando")

            HabilitarEncabezado = True
            HabilitarEstado = True
            If logHabilitarCompaniaPasivaTesoreria Then
                HabilitarTipoCompania = False
            Else
                HabilitarTipoCompania = True
            End If
            HabilitarCamposEdicion = False
            HabilitarCamposGestor = True
            HabilitarEdicionDetalle = True
            HabilitarCamposMoneda = True
            HabilitarBotonImportar = Visibility.Visible
            logParametrosPorDefecto = False
            VerificarHabilitarDeshabilitar()

            'JDCP20170124
            Dim strTipoCompania As String = _EncabezadoSeleccionado.strTipoCompania

            CargarTipoCompaniaValidas(True)

            _EncabezadoSeleccionado.strTipoCompania = strTipoCompania

            HabilitarBoton = False
            HabilitarCampoActiva = True

            ListaAcumulacionComisionesPorTipoTabla = Nothing
            If EncabezadoSeleccionado.strTipoAcumuladorComision = "AD" Then
                MostrarComboTipoTabla = Visibility.Visible
            End If

            IsBusy = False
        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                'JDCP20170124
                CargarTipoCompaniaValidas(False)

                mdcProxy.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
                HabilitarEstado = False
                HabilitarTipoCompania = False
                HabilitarCamposEdicion = False
                HabilitarCamposGestor = False
                HabilitarCamposMoneda = False
                HabilitarEdicionDetalle = False
                HabilitarBotonImportar = Visibility.Visible
                logParametrosPorDefecto = True
                HabilitarCampoActiva = False
                strTipoTabla = ""
                ListaAcumulacionComisionesPorTipoTabla = Nothing
                If EncabezadoSeleccionado.strTipoAcumuladorComision = "AD" Then
                    MostrarComboTipoTabla = Visibility.Visible
                End If
                VerificarHabilitarDeshabilitar()

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' ID caso de prueba:  CP011
    ''' </summary>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                'Preguntar que si desea activar/inactivar la compañía
                If mdcProxy.Companias.Where(Function(i) i.intIDCompania = EncabezadoSeleccionado.intIDCompania And CBool(EncabezadoSeleccionado.logActiva) = False).Count > 0 Then
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción activa la compañía seleccionada. ¿Confirma la activación de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)

                Else
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción inactiva la compañía seleccionada. ¿Confirma la inactivación de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
                End If

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inactivar un registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Ejecuta el proceso de eliminación en la base de datos
    ''' </summary>
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.Companias.Where(Function(i) i.intIDCompania = EncabezadoSeleccionado.intIDCompania).Count > 0 Then
                        mdcProxy.Companias.Remove(mdcProxy.Companias.Where(Function(i) i.intIDCompania = EncabezadoSeleccionado.intIDCompania).First)
                    End If

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del comitente
    ''' ID caso de prueba:  CP005, CP006. Asignar datos de cliente en el encabezado o detalles
    ''' </summary>
    ''' <param name="pstrFiltroComitente"></param>
    ''' <param name="pstrBusqueda"></param>
    ''' <remarks>Filtro por Numero de documento o de Código OyD del comitente según de donde se llame. Si se llama del encabezado usa el número de documento y si se llama de detalles utiliza el código OyD.</remarks>
    Friend Sub buscarComitente(Optional ByVal pstrFiltroComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim pstrIdComitente As String = String.Empty

        Try

            'Filtro por Numero de documento si busca del encabezado
            If (_mlogBuscarClienteEncabezado) Then
                If Not Me.EncabezadoSeleccionado Is Nothing Then
                    If Not pstrIdComitente.Equals(Me.EncabezadoSeleccionado.strNumeroDocumento) Then
                        If pstrFiltroComitente.Trim.Equals(String.Empty) Then
                            pstrIdComitente = Me.EncabezadoSeleccionado.strNumeroDocumento
                        Else
                            pstrIdComitente = pstrFiltroComitente
                        End If

                        If Not pstrIdComitente Is Nothing AndAlso Not pstrIdComitente.Trim.Equals(String.Empty) Then
                            objProxy.BuscadorClientes.Clear()
                            'objProxy.Load(objProxy.buscarClientesQuery(pstrIdComitente, "A", String.Empty, "clienteagrupador", Program.Usuario, False, 0), AddressOf buscarComitenteCompleted, pstrBusqueda)
                            objProxy.Load(objProxy.buscarClienteEspecificoQuery(strCodigoOYDEncabezado, Program.Usuario, "IdComitenteLectura", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                            If logPreguntarCambio = True Then
                                ValidarCodigosOyDExistentes(pstrIdComitente)
                            Else
                                logPreguntarCambio = True
                            End If
                        End If

                    End If
                End If
            End If

            'Filtro por Código OyD del comitente si busca del detalles
            If (_mLogBuscarClienteDetalle) Then
                If Not Me.DetalleSeleccionado Is Nothing Then
                    If Not pstrIdComitente.Equals(CStr(Me.DetalleSeleccionado.lngIDComitente)) Then
                        If pstrFiltroComitente.Trim.Equals(String.Empty) Then
                            pstrIdComitente = CStr(Me.DetalleSeleccionado.lngIDComitente)
                        Else
                            pstrIdComitente = pstrFiltroComitente
                        End If

                        If Not pstrIdComitente Is Nothing AndAlso Not pstrIdComitente.Trim.Equals(String.Empty) Then
                            objProxy.BuscadorClientes.Clear()
                            objProxy.Load(objProxy.buscarClienteEspecificoQuery(pstrIdComitente, Program.Usuario, "IdComitenteLectura", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                        End If

                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del comitente cambiado
    ''' 
    ''' </summary>
    ''' <param name="pstrFiltroComitente"></param>
    ''' <param name="pstrBusqueda"></param>
    ''' <remarks>Filtro por Numero de documento que quede en el encabezado</remarks>
    Friend Sub buscarComitenteCambiado(Optional ByVal pstrFiltroComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim pstrIdComitente As String = String.Empty

        Try

            'Filtro por Numero de documento si busca del encabezado
            If (_mlogBuscarClienteEncabezado) Then
                If Not Me.EncabezadoSeleccionado Is Nothing Then
                    If Not pstrIdComitente.Equals(Me.EncabezadoSeleccionado.strNumeroDocumento) Then
                        If pstrFiltroComitente.Trim.Equals(String.Empty) Then
                            pstrIdComitente = Me.EncabezadoSeleccionado.strNumeroDocumento
                        Else
                            pstrIdComitente = pstrFiltroComitente
                        End If

                        If Not pstrIdComitente Is Nothing AndAlso Not pstrIdComitente.Trim.Equals(String.Empty) Then
                            objProxy.BuscadorClientes.Clear()
                            'objProxy.Load(objProxy.buscarClientesQuery(pstrIdComitente, "A", String.Empty, "clienteagrupador", Program.Usuario, False, 0), AddressOf buscarComitenteCompleted, pstrBusqueda)
                            objProxy.Load(objProxy.buscarClienteEspecificoQuery(strCodigoOYDEncabezado, Program.Usuario, "IdComitenteLectura", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                        End If

                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente", Me.ToString(), "buscarComitenteCambiado", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub




    ''' <summary>
    ''' Buscar los datos del comitente DE PARTIDAS
    ''' ID caso de prueba:  
    ''' </summary>
    ''' <param name="pstrFiltroComitentePartidas"></param>
    ''' <param name="pstrBusqueda"></param>
    ''' <remarks>Filtro para buscar los clientes de partidas y verificar que solo traiga el dato si encuentra el texto completo</remarks>
    Friend Sub buscarComitentePartidas(Optional ByVal pstrFiltroComitentePartidas As String = "", Optional ByVal pstrBusqueda As String = "")

        Dim pstrIdComitente As String = String.Empty

        Try

            'Filtro por Numero de documento si busca del encabezado
            'If (_mlogBuscarClienteEncabezado) Then
            If Not Me.EncabezadoSeleccionado Is Nothing Then
                If Not pstrIdComitente.Equals(Me.EncabezadoSeleccionado.strNumeroDocumentoClientePartidas) Then
                    If pstrFiltroComitentePartidas.Trim.Equals(String.Empty) Then
                        pstrIdComitente = Me.EncabezadoSeleccionado.strNumeroDocumentoClientePartidas
                    Else
                        pstrIdComitente = pstrFiltroComitentePartidas
                    End If

                    If Not pstrIdComitente Is Nothing AndAlso Not pstrIdComitente.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorClientes.Clear()
                        'objProxy.Load(objProxy.buscarClientesQuery(pstrIdComitente, "A", String.Empty, "clienteagrupador", Program.Usuario, False, 0), AddressOf buscarComitentePartidasCompleted, pstrBusqueda)
                        objProxy.Load(objProxy.buscarClienteEspecificoQuery(strCodOyDPartidas, Program.Usuario, "IdComitenteLectura", Program.HashConexion), AddressOf buscarComitentePartidasCompleted, pstrBusqueda)
                    End If

                End If
            End If
            'End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente partidas", Me.ToString(), "buscarComitentePartidas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del comitente DE PARTIDAS
    ''' ID caso de prueba:  
    ''' </summary>
    ''' <param name="pstrFiltroComitentePartidas"></param>
    ''' <param name="pstrBusqueda"></param>
    ''' <remarks>Filtro para buscar los clientes de partidas y verificar que solo traiga el dato si encuentra el texto completo</remarks>
    Friend Sub buscarComitentePartidasOyD(Optional ByVal pstrFiltroComitentePartidas As String = "", Optional ByVal pstrBusqueda As String = "")

        Dim pstrIdComitente As String = String.Empty

        Try

            'Filtro por Numero de documento si busca del encabezado
            'If (_mlogBuscarClienteEncabezado) Then
            If Not Me.EncabezadoSeleccionado Is Nothing Then
                If Not pstrIdComitente.Equals(Me.EncabezadoSeleccionado.lngIDComitenteClientePartidas) Then
                    If pstrFiltroComitentePartidas.Trim.Equals(String.Empty) Then
                        pstrIdComitente = Me.EncabezadoSeleccionado.lngIDComitenteClientePartidas
                    Else
                        pstrIdComitente = pstrFiltroComitentePartidas
                    End If

                    If Not pstrIdComitente Is Nothing AndAlso Not pstrIdComitente.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorClientes.Clear()
                        objProxy.Load(objProxy.buscarClienteEspecificoQuery(pstrIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitentePartidasCompletedOyD, pstrBusqueda)
                    End If

                End If
            End If
            'End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente partidas", Me.ToString(), "buscarComitentePartidasOyD", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub





    ''' <summary>
    ''' Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' ID caso de prueba:  CP013, CP014 si no existe el código avisar al usuario y no permitir continuar. En caso que exista cargar datos
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            IsBusy = True
            '----------------------------------------------------------------------------------------------------
            'Si busca el cliente del encabezado
            '----------------------------------------------------------------------------------------------------
            If (_mlogBuscarClienteEncabezado) Then
                If lo.Entities.ToList.Count > 0 Then 'CP013
                    If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        EncabezadoSeleccionado.strNombre = String.Empty
                        EncabezadoSeleccionado.strTipoDocumento = String.Empty

                        _mlogBuscarClienteEncabezado = False
                    Else
                        'JCM20160224 VERIFICAR QUE EL NOMBBRE DIGITADO SEA IGUAL AL ENCONTRADO
                        'CP058
                        If EncabezadoSeleccionado.strNumeroDocumento = lo.Entities.ToList.Item(0).NroDocumento Then
                            _EncabezadoSeleccionado.strNombre = lo.Entities.ToList.Item(0).Nombre
                            _EncabezadoSeleccionado.strNumeroDocumento = lo.Entities.ToList.Item(0).NroDocumento
                            _EncabezadoSeleccionado.strTipoDocumento = lo.Entities.ToList.Item(0).CodTipoIdentificacion
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    EncabezadoSeleccionado.strNombre = String.Empty
                    EncabezadoSeleccionado.strNumeroDocumento = String.Empty
                    EncabezadoSeleccionado.strTipoDocumento = String.Empty

                End If
            End If

            '----------------------------------------------------------------------------------------------------
            'Si busca el cliente del detalle
            '----------------------------------------------------------------------------------------------------
            If (_mLogBuscarClienteDetalle) Then
                If DetalleSeleccionado.logCompania = False Then
                    If lo.Entities.ToList.Count > 0 Then
                        If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                            A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el detalle se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            DetalleSeleccionado.lngIDComitente = Nothing
                            DetalleSeleccionado.strNombre = String.Empty
                            DetalleSeleccionado.strNroDocumento = String.Empty
                            _mLogBuscarClienteDetalle = False
                        Else
                            DetalleSeleccionado.strNombre = lo.Entities.ToList.Item(0).Nombre
                            DetalleSeleccionado.strNroDocumento = lo.Entities.ToList.Item(0).NroDocumento
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el detalle no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        DetalleSeleccionado.lngIDComitente = Nothing
                        DetalleSeleccionado.strNombre = String.Empty
                        DetalleSeleccionado.strNroDocumento = String.Empty
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' ID caso de prueba:  CP013, CP014 si no existe el código avisar al usuario y no permitir continuar. En caso que exista cargar datos
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub buscarComitentePartidasCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            IsBusy = True
            '----------------------------------------------------------------------------------------------------
            'Si busca el cliente del encabezado
            '----------------------------------------------------------------------------------------------------
            'If (_mlogBuscarClienteEncabezado) Then
            If lo.Entities.ToList.Count > 0 Then 'CP013
                If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado como cliente de partidas se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    EncabezadoSeleccionado.strNombreclientepartidas = String.Empty

                Else
                    'JCM20160316 VERIFICAR QUE EL NOMBBRE DIGITADO SEA IGUAL AL ENCONTRADO
                    If EncabezadoSeleccionado.strNumeroDocumentoClientePartidas = lo.Entities.ToList.Item(0).NroDocumento Then
                        _EncabezadoSeleccionado.strNombreclientepartidas = lo.Entities.ToList.Item(0).Nombre
                        _EncabezadoSeleccionado.strNumeroDocumentoClientePartidas = lo.Entities.ToList.Item(0).NroDocumento

                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el cliente partidas no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                EncabezadoSeleccionado.strNombreclientepartidas = String.Empty
                EncabezadoSeleccionado.strNumeroDocumentoClientePartidas = String.Empty

            End If
            'End If



        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' ID caso de prueba:  CP013, CP014 si no existe el código avisar al usuario y no permitir continuar. En caso que exista cargar datos
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub buscarComitentePartidasCompletedOyD(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            IsBusy = True
            '----------------------------------------------------------------------------------------------------
            'Si busca el cliente del encabezado
            '----------------------------------------------------------------------------------------------------
            'If (_mlogBuscarClienteEncabezado) Then
            If lo.Entities.ToList.Count > 0 Then 'CP013
                If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado como cliente de partidas se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    EncabezadoSeleccionado.strNombreclientepartidas = String.Empty
                    EncabezadoSeleccionado.lngIDComitenteClientePartidas = String.Empty

                Else
                    'JCM20160316 VERIFICAR QUE EL NOMBBRE DIGITADO SEA IGUAL AL ENCONTRADO
                    If EncabezadoSeleccionado.strNumeroDocumentoClientePartidas = lo.Entities.ToList.Item(0).NroDocumento Then
                        _EncabezadoSeleccionado.strNombreclientepartidas = lo.Entities.ToList.Item(0).Nombre
                        _EncabezadoSeleccionado.strNumeroDocumentoClientePartidas = lo.Entities.ToList.Item(0).NroDocumento
                        _EncabezadoSeleccionado.lngIDComitenteClientePartidas = lo.Entities.ToList.Item(0).CodigoOYD
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el cliente partidas no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                EncabezadoSeleccionado.strNombreclientepartidas = String.Empty
                EncabezadoSeleccionado.strNumeroDocumentoClientePartidas = String.Empty
                EncabezadoSeleccionado.lngIDComitenteClientePartidas = String.Empty

            End If
            'End If



        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub





    ''' <summary>
    ''' Verifica que el código ingresado en el campo compañía contable exista en Encuenta.
    ''' ID caso de prueba:  CP015
    ''' </summary>
    ''' <param name="strCompaniaContable">Código de la compañía contable</param>
    ''' <remarks></remarks>
    Friend Async Sub VerificarExistenciaCompaniaEnCuenta(ByVal strCompaniaContable As String)
        Try
            Dim objRet As InvokeOperation(Of Integer)
            Dim strMsg As String = String.Empty

            objRet = Await mdcProxy.VerificarExistenciaCompaniaEnCuentaSync(EncabezadoSeleccionado.strCompaniaContable, Program.Usuario, Program.HashConexion).AsTask()


            If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then

                'si no existe retorna 0
                If objRet.Value.ToString() = "0" Then
                    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, "El código de compañía contable no existe en Encuenta")
                End If

                If Not String.IsNullOrEmpty(strMsg) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            End If 'If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Verificar la Existencia del código de la compañía en Encuenta", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Verifica que el código ingresado en el campo compañía contable exista en Encuenta.
    ''' ID caso de prueba:  CP015
    ''' </summary>
    ''' <param name="pstrNroDocumento">Código de la compañía contable</param>
    ''' <remarks></remarks>
    Friend Async Sub ObtenerCodigoOyDCliente(ByVal pstrNroDocumento As String, ByVal LogCambiado As Boolean)
        Try
            Dim objRet As InvokeOperation(Of String)
            Dim strMsg As String = String.Empty
            'Dim StrDatos As String

            objRet = Await mdcProxy.ObtenerCodigoOyDCliente(pstrNroDocumento, Program.Usuario, Program.HashConexion).AsTask()

            'objRet = mdcProxy.ObtenerCodigoOyDCliente(pstrNroDocumento, Program.Usuario, Program.HashConexion).AsTask


            If Not String.IsNullOrEmpty(objRet.Value) Then
                'si no existe retorna 0
                If objRet.Value.ToString() = "0" Then
                    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, "El cliente del encabezado no se encontró")
                Else
                    'If strCodigoOYDEncabezado = objRet.Value.ToString() Then
                    'strCodigoOYDEncabezado = objRet.Value.ToString()
                    'End If

                    If LogCambiado = False Then
                        If strCodigoOYDEncabezado = objRet.Value Then
                            strCodigoOYDEncabezado = objRet.Value
                        End If
                        If EncabezadoSeleccionado.strNombre = String.Empty Then
                            strCodigoSeleccionado = strCodigoOYDEncabezado
                        End If
                        buscarComitente(_EncabezadoSeleccionado.strNumeroDocumento)
                    Else
                        If strCodigoSeleccionado = objRet.Value Then
                            strCodigoOYDEncabezado = objRet.Value
                        Else
                            strCodigoOYDEncabezado = strCodigoSeleccionado
                        End If
                        buscarComitenteCambiado(_EncabezadoSeleccionado.strNumeroDocumento)
                    End If
                End If
                If Not String.IsNullOrEmpty(strMsg) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, "El cliente del encabezado no se encontró")
            End If 'If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then

            If Not String.IsNullOrEmpty(strMsg) Then
                _EncabezadoSeleccionado.strNumeroDocumento = String.Empty
                _EncabezadoSeleccionado.strNombre = String.Empty
                _EncabezadoSeleccionado.strTipoDocumento = String.Empty
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Verificar la Existencia del código OyD del cliente", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Verifica que el código ingresado en el campo compañía contable exista en Encuenta.
    ''' ID caso de prueba:  CP015
    ''' </summary>
    ''' <param name="pstrNroDocumento">Código de la compañía contable</param>
    ''' <remarks></remarks>
    Friend Async Sub ObtenerCodigoOyDClientePartidas(ByVal pstrNroDocumento As String)
        Try
            Dim objRet As InvokeOperation(Of String)
            Dim strMsg As String = String.Empty
            'Dim StrDatos As String

            objRet = Await mdcProxy.ObtenerCodigoOyDCliente(pstrNroDocumento, Program.Usuario, Program.HashConexion).AsTask()

            'objRet = mdcProxy.ObtenerCodigoOyDCliente(pstrNroDocumento, Program.Usuario, Program.HashConexion).AsTask


            If Not String.IsNullOrEmpty(objRet.Value) Then

                'si no existe retorna 0
                If objRet.Value.ToString() = "" Then
                    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, "El cliente de partidas no se encontró")
                Else
                    strCodOyDPartidas = objRet.Value
                    buscarComitentePartidas(_EncabezadoSeleccionado.strNumeroDocumento)
                End If

                If Not String.IsNullOrEmpty(strMsg) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, "El cliente de partidas no se encontró")
            End If 'If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then


            If Not String.IsNullOrEmpty(strMsg) Then
                _EncabezadoSeleccionado.strNumeroDocumentoClientePartidas = String.Empty
                _EncabezadoSeleccionado.strNombreclientepartidas = String.Empty
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Verificar la Existencia del código OyD del cliente de partidas", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub



#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaCompanias
            objCB.strTipoDocumento = String.Empty
            objCB.strNumeroDocumento = String.Empty
            objCB.strNombre = String.Empty
            objCB.strTipoCompania = String.Empty
            objCB.strTipoPlazo = String.Empty
            objCB.strParticipacion = String.Empty

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una copia del encabezado activo. 
    ''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>
    Private Function ObtenerRegistroAnterior() As Companias
        Dim objEncabezado As Companias = New Companias

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of Companias)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDCompania = _EncabezadoSeleccionado.intIDCompania
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <summary>
    ''' Validar campos obligatorios antes de actualizar o insertar.
    ''' ID caso de prueba:  CP010
    ''' </summary>
    ''' <history>
    ''' Descripción      : Actualización. Nuevos campos a validar strContabDividendos, dtmApertura y strProveedorPrecios. La fecha de último cierre no es obligatoria y solamente la asigna el sistema
    ''' Fecha            : Agosto 12/2015
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Agosto 12/2015 - Resultado Ok 
    ''' 'JEPM201500812
    ''' </history>
    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                'Tipo Documento
                If (_EncabezadoSeleccionado.strTipoDocumento) = "" Then
                    strMsg = String.Format("{0}{1} + El tipo de documento es un campo requerido.", strMsg, vbCrLf)
                End If

                'Número de Documento
                If (_EncabezadoSeleccionado.strNumeroDocumento) = "" Then
                    strMsg = String.Format("{0}{1} + El número de documento es un campo requerido.", strMsg, vbCrLf)
                End If

                'Nombre
                If (_EncabezadoSeleccionado.strNombre) = "" Then
                    strMsg = String.Format("{0}{1} + El nombre es un campo requerido.", strMsg, vbCrLf)
                End If

                'Tipo Compañía
                If (_EncabezadoSeleccionado.strTipoCompania) = "" Then
                    strMsg = String.Format("{0}{1} + El tipo de compañía es un campo requerido.", strMsg, vbCrLf)
                End If

                'JDCP20170124
                If _EncabezadoSeleccionado.strTipoCompania <> TIPOCOMPANIA_TESORERIA Then
                    'Contabilización Dividendos
                    If (_EncabezadoSeleccionado.strContabDividendos) = "" Then
                        strMsg = String.Format("{0}{1} + El tipo de contabilización dividendos es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If

                'JDCP20170124
                If _EncabezadoSeleccionado.strTipoCompania <> TIPOCOMPANIA_TESORERIA Then
                    'Valida la fecha de apertura
                    If IsNothing(_EncabezadoSeleccionado.dtmApertura) Then
                        strMsg = String.Format("{0}{1} + La fecha de Apertura es un campo requerido.", strMsg, vbCrLf)
                    End If

                    'Proveedor Precio
                    If (_EncabezadoSeleccionado.strProveedorPrecios) = "" Then
                        strMsg = String.Format("{0}{1} + El tipo de proveedor precio es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If

                'Validar Nombre Corto para la compañia, JCM20160210
                If (_EncabezadoSeleccionado.strNombreCorto) = "" Then
                    strMsg = String.Format("{0}{1} + El nombre corto es un campo requerido.", strMsg, vbCrLf)
                End If

                'Validar el gestor cuando la cuenta es omnibus
                If HabilitarCamposOmnibus And (IsNothing(_EncabezadoSeleccionado.intIdGestor) Or _EncabezadoSeleccionado.intIdGestor = 0) Then
                    strMsg = String.Format("{0}{1} + El Gestor es un campo requerido.", strMsg, vbCrLf)
                End If


                'JDCP20170124
                If _EncabezadoSeleccionado.strTipoCompania <> TIPOCOMPANIA_TESORERIA Then
                    'Validar el Nombre de compañia contable, JCM20160222
                    If (_EncabezadoSeleccionado.strCompaniaContable) = "" Then
                        strMsg = String.Format("{0}{1} + El nombre de la compañia contable es un campo requerido.", strMsg, vbCrLf)
                    End If

                    'Validar el Nombre del código de la super, JCM20160222
                    If (_EncabezadoSeleccionado.strCodigoSuper) = "" Then
                        strMsg = String.Format("{0}{1} + El código superintencia es un campo requerido.", strMsg, vbCrLf)
                    End If

                    'Validar que el nombre escogido en el cliente no sea el mismo cliente partidas, JCM20160311
                    'CP066
                    If (_EncabezadoSeleccionado.strTipoCompania = "FIC" AndAlso _EncabezadoSeleccionado.strNumeroDocumento = _EncabezadoSeleccionado.strNumeroDocumentoClientePartidas) Then
                        strMsg = String.Format("{0}{1} + El código OYD del cliente y el código cliente partidas no pueden ser el mismo.", strMsg, vbCrLf)
                    End If

                    'Validar que si esta escogida la opción comisión administración se debe escoger un tipo de comisión. JCM20160315
                    'CP067
                    If (_EncabezadoSeleccionado.strComisionAdministracion = "S" And _EncabezadoSeleccionado.strTipoComision = String.Empty) Then
                        strMsg = String.Format("{0}{1} + El tipo de comisión se debe escoger.", strMsg, vbCrLf)
                    End If

                    'Validar que sí selecciona que se maneja valor de unidad, se debe ingresar se debe escoger una fecha de inicio
                    'CP071
                    If (_EncabezadoSeleccionado.logManejaValorUnidad = True) And (_EncabezadoSeleccionado.dtmFechaInicio Is Nothing) Then
                        strMsg = String.Format("{0}{1} + Debe escoger una fecha de inicio para la compañía.", strMsg, vbCrLf)
                    End If

                    'Validar que sí se selecciona que se manjea valor de unidad, se debe ingresar el valor de inicio del fondo
                    'CP071
                    If (_EncabezadoSeleccionado.logManejaValorUnidad = True) And (_EncabezadoSeleccionado.dblValorUnidadInicial Is Nothing) Then
                        strMsg = String.Format("{0}{1} + Debe ingresar un valor para la unidad inicial mayor que 0.", strMsg, vbCrLf)
                    End If

                    'JCM20170810, se ajsta la validación para que la moneda sea obligatoria no importa el tipo de compania

                    ' If ((_EncabezadoSeleccionado.strTipoCompania = "APT") Or (_EncabezadoSeleccionado.strTipoCompania = "FIC")) AndAlso (_EncabezadoSeleccionado.intIDMoneda Is Nothing) Then
                    If (_EncabezadoSeleccionado.intIDMoneda Is Nothing) Then
                        strMsg = String.Format("{0}{1} + Debe escoger una moneda.", strMsg, vbCrLf)
                    End If

                End If

            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar un registro", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            If EncabezadoSeleccionado.lngIDBancos Is Nothing Then
                EncabezadoSeleccionado.lngIDBancos = 0
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Validar que no se guarde en el detalle la misma compañia del encabezado.
    ''' ID caso de prueba:  CP044
    ''' Se debe llamar en la finalizacion de la busqueda de las compañias.
    ''' </summary>
    ''' <history>
    ''' Descripción      : Se verifica que la compañia que se guarde en el detalle la misma compañia que se encuentra en el encabezado
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Public Function ValidarCompaniaRepetida(ByVal pCompania_Insertar As Integer) As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL COMPAÑIA
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If pCompania_Insertar = _EncabezadoSeleccionado.intIDCompania Then
                    strMsg = String.Format("{0}{1} + No puede ingresar la misma compañia principal como detalle.", strMsg, vbCrLf)
                End If
            End If
            If strMsg.Equals(String.Empty) Then

                logResultado = True
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique la siguiente inconsistencia antes de continuar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)

    End Function

    ''' <summary>
    ''' Validar que cuando se realiza una importación de la penalidad no se ingrese una compañia distinta a la del encabezado 
    ''' ID caso de prueba:  CP010
    ''' </summary>
    ''' <history>
    ''' Descripción      : Verificar que se ingrese la compañia en la importación igual a la del encabezado.
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Public Function ValidarCompaniaPenalidad(ByVal Compania_Penalidad As Integer, ByVal Fila_Insertar As Integer) As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL COMPAÑIA
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If Compania_Penalidad <> _EncabezadoSeleccionado.intIDCompania Then
                    strMsg = String.Format("{0}{1} + La compañia en el archivo de importación no es la misma a la compania actual." & " " & "Fila:" & " " & Fila_Insertar, strMsg, vbCrLf)
                End If
            End If
            If strMsg.Equals(String.Empty) Then
                logResultado = True
            Else
                logResultado = False
                'A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique la siguiente inconsistencia antes de continuar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarCompaniaPenalidad", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)

    End Function

    ''' <summary>
    ''' Mostrar toda la información de acuerdo a los datos guardados
    ''' ID caso de prueba:  CP010
    ''' </summary>
    ''' <history>
    ''' Descripción      : Metodo para mostrar toda la información correcta (De acuerdo al comportamiento de los controles dependientes)
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Private Sub InicializarEncabezado()
        'manejo NULL's
        '_EncabezadoSeleccionado.logManejaValorUnidad = CType(IIf(_EncabezadoSeleccionado.logManejaValorUnidad.HasValue, _EncabezadoSeleccionado.logManejaValorUnidad.Value, False), Boolean?)
        If Not IsNothing(_EncabezadoSeleccionado.logManejaValorUnidad) Then


            HabilitarTabEstructura = CType(IIf(_EncabezadoSeleccionado.logManejaValorUnidad.Value, Visibility.Visible, Visibility.Collapsed), Visibility)
            HabilitarTabComisiones = CType(IIf(_EncabezadoSeleccionado.logManejaValorUnidad.Value, Visibility.Visible, Visibility.Collapsed), Visibility)
            'If _EncabezadoSeleccionado.strParticipacion <> PARTICIPACION_CONSOLIDACLASES Then
            HabilitarTabTesoreria = CType(IIf(_EncabezadoSeleccionado.logManejaValorUnidad.Value, Visibility.Visible, Visibility.Collapsed), Visibility)
            'Else
            '    HabilitarTabTesoreria = Visibility.Collapsed
            'End If
            HabilitarTabLimites = CType(IIf(_EncabezadoSeleccionado.logManejaValorUnidad.Value, Visibility.Visible, Visibility.Collapsed), Visibility)
            VerificarHabilitacionCuentasOmnibus()
            HabilitarTabDetalle = CType(IIf(_EncabezadoSeleccionado.logManejaValorUnidad.Value, Visibility.Collapsed, Visibility.Visible), Visibility)
            FocoTabDetalle = CBool(IIf(CBool(_EncabezadoSeleccionado.logManejaValorUnidad = False), True, False))
            FocoTabEstructura = CBool(IIf(CBool(_EncabezadoSeleccionado.logManejaValorUnidad = True), True, False))
        Else
            HabilitarTabEstructura = Visibility.Collapsed
            HabilitarTabComisiones = Visibility.Collapsed
            HabilitarTabTesoreria = Visibility.Collapsed
            HabilitarTabLimites = Visibility.Collapsed
            HabilitarTabClientesAutorizados = Visibility.Collapsed
            HabilitarTabDetalle = Visibility.Visible
            FocoTabDetalle = True
            FocoTabEstructura = False
        End If




        'HabilitarTabClientesAutorizados = CType(IIf(_EncabezadoSeleccionado.logManejaValorUnidad.Value, Visibility.Collapsed, Visibility.Visible), Visibility)
        MostrarManejaValorUnidad = CType(IIf(_EncabezadoSeleccionado.strTipoCompania = "APT" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "CC" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "EF" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "FIC" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "PA", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarManejaOmnibus = CType(IIf(_EncabezadoSeleccionado.strTipoCompania = "APT" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "CC" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "EF" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "FIC" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "PA", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarCamposPrincipales = CType(IIf(_EncabezadoSeleccionado.strTipoCompania = "APT" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "CC" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "EF" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "FIC" Or
                                            _EncabezadoSeleccionado.strTipoCompania = "PA", Visibility.Visible, Visibility.Collapsed), Visibility)


        MostrarManejaComisionMin = CType(IIf(_EncabezadoSeleccionado.strComisionMinima = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarOtroPrefijo = CType(IIf(_EncabezadoSeleccionado.strIdentificadorCuenta = "O", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarPactoPermanencia = CType(IIf(_EncabezadoSeleccionado.strTipoPlazo = "A", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarDatosPermanencia = CType(IIf((_EncabezadoSeleccionado.strTipoPlazo = "A" And _EncabezadoSeleccionado.strPactoPermanencia = "S") Or _EncabezadoSeleccionado.strTipoPlazo = "CF", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarCamposPenalidadFactor = CType(IIf(_EncabezadoSeleccionado.strPenalidad = "F" And ((_EncabezadoSeleccionado.strTipoPlazo = "A" And _EncabezadoSeleccionado.strPactoPermanencia = "S") Or _EncabezadoSeleccionado.strTipoPlazo = "CF"), Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarCamposPenalidadTabla = CType(IIf(_EncabezadoSeleccionado.strPenalidad = "T" And ((_EncabezadoSeleccionado.strTipoPlazo = "A" And _EncabezadoSeleccionado.strPactoPermanencia = "S") Or _EncabezadoSeleccionado.strTipoPlazo = "CF"), Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarFCP = CType(IIf(_EncabezadoSeleccionado.strTipoPlazo = "C", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarFechaVencimiento = CType(IIf(_EncabezadoSeleccionado.strTipoPlazo = "C", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarInscritoRNVE = CType(IIf(_EncabezadoSeleccionado.strTipoPlazo = "C" Or _EncabezadoSeleccionado.strTipoPlazo = "CF", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarCompartimentos = CType(IIf(_EncabezadoSeleccionado.strFondoCapitalPrivado = "S" And _EncabezadoSeleccionado.strTipoPlazo = "C", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarISIN = CType(IIf(_EncabezadoSeleccionado.strInscritoRNVE = "S" And (_EncabezadoSeleccionado.strTipoPlazo = "C" Or _EncabezadoSeleccionado.strTipoPlazo = "CF"), Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarTipoParticipacion = CType(IIf(_EncabezadoSeleccionado.strTipoPlazo = "A" Or _EncabezadoSeleccionado.strTipoPlazo = "CF", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarTipoComision = CType(IIf(_EncabezadoSeleccionado.strComisionAdministracion = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarTasaComision = CType(IIf((_EncabezadoSeleccionado.strTipoComision = "TE" Or _EncabezadoSeleccionado.strTipoComision = "TN") And _EncabezadoSeleccionado.strComisionAdministracion = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarValorComision = CType(IIf(_EncabezadoSeleccionado.strTipoComision = "V" And _EncabezadoSeleccionado.strComisionAdministracion = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarOpcionesOtrasComisiones = CType(IIf(_EncabezadoSeleccionado.strOtrasComisiones = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarFactorEntrada = CType(IIf(_EncabezadoSeleccionado.strEntrada = "F" And _EncabezadoSeleccionado.strOtrasComisiones = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarValorEntrada = CType(IIf(_EncabezadoSeleccionado.strEntrada = "V" And _EncabezadoSeleccionado.strOtrasComisiones = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarFactorSalida = CType(IIf(_EncabezadoSeleccionado.strSalida = "F" And _EncabezadoSeleccionado.strOtrasComisiones = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarValorSalida = CType(IIf(_EncabezadoSeleccionado.strSalida = "V" And _EncabezadoSeleccionado.strOtrasComisiones = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        'MostrarBuscadorOyd = CType(IIf(_EncabezadoSeleccionado.strParticipacion = "CC" Or _EncabezadoSeleccionado.strParticipacion = "CI" Or _EncabezadoSeleccionado.strParticipacion = "C", Visibility.Visible, Visibility.Collapsed), Visibility)
        'JCM 20170128, SE COMENTA PARA SOLO MOSTRAR EL BUSCADOR DE CODIGOS OYD
        MostrarBuscadorOyd = Visibility.Visible
        MostrarBuscadorCompanias = CType(IIf(_EncabezadoSeleccionado.strParticipacion = "CI", Visibility.Visible, Visibility.Collapsed), Visibility)
        mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
        MostrarCamposGrid = CType(IIf(_EncabezadoSeleccionado.strTipoPlazo = "A", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarGridAutorizaciones = CType(IIf(_EncabezadoSeleccionado.strTipoAutorizados = "C", Visibility.Visible, Visibility.Collapsed), Visibility)
        'Mostrar el buscador para el cliente partidas dependiendo del tipo de compañias:FIC
        'CP060
        MostrarClientePartidas = CType(IIf(_EncabezadoSeleccionado.strTipoCompania = "FIC", Visibility.Visible, Visibility.Collapsed), Visibility)
        VerificarHabilitarDeshabilitar()
        strNroDocumentoActual = _EncabezadoSeleccionado.strNumeroDocumento
        MostrarTipoComisionGestor = CType(IIf(_EncabezadoSeleccionado.strComisionGestor = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarTasaComisionGestor = CType(IIf((_EncabezadoSeleccionado.strTipoComisionGestor = "TE" Or _EncabezadoSeleccionado.strTipoComisionGestor = "TN") And _EncabezadoSeleccionado.strComisionGestor = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarValorComisionGestor = CType(IIf(_EncabezadoSeleccionado.strTipoComisionGestor = "V" And _EncabezadoSeleccionado.strComisionGestor = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        strTipoAgrupacion = CStr(IIf(_EncabezadoSeleccionado.strParticipacion = "CC", "todoslosreceptores&**&" & _EncabezadoSeleccionado.strNumeroDocumento, "todoslosreceptores"))
        If logHabilitarCompaniaPasivaTesoreria = True Then
            MostrarControlesTipoCompania = Visibility.Collapsed
        Else
            MostrarControlesTipoCompania = Visibility.Visible
        End If
        MostrarTablaAcumuladoComision = CType(IIf(_EncabezadoSeleccionado.strTipoComision = "TR", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarGridAcumuladoComisiones = CType(IIf(_EncabezadoSeleccionado.strTipoComision = "TR", Visibility.Visible, Visibility.Collapsed), Visibility)
        'MostrarComboTipoTabla = CType(IIf(_EncabezadoSeleccionado.strTipoComision = "TR", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarComboTipoTabla = Visibility.Visible
        MostrarIvaComisionAdmon = CType(IIf(_EncabezadoSeleccionado.strTipoCompania = "APT" And _EncabezadoSeleccionado.strComisionAdministracion = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarIvaComisionGestor = CType(IIf(_EncabezadoSeleccionado.strTipoCompania = "FIC" And _EncabezadoSeleccionado.strComisionGestor = "S", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarPeriodoGracia = CType(IIf(MostrarDatosPermanencia = Visibility.Visible And _EncabezadoSeleccionado.strFondoRenovable <> "N", Visibility.Visible, Visibility.Collapsed), Visibility)
        MostrarDatosComisionAdmon = CType(IIf(_EncabezadoSeleccionado.strComisionAdministracion = "S", IIf(_EncabezadoSeleccionado.strTipoComision <> "", Visibility.Visible, Visibility.Collapsed), Visibility.Collapsed), Visibility)
    End Sub

    ''' <summary>
    ''' Verificar si se habilitan o no los controles del tab Estructura
    ''' ID caso de prueba:  CP050
    ''' </summary>
    ''' <history>
    ''' Descripción      : En este metodo se verifica si la fecha de ultimo cierre no es null, si lo es se deben bloquear los controles, sino se pregunta
    '''                    de acuerdo al estado de la variable Habilitarncabezado
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Private Sub VerificarHabilitarDeshabilitar()
        If IsNothing(_EncabezadoSeleccionado.dtmUltimoCierre) Then
            If HabilitarEncabezado = True Then
                HabilitarCamposlectura = True
            Else
                HabilitarCamposlectura = False
            End If
        Else
            HabilitarCamposlectura = False
        End If
    End Sub

    ''' <summary>
    ''' Verificar que debe suceder cuando se cambie el Tipo de Participación 
    ''' ID caso de prueba:  CP031
    ''' </summary>
    ''' <history>
    ''' Descripción      : En este método se verifica que acción se debe tomar de acuerdo al Tipo de Participación.
    '''                    
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Private Sub ValidarCambioParticipacion(ByVal pEncabezadoParticipacion As String, ByVal pOpcion As String)
        Dim strMsg As String = String.Empty
        Dim logValidarCambioParticipacion As Boolean = False

        If mobjTipoParticipacion <> "" Then

            Select Case mobjTipoParticipacion
                Case "CC"
                    If pEncabezadoParticipacion <> "CC" Then
                        'ValidarDetalle
                        If pEncabezadoParticipacion <> "CI" Then
                            If ValidarDatosExistentes("BorrarCompanias") = True Then
                                'logValidarCambioParticipacion = True
                                strMsg = "Ya existen compañias, si continua serán eliminados ¿Desea Continuar?"
                                A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntaDetalleEspecificoCodigos)
                            Else
                                mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                            End If
                        Else
                            mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                            'Preguntar
                        End If
                    End If
                Case "CI"
                    If pEncabezadoParticipacion <> "CI" Then
                        'ValidarDetalle
                        If pEncabezadoParticipacion <> "CC" Then
                            If ValidarDatosExistentes("BorrarCompanias") = True Then
                                'logValidarCambioParticipacion = True
                                strMsg = "Ya existen compañias, si continua serán eliminados ¿Desea Continuar?"
                                A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntaDetalleEspecificoCodigos)
                            Else
                                mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                            End If
                        Else
                            mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                            'Preguntar
                        End If
                    End If
                Case "C"
                    If pEncabezadoParticipacion <> "C" And pEncabezadoParticipacion <> "CC" Then
                        'ValidarDetalle
                        If pEncabezadoParticipacion <> "I" Then
                            If ValidarDatosExistentes("BorrarCodigosOyD") Then
                                strMsg = "Ya existen códigos oyd, si continua serán eliminados ¿Desea Continuar?"
                                A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntaDetalleEspecificoCompanias)
                                'logValidarCambioParticipacion = True
                                'Preguntar
                            Else
                                mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                            End If
                        Else
                            mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                        End If
                    End If
                Case "I"
                    If pEncabezadoParticipacion <> "I" Then
                        'ValidarDetalle
                        If pEncabezadoParticipacion <> "C" Or pEncabezadoParticipacion <> "CC" Then
                            If ValidarDatosExistentes("BorrarCodigosOyD") Then
                                strMsg = "Ya existen códigos oyd, si continua serán eliminados ¿Desea Continuar?"
                                A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntaDetalleEspecificoCompanias)
                                'logValidarCambioParticipacion = True
                                'Preguntar
                            Else
                                mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                            End If
                        Else
                            mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                        End If
                    End If
            End Select

        Else
            'If pEncabezadoParticipacion = "I" Then
            '    If ValidarDatosExistentes("BorrarCodigosOyD") = True Then
            '        'logValidarCambioParticipacion = True
            '        strMsg = "Ya existen códigos oyd, si continua serán eliminados ¿Desea Continuar?"
            '        A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntaDetalleEspecificoCodigos2)
            '    End If
            'End If
            mobjTipoParticipacion = pEncabezadoParticipacion
        End If
    End Sub

    ''' <summary>
    ''' Acción dependiendo del tipo de participación
    ''' ID caso de prueba:  CP010
    ''' </summary>
    ''' <history>
    ''' Descripción      : En este método se toma la respuesta del usuario de acuerdo a si desea continuar o no, si lo hace se eliminaran los datos especificados en la función  
    '''                    BorrarDetalleEspecifico (Elimninar solo codigos)    '''                    
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Private Sub TerminoPreguntaDetalleEspecificoCodigos(ByVal sender As Object, e As System.EventArgs)
        Try

            If IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    BorrarDetalleEspecifico("BorrarCodigosOyD")
                    mobjCambioParticipacion = True
                    mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                    NoValidarCambio = False
                Else
                    NoValidarCambio = True
                    _EncabezadoSeleccionado.strParticipacion = mobjTipoParticipacion
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de borrar el detalle",
             Me.ToString(), "TerminoPreguntaDetalleEspecificoCodigos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Acción dependiendo del tipo de participación
    ''' ID caso de prueba:  CP010
    ''' </summary>
    ''' <history>
    ''' Descripción      : En este método se toma la respuesta del usuario de acuerdo a si desea continuar o no, si lo hace se eliminaran los datos especificados en la función  
    '''                    BorrarDetalleEspecifico (Elimninar solo codigos)    '''                    
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Private Sub TerminoPreguntaDetalleEspecificoCodigos2(ByVal sender As Object, e As System.EventArgs)
        Try

            If IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    BorrarDetalleEspecifico("BorrarCodigosOyD")
                    mobjCambioParticipacion = True
                    mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                    NoValidarCambio = False
                Else
                    NoValidarCambio = True
                    _EncabezadoSeleccionado.strParticipacion = ""
                    mobjTipoParticipacion = ""
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de borrar el detalle",
             Me.ToString(), "TerminoPreguntaDetalleEspecificoCodigos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub







    ''' <summary>
    ''' Acción dependiendo del tipo de participación
    ''' ID caso de prueba:  CP010
    ''' </summary>
    ''' <history>
    ''' Descripción      : En este método se toma la respuesta del usuario de acuerdo a si desea continuar o no, si lo hace se eliminaran los datos especificados en la función  
    '''                   BorrarDetalleEspecifico (eliminar solo companias)
    '''                     
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Private Sub TerminoPreguntaDetalleEspecificoCompanias(ByVal sender As Object, e As System.EventArgs)
        Try

            If Not IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    BorrarDetalleEspecifico("BorrarCompanias")
                    mobjCambioParticipacion = True
                    mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                    NoValidarCambio = False
                Else
                    NoValidarCambio = True
                    _EncabezadoSeleccionado.strParticipacion = mobjTipoParticipacion
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de borrar el detalle",
             Me.ToString(), "TerminoPreguntaDetalleEspecificoCompanias", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Acción dependiendo del tipo de participación
    ''' ID caso de prueba:  CP010
    ''' </summary>
    ''' <history>
    ''' Descripción      : En este método se toma la respuesta del usuario de acuerdo a si desea continuar o no, si lo hace se eliminaran los datos especificados en la función  
    '''                   BorrarDetalleEspecifico (eliminar todo)
    '''                     
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Private Sub TerminoPreguntaDetalleEspecificoTodos(ByVal sender As Object, e As System.EventArgs)
        Try

            If IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    BorrarDetalleEspecifico("BorrarTodo")
                    mobjCambioParticipacion = True
                    mobjTipoParticipacion = _EncabezadoSeleccionado.strParticipacion
                    NoValidarCambio = False
                Else
                    NoValidarCambio = True
                    _EncabezadoSeleccionado.strParticipacion = mobjTipoParticipacion
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de borrar el detalle",
             Me.ToString(), "TerminoPreguntaDetalleEspecificoTodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Mostrar el bótón importar cuando se haya guardado la compañia previamente
    ''' ID caso de prueba:  CP010
    ''' </summary>
    ''' <history>
    ''' Descripción      : Solo habilitar el botón importar cuando la compañia se haya grabado previamente.
    '''                  
    '''                     
    ''' Fecha            : Enero 29/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' 'JCM20160129
    ''' </history>
    Public Function MostrarCampoImportar() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL COMPAÑIA
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If HabilitarBotonImportar = Visibility.Collapsed Then
                    strMsg = String.Format("{0}{1} + Ocultar el botón.", strMsg, vbCrLf)
                End If
            Else
                If HabilitarBotonImportar = Visibility.Collapsed Then
                    strMsg = String.Format("{0}{1} + Ocultar el botón.", strMsg, vbCrLf)
                End If
            End If
            If strMsg.Equals(String.Empty) Then
                logResultado = True
            Else
                logResultado = False
                'A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique la siguiente inconsistencia antes de continuar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "MostrarCampoImportar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)

    End Function

    ''' <summary>
    ''' Sub para verificar los datos del detalle vs el encabezado en PP
    ''' ID caso de prueba:  CP052
    ''' </summary>
    ''' <history>
    ''' Descripción      : Verificar si en el caso que el Tipo de Compañia sea PP y exista un detalle, verificar si el mismo cliente del encabezado exste en el detalle 
    '''                  y realizar la advertencia pertinente.    '''                     
    ''' Fecha            : Febrero 11/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Febrero 11/2016 - Resultado Ok 
    ''' 'JCM20160211
    ''' </history>
    Public Function ValidarCodigosCompaniaTipoCompania() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL COMPAÑIA
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.strTipoCompania = "PP" Then
                    If Not IsNothing(_EncabezadoSeleccionado.strNumeroDocumento) And _EncabezadoSeleccionado.strNumeroDocumento <> "" Then
                        If Not IsNothing(_ListaDetalle) Then
                            Dim intEncotradoCodigo As Integer
                            intEncotradoCodigo = (From item In _ListaDetalle Where item.strNroDocumento = _EncabezadoSeleccionado.strNumeroDocumento).Count

                            If intEncotradoCodigo = 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje("Tenga en cuenta que el documento del encabezado no se encuentra en el detalle " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Recuerde ingresar el documento del encabezado en el detalle " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        If Not IsNothing(_ListaDetalle) Then
                            Dim intEncotradoCodigo As Integer
                            intEncotradoCodigo = (From item In _ListaDetalle Where item.strNroDocumento = _EncabezadoSeleccionado.strNumeroDocumento).Count

                            If intEncotradoCodigo = 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje("Tenga en cuenta que el documento del encabezado no se encuentra en el detalle " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Recuerde ingresar el documento del encabezado en el detalle " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarCodigosCompaniaTipoCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)

    End Function

    'JDCP20170124
    Private Sub CargarTipoCompaniaValidas(ByVal plogHabilitar As Boolean)
        Try
            If Not IsNothing(ListaTipoCompaniaCompleta) Then
                Dim objListaNueva As New List(Of OYDUtilidades.ItemCombo)
                For Each li In ListaTipoCompaniaCompleta
                    objListaNueva.Add(li)
                Next

                ListaTipoCompania = objListaNueva
            Else
                ListaTipoCompania = Nothing
            End If

            'If plogHabilitar Then
            '    If Not IsNothing(ListaTipoCompaniaEdicion) Then
            '        Dim objListaNueva As New List(Of OYDUtilidades.ItemCombo)
            '        For Each li In ListaTipoCompaniaEdicion
            '            objListaNueva.Add(li)
            '        Next

            '        ListaTipoCompania = objListaNueva
            '    Else
            '        ListaTipoCompania = Nothing
            '    End If
            'Else
            '    If Not IsNothing(ListaTipoCompaniaCompleta) Then
            '        Dim objListaNueva As New List(Of OYDUtilidades.ItemCombo)
            '        For Each li In ListaTipoCompaniaCompleta
            '            objListaNueva.Add(li)
            '        Next

            '        ListaTipoCompania = objListaNueva
            '    Else
            '        ListaTipoCompania = Nothing
            '    End If
            'End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los tipos de compañia validos.", Me.ToString(), "CargarTipoCompaniaValidas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Procedimiento que se ejecuta cuando finaliza la ejecución de una actualización a la base de datos
    ''' </summary>
    Public Overrides Async Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Dim strMsg As String = String.Empty

        Try
            IsBusy = False
            If So.HasError Then
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If Not So.Error Is Nothing Then
                        strMsg = So.Error.Message
                    End If
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.obtenerMensajeValidacion(strMsg, So.UserState.ToString, True), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                ' Marcar los cambios como rechazados
                mdcProxy.RejectChanges()

                If So.UserState.Equals(ValoresUserState.Borrar.ToString) Then
                    ' Cuando se elimina un registro, el método RejectChanges lo vuelve a adicionar a la lista pero al final no en la posición original
                    _ListaEncabezadoPaginada.MoveToLastPage()
                    _ListaEncabezadoPaginada.MoveCurrentToLast()
                    MyBase.CambioItem("ListaEncabezadoPaginada")
                End If

                So.MarkErrorAsHandled()
            Else
                HabilitarEncabezado = False
                HabilitarEstado = False
                HabilitarTipoCompania = False
                HabilitarCamposEdicion = False
                HabilitarCamposGestor = False
                HabilitarCamposMoneda = False
                HabilitarEdicionDetalle = False
                HabilitarCampoActiva = False
                HabilitarBotonImportar = Visibility.Visible
                logParametrosPorDefecto = False

                MyBase.TerminoSubmitChanges(So)
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del resultado de la actualización de los datos", Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
            If Not So Is Nothing Then
                If So.HasError Then
                    So.MarkErrorAsHandled()
                End If
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Sub de Termino de Traer Companias
    ''' </summary>
    ''' <history>
    ''' Descripción      : Termina de trear compañias                    
    ''' Fecha            : Febrero 11/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Febrero 11/2016 - Resultado Ok 
    ''' 'JCM20160211
    ''' </history>


    Private Sub TerminoTraerCompanias(ByVal lo As LoadOperation(Of Companias))
        If Not lo.HasError Then
            If mdcProxy.Companias.Count > 0 Then
                Companias = mdcProxy.Companias.ToList
                IsBusy = False
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro de Compañías, por favor ingrese por lo menos un registro en el maestro de Companias", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

            End If
        Else
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Companias",
                                             Me.ToString(), "TerminoTraerCompanias", Application.Current.ToString(), Program.Maquina, lo.Error)

        End If
        IsBusy = False
    End Sub
#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of Companias)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarCompaniasPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjEncabezadoPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjEncabezadoPorDefecto = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de Companias
    ''' ID caso de prueba:  CP003, CP004, CP049
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrTipoDocumento As String = "",
                                               Optional ByVal pstrNumeroDocumento As String = "",
                                               Optional ByVal pstrNombre As String = "",
                                               Optional ByVal pstrTipoCompania As String = "",
                                               Optional ByVal pstrTipoPlazo As String = "",
                                               Optional ByVal pstrParticipacion As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of Companias)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.Companias.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarCompaniasSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarCompaniasSyncQuery(pstrTipoDocumento:=pstrTipoDocumento,
                                                                                  pstrNumeroDocumento:=pstrNumeroDocumento,
                                                                                  pstrNombre:=pstrNombre,
                                                                                  pstrTipoCompania:=pstrTipoCompania,
                                                                                  pstrTipoPlazo:=pstrTipoPlazo,
                                                                                  pstrParticipacion:=pstrParticipacion,
                                                                                  pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEncabezado = mdcProxy.Companias

                    If objRet.Entities.Count > 0 Then
                        If Not plogFiltrar Then
                            MyBase.ConfirmarBuscar()
                        End If
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de Companias ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function


    ''' <summary>
    ''' Carga el valor del parámetro CF_UTILIZACALCULOSFINANCIEROS -- 09/02/2016
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub TerminotraerparametroCF(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminotraerparametroCF", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            If Not IsNothing(obj.UserState) Then
                If obj.UserState.ToString = PARAM_STR_CALCULOS_FINANCIEROS Then
                    If obj.Value = "SI" Then
                        TieneParametro = True
                    Else
                        TieneParametro = False
                    End If
                ElseIf obj.UserState.ToString = PARAM_CF_UTILIZAPASIVA_TESORERIA_A2 Then
                    If obj.Value = "SI" Then
                        logHabilitarCompaniaPasivaTesoreria = True
                    Else
                        logHabilitarCompaniaPasivaTesoreria = False
                    End If
                End If
            End If
        End If



    End Sub

    'JDCP20170124
    Private Sub TerminoConsultarCombosCondicional(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then

                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    If Not IsNothing(lo.UserState) Then
                        If lo.UserState.ToString = "TIPOCOMPANIA" Then
                            If objDiccionario.ContainsKey("TIPOCOMPANIA") Then
                                ListaTipoCompaniaCompleta = objDiccionario("TIPOCOMPANIA")
                                ListaTipoCompania = objDiccionario("TIPOCOMPANIA")
                            Else
                                ListaTipoCompaniaCompleta = Nothing
                                ListaTipoCompania = Nothing
                            End If

                            If objDiccionario.ContainsKey("TIPOCOMPANIAEDICION") Then
                                ListaTipoCompaniaEdicion = objDiccionario("TIPOCOMPANIAEDICION")
                            Else
                                ListaTipoCompaniaEdicion = Nothing
                            End If


                            If objDiccionario.ContainsKey("TIPOCOMPANIASINCL") Then
                                ListaTipoCompaniaBusqueda = objDiccionario("TIPOCOMPANIASINCL")
                            Else
                                ListaTipoCompaniaBusqueda = Nothing
                            End If
                        Else
                            'JCM20170213
                            If lo.UserState.ToString = "TIPOPLAZO_PASIVA" Then
                                If objDiccionario.ContainsKey("TIPOPLAZO") Then
                                    ListaTipoPlazoCompleta = objDiccionario("TIPOPLAZO")
                                Else
                                    ListaTipoPlazoCompleta = Nothing
                                End If

                                If objDiccionario.ContainsKey("TIPOPLAZO") Then
                                    ListaTipoPlazoBusqueda = objDiccionario("TIPOPLAZO")
                                Else
                                    ListaTipoPlazoBusqueda = Nothing
                                End If

                            End If
                            If lo.UserState.ToString = "TIPOPARTICIPACION_PASIVA" Then
                                If objDiccionario.ContainsKey("TIPOPLAZO") Then
                                    ListaTipoParticipacion = objDiccionario("TIPOPLAZO")
                                Else
                                    ListaTipoParticipacion = Nothing
                                End If

                                If objDiccionario.ContainsKey("TIPOPARTICIPACION") Then
                                    ListaTipoParticipacion = objDiccionario("TIPOPARTICIPACION")
                                Else
                                    ListaTipoParticipacion = Nothing
                                End If
                            End If



                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoConsultarCombosCondicional2(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then

                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    If Not IsNothing(lo.UserState) Then
                        If lo.UserState.ToString = "TIPOPARTICIPACION_PASIVA" Then
                            If objDiccionario.ContainsKey("TIPOPLAZO") Then
                                ListaTipoParticipacion = objDiccionario("TIPOPLAZO")
                            Else
                                ListaTipoParticipacion = Nothing
                            End If

                            If objDiccionario.ContainsKey("TIPOPLAZO") Then
                                ListaTipoParticipacion = objDiccionario("TIPOPLAZO")
                            Else
                                ListaTipoParticipacion = Nothing
                            End If
                        End If
                    End If

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusy = False

    End Sub

#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"
    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle
    ''' </summary>
    Private Async Sub ConsultarDetallePorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of CompaniasCodigos)

            objRet = Await dcProxy.Load(dcProxy.ConsultarCompaniasCodigosPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Me.ToString(), "ConsultarDetallePorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetallePorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetallePorDefecto = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Me.ToString(), "consultarDetallePorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Sub para consultar los detalles de las autorizaciones
    ''' </summary>
    ''' <history>
    ''' Descripción      : Sub para consultar los valores por defecto para el detalle de las autorizaciones                  
    ''' Fecha            : Mayo 16/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Mayo 16/2016 - Resultado Ok 
    ''' 'JCM20160516
    ''' </history>

    Private Async Sub ConsultarDetalleAutorizacionesPorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of CompaniasAutorizaciones)

            objRet = Await dcProxy.Load(dcProxy.ConsultarCompaniasAutorizacionesPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto del detalle de autorizaciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle de autorizaciones.", Me.ToString(), "ConsultarDetalleAutorizacionesPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetalleAutorizacionesPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetalleAutorizacionesPorDefecto = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle de autorizaciones.", Me.ToString(), "ConsultarDetalleAutorizacionesPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub




    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks></remarks>
    Public Async Function ConsultarDetalle(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of CompaniasCodigos)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CompaniasCodigos Is Nothing Then
                mdcProxy.CompaniasCodigos.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarCompaniasCodigosSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la compañía pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la compañía.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                End If
            End If

            logResultado = True
            If Not IsNothing(ListaDetalle) Then
                If ListaDetalle.Count >= 1 Then
                    If Not IsNothing(_EncabezadoSeleccionado) Then
                        strCodigoOYDEncabezado = CStr(IIf(_EncabezadoSeleccionado.strParticipacion = "C", ListaDetalle.Item(0).lngIDComitente, ""))
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de la compañía seleccionada.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Procedimiento que se encarga de consultar todos los codigos OyD relacionados con el cliente 
    ''' Se debe llamar desde el procedimiento BuscarComitente
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP070
    ''' Creado por:        Juan Camilo Munera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13/2016
    ''' Pruebas CB:        Juan Camilo Munera (Alcuadrado S.A.) - Mayo 13/2016 - Resultado Ok 
    ''' </history>

    Private Async Sub ConsultarCodigosOyD(ByVal pstrNroDocumento As String)
        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of CompaniasCodigos)

        Try


            ErrorForma = String.Empty

            If Not mdcProxy.CompaniasCodigos Is Nothing Then
                mdcProxy.CompaniasCodigos.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarCodigosOyDQuery(pstrNroDocumento, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la compañía pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la compañía.", Me.ToString(), "ConsultarCodigosOyD", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                End If
            End If
            logPreguntarCambio = True
            strNroDocumentoActual = pstrNroDocumento
            logResultado = True

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de la compañía seleccionada.", Me.ToString(), "ConsultarCodigosOyD", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        'Return (logResultado)
    End Sub

    Private Sub IngresarDetallePorDefecto(ByVal pstrNroDocumento As String)
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasCodigos
        Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasCodigos)
        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CompaniasCodigos Is Nothing Then
                mdcProxy.CompaniasCodigos.Clear()
            End If


            ListaDetalle = Nothing


            IngresarDetalle()



            ListaDetalle.Item(0).intIDCompania = _EncabezadoSeleccionado.intIDCompania
            ListaDetalle.Item(0).intIDCompaniaCodigo = 1
            ListaDetalle.Item(0).lngIDComitente = strCodigoOYDEncabezado
            ListaDetalle.Item(0).logCompania = False
            ListaDetalle.Item(0).strNombre = _EncabezadoSeleccionado.strNombre
            ListaDetalle.Item(0).strNroDocumento = _EncabezadoSeleccionado.strNumeroDocumento
            strNroDocumentoActual = _EncabezadoSeleccionado.strNumeroDocumento
            _mLogBuscarClienteDetalle = False
        Catch ex As Exception

        End Try
    End Sub


    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle penalidad relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks></remarks>
    Public Async Function ConsultarDetallePenalidad(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of CompaniasPenalidades)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CompaniasPenalidades Is Nothing Then
                mdcProxy.CompaniasPenalidades.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarCompaniasPenalidadesSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la Penalidad pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la penalidad.", Me.ToString(), "ConsultarDetallePenalidad", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaPenalidad = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de la penalidad seleccionada.", Me.ToString(), "ConsultarDetallePenalidad", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle limite relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks></remarks>
    Public Async Function ConsultarDetalleLimite(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of CompaniasLimites)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CompaniasLimites Is Nothing Then
                mdcProxy.CompaniasLimites.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarCompaniasLimitesSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de los limites pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de los limites.", Me.ToString(), "ConsultarDetalleLimite", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalleLimites = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de los limites seleccionados.", Me.ToString(), "ConsultarDetalleLimite", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle para la penalidad por defecto
    ''' </summary>
    Private Async Sub ConsultarPenalidadPorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of CompaniasPenalidades)

            objRet = Await dcProxy.Load(dcProxy.ConsultarCompaniasPenalidadesPorDefectoQuery(Program.Usuario, Program.HashConexion)).AsTask
            MyBase.CambioItem("ListaDetallePaginadaPenalidad")

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto de la penalidad.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de la penalidad.", Me.ToString(), "ConsultarPenalidadPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetallePenalidadPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetallePenalidadPorDefecto = Nothing
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de la penalidad.", Me.ToString(), "ConsultarPenalidadPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle para los acumulados de comisiones
    ''' </summary>
    Private Async Sub ConsultarAcumuladoComisionesPorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of CompaniasAcumuladoComisiones)

            objRet = Await dcProxy.Load(dcProxy.ConsultarCompaniasAcumuladoComisionesPorDefectoQuery(Program.Usuario, Program.HashConexion)).AsTask
            MyBase.CambioItem("ListaDetallePaginadaListaAcumulacionComisiones")

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto de la penalidad.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de la penalidad.", Me.ToString(), "ConsultarPenalidadPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetalleAcumuladosComisionesPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetalleAcumuladosComisionesPorDefecto = Nothing
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de la acumulación de las comisiones.", Me.ToString(), "ConsultarAcumuladoComisionesPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub




    'Public Sub FormatoCambio()
    '    EncabezadoSeleccionado.strNombreCorto = EncabezadoSeleccionado.strNombreCorto.ToUpper
    'End Sub


    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle para la tesoreria por defecto
    ''' </summary>
    Private Async Sub ConsultarTesoreriaPorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of CompaniasTesoreria)

            objRet = Await dcProxy.Load(dcProxy.ConsultarCompaniasTesoreriaPorDefectoQuery(Program.Usuario, Program.HashConexion)).AsTask
            MyBase.CambioItem("ListaDetallePaginadaTesoreria")

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto de la tesoreria.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de la tesoreria.", Me.ToString(), "ConsultarTesoreriaPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetalleTesoreriaPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetalleTesoreriaPorDefecto = Nothing
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de la tesoreria.", Me.ToString(), "ConsultarTesoreriaPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle para los limites por defecto
    ''' </summary>
    Private Async Sub ConsultarLimitesPorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of CompaniasLimites)

            objRet = Await dcProxy.Load(dcProxy.ConsultarCompaniasLimitesPorDefectoQuery(Program.Usuario, Program.HashConexion)).AsTask
            MyBase.CambioItem("ListaDetalleLimites")

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto de los limites.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de los limites.", Me.ToString(), "ConsultarLimitesPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetalleLimitesPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetalleLimitesPorDefecto = Nothing
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de los limites.", Me.ToString(), "ConsultarLimitesPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle para la tesoreria por defecto
    ''' </summary>
    Private Async Sub ConsultarCondicionesTesoreriaPorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of CompaniasCondicionesTesoreria)

            objRet = Await dcProxy.Load(dcProxy.ConsultarCompaniasCondicionesTesoreriaPorDefectoQuery(Program.Usuario, Program.HashConexion)).AsTask
            MyBase.CambioItem("ListaDetallePaginadaCondicionesTesoreria")

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto de las condiciones de tesoreria.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de las condiciones de tesoreria.", Me.ToString(), "ConsultarTesoreriaPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetalleTesoreriaCondicionesPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetalleTesoreriaCondicionesPorDefecto = Nothing
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de la tesoreria.", Me.ToString(), "ConsultarTesoreriaPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle de parametros compania
    ''' </summary>
    Private Async Sub ConsultarParametrosCompaniaPorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of ParametrosCompania)

            objRet = Await dcProxy.Load(dcProxy.ConsultarParametrosCompaniaPorDefectoQuery(Program.Usuario, Program.HashConexion)).AsTask
            MyBase.CambioItem("ListaDetalleParametrosPaginadaCompania")

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto de las parametros compañía.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de las parametros compañía.", Me.ToString(), "ConsultarParametrosCompaniaPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetalleParametrosCompaniaPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetalleParametrosCompaniaPorDefecto = Nothing
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto de los parametros compañía.", Me.ToString(), "ConsultarTesoreriaPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub





    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle tesoreria relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalleTesoreria(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of CompaniasTesoreria)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CompaniasTesorerias Is Nothing Then
                mdcProxy.CompaniasTesorerias.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarCompaniasTesoreriaSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la tesorería pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la tesorería.", Me.ToString(), "ConsultarDetalleTesoreria", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalleTesoreria = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de la tesorería seleccionada.", Me.ToString(), "ConsultarDetalleTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function


    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle de las autorizaciones relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks></remarks>
    Public Async Function ConsultarDetalleAutorizaciones(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of CompaniasAutorizaciones)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CompaniasLimites Is Nothing Then
                mdcProxy.CompaniasLimites.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarCompaniasAutorizacionesSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de los limites pero no se recibió detalle de autorizaciones de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de autorizaciones.", Me.ToString(), "ConsultarDetalleAutorizaciones", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalleAutorizaciones = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de autorizaciones seleccionados.", Me.ToString(), "ConsultarDetalleLimite", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle condiciones tesoreria relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalleCondicionesTesoreria(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of CompaniasCondicionesTesoreria)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CompaniasCondicionesTesorerias Is Nothing Then
                mdcProxy.CompaniasCondicionesTesorerias.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarCompaniasCondicionesTesoreriaSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de las condiciones de tesorería pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de las condiciones tesorería.", Me.ToString(), "ConsultarDetalleCondicionesTesoreria", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalleCondicionesTesoreria = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de las condiciones tesorería seleccionada.", Me.ToString(), "ConsultarDetalleCondicionesTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle de parametros compañía
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks>NOTA: </remarks>
    Public Async Function ConsultarDetalleParametrosCompania(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of ParametrosCompania)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.ParametrosCompanias Is Nothing Then
                mdcProxy.ParametrosCompanias.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarParametrosCompaniaSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de los parametros de condiciones operativas por defecto de la compañía pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de las condiciones operativas por defecto.", Me.ToString(), "ConsultarDetalleParametrosCompania", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalleParametrosCompania = objRet.Entities.ToList
                    ListaParametrosSeleccioando = ListaDetalleParametrosCompania
                End If
            End If

            logResultado = True



            'Validar que los datos no se solapen
            'DARM 20171027 Se quitan las 
            'If _ListaPenalidad IsNot Nothing Then
            '    If _ListaPenalidad.Count > 1 Then
            '        _ListaPenalidad = _ListaPenalidad.OrderBy(Function(i) i.intDiasInicial).ToList
            '        Dim K As Integer
            '        Dim l As Integer
            '        For K = 0 To _ListaPenalidad.Count - 1
            '            For l = K + 1 To _ListaPenalidad.Count - 1
            '                If Not _ListaPenalidad(l).intDiasInicial > _ListaPenalidad(K).intDiasFinal Then
            '                    strMsg = String.Format("{0}{1} + Existe un conflicto ya que el día inicial" & " " & _ListaPenalidad(l).intDiasInicial & "," & " " & "no es mayor que el día final en el detalle penalidad" & " " & _ListaPenalidad(K).intDiasFinal, strMsg, vbCrLf)
            '                End If
            '            Next
            '        Next
            '    End If
            'End If

            If _ListaDetalleParametrosCompania IsNot Nothing Then
                Dim LL As Integer
                Dim CantidadDecimalesComision As String = "0"
                For LL = 0 To _ListaDetalleParametrosCompania.Count - 1
                    If _ListaDetalleParametrosCompania(LL).strParametro = "DECIMALES_COMISION_ADMINISTRACION" Then
                        CantidadDecimalesComision = _ListaDetalleParametrosCompania(LL).strValorParametro
                    End If
                Next

                If IsNumeric(CStr(CantidadDecimalesComision)) Then
                    DecimalesComision = CantidadDecimalesComision
                Else
                    DecimalesComision = "2"
                End If

            End If


            'If _ListaDetalleParametrosCompania IsNot Nothing Then
            '    Dim CantidadDecimalesComision As String = CStr((From item In _ListaDetalleParametrosCompania Select item.strParametro = "DECIMALES_COMISION_ADMINISTRACION").First)

            '    If IsNumeric(CStr(CantidadDecimalesComision)) Then
            '        CantidadDecimalesComision = "2"
            '    End If

            'End If



        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de las condiciones operativas por defecto compañía seleccionada.", Me.ToString(), "ConsultarDetalleParametrosCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Private Function IsNumeric(ByVal valor As String) As Boolean
        Dim result As Integer
        'Return Int.TryParse(valor, )

        Return Integer.TryParse(valor, result)

    End Function
    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del Condiciones operativas por defecto
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks>NOTA: </remarks>
    Public Async Function ConsultarDetalleCondicionesOperativasPorDefecto(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of CompaniasCondicionesTesoreria)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.ParametrosCompanias Is Nothing Then
                mdcProxy.ParametrosCompanias.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConceptosTesoreriaPorDefectoCiasSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de los parametros de la compañía pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de los parametros de la compañía.", Me.ToString(), "ConsultarDetalleParametrosCompania", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalleCondicionesTesoreria = objRet.Entities.ToList
                    'DetalleTesoreriaCondicioneSeleccionado = ListaDetalleCondicionesTesoreria
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de los paramteros de la compañía seleccionada.", Me.ToString(), "ConsultarDetalleParametrosCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function


    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle para los acumulados de comisiones relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalleAcumuladoComisiones(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of CompaniasAcumuladoComisiones)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CompaniasCondicionesTesorerias Is Nothing Then
                mdcProxy.CompaniasCondicionesTesorerias.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarCompaniasAcumuladoComisionesSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de los rangos de comisiones", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de los rengos de comisiones.", Me.ToString(), "ConsultarDetalleAcumuladoComisiones", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaAcumulacionComisiones = objRet.Entities.ToList
                    ListaAcumulacionComisionesPorTipoTabla = ListaAcumulacionComisiones.Where(Function(i) i.strTipoTabla = strTipoTabla).ToList()
                    If (ListaAcumulacionComisiones.Count > 0) Then
                        DetalleAcumuladoComisionesSeleccionado = ListaAcumulacionComisiones.Last
                    End If
                End If
            End If



            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de los rangos de comisiones seleccionada.", Me.ToString(), "ConsultarDetalleAcumuladoComisiones", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function


#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"
    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ValidarRegistro
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: TODO CP
    ''' Creado por:        Javier Pardo (Alcuadrado S.A.)
    ''' Fecha:             Julio 31/2015
    ''' Pruebas CB:        Javier Pardo (Alcuadrado S.A.) - Julio 31/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: TODO CP
    ''' Descripción:       Ajuste a validación de códigos repetidos en los detalles de la misma compañía. Se añade Trim para quitar espacios
    ''' Fecha:             Septiembre 11/2015
    ''' Pruebas CB:        Javier Pardo (Alcuadrado S.A.) - Septiembre 11/2015 - Resultado Ok 
    ''' ID del cambio:     JEPM20150911
    ''' </history>
    ''' 
    ''' <history>
    ''' ID caso de prueba: TODO CP
    ''' Descripción:       Ajuste a la forma de hacer la validación en los detalles, se debe distinguir si se maneja valor unidad o  no
    ''' Fecha:             Febrero 9 de 2016
    ''' Pruebas CB:        Juan Camilo Munera (Alcuadrado S.A.) - Febrero 9 de 2016 - Resultado Ok 
    ''' ID del cambio:     JCM20160209
    ''' </history>


    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try




            'JCM 20160218 Solo se valida que exista un código para el caso en que el tipo de compañia sea Posición Propia y cuando sea un nuevo registro
            If _EncabezadoSeleccionado.strTipoCompania = "PP" And _EncabezadoSeleccionado.intIDCompania <= 0 Then



                '------------------------------------------------------------------------------------------------------------------------------------------------
                '-- Valida que por lo menos exista un detalle para poder crear todo un registro
                '-- CP057
                '------------------------------------------------------------------------------------------------------------------------------------------------
                If IsNothing(_ListaDetalle) Then
                    strMsg = String.Format("{0}{1} + La compañía debe tener por lo menos un código OyD asociado.", strMsg, vbCrLf)
                ElseIf _ListaDetalle.Count = 0 Then
                    strMsg = String.Format("{0}{1} + La compañía debe tener por lo menos un código OyD asociado.", strMsg, vbCrLf)
                End If



                'Validar que se haya asignado un cliente en el detalla
                If _ListaDetalle IsNot Nothing Then
                    Dim intCantidadDetallesSinAsignar = (From item In _ListaDetalle Select item.lngIDComitente Where lngIDComitente = "0").Count

                    If intCantidadDetallesSinAsignar > 0 Then
                        strMsg = String.Format("{0}{1} + Debe asignar correctamente los códigos OyD en el detalle.", strMsg, vbCrLf)
                    End If
                End If
            End If



            ''------------------------------------------------------------------------------------------------------------------------------------------------
            ''-- Valida que no existan clientes repetidos en el detalle
            ''ID caso de prueba:  CP016
            ''------------------------------------------------------------------------------------------------------------------------------------------------
            'If _ListaDetalle IsNot Nothing Then

            '    Dim intCantidadDetalles As Integer = _ListaDetalle.Count
            '    Dim ValidarDetallesRepetidos = (From item In _ListaDetalle Select item.lngIDComitente.Trim()).Distinct  '.OrderBy(Function(x As CompaniasCodigos) x.lngIDComitente) --JEPM20150911
            '    Dim intCantidadDetallesRepetidos As Integer = ValidarDetallesRepetidos.Count

            '    If intCantidadDetalles <> intCantidadDetallesRepetidos Then
            '        strMsg = String.Format("{0}{1} + No se permite asociar el código OyD más de una vez a la misma compañía", strMsg, vbCrLf)
            '    End If

            'End If






            If _EncabezadoSeleccionado.logManejaValorUnidad <> True Then


                '------------------------------------------------------------------------------------------------------------------------------------------------
                '-- Valida que no existan clientes repetidos en el detalle
                'ID caso de prueba:  CP016
                '------------------------------------------------------------------------------------------------------------------------------------------------
                If _ListaDetalle IsNot Nothing Then

                    Dim intCantidadDetalles As Integer = _ListaDetalle.Count
                    Dim ValidarDetallesRepetidos = (From item In _ListaDetalle Select item.lngIDComitente.Trim()).Distinct  '.OrderBy(Function(x As CompaniasCodigos) x.lngIDComitente) --JEPM20150911
                    Dim intCantidadDetallesRepetidos As Integer = ValidarDetallesRepetidos.Count

                    If intCantidadDetalles <> intCantidadDetallesRepetidos Then
                        strMsg = String.Format("{0}{1} + No se permite asociar el código OyD más de una vez a la misma compañía", strMsg, vbCrLf)
                    End If

                End If

            Else



                '------------------------------------------------------------------------------------------------------------------------------------------------
                '-- Valida que no existan una compañia repetida en el detalle
                'ID caso de prueba:  CP045
                '------------------------------------------------------------------------------------------------------------------------------------------------

                If _ListaDetalle IsNot Nothing Then

                    'Dim intCantidadDetalles As Integer = _ListaDetalle.Count
                    Dim intCantidadDetalles As Integer = (From item In _ListaDetalle Where item.logCompania = False).Count
                    'intCantidadDetalles = intCantidadDetalles - intCantidadDetallesAValidar



                    Dim ValidarDetallesRepetidos = (From item In _ListaDetalle Where item.logCompania = False Select item.lngIDComitente.Trim()).Distinct  '.OrderBy(Function(x As CompaniasCodigos) x.lngIDComitente) --JEPM20150911
                    Dim intCantidadDetallesRepetidos As Integer = ValidarDetallesRepetidos.Count

                    If intCantidadDetalles <> intCantidadDetallesRepetidos And intCantidadDetallesRepetidos > 0 Then
                        strMsg = String.Format("{0}{1} + No se permite asociar el código OyD más de una vez a la misma compañía", strMsg, vbCrLf)
                    End If

                End If

                '------------------------------------------------------------------------------------------------------------------------------------------------
                '-- Valida que no existan una compañia repetida en el detalle
                'ID caso de prueba:  CP046
                '------------------------------------------------------------------------------------------------------------------------------------------------


                If _ListaDetalle IsNot Nothing Then
                    'Dim intCantidadDetalles As Integer = _ListaDetalle.Count
                    Dim intCantidadDetalles As Integer = (From item In _ListaDetalle Where item.logCompania = True).Count

                    Dim ValidarDetallesRepetidos = (From item In _ListaDetalle Where item.logCompania = True Select item.lngIDComitente.Trim()).Distinct
                    Dim intCantidadDetallesRepetidos As Integer = ValidarDetallesRepetidos.Count

                    If intCantidadDetalles <> intCantidadDetallesRepetidos And intCantidadDetallesRepetidos > 0 Then
                        strMsg = String.Format("{0}{1} + No se permite asociar a la compañia más de una vez la misma compañía", strMsg, vbCrLf)
                    End If


                End If

                'JCM2017, Validar que si el Tipo de Participación es por Clases, solo se pueda ingresar un codigo oyd y debe ser igual el codigo oyd seleccionado en el encabezado
                If _EncabezadoSeleccionado.strParticipacion = "C" Then
                    Dim intCantidadDetalles As Integer = (From item In _ListaDetalle Where item.strNroDocumento <> String.Empty).Count
                    Dim intCodigoOyd As Integer = (From item In _ListaDetalle Where item.lngIDComitente = strCodigoOYDEncabezado).Count

                    If intCantidadDetalles <> 1 Or intCodigoOyd <> 1 Then
                        strMsg = String.Format("{0}{1} + Cuando es un tipo de participación por clases, solo es posible ingresar un código oyd en el detalle y esté debe ser igual al del encabezado", strMsg, vbCrLf)
                    End If
                End If


                ''JCM20170128, Validar que si el Tipo de Participación es por Clases, solo se pueda ingresar un codigo oyd y debe ser igual el codigo oyd seleccionado en el encabezado
                'If _EncabezadoSeleccionado.strParticipacion = "CC" Then
                '    Dim intCantidadDetalles As Integer = (From item In _ListaDetalle Where item.strNroDocumento = _EncabezadoSeleccionado.strNumeroDocumento).Count
                '    Dim inCantidadRegistros As Integer = _ListaDetalle.Count

                '    If intCantidadDetalles <> inCantidadRegistros Then
                '        strMsg = String.Format("{0}{1} + Cuando es un tipo de participación que consolida clases, solo es posible ingresar un código oyd que sean del mismo documento que el encabezado", strMsg, vbCrLf)
                '    End If
                'End If



            End If





            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "validarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

    Private Function ValidarDetalleLimitesActualizar() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try




            'JCM20170614, Validaciones para los limites de las compañias, configurados en la pestaña de limites
            If _EncabezadoSeleccionado.strTipoCompania = "FIC" Or _EncabezadoSeleccionado.strTipoCompania = "APT" Then
                If Not IsNothing(_ListaDetalleLimites) Then
                    If _ListaDetalleLimites.Count > 0 Then
                        If _EncabezadoSeleccionado.strParticipacion = "I" Or _EncabezadoSeleccionado.strParticipacion = "CC" Then
                            Dim intCantidadDetalles As Integer = (From item In _ListaDetalleLimites Select item.strTipoConcepto, item.dblValor, item.logValor Where strTipoConcepto = "PMAX" And dblValor > 0 And logValor = True).Count
                            If intCantidadDetalles > 0 Then
                                strMsg = String.Format("{0}{1} + No es posible configurar el límite de participación máxima en valor para es tipo de participación.", strMsg, vbCrLf)
                            End If
                        ElseIf _EncabezadoSeleccionado.strParticipacion = "C" Then
                            Dim intCantidadDetalles As Integer = (From item In _ListaDetalleLimites Select item.strTipoConcepto, item.dblPorcentaje, item.logPorcentaje Where strTipoConcepto = "PMAX" And dblPorcentaje > 0 And logPorcentaje = True).Count
                            If intCantidadDetalles > 0 Then
                                strMsg = String.Format("{0}{1} + No es posible configurar el límite de participación máxima en porcentaje para es tipo de participación.", strMsg, vbCrLf)
                            End If
                        End If
                    End If
                End If



                If Not IsNothing(_ListaDetalleLimites) Then
                    If _ListaDetalleLimites.Count > 0 Then
                        If _EncabezadoSeleccionado.strParticipacion = "CC" Then
                            Dim intCantidadDetalles As Integer = (From item In _ListaDetalleLimites Select item.strTipoConcepto, item.dblValor, item.logValor, item.dblPorcentaje, item.logPorcentaje Where strTipoConcepto = "PMIN" And ((dblValor > 0 And logValor = True) Or (dblPorcentaje > 0 And logPorcentaje = True))).Count
                            If intCantidadDetalles > 0 Then
                                strMsg = String.Format("{0}{1} + No es posible configurar en límite de saldo mínimo para este tipo de participación.", strMsg, vbCrLf)
                            End If
                        ElseIf _EncabezadoSeleccionado.strParticipacion = "C" Or _EncabezadoSeleccionado.strParticipacion = "I" Then
                            Dim intCantidadDetalles As Integer = (From item In _ListaDetalleLimites Select item.strTipoConcepto, item.dblValor, item.logValor, item.dblPorcentaje, item.logPorcentaje Where strTipoConcepto = "PMIN" And dblPorcentaje > 0 And logPorcentaje = True).Count
                            If intCantidadDetalles > 0 Then
                                strMsg = String.Format("{0}{1} + No es posible configurar en límite de saldo mínimo en porcentaje para este tipo de participación.", strMsg, vbCrLf)
                            End If
                        End If
                    End If
                End If

                If Not IsNothing(_ListaDetalleLimites) Then
                    If _ListaDetalleLimites.Count > 0 Then
                        If _EncabezadoSeleccionado.strParticipacion = "I" Or _EncabezadoSeleccionado.strParticipacion = "C" Then
                            Dim intCantidadDetalles As Integer = (From item In _ListaDetalleLimites Select item.strTipoConcepto, item.dblPorcentaje, item.logPorcentaje Where (strTipoConcepto = "AD" Or strTipoConcepto = "II" Or strTipoConcepto = "RP") And dblPorcentaje > 0 And logPorcentaje = True).Count
                            If intCantidadDetalles > 0 Then
                                strMsg = String.Format("{0}{1} + No es posible configurar un aporte adicional,inversión inicial o retiro parcial para este tipo de participación.", strMsg, vbCrLf)
                            End If
                        ElseIf _EncabezadoSeleccionado.strParticipacion = "CC" Then
                            Dim intCantidadDetalles As Integer = (From item In _ListaDetalleLimites Select item.strTipoConcepto, item.dblPorcentaje, item.logPorcentaje, item.dblValor, item.logValor Where (strTipoConcepto = "AD" Or strTipoConcepto = "II" Or strTipoConcepto = "RP") And ((dblPorcentaje > 0 And logPorcentaje = True) Or (dblValor > 0 And logValor = True))).Count
                            If intCantidadDetalles > 0 Then
                                strMsg = String.Format("{0}{1} + No es posible configurar un aporte adicional,inversión inicial o retiro parcial para este tipo de participación.", strMsg, vbCrLf)
                            End If
                        End If
                    End If
                End If

                If Not IsNothing(_ListaDetalleLimites) Then
                    If _ListaDetalleLimites.Count > 0 Then
                        If _EncabezadoSeleccionado.strParticipacion = "I" Or _EncabezadoSeleccionado.strParticipacion = "C" Then
                            Dim intCantidadDetalles As Integer = (From item In _ListaDetalleLimites Select item.strTipoConcepto, item.dblValor, item.logValor Where strTipoConcepto = "SP" And dblValor > 0 And logValor = True).Count
                            If intCantidadDetalles > 0 Then
                                strMsg = String.Format("{0}{1} + No es posible configurar un saldo protegido en valor para este tipo de participación.", strMsg, vbCrLf)
                            End If
                        ElseIf _EncabezadoSeleccionado.strParticipacion = "CC" Then
                            Dim intCantidadDetalles As Integer = (From item In _ListaDetalleLimites Select item.strTipoConcepto, item.dblPorcentaje, item.logPorcentaje, item.dblValor, item.logValor Where strTipoConcepto = "SP" And ((dblValor > 0 And logValor = True) Or (dblPorcentaje > 0 And logPorcentaje = True))).Count
                            If intCantidadDetalles > 0 Then
                                strMsg = String.Format("{0}{1} + No es posible configurar un saldo protegido en valor para este tipo de participación.", strMsg, vbCrLf)
                            End If
                        End If
                    End If
                End If


            End If




            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos para los limites", Me.ToString(), "ValidarDetalleLimitesActualizar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function



    Public Function ValidarDetalleTesoreria(ByVal pstrTipoOperacion As String, ByVal pintDiasAplicaTransaccion As Integer, ByVal pLngIDConcepto As Integer, ByVal pintDiasPago As Integer) As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            If IsNothing(pstrTipoOperacion) Or pstrTipoOperacion = String.Empty Then
                strMsg = String.Format("{0}{1} + Es necesario ingresar un tipo de operación.", strMsg, vbCrLf)
            End If

            If pintDiasAplicaTransaccion < 0 Then
                strMsg = String.Format("{0}{1} + El número de días que aplica transacción no puede ser negativo.", strMsg, vbCrLf)
            End If

            If IsNothing(pLngIDConcepto) Then
                strMsg = String.Format("{0}{1} + Es necesario ingresar un conepto contable", strMsg, vbCrLf)
            End If

            If pLngIDConcepto = 0 And (pstrTipoOperacion = "AC" Or pstrTipoOperacion = "IA" Or pstrTipoOperacion = "RP" Or pstrTipoOperacion = "RTC" Or pstrTipoOperacion = "TF" Or pstrTipoOperacion = "RR") Then
                strMsg = String.Format("{0}{1} + Es necesario ingresar un conepto contable", strMsg, vbCrLf)
            End If

            If pintDiasPago < 0 Then
                strMsg = String.Format("{0}{1} + El número de días de pago no puede ser negativo.", strMsg, vbCrLf)
            End If


            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If



        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "ValidarDetalleTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Public Function ValidarDetalleCondicionesTesoreria(ByVal pstrTipoOperacion As String, ByVal pLngIDConcepto As Integer) As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            If IsNothing(pstrTipoOperacion) Or pstrTipoOperacion = String.Empty Then
                strMsg = String.Format("{0}{1} + Es necesario ingresar un tipo de operación.", strMsg, vbCrLf)
            End If


            If IsNothing(pLngIDConcepto) Then
                strMsg = String.Format("{0}{1} + Es necesario ingresar un conepto contable", strMsg, vbCrLf)
            End If

            If pLngIDConcepto = 0 And (pstrTipoOperacion = "GMF" Or pstrTipoOperacion = "PE") Then
                strMsg = String.Format("{0}{1} + Es necesario ingresar un conepto contable", strMsg, vbCrLf)
            End If


            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If



        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "ValidarDetalleCondicionesTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Public Function ValidarDetalleParametrosCompania(ByVal pstrCategoriaParametro As String, ByVal pstrParametro As String, ByVal pstrValorParametro As String, ByVal pstrDescripcionParametro As String, ByVal plogManejaFechaParametro As Boolean, ByVal pdtmFechaInicialParametro As Date) As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try

            If plogManejaFechaParametro = True Then
                If pdtmFechaInicialParametro = Nothing Then
                    strMsg = String.Format("{0}{1} + Es necesario seleccionar una fecha.", strMsg, vbCrLf)
                End If
            End If

            If pstrValorParametro = Nothing Then
                strMsg = String.Format("{0}{1} + Es necesario ingresar un valor para el párametro.", strMsg, vbCrLf)
            End If



            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If



        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle de los parametros de compañía.", Me.ToString(), "ValidarDetalleParametrosCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function




    ''' <summary>
    ''' Funcion que se utiliza para validar el detalle de los limites  
    ''' Se debe llamar la pantalla cwConfiguraciónLímitesCompañia, tanto cuando se ingresa un registro nuevo como cuando se actualiza un registro
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP038
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13/2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13/2016 - Resultado Ok 
    ''' </history>


    Public Function ValidarDetalleLimites(ByVal pstrTipoConcepto As String, ByVal pdblPorcentaje As Double, ByVal plogPorcentaje As Boolean, ByVal dblValor As Double, ByVal plogValor As Boolean) As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            If IsNothing(pstrTipoConcepto) Or pstrTipoConcepto = String.Empty Then
                strMsg = String.Format("{0}{1} + Es necesario ingresar un tipo de concepto.", strMsg, vbCrLf)
            End If


            'Validar que se ingrese un porcentaje o el valor
            'CP062
            If pdblPorcentaje = 0 And dblValor = 0 Then
                strMsg = String.Format("{0}{1} + Debe ingresar el porcentaje o el valor.", strMsg, vbCrLf)
            End If

            If pdblPorcentaje > 0 And dblValor > 0 Then
                strMsg = String.Format("{0}{1} + Solo es posible ingresar un dato en valor o en porcentaje, no es posible ingresarlo en ambas opciones.", strMsg, vbCrLf)
            End If

            If pstrTipoConcepto = "PMAX" Then
                If _EncabezadoSeleccionado.strParticipacion = "I" Or _EncabezadoSeleccionado.strParticipacion = "CC" Then
                    If plogPorcentaje = False Or pdblPorcentaje <= 0 Then
                        strMsg = String.Format("{0}{1} + Solo es posible configurar una participación máxima en porcentaje para este tipo de participación", strMsg, vbCrLf)
                    End If
                ElseIf _EncabezadoSeleccionado.strParticipacion = "C" Then
                    If plogValor = False Or dblValor <= 0 Then
                        strMsg = String.Format("{0}{1} + Solo es posible configurar una participación máxima en pesos para este tipo de participación", strMsg, vbCrLf)
                    End If
                End If
            End If

            If pstrTipoConcepto = "PMIN" Then
                If _EncabezadoSeleccionado.strParticipacion = "C" Or _EncabezadoSeleccionado.strParticipacion = "I" Then
                    If plogValor = False Or dblValor <= 0 Then
                        strMsg = String.Format("{0}{1} + Solo es posible configurar una saldo mínimo en pesos para este tipo de participación", strMsg, vbCrLf)
                    End If
                Else
                    strMsg = String.Format("{0}{1} + Para este tipo de participación no aplica una configuración para un saldo mínima", strMsg, vbCrLf)
                End If
            End If


            If pstrTipoConcepto = "AD" Or pstrTipoConcepto = "II" Or pstrTipoConcepto = "RP" Then
                If _EncabezadoSeleccionado.strParticipacion = "I" Or _EncabezadoSeleccionado.strParticipacion = "C" Then
                    If plogValor = False Or dblValor <= 0 Then
                        strMsg = String.Format("{0}{1} + Solo es posible configurar un aporte adicional, inversión inicial o retiro parcial para este tipo de participación en pesos", strMsg, vbCrLf)
                    End If
                Else
                    strMsg = String.Format("{0}{1} + Para este tipo de participación no aplica una configuración para un aporte adicional, inversión inicial o retiro parcial", strMsg, vbCrLf)
                End If
            End If

            If pstrTipoConcepto = "SP" Then
                If _EncabezadoSeleccionado.strParticipacion = "I" Or _EncabezadoSeleccionado.strParticipacion = "C" Then
                    If plogPorcentaje = False Or pdblPorcentaje <= 0 Then
                        strMsg = String.Format("{0}{1} + Solo es posible configurar un saldo protegido en porcentaje", strMsg, vbCrLf)
                    End If
                Else
                    strMsg = String.Format("{0}{1} + Para este tipo de participación no aplica una configuración de saldo protegido ", strMsg, vbCrLf)
                End If
            End If



            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle de los limites. " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle de los limites.", Me.ToString(), "ValidarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)

    End Function

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ValidarRegistro
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: TODO CP
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Enero 29/2016
    ''' Descripción:       Se valida que la información ingresada en el Grid de penalidad no tenga inconsistencias
    ''' Pruebas CB:        Juan Camilo Munera (Alcuadrado S.A.) - Enero 29/2016 - Resultado Ok 
    ''' </history>
    Private Function ValidarDetallePenalidad() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            If EncabezadoSeleccionado.strPenalidad = "T" Then


                'Validar que se haya asignado ningun valor de dias con 0, ni penalidad 0
                'JCM20170123, Se realiza cambio para permitir que sea ingrese un dia inicial de 0 y un factor de 0
                'DARM 20171031 SE PASAN LAS VALIDACIONES PARA BASE DE DATOS
                'If _ListaPenalidad IsNot Nothing Then
                '    Dim intCantidadDetallesSinEditar = (From item In _ListaPenalidad Select item.intDiasFinal Where intDiasFinal = 0).Count

                '    If intCantidadDetallesSinEditar > 0 Then
                '        strMsg = String.Format("{0}{1} + No puede existir un valor de 0 en los días finales en el detalle penalidad", strMsg, vbCrLf)
                '    End If
                'End If

                'JCM20170123, Se comenta validacion para permitir que los dias iniciales y finales puedan coincidir

                'If _ListaPenalidad IsNot Nothing Then
                '    Dim intCantidadDetallesPenalidades As Integer = _ListaPenalidad.Count
                '    Dim ValidarDetallesPenalidades = (From item In _ListaPenalidad Select item.intDiasInicial, item.intDiasFinal Where intDiasInicial = intDiasFinal).Count
                '    Dim intCantidadDetallePenalidadesEncontrados As Integer = ValidarDetallesPenalidades

                '    If intCantidadDetallePenalidadesEncontrados > 0 Then
                '        strMsg = String.Format("{0}{1} + El día de inicio y el día final no pueden ser el mismo para el detalle penalidad", strMsg, vbCrLf)
                '    End If
                'End If

                'DARM 20171031 SE PASAN LAS VALIDACIONES PARA BASE DE DATOS
                'If _ListaPenalidad IsNot Nothing Then
                '    Dim intCantidadDetallesPenalidades As Integer = _ListaPenalidad.Count
                '    Dim ValidarDetallesPenalidades = (From item In _ListaPenalidad Select item.intDiasInicial, item.intDiasFinal Where intDiasInicial > intDiasFinal).Count
                '    Dim intCantidadDetallePenalidadesEncontrados As Integer = ValidarDetallesPenalidades

                '    If intCantidadDetallePenalidadesEncontrados > 0 Then
                '        strMsg = String.Format("{0}{1} + El día de inicio no puede ser mayor que el día final para el detalle penalidad", strMsg, vbCrLf)
                '    End If
                'End If


                'Validar que los datos no se solapen
                'DARM 20171027 Se quitan las 
                'If _ListaPenalidad IsNot Nothing Then
                '    If _ListaPenalidad.Count > 1 Then
                '        _ListaPenalidad = _ListaPenalidad.OrderBy(Function(i) i.intDiasInicial).ToList
                '        Dim K As Integer
                '        Dim l As Integer
                '        For K = 0 To _ListaPenalidad.Count - 1
                '            For l = K + 1 To _ListaPenalidad.Count - 1
                '                If Not _ListaPenalidad(l).intDiasInicial > _ListaPenalidad(K).intDiasFinal Then
                '                    strMsg = String.Format("{0}{1} + Existe un conflicto ya que el día inicial" & " " & _ListaPenalidad(l).intDiasInicial & "," & " " & "no es mayor que el día final en el detalle penalidad" & " " & _ListaPenalidad(K).intDiasFinal, strMsg, vbCrLf)
                '                End If
                '            Next
                '        Next
                '    End If
                'End If


                If (_EncabezadoSeleccionado.intDiasPlazo <= 0 Or IsNothing(_EncabezadoSeleccionado.intDiasPlazo)) And (_EncabezadoSeleccionado.strPactoPermanencia = "S") Then
                    strMsg = String.Format("{0}{1} + Los dias de plazo deben ser mayores que 0, ya que se esta ingresando una tabla de penalidad", strMsg, vbCrLf)
                Else
                    'JCM20160131, Debemos hallar el mayor dia definido en la tabla
                    Dim IntMayorValor As Integer
                    If _ListaPenalidad IsNot Nothing Then
                        If _ListaPenalidad.Count >= 1 Then
                            _ListaPenalidad = _ListaPenalidad.OrderBy(Function(i) i.intDiasInicial).ToList

                            Dim K As Integer
                            Dim l As Integer
                            IntMayorValor = CInt(_ListaPenalidad(0).intDiasFinal)
                            For K = 0 To _ListaPenalidad.Count - 1
                                For l = K + 1 To _ListaPenalidad.Count - 1
                                    If _ListaPenalidad(l).intDiasFinal > IntMayorValor Then
                                        IntMayorValor = CInt(_ListaPenalidad(l).intDiasFinal)
                                    End If
                                Next
                            Next
                        End If
                    End If

                    If IntMayorValor <> _EncabezadoSeleccionado.intDiasPlazo Then
                        strMsg = String.Format("{0}{1} + los dias de plazo y el mayor dia configurado en la tabla de penalidad deben ser iguales", strMsg, vbCrLf)
                    End If

                End If







                If Not strMsg.Equals(String.Empty) Then
                    logResultado = False
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle penalidad " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            End If

        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle penalidad.", Me.ToString(), "ValidarDetallePenalidad", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el registros 
    ''' Se debe llamar desde el procedimiento Actualizar Registro
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 10 de 2017
    ''' Descripción:       Se valida que la información ingresada en el Grid de acumulado de comisiones no tenga inconsistencias
    ''' Pruebas CB:        Juan Camilo Munera (Alcuadrado S.A.) - Mayo 10/2017 - Resultado Ok 
    ''' </history>
    Private Function ValidarDetalleAcumuladoComisiones() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            Dim actual As Integer
            Dim intCantidadComisiones As Integer
            Dim Actual_RangoInicial As Double
            Dim Actual_RangoFinal As Double
            Dim Actual_Porcentaje As Double
            Dim Siguiente_RangoInicio As Double

            If _ListaAcumulacionComisiones IsNot Nothing Then

                intCantidadComisiones = _ListaAcumulacionComisiones.Count

                If intCantidadComisiones > 0 Then

                    _ListaAcumulacionComisiones = (From item In _ListaAcumulacionComisiones Order By item.strTipoTabla, item.dblRangoInicio Select item).ToList()

                    For actual = 0 To intCantidadComisiones - 1

                        Actual_RangoInicial = CDbl(_ListaAcumulacionComisiones(actual).dblRangoInicio)
                        Actual_RangoFinal = CDbl(_ListaAcumulacionComisiones(actual).dblRangoFinal)
                        Actual_Porcentaje = CDbl(_ListaAcumulacionComisiones(actual).dblPorcentaje)

                        If Actual_RangoFinal = 0 Then
                            strMsg = String.Format("{0}{1} + No puede existir un valor de 0 en el rango final para el detalle de rangos de comisiones", strMsg, vbCrLf)
                        End If

                        If Actual_RangoInicial < 0 Then
                            strMsg = String.Format("{0}{1} + El rango de inicio debe ser un valor positivo", strMsg, vbCrLf)
                        End If

                        If Actual_RangoInicial > Actual_RangoFinal Then
                            strMsg = String.Format("{0}{1} + El rango de inicio no puede ser mayor que el rango final", strMsg, vbCrLf)
                        End If

                        If Actual_Porcentaje < 0 Or Actual_Porcentaje > 100 Then
                            strMsg = String.Format("{0}{1} + El porcentaje debe ser un valor entre 0-100", strMsg, vbCrLf)
                        End If

                        If actual <> intCantidadComisiones - 1 Then

                            Siguiente_RangoInicio = CDbl(_ListaAcumulacionComisiones(actual + 1).dblRangoInicio)

                            If _ListaAcumulacionComisiones(actual).strTipoTabla = _ListaAcumulacionComisiones(actual + 1).strTipoTabla Then

                                If Siguiente_RangoInicio - Actual_RangoFinal <> 1 Then
                                    strMsg = String.Format("{0}{1} + Existe un conflicto ya que el rango incial" & " " & String.Format("{0:$#,##0}", Siguiente_RangoInicio) & "," & " " & "no es el rango que debe continuar despues del rango final" & " " & String.Format("{0:$#,##0}", Actual_RangoFinal), strMsg, vbCrLf)
                                End If

                            End If

                        End If

                    Next

                    If (From item In _ListaAcumulacionComisiones Group By item.strTipoTabla, item.dblRangoInicio Into Resultado = Group Where Resultado.Count > 1 Select Resultado).Count > 0 Then
                        strMsg = String.Format("{0}{1} + No se puede repetir los rangos iniciales para el detalle de rangos de comisiones", strMsg, vbCrLf)
                    End If

                    If (From item In _ListaAcumulacionComisiones Group By item.strTipoTabla Into Resultado = Group Where Resultado.Count > 9 Select Resultado).Count > 0 Then
                        strMsg = String.Format("{0}{1} + Por cada tipo de tabla sólo se pueden ingresar 10 registros", strMsg, vbCrLf)
                    End If

                    Dim intCantidadValoresMaximos = (From item In _ListaAcumulacionComisiones Select item.dblRangoFinal Where dblRangoFinal = Program.ValorMaximoRangoAcumuladoComisiones).Count
                    Dim intCantidadTablas = (From item In _ListaAcumulacionComisiones Select item.strTipoTabla).Distinct.Count

                    If Not intCantidadValoresMaximos = intCantidadTablas Then
                        strMsg = String.Format("{0}{1} + Se debe especificar un rango maximo para cada tipo de tabla", strMsg, vbCrLf)
                    End If

                End If

            End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle de rango de comisiones " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle rango de comisiones.", Me.ToString(), "ValidarDetalleAcumuladoComisiones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function




    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle 
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle para que el usuario ingrese el nuevo detalle
    ''' ID caso de prueba:  CP007
    ''' </summary>
    Public Sub IngresarDetalle()
        Try
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then
                Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasCodigos
                Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasCodigos)

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasCodigos)(mobjDetallePorDefecto, objNvoDetalle)

                objNvoDetalle.intIDCompaniaCodigo = -New Random().Next(0, 1000000)

                If IsNothing(ListaDetalle) Then
                    '' ''If Not Await LlenarComboValor() Then
                    '' ''    Exit Sub
                    '' ''End If
                    ListaDetalle = New List(Of CFCalculosFinancieros.CompaniasCodigos)
                End If

                objNuevaLista = ListaDetalle
                objNuevaLista.Add(objNvoDetalle)
                ListaDetalle = objNuevaLista
                DetalleSeleccionado = _ListaDetalle.First

                MyBase.CambioItem("ListaDetalle")
                MyBase.CambioItem("ListaDetallesPaginada")
                MyBase.CambioItem("DetalleSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle", Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try

            If Not IsNothing(ListaDetalle) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la compañía", Me.ToString(), "IngresarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle en el Grid de Limites y se abre la ventana emergente
    ''' El nuevo registro es ingresado desde la ventana emergente cwConfiguracionlimitesCompañia
    ''' ID caso de prueba:  CP037
    ''' </summary>
    Public Sub IngresarDetalleLimites(ByVal pstrTipoConcepto As String, ByVal pstrTipoConceptoDescripcion As String, ByVal plogValor As Boolean, ByVal pdblvalor As Double, ByVal plogPorentaje As Boolean, ByVal pdblPorcentaje As Double)
        Try
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then
                Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasLimites
                Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasLimites)

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasLimites)(mobjDetalleLimitesPorDefecto, objNvoDetalle)

                'mobjDetalleLimitesPorDefecto.intIDCompaniaLimite = -New Random().Next(0, 1000000)


                If IsNothing(ListaDetalleLimites) Then
                    '' ''If Not Await LlenarComboValor() Then
                    '' ''    Exit Sub
                    '' ''End If
                    ListaDetalleLimites = New List(Of CFCalculosFinancieros.CompaniasLimites)
                End If


                ''''JCM 20160309, VERIFICAR QUE NO SE REPITA UN CONCEPTO
                'CP061
                If _ListaDetalleLimites IsNot Nothing Then
                    Dim intCantidadDetallerepetidosTipo = (From item In _ListaDetalleLimites Select item.strTipoConcepto Where strTipoConcepto = pstrTipoConcepto).Count

                    If intCantidadDetallerepetidosTipo > 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Ya existe un registro con ese tipo de concepto," & " " & "no es posible repetir el registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
                ''''JCM 20160309, VERIFICAR QUE NO SE REPITA UN CONCEPTO



                objNuevaLista = ListaDetalleLimites
                'objNuevaLista.Add(objNvoDetalle)
                objNuevaLista.Add(New CompaniasLimites With {.intIDCompaniaLimite = -New Random().Next(0, 1000000),
                                                             .strTipoConcepto = pstrTipoConcepto,
                                                             .strTipoConceptoDescripcion = pstrTipoConceptoDescripcion,
                                                             .logValor = plogValor,
                                                             .dblValor = pdblvalor,
                                                             .logPorcentaje = plogPorentaje,
                                                             .dblPorcentaje = pdblPorcentaje})
                ListaDetalleLimites = objNuevaLista
                DetalleLimitesSeleccionado = _ListaDetalleLimites.First

                MyBase.CambioItem("ListaDetalleLimites")
                MyBase.CambioItem("ListaDetalleLimitesPaginada")
                MyBase.CambioItem("DetalleLimitesSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle de limites de la compañía. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle de limites de la  compañía.", Me.ToString(), "IngresarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try

            If Not IsNothing(ListaDetalle) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de los limites de la  compañía.", Me.ToString(), "IngresarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va a ingresar un registro en el detalle de tesoreria
    ''' Se debe llamar desde la pantalla cwConfiguracionTesoreriaView
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP063
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13/2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13/2016 - Resultado Ok 
    ''' </history>


    Public Sub IngresarDetalleTesoreria(ByVal pstrTipoOperacion As String, ByVal pstrTipoOperacionDescripcion As String, ByVal pintDiasAplicaTransaccion As Integer, PlngIDConcepto As Integer, PstrDetalleConcepto As String, ByVal PintDiasPago As Integer)
        Try
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then
                Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasTesoreria
                Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasTesoreria)

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasTesoreria)(mobjDetalleTesoreriaPorDefecto, objNvoDetalle)

                'mobjDetalleLimitesPorDefecto.intIDCompaniaLimite = -New Random().Next(0, 1000000)


                If IsNothing(ListaDetalleTesoreria) Then
                    '' ''If Not Await LlenarComboValor() Then
                    '' ''    Exit Sub
                    '' ''End If
                    ListaDetalleTesoreria = New List(Of CFCalculosFinancieros.CompaniasTesoreria)
                End If

                ''''JCM 20160314, VERIFICAR QUE NO SE REPITA UN TIPO DE OPERACION
                'CP065
                If _ListaDetalleTesoreria IsNot Nothing Then
                    Dim intCantidadDetalleTesoreriarepetidosTipoOperacion = (From item In _ListaDetalleTesoreria Select item.strTipoOperacion Where strTipoOperacion = pstrTipoOperacion).Count

                    If intCantidadDetalleTesoreriarepetidosTipoOperacion > 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Ya existe un registro con ese tipo de operación," & " " & "no es posible repetir el registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
                ''''JCM 20160314, VERIFICAR QUE NO SE REPITA UN TIPO DE OPERACION



                objNuevaLista = ListaDetalleTesoreria
                'objNuevaLista.Add(objNvoDetalle)
                objNuevaLista.Add(New CompaniasTesoreria With {.intIDConfigTesoreria = -New Random().Next(0, 1000000),
                                                                .strTipoOperacion = pstrTipoOperacion,
                                                                .strTipoOperacionDescripcion = pstrTipoOperacionDescripcion,
                                                                .intDiasAplicaTransaccion = pintDiasAplicaTransaccion,
                                                                .lngIDConcepto = PlngIDConcepto,
                                                                .strDetalleConcepto = PstrDetalleConcepto,
                                                                .intDiasPago = PintDiasPago
                                                               })
                ListaDetalleTesoreria = objNuevaLista
                DetalleTesoreriaSeleccionado = _ListaDetalleTesoreria.First

                MyBase.CambioItem("ListaDetalleTesoreria")
                MyBase.CambioItem("ListaDetallePaginadaTesoreria")
                MyBase.CambioItem("DetalleTesoreriaSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle de tesoreria de la compañía. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle de limites de la  compañía.", Me.ToString(), "IngresarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try

            If Not IsNothing(ListaDetalle) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la tesoreria de la compañía.", Me.ToString(), "IngresarDetalleTesorería", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va a ingresar un registro en el detalle de condiciones tesoreria
    ''' Se debe llamar desde la pantalla cwCondicionesTesoreriaCompaniaView
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP063
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Agosto/206
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Agosto/206 - Resultado Ok 
    ''' </history>


    Public Sub IngresarDetalleCondicionesTesoreria(ByVal pstrTipoOperacion As String, ByVal pstrTipoOperacionDescripcion As String, plngIDConcepto As Integer, PstrDetalleConcepto As String)
        Try
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then
                Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasCondicionesTesoreria
                Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasCondicionesTesoreria)

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasCondicionesTesoreria)(mobjDetalleTesoreriaCondicionesPorDefecto, objNvoDetalle)

                'mobjDetalleLimitesPorDefecto.intIDCompaniaLimite = -New Random().Next(0, 1000000)


                If IsNothing(ListaDetalleCondicionesTesoreria) Then
                    ListaDetalleCondicionesTesoreria = New List(Of CFCalculosFinancieros.CompaniasCondicionesTesoreria)
                End If

                ''''JCM 20160314, VERIFICAR QUE NO SE REPITA UN TIPO DE OPERACION
                'CP065
                If _ListaDetalleCondicionesTesoreria IsNot Nothing Then
                    Dim intCantidadDetalleTesoreriarepetidosTipoOperacion = (From item In _ListaDetalleCondicionesTesoreria Select item.strTipoOperacionCT Where strTipoOperacionCT = pstrTipoOperacion).Count

                    If intCantidadDetalleTesoreriarepetidosTipoOperacion > 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Ya existe un registro con ese tipo de operación," & " " & "no es posible repetir el registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
                ''''JCM 20160314, VERIFICAR QUE NO SE REPITA UN TIPO DE OPERACION



                objNuevaLista = ListaDetalleCondicionesTesoreria
                'objNuevaLista.Add(objNvoDetalle)
                objNuevaLista.Add(New CompaniasCondicionesTesoreria With {.intIDCiaCondicionesTesoreria = -New Random().Next(0, 1000000),
                                                                .strTipoOperacionCT = pstrTipoOperacion,
                                                                .strTipoOperacionDescripcionCT = pstrTipoOperacionDescripcion,
                                                                .lngIDConceptoCT = plngIDConcepto,
                                                                .strDetalleConceptoCT = PstrDetalleConcepto
                                                              })
                ListaDetalleCondicionesTesoreria = objNuevaLista
                DetalleTesoreriaCondicioneSeleccionado = _ListaDetalleCondicionesTesoreria.First

                MyBase.CambioItem("ListaDetalleCondicionesTesoreria")
                MyBase.CambioItem("ListaDetalleCondicionesPaginadaTesoreria")
                MyBase.CambioItem("DetalleTesoreriaCondicioneSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle de condiciones tesoreria de la compañía. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle de condiciones tesoreria de la  compañía.", Me.ToString(), "IngresarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try

            If Not IsNothing(ListaDetalleCondicionesTesoreria) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de los limites de la compañía.", Me.ToString(), "IngresarDetalleCondicionesTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub




    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón editar del detalle en el Grid de Limites 
    ''' El registro Editado es visualizado en la ventana emergente cwConfiguracionlimitesCompañia
    ''' ID caso de prueba:  CP040
    ''' </summary>
    Public Sub EditarDetalleLimites(ByVal pstrTipoConcepto As String, ByVal pstrTipoConceptoDescripcion As String, ByVal plogValor As Boolean, ByVal pdblvalor As Double, ByVal plogPorentaje As Boolean, ByVal pdblPorcentaje As Double, ByVal Editandoelmismo As Boolean)
        Try
            IsBusy = True
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then
                Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasLimites
                Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasLimites)

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasLimites)(mobjDetalleLimitesPorDefecto, objNvoDetalle)

                'mobjDetalleLimitesPorDefecto.intIDCompaniaLimite = -New Random().Next(0, 1000000)

                ''''JCM 20160314, VERIFICAR QUE NO SE REPITA UN TIPO DE CONCEPTO
                If _ListaDetalleLimites IsNot Nothing Then


                    Dim intCantidadDetalleLimitesrepetidosTipoConcepto = (From item In _ListaDetalleLimites Select item.strTipoConcepto Where strTipoConcepto = pstrTipoConcepto).Count
                    If Editandoelmismo = True Then
                        If intCantidadDetalleLimitesrepetidosTipoConcepto > 1 Then
                            A2Utilidades.Mensajes.mostrarMensaje("No es posible ingresar este tipo de concepto ya que no puede exitir un tipo de concepto mas de una vez", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    Else
                        If intCantidadDetalleLimitesrepetidosTipoConcepto > 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("No es posible ingresar este tipo de concepto ya que no puede exitir un tipo de concepto mas de una vez", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                End If
                ''''JCM 20160314, VERIFICAR QUE NO SE REPITA UN TIPO DE CONCEPTO



                DetalleLimitesSeleccionado.strTipoConcepto = pstrTipoConcepto
                DetalleLimitesSeleccionado.strTipoConceptoDescripcion = pstrTipoConceptoDescripcion
                DetalleLimitesSeleccionado.logValor = plogValor
                DetalleLimitesSeleccionado.dblValor = pdblvalor
                DetalleLimitesSeleccionado.logPorcentaje = plogPorentaje
                DetalleLimitesSeleccionado.dblPorcentaje = pdblPorcentaje

                MyBase.CambioItem("ListaDetalleLimites")
                MyBase.CambioItem("ListaDetalleLimitesPaginada")
                MyBase.CambioItem("DetalleLimitesSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle de limites de la compañía. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle de limites de la  compañía.", Me.ToString(), "EditarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try
            IsBusy = False
            If Not IsNothing(ListaDetalle) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de los limites de la  compañía.", Me.ToString(), "IngresarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón editar del detalle en el Grid de Tesoreria 
    ''' El registro Editado es visualizado en la ventana emergente cwConfiguracionTesoreriaCompañiaView
    ''' ID caso de prueba:  
    ''' </summary>
    Public Sub EditarDetalleTesoreria(ByVal pstrTipoOperacion As String, ByVal pstrTipoOperacionDescripcion As String, ByVal pintDiasAplicaTransaccion As Integer, ByVal pintDiasPago As Integer, plngIDConcepto As Integer, ByVal pstrDetalleConcepto As String, ByVal EditandoElMismo As Boolean)
        Try
            IsBusy = True
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then
                Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasTesoreria
                Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasTesoreria)

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasTesoreria)(mobjDetalleTesoreriaPorDefecto, objNvoDetalle)

                'mobjDetalleLimitesPorDefecto.intIDCompaniaLimite = -New Random().Next(0, 1000000)



                ''''JCM 20160314, VERIFICAR QUE NO SE REPITA UN TIPO DE OPERACION
                If _ListaDetalleTesoreria IsNot Nothing Then


                    Dim intCantidadDetalleTesoreriarepetidosTipoOperacion = (From item In _ListaDetalleTesoreria Select item.strTipoOperacion Where strTipoOperacion = pstrTipoOperacion).Count
                    If EditandoElMismo = True Then
                        If intCantidadDetalleTesoreriarepetidosTipoOperacion > 1 Then
                            A2Utilidades.Mensajes.mostrarMensaje("No es posible ingresar este tipo de operación ya que no puede exitir un tipo de operación mas de una vez", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    Else
                        If intCantidadDetalleTesoreriarepetidosTipoOperacion > 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("No es posible ingresar este tipo de operación ya que no puede exitir un tipo de operación mas de una vez", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If


                End If
                ''''JCM 20160314, VERIFICAR QUE NO SE REPITA UN TIPO DE OPERACION


                DetalleTesoreriaSeleccionado.strTipoOperacion = pstrTipoOperacion
                DetalleTesoreriaSeleccionado.strTipoOperacionDescripcion = pstrTipoOperacionDescripcion
                DetalleTesoreriaSeleccionado.intDiasAplicaTransaccion = pintDiasAplicaTransaccion
                DetalleTesoreriaSeleccionado.intDiasPago = pintDiasPago
                DetalleTesoreriaSeleccionado.lngIDConcepto = plngIDConcepto
                DetalleTesoreriaSeleccionado.strDetalleConcepto = pstrDetalleConcepto





                MyBase.CambioItem("ListaDetalleTesoreria")
                MyBase.CambioItem("ListaDetallePaginadaTesoreria")
                MyBase.CambioItem("DetalleTesoreriaSeleccionado")
                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle de tesoreria de la compañía. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle de limites de la  compañía.", Me.ToString(), "EditarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try
            IsBusy = False
            If Not IsNothing(ListaDetalle) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de los limites de la  compañía.", Me.ToString(), "IngresarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón editar del detalle en el Grid de Condiciones Tesoreria 
    ''' El registro Editado es visualizado en la ventana emergente cwConfiguracionCondicionesTesoreriaCompañiaView
    ''' ID caso de prueba:  
    ''' </summary>
    Public Sub EditarDetalleCondicionesTesoreria(ByVal pstrTipoOperacion As String, ByVal pstrTipoOperacionDescripcion As String, plngIDConcepto As Integer, ByVal pstrDetalleConcepto As String, ByVal EditandoElMismo As Boolean)
        Try
            IsBusy = True
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then
                Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasCondicionesTesoreria
                Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasCondicionesTesoreria)

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasCondicionesTesoreria)(mobjDetalleTesoreriaCondicionesPorDefecto, objNvoDetalle)

                'mobjDetalleLimitesPorDefecto.intIDCompaniaLimite = -New Random().Next(0, 1000000)



                ''''JCM 20160817, VERIFICAR QUE NO SE REPITA UN TIPO DE OPERACION
                If _ListaDetalleCondicionesTesoreria IsNot Nothing Then


                    Dim intCantidadDetalleTesoreriarepetidosTipoOperacion = (From item In _ListaDetalleCondicionesTesoreria Select item.strTipoOperacionCT Where strTipoOperacionCT = pstrTipoOperacion).Count
                    If EditandoElMismo = True Then
                        If intCantidadDetalleTesoreriarepetidosTipoOperacion > 1 Then
                            A2Utilidades.Mensajes.mostrarMensaje("No es posible ingresar este tipo de operación ya que no puede exitir un tipo de operación mas de una vez", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    Else
                        If intCantidadDetalleTesoreriarepetidosTipoOperacion > 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("No es posible ingresar este tipo de operación ya que no puede exitir un tipo de operación mas de una vez", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If


                End If
                ''''JCM 20160817, VERIFICAR QUE NO SE REPITA UN TIPO DE OPERACION


                DetalleTesoreriaCondicioneSeleccionado.strTipoOperacionCT = pstrTipoOperacion
                DetalleTesoreriaCondicioneSeleccionado.strTipoOperacionDescripcionCT = pstrTipoOperacionDescripcion
                DetalleTesoreriaCondicioneSeleccionado.lngIDConceptoCT = plngIDConcepto
                DetalleTesoreriaCondicioneSeleccionado.strDetalleConceptoCT = pstrDetalleConcepto





                MyBase.CambioItem("ListaDetalleCondicionesTesoreria")
                MyBase.CambioItem("ListaDetalleCondicionesPaginadaTesoreria")
                MyBase.CambioItem("DetalleTesoreriaCondicioneSeleccionado")
                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle de condiciones tesoreria de la compañía. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle de condiciones tesoreria.", Me.ToString(), "EditarDetalleCondicionesTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try
            IsBusy = False
            If Not IsNothing(ListaDetalleCondicionesTesoreria) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de los limites de la  compañía.", Me.ToString(), "EditarDetalleCondicionesTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón editar del detalle en el Grid de parametros compania
    ''' El registro Editado es visualizado en la ventana emergente cwConfiguracionParametrosCompaniaView
    ''' ID caso de prueba:  
    ''' </summary>
    Public Sub EditarDetalleParametrosCompania(pstrValorParametro As String, ByVal plogManejaFechaParametro As Boolean, ByVal pdtmFechaInicialParametro As DateTime, ByVal pstrFechaInicialParametro As String)
        Try
            IsBusy = True
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then
                Dim objNvoDetalle As New CFCalculosFinancieros.ParametrosCompania
                Dim objNuevaLista As New List(Of CFCalculosFinancieros.ParametrosCompania)

                Program.CopiarObjeto(Of CFCalculosFinancieros.ParametrosCompania)(mobjDetalleParametrosCompaniaPorDefecto, objNvoDetalle)

                'mobjDetalleLimitesPorDefecto.intIDCompaniaLimite = -New Random().Next(0, 1000000)







                DetalleParametrosCompaniaSeleccionado.strValorParametro = pstrValorParametro
                If plogManejaFechaParametro = True Then
                    'DetalleParametrosCompaniaSeleccionado.dtmFechaInicialParametro = CType(pdtmFechaInicialParametro.ToShortDateString, Date?)
                    DetalleParametrosCompaniaSeleccionado.dtmFechaInicialParametro = pdtmFechaInicialParametro
                    'DetalleParametrosCompaniaSeleccionado.strFechaInicialParametro = pdtmFechaInicialParametro.ToString("yyyy-MM-dd")
                    'dtmFechaProceso.Value.ToString("yyyy-MM-dd")
                End If






                MyBase.CambioItem("ListaDetalleParametrosCompania")
                MyBase.CambioItem("ListaDetalleParametrosPaginadaCompania")
                MyBase.CambioItem("DetalleParametrosCompaniaSeleccionado")


                If _ListaDetalleParametrosCompania IsNot Nothing Then
                    Dim LL As Integer
                    Dim CantidadDecimalesComision As String = "0"
                    For LL = 0 To _ListaDetalleParametrosCompania.Count - 1
                        If _ListaDetalleParametrosCompania(LL).strParametro = "DECIMALES_COMISION_ADMINISTRACION" Then
                            CantidadDecimalesComision = _ListaDetalleParametrosCompania(LL).strValorParametro
                        End If
                    Next

                    If IsNumeric(CStr(CantidadDecimalesComision)) Then
                        DecimalesComision = CantidadDecimalesComision
                    Else
                        DecimalesComision = "2"
                    End If

                End If



                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle de parametros compañía. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle de parametros compañía.", Me.ToString(), "EditarDetalleParametrosCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try
            IsBusy = False
            If Not IsNothing(ListaDetalleParametrosCompania) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de los parametros de la compañía.", Me.ToString(), "IngresarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub





    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle en el Grid de Penalidades 
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle Penalidad para que el usuario ingrese el nuevo detalle
    ''' ID caso de prueba:  CP022
    ''' </summary>
    Public Sub IngresarDetallePenalidad()
        Try
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then

                Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasPenalidades
                'Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasPenalidades)
                Dim objn As New List(Of CFCalculosFinancieros.CompaniasPenalidades)

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasPenalidades)(mobjDetallePenalidadPorDefecto, objNvoDetalle)

                objNvoDetalle.intIDPenalidad = -New Random().Next(0, 1000000)

                If IsNothing(ListaPenalidad) Then
                    '' ''If Not Await LlenarComboValor() Then
                    '' ''    Exit Sub
                    '' ''End If
                    ListaPenalidad = New List(Of CompaniasPenalidades)
                    'ListaPenalidad = New List(Of CFCalculosFinancieros.CompaniasPenalidades)
                End If

                objn = ListaPenalidad
                objn.Add(objNvoDetalle)
                'objNuevaLista.Add(objNvoDetalle)
                ListaPenalidad = objn
                DetallePenalidadSeleccionado = _ListaPenalidad.First

                MyBase.CambioItem("ListaPenalidad")
                MyBase.CambioItem("ListaDetallePaginadaPenalidad")
                MyBase.CambioItem("DetallePenalidadSeleccionado")
                'MyBase.CambioItem("EncabezadoSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle", Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try

            If Not IsNothing(ListaPenalidad) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la compañía", Me.ToString(), "IngresarDetallePenalidad", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle en el Grid de Autorizaciones
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle Autorizaciones para que el usuario ingrese el nuevo detalle, Febrero 02/02/2016
    ''' ID caso de prueba:  CP042
    ''' </summary>
    Public Sub IngresarDetalleAutorizaciones()
        Try
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then

                Dim objNvoDetalleAutorizaciones As New CFCalculosFinancieros.CompaniasAutorizaciones
                'Dim objNuevaLista As New List(Of CFCalculosFinancieros.CompaniasPenalidades)
                Dim objn As New List(Of CFCalculosFinancieros.CompaniasAutorizaciones)

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasAutorizaciones)(mobjDetalleAutorizacionesPorDefecto, objNvoDetalleAutorizaciones)

                objNvoDetalleAutorizaciones.intIDCompaniaAutorizacion = -New Random().Next(0, 1000000)

                If IsNothing(ListaDetalleAutorizaciones) Then
                    '' ''If Not Await LlenarComboValor() Then
                    '' ''    Exit Sub
                    '' ''End If
                    ListaDetalleAutorizaciones = New List(Of CompaniasAutorizaciones)
                    'ListaPenalidad = New List(Of CFCalculosFinancieros.CompaniasPenalidades)
                End If

                objn = ListaDetalleAutorizaciones
                objn.Add(objNvoDetalleAutorizaciones)
                'objNuevaLista.Add(objNvoDetalle)
                ListaDetalleAutorizaciones = objn
                DetalleAutorizacionesSeleccionado = _ListaDetalleAutorizaciones.First

                MyBase.CambioItem("ListaDetalleAutorizaciones")
                MyBase.CambioItem("ListaDetalleAutorizacionesPaginada")
                MyBase.CambioItem("DetalleAutorizacionesSeleccionado")
                'MyBase.CambioItem("EncabezadoSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle de autorizaciones. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle de autorizaciones", Me.ToString(), "IngresarDetalleAutorizaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try

            If Not IsNothing(ListaDetalleAutorizaciones) Then

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de autorizaciones de la compañía", Me.ToString(), "IngresarDetalleAutorizaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub RefrescarDetalleAcumuladoComisiones(ByVal pstrTipoTabla As String)
        Try
            strTipoTabla = pstrTipoTabla

            If Not IsNothing(ListaAcumulacionComisiones) Then

                ListaAcumulacionComisionesPorTipoTabla = ListaAcumulacionComisiones.Where(Function(i) i.strTipoTabla = strTipoTabla).ToList()

                If ListaAcumulacionComisionesPorTipoTabla.Count > 0 Then
                    DetalleAcumuladoComisionesSeleccionado = ListaAcumulacionComisionesPorTipoTabla.Last
                End If

                MyBase.CambioItem("strTipoTabla")
                MyBase.CambioItem("ListaAcumulacionComisiones")
                MyBase.CambioItem("ListaAcumulacionComisionesPorTipoTabla")
                MyBase.CambioItem("ListaDetallePaginadaListaAcumulacionComisiones")
                MyBase.CambioItem("DetalleAcumuladoComisionesSeleccionado")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los detalles de comisiones acumuladas.", Me.ToString(), "IngresarDetalleAutorizaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle de Acumulado de Comisiones
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle de Acumulado de Comisiones para que el usuario ingrese el nuevo detalle
    ''' 
    ''' </summary>
    Public Sub IngresarDetalleAcumuladoComisiones()
        Try
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strNombre) Then

                Dim dblRango As Double = 0
                Dim objNvoDetalle As New CFCalculosFinancieros.CompaniasAcumuladoComisiones

                'mobjDetalleAcumuladosComisionesPorDefecto = -1
                If Not IsNothing(ListaAcumulacionComisionesPorTipoTabla) Then
                    If ListaAcumulacionComisionesPorTipoTabla.Count >= 1 Then
                        If DetalleAcumuladoComisionesSeleccionado.logValorMaximo = True Then
                            Exit Sub
                        End If
                    End If
                End If

                If Not IsNothing(ListaAcumulacionComisionesPorTipoTabla) Then
                    If ListaAcumulacionComisionesPorTipoTabla.Count >= 1 Then
                        DetalleAcumuladoComisionesSeleccionado = ListaAcumulacionComisionesPorTipoTabla.Last
                        If Not IsNothing(DetalleAcumuladoComisionesSeleccionado.dblRangoFinal) Then
                            dblRango = CDbl(DetalleAcumuladoComisionesSeleccionado.dblRangoFinal)
                        End If
                    End If
                End If

                Program.CopiarObjeto(Of CFCalculosFinancieros.CompaniasAcumuladoComisiones)(mobjDetalleAcumuladosComisionesPorDefecto, objNvoDetalle)

                objNvoDetalle.IDCompaniasAcumuladoComisiones = -New Random().Next(0, 1000000)
                objNvoDetalle.strComisionOrigen = "A"
                objNvoDetalle.strTipoTabla = strTipoTabla
                objNvoDetalle.dblRangoInicio = 0

                If IsNothing(ListaAcumulacionComisiones) Then
                    ListaAcumulacionComisiones = New List(Of CompaniasAcumuladoComisiones)
                End If

                ListaAcumulacionComisiones.Add(objNvoDetalle)
                DetalleAcumuladoComisionesSeleccionado = ListaAcumulacionComisiones.Last
                ListaAcumulacionComisionesPorTipoTabla = ListaAcumulacionComisiones.Where(Function(i) i.strTipoTabla = _strTipoTabla).ToList()

                If dblRango > 0 Then
                    DetalleAcumuladoComisionesSeleccionado.dblRangoInicio = dblRango + 1
                End If

                MyBase.CambioItem("ListaAcumulacionComisiones")
                MyBase.CambioItem("ListaAcumulacionComisionesPorTipoTabla")
                MyBase.CambioItem("ListaDetallePaginadaListaAcumulacionComisiones")
                MyBase.CambioItem("DetalleAcumuladoComisionesSeleccionado")

            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la compañía para poder agregar un detalle. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle para el acumulado de comisiones", Me.ToString(), "IngresarDetalleAcumuladoComisiones", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub





    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle 
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP007
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history> 
    Public Sub BorrarDetalle()
        Try
            If Not IsNothing(DetalleSeleccionado) Then

                If _ListaDetalle.Where(Function(i) i.intIDCompaniaCodigo = _DetalleSeleccionado.intIDCompaniaCodigo).Count > 0 Then
                    _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) i.intIDCompaniaCodigo = _DetalleSeleccionado.intIDCompaniaCodigo).First)
                End If

                Dim objNuevaListaDetalle As New List(Of CompaniasCodigos)

                For Each li In _ListaDetalle
                    objNuevaListaDetalle.Add(li)
                Next

                If objNuevaListaDetalle.Where(Function(i) i.intIDCompaniaCodigo = _DetalleSeleccionado.intIDCompaniaCodigo).Count > 0 Then
                    objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intIDCompaniaCodigo = _DetalleSeleccionado.intIDCompaniaCodigo).First)
                End If

                ListaDetalle = objNuevaListaDetalle

                If Not IsNothing(_ListaDetalle) Then
                    If _ListaDetalle.Count > 0 Then
                        DetalleSeleccionado = _ListaDetalle.First
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la compañía", Me.ToString(), "BorrarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle de acumulado de comisiones
    ''' Solamente se elimina el registro de la lista del detalle del acumulado de comisiones pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    ''' <history>
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 8 de 2017
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 8 de 2017 - Resultado Ok 
    ''' </history> 


    Public Sub BorrarDetalleAcumulacionComsiones()

        Try

            If Not IsNothing(DetalleAcumuladoComisionesSeleccionado) Then

                ListaAcumulacionComisiones.Remove(DetalleAcumuladoComisionesSeleccionado)
                ListaAcumulacionComisionesPorTipoTabla.Remove(DetalleAcumuladoComisionesSeleccionado)

                If Not IsNothing(ListaAcumulacionComisionesPorTipoTabla) Then
                    If ListaAcumulacionComisionesPorTipoTabla.Count > 0 Then
                        DetalleAcumuladoComisionesSeleccionado = ListaAcumulacionComisionesPorTipoTabla.Last
                    End If
                End If

                MyBase.CambioItem("ListaAcumulacionComisiones")
                MyBase.CambioItem("ListaAcumulacionComisionesPorTipoTabla")
                MyBase.CambioItem("ListaDetallePaginadaListaAcumulacionComisiones")
                MyBase.CambioItem("DetalleAcumuladoComisionesSeleccionado")

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle del acumulado de comisiones", Me.ToString(), "BorrarDetalleAcumulacionComsiones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub






    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle penalidad
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP023
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history> 


    Public Sub BorrarDetallePenalidad()

        Try
            'If ValidarBorrarConfirmacion("Penalidad") = True Then




            If Not IsNothing(DetallePenalidadSeleccionado) Then

                If _ListaPenalidad.Where(Function(i) i.intIDPenalidad = _DetallePenalidadSeleccionado.intIDPenalidad).Count > 0 Then
                    _ListaPenalidad.Remove(_ListaPenalidad.Where(Function(i) i.intIDPenalidad = _DetallePenalidadSeleccionado.intIDPenalidad).First)
                End If

                Dim objNuevaListaDetallePenalidad As New List(Of CompaniasPenalidades)

                For Each li In _ListaPenalidad
                    objNuevaListaDetallePenalidad.Add(li)
                Next

                If objNuevaListaDetallePenalidad.Where(Function(i) i.intIDPenalidad = _DetallePenalidadSeleccionado.intIDPenalidad).Count > 0 Then
                    objNuevaListaDetallePenalidad.Remove(objNuevaListaDetallePenalidad.Where(Function(i) i.intIDPenalidad = _DetallePenalidadSeleccionado.intIDPenalidad).First)
                End If

                ListaPenalidad = objNuevaListaDetallePenalidad

                If Not IsNothing(_ListaPenalidad) Then
                    If _ListaPenalidad.Count > 0 Then
                        DetallePenalidadSeleccionado = _ListaPenalidad.First
                    End If
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la compañía", Me.ToString(), "BorrarDetallePenalidad", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle tesoreria
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP064
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history>

    Public Sub BorrarDetalleTesoreria()
        Try
            If Not IsNothing(DetalleTesoreriaSeleccionado) Then

                If _ListaDetalleTesoreria.Where(Function(i) i.intIDConfigTesoreria = _DetalleTesoreriaSeleccionado.intIDConfigTesoreria).Count > 0 Then
                    _ListaDetalleTesoreria.Remove(_ListaDetalleTesoreria.Where(Function(i) i.intIDConfigTesoreria = _DetalleTesoreriaSeleccionado.intIDConfigTesoreria).First)
                End If

                Dim objNuevaListaDetalleTesoreria As New List(Of CompaniasTesoreria)

                For Each li In _ListaDetalleTesoreria
                    objNuevaListaDetalleTesoreria.Add(li)
                Next

                If objNuevaListaDetalleTesoreria.Where(Function(i) i.intIDConfigTesoreria = _DetalleTesoreriaSeleccionado.intIDConfigTesoreria).Count > 0 Then
                    objNuevaListaDetalleTesoreria.Remove(objNuevaListaDetalleTesoreria.Where(Function(i) i.intIDConfigTesoreria = _DetalleTesoreriaSeleccionado.intIDConfigTesoreria).First)
                End If

                ListaDetalleTesoreria = objNuevaListaDetalleTesoreria

                If Not IsNothing(_ListaDetalleTesoreria) Then
                    If _ListaDetalleTesoreria.Count > 0 Then
                        DetalleTesoreriaSeleccionado = _ListaDetalleTesoreria.First
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la tesoreria", Me.ToString(), "BorrarDetalleTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle tesoreria codiciones
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP064
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Agosto 17 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Agosto 17 de 2016 - Resultado Ok 
    ''' </history>

    Public Sub BorrarDetalleTesoreriaCondiciones()
        Try
            If Not IsNothing(DetalleTesoreriaCondicioneSeleccionado) Then

                If _ListaDetalleCondicionesTesoreria.Where(Function(i) i.intIDCiaCondicionesTesoreria = _DetalleTesoreriaCondicioneSeleccionado.intIDCiaCondicionesTesoreria).Count > 0 Then
                    _ListaDetalleCondicionesTesoreria.Remove(_ListaDetalleCondicionesTesoreria.Where(Function(i) i.intIDCiaCondicionesTesoreria = _DetalleTesoreriaCondicioneSeleccionado.intIDCiaCondicionesTesoreria).First)
                End If

                Dim objNuevaListaDetalleTesoreria As New List(Of CompaniasCondicionesTesoreria)

                For Each li In _ListaDetalleCondicionesTesoreria
                    objNuevaListaDetalleTesoreria.Add(li)
                Next

                If objNuevaListaDetalleTesoreria.Where(Function(i) i.intIDCiaCondicionesTesoreria = _DetalleTesoreriaCondicioneSeleccionado.intIDCiaCondicionesTesoreria).Count > 0 Then
                    objNuevaListaDetalleTesoreria.Remove(objNuevaListaDetalleTesoreria.Where(Function(i) i.intIDCiaCondicionesTesoreria = _DetalleTesoreriaCondicioneSeleccionado.intIDCiaCondicionesTesoreria).First)
                End If

                ListaDetalleCondicionesTesoreria = objNuevaListaDetalleTesoreria

                If Not IsNothing(_ListaDetalleCondicionesTesoreria) Then
                    If _ListaDetalleCondicionesTesoreria.Count > 0 Then
                        DetalleTesoreriaCondicioneSeleccionado = _ListaDetalleCondicionesTesoreria.First
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de las condiciones tesoreria", Me.ToString(), "BorrarDetalleTesoreriaCondiciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub





    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle limites
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP039
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history>

    Public Sub BorrarDetalleLimites()
        Try
            If Not IsNothing(DetalleLimitesSeleccionado) Then

                If _ListaDetalleLimites.Where(Function(i) i.intIDCompaniaLimite = _DetalleLimitesSeleccionado.intIDCompaniaLimite).Count > 0 Then
                    _ListaDetalleLimites.Remove(_ListaDetalleLimites.Where(Function(i) i.intIDCompaniaLimite = _DetalleLimitesSeleccionado.intIDCompaniaLimite).First)
                End If

                Dim objNuevaListaDetalleTesoreria As New List(Of CompaniasLimites)

                For Each li In _ListaDetalleLimites
                    objNuevaListaDetalleTesoreria.Add(li)
                Next

                If objNuevaListaDetalleTesoreria.Where(Function(i) i.intIDCompaniaLimite = _DetalleLimitesSeleccionado.intIDCompaniaLimite).Count > 0 Then
                    objNuevaListaDetalleTesoreria.Remove(objNuevaListaDetalleTesoreria.Where(Function(i) i.intIDCompaniaLimite = _DetalleLimitesSeleccionado.intIDCompaniaLimite).First)
                End If

                ListaDetalleLimites = objNuevaListaDetalleTesoreria

                If Not IsNothing(_ListaDetalleLimites) Then
                    If _ListaDetalleLimites.Count > 0 Then
                        DetalleLimitesSeleccionado = _ListaDetalleLimites.First
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de los limites", Me.ToString(), "BorrarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle de Autorizaciones 
    ''' Solamente se elimina el registro de la lista del detalle de Autorizaciones pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP043
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history>


    Public Sub BorrarDetalleAutorizaciones()
        Try
            If Not IsNothing(DetalleAutorizacionesSeleccionado) Then

                If _ListaDetalleAutorizaciones.Where(Function(i) i.intIDCompaniaAutorizacion = _DetalleAutorizacionesSeleccionado.intIDCompaniaAutorizacion).Count > 0 Then
                    _ListaDetalleAutorizaciones.Remove(_ListaDetalleAutorizaciones.Where(Function(i) i.intIDCompaniaAutorizacion = _DetalleAutorizacionesSeleccionado.intIDCompaniaAutorizacion).First)
                End If

                Dim objNuevaListaDetalleAutorizaciones As New List(Of CompaniasAutorizaciones)

                For Each li In _ListaDetalleAutorizaciones
                    objNuevaListaDetalleAutorizaciones.Add(li)
                Next

                If objNuevaListaDetalleAutorizaciones.Where(Function(i) i.intIDCompaniaAutorizacion = _DetalleAutorizacionesSeleccionado.intIDCompaniaAutorizacion).Count > 0 Then
                    objNuevaListaDetalleAutorizaciones.Remove(objNuevaListaDetalleAutorizaciones.Where(Function(i) i.intIDCompaniaAutorizacion = _DetalleAutorizacionesSeleccionado.intIDCompaniaAutorizacion).First)
                End If

                ListaDetalleAutorizaciones = objNuevaListaDetalleAutorizaciones

                If Not IsNothing(_ListaDetalleAutorizaciones) Then
                    If _ListaDetalleAutorizaciones.Count > 0 Then
                        DetalleAutorizacionesSeleccionado = _ListaDetalleAutorizaciones.First
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de autorizaciones", Me.ToString(), "BorrarDetalleAutorizaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Property detalle seleccionado
    ''' </summary>
    ''' <history>
    ''' Descripción      : Sub para manejar los eventos del detalle del property changed              
    ''' Fecha            : Febrero 11/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Febrero 11/2016 - Resultado Ok 
    ''' 'JCM20160211
    ''' </history>

    Private Sub _DetalleSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleSeleccionado.PropertyChanged
        Try
            If Editando Then
                Select Case e.PropertyName
                    Case "lngIDComitente"
                        _mlogBuscarClienteEncabezado = False
                        _mLogBuscarClienteDetalle = True
                        If Not String.IsNullOrEmpty(CStr(_DetalleSeleccionado.lngIDComitente)) And _mLogBuscarClienteDetalle Then
                            If _DetalleSeleccionado.lngIDComitente <> "0" Then buscarComitente(CStr(_DetalleSeleccionado.lngIDComitente))
                        End If
                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_DetalleSeleccionado.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Property detalle seleccionado de acumulacion de comisiones
    ''' </summary>
    ''' <history>
    ''' Descripción      : Sub para manejar los eventos del detalle del property changed  del acumulado de comisiones            
    ''' Fecha            : Mayo 8 de 2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Mayo 8 de 2016 - Resultado Ok 
    ''' 'JCM20170508
    ''' </history>

    Private Sub _DetalleAcumuladoComisionesSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleAcumuladoComisionesSeleccionado.PropertyChanged
        Try
            If Editando Then
                Select Case e.PropertyName
                    Case "logValorMaximo"
                        If DetalleAcumuladoComisionesSeleccionado.logValorMaximo = True Then
                            DetalleAcumuladoComisionesSeleccionado.dblRangoFinal = Program.ValorMaximoRangoAcumuladoComisiones
                        Else
                            DetalleAcumuladoComisionesSeleccionado.dblRangoFinal = 0
                        End If
                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del acumulado de comisiones",
                                 Me.ToString(), "_DetalleAcumuladoComisionesSeleccionado.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Se ejecuta cuando el usuario hace un cambio en el combo del Tipo de participación y se debe eliminar datos del Grid
    ''' Se eliminana los datos dependiendo del Tipo de Participación
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP031
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Febrero 8 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Febrero 8 de 2016 - Resultado Ok 
    ''' </history> 

    Public Sub BorrarDetalleEspecifico(ByVal pOpcion As String)
        Dim logIterar As Boolean
        Try
            Select Case pOpcion
                Case "BorrarCodigosOyD"
                    logIterar = True
                    Do While logIterar = True


                        If _ListaDetalle.Where(Function(i) CBool(i.logCompania) = False).Count > 0 Then
                            _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) CBool(i.logCompania) = False).First)
                        End If


                        Dim objNuevaListaDetalle As New List(Of CompaniasCodigos)

                        For Each li In _ListaDetalle
                            objNuevaListaDetalle.Add(li)
                        Next


                        ListaDetalle = objNuevaListaDetalle

                        If Not IsNothing(_ListaDetalle) Then
                            If _ListaDetalle.Count > 0 Then
                                DetalleSeleccionado = _ListaDetalle.First
                            End If
                        End If

                        If _ListaDetalle.Where(Function(i) CBool(i.logCompania) = False).Count > 0 Then
                            '_ListaDetalle.Remove(_ListaDetalle.Where(Function(i) CBool(i.logCompania) = False).First)
                            logIterar = True
                        Else
                            logIterar = False
                        End If


                    Loop


                Case "BorrarCompanias"
                    logIterar = True
                    Do While logIterar = True


                        If _ListaDetalle.Where(Function(i) CBool(i.logCompania) = True).Count > 0 Then
                            _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) CBool(i.logCompania) = True).First)
                        End If


                        Dim objNuevaListaDetalle As New List(Of CompaniasCodigos)

                        For Each li In _ListaDetalle
                            objNuevaListaDetalle.Add(li)
                        Next


                        ListaDetalle = objNuevaListaDetalle

                        If Not IsNothing(_ListaDetalle) Then
                            If _ListaDetalle.Count > 0 Then
                                DetalleSeleccionado = _ListaDetalle.First
                            End If
                        End If


                        If _ListaDetalle.Where(Function(i) CBool(i.logCompania) = True).Count > 0 Then
                            '_ListaDetalle.Remove(_ListaDetalle.Where(Function(i) CBool(i.logCompania) = False).First)
                            logIterar = True
                        Else
                            logIterar = False
                        End If


                    Loop

                Case "BorrarTodo"
                    logIterar = True

                    Do While logIterar = True

                        If _ListaDetalle.Where(Function(i) CBool(i.logCompania) = True Or CBool(i.logCompania) = False).Count > 0 Then
                            _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) CBool(i.logCompania) = True Or CBool(i.logCompania) = False).First)
                        End If


                        Dim objNuevaListaDetalle As New List(Of CompaniasCodigos)

                        For Each li In _ListaDetalle
                            objNuevaListaDetalle.Add(li)
                        Next


                        ListaDetalle = objNuevaListaDetalle

                        If Not IsNothing(_ListaDetalle) Then
                            If _ListaDetalle.Count > 0 Then
                                DetalleSeleccionado = _ListaDetalle.First
                            End If
                        End If


                        If _ListaDetalle.Where(Function(i) CBool(i.logCompania) = True Or CBool(i.logCompania) = False).Count > 0 Then
                            '_ListaDetalle.Remove(_ListaDetalle.Where(Function(i) CBool(i.logCompania) = False).First)
                            logIterar = True
                        Else
                            logIterar = False
                        End If




                    Loop

            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la compañía", Me.ToString(), "BorrarDetalleEspecifico", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario hace un cambio en el combo del Tipo de participación y se debe valida si existen datos para se eliminados o no
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP031
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Febrero 8 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Febrero 8 de 2016 - Resultado Ok 
    ''' </history> 

    Private Function ValidarDatosExistentes(ByVal pOpcion As String) As Boolean
        Dim logDatosValidarExistentes As Boolean = False
        Try
            Select Case pOpcion
                Case "BorrarCodigosOyD"
                    If Not IsNothing(_ListaDetalle) Then
                        If Not IsNothing(_DetalleSeleccionado) Then
                            If Not IsNothing(_DetalleSeleccionado.logCompania) Then
                                If _ListaDetalle.Where(Function(i) CBool(i.logCompania) = False).Count > 0 Then
                                    logDatosValidarExistentes = True
                                    Return (logDatosValidarExistentes)
                                    Exit Function
                                End If
                            End If
                        End If

                    End If
                Case "BorrarCompanias"
                    If Not IsNothing(_ListaDetalle) Then
                        If Not IsNothing(_DetalleSeleccionado) Then
                            If Not IsNothing(_DetalleSeleccionado.logCompania) Then
                                If _ListaDetalle.Where(Function(i) CBool(i.logCompania) = True).Count > 0 Then
                                    logDatosValidarExistentes = True
                                    Return (logDatosValidarExistentes)
                                    Exit Function
                                End If
                            End If
                        End If
                    End If
                Case "BorrarTodo"
                    If Not IsNothing(_ListaDetalle) Then
                        If Not IsNothing(_DetalleSeleccionado) Then
                            If Not IsNothing(_DetalleSeleccionado.logCompania) Then
                                If _ListaDetalle.Where(Function(i) CBool(i.logCompania) = True Or CBool(i.logCompania) = False).Count > 0 Then
                                    logDatosValidarExistentes = True
                                    Return (logDatosValidarExistentes)
                                    Exit Function
                                End If
                            End If
                        End If
                    End If
                Case Else
                    logDatosValidarExistentes = False
            End Select


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la compañía", Me.ToString(), "ValidarDatosExistentes", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logDatosValidarExistentes)
    End Function

    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de Limites.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Febrero 8 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Febrero 8 de 2016 - Resultado Ok 
    ''' </history>  

    Public Sub ValidarBorrarConfirmacionLimites()
        Dim strMsg As String
        strMsg = "Esta seguro que desea eliminar el registro"
        A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntarDetalleLimites)
    End Sub

    ''' <summary>
    ''' Dependiendo si se elimina o no el Detalle del Grid de Limites
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Febrero 8 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) -  Febrero 8 de 2016 - Resultado Ok 
    ''' </history>  

    Private Sub TerminoPreguntarDetalleLimites(ByVal sender As Object, e As System.EventArgs)
        Try

            If Not IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    BorrarDetalleLimites()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de borrar el detalle Limites",
             Me.ToString(), "TerminoPreguntarDetalleLimites", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de Detalles.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history> 
    Public Sub ValidarBorrarConfirmacionDetalle()
        Dim strMsg As String
        strMsg = "Esta seguro que desea eliminar el registro"
        A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntarDetalle)
    End Sub


    ''' <summary>
    ''' Validá si ya existen códigos OyD en el detalle, cuando se ingresa un cliente en el encabezado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP070
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history> 
    Public Sub ValidarCodigosOyDExistentes(ByVal pstrDto As String)
        Try
            strNroDto = pstrDto

            If _ListaDetalle IsNot Nothing Then
                'If _ListaDetalle.Count > 0 Then
                '    Dim intCantidadCompanias = (From item In _ListaDetalle Select item.intIDCompania).Count
                '    If intCantidadCompanias > 0 Then
                '        Dim strMsg As String
                '        strMsg = "Ya existen registros de códigos en el detalle, solo quedaran los codigos asociados al cliente, ¿Desea Continuar?"
                '        A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntarDetalleCodigosOyD)
                '    End If
                'Else
                If _EncabezadoSeleccionado.strTipoCompania = "PP" Then
                    ConsultarCodigosOyD(strNroDto)
                Else
                    IngresarDetallePorDefecto(strNroDto)
                    'End If
                End If
            Else
                If _EncabezadoSeleccionado.strTipoCompania = "PP" Then
                    ConsultarCodigosOyD(strNroDto)
                Else
                    IngresarDetallePorDefecto(strNroDto)
                End If
            End If






        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al momento de validar los códigos OyD", Me.ToString(), "ValidarCodigosOyDExistentes", Application.Current.ToString(), Program.Maquina, ex)
        End Try



    End Sub

    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de Detalles.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history> 
    Private Sub TerminoPreguntarDetalle(ByVal sender As Object, e As System.EventArgs)
        Try

            If Not IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    BorrarDetalle()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de borrar el detalle limites",
             Me.ToString(), "TerminoPreguntarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de Tesoreria.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history> 
    Public Sub ValidarBorrarConfirmacionTesoreria()
        Dim strMsg As String
        strMsg = "Esta seguro que desea eliminar el registro"
        A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntarDetalleTesoreria)
    End Sub


    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de condiciones Tesoreria.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Agosto 17 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Agosto 17 de 2016 - Resultado Ok 
    ''' </history> 
    Public Sub ValidarBorrarConfirmacionTesoreriaCondiciones()
        Dim strMsg As String
        strMsg = "Esta seguro que desea eliminar el registro"
        A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntarDetalleTesoreriaCondiciones)
    End Sub

    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de Detalles de Tesoreria.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history> 
    Private Sub TerminoPreguntarDetalleTesoreria(ByVal sender As Object, e As System.EventArgs)
        Try
            If Not IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    BorrarDetalleTesoreria()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de borrar el detalle tesoreria",
             Me.ToString(), "TerminoPreguntarDetalleTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de Detalles de Tesoreria Condiciones.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Agosto 17 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Agosto 17 de 2016 - Resultado Ok 
    ''' </history> 
    Private Sub TerminoPreguntarDetalleTesoreriaCondiciones(ByVal sender As Object, e As System.EventArgs)
        Try
            If Not IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    BorrarDetalleTesoreriaCondiciones()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de borrar el detalle de condiciones tesoreria",
             Me.ToString(), "TerminoPreguntarDetalleTesoreriaCondiciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub




    ''' <summary>
    ''' Sub para verificar los códigos OyD que pertenecen al documento ingresado en el encabezado
    ''' ID caso de prueba:  CP070
    ''' </summary>
    ''' <history>
    ''' Descripción      : Se verifican todos los códigos que estan relaciondos con el documento del encabezado
    ''' Fecha            : Abril 26/2016
    ''' Pruebas CB       : Juan Camilo Munera (Alcuadrado S.A.) - Abril 26/2016 - Resultado Ok 
    ''' 'JCM2016426
    ''' </history>

    Private Sub TerminoPreguntarDetalleCodigosOyD(ByVal sender As Object, e As System.EventArgs)
        Try
            If Not IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    ConsultarCodigosOyD(strNroDto)
                    strNroDocumentoActual = strNroDto
                    strCodigoSeleccionado = strCodigoOYDEncabezado
                Else
                    logPreguntarCambio = False
                    EncabezadoSeleccionado.strNumeroDocumento = strNroDocumentoActual
                    ObtenerCodigoOyDCliente(EncabezadoSeleccionado.strNumeroDocumento, True)
                    'buscarComitenteCambiado(EncabezadoSeleccionado.strNumeroDocumento, "")
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de ingresar los códigos OyD",
             Me.ToString(), "TerminoPreguntarDetalleCodigosOyD", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub




    ''' <summary>
    ''' Se pregunta si en realidad si existe un pacto de permanencia en si.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP007
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history>  

    Private Sub VerificarDatosEncabezado()
        Dim strg As String
        If _EncabezadoSeleccionado.strPactoPermanencia = "S" Then
            If _EncabezadoSeleccionado.intDiasPlazo <= 0 Then
                strg = "Debido a que se escogío pacto de permanencia, los dias de plazo deben ser mayores que 0"
            End If

            If _EncabezadoSeleccionado.intDiasGracia <= 0 Then
                strg = "Debido a que se escogío pacto de permanencia, el periodo de gracia deben ser mayores que 0"
            End If

            If _EncabezadoSeleccionado.strPenalidad = "p" Then
                strg = "Debido a que se escogío pacto de permanencia, el periodo de gracia deben ser mayores que 0"
            End If
        End If
    End Sub

    ''' <summary>
    ''' Se pregunta si en realidad si en el combo de Autorizacion esta en la opcion de todos para limpiar la informacion que pueda tener el grid de autorizaciones
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP041
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Mayo 13 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Mayo 13 de 2016 - Resultado Ok 
    ''' </history> 
    Private Sub ValidarAutorizacion()
        If _EncabezadoSeleccionado.strTipoAutorizados <> "C" Then
            If Not IsNothing(_ListaDetalleAutorizaciones) Then
                If Not IsNothing(_DetalleAutorizacionesSeleccionado) Then
                    If _ListaDetalleAutorizaciones.Where(Function(i) i.intIDCompaniaAutorizacion <> 0).Count > 0 Then
                        _ListaDetalleAutorizaciones.Remove(_ListaDetalleAutorizaciones.Where(Function(i) i.intIDCompaniaAutorizacion <> 0).First)
                    End If

                    Dim objnuevaListaDetalleAutorizaciones As New List(Of CompaniasAutorizaciones)

                    For Each li In _ListaDetalleAutorizaciones
                        objnuevaListaDetalleAutorizaciones.Add(li)
                    Next

                    If objnuevaListaDetalleAutorizaciones.Where(Function(i) i.intIDCompaniaAutorizacion <> 0).Count > 0 Then
                        objnuevaListaDetalleAutorizaciones.Remove(objnuevaListaDetalleAutorizaciones.Where(Function(i) i.intIDCompaniaAutorizacion <> 0).First)
                    End If

                    ListaDetalleAutorizaciones = objnuevaListaDetalleAutorizaciones

                    If Not IsNothing(_ListaDetalleAutorizaciones) Then
                        If _ListaDetalleAutorizaciones.Count > 0 Then
                            DetalleAutorizacionesSeleccionado = _ListaDetalleAutorizaciones.First
                        End If
                    End If
                End If
            End If
        End If


    End Sub

    ''' <summary>
    ''' Funcion encargada de convertir el texto para poderle enviar los valores html
    ''' </summary>
    ''' <history>
    ''' Creado por:        Juan Camilo Múnera (Alcuadrado S.A.)
    ''' Fecha:             Octubre 25 de 2016
    ''' Pruebas CB:        Juan Camilo Múnera (Alcuadrado S.A.) - Octubre 25 de 2016 - Resultado Ok 
    ''' </history> 
    Private Function ConvertirTexto(ByVal strTexto As String) As String
        strTexto = Replace(strTexto, "<", "{##}")
        strTexto = Replace(strTexto, """", "{###}")
        strTexto = Replace(strTexto, "&", "{####}")
        Return strTexto
    End Function

    Private Function Convertirfecha(ByVal dtmFecha As DateTime) As Date

        Return dtmFecha
    End Function

    Public Sub FiltrarInformacion(ByVal pstrFiltro As String)
        Try
            Dim objListaInformacion As New List(Of ParametrosCompania)

            'ActualizarSeleccionados()

            If Not IsNothing(ListaDetalleParametrosCompania) Then

                If String.IsNullOrEmpty(pstrFiltro) Then
                    For Each li In ListaParametrosSeleccioando
                        objListaInformacion.Add(li)
                    Next
                Else
                    For Each li In ListaParametrosSeleccioando
                        If li.strDescripcionParametro.ToUpper.Contains(pstrFiltro.ToUpper) Then
                            objListaInformacion.Add(li)
                        End If
                    Next
                End If
            End If

            ListaDetalleParametrosCompania = Nothing
            ListaDetalleParametrosCompania = objListaInformacion
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar la información.", Me.ToString(),
                                                         "FiltrarInformacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Sub ActualizarSeleccionados()
        Try
            If Not IsNothing(ListaParametrosSeleccioando) And Not IsNothing(ListaParametrosSeleccioando) Then
                If ListaParametrosSeleccioando.Count > 0 Then
                    For Each li In ListaDetalleParametrosCompania
                        If ListaParametrosSeleccioando.Where(Function(i) i.strValorParametro = li.strValorParametro).Count > 0 Then
                            Dim objActualizarRegistro = ListaParametrosSeleccioando.Where(Function(i) i.strValorParametro = li.strValorParametro).First
                            objActualizarRegistro.strValorParametro = li.strValorParametro
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar los registros seleccionados.", Me.ToString(), "ActualizarSeleccionados", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class


''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
''' 
Public Class CamposBusquedaCompanias
    Implements INotifyPropertyChanged

    Private _strTipoDocumento As String
    Public Property strTipoDocumento() As String
        Get
            Return _strTipoDocumento
        End Get
        Set(ByVal value As String)
            _strTipoDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoDocumento"))
        End Set
    End Property


    Private _strNumeroDocumento As String
    Public Property strNumeroDocumento() As String
        Get
            Return _strNumeroDocumento
        End Get
        Set(ByVal value As String)
            _strNumeroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNumeroDocumento"))
        End Set
    End Property

    Private _strNombre As String
    Public Property strNombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombre"))
        End Set
    End Property

    Private _strTipoCompania As String
    Public Property strTipoCompania() As String
        Get
            Return _strTipoCompania
        End Get
        Set(ByVal value As String)
            _strTipoCompania = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoCompania"))
        End Set
    End Property


    Private _strTipoPlazo As String
    Public Property strTipoPlazo() As String
        Get
            Return _strTipoPlazo
        End Get
        Set(ByVal value As String)
            _strTipoPlazo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoPlazo"))
        End Set
    End Property


    Private _strParticipacion As String
    Public Property strParticipacion() As String
        Get
            Return _strParticipacion
        End Get
        Set(ByVal value As String)
            _strParticipacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strParticipacion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class