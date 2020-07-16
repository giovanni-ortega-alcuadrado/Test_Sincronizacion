Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ClasificacionesView.xaml.vb
'Generado el : 02/24/2011 13:27:32
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  ClasificacionesView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ClasificacionesViewModel
InitializeComponent()
    End Sub

    Private Sub Clasificaciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        'cm.DF = df
        CType(Me.DataContext, ClasificacionesViewModel).NombreColeccionDetalle = Me.ToString
    End Sub

    
    

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

End Class


