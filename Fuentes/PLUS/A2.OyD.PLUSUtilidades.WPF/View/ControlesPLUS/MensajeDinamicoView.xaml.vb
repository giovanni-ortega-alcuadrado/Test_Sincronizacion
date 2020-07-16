Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Partial Public Class MensajeDinamicoView
    Inherits UserControl
    Implements INotifyPropertyChanged

    Dim AnchoPantalla As Double = 0
    Dim AltoPantalla As Double = 0

    Public Sub New()
        Try
            InitializeComponent()
            Me.DataContext = Me
            AnchoPantalla = Application.Current.MainWindow.ActualWidth - 100
            AltoPantalla = Application.Current.MainWindow.ActualHeight - 100
            'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error cuando se cargaba el mensaje dinamico", Me.Name, "MensajeDinamicoView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Public Shared ReadOnly DefectoProperty As DependencyProperty = DependencyProperty.Register("Defecto", _
                                                                                             GetType(Boolean), _
                                                                                             GetType(MensajeDinamicoView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf DefectoChanged)))
    Public Property Defecto() As Boolean
        Get
            Return CType(GetValue(DefectoProperty), Boolean)
        End Get
        Set(ByVal value As Boolean)
            SetValue(DefectoProperty, value)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Defecto"))
        End Set
    End Property
    Private Shared Sub DefectoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As MensajeDinamicoView = DirectCast(d, MensajeDinamicoView)

        If obj.Defecto Then
            obj.EstablecerValoresDefecto()
        End If
    End Sub


    'Propiedad para la lista de mensajes.
    Public Shared ReadOnly ListaMensajeProperty As DependencyProperty = DependencyProperty.Register("ListaMensaje", _
                                                                                              GetType(List(Of OYDPLUSUtilidades.tblMensajes)), _
                                                                                              GetType(MensajeDinamicoView), New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf ListaMensajeChanged)))
    Public Property ListaMensaje() As List(Of OYDPLUSUtilidades.tblMensajes)
        Get
            Return CType(GetValue(ListaMensajeProperty), List(Of OYDPLUSUtilidades.tblMensajes))
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblMensajes))
            SetValue(ListaMensajeProperty, value)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaMensaje"))
        End Set
    End Property

    Private Shared Sub ListaMensajeChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As MensajeDinamicoView = DirectCast(d, MensajeDinamicoView)

        If Not IsNothing(obj.Titulo) Then
            obj.EstablecerAnchoMensaje()
        End If
    End Sub

    Public Shared ReadOnly TituloProperty As DependencyProperty = DependencyProperty.Register("Titulo", _
                                                                                       GetType(String), _
                                                                                       GetType(MensajeDinamicoView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TituloChanged)))
    Public Property Titulo() As String
        Get
            Return CStr(GetValue(TituloProperty))
        End Get
        Set(ByVal value As String)
            SetValue(TituloProperty, value)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Titulo"))
        End Set
    End Property

    Private Shared Sub TituloChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As MensajeDinamicoView = DirectCast(d, MensajeDinamicoView)

        If Not String.IsNullOrEmpty(obj.Titulo) Then

            obj.EstablecerAnchoMensaje()
        End If
    End Sub

    Public Shared ReadOnly VelocidadTextoProperty As DependencyProperty = DependencyProperty.Register("VelocidadTexto", _
                                                                                       GetType(String), _
                                                                                       GetType(MensajeDinamicoView), New PropertyMetadata("0:00:40", New PropertyChangedCallback(AddressOf VelocidadTextoChanged)))
    Public Property VelocidadTexto() As String
        Get
            Return CStr(GetValue(VelocidadTextoProperty))
        End Get
        Set(ByVal value As String)
            SetValue(VelocidadTextoProperty, value)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VelocidadTexto"))
        End Set
    End Property

    Private Shared Sub VelocidadTextoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)

    End Sub

    Private _AnchoTitulo As Double = 100
    Public Property AnchoTitulo() As Double
        Get
            Return _AnchoTitulo
        End Get
        Set(ByVal value As Double)
            _AnchoTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AnchoTitulo"))
        End Set
    End Property

    Private _AnchoMensaje As Double = 1200
    Public Property AnchoMensaje() As Double
        Get
            Return _AnchoMensaje
        End Get
        Set(ByVal value As Double)
            _AnchoMensaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AnchoMensaje"))
        End Set
    End Property

    Private _AnchoNegativoMensaje As Double = -500
    Public Property AnchoNegativoMensaje() As Double
        Get
            Return _AnchoNegativoMensaje
        End Get
        Set(ByVal value As Double)
            _AnchoNegativoMensaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AnchoNegativoMensaje"))
        End Set
    End Property

    Private _AnchoNegativoTitulo As Double = -600
    Public Property AnchoNegativoTitulo() As Double
        Get
            Return _AnchoNegativoTitulo
        End Get
        Set(ByVal value As Double)
            _AnchoNegativoTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AnchoNegativoTitulo"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Private Sub LayoutRoot_SizeChanged(sender As System.Object, e As System.Windows.SizeChangedEventArgs)
        EstablecerAnchoMensaje()
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        AnchoPantalla = Application.Current.MainWindow.ActualWidth
        AltoPantalla = Application.Current.MainWindow.ActualHeight
    End Sub

    Public Sub EstablecerAnchoMensaje()
        Try
            Dim strMensaje As String = String.Empty
            If Not IsNothing(ListaMensaje) Then
                For Each li In ListaMensaje
                    strMensaje = String.Format("{0} {1} {2}", strMensaje, li.Valor, li.Descripcion)
                Next

                If Not String.IsNullOrEmpty(strMensaje) Then
                    AnchoNegativoMensaje = -(strMensaje.Length + 200 + AnchoPantalla)
                Else
                    AnchoNegativoMensaje = -(500 + AnchoPantalla)
                End If

                If Not String.IsNullOrEmpty(Titulo) Then
                    AnchoMensaje = Titulo.Length + AnchoPantalla + 200
                    AnchoNegativoMensaje = AnchoNegativoMensaje - Titulo.Length
                End If

                Dim anchoadicional As Integer = 500

                If strMensaje.Length > 500 Then
                    anchoadicional = 3500
                    AnchoNegativoMensaje = AnchoNegativoMensaje - anchoadicional
                End If

            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error cuando se intentaba establecer el ancho del mensaje", Me.Name, "EstablecerAnchoMensaje", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Public Sub EstablecerValoresDefecto()
        Try
            AnchoTitulo = 100
            AnchoMensaje = 100
            AnchoNegativoMensaje = -500
            AnchoNegativoTitulo = -600
            EstablecerAnchoMensaje()
        Catch ex As Exception

        End Try
    End Sub
End Class
