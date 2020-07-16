Imports Telerik.Windows.Controls
Partial Public Class wppProg_Fechas
    Inherits Window

    Private _view As ProgramacionViewModel

    Public Sub New()
        _view = CType(Application.Current.Resources("ProgViewModel"), ProgramacionViewModel)
        InitializeComponent()
        Me.DataContext = _view
        _view.viewProgramacionesFechas = Me
    End Sub

    Private Async Sub wppProg_Fechas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Not Program.IsDesignMode() Then
                Await _view.ConsultarFechasProgramacion()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga inicial las fechas.", Me.ToString(), "wppProg_Fechas_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        If Not IsNothing(_view) Then
            _view.RealizarLlamadosSincronicos(ProgramacionViewModel.TipoConsulta.InactivarFechas)
        End If
    End Sub

    Private Sub btnVerLog_Click(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(_view) Then
            If CType(sender, Button).Tag.ToString <> _view.FechaSeleccionada.ID.ToString Then
                _view.FechaSeleccionada = _view.ListaFechas.Where(Function(i) i.ID.ToString = CType(sender, Button).Tag.ToString).First
            End If

            _view.RealizarLlamadosSincronicos(ProgramacionViewModel.TipoConsulta.LogsFechas)
        End If
    End Sub
End Class
