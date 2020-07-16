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

Public Class ClienteResponsableViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaClienteResponsable
    Private ClienteResponsablePorDefecto As Clientes_Responsables
    Private ClienteResponsableAnterior As Clientes_Responsables
    Private EncabezadoResponsablePorDefecto As EncabezadoResponsable
    Private EncabezadoResponsableAnterior As EncabezadoResponsable
    Private ListaClienteResponsableAnterior As New List(Of Clientes_Responsables)
    'Private ClienteResponsableAnterior As Clientes_Responsables
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
    Dim expresionemail As String = Program.ExpresionRegularEmail
    Dim LogExisteRegistros As Boolean = False
    Dim contadorRespuesta As Integer = 0
    Dim CantidadValidados As Integer = 0
    Dim CantidadNovalidados As Integer = 0
    Dim Acomulador As Integer = 0

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
                dcProxy2.Load(dcProxy2.EncabezadoCResponsable_FiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoResponsable, "")
                'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerClientesResponsablesPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsablePorDefecto_Completed, "Default")
                objProxy3.Load(objProxy3.TraerEncabezadoResponsablePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoResponsablePorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClienteAgrupadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClienteResponsablePorDefecto_Completed(ByVal lo As LoadOperation(Of Clientes_Responsables))
        If Not lo.HasError Then
            ClienteResponsablePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClienteResponsablepor defecto", _
                                             Me.ToString(), "TerminoTraerClienteResponsablePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClienteResponsable(ByVal lo As LoadOperation(Of Clientes_Responsables))
        If Not lo.HasError Then
            ListaClienteResponsable = dcProxy.Clientes_Responsables
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClienteAgrupador", _
                                             Me.ToString(), "TerminoTraerClienteAgrupado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#Region "Resultados Asincrónicos Tabla Hija"

    Private Sub TerminoTraerEncabezadoResponsablePorDefecto_Completed(ByVal lo As LoadOperation(Of EncabezadoResponsable))
        If Not lo.HasError Then
            EncabezadoResponsablePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la DetalleClienteAgrupado por defecto", _
                                             Me.ToString(), "TerminoTraerDetalleClienteAgrupadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEncabezadoResponsable(ByVal lo As LoadOperation(Of EncabezadoResponsable))
        If Not lo.HasError Then
            ListaEncabezadoResponsable = dcProxy2.EncabezadoResponsables.ToList
            If dcProxy2.EncabezadoResponsables.Count > 0 Then
                'If lo.UserState = "Ingresar" Then
                '    ClienteEncabezadoSelected = ListaEncabezadoResponsable.First
                'End If
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EncabezadoResponsable", _
                                             Me.ToString(), "TerminoTraerEncabezadoResponsable", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres

#End Region
    'Tablas padres

#End Region

#Region "Propiedades"



    Private _ListaClienteResponsable As EntitySet(Of Clientes_Responsables)
    Public Property ListaClienteResponsable() As EntitySet(Of Clientes_Responsables)
        Get
            Return _ListaClienteResponsable
        End Get
        Set(ByVal value As EntitySet(Of Clientes_Responsables))
            _ListaClienteResponsable = value

            MyBase.CambioItem("ListaClienteResponsable")
            MyBase.CambioItem("ListaClienteResponsablePaged")
            If Not IsNothing(value) Then
                If IsNothing(ClienteResponsableAnterior) Then
                    If Not IsNothing(_ClienteEncabezadoSelected) Then
                        ClienteResponsableSelected = _ListaClienteResponsable.Where(Function(i) i.IDComitente = _ClienteEncabezadoSelected.IdEncabezado).FirstOrDefault
                    End If
                Else
                    ClienteResponsableSelected = ClienteResponsableAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClienteResponsablePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClienteResponsable) Then
                Dim view = New PagedCollectionView(_ListaClienteResponsable)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ClienteResponsableSelected As Clientes_Responsables
    Public Property ClienteResponsableSelected() As Clientes_Responsables
        Get
            Return _ClienteResponsableSelected
        End Get
        Set(ByVal value As Clientes_Responsables)
            _ClienteResponsableSelected = value
            'If Not value Is Nothing Then
            '    dcProxy.DetalleClienteAgrupados.Clear()
            '    'dcProxy.Load(dcProxy.DetalleClienteAgrupadorConsultarQuery(0, ClienteAgrupadoSelected.NroDocumento, String.Empty, String.Empty, String.Empty,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoResponsable, Nothing)
            'End If
            MyBase.CambioItem("ClienteResponsableSelected")

        End Set
    End Property

    Private WithEvents _ClienteEncabezadoSelected As EncabezadoResponsable
    Public Property ClienteEncabezadoSelected() As EncabezadoResponsable
        Get
            Return _ClienteEncabezadoSelected
        End Get
        Set(ByVal value As EncabezadoResponsable)
            _ClienteEncabezadoSelected = value
            If Not value Is Nothing Then
                If Not IsNothing(ClienteEncabezadoSelected.IdEncabezado) Or ClienteEncabezadoSelected.IdEncabezado <> "" Then
                    dcProxy.Clientes_Responsables.Clear()
                    dcProxy.Load(dcProxy.ClientesResponsables_ConsultarQuery(ClienteEncabezadoSelected.IdEncabezado, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, Nothing)
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

#Region "Propiedades de las Tablas Hijas"

    Private _ListaEncabezadoResponsable As List(Of EncabezadoResponsable)
    Public Property ListaEncabezadoResponsable() As List(Of EncabezadoResponsable)
        Get
            Return _ListaEncabezadoResponsable
        End Get
        Set(ByVal value As List(Of EncabezadoResponsable))
            _ListaEncabezadoResponsable = value
            If Not IsNothing(value) Then
                If IsNothing(EncabezadoResponsableAnterior) Then
                    ClienteEncabezadoSelected = _ListaEncabezadoResponsable.FirstOrDefault
                Else
                    ClienteEncabezadoSelected = EncabezadoResponsableAnterior
                End If
            End If
            MyBase.CambioItem("ListaEncabezadoResponsable")
            MyBase.CambioItem("ListaEncabezadoResponsablePaged")
        End Set
    End Property

    Public ReadOnly Property ListaEncabezadoResponsablePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezadoResponsable) Then
                Dim view = New PagedCollectionView(_ListaEncabezadoResponsable)
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
            
            Dim NewClienteResponsable As New EncabezadoResponsable
            '	'TODO: Verificar cuales son los campos que deben inicializarse
            NewClienteResponsable.IdEncabezado = EncabezadoResponsablePorDefecto.IdEncabezado
            NewClienteResponsable.nrodocumento = EncabezadoResponsablePorDefecto.nrodocumento
            NewClienteResponsable.Nombre = EncabezadoResponsablePorDefecto.Nombre
            NewClienteResponsable.IDEncabezadoResponsable = EncabezadoResponsablePorDefecto.IDEncabezadoResponsable
            EncabezadoResponsableAnterior = ClienteEncabezadoSelected
            ClienteEncabezadoSelected = NewClienteResponsable

            If Not IsNothing(ListaClienteResponsable) Then
                For Each li In ListaClienteResponsable
                    ListaClienteResponsableAnterior.Add(li)
                Next
            End If

            MyBase.CambioItem("ClienteEncabezadoSelected")
            dcProxy.Clientes_Responsables.Clear()
            Editando = True
            HabilitarBuscador = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            'dcProxy.Clientes_Responsables.Clear()
            dcProxy2.EncabezadoResponsables.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy2.Load(dcProxy2.EncabezadoCResponsable_FiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoResponsable, "FiltroVM")
                'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "FiltroVM")
            Else
                dcProxy2.Load(dcProxy2.EncabezadoCResponsable_FiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoResponsable, Nothing)
                'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaClienteResponsable
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Comitente <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                'dcProxy.Clientes_Responsables.Clear()
                dcProxy2.EncabezadoResponsables.Clear()
                ClienteResponsableAnterior = Nothing
                EncabezadoResponsableAnterior = Nothing
                ListaClienteResponsableAnterior.Clear()
                IsBusy = True
                dcProxy2.Load(dcProxy2.EncabezadoClienteResponsableConsultarQuery(cb.Comitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoResponsable, "Busqueda")
                'dcProxy.Load(dcProxy.ClientesResponsables_ConsultarQuery(cb.Comitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "Busqueda")

                MyBase.ConfirmarBuscar()
                'cb = New CamposBusquedaClienteAgrupado
                'CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
                Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub ValidaClienteInhabil(ByVal strnrodocumento As String, ByVal strnombre As String, ByVal _strmodulos As String, ByVal ID_Responsable As Integer)
        Try
            If (IsNothing(strnrodocumento) OrElse strnrodocumento.Equals("")) And (IsNothing(strnombre) OrElse strnombre.Equals("")) Then Exit Sub

            mdcProxyUtilidad01.ClienteInhabilitados.Clear()
            If Not String.IsNullOrEmpty(strnrodocumento) And String.IsNullOrEmpty(strnombre) Then


                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.ValidarClienteInhabilitadoQuery(strnrodocumento, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteInhabilitado, "CIstrNroDocumento" + "‡" + strnrodocumento + "‡" + strnombre + "‡" + _strmodulos + "‡" + (ListaDisparosAsync.Count + 1).ToString)
            Else
                mdcProxyUtilidad02.Load(mdcProxyUtilidad02.ValidarClienteInhabilitadoNombreQuery("", strnombre + "|" + ID_Responsable.ToString, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteInhabilitado, "CINombre" + "‡" + strnrodocumento + "‡" + strnombre + "‡" + _strmodulos + "‡" + (ListaDisparosAsync.Count + 1).ToString)





            End If




            If Not ListaDisparosAsync.ContainsKey(ListaDisparosAsync.Count + 1) Then
                ListaDisparosAsync.Add(ListaDisparosAsync.Count + 1, IIf(String.IsNullOrEmpty(strnrodocumento), strnombre, strnrodocumento))
            Else

                ListaDisparosAsync(ListaDisparosAsync.Count + 1) = IIf(String.IsNullOrEmpty(strnrodocumento), strnombre, strnrodocumento)

            End If

            'If strnrodocumento <> "" Then
            '    mdcProxyUtilidad01.ClienteInhabilitados.Clear()
            '    mdcProxyUtilidad01.Load(mdcProxyUtilidad01.ValidarClienteInhabilitadoQuery(strnrodocumento, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteInhabilitado, "CIstrNroDocumento" + "‡" + strnrodocumento + "‡" + strnombre + "‡" + _strmodulos + "‡" + (ListaDisparosAsync.Count + 1).ToString)
            '    If Not ListaDisparosAsync.ContainsKey(ListaDisparosAsync.Count + 1) Then
            '        ListaDisparosAsync.Add(ListaDisparosAsync.Count + 1, strnrodocumento)
            '    Else
            '        ListaDisparosAsync(ListaDisparosAsync.Count + 1) = strnrodocumento
            '    End If
            'End If
            'If strnombre <> "" Then
            '    mdcProxyUtilidad02.ClienteInhabilitados.Clear()
            '    mdcProxyUtilidad02.Load(mdcProxyUtilidad02.ValidarClienteInhabilitadoNombreQuery("", strnombre + "|" + ID_Responsable.ToString, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteInhabilitado, "CINombre" + "‡" + strnrodocumento + "‡" + strnombre + "‡" + _strmodulos + "‡" + (ListaDisparosAsync.Count + 1).ToString)
            '    If Not ListaDisparosAsync.ContainsKey(ListaDisparosAsync.Count + 1) Then
            '        ListaDisparosAsync.Add(ListaDisparosAsync.Count + 1, strnombre)
            '    Else
            '        ListaDisparosAsync(ListaDisparosAsync.Count + 1) = strnombre
            '    End If
            'End If
        Catch ex As Exception
            IsBusy = False
            ListaDisparosAsync.Clear()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los Clientes Inhabilitados",
                                 Me.ToString(), "ValidaClienteInhabil", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoConsultarClienteInhabilitado(ByVal lo As LoadOperation(Of ClienteInhabilitado))
        Try  'Se agrega Try - Catch a la funcion de consultarClienteInhabilitado CAFT20151028
            If lo.Entities.Count > 0 Then
                If Not lo.HasError Then
                    If TerminoConsultainhabil Then
                        Exit Sub
                    End If


                    Dim a = lo.UserState.split("‡")
                    Dim strNroDocumento = a(1)
                    Dim strnombre = a(2)
                    'mlogterminolistaclinton = a(4)
                    Select Case a(0)
                        Case "CIstrNroDocumento"
                            If lo.Entities.Count > 0 Then
                                Select Case a(3)
                                    Case "C"


                                        A2Utilidades.Mensajes.mostrarMensaje("El documento del cliente " & strNroDocumento.ToString & " ya existe como Inhabilitado." & vbCr &
                                                    "Pertenece a: " & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                                    "Fecha: " & lo.Entities.First.Ingreso,
                                                    Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        ListaDisparosAsync.Clear()
                                        'logvalidacion = True
                                        IsBusy = False
                                        Exit Sub
                                End Select

                            End If





                        Case "CINombre"


                            Select Case a(3)


                                Case "C"
                                    If lo.Entities.Count > 0 Then
                                        For Each li In lo.Entities.ToList()
                                            ListaClienteResponsable.Where(Function(i) i.IDClientes_Responsable = li.Comitente).First.ValidoLista = True
                                        Next
                                        CantidadValidados = ListaClienteResponsable.Where(Function(i) i.ValidoLista = True).Count
                                        LogExisteRegistros = True
                                        If ListaClienteResponsable.Where(Function(i) i.IDClientes_Responsable = lo.Entities.First.Comitente And i.ValidoLista = True).Count > 0 Then
                                            'A2Utilidades.Mensajes.mostrarMensaje("El Cliente: " & lo.Entities.First.Nombre & " Tiene semejanza con el cliente Inhabilitado " _
                                            '                             & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%",
                                            '               Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                            mostrarMensajePregunta("El Cliente: " & strnombre.ToString & " Tiene semejanza con el cliente Inhabilitado " _
                                               & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%",
                                               Program.TituloSistema,
                                               "CLIENTEINHABILITADO",
                                               AddressOf TerminoPreguntaSemejanza, True, "¿Desea continuar?")
                                            ListaDisparosAsync.Clear()
                                            'TerminoConsultainhabil = False
                                        End If


                                        IsBusy = False
                                    End If
                            End Select
                    End Select














                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los paramétros",
                                                     Me.ToString(), "TerminoConsultarClienteInhabilitado", Application.Current.ToString(), Program.Maquina, lo.Error)
                    ListaDisparosAsync.Clear()
                    lo.MarkErrorAsHandled()
                    IsBusy = False





                End If
                'If listaRespuesta.Count > 0 Then
                '    If listaRespuesta.Contains(False) Then
                '        Exit Sub
                '        listaRespuesta.Clear()
                '    Else
                '        YaValido = True
                '        ActualizarRegistro()
                '        listaRespuesta.Clear()
                '    End If

                'End If
                'If ListaDisparosAsync.Count = 0 Then
                '    Exit Sub
                'Else
                '    ListaDisparosAsync.Remove(a(4))
                '    If ListaDisparosAsync.Count = 0 Then
                '        YaValido = True
                '        ActualizarRegistro()
                '    End If
                'End If

            Else
                Acomulador = Acomulador + 1
                If Acomulador = CantidadNovalidados Then
                    YaValido = True
                    ActualizarRegistro()
                End If





            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al termino de la consulta de Cliente Inhabilitado.",
                                                 Me.ToString(), "TerminoConsultarClienteInhabilitado", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoPreguntaSemejanza(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            contadorRespuesta = contadorRespuesta + 1
            If contadorRespuesta = CantidadValidados Then
                YaValido = True
                ActualizarRegistro()
            End If
        Else
            IsBusy = False
            contadorRespuesta = 0
            CantidadValidados = 0
            Exit Sub
        End If


    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If IsNothing(ListaClienteResponsable) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de ingresar al menos un detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            ElseIf ListaClienteResponsable.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de ingresar al menos un detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            validaciones()
            If logvalidacion = True Then
                logvalidacion = False
                Exit Sub
            End If
            IsBusy = True

            Dim logEjecutarValidaciones As Boolean = False

            If YaValido = False Then
                If Not IsNothing(_ListaClienteResponsable) Then
                    If _ListaClienteResponsable.Where(Function(i) i.Activo).Count > 0 Then
                        logEjecutarValidaciones = True
                        CantidadNovalidados = _ListaClienteResponsable.Where(Function(i) i.Activo).Count
                        Acomulador = 0
                        For Each objDet In _ListaClienteResponsable
                            If objDet.Activo = True Then
                                ValidaClienteInhabil(objDet.NroDocumento_Responsable, objDet.Nombre_Responsable, "C", objDet.IDClientes_Responsable)
                            End If
                        Next
                    End If
                End If
            End If

            If YaValido And logEjecutarValidaciones = False Then
                Dim strXMLDetalle As String
                strXMLDetalle = "<?xml version=" & """1.0" & """ encoding=" & """iso-8859-1" & """ ?> <Detalles>"
                If Not IsNothing(_ListaClienteResponsable) AndAlso _ListaClienteResponsable.Count > 0 Then

                    If IsNothing(ClienteResponsableSelected) Then
                        ClienteResponsableSelected = _ListaClienteResponsable.FirstOrDefault

                    End If
                End If

                For Each objDet In _ListaClienteResponsable
                    If Not String.IsNullOrEmpty(objDet.Email_Responsable) Then
                        If Not IsValidmail(objDet.Email_Responsable) Then
                            A2Utilidades.Mensajes.mostrarMensaje("El campo Email responsable de al menos un detalle, no es válido. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                    strXMLDetalle &= "<Detalle IDClientes_Responsable=""" & CStr(HttpUtility.HtmlEncode(objDet.IDClientes_Responsable)) & """ TipoIdentificacion_Responsable=""" & CStr(HttpUtility.HtmlEncode(objDet.TipoIdentificacion_Responsable)) & """ NroDocumentoComitente=""" & CStr(HttpUtility.HtmlEncode(objDet.NroDocumento_Responsable)) & """ Nombre_Responsable=""" & CStr(HttpUtility.HtmlEncode(objDet.Nombre_Responsable)) & """ Telefono1_Responsable=""" & CStr(HttpUtility.HtmlEncode(objDet.Telefono1_Responsable)) & """ Email_Responsable=""" & CStr(HttpUtility.HtmlEncode(IIf(String.IsNullOrEmpty(objDet.Email_Responsable), "", objDet.Email_Responsable))) & """ Cargo_Responsable=""" & CStr(HttpUtility.HtmlEncode(IIf(String.IsNullOrEmpty(objDet.Cargo_Responsable), "", objDet.Cargo_Responsable))) & """ IDPoblacion=""" & CStr(HttpUtility.HtmlEncode(objDet.IDPoblacion)) & """ IDDepartamento=""" & CStr(HttpUtility.HtmlEncode(objDet.IDDepartamento)) & """ IDPais=""" & CStr(HttpUtility.HtmlEncode(objDet.IdPais)) & """ NombreCuidad=""" & CStr(HttpUtility.HtmlEncode(objDet.NombreCuidad)) & """ Tipo_Responsable=""" & CStr(HttpUtility.HtmlEncode(objDet.Tipo_Responsable)) & """ Activo=""" & IIf(objDet.Activo = True, "1", "0") & """ />"
                Next

                strXMLDetalle &= " </Detalles>"

                _ClienteResponsableSelected.strXml_Detalles = strXMLDetalle

                Dim origen = "Actualizar"
                ErrorForma = ""
                HabilitarBuscador = True
                ClienteResponsableAnterior = ClienteResponsableSelected
                If Not ListaClienteResponsable.Contains(ClienteResponsableSelected) Then
                    origen = "Ingresar"
                    ListaClienteResponsable.Add(ClienteResponsableSelected)
                End If
                'IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            End If


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
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            If So.UserState = "Ingresar" Or So.UserState = "Actualizar" Then
                IsBusy = True
                dcProxy.Clientes_Responsables.Clear()
                dcProxy2.EncabezadoResponsables.Clear()
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy2.Load(dcProxy2.EncabezadoCResponsable_FiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoResponsable, "Ingresar")
                    'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "Ingresar")

                Else
                    dcProxy2.Load(dcProxy2.EncabezadoCResponsable_FiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEncabezadoResponsable, "Ingresar")
                    'dcProxy.Load(dcProxy.ClientesResponsables_FiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClienteResponsable, "Ingresar")

                End If

                CantidadValidados = 0
                contadorRespuesta = 0
                YaValido = False

            End If
            HabilitarBuscador = False
            ClienteResponsableAnterior = Nothing
            EncabezadoResponsableAnterior = Nothing
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaClienteResponsable
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ClienteEncabezadoSelected) Then
            'EncabezadoResponsableAnterior = ClienteEncabezadoSelected
            Editando = True
            HabilitarBuscador = False
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
                    _ClienteEncabezadoSelected = EncabezadoResponsableAnterior
                End If
            End If

            If Not IsNothing(_ClienteResponsableSelected) Then
                dcProxy2.RejectChanges()
                If Not IsNothing(_ClienteResponsableSelected) Then
                    If _ClienteResponsableSelected.EntityState = EntityState.Detached Then
                        _ClienteResponsableSelected = ClienteResponsableAnterior
                    End If
                End If

            End If
            HabilitarBuscador = False
            Editando = False
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Métodos Tablas Hijas"
    Public Overrides Sub NuevoRegistroDetalle()
        Try
            If IsNothing(ClienteEncabezadoSelected.IdEncabezado) Or ClienteEncabezadoSelected.IdEncabezado = "" Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un comitente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                Select Case NombreColeccionDetalle
                    Case "cmClienteResponsable"
                        Dim NewDetalleClienteResponsable As New Clientes_Responsables
                        If Not IsNothing(ListaClienteResponsable) AndAlso _ListaClienteResponsable.Count >= 1 Then
                            NewDetalleClienteResponsable.IDClientes_Responsable = (From c In _ListaClienteResponsable Select c.IDClientes_Responsable).Min - 1
                        Else
                            NewDetalleClienteResponsable.IDClientes_Responsable = -1
                        End If

                        NewDetalleClienteResponsable.IDComitente = ClienteEncabezadoSelected.IdEncabezado
                        NewDetalleClienteResponsable.Activo = True
                        NewDetalleClienteResponsable.Email_Responsable = String.Empty
                        NewDetalleClienteResponsable.Usuario = Program.Usuario

                        If IsNothing(ListaClienteResponsable) Then
                            ListaClienteResponsable = dcProxy.Clientes_Responsables
                        End If

                        ListaClienteResponsable.Add(NewDetalleClienteResponsable)
                        ClienteResponsableSelected = NewDetalleClienteResponsable

                        MyBase.CambioItem("ClienteResponsableSelected")
                        MyBase.CambioItem("ListaClienteResponsable")

                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function IsValidmail(emailaddress As String) As Boolean
        Try
            'Dim match As Match = regex.Match(emailaddress, "^[A-Z0-9._%+-]+@(?:[A-Z0-9-]+.)+[A-Z]{2,4}$")
            'If match.Success Then
            '    Return True
            'Else
            '    Return False
            'End If
            'Return Regex.IsMatch(emailaddress, "^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$")
            Return Regex.IsMatch(emailaddress, expresionemail)
        Catch generatedExceptionName As FormatException
            Return False
        End Try
    End Function

    Public Sub validaciones()
        Try

            If Not IsNothing(ListaClienteResponsable) Then

                If ListaClienteResponsable.GroupBy(Function(i) i.NroDocumento_Responsable).Count < ListaClienteResponsable.Count Then
                    logvalidacion = True
                    A2Utilidades.Mensajes.mostrarMensaje("El número de documento ya existe en el detalle. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                For Each li In ListaClienteResponsable

                    If String.IsNullOrEmpty(li.TipoIdentificacion_Responsable) Then
                        logvalidacion = True
                        A2Utilidades.Mensajes.mostrarMensaje("Debe elegir el tipo de identificación del responsable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                    If String.IsNullOrEmpty(li.Nombre_Responsable) Then
                        logvalidacion = True
                        A2Utilidades.Mensajes.mostrarMensaje("Debe digitar el nombre del responsable", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(li.NroDocumento_Responsable) Then
                        logvalidacion = True
                        A2Utilidades.Mensajes.mostrarMensaje("Debe digitar el número de documento del responsable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(li.NroDocumento_Responsable) Then
                        logvalidacion = True
                        A2Utilidades.Mensajes.mostrarMensaje("Debe digitar el número de documento del responsable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    If IsNothing(li.IDPoblacion) Then
                        logvalidacion = True
                        A2Utilidades.Mensajes.mostrarMensaje("Debe escoger una poblacion para el responsable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(li.Tipo_Responsable) Then
                        logvalidacion = True
                        A2Utilidades.Mensajes.mostrarMensaje("Debe escoger un tipo de responsable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    If li.NroDocumento_Responsable = ClienteEncabezadoSelected.nrodocumento And li.TipoIdentificacion_Responsable = ClienteEncabezadoSelected.TipoIdentificacion Then
                        logvalidacion = True
                        A2Utilidades.Mensajes.mostrarMensaje("Un cliente no puede cliente responsable de el mismo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Next
            End If
        Catch ex As Exception
            logvalidacion = True
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los Clientes", _
                                 Me.ToString(), "Validaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarComitente(comitente As String)

        Try
            objProxy4.ConsultarClienteResponsable_comitente(comitente, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarComitente, "consulta")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos", _
                                         Me.ToString(), "ConsultarComitente", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            '????
        End Try

    End Sub

    Private Sub TerminoConsultarComitente(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then
            If Not IsNothing(lo.Value) Then
                ExisteCliente = lo.Value
                If ExisteCliente = True Then
                    A2Utilidades.Mensajes.mostrarMensaje("Ya existe este comitente en el maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ClienteEncabezadoSelected.Nombre = Nothing
                    ClienteEncabezadoSelected.nrodocumento = Nothing
                    ClienteEncabezadoSelected.IdEncabezado = Nothing
                    'Else
                    '    HabilitarExento = False
                    '    _OrdenSelected.ExentoRetencion = 0
                    '    VisibilidadCampoExento = Visibility.Collapsed
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la clase de la orden", _
                     Me.ToString(), "TerminoConsultarComitente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmClienteResponsable"

                    If Not IsNothing(ListaClienteResponsable) Then
                        If ListaClienteResponsable.Count > 0 Then
                            If Not IsNothing(_ClienteResponsableSelected) Then
                                If Not IsNothing(_ClienteResponsableSelected.Actualizacion) Then
                                    If _ClienteResponsableSelected.Activo Then
                                        mostrarMensajePregunta("Este proceso no borra el registro sino que lo inactiva, ¿Desea realizar el proceso ? ",
                                                               Program.TituloSistema,
                                                               "INACTIVAR DETALLES",
                                                               AddressOf TerminaPregunta, False)
                                    Else
                                        mostrarMensajePregunta("Este proceso no borra el registro sino que lo activa, ¿Desea realizar el proceso ? ",
                                                               Program.TituloSistema,
                                                               "ACTIVAR DETALLES",
                                                               AddressOf TerminaPregunta, False)
                                    End If
                                    Exit Sub
                                Else
                                    If _ListaClienteResponsable.Where(Function(i) i.IDClientes_Responsable = _ClienteResponsableSelected.IDClientes_Responsable).Count > 0 Then
                                        _ListaClienteResponsable.Remove(_ListaClienteResponsable.Where(Function(i) i.IDClientes_Responsable = _ClienteResponsableSelected.IDClientes_Responsable).First)
                                    End If
                                End If

                                If ListaClienteResponsable.Count > 0 Then
                                    ClienteResponsableSelected = ListaClienteResponsable.FirstOrDefault
                                End If

                                MyBase.CambioItem("ClienteResponsableSelected")
                                MyBase.CambioItem("ListaClienteResponsable")

                            Else
                                Exit Sub
                            End If
                        End If
                    End If
            End Select

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If _ListaClienteResponsable.Where(Function(i) i.IDClientes_Responsable = _ClienteResponsableSelected.IDClientes_Responsable).Count > 0 Then
                If ClienteResponsableSelected.Activo Then
                    ClienteResponsableSelected.Activo = False

                Else
                    ClienteResponsableSelected.Activo = True
                End If
            End If
            MyBase.CambioItem("ClienteResponsableSelected")
            MyBase.CambioItem("ListaClienteResponsable")
        Else
            Exit Sub
        End If
    End Sub
#End Region

#End Region

    Private Sub _ClienteEncabezadoSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _ClienteEncabezadoSelected.PropertyChanged

    End Sub
End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaClienteResponsable
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





