
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class GenerarAuditoriasdeRecaudoView
    Inherits UserControl
    Dim objVM As GenerarAuditoriasdeRecaudoViewModel
    Private mlogInicializar As Boolean = True

    Public Sub New()

        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.DataContext = New GenerarAuditoriasdeRecaudoViewModel
InitializeComponent()
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                
                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            objVM = CType(Me.DataContext, GenerarAuditoriasdeRecaudoViewModel)
            objVM.NombreView = Me.ToString

            objVM.inicializar()
        End If
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

    Private Sub btnSubirArchivo_CargarArchivo_Saldos(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog) Then
                If Not IsNothing(objDialog.FileName) Then
                    If Path.GetExtension(objDialog.FileName).Equals(".csv") Then
                        objVM.NombreArchivoSaldos = Path.GetFileName(objDialog.FileName)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirArchivo_CargarArchivo_Saldos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnGenerarCEyRCTotales_Click(sender As Object, e As RoutedEventArgs)
        objVM.ExportarTotales()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        objVM.Aceptar()
    End Sub

    Private Sub btnSubirArchivo_CargarArchivo_Totales(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog) Then
                If Not IsNothing(objDialog.FileName) Then
                    If Path.GetExtension(objDialog.FileName).Equals(".csv") Then
                        objVM.NombreArchivoTotales = Path.GetFileName(objDialog.FileName)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirArchivo_CargarArchivo_Totales", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub



End Class