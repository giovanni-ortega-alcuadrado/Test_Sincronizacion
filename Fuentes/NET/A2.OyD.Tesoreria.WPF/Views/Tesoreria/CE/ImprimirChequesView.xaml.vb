Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes


Partial Public Class ImprimirChequesView
    Inherits UserControl

#Region "Variables"
    Private mlogInicializado As Boolean = False
    Dim objA2VM As A2UtilsViewModel
    Dim logcargainicial As Boolean = True
    Private mobjVM As ImprimirChequesViewModel
#End Region

    Public Sub New()
        Me.DataContext = New ImprimirChequesViewModel
InitializeComponent()
    End Sub

#Region "Eventos"

    Private Sub ImprimirCheques_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If logcargainicial Then
            logcargainicial = False
        Else
            mobjVM = CType(Me.DataContext, ImprimirChequesViewModel)
            mobjVM.BuscarChequesxImprimir()
        End If
    End Sub


    Private Sub btnBuscarChequesxImprimir_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, ImprimirChequesViewModel).ImprimirCheques()
    End Sub

#End Region

End Class
