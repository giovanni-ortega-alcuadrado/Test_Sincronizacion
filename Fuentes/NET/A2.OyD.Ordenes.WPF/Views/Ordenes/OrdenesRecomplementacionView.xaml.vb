
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls

''' <summary>
''' Vista de la forma de recomplementación
''' </summary>
''' <remarks></remarks>

Partial Public Class OrdenesRecomplementacionView
    Inherits UserControl


    ''' <summary>
    ''' Constructor de la vista
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me.DataContext = New OrdenesRecomplementacionViewModel
InitializeComponent()
    End Sub

    ''' <summary>
    ''' Se encarga de colocar la fila de los totales con fondo azul y letra blanca
    ''' </summary>
    ''' <param name="sender">Datagrid</param>
    ''' <param name="e">argumentos del evento</param>
    ''' <remarks></remarks>
        'Private Sub C1DataGrid_LoadedCellPresenter(sender As Object, e As C1.Silverlight.DataGrid.DataGridCellEventArgs)
    '    Dim objcell As C1.Silverlight.DataGrid.DataGridCell = e.Cell

    '    If (objcell.Presenter IsNot Nothing) Then

    '        objcell.Presenter.HorizontalContentAlignment = Windows.HorizontalAlignment.Right


    '        If objcell.Row.Index = DirectCast(e.Cell.DataGrid.ItemsSource, List(Of Object)).Count - 1 Then

    '            objcell.Presenter.Background = New SolidColorBrush(Color.FromArgb(255, 79, 129, 189))
    '            objcell.Presenter.Foreground = New SolidColorBrush(Colors.White)

    '        Else

    '            objcell.Presenter.Background = New SolidColorBrush(Colors.Transparent)
    '            objcell.Presenter.Foreground = New SolidColorBrush(Colors.Black)

    '        End If


    '    End If


    'End Sub

    Private Sub ucbtnCargar_SubirArchivo(sender As Object, e As RoutedEventArgs)
        Try
            CType(Me.DataContext, OrdenesRecomplementacionViewModel).CargarArchivo()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema mientras se cargaba el archivo", _
                                 Me.ToString(), "ucbtnCargar_SubirArchivo", Program.TituloSistema, Program.Maquina, ex)
        End Try
        
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, OrdenesRecomplementacionViewModel).Consultar()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, OrdenesRecomplementacionViewModel).Calcular()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, OrdenesRecomplementacionViewModel).Procesar()
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, OrdenesRecomplementacionViewModel).NuevoProceso()
    End Sub

Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog) Then
                If Not IsNothing(objDialog.FileName) Then
                    CType(Me.DataContext, OrdenesRecomplementacionViewModel).strNombreArchivo = Path.GetFileName(objDialog.FileName)
                    CType(Me.DataContext, OrdenesRecomplementacionViewModel).strRutaArchivo = Path.GetFileName(objDialog.FileName)
                    CType(Me.DataContext, OrdenesRecomplementacionViewModel).ProcesarArchivo()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirArchivoTesoreria_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
