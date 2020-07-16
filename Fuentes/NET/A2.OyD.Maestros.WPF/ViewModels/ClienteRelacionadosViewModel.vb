Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClienteAgrupadorViewModel.vb
'Generado el : 03/06/2012 17:14:59
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
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Text.RegularExpressions
Imports A2Utilidades.Mensajes

Public Class ClienteRelacionadosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaClienteRelacionado
    Private ClienteRelacionadoPorDefecto As Clientes_Relacionados
    Private ClienteRelacionadoAnterior As Clientes_Relacionados
    Private EncabezadoRelacionadoPorDefecto As EncabezadoRelacionado
    Private EncabezadoRelacionadoAnterior As EncabezadoRelacionado
    Private ListaClienteRelacionadoAnterior As New List(Of Clientes_Relacionados)
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim dcProxy2 As MaestrosDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Dim ListaDisparosAsync As New Dictionary(Of Integer, String)
    Dim PORCENTAJE_CERCANIA_SEGUNDO_MENSAJE As Integer = 0
    Dim objProxy2 As MaestrosDomainContext
    Dim objProxy3 As MaestrosDomainContext
    Dim objProxy4 As MaestrosDomainContext
    Dim TerminoConsultainhabil
    Dim YaValido As Boolean = False
    Dim logvalidacion As Boolean
    Dim ExisteCliente As Boolean = False
    Dim IdRelacionadoEliminar As String
    Dim strAccion As String
    Dim IdRelacionado As String
    Dim expresionemail As String = Program.ExpresionRegularEmail
    Dim strcodingreso As String
    'Dim strXMLDetalleEliminar As String
    'Dim strXMLDetalleE As String
    'Dim strXMLDetalleEliminarencabezado As String
    'Dim strXMLDetalleEliminarPie As String
    Dim logRecargarDetalle As Boolean = True 'DEMC20190523
    Dim intIDRegistroGuardado As String = String.Empty


    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New MaestrosDomainContext()
                dcProxy1 = New MaestrosDomainContext()
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                mdcProxyUtilidad02 = New UtilidadesDomainContext()
                objProxy2 = New MaestrosDomainContext()
                objProxy3 = New MaestrosDomainContext()
                objProxy4 = New MaestrosDomainContext()
            Else
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                dcProxy2 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                objProxy3 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                objProxy4 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))

            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy2.Load(dcProxy2.EncabezadoCRelacionadoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, "")
                'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerClientesRelacionadosPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteRelacionadoPorDefecto_Completed, "Default")
                objProxy3.Load(objProxy3.TraerEncabezadoRelacionadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionadoPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ClienteAgrupadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClienteRelacionadoPorDefecto_Completed(ByVal lo As LoadOperation(Of Clientes_Relacionados))
        If Not lo.HasError Then
            ClienteRelacionadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TerminoTraerClienteRelacionado Por Defecto",
                                             Me.ToString(), "TerminoTraerClienteRelacionadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Public Sub ConsultarComitente(comitente As String)

        Try
            objProxy4.ConsultaClientesRelacionados(comitente, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarComitente, "consulta")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos",
                                         Me.ToString(), "ConsultarComitente", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            '????
        End Try

    End Sub

    Private Sub TerminoTraerClienteRelacionado(ByVal lo As LoadOperation(Of Clientes_Relacionados))
        If Not lo.HasError Then
            ListaClienteRelacionados = dcProxy.Clientes_Relacionados
            'If dcProxy.Clientes_Responsables.Count > 0 Then
            '    If lo.UserState = "Ingresar" Then
            '        ClienteResponsableSelected = ListaClienteResponsable.Last
            '    End If
            'Else
            '    If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
            '        'MessageBox.Show("No se encontró ningún registro") Se comenta esta linea para que el mensaje sea mostrado en un control diferente a el messagebox
            '        'A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        'MyBase.Buscar()
            '        'MyBase.CancelarBuscar()
            '    End If
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClienteAgrupador",
                                             Me.ToString(), "TerminoTraerClienteAgrupado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        'IsBusy = False 'DEMC20190318
    End Sub

#Region "Resultados Asincrónicos Tabla Hija"

    Private Sub TerminoTraerEncabezadoRelacionadoPorDefecto_Completed(ByVal lo As LoadOperation(Of EncabezadoRelacionado))
        If Not lo.HasError Then
            EncabezadoRelacionadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la DetalleClienteAgrupado por defecto",
                                             Me.ToString(), "TerminoTraerDetalleClienteAgrupadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEncabezadoRelacionados(ByVal lo As LoadOperation(Of EncabezadoRelacionado))
        If Not lo.HasError Then 'DEMC20190523
            If lo.UserState = "Ingresar" Then
                logRecargarDetalle = False
            End If

            ListaEncabezadoRelacionado = dcProxy2.EncabezadoRelacionados.ToList
            If dcProxy2.EncabezadoRelacionados.Count > 0 Then
                If lo.UserState = "Ingresar" Then
                    If ListaEncabezadoRelacionado.Where(Function(i) i.IdEncabezado = intIDRegistroGuardado).Count > 0 Then
                        logRecargarDetalle = True
                        ClienteEncabezadoSelected = ListaEncabezadoRelacionado.Where(Function(i) i.IdEncabezado = intIDRegistroGuardado).First
                    Else
                        logRecargarDetalle = True
                        ClienteEncabezadoSelected = ListaEncabezadoRelacionado.FirstOrDefault
                    End If 'DEMC20190523
                End If
            Else
                ClienteEncabezadoSelected = Nothing
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    'MessageBox.Show("No se encontró ningún registro") Se comenta esta linea para que el mensaje sea mostrado en un control diferente a el messagebox
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EncabezadoRelacionado",
                                             Me.ToString(), "TerminoTraerEncabezadoRelacionados", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres

#End Region
    'Tablas padres

#End Region

#Region "Propiedades"



    Private _ListaClienteRelacionados As EntitySet(Of Clientes_Relacionados)
    Public Property ListaClienteRelacionados() As EntitySet(Of Clientes_Relacionados)
        Get
            Return _ListaClienteRelacionados
        End Get
        Set(ByVal value As EntitySet(Of Clientes_Relacionados))
            _ListaClienteRelacionados = value

            MyBase.CambioItem("ListaClienteRelacionados")
            MyBase.CambioItem("ListaClienteRelacionadosPaged")
            If Not IsNothing(value) Then
                If IsNothing(ClienteRelacionadoAnterior) Then
                    If Not IsNothing(_ClienteEncabezadoSelected) Then
                        ClienteRelacionadoSelected = _ListaClienteRelacionados.Where(Function(i) i.IDComitente = _ClienteEncabezadoSelected.IdEncabezado).LastOrDefault 'DEMC20190523
                    End If
                Else
                    ClienteRelacionadoSelected = ClienteRelacionadoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClienteRelacionadosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClienteRelacionados) Then
                Dim view = New PagedCollectionView(_ListaClienteRelacionados)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ClienteRelacionadoSelected As Clientes_Relacionados
    Public Property ClienteRelacionadoSelected() As Clientes_Relacionados
        Get
            Return _ClienteRelacionadoSelected
        End Get
        Set(ByVal value As Clientes_Relacionados)
            _ClienteRelacionadoSelected = value
            'If Not value Is Nothing Then
            '    dcProxy.DetalleClienteAgrupados.Clear()
            '    'dcProxy.Load(dcProxy.DetalleClienteAgrupadorConsultarQuery(0, ClienteAgrupadoSelected.NroDocumento, String.Empty, String.Empty, String.Empty,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoResponsable, Nothing)
            'End If
            MyBase.CambioItem("ClienteRelacionadoSelected")

        End Set
    End Property

    Private WithEvents _ClienteEncabezadoSelected As EncabezadoRelacionado
    Public Property ClienteEncabezadoSelected() As EncabezadoRelacionado
        Get
            Return _ClienteEncabezadoSelected
        End Get
        Set(ByVal value As EncabezadoRelacionado)
            _ClienteEncabezadoSelected = value
            If Not value Is Nothing And logRecargarDetalle Then 'DEMC20190523
                If Not IsNothing(ClienteEncabezadoSelected.IdEncabezado) Or ClienteEncabezadoSelected.IdEncabezado <> "" Then
                    dcProxy.Clientes_Relacionados.Clear()
                    dcProxy.Load(dcProxy.ClientesRelacionados_ConsultarQuery(ClienteEncabezadoSelected.IdEncabezado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteRelacionado, Nothing)
                End If
            End If
            MyBase.CambioItem("ClienteEncabezadoSelected")

        End Set
    End Property

    Private _HabilitarBuscador As Boolean = False
    Public Property HabilitarBuscador() As Boolean
        Get
            Return _HabilitarBuscador
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBuscador = value
            MyBase.CambioItem("HabilitarBuscador")
        End Set
    End Property

    Private _objTipoId As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property objTipoId() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _objTipoId
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _objTipoId = value
            MyBase.CambioItem("objTipoId")
        End Set
    End Property

#Region "Propiedades de las Tablas Hijas"

    Private _ListaEncabezadoRelacionado As List(Of EncabezadoRelacionado)
    Public Property ListaEncabezadoRelacionado() As List(Of EncabezadoRelacionado)
        Get
            Return _ListaEncabezadoRelacionado
        End Get
        Set(ByVal value As List(Of EncabezadoRelacionado))
            _ListaEncabezadoRelacionado = value
            If Not IsNothing(value) Then
                If IsNothing(EncabezadoRelacionadoAnterior) Then
                    ClienteEncabezadoSelected = _ListaEncabezadoRelacionado.FirstOrDefault
                Else
                    ClienteEncabezadoSelected = EncabezadoRelacionadoAnterior
                End If
            End If
            MyBase.CambioItem("ListaEncabezadoRelacionado")
            MyBase.CambioItem("ListaEncabezadoRelacionadoPaged")
        End Set
    End Property

    Public ReadOnly Property ListaEncabezadoRelacionadoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezadoRelacionado) Then
                Dim view = New PagedCollectionView(_ListaEncabezadoRelacionado)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property



#End Region

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try

            Dim NewClienteRelacionado As New EncabezadoRelacionado
            '	'TODO: Verificar cuales son los campos que deben inicializarse
            NewClienteRelacionado.IdEncabezado = EncabezadoRelacionadoPorDefecto.IdEncabezado
            NewClienteRelacionado.nrodocumento = EncabezadoRelacionadoPorDefecto.nrodocumento
            NewClienteRelacionado.TipoIdentificacion = EncabezadoRelacionadoPorDefecto.TipoIdentificacion
            NewClienteRelacionado.EstadoCliente = EncabezadoRelacionadoPorDefecto.EstadoCliente
            NewClienteRelacionado.IDEncabezadoRelacionado = EncabezadoRelacionadoPorDefecto.IDEncabezadoRelacionado
            EncabezadoRelacionadoAnterior = ClienteEncabezadoSelected
            ClienteEncabezadoSelected = NewClienteRelacionado

            If Not IsNothing(ListaClienteRelacionados) Then
                For Each li In ListaClienteRelacionados
                    ListaClienteRelacionadoAnterior.Add(li)
                Next
            End If

            MyBase.CambioItem("ClienteEncabezadoSelected")
            dcProxy.Clientes_Relacionados.Clear()
            Editando = True
            HabilitarBuscador = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            'dcProxy.Clientes_Responsables.Clear()
            dcProxy2.EncabezadoRelacionados.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy2.Load(dcProxy2.EncabezadoCRelacionadoFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, "FiltroVM")
                'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "FiltroVM")
            Else
                dcProxy2.Load(dcProxy2.EncabezadoCRelacionadoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, Nothing)
                'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaClienteRelacionado
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Comitente <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy2.EncabezadoRelacionados.Clear()
                ClienteRelacionadoAnterior = Nothing
                EncabezadoRelacionadoAnterior = Nothing
                ListaClienteRelacionadoAnterior.Clear()
                IsBusy = True
                dcProxy2.Load(dcProxy2.EncabezadoClienteRelacionadoConsultarQuery(cb.Comitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, "Busqueda")
                'dcProxy.Load(dcProxy.ClientesResponsables_ConsultarQuery(cb.Comitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "Busqueda")

                MyBase.ConfirmarBuscar()
                'cb = New CamposBusquedaClienteAgrupado
                'CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
                Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            validaciones()
            If logvalidacion = True Then
                logvalidacion = False
                Exit Sub
            End If
            IsBusy = True

            Dim strXMLDetalle As String
            strXMLDetalle = "<?xml version=" & """1.0" & """ encoding=" & """iso-8859-1" & """ ?> <Detalles>"
            For Each objDet In _ListaClienteRelacionados
                strXMLDetalle &= "<Detalle IDClientes_Relacionados=""" & CStr(System.Web.HttpUtility.HtmlEncode(objDet.IDClientes_Relacionados)) & """ IDComitente=""" & CStr(System.Web.HttpUtility.HtmlEncode(ClienteEncabezadoSelected.IdEncabezado)) & """ IDComitente_Relacionado=""" & CStr(System.Web.HttpUtility.HtmlEncode(objDet.IDComitente_Relacionado)) & """ Lider=""" & CStr(System.Web.HttpUtility.HtmlEncode(objDet.Lider)) & """ />"
            Next

            strXMLDetalle &= " </Detalles>"
            strXMLDetalle = System.Web.HttpUtility.UrlEncode(strXMLDetalle)

            'strXMLDetalleEliminar = System.Web.HttpUtility.UrlEncode(strXMLDetalleEliminar)
            If strAccion = "E" Then
                _ClienteRelacionadoSelected.strAccion = "E"
            End If

            If Not IsNothing(ClienteEncabezadoSelected.IdEncabezado) Then
                _ClienteRelacionadoSelected.IDComitente = ClienteEncabezadoSelected.IdEncabezado
                _ClienteRelacionadoSelected.strXml_Detalles = strXMLDetalle
            End If

            MyBase.CambioItem("ClienteRelacionadoSelected")
            Dim origen = "Actualizar"
            ErrorForma = ""
            HabilitarBuscador = True
            ClienteRelacionadoAnterior = ClienteRelacionadoSelected
            If Not ListaClienteRelacionados.Contains(ClienteRelacionadoSelected) Then
                origen = "Ingresar"
                ListaClienteRelacionados.Add(ClienteRelacionadoSelected)
            End If
            'IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)

            'DEMC20190220 se valida si el cliente que esta en el encabezado es diferente al que tengo marcado como lider, si se cumple modifico la variable del lider.
            If _ListaClienteRelacionados IsNot Nothing Then

                Dim intCantidadRegistroLider = (From item In _ListaClienteRelacionados Select item.IDComitente_Relacionado, item.Lider Where Lider = True And IDComitente_Relacionado <> _ClienteEncabezadoSelected.IdEncabezado).Count
                If intCantidadRegistroLider > 0 Then
                    _ClienteEncabezadoSelected.IdEncabezado = (From item In _ListaClienteRelacionados Select item.IDComitente_Relacionado, item.Lider Where Lider = True).Last.IDComitente_Relacionado
                End If
            End If
            'DEMC20190220
            objProxy4.ConsultaClientesTP(_ClienteEncabezadoSelected.IdEncabezado, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarComitenteTP, origen) 'DEMC20190212 Se coloca condicion del tipo producto del cliente al guardar.


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
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
                    If (So.Error.Message.Contains("ErrorPersonalizado,") = True) Then
                        Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                        Exit Sub
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                    End If
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                '                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)

            End If
            If So.UserState = "Ingresar" Or So.UserState = "Actualizar" Then
                IsBusy = True
                intIDRegistroGuardado = ClienteEncabezadoSelected.IdEncabezado 'DEMC20190523
                strcodingreso = ClienteEncabezadoSelected.IdEncabezado
                dcProxy.Clientes_Relacionados.Clear()
                dcProxy2.EncabezadoRelacionados.Clear()
                A2Utilidades.Mensajes.mostrarMensaje("Cliente relacionado grabado con éxito", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy2.Load(dcProxy2.EncabezadoCRelacionadoFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, "Ingresar")
                    'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "Ingresar")

                Else
                    'dcProxy2.Load(dcProxy2.EncabezadoCRelacionadoFiltrarQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, "Ingresar")
                    'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "Ingresar")
                    dcProxy2.Load(dcProxy2.EncabezadoCRelacionadoFiltrarQuery(ClienteEncabezadoSelected.IdEncabezado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, "Ingresar")

                End If
            End If
            If So.UserState = "BorrarRegistro" Then
                IsBusy = True
                dcProxy.RejectChanges()
                ' dcProxy.EncabezadoResponsables.Clear()
                dcProxy2.EncabezadoRelacionados.Clear() 'DEMC20190318
                A2Utilidades.Mensajes.mostrarMensaje("Cliente relacionado eliminado con éxito", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy2.Load(dcProxy2.EncabezadoCRelacionadoFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, "")
                Else
                    'dcProxy2.Load(dcProxy2.EncabezadoCRelacionadoFiltrarQuery(ClienteEncabezadoSelected.IdEncabezado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, "Ingresar")
                    dcProxy2.Load(dcProxy2.EncabezadoCRelacionadoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoRelacionados, "Ingresar") 'DEMC20190322
                End If
            End If
            ' So.MarkErrorAsHandled()
            ' Exit Try
            'If So.UserState = "Ingresar" Or So.UserState = "Actualizar" Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Cliente relacionado grabado con exito", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            'Else
            '    A2Utilidades.Mensajes.mostrarMensaje("Cliente relacionado eliminado con exito", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            'End If

            'If Not IsNothing(ClienteEncabezadoSelected.IdEncabezado) Or ClienteEncabezadoSelected.IdEncabezado <> "" Then
            '    dcProxy.Clientes_Relacionados.Clear()
            '    dcProxy.Load(dcProxy.ClientesRelacionados_ConsultarQuery(ClienteEncabezadoSelected.IdEncabezado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteRelacionado, Nothing)
            'End If

            HabilitarBuscador = False
            ClienteRelacionadoAnterior = Nothing
            EncabezadoRelacionadoAnterior = Nothing
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaClienteRelacionado
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda",
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ClienteEncabezadoSelected) Then
            'EncabezadoResponsableAnterior = ClienteEncabezadoSelected
            objProxy4.ConsultaClientesTP(_ClienteEncabezadoSelected.IdEncabezado, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarComitenteTP, "consulta")
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            dcProxy.RejectChanges()
            If Not IsNothing(_ClienteEncabezadoSelected) Then
                If _ClienteEncabezadoSelected.EntityState = EntityState.Detached Then
                    ClienteEncabezadoSelected = EncabezadoRelacionadoAnterior
                End If
            End If

            If Not IsNothing(_ClienteRelacionadoSelected) Then
                dcProxy2.RejectChanges()
                If Not IsNothing(_ClienteRelacionadoSelected) Then
                    If _ClienteRelacionadoSelected.EntityState = EntityState.Detached Then
                        _ClienteRelacionadoSelected = ClienteRelacionadoAnterior
                    End If
                End If

            End If
            HabilitarBuscador = False
            Editando = False
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(ClienteEncabezadoSelected) Then  'DEMC20190122 Se agrega mensaje de confirmacion a la hora de eliminar un registro.
                mostrarMensajePregunta("¿Está seguro de eliminar este registro?",
                                 Program.TituloSistema,
                                 "ELIMINARREGISTRO",
                                 AddressOf TerminaPreguntaEliminar, False)
                'If YaValido = False Then
                '    dcProxy.Clientes_Relacionados.Remove(ClienteRelacionadoSelected)
                '    dcProxy.EncabezadoRelacionados.Remove(ClienteEncabezadoSelected)
                '    ClienteRelacionadoSelected = _ListaClienteRelacionados.LastOrDefault
                '    ClienteEncabezadoSelected = _ListaEncabezadoRelacionado.LastOrDefault
                '    IsBusy = True
                '    Program.VerificarCambiosProxyServidor(dcProxy)
                '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                'End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'DEMC20190122
    Private Sub TerminaPreguntaEliminar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True 'DEMC20190318
                objProxy4.ConsultaClientesTP(_ClienteEncabezadoSelected.IdEncabezado, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarComitenteTP, "BorrarRegistro")
            Else
                Exit Sub
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "TerminaPreguntaEliminar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    'DEMC20190122

#Region "Métodos Tablas Hijas"
    Public Overrides Sub NuevoRegistroDetalle()
        Try
            If IsNothing(ClienteEncabezadoSelected.IdEncabezado) Or ClienteEncabezadoSelected.IdEncabezado = "" Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un comitente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                Select Case NombreColeccionDetalle
                    Case "cmClienteRelacionados"
                        Dim NewDetalleClienteRelacionado As New Clientes_Relacionados
                        If Not IsNothing(ListaClienteRelacionados) AndAlso _ListaClienteRelacionados.Count >= 1 Then
                            NewDetalleClienteRelacionado.IDClientes_Relacionados = (From c In _ListaClienteRelacionados Select c.IDClientes_Relacionados).Min - 1
                        Else
                            NewDetalleClienteRelacionado.IDClientes_Relacionados = -1
                        End If

                        'NewDetalleClienteRelacionado.IDComitente = ClienteEncabezadoSelected.IdEncabezado
                        'NewDetalleClienteResponsable.Activo = True
                        'NewDetalleClienteResponsable.Email_Responsable = String.Empty
                        NewDetalleClienteRelacionado.Usuario = Program.Usuario

                        If IsNothing(ListaClienteRelacionados) Then
                            ListaClienteRelacionados = dcProxy.Clientes_Relacionados
                        End If

                        ListaClienteRelacionados.Add(NewDetalleClienteRelacionado)
                        ClienteRelacionadoSelected = NewDetalleClienteRelacionado

                        MyBase.CambioItem("ClienteRelacionadoSelected")
                        MyBase.CambioItem("ListaClienteRelacionados")

                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    Public Sub validaciones()
        Try

            If Not IsNothing(ListaClienteRelacionados) Then

                If ListaClienteRelacionados.GroupBy(Function(i) i.IDComitente_Relacionado).Count < ListaClienteRelacionados.Count Then
                    logvalidacion = True
                    A2Utilidades.Mensajes.mostrarMensaje("Existe un código del cliente relacionado que ya se existe en el detalle, por favor verifique. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                Dim obj = ListaClienteRelacionados.Where(Function(li) li.Lider = True).ToList
                If IsNothing(ListaClienteRelacionados) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe de ingresar al menos un detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    Exit Sub
                ElseIf ListaClienteRelacionados.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe de ingresar al menos un detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    Exit Sub
                ElseIf obj.Count > 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe haber solo un cliente relacionado líder.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    Exit Sub
                ElseIf obj.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe marcar algún cliente relacionado como Líder.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    Exit Sub
                End If

            End If

        Catch ex As Exception
            logvalidacion = True
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los Clientes",
                             Me.ToString(), "Validaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Public Sub ConsultarComitente(comitente As String)

    '    Try
    '        objProxy4.ConsultarClienteResponsable_comitente(comitente, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarComitente, "consulta")
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos",
    '                                     Me.ToString(), "ConsultarComitente", Application.Current.ToString(), Program.Maquina, ex.InnerException)
    '        '????
    '    End Try

    'End Sub

    Private Sub TerminoConsultarComitente(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then
            If Not IsNothing(lo.Value) Then
                ExisteCliente = lo.Value
                If ExisteCliente = True Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente seleccionado ya se tiene una relación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ClienteEncabezadoSelected.Nombre = Nothing
                    ClienteEncabezadoSelected.nrodocumento = Nothing
                    ClienteEncabezadoSelected.IdEncabezado = Nothing
                    ClienteEncabezadoSelected.TipoIdentificacion = Nothing
                    ClienteEncabezadoSelected.EstadoClienteD = Nothing
                    ClienteEncabezadoSelected.Activo = False
                    'Else
                    '    HabilitarExento = False
                    '    _OrdenSelected.ExentoRetencion = 0
                    '    VisibilidadCampoExento = Visibility.Collapsed
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la clase de la orden",
                     Me.ToString(), "TerminoConsultarComitente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub


    Private Sub TerminoConsultarComitenteTP(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then
            If Not IsNothing(lo.Value) Then
                YaValido = lo.Value
            End If
            If YaValido = True Then
                If lo.UserState <> "Actualizar" And lo.UserState <> "Ingresar" Then 'DEMC20190212
                    Editando = False
                End If
                A2Utilidades.Mensajes.mostrarMensaje("No se puede guardar, editar o eliminar este cliente ya que su tipo de producto es diferente a Patrimonio autónomo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            Else
                If lo.UserState = "Actualizar" Or lo.UserState = "Ingresar" Then 'DEMC20190212
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, lo.UserState)
                    strAccion = Nothing
                Else
                    Editando = True
                    HabilitarBuscador = False
                    If lo.UserState = "BorrarRegistro" Then
                        If Not IsNothing(ClienteRelacionadoSelected) Then
                            IsBusy = True 'DEMC20190318
                            'dcProxy.Clientes_Relacionados.Remove(_ClienteRelacionadoSelected)
                            dcProxy2.EncabezadoRelacionados.Remove(_ClienteEncabezadoSelected)
                            ClienteEncabezadoSelected = ListaEncabezadoRelacionado.LastOrDefault
                            ' ClienteRelacionadoSelected = ListaClienteRelacionados.LastOrDefault
                            Program.VerificarCambiosProxyServidor(dcProxy2)
                            dcProxy2.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                        End If
                    End If
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la clase de la orden",
                     Me.ToString(), "TerminoConsultarComitente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Public Overrides Sub BorrarRegistroDetalle() 'DEMC20190122
        Try
            mostrarMensajePregunta("¿Está seguro de eliminar este detalle?",
                                Program.TituloSistema,
                                "ELIMINARREGISTRODETALLE",
                                AddressOf TerminaPreguntaEliminarDetalle, False)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'DEMC20190122
    Private Sub TerminaPreguntaEliminarDetalle(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                Select Case NombreColeccionDetalle
                    Case "cmClienteRelacionados"

                        If Not IsNothing(ListaClienteRelacionados) Then
                            If ListaClienteRelacionados.Count > 0 Then
                                If Not IsNothing(_ClienteRelacionadoSelected) Then
                                    strAccion = "E"
                                    ListaClienteRelacionados.Remove(_ListaClienteRelacionados.Where(Function(i) i.IDClientes_Relacionados = _ClienteRelacionadoSelected.IDClientes_Relacionados).First)
                                    ClienteRelacionadoSelected = _ListaClienteRelacionados.LastOrDefault
                                    If _ListaClienteRelacionados.Count > 0 Then
                                        ClienteRelacionadoSelected = _ListaClienteRelacionados.FirstOrDefault
                                    End If
                                    MyBase.CambioItem("ClienteRelacionadoSelected")
                                Else
                                    Exit Sub
                                End If
                            End If
                        End If
                End Select
            Else
                Exit Sub
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "TerminaPreguntaEliminarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    'DEMC2019012

    'Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
    '    'Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
    '    'If objResultado.DialogResult Then
    '    '    If _ListaClienteResponsable.Where(Function(i) i.IDClientes_Responsable = _ClienteResponsableSelected.IDClientes_Responsable).Count > 0 Then
    '    '        If ClienteResponsableSelected.Activo Then
    '    '            ClienteResponsableSelected.Activo = False

    '    '        Else
    '    '            ClienteResponsableSelected.Activo = True
    '    '        End If
    '    '    End If
    '    '    MyBase.CambioItem("ClienteResponsableSelected")
    '    '    MyBase.CambioItem("ListaClienteResponsable")
    '    'Else
    '    '    Exit Sub
    '    'End If
    'End Sub
#End Region

#End Region

    Private Sub _ClienteEncabezadoSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _ClienteEncabezadoSelected.PropertyChanged

    End Sub
End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaClienteRelacionado
    Implements INotifyPropertyChanged
    Private _Comitente As String
    Public Property Comitente() As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class





