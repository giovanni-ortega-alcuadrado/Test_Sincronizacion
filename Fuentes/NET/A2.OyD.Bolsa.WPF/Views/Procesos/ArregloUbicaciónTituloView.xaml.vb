Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes



Partial Public Class ArregloUbicaciónTituloView
    Inherits UserControl

    Dim vm As New ArregloUBICACIONTITULOViewModel
    Private objLiqSel As New A2.OyD.OYDServer.RIA.Web.OyDBolsa.TituloArregloUbicacion

    Public Sub New()
        Me.DataContext = New ArregloUBICACIONTITULOViewModel
InitializeComponent()
    End Sub

    Private Sub ArregloUbicaciónTituloView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, ArregloUBICACIONTITULOViewModel)
        CType(Me.DataContext, ArregloUBICACIONTITULOViewModel).NombreView = Me.ToString
        'dtpFechaLiquidacion.Text = Now.AddDays(-1)
    End Sub

    Private Sub cboUbicacion_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles cboUbicacion.SelectionChanged
        If Not IsNothing(vm) Then
            vm.BuscarCuentasDeposito()
        End If
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        vm.Ejecutar()
    End Sub

End Class
