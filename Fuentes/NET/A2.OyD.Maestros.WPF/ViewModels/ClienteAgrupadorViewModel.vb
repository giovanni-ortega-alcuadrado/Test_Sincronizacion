Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClienteAgrupadorViewModel.vb
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

Public Class ClienteAgrupadorViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaClienteAgrupado
	Private ClienteAgrupadoPorDefecto As ClienteAgrupado
    Private ClienteAgrupadoAnterior As ClienteAgrupado
    Private DetalleClienteAgrupadoPorDefecto As DetalleClienteAgrupado
    Private DetalleClienteAgrupadoAnterior As DetalleClienteAgrupado
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
                dcProxy.Load(dcProxy.ClienteAgrupadorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteAgrupador, "")
                dcProxy1.Load(dcProxy1.TraerClienteAgrupadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteAgrupadorPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ClienteAgrupadorViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClienteAgrupadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClienteAgrupadorPorDefecto_Completed(ByVal lo As LoadOperation(Of ClienteAgrupado))
        If Not lo.HasError Then
            ClienteAgrupadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClienteAgrupado por defecto", _
                                             Me.ToString(), "TerminoTraerClienteAgrupadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClienteAgrupador(ByVal lo As LoadOperation(Of ClienteAgrupado))
        If Not lo.HasError Then
            ListaClienteAgrupador = dcProxy.ClienteAgrupados
            If dcProxy.ClienteAgrupados.Count > 0 Then
                If lo.UserState = "insert" Then
                    ClienteAgrupadoSelected = ListaClienteAgrupador.Last
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
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de ClienteAgrupador", Me.ToString, "TerminoTraerClienteAgrupado", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClienteAgrupador", _
            '                                 Me.ToString(), "TerminoTraerClienteAgrupado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub


#Region "Resultados Asincrónicos Tabla Hija"

    Private Sub TerminoTraerDetalleClienteAgrupadorPorDefecto_Completed(ByVal lo As LoadOperation(Of DetalleClienteAgrupado))
        If Not lo.HasError Then
            DetalleClienteAgrupadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la DetalleClienteAgrupado por defecto", _
                                             Me.ToString(), "TerminoTraerDetalleClienteAgrupadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDetalleClienteAgrupador(ByVal lo As LoadOperation(Of DetalleClienteAgrupado))
        If Not lo.HasError Then
            ListaDetalleClienteAgrupador = dcProxy.DetalleClienteAgrupados
            If dcProxy.DetalleClienteAgrupados.Count > 0 Then
                If lo.UserState = "insert" Then
                    DetalleClienteAgrupadoSelected = ListaDetalleClienteAgrupador.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    MessageBox.Show("No se encontró ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleClienteAgrupador", _
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

    Private _ListaClienteAgrupador As EntitySet(Of ClienteAgrupado)
    Public Property ListaClienteAgrupador() As EntitySet(Of ClienteAgrupado)
        Get
            Return _ListaClienteAgrupador
        End Get
        Set(ByVal value As EntitySet(Of ClienteAgrupado))
            _ListaClienteAgrupador = value

            MyBase.CambioItem("ListaClienteAgrupador")
            MyBase.CambioItem("ListaClienteAgrupadorPaged")
            If Not IsNothing(value) Then
                If IsNothing(ClienteAgrupadoAnterior) Then
                    ClienteAgrupadoSelected = _ListaClienteAgrupador.FirstOrDefault
                Else
                    ClienteAgrupadoSelected = ClienteAgrupadoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClienteAgrupadorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClienteAgrupador) Then
                Dim view = New PagedCollectionView(_ListaClienteAgrupador)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ClienteAgrupadoSelected As ClienteAgrupado
    Public Property ClienteAgrupadoSelected() As ClienteAgrupado
        Get
            Return _ClienteAgrupadoSelected
        End Get
        Set(ByVal value As ClienteAgrupado)
            _ClienteAgrupadoSelected = value
            If Not value Is Nothing Then
                dcProxy.DetalleClienteAgrupados.Clear()
                dcProxy.Load(dcProxy.DetalleClienteAgrupadorConsultarQuery(0, ClienteAgrupadoSelected.NroDocumento, String.Empty, String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleClienteAgrupador, Nothing)
            End If
            MyBase.CambioItem("ClienteAgrupadoSelected")

        End Set
    End Property

    Private _HabilitarBuscador As Boolean = False
    Public Property HabilitarBuscador() As Boolean
        Get
            Return _HabilitarBuscador
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBuscador = value
            MyBase.CambioItem("HabilitarBuscador")
        End Set
    End Property

#Region "Propiedades de las Tablas Hijas"

    Private _ListaDetalleClienteAgrupador As EntitySet(Of DetalleClienteAgrupado)
    Public Property ListaDetalleClienteAgrupador() As EntitySet(Of DetalleClienteAgrupado)
        Get
            Return _ListaDetalleClienteAgrupador
        End Get
        Set(ByVal value As EntitySet(Of DetalleClienteAgrupado))
            _ListaDetalleClienteAgrupador = value
            If Not IsNothing(value) Then
                If IsNothing(DetalleClienteAgrupadoAnterior) Then
                    DetalleClienteAgrupadoSelected = _ListaDetalleClienteAgrupador.FirstOrDefault
                Else
                    DetalleClienteAgrupadoSelected = DetalleClienteAgrupadoAnterior
                End If
            End If
            MyBase.CambioItem("ListaDetalleClienteAgrupador")
            MyBase.CambioItem("ListaDetalleClienteAgrupadorPaged")
        End Set
    End Property

    Public ReadOnly Property ListaDetalleClienteAgrupadorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleClienteAgrupador) Then
                Dim view = New PagedCollectionView(_ListaDetalleClienteAgrupador)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DetalleClienteAgrupadoSelected As DetalleClienteAgrupado
    Public Property DetalleClienteAgrupadoSelected() As DetalleClienteAgrupado
        Get
            Return _DetalleClienteAgrupadoSelected
        End Get
        Set(ByVal value As DetalleClienteAgrupado)
            _DetalleClienteAgrupadoSelected = value
            MyBase.CambioItem("DetalleClienteAgrupadoSelected")
        End Set
    End Property

#End Region

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '          Dim NewClienteAgrupado As New ClienteAgrupado
            '	'TODO: Verificar cuales son los campos que deben inicializarse
            'NewClienteAgrupado.idComisionista = ClienteAgrupadoPorDefecto.idComisionista
            'NewClienteAgrupado.idSucComisionista = ClienteAgrupadoPorDefecto.idSucComisionista
            'NewClienteAgrupado.IDAgrupador = ClienteAgrupadoPorDefecto.IDAgrupador
            'NewClienteAgrupado.NroDocumento = ClienteAgrupadoPorDefecto.NroDocumento
            'NewClienteAgrupado.TipoIdentificacion = ClienteAgrupadoPorDefecto.TipoIdentificacion
            'NewClienteAgrupado.idComitenteLider = ClienteAgrupadoPorDefecto.idComitenteLider
            'NewClienteAgrupado.NombreAgrupador = ClienteAgrupadoPorDefecto.NombreAgrupador
            'NewClienteAgrupado.Usuario = Program.Usuario
            'NewClienteAgrupado.Actualizacion = ClienteAgrupadoPorDefecto.Actualizacion
            '      ClienteAgrupadoAnterior = ClienteAgrupadoSelected
            '      ClienteAgrupadoSelected = NewClienteAgrupado
            '      MyBase.CambioItem("ClienteAgrupador")
            '      Editando = True
            '      MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ClienteAgrupados.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ClienteAgrupadorFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteAgrupador, Nothing)
            Else
                dcProxy.Load(dcProxy.ClienteAgrupadorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteAgrupador, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaClienteAgrupado
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.NroDocumento <> String.Empty Or cb.NombreAgrupador <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ClienteAgrupados.Clear()
                ClienteAgrupadoAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.ClienteAgrupadorConsultarQuery(cb.NroDocumento, cb.NombreAgrupador, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteAgrupador, "Busqueda")
                MyBase.ConfirmarBuscar()
                'cb = New CamposBusquedaClienteAgrupado
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
            Dim origen = "update"
            ErrorForma = ""
            HabilitarBuscador = False
            ClienteAgrupadoAnterior = ClienteAgrupadoSelected
            If Not ListaClienteAgrupador.Contains(ClienteAgrupadoSelected) Then
                origen = "insert"
                ListaClienteAgrupador.Add(ClienteAgrupadoSelected)
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
        If Not IsNothing(_ClienteAgrupadoSelected) Then
            Editando = True
            HabilitarBuscador = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ClienteAgrupadoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ClienteAgrupadoSelected.EntityState = EntityState.Detached Then
                    ClienteAgrupadoSelected = ClienteAgrupadoAnterior
                End If
            End If
            HabilitarBuscador = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'If Not IsNothing(_ClienteAgrupadoSelected) Then
            '        dcProxy.ClienteAgrupados.Remove(_ClienteAgrupadoSelected)
            '   ClienteAgrupadoSelected = _ListaClienteAgrupador.LastOrDefault  'Dic202011  nueva
            '    IsBusy = True
            '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Métodos Tablas Hijas"
    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Dim NewDetalleClienteAgrupado As New DetalleClienteAgrupado
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewDetalleClienteAgrupado.Id = DetalleClienteAgrupadoPorDefecto.Id
            NewDetalleClienteAgrupado.Nombre = DetalleClienteAgrupadoPorDefecto.Nombre
            NewDetalleClienteAgrupado.DireccionEnvio = DetalleClienteAgrupadoPorDefecto.DireccionEnvio
            NewDetalleClienteAgrupado.idReceptor = DetalleClienteAgrupadoPorDefecto.idReceptor
            NewDetalleClienteAgrupado.IDSucCliente = DetalleClienteAgrupadoPorDefecto.IDSucCliente
            NewDetalleClienteAgrupado.IDDetalleClienteAgrupador = DetalleClienteAgrupadoPorDefecto.IDDetalleClienteAgrupador
            DetalleClienteAgrupadoAnterior = DetalleClienteAgrupadoSelected
            DetalleClienteAgrupadoSelected = NewDetalleClienteAgrupado
            MyBase.CambioItem("DetalleClienteAgrupador")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            If Not IsNothing(_DetalleClienteAgrupadoSelected) Then
                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DetalleClienteAgrupadoSelected, ListaDetalleClienteAgrupador.ToList)
                dcProxy.DetalleClienteAgrupados.Remove(_DetalleClienteAgrupadoSelected)
                Program.PosicionarItemLista(DetalleClienteAgrupadoSelected, ListaDetalleClienteAgrupador.ToList, intRegistroPosicionar)
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaClienteAgrupado
 	
    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
     <Display(Name:="Nro. Documento")> _
    Public Property NroDocumento As String
 	
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre Agrupador")> _
    Public Property NombreAgrupador As String
End Class




