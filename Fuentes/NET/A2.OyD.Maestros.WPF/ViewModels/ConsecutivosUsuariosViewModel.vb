Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ConsecutivosUsuariosViewModel.vb
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

Public Class ConsecutivosUsuariosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaConsecutivosUsuario
    Private ConsecutivosUsuarioPorDefecto As ConsecutivosUsuario
    Private ConsecutivosUsuarioAnterior As ConsecutivosUsuario
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim changes As Boolean
    Dim count As Integer
    Dim Nombre_usuario As String
    Dim usuario As String

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
                dcProxy.Load(dcProxy.ConsecutivosUsuariosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosUsuarios, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerConsecutivosUsuarioPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosUsuariosPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConsecutivosUsuariosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"
    Private Sub TerminoTraertabla(ByVal lo As LoadOperation(Of ListaUsuario))
        If Not lo.HasError Then
            Tabladisponibles.Clear()
            For Each ll In dcProxy.ListaUsuarios
                Tabladisponibles.Add(New ItemConsecutivosusuarios With {.Consecutivo = ll.Consecutivo, .Chequear = ll.Chequear, .CheckedOriginal = ll.Chequear})
            Next
            For Each co In dcProxy.ConsecutivosUsuarios
                If co.Usuario_Consecutivo = ConsecutivosUsuarioSelected.Usuario_Consecutivo Then
                    Tabladisponibles.Add(New ItemConsecutivosusuarios With {.Consecutivo = co.Nombre_Consecutivo, .Chequear = True, .CheckedOriginal = True})
                End If
            Next
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConceptosConsecutivos", _
                                       Me.ToString(), "TerminoTraerConceptosConsecutivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If

    End Sub

    Private Sub TerminoTraerConsecutivosUsuariosPorDefecto_Completed(ByVal lo As LoadOperation(Of ConsecutivosUsuario))
        If Not lo.HasError Then
            ConsecutivosUsuarioPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConsecutivosUsuario por defecto", _
                                             Me.ToString(), "TerminoTraerConsecutivosUsuarioPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerConsecutivosUsuarios(ByVal lo As LoadOperation(Of ConsecutivosUsuario))
        If Not lo.HasError Then
            ListaConsecutivosUsuarios = dcProxy.ConsecutivosUsuarios
            If dcProxy.ConsecutivosUsuarios.Count > 0 Then
                If lo.UserState = "insert" Then
                    ConsecutivosUsuarioSelected = ListaConsecutivosUsuarios.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    If ListaConsecutivosUsuarios.Count = 0 And lo.UserState <> "FiltroVM" Then
                        ConsecutivosUsuarioSelected = New ConsecutivosUsuario
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConsecutivosUsuarios", _
                                             Me.ToString(), "TerminoTraerConsecutivosUsuario", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    'Public Overrides Sub CambiarAForma()
    '    MyBase.CambiarAForma()
    'End Sub
    'Public Overrides Sub CambiarALista()
    '    MyBase.CambiarALista()
    'End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaConsecutivosUsuarios As EntitySet(Of ConsecutivosUsuario)
    Public Property ListaConsecutivosUsuarios() As EntitySet(Of ConsecutivosUsuario)
        Get
            Return _ListaConsecutivosUsuarios
        End Get
        Set(ByVal value As EntitySet(Of ConsecutivosUsuario))
            _ListaConsecutivosUsuarios = value

            MyBase.CambioItem("ListaConsecutivosUsuarios")
            MyBase.CambioItem("ListaConsecutivosUsuariosPaged")
            If Not IsNothing(value) Then
                If IsNothing(ConsecutivosUsuarioAnterior) Then
                    ConsecutivosUsuarioSelected = _ListaConsecutivosUsuarios.FirstOrDefault
                Else
                    ConsecutivosUsuarioSelected = ConsecutivosUsuarioAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaConsecutivosUsuariosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConsecutivosUsuarios) Then
                Dim view = New PagedCollectionView(_ListaConsecutivosUsuarios)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ConsecutivosUsuarioSelected As ConsecutivosUsuario
    Public Property ConsecutivosUsuarioSelected() As ConsecutivosUsuario
        Get
            Return _ConsecutivosUsuarioSelected
        End Get
        Set(ByVal value As ConsecutivosUsuario)
            _ConsecutivosUsuarioSelected = value
            If Not value Is Nothing Then
                If ListaConsecutivosUsuarios.Count = 0 Then
                    ConsecutivosUsuarioSelected.Usuario_Consecutivo = usuario
                End If
                dcProxy.ListaUsuarios.Clear()
                Tabladisponibles.Clear()
                dcProxy.Load(dcProxy.llenarconsecudisponiblesQuery(ConsecutivosUsuarioSelected.Usuario_Consecutivo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraertabla, Nothing)
            End If
            MyBase.CambioItem("ConsecutivosUsuarioSelected")
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
    Private _Tabladisponibles As ObservableCollection(Of ItemConsecutivosusuarios) = New ObservableCollection(Of ItemConsecutivosusuarios)
    Public Property Tabladisponibles() As ObservableCollection(Of ItemConsecutivosusuarios)
        Get
            Return _Tabladisponibles
        End Get
        Set(ByVal value As ObservableCollection(Of ItemConsecutivosusuarios))
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
    Private _tablaSeleccionada As ItemConsecutivosusuarios
    Public Property tablaSeleccionada() As ItemConsecutivosusuarios
        Get
            Return _tablaSeleccionada
        End Get
        Set(ByVal value As ItemConsecutivosusuarios)
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
        '      Try
        '          Dim NewConsecutivosUsuario As New ConsecutivosUsuario
        '	'TODO: Verificar cuales son los campos que deben inicializarse
        'NewConsecutivosUsuario.IDComisionista = ConsecutivosUsuarioPorDefecto.IDComisionista
        'NewConsecutivosUsuario.IDSucComisionista = ConsecutivosUsuarioPorDefecto.IDSucComisionista
        '          NewConsecutivosUsuario.Usuario_Consecutivo = ConsecutivosUsuarioSelected.Usuario_Consecutivo
        '          NewConsecutivosUsuario.Nombre_Consecutivo = ConsecutivosUsuarioSelected.Nombre_Consecutivo
        'NewConsecutivosUsuario.Actualizacion = ConsecutivosUsuarioPorDefecto.Actualizacion
        '          NewConsecutivosUsuario.Usuario = Program.Usuario
        '          NewConsecutivosUsuario.IDConsecutivosUsuarios = ConsecutivosUsuarioPorDefecto.IDConsecutivosUsuarios
        '      ConsecutivosUsuarioAnterior = ConsecutivosUsuarioSelected
        '      ConsecutivosUsuarioSelected = NewConsecutivosUsuario
        '      MyBase.CambioItem("ConsecutivosUsuarios")
        '          Editando = True
        '          EditaReg = True
        '      MyBase.CambioItem("Editando")
        '  Catch ex As Exception
        'IsBusy = False
        '      A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
        '                                                   Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '  End Try
    End Sub
    Public Sub NuevosRegistro()
        Try
            Dim NewConsecutivosUsuario As New ConsecutivosUsuario
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewConsecutivosUsuario.IDComisionista = ConsecutivosUsuarioPorDefecto.IDComisionista
            NewConsecutivosUsuario.IDSucComisionista = ConsecutivosUsuarioPorDefecto.IDSucComisionista
            NewConsecutivosUsuario.Usuario_Consecutivo = ConsecutivosUsuarioSelected.Usuario_Consecutivo
            NewConsecutivosUsuario.Nombre_Consecutivo = "Por defecto"
            NewConsecutivosUsuario.Actualizacion = ConsecutivosUsuarioPorDefecto.Actualizacion
            NewConsecutivosUsuario.Usuario = Program.Usuario
            NewConsecutivosUsuario.IDConsecutivosUsuarios = ConsecutivosUsuarioPorDefecto.IDConsecutivosUsuarios
            ConsecutivosUsuarioAnterior = ConsecutivosUsuarioSelected
            ConsecutivosUsuarioSelected = NewConsecutivosUsuario
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
                dcProxy.Load(dcProxy.ConsecutivosUsuariosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosUsuarios, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ConsecutivosUsuariosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosUsuarios, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Usuario_Consecutivo <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ConsecutivosUsuarios.Clear()
                dcProxy.ListaUsuarios.Clear()
                ConsecutivosUsuarioAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.ConsecutivosUsuariosConsultarQuery(cb.Usuario_Consecutivo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosUsuarios, "Busqueda")
                usuario = cb.Usuario_Consecutivo
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaConsecutivosUsuario
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

            Dim cambiaron As List(Of ItemConsecutivosusuarios) = Tabladisponibles.Where(Function(ic) ic.Chequear <> ic.CheckedOriginal).ToList
            Dim cambiaronfalse As List(Of ItemConsecutivosusuarios) = cambiaron.Where(Function(ai) ai.Chequear = False).ToList
            Dim cambiarontrue As List(Of ItemConsecutivosusuarios) = cambiaron.Where(Function(ab) ab.Chequear = True).ToList
            Dim resultadofinal As List(Of ItemConsecutivosusuarios) = Tabladisponibles.Where(Function(o) o.Chequear = True).ToList
            Dim a As ItemConsecutivosusuarios

            While cambiarontrue.Any
                a = cambiarontrue.FirstOrDefault
                If a.Chequear = True Then
                    NuevosRegistro()
                    ConsecutivosUsuarioAnterior = ConsecutivosUsuarioSelected
                    tablaSeleccionada = a
                    ConsecutivosUsuarioSelected.Nombre_Consecutivo = tablaSeleccionada.Consecutivo
                    If Not ListaConsecutivosUsuarios.Contains(ConsecutivosUsuarioSelected) Then
                        origen = "insert"
                        ListaConsecutivosUsuarios.Add(ConsecutivosUsuarioSelected)

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
                        Nombre_usuario = ConsecutivosUsuarioSelected.Usuario_Consecutivo
                    End If
                    tablaSeleccionada = a
                    For Each e In ListaConsecutivosUsuarios
                        If (tablaSeleccionada.Consecutivo.Equals(e.Nombre_Consecutivo) And Nombre_usuario.Equals(e.Usuario_Consecutivo)) Then
                            ConsecutivosUsuarioSelected = e
                        End If
                    Next
                    If Not IsNothing(ConsecutivosUsuarioSelected) Then
                        ListaConsecutivosUsuarios.Remove(ConsecutivosUsuarioSelected)
                        ConsecutivosUsuarioSelected = _ListaConsecutivosUsuarios.LastOrDefault
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
            '    A2Utilidades.Mensajes.mostrarMensaje("Debe de registrar minimo un Consecutivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    dcProxy.RejectChanges()
            '    Editando = False
            '    EditaReg = False
            '    ConsecutivosUsuarioSelected = ConsecutivosUsuarioAnterior
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
            ConsecutivosUsuarioSelected = ConsecutivosUsuarioSelected
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
        If Not IsNothing(_ConsecutivosUsuarioSelected) Then
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
            If Not IsNothing(_ConsecutivosUsuarioSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditaReg = False
                If _ConsecutivosUsuarioSelected.EntityState = EntityState.Detached Then
                    ConsecutivosUsuarioSelected = ConsecutivosUsuarioAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaConsecutivosUsuario
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()

        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '  Try
        '      If Not IsNothing(_ConsecutivosUsuarioSelected) Then
        '          dcProxy.ConsecutivosUsuarios.Remove(_ConsecutivosUsuarioSelected)
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
Public Class CamposBusquedaConsecutivosUsuario

    <StringLength(60, ErrorMessage:="La longitud máxima es de 60")> _
     <Display(Name:="Usuario Consecutivo")> _
    Public Property Usuario_Consecutivo As String
End Class

'Clase base para llenar listbox
Public Class ItemConsecutivosusuarios
    Implements INotifyPropertyChanged
    Private _Chequear As Boolean
    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Checked", Description:="Chequear")> _
    Public Property Chequear As Boolean
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




