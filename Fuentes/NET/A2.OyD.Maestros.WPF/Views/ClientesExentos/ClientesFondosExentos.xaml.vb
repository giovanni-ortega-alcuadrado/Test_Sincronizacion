Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Partial Public Class ClientesFondosExentos
    Dim dcconsulta As MaestrosDomainContext
    Dim vm As New ClientesFondosPensionesViewModel
    Dim view As New ViewClientes_Exentos_Consultar
    Dim pstrfiltro As String = ""
    Public nombre As String
    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcconsulta = New MaestrosDomainContext()
        Else
            dcconsulta = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If
        InitializeComponent()
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub
    Private Sub ClientesFondosExentos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        dcconsulta.Load(dcconsulta.GetViewClientes_Exentos_ConsultarsQuery(" ", Program.Usuario, Program.HashConexion), AddressOf terminotraerclientes, " ")
    End Sub

    Private Sub terminotraerclientes()
        vm.Listatabla = dcconsulta.ViewClientes_Exentos_Consultars.ToList
        If vm.ListatablaPaged IsNot Nothing Then
            vm.ListatablaPaged.Refresh()
        End If
        Me.LayoutRoot.DataContext = vm
    End Sub
    Private Sub dg_SelectionChanged(ByVal sender As Object, ByVal e As SelectionChangeEventArgs) Handles dg.SelectionChanged
        If dg.SelectedItem IsNot Nothing Then
            view = dg.SelectedItem
            txtFiltro.Text = view.Codigo
            nombre = view.Nombre
        End If
    End Sub

    Private Sub btnFiltrar_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnFiltrar.Click
        dcconsulta.ViewClientes_Exentos_Consultars.Clear()
        If pstrfiltro.Length >= 0 Then
            pstrfiltro = txtFiltroUsuario.Text
            dcconsulta.Load(dcconsulta.GetViewClientes_Exentos_ConsultarsQuery(pstrfiltro, Program.Usuario, Program.HashConexion), AddressOf terminotraerclientes, " ")
        Else
            dcconsulta.Load(dcconsulta.GetViewClientes_Exentos_ConsultarsQuery(" ", Program.Usuario, Program.HashConexion), AddressOf terminotraerclientes, " ")
        End If
    End Sub

    'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

End Class
