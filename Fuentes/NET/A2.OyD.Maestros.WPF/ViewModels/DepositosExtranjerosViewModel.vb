Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: DepositosExtranjerosViewModel.vb
'Generado el : 02/28/2011 12:38:32
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

Public Class DepositosExtranjerosViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaDepositosExtranjero
	Private DepositosExtranjeroPorDefecto As DepositosExtranjero
	Private DepositosExtranjeroAnterior As DepositosExtranjero
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
                dcProxy.Load(dcProxy.DepositosExtranjerosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepositosExtranjeros, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerDepositosExtranjeroPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepositosExtranjerosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  DepositosExtranjerosViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "DepositosExtranjerosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerDepositosExtranjerosPorDefecto_Completed(ByVal lo As LoadOperation(Of DepositosExtranjero))
        If Not lo.HasError Then
            DepositosExtranjeroPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de DepositosExtranjeros por defecto", _
                                             Me.ToString(), "TerminoTraerDepositosExtranjeroPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDepositosExtranjeros(ByVal lo As LoadOperation(Of DepositosExtranjero))
        If Not lo.HasError Then
            ListaDepositosExtranjeros = dcProxy.DepositosExtranjeros
            If dcProxy.DepositosExtranjeros.Count > 0 Then
                If lo.UserState = "insert" Then
                    DepositosExtranjeroSelected = ListaDepositosExtranjeros.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DepositosExtranjeros", _
                                             Me.ToString(), "TerminoTraerDepositosExtranjero", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.DepositosExtranjeros.Clear()
                    dcProxy.Load(dcProxy.DepositosExtranjerosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepositosExtranjeros, "insert")
                End If
            End If
        End If
        IsBusy = False
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaDepositosExtranjeros As EntitySet(Of DepositosExtranjero)
    Public Property ListaDepositosExtranjeros() As EntitySet(Of DepositosExtranjero)
        Get
            Return _ListaDepositosExtranjeros
        End Get
        Set(ByVal value As EntitySet(Of DepositosExtranjero))
            _ListaDepositosExtranjeros = value

            MyBase.CambioItem("ListaDepositosExtranjeros")
            MyBase.CambioItem("ListaDepositosExtranjerosPaged")
            If Not IsNothing(value) Then
                If IsNothing(DepositosExtranjeroAnterior) Then
                    DepositosExtranjeroSelected = _ListaDepositosExtranjeros.FirstOrDefault
                Else
                    DepositosExtranjeroSelected = DepositosExtranjeroAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaDepositosExtranjerosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDepositosExtranjeros) Then
                Dim view = New PagedCollectionView(_ListaDepositosExtranjeros)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DepositosExtranjeroSelected As DepositosExtranjero
    Public Property DepositosExtranjeroSelected() As DepositosExtranjero
        Get
            Return _DepositosExtranjeroSelected
        End Get
        Set(ByVal value As DepositosExtranjero)
            _DepositosExtranjeroSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("DepositosExtranjeroSelected")
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
            Dim NewDepositosExtranjero As New DepositosExtranjero
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewDepositosExtranjero.IDComisionista = DepositosExtranjeroPorDefecto.IDComisionista
            NewDepositosExtranjero.IDSucComisionista = DepositosExtranjeroPorDefecto.IDSucComisionista
            NewDepositosExtranjero.IDdeposito = DepositosExtranjeroPorDefecto.IDdeposito
            NewDepositosExtranjero.Nombre = DepositosExtranjeroPorDefecto.Nombre
            NewDepositosExtranjero.IDPais = DepositosExtranjeroPorDefecto.IDPais
            NewDepositosExtranjero.Actualizacion = DepositosExtranjeroPorDefecto.Actualizacion
            NewDepositosExtranjero.Usuario = Program.Usuario
            DepositosExtranjeroAnterior = DepositosExtranjeroSelected
            DepositosExtranjeroSelected = NewDepositosExtranjero
            MyBase.CambioItem("DepositosExtranjeros")
            MyBase.CambioItem("DepositosExtranjeroSelected")
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
            dcProxy.DepositosExtranjeros.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.DepositosExtranjerosFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepositosExtranjeros, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.DepositosExtranjerosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepositosExtranjeros, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDdeposito <> 0 Or cb.Nombre <> String.Empty Or cb.IDPais <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.DepositosExtranjeros.Clear()
                DepositosExtranjeroAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDdeposito = " &  cb.IDdeposito.ToString() & " Nombre = " &  cb.Nombre.ToString() & " IDPais = " &  cb.IDPais.ToString() 
                dcProxy.Load(dcProxy.DepositosExtranjerosConsultarQuery(cb.IDdeposito, cb.Nombre, cb.IDPais,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepositosExtranjeros, "Busqueda")
                MyBase.ConfirmarBuscar()
                'cb = New CamposBusquedaDepositosExtranjero
                'CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If DepositosExtranjeroSelected.IDPais = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe Seleccionar un país", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim origen = "update"
            ErrorForma = ""
            DepositosExtranjeroAnterior = DepositosExtranjeroSelected
            If Not ListaDepositosExtranjeros.Contains(DepositosExtranjeroSelected) Then
                origen = "insert"
                ListaDepositosExtranjeros.Add(DepositosExtranjeroSelected)
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
            'If So.UserState = "insert" Then
            '    dcProxy.DepositosExtranjeros.Clear()
            '    dcProxy.Load(dcProxy.DepositosExtranjerosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepositosExtranjeros, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_DepositosExtranjeroSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_DepositosExtranjeroSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _DepositosExtranjeroSelected.EntityState = EntityState.Detached Then
                    DepositosExtranjeroSelected = DepositosExtranjeroAnterior
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
            If Not IsNothing(_DepositosExtranjeroSelected) Then
                'dcProxy.DepositosExtranjeros.Remove(_DepositosExtranjeroSelected)
                'DepositosExtranjeroSelected = _ListaDepositosExtranjeros.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarDepositoExtranjero(DepositosExtranjeroSelected.IDdeposito,
                                                   String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
            End If
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
    End Sub

    Public Overrides Sub Buscar()
        cb.IDdeposito = 0
        cb.IDPais = 0
        cb.Nombre = String.Empty
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaDepositosExtranjero
    Implements INotifyPropertyChanged

    Private _IDdeposito As Integer = 0
    <Display(Name:="Código")> _
    Public Property IDdeposito As Integer
        Get
            Return _IDdeposito
        End Get
        Set(ByVal value As Integer)
            _IDdeposito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDdeposito"))
        End Set
    End Property

    Private _Nombre As String = String.Empty
    <Display(Name:="Nombre")> _
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Private _IDPais As Integer = 0
    <Display(Name:="País")> _
    Public Property IDPais As Integer
        Get
            Return _IDPais
        End Get
        Set(ByVal value As Integer)
            _IDPais = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDPais"))
        End Set
    End Property
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




