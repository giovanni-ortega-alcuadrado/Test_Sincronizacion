Imports Telerik.Windows.Controls
' Desarrollo       : TrasladarSaldoView
' Creado por       : Juan Carlos Soto Cruz.
' Fecha            : Agosto 17/2011 4:00 p.m  

#Region "Librerias"

Imports A2.OYD.OYDServer.RIA.Web

#End Region

#Region "Clases"


Partial Public Class TrasladarSaldoView
    Inherits UserControl

#Region "Declaraciones"

    Dim vm As TrasladarSaldoViewModel

#End Region

#Region "Inicializacion"

    Public Sub New()
        Me.DataContext = New TrasladarSaldoViewModel
InitializeComponent()
    End Sub

    Private Sub TrasladarSaldoView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, TrasladarSaldoViewModel)
    End Sub

#End Region

#Region "Eventos"

    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        vm.Consultar()
    End Sub

    Private Sub btnTrasladar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        vm.trasladaSaldo()
    End Sub

    

#End Region

#Region "Buscadores"
    Private Sub Buscar_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, TrasladarSaldoViewModel).cambiocomitente = True
            CType(Me.DataContext, TrasladarSaldoViewModel).trasladarSaldo.IdComitente = pobjComitente.CodigoOYD
            CType(Me.DataContext, TrasladarSaldoViewModel).trasladarSaldo.Nombre = pobjComitente.Nombre
            CType(Me.DataContext, TrasladarSaldoViewModel).trasladarSaldo.valorATrasladar = String.Empty
            CType(Me.DataContext, TrasladarSaldoViewModel).trasladarSaldo.valorSaldoCredito = String.Empty
            CType(Me.DataContext, TrasladarSaldoViewModel).trasladarSaldo.valorSaldoDebito = String.Empty
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaCtasContables(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, TrasladarSaldoViewModel).trasladarSaldo.IDCuentaContable = pobjItem.IdItem
        End If
    End Sub
#End Region

End Class

#End Region
