Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: DepartamentosView.xaml.vb
'Generado el : 04/12/2011 16:59:11
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class DepartamentosView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New DepartamentosViewModel
InitializeComponent()
    End Sub

    Private Sub Departamentos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        'cm.DF = df
        CType(Me.DataContext, DepartamentosViewModel).NombreColeccionDetalle = Me.ToString
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

End Class


