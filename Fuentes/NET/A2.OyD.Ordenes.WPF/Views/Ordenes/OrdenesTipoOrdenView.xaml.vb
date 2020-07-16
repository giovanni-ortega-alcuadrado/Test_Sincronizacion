Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class OrdenesTipoOrdenView
    Inherits Window

    Public mlogError As Boolean = False

    Public Property TipoOrden As String = String.Empty
    Public Property ClaseOrden As OrdenesViewModel.ClasesOrden
    Private Property EsCompra As System.Nullable(Of Boolean) = Nothing
    Private Property ListaCombos As String = String.Empty

    Public Sub New()
        InitializeComponent()

        Try
            Dim objItems As New List(Of OYDUtilidades.ItemCombo)
            objItems.Add(New OYDUtilidades.ItemCombo With {.Categoria = "TIPO", .Descripcion = "Compra", .ID = "C"})
            objItems.Add(New OYDUtilidades.ItemCombo With {.Categoria = "TIPO", .Descripcion = "Venta", .ID = "V"})

            Me.lstTipoOrden.ItemsSource = objItems
        Catch ex As Exception
            mlogError = True
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar la selección del tipo de orden", Me.Name, "cmdAceptar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub New(ByVal pintClaseOrden As OrdenesViewModel.ClasesOrden, ByVal pstrListaCombos As String)
        InitializeComponent()
        Me.ClaseOrden = pintClaseOrden
        Me.ListaCombos = pstrListaCombos
        inicializar()
    End Sub

    Public Sub New(ByVal pintClaseOrden As OrdenesViewModel.ClasesOrden, ByVal plogComprar As Boolean, ByVal pstrListaCombos As String)
        InitializeComponent()
        Me.ClaseOrden = pintClaseOrden
        Me.EsCompra = plogComprar
        Me.ListaCombos = pstrListaCombos
        inicializar()
    End Sub


    Private Sub inicializar()
        Dim objItems As List(Of OYDUtilidades.ItemCombo)
        If Application.Current.Resources.Contains(Me.ListaCombos) Then
            objItems = CType(Application.Current.Resources(Me.ListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("O_TIPO_OPERACION")
            If Me.EsCompra Is Nothing Then
                Me.lstTipoOrden.ItemsSource = objItems
            ElseIf Me.EsCompra Then
                Me.lstTipoOrden.ItemsSource = (From obj In objItems Where obj.ID = OrdenesViewModel.TiposOrden.C.ToString() Or obj.ID = OrdenesViewModel.TiposOrden.R.ToString() Select obj)
            Else
                Me.lstTipoOrden.ItemsSource = (From obj In objItems Where obj.ID = OrdenesViewModel.TiposOrden.V.ToString() Or obj.ID = OrdenesViewModel.TiposOrden.S.ToString() Select obj)
            End If
        End If
    End Sub

    Private Sub cmdAceptar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles cmdAceptar.Click
        seleccionarTipo()
    End Sub

    Private Sub OrdenesTipoOrdenView_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        If Me.lstTipoOrden.SelectedIndex < 0 And mlogError = False Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el tipo de la orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            e.Cancel = True
        End If
    End Sub

    Private Sub lstTipoOrden_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles lstTipoOrden.SelectionChanged
        seleccionarTipo()
    End Sub

    Private Sub seleccionarTipo()
        Try
            If Me.lstTipoOrden.SelectedIndex >= 0 Then
                Me.TipoOrden = Me.lstTipoOrden.SelectedValue
            End If

            Me.DialogResult = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el tipo de orden", Me.Name, "cmdAceptar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub
End Class
