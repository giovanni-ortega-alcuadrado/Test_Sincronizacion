Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports C1.Silverlight
Imports C1.Silverlight.RichTextBox
Imports C1.Silverlight.RichTextBox.Documents


Partial Public Class PlantillaProcesoView
    Inherits UserControl

    Private WithEvents objVMPlantillabanco As PlantillaProcesoViewModel

    Public Sub New()
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Me.DataContext = New PlantillaProcesoViewModel
InitializeComponent()
        objVMPlantillabanco = Me.LayoutRoot.DataContext
    End Sub

    Private Sub PlantillaProcesoView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df

        If objVMPlantillabanco Is Nothing Then
            objVMPlantillabanco = CType(Me.DataContext, PlantillaProcesoViewModel)

        End If

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

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem AS OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            objVMPlantillabanco.PlantillaBancoSelected.strNombre = pobjItem.Descripcion
            objVMPlantillabanco.PlantillaBancoSelected.IdBanco = pobjItem.IdItem
            objVMPlantillabanco.idBanco = pobjItem.IdItem
        End If
    End Sub
End Class
