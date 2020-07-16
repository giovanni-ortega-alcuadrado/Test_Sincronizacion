
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl



Partial Public Class ImportarOrdenesLEO
    Inherits UserControl

    Dim objVMA2Utils As A2UtilsViewModel

    Property checkBoxes As New List(Of CheckBox)

    Public Sub New()
        Try
            Me.DataContext = New ImportarOrdenesLEOViewModel
            InitializeComponent()
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
            Me.grdGridForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.grdGridForma.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.96

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de importación de órdenes LEO", Me.Name, "", "ImportarOrdenesLEO_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.grdGridForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Me.grdGridForma.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.96
    End Sub

    Private Sub chkAll_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim chk As CheckBox = CType(sender, CheckBox)
            Dim check As Boolean = chk.IsChecked.Value

            CType(Me.DataContext, ImportarOrdenesLEOViewModel).SeleccionarTodasLEO(check)

        Catch ex As Exception
            mostrarMensaje("Error chkAll_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try

    End Sub

    Private Sub chkBoxID_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim cb As CheckBox = GetCheckBoxWithParent(Me.dg, GetType(CheckBox), "chkAll")
            If Not IsNothing(cb) Then
                cb.IsChecked = False
            End If
        Catch ex As Exception
            mostrarMensaje("Error chkBoxID_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Function GetCheckBoxWithParent(parent As UIElement, targetType As Type, chkName As String) As CheckBox
        Try
            If parent.GetType Is targetType AndAlso DirectCast(parent, CheckBox).Name = chkName Then
                Return DirectCast(parent, CheckBox)
            End If
            Dim result As CheckBox = Nothing
            Dim count As Integer = VisualTreeHelper.GetChildrenCount(parent)
            For i As Integer = 0 To count - 1
                Dim child As UIElement = DirectCast(VisualTreeHelper.GetChild(parent, i), UIElement)
                If GetCheckBoxWithParent(child, targetType, chkName) IsNot Nothing Then
                    result = GetCheckBoxWithParent(child, targetType, chkName)
                    Exit For
                End If
            Next
            Return result
        Catch ex As Exception
            mostrarMensaje("Error GetCheckBoxWithParent.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
            Return Nothing
        End Try
    End Function

    Private Sub cargarArchivoLEO(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)

            If Not IsNothing(objDialog.FileName) Then
                If Path.GetExtension(objDialog.FileName).Equals(".csv") Or Path.GetExtension(objDialog.FileName).Equals(".xls") Or Path.GetExtension(objDialog.FileName).Equals(".xlsx") Then
                    Dim strRutaArchivo As String = Path.GetFileName(objDialog.FileName)

                    Dim viewImportacion As New ImportarArchivosLeo(CType(Me.DataContext, ImportarOrdenesLEOViewModel), strRutaArchivo)
                    Me.txtRuta.Text = strRutaArchivo
                    Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                    viewImportacion.ShowDialog()
                Else
                    mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "cargarArchivoLEO", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ImportarOrdenesLEO_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            CType(Me.DataContext, ImportarOrdenesLEOViewModel).CargarObjetoClasificacion()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "Ordenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class