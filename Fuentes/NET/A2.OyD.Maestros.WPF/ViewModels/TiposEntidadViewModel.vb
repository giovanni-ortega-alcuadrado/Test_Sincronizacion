Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: TiposEntidadViewModel.vb
'Generado el : 01/21/2011 16:01:17
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

Public Class TiposEntidadViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaTiposEntida
    Private TiposEntidaPorDefecto As TiposEntida
    Private TiposEntidaAnterior As TiposEntida
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
                dcProxy.Load(dcProxy.TiposEntidadFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEntidad, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerTiposEntidaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEntidadPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  TiposEntidadViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "TiposEntidadViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerTiposEntidadPorDefecto_Completed(ByVal lo As LoadOperation(Of TiposEntida))
        If Not lo.HasError Then
            TiposEntidaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TiposEntida por defecto",
                                             Me.ToString(), "TerminoTraerTiposEntidaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerTiposEntidad(ByVal lo As LoadOperation(Of TiposEntida))
        If Not lo.HasError Then
            ListaTiposEntidad = dcProxy.TiposEntidas
            If dcProxy.TiposEntidas.Count > 0 Then
                If lo.UserState = "insert" Then
                    TiposEntidaSelected = ListaTiposEntidad.Last
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TiposEntidad",
                                             Me.ToString(), "TerminoTraerTiposEntida", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaTiposEntidad As EntitySet(Of TiposEntida)
    Public Property ListaTiposEntidad() As EntitySet(Of TiposEntida)
        Get
            Return _ListaTiposEntidad
        End Get
        Set(ByVal value As EntitySet(Of TiposEntida))
            _ListaTiposEntidad = value

            MyBase.CambioItem("ListaTiposEntidad")
            MyBase.CambioItem("ListaTiposEntidadPaged")
            If Not IsNothing(value) Then
                If IsNothing(TiposEntidaAnterior) Then
                    TiposEntidaSelected = _ListaTiposEntidad.FirstOrDefault
                Else
                    TiposEntidaSelected = TiposEntidaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaTiposEntidadPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTiposEntidad) Then
                Dim view = New PagedCollectionView(_ListaTiposEntidad)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TiposEntidaSelected As TiposEntida
    Public Property TiposEntidaSelected() As TiposEntida
        Get
            Return _TiposEntidaSelected
        End Get
        Set(ByVal value As TiposEntida)
            _TiposEntidaSelected = value
            'If Not value Is Nothing Then
            '    End If
            MyBase.CambioItem("TiposEntidaSelected")
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
            Dim NewTiposEntida As New TiposEntida
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewTiposEntida.IDComisionista = TiposEntidaPorDefecto.IDComisionista
            NewTiposEntida.IDSucComisionista = TiposEntidaPorDefecto.IDSucComisionista
            NewTiposEntida.IDTipoEntidad = TiposEntidaPorDefecto.IDTipoEntidad
            NewTiposEntida.Nombre = TiposEntidaPorDefecto.Nombre
            NewTiposEntida.Actualizacion = TiposEntidaPorDefecto.Actualizacion
            NewTiposEntida.Usuario = Program.Usuario
            NewTiposEntida.IdTipoEntidadI = TiposEntidaPorDefecto.IdTipoEntidadI
            TiposEntidaAnterior = TiposEntidaSelected
            TiposEntidaSelected = NewTiposEntida
            MyBase.CambioItem("TiposEntidad")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.TiposEntidas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.TiposEntidadFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEntidad, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.TiposEntidadFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEntidad, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDTipoEntidad <> 0 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.TiposEntidas.Clear()
                TiposEntidaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDTipoEntidad = " &  cb.IDTipoEntidad.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.TiposEntidadConsultarQuery(cb.IDTipoEntidad, cb.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEntidad, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaTiposEntida
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
            Dim origen = "update"
            ErrorForma = ""
            TiposEntidaAnterior = TiposEntidaSelected
            If Not ListaTiposEntidad.Contains(TiposEntidaSelected) Then
                origen = "insert"
                ListaTiposEntidad.Add(TiposEntidaSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As OpenRiaServices.DomainServices.Client.SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío

                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And (So.UserState = "BorrarRegistro") Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.TiposEntidas.Clear()
            '    dcProxy.Load(dcProxy.TiposEntidadFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEntidad, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_TiposEntidaSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_TiposEntidaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _TiposEntidaSelected.EntityState = EntityState.Detached Then
                    TiposEntidaSelected = TiposEntidaAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_TiposEntidaSelected) Then
                dcProxy.TiposEntidas.Remove(_TiposEntidaSelected)
                TiposEntidaSelected = _ListaTiposEntidad.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
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
    Public Sub llenarDiccionario()
        DicCamposTab.Add("Nombre", 1)
    End Sub
    Public Overrides Sub Buscar()
        cb.IDTipoEntidad = Nothing
        cb.Nombre = String.Empty
        MyBase.Buscar()
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaTiposEntida
    Implements INotifyPropertyChanged
    Private _IDTipoEntidad As Integer
    <Display(Name:="Código")>
    Public Property IDTipoEntidad As Integer
        Get
            Return _IDTipoEntidad
        End Get
        Set(ByVal value As Integer)
            _IDTipoEntidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDTipoEntidad"))
        End Set
    End Property
    Private _Nombre As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Nombre")>
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




