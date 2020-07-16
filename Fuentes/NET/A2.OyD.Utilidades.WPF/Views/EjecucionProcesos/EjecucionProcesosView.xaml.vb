Imports Telerik.Windows.Controls
Partial Public Class EjecucionProcesosView
    Inherits UserControl
    Dim objVMEjecucion As EjecucionProcesosViewModel
    Dim objVMEjecucionProcesos As EjecucionProcesosViewModel
    Public Sub New()
        InitializeComponent()
        AddHandler Me.Unloaded, AddressOf View_Unloaded
        objVMEjecucionProcesos = Me.LayoutRoot.DataContext
        'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla
        Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.97
        Me.dgDetallesproceso.MaxHeight = Application.Current.MainWindow.ActualHeight - 350

        objVMEjecucion = CType(Me.Resources("VMEjecucion"), EjecucionProcesosViewModel)
        Me.DataContext = objVMEjecucion
    End Sub

    Private Sub btnEjecutar_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objVMEjecucion) Then
            objVMEjecucion.ConsultarIniciarProceso()
        End If
    End Sub
    Private Sub btnRefrescar_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objVMEjecucion) Then
            objVMEjecucion.ConsultarProceso()
        End If
    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMEjecucion) Then
                objVMEjecucion.pararTemporizador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de ordenes.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'JAEZ 20161214  se creametodo para Refrescar  cuando se entra de nuevo la pantalla
    Private Sub View_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Not IsNothing(objVMEjecucion) Then
                objVMEjecucion.ConsultarProceso()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de ordenes.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.97
        Me.dgDetallesproceso.MaxHeight = Application.Current.MainWindow.ActualHeight - 350
    End Sub

End Class
