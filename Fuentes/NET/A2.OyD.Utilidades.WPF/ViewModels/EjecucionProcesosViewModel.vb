Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: AuditoriaViewModel.vb
'Generado el : 08/31/2011 11:43:52
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.OYDUtilidades

Public Class EjecucionProcesosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As UtilidadesDomainContext
    Dim intMinutosRefrescar As Integer = 1


    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New UtilidadesDomainContext()

            Else
                dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))

            End If
            If Not Program.IsDesignMode() Then
                IsBusy = True
                ConsultarTiposProceso()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "AuditoriaViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoConsutarTiposProcesos(ByVal lo As LoadOperation(Of OYDUtilidades.ProcesoTipos))
        Try
            If Not lo.HasError Then
                ListaTiposProceso = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de los tipos de proceso", _
                                                 Me.ToString(), "TerminoConsutarTiposProcesos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de los tipos de proceso", _
                                                Me.ToString(), "TerminoConsutarTiposProcesos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusy = False

    End Sub

    Private Sub TerminoConsultarProcesos(ByVal lo As LoadOperation(Of OYDUtilidades.Proceso))
        Try
            If Not lo.HasError Then
                ListaConsultarProcesos = lo.Entities.ToList
                IsBusy = False

                If Not IsNothing(_ListaConsultarProcesos) Then
                    If _ListaConsultarProcesos.Count > 0 Then
                        ProcesosSeleccionado = _ListaConsultarProcesos.First
                    Else
                        HabilitarEjecucionProceso = True
                    End If
                Else
                    HabilitarEjecucionProceso = True
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los procesos", _
                                                 Me.ToString(), "TerminoConsultarProcesos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los procesos", _
                                                Me.ToString(), "TerminoConsultarProcesos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

        ReiniciaTimer()
    End Sub

    Private Sub TerminoConsultarDetalleProcesoSeleccionado(ByVal lo As LoadOperation(Of OYDUtilidades.ProcesoDetalle))
        Try
            If Not lo.HasError Then
                ListaConsultarDetalleProcesoSeleccionado = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista del detalle", _
                                                 Me.ToString(), "TerminoConsultarDetalleProcesoSeleccionado", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista del detalle", _
                                                Me.ToString(), "TerminoConsultarDetalleProcesoSeleccionado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False

    End Sub

    Private Sub TerminoIniciarEjecucionProceso(ByVal lo As LoadOperation(Of OYDUtilidades.ResultadoProceso))
        Try
            If Not lo.HasError Then
                ListaIniciarEjecucionProceso = lo.Entities.ToList

                If Not IsNothing(ListaIniciarEjecucionProceso) Then
                    If ListaIniciarEjecucionProceso.Count > 0 Then
                        Dim strMensaje As String = ListaIniciarEjecucionProceso.First.Mensaje
                        If ListaIniciarEjecucionProceso.First.Exitoso Then
                            A2Utilidades.Mensajes.mostrarMensajeResultadoAsincronico(strMensaje, Program.TituloSistema, AddressOf TerminoMostrarMensajeUsuario, "INICIARPROCESO", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                            ConsultarProceso()
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                        End If
                    Else
                        IsBusy = False
                    End If
                Else
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del resultado del proceso", _
                                                 Me.ToString(), "TerminoIniciarEjecucionProceso", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del resultado del proceso", _
                                                Me.ToString(), "TerminoIniciarEjecucionProceso", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoMostrarMensajeUsuario(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado = CType(sender, A2Utilidades.wcpMensajes)

            Select Case objResultado.CodigoLlamado.ToUpper
                Case "INICIARPROCESO"
                    ConsultarProceso()
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del mensaje usuario.", Me.ToString(), "TerminoMostrarMensajeUsuario", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Propiedades"


    Private _ListaTiposProceso As List(Of OYDUtilidades.ProcesoTipos)
    Public Property ListaTiposProceso() As List(Of OYDUtilidades.ProcesoTipos)
        Get
            Return _ListaTiposProceso
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ProcesoTipos))
            _ListaTiposProceso = value
            MyBase.CambioItem("ListaTiposProceso")
        End Set
    End Property

    Private _TipoProcesoSeleccionado As OYDUtilidades.ProcesoTipos
    Public Property TipoProcesoSeleccionado() As OYDUtilidades.ProcesoTipos
        Get
            Return _TipoProcesoSeleccionado
        End Get
        Set(ByVal value As OYDUtilidades.ProcesoTipos)
            _TipoProcesoSeleccionado = value
            If Not IsNothing(_TipoProcesoSeleccionado) Then
                ConsultarProceso()
                If _TipoProcesoSeleccionado.SolicitarFecha Then
                    HabilitarFechaProceso = True
                Else
                    HabilitarFechaProceso = False
                End If
            Else
                HabilitarFechaProceso = False
            End If
            MyBase.CambioItem("TipoProcesoSeleccionado")
        End Set
    End Property

    Private _ListaConsultarProcesos As List(Of OYDUtilidades.Proceso)
    Public Property ListaConsultarProcesos() As List(Of OYDUtilidades.Proceso)
        Get
            Return _ListaConsultarProcesos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.Proceso))
            _ListaConsultarProcesos = value
            MyBase.CambioItem("ListaConsultarProcesos")
        End Set
    End Property

    Private _ProcesosSeleccionado As OYDUtilidades.Proceso
    Public Property ProcesosSeleccionado() As OYDUtilidades.Proceso
        Get
            Return _ProcesosSeleccionado
        End Get
        Set(ByVal value As OYDUtilidades.Proceso)
            _ProcesosSeleccionado = value
            If Not IsNothing(_ProcesosSeleccionado) Then
                If ProcesosSeleccionado.ProcesoActivo And TipoProcesoSeleccionado.TipoProceso = "PROCEDIMIENTO" Then
                    HabilitarEjecucionProceso = False
                Else
                    HabilitarEjecucionProceso = True
                End If
                ConsultarDetalleProceso()
            End If
            MyBase.CambioItem("ProcesosSeleccionado")
        End Set
    End Property

    Private _FechaTipoProceso As DateTime = Now
    Public Property FechaTipoProceso() As DateTime
        Get
            Return _FechaTipoProceso
        End Get
        Set(ByVal value As DateTime)
            _FechaTipoProceso = value
            MyBase.CambioItem("FechaTipoProceso")
        End Set
    End Property

    Private _HabilitarFechaProceso As Boolean = False
    Public Property HabilitarFechaProceso() As Boolean
        Get
            Return _HabilitarFechaProceso
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaProceso = value
            MyBase.CambioItem("HabilitarFechaProceso")
        End Set
    End Property

    Private _ListaConsultarDetalleProcesoSeleccionado As List(Of OYDUtilidades.ProcesoDetalle)
    Public Property ListaConsultarDetalleProcesoSeleccionado() As List(Of OYDUtilidades.ProcesoDetalle)
        Get
            Return _ListaConsultarDetalleProcesoSeleccionado
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ProcesoDetalle))
            _ListaConsultarDetalleProcesoSeleccionado = value
            MyBase.CambioItem("ListaConsultarDetalleProcesoSeleccionado")
            MyBase.CambioItem("ListaConsultarDetalleProcesoSeleccionadoPaged")
        End Set
    End Property

    Public ReadOnly Property ListaConsultarDetalleProcesoSeleccionadoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConsultarDetalleProcesoSeleccionado) Then
                Dim view = New PagedCollectionView(_ListaConsultarDetalleProcesoSeleccionado)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaIniciarEjecucionProceso As List(Of OYDUtilidades.ResultadoProceso)
    Public Property ListaIniciarEjecucionProceso() As List(Of OYDUtilidades.ResultadoProceso)
        Get
            Return _ListaIniciarEjecucionProceso
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ResultadoProceso))
            _ListaIniciarEjecucionProceso = value
            MyBase.CambioItem("ListaIniciarEjecucionProceso")
        End Set
    End Property

    Private _HabilitarEjecucionProceso As Boolean
    Public Property HabilitarEjecucionProceso() As Boolean
        Get
            Return _HabilitarEjecucionProceso
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEjecucionProceso = value
            MyBase.CambioItem("HabilitarEjecucionProceso")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Sub ConsultarTiposProceso()
        Try
            IsBusy = True
            If Not IsNothing(dcProxy.ProcesoTipos) Then
                dcProxy.ProcesoTipos.Clear()
            End If
            dcProxy.Load(dcProxy.Procesos_ConsultarTiposQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoConsutarTiposProcesos, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los tipos de procesos", _
             Me.ToString(), "ConsultarTiposProceso", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub ConsultarProceso()
        Try
            If Not IsNothing(TipoProcesoSeleccionado) Then
                IsBusy = True
                ListaConsultarDetalleProcesoSeleccionado = Nothing
                ListaConsultarProcesos = Nothing
                ProcesosSeleccionado = Nothing

                If Not IsNothing(dcProxy.Procesos) Then
                    dcProxy.Procesos.Clear()
                End If
                dcProxy.Load(dcProxy.Procesos_ConsultarQuery(TipoProcesoSeleccionado.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarProcesos, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los procesos", _
             Me.ToString(), "ConsultarTiposProceso", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub ConsultarDetalleProceso()
        Try
            If Not IsNothing(ProcesosSeleccionado) Then
                IsBusy = True
                If Not IsNothing(dcProxy.Procesos) Then
                    dcProxy.ProcesoDetalles.Clear()
                End If
                dcProxy.Load(dcProxy.Procesos_ConsultarDetalleQuery(ProcesosSeleccionado.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarDetalleProcesoSeleccionado, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los procesos", _
             Me.ToString(), "ConsultarDetalleProceso", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub ConsultarIniciarProceso()
        Try
            If Not IsNothing(TipoProcesoSeleccionado) Then

                If TipoProcesoSeleccionado.SolicitarFecha Then
                    If IsNothing(FechaTipoProceso) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccione una fecha para el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                IsBusy = True
                If Not IsNothing(dcProxy.Procesos) Then
                    dcProxy.ResultadoProcesos.Clear()
                End If

                dcProxy.Load(dcProxy.Procesos_IniciarQuery(TipoProcesoSeleccionado.ID, Program.Usuario, TipoProcesoSeleccionado.SolicitarFecha, FechaTipoProceso, Program.HashConexion), AddressOf TerminoIniciarEjecucionProceso, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los procesos", _
             Me.ToString(), "ConsultarIniciarProceso", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Timer Refrescar pantalla"

    Private _myDispatcherTimerOrdenes As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

    Public Sub ReiniciaTimer()
        Try
            If _myDispatcherTimerOrdenes Is Nothing Then
                _myDispatcherTimerOrdenes = New System.Windows.Threading.DispatcherTimer
                _myDispatcherTimerOrdenes.Interval = New TimeSpan(0, intMinutosRefrescar, 0)
                AddHandler _myDispatcherTimerOrdenes.Tick, AddressOf Me.Each_Tick
            End If
            _myDispatcherTimerOrdenes.Start()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' Para hilo del temporizador
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub pararTemporizador()
        Try
            If Not IsNothing(_myDispatcherTimerOrdenes) Then
                _myDispatcherTimerOrdenes.Stop()
                RemoveHandler _myDispatcherTimerOrdenes.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerOrdenes = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar parar el timer.", Me.ToString, "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Each_Tick(sender As Object, e As EventArgs)
        'Recarga la pantalla cuando se este en un tiempo de espera sin notificaciones y sin refrescar la pantalla.
        ConsultarProceso()
    End Sub

#End Region

End Class



