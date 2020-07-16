Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: Usuarios_SeleccionarViewModel.vb
'Generado el : 03/05/2012 10:50:45
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

Public Class Usuarios_SeleccionarViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaUsuarios_Selecciona
    Private Usuarios_SeleccionaPorDefecto As Usuarios_Selecciona
	Private Usuarios_SeleccionaAnterior As Usuarios_Selecciona
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New MaestrosDomainContext()
                dcProxy1 = New MaestrosDomainContext()
            Else
                dcProxy = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.Usuarios_SeleccionarFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuarios_Seleccionar, "")
                dcProxy1.Load(dcProxy1.TraerUsuarios_SeleccionaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuarios_SeleccionarPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  Usuarios_SeleccionarViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Usuarios_SeleccionarViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerUsuarios_SeleccionarPorDefecto_Completed(ByVal lo As LoadOperation(Of Usuarios_Selecciona))
        If Not lo.HasError Then
            Usuarios_SeleccionaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Usuarios_Selecciona por defecto", _
                                             Me.ToString(), "TerminoTraerUsuarios_SeleccionaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerUsuarios_Seleccionar(ByVal lo As LoadOperation(Of Usuarios_Selecciona))
        If Not lo.HasError Then
            ListaUsuarios_Seleccionar = dcProxy.Usuarios_Seleccionas
            If dcProxy.Usuarios_Seleccionas.Count > 0 Then
                If lo.UserState = "insert" Then
                    Usuarios_SeleccionaSelected = ListaUsuarios_Seleccionar.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MessageBox.Show("No se encontró ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Usuarios_Seleccionar", _
                                             Me.ToString(), "TerminoTraerUsuarios_Selecciona", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

'Tablas padres

	

#End Region

#Region "Propiedades"

    Private _ListaUsuarios_Seleccionar As EntitySet(Of Usuarios_Selecciona)
    Public Property ListaUsuarios_Seleccionar() As EntitySet(Of Usuarios_Selecciona)
        Get
            Return _ListaUsuarios_Seleccionar
        End Get
        Set(ByVal value As EntitySet(Of Usuarios_Selecciona))
            _ListaUsuarios_Seleccionar = value

            MyBase.CambioItem("ListaUsuarios_Seleccionar")
            MyBase.CambioItem("ListaUsuarios_SeleccionarPaged")
            If Not IsNothing(value) Then
                If IsNothing(Usuarios_SeleccionaAnterior) Then
                    Usuarios_SeleccionaSelected = _ListaUsuarios_Seleccionar.FirstOrDefault
                Else
                    Usuarios_SeleccionaSelected = Usuarios_SeleccionaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaUsuarios_SeleccionarPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaUsuarios_Seleccionar) Then
                Dim view = New PagedCollectionView(_ListaUsuarios_Seleccionar)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _Usuarios_SeleccionaSelected As Usuarios_Selecciona
    Public Property Usuarios_SeleccionaSelected() As Usuarios_Selecciona
        Get
            Return _Usuarios_SeleccionaSelected
        End Get
        Set(ByVal value As Usuarios_Selecciona)
            _Usuarios_SeleccionaSelected = value
			MyBase.CambioItem("Usuarios_SeleccionaSelected")
    End Set
End Property
		
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewUsuarios_Selecciona As New Usuarios_Selecciona
            'TODO: Verificar cuales son los campos que deben inicializarse
            If (ListaUsuarios_Seleccionar.Count) = 0 Or IsNothing(ListaUsuarios_Seleccionar) Then
                NewUsuarios_Selecciona.Id = Usuarios_SeleccionaPorDefecto.Id
            Else
                NewUsuarios_Selecciona.Id = ListaUsuarios_Seleccionar.Last.Id + 1
            End If


            NewUsuarios_Selecciona.Login = Usuarios_SeleccionaPorDefecto.Login
            Usuarios_SeleccionaAnterior = Usuarios_SeleccionaSelected
            Usuarios_SeleccionaSelected = NewUsuarios_Selecciona
            MyBase.CambioItem("Usuarios_Seleccionar")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Usuarios_Seleccionas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.Usuarios_SeleccionarFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuarios_Seleccionar, Nothing)
            Else
                dcProxy.Load(dcProxy.Usuarios_SeleccionarFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuarios_Seleccionar, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Id <> 0 Or cb.Login <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Usuarios_Seleccionas.Clear()
                Usuarios_SeleccionaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Id = " &  cb.Id.ToString() & " Login = " &  cb.Login.ToString()    'Dic202011 quitar
                dcProxy.Load(dcProxy.Usuarios_SeleccionarConsultarQuery(cb.Id, cb.Login,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuarios_Seleccionar, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaUsuarios_Selecciona
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
            For Each led In ListaUsuarios_Seleccionar
                If Not IsNothing(Usuarios_SeleccionaSelected.Login) Then
                    If led.Login.ToUpper = Usuarios_SeleccionaSelected.Login.ToUpper Then
                        A2Utilidades.Mensajes.mostrarMensaje("El usuario  Ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
            Next
            Dim origen = "update"
            ErrorForma = ""
            Usuarios_SeleccionaAnterior = Usuarios_SeleccionaSelected
            If Not ListaUsuarios_Seleccionar.Contains(Usuarios_SeleccionaSelected) Then
                origen = "insert"
                ListaUsuarios_Seleccionar.Add(Usuarios_SeleccionaSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
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
        'If Not IsNothing(_Usuarios_SeleccionaSelected) Then
        '    Editando = True
        'End If
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_Usuarios_SeleccionaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _Usuarios_SeleccionaSelected.EntityState = EntityState.Detached Then
                    Usuarios_SeleccionaSelected = Usuarios_SeleccionaAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_Usuarios_SeleccionaSelected) Then
                dcProxy.Usuarios_Seleccionas.Remove(_Usuarios_SeleccionaSelected)
                Usuarios_SeleccionaSelected = _ListaUsuarios_Seleccionar.LastOrDefault  'Dic202011  nueva
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Buscar()
        cb = New CamposBusquedaUsuarios_Selecciona
        CambioItem("cb")
        MyBase.Buscar()
    End Sub


#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaUsuarios_Selecciona
 	
    <Display(Name:="Id")> _
    Public Property Id As Integer
 	
    <StringLength(60, ErrorMessage:="La longitud máxima es de 60")> _
     <Display(Name:="Login")> _
    Public Property Login As String
End Class




