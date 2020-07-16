Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: UsuariosSucursalesViewModel.vb
'Generado el : 03/16/2011 15:58:36
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web

Public Class UsuariosSucursalesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaUsuariosSucursale
    Private UsuariosSucursalePorDefecto As UsuariosSucursale
    Private UsuariosSucursaleAnterior As UsuariosSucursale
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim dcProxyUtilidades As UtilidadesDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim CambiarPrioridad As Boolean = True

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            dcProxyUtilidades = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                ConsultarReceptores("TODOSLOSRECEPTORES")
                dcProxy.Load(dcProxy.UsuariosSucursalesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosSucursales, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerUsuariosSucursalePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosSucursalesPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  UsuariosSucursalesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "UsuariosSucursalesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerUsuariosSucursalesPorDefecto_Completed(ByVal lo As LoadOperation(Of UsuariosSucursale))
        If Not lo.HasError Then
            UsuariosSucursalePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la UsuariosSucursale por defecto",
                                             Me.ToString(), "TerminoTraerUsuariosSucursalePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerUsuariosSucursales(ByVal lo As LoadOperation(Of UsuariosSucursale))
        If Not lo.HasError Then
            ListaUsuariosSucursales = dcProxy.UsuariosSucursales
            If dcProxy.UsuariosSucursales.Count > 0 Then
                If lo.UserState = "insert" Then
                    UsuariosSucursaleSelected = ListaUsuariosSucursales.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    'MessageBox.Show("No se encontró ningún registro")
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de UsuariosSucursales",
                                             Me.ToString(), "TerminoTraerUsuariosSucursale", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private _Enabled As Boolean
    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
            MyBase.CambioItem("Enabled")
        End Set
    End Property
#End Region

#Region "Propiedades"

    Private _ListaUsuariosSucursales As EntitySet(Of UsuariosSucursale)
    Public Property ListaUsuariosSucursales() As EntitySet(Of UsuariosSucursale)
        Get
            Return _ListaUsuariosSucursales
        End Get
        Set(ByVal value As EntitySet(Of UsuariosSucursale))
            _ListaUsuariosSucursales = value

            MyBase.CambioItem("ListaUsuariosSucursales")
            MyBase.CambioItem("ListaUsuariosSucursalesPaged")
            If Not IsNothing(value) Then
                If IsNothing(UsuariosSucursaleAnterior) Then
                    UsuariosSucursaleSelected = _ListaUsuariosSucursales.FirstOrDefault
                Else
                    UsuariosSucursaleSelected = UsuariosSucursaleAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaUsuariosSucursalesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaUsuariosSucursales) Then
                Dim view = New PagedCollectionView(_ListaUsuariosSucursales)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _UsuariosSucursaleSelected As UsuariosSucursale
    Public Property UsuariosSucursaleSelected() As UsuariosSucursale
        Get
            Return _UsuariosSucursaleSelected
        End Get
        Set(ByVal value As UsuariosSucursale)
            _UsuariosSucursaleSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("UsuariosSucursaleSelected")
        End Set
    End Property

    Private _ListaReceptoresCompleta As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaReceptoresCompleta() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaReceptoresCompleta
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaReceptoresCompleta = value
            MyBase.CambioItem("ListaReceptoresCompleta")
        End Set
    End Property

    Private _ListaReceptoresActivos As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaReceptoresActivos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaReceptoresActivos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaReceptoresActivos = value
            MyBase.CambioItem("ListaReceptoresActivos")
        End Set
    End Property

    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")

        End Set
    End Property
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            ConsultarReceptores("RECEPTORESACTIVOS_TODOS")
            Dim NewUsuariosSucursale As New UsuariosSucursale
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewUsuariosSucursale.IDComisionista = UsuariosSucursalePorDefecto.IDComisionista
            NewUsuariosSucursale.IDSucComisionista = UsuariosSucursalePorDefecto.IDSucComisionista
            NewUsuariosSucursale.Nombre_Usuario = UsuariosSucursalePorDefecto.Nombre_Usuario
            NewUsuariosSucursale.Receptor = UsuariosSucursalePorDefecto.Receptor
            NewUsuariosSucursale.IDSucursal = UsuariosSucursalePorDefecto.IDSucursal
            NewUsuariosSucursale.Actualizacion = UsuariosSucursalePorDefecto.Actualizacion
            NewUsuariosSucursale.Usuario = Program.Usuario
            NewUsuariosSucursale.IDUsuariosSucursales = UsuariosSucursalePorDefecto.IDUsuariosSucursales
            UsuariosSucursaleAnterior = UsuariosSucursaleSelected
            UsuariosSucursaleSelected = NewUsuariosSucursale
            MyBase.CambioItem("UsuariosSucursales")
            Editando = True
            Enabled = True

            MyBase.CambioItem("Editando")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.UsuariosSucursales.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.UsuariosSucursalesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosSucursales, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.UsuariosSucursalesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosSucursales, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaUsuariosSucursale()
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Nombre_Usuario <> String.Empty Or cb.Receptor <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.UsuariosSucursales.Clear()
                UsuariosSucursaleAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Usuario = " &  cb.Usuario.ToString() & " Receptor = " &  cb.Receptor.ToString() 
                dcProxy.Load(dcProxy.UsuariosSucursalesConsultarQuery(cb.Nombre_Usuario, cb.Receptor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosSucursales, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaUsuariosSucursale
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
                Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim logUsuarioInactivo As Boolean = False

            If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                Dim objDiccionarioCombos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)) = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
                If Not IsNothing(objDiccionarioCombos) Then
                    If objDiccionarioCombos.ContainsKey("LoginUsuario") Then
                        If objDiccionarioCombos("LoginUsuario").Where(Function(i) i.ID = _UsuariosSucursaleSelected.Nombre_Usuario).Count = 0 Then
                            logUsuarioInactivo = True
                        End If
                    End If
                End If
            End If

            If logUsuarioInactivo Then
                A2Utilidades.Mensajes.mostrarMensaje("El usuario se encuentra inactivo el registro no se puede modificar el registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If UsuariosSucursaleSelected.Receptor <> "T" Then
                    ConsultarReceptores("RECEPTORESACTIVOS", UsuariosSucursaleSelected.Receptor, "VALIDARRECEPTORACTIVOGUARDADO")
                Else
                    ContinuarGuardadoRegistro()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarGuardadoRegistro()
        Try
            For Each led In ListaUsuariosSucursales
                If Not IsNothing(UsuariosSucursaleSelected.Receptor) Then
                    If Not IsNothing(UsuariosSucursaleSelected.Nombre_Usuario) Then
                        If led.Receptor = UsuariosSucursaleSelected.Receptor And led.Nombre_Usuario = UsuariosSucursaleSelected.Nombre_Usuario And UsuariosSucursaleSelected.IDUsuariosSucursales <> led.IDUsuariosSucursales Then
                            A2Utilidades.Mensajes.mostrarMensaje("El Nombre de Usuario " + UsuariosSucursaleSelected.Nombre_Usuario.ToString + " y el Receptor " + UsuariosSucursaleSelected.Receptor + " Ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                End If
            Next

            Dim origen = "update"
            ErrorForma = ""
            UsuariosSucursaleAnterior = UsuariosSucursaleSelected
            If Not ListaUsuariosSucursales.Contains(UsuariosSucursaleSelected) Then
                origen = "insert"
                ListaUsuariosSucursales.Add(UsuariosSucursaleSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ContinuarGuardadoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                        Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        strMsg = Mensaje(0)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                So.MarkErrorAsHandled()
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                Exit Try
            End If

            MyBase.TerminoSubmitChanges(So)
            Enabled = False

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_UsuariosSucursaleSelected) Then
                Dim logUsuarioInactivo As Boolean = False

                If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                    Dim objDiccionarioCombos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)) = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
                    If Not IsNothing(objDiccionarioCombos) Then
                        If objDiccionarioCombos.ContainsKey("LoginUsuario") Then
                            If objDiccionarioCombos("LoginUsuario").Where(Function(i) i.ID = _UsuariosSucursaleSelected.Nombre_Usuario).Count = 0 Then
                                logUsuarioInactivo = True
                            End If
                        End If
                    End If
                End If

                If logUsuarioInactivo Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("El usuario se encuentra inactivo el registro no se puede modificar el registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    ConsultarReceptores("RECEPTORESACTIVOS", String.Empty, "VALIDARRECEPTORACTIVO")
                End If
            Else
                MyBase.RetornarValorEdicionNavegacion()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro",
                     Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ContinuarEdicionRegistro()
        If Not IsNothing(_UsuariosSucursaleSelected) Then
            Editando = True
            Enabled = False
        End If
    End Sub


    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""

            If Not IsNothing(_UsuariosSucursaleSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Enabled = False


                UsuariosSucursaleSelected = UsuariosSucursaleAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_UsuariosSucursaleSelected) Then
                If _ListaUsuariosSucursales.Where(Function(i) i.IDUsuariosSucursales = _UsuariosSucursaleSelected.IDUsuariosSucursales).Count > 0 Then
                    _ListaUsuariosSucursales.Remove(dcProxy.UsuariosSucursales.Where(Function(i) i.IDUsuariosSucursales = _UsuariosSucursaleSelected.IDUsuariosSucursales).First)

                    If _ListaUsuariosSucursales.Count > 0 Then
                        UsuariosSucursaleSelected = _ListaUsuariosSucursales.First
                    Else
                        UsuariosSucursaleSelected = Nothing
                    End If

                    IsBusy = True
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Private Sub ConsultarReceptores(ByVal pstrConsulta As String, Optional ByVal pstrFiltroTexto As String = "", Optional ByVal pstrUserState As String = "")
        Try
            If pstrConsulta <> "TODOSLOSRECEPTORES" Then
                IsBusy = True
            End If

            If String.IsNullOrEmpty(pstrUserState) Then
                pstrUserState = pstrConsulta
            End If
            dcProxyUtilidades.ItemCombos.Clear()
            dcProxyUtilidades.Load(dcProxyUtilidades.cargarCombosCondicionalQuery(pstrConsulta, pstrFiltroTexto, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptores, pstrUserState)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los receptores.",
                     Me.ToString(), "ConsultarReceptores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarReceptores(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If IsNothing(lo.Error) Then
                If lo.UserState = "TODOSLOSRECEPTORES" Then
                    ListaReceptoresCompleta = lo.Entities.ToList
                ElseIf lo.UserState = "VALIDARRECEPTORACTIVO" Then

                    Dim strReceptorSeleccionado As String = _UsuariosSucursaleSelected.Receptor
                    Dim strNombreReceptorSeleccionado As String = _UsuariosSucursaleSelected.Nombre_Receptor

                    ListaReceptoresActivos = lo.Entities.ToList
                    Dim logReceptorInactivo As Boolean = False

                    If ListaReceptoresActivos.Where(Function(i) i.ID = strReceptorSeleccionado).Count = 0 Then
                        logReceptorInactivo = True
                    End If

                    _UsuariosSucursaleSelected.Receptor = strReceptorSeleccionado
                    _UsuariosSucursaleSelected.Nombre_Receptor = strNombreReceptorSeleccionado

                    UsuariosSucursaleAnterior = _UsuariosSucursaleSelected

                    If logReceptorInactivo Then
                        _UsuariosSucursaleSelected.Receptor = String.Empty
                        _UsuariosSucursaleSelected.Nombre_Receptor = String.Empty
                        A2Utilidades.Mensajes.mostrarMensaje("El Receptor del registro seleccionado se encuentra inactivo, debe de seleccionar otro Receptor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If

                    ContinuarEdicionRegistro()
                    IsBusy = False
                ElseIf lo.UserState = "VALIDARRECEPTORACTIVOGUARDADO" Then
                    If lo.Entities.Count > 0 Then
                        ContinuarGuardadoRegistro()
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El receptor se encuentra inactivo, por lo tanto no se puede guardar el registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                Else
                    ListaReceptoresActivos = lo.Entities.ToList
                    IsBusy = False
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los receptores.",
                     Me.ToString(), "TerminoConsultarReceptores", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los receptores.",
                     Me.ToString(), "TerminoConsultarReceptores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("Nombre_Usuario", 1)
        DicCamposTab.Add("Receptor", 1)
    End Sub
#End Region

#Region "Eventos"

    Private Sub _UsuariosSucursaleSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _UsuariosSucursaleSelected.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName.Equals("Prioridad") Then
                    If CambiarPrioridad Then
                        CambiarPrioridad = False

                        'Reorganizar las prioridad del tipo de producto.
                        If ListaUsuariosSucursales.Where(Function(i) i.Prioridad = UsuariosSucursaleSelected.Prioridad And i.IDUsuariosSucursales <> UsuariosSucursaleSelected.IDUsuariosSucursales And i.Nombre_Usuario = UsuariosSucursaleSelected.Nombre_Usuario).Count > 0 Then
                            Dim UltimaPrioridad As Integer = 0

                            For Each li In ListaUsuariosSucursales.Where(Function(i) i.Prioridad >= UsuariosSucursaleSelected.Prioridad And i.IDUsuariosSucursales <> UsuariosSucursaleSelected.IDUsuariosSucursales And i.Nombre_Usuario = UsuariosSucursaleSelected.Nombre_Usuario).OrderBy(Function(i) i.Prioridad)
                                If li.Prioridad = UsuariosSucursaleSelected.Prioridad Then
                                    li.Prioridad = li.Prioridad + 1
                                    UltimaPrioridad = li.Prioridad
                                Else
                                    If li.Prioridad = UltimaPrioridad Then
                                        li.Prioridad = li.Prioridad + 1
                                        UltimaPrioridad = li.Prioridad
                                    End If
                                End If
                            Next
                        End If

                        CambiarPrioridad = True
                    End If
                ElseIf e.PropertyName.Equals("Receptor") Then
                    If Not IsNothing(_ListaReceptoresCompleta) And Not String.IsNullOrEmpty(_UsuariosSucursaleSelected.Receptor) Then
                        If _ListaReceptoresCompleta.Where(Function(i) i.ID = _UsuariosSucursaleSelected.Receptor).Count > 0 Then
                            _UsuariosSucursaleSelected.Nombre_Receptor = _ListaReceptoresCompleta.Where(Function(i) i.ID = _UsuariosSucursaleSelected.Receptor).First.Descripcion
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.",
             Me.ToString(), "_UsuariosSucursaleSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaUsuariosSucursale

    <StringLength(60, ErrorMessage:="La longitud máxima es de 60")>
    <Display(Name:="Usuario")>
    Public Property Nombre_Usuario As String

    <StringLength(4, ErrorMessage:="La longitud máxima es de 4")>
    <Display(Name:="Receptor")>
    Public Property Receptor As String
End Class




