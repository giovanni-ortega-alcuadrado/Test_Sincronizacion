Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ReceptoresViewModel.vb
'Generado el : 04/20/2011 11:55:30
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
Imports A2Utilidades.Mensajes

Public Class ReceptoresViewModel
    Inherits A2ControlMenu.A2ViewModel
    'Public Property cb As New CamposBusquedaReceptore
    Private ReceptorePorDefecto As Receptore
    Private ReceptoreAnterior As Receptore
    Private ReceptoreAnteriorAnt As Receptore
    Private ReceptoreEliminar As Receptore
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim dcProxyUtilidades As UtilidadesDomainContext
    Dim Consulta As Boolean = False
    Dim mensaje As String
    
    Private origen As String = String.Empty
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            dcProxyUtilidades = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ReceptoresFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptores, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerReceptorePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresPorDefecto_Completed, "Default")
                dcProxyUtilidades.Verificaparametro("VALIDAR_MONTOLIMITE_OAENRUTADAS",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "VALIDAR_MONTOLIMITE_OAENRUTADAS")

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ReceptoresViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerReceptoresPorDefecto_Completed(ByVal lo As LoadOperation(Of Receptore))
        If Not lo.HasError Then
            ReceptorePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Receptore por defecto", _
                                             Me.ToString(), "TerminoTraerReceptorePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerReceptores(ByVal lo As LoadOperation(Of Receptore))
        If Not lo.HasError Then
            Dim objTabladisponibles As New List(Of Items)
            objTabladisponibles.Add(New Items With {.Tipo = "R", .Descripcion = "Receptor"})
            objTabladisponibles.Add(New Items With {.Tipo = "L", .Descripcion = "Representante Legal"})
            objTabladisponibles.Add(New Items With {.Tipo = "A", .Descripcion = "Receptor y Representante Legal"})

            Tabladisponibles = Nothing
            Tabladisponibles = objTabladisponibles

            ListaReceptores = dcProxy.Receptores
            'If ReceptoreConsultaEliminarSelected.Existe = 0 Then
            '    For Each e In dcProxy.Receptores
            '        e.Accion = "A"

            '    Next
            'ElseIf (ReceptoreConsultaEliminarSelected.Existe = 1) Then
            '    For Each e In dcProxy.Receptores
            '        e.Accion = "R"

            '    Next
            'End If
            If dcProxy.Receptores.Count > 0 Then
                If lo.UserState = "insert" Then
                    ReceptoreSelected = ListaReceptores.First
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Receptores", _
                                             Me.ToString(), "TerminoTraerReceptore", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If

        IsBusy = False
    End Sub
    Private Sub TerminoTraerReceptorEliminar(ByVal lo As LoadOperation(Of ConsultaExiste))
        If Not lo.HasError Then
            'For Each a In ListaReceptores
            '    a.Existe = ReceptoreEliminar.Existe
            'Next
            ListaReceptoresConsultaEliminar = dcProxy.ConsultaExistes
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Receptores", _
                                             Me.ToString(), "TerminoTraerReceptore", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres

    Private Sub TerminoTraerReceptoresSistemas(ByVal lo As LoadOperation(Of ReceptoresSistema))
        If Not lo.HasError Then
            ListaReceptoresSistemas = dcProxy.ReceptoresSistemas
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresSistemas", _
                                             Me.ToString(), "TerminoTraerReceptoresSistemas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        consultar()

    End Sub

    Private Sub Terminotraerparametro(ByVal lo As InvokeOperation(Of String))
        Try
            If IsNothing(lo.Error) Then
                Dim strNombreParametro As String = lo.UserState.ToString.ToUpper

                Select Case strNombreParametro
                    Case "VALIDAR_MONTOLIMITE_OAENRUTADAS"
                        If lo.Value.ToUpper.Equals("SI") Then
                            MostrarMontoLimite = Visibility.Visible
                        Else
                            MostrarMontoLimite = Visibility.Collapsed
                        End If
                End Select

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los parametros.", _
                                            Me.ToString(), "Terminotraerparametro", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los parametros.", _
                                             Me.ToString(), "Terminotraerparametro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaReceptores As EntitySet(Of Receptore)
    Public Property ListaReceptores() As EntitySet(Of Receptore)
        Get
            Return _ListaReceptores
        End Get
        Set(ByVal value As EntitySet(Of Receptore))
            _ListaReceptores = value

            MyBase.CambioItem("ReceptoreSelected")
            MyBase.CambioItem("ListaReceptores")
            MyBase.CambioItem("ListaReceptoresPaged")
            If Not IsNothing(value) Then
                If IsNothing(ReceptoreAnterior) Then
                    ReceptoreSelected = _ListaReceptores.FirstOrDefault
                Else
                    ReceptoreSelected = ReceptoreAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaReceptoresPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaReceptores) Then
                Dim view = New PagedCollectionView(_ListaReceptores)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ReceptoreSelected As Receptore
    Public Property ReceptoreSelected() As Receptore
        Get
            Return _ReceptoreSelected
        End Get
        Set(ByVal value As Receptore)
            _ReceptoreSelected = value
            If Not value Is Nothing Then
                dcProxy.ReceptoresSistemas.Clear()

                dcProxy.Load(dcProxy.Traer_ReceptoresSistemas_ReceptoreQuery(ReceptoreSelected.Codigo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresSistemas, Nothing)

            End If
            MyBase.CambioItem("ReceptoreSelected")
        End Set
    End Property
    Private _ListaReceptoresConsultaEliminar As EntitySet(Of ConsultaExiste)
    Public Property ListaReceptoresConsultaEliminar() As EntitySet(Of ConsultaExiste)
        Get
            Return _ListaReceptoresConsultaEliminar
        End Get
        Set(ByVal value As EntitySet(Of ConsultaExiste))
            _ListaReceptoresConsultaEliminar = value
            If Not IsNothing(value) Then

                ReceptoreConsultaEliminarSelected = _ListaReceptoresConsultaEliminar.FirstOrDefault

            End If

            MyBase.CambioItem("ListaReceptoresConsultaEliminar")

        End Set
    End Property
    Private _ReceptoreConsultaEliminarSelected As ConsultaExiste
    Public Property ReceptoreConsultaEliminarSelected() As ConsultaExiste
        Get
            Return _ReceptoreConsultaEliminarSelected
        End Get
        Set(ByVal value As ConsultaExiste)
            _ReceptoreConsultaEliminarSelected = value

            MyBase.CambioItem("ReceptoreConsultaEliminarSelected")
        End Set
    End Property

    Private _Tabladisponibles As List(Of Items) = New List(Of Items)
    Public Property Tabladisponibles() As List(Of Items)
        Get
            Return _Tabladisponibles
        End Get
        Set(ByVal value As List(Of Items))
            _Tabladisponibles = value


            MyBase.CambioItem("Tabladisponibles")

        End Set

    End Property

    Private _tablaSeleccionada As Items
    Public Property tablaSeleccionada() As Items
        Get
            Return _tablaSeleccionada
        End Get
        Set(ByVal value As Items)
            _tablaSeleccionada = value
            If Not value Is Nothing Then
                MyBase.CambioItem("tablaSeleccionada")
            End If
        End Set
    End Property

    Private _EditaReg As Boolean
    Public Property EditaReg() As Boolean
        Get
            Return _EditaReg
        End Get
        Set(ByVal value As Boolean)
            _EditaReg = value
            MyBase.CambioItem("EditaReg")
        End Set
    End Property

    Private _Enabled As Boolean = False
    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
            MyBase.CambioItem("Enabled")
        End Set
    End Property

    Private _EnabledCampos As Boolean = False
    Public Property EnabledCampos() As Boolean
        Get
            Return _EnabledCampos
        End Get
        Set(ByVal value As Boolean)
            _EnabledCampos = value
            MyBase.CambioItem("EnabledCampos")
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

    Private _TextoCombos As String
    Public Property TextoCombos As String
        Get
            Return _TextoCombos
        End Get
        Set(ByVal value As String)
            _TextoCombos = value
            MyBase.CambioItem("TextoCombos")

        End Set
    End Property

    Private _TextoComboBusqueda As String
    Public Property TextoComboBusqueda() As String
        Get
            Return _TextoComboBusqueda
        End Get
        Set(ByVal value As String)
            _TextoComboBusqueda = value
            MyBase.CambioItem("TextoComboBusqueda")
        End Set
    End Property

    Private WithEvents _cb As CamposBusquedaReceptore = New CamposBusquedaReceptore
    Public Property cb() As CamposBusquedaReceptore
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaReceptore)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewReceptore As New Receptore
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewReceptore.IDComisionista = ReceptorePorDefecto.IDComisionista
            NewReceptore.IDSucComisionista = ReceptorePorDefecto.IDSucComisionista
            NewReceptore.Sucursal = Nothing
            NewReceptore.Codigo = ReceptorePorDefecto.Codigo
            NewReceptore.Nombre = ReceptorePorDefecto.Nombre
            NewReceptore.Activo = 1
            NewReceptore.Tipo = ReceptorePorDefecto.Tipo
            NewReceptore.Estado = ReceptorePorDefecto.Estado
            NewReceptore.Centro_costos = ReceptorePorDefecto.Centro_costos
            NewReceptore.Login = ReceptorePorDefecto.Login
            NewReceptore.Lider_Mesa = ReceptorePorDefecto.Lider_Mesa
            NewReceptore.Codigo_Mesa = 0
            NewReceptore.Numero_Documento = ReceptorePorDefecto.Numero_Documento
            NewReceptore.E_Mail = ReceptorePorDefecto.E_Mail
            NewReceptore.Actualizacion = ReceptorePorDefecto.Actualizacion
            NewReceptore.Usuario = Program.Usuario
            NewReceptore.IdOficina = ReceptorePorDefecto.IdOficina
            NewReceptore.IdReceptor = ReceptorePorDefecto.IdReceptor
            NewReceptore.NumTrader = ReceptorePorDefecto.NumTrader
            NewReceptore.CodSetFX = ReceptorePorDefecto.CodSetFX
            NewReceptore.RepresentanteLegalOtrosNegocios = ReceptorePorDefecto.RepresentanteLegalOtrosNegocios
            NewReceptore.IdReceptores = ReceptorePorDefecto.IdReceptores
            NewReceptore.IDReceptorSafyr = ReceptorePorDefecto.IDReceptorSafyr
            NewReceptore.Existe = 0

            Dim objTabladisponibles As New List(Of Items)
            objTabladisponibles.Add(New Items With {.Tipo = "R", .Descripcion = "Receptor"})
            objTabladisponibles.Add(New Items With {.Tipo = "L", .Descripcion = "Representante Legal"})
            objTabladisponibles.Add(New Items With {.Tipo = "A", .Descripcion = "Receptor y Representante Legal"})

            Tabladisponibles = objTabladisponibles
            'TextoCombos = Nothing
            TextoCombos = ""
            MyBase.CambioItem("TextoCombos")
            ReceptoreAnterior = ReceptoreSelected
            ReceptoreSelected = NewReceptore
            MyBase.CambioItem("Receptores")
            Editando = True
            EditaReg = True
            Enabled = True
            EnabledCampos = True
            Consulta = True
            MyBase.CambioItem("Editando")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            Consulta = False
            dcProxy.Receptores.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ReceptoresFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptores, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ReceptoresFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptores, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            Consulta = False
            If cb.Codigo <> String.Empty Or Not IsNothing(cb.Sucursal) Or Not IsNothing(cb.Codigo_Mesa) Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Receptores.Clear()
                ReceptoreAnterior = Nothing
                IsBusy = True

                If IsNothing(cb.Sucursal) Then
                    cb.Sucursal = 0
                End If

                If IsNothing(cb.Codigo_Mesa) Then
                    cb.Codigo_Mesa = 0
                End If
                'DescripcionFiltroVM = " Codigo = " &  cb.Codigo.ToString() 
                dcProxy.Load(dcProxy.ReceptoresConsultarQuery(cb.Codigo, cb.Sucursal, cb.Codigo_Mesa, cb.Nombre,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptores, "Busqueda")
                MyBase.ConfirmarBuscar()
                BusquedaPorDefecto()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If ReceptoreSelected.Codigo_Mesa = 0 Or IsNothing(ReceptoreSelected.Codigo_Mesa) Then
                A2Utilidades.Mensajes.mostrarMensaje("El Código de la mesa es un dato requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'CFMA20172510
            If ReceptoreSelected.IdOficina = 0 Or IsNothing(ReceptoreSelected.IdOficina) Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor ingrese el Código de la Oficina, es un dato requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            'CFMA20172510


            Consulta = False
            Dim origen = "update"
            ErrorForma = ""
            'ReceptoreAnterior = ReceptoreSelected
            If Not ListaReceptores.Where(Function(i) i.IdReceptores = _ReceptoreSelected.IdReceptores).Count > 0 Then
                origen = "insert"
                For Each a In ListaReceptores
                    If ReceptoreSelected.Codigo.Equals(a.Codigo) Then
                        mensaje = a.Codigo
                    End If
                Next
                If (ReceptoreSelected.Codigo.Equals(mensaje) And (origen = "insert")) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El código del Receptor ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    ListaReceptores.Add(ReceptoreSelected)
                End If

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
    ''' <summary>
    ''' Descripción:     Se colocaron condiciones para evitar que se intentarara acceder a los objetos ReceptoresSelected, dcProxy.Receptores cuando esten null 
    ''' Responsable:     Yessid Andres Paniagua Pabon (AlCuadrado)
    ''' Fecha:           20151217
    ''' ID del cambio:   YAPP20151217
    ''' </summary>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False

            If So.HasError Then
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío

                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                        Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        'A2Utilidades.Mensajes.mostrarMensaje(ValidaMensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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

                If So.UserState = "BorrarRegistro" Or So.UserState = "update" Then

                    dcProxy.RejectChanges()

                End If
                So.MarkErrorAsHandled()
                Exit Try
            ElseIf So.UserState = "InactivarRegistro" Then
                'YAPP20151217
                If Not IsNothing(ReceptoreSelected) Then
                    ReceptoreSelected.Activo = False
                End If
                'YAPP20151217
                If Not IsNothing(dcProxy.Receptores) Then
                    dcProxy.Receptores.Clear()
                End If

                dcProxy.Load(dcProxy.ReceptoresFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptores, "insert")
            End If
                MyBase.TerminoSubmitChanges(So)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar** en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ReceptoreSelected) Then
            Editando = True
            EditaReg = False
            Consulta = False

            If Program.VersionAplicacionCliente = EnumVersionAplicacionCliente.G.ToString Then 'Si es Genérico
                Enabled = True
            ElseIf Program.VersionAplicacionCliente = EnumVersionAplicacionCliente.C.ToString Then 'Si es City
                Enabled = False
            End If
            EnabledCampos = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaReceptore
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            Consulta = False
            ErrorForma = ""
            If Not IsNothing(_ReceptoreSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Enabled = False
                EnabledCampos = False

                If _ReceptoreSelected.EntityState = EntityState.Detached Then

                    ReceptoreSelected = ReceptoreAnterior
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub consultar()
        'SLB se le adiciona el try catch para que muestre el error personalizado.
        Try
            If Consulta = False Then
                If Not IsNothing(ReceptoreSelected) Then
                    dcProxy.ConsultaExistes.Clear()
                    dcProxy.Load(dcProxy.ReceptoresConsultarEliminarQuery(ReceptoreSelected.Codigo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptorEliminar, Nothing)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los Receptor que se puede Eliminar", _
                     Me.ToString(), "consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    ''' <summary>
    ''' Descripción:     Se cambio la forma de preguntar para evitar que se enviara varios submit changes 
    ''' Responsable:     Yessid Andres Paniagua Pabon (AlCuadrado)
    ''' Fecha:           20151217
    ''' ID del cambio:   YAPP20151217
    ''' </summary>
    Public Overrides Sub BorrarRegistro()
        Try
            Consulta = False
            For Each a In ListaReceptoresConsultaEliminar
                If a.Existe = 1 And IsNothing(ReceptoreSelected) = False Then
                    ReceptoreSelected.Existe = 1
                ElseIf (a.Existe = 0) And IsNothing(ReceptoreSelected) = False Then
                    ReceptoreSelected.Existe = 0
                End If
                Exit For
            Next
            'YAPP20151217
            If ReceptoreSelected.Existe = 1 Then
                mostrarMensajePregunta("El registro no se puede eliminar.", _
                                       Program.TituloSistema, _
                                       "INACTIVARREGISTRO", _
                                       AddressOf TerminaPregunta, True, "¿Desea inactivarlo?")
            Else
                mostrarMensajePregunta("", _
                                       Program.TituloSistema, _
                                       "ELIMINARREGISTRO", _
                                       AddressOf TerminaPregunta, True, "¿Desea borrar el registro?")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al eliminar el registro.", _
                     Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Descripción:     Se cambio la forma de preguntar para evitar que se enviara varios submit changes 
    ''' Responsable:     Yessid Andres Paniagua Pabon (AlCuadrado)
    ''' Fecha:           20151217
    ''' ID del cambio:   YAPP20151217
    ''' </summary>
    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            'YAPP20151217
            If objResultado.DialogResult Then
                If objResultado.CodigoLlamado = "INACTIVARREGISTRO" Then
                    'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                    If Not IsNothing(ReceptoreSelected) Then
                        'dcProxy.Receptores.Remove(_ReceptoreSelected)
                        ReceptoreAnteriorAnt = ReceptoreSelected
                        ReceptoreAnteriorAnt.Accion = CType("A", Char?)
                        ReceptoreSelected = ReceptoreAnteriorAnt
                        ListaReceptores.Remove(ReceptoreSelected)
                        ReceptoreSelected = _ListaReceptores.LastOrDefault
                        IsBusy = True
                        'dcProxy.SubmitChanges(AddressOf terminochanges, "")
                        If Not dcProxy.IsSubmitting Then
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "InactivarRegistro")
                        End If
                    End If
                ElseIf objResultado.CodigoLlamado = "ELIMINARREGISTRO" Then
                    If Not IsNothing(ReceptoreSelected) Then

                        'dcProxy.Receptores.Remove(_ReceptoreSelected)
                        ReceptoreAnteriorAnt = ReceptoreSelected
                        ReceptoreAnteriorAnt.Accion = CType("R", Char?)
                        ReceptoreSelected = ReceptoreAnteriorAnt
                        ListaReceptores.Remove(ReceptoreSelected)
                        ReceptoreSelected = _ListaReceptores.LastOrDefault
                        IsBusy = True
                        Program.VerificarCambiosProxyServidor(dcProxy)
                        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                    End If
                End If
            Else
                IsBusy = False
                Exit Sub
                'dcProxy.RejectChanges()
                'IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta.", _
                     Me.ToString(), "TerminaPregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("Sucursal", 1)
        DicCamposTab.Add("Codigo_Mesa", 1)
        DicCamposTab.Add("Codigo", 1)
        DicCamposTab.Add("Nombre", 1)
        DicCamposTab.Add("Tipo", 1)
        DicCamposTab.Add("Oficina", 1)
    End Sub
    Public Overrides Sub Buscar()
        BusquedaPorDefecto()
        MyBase.Buscar()
    End Sub

    Public Sub BusquedaPorDefecto()
        Try
            Dim CamposBusqueda As New CamposBusquedaReceptore
            CamposBusqueda.Sucursal = Nothing
            CamposBusqueda.Nombre = Nothing
            CamposBusqueda.Codigo_Mesa = Nothing
            CamposBusqueda.Codigo = Nothing
            cb = CamposBusqueda
            TextoComboBusqueda = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los valores de busqueda por defecto.", _
             Me.ToString(), "BusquedaPorDefecto", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

#End Region

#Region "Tablas hijas"


    '******************************************************** ReceptoresSistemas 
    Private _ListaReceptoresSistemas As EntitySet(Of ReceptoresSistema)
    Public Property ListaReceptoresSistemas() As EntitySet(Of ReceptoresSistema)
        Get
            Return _ListaReceptoresSistemas
        End Get
        Set(ByVal value As EntitySet(Of ReceptoresSistema))
            _ListaReceptoresSistemas = value
            MyBase.CambioItem("ListaReceptoresSistemas")
            MyBase.CambioItem("ReceptoresSistemasPaged")
        End Set
    End Property

    Public ReadOnly Property ReceptoresSistemasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaReceptoresSistemas) Then
                Dim view = New PagedCollectionView(_ListaReceptoresSistemas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ReceptoresSistemaSelected As ReceptoresSistema
    Public Property ReceptoresSistemaSelected() As ReceptoresSistema
        Get
            Return _ReceptoresSistemaSelected
        End Get
        Set(ByVal value As ReceptoresSistema)
            _ReceptoresSistemaSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("ReceptoresSistemaSelected")
            End If
        End Set
    End Property

    Private _MostrarMontoLimite As Visibility
    Public Property MostrarMontoLimite() As Visibility
        Get
            Return _MostrarMontoLimite
        End Get
        Set(ByVal value As Visibility)
            _MostrarMontoLimite = value
            MyBase.CambioItem("MostrarMontoLimite")
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptoresSistema"
                Dim NewReceptoresSistema As New ReceptoresSistema
                NewReceptoresSistema.Codigo = ReceptoreSelected.Codigo
                ListaReceptoresSistemas.Add(NewReceptoresSistema)
                ReceptoresSistemaSelected = NewReceptoresSistema
                MyBase.CambioItem("ReceptoresSistemaSelected")
                MyBase.CambioItem("ListaReceptoresSistema")

        End Select
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptoresSistema"
                If Not IsNothing(ListaReceptoresSistemas) Then
                    If Not IsNothing(ListaReceptoresSistemas) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptoresSistemaSelected, ListaReceptoresSistemas.ToList)

                        ListaReceptoresSistemas.Remove(_ReceptoresSistemaSelected)
                        If ListaReceptoresSistemas.Count > 0 Then
                            Program.PosicionarItemLista(ReceptoresSistemaSelected, ListaReceptoresSistemas.ToList, intRegistroPosicionar)
                        Else
                            ReceptoresSistemaSelected = Nothing
                        End If
                        MyBase.CambioItem("ReceptoresSistemaSelected")
                        MyBase.CambioItem("ListaReceptoresSistemas")
                    End If
                End If

        End Select
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaReceptore


    Implements INotifyPropertyChanged

    Private _Codigo As String
    <StringLength(4, ErrorMessage:="La longitud máxima es de 4")> _
     <Display(Name:="Código")> _
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    Private _Codigo_Mesa As Nullable(Of Integer)
    <Display(Name:="Código Mesa")> _
    Public Property Codigo_Mesa() As Nullable(Of Integer)
        Get
            Return _Codigo_Mesa
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Codigo_Mesa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo_Mesa"))
        End Set
    End Property

    Private _Sucursal As Nullable(Of Integer)
    <Display(Name:="Sucursal")> _
    Public Property Sucursal() As Nullable(Of Integer)
        Get
            Return _Sucursal
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Sucursal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Sucursal"))
        End Set
    End Property

    Private _Oficina As Nullable(Of Integer)
    <Display(Name:="Oficina")> _
    Public Property Oficina() As Nullable(Of Integer)
        Get
            Return Oficina
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Oficina = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Oficina"))
        End Set
    End Property

    Private _Nombre As String
    <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")> _
    <Display(Name:="Nombre")> _
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

Public Class Items
    Implements INotifyPropertyChanged

    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Tipo", Description:="Tipo")> _
    Private _Tipo As String
    Public Property Tipo() As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tipo"))
        End Set
    End Property


    <Display(Name:="Descripcion", Description:="Descripcion")> _
    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property



    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class




