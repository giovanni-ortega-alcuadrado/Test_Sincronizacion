Imports System.Windows.Markup
Imports System.Globalization
Imports C1.WPF.DataGrid

''' <summary>
''' Fila tipo grupo para mostrar los datos del visor de garantias y mostrar los valores necesarios en las celdas
''' </summary>
''' <remarks></remarks>
Public Class VisorDeGarantiasGroupRow
    Inherits DataGridGroupRow

#Region "Constructores"
    ''' <summary>
    ''' Constructor que inicializa la lista de summaries de la fila
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Summaries = New Dictionary(Of DataGridColumn, IComputeGroup)
    End Sub
#End Region

#Region "Propiedades"
    ''' <summary>
    ''' Summaries que determinan el valor de cada columna de la fila
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Summaries As Dictionary(Of DataGridColumn, IComputeGroup)
#End Region

#Region "Overrides"
    ''' <summary>
    ''' Dar el color de fondo
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub OnLoaded()
        MyBase.OnLoaded()
        If Presenter IsNot Nothing Then
            MyBase.Presenter.Background = New SolidColorBrush(Colors.White)
        End If
    End Sub
    ''' <summary>
    ''' Obtiene el tipo de dato de la fila
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function GetRowPresenterRecyclingKey() As Object
        Return GetType(VisorDeGarantiasGroupRow)
    End Function

    ''' <summary>
    ''' Indica si el valor de la columna requiere ser mostrado en la fila de grupo
    ''' </summary>
    ''' <param name="column"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function HasCellPresenter(ByVal column As DataGridColumn) As Boolean
        If Summaries IsNot Nothing AndAlso Summaries.ContainsKey(column) Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Obtener el tipo de contenedor que va a retornar cada columna
    ''' </summary>
    ''' <param name="column"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function GetCellContentRecyclingKey(ByVal column As DataGridColumn) As Object

        Select Case column.Name
            Case "cCantidad"
                Return GetType(System.Windows.Controls.Grid)
            Case Else
                Return GetType(System.Windows.Controls.TextBlock)
        End Select

        Return GetType(System.Windows.Controls.TextBlock)
    End Function

    ''' <summary>
    ''' Obtener el contenedor de las columnas a mostra
    ''' </summary>
    ''' <param name="column"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function CreateCellContent(ByVal column As DataGridColumn) As FrameworkElement
        Dim txt As FrameworkElement = Nothing

        Select Case column.Name
            Case "cCantidad"
                txt = New System.Windows.Controls.Grid
            Case Else
                txt = New System.Windows.Controls.TextBlock
        End Select

        Return txt
    End Function

    ''' <summary>
    ''' Realizar el calculo y ontener el contenido de cada columna que se va a mostrar
    ''' </summary>
    ''' <param name="cellContent"></param>
    ''' <param name="column"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub BindCellContent(ByVal cellContent As FrameworkElement, ByVal column As DataGridColumn)
        'Try
        'Obtener el contenedor
        Dim txt As System.Windows.FrameworkElement = cellContent
        txt.HorizontalAlignment = column.HorizontalAlignment
        txt.VerticalAlignment = column.VerticalAlignment

        Dim value As Object

        If Me.Rows.Where(Function(r) r.Type = DataGridRowType.Item).Count = 1 Then
            Me.Height = New DataGridLength(0.3)
            Me.GroupRowsVisibility = Visibility.Visible

            value = String.Empty

            'Asignar el contenido
            Select Case column.Name
                Case "cCantidad"
                    value = New TextBlock
            End Select
        Else
            'Recuperar el contenido de acuerdo al Compute de cada columna
            value = Summaries(column).Compute(Me.Rows, column, True)
        End If

        'Asignar el contenido
        Select Case column.Name
            Case "cCantidad"
                Dim grid As Grid = txt
                grid.HorizontalAlignment = HorizontalAlignment.Center
                grid.Children.Clear()
                grid.Children.Add(value)
            Case Else
                DirectCast(txt, TextBlock).Text = value
        End Select
        'Catch
        '    Throw
        'End Try
    End Sub
#End Region

End Class

