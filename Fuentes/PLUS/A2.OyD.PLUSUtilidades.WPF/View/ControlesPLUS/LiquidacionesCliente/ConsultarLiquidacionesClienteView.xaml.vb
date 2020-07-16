Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls


Partial Public Class ConsultarLiquidacionesClienteView
    Inherits Window
    Dim vmLiquidacionesCliente As ConsultarLiquidacionesClienteViewModel


    Public Sub New()
        Try
            InitializeComponent()

            vmLiquidacionesCliente = CType(Me.Resources("vmLiquidacionesCliente"), ConsultarLiquidacionesClienteViewModel)
            Me.DataContext = vmLiquidacionesCliente
            vmLiquidacionesCliente.ViewLiquidacionesCliente = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización de la consulta de las liquidaciones.", Me.ToString(), "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

#Region "Propiedades dependientes"

    Public Shared ReadOnly ClienteABuscarProperty As DependencyProperty = DependencyProperty.Register("ClienteABuscar", _
                                                                                               GetType(String), _
                                                                                               GetType(ConsultarLiquidacionesClienteView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf ClienteABuscarChanged)))
    Public Property ClienteABuscar() As String
        Get
            Return CStr(GetValue(ClienteABuscarProperty))
        End Get
        Set(ByVal value As String)
            SetValue(ClienteABuscarProperty, value)
        End Set
    End Property

    Private Shared Sub ClienteABuscarChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ConsultarLiquidacionesClienteView = DirectCast(d, ConsultarLiquidacionesClienteView)

        If Not IsNothing(obj.vmLiquidacionesCliente) Then
            obj.vmLiquidacionesCliente.Cliente = obj.ClienteABuscar
        End If
    End Sub

    Public Shared ReadOnly TipoOperacionABuscarProperty As DependencyProperty = DependencyProperty.Register("TipoOperacionABuscar", _
                                                                                               GetType(String), _
                                                                                               GetType(ConsultarLiquidacionesClienteView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoOperacionABuscarChanged)))
    Public Property TipoOperacionABuscar() As String
        Get
            Return CStr(GetValue(TipoOperacionABuscarProperty))
        End Get
        Set(ByVal value As String)
            SetValue(TipoOperacionABuscarProperty, value)
        End Set
    End Property

    Private Shared Sub TipoOperacionABuscarChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ConsultarLiquidacionesClienteView = DirectCast(d, ConsultarLiquidacionesClienteView)

        If Not IsNothing(obj.vmLiquidacionesCliente) Then
            obj.vmLiquidacionesCliente.TipoOperacion = obj.TipoOperacionABuscar
        End If
    End Sub

    Public Shared ReadOnly ValorTotalLiquidacionesProperty As DependencyProperty = DependencyProperty.Register("ValorTotalLiquidaciones", _
                                                                                                        GetType(Double), _
                                                                                                        GetType(ConsultarLiquidacionesClienteView), New PropertyMetadata(CDbl(0)))
    Public Property ValorTotalLiquidaciones() As Double
        Get
            Return CDbl(GetValue(ValorTotalLiquidacionesProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(ValorTotalLiquidacionesProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ValorTotalLiquidacionesSeleccionadasProperty As DependencyProperty = DependencyProperty.Register("ValorTotalLiquidacionesSeleccionadas", _
                                                                                                                     GetType(Double), _
                                                                                                                     GetType(ConsultarLiquidacionesClienteView), New PropertyMetadata(CDbl(0)))
    Public Property ValorTotalLiquidacionesSeleccionadas() As Double
        Get
            Return CDbl(GetValue(ValorTotalLiquidacionesSeleccionadasProperty))
        End Get
        Set(ByVal value As Double)
            SetValue(ValorTotalLiquidacionesSeleccionadasProperty, value)
        End Set
    End Property

    Public Shared ReadOnly LiquidacionesSeleccionadasProperty As DependencyProperty = DependencyProperty.Register("LiquidacionesSeleccionadas", _
                                                                                                           GetType(String), _
                                                                                                           GetType(ConsultarLiquidacionesClienteView), New PropertyMetadata(""))
    Public Property LiquidacionesSeleccionadas() As String
        Get
            Return CStr(GetValue(LiquidacionesSeleccionadasProperty))
        End Get
        Set(ByVal value As String)
            SetValue(LiquidacionesSeleccionadasProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ListaLiquidacionesSeleccionadasProperty As DependencyProperty = DependencyProperty.Register("ListaLiquidacionesSeleccionadas", _
                                                                                                           GetType(List(Of OYDPLUSUtilidades.tblLiquidacionesCliente)), _
                                                                                                           GetType(ConsultarLiquidacionesClienteView), New PropertyMetadata(Nothing))
    Public Property ListaLiquidacionesSeleccionadas() As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente)
        Get
            Return CType(GetValue(ListaLiquidacionesSeleccionadasProperty), List(Of OYDPLUSUtilidades.tblLiquidacionesCliente))
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblLiquidacionesCliente))
            SetValue(ListaLiquidacionesSeleccionadasProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ClienteSeleccionadoProperty As DependencyProperty = DependencyProperty.Register("ClienteSeleccionado", _
                                                                                            GetType(String), _
                                                                                            GetType(ConsultarLiquidacionesClienteView), New PropertyMetadata(""))
    Public Property ClienteSeleccionado() As String
        Get
            Return CStr(GetValue(ClienteSeleccionadoProperty))
        End Get
        Set(ByVal value As String)
            SetValue(ClienteSeleccionadoProperty, value)
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
            Me.vmLiquidacionesCliente.HabilitarBuscadorCliente = value
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
            Me.vmLiquidacionesCliente.HabilitarTipoOperacion = value
        End Set
    End Property

#End Region

    Private Sub btnConsultar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(vmLiquidacionesCliente) Then
            vmLiquidacionesCliente.ConsultarLiquidacionesCliente()
        End If
    End Sub

    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(vmLiquidacionesCliente) Then
                Me.vmLiquidacionesCliente.Cliente = pstrIdComitente
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.Close()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cerrar la consulta de las liquidaciones.", Me.Name, "btnCerrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ConsultarLiquidacionesClienteView_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Try
            Me.vmLiquidacionesCliente.CalcularValorLiquidaciones()
            'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cerrar la consulta de las liquidaciones.", Me.Name, "ConsultarLiquidacionesClienteView_Closed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
