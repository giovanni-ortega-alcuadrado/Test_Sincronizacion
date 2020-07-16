Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: BolsasViewModel.vb
'Generado el : 02/09/2011 11:50:52
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

Public Class BolsasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaBolsa
    Private BolsaPorDefecto As Bolsa
    Private BolsaAnterior As Bolsa
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)


    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
        Else
            'dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            'dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If


        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.BolsasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerBolsaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsasPorDefecto_Completed, "Default")
                'If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then

                'End If
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  BolsasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "BolsasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerBolsasPorDefecto_Completed(ByVal lo As LoadOperation(Of Bolsa))
        If Not lo.HasError Then
            BolsaPorDefecto = lo.Entities.FirstOrDefault
            If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                listapoblacion = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Ciudades").ToList
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Bolsa por defecto", _
                                             Me.ToString(), "TerminoTraerBolsaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerBolsas(ByVal lo As LoadOperation(Of Bolsa))
        If Not lo.HasError Then
            ListaBolsas = dcProxy.Bolsas
            If dcProxy.Bolsas.Count > 0 Then
                If lo.UserState = "insert" Then
                    BolsaSelected = ListaBolsas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'Se comentan estas dos lineas para que no vuelva a regarcar la lista si no encuentra ningun registro.
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Bolsas", Me.ToString, "TerminoTraerBolsas", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bolsas", _
            '                                 Me.ToString(), "TerminoTraerBolsa", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaBolsas As EntitySet(Of Bolsa)
    Public Property ListaBolsas() As EntitySet(Of Bolsa)
        Get
            Return _ListaBolsas
        End Get
        Set(ByVal value As EntitySet(Of Bolsa))
            _ListaBolsas = value

            MyBase.CambioItem("ListaBolsas")
            MyBase.CambioItem("ListaBolsasPaged")
            If Not IsNothing(value) Then
                If IsNothing(BolsaAnterior) Then
                    BolsaSelected = _ListaBolsas.FirstOrDefault
                Else
                    BolsaSelected = BolsaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaBolsasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBolsas) Then
                Dim view = New PagedCollectionView(_ListaBolsas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _BolsaSelected As Bolsa
    Public Property BolsaSelected() As Bolsa
        Get
            Return _BolsaSelected
        End Get
        Set(ByVal value As Bolsa)
            _BolsaSelected = value
            'If Not value Is Nothing Then
            '    _BolsaSelected = value
            'End If
            MyBase.CambioItem("BolsaSelected")
        End Set
    End Property

    Private _listapoblacion As List(Of OYDUtilidades.ItemCombo)
    Public Property listapoblacion As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listapoblacion
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listapoblacion = value
            MyBase.CambioItem("listapoblacion")
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

    Private _HabilitarCodigo As Boolean
    Public Property HabilitarCodigo As Boolean
        Get
            Return _HabilitarCodigo
        End Get
        Set(value As Boolean)
            _HabilitarCodigo = value
            MyBase.CambioItem("HabilitarCodigo")
        End Set
    End Property


#End Region


#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewBolsa As New Bolsa
            'TODO: Verificar cuales son los campos que deben inicializarse
            'NewBolsa.IDComisionista = BolsaPorDefecto.IDComisionista
            'NewBolsa.IDSucComisionista = BolsaPorDefecto.IDSucComisionista
            NewBolsa.IdBolsa = 0
            'NewBolsa.Nombre = BolsaPorDefecto.Nombre
            NewBolsa.IDPoblacion = BolsaPorDefecto.IDPoblacion
            'NewBolsa.NroDocumento = BolsaPorDefecto.NroDocumento
            'NewBolsa.Actualizacion = BolsaPorDefecto.Actualizacion
            NewBolsa.Usuario = Program.Usuario
            'NewBolsa.MercadoIntegrado = BolsaPorDefecto.MercadoIntegrado
            'NewBolsa.Activa = BolsaPorDefecto.Activa

            BolsaAnterior = BolsaSelected
            BolsaSelected = NewBolsa
            If Not IsNothing(listapoblacion) Then
                listapoblacion.Clear()
            End If
            PropiedadTextoCombos = ""
            If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                listapoblacion = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Ciudades").ToList
            End If
            MyBase.CambioItem("listapoblacion")
            BolsaSelected.IDPoblacion = Nothing
            MyBase.CambioItem("BolsaSelected")
            Editando = True
            HabilitarCodigo = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Bolsas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.BolsasFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.BolsasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.IdBolsa = 0
        cb.Nombre = String.Empty
        cb.lngIDPoblacion = 0
        cb.lngNroDocumento = 0
        cb.logMercadoIntegrado = Nothing
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IdBolsa <> 0 Or cb.Nombre <> String.Empty Or cb.lngIDPoblacion <> 0 Or cb.lngNroDocumento <> 0 Or Not IsNothing(cb.logMercadoIntegrado) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Bolsas.Clear()
                BolsaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IdBolsa = " &  cb.IdBolsa.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.BolsasConsultarQuery(cb.IdBolsa, cb.Nombre, cb.lngIDPoblacion, cb.lngNroDocumento, cb.logMercadoIntegrado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaBolsa
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
            If BolsaSelected.IdBolsa = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El Código es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            If BolsaSelected.NroDocumento = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El Nit de la bolsa es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            If BolsaSelected.IDPoblacion = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La ciudad de la bolsa es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            Else
                Dim origen = "update"
                ErrorForma = ""
                'BolsaAnterior = BolsaSelected
                If Not ListaBolsas.Contains(BolsaSelected) Then
                    origen = "insert"
                    ListaBolsas.Add(BolsaSelected)
                End If
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            End If
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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'If So.UserState = "insert" Then
                    '    ListaEmpleados.Remove(EmpleadoSelected)
                    'End If
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
            '    dcProxy.Bolsas.Clear()
            '    dcProxy.Load(dcProxy.BolsasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_BolsaSelected) Then
            Editando = True
            HabilitarCodigo = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_BolsaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                HabilitarCodigo = False
                If _BolsaSelected.EntityState = EntityState.Detached Then
                    BolsaSelected = BolsaAnterior
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
            If Not IsNothing(_BolsaSelected) Then
                'dcProxy.Bolsas.Remove(_BolsaSelected)
                'BolsaSelected = _ListaBolsas.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarBolsa(BolsaSelected.intIDBolsa, BolsaSelected.Nombre, String.Empty, Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
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
                    dcProxy.Bolsas.Clear()
                    dcProxy.Load(dcProxy.BolsasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "insert") ' Recarga la lista para que carguen los include
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
        DicCamposTab.Add("Nombre", 1)
        DicCamposTab.Add("Ciudad", 1)
        DicCamposTab.Add("Nit", 1)
    End Sub



#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaBolsa
    Implements INotifyPropertyChanged

    '<Display(Name:="Código")> _
    'Public Property IdBolsa As Integer

    Private _IdBolsa As Integer
    <Display(Name:="Código")> _
    Public Property IdBolsa As Integer
        Get
            Return _IdBolsa
        End Get
        Set(ByVal value As Integer)
            _IdBolsa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdBolsa"))
        End Set
    End Property

    '<Display(Name:="Nombre")> _
    'Public Property Nombre As String

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

    '<Display(Name:="Ciudad")> _
    'Public Property lngIDPoblacion As Integer

    Private _lngIDPoblacion As Integer
    <Display(Name:="Ciudad")> _
    Public Property lngIDPoblacion As Integer
        Get
            Return _lngIDPoblacion
        End Get
        Set(ByVal value As Integer)
            _lngIDPoblacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDPoblacion"))
        End Set
    End Property

    '<Display(Name:="Nit")> _
    'Public Property lngNroDocumento As Decimal

    Private _lngNroDocumento As Decimal
    <Display(Name:="Nit")> _
    Public Property lngNroDocumento As Decimal
        Get
            Return _lngNroDocumento
        End Get
        Set(ByVal value As Decimal)
            _lngNroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngNroDocumento"))
        End Set
    End Property

    '<Display(Name:="Mercado Integrado")> _
    'Public Property logMercadoIntegrado As System.Nullable(Of Boolean) ' = False

    Private _logMercadoIntegrado As System.Nullable(Of Boolean)
    <Display(Name:="Mercado Integrado")> _
    Public Property logMercadoIntegrado As System.Nullable(Of Boolean)
        Get
            Return _logMercadoIntegrado
        End Get
        Set(ByVal value As System.Nullable(Of Boolean))
            _logMercadoIntegrado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logMercadoIntegrado"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




