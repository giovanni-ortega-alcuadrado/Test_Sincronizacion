Imports Telerik.Windows.Controls

Imports System.ComponentModel
Imports System.Linq

Imports A2Utilidades.Mensajes


Partial Public Class DetalleArchivosDeceval
    Inherits Window
    Dim objVMDetalleArchivos As ViewModelDetalleArchivosDeceval
    'modificar

    Dim intIDArchivo As Integer = 0
    Dim strNombreArchivo As String = String.Empty
    Dim strUsuario As String = String.Empty
    Dim logEjecucionAutomatica As Boolean = False
    '
    Public Sub New(ByVal pintintIDArchivo As Integer, ByVal pstrNombreArchivo As String, ByVal pstrUsuario As String, ByVal plogEjecucionAutomatica As Boolean)
        Try
            '
            intIDArchivo = pintintIDArchivo
            strNombreArchivo = pstrNombreArchivo
            strUsuario = pstrUsuario
            logEjecucionAutomatica = plogEjecucionAutomatica

            InitializeComponent()

            objVMDetalleArchivos = New ViewModelDetalleArchivosDeceval()

            Me.DataContext = objVMDetalleArchivos

            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            dg.Width = Application.Current.MainWindow.ActualWidth * 0.9
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la pantalla.", Me.Name, "DetalleArchivosDeceval_New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        dg.Width = Application.Current.MainWindow.ActualWidth * 0.9
    End Sub

    Private Sub DetalleArchivosDeceval_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Not IsNothing(objVMDetalleArchivos) Then
                objVMDetalleArchivos.intIDArchivo = intIDArchivo

                objVMDetalleArchivos.NombreArchivo = strNombreArchivo
                objVMDetalleArchivos.UsuarioArchivo = strUsuario
                objVMDetalleArchivos.GeneracionAutomatica = logEjecucionAutomatica

                objVMDetalleArchivos.Consultar()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la pantalla.", Me.Name, "DetalleInversionistasDeceval_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnRefrescar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMDetalleArchivos) Then
                objVMDetalleArchivos.Consultar()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los resultados de la importación.", Me.Name, "btnRefrescar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Cerrar_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

End Class
