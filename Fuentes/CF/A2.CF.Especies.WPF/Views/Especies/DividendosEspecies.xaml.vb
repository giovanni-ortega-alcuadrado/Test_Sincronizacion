Imports Telerik.Windows.Controls

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: DividendosEspecies.xaml.vb
'Generado el : 06/30/2011 10:47:24
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class DividendosEspecies
    Public Sub New()
        Me.DataContext = New DividendosEspeciesViewModel
InitializeComponent()
    End Sub
    Private Sub DividendosEspecies_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        'cm.GridTransicion = LayoutRoot
        'cm.DF = df
        CType(Me.DataContext, DividendosEspeciesViewModel).NombreView = Me.ToString
        'mobjVM = CType(Me.DataContext, EspeciesViewModel)
        inicializar()
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
        End If
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, DividendosEspeciesViewModel).editar()
    End Sub

Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, DividendosEspeciesViewModel).cancelar()
    End Sub

Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'df.CancelEdit()
                ''If Not IsNothing(df.ValidationSummary) Then
        '    df.ValidationSummary.DataContext = Nothing
        'End If
    End Sub

    'Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
    '    'df.ValidateItem()
    '    'If df.ValidationSummary.HasErrors Then
    '    '    df.CancelEdit()
    '    'Else
    '    '    df.CommitEdit()
    '    '    df.FindName("dgEspeciesISINFungible").EndEdit()
    '    'End If
    'End Sub
End Class