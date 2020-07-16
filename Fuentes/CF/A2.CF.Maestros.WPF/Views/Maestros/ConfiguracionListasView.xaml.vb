Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ListasView.xaml.vb
'Generado el : 01/27/2011 09:30:33
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ConfiguracionListasView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ConfiguracionListasViewModel
InitializeComponent()
    End Sub

    Private Sub Listas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        inicializar()
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
        End If
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    End Sub

    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        
    End Sub
    

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub
    Private Sub txtRetornoEntero_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Try
            If Not (e.Key > 47 And e.Key < 58) And Not (e.Key > 95 And e.Key < 106) And Not (e.Key = 9) And Not (e.Key = 13) Then
                e.Handled = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema con el control de número de orden en la búsqueda", Me.Name, "txtRetornoEntero_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


End Class


