Imports Telerik.Windows.Controls

'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: PerfilesRiesgoView.xaml.vb
'Generado el : 07/22/2014
'Propiedad de Alcuadrado S.A.


Partial Public Class PerfilesRiesgoView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New PerfilesRiesgoViewModel
InitializeComponent()
    End Sub

    Private Sub PerfilesRiesgo_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, PerfilesRiesgoViewModel).NombreColeccionDetalle = Me.ToString
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


