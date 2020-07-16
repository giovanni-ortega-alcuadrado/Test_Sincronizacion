Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports Microsoft.VisualBasic.CompilerServices


Partial Public Class InformeAuditoriaTablas
    Inherits UserControl

    Public Sub New()
        'Dim objA2VM As A2UtilsViewModel
        'Dim strClaseCombos As String = ""
        'Dim strModuloTesoreria As String = ""

        'strClaseCombos = "InformeAuditoriaTablas"

        Try
            'objA2VM = New A2UtilsViewModel(strClaseCombos)
            'Me.Resources.Add("A2VM", objA2VM)

            Me.DataContext = New InformeAuditoriaTablasViewModel
InitializeComponent()

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub InformeAuditoriaTablas_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CType(Me.DataContext, InformeAuditoriaTablasViewModel).Limpiar()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, InformeAuditoriaTablasViewModel).EjecutarConsulta()
    End Sub
End Class
