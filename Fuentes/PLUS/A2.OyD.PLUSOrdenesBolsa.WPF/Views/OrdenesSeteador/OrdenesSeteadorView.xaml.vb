Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesBolsa
Imports System.Web
Imports System.Windows.Data

Partial Public Class OrdenesSeteadorView
    Inherits UserControl


#Region "Variables"

    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False
    Private WithEvents mobjVM As OrdenSeteadorViewModel

#End Region

#Region "Inicializaciones"

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            InitializeComponent()

            mobjVM = Me.Resources("VM")

            mlogInicializado = True

            mobjVM = Me.LayoutRoot.DataContext

            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.grdGridForma.Width = Application.Current.MainWindow.ActualWidth * 0.97
            mlogErrorInicializando = True

        Catch ex As Exception
            mlogErrorInicializando = False
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de seteador", Me.Name, "", "OrdenesSeteador_New", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub OrdenesSeteador_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            mobjVM.visNavegando = "Collapsed"
            'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla
            'Inicia el timer 
            If Not IsNothing(mobjVM) Then
                mobjVM.ReiniciaTimer()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de seteador", Me.Name, "", "OrdenesSeteador_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub
    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.grdGridForma.Width = Application.Current.MainWindow.ActualWidth * 0.97
    End Sub

#End Region

    Private Sub CntxMenu_ItemClick_1(sender As Object, e As RoutedEventArgs)
        mobjVM.AccionElegida(CType(CType(sender, Button).Tag, Short))
    End Sub

    Private Sub dg_MouseRightButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles dg.MouseRightButtonDown
        e.Handled = True

        Dim s = CType(e.OriginalSource, FrameworkElement)
        If (TypeOf s Is TextBlock) Or (TypeOf s Is Button) Then
            Dim parentRow = s.ParentOfType(Of Telerik.Windows.Controls.GridView.GridViewRow)()
            If (Not IsNothing(parentRow)) Then
                parentRow.IsSelected = True
            End If
        End If

    End Sub

    Private Sub dg_MouseRightButtonUp(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles dg.MouseRightButtonUp
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó error al seleccionar fila al abrir el menú contextual.",
                                       Me.ToString(), "dg_MouseRightButtonUp", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        mobjVM.pararTemporizador()
        ' mobjVM.CerrarVisor()
    End Sub

    Private Sub dg_RowLoaded(sender As Object, e As GridView.RowLoadedEventArgs)
        Try
            If e.Row.Item IsNot Nothing Then
                'If Not TypeOf e.Row Is C1.Silverlight.DataGrid.Summaries.DataGridGroupWithSummaryRow Then
                    Dim ordOrden As OrdenSeteador = DirectCast(e.Row.Item, OrdenSeteador)
                    Dim clr As Color
                    If ordOrden.strEstadoLEO = "L" And ordOrden.strEstadoVisor = "PE" Then
                        clr = Program.colorFromHex(Program.par_color_orden_pendiente_SAE)
                    Else
                        If ordOrden.Cruza.Length > 1 Then
                            clr = Program.colorFromHex(Program.par_color_cruzada)
                        Else
                            clr = Program.colorFromHex(Program.par_color_normal)
                        End If
                    End If
                    e.Row.Background = New SolidColorBrush(clr)
                'End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó error al asignar color a la fila con estado LEO lanzada y pendiente.", _
                                       Me.ToString(), "dg_LoadedRowPresenter", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


End Class