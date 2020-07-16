Imports Telerik.Windows.Controls
Partial Public Class wppProg_GeneracionFechas
    Inherits Window

    Private _view As ProgramacionViewModel

    Public Sub New()
        _view = CType(Application.Current.Resources("ProgViewModel"), ProgramacionViewModel)
        InitializeComponent()
        Me.DataContext = _view
    End Sub

    Private Async Sub wppProg_GeneracionFechas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Not Program.IsDesignMode() Then
                Await _view.ConsultarFechasGeneradas()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga inicial de la generación de fechas.", Me.ToString(), "wppProg_GeneracionFechas_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub

End Class
