Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: Liquidaciones_MIView.xaml.vb
'Generado el : 06/05/2012 17:17:01
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ReliquidacionOperaView
    Inherits UserControl
    Private WithEvents buscadorClientes As A2ComunesControl.BuscadorClienteLista

    Public Sub New()
        Me.DataContext = New Liquidaciones_MIViewModel
InitializeComponent()
    End Sub

    Private Sub Consultar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, Liquidaciones_MIViewModel).Filtrar()
    End Sub

    Private Sub Buscar_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, Liquidaciones_MIViewModel).ClaseReliquidacionesSelected.lngIDComitente = pobjComitente.IdComitente
            CType(Me.DataContext, Liquidaciones_MIViewModel).ClaseReliquidacionesSelected.Nombre = pobjComitente.Nombre
        End If

    End Sub

    Private Sub Aceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, Liquidaciones_MIViewModel).ActualizarRegistro()
    End Sub

    Private Sub Chequear_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, Liquidaciones_MIViewModel).Chequear()
    End Sub

End Class


