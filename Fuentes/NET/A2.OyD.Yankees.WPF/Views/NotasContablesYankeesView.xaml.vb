Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2ControlMenu
Imports A2Utilidades.Mensajes
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: NotasContablesYankeesView.xaml.vb
'Generado el : 06/21/2013
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class NotasContablesYankeesView
    Inherits UserControl
    Dim p As Boolean
    Private mobjVM As NotasYankeesViewModel
    Public Sub New()
        Me.DataContext = New NotasYankeesViewModel
InitializeComponent()
    End Sub
    Private Sub Liquidaciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        'cm.DF = df
        CType(Me.DataContext, NotasYankeesViewModel).NombreView = Me.ToString
        mobjVM = CType(Me.DataContext, NotasYankeesViewModel)
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

    Private Sub NotasContablesYankeesView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        DirectCast(sender, System.Windows.Controls.TextBox).Focus()
    End Sub

    Private Sub Buscar_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, NotasYankeesViewModel)._mlogBuscarCliente = False
            CType(Me.DataContext, NotasYankeesViewModel).SelectedNotasYankeesDetalle.strIdComitente = pobjComitente.IdComitente
            CType(Me.DataContext, NotasYankeesViewModel).SelectedNotasYankeesDetalle.strNombre = pobjComitente.Nombre
            CType(Me.DataContext, NotasYankeesViewModel)._mlogBuscarCliente = True
        End If
    End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            If pobjItem.EtiquetaCodItem = "Cuenta Contable" Then
                CType(Me.DataContext, NotasYankeesViewModel)._mlogBuscarCuentaContable = False
                CType(Me.DataContext, NotasYankeesViewModel).SelectedNotasYankeesDetalle.strIDCuentaContable = pobjItem.IdItem
                CType(Me.DataContext, NotasYankeesViewModel)._mlogBuscarCuentaContable = True
            End If
        End If
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.ImprimirReporteYanKees("imprimirNotaYankees")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para lanzar la impresión de la nota ", Me.Name, "Button_Click_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class


