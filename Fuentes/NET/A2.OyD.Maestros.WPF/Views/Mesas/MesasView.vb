Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: MesasView.xaml.vb
'Generado el : 04/20/2011 11:16:04
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class  MesasView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista

    Public Sub New()
        Me.DataContext = New MesasViewModel
InitializeComponent()
    End Sub

    Private Sub Mesas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, MesasViewModel).NombreColeccionDetalle = Me.ToString
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
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "ciudades"
                    CType(Me.DataContext, MesasViewModel).MesaSelected.IdPoblacion = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, MesasViewModel).MesaSelected.NombreCiu = pobjItem.Nombre
                    CType(Me.DataContext, MesasViewModel).CiudadesClaseSelected.Ciudad = pobjItem.Nombre
                Case "cuentascontables"
                    CType(Me.DataContext, MesasViewModel).MesaSelected.CuentaContable = CStr(pobjItem.IdItem)
                    'CType(Me.DataContext, MesasViewModel).CuentascontablesClasesSelected.CuentaContable = pobjItem.Nombre
                    CType(Me.DataContext, MesasViewModel).CuentascontablesClasesSelected.CuentaContable = CStr(pobjItem.IdItem)
                Case "centroscosto"
                    CType(Me.DataContext, MesasViewModel).MesaSelected.Ccostos = CStr(pobjItem.IdItem)
            End Select
        End If
    End Sub
End Class


