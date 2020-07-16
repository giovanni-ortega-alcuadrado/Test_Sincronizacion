Imports Telerik.Windows.Controls
Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports C1.Silverlight.DataGrid
Imports C1.Silverlight.DataGrid.Summaries

'Codigo Desarrollado por: Santiago Alexander Vergara Orrego
'Archivo: Partial Public Class LiquidacionesDividendosTTVView.vb
'Junio 07/2013
'Propiedad de Alcuadrado S.A. 2013

Partial Public Class LiquidacionesDividendosTTVView
    Inherits UserControl


#Region "Inicializaciones"


    Public Sub New()
        Me.DataContext = New LiquidacionesDividendosTTVViewModel
InitializeComponent()
    End Sub

#End Region

#Region "Eventos"

    Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesDividendosTTVViewModel).ConsultarLiquidacionesDividendos()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesDividendosTTVViewModel).ProcesarDatos()
    End Sub

#End Region

End Class