Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Threading.Tasks
Imports System.Threading

''' <summary>
''' ViewModel para la pantalla Permisos a exportación de permisos.
''' </summary>
''' Creado por   : Jhonatan Arley Acevedo
''' Fecha        : Abril 09/2015
''' Pruebas CB   : Jhonatan Arley Acevedo - Abril 09/2015 - Resultado OK
''' <remarks></remarks>

Public Class PermisosExportacionFormatosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables"

    ''' <summary>
    ''' Propiedad para realizar el enlace con la capa de datos del DomainContext correspondiente
    ''' </summary>
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxyUtil As UtilidadesDomainContext
    Dim logConsultarRegistros As Boolean = True
    Public viewAsignarPemrisos As PermisosExportacionFormatosView
#End Region

#Region "Inicializar"

    ''' <summary>
    ''' Constructor del ViewModel para asociar el DomainContext
    ''' </summary>
    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxyUtil = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxyUtil = New UtilidadesDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxyUtil.Load(dcProxyUtil.cargarCombosEspecificosQuery("PermisosPantallaExportacionFormatos", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, "")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PermisosExportacionFormatosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para validar que no se intente conectar al web service si se está en modo diseño, necesario para que funcione el seleccionar todo
    ''' </summary>
    ''' <returns>Retorna una variable tipo Boolean que indica si hay errores en el proceso</returns>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False
        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then                
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)

        End Try

        Return (logResultado)

    End Function

#End Region
    
#Region "Métodos"

    ''' <summary>
    ''' Método para limpiar el datagrid y consultar los datos nuevamente una vez se hayan grabado los datos
    ''' </summary>
    ''' <param name="value">Parámetro tipo Boolean</param>
    Private Sub ActualizarGridPermisos(value As Boolean)
        Try
            If logConsultarRegistros Then
                If Not String.IsNullOrEmpty(SistemaOrigen) And Not String.IsNullOrEmpty(strTIPOBUSQUEDA) Then
                    If (strTIPOBUSQUEDA = "U" And Not String.IsNullOrEmpty(strLoginUsuario)) _
                        Or (strTIPOBUSQUEDA = "T" And Not String.IsNullOrEmpty(strBuscadorTipo)) Then
                        IsBusy = True
                        chkSeleccionarTodo = False

                        ListaRegistros = Nothing
                        Me.viewAsignarPemrisos.txtFiltro.Text = String.Empty
                        If Not IsNothing(dcProxy.PermisosFormatosExportars) Then
                            dcProxy.PermisosFormatosExportars.Clear()
                        End If
                        dcProxy.Load(dcProxy.PermisoExportacionFormatoConsultarQuery(strLoginUsuario, value, SistemaOrigen, strTIPOBUSQUEDA, strBuscadorTipo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFormatos, "Default")
                    Else
                        ListaRegistrosCompletos = Nothing
                        FiltrarInformacion(String.Empty)
                        Me.viewAsignarPemrisos.txtFiltro.Text = String.Empty
                    End If
                Else
                    ListaRegistrosCompletos = Nothing
                    FiltrarInformacion(String.Empty)
                    Me.viewAsignarPemrisos.txtFiltro.Text = String.Empty
                End If
                
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los permisos.", Me.ToString(), _
                                                         "ActualizarGridPermisos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para seleccionar o retirar la selección de todas las casillas tipo check del datagrid
    ''' </summary>
    ''' <param name="blnIsChequed">Parámetro tipo Boolean</param>
    Public Sub SeleccionarTodo(blnIsChequed As Boolean)
        Try
            If Not IsNothing(ListaRegistros) Then
                For Each it In ListaRegistros
                    it.Seleccionado = blnIsChequed
                Next
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar todos los registros.", Me.ToString(), _
                                                         "SeleccionarTodo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para grabar los formatos que hayan sido seleccionados en el datagrid
    ''' </summary>
    Public Sub GrabarPermisosAsignados()
        Try
            Dim xmlCompleto As String = String.Empty
            Dim xmlDetalle As String = String.Empty
            Dim ValidarAsignarPermisos As Boolean = False

            IsBusy = True
            If Not IsNothing(strLoginUsuario) And strLoginUsuario <> "" Then
                If Not IsNothing(SistemaOrigen) And SistemaOrigen <> "" Then

                    ActualizarSeleccionados()

                    If ListaRegistrosCompletos.Count > 0 Then

                        For Each chkSeleccionado In (From c In ListaRegistrosCompletos)
                            If chkSeleccionado.Seleccionado = True Then
                                ValidarAsignarPermisos = True
                            End If
                        Next

                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar un tipo de permiso para continuar.", Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar un usuario para continuar.", Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
            If ValidarAsignarPermisos Then

                xmlCompleto = "<ListaRegistros>"

                For Each objeto In (From c In ListaRegistrosCompletos)
                    If objeto.Seleccionado Then
                        xmlDetalle = "<Detalle intID=""" & objeto.Valor &
                                 """ strSistema=""" & objeto.Sistema &
                                 """ logSeleccionado=""" & IIf(CBool(objeto.Seleccionado), 1, 0) & """ ></Detalle>"
                        xmlCompleto = xmlCompleto & xmlDetalle
                    End If
                Next
                xmlCompleto = xmlCompleto & "</ListaRegistros>"

                chkSeleccionarTodo = False

                ListaRegistros = Nothing
                If Not IsNothing(dcProxy.PermisosFormatosExportars) Then
                    dcProxy.PermisosFormatosExportars.Clear()
                End If
                dcProxy.Load(dcProxy.PermisoExportacionFormatoActualizarQuery(pxmlPermisosAsignados:=xmlCompleto, plogAsignar:=logActivo, pstrUsuario:=strLoginUsuario, _
                                                                              pstrUsuarioActualizacion:=Program.Usuario, pstrMaquina:=Program.Maquina,
                                                                              pstrSistema:=SistemaOrigen, pstrTipoBusqueda:=strTIPOBUSQUEDA, pstrObjetoBusqueda:=strBuscadorTipo, pstrInfoConexion:=Program.HashConexion), AddressOf TerminoTraerFormatos, "Default")
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar los permisos.", Me.ToString(), _
                                                         "PermisoExportacionFormatoActualizar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub FiltrarInformacion(ByVal pstrFiltro As String)
        Try
            Dim objListaInformacion As New List(Of PermisosFormatosExportar)

            ActualizarSeleccionados()

            If Not IsNothing(ListaRegistrosCompletos) Then

                If String.IsNullOrEmpty(pstrFiltro) Then
                    For Each li In ListaRegistrosCompletos
                        objListaInformacion.Add(li)
                    Next
                Else
                    For Each li In ListaRegistrosCompletos
                        If li.Descripcion.ToUpper.Contains(pstrFiltro.ToUpper) Then
                            objListaInformacion.Add(li)
                        End If
                    Next
                End If
            End If

            ListaRegistros = Nothing
            ListaRegistros = objListaInformacion
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar la información.", Me.ToString(), _
                                                         "FiltrarInformacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ActualizarSeleccionados()
        Try
            If Not IsNothing(ListaRegistros) And Not IsNothing(ListaRegistrosCompletos) Then
                If ListaRegistrosCompletos.Count > 0 Then                    
                    For Each li In ListaRegistros
                        If ListaRegistrosCompletos.Where(Function(i) i.Valor = li.Valor).Count > 0 Then
                            Dim objActualizarRegistro = ListaRegistrosCompletos.Where(Function(i) i.Valor = li.Valor).First
                            objActualizarRegistro.Seleccionado = li.Seleccionado
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar los registros seleccionados.", Me.ToString(), "ActualizarSeleccionados", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos"

    ''' <summary>
    ''' Método asincrónico que llena la entidad ListaRegistros
    ''' </summary>
    Private Sub TerminoTraerFormatos(ByVal lo As LoadOperation(Of PermisosFormatosExportar))
        If Not lo.HasError Then
            ListaRegistrosCompletos = dcProxy.PermisosFormatosExportars.ToList
            FiltrarInformacion(String.Empty)
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de formatos", _
                                             Me.ToString(), "TerminoTraerFormatos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoCargarCombosEspecificos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        'se definen de tipo observable los diccionarios y los recursos List
        Dim objListaCombos As ObservableCollection(Of OYDUtilidades.ItemCombo) = Nothing
        Dim objListaNodosCategoria As ObservableCollection(Of OYDUtilidades.ItemCombo) = Nothing
        Dim dicListaCombos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)) = Nothing
        Dim strNombreCategoria As String = String.Empty

        Try
            If Not lo.HasError Then
                'Obtiene los valores del UserState
                'Convierte los datos recibidos en un diccionario donde el nombre de la categoría es la clave
                objListaCombos = New ObservableCollection(Of OYDUtilidades.ItemCombo)(lo.Entities)
                If objListaCombos.Count > 0 Then
                    Dim listaCategorias = From lc In objListaCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada

                    ' Guardar los datos recibidos en un diccionario
                    dicListaCombos = New Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))

                    For Each NombreCategoria As String In listaCategorias
                        strNombreCategoria = NombreCategoria
                        objListaNodosCategoria = New ObservableCollection(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria))

                        'dicListaCombos.Add(NombreCategoria, objListaNodosCategoria)
                        dicListaCombos.Add(NombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCombosPermisos = dicListaCombos
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusy = False

    End Sub

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Esta propiedad con valor true por defecto hace que la pantalla de entrada sea la de asignar permisos, así mismo envia al procedimiento que consulta el método grabar, 
    ''' un bit, para saber si se van a asignar o quitar permisos
    ''' </summary>
    Private _logActivo As System.Nullable(Of Boolean) = True
    Public Property logActivo() As System.Nullable(Of Boolean)
        Get
            Return _logActivo
        End Get
        Set(ByVal value As System.Nullable(Of Boolean))
            _logActivo = value
            MyBase.CambioItem("logActivo")

            ActualizarGridPermisos(_logActivo)

        End Set
    End Property

    Private _strActivo As String = "1"
    Public Property strActivo() As String
        Get
            Return _strActivo
        End Get
        Set(ByVal value As String)
            _strActivo = value
            MyBase.CambioItem("strActivo")
            If _strActivo = "1" Then
                logActivo = True
            Else
                logActivo = False
            End If
        End Set
    End Property

    ''' <summary>
    ''' Esta propiedad sera la que contienga la cantidad de formatos que arroje la consulta inicial
    ''' </summary>
    Private _ListaRegistrosCompletos As List(Of PermisosFormatosExportar)
    Public Property ListaRegistrosCompletos() As List(Of PermisosFormatosExportar)
        Get
            Return _ListaRegistrosCompletos
        End Get
        Set(ByVal value As List(Of PermisosFormatosExportar))
            _ListaRegistrosCompletos = value
        End Set
    End Property

    ''' <summary>
    ''' Esta propiedad sera la que contienga la cantidad de formatos que arroje la consulta inicial
    ''' </summary>
    Private _ListaRegistros As List(Of PermisosFormatosExportar)
    Public Property ListaRegistros() As List(Of PermisosFormatosExportar)
        Get
            Return _ListaRegistros
        End Get
        Set(ByVal value As List(Of PermisosFormatosExportar))
            _ListaRegistros = value
            MyBase.CambioItem("ListaRegistros")
            MyBase.CambioItem("ListaRegistrosPaged")
        End Set
    End Property

    ''' <summary>
    ''' Esta propiedad es la que permite paginar el grid
    ''' </summary>
    Public ReadOnly Property ListaRegistrosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaRegistros) Then
                Dim view = New PagedCollectionView(_ListaRegistros)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Esta propiedad permite saber que usuario se ha seleccionado en el comboBox
    ''' </summary>
    Private _strLoginUsuario As String = String.Empty
    Public Property strLoginUsuario() As String
        Get
            Return _strLoginUsuario
        End Get
        Set(ByVal value As String)
            _strLoginUsuario = value
            ActualizarGridPermisos(_logActivo)
            MyBase.CambioItem("strLoginUsuario")
        End Set
    End Property

    ''' <summary>
    ''' Esta propiedad permite saber que tipo se ha seleccionado en el buscador.
    ''' </summary>    
    Private _strBuscadorTipo As String = String.Empty
    Public Property strBuscadorTipo() As String
        Get
            Return _strBuscadorTipo
        End Get
        Set(ByVal value As String)
            _strBuscadorTipo = value
            ActualizarGridPermisos(_logActivo)
            MyBase.CambioItem("strBuscadorTipo")
        End Set
    End Property

    ''' <summary>
    ''' Esta Propiedad es para cambiar el título en el grid, dependiendo del buscador seleccionado.
    ''' </summary>    
    Private _strTituloGrid As String = String.Empty
    Public Property strTituloGrid() As String
        Get
            Return _strTituloGrid
        End Get
        Set(ByVal value As String)
            _strTituloGrid = value
            MyBase.CambioItem("strTituloGrid")
        End Set
    End Property


    ''' <summary>
    ''' Esta propiedad controla el estado del checkBox seleccionar todo en el datagrid
    ''' </summary>
    Private _chkSeleccionarTodo As System.Nullable(Of Boolean) = False
    Public Property chkSeleccionarTodo() As System.Nullable(Of Boolean)
        Get
            Return _chkSeleccionarTodo
        End Get
        Set(ByVal value As System.Nullable(Of Boolean))
            _chkSeleccionarTodo = value
            MyBase.CambioItem("chkSeleccionarTodo")
        End Set
    End Property

    Private _SistemaOrigen As String
    Public Property SistemaOrigen() As String
        Get
            Return _SistemaOrigen
        End Get
        Set(ByVal value As String)
            _SistemaOrigen = value
            If _strTIPOBUSQUEDA = "T" Then
                strBuscadorTipo = String.Empty
            End If
            ActualizarGridPermisos(_logActivo)
            MyBase.CambioItem("SistemaOrigen")
        End Set
    End Property

    Private _strTIPOBUSQUEDA As String = "U"
    Public Property strTIPOBUSQUEDA() As String
        Get
            Return _strTIPOBUSQUEDA
        End Get
        Set(ByVal value As String)
            _strTIPOBUSQUEDA = value
            If _strTIPOBUSQUEDA = "U" Then
                MostrarBuscadorTipo = Visibility.Collapsed
                MostrarBuscadorUsuario = Visibility.Visible
                strTituloGrid = "Usuario"
            Else
                MostrarBuscadorTipo = Visibility.Visible
                MostrarBuscadorUsuario = Visibility.Collapsed
                strTituloGrid = "Tipo"
            End If
            ActualizarGridPermisos(_logActivo)
            MyBase.CambioItem("SistemaOrigen")
        End Set
    End Property

    Private _MostrarBuscadorTipo As Visibility = Visibility.Collapsed
    Public Property MostrarBuscadorTipo() As Visibility
        Get
            Return _MostrarBuscadorTipo
        End Get
        Set(ByVal value As Visibility)
            _MostrarBuscadorTipo = value
            MyBase.CambioItem("MostrarBuscadorTipo")
        End Set
    End Property

    Private _MostrarBuscadorUsuario As Visibility = Visibility.Visible
    Public Property MostrarBuscadorUsuario() As Visibility
        Get
            Return _MostrarBuscadorUsuario
        End Get
        Set(ByVal value As Visibility)
            _MostrarBuscadorUsuario = value
            MyBase.CambioItem("MostrarBuscadorUsuario")
        End Set
    End Property

    Private _DiccionarioCombosPermisos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombosPermisos() As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombosPermisos
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombosPermisos = value
            MyBase.CambioItem("DiccionarioCombosPermisos")
        End Set
    End Property



#End Region
End Class