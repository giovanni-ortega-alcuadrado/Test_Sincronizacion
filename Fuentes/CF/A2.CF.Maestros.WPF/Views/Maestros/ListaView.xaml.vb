Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ListasView.xaml.vb
'Generado el : 01/27/2011 09:30:33
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ListasView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ListasViewModel
InitializeComponent()
    End Sub

    Private Sub Listas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        'cm.DF = df
        inicializar()
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
        End If
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

End Class


