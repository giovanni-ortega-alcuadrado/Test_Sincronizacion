Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports Microsoft.VisualBasic.CompilerServices


Partial Public Class CodigosTransaccionView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New CodigosTransaccionViewModel
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
    

    Private Sub TextBox_KeyDown_1(sender As Object, e As KeyEventArgs)
        'JBT valida que solo deje entrar numeros,guiones y espacios en blanco ya que son numeros de cuentas JBT20140304
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) And Not e.Key = 189 And Not e.Key = 32 And Not e.Key = 109 Then
            e.Handled = True
        End If
    End Sub

End Class


