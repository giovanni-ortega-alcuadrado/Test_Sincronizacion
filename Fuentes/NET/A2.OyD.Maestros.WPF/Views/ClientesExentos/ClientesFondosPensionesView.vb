Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ClientesFondosPensionesView.xaml.vb
'Generado el : 03/23/2011 13:41:42
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ClientesFondosPensionesView
    Inherits UserControl
    
    Dim vm As ClientesFondosPensionesViewModel

    Public Sub New()
        Me.DataContext = New ClientesFondosPensionesViewModel
InitializeComponent()
    End Sub

    Private Sub ClientesFondosPensiones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
        cm.GridViewRegistros = datapager1
        'cm.DF = df
        vm = CType(Me.DataContext, ClientesFondosPensionesViewModel)
    End Sub

    Private Sub Builder_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim cfe As New ClientesFondosExentos
        AddHandler cfe.Closed, AddressOf CerroVentana
        Program.Modal_OwnerMainWindowsPrincipal(cfe)
        cfe.ShowDialog()
    End Sub
    Private Sub Builder_ClickBuscar(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim cfe As New ClientesFondosExentos
        AddHandler cfe.Closed, AddressOf CerroVentanaBuscar
        Program.Modal_OwnerMainWindowsPrincipal(cfe)
        cfe.ShowDialog()
    End Sub
    
    Private Sub CerroVentana(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cfe = CType(sender, ClientesFondosExentos)
        If Not IsNothing(cfe.DialogResult) Then
            If cfe.DialogResult = True Then
                'Dim ce As New ViewClientes_Exento
                'ce.Comitente = cfe.txtFiltro.Text
                'ce.Nombre = cfe.nombre
                'vm.ViewClientesExentoSelected = ce
                vm.ViewClientesExentoSelected.Comitente = cfe.txtFiltro.Text
                vm.ViewClientesExentoSelected.Nombre = cfe.nombre
                vm.CambioViewClientesExentoSelected()
            End If
        End If
    End Sub
    Private Sub CerroVentanaBuscar(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cfe = CType(sender, ClientesFondosExentos)
        If cfe.DialogResult = True Then
            vm.cb.Comitente = cfe.txtFiltro.Text
            'vm.ViewClientesExentoSelected.Nombre = cfe.nombre
            'vm.CambioViewClientesExentoSelected()
        End If
    End Sub
End Class


