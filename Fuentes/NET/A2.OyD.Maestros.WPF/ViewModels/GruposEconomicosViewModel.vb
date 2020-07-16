Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: GruposEconomicosViewModel.vb
'Generado el : 03/06/2012 17:14:59
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
Imports System.Text

Public Class GruposEconomicosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaGruposEconomico
    Private GrupoEconomicoPorDefecto As GrupoEconomicos
    Private GrupoEconomicoAnterior As GrupoEconomicos
    Private DetalleGrupoEconomicoPorDefecto As DetalleGrupoEconomico
    Private DetalleGrupoEconomicoAnterior As DetalleGrupoEconomico
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
                dcProxy.Load(dcProxy.GruposEconomicosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposEconomicos, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "GruposEconomicosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerGruposEconomicosPorDefecto_Completed(ByVal lo As LoadOperation(Of GrupoEconomicos))
        If Not lo.HasError Then
            GrupoEconomicoSelected = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la GrupoEconomicos por defecto",
                                             Me.ToString(), "TerminoTraerGruposEconomicosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerGruposEconomicos(ByVal lo As LoadOperation(Of GrupoEconomicos))
        If Not lo.HasError Then
            ListaGruposEconomicos = dcProxy.GrupoEconomicos
            If dcProxy.GrupoEconomicos.Count > 0 Then
                If lo.UserState = "insert" Then
                    GrupoEconomicoSelected = ListaGruposEconomicos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    'MessageBox.Show("No se encontró ningún registro") Se comenta esta linea para que el mensaje sea mostrado en un control diferente a el messagebox
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de GruposEconomicos",
                                             Me.ToString(), "TerminoTraerClienteAgrupado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub


#Region "Resultados Asincrónicos Tabla Hija"

    Private Sub TerminoTraerDetalleGruposEconomicosPorDefecto_Completed(ByVal lo As LoadOperation(Of DetalleGrupoEconomico))
        If Not lo.HasError Then
            DetalleGrupoEconomicoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la DetalleGrupoEconomico por defecto",
                                             Me.ToString(), "TerminoTraerDetalleClienteAgrupadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDetalleGruposEconomicos(ByVal lo As LoadOperation(Of DetalleGrupoEconomico))
        If Not lo.HasError Then
            ListaDetalleGruposEconomicos = dcProxy.DetalleGrupoEconomicos.ToList
            If dcProxy.DetalleClienteAgrupados.Count > 0 Then
                If lo.UserState = "insert" Then
                    DetalleGrupoEconomicoSelected = ListaDetalleGruposEconomicos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    MessageBox.Show("No se encontró ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleGruposEconomicos",
                                             Me.ToString(), "TerminoTraerDetalleClienteAgrupado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres

#End Region
    'Tablas padres

#End Region

#Region "Propiedades"

    Private _ListaGruposEconomicos As EntitySet(Of GrupoEconomicos)
    Public Property ListaGruposEconomicos() As EntitySet(Of GrupoEconomicos)
        Get
            Return _ListaGruposEconomicos
        End Get
        Set(ByVal value As EntitySet(Of GrupoEconomicos))
            _ListaGruposEconomicos = value

            MyBase.CambioItem("ListaGruposEconomicos")
            MyBase.CambioItem("ListaGruposEconomicosPaged")
            If Not IsNothing(value) Then
                If IsNothing(GrupoEconomicoAnterior) Then
                    GrupoEconomicoSelected = _ListaGruposEconomicos.FirstOrDefault
                Else
                    GrupoEconomicoSelected = GrupoEconomicoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaGruposEconomicosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaGruposEconomicos) Then
                Dim view = New PagedCollectionView(_ListaGruposEconomicos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Dim loDGE As LoadOperation(Of DetalleGrupoEconomico)

    Private _GrupoEconomicoSelected As GrupoEconomicos
    Public Property GrupoEconomicoSelected() As GrupoEconomicos
        Get
            Return _GrupoEconomicoSelected
        End Get
        Set(ByVal value As GrupoEconomicos)
            _GrupoEconomicoSelected = value
            If Not value Is Nothing Then

                If Not IsNothing(loDGE) Then
                    If Not loDGE.IsComplete Then
                        loDGE.Cancel()
                    End If
                End If

                dcProxy.DetalleGrupoEconomicos.Clear()
                loDGE = dcProxy.Load(dcProxy.DetalleGruposEconomicosConsultarQuery(_GrupoEconomicoSelected.IdGrupoEconomico, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleGruposEconomicos, Nothing)

            End If
            MyBase.CambioItem("GrupoEconomicoSelected")

        End Set
    End Property

#Region "Propiedades de las Tablas Hijas"

    Private _ListaDetalleGruposEconomicos As List(Of DetalleGrupoEconomico)
    Public Property ListaDetalleGruposEconomicos() As List(Of DetalleGrupoEconomico)
        Get
            Return _ListaDetalleGruposEconomicos
        End Get
        Set(ByVal value As List(Of DetalleGrupoEconomico))
            _ListaDetalleGruposEconomicos = value
            MyBase.CambioItem("ListaDetalleGruposEconomicos")
        End Set
    End Property

    Private WithEvents _DetalleGrupoEconomicoSelected As DetalleGrupoEconomico
    Public Property DetalleGrupoEconomicoSelected() As DetalleGrupoEconomico
        Get
            Return _DetalleGrupoEconomicoSelected
        End Get
        Set(ByVal value As DetalleGrupoEconomico)
            _DetalleGrupoEconomicoSelected = value
            MyBase.CambioItem("DetalleGrupoEconomicoSelected")
        End Set
    End Property

    Private _habilitarClienteLider As Boolean = False
    Public Property habilitarClienteLider() As Boolean
        Get
            Return _habilitarClienteLider
        End Get
        Set(ByVal value As Boolean)
            _habilitarClienteLider = value
            MyBase.CambioItem("habilitarClienteLider")
        End Set
    End Property


#End Region

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try

            Dim NewGrupoEconomico As New GrupoEconomicos
            NewGrupoEconomico.Usuario = Program.Usuario

            GrupoEconomicoAnterior = GrupoEconomicoSelected
            GrupoEconomicoSelected = NewGrupoEconomico
            MyBase.CambioItem("ListaGruposEconomicos")
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
            dcProxy.GrupoEconomicos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.GruposEconomicosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposEconomicos, Nothing)
            Else
                dcProxy.Load(dcProxy.GruposEconomicosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposEconomicos, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaGruposEconomico
        cb.idComitenteLider = Nothing
        cb.NombreGrupo = Nothing
        cb.NroGrupo = Nothing
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.NroGrupo <> 0 Or cb.NombreGrupo <> String.Empty Or cb.idComitenteLider <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.GrupoEconomicos.Clear()
                GrupoEconomicoAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.GruposEconomicosConsultarQuery(cb.NroGrupo, cb.NombreGrupo, cb.idComitenteLider, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposEconomicos, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaGruposEconomico
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
            If GrupoEconomicoSelected.ComitenteLider = Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("El grupo económico debe tener asignado un lider", "ActualizarRegistro", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            GrupoEconomicoAnterior = GrupoEconomicoSelected
            GrupoEconomicoSelected.Usuario = Program.Usuario
            If dcProxy.GrupoEconomicos.Where(Function(i) i.IdGrupoEconomico = _GrupoEconomicoSelected.IdGrupoEconomico).Count > 0 Then
                GrupoEconomicoSelected.IdsComitentes = ListaDetalleGruposEconomicos.Select(Function(x) x.IdComitente).Aggregate(Function(a, b) (a + "," + b))
            End If


            If Not ListaGruposEconomicos.Contains(GrupoEconomicoSelected) Then
                ''JCM20191018
                If GrupoEconomicoSelected.IdsComitentes Is Nothing And ListaDetalleGruposEconomicos.Count > 0 Then
                    GrupoEconomicoSelected.IdsComitentes = ListaDetalleGruposEconomicos.Select(Function(x) x.IdComitente).Aggregate(Function(a, b) (a + "," + b))
                End If
                ''JCM20191018
                origen = "insert"
                ListaGruposEconomicos.Add(GrupoEconomicoSelected)
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

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState <> "insert" And So.UserState <> "update" Then
                    dcProxy.GrupoEconomicos.Clear()
                    dcProxy.Load(dcProxy.GruposEconomicosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposEconomicos, So.UserState)
                Else

                    Dim list As List(Of DetalleGrupoEconomico) = ListaDetalleGruposEconomicos.Where(Function(i) i.Nombre.Length > 0).ToList
                    ListaDetalleGruposEconomicos = list
                End If
            End If
            MyBase.TerminoSubmitChanges(So)
            habilitarClienteLider = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_GrupoEconomicoSelected) Then
            Editando = True
            habilitarClienteLider = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_GrupoEconomicoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _GrupoEconomicoSelected.EntityState = EntityState.Detached Then
                    GrupoEconomicoSelected = GrupoEconomicoAnterior
                End If
            End If
            habilitarClienteLider = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_GrupoEconomicoSelected) Then
                If dcProxy.GrupoEconomicos.Where(Function(i) i.IdGrupoEconomico = _GrupoEconomicoSelected.IdGrupoEconomico).Count > 0 Then
                    dcProxy.GrupoEconomicos.Remove(dcProxy.GrupoEconomicos.Where(Function(i) i.IdGrupoEconomico = _GrupoEconomicoSelected.IdGrupoEconomico).First)
                End If

                If _ListaGruposEconomicos.Count > 0 Then
                    GrupoEconomicoSelected = _ListaGruposEconomicos.LastOrDefault
                Else
                    GrupoEconomicoSelected = Nothing
                End If

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

    Private Sub _DetalleGrupoEconomicoSelected_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleGrupoEconomicoSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "clientelider"
                    If Not IsNothing(_DetalleGrupoEconomicoSelected.ClienteLider) Then
                        If _DetalleGrupoEconomicoSelected.ClienteLider Then
                            Dim lider As String = _DetalleGrupoEconomicoSelected.IdComitente
                            GrupoEconomicoSelected.ComitenteLider = _DetalleGrupoEconomicoSelected.IdComitente
                            GrupoEconomicoSelected.NombreLider = _DetalleGrupoEconomicoSelected.Nombre
                            'ListaDetalleGruposEconomicos.Where(Function(i) i.IdComitente <> _DetalleGrupoEconomicoSelected.IdComitente).ToList().ForEach(Function(dg) dg.ClienteLider = False)
                            For Each dt As DetalleGrupoEconomico In ListaDetalleGruposEconomicos
                                If dt.IdComitente <> lider Then
                                    dt.ClienteLider = False
                                End If
                            Next
                        Else
                            GrupoEconomicoSelected.ComitenteLider = Nothing
                            GrupoEconomicoSelected.NombreLider = Nothing
                        End If
                        MyBase.CambioItem("ListaDetalleGruposEconomicos")
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenesLEOSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub


#End Region

#Region "Métodos Tablas Hijas"
    Public Overrides Sub NuevoRegistroDetalle()

        Select Case NombreColeccionDetalle
            Case "cmDetalleGrupoEconomico"

                Dim NewDetallegrupoEconomico As New DetalleGrupoEconomico
                NewDetallegrupoEconomico.IdComitente = Nothing
                NewDetallegrupoEconomico.Nombre = String.Empty
                NewDetallegrupoEconomico.DireccionEnvio = String.Empty
                NewDetallegrupoEconomico.NroDocumento = Nothing
                NewDetallegrupoEconomico.IDReceptor = String.Empty
                NewDetallegrupoEconomico.IDSucCliente = String.Empty
                habilitarClienteLider = True

                Dim objListaDetalleGruposEconomicos As New List(Of DetalleGrupoEconomico)

                If Not IsNothing(ListaDetalleGruposEconomicos) Then
                    For Each li In ListaDetalleGruposEconomicos
                        objListaDetalleGruposEconomicos.Add(li)
                    Next
                End If

                objListaDetalleGruposEconomicos.Add(NewDetallegrupoEconomico)

                ListaDetalleGruposEconomicos = objListaDetalleGruposEconomicos
                DetalleGrupoEconomicoSelected = NewDetallegrupoEconomico
                MyBase.CambioItem("ListaDetalleGruposEconomicos")
                MyBase.CambioItem("DetalleGrupoEconomicoSelected")
                MyBase.CambioItem("Editando")

        End Select
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        If NombreColeccionDetalle = "cmDetalleGrupoEconomico" Then
            If Not IsNothing(ListaDetalleGruposEconomicos) Then
                If Not IsNothing(DetalleGrupoEconomicoSelected) Then
                    Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DetalleGrupoEconomicoSelected, ListaDetalleGruposEconomicos)
                    Dim objListaDetalleGruposEconomicos As New List(Of DetalleGrupoEconomico)

                    For Each li In _ListaDetalleGruposEconomicos
                        objListaDetalleGruposEconomicos.Add(li)
                    Next

                    objListaDetalleGruposEconomicos.Remove(DetalleGrupoEconomicoSelected)

                    ListaDetalleGruposEconomicos = objListaDetalleGruposEconomicos

                    Program.PosicionarItemLista(DetalleGrupoEconomicoSelected, ListaDetalleGruposEconomicos, intRegistroPosicionar)
                    MyBase.CambioItem("DetalleGrupoEconomicoSelected")
                    MyBase.CambioItem("ListaDetalleGruposEconomicos")
                End If
            End If
        End If
    End Sub

#End Region

    Sub validarNuevoDetalle(ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        Dim dtll As DetalleGrupoEconomico = ListaDetalleGruposEconomicos.Where(Function(x) x.IdComitente = pobjItem.CodigoOYD).ToList.FirstOrDefault
        If Not IsNothing(dtll) Then
            A2Utilidades.Mensajes.mostrarMensaje("El cliente " & dtll.Nombre & ", ya pertenece a este grupo económico.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            DetalleGrupoEconomicoSelected.IdComitente = pobjItem.CodigoOYD
            DetalleGrupoEconomicoSelected.Nombre = pobjItem.Nombre
            DetalleGrupoEconomicoSelected.DireccionEnvio = pobjItem.DireccionEnvio
            DetalleGrupoEconomicoSelected.IDReceptor = pobjItem.CodReceptorLider
            DetalleGrupoEconomicoSelected.IDSucCliente = pobjItem.SucursalFiducia
            DetalleGrupoEconomicoSelected.NroDocumento = pobjItem.NroDocumento
            habilitarClienteLider = True
        End If
    End Sub

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaGruposEconomico
    Implements INotifyPropertyChanged

    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")>
    <Display(Name:="Nro. Grupo")>
    Private _NroGrupo As Integer
    Public Property NroGrupo As Integer
        Get
            Return _NroGrupo
        End Get
        Set(ByVal value As Integer)
            _NroGrupo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroGrupo"))
        End Set
    End Property

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Nombre Grupo Económico")>
    Private _NombreGrupo As String
    Public Property NombreGrupo As String
        Get
            Return _NombreGrupo
        End Get
        Set(ByVal value As String)
            _NombreGrupo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreGrupo"))
        End Set
    End Property

    <StringLength(50, ErrorMessage:="La longitud máxima es de 17")>
    <Display(Name:="Comitente líder")>
    Private _idComitenteLider As String
    Public Property idComitenteLider As String
        Get
            Return _idComitenteLider
        End Get
        Set(ByVal value As String)
            _idComitenteLider = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("idComitenteLider"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class




