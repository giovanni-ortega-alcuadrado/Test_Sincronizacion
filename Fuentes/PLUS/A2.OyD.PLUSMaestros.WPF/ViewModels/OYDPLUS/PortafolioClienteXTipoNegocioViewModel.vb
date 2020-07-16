Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestros
Imports OpenRiaServices.DomainServices.Client

Public Class PortafolioClienteXTipoNegocioViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ClienteRelacionadoPorDefecto As PortalioClienteXTipoNegocio
    Private PortafolioClienteAnterior As PortalioClienteXTipoNegocioPrincipal
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim dcProxy2 As OyDPLUSMaestrosDomainContext
    Dim objProxy As OyDPLUSutilidadesDomainContext
    Dim IDItemTipoProducto As String = String.Empty
    Dim IDItemPerfilRiesgo As String = String.Empty
    Dim IDItemCodigoOYD As String = String.Empty

    Dim logRefrescarDetalle As Boolean = False
    Dim logEditarRegistro As Boolean = False

    Public Sub New()
        Try

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
                dcProxy2 = New OyDPLUSMaestrosDomainContext()
                objProxy = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy2 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                objProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.PortafolioClientesXTipoNegocioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolioClientes, "")
                dcProxy2.Load(dcProxy2.TraerPortalioClienteXTipoNegocioPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolioClientePorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PortafolioClienteXTipoNegocioViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerPortafolioClientePorDefecto_Completed(ByVal lo As LoadOperation(Of PortalioClienteXTipoNegocio))
        If Not lo.HasError Then
            ClienteRelacionadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClientesRelacionados por defecto", _
                                             Me.ToString(), "TerminoTraerClientesRelacionadosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerPortafolioClientes(ByVal lo As LoadOperation(Of PortalioClienteXTipoNegocioPrincipal))
        If Not lo.HasError Then
            If lo.UserState = "actualizar" Then
                logRefrescarDetalle = False
            Else
                logRefrescarDetalle = True
            End If

            ListaPortafolioCliente = lo.Entities.ToList

            If _ListaPortafolioCliente.Count > 0 Then
                If lo.UserState = "actualizar" Then
                    logRefrescarDetalle = True
                    If _ListaPortafolioCliente.Where(Function(i) i.TipoProducto = IDItemTipoProducto And i.PerfilRiesgo = IDItemPerfilRiesgo And i.CodigoOYD = IDItemCodigoOYD).Count > 0 Then
                        PortafolioClienteSeleccionado = _ListaPortafolioCliente.Where(Function(i) i.TipoProducto = IDItemTipoProducto And i.PerfilRiesgo = IDItemPerfilRiesgo And i.CodigoOYD = IDItemCodigoOYD).FirstOrDefault
                    Else
                        PortafolioClienteSeleccionado = _ListaPortafolioCliente.First
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesRelacionados", _
                                             Me.ToString(), "TerminoTraerClientesRelacionados", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerPortafolioClientesXTipoNegocio(ByVal lo As LoadOperation(Of PortalioClienteXTipoNegocio))
        Try
            If Not lo.HasError Then
                ListaPortafolioClientesXTipoNegocio = dcProxy1.PortalioClienteXTipoNegocios
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de portafolio cliente x tipo de negocio", _
                                                 Me.ToString(), "TerminoTraerPortafolioClientesXTipoNegocio", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de portafolio cliente x tipo de negocio", _
                                                Me.ToString(), "TerminoTraerPortafolioClientesXTipoNegocio", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try
    End Sub

    Private Sub TerminoValidarRegistro(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.ValidacionEliminarRegistro))
        Try
            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.UserState = "ELIMINAR" Then
                        IsBusy = False
                    Else
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            NuevoDetalle()
                            IsBusy = False
                        Else
                            IsBusy = False
                            mostrarMensaje("La combinación del encabezado ya existe por favor modifique los valores para poder crear los detalles.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.", _
                                                 Me.ToString(), "TerminoValidarRegistro", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.", _
                                                 Me.ToString(), "TerminoValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaPortafolioCliente As List(Of PortalioClienteXTipoNegocioPrincipal)
    Public Property ListaPortafolioCliente() As List(Of PortalioClienteXTipoNegocioPrincipal)
        Get
            Return _ListaPortafolioCliente
        End Get
        Set(ByVal value As List(Of PortalioClienteXTipoNegocioPrincipal))
            _ListaPortafolioCliente = value

            MyBase.CambioItem("ListaPortafolioCliente")
            MyBase.CambioItem("ListaPortafolioClientePaged")
            If Not IsNothing(_ListaPortafolioCliente) Then
                If _ListaPortafolioCliente.Count > 0 Then
                    PortafolioClienteSeleccionado = _ListaPortafolioCliente.First
                Else
                    PortafolioClienteSeleccionado = Nothing
                End If
            Else
                PortafolioClienteSeleccionado = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaPortafolioClientePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPortafolioCliente) Then
                Dim view = New PagedCollectionView(_ListaPortafolioCliente)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _PortafolioClienteSeleccionado As PortalioClienteXTipoNegocioPrincipal
    Public Property PortafolioClienteSeleccionado() As PortalioClienteXTipoNegocioPrincipal
        Get
            Return _PortafolioClienteSeleccionado
        End Get
        Set(ByVal value As PortalioClienteXTipoNegocioPrincipal)
            _PortafolioClienteSeleccionado = value
            If Not IsNothing(_PortafolioClienteSeleccionado) And logRefrescarDetalle Then
                ConsultarRelaciones(_PortafolioClienteSeleccionado)
            End If
            MyBase.CambioItem("PortafolioClienteSeleccionado")
        End Set
    End Property

    Private _ListaPortafolioClientesXTipoNegocio As EntitySet(Of PortalioClienteXTipoNegocio)
    Public Property ListaPortafolioClientesXTipoNegocio() As EntitySet(Of PortalioClienteXTipoNegocio)
        Get
            Return _ListaPortafolioClientesXTipoNegocio
        End Get
        Set(ByVal value As EntitySet(Of PortalioClienteXTipoNegocio))
            _ListaPortafolioClientesXTipoNegocio = value
            MyBase.CambioItem("ListaPortafolioClientesXTipoNegocio")
            MyBase.CambioItem("ListaPortafolioClientesXTipoNegocioPaged")
            If Not IsNothing(_ListaPortafolioClientesXTipoNegocio) Then
                If _ListaPortafolioClientesXTipoNegocio.Count > 0 Then
                    PortafolioClientesXTipoNegocio = _ListaPortafolioClientesXTipoNegocio.First
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaPortafolioClientesXTipoNegocioPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPortafolioClientesXTipoNegocio) Then
                Dim view = New PagedCollectionView(_ListaPortafolioClientesXTipoNegocio)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _PortafolioClientesXTipoNegocio As PortalioClienteXTipoNegocio
    Public Property PortafolioClientesXTipoNegocio() As PortalioClienteXTipoNegocio
        Get
            Return _PortafolioClientesXTipoNegocio
        End Get
        Set(ByVal value As PortalioClienteXTipoNegocio)
            _PortafolioClientesXTipoNegocio = value
            MyBase.CambioItem("PortafolioClientesXTipoNegocio")
        End Set
    End Property

    Private WithEvents _cb As CamposBusquedaPortafolioClientes = New CamposBusquedaPortafolioClientes
    Public Property cb() As CamposBusquedaPortafolioClientes
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaPortafolioClientes)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _LimpiarItem As Boolean = False
    Public Property LimpiarItem() As Boolean
        Get
            Return _LimpiarItem
        End Get
        Set(ByVal value As Boolean)
            _LimpiarItem = value
            MyBase.CambioItem("LimpiarItem")
        End Set
    End Property

    Private _HabilitarEdicion As Boolean = True
    Public Property HabilitarEdicion() As Boolean
        Get
            Return _HabilitarEdicion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicion = value
            MyBase.CambioItem("HabilitarEdicion")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewPortafolioCliente As New PortalioClienteXTipoNegocioPrincipal
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewPortafolioCliente.TipoProducto = "TOD"
            NewPortafolioCliente.NombreTipoProducto = "Todos"
            NewPortafolioCliente.PerfilRiesgo = "TOD"
            NewPortafolioCliente.NombrePerfilRiesgo = "Todos"
            NewPortafolioCliente.CodigoOYD = "TOD"
            NewPortafolioCliente.NombreCodigoOYD = "Todos"

            ObtenerRegistroAnterior()
            logRefrescarDetalle = False
            PortafolioClienteSeleccionado = NewPortafolioCliente
            logRefrescarDetalle = True
            Editando = True
            MyBase.CambioItem("Editando")
            LimpiarItem = True

            If Not IsNothing(dcProxy1.PortalioClienteXTipoNegocioPrincipals) Then
                dcProxy1.PortalioClienteXTipoNegocioPrincipals.Clear()
            End If

            If Not IsNothing(ListaPortafolioClientesXTipoNegocio) Then
                ListaPortafolioClientesXTipoNegocio.Clear()
            End If

            MyBase.CambioItem("ListaPortafolioClientesXTipoNegocio")

            logEditarRegistro = False
            HabilitarEdicion = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.PortalioClienteXTipoNegocioPrincipals.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.PortafolioClientesXTipoNegocioFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolioClientes, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.PortafolioClientesXTipoNegocioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolioClientes, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            cb = New CamposBusquedaPortafolioClientes()
            cb.TipoProducto = String.Empty
            cb.PerfilRiesgo = String.Empty

            MyBase.CambioItem("cb")
            MyBase.Buscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación la busqueda", _
                                             Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not IsNothing(_cb.TipoProducto) Or Not String.IsNullOrEmpty(_cb.PerfilRiesgo) Or Not IsNothing(_cb.CodigoOYD) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""

                PortafolioClienteAnterior = Nothing
                IsBusy = True

                If Not IsNothing(dcProxy.PortalioClienteXTipoNegocioPrincipals) Then
                    dcProxy.PortalioClienteXTipoNegocioPrincipals.Clear()
                End If

                dcProxy.Load(dcProxy.PortafolioClientesXTipoNegocioConsultarQuery(_cb.TipoProducto, _cb.PerfilRiesgo, _cb.CodigoOYD, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolioClientes, "Busqueda")

                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaPortafolioClientes
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
            If IsNothing(_ListaPortafolioClientesXTipoNegocio) Then
                mostrarMensaje("Debe de ingresar al menos un detalle para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            Else
                If _ListaPortafolioClientesXTipoNegocio.Count = 0 Then
                    mostrarMensaje("Debe de ingresar al menos un detalle para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    'Valida que no halla un tipo de negocio repetido
                    For Each li In _ListaPortafolioClientesXTipoNegocio
                        If li.TipoNegocio = "TOD" Then
                            mostrarMensaje("Debe seleccionar todos los tipos de negocio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                        If _ListaPortafolioClientesXTipoNegocio.Where(Function(i) i.TipoNegocio = li.TipoNegocio).Count > 1 Then
                            mostrarMensaje("Hay tipos de negocio repetidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    Next
                    'Valida la suma de los porcentajes
                    Dim intPorcentajes As Double = 0

                    For Each li In _ListaPortafolioClientesXTipoNegocio
                        If li.TipoNegocio <> "CG" Then
                            intPorcentajes += li.Porcentaje
                        End If
                    Next

                    If intPorcentajes > 100 Then
                        mostrarMensaje("La suma de los porcentajes diferentes a Cupo Global no puede sumar más del 100%", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
            End If

            IsBusy = True

            For Each li In _ListaPortafolioClientesXTipoNegocio
                If li.TipoProducto <> _PortafolioClienteSeleccionado.TipoProducto Then
                    li.TipoProducto = _PortafolioClienteSeleccionado.TipoProducto
                End If

                If li.PerfilRiesgo <> _PortafolioClienteSeleccionado.PerfilRiesgo Then
                    li.PerfilRiesgo = _PortafolioClienteSeleccionado.PerfilRiesgo
                End If

                If li.CodigoOYD <> _PortafolioClienteSeleccionado.CodigoOYD Then
                    li.CodigoOYD = _PortafolioClienteSeleccionado.CodigoOYD
                    li.NombreCodigoOYD = _PortafolioClienteSeleccionado.NombreCodigoOYD
                End If
            Next

            Program.VerificarCambiosProxyServidor(dcProxy1)
            dcProxy1.SubmitChanges(AddressOf TerminoSubmitChanges, "actualizar")
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
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
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
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)

            LimpiarItem = False
            logEditarRegistro = False

            IDItemCodigoOYD = PortafolioClienteSeleccionado.CodigoOYD
            IDItemPerfilRiesgo = PortafolioClienteSeleccionado.PerfilRiesgo
            IDItemTipoProducto = PortafolioClienteSeleccionado.TipoProducto

            If Not IsNothing(dcProxy.PortalioClienteXTipoNegocioPrincipals) Then
                dcProxy.PortalioClienteXTipoNegocioPrincipals.Clear()
            End If

            MyBase.QuitarFiltroDespuesGuardar()
            dcProxy.Load(dcProxy.PortafolioClientesXTipoNegocioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolioClientes, So.UserState)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_PortafolioClienteSeleccionado) Then
                Editando = True
                ObtenerRegistroAnterior()
                LimpiarItem = True
                logEditarRegistro = True
                HabilitarEdicion = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro.", _
                                             Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_PortafolioClienteSeleccionado) Then
                dcProxy1.RejectChanges()
                PortafolioClienteSeleccionado = PortafolioClienteAnterior
            End If
            Editando = False
            LimpiarItem = False
            logEditarRegistro = False
            HabilitarEdicion = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_PortafolioClienteSeleccionado) Then
                'dcProxy.ClientesRelacionados.Remove(_ClienteRelacionadoSelected)
                If Not IsNothing(ListaPortafolioCliente) Then
                    Dim objListaPortafolioCliente As List(Of PortalioClienteXTipoNegocio)
                    objListaPortafolioCliente = ListaPortafolioClientesXTipoNegocio.ToList
                    For Each li In objListaPortafolioCliente
                        Dim objRemover = ListaPortafolioClientesXTipoNegocio.Where(Function(i) i.ID = li.ID).First
                        dcProxy1.PortalioClienteXTipoNegocios.Remove(objRemover)
                    Next
                End If

                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy1)
                dcProxy1.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objClientesRelacionados As New PortalioClienteXTipoNegocioPrincipal
            If Not IsNothing(_PortafolioClienteSeleccionado) Then
                objClientesRelacionados.ID = _PortafolioClienteSeleccionado.ID
                objClientesRelacionados.CodigoOYD = _PortafolioClienteSeleccionado.CodigoOYD
                objClientesRelacionados.NombreCodigoOYD = _PortafolioClienteSeleccionado.NombreCodigoOYD
                objClientesRelacionados.NombrePerfilRiesgo = _PortafolioClienteSeleccionado.NombrePerfilRiesgo
                objClientesRelacionados.NombreTipoProducto = _PortafolioClienteSeleccionado.NombreTipoProducto
                objClientesRelacionados.PerfilRiesgo = _PortafolioClienteSeleccionado.PerfilRiesgo
                objClientesRelacionados.TipoProducto = _PortafolioClienteSeleccionado.TipoProducto
            End If

            PortafolioClienteAnterior = Nothing
            PortafolioClienteAnterior = objClientesRelacionados
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarRelaciones(ByVal pobjClienteSeleccionado As PortalioClienteXTipoNegocioPrincipal, Optional ByVal pstrUserstate As String = "")
        Try
            If Not IsNothing(pobjClienteSeleccionado) Then
                If pobjClienteSeleccionado.ID <> 0 Then
                    If Not IsNothing(dcProxy1.PortalioClienteXTipoNegocios) Then
                        dcProxy1.PortalioClienteXTipoNegocios.Clear()
                    End If

                    dcProxy1.Load(dcProxy1.PortafolioClientesXTipoNegocioConsultar_RelacionesQuery(pobjClienteSeleccionado.TipoProducto, pobjClienteSeleccionado.PerfilRiesgo, pobjClienteSeleccionado.CodigoOYD, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolioClientesXTipoNegocio, pstrUserstate)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la información de los clientes relacionados.", _
             Me.ToString(), "ConsultarRelacionesCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub InsertarDatos(pobjItem As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjItem) Then
                PortafolioClienteSeleccionado.CodigoOYD = pobjItem.CodigoOYD
                PortafolioClienteSeleccionado.NombreCodigoOYD = pobjItem.Nombre

                If Not IsNothing(_ListaPortafolioClientesXTipoNegocio) Then
                    For Each li In ListaPortafolioClientesXTipoNegocio
                        li.CodigoOYD = PortafolioClienteSeleccionado.CodigoOYD
                        li.NombreCodigoOYD = PortafolioClienteSeleccionado.NombreCodigoOYD
                    Next
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar los datos del buscador de clientes.", _
             Me.ToString(), "InsertarDatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub InsertarDatosBusqueda(pobjItem As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjItem) Then
                cb.CodigoOYD = pobjItem.CodigoOYD
                cb.Nombre = pobjItem.Nombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar los datos del buscador de clientes.", _
             Me.ToString(), "InsertarDatosBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Detalles Maestros del Tipo negocio"

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmPortafolioCliente"
                    If Not IsNothing(PortafolioClienteSeleccionado) Then
                        If Not String.IsNullOrEmpty(_PortafolioClienteSeleccionado.CodigoOYD) Or Not String.IsNullOrEmpty(_PortafolioClienteSeleccionado.TipoProducto) Or Not String.IsNullOrEmpty(_PortafolioClienteSeleccionado.PerfilRiesgo) Then
                            Dim logValidar As Boolean = False

                            If Not IsNothing(_ListaPortafolioClientesXTipoNegocio) Then
                                If ListaPortafolioClientesXTipoNegocio.Count = 0 And HabilitarEdicion Then
                                    logValidar = True
                                End If
                            Else
                                If HabilitarEdicion Then
                                    logValidar = True
                                End If
                            End If

                            If logValidar Then
                                IsBusy = True

                                If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                                    objProxy.ValidacionEliminarRegistros.Clear()
                                End If
                                Dim ValorTipoProducto As String = "NULL"
                                Dim ValorPerfilRiesgo As String = "NULL"
                                Dim ValorCodigoOYD As String = "NULL"

                                If Not String.IsNullOrEmpty(_PortafolioClienteSeleccionado.TipoProducto) And _PortafolioClienteSeleccionado.TipoProducto <> "TOD" Then
                                    ValorTipoProducto = _PortafolioClienteSeleccionado.TipoProducto
                                End If

                                If Not String.IsNullOrEmpty(_PortafolioClienteSeleccionado.PerfilRiesgo) And _PortafolioClienteSeleccionado.PerfilRiesgo <> "TOD" Then
                                    ValorPerfilRiesgo = _PortafolioClienteSeleccionado.PerfilRiesgo
                                End If

                                If Not String.IsNullOrEmpty(_PortafolioClienteSeleccionado.CodigoOYD) And _PortafolioClienteSeleccionado.CodigoOYD <> "TOD" Then
                                    ValorCodigoOYD = _PortafolioClienteSeleccionado.CodigoOYD
                                End If

                                objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblPortalioClienteXTipoNegocio", "'strTipoProducto'|'strPerfilRiesgo'|'strCodigoOYD'", String.Format("'{0}'|'{1}'|'{2}'", ValorTipoProducto, ValorPerfilRiesgo, ValorCodigoOYD), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
                            Else
                                NuevoDetalle()
                            End If

                        Else
                            mostrarMensaje("Debe de seleccionar el tipo de producto, el perfil de riesgo o el cliente para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        mostrarMensaje("Debe de seleccionar el tipo de producto, el perfil de riesgo o el cliente para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If

            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar el nuevo detalle.", _
                                                         Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmPortafolioCliente"
                    If Not IsNothing(ListaPortafolioClientesXTipoNegocio) Then
                        If Not IsNothing(_PortafolioClientesXTipoNegocio) Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(PortafolioClientesXTipoNegocio, ListaPortafolioClientesXTipoNegocio.ToList)

                            If ListaPortafolioClientesXTipoNegocio.Contains(_PortafolioClientesXTipoNegocio) Then
                                ListaPortafolioClientesXTipoNegocio.Remove(_PortafolioClientesXTipoNegocio)
                            End If
                            If ListaPortafolioClientesXTipoNegocio.Count > 0 Then
                                Program.PosicionarItemLista(PortafolioClientesXTipoNegocio, ListaPortafolioClientesXTipoNegocio.ToList, intRegistroPosicionar)
                            Else
                                PortafolioClientesXTipoNegocio = Nothing
                            End If
                        End If
                        MyBase.CambioItem("PortafolioClientesXTipoNegocio")
                        MyBase.CambioItem("ListaPortafolioClientesXTipoNegocio")
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle.", _
                                                         Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub NuevoDetalle()
        Try
            Dim NewPortafolioCliente As New PortalioClienteXTipoNegocio
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewPortafolioCliente.ID = -New Random().Next()
            NewPortafolioCliente.CodigoOYD = IIf(String.IsNullOrEmpty(PortafolioClienteSeleccionado.CodigoOYD), Nothing, PortafolioClienteSeleccionado.CodigoOYD)
            NewPortafolioCliente.NombreCodigoOYD = IIf(String.IsNullOrEmpty(PortafolioClienteSeleccionado.CodigoOYD), "TODOS", PortafolioClienteSeleccionado.NombreCodigoOYD)
            NewPortafolioCliente.PerfilRiesgo = IIf(PortafolioClienteSeleccionado.PerfilRiesgo = "TOD", Nothing, PortafolioClienteSeleccionado.PerfilRiesgo)
            NewPortafolioCliente.TipoProducto = IIf(PortafolioClienteSeleccionado.TipoProducto = "TOD", Nothing, PortafolioClienteSeleccionado.TipoProducto)
            NewPortafolioCliente.TipoNegocio = "TOD"
            NewPortafolioCliente.Porcentaje = 0
            NewPortafolioCliente.ValorMaximoCupo = 0
            NewPortafolioCliente.Usuario = Program.Usuario

            If IsNothing(ListaPortafolioClientesXTipoNegocio) Then
                ListaPortafolioClientesXTipoNegocio = dcProxy1.PortalioClienteXTipoNegocios
            End If

            ListaPortafolioClientesXTipoNegocio.Add(NewPortafolioCliente)
            PortafolioClientesXTipoNegocio = NewPortafolioCliente

            MyBase.CambioItem("ListaPortafolioClientesXTipoNegocio")
            MyBase.CambioItem("PortafolioClientesXTipoNegocio")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar el nuevo registro.", _
                                                         Me.ToString(), "NuevoDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    Private Sub _PortafolioClienteSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _PortafolioClientesXTipoNegocio.PropertyChanged
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_PortafolioClienteSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaPortafolioClientes
    Implements INotifyPropertyChanged

    Private _TipoProducto As String
    <Display(Name:="Tipo producto", Description:="Tipo producto")> _
    Public Property TipoProducto() As String
        Get
            Return _TipoProducto
        End Get
        Set(ByVal value As String)
            _TipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoProducto"))
        End Set
    End Property

    Private _PerfilRiesgo As String
    <Display(Name:="Perfil riesgo", Description:="Perfil riesgo")> _
    Public Property PerfilRiesgo() As String
        Get
            Return _PerfilRiesgo
        End Get
        Set(ByVal value As String)
            _PerfilRiesgo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PerfilRiesgo"))
        End Set
    End Property

    Private _CodigoOYD As String
    <Display(Name:="Código OYD", Description:="Código OYD")> _
    Public Property CodigoOYD() As String
        Get
            Return _CodigoOYD
        End Get
        Set(ByVal value As String)
            _CodigoOYD = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoOYD"))
        End Set
    End Property

    Private _Nombre As String
    <Display(Name:="Nombre", Description:="Nombre")> _
    Public Property Nombre() As String
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