Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web


Imports System.Threading.Tasks
Imports Microsoft.VisualBasic.CompilerServices
Imports A2Utilidades.Mensajes
Imports A2ControlMenu
Imports A2.OyD.OYDServer.RIA.Web
Imports OyDCtl = A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web.CFUtilidades
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades

Public Class SeguridadEjecutarScriptsViewModel
    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged


#Region "Eventos"

    Public Event actualizarControles()

#End Region

#Region "Variables - REQUERIDO"


    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Variables
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public logEditar As Boolean = False
    Public logCambio As Boolean = False

    Public strScriptPermiso As String = String.Empty
    Public IDScriptPermiso As Integer = 0

    Dim strScripts As String = String.Empty
    Dim strListaScripts As String = String.Empty
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxyActualizar As UtilidadesCFDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios


    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjScriptsAnterior As ScriptsA2
    Private mobjPorDefecto As ScriptsA2
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private logCambiarPropiedades As Boolean = True

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Id del script seleccionado
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mintIdScriptAnterior As Integer = -1

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
        Dim objA2ViewModel As A2UtilsViewModel

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando

                ' Notificar cambio de compañía activa en caso de no tener una seleccionada al crear el view model
                MyBase.CambioItem("DescripcionCompaniaActiva")

                objA2ViewModel = New A2UtilsViewModel
                Await objA2ViewModel.inicializarCombos(String.Empty, String.Empty)

                EncabezadoSeleccionado = Nothing
                If Not ListaEncabezado Is Nothing Then
                    ListaEncabezado.Clear()
                End If

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await consultarEncabezado(True, String.Empty, MostrarTodoScripts, -1, String.Empty, String.Empty, String.Empty, 0, String.Empty, String.Empty) ' Consultar iniciales que se presentan en la lista de Normas Contables

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
            objA2ViewModel = Nothing
        End Try

        Return (logResultado)

    End Function

    ''' <summary>
    ''' Procedimiento que se llama desde el proceso de inicializar para llamar los procesos específicos del control
    ''' </summary>
    ''' 
    Public Async Function inicializacionEspecifica() As Task(Of Boolean)
        Dim logResultado As Boolean = True
        Try
            Dim objA2ViewModel As New A2UtilsViewModel
            Await objA2ViewModel.inicializarCombos(String.Empty, String.Empty)

            ListaCombosGenerales = objA2ViewModel.DiccionarioCombos

            Await inicializar()

        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la incialización de los datos específicos del control.", Me.ToString(), "inicializacionEspecifica", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _HabilitarScripts As Boolean
    Public Property HabilitarScripts() As Boolean
        Get
            Return _HabilitarScripts
        End Get
        Set(ByVal value As Boolean)
            _HabilitarScripts = value
            MyBase.CambioItem("HabilitarScripts")
        End Set
    End Property

    Private _HabilitarCodigo As Boolean
    Public Property HabilitarCodigo() As Boolean
        Get
            Return _HabilitarCodigo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodigo = value
            MyBase.CambioItem("HabilitarCodigo")
        End Set
    End Property



    Private _MostrarTodoScripts As String = "S"
    Public Property MostrarTodoScripts() As String
        Get
            Return _MostrarTodoScripts
        End Get
        Set(ByVal value As String)
            _MostrarTodoScripts = value
            MyBase.CambioItem("MostrarTodoScripts")
        End Set
    End Property

    Private _ListaUsuarios As EntitySet(Of Usuarios)
    Public Property ListaUsuarios() As EntitySet(Of Usuarios)
        Get
            Return _ListaUsuarios
        End Get
        Set(ByVal value As EntitySet(Of Usuarios))
            _ListaUsuarios = value

            MyBase.CambioItem("ListaUsuarios")
            MyBase.CambioItem("ListaUsuariosPaged")
            If Not IsNothing(_ListaUsuarios) Then
                UsuariosSelected = _ListaUsuarios.FirstOrDefault
            End If
        End Set
    End Property

    Private _ListaUsuariosPaged As PagedCollectionView
    Public ReadOnly Property ListaUsuariosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaUsuarios) Then
                If IsNothing(_ListaUsuariosPaged) Then
                    Dim view = New PagedCollectionView(_ListaUsuarios)
                    _ListaUsuariosPaged = view
                    Return view
                Else
                    Return _ListaUsuariosPaged
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _UsuarioCombo As String
    Public Property UsuarioCombo() As String
        Get
            Return _UsuarioCombo
        End Get
        Set(ByVal value As String)
            _UsuarioCombo = value
            If Not String.IsNullOrEmpty(_UsuarioCombo) Then
                consultarScripts(_UsuarioCombo)
            End If

            MyBase.CambioItem("UsuarioCombo")
        End Set
    End Property



    Private WithEvents _UsuariosSelected As Usuarios
    Public Property UsuariosSelected() As Usuarios
        Get
            Return _UsuariosSelected
        End Get
        Set(ByVal value As Usuarios)
            _UsuariosSelected = value
            MyBase.CambioItem("UsuariosSelected")
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
            If Not IsNothing(_ListaEncabezado) Then
                EncabezadoSeleccionado = _ListaEncabezado.FirstOrDefault
            End If

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
                consultarUsuarioPorScripts(EncabezadoSeleccionado.ID, Program.Usuario, Program.Maquina)
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaSeguridadScripts
    Public Property cb() As CamposBusquedaSeguridadScripts
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaSeguridadScripts)
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

    Private _ListaCombosGenerales As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
    Public Property ListaCombosGenerales() As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
        Get
            Return _ListaCombosGenerales
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)))
            _ListaCombosGenerales = value
            MyBase.CambioItem("ListaCombosGenerales")
        End Set
    End Property

    Private _VerConsulta As Visibility = Visibility.Visible
    Public Property VerConsulta() As Visibility
        Get
            Return _VerConsulta
        End Get
        Set(ByVal value As Visibility)
            _VerConsulta = value
            MyBase.CambioItem("VerConsulta")
        End Set
    End Property

    Private _VerIngreso As Visibility = Visibility.Collapsed
    Public Property VerIngreso() As Visibility
        Get
            Return _VerIngreso
        End Get
        Set(ByVal value As Visibility)
            _VerIngreso = value
            MyBase.CambioItem("VerIngreso")
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

    Private _CodNorma As String
    Public Property CodNorma() As String
        Get
            Return _CodNorma
        End Get
        Set(ByVal value As String)
            _CodNorma = value
            MyBase.CambioItem("CodNorma")
        End Set
    End Property

    Private _NormaContableSeleccionada As ItemCombo
    Public Property NormaContableSeleccionada() As ItemCombo
        Get
            Return _NormaContableSeleccionada
        End Get
        Set(ByVal value As ItemCombo)
            _NormaContableSeleccionada = value
            If Not IsNothing(_NormaContableSeleccionada) Then
                CodNorma = _NormaContableSeleccionada.ID
            End If
            MyBase.CambioItem("NormaContableSeleccionada")
        End Set
    End Property

    Private _PermitirCambiarNorma As Boolean = True
    Public Property PermitirCambiarNorma() As Boolean
        Get
            Return _PermitirCambiarNorma
        End Get
        Set(ByVal value As Boolean)
            _PermitirCambiarNorma = value
            MyBase.CambioItem("PermitirCambiarNorma")
        End Set
    End Property

    Private WithEvents _ListaScriptsAutorizados As List(Of ScriptXUsuarios)
    Public Property ListaScriptsAutorizados() As List(Of ScriptXUsuarios)
        Get
            Return _ListaScriptsAutorizados
        End Get
        Set(ByVal value As List(Of ScriptXUsuarios))
            _ListaScriptsAutorizados = value
            MyBase.CambioItem("ListaScriptsAutorizados")
        End Set
    End Property

    Private _ListaScripts As New List(Of ScriptXUsuarios)
    Public Property ListaScripts() As List(Of ScriptXUsuarios)
        Get
            Return _ListaScripts
        End Get
        Set(ByVal value As List(Of ScriptXUsuarios))
            _ListaScripts = value
            MyBase.CambioItem("ListaScripts")
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

            Await consultarEncabezado(True, FiltroVM, MostrarTodoScripts, -1, String.Empty, String.Empty, String.Empty, 0, String.Empty, String.Empty)

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
        consultarUsuarioPorScripts(0, Program.Usuario, Program.Maquina)
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

            Dim intConsultUsuario As Integer = 0

            If Not String.IsNullOrEmpty(cb.Grupo) Or
                Not String.IsNullOrEmpty(cb.Nombre) Or
                Not String.IsNullOrEmpty(cb.strUsuariocb) Then 'Validar que ingresó algo en los campos de búsqueda

                If Not IsNothing(cb.strUsuariocb) Then
                    If cb.strUsuariocb <> "" Then
                        intConsultUsuario = 1
                    End If
                End If

                Await consultarEncabezado(False, String.Empty, MostrarTodoScripts, cb.IdScript, cb.Grupo, cb.Nombre, String.Empty, intConsultUsuario, cb.strUsuariocb, String.Empty, String.Empty)
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
            ErrorForma = String.Empty
            IsBusy = True


            If validarRegistro() Then
                ConcatenarScripts()

                mdcProxyActualizar.ActualizarScriptsPermisos(strListaScripts, UsuarioCombo, Program.Maquina, Program.Usuario, Program.HashConexion, AddressOf TerminoActualizar, String.Empty)
            End If

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

        logEditar = True
        If Not IsNothing(ListaScriptsAutorizados) Then
            ListaScriptsAutorizados = Nothing
        End If
        If Not IsNothing(ListaScripts) Then
            ListaScripts = Nothing
        End If

        Dim objScripts As ScriptsA2
        VerIngreso = Visibility.Visible
        Try

            HabilitarCodigo = True
            objScripts = New ScriptsA2

            If Not IsNothing(EncabezadoSeleccionado) Then
                mobjScriptsAnterior = EncabezadoSeleccionado
            End If
            mobjScriptsAnterior = EncabezadoSeleccionado
            EncabezadoSeleccionado = objScripts

            EncabezadoSeleccionado.Usuario = Program.Usuario
            EncabezadoSeleccionado.IdCia = -1

            Editando = True
            MyBase.CambioItem("Editando")
            VerConsulta = Visibility.Collapsed
            HabilitarScripts = True
            consultarUsuarioPorScripts(0, Program.Usuario, Program.Maquina)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar crear nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub CancelarEditarRegistro()
        Try

            If Not IsNothing(ListaScriptsAutorizados) Then
                ListaScriptsAutorizados.Clear()
            End If
            If Not IsNothing(ListaScripts) Then
                ListaScripts.Clear()
            End If

            VerIngreso = Visibility.Collapsed
            VerConsulta = Visibility.Visible
            ErrorForma = String.Empty
            HabilitarScripts = False
            Editando = True
            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxyActualizar.RejectChanges()
                Editando = False
                EncabezadoSeleccionado = mobjScriptsAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
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


    Public Sub DesautorizaScriptsTodos()
        Try
            logCambio = True
            Dim obj As New List(Of ScriptXUsuarios)
            Dim obj1 As New List(Of ScriptXUsuarios)
            If Not IsNothing(ListaScripts) Then
                obj = ListaScripts
            End If
            If Not IsNothing(ListaScriptsAutorizados) Then
                If ListaScriptsAutorizados.Count > 0 Then
                    obj1 = ListaScriptsAutorizados

                    For Each li In ListaScriptsAutorizados
                        Dim objUsuariosxScripts As New ScriptXUsuarios
                        Program.CopiarObjeto(Of ScriptXUsuarios)(li, objUsuariosxScripts)
                        objUsuariosxScripts.IdScripts = li.IdScripts
                        objUsuariosxScripts.Seleccionada = False
                        obj.Add(objUsuariosxScripts)
                    Next
                    obj = obj.OrderBy(Function(i) i.Scripts).ToList
                    ListaScripts = Nothing
                    ListaScripts = obj
                    If Not IsNothing(ListaScripts) Then
                        For Each x In ListaScripts
                            If obj1.Where(Function(i) i.IdScripts = x.IdScripts).Count > 0 Then
                                obj1.Remove(obj1.Where(Function(i) i.IdScripts = x.IdScripts).First)
                            End If
                        Next
                    End If

                    ListaScriptsAutorizados = Nothing
                    ListaScriptsAutorizados = obj1
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al desautorizar todos los scripts.", Me.ToString(), "DesautorizaScriptsTodos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub DesautorizaScripts()
        Try
            logCambio = True
            Dim obj As New List(Of ScriptXUsuarios)
            Dim obj1 As New List(Of ScriptXUsuarios)
            If Not IsNothing(ListaScripts) Then
                obj = ListaScripts
            End If

            If Not IsNothing(ListaScriptsAutorizados) Then
                If ListaScriptsAutorizados.Count > 0 Then
                    obj1 = ListaScriptsAutorizados

                    For Each li In ListaScriptsAutorizados.Where(Function(i) CBool(i.Seleccionada) = True)
                        Dim objusuariosXScripts As New ScriptXUsuarios
                        Program.CopiarObjeto(Of ScriptXUsuarios)(li, objusuariosXScripts)
                        objusuariosXScripts.IdScripts = li.IdScripts
                        objusuariosXScripts.Seleccionada = False
                        obj.Add(objusuariosXScripts)
                    Next
                    obj = obj.OrderBy(Function(i) i.Scripts).ToList
                    ListaScripts = Nothing
                    ListaScripts = obj
                    If Not IsNothing(ListaScripts) Then
                        For Each x In ListaScripts
                            If obj1.Where(Function(i) i.IdScripts = x.IdScripts).Count > 0 Then
                                obj1.Remove(obj1.Where(Function(i) i.IdScripts = x.IdScripts).First)
                            End If
                        Next
                    End If

                    ListaScriptsAutorizados = Nothing
                    ListaScriptsAutorizados = obj1
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al desautorizar todos los scripts.", Me.ToString(), "DesautorizaScripts", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub AutorizaScriptsTodas()

        Try
            logCambio = True
            Dim obj As New List(Of ScriptXUsuarios)
            Dim obj1 As New List(Of ScriptXUsuarios)
            If Not IsNothing(ListaScriptsAutorizados) Then
                obj = ListaScriptsAutorizados
            End If
            If Not IsNothing(ListaScripts) Then
                If ListaScripts.Count > 0 Then
                    obj1 = ListaScripts

                    For Each li In ListaScripts
                        Dim objusuariosXScripts As New ScriptXUsuarios
                        Program.CopiarObjeto(Of ScriptXUsuarios)(li, objusuariosXScripts)
                        objusuariosXScripts.IdScripts = li.IdScripts
                        objusuariosXScripts.Seleccionada = False
                        obj.Add(objusuariosXScripts)
                    Next
                    obj = obj.OrderBy(Function(i) i.Scripts).ToList
                    ListaScriptsAutorizados = Nothing
                    ListaScriptsAutorizados = obj
                    If Not IsNothing(ListaScriptsAutorizados) Then
                        For Each x In ListaScriptsAutorizados
                            If obj1.Where(Function(i) i.IdScripts = x.IdScripts).Count > 0 Then
                                obj1.Remove(obj1.Where(Function(i) i.IdScripts = x.IdScripts).First)
                            End If
                        Next
                    End If

                    ListaScripts = Nothing
                    ListaScripts = obj1
                End If

            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al autorizar todos los scripts.", Me.ToString(), "AutorizaScriptsTodas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub AutorizaScripts()
        Try
            logCambio = True
            Dim obj As New List(Of ScriptXUsuarios)
            Dim obj1 As New List(Of ScriptXUsuarios)
            If Not IsNothing(ListaScriptsAutorizados) Then
                obj = ListaScriptsAutorizados
            End If
            If Not IsNothing(ListaScripts) Then
                If ListaScripts.Count > 0 Then
                    obj1 = ListaScripts

                    For Each li In ListaScripts.Where(Function(i) CBool(i.Seleccionada) = True)
                        Dim objusuariosXScripts As New ScriptXUsuarios
                        Program.CopiarObjeto(Of ScriptXUsuarios)(li, objusuariosXScripts)
                        objusuariosXScripts.IdScripts = li.IdScripts
                        objusuariosXScripts.Seleccionada = False
                        obj.Add(objusuariosXScripts)
                    Next

                    obj = obj.OrderBy(Function(i) i.Scripts).ToList

                    ListaScriptsAutorizados = Nothing
                    ListaScriptsAutorizados = obj
                    If Not IsNothing(ListaScriptsAutorizados) Then
                        For Each x In ListaScriptsAutorizados
                            If obj1.Where(Function(i) i.IdScripts = x.IdScripts).Count > 0 Then
                                obj1.Remove(obj1.Where(Function(i) i.IdScripts = x.IdScripts).First)
                            End If
                        Next
                    End If
                    obj1 = obj1.OrderBy(Function(i) i.Scripts).ToList
                    ListaScripts = Nothing
                    ListaScripts = obj1
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al autorizar los scripts.", Me.ToString(), "AutorizaScripts", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Public Sub ConcatenarScripts()
        Try
            Dim logPermitir = True

            If Not IsNothing(ListaScriptsAutorizados) Then
                For Each li In ListaScriptsAutorizados
                    If logPermitir Then
                        strScripts = String.Format("{0}", li.IdScripts)
                        logPermitir = False
                    Else
                        strScripts = String.Format("{0}|{1}", strScripts, li.IdScripts)
                    End If
                Next
            End If
            strListaScripts = strScripts
        Catch ex As Exception

            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concatenar los scripts.", Me.ToString(), "ConcatenarScripts", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    ''' <history>
    ''' Modificado por   : Juan Carlos Soto (JCS). 
    ''' Descripcion      : Se ajusta codigo y se adiciona funcionlidad para el cambio de grupos sin guardar.   
    ''' Fecha            : Noviembre 28/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Noviembre 28/2013 - Resultado Ok 
    ''' </history> 
    Private Async Function TerminoActualizar(lo As InvokeOperation(Of Integer)) As Task
        Try
            If lo.HasError = False Then
                Editando = False
                logCambio = False
                HabilitarScripts = False
                VerIngreso = Visibility.Collapsed
                VerConsulta = Visibility.Visible
                Filtrar()
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar el proceso de actualización.", Me.ToString(), "TerminoActualizar", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Function


#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    ''' 
    Private Sub prepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaSeguridadScripts
            objCB.Grupo = String.Empty
            objCB.Nombre = String.Empty
            objCB.strUsuariocb = String.Empty
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

        Dim logValidacion As Boolean = True
        Dim strMensajeValidacion As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If String.IsNullOrEmpty(_UsuarioCombo) Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} + Debe seleccionar un usuario.", strMensajeValidacion, vbCrLf)
            End If


            If logValidacion = False Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("Validaciones:" & vbCrLf & strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                Return False
            Else
                strMensajeValidacion = String.Empty
                Return True
            End If

        Catch ex As Exception
            IsBusy = False
            Return False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "validarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el cambio de propiedades del encabezado.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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
                                               ByVal pstrMostrarTodos As String,
                                               ByVal pintIdScript As Integer,
                                               ByVal pstrGrupo As String,
                                               ByVal pstrNombre As String,
                                               ByVal pstrDescripcion As String,
                                               ByVal plogUsuario As Integer,
                                               ByVal pstrUsuario As String,
                                               Optional ByVal pstrTipoResultado As String = "",
                                               Optional ByVal pstrTipoProceso As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of ScriptsA2)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyUtilidades()
            End If

            mdcProxyActualizar.ScriptsA2s.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación

                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.filtrarScriptsSyncQuery(pstrFiltro:=pstrFiltro,
                                                                                                        pstrMostrarTodos:=pstrMostrarTodos,
                                                                                                         plngIDCia:=-1,
                                                                                                         pstrMaquina:=Program.Maquina,
                                                                                                         pstrUsuario:=pstrUsuario,
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
                                                                                                    pstrUsuario:=pstrUsuario,
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de scripts ", Me.ToString(), "consultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Private Async Function consultarUsuarioPorScripts(ByVal pintIdScript As Integer, ByVal pstrUsuario As String, ByVal pstrMaquina As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Dim objRet As LoadOperation(Of Usuarios)

        Try
            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyUtilidades()
            End If
            mdcProxyActualizar.Usuarios.Clear()

            objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.Consultar_EjecutarScripts_UsuariosAppSyncQuery(pintIdScript, pstrMaquina, pstrUsuario, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "consultarUsuarioPorScripts", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                Else
                    ListaUsuarios = mdcProxyActualizar.Usuarios
                End If

            Else
                ListaUsuarios.Clear()
            End If

            MyBase.CambioItem("ListaUsuarios")
            MyBase.CambioItem("ListaUsuariosPaged")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de usuarios", Me.ToString(), "consultarUsuarioPorScripts", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Public Async Function consultarScripts(pstrUsuarioAutorizado As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Dim objRet As LoadOperation(Of ScriptXUsuarios)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyUtilidades()
            End If
            mdcProxyActualizar.ScriptXUsuarios.Clear()


            objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.filtrarScriptXUsuariosSyncQuery(pstrUsuarioAutorizado, Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask()



            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "consultarScripts", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                Else

                    ListaScripts = mdcProxyActualizar.ScriptXUsuarios.Where(Function(i) Convert.ToBoolean(i.Asignado) = False).ToList
                    ListaScriptsAutorizados = mdcProxyActualizar.ScriptXUsuarios.Where(Function(i) Convert.ToBoolean(i.Asignado) = True).ToList


                    If objRet.Entities.Count = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                ListaScripts.Clear()
                ListaScriptsAutorizados.Clear()
            End If

            MyBase.CambioItem("ListaScripts")
            MyBase.CambioItem("ListaScriptsAutorizados")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de companias", Me.ToString(), "consultarCompañia", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region



End Class

Public Class CamposBusquedaSeguridadScripts
    Implements INotifyPropertyChanged

    <Display(Name:="IdScript")> _
    Public Property IdScript As Integer

    <Display(Name:="Grupo")> _
    Public Property Grupo As String

    <Display(Name:="Nombre")> _
    Public Property Nombre As String

    <Display(Name:="strUsuariocb")> _
    Private _strUsuariocb As String
    Public Property strUsuariocb() As String
        Get
            Return _strUsuariocb
        End Get
        Set(ByVal value As String)
            _strUsuariocb = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strUsuariocb"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

