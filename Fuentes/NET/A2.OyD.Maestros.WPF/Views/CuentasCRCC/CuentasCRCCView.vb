Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class CuentasCRCCView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New CuentasCRCCViewModel
InitializeComponent()
    End Sub

    Private Sub CuentasCRCC_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
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

    Private Sub BuscadorGenerico_finalizoBusquedaComitente(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "comitente"
                    CType(Me.DataContext, CuentasCRCCViewModel)._mlogBuscarCliente = False
                    CType(Me.DataContext, CuentasCRCCViewModel).CuentasCRCCSelected.IDComitente = pobjItem.IdComitente
                    CType(Me.DataContext, CuentasCRCCViewModel).CuentasCRCCSelected.NomCliente = pobjItem.Nombre
                    CType(Me.DataContext, CuentasCRCCViewModel).CuentasCRCCSelected.NroDocumento = pobjItem.NroDocumento
                    CType(Me.DataContext, CuentasCRCCViewModel).CuentasCRCCSelected.TipoIdComitente = pobjItem.CodTipoIdentificacion
                    CType(Me.DataContext, CuentasCRCCViewModel)._mlogBuscarCliente = True
                Case "idcomitente"
                    CType(Me.DataContext, CuentasCRCCViewModel).cb.IDComitente = pobjItem.IdComitente
            End Select
        End If
    End Sub


    Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
        df.ValidateItem()
    End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not (e.Key > 95 And e.Key < 106) And Not (e.Key = 9) And Not (e.Key = 13) Then
            e.Handled = True
        End If
    End Sub

End Class


