Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClasificacionesCiiuViewModel.vb
'Generado el : 03/12/2012 08:54:49
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

Public Class ClasificacionesCiiuViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaClasificacionesCii
    Private ClasificacionesCiiPorDefecto As ClasificacionesCii
    Private ClasificacionesCiiAnterior As ClasificacionesCii
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
                dcProxy.Load(dcProxy.ClasificacionesCiiuFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionesCiiu, "")
                dcProxy1.Load(dcProxy1.TraerClasificacionesCiiPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionesCiiuPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ClasificacionesCiiuViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClasificacionesCiiuViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClasificacionesCiiuPorDefecto_Completed(ByVal lo As LoadOperation(Of ClasificacionesCii))
        If Not lo.HasError Then
            ClasificacionesCiiPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClasificacionesCii por defecto", _
                                             Me.ToString(), "TerminoTraerClasificacionesCiiPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClasificacionesCiiu(ByVal lo As LoadOperation(Of ClasificacionesCii))
        If Not lo.HasError Then
            ListaClasificacionesCiiu = dcProxy.ClasificacionesCiis
            If dcProxy.ClasificacionesCiis.Count > 0 Then
                If lo.UserState = "insert" Then
                    ClasificacionesCiiSelected = ListaClasificacionesCiiu.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MessageBox.Show("No se encontró ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClasificacionesCiiu", _
                                             Me.ToString(), "TerminoTraerClasificacionesCii", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerClasificacionesCiiuCombo(ByVal lo As LoadOperation(Of ItemCombo))
        If Not lo.HasError Then
            ListaClasificacionesCiiuCombo = dcProxy.ItemCombos
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de ClasificacionesCiiu", Me.ToString, "TerminoTraerClasificacionesCiiCombo", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClasificacionesCiiu", _
            '                                 Me.ToString(), "TerminoTraerClasificacionesCiiCombo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaClasificacionesCiiu As EntitySet(Of ClasificacionesCii)
    Public Property ListaClasificacionesCiiu() As EntitySet(Of ClasificacionesCii)
        Get
            Return _ListaClasificacionesCiiu
        End Get
        Set(ByVal value As EntitySet(Of ClasificacionesCii))
            _ListaClasificacionesCiiu = value
            MyBase.CambioItem("ListaClasificacionesCiiu")
            MyBase.CambioItem("ListaClasificacionesCiiuPaged")
            If Not IsNothing(value) Then
                If IsNothing(ClasificacionesCiiAnterior) Then
                    ClasificacionesCiiSelected = _ListaClasificacionesCiiu.FirstOrDefault
                Else
                    ClasificacionesCiiSelected = ClasificacionesCiiAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClasificacionesCiiuPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClasificacionesCiiu) Then
                Dim view = New PagedCollectionView(_ListaClasificacionesCiiu)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ClasificacionesCiiSelected As ClasificacionesCii
    Public Property ClasificacionesCiiSelected() As ClasificacionesCii
        Get
            Return _ClasificacionesCiiSelected
        End Get
        Set(ByVal value As ClasificacionesCii)
            _ClasificacionesCiiSelected = value
            If Not IsNothing(value) Then
                dcProxy.ItemCombos.Clear()
                dcProxy.Load(dcProxy.ClasificacionesCiiuConsultarComboQuery(ClasificacionesCiiSelected.Tipo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionesCiiuCombo, "Busqueda")
            End If
            MyBase.CambioItem("ClasificacionesCiiSelected")
        End Set
    End Property

    Private _HablitarPertenece As Boolean = True
    Public Property HablitarPertenece() As Boolean
        Get
            Return _HablitarPertenece
        End Get
        Set(ByVal value As Boolean)
            _HablitarPertenece = value
            MyBase.CambioItem("HablitarPertenece")
        End Set
    End Property

    Private _ListaClasificacionesCiiuCombo As EntitySet(Of ItemCombo)
    Public Property ListaClasificacionesCiiuCombo() As EntitySet(Of ItemCombo)
        Get
            Return _ListaClasificacionesCiiuCombo
        End Get
        Set(ByVal value As EntitySet(Of ItemCombo))
            _ListaClasificacionesCiiuCombo = value
            MyBase.CambioItem("ListaClasificacionesCiiuCombo")
        End Set
    End Property


    Private _LimpiarCombos As String
    Public Property LimpiarCombos() As String
        Get
            Return _LimpiarCombos
        End Get
        Set(ByVal value As String)
            _LimpiarCombos = value
            MyBase.CambioItem("LimpiarCombos")
        End Set
    End Property




    Private _codigo As Boolean = True
    Public Property codigo() As Boolean
        Get
            Return _codigo
        End Get
        Set(ByVal value As Boolean)
            _codigo = value
            MyBase.CambioItem("codigo")
        End Set
    End Property


#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewClasificacionesCii As New ClasificacionesCii
            'TODO: Verificar cuales son los campos que deben inicializarse            
            NewClasificacionesCii.ID = ClasificacionesCiiPorDefecto.ID
            NewClasificacionesCii.Nombre = ClasificacionesCiiPorDefecto.Nombre
            NewClasificacionesCii.Tipo = -1
            NewClasificacionesCii.Tipo = ClasificacionesCiiPorDefecto.Tipo
            NewClasificacionesCii.IDPerteneceA = ClasificacionesCiiPorDefecto.IDPerteneceA
            NewClasificacionesCii.Actualizacion = ClasificacionesCiiPorDefecto.Actualizacion
            NewClasificacionesCii.Usuario = Program.Usuario
            NewClasificacionesCii.IDClasificacionCiiu = ClasificacionesCiiPorDefecto.IDClasificacionCiiu
            ClasificacionesCiiAnterior = ClasificacionesCiiSelected
            ClasificacionesCiiSelected = NewClasificacionesCii
            LimpiarCombos = ""
            MyBase.CambioItem("LimpiarCombos")
            PropiedadTextoCombos = ""
            MyBase.CambioItem("ClasificacionesCiiu")
            Editando = True
            HablitarPertenece = True
            codigo = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ClasificacionesCiis.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ClasificacionesCiiuFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionesCiiu, Nothing)
            Else
                dcProxy.Load(dcProxy.ClasificacionesCiiuFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionesCiiu, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> String.Empty Or cb.Tipo <> 0 Or cb.IDPerteneceA <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ClasificacionesCiis.Clear()
                ClasificacionesCiiAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " ID = " &  cb.ID.ToString() & " Nombre = " &  cb.Nombre.ToString() & " Tipo = " &  cb.Tipo.ToString() & " IDPerteneceA = " &  cb.IDPerteneceA.ToString() & " IDClasificacionCiiu = " &  cb.IDClasificacionCiiu.ToString()    'Dic202011 quitar
                dcProxy.Load(dcProxy.ClasificacionesCiiuConsultarQuery(cb.ID, cb.Nombre, cb.Tipo, cb.IDPerteneceA, cb.IDClasificacionCiiu,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionesCiiu, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaClasificacionesCii
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

            If Not IsNothing(ClasificacionesCiiSelected) Then
                If IsNothing(ClasificacionesCiiSelected.ID) Or ClasificacionesCiiSelected.ID = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El Código es un dato requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If IsNothing(ClasificacionesCiiSelected.Tipo) Or ClasificacionesCiiSelected.Tipo = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El Tipo es Requerido por favor Ingreselo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If IsNothing(ClasificacionesCiiSelected.IDPerteneceA) Or ClasificacionesCiiSelected.IDPerteneceA = 0 And ClasificacionesCiiSelected.Tipo <> 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Pertenece es Requerido por favor Ingreselo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If


            Dim origen = "update"
            ErrorForma = ""
            ClasificacionesCiiAnterior = ClasificacionesCiiSelected
            If Not ListaClasificacionesCiiu.Contains(ClasificacionesCiiSelected) Then
                origen = "insert"
                If Not IsNothing(ClasificacionesCiiSelected) Then
                    If Not IsNothing(ClasificacionesCiiSelected.ID) Then
                        For Each led In ListaClasificacionesCiiu
                            If led.ID = ClasificacionesCiiSelected.ID Then
                                A2Utilidades.Mensajes.mostrarMensaje("El Código  Ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        Next
                    End If
                End If
                ListaClasificacionesCiiu.Add(ClasificacionesCiiSelected)
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
        If Not IsNothing(_ClasificacionesCiiSelected) Then
            Editando = True
            codigo = False
            HablitarPertenece = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ClasificacionesCiiSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ClasificacionesCiiSelected.EntityState = EntityState.Detached Then
                    ClasificacionesCiiSelected = ClasificacionesCiiAnterior
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
            A2Utilidades.Mensajes.mostrarMensaje("Esta función esta inhabilitada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'If Not IsNothing(_ClasificacionesCiiSelected) Then
            '    dcProxy.ClasificacionesCiis.Remove(_ClasificacionesCiiSelected)
            '    ClasificacionesCiiSelected = _ListaClasificacionesCiiu.LastOrDefault  'Dic202011  nueva
            '    IsBusy = True
            '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _ClasificacionesCiiSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClasificacionesCiiSelected.PropertyChanged
        If e.PropertyName.Equals("Tipo") Then
            If ClasificacionesCiiSelected.Tipo = 1 Then
                HablitarPertenece = False
                ClasificacionesCiiSelected.IDPerteneceA = ClasificacionesCiiSelected.ID
                'dcProxy.ClasificacionesCiis.Clear()                              
                'dcProxy.Load(dcProxy.ClasificacionesCiiuConsultarComboQuery(ClasificacionesCiiSelected.Tipo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionesCiiuCombo, "Busqueda")
                ListaClasificacionesCiiuCombo = Nothing
                MyBase.CambioItem("ListaClasificacionesCiiuCombo")
                LimpiarCombos = String.Empty
            Else
                HablitarPertenece = True
                ListaClasificacionesCiiuCombo = Nothing
                MyBase.CambioItem("ListaClasificacionesCiiuCombo")
                LimpiarCombos = String.Empty
                dcProxy.ItemCombos.Clear()
                dcProxy.Load(dcProxy.ClasificacionesCiiuConsultarComboQuery(ClasificacionesCiiSelected.Tipo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionesCiiuCombo, "Busqueda")
            End If
        End If
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaClasificacionesCii

    <Display(Name:="Código")> _
    Public Property ID As Integer

    <StringLength(100, ErrorMessage:="La longitud máxima es de 100")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String

    <Display(Name:="Tipo")> _
    Public Property Tipo As Integer

    <Display(Name:="Pertenece A")> _
    Public Property IDPerteneceA As Integer

    <Display(Name:="IDClasificacionCiiu")> _
    Public Property IDClasificacionCiiu As Integer
End Class




