Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClientesExternosViewModel.vb
'Generado el : 02/09/2011 10:20:44
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

Public Class ClientesExternosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaClientesExterno
    Private ClientesExternoPorDefecto As ClientesExterno
    Private ClientesExternoAnterior As ClientesExterno
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

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
                dcProxy.Load(dcProxy.ClientesExternosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesExternos, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerClientesExternoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesExternosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ClientesExternosViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClientesExternosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClientesExternosPorDefecto_Completed(ByVal lo As LoadOperation(Of ClientesExterno))
        If Not lo.HasError Then
            ClientesExternoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClientesExterno por defecto", _
                                             Me.ToString(), "TerminoTraerClientesExternoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesExternos(ByVal lo As LoadOperation(Of ClientesExterno))
        If Not lo.HasError Then
            ListaClientesExternos = dcProxy.ClientesExternos
            If dcProxy.ClientesExternos.Count > 0 Then
                ListaClientesExternos = dcProxy.ClientesExternos
                If lo.UserState = "insert" Then
                    ClientesExternoSelected = ListaClientesExternos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesExternos", _
                                             Me.ToString(), "TerminoTraerClientesExterno", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaClientesExternos As EntitySet(Of ClientesExterno)
    Public Property ListaClientesExternos() As EntitySet(Of ClientesExterno)
        Get
            Return _ListaClientesExternos
        End Get
        Set(ByVal value As EntitySet(Of ClientesExterno))
            _ListaClientesExternos = value

            MyBase.CambioItem("ListaClientesExternos")
            MyBase.CambioItem("ListaClientesExternosPaged")
            If Not IsNothing(value) Then
                If IsNothing(ClientesExternoAnterior) Then
                    ClientesExternoSelected = _ListaClientesExternos.FirstOrDefault
                Else
                    ClientesExternoSelected = ClientesExternoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClientesExternosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClientesExternos) Then
                Dim view = New PagedCollectionView(_ListaClientesExternos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ClientesExternoSelected As ClientesExterno
    Public Property ClientesExternoSelected() As ClientesExterno
        Get
            Return _ClientesExternoSelected
        End Get
        Set(ByVal value As ClientesExterno)
            _ClientesExternoSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("ClientesExternoSelected")
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


    Private _habilitar As Boolean = False
    Public Property habilitar() As Boolean
        Get
            Return _habilitar
        End Get
        Set(ByVal value As Boolean)
            _habilitar = value
            MyBase.CambioItem("habilitar")
        End Set
    End Property


#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewClientesExterno As New ClientesExterno
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewClientesExterno.IDComisionista = ClientesExternoPorDefecto.IDComisionista
            NewClientesExterno.IDSucComisionista = ClientesExternoPorDefecto.IDSucComisionista
            NewClientesExterno.ID = ClientesExternoPorDefecto.ID
            NewClientesExterno.Nombre = ClientesExternoPorDefecto.Nombre
            NewClientesExterno.Vendedor = ClientesExternoPorDefecto.Vendedor
            NewClientesExterno.IDDepositoExtranjero = 0
            NewClientesExterno.NumeroCuenta = ClientesExternoPorDefecto.NumeroCuenta
            NewClientesExterno.NombreTitular = ClientesExternoPorDefecto.NombreTitular
            NewClientesExterno.Actualizacion = ClientesExternoPorDefecto.Actualizacion
            NewClientesExterno.Usuario = Program.Usuario
            NewClientesExterno.IDClienteExt = ClientesExternoPorDefecto.IDClienteExt
            ClientesExternoAnterior = ClientesExternoSelected
            ClientesExternoSelected = NewClientesExterno
            PropiedadTextoCombos = ""
            MyBase.CambioItem("ClientesExternos")
            Editando = True
            MyBase.CambioItem("Editando")
            habilitar = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ClientesExternos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ClientesExternosFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesExternos, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ClientesExternosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesExternos, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> String.Empty Or cb.Nombre <> String.Empty Or cb.Vendedor <> String.Empty Or cb.IDDepositoExtranjero <> 0 Or cb.NumeroCuenta <> String.Empty Or cb.NombreTitular <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ClientesExternos.Clear()
                ClientesExternoAnterior = Nothing
                IsBusy = True
                If cb.IDDepositoExtranjero = 0 Then
                    cb.IDDepositoExtranjero = Nothing
                End If
                'DescripcionFiltroVM = " ID = " &  cb.ID.ToString() & " Nombre = " &  cb.Nombre.ToString() & " Vendedor = " &  cb.Vendedor.ToString() & " IDDepositoExtranjero = " &  cb.IDDepositoExtranjero.ToString() & " NumeroCuenta = " &  cb.NumeroCuenta.ToString() & " NombreTitular = " &  cb.NombreTitular.ToString() 
                dcProxy.Load(dcProxy.ClientesExternosConsultarQuery(cb.ID, cb.Nombre, cb.Vendedor, cb.IDDepositoExtranjero, cb.NumeroCuenta, cb.NombreTitular,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesExternos, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaClientesExterno
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Buscar()
        cb = New CamposBusquedaClientesExterno
        CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            ClientesExternoAnterior = ClientesExternoSelected
            If Not ListaClientesExternos.Contains(ClientesExternoSelected) Then
                origen = "insert"
                ListaClientesExternos.Add(ClientesExternoSelected)
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
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            If So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.ClientesExternos.Clear()
                dcProxy.Load(dcProxy.ClientesExternosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesExternos, "insert") ' Recarga la lista para que carguen los include
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ClientesExternoSelected) Then
            Editando = True
            habilitar = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ClientesExternoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ClientesExternoSelected.EntityState = EntityState.Detached Then
                    ClientesExternoSelected = ClientesExternoAnterior
                End If
            End If
            habilitar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            'If Not IsNothing(_ClientesExternoSelected) Then
            '    dcProxy.ClientesExternos.Remove(_ClientesExternoSelected)
            '    ClientesExternoSelected = _ListaClientesExternos.LastOrDefault
            '    IsBusy = True
            '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            'End If
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            habilitar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("Nombre", 1)
        DicCamposTab.Add("Vendedor", 1)
        DicCamposTab.Add("IDDepositoExtranjero", 1)
        DicCamposTab.Add("NombreTitular", 1)
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaClientesExterno
    Implements INotifyPropertyChanged

    <Display(Name:="Código")> _
    Public Property ID As String

    <Display(Name:="Nombre")> _
    Public Property Nombre As String

    <Display(Name:="Vendedor")> _
    Public Property Vendedor As String

    <Display(Name:="Depósito Extranjero")> _
    Public Property IDDepositoExtranjero As Integer

    <Display(Name:="Número Cuenta")> _
    Public Property NumeroCuenta As String

    <Display(Name:="Nombre Titular")> _
    Public Property NombreTitular As String

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




