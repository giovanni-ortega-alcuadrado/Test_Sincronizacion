Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes

Partial Public Class ConsultarEstadoDocumentoView
    Inherits Window
    Dim vmEstadosDocumento As ConsultarEstadosDocumentoViewModel


    Public Sub New()
        Try
            InitializeComponent()

            vmEstadosDocumento = CType(Me.Resources("vmEstadosDocumento"), ConsultarEstadosDocumentoViewModel)
            Me.DataContext = vmEstadosDocumento
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.GridRegistroEstados.Width = Application.Current.MainWindow.ActualWidth * 0.95
            Me.GridRegistroEstados.Height = Application.Current.MainWindow.ActualHeight * 0.6

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Falló la inicialización de Consultar estados documento", Me.Name, "ConsultarEstadoDocumentoView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
       Me.GridRegistroEstados.Width = Application.Current.MainWindow.ActualWidth * 0.95
        Me.GridRegistroEstados.Height = Application.Current.MainWindow.ActualHeight * 0.6
    End Sub

    Public Shared ReadOnly IDDocumentoProperty As DependencyProperty = DependencyProperty.Register("IDDocumento", _
                                                                                            GetType(Integer), _
                                                                                            GetType(ConsultarEstadoDocumentoView), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf IDDocumentoChanged)))
    Public Property IDDocumento() As Integer
        Get
            Return CInt(GetValue(IDDocumentoProperty))
        End Get
        Set(ByVal value As Integer)
            SetValue(IDDocumentoProperty, value)
        End Set
    End Property

    Private Shared Sub IDDocumentoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ConsultarEstadoDocumentoView = DirectCast(d, ConsultarEstadoDocumentoView)
        If Not IsNothing(obj.vmEstadosDocumento) Then
            obj.vmEstadosDocumento.IDDocumento = obj.IDDocumento
        End If
    End Sub

    Public Shared ReadOnly IDNumeroUnicoProperty As DependencyProperty = DependencyProperty.Register("IDNumeroUnico", _
                                                                                            GetType(Integer), _
                                                                                            GetType(ConsultarEstadoDocumentoView), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf IDNumeroUnicoChanged)))
    Public Property IDNumeroUnico() As Integer
        Get
            Return CInt(GetValue(IDNumeroUnicoProperty))
        End Get
        Set(ByVal value As Integer)
            SetValue(IDNumeroUnicoProperty, value)
        End Set
    End Property

    Private Shared Sub IDNumeroUnicoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ConsultarEstadoDocumentoView = DirectCast(d, ConsultarEstadoDocumentoView)
        If Not IsNothing(obj.vmEstadosDocumento) Then
            obj.vmEstadosDocumento.IDNumeroUnico = obj.IDNumeroUnico
        End If
    End Sub

    Public Shared ReadOnly ConsultarEstadosProperty As DependencyProperty = DependencyProperty.Register("ConsultarEstados", _
                                                                                            GetType(Boolean), _
                                                                                            GetType(ConsultarEstadoDocumentoView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf ConsultarEstadosChanged)))
    Public Property ConsultarEstados() As Boolean
        Get
            Return CBool(GetValue(ConsultarEstadosProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConsultarEstadosProperty, value)
        End Set
    End Property

    Private Shared Sub ConsultarEstadosChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ConsultarEstadoDocumentoView = DirectCast(d, ConsultarEstadoDocumentoView)
        Try
            If Not IsNothing(obj.vmEstadosDocumento) Then
                If obj.ConsultarEstados Then
                    obj.vmEstadosDocumento.ConsultarEstadoDocumento()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error cuando se consulta el estado", obj.Name, "ConsultarEstadosChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Shared ReadOnly ModuloProperty As DependencyProperty = DependencyProperty.Register("Modulo", _
                                                                                       GetType(String), _
                                                                                       GetType(ConsultarEstadoDocumentoView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf ModuloChanged)))
    Public Property Modulo() As String
        Get
            Return CStr(GetValue(ModuloProperty))
        End Get
        Set(ByVal value As String)
            SetValue(ModuloProperty, value)
        End Set
    End Property

    Private Shared Sub ModuloChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As ConsultarEstadoDocumentoView = DirectCast(d, ConsultarEstadoDocumentoView)
        If Not IsNothing(obj.vmEstadosDocumento) Then
            obj.vmEstadosDocumento.Modulo = obj.Modulo
        End If
    End Sub

    Private Sub btnRefrescar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(vmEstadosDocumento) Then
                vmEstadosDocumento.ConsultarEstadoDocumento()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error cuando se refrescaba la pantalla", Me.Name, "btnRefrescar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub HyperlinkButton_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(sender) Then
                If Not IsNothing(CType(sender, Button).Tag) Then
                    Dim strValor As String = CStr(CType(sender, Button).Tag)
                    If Not String.IsNullOrEmpty(strValor) Then
                        Dim objMostrarInformacionAdicional As New MostrarInformacionAdicionalDocumentoView(strValor)

                        Program.Modal_OwnerMainWindowsPrincipal(objMostrarInformacionAdicional)
                        objMostrarInformacionAdicional.ShowDialog()
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error cuando se intento abrir la información adicional.", Me.Name, "HyperlinkButton_Click_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
