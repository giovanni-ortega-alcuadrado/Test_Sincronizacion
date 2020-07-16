Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ClienteAgrupadorView.xaml.vb
'Generado el : 03/06/2012 17:14:59
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  ClienteAgrupadorView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorClienteListaButon

    Public Sub New()
        Me.DataContext = New ClienteAgrupadorViewModel
InitializeComponent()
    End Sub

    Private Sub ClienteAgrupador_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
		CType(Me.DataContext,ClienteAgrupadorViewModel).NombreView = Me.ToString
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
    Private Sub Buscador_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, ClienteAgrupadorViewModel).ClienteAgrupadoSelected.idComitenteLider = pobjItem.CodigoOYD
            CType(Me.DataContext, ClienteAgrupadorViewModel).ClienteAgrupadoSelected.Nombre = pobjItem.Nombre

        End If
    End Sub

    Private Sub Buscador_Cliente_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'df.FindName("Buscador_Cliente").TipoVinculacion = "CA"
        CType(sender, A2ComunesControl.BuscadorClienteListaButon).Agrupamiento = "CA," + CType(Me.DataContext, ClienteAgrupadorViewModel).ClienteAgrupadoSelected.TipoIdentificacion + "." + CType(Me.DataContext, ClienteAgrupadorViewModel).ClienteAgrupadoSelected.NroDocumento + "-"
    End Sub

End Class



