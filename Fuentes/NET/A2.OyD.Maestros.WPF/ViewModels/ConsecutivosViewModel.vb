Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ConsecutivosViewModel.vb
'Generado el : 02/18/2011 09:56:35
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

Public Class ConsecutivosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaConsecutivo
    Private ConsecutivoPorDefecto As Consecutivo
    Private ConsecutivoAnterior As Consecutivo
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
                dcProxy.Load(dcProxy.ConsecutivosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerConsecutivoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ConsecutivosViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ConsecutivosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerConsecutivosPorDefecto_Completed(ByVal lo As LoadOperation(Of Consecutivo))
        If Not lo.HasError Then
            ConsecutivoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Consecutivo por defecto",
                                             Me.ToString(), "TerminoTraerConsecutivoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerConsecutivos(ByVal lo As LoadOperation(Of Consecutivo))
        If Not lo.HasError Then
            ListaConsecutivos = dcProxy.Consecutivos
            If dcProxy.Consecutivos.Count > 0 Then
                If lo.UserState = "insert" Then
                    ConsecutivoSelected = ListaConsecutivos.First   ' JFSB 20160827 Se pone el primer registro que tiene la lista
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Consecutivos",
                                             Me.ToString(), "TerminoTraerConsecutivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaConsecutivos As EntitySet(Of Consecutivo)
    Public Property ListaConsecutivos() As EntitySet(Of Consecutivo)
        Get
            Return _ListaConsecutivos
        End Get
        Set(ByVal value As EntitySet(Of Consecutivo))
            _ListaConsecutivos = value

            MyBase.CambioItem("ListaConsecutivos")
            MyBase.CambioItem("ListaConsecutivosPaged")
            If Not IsNothing(value) Then
                If IsNothing(ConsecutivoAnterior) Then
                    ConsecutivoSelected = _ListaConsecutivos.FirstOrDefault
                Else
                    ConsecutivoSelected = ConsecutivoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaConsecutivosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConsecutivos) Then
                Dim view = New PagedCollectionView(_ListaConsecutivos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ConsecutivoSelected As Consecutivo
    Public Property ConsecutivoSelected() As Consecutivo
        Get
            Return _ConsecutivoSelected
        End Get
        Set(ByVal value As Consecutivo)
            _ConsecutivoSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("ConsecutivoSelected")
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
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewConsecutivo As New Consecutivo
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewConsecutivo.IDComisionista = ConsecutivoPorDefecto.IDComisionista
            NewConsecutivo.IDSucComisionista = ConsecutivoPorDefecto.IDSucComisionista
            NewConsecutivo.IDOwner = ConsecutivoPorDefecto.IDOwner
            NewConsecutivo.NombreConsecutivo = ConsecutivoPorDefecto.NombreConsecutivo
            NewConsecutivo.Descripcion = ConsecutivoPorDefecto.Descripcion
            NewConsecutivo.Minimo = ConsecutivoPorDefecto.Minimo
            NewConsecutivo.Maximo = ConsecutivoPorDefecto.Maximo
            NewConsecutivo.Actual = ConsecutivoPorDefecto.Actual
            NewConsecutivo.Actualizacion = ConsecutivoPorDefecto.Actualizacion
            NewConsecutivo.Usuario = Program.Usuario
            NewConsecutivo.CodContabilidad = ConsecutivoPorDefecto.CodContabilidad
            NewConsecutivo.Activo = ConsecutivoPorDefecto.Activo
            NewConsecutivo.IdConsecutivos = ConsecutivoPorDefecto.IdConsecutivos
            ConsecutivoAnterior = ConsecutivoSelected
            ConsecutivoSelected = NewConsecutivo
            MyBase.CambioItem("Consecutivos")
            Editando = True
            Editareg = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Consecutivos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ConsecutivosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ConsecutivosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.IDOwner = String.Empty
        cb.NombreConsecutivo = String.Empty
        cb.Descripcion = String.Empty
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDOwner <> String.Empty Or cb.NombreConsecutivo <> String.Empty Or cb.Descripcion <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Consecutivos.Clear()
                ConsecutivoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDOwner = " &  cb.IDOwner.ToString() & " NombreConsecutivo = " &  cb.NombreConsecutivo.ToString() & " Descripcion = " &  cb.Descripcion.ToString() 
                dcProxy.Load(dcProxy.ConsecutivosConsultarQuery(cb.IDOwner, cb.NombreConsecutivo, cb.Descripcion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaConsecutivo
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
            If ConsecutivoSelected.Actual < ConsecutivoSelected.Minimo Then
                A2Utilidades.Mensajes.mostrarMensaje("El Valor Actual debe ser igual o mayor al Valor mínimo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If ConsecutivoSelected.Maximo < ConsecutivoSelected.Actual Then
                A2Utilidades.Mensajes.mostrarMensaje("El Valor Actual deber ser igual o menor al Valor Máximo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If ConsecutivoSelected.Maximo = 0 And ConsecutivoSelected.Actual = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El Valor Máximo y el Valor Actual no pueden ser igual a cero", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If ConsecutivoSelected.Minimo = ConsecutivoSelected.Maximo Then
                A2Utilidades.Mensajes.mostrarMensaje("El Valor Minimo y el Valor Máximo no pueden ser iguales", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If ConsecutivoSelected.Maximo < ConsecutivoSelected.Actual And ConsecutivoSelected.Maximo <= ConsecutivoSelected.Minimo Then
                A2Utilidades.Mensajes.mostrarMensaje("El Valor Mayor O Igual al Valor Actual", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim origen = "update"
            ErrorForma = ""
            ConsecutivoAnterior = ConsecutivoSelected
            If Not ListaConsecutivos.Contains(ConsecutivoSelected) Then
                origen = "insert"
                ListaConsecutivos.Add(ConsecutivoSelected)
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

    Public Overrides Sub TerminoSubmitChanges(ByVal So As OpenRiaServices.DomainServices.Client.SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And (So.UserState = "insert") Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    If So.UserState = "insert" Then
                        ListaConsecutivos.Remove(ConsecutivoSelected)
                    End If
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            If So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.Consecutivos.Clear()
                dcProxy.Load(dcProxy.ConsecutivosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, "insert") ' Recarga la lista para que carguen los include
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ConsecutivoSelected) Then
            Editando = True
            Editareg = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ConsecutivoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                If _ConsecutivoSelected.EntityState = EntityState.Detached Then
                    ConsecutivoSelected = ConsecutivoAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'MessageBox.Show("Esta funcionalidad no esta habilitada para este maestro", "Funcionalidad ", MessageBoxButton.OK)
        '  Try
        '      If Not IsNothing(_ConsecutivoSelected) Then
        '              dcProxy.Consecutivos.Remove(_ConsecutivoSelected)
        '              ConsecutivoSelected = _ListaConsecutivos.LastOrDefault
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
        DicCamposTab.Add("NombreConsecutivo", 1)
        DicCamposTab.Add("Descripcion", 1)
        DicCamposTab.Add("IDOwner", 1)
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaConsecutivo
    Implements INotifyPropertyChanged

    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    ' <Display(Name:="Módulo")> _
    'Public Property IDOwner As String

    Private _IDOwner As String = String.Empty
    <Display(Name:="Módulo")>
    Public Property IDOwner As String
        Get
            Return _IDOwner
        End Get
        Set(ByVal value As String)
            _IDOwner = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDOwner"))
        End Set
    End Property

    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    ' <Display(Name:="Nombre")> _
    'Public Property NombreConsecutivo As String

    Private _NombreConsecutivo As String = String.Empty
    <Display(Name:="Nombre")>
    Public Property NombreConsecutivo As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            _NombreConsecutivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreConsecutivo"))
        End Set
    End Property

    '<StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
    ' <Display(Name:="Descripción")> _
    'Public Property Descripcion As String

    Private _Descripcion As String = String.Empty
    <Display(Name:="Descripción")>
    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




