Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClasesEspeciesViewModel.vb
'Generado el : 01/20/2011 09:58:14
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
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies

Public Class ClasesEspeciesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaClasesEspecie
    Private ClasesEspeciePorDefecto As ClasesEspecie
    Private ClasesEspecieAnterior As ClasesEspecie
    Dim dcProxy As EspeciesCFDomainContext
    Dim dcProxy1 As EspeciesCFDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim IdRegistro As Integer = 0

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New EspeciesCFDomainContext()
            dcProxy1 = New EspeciesCFDomainContext()
        Else
            dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ClasesEspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasesEspecies, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerClasesEspeciePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasesEspeciesPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ClasesEspeciesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClasesEspeciesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClasesEspeciesPorDefecto_Completed(ByVal lo As LoadOperation(Of ClasesEspecie))
        If Not lo.HasError Then
            ClasesEspeciePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClasesEspecie por defecto", _
                                             Me.ToString(), "TerminoTraerClasesEspeciePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClasesEspecies(ByVal lo As LoadOperation(Of ClasesEspecie))
        If Not lo.HasError Then
            ListaClasesEspecies = dcProxy.ClasesEspecies
            If dcProxy.ClasesEspecies.Count > 0 Then
                If lo.UserState = "insert" Then
                    ClasesEspecieSelected = ListaClasesEspecies.Last
                ElseIf lo.UserState = "update" Then
                    If ListaClasesEspecies.Where(Function(i) i.IDClasesEspecies = IdRegistro).Count > 0 Then
                        ClasesEspecieSelected = ListaClasesEspecies.Where(Function(i) i.IDClasesEspecies = IdRegistro).First
                    Else
                        ClasesEspecieSelected = ListaClasesEspecies.Last
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClasesEspecies", _
                                             Me.ToString(), "TerminoTraerClasesEspecie", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.ClasesEspecies.Clear()
                    dcProxy.Load(dcProxy.ClasesEspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasesEspecies, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaClasesEspecies As EntitySet(Of ClasesEspecie)
    Public Property ListaClasesEspecies() As EntitySet(Of ClasesEspecie)
        Get
            Return _ListaClasesEspecies
        End Get
        Set(ByVal value As EntitySet(Of ClasesEspecie))
            _ListaClasesEspecies = value

            MyBase.CambioItem("ListaClasesEspecies")
            MyBase.CambioItem("ListaClasesEspeciesPaged")
            If Not IsNothing(value) Then
                If IsNothing(ClasesEspecieAnterior) Then
                    ClasesEspecieSelected = _ListaClasesEspecies.FirstOrDefault
                Else
                    ClasesEspecieSelected = ClasesEspecieAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClasesEspeciesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClasesEspecies) Then
                Dim view = New PagedCollectionView(_ListaClasesEspecies)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ClasesEspecieSelected As ClasesEspecie
    Public Property ClasesEspecieSelected() As ClasesEspecie
        Get
            Return _ClasesEspecieSelected
        End Get
        Set(ByVal value As ClasesEspecie)
            _ClasesEspecieSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("ClasesEspecieSelected")
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
            Dim NewClasesEspecie As New ClasesEspecie
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewClasesEspecie.IDClasesEspecies = ClasesEspeciePorDefecto.IDClasesEspecies
            NewClasesEspecie.IDComisionista = ClasesEspeciePorDefecto.IDComisionista
            NewClasesEspecie.IDSucComisionista = ClasesEspeciePorDefecto.IDSucComisionista
            NewClasesEspecie.IDClaseEspecie = ClasesEspeciePorDefecto.IDClaseEspecie
            NewClasesEspecie.AplicaAccion = ClasesEspeciePorDefecto.AplicaAccion
            NewClasesEspecie.Nombre = ClasesEspeciePorDefecto.Nombre
            NewClasesEspecie.Actualizacion = ClasesEspeciePorDefecto.Actualizacion
            'NewClasesEspecie.Usuario = ClasesEspeciePorDefecto.Usuario
            NewClasesEspecie.Usuario = Program.Usuario      'eomc -- 21/16/2013 -- se usa program.usuario en veez de HttpContext.Current.User.Identity.Name
            NewClasesEspecie.IdProducto = ClasesEspeciePorDefecto.IdProducto
            NewClasesEspecie.TituloCarteraColectiva = ClasesEspeciePorDefecto.TituloCarteraColectiva
            NewClasesEspecie.ClaseContableTitulo = ClasesEspeciePorDefecto.ClaseContableTitulo
            NewClasesEspecie.CodigoClaseDeceval = ClasesEspeciePorDefecto.CodigoClaseDeceval
            ClasesEspecieAnterior = ClasesEspecieSelected
            ClasesEspecieSelected = NewClasesEspecie
            MyBase.CambioItem("ClasesEspecies")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ClasesEspecies.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ClasesEspeciesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasesEspecies, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ClasesEspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasesEspecies, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.IDClaseEspecie) Or cb.Nombre <> String.Empty Or cb.strCodInversionSuper <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ClasesEspecies.Clear()
                ClasesEspecieAnterior = Nothing
                IsBusy = True
                cb.IDClaseEspecie = IIf(IsNothing(cb.IDClaseEspecie), -1, cb.IDClaseEspecie)
                'DescripcionFiltroVM = " IDClaseEspecie = " &  cb.IDClaseEspecie.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.ClasesEspeciesConsultarQuery(cb.IDClaseEspecie, cb.Nombre, cb.strCodInversionSuper, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasesEspecies, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaClasesEspecie
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
            ClasesEspecieAnterior = ClasesEspecieSelected
            If Not ListaClasesEspecies.Contains(ClasesEspecieSelected) Then
                origen = "insert"
                ListaClasesEspecies.Add(ClasesEspecieSelected)
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
            If So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.ClasesEspecies.Clear()
                dcProxy.Load(dcProxy.ClasesEspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasesEspecies, "insert") ' Recarga la lista para que carguen los include
            ElseIf So.UserState = "update" Then
                IdRegistro = ClasesEspecieSelected.IDClasesEspecies
                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.ClasesEspecies.Clear()
                dcProxy.Load(dcProxy.ClasesEspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasesEspecies, "update") ' Recarga la lista para que carguen los include
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ClasesEspecieSelected) Then
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ClasesEspecieSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ClasesEspecieSelected.EntityState = EntityState.Detached Then
                    ClasesEspecieSelected = ClasesEspecieAnterior
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
            If Not IsNothing(_ClasesEspecieSelected) Then
                'dcProxy.ClasesEspecies.Remove(_ClasesEspecieSelected)
                'ClasesEspecieSelected = _ListaClasesEspecies.LastOrDefault
                IsBusy = True
                dcProxy.EliminarClasesEspecie(ClasesEspecieSelected.IDClasesEspecies, ClasesEspecieSelected.Usuario, String.Empty, Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")

                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
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
            cb = New CamposBusquedaClasesEspecie
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.IDClaseEspecie = Nothing
        MyBase.Buscar()
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaClasesEspecie

    <Display(Name:="Código")> _
    Public Property IDClaseEspecie As Integer?

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String

    <StringLength(15, ErrorMessage:="Código inversión super")> _
 <Display(Name:="Código inversión super")> _
    Public Property strCodInversionSuper As String



End Class




