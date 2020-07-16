Imports Telerik.Windows.Controls
Imports System.Text.RegularExpressions
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OYDPLUSUtilidades

Imports System.Xml.Linq
Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports A2Utilidades
Imports System.Windows.Data
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client

Public Class ProgramacionViewModel
    Inherits BaseViewModel

#Region "Constantes"

    Enum Recurrencias
        D       'DIARIAN
        S       'SEMANAL
        M       'MENSUAL
        A       'ANUAL
    End Enum

    Enum ModoFinalizacion
        S       'SIN FINALIZACIÓN
        N       'NRO DE RECURRENCIAS
        F       'EN UNA FECHA EN ESPECIFICO
    End Enum

    Enum TipoConsulta
        Programaciones
        Fechas
        GenerarFechas
        ActualizarProgramacion
        InactivarProgramacion
        InactivarFechas
        LogsFechas
    End Enum

#End Region

#Region "Contructores"

    Dim proxy As OyDPLUSutilidadesDomainContext = Nothing

    Public Sub New()

        Try
            If Application.Current.Resources.Contains("ListaCombosOYD") Then
                If Not IsNothing(Application.Current.Resources("ListaCombosOYD")) Then
                    ListaCombosProgramacion = CType(Application.Current.Resources("ListaCombosOYD"), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
                End If
            End If

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                proxy = New OyDPLUSutilidadesDomainContext()
            Else
                proxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            ObtenerValoresDefecto()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control 'ProgramacionViewModel'", Me.ToString(), "New", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Public viewProgramaciones As ucProgramacionesView
    Public viewProgramacionesFechas As wppProg_Fechas

    Private _ListaCombosProgramacion As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property ListaCombosProgramacion() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _ListaCombosProgramacion
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _ListaCombosProgramacion = value
            CambioItem("ListaCombosProgramacion")
        End Set
    End Property

    Private WithEvents _ProgramacionSeleccionado As tblControlProgramacion
    Public Property ProgramacionSeleccionado() As tblControlProgramacion
        Get
            Return _ProgramacionSeleccionado
        End Get
        Set(ByVal value As tblControlProgramacion)
            _ProgramacionSeleccionado = value
            CambioItem("ProgramacionSeleccionado")
        End Set
    End Property

    Private _ListaFechas As List(Of tblControlProgramacionFechas)
    Public Property ListaFechas() As List(Of tblControlProgramacionFechas)
        Get
            Return _ListaFechas
        End Get
        Set(ByVal value As List(Of tblControlProgramacionFechas))
            _ListaFechas = value
            CambioItem("ListaFechas")
            CambioItem("ListaFechasPaginada")
        End Set
    End Property

    Private _ListaFechasPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de cuentas para navegar sobre el grid con paginación
    ''' </summary>
    ''' 
    Public ReadOnly Property ListaFechasPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaFechas) Then
                Dim view = New PagedCollectionView(_ListaFechas)
                _ListaFechasPaginada = view
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _FechaSeleccionada As tblControlProgramacionFechas
    Public Property FechaSeleccionada() As tblControlProgramacionFechas
        Get
            Return _FechaSeleccionada
        End Get
        Set(ByVal value As tblControlProgramacionFechas)
            _FechaSeleccionada = value
            CambioItem("FechaSeleccionada")
        End Set
    End Property

    Private _ListaFechasGeneradas As List(Of tblControlProgramacionGeneracionFechas)
    Public Property ListaFechasGeneradas() As List(Of tblControlProgramacionGeneracionFechas)
        Get
            Return _ListaFechasGeneradas
        End Get
        Set(ByVal value As List(Of tblControlProgramacionGeneracionFechas))
            _ListaFechasGeneradas = value
            CambioItem("ListaFechasGeneradas")
            CambioItem("ListaFechasGeneradasPaginada")
        End Set
    End Property

    Private _ListaFechasGeneradasPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de cuentas para navegar sobre el grid con paginación
    ''' </summary>
    ''' 
    Public ReadOnly Property ListaFechasGeneradasPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaFechasGeneradas) Then
                Dim view = New PagedCollectionView(_ListaFechasGeneradas)
                _ListaFechasPaginada = view
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaLogsFecha As List(Of tblControlProgramacionLog)
    Public Property ListaLogsFecha() As List(Of tblControlProgramacionLog)
        Get
            Return _ListaLogsFecha
        End Get
        Set(ByVal value As List(Of tblControlProgramacionLog))
            _ListaLogsFecha = value
            CambioItem("ListaLogsFecha")
            CambioItem("ListaLogsFechaPaginada")
        End Set
    End Property

    Private _ListaLogsFechaPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de cuentas para navegar sobre el grid con paginación
    ''' </summary>
    ''' 
    Public ReadOnly Property ListaLogsFechaPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLogsFecha) Then
                Dim view = New PagedCollectionView(_ListaLogsFecha)
                _ListaLogsFechaPaginada = view
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _Modulo As String
    Public Property Modulo() As String
        Get
            Return _Modulo
        End Get
        Set(ByVal value As String)
            _Modulo = value
            RealizarLlamadosSincronicos(TipoConsulta.Programaciones)
            CambioItem("Modulo")
        End Set
    End Property

    Private _NroDocumento As Integer = 0
    Public Property NroDocumento() As Integer
        Get
            Return _NroDocumento
        End Get
        Set(ByVal value As Integer)
            _NroDocumento = value
            RealizarLlamadosSincronicos(TipoConsulta.Programaciones)
            CambioItem("NroDocumento")
        End Set
    End Property

    Private _IsBusy As Boolean
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            CambioItem("IsBusy")
        End Set
    End Property

#End Region

#Region "Propiedades recurrencia diaria"

    Private _RecurrenciaDiaria As Boolean
    Public Property RecurrenciaDiaria() As Boolean
        Get
            Return _RecurrenciaDiaria
        End Get
        Set(ByVal value As Boolean)
            _RecurrenciaDiaria = value
            CambioItem("RecurrenciaDiaria")
        End Set
    End Property

    Private _DiariaCadaDia As Boolean
    Public Property DiariaCadaDia() As Boolean
        Get
            Return _DiariaCadaDia
        End Get
        Set(ByVal value As Boolean)
            _DiariaCadaDia = value
            If _DiariaCadaDia Then
                DiariaCadaDiaHabil = False
            End If
            CambioItem("DiariaCadaDia")
        End Set
    End Property

    Private _DiariaCadaDiaHabil As Boolean
    Public Property DiariaCadaDiaHabil() As Boolean
        Get
            Return _DiariaCadaDiaHabil
        End Get
        Set(ByVal value As Boolean)
            _DiariaCadaDiaHabil = value
            If _DiariaCadaDiaHabil Then
                DiariaCadaDia = False
            End If
            CambioItem("DiariaCadaDiaHabil")
        End Set
    End Property

#End Region

#Region "Propiedades recurrencia semanal"

    Private _RecurrenciaSemenal As Boolean
    Public Property RecurrenciaSemenal() As Boolean
        Get
            Return _RecurrenciaSemenal
        End Get
        Set(ByVal value As Boolean)
            _RecurrenciaSemenal = value
            CambioItem("RecurrenciaSemenal")
        End Set
    End Property

    Private _ListaDias As List(Of clsDiasSeleccionar)
    Public Property ListaDias() As List(Of clsDiasSeleccionar)
        Get
            Return _ListaDias
        End Get
        Set(ByVal value As List(Of clsDiasSeleccionar))
            _ListaDias = value
            CambioItem("ListaDias")
        End Set
    End Property

#End Region

#Region "Propiedades recurrencia mensual"

    Private _RecurrenciaMensual As Boolean
    Public Property RecurrenciaMensual() As Boolean
        Get
            Return _RecurrenciaMensual
        End Get
        Set(ByVal value As Boolean)
            _RecurrenciaMensual = value
            CambioItem("RecurrenciaMensual")
        End Set
    End Property

    Private _MensualElDia As Boolean
    Public Property MensualElDia() As Boolean
        Get
            Return _MensualElDia
        End Get
        Set(ByVal value As Boolean)
            _MensualElDia = value
            CambioItem("MensualElDia")
        End Set
    End Property

    Private _MensualCada As Boolean
    Public Property MensualCada() As Boolean
        Get
            Return _MensualCada
        End Get
        Set(ByVal value As Boolean)
            _MensualCada = value
            CambioItem("MensualCada")
        End Set
    End Property

#End Region

#Region "Recurrencia anual"

    Private _RecurrenciaAnual As Boolean
    Public Property RecurrenciaAnual() As Boolean
        Get
            Return _RecurrenciaAnual
        End Get
        Set(ByVal value As Boolean)
            _RecurrenciaAnual = value
            CambioItem("RecurrenciaAnual")
        End Set
    End Property

    Private _AnualElDia As Boolean
    Public Property AnualElDia() As Boolean
        Get
            Return _AnualElDia
        End Get
        Set(ByVal value As Boolean)
            _AnualElDia = value
            CambioItem("AnualElDia")
        End Set
    End Property

    Private _AnualCada As Boolean
    Public Property AnualCada() As Boolean
        Get
            Return _AnualCada
        End Get
        Set(ByVal value As Boolean)
            _AnualCada = value
            CambioItem("AnualCada")
        End Set
    End Property

#End Region

#Region "Finalización de recurrencia"

    Private _FinalizacionSinFinalizacion As Boolean
    Public Property FinalizacionSinFinalizacion() As Boolean
        Get
            Return _FinalizacionSinFinalizacion
        End Get
        Set(ByVal value As Boolean)
            _FinalizacionSinFinalizacion = value
            'If Not IsNothing(_ProgramacionSeleccionado) Then DEMC20180612
            If Not IsNothing(_ProgramacionSeleccionado) And _FinalizacionSinFinalizacion = True Then
                _ProgramacionSeleccionado.FechaFinalizacion = Now.Date.AddYears(10)
            End If
            CambioItem("FinalizacionSinFinalizacion")
        End Set
    End Property

    Private _FinalizacionFinalizaDespues As Boolean
    Public Property FinalizacionFinalizaDespues() As Boolean
        Get
            Return _FinalizacionFinalizaDespues
        End Get
        Set(ByVal value As Boolean)
            _FinalizacionFinalizaDespues = value
            'If Not IsNothing(_ProgramacionSeleccionado) Then DEMC20180612
            If Not IsNothing(_ProgramacionSeleccionado) And _FinalizacionFinalizaDespues = True Then
                _ProgramacionSeleccionado.FechaFinalizacion = Now.Date.AddYears(10)
            End If
            CambioItem("FinalizacionFinalizaDespues")
        End Set
    End Property

    Private _FinalizaEl As Boolean
    Public Property FinalizaEl() As Boolean
        Get
            Return _FinalizaEl
        End Get
        Set(ByVal value As Boolean)
            _FinalizaEl = value
            'If Not IsNothing(_ProgramacionSeleccionado) Then DEMC20180612
            If Not IsNothing(_ProgramacionSeleccionado) And _FinalizaEl = True Then
                ' _ProgramacionSeleccionado.FechaFinalizacion = _ProgramacionSeleccionado.FechaInicio
                _ProgramacionSeleccionado.FechaFinalizacion = _ProgramacionSeleccionado.FechaFinalizacion
            End If
            CambioItem("FinalizaEl")
        End Set
    End Property

#End Region

#Region "Metodos"

    Private Sub ObtenerValoresDefecto()
        Try
            RecurrenciaDiaria = True
            RecurrenciaSemenal = False
            RecurrenciaMensual = False
            RecurrenciaAnual = False

            DiariaCadaDia = True
            DiariaCadaDiaHabil = False

            MensualElDia = True
            MensualCada = False

            AnualElDia = True
            AnualCada = False

            FinalizacionSinFinalizacion = True
            FinalizacionFinalizaDespues = False
            FinalizaEl = False

            Dim objListaNuevaDias As New List(Of clsDiasSeleccionar)

            If Not IsNothing(ListaCombosProgramacion("CONTROLPROGRAMACIONES_DIAS")) Then
                For Each li In ListaCombosProgramacion("CONTROLPROGRAMACIONES_DIAS").OrderBy(Function(i) i.ID)
                    AdicionarParametro(String.Empty, li.ID, li.Descripcion, objListaNuevaDias)
                Next
            End If

            ListaDias = objListaNuevaDias

            ProgramacionSeleccionado = Nothing
            ProgramacionSeleccionado = New tblControlProgramacion

            _ProgramacionSeleccionado.ID = 0
            _ProgramacionSeleccionado.DiariaDias = 1

            _ProgramacionSeleccionado.SemanalNroSemanas = 1

            _ProgramacionSeleccionado.MensualCadaDias = 13
            _ProgramacionSeleccionado.MensualCadaMes = 1
            _ProgramacionSeleccionado.MensualDias = "P"
            _ProgramacionSeleccionado.MensualTipoDia = "D"

            _ProgramacionSeleccionado.AnualDia = 13
            _ProgramacionSeleccionado.AnualMeses = Now.Date.Month.ToString
            _ProgramacionSeleccionado.AnualDias = "P"
            _ProgramacionSeleccionado.AnualTipoDia = "D"

            _ProgramacionSeleccionado.FechaInicio = Now.Date
            _ProgramacionSeleccionado.FechaFinalizacion = Now.Date.AddYears(10)
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores x defecto.", "ObtenerValoresDefecto", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ReorganizarDatos(ByVal plogGuardar As Boolean)
        Try
            If Not IsNothing(_ProgramacionSeleccionado) Then
                If plogGuardar = False Then
                    If _ProgramacionSeleccionado.TipoRecurrencia = Recurrencias.D.ToString Then
                        RecurrenciaDiaria = True
                        RecurrenciaSemenal = False
                        RecurrenciaMensual = False
                        RecurrenciaAnual = False

                        If _ProgramacionSeleccionado.DiariaCadaDia Then
                            DiariaCadaDia = True
                            DiariaCadaDiaHabil = False
                        Else
                            DiariaCadaDia = False
                            DiariaCadaDiaHabil = True
                        End If
                    ElseIf _ProgramacionSeleccionado.TipoRecurrencia = Recurrencias.S.ToString Then
                        RecurrenciaDiaria = False
                        RecurrenciaSemenal = True
                        RecurrenciaMensual = False
                        RecurrenciaAnual = False

                        Dim objListaNuevaDias As New List(Of clsDiasSeleccionar)

                        If Not String.IsNullOrEmpty(_ProgramacionSeleccionado.SemanalDiasSemana) Then
                            For Each li In ListaCombosProgramacion("CONTROLPROGRAMACIONES_DIAS")
                                AdicionarParametro(_ProgramacionSeleccionado.SemanalDiasSemana, li.ID, li.Descripcion, objListaNuevaDias)
                            Next
                        End If

                        ListaDias = objListaNuevaDias
                    ElseIf _ProgramacionSeleccionado.TipoRecurrencia = Recurrencias.M.ToString Then
                        RecurrenciaDiaria = False
                        RecurrenciaSemenal = False
                        RecurrenciaMensual = True
                        RecurrenciaAnual = False

                        If _ProgramacionSeleccionado.MensualElDia Then
                            MensualElDia = True
                            MensualCada = False
                        Else
                            MensualElDia = False
                            MensualCada = True
                        End If
                    ElseIf _ProgramacionSeleccionado.TipoRecurrencia = Recurrencias.A.ToString Then
                        RecurrenciaDiaria = False
                        RecurrenciaSemenal = False
                        RecurrenciaMensual = False
                        RecurrenciaAnual = True

                        If _ProgramacionSeleccionado.AnualElDia Then
                            AnualElDia = True
                            AnualCada = False
                        Else
                            AnualElDia = False
                            AnualCada = True
                        End If
                    End If

                    If _ProgramacionSeleccionado.ModoFinalizacion = ModoFinalizacion.S.ToString Then
                        FinalizacionSinFinalizacion = True
                        FinalizacionFinalizaDespues = False
                        FinalizaEl = False
                    ElseIf _ProgramacionSeleccionado.ModoFinalizacion = ModoFinalizacion.N.ToString Then
                        FinalizacionSinFinalizacion = False
                        FinalizacionFinalizaDespues = True
                        FinalizaEl = False
                    Else
                        FinalizacionSinFinalizacion = False
                        FinalizacionFinalizaDespues = False
                        FinalizaEl = True
                    End If
                Else
                    If RecurrenciaDiaria Then
                        _ProgramacionSeleccionado.TipoRecurrencia = Recurrencias.D.ToString

                        If DiariaCadaDia Then
                            _ProgramacionSeleccionado.DiariaCadaDia = True
                        Else
                            _ProgramacionSeleccionado.DiariaCadaDia = False
                        End If
                    ElseIf RecurrenciaSemenal Then
                        _ProgramacionSeleccionado.TipoRecurrencia = Recurrencias.S.ToString
                        _ProgramacionSeleccionado.SemanalDiasSemana = String.Empty

                        For Each li In _ListaDias
                            If li.Seleccionada Then
                                _ProgramacionSeleccionado.SemanalDiasSemana = String.Format("{0}{1}", _ProgramacionSeleccionado.SemanalDiasSemana, li.ID)
                            End If
                        Next
                    ElseIf RecurrenciaMensual Then
                        _ProgramacionSeleccionado.TipoRecurrencia = Recurrencias.M.ToString

                        If MensualElDia Then
                            _ProgramacionSeleccionado.MensualElDia = True
                        Else
                            _ProgramacionSeleccionado.MensualElDia = False
                        End If
                    ElseIf RecurrenciaAnual Then
                        _ProgramacionSeleccionado.TipoRecurrencia = Recurrencias.A.ToString

                        If AnualElDia Then
                            _ProgramacionSeleccionado.AnualElDia = True
                        Else
                            _ProgramacionSeleccionado.AnualElDia = False
                        End If
                    End If

                    If FinalizacionSinFinalizacion Then
                        _ProgramacionSeleccionado.ModoFinalizacion = ModoFinalizacion.S.ToString
                    ElseIf FinalizacionFinalizaDespues Then
                        _ProgramacionSeleccionado.ModoFinalizacion = ModoFinalizacion.N.ToString
                    Else
                        _ProgramacionSeleccionado.ModoFinalizacion = ModoFinalizacion.F.ToString
                    End If
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al reorganizar los datos para mostrar en pantalla.", "ReorganizarDatos", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Function ValidarProgramacion() As Boolean
        Dim logResultado As Boolean = True

        Try
            If RecurrenciaDiaria = False And RecurrenciaAnual = False And RecurrenciaMensual = False And RecurrenciaSemenal = False Then
                Mensajes.mostrarMensaje("Debe de seleccionar el tipo de recurrencia.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                logResultado = False
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar la programación.", "ValidarProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Return logResultado
    End Function

    Private Sub AdicionarParametro(ByVal pstrDiasGuardados As String, ByVal pstrID As String, ByVal pstrNombre As String, ByRef pobjListaNueva As List(Of clsDiasSeleccionar))
        Dim objDiaSeleccionar As New clsDiasSeleccionar
        objDiaSeleccionar = New clsDiasSeleccionar
        objDiaSeleccionar.ID = pstrID
        objDiaSeleccionar.Nombre = pstrNombre

        If pstrDiasGuardados.Contains(pstrID) Then
            objDiaSeleccionar.Seleccionada = True
        Else
            objDiaSeleccionar.Seleccionada = False
        End If
        pobjListaNueva.Add(objDiaSeleccionar)
    End Sub

    Public Async Sub RealizarLlamadosSincronicos(ByVal pobjTipo As TipoConsulta)
        Try
            If pobjTipo = TipoConsulta.Programaciones Then
                If Not String.IsNullOrEmpty(Modulo) And Not IsNothing(NroDocumento) And NroDocumento <> 0 Then
                    IsBusy = True
                    Await ConsultarProgramacion()
                End If
            ElseIf pobjTipo = TipoConsulta.Fechas Then
                If Not IsNothing(_ProgramacionSeleccionado) Then
                    If _ProgramacionSeleccionado.ID <> 0 Then
                        Dim objVisualizarFechas As New wppProg_Fechas()
                        Program.Modal_OwnerMainWindowsPrincipal(objVisualizarFechas)
                        objVisualizarFechas.ShowDialog()
                    Else
                        Mensajes.mostrarMensaje("La programación aun no ha sido guardada por lo tanto no tiene datos en las fechas.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            ElseIf pobjTipo = TipoConsulta.GenerarFechas Then
                If Not IsNothing(_ProgramacionSeleccionado) Then
                    ReorganizarDatos(True)
                    ListaFechasGeneradas = Nothing
                    Dim objVisualizarFechas As New wppProg_GeneracionFechas()
                    Program.Modal_OwnerMainWindowsPrincipal(objVisualizarFechas)
                    objVisualizarFechas.ShowDialog()
                End If
            ElseIf pobjTipo = TipoConsulta.ActualizarProgramacion Then
                If Not IsNothing(_ProgramacionSeleccionado) Then
                    If ValidarProgramacion() Then
                        IsBusy = True
                        ReorganizarDatos(True)
                        Await ActualizarProgramacion()
                    End If
                End If
            ElseIf pobjTipo = TipoConsulta.InactivarProgramacion Then
                If Not IsNothing(_ProgramacionSeleccionado) Then
                    If _ProgramacionSeleccionado.ID <> 0 Then
                        IsBusy = False
                        Await InactivarProgramacion()
                    Else
                        Mensajes.mostrarMensaje("La programación aun no ha sido guardada por lo tanto no tiene datos a inactivar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            ElseIf pobjTipo = TipoConsulta.InactivarFechas Then
                If Not IsNothing(_ListaFechas) Then
                    IsBusy = True
                    Await InactivarProgramacionFechas()
                End If
            ElseIf pobjTipo = TipoConsulta.LogsFechas Then
                If Not IsNothing(_FechaSeleccionada) Then
                    Dim objVisualizarLogFechas As New wppProg_Fechas_Log()
                    Program.Modal_OwnerMainWindowsPrincipal(objVisualizarLogFechas)
                    objVisualizarLogFechas.ShowDialog()
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las programaciones.", "ValidarConsultarProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Async Function ConsultarProgramacion() As Task(Of Boolean)
        Dim logRetorno As Boolean = True

        Try
            Dim objRet As LoadOperation(Of tblControlProgramacion)

            If Not IsNothing(proxy.tblControlProgramacions) Then
                proxy.tblControlProgramacions.Clear()
            End If

            IsBusy = True
            objRet = Await proxy.Load(proxy.ControlProgramacionesConsultarSyncQuery(Modulo, NroDocumento, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar las programaciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las programaciones.", "ConsultarProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                    logRetorno = False
                Else
                    If objRet.Entities.Count > 0 Then
                        ProgramacionSeleccionado = objRet.Entities.First
                        ReorganizarDatos(False)
                    End If
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las programaciones.", "ConsultarProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRetorno = False
        End Try
        IsBusy = False
        Return logRetorno
    End Function

    Public Async Function ConsultarFechasProgramacion() As Task(Of Boolean)
        Dim logRetorno As Boolean = True

        Try
            Dim objRet As LoadOperation(Of tblControlProgramacionFechas)

            If Not IsNothing(proxy.tblControlProgramacionFechas) Then
                proxy.tblControlProgramacionFechas.Clear()
            End If

            IsBusy = True
            objRet = Await proxy.Load(proxy.ControlProgramacionesConsultarFechasSyncQuery(_ProgramacionSeleccionado.ID, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar las fechas de la programaciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las fechas de la programaciones.", "ConsultarFechasProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                    logRetorno = False
                Else
                    ListaFechas = objRet.Entities.ToList
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las fecha de la programaciones.", "ConsultarFechasProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRetorno = False
        End Try
        IsBusy = False
        Return logRetorno
    End Function

    Public Async Function ConsultarFechasGeneradas() As Task(Of Boolean)
        Dim logRetorno As Boolean = True

        Try
            Dim objRet As LoadOperation(Of tblControlProgramacionGeneracionFechas)

            If Not IsNothing(proxy.tblControlProgramacionGeneracionFechas) Then
                proxy.tblControlProgramacionGeneracionFechas.Clear()
            End If

            IsBusy = True
            objRet = Await proxy.Load(proxy.ControlProgramacionesObtenerFechasSyncQuery(_ProgramacionSeleccionado.TipoRecurrencia, _ProgramacionSeleccionado.DiariaCadaDia, _ProgramacionSeleccionado.DiariaDias,
                                                                                        _ProgramacionSeleccionado.SemanalNroSemanas, _ProgramacionSeleccionado.SemanalDiasSemana, _ProgramacionSeleccionado.MensualElDia,
                                                                                        _ProgramacionSeleccionado.MensualCadaDias, _ProgramacionSeleccionado.MensualCadaMes, _ProgramacionSeleccionado.MensualDias,
                                                                                        _ProgramacionSeleccionado.MensualTipoDia, _ProgramacionSeleccionado.AnualElDia, _ProgramacionSeleccionado.AnualMeses,
                                                                                        _ProgramacionSeleccionado.AnualDia, _ProgramacionSeleccionado.AnualDias, _ProgramacionSeleccionado.AnualTipoDia,
                                                                                        _ProgramacionSeleccionado.FechaInicio, _ProgramacionSeleccionado.ModoFinalizacion, _ProgramacionSeleccionado.Repeticiones,
                                                                                        _ProgramacionSeleccionado.FechaFinalizacion, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar las fechas generadas de programaciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las fechas generadas de programaciones.", "ConsultarFechasGeneradas", Program.TituloSistema, Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                    logRetorno = False
                Else
                    ListaFechasGeneradas = objRet.Entities.ToList
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las fechas generadas de programaciones.", "ConsultarFechasGeneradas", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRetorno = False
        End Try
        IsBusy = False
        Return logRetorno
    End Function

    Public Async Function ActualizarProgramacion() As Task(Of Boolean)
        Dim logRetorno As Boolean = True

        Try
            Dim objRet As InvokeOperation(Of Boolean)

            IsBusy = True
            objRet = Await proxy.ControlProgramacionesActualizarSync(Modulo, NroDocumento, _ProgramacionSeleccionado.TipoRecurrencia, _ProgramacionSeleccionado.DiariaCadaDia, _ProgramacionSeleccionado.DiariaDias,
                                                                    _ProgramacionSeleccionado.SemanalNroSemanas, _ProgramacionSeleccionado.SemanalDiasSemana, _ProgramacionSeleccionado.MensualElDia,
                                                                    _ProgramacionSeleccionado.MensualCadaDias, _ProgramacionSeleccionado.MensualCadaMes, _ProgramacionSeleccionado.MensualDias,
                                                                    _ProgramacionSeleccionado.MensualTipoDia, _ProgramacionSeleccionado.AnualElDia, _ProgramacionSeleccionado.AnualMeses,
                                                                    _ProgramacionSeleccionado.AnualDia, _ProgramacionSeleccionado.AnualDias, _ProgramacionSeleccionado.AnualTipoDia,
                                                                    _ProgramacionSeleccionado.FechaInicio, _ProgramacionSeleccionado.ModoFinalizacion, _ProgramacionSeleccionado.Repeticiones,
                                                                    _ProgramacionSeleccionado.FechaFinalizacion, Program.Usuario, Program.HashConexion).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó al actualizar la programación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al actualizar la programación.", "ActualizarProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                    logRetorno = False
                Else
                    If objRet.Value Then
                        Mensajes.mostrarMensaje("La configuración de la programación se guardo exitosamente.", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        If Not IsNothing(viewProgramacionesFechas) Then
                            viewProgramacionesFechas.DialogResult = True
                        End If
                        If Not IsNothing(viewProgramaciones) Then
                            viewProgramaciones.DialogResult = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al actualizar la programación.", "ActualizarProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRetorno = False
        End Try
        IsBusy = False
        Return logRetorno
    End Function

    Public Async Function InactivarProgramacion() As Task(Of Boolean)
        Dim logRetorno As Boolean = True

        Try
            Dim objRet As InvokeOperation(Of Boolean)

            IsBusy = True
            objRet = Await proxy.ControlProgramacionesInactivarSync(Modulo, NroDocumento, Program.Usuario, Program.HashConexion).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presento un error al inactivar la programación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un error al inactivar la programación.", "InactivarProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                    logRetorno = False
                Else
                    If objRet.Value Then
                        Mensajes.mostrarMensaje("Se inactivo la programación exitosamente.", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        ObtenerValoresDefecto()
                    End If
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un error al inactivar la programación.", "InactivarProgramacion", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRetorno = False
        End Try
        IsBusy = False
        Return logRetorno
    End Function

    Public Async Function InactivarProgramacionFechas() As Task(Of Boolean)
        Dim logRetorno As Boolean = True

        Try
            Dim objRet As InvokeOperation(Of Boolean)
            Dim strIDInactivar As String = String.Empty

            If Not IsNothing(_ListaFechas) Then
                For Each li In _ListaFechas
                    If li.ProgramacionActiva = False Then
                        If String.IsNullOrEmpty(strIDInactivar) Then
                            strIDInactivar = li.ID.ToString
                        Else
                            strIDInactivar = String.Format("{0}|{1}", strIDInactivar, li.ID)
                        End If
                    End If
                Next
            End If

            If String.IsNullOrEmpty(strIDInactivar) Then
                viewProgramacionesFechas.DialogResult = True
                IsBusy = False
                Return False
            End If
            IsBusy = True

            objRet = Await proxy.ControlProgramacionesInactivarFechasSync(strIDInactivar, Program.Usuario, Program.HashConexion).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presento un error al inactivar las fechas la programación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un error al inactivar las fechas la programación.", "InactivarProgramacionFechas", Program.TituloSistema, Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                    logRetorno = False
                Else
                    If objRet.Value Then
                        Mensajes.mostrarMensaje("Se inactivo las fechas de programación exitosamente.", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        viewProgramacionesFechas.DialogResult = True
                    End If
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un error al inactivar las fechas la programación.", "InactivarProgramacionFechas", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRetorno = False
        End Try
        IsBusy = False
        Return logRetorno
    End Function

    Public Async Function ConsultarLogFechaGeneradas() As Task(Of Boolean)
        Dim logRetorno As Boolean = True

        Try
            Dim objRet As LoadOperation(Of tblControlProgramacionLog)

            IsBusy = True
            If Not IsNothing(proxy.tblControlProgramacionLogs) Then
                proxy.tblControlProgramacionLogs.Clear()
            End If

            objRet = Await proxy.Load(proxy.ControlProgramacionesConsultarLogSyncQuery(_FechaSeleccionada.ID, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar los logs generados de programaciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los logs generados de programaciones.", "ConsultarLogFechaGeneradas", Program.TituloSistema, Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    objRet.MarkErrorAsHandled()
                    logRetorno = False
                Else
                    ListaLogsFecha = objRet.Entities.ToList
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los logs generados de programaciones.", "ConsultarLogFechaGeneradas", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRetorno = False
        End Try
        IsBusy = False
        Return logRetorno
    End Function

#End Region

End Class

Public Class clsDiasSeleccionar
    Implements INotifyPropertyChanged


    Private _ID As String
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _Nombre As String
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Private _Seleccionada As Boolean
    Public Property Seleccionada() As Boolean
        Get
            Return _Seleccionada
        End Get
        Set(ByVal value As Boolean)
            _Seleccionada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Seleccionada"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class