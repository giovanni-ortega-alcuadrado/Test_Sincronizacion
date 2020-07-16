Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ControlHorariosViewModel.vb
'Generado el : 11/15/2012 07:29:09
'Propiedad de Alcuadrado S.A. 2010

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

Public Class ControlHorariosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ControlHorarioPorDefecto As ControlHorario
    Private ControlHorarioAnterior As ControlHorario
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim objProxy As OyDPLUSutilidadesDomainContext
    Dim IdItemActualizar As Integer = 0
    Private _MODULOORDNES As String = String.Empty
    Private _MODULOORDNESTESORERIA As String = String.Empty

    Public Sub New()
        Try

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
                objProxy = New OyDPLUSutilidadesDomainContext()
                _HoraInicioPermitida = "07:00"
                _HoraFinPermitida = "18:00"
                _MODULOORDNES = "O"
                _MODULOORDNESTESORERIA = "OT"
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                objProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
                _HoraInicioPermitida = Program.RetornarValorProgram(Program.HoraInicio_Horario, "07:00")
                _HoraFinPermitida = Program.RetornarValorProgram(Program.HoraFin_Horario, "18:00")

                _MODULOORDNES = Program.RetornarValorProgram(Program.Modulo_Ordenes, "O")
                _MODULOORDNESTESORERIA = Program.RetornarValorProgram(Program.Modulo_OrdenesTesoreria, "OT")
            End If

            If _HoraInicioPermitida = "24:00" Then
                _HoraInicioPermitida = "00:00"
            ElseIf _HoraInicioPermitida = "00:00" Then
                _HoraInicioPermitida = "00:01"
            End If

            If _HoraFinPermitida = "24:00" Or _HoraFinPermitida = "23:59" Then
                _HoraFinPermitida = "23:58"
            End If

            Dim _HoraInicio As TimeSpan = TimeSpan.Parse(_HoraInicioPermitida)
            Dim _HoraFin As TimeSpan = TimeSpan.Parse(_HoraFinPermitida)

            _HoraInicio = _HoraInicio.Add(TimeSpan.FromMinutes(-1))
            _HoraFin = _HoraFin.Add(TimeSpan.FromMinutes(1))

            _HoraInicioPermitida = _HoraInicio.ToString().Substring(0, 5)
            _HoraFinPermitida = _HoraFin.ToString().Substring(0, 5)

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ControlHorarioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerControlHorario, "")
                dcProxy1.Load(dcProxy1.TraerControlHorarioPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerControlHorariosPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ControlHorariosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function RetornarValorProgram(ByVal strProgram As String, ByVal strRetornoOpcional As String)
        Dim objRetorno As String = String.Empty

        If Not String.IsNullOrEmpty(strProgram) Then
            objRetorno = strProgram
        Else
            objRetorno = strRetornoOpcional
        End If

        Return objRetorno
    End Function

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerControlHorariosPorDefecto_Completed(ByVal lo As LoadOperation(Of ControlHorario))
        If Not lo.HasError Then
            ControlHorarioPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ControlHorario por defecto", _
                                             Me.ToString(), "TerminoTraerControlHorarioPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerControlHorario(ByVal lo As LoadOperation(Of ControlHorario))
        If Not lo.HasError Then
            ListaControlHorario = dcProxy.ControlHorarios
            If dcProxy.ControlHorarios.Count > 0 Then
                If lo.UserState = "insert" Then
                    ControlHorarioSelected = _ListaControlHorario.Last
                ElseIf lo.UserState = "update" Then
                    If _ListaControlHorario.Where(Function(i) i.ID = IdItemActualizar).Count > 0 Then
                        ControlHorarioSelected = _ListaControlHorario.Where(Function(i) i.ID = IdItemActualizar).FirstOrDefault
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ControlHorarios", _
                                             Me.ToString(), "TerminoTraerControlHorario", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaControlHorario As EntitySet(Of ControlHorario)
    Public Property ListaControlHorario() As EntitySet(Of ControlHorario)
        Get
            Return _ListaControlHorario
        End Get
        Set(ByVal value As EntitySet(Of ControlHorario))
            _ListaControlHorario = value

            MyBase.CambioItem("ListaControlHorario")
            MyBase.CambioItem("ListaControlHorarioPaged")
            If Not IsNothing(ListaControlHorario) Then
                If ListaControlHorario.Count > 0 Then
                    ControlHorarioSelected = ListaControlHorario.FirstOrDefault
                Else
                    ControlHorarioSelected = Nothing
                End If
            Else
                ControlHorarioSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaControlHorarioPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaControlHorario) Then
                Dim view = New PagedCollectionView(_ListaControlHorario)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ControlHorarioSelected As ControlHorario
    Public Property ControlHorarioSelected() As ControlHorario
        Get
            Return _ControlHorarioSelected
        End Get
        Set(ByVal value As ControlHorario)
            _ControlHorarioSelected = value
            MyBase.CambioItem("ControlHorarioSelected")
        End Set
    End Property

    Private _HoraInicioPermitida As String
    Public Property HoraInicioPermitida() As String
        Get
            Return _HoraInicioPermitida
        End Get
        Set(ByVal value As String)
            _HoraInicioPermitida = value
            MyBase.CambioItem("HoraInicioPermitida")
        End Set
    End Property

    Private _HoraFinPermitida As String
    Public Property HoraFinPermitida() As String
        Get
            Return _HoraFinPermitida
        End Get
        Set(ByVal value As String)
            _HoraFinPermitida = value
            MyBase.CambioItem("HoraFinPermitida")
        End Set
    End Property

    Private _HabilitarOrdenes As Boolean
    Public Property HabilitarOrdenes() As Boolean
        Get
            Return _HabilitarOrdenes
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOrdenes = value
            MyBase.CambioItem("HabilitarOrdenes")
        End Set
    End Property

    Private _HabilitarTesoreria As Boolean
    Public Property HabilitarTesoreria() As Boolean
        Get
            Return _HabilitarTesoreria
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTesoreria = value
            MyBase.CambioItem("HabilitarTesoreria")
        End Set
    End Property

    Private _DiccionarioCombosEspecificos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombosEspecificos() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombosEspecificos
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombosEspecificos = value
        End Set
    End Property

    Private _NombreVistaControl As String
    Public Property NombreVistaControl() As String
        Get
            Return _NombreVistaControl
        End Get
        Set(ByVal value As String)
            _NombreVistaControl = value
        End Set
    End Property

    Private _NombreDiccionarioCombos As String
    Public Property NombreDiccionarioCombos() As String
        Get
            Return _NombreDiccionarioCombos
        End Get
        Set(ByVal value As String)
            _NombreDiccionarioCombos = value
        End Set
    End Property

    Private WithEvents _cb As CamposBusquedaControlHorario = New CamposBusquedaControlHorario
    Public Property cb() As CamposBusquedaControlHorario
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaControlHorario)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            ObtenerRegistroAnterior()

            Dim NewControlHorario As New ControlHorario
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewControlHorario.ID = ControlHorarioPorDefecto.ID
            NewControlHorario.Modulo = ControlHorarioPorDefecto.Modulo
            NewControlHorario.TipoNegocio = ControlHorarioPorDefecto.TipoNegocio
            NewControlHorario.TipoOrden = ControlHorarioPorDefecto.TipoOrden
            NewControlHorario.TipoProducto = ControlHorarioPorDefecto.TipoProducto
            NewControlHorario.Instruccion = ControlHorarioPorDefecto.Instruccion
            NewControlHorario.HoraInicio = "08:00"
            NewControlHorario.HoraFin = "18:00"
            NewControlHorario.Usuario = Program.Usuario
            ControlHorarioAnterior = _ControlHorarioSelected
            ControlHorarioSelected = NewControlHorario
            MyBase.CambioItem("ControlHorarioSelected")
            Editando = True
            MyBase.CambioItem("Editando")

            HabilitarOrdenes = False
            HabilitarTesoreria = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ControlHorarios.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ControlHorarioFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerControlHorario, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ControlHorarioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerControlHorario, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaControlHorario()
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not String.IsNullOrEmpty(_cb.Modulo) Or Not String.IsNullOrEmpty(_cb.TipoNegocio) Or Not String.IsNullOrEmpty(_cb.TipoOrden) Or Not String.IsNullOrEmpty(_cb.TipoProducto) Or Not String.IsNullOrEmpty(_cb.Instruccion) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                If Not IsNothing(dcProxy.ControlHorarios) Then
                    dcProxy.ControlHorarios.Clear()
                End If
                ControlHorarioAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " CodigoFormapago = " &  cb.CodigoFormapago.ToString() & " CodigoTipoCheque = " &  cb.CodigoTipoCheque.ToString() & " CodigoTipoCruce = " &  cb.CodigoTipoCruce.ToString()    'Dic202011 quitar

                dcProxy.Load(dcProxy.ControlHorarioConsultarQuery(_cb.Modulo, _cb.TipoNegocio, _cb.TipoOrden, _cb.TipoProducto, _cb.Instruccion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerControlHorario, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaControlHorario
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
            Dim _dtmHoraInicia As TimeSpan = TimeSpan.Parse(_HoraInicioPermitida)
            Dim _dtmHoraFinal As TimeSpan = TimeSpan.Parse(_HoraFinPermitida)
            Dim _dtmHoraInicialEscogida As TimeSpan = TimeSpan.Parse(_ControlHorarioSelected.HoraInicio)
            Dim _dtmHoraFinalEscogida As TimeSpan = TimeSpan.Parse(_ControlHorarioSelected.HoraFin)

            'Valida que la hora sea valida.
            If _dtmHoraInicialEscogida < _dtmHoraInicia Or _dtmHoraInicialEscogida > _dtmHoraFinal Then
                mostrarMensaje(String.Format("La hora de inicio no esta entre los horarios permitidos de configuración {0} y {1}", HoraInicioPermitida, HoraFinPermitida), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _dtmHoraFinalEscogida < _dtmHoraInicia Or _dtmHoraFinalEscogida > _dtmHoraFinal Then
                mostrarMensaje(String.Format("La hora de finalización no esta entre los horarios permitidos de configuración {0} y {1}", HoraInicioPermitida, HoraFinPermitida), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _dtmHoraInicialEscogida >= _dtmHoraFinalEscogida Then
                mostrarMensaje(String.Format("La hora de inicio no puede ser mayor o igual a la hora de finalización."), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim origen = "update"
            ErrorForma = ""
            If Not _ListaControlHorario.Where(Function(i) i.ID = _ControlHorarioSelected.ID).Count > 0 Then
                origen = "insert"
            End If

            If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                objProxy.ValidacionEliminarRegistros.Clear()
            End If

            If _ControlHorarioSelected.TipoNegocio = "TOD" Then _ControlHorarioSelected.TipoNegocio = Nothing
            If _ControlHorarioSelected.TipoOrden = "TOD" Then _ControlHorarioSelected.TipoOrden = Nothing
            If _ControlHorarioSelected.TipoProducto = "TOD" Then _ControlHorarioSelected.TipoProducto = Nothing
            If _ControlHorarioSelected.Instruccion = "TOD" Then _ControlHorarioSelected.Instruccion = Nothing

            If origen = "insert" Then
                objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblControlHorarios", "'intModulo'|'strTipoNegocio'|'intTipoOrden'|'intTipoProducto'|'intInstrucciones'", String.Format("'{0}'|'{1}'|'{2}'|'{3}'|'{4}'", _ControlHorarioSelected.Modulo, ObtenerValorPropiedad(_ControlHorarioSelected.TipoNegocio), ObtenerValorPropiedad(_ControlHorarioSelected.TipoOrden), ObtenerValorPropiedad(_ControlHorarioSelected.TipoProducto), ObtenerValorPropiedad(_ControlHorarioSelected.Instruccion)), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
            Else
                If ControlHorarioAnterior.Modulo <> _ControlHorarioSelected.Modulo _
                    Or ControlHorarioAnterior.TipoNegocio <> _ControlHorarioSelected.TipoNegocio _
                    Or ControlHorarioAnterior.TipoOrden <> _ControlHorarioSelected.TipoOrden _
                    Or ControlHorarioAnterior.TipoProducto <> _ControlHorarioSelected.TipoProducto _
                    Or ControlHorarioAnterior.Instruccion <> _ControlHorarioSelected.Instruccion Then
                    objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblControlHorarios", "'intModulo'|'strTipoNegocio'|'intTipoOrden'|'intTipoProducto'|'intInstrucciones'", String.Format("'{0}'|'{1}'|'{2}'|'{3}'|'{4}'", _ControlHorarioSelected.Modulo, ObtenerValorPropiedad(_ControlHorarioSelected.TipoNegocio), ObtenerValorPropiedad(_ControlHorarioSelected.TipoOrden), ObtenerValorPropiedad(_ControlHorarioSelected.TipoProducto), ObtenerValorPropiedad(_ControlHorarioSelected.Instruccion)), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
                Else
                    If origen = "insert" Then
                        ListaControlHorario.Add(_ControlHorarioSelected)
                    End If
                    IsBusy = True
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ObtenerValorPropiedad(ByVal objValor As Object) As String
        If IsNothing(objValor) Then
            Return "NULL"
        Else
            Return objValor
        End If
    End Function

    Private Sub TerminoValidarRegistro(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.ValidacionEliminarRegistro))
        Try
            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.UserState = "ELIMINAR" Then

                    Else
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            Dim origen = "update"
                            ErrorForma = ""
                            If Not _ListaControlHorario.Where(Function(i) i.ID = _ControlHorarioSelected.ID).Count > 0 Then
                                origen = "insert"
                                ListaControlHorario.Add(ControlHorarioSelected)
                            End If
                            IsBusy = True
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                        Else
                            IsBusy = False
                            mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.", _
                                                 Me.ToString(), "TerminoValidarEliminarRegistro", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.", _
                                                 Me.ToString(), "TerminoValidarEliminarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
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


            If So.UserState = "update" Or So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()

                IdItemActualizar = _ControlHorarioSelected.ID
                If Not IsNothing(dcProxy.ControlHorarios) Then
                    dcProxy.ControlHorarios.Clear()
                End If

                dcProxy.Load(dcProxy.ControlHorarioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerControlHorario, So.UserState)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ControlHorarioSelected) Then
            Editando = True
            ObtenerRegistroAnterior()
            HabilitarDeshabilitarControles(_ControlHorarioSelected.Modulo)
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ControlHorarioSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                ControlHorarioSelected = ControlHorarioAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ControlHorarioSelected) Then
                'dcProxy.ControlHorarios.Remove(_ControlHorarioSelected)
                dcProxy.ControlHorarios.Remove(dcProxy.ControlHorarios.Where(Function(i) i.ID = _ControlHorarioSelected.ID).First())

                ControlHorarioSelected = _ListaControlHorario.LastOrDefault  'Dic202011  nueva
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objControlHorario As New ControlHorario
            If Not IsNothing(_ControlHorarioSelected) Then
                objControlHorario.ID = _ControlHorarioSelected.ID
                objControlHorario.Modulo = _ControlHorarioSelected.Modulo
                objControlHorario.TipoNegocio = _ControlHorarioSelected.TipoNegocio
                objControlHorario.TipoOrden = _ControlHorarioSelected.TipoOrden
                objControlHorario.TipoProducto = _ControlHorarioSelected.TipoProducto
                objControlHorario.Instruccion = _ControlHorarioSelected.Instruccion
                objControlHorario.Usuario = _ControlHorarioSelected.Usuario
            End If

            ControlHorarioAnterior = Nothing
            ControlHorarioAnterior = objControlHorario
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function NombreCategoriaEnDiccionario(ByVal pstrCategoria As String) As String
        Return String.Format("{0}_{1}", NombreVistaControl, pstrCategoria)
    End Function

    Private Sub HabilitarDeshabilitarControles(ByVal pintModulo As String, Optional ByVal esBusqueda As Boolean = False)
        Try
            If IsNothing(DiccionarioCombosEspecificos) Then
                If Application.Current.Resources.Contains(NombreDiccionarioCombos) Then
                    DiccionarioCombosEspecificos = CType(Application.Current.Resources(NombreDiccionarioCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
                End If
            End If

            If Not IsNothing(DiccionarioCombosEspecificos) And Not IsNothing(pintModulo) Then
                If DiccionarioCombosEspecificos.Where(Function(i) i.Key = NombreCategoriaEnDiccionario("MODULOSHORARIOS")).Count > 0 Then
                    Dim listModulos As List(Of OYDUtilidades.ItemCombo)
                    listModulos = DiccionarioCombosEspecificos.Where(Function(i) i.Key = NombreCategoriaEnDiccionario("MODULOSHORARIOS")).First.Value

                    If listModulos.Where(Function(i) i.ID = pintModulo).Count > 0 Then
                        If listModulos.Where(Function(i) i.ID = pintModulo).First.ID = _MODULOORDNES Then
                            HabilitarOrdenes = True
                            HabilitarTesoreria = False
                            If esBusqueda Then
                                _cb.TipoOrden = Nothing
                                _cb.TipoNegocio = Nothing
                            End If

                            If esBusqueda Then
                                _cb.Instruccion = "TOD"
                            Else
                                _ControlHorarioSelected.Instruccion = "TOD"
                            End If
                        ElseIf listModulos.Where(Function(i) i.ID = pintModulo).First.ID = _MODULOORDNESTESORERIA Then
                            HabilitarOrdenes = False
                            HabilitarTesoreria = True
                            If esBusqueda Then
                                _cb.TipoOrden = "TOD"
                                _cb.TipoNegocio = "TOD"
                            Else
                                _ControlHorarioSelected.TipoOrden = "TOD"
                                _ControlHorarioSelected.TipoNegocio = "TOD"
                            End If
                        Else
                            HabilitarOrdenes = True
                            HabilitarTesoreria = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los controles.", _
             Me.ToString(), "HabilitarDeshabilitarControles", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    Private Sub _ControlHorarioSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ControlHorarioSelected.PropertyChanged
        Try
            If e.PropertyName.Equals("Modulo") Then
                If Editando Then
                    If Not IsNothing(_ControlHorarioSelected) Then
                        If Not IsNothing(_ControlHorarioSelected.Modulo) Then
                            HabilitarDeshabilitarControles(_ControlHorarioSelected.Modulo)
                        End If
                    End If

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_ControlHorarioSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _cb_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _cb.PropertyChanged
        Try
            If e.PropertyName.Equals("Modulo") Then
                HabilitarDeshabilitarControles(_cb.Modulo, True)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_ControlHorarioSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaControlHorario
    Implements INotifyPropertyChanged

    Private _Modulo As String
    <Display(Name:="Modulo", Description:="Modulo")> _
    Public Property Modulo() As String
        Get
            Return _Modulo
        End Get
        Set(ByVal value As String)
            _Modulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modulo"))
        End Set
    End Property

    Private _TipoNegocio As String
    <Display(Name:="Tipo negocio", Description:="Tipo negocio")> _
    Public Property TipoNegocio() As String
        Get
            Return _TipoNegocio
        End Get
        Set(ByVal value As String)
            _TipoNegocio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoNegocio"))
        End Set
    End Property

    Private _TipoOrden As String
    <Display(Name:="Tipo orden", Description:="Tipo orden")> _
    Public Property TipoOrden() As String
        Get
            Return _TipoOrden
        End Get
        Set(ByVal value As String)
            _TipoOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOrden"))
        End Set
    End Property

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

    Private _Instruccion As String
    <Display(Name:="Instrucción", Description:="Instrucción")> _
    Public Property Instruccion() As String
        Get
            Return _Instruccion
        End Get
        Set(ByVal value As String)
            _Instruccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Instruccion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class