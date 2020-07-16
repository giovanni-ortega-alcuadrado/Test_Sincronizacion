Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: TipoPersonaPorDctoViewModel.vb
'Generado el : 04/19/2011 16:13:37
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

Public Class TipoPersonaPorDctoViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaTipoPersonaPorDct
	Private TipoPersonaPorDctPorDefecto As TipoPersonaPorDct
	Private TipoPersonaPorDctAnterior As TipoPersonaPorDct
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext

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
                dcProxy.Load(dcProxy.TipoPersonaPorDctoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoPersonaPorDcto, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerTipoPersonaPorDctPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoPersonaPorDctoPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  TipoPersonaPorDctoViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "TipoPersonaPorDctoViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerTipoPersonaPorDctoPorDefecto_Completed(ByVal lo As LoadOperation(Of TipoPersonaPorDct))
        If Not lo.HasError Then
            TipoPersonaPorDctPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoPersonaPorDct por defecto", _
                                             Me.ToString(), "TerminoTraerTipoPersonaPorDctPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerTipoPersonaPorDcto(ByVal lo As LoadOperation(Of TipoPersonaPorDct))
        If Not lo.HasError Then
            ListaTipoPersonaPorDcto = dcProxy.TipoPersonaPorDcts
            If dcProxy.TipoPersonaPorDcts.Count > 0 Then
                If lo.UserState = "insert" Then
                    TipoPersonaPorDctSelected = ListaTipoPersonaPorDcto.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoPersonaPorDcto", _
                                             Me.ToString(), "TerminoTraerTipoPersonaPorDct", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaTipoPersonaPorDcto As EntitySet(Of TipoPersonaPorDct)
    Public Property ListaTipoPersonaPorDcto() As EntitySet(Of TipoPersonaPorDct)
        Get
            Return _ListaTipoPersonaPorDcto
        End Get
        Set(ByVal value As EntitySet(Of TipoPersonaPorDct))
            _ListaTipoPersonaPorDcto = value

            MyBase.CambioItem("ListaTipoPersonaPorDcto")
            MyBase.CambioItem("ListaTipoPersonaPorDctoPaged")
            If Not IsNothing(value) Then
                If IsNothing(TipoPersonaPorDctAnterior) Then
                    TipoPersonaPorDctSelected = _ListaTipoPersonaPorDcto.FirstOrDefault
                Else
                    TipoPersonaPorDctSelected = TipoPersonaPorDctAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaTipoPersonaPorDctoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoPersonaPorDcto) Then
                Dim view = New PagedCollectionView(_ListaTipoPersonaPorDcto)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _TipoPersonaPorDctSelected As TipoPersonaPorDct
    Public Property TipoPersonaPorDctSelected() As TipoPersonaPorDct
        Get
            Return _TipoPersonaPorDctSelected
        End Get
        Set(ByVal value As TipoPersonaPorDct)
            _TipoPersonaPorDctSelected = value
            MyBase.CambioItem("TipoPersonaPorDctSelected")
        End Set
    End Property


    Private _chkMenorEdadVisible As Visibility
    Public Property chkMenorEdadVisible() As Visibility
        Get
            Return _chkMenorEdadVisible
        End Get
        Set(ByVal value As Visibility)
            _chkMenorEdadVisible = value
            MyBase.CambioItem("chkMenorEdadVisible")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '      Try
        '          Dim NewTipoPersonaPorDct As New TipoPersonaPorDct
        '	'TODO: Verificar cuales son los campos que deben inicializarse
        'NewTipoPersonaPorDct.ID = TipoPersonaPorDctPorDefecto.ID
        'NewTipoPersonaPorDct.TipoIdentificacion = TipoPersonaPorDctPorDefecto.TipoIdentificacion
        'NewTipoPersonaPorDct.IDTipoPersona = TipoPersonaPorDctPorDefecto.IDTipoPersona
        'NewTipoPersonaPorDct.menored = TipoPersonaPorDctPorDefecto.menored
        '      TipoPersonaPorDctAnterior = TipoPersonaPorDctSelected
        '      TipoPersonaPorDctSelected = NewTipoPersonaPorDct
        '      MyBase.CambioItem("TipoPersonaPorDcto")
        '      Editando = True
        '      MyBase.CambioItem("Editando")
        '  Catch ex As Exception
        'IsBusy = False
        '      A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
        '                                                   Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '  End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.TipoPersonaPorDcts.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.TipoPersonaPorDctoFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoPersonaPorDcto, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.TipoPersonaPorDctoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoPersonaPorDcto, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            TipoPersonaPorDctAnterior = TipoPersonaPorDctSelected
            If Not ListaTipoPersonaPorDcto.Contains(TipoPersonaPorDctSelected) Then
                origen = "insert"
                ListaTipoPersonaPorDcto.Add(TipoPersonaPorDctSelected)
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
            End If

            'If So.UserState = "insert" Then
            '    dcProxy.TipoPersonaPorDcts.Clear()
            '    dcProxy.Load(dcProxy.TipoPersonaPorDctoFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoPersonaPorDcto, "insert") ' Recarga la lista para que carguen los include
            'End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_TipoPersonaPorDctSelected) Then
            Editando = True
            If TipoPersonaPorDctSelected.IDTipoPersona = 1 Then
                chkMenorEdadVisible = Visibility.Visible
            Else
                chkMenorEdadVisible = Visibility.Collapsed
            End If
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_TipoPersonaPorDctSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _TipoPersonaPorDctSelected.EntityState = EntityState.Detached Then
                    TipoPersonaPorDctSelected = TipoPersonaPorDctAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'Try
        '    If Not IsNothing(_TipoPersonaPorDctSelected) Then
        '        dcProxy.TipoPersonaPorDcts.Remove(_TipoPersonaPorDctSelected)
        '        TipoPersonaPorDctSelected = _ListaTipoPersonaPorDcto.LastOrDefault
        '        IsBusy = True
        '        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        '    End If
        'Catch ex As Exception
        '    IsBusy = False
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '     Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        'End Try
    End Sub

    Public Overrides Sub Buscar()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub

    'Private Sub _TipoPersonaPorDctSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _TipoPersonaPorDctSelected.PropertyChanged
    '    If Editando Then
    '        If TipoPersonaPorDctSelected.IDTipoPersona = 1 Then
    '            chkMenorEdadVisible = Visibility.Visible
    '        Else
    '            chkMenorEdadVisible = Visibility.Collapsed
    '        End If
    '    End If
    'End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaTipoPersonaPorDct
End Class




