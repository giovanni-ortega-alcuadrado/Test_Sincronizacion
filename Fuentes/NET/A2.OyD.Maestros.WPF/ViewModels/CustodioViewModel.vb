Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: CustodioViewModel.vb
'Generado el : 02/02/2011 16:49:19
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

Public Class CustodioViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCustodi
    Private CustodiPorDefecto As Custodi
    Private CustodiAnterior As Custodi
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
                dcProxy.Load(dcProxy.CustodioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodio, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCustodiPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodioPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  CustodioViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CustodioViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCustodioPorDefecto_Completed(ByVal lo As LoadOperation(Of Custodi))
        If Not lo.HasError Then
            CustodiPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Custodi por defecto", _
                                             Me.ToString(), "TerminoTraerCustodiPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCustodio(ByVal lo As LoadOperation(Of Custodi))
        If Not lo.HasError Then
            ListaCustodio = dcProxy.Custodis
            If dcProxy.Custodis.Count > 0 Then
                If lo.UserState = "insert" Then
                    CustodiSelected = ListaCustodio.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Custodio", _
                                             Me.ToString(), "TerminoTraerCustodi", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaCustodio As EntitySet(Of Custodi)
    Public Property ListaCustodio() As EntitySet(Of Custodi)
        Get
            Return _ListaCustodio
        End Get
        Set(ByVal value As EntitySet(Of Custodi))
            _ListaCustodio = value

            MyBase.CambioItem("ListaCustodio")
            MyBase.CambioItem("ListaCustodioPaged")
            If Not IsNothing(value) Then
                If IsNothing(CustodiAnterior) Then
                    CustodiSelected = _ListaCustodio.FirstOrDefault
                Else
                    CustodiSelected = CustodiAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCustodioPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCustodio) Then
                Dim view = New PagedCollectionView(_ListaCustodio)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CustodiSelected As Custodi
    Public Property CustodiSelected() As Custodi
        Get
            Return _CustodiSelected
        End Get
        Set(ByVal value As Custodi)
            _CustodiSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("CustodiSelected")
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
            Dim NewCustodi As New Custodi
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCustodi.ID = CustodiPorDefecto.ID
            NewCustodi.Nombre = CustodiPorDefecto.Nombre
            NewCustodi.Local = CustodiPorDefecto.Local
            NewCustodi.Actualizacion = CustodiPorDefecto.Actualizacion
            NewCustodi.Usuario = Program.Usuario
            CustodiAnterior = CustodiSelected
            CustodiSelected = NewCustodi
            MyBase.CambioItem("Custodio")
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
            dcProxy.Custodis.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CustodioFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodio, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CustodioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodio, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Custodis.Clear()
                CustodiAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " Nombre = " & cb.Nombre.ToString()
                dcProxy.Load(dcProxy.CustodioConsultarQuery(cb.ID, cb.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodio, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCustodi
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Buscar()
        cb = New CamposBusquedaCustodi
        CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            CustodiAnterior = CustodiSelected
            If Not ListaCustodio.Contains(CustodiSelected) Then
                origen = "insert"
                ListaCustodio.Add(CustodiSelected)
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
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'If So.UserState = "insert" Then
                    '    ListaCiudades.Remove(CiudadeSelected)
                    'End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                    CustodiSelected = ListaCustodio.Last
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.Custodis.Clear()
            '    dcProxy.Load(dcProxy.CustodioFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodio, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CustodiSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CustodiSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _CustodiSelected.EntityState = EntityState.Detached Then
                    CustodiSelected = CustodiAnterior
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
            If Not IsNothing(_CustodiSelected) Then
                dcProxy.Custodis.Remove(_CustodiSelected)
                CustodiSelected = _ListaCustodio.LastOrDefault()
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
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
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaCustodi

    <Display(Name:="Código")> _
    Public Property ID As Integer

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String
End Class




