Imports Telerik.Windows.Controls
Partial Public Class ConsultarEstadosDocumentoBoton
    Inherits UserControl

    Public Sub New 
        InitializeComponent()
    End Sub

    Private Sub btnConsultarEstados_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim viewEstadosDocumento As New ConsultarEstadoDocumentoView
            viewEstadosDocumento.IDDocumento = IDDocumento
            viewEstadosDocumento.IDNumeroUnico = IDNumeroUnico
            viewEstadosDocumento.Modulo = Modulo
            viewEstadosDocumento.ConsultarEstados = True

            'viewEstadosDocumento.Top = (Application.Current.MainWindow.ActualHeight / 2) - 120
            'viewEstadosDocumento.Left = 50

            Program.Modal_OwnerMainWindowsPrincipal(viewEstadosDocumento)
            viewEstadosDocumento.ShowDialog()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar el estado del documento.", Me.ToString(), "btnConsultarEstados_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Shared ReadOnly IDNumeroUnicoProperty As DependencyProperty = DependencyProperty.Register("IDNumeroUnico", _
                                                                                            GetType(Integer), _
                                                                                            GetType(ConsultarEstadosDocumentoBoton), New PropertyMetadata(0))
    Public Property IDNumeroUnico() As Integer
        Get
            Return CInt(GetValue(IDNumeroUnicoProperty))
        End Get
        Set(ByVal value As Integer)
            SetValue(IDNumeroUnicoProperty, value)
        End Set
    End Property

    Public Shared ReadOnly IDDocumentoProperty As DependencyProperty = DependencyProperty.Register("IDDocumento", _
                                                                                            GetType(Integer), _
                                                                                            GetType(ConsultarEstadosDocumentoBoton), New PropertyMetadata(0))
    Public Property IDDocumento() As Integer
        Get
            Return CInt(GetValue(IDDocumentoProperty))
        End Get
        Set(ByVal value As Integer)
            SetValue(IDDocumentoProperty, value)
        End Set
    End Property

    Public Shared ReadOnly ModuloProperty As DependencyProperty = DependencyProperty.Register("Modulo", _
                                                                                       GetType(String), _
                                                                                       GetType(ConsultarEstadosDocumentoBoton), New PropertyMetadata("ORDENES"))
    Public Property Modulo() As String
        Get
            Return CStr(GetValue(ModuloProperty))
        End Get
        Set(ByVal value As String)
            SetValue(ModuloProperty, value)
        End Set
    End Property

End Class
