Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: CuentasDecevalPorAgrupadorView.xaml.vb
'Generado el : 04/29/2011 16:14:31
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class CuentasDecevalPorAgrupadorView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New CuentasDecevalPorAgrupadorViewModel
InitializeComponent()
    End Sub

    Private Sub CuentasDecevalPorAgrupador_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        'cm.DF = df
        CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).NombreColeccionDetalle = Me.ToString
    End Sub

    'Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    df.CancelEdit()
        ''    If Not IsNothing(df.ValidationSummary) Then
    '        df.ValidationSummary.DataContext = Nothing
    '    End If
    'End Sub

    'Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
    '    df.ValidateItem()
    '    If df.ValidationSummary.HasErrors Then
    '        df.CancelEdit()
    '    Else
    '        df.CommitEdit()
    '    End If
    'End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaComitente(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "comitente"
                    CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel)._mlogBuscarCliente = False
                    CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).CuentasDecevalPorAgrupadoSelected.Comitente = pobjItem.IdComitente
                    CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).CuentasDecevalPorAgrupadoSelected.NomCliente = pobjItem.Nombre
                    CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).CuentasDecevalPorAgrupadoSelected.NroDocumento = pobjItem.NroDocumento
                    CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).CuentasDecevalPorAgrupadoSelected.TipoIdComitente = pobjItem.CodTipoIdentificacion
                    CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).Llamarbeneficiario()
                    CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel)._mlogBuscarCliente = True
                Case "idcomitente"
                    'CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).CuentasDecevalPorAgrupadoSelected.Comitente = pobjItem.IdComitente
                    CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).cb.Comitente = pobjItem.IdComitente
                    'CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).CuentasDecevalPorAgrupadoSelected.Comitente = pobjItem.Nombre
                    'CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).Llamarbeneficiario()
                    'CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).EspecieSelected.IDClase = CType(pobjItem.IdItem, Integer)
                    'CType(Me.DataContext, CuentasDecevalPorAgrupadorViewModel).ClaseClaseSelected.Clase = pobjItem.Nombre
            End Select
        End If
    End Sub
    

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not (e.Key > 95 And e.Key < 106) And Not (e.Key = 9) And Not (e.Key = 13) Then
            e.Handled = True
        End If
    End Sub

End Class


