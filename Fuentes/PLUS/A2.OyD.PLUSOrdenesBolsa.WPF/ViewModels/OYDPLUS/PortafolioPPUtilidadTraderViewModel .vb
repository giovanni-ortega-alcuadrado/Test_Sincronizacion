Imports Telerik.Windows.Controls
'Desarrollado Por:      Santiago Alexander Vergara Orrego
'Descripcion:           Desarrollo Av-Ticket - Forma para mostrar el portafolio de posición propia segun un trader
'                       Ademas permite visualizar el total de rentabilidad
'Fecha:                 Febrero 28/2013
'------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Windows.Data
Imports A2ControlMenu
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Windows.Messaging
Imports System.Web
Imports OpenRiaServices.DomainServices.Client


Public Class PortafolioPPUtilidadTraderViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        MyBase.New()

        Try

            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext()
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad01.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If Not Program.IsDesignMode() Then
                dtmFechaInicial = Date.Now
                dtmFechaFinal = Date.Now
                logIncluirFechas = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", Me.ToString(), "PortafolioPPUtilidadTraderViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Variables"

    Private dcProxy As OYDPLUSOrdenesBolsaDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private logLlamadoAutomatico As Boolean

#End Region

#Region "Propiedades"

    Private _ListaPortafolio As List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
    Public Property ListaPortafolio() As List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
        Get
            Return _ListaPortafolio
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia))
            _ListaPortafolio = value
            MyBase.CambioItem("ListaPortafolio")
            MyBase.CambioItem("ListaPortafolioPaged")
        End Set
    End Property

    Public ReadOnly Property ListaPortafolioPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPortafolio) Then
                Dim view = New PagedCollectionView(_ListaPortafolio)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _strReceptor As String
    Public Property strReceptor() As String
        Get
            Return _strReceptor
        End Get
        Set(ByVal value As String)
            _strReceptor = value
            MyBase.CambioItem("strReceptor")
        End Set
    End Property

    Private _strNombreReceptor As String
    Public Property strNombreReceptor() As String
        Get
            Return _strNombreReceptor
        End Get
        Set(ByVal value As String)
            _strNombreReceptor = value
            MyBase.CambioItem("strNombreReceptor")
        End Set
    End Property

    Private _intEmisor As Integer
    Public Property intEmisor() As Integer
        Get
            Return _intEmisor
        End Get
        Set(ByVal value As Integer)
            _intEmisor = value
            MyBase.CambioItem("intEmisor")
        End Set
    End Property

    Private _strEmisor As String
    Public Property strEmisor() As String
        Get
            Return _strEmisor
        End Get
        Set(ByVal value As String)
            _strEmisor = value
            MyBase.CambioItem("strEmisor")
        End Set
    End Property

    Private _intMesa As Integer
    Public Property intMesa() As Integer
        Get
            Return _intMesa
        End Get
        Set(ByVal value As Integer)
            _intMesa = value
            MyBase.CambioItem("intMesa")
        End Set
    End Property

    Private _strNemotecnico As String
    Public Property strNemotecnico() As String
        Get
            Return _strNemotecnico
        End Get
        Set(ByVal value As String)
            _strNemotecnico = value
            MyBase.CambioItem("strNemotecnico")
        End Set
    End Property

    Private _strEspecie As String
    Public Property strEspecie() As String
        Get
            Return _strEspecie
        End Get
        Set(ByVal value As String)
            _strEspecie = value
            MyBase.CambioItem("strEspecie")
        End Set
    End Property

    Private _logIncluirFechas As Boolean
    Public Property logIncluirFechas() As Boolean
        Get
            Return _logIncluirFechas
        End Get
        Set(ByVal value As Boolean)
            _logIncluirFechas = value
            MyBase.CambioItem("logIncluirFechas")
        End Set
    End Property

    Private _dtmFechaInicial As DateTime
    Public Property dtmFechaInicial() As DateTime
        Get
            Return _dtmFechaInicial
        End Get
        Set(ByVal value As DateTime)
            _dtmFechaInicial = value
            MyBase.CambioItem("dtmFechaInicial")
        End Set
    End Property

    Private _dtmFechaFinal As DateTime
    Public Property dtmFechaFinal() As DateTime
        Get
            Return _dtmFechaFinal
        End Get
        Set(ByVal value As DateTime)
            _dtmFechaFinal = value
            MyBase.CambioItem("dtmFechaFinal")
        End Set
    End Property

    Private _strTipoProducto As String
    Public Property strTipoProducto() As String
        Get
            Return _strTipoProducto
        End Get
        Set(ByVal value As String)
            _strTipoProducto = value
            MyBase.CambioItem("strTipoProducto")
        End Set
    End Property

    Private _dblTotalVentas As Double
    Public Property dblTotalVentas() As Double
        Get
            Return _dblTotalVentas
        End Get
        Set(ByVal value As Double)
            _dblTotalVentas = value
            MyBase.CambioItem("dblTotalVentas")
        End Set
    End Property

    Private _dblTotalCompras As Double
    Public Property dblTotalCompras() As Double
        Get
            Return _dblTotalCompras
        End Get
        Set(ByVal value As Double)
            _dblTotalCompras = value
            MyBase.CambioItem("dblTotalCompras")
        End Set
    End Property

    Private _dblTotalUtilidad As Double
    Public Property dblTotalUtilidad() As Double
        Get
            Return _dblTotalUtilidad
        End Get
        Set(ByVal value As Double)
            _dblTotalUtilidad = value
            MyBase.CambioItem("dblTotalUtilidad")
        End Set
    End Property

#End Region

#Region "Metodos"

    ''' <summary>
    ''' LLama el proxy para la consulta del a utilidad del portafolio por trader
    ''' </summary>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Public Sub ConsultarRentabilidadPorTrader()
        Try
            ListaPortafolio = New List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
            dblTotalVentas = 0
            dblTotalCompras = 0
            dblTotalUtilidad = 0

            If String.IsNullOrEmpty(_strReceptor) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un trader", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _logIncluirFechas = True Then

                If _dtmFechaFinal.Date < _dtmFechaInicial.Date Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha final debe ser mayor o igual a la fecha inicial", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If _dtmFechaInicial.Year < 1753 Or _dtmFechaFinal.Year < 1753 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

            End If

            logLlamadoAutomatico = False
            If Not IsNothing(dcProxy.DatosPortafolioPPropias) Then
                dcProxy.DatosPortafolioPPropias.Clear()
            End If
            IsBusy = True
            dcProxy.Load(dcProxy.PortafolioPosicionPropiaRentabilidadQuery(_strReceptor, _intEmisor, _intMesa, _strNemotecnico, _logIncluirFechas, IIf(_logIncluirFechas = True, _dtmFechaInicial, Date.Now) _
                                                                            , IIf(_logIncluirFechas = True, _dtmFechaFinal, Date.Now), _strTipoProducto, Program.Usuario, Program.HashConexion), AddressOf TerminoTraesPortafolioRentabilidad, "CONSULTA")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al iniciar el llamado para consultar la rentabilidad del portafolio por trader.", Me.ToString(), "ConsultarRentabilidadPorTrader", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarForma()
        Try
            ListaPortafolio = New List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
            strReceptor = String.Empty
            strNombreReceptor = String.Empty
            intEmisor = Nothing
            strEmisor = String.Empty
            intMesa = Nothing
            strNemotecnico = String.Empty
            strEspecie = String.Empty
            logIncluirFechas = False
            dtmFechaInicial = Date.Now
            dtmFechaFinal = Date.Now
            dblTotalVentas = Nothing
            dblTotalCompras = Nothing
            dblTotalUtilidad = Nothing
            strTipoProducto = String.Empty

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al limpiar la forma.", Me.ToString(), "LimpiarForma", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ExportarInformacion()
        Try
            If String.IsNullOrEmpty(_strReceptor) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un trader", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _logIncluirFechas = True Then

                If _dtmFechaFinal.Date < _dtmFechaInicial.Date Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha final debe ser mayor o igual a la fecha inicial", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If _dtmFechaInicial.Year < 1753 Or _dtmFechaFinal.Year < 1753 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

            End If

            IsBusy = True

            Dim strParametrosEnviar As String = String.Empty
            Dim strNombreArchivo = String.Format("PortafolioPP_{0:yyyyMMddHHmmss}", Now)

            strParametrosEnviar = String.Format("[IDRECEPTOR]={0}", strReceptor)
            strParametrosEnviar = String.Format("{0}|[EMISOR]={1}", strParametrosEnviar, intEmisor)
            strParametrosEnviar = String.Format("{0}|[MESA]={1}", strParametrosEnviar, intMesa)
            strParametrosEnviar = String.Format("{0}|[ESPECIE]={1}", strParametrosEnviar, strEspecie)
            strParametrosEnviar = String.Format("{0}|[INCLUIRFECHA]={1}", strParametrosEnviar, IIf(logIncluirFechas, 1, 0))
            strParametrosEnviar = String.Format("{0}|[FECHAINICIAL]={1}", strParametrosEnviar, dtmFechaInicial.ToString("yyyy-MM-dd"))
            strParametrosEnviar = String.Format("{0}|[FECHAFINAL]={1}", strParametrosEnviar, dtmFechaFinal.ToString("yyyy-MM-dd"))
            strParametrosEnviar = String.Format("{0}|[TIPOPRODUCTO]={1}", strParametrosEnviar, strTipoProducto)
            strParametrosEnviar = String.Format("{0}|[USUARIO]={1}", strParametrosEnviar, Program.Usuario)

            If Not IsNothing(mdcProxyUtilidad01.GenerarArchivosPlanos) Then
                mdcProxyUtilidad01.GenerarArchivosPlanos.Clear()
            End If

            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.GenerarArchivoPlanoQuery("GENERARRENTABILIDADPP", "GENERARRENTABILIDADPP", strParametrosEnviar, strNombreArchivo, "Operaciones", "EXCELVIEJO", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarArchivoPlano, "GENERARRENTABILIDADPP")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "ExportarInformacion", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoGenerarArchivoPlano(ByVal lo As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.First.Exitoso Then
                        Program.VisorArchivosWeb_DescargarURL(lo.Entities.First.RutaArchivoPlano)
                    Else
                        mostrarMensaje(lo.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "TerminoGenerarArchivoPlanoFondos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "TerminoGenerarArchivoPlanoFondos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

#End Region

#Region "Resultados Asincronicos"

    ''' <summary>
    ''' Recibe el resultado de la consulta de la rentabilidad del portafolio de posición propia
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Private Sub TerminoTraesPortafolioRentabilidad(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia))
        Try
            IsBusy = False
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar la rentabilidad del portafolio de posición propia.", Me.ToString(), "TerminoTraesPortafolioRentabilidad", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            Else
                If dcProxy.DatosPortafolioPPropias.Count > 0 Then
                    ListaPortafolio = dcProxy.DatosPortafolioPPropias.ToList
                    dblTotalVentas = _ListaPortafolio.FirstOrDefault.TotalVentas
                    dblTotalCompras = _ListaPortafolio.FirstOrDefault.TotalCompras
                    dblTotalUtilidad = _ListaPortafolio.FirstOrDefault.TotalUtilidad
                Else
                    dblTotalVentas = 0
                    dblTotalCompras = 0
                    dblTotalUtilidad = 0
                    If logLlamadoAutomatico = False Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar la rentabilidad del portafolio de posición propia.", Me.ToString(), "TerminoTraesPortafolioRentabilidad", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

#End Region

#Region "Notificaciones"

    Private Const TIPOMENSAJE_TICKET = "OYD_LIQUIDACIONES_TICKETS"

    ''' <summary>
    ''' Método que recibe una notificaión
    ''' </summary>
    ''' <param name="pobjInfoNotificacion"></param>
    ''' <remarks>Santiago Vergara - Mayo 27/2014</remarks>
    Public Overrides Sub LlegoNotificacion(pobjInfoNotificacion As A2.Notificaciones.Cliente.clsNotificacion)
        Try

            Dim lstNotificacion As List(Of clsNotificacionTicket)

            If Not String.IsNullOrEmpty(pobjInfoNotificacion.strTipoMensaje) Then

                If pobjInfoNotificacion.strTipoMensaje.ToUpper = TIPOMENSAJE_TICKET Then

                    If Not IsNothing(pobjInfoNotificacion.strInfoMensaje) Then

                        lstNotificacion = New List(Of clsNotificacionTicket)

                        lstNotificacion = clsNotificacionTicket.DeserializeLista(pobjInfoNotificacion.strInfoMensaje)

                        If (From lo In lstNotificacion
                            Where (lo.strIDReceptor = _strReceptor Or lo.strIDReceptorB = _strReceptor) And
                            (_intEmisor = 0 Or lo.intIdEmisor = _intEmisor) And
                            (String.IsNullOrEmpty(_strNemotecnico) Or lo.strEspecie = _strNemotecnico) And
                            (_logIncluirFechas = False Or (lo.dtmCumplimiento >= _dtmFechaInicial And lo.dtmCumplimiento <= _dtmFechaFinal)) Select lo).Count > 0 Then

                            logLlamadoAutomatico = True
                            If Not IsNothing(dcProxy.DatosPortafolioPPropias) Then
                                dcProxy.DatosPortafolioPPropias.Clear()
                            End If
                            IsBusy = True
                            dcProxy.Load(dcProxy.PortafolioPosicionPropiaRentabilidadQuery(_strReceptor, _intEmisor, _intMesa, _strNemotecnico, _logIncluirFechas, IIf(_logIncluirFechas = True, _dtmFechaInicial, Date.Now) _
                                                                                            , IIf(_logIncluirFechas = True, _dtmFechaFinal, Date.Now), _strTipoProducto, Program.Usuario, Program.HashConexion), AddressOf TerminoTraesPortafolioRentabilidad, "CONSULTA")
                        End If

                    End If

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el mensaje de la notificación.", _
                                Me.ToString(), "LlegoNotificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class