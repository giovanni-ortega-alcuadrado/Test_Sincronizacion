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
Imports A2Utilidades.Mensajes
Imports System.Runtime.InteropServices.Automation
Imports System.IO
Imports System.Threading.Tasks

Public Class FlujoOrdenesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        inicializarServicio()
    End Sub

    Private Sub inicializarServicio()

        Try
            

            IsBusy = True
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OrdenesDomainContext()
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext()
            Else
                dcProxy = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OrdenesDomainContext.IOrdenesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 10, 0)
            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 10, 0)
            DirectCast(mdcProxyUtilidad01.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 10, 0)

            If Not IsNothing(mdcProxyUtilidad01.ItemCombos) Then
                mdcProxyUtilidad01.ItemCombos.Clear()
            End If

            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.cargarCombosEspecificosQuery("FLUJOORDENES", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosFlujo, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "OrdenesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoGenerarArchivoReporte(ByVal lo As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos))
        If lo.HasError = False Then

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "TerminoGenerarArchivoReporte", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub


#End Region

#Region "Eventos"


#End Region

#Region "Constantes"

    Public Enum TipoReporte
        PDF
        XLS
    End Enum

    Private Enum PARAMETROSRUTA
        SERVIDOR
        REPORTE
        IDORDENES
        BROKER
        MAQUINA
        USUARIO
        CULTURA
        FORMATO
        VALORORDENES
        VALORUSUARIO
    End Enum

    Private Const STR_CONSULTATRADECONFIRMATION As String = ""
    Private Const STR_CONSULTAFRACCIONAMIENTO As String = "T"
    Private Const STR_CONSULTABLOTTER As String = "F"
    Private Const STR_CONSULTAPREMATCHED As String = "P"

    Private Const STRPROCESO As String = "ImpFlujoOrdenes"

    Private Const STRBROKERLOCAL As String = "0"

    Private Const STR_PARAMETROSREPORTE As String = "pstrOrdenes=[IDORDENES]&pstrBroker=[BROKER]&pstrUsuario=[USUARIO]&pstrLogUsuario=[USUARIO]&pstrMaquina=[MAQUINA]&pstrOpcion='Reporte'&pstrProceso='Flujo ordenes'&pstrRefrescar=''"
    Private Const STR_REPORTE_BROKEREXTERIOR As String = "rptFlujoOrdenes_Internacional"
    Private Const STR_REPORTE_BROKERLOCAL As String = "rptFlujoOrdenes_Local"
    Private Const STR_REPORTE_FRACCIONAMIENTO As String = "rptFlujoOrdenes_Fraccionamiento"

    Private Const STR_ARCHIVO_BROKEREXTERIOR As String = "Internacional"
    Private Const STR_ARCHIVO_BROKERLOCAL As String = "Local"
    Private Const STR_ARCHIVO_FRACCIONAMIENTO As String = "Fraccionamiento"

    Private Const STR_GENERARPREMATCHED_NOMBREARCHIVO As String = "OrdenesPreMatched"
    Private Const STR_GENERARPREMATCHED_PROCESO As String = "FlujoOrdenes_GenerarPrematched"
    Private Const STR_GENERARPREMATCHED_PARAMETROS As String = "[ORDENES]=[VALORORDENES]|[USUARIO]=[VALORUSUARIO]"

#End Region

#Region "Variables"

    Private dcProxy As OrdenesDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private dcProxyImportaciones As ImportacionesDomainContext
    Public viewFraccionamiento As FraccionamientoOrdenView

    Private BROKER_FRACCIONAMIENTO_ASUNTO As String = String.Empty
    Private BROKER_FRACCIONAMIENTO_CORREOELECTRONICO As String = String.Empty
    Private BROKER_FRACCIONAMIENTO_MENSAJECORREO As String = String.Empty
    Private BROKER_TRADECONFIRMATION_ASUNTO As String = String.Empty
    Private BROKER_TRADECONFIRMATION_MENSAJECORREO As String = String.Empty

    Private objOutlook As Object = Nothing
#End Region

#Region "Propiedades"

    Private _BrokenSeleccionado As String = String.Empty
    Public Property BrokenSeleccionado() As String
        Get
            Return _BrokenSeleccionado
        End Get
        Set(ByVal value As String)
            _BrokenSeleccionado = value
            If Not IsNothing(_BrokenSeleccionado) Then
                ConsultarOrdenesBroken()
            End If
            MyBase.CambioItem("BrokenSeleccionado")
        End Set
    End Property

    Private _TabSeleccionado As Integer
    Public Property TabSeleccionado() As Integer
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionado = value
            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property

    Private _ListaTradeConfirmation As List(Of OyDOrdenes.FlujoOrdenes)
    Public Property ListaTradeConfirmation() As List(Of OyDOrdenes.FlujoOrdenes)
        Get
            Return _ListaTradeConfirmation
        End Get
        Set(ByVal value As List(Of OyDOrdenes.FlujoOrdenes))
            _ListaTradeConfirmation = value
            MyBase.CambioItem("ListaTradeConfirmation")
        End Set
    End Property

    Private _TradeConfirmationSelected As OyDOrdenes.FlujoOrdenes
    Public Property TradeConfirmationSelected() As OyDOrdenes.FlujoOrdenes
        Get
            Return _TradeConfirmationSelected
        End Get
        Set(ByVal value As OyDOrdenes.FlujoOrdenes)
            _TradeConfirmationSelected = value
            MyBase.CambioItem("TradeConfirmationSelected")
        End Set
    End Property

    Private _ListaFraccionamiento As List(Of OyDOrdenes.FlujoOrdenes)
    Public Property ListaFraccionamiento() As List(Of OyDOrdenes.FlujoOrdenes)
        Get
            Return _ListaFraccionamiento
        End Get
        Set(ByVal value As List(Of OyDOrdenes.FlujoOrdenes))
            _ListaFraccionamiento = value
            MyBase.CambioItem("ListaFraccionamiento")
        End Set
    End Property

    Private _FraccionamientoSelected As OyDOrdenes.FlujoOrdenes
    Public Property FraccionamientoSelected() As OyDOrdenes.FlujoOrdenes
        Get
            Return _FraccionamientoSelected
        End Get
        Set(ByVal value As OyDOrdenes.FlujoOrdenes)
            _FraccionamientoSelected = value
            MyBase.CambioItem("FraccionamientoSelected")
        End Set
    End Property

    Private _ListaBlotter As List(Of OyDOrdenes.FlujoOrdenes)
    Public Property ListaBlotter() As List(Of OyDOrdenes.FlujoOrdenes)
        Get
            Return _ListaBlotter
        End Get
        Set(ByVal value As List(Of OyDOrdenes.FlujoOrdenes))
            _ListaBlotter = value
            MyBase.CambioItem("ListaBlotter")
        End Set
    End Property

    Private _BlotterSelected As OyDOrdenes.FlujoOrdenes
    Public Property BlotterSelected() As OyDOrdenes.FlujoOrdenes
        Get
            Return _BlotterSelected
        End Get
        Set(ByVal value As OyDOrdenes.FlujoOrdenes)
            _BlotterSelected = value
            MyBase.CambioItem("BlotterSelected")
        End Set
    End Property

    Private _ListaPreMATCHED As List(Of OyDOrdenes.FlujoOrdenes)
    Public Property ListaPreMATCHED() As List(Of OyDOrdenes.FlujoOrdenes)
        Get
            Return _ListaPreMATCHED
        End Get
        Set(ByVal value As List(Of OyDOrdenes.FlujoOrdenes))
            _ListaPreMATCHED = value
            MyBase.CambioItem("ListaPreMATCHED")
        End Set
    End Property

    Private _PreMATCHEDSelected As OyDOrdenes.FlujoOrdenes
    Public Property PreMATCHEDSelected() As OyDOrdenes.FlujoOrdenes
        Get
            Return _PreMATCHEDSelected
        End Get
        Set(ByVal value As OyDOrdenes.FlujoOrdenes)
            _PreMATCHEDSelected = value
            MyBase.CambioItem("PreMATCHEDSelected")
        End Set
    End Property

    Private _ListaFraccionamientoOrden As List(Of OyDOrdenes.OrdenesFraccionamiento)
    Public Property ListaFraccionamientoOrden() As List(Of OyDOrdenes.OrdenesFraccionamiento)
        Get
            Return _ListaFraccionamientoOrden
        End Get
        Set(ByVal value As List(Of OyDOrdenes.OrdenesFraccionamiento))
            _ListaFraccionamientoOrden = value
            MyBase.CambioItem("ListaFraccionamientoOrden")
        End Set
    End Property

    Private WithEvents _FraccionamientoOrdenSelected As OyDOrdenes.OrdenesFraccionamiento
    Public Property FraccionamientoOrdenSelected() As OyDOrdenes.OrdenesFraccionamiento
        Get
            Return _FraccionamientoOrdenSelected
        End Get
        Set(ByVal value As OyDOrdenes.OrdenesFraccionamiento)
            _FraccionamientoOrdenSelected = value
            MyBase.CambioItem("FraccionamientoOrdenSelected")
        End Set
    End Property

    Private _ListaBroken As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaBroken() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaBroken
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaBroken = value
            MyBase.CambioItem("ListaBroken")
        End Set
    End Property

    Private _HabilitarFraccionamientoOrden As Boolean
    Public Property HabilitarFraccionamientoOrden() As Boolean
        Get
            Return _HabilitarFraccionamientoOrden
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFraccionamientoOrden = value
            MyBase.CambioItem("HabilitarFraccionamientoOrden")
        End Set
    End Property

    Private _TotalFraccionado As Double
    Public Property TotalFraccionado() As Double
        Get
            Return _TotalFraccionado
        End Get
        Set(ByVal value As Double)
            _TotalFraccionado = value
            MyBase.CambioItem("TotalFraccionado")
        End Set
    End Property

    Private _NombreArchivo As String
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property

    Private _SeleccionarTodasTradeConfirmation As Boolean
    Public Property SeleccionarTodasTradeConfirmation() As Boolean
        Get
            Return _SeleccionarTodasTradeConfirmation
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodasTradeConfirmation = value
            If Not IsNothing(_SeleccionarTodasTradeConfirmation) Then
                If Not IsNothing(_ListaTradeConfirmation) Then
                    For Each li In _ListaTradeConfirmation
                        li.Generar = _SeleccionarTodasTradeConfirmation
                    Next
                End If
            End If
            MyBase.CambioItem("SeleccionarTodasTradeConfirmation")
        End Set
    End Property

    Private _SeleccionarTodasPreMATCHED As Boolean
    Public Property SeleccionarTodasPreMATCHED() As Boolean
        Get
            Return _SeleccionarTodasPreMATCHED
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodasPreMATCHED = value
            If Not IsNothing(_SeleccionarTodasPreMATCHED) Then
                If Not IsNothing(_ListaPreMATCHED) Then
                    For Each li In _ListaPreMATCHED
                        li.Generar = _SeleccionarTodasPreMATCHED
                    Next
                End If
            End If
            MyBase.CambioItem("SeleccionarTodasPreMATCHED")
        End Set
    End Property

    Private _SeleccionarTodasFraccionamiento As Boolean
    Public Property SeleccionarTodasFraccionamiento() As Boolean
        Get
            Return _SeleccionarTodasFraccionamiento
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodasFraccionamiento = value
            If Not IsNothing(_SeleccionarTodasFraccionamiento) Then
                If Not IsNothing(_ListaFraccionamiento) Then
                    For Each li In _ListaFraccionamiento
                        li.Generar = _SeleccionarTodasFraccionamiento
                    Next
                End If
            End If
            MyBase.CambioItem("SeleccionarTodasFraccionamiento")
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub ConsultarOrdenesBroken()
        Try
            IsBusy = True

            If Not IsNothing(dcProxy.FlujoOrdenes) Then
                dcProxy.FlujoOrdenes.Clear()
            End If

            dcProxy.Load(dcProxy.FlujoOrdenes_ConsultarQuery(BrokenSeleccionado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFlujoOrdenes, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "ConsultarOrdenesBroken", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Async Sub GenerarTradeConfirmation()
        Try
            If IsNothing(_ListaTradeConfirmation) Then
                mostrarMensaje("No se ha seleccionado ninguna orden para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _ListaTradeConfirmation.Where(Function(i) i.Generar).Count = 0 Then
                mostrarMensaje("No se ha seleccionado ninguna orden para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            'Dim strBrokerNoEnviados As String = String.Empty
            'Dim strClientesNoEnviados As String = String.Empty
            Dim objListaCorreosBrokerLocal As New List(Of InformacionCorreo)
            Dim objListaCorreosBrokerExtrangero As New List(Of InformacionCorreo)
            Dim strIDOrdenesEnviar As String = String.Empty
            Dim logEnvioCorreo As Boolean = True

            For Each li In _ListaTradeConfirmation
                If li.BrokenTrader = STRBROKERLOCAL And li.Generar Then
                    If objListaCorreosBrokerLocal.Where(Function(i) i.Filtro = li.IDComitente).Count = 0 Then
                        strIDOrdenesEnviar = String.Empty
                        For Each liCorreo In _ListaTradeConfirmation
                            If liCorreo.BrokenTrader = STRBROKERLOCAL And liCorreo.Generar And li.IDComitente = liCorreo.IDComitente Then
                                If String.IsNullOrEmpty(strIDOrdenesEnviar) Then
                                    strIDOrdenesEnviar = String.Format("{0}", liCorreo.ID)
                                Else
                                    strIDOrdenesEnviar = String.Format("{0},{1}", strIDOrdenesEnviar, liCorreo.ID)
                                End If
                            End If
                        Next

                        objListaCorreosBrokerLocal.Add(New InformacionCorreo With {.Correo = li.CorreoElectronico, .Ordenes = strIDOrdenesEnviar, .Filtro = li.IDComitente})
                    End If
                End If
            Next

            For Each li In _ListaTradeConfirmation
                If li.BrokenTrader <> STRBROKERLOCAL And li.Generar Then
                    If objListaCorreosBrokerExtrangero.Where(Function(i) i.Filtro = li.BrokenTrader).Count = 0 Then
                        strIDOrdenesEnviar = String.Empty
                        For Each liCorreo In _ListaTradeConfirmation
                            If liCorreo.BrokenTrader <> STRBROKERLOCAL And liCorreo.Generar And li.BrokenTrader = liCorreo.BrokenTrader Then
                                If String.IsNullOrEmpty(strIDOrdenesEnviar) Then
                                    strIDOrdenesEnviar = String.Format("{0}", liCorreo.ID)
                                Else
                                    strIDOrdenesEnviar = String.Format("{0},{1}", strIDOrdenesEnviar, liCorreo.ID)
                                End If
                            End If
                        Next

                        objListaCorreosBrokerExtrangero.Add(New InformacionCorreo With {.Correo = li.CorreoElectronico, .Ordenes = strIDOrdenesEnviar, .Filtro = li.BrokenTrader})
                    End If
                End If
            Next

            If objListaCorreosBrokerLocal.Count > 0 Then
                For Each li In objListaCorreosBrokerLocal
                    Dim strRutaRerpote As String = Await ObtenerRutaReporte(STR_REPORTE_BROKERLOCAL, STR_ARCHIVO_BROKERLOCAL, li.Ordenes, BrokenSeleccionado, TipoReporte.PDF)
                    If Not String.IsNullOrEmpty(strRutaRerpote) Then
                        logEnvioCorreo = EnviarCorreoElectronico(li.Correo, BROKER_TRADECONFIRMATION_ASUNTO, BROKER_TRADECONFIRMATION_MENSAJECORREO, strRutaRerpote)
                        If logEnvioCorreo = False Then
                            Exit For
                        End If
                    Else
                        logEnvioCorreo = False
                        Exit For
                    End If
                    'li.TipoPestaña = "LOCAL"
                    'ObtenerRutaReporte(STR_REPORTE_BROKERLOCAL, STR_ARCHIVO_BROKERLOCAL, li.Ordenes, BrokenSeleccionado, TipoReporte.PDF, li)
                Next
            End If

            If logEnvioCorreo Then
                If objListaCorreosBrokerExtrangero.Count > 0 Then
                    For Each li In objListaCorreosBrokerExtrangero
                        Dim strRutaRerpote As String = Await ObtenerRutaReporte(STR_REPORTE_BROKEREXTERIOR, STR_ARCHIVO_BROKEREXTERIOR, li.Ordenes, BrokenSeleccionado, TipoReporte.PDF)
                        If Not String.IsNullOrEmpty(strRutaRerpote) Then
                            logEnvioCorreo = EnviarCorreoElectronico(li.Correo, BROKER_TRADECONFIRMATION_ASUNTO, BROKER_TRADECONFIRMATION_MENSAJECORREO, strRutaRerpote)
                            If logEnvioCorreo = False Then
                                Exit For
                            End If
                        Else
                            logEnvioCorreo = False
                            Exit For
                        End If
                        'li.TipoPestaña = "EXTERIOR"
                        'ObtenerRutaReporte(STR_REPORTE_BROKEREXTERIOR, STR_ARCHIVO_BROKEREXTERIOR, li.Ordenes, BrokenSeleccionado, TipoReporte.PDF, li)
                    Next
                End If
            End If


            If logEnvioCorreo Then
                Dim strRegistrosActualizar As String = String.Empty

                For Each li In _ListaTradeConfirmation
                    If li.Generar Then
                        If String.IsNullOrEmpty(strRegistrosActualizar) Then
                            strRegistrosActualizar = String.Format("{0}", li.ID)
                        Else
                            strRegistrosActualizar = String.Format("{0}|{1}", strRegistrosActualizar, li.ID)
                        End If
                    End If
                Next

                If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                    dcProxy.tblRespuestaValidaciones.Clear()
                End If

                dcProxy.Load(dcProxy.FlujoOrdenes_ActualizarTradeConfirmationQuery(strRegistrosActualizar, Program.Usuario, Program.HashConexion), AddressOf TerminoActualizarFlujoOrdenes, "TRADECONFIRMATION")
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el trade confirmacion.", Me.ToString(), "GenerarTradeConfirmation", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Async Sub GenerarFraccionamiento()
        Try
            If IsNothing(_ListaFraccionamiento) Then
                mostrarMensaje("No se ha seleccionado ninguna orden para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _ListaFraccionamiento.Where(Function(i) i.Generar).Count = 0 Then
                mostrarMensaje("No se ha seleccionado ninguna orden para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'For Each li In _ListaFraccionamiento
            '    If li.Generar And li.CantidadFraccionada = 0 Then
            '        mostrarMensaje("Las ordenes seleccionadas tienen que tener la cantidad fraccionada diferente de cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        Exit Sub
            '    End If
            'Next

            IsBusy = True

            Dim objListaCorreosBrokerLocal As New List(Of InformacionCorreo)
            Dim objListaCorreosBrokerExtrangero As New List(Of InformacionCorreo)
            Dim strIDOrdenesEnviar As String = String.Empty
            Dim logEnvioCorreo As Boolean = True

            For Each li In _ListaFraccionamiento
                If li.BrokenTrader = STRBROKERLOCAL And li.Generar Then
                    If objListaCorreosBrokerLocal.Where(Function(i) i.Filtro = li.IDComitente).Count = 0 Then
                        strIDOrdenesEnviar = String.Empty
                        For Each liCorreo In _ListaFraccionamiento
                            If liCorreo.BrokenTrader = STRBROKERLOCAL And liCorreo.Generar And li.IDComitente = liCorreo.IDComitente Then
                                If String.IsNullOrEmpty(strIDOrdenesEnviar) Then
                                    strIDOrdenesEnviar = String.Format("{0}", liCorreo.ID)
                                Else
                                    strIDOrdenesEnviar = String.Format("{0},{1}", strIDOrdenesEnviar, liCorreo.ID)
                                End If
                            End If
                        Next

                        objListaCorreosBrokerLocal.Add(New InformacionCorreo With {.Correo = li.CorreoElectronico, .Ordenes = strIDOrdenesEnviar, .Filtro = li.IDComitente})
                    End If
                End If
            Next

            For Each li In _ListaFraccionamiento
                If li.BrokenTrader <> STRBROKERLOCAL And li.Generar Then
                    If objListaCorreosBrokerExtrangero.Where(Function(i) i.Filtro = li.BrokenTrader).Count = 0 Then
                        strIDOrdenesEnviar = String.Empty
                        For Each liCorreo In _ListaFraccionamiento
                            If liCorreo.BrokenTrader <> STRBROKERLOCAL And liCorreo.Generar And li.BrokenTrader = liCorreo.BrokenTrader Then
                                If String.IsNullOrEmpty(strIDOrdenesEnviar) Then
                                    strIDOrdenesEnviar = String.Format("{0}", liCorreo.ID)
                                Else
                                    strIDOrdenesEnviar = String.Format("{0},{1}", strIDOrdenesEnviar, liCorreo.ID)
                                End If
                            End If
                        Next

                        objListaCorreosBrokerExtrangero.Add(New InformacionCorreo With {.Correo = li.CorreoElectronico, .Ordenes = strIDOrdenesEnviar, .Filtro = li.BrokenTrader})
                    End If
                End If
            Next

            If objListaCorreosBrokerLocal.Count > 0 Then
                For Each li In objListaCorreosBrokerLocal
                    Dim strRutaRerpote As String = Await ObtenerRutaReporte(STR_REPORTE_FRACCIONAMIENTO, STR_ARCHIVO_FRACCIONAMIENTO, li.Ordenes, BrokenSeleccionado, TipoReporte.XLS)
                    If Not String.IsNullOrEmpty(strRutaRerpote) Then
                        logEnvioCorreo = EnviarCorreoElectronico(BROKER_FRACCIONAMIENTO_CORREOELECTRONICO, BROKER_FRACCIONAMIENTO_ASUNTO, BROKER_FRACCIONAMIENTO_MENSAJECORREO, strRutaRerpote)
                        If logEnvioCorreo = False Then
                            Exit For
                        End If
                    Else
                        logEnvioCorreo = False
                        Exit For
                    End If

                    'li.TipoPestaña = "LOCAL"
                    'ObtenerRutaReporte(STR_REPORTE_BROKERLOCAL, STR_ARCHIVO_BROKERLOCAL, li.Ordenes, BrokenSeleccionado, TipoReporte.PDF, li)
                Next
            End If

            If logEnvioCorreo Then
                If objListaCorreosBrokerExtrangero.Count > 0 Then
                    For Each li In objListaCorreosBrokerExtrangero
                        Dim strRutaRerpote As String = Await ObtenerRutaReporte(STR_REPORTE_FRACCIONAMIENTO, STR_ARCHIVO_FRACCIONAMIENTO, li.Ordenes, BrokenSeleccionado, TipoReporte.XLS)
                        If Not String.IsNullOrEmpty(strRutaRerpote) Then
                            logEnvioCorreo = EnviarCorreoElectronico(BROKER_FRACCIONAMIENTO_CORREOELECTRONICO, BROKER_FRACCIONAMIENTO_ASUNTO, BROKER_FRACCIONAMIENTO_MENSAJECORREO, strRutaRerpote)
                            If logEnvioCorreo = False Then
                                Exit For
                            End If
                        Else
                            logEnvioCorreo = False
                            Exit For
                        End If
                        'li.TipoPestaña = "EXTERIOR"
                        'ObtenerRutaReporte(STR_REPORTE_BROKEREXTERIOR, STR_ARCHIVO_BROKEREXTERIOR, li.Ordenes, BrokenSeleccionado, TipoReporte.PDF, li)
                    Next
                End If
            End If

            If logEnvioCorreo Then
                Dim strRegistrosActualizar As String = String.Empty

                For Each li In _ListaFraccionamiento
                    If li.Generar Then
                        If String.IsNullOrEmpty(strRegistrosActualizar) Then
                            strRegistrosActualizar = String.Format("{0}", li.ID)
                        Else
                            strRegistrosActualizar = String.Format("{0}|{1}", strRegistrosActualizar, li.ID)
                        End If
                    End If
                Next

                If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                    dcProxy.tblRespuestaValidaciones.Clear()
                End If

                dcProxy.Load(dcProxy.FlujoOrdenes_ActualizarFraccionamientoQuery(strRegistrosActualizar, Program.Usuario, Program.HashConexion), AddressOf TerminoActualizarFlujoOrdenes, "FRACCIONAMIENTO")
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "GenerarFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub ImportarArchivoRespueta()
        Try
            IsBusy = True
            If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            End If

            dcProxyImportaciones.Load(dcProxyImportaciones.FlujoOrdenes_CargarArchivoRespuestaQuery(NombreArchivo, STRPROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarArchivoRespuestaFlujo, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "ImportarArchivoRespueta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub DeshacerPreMATCHED()
        Try
            If IsNothing(_ListaPreMATCHED) Then
                mostrarMensaje("No se ha seleccionado ninguna orden para deshacer.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _ListaPreMATCHED.Where(Function(i) i.Generar).Count = 0 Then
                mostrarMensaje("No se ha seleccionado ninguna orden para deshacer.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            Dim strRegistrosActualizar As String = String.Empty

            For Each li In _ListaPreMATCHED
                If li.Generar Then
                    If String.IsNullOrEmpty(strRegistrosActualizar) Then
                        strRegistrosActualizar = String.Format("{0}", li.ID)
                    Else
                        strRegistrosActualizar = String.Format("{0}|{1}", strRegistrosActualizar, li.ID)
                    End If
                End If
            Next

            If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                dcProxy.tblRespuestaValidaciones.Clear()
            End If

            dcProxy.Load(dcProxy.FlujoOrdenes_DeshacerOrdenQuery(strRegistrosActualizar, Program.Usuario, Program.HashConexion), AddressOf TerminoActualizarFlujoOrdenes, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "GenerarFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub GenerarPreMATCHED()
        Try
            If IsNothing(_ListaPreMATCHED) Then
                mostrarMensaje("No se ha seleccionado ninguna orden para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _ListaPreMATCHED.Where(Function(i) i.Generar).Count = 0 Then
                mostrarMensaje("No se ha seleccionado ninguna orden para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _ListaPreMATCHED.Where(Function(i) i.Generar).Count > 1 Then
                mostrarMensaje("Solo puede seleccionar una orden para generar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            Dim strRegistrosActualizar As String = String.Empty

            For Each li In _ListaPreMATCHED
                If li.Generar Then
                    If String.IsNullOrEmpty(strRegistrosActualizar) Then
                        strRegistrosActualizar = String.Format("{0}", li.ID)
                    Else
                        strRegistrosActualizar = String.Format("{0},{1}", strRegistrosActualizar, li.ID)
                    End If
                End If
            Next

            Dim strParametrosEnviar As String = STR_GENERARPREMATCHED_PARAMETROS
            Dim pstrNombreArchivo As String = STR_GENERARPREMATCHED_NOMBREARCHIVO & "_" & Now.ToString("yyyyMMddHHmmss")

            strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.VALORORDENES), strRegistrosActualizar)
            strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.VALORUSUARIO), Program.Usuario)

            If Not IsNothing(mdcProxyUtilidad01.GenerarArchivosPlanos) Then
                mdcProxyUtilidad01.GenerarArchivosPlanos.Clear()
            End If

            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.GenerarArchivoPlanoQuery(STRPROCESO, STR_GENERARPREMATCHED_PROCESO, strParametrosEnviar, pstrNombreArchivo, ",", "CSV", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarPreMatched, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "GenerarFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub EditarFraccionamientoOrden()
        Try
            IsBusy = True

            If Not IsNothing(dcProxy.OrdenesFraccionamientos) Then
                dcProxy.OrdenesFraccionamientos.Clear()
            End If

            dcProxy.Load(dcProxy.FlujoOrdenes_ConsultarFraccionamientoQuery(FraccionamientoSelected.ID, BrokenSeleccionado, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarFraccionamientoOrden, "EDITAR")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "EditarFraccionamientoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub ConsultarFraccionamientoOrdenBlotter()
        Try
            IsBusy = True

            If Not IsNothing(dcProxy.OrdenesFraccionamientos) Then
                dcProxy.OrdenesFraccionamientos.Clear()
            End If

            dcProxy.Load(dcProxy.FlujoOrdenes_ConsultarFraccionamientoQuery(BlotterSelected.ID, BrokenSeleccionado, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarFraccionamientoOrdenBlotter, "CONSULTAR")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "ConsultarFraccionamientoOrdenBlotter", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub ConsultarFraccionamientoOrdenPreMatched()
        Try
            IsBusy = True

            If Not IsNothing(dcProxy.OrdenesFraccionamientos) Then
                dcProxy.OrdenesFraccionamientos.Clear()
            End If

            dcProxy.Load(dcProxy.FlujoOrdenes_ConsultarFraccionamientoQuery(PreMATCHEDSelected.ID, BrokenSeleccionado, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarFraccionamientoOrdenPreMatched, "CONSULTAR")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "ConsultarFraccionamientoOrdenPreMatched", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub ActualizarFraccionamiento()
        Try
            If Not IsNothing(_ListaFraccionamientoOrden) Then
                For Each li In _ListaFraccionamientoOrden
                    If li.Cantidad = 0 Then
                        mostrarMensaje("La cantidad no puede ser cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(li.NroDocumento) Then
                        mostrarMensaje("El Cliente No OyD es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(li.NombreCliente) Then
                        mostrarMensaje("El Nombre cliente OyD es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Next

                TotalFraccionado = 0
                For Each LI In _ListaFraccionamientoOrden
                    TotalFraccionado += LI.Cantidad
                Next
            End If

            If Not IsNothing(_FraccionamientoSelected) Then
                If TotalFraccionado > _FraccionamientoSelected.Cantidad Then
                    mostrarMensaje("La cantidad fraccionada no puede ser superior a la cantidad de la orden.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            IsBusy = True

            Dim strRegistrosActualizar As String = String.Empty

            For Each LI In _ListaFraccionamientoOrden
                If String.IsNullOrEmpty(strRegistrosActualizar) Then
                    strRegistrosActualizar = String.Format("{0}**{1}**{2}**{3}**{4}", LI.ID, LI.IDComitente, LI.NroDocumento, LI.NombreCliente, LI.Cantidad)
                Else
                    strRegistrosActualizar = String.Format("{0}|{1}**{2}**{3}**{4}**{5}", strRegistrosActualizar, LI.ID, LI.IDComitente, LI.NroDocumento, LI.NombreCliente, LI.Cantidad)
                End If
            Next

            If Not IsNothing(dcProxy.OrdenesFraccionamientos) Then
                dcProxy.OrdenesFraccionamientos.Clear()
            End If

            dcProxy.Load(dcProxy.FlujoOrdenes_ActualizarFraccionamientoOrdenQuery(_FraccionamientoSelected.ID, strRegistrosActualizar, Program.Usuario, Program.HashConexion), AddressOf TerminoActualizarFlujoOrdenes, "FRACCIONAMIENTOORDEN")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "ActualizarFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub NuevoRegistrFraccionamiento()
        Try
            Dim objNuevaLista As New List(Of OyDOrdenes.OrdenesFraccionamiento)

            If Not IsNothing(_ListaFraccionamientoOrden) Then
                For Each li In _ListaFraccionamientoOrden
                    objNuevaLista.Add(li)
                Next
            End If

            Dim objNuevo As New OyDOrdenes.OrdenesFraccionamiento
            objNuevo.ID = -(objNuevaLista.Count + 1)
            objNuevo.Cantidad = 0

            objNuevaLista.Add(objNuevo)

            ListaFraccionamientoOrden = objNuevaLista
            FraccionamientoOrdenSelected = _ListaFraccionamientoOrden.LastOrDefault
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "NuevoRegistrFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub BorrarRegistrFraccionamiento()
        Try
            If Not IsNothing(_FraccionamientoOrdenSelected) Then
                Dim objNuevaLista As New List(Of OyDOrdenes.OrdenesFraccionamiento)

                If Not IsNothing(_ListaFraccionamientoOrden) Then
                    For Each li In _ListaFraccionamientoOrden
                        objNuevaLista.Add(li)
                    Next
                End If

                If objNuevaLista.Where(Function(i) i.ID = _FraccionamientoOrdenSelected.ID).Count > 0 Then
                    objNuevaLista.Remove(objNuevaLista.Where(Function(i) i.ID = _FraccionamientoOrdenSelected.ID).First)
                End If

                ListaFraccionamientoOrden = objNuevaLista

                If _ListaFraccionamientoOrden.Count > 0 Then
                    FraccionamientoOrdenSelected = _ListaFraccionamientoOrden.First
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "BorrarRegistrFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub _FraccionamientoOrdenSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _FraccionamientoOrdenSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "cantidad"
                    TotalFraccionado = 0
                    For Each li In _ListaFraccionamientoOrden
                        TotalFraccionado += li.Cantidad
                    Next
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el fraccionamiento.", Me.ToString(), "_FraccionamientoOrdenSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Function EnviarCorreoElectronico(ByVal pstrDestinatarios As String, ByVal pstrAsunto As String, ByVal pstrMensajeCorreo As String, ByVal pstrRutaArchivoAdjunto As String) As Boolean
        Dim logEnvioCorreo As Boolean = False
        Try
            Dim objEmail = objOutlook.CreateItem(0)
                objEmail.To = pstrDestinatarios
                If Not String.IsNullOrEmpty(pstrRutaArchivoAdjunto) Then
                    objEmail.Attachments.Add(pstrRutaArchivoAdjunto)
                End If
                objEmail.Subject = pstrAsunto
                objEmail.HTMLBody = pstrMensajeCorreo
                objEmail.Display()
                objEmail.Save()
                logEnvioCorreo = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enviar el correo electronico.", Me.ToString(), "EnviarCorreoElectronico", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
        Return logEnvioCorreo
    End Function

    Private Async Function ObtenerRutaReporte(ByVal pstrNombreReporte As String, ByVal pstrNombreArchivo As String, ByVal pstrIDOrdenes As String, ByVal pstrBroker As String, ByVal pstrOpcion As TipoReporte) As Task(Of String)
        Try
            Dim objRet As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos)
            Dim objProxy = New UtilidadesDomainContext()
            Dim strParametrosReporte As String = STR_PARAMETROSREPORTE
            Dim strRutaServidor As String = Program.ServidorReportesEjecution
            Dim strNombreReporte As String = Program.CarpetaReportes

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                objProxy = New UtilidadesDomainContext()
            Else
                objProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 10, 0)

            If Right(strRutaServidor, 1) = "/" Then
                strRutaServidor = Left(strRutaServidor, Len(strRutaServidor) - 1)
            End If

            If Left(strNombreReporte, 1) <> "/" Then
                strNombreReporte = "/" & strNombreReporte
            End If

            If Right(strNombreReporte, 1) <> "/" Then
                strNombreReporte = strNombreReporte & "/"
            End If

            strNombreReporte = strNombreReporte & pstrNombreReporte

            pstrNombreArchivo = pstrNombreArchivo & "_" & Now.ToString("yyyyMMddHHmmss")

            strParametrosReporte = strParametrosReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.MAQUINA), Program.Maquina)
            strParametrosReporte = strParametrosReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.USUARIO), Program.Usuario)
            strParametrosReporte = strParametrosReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.IDORDENES), pstrIDOrdenes)
            strParametrosReporte = strParametrosReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.BROKER), BrokenSeleccionado)

            If Not IsNothing(objProxy.GenerarArchivosPlanos) Then
                objProxy.GenerarArchivosPlanos.Clear()
            End If

            objRet = Await objProxy.Load(objProxy.GenerarArchivoReporteSyncQuery(strRutaServidor, _
                                                                                strNombreReporte, _
                                                                                strParametrosReporte, _
                                                                                "&",
                                                                                pstrOpcion.ToString, _
                                                                                "FlujoOrdenes", _
                                                                                pstrNombreArchivo, _
                                                                                Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al generar el reporte.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el reporte.", "ObtenerRutaReporte", Program.TituloSistema, Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                    Return String.Empty
                Else
                    If objRet.Entities.Count > 0 Then
                        If objRet.Entities.First.Exitoso Then
                            Return objRet.Entities.First.RutaArchivoPlano
                        Else
                            mostrarMensaje(objRet.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Return String.Empty
                        End If
                    Else
                        Return String.Empty
                    End If
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el reporte.", Me.ToString(), "ObtenerRutaReporte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Return String.Empty
        End Try
    End Function

    'Private Sub ObtenerRutaReporte(ByVal pstrNombreReporte As String, ByVal pstrNombreArchivo As String, ByVal pstrIDOrdenes As String, ByVal pstrBroker As String, ByVal pstrOpcion As TipoReporte, ByVal pobjUserState As Object)
    '    Try
    '        Dim objProxy = New UtilidadesDomainContext()
    '        Dim strParametrosReporte As String = STR_PARAMETROSREPORTE
    '        Dim strRutaServidor As String = Program.ServidorReportesEjecution
    '        Dim strNombreReporte As String = Program.CarpetaReportes

    '        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
    '            objProxy = New UtilidadesDomainContext()
    '        Else
    '            objProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
    '        End If

    '        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)

    '        If Right(strRutaServidor, 1) = "/" Then
    '            strRutaServidor = Left(strRutaServidor, Len(strRutaServidor) - 1)
    '        End If

    '        If Left(strNombreReporte, 1) <> "/" Then
    '            strNombreReporte = "/" & strNombreReporte
    '        End If

    '        If Right(strNombreReporte, 1) <> "/" Then
    '            strNombreReporte = strNombreReporte & "/"
    '        End If

    '        strNombreReporte = strNombreReporte & pstrNombreReporte

    '        pstrNombreArchivo = pstrNombreArchivo & "_" & Now.ToString("yyyyMMddHHmmss")

    '        strParametrosReporte = strParametrosReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.MAQUINA), Program.Maquina)
    '        strParametrosReporte = strParametrosReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.USUARIO), Program.Usuario)
    '        strParametrosReporte = strParametrosReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.IDORDENES), pstrIDOrdenes)
    '        strParametrosReporte = strParametrosReporte.Replace(ValorAReemplazar(PARAMETROSRUTA.BROKER), BrokenSeleccionado)

    '        If Not IsNothing(objProxy.GenerarArchivosPlanos) Then
    '            objProxy.GenerarArchivosPlanos.Clear()
    '        End If

    '        objProxy.Load(objProxy.GenerarArchivoReporteSyncQuery(strRutaServidor, _
    '                                                                            strNombreReporte, _
    '                                                                            strParametrosReporte, _
    '                                                                            "&",
    '                                                                            pstrOpcion.ToString, _
    '                                                                            "FlujoOrdenes", _
    '                                                                            pstrNombreArchivo, _
    '                                                                            Program.Usuario, Program.HashConexion), AddressOf TerminoObtenerRutaReporte, pobjUserState)

    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el reporte.", Me.ToString(), "ObtenerRutaReporte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '    End Try
    'End Sub

    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSRUTA) As String
        Return String.Format("[{0}]", pintTipoCampo.ToString)
    End Function

#End Region

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerFlujoOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.FlujoOrdenes))
        Try
            If lo.HasError = False Then
                Dim objListaTradeConfirmation As New List(Of OyDOrdenes.FlujoOrdenes)
                Dim objListaFraccionamiento As New List(Of OyDOrdenes.FlujoOrdenes)
                Dim objListaBlooter As New List(Of OyDOrdenes.FlujoOrdenes)
                Dim objListaPreMatched As New List(Of OyDOrdenes.FlujoOrdenes)

                If lo.Entities.Count > 0 Then
                    For Each li In lo.Entities.ToList
                        If li.EstadoFlujo = STR_CONSULTATRADECONFIRMATION Then
                            objListaTradeConfirmation.Add(li)
                        ElseIf li.EstadoFlujo = STR_CONSULTAFRACCIONAMIENTO Then
                            objListaFraccionamiento.Add(li)
                        ElseIf li.EstadoFlujo = STR_CONSULTABLOTTER Then
                            objListaFraccionamiento.Add(li)
                            objListaBlooter.Add(li)
                        ElseIf li.EstadoFlujo = STR_CONSULTAPREMATCHED Then
                            objListaPreMatched.Add(li)
                        End If
                    Next
                End If

                ListaTradeConfirmation = objListaTradeConfirmation
                ListaFraccionamiento = objListaFraccionamiento
                ListaBlotter = objListaBlooter
                ListaPreMATCHED = objListaPreMatched

                SeleccionarTodasTradeConfirmation = False
                SeleccionarTodasFraccionamiento = False
                SeleccionarTodasPreMATCHED = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerFlujoOrdenes", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerFlujoOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoTraerTradeConfirmation(ByVal lo As LoadOperation(Of OyDOrdenes.FlujoOrdenes))
        Try
            If lo.HasError = False Then
                ListaTradeConfirmation = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerTradeConfirmation", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerTradeConfirmation", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerFraccionamiento(ByVal lo As LoadOperation(Of OyDOrdenes.FlujoOrdenes))
        Try
            If lo.HasError = False Then
                ListaFraccionamiento = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerFraccionamiento", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerBlotter(ByVal lo As LoadOperation(Of OyDOrdenes.FlujoOrdenes))
        Try
            If lo.HasError = False Then
                ListaBlotter = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerFraccionamiento", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerPreMached(ByVal lo As LoadOperation(Of OyDOrdenes.FlujoOrdenes))
        Try
            If lo.HasError = False Then
                ListaPreMATCHED = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerPreMached", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los flujo de las ordenes.", Me.ToString(), "TerminoTraerPreMached", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoCargarCombosFlujo(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.Categoria = "BROKER_TRADE").Count > 0 Then
                        ListaBroken = lo.Entities.Where(Function(i) i.Categoria = "BROKER_TRADE").ToList

                        BrokenSeleccionado = "-"
                    End If

                    If Not IsNothing(mdcProxyUtilidad01.valoresparametros) Then
                        mdcProxyUtilidad01.valoresparametros.Clear()
                    End If

                    mdcProxyUtilidad01.Load(mdcProxyUtilidad01.listaVerificaparametroQuery(String.Empty, "FLUJOORDENES", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarParametrosFlujo, String.Empty)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoTraerFraccionamiento", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoTraerFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoCargarParametrosFlujo(ByVal lo As LoadOperation(Of OYDUtilidades.valoresparametro))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    For Each li In lo.Entities.ToList
                        If li.Parametro = "BROKER_FRACCIONAMIENTO_ASUNTO" Then
                            BROKER_FRACCIONAMIENTO_ASUNTO = li.Valor
                        ElseIf li.Parametro = "BROKER_FRACCIONAMIENTO_CORREOELECTRONICO" Then
                            BROKER_FRACCIONAMIENTO_CORREOELECTRONICO = li.Valor
                        ElseIf li.Parametro = "BROKER_FRACCIONAMIENTO_MENSAJECORREO" Then
                            BROKER_FRACCIONAMIENTO_MENSAJECORREO = li.Valor
                        ElseIf li.Parametro = "BROKER_TRADECONFIRMATION_ASUNTO" Then
                            BROKER_TRADECONFIRMATION_ASUNTO = li.Valor
                        ElseIf li.Parametro = "BROKER_TRADECONFIRMATION_MENSAJECORREO" Then
                            BROKER_TRADECONFIRMATION_MENSAJECORREO = li.Valor
                        End If
                    Next
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoTraerFraccionamiento", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoTraerFraccionamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoActualizarFlujoOrdenes(ByVal lo As LoadOperation(Of OyDOrdenes.tblRespuestaValidaciones))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.First.Exitoso Then
                        If lo.UserState = "FRACCIONAMIENTOORDEN" Then
                            viewFraccionamiento.Close()
                        End If

                        ConsultarOrdenesBroken()
                    Else
                        IsBusy = False
                    End If
                Else
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar la información de la orden.", Me.ToString(), "TerminoActualizarTradeConfirmation", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar la información de la orden.", Me.ToString(), "TerminoActualizarTradeConfirmation", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoGenerarPreMatched(ByVal lo As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.First.Exitoso Then
                        Dim strRutaReporte As String = lo.Entities.First.RutaArchivoPlano
                        Dim strNroVentana As String = String.Empty

                        'Visualiza el reporte en pantalla.
                        Program.VisorArchivosWeb_CargarURL(strRutaReporte)

                        Dim strRegistrosActualizar As String = String.Empty

                        For Each li In _ListaPreMATCHED
                            If li.Generar Then
                                If String.IsNullOrEmpty(strRegistrosActualizar) Then
                                    strRegistrosActualizar = String.Format("{0}", li.ID)
                                Else
                                    strRegistrosActualizar = String.Format("{0}|{1}", strRegistrosActualizar, li.ID)
                                End If
                            End If
                        Next

                        If Not IsNothing(dcProxy.tblRespuestaValidaciones) Then
                            dcProxy.tblRespuestaValidaciones.Clear()
                        End If

                        dcProxy.Load(dcProxy.FlujoOrdenes_ActualizarPreMatchedQuery(strRegistrosActualizar, Program.Usuario, Program.HashConexion), AddressOf TerminoActualizarFlujoOrdenes, "PREMATCHED")
                    Else
                        mostrarMensaje(lo.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento al genera el archivo de prematched.", Me.ToString(), "TerminoGenerarPreMatched", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento al genera el archivo de prematched.", Me.ToString(), "TerminoGenerarPreMatched", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarFraccionamientoOrden(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenesFraccionamiento))
        Try
            If lo.HasError = False Then
                ListaFraccionamientoOrden = lo.Entities.ToList

                TotalFraccionado = 0

                If _ListaFraccionamientoOrden.Count > 0 Then
                    For Each li In _ListaFraccionamientoOrden
                        TotalFraccionado += li.Cantidad
                    Next
                End If

                HabilitarFraccionamientoOrden = True

                Dim objFraccionamiento As New FraccionamientoOrdenView(Me)
                Program.Modal_OwnerMainWindowsPrincipal(objFraccionamiento)
                objFraccionamiento.Title = String.Format("Fraccionar orden - {0} - Cantidad total {1:N2}", _FraccionamientoSelected.NroOrden, _FraccionamientoSelected.Cantidad)
                objFraccionamiento.ShowDialog()

                If Not IsNothing(_ListaFraccionamientoOrden) Then
                    If _ListaFraccionamientoOrden.Count > 0 Then
                        FraccionamientoOrdenSelected = _ListaFraccionamientoOrden.First
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoConsultarFraccionamientoOrden", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoConsultarFraccionamientoOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarFraccionamientoOrdenBlotter(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenesFraccionamiento))
        Try
            If lo.HasError = False Then
                ListaFraccionamientoOrden = lo.Entities.ToList

                TotalFraccionado = 0

                If _ListaFraccionamientoOrden.Count > 0 Then
                    For Each li In _ListaFraccionamientoOrden
                        TotalFraccionado += li.Cantidad
                        li.HabilitarDocumento = False
                    Next
                End If

                HabilitarFraccionamientoOrden = False

                Dim objFraccionamiento As New FraccionamientoOrdenView(Me)
                Program.Modal_OwnerMainWindowsPrincipal(objFraccionamiento)
                objFraccionamiento.Title = String.Format("Fraccionar orden - {0} - Cantidad total {1:N2}", _BlotterSelected.NroOrden, _BlotterSelected.Cantidad)
                objFraccionamiento.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoConsultarFraccionamientoOrdenBlotter", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoConsultarFraccionamientoOrdenBlotter", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarFraccionamientoOrdenPreMatched(ByVal lo As LoadOperation(Of OyDOrdenes.OrdenesFraccionamiento))
        Try
            If lo.HasError = False Then
                ListaFraccionamientoOrden = lo.Entities.ToList

                TotalFraccionado = 0

                If _ListaFraccionamientoOrden.Count > 0 Then
                    For Each li In _ListaFraccionamientoOrden
                        TotalFraccionado += li.Cantidad
                        li.HabilitarDocumento = False
                    Next
                End If

                HabilitarFraccionamientoOrden = False

                Dim objFraccionamiento As New FraccionamientoOrdenView(Me)
                Program.Modal_OwnerMainWindowsPrincipal(objFraccionamiento)
                objFraccionamiento.Title = String.Format("Fraccionar orden - {0} - Cantidad total {1:N2}", _PreMATCHEDSelected.NroOrden, _PreMATCHEDSelected.Cantidad)
                objFraccionamiento.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoConsultarFraccionamientoOrdenPreMatched", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el fraccionamiento de la orden.", Me.ToString(), "TerminoConsultarFraccionamientoOrdenPreMatched", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoCargarArchivoRespuestaFlujo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If lo.HasError = False Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim ViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones(NombreArchivo)

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
                        ViewImportarArchivo.Title = "Subir archivo fondos"
                    Else
                        If objListaRespuesta.Where(Function(i) i.Exitoso = True).Count > 0 Then
                            ViewImportarArchivo.ListaMensajes.Add(objListaRespuesta.Where(Function(i) i.Exitoso = True).First.Mensaje)
                        Else
                            ViewImportarArchivo.ListaMensajes.Add("Se importo el archivo exitosamente.")
                        End If

                        ViewImportarArchivo.IsBusy = False
                        ViewImportarArchivo.Title = "Subir archivo fondos"
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo fondos"
                End If

                Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
                ViewImportarArchivo.ShowDialog()

                ConsultarOrdenesBroken()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de la respuesta del flujo de ordenes.", Me.ToString(), "TerminoCargarArchivoRespuestaFlujo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de la respuesta del flujo de ordenes.", Me.ToString(), "TerminoCargarArchivoRespuestaFlujo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    'Private Sub TerminoObtenerRutaReporte(ByVal lo As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos))
    '    Try
    '        If lo.HasError = False Then
    '            If lo.Entities.Count > 0 Then
    '                If lo.Entities.First.Exitoso Then
    '                    Dim objInformacionCorreo As InformacionCorreo = CType(lo.UserState, InformacionCorreo)
    '                    Dim strRutaReporte As String = lo.Entities.First.RutaArchivoPlano

    '                    If objInformacionCorreo.TipoPestaña = "LOCAL" Then
    '                        EnviarCorreoElectronico(objInformacionCorreo.Correo, BROKER_TRADECONFIRMATION_ASUNTO, BROKER_TRADECONFIRMATION_MENSAJECORREO, strRutaReporte)
    '                    ElseIf objInformacionCorreo.TipoPestaña = "EXTERIOR" Then
    '                        EnviarCorreoElectronico(objInformacionCorreo.Correo, BROKER_TRADECONFIRMATION_ASUNTO, BROKER_TRADECONFIRMATION_MENSAJECORREO, strRutaReporte)
    '                    ElseIf objInformacionCorreo.TipoPestaña = "FRACCIONAMIENTO" Then
    '                        EnviarCorreoElectronico(objInformacionCorreo.Correo, BROKER_FRACCIONAMIENTO_ASUNTO, BROKER_FRACCIONAMIENTO_MENSAJECORREO, strRutaReporte)
    '                    End If

    '                Else
    '                    mostrarMensaje(lo.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '                End If
    '            End If
    '        Else
    '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de fondos.", Me.ToString(), "TerminoCargarArchivoRecibos", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de fondos.", Me.ToString(), "TerminoCargarArchivoRecibos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '    End Try
    'End Sub

#End Region

End Class

Public Class InformacionCorreo
    Public Property TipoPestaña As String
    Public Property Filtro As String
    Public Property Correo As String
    Public Property Ordenes As String
End Class