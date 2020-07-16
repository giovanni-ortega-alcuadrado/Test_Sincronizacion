Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: Codigos_CIIUViewModel.vb
'Generado el : 01/24/2011 16:36:55
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web

Public Class Codigos_CIIUViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCodigos_CII
    Private Codigos_CIIPorDefecto As Codigos_CII
    Private Codigos_CIIAnterior As Codigos_CII
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
                dcProxy.Load(dcProxy.Codigos_CIIUFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigos_CIIU, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCodigos_CIIPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigos_CIIUPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  Codigos_CIIUViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Codigos_CIIUViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCodigos_CIIUPorDefecto_Completed(ByVal lo As LoadOperation(Of Codigos_CII))
        If Not lo.HasError Then
            Codigos_CIIPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Codigos_CII por defecto", _
                                             Me.ToString(), "TerminoTraerCodigos_CIIPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCodigos_CIIU(ByVal lo As LoadOperation(Of Codigos_CII))
        If Not lo.HasError Then
            ListaCodigos_CIIU = dcProxy.Codigos_CIIs
            If dcProxy.Codigos_CIIs.Count > 0 Then
                If lo.UserState = "insert" Then
                    Codigos_CIISelected = ListaCodigos_CIIU.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Codigos_CIIU", _
                                             Me.ToString(), "TerminoTraerCodigos_CII", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaCodigos_CIIU As EntitySet(Of Codigos_CII)
    Public Property ListaCodigos_CIIU() As EntitySet(Of Codigos_CII)
        Get
            Return _ListaCodigos_CIIU
        End Get
        Set(ByVal value As EntitySet(Of Codigos_CII))
            _ListaCodigos_CIIU = value

            MyBase.CambioItem("ListaCodigos_CIIU")
            MyBase.CambioItem("ListaCodigos_CIIUPaged")
            If Not IsNothing(value) Then
                If IsNothing(Codigos_CIIAnterior) Then
                    Codigos_CIISelected = _ListaCodigos_CIIU.FirstOrDefault
                Else
                    Codigos_CIISelected = Codigos_CIIAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCodigos_CIIUPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCodigos_CIIU) Then
                Dim view = New PagedCollectionView(_ListaCodigos_CIIU)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _Codigos_CIISelected As Codigos_CII
    Public Property Codigos_CIISelected() As Codigos_CII
        Get
            Return _Codigos_CIISelected
        End Get
        Set(ByVal value As Codigos_CII)
            _Codigos_CIISelected = value
            'If Not value Is Nothing Then
            '    End If
            MyBase.CambioItem("Codigos_CIISelected")
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
            Dim NewCodigos_CII As New Codigos_CII
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCodigos_CII.Codigo = Codigos_CIIPorDefecto.Codigo
            NewCodigos_CII.Descripcion = Codigos_CIIPorDefecto.Descripcion
            NewCodigos_CII.Actualizacion = Codigos_CIIPorDefecto.Actualizacion
            NewCodigos_CII.Usuario = Program.Usuario
            NewCodigos_CII.IDCodigoCIIU = Codigos_CIIPorDefecto.IDCodigoCIIU
            NewCodigos_CII.TasaRteCREE = 0
            Codigos_CIIAnterior = Codigos_CIISelected
            Codigos_CIISelected = NewCodigos_CII
            MyBase.CambioItem("Codigos_CIIU")
            Editando = True
            MyBase.CambioItem("Editando")
            habilitar = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Codigos_CIIs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.Codigos_CIIUFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigos_CIIU, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.Codigos_CIIUFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigos_CIIU, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Codigo <> 0 Or cb.Descripcion <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Codigos_CIIs.Clear()
                Codigos_CIIAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Codigo = " &  cb.Codigo.ToString() & " Descripcion = " &  cb.Descripcion.ToString() 
                dcProxy.Load(dcProxy.Codigos_CIIUConsultarQuery(cb.Codigo, cb.Descripcion,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigos_CIIU, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCodigos_CII
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
            If IsNothing(Codigos_CIISelected.TasaRteCREE) Then
                Codigos_CIISelected.TasaRteCREE = 0
            End If
            'Codigos_CIIAnterior = Codigos_CIISelected
            If Not ListaCodigos_CIIU.Contains(Codigos_CIISelected) Then
                origen = "insert"
                ListaCodigos_CIIU.Add(Codigos_CIISelected)
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

                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaCodigos_CIIU.Remove(Codigos_CIISelected)
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
            '    dcProxy.Codigos_CIIs.Clear()
            '    dcProxy.Load(dcProxy.Codigos_CIIUFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigos_CIIU, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_Codigos_CIISelected) Then
            Editando = True
            habilitar = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_Codigos_CIISelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _Codigos_CIISelected.EntityState = EntityState.Detached Then
                    Codigos_CIISelected = Codigos_CIIAnterior
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
            If Not IsNothing(_Codigos_CIISelected) Then
                dcProxy.Codigos_CIIs.Remove(_Codigos_CIISelected)
                Codigos_CIISelected = _ListaCodigos_CIIU.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
            habilitar = False
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
            cb = New CamposBusquedaCodigos_CII
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
Public Class CamposBusquedaCodigos_CII

    <StringLength(10, ErrorMessage:="La longitud máxima es de 10")> _
     <Display(Name:="Código")> _
    Public Property Codigo As String

    <StringLength(255, ErrorMessage:="La longitud máxima es de 255")> _
     <Display(Name:="Nombre")> _
    Public Property Descripcion As String
End Class




