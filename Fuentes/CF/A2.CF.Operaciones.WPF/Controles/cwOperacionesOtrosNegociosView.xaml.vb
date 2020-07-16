Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwOperacionesOtrosNegociosView
        Inherits Window
        Implements INotifyPropertyChanged

#Region "Variables"

    Private mobjVM As OperacionesOtrosNegociosViewModel
    Private mdcProxyUtilidad01 As UtilidadesDomainContext

#End Region

#Region "Propiedades"

    Private _dtmFechaCumplimiento As System.Nullable(Of System.DateTime)
    Public Property dtmFechaCumplimiento() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaCumplimiento
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaCumplimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaCumplimiento"))
        End Set
    End Property

    Private _dblNominal As System.Nullable(Of Double)
    Public Property dblNominal() As System.Nullable(Of Double)
        Get
            Return _dblNominal
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblNominal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblNominal"))
        End Set
    End Property

    Private _FormatoCamposDecimales As String = "2"
    Public Property FormatoCamposDecimales() As String
        Get
            Return _FormatoCamposDecimales
        End Get
        Set(ByVal value As String)
            _FormatoCamposDecimales = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FormatoCamposDecimales"))
        End Set
    End Property

    Private _DiccionarioHabilitarCampos As Dictionary(Of String, Boolean)
    Public Property DiccionarioHabilitarCampos() As Dictionary(Of String, Boolean)
        Get
            Return _DiccionarioHabilitarCampos
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            _DiccionarioHabilitarCampos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DiccionarioHabilitarCampos"))
        End Set
    End Property

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal pmobjVM As OperacionesOtrosNegociosViewModel, ByVal EncabezadoSeleccionadoVM As CFOperaciones.Operaciones_OtrosNegocios, ByVal FormatoCamposDecimalesVM As String, ByVal DiccionarioHabilitarCamposVM As Dictionary(Of String, Boolean))
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
            Else
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            'Me.DataContext = mobjVM
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me

            DiccionarioHabilitarCampos = DiccionarioHabilitarCamposVM
            dtmFechaCumplimiento = EncabezadoSeleccionadoVM.FechaCumplimiento
            dblNominal = EncabezadoSeleccionadoVM.Nominal
            FormatoCamposDecimales = FormatoCamposDecimalesVM

            mobjVM = pmobjVM
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

    End Sub

#End Region

#Region "Métodos para control de eventos"

    Private Async Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try

            mobjVM.dtmFechaCumplimientoAplazada = dtmFechaCumplimiento
            mobjVM.dblCantidadAplazada = dblNominal
            mobjVM.EncabezadoSeleccionado.logOperacionAplazada = True

            Me.DialogResult = True

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            'Me.Close()
            Me.DialogResult = False
            mobjVM.IsBusy = False
            mobjVM.EncabezadoSeleccionado.logOperacionAplazada = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

End Class




