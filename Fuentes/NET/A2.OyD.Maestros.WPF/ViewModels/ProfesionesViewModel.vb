Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ProfesionesViewModel.vb
'Generado el : 01/24/2011 12:09:16
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

Public Class ProfesionesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaProfesione
    Private ProfesionePorDefecto As Profesione
    Private ProfesioneAnterior As Profesione

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
                dcProxy.Load(dcProxy.ProfesionesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProfesiones, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerProfesionePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProfesionesPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ProfesionesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ProfesionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerProfesionesPorDefecto_Completed(ByVal lo As LoadOperation(Of Profesione))
        If Not lo.HasError Then
            ProfesionePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Profesione por defecto",
                                             Me.ToString(), "TerminoTraerProfesionePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerProfesiones(ByVal lo As LoadOperation(Of Profesione))
        If Not lo.HasError Then
            ListaProfesiones = dcProxy.Profesiones
            If dcProxy.Profesiones.Count > 0 Then
                If lo.UserState = "insert" Then
                    ProfesioneSelected = ListaProfesiones.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Profesiones",
                                             Me.ToString(), "TerminoTraerProfesione", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaProfesiones As EntitySet(Of Profesione)
    Public Property ListaProfesiones() As EntitySet(Of Profesione)
        Get
            Return _ListaProfesiones
        End Get
        Set(ByVal value As EntitySet(Of Profesione))
            _ListaProfesiones = value

            MyBase.CambioItem("ListaProfesiones")
            MyBase.CambioItem("ListaProfesionesPaged")
            If Not IsNothing(value) Then
                If IsNothing(ProfesioneAnterior) Then
                    ProfesioneSelected = _ListaProfesiones.FirstOrDefault
                Else
                    ProfesioneSelected = ProfesioneAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaProfesionesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaProfesiones) Then
                Dim view = New PagedCollectionView(_ListaProfesiones)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ProfesioneSelected As Profesione
    Public Property ProfesioneSelected() As Profesione
        Get
            Return _ProfesioneSelected
        End Get
        Set(ByVal value As Profesione)
            _ProfesioneSelected = value
            'If Not value Is Nothing Then
            '    End If
            MyBase.CambioItem("ProfesioneSelected")
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
            Dim NewProfesione As New Profesione
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewProfesione.IDComisionista = ProfesionePorDefecto.IDComisionista
            NewProfesione.IDSucComisionista = ProfesionePorDefecto.IDSucComisionista
            NewProfesione.CodigoProfesion = ProfesionePorDefecto.CodigoProfesion
            NewProfesione.Nombre = ProfesionePorDefecto.Nombre
            NewProfesione.Actualizacion = ProfesionePorDefecto.Actualizacion
            NewProfesione.Usuario = Program.Usuario
            NewProfesione.IDProfesion = ProfesionePorDefecto.IDProfesion
            ProfesioneAnterior = ProfesioneSelected
            ProfesioneSelected = NewProfesione
            MyBase.CambioItem("Profesiones")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Profesiones.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ProfesionesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProfesiones, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ProfesionesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProfesiones, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.CodigoProfesion <> 0 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Profesiones.Clear()
                ProfesioneAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " CodigoProfesion = " &  cb.CodigoProfesion.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.ProfesionesConsultarQuery(cb.CodigoProfesion, cb.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProfesiones, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaProfesione
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
            'For Each LP In ListaProfesiones
            '    If LP.Nombre.ToUpper = ProfesioneSelected.Nombre.ToUpper Then
            '        A2Utilidades.Mensajes.mostrarMensaje("La Profesión " + ProfesioneSelected.Nombre + " ya Existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        Exit Sub
            '    End If
            'Next
            Dim origen = "update"
            ErrorForma = ""
            ProfesioneAnterior = ProfesioneSelected
            If Not ListaProfesiones.Contains(ProfesioneSelected) Then
                origen = "insert"
                ListaProfesiones.Add(ProfesioneSelected)
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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And (So.UserState = "BorrarRegistro") Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
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
            'If So.UserState = "insert" Then
            '	dcProxy.Profesiones.Clear()
            '      	dcProxy.Load(dcProxy.ProfesionesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProfesiones, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ProfesioneSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ProfesioneSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ProfesioneSelected.EntityState = EntityState.Detached Then
                    ProfesioneSelected = ProfesioneAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ProfesioneSelected) Then
                dcProxy.Profesiones.Remove(_ProfesioneSelected)
                ProfesioneSelected = _ListaProfesiones.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
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
    Public Overrides Sub Buscar()
        cb.CodigoProfesion = Nothing
        cb.Nombre = String.Empty
        MyBase.CambioItem("cb")
        MyBase.Buscar()

    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaProfesione
    Implements INotifyPropertyChanged
    Private _CodigoProfesion As Integer
    <Display(Name:="Código")>
    Public Property CodigoProfesion As Integer
        Get
            Return _CodigoProfesion
        End Get
        Set(ByVal value As Integer)
            _CodigoProfesion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoProfesion"))
        End Set
    End Property
    Private _Nombre As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Nombre")>
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




