Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports GalaSoft.MvvmLight.Command

Public Class DinamicaContableConfigViewModel
    Inherits A2ControlMenu.A2ViewModel

    ''' <summary>
    ''' ViewModel para la pantalla Tipos de configuración contable.
    ''' </summary>
    ''' <history>
    ''' Desarrollado por : Cristian Ciceri Muñetón 
    ''' Fecha            : Septiembre/2015
    ''' </history>

#Region "Eventos"

    Public Event CambioItemLista(ByVal value As CFCodificacionContable.DinamicaContableListas, ByVal Nombre As String)

#End Region

#Region "Constantes"

    Private Const MSTR_SEPARADOR_REG As String = "|"
    Private Const MSTR_INACTIVAR_CONFIG As String = "INACTIVAR"

    Public Shadows Enum TiposListasCombos
        Generales
        Normas
        Eventos
        ClasesContables
    End Enum

    Public Enum Detalles
        DinamicaContable
        ConceptosComunes
        ConceptosEspecificos
    End Enum

    Public Enum Estados
        SI
        NO
    End Enum
#End Region

#Region "Variables - REQUERIDO"

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As CFCodificacionContable.DinamicaContableConfig
    Private mobjDetallePorDefecto As CFCodificacionContable.DinamicaContableConfigDinamica

    Private mdcProxy As CodificacionContableDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As CFCodificacionContable.DinamicaContableConfig

    Friend ViewDinamicaContable As DinamicaContableConfigView

    '--------------------------------------------------------------------------------------------------
    '-- Variables para controlar el cambio de clase contable y las cuentas asociadas
    Private mstrClaseContableAnterior As String = String.Empty
    Private mdicCuentasPorClase As Dictionary(Of String, String)

#End Region

#Region "Inicialización - REQUERIDO"

    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando

            mdicCuentasPorClase = New Dictionary(Of String, String)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la construcción del objeto de negocio.", Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
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
                ConsultarEncabezadoPorDefectoSync()

                ' Consultar listas para combos generales
                Await ConsultarListasConfiguracionCombos(TiposListasCombos.Generales, String.Empty, -1, String.Empty)
                cargarCombosGenerales()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaDinamicaContableConfig
    Public Property cb() As CamposBusquedaDinamicaContableConfig
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaDinamicaContableConfig)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    ''' <summary>
    ''' Indica la pestaña del tab de detalle seleccionado. Por defecto se selecciona el primero
    ''' </summary>
    Public Property TabSeleccionado As Short = 0

    ''' <summary>
    ''' Diccionario para combos generales
    ''' </summary>
    Private _CombosGenerales As Dictionary(Of String, List(Of CFCodificacionContable.DinamicaContableListas))
    Public Property CombosGenerales() As Dictionary(Of String, List(Of CFCodificacionContable.DinamicaContableListas))
        Get
            Return _CombosGenerales
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of CFCodificacionContable.DinamicaContableListas)))
            _CombosGenerales = value

            MyBase.CambioItem("CombosGenerales")
        End Set
    End Property

    ''' <summary>
    ''' Listas generales para combos
    ''' </summary>
    Private _ListasGeneralesConfiguracion As List(Of CFCodificacionContable.DinamicaContableListas)
    Public Property ListasGeneralesConfiguracion() As List(Of CFCodificacionContable.DinamicaContableListas)
        Get
            Return _ListasGeneralesConfiguracion
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.DinamicaContableListas))
            _ListasGeneralesConfiguracion = value

            MyBase.CambioItem("ListasGeneralesConfiguracion")
        End Set
    End Property

    ''' <summary>
    ''' Lista de eventos contables para una norma contable y tipo de inversión
    ''' </summary>
    Private _ListaConfiguracionNorma As List(Of CFCodificacionContable.DinamicaContableListas)
    Public Property ListaConfiguracionNorma() As List(Of CFCodificacionContable.DinamicaContableListas)
        Get
            Return _ListaConfiguracionNorma
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.DinamicaContableListas))
            _ListaConfiguracionNorma = value
            MyBase.CambioItem("ListaConfiguracionNorma")
        End Set
    End Property

    ''' <summary>
    ''' Lista de detalles de la dinámica contable para el evento seleccionado
    ''' </summary>
    Private _ListaDetalleDinamica As List(Of CFCodificacionContable.DinamicaContableConfigDinamica)
    Public Property ListaDetalleDinamica() As List(Of CFCodificacionContable.DinamicaContableConfigDinamica)
        Get
            Return _ListaDetalleDinamica
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.DinamicaContableConfigDinamica))
            _ListaDetalleDinamica = value
            MyBase.CambioItem("ListaDetalleDinamica")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles de la dinámica está seleccionado
    ''' </summary>
    Private WithEvents _DetalleDinamicaSeleccionado As CFCodificacionContable.DinamicaContableConfigDinamica
    Public Property DetalleDinamicaSeleccionado() As CFCodificacionContable.DinamicaContableConfigDinamica
        Get
            Return _DetalleDinamicaSeleccionado
        End Get
        Set(ByVal value As CFCodificacionContable.DinamicaContableConfigDinamica)
            _DetalleDinamicaSeleccionado = value
            MyBase.CambioItem("DetalleDinamicaSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Lista de conceptos que son comunes a todas las clases contables (la misma cuenta contable para todas las clases contables)
    ''' </summary>
    Private _ListaDetalleConceptosComunes As List(Of CFCodificacionContable.DinamicaContableConfigConceptosComunes)
    Public Property ListaDetalleConceptosComunes() As List(Of CFCodificacionContable.DinamicaContableConfigConceptosComunes)
        Get
            Return _ListaDetalleConceptosComunes
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.DinamicaContableConfigConceptosComunes))
            _ListaDetalleConceptosComunes = value
            MyBase.CambioItem("ListaDetalleConceptosComunes")
            MyBase.CambioItem("ListaDetalleConceptosComunesPag")
        End Set
    End Property

    ''' <summary>
    ''' Lista paginada de conceptos que son específicos por clase contable (la misma cuenta contable no aplica para todas las clases contables)
    ''' </summary>
    Private _ListaDetalleConceptosComunesPag As PagedCollectionView = Nothing
    Public ReadOnly Property ListaDetalleConceptosComunesPag() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleConceptosComunes) Then
                Dim view = New PagedCollectionView(_ListaDetalleConceptosComunes)
                _ListaDetalleConceptosComunesPag = view
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica cual de los conceptos que son generales por clase contable está seleccionado
    ''' </summary>
    Private WithEvents _DetalleConceptosComunesSeleccionado As CFCodificacionContable.DinamicaContableConfigConceptosComunes
    Public Property DetalleConceptosComunesSeleccionado() As CFCodificacionContable.DinamicaContableConfigConceptosComunes
        Get
            Return _DetalleConceptosComunesSeleccionado
        End Get
        Set(ByVal value As CFCodificacionContable.DinamicaContableConfigConceptosComunes)
            _DetalleConceptosComunesSeleccionado = value
            MyBase.CambioItem("DetalleConceptosComunesSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Lista de conceptos que son específicos por clase contable (la misma cuenta contable no aplica para todas las clases contables)
    ''' </summary>
    Private _ListaDetalleConceptosEspecificos As List(Of CFCodificacionContable.DinamicaContableConfigConceptosEspecificos)
    Public Property ListaDetalleConceptosEspecificos() As List(Of CFCodificacionContable.DinamicaContableConfigConceptosEspecificos)
        Get
            Return _ListaDetalleConceptosEspecificos
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.DinamicaContableConfigConceptosEspecificos))
            _ListaDetalleConceptosEspecificos = value
            MyBase.CambioItem("ListaDetalleConceptosEspecificos")
            MyBase.CambioItem("ListaDetalleConceptosEspecificosPag")
        End Set
    End Property

    ''' <summary>
    ''' Lista paginada de conceptos que son específicos por clase contable (la misma cuenta contable no aplica para todas las clases contables)
    ''' </summary>
    Private _ListaDetalleConceptosEspecificosPag As PagedCollectionView = Nothing
    Public ReadOnly Property ListaDetalleConceptosEspecificosPag() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleConceptosEspecificos) Then
                Dim view = New PagedCollectionView(_ListaDetalleConceptosEspecificos)
                _ListaDetalleConceptosEspecificosPag = view
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica cual de los conceptos que son específicos por clase contable está seleccionado
    ''' </summary>
    Private _DetalleConceptosEspecificosSeleccionado As CFCodificacionContable.DinamicaContableConfigConceptosEspecificos
    Public Property DetalleConceptosEspecificosSeleccionado() As CFCodificacionContable.DinamicaContableConfigConceptosEspecificos
        Get
            Return _DetalleConceptosEspecificosSeleccionado
        End Get
        Set(ByVal value As CFCodificacionContable.DinamicaContableConfigConceptosEspecificos)
            _DetalleConceptosEspecificosSeleccionado = value
            MyBase.CambioItem("DetalleConceptosEspecificosSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Lista de encabezado que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of CFCodificacionContable.DinamicaContableConfig)
    Public Property ListaEncabezado() As EntitySet(Of CFCodificacionContable.DinamicaContableConfig)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of CFCodificacionContable.DinamicaContableConfig))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de CodificacionContableDetalle para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
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
    ''' Elemento de la lista de CodificacionContable que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As CFCodificacionContable.DinamicaContableConfig
    Public Property EncabezadoSeleccionado() As CFCodificacionContable.DinamicaContableConfig
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CFCodificacionContable.DinamicaContableConfig)
            Dim logInicializar As Boolean = False

            If value Is Nothing Then
                _EncabezadoSeleccionado = value
                logInicializar = True
            Else
                If Not _EncabezadoSeleccionado Is Nothing Then
                    If Not _EncabezadoSeleccionado.Equals(value) Then
                        _EncabezadoSeleccionado = value
                        logInicializar = True
                    End If
                Else
                    _EncabezadoSeleccionado = value
                    logInicializar = True
                End If
            End If

            If logInicializar Then
                _EncabezadoSeleccionado = value

                _NormaContableTipoInversionSeleccion = Nothing
                _EventoContableSeleccion = Nothing
                _ClaseContableSeleccion = Nothing

                If Not _ListaDetalleDinamica Is Nothing Then
                    _ListaDetalleDinamica.Clear()
                End If

                If Not _ListaDetalleConceptosComunes Is Nothing Then
                    _ListaDetalleConceptosComunes.Clear()
                End If

                If Not _ListaDetalleConceptosEspecificos Is Nothing Then
                    _ListaDetalleConceptosEspecificos.Clear()
                End If

                _VisibilidadDuplicarDinamica = Visibility.Collapsed

                _DetalleDinamicaSeleccionado = Nothing
                _DetalleConceptosComunesSeleccionado = Nothing
                _DetalleConceptosEspecificosSeleccionado = Nothing

                mdicCuentasPorClase.Clear()

                MyBase.CambioItem("NormaContableTipoInversionSeleccion")
                MyBase.CambioItem("ClaseContableSeleccion")
                MyBase.CambioItem("EventoContableSeleccion")
                MyBase.CambioItem("EncabezadoSeleccionado")
                MyBase.CambioItem("DetalleDinamicaSeleccionado")
                MyBase.CambioItem("DetalleConceptosComunesSeleccionado")
                MyBase.CambioItem("DetalleConceptosEspecificosSeleccionado")

                MyBase.CambioItem("ListaDetalleDinamica")
                MyBase.CambioItem("ListaDetalleConceptosComunes")
                MyBase.CambioItem("ListaDetalleConceptosEspecificos")
                MyBase.CambioItem("ListaDetalleConceptosComunesPag")
                MyBase.CambioItem("ListaDetalleConceptosEspecificosPag")

                MyBase.CambioItem("VisibilidadDuplicarDinamica")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para manejar el IsEnabled en el View
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
    ''' Propiedad para manejar el IsEnabled de la pestaña de la dinámica contable
    ''' </summary>
    Private _HabilitarEstadoConfig As Boolean = False
    Public Property HabilitarEstadoConfig() As Boolean
        Get
            Return _HabilitarEstadoConfig
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEstadoConfig = value
            MyBase.CambioItem("HabilitarEstadoConfig")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para manejar el IsEnabled de la pestaña de la dinámica contable
    ''' </summary>
    Private _HabilitarDetalle As Boolean = True
    Public Property HabilitarDetalle() As Boolean
        Get
            Return _HabilitarDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDetalle = value
            MyBase.CambioItem("HabilitarDetalle")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para manejar el IsEnabled de la pestaña de conceptos contables
    ''' </summary>
    Private _HabilitarDetalleConceptos As Boolean = False
    Public Property HabilitarDetalleConceptos() As Boolean
        Get
            Return _HabilitarDetalleConceptos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDetalleConceptos = value
            MyBase.CambioItem("HabilitarDetalleConceptos")
        End Set
    End Property

    Private _VisibilidadDuplicarDinamica As Visibility = Visibility.Collapsed
    Public Property VisibilidadDuplicarDinamica() As Visibility
        Get
            Return _VisibilidadDuplicarDinamica
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadDuplicarDinamica = value
            MyBase.CambioItem("VisibilidadDuplicarDinamica")
        End Set
    End Property

    ''' <summary>
    ''' Código de la norma contable y tipo de inversión seleccionada 
    ''' </summary>
    Private _NormaContableTipoInversion As String
    Public Property NormaContableTipoInversion() As String
        Get
            Return _NormaContableTipoInversion
        End Get
        Set(ByVal value As String)
            _NormaContableTipoInversion = value
            MyBase.CambioItem("NormaContableTipoInversion")
        End Set
    End Property

    ''' <summary>
    ''' Norma contable y tipo de inversión seleccionada
    ''' </summary>
    Private _NormaContableTipoInversionSeleccion As CFCodificacionContable.DinamicaContableListas
    Public Property NormaContableTipoInversionSeleccion() As CFCodificacionContable.DinamicaContableListas
        Get
            Return _NormaContableTipoInversionSeleccion
        End Get
        Set(ByVal value As CFCodificacionContable.DinamicaContableListas)
            _NormaContableTipoInversionSeleccion = value

            MyBase.CambioItem("NormaContableTipoInversionSeleccion")
        End Set
    End Property

    ''' <summary>
    ''' Norma contable y tipo de inversión seleccionada de solo lectura o selecconable
    ''' </summary>
    Private _NormaContableTipoInversionActiva As Boolean = True
    Public Property NormaContableTipoInversionActiva() As Boolean
        Get
            Return _NormaContableTipoInversionActiva
        End Get
        Set(ByVal value As Boolean)
            _NormaContableTipoInversionActiva = value
            MyBase.CambioItem("NormaContableTipoInversionActiva")
        End Set
    End Property

    ''' <summary>
    ''' Evento contable seleccionado
    ''' </summary>
    Private _EventoContableSeleccion As CFCodificacionContable.DinamicaContableListas
    Public Property EventoContableSeleccion() As CFCodificacionContable.DinamicaContableListas
        Get
            Return _EventoContableSeleccion
        End Get
        Set(ByVal value As CFCodificacionContable.DinamicaContableListas)
            _EventoContableSeleccion = value
            MyBase.CambioItem("EventoContableSeleccion")
        End Set
    End Property

    ''' <summary>
    ''' Clase contable seleccionada
    ''' </summary>
    Private _ClaseContableSeleccion As CFCodificacionContable.DinamicaContableListas
    Public Property ClaseContableSeleccion() As CFCodificacionContable.DinamicaContableListas
        Get
            Return _ClaseContableSeleccion
        End Get
        Set(ByVal value As CFCodificacionContable.DinamicaContableListas)
            _ClaseContableSeleccion = value
            MyBase.CambioItem("ClaseContableSeleccion")
        End Set
    End Property

    ''' <summary>
    ''' Permite indicar el estado actual de la entidad: Verdadero si está en modo de edición o False de lo contrario
    ''' </summary>
    Private _EditandoDetalle As Boolean = True
    Public Property EditandoDetalle() As Boolean
        Get
            Return _EditandoDetalle
        End Get
        Set(ByVal value As Boolean)
            _EditandoDetalle = value
            MyBase.CambioItem("EditandoDetalle")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' 
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New CFCodificacionContable.DinamicaContableConfig

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            NormaContableTipoInversionActiva = False
            HabilitarEstadoConfig = False

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of CFCodificacionContable.DinamicaContableConfig)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.IdDinamicaContableConfig = -1
                objNvoEncabezado.IdCompaniaBasePUC = Nothing
                objNvoEncabezado.NombreConfiguracion = String.Empty
                objNvoEncabezado.TipoCompania = String.Empty
                objNvoEncabezado.Descripcion = String.Empty
                objNvoEncabezado.strInfoSesion = String.Empty
                objNvoEncabezado.strMsgValidacion = String.Empty
                objNvoEncabezado.Activa = String.Empty
                objNvoEncabezado.EstaActiva = True
                objNvoEncabezado.FechaInactivacion = Nothing
                objNvoEncabezado.FechaActualizacion = Now.Date()
            End If

            objNvoEncabezado.Usuario = Program.Usuario

            mobjEncabezadoAnterior = Nothing

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            HabilitarEncabezado = True
            HabilitarEstadoConfig = False
            HabilitarDetalle = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto 
    ''' ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' 
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
    ''' 
    Public Overrides Sub Buscar()
        Try
            PrepararNuevaBusqueda()
            MyBase.Buscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método buscar", Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    ''' 
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If IsNothing(cb.NombreConfiguracion) Or IsNothing(cb.Descripcion) Or IsNothing(cb.Activa) Then
                Await ConsultarEncabezado(False, String.Empty, -1, String.Empty, cb.NombreConfiguracion, cb.Descripcion, cb.Activa)
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
    ''' 
    Public Overrides Async Sub ActualizarRegistro()
        Await GuardarRegistro(False)
    End Sub

    ''' <summary>
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' 
    Private Async Function GuardarRegistro(ByVal plogSoloInactivar As Boolean) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim logNuevo As Boolean = False
        Dim logResultadoValidar As Boolean = False
        Dim strNombreConfiguracion As String = String.Empty
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()
        Dim strDinamicaContable As String = String.Empty
        Dim strConceptosComunes As String = String.Empty
        Dim strConceptosEspecificos As String = String.Empty
        Dim objEnc As CFCodificacionContable.DinamicaContableConfig = Nothing

        Try
            ErrorForma = String.Empty

            IsBusy = True

            If plogSoloInactivar Then
                logResultadoValidar = True
            Else
                logResultadoValidar = ValidarRegistro()
            End If

            If logResultadoValidar Then
                Dim strMsg As String = ""
                Dim objRet As InvokeOperation(Of String) = Nothing

                If plogSoloInactivar Then
                    EncabezadoSeleccionado.Activa = Estados.NO.ToString
                Else

                    If Not _ListaDetalleConceptosComunes Is Nothing Then
                        For Each obj In _ListaDetalleConceptosComunes
                            If Not obj.CuentaContable.Trim.Equals(String.Empty) Then
                                strConceptosComunes = strConceptosComunes & obj.IdConceptoContable & "," & obj.IdNaturaleza & "," & obj.IdTipoCCosto & "," & obj.IdTipoTercero & ",""" & obj.CuentaContable & """" & MSTR_SEPARADOR_REG
                            End If
                        Next

                        ' Eliminar el último separador
                        If Not strConceptosComunes.Equals(String.Empty) Then
                            strConceptosComunes = strConceptosComunes.Substring(0, strConceptosComunes.Length - 1)
                        End If
                    End If

                    '----------------------------------------------------------------------------------------------------
                    ' Conceptos específicos por clase contable
                    mstrClaseContableAnterior = guardarCuentasClaseContable(mstrClaseContableAnterior, String.Empty)

                    If Not mdicCuentasPorClase Is Nothing Then
                        For Each obj In mdicCuentasPorClase
                            strConceptosEspecificos = strConceptosEspecificos & obj.Key & "/" & obj.Value & "#"
                        Next

                        ' Eliminar el último separador
                        If Not strConceptosEspecificos.Equals(String.Empty) Then
                            strConceptosEspecificos = strConceptosEspecificos.Substring(0, strConceptosEspecificos.Length - 1)
                        End If
                    End If
                End If

                strNombreConfiguracion = EncabezadoSeleccionado.NombreConfiguracion ' Guardar el nombre de la configuración para buscar el registro ingresado (cuando es nuevo). El nombre es único.

                If EncabezadoSeleccionado.IdDinamicaContableConfig <= 0 Then
                    logNuevo = True
                End If

                objRet = Await mdcProxy.ActualizarDinamicaContableConfigSync(EncabezadoSeleccionado.IdDinamicaContableConfig, EncabezadoSeleccionado.NombreConfiguracion, EncabezadoSeleccionado.Activa, _
                                                        EncabezadoSeleccionado.Descripcion, NormaContableTipoInversion, strDinamicaContable, strConceptosComunes, strConceptosEspecificos, _
                                                        Program.Usuario, EncabezadoSeleccionado.IdCompaniaBasePUC, EncabezadoSeleccionado.TipoCompania, Program.HashConexion).AsTask()

                If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                    If Not String.IsNullOrEmpty(strMsg) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    Editando = False
                    EditandoDetalle = False
                    HabilitarEncabezado = False
                    HabilitarEstadoConfig = False
                    HabilitarDetalle = True

                    ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                    Await ConsultarEncabezado(True, String.Empty)

                    ' Validar el registro que se debe mostrar
                    logResultadoValidar = True

                    If logNuevo Then
                        If (From obj In ListaEncabezado Where obj.NombreConfiguracion.ToLower() = strNombreConfiguracion.ToLower Select obj).Count = 0 Then
                            logResultadoValidar = False
                        Else
                            objEnc = (From obj In ListaEncabezado Where obj.NombreConfiguracion.ToLower() = strNombreConfiguracion.ToLower Select obj).First
                        End If
                    Else
                        strNombreConfiguracion = String.Empty

                        If mobjEncabezadoAnterior Is Nothing Then
                            logResultadoValidar = False
                        Else
                            If (From obj In ListaEncabezado Where obj.IdDinamicaContableConfig = mobjEncabezadoAnterior.IdDinamicaContableConfig Select obj).Count = 0 Then
                                logResultadoValidar = False
                            Else
                                objEnc = (From obj In ListaEncabezado Where obj.IdDinamicaContableConfig = mobjEncabezadoAnterior.IdDinamicaContableConfig Select obj).First
                            End If
                        End If
                    End If


                    If logResultadoValidar Then
                        EncabezadoSeleccionado = objEnc
                    Else
                        If ListaEncabezado.Count > 0 Then
                            EncabezadoSeleccionado = ListaEncabezado.First
                        Else
                            EncabezadoSeleccionado = Nothing
                        End If
                    End If
                End If

                '---------------------------------------------------------------------------------------
                ' Activar la selección de norma contable y dejar de guardar los cambios de clase contable
                NormaContableTipoInversionActiva = True
                mstrClaseContableAnterior = String.Empty
            Else
                HabilitarEncabezado = True
                HabilitarEstadoConfig = True
            End If

            IsBusy = False

            logResultado = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function


    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then

                If mdcProxy.IsLoading Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Not CBool(_EncabezadoSeleccionado.EstaActiva) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La configuración contable está inactiva y no puede ser modificada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                IsBusy = True

                NormaContableTipoInversionActiva = False

                mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                Editando = True
                MyBase.CambioItem("Editando")

                HabilitarEncabezado = True
                HabilitarEstadoConfig = True
                HabilitarDetalle = True
                HabilitarDetalleConceptos = False

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar la edición del registro seleccionado.", Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxy.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")
                EncabezadoSeleccionado = mobjEncabezadoAnterior

                HabilitarDetalle = True
                HabilitarDetalleConceptos = False
                HabilitarEncabezado = False
                HabilitarEstadoConfig = False
                NormaContableTipoInversionActiva = True
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
    ''' 
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Not CBool(_EncabezadoSeleccionado.EstaActiva) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La configuración contable está inactiva. No puede ser modificada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Una configuración contable no puede ser borrada, solamente inactivada." & vbNewLine & vbNewLine & "¿Desea inactivar la configuración contable " & EncabezadoSeleccionado.NombreConfiguracion & "?", Program.TituloSistema, MSTR_INACTIVAR_CONFIG, AddressOf BorrarRegistroConfirmado)
                End If
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
    ''' 
    Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)

        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    Await GuardarRegistro(True)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos públicos del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Método para capturar el objeto desde el evento ComboBox_DropDownClosedValor en el code behind del combo valor que existe en el detalle.
    ''' </summary>
    ''' 
    Public Sub CerroDropDownValor(sender As Object)

        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE


        Try
            Dim objComboBox As ComboBox = CType(sender, ComboBox)
            If Not IsNothing(objComboBox.SelectedItem) Then
                Dim objNuevo As CFCodificacionContable.DinamicaContableListas = CType(objComboBox.SelectedItem, CFCodificacionContable.DinamicaContableListas)
                'If Not IsNothing(objNuevo.logTotalizado) And objNuevo.logTotalizado And Not IsNothing(objNuevo) Then
                '    cwCodificacionContableValor = New cwCodificacionContableValor(DetalleDinamicaSeleccionado.strCamposTotalizados, True) 'strCWChequeados)
                '    MyBase.CambioItem("ListaComboValor")
                '    AddHandler cwCodificacionContableValor.Closed, AddressOf CerroVentanaCodificacionContableValor
                '    cwCodificacionContableValor.Show()
                'Else
                '    DetalleDinamicaSeleccionado.strCamposTotalizados = String.Empty
                'End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cerrando el control de valor en el detalle", Me.ToString(), "CerroDropDownValor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para capturar el objeto desde el evento ComboBox_DropDownClosedCentroCostos en el code behind del combo centro de costos que existe en el detalle.
    ''' </summary>
    ''' 
    Public Sub CerroDropDownCentroCostos(sender As Object)

        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE





        Try
            Dim objComboBox As ComboBox = CType(sender, ComboBox)
            If Not IsNothing(objComboBox.SelectedItem) Then
                Dim objNuevo As OYDUtilidades.ItemCombo = CType(objComboBox.SelectedItem, OYDUtilidades.ItemCombo)
                'If Not IsNothing(objNuevo.ID) And (objNuevo.ID = STR_CENTRO_COSTOS_FIJO) And Not IsNothing(objNuevo) Then
                '    cwCodificacionContableCCostos = New cwCodificacionContableCCostos(DetalleDinamicaSeleccionado.strCentroCostosFijo)
                '    AddHandler cwCodificacionContableCCostos.Closed, AddressOf CerroVentanaCodificacionContableCCostos
                '    cwCodificacionContableCCostos.Show()
                'Else
                '    DetalleDinamicaSeleccionado.strCentroCostosFijo = String.Empty
                'End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cerrando el control de valor en el detalle", Me.ToString(), "CerroDropDownValor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Método para capturar el valor ingresado desde la venta emergente que se activa al 
    ''' presionar la opción "Fijo" desde el combo centro de costos en el detalle
    ''' </summary>
    ''' 
    Private Sub CerroVentanaCodificacionContableCCostos(sender As Object, e As EventArgs)

        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE
        'OJO: PENDIENTE



        Try
            'If cwCodificacionContableCCostos.DialogResult.Value Then
            '    If Not IsNothing(cwCodificacionContableCCostos.strCentroCostoFijo) Then
            '        'DetalleDinamicaSeleccionado.strCentroCostosFijo = cwCodificacionContableCCostos.strCentroCostoFijo
            '    End If
            'End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana para totalizar el valor", _
                     Me.ToString(), "CerroVentanaCodificacionContableCCostos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ValidarRegistro
    ''' </summary>
    ''' 
    Private Function ValidarDetalle(ByVal pintTipoDetalle As Integer) As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            Select Case pintTipoDetalle
                Case Detalles.ConceptosComunes
                    '------------------------------------------------------------------------------------------------------------------------------------------------
                    '-- Valida que por lo menos exista un detalle para poder crear todo un registro
                    '------------------------------------------------------------------------------------------------------------------------------------------------
                    If IsNothing(_ListaDetalleConceptosComunes) Then
                        strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
                    ElseIf _ListaDetalleConceptosComunes.Count = 0 Then
                        strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
                    Else

                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        logResultado = False
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If

                Case Detalles.ConceptosEspecificos
                    '------------------------------------------------------------------------------------------------------------------------------------------------
                    '-- Valida que por lo menos exista un detalle para poder crear todo un registro
                    '------------------------------------------------------------------------------------------------------------------------------------------------
                    If IsNothing(_ListaDetalleConceptosEspecificos) Then
                        strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
                    ElseIf _ListaDetalleConceptosEspecificos.Count = 0 Then
                        strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
                    Else

                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        logResultado = False
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If

            End Select
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "validarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    ''' 
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaDinamicaContableConfig()

            objCB.Activa = Nothing
            objCB.Descripcion = Nothing
            objCB.NombreConfiguracion = Nothing
            objCB.IdDinamicaContableConfig = -1

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
    ''' 
    Private Function ObtenerRegistroAnterior() As CFCodificacionContable.DinamicaContableConfig
        Dim objEncabezado As CFCodificacionContable.DinamicaContableConfig = New CFCodificacionContable.DinamicaContableConfig

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of CFCodificacionContable.DinamicaContableConfig)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.IdDinamicaContableConfig = _EncabezadoSeleccionado.IdDinamicaContableConfig
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <summary>
    ''' Función para validar todos los campos obligatorios de la pantalla, si la pantalla tiene detalle 
    ''' esta función realizará el llamado a otra función para validar el detalle.
    ''' </summary>
    ''' 
    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty


        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                'Valida la norma contable
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.NombreConfiguracion) Then
                    strMsg = String.Format("{0}{1} + El nombre de la configuración contable es requerido.", strMsg, vbCrLf)
                End If
            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar una configuración contable", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = ValidarDetalle(Detalles.DinamicaContable)
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor ajuste las siguientes inconsistencias para poder guardar los cambios realizados: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged

    End Sub

    Private Sub cargarCombosGenerales()

        Dim strNombreCategoria As String = String.Empty
        Dim objNodosCategoria As List(Of CFCodificacionContable.DinamicaContableListas)
        Dim objDiccionarioNuevo As New Dictionary(Of String, List(Of CFCodificacionContable.DinamicaContableListas))

        Try

            Dim lstCombos = From lc In ListasGeneralesConfiguracion Select lc.Topico Distinct 'Lista de tópicos incluidos en la consulta retornada

            objDiccionarioNuevo = New Dictionary(Of String, List(Of CFCodificacionContable.DinamicaContableListas))

            For Each Topico As String In lstCombos
                strNombreCategoria = Topico
                objNodosCategoria = New List(Of CFCodificacionContable.DinamicaContableListas)((From ln In ListasGeneralesConfiguracion Where ln.Topico = strNombreCategoria))

                objDiccionarioNuevo.Add(strNombreCategoria, objNodosCategoria)
            Next

            CombosGenerales = objDiccionarioNuevo
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para cargar los combos.", Me.ToString(), "cargarCombos", Application.Current.ToString(), Program.Maquina, ex)
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
                HabilitarDetalle = True
                HabilitarEncabezado = False
                HabilitarEstadoConfig = False

                MyBase.TerminoSubmitChanges(So)
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico

                Await ConsultarEncabezado(True, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
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
    ''' 
    Private Async Sub ConsultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of CFCodificacionContable.DinamicaContableConfig)
        Dim dcProxy As CodificacionContableDomainContext

        Try
            dcProxy = inicializarProxyCodificacionContable()

            objRet = Await dcProxy.Load(dcProxy.ConsultarDinamicaContableConfigPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de CodificacionContable
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    ''' 
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pintIdDinamicaContableConfig As Integer = -1,
                                               Optional ByVal pstrTipoCompania As String = "",
                                               Optional ByVal pstrNombreConfiguracion As String = "",
                                               Optional ByVal pstrDescripcion As String = "",
                                               Optional ByVal pstrActiva As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCodificacionContable.DinamicaContableConfig)

        Try

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If

            mdcProxy.DinamicaContableConfigs.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarDinamicaContableConfigSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarDinamicaContableConfigSyncQuery(pintIdDinamicaContableConfig:=pintIdDinamicaContableConfig, pstrTipoCompania:=pstrTipoCompania,
                                                                                             pstrNombreConfiguracion:=pstrNombreConfiguracion, pstrDescripcion:=pstrDescripcion, pstrActiva:=pstrActiva, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
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
                    ListaEncabezado = mdcProxy.DinamicaContableConfigs

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
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de CodificacionContableDetalle ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consultar la dinámica contable asociada a un evento contable.
    ''' </summary>
    ''' 
    Private Async Function ConsultarListasConfiguracionCombos(ByVal pintTipoLista As TiposListasCombos, ByVal pstrTopico As String, ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrCodigo As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCodificacionContable.DinamicaContableListas)

        Try

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If

            mdcProxy.DinamicaContableListas.Clear()

            Select Case pintTipoLista
                Case TiposListasCombos.Generales
                    objRet = Await mdcProxy.Load(mdcProxy.ConsultarListasDinamicaContableSyncQuery(pstrTopico, -1, String.Empty, Program.Usuario, Program.HashConexion)).AsTask()
                Case TiposListasCombos.Eventos
                    objRet = Await mdcProxy.Load(mdcProxy.ConsultarListasDinamicaContableSyncQuery(pstrTopico, pintIdDinamicaContableConfig, pstrCodigo, Program.Usuario, Program.HashConexion)).AsTask()
                Case Else
                    objRet = Nothing
            End Select

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarListasConfiguracionCombos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Select Case pintTipoLista
                            Case TiposListasCombos.Generales
                                ListasGeneralesConfiguracion = mdcProxy.DinamicaContableListas.ToList
                            Case TiposListasCombos.Eventos
                                ListaConfiguracionNorma = mdcProxy.DinamicaContableListas.ToList
                            Case Else

                        End Select
                    End If
                End If
            Else
                Select Case pintTipoLista
                    Case TiposListasCombos.Generales
                        ListasGeneralesConfiguracion.Clear()
                    Case TiposListasCombos.Eventos
                        ListaConfiguracionNorma.Clear()
                    Case Else

                End Select
            End If

            MyBase.CambioItem("ListaCombos")

            logResultado = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de las listas", Me.ToString(), "ConsultarListasConfiguracionCombos", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Async Sub SeleccionarNormaAConfigurar(ByVal pobjNorma As Object)
        Dim objNorma As CFCodificacionContable.DinamicaContableListas
        Try
            If Not _EncabezadoSeleccionado Is Nothing And Not pobjNorma Is Nothing Then

                IsBusy = True

                objNorma = CType(pobjNorma, CFCodificacionContable.DinamicaContableListas)

                NormaContableTipoInversion = objNorma.Codigo

                If Not ListaDetalleConceptosComunes Is Nothing Then
                    ListaDetalleConceptosComunes.Clear()
                End If

                If Not ListaDetalleConceptosEspecificos Is Nothing Then
                    ListaDetalleConceptosEspecificos.Clear()
                End If

                If Not ListaDetalleDinamica Is Nothing Then
                    ListaDetalleDinamica.Clear()
                End If

                If Not ListaConfiguracionNorma Is Nothing Then
                    ListaConfiguracionNorma.Clear()
                End If

                DetalleDinamicaSeleccionado = Nothing
                DetalleConceptosComunesSeleccionado = Nothing
                DetalleConceptosEspecificosSeleccionado = Nothing
                _ListaDetalleConceptosComunesPag = Nothing ' Colocar en nothing para asegurar que cuando se recargue la lista se pagine correctamente
                EventoContableSeleccion = Nothing
                ClaseContableSeleccion = Nothing

                MyBase.CambioItem("ListaDetalleConceptosComunes")
                MyBase.CambioItem("ListaDetalleConceptosEspecificos")
                MyBase.CambioItem("ListaDetalleDinamica")
                MyBase.CambioItem("ListaDetalleConceptosComunesPag")
                MyBase.CambioItem("ListaDetalleConceptosEspecificosPag")
                MyBase.CambioItem("ListaConfiguracionNorma")
                MyBase.CambioItem("DetalleDinamicaSeleccionado")
                MyBase.CambioItem("DetalleConceptosComunesSeleccionado")
                MyBase.CambioItem("DetalleConceptosEspecificosSeleccionado")
                MyBase.CambioItem("EventoContableSeleccion")
                MyBase.CambioItem("ClaseContableSeleccion")

                Await ConsultarListasConfiguracionCombos(TiposListasCombos.Eventos, "EVENTOS_NORMA_TIPOINVERSION", _EncabezadoSeleccionado.IdDinamicaContableConfig, objNorma.Codigo)

                If Not ListaConfiguracionNorma Is Nothing AndAlso ListaConfiguracionNorma.Count > 0 Then
                    Await ConsultarConceptosComunes(_EncabezadoSeleccionado.IdDinamicaContableConfig, objNorma.Codigo)
                End If

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para actualizar las listas según la norma contable", Me.ToString(), "SeleccionarNormaAConfigurar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub SeleccionarEventoContable(ByVal pobjEvento As Object)
        Dim objEvento As CFCodificacionContable.DinamicaContableListas
        Try
            If Not _EncabezadoSeleccionado Is Nothing And Not pobjEvento Is Nothing Then

                IsBusy = True

                objEvento = CType(pobjEvento, CFCodificacionContable.DinamicaContableListas)

                If Not ListaDetalleDinamica Is Nothing Then
                    ListaDetalleDinamica.Clear()
                End If

                DetalleDinamicaSeleccionado = Nothing

                MyBase.CambioItem("ListaDetalleDinamica")
                MyBase.CambioItem("DetalleDinamicaSeleccionado")

                Await ConsultarDinamicaContable(objEvento.IdLista)

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para actualizar las listas según el evento contable", Me.ToString(), "SeleccionarEventoContable", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub SeleccionarClaseContable(ByVal pobjClaseContable As Object)
        Dim objClaseContable As CFCodificacionContable.DinamicaContableListas

        Try
            If Not _EncabezadoSeleccionado Is Nothing And Not pobjClaseContable Is Nothing Then

                IsBusy = True

                objClaseContable = CType(pobjClaseContable, CFCodificacionContable.DinamicaContableListas)

                mstrClaseContableAnterior = guardarCuentasClaseContable(mstrClaseContableAnterior, objClaseContable.Codigo)

                If Not ListaDetalleConceptosEspecificos Is Nothing Then
                    ListaDetalleConceptosEspecificos.Clear()
                End If

                DetalleConceptosEspecificosSeleccionado = Nothing

                MyBase.CambioItem("ListaDetalleConceptosEspecificos")
                MyBase.CambioItem("ListaDetalleConceptosEspecificosPag")
                MyBase.CambioItem("DetalleConceptosEspecificosSeleccionado")

                Await ConsultarConceptosEspecificos(EncabezadoSeleccionado.IdDinamicaContableConfig, NormaContableTipoInversion, objClaseContable.Codigo)

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para actualizar las listas de clases contables", Me.ToString(), "SeleccionarClaseContable", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function guardarCuentasClaseContable(ByVal pstrClaseContableOrigen As String, ByVal pstrClaseContableDestino As String) As String
        Try
            Dim strCuentas As String = String.Empty

            If Not ListaDetalleConceptosEspecificos Is Nothing AndAlso Not pstrClaseContableOrigen.Equals(String.Empty) And Editando Then
                For Each obj In _ListaDetalleConceptosEspecificos
                    If Not obj.CuentaContable.Trim.Equals(String.Empty) Then
                        strCuentas = strCuentas & obj.IdConceptoContable & "," & obj.IdNaturaleza & "," & obj.IdTipoCCosto & "," & obj.IdTipoTercero & ",""" & obj.CuentaContable & """" & MSTR_SEPARADOR_REG
                    End If
                Next

                ' Eliminar el último separador
                If Not strCuentas.Equals(String.Empty) Then
                    strCuentas = strCuentas.Substring(0, strCuentas.Length - 1)

                    If mdicCuentasPorClase.ContainsKey(pstrClaseContableOrigen) Then
                        mdicCuentasPorClase.Remove(pstrClaseContableOrigen)
                    End If
                    mdicCuentasPorClase.Add(pstrClaseContableOrigen, strCuentas)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para almacenar la lista de cuentas según la clase contable", Me.ToString(), "guardarCuentasClaseContable", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (pstrClaseContableDestino)
    End Function

#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Consultar la dinámica contable asociada a un evento contable.
    ''' </summary>
    ''' 
    Private Async Function ConsultarDinamicaContable(ByVal pintIdDinamicaContableConfigNorma As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCodificacionContable.DinamicaContableConfigDinamica)

        Try

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If

            mdcProxy.DinamicaContableConfigDinamicas.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarDinamicaContableConfigDinamicaSyncQuery(pintIdDinamicaContableConfigNorma, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarDinamicaContable", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        ListaDetalleDinamica = mdcProxy.DinamicaContableConfigDinamicas.ToList
                    Else
                        If Not ListaDetalleDinamica Is Nothing Then
                            ListaDetalleDinamica.Clear()
                        End If
                    End If
                End If
            Else
                ListaDetalleDinamica.Clear()
            End If

            MyBase.CambioItem("ListaDetalleDinamica")

            logResultado = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de CodificacionContableDetalle ", Me.ToString(), "ConsultarDinamicaContable", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consultar los conceptos que son comunes en su configuración de cuentas a todas las clases contables
    ''' </summary>
    ''' 
    Private Async Function ConsultarConceptosComunes(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrNormaTipoInversion As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCodificacionContable.DinamicaContableConfigConceptosComunes)

        Try

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If

            mdcProxy.DinamicaContableConfigConceptosComunes.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarConceptosComunesSyncQuery(pintIdDinamicaContableConfig, pstrNormaTipoInversion, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarConceptosComunes", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        ListaDetalleConceptosComunes = mdcProxy.DinamicaContableConfigConceptosComunes.ToList
                    Else
                        If Not _ListaDetalleConceptosComunes Is Nothing Then
                            ListaDetalleConceptosComunes.Clear()
                        End If
                    End If
                End If
            Else
                If Not _ListaDetalleConceptosComunes Is Nothing Then
                    ListaDetalleConceptosComunes.Clear()
                End If
            End If

            MyBase.CambioItem("ListaDetalleConceptosComunes")
            MyBase.CambioItem("ListaDetalleConceptosComunesPag")

            logResultado = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de conceptos comunes.", Me.ToString(), "ConsultarConceptosComunes", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consultar los conceptos que son diferentes en su configuración de cuentas por clase contable
    ''' </summary>
    ''' 
    Private Async Function ConsultarConceptosEspecificos(ByVal pintIdDinamicaContableConfig As Integer, ByVal pstrNormaContableTipoInversion As String, ByVal pstrIdClaseContable As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCodificacionContable.DinamicaContableConfigConceptosEspecificos)

        Try

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If

            mdcProxy.DinamicaContableConfigConceptosEspecificos.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarConceptosClaseContableSyncQuery(pintIdDinamicaContableConfig, pstrNormaContableTipoInversion, pstrIdClaseContable, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarConceptosEspecificos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        ListaDetalleConceptosEspecificos = mdcProxy.DinamicaContableConfigConceptosEspecificos.ToList
                    Else
                        If Not _ListaDetalleConceptosEspecificos Is Nothing Then
                            ListaDetalleConceptosEspecificos.Clear()
                        End If
                    End If
                End If
            Else
                If Not _ListaDetalleConceptosEspecificos Is Nothing Then
                    ListaDetalleConceptosEspecificos.Clear()
                End If
            End If

            MyBase.CambioItem("ListaDetalleConceptosEspecificos")
            MyBase.CambioItem("ListaDetalleConceptosEspecificosPag")

            logResultado = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de conceptos por clase contable", Me.ToString(), "ConsultarConceptosEspecificos", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
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
''' 
Public Class CamposBusquedaDinamicaContableConfig
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Private _intIdDinamicaContableConfig As System.Nullable(Of Integer)
    Public Property IdDinamicaContableConfig() As System.Nullable(Of Integer)
        Get
            Return _intIdDinamicaContableConfig
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intIdDinamicaContableConfig = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdDinamicaContableConfig"))
        End Set
    End Property

    Private _strNombreConfiguracion As System.String
    Public Property NombreConfiguracion() As System.String
        Get
            Return _strNombreConfiguracion
        End Get
        Set(ByVal value As System.String)
            _strNombreConfiguracion = value
        End Set
    End Property

    Private _strDescripcion As System.String
    Public Property Descripcion() As System.String
        Get
            Return _strDescripcion
        End Get
        Set(ByVal value As System.String)
            _strDescripcion = value
        End Set
    End Property

    Private _strActiva As System.String
    Public Property Activa() As String
        Get
            Return _strActiva
        End Get
        Set(ByVal value As String)
            _strActiva = value
        End Set
    End Property

End Class

