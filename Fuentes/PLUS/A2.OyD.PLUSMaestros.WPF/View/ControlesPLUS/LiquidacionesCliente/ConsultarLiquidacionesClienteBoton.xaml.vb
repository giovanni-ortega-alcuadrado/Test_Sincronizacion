Imports Telerik.Windows.Controls
Imports A2.OYD.OYDServer.RIA.Web

Partial Public Class ConsultarLiquidacionesClienteBoton
    Inherits UserControl
    Dim mobjLiquidaciones As ConsultarLiquidacionesClienteView

#Region "Eventos"

    Public Event finalizoBusquedaLiquidacion(ByVal pstrCliente As String, ByVal pobjValores As RetornoValoresLiquidacion)

#End Region

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub btnConsultarLiquidaciones_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            mobjLiquidaciones = New ConsultarLiquidacionesClienteView()
            AddHandler mobjLiquidaciones.Closed, AddressOf mobjLiquidaciones_Closed

            mobjLiquidaciones.HabilitarBuscadorCliente = Me.HabilitarBuscadorCliente
            mobjLiquidaciones.HabilitarTipoOperacion = Me.HabilitarTipoOperacion
            mobjLiquidaciones.ClienteABuscar = Me.ClienteABuscar

            Select Case Me.TipoOperacion
                Case TiposOperacionLiquidacion.COMPRA
                    mobjLiquidaciones.TipoOperacionABuscar = "C"
                Case TiposOperacionLiquidacion.VENTA
                    mobjLiquidaciones.TipoOperacionABuscar = "V"
            End Select

            mobjLiquidaciones.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar las liquidaciones del cliente.", Me.ToString(), "btnConsultarLiquidaciones_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub mobjLiquidaciones_Closed(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim strCliente As String = mobjLiquidaciones.ClienteSeleccionado
            Dim dblValor As Double = mobjLiquidaciones.ValorTotalLiquidaciones
            Dim dblValorSeleccionado As Double = mobjLiquidaciones.ValorTotalLiquidacionesSeleccionadas
            Dim strLiquidacionesSeleccionadas As String = mobjLiquidaciones.LiquidacionesSeleccionadas
            Dim objListaLiquidacionesSeleccionadas As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente) = mobjLiquidaciones.ListaLiquidacionesSeleccionadas

            Dim objValores As New RetornoValoresLiquidacion

            objValores.strCliente = strCliente
            objValores.dblValorTotalLiquidaciones = dblValor
            objValores.dblValorLiquidacionesSeleccionadas = dblValorSeleccionado
            objValores.strLiquidaciones = strLiquidacionesSeleccionadas
            objValores.listLiquidacionesSeleccionadas = objListaLiquidacionesSeleccionadas

            RaiseEvent finalizoBusquedaLiquidacion(strCliente, objValores)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al cerrar el control de consulta de liquidaciones", Me.Name, "mobjLiquidaciones_Closed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#Region "Propiedades dependientes"

    Public Shared ReadOnly ClienteABuscarProperty As DependencyProperty = DependencyProperty.Register("ClienteABuscar", _
                                                                                               GetType(String), _
                                                                                               GetType(ConsultarLiquidacionesClienteBoton), New PropertyMetadata(""))
    Public Property ClienteABuscar() As String
        Get
            Return CStr(GetValue(ClienteABuscarProperty))
        End Get
        Set(ByVal value As String)
            SetValue(ClienteABuscarProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TipoOperacionProperty As DependencyProperty = DependencyProperty.Register("TipoOperacion", _
                                                                                       GetType(TiposOperacionLiquidacion), _
                                                                                       GetType(ConsultarLiquidacionesClienteBoton), New PropertyMetadata(TiposOperacionLiquidacion.COMPRA))
    Public Property TipoOperacion() As TiposOperacionLiquidacion
        Get
            Return CType(GetValue(TipoOperacionProperty), TiposOperacionLiquidacion)
        End Get
        Set(ByVal value As TiposOperacionLiquidacion)
            SetValue(TipoOperacionProperty, value)
        End Set
    End Property

#End Region

#Region "Propiedades"

    Private _mlogHabilitarBuscadorCliente As Boolean = True
    ''' <summary>
    ''' Indica si se habilite el buscador de clientes.
    ''' </summary>
    Public Property HabilitarBuscadorCliente As Boolean
        Get
            Return (_mlogHabilitarBuscadorCliente)
        End Get
        Set(ByVal value As Boolean)
            _mlogHabilitarBuscadorCliente = value
        End Set
    End Property

    Private _mlogHabilitarTipoOperacion As Boolean = True
    ''' <summary>
    ''' Indica si se habilite el combo del tipo de operación.
    ''' </summary>
    Public Property HabilitarTipoOperacion As Boolean
        Get
            Return (_mlogHabilitarTipoOperacion)
        End Get
        Set(ByVal value As Boolean)
            _mlogHabilitarTipoOperacion = value
        End Set
    End Property

#End Region

End Class


Public Enum TiposOperacionLiquidacion
    COMPRA = 1
    VENTA = 2
End Enum

Public Class RetornoValoresLiquidacion

    Public Property strCliente As String
    Public Property strLiquidaciones As String
    Public Property listLiquidacionesSeleccionadas As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente)
    Public Property dblValorTotalLiquidaciones As Double
    Public Property dblValorLiquidacionesSeleccionadas As Double

End Class
