Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: BolsasView.xaml.vb
'Generado el : 02/09/2011 11:50:52
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  BolsasView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New BolsasViewModel
InitializeComponent()
    End Sub

    Private Sub Bolsas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
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
        'End If
    End Sub
    Private Sub cm_Nuevo(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoNuevoRegistro
        ' df.FindName("FontsCombo").Text = " "

    End Sub
    

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub
	
End Class


