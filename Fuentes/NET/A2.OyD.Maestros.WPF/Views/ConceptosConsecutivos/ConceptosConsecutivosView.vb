Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ConceptosConsecutivosView.xaml.vb
'Generado el : 03/31/2011 13:12:36
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  ConceptosConsecutivosView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ConceptosConsecutivosViewModel
InitializeComponent()
        'Disponibles.Visibility = Visibility.Collapsed
        'Registrados.Visibility = Visibility.Collapsed

    End Sub

    Private Sub ConceptosConsecutivos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, ConceptosConsecutivosViewModel).NombreColeccionDetalle = Me.ToString
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
	
        'Private Sub dfBuscar_ContentLoaded(ByVal sender As Object, ByVal e As System.Windows.Controls.DataFormContentLoadEventArgs) Handles dfBuscar.ContentLoaded
    '    Me.dfBuscar.FindName("NombreConcepto")
    'End Sub
End Class


