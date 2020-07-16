Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports System.Globalization
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class PortafolioClienteView
    Inherits UserControl

#Region "Variables"

    Private WithEvents mvPortafolio As PortafolioClienteViewModel
    Private Const STR_NOMBRE_VIEW_MODEL As String = "VMPortafolio"

#End Region

#Region "Inicializadores"

    Public Sub New()
        Try
            InitializeComponent()
            mvPortafolio = CType(Me.Resources(STR_NOMBRE_VIEW_MODEL), PortafolioClienteViewModel)
            Me.LayoutRoot.DataContext = mvPortafolio
            mvPortafolio.ViewPortafolio = Me

            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.dgPortafolioAcciones.Width = Application.Current.MainWindow.ActualWidth * 0.95
            Me.dgPortafolioRentaFija.Width = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consultar el portafolio del cliente.", _
                                 Me.ToString(), "PortafolioClienteView", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.dgPortafolioAcciones.Width = Application.Current.MainWindow.ActualWidth * 0.95
        Me.dgPortafolioRentaFija.Width = Application.Current.MainWindow.ActualWidth * 0.95
    End Sub

#End Region

#Region "Propiedad Dependencias"

    Public Shared ReadOnly PortafolioClienteSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("PortafolioCliente", _
                                                                                                       GetType(OYDPLUSUtilidades.tblPortafolioCliente), _
                                                                                                       GetType(PortafolioClienteView), New PropertyMetadata(Nothing))
    Public Property PortafolioCliente() As OYDPLUSUtilidades.tblPortafolioCliente
        Get
            Return CType(GetValue(PortafolioClienteSeleccionadaProperty), OYDPLUSUtilidades.tblPortafolioCliente)
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblPortafolioCliente)
            SetValue(PortafolioClienteSeleccionadaProperty, value)
        End Set
    End Property

    Public Shared ReadOnly PortafolioSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("PortafolioSeleccionada", _
                                                                                                GetType(Boolean), _
                                                                                                GetType(PortafolioClienteView), New PropertyMetadata(Nothing))
    Public Property PortafolioSeleccionada() As Boolean
        Get
            Return CBool(GetValue(PortafolioSeleccionadaProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(PortafolioSeleccionadaProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TipoNegocioProperty As DependencyProperty = DependencyProperty.Register("TipoNegocio", _
                                                                                           GetType(String), _
                                                                                           GetType(PortafolioClienteView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoNegocioChanged)))
    Public Property TipoNegocio() As String
        Get
            Return CStr(GetValue(TipoNegocioProperty))
        End Get
        Set(ByVal value As String)
            SetValue(TipoNegocioProperty, value)
        End Set
    End Property

    Private Shared Sub TipoNegocioChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As PortafolioClienteView = DirectCast(d, PortafolioClienteView)

        If Not IsNothing(obj.mvPortafolio) Then
            obj.mvPortafolio.TipoNegocio = obj.TipoNegocio
        End If
    End Sub

    Public Shared ReadOnly CodigoOYDSeleccionadoProperty As DependencyProperty = DependencyProperty.Register("CodigoOYDSeleccionado", _
                                                                                                        GetType(String), _
                                                                                                        GetType(PortafolioClienteView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf CodigoOYDSeleccionadoChanged)))
    Public Property CodigoOYDSeleccionado() As String
        Get
            Return CStr(GetValue(CodigoOYDSeleccionadoProperty))
        End Get
        Set(ByVal value As String)
            SetValue(CodigoOYDSeleccionadoProperty, value)
        End Set
    End Property

    Private Shared Sub CodigoOYDSeleccionadoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As PortafolioClienteView = DirectCast(d, PortafolioClienteView)

        If Not IsNothing(obj.mvPortafolio) Then
            obj.mvPortafolio.CodigoOYD = obj.CodigoOYDSeleccionado
        End If
    End Sub

    Public Shared ReadOnly EspecieSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("EspecieSeleccionada", _
                                                                                                    GetType(String), _
                                                                                                    GetType(PortafolioClienteView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf EspecieSeleccionadaChanged)))
    Public Property EspecieSeleccionada() As String
        Get
            Return CStr(GetValue(EspecieSeleccionadaProperty))
        End Get
        Set(ByVal value As String)
            SetValue(EspecieSeleccionadaProperty, value)
        End Set
    End Property

    Private Shared Sub EspecieSeleccionadaChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As PortafolioClienteView = DirectCast(d, PortafolioClienteView)

        If Not IsNothing(obj.mvPortafolio) Then
            obj.mvPortafolio.Especie = obj.EspecieSeleccionada
        End If
    End Sub

    Public Shared ReadOnly ConsultarPortafolioProperty As DependencyProperty = DependencyProperty.Register("ConsultarPortafolio", _
                                                                                           GetType(Boolean), _
                                                                                           GetType(PortafolioClienteView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf ConsultarPortafolioChanged)))
    Public Property ConsultarPortafolio() As Boolean
        Get
            Return CBool(GetValue(ConsultarPortafolioProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConsultarPortafolioProperty, value)
        End Set
    End Property

    Private Shared Sub ConsultarPortafolioChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As PortafolioClienteView = DirectCast(d, PortafolioClienteView)

        If Not IsNothing(obj.mvPortafolio) Then
            obj.mvPortafolio.ConsultarPortafolio = obj.ConsultarPortafolio
        End If
    End Sub

#End Region

    Private Sub btnRefrescar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mvPortafolio) Then
                mvPortafolio.ConsultarPortafolioCliente()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar la consulta", _
                                 Me.ToString(), "btnRefrescar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

End Class
