Imports Telerik.Windows.Controls
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

Public Class TiposEmisoresViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaTiposEmisores
    Private TiposEmisoresPorDefecto As TiposEmisores
    Private TiposEmisoresAnterior As TiposEmisores
    Dim dcProxy As EspeciesCFDomainContext
    Dim dcProxy1 As EspeciesCFDomainContext
    Dim changes As Boolean
    Dim count As Integer
    Dim Nombre_usuario As String
    Dim usuario As String
    Dim intidTiposEmisores As Integer = 0
    Dim dcProxyMaestrosCF As MaestrosCFDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New EspeciesCFDomainContext()
                dcProxy1 = New EspeciesCFDomainContext()
                dcProxyMaestrosCF = New MaestrosCFDomainContext()
            Else
                dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
                dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
                dcProxyMaestrosCF = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.TiposEmisoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEmisores, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerTiposEmisoresPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEmisoresPorDefecto_Completed, "Default")
                dcProxy.Load(dcProxy.EmisoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmisores, "FiltroInicial")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CalificadorasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



#Region "Asincronicos"

    Private Sub TerminoTraerEmisores(ByVal lo As LoadOperation(Of Emisore))
        If Not lo.HasError Then

            If dcProxy.Emisores.Count > 0 Then
                ListaEmisores = dcProxy.Emisores
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro de EMISORES", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EMISORES", _
                                             Me.ToString(), "TerminoTraerEmisores", Application.Current.ToString(), Program.Maquina, lo.Error)

        End If
        IsBusy = False
    End Sub
    Private Sub TerminoTraerTiposEmisores(ByVal lo As LoadOperation(Of TiposEmisores))
        If Not lo.HasError Then
            ListaTiposEmisores = dcProxy.TiposEmisores
            If dcProxy.TiposEmisores.Count > 0 Then
                If lo.UserState = "insert" Then
                    TiposEmisoresSelected = ListaTiposEmisores.LastOrDefault
                ElseIf lo.UserState = "update" Then
                    TiposEmisoresSelected = ListaTiposEmisores.Where(Function(i) i.intIdTipoEmisor = intidTiposEmisores).First
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    If ListaTiposEmisores.Count = 0 Then
                        TiposEmisoresSelected = Nothing
                        'TiposEmisoresSelected = New Calificadoras
                        MyBase.CambioItem("TiposEmisoresSelected")
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Calificadoras", _
                                             Me.ToString(), "TerminoTraerCalificadoras", Application.Current.ToString(), Program.Maquina, lo.Error)

        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerTiposEmisoresPorDefecto_Completed(ByVal lo As LoadOperation(Of TiposEmisores))
        If Not lo.HasError Then
            TiposEmisoresPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Calificadoras por defecto", _
                                             Me.ToString(), "TerminoTraerCalificadorasPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)

        End If
    End Sub

    ''' <history>
    ''' Modificado por   : Juan Carlos Soto Cruz (JCS).
    ''' Fecha            : Enero 23/2014
    ''' Descripción      : Se cambia el proxie por el nuevo datacontext creado para calculos financieros en la consulta de las calificadoras.
    ''' </history>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try

            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaTiposEmisores.Remove(TiposEmisoresSelected)
                    End If
                Else

                    'TODO: Pendiente garantizar que Userstate no venga vacío
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                                   Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If
                If changes = True Then
                    dcProxy.RejectChanges()
                    changes = False

                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)

            ' JCS Enero 23/2014
            dcProxyMaestrosCF.Calificadoras.Clear()
            ' FIN JCS

            If Not IsNothing(ListaTiposEmisores) Then
                ListaTiposEmisores.Clear()
            End If

            MyBase.QuitarFiltroDespuesGuardar()
            dcProxy.Load(dcProxy.TiposEmisoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEmisores, So.UserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region


#Region "Propiedades"

    Private _idemisor As Integer
    Public Property idemisor() As Integer
        Get
            Return _idemisor
        End Get
        Set(ByVal value As Integer)
            _idemisor = value
            MyBase.CambioItem("idemisor")
        End Set
    End Property



    Private _Emisores As Emisore
    Public Property Emisores() As Emisore
        Get
            Return _Emisores
        End Get
        Set(ByVal value As Emisore)
            _Emisores = value
            MyBase.CambioItem("Emisores")
        End Set
    End Property

    Private _ListaEmisores As EntitySet(Of Emisore)
    Public Property ListaEmisores() As EntitySet(Of Emisore)
        Get
            Return _ListaEmisores
        End Get
        Set(ByVal value As EntitySet(Of Emisore))
            _ListaEmisores = value
            MyBase.CambioItem("ListaEmisores")
        End Set
    End Property



    Private _ListaTiposEmisores As EntitySet(Of TiposEmisores)
    Public Property ListaTiposEmisores() As EntitySet(Of TiposEmisores)
        Get
            Return _ListaTiposEmisores
        End Get
        Set(ByVal value As EntitySet(Of TiposEmisores))
            _ListaTiposEmisores = value

            MyBase.CambioItem("ListaTiposEmisores")
            MyBase.CambioItem("ListaTiposEmisoresPaged")
            If Not IsNothing(value) Then
                If IsNothing(TiposEmisoresAnterior) Then
                    TiposEmisoresSelected = _ListaTiposEmisores.FirstOrDefault
                Else
                    TiposEmisoresSelected = TiposEmisoresAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaTiposEmisoresPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTiposEmisores) Then
                Dim view = New PagedCollectionView(_ListaTiposEmisores)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TiposEmisoresSelected As TiposEmisores
    Public Property TiposEmisoresSelected() As TiposEmisores
        Get
            Return _TiposEmisoresSelected
        End Get
        Set(ByVal value As TiposEmisores)
            _TiposEmisoresSelected = value
            MyBase.CambioItem("TiposEmisoresSelected")
        End Set
    End Property

#End Region

#Region "METODOS"
    Public Overrides Sub NuevoRegistro()
        Try

            Dim NewCalificadora As New TiposEmisores
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCalificadora.intCodTipoEmisor = TiposEmisoresPorDefecto.intCodTipoEmisor
            NewCalificadora.intIdEmisor = TiposEmisoresPorDefecto.intIdEmisor
            NewCalificadora.strDescripcionTipoEmisor = TiposEmisoresPorDefecto.strDescripcionTipoEmisor

            NewCalificadora.strUsuario = Program.Usuario
            NewCalificadora.dtmActualizacion = TiposEmisoresPorDefecto.dtmActualizacion
            TiposEmisoresAnterior = TiposEmisoresSelected
            TiposEmisoresSelected = NewCalificadora
            ' MyBase.CambioItem("ConsecutivosUsuarios")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_TiposEmisoresSelected) Then
            Editando = True
        End If
    End Sub
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_TiposEmisoresSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _TiposEmisoresSelected.EntityState = EntityState.Detached Then
                    TiposEmisoresSelected = TiposEmisoresAnterior
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
            cb = New CamposBusquedaTiposEmisores
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
            If Not IsNothing(_TiposEmisoresSelected) Then
                dcProxy.TiposEmisores.Remove(_TiposEmisoresSelected)
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
    Public Overrides Sub ActualizarRegistro()

        Try
            Dim origen = "update"
            ErrorForma = ""
            TiposEmisoresAnterior = TiposEmisoresSelected
            If Not ListaTiposEmisores.Contains(TiposEmisoresSelected) Then
                origen = "insert"
                ListaTiposEmisores.Add(TiposEmisoresSelected)
            Else
                intidTiposEmisores = TiposEmisoresSelected.intIdTipoEmisor
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
    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.intIdEmisor <> 0 Or cb.intIdTipoEmisor <> 0 Or cb.intCodTipoEmisor <> 0 Then                'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.TiposEmisores.Clear()
                TiposEmisoresAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.TiposEmisoresConsultarQuery(cb.intIdTipoEmisor, cb.intCodTipoEmisor, cb.intIdEmisor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEmisores, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaTiposEmisores
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Filtrar()
        Try
            dcProxy.TiposEmisores.Clear()
            If Not IsNothing(ListaTiposEmisores) Then
                ListaTiposEmisores.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.TiposEmisoresFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEmisores, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.TiposEmisoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEmisores, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaTiposEmisores
            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region
End Class



'Clase base para forma de búsquedas
Public Class CamposBusquedaTiposEmisores



    <Display(Name:="Id tipo emisor")> _
    Public Property intIdTipoEmisor As Integer

    <Display(Name:="Código emisor")> _
    Public Property intCodTipoEmisor As Integer

    <Display(Name:="Emisor")> _
    Public Property intIdEmisor As Integer


End Class
