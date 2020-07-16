Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class ModIdentiClientesView
    Inherits UserControl
    'Dim vm As New ModIdentiClientesViewModel

    Public Sub New()
        Me.DataContext = New ModIdentiClientesViewModel
InitializeComponent()
    End Sub

    Private Sub ModIdentiClientes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        'vm = CType(Me.DataContext, ModIdentiClientesViewModel)
        'CType(Me.DataContext, ModIdentiClientesViewModel).NombreView = Me.ToString
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, ModIdentiClientesViewModel).UtilizandoBuscador = True
            CType(Me.DataContext, ModIdentiClientesViewModel).ClienteSelected.NumeroDocumentoActual = pobjItem.NroDocumento
            CType(Me.DataContext, ModIdentiClientesViewModel).Consultar = True
            CType(Me.DataContext, ModIdentiClientesViewModel).Modificar = False
            CType(Me.DataContext, ModIdentiClientesViewModel).UtilizandoBuscador = False
            'CType(Me.DataContext, ModIdentiClientesViewModel).CodigoCliente.strCodigo = pobjItem.IdComitente
            'CType(Me.DataContext, ModIdentiClientesViewModel).CodigoCliente.strNombre = pobjItem.Nombre
            'CType(Me.DataContext, CustodiaViewModel).CustodiSelected.Direccion = pobjItem.DireccionEnvio
            'CType(Me.DataContext, CustodiaViewModel).CustodiSelected.Telefono1 = pobjItem.Telefono
            'CType(Me.DataContext, CustodiaViewModel).CustodiSelected.NroDocumento = pobjItem.NroDocumento
            'CType(Me.DataContext, CustodiaViewModel).CustodiSelected.TipoIdentificacion = pobjItem.CodTipoIdentificacion
        End If
    End Sub

    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ModIdentiClientesViewModel).ConsultarIdentificacionCliente()
    End Sub

    Private Sub btnModificar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ModIdentiClientesViewModel).ModificarIdentificacionCliente()
    End Sub

    Private Sub Buscador_Cliente_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Buscador_Cliente.Agrupamiento = "id_mostrardigito," + CType(Me.DataContext, ModIdentiClientesViewModel).ClienteSelected.TipoIdentificacionIdActual
    End Sub

End Class
