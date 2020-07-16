Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports System.Globalization
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class OperacionesXCumplirView
    Inherits UserControl

#Region "Variables"

    Private WithEvents mvOperaciones As OperacionesXCumplirViewModel
    Private Const STR_NOMBRE_VIEW_MODEL As String = "VMOperaciones"

#End Region

#Region "Inicializadores"

    Public Sub New()
        Try
            InitializeComponent()
            mvOperaciones = CType(Me.Resources(STR_NOMBRE_VIEW_MODEL), OperacionesXCumplirViewModel)
            Me.LayoutRoot.DataContext = mvOperaciones
            mvOperaciones.ViewOperacionXCumplir = Me
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.dgOperaciones.Width = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de las operaciones x cumplir.", _
                                 Me.ToString(), "OperacionesXCumplirView", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.dgOperaciones.Width = Application.Current.MainWindow.ActualWidth * 0.95
    End Sub

#End Region

#Region "Eventos"

#End Region

#Region "Propiedad Dependencias"

    Public Shared ReadOnly OperacionXCumplirSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("OperacionXCumplirSeleccionada", _
                                                                                                             GetType(OYDPLUSUtilidades.tblOperacionesCumplir), _
                                                                                                             GetType(OperacionesXCumplirView), New PropertyMetadata(Nothing))
    Public Property OperacionXCumplirSeleccionada() As OYDPLUSUtilidades.tblOperacionesCumplir
        Get
            Return CType(GetValue(OperacionXCumplirSeleccionadaProperty), OYDPLUSUtilidades.tblOperacionesCumplir)
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOperacionesCumplir)
            SetValue(OperacionXCumplirSeleccionadaProperty, value)
        End Set
    End Property

    Public Shared ReadOnly OperacionSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("OperacionSeleccionada", _
                                                                                                GetType(Boolean), _
                                                                                                GetType(OperacionesXCumplirView), New PropertyMetadata(Nothing))
    Public Property OperacionSeleccionada() As Boolean
        Get
            Return CBool(GetValue(OperacionSeleccionadaProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(OperacionSeleccionadaProperty, value)
        End Set
    End Property

    Public Shared ReadOnly TipoNegocioProperty As DependencyProperty = DependencyProperty.Register("TipoNegocio", _
                                                                                           GetType(String), _
                                                                                           GetType(OperacionesXCumplirView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoNegocioChanged)))
    Public Property TipoNegocio() As String
        Get
            Return CStr(GetValue(TipoNegocioProperty))
        End Get
        Set(ByVal value As String)
            SetValue(TipoNegocioProperty, value)
        End Set
    End Property

    Private Shared Sub TipoNegocioChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As OperacionesXCumplirView = DirectCast(d, OperacionesXCumplirView)

        If Not IsNothing(obj.mvOperaciones) Then
            obj.mvOperaciones.TipoNegocio = obj.TipoNegocio
        End If
    End Sub

    Public Shared ReadOnly TipoOperacionProperty As DependencyProperty = DependencyProperty.Register("TipoOperacion", _
                                                                                           GetType(String), _
                                                                                           GetType(OperacionesXCumplirView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoOperacionChanged)))
    Public Property TipoOperacion() As String
        Get
            Return CStr(GetValue(TipoOperacionProperty))
        End Get
        Set(ByVal value As String)
            SetValue(TipoOperacionProperty, value)
        End Set
    End Property

    Private Shared Sub TipoOperacionChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As OperacionesXCumplirView = DirectCast(d, OperacionesXCumplirView)

        If Not IsNothing(obj.mvOperaciones) Then
            obj.mvOperaciones.TipoOperacion = obj.TipoOperacion
        End If
    End Sub

    Public Shared ReadOnly CodigoOYDSeleccionadoProperty As DependencyProperty = DependencyProperty.Register("CodigoOYDSeleccionado", _
                                                                                                        GetType(String), _
                                                                                                        GetType(OperacionesXCumplirView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf CodigoOYDSeleccionadoChanged)))
    Public Property CodigoOYDSeleccionado() As String
        Get
            Return CStr(GetValue(CodigoOYDSeleccionadoProperty))
        End Get
        Set(ByVal value As String)
            SetValue(CodigoOYDSeleccionadoProperty, value)
        End Set
    End Property

    Private Shared Sub CodigoOYDSeleccionadoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As OperacionesXCumplirView = DirectCast(d, OperacionesXCumplirView)

        If Not IsNothing(obj.mvOperaciones) Then
            obj.mvOperaciones.CodigoOYD = obj.CodigoOYDSeleccionado
        End If
    End Sub

    Public Shared ReadOnly EspecieSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("EspecieSeleccionada", _
                                                                                                    GetType(String), _
                                                                                                    GetType(OperacionesXCumplirView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf EspecieSeleccionadaChanged)))
    Public Property EspecieSeleccionada() As String
        Get
            Return CStr(GetValue(EspecieSeleccionadaProperty))
        End Get
        Set(ByVal value As String)
            SetValue(EspecieSeleccionadaProperty, value)
        End Set
    End Property

    Private Shared Sub EspecieSeleccionadaChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As OperacionesXCumplirView = DirectCast(d, OperacionesXCumplirView)

        If Not IsNothing(obj.mvOperaciones) Then
            obj.mvOperaciones.Especie = obj.EspecieSeleccionada
        End If
    End Sub

    Public Shared ReadOnly ConsultarOperacionesProperty As DependencyProperty = DependencyProperty.Register("ConsultarOperaciones", _
                                                                                           GetType(Boolean), _
                                                                                           GetType(OperacionesXCumplirView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf ConsultarOperacionesChanged)))
    Public Property ConsultarOperaciones() As Boolean
        Get
            Return CBool(GetValue(ConsultarOperacionesProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConsultarOperacionesProperty, value)
        End Set
    End Property

    Private Shared Sub ConsultarOperacionesChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As OperacionesXCumplirView = DirectCast(d, OperacionesXCumplirView)

        If Not IsNothing(obj.mvOperaciones) Then
            obj.mvOperaciones.ConsultarOperaciones = obj.ConsultarOperaciones
        End If
    End Sub

#End Region

    Private Sub btnRefrescar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mvOperaciones) Then
                mvOperaciones.ConsultarOperacionesXCumplir()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar la consulta", _
                                 Me.ToString(), "btnRefrescar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

End Class
