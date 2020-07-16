Imports System.ComponentModel
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Public Class CuentasDepositoClienteViewModel
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
    Public Property CodigoComitente() As String
        Get
            Return (_mstrIdComitente)
        End Get
        Set(ByVal value As String)
            If Not _mstrIdComitente.Equals(value) Then
                _mstrIdComitente = value

                Me.consultarCuentasDeposito()
            End If
        End Set
    End Property

    Private _mintCuentaDeposito As Integer = -1
    Public Property CuentaDeposito() As Integer
        Get
            Return (_mintCuentaDeposito)
        End Get
        Set(ByVal value As Integer)
            If Not _mintCuentaDeposito.Equals(value) Then
                _mintCuentaDeposito = value

                buscarCuentaDeposito()
            End If
        End Set
    End Property

    Private _mintNroCuentaSeleccionada As Integer
    Public Property NroCuentaSeleccionada() As Integer
        Get
            Return _mintNroCuentaSeleccionada
        End Get
        Set(ByVal value As Integer)
            _mintNroCuentaSeleccionada = value
        End Set
    End Property


    Private _mobjCuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property CuentasDeposito() As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return (_mobjCuentasDeposito)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _mobjCuentasDeposito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentasDeposito"))
        End Set
    End Property

    Private _mobjCuentaDepositoSeleccionada As OYDUtilidades.BuscadorCuentasDeposito
    Public Property CuentaDepositoSeleccionada() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCuentaDepositoSeleccionada)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorCuentasDeposito)
            _mobjCuentaDepositoSeleccionada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaDepositoSeleccionada"))
        End Set
    End Property

    Private _TraerTodosLosDepositos As Boolean
    Public Property TraerTodosLosDepositos() As Boolean
        Get
            Return _TraerTodosLosDepositos
        End Get
        Set(ByVal value As Boolean)
            _TraerTodosLosDepositos = value
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

    Friend Sub consultarCuentasDeposito()
        Try
            mdcProxy.BuscadorCuentasDepositos.Clear()
            mdcProxy.Load(mdcProxy.buscarCuentasDepositoComitenteQuery(Me.CodigoComitente, TraerTodosLosDepositos, Program.Usuario, Program.HashConexion), AddressOf buscarCuentaDepositoComplete, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de cuentas depósito", Me.ToString(), "consultarCuentasDeposito", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub buscarCuentaDeposito()
        If Not CuentasDeposito Is Nothing AndAlso _mintCuentaDeposito > 0 Then
            If (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _mintCuentaDeposito Select cta).ToList.Count > 0 Then
                CuentaDepositoSeleccionada = (From cta In CuentasDeposito Where cta.NroCuentaDeposito = _mintCuentaDeposito Select cta).ToList.ElementAt(0)
            Else
                CuentaDepositoSeleccionada = Nothing
            End If
        End If
    End Sub
#End Region

#Region "Eventos respuesta de servicios"

    Private Sub buscarCuentaDepositoComplete(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        Try
            If lo.HasError Then
                If Not lo.Error Is Nothing Then
                    Throw New Exception(lo.Error.Message, lo.Error.InnerException)
                Else
                    Throw New Exception("Se presentó un error al ejecutar la consulta de cuentas depóisito pero no se recibió detalle del problema generado")
                End If
            Else
                CuentasDeposito = lo.Entities.ToList
                buscarCuentaDeposito()
            End If
        Catch ex As Exception
            CuentaDepositoSeleccionada = Nothing
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir las cuentas depósito del cliente", Me.ToString(), "buscarCuentaDepositoComplete", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
        mdcProxy.RejectChanges()
    End Sub

#End Region

End Class
