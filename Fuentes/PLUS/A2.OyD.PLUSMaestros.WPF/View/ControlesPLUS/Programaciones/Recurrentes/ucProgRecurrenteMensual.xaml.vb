Imports Telerik.Windows.Controls
Partial Public Class ucProgRecurrenteMensual
    Inherits UserControl

    Private _view As ProgramacionViewModel

    Public Sub New()
        Try
            _view = CType(Application.Current.Resources("ProgViewModel"), ProgramacionViewModel)
            InitializeComponent()
            Me.DataContext = _view
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al instanciar el formulario de recurrencia mensual.", _
                                                      Me.ToString(), "ucProgRecurrenteMensual", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
End Class
