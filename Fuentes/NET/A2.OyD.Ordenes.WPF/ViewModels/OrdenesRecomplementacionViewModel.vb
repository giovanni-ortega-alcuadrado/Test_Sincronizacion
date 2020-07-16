Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.IO
Imports System.Reflection
Imports System.Reflection.Emit
Imports A2Utilidades



''' <summary>
''' ViewModel de la forma de recomplementación de órdenes.
''' </summary>
''' <remarks></remarks>
Public Class OrdenesRecomplementacionViewModel
    Implements INotifyPropertyChanged


#Region "Eventos"



    ''' <summary>
    ''' informa a la vista el cambio de alguna de las propiedades del ViewModel
    ''' </summary>
    ''' <param name="sender">Objeto que dispara el cambio</param>
    ''' <param name="e">parámetros del evento</param>
    ''' <remarks></remarks>
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

#End Region

#Region "Variables"

    Private strAgrupacionBusquedaCliente As String = "idcomitentelectura"
    Private _IsBusy As Boolean = False
    Private _BusyContent As String = "Cargando..."

    Private _lngIdOrdenAno As Nullable(Of Integer) = Now.Year
    Private _lngIdOrden As Nullable(Of Integer)
    Private _intVersion As Nullable(Of Integer) = 0
    Private _ListaOrdenes As EntitySet(Of OyDOrdenes.Orden)
    Private _ListaLiquidaciones As List(Of OyDBolsa.Liquidacione)
    Private _ListaDistribucion As EntitySet(Of OyDOrdenes.tblDistribucion)
    Private _ListaValidacion As EntitySet(Of OyDOrdenes.tblValidacion)
    Private _ListaDistribucionCliente As List(Of Object)
    Private _ListaDistribucionPrecio As List(Of Object)
    Private _ListaValidacionProceso As EntitySet(Of OyDOrdenes.tblValidacionProceso)
    Private _ListaNombresDistribucionCliente As List(Of String)
    Private _ListaNombresDistribucionPrecio As List(Of String)
    Private _strRutaArchivo As String


    Private _IsVisiblePantallaCarga As Visibility = Visibility.Visible
    Private _IsVisibleGridDistribucion As Visibility = Visibility.Collapsed
    Private _IsVisiblePantallaCalculo As Visibility = Visibility.Collapsed
    Private _IsVisibleCargaArchivo As Visibility = Visibility.Collapsed
    Private _IsVisibleCalcular As Visibility = Visibility.Collapsed
    Private _IsVisibleValidaciones As Visibility = Visibility.Collapsed
    Private _IsVisibleProcesar As Visibility = Visibility.Collapsed
    Private _IsVisibleNuevoProceso As Visibility = Visibility.Collapsed
    Private _IsVisibleValidacionesProceso As Visibility = Visibility.Collapsed


    Private _dblCantidadTotal As Double
    Private _dblTotalLiquidaciones As Double
    Private _dblPrecioPromedio As Double
    Private _dblTotalDistribucion As Double



    Public STR_PROCESO As String = "RECOMPLEMENTACION"
    Private STR_MSG_CARGA_ERRONEA As String = "El archivo {0} no pudo ser cargado correctamente."
    Private STR_CLASEORDEN As String = "A"
    Private Const MSTR_ACCION_BUSCAR As String = "buscar"
    Private Const MSTR_SEPARADOR As String = "|"


    Private objProxyOrdenes As OrdenesDomainContext
    Private objProxyBolsa As BolsaDomainContext
    Public objProxyRecomplementacion As OrdenesDomainContext
    Private objProxyUtilidades As UtilidadesDomainContext

    Private dtmFechaDefecto As Date = CDate("1899/01/01")
    Public strComplementoRutaServidor As String
    Public strNombreArchivo As String


#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Indica si en la vista se activa el control BusyIndicator
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(value As Boolean)
            _IsBusy = value
            CambioItem("IsBusy")
        End Set
    End Property

    ''' <summary>
    ''' Mensaje que muestra el control BusyIndicator
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Property BusyContent As String
        Get
            Return _BusyContent
        End Get
        Set(value As String)
            _BusyContent = value
            CambioItem("BusyContent")
        End Set
    End Property

    ''' <summary>
    ''' Año de la orden original
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property lngIdOrdenAno As Nullable(Of Integer)
        Get
            Return _lngIdOrdenAno
        End Get
        Set(value As Nullable(Of Integer))
            _lngIdOrdenAno = value
            CambioItem("lngIdOrdenAno")
        End Set
    End Property

    ''' <summary>
    ''' Consecutivo de la orden original
    ''' </summary>
    ''' <value>Nullable(Of Integer)</value>
    ''' <returns>Nullable(Of Integer)</returns>
    ''' <remarks></remarks>
    Public Property lngIdOrden As Nullable(Of Integer)
        Get
            Return _lngIdOrden
        End Get
        Set(value As Nullable(Of Integer))
            _lngIdOrden = value
            CambioItem("lngIdOrden")
        End Set
    End Property

    ''' <summary>
    ''' EntitySet en el que se carga la orden original.
    ''' </summary>
    ''' <value>EntitySet(Of OyDOrdenes.Orden)</value>
    ''' <returns>EntitySet(Of OyDOrdenes.Orden)</returns>
    ''' <remarks></remarks>
    Public Property ListaOrdenes As EntitySet(Of OyDOrdenes.Orden)
        Get
            Return _ListaOrdenes
        End Get
        Set(value As EntitySet(Of OyDOrdenes.Orden))
            _ListaOrdenes = value
            CambioItem("ListaOrdenes")
        End Set
    End Property

    ''' <summary>
    ''' EntitySet que contiene las liquidaciones de la orden original.
    ''' </summary>
    ''' <value>EntitySet(Of OyDBolsa.Liquidacione)</value>
    ''' <returns>EntitySet(Of OyDBolsa.Liquidacione)</returns>
    ''' <remarks></remarks>
    Public Property ListaLiquidaciones As List(Of OyDBolsa.Liquidacione)
        Get
            Return _ListaLiquidaciones
        End Get
        Set(value As List(Of OyDBolsa.Liquidacione))
            _ListaLiquidaciones = value
            CambioItem("ListaLiquidaciones")
        End Set
    End Property

    ''' <summary>
    ''' EntitySet con la distribución que se carga del archivo.
    ''' </summary>
    ''' <value>EntitySet(Of OyD.Ordenes.SL.Web.tblDistribucion)</value>
    ''' <returns>EntitySet(Of OyD.Ordenes.SL.Web.tblDistribucion)</returns>
    ''' <remarks></remarks>
    Public Property ListaDistribucion As EntitySet(Of OyDOrdenes.tblDistribucion)
        Get
            Return _ListaDistribucion
        End Get
        Set(value As EntitySet(Of OyDOrdenes.tblDistribucion))
            _ListaDistribucion = value
            CambioItem("ListaDistribucion")
        End Set
    End Property

    ''' <summary>
    ''' EntitySet con las validaciones de la carga del archivo.
    ''' </summary>
    ''' <value>EntitySet(Of OyD.Ordenes.SL.Web.tblValidacion)</value>
    ''' <returns>EntitySet(Of OyD.Ordenes.SL.Web.tblValidacion)</returns>
    ''' <remarks></remarks>
    Public Property ListaValidacion As EntitySet(Of OyDOrdenes.tblValidacion)
        Get
            Return _ListaValidacion
        End Get
        Set(value As EntitySet(Of OyDOrdenes.tblValidacion))
            _ListaValidacion = value
            CambioItem("ListaValidacion")
        End Set
    End Property

    ''' <summary>
    ''' Lista con la distribución de cantidad por liquidación y cliente realizada por el procedimiento de cálculo.
    ''' </summary>
    ''' <value>List(Of Object)</value>
    ''' <returns>List(Of Object)</returns>
    ''' <remarks></remarks>
    Public Property ListaDistribucionCliente As List(Of Object)
        Get
            Return _ListaDistribucionCliente
        End Get
        Set(value As List(Of Object))
            _ListaDistribucionCliente = value
            CambioItem("ListaDistribucionCliente")
        End Set
    End Property

    ''' <summary>
    ''' Lista con la distribución de total por liquidación y cliente realizada por el procedimiento de cálculo.
    ''' </summary>
    ''' <value>List(Of Object)</value>
    ''' <returns>List(Of Object)</returns>
    ''' <remarks></remarks>
    Public Property ListaDistribucionPrecio As List(Of Object)
        Get
            Return _ListaDistribucionPrecio
        End Get
        Set(value As List(Of Object))
            _ListaDistribucionPrecio = value
            CambioItem("ListaDistribucionPrecio")
        End Set
    End Property

    ''' <summary>
    ''' EntitySet con las validaciones retornadas por el procedimiento de cálculo.
    ''' </summary>
    ''' <value>EntitySet(Of OyD.Ordenes.SL.Web.tblValidacionProceso)</value>
    ''' <returns>EntitySet(Of OyD.Ordenes.SL.Web.tblValidacionProceso)</returns>
    ''' <remarks></remarks>
    Public Property ListaValidacionProceso As EntitySet(Of OyDOrdenes.tblValidacionProceso)
        Get
            Return _ListaValidacionProceso
        End Get
        Set(value As EntitySet(Of OyDOrdenes.tblValidacionProceso))
            _ListaValidacionProceso = value
            CambioItem("ListaValidacionProceso")
        End Set
    End Property

    ''' <summary>
    ''' Lista con los nombres de columnas que muestra el grid de cantidad distribuida por cliente y liquidación.
    ''' </summary>
    ''' <value>List(Of String)</value>
    ''' <returns>List(Of String)</returns>
    ''' <remarks></remarks>
    Public Property ListaNombresDistribucionCliente As List(Of String)
        Get
            Return _ListaNombresDistribucionCliente
        End Get
        Set(value As List(Of String))
            _ListaNombresDistribucionCliente = value
            CambioItem("ListaNombresDistribucionCliente")
        End Set
    End Property

    ''' <summary>
    ''' Lista con los nombres de las columnas que muestra el grid de cálculo del precio promedio por cliente.
    ''' </summary>
    ''' <value>List(Of String)</value>
    ''' <returns>List(Of String)</returns>
    ''' <remarks></remarks>
    Public Property ListaNombresDistribucionPrecio As List(Of String)
        Get
            Return _ListaNombresDistribucionPrecio
        End Get
        Set(value As List(Of String))
            _ListaNombresDistribucionPrecio = value
            CambioItem("ListaNombresDistribucionPrecio")
        End Set
    End Property

    ''' <summary>
    ''' Ruta del archivo que se carga.
    ''' </summary>
    ''' <value>String</value>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Property strRutaArchivo As String
        Get
            Return _strRutaArchivo
        End Get
        Set(value As String)
            _strRutaArchivo = value
            CambioItem("strRutaArchivo")
        End Set
    End Property

    ''' <summary>
    ''' Indica si la primera pantalla de la forma es visible.
    ''' </summary>
    ''' <value>Visibility</value>
    ''' <returns>Visibility</returns>
    ''' <remarks></remarks>
    Public Property IsVisiblePantallaCarga As Visibility
        Get
            Return _IsVisiblePantallaCarga
        End Get
        Set(value As Visibility)
            _IsVisiblePantallaCarga = value
            CambioItem("IsVisiblePantallaCarga")
        End Set
    End Property

    ''' <summary>
    ''' Indica si el grid que muestra la distribución del archivo cargado es visible.
    ''' </summary>
    ''' <value>Visibility</value>
    ''' <returns>Visibility</returns>
    ''' <remarks></remarks>
    Public Property IsVisibleGridDistribucion As Visibility
        Get
            Return _IsVisibleGridDistribucion
        End Get
        Set(value As Visibility)
            _IsVisibleGridDistribucion = value
            CambioItem("IsVisibleGridDistribucion")
        End Set
    End Property

    ''' <summary>
    ''' Indica si la segunda pantalla de la forma es visible.
    ''' </summary>
    ''' <value>Visibility</value>
    ''' <returns>Visibility</returns>
    ''' <remarks></remarks>
    Public Property IsVisiblePantallaCalculo As Visibility
        Get
            Return _IsVisiblePantallaCalculo
        End Get
        Set(value As Visibility)
            _IsVisiblePantallaCalculo = value
            CambioItem("IsVisiblePantallaCalculo")
        End Set
    End Property

    ''' <summary>
    ''' Indica si el control que se encarga de la carga del archivo es visible.
    ''' </summary>
    ''' <value>Visibility</value>
    ''' <returns>Visibility</returns>
    ''' <remarks></remarks>
    Public Property IsVisibleCargaArchivo As Visibility
        Get
            Return _IsVisibleCargaArchivo
        End Get
        Set(value As Visibility)
            _IsVisibleCargaArchivo = value
            CambioItem("IsVisibleCargaArchivo")
        End Set
    End Property

    ''' <summary>
    ''' Indica si el grid que muestra las validaciones de la carga del archivo es visible.
    ''' </summary>
    ''' <value>Visibility</value>
    ''' <returns>Visibility</returns>
    ''' <remarks></remarks>
    Public Property IsVisibleValidaciones As Visibility
        Get
            Return _IsVisibleValidaciones
        End Get
        Set(value As Visibility)
            _IsVisibleValidaciones = value
            CambioItem("IsVisibleValidaciones")
        End Set
    End Property

    ''' <summary>
    ''' Indica si el botón calcular es visible.
    ''' </summary>
    ''' <value>Visibility</value>
    ''' <returns>Visibility</returns>
    ''' <remarks></remarks>
    Public Property IsVisibleCalcular As Visibility
        Get
            Return _IsVisibleCalcular
        End Get
        Set(value As Visibility)
            _IsVisibleCalcular = value
            CambioItem("IsVisibleCalcular")
        End Set
    End Property

    ''' <summary>
    ''' Indica si el botón procesar es visible.
    ''' </summary>
    ''' <value>Visibility</value>
    ''' <returns>Visibility</returns>
    ''' <remarks></remarks>
    Public Property IsVisibleProcesar As Visibility
        Get
            Return _IsVisibleProcesar
        End Get
        Set(value As Visibility)
            _IsVisibleProcesar = value
            CambioItem("IsVisibleProcesar")
        End Set
    End Property

    ''' <summary>
    ''' Indica si el botón nuevo proceso es visible.
    ''' </summary>
    ''' <value>Visibility</value>
    ''' <returns>Visibility</returns>
    ''' <remarks></remarks>
    Public Property IsVisibleNuevoProceso As Visibility
        Get
            Return _IsVisibleNuevoProceso
        End Get
        Set(value As Visibility)
            _IsVisibleNuevoProceso = value
            CambioItem("IsVisibleNuevoProceso")
        End Set
    End Property

    ''' <summary>
    ''' Indica si el grid que muestra la validaciones del cálculo de la distribución es visible.
    ''' </summary>
    ''' <value>Visibility</value>
    ''' <returns>Visibility</returns>
    ''' <remarks></remarks>
    Public Property IsVisibleValidacionesProceso As Visibility
        Get
            Return _IsVisibleValidacionesProceso
        End Get
        Set(value As Visibility)
            _IsVisibleValidacionesProceso = value
            CambioItem("IsVisibleValidacionesProceso")
        End Set
    End Property


    ''' <summary>
    ''' Cantidad total de las liquidaciones asociadas a la orden.
    ''' </summary>
    ''' <value>Double</value>
    ''' <returns>Double</returns>
    ''' <remarks></remarks>
    Public Property dblCantidadTotal As Double
        Get
            Return _dblCantidadTotal
        End Get
        Set(value As Double)
            _dblCantidadTotal = value
            CambioItem("dblCantidadTotal")
        End Set
    End Property

    ''' <summary>
    ''' Total de las liquidaciones asociadas a la orden
    ''' </summary>
    ''' <value>Double</value>
    ''' <returns>Double</returns>
    ''' <remarks></remarks>
    Public Property dblTotalLiquidaciones As Double
        Get
            Return _dblTotalLiquidaciones
        End Get
        Set(value As Double)
            _dblTotalLiquidaciones = value
            CambioItem("dblTotalLiquidaciones")
        End Set
    End Property

    ''' <summary>
    ''' Precio promedio de las liquidaciones asociadas a la orden.
    ''' </summary>
    ''' <value>Double</value>
    ''' <returns>Double</returns>
    ''' <remarks></remarks>
    Public Property dblPrecioPromedio As Double
        Get
            Return _dblPrecioPromedio
        End Get
        Set(value As Double)
            _dblPrecioPromedio = value
            CambioItem("dblPrecioPromedio")
        End Set
    End Property

    ''' <summary>
    ''' Total de las cantidades cargadas en el archivo.
    ''' </summary>
    ''' <value>Double</value>
    ''' <returns>Double</returns>
    ''' <remarks></remarks>
    Public Property dblTotalDistribucion As Double
        Get
            Return _dblTotalDistribucion
        End Get
        Set(value As Double)
            _dblTotalDistribucion = value
            CambioItem("dblTotalDistribucion")
        End Set
    End Property


#End Region

#Region "Inicialización"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Try

            InicializarControles()
            InicializarServicio()
            lngIdOrdenAno = Now.Year
            lngIdOrden = 0

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "OrdenesRecomplementacionViewModel.New", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Estable la configuración inicial de los controles del formulario.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InicializarControles()

        strRutaArchivo = String.Empty
        ListaOrdenes = Nothing
        ListaLiquidaciones = Nothing
        ListaDistribucion = Nothing
        ListaValidacion = Nothing
        ListaDistribucionCliente = Nothing
        ListaDistribucionPrecio = Nothing
        ListaValidacionProceso = Nothing
        IsVisiblePantallaCarga = Visibility.Visible
        IsVisibleCargaArchivo = Visibility.Collapsed
        IsVisibleGridDistribucion = Visibility.Collapsed
        IsVisibleValidaciones = Visibility.Collapsed
        IsVisibleCalcular = Visibility.Collapsed
        IsVisiblePantallaCalculo = Visibility.Collapsed
        IsVisibleProcesar = Visibility.Collapsed
        IsVisibleNuevoProceso = Visibility.Collapsed
        IsVisibleValidacionesProceso = Visibility.Collapsed

    End Sub

    ''' <summary>
    ''' Instancia el canal de comunicación hacia los servicios a utilizar.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InicializarServicio()

     

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxyOrdenes = New OrdenesDomainContext
            objProxyBolsa = New BolsaDomainContext
            objProxyUtilidades = New UtilidadesDomainContext
            objProxyRecomplementacion = New OrdenesDomainContext
            'dcProxy = New OrdenesDomainContext()
            'dcProxy1 = New OrdenesDomainContext()
            'mdcProxyUtilidad01 = New UtilidadesDomainContext()
            'mdcProxyUtilidad02 = New UtilidadesDomainContext()
        Else
            objProxyOrdenes = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxyBolsa = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocioBolsa))
            objProxyUtilidades = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            objProxyRecomplementacion = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            'dcProxy = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            'dcProxy1 = New OrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            'mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            'mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Dispara el proceso de consulta de la orden original.
    ''' </summary>
    ''' <remarks></remarks>
    Sub Consultar()
        Try


            If Not IsNumeric(lngIdOrden) Or lngIdOrden = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La orden debe ser un valor númerico y diferente de cero", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'INVOCA EL PROCEDIMIENTO PARA SUBIR
            If lngIdOrden.HasValue And lngIdOrdenAno.HasValue Then

                IsBusy = True
                BusyContent = "Consultando información de la orden..."

                InicializarControles()

                If ListaOrdenes IsNot Nothing Then
                    ListaOrdenes.Clear()
                End If

                If ListaLiquidaciones IsNot Nothing Then
                    ListaLiquidaciones.Clear()
                End If

                objProxyOrdenes.Ordens.Clear()
                objProxyBolsa.Liquidaciones.Clear()

                Dim strOrdenCompuesta = lngIdOrdenAno.ToString + Format(lngIdOrden, "000000")

                objProxyOrdenes.Load(objProxyOrdenes.OrdenesConsultarQuery(STR_CLASEORDEN, String.Empty, CInt(strOrdenCompuesta), _intVersion, String.Empty, String.Empty, Nothing, String.Empty,
                                               Nothing, String.Empty, String.Empty, String.Empty, String.Empty, Nothing, String.Empty,
                                                Nothing, String.Empty, String.Empty, String.Empty, String.Empty, Program.Usuario(), Program.HashConexion), AddressOf TerminoConsultarOrden, MSTR_ACCION_BUSCAR)
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe digitar una orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If



        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema mientras se consultaba la orden", _
                                Me.ToString(), "OrdenesRecomplementacionViewModel.Consultar", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Ejecuta la lógica que carga la información de la orden original y consulta las liquidaciones asociadas.
    ''' </summary>
    ''' <param name="lo">contiene la información de la orden</param>
    ''' <remarks></remarks>
    Private Sub TerminoConsultarOrden(ByVal lo As LoadOperation(Of OyDOrdenes.Orden))
        Try
            If Not lo.HasError Then

                If objProxyOrdenes.Ordens.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró la orden especificada.", Program.TituloSistema)
                ElseIf objProxyOrdenes.Ordens.Count > 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Se encontraron más de una orden, existe una inconsistencia en el sistema.", Program.TituloSistema)
                Else

                    For Each ord In objProxyOrdenes.Ordens
                        Select Case ord.Tipo.ToUpper
                            Case "V"
                                ord.Tipo = "VENTA"
                            Case "C"
                                ord.Tipo = "COMPRA"
                        End Select
                    Next

                    ListaOrdenes = objProxyOrdenes.Ordens

                    'CONSULTANDO LAS LIQUIDACIONES.
                    BusyContent = "Consultando las liquidaciones de la orden..."

                    If ListaLiquidaciones IsNot Nothing Then
                        ListaLiquidaciones.Clear()
                    End If

                    objProxyBolsa.Liquidaciones.Clear()

                    Dim strOrdenCompuesta = lngIdOrdenAno.ToString + Format(lngIdOrden, "000000")

                    objProxyBolsa.Load(objProxyBolsa.LiquidacionesConsultarQuery(0, ListaOrdenes(0).IDComitente, ListaOrdenes(0).Tipo, STR_CLASEORDEN, dtmFechaDefecto, dtmFechaDefecto, CInt(strOrdenCompuesta), 0, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarLiquidaciones, MSTR_ACCION_BUSCAR)

                    objProxyUtilidades.Load(objProxyUtilidades.buscarClienteEspecificoQuery(ListaOrdenes(0).IDComitente, Program.Usuario, strAgrupacionBusquedaCliente, Program.HashConexion), AddressOf TerminoConsultarCliente, Nothing)


                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de la lista de Ordenes", Me.ToString(), "TerminoConsultarOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes", Me.ToString(), "TerminoConsultarOrden", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta la lógica que carga la información del cliente de la orden.
    ''' </summary>
    ''' <param name="lo">contiene la información del cliente</param>
    ''' <remarks></remarks>
    Private Sub TerminoConsultarCliente(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If Not lo.HasError Then

                If objProxyUtilidades.BuscadorClientes IsNot Nothing AndAlso objProxyUtilidades.BuscadorClientes.Count > 0 Then

                    ListaOrdenes(0).Mercado = objProxyUtilidades.BuscadorClientes(0).NroDocumento

                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de la información del cliente", Me.ToString(), "TerminoConsultarCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la información del cliente", Me.ToString(), "TerminoConsultarCliente", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta la lógica que carga la información de las liquidaciones asociadas de la orden.
    ''' </summary>
    ''' <param name="lo">contiene la información de las liquidaciones.</param>
    ''' <remarks></remarks>
    Private Sub TerminoConsultarLiquidaciones(ByVal lo As LoadOperation(Of OyDBolsa.Liquidacione))
        Try
            If Not lo.HasError Then

                For Each Liq In objProxyBolsa.Liquidaciones
                    Select Case Liq.Tipo.ToUpper
                        Case "C"
                            Liq.Tipo = "COMPRA"
                        Case "V"
                            Liq.Tipo = "VENTA"
                    End Select
                Next

                ListaLiquidaciones = objProxyBolsa.Liquidaciones.OrderByDescending(Function(l) l.Cantidad).ToList

                'CALCULO DE LOS TOTALES.
                dblCantidadTotal = objProxyBolsa.Liquidaciones.Sum(Function(liq) liq.Cantidad)
                dblTotalLiquidaciones = objProxyBolsa.Liquidaciones.Sum(Function(liq) liq.Transaccion_cur)
                dblPrecioPromedio = Math.Round(dblTotalLiquidaciones / dblCantidadTotal, 2)

                IsVisibleCargaArchivo = Visibility.Visible

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para la obtención de la lista de liquidaciones", Me.ToString(), "TerminoConsultarLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de liquidaciones", Me.ToString(), "TerminoConsultarLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Dispara el proceso que carga el archivo con la distribución.
    ''' </summary>
    ''' <remarks></remarks>
    Sub CargarArchivo()
        Try
            Dim strActualParaWeb As String = Program.Usuario.Replace("\", "_").Replace(".", "_").Replace("/", "_")
            BusyContent = "Subiendo archivo"
            IsBusy = True

            objProxyRecomplementacion.tblDistribucions.Clear()
            objProxyRecomplementacion.tblValidacions.Clear()

            If ListaDistribucion IsNot Nothing Then
                ListaDistribucion.Clear()
            End If

            If ListaValidacion IsNot Nothing Then
                ListaValidacion.Clear()
            End If

            strComplementoRutaServidor = "\" & strActualParaWeb & "\" & STR_PROCESO & "\"

            IsVisibleCalcular = Visibility.Collapsed
            IsVisibleValidaciones = Visibility.Collapsed
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema mientras se cargaba el archivo", _
                                 Me.ToString(), "OrdenesRecomplementacionViewModel.CargarArchivo", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se termina de subir el archivo a cargar.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ProcesarArchivo()
        Try
            Dim strOrdenCompuesta = lngIdOrdenAno.ToString + Format(lngIdOrden, "000000")
            objProxyRecomplementacion.Load(objProxyRecomplementacion.CargarArchivoRecomplementacionQuery(strComplementoRutaServidor, strNombreArchivo, CInt(strOrdenCompuesta), Program.Usuario, Program.HashConexion), AddressOf TerminoCargaArchivo, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema mientras se cargaba el archivo", _
                                 Me.ToString(), "OrdenesRecomplementacionViewModel.TerminaSubir", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Contiene la lógica que interpreta la respuesta del procedimiento que carga el archivo
    ''' </summary>
    ''' <param name="lo">contiene la información de la distribución del archivo y las validaciones del proceso</param>
    ''' <remarks></remarks>
    Private Sub TerminoCargaArchivo(ByVal lo As LoadOperation(Of OyDOrdenes.tblResultadoCargaRecomplementacion))
        Try
            If Not lo.HasError Then

                ListaDistribucion = objProxyRecomplementacion.tblDistribucions
                dblTotalDistribucion = objProxyRecomplementacion.tblDistribucions.Sum(Function(dis) dis.dblCantidad)
                IsVisibleGridDistribucion = Visibility.Visible

                'SI EXISTEN VALIDACIONES SE MUESTRA EL GRID DE VALIDACIONES Y NO SE ACTIVA EL BOTON PROCESAR.
                If objProxyRecomplementacion.tblValidacions.Count > 0 Then

                    IsVisibleCalcular = Visibility.Visible
                    IsVisibleValidaciones = Visibility.Collapsed

                    ListaValidacion = objProxyRecomplementacion.tblValidacions


                Else

                    IsVisibleCalcular = Visibility.Visible
                    IsVisibleValidaciones = Visibility.Collapsed

                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para cargar el archivo", Me.ToString(), "OrdenesRecomplementacionViewModel.TerminoCargaArchivo", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga del archivo", Me.ToString(), "OrdenesRecomplementacionViewModel.TerminoCargaArchivo", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Dispara el proceso que realiza el calculo de las distribuciones de cantidad y precio.
    ''' </summary>
    ''' <remarks></remarks>
    Sub Calcular()
        Try

            IsBusy = True

            BusyContent = "Realizando el cálculo de la distribución..."


            'CAMBIO DE PANTALLA.
            IsVisiblePantallaCarga = Visibility.Collapsed
            IsVisiblePantallaCalculo = Visibility.Visible
            IsVisibleValidacionesProceso = Visibility.Collapsed
            IsVisibleProcesar = Visibility.Collapsed

            objProxyRecomplementacion.tblResultadoCalculoRecomplementacions.Clear()
            objProxyRecomplementacion.tblDistribucionClientes.Clear()
            objProxyRecomplementacion.tblDistribucionPrecios.Clear()
            objProxyRecomplementacion.tblValidacionProcesos.Clear()

            If ListaValidacionProceso IsNot Nothing Then
                ListaValidacionProceso.Clear()
            End If

            'If ListaColoresCeldas IsNot Nothing Then
            '    Dim intNumeroRegistros As Integer

            '    intNumeroRegistros = ListaColoresCeldas.Count

            '    Dim tempListaColoresCeldas = New List(Of SolidColorBrush)

            '    For i = 1 To intNumeroRegistros * 2
            '        tempListaColoresCeldas.Add(New SolidColorBrush(Colors.White))
            '    Next

            '    ListaColoresCeldas = tempListaColoresCeldas



            'End If


            'LLAMADO AL PROCEDIMIENTO DE CALCULOS.
            Dim strOrdenCompuesta = lngIdOrdenAno.ToString + Format(lngIdOrden, "000000")
            objProxyRecomplementacion.Load(objProxyRecomplementacion.CalcularDistribucionRecomplementacionQuery(strComplementoRutaServidor, strNombreArchivo, CInt(strOrdenCompuesta), Program.Usuario, Program.HashConexion), AddressOf TerminoCalculo, Nothing)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el cálculo de la distribución.", Me.ToString(), "OrdenesRecomplementacionViewModel.Calcular", Application.Current.ToString(), Program.Maquina, ex)
        End Try



    End Sub

    ''' <summary>
    ''' contiene la lógica que interpreta la respuesta del procedimiento de cálculo de las distribuciones.
    ''' </summary>
    ''' <param name="lo">contiene la información de las distribuciones y las validaciones</param>
    ''' <remarks></remarks>
    Private Sub TerminoCalculo(ByVal lo As LoadOperation(Of OyDOrdenes.tblResultadoCalculoRecomplementacion))
        Try
            If Not lo.HasError Then

                'CREA LAS LISTAS PARA LOS COLORES DE LAS FILAS.

                Dim strTick As String = Now.Ticks.ToString

                ListaDistribucionCliente = CrearTipoDinamico("clsDistribucionCliente", strTick, objProxyRecomplementacion.tblDistribucionClientes.Select(Function(dc) dc.strResultado).ToList())

                ListaDistribucionPrecio = CrearTipoDinamico("clsDistribucionPrecio", strTick, objProxyRecomplementacion.tblDistribucionPrecios.Select(Function(dc) dc.strResultado).ToList())

                ListaValidacionProceso = objProxyRecomplementacion.tblValidacionProcesos

                'CREA LAS LISTAS PARA LOS NOMBRES DE LAS COLUMNAS.
                ListaNombresDistribucionCliente = objProxyRecomplementacion.tblDistribucionClientes(1).strResultado.Split(MSTR_SEPARADOR).ToList

                ListaNombresDistribucionPrecio = objProxyRecomplementacion.tblDistribucionPrecios(1).strResultado.Split(MSTR_SEPARADOR).ToList





                If ListaValidacionProceso.Count > 0 Then

                    IsVisibleValidacionesProceso = Visibility.Visible

                    IsVisibleProcesar = Visibility.Collapsed

                    IsVisibleNuevoProceso = Visibility.Visible

                Else

                    IsVisibleValidacionesProceso = Visibility.Collapsed

                    IsVisibleProcesar = Visibility.Visible

                    IsVisibleNuevoProceso = Visibility.Collapsed

                End If


                'ListaColoresCeldas = New List(Of SolidColorBrush)

                'Dim intNumeroFilas As Integer = objProxyRecomplementacion.tblDistribucionClientes.Count - 2

                'Dim tempListaColoresCeldas = New List(Of SolidColorBrush)

                'For I = 1 To intNumeroFilas - 1
                'tempListaColoresCeldas.Add(New SolidColorBrush(Colors.Transparent))
                'Next

                'tempListaColoresCeldas.Add(New SolidColorBrush(Color.FromArgb(255, 79, 129, 189)))

                'ListaColoresCeldas = tempListaColoresCeldas
                'ListaColoresCeldas = New ObservableCollection(Of SolidColorBrush)
                'ListaColoresCeldas.Add(New SolidColorBrush(Colors.Blue))
                'ListaColoresCeldas.Add(New SolidColorBrush(Colors.Cyan))
                'ListaColoresCeldas.Add(New SolidColorBrush(Colors.Green))
                'ListaColoresCeldas.Add(New SolidColorBrush(Colors.Red))

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para calcular la distribución.", Me.ToString(), "OrdenesRecomplementacionViewModel.TerminoCalculo", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el cálculo de la distribución.", Me.ToString(), "OrdenesRecomplementacionViewModel.TerminoCalculo", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Genera un tipo de datos en tiempo de ejecución
    ''' </summary>
    ''' <param name="pstrNombreTipoDato">Nombre del nuevo tipo de datos</param>
    ''' <param name="pstrSufijo">Sufijo para el nombre del ensamblado</param>
    ''' <param name="source">Fuente de datos</param>
    ''' <returns>Lista de objetos creados</returns>
    ''' <remarks></remarks>
    Private Function CrearTipoDinamico(ByVal pstrNombreTipoDato As String, ByVal pstrSufijo As String, ByVal source As List(Of String)) As List(Of Object)

        'Se define el assembly y el módulo para el nuevo tipo de datos.
        Dim aName As AssemblyName = New AssemblyName("DataAssembly" & pstrSufijo)
        Dim aBuilder As AssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run)
        Dim mBuilder As ModuleBuilder = aBuilder.DefineDynamicModule(aName.Name)
        'Se define la clase.
        Dim tb As TypeBuilder = mBuilder.DefineType(pstrNombreTipoDato, TypeAttributes.Public)

        Dim objResultado As List(Of Object) = New List(Of Object)

        If source.Count > 0 Then

            Dim objListaNombres = source(0).Split(MSTR_SEPARADOR)

            'se definen la propiedades de la clase.
            For Each item In objListaNombres

                item = Trim(item)
                'Se define un campo privado para la propiedad
                Dim objCampo As FieldBuilder = tb.DefineField("_" + item, GetType(String), FieldAttributes.Private)

                'Se define la propiedad.
                Dim NamePropBldr As PropertyBuilder = tb.DefineProperty(item, PropertyAttributes.HasDefault, GetType(String), Nothing)
                Dim getSetAttr As MethodAttributes = MethodAttributes.Public Or MethodAttributes.SpecialName Or MethodAttributes.HideBySig

                Dim GetPropMthdBldr As MethodBuilder = tb.DefineMethod("Get" + item, getSetAttr, GetType(String), Type.EmptyTypes)
                Dim NameGetIL As ILGenerator = GetPropMthdBldr.GetILGenerator()
                NameGetIL.Emit(OpCodes.Ldarg_0)
                NameGetIL.Emit(OpCodes.Ldfld, objCampo)
                NameGetIL.Emit(OpCodes.Ret)

                Dim SetPropMthdBldr As MethodBuilder = tb.DefineMethod("Set", getSetAttr, Nothing, New Type() {GetType(String)})
                Dim NameSetIL As ILGenerator = SetPropMthdBldr.GetILGenerator()
                NameSetIL.Emit(OpCodes.Ldarg_0)
                NameSetIL.Emit(OpCodes.Ldarg_1)
                NameSetIL.Emit(OpCodes.Stfld, objCampo)
                NameSetIL.Emit(OpCodes.Ret)

                NamePropBldr.SetGetMethod(GetPropMthdBldr)
                NamePropBldr.SetSetMethod(SetPropMthdBldr)


            Next


            If source.Count > 2 Then

                'Se crea el tipo de datos.
                Dim retval As Type = tb.CreateType()


                For i As Integer = 2 To source.Count - 1

                    Dim objDatos As Object = Activator.CreateInstance(retval)

                    Dim objInfo = source(i).Split(MSTR_SEPARADOR)

                    For j As Integer = 0 To objInfo.Count - 1

                        retval.InvokeMember(Trim(objListaNombres(j)), BindingFlags.SetProperty, Nothing, _
                                objDatos, New Object() {objInfo(j)})

                    Next

                    objResultado.Add(objDatos)

                Next


            End If


        End If



        Return objResultado
    End Function

    ''' <summary>
    ''' Procesa el archivo de distribución.
    ''' </summary>
    ''' <remarks></remarks>
    Sub Procesar()
        Try

            IsBusy = True

            BusyContent = "Realizando el proceso de recomplementación..."
            Dim strOrdenCompuesta = lngIdOrdenAno.ToString + Format(lngIdOrden, "000000")

            objProxyRecomplementacion.ActualizarRecomplementacion(strComplementoRutaServidor, strNombreArchivo, CInt(strOrdenCompuesta), Program.Usuario, Program.HashConexion, AddressOf TerminoActualizarRecomplementacion, Nothing)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema mientras se procesaba la recomplementación.", Me.ToString(), "OrdenesRecomplementacionViewModel.Procesar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Respuesta del proceso de distribución
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoActualizarRecomplementacion(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If Not lo.HasError Then

                IsBusy = False

                A2Utilidades.Mensajes.mostrarMensaje("El proceso de recomplementación se completó satisfactoriamente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

                IsVisibleProcesar = Visibility.Collapsed
                IsVisibleNuevoProceso = Visibility.Visible

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el servicio para realizar la recomplementación.", Me.ToString(), "OrdenesRecomplementacionViewModel.TerminoActualizarRecomplementacion", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recomplementación.", Me.ToString(), "OrdenesRecomplementacionViewModel.TerminoActualizarRecomplementacion", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Establece la pantalla para comenzar un nuevo proceso.
    ''' </summary>
    ''' <remarks></remarks>
    Sub NuevoProceso()
        Try
            InicializarControles()
            lngIdOrdenAno = Now.Year
            lngIdOrden = 0
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema mientras se limpiaba la pantalla.", Me.ToString(), "OrdenesRecomplementacionViewModel.NuevoProceso", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Notifica del cambio de alguna de las propiedades de la clase.
    ''' </summary>
    ''' <param name="pstrItem">Nombre de la propiedad</param>
    ''' <remarks></remarks>
    Private Sub CambioItem(ByVal pstrItem As String)
        Try
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(pstrItem))
        Catch ex As Exception

        End Try
    End Sub
    Public Function [IsNumeric](ByVal str As String) As Boolean
        Dim result As Decimal = 0
        Return Decimal.TryParse(str, result)
    End Function

#End Region

End Class
