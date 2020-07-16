Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ClasificacionRiesgoViewModel.xaml.vb
'Generado el : 07/03/2014 11:45:58
'Propiedad de Alcuadrado S.A. 2013


Partial Public Class ClasificacionRiesgoTipoClienteView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ClasificacionRiesgoTipoClienteViewModel
InitializeComponent()
    End Sub

    Private Sub ClasificacionRiesgoTipoCliente_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        'CType(Me.DataContext,CiudadesViewModel).NombreView = Me.ToString
    End Sub

    Private Sub CancelarEditarRegistroTipoCliente_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
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



