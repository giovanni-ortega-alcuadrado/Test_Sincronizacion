Imports A2.OyD.OYDServer.RIA.Web
Imports C1.WPF.DataGrid

' ' '
' Funciones para calcular los valores que van a ser mostrados en las filas agrupadas
' ' '

''' <summary>
''' Interface para unificar las clases que realizan el cálculo
''' </summary>
''' <remarks></remarks>
Public Interface IComputeGroup
    Function Compute(ByVal rows As DataGridRowCollection, ByVal column As DataGridColumn, ByVal recursive As Boolean) As Object
End Interface

''' <summary>
''' Obtiene el texto para mostrar, cuando solo tiene un valor en la colección
''' </summary>
''' <remarks></remarks>
Public Class ComputeTextosGrupo
    Implements IComputeGroup

    Public Const STR_PUNTOS_SUSPENSIVOS As String = "..."

#Region "IComputeGroup Members"

    ''' <summary>
    ''' Calcular el valor del grupo
    ''' </summary>
    ''' <param name="rows"></param>
    ''' <param name="column"></param>
    ''' <param name="recursive"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Compute(ByVal rows As DataGridRowCollection, ByVal column As DataGridColumn, ByVal recursive As Boolean) As Object Implements IComputeGroup.Compute
        Dim sum As Double = 0
        Dim count As Double = 0
        Dim item As New List(Of VisorDeGarantias)

        ' Obtener los registros asociados a la fila grupo
        For Each row In rows.AsEnumerable(Function(r) r.Type = DataGridRowType.Item, Function(groupRow) recursive)
            item.Add(row.DataItem)
        Next

        'Obtener los valores de la columna e identifica si todos tienen el mismo valor para mostrarlo
        If Not String.IsNullOrWhiteSpace(column.Tag) Then

            'Obtiene la lista de valores con distinct
            Dim lista = item.Select(Function(r) r.GetType().GetProperty(column.Tag).GetValue(r, Nothing)).Where(Function(r) Not String.IsNullOrWhiteSpace(r)).Distinct().ToList()

            'Si la lista contiene mas de un valor muestra la columna en blanco
            If lista.Count > 1 Then
                Return STR_PUNTOS_SUSPENSIVOS
            ElseIf lista.Any Then
                'Sino muestra el valor de la columna
                Return lista.FirstOrDefault()
            End If
        End If
        Return String.Empty
    End Function

#End Region
End Class

''' <summary>
''' Obtener el estado de las filas agrupadas
''' </summary>
''' <remarks></remarks>
Public Class ComputeEstadoGrupo
    Implements IComputeGroup

#Region "Variables de clase"
    Dim stack As Grid
    Dim txt As TextBlock
    Dim rctValor As Rectangle
    Dim rctEstado As Rectangle
    Dim visor As VisorDeGarantias
    Dim valorLiquidaciones As Decimal = 0
    Dim valorBloqueos As Decimal = 0
    Dim valorEspecieBloqueado As Decimal = 0
    Dim porcentajeAlarma As Decimal = 0
#End Region

#Region "IComputeGroup Members"

    ''' <summary>
    ''' Obtener el contenido de la celda Estado para las filas agrupadas
    ''' </summary>
    ''' <param name="rows"></param>
    ''' <param name="column"></param>
    ''' <param name="recursive"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Compute(ByVal rows As DataGridRowCollection, ByVal column As DataGridColumn, ByVal recursive As Boolean) As Object Implements IComputeGroup.Compute
        'Crear y asignar controles y valores iniciales
        AsignarValoresIniciales()

        'Obtener la suma de los valores totales
        For Each row In rows.AsEnumerable(Function(r) r.Type = DataGridRowType.Item, Function(groupRow) recursive)
            visor = row.DataItem
            valorLiquidaciones += visor.ValorTotalRequerido
            valorBloqueos += visor.ValorInternoAsociado

            If visor.PorcCumplimientoInterno > porcentajeAlarma Then
                porcentajeAlarma = visor.PorcCumplimientoInterno
            End If
        Next

        'Obtener el estado actual
        Dim estadoAlarma As EstadosAlarma = VisorDeGarantiasViewModel.ObtenerEstado(valorLiquidaciones, valorBloqueos, porcentajeAlarma)

        'Obtener el texto con el valor del estado a mostrar
        txt.Text = VisorDeGarantiasViewModel.ObtenerSaldoLiquidacion(estadoAlarma, valorLiquidaciones, valorBloqueos)

        'Asignar los valores a las barras y los colores correspondientes
        AsignarValoresEstado(estadoAlarma, valorLiquidaciones, valorBloqueos, porcentajeAlarma)

        Return stack
    End Function

#End Region

#Region "Métodos"
    ''' <summary>
    ''' Asignar los valores iniciales para obtener el estado
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AsignarValoresIniciales()
        stack = New Grid With {.HorizontalAlignment = HorizontalAlignment.Center, .VerticalAlignment = VerticalAlignment.Center}
        txt = New TextBlock With {.HorizontalAlignment = HorizontalAlignment.Center}
        rctValor = New Rectangle With {.HorizontalAlignment = HorizontalAlignment.Left, .VerticalAlignment = VerticalAlignment.Bottom, .Height = 3, .Width = 24}
        rctEstado = New Rectangle With {.HorizontalAlignment = HorizontalAlignment.Left, .VerticalAlignment = VerticalAlignment.Bottom, .Height = 3, .Width = 24}

        stack.Children.Add(txt)
        stack.Children.Add(rctEstado)
        stack.Children.Add(rctValor)

        valorLiquidaciones = 0
        valorBloqueos = 0
        valorEspecieBloqueado = 0
        porcentajeAlarma = 0
    End Sub

    ''' <summary>
    ''' Asignar los colores y tamaño de las barras del estado
    ''' </summary>
    ''' <param name="estadoAlarma"></param>
    ''' <remarks></remarks>
    Private Sub AsignarValoresEstado(estadoAlarma As EstadosAlarma, valorLiquidaciones As Decimal, totalSaldoBloqueado As Decimal, porcentajeAlarma As Decimal)

        'Obtener el tamaño de la barra del estado
        rctValor.Width = VisorDeGarantiasViewModel.ObtenerTamanoEstado(valorLiquidaciones, totalSaldoBloqueado, porcentajeAlarma)
        rctValor.Fill = New SolidColorBrush(Color.FromArgb(255, 0, 141, 193))
        rctEstado.Width = 100
        'Obtener el color del estado
        rctEstado.Fill = New SolidColorBrush(VisorDeGarantiasViewModel.ObtenerColorEstado(estadoAlarma))

    End Sub
#End Region
End Class



''' <summary>
''' Obtener la sumatoria del valor de la columna
''' </summary>
''' <remarks></remarks>
Public Class ComputeSaldosGrupo
    Implements IComputeGroup

#Region "IComputeGroup Members"

    ''' <summary>
    ''' Calcular el valor del grupo
    ''' </summary>
    ''' <param name="rows"></param>
    ''' <param name="column"></param>
    ''' <param name="recursive"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Compute(ByVal rows As DataGridRowCollection, ByVal column As DataGridColumn, ByVal recursive As Boolean) As Object Implements IComputeGroup.Compute
        'Sumar los valores de la columna de saldo especificado
        If Not String.IsNullOrWhiteSpace(column.Tag) Then

            Dim sum As Double = 0
            Dim valor As Double

            ' Obtener los registros asociados a la fila grupo
            For Each row In rows.AsEnumerable(Function(r) r.Type = DataGridRowType.Item, Function(groupRow) recursive)
                If Double.TryParse(row.DataItem.GetType().GetProperty(column.Tag).GetValue(row.DataItem, Nothing), valor) Then
                    sum += valor
                End If
            Next

            Return String.Format(VisorDeGarantiasViewModel.STR_FORMATO_VALOR, sum)
        End If
        Return String.Empty
    End Function

#End Region
End Class

''' <summary>
''' Obtener el estado y la cantidad de registros agrupados
''' </summary>
''' <remarks></remarks>
Public Class ComputeCantidadGrupo
    Implements IComputeGroup

#Region "Constantes"
    Private Const STR_FORMATO_CANTIDAD As String = "{0}/{1}"
#End Region

#Region "IComputeGroup Members"

    ''' <summary>
    ''' Obtener los valores para la columna Cantidad de las filas agrupadas
    ''' </summary>
    ''' <param name="rows"></param>
    ''' <param name="column"></param>
    ''' <param name="recursive"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Compute(ByVal rows As DataGridRowCollection, ByVal column As DataGridColumn, ByVal recursive As Boolean) As Object Implements IComputeGroup.Compute
        Dim stack As New StackPanel With {.HorizontalAlignment = HorizontalAlignment.Center, .VerticalAlignment = VerticalAlignment.Center}
        Dim txt As New TextBlock With {.HorizontalAlignment = HorizontalAlignment.Center}
        Dim rct As New Rectangle With {.HorizontalAlignment = HorizontalAlignment.Center, .Height = 3, .Width = 24}

        Dim fe As FrameworkElement = stack

        stack.Children.Add(txt)
        stack.Children.Add(rct)

        'Obtener las filas del grupo formateadas
        Dim filas = rows.AsEnumerable(Function(r) r.Type = DataGridRowType.Item, Function(groupRow) recursive).Select(Function(r) CType(r.DataItem, VisorDeGarantias)).ToList

        'Obtener el estado y la cantidad de registros con ese estado
        Dim estadoCantidad As KeyValuePair(Of EstadosAlarma, Integer) = VisorDeGarantiasViewModel.ObtenerEstadoCantidad(filas)


        If VisorDeGarantiasViewModel.ObtenerClientesSeleccionadosPorGrupo(rows).Count = 1 Then
            Dim btn As New Button With {.HorizontalAlignment = HorizontalAlignment.Center}

            btn.Content = stack
            Dim rDictionary = New ResourceDictionary()
            rDictionary.Source = New Uri("/A2.OyD.PLUSGarantias.SL;component/Assets/CoreStyles.xaml", UriKind.Relative)
            btn.Style = rDictionary("ControlButtonSecundaryStyle")

            AddHandler btn.Click, Sub(sender As Object, e As EventArgs)
                                      CType(column.DataGrid.DataContext, VisorDeGarantiasViewModel).AbrirVistaDetalle(filas.FirstOrDefault)
                                  End Sub

            fe = btn
        End If

        Dim cantidadtotal As Integer = filas.Count

        'Asignar el color a la barra de estado
        rct.Fill = New SolidColorBrush(VisorDeGarantiasViewModel.ObtenerColorEstado(estadoCantidad.Key))

        'Asignar el texto a la cantidad
        If estadoCantidad.Value = cantidadtotal Then
            txt.Text = estadoCantidad.Value
        Else
            txt.Text = String.Format(STR_FORMATO_CANTIDAD, estadoCantidad.Value, cantidadtotal)
        End If

        Return fe
    End Function

#End Region
End Class