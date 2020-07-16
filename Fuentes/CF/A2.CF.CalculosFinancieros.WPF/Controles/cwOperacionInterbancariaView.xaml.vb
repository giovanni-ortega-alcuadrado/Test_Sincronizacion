Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwOperacionInterbancariaView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Variables"

    Private mobjVM As OperacionInterbancariaViewModel

#End Region

#Region "Propiedades"

    Private _dtmFechaPago As System.Nullable(Of System.DateTime)
    Public Property dtmFechaPago() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaPago
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaPago = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaPago"))
        End Set
    End Property

    Private _dblTasaPago As System.Nullable(Of Double)
    Public Property dblTasaPago() As System.Nullable(Of Double)
        Get
            Return _dblTasaPago
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblTasaPago = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblTasaPago"))
        End Set
    End Property

    Private _intDias As System.Nullable(Of Integer)
    Public Property intDias() As System.Nullable(Of Integer)
        Get
            Return _intDias
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intDias = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intDias"))
        End Set
    End Property

    Private _dblValorPago As System.Nullable(Of Double)
    Public Property dblValorPago() As System.Nullable(Of Double)
        Get
            Return _dblValorPago
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblValorPago = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorPago"))
        End Set
    End Property

    Private _dblValorPagoAdicional As System.Nullable(Of Double)
    Public Property dblValorPagoAdicional() As System.Nullable(Of Double)
        Get
            Return _dblValorPagoAdicional
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblValorPagoAdicional = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorPagoAdicional"))
        End Set
    End Property

    Private _dtmCalculo As System.Nullable(Of System.DateTime)
    Public Property dtmCalculo() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmCalculo
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmCalculo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmCalculo"))
        End Set
    End Property

    Private _intDiasEntreFlujos As System.Nullable(Of Integer)
    Public Property intDiasEntreFlujos() As System.Nullable(Of Integer)
        Get
            Return _intDiasEntreFlujos
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intDiasEntreFlujos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intDiasEntreFlujos"))
        End Set
    End Property

    Private _logPagado As Boolean
    Public Property logPagado() As Boolean
        Get
            Return _logPagado
        End Get
        Set(ByVal value As Boolean)
            _logPagado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logPagado"))
        End Set
    End Property

    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarEncabezado"))
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As OperacionInterbancariaDetalle
    Public Property DetalleSeleccionado() As OperacionInterbancariaDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As OperacionInterbancariaDetalle)
            _DetalleSeleccionado = value
        End Set
    End Property

    Private _IsBusy As Boolean = False
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal pmobjVM As OperacionInterbancariaViewModel, ByVal DetalleSeleccionadoVM As OperacionInterbancariaDetalle, ByVal HabilitarEncabezadoVM As Boolean)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        'Me.DataContext = mobjVM
        InitializeComponent()
        Me.LayoutRoot.DataContext = Me

        DetalleSeleccionado = DetalleSeleccionadoVM

        dtmFechaPago = DetalleSeleccionadoVM.dtmFechaPago
        dblTasaPago = DetalleSeleccionadoVM.dblTasaPago
        intDias = DetalleSeleccionadoVM.intDias
        dblValorPago = DetalleSeleccionadoVM.dblValorPago
        dblValorPagoAdicional = DetalleSeleccionadoVM.dblValorPagoAdicional
        dtmCalculo = DetalleSeleccionadoVM.dtmCalculo
        intDiasEntreFlujos = DetalleSeleccionadoVM.intDiasEntreFlujos
        logPagado = DetalleSeleccionadoVM.logPagado

        HabilitarEncabezado = HabilitarEncabezadoVM

        mobjVM = pmobjVM

    End Sub

#End Region

#Region "Métodos para control de eventos"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            DetalleSeleccionado.dblValorPagoAdicional = dblValorPagoAdicional
            DetalleSeleccionado.logPagado = logPagado

            mobjVM.DetalleSeleccionado = DetalleSeleccionado

            Me.DialogResult = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            'Me.Close()
            Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged


End Class
