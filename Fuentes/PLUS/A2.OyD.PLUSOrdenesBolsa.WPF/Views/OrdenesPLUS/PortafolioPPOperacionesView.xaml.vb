Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel
Imports System.Web

Imports System.Windows.Data
Imports OpenRiaServices.DomainServices.Client

Partial Public Class PortafolioPPOperacionesView
    Inherits Window
    Implements INotifyPropertyChanged

    Private dcProxy As OYDPLUSOrdenesBolsaDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext

    Dim logEsPortafolioOperaciones As Boolean = True
    Dim logEsDiaActual As Boolean = False
    Dim strIDReceptor As String = String.Empty
    Dim strTipoProducto As String = String.Empty
    Dim logTodosLosClientes As Boolean = False
    Dim strIDCliente As String = String.Empty
    Dim dtmFecha As DateTime = Nothing
    Dim strEspecie As String = String.Empty
    Dim dtmFechaEmision As Nullable(Of DateTime)
    Dim dtmFechaVencimiento As Nullable(Of DateTime)
    Dim strModalidad As String = String.Empty
    Dim strIndicador As String = String.Empty
    Dim dblTasaOpuntos As Nullable(Of Double)

    Dim intEmisor As Integer = Nothing
    Dim intMesa As Integer = Nothing
    Dim logIncluirFechas As Boolean = False
    Dim dtmFechaInicial As DateTime = Nothing
    Dim dtmFechaFinal As DateTime = Nothing


    Public Sub New(ByVal pstrIDReceptor As String,
                   ByVal pstrTipoProducto As String,
                   ByVal plogTodosLosClientes As Boolean,
                   ByVal pstrIDCliente As String,
                   ByVal plogEsDiaActual As Boolean,
                   ByVal pdtmFecha As DateTime,
                   ByVal pstrEspecie As String,
                   ByVal pdtmFechaEmision As Nullable(Of DateTime),
                   ByVal pdtmFechaVencimiento As Nullable(Of DateTime),
                   ByVal pstrModalidad As String,
                   ByVal pstrIndicador As String,
                   ByVal pdblTasaOpuntos As Nullable(Of Double)
                   )
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = Me
            logEsPortafolioOperaciones = True
            strIDReceptor = pstrIDReceptor
            strTipoProducto = pstrTipoProducto
            logTodosLosClientes = plogTodosLosClientes
            strIDCliente = pstrIDCliente
            logEsDiaActual = plogEsDiaActual
            dtmFecha = pdtmFecha
            strEspecie = pstrEspecie
            dtmFechaEmision = pdtmFechaEmision
            dtmFechaVencimiento = pdtmFechaVencimiento
            strModalidad = pstrModalidad
            strIndicador = pstrIndicador
            dblTasaOpuntos = pdblTasaOpuntos
            InitializeComponent()

            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            dgOperaciones.Width = Application.Current.MainWindow.ActualWidth * 0.9
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "PortafolioPPOperacionesView", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub New(ByVal pstrIDReceptor As String,
                   ByVal pintEmisor As Integer,
                   ByVal pintMesa As Integer,
                   ByVal pstrEspecie As String,
                   ByVal plogIncluirFechas As Boolean,
                   ByVal pdtmFechaInicial As DateTime,
                   ByVal pdtmFechaFinal As DateTime,
                   ByVal pstrTipoProducto As String,
                   ByVal pdtmFechaEmision As Nullable(Of DateTime),
                   ByVal pdtmFechaVencimiento As Nullable(Of DateTime),
                   ByVal pstrModalidad As String,
                   ByVal pstrIndicador As String,
                   ByVal pdblTasaOpuntos As Nullable(Of Double)
                   )
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = Me
            logEsPortafolioOperaciones = False
            strIDReceptor = pstrIDReceptor
            intEmisor = pintEmisor
            intMesa = pintMesa
            strEspecie = pstrEspecie
            logIncluirFechas = plogIncluirFechas
            dtmFechaInicial = pdtmFechaInicial
            dtmFechaFinal = pdtmFechaFinal
            strTipoProducto = pstrTipoProducto
            dtmFechaEmision = pdtmFechaEmision
            dtmFechaVencimiento = pdtmFechaVencimiento
            strModalidad = pstrModalidad
            strIndicador = pstrIndicador
            dblTasaOpuntos = pdblTasaOpuntos

            InitializeComponent()
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            dgOperaciones.Width = Application.Current.MainWindow.ActualWidth * 0.9
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "PortafolioPPOperacionesView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        dgOperaciones.Width = Application.Current.MainWindow.ActualWidth * 0.9
    End Sub

    Private Sub PortafolioPPOperacionesView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            IsBusyOperaciones = True

            If Not IsNothing(dcProxy.OperacionesPPropias) Then
                dcProxy.OperacionesPPropias.Clear()
            End If

            If logEsPortafolioOperaciones Then
                dcProxy.Load(dcProxy.PortafolioPosicionPropiaOperacionesQuery(strIDReceptor, strTipoProducto, logTodosLosClientes, strIDCliente, _
                                                                              dtmFecha, Program.Usuario, logEsDiaActual, strEspecie, dtmFechaEmision, _
                                                                              dtmFechaVencimiento, strModalidad, strIndicador, dblTasaOpuntos, Program.HashConexion),
                             AddressOf TerminoTraerOperaciones, "OPERACIONESPORTAFOLIOPP")
            Else
                dcProxy.Load(dcProxy.PortafolioPosicionPropiaRentabilidadOperacionesQuery(strIDReceptor, intEmisor, intMesa, strEspecie, logIncluirFechas, _
                                                                                          dtmFechaInicial, dtmFechaFinal, strTipoProducto, Program.Usuario,
                                                                                          dtmFechaEmision, dtmFechaVencimiento, strModalidad, strIndicador, dblTasaOpuntos, Program.HashConexion),
                             AddressOf TerminoTraerOperaciones, "OPERACIONESRENTABILIDADPP")
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de operaciones posición propia", Me.Name, "", "PortafolioPPOperacionesView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerOperaciones(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.OperacionesPPropia))
        Try

            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar la rentabilidad del portafolio de posición propia.", Me.ToString(), "TerminoTraesPortafolioRentabilidad", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            Else
                ListaOperaciones = lo.Entities.ToList
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar la rentabilidad del portafolio de posición propia.", Me.ToString(), "TerminoTraesPortafolioRentabilidad", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusyOperaciones = False
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

    Public Sub ExportarInformacion()
        Try
            IsBusyOperaciones = True

            If Not IsNothing(mdcProxyUtilidad.GenerarArchivosPlanos) Then
                mdcProxyUtilidad.GenerarArchivosPlanos.Clear()
            End If

            If logEsPortafolioOperaciones Then
                Dim strParametrosEnviar As String = String.Empty
                Dim strNombreArchivo = String.Format("PortafolioPP_{0:yyyyMMddHHmmss}", Now)

                strParametrosEnviar = String.Format("[IDRECEPTOR]={0}", strIDReceptor)
                strParametrosEnviar = String.Format("{0}|[TIPOPRODUCTO]={1}", strParametrosEnviar, strTipoProducto)
                strParametrosEnviar = String.Format("{0}|[TODOSLOSCLIENTES]={1}", strParametrosEnviar, IIf(logTodosLosClientes, 1, 0))
                strParametrosEnviar = String.Format("{0}|[IDCLIENTE]={1}", strParametrosEnviar, strIDCliente)
                strParametrosEnviar = String.Format("{0}|[DIAACTUAL]={1}", strParametrosEnviar, logEsDiaActual)
                strParametrosEnviar = String.Format("{0}|[FECHA]={1}", strParametrosEnviar, dtmFecha.ToString("yyyy-MM-dd"))
                strParametrosEnviar = String.Format("{0}|[USUARIO]={1}", strParametrosEnviar, Program.Usuario)
                strParametrosEnviar = String.Format("{0}|[ESPECIEOPERACION]={1}", strParametrosEnviar, strEspecie)

                mdcProxyUtilidad.Load(mdcProxyUtilidad.GenerarArchivoPlanoQuery("GENERARPORTAFOLIOPP", "GENERARPORTAFOLIOPP", strParametrosEnviar, strNombreArchivo, "Operaciones", "EXCELVIEJO", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarArchivoPlano, "PORTAFOLIOPP")
            Else
                Dim strParametrosEnviar As String = String.Empty
                Dim strNombreArchivo = String.Format("PortafolioPP_{0:yyyyMMddHHmmss}", Now)

                strParametrosEnviar = String.Format("[IDRECEPTOR]={0}", strIDReceptor)
                strParametrosEnviar = String.Format("{0}|[EMISOR]={1}", strParametrosEnviar, intEmisor)
                strParametrosEnviar = String.Format("{0}|[MESA]={1}", strParametrosEnviar, intMesa)
                strParametrosEnviar = String.Format("{0}|[ESPECIE]={1}", strParametrosEnviar, strEspecie)
                strParametrosEnviar = String.Format("{0}|[INCLUIRFECHA]={1}", strParametrosEnviar, IIf(logIncluirFechas, 1, 0))
                strParametrosEnviar = String.Format("{0}|[FECHAINICIAL]={1}", strParametrosEnviar, dtmFechaInicial.ToString("yyyy-MM-dd"))
                strParametrosEnviar = String.Format("{0}|[FECHAFINAL]={1}", strParametrosEnviar, dtmFechaFinal.ToString("yyyy-MM-dd"))
                strParametrosEnviar = String.Format("{0}|[TIPOPRODUCTO]={1}", strParametrosEnviar, strTipoProducto)
                strParametrosEnviar = String.Format("{0}|[USUARIO]={1}", strParametrosEnviar, Program.Usuario)

                mdcProxyUtilidad.Load(mdcProxyUtilidad.GenerarArchivoPlanoQuery("GENERARRENTABILIDADPP", "GENERARRENTABILIDADPP", strParametrosEnviar, strNombreArchivo, "Operaciones", "EXCELVIEJO", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarArchivoPlano, "GENERARRENTABILIDADPP")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "ExportarInformacion", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyOperaciones = False
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

        IsBusyOperaciones = False
    End Sub

    Private Sub btnExportarExcel_Click(sender As Object, e As RoutedEventArgs)
        ExportarInformacion()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

#Region "Propiedades"

    Private _IsBusyOperaciones As String
    Public Property IsBusyOperaciones() As String
        Get
            Return _IsBusyOperaciones
        End Get
        Set(ByVal value As String)
            _IsBusyOperaciones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusyOperaciones"))
        End Set
    End Property

    Private _ListaOperaciones As List(Of OyDPLUSOrdenesBolsa.OperacionesPPropia)
    Public Property ListaOperaciones() As List(Of OyDPLUSOrdenesBolsa.OperacionesPPropia)
        Get
            Return _ListaOperaciones
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.OperacionesPPropia))
            _ListaOperaciones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOperaciones"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOperacionesPaged"))
        End Set
    End Property

    Public ReadOnly Property ListaOperacionesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOperaciones) Then
                Dim view = New PagedCollectionView(_ListaOperaciones)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property


#End Region

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged


End Class
