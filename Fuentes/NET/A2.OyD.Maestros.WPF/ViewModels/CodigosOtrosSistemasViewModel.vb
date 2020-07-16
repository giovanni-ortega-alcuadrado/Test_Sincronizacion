Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: CodigosOtrosSistemasViewModel.vb
'Generado el : 04/27/2011 15:43:42
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

Public Class CodigosOtrosSistemasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCodigosOtrosSistema
    Private CodigosOtrosSistemaPorDefecto As CodigosOtrosSistema
    Private CodigosOtrosSistemaAnterior As CodigosOtrosSistema
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.CodigosOtrosSistemasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosOtrosSistemas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCodigosOtrosSistemaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosOtrosSistemasPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  CodigosOtrosSistemasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CodigosOtrosSistemasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCodigosOtrosSistemasPorDefecto_Completed(ByVal lo As LoadOperation(Of CodigosOtrosSistema))
        If Not lo.HasError Then
            CodigosOtrosSistemaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la CodigosOtrosSistema por defecto", _
                                             Me.ToString(), "TerminoTraerCodigosOtrosSistemaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCodigosOtrosSistemas(ByVal lo As LoadOperation(Of CodigosOtrosSistema))
        If Not lo.HasError Then
            ListaCodigosOtrosSistemas = dcProxy.CodigosOtrosSistemas
            If dcProxy.CodigosOtrosSistemas.Count > 0 Then
                If lo.UserState = "insert" Then
                    CodigosOtrosSistemaSelected = ListaCodigosOtrosSistemas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CodigosOtrosSistemas", _
                                             Me.ToString(), "TerminoTraerCodigosOtrosSistema", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaCodigosOtrosSistemas As EntitySet(Of CodigosOtrosSistema)
    Public Property ListaCodigosOtrosSistemas() As EntitySet(Of CodigosOtrosSistema)
        Get
            Return _ListaCodigosOtrosSistemas
        End Get
        Set(ByVal value As EntitySet(Of CodigosOtrosSistema))
            _ListaCodigosOtrosSistemas = value

            MyBase.CambioItem("ListaCodigosOtrosSistemas")
            MyBase.CambioItem("ListaCodigosOtrosSistemasPaged")
            If Not IsNothing(value) Then
                If IsNothing(CodigosOtrosSistemaAnterior) Then
                    CodigosOtrosSistemaSelected = _ListaCodigosOtrosSistemas.FirstOrDefault
                Else
                    CodigosOtrosSistemaSelected = CodigosOtrosSistemaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCodigosOtrosSistemasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCodigosOtrosSistemas) Then
                Dim view = New PagedCollectionView(_ListaCodigosOtrosSistemas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _CodigosOtrosSistemaSelected As CodigosOtrosSistema
    Public Property CodigosOtrosSistemaSelected() As CodigosOtrosSistema
        Get
            Return _CodigosOtrosSistemaSelected
        End Get
        Set(ByVal value As CodigosOtrosSistema)
            _CodigosOtrosSistemaSelected = value
            buscarItem("Comitente")
            MyBase.CambioItem("CodigosOtrosSistemaSelected")
        End Set
    End Property
    Private _Enabled As Boolean = False
    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value

            MyBase.CambioItem("Enabled")
        End Set
    End Property
    Private _Cambio As Boolean = False
    Public Property Cambio() As Boolean
        Get
            Return _Cambio
        End Get
        Set(ByVal value As Boolean)
            _Cambio = value

            MyBase.CambioItem("Cambio")
        End Set
    End Property

    Private _ComitenteClaseSelected As Comitente = New Comitente

    Public Property ComitenteClaseSelected As Comitente
        Get
            Return _ComitenteClaseSelected
        End Get
        Set(ByVal value As Comitente)
            _ComitenteClaseSelected = value
            MyBase.CambioItem("ComitenteClaseSelected")
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
            Dim NewCodigosOtrosSistema As New CodigosOtrosSistema
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCodigosOtrosSistema.Id = CodigosOtrosSistemaPorDefecto.Id
            NewCodigosOtrosSistema.IdComisionista = CodigosOtrosSistemaPorDefecto.IdComisionista
            NewCodigosOtrosSistema.IdSucComisionista = CodigosOtrosSistemaPorDefecto.IdSucComisionista
            NewCodigosOtrosSistema.Comitente = CodigosOtrosSistemaPorDefecto.Comitente
            NewCodigosOtrosSistema.Sistema = CodigosOtrosSistemaPorDefecto.Sistema
            NewCodigosOtrosSistema.CodigoSistema = CodigosOtrosSistemaPorDefecto.CodigoSistema
            NewCodigosOtrosSistema.Actualizacion = CodigosOtrosSistemaPorDefecto.Actualizacion
            NewCodigosOtrosSistema.Usuario = Program.Usuario
            CodigosOtrosSistemaAnterior = CodigosOtrosSistemaSelected
            CodigosOtrosSistemaSelected = NewCodigosOtrosSistema
            MyBase.CambioItem("CodigosOtrosSistemas")
            Editando = True
            Enabled = True
            Cambio = True
            ComitenteClaseSelected.Cambio = True
            MyBase.CambioItem("Editando")
            ComitenteClaseSelected.Comitente = String.Empty
            habilitar = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.CodigosOtrosSistemas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CodigosOtrosSistemasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosOtrosSistemas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CodigosOtrosSistemasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosOtrosSistemas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Comitente <> String.Empty Or cb.CodigoSistema <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CodigosOtrosSistemas.Clear()
                CodigosOtrosSistemaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Comitente = " &  cb.Comitente.ToString() & " CodigoSistema = " &  cb.CodigoSistema.ToString() 
                dcProxy.Load(dcProxy.CodigosOtrosSistemasConsultarQuery(cb.Comitente, cb.CodigoSistema,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosOtrosSistemas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCodigosOtrosSistema
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
            CodigosOtrosSistemaAnterior = CodigosOtrosSistemaSelected
            If Not ListaCodigosOtrosSistemas.Contains(CodigosOtrosSistemaSelected) Then
                origen = "insert"
                ListaCodigosOtrosSistemas.Add(CodigosOtrosSistemaSelected)
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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert")) Or ((So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaCodigosOtrosSistemas.Remove(CodigosOtrosSistemaSelected)
                    End If
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
            '    dcProxy.CodigosOtrosSistemas.Clear()
            '    dcProxy.Load(dcProxy.CodigosOtrosSistemasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosOtrosSistemas, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
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
                    dcProxy.CodigosOtrosSistemas.Clear()
                    dcProxy.Load(dcProxy.CodigosOtrosSistemasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosOtrosSistemas, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CodigosOtrosSistemaSelected) Then
            Editando = True
            Enabled = False
            Cambio = False
            habilitar = False
            ComitenteClaseSelected.Cambio = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CodigosOtrosSistemaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Enabled = False
                Cambio = False
                ComitenteClaseSelected.Cambio = False
                If _CodigosOtrosSistemaSelected.EntityState = EntityState.Detached Then
                    CodigosOtrosSistemaSelected = CodigosOtrosSistemaAnterior
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
            If Not IsNothing(_CodigosOtrosSistemaSelected) Then
                'dcProxy.CodigosOtrosSistemas.Remove(_CodigosOtrosSistemaSelected)
                'CodigosOtrosSistemaSelected = _ListaCodigosOtrosSistemas.LastOrDefault
                IsBusy = True
                dcProxy.EliminarCodigoOtrosSistemas(CodigosOtrosSistemaSelected.Id, CodigosOtrosSistemaSelected.Comitente, CodigosOtrosSistemaSelected.Sistema, CodigosOtrosSistemaSelected.CodigoSistema, CodigosOtrosSistemaSelected.Usuario, String.Empty, Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
            habilitar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.CodigosOtrosSistemaSelected Is Nothing Then
                Select Case pstrTipoItem.ToLower()
                    Case "comitente"
                        'pstrIdItem = pstrIdItem.Trim()
                        If pstrIdItem.Equals(String.Empty) Then
                            If Not IsNothing(Me.CodigosOtrosSistemaSelected.Comitente) Then
                                strIdItem = Me.CodigosOtrosSistemaSelected.Comitente
                            End If
                        Else
                            strIdItem = pstrIdItem
                        End If
                        If Not strIdItem.Equals(String.Empty) Then
                            logConsultar = True
                        End If
                        If logConsultar Then
                            mdcProxyUtilidad01.BuscadorClientes.Clear()
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(strIdItem, Program.Usuario, "IdComitenteLectura", Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                        End If
                    Case Else
                        logConsultar = False
                End Select


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarGenericoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Select Case strTipoItem.ToLower()
                    Case "comitente"
                        Me.ComitenteClaseSelected.Comitente = lo.Entities.ToList.Item(0).Nombre
                End Select
            Else
                Select Case strTipoItem.ToLower()
                    Case "comitente"
                        Me.ComitenteClaseSelected.Comitente = String.Empty
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("Comitente", 1)
        DicCamposTab.Add("Sistema", 1)
        DicCamposTab.Add("CodigoSistema", 1)
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaCodigosOtrosSistema
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaCodigosOtrosSistema
    Implements INotifyPropertyChanged


    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
     <Display(Name:="Comitente")> _
    Private _Comitente As String
    Public Property Comitente As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Private _nombre As String
    Public Property NombreComitente() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreComitente"))
        End Set
    End Property


    <StringLength(30, ErrorMessage:="La longitud máxima es de 30")> _
     <Display(Name:="Código Sistema")> _
    Private _CodigoSistema As String
    Public Property CodigoSistema() As String
        Get
            Return _CodigoSistema
        End Get
        Set(ByVal value As String)
            _CodigoSistema = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoSistema"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

Public Class Comitente
    Implements INotifyPropertyChanged

    Private _Comitente As String
    <Display(Name:=" ")> _
    Public Property Comitente As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Private _Cambio As Boolean = False
    Public Property Cambio() As Boolean
        Get
            Return _Cambio
        End Get
        Set(ByVal value As Boolean)
            _Cambio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Cambio"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




