Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: LiquidacionesView.xaml.vb
'Generado el : 05/30/2011 09:18:58
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class LiquidacionesView_MI
    Inherits UserControl
    Dim p As Boolean

    Public Sub New()
        Me.DataContext = New LiquidacionesViewModel_MI
InitializeComponent()

    End Sub

    Private Sub Liquidaciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, LiquidacionesViewModel_MI).NombreView = Me.ToString
        CType(Me.DataContext, LiquidacionesViewModel_MI).VmLiquidaciones = Me
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

    Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel_MI).llevarvalores()
    End Sub
    Private Sub df_LostFocusCA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel_MI).llevarvalores()
    End Sub
    Private Sub df_LostFocusFA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel_MI).llevarvalores()
    End Sub
    Private Sub Validaliq(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        If CType(Me.DataContext, LiquidacionesViewModel_MI).estadoregistro = False Then
            Exit Sub
        End If
        CType(Me.DataContext, LiquidacionesViewModel_MI).validaliq()
    End Sub
    'Private Sub df_LostFocusorden(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
    '    CType(Me.DataContext, LiquidacionesViewModel_MI).validaorden()
    'End Sub
    'Private Sub Diasvencimientolostfocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
    '    If Not IsNothing(CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.Plazo) And Not IsNothing(CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.DiasVencimiento) _
    '        And CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.DiasVencimiento > CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.Plazo Then
    '        CType(Me.DataContext, LiquidacionesViewModel).mostrarmensaje()
    '        Exit Sub
    '    End If

    '    'If Not IsNothing(CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.DiasVencimiento) Then
    '    '    CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.Vencimiento = Now
    '    'End If

    'End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesViewModel_MI).ReceptoresOrdenesSelected.IDReceptor = pobjItem.IdItem
            CType(Me.DataContext, LiquidacionesViewModel_MI).ReceptoresOrdenesSelected.Nombre = pobjItem.Nombre
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaPLAZA(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesViewModel_MI).LiquidacioneSelected.IDComisionistaOtraPlaza = pobjItem.IdItem
        End If
    End Sub
    Private Sub BuscadorGenerico_finalizoBusquedaPLAZALocal(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesViewModel_MI).LiquidacioneSelected.IDComisionistaLocal = pobjItem.IdItem
        End If
    End Sub
    Private Sub Buscar_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "Nombre"
                    CType(Me.DataContext, LiquidacionesViewModel_MI).cb.IDComitente = pobjItem.IdComitente
                Case "NombreOrdenante"
                    CType(Me.DataContext, LiquidacionesViewModel_MI).cb.IDOrdenante = pobjItem.IdComitente
            End Select

        End If
    End Sub

    Private Sub dgReceptoresOrdenes_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        e.Handled = True
    End Sub

    
    Private Sub btnAplazar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel_MI).Aplazamientos()
    End Sub

Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel_MI).Duplicar()
    End Sub

Private Sub LiquidacionesBuscar_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        DirectCast(sender, System.Windows.Controls.TextBox).Focus()
    End Sub



End Class


