Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports Microsoft.VisualBasic.CompilerServices


Partial Public Class CodigosAjustesView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New CodigosAjustesViewModel
InitializeComponent()
    End Sub

    Private Sub Consecutivos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
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
    

    Private Sub ctlConsecutivosVsUsuarios_itemAsignado(pstrIdItem As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrIdItem = "ConceptoXConsecutivos" Then
                    CType(Me.DataContext, CodigosAjustesViewModel).CodigosAjustesSelected.DescripcionConcepto = pobjItem.Descripcion
                    CType(Me.DataContext, CodigosAjustesViewModel).CodigosAjustesSelected.IdConcepto = pobjItem.IdItem

                End If
            End If

        Catch ex As Exception
            mostrarMensaje("Error ctlConsecutivosVsUsuarios_itemAsignado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

End Class


