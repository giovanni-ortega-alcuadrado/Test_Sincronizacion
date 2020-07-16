Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: Bancos_BancosNacionalesRelacionadosView.xaml.vb
'Generado el : 04/11/2012 14:00:12
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class Bancos_BancosNacionalesRelacionadosView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista

    Public Sub New()
        Me.DataContext = New Bancos_BancosNacionalesRelacionadosViewModel
InitializeComponent()
    End Sub

    Private Sub Bancos_BancosNacionalesRelacionados_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, Bancos_BancosNacionalesRelacionadosViewModel).NombreView = Me.ToString
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

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, Bancos_BancosNacionalesRelacionadosViewModel).cb.IDBanco = CType(pobjItem.IdItem, Integer)
            CType(Me.DataContext, Bancos_BancosNacionalesRelacionadosViewModel).cb.NombreBanco = pobjItem.Nombre
        End If
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

End Class


