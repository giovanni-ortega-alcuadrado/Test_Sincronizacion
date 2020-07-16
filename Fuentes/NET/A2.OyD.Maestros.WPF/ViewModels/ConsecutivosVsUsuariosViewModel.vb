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
''' ViewModel para la pantalla Consecutivs Vs Usuario.
''' </summary>
''' Creado por   : Juan Camilo Múnera
''' Fecha        : Mayo 20/2016
''' Pruebas CB   : Juan Camilo Múnera - Mayo 20/2016 - Resultado OK
''' <remarks></remarks>

Public Class ConsecutivosVsUsuariosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables"

    ''' <summary>
    ''' Propiedad para realizar el enlace con la capa de datos del DomainContext correspondiente
    ''' </summary>
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxyUtil As UtilidadesDomainContext
    Dim logConsultarRegistros As Boolean = True
    Private mobjVM As ConsecutivosVsUsuarios
    Public strUsuarioProceso As String

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
                MostrarBuscadorConsecutivos = Visibility.Collapsed
                MostrarBuscadorUsuarios = Visibility.Collapsed
                MostrarCompañia = Visibility.Collapsed
                logHabilitarGrid = False
                dcProxyUtil.Load(dcProxyUtil.cargarCombosEspecificosQuery("PermisosConsecVsUsu", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, "")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConsecutivosVsUsuariosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
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
    ''' Método para seleccionar o retirar la selección de todas las casillas tipo check del datagrid
    ''' </summary>
    ''' <param name="blnIsChequed">Parámetro tipo Boolean</param>
    Public Sub SeleccionarTodo(blnIsChequed As Boolean)
        Try
            If Not IsNothing(ListaConsecutivosVsUsuarios) Then
                For Each it In ListaConsecutivosVsUsuarios
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
    ''' Método para validar que se hayan ingresado los datos necesarios para realizar el filtro 
    ''' </summary>
    ''' <returns>Retorna una variable tipo Boolean que indica si hay validaciones encontradas en el proceso</returns>
    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- Validar Datos
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_ListaConsecutivosVsUsuarios) Then
                'Modulos
                If IsNothing(strModulos) Then
                    strMsg = String.Format("{0}{1} + Debe seleccionar un modulo para continuar.", strMsg, vbCrLf)
                End If

                'Usuarios
                If strModulos = "U" And strNombreUsuarioProceso = "" Then
                    strMsg = String.Format("{0}{1} + Debe seleccionar un usuario para continuar.", strMsg, vbCrLf)
                End If

                'Consecutivos
                If strModulos = "C" And strNombreConsecutivo = "" Then
                    strMsg = String.Format("{0}{1} + Debe seleccionar un consecutivo para continuar.", strMsg, vbCrLf)
                End If
            Else
                strMsg = String.Format("{0}{1} No existe ningún registro para guardar", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                logResultado = True
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarDatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Método que se encarga de realizar la actualizacion de los registros
    ''' </summary>


    Public Overrides Async Sub ActualizarRegistro()
        IsBusy = True
        Try
            Dim xmlCompleto As String = String.Empty
            Dim xmlDetalle As String = String.Empty


            If ValidarDatos() Then


                If ListaConsecutivosVsUsuarios.Count > 0 Then

                    xmlCompleto = "<ConsecutivosVsUsuarios>"

                    If strModulos = "U" Then
                        For Each objeto In (From c In ListaConsecutivosVsUsuarios)
                            xmlDetalle = "<Detalle strNombreConsecutivo=""" & objeto.NombreConsecutivo &
                                         """ strUsuarioConsecutivo=""" & strUsuarioProceso &
                                         """ logSeleccionado=""" & IIf(CBool(objeto.Seleccionado), 1, 0) &
                                         """ logEvaluar=""" & 1 & """ ></Detalle>"
                            xmlCompleto = xmlCompleto & xmlDetalle
                        Next
                        'xmlCompleto = xmlCompleto & "</ConsecutivosVsUsuarios>"

                    Else

                        For Each objeto In (From c In ListaConsecutivosVsUsuarios)
                            xmlDetalle = "<Detalle strNombreConsecutivo=""" & strNombreConsecutivo &
                                         """ strUsuarioConsecutivo=""" & objeto.NombreConsecutivo &
                                         """ logSeleccionado=""" & IIf(CBool(objeto.Seleccionado), 1, 0) &
                                         """ logEvaluar=""" & 1 & """ ></Detalle>"
                            xmlCompleto = xmlCompleto & xmlDetalle
                        Next
                    End If

                    xmlCompleto = xmlCompleto & "</ConsecutivosVsUsuarios>"


                    Dim objRet As InvokeOperation(Of String)
                    Dim strMsg As String = String.Empty
                    objRet = Await dcProxy.ConsecutivosVsUsuariosActualizar(strModulos, xmlCompleto, Program.Usuario, Program.HashConexion).AsTask()

                    strMsg = objRet.Value.ToString()


                    If Not String.IsNullOrEmpty(strMsg) Then
                        If strMsg.StartsWith("OK|") Then
                            A2Utilidades.Mensajes.mostrarMensaje(strMsg.Replace("OK|", ""), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                            'strNombreConsecutivo = String.Empty
                            'strNombreUsuarioProceso = String.Empty
                            Editando = False
                            logHabilitarGrid = False
                            Await FiltrarDatos()


                        Else
                            strMsg = String.Format("{0}{1}", vbCrLf, strMsg)
                            A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If



                End If
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar los permisos.", Me.ToString(), _
                                                         "GrabarPermisosAsignados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se encarga de manejar la edición de los registros
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        Try
            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(ListaConsecutivosVsUsuarios) Then
                logHabilitarGrid = True

                Editando = True
                MyBase.CambioItem("Editando")
            Else
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("No existen datos para editar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar los datos", Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
       
    End Sub

    ''' <summary>
    ''' Método que se encarga de manejar la cancelación de la edición de los registros
    ''' </summary>
    Public Overrides Async Sub CancelarEditarRegistro()
        Try
            Editando = False
            MyBase.CambioItem("Editando")

            Await FiltrarDatos()
            logHabilitarGrid = False


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición de los datos", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    
#End Region

#Region "Resultados Asincrónicos"

    

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

    ''' <summary>
    ''' Método para limpiar los datos del grid
    ''' </summary>
    Private Sub BorrarDatosGrid()
        Try
            Editando = False
            logHabilitarGrid = False
            If Not IsNothing(ListaConsecutivosVsUsuarios) Then
                ListaConsecutivosVsUsuarios = Nothing
                ListaConsecutivosVsUsuariosPaginada = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los datos del grid", _
                     Me.ToString(), "BorrarDatosGrid", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Método para realizar el filtrado de la información, dependiendo si es U:Usuarios o C:Consecutivos
    ''' </summary>
    Public Async Function FiltrarDatos() As Task
        If _strModulos = "U" Then
            If Not String.IsNullOrEmpty(_strCondicionTipo) Then
                strUsuarioProceso = strCondicionTipo
                strNombreConsecutivo = String.Empty
                Await ConsultarDetalle(strModulos, strUsuarioProceso, strNombreConsecutivo, intCompanias)
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un usuario para realizar la consulta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '_ListaConsecutivosVsUsuarios.Clear()
                ' EliminarTodo()
            End If
        Else
            If Not String.IsNullOrEmpty(_strCondicionTipo) Then
                strUsuarioProceso = String.Empty
                strNombreConsecutivo = strCondicionTipo
                Await ConsultarDetalle(strModulos, strUsuarioProceso, strNombreConsecutivo, intCompanias)
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un consecutivo para realizar la consulta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'ListaConsecutivosVsUsuarios.Clear()
                'EliminarTodo()
            End If
        End If
    End Function
    ''' <summary>
    ''' Método que se encarga de consultar el detalle
    ''' </summary>
    Public Async Function ConsultarDetalle(ByVal pstrModulo As String, pstrUsuarioProceso As String, pstrNombreConsecutivo As String, pIntIDCompanias As Integer) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of ConsecutivosVsUsuarios)

        Try
            If Not dcProxy.ConsecutivosVsUsuarios Is Nothing Then
                dcProxy.ConsecutivosVsUsuarios.Clear()
            End If


            objRet = Await dcProxy.Load(dcProxy.ConsecutivosVsUsuariosFiltrarQuery(pstrModulo, pIntIDCompanias, pstrUsuarioProceso, pstrNombreConsecutivo, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de los consecutivos Usuarios. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de los consecutivos vs Usuarios.", Me.ToString(), "ConsultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaConsecutivosVsUsuarios = objRet.Entities.ToList
                End If
            End If
            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de los consecutivos vs Usuarios.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)


    End Function




#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Esta propiedad obtiene el valor de la descripción de los items
    ''' </summary>
    Private _strDescripcionItem As String = String.Empty
    Public Property strDescripcionItem() As String
        Get
            Return _strDescripcionItem
        End Get
        Set(ByVal value As String)
            _strDescripcionItem = value
            'ActualizarGridPermisos()
            MyBase.CambioItem("strDescripcionItem")
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
    ''' <summary>
    ''' Esta propiedad controla el diccionario de datos
    ''' </summary>
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


    ''' <summary>
    ''' Esta propiedad almacena la infromación del modulo a consultar
    ''' </summary>
    Private _strModulos As String = String.Empty
    Public Property strModulos() As String
        Get
            Return _strModulos
        End Get
        Set(ByVal value As String)
            _strModulos = value

            If _strModulos = "U" Then
                MostrarBuscadorUsuarios = Visibility.Visible
                MostrarBuscadorConsecutivos = Visibility.Collapsed
                MostrarCompañia = Visibility.Visible
                strlabelProceso = "Usuario"
            Else
                If _strModulos = "C" Then
                    MostrarBuscadorConsecutivos = Visibility.Visible
                    MostrarBuscadorUsuarios = Visibility.Collapsed
                    MostrarCompañia = Visibility.Visible
                    strlabelProceso = "Consecutivo"
                Else
                    MostrarBuscadorConsecutivos = Visibility.Collapsed
                    MostrarBuscadorUsuarios = Visibility.Collapsed
                    MostrarCompañia = Visibility.Collapsed
                    strlabelProceso = String.Empty
                End If
            End If

            strNombreCompañia = String.Empty
            strCondicionTipo = String.Empty
            strNombreUsuarioProceso = String.Empty
            intCompanias = 0

            BorrarDatosGrid()



            'ActualizarGridPermisos()
            MyBase.CambioItem("strModulos")
        End Set
    End Property

    ''' <summary>
    ''' Esta propiedad maneja la edición o no de los registros en el grid
    ''' </summary>
    Private _logHabilitarGrid As Boolean = False
    Public Property logHabilitarGrid() As Boolean
        Get
            Return _logHabilitarGrid
        End Get
        Set(value As Boolean)
            _logHabilitarGrid = value
            MyBase.CambioItem("logHabilitarGrid")
        End Set
    End Property




    ''' <summary>
    ''' Esta propiedad almacena la informacion del usuario en proceso
    ''' </summary>
    Private _strNombreUsuarioProceso As String = String.Empty
    Public Property strNombreUsuarioProceso() As String
        Get
            Return _strNombreUsuarioProceso
        End Get
        Set(ByVal value As String)
            _strNombreUsuarioProceso = value

            If Not IsNothing(ListaConsecutivosVsUsuarios) Then
                ListaConsecutivosVsUsuarios = Nothing
                ListaConsecutivosVsUsuariosPaginada = Nothing
            End If

            BorrarDatosGrid()
            'ActualizarGridPermisos()
            'FiltrarDatos()
            MyBase.CambioItem("strNombreUsuarioProceso")
        End Set
    End Property

    ''' <summary>
    ''' Esta propiedad almacena la infromación del nombre de la compañía
    ''' </summary>
    Private _strNombreCompañia As String = String.Empty
    Public Property strNombreCompañia() As String
        Get
            Return _strNombreCompañia
        End Get
        Set(ByVal value As String)
            _strNombreCompañia = value

            If Not IsNothing(ListaConsecutivosVsUsuarios) Then
                ListaConsecutivosVsUsuarios = Nothing
                ListaConsecutivosVsUsuariosPaginada = Nothing
            End If

            BorrarDatosGrid()
            'ActualizarGridPermisos()
            MyBase.CambioItem("strNombreCompañia")
        End Set
    End Property


    ''' <summary>
    ''' Esta propiedad almacena la información por la cual se va a realizar el filtro de la información
    ''' </summary>
    Private _strCondicionTipo As String = String.Empty
    Public Property strCondicionTipo() As String
        Get
            Return _strCondicionTipo
        End Get
        Set(ByVal value As String)
            _strCondicionTipo = value
            'ActualizarGridPermisos()

            If Not IsNothing(ListaConsecutivosVsUsuarios) Then
                ListaConsecutivosVsUsuarios = Nothing
                ListaConsecutivosVsUsuariosPaginada = Nothing
            End If

            BorrarDatosGrid()
            MyBase.CambioItem("strCondicionTipo")
        End Set
    End Property


    ''' <summary>
    ''' Esta propiedad Define si mostramos o no el buscador de los usuarios
    ''' </summary>
    Private _MostrarBuscadorUsuarios As Visibility = Visibility.Collapsed
    Public Property MostrarBuscadorUsuarios() As Visibility
        Get
            Return _MostrarBuscadorUsuarios
        End Get
        Set(value As Visibility)
            _MostrarBuscadorUsuarios = value
            MyBase.CambioItem("MostrarBuscadorUsuarios")
        End Set
    End Property
    ''' <summary>
    ''' Esta propiedad Mostramos o no los buscadores de acuerdo a los consecutivos
    ''' </summary>
    Private _MostrarBuscadorConsecutivos As Visibility = Visibility.Collapsed
    Public Property MostrarBuscadorConsecutivos() As Visibility
        Get
            Return _MostrarBuscadorConsecutivos
        End Get
        Set(value As Visibility)
            _MostrarBuscadorConsecutivos = value
            MyBase.CambioItem("MostrarBuscadorConsecutivos")
        End Set
    End Property

    ''' <summary>
    ''' Esta propiedad Mostramos o no los campos comunes
    ''' </summary>
    Private _MostrarCompañia As Visibility = Visibility.Collapsed
    Public Property MostrarCompañia() As Visibility
        Get
            Return _MostrarCompañia
        End Get
        Set(value As Visibility)
            _MostrarCompañia = value
            MyBase.CambioItem("MostrarCompañia")
        End Set
    End Property

    ''' <summary>
    ''' Esta propiedad Contiene el nombre del consecutivo''' 
    ''' </summary>
    Private _strNombreConsecutivo As String = String.Empty
    Public Property strNombreConsecutivo() As String
        Get
            Return _strNombreConsecutivo
        End Get
        Set(ByVal value As String)
            _strNombreConsecutivo = value
            'FiltrarDatos()

            BorrarDatosGrid()
            MyBase.CambioItem("strNombreConsecutivo")
        End Set
    End Property
    ''' <summary>
    ''' Esta propiedad Contiene el id de la compañía, esto para la busqueda de los consecutivos de acuerdo a la compañía''' 
    ''' </summary>
    Private _intCompanias As Integer = 0
    Public Property intCompanias() As Integer
        Get
            Return _intCompanias
        End Get
        Set(ByVal value As Integer)
            _intCompanias = value
            'ActualizarGridPermisos()
            strCondicionTipo = String.Empty

            BorrarDatosGrid()
            MyBase.CambioItem("intCompanias")
        End Set
    End Property
    ''' <summary>
    ''' Esta propiedad Contiene la lista de los datos consecutivos y usuarios''' 
    ''' </summary>
    Private _ListaConsecutivosVsUsuarios As List(Of ConsecutivosVsUsuarios)
    Public Property ListaConsecutivosVsUsuarios As List(Of ConsecutivosVsUsuarios)
        Get
            Return _ListaConsecutivosVsUsuarios
        End Get
        Set(value As List(Of ConsecutivosVsUsuarios))
            _ListaConsecutivosVsUsuarios = value
            MyBase.CambioItem("ListaConsecutivosVsUsuarios")
            MyBase.CambioItem("ListaConsecutivosVsUsuariosPaginada")
        End Set
    End Property
    ''' <summary>
    ''' Esta propiedad contiene los items detallados del grid''' 
    ''' </summary>
    Public Property ListaConsecutivosVsUsuariosPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConsecutivosVsUsuarios) Then
                Dim view = New PagedCollectionView(_ListaConsecutivosVsUsuarios)
                Return view
            Else
                Return Nothing
            End If
        End Get
        Set(value As PagedCollectionView)
            MyBase.CambioItem("ListaConsecutivosVsUsuariosPaginada")
        End Set
    End Property


    ''' <summary>
    ''' Esta propiedad es del dato seleccionado en el grid
    ''' </summary>
    Private WithEvents _ListaConsecutivosVsUsuariosSeleccionado As ConsecutivosVsUsuarios
    Public Property ListaConsecutivosVsUsuariosSeleccionado() As ConsecutivosVsUsuarios
        Get
            Return _ListaConsecutivosVsUsuariosSeleccionado
        End Get
        Set(ByVal value As ConsecutivosVsUsuarios)
            _ListaConsecutivosVsUsuariosSeleccionado = value
            MyBase.CambioItem("ListaConsecutivosVsUsuariosSeleccionado")
        End Set
    End Property


    ''' <summary>
    ''' Esta propiedad almacena la infromación que se mostrará de acuerdo al tipo seleccionado (Usuario o consecutivo)
    ''' </summary>
    Private _strlabelProceso As String = String.Empty
    Public Property strlabelProceso() As String
        Get
            Return _strlabelProceso
        End Get
        Set(ByVal value As String)
            _strlabelProceso = value
            'ActualizarGridPermisos()
            'FiltrarDatos()
            MyBase.CambioItem("strlabelProceso")
        End Set
    End Property




#End Region


End Class
