﻿Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ConsecutivosUsuariosView.xaml.vb
'Generado el : 04/14/2011 07:31:12
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  ConsecutivosUsuariosView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ConsecutivosUsuariosViewModel
InitializeComponent()
    End Sub

    Private Sub ConsecutivosUsuarios_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, ConsecutivosUsuariosViewModel).NombreColeccionDetalle = Me.ToString
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        End If
    End Sub

    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        df.ValidateItem()
        If Not IsNothing(df.ValidationSummary) Then
            If df.ValidationSummary.HasErrors Then
                df.CancelEdit()
            Else
                df.CommitEdit()
            End If
        End If
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub
    'Private Sub cm_EventoConfirmarGraba(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoCambiarALista
    '    CType(Me.DataContext, ConsecutivosUsuariosViewModel).CambiarAForma()
    'End Sub
End Class


