Imports System.ComponentModel
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Public Class OrdenantesClienteViewModel
    Implements INotifyPropertyChanged

#Region "Eventos"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE EVENTOS
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- Indicar cualquier cambio en una propiedad del objeto
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

#Region "Variables"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE VARIABLES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private mlogMostrarMensajeLog As Boolean = False
    Public mdcProxy As UtilidadesDomainContext

#End Region

#Region "Propiedades"

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    ' PROPIEDADES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private _mstrIdComitente As String = String.Empty
    Public Property IdComitente() As String
        Get
            Return (_mstrIdComitente)
        End Get
        Set(ByVal value As String)
            If Not _mstrIdComitente.Equals(value) Then
                _mstrIdComitente = value

                Me.consultarOrdenantes()
            End If
        End Set
    End Property

    Private _mstrIdOrdenante As String = String.Empty
    Public Property IdOrdenante() As String
        Get
            Return (_mstrIdOrdenante)
        End Get
        Set(ByVal value As String)
            _mstrIdOrdenante = value

            buscarOrdenante()
        End Set
    End Property

    Private _mobjOrdenantes As List(Of OYDUtilidades.BuscadorOrdenantes)
    Public Property Ordenantes() As List(Of OYDUtilidades.BuscadorOrdenantes)
        Get
            Return (_mobjOrdenantes)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorOrdenantes))
            _mobjOrdenantes = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ordenantes"))
        End Set
    End Property

    Private _mobjOrdenanteSeleccionado As OYDUtilidades.BuscadorOrdenantes
    Public Property OrdenanteSeleccionado() As OYDUtilidades.BuscadorOrdenantes
        Get
            Return (_mobjOrdenanteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorOrdenantes)
            _mobjOrdenanteSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OrdenanteSeleccionado"))
        End Set
    End Property

#End Region

#Region "Inicializaciones"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- PROCESOS DE INICIALIZACIÓN
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Public Sub New()

        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New UtilidadesDomainContext()
            Else
                mdcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            '-- Inicializar servicios
            inicializarServicios()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control", Me.ToString(), "New", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' Inicializa los proxies para acceder a los servicios web y configura los manejadores de evento de los diferentes métodos asincrónicos disponibles
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub inicializarServicios()
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control", Me.ToString(), "New", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region

#Region "Métodos privados"

    Friend Sub consultarOrdenantes()
        Try
            mdcProxy.BuscadorOrdenantes.Clear()
            mdcProxy.Load(mdcProxy.buscarOrdenantesComitenteQuery(Me.IdComitente, Program.Usuario, Program.HashConexion), AddressOf buscarOrdenantesComplete, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de ordenantes", Me.ToString(), "consultarOrdenantes", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub buscarOrdenante()
        If Not Ordenantes Is Nothing AndAlso Not _mstrIdOrdenante.Equals(String.Empty) Then
            If (From obj In Ordenantes Where obj.IdOrdenante.Trim() = _mstrIdOrdenante Select obj).ToList.Count > 0 Then
                OrdenanteSeleccionado = (From cta In Ordenantes Where cta.IdOrdenante.Trim() = _mstrIdOrdenante.Trim() Select cta).ToList.ElementAt(0)
            Else
                OrdenanteSeleccionado = Nothing
            End If
        End If
    End Sub
#End Region

#Region "Eventos respuesta de servicios"

    Private Sub buscarOrdenantesComplete(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))
        Try
            If lo.HasError Then
                If Not lo.Error Is Nothing Then
                    Throw New Exception(lo.Error.Message, lo.Error.InnerException)
                Else
                    Throw New Exception("Se presentó un error al ejecutar la consulta de los ordenantes del cliente pero no se recibió detalle del problema generado")
                End If
            Else
                Ordenantes = lo.Entities.ToList

                buscarOrdenante()
            End If
        Catch ex As Exception
            OrdenanteSeleccionado = Nothing
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los ordenantes del cliente", Me.ToString(), "buscarOrdenantesComplete", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
        mdcProxy.RejectChanges()
    End Sub

#End Region

End Class
