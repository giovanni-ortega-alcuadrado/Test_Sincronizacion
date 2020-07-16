Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: AuditoriaViewModel.vb
'Generado el : 08/31/2011 11:43:52
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
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades


Public Class AuditoriasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaAuditori
    Private AuditoriPorDefecto As AuditoriaTabla
    Private AuditoriAnterior As AuditoriaTabla
    Private NuevaAuditoria As AuditoriaTabla
    Dim dcProxy As UtilidadesDomainContext
    Dim dcProxy1 As UtilidadesDomainContext
    Dim dcProxy2 As UtilidadesDomainContext
    Dim IdAuditoriaBusqueda As Integer = 0 'Obtener el ultimo ID de la lista de Auditoria

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New UtilidadesDomainContext()
                dcProxy1 = New UtilidadesDomainContext()
                dcProxy2 = New UtilidadesDomainContext()
            Else
                dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxy1 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxy2 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.AuditoriaFiltrarQuery("LIS", String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAuditoria, "")
                dcProxy1.Load(dcProxy1.AuditoriaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAuditoriaPorDefecto_Completed, "Default")
                'DARM 20180110: Los combos tenían carga en todos casi todos los eventos causando que se perdieran los elementos seleccionados desaparecieran instantes después deseleccionarlos
                '               tanto del grid como del combo de edición.
                '               Se deshabilita la carga del combo de todos esos eventos (nuevo, grabar, borrar) y solo se deja en esta rutina.
                dcProxy2.Load(dcProxy2.cargarCombosAuditoriaQuery("Utilidades_Auditorias", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "ComboBusqueda")


                'Cargar los combos en el boton nuevo
                'dcProxy2.ItemCombos.Clear()
                'dcProxy2.Load(dcProxy2.cargarCombosAuditoriaQuery("Utilidades_Auditorias", Program.Usuario), AddressOf TerminoCargarCombos, "ComboBusqueda")

                'Esconder el combo para la vista de formulario
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  AuditoriaViewModel)(Me)

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "AuditoriaViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerAuditoriaPorDefecto_Completed(ByVal lo As LoadOperation(Of AuditoriaTabla))
        If Not lo.HasError Then
            AuditoriPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Auditori por defecto", _
                                             Me.ToString(), "TerminoTraerAuditoriPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerAuditoria(ByVal lo As LoadOperation(Of AuditoriaTabla))
        Dim strMsg As String = String.Empty
        If Not lo.HasError Then
            ListaAuditoria = dcProxy.AuditoriaTablas
            If dcProxy.AuditoriaTablas.Count > 0 Then
                If lo.UserState = "CargarTAB" Then
                    AuditoriSelected = ListaAuditoria.Last
                ElseIf lo.UserState = "Busqueda" Then
                    If AuditoriSelected.IdAuditoria = 0 Then
                        cb.IdAuditoria = IdAuditoriaBusqueda
                    End If

                Else
                    AuditoriSelected = ListaAuditoria.First
                End If

                'dcProxy2.ItemCombos.Clear()
                'dcProxy2.Load(dcProxy2.cargarCombosAuditoriaQuery("Utilidades_Auditorias", Program.Usuario), AddressOf TerminoCargarCombos, "ComboBusqueda")

            Else
                If lo.UserState = "Busqueda" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            IsBusy = False
            If lo.Error.Message.ToString.Contains("la tabla de auditoria") Then
                Dim intPosIni As Integer = lo.Error.Message.ToString.IndexOf("La tabla ")
                Dim intPosFin As Integer = lo.Error.Message.ToString.IndexOf("|")
                strMsg = lo.Error.Message.ToString.Substring(intPosIni, intPosFin - intPosIni)
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Auditoria", _
                                                 Me.ToString(), "TerminoTraerAuditori", Application.Current.ToString(), Program.Maquina, lo.Error)

            End If
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))

        Try
            If Not lo.HasError Then

                If lo.UserState = "ComboBusqueda" Then
                    DiccionarioLista = dcProxy2.ItemCombos.ToList
                    MyBase.CambioItem("DiccionarioLista")
                End If

                DiccionarioListaTablas = dcProxy2.ItemCombos.ToList
                MyBase.CambioItem("DiccionarioListaTablas")
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    'Tablas padres
#End Region

#Region "Propiedades"

    Private _ListaAuditoria As EntitySet(Of AuditoriaTabla)
    Public Property ListaAuditoria() As EntitySet(Of AuditoriaTabla)
        Get
            Return _ListaAuditoria
        End Get
        Set(ByVal value As EntitySet(Of AuditoriaTabla))
            _ListaAuditoria = value

            MyBase.CambioItem("ListaAuditoria")
            MyBase.CambioItem("ListaAuditoriaPaged")
            If Not IsNothing(value) Then
                If IsNothing(AuditoriAnterior) Then
                    AuditoriSelected = _ListaAuditoria.FirstOrDefault
                Else
                    AuditoriSelected = AuditoriAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaAuditoriaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaAuditoria) Then
                Dim view = New PagedCollectionView(_ListaAuditoria)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _AuditoriSelected As AuditoriaTabla
    Public Property AuditoriSelected() As AuditoriaTabla
        Get
            Return _AuditoriSelected
        End Get
        Set(ByVal value As AuditoriaTabla)
            _AuditoriSelected = value
            MyBase.CambioItem("AuditoriSelected")
        End Set
    End Property

    Private _cmbNombreTablaEdicion As Boolean = False
    Public Property cmbNombreTablaEdicion() As Boolean
        Get
            Return _cmbNombreTablaEdicion
        End Get
        Set(ByVal value As Boolean)
            _cmbNombreTablaEdicion = value
            MyBase.CambioItem("cmbNombreTablaEdicion")
        End Set
    End Property

    Private _DiccionarioLista As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property DiccionarioLista() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _DiccionarioLista
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _DiccionarioLista = value
            MyBase.CambioItem("DiccionarioLista")
        End Set
    End Property

    Private _DiccionarioListaTablas As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property DiccionarioListaTablas() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _DiccionarioListaTablas
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _DiccionarioListaTablas = value
            MyBase.CambioItem("DiccionarioListaTablas")
        End Set
    End Property

    Private _NombreTablaNuevoRegistro As String = String.Empty
    Public Property NombreTablaNuevoRegistro() As String
        Get
            Return _NombreTablaNuevoRegistro
        End Get
        Set(ByVal value As String)
            _NombreTablaNuevoRegistro = value
            MyBase.CambioItem("NombreTablaNuevoRegistro")
        End Set
    End Property

    Private _MostrarComboNombreTabla As Boolean = False
    Public Property MostrarComboNombreTabla() As Boolean
        Get
            Return _MostrarComboNombreTabla
        End Get
        Set(ByVal value As Boolean)
            _MostrarComboNombreTabla = value
            MyBase.CambioItem("MostrarComboNombreTabla")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            'Dim NewAuditori As New AuditoriaTabla
            'TODO: Verificar cuales son los campos que deben inicializarse
            'NewAuditori.IdAuditoria = ListaAuditoria.Last.IdAuditoria + 1
            'NewAuditori.NombreTabla = AuditoriPorDefecto.NombreTabla
            NuevaAuditoria = New AuditoriaTabla
            NuevaAuditoria.Ingreso = AuditoriPorDefecto.Ingreso
            NuevaAuditoria.Modificacion = AuditoriPorDefecto.Modificacion
            NuevaAuditoria.Eliminacion = AuditoriPorDefecto.Eliminacion
            NuevaAuditoria.Usuario = Program.Usuario
            AuditoriAnterior = AuditoriSelected
            AuditoriSelected = NuevaAuditoria
            PropiedadTextoCombos = ""
            NombreTablaNuevoRegistro = String.Empty
            MyBase.CambioItem("TextoCombos")
            MyBase.CambioItem("AuditoriSelected")
            Editando = True
            MyBase.CambioItem("Editando")

            cmbNombreTablaEdicion = True
            MyBase.CambioItem("cmbNombreTablaEdicion")
            MostrarComboNombreTabla = True
            'dcProxy2.ItemCombos.Clear()
            'dcProxy2.Load(dcProxy2.cargarCombosAuditoriaQuery("Utilidades_Auditorias_Nuevo", Program.Usuario), AddressOf TerminoCargarCombos, "Edicion")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.AuditoriaTablas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.AuditoriaFiltrarQuery("CON", TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAuditoria, Nothing)
            Else
                dcProxy.Load(dcProxy.AuditoriaFiltrarQuery("LIS", String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAuditoria, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If Not IsNothing(NuevaAuditoria) Then
                If NuevaAuditoria.IdAuditoria <= 0 Or IsNothing(NuevaAuditoria.IdAuditoria) Then
                    NuevaAuditoria.NombreTabla = NombreTablaNuevoRegistro
                End If
            End If

            If ValidarAuditoria() = True Then
                Dim origen = "update"
                ErrorForma = ""
                AuditoriAnterior = AuditoriSelected

                If Not IsNothing(NuevaAuditoria) Then
                    If NuevaAuditoria.IdAuditoria <> 0 Then

                        origen = "insert"
                        Dim newAuditoria As New AuditoriaTabla
                        newAuditoria.IdAuditoria = NuevaAuditoria.IdAuditoria
                        newAuditoria.NombreTabla = NuevaAuditoria.NombreTabla
                        newAuditoria.Ingreso = NuevaAuditoria.Ingreso
                        newAuditoria.Modificacion = NuevaAuditoria.Modificacion
                        newAuditoria.Eliminacion = NuevaAuditoria.Eliminacion
                        newAuditoria.Usuario = Program.Usuario
                        ListaAuditoria.Add(newAuditoria)

                    Else

                        If Not ListaAuditoria.Contains(AuditoriSelected) Then
                            origen = "insert"
                            ListaAuditoria.Add(AuditoriSelected)
                        End If

                    End If
                End If

                IdAuditoriaBusqueda = 0
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
            Dim strMsg As String = String.Empty
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), "|")
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                    'Else
                    '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                    '                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                    'End If
                ElseIf So.Error.Message.Contains("No se puede eliminar") Then
                    Dim intPosIni As Integer = So.Error.ToString.IndexOf("No se puede eliminar")
                    Dim intPosFin As Integer = So.Error.ToString.IndexOf("|")
                    strMsg = So.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'If So.Error.Message.Contains("DELETE") And So.Error.Message.Contains("REFERENCE") Then
                    '    A2Utilidades.Mensajes.mostrarMensaje("La auditoría de la tabla " & AuditoriSelected.NombreTabla & " no se puede eliminar, porque tiene auditorias asociadas... ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                                   Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                AuditoriSelected = ListaAuditoria.Last
                Exit Try
            End If
            If ((So.UserState = "insert") Or (So.UserState = "BorrarRegistro")) Then
                MyBase.QuitarFiltroDespuesGuardar()
                AuditoriAnterior = Nothing
                If So.UserState = "insert" Then NuevaAuditoria = Nothing
                dcProxy.AuditoriaTablas.Clear()
                dcProxy.Load(dcProxy.AuditoriaFiltrarQuery("LIS", String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAuditoria, "CargarTAB")
            End If

            cmbNombreTablaEdicion = False
            MyBase.CambioItem("cmbNombreTablaEdicion")
            MostrarComboNombreTabla = False
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_AuditoriSelected) Then
            AuditoriAnterior = AuditoriSelected 'DARM 20180110 No se esta salvando el anterior registro.
            Editando = True
            cmbNombreTablaEdicion = False
            MyBase.CambioItem("cmbNombreTablaEdicion")
            MyBase.CambioItem("Editando")
            MostrarComboNombreTabla = False

        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_AuditoriSelected) Then
                dcProxy.RejectChanges()
                If _AuditoriSelected.EntityState = EntityState.Detached Then
                    AuditoriSelected = AuditoriAnterior
                    MyBase.CambioItem("AuditoriSelected")
                End If
            End If
            Editando = False
            cmbNombreTablaEdicion = False
            MyBase.CambioItem("cmbNombreTablaEdicion")
            MyBase.CambioItem("Editando")
            MostrarComboNombreTabla = False
            'dcProxy2.ItemCombos.Clear()
            'dcProxy2.Load(dcProxy2.cargarCombosAuditoriaQuery("Utilidades_Auditorias", Program.Usuario), AddressOf TerminoCargarCombos, "ComboBusqueda")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_AuditoriSelected) Then
                dcProxy.AuditoriaTablas.Remove(_AuditoriSelected)
                IsBusy = True
                'dcProxy2.ItemCombos.Clear()
                'dcProxy2.Load(dcProxy2.cargarCombosAuditoriaQuery("Utilidades_Auditorias", Program.Usuario), AddressOf TerminoCargarCombos, "Busqueda")
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            'cb.IdAuditoria = 0
            'cb.NombreTabla = String.Empty
            'cb.Ingresa = False
            'cb.Modifica = False
            'cb.Retira = False
            'cb = New CamposBusquedaAuditori
            MyBase.Buscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Buscar los registros", _
                                 Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            ErrorForma = ""
            If String.IsNullOrEmpty(cb.NombreTabla) Then
                A2Utilidades.Mensajes.mostrarMensaje("Se requiere seleccionar la tabla.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                'Se asigna porque en la busqueda se pueden guardar cambios así la tabla no exista.
                IdAuditoriaBusqueda = ListaAuditoria.Last.IdAuditoria + 1
                dcProxy.AuditoriaTablas.Clear()
                AuditoriAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.AuditoriaConsultarQuery("CON", cb.NombreTabla, cb.Ingresa, cb.Modifica, cb.Retira, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAuditoria, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaAuditori
                CambioItem("cb")
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function ValidarAuditoria() As Boolean
        Try

            If IsNothing(AuditoriSelected.NombreTabla) Or AuditoriSelected.NombreTabla = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Se requiere seleccionar la tabla.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            If AuditoriSelected.Ingreso = False And AuditoriSelected.Modificacion = False And AuditoriSelected.Eliminacion = False Then
                A2Utilidades.Mensajes.mostrarMensaje("Se requiere seleccionar por lo menos un Log de Auditoria.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            End If

            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function


#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaAuditori
    Implements INotifyPropertyChanged

    <Display(Name:="Id Auditoria:", Description:="ID")> _
    Private _IdAuditoria As Integer
    Public Property IdAuditoria As Integer
        Get
            Return _IdAuditoria
        End Get
        Set(ByVal value As Integer)
            _IdAuditoria = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdAuditoria"))
        End Set
    End Property

    <Display(Name:="Tabla para Auditar:", Description:="Nombre Tabla")> _
    Private _NombreTabla As String
    Public Property NombreTabla As String
        Get
            Return _NombreTabla
        End Get
        Set(ByVal value As String)
            _NombreTabla = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreTabla"))
        End Set
    End Property


    <Display(Name:="Log Ingreso:", Description:="Ingreso")> _
    Private _Ingresa As Boolean
    Public Property Ingresa As Boolean
        Get
            Return _Ingresa
        End Get
        Set(ByVal value As Boolean)
            _Ingresa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ingresa"))
        End Set
    End Property


    <Display(Name:="Log Modificar:", Description:="Modificación")> _
    Private _Modifica As Boolean
    Public Property Modifica As Boolean
        Get
            Return _Modifica
        End Get
        Set(ByVal value As Boolean)
            _Modifica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modifica"))
        End Set
    End Property

    <Display(Name:="Log Borrar:", Description:="Eliminación")> _
    Private _Retira As Boolean
    Public Property Retira As Boolean
        Get
            Return _Retira
        End Get
        Set(ByVal value As Boolean)
            _Retira = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Retira"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class




