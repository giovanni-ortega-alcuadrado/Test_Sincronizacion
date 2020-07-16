Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: InhabilitadosView.xaml.vb
'Generado el : 03/15/2011 10:24:50
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  InhabilitadosView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New InhabilitadosViewModel
InitializeComponent()
    End Sub

    Private Sub Inhabilitados_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        'CType(Me.DataContext,InhabilitadosViewModel).NombreView = Me.ToString
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

    Private Sub cmbTipoDoc_changed(sender As Object, e As SelectionChangedEventArgs)
        Dim it As OYDUtilidades.ItemCombo = CType(sender.selecteditem(), A2.OYD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo)
        If Not IsNothing(CType(Me.DataContext, InhabilitadosViewModel).InhabilitadoSelected) Then
            If Not IsNothing(it) Then
                Dim tcl As tipoclasificacion = New tipoclasificacion() With {.ID = it.ID, .Descripcion = it.Descripcion}
                CType(Me.DataContext, InhabilitadosViewModel).InhabilitadoSelected.DescripcionIdentificacion = tcl.Descripcion
            Else
                CType(Me.DataContext, InhabilitadosViewModel).InhabilitadoSelected.DescripcionIdentificacion = String.Empty
            End If
        End If
    End Sub

End Class


