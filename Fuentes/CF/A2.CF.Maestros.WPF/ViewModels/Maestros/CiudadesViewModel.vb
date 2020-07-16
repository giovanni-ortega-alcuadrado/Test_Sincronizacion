Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: CiudadesViewModel.vb
'Generado el : 02/24/2011 11:45:58
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

Public Class CiudadesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCiudade
    Private CiudadePorDefecto As Ciudade
    Private CiudadeAnterior As Ciudade

    Dim dcProxy As MaestrosCFDomainContext
    Dim dcProxy1 As MaestrosCFDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosCFDomainContext()
            dcProxy1 = New MaestrosCFDomainContext()
        Else
            dcProxy = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
            dcProxy1 = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.CiudadesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCiudades, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCiudadePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCiudadesPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  CiudadesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CiudadesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCiudadesPorDefecto_Completed(ByVal lo As LoadOperation(Of Ciudade))
        If Not lo.HasError Then
            CiudadePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Ciudade por defecto", _
                                             Me.ToString(), "TerminoTraerCiudadePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCiudades(ByVal lo As LoadOperation(Of Ciudade))
        If Not lo.HasError Then
            ListaCiudades = dcProxy.Ciudades
            If dcProxy.Ciudades.Count > 0 Then
                If lo.UserState = "insert" Then
                    CiudadeSelected = ListaCiudades.FirstOrDefault
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Ciudades", Me.ToString, "TerminoTraerEspecie", lo.Error)
            lo.MarkErrorAsHandled()
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ciudades", _
            '                                 Me.ToString(), "TerminoTraerCiudade", Application.Current.ToString(), Program.Maquina, lo.Error)
            'lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaCiudades As EntitySet(Of Ciudade)
    Public Property ListaCiudades() As EntitySet(Of Ciudade)
        Get
            Return _ListaCiudades
        End Get
        Set(ByVal value As EntitySet(Of Ciudade))
            _ListaCiudades = value

            MyBase.CambioItem("ListaCiudades")
            MyBase.CambioItem("ListaCiudadesPaged")

            If Not IsNothing(value) Then
                If IsNothing(CiudadeAnterior) Then
                    CiudadeSelected = _ListaCiudades.FirstOrDefault
                Else
                    CiudadeSelected = CiudadeAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCiudadesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCiudades) Then
                Dim view = New PagedCollectionView(_ListaCiudades)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CiudadeSelected As Ciudade
    Public Property CiudadeSelected() As Ciudade
        Get
            Return _CiudadeSelected
        End Get
        Set(ByVal value As Ciudade)
            _CiudadeSelected = value
            'If Not value Is Nothing Then
            'End If
            MyBase.CambioItem("CiudadeSelected")
        End Set
    End Property


    Private _Editareg As Boolean
    Public Property Editareg() As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("Editareg")
        End Set
    End Property
    Private _Limpiar As Integer
    Public Property Limpiar As Integer
        Get
            Return _Limpiar
        End Get
        Set(ByVal value As Integer)
            _Limpiar = value
            MyBase.CambioItem("Limpiar")
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
            Dim NewCiudade As New Ciudade
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCiudade.IDComisionista = CiudadePorDefecto.IDComisionista
            NewCiudade.IDSucComisionista = CiudadePorDefecto.IDSucComisionista
            NewCiudade.IDCodigo = CiudadePorDefecto.IDCodigo
            NewCiudade.Nombre = CiudadePorDefecto.Nombre
            NewCiudade.EsCapital = CiudadePorDefecto.EsCapital
            NewCiudade.IDdepartamento = CiudadePorDefecto.IDdepartamento
            NewCiudade.CodigoDANE = CiudadePorDefecto.CodigoDANE
            NewCiudade.Actualizacion = CiudadePorDefecto.Actualizacion
            NewCiudade.Usuario = Program.Usuario
            NewCiudade.IDCiudad = CiudadePorDefecto.IDCiudad
            CiudadeAnterior = CiudadeSelected
            CiudadeSelected = NewCiudade
            PropiedadTextoCombos = ""
            Limpiar = 0
            MyBase.CambioItem("Ciudade")
            Editando = True
            Editareg = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Ciudades.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CiudadesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCiudades, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CiudadesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCiudades, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDCodigo <> 0 Or cb.Nombre <> String.Empty Or cb.CodigoDANE <> String.Empty Or cb.IDdepartamento <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Ciudades.Clear()
                CiudadeAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDCodigo = " &  cb.IDCodigo.ToString() & " Nombre = " &  cb.Nombre.ToString() & " CodigoDANE = " &  cb.CodigoDANE.ToString() 
                dcProxy.Load(dcProxy.CiudadesConsultarQuery(cb.IDCodigo, cb.Nombre, cb.CodigoDANE, cb.IDdepartamento, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCiudades, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCiudade
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
            If CiudadeSelected.IDdepartamento = 0 Or IsNothing(CiudadeSelected.IDdepartamento) Then
                A2Utilidades.Mensajes.mostrarMensaje("Digite el nombre del departamento", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
                'Else
                '    Dim capita = From LC In ListaCiudades.Where(Function(L) L.IDdepartamento = CiudadeSelected.IDdepartamento And L.EsCapital = CiudadeSelected.EsCapital)
                '                                                Select LC
                '    If capita.Count > 0 Then
                '        A2Utilidades.Mensajes.mostrarMensaje("El Departamento ya tiene una Capital.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    End If
            End If
            Dim origen = "update"
            ErrorForma = ""
            CiudadeAnterior = CiudadeSelected

            If ListaCiudades.Where(Function(i) i.IDCodigo = CiudadeSelected.IDCodigo).Count = 0 Then
                origen = "insert"
                ListaCiudades.Add(CiudadeSelected)
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
                        ListaCiudades.Remove(CiudadeSelected)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.Ciudades.Clear()
            '    dcProxy.Load(dcProxy.CiudadesFiltrarQuery(""), AddressOf TerminoTraerCiudades, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CiudadeSelected) Then
            Editando = True
            Editareg = False
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CiudadeSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                If _CiudadeSelected.EntityState = EntityState.Detached Then
                    CiudadeSelected = CiudadeAnterior
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
            cb = New CamposBusquedaCiudade
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_CiudadeSelected) Then
                'dcProxy.Ciudades.Remove(_CiudadeSelected)
                'CiudadeSelected = _ListaCiudades.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.EliminarCiudad(CiudadeSelected.IDCiudad, String.Empty, Program.Usuario, Program.HashConexion, AddressOf terminoeliminar, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.Ciudades.Clear()
                    dcProxy.Load(dcProxy.CiudadesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCiudades, "insert") ' Recarga la lista para que carguen los include
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
    End Sub

    Public Overrides Sub Buscar()
        cb.PropiedadTextoCombos = ""
        MyBase.Buscar()
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaCiudade
    Implements INotifyPropertyChanged

    <Display(Name:="Departamento")> _
    Public Property IDdepartamento As Integer

    <Display(Name:="Código")> _
    Public Property IDCodigo As Integer

    <StringLength(40, ErrorMessage:="La longitud máxima es de 40")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String

    <StringLength(6, ErrorMessage:="La longitud máxima es de 6")> _
     <Display(Name:="Código DANE")> _
    Public Property CodigoDANE As String

    Private _PropiedadTextoCombos As String
    Public Property PropiedadTextoCombos() As String
        Get
            Return _PropiedadTextoCombos
        End Get
        Set(ByVal value As String)
            _PropiedadTextoCombos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PropiedadTextoCombos"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




