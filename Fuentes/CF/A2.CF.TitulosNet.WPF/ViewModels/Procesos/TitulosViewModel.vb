Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFTitulosNet
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports A2ComunesControl
Imports A2ComunesImportaciones

Public Class TitulosViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla "Títulos" perteneciente al proyecto OyD Server.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : Febrero 21/2014
    ''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok 
    ''' </history>

#Region "Constantes"
    Private Const STR_PARAMETROS_EXPORTAR As String = "[DTMFECHALIMITE]=[[DTMFECHALIMITE]]|[LNGCOMITENTE]=[[LNGCOMITENTE]]|[STRIDESPECIE]=[[STRIDESPECIE]]|[STRCLASE]=[[STRCLASE]]|[USUARIO]=[[USUARIO]]"
    Private Const STR_CARPETA As String = "EXPORTARPORTAFOLIO"
    Private Const STR_PROCESO As String = "TitulosExportarPortafolio"

    Private Enum PARAMETROSEXPORTAR
        DTMFECHALIMITE
        LNGCOMITENTE
        STRIDESPECIE
        STRCLASE
        USUARIO
    End Enum

    Const TAB_TITULOS_COMPRADOS = 0
    Const TAB_TITULOS_VENDIDOS = 1
    Const TAB_TITULOS_CRUCE = 2
#End Region

#Region "Variables - REQUERIDO"

    Public ViewTitulos As TitulosView = Nothing
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TitulosNetDomainContext  ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private intConsecutivo As Integer
    Private dtmExportarPortafolio As System.Nullable(Of System.DateTime)
    Private intTotalRegistrosCompras As Integer = 0
    Private intTotalRegistrosVentas As Integer = 0
    Public STRARCH_TITULOS_MVTOS As String = "NO"
    Private objArchivoSeleccionado As A2ComunesControl.ResultadoArchivoSeleccionado
    Private intIDSolicitudArchivoDeceval As Integer = 0
    Private strCodigoSolicitudArchivoDeceval As String = String.Empty


#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IndexClase = 0  '(Todos)
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        IsBusyArchivos = True
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0001
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Enero 5/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Enero 5/2015 - Resultado Ok 
    ''' </history>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                'Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                dtmFechaCorte = Date.Now()
                ConsultarParametros()

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

    Private _dtmFechaCorte As System.Nullable(Of System.DateTime)
    Public Property dtmFechaCorte() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaCorte
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaCorte = value
            MyBase.CambioItem("dtmFechaCorte")
        End Set
    End Property

    Private _strIdEspecie As String = String.Empty
    Public Property strIdEspecie() As String
        Get
            Return _strIdEspecie
        End Get
        Set(ByVal value As String)
            _strIdEspecie = value
            MyBase.CambioItem("strIdEspecie")
        End Set
    End Property

    Private _strClase As String = String.Empty
    Public Property strClase() As String
        Get
            Return _strClase
        End Get
        Set(ByVal value As String)
            _strClase = value
            MyBase.CambioItem("strClase")
        End Set
    End Property

    Private _lngIDComitente As String = String.Empty
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = LTrim(RTrim(value))
            MyBase.CambioItem("lngIDComitente")
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

    Private _strTipoProceso As String = String.Empty
    Public Property strTipoProceso() As String
        Get
            Return _strTipoProceso
        End Get
        Set(ByVal value As String)
            _strTipoProceso = value
            MyBase.CambioItem("strTipoProceso")
        End Set
    End Property

    Private _IndexClase As Integer
    Public Property IndexClase() As Integer
        Get
            Return _IndexClase
        End Get
        Set(ByVal value As Integer)
            _IndexClase = value
            MyBase.CambioItem("IndexClase")
        End Set
    End Property

    Private _HabilitarbtnComprar As Boolean = False
    Public Property HabilitarbtnComprar() As Boolean
        Get
            Return _HabilitarbtnComprar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarbtnComprar = value
            MyBase.CambioItem("HabilitarbtnComprar")
        End Set
    End Property

    Private _HabilitarbtnDescargar As Boolean = False
    Public Property HabilitarbtnDescargar() As Boolean
        Get
            Return _HabilitarbtnDescargar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarbtnDescargar = value
            MyBase.CambioItem("HabilitarbtnDescargar")
        End Set
    End Property

    Private _HabilitarbtnExportarExcel As Boolean = False
    Public Property HabilitarbtnExportarExcel() As Boolean
        Get
            Return _HabilitarbtnExportarExcel
        End Get
        Set(ByVal value As Boolean)
            _HabilitarbtnExportarExcel = value
            MyBase.CambioItem("HabilitarbtnExportarExcel")
        End Set
    End Property

    Private _HabilitarbtnVolver As Boolean = False
    Public Property HabilitarbtnVolver() As Boolean
        Get
            Return _HabilitarbtnVolver
        End Get
        Set(ByVal value As Boolean)
            _HabilitarbtnVolver = value
            MyBase.CambioItem("HabilitarbtnVolver")
        End Set
    End Property

    ''' <summary>
    ''' Lista de Titulos que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezadoCompras As EntitySet(Of TitulosCompras)
    Public Property ListaEncabezadoCompras() As EntitySet(Of TitulosCompras)
        Get
            Return _ListaEncabezadoCompras
        End Get
        Set(ByVal value As EntitySet(Of TitulosCompras))
            _ListaEncabezadoPaginadaCompras = Nothing
            _ListaEncabezadoPortafolio = Nothing
            _ListaEncabezadoCompras = value
            MyBase.CambioItem("ListaEncabezadoCompras")
            MyBase.CambioItem("ListaEncabezadoPortafolio")
            MyBase.CambioItem("ListaEncabezadoPaginadaCompras")
        End Set
    End Property

    Private _ListaEncabezadoComprasFiltro As List(Of TitulosCompras)
    Public Property ListaEncabezadoComprasFiltro() As List(Of TitulosCompras)
        Get
            Return _ListaEncabezadoComprasFiltro
        End Get
        Set(ByVal value As List(Of TitulosCompras))
            _ListaEncabezadoPaginadaCompras = Nothing
            _ListaEncabezadoComprasFiltro = value
            MyBase.CambioItem("ListaEncabezadoComprasFiltro")
            MyBase.CambioItem("ListaEncabezadoPaginadaCompras")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de Titulos para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginadaCompras As PagedCollectionView = Nothing
    Public ReadOnly Property ListaEncabezadoPaginadaCompras() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezadoCompras) Then
                If IsNothing(_ListaEncabezadoPaginadaCompras) Then
                    Dim view = New PagedCollectionView(_ListaEncabezadoCompras)
                    _ListaEncabezadoPaginadaCompras = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginadaCompras)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Lista de Titulos que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezadoVentas As EntitySet(Of TitulosVentas)
    Public Property ListaEncabezadoVentas() As EntitySet(Of TitulosVentas)
        Get
            Return _ListaEncabezadoVentas
        End Get
        Set(ByVal value As EntitySet(Of TitulosVentas))
            _ListaEncabezadoPaginadaVentas = Nothing
            _ListaEncabezadoVentas = value
            MyBase.CambioItem("ListaEncabezadoVentas")
            MyBase.CambioItem("ListaEncabezadoPaginadaVentas")
        End Set
    End Property

    Private _ListaEncabezadoVentasFiltro As List(Of TitulosVentas)
    Public Property ListaEncabezadoVentasFiltro() As List(Of TitulosVentas)
        Get
            Return _ListaEncabezadoVentasFiltro
        End Get
        Set(ByVal value As List(Of TitulosVentas))
            _ListaEncabezadoPaginadaVentas = Nothing
            _ListaEncabezadoVentasFiltro = value

            MyBase.CambioItem("ListaEncabezadoVentasFiltro")
            MyBase.CambioItem("ListaEncabezadoPaginadaVentas")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de Titulos para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginadaVentas As PagedCollectionView = Nothing
    Public ReadOnly Property ListaEncabezadoPaginadaVentas() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezadoVentas) Then
                If IsNothing(_ListaEncabezadoPaginadaVentas) Then
                    Dim view = New PagedCollectionView(_ListaEncabezadoVentas)
                    _ListaEncabezadoPaginadaVentas = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginadaVentas)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Lista de Titulos que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezadoCruce As EntitySet(Of TitulosCruce)
    Public Property ListaEncabezadoCruce() As EntitySet(Of TitulosCruce)
        Get
            Return _ListaEncabezadoCruce
        End Get
        Set(ByVal value As EntitySet(Of TitulosCruce))
            _ListaEncabezadoCruce = value
            FiltrarCruces(String.Empty)
            MyBase.CambioItem("ListaEncabezadoCruce")
        End Set
    End Property

    Private _ListaEncabezadoCruceFiltro As List(Of TitulosCruce)
    Public Property ListaEncabezadoCruceFiltro() As List(Of TitulosCruce)
        Get
            Return _ListaEncabezadoCruceFiltro
        End Get
        Set(ByVal value As List(Of TitulosCruce))
            _ListaEncabezadoPaginadaCruce = Nothing
            _ListaEncabezadoCruceFiltro = value

            MyBase.CambioItem("ListaEncabezadoCruceFiltro")
            MyBase.CambioItem("ListaEncabezadoPaginadaCruce")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de Titulos para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginadaCruce As PagedCollectionView = Nothing
    Public ReadOnly Property ListaEncabezadoPaginadaCruce() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezadoCruceFiltro) Then
                If IsNothing(_ListaEncabezadoPaginadaCruce) Then
                    Dim view = New PagedCollectionView(_ListaEncabezadoCruceFiltro)
                    _ListaEncabezadoPaginadaCruce = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginadaCruce)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Lista de Titulos que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezadoPortafolio As EntitySet(Of TitulosPortafolio)
    Public Property ListaEncabezadoPortafolio() As EntitySet(Of TitulosPortafolio)
        Get
            Return _ListaEncabezadoPortafolio
        End Get
        Set(ByVal value As EntitySet(Of TitulosPortafolio))
            _ListaEncabezadoPaginadaPortafolio = Nothing
            _ListaEncabezadoPortafolio = value

            MyBase.CambioItem("ListaEncabezadoPortafolio")
            MyBase.CambioItem("ListaEncabezadoPaginadaPortafolio")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de Titulos para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginadaPortafolio As PagedCollectionView = Nothing
    Public ReadOnly Property ListaEncabezadoPaginadaPortafolio() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezadoPortafolio) Then
                If IsNothing(_ListaEncabezadoPaginadaPortafolio) Then
                    Dim view = New PagedCollectionView(_ListaEncabezadoPortafolio)
                    _ListaEncabezadoPaginadaPortafolio = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginadaPortafolio)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TabSeleccionado As Integer = 0
    Public Property TabSeleccionado As Integer
        Get
            Return _TabSeleccionado
        End Get
        Set(value As Integer)
            _TabSeleccionado = value
            Select Case value
                Case TAB_TITULOS_COMPRADOS
                    VisibilidadContenedorPrincipalPortafolio = Visibility.Visible
                Case TAB_TITULOS_VENDIDOS
                    VisibilidadContenedorPrincipalPortafolio = Visibility.Visible
                Case TAB_TITULOS_CRUCE
                    VisibilidadContenedorPrincipalPortafolio = Visibility.Collapsed
            End Select
            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property

    Private _VisibilidadContenedorPrincipalPortafolio As Visibility = Visibility.Visible
    Public Property VisibilidadContenedorPrincipalPortafolio() As Visibility
        Get
            Return _VisibilidadContenedorPrincipalPortafolio
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadContenedorPrincipalPortafolio = value
            MyBase.CambioItem("VisibilidadContenedorPrincipalPortafolio")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As TitulosVentas
    Public Property DetalleSeleccionado() As TitulosVentas
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As TitulosVentas)
            _DetalleSeleccionado = value
            If Not IsNothing(DetalleSeleccionado) Then
                ConsultarPortafolio()
            End If

            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    Private _IsBusyPortafolio As Boolean
    Public Property IsBusyPortafolio() As Boolean
        Get
            Return _IsBusyPortafolio
        End Get
        Set(ByVal value As Boolean)
            _IsBusyPortafolio = value
            MyBase.CambioItem("IsBusyPortafolio")
        End Set
    End Property

    Private _strCantidadRegistrosCompras As String = String.Empty
    Public Property strCantidadRegistrosCompras() As String
        Get
            Return _strCantidadRegistrosCompras
        End Get
        Set(ByVal value As String)
            _strCantidadRegistrosCompras = value
            MyBase.CambioItem("strCantidadRegistrosCompras")
        End Set
    End Property

    Private _strCantidadRegistrosVentas As String = String.Empty
    Public Property strCantidadRegistrosVentas() As String
        Get
            Return _strCantidadRegistrosVentas
        End Get
        Set(ByVal value As String)
            _strCantidadRegistrosVentas = value
            MyBase.CambioItem("strCantidadRegistrosVentas")
        End Set
    End Property

    Private _strFiltroCompras As String = String.Empty
    Public Property strFiltroCompras() As String
        Get
            Return _strFiltroCompras
        End Get
        Set(ByVal value As String)
            _strFiltroCompras = value
            MyBase.CambioItem("strFiltroCompras")
        End Set
    End Property

    Private _strFiltroVentas As String = String.Empty
    Public Property strFiltroVentas() As String
        Get
            Return _strFiltroVentas
        End Get
        Set(ByVal value As String)
            _strFiltroVentas = value
            MyBase.CambioItem("strFiltroVentas")
        End Set
    End Property

    Private _ViewImportarArchivo As cwCargaArchivos
    Public Property ViewImportarArchivo() As cwCargaArchivos
        Get
            Return _ViewImportarArchivo
        End Get
        Set(ByVal value As cwCargaArchivos)
            _ViewImportarArchivo = value
        End Set
    End Property

    Private _strRuta As String
    Public Property strRuta() As String
        Get
            Return _strRuta
        End Get
        Set(ByVal value As String)
            _strRuta = value
            MyBase.CambioItem("strRuta")
        End Set
    End Property

    Private _strExtensionesPermitidas As String = "Archivo de Texto|*.txt"
    Public Property strExtensionesPermitidas() As String
        Get
            Return _strExtensionesPermitidas
        End Get
        Set(ByVal value As String)
            _strExtensionesPermitidas = value
            MyBase.CambioItem("strExtensionesPermitidas")
        End Set
    End Property

    Private _MostrarSolicitudArchivoDeceval As Boolean = False
    Public Property MostrarSolicitudArchivoDeceval() As Boolean
        Get
            Return _MostrarSolicitudArchivoDeceval
        End Get
        Set(ByVal value As Boolean)
            _MostrarSolicitudArchivoDeceval = value
            MyBase.CambioItem("MostrarSolicitudArchivoDeceval")
        End Set
    End Property

    Private _RutaInternaUploadsArchivosDeceval As String
    Public Property RutaInternaUploadsArchivosDeceval() As String
        Get
            Return _RutaInternaUploadsArchivosDeceval
        End Get
        Set(ByVal value As String)
            _RutaInternaUploadsArchivosDeceval = value
            MyBase.CambioItem("RutaInternaUploadsArchivosDeceval")
        End Set
    End Property

    Private _IsBusyArchivos As Boolean = False
    Public Property IsBusyArchivos() As Boolean
        Get
            Return _IsBusyArchivos
        End Get
        Set(ByVal value As Boolean)
            _IsBusyArchivos = value
            MyBase.CambioItem("IsBusyArchivos")
        End Set
    End Property


#End Region

#Region "Propiedades de la Especie"
    Private _NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionado = value
            strIdEspecie = String.Empty
            MyBase.CambioItem("strIdEspecie")
        End Set
    End Property
#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    Public Async Function ConsultarLiquidacionesCompra() As Task
        Try
            Dim objTitulosCompras As LoadOperation(Of TitulosCompras)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosCompras.Clear()

            objTitulosCompras = Await mdcProxy.Load(mdcProxy.Titulos_CompradosSinCargar_ConsultarSyncQuery(Nothing, False, Nothing, lngIDComitente, strIdEspecie, dtmFechaCorte, strClase, intConsecutivo, Program.Usuario, Program.HashConexion)).AsTask

            If Not objTitulosCompras Is Nothing Then
                If objTitulosCompras.HasError Then
                    If objTitulosCompras.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar las liquidaciones de compra.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las liquidaciones de compra.", Me.ToString(), "ConsultarLiquidacionesCompra", Program.TituloSistema, Program.Maquina, objTitulosCompras.Error)
                    End If

                    objTitulosCompras.MarkErrorAsHandled()
                Else
                    ListaEncabezadoCompras = mdcProxy.TitulosCompras
                    If ListaEncabezadoCompras.Count > 0 Then
                        intTotalRegistrosCompras = CInt(mdcProxy.TitulosCompras.FirstOrDefault.intCantidadRegistros)
                        HabilitarbtnComprar = True
                        strCantidadRegistrosCompras = "10" +
                                                      " registro(s) de " + CStr(mdcProxy.TitulosCompras.Count()) +
                                                      " existente(s)"
                        'CStr(ListaEncabezadoCompras.Count)
                        '" registro(s) de " + CStr(mdcProxy.TitulosCompras.FirstOrDefault.intCantidadRegistros) +
                    Else
                        HabilitarbtnComprar = False
                        strCantidadRegistrosCompras = "0 registro(s) de 0 existente(s)"
                    End If
                End If
            Else
                ListaEncabezadoCompras = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar liquidaciones de compra", Me.ToString, "ConsultarLiquidacionesCompra", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Public Async Function ConsultarLiquidacionesVenta() As Task
        Try
            Dim objTitulosVentas As LoadOperation(Of TitulosVentas)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosVentas.Clear()

            objTitulosVentas = Await mdcProxy.Load(mdcProxy.Titulos_VendidosSinDescargar_ConsultarSyncQuery(Nothing, False, Nothing, lngIDComitente, strIdEspecie, dtmFechaCorte, strClase, intConsecutivo, Program.Usuario, Program.HashConexion)).AsTask

            If Not objTitulosVentas Is Nothing Then
                If objTitulosVentas.HasError Then
                    If objTitulosVentas.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar liquidaciones de venta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar liquidaciones de venta.", Me.ToString(), "ConsultarLiquidacionesVenta", Program.TituloSistema, Program.Maquina, objTitulosVentas.Error)
                    End If

                    objTitulosVentas.MarkErrorAsHandled()
                Else
                    ListaEncabezadoVentas = mdcProxy.TitulosVentas
                    If ListaEncabezadoVentas.Count > 0 Then
                        intTotalRegistrosVentas = CInt(mdcProxy.TitulosVentas.FirstOrDefault.intCantidadRegistros)
                        HabilitarbtnDescargar = True
                        strCantidadRegistrosVentas = "10" +
                                                      " registro(s) de " + CStr(mdcProxy.TitulosVentas.Count()) +
                                                      " existente(s)"
                    Else
                        HabilitarbtnDescargar = False
                        strCantidadRegistrosVentas = "0 registro(s) de 0 existente(s)"
                    End If
                End If
            Else
                ListaEncabezadoVentas = Nothing
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar liquidaciones de venta", Me.ToString, "ConsultarLiquidacionesVenta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Public Async Function ConsultarCruce() As Task
        Try
            Dim objTitulosCruce As LoadOperation(Of TitulosCruce)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosCruces.Clear()

            objTitulosCruce = Await mdcProxy.Load(mdcProxy.ConsultarCruceSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objTitulosCruce Is Nothing Then
                If objTitulosCruce.HasError Then
                    If objTitulosCruce.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el cruce de liquidaciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el cruce de liquidaciones.", Me.ToString(), "ConsultarCruce", Program.TituloSistema, Program.Maquina, objTitulosCruce.Error)
                    End If

                    objTitulosCruce.MarkErrorAsHandled()
                Else
                    ListaEncabezadoCruce = mdcProxy.TitulosCruces
                    If Not ListaEncabezadoCruce.Count > 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("No hay títulos para cruzar con las liquidaciones de venta, recuerde primero cargar las compras.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionado = TAB_TITULOS_COMPRADOS
                        VisibilidadContenedorPrincipalPortafolio = Visibility.Visible
                        HabilitarbtnVolver = False
                    Else
                        TabSeleccionado = TAB_TITULOS_CRUCE
                        VisibilidadContenedorPrincipalPortafolio = Visibility.Collapsed
                        mdcProxy.TitulosPortafolios.Clear()
                        HabilitarbtnVolver = True
                        HabilitarbtnExportarExcel = False
                    End If
                End If
            Else
                ListaEncabezadoCruce = Nothing
            End If

            MyBase.CambioItem("ListaEncabezadoCruce")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el cruce de liquidaciones.", Me.ToString, "ConsultarCruce", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    ''' <history>
    ''' ID caso de prueba: CP0007
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Enero 5/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Enero 5/2015 - Resultado Ok 
    ''' </history>
    Public Async Function ConsultarPortafolio() As Task
        Try
            Dim objTitulosPortafolio As LoadOperation(Of TitulosPortafolio)

            IsBusyPortafolio = True

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosPortafolios.Clear()

            objTitulosPortafolio = Await mdcProxy.Load(mdcProxy.PortafolioCliente_ConsultarSyncQuery(dtmFechaCorte, DetalleSeleccionado.lngIDComitente, DetalleSeleccionado.strIDEspecie, DetalleSeleccionado.strClase_Descripcion, Program.Usuario, Program.HashConexion)).AsTask

            dtmExportarPortafolio = dtmFechaCorte

            If Not objTitulosPortafolio Is Nothing Then
                If objTitulosPortafolio.HasError Then
                    If objTitulosPortafolio.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el portafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el portafolio.", Me.ToString(), "ConsultarPortafolio", Program.TituloSistema, Program.Maquina, objTitulosPortafolio.Error)
                    End If

                    objTitulosPortafolio.MarkErrorAsHandled()
                Else
                    ListaEncabezadoPortafolio = mdcProxy.TitulosPortafolios
                    If ListaEncabezadoPortafolio.Count > 0 Then
                        HabilitarbtnExportarExcel = True
                    Else
                        HabilitarbtnExportarExcel = False
                    End If
                End If
            Else
                ListaEncabezadoPortafolio = Nothing
            End If

            IsBusyPortafolio = False

        Catch ex As Exception
            IsBusyPortafolio = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar el portafolio", Me.ToString, "ConsultarPortafolio", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Public Async Function CerrarPantalla() As Task
        Try
            If Not IsNothing(intConsecutivo) Then
                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyTitulosNet()
                End If

                Await mdcProxy.GuardarUsuarioProcesoTitulosSync("D", intConsecutivo, "", Program.Usuario, Program.HashConexion).AsTask()

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de títulos", Me.ToString, "CerrarPantalla", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    ''' <summary>
    ''' Función utilizada para importar el archivo Deceval.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0010
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              8 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 8 de Septiembre/2015
    ''' </history> 
    Public Async Function CargarArchivo(pstrModulo As String, pstrNombreCompletoArchivo As String, plogEliminarRegistrosTodos As Boolean) As Task
        Try
            If MostrarSolicitudArchivoDeceval Then

            Else

            End If
            Dim objRet As LoadOperation(Of RespuestaArchivoImportacion)

            ViewImportarArchivo.IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            strRuta = pstrNombreCompletoArchivo

            mdcProxy.RespuestaArchivoImportacions.Clear()

            If MostrarSolicitudArchivoDeceval Then
                If Not IsNothing(objArchivoSeleccionado) Then
                    If objArchivoSeleccionado.EsArchivoLocal = False Then
                        objRet = Await mdcProxy.Load(mdcProxy.Titulos_MovimientosDeceval_ImportarSeleccionArchivoSyncQuery(dtmFechaCorte, objArchivoSeleccionado.RutaInterna, Program.Usuario, Program.HashConexion)).AsTask
                    Else
                        objRet = Await mdcProxy.Load(mdcProxy.Titulos_MovimientosDeceval_ImportarSyncQuery(dtmFechaCorte, "MovimientosDeceval", pstrNombreCompletoArchivo, Program.Usuario, Program.HashConexion)).AsTask
                    End If
                Else
                    objRet = Await mdcProxy.Load(mdcProxy.Titulos_MovimientosDeceval_ImportarSyncQuery(dtmFechaCorte, "MovimientosDeceval", pstrNombreCompletoArchivo, Program.Usuario, Program.HashConexion)).AsTask
                End If
            Else
                objRet = Await mdcProxy.Load(mdcProxy.Titulos_MovimientosDeceval_ImportarSyncQuery(dtmFechaCorte, "MovimientosDeceval", pstrNombreCompletoArchivo, Program.Usuario, Program.HashConexion)).AsTask
            End If


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al validar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objListaRespuesta As List(Of RespuestaArchivoImportacion)
                        Dim objListaMensajes As New List(Of String)

                        objListaRespuesta = objRet.Entities.ToList

                        If objListaRespuesta.Count > 0 Then
                            If objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).Count > 0 Then

                                objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).OrderBy(Function(o) o.Tipo)
                                    If li.Tipo = "C" Then
                                        objListaMensajes.Add(li.Mensaje)
                                    Else
                                        objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                                    End If
                                Next

                                objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                            Else
                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean))

                                    If li.lngOperacion = 0 Then
                                        If ViewImportarArchivo.strMovimientosDeceval = String.Empty Then
                                            ViewImportarArchivo.strMovimientosDeceval = ViewImportarArchivo.strMovimientosDeceval + li.Mensaje
                                        Else
                                            ViewImportarArchivo.strMovimientosDeceval = ViewImportarArchivo.strMovimientosDeceval + vbNewLine + li.Mensaje
                                        End If
                                    Else
                                        objListaMensajes.Add(li.Mensaje)
                                    End If
                                Next
                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                            End If
                        Else
                            ViewImportarArchivo.ListaMensajes = objListaMensajes
                        End If
                    End If
                End If
            End If

            ViewImportarArchivo.IsBusy = False

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al subir el archivo.", _
                               Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Function

    Public Sub ConsultarArchivosSolicitados()
        Try
            Dim objControlArchivos As New A2ComunesControl.ListarArchivosDirectorioInternoUploads(RutaInternaUploadsArchivosDeceval, True, "MovimientosDeceval", strExtensionesPermitidas)
            AddHandler objControlArchivos.Closed, AddressOf RespuestaControlListarDirectorio
            Program.Modal_OwnerMainWindowsPrincipal(objControlArchivos)
            objControlArchivos.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al abrir el control para seleccionar los archivos.",
                              Me.ToString(), "ConsultarArchivosSolicitados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub RespuestaControlListarDirectorio(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objRespueta = CType(sender, ListarArchivosDirectorioInternoUploads)

            If objRespueta.DialogResult Then
                objArchivoSeleccionado = objRespueta.ArchivoSeleccionado
                Dim viewImportacion As New cwCargaArchivos(Me, objArchivoSeleccionado.Nombre, "MovimientosDeceval", STRARCH_TITULOS_MVTOS, False)
                Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                viewImportacion.ShowDialog()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta para seleccionar los archivos.",
                                          Me.ToString(), "RespuestaControlListarDirectorio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SolicitarArchivoDeceval()
        Try
            IsBusyArchivos = True
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            If Not IsNothing(mdcProxy.tblResultadoEnvioArchivos) Then
                mdcProxy.tblResultadoEnvioArchivos.Clear()
            End If

            mdcProxy.Load(mdcProxy.SolicitarArchivoMovimientosDecevalQuery(strCodigoSolicitudArchivoDeceval, intIDSolicitudArchivoDeceval, String.Empty, Nothing, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoProcesarArchivoDeceval, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al solicitar los archivos de Deceval.",
                              Me.ToString(), "SolicitarArchivoDeceval", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyArchivos = False
        End Try
    End Sub

    Private Sub TerminoProcesarArchivoDeceval(ByVal lo As LoadOperation(Of tblResultadoEnvioArchivo))
        Try
            If Not lo.HasError Then
                Dim strValidacionesProcesar As String = String.Empty
                Dim intIDArchivoInsertado As Integer = 0
                Dim strNombreConfiguracion As String = String.Empty
                Dim logEjecucionAutomatica As Boolean = False

                For Each li In lo.Entities.ToList
                    If li.logExitoso = False Then
                        If String.IsNullOrEmpty(strValidacionesProcesar) Then
                            strValidacionesProcesar = li.Mensaje
                        Else
                            strValidacionesProcesar = String.Format("{0}{1}{2}", strValidacionesProcesar, vbCrLf, li.Mensaje)
                        End If
                    End If
                Next

                If String.IsNullOrEmpty(strValidacionesProcesar) Then
                    Dim objControlArchivos As New A2ComunesControl.ListarArchivosDirectorioInternoUploads(RutaInternaUploadsArchivosDeceval, True, "MovimientosDeceval", strExtensionesPermitidas)
                    AddHandler objControlArchivos.Closed, AddressOf RespuestaControlListarDirectorio
                    Program.Modal_OwnerMainWindowsPrincipal(objControlArchivos)
                    objControlArchivos.ShowDialog()
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(String.Format("Al realizar la solicitud del archivo ocurrieron las siguientes validaciones:{0}{1}", vbCrLf, strValidacionesProcesar), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de archivos procesados", _
                                                 Me.ToString(), "TerminoProcesarArchivoDeceval", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de archivos procesados", _
                                                 Me.ToString(), "TerminoProcesarArchivoDeceval", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
        IsBusyArchivos = False
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            'Valida la fecha de valoración
            If IsNothing(dtmFechaCorte) Then
                strMsg = String.Format("{0}{1} + La fecha de corte es un campo requerido.", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                IsBusy = False
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Private Async Sub ConfirmarConsultarLiquidaciones(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then

                IsBusy = True

                Await mdcProxy.GuardarUsuarioProcesoTitulosSync("U", intConsecutivo, "", Program.Usuario, Program.HashConexion).AsTask()

                Await ConsultarLiquidacionesCompra()
                Await ConsultarLiquidacionesVenta()

                lngIDComitente = String.Empty
                strIdEspecie = String.Empty
                strClase = "T"

                MyBase.CambioItem("ListaEncabezadoCompras")
                MyBase.CambioItem("ListaEncabezadoVentas")
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar consultar liquidaciones.", Me.ToString(), "ConfirmarConsultarLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub ConfirmarExportarPortafolio(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True

            Dim objRet As LoadOperation(Of GenerarArchivosPlanos)
            Dim dcProxyUtil As UtilidadesDomainContext
            Dim strParametros As String = STR_PARAMETROS_EXPORTAR
            Dim dia As String = String.Empty
            Dim mes As String = String.Empty
            Dim ano As String = String.Empty
            Dim strFechaLimite As String = String.Empty

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not ValidarDatos() Then Exit Sub

                dcProxyUtil = inicializarProxyUtilidadesOYD()

                If (dtmExportarPortafolio.Value.Day < 10) Then
                    dia = "0" + dtmExportarPortafolio.Value.Day.ToString
                Else
                    dia = dtmExportarPortafolio.Value.Day.ToString
                End If

                If (dtmExportarPortafolio.Value.Month < 10) Then
                    mes = "0" + dtmExportarPortafolio.Value.Month.ToString
                Else
                    mes = dtmExportarPortafolio.Value.Month.ToString
                End If

                ano = dtmExportarPortafolio.Value.Year.ToString

                strFechaLimite = ano + mes + dia

                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.DTMFECHALIMITE), strFechaLimite)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.LNGCOMITENTE), DetalleSeleccionado.lngIDComitente)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.STRIDESPECIE), DetalleSeleccionado.strIDEspecie)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.STRCLASE), DetalleSeleccionado.strClase_Descripcion)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.USUARIO), Program.Usuario)

                objRet = Await dcProxyUtil.Load(dcProxyUtil.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_PROCESO, strParametros, "InformePortafolio", "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "ExportarPortafolioExcel", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If
                        objRet.MarkErrorAsHandled()
                    Else
                        If objRet.Entities.Count > 0 Then
                            Dim objResultado = objRet.Entities.First

                            If objResultado.Exitoso Then
                                Program.VisorArchivosWeb_DescargarURL(objResultado.RutaArchivoPlano & "?date=" & strFechaLimite & DateTime.Now.ToString("HH:mm:ss"))
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If

            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar la exportación del portafolio en excel.", Me.ToString(), "ConfirmarExportarPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba: CP0004
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Enero 5/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Enero 5/2015 - Resultado Ok 
    ''' </history>
    Public Async Function Cargar() As Task
        Try
            Dim objCargar As InvokeOperation(Of String)

            Dim intCantidadLiquidacionesAntes = ListaEncabezadoCompras.FirstOrDefault.intCantidadRegistros

            IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            objCargar = Await mdcProxy.Titulos_ComprasXCargar_ConsultarSync(lngIDComitente, strIdEspecie, strClase, intConsecutivo, Program.Usuario, True, Program.HashConexion).AsTask()

            If Not objCargar Is Nothing Then
                If objCargar.HasError Then
                    If objCargar.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al cargar liquidaciones de compra.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al cargar liquidaciones de compra.", Me.ToString(), "Cargar", Program.TituloSistema, Program.Maquina, objCargar.Error)
                    End If

                    objCargar.MarkErrorAsHandled()
                Else
                    Await ConsultarLiquidacionesCompra()

                    Dim intCantidadLiquidacionesDespues As Integer = 0

                    If Not IsNothing(ListaEncabezadoCompras.FirstOrDefault) Then
                        intCantidadLiquidacionesDespues = CInt(ListaEncabezadoCompras.FirstOrDefault.intCantidadRegistros)
                    End If

                    A2Utilidades.Mensajes.mostrarMensaje("Se cargaron " & (intCantidadLiquidacionesAntes - intCantidadLiquidacionesDespues).ToString & " custodias", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                End If
            Else
                ListaEncabezadoCompras = Nothing
            End If

            strFiltroCompras = String.Empty

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al cargar liquidaciones de compra.", Me.ToString(), "Cargar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    ''' <history>
    ''' ID caso de prueba: CP0005, CP0006
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Enero 5/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Enero 5/2015 - Resultado Ok 
    ''' </history>
    Public Async Function Descargar() As Task
        Try
            Dim objDescargar As InvokeOperation(Of String)

            IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            objDescargar = Await mdcProxy.Titulos_CruceTitulosPortafolio_ConsultarSync(strIdEspecie, lngIDComitente, Date.Now(), dtmFechaCorte, Date.Now(), 0, Nothing, 0, Nothing, 0, strClase, 0, 0, Nothing, 0, 0, intConsecutivo, Program.Usuario, True, Program.HashConexion).AsTask()

            If Not objDescargar Is Nothing Then
                If objDescargar.HasError Then
                    If objDescargar.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al descargar liquidaciones de venta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al descargar liquidaciones de venta.", Me.ToString(), "Descargar", Program.TituloSistema, Program.Maquina, objDescargar.Error)
                    End If

                    objDescargar.MarkErrorAsHandled()
                Else
                    Await ConsultarLiquidacionesVenta()
                    Await ConsultarCruce()

                    lngIDComitente = String.Empty
                    strFiltroCompras = String.Empty
                    strClase = "T"
                End If
            Else
                ListaEncabezadoVentas = Nothing
            End If

            strFiltroVentas = String.Empty

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al descargar liquidaciones de venta.", Me.ToString(), "Descargar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Public Sub Volver()
        Try
            TabSeleccionado = TAB_TITULOS_COMPRADOS
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al volver al tab Títulos comprados.", Me.ToString(), "Volver", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function FiltrarCompras(ByVal pstrFiltro As String) As Task
        Try
            Dim objTitulosCompras As LoadOperation(Of TitulosCompras)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosCompras.Clear()

            IsBusy = True

            objTitulosCompras = Await mdcProxy.Load(mdcProxy.Titulos_CompradosSinCargar_ConsultarSyncQuery(pstrFiltro, CType(IIf(pstrFiltro = "", True, False), Boolean?), Nothing, lngIDComitente, strIdEspecie, dtmFechaCorte, strClase, intConsecutivo, Program.Usuario, Program.HashConexion)).AsTask

            If Not objTitulosCompras Is Nothing Then
                If objTitulosCompras.HasError Then
                    If objTitulosCompras.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar las liquidaciones de compra.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las liquidaciones de compra.", Me.ToString(), "ConsultarLiquidacionesCompra", Program.TituloSistema, Program.Maquina, objTitulosCompras.Error)
                    End If

                    objTitulosCompras.MarkErrorAsHandled()
                Else
                    ListaEncabezadoCompras = mdcProxy.TitulosCompras
                    If ListaEncabezadoCompras.Count > 0 Then
                        HabilitarbtnComprar = True
                        strCantidadRegistrosCompras = CStr(ListaEncabezadoCompras.Count) +
                                                      " registro(s) de " + CStr(mdcProxy.TitulosCompras.FirstOrDefault.intCantidadRegistros) +
                                                      " existente(s)"
                    Else
                        HabilitarbtnComprar = False
                        strCantidadRegistrosCompras = "0 registro(s) de " +
                                                      CStr(intTotalRegistrosCompras) + " existente(s)"
                    End If

                End If
            Else
                ListaEncabezadoCompras = Nothing
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al filtrar la información de las compras.", Me.ToString(), "FiltrarCompras", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Public Async Function FiltrarVentas(ByVal pstrFiltro As String) As Task
        Try
            Dim objTitulosVentas As LoadOperation(Of TitulosVentas)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosVentas.Clear()

            IsBusy = True

            objTitulosVentas = Await mdcProxy.Load(mdcProxy.Titulos_VendidosSinDescargar_ConsultarSyncQuery(pstrFiltro, CType(IIf(pstrFiltro = "", True, False), Boolean?), Nothing, lngIDComitente, strIdEspecie, dtmFechaCorte, strClase, intConsecutivo, Program.Usuario, Program.HashConexion)).AsTask

            If Not objTitulosVentas Is Nothing Then
                If objTitulosVentas.HasError Then
                    If objTitulosVentas.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar liquidaciones de venta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar liquidaciones de venta.", Me.ToString(), "ConsultarLiquidacionesVenta", Program.TituloSistema, Program.Maquina, objTitulosVentas.Error)
                    End If

                    objTitulosVentas.MarkErrorAsHandled()
                Else
                    ListaEncabezadoVentas = mdcProxy.TitulosVentas
                    If ListaEncabezadoVentas.Count > 0 Then
                        HabilitarbtnDescargar = True
                        strCantidadRegistrosVentas = CStr(ListaEncabezadoVentas.Count) +
                                                      " registro(s) de " + CStr(mdcProxy.TitulosVentas.FirstOrDefault.intCantidadRegistros) +
                                                      " existente(s)"
                    Else
                        HabilitarbtnDescargar = False
                        strCantidadRegistrosVentas = "0 registro(s) de " +
                                                      CStr(intTotalRegistrosVentas) + " existente(s)"
                    End If
                End If
            Else
                ListaEncabezadoVentas = Nothing
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al filtrar la información de las ventas.", Me.ToString(), "FiltrarVentas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Public Sub FiltrarCruces(ByVal pstrFiltro As String)
        Try
            Dim objListaFiltrada As New List(Of TitulosCruce)

            If Not IsNothing(pstrFiltro) Then
                pstrFiltro = pstrFiltro.ToLower
            End If

            If Not IsNothing(_ListaEncabezadoCruce) Then
                If String.IsNullOrEmpty(pstrFiltro) Then
                    objListaFiltrada = (From objCruce In _ListaEncabezadoCruce Select objCruce).ToList
                Else
                    objListaFiltrada = (From objCruce In _ListaEncabezadoCruce
                                                        Where (Not IsNothing(objCruce.strLiq_Parcial) AndAlso objCruce.strLiq_Parcial.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.lngIdRecibo) AndAlso objCruce.lngIdRecibo.ToString.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.lngSecuencia) AndAlso objCruce.lngSecuencia.ToString.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.lngIDComitente) AndAlso objCruce.lngIDComitente.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.dblCantidad) AndAlso objCruce.dblCantidad.ToString.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.dblCantidadPedida) AndAlso objCruce.dblCantidadPedida.ToString.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.strIDEspecie) AndAlso objCruce.strIDEspecie.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.strClase) AndAlso objCruce.strClase.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.strTipo) AndAlso objCruce.strTipo.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.strTipoDeOferta) AndAlso objCruce.strTipoDeOferta.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.dtmLiquidacion) AndAlso objCruce.dtmLiquidacion.Value.ToString("dd/MM/yyyy") = pstrFiltro) _
                                                        Or (Not IsNothing(objCruce.dtmEmision) AndAlso objCruce.dtmEmision.Value.ToString("dd/MM/yyyy") = pstrFiltro) _
                                                        Or (Not IsNothing(objCruce.dtmCumplimiento) AndAlso objCruce.dtmCumplimiento.Value.ToString("dd/MM/yyyy") = pstrFiltro) _
                                                        Or (Not IsNothing(objCruce.dtmEmision) AndAlso objCruce.dtmEmision.Value.ToString("dd/MM/yyyy") = pstrFiltro) _
                                                        Or (Not IsNothing(objCruce.dtmVencimiento) AndAlso objCruce.dtmVencimiento.Value.ToString("dd/MM/yyyy") = pstrFiltro) _
                                                        Or (Not IsNothing(objCruce.dblTasa) AndAlso objCruce.dblTasa.ToString.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.strIndicadorEconomico) AndAlso objCruce.strIndicadorEconomico.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.dblPuntosIndicador) AndAlso objCruce.dblPuntosIndicador.ToString.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.strModalidad) AndAlso objCruce.strModalidad.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.strObservaciones) AndAlso objCruce.strObservaciones.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.dblLiq_Cantidad) AndAlso objCruce.dblLiq_Cantidad.ToString.ToLower.Contains(pstrFiltro)) _
                                                        Or (Not IsNothing(objCruce.intCruzar) AndAlso objCruce.intCruzar.ToString.ToLower.Contains(pstrFiltro)) _
                                                        Select objCruce).ToList
                End If
            End If

            ListaEncabezadoCruceFiltro = objListaFiltrada
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al filtrar la información de las ventas.", Me.ToString(), "FiltrarVentas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados asincrónicos del encabezado - REQUERIDO"
    ''' <history>
    ''' ID caso de prueba: CP0002, CP0003
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Enero 5/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Enero 5/2015 - Resultado Ok 
    ''' </history>
    Public Async Function ConsultarLiquidaciones() As Task
        Try
            IsBusy = True

            'JAEZ 20161202  se limpian las listas
            ListaEncabezadoCompras = Nothing
            ListaEncabezadoVentas = Nothing
            ListaEncabezadoCruce = Nothing

            Dim objControlUsuarios As InvokeOperation(Of String)
            Dim objConsecutivo As InvokeOperation(Of Integer)

            If ValidarDatos() Then
                If Not IsNothing(dtmFechaCorte) Then
                    If dtmFechaCorte > Date.Now() Then
                        A2Utilidades.Mensajes.mostrarMensaje("La fecha de corte debe ser igual o menor a la actual.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        dtmFechaCorte = Date.Now()
                        IsBusy = False
                        Exit Function
                    End If
                End If
            Else
                IsBusy = False
                dtmFechaCorte = Date.Now()
                Exit Function
            End If

            HabilitarbtnVolver = False

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            objConsecutivo = Await mdcProxy.ConsultarConsecutivoSync("PROCESO_TITULOS", Program.Usuario, Program.HashConexion).AsTask()

            If objConsecutivo.Value <> -1 Then
                intConsecutivo = objConsecutivo.Value
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("No existe el consecutivo PROCESO_TITULOS en el maestro de consecutivos, debe crearlo primero", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            mdcProxy.TitulosCruces.Clear()
            TabSeleccionado = TAB_TITULOS_COMPRADOS

            objControlUsuarios = Await mdcProxy.GuardarUsuarioProcesoTitulosSync("S", intConsecutivo, "", Program.Usuario, Program.HashConexion).AsTask()

            If objControlUsuarios.Value <> String.Empty Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje(objControlUsuarios.Value, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            Else
                Await mdcProxy.GuardarUsuarioProcesoTitulosSync("I", intConsecutivo, "", Program.Usuario, Program.HashConexion).AsTask()
            End If

            strFiltroCompras = String.Empty
            strFiltroVentas = String.Empty

            If (strClase = "" Or strClase = "T") And lngIDComitente = "" And strIdEspecie = "" Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensajePregunta("Esta operación puede tardar varios minutos, ¿Desea continuar?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarConsultarLiquidaciones)
            Else
                Await mdcProxy.GuardarUsuarioProcesoTitulosSync("U", intConsecutivo, "", Program.Usuario, Program.HashConexion).AsTask()

                Await ConsultarLiquidacionesCompra()
                Await ConsultarLiquidacionesVenta()

                MyBase.CambioItem("ListaEncabezadoCompras")
                MyBase.CambioItem("ListaEncabezadoVentas")

                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar liquidaciones", Me.ToString(), "ConsultarLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSEXPORTAR) As String
        Return String.Format("[[{0}]]", pintTipoCampo.ToString)
    End Function

    Public Sub ExportarPortafolioExcel()
        Try
            If HabilitarbtnExportarExcel Then
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿Exportar portafolio a Excel?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarExportarPortafolio)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso método exportar portafolio excel", _
                                                             Me.ToString(), "ExportarPortafolioExcel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Consultar parametros"

    ''' <summary>
    ''' Método para consultar el parámetro que indica si activa popup para los movimientos Deceval.
    ''' </summary>
    Public Sub ConsultarParametros()
        Try
            Dim dcProxyUtil As UtilidadesDomainContext

            dcProxyUtil = inicializarProxyUtilidadesOYD()

            dcProxyUtil.Verificaparametro("ARCH_TITULOS_MVTOS", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "ARCH_TITULOS_MVTOS")
            dcProxyUtil.Load(dcProxyUtil.listaVerificaparametroQuery(String.Empty, "DECEVAL", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarListaParametros, "DECEVAL")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método consultar parámetros", _
                                                             Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados en la tabla de parámetros.
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState.ToString
                Case "ARCH_TITULOS_MVTOS"
                    STRARCH_TITULOS_MVTOS = lo.Value.ToString
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados en la tabla de parámetros.
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    Private Sub TerminoConsultarListaParametros(ByVal lo As LoadOperation(Of valoresparametro))
        If Not lo.HasError Then
            For Each li In lo.Entities
                Select Case li.Parametro
                    Case "DECEVALSOLICITARARCHIVOS"
                        If li.Valor.ToUpper = "SI" Then
                            MostrarSolicitudArchivoDeceval = True
                        End If
                    Case "DECEVALRUTAINTERNAUPLOADSMOVIMIENTOS"
                        RutaInternaUploadsArchivosDeceval = li.Valor
                    Case "DECEVALCODIGOEXTERNOARCHIVOMOVIMIENTOS"
                        intIDSolicitudArchivoDeceval = CInt(li.Valor)
                    Case "DECEVALCODIGOINTERNOARCHIVOMOVIMIENTOS"
                        strCodigoSolicitudArchivoDeceval = li.Valor
                End Select
            Next
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If

        IsBusyArchivos = False
    End Sub

#End Region

End Class