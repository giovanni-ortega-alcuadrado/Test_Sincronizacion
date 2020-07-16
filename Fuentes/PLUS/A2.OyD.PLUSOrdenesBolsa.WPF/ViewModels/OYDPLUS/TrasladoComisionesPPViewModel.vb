Imports Telerik.Windows.Controls
'ViewModel para el registro de opraciones entre receptores (comisiones en dinero)
'Marzo 31 de 2014
'Santiago Alexander Vergara Orrego
'-------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports OpenRiaServices.DomainServices.Client


Public Class TrasladoComisionesPPViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private OperacionPorReceptorPorDefecto As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor
    Private OperacionPorReceptorAnterior As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor
    Private dcProxy As OYDPLUSOrdenesBolsaDomainContext
    Private dcProxy1 As OYDPLUSOrdenesBolsaDomainContext
    Private mdcProxyUtilidad03 As OyDPLUSutilidadesDomainContext


    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext()
                dcProxy1 = New OYDPLUSOrdenesBolsaDomainContext()
                mdcProxyUtilidad03 = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OYDPLUSOrdenesBolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad03 = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxy1.DomainClient, WebDomainClient(Of OYDPLUSOrdenesBolsaDomainContext.IOYDPLUSOrdenesBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad03.DomainClient, WebDomainClient(Of OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If Not Program.IsDesignMode() Then
                IsBusy = True
                ConsultarReceptoresUsuario("INICIO")
                dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorFiltrarQuery("", MLOG_TRASLADOENDINERO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "")
                dcProxy1.Load(dcProxy1.TraerRegistroOperacionesPorReceptorPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionPorReceptorPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "TrasladoComisionesPPViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Constantes"

    Private Const MLOG_TRASLADOENDINERO As Boolean = True

#End Region

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerOperacionPorReceptorPorDefecto_Completed(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor))
        If Not lo.HasError Then
            OperacionPorReceptorPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la operación por receptor por defecto", _
                                             Me.ToString(), "TerminoTraerOperacionPorReceptorPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' resultado de la consulta de re gistros de operaiciones
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara - Mayo 27/2014</remarks>
    Private Sub TerminoTraerOperacionesPorReceptor(ByVal lo As LoadOperation(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor))
        If Not lo.HasError Then
            ListaOperacionesPorReceptor = dcProxy.RegistroOperacionesPorReceptors
            If dcProxy.RegistroOperacionesPorReceptors.Count > 0 Then
                If lo.UserState = "insert" Then
                    _OperacionesPorReceptorSelected = ListaOperacionesPorReceptor.First
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Operaciones por receptor", _
                                             Me.ToString(), "TerminoTraerOperacionesPorReceptor", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Recibe el resultado de la consulta de receptores segun el usuario logueado en el sistema
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>Santiago Vergara -  Marzo 28/2014</remarks>
    Private Sub TerminoConsultarTodosReceptoresUsuario(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblReceptoresUsuario))
        Try
            IsBusy = False
            If lo.HasError = False Then
                ListaReceptoresUsuario = lo.Entities.ToList
                If lo.UserState = "EDITAR" Then
                    If IsNothing(ListaReceptoresUsuario) Then
                        mostrarMensaje("No se puede modificar porque no tiene permisos sobre uno de los receptores de la operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf _ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = _OperacionesPorReceptorSelected.ReceptorA).Count = 0 And
                        _ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = _OperacionesPorReceptorSelected.ReceptorB).Count = 0 Then
                        mostrarMensaje("No se puede modificar porque no tiene permisos sobre uno de los receptores de la operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        ObtenerRegistroAnterior()
                        Editando = True
                    End If
                ElseIf lo.UserState = "BORRAR" Then
                    If IsNothing(ListaReceptoresUsuario) Then
                        mostrarMensaje("No se puede inactivar porque no tiene permisos sobre uno de los receptores de la operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf _ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = _OperacionesPorReceptorSelected.ReceptorA).Count = 0 And
                        _ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = _OperacionesPorReceptorSelected.ReceptorB).Count = 0 Then
                        mostrarMensaje("No se puede inactivar porque no tiene permisos sobre uno de los receptores de la operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        If Not IsNothing(_OperacionesPorReceptorSelected) Then
                            A2Utilidades.Mensajes.mostrarMensajePregunta("Esta opción permite inactivar el registro seleccionado. ¿Confirma la inactivación de el registro?", Program.TituloSistema, "inactivar", AddressOf TerminoMensajePregunta)
                        End If
                    End If
                End If
            Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaOperacionesPorReceptor As EntitySet(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor)
    Public Property ListaOperacionesPorReceptor() As EntitySet(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor)
        Get
            Return _ListaOperacionesPorReceptor
        End Get
        Set(ByVal value As EntitySet(Of OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor))
            _ListaOperacionesPorReceptor = value
            MyBase.CambioItem("ListaOperacionesPorReceptor")
            MyBase.CambioItem("ListaOperacionesPorReceptorPaged")
            If Not IsNothing(_ListaOperacionesPorReceptor) Then
                If _ListaOperacionesPorReceptor.Count > 0 Then
                    OperacionesPorReceptorSelected = _ListaOperacionesPorReceptor.FirstOrDefault
                Else
                    _OperacionesPorReceptorSelected = Nothing
                End If
            Else
                _OperacionesPorReceptorSelected = Nothing
            End If

        End Set
    End Property

    Public ReadOnly Property ListaOperacionesPorReceptorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOperacionesPorReceptor) Then
                Dim view = New PagedCollectionView(_ListaOperacionesPorReceptor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _OperacionesPorReceptorSelected As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor
    Public Property OperacionesPorReceptorSelected() As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor
        Get
            Return _OperacionesPorReceptorSelected
        End Get
        Set(ByVal value As OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor)
            _OperacionesPorReceptorSelected = value
            MyBase.CambioItem("OperacionesPorReceptorSelected")
        End Set
    End Property

    Private _cb As CamposBusquedaOperacionesPorReceptor
    Public Property cb() As CamposBusquedaOperacionesPorReceptor
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaOperacionesPorReceptor)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _ListaReceptoresUsuario As List(Of OYDPLUSUtilidades.tblReceptoresUsuario)
    Public Property ListaReceptoresUsuario() As List(Of OYDPLUSUtilidades.tblReceptoresUsuario)
        Get
            Return _ListaReceptoresUsuario
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblReceptoresUsuario))
            _ListaReceptoresUsuario = value
            MyBase.CambioItem("ListaReceptoresUsuario")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try

            Dim NewOperacionesPorReceptor As New OyDPLUSOrdenesBolsa.RegistroOperacionesPorReceptor

            NewOperacionesPorReceptor.IdOperacion = OperacionPorReceptorPorDefecto.IdOperacion
            NewOperacionesPorReceptor.Activo = True
            NewOperacionesPorReceptor.TrasladoEnDinero = MLOG_TRASLADOENDINERO
            NewOperacionesPorReceptor.Usuario = Program.Usuario
            NewOperacionesPorReceptor.FechaLiquidacion = Date.Now
            NewOperacionesPorReceptor.FechaCumplimiento = Date.Now
            NewOperacionesPorReceptor.FechaIngreso = Date.Now

            ObtenerRegistroAnterior()
            Editando = True
            MyBase.CambioItem("Editando")
            OperacionesPorReceptorSelected = NewOperacionesPorReceptor
            MyBase.CambioItem("OperacionesPorReceptorSelected")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            If Not IsNothing(dcProxy.RegistroOperacionesPorReceptors) Then
                dcProxy.RegistroOperacionesPorReceptors.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorFiltrarQuery(TextoFiltroSeguro, MLOG_TRASLADOENDINERO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorFiltrarQuery("", MLOG_TRASLADOENDINERO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se hacen validaciones basicas del registro ingresado
    ''' </summary>
    ''' <returns>true si no hay validaciones que detengan el proceso</returns>
    ''' <remarks>Santiago Vergara - Junio 06/2014</remarks>
    Private Function RegistroValido() As Boolean

        If String.IsNullOrEmpty(_OperacionesPorReceptorSelected.ReceptorA) Then
            mostrarMensaje("El receptor A es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If String.IsNullOrEmpty(_OperacionesPorReceptorSelected.ReceptorB) Then
            mostrarMensaje("El receptor B es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If IsNothing(_OperacionesPorReceptorSelected.ValorGiro) Or _OperacionesPorReceptorSelected.ValorGiro = 0 Then
            mostrarMensaje("El valor giro es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If _OperacionesPorReceptorSelected.ReceptorA = _OperacionesPorReceptorSelected.ReceptorB Then
            mostrarMensaje("El receptor B debe ser didferente al receptor A", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        If _OperacionesPorReceptorSelected.FechaCumplimiento.Value.Date < _OperacionesPorReceptorSelected.FechaLiquidacion.Value.Date Then
            mostrarMensaje("La fecha de cumplimiento no puede ser menor a la fecha de liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        Return True
    End Function

    Public Overrides Sub ActualizarRegistro()
        Try

            If RegistroValido() Then

                Dim origen = "update"
                ErrorForma = ""
                _OperacionesPorReceptorSelected.Usuario = Program.Usuario
                _OperacionesPorReceptorSelected.Activo = True
                _OperacionesPorReceptorSelected.TrasladoEnDinero = MLOG_TRASLADOENDINERO


                If (From li In ListaOperacionesPorReceptor Where li.IdOperacion = _OperacionesPorReceptorSelected.IdOperacion Select li).Count = 0 Then
                    origen = "insert"
                    ListaOperacionesPorReceptor.Add(_OperacionesPorReceptorSelected)
                End If

                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)

                ObtenerRegistroAnterior()
            End If

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
                Dim strMsg As String = String.Empty

                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState = "insert" Or So.UserState = "update" Then
                    mostrarMensaje("El registro se actualizó correctamente ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    If Not IsNothing(dcProxy.RegistroOperacionesPorReceptors) Then
                        dcProxy.RegistroOperacionesPorReceptors.Clear()
                    End If
                    IsBusy = True
                    dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorFiltrarQuery("", MLOG_TRASLADOENDINERO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "FiltroGuardar")
                ElseIf So.UserState = "BorrarRegistro" Then
                    mostrarMensaje("El registro se inactivó correctamente ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                End If
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_OperacionesPorReceptorSelected) Then
                If _OperacionesPorReceptorSelected.FechaCumplimiento.Value.Date < Date.Today Then
                    MyBase.RetornarValorEdicionNavegacion()
                    mostrarMensaje("No se puede modificar un traslado cumplido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    ObtenerRegistroAnterior()
                    Editando = True
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición de un registro", _
                                                         Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_OperacionesPorReceptorSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                OperacionesPorReceptorSelected = OperacionPorReceptorAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
             If Not IsNothing(_OperacionesPorReceptorSelected) Then
                A2Utilidades.Mensajes.mostrarMensajePregunta("Esta opción permite inactivar el registro seleccionado. ¿Confirma la inactivación de el registro?", Program.TituloSistema, "inactivar", AddressOf TerminoMensajePregunta)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Recibe la respuesta de una confirmación del usuario en la pantalla
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Santiago Vergara - Mayo 27/2014</remarks>
    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Select Case CType(sender, A2Utilidades.wppMensajePregunta).CodigoLlamado.ToLower
                    Case "inactivar"
                        dcProxy.RegistroOperacionesPorReceptors.Remove(_OperacionesPorReceptorSelected)

                        OperacionesPorReceptorSelected = _ListaOperacionesPorReceptor.LastOrDefault
                        IsBusy = True
                        Program.VerificarCambiosProxyServidor(dcProxy)
                        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                End Select
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inactivar un registro", Me.ToString(), "TerminoMensajePregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IdOperacion <> 0 Or Not String.IsNullOrEmpty(cb.ReceptorA) Or Not String.IsNullOrEmpty(cb.ReceptorB) Or Not IsNothing(cb.FechaLiquidacion) Or Not IsNothing(cb.FechaCumplimiento) Then
                ErrorForma = ""
                If Not IsNothing(dcProxy.RegistroOperacionesPorReceptors) Then
                    dcProxy.RegistroOperacionesPorReceptors.Clear()
                End If
                OperacionPorReceptorAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.RegistroOperacionesPorReceptorConsultarQuery(cb.IdOperacion, Nothing, Nothing, cb.ReceptorA, cb.ReceptorB, Nothing, MLOG_TRASLADOENDINERO, Program.Usuario, cb.FechaLiquidacion, cb.FechaCumplimiento, Program.HashConexion), AddressOf TerminoTraerOperacionesPorReceptor, "Busqueda")
                MyBase.ConfirmarBuscar()
                PrepararNuevaBusqueda()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' inicializa los campos de la pantalla de busqueda
    ''' </summary>
    ''' <remarks>Santiago Vergara - Mayo 27/2014</remarks>
    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaOperacionesPorReceptor
            objCB.IdOperacion = Nothing
            objCB.ReceptorA = String.Empty
            objCB.ReceptorB = String.Empty
            objCB.FechaLiquidacion = Nothing
            objCB.FechaCumplimiento = Nothing

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la nueva busqueda", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se almacena el registro anterior por transacciones de la pantalla
    ''' </summary>
    ''' <remarks>Santiago Vergara - Mayo 27/2014</remarks>
    Public Sub ObtenerRegistroAnterior()
        Try
            If Not IsNothing(_OperacionesPorReceptorSelected) Then
                OperacionPorReceptorAnterior = _OperacionesPorReceptorSelected
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos del registro anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarReceptoresUsuario(ByVal pstrOpcion As String)
        Try
            If Not IsNothing(mdcProxyUtilidad03.tblReceptoresUsuarios) Then
                mdcProxyUtilidad03.tblReceptoresUsuarios.Clear()
            End If

            mdcProxyUtilidad03.Load(mdcProxyUtilidad03.OYDPLUS_ConsultarReceptoresUsuarioQuery(True, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarTodosReceptoresUsuario, pstrOpcion)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los receptores del usuario.", _
             Me.ToString(), "ConsultarReceptoresUsuario", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaOperacionesPorReceptor
    Implements INotifyPropertyChanged

    Private _IdOperacion As Integer
    <Display(Name:="Id operación")> _
    Public Property IdOperacion() As Integer
        Get
            Return _IdOperacion
        End Get
        Set(ByVal value As Integer)
            _IdOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdOperacion"))
        End Set
    End Property

    Private _ReceptorA As String
    <Display(Name:="Receptor A")> _
    Public Property ReceptorA() As String
        Get
            Return _ReceptorA
        End Get
        Set(ByVal value As String)
            _ReceptorA = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ReceptorA"))
        End Set
    End Property

    Private _NombreReceptorA As String
    <Display(Name:="Receptor A")> _
    Public Property NombreReceptorA() As String
        Get
            Return _NombreReceptorA
        End Get
        Set(ByVal value As String)
            _NombreReceptorA = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreReceptorA"))
        End Set
    End Property

    Private _ReceptorB As String
    <Display(Name:="Receptor B")> _
    Public Property ReceptorB() As String
        Get
            Return _ReceptorB
        End Get
        Set(ByVal value As String)
            _ReceptorB = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ReceptorB"))
        End Set
    End Property

    Private _NombreReceptorB As String
    <Display(Name:="Receptor B")> _
    Public Property NombreReceptorB() As String
        Get
            Return _NombreReceptorB
        End Get
        Set(ByVal value As String)
            _NombreReceptorB = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreReceptorB"))
        End Set
    End Property

    Private _FechaLiquidacion As Nullable(Of DateTime)
    <Display(Name:="Fecha liquidación")> _
    Public Property FechaLiquidacion() As Nullable(Of DateTime)
        Get
            Return _FechaLiquidacion
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaLiquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaLiquidacion"))
        End Set
    End Property

    Private _FechaCumplimiento As Nullable(Of DateTime)
    <Display(Name:="Fecha cumplimiento")> _
    Public Property FechaCumplimiento() As Nullable(Of DateTime)
        Get
            Return _FechaCumplimiento
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaCumplimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaCumplimiento"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class