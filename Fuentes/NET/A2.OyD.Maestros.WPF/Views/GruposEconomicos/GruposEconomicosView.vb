Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: GruposEconomicosView.xaml.vb
'Generado el : 03/06/2012 17:14:59
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class GruposEconomicosView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorClienteListaButon

    Public Sub New()
        Me.DataContext = New GruposEconomicosViewModel
InitializeComponent()
    End Sub

    Private Sub GruposEconomicos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, GruposEconomicosViewModel).NombreView = Me.ToString
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
    Private Sub Buscador_finalizoBusquedaGrupos(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, GruposEconomicosViewModel).GrupoEconomicoSelected.ComitenteLider = pobjItem.CodigoOYD
            CType(Me.DataContext, GruposEconomicosViewModel).GrupoEconomicoSelected.NombreLider = pobjItem.Nombre
        End If
    End Sub


    Private Sub BuscadorGenerico_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "IdComitente"
                    CType(Me.DataContext, GruposEconomicosViewModel).validarNuevoDetalle(pobjItem)
            End Select
        End If
    End Sub

    Private Sub Buscador_finalizoBusquedaCliente(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "idComitenteLider"
                    CType(Me.DataContext, GruposEconomicosViewModel).cb.idComitenteLider = pobjItem.CodigoOYD
            End Select
        End If
    End Sub

End Class


