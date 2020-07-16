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

Public Class ClientesRelacionadosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ClienteRelacionadoPorDefecto As ClientesRelacionadosEncabezado
    Private ClienteRelacionadoAnterior As ClientesRelacionadosEncabezado
    Private ListaClienteRelaciondoAnterior As New List(Of String)
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim dcProxy2 As OyDPLUSMaestrosDomainContext
    Dim IdItemActualizar As Long = 0
    Dim logRefrescarDetalle As Boolean = False
    Dim logEditarRegistro As Boolean = False

    Public Sub New()
        Try

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
                dcProxy2 = New OyDPLUSMaestrosDomainContext()
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy2 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ClientesRelacionadosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesRelacionados, "")
                dcProxy2.Load(dcProxy2.TraerClienteRelacionadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesRelacionadosPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClientesRelacionadosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClientesRelacionadosPorDefecto_Completed(ByVal lo As LoadOperation(Of ClientesRelacionadosEncabezado))
        If Not lo.HasError Then
            ClienteRelacionadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClientesRelacionados por defecto", _
                                             Me.ToString(), "TerminoTraerClientesRelacionadosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesRelacionados(ByVal lo As LoadOperation(Of ClientesRelacionadosEncabezado))
        If Not lo.HasError Then
            If lo.UserState = "actualizar" Then
                logRefrescarDetalle = False
            Else
                logRefrescarDetalle = True
            End If

            ListaClientes = dcProxy.ClientesRelacionadosEncabezados

            If _ListaClientes.Count > 0 Then
                If lo.UserState = "actualizar" Then
                    If IdItemActualizar <> 0 Then
                        logRefrescarDetalle = True
                        If _ListaClientes.Where(Function(i) i.NroDocumentoCliente = IdItemActualizar).Count > 0 Then
                            ClienteSeleccionado = _ListaClientes.Where(Function(i) i.NroDocumentoCliente = IdItemActualizar).FirstOrDefault
                        Else
                            ClienteSeleccionado = _ListaClientes.First
                        End If
                    Else
                        logRefrescarDetalle = True
                        ClienteSeleccionado = _ListaClientes.Last
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

    Private Sub TerminoTraerRelacionesCliente(ByVal lo As LoadOperation(Of ClientesRelacionados))
        Try
            If Not lo.HasError Then
                ListaClientesRelacionados = dcProxy1.ClientesRelacionados
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesRelacionados", _
                                                 Me.ToString(), "TerminoTraerRelacionesCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesRelacionados", _
                                                Me.ToString(), "TerminoTraerRelacionesCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try
    End Sub

    Private Sub TerminoValidarRelaciones(ByVal lo As LoadOperation(Of ValidacionClientesRelacionados))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim objListaRelaciones As String = String.Empty

                    For Each li In lo.Entities.ToList
                        objListaRelaciones = IIf(String.IsNullOrEmpty(objListaRelaciones), String.Format("Cliente {0} - Relacionado con {1}", _ClienteSeleccionado.NroDocumentoCliente, li.NroDocumento), _
                                                                      String.Format("{0}{1}Cliente {2} - Relacionado con {3}", objListaRelaciones, vbCrLf, _ClienteSeleccionado.NroDocumentoCliente, li.NroDocumento))
                    Next

                    mostrarMensaje(String.Format("No se puede guardar la información porque ya existen relaciones entre el cliente y algunos clientes de la relación {0}{1}", vbCrLf, objListaRelaciones), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                Else
                    Program.VerificarCambiosProxyServidor(dcProxy1)
                    dcProxy1.SubmitChanges(AddressOf TerminoSubmitChanges, "actualizar")
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las validaciones de los relacionados", _
                                                 Me.ToString(), "TerminoValidarRelaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las validaciones de los relacionados", _
                                                Me.ToString(), "TerminoValidarRelaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaClientes As EntitySet(Of ClientesRelacionadosEncabezado)
    Public Property ListaClientes() As EntitySet(Of ClientesRelacionadosEncabezado)
        Get
            Return _ListaClientes
        End Get
        Set(ByVal value As EntitySet(Of ClientesRelacionadosEncabezado))
            _ListaClientes = value
            MyBase.CambioItem("ListaClientes")
            MyBase.CambioItem("ListaClientesPaged")

            If Not IsNothing(_ListaClientes) Then
                If _ListaClientes.Count > 0 Then
                    ClienteSeleccionado = _ListaClientes.First
                Else
                    ClienteSeleccionado = Nothing
                End If
            Else
                ClienteSeleccionado = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClientesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClientes) Then
                Dim view = New PagedCollectionView(_ListaClientes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ClienteSeleccionado As ClientesRelacionadosEncabezado
    Public Property ClienteSeleccionado() As ClientesRelacionadosEncabezado
        Get
            Return _ClienteSeleccionado
        End Get
        Set(ByVal value As ClientesRelacionadosEncabezado)
            _ClienteSeleccionado = value
            If Not IsNothing(_ClienteSeleccionado) And logRefrescarDetalle Then
                ConsultarRelacionesCliente(_ClienteSeleccionado)
            End If
            MyBase.CambioItem("ClienteSeleccionado")
        End Set
    End Property

    Private _ListaClientesRelacionados As EntitySet(Of ClientesRelacionados)
    Public Property ListaClientesRelacionados() As EntitySet(Of ClientesRelacionados)
        Get
            Return _ListaClientesRelacionados
        End Get
        Set(ByVal value As EntitySet(Of ClientesRelacionados))
            _ListaClientesRelacionados = value
            MyBase.CambioItem("ListaClientesRelacionados")
            If Not IsNothing(_ListaClientesRelacionados) Then
                If _ListaClientesRelacionados.Count > 0 Then
                    ClienteRelacionadoSelected = _ListaClientesRelacionados.First
                End If
            End If
        End Set
    End Property

    Private _ClienteRelacionadoSelected As ClientesRelacionados
    Public Property ClienteRelacionadoSelected() As ClientesRelacionados
        Get
            Return _ClienteRelacionadoSelected
        End Get
        Set(ByVal value As ClientesRelacionados)
            _ClienteRelacionadoSelected = value
            MyBase.CambioItem("ClienteRelacionadoSelected")
        End Set
    End Property

    Private WithEvents _cb As CamposBusquedaClientesRelacionados = New CamposBusquedaClientesRelacionados
    Public Property cb() As CamposBusquedaClientesRelacionados
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaClientesRelacionados)
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



#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewClientesRelacionados As New ClientesRelacionadosEncabezado
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewClientesRelacionados.NombreCliente = ClienteRelacionadoPorDefecto.NombreCliente
            NewClientesRelacionados.NombreTipoIdCliente = ClienteRelacionadoPorDefecto.NombreTipoIdCliente
            NewClientesRelacionados.NroDocumentoCliente = ClienteRelacionadoPorDefecto.NroDocumentoCliente
            NewClientesRelacionados.TipoIdCliente = ClienteRelacionadoPorDefecto.TipoIdCliente

            ObtenerRegistroAnterior()
            ClienteSeleccionado = NewClientesRelacionados
            Editando = True
            MyBase.CambioItem("Editando")
            LimpiarItem = True

            If Not IsNothing(dcProxy1.ClientesRelacionados) Then
                dcProxy1.ClientesRelacionados.Clear()
            End If

            If Not IsNothing(ListaClientesRelacionados) Then
                ListaClientesRelacionados.Clear()
            End If

            logEditarRegistro = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ClientesRelacionadosEncabezados.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ClientesRelacionadosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesRelacionados, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ClientesRelacionadosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesRelacionados, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            cb = New CamposBusquedaClientesRelacionados()
            MyBase.CambioItem("cb")
            MyBase.Buscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación la busqueda", _
                                             Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not IsNothing(_cb.Nombre) Or Not String.IsNullOrEmpty(_cb.NroDocumento) Or Not IsNothing(_cb.strTipoRelacion) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""

                ClienteRelacionadoAnterior = Nothing
                IsBusy = True

                If _cb.NroDocumento = "0" Then
                    _cb.NroDocumento = String.Empty
                End If

                If Not IsNothing(dcProxy.ClientesRelacionadosEncabezados) Then
                    dcProxy.ClientesRelacionadosEncabezados.Clear()
                End If

                dcProxy.Load(dcProxy.ClientesRelacionadosConsultarQuery(_cb.TipoDocumento, _cb.NroDocumento, _cb.Nombre, Program.Usuario, cb.strTipoRelacion, Program.HashConexion), AddressOf TerminoTraerClientesRelacionados, "Busqueda")

                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaClientesRelacionados
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
            If Not IsNothing(_ClienteSeleccionado) Then
                If String.IsNullOrEmpty(_ClienteSeleccionado.NroDocumentoCliente) Then
                    mostrarMensaje("Debe seleccionar una entidad en el encabezado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If IsNothing(ListaClientesRelacionados) Then
                mostrarMensaje("Debe ingresar al menos un detalle para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            Else
                If ListaClientesRelacionados.Count = 0 Then
                    mostrarMensaje("Debe ingresar al menos un detalle para realizar la relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    For Each li In _ListaClientesRelacionados
                        If String.IsNullOrEmpty(li.NroDocumentoClienteRelacionado) Then
                            mostrarMensaje("Debe ingresar toda la información de relación en el detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                        If String.IsNullOrEmpty(li.TipoRelacion) Or li.TipoRelacion = "NING" Then
                            mostrarMensaje("Debe ingresar toda la información de relación en el detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    Next
                End If
            End If

            IsBusy = True

            Dim objListaClientesRelacionados As New List(Of ClientesRelacionados)

            If logEditarRegistro Then
                For Each li In _ListaClientesRelacionados
                    If Not ListaClienteRelaciondoAnterior.Contains(li.NroDocumentoClienteRelacionado) Then
                        objListaClientesRelacionados.Add(li)
                    End If
                Next
            Else
                objListaClientesRelacionados = _ListaClientesRelacionados.ToList
            End If

            If logEditarRegistro Then
                If objListaClientesRelacionados.Count > 0 Then
                    ValidarRelacionesCliente(_ClienteSeleccionado.TipoIdCliente, _ClienteSeleccionado.NroDocumentoCliente, objListaClientesRelacionados)
                Else
                    Program.VerificarCambiosProxyServidor(dcProxy1)
                    dcProxy1.SubmitChanges(AddressOf TerminoSubmitChanges, "actualizar")
                End If
            Else
                ValidarRelacionesCliente(_ClienteSeleccionado.TipoIdCliente, _ClienteSeleccionado.NroDocumentoCliente, objListaClientesRelacionados)
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

            IdItemActualizar = _ClienteSeleccionado.NroDocumentoCliente

            If Not IsNothing(dcProxy.ClientesRelacionadosEncabezados) Then
                dcProxy.ClientesRelacionadosEncabezados.Clear()
            End If

            MyBase.QuitarFiltroDespuesGuardar()
            dcProxy.Load(dcProxy.ClientesRelacionadosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesRelacionados, So.UserState)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_ClienteSeleccionado) Then
                Editando = True
                ObtenerRegistroAnterior()
                LimpiarItem = True
                logEditarRegistro = True

                If Not IsNothing(_ListaClientesRelacionados) Then
                    If Not IsNothing(ListaClienteRelaciondoAnterior) Then
                        ListaClienteRelaciondoAnterior.Clear()
                    Else
                        ListaClienteRelaciondoAnterior = New List(Of String)
                    End If

                    For Each li In _ListaClientesRelacionados
                        ListaClienteRelaciondoAnterior.Add(li.NroDocumentoClienteRelacionado)
                    Next
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro.", _
                                             Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ClienteSeleccionado) Then
                dcProxy1.RejectChanges()
                ClienteSeleccionado = ClienteRelacionadoAnterior
            End If
            Editando = False
            LimpiarItem = False
            logEditarRegistro = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ClienteSeleccionado) Then
                IsBusy = True
                dcProxy.ClientesRelacionadosEliminar(_ClienteSeleccionado.NroDocumentoCliente, Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarClientes, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoEliminarClientes(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If lo.HasError = False Then
                Filtrar()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "TerminoEliminarClientes", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "TerminoEliminarClientes", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objClientesRelacionados As New ClientesRelacionadosEncabezado
            If Not IsNothing(_ClienteSeleccionado) Then
                objClientesRelacionados.ID = _ClienteSeleccionado.ID
                objClientesRelacionados.NombreCliente = _ClienteSeleccionado.NombreCliente
                objClientesRelacionados.NombreTipoIdCliente = _ClienteSeleccionado.NombreTipoIdCliente
                objClientesRelacionados.NroDocumentoCliente = _ClienteSeleccionado.NroDocumentoCliente
                objClientesRelacionados.TipoIdCliente = _ClienteSeleccionado.TipoIdCliente
            End If

            ClienteRelacionadoAnterior = Nothing
            ClienteRelacionadoAnterior = objClientesRelacionados
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarRelacionesCliente(ByVal pobjClienteSeleccionado As ClientesRelacionadosEncabezado, Optional ByVal pstrUserstate As String = "")
        Try
            If Not IsNothing(pobjClienteSeleccionado) Then
                If Not IsNothing(dcProxy1.ClientesRelacionados) Then
                    dcProxy1.ClientesRelacionados.Clear()
                End If

                dcProxy1.Load(dcProxy1.ClientesRelacionadosConsultar_RelacionesQuery(pobjClienteSeleccionado.TipoIdCliente, pobjClienteSeleccionado.NroDocumentoCliente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRelacionesCliente, pstrUserstate)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la información de los clientes relacionados.", _
             Me.ToString(), "ConsultarRelacionesCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub InsertarDatos(pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(_ListaClientesRelacionados) Then
                    If _ListaClientesRelacionados.Where(Function(i) i.NroDocumentoClienteRelacionado = pobjItem.IdItem).Count = 0 Then
                        _ClienteSeleccionado.NroDocumentoCliente = pobjItem.IdItem
                        _ClienteSeleccionado.TipoIdCliente = pobjItem.CodigoAuxiliar
                        _ClienteSeleccionado.NombreTipoIdCliente = pobjItem.InfoAdicional01
                        _ClienteSeleccionado.NombreCliente = pobjItem.Nombre

                        For Each li In _ListaClientesRelacionados
                            li.NombreCliente = _ClienteSeleccionado.NombreCliente
                            li.NombreTipoIdCliente = _ClienteSeleccionado.NombreTipoIdCliente
                            li.NroDocumentoCliente = _ClienteSeleccionado.NroDocumentoCliente
                            li.TipoIdCliente = _ClienteSeleccionado.TipoIdCliente
                        Next
                    Else
                        mostrarMensaje("El nro de documento ya existe en el detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    _ClienteSeleccionado.NroDocumentoCliente = pobjItem.IdItem
                    _ClienteSeleccionado.TipoIdCliente = pobjItem.CodigoAuxiliar
                    _ClienteSeleccionado.NombreTipoIdCliente = pobjItem.InfoAdicional01
                    _ClienteSeleccionado.NombreCliente = pobjItem.Nombre
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar los datos del buscador de clientes.", _
             Me.ToString(), "InsertarDatosNuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub InsertarDatosNuevoRegistro(pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If _ListaClientesRelacionados.Where(Function(i) i.NroDocumentoClienteRelacionado = pobjItem.IdItem).Count = 0 Then
                    ClienteRelacionadoSelected.NroDocumentoClienteRelacionado = pobjItem.IdItem
                    ClienteRelacionadoSelected.TipoIdClienteRelacionado = pobjItem.CodigoAuxiliar
                    ClienteRelacionadoSelected.NombreTipoIdClienteRelacionado = pobjItem.InfoAdicional01
                    ClienteRelacionadoSelected.NombreClienteRelacionado = pobjItem.Nombre
                Else
                    mostrarMensaje("El nro de documento ya existe en la tabla.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar los datos del buscador de clientes.", _
             Me.ToString(), "InsertarDatosNuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ValidarRelacionesCliente(ByVal pstrTipoIdentificacion As String, ByVal pstrNroDocumento As String, ByVal pobjListaRelaciones As List(Of ClientesRelacionados))
        Try
            IsBusy = True

            Dim objRelaciones As String = String.Empty

            If Not IsNothing(pobjListaRelaciones) Then
                For Each li In pobjListaRelaciones
                    objRelaciones = IIf(String.IsNullOrEmpty(objRelaciones), String.Format("'{0}'**'{1}'**'{2}'", li.TipoIdClienteRelacionado, li.NroDocumentoClienteRelacionado, li.TipoRelacion), _
                                                                             String.Format("{0}|'{1}'**'{2}'**'{3}'", objRelaciones, li.TipoIdClienteRelacionado, li.NroDocumentoClienteRelacionado, li.TipoRelacion))
                Next
            End If

            If Not String.IsNullOrEmpty(objRelaciones) Then
                If Not IsNothing(dcProxy1.ValidacionClientesRelacionados) Then
                    dcProxy2.ValidacionClientesRelacionados.Clear()
                End If

                dcProxy2.Load(dcProxy2.ClientesRelacionadosValidar_RelacionesQuery(pstrTipoIdentificacion, pstrNroDocumento, objRelaciones, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRelaciones, "VALIDACION")
            Else
                mostrarMensaje("No se ha ingresado ninguna relación, por favor ingrese al menos una relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la información de los clientes relacionados.", _
             Me.ToString(), "ConsultarRelacionesCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Detalles Maestros del Tipo negocio"

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmClientesRelacionados"
                    If Not IsNothing(_ClienteSeleccionado) Then
                        If Not String.IsNullOrEmpty(_ClienteSeleccionado.NombreTipoIdCliente) And Not String.IsNullOrEmpty(_ClienteSeleccionado.NombreCliente) And Not String.IsNullOrEmpty(_ClienteSeleccionado.NroDocumentoCliente) Then
                            Dim NewClientesRelacionados As New ClientesRelacionados
                            'TODO: Verificar cuales son los campos que deben inicializarse
                            NewClientesRelacionados.ID = -New Random().Next()
                            NewClientesRelacionados.NombreCliente = _ClienteSeleccionado.NombreCliente
                            NewClientesRelacionados.NombreClienteRelacionado = String.Empty
                            NewClientesRelacionados.NombreTipoIdCliente = _ClienteSeleccionado.NombreTipoIdCliente
                            NewClientesRelacionados.NombreTipoIdClienteRelacionado = String.Empty
                            NewClientesRelacionados.NroDocumentoCliente = _ClienteSeleccionado.NroDocumentoCliente
                            NewClientesRelacionados.NroDocumentoClienteRelacionado = String.Empty
                            NewClientesRelacionados.TipoIdCliente = ClienteSeleccionado.TipoIdCliente
                            NewClientesRelacionados.TipoIdClienteRelacionado = String.Empty
                            NewClientesRelacionados.TipoRelacion = "NING"
                            NewClientesRelacionados.Usuario = Program.Usuario

                            ListaClientesRelacionados.Add(NewClientesRelacionados)
                            ClienteRelacionadoSelected = NewClientesRelacionados

                            MyBase.CambioItem("ListaClientesRelacionados")
                            MyBase.CambioItem("ClienteRelacionadoSelected")
                        Else
                            mostrarMensaje("Primero debe seleccionar una entidad en el encabezado, para poder crear detalles.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        mostrarMensaje("Primero debe seleccionar una entidad en el encabezado, para poder crear detalles.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
                Case "cmClientesRelacionados"
                    If Not IsNothing(ListaClientesRelacionados) Then
                        If Not IsNothing(_ClienteRelacionadoSelected) Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClienteRelacionadoSelected, ListaClientesRelacionados.ToList)

                            If ListaClientesRelacionados.Contains(ClienteRelacionadoSelected) Then
                                ListaClientesRelacionados.Remove(ClienteRelacionadoSelected)
                            End If
                            If ListaClientesRelacionados.Count > 0 Then
                                Program.PosicionarItemLista(ClienteRelacionadoSelected, ListaClientesRelacionados.ToList, intRegistroPosicionar)
                            Else
                                ClienteRelacionadoSelected = Nothing
                            End If
                        End If
                        MyBase.CambioItem("ClienteRelacionadoSelected")
                        MyBase.CambioItem("ListaClientesRelacionados")
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle.", _
                                                         Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    'Private Sub _ClienteRelacionadoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClienteRelacionadoSelected.PropertyChanged
    '    Try

    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
    '         Me.ToString(), "_ClienteRelacionadoSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaClientesRelacionados
    Implements INotifyPropertyChanged

    Private _TipoDocumento As String
    <Display(Name:="Tipo identificación", Description:="Tipo identificación")> _
    Public Property TipoDocumento() As String
        Get
            Return _TipoDocumento
        End Get
        Set(ByVal value As String)
            _TipoDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoDocumento"))
        End Set
    End Property

    Private _NroDocumento As String
    <Display(Name:="Nro documento", Description:="Nro documento")> _
    Public Property NroDocumento() As String
        Get
            Return _NroDocumento
        End Get
        Set(ByVal value As String)
            _NroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroDocumento"))
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

    Private _strTipoRelacion As String
    <Display(Name:="Tipo relación", Description:="Tipo relación")> _
    Public Property strTipoRelacion() As String
        Get
            Return _strTipoRelacion
        End Get
        Set(ByVal value As String)
            _strTipoRelacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tipo relación"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


