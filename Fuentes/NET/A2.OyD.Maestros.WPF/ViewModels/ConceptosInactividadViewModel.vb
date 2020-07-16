Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ConceptosInactividadViewModel.vb
'Generado el : 01/25/2011 14:18:20
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

Public Class ConceptosInactividadViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaConceptosInactivida
	Private ConceptosInactividaPorDefecto As ConceptosInactivida
	Private ConceptosInactividaAnterior As ConceptosInactivida
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
                dcProxy.Load(dcProxy.ConceptosInactividadFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosInactividad, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerConceptosInactividaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosInactividadPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ConceptosInactividadViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConceptosInactividadViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerConceptosInactividadPorDefecto_Completed(ByVal lo As LoadOperation(Of ConceptosInactivida))
        If Not lo.HasError Then
            ConceptosInactividaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConceptosInactivida por defecto", _
                                             Me.ToString(), "TerminoTraerConceptosInactividaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerConceptosInactividad(ByVal lo As LoadOperation(Of ConceptosInactivida))
        If Not lo.HasError Then
            ListaConceptosInactividad = dcProxy.ConceptosInactividas
            If dcProxy.ConceptosInactividas.Count > 0 Then
                If lo.UserState = "insert" Then
                    ConceptosInactividaSelected = ListaConceptosInactividad.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConceptosInactividad", _
                                             Me.ToString(), "TerminoTraerConceptosInactivida", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub


	

#End Region

#Region "Propiedades"

    Private _ListaConceptosInactividad As EntitySet(Of ConceptosInactivida)
    Public Property ListaConceptosInactividad() As EntitySet(Of ConceptosInactivida)
        Get
            Return _ListaConceptosInactividad
        End Get
        Set(ByVal value As EntitySet(Of ConceptosInactivida))
            _ListaConceptosInactividad = value

            MyBase.CambioItem("ListaConceptosInactividad")
            MyBase.CambioItem("ListaConceptosInactividadPaged")
            If Not IsNothing(value) Then
                If IsNothing(ConceptosInactividaAnterior) Then
                    ConceptosInactividaSelected = _ListaConceptosInactividad.FirstOrDefault
                Else
                    ConceptosInactividaSelected = ConceptosInactividaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaConceptosInactividadPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConceptosInactividad) Then
                Dim view = New PagedCollectionView(_ListaConceptosInactividad)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ConceptosInactividaSelected As ConceptosInactivida
    Public Property ConceptosInactividaSelected() As ConceptosInactivida
        Get
            Return _ConceptosInactividaSelected
        End Get
        Set(ByVal value As ConceptosInactivida)
            _ConceptosInactividaSelected = value
            If Not value Is Nothing Then
				            End If
			MyBase.CambioItem("ConceptosInactividaSelected")
    End Set
End Property

    Private _HabilitarCodigo As Boolean = False
    Public Property HabilitarCodigo() As Boolean
        Get
            Return _HabilitarCodigo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodigo = value
            MyBase.CambioItem("HabilitarCodigo")
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

    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.ConceptosInactividas.Clear()
                    dcProxy.Load(dcProxy.ConceptosInactividadFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosInactividad, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewConceptosInactivida As New ConceptosInactivida
			'TODO: Verificar cuales son los campos que deben inicializarse
		NewConceptosInactivida.IDComisionista = ConceptosInactividaPorDefecto.IDComisionista
		NewConceptosInactivida.IDSucComisionista = ConceptosInactividaPorDefecto.IDSucComisionista
		NewConceptosInactivida.ID = ConceptosInactividaPorDefecto.ID
		NewConceptosInactivida.Actividad = ConceptosInactividaPorDefecto.Actividad
		NewConceptosInactivida.Nombre = ConceptosInactividaPorDefecto.Nombre
		NewConceptosInactivida.Actualizacion = ConceptosInactividaPorDefecto.Actualizacion
		NewConceptosInactivida.Usuario = Program.Usuario
		NewConceptosInactivida.IdConceptoInactividad = ConceptosInactividaPorDefecto.IdConceptoInactividad
        ConceptosInactividaAnterior = ConceptosInactividaSelected
        ConceptosInactividaSelected = NewConceptosInactivida
        MyBase.CambioItem("ConceptosInactividad")
            Editando = True
            HabilitarCodigo = True
            MyBase.CambioItem("Editando")

    Catch ex As Exception
		IsBusy = False
        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                     Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
    End Try
End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ConceptosInactividas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ConceptosInactividadFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosInactividad, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ConceptosInactividadFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosInactividad, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ConceptosInactividas.Clear()
                ConceptosInactividaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " ID = " &  cb.ID.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.ConceptosInactividadConsultarQuery(cb.ID, cb.Nombre,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosInactividad, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaConceptosInactivida
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
            ConceptosInactividaAnterior = ConceptosInactividaSelected
            If Not ListaConceptosInactividad.Contains(ConceptosInactividaSelected) Then
                origen = "insert"
                ListaConceptosInactividad.Add(ConceptosInactividaSelected)
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
                dcProxy.ConceptosInactividas.Clear()
                dcProxy.Load(dcProxy.ConceptosInactividadFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosInactividad, "insert") ' Recarga la lista para que carguen los include
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ConceptosInactividaSelected) Then
            Editando = True
            HabilitarCodigo = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ConceptosInactividaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ConceptosInactividaSelected.EntityState = EntityState.Detached Then
                    ConceptosInactividaSelected = ConceptosInactividaAnterior
                End If
            End If
            HabilitarCodigo = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ConceptosInactividaSelected) Then
                'dcProxy.ConceptosInactividas.Remove(_ConceptosInactividaSelected)
                'ConceptosInactividaSelected = _ListaConceptosInactividad.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarConceptosInactividad(ConceptosInactividaSelected.IdConceptoInactividad, String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
            End If
            HabilitarCodigo = False
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

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaConceptosInactivida
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaConceptosInactivida
 	
    <Display(Name:="Código")> _
    Public Property ID As Integer
 	
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String
End Class




