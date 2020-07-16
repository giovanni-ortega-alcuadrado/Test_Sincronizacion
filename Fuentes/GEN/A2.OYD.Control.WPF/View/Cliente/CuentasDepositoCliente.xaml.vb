Imports A2Utilidades.Mensajes
Imports A2.OYD.OYDServer.RIA.Web

Partial Public Class CuentasDepositoCliente
    Inherits UserControl

#Region "Constantes"

    Private Const STR_NOMBRE_VIEW_MODEL As String = "VM"

#End Region

#Region "Variables"

    Public mobjVM As CuentasDepositoClienteViewModel ' Referencia al view model del control

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Evento que se dispara cuando se selecciona un cuenta deposito
    ''' </summary>
    ''' <param name="pintCuentaDeposito">Nro de cuenta deposito</param>
    ''' <param name="pobjCuentaDeposito">Objeto con la información de la cuenta deposito</param>
    ''' <remarks></remarks>
    Public Event cuentaDepositoSeleccionada(ByVal pintCuentaDeposito As Integer, ByVal pobjCuentaDeposito As OYDUtilidades.BuscadorCuentasDeposito)

#End Region

#Region "Propiedades"

    Private Shared CuentaDepositoDep As DependencyProperty = DependencyProperty.Register("CuentaDeposito", GetType(Integer), GetType(CuentasDepositoCliente), New PropertyMetadata(AddressOf cambioPropiedadDep))
    Private Shared IdComitenteDep As DependencyProperty = DependencyProperty.Register("IdComitente", GetType(String), GetType(CuentasDepositoCliente), New PropertyMetadata(AddressOf cambioPropiedadDep))
    Private Shared TraerTodasCuentaDepositoDep As DependencyProperty = DependencyProperty.Register("TraerTodasCuentaDeposito", GetType(Boolean), GetType(CuentasDepositoCliente), New PropertyMetadata(AddressOf cambioPropiedadDep))

    Public Property IdComitente As String
        Get
            Return (Me.GetValue(IdComitenteDep).ToString)
        End Get
        Set(ByVal value As String)
            Me.SetValue(IdComitenteDep, value)
            Me.mobjVM.CodigoComitente = value
        End Set
    End Property

    Public Property CuentaDeposito As Integer
        Get
            Return (CType(Me.GetValue(CuentaDepositoDep), Integer))
        End Get
        Set(ByVal value As Integer)
            Me.SetValue(CuentaDepositoDep, value)
            Me.mobjVM.CuentaDeposito = value
        End Set
    End Property

    Public Property TraerTodasCuentaDeposito As Boolean
        Get
            Return CBool(Me.GetValue(TraerTodasCuentaDepositoDep))
        End Get
        Set(ByVal value As Boolean)
            Me.SetValue(TraerTodasCuentaDepositoDep, value)
            Me.mobjVM.TraerTodosLosDepositos = value
        End Set
    End Property

    ''' <summary>
    ''' Especie seleccionada en el buscador
    ''' </summary>
    Public ReadOnly Property mobjCuentaDeposito() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (mobjVM.CuentaDepositoSeleccionada)
        End Get
    End Property

    ''' <summary>
    ''' Especie seleccionada en el buscador
    ''' </summary>
    Public ReadOnly Property mobjNroCuentaDeposito() As Integer
        Get
            Return (mobjVM.NroCuentaSeleccionada)
        End Get
    End Property

#End Region

#Region "Callback"

    ''' <summary>
    ''' Call back de las dependecy properties
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Shared Sub cambioPropiedadDep(ByVal sender As Object, ByVal args As DependencyPropertyChangedEventArgs)

    End Sub

#End Region

#Region "Inicialización"

    Public Sub New()
        Try
            InitializeComponent()

            mobjVM = CType(Me.LayoutRoot.Resources(STR_NOMBRE_VIEW_MODEL), CuentasDepositoClienteViewModel)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del buscador de cuentas depósito.", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CuentasDepositoCliente_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Not Me.GetValue(CuentaDepositoDep) Is Nothing Then
                Me.mobjVM.CuentaDeposito = CType(Me.GetValue(CuentaDepositoDep), Integer)
            End If

            If Not Me.GetValue(IdComitenteDep) Is Nothing Then
                Me.mobjVM.CodigoComitente = Me.GetValue(IdComitenteDep).ToString
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del buscador de cuentas depósito.", Me.Name, "CuentasDepositoCliente_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos de controles"

    Private Sub cboCuentasDeposito_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cboCuentasDeposito.SelectionChanged
        Dim objSeleccion As OYDUtilidades.BuscadorCuentasDeposito
        Try
            objSeleccion = CType(CType(sender, ComboBox).SelectedItem, OYDUtilidades.BuscadorCuentasDeposito)
            If objSeleccion Is Nothing Then
                RaiseEvent cuentaDepositoSeleccionada(0, Nothing)
            Else
                mobjVM.NroCuentaSeleccionada = CInt(objSeleccion.NroCuentaDeposito)
                RaiseEvent cuentaDepositoSeleccionada(mobjNroCuentaDeposito, objSeleccion)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al asignar la cuenta depósito seleccionada.", Me.Name, "cboCuentasDeposito_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
