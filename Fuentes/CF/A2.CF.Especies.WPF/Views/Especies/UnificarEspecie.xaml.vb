﻿Imports Telerik.Windows.Controls


Partial Public Class UnificarEspecie


    Inherits UserControl
    Dim vm As UnificarEspecieViewModel

    Public Sub New()

        Me.DataContext = New UnificarEspecieViewModel
InitializeComponent()
    End Sub
    Private Sub UnificarCuenta_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        vm = CType(Me.DataContext, UnificarEspecieViewModel)
        CType(Me.DataContext, UnificarEspecieViewModel).NombreColeccionDetalle = Me.ToString
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub


    Public Sub Rfocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub
    Public Sub Ufocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

    End Sub



    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

    Private Sub btnUnificar_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnUnificar.Click
        vm.ActualizaRegistro()
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnCancelar.Click
        vm.cancelar()
    End Sub
End Class
