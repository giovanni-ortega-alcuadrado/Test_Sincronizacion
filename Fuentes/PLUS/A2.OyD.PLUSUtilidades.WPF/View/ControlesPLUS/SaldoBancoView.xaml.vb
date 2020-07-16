Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports System.Globalization

Partial Public Class SaldoBancoView
    Inherits UserControl

#Region "Variables"

    Private WithEvents mvSaldoBanco As SaldoBancoViewModel
    Private Const STR_NOMBRE_VIEW_MODEL As String = "VMSaldoBanco"

#End Region

#Region "Inicializadores"

    Public Sub New()
        Try

            Me.DataContext = New SaldoBancoViewModel
            InitializeComponent()
            mvSaldoBanco = CType(Me.DataContext, SaldoBancoViewModel)
            mvSaldoBanco.viewSaldoBanco = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar", _
                        Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedad Dependencias"

    Public Shared ReadOnly BancoOYDSeleccionadoProperty As DependencyProperty = DependencyProperty.Register("BancoOYDSeleccionado", _
                                                                                                        GetType(Integer), _
                                                                                                        GetType(SaldoBancoView), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf BancoOYDSeleccionadoChanged)))
    Public Property BancoOYDSeleccionado() As Integer
        Get
            Return CInt((GetValue(BancoOYDSeleccionadoProperty)))
        End Get
        Set(ByVal value As Integer)
            SetValue(BancoOYDSeleccionadoProperty, value)
        End Set
    End Property

    Private Shared Sub BancoOYDSeleccionadoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As SaldoBancoView = DirectCast(d, SaldoBancoView)

        If Not IsNothing(obj.mvSaldoBanco) Then
            obj.mvSaldoBanco.IdBanco = obj.BancoOYDSeleccionado
        End If
    End Sub

    Public Shared ReadOnly ConsultarSaldoBancoProperty As DependencyProperty = DependencyProperty.Register("ConsultarSaldoBanco", _
                                                                                           GetType(Boolean), _
                                                                                           GetType(SaldoBancoView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf ConsultarSaldoBancoChanged)))
    Public Property ConsultarSaldoBanco() As Boolean
        Get
            Return CBool(GetValue(ConsultarSaldoBancoProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConsultarSaldoBancoProperty, value)
        End Set
    End Property

    Private Shared Sub ConsultarSaldoBancoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As SaldoBancoView = DirectCast(d, SaldoBancoView)

        
        If Not IsNothing(obj.mvSaldoBanco) Then
            obj.mvSaldoBanco.ConsultarSaldo = obj.ConsultarSaldoBanco
        Else
            obj.mvSaldoBanco.LimpiarSaldoBanco()
        End If
    End Sub
    Public Shared ReadOnly HabilitarSaldoBancoProperty As DependencyProperty = DependencyProperty.Register("HabilitarSaldoBanco", _
                                                                                         GetType(Boolean), _
                                                                                         GetType(SaldoBancoView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf HabilitarSaldoBancoChanged)))
    Public Property HabilitarSaldoBanco() As Boolean
        Get
            Return CBool(GetValue(HabilitarSaldoBancoProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(HabilitarSaldoBancoProperty, value)
        End Set
    End Property

    Private Shared Sub HabilitarSaldoBancoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As SaldoBancoView = DirectCast(d, SaldoBancoView)

        If Not IsNothing(obj.mvSaldoBanco) Then
            obj.mvSaldoBanco.HabilitarSaldoBanco = obj.HabilitarSaldoBanco
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
            Else
            End If
        End Set
    End Property

#End Region

    Private Sub Expander_Collapsed(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mvSaldoBanco) Then
                mvSaldoBanco.VerEncabezadoSaldoBanco = Visibility.Visible
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Expandir", _
                         Me.ToString(), "Expander_Expanded", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Expander_Expanded(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mvSaldoBanco) Then
                mvSaldoBanco.VerEncabezadoSaldoBanco = Visibility.Collapsed
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Expandir", _
                         Me.ToString(), "Expander_Expanded", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub

    Private Sub btnRefrescar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mvSaldoBanco) Then
                mvSaldoBanco.ConsultarSaldoBanco()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar la consulta", _
                                 Me.ToString(), "btnRefrescar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
End Class
