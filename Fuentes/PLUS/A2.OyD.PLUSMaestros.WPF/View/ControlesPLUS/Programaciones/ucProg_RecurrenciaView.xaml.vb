Imports Telerik.Windows.Controls
Partial Public Class ucProg_RecurrenciaView
    Inherits UserControl

    Private _viewModel As ProgramacionViewModel

    Public Sub New()
        Try
            _viewModel = CType(Application.Current.Resources("ProgViewModel"), ProgramacionViewModel)
            InitializeComponent()
            Me.DataContext = _viewModel
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al instanciar el formulario de recurrencias.", _
                                                        Me.ToString(), "ucProg_RecurrenciaView", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
End Class
