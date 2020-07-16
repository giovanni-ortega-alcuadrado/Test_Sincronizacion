Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel

''' <summary>
''' Ventana para ver el resumen y confirmar la operación realizada
''' </summary>
''' <remarks></remarks>
Partial Public Class WGenerarReporteView
    Inherits Window

#Region "Constructores"

    ''' Constructor único recibir y mostrar la información en pantalla

    Public Sub New()
        InitializeComponent()
        rdResumido.IsChecked = True
        rdActual.IsChecked = True
    End Sub
#End Region

#Region "Propiedades"

    Public Property bitTiporesumido As Boolean
    Public Property bitClienteActual As Boolean

#End Region

#Region "Eventos"
    ''' <summary>
    ''' Confirmar la operación y cerrar la ventana
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Aceptar_Click(sender As Object, e As RoutedEventArgs)
        bitTiporesumido = rdResumido.IsChecked
        bitClienteActual = rdActual.IsChecked
        DialogResult = True
    End Sub

    ''' <summary>
    ''' Cancelar la operación y cerrar la ventana
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Cancelar_Click(sender As Object, e As RoutedEventArgs)
        DialogResult = False
    End Sub
#End Region
End Class
