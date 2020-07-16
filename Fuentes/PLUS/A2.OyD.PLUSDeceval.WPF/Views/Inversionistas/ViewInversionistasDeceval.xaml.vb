﻿Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports System.ComponentModel


Partial Public Class ViewInversionistasDeceval
    Inherits UserControl

    Dim vm As ViewModelInvercionistasDeceval

    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Me.DataContext = New ViewModelInvercionistasDeceval
InitializeComponent()
        vm = Me.LayoutRoot.DataContext

        'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla
        Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.97
    End Sub
    Private Sub ViewInversionistasDeceval_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        Try
            
            'cm.DF = df
            If vm Is Nothing Then
                vm = CType(Me.DataContext, ViewModelInvercionistasDeceval)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "ViewInversionistasDeceval_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.97
    End Sub


    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            If Not IsNothing(vm) Then
                vm.Comitente = pobjComitente.CodigoOYD
                vm.DescripcionComitente = "(" & LTrim(RTrim(pobjComitente.CodigoOYD)) & ")" & " " & pobjComitente.NombreCodigoOYD
            End If
        End If
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(vm) Then
            vm.LimpiarComitente()
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        vm.Consultar()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        vm.VerDetalle()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        vm.EnviarConfirmar()
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        vm.Rechazar()
    End Sub
End Class
