Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class  ComisionEspeciesView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ComisionEspeciesViewModel
InitializeComponent()
    End Sub

    Private Sub ComisionEspecies_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
		CType(Me.DataContext,ComisionEspeciesViewModel).NombreView = Me.ToString
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

    Private Sub Buscador_finalizoBusquedaespecies(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, ComisionEspeciesViewModel).ComisionEspecieSelected.IDEspecie = pobjItem.Nemotecnico
            CType(Me.DataContext, ComisionEspeciesViewModel).ComisionEspecieSelected.NombreEspecie = pobjItem.Especie & "(" & pobjItem.Nemotecnico & ")"
        End If
    End Sub

    Private Sub Buscador_finalizoBusquedaespeciesBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, ComisionEspeciesViewModel).cb.IDEspecie = pobjItem.Nemotecnico
            CType(Me.DataContext, ComisionEspeciesViewModel).cb.NombreEspecie = pobjItem.Especie & "(" & pobjItem.Nemotecnico & ")"
        End If
    End Sub
	
End Class


