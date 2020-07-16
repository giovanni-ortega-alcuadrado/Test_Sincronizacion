Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel

''' <summary>
''' Ventana para ingresar los valores de distribución del bloqueo
''' </summary>
''' <remarks></remarks>
Partial Public Class WDistribuirBloqueoView
    Inherits Window

#Region "Constructores"

    ''' Constructor único recibir y mostrar la información en pantalla

    Public Sub New()
        InitializeComponent()
    End Sub
#End Region

#Region "Propiedades"

    Public Property dblValorLegal As Double
    Public Property dbValorInterno As Double


#End Region

#Region "Eventos"
    ''' <summary>
    ''' Confirmar la operación y cerrar la ventana
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Aceptar_Click(sender As Object, e As RoutedEventArgs)
        dblValorLegal = nbxBloqueoSag.Value
        dbValorInterno = nbxBloqueoInterno.Value
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

    ''' <summary>
    ''' Mover todo el valor
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Mover_Click(sender As Object, e As RoutedEventArgs)
        If nbxBloqueoSag.IsEnabled = True Then
            nbxBloqueoSag.Value = nbxBloqueoInterno.Value
        Else
            nbxBloqueoInterno.Value = nbxBloqueoSag.Value
        End If
    End Sub

#End Region
End Class
