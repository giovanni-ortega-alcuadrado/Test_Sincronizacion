Imports Telerik.Windows.Controls
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Partial Public Class ObtenerOrdenSAEView
    Inherits UserControl
    Dim vmObtenerOrdenSAE As ObtenerOrdenSAEViewModel


    Public Sub New()
        Try
            InitializeComponent()
            vmObtenerOrdenSAE = CType(Me.Resources("vmObtenerOrdenSAE"), ObtenerOrdenSAEViewModel)
            Me.LayoutRoot.DataContext = vmObtenerOrdenSAE
            vmObtenerOrdenSAE.ViewOrdenSAE = Me
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.dgAcciones.Width = Application.Current.MainWindow.ActualWidth * 0.95
            Me.dgRentaFija.Width = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error cuando se iniciaba el grid de las ordenes de SAE", Me.Name, "ObtenerOrdenSAEView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.dgAcciones.Width = Application.Current.MainWindow.ActualWidth * 0.95
        Me.dgRentaFija.Width = Application.Current.MainWindow.ActualWidth * 0.95
    End Sub

#Region "Propiedad Dependencias"
    'Propiedad para validacion de Carga Masiva por Precio Promedio : JDOL20170309
    Public Shared ReadOnly ComplementacionPrecioPromedioProperty As DependencyProperty = DependencyProperty.Register("ComplementacionPrecioPromedio",
                                                                                               GetType(Boolean),
                                                                                               GetType(ObtenerOrdenSAEView), New PropertyMetadata(New PropertyChangedCallback(AddressOf ComplementacionPrecioPromedioChanged)))
    Public Property ComplementacionPrecioPromedio() As Boolean
        Get
            Return CStr(GetValue(ComplementacionPrecioPromedioProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ComplementacionPrecioPromedioProperty, value)
        End Set
    End Property

    Private Shared Sub ComplementacionPrecioPromedioChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        obj.vmObtenerOrdenSAE.ComplementacionPrecioPromedio = obj.ComplementacionPrecioPromedio
    End Sub

    '************************************************************************************************
    Public Shared ReadOnly EspecieComplementacionPrecioPromedioProperty As DependencyProperty = DependencyProperty.Register("EspecieComplementacionPrecioPromedio",
                                                                                             GetType(String),
                                                                                             GetType(ObtenerOrdenSAEView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf EspecieComplementacionPrecioPromedioChanged)))
    Public Property EspecieComplementacionPrecioPromedio() As String
        Get
            Return CStr(GetValue(EspecieComplementacionPrecioPromedioProperty))
        End Get
        Set(ByVal value As String)
            SetValue(EspecieComplementacionPrecioPromedioProperty, value)
        End Set
    End Property

    Private Shared Sub EspecieComplementacionPrecioPromedioChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        obj.vmObtenerOrdenSAE.strEspecieComplementacionPrecioPromedio = obj.EspecieComplementacionPrecioPromedio
    End Sub

    '************************************************************************************************

    Public Shared ReadOnly ReceptorSeleccionadoProperty As DependencyProperty = DependencyProperty.Register("ReceptorSeleccionado",
                                                                                                GetType(String),
                                                                                                GetType(ObtenerOrdenSAEView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf ReceptorSeleccionadoChanged)))
    Public Property ReceptorSeleccionado() As String
        Get
            Return CStr(GetValue(ReceptorSeleccionadoProperty))
        End Get
        Set(ByVal value As String)
            SetValue(ReceptorSeleccionadoProperty, value)
        End Set
    End Property

    Private Shared Sub ReceptorSeleccionadoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        obj.vmObtenerOrdenSAE.CodigoReceptor = obj.ReceptorSeleccionado
    End Sub

    Public Shared ReadOnly OrdenSAEAccionesSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("OrdenSAEAccionesSeleccionada",
                                                                                                GetType(OYDPLUSUtilidades.tblOrdenesSAEAcciones),
                                                                                                GetType(ObtenerOrdenSAEView), New PropertyMetadata(Nothing))
    Public Property OrdenSAEAccionesSeleccionada() As OYDPLUSUtilidades.tblOrdenesSAEAcciones
        Get
            Return CType(GetValue(OrdenSAEAccionesSeleccionadaProperty), OYDPLUSUtilidades.tblOrdenesSAEAcciones)
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOrdenesSAEAcciones)
            SetValue(OrdenSAEAccionesSeleccionadaProperty, value)
        End Set
    End Property

    Public Shared ReadOnly OrdenSAERentaFijaSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("OrdenSAERentaFijaSeleccionada",
                                                                                                GetType(OYDPLUSUtilidades.tblOrdenesSAERentaFija),
                                                                                                GetType(ObtenerOrdenSAEView), New PropertyMetadata(Nothing))
    Public Property OrdenSAERentaFijaSeleccionada() As OYDPLUSUtilidades.tblOrdenesSAERentaFija
        Get
            Return CType(GetValue(OrdenSAERentaFijaSeleccionadaProperty), OYDPLUSUtilidades.tblOrdenesSAERentaFija)
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOrdenesSAERentaFija)
            SetValue(OrdenSAERentaFijaSeleccionadaProperty, value)
        End Set
    End Property

    Public Shared ReadOnly OrdenSeleccionadaProperty As DependencyProperty = DependencyProperty.Register("OrdenSeleccionada",
                                                                                                GetType(Boolean),
                                                                                                GetType(ObtenerOrdenSAEView), New PropertyMetadata(Nothing))
    Public Property OrdenSeleccionada() As Boolean
        Get
            Return CBool(GetValue(OrdenSeleccionadaProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(OrdenSeleccionadaProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ClaseOrdenProperty As DependencyProperty = DependencyProperty.Register("ClaseOrden",
                                                                                           GetType(String),
                                                                                           GetType(ObtenerOrdenSAEView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf ClaseOrdenChanged)))
    Public Property ClaseOrden() As String
        Get
            Return CStr(GetValue(ClaseOrdenProperty))
        End Get
        Set(ByVal value As String)
            SetValue(ClaseOrdenProperty, value)
        End Set
    End Property

    Private Shared Sub ClaseOrdenChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        If Not IsNothing(obj.vmObtenerOrdenSAE) Then
            obj.vmObtenerOrdenSAE.ClaseOrden = obj.ClaseOrden
        End If
    End Sub

    Public Shared ReadOnly TipoOperacionProperty As DependencyProperty = DependencyProperty.Register("TipoOperacion",
                                                                                           GetType(String),
                                                                                           GetType(ObtenerOrdenSAEView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoOperacionChanged)))
    Public Property TipoOperacion() As String
        Get
            Return CStr(GetValue(TipoOperacionProperty))
        End Get
        Set(ByVal value As String)
            SetValue(TipoOperacionProperty, value)
        End Set
    End Property

    Private Shared Sub TipoOperacionChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        If Not IsNothing(obj.vmObtenerOrdenSAE) Then
            obj.vmObtenerOrdenSAE.TipoOperacion = obj.TipoOperacion
        End If
    End Sub

    Public Shared ReadOnly ConsultarOrdenesProperty As DependencyProperty = DependencyProperty.Register("ConsultarOrdenes",
                                                                                           GetType(Boolean),
                                                                                           GetType(ObtenerOrdenSAEView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf ConsultarOrdenesChanged)))
    Public Property ConsultarOrdenes() As Boolean
        Get
            Return CBool(GetValue(ConsultarOrdenesProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConsultarOrdenesProperty, value)
        End Set
    End Property

    Private Shared Sub ConsultarOrdenesChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        If Not IsNothing(obj.vmObtenerOrdenSAE) Then
            obj.vmObtenerOrdenSAE.ConsultarOrdenes = obj.ConsultarOrdenes
        End If
    End Sub

    Public Shared ReadOnly TipoNegocioProperty As DependencyProperty = DependencyProperty.Register("TipoNegocio",
                                                                                                   GetType(String),
                                                                                                   GetType(ObtenerOrdenSAEView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoNegocioChanged)))
    Public Property TipoNegocio() As String
        Get
            Return CStr(GetValue(TipoNegocioProperty))
        End Get
        Set(ByVal value As String)
            SetValue(TipoNegocioProperty, value)
        End Set
    End Property

    Private Shared Sub TipoNegocioChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        obj.vmObtenerOrdenSAE.TipoNegocio = obj.TipoNegocio
    End Sub

    Public Shared ReadOnly ListaOrdenSAEAccionesProperty As DependencyProperty = DependencyProperty.Register("ListaOrdenSAEAcciones",
                                                                                                GetType(List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)),
                                                                                                GetType(ObtenerOrdenSAEView), New PropertyMetadata(Nothing))
    Public Property ListaOrdenSAEAcciones() As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones)
        Get
            Return CType(GetValue(ListaOrdenSAEAccionesProperty), List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones))
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblOrdenesSAEAcciones))
            SetValue(ListaOrdenSAEAccionesProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ListaOrdenSAERentaFijaProperty As DependencyProperty = DependencyProperty.Register("ListaOrdenSAERentaFija",
                                                                                                GetType(List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)),
                                                                                                GetType(ObtenerOrdenSAEView), New PropertyMetadata(Nothing))
    Public Property ListaOrdenSAERentaFija() As List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija)
        Get
            Return CType(GetValue(ListaOrdenSAERentaFijaProperty), List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija))
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblOrdenesSAERentaFija))
            SetValue(ListaOrdenSAERentaFijaProperty, value)
        End Set
    End Property

    Private Shared Sub ListaOrdenSAERentaFijaHabilitarChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        obj.vmObtenerOrdenSAE.ListaOrdenesSAERentaFija = obj.ListaOrdenSAERentaFija
    End Sub

    Public Shared ReadOnly LiquidacionesHabilitarProperty As DependencyProperty = DependencyProperty.Register("LiquidacionesHabilitar",
                                                                                                   GetType(String),
                                                                                                   GetType(ObtenerOrdenSAEView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf LiquidacionesHabilitarChanged)))
    Public Property LiquidacionesHabilitar() As String
        Get
            Return CStr(GetValue(LiquidacionesHabilitarProperty))
        End Get
        Set(ByVal value As String)
            SetValue(LiquidacionesHabilitarProperty, value)
        End Set
    End Property

    Private Shared Sub LiquidacionesHabilitarChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        obj.vmObtenerOrdenSAE.LiquidacionesHabilitar = obj.LiquidacionesHabilitar
    End Sub


    Public Shared ReadOnly strEspeciesProperty As DependencyProperty = DependencyProperty.Register("strEspecies",
                                                                                                   GetType(String),
                                                                                                   GetType(ObtenerOrdenSAEView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf strEspecieChanged)))

    Public Property strEspecies() As String
        Get
            Return CStr(GetValue(strEspeciesProperty))
        End Get
        Set(ByVal value As String)
            SetValue(strEspeciesProperty, value)
        End Set
    End Property

    Private Shared Sub strEspecieChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(d, ObtenerOrdenSAEView)

        If Not IsNothing(obj.vmObtenerOrdenSAE.ClaseOrden) Then

            obj.vmObtenerOrdenSAE.Especie = obj.strEspecies

            If obj.vmObtenerOrdenSAE.ClaseOrden = "C" Then
                    obj.vmObtenerOrdenSAE.FechaEmision = obj.FechaEmision
                obj.vmObtenerOrdenSAE.FechaVencimiento = obj.FechaVencimiento
                obj.vmObtenerOrdenSAE.Modalidad = obj.Modalidad
            End If

            'obj.vmObtenerOrdenSAE.FiltrarOperacionesEspecieCumplimiento()
        End If
    End Sub

    Public Shared ReadOnly FechaEmisionProperty As DependencyProperty = DependencyProperty.Register("FechaEmision",
                                                                                                    GetType(Nullable(Of DateTime)),
                                                                                                    GetType(ObtenerOrdenSAEView), New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf FechaEmisionChanged)))

    Public Property FechaEmision() As Nullable(Of DateTime)
        Get
            Return CType(GetValue(FechaEmisionProperty), Nullable(Of DateTime))
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            SetValue(FechaEmisionProperty, value)
        End Set
    End Property

    Private Shared Sub FechaEmisionChanged(ByVal e As DependencyObject, ByVal f As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(e, ObtenerOrdenSAEView)

        'obj.vmObtenerOrdenSAE.FechaEmision = Date.Now
        obj.vmObtenerOrdenSAE.FechaEmision = obj.FechaEmision
        'obj.vmObtenerOrdenSAE.FiltrarOperacionesEspecieCumplimiento()
    End Sub

    Public Shared ReadOnly FechaVencimientoProperty As DependencyProperty = DependencyProperty.Register("FechaVencimiento",
                                                                                                        GetType(Nullable(Of DateTime)),
                                                                                                        GetType(ObtenerOrdenSAEView), New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf FechaVencimientoChanged)))

    Public Property FechaVencimiento() As Nullable(Of DateTime)
        Get
            Return CType(GetValue(FechaVencimientoProperty), Nullable(Of DateTime))
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            SetValue(FechaVencimientoProperty, value)
        End Set
    End Property

    Private Shared Sub FechaVencimientoChanged(ByVal v As DependencyObject, ByVal f As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(v, ObtenerOrdenSAEView)

        'obj.vmObtenerOrdenSAE.FechaVencimiento = Date.Now
        obj.vmObtenerOrdenSAE.FechaVencimiento = obj.FechaVencimiento
        'obj.vmObtenerOrdenSAE.FiltrarOperacionesEspecieCumplimiento()
    End Sub

    Public Shared ReadOnly ModalidadProperty As DependencyProperty = DependencyProperty.Register("Modalidad",
                                                                                                 GetType(String),
                                                                                                 GetType(ObtenerOrdenSAEView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf ModalidadChanged)))

    Public Property Modalidad() As String
        Get
            Return CStr(GetValue(ModalidadProperty))
        End Get
        Set(ByVal value As String)
            SetValue(ModalidadProperty, value)
        End Set
    End Property

    Private Shared Sub ModalidadChanged(ByVal m As DependencyObject, ByVal o As DependencyPropertyChangedEventArgs)
        Dim obj As ObtenerOrdenSAEView = DirectCast(m, ObtenerOrdenSAEView)

        obj.vmObtenerOrdenSAE.Modalidad = obj.Modalidad
        'obj.vmObtenerOrdenSAE.FiltrarOperacionesEspecieCumplimiento()
    End Sub
#End Region


    Private Async Sub btnRefrescar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(vmObtenerOrdenSAE) Then
                If ComplementacionPrecioPromedio Then
                    Await vmObtenerOrdenSAE.ConsultarOrdenesSAE_CargaMasivaComplementacionPrecioPromedio()

                Else
                    Await vmObtenerOrdenSAE.ConsultarOrdenesSAE()

                    vmObtenerOrdenSAE.FiltrarOperacionesEspecieCumplimiento()

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar la consulta",
                                 Me.ToString(), "btnRefrescar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub AccionesCheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Try
            Dim intItemSeleccionado As Integer = CInt(CType(sender, CheckBox).Tag)
            If vmObtenerOrdenSAE.ListaOrdenesSAEAcciones.Where(Function(i) i.ID = intItemSeleccionado).Count > 0 Then
                If IsNothing(vmObtenerOrdenSAE.OrdenesSAEAccionesSelected) Then
                    vmObtenerOrdenSAE.OrdenesSAEAccionesSelected = vmObtenerOrdenSAE.ListaOrdenesSAEAcciones.Where(Function(i) i.ID = intItemSeleccionado).First
                Else
                    If vmObtenerOrdenSAE.OrdenesSAEAccionesSelected.ID <> intItemSeleccionado Then
                        vmObtenerOrdenSAE.OrdenesSAEAccionesSelected = vmObtenerOrdenSAE.ListaOrdenesSAEAcciones.Where(Function(i) i.ID = intItemSeleccionado).First
                    End If
                End If

                vmObtenerOrdenSAE.OrdenesSAEAccionesSelected.Seleccionada = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el registro",
                                 Me.ToString(), "AccionesCheckBox_Checked", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub AccionesCheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        Try
            Dim intItemSeleccionado As Integer = CInt(CType(sender, CheckBox).Tag)
            If vmObtenerOrdenSAE.ListaOrdenesSAEAcciones.Where(Function(i) i.ID = intItemSeleccionado).Count > 0 Then
                If IsNothing(vmObtenerOrdenSAE.OrdenesSAEAccionesSelected) Then
                    vmObtenerOrdenSAE.OrdenesSAEAccionesSelected = vmObtenerOrdenSAE.ListaOrdenesSAEAcciones.Where(Function(i) i.ID = intItemSeleccionado).First
                Else
                    If vmObtenerOrdenSAE.OrdenesSAEAccionesSelected.ID <> intItemSeleccionado Then
                        vmObtenerOrdenSAE.OrdenesSAEAccionesSelected = vmObtenerOrdenSAE.ListaOrdenesSAEAcciones.Where(Function(i) i.ID = intItemSeleccionado).First
                    End If
                End If

                vmObtenerOrdenSAE.OrdenesSAEAccionesSelected.Seleccionada = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el registro",
                                 Me.ToString(), "AccionesCheckBox_Unchecked", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
End Class
