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
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies

Public Class CalificacionesEmisorViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCalificacionesEmisor
    Private CalificacionesEmisorPorDefecto As CalificacionesEmisor
    Private CalificacionesEmisorAnterior As CalificacionesEmisor
    Dim dcProxy As MaestrosCFDomainContext
    Dim dcProxy1 As MaestrosCFDomainContext
    Dim changes As Boolean
    Dim count As Integer
    Dim Nombre_usuario As String
    Dim usuario As String
    Dim intidCalificacionesEmisor As Integer = 0
    Dim dcProxyMaestrosCF As MaestrosCFDomainContext
    Dim dcProxyEspecies As EspeciesCFDomainContext

    ''' <history>
    ''' Descripción:    Se cambia el proxie por el nuevo datacontext creado para calculos financieros en la consulta de las calificadoras.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          Septiembre 2/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - Septiembre 2/2014
    ''' </history>  
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New MaestrosCFDomainContext()
                dcProxy1 = New MaestrosCFDomainContext()
                dcProxyMaestrosCF = New MaestrosCFDomainContext()
                dcProxyEspecies = New EspeciesCFDomainContext()
            Else
                dcProxy = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
                dcProxy1 = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
                dcProxyMaestrosCF = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
                dcProxyEspecies = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.CalificacionesEmisorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificacionesEmisor, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCalificacionesEmisorPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificacionesEmisorPorDefecto_Completed, "Default")
                dcProxyEspecies.Load(dcProxyEspecies.EmisoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmisores, "FiltroInicial")
                'dcProxy.Load(dcProxy.CalificacionesInversionesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificacionesInversiones, "FiltroInicial")
                dcProxyMaestrosCF.Load(dcProxyMaestrosCF.FiltrarCalificacionesInversionesQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificacionesInversiones, "FiltroInicial")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CalificadorasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



#Region "Asincronicos"
    ''' <history>
    ''' Descripción:    Se cambia el proxie por el nuevo datacontext creado para calculos financieros en la consulta de las calificadoras.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          Septiembre 2/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - Septiembre 2/2014
    ''' </history> 
    Private Sub TerminoTraerCalificacionesInversiones(ByVal lo As LoadOperation(Of CalificacionesInversiones))
        If Not lo.HasError Then
            If dcProxyMaestrosCF.CalificacionesInversiones.Count > 0 Then
                ListaCalificacionInversion = dcProxyMaestrosCF.CalificacionesInversiones
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro de calificaciones inversiones", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'MyBase.Buscar()
                'MyBase.CancelarBuscar()

            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de calificaciones inversiones", _
                                             Me.ToString(), "TerminoTraerCalificacionesInversiones", Application.Current.ToString(), Program.Maquina, lo.Error)

        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerEmisores(ByVal lo As LoadOperation(Of Emisore))
        If Not lo.HasError Then

            If dcProxyEspecies.Emisores.Count > 0 Then
                ListaEmisores = dcProxyEspecies.Emisores
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro de EMISORES", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EMISORES", _
                                             Me.ToString(), "TerminoTraerEmisores", Application.Current.ToString(), Program.Maquina, lo.Error)

        End If
        IsBusy = False
    End Sub
    Private Sub TerminoTraerCalificacionesEmisor(ByVal lo As LoadOperation(Of CalificacionesEmisor))
        If Not lo.HasError Then
            ListaCalificacionesEmisor = dcProxy.CalificacionesEmisors
            If dcProxy.CalificacionesEmisors.Count > 0 Then
                If lo.UserState = "insert" Then
                    CalificacionesEmisorSelected = ListaCalificacionesEmisor.LastOrDefault
                ElseIf lo.UserState = "update" Then
                    CalificacionesEmisorSelected = ListaCalificacionesEmisor.Where(Function(i) i.intIdCalificacionEmisor = intidCalificacionesEmisor).First
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    If ListaCalificacionesEmisor.Count = 0 Then
                        CalificacionesEmisorSelected = Nothing
                        'CalificacionesEmisorSelected = New Calificadoras
                        MyBase.CambioItem("CalificacionesEmisorSelected")
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

    Private Sub TerminoTraerCalificacionesEmisorPorDefecto_Completed(ByVal lo As LoadOperation(Of CalificacionesEmisor))
        If Not lo.HasError Then
            CalificacionesEmisorPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Calificadoras por defecto", _
                                             Me.ToString(), "TerminoTraerCalificadorasPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)

        End If
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            CalificacionesEmisorSelected = CalificacionesEmisorSelected
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If changes = True Then
                    dcProxy.RejectChanges()
                    changes = False

                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)
            dcProxyMaestrosCF.Calificadoras.Clear()
            MyBase.QuitarFiltroDespuesGuardar()
            If Not IsNothing(ListaCalificacionesEmisor) Then
                ListaCalificacionesEmisor.Clear()
            End If
            dcProxy.Load(dcProxy.CalificacionesEmisorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificacionesEmisor, So.UserState)
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

    Private _ListaCalificacionInversion As EntitySet(Of CalificacionesInversiones)
    Public Property ListaCalificacionInversion() As EntitySet(Of CalificacionesInversiones)
        Get
            Return _ListaCalificacionInversion
        End Get
        Set(ByVal value As EntitySet(Of CalificacionesInversiones))
            _ListaCalificacionInversion = value
            MyBase.CambioItem("ListaCalificacionInversion")
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



    Private _ListaCalificacionesEmisor As EntitySet(Of CalificacionesEmisor)
    Public Property ListaCalificacionesEmisor() As EntitySet(Of CalificacionesEmisor)
        Get
            Return _ListaCalificacionesEmisor
        End Get
        Set(ByVal value As EntitySet(Of CalificacionesEmisor))
            _ListaCalificacionesEmisor = value

            MyBase.CambioItem("ListaCalificacionesEmisor")
            MyBase.CambioItem("ListaCalificacionesEmisorPaged")

            If Not IsNothing(value) Then
                If IsNothing(CalificacionesEmisorAnterior) Then
                    CalificacionesEmisorSelected = _ListaCalificacionesEmisor.FirstOrDefault
                Else
                    CalificacionesEmisorSelected = CalificacionesEmisorAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCalificacionesEmisorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCalificacionesEmisor) Then
                Dim view = New PagedCollectionView(_ListaCalificacionesEmisor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CalificacionesEmisorSelected As CalificacionesEmisor
    Public Property CalificacionesEmisorSelected() As CalificacionesEmisor
        Get
            Return _CalificacionesEmisorSelected
        End Get
        Set(ByVal value As CalificacionesEmisor)
            _CalificacionesEmisorSelected = value
            MyBase.CambioItem("CalificacionesEmisorSelected")
        End Set
    End Property

#End Region

#Region "METODOS"
    Public Overrides Sub NuevoRegistro()
        Try

            Dim NewCalificadora As New CalificacionesEmisor
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCalificadora.intIdEmisor = CalificacionesEmisorPorDefecto.intIdCalificacionEmisor
            NewCalificadora.intIdCalificacionInversion = CalificacionesEmisorPorDefecto.intIdCalificacionInversion
            NewCalificadora.logTransmitirASuper = CalificacionesEmisorPorDefecto.logTransmitirASuper
            NewCalificadora.strUsuario = Program.Usuario
            NewCalificadora.dtmActualizacion = CalificacionesEmisorPorDefecto.dtmActualizacion
            NewCalificadora.dtmFechaCalificacionEmisor = DateTime.Today
            CalificacionesEmisorAnterior = CalificacionesEmisorSelected
            CalificacionesEmisorSelected = NewCalificadora
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
        If Not IsNothing(_CalificacionesEmisorSelected) Then
            Editando = True
        End If
    End Sub
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CalificacionesEmisorSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _CalificacionesEmisorSelected.EntityState = EntityState.Detached Then
                    CalificacionesEmisorSelected = CalificacionesEmisorAnterior
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
            cb = New CamposBusquedaCalificacionesEmisor
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
            If Not IsNothing(_CalificacionesEmisorSelected) Then
                dcProxy.CalificacionesEmisors.Remove(_CalificacionesEmisorSelected)
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
            CalificacionesEmisorAnterior = CalificacionesEmisorSelected
            If Not ListaCalificacionesEmisor.Contains(CalificacionesEmisorSelected) Then
                origen = "insert"
                ListaCalificacionesEmisor.Add(CalificacionesEmisorSelected)
            Else
                intidCalificacionesEmisor = CalificacionesEmisorSelected.intIdCalificacionEmisor
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
            If cb.intIdEmisor <> 0 Or cb.intIdCalificacionEmisor <> 0 Or cb.intIdCalificacionInversion <> 0 Then                'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CalificacionesEmisors.Clear()
                CalificacionesEmisorAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.CalificacionesEmisorConsultarQuery(cb.intIdCalificacionEmisor, cb.intIdEmisor, cb.intIdCalificacionInversion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificacionesEmisor, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCalificacionesEmisor
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
            dcProxy.CalificacionesEmisors.Clear()
            If Not IsNothing(ListaCalificacionesEmisor) Then
                ListaCalificacionesEmisor.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CalificacionesEmisorFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificacionesEmisor, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CalificacionesEmisorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificacionesEmisor, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaCalificacionesEmisor
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
Public Class CamposBusquedaCalificacionesEmisor
    <Display(Name:="Id calificación emisor")> _
    Public Property intIdCalificacionEmisor As Integer

    <Display(Name:="Id calificación inversión")> _
    Public Property intIdCalificacionInversion As Integer

    <Display(Name:="Emisor")> _
    Public Property intIdEmisor As Integer


End Class
