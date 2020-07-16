Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: SucursalesViewModel.vb
'Generado el : 01/21/2011 15:32:26
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
Imports System.Exception

Public Class SucursalesViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaSucursale
	Private SucursalePorDefecto As Sucursale
	Private SucursaleAnterior As Sucursale
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
                dcProxy.Load(dcProxy.SucursalesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSucursales, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerSucursalePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSucursalesPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  SucursalesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "SucursalesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerSucursalesPorDefecto_Completed(ByVal lo As LoadOperation(Of Sucursale))
        If Not lo.HasError Then
            SucursalePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Sucursale por defecto", _
                                             Me.ToString(), "TerminoTraerSucursalePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerSucursales(ByVal lo As LoadOperation(Of Sucursale))
        If Not lo.HasError Then
            ListaSucursales= dcProxy.Sucursales
           	If dcProxy.Sucursales.Count > 0 Then
				If lo.UserState = "insert" Then
                    SucursaleSelected = ListaSucursales.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Sucursale", Me.ToString, "TerminoTraerSucursale", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Sucursales", Me.ToString(), "TerminoTraerSucursale", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub


	

#End Region

#Region "Propiedades"

    Private _ListaSucursales As EntitySet(Of Sucursale)
    Public Property ListaSucursales() As EntitySet(Of Sucursale)
        Get
            Return _ListaSucursales
        End Get
        Set(ByVal value As EntitySet(Of Sucursale))
            _ListaSucursales = value

            MyBase.CambioItem("ListaSucursales")
            MyBase.CambioItem("ListaSucursalesPaged")
            If Not IsNothing(value) Then
                If IsNothing(SucursaleAnterior) Then
                    SucursaleSelected = _ListaSucursales.FirstOrDefault
                Else
                    SucursaleSelected = SucursaleAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaSucursalesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaSucursales) Then
                Dim view = New PagedCollectionView(_ListaSucursales)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _SucursaleSelected As Sucursale
    Public Property SucursaleSelected() As Sucursale
        Get
            Return _SucursaleSelected
        End Get
        Set(ByVal value As Sucursale)
            _SucursaleSelected = value
            If Not value Is Nothing Then
				            End If
			MyBase.CambioItem("SucursaleSelected")
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
            Dim NewSucursale As New Sucursale
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewSucursale.IDComisionista = SucursalePorDefecto.IDComisionista
            NewSucursale.IDSucComisionista = SucursalePorDefecto.IDSucComisionista
            NewSucursale.IDSucursal = SucursalePorDefecto.IDSucursal
            NewSucursale.Nombre = SucursalePorDefecto.Nombre
            NewSucursale.Actualizacion = SucursalePorDefecto.Actualizacion
            NewSucursale.Usuario = Program.Usuario
            NewSucursale.IDSuc = SucursalePorDefecto.IDSuc
            NewSucursale.PorcentajePatrimonioTecnico = SucursalePorDefecto.PorcentajePatrimonioTecnico
            SucursaleAnterior = SucursaleSelected
            SucursaleSelected = NewSucursale
            MyBase.CambioItem("Sucursales")
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
            dcProxy.Sucursales.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.SucursalesFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSucursales, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.SucursalesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSucursales, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDSucursal <> 0 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Sucursales.Clear()
                SucursaleAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDSucursal = " &  cb.IDSucursal.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.SucursalesConsultarQuery(cb.IDSucursal, cb.Nombre,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSucursales, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaSucursale
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
            SucursaleAnterior = SucursaleSelected
            If Not ListaSucursales.Contains(SucursaleSelected) Then
                origen = "insert"
                ListaSucursales.Add(SucursaleSelected)
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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And (So.UserState = "BorrarRegistro") Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.Sucursales.Clear()
            '    dcProxy.Load(dcProxy.SucursalesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSucursales, "insert") ' Recarga la lista para que carguen los include
            'End If
                MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_SucursaleSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_SucursaleSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _SucursaleSelected.EntityState = EntityState.Detached Then
                    SucursaleSelected = SucursaleAnterior
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
            If Not IsNothing(_SucursaleSelected) Then
                dcProxy.Sucursales.Remove(_SucursaleSelected)
                SucursaleSelected = _ListaSucursales.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")

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


    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaSucursale
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
Public Class CamposBusquedaSucursale
 	
    <Display(Name:="Código")> _
    Public Property IDSucursal As Integer

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
    <Display(Name:="Nombre")> _
    Public Property Nombre As String
End Class




