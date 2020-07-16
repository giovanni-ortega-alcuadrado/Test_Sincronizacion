Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ConfigLEOViewModel.vb
'Generado el : 11/21/2011 16:36:48
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

Public Class ConfigLEOViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ConfigLEPorDefecto As ConfigLE
    Private ConfigLEAnterior As ConfigLE
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim dcProxy2 As MaestrosDomainContext
    Dim origen As String

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            dcProxy2 = New MaestrosDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy2 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ConfigLEOFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfigLEO, "")
                dcProxy2.Load(dcProxy2.ConfigLEOFiltrarDisponiblesQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDisponiblesConfigLEO, "")
                dcProxy1.Load(dcProxy1.TraerConfigLEPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfigLEOPorDefecto_Completed, "Default")

                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ConfigLEOViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConfigLEOViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerConfigLEOPorDefecto_Completed(ByVal lo As LoadOperation(Of ConfigLE))
        If Not lo.HasError Then
            ConfigLEPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConfigLE por defecto", _
                                             Me.ToString(), "TerminoTraerConfigLEPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerConfigLEO(ByVal lo As LoadOperation(Of ConfigLE))
        If Not lo.HasError Then
            ListaConfigLEO = dcProxy.ConfigLEs
            For Each co In ListaConfigLEO

                listadisponibles.Add(New ConfigLeoLista With {.Descripcion = co.Nombre, .Chequear = True})

            Next
            If dcProxy.ConfigLEs.Count > 0 Then
                If lo.UserState = "insert" Then
                    ConfigLESelected = ListaConfigLEO.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    MessageBox.Show("No se encontró ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConfigLEO", _
                                             Me.ToString(), "TerminoTraerConfigLE", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerDisponiblesConfigLEO(ByVal lo As LoadOperation(Of ListaConfigLEO))
        If Not lo.HasError Then
            'listadisponibles.Clear()
            For Each ll In dcProxy2.ListaConfigLEOs
                listadisponibles.Add(New ConfigLeoLista With {.Descripcion = ll.Descripción, .Chequear = ll.Cheked})
            Next

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Configuracion leo", _
                                       Me.ToString(), "TerminoTraerConceptosConsecutivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If



    End Sub

    ''' <summary>
    ''' Método que recibe la respuesta del llamado al servicio para actualizar LEOS
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara -  Marzo 13/2014</remarks>
    Private Sub TerminoInsertarLEOS(ByVal lo As InvokeOperation(Of Integer))
        Try
            IsBusy = False
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar LEOS", _
                Me.ToString(), "AddressOf TerminoInsertarLEOS", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al término de la llamada al proceso de actualización de LEOS.", Me.ToString(), "TerminoInsertarLEOS", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaConfigLEO As EntitySet(Of ConfigLE)
    Public Property ListaConfigLEO() As EntitySet(Of ConfigLE)
        Get
            Return _ListaConfigLEO
        End Get
        Set(ByVal value As EntitySet(Of ConfigLE))
            _ListaConfigLEO = value
            If Not IsNothing(value) Then
                If IsNothing(ConfigLEAnterior) Then
                    ConfigLESelected = _ListaConfigLEO.FirstOrDefault
                Else
                    ConfigLESelected = ConfigLEAnterior
                End If
            End If
            MyBase.CambioItem("ListaConfigLEO")
            MyBase.CambioItem("ListaConfigLEOPaged")
        End Set
    End Property

    Private _ConfigLESelected As ConfigLE
    Public Property ConfigLESelected() As ConfigLE
        Get
            Return _ConfigLESelected
        End Get
        Set(ByVal value As ConfigLE)
            _ConfigLESelected = value
            MyBase.CambioItem("ConfigLESelected")
        End Set
    End Property

    Private _listadisponibles As ObservableCollection(Of ConfigLeoLista) = New ObservableCollection(Of ConfigLeoLista)
    Public Property listadisponibles As ObservableCollection(Of ConfigLeoLista)
        Get
            Return _listadisponibles
        End Get
        Set(ByVal value As ObservableCollection(Of ConfigLeoLista))
            _listadisponibles = value
            MyBase.CambioItem("listadisponibles")
        End Set
    End Property

    Private WithEvents _disponibles As ConfigLeoLista = New ConfigLeoLista
    Public Property disponibles As ConfigLeoLista
        Get
            Return _disponibles
        End Get
        Set(ByVal value As ConfigLeoLista)
            _disponibles = value
            MyBase.CambioItem("disponibles")
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
            '          Dim NewConfigLE As New ConfigLE
            '	'TODO: Verificar cuales son los campos que deben inicializarse
            'NewConfigLE.IDComisionista = ConfigLEPorDefecto.IDComisionista
            'NewConfigLE.IDSucComisionista = ConfigLEPorDefecto.IDSucComisionista
            'NewConfigLE.ID = ConfigLEPorDefecto.ID
            'NewConfigLE.Nombre = ConfigLEPorDefecto.Nombre
            'NewConfigLE.NombreSQL = ConfigLEPorDefecto.NombreSQL
            'NewConfigLE.Actualizacion = ConfigLEPorDefecto.Actualizacion
            'NewConfigLE.Usuario = Program.Usuario
            'NewConfigLE.NombreDescSQL = ConfigLEPorDefecto.NombreDescSQL
            'NewConfigLE.IDConfigLEO = ConfigLEPorDefecto.IDConfigLEO
            '      ConfigLEAnterior = ConfigLESelected
            '      ConfigLESelected = NewConfigLE
            '      MyBase.CambioItem("ConfigLEO")
            '      Editando = True
            '      MyBase.CambioItem("Editando")
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ConfigLEs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ConfigLEOFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfigLEO, Nothing)
            Else
                dcProxy.Load(dcProxy.ConfigLEOFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfigLEO, Nothing)
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
            ConfigLEAnterior = ConfigLESelected
            If Not ListaConfigLEO.Contains(ConfigLESelected) Then
                origen = "insert"
                ListaConfigLEO.Add(ConfigLESelected)
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

            habilitar = False
            If So.HasError Then
                IsBusy = False
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                Exit Try
            Else
                MyBase.TerminoSubmitChanges(So)
                'Santiago Vergara -Marzo 13/2014 - Se hace el llamado al proceso para la actualización de LEOS
                IsBusy = True
                dcProxy.InsertarLEOS(1, Program.Usuario, Program.HashConexion, AddressOf TerminoInsertarLEOS, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ConfigLESelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ConfigLESelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ConfigLESelected.EntityState = EntityState.Detached Then
                    ConfigLESelected = ConfigLEAnterior
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
            'If Not IsNothing(_ConfigLESelected) Then
            '        dcProxy.ConfigLEs.Remove(_ConfigLESelected)
            '    IsBusy = True
            '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
            'End If
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _disponiblesSelectedClase_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _disponibles.PropertyChanged
        If e.PropertyName.Equals("Chequear") Then
            habilitar = True
            If disponibles.Chequear = True Then
                nuevaAficion()
            Else
                For Each a In ListaConfigLEO
                    If (disponibles.Descripcion.Equals(a.Nombre)) Then
                        ConfigLESelected = a
                    End If
                Next
                ListaConfigLEO.Remove(ConfigLESelected)
                ConfigLESelected = _ListaConfigLEO.LastOrDefault
            End If

        End If

    End Sub

    Sub nuevaAficion()
        Try
            Dim NewConfigLEO As New ConfigLE
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewConfigLEO.ID = ConfigLEPorDefecto.ID
            NewConfigLEO.Nombre = disponibles.Descripcion
            NewConfigLEO.NombreSQL = ConfigLEPorDefecto.NombreSQL
            NewConfigLEO.Usuario = Program.Usuario
            NewConfigLEO.NombreDescSQL = ConfigLEPorDefecto.NombreDescSQL
            NewConfigLEO.Index = 1
            ConfigLESelected = NewConfigLEO
            Editando = True
            MyBase.CambioItem("Editando")
            origen = "update"
            If Not ListaConfigLEO.Contains(ConfigLESelected) Then
                origen = "insert"
                ListaConfigLEO.Add(ConfigLESelected)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub Actualizar()
        IsBusy = True
        ErrorForma = ""
        ConfigLEAnterior = ConfigLESelected

        Program.VerificarCambiosProxyServidor(dcProxy)
        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
    End Sub


#End Region

End Class


'Clase base para forma de búsquedas
Public Class ConfigLeoLista
    Implements INotifyPropertyChanged
    Private _Chequear As Boolean
    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Checked", Description:="Chequear")> _
    Public Property Chequear() As Boolean
        Get
            Return _Chequear
        End Get
        Set(ByVal value As Boolean)
            _Chequear = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Chequear"))
        End Set
    End Property
    Private _Descripcion As String
    <Display(Name:="Descripcion")> _
    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))

        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
