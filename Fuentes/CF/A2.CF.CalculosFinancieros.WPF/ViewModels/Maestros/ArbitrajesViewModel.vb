Imports Telerik.Windows.Controls

Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Text.RegularExpressions
Imports System.Object
Imports System.Globalization.CultureInfo

Public Class ArbitrajesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As Arbitrajes
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mobjVM As ArbitrajesViewModel
    Dim cwArbitrajesView As cwArbitrajesView

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As Arbitrajes
    Private mobjDetallePorDefecto As ArbitrajesDetalle
    Private strDatosPortafolioADR As String
    Private strAccion As String
    Private mlngIDPortafolioADRAnterior As String
    Private mstrNombrePortafolioADRAnterior As String
    Private mstrIDOperacionCruceAnterior As String
    Private mstrDatosOperacionCruceAnterior As String
    Private mlngIDPortafolioReplicaAnterior As String
    Private mstrNombrePortafolioReplicaAnterior As String
    Private mlngIDPortafolioTrasladoAnterior As String
    Private mstrNombrePortafolioTrasladoAnterior As String
    Private mstrDepositoReplicaBuscadorAnterior As String
    Private mstrDepositoTrasladoBuscadorAnterior As String
    Private mstrIDCuentaDepositoReplicaBuscadorAnterior As String
    Private mstrIDCuentaDepositoTrasladoBuscadorAnterior As String

    Private mdtmFechaCierrePortafolioADR As DateTime? = Nothing
    Dim dtmFechaCierre As DateTime? = Nothing
    Dim logNuevo As Boolean = False
    Dim logEditar As Boolean = False

#End Region

#Region "Inicialización - REQUERIDO"

    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()
                ConsultarDetallePorDefectoSync()

                Await ConsultarConceptoTitulo()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

                'dtmFechaProceso = Date.Now
                IsBusy = False

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

    Private _dtmFechaProceso As System.Nullable(Of System.DateTime)
    Public Property dtmFechaProceso() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaProceso
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaProceso = value
            MyBase.CambioItem("dtmFechaProceso")
        End Set
    End Property

    Private _logConstruir As Boolean = False
    Public Property logConstruir() As Boolean
        Get
            Return _logConstruir
        End Get
        Set(ByVal value As Boolean)
            _logConstruir = value
            MyBase.CambioItem("logConstruir")
        End Set
    End Property

    ''' <summary>
    ''' Lista de ArbitrajesDetalle que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of Arbitrajes)
    Public Property ListaEncabezado() As EntitySet(Of Arbitrajes)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of Arbitrajes))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Arbitrajes para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de Arbitrajes que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As Arbitrajes
    Public Property EncabezadoSeleccionado() As Arbitrajes
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As Arbitrajes)

            Dim logIncializarDet As Boolean = False

            _EncabezadoSeleccionado = value

            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intIDArbitrajes > 0 Then

                    If Not IsNothing(_EncabezadoSeleccionado.intIDEstadosConceptoTitulos) Then
                        strEstadoConceptoSeleccionado = CStr(_EncabezadoSeleccionado.intIDEstadosConceptoTitulos)
                    End If

                    ConsultarDetalle(_EncabezadoSeleccionado.intIDArbitrajes)
                Else
                    logIncializarDet = True
                End If
            End If

            If logIncializarDet Then
                ListaDetalle = Nothing
            End If

            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try

            If Not IsNothing(EncabezadoSeleccionado) Then

                If Not String.IsNullOrEmpty(EncabezadoSeleccionado.strIDEspecie) And Not IsNothing(EncabezadoSeleccionado.dtmFechaProceso) And Editando Then
                    HabilitarEdicionDetalle = True
                    HabilitarBorrarDetalle = True
                Else
                    HabilitarEdicionDetalle = False
                    HabilitarBorrarDetalle = False
                End If

                If EncabezadoSeleccionado.logConstruir Then
                    strAccion = "1"
                Else
                    strAccion = "0"
                End If

                strDatosEncabezado = "DATOS_REPLICA!" & Format(EncabezadoSeleccionado.dtmFechaProceso, "yyyyMMdd") & _
                                     "!" & strAccion & "!" & EncabezadoSeleccionado.strIDEspecie

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los botones del detalle.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of ArbitrajesDetalle)
    Public Property ListaDetalle() As List(Of ArbitrajesDetalle)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of ArbitrajesDetalle))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    Private Sub _ListaDetalle_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
        Try

            If Not IsNothing(ListaDetalle) Then
                If e.PropertyName = "ListaDetalle" Then
                    dblNominalSeleccionado = CDbl(ListaDetalle.Where(Function(i) CType(i.logSeleccionado, Boolean) = True).Sum(Function(i) i.dblCantidadOperacion))
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los botones del detalle.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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
    Private WithEvents _DetalleSeleccionado As ArbitrajesDetalle
    Public Property DetalleSeleccionado() As ArbitrajesDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As ArbitrajesDetalle)
            _DetalleSeleccionado = value
            If Not IsNothing(_DetalleSeleccionado) Then

                If logNuevo = False And logEditar = False Then

                    lngIDPortafolioADR = _DetalleSeleccionado.lngIDPortafolioADR
                    strNombrePortafolioADR = _DetalleSeleccionado.strNombrePortafolioADR
                    lngIDPortafolioReplica = _DetalleSeleccionado.lngIDComitenteReplica
                    strNombrePortafolioReplica = _DetalleSeleccionado.strNombrePortafolioReplica
                    lngIDPortafolioTraslado = _DetalleSeleccionado.lngIDComitenteTraslado
                    strNombrePortafolioTraslado = _DetalleSeleccionado.strNombrePortafolioTraslado
                    intIDLiquidacionesCruce = Convert.ToInt32(_DetalleSeleccionado.intIDLiquidacionesCruce)
                    strIDOperacionCruce = _DetalleSeleccionado.intIDLiquidacionesCruce.ToString
                    strDatosOperacionCruce = _DetalleSeleccionado.strDatosOperacionCruce
                    strDepositoReplicaBuscador = _DetalleSeleccionado.strDepositoReplica
                    strDepositoTrasladoBuscador = _DetalleSeleccionado.strDepositoTraslado
                    strIDCuentaDepositoReplicaBuscador = _DetalleSeleccionado.lngIDCuentaDepositoReplica.ToString
                    strNombreDepositoReplicaBuscador = _DetalleSeleccionado.strNombreDepositoReplicaBuscador
                    strIDCuentaDepositoTrasladoBuscador = _DetalleSeleccionado.lngIDCuentaDepositoTraslado.ToString
                    strNombreDepositoTrasladoBuscador = _DetalleSeleccionado.strNombreDepositoTrasladoBuscador

                ElseIf _DetalleSeleccionado.logSeleccionado = True And logNuevo = True Then

                    If Not IsNothing(_DetalleSeleccionado.intIDLiquidacionesCruce) Then

                        lngIDPortafolioReplica = _DetalleSeleccionado.lngIDComitenteReplica
                        strNombrePortafolioReplica = _DetalleSeleccionado.strNombrePortafolioReplica
                        lngIDPortafolioTraslado = _DetalleSeleccionado.lngIDComitenteTraslado
                        strNombrePortafolioTraslado = _DetalleSeleccionado.strNombrePortafolioTraslado
                        intIDLiquidacionesCruce = Convert.ToInt32(_DetalleSeleccionado.intIDLiquidacionesCruce)
                        strIDOperacionCruce = _DetalleSeleccionado.intIDLiquidacionesCruce.ToString
                        strDatosOperacionCruce = _DetalleSeleccionado.strDatosOperacionCruce
                        strDepositoReplicaBuscador = _DetalleSeleccionado.strDepositoReplica
                        strDepositoTrasladoBuscador = _DetalleSeleccionado.strDepositoTraslado
                        strIDCuentaDepositoReplicaBuscador = _DetalleSeleccionado.lngIDCuentaDepositoReplica.ToString
                        strNombreDepositoReplicaBuscador = _DetalleSeleccionado.strNombreDepositoReplicaBuscador
                        strIDCuentaDepositoTrasladoBuscador = _DetalleSeleccionado.lngIDCuentaDepositoTraslado.ToString
                        strNombreDepositoTrasladoBuscador = _DetalleSeleccionado.strNombreDepositoTrasladoBuscador
                    End If

                ElseIf _DetalleSeleccionado.logSeleccionado = True And logEditar = True Then
                    If Not IsNothing(_DetalleSeleccionado.intIDLiquidacionesCruce) Then
                        lngIDPortafolioReplica = _DetalleSeleccionado.lngIDComitenteReplica
                        strNombrePortafolioReplica = _DetalleSeleccionado.strNombrePortafolioReplica
                        lngIDPortafolioTraslado = _DetalleSeleccionado.lngIDComitenteTraslado
                        strNombrePortafolioTraslado = _DetalleSeleccionado.strNombrePortafolioTraslado
                        intIDLiquidacionesCruce = Convert.ToInt32(_DetalleSeleccionado.intIDLiquidacionesCruce)
                        strIDOperacionCruce = _DetalleSeleccionado.intIDLiquidacionesCruce.ToString
                        strDatosOperacionCruce = _DetalleSeleccionado.strDatosOperacionCruce
                        strDepositoReplicaBuscador = _DetalleSeleccionado.strDepositoReplica
                        strDepositoTrasladoBuscador = _DetalleSeleccionado.strDepositoTraslado
                        strIDCuentaDepositoReplicaBuscador = _DetalleSeleccionado.lngIDCuentaDepositoReplica.ToString
                        strNombreDepositoReplicaBuscador = _DetalleSeleccionado.strNombreDepositoReplicaBuscador
                        strIDCuentaDepositoTrasladoBuscador = _DetalleSeleccionado.lngIDCuentaDepositoTraslado.ToString
                        strNombreDepositoTrasladoBuscador = _DetalleSeleccionado.strNombreDepositoTrasladoBuscador
                    End If

                End If

            End If
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    Private Sub _DetalleSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleSeleccionado.PropertyChanged
        Try

            If Not IsNothing(DetalleSeleccionado) Then
                If e.PropertyName = "logSeleccionado" Then
                    dblNominalSeleccionado = CDbl(ListaDetalle.Where(Function(i) CType(i.logSeleccionado, Boolean) = True).Sum(Function(i) i.dblCantidadOperacion))
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los botones del detalle.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaArbitrajes
    Public Property cb() As CamposBusquedaArbitrajes
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaArbitrajes)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

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

    Private _HabilitarEdicionDetalle As Boolean = False
    Public Property HabilitarEdicionDetalle() As Boolean
        Get
            Return _HabilitarEdicionDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionDetalle = value
            MyBase.CambioItem("HabilitarEdicionDetalle")
        End Set
    End Property

    Private _HabilitarBorrarDetalle As Boolean = False
    Public Property HabilitarBorrarDetalle() As Boolean
        Get
            Return _HabilitarBorrarDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBorrarDetalle = value
            MyBase.CambioItem("HabilitarBorrarDetalle")
        End Set
    End Property

    Private _logSeleccionarTodos As Boolean
    Public Property logSeleccionarTodos() As Boolean
        Get
            Return _logSeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _logSeleccionarTodos = value
            SeleccionarTodos(_logSeleccionarTodos)
            MyBase.CambioItem("logSeleccionarTodos")
        End Set
    End Property

    Private _ListaEstadoConcepto As List(Of CFMaestros.EstadosConceptoTitulos)
    Public Property ListaEstadoConcepto() As List(Of CFMaestros.EstadosConceptoTitulos)
        Get
            Return _ListaEstadoConcepto
        End Get
        Set(ByVal value As List(Of CFMaestros.EstadosConceptoTitulos))
            _ListaEstadoConcepto = value

            MyBase.CambioItem("ListaEstadoConcepto")
        End Set
    End Property

    Private _strEstadoConceptoSeleccionado As String
    Public Property strEstadoConceptoSeleccionado() As String
        Get
            Return _strEstadoConceptoSeleccionado
        End Get
        Set(ByVal value As String)
            _strEstadoConceptoSeleccionado = value
            MyBase.CambioItem("strEstadoConceptoSeleccionado")
        End Set
    End Property

    Private _lngIDPortafolioADR As String
    Public Property lngIDPortafolioADR() As String
        Get
            Return _lngIDPortafolioADR
        End Get
        Set(ByVal value As String)
            _lngIDPortafolioADR = value
            'If Not IsNothing(_lngIDPortafolioADR) Then
            '    ObtenerFechaCierrePortafolio(_lngIDPortafolioADR)
            'End If
            MyBase.CambioItem("lngIDPortafolioADR")
        End Set
    End Property

    Private _strNombrePortafolioADR As String
    Public Property strNombrePortafolioADR() As String
        Get
            Return _strNombrePortafolioADR
        End Get
        Set(ByVal value As String)
            _strNombrePortafolioADR = value
            MyBase.CambioItem("strNombrePortafolioADR")
        End Set
    End Property

    Private _lngIDPortafolioTraslado As String
    Public Property lngIDPortafolioTraslado() As String
        Get
            Return _lngIDPortafolioTraslado
        End Get
        Set(ByVal value As String)
            _lngIDPortafolioTraslado = value
            MyBase.CambioItem("lngIDPortafolioTraslado")
        End Set
    End Property

    Private _strNombrePortafolioTraslado As String
    Public Property strNombrePortafolioTraslado() As String
        Get
            Return _strNombrePortafolioTraslado
        End Get
        Set(ByVal value As String)
            _strNombrePortafolioTraslado = value
            MyBase.CambioItem("strNombrePortafolioTraslado")
        End Set
    End Property

    Private _lngIDPortafolioReplica As String
    Public Property lngIDPortafolioReplica() As String
        Get
            Return _lngIDPortafolioReplica
        End Get
        Set(ByVal value As String)
            _lngIDPortafolioReplica = value
            MyBase.CambioItem("lngIDPortafolioReplica")
        End Set
    End Property

    Private _strNombrePortafolioReplica As String
    Public Property strNombrePortafolioReplica() As String
        Get
            Return _strNombrePortafolioReplica
        End Get
        Set(ByVal value As String)
            _strNombrePortafolioReplica = value
            MyBase.CambioItem("strNombrePortafolioReplica")
        End Set
    End Property

    Private _dblNominalSeleccionado As Double
    Public Property dblNominalSeleccionado() As Double
        Get
            Return _dblNominalSeleccionado
        End Get
        Set(ByVal value As Double)
            _dblNominalSeleccionado = value
            MyBase.CambioItem("dblNominalSeleccionado")
        End Set
    End Property

    Private _strDatosOperacionCruce As String
    Public Property strDatosOperacionCruce() As String
        Get
            Return _strDatosOperacionCruce
        End Get
        Set(ByVal value As String)
            _strDatosOperacionCruce = value
            MyBase.CambioItem("strDatosOperacionCruce")
        End Set
    End Property

    Private _intIDLiquidacionesCruce As Integer
    Public Property intIDLiquidacionesCruce() As Integer
        Get
            Return _intIDLiquidacionesCruce
        End Get
        Set(ByVal value As Integer)
            _intIDLiquidacionesCruce = value
            MyBase.CambioItem("intIDLiquidacionesCruce")
        End Set
    End Property

    Private _strOrigenCruce As String
    Public Property strOrigenCruce() As String
        Get
            Return _strOrigenCruce
        End Get
        Set(ByVal value As String)
            _strOrigenCruce = value
            MyBase.CambioItem("strOrigenCruce")
        End Set
    End Property

    Private _strDatosEncabezado As String
    Public Property strDatosEncabezado() As String
        Get
            Return _strDatosEncabezado
        End Get
        Set(ByVal value As String)
            _strDatosEncabezado = value
            MyBase.CambioItem("strDatosEncabezado")
        End Set
    End Property

    Private _strNombreDepositoReplicaBuscador As String
    Public Property strNombreDepositoReplicaBuscador() As String
        Get
            Return _strNombreDepositoReplicaBuscador
        End Get
        Set(ByVal value As String)
            _strNombreDepositoReplicaBuscador = value
            MyBase.CambioItem("strNombreDepositoReplicaBuscador")
        End Set
    End Property

    Private _strDepositoReplicaBuscador As String
    Public Property strDepositoReplicaBuscador() As String
        Get
            Return _strDepositoReplicaBuscador
        End Get
        Set(ByVal value As String)
            _strDepositoReplicaBuscador = value
            MyBase.CambioItem("strDepositoReplicaBuscador")
        End Set
    End Property

    Private _strIDCuentaDepositoReplicaBuscador As String
    Public Property strIDCuentaDepositoReplicaBuscador() As String
        Get
            Return _strIDCuentaDepositoReplicaBuscador
        End Get
        Set(ByVal value As String)
            _strIDCuentaDepositoReplicaBuscador = value
            MyBase.CambioItem("strIDCuentaDepositoReplicaBuscador")
        End Set
    End Property

    Private _strNombreDepositoTrasladoBuscador As String
    Public Property strNombreDepositoTrasladoBuscador() As String
        Get
            Return _strNombreDepositoTrasladoBuscador
        End Get
        Set(ByVal value As String)
            _strNombreDepositoTrasladoBuscador = value
            MyBase.CambioItem("strNombreDepositoTrasladoBuscador")
        End Set
    End Property

    Private _strDepositoTrasladoBuscador As String
    Public Property strDepositoTrasladoBuscador() As String
        Get
            Return _strDepositoTrasladoBuscador
        End Get
        Set(ByVal value As String)
            _strDepositoTrasladoBuscador = value
            MyBase.CambioItem("strDepositoTrasladoBuscador")
        End Set
    End Property

    Private _strIDCuentaDepositoTrasladoBuscador As String
    Public Property strIDCuentaDepositoTrasladoBuscador() As String
        Get
            Return _strIDCuentaDepositoTrasladoBuscador
        End Get
        Set(ByVal value As String)
            _strIDCuentaDepositoTrasladoBuscador = value
            MyBase.CambioItem("strIDCuentaDepositoTrasladoBuscador")
        End Set
    End Property

    Private _strIDOperacionCruce As String
    Public Property strIDOperacionCruce() As String
        Get
            Return _strIDOperacionCruce
        End Get
        Set(ByVal value As String)
            _strIDOperacionCruce = value
            MyBase.CambioItem("strIDOperacionCruce")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New Arbitrajes
        logNuevo = True

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of Arbitrajes)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIDArbitrajes = -1
                objNvoEncabezado.strIDEspecie = String.Empty
                objNvoEncabezado.logConstruir = True
                objNvoEncabezado.dtmFechaProceso = Date.Now
            End If

            strEstadoConceptoSeleccionado = String.Empty
            dblNominalSeleccionado = 0
            lngIDPortafolioADR = String.Empty
            strNombrePortafolioADR = String.Empty
            strDatosOperacionCruce = String.Empty
            strDatosOperacionCruce = String.Empty
            strIDOperacionCruce = String.Empty
            lngIDPortafolioReplica = String.Empty
            strNombrePortafolioReplica = String.Empty
            strDepositoReplicaBuscador = String.Empty
            strNombreDepositoReplicaBuscador = String.Empty
            strIDCuentaDepositoReplicaBuscador = String.Empty
            lngIDPortafolioTraslado = String.Empty
            strNombrePortafolioTraslado = String.Empty
            strDepositoTrasladoBuscador = String.Empty
            strNombreDepositoTrasladoBuscador = String.Empty
            strIDCuentaDepositoTrasladoBuscador = String.Empty

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            HabilitarEncabezado = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
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
            If Not IsNothing(cb.strIDEspecie) Or Not IsNothing(cb.logConstruir) Or Not IsNothing(cb.dtmFechaProceso) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.strIDEspecie, cb.logConstruir, cb.dtmFechaProceso)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    Public Overrides Async Sub ActualizarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            Dim xmlCompleto As String
            Dim xmlDetalle As String
            ErrorForma = String.Empty

            If ValidarRegistro() Then

                If ValidarDetalle() Then

                    xmlCompleto = "<ArbitrajesDetalle>"

                    For Each objeto In (From c In ListaDetalle).Where(Function(i) CType(i.logSeleccionado, Boolean) = True)

                        xmlDetalle = "<Detalle intIDArbitrajesDetalle=""" & objeto.intIDArbitrajesDetalle &
                                    """ intIDLiquidacionesOperacion = """ & objeto.intIDLiquidacionesOperacion &
                                    """ strOrigenOperacion=""" & objeto.strOrigenOperacion &
                                    """ intIDLiquidacionesCruce=""" & objeto.intIDLiquidacionesCruce &
                                    """ strOrigenCruce=""" & objeto.strOrigenCruce &
                                    """ lngIDComitenteReplica=""" & objeto.lngIDComitenteReplica &
                                    """ strDepositoReplica=""" & objeto.strDepositoReplica &
                                    """ lngIDCuentaDepositoReplica=""" & objeto.lngIDCuentaDepositoReplica &
                                    """ lngIDComitenteTraslado=""" & objeto.lngIDComitenteTraslado &
                                    """ strDepositoTraslado=""" & objeto.strDepositoTraslado &
                                    """ lngIDCuentaDepositoTraslado=""" & objeto.lngIDCuentaDepositoTraslado & """></Detalle>"

                        xmlCompleto = xmlCompleto & xmlDetalle
                    Next

                    xmlCompleto = xmlCompleto & "</ArbitrajesDetalle>"

                    If String.IsNullOrEmpty(strEstadoConceptoSeleccionado) Then
                        strEstadoConceptoSeleccionado = "0"
                    End If

                    IsBusy = True

                    Dim strMsg As String = ""
                    Dim objRet As InvokeOperation(Of String)

                    objRet = Await mdcProxy.Arbitrajes_ActualizarSync(EncabezadoSeleccionado.intIDArbitrajes, EncabezadoSeleccionado.strIDEspecie,
                                                                      EncabezadoSeleccionado.logConstruir, EncabezadoSeleccionado.dtmFechaProceso,
                                                                      xmlCompleto, CType(strEstadoConceptoSeleccionado, Integer?), EncabezadoSeleccionado.logMGC, Program.Usuario, Program.HashConexion).AsTask()

                    If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                        strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                        If Not String.IsNullOrEmpty(strMsg) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        Editando = False
                        HabilitarEncabezado = False
                        HabilitarEdicionDetalle = False
                        HabilitarBorrarDetalle = False

                        strEstadoConceptoSeleccionado = String.Empty
                        dblNominalSeleccionado = 0
                        lngIDPortafolioADR = String.Empty
                        strNombrePortafolioADR = String.Empty
                        strDatosOperacionCruce = String.Empty
                        strIDOperacionCruce = String.Empty
                        lngIDPortafolioReplica = String.Empty
                        strNombrePortafolioReplica = String.Empty
                        strDepositoReplicaBuscador = String.Empty
                        strNombreDepositoReplicaBuscador = String.Empty
                        strIDCuentaDepositoReplicaBuscador = String.Empty
                        lngIDPortafolioTraslado = String.Empty
                        strNombrePortafolioTraslado = String.Empty
                        strDepositoTrasladoBuscador = String.Empty
                        strNombreDepositoTrasladoBuscador = String.Empty
                        strIDCuentaDepositoTrasladoBuscador = String.Empty


                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        Await ConsultarEncabezado(True, String.Empty)
                    End If
                Else
                    HabilitarEncabezado = True
                    HabilitarEdicionDetalle = True
                    HabilitarBorrarDetalle = True
                End If

            Else
                HabilitarEncabezado = True
                HabilitarEdicionDetalle = True
                HabilitarBorrarDetalle = True
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
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            logEditar = True

            _EncabezadoSeleccionado.strUsuario = Program.Usuario

            If EncabezadoSeleccionado.logConstruir Then
                strAccion = "1"
            Else
                strAccion = "0"
            End If

            strDatosEncabezado = "DATOS_REPLICA!" & Format(EncabezadoSeleccionado.dtmFechaProceso, "yyyyMMdd") & _
                     "!" & strAccion & "!" & EncabezadoSeleccionado.strIDEspecie

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()
            If DetalleSeleccionado IsNot Nothing Then
                mlngIDPortafolioADRAnterior = DetalleSeleccionado.lngIDPortafolioADR
                mstrNombrePortafolioADRAnterior = DetalleSeleccionado.strNombrePortafolioADR
                mstrIDOperacionCruceAnterior = DetalleSeleccionado.strIDOperacionCruce
                mstrDatosOperacionCruceAnterior = DetalleSeleccionado.strDatosOperacionCruce
                mlngIDPortafolioReplicaAnterior = DetalleSeleccionado.lngIDComitenteReplica
                mstrNombrePortafolioReplicaAnterior = DetalleSeleccionado.strNombrePortafolioReplica
                mlngIDPortafolioTrasladoAnterior = DetalleSeleccionado.lngIDComitenteTraslado
                mstrNombrePortafolioTrasladoAnterior = DetalleSeleccionado.strNombrePortafolioTraslado
                mstrDepositoReplicaBuscadorAnterior = DetalleSeleccionado.strDepositoReplica
                mstrDepositoTrasladoBuscadorAnterior = DetalleSeleccionado.strDepositoTraslado
                mstrIDCuentaDepositoReplicaBuscadorAnterior = DetalleSeleccionado.lngIDCuentaDepositoReplica.ToString
                mstrIDCuentaDepositoTrasladoBuscadorAnterior = DetalleSeleccionado.lngIDCuentaDepositoTraslado.ToString
                Editando = True
                MyBase.CambioItem("Editando")
            End If

            HabilitarEncabezado = True
            HabilitarEdicionDetalle = True
            HabilitarBorrarDetalle = True

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
                mdcProxy.RejectChanges()

                Editando = False
                logNuevo = False
                logEditar = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                lngIDPortafolioADR = mlngIDPortafolioADRAnterior
                strNombrePortafolioADR = mstrNombrePortafolioADRAnterior
                strIDOperacionCruce = mstrIDOperacionCruceAnterior
                strDatosOperacionCruce = mstrDatosOperacionCruceAnterior
                HabilitarEncabezado = False
                HabilitarEdicionDetalle = False
                HabilitarBorrarDetalle = False
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
    ''' </summary>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra el registro seleccionado. ¿Confirma el borrado de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Ejecuta el proceso de eliminación en la base de datos
    ''' </summary>
    Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.Arbitrajes.Where(Function(i) i.intIDArbitrajes = EncabezadoSeleccionado.intIDArbitrajes).Count > 0 Then
                        mdcProxy.Arbitrajes.Remove(mdcProxy.Arbitrajes.Where(Function(i) i.intIDArbitrajes = EncabezadoSeleccionado.intIDArbitrajes).First)
                    End If

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)

                    Await ConsultarEncabezado(True, String.Empty)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ActualizarDetalle()
        Try
            If Not IsNothing(DetalleSeleccionado) Then

                strDatosPortafolioADR = "!" + _lngIDPortafolioADR

                cwArbitrajesView = New cwArbitrajesView(Me, DetalleSeleccionado, mobjDetallePorDefecto, EncabezadoSeleccionado, HabilitarEncabezado, strDatosPortafolioADR)
                Program.Modal_OwnerMainWindowsPrincipal(cwArbitrajesView)
                cwArbitrajesView.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No es posible editar este detalle.", "ActualizarDetalle", wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la configuración arbitraje.", Me.ToString(), "ActualizarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle 
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle para que el usuario ingrese el nuevo detalle
    ''' </summary>
    Public Sub IngresarDetalle()
        Try
            strDatosPortafolioADR = "!" + _lngIDPortafolioADR

            cwArbitrajesView = New cwArbitrajesView(Me, Nothing, mobjDetallePorDefecto, EncabezadoSeleccionado, HabilitarEncabezado, strDatosPortafolioADR)
            Program.Modal_OwnerMainWindowsPrincipal(cwArbitrajesView)
            cwArbitrajesView.ShowDialog()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la operación interbancaria", Me.ToString(), "IngresarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle 
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    Public Sub BorrarDetalle()
        Try

            If Not IsNothing(DetalleSeleccionado) Then

                If _ListaDetalle.Where(Function(i) i.intIDArbitrajesDetalle = _DetalleSeleccionado.intIDArbitrajesDetalle).Count > 0 Then
                    _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) i.intIDArbitrajesDetalle = _DetalleSeleccionado.intIDArbitrajesDetalle).First)
                End If

                Dim objNuevaListaDetalle As New List(Of ArbitrajesDetalle)

                For Each li In _ListaDetalle
                    objNuevaListaDetalle.Add(li)
                Next

                If objNuevaListaDetalle.Where(Function(i) i.intIDArbitrajesDetalle = _DetalleSeleccionado.intIDArbitrajesDetalle).Count > 0 Then
                    objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intIDArbitrajesDetalle = _DetalleSeleccionado.intIDArbitrajesDetalle).First)
                End If

                ListaDetalle = objNuevaListaDetalle

                If Not IsNothing(_ListaDetalle) Then
                    If _ListaDetalle.Count > 0 Then
                        DetalleSeleccionado = _ListaDetalle.First
                    End If
                End If

            Else
                A2Utilidades.Mensajes.mostrarMensaje("No es posible borrar este detalle o no existe.", "BorrarDetalle", wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de configuración arbitraje.", Me.ToString(), "BorrarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function BuscarOperaciones() As Task
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of ArbitrajesDetalle)

        Try
            IsBusy = True
            ErrorForma = String.Empty

            If Not mdcProxy.ArbitrajesDetalles Is Nothing Then
                mdcProxy.ArbitrajesDetalles.Clear()
            End If

            mdcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await mdcProxy.Load(mdcProxy.Arbitrajes_Operaciones_ConsultarSyncQuery(EncabezadoSeleccionado.dtmFechaProceso, EncabezadoSeleccionado.strIDEspecie, EncabezadoSeleccionado.logConstruir, False, lngIDPortafolioADR, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        ListaDetalle = objRet.Entities.ToList
                    End If

                End If
            Else
                ListaDetalle = Nothing
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Consulta el nombre y la fecha de portafolio de un cliente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function ConsultarDatosPortafolio(ByVal plngIDComitente As String, ByVal logEsComitenteTraslado As Boolean) As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            Dim dcProxy As CalculosFinancierosDomainContext

            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarDatosPortafolioSyncQuery(plngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        If logEsComitenteTraslado Then
                            cwArbitrajesView.strNombreComitenteTraslado = objRet.Entities.First.strNombre
                        Else
                            cwArbitrajesView.strNombreComitente = objRet.Entities.First.strNombre
                        End If
                    Else
                        If logEsComitenteTraslado Then
                            cwArbitrajesView.lngIDComitenteTraslado = Nothing
                            cwArbitrajesView.strNombreComitenteTraslado = Nothing
                        Else
                            cwArbitrajesView.lngIDComitenteReplica = Nothing
                            cwArbitrajesView.strNombreComitente = Nothing
                        End If
                    End If
                End If
            Else
                If logEsComitenteTraslado Then
                    cwArbitrajesView.lngIDComitenteTraslado = Nothing
                    cwArbitrajesView.strNombreComitenteTraslado = Nothing
                Else
                    cwArbitrajesView.lngIDComitenteReplica = Nothing
                    cwArbitrajesView.strNombreComitente = Nothing
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosPortafolio. ", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    'JAEZ 20161121
    'Public Async Sub ObtenerFechaCierrePortafolio(ByVal pstrIdComitente As String)
    '    Try
    '        If String.IsNullOrEmpty(pstrIdComitente) Then
    '            'Return (Nothing)
    '        End If

    '        Dim objRet As InvokeOperation(Of DateTime?)
    '        Dim objProxyUtil As UtilidadesCFDomainContext

    '        objProxyUtil = inicializarProxyUtilidades()

    '        objRet = Await objProxyUtil.ConsultarFechaCierrePortafolioSync(pstrIdComitente).AsTask

    '        If Not objRet Is Nothing Then
    '            If objRet.HasError Then
    '                If objRet.Error Is Nothing Then
    '                    A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar la fecha de cierre del portafolio del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
    '                End If

    '                objRet.MarkErrorAsHandled()
    '            Else
    '                If Not IsNothing(objRet.Value) Then
    '                    mdtmFechaCierrePortafolioADR = CDate(objRet.Value)
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre del portafolio. ", Me.ToString(), "ObtenerFechaCierrePortafolio", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    Public Sub SeleccionarTodos(blnIsChequed As Boolean)
        Try
            If ListaDetalle IsNot Nothing Then
                For Each li In ListaDetalle
                    li.logSeleccionado = blnIsChequed
                Next
                MyBase.CambioItem("ListaDetalle")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cobrar todas las utilidades.", Me.ToString(), "SeleccionarTodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta el nombre y la fecha de portafolio de un cliente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function ConsultarDatosPortafolio(ByVal plngIDComitente As String, ByVal pstrOpcion As String) As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            Dim dcProxy As CalculosFinancierosDomainContext

            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarDatosPortafolioSyncQuery(plngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        If pstrOpcion = "PortafolioADR" Then
                            strNombrePortafolioADR = objRet.Entities.First.strNombre
                        ElseIf pstrOpcion = "PortafolioReplica" Then
                            strNombrePortafolioReplica = objRet.Entities.First.strNombre
                        ElseIf pstrOpcion = "PortafolioTraslado" Then
                            strNombrePortafolioTraslado = objRet.Entities.First.strNombre
                        End If
                    Else
                        If pstrOpcion = "PortafolioADR" Then
                            lngIDPortafolioADR = Nothing
                            strNombrePortafolioADR = Nothing
                        ElseIf pstrOpcion = "PortafolioReplica" Then
                            lngIDPortafolioReplica = Nothing
                            strNombrePortafolioReplica = Nothing
                        ElseIf pstrOpcion = "PortafolioTraslado" Then
                            lngIDPortafolioTraslado = Nothing
                            strNombrePortafolioTraslado = Nothing
                        End If
                    End If
                End If
            Else
                If pstrOpcion = "PortafolioADR" Then
                    lngIDPortafolioADR = Nothing
                    strNombrePortafolioADR = Nothing
                ElseIf pstrOpcion = "PortafolioReplica" Then
                    lngIDPortafolioReplica = Nothing
                    strNombrePortafolioReplica = Nothing
                ElseIf pstrOpcion = "PortafolioTraslado" Then
                    lngIDPortafolioTraslado = Nothing
                    strNombrePortafolioTraslado = Nothing
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Sub ReplicarSeleccionados()
        Try
            If Not IsNothing(ListaDetalle) Then

                For Each li In ListaDetalle.Where(Function(i) CType(i.logSeleccionado, Boolean) = True)

                    If Not String.IsNullOrEmpty(lngIDPortafolioReplica) Then
                        If IsNothing(li.lngIDComitenteReplica) Then
                            li.lngIDComitenteReplica = lngIDPortafolioReplica
                        End If
                        If String.IsNullOrEmpty(li.strNombrePortafolioReplica) Then
                            li.strNombrePortafolioReplica = strNombrePortafolioReplica
                        End If

                    End If

                    If Not String.IsNullOrEmpty(lngIDPortafolioTraslado) Then
                        If IsNothing(li.lngIDComitenteTraslado) Then
                            li.lngIDComitenteTraslado = lngIDPortafolioTraslado
                        End If

                        If String.IsNullOrEmpty(li.strNombrePortafolioTraslado) Then
                            li.strNombrePortafolioTraslado = strNombrePortafolioTraslado
                        End If

                    End If

                    If Not String.IsNullOrEmpty(strDatosOperacionCruce) Then

                        If String.IsNullOrEmpty(li.strDatosOperacionCruce) Then
                            li.strDatosOperacionCruce = strDatosOperacionCruce
                        End If

                        If IsNothing(li.intIDLiquidacionesCruce) Then
                            li.intIDLiquidacionesCruce = Convert.ToInt32(strIDOperacionCruce)
                        End If

                        If String.IsNullOrEmpty(li.strOrigenCruce) Then
                            li.strOrigenCruce = strOrigenCruce
                        End If

                    End If

                    If Not String.IsNullOrEmpty(strDepositoReplicaBuscador) Then
                        If String.IsNullOrEmpty(li.strDepositoReplica) Then
                            li.strDepositoReplica = strDepositoReplicaBuscador
                        End If

                        If String.IsNullOrEmpty(li.strNombreDepositoReplicaBuscador) Then
                            li.strNombreDepositoReplicaBuscador = strNombreDepositoReplicaBuscador
                        End If
                        If IsNothing(li.lngIDCuentaDepositoReplica) Then
                            li.lngIDCuentaDepositoReplica = CType(strIDCuentaDepositoReplicaBuscador, Integer?)
                        End If

                    End If

                    If Not String.IsNullOrEmpty(strDepositoTrasladoBuscador) Then
                        If String.IsNullOrEmpty(li.strDepositoTraslado) Then
                            li.strDepositoTraslado = strDepositoTrasladoBuscador
                        End If

                        If String.IsNullOrEmpty(li.strNombreDepositoTrasladoBuscador) Then
                            li.strNombreDepositoTrasladoBuscador = strNombreDepositoTrasladoBuscador
                        End If

                        If IsNothing(li.lngIDCuentaDepositoTraslado) Then
                            li.lngIDCuentaDepositoTraslado = CType(strIDCuentaDepositoTrasladoBuscador, Integer?)
                        End If

                    End If
                Next

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cobrar todas las utilidades.", Me.ToString(), "SeleccionarTodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaArbitrajes
            objCB.strIDEspecie = String.Empty
            objCB.logConstruir = True
            objCB.dtmFechaProceso = Date.Now
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
    Private Function ObtenerRegistroAnterior() As Arbitrajes
        Dim objEncabezado As Arbitrajes = New Arbitrajes

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of Arbitrajes)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDArbitrajes = _EncabezadoSeleccionado.intIDArbitrajes
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                'Valida el subyacente
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strIDEspecie) Then
                    strMsg = String.Format("{0}{1} + El subyacente es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la fecha proceso
                If IsNothing(_EncabezadoSeleccionado.dtmFechaProceso) Then
                    strMsg = String.Format("{0}{1} + La fecha de proceso es un campo requerido.", strMsg, vbCrLf)
                End If

                'If Not IsNothing(mdtmFechaCierrePortafolioADR) Then
                '    If (mdtmFechaCierrePortafolioADR >= _EncabezadoSeleccionado.dtmFechaProceso) Then
                '        strMsg = String.Format("{0}{1}El portafolio del cliente " & lngIDPortafolioADR.Trim & "-" & strNombrePortafolioADR & " está cerrado para la fecha de proceso (Fecha de cierre " & Year(CDate(mdtmFechaCierrePortafolioADR)) & "/" & Month(CDate(mdtmFechaCierrePortafolioADR)) & "/" & Day(CDate(mdtmFechaCierrePortafolioADR)) & ", Fecha de proceso " & Year(CDate(_EncabezadoSeleccionado.dtmFechaProceso)) & "/" & Month(CDate(_EncabezadoSeleccionado.dtmFechaProceso)) & "/" & Day(CDate(_EncabezadoSeleccionado.dtmFechaProceso)) & "). ", strMsg, vbCrLf)
                '    End If
                'End If

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

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Private Async Function ConsultarConceptoTitulo() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxyMaestros As MaestrosCFDomainContext = inicializarProxyMaestros()
        Dim objRet As LoadOperation(Of CFMaestros.EstadosConceptoTitulos)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If Not mdcProxyMaestros.EstadosConceptoTitulos Is Nothing Then
                mdcProxyMaestros.EstadosConceptoTitulos.Clear()
            End If

            objRet = Await mdcProxyMaestros.Load(mdcProxyMaestros.FiltrarEstadosConceptoTitulosSyncQuery(pstrFiltro:=String.Empty, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEstadoConcepto = mdcProxyMaestros.EstadosConceptoTitulos.ToList

                End If
            End If

            logResultado = True

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de EstadosConceptoTitulos ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

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
                HabilitarEdicionDetalle = False
                HabilitarBorrarDetalle = False


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

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of Arbitrajes)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.Arbitrajes_ConsultarPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' Consultar de forma sincrónica los datos de Arbitrajes
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrIDEspecie As String = "",
                                               Optional ByVal plogConstruir As System.Nullable(Of System.Boolean) = False,
                                               Optional ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime) = Nothing) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of Arbitrajes)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.Arbitrajes.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.Arbitrajes_FiltrarSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.Arbitrajes_ConsultarSyncQuery(pstrIDEspecie:=pstrIDEspecie,
                                                                                    plogConstruir:=plogConstruir,
                                                                                    pdtmFechaProceso:=pdtmFechaProceso,
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
                    ListaEncabezado = mdcProxy.Arbitrajes

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de Arbitrajes ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle
    ''' </summary>
    Private Async Sub ConsultarDetallePorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of ArbitrajesDetalle)

            objRet = Await dcProxy.Load(dcProxy.ArbitrajesDetalle_ConsultarPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalle(ByVal intIDArbitrajes As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of ArbitrajesDetalle)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.ArbitrajesDetalles Is Nothing Then
                mdcProxy.ArbitrajesDetalles.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ArbitrajesDetalle_ConsultarSyncQuery(intIDArbitrajes, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la operación interbancaria pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la operación interbancaria.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                    Dim objDetalle As New ArbitrajesDetalle
                    objDetalle = ListaDetalle.FirstOrDefault

                    If logNuevo = False Then

                        lngIDPortafolioADR = objDetalle.lngIDPortafolioADR
                        strNombrePortafolioADR = objDetalle.strNombrePortafolioADR
                        strIDOperacionCruce = objDetalle.strIDOperacionCruce
                        strDatosOperacionCruce = objDetalle.strDatosOperacionCruce
                        lngIDPortafolioReplica = objDetalle.lngIDComitenteReplica
                        strNombrePortafolioReplica = objDetalle.strNombrePortafolioReplica
                        lngIDPortafolioTraslado = objDetalle.lngIDComitenteTraslado
                        strNombrePortafolioTraslado = objDetalle.strNombrePortafolioTraslado
                        strDepositoReplicaBuscador = objDetalle.strDepositoReplica
                        strDepositoTrasladoBuscador = objDetalle.strDepositoTraslado
                        strIDCuentaDepositoReplicaBuscador = objDetalle.lngIDCuentaDepositoReplica.ToString
                        strNombreDepositoReplicaBuscador = objDetalle.strNombreDepositoReplicaBuscador
                        strIDCuentaDepositoTrasladoBuscador = objDetalle.lngIDCuentaDepositoTraslado.ToString
                        strNombreDepositoTrasladoBuscador = objDetalle.strNombreDepositoTrasladoBuscador
                    End If
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de la operación interbancaria seleccionada.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ValidarRegistro
    ''' </summary>
    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty
        Dim logExistePortafolioReplica As Boolean = True

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------
            If IsNothing(_ListaDetalle) Then
                strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            ElseIf _ListaDetalle.Count = 0 Then
                strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            ElseIf ListaDetalle.Where(Function(i) CType(i.logSeleccionado, Boolean) = True).Count = 0 Then
                strMsg = String.Format("{0}{1} + Debe existir por lo menos un detalle seleccionado.", strMsg, vbCrLf)
            End If

            If Not EncabezadoSeleccionado.logMGC Then
                If Not IsNothing(_ListaDetalle) Then
                    For Each li In ListaDetalle.Where(Function(i) CType(i.logSeleccionado, Boolean) = True)
                        If String.IsNullOrEmpty(li.lngIDComitenteReplica) Then
                            logExistePortafolioReplica = False
                        End If
                    Next

                    If Not logExistePortafolioReplica Then
                        strMsg = String.Format("{0}{1} + El portafolio replica en el detalle es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If
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

#End Region

End Class

''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
Public Class CamposBusquedaArbitrajes
    Implements INotifyPropertyChanged

    Private _strIDEspecie As String
    Public Property strIDEspecie() As String
        Get
            Return _strIDEspecie
        End Get
        Set(ByVal value As String)
            _strIDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDEspecie"))
        End Set
    End Property

    Private _logConstruir As System.Nullable(Of Boolean) = True
    Public Property logConstruir() As System.Nullable(Of Boolean)
        Get
            Return _logConstruir
        End Get
        Set(ByVal value As System.Nullable(Of Boolean))
            _logConstruir = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logConstruir"))
        End Set
    End Property

    Private _dtmFechaProceso As System.Nullable(Of System.DateTime)
    Public Property dtmFechaProceso() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaProceso
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaProceso = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaProceso"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class





