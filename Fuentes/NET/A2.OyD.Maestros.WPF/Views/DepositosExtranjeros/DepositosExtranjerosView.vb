﻿Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: DepositosExtranjerosView.xaml.vb
'Generado el : 02/28/2011 12:38:32
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  DepositosExtranjerosView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New DepositosExtranjerosViewModel
InitializeComponent()
    End Sub

    Private Sub DepositosExtranjeros_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
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
	
End Class


