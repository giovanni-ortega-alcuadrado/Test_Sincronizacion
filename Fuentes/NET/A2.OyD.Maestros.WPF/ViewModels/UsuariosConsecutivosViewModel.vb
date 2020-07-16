Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: UsuariosConsecutivosViewModel.vb
'Generado el : 04/14/2011 07:31:12
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

Public Class UsuariosConsecutivosViewModel
    Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaUsuariosConsecutivo
    Private UsuariosConsecutivoPorDefecto As ConsecutivosUsuario
    Private UsuariosConsecutivoAnterior As ConsecutivosUsuario
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim changes As Boolean
    Dim count As Integer
    Dim Nom_consecutivo As String
    Dim consecutivo As String


    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ConsecutivosUsuariosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosConsecutivos, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerConsecutivosUsuarioPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosConsecutivosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  UsuariosConsecutivosViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "UsuariosConsecutivosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"
    Private Sub TerminoTraertabla(ByVal lo As LoadOperation(Of ListaUsuario))
        If Not lo.HasError Then
            Tabladisponibles.Clear()
            For Each ll In dcProxy.ListaUsuarios
                Tabladisponibles.Add(New ItemUsuariosConsecutivos With {.Consecutivo = ll.Consecutivo, .Chequear = ll.Chequear, .CheckedOriginal = ll.Chequear})
            Next
            For Each co In dcProxy.ConsecutivosUsuarios
                If co.Nombre_Consecutivo = UsuariosConsecutivoSelected.Nombre_Consecutivo Then
                    Tabladisponibles.Add(New ItemUsuariosConsecutivos With {.Consecutivo = co.Usuario_Consecutivo, .Chequear = True, .CheckedOriginal = True})
                End If
            Next
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConceptosConsecutivos", _
                                       Me.ToString(), "TerminoTraerConceptosConsecutivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If

    End Sub

    Private Sub TerminoTraerUsuariosConsecutivosPorDefecto_Completed(ByVal lo As LoadOperation(Of ConsecutivosUsuario))
        If Not lo.HasError Then
            UsuariosConsecutivoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConsecutivosUsuario por defecto", _
                                             Me.ToString(), "TerminoTraerConsecutivosUsuarioPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerUsuariosConsecutivos(ByVal lo As LoadOperation(Of ConsecutivosUsuario))
        If Not lo.HasError Then
            ListaUsuariosConsecutivos = dcProxy.ConsecutivosUsuarios
            If dcProxy.ConsecutivosUsuarios.Count > 0 Then
                If lo.UserState = "insert" Then
                    UsuariosConsecutivoselected = ListaUsuariosConsecutivos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    If ListaUsuariosConsecutivos.Count = 0 Then
                        UsuariosConsecutivoSelected = New ConsecutivosUsuario
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de UsuariosConsecutivos", _
                                             Me.ToString(), "TerminoTraerConsecutivosUsuario", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaUsuariosConsecutivos As EntitySet(Of ConsecutivosUsuario)
    Public Property ListaUsuariosConsecutivos() As EntitySet(Of ConsecutivosUsuario)
        Get
            Return _ListaUsuariosConsecutivos
        End Get
        Set(ByVal value As EntitySet(Of ConsecutivosUsuario))
            _ListaUsuariosConsecutivos = value

            MyBase.CambioItem("ListaUsuariosConsecutivos")
            MyBase.CambioItem("ListaUsuariosConsecutivosPaged")
            If Not IsNothing(value) Then
                If IsNothing(UsuariosConsecutivoAnterior) Then
                    UsuariosConsecutivoSelected = _ListaUsuariosConsecutivos.FirstOrDefault
                Else
                    UsuariosConsecutivoSelected = UsuariosConsecutivoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaUsuariosConsecutivosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaUsuariosConsecutivos) Then
                Dim view = New PagedCollectionView(_ListaUsuariosConsecutivos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _UsuariosConsecutivoSelected As ConsecutivosUsuario
    Public Property UsuariosConsecutivoSelected() As ConsecutivosUsuario
        Get
            Return _UsuariosConsecutivoSelected
        End Get
        Set(ByVal value As ConsecutivosUsuario)
            _UsuariosConsecutivoSelected = value
            If Not value Is Nothing Then
                If ListaUsuariosConsecutivos.Count = 0 Then
                    UsuariosConsecutivoSelected.Nombre_Consecutivo = consecutivo
                End If
                dcProxy.ListaUsuarios.Clear()
                Tabladisponibles.Clear()
                dcProxy.Load(dcProxy.llenarusuariosinpermisoQuery(UsuariosConsecutivoSelected.Nombre_Consecutivo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraertabla, Nothing)
            End If
            MyBase.CambioItem("UsuariosConsecutivoSelected")
        End Set
    End Property
    Private _EditaReg As Boolean
    Public Property EditaReg() As Boolean
        Get
            Return _EditaReg
        End Get
        Set(ByVal value As Boolean)

            _EditaReg = value
            MyBase.CambioItem("EditaReg")
        End Set
    End Property
    Private _Tabladisponibles As ObservableCollection(Of ItemUsuariosConsecutivos) = New ObservableCollection(Of ItemUsuariosConsecutivos)
    Public Property Tabladisponibles() As ObservableCollection(Of ItemUsuariosConsecutivos)
        Get
            Return _Tabladisponibles
        End Get
        Set(ByVal value As ObservableCollection(Of ItemUsuariosConsecutivos))
            _Tabladisponibles = value
            tablaSeleccionada = value.FirstOrDefault

            MyBase.CambioItem("Tabladisponibles")
            MyBase.CambioItem("TabladisponiblesPaged")
        End Set

    End Property
    Public ReadOnly Property TabladisponiblesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_Tabladisponibles) Then
                Dim view = New PagedCollectionView(_Tabladisponibles)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _tablaSeleccionada As ItemUsuariosConsecutivos
    Public Property tablaSeleccionada() As ItemUsuariosConsecutivos
        Get
            Return _tablaSeleccionada
        End Get
        Set(ByVal value As ItemUsuariosConsecutivos)
            _tablaSeleccionada = value
            If Not value Is Nothing Then
                MyBase.CambioItem("tablaSeleccionada")
            End If
        End Set
    End Property
    Public Sub limpiar()
        MyBase.CambioItem("Tabladisponibles")
        MyBase.CambioItem("tablaSeleccionada")
    End Sub

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '    Try
        '        Dim NewUsuariosConsecutivo As New ConsecutivosUsuario
        '        'TODO: Verificar cuales son los campos que deben inicializarse
        '        NewUsuariosConsecutivo.IDComisionista = UsuariosConsecutivoPorDefecto.IDComisionista
        '        NewUsuariosConsecutivo.IDSucComisionista = UsuariosConsecutivoPorDefecto.IDSucComisionista
        '        NewUsuariosConsecutivo.Usuario_Consecutivo = UsuariosConsecutivoSelected.Usuario_Consecutivo
        '        NewUsuariosConsecutivo.Nombre_Consecutivo = UsuariosConsecutivoSelected.Nombre_Consecutivo
        '        NewUsuariosConsecutivo.Actualizacion = UsuariosConsecutivoPorDefecto.Actualizacion
        '        NewUsuariosConsecutivo.Usuario = Program.Usuario
        '        NewUsuariosConsecutivo.IDConsecutivosUsuarios = UsuariosConsecutivoPorDefecto.IDConsecutivosUsuarios
        '        UsuariosConsecutivoAnterior = UsuariosConsecutivoSelected
        '        UsuariosConsecutivoSelected = NewUsuariosConsecutivo
        '        MyBase.CambioItem("ConsecutivosUsuarios")
        '        Editando = True
        '        EditaReg = True
        '        MyBase.CambioItem("Editando")
        '    Catch ex As Exception
        '        IsBusy = False
        '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
        '                                                     Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '    End Try
    End Sub
    Public Sub NuevosRegistro()
        Try
            Dim NewUsuariosConsecutivo As New ConsecutivosUsuario
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewUsuariosConsecutivo.IDComisionista = UsuariosConsecutivoPorDefecto.IDComisionista
            NewUsuariosConsecutivo.IDSucComisionista = UsuariosConsecutivoPorDefecto.IDSucComisionista
            NewUsuariosConsecutivo.Usuario_Consecutivo = UsuariosConsecutivoPorDefecto.Usuario
            NewUsuariosConsecutivo.Nombre_Consecutivo = UsuariosConsecutivoSelected.Nombre_Consecutivo
            NewUsuariosConsecutivo.Actualizacion = UsuariosConsecutivoPorDefecto.Actualizacion
            NewUsuariosConsecutivo.Usuario = Program.Usuario
            NewUsuariosConsecutivo.IDConsecutivosUsuarios = UsuariosConsecutivoPorDefecto.IDConsecutivosUsuarios
            UsuariosConsecutivoAnterior = UsuariosConsecutivoSelected
            UsuariosConsecutivoSelected = NewUsuariosConsecutivo
            MyBase.CambioItem("ConsecutivosUsuarios")
            Editando = True
            EditaReg = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Overrides Sub Filtrar()
        Try

            dcProxy.ConsecutivosUsuarios.Clear()
            dcProxy.ListaUsuarios.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ConsecutivosUsuariosFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosConsecutivos, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ConsecutivosUsuariosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosConsecutivos, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Nombre_Consecutivo <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ConsecutivosUsuarios.Clear()
                dcProxy.ListaUsuarios.Clear()
                UsuariosConsecutivoAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.ConsecutivosUsuariosConsultarQuery(cb.Nombre_Consecutivo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosConsecutivos, "Busqueda")
                consecutivo = cb.Nombre_Consecutivo
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaUsuariosConsecutivo
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()

        Try
            Dim origen = "update"
            ErrorForma = ""

            Dim cambiaron As List(Of ItemUsuariosConsecutivos) = Tabladisponibles.Where(Function(ic) ic.Chequear <> ic.CheckedOriginal).ToList
            Dim cambiaronfalse As List(Of ItemUsuariosConsecutivos) = cambiaron.Where(Function(ai) ai.Chequear = False).ToList
            Dim cambiarontrue As List(Of ItemUsuariosConsecutivos) = cambiaron.Where(Function(ab) ab.Chequear = True).ToList
            Dim resultadofinal As List(Of ItemUsuariosConsecutivos) = Tabladisponibles.Where(Function(o) o.Chequear = True).ToList
            Dim a As ItemUsuariosConsecutivos

            While cambiarontrue.Any
                a = cambiarontrue.FirstOrDefault
                If a.Chequear = True Then
                    NuevosRegistro()
                    UsuariosConsecutivoAnterior = UsuariosConsecutivoSelected
                    tablaSeleccionada = a
                    UsuariosConsecutivoSelected.Usuario_Consecutivo = tablaSeleccionada.Consecutivo
                    If Not ListaUsuariosConsecutivos.Contains(UsuariosConsecutivoSelected) Then
                        origen = "insert"
                        ListaUsuariosConsecutivos.Add(UsuariosConsecutivoSelected)

                    End If
                End If
                If cambiarontrue.Contains(a) Then
                    cambiarontrue.Remove(a)
                End If
            End While

            While cambiaronfalse.Any
                a = cambiaronfalse.FirstOrDefault
                If a.Chequear = False Then
                    If count = 0 Then
                        Nom_consecutivo = UsuariosConsecutivoSelected.Nombre_Consecutivo
                    End If
                    tablaSeleccionada = a
                    For Each e In ListaUsuariosConsecutivos
                        If (tablaSeleccionada.Consecutivo.Equals(e.Usuario_Consecutivo) And Nom_consecutivo.Equals(e.Nombre_Consecutivo)) Then
                            UsuariosConsecutivoSelected = e
                        End If
                    Next
                    If Not IsNothing(UsuariosConsecutivoSelected) Then
                        ListaUsuariosConsecutivos.Remove(UsuariosConsecutivoSelected)
                        UsuariosConsecutivoSelected = _ListaUsuariosConsecutivos.LastOrDefault
                    End If
                End If
                If cambiaronfalse.Contains(a) Then
                    cambiaronfalse.Remove(a)
                End If
                changes = True
                count = count + 1
            End While
            count = 0
            IsBusy = True
            'If resultadofinal.Count = 0 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Debe de registrar minimo un Usuario", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    dcProxy.RejectChanges()
            '    Editando = False
            '    EditaReg = False
            '    UsuariosConsecutivoSelected = UsuariosConsecutivoAnterior
            '    IsBusy = False
            'Else
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            'End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            UsuariosConsecutivoSelected = UsuariosConsecutivoSelected
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If changes = True Then
                    dcProxy.RejectChanges()
                    changes = False
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_UsuariosConsecutivoSelected) Then
            Editando = True
            EditaReg = False
            'consulta = True
            NuevosRegistro()
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_UsuariosConsecutivoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditaReg = False
                If _UsuariosConsecutivoSelected.EntityState = EntityState.Detached Then
                    UsuariosConsecutivoSelected = UsuariosConsecutivoAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '  Try
        '      If Not IsNothing(_UsuariosConsecutivoSelected) Then
        '          dcProxy.ConsecutivosUsuarios.Remove(_UsuariosConsecutivoSelected)
        '          IsBusy = True
        '              dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        '      End If
        '  Catch ex As Exception
        'IsBusy = False
        '      A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '       Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '  End Try
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaUsuariosConsecutivo

    <StringLength(60, ErrorMessage:="La longitud máxima es de 60")> _
     <Display(Name:="Nombre consecutivo")> _
    Public Property Nombre_Consecutivo() As String
End Class

'Clase base para llenar listbox
Public Class ItemUsuariosConsecutivos
    Implements INotifyPropertyChanged
    Private _Chequear As Boolean
    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Checked", Description:="Chequear")> _
    Public Property Chequear() As Boolean
        Get
            Return _Chequear
        End Get
        Set(ByVal value As Boolean)
            _Chequear = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Chequear"))
        End Set
    End Property
    Private _Consecutivo As String
    <Display(Name:="Descripcion", Description:="Descripcion")> _
    Public Property Consecutivo As String
        Get
            Return _Consecutivo
        End Get
        Set(ByVal value As String)
            _Consecutivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Consecutivo"))

        End Set
    End Property

    <Display(Name:="CheckedOriginal", Description:="CheckedOriginal")> _
    Public Property CheckedOriginal As Boolean
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class




