Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports System.Globalization

Partial Public Class SaldoClienteView
    Inherits UserControl

#Region "Variables"

    Private WithEvents mvSaldo As SaldoClienteViewModel
    Private Const STR_NOMBRE_VIEW_MODEL As String = "VMSaldo"

#End Region

#Region "Inicializadores"

    Public Sub New()
        Try
            InitializeComponent()
            mvSaldo = CType(Me.Resources(STR_NOMBRE_VIEW_MODEL), SaldoClienteViewModel)
            Me.LayoutRoot.DataContext = mvSaldo
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consultar del saldo del cliente.", _
                                Me.ToString(), "SaldoClienteView", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        
    End Sub

#End Region

#Region "Eventos"

#End Region

#Region "Propiedad Dependencias"

    Public Shared ReadOnly CodigoOYDSeleccionadoProperty As DependencyProperty = DependencyProperty.Register("CodigoOYDSeleccionado", _
                                                                                                        GetType(String), _
                                                                                                        GetType(SaldoClienteView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf CodigoOYDSeleccionadoChanged)))
    Public Property CodigoOYDSeleccionado() As String
        Get
            Return CStr(GetValue(CodigoOYDSeleccionadoProperty))
        End Get
        Set(ByVal value As String)
            SetValue(CodigoOYDSeleccionadoProperty, value)
        End Set
    End Property

    Private Shared Sub CodigoOYDSeleccionadoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As SaldoClienteView = DirectCast(d, SaldoClienteView)

        If Not IsNothing(obj.mvSaldo) Then
            obj.mvSaldo.CodigoOYD = obj.CodigoOYDSeleccionado
        End If
    End Sub

    Public Shared ReadOnly TipoProductoSeleccionadoProperty As DependencyProperty = DependencyProperty.Register("TipoProductoSeleccionado", _
                                                                                                        GetType(String), _
                                                                                                        GetType(SaldoClienteView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoProductoSeleccionadoChanged)))
    Public Property TipoProductoSeleccionado() As String
        Get
            Return CStr(GetValue(TipoProductoSeleccionadoProperty))
        End Get
        Set(ByVal value As String)
            SetValue(TipoProductoSeleccionadoProperty, value)
        End Set
    End Property

    Private Shared Sub TipoProductoSeleccionadoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As SaldoClienteView = DirectCast(d, SaldoClienteView)

        If Not IsNothing(obj.mvSaldo) Then
            obj.mvSaldo.TipoProducto = obj.TipoProductoSeleccionado
        End If
    End Sub

    Public Shared ReadOnly CarteraColectivaFondosSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("CarteraColectivaFondosSeleccionada", _
                                                                                                                          GetType(String), _
                                                                                                                          GetType(SaldoClienteView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf CarteraColectivaFondosSeleccionadaChanged)))
    Public Property CarteraColectivaFondosSeleccionada() As String
        Get
            Return CStr(GetValue(CarteraColectivaFondosSeleccionadaProperty))
        End Get
        Set(ByVal value As String)
            SetValue(CarteraColectivaFondosSeleccionadaProperty, value)
        End Set
    End Property

    Private Shared Sub CarteraColectivaFondosSeleccionadaChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As SaldoClienteView = DirectCast(d, SaldoClienteView)

        If Not IsNothing(obj.mvSaldo) Then
            obj.mvSaldo.CarteraColectivaFondos = obj.CarteraColectivaFondosSeleccionada
        End If
    End Sub

    Public Shared ReadOnly NroEncargoFondosSeleccionadoProperty As DependencyProperty = DependencyProperty.Register("NroEncargoFondosSeleccionado", _
                                                                                                                    GetType(String), _
                                                                                                                    GetType(SaldoClienteView), New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf NroEncargoFondosSeleccionadoChanged)))
    Public Property NroEncargoFondosSeleccionado() As String
        Get
            Return CType(GetValue(NroEncargoFondosSeleccionadoProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(NroEncargoFondosSeleccionadoProperty, value)
        End Set
    End Property

    Private Shared Sub NroEncargoFondosSeleccionadoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As SaldoClienteView = DirectCast(d, SaldoClienteView)

        If Not IsNothing(obj.mvSaldo) Then
            obj.mvSaldo.NroEncargoFondos = obj.NroEncargoFondosSeleccionado
        End If
    End Sub

    Public Shared ReadOnly ConsultarSaldoProperty As DependencyProperty = DependencyProperty.Register("ConsultarSaldo", _
                                                                                           GetType(Boolean), _
                                                                                           GetType(SaldoClienteView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf ConsultarSaldoChanged)))
    Public Property ConsultarSaldo() As Boolean
        Get
            Return CBool(GetValue(ConsultarSaldoProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConsultarSaldoProperty, value)
        End Set
    End Property

    Private Shared Sub ConsultarSaldoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As SaldoClienteView = DirectCast(d, SaldoClienteView)

        If Not IsNothing(obj.mvSaldo) Then
            obj.mvSaldo.ConsultarSaldo = obj.ConsultarSaldo
        End If
    End Sub

    Private _mlogMostrarExpanderAbierto As Boolean = False
    ''' <summary>
    ''' Indica si al iniciar el control lanza una consulta de comitentes. Solamente aplica si IdComitente no se envía
    ''' </summary>
    Public Property MostrarExpanderAbierto As Boolean
        Get
            Return (_mlogMostrarExpanderAbierto)
        End Get
        Set(ByVal value As Boolean)
            _mlogMostrarExpanderAbierto = value
            If _mlogMostrarExpanderAbierto Then
                ControlExpander.IsExpanded = True
            Else
                ControlExpander.IsExpanded = False
            End If
        End Set
    End Property

#End Region

    Private Sub btnRefrescar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mvSaldo) Then
                mvSaldo.ConsultarSaldoCliente()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar la consulta", _
                                 Me.ToString(), "btnRefrescar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Expander_Collapsed(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mvSaldo) Then
                mvSaldo.VerEncabezadoSaldoCliente = Visibility.Visible
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Expandir", _
                         Me.ToString(), "Expander_Expanded", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Expander_Expanded(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mvSaldo) Then
                mvSaldo.VerEncabezadoSaldoCliente = Visibility.Collapsed
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Expandir", _
                         Me.ToString(), "Expander_Expanded", Application.Current.ToString(), Program.Maquina, ex)
        End Try

     
    End Sub
End Class
