Imports A2.OYD.OYDServer.RIA.Web

''' <summary>
''' Ventana para ver el resumen y confirmar la operación realizada
''' </summary>
''' <remarks></remarks>
Partial Public Class WConfirmarView
    Inherits Window

    Public Const STR_OPERACION_DESBLOQUEAR As String = "desbloquear"

#Region "Constructores"

    ''' Constructor único recibir y mostrar la información en pantalla
 
    Public Sub New(logBloquear As Boolean, strNombreCliente As String, strCodigoCliente As String, lngliquidacion As Integer, _
                   numSaldoBloqueoDesbloqueo As Decimal, numPortafolioBloqueoDesbloqueo As Decimal)

        InitializeComponent()
        txtCliente.Text = strNombreCliente
        txtLiquidacion.Text = lngliquidacion
        txtSaldoBloquear.Text = String.Format(VisorDeGarantiasViewModel.STR_FORMATO_VALOR, numSaldoBloqueoDesbloqueo)
        txtPortafolioBloquear.Text = String.Format(VisorDeGarantiasViewModel.STR_FORMATO_VALOR, numPortafolioBloqueoDesbloqueo)
        txtTotalBloquear.Text = String.Format(VisorDeGarantiasViewModel.STR_FORMATO_VALOR, (numSaldoBloqueoDesbloqueo + numPortafolioBloqueoDesbloqueo))

        If Not logBloquear Then
            txtSaldoOperacion.Text = STR_OPERACION_DESBLOQUEAR
            txtTotalOperacion.Text = STR_OPERACION_DESBLOQUEAR
            txtPortafolioOperacion.Text = STR_OPERACION_DESBLOQUEAR
        End If

    End Sub
#End Region

#Region "Eventos"
    ''' <summary>
    ''' Confirmar la operación y cerrar la ventana
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Aceptar_Click(sender As Object, e As RoutedEventArgs)
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
