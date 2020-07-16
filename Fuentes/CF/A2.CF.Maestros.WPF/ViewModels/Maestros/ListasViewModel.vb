Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ListasViewModel.vb
'Generado el : 01/27/2011 09:30:33
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
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros

Public Class ListasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaLista
    Private ListaPorDefecto As Lista
    Private ListaAnterior As Lista

    Dim dcProxy As MaestrosCFDomainContext
    Dim dcProxy1 As MaestrosCFDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosCFDomainContext()
            dcProxy1 = New MaestrosCFDomainContext()
        Else
            dcProxy = New MaestrosCFDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosCFDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ListasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerListaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListasPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ListasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ListasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerListasPorDefecto_Completed(ByVal lo As LoadOperation(Of Lista))
        If Not lo.HasError Then
            ListaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Lista por defecto", _
                                             Me.ToString(), "TerminoTraerListaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerListas(ByVal lo As LoadOperation(Of Lista))
        If Not lo.HasError Then
            ListaListas = dcProxy.Listas
            If dcProxy.Listas.Count > 0 Then
                If lo.UserState = "insert" Then
                    ListaSelected = ListaListas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Listas", _
                                             Me.ToString(), "TerminoTraerLista", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaListas As EntitySet(Of Lista)
    Public Property ListaListas() As EntitySet(Of Lista)
        Get
            Return _ListaListas
        End Get
        Set(ByVal value As EntitySet(Of Lista))
            _ListaListas = value

            MyBase.CambioItem("ListaListas")
            MyBase.CambioItem("ListaListasPaged")
            If Not IsNothing(value) Then
                If IsNothing(ListaAnterior) Then
                    ListaSelected = _ListaListas.FirstOrDefault
                Else
                    ListaSelected = ListaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaListasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaListas) Then
                Dim view = New PagedCollectionView(_ListaListas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaSelected As Lista
    Public Property ListaSelected() As Lista
        Get
            Return _ListaSelected
        End Get
        Set(ByVal value As Lista)
            _ListaSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("ListaSelected")
        End Set
    End Property

    Private _Editareg As Boolean
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("Editareg")
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


    Private _HabilitarActivo As Boolean
    Public Property HabilitarActivo As Boolean
        Get
            Return _HabilitarActivo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarActivo = value
            MyBase.CambioItem("HabilitarActivo")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewLista As New Lista
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewLista.IDComisionista = ListaPorDefecto.IDComisionista
            NewLista.IDSucComisionista = ListaPorDefecto.IDSucComisionista
            NewLista.Topico = ListaPorDefecto.Topico
            NewLista.Descripcion = ListaPorDefecto.Descripcion
            NewLista.Retorno = ListaPorDefecto.Retorno
            NewLista.Actualizacion = ListaPorDefecto.Actualizacion
            NewLista.Usuario = Program.Usuario
            NewLista.Activo = ListaPorDefecto.Activo
            NewLista.IDLista = ListaPorDefecto.IDLista
            NewLista.Comentario = ListaPorDefecto.Comentario
            ListaAnterior = ListaSelected
            ListaSelected = NewLista
            MyBase.CambioItem("Listas")
            Editando = True
            Editareg = True
            HabilitarActivo = False
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Listas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ListasFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ListasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Topico <> String.Empty Or cb.Descripcion <> String.Empty Or cb.Retorno <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Listas.Clear()
                ListaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Topico = " &  cb.Topico.ToString() & " Descripcion = " &  cb.Descripcion.ToString() & " Retorno = " &  cb.Retorno.ToString() 
                dcProxy.Load(dcProxy.ListasConsultarQuery(cb.Topico, cb.Descripcion, cb.Retorno, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaLista
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
            'ListaAnterior = ListaSelected
            If Not ListaListas.Contains(ListaSelected) Then
                origen = "insert"
                ListaListas.Add(ListaSelected)
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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And (So.UserState = "insert" Or So.UserState = "update") Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    If So.UserState = "insert" Then
                        ListaListas.Remove(ListaSelected)
                    End If
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
            '    dcProxy.Listas.Clear()
            '    dcProxy.Load(dcProxy.ListasFiltrarQuery(""), AddressOf TerminoTraerListas, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ListaSelected) Then
            HabilitarActivo = True
            Editando = True
            Editareg = False
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ListaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                If _ListaSelected.EntityState = EntityState.Detached Then
                    ListaSelected = ListaAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaLista
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '  Try
        '      If Not IsNothing(_ListaSelected) Then
        '              dcProxy.Listas.Remove(_ListaSelected)
        '              ListaSelected = _ListaListas.LastOrDefault
        '          IsBusy = True
        '              dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        '      End If
        '  Catch ex As Exception
        'IsBusy = False
        '      A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '       Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '  End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("Topico", 1)
        DicCamposTab.Add("Retorno", 1)
        DicCamposTab.Add("Descripcion", 1)
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaLista

    <StringLength(20, ErrorMessage:="La longitud máxima es de 20")> _
     <Display(Name:="Tópico")> _
    Public Property Topico As String

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Descripcion As String

    <StringLength(80, ErrorMessage:="La longitud máxima es de 80")> _
     <Display(Name:="Retorno")> _
    Public Property Retorno As String
End Class




