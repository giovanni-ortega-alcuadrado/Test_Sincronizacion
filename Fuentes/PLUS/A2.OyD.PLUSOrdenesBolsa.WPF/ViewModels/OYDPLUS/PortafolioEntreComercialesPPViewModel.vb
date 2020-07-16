Imports Telerik.Windows.Controls
'ViewModel para el registro portafolio entre receptores
'Mayo 19 de 2014
'Giovanny Velez
'-------------------------------------------------------------------------
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports OpenRiaServices.DomainServices.Client

Public Class PortafolioEntreComercialesPPViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Constantes y definición de variables"

    ''' <summary>
    ''' Definición de variables y constantes utilizadas en el view Model
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private OperacionPorReceptorPorDefecto As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor
    Private OperacionPorReceptorAnterior As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor
    Private dcProxy As OYDPLUSOrdenesBolsaDomainContext
    Private dcProxy1 As OYDPLUSOrdenesBolsaDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Private mdcProxyUtilidad03 As UtilidadesDomainContext
    Private mdcProxyUtilidadPLUS As OyDPLUSutilidadesDomainContext

    Private Const MLOG_TRASLADOENDINERO As Boolean = False
    Private MINT_LONG_MAX_CODIGO_OYD As Byte = 17
    Public TIPONEGOCIO_ACCIONES As String = ""
    Public TIPONEGOCIO_REPO As String = ""
    Public TIPONEGOCIO_SIMULTANEA As String = ""
    Public TIPONEGOCIO_RENTAFIJA As String = ""
    Public TIPONEGOCIO_TTV As String = ""
    Public TIPONEGOCIO_ADR As String = ""
    Public TIPONEGOCIO_REPOC As String = ""
    Public TIPONEGOCIO_TTVC As String = ""
    Private logCambiarConsultaPortafolio As Boolean = True
    Private logCambiarValoresReceptorA As Boolean = True

    Private TIPOTASA_VARIABLE As String = "V"
    Private TIPOTASA_FIJA As String = "F"
    Private INDICADOR_TASA_FIJA As String = "0"

    'JDCP20180607
    Public logCalcularValores As Boolean = False

#End Region

    ''' <summary>
    ''' Método que se lanza cuando se inicializa el ViewModel
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext()
                dcProxy1 = New OYDPLUSOrdenesBolsaDomainContext()
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad02 = New UtilidadesDomainContext()
                mdcProxyUtilidad03 = New UtilidadesDomainContext()
                mdcProxyUtilidadPLUS = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad03 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidadPLUS = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxy1.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad01.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad02.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad03.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidadPLUS.DomainClient, WebDomainClient(Of OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            TIPONEGOCIO_ACCIONES = "A"
            TIPONEGOCIO_ADR = "ADR"
            TIPONEGOCIO_RENTAFIJA = "C"
            TIPONEGOCIO_REPO = "R"
            TIPONEGOCIO_SIMULTANEA = "S"
            TIPONEGOCIO_TTV = "TTV"
            TIPONEGOCIO_TTVC = "TTVC"
            TIPONEGOCIO_REPOC = "RC"
            MostrarGrid = False

            If Not Program.IsDesignMode() Then
                IsBusy = True
                ConsultarReceptoresUsuario("INICIO")
                dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorFiltrarQuery("", MLOG_TRASLADOENDINERO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "")
                dcProxy1.Load(dcProxy1.TraerRegistroOperacionesPorReceptorPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionPorReceptorPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PortafolioEntreComercialesPPViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    ''' <summary>
    ''' Recibe el resultado de las consultas de combos
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))

        Try
            'IsBusy = False
            If Not lo.HasError Then

                Select Case lo.UserState
                    Case "TIPO_PRODUCTO_PP"

                        If mdcProxyUtilidad01.ItemCombos.Count > 0 Then
                            listaTipoProducto = lo.Entities.ToList
                            If Editando = True And (From c In _listaTipoProducto Where c.intID = 0 Select c).Count > 0 Then
                                strTipoProducto = (From c In _listaTipoProducto Where c.intID = 0 Select c).FirstOrDefault.ID
                            Else
                                strTipoProducto = _OperacionesPorReceptorSelected.TipoProducto
                            End If
                        End If

                        If Editando Then
                            _strClase = String.Empty
                            _strTipoOperacion = String.Empty
                        End If

                        If listaTipoNegocio IsNot Nothing Then
                            listaTipoNegocio = Nothing
                        End If

                        If mdcProxyUtilidad02.ItemCombos IsNot Nothing Then
                            mdcProxyUtilidad02.ItemCombos.Clear()
                        End If

                        'IsBusy = True
                        mdcProxyUtilidad02.Load(mdcProxyUtilidad02.cargarCombosCondicionalQuery("TIPO_NEGOCIO_PP", strReceptorA, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "TIPO_NEGOCIO_PP")

                    Case "TIPO_NEGOCIO_PP"

                        listaTipoNegocio = lo.Entities.ToList
                        If Editando = True And (From c In _listaTipoNegocio Where c.intID = 0 Select c).Count > 0 Then
                            strClase = (From c In _listaTipoNegocio Where c.intID = 0 Select c).FirstOrDefault.ID
                        Else
                            strClase = _OperacionesPorReceptorSelected.Clase
                        End If

                    Case "TIPO_OPERACION_PP"

                        listaTipoOperacion = lo.Entities.ToList
                        If Editando = False Then
                            strTipoOperacion = _OperacionesPorReceptorSelected.TipoOperacion
                        End If

                    Case "TIPO_OPERACION_PP_CONSULTA"

                        listaTipoOperacionConsulta = lo.Entities.ToList
                End Select

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                     Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                       Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try
        'IsBusy = False
    End Sub

    Private Sub TerminoTraerOperacionPorReceptorPorDefecto_Completed(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor))
        If Not lo.HasError Then
            OperacionPorReceptorPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la operación por receptor por defecto", _
                                             Me.ToString(), "TerminoTraerOperacionPorReceptorPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerOperacionesPorReceptor(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor))
        If Not lo.HasError Then
            ListaOperacionesPorReceptor = dcProxy.RegistroOperacionesPorReceptors
            If dcProxy.RegistroOperacionesPorReceptors.Count > 0 Then
                If lo.UserState = "insert" Then
                    _OperacionesPorReceptorSelected = ListaOperacionesPorReceptor.First
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Operaciones por receptor", _
                                             Me.ToString(), "TerminoTraerOperacionesPorReceptor", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Recibe el resultado de la consulta de receptores segun el usuario logueado en el sistema
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara -  Marzo 28/2014</remarks>
    Private Sub TerminoConsultarTodosReceptoresUsuario(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblReceptoresUsuario))
        Try
            IsBusy = False
            If lo.HasError = False Then
                Dim strReceptorSalvar As String = String.Empty

                If lo.UserState = "EDITAR" Or lo.UserState = "BORRAR" Then
                    strReceptorSalvar = _strReceptorA
                    logCambiarValoresReceptorA = False
                End If

                ListaReceptoresUsuario = lo.Entities.ToList

                If lo.UserState = "EDITAR" Or lo.UserState = "BORRAR" Then
                    strReceptorA = strReceptorSalvar
                End If
                logCambiarValoresReceptorA = True

                If lo.UserState = "EDITAR" Then
                    If IsNothing(ListaReceptoresUsuario) Then
                        MyBase.RetornarValorEdicionNavegacion()
                        mostrarMensaje("No se puede modificar porque no tiene permisos sobre uno de los receptores de la operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf _ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = _OperacionesPorReceptorSelected.ReceptorA).Count = 0 And
                        _ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = _OperacionesPorReceptorSelected.ReceptorB).Count = 0 Then
                        MyBase.RetornarValorEdicionNavegacion()
                        mostrarMensaje("No se puede modificar porque no tiene permisos sobre uno de los receptores de la operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        If _ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = _OperacionesPorReceptorSelected.ReceptorA).Count > 0 Then
                            HabilitarEdicionReceptorA = True
                        Else
                            HabilitarEdicionReceptorA = False
                        End If

                        ObtenerRegistroAnterior()
                        Editando = True
                        EditandoEncabezado = False
                        HabilitarEdicionReceptorA = False
                        If _OperacionesPorReceptorSelected.Clase = TIPONEGOCIO_SIMULTANEA Then
                            EditandoFuturo = True
                        Else
                            EditandoFuturo = False
                        End If
                        MostrarGrid = True
                        MostrarControles = Visibility.Visible

                        consultarOrdenantesOYDPLUS(_OperacionesPorReceptorSelected.IdCliente, "EDITAR")
                        consultarCuentasDepositoOYDPLUS(_OperacionesPorReceptorSelected.IdCliente, "EDITAR")

                        If Not String.IsNullOrEmpty(_OperacionesPorReceptorSelected.IdEspecie) And _OperacionesPorReceptorSelected.IdEspecie <> "(No Seleccionado)" Then
                            HabilitarDeshabilitarControlesEspecies(False, _OperacionesPorReceptorSelected.EspecieEsAccion, _OperacionesPorReceptorSelected.TipoTasaFija, _OperacionesPorReceptorSelected.Indicador)
                        End If
                    End If
                ElseIf lo.UserState = "BORRAR" Then
                    If IsNothing(ListaReceptoresUsuario) Then
                        mostrarMensaje("No se puede inactivar porque no tiene permisos sobre uno de los receptores de la operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf _ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = _OperacionesPorReceptorSelected.ReceptorA).Count = 0 And
                        _ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = _OperacionesPorReceptorSelected.ReceptorB).Count = 0 Then
                        mostrarMensaje("No se puede inactivar porque no tiene permisos sobre uno de los receptores de la operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        A2Utilidades.Mensajes.mostrarMensajePregunta("Esta opción permite inactivar el registro seleccionado. ¿Confirma la inactivación de el registro?", Program.TituloSistema, "inactivar", AddressOf TerminoMensajePregunta)
                        consultarCuentasDepositoOYDPLUS(_OperacionesPorReceptorSelected.IdCliente, "EDITAR")
                    End If
                ElseIf lo.UserState = "NUEVO" Then
                    NuevaOperacion()
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub buscarComitentesComplete(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.HasError Then
                If Not lo.Error Is Nothing Then
                    Throw New Exception(lo.Error.Message, lo.Error.InnerException)
                Else
                    Throw New Exception("Se presentó un error al ejecutar la consulta de Comitentes pero no se recibió detalle del problema generado")
                End If
            Else
                If lo.Entities.Count = 1 Then
                    Me.ComitenteSeleccionadoOYDPLUS = lo.Entities.First
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la consulta de Comitentes", Me.ToString(), "buscarComitentesComplete", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub buscarEspeciesComplete(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If lo.HasError Then
                If Not lo.Error Is Nothing Then
                    Throw New Exception(lo.Error.Message, lo.Error.InnerException)
                Else
                    Throw New Exception("Se presentó un error al ejecutar la consulta de especies pero no se recibió detalle del problema generado")
                End If
            Else
                If _strClase = TIPONEGOCIO_ACCIONES Or _strClase = TIPONEGOCIO_ADR Or _strClase = TIPONEGOCIO_REPO Or _strClase = TIPONEGOCIO_TTV Then
                    Dim objEspecies As List(Of A2ComunesControl.EspeciesAgrupadas)
                    objEspecies = (From obj In (From obj In lo.Entities.ToList Select obj.Nemotecnico, obj.Especie, obj.Emisor, obj.Mercado).Distinct
                            Select New A2ComunesControl.EspeciesAgrupadas(obj.Nemotecnico, obj.Especie, obj.Emisor, obj.Mercado)).ToList
                    If objEspecies.Count = 1 Then
                        Me.NemotecnicoSeleccionadoOYDPLUS = lo.Entities.First
                    End If
                ElseIf _strClase = TIPONEGOCIO_RENTAFIJA Or _strClase = TIPONEGOCIO_SIMULTANEA Or _strClase = TIPONEGOCIO_REPOC Or _strClase = TIPONEGOCIO_TTVC Then
                    If lo.Entities.Count = 1 Then
                        Me.NemotecnicoSeleccionadoOYDPLUS = lo.Entities.First
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la consulta de especies", Me.ToString(), "buscarEspeciesComplete", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Propiedades"

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

    ''' <summary>
    ''' Propiedad que permite manejar la lista de operaciones por receptor de View Portafolio entre comerciales
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _ListaOperacionesPorReceptor As EntitySet(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor)
    Public Property ListaOperacionesPorReceptor() As EntitySet(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor)
        Get
            Return _ListaOperacionesPorReceptor
        End Get
        Set(ByVal value As EntitySet(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor))
            _ListaOperacionesPorReceptor = value

            MyBase.CambioItem("ListaOperacionesPorReceptor")
            MyBase.CambioItem("ListaOperacionesPorReceptorPaged")
            If Not IsNothing(_ListaOperacionesPorReceptor) Then
                If _ListaOperacionesPorReceptor.Count > 0 Then
                    _OperacionesPorReceptorSelected = _ListaOperacionesPorReceptor.FirstOrDefault
                Else
                    _OperacionesPorReceptorSelected = Nothing
                End If
            Else
                _OperacionesPorReceptorSelected = Nothing
            End If
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite manejar la paginación de lista de operaciones por receptor de View Portafolio entre comerciales
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public ReadOnly Property ListaOperacionesPorReceptorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOperacionesPorReceptor) Then
                Dim view = New PagedCollectionView(_ListaOperacionesPorReceptor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Propiedad que permite manejar el registro seleccionado en el view
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private WithEvents _OperacionesPorReceptorSelected As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor
    Public Property OperacionesPorReceptorSelected() As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor
        Get
            Return _OperacionesPorReceptorSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor)

            strReceptorA = String.Empty

            If Not IsNothing(_OperacionesPorReceptorSelected) AndAlso Not IsNothing(OperacionPorReceptorAnterior) Then
                _OperacionesPorReceptorSelected = OperacionPorReceptorAnterior
            End If

            _OperacionesPorReceptorSelected = value

            If Not IsNothing(_OperacionesPorReceptorSelected) Then
                OperacionPorReceptorAnterior = _OperacionesPorReceptorSelected
                strClase = String.Empty
                strTipoOperacion = String.Empty
                strTipoProducto = String.Empty
                strReceptorA = value.ReceptorA

                MostrarGrid = True
                OrdenanteSeleccionadoOYDPLUS = Nothing
                CtaDepositoSeleccionadaOYDPLUS = Nothing
                consultarOrdenantesOYDPLUS(_OperacionesPorReceptorSelected.IdCliente)
                consultarCuentasDepositoOYDPLUS(_OperacionesPorReceptorSelected.IdCliente)
            End If

            MyBase.CambioItem("OperacionesPorReceptorSelected")

        End Set
    End Property

    Private _strClase As String
    <Display(Name:="Tipo negocio")> _
    Public Property strClase() As String
        Get
            Return _strClase
        End Get
        Set(ByVal value As String)
            Dim logLimpiarDatos As Boolean = False
            If _strClase <> value Then
                logLimpiarDatos = True
            End If
            _strClase = value
            If Editando Then
                If logLimpiarDatos Then
                    LimpiarEspecie(_OperacionesPorReceptorSelected)
                End If

                _OperacionesPorReceptorSelected.Clase = _strClase
                If Not IsNothing(_OperacionesPorReceptorSelected.TipoProducto) And Not IsNothing(_OperacionesPorReceptorSelected.Clase) Then
                    MostrarGrid = True
                End If

                MostrarControles = Visibility.Visible
            End If

            If _strClase = TIPONEGOCIO_ACCIONES Or _strClase = TIPONEGOCIO_ADR Or _strClase = TIPONEGOCIO_REPO Or _strClase = TIPONEGOCIO_TTV Then
                HabilitarComboISIN = False
            ElseIf _strClase = TIPONEGOCIO_RENTAFIJA Or _strClase = TIPONEGOCIO_SIMULTANEA Or _strClase = TIPONEGOCIO_REPOC Or _strClase = TIPONEGOCIO_TTVC Then
                HabilitarComboISIN = True
            End If

            If logLimpiarDatos Then
                If mdcProxyUtilidad03.ItemCombos IsNot Nothing Then
                    mdcProxyUtilidad03.ItemCombos.Clear()
                End If

                'IsBusy = True
                mdcProxyUtilidad03.Load(mdcProxyUtilidad03.cargarCombosCondicionalQuery("TIPO_OPERACION_PP", strClase, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "TIPO_OPERACION_PP")
            End If

            MyBase.CambioItem("strClase")
        End Set
    End Property

    Private _strTipoOperacion As String
    <Display(Name:="Tipo operación")> _
    Public Property strTipoOperacion() As String
        Get
            Return _strTipoOperacion
        End Get
        Set(ByVal value As String)
            _strTipoOperacion = value
            If Editando Then
                _OperacionesPorReceptorSelected.TipoOperacion = _strTipoOperacion
            End If
            MyBase.CambioItem("strTipoOperacion")
        End Set
    End Property

    Private _strTipoProducto As String
    <Display(Name:="Tipo producto")> _
    Public Property strTipoProducto() As String
        Get
            Return _strTipoProducto
        End Get
        Set(ByVal value As String)
            Dim logLimpiarDatos As Boolean = False
            If _strTipoProducto <> value Then
                logLimpiarDatos = True
            End If
            _strTipoProducto = value
            If Editando Then
                If logLimpiarDatos Then
                    LimpiarEspecie(_OperacionesPorReceptorSelected)
                End If

                _OperacionesPorReceptorSelected.TipoProducto = _strTipoProducto
                If Not IsNothing(_OperacionesPorReceptorSelected.TipoProducto) And Not IsNothing(_OperacionesPorReceptorSelected.Clase) Then
                    MostrarGrid = True
                End If
                MostrarControles = Visibility.Visible
            End If
            MyBase.CambioItem("strTipoProducto")
        End Set
    End Property

    Private _strReceptorA As String
    <Display(Name:="Receptor A")> _
    Public Property strReceptorA() As String
        Get
            Return _strReceptorA
        End Get
        Set(ByVal value As String)
            If _strReceptorA <> value Then
                _strReceptorA = value

                If logCambiarValoresReceptorA Then
                    If Not String.IsNullOrWhiteSpace(_strReceptorA) Then

                        If Editando And HabilitarEdicionReceptorA Then
                            LimpiarCliente(_OperacionesPorReceptorSelected)
                            LimpiarEspecie(_OperacionesPorReceptorSelected)
                        End If

                        If Editando And HabilitarEdicionReceptorA Then
                            strTipoProducto = String.Empty
                        End If

                        If listaTipoProducto IsNot Nothing Then
                            listaTipoProducto = Nothing
                        End If

                        ConsultarComboespecifico("TIPO_PRODUCTO_PP")

                    End If
                End If

                MyBase.CambioItem("strReceptorA")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para el manejo de los campos de busqueda en el view
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private WithEvents _cb As CamposBusquedaOperacionesPorReceptor2
    Public Property cb() As CamposBusquedaOperacionesPorReceptor2
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaOperacionesPorReceptor2)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite manejar la lista de receptores por usuario
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
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

    ''' <summary>
    ''' Propiedad que permite manejar la lista de ordenantes por usuario
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _ListaOrdenantesOYDPLUS As List(Of OYDUtilidades.BuscadorOrdenantes)
    Public Property ListaOrdenantesOYDPLUS As List(Of OYDUtilidades.BuscadorOrdenantes)
        Get
            Return (_ListaOrdenantesOYDPLUS)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorOrdenantes))
            _ListaOrdenantesOYDPLUS = value
            MyBase.CambioItem("ListaOrdenantesOYDPLUS")

            If Not IsNothing(ListaOrdenantesOYDPLUS) Then
                If ListaOrdenantesOYDPLUS.Count = 1 Then
                    OrdenanteSeleccionadoOYDPLUS = ListaOrdenantesOYDPLUS.FirstOrDefault
                Else
                    If ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = _OperacionesPorReceptorSelected.IdOrdenante).Count > 0 Then
                        OrdenanteSeleccionadoOYDPLUS = ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = _OperacionesPorReceptorSelected.IdOrdenante).FirstOrDefault
                    End If
                End If

            Else
                OrdenanteSeleccionadoOYDPLUS = Nothing
            End If
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite el manejo de los ordenantes seleccionados en el view
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _mobjOrdenanteSeleccionadoOYDPLUS As OYDUtilidades.BuscadorOrdenantes
    Public Property OrdenanteSeleccionadoOYDPLUS() As OYDUtilidades.BuscadorOrdenantes
        Get
            Return (_mobjOrdenanteSeleccionadoOYDPLUS)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorOrdenantes)
            _mobjOrdenanteSeleccionadoOYDPLUS = value
            If Not IsNothing(OrdenanteSeleccionadoOYDPLUS) Then
                If Not IsNothing(_OperacionesPorReceptorSelected) Then
                    If Editando Then
                        _OperacionesPorReceptorSelected.IdOrdenante = _mobjOrdenanteSeleccionadoOYDPLUS.IdOrdenante
                    End If
                End If
            End If
            MyBase.CambioItem("OrdenanteSeleccionadoOYDPLUS")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite el manejo de las cuentas deposito seleccionadas en el view 
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _mobjCtaDepositoSeleccionadaOYDPLUS As OYDUtilidades.BuscadorCuentasDeposito
    Public Property CtaDepositoSeleccionadaOYDPLUS() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCtaDepositoSeleccionadaOYDPLUS)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorCuentasDeposito)
            _mobjCtaDepositoSeleccionadaOYDPLUS = value
            If Not IsNothing(CtaDepositoSeleccionadaOYDPLUS) Then
                If Not IsNothing(_OperacionesPorReceptorSelected) Then
                    If Editando Then
                        _OperacionesPorReceptorSelected.CuentaDeposito = CtaDepositoSeleccionadaOYDPLUS.NroCuentaDeposito
                        _OperacionesPorReceptorSelected.UbicacionTitulo = CtaDepositoSeleccionadaOYDPLUS.Deposito
                    End If
                End If
            Else
                If Editando Then
                    _OperacionesPorReceptorSelected.CuentaDeposito = 0
                    _OperacionesPorReceptorSelected.UbicacionTitulo = String.Empty
                End If
            End If
            MyBase.CambioItem("CtaDepositoSeleccionadaOYDPLUS")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite el manejo de la listas de las cuentas deposito en el view 
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _ListaCuentasDepositoOYDPLUS As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property ListaCuentasDepositoOYDPLUS As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return (_ListaCuentasDepositoOYDPLUS)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _ListaCuentasDepositoOYDPLUS = value
            MyBase.CambioItem("ListaCuentasDepositoOYDPLUS")
            If Not IsNothing(ListaCuentasDepositoOYDPLUS) Then
                If ListaCuentasDepositoOYDPLUS.Count = 1 Then
                    CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.FirstOrDefault
                Else
                    If ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = _OperacionesPorReceptorSelected.CuentaDeposito).Count > 0 Then
                        CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = _OperacionesPorReceptorSelected.CuentaDeposito).FirstOrDefault
                    End If
                End If

            Else
                CtaDepositoSeleccionadaOYDPLUS = Nothing
            End If
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite el manejo de eventos cuando se borra un cliente
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
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
    ''' Propiedad que permite el manejo de eventos cuando se realiza la busqueda de un cliente
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _ClienteBuscar As String
    Public Property ClienteBuscar() As String
        Get
            Return _ClienteBuscar
        End Get
        Set(ByVal value As String)
            _ClienteBuscar = value
            MyBase.CambioItem("ClienteBuscar")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite el manejo de un cliente seleccionado en el view
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _ComitenteSeleccionadoOYDPLUS As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionadoOYDPLUS As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionadoOYDPLUS)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _ComitenteSeleccionadoOYDPLUS = value
            Try
                If Not IsNothing(_OperacionesPorReceptorSelected) Then
                    SeleccionarClienteOYDPLUS(_ComitenteSeleccionadoOYDPLUS, _OperacionesPorReceptorSelected)

                    If Not IsNothing(ComitenteSeleccionadoOYDPLUS) Then
                        OrdenanteSeleccionadoOYDPLUS = Nothing
                        CtaDepositoSeleccionadaOYDPLUS = Nothing
                        consultarOrdenantesOYDPLUS(ComitenteSeleccionadoOYDPLUS.IdComitente)
                        consultarCuentasDepositoOYDPLUS(ComitenteSeleccionadoOYDPLUS.IdComitente)
                    End If
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad del cliente seleccionado.", Me.ToString, "ComitenteSeleccionadoOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try

            MyBase.CambioItem("ComitenteSeleccionadoOYDPLUS")

        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite el manejo de eventos cuando se borra una especie
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
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

    ''' <summary>
    ''' Propiedad que permite el manejo de eventos cuando se realiza la selección de un especie o nemotecnico
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _NemotecnicoSeleccionadoOYDPLUS As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionadoOYDPLUS As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionadoOYDPLUS)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionadoOYDPLUS = value
            Try
                If Not IsNothing(_OperacionesPorReceptorSelected) Then
                    SeleccionarEspecieOYDPLUS(NemotecnicoSeleccionadoOYDPLUS)
                End If

            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad de la especie seleccionada.", Me.ToString, "NemotecnicoSeleccionadoOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try
            MyBase.CambioItem("NemotecnicoSeleccionadoOYDPLUS")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite ocultar o mostrar el grid de ingreso de datos en el view
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _MostrarGrid As Boolean = False
    Public Property MostrarGrid() As Boolean
        Get
            Return _MostrarGrid
        End Get
        Set(ByVal value As Boolean)
            _MostrarGrid = value
            MyBase.CambioItem("MostrarGrid")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite el manejo de los controles asociados al cliente OYD en la parte del portafolio
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
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

    ''' <summary>
    ''' Propiedad que permite el manejo del tipo de negocio en la parte del portafolio
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _TipoNegocioControles As String
    Public Property TipoNegocioControles() As String
        Get
            Return _TipoNegocioControles
        End Get
        Set(ByVal value As String)
            _TipoNegocioControles = value
            MyBase.CambioItem("TipoNegocioControles")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite el manejo de los controles asociados a la especie en la parte del portafolio
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _EspecieControles As String
    Public Property EspecieControles() As String
        Get
            Return _EspecieControles
        End Get
        Set(ByVal value As String)
            _EspecieControles = value
            MyBase.CambioItem("EspecieControles")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que permite mostrar y habilitar los controles una vez seleccionado el portafolio 
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private _MostrarControles As Visibility = Visibility.Collapsed
    Public Property MostrarControles() As Visibility
        Get
            Return _MostrarControles
        End Get
        Set(ByVal value As Visibility)
            _MostrarControles = value
            MyBase.CambioItem("MostrarControles")
        End Set
    End Property

    Private _listaTipoProducto As List(Of OYDUtilidades.ItemCombo)
    Public Property listaTipoProducto() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listaTipoProducto
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaTipoProducto = value
            MyBase.CambioItem("listaTipoProducto")
        End Set
    End Property

    Private _listaTipoNegocio As List(Of OYDUtilidades.ItemCombo)
    Public Property listaTipoNegocio() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listaTipoNegocio
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaTipoNegocio = value
            MyBase.CambioItem("listaTipoNegocio")
        End Set
    End Property

    Private _listaTipoOperacion As List(Of OYDUtilidades.ItemCombo)
    Public Property listaTipoOperacion() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listaTipoOperacion
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaTipoOperacion = value
            MyBase.CambioItem("listaTipoOperacion")
        End Set
    End Property

    Private _listaTipoOperacionConsulta As List(Of OYDUtilidades.ItemCombo)
    Public Property listaTipoOperacionConsulta() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listaTipoOperacionConsulta
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaTipoOperacionConsulta = value
            MyBase.CambioItem("listaTipoOperacionConsulta")
        End Set
    End Property

    Private _MostrarCamposFaciales As Visibility = Visibility.Collapsed
    Public Property MostrarCamposFaciales() As Visibility
        Get
            Return _MostrarCamposFaciales
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposFaciales = value
            MyBase.CambioItem("MostrarCamposFaciales")
        End Set

    End Property

    Private _MostrarCampoTasaFacial As Visibility = Visibility.Collapsed
    Public Property MostrarCampoTasaFacial() As Visibility
        Get
            Return _MostrarCampoTasaFacial
        End Get
        Set(ByVal value As Visibility)
            _MostrarCampoTasaFacial = value
            MyBase.CambioItem("MostrarCampoTasaFacial")
        End Set
    End Property

    Private _MostrarCampoIndicador As Visibility = Visibility.Collapsed
    Public Property MostrarCampoIndicador() As Visibility
        Get
            Return _MostrarCampoIndicador
        End Get
        Set(ByVal value As Visibility)
            _MostrarCampoIndicador = value
            MyBase.CambioItem("MostrarCampoIndicador")
        End Set
    End Property

    Private _MostrarCampoPuntosIndicador As Visibility = Visibility.Collapsed
    Public Property MostrarCampoPuntosIndicador() As Visibility
        Get
            Return _MostrarCampoPuntosIndicador
        End Get
        Set(ByVal value As Visibility)
            _MostrarCampoPuntosIndicador = value
            MyBase.CambioItem("MostrarCampoPuntosIndicador")
        End Set
    End Property

    Private _EditandoEncabezado As Boolean
    Public Property EditandoEncabezado() As Boolean
        Get
            Return _EditandoEncabezado
        End Get
        Set(ByVal value As Boolean)
            _EditandoEncabezado = value
            MyBase.CambioItem("EditandoEncabezado")
        End Set
    End Property

    Private _HabilitarEdicionReceptorA As Boolean
    Public Property HabilitarEdicionReceptorA() As Boolean
        Get
            Return _HabilitarEdicionReceptorA
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionReceptorA = value
            MyBase.CambioItem("HabilitarEdicionReceptorA")
        End Set
    End Property


    Private _HabilitarComboISIN As Boolean = False
    Public Property HabilitarComboISIN() As Boolean
        Get
            Return _HabilitarComboISIN
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComboISIN = value
            MyBase.CambioItem("HabilitarComboISIN")
        End Set
    End Property

    Private _EditandoFuturo As Boolean = False
    Public Property EditandoFuturo() As Boolean
        Get
            Return _EditandoFuturo
        End Get
        Set(ByVal value As Boolean)
            _EditandoFuturo = value
            MyBase.CambioItem("EditandoFuturo")
        End Set
    End Property


#End Region

#Region "Métodos"

    ''' <summary>
    ''' Método para inicializar el ingreso de un nuevo registro
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Overrides Sub NuevoRegistro()
        Try
            IsBusy = True
            ConsultarReceptoresUsuario("NUEVO")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub NuevaOperacion()
        Try
            Dim NewOperacionesPorReceptor As New OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor

            NewOperacionesPorReceptor.IdOperacion = OperacionPorReceptorPorDefecto.IdOperacion
            NewOperacionesPorReceptor.ReceptorA = String.Empty
            NewOperacionesPorReceptor.NombreReceptorA = String.Empty
            NewOperacionesPorReceptor.ReceptorB = String.Empty
            NewOperacionesPorReceptor.NombreReceptorB = String.Empty
            NewOperacionesPorReceptor.Activo = True
            NewOperacionesPorReceptor.TrasladoEnDinero = MLOG_TRASLADOENDINERO
            NewOperacionesPorReceptor.Usuario = Program.Usuario
            NewOperacionesPorReceptor.FechaLiquidacion = Date.Now
            NewOperacionesPorReceptor.FechaCumplimiento = Date.Now
            NewOperacionesPorReceptor.FechaIngreso = Date.Now
            NewOperacionesPorReceptor.FechaRegreso = Date.Now
            NewOperacionesPorReceptor.OperacionRegreso = False

            strReceptorA = String.Empty
            strTipoProducto = String.Empty
            strClase = String.Empty
            strTipoOperacion = String.Empty

            listaTipoNegocio = Nothing
            listaTipoProducto = Nothing

            ObtenerRegistroAnterior()

            OperacionesPorReceptorSelected = NewOperacionesPorReceptor
            MyBase.CambioItem("OperacionesPorReceptorSelected")
            Editando = True
            EditandoEncabezado = True
            HabilitarEdicionReceptorA = True
            EditandoFuturo = False

            MyBase.CambioItem("Editando")

            If BorrarCliente Then
                BorrarCliente = False
            End If

            BorrarCliente = True

            If BorrarEspecie Then
                BorrarEspecie = False
            End If

            BorrarEspecie = True

            MostrarGrid = False

            MostrarControles = Visibility.Collapsed
            MostrarCampoTasaFacial = Visibility.Collapsed
            MostrarCampoIndicador = Visibility.Collapsed
            MostrarCampoPuntosIndicador = Visibility.Collapsed
            MostrarCamposFaciales = Visibility.Collapsed
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para filtrar los registros
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Overrides Sub Filtrar()
        Try
            If Not IsNothing(dcProxy.RegistroOperacionesPorReceptors) Then
                dcProxy.RegistroOperacionesPorReceptors.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorFiltrarQuery(TextoFiltroSeguro, MLOG_TRASLADOENDINERO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorFiltrarQuery("", MLOG_TRASLADOENDINERO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se hacen validaciones basicas del registro ingresado
    ''' </summary>
    ''' <returns>true si no hay validaciones que detengan el proceso</returns>
    ''' <remarks>Santiago Vergara - Junio 06/2014</remarks>
    Private Function RegistroValido() As Boolean

        If String.IsNullOrEmpty(strReceptorA) And HabilitarEdicionReceptorA Then
            mostrarMensaje("El receptor A es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If String.IsNullOrEmpty(strTipoProducto) Then
            mostrarMensaje("El tipo de producto es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If String.IsNullOrEmpty(strClase) Then
            mostrarMensaje("El tipo de negocio es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If String.IsNullOrEmpty(strTipoOperacion) Then
            mostrarMensaje("El tipo de operación es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If String.IsNullOrEmpty(_OperacionesPorReceptorSelected.ReceptorB) Then
            mostrarMensaje("El receptor B es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If strReceptorA = _OperacionesPorReceptorSelected.ReceptorB Then
            mostrarMensaje("El receptor B debe ser didferente al receptor A.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If _OperacionesPorReceptorSelected.FechaCumplimiento.Value.Date < _OperacionesPorReceptorSelected.FechaLiquidacion.Value.Date Then
            mostrarMensaje("La fecha de cumplimiento no puede ser menor a la fecha de liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If String.IsNullOrEmpty(_OperacionesPorReceptorSelected.IdCliente) Or _OperacionesPorReceptorSelected.IdCliente = "-9999999999" Then
            mostrarMensaje("El cliente es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If String.IsNullOrEmpty(_OperacionesPorReceptorSelected.IdEspecie) Then
            mostrarMensaje("la especie es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If strClase = TIPONEGOCIO_RENTAFIJA Or strClase = TIPONEGOCIO_SIMULTANEA Then
            If IsNothing(_OperacionesPorReceptorSelected.Emision) Then
                mostrarMensaje("La fecha de emisión es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If
            If IsNothing(_OperacionesPorReceptorSelected.Vencimiento) Then
                mostrarMensaje("La fecha de vencimiento es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If
            If String.IsNullOrEmpty(_OperacionesPorReceptorSelected.Modalidad) Then
                mostrarMensaje("La Modalidad es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If
            If _OperacionesPorReceptorSelected.TipoTasaFija = TIPOTASA_VARIABLE Then
                If String.IsNullOrEmpty(_OperacionesPorReceptorSelected.Indicador) Then
                    mostrarMensaje("El indicador es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                    Exit Function
                End If

                If IsNothing(_OperacionesPorReceptorSelected.PuntosIndicador) Or _OperacionesPorReceptorSelected.PuntosIndicador = 0 Then
                    mostrarMensaje("Los puntos del indicador es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                    Exit Function
                Else
                    If _OperacionesPorReceptorSelected.PuntosIndicador > 99 Or _OperacionesPorReceptorSelected.PuntosIndicador < -99 Then
                        mostrarMensaje("Los puntos del indicador es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return False
                        Exit Function
                    End If
                End If
            ElseIf _OperacionesPorReceptorSelected.TipoTasaFija = TIPOTASA_FIJA Then
                If (IsNothing(_OperacionesPorReceptorSelected.TasaFacial) Or _OperacionesPorReceptorSelected.TasaFacial = 0) And _OperacionesPorReceptorSelected.Modalidad.ToUpper <> "NO" Then
                    mostrarMensaje("La tasa facial es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                    Exit Function
                End If
            ElseIf String.IsNullOrEmpty(_OperacionesPorReceptorSelected.TipoTasaFija) Then
                If String.IsNullOrEmpty(_OperacionesPorReceptorSelected.Indicador) Then
                    mostrarMensaje("El indicador es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                    Exit Function
                Else
                    If _OperacionesPorReceptorSelected.Indicador = INDICADOR_TASA_FIJA Then
                        If (IsNothing(_OperacionesPorReceptorSelected.TasaFacial) Or _OperacionesPorReceptorSelected.TasaFacial = 0) And _OperacionesPorReceptorSelected.Modalidad.ToUpper <> "NO" Then
                            mostrarMensaje("La tasa facial es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return False
                            Exit Function
                        End If
                    Else
                        If IsNothing(_OperacionesPorReceptorSelected.PuntosIndicador) Or _OperacionesPorReceptorSelected.PuntosIndicador = 0 Then
                            mostrarMensaje("Los puntos del indicador es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return False
                            Exit Function
                        Else
                            If _OperacionesPorReceptorSelected.PuntosIndicador > 99 Or _OperacionesPorReceptorSelected.PuntosIndicador < -99 Then
                                mostrarMensaje("Los puntos del indicador es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Return False
                                Exit Function
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If IsNothing(_OperacionesPorReceptorSelected.ValorNominal) Or _OperacionesPorReceptorSelected.ValorNominal = 0 Then
            mostrarMensaje("El valor nominal es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If IsNothing(_OperacionesPorReceptorSelected.ValorGiro) Or _OperacionesPorReceptorSelected.ValorGiro = 0 Then
            mostrarMensaje("El valor giro es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If IsNothing(_OperacionesPorReceptorSelected.PrecioSucio) Or _OperacionesPorReceptorSelected.PrecioSucio = 0 Then
            mostrarMensaje("El precio es requerido, este es calculado al ingresar el Valor Giro y el Valor Nominal.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If strClase = TIPONEGOCIO_SIMULTANEA Then
            If IsNothing(_OperacionesPorReceptorSelected.ValorGiro2) Or _OperacionesPorReceptorSelected.ValorGiro2 = 0 Then
                mostrarMensaje("El valor regreso es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If
        End If



        If strClase = TIPONEGOCIO_SIMULTANEA Then
            If _OperacionesPorReceptorSelected.FechaRegreso.Value.Date < _OperacionesPorReceptorSelected.FechaLiquidacion.Value.Date Then
                mostrarMensaje("La fecha de regreso no puede ser menor a la fecha de liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If
        End If

        Return True
    End Function

    ''' <summary>
    ''' Método para inicializar el actualizar un registro
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Overrides Sub ActualizarRegistro()
        Try

            If RegistroValido() Then

                Dim origen = "update"
                ErrorForma = ""
                _OperacionesPorReceptorSelected.Usuario = Program.Usuario
                _OperacionesPorReceptorSelected.Clase = strClase
                _OperacionesPorReceptorSelected.TipoOperacion = strTipoOperacion
                _OperacionesPorReceptorSelected.TipoProducto = strTipoProducto
                If HabilitarEdicionReceptorA Then
                    _OperacionesPorReceptorSelected.ReceptorA = strReceptorA
                End If
                _OperacionesPorReceptorSelected.Activo = True
                _OperacionesPorReceptorSelected.TrasladoEnDinero = MLOG_TRASLADOENDINERO

                ObtenerRegistroAnterior()

                If (From li In ListaOperacionesPorReceptor Where li.IdOperacion = _OperacionesPorReceptorSelected.IdOperacion Select li).Count = 0 Then
                    origen = "insert"
                    ListaOperacionesPorReceptor.Add(_OperacionesPorReceptorSelected)
                End If

                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para guardar los cambios realizados en los registros
    ''' </summary>
    ''' <param name="So"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                Dim strMsg As String = String.Empty

                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                If BorrarEspecie = True Then
                    BorrarEspecie = False
                End If

                If BorrarCliente = True Then
                    BorrarCliente = False
                End If

                BorrarCliente = True
                BorrarEspecie = True

                MostrarControles = Visibility.Collapsed

                So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState = "insert" Or So.UserState = "update" Or So.UserState = "BorrarRegistro" Then
                    EditandoEncabezado = False
                    HabilitarEdicionReceptorA = False
                    EditandoFuturo = False
                    If So.UserState <> "BorrarRegistro" Then
                        mostrarMensaje("El registro se actualizó correctamente ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    Else
                        mostrarMensaje("El registro se inactivó correctamente ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    End If

                    If Not IsNothing(dcProxy.RegistroOperacionesPorReceptors) Then
                        dcProxy.RegistroOperacionesPorReceptors.Clear()
                    End If
                    IsBusy = True
                    dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorFiltrarQuery("", MLOG_TRASLADOENDINERO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "FiltroGuardar")
                End If
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para controlar eventos al editar los registros
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_OperacionesPorReceptorSelected) Then

            If _OperacionesPorReceptorSelected.FechaCumplimiento.Value.Date < Date.Today Then
                MyBase.RetornarValorEdicionNavegacion()
                mostrarMensaje("No se puede modificar una operación cumplida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            ElseIf _OperacionesPorReceptorSelected.OperacionRegreso Then
                MyBase.RetornarValorEdicionNavegacion()
                mostrarMensaje("No se puede modificar una operación de regreso por favor modificar la operación de salida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                IsBusy = True
                ConsultarReceptoresUsuario("EDITAR")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Método para controlar eventos cuando se cancela un registro
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_OperacionesPorReceptorSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditandoEncabezado = False
                HabilitarEdicionReceptorA = False
                EditandoFuturo = False
                OperacionesPorReceptorSelected = OperacionPorReceptorAnterior

                If BorrarEspecie = True Then
                    BorrarEspecie = False
                End If

                If BorrarCliente = True Then
                    BorrarCliente = False
                End If

                BorrarCliente = True
                BorrarEspecie = True

                MostrarCamposFaciales = Visibility.Collapsed

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Método para controlar eventos cuando se borra un registro
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_OperacionesPorReceptorSelected) Then
                If _OperacionesPorReceptorSelected.OperacionRegreso Then
                    mostrarMensaje("No se puede inactivar una operación de regreso por favor inactivar la operación de salida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    IsBusy = True
                    ConsultarReceptoresUsuario("BORRAR")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Select Case CType(sender, A2Utilidades.wppMensajePregunta).CodigoLlamado.ToLower
                    Case "inactivar"
                        dcProxy.RegistroOperacionesPorReceptors.Remove(_OperacionesPorReceptorSelected)
                        OperacionesPorReceptorSelected = _ListaOperacionesPorReceptor.LastOrDefault
                        IsBusy = True
                        Program.VerificarCambiosProxyServidor(dcProxy)
                        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                End Select
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inactivar un registro", Me.ToString(), "TerminoMensajePregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para inicializar busqueda de registros
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Método para confirmar busqueda de registros
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Overrides Sub ConfirmarBuscar()
        Try
            ErrorForma = ""
            If Not IsNothing(dcProxy.RegistroOperacionesPorReceptors) Then
                dcProxy.RegistroOperacionesPorReceptors.Clear()
            End If
            OperacionPorReceptorAnterior = Nothing
            IsBusy = True
            dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorConsultarQuery(cb.IdOperacion, cb.Clase, cb.TipoOperacion, cb.ReceptorA, cb.ReceptorB, cb.Especie, MLOG_TRASLADOENDINERO, Program.Usuario, cb.FechaLiquidacion, cb.FechaCumplimiento, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "Busqueda")
            MyBase.ConfirmarBuscar()
            PrepararNuevaBusqueda()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para inicializar una nueva busqueda de registros 
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaOperacionesPorReceptor2
            objCB.IdOperacion = Nothing
            objCB.Clase = String.Empty
            objCB.TipoOperacion = String.Empty
            objCB.ReceptorA = String.Empty
            objCB.ReceptorB = String.Empty
            objCB.Especie = String.Empty
            objCB.FechaLiquidacion = Nothing
            objCB.FechaCumplimiento = Nothing

            listaTipoOperacionConsulta = Nothing

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la nueva busqueda", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para gestionar eventos del registro anterior
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Sub ObtenerRegistroAnterior()
        Try
            If Not IsNothing(_OperacionesPorReceptorSelected) Then
                OperacionPorReceptorAnterior = _OperacionesPorReceptorSelected
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos del registro anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para el manejo de seleccion de la especie
    ''' </summary>
    ''' <param name="pobjEspecie"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Sub SeleccionarEspecieOYDPLUS(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            HabilitarControlesEspecieOYDPLUS(pobjEspecie)

            If Not IsNothing(pobjEspecie) Then


                _OperacionesPorReceptorSelected.IdEspecie = pobjEspecie.Nemotecnico

                If pobjEspecie.EsAccion Then
                    _OperacionesPorReceptorSelected.Indicador = String.Empty
                    _OperacionesPorReceptorSelected.PuntosIndicador = 0
                    _OperacionesPorReceptorSelected.Isin = String.Empty
                    _OperacionesPorReceptorSelected.Emision = Nothing
                    _OperacionesPorReceptorSelected.Vencimiento = Nothing
                    _OperacionesPorReceptorSelected.TasaFacial = Nothing
                    _OperacionesPorReceptorSelected.Modalidad = String.Empty
                Else
                    _OperacionesPorReceptorSelected.Isin = pobjEspecie.ISIN
                    _OperacionesPorReceptorSelected.Emision = pobjEspecie.Emision
                    _OperacionesPorReceptorSelected.Vencimiento = pobjEspecie.Vencimiento
                    _OperacionesPorReceptorSelected.Modalidad = pobjEspecie.CodModalidad
                    _OperacionesPorReceptorSelected.Indicador = String.Empty
                    _OperacionesPorReceptorSelected.PuntosIndicador = 0
                    _OperacionesPorReceptorSelected.TasaFacial = 0

                    If pobjEspecie.CodTipoTasaFija = TIPOTASA_FIJA Then
                        If Not IsNothing(pobjEspecie.TasaFacial) Then
                            _OperacionesPorReceptorSelected.TasaFacial = pobjEspecie.TasaFacial
                        End If
                    ElseIf pobjEspecie.CodTipoTasaFija = TIPOTASA_VARIABLE Then
                        If Not IsNothing(pobjEspecie.IdIndicador) Then
                            _OperacionesPorReceptorSelected.Indicador = pobjEspecie.IdIndicador.ToString
                        End If

                        If Not IsNothing(pobjEspecie.PuntosIndicador) Then
                            If pobjEspecie.PuntosIndicador > 99 Or pobjEspecie.PuntosIndicador < -99 Then
                                mostrarMensaje("Los puntos del indicador estan en un rango no permitido (-99->99), por favor corrija los valores", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else
                                If Not IsNothing(pobjEspecie.PuntosIndicador) Then
                                    _OperacionesPorReceptorSelected.PuntosIndicador = pobjEspecie.PuntosIndicador
                                End If
                            End If
                        End If
                    ElseIf pobjEspecie.IdIndicador = INDICADOR_TASA_FIJA Then
                        If Not IsNothing(pobjEspecie.IdIndicador) Then
                            _OperacionesPorReceptorSelected.Indicador = pobjEspecie.IdIndicador.ToString
                        End If

                        If Not IsNothing(pobjEspecie.TasaFacial) Then
                            _OperacionesPorReceptorSelected.TasaFacial = pobjEspecie.TasaFacial
                        End If
                    End If
                End If

                BorrarEspecie = False

            Else
                LimpiarEspecie(_OperacionesPorReceptorSelected)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos de la especie.", Me.ToString, "SeleccionarEspecie", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub HabilitarControlesEspecieOYDPLUS(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                HabilitarDeshabilitarControlesEspecies(False, pobjEspecie.EsAccion, pobjEspecie.CodTipoTasaFija, pobjEspecie.IdIndicador)
            Else
                HabilitarDeshabilitarControlesEspecies(True)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos de la especie.", Me.ToString, "HabilitarControlesEspecieOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub HabilitarControlesEspecieOYDPLUS(ByVal pobjEspecie As EspeciesAgrupadas)
        Try
            If Not IsNothing(pobjEspecie) Then
                HabilitarDeshabilitarControlesEspecies(False, pobjEspecie.EsAccion, pobjEspecie.CodTipoTasaFija, pobjEspecie.IdIndicador)
            Else
                HabilitarDeshabilitarControlesEspecies(True)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos de la especie.", Me.ToString, "HabilitarControlesEspecieOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub HabilitarDeshabilitarControlesEspecies(ByVal plogDeshabilitarControles As Boolean, Optional ByVal plogEsAccion As Boolean = False, Optional ByVal pstrTipoTasaFija As String = "", Optional ByVal pstrIndicador As String = "")
        Try
            If plogDeshabilitarControles = False Then
                If Editando Then
                    If plogEsAccion Then
                        MostrarCamposFaciales = Visibility.Collapsed
                        MostrarCampoTasaFacial = Visibility.Collapsed
                        MostrarCampoIndicador = Visibility.Collapsed
                        MostrarCampoPuntosIndicador = Visibility.Collapsed
                    Else
                        MostrarCamposFaciales = Visibility.Visible
                        If pstrTipoTasaFija = TIPOTASA_FIJA Then
                            MostrarCampoTasaFacial = Visibility.Visible
                            MostrarCampoIndicador = Visibility.Collapsed
                            MostrarCampoPuntosIndicador = Visibility.Collapsed
                        ElseIf pstrTipoTasaFija = TIPOTASA_VARIABLE Then
                            MostrarCampoIndicador = Visibility.Visible

                            If String.IsNullOrEmpty(pstrIndicador) Then
                                MostrarCampoTasaFacial = Visibility.Collapsed
                                MostrarCampoPuntosIndicador = Visibility.Collapsed
                            Else
                                If pstrIndicador = INDICADOR_TASA_FIJA Then
                                    MostrarCampoTasaFacial = Visibility.Visible
                                    MostrarCampoPuntosIndicador = Visibility.Collapsed
                                Else
                                    MostrarCampoTasaFacial = Visibility.Collapsed
                                    MostrarCampoPuntosIndicador = Visibility.Visible
                                End If
                            End If

                        ElseIf String.IsNullOrEmpty(pstrTipoTasaFija) Then
                            If String.IsNullOrEmpty(pstrIndicador) Then
                                MostrarCampoTasaFacial = Visibility.Collapsed
                                MostrarCampoIndicador = Visibility.Visible
                                MostrarCampoPuntosIndicador = Visibility.Collapsed
                            Else
                                MostrarCampoIndicador = Visibility.Visible
                                If pstrIndicador = INDICADOR_TASA_FIJA Then
                                    MostrarCampoTasaFacial = Visibility.Visible
                                    MostrarCampoPuntosIndicador = Visibility.Collapsed
                                Else
                                    MostrarCampoTasaFacial = Visibility.Collapsed
                                    MostrarCampoPuntosIndicador = Visibility.Visible
                                End If
                            End If
                        End If
                    End If
                Else
                    MostrarCamposFaciales = Visibility.Collapsed
                    MostrarCampoTasaFacial = Visibility.Collapsed
                    MostrarCampoIndicador = Visibility.Collapsed
                    MostrarCampoPuntosIndicador = Visibility.Collapsed
                End If
            Else
                MostrarCamposFaciales = Visibility.Collapsed
                MostrarCampoTasaFacial = Visibility.Collapsed
                MostrarCampoIndicador = Visibility.Collapsed
                MostrarCampoPuntosIndicador = Visibility.Collapsed
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos de la especie.", Me.ToString, "HabilitarDeshabilitarControlesEspecies", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Public Sub LimpiarEspecie(ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor)
        pobjOrdenSelected.IdEspecie = ""
        pobjOrdenSelected.Isin = ""
        pobjOrdenSelected.Emision = Nothing
        pobjOrdenSelected.Vencimiento = Nothing
        pobjOrdenSelected.TasaFacial = 0
        pobjOrdenSelected.Modalidad = String.Empty
        pobjOrdenSelected.Indicador = String.Empty
        pobjOrdenSelected.PuntosIndicador = 0

        MostrarCampoTasaFacial = Visibility.Collapsed
        MostrarCampoIndicador = Visibility.Collapsed
        MostrarCampoPuntosIndicador = Visibility.Collapsed

        If BorrarEspecie Then
            BorrarEspecie = False
        End If
        BorrarEspecie = True
    End Sub

    ''' <summary>
    ''' Método para el manejo de seleccion del cliente OYD
    ''' </summary>
    ''' <param name="pobjCliente"></param>
    ''' <param name="pobjOrdenSelected"></param>
    ''' <param name="pstrOpcion"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Sub SeleccionarClienteOYDPLUS(ByVal pobjCliente As OYDUtilidades.BuscadorClientes, ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor, Optional ByVal pstrOpcion As String = "")
        Try
            If Not IsNothing(pobjCliente) Then
                If Not IsNothing(pobjOrdenSelected) Then
                    _OperacionesPorReceptorSelected.IdCliente = pobjCliente.IdComitente
                    pobjOrdenSelected.IdCliente = pobjCliente.IdComitente
                    pobjOrdenSelected.NombreCliente = pobjCliente.Nombre
                    pobjOrdenSelected.NroDocumento = pobjCliente.NroDocumento
                    pobjOrdenSelected.CategoriaCliente = pobjCliente.Categoria
                    BorrarCliente = False
                End If

            Else
                If Not IsNothing(pobjOrdenSelected) Then
                    LimpiarCliente(pobjOrdenSelected)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarClienteOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Public Sub LimpiarCliente(ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor)
        pobjOrdenSelected.IdCliente = "-9999999999"
        pobjOrdenSelected.NombreCliente = "(No Seleccionado)"
        pobjOrdenSelected.NroDocumento = 0
        pobjOrdenSelected.CategoriaCliente = "(No Seleccionado)"
        pobjOrdenSelected.CuentaDeposito = 0
        pobjOrdenSelected.IdOrdenante = String.Empty

        OrdenanteSeleccionadoOYDPLUS = Nothing
        CtaDepositoSeleccionadaOYDPLUS = Nothing

        ListaOrdenantesOYDPLUS = Nothing
        ListaCuentasDepositoOYDPLUS = Nothing

        If BorrarCliente Then
            BorrarCliente = False
        End If

        BorrarCliente = True
    End Sub

    ''' <summary>
    ''' Método para la consulta de ordenantes
    ''' </summary>
    ''' <param name="pstrIdComitente"></param>
    ''' <param name="pstrUserState"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub consultarOrdenantesOYDPLUS(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try

            If Not IsNothing(mdcProxyUtilidad01.BuscadorOrdenantes) Then
                mdcProxyUtilidad01.BuscadorOrdenantes.Clear()
            End If

            Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)

            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarOrdenantesComitenteQuery(strClienteABuscar, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOrdenantes, pstrUserState)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar los ordenantes del cliente.", Me.ToString, "consultarOrdenantesOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    ''' <summary>
    ''' Método que indica cuando finalizo la consulta de ordenantes
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub TerminoTraerOrdenantes(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))

        Try
            If Not lo.HasError Then
                Dim objOrdenante As String = String.Empty
                If lo.UserState = "EDITAR" Then
                    objOrdenante = OperacionesPorReceptorSelected.IdOrdenante
                End If

                ListaOrdenantesOYDPLUS = lo.Entities.ToList

                If _ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = objOrdenante).Count > 0 Then
                    OrdenanteSeleccionadoOYDPLUS = _ListaOrdenantesOYDPLUS.Where(Function(i) i.IdOrdenante = objOrdenante).FirstOrDefault
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes", _
                                                 Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, lo.Error)
                'lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ordenantes", _
                                                 Me.ToString(), "TerminoTraerOrdenantes", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Método para la consulta de cuentas deposito
    ''' </summary>
    ''' <param name="pstrIdComitente"></param>
    ''' <param name="pstrUserState"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub consultarCuentasDepositoOYDPLUS(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try

            If Not IsNothing(mdcProxyUtilidad02.BuscadorCuentasDepositos) Then
                mdcProxyUtilidad02.BuscadorCuentasDepositos.Clear()
            End If

            If Not IsNothing(pstrIdComitente) Then
                Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, MINT_LONG_MAX_CODIGO_OYD)
                mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarCuentasDepositoComitenteQuery(strClienteABuscar, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDeposito, pstrUserState)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar las cuentas deposito del cliente.", Me.ToString, "consultarCuentasDepositoOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Método que indica cuando finalizo la consulta de cuentas deposito
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub TerminoTraerCuentasDeposito(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        Try
            If Not lo.HasError Then
                Dim objNroCuenta As Integer = 0

                If lo.UserState = "EDITAR" Then
                    objNroCuenta = _OperacionesPorReceptorSelected.CuentaDeposito
                End If

                If _OperacionesPorReceptorSelected.Clase = TIPONEGOCIO_ACCIONES Or _OperacionesPorReceptorSelected.Clase = TIPONEGOCIO_REPO Or _OperacionesPorReceptorSelected.Clase = TIPONEGOCIO_TTV Then
                    If lo.Entities.ToList.Where(Function(I) I.Deposito = "D").Count > 0 Then
                        Dim objListaCuentasDepositoOYDPLUS = New List(Of OYDUtilidades.BuscadorCuentasDeposito)
                        For Each li In lo.Entities.ToList
                            If li.Deposito = "D" Then
                                objListaCuentasDepositoOYDPLUS.Add(li)
                            End If
                        Next

                        ListaCuentasDepositoOYDPLUS = objListaCuentasDepositoOYDPLUS

                        If ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = objNroCuenta).Count > 0 Then
                            CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = objNroCuenta).FirstOrDefault
                        End If
                    Else
                        ListaCuentasDepositoOYDPLUS = Nothing
                    End If
                Else
                    ListaCuentasDepositoOYDPLUS = lo.Entities.ToList
                    If ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = objNroCuenta).Count > 0 Then
                        CtaDepositoSeleccionadaOYDPLUS = ListaCuentasDepositoOYDPLUS.Where(Function(i) i.NroCuentaDeposito = objNroCuenta).FirstOrDefault
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                                 Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Método que permite notificar cuando cambio la lista de operaciones por receptor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub _OperacionesPorReceptorSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _OperacionesPorReceptorSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "idcliente"
                    If Not IsNothing(_OperacionesPorReceptorSelected.TipoOperacion) And
                       Not IsNothing(_OperacionesPorReceptorSelected.IdCliente) Then
                        HabilitarConsultaControles(_OperacionesPorReceptorSelected)
                    End If
                Case "tipoproducto"
                    If Editando Then
                        ConsultarClientesReceptorA(strReceptorA, _OperacionesPorReceptorSelected.TipoProducto, _OperacionesPorReceptorSelected.Clase)
                        ConsultarEspeciesAutorizadasReceptorA(strReceptorA, _OperacionesPorReceptorSelected.TipoProducto, _OperacionesPorReceptorSelected.Clase)
                    End If
                Case "clase"
                    If Editando Then
                        ConsultarClientesReceptorA(strReceptorA, _OperacionesPorReceptorSelected.TipoProducto, _OperacionesPorReceptorSelected.Clase)
                        ConsultarEspeciesAutorizadasReceptorA(strReceptorA, _OperacionesPorReceptorSelected.TipoProducto, _OperacionesPorReceptorSelected.Clase)
                        If _OperacionesPorReceptorSelected.Clase = TIPONEGOCIO_SIMULTANEA Then
                            EditandoFuturo = True
                        Else
                            EditandoFuturo = False
                        End If
                    End If
                Case "fecharegreso"
                    If Editando Then
                        If _strClase = TIPONEGOCIO_SIMULTANEA Then
                            If Not IsNothing(_OperacionesPorReceptorSelected.FechaRegreso) Then
                                _OperacionesPorReceptorSelected.Plazo = DateDiff(DateInterval.Day, _OperacionesPorReceptorSelected.FechaLiquidacion.Value.Date, _OperacionesPorReceptorSelected.FechaRegreso.Value.Date)
                            End If
                        End If
                    End If
                Case "fechaliquidacion"
                    If Editando Then
                        If _strClase = TIPONEGOCIO_SIMULTANEA Then
                            If Not IsNothing(_OperacionesPorReceptorSelected.FechaRegreso) Then
                                _OperacionesPorReceptorSelected.Plazo = DateDiff(DateInterval.Day, _OperacionesPorReceptorSelected.FechaLiquidacion.Value.Date, _OperacionesPorReceptorSelected.FechaRegreso.Value.Date)
                            End If
                        End If
                    End If
                Case "indicador"
                    If Editando Then
                        If Not String.IsNullOrEmpty(_OperacionesPorReceptorSelected.IdEspecie) And _OperacionesPorReceptorSelected.IdEspecie <> "(No Seleccionado)" Then
                            HabilitarDeshabilitarControlesEspecies(False, _OperacionesPorReceptorSelected.EspecieEsAccion, _OperacionesPorReceptorSelected.TipoTasaFija, _OperacionesPorReceptorSelected.Indicador)

                            If _OperacionesPorReceptorSelected.Indicador = INDICADOR_TASA_FIJA Then
                                If Not IsNothing(_OperacionesPorReceptorSelected.PuntosIndicador) And _OperacionesPorReceptorSelected.PuntosIndicador <> 0 Then
                                    _OperacionesPorReceptorSelected.PuntosIndicador = 0
                                End If
                            Else
                                If Not String.IsNullOrEmpty(_OperacionesPorReceptorSelected.Indicador) Then
                                    If Not IsNothing(_OperacionesPorReceptorSelected.TasaFacial) And _OperacionesPorReceptorSelected.TasaFacial <> 0 Then
                                        _OperacionesPorReceptorSelected.TasaFacial = 0
                                    End If
                                End If
                            End If

                        End If
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la propiedad.", Me.ToString(), "_OperacionesPorReceptorSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub _cb_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _cb.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "clase"
                    If Not IsNothing(_cb) Then
                        If mdcProxyUtilidad03.ItemCombos IsNot Nothing Then
                            mdcProxyUtilidad03.ItemCombos.Clear()
                        End If

                        'IsBusy = True
                        mdcProxyUtilidad03.Load(mdcProxyUtilidad03.cargarCombosCondicionalQuery("TIPO_OPERACION_PP", cb.Clase, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "TIPO_OPERACION_PP_CONSULTA")
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la propiedad.", Me.ToString(), "_cb_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
        
    End Sub

    ''' <summary>
    ''' Método que permite habiliar o deshabilitar los controles para el tema de seleccion del registro desde el portafolio
    ''' </summary>
    ''' <param name="pobjOrdenSelected"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Sub HabilitarConsultaControles(ByVal pobjOrdenSelected As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor)
        Try
            If Not IsNothing(pobjOrdenSelected) Then

                TipoNegocioControles = pobjOrdenSelected.Clase

                CodigoOYDControles = pobjOrdenSelected.IdCliente

                EspecieControles = pobjOrdenSelected.IdEspecie
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al habilitar la busqueda en los controles dependiendo del tipo de orden.", Me.ToString, "HabilitarConsultaControles", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    ''' <summary>
    ''' Consulta los tipos de producto de posición propia asociados a un receptor
    ''' </summary>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Public Sub ConsultarComboespecifico(ByVal strTopico As String)
        Try
            If Not IsNothing(mdcProxyUtilidad01.ItemCombos) Then
                mdcProxyUtilidad01.ItemCombos.Clear()
            End If
            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.cargarCombosCondicionalQuery(strTopico, strReceptorA, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, strTopico)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llamar la consulta de tipos de producto por receptor", Me.ToString(), "ConsultarTiposProductoPorReceptor", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Consulta los clientes del receptor y el tipo de producto seleccionado sí solo contiene un cliente
    ''' se selecciona por defecto
    ''' </summary>
    ''' <remarks>Juan David Correa -  Febrero 16/2015</remarks>
    Public Sub ConsultarClientesReceptorA(ByVal pstrCodigoReceptor As String, ByVal pstrTipoProducto As String, ByVal pstrTipoNegocio As String)
        Try
            If Not String.IsNullOrEmpty(pstrCodigoReceptor) And Not String.IsNullOrEmpty(pstrTipoProducto) And Not String.IsNullOrEmpty(pstrTipoNegocio) Then
                If Not IsNothing(mdcProxyUtilidad01.BuscadorClientes) Then
                    mdcProxyUtilidad01.BuscadorClientes.Clear()
                End If
                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClientesOYDPLUSQuery(String.Empty,
                                                                                      A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.A.ToString,
                                                                                      A2ComunesControl.BuscadorClienteViewModel.TiposVinculacion.C.ToString,
                                                                                      String.Empty,
                                                                                      True,
                                                                                      False,
                                                                                      pstrCodigoReceptor,
                                                                                      pstrTipoNegocio,
                                                                                      pstrTipoProducto,
                                                                                      String.Empty,
                                                                                      Program.Usuario,
                                                                                      False,
                                                                                      Nothing, Program.HashConexion), AddressOf buscarComitentesComplete, "")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los clientes del receptor.", Me.ToString(), "ConsultarClientesReceptorA", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Consulta las especies autorizadas para el receptor y el tipo de producto seleccionado sí solo contiene una especie
    ''' se selecciona por defecto
    ''' </summary>
    ''' <remarks>Juan David Correa -  Febrero 16/2015</remarks>
    Public Sub ConsultarEspeciesAutorizadasReceptorA(ByVal pstrCodigoReceptor As String, ByVal pstrTipoProducto As String, ByVal pstrTipoNegocio As String)
        Try
            If Not String.IsNullOrEmpty(pstrCodigoReceptor) And Not String.IsNullOrEmpty(pstrTipoProducto) And Not String.IsNullOrEmpty(pstrTipoNegocio) Then
                Dim objClase As String = String.Empty

                If _strClase = TIPONEGOCIO_ACCIONES Or _strClase = TIPONEGOCIO_ADR Or _strClase = TIPONEGOCIO_REPO Or _strClase = TIPONEGOCIO_TTV Then
                    objClase = "A"
                ElseIf _strClase = TIPONEGOCIO_RENTAFIJA Or _strClase = TIPONEGOCIO_SIMULTANEA Or _strClase = TIPONEGOCIO_REPOC Or _strClase = TIPONEGOCIO_TTVC Then
                    objClase = "C"
                End If

                If Not IsNothing(mdcProxyUtilidad01.BuscadorEspecies) Then
                    mdcProxyUtilidad01.BuscadorEspecies.Clear()
                End If
                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarEspeciesOyDPLUSQuery("",
                                                                                      objClase,
                                                                                      A2ComunesControl.BuscadorEspecieViewModel.EstadosEspecie.A.ToString,
                                                                                      String.Empty,
                                                                                      pstrTipoNegocio,
                                                                                      pstrTipoProducto,
                                                                                      Program.Usuario,
                                                                                      False, Program.HashConexion), AddressOf buscarEspeciesComplete, "")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las especies del receptor.", Me.ToString(), "ConsultarClientesReceptorA", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ConsultarReceptoresUsuario(ByVal pstrOpcion As String)
        Try
            If Not IsNothing(mdcProxyUtilidadPLUS.tblReceptoresUsuarios) Then
                mdcProxyUtilidadPLUS.tblReceptoresUsuarios.Clear()
            End If

            mdcProxyUtilidadPLUS.Load(mdcProxyUtilidadPLUS.OYDPLUS_ConsultarReceptoresUsuarioQuery(True, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTodosReceptoresUsuario, pstrOpcion)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los receptores del usuario.", _
             Me.ToString(), "ConsultarReceptoresUsuario", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function CalcularValorOrden(ByVal pobjRegistroSeleccionado As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor) As System.Threading.Tasks.Task
        Try
            If Not IsNothing(pobjRegistroSeleccionado) Then

                If Not String.IsNullOrEmpty(pobjRegistroSeleccionado.Clase) And Not String.IsNullOrEmpty(pobjRegistroSeleccionado.TipoOperacion) _
                    And Not String.IsNullOrEmpty(pobjRegistroSeleccionado.IdEspecie) And pobjRegistroSeleccionado.ValorNominal > 0 And pobjRegistroSeleccionado.ValorGiro > 0 Then
                    If logCalcularValores Then
                        logCalcularValores = False
                        Await ObtenerCalculosMotor(pobjRegistroSeleccionado)
                        logCalcularValores = True
                    End If
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular el valor de la orden.", Me.ToString(), "CalcularValorOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Private Async Function ObtenerCalculosMotor(ByVal pobjRegistroSeleccionado As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor) As System.Threading.Tasks.Task(Of Boolean)
        Dim logLlamadoExitoso As Boolean = False
        Try
            If Not IsNothing(pobjRegistroSeleccionado) Then
                Try
                    Dim objRet As LoadOperation(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor_Calculos)
                    Dim objProxy As OYDPLUSOrdenesBolsaDomainContext

                    If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                        objProxy = New OYDPLUSOrdenesBolsaDomainContext()
                    Else
                        objProxy = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                    End If

                    'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
                    DirectCast(objProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

                    IsBusyCalculos = True

                    If Not IsNothing(objProxy.RegistroOperacionesPorReceptor_Calculos) Then
                        objProxy.RegistroOperacionesPorReceptor_Calculos.Clear()
                    End If

                    objRet = Await objProxy.Load(objProxy.RegistroOperacionesPorReceptor_CalcularSyncQuery(pobjRegistroSeleccionado.Clase,
                                                                                                           pobjRegistroSeleccionado.IdCliente,
                                                                                                           pobjRegistroSeleccionado.FechaLiquidacion,
                                                                                                           pobjRegistroSeleccionado.FechaCumplimiento,
                                                                                                           pobjRegistroSeleccionado.IdEspecie,
                                                                                                           pobjRegistroSeleccionado.Isin,
                                                                                                           pobjRegistroSeleccionado.Emision,
                                                                                                           pobjRegistroSeleccionado.Vencimiento,
                                                                                                           pobjRegistroSeleccionado.TasaFacial,
                                                                                                           pobjRegistroSeleccionado.Modalidad,
                                                                                                           pobjRegistroSeleccionado.Indicador,
                                                                                                           pobjRegistroSeleccionado.PuntosIndicador,
                                                                                                           pobjRegistroSeleccionado.ValorNominal,
                                                                                                           pobjRegistroSeleccionado.PrecioSucio,
                                                                                                           pobjRegistroSeleccionado.ValorGiro,
                                                                                                           pobjRegistroSeleccionado.ValorGiro2,
                                                                                                           Program.Usuario,
                                                                                                           Program.HashConexion)).AsTask()
                    IsBusy = False

                    If Not objRet Is Nothing Then
                        If objRet.HasError Then
                            If objRet.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al calcular los valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            Else
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "ObtenerCalculosMotor", Program.TituloSistema, Program.Maquina, objRet.Error)
                            End If
                            IsBusy = False
                            IsBusyCalculos = False
                            objRet.MarkErrorAsHandled()
                        Else
                            If objRet.Entities.Count > 0 Then
                                If objRet.Entities.First.Exitoso Then
                                    Dim objResultadoCalculos = objRet.Entities.First

                                    pobjRegistroSeleccionado.ValorNominal = objResultadoCalculos.ValorNominal
                                    pobjRegistroSeleccionado.PrecioSucio = objResultadoCalculos.Precio
                                    pobjRegistroSeleccionado.ValorGiro = objResultadoCalculos.ValorGiro
                                    'pobjRegistroSeleccionado.ValorGiro2 = objResultadoCalculos.ValorGiro2

                                    logLlamadoExitoso = True
                                Else
                                    logLlamadoExitoso = False
                                    Dim strMensajesValidacion As String = String.Empty

                                    For Each li In objRet.Entities.ToList
                                        If String.IsNullOrEmpty(strMensajesValidacion) Then
                                            strMensajesValidacion = li.Mensaje
                                        Else
                                            strMensajesValidacion = String.Format("{0}{1}{2}", strMensajesValidacion, vbCrLf, li.Mensaje)
                                        End If
                                    Next
                                    If Not String.IsNullOrEmpty(strMensajesValidacion) Then
                                        mostrarMensaje(strMensajesValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    End If

                                End If
                            End If
                        End If

                        IsBusyCalculos = False
                    End If
                Catch ex As Exception
                    IsBusy = False
                    IsBusyCalculos = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "CalcularValorRegistro", Application.Current.ToString(), Program.Maquina, ex)
                    logLlamadoExitoso = False
                Finally
                End Try
            End If
        Catch ex As Exception
            IsBusy = False
            IsBusyCalculos = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular el valor en el motor de calculos.", Me.ToString(), "ObtenerCalculosMotor", Program.TituloSistema, Program.Maquina, ex)
        End Try

        logCalcularValores = True

        Return logLlamadoExitoso
    End Function
#End Region

End Class

''' <summary>
''' Clase base para forma de búsquedas
''' </summary>
''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
Public Class CamposBusquedaOperacionesPorReceptor2
    Implements INotifyPropertyChanged

    Private _IdOperacion As Integer
    <Display(Name:="Id operación")> _
    Public Property IdOperacion() As Integer
        Get
            Return _IdOperacion
        End Get
        Set(ByVal value As Integer)
            _IdOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdOperacion"))
        End Set
    End Property

    Private _Clase As String
    <Display(Name:="Tipo negocio")> _
    Public Property Clase() As String
        Get
            Return _Clase
        End Get
        Set(ByVal value As String)
            _Clase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clase"))
        End Set
    End Property

    Private _TipoOperacion As String
    <Display(Name:="Tipo operación")> _
    Public Property TipoOperacion() As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _ReceptorA As String
    <Display(Name:="Receptor A")> _
    Public Property ReceptorA() As String
        Get
            Return _ReceptorA
        End Get
        Set(ByVal value As String)
            _ReceptorA = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ReceptorA"))
        End Set
    End Property

    Private _NombreReceptorA As String
    <Display(Name:="Receptor A")> _
    Public Property NombreReceptorA() As String
        Get
            Return _NombreReceptorA
        End Get
        Set(ByVal value As String)
            _NombreReceptorA = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreReceptorA"))
        End Set
    End Property

    Private _ReceptorB As String
    <Display(Name:="Receptor B")> _
    Public Property ReceptorB() As String
        Get
            Return _ReceptorB
        End Get
        Set(ByVal value As String)
            _ReceptorB = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ReceptorB"))
        End Set
    End Property

    Private _NombreReceptorB As String
    <Display(Name:="Receptor B")> _
    Public Property NombreReceptorB() As String
        Get
            Return _NombreReceptorB
        End Get
        Set(ByVal value As String)
            _NombreReceptorB = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreReceptorB"))
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

    Private _FechaLiquidacion As Nullable(Of DateTime)
    <Display(Name:="Fecha liquidación")> _
    Public Property FechaLiquidacion() As Nullable(Of DateTime)
        Get
            Return _FechaLiquidacion
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaLiquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaLiquidacion"))
        End Set
    End Property

    Private _FechaCumplimiento As Nullable(Of DateTime)
    <Display(Name:="Fecha cumplimiento")> _
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

