Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ResolucionesFacturasViewModel.vb
'Generado el : 06/22/2011 12:07:42
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

Public Class ResolucionesFacturasViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaResolucionesFactura
	Private ResolucionesFacturaPorDefecto As ResolucionesFactura
	Private ResolucionesFacturaAnterior As ResolucionesFactura
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
                dcProxy.Load(dcProxy.ResolucionesFacturasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerResolucionesFacturas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerResolucionesFacturaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerResolucionesFacturasPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ResolucionesFacturasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ResolucionesFacturasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerResolucionesFacturasPorDefecto_Completed(ByVal lo As LoadOperation(Of ResolucionesFactura))
        If Not lo.HasError Then
            ResolucionesFacturaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ResolucionesFactura por defecto", _
                                             Me.ToString(), "TerminoTraerResolucionesFacturaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerResolucionesFacturas(ByVal lo As LoadOperation(Of ResolucionesFactura))
        If Not lo.HasError Then
            ListaResolucionesFacturas= dcProxy.ResolucionesFacturas
           	If dcProxy.ResolucionesFacturas.Count > 0 Then
				If lo.UserState = "insert" Then
                    ResolucionesFacturaSelected = ListaResolucionesFacturas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ResolucionesFacturas", _
                                             Me.ToString(), "TerminoTraerResolucionesFactura", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

'Tablas padres

	

#End Region

#Region "Propiedades"

    Private _ListaResolucionesFacturas As EntitySet(Of ResolucionesFactura)
    Public Property ListaResolucionesFacturas() As EntitySet(Of ResolucionesFactura)
        Get
            Return _ListaResolucionesFacturas
        End Get
        Set(ByVal value As EntitySet(Of ResolucionesFactura))
            _ListaResolucionesFacturas = value

            MyBase.CambioItem("ListaResolucionesFacturas")
            MyBase.CambioItem("ListaResolucionesFacturasPaged")
            If Not IsNothing(value) Then
                If IsNothing(ResolucionesFacturaAnterior) Then
                    ResolucionesFacturaSelected = _ListaResolucionesFacturas.FirstOrDefault
                Else
                    ResolucionesFacturaSelected = ResolucionesFacturaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaResolucionesFacturasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResolucionesFacturas) Then
                Dim view = New PagedCollectionView(_ListaResolucionesFacturas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ResolucionesFacturaSelected As ResolucionesFactura
    Public Property ResolucionesFacturaSelected() As ResolucionesFactura
        Get
            Return _ResolucionesFacturaSelected
        End Get
        Set(ByVal value As ResolucionesFactura)
            _ResolucionesFacturaSelected = value
			MyBase.CambioItem("ResolucionesFacturaSelected")
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
            Dim NewResolucionesFactura As New ResolucionesFactura
			'TODO: Verificar cuales son los campos que deben inicializarse
		NewResolucionesFactura.IDComisionista = ResolucionesFacturaPorDefecto.IDComisionista
		NewResolucionesFactura.IDSucComisionista = ResolucionesFacturaPorDefecto.IDSucComisionista
            NewResolucionesFactura.IDCodigoResolucion = ResolucionesFacturaPorDefecto.IDCodigoResolucion
            NewResolucionesFactura.NumeroResolucion = Nothing
		NewResolucionesFactura.DescripcionResolucion = ResolucionesFacturaPorDefecto.DescripcionResolucion
		NewResolucionesFactura.Usuario = Program.Usuario
		NewResolucionesFactura.Actualizacion = ResolucionesFacturaPorDefecto.Actualizacion
        ResolucionesFacturaAnterior = ResolucionesFacturaSelected
        ResolucionesFacturaSelected = NewResolucionesFactura
        MyBase.CambioItem("ResolucionesFacturas")
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
        dcProxy.ResolucionesFacturas.Clear()
        IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ResolucionesFacturasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerResolucionesFacturas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ResolucionesFacturasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerResolucionesFacturas, "Filtrar")
            End If
    Catch ex As Exception
		IsBusy = False
        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                         Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
    End Try
End Sub


Public Overrides Sub ConfirmarBuscar()
	Try
            If cb.NumeroResolucion <> 0 Or cb.DescripcionResolucion <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ResolucionesFacturas.Clear()
                ResolucionesFacturaAnterior = Nothing
                IsBusy = True
                '	DescripcionFiltroVM = " NumeroResolucion = " &  cb.NumeroResolucion.ToString() 
                dcProxy.Load(dcProxy.ResolucionesFacturasConsultarQuery(cb.NumeroResolucion, cb.DescripcionResolucion,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerResolucionesFacturas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaResolucionesFactura
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
            If IsNothing(ResolucionesFacturaSelected.NumeroResolucion) Then
                A2Utilidades.Mensajes.mostrarMensaje("El Número de Resolución no puede quedar vacio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                For Each led In ListaResolucionesFacturas
                    If Not ListaResolucionesFacturas.Contains(ResolucionesFacturaSelected) Then
                        If Not IsNothing(ResolucionesFacturaSelected.NumeroResolucion) Then
                            If led.NumeroResolucion = ResolucionesFacturaSelected.NumeroResolucion Then
                                A2Utilidades.Mensajes.mostrarMensaje("El número de Resolución " + ResolucionesFacturaSelected.NumeroResolucion.ToString + " Ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If

            Dim origen = "update"
            ErrorForma = ""
            ResolucionesFacturaAnterior = ResolucionesFacturaSelected
            If Not ListaResolucionesFacturas.Contains(ResolucionesFacturaSelected) Then
                origen = "insert"
                ListaResolucionesFacturas.Add(ResolucionesFacturaSelected)
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
        If Not IsNothing(_ResolucionesFacturaSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

Public Overrides Sub CancelarEditarRegistro()
    Try
        ErrorForma = ""
        If Not IsNothing(_ResolucionesFacturaSelected) Then
            dcProxy.RejectChanges()
            Editando = False
            If _ResolucionesFacturaSelected.EntityState = EntityState.Detached Then
                ResolucionesFacturaSelected = ResolucionesFacturaAnterior
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
        If Not IsNothing(_ResolucionesFacturaSelected) Then
                'dcProxy.ResolucionesFacturas.Remove(_ResolucionesFacturaSelected)
                'ResolucionesFacturaSelected = ListaResolucionesFacturas.LastOrDefault
            IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarResolucionesFacturas(ResolucionesFacturaSelected.IDCodigoResolucion, String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
        End If
    Catch ex As Exception
		IsBusy = False
        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
         Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
    End Try
    End Sub


    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.ResolucionesFacturas.Clear()
                    dcProxy.Load(dcProxy.ResolucionesFacturasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerResolucionesFacturas, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("DescripcionResolucion", 1)
    End Sub

    'Private Sub _ResolucionesFacturaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ResolucionesFacturaSelected.PropertyChanged
    '    If e.PropertyName.Equals("NumeroResolucion") Then
    '        For Each led In ListaResolucionesFacturas
    '            If Not IsNothing(ResolucionesFacturaSelected.NumeroResolucion) Then
    '                If led.NumeroResolucion = ResolucionesFacturaSelected.NumeroResolucion Then
    '                    A2Utilidades.Mensajes.mostrarMensaje("El número de Resolución " + ResolucionesFacturaSelected.NumeroResolucion.ToString + " Ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '                    Exit Sub
    '                End If
    '            End If
    '        Next
    '    End If
    'End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaResolucionesFactura
 	
    <Display(Name:="Número Resolución")> _
    Public Property NumeroResolucion As Int64

    <Display(Name:="Descripción Resolución")> _
    Public Property DescripcionResolucion As String

End Class




