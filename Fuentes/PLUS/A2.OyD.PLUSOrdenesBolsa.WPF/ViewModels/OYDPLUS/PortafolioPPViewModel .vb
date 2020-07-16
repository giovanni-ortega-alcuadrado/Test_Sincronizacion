Imports Telerik.Windows.Controls
'Desarrollado Por:      Santiago Alexander Vergara Orrego
'Descripcion:           Desarrollo Av-Ticket - Forma para mostrar el portafolio de posición propia agrupado por especie
'Fecha:                 Febrero 25/2013
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

Public Class PortafolioPPViewModel
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
                dcProxy1 = New OYDPLUSOrdenesBolsaDomainContext()
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad03 = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad03 = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxy1.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad01.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad03.DomainClient, WebDomainClient(Of OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If Not Program.IsDesignMode() Then
                'IsBusy = True
                'mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarReceptoresUsuarioQuery(True, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTodosReceptoresUsuario, "CONSULTA")
                logTodosLosClientes = True
                dtmFecha = Date.Now
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", Me.ToString(), "PortafolioPPViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Variables"

    Private dcProxy As OYDPLUSOrdenesBolsaDomainContext
    Private dcProxy1 As OYDPLUSOrdenesBolsaDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad03 As OyDPLUSutilidadesDomainContext


#End Region

#Region "Propiedades"

    Private _PortafolioDiaActualSelected As OyDPLUSOrdenesBolsa.DatosPortafolioPPropia
    Public Property PortafolioDiaActualSelected() As OyDPLUSOrdenesBolsa.DatosPortafolioPPropia
        Get
            Return _PortafolioDiaActualSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
            _PortafolioDiaActualSelected = value
            MyBase.CambioItem("PortafolioDiaActualSelected")
        End Set
    End Property


    Private _ListaPortafolioDiaActual As List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
    Public Property ListaPortafolioDiaActual() As List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
        Get
            Return _ListaPortafolioDiaActual
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia))
            _ListaPortafolioDiaActual = value

            MyBase.CambioItem("ListaPortafolioDiaActual")
            MyBase.CambioItem("ListaPortafolioDiaActualPaged")
            If Not IsNothing(_ListaPortafolioDiaActual) Then
                PortafolioDiaActualSelected = _ListaPortafolioDiaActual.FirstOrDefault
            End If
        End Set
    End Property

    Public ReadOnly Property ListaPortafolioDiaActualPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPortafolioDiaActual) Then
                Dim view = New PagedCollectionView(_ListaPortafolioDiaActual)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaPortafolioOtrosDias As List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
    Public Property ListaPortafolioOtrosDias() As List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
        Get
            Return _ListaPortafolioOtrosDias
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia))
            _ListaPortafolioOtrosDias = value
            MyBase.CambioItem("ListaPortafolioOtrosDias")
            MyBase.CambioItem("ListaPortafolioOtrosDiasPaged")
        End Set
    End Property

    Public ReadOnly Property ListaPortafolioOtrosDiasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPortafolioOtrosDias) Then
                Dim view = New PagedCollectionView(_ListaPortafolioOtrosDias)
                Return view
            Else
                Return Nothing
            End If
        End Get
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

    Private _listaTipoProducto As New List(Of OYDUtilidades.ItemCombo)
    Public Property listaTipoProducto() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listaTipoProducto
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaTipoProducto = value
            MyBase.CambioItem("listaTipoProducto")
        End Set
    End Property

    Private _strReceptor As String
    Public Property strReceptor() As String
        Get
            Return _strReceptor
        End Get
        Set(ByVal value As String)
            _strReceptor = value

            If Not String.IsNullOrEmpty(value) Then
                ConsultarTiposProductoPorReceptor()
                LimpiarCliente()
            End If

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

    Private _strTipoProducto As String
    Public Property strTipoProducto() As String
        Get
            Return _strTipoProducto
        End Get
        Set(ByVal value As String)
            _strTipoProducto = value
            LimpiarCliente()
            MyBase.CambioItem("strTipoProducto")
        End Set
    End Property

    Private _strCliente As String
    Public Property strCliente() As String
        Get
            Return _strCliente
        End Get
        Set(ByVal value As String)
            _strCliente = value
            MyBase.CambioItem("strCliente")
        End Set
    End Property

    Private _strNombreCliente As String
    Public Property strNombreCliente() As String
        Get
            Return _strNombreCliente
        End Get
        Set(ByVal value As String)
            _strNombreCliente = value
            MyBase.CambioItem("strNombreCliente")
        End Set
    End Property

    Private _logTodosLosClientes As Boolean
    Public Property logTodosLosClientes() As Boolean
        Get
            Return _logTodosLosClientes
        End Get
        Set(ByVal value As Boolean)
            _logTodosLosClientes = value
            If value = True Then
                logHabilitarBuscador = False
                LimpiarCliente()
            Else
                logHabilitarBuscador = True
            End If
            MyBase.CambioItem("logTodosLosClientes")
        End Set
    End Property

    Private _logHabilitarBuscador As Boolean = False
    Public Property logHabilitarBuscador() As Boolean
        Get
            Return _logHabilitarBuscador
        End Get
        Set(ByVal value As Boolean)
            _logHabilitarBuscador = value
            MyBase.CambioItem("logHabilitarBuscador")

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

    Private _dtmFecha As DateTime
    Public Property dtmFecha() As DateTime
        Get
            Return _dtmFecha
        End Get
        Set(ByVal value As DateTime)
            _dtmFecha = value
            MyBase.CambioItem("dtmFecha")
        End Set
    End Property


#End Region

#Region "Metodos"

    Public Sub LimpiarCliente()
        If Not String.IsNullOrEmpty(_strCliente) Then
            strCliente = String.Empty
            strNombreCliente = String.Empty
            BorrarCliente = True
            BorrarCliente = False
        End If
    End Sub

    ''' <summary>
    ''' Consulta los tipos de producto de posición propia asociados a un receptor
    ''' </summary>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Public Sub ConsultarTiposProductoPorReceptor()
        Try
            If Not IsNothing(mdcProxyUtilidad01.ItemCombos) Then
                mdcProxyUtilidad01.ItemCombos.Clear()
            End If
            IsBusy = True
            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.cargarCombosCondicionalQuery("TIPO_PRODUCTO_PP_CONSULTA", _strReceptor, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "TIPO_PRODUCTO_PP_CONSULTA")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llamar la consulta de tipos de producto por receptor", Me.ToString(), "ConsultarTiposProductoPorReceptor", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Se llama desde el botón de la vista para que ejecute las consultas de portafolios
    ''' </summary>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Public Sub ConsultarPortafolio()
        Try

            ListaPortafolioDiaActual = New List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
            ListaPortafolioOtrosDias = New List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)

            If String.IsNullOrEmpty(_strReceptor) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un receptor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_strTipoProducto) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el tipo de producto", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _logTodosLosClientes = False And String.IsNullOrEmpty(_strCliente) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un cliente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            ConsultarPortafolioDiaActual()
            ConsultarPortafolioOtrosDias()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al iniciar el llamado para consultar el portafolio.", Me.ToString(), "ConsultarPortafolio", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' LLama el proxy para la consulta del portafolio del dia actual
    ''' </summary>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Public Sub ConsultarPortafolioDiaActual()
        Try
            If Not IsNothing(dcProxy.DatosPortafolioPPropias) Then
                dcProxy.DatosPortafolioPPropias.Clear()
            End If
            IsBusy = True
            dcProxy.Load(dcProxy.PortafolioPosicionPropiaDiaActualQuery(_strReceptor, _strTipoProducto, _logTodosLosClientes, _strCliente, _dtmFecha, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPortafolioDiaActual, "CONSULTA")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al iniciar el llamado para consultar el portafolio del dia actual.", Me.ToString(), "ConsultarPortafolioDiaActual", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' LLama el proxy para la consulta del portafolio de dias siguientes
    ''' </summary>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Public Sub ConsultarPortafolioOtrosDias()
        Try
            If Not IsNothing(dcProxy.DatosPortafolioPPropias) Then
                dcProxy1.DatosPortafolioPPropias.Clear()
            End If
            IsBusy = True
            dcProxy1.Load(dcProxy1.PortafolioPosicionPropiaOtrosDiasQuery(_strReceptor, _strTipoProducto, _logTodosLosClientes, _strCliente, _dtmFecha, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPortafolioOtrosDias, "CONSULTA")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al iniciar el llamado para consultar el portafolio de otros dias.", Me.ToString(), "ConsultarPortafolioOtrosDias", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarForma()
        Try
            ListaPortafolioDiaActual = New List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
            ListaPortafolioOtrosDias = New List(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia)
            strReceptor = String.Empty
            strNombreReceptor = String.Empty
            strTipoProducto = String.Empty
            LimpiarCliente()
            logTodosLosClientes = True
            listaTipoProducto = New List(Of OYDUtilidades.ItemCombo)
            dtmFecha = Date.Now
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al limpiar la forma.", Me.ToString(), "LimpiarForma", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ExportarInformacion()
        Try
            If String.IsNullOrEmpty(_strReceptor) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un receptor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_strTipoProducto) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el tipo de producto", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _logTodosLosClientes = False And String.IsNullOrEmpty(_strCliente) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un cliente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            Dim strParametrosEnviar As String = String.Empty
            Dim strNombreArchivo = String.Format("PortafolioPP_{0:yyyyMMddHHmmss}", Now)

            strParametrosEnviar = String.Format("[IDRECEPTOR]={0}", strReceptor)
            strParametrosEnviar = String.Format("{0}|[TIPOPRODUCTO]={1}", strParametrosEnviar, strTipoProducto)
            strParametrosEnviar = String.Format("{0}|[TODOSLOSCLIENTES]={1}", strParametrosEnviar, IIf(logTodosLosClientes, 1, 0))
            strParametrosEnviar = String.Format("{0}|[IDCLIENTE]={1}", strParametrosEnviar, strCliente)
            strParametrosEnviar = String.Format("{0}|[DIAACTUAL]={1}", strParametrosEnviar, "1")
            strParametrosEnviar = String.Format("{0}|[FECHA]={1}", strParametrosEnviar, dtmFecha.ToString("yyyy-MM-dd"))
            strParametrosEnviar = String.Format("{0}|[USUARIO]={1}", strParametrosEnviar, Program.Usuario)
            strParametrosEnviar = String.Format("{0}|[ESPECIEOPERACION]={1}", strParametrosEnviar, "TODOS")

            If Not IsNothing(mdcProxyUtilidad01.GenerarArchivosPlanos) Then
                mdcProxyUtilidad01.GenerarArchivosPlanos.Clear()
            End If

            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.GenerarArchivoPlanoQuery("GENERARPORTAFOLIOPP", "GENERARPORTAFOLIOPP", strParametrosEnviar, strNombreArchivo, "Operaciones", "EXCELVIEJO", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarArchivoPlano, "PORTAFOLIOPP")
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
    ''' Recibe el resultado de la consulta de receptores segun el usuario logueado en el sistema
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Private Sub TerminoConsultarTodosReceptoresUsuario(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblReceptoresUsuario))
        Try
            IsBusy = False
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaReceptoresUsuario = lo.Entities.ToList
                    If (From c In _ListaReceptoresUsuario Where c.Prioridad = 0 Select c).Count > 0 Then
                        strReceptor = (From c In _ListaReceptoresUsuario Where c.Prioridad = 0 Select c).FirstOrDefault.CodigoReceptor
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Recibe el resultado de las consultas de combos
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))

        Try
            IsBusy = False
            If Not lo.HasError Then

                Select Case lo.UserState
                    Case "TIPO_PRODUCTO_PP_CONSULTA"
                        listaTipoProducto = mdcProxyUtilidad01.ItemCombos.ToList

                        If (From c In _listaTipoProducto Where c.intID = 0 Select c).Count > 0 Then
                            strTipoProducto = (From c In _listaTipoProducto Where c.intID = 0 Select c).FirstOrDefault.ID
                        End If

                End Select

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                     Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                       Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Recibe el resultado de la consulta del portafolio del dia actual
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Private Sub TerminoConsultarPortafolioDiaActual(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia))
        Try
            IsBusy = False

            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar el portafolio del dia actual.", Me.ToString(), "TerminoConsultarPortafolioDiaActual", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            Else
                If dcProxy.DatosPortafolioPPropias.Count > 0 Then
                    ListaPortafolioDiaActual = dcProxy.DatosPortafolioPPropias.ToList
                End If

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar el portafolio del dia actual.", Me.ToString(), "TerminoConsultarPortafolioDiaActual", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Recibe el resultado de la consulta de portafolio de dias siguientes 
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara -  Febrero 28/2014</remarks>
    Private Sub TerminoConsultarPortafolioOtrosDias(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.DatosPortafolioPPropia))
        Try
            IsBusy = False
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar el portafolio de otros dias.", Me.ToString(), "TerminoConsultarPortafolioOtrosDias", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            Else
                If dcProxy1.DatosPortafolioPPropias.Count > 0 Then
                    ListaPortafolioOtrosDias = dcProxy1.DatosPortafolioPPropias.ToList
                End If

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar el portafolio de otros dias.", Me.ToString(), "TerminoConsultarPortafolioOtrosDias", Program.TituloSistema, Program.Maquina, ex)
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
                            lo.strTipoProducto = _strTipoProducto And
                            (_logTodosLosClientes = True Or lo.strIDCliente = strCliente) Select lo).Count > 0 Then

                            ConsultarPortafolioDiaActual()

                            ConsultarPortafolioOtrosDias()

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