Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ReceptoresSistemasView.xaml.vb
'Generado el : 04/20/2011 11:55:31
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  ReceptoresSistemasView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ReceptoresSistemasViewModel
InitializeComponent()

    End Sub

    Private Sub ReceptoresSistemas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, ReceptoresSistemasViewModel).NombreColeccionDetalle = Me.ToString
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

    Private Sub txtNombre_TextChanged(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        If CType(sender, TextBox).Text <> String.Empty Then
            CType(Me.DataContext, ReceptoresSistemasViewModel).ReceptoresSistemaSelected.Nombre = CType(sender, TextBox).Text
        End If
    End Sub
	
End Class


