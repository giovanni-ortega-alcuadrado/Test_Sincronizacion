Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClientesFondosPensionesViewModel.vb
'Generado el : 03/23/2011 13:41:42
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

Public Class ClientesFondosPensionesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged
    Public Property cb As New CamposBusquedaClientesFondosPensione
    Private ClientesFondosPensionePorDefecto As ClientesFondosPensione
    Private ClientesFondosPensioneAnterior As ClientesFondosPensione
    Public ViewClientesExentoPordefecto As ViewClientes_Exento
    Private ViewClientesExentoAnterior As ViewClientes_Exento
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

                'dcProxy.Load(dcProxy.ClientesFondosPensionesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFondosPensiones, "FiltroInicial")
                ' dcProxy1.Load(dcProxy1.TraerClientesFondosPensionePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFondosPensionesPorDefecto_Completed, "Default")
                dcProxy.Load(dcProxy.Clientes_ExentosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerViewClientesExento, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerClientes_ExentoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerViewClientesExentoPordefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClientesFondosPensionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"
    '4
    Private Sub TerminoTraerClientesFondosPensionesPorDefecto_Completed(ByVal lo As LoadOperation(Of ClientesFondosPensione))
        If Not lo.HasError Then
            ClientesFondosPensionePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClientesFondosPensione por defecto", _
                                             Me.ToString(), "TerminoTraerClientesFondosPensionePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerViewClientesExentoPordefecto_Completed(ByVal lo As LoadOperation(Of ViewClientes_Exento))
        If Not lo.HasError Then
            ViewClientesExentoPordefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClientesExento por defecto", _
                                             Me.ToString(), "TerminoTraerClientesFondosPensionePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesFondosPensiones(ByVal lo As LoadOperation(Of ClientesFondosPensione))
        If Not lo.HasError Then
            ListaClientesFondosPensiones = dcProxy.ClientesFondosPensiones
            If dcProxy.ClientesFondosPensiones.Count > 0 Then
                If lo.UserState = "insert" Then
                    ClientesFondosPensioneSelected = ListaClientesFondosPensiones.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesFondosPensiones", _
                                             Me.ToString(), "TerminoTraerClientesFondosPensione", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub TerminoTraerViewClientesExento(ByVal lo As LoadOperation(Of ViewClientes_Exento))
        If Not lo.HasError Then
            ListaViewClientesExentos = dcProxy.ViewClientes_Exentos
            If dcProxy.ViewClientes_Exentos.Count > 0 Then
                If lo.UserState = "insert" Then
                    ViewClientesExentoSelected = ListaViewClientesExentos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesFondosPensiones", _
                                             Me.ToString(), "TerminoTraerClientesFondosPensione", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
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


    'Tablas padres

#End Region

#Region "Propiedades"
    '2
    Private _ListaClientesFondosPensiones As EntitySet(Of ClientesFondosPensione)
    Public Property ListaClientesFondosPensiones() As EntitySet(Of ClientesFondosPensione)
        Get
            Return _ListaClientesFondosPensiones
        End Get
        Set(ByVal value As EntitySet(Of ClientesFondosPensione))
            _ListaClientesFondosPensiones = value

            MyBase.CambioItem("ListaClientesFondosPensiones")
            MyBase.CambioItem("ListaClientesFondosPensionesPaged")
            If Not IsNothing(value) Then
                If IsNothing(ClientesFondosPensioneAnterior) Then
                    ClientesFondosPensioneSelected = _ListaClientesFondosPensiones.FirstOrDefault
                Else
                    ClientesFondosPensioneSelected = ClientesFondosPensioneAnterior
                End If
            End If
        End Set
    End Property

    '3
    Public ReadOnly Property ListaClientesFondosPensionesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClientesFondosPensiones) Then
                Dim view = New PagedCollectionView(_ListaClientesFondosPensiones)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    '1
    Private _ClientesFondosPensioneSelected As ClientesFondosPensione
    Public Property ClientesFondosPensioneSelected() As ClientesFondosPensione
        Get
            Return _ClientesFondosPensioneSelected
        End Get
        Set(ByVal value As ClientesFondosPensione)
            _ClientesFondosPensioneSelected = value
            MyBase.CambioItem("ClientesFondosPensioneSelected")
        End Set
    End Property

    Private _ViewClientesExentoSelected As ViewClientes_Exento
    Public Property ViewClientesExentoSelected() As ViewClientes_Exento
        Get
            Return _ViewClientesExentoSelected

        End Get
        Set(ByVal value As ViewClientes_Exento)
            _ViewClientesExentoSelected = value
            MyBase.CambioItem("ViewClientesExentoSelected")
        End Set
    End Property

    Public Sub CambioViewClientesExentoSelected()
        MyBase.CambioItem("ViewClientesExentoSelected")
    End Sub

    Private _ListaViewClientesExentos As EntitySet(Of ViewClientes_Exento)
    Public Property ListaViewClientesExentos() As EntitySet(Of ViewClientes_Exento)
        Get
            Return _ListaViewClientesExentos
        End Get
        Set(ByVal value As EntitySet(Of ViewClientes_Exento))
            _ListaViewClientesExentos = value
            MyBase.CambioItem("ListaViewClientesExentos")
            MyBase.CambioItem("ListaViewClientesExentosPaged")
            If Not IsNothing(value) Then
                If IsNothing(ViewClientesExentoAnterior) Then
                    ViewClientesExentoSelected = _ListaViewClientesExentos.FirstOrDefault
                Else
                    ViewClientesExentoSelected = ViewClientesExentoAnterior
                End If
            End If
        End Set
    End Property
    Public ReadOnly Property ListaViewClientesExentosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaViewClientesExentos) Then
                Dim view = New PagedCollectionView(_ListaViewClientesExentos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _Listatabla As List(Of ViewClientes_Exentos_Consultar)
    Public Property Listatabla() As List(Of ViewClientes_Exentos_Consultar)
        Get
            Return _Listatabla
        End Get
        Set(ByVal value As List(Of ViewClientes_Exentos_Consultar))
            _Listatabla = value
            tablaSeleccionada = value.FirstOrDefault
            MyBase.CambioItem("Listatabla")
            MyBase.CambioItem("ListatablaPaged")
        End Set
    End Property
    Private _tablaSeleccionada As ViewClientes_Exentos_Consultar
    Public Property tablaSeleccionada() As ViewClientes_Exentos_Consultar
        Get
            Return _tablaSeleccionada
        End Get
        Set(ByVal value As ViewClientes_Exentos_Consultar)
            _tablaSeleccionada = value
            MyBase.CambioItem("tablaSeleccionada")

        End Set
    End Property
    Public ReadOnly Property ListatablaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_Listatabla) Then
                Dim view = New PagedCollectionView(_Listatabla)
                Return view
            Else
                Return Nothing
            End If
        End Get
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
            Dim NewViewClientes_Exento As New ViewClientes_Exento

            'TODO: Verificar cuales son los campos que deben inicializarse
            NewViewClientes_Exento.Comitente = ViewClientesExentoPordefecto.Comitente
            NewViewClientes_Exento.IDClientesfondospensiones = ViewClientesExentoPordefecto.IDClientesfondospensiones
            NewViewClientes_Exento.NroDocumento = ViewClientesExentoPordefecto.NroDocumento
            'SLB20130605
            NewViewClientes_Exento.Usuario = Program.Usuario
            'ViewClientesExentoAnterior = ViewClientesExentoSelected
            ViewClientesExentoSelected = NewViewClientes_Exento


            MyBase.CambioItem("ClientesFondosPensiones")

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
            dcProxy.ClientesFondosPensiones.Clear()
            dcProxy.ViewClientes_Exentos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.Clientes_ExentosFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerViewClientesExento, "FiltrarVM")
            Else
                dcProxy.Load(dcProxy.Clientes_ExentosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerViewClientesExento, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Comitente <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                ' dcProxy.ClientesFondosPensiones.Clear()
                dcProxy.ViewClientes_Exentos.Clear()
                ' ClientesFondosPensioneAnterior = Nothing
                ViewClientesExentoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Comitente = " &  cb.Comitente.ToString() 
                dcProxy.Load(dcProxy.ViewClientes_ExentoConsultarQuery(cb.Comitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerViewClientesExento, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaClientesFondosPensione
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
            If Not ListaViewClientesExentos.Count = 0 Then
                For Each Lista In ListaViewClientesExentos
                    If Lista.Comitente.TrimEnd = ViewClientesExentoSelected.Comitente Then
                        A2Utilidades.Mensajes.mostrarMensaje("El Código elegido ya ha sido asignado.", "OyD Server", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub

                    End If
                Next
            Else
            End If
            Dim origen = "update"
            ErrorForma = ""

            ViewClientesExentoAnterior = ViewClientesExentoSelected

            If Not ListaViewClientesExentos.Contains(ViewClientesExentoSelected) Then
                origen = "insert"
                ListaViewClientesExentos.Add(ViewClientesExentoSelected)

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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaClientesFondosPensiones.Remove(ClientesFondosPensioneSelected)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.UserState = "BorrarRegistro" Then
                    'dcProxy.UsuariosSucursales.Clear()
                    'dcProxy.Load(dcProxy.UsuariosSucursalesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosSucursales, "RecargaBorradoFallido")
                    dcProxy.RejectChanges()

                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    ' dcProxy.ClientesFondosPensiones.Clear()
            '    dcProxy.ViewClientes_Exentos.Clear()
            '    ' dcProxy.Load(dcProxy.ClientesFondosPensionesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFondosPensiones, "insert") ' Recarga la lista para que carguen los include
            '    dcProxy.Load(dcProxy.Clientes_ExentosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerViewClientesExento, "insert")
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'If Not IsNothing(_ViewClientesExentoSelected) Then
        '    Editando = True
        '    EditaReg = False
        'End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try

            ErrorForma = ""
            If Not IsNothing(_ViewClientesExentoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditaReg = False
                cb = New CamposBusquedaClientesFondosPensione
                CambioItem("cb")
                If _ViewClientesExentoSelected.EntityState = EntityState.Detached Then
                    ViewClientesExentoSelected = ViewClientesExentoAnterior

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
            If Not IsNothing(_ViewClientesExentoSelected) Then
                dcProxy.ViewClientes_Exentos.Remove(_ViewClientesExentoSelected)
                ViewClientesExentoSelected = _ListaViewClientesExentos.LastOrDefault
                IsBusy = True
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
        DicCamposTab.Add("Comitente", 1)
    End Sub

    Public Overrides Sub Buscar()
        cb.Comitente = String.Empty
        MyBase.Buscar()
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaClientesFondosPensione
    Implements INotifyPropertyChanged

    Private _Comitente As String
    <Display(Name:="Comitente")> _
    Public Property Comitente As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




