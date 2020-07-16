Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: DiasNoHabilesViewModel.vb
'Generado el : 04/15/2011 08:42:15
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

Public Class DiasNoHabilesViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaDiasNoHabile
	Private DiasNoHabilePorDefecto As DiasNoHabile
	Private DiasNoHabileAnterior As DiasNoHabile
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
                dcProxy.Load(dcProxy.DiasNoHabilesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDiasNoHabiles, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerDiasNoHabilePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDiasNoHabilesPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  DiasNoHabilesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "DiasNoHabilesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerDiasNoHabilesPorDefecto_Completed(ByVal lo As LoadOperation(Of DiasNoHabile))
        If Not lo.HasError Then
            DiasNoHabilePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la DiasNoHabile por defecto", _
                                             Me.ToString(), "TerminoTraerDiasNoHabilePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDiasNoHabiles(ByVal lo As LoadOperation(Of DiasNoHabile))
        If Not lo.HasError Then
            ListaDiasNoHabiles= dcProxy.DiasNoHabiles
           	If dcProxy.DiasNoHabiles.Count > 0 Then
                If lo.UserState = "insert" Then
                    DiasNoHabileSelected = ListaDiasNoHabiles.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DiasNoHabiles", _
                                             Me.ToString(), "TerminoTraerDiasNoHabile", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaDiasNoHabiles As EntitySet(Of DiasNoHabile)
    Public Property ListaDiasNoHabiles() As EntitySet(Of DiasNoHabile)
        Get
            Return _ListaDiasNoHabiles
        End Get
        Set(ByVal value As EntitySet(Of DiasNoHabile))
            _ListaDiasNoHabiles = value

            MyBase.CambioItem("ListaDiasNoHabiles")
            MyBase.CambioItem("ListaDiasNoHabilesPaged")
            If Not IsNothing(value) Then
                If IsNothing(DiasNoHabileAnterior) Then
                    DiasNoHabileSelected = _ListaDiasNoHabiles.FirstOrDefault
                Else
                    DiasNoHabileSelected = DiasNoHabileAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaDiasNoHabilesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDiasNoHabiles) Then
                Dim view = New PagedCollectionView(_ListaDiasNoHabiles)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DiasNoHabileSelected As DiasNoHabile
    Public Property DiasNoHabileSelected() As DiasNoHabile
        Get
            Return _DiasNoHabileSelected
        End Get
        Set(ByVal value As DiasNoHabile)
            _DiasNoHabileSelected = value
            If Not IsNothing(value) Then
                PropiedadTextoCombos = ""
                _DiasNoHabileSelected.strIdPais = CStr(value.IdPais)
            End If
			MyBase.CambioItem("DiasNoHabileSelected")
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
            Dim NewDiasNoHabile As New DiasNoHabile
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewDiasNoHabile.IDComisionista = DiasNoHabilePorDefecto.IDComisionista
            NewDiasNoHabile.IDSucComisionista = DiasNoHabilePorDefecto.IDSucComisionista
            'NewDiasNoHabile.NoHabil = DiasNoHabilePorDefecto.NoHabil
            NewDiasNoHabile.NoHabil = Nothing
            NewDiasNoHabile.Activo = DiasNoHabilePorDefecto.Activo
            NewDiasNoHabile.Activo = DiasNoHabilePorDefecto.Activo
            NewDiasNoHabile.Inactivo = DiasNoHabilePorDefecto.Inactivo
            NewDiasNoHabile.Actualizacion = DiasNoHabilePorDefecto.Actualizacion
            NewDiasNoHabile.Usuario = Program.Usuario
            'NewDiasNoHabile.IdPais = DiasNoHabilePorDefecto.IdPais
            'NewDiasNoHabile.IdPais = DiasNoHabileSelected.IdPais
            'NewDiasNoHabile.IdPais = -1
            NewDiasNoHabile.IdPais = DiasNoHabilePorDefecto.IdPais
            ' NewDiasNoHabile.strIdPais = "-1"
            PropiedadTextoCombos = String.Empty
            NewDiasNoHabile.IDDiaNoHabil = DiasNoHabilePorDefecto.IDDiaNoHabil
            DiasNoHabileAnterior = DiasNoHabileSelected
            DiasNoHabileSelected = NewDiasNoHabile
            MyBase.CambioItem("DiasNoHabiles")
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
            dcProxy.DiasNoHabiles.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.DiasNoHabilesFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDiasNoHabiles, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.DiasNoHabilesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDiasNoHabiles, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            'If cb.NoHabil = Nothing Then
            '    cb.NoHabil = String.Empty
            'End If
            If (Not IsNothing(cb.NoHabil)) Or (cb.Activo = True) Or (cb.IdPais <> 0) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = String.Empty
                dcProxy.DiasNoHabiles.Clear()
                DiasNoHabileAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " NoHabil = " &  cb.NoHabil.ToString() & " Activo = " &  cb.Activo.ToString() 
                dcProxy.Load(dcProxy.DiasNoHabilesConsultarQuery(cb.NoHabil, cb.Activo, cb.IdPais,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDiasNoHabiles, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaDiasNoHabile
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
            If DiasNoHabileSelected.NoHabil.Value.Year < 1900 Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha del día no habil no puede ser menor al año de 1900", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If Not DiasNoHabileSelected.strIdPais = "-1" Then
                DiasNoHabileSelected.IdPais = CInt(DiasNoHabileSelected.strIdPais)
            End If

            DiasNoHabileSelected.Usuario = Program.Usuario

            'DiasNoHabileSelected.NoHabil = DiasNoHabileSelected.NoHabil.Date
            DiasNoHabileAnterior = DiasNoHabileSelected
            If Not ListaDiasNoHabiles.Contains(DiasNoHabileSelected) Then
                origen = "insert"
                ListaDiasNoHabiles.Add(DiasNoHabileSelected)
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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaDiasNoHabiles.Remove(DiasNoHabileSelected)
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

            Else
                MyBase.TerminoSubmitChanges(So)

                'dcProxy.Load(dcProxy.DiasNoHabilesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDiasNoHabiles, "FiltroInicial")
                Filtrar()
            End If

            'If So.UserState = "insert" Then                
            '    dcProxy.DiasNoHabiles.Clear()
            '    dcProxy.Load(dcProxy.DiasNoHabilesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDiasNoHabiles, "insert") ' Recarga la lista para que carguen los include
            'End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_DiasNoHabileSelected) Then
            Editando = True
            habilitar = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_DiasNoHabileSelected) Then
                dcProxy.RejectChanges()
                _DiasNoHabileSelected.strIdPais = _DiasNoHabileSelected.IdPais
                Editando = False
                If _DiasNoHabileSelected.EntityState = EntityState.Detached Then
                    DiasNoHabileSelected = DiasNoHabileAnterior
                End If
            End If
            habilitar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaDiasNoHabile
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        'Try
        '    If Not IsNothing(_DiasNoHabileSelected) Then
        '        dcProxy.DiasNoHabiles.Remove(_DiasNoHabileSelected)
        '        DiasNoHabileSelected = _ListaDiasNoHabiles.LastOrDefault
        '        IsBusy = True
        '        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        '    End If
        '    habilitar = False
        'Catch ex As Exception
        '    IsBusy = False
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '     Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        'End Try
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub

    Public Overrides Sub Buscar()
        'cb.NoHabil = DiasNoHabilePorDefecto.NoHabil.Date
        cb.NoHabil = Nothing
        cb.Activo = True
        MyBase.Buscar()
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("IdPais", 1)
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaDiasNoHabile
 	
    <Display(Name:="Día no hábil")> _
    Public Property NoHabil As DateTime?

    <Display(Name:="País")> _
    Public Property IdPais As Integer
 	
    <Display(Name:="¿Es Día no Hábil?")> _
    Public Property Activo As Boolean

End Class




