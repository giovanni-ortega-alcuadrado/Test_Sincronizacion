Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports System.Collections.ObjectModel


Partial Public Class InactivacionClientesView
    Inherits UserControl

    Dim vm As InactivacionClientesViewModel
    Private objLiqSel As New A2.OyD.OYDServer.RIA.Web.OyDBolsa.TituloArregloUbicacion
    Private personList As List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ListaClientesInactivar) = New List(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.ListaClientesInactivar)

    Public Sub New()
        Me.DataContext = New InactivacionClientesViewModel
InitializeComponent()
    End Sub

    Private Sub ArregloUbicaciónTituloView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, InactivacionClientesViewModel)
        CType(Me.DataContext, InactivacionClientesViewModel).NombreView = Me.ToString
    End Sub

    Private Sub btnConsultar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnConsultar.Click
        vm.ConsultarListado()
    End Sub

    Private Sub btnAceptar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        vm.EjecutarActualizacion(False)
    End Sub
    Private Sub btnCancelar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCancelar.Click
        vm.LimpiarGrid()
    End Sub

End Class


