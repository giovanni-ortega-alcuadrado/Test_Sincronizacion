Imports Telerik.Windows.Controls

Imports System.ComponentModel
Imports System.Linq

Imports A2Utilidades.Mensajes


Partial Public Class DetalleInversionistasDeceval
    Inherits Window
    Dim objVMDetalleInversionista As ViewModelDetalleInversionistasDeceval
    Dim intIDInversionista As Integer = 0
    Dim strCodigoOYD As String = String.Empty

    Public Sub New(ByVal pintIDInversionista As Integer, ByVal pstrCodigoOYD As String)
        Try
            intIDInversionista = pintIDInversionista
            strCodigoOYD = pstrCodigoOYD

            InitializeComponent()

            objVMDetalleInversionista = New ViewModelDetalleInversionistasDeceval()

            Me.DataContext = objVMDetalleInversionista

            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            dg.Width = Application.Current.MainWindow.ActualWidth * 0.9
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la pantalla.", Me.Name, "DetalleInversionistasDeceval_New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        dg.Width = Application.Current.MainWindow.ActualWidth * 0.9
    End Sub

    Private Sub DetalleInversionistasDeceval_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Not IsNothing(objVMDetalleInversionista) Then
                objVMDetalleInversionista.IDInversionista = intIDInversionista
                objVMDetalleInversionista.IDComitente = strCodigoOYD

                objVMDetalleInversionista.Consultar()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la pantalla.", Me.Name, "DetalleInversionistasDeceval_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnRefrescar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMDetalleInversionista) Then
                objVMDetalleInversionista.Consultar()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los resultados de la importación.", Me.Name, "btnRefrescar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Cerrar_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

End Class
