Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class CuentasDepositoClienteButton
    Inherits UserControl

#Region "Eventos"

    Public Event finalizoBusqueda(ByVal pintCuentaDeposito As Integer, ByVal pobjCuentaDeposito As OYDUtilidades.BuscadorCuentasDeposito)
#End Region

#Region "Variables"

    Private WithEvents mobjBuscador As CuentasDepositoClienteLista

#End Region

#Region "Propiedades"

    'Private _mobjComitente As OYDUtilidades.BuscadorClientes = Nothing
    'Public ReadOnly Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    '    Get
    '        Return (_mobjComitente)
    '    End Get
    'End Property

    Private _mobjCuentaDeposito As OYDUtilidades.BuscadorCuentasDeposito = Nothing
    Public ReadOnly Property CuentaSeleccionada As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCuentaDeposito)
        End Get
    End Property

    Private Shared ReadOnly IDComitenteDep As DependencyProperty = DependencyProperty.Register("IDComitente", GetType(String), GetType(CuentasDepositoClienteButton), New PropertyMetadata("", New PropertyChangedCallback(AddressOf IDComitenteChanged)))

    ''' <summary>
    ''' Indica si se realiza la carga de clientes dependiendo de un receptor en especifico
    ''' </summary>
    Public Property IDComitente As String
        Get
            Return CStr(GetValue(IDComitenteDep))
        End Get
        Set(ByVal value As String)
            SetValue(IDComitenteDep, value)
        End Set
    End Property

    Private Shared Sub IDComitenteChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCuentasDepositoCliente", "IDComitenteChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Shared ReadOnly CuentaDepositoDep As DependencyProperty = DependencyProperty.Register("CuentaDeposito", GetType(Integer), GetType(CuentasDepositoClienteButton), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf CuentaDepositoChanged)))

    ''' <summary>
    ''' Indica si se realiza la carga de clientes dependiendo de un receptor en especifico
    ''' </summary>
    Public Property CuentaDeposito As Integer
        Get
            Return CInt(GetValue(CuentaDepositoDep))
        End Get
        Set(ByVal value As Integer)
            SetValue(CuentaDepositoDep, value)
        End Set
    End Property

    Private Shared Sub CuentaDepositoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCuentasDepositoCliente", "CuentaDepositoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Shared ReadOnly TraerTodasCuentaDepositoDep As DependencyProperty = DependencyProperty.Register("TraerTodasCuentaDeposito", GetType(Boolean), GetType(CuentasDepositoClienteButton), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf TraerTodasCuentaDepositoChanged)))

    ''' <summary>
    ''' Indica si se realiza la carga de clientes dependiendo de un receptor en especifico
    ''' </summary>
    Public Property TraerTodasCuentaDeposito As Boolean
        Get
            Return CBool(GetValue(TraerTodasCuentaDepositoDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(TraerTodasCuentaDepositoDep, value)
        End Set
    End Property

    Private Shared Sub TraerTodasCuentaDepositoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCuentasDepositoCliente", "TraerTodasCuentaDepositoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _mstrEtiqueta As String = String.Empty
    Public Property Etiqueta As String
        Get
            Return (_mstrEtiqueta)
        End Get
        Set(ByVal value As String)
            _mstrEtiqueta = value
        End Set
    End Property

    Private _mintIzquierda As Integer = 0
    Public Property Izquierda As Integer
        Get
            Return (_mintIzquierda)
        End Get
        Set(ByVal value As Integer)
            _mintIzquierda = value
        End Set
    End Property

    Private _mintSuperior As Integer = 0
    Public Property Superior As Integer
        Get
            Return (_mintSuperior)
        End Get
        Set(ByVal value As Integer)
            _mintSuperior = value
        End Set
    End Property
#End Region

#Region "Inicialización"

    Public Sub New()
        InitializeComponent()
    End Sub

#End Region

#Region "Eventos controles"

    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mobjBuscador = New CuentasDepositoClienteLista(IDComitente, CuentaDeposito, _mstrEtiqueta, TraerTodasCuentaDeposito) ', _mstrEtiqueta, _mintEstado, _mintTipoVinculacion, _mstrAgrupamiento)

            If _mintIzquierda > 0 And _mintSuperior > 0 Then
                mobjBuscador.Top = _mintSuperior
                mobjBuscador.Left = _mintIzquierda
            Else
                'mobjBuscador.CenterOnScreen()
            End If
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscador)
            mobjBuscador.ShowDialog()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al buscar una cuenta depósito", Me.Name, "cmdBuscar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub mobjBuscador_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscador.Closed
        Try
            If IsNothing(mobjBuscador) Then
                RaiseEvent finalizoBusqueda(0, Nothing)
            Else
                RaiseEvent finalizoBusqueda(mobjBuscador.NroCuentaDeposito, mobjBuscador.CuentaDeposito)
            End If
        Catch ex As Exception
            Me._mobjCuentaDeposito = Nothing
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al cerrar el buscador de cuentas depósito", Me.Name, "mobjBuscador_Closed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
