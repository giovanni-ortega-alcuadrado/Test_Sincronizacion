Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFUtilidades
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Windows.Data
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports Microsoft.VisualBasic.CompilerServices
Imports A2Utilidades
Imports System.Collections.ObjectModel
Imports Newtonsoft.Json

Public Class EjecutarScriptsViewModel
    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged

#Region "Eventos"

    Public Event actualizarControles()
    Public Event actualizarControlesDependientes(ByVal pstrParametroCambio As String, ByVal pstrValorParametro As String)
    Public Event actualizarListaComboDependiente(ByVal pstrParametroCambio As String, ByVal pobjListaNueva As List(Of OYDUtilidades.ItemCombo))

#End Region

#Region "Constantes - REQUERIDO"

    Private Const MINT_MAX_LONG_ENTERO As Byte = 14 '-2.147.483.648
    Private Const MINT_MAX_LONG_DECIMAL As Byte = 30 '100.000.000.000.000.000,000000
    Private Const MINT_MAX_LONG_TEXTO As Byte = 100

    Private Const MINT_MIN_ENTERO As Integer = -2147483648
    Private Const MINT_MAX_ENTERO As Integer = 2147483647

    Public Const MSTR_COMBOS_ESPECIFICOS As String = "EjecutarScripts"

    Private Enum TipoDatoParametro As Byte
        TIPO_DATO_TEXTO
        TIPO_DATO_ENTERO
        TIPO_DATO_DECIMAL
        TIPO_DATO_FECHA
        TIPO_DATO_SI_NO
    End Enum

    Private Enum TipoFuenteDatos As Byte
        TIPO_FUENTE_DATOS_VALOR_FIJO
        TIPO_FUENTE_DATOS_FECHA
        TIPO_FUENTE_DATOS_LISTA_VALORES
        TIPO_FUENTE_DATOS_COMBOA2
        TIPO_FUENTE_DATOS_COMBOA2ESPECIFICO
        TIPO_FUENTE_DATOS_BUSCADORGENERICO
        TIPO_FUENTE_DATOS_BUSCADORESPECIES
        TIPO_FUENTE_DATOS_BUSCADORCLIENTES
        TIPO_FUENTE_DATOS_CONSULTA
        TIPO_FUENTE_DATOS_MULTICOMPANIA
        TIPO_FUENTE_DATOS_SISTEMA
    End Enum

    Private Enum ParametrosSistema As Byte
        USUARIO
        MAQUINA
        USUARIOWINDOWS
    End Enum

    Private Enum ParametrosBuscadorGenerico As Byte
        TIPOITEM
        ESTADO
        AGRUPAMIENTO
    End Enum

    Private Enum ParametrosBuscadorCliente As Byte
        ESTADO
        TIPOVINCULACION
        AGRUPAMIENTO
        CARGARCLIENTESRESTRICCION
        CARGARCLIENTESXTIPOPRODUCTOPERFIL
        CARGARCLIENTETERCERO
        RECEPTOR
        TIPONEGOCIO
        TIPOPRODUCTO
    End Enum

    Private Enum ParametrosBuscadorEspecie As Byte
        CLASE
        ESTADO
        AGRUPAMIENTO
        CARGARESPECIESRESTRICCION
        TIPONEGOCIO
        TIPOPRODUCTO
        HABILITARCONSULTAISIN
    End Enum

#End Region

#Region "Variables - REQUERIDO"

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxyActualizar As UtilidadesCFDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxyUtils As UtilidadesDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private logCambiarPropiedades As Boolean = True

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Id del script seleccionado
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mintIdScriptAnterior As Integer = -1

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' View model
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public objA2ViewModel As A2UtilsViewModel

#End Region

#Region "Inicialización - REQUERIDO"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' 
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' 
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando

                ' Notificar cambio de compañía activa en caso de no tener una seleccionada al crear el view model
                MyBase.CambioItem("DescripcionCompaniaActiva")

                objA2ViewModel = New A2UtilsViewModel
                Await objA2ViewModel.inicializarCombos(String.Empty, String.Empty)
                Await objA2ViewModel.inicializarCombos(MSTR_COMBOS_ESPECIFICOS, MSTR_COMBOS_ESPECIFICOS)

                EncabezadoSeleccionado = Nothing
                If Not ListaEncabezado Is Nothing Then
                    ListaEncabezado.Clear()
                End If

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                If logEsSoloUnScript Then
                    If String.IsNullOrEmpty(NombreScriptFiltro) Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se recibio el nombre del script a filtrar, por favor comuniquese con el administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    End If
                    Await consultarEncabezado(True, NombreScriptFiltro, -1, String.Empty, String.Empty, String.Empty, String.Empty, 0, String.Empty) ' Consultar iniciales que se presentan en la lista de Normas Contables
                Else
                    If logEsFiltroGrupo Then
                        If String.IsNullOrEmpty(GrupoScriptFiltro) Then
                            A2Utilidades.Mensajes.mostrarMensaje("No se recibio el nombre del grupo a filtrar, por favor comuniquese con el administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                    Await consultarEncabezado(True, String.Empty, -1, String.Empty, GrupoScriptFiltro, String.Empty, String.Empty, 0, String.Empty) ' Consultar iniciales que se presentan en la lista de Normas Contables
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

    Public Sub iniciarFormularioGenerador()
        Try
            viewEjecutarScript.Editar.Children.Clear()
            Dim objEjecutarScriptGeneradoConsulta As New GeneradorConsultaView(Me)
            viewEjecutarScript.Editar.Children.Add(objEjecutarScriptGeneradoConsulta)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "iniciarFormularioGenerador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CargarComboDestino()
        Try
            Dim strOpcionesHabilitadas As String = String.Empty

            If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strDestinoOpciones) Then
                strOpcionesHabilitadas = _EncabezadoSeleccionado.strDestinoOpciones
            Else
                strOpcionesHabilitadas = _EncabezadoSeleccionado.TipoResultado.ToUpper & "|PANTALLA"
            End If

            Dim objListaTipoGeneracion As New List(Of String)
            For Each li In strOpcionesHabilitadas.Split(CChar("|"))
                objListaTipoGeneracion.Add(li)
            Next

            ListaOpcionesGeneracion = objListaTipoGeneracion

            If ListaOpcionesGeneracion.Count = 1 Then
                OpcionGeneracionSeleccionado = ListaOpcionesGeneracion.First
            Else
                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strDestinoDefecto) Then
                    If ListaOpcionesGeneracion.Where(Function(i) i = _EncabezadoSeleccionado.strDestinoDefecto.ToUpper).Count > 0 Then
                        OpcionGeneracionSeleccionado = _EncabezadoSeleccionado.strDestinoDefecto.ToUpper
                    End If
                Else
                    If ListaOpcionesGeneracion.Where(Function(i) i = _EncabezadoSeleccionado.TipoResultado.ToUpper).Count > 0 Then
                        OpcionGeneracionSeleccionado = _EncabezadoSeleccionado.TipoResultado.ToUpper
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "CargarComboDestino", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Public viewEjecutarScript As EjecutarScriptsView

    Private _logEsSoloUnScript As Boolean = False
    Public Property logEsSoloUnScript() As Boolean
        Get
            Return _logEsSoloUnScript
        End Get
        Set(ByVal value As Boolean)
            _logEsSoloUnScript = value
            If _logEsSoloUnScript Then
                MostrarSoloNombreScript = Visibility.Visible
                MostrarDetalleNombreScript = Visibility.Collapsed
            Else
                MostrarSoloNombreScript = Visibility.Collapsed
                MostrarDetalleNombreScript = Visibility.Visible
            End If
        End Set
    End Property

    Private _NombreScriptFiltro As String = String.Empty
    Public Property NombreScriptFiltro() As String
        Get
            Return _NombreScriptFiltro
        End Get
        Set(ByVal value As String)
            _NombreScriptFiltro = value
        End Set
    End Property

    Private _logEsFiltroGrupo As Boolean
    Public Property logEsFiltroGrupo() As Boolean
        Get
            Return _logEsFiltroGrupo
        End Get
        Set(ByVal value As Boolean)
            _logEsFiltroGrupo = value
            If _logEsFiltroGrupo Then
                MostrarGrupoBusqueda = Visibility.Collapsed
            Else
                MostrarGrupoBusqueda = Visibility.Visible
            End If
        End Set
    End Property

    Private _GrupoScriptFiltro As String = String.Empty
    Public Property GrupoScriptFiltro() As String
        Get
            Return _GrupoScriptFiltro
        End Get
        Set(ByVal value As String)
            _GrupoScriptFiltro = value
        End Set
    End Property

    Private _MostrarGrupoBusqueda As Visibility = Visibility.Visible
    Public Property MostrarGrupoBusqueda() As Visibility
        Get
            Return _MostrarGrupoBusqueda
        End Get
        Set(ByVal value As Visibility)
            _MostrarGrupoBusqueda = value
            MyBase.CambioItem("MostrarGrupoBusqueda")
        End Set
    End Property

    Private _MostrarSoloNombreScript As Visibility = Visibility.Collapsed
    Public Property MostrarSoloNombreScript() As Visibility
        Get
            Return _MostrarSoloNombreScript
        End Get
        Set(ByVal value As Visibility)
            _MostrarSoloNombreScript = value
            MyBase.CambioItem("MostrarSoloNombreScript")
        End Set
    End Property

    Private _MostrarDetalleNombreScript As Visibility = Visibility.Visible
    Public Property MostrarDetalleNombreScript() As Visibility
        Get
            Return _MostrarDetalleNombreScript
        End Get
        Set(ByVal value As Visibility)
            _MostrarDetalleNombreScript = value
            MyBase.CambioItem("MostrarDetalleNombreScript")
        End Set
    End Property


    ''' <summary>
    ''' Lista de parámetros del grid seleccionado
    ''' </summary>
    Private _ListaParametros As EntitySet(Of ScriptsA2Parametros)
    Public Property ListaParametros() As EntitySet(Of ScriptsA2Parametros)
        Get
            Return _ListaParametros
        End Get
        Set(ByVal value As EntitySet(Of ScriptsA2Parametros))
            _ListaParametros = value

            MyBase.CambioItem("ListaParametros")
        End Set
    End Property

    Private _ListaDependenciaParametros As List(Of clsDependenciaParametros)
    Public Property ListaDependenciaParametros() As List(Of clsDependenciaParametros)
        Get
            Return _ListaDependenciaParametros
        End Get
        Set(ByVal value As List(Of clsDependenciaParametros))
            _ListaDependenciaParametros = value
        End Set
    End Property

    ''' <summary>
    ''' Lista de scripts que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of ScriptsA2)
    Public Property ListaEncabezado() As EntitySet(Of ScriptsA2)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of ScriptsA2))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de scripts que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As ScriptsA2
    Public Property EncabezadoSeleccionado() As ScriptsA2
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As ScriptsA2)
            _EncabezadoSeleccionado = value
            If Not IsNothing(_EncabezadoSeleccionado) Then
                CargarOpcionesScript()
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaScripts
    Public Property cb() As CamposBusquedaScripts
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaScripts)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    ''' <summary>
    ''' Indicar si las compañías están o no disponibles para ser seleccionados
    ''' </summary>
    ''' 
    Private _ActivarCompanias As Boolean = False
    Public Property ActivarCompanias() As Boolean
        Get
            Return _ActivarCompanias
        End Get
        Set(ByVal value As Boolean)
            _ActivarCompanias = value
            MyBase.CambioItem("ActivarCompanias")
        End Set
    End Property

    ''' <summary>
    ''' Indicar si las compañías están o no disponibles para ser seleccionados
    ''' </summary>
    ''' 
    Private _CompaniasVisibles As Visibility = Visibility.Visible
    Public Property CompaniasVisibles() As Visibility
        Get
            Return _CompaniasVisibles
        End Get
        Set(ByVal value As Visibility)
            _CompaniasVisibles = value
            MyBase.CambioItem("CompaniasVisibles")
        End Set
    End Property

    ''' <summary>
    ''' Lista de compañías seleccionadas para incluir
    ''' </summary>
    ''' 
    Private _ListaCompaniasSeleccionadas As String = String.Empty
    Public Property ListaCompaniasSeleccionadas As String
        Get
            Return _ListaCompaniasSeleccionadas
        End Get
        Set(ByVal value As String)
            _ListaCompaniasSeleccionadas = value
            MyBase.CambioItem("ListaCompaniasSeleccionadas")
        End Set
    End Property

    ''' <summary>
    ''' Indica la pestaña seleccionada en el control Tab de la interfase de usuario
    ''' </summary>
    ''' 
    Private _TabSeleccionado As Integer = 0
    Public Property TabSeleccionado As Integer
        Get
            Return (_TabSeleccionado)
        End Get
        Set(value As Integer)
            If value < 0 Then
                _TabSeleccionado = 0
            Else
                _TabSeleccionado = value
            End If

            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Indica si las listas de compañías deben ser actualizadas
    ''' </summary>
    ''' 
    Private _ActualizarListas As Boolean = False
    Public Property ActualizarListas As Boolean
        Get
            Return (_ActualizarListas)
        End Get
        Set(value As Boolean)
            _ActualizarListas = value

            MyBase.CambioItem("ActualizarListas")
        End Set
    End Property

    Private _ListaOpcionesGeneracion As List(Of String)
    Public Property ListaOpcionesGeneracion() As List(Of String)
        Get
            Return _ListaOpcionesGeneracion
        End Get
        Set(ByVal value As List(Of String))
            _ListaOpcionesGeneracion = value
            MyBase.CambioItem("ListaOpcionesGeneracion")
        End Set
    End Property

    Private _OpcionGeneracionSeleccionado As String
    Public Property OpcionGeneracionSeleccionado() As String
        Get
            Return _OpcionGeneracionSeleccionado
        End Get
        Set(ByVal value As String)
            _OpcionGeneracionSeleccionado = value
            If _OpcionGeneracionSeleccionado = "PANTALLA" Then
                HabilitarSeleccionDiseno = Visibility.Visible
            Else
                HabilitarSeleccionDiseno = Visibility.Collapsed
            End If
            MyBase.CambioItem("OpcionGeneracionSeleccionado")
        End Set
    End Property

    Private _ListaDisenos As List(Of ScriptsA2DisenosPrincipales)
    Public Property ListaDisenos() As List(Of ScriptsA2DisenosPrincipales)
        Get
            Return _ListaDisenos
        End Get
        Set(ByVal value As List(Of ScriptsA2DisenosPrincipales))
            _ListaDisenos = value
            MyBase.CambioItem("ListaDisenos")
        End Set
    End Property

    Private _DisenoSeleccionado As Integer
    Public Property DisenoSeleccionado() As Integer
        Get
            Return _DisenoSeleccionado
        End Get
        Set(ByVal value As Integer)
            _DisenoSeleccionado = value
            MyBase.CambioItem("DisenoSeleccionado")
        End Set
    End Property

    Private _HabilitarSeleccionDiseno As Visibility = Visibility.Collapsed
    Public Property HabilitarSeleccionDiseno() As Visibility
        Get
            Return _HabilitarSeleccionDiseno
        End Get
        Set(ByVal value As Visibility)
            _HabilitarSeleccionDiseno = value
            MyBase.CambioItem("HabilitarSeleccionDiseno")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' 
    ''' </summary>
    ''' 
    Public Overrides Async Sub CambiarAForma()
        Try
            If Not EncabezadoSeleccionado Is Nothing Then
                If mintIdScriptAnterior <> _EncabezadoSeleccionado.ID Then
                    Await cargarDefinicionParametrosScript(EncabezadoSeleccionado.ID)

                    mintIdScriptAnterior = _EncabezadoSeleccionado.ID
                End If

                TabSeleccionado = 0

                MyBase.CambiarAForma()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar a vista de formulario", Me.ToString(), "CambiarAForma", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' 
    Public Overrides Sub NuevoRegistro()
        Try
            A2Utilidades.Mensajes.mostrarMensaje("La creación de nuevos scripts no está permitida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' 
    Public Overrides Async Sub Filtrar()
        Try
            Await consultarEncabezado(True, FiltroVM, -1, String.Empty, GrupoScriptFiltro, String.Empty, String.Empty, 0, String.Empty)
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
        prepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    ''' 
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If logEsFiltroGrupo Then
                Await consultarEncabezado(False, String.Empty, cb.IdScript, "G", GrupoScriptFiltro, cb.Nombre, cb.Descripcion, 1, cb.TipoResultado, cb.TipoProceso)
            Else
                If Not String.IsNullOrEmpty(cb.Grupo) Or
                Not String.IsNullOrEmpty(cb.Nombre) Or
                Not String.IsNullOrEmpty(cb.Descripcion) Then 'Validar que ingresó algo en los campos de búsqueda
                    Await consultarEncabezado(False, String.Empty, cb.IdScript, String.Empty, cb.Grupo, cb.Nombre, cb.Descripcion, 1, cb.TipoResultado, cb.TipoProceso)
                End If
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
    Public Overrides Sub ActualizarRegistro()

        Try
            A2Utilidades.Mensajes.mostrarMensaje("La actualización de los scripts no está permitida en el sistema.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub EditarRegistro()
        Try
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("La edición de los scripts no está permitida en el sistema.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de edición.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub CancelarEditarRegistro()

    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub BorrarRegistro()
        Try
            A2Utilidades.Mensajes.mostrarMensaje("El borrado de scripts no está permitido en el sistema.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
    Private Sub borrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    ''' <summary>
    ''' Crea la definición de los controles asociados a los parámetros en forma dinámica
    ''' </summary>
    ''' 
    Public Function crearControlesParametros(Optional ByVal plogRefrescarListaCombos As Boolean = False) As clsObjetosAgregados

        Dim intFilaActual As Integer = 0
        Dim objRetorno As New clsObjetosAgregados
        Dim objListaControlesGrid As New List(Of clsControlesGrid)
        Dim objGrid As Controls.Grid = Nothing
        Dim objEtiqueta As Label
        Dim objTexto As TextBox
        Dim objCombo As ComboBox
        Dim objFecha As A2DatePicker
        Dim objBuscadorGenerico As A2ComunesControl.BuscadorGenerico
        Dim objBuscadorEspecies As A2ComunesControl.BuscadorEspecie
        Dim objBuscadorCliente As A2ComunesControl.BuscadorCliente
        Dim objDicCombos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)) = Nothing
        Dim objDicCombosEspecificos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)) = Nothing

        Try
            objGrid = New Grid
            objGrid.Margin = New Thickness(20)
            objGrid.ColumnDefinitions.Add(New Controls.ColumnDefinition() With {.Width = New GridLength()})
            objGrid.ColumnDefinitions.Add(New Controls.ColumnDefinition())

            If _EncabezadoSeleccionado.ParametroMulticompania Is Nothing OrElse _EncabezadoSeleccionado.ParametroMulticompania.Trim().Equals(String.Empty) Then
                CompaniasVisibles = Visibility.Collapsed
                ActivarCompanias = False
            Else
                CompaniasVisibles = Visibility.Visible
                ActivarCompanias = True
            End If

            ' CCM20150319: Si no se tiene parámetros se adiciona una etiqueta indicandole al usuario que el script no requiere parámetros
            If IsNothing(ListaParametros) OrElse ListaParametros.Count = 0 Then
                objGrid.RowDefinitions.Add(New RowDefinition)
                objGrid.RowDefinitions.Item(intFilaActual).Height = New GridLength(25)

                objEtiqueta = New Label With {.Content = "La consulta no tiene parámetros.",
                                            .Margin = New Thickness(2),
                                            .Style = CType(Application.Current.Resources("LabelForm"), Style)}

                objGrid.Children.Add(objEtiqueta)
                Grid.SetRow(objEtiqueta, 0)
                Grid.SetColumn(objEtiqueta, 0)

                intFilaActual += 1

                objGrid.RowDefinitions.Add(New RowDefinition)
                objGrid.RowDefinitions.Item(intFilaActual).Height = New GridLength(25)

                objEtiqueta = New Label With {.Content = "De clic sobre el botón Ejecutar para iniciar la ejecución de la consulta.",
                                            .Margin = New Thickness(2),
                                            .Style = CType(Application.Current.Resources("LabelForm"), Style)}

                objGrid.Children.Add(objEtiqueta)
                Grid.SetRow(objEtiqueta, intFilaActual)
                Grid.SetColumn(objEtiqueta, 0)
            Else
                For Each objParam In ListaParametros
                    If Not objParam.TipoFuenteDatos.ToUpper.Equals(TipoFuenteDatos.TIPO_FUENTE_DATOS_MULTICOMPANIA.ToString.ToUpper) And
                        Not objParam.TipoFuenteDatos.ToUpper.Equals(TipoFuenteDatos.TIPO_FUENTE_DATOS_SISTEMA.ToString.ToUpper) Then
                        objGrid.RowDefinitions.Add(New RowDefinition)
                        objGrid.RowDefinitions.Item(intFilaActual).Height = New GridLength(30)

                        objEtiqueta = New Label With {.Content = objParam.Etiqueta,
                                                    .Margin = New Thickness(2),
                                                    .Style = CType(Application.Current.Resources("LabelForm"), Style)}

                        ToolTipService.SetToolTip(objEtiqueta, objParam.Descripcion)

                        objGrid.Children.Add(objEtiqueta)
                        Grid.SetRow(objEtiqueta, intFilaActual)
                        Grid.SetColumn(objEtiqueta, 0)

                        Select Case objParam.TipoFuenteDatos.ToUpper
                            Case TipoFuenteDatos.TIPO_FUENTE_DATOS_VALOR_FIJO.ToString.ToUpper

                                objTexto = New TextBox With {.Text = objParam.ValorPorDefecto}
                                objTexto.Name = objParam.Parametro.Replace("@", "")
                                objTexto.Height = 20
                                objTexto.MinWidth = 130

                                If (ListaDependenciaParametros.Where(Function(i) i.NombreParametro = objParam.Parametro)).Count > 0 Then
                                    AddHandler objTexto.TextChanged, AddressOf TextBox_TextChanged
                                End If

                                objGrid.Children.Add(objTexto)
                                objListaControlesGrid.Add(New clsControlesGrid With {.Nombre = objTexto.Name, .Elemento = objTexto})
                                Grid.SetRow(objTexto, intFilaActual)
                                Grid.SetColumn(objTexto, 1)

                                If objParam.TipoDato.ToUpper.Equals(TipoDatoParametro.TIPO_DATO_ENTERO.ToString.ToUpper) Then
                                    objTexto.MaxLength = MINT_MAX_LONG_ENTERO
                                ElseIf objParam.TipoDato.ToUpper.Equals(TipoDatoParametro.TIPO_DATO_DECIMAL.ToString.ToUpper) Then
                                    objTexto.MaxLength = MINT_MAX_LONG_DECIMAL
                                Else
                                    If Math.Round(objParam.LongitudMaxima, 0) > 0 And Math.Round(objParam.LongitudMaxima, 0) < 8000 Then
                                        objTexto.MaxLength = CInt(objParam.LongitudMaxima)
                                    Else
                                        objTexto.MaxLength = MINT_MAX_LONG_TEXTO
                                    End If
                                End If

                            Case TipoFuenteDatos.TIPO_FUENTE_DATOS_FECHA.ToString.ToUpper
                                objFecha = New A2DatePicker()

                                If IsDate(objParam.ValorPorDefecto) Then
                                    objFecha.SelectedDate = CType(objParam.ValorPorDefecto, Date?)
                                End If

                                objFecha.Name = objParam.Parametro.Replace("@", "")
                                objFecha.Height = 25
                                objFecha.MinWidth = 130

                                If (ListaDependenciaParametros.Where(Function(i) i.NombreParametro = objParam.Parametro)).Count > 0 Then
                                    AddHandler objFecha.SelectionChanged, AddressOf DatePicker_SelectedDateChanged
                                End If

                                objGrid.Children.Add(objFecha)
                                objListaControlesGrid.Add(New clsControlesGrid With {.Nombre = objFecha.Name, .Elemento = objFecha})
                                Grid.SetRow(objFecha, intFilaActual)
                                Grid.SetColumn(objFecha, 1)

                            Case TipoFuenteDatos.TIPO_FUENTE_DATOS_COMBOA2.ToString.ToUpper, TipoFuenteDatos.TIPO_FUENTE_DATOS_LISTA_VALORES.ToString.ToUpper, TipoFuenteDatos.TIPO_FUENTE_DATOS_COMBOA2ESPECIFICO.ToString.ToUpper
                                objCombo = New ComboBox
                                objCombo.Name = objParam.Parametro.Replace("@", "")
                                objCombo.Height = 25
                                objCombo.MinWidth = 130


                                If objParam.TipoFuenteDatos.ToUpper = TipoFuenteDatos.TIPO_FUENTE_DATOS_COMBOA2.ToString.ToUpper Then
                                    If plogRefrescarListaCombos Then
                                        If Application.Current.Resources.Contains(A2ComunesControl.FuncionesCompartidas.NOMBRE_LISTA_COMBOS_APP) Then
                                            objDicCombos = CType(Application.Current.Resources(A2ComunesControl.FuncionesCompartidas.NOMBRE_LISTA_COMBOS_APP), Dictionary(Of String, System.Collections.Generic.List(Of OYDUtilidades.ItemCombo))).ToDictionary(Function(k) k.Key, Function(k) New ObservableCollection(Of OYDUtilidades.ItemCombo)(k.Value))
                                        End If
                                    Else
                                        If objDicCombos Is Nothing AndAlso Application.Current.Resources.Contains(A2ComunesControl.FuncionesCompartidas.NOMBRE_LISTA_COMBOS_APP) Then
                                            objDicCombos = CType(Application.Current.Resources(A2ComunesControl.FuncionesCompartidas.NOMBRE_LISTA_COMBOS_APP), Dictionary(Of String, System.Collections.Generic.List(Of OYDUtilidades.ItemCombo))).ToDictionary(Function(k) k.Key, Function(k) New ObservableCollection(Of OYDUtilidades.ItemCombo)(k.Value))
                                        End If
                                    End If


                                    If Not objDicCombos Is Nothing Then
                                        If objDicCombos.ContainsKey(objParam.FuenteDatos) AndAlso Not objDicCombos.Item(objParam.FuenteDatos) Is Nothing Then
                                            objCombo.ItemsSource = objDicCombos.Item(objParam.FuenteDatos)
                                        End If
                                    End If

                                    objCombo.SelectedValuePath = "ID"
                                    objCombo.DisplayMemberPath = "Descripcion"

                                    objCombo.SelectedValue = objParam.ValorPorDefecto

                                ElseIf objParam.TipoFuenteDatos.ToUpper = TipoFuenteDatos.TIPO_FUENTE_DATOS_COMBOA2ESPECIFICO.ToString.ToUpper Then
                                    If String.IsNullOrEmpty(objParam.strParametroDependencia) Then
                                        If plogRefrescarListaCombos Then
                                            If Application.Current.Resources.Contains(MSTR_COMBOS_ESPECIFICOS) Then
                                                objDicCombosEspecificos = CType(Application.Current.Resources(MSTR_COMBOS_ESPECIFICOS), Dictionary(Of String, System.Collections.Generic.List(Of OYDUtilidades.ItemCombo))).ToDictionary(Function(k) k.Key, Function(k) New ObservableCollection(Of OYDUtilidades.ItemCombo)(k.Value))
                                            End If
                                        Else
                                            If objDicCombosEspecificos Is Nothing AndAlso Application.Current.Resources.Contains(MSTR_COMBOS_ESPECIFICOS) Then
                                                objDicCombosEspecificos = CType(Application.Current.Resources(MSTR_COMBOS_ESPECIFICOS), Dictionary(Of String, System.Collections.Generic.List(Of OYDUtilidades.ItemCombo))).ToDictionary(Function(k) k.Key, Function(k) New ObservableCollection(Of OYDUtilidades.ItemCombo)(k.Value))
                                            End If
                                        End If


                                        If Not objDicCombosEspecificos Is Nothing Then
                                            If objDicCombosEspecificos.ContainsKey(objParam.FuenteDatos) AndAlso Not objDicCombosEspecificos.Item(objParam.FuenteDatos) Is Nothing Then
                                                objCombo.ItemsSource = objDicCombosEspecificos.Item(objParam.FuenteDatos)
                                            End If
                                        End If
                                    End If

                                    objCombo.Tag = objParam.FuenteDatos
                                    objCombo.SelectedValuePath = "ID"
                                    objCombo.DisplayMemberPath = "Descripcion"

                                    If Not String.IsNullOrEmpty(objParam.strParametroDependencia) Then
                                        objCombo.SelectedValue = objParam.ValorPorDefecto
                                    End If

                                Else
                                    If Not objParam.FuenteDatos Is Nothing AndAlso Not objParam.FuenteDatos.Trim().Equals(String.Empty) Then
                                        objCombo.ItemsSource = objParam.FuenteDatos.Split(CChar(objParam.SeparadorFuenteDatos)).ToList
                                        objCombo.SelectedValue = objParam.ValorPorDefecto
                                    End If
                                End If

                                If (ListaDependenciaParametros.Where(Function(i) i.NombreParametro = objParam.Parametro)).Count > 0 Then
                                    AddHandler objCombo.SelectionChanged, AddressOf ComboBox_SelectionChanged
                                End If

                                objGrid.Children.Add(objCombo)
                                objListaControlesGrid.Add(New clsControlesGrid With {.Nombre = objCombo.Name, .Elemento = objCombo})
                                Grid.SetRow(objCombo, intFilaActual)
                                Grid.SetColumn(objCombo, 1)
                            Case TipoFuenteDatos.TIPO_FUENTE_DATOS_BUSCADORGENERICO.ToString.ToUpper
                                objBuscadorGenerico = New A2ComunesControl.BuscadorGenerico()
                                objBuscadorGenerico.BuscarAlIniciar = False
                                objBuscadorGenerico.MostrarLimpiar = True
                                objBuscadorGenerico.Name = objParam.Parametro.Replace("@", "")

                                If Not String.IsNullOrEmpty(objParam.FuenteDatos) And Not String.IsNullOrEmpty(objParam.SeparadorFuenteDatos) Then
                                    For Each objInfoParametro In objParam.FuenteDatos.Split(CChar(objParam.SeparadorFuenteDatos))
                                        If Not IsNothing(objInfoParametro.Split(CChar("="))) Then
                                            Select Case objInfoParametro.Split(CChar("="))(0).ToUpper
                                                Case ParametrosBuscadorGenerico.TIPOITEM.ToString.ToUpper
                                                    objBuscadorGenerico.TipoItem = objInfoParametro.Split(CChar("="))(1)
                                                Case ParametrosBuscadorGenerico.ESTADO.ToString.ToUpper
                                                    Select Case objInfoParametro.Split(CChar("="))(1)
                                                        Case A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A.ToString
                                                            objBuscadorGenerico.EstadoItem = A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A
                                                        Case A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.I.ToString
                                                            objBuscadorGenerico.EstadoItem = A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.I
                                                        Case A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.T.ToString
                                                            objBuscadorGenerico.EstadoItem = A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.T
                                                    End Select
                                                Case ParametrosBuscadorGenerico.AGRUPAMIENTO.ToString.ToUpper
                                                    If objInfoParametro.Split(CChar("="))(1) <> "{PARAMETRODEPENDIENTE}" Then
                                                        objBuscadorGenerico.Agrupamiento = objInfoParametro.Split(CChar("="))(1)
                                                    End If
                                            End Select
                                        End If
                                    Next
                                End If

                                If (ListaDependenciaParametros.Where(Function(i) i.NombreParametro = objParam.Parametro)).Count > 0 Then
                                    AddHandler objBuscadorGenerico.itemAsignadoControlOrigen, AddressOf buscadorGenerico_ItemAsignado
                                End If

                                objGrid.Children.Add(objBuscadorGenerico)
                                objListaControlesGrid.Add(New clsControlesGrid With {.Nombre = objBuscadorGenerico.Name, .Elemento = objBuscadorGenerico})
                                Grid.SetRow(objBuscadorGenerico, intFilaActual)
                                Grid.SetColumn(objBuscadorGenerico, 1)
                            Case TipoFuenteDatos.TIPO_FUENTE_DATOS_BUSCADORESPECIES.ToString.ToUpper
                                objBuscadorEspecies = New A2ComunesControl.BuscadorEspecie()
                                objBuscadorEspecies.BuscarAlIniciar = False
                                objBuscadorEspecies.MostrarLimpiar = True
                                objBuscadorEspecies.Name = objParam.Parametro.Replace("@", "")


                                If Not String.IsNullOrEmpty(objParam.FuenteDatos) And Not String.IsNullOrEmpty(objParam.SeparadorFuenteDatos) Then
                                    For Each objInfoParametro In objParam.FuenteDatos.Split(CChar(objParam.SeparadorFuenteDatos))
                                        If Not IsNothing(objInfoParametro.Split(CChar("="))) Then
                                            Select Case objInfoParametro.Split(CChar("="))(0).ToUpper
                                                Case ParametrosBuscadorEspecie.CLASE.ToString.ToUpper
                                                    Select Case objInfoParametro.Split(CChar("="))(1)
                                                        Case A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A.ToString
                                                            objBuscadorEspecies.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
                                                        Case A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C.ToString
                                                            objBuscadorEspecies.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
                                                        Case A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T.ToString
                                                            objBuscadorEspecies.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
                                                        Case Else
                                                            objBuscadorEspecies.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
                                                    End Select

                                                Case ParametrosBuscadorEspecie.TIPONEGOCIO.ToString.ToUpper
                                                    objBuscadorEspecies.TipoNegocio = objInfoParametro.Split(CChar("="))(1)
                                                Case ParametrosBuscadorEspecie.TIPOPRODUCTO.ToString.ToUpper
                                                    objBuscadorEspecies.TipoProducto = objInfoParametro.Split(CChar("="))(1)
                                                Case ParametrosBuscadorEspecie.ESTADO.ToString.ToUpper
                                                    Select Case objInfoParametro.Split(CChar("="))(1)
                                                        Case A2ComunesControl.BuscadorEspecieViewModel.EstadosEspecie.A.ToString
                                                            objBuscadorEspecies.EstadoEspecie = A2ComunesControl.BuscadorEspecieViewModel.EstadosEspecie.A
                                                        Case A2ComunesControl.BuscadorEspecieViewModel.EstadosEspecie.I.ToString
                                                            objBuscadorEspecies.EstadoEspecie = A2ComunesControl.BuscadorEspecieViewModel.EstadosEspecie.I
                                                        Case A2ComunesControl.BuscadorEspecieViewModel.EstadosEspecie.T.ToString
                                                            objBuscadorEspecies.EstadoEspecie = A2ComunesControl.BuscadorEspecieViewModel.EstadosEspecie.T
                                                        Case Else
                                                            objBuscadorEspecies.EstadoEspecie = A2ComunesControl.BuscadorEspecieViewModel.EstadosEspecie.T
                                                    End Select
                                                Case ParametrosBuscadorEspecie.AGRUPAMIENTO.ToString.ToUpper
                                                    objBuscadorEspecies.Agrupamiento = objInfoParametro.Split(CChar("="))(1)
                                                Case ParametrosBuscadorEspecie.CARGARESPECIESRESTRICCION.ToString.ToUpper
                                                    If objInfoParametro.Split(CChar("="))(1) = "1" Then
                                                        objBuscadorEspecies.CargarEspeciesRestriccion = True
                                                    Else
                                                        objBuscadorEspecies.CargarEspeciesRestriccion = False
                                                    End If
                                                Case ParametrosBuscadorEspecie.HABILITARCONSULTAISIN.ToString.ToUpper
                                                    If objInfoParametro.Split(CChar("="))(1) = "1" Then
                                                        objBuscadorEspecies.HabilitarConsultaISIN = True
                                                    Else
                                                        objBuscadorEspecies.HabilitarConsultaISIN = False
                                                    End If
                                            End Select
                                        End If
                                    Next
                                End If

                                If (ListaDependenciaParametros.Where(Function(i) i.NombreParametro = objParam.Parametro)).Count > 0 Then
                                    AddHandler objBuscadorEspecies.especieAsignadaControlOrigen, AddressOf buscadorEspecie_especieAsignada
                                End If

                                objGrid.Children.Add(objBuscadorEspecies)
                                objListaControlesGrid.Add(New clsControlesGrid With {.Nombre = objBuscadorEspecies.Name, .Elemento = objBuscadorEspecies})
                                Grid.SetRow(objBuscadorEspecies, intFilaActual)
                                Grid.SetColumn(objBuscadorEspecies, 1)
                            Case TipoFuenteDatos.TIPO_FUENTE_DATOS_BUSCADORCLIENTES.ToString.ToUpper
                                objBuscadorCliente = New A2ComunesControl.BuscadorCliente()
                                objBuscadorCliente.BuscarAlIniciar = False
                                objBuscadorCliente.MostrarLimpiar = True
                                objBuscadorCliente.Name = objParam.Parametro.Replace("@", "")

                                If Not String.IsNullOrEmpty(objParam.FuenteDatos) And Not String.IsNullOrEmpty(objParam.SeparadorFuenteDatos) Then
                                    For Each objInfoParametro In objParam.FuenteDatos.Split(CChar(objParam.SeparadorFuenteDatos))
                                        If Not IsNothing(objInfoParametro.Split(CChar("="))) Then
                                            Select Case objInfoParametro.Split(CChar("="))(0).ToUpper
                                                Case ParametrosBuscadorCliente.RECEPTOR.ToString.ToUpper
                                                    objBuscadorCliente.IDReceptor = objInfoParametro.Split(CChar("="))(1)
                                                Case ParametrosBuscadorCliente.TIPONEGOCIO.ToString.ToUpper
                                                    objBuscadorCliente.TipoNegocio = objInfoParametro.Split(CChar("="))(1)
                                                Case ParametrosBuscadorCliente.TIPOPRODUCTO.ToString.ToUpper
                                                    objBuscadorCliente.TipoProducto = objInfoParametro.Split(CChar("="))(1)
                                                Case ParametrosBuscadorCliente.ESTADO.ToString.ToUpper
                                                    Select Case objInfoParametro.Split(CChar("="))(1)
                                                        Case A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.A.ToString
                                                            objBuscadorCliente.EstadoComitente = A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.A
                                                        Case A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.I.ToString
                                                            objBuscadorCliente.EstadoComitente = A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.I
                                                        Case A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.T.ToString
                                                            objBuscadorCliente.EstadoComitente = A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.T
                                                        Case A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.B.ToString
                                                            objBuscadorCliente.EstadoComitente = A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.B
                                                        Case Else
                                                            objBuscadorCliente.EstadoComitente = A2ComunesControl.BuscadorClienteViewModel.EstadosComitente.T
                                                    End Select
                                                Case ParametrosBuscadorCliente.AGRUPAMIENTO.ToString.ToUpper
                                                    objBuscadorCliente.Agrupamiento = objInfoParametro.Split(CChar("="))(1)
                                                Case ParametrosBuscadorCliente.TIPOVINCULACION.ToString.ToUpper
                                                    Select Case objInfoParametro.Split(CChar("="))(1)
                                                        Case A2ComunesControl.BuscadorClienteViewModel.TiposVinculacion.C.ToString
                                                            objBuscadorCliente.TipoVinculacion = A2ComunesControl.BuscadorClienteViewModel.TiposVinculacion.C
                                                        Case A2ComunesControl.BuscadorClienteViewModel.TiposVinculacion.O.ToString
                                                            objBuscadorCliente.TipoVinculacion = A2ComunesControl.BuscadorClienteViewModel.TiposVinculacion.O
                                                        Case A2ComunesControl.BuscadorClienteViewModel.TiposVinculacion.T.ToString
                                                            objBuscadorCliente.TipoVinculacion = A2ComunesControl.BuscadorClienteViewModel.TiposVinculacion.T
                                                        Case Else
                                                            objBuscadorCliente.TipoVinculacion = A2ComunesControl.BuscadorClienteViewModel.TiposVinculacion.T
                                                    End Select
                                                Case ParametrosBuscadorCliente.CARGARCLIENTESRESTRICCION.ToString.ToUpper
                                                    If objInfoParametro.Split(CChar("="))(1) = "1" Then
                                                        objBuscadorCliente.CargarClientesRestriccion = True
                                                    Else
                                                        objBuscadorCliente.CargarClientesRestriccion = False
                                                    End If
                                                Case ParametrosBuscadorCliente.CARGARCLIENTESXTIPOPRODUCTOPERFIL.ToString.ToUpper
                                                    If objInfoParametro.Split(CChar("="))(1) = "1" Then
                                                        objBuscadorCliente.CargarClientesXTipoProductoPerfil = True
                                                    Else
                                                        objBuscadorCliente.CargarClientesXTipoProductoPerfil = False
                                                    End If
                                                Case ParametrosBuscadorCliente.CARGARCLIENTETERCERO.ToString.ToUpper
                                                    If objInfoParametro.Split(CChar("="))(1) = "1" Then
                                                        objBuscadorCliente.CargarClientesTercero = Visibility.Visible
                                                    Else
                                                        objBuscadorCliente.CargarClientesTercero = Visibility.Collapsed
                                                    End If
                                            End Select
                                        End If
                                    Next
                                End If

                                If (ListaDependenciaParametros.Where(Function(i) i.NombreParametro = objParam.Parametro)).Count > 0 Then
                                    AddHandler objBuscadorCliente.comitenteAsignadoControlOrigen, AddressOf buscadorCliente_comitenteAsignado
                                End If

                                objGrid.Children.Add(objBuscadorCliente)
                                objListaControlesGrid.Add(New clsControlesGrid With {.Nombre = objBuscadorCliente.Name, .Elemento = objBuscadorCliente})
                                Grid.SetRow(objBuscadorCliente, intFilaActual)
                                Grid.SetColumn(objBuscadorCliente, 1)
                        End Select

                        intFilaActual += 1

                    End If
                Next
            End If

            objDicCombos = Nothing
            objCombo = Nothing
            objTexto = Nothing
            objEtiqueta = Nothing

        Catch ex As Exception
            If Not objGrid Is Nothing Then
                objGrid.Children.Clear()
            End If

            objGrid.Children.Add(New TextBlock With {.Text = "No fue posible generar la lista de parámetros del script."})
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "crearControlesParametros", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Try
            objRetorno.GridRetorno = objGrid
            objRetorno.ListaControles = objListaControlesGrid
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "crearControlesParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objRetorno)
    End Function

    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            Dim objControl As TextBox = CType(sender, TextBox)
            RecargarParametroDependiente(objControl.Name, objControl.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "TextBox_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub DatePicker_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            Dim objControl As A2DatePicker = CType(sender, A2DatePicker)
            RecargarParametroDependiente(objControl.Name, objControl.SelectedDate.ToString)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "DatePicker_SelectedDateChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            Dim objControl As ComboBox = CType(sender, ComboBox)
            RecargarParametroDependiente(objControl.Name, objControl.SelectedValue.ToString)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "ComboBox_SelectionChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscadorGenerico_ItemAsignado(ByVal pstrControlOrigen As String, ByVal pstrIdItem As String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                RecargarParametroDependiente("@" & pstrControlOrigen, pobjItem.IdItem)
            Else
                RecargarParametroDependiente("@" & pstrControlOrigen, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "buscadorGenerico_ItemAsignado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscadorEspecie_especieAsignada(ByVal pstrControlOrigen As String, ByVal pstrNemotecnico As String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                RecargarParametroDependiente("@" & pstrControlOrigen, pobjEspecie.Nemotecnico)
            Else
                RecargarParametroDependiente("@" & pstrControlOrigen, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "buscadorEspecie_especieAsignada", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscadorCliente_comitenteAsignado(ByVal pstrControlOrigen As String, ByVal pstrIdComitente As String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                RecargarParametroDependiente("@" & pstrControlOrigen, pobjComitente.IdComitente)
            Else
                RecargarParametroDependiente("@" & pstrControlOrigen, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "buscadorCliente_comitenteAsignado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub RecargarParametroDependiente(ByVal pstrParametroCambio As String, ByVal pstrValorParametro As String)
        Try
            RaiseEvent actualizarControlesDependientes(pstrParametroCambio, pstrValorParametro)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "RecargarParametroDependiente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Captura y valida el valor de los paráetros del script y si cumple con las condiciones definidas ejecuta el script
    ''' </summary>
    ''' <param name="pobjParametros">Lista de controles asociados a los parámetros</param>
    ''' 
    Public Async Function ejecutarScript(ByVal pobjParametros As Dictionary(Of String, FrameworkElement)) As Task(Of Boolean)

        Const STR_SEPARADOR_VALOR As String = "="
        Const STR_SEPARADOR_PARAMETRO As String = ","

        Dim logResultado As Boolean = False
        Dim logErrorTipoDatos As Boolean
        Dim logErrorValorFueraRango As Boolean
        Dim logErrorDecimalesNoPermitidos As Boolean

        Dim strParametros As String = String.Empty
        Dim strParametrosVisualicion As String = String.Empty
        Dim strResultado As String = String.Empty
        Dim strValorParametro As String
        Dim objValorParametro As Object

        Try
            If EncabezadoSeleccionado Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("No hay ningún script seleccionado para ser ejecutado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                Return (False)
            End If

            For Each objParam In ListaParametros
                If pobjParametros.ContainsKey(objParam.Parametro) Then
                    logErrorTipoDatos = False
                    logErrorValorFueraRango = False
                    logErrorDecimalesNoPermitidos = False
                    strValorParametro = String.Empty
                    objValorParametro = Nothing

                    Select Case objParam.TipoFuenteDatos.ToUpper
                        Case TipoFuenteDatos.TIPO_FUENTE_DATOS_VALOR_FIJO.ToString.ToUpper

                            strValorParametro = CType(pobjParametros.Item(objParam.Parametro), TextBox).Text

                            If objParam.Requerido And strValorParametro.Equals(String.Empty) Then
                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ es requerido." & vbNewLine
                            ElseIf Not strValorParametro.Equals(String.Empty) Then
                                Select Case objParam.TipoDato.ToUpper
                                    Case TipoDatoParametro.TIPO_DATO_TEXTO.ToString.ToUpper
                                        ' Se reemplaza la comilla sencilla (') por el caraceter (`) para evitar conflictos en el envío a SQL Server en instrucciones dinámicas
                                        strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & strValorParametro.Replace("'", "`") & "'" & STR_SEPARADOR_PARAMETRO
                                        strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & strValorParametro.Replace("'", "`") & "'" & STR_SEPARADOR_PARAMETRO

                                    Case TipoDatoParametro.TIPO_DATO_ENTERO.ToString.ToUpper
                                        If validarDatosNumericos(strValorParametro, TipoDatoParametro.TIPO_DATO_ENTERO, logErrorTipoDatos, logErrorValorFueraRango, logErrorDecimalesNoPermitidos) Then
                                            If objParam.LongitudMaxima <> 0 Then
                                                strResultado &= "* El valor máximo del parámetro """ & objParam.Etiqueta & """ puede ser " & FormatNumber(objParam.LongitudMaxima, 0, , , TriState.True) & "." & vbNewLine
                                            Else
                                                strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & strValorParametro & STR_SEPARADOR_PARAMETRO
                                                strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & strValorParametro & STR_SEPARADOR_PARAMETRO
                                            End If
                                        Else
                                            If logErrorTipoDatos Then
                                                strResultado &= "* El parámetro """ & objParam.Etiqueta & """ debe ser un valor numérico entero sin decimales." & vbNewLine
                                            ElseIf logErrorValorFueraRango Then
                                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ debe estar entre " & FormatNumber(MINT_MIN_ENTERO, 0, , , TriState.True) & " y " & FormatNumber(MINT_MAX_ENTERO, 0, , , TriState.True) & "." & vbNewLine
                                            ElseIf logErrorDecimalesNoPermitidos Then
                                                strResultado &= "* El valor ingresado para el parámetro """ & objParam.Etiqueta & """ debe ser numérico entero sin decimales." & vbNewLine
                                            Else
                                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ no es válido." & vbNewLine
                                            End If
                                        End If

                                    Case TipoDatoParametro.TIPO_DATO_DECIMAL.ToString.ToUpper
                                        If validarDatosNumericos(strValorParametro, TipoDatoParametro.TIPO_DATO_DECIMAL, logErrorTipoDatos, logErrorValorFueraRango, logErrorDecimalesNoPermitidos) Then
                                            If objParam.LongitudMaxima <> 0 AndAlso objParam.LongitudMaxima < Math.Round(CDbl(strValorParametro), 6) Then
                                                strResultado &= "* El valor máximo del parámetro """ & objParam.Etiqueta & """ puede ser " & FormatNumber(objParam.LongitudMaxima, 6, , , TriState.True) & "." & vbNewLine
                                            Else
                                                strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & strValorParametro & STR_SEPARADOR_PARAMETRO
                                                strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & strValorParametro & STR_SEPARADOR_PARAMETRO
                                            End If
                                        Else
                                            If logErrorTipoDatos Then
                                                strResultado &= "* El parámetro """ & objParam.Etiqueta & """ debe ser un valor numérico entero sin decimales." & vbNewLine
                                            ElseIf logErrorValorFueraRango Then
                                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ debe estar entre " & FormatNumber(MINT_MIN_ENTERO, 0, , , TriState.True) & " y " & FormatNumber(MINT_MAX_ENTERO, 0, , , TriState.True) & "." & vbNewLine
                                            ElseIf logErrorDecimalesNoPermitidos Then
                                                strResultado &= "* El valor ingresado para el parámetro """ & objParam.Etiqueta & """ debe ser numérico entero sin decimales." & vbNewLine
                                            Else
                                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ no es válido." & vbNewLine
                                            End If
                                        End If

                                    Case Else
                                        If objParam.Requerido And strValorParametro.Equals(String.Empty) Then
                                            strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ es requerido." & vbNewLine
                                        Else
                                            ' Se reemplaza la comilla sencilla (') por el caraceter (`) para evitar conflictos en el envío a SQL Server en instrucciones dinámicas
                                            strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & strValorParametro.Replace("'", "`") & "'" & STR_SEPARADOR_PARAMETRO
                                            strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & strValorParametro.Replace("'", "`") & "'" & STR_SEPARADOR_PARAMETRO
                                        End If

                                End Select
                            End If

                        Case TipoFuenteDatos.TIPO_FUENTE_DATOS_FECHA.ToString.ToUpper

                            Dim dtmFecha As Date?



                            dtmFecha = CType(pobjParametros.Item(objParam.Parametro), A2DatePicker).SelectedDate

                            If objParam.Requerido And dtmFecha Is Nothing Then
                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ es requerido." & vbNewLine
                            ElseIf Not dtmFecha Is Nothing Then
                                Dim strFormatoFechaParametros As String = String.Empty

                                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strFormatoFechaParametros) Then
                                    strFormatoFechaParametros = _EncabezadoSeleccionado.strFormatoFechaParametros.ToUpper
                                    strFormatoFechaParametros = strFormatoFechaParametros.Replace("MM", Right("00" & dtmFecha.Value.Month().ToString, 2))
                                    strFormatoFechaParametros = strFormatoFechaParametros.Replace("DD", Right("00" & dtmFecha.Value.Day().ToString, 2))
                                    strFormatoFechaParametros = strFormatoFechaParametros.Replace("YYYY", dtmFecha.Value.Year().ToString)


                                    strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & strFormatoFechaParametros & "'" & STR_SEPARADOR_PARAMETRO
                                    strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & strFormatoFechaParametros & "'" & STR_SEPARADOR_PARAMETRO

                                Else
                                    strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & Right("00" & dtmFecha.Value.Month().ToString, 2) & "/" & Right("00" & dtmFecha.Value.Day().ToString, 2) & "/" & dtmFecha.Value.Year().ToString & "'" & STR_SEPARADOR_PARAMETRO
                                    strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & Right("00" & dtmFecha.Value.Month().ToString, 2) & "/" & Right("00" & dtmFecha.Value.Day().ToString, 2) & "/" & dtmFecha.Value.Year().ToString & "'" & STR_SEPARADOR_PARAMETRO
                                End If

                            End If

                        Case TipoFuenteDatos.TIPO_FUENTE_DATOS_COMBOA2.ToString.ToUpper, TipoFuenteDatos.TIPO_FUENTE_DATOS_COMBOA2ESPECIFICO.ToString.ToUpper, TipoFuenteDatos.TIPO_FUENTE_DATOS_LISTA_VALORES.ToString.ToUpper
                            ' CCM20150319: Incluir el tipo TIPO_FUENTE_DATOS_COMBOA2ESPECIFICO

                            objValorParametro = CType(pobjParametros.Item(objParam.Parametro), ComboBox).SelectedValue

                            If objParam.Requerido And (objValorParametro Is Nothing OrElse objValorParametro.ToString.Equals(String.Empty)) Then
                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ es requerido." & vbNewLine
                            ElseIf Not (objValorParametro Is Nothing OrElse objValorParametro.ToString.Equals(String.Empty)) Then
                                Select Case objParam.TipoDato.ToUpper
                                    Case TipoDatoParametro.TIPO_DATO_SI_NO.ToString.ToUpper, TipoDatoParametro.TIPO_DATO_TEXTO.ToString.ToUpper, TipoDatoParametro.TIPO_DATO_FECHA.ToString.ToUpper
                                        strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & objValorParametro.ToString & "'" & STR_SEPARADOR_PARAMETRO
                                        strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & objValorParametro.ToString & "'" & STR_SEPARADOR_PARAMETRO
                                    Case Else
                                        strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & objValorParametro.ToString & STR_SEPARADOR_PARAMETRO
                                        strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & objValorParametro.ToString & STR_SEPARADOR_PARAMETRO
                                End Select
                            End If
                        Case TipoFuenteDatos.TIPO_FUENTE_DATOS_BUSCADORGENERICO.ToString.ToUpper
                            objValorParametro = CType(pobjParametros.Item(objParam.Parametro), A2ComunesControl.BuscadorGenerico).ItemActivo

                            If objParam.Requerido And (objValorParametro Is Nothing) Then
                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ es requerido." & vbNewLine
                            ElseIf Not (objValorParametro Is Nothing) Then
                                strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & CType(objValorParametro, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.BuscadorGenerico).IdItem.ToString & "'" & STR_SEPARADOR_PARAMETRO
                                strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & CType(objValorParametro, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.BuscadorGenerico).IdItem.ToString & "'" & STR_SEPARADOR_PARAMETRO
                            End If
                        Case TipoFuenteDatos.TIPO_FUENTE_DATOS_BUSCADORESPECIES.ToString.ToUpper
                            objValorParametro = CType(pobjParametros.Item(objParam.Parametro), A2ComunesControl.BuscadorEspecie).EspecieActiva

                            If objParam.Requerido And (objValorParametro Is Nothing) Then
                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ es requerido." & vbNewLine
                            ElseIf Not (objValorParametro Is Nothing) Then
                                strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & CType(objValorParametro, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.BuscadorEspecies).Nemotecnico.ToString & "'" & STR_SEPARADOR_PARAMETRO
                                strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & CType(objValorParametro, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.BuscadorEspecies).Nemotecnico.ToString & "'" & STR_SEPARADOR_PARAMETRO
                            End If
                        Case TipoFuenteDatos.TIPO_FUENTE_DATOS_BUSCADORCLIENTES.ToString.ToUpper
                            objValorParametro = CType(pobjParametros.Item(objParam.Parametro), A2ComunesControl.BuscadorCliente).ComitenteActivo

                            If objParam.Requerido And (objValorParametro Is Nothing) Then
                                strResultado &= "* El valor del parámetro """ & objParam.Etiqueta & """ es requerido." & vbNewLine
                            ElseIf Not (objValorParametro Is Nothing) Then
                                strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & CType(objValorParametro, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.BuscadorClientes).CodigoOYD.ToString & "'" & STR_SEPARADOR_PARAMETRO
                                strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & CType(objValorParametro, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.BuscadorClientes).CodigoOYD.ToString & "'" & STR_SEPARADOR_PARAMETRO
                            End If
                    End Select
                Else
                    If objParam.TipoFuenteDatos.ToUpper().Equals(TipoFuenteDatos.TIPO_FUENTE_DATOS_MULTICOMPANIA.ToString.ToUpper) Then
                        If ListaCompaniasSeleccionadas.Trim().Equals(String.Empty) Then
                            strResultado &= "* Debe seleccionar por lo menos una compañía." & vbNewLine
                        Else
                            strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & _ListaCompaniasSeleccionadas & "'" & STR_SEPARADOR_PARAMETRO
                            strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & _ListaCompaniasSeleccionadas & "'" & STR_SEPARADOR_PARAMETRO
                        End If
                    ElseIf objParam.TipoFuenteDatos.ToUpper().Equals(TipoFuenteDatos.TIPO_FUENTE_DATOS_SISTEMA.ToString.ToUpper) Then
                        Select Case objParam.FuenteDatos.ToUpper
                            Case ParametrosSistema.MAQUINA.ToString.ToUpper
                                strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & Program.Maquina & "'" & STR_SEPARADOR_PARAMETRO
                                strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & Program.Maquina & "'" & STR_SEPARADOR_PARAMETRO
                            Case ParametrosSistema.USUARIO.ToString.ToUpper
                                strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & Program.Usuario & "'" & STR_SEPARADOR_PARAMETRO
                                strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & Program.Usuario & "'" & STR_SEPARADOR_PARAMETRO
                            Case ParametrosSistema.USUARIOWINDOWS.ToString.ToUpper
                                strParametros &= objParam.Parametro & STR_SEPARADOR_VALOR & "'" & Program.UsuarioWindows & "'" & STR_SEPARADOR_PARAMETRO
                                strParametrosVisualicion &= objParam.Etiqueta & STR_SEPARADOR_VALOR & "'" & Program.UsuarioWindows & "'" & STR_SEPARADOR_PARAMETRO
                        End Select
                    End If
                End If
            Next

            If String.IsNullOrEmpty(OpcionGeneracionSeleccionado) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se ha seleccionado el Destino.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return (False)
            Else
                If OpcionGeneracionSeleccionado = "PANTALLA" Then
                    If DisenoSeleccionado = 0 Or IsNothing(DisenoSeleccionado) Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se ha seleccionado el Diseño.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Return (False)
                    End If
                End If
            End If

            If strResultado.Equals(String.Empty) Then
                If Right(strParametros, 1) = "," Then
                    strParametros = strParametros.Substring(0, strParametros.Length - 1)
                End If
                If Right(strParametrosVisualicion, 1) = "," Then
                    strParametrosVisualicion = strParametrosVisualicion.Substring(0, strParametrosVisualicion.Length - 1)
                End If

                If OpcionGeneracionSeleccionado = "PANTALLA" Then
                    Return IniciarFormularioWPF(strParametros, strParametrosVisualicion)
                Else
                    Await ejecutarScriptBD(pintIdScript:=EncabezadoSeleccionado.ID, pstrNombreScript:=EncabezadoSeleccionado.Nombre, pstrValorParametros:=strParametros)
                    logResultado = True
                End If
            Else
                strResultado = "Por favor corregir las siguientes inconsistencias en los valores ingresados para los parámetros:" & vbNewLine & vbNewLine & strResultado
                A2Utilidades.Mensajes.mostrarMensaje(strResultado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para crear los controles de los parámetros", Me.ToString(), "crearControlesParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Private Function IniciarFormularioWPF(ByVal pstrParametrosEjecucion As String, ByVal pstrParametrosVisualizacion As String) As Boolean
        Dim logResultado As Boolean = False
        Try
            Return InvocarComponenteWPF(pstrParametrosEjecucion, pstrParametrosVisualizacion)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el formulario WPF", Me.ToString(), "IniciarFormularioWPF", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Validar el valor que se envía para saber si cumple con las condiciones del tipo de dato
    ''' </summary>
    ''' <param name="pstrValor">Valor que se debe validar</param>
    ''' <param name="pintTipoDato">Tipo de dato que debe tener el valor</param>
    ''' <param name="ologErrorTipoDato">Parámetro de salida que indica si hay un error en el tipo de dato</param>
    ''' <param name="ologValorFueraRango">Parámetro de salida que indica que el valor recibido está por fuera de los valores permitidos por el tipo de dato</param>
    ''' <param name="ologDecimalesNoPermitidos">Parámetro de salida que indica si el valor recibido tiene decimales pero el tipo de dato no lo permite</param>
    ''' 
    Private Function validarDatosNumericos(ByRef pstrValor As String, ByVal pintTipoDato As TipoDatoParametro, ByRef ologErrorTipoDato As Boolean, ByRef ologValorFueraRango As Boolean, ByRef ologDecimalesNoPermitidos As Boolean) As Boolean
        Dim dblValor As Double
        Dim dblValorDecimales As Double
        Dim strValorTexto As String
        Dim strSeparadorMiles As String
        Dim strSeparadorDec As String

        ologErrorTipoDato = False
        ologValorFueraRango = False
        ologDecimalesNoPermitidos = False

        If Not Versioned.IsNumeric(pstrValor) Then
            ologErrorTipoDato = True
            Return False
        Else
            strSeparadorMiles = FormatNumber(1000, 0, TriState.False, TriState.False, TriState.True)
            If strSeparadorMiles.IndexOf(",") > 0 Then
                strSeparadorMiles = ","
                strSeparadorDec = "."
            Else
                strSeparadorMiles = "."
                strSeparadorDec = ","
            End If

            strValorTexto = Replace(pstrValor, strSeparadorMiles, String.Empty)
            pstrValor = strValorTexto
            strValorTexto = pstrValor.Replace(",", "").Replace(".", "").Replace("-", "")
            If strValorTexto.Length > MINT_MAX_LONG_DECIMAL Then
                ologValorFueraRango = True
                Return False
            End If

            If pintTipoDato = TipoDatoParametro.TIPO_DATO_ENTERO Then
                If CDbl(pstrValor) > MINT_MAX_ENTERO Or CDbl(pstrValor) < MINT_MIN_ENTERO Then
                    ologValorFueraRango = True
                    Return False
                Else
                    dblValor = Math.Abs(CDbl(pstrValor)) - 1 ' Se resta uno por si se está en el máximo entero negativo que al tomar el valor absoluto queda mayor al máximo entero positivo
                    dblValorDecimales = dblValor - CInt(dblValor)  ' Valor decimal

                    If dblValorDecimales <> 0 Then
                        ologDecimalesNoPermitidos = True
                        Return False
                    End If
                End If
            ElseIf pintTipoDato = TipoDatoParametro.TIPO_DATO_DECIMAL Then

            End If

        End If

        Return True
    End Function

    ''' <summary>
    ''' Ejecutar script de acuerdo a los parámetros recibidos
    ''' </summary>
    ''' <param name="pintIdScript">Identificador único del script.</param>
    ''' <param name="pstrNombreScript">Nombre del reporte a ejecutar.</param>
    ''' <param name="pstrValorParametros">Parametros con los cuales ejecutara el reporte. Importante cuando los parametros sean de tipo fecha o varchar enviar doble comilla simple. Ejm: @pdtmFecha=''2013-06-14'', @pstrCiudad='''', @pstrSector='''', @pstrLogUsuario=''juan.correa'', @pstrMaquina=''pc-jcorrea''</param>
    ''' 
    Private Async Function ejecutarScriptBD(ByVal pintIdScript As Integer, ByVal pstrNombreScript As String, ByVal pstrValorParametros As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFUtilidades.MensajesProceso)
        Dim objResultado As MensajesProceso
        Dim objListaRes As List(Of A2.OyD.OYDServer.RIA.Web.CFUtilidades.MensajesProceso)
        Dim objVista As EjecutarScriptResultadoView
        Dim objProxyES As UtilidadesCFDomainContext
        Dim strUsuario As String = String.Empty

        Try
            If EncabezadoSeleccionado Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccionar el script que se debe ejecutar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)

                Return (False)
            End If

            IsBusy = True

            ErrorForma = String.Empty

            objProxyES = inicializarProxyUtilidades()

            objProxyES.MensajesProcesos.Clear()

            strUsuario = Program.Usuario
            'If A2Utilidades.Program.Usuario = Program.Usuario Then
            '    strUsuario = Program.Usuario
            'Else
            '    strUsuario = A2Utilidades.Program.Usuario
            'End If

            objRet = Await objProxyES.Load(objProxyES.ejecutarScriptSyncQuery(pintIdCompania:=-1,
                                                                            pintIdScript:=pintIdScript,
                                                                            pstrNombreScript:=pstrNombreScript,
                                                                            pstrParametros:=pstrValorParametros,
                                                                            pstrMaquina:=Program.Maquina,
                                                                            pstrUsuario:=strUsuario, pstrInfoConexion:=Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los parámetros del scripts pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los parámetros del script.", Me.ToString(), "cargarDefinicionParametrosScript", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    objListaRes = objProxyES.MensajesProcesos.ToList
                    objResultado = objListaRes.First

                    If objResultado Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se obtuvo respuesta de la ejecución del script. No se puede determinar si fue o no ejecutado con éxito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf objResultado.TipoLinea.Equals("MENSAJE") And objResultado.NombreArchivo.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(objResultado.Linea, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    ElseIf objResultado.TipoLinea.Equals(String.Empty) And objResultado.NombreArchivo.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El script fue ejecutado exitosamente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    ElseIf objResultado.TipoLinea.Equals("INCONSISTENCIA") And objResultado.NombreArchivo.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se obtuvo respuesta de la ejecución del script. No se puede determinar si fue o no ejecutado con éxito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        objVista = New EjecutarScriptResultadoView(objListaRes)
                        Program.Modal_OwnerMainWindowsPrincipal(objVista)
                        objVista.ShowDialog()
                    End If
                End If
            End If

            MyBase.CambioItem("ListaEncabezado")

            logResultado = True
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción la definción de los parámetros del script", Me.ToString(), "cargarDefinicionParametrosScript", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False

            objProxyES = Nothing
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    ''' 
    Private Sub prepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaScripts
            objCB.Grupo = String.Empty
            objCB.Nombre = String.Empty
            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "prepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ActualizarRegistro
    ''' </summary>
    ''' 
    Private Function validarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.Grupo) Then
                    strMsg = String.Format("{0}{1} + El código asignado para identificar el tipo de entidad que transmite es requerido.", strMsg, vbCrLf)
                End If

                If String.IsNullOrEmpty(_EncabezadoSeleccionado.Nombre) Then
                    strMsg = String.Format("{0}{1} + El código asignado para identificar a la entidad que transmite es requerido.", strMsg, vbCrLf)
                End If

                If String.IsNullOrEmpty(_EncabezadoSeleccionado.Descripcion) Then
                    strMsg = String.Format("{0}{1} + La descripción del tipo de transmisión es requerida.", strMsg, vbCrLf)
                End If
            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar un tipo de transmisión", strMsg, vbCrLf)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "validarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el cambio de propiedades del encabezado.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Metodo que permite ejecutar el ImprimirReporte.exe
    ''' </summary>
    Private Function InvocarComponenteWPF(ByVal pstrParametrosConsulta As String, ByVal pstrParametrosVisualizacion As String) As Boolean
        Try
            IsBusy = True

            Dim objParametros As New clsParametrosEjecutarScript
            objParametros.pintIDScript = _EncabezadoSeleccionado.ID
            objParametros.pstrNombreScript = _EncabezadoSeleccionado.Nombre
            objParametros.pstrDescripcionScript = _EncabezadoSeleccionado.Descripcion
            objParametros.pstrNombreExportacion = _EncabezadoSeleccionado.NombreArchivo
            objParametros.pintIDCompania = -1
            objParametros.pstrParametrosFiltro = pstrParametrosConsulta
            objParametros.pstrParametrosVisualizar = pstrParametrosVisualizacion
            objParametros.pintIDDisenoDefecto = DisenoSeleccionado.ToString

            Dim objEjecutar As New EjecutarScriptGridPantallaView(JsonConvert.SerializeObject(objParametros))
            objEjecutar.Show()

            IsBusy = False
            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar las utilidades.",
               Me.ToString(), "InvocarComponenteWPF", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try

    End Function

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando finaliza la ejecución de una actualización a la base de datos
    ''' </summary>
    ''' 
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Dim strMsg As String = String.Empty

        Try

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

    Public Async Sub CargarOpcionesScript()
        Try
            CargarComboDestino()
            Await consultarDisenosScript()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar las opciones del script.", Me.ToString(), "CargarOpcionesScript", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de scripts
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>
    ''' <param name="pstrGrupo">Texto que permite agrupar varios informes para temas informativos</param>
    ''' <param name="pstrNombre">Nombre del script</param>
    ''' <param name="pstrDescripcion">Descripción del script</param>
    ''' <param name="pstrTipoResultado">Indica si el resultado genera un texto, excel o no retorna resultado</param>
    ''' <param name="pstrTipoProceso">Indica si el proceso es sincrónico o asincrónico</param>
    ''' 
    Private Async Function consultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               ByVal pintIdScript As Integer,
                                               ByVal pstrMostrarTodos As String,
                                               ByVal pstrGrupo As String,
                                               ByVal pstrNombre As String,
                                               ByVal pstrDescripcion As String,
                                               ByVal plogUsuario As Integer,
                                               Optional ByVal pstrTipoResultado As String = "",
                                               Optional ByVal pstrTipoProceso As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim strUsuario As String = String.Empty
        Dim objRet As LoadOperation(Of ScriptsA2)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyUtilidades()
            End If

            mdcProxyActualizar.ScriptsA2s.Clear()

            strUsuario = Program.Usuario
            'If A2Utilidades.Program.Usuario = Program.Usuario Then
            '    strUsuario = Program.Usuario
            'Else
            '    strUsuario = A2Utilidades.Program.Usuario
            'End If

            If plogFiltrar Then
                If logEsFiltroGrupo Then
                    pstrMostrarTodos = "G"
                    pstrFiltro = String.Format("{0}={1}", pstrGrupo, pstrFiltro)
                ElseIf logEsSoloUnScript Then
                    pstrMostrarTodos = "E"
                End If

                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación

                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.filtrarScriptsSyncQuery(pstrFiltro:=pstrFiltro,
                                                                                                         pstrMostrarTodos:=pstrMostrarTodos,
                                                                                                         plngIDCia:=-1,
                                                                                                         pstrMaquina:=Program.Maquina,
                                                                                                         pstrUsuario:=strUsuario,
                                                                                                         pstrUsuarioLlamado:=Program.Usuario,
                                                                                                         pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.consultarScriptsSyncQuery(pintIdScript:=pintIdScript,
                                                                                                    pstrMostrarTodos:=pstrMostrarTodos,
                                                                                                    plngIDCia:=-1,
                                                                                                    pstrGrupo:=pstrGrupo,
                                                                                                    pstrNombre:=pstrNombre,
                                                                                                    pstrDescripcion:=pstrDescripcion,
                                                                                                    plogUsuario:=plogUsuario,
                                                                                                    pstrTipoResultado:=pstrTipoResultado,
                                                                                                    pstrTipoProceso:=pstrTipoProceso,
                                                                                                    pstrMaquina:=Program.Maquina,
                                                                                                    pstrUsuario:=strUsuario,
                                                                                                    pstrUsuarioLlamado:=Program.Usuario,
                                                                                                    pstrInfoConexion:=Program.HashConexion)).AsTask()
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "consultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEncabezado = mdcProxyActualizar.ScriptsA2s


                    If objRet.Entities.Count > 0 Then
                        If logEsSoloUnScript Then
                            EncabezadoSeleccionado = ListaEncabezado.First
                            Await cargarDefinicionParametrosScript(EncabezadoSeleccionado.ID)
                        Else
                            If Not plogFiltrar Then
                                MyBase.ConfirmarBuscar()
                            End If
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de scripts ", Me.ToString(), "consultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consulta la definición de los parámetros de un script
    ''' </summary>
    ''' <param name="pintIdScript">Id del script</param>
    ''' <remarks></remarks>
    ''' 
    Private Async Function cargarDefinicionParametrosScript(ByVal pintIdScript As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of ScriptsA2Parametros)
        Dim strUsuario As String = String.Empty
        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyUtilidades()
            End If

            strUsuario = Program.Usuario
            'If A2Utilidades.Program.Usuario = Program.Usuario Then
            '    strUsuario = Program.Usuario
            'Else
            '    strUsuario = A2Utilidades.Program.Usuario
            'End If

            mdcProxyActualizar.ScriptsA2Parametros.Clear()

            objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.consultarParametrosScriptSyncQuery(pintIdScript:=pintIdScript,
                                                                                                        plngIDCia:=-1,
                                                                                                        pstrMaquina:=Program.Maquina,
                                                                                                        pstrUsuario:=strUsuario, pstrInfoConexion:=Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los parámetros del scripts pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los parámetros del script.", Me.ToString(), "cargarDefinicionParametrosScript", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaParametros = mdcProxyActualizar.ScriptsA2Parametros

                    'actualiza la lista dependencia de parametros
                    ListaDependenciaParametros = New List(Of clsDependenciaParametros)

                    For Each li In ListaParametros
                        If Not String.IsNullOrEmpty(li.strParametroDependencia) Then
                            If ListaParametros.Where(Function(i) i.Parametro = li.strParametroDependencia).Count > 0 Then
                                ListaDependenciaParametros.Add(New clsDependenciaParametros With {
                                    .NombreDependencia = li.Parametro,
                                    .NombreParametro = li.strParametroDependencia
                                    })
                            Else
                                li.strParametroDependencia = String.Empty
                            End If
                        End If
                    Next

                    ' CCM20150319: Se elimina la condición que validaba si existén registros. Se llama siempre para soportar scripts sin parámetros.
                    RaiseEvent actualizarControles()
                End If
            Else
                ListaEncabezado.Clear()
            End If

            MyBase.CambioItem("ListaEncabezado")

            logResultado = True
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción la definción de los parámetros del script", Me.ToString(), "cargarDefinicionParametrosScript", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consultar de forma sincrónica los diseños habilitados para el scripts
    ''' </summary>
    ''' 
    Public Async Function consultarDisenosScript() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim strUsuario As String = String.Empty
        Dim objRet As LoadOperation(Of ScriptsA2DisenosPrincipales)

        Try
            ListaDisenos = Nothing

            If Not IsNothing(_EncabezadoSeleccionado) Then

                If mdcProxyActualizar Is Nothing Then
                    mdcProxyActualizar = inicializarProxyUtilidades()
                End If

                mdcProxyActualizar.ScriptsA2DisenosPrincipales.Clear()

                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.ejecutarScriptDiseno_ConsultarPrincipalesSyncQuery(_EncabezadoSeleccionado.ID, Program.Usuario, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "consultarDisenosScript", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        ListaDisenos = objRet.Entities.ToList
                        DisenoSeleccionado = -1

                        For Each li In ListaDisenos
                            If Not IsNothing(li.Defecto) Then
                                If li.Defecto Then
                                    DisenoSeleccionado = li.IDScriptDiseno
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If

                logResultado = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de Diseños ", Me.ToString(), "consultarDisenosScript", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consultar de forma sincrónica los diseños habilitados para el scripts
    ''' </summary>
    ''' 
    Public Sub ConsultarInformacionComboDependiente(ByVal pstrValoresParametroRecargar As String)
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                IsBusy = True

                If mdcProxyUtils Is Nothing Then
                    mdcProxyUtils = inicializarProxyUtilidadesOYD()
                End If

                mdcProxyUtils.ItemCombos.Clear()

                mdcProxyUtils.Load(mdcProxyUtils.cargarCombosEspecificosQuery(MSTR_COMBOS_ESPECIFICOS, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, pstrValoresParametroRecargar)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la listas dependientes. ", Me.ToString(), "ConsultarInformacionComboDependiente", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then
                Dim strParametroDependiente As String = lo.UserState.ToString.Split(",")(0)
                Dim strFuenteDatos As String = lo.UserState.ToString.Split(",")(1)
                Dim strValorFiltro As String = lo.UserState.ToString.Split(",")(2)
                Dim objListaNueva As New List(Of OYDUtilidades.ItemCombo)

                For Each li In lo.Entities.ToList
                    If li.Categoria = strFuenteDatos Then
                        If String.IsNullOrEmpty(li.Retorno) Then
                            objListaNueva.Add(li)
                        Else
                            If li.Retorno = strValorFiltro Then
                                objListaNueva.Add(li)
                            End If
                        End If
                    End If
                Next

                RaiseEvent actualizarListaComboDependiente(strParametroDependiente, objListaNueva)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la listas dependientes. ", Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la listas dependientes. ", Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, ex)
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
Public Class CamposBusquedaScripts
    Implements INotifyPropertyChanged

    <Display(Name:="IdScript")>
    Public Property IdScript As Integer

    <Display(Name:="Grupo")>
    Public Property Grupo As String

    <Display(Name:="Nombre")>
    Public Property Nombre As String

    <Display(Name:="Descripcion")>
    Public Property Descripcion As String

    <Display(Name:="TipoResultado")>
    Public Property TipoResultado As String

    <Display(Name:="TipoProceso")>
    Public Property TipoProceso As String

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

Public Class clsParametrosEjecutarScript
    <JsonProperty("ID", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pintIDScript As Integer

    <JsonProperty("N", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrNombreScript As String

    <JsonProperty("D", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrDescripcionScript As String

    <JsonProperty("E", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrNombreExportacion As String

    <JsonProperty("IDC", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pintIDCompania As Nullable(Of Integer)

    <JsonProperty("PF", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrParametrosFiltro As String

    <JsonProperty("PV", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrParametrosVisualizar As String

    <JsonProperty("IDD", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pintIDDisenoDefecto As Nullable(Of Integer)
End Class

Public Class clsDependenciaParametros
    Public Property NombreParametro As String
    Public Property NombreDependencia As String
End Class

Public Class clsObjetosAgregados
    Public Property GridRetorno As Grid
    Public Property ListaControles As List(Of clsControlesGrid)
End Class

Public Class clsControlesGrid
    Public Property Nombre As String
    Public Property Elemento As UIElement
End Class