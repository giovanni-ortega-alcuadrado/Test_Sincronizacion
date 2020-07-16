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

Public Class CupoReceptorXTipoNegocioViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ClienteRelacionadoPorDefecto As CupoReceptorXTipoNegocio
    Private CupoReceptorAnterior As CupoReceptorXTipoNegocioPrincipal
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim dcProxy2 As OyDPLUSMaestrosDomainContext
    Dim objProxy As OyDPLUSutilidadesDomainContext

    Dim IDItemSucursal As Integer = 0
    Dim IDItemMesa As Integer = 0
    Dim IDItemReceptor As String = String.Empty

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
                dcProxy.Load(dcProxy.CupoReceptorXTipoNegocioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCupoReceptor, "")
                dcProxy2.Load(dcProxy2.TraerCupoReceptorXTipoNegocioPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCupoReceptorPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CupoReceptorXTipoNegocioViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCupoReceptorPorDefecto_Completed(ByVal lo As LoadOperation(Of CupoReceptorXTipoNegocio))
        If Not lo.HasError Then
            ClienteRelacionadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la CupoReceptor por defecto", _
                                             Me.ToString(), "TerminoTraerCupoReceptorPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCupoReceptor(ByVal lo As LoadOperation(Of CupoReceptorXTipoNegocioPrincipal))
        If Not lo.HasError Then
            If lo.UserState = "actualizar" Then
                logRefrescarDetalle = False
            Else
                logRefrescarDetalle = True
            End If

            ListaCupoReceptor = lo.Entities.ToList

            If _ListaCupoReceptor.Count > 0 Then
                If lo.UserState = "actualizar" Then
                    logRefrescarDetalle = True
                    If _ListaCupoReceptor.Where(Function(i) i.IDSucursal = IDItemSucursal And i.IDMesa = IDItemMesa And i.IDReceptor = IDItemReceptor).Count > 0 Then
                        CupoReceptorSeleccionado = _ListaCupoReceptor.Where(Function(i) i.IDSucursal = IDItemSucursal And i.IDMesa = IDItemMesa And i.IDReceptor = IDItemReceptor).FirstOrDefault
                    Else
                        CupoReceptorSeleccionado = _ListaCupoReceptor.First
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CupoReceptor", _
                                             Me.ToString(), "TerminoTraerCupoReceptor", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerCupoReceptorXTipoNegocio(ByVal lo As LoadOperation(Of CupoReceptorXTipoNegocio))
        Try
            If Not lo.HasError Then
                ListaCupoReceptorXTipoNegocio = dcProxy1.CupoReceptorXTipoNegocios
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cupo receptor x tipo de negocio", _
                                                 Me.ToString(), "TerminoTraerCupoReceptorXTipoNegocio", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cupo receptor x tipo de negocio", _
                                                Me.ToString(), "TerminoTraerCupoReceptorXTipoNegocio", Application.Current.ToString(), Program.Maquina, lo.Error)
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

    Private _ListaCupoReceptor As List(Of CupoReceptorXTipoNegocioPrincipal)
    Public Property ListaCupoReceptor() As List(Of CupoReceptorXTipoNegocioPrincipal)
        Get
            Return _ListaCupoReceptor
        End Get
        Set(ByVal value As List(Of CupoReceptorXTipoNegocioPrincipal))
            _ListaCupoReceptor = value

            MyBase.CambioItem("ListaCupoReceptor")
            MyBase.CambioItem("ListaCupoReceptorPaged")
            If Not IsNothing(_ListaCupoReceptor) Then
                If _ListaCupoReceptor.Count > 0 Then
                    CupoReceptorSeleccionado = _ListaCupoReceptor.First
                Else
                    CupoReceptorSeleccionado = Nothing
                End If
            Else
                CupoReceptorSeleccionado = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCupoReceptorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCupoReceptor) Then
                Dim view = New PagedCollectionView(_ListaCupoReceptor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _CupoReceptorSeleccionado As CupoReceptorXTipoNegocioPrincipal
    Public Property CupoReceptorSeleccionado() As CupoReceptorXTipoNegocioPrincipal
        Get
            Return _CupoReceptorSeleccionado
        End Get
        Set(ByVal value As CupoReceptorXTipoNegocioPrincipal)
            _CupoReceptorSeleccionado = value
            If Not IsNothing(_CupoReceptorSeleccionado) And logRefrescarDetalle Then
                ConsultarRelaciones(_CupoReceptorSeleccionado)
                SucursalMesa = String.Format("{0}-{1}", _CupoReceptorSeleccionado.IDSucursal, _CupoReceptorSeleccionado.IDMesa)
            End If
            MyBase.CambioItem("CupoReceptorSeleccionado")
        End Set
    End Property

    Private _ListaCupoReceptorXTipoNegocio As EntitySet(Of CupoReceptorXTipoNegocio)
    Public Property ListaCupoReceptorXTipoNegocio() As EntitySet(Of CupoReceptorXTipoNegocio)
        Get
            Return _ListaCupoReceptorXTipoNegocio
        End Get
        Set(ByVal value As EntitySet(Of CupoReceptorXTipoNegocio))
            _ListaCupoReceptorXTipoNegocio = value
            MyBase.CambioItem("ListaCupoReceptorXTipoNegocio")
            MyBase.CambioItem("ListaCupoReceptorXTipoNegocioPaged")
            If Not IsNothing(_ListaCupoReceptorXTipoNegocio) Then
                If _ListaCupoReceptorXTipoNegocio.Count > 0 Then
                    CupoReceptorXTipoNegocio = _ListaCupoReceptorXTipoNegocio.First
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCupoReceptorXTipoNegocioPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCupoReceptorXTipoNegocio) Then
                Dim view = New PagedCollectionView(_ListaCupoReceptorXTipoNegocio)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _CupoReceptorXTipoNegocio As CupoReceptorXTipoNegocio
    Public Property CupoReceptorXTipoNegocio() As CupoReceptorXTipoNegocio
        Get
            Return _CupoReceptorXTipoNegocio
        End Get
        Set(ByVal value As CupoReceptorXTipoNegocio)
            _CupoReceptorXTipoNegocio = value
            MyBase.CambioItem("CupoReceptorXTipoNegocio")
        End Set
    End Property

    Private WithEvents _cb As CamposBusquedaCopoReceptor = New CamposBusquedaCopoReceptor
    Public Property cb() As CamposBusquedaCopoReceptor
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaCopoReceptor)
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

    Private _HabilitarEdicion As Boolean = False
    Public Property HabilitarEdicion() As Boolean
        Get
            Return _HabilitarEdicion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicion = value
            MyBase.CambioItem("HabilitarEdicion")
        End Set
    End Property

    Private _SucursalMesa As String
    Public Property SucursalMesa() As String
        Get
            Return _SucursalMesa
        End Get
        Set(ByVal value As String)
            _SucursalMesa = value
            MyBase.CambioItem("SucursalMesa")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim NewCupoReceptor As New CupoReceptorXTipoNegocioPrincipal
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCupoReceptor.IDSucursal = 0
            NewCupoReceptor.NombreSucursal = "Todas"
            NewCupoReceptor.IDMesa = 0
            NewCupoReceptor.NombreMesa = "Todas"
            NewCupoReceptor.IDReceptor = "TOD"
            NewCupoReceptor.NombreReceptor = "Todos"

            ObtenerRegistroAnterior()
            logRefrescarDetalle = False
            CupoReceptorSeleccionado = NewCupoReceptor
            logRefrescarDetalle = True
            Editando = True
            MyBase.CambioItem("Editando")
            LimpiarItem = True

            If Not IsNothing(dcProxy1.PortalioClienteXTipoNegocioPrincipals) Then
                dcProxy1.CupoReceptorXTipoNegocioPrincipals.Clear()
            End If

            If Not IsNothing(ListaCupoReceptorXTipoNegocio) Then
                ListaCupoReceptorXTipoNegocio.Clear()
            End If

            MyBase.CambioItem("ListaCupoReceptorXTipoNegocio")

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
                dcProxy.Load(dcProxy.CupoReceptorXTipoNegocioFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCupoReceptor, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CupoReceptorXTipoNegocioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCupoReceptor, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            cb = New CamposBusquedaCopoReceptor()
            MyBase.CambioItem("cb")
            MyBase.Buscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación la busqueda", _
                                             Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not IsNothing(_cb.Sucursal) Or Not IsNothing(_cb.Mesa) Or Not String.IsNullOrEmpty(_cb.Receptor) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""

                CupoReceptorAnterior = Nothing
                IsBusy = True

                If Not IsNothing(dcProxy.CupoReceptorXTipoNegocioPrincipals) Then
                    dcProxy.CupoReceptorXTipoNegocioPrincipals.Clear()
                End If

                dcProxy.Load(dcProxy.CupoReceptorXTipoNegocioConsultarQuery(_cb.Sucursal, _cb.Mesa, _cb.Receptor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCupoReceptor, "Busqueda")

                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCopoReceptor
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
            If IsNothing(_ListaCupoReceptorXTipoNegocio) Then
                mostrarMensaje("Debe de ingresar al menos un detalle para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            Else
                If _ListaCupoReceptorXTipoNegocio.Count = 0 Then
                    mostrarMensaje("Debe de ingresar al menos un detalle para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    'Valida que no halla un tipo de negocio repetido
                    For Each li In _ListaCupoReceptorXTipoNegocio
                        If li.TipoNegocio = "TOD" Then
                            mostrarMensaje("Debe seleccionar todos los tipos de negocio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                        If _ListaCupoReceptorXTipoNegocio.Where(Function(i) i.TipoNegocio = li.TipoNegocio).Count > 1 Then
                            mostrarMensaje("Hay tipos de negocio repetidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    Next
                    'Valida la suma de los porcentajes
                    Dim dblPorcentajes As Double = 0

                    For Each li In _ListaCupoReceptorXTipoNegocio
                        dblPorcentajes += li.PorcentajeCupo
                    Next

                    If dblPorcentajes > 100 Then
                        mostrarMensaje("La suma de los porcentajes no puede sumar más del 100%", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
            End If

            IsBusy = True

            'Antes de guardar se estan validan que los datos del encabezado sean iguales al del detalle
            For Each li In _ListaCupoReceptorXTipoNegocio
                If li.IDSucursal <> _CupoReceptorSeleccionado.IDSucursal Then
                    li.IDSucursal = _CupoReceptorSeleccionado.IDSucursal
                End If

                If li.IDMesa <> _CupoReceptorSeleccionado.IDMesa Then
                    li.IDMesa = _CupoReceptorSeleccionado.IDMesa
                End If

                If li.IDReceptor <> _CupoReceptorSeleccionado.IDReceptor Then
                    li.IDReceptor = _CupoReceptorSeleccionado.IDReceptor
                    li.NombreIDReceptor = _CupoReceptorSeleccionado.NombreReceptor
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
            HabilitarEdicion = False

            IDItemSucursal = _CupoReceptorSeleccionado.IDSucursal
            IDItemMesa = _CupoReceptorSeleccionado.IDMesa
            IDItemReceptor = _CupoReceptorSeleccionado.IDReceptor

            MyBase.QuitarFiltroDespuesGuardar()
            If Not IsNothing(dcProxy.PortalioClienteXTipoNegocios) Then
                dcProxy.PortalioClienteXTipoNegocios.Clear()
            End If

            dcProxy.Load(dcProxy.CupoReceptorXTipoNegocioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCupoReceptor, So.UserState)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        Try
            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_CupoReceptorSeleccionado) Then
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
            If Not IsNothing(_CupoReceptorSeleccionado) Then
                dcProxy1.RejectChanges()
                CupoReceptorSeleccionado = CupoReceptorAnterior
            End If
            Editando = False
            LimpiarItem = False
            logEditarRegistro = False
            HabilitarEdicion = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_CupoReceptorSeleccionado) Then
                'dcProxy.ClientesRelacionados.Remove(_ClienteRelacionadoSelected)
                If Not IsNothing(_ListaCupoReceptor) Then
                    Dim objListaCupoReceptor As List(Of CupoReceptorXTipoNegocio)
                    objListaCupoReceptor = ListaCupoReceptorXTipoNegocio.ToList
                    For Each li In objListaCupoReceptor
                        Dim objRemover = ListaCupoReceptorXTipoNegocio.Where(Function(i) i.ID = li.ID).First
                        dcProxy1.CupoReceptorXTipoNegocios.Remove(objRemover)
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
            Dim objCupoReceptor As New CupoReceptorXTipoNegocioPrincipal
            If Not IsNothing(_CupoReceptorSeleccionado) Then
                objCupoReceptor.ID = _CupoReceptorSeleccionado.ID
                objCupoReceptor.IDSucursal = _CupoReceptorSeleccionado.IDSucursal
                objCupoReceptor.IDMesa = _CupoReceptorSeleccionado.IDMesa
                objCupoReceptor.IDReceptor = _CupoReceptorSeleccionado.IDReceptor
                objCupoReceptor.NombreSucursal = _CupoReceptorSeleccionado.NombreSucursal
                objCupoReceptor.NombreMesa = _CupoReceptorSeleccionado.NombreMesa
                objCupoReceptor.NombreReceptor = _CupoReceptorSeleccionado.NombreReceptor
            End If

            CupoReceptorAnterior = Nothing
            CupoReceptorAnterior = objCupoReceptor
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarRelaciones(ByVal pobjCupoReceptorSeleccionado As CupoReceptorXTipoNegocioPrincipal, Optional ByVal pstrUserstate As String = "")
        Try
            If Not IsNothing(pobjCupoReceptorSeleccionado) Then
                If pobjCupoReceptorSeleccionado.ID <> 0 Then
                    If Not IsNothing(dcProxy1.CupoReceptorXTipoNegocios) Then
                        dcProxy1.CupoReceptorXTipoNegocios.Clear()
                    End If

                    dcProxy1.Load(dcProxy1.CupoReceptorXTipoNegocioConsultar_RelacionesQuery(pobjCupoReceptorSeleccionado.IDSucursal, pobjCupoReceptorSeleccionado.IDMesa, pobjCupoReceptorSeleccionado.IDReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCupoReceptorXTipoNegocio, pstrUserstate)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la información de los clientes relacionados.", _
             Me.ToString(), "ConsultarRelacionesReceptor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub InsertarDatos(pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                _CupoReceptorSeleccionado.IDReceptor = pobjItem.IdItem
                _CupoReceptorSeleccionado.NombreReceptor = pobjItem.Nombre

                If Not IsNothing(_ListaCupoReceptorXTipoNegocio) Then
                    For Each li In ListaCupoReceptorXTipoNegocio
                        li.IDReceptor = _CupoReceptorSeleccionado.IDReceptor
                        li.NombreIDReceptor = _CupoReceptorSeleccionado.NombreReceptor
                    Next
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar los datos del buscador de clientes.", _
             Me.ToString(), "InsertarDatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub InsertarDatosBusqueda(pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                cb.Receptor = pobjItem.IdItem
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
                Case "cmCupoReceptor"
                    If Not IsNothing(CupoReceptorSeleccionado) Then
                        If Not IsNothing(_CupoReceptorSeleccionado.IDSucursal) Or Not IsNothing(_CupoReceptorSeleccionado.IDMesa) Or Not String.IsNullOrEmpty(_CupoReceptorSeleccionado.IDReceptor) Then
                            Dim logValidar As Boolean = False

                            If Not IsNothing(_ListaCupoReceptorXTipoNegocio) Then
                                If _ListaCupoReceptorXTipoNegocio.Count = 0 Then
                                    logValidar = True
                                End If
                            Else
                                logValidar = True
                            End If

                            If logValidar Then
                                IsBusy = True

                                If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                                    objProxy.ValidacionEliminarRegistros.Clear()
                                End If
                                Dim ValorSucursal As String = "NULL"
                                Dim ValorMesa As String = "NULL"
                                Dim ValorReceptor As String = "NULL"

                                If Not IsNothing(_CupoReceptorSeleccionado.IDSucursal) And _CupoReceptorSeleccionado.IDSucursal <> 0 Then
                                    ValorSucursal = _CupoReceptorSeleccionado.IDSucursal.ToString
                                End If

                                If Not IsNothing(_CupoReceptorSeleccionado.IDMesa) And _CupoReceptorSeleccionado.IDMesa <> 0 Then
                                    ValorMesa = _CupoReceptorSeleccionado.IDMesa.ToString
                                End If

                                If Not String.IsNullOrEmpty(_CupoReceptorSeleccionado.IDReceptor) And _CupoReceptorSeleccionado.IDReceptor <> "TOD" Then
                                    ValorReceptor = _CupoReceptorSeleccionado.IDReceptor
                                End If

                                objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblCupoReceptorXTipoNegocio", "'intIDSucursal'|'intIDMesa'|'strIDReceptor'", String.Format("'{0}'|'{1}'|'{2}'", ValorSucursal, ValorMesa, ValorReceptor), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
                            Else
                                NuevoDetalle()
                            End If

                        Else
                            mostrarMensaje("Debe de seleccionar la sucursal, la mesa o el receptor para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        mostrarMensaje("Debe de seleccionar la sucursal, la mesa o el receptor para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
                Case "cmCupoReceptor"
                    If Not IsNothing(ListaCupoReceptorXTipoNegocio) Then
                        If Not IsNothing(_CupoReceptorXTipoNegocio) Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(CupoReceptorXTipoNegocio, ListaCupoReceptorXTipoNegocio.ToList)

                            If ListaCupoReceptorXTipoNegocio.Contains(_CupoReceptorXTipoNegocio) Then
                                ListaCupoReceptorXTipoNegocio.Remove(_CupoReceptorXTipoNegocio)
                            End If
                            If ListaCupoReceptorXTipoNegocio.Count > 0 Then
                                Program.PosicionarItemLista(CupoReceptorXTipoNegocio, ListaCupoReceptorXTipoNegocio.ToList, intRegistroPosicionar)
                            Else
                                CupoReceptorXTipoNegocio = Nothing
                            End If
                        End If
                        MyBase.CambioItem("CupoReceptorXTipoNegocio")
                        MyBase.CambioItem("ListaCupoReceptorXTipoNegocio")
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
            Dim newCupoReceptor As New CupoReceptorXTipoNegocio
            'TODO: Verificar cuales son los campos que deben inicializarse
            newCupoReceptor.ID = -New Random().Next()
            newCupoReceptor.IDSucursal = IIf(IsNothing(_CupoReceptorSeleccionado.IDSucursal), 0, _CupoReceptorSeleccionado.IDSucursal)
            newCupoReceptor.IDMesa = IIf(IsNothing(_CupoReceptorSeleccionado.IDMesa), 0, _CupoReceptorSeleccionado.IDMesa)
            newCupoReceptor.IDReceptor = IIf(String.IsNullOrEmpty(_CupoReceptorSeleccionado.IDReceptor), "TOD", _CupoReceptorSeleccionado.IDReceptor)
            newCupoReceptor.NombreIDReceptor = _CupoReceptorSeleccionado.NombreReceptor
            newCupoReceptor.TipoNegocio = "TOD"
            newCupoReceptor.PorcentajeCupo = 0
            newCupoReceptor.Usuario = Program.Usuario

            If IsNothing(ListaCupoReceptorXTipoNegocio) Then
                ListaCupoReceptorXTipoNegocio = dcProxy1.CupoReceptorXTipoNegocios
            End If

            ListaCupoReceptorXTipoNegocio.Add(newCupoReceptor)
            CupoReceptorXTipoNegocio = newCupoReceptor

            MyBase.CambioItem("ListaCupoReceptorXTipoNegocio")
            MyBase.CambioItem("CupoReceptorXTipoNegocio")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar el nuevo registro.", _
                                                         Me.ToString(), "NuevoDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    Private Sub _CupoReceptorSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CupoReceptorSeleccionado.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName.ToLower = "idsucursal" Or e.PropertyName.ToLower = "idmesa" Then
                    SucursalMesa = String.Format("{0}-{1}", _CupoReceptorSeleccionado.IDSucursal, _CupoReceptorSeleccionado.IDMesa)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_CupoReceptorSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _CupoReceptorSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CupoReceptorXTipoNegocio.PropertyChanged
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_CupoReceptorSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaCopoReceptor
    Implements INotifyPropertyChanged

    Private _Sucursal As Integer
    <Display(Name:="Sucursal", Description:="Sucursal")> _
    Public Property Sucursal() As Integer
        Get
            Return _Sucursal
        End Get
        Set(ByVal value As Integer)
            _Sucursal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Sucursal"))
        End Set
    End Property

    Private _Mesa As Integer
    <Display(Name:="Mesa", Description:="Mesa")> _
    Public Property Mesa() As Integer
        Get
            Return _Mesa
        End Get
        Set(ByVal value As Integer)
            _Mesa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Mesa"))
        End Set
    End Property

    Private _Receptor As String
    <Display(Name:="Receptor", Description:="Receptor")> _
    Public Property Receptor() As String
        Get
            Return _Receptor
        End Get
        Set(ByVal value As String)
            _Receptor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Receptor"))
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