Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ImportacionLiqView.xaml.vb
'Generado el : 07/19/2011 09:26:12
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OYD.OYDServer.RIA.Web

Partial Public Class ActLiquidacionesImportadasView_MI
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ActLiquidacionesImportadasViewModel_MI
InitializeComponent()
    End Sub

    Private Sub ImportacionLiq_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, ActLiquidacionesImportadasViewModel_MI).NombreView = Me.ToString
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
    Private Sub BuscadorGenerico_finalizoBusquedaorden(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel_MI).ImportacionLiSelected.IDOrden = CType(pobjItem.CodItem, Integer)
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel_MI).ordenselec.Cantidad = pobjItem.InfoAdicional01
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel_MI).ordenselec.Comitente = pobjItem.InfoAdicional02
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel_MI).ordenselec.Nombre = pobjItem.Clasificacion
        End If
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'df.FindName("BuscadorOrdenes").EstadoItem = CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ImportacionLiSelected.TipoOrden
        CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = CType(Me.DataContext, ActLiquidacionesImportadasViewModel_MI).ImportacionLiSelected.ClaseOrden & "," &
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel_MI).ImportacionLiSelected.Tipo & "." & CType(Me.DataContext, ActLiquidacionesImportadasViewModel_MI).ImportacionLiSelected.IDEspecie
    End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

End Class


