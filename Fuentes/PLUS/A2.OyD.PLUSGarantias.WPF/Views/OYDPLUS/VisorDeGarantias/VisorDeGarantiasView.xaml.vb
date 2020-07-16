Imports SilverlightFX.UserInterface
Imports C1.Silverlight.Docking
Imports C1.Silverlight.DataGrid
Imports System.Windows.Data
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports C1.WPF.DataGrid

Partial Public Class VisorDeGarantiasView
    Inherits UserControl

    Dim objVM As VisorDeGarantiasViewModel
#Region "Constructores"
    ''' <summary>
    ''' Constructor de la ventana
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            Me.DataContext = New VisorDeGarantiasViewModel

            InitializeComponent()
            'Asignar el combobox para las vistas disponibles
            AsignarListaVistas()

            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar.", Me.ToString(),
                                                        "VisorDeGarantiasView_New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region

#Region "Eventos"
    ''' <summary>
    ''' Cambiar el tamaño de la ventana
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.Width = Application.Current.MainWindow.ActualWidth * 0.96
    End Sub
    ''' <summary>
    ''' Cuando el datagrid crea una fila agrupada, asociarle el tipo de fila para que muestre los valores agrupados del visor de garantias
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grid_CreatingRow(ByVal sender As System.Object, ByVal e As DataGridCreatingRowEventArgs)
        If e.Type = DataGridRowType.Group Then
            e.Row = New VisorDeGarantiasGroupRow
        End If
    End Sub

    ''' <summary>
    ''' Asignar a las columnas, el método que muestra los totales en la fila del grupo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grid_LoadingRow(ByVal sender As System.Object, ByVal e As DataGridRowEventArgs)

        If e.Row.Type = DataGridRowType.Group Then
            Dim groupRow As VisorDeGarantiasGroupRow = e.Row

            If groupRow.Rows.Where(Function(r) r.Type = DataGridRowType.Item).Count = 1 Then
                groupRow.Height = New DataGridLength(1) '1 Windows.Visibility.Collapsed
                Return
            End If

            Dim columns As New Dictionary(Of String, IComputeGroup)

            'Asignar a cada columna el método que obtiene el valor a mostrar en la fila del grupo
            For Each col In grid.Columns
                Select Case col.Name
                    Case "cCantidad"
                        columns.Add(col.Name, New ComputeCantidadGrupo())
                    Case "cEstado"
                        columns.Add(col.Name, New ComputeEstadoGrupo())
                    Case "cSaldo", "cSaldoBloqueado", "cDisponibleTitulos", "cSaldoBloqueadoTitulos", "cValorInicio", "cValorRegreso"
                        columns.Add(col.Name, New ComputeSaldosGrupo())
                    Case Else
                        columns.Add(col.Name, New ComputeTextosGrupo())
                End Select

                'Asignar el convertidor, para que el texto de la columna agrupada no se sobreescriba en la fila del grupo
                col.GroupContentConverter = Resources("TextoVacioConverter")

                groupRow.Summaries(grid.Columns(col.Name)) = columns(col.Name)
            Next

            groupRow.GroupRowsVisibility = Visibility.Collapsed
        End If
    End Sub

    ''' <summary>
    ''' Asignar la vista cuando se selecciona la opción en el combo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cbVistas_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not cbVistas.SelectedValue Is Nothing Then
            AsignarVista(cbVistas.SelectedValue.ToString)
        End If
    End Sub


    ''' <summary>
    ''' Iddentificar si es un solo cliente, cuando se selecciona una fila en el grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GridVisor_SelectionChanged(sender As Object, e As DataGridSelectionChangedEventArgs)
        objVM.CambioSeleccion(e.AddedRanges.Rows())
    End Sub

    ''' <summary>
    ''' Asignar el orden a las filas del grid, de acuerdo a la prioridad y cantidad de registros del grid
    ''' Elimina el ordenamiento por la columna agrupada y reordena únicamente por las columnas de prioridad
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub grid_GroupChanged(sender As Object, e As DataGridGroupChangedEventArgs)

        cbVistas.SelectedValue = "SIN_VISTA"

        'Obtener las columnas por las que se ordeno el grid, que son diferentes de la prioridad
        Dim ordenamientos = CType(grid.ItemsSource, PagedCollectionView).SortDescriptions.Where(Function(r) Not r.PropertyName.ToLower.Contains("prioridad")).ToList

        If ordenamientos.Any Then
            'asignar el orden de acuerdo a los grupos
            AsignarOrdenALasFilasDelGrid()

            'Eliminar los ordenamientos diferentes de la prioridad
            For Each sd As ComponentModel.SortDescription In ordenamientos
                CType(grid.ItemsSource, PagedCollectionView).SortDescriptions.Remove(sd)
            Next
        ElseIf Not e.NewGroupedColumns.All(Function(n) e.OldGroupedColumns.Any(Function(o) o.Column.Name = n.Column.Name)) Or Not e.OldGroupedColumns.All(Function(n) e.NewGroupedColumns.Any(Function(o) o.Column.Name = n.Column.Name)) Then
            'asignar el orden de acuerdo a los grupos
            AsignarOrdenALasFilasDelGrid()

            'Obtener las columnas de prioridad
            Dim cPrioridadGrupo = grid.Columns.FirstOrDefault(Function(r) r.Name = "cPrioridadGrupo")
            Dim cPrioridad = grid.Columns.FirstOrDefault(Function(r) r.Name = "cPrioridad")

            'Reordenar por las columnas de prioridad
            grid.SortBy(New KeyValuePair(Of DataGridColumn, DataGridSortDirection)(cPrioridadGrupo, DataGridSortDirection.Descending), New KeyValuePair(Of DataGridColumn, DataGridSortDirection)(cPrioridad, DataGridSortDirection.Descending))

        End If
    End Sub

    ''' <summary>
    ''' Asignar los valores iniciales al abrir la ventana
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub View_Loaded(sender As Object, e As RoutedEventArgs)
        Try
            objVM = CType(Me.DataContext, VisorDeGarantiasViewModel)

            objVM.ColumnaCantidadBloquear = gridPortafolio.Columns("ColumnCantidadBloquear")


            objVM.GridDetalle = gDetalle
            objVM.GridPrincipal = gPrincipal
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar.", Me.ToString(),
                                                        "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As String, pobjComitente As A2.OyD.OYDServer.RIA.Web.OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                objVM.strCodigoClienteBloqueo = pobjComitente.CodigoOYD
                objVM.strNombreClienteBloqueo = pobjComitente.Nombre
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se lanza cuando se borra o se limpia la información de un cliente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SV20160424</remarks>
    Private Sub btnLimpiarCliente_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVM.strCodigoClienteBloqueo = String.Empty
            objVM.strNombreClienteBloqueo = String.Empty
            objVM.logBorrarCliente = False
            objVM.logBorrarCliente = True
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region


#Region "Métodos"
    ''' <summary>
    ''' Agrupar por la columna seleccionada en el combo
    ''' </summary>
    ''' <param name="nombreColumna"></param>
    ''' <remarks></remarks>
    Private Sub AsignarVista(nombreColumna As String)
        Dim column As DataGridColumn = grid.Columns.FirstOrDefault(Function(r) r.Name = nombreColumna)
        If Not column Is Nothing Then
            'Agrupar el grid de acuerdo a la columna seleccionada
            grid.GroupBy(column)
        End If
    End Sub
    ''' <summary>
    ''' Asignar la lista de campos disponibles para agrupar de acuerdo a las columnas del grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AsignarListaVistas()

        'Obteiene cada columna que permite agrupar y la adiciona a la lista
        Dim listaVistas As New Dictionary(Of String, String)
        listaVistas.Add("SIN_VISTA", "")
        For Each col As DataGridColumn In grid.Columns
            If TypeOf col Is DataGridBoundColumn Then
                If CType(col, DataGridBoundColumn).CanUserMove Then
                    listaVistas.Add(col.Name, col.Header)
                End If
            ElseIf TypeOf col Is DataGridNumericColumn Then
                If CType(col, DataGridNumericColumn).CanUserMove Then
                    listaVistas.Add(col.Name, col.Header)
                End If
            End If
        Next

        cbVistas.ItemsSource = listaVistas
        cbVistas.SelectedValue = "SIN_VISTA"
    End Sub

    ''' <summary>
    ''' Asignar el orden en el campo prioridad a cada una de las filas del grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AsignarOrdenALasFilasDelGrid()
        'Limpriar el ordenamiento anterior
        For Each v As VisorDeGarantias In grid.ItemsSource
            v.PrioridadGrupo = 0
        Next

        Dim listafilas As New List(Of VisorDeGarantias)
        For Each row As DataGridGroupRow In grid.Rows.Where(Function(r) r.Type = DataGridRowType.Group)

            'Obtener los registros asociados a la fila tipo grupo
            listafilas = row.Rows.Where(Function(r) r.Type = DataGridRowType.Item).Select(Function(r) CType(r.DataItem, VisorDeGarantias)).ToList()

            'Asignar la prioridad a cada fila
            VisorDeGarantiasViewModel.AsignarPrioridad(listafilas)
        Next
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        objVM.ConsultarInformacion()
    End Sub

    Private Sub btnFiltro_Click(sender As Object, e As RoutedEventArgs)
        objVM.LimpiarFiltro()
    End Sub

    Private Sub btnVerInforme_Click(sender As Object, e As RoutedEventArgs)
        objVM.VerInforme()
    End Sub

    Private Sub btnEnviarCorreos_Click(sender As Object, e As RoutedEventArgs)
        'objVM.EnviarCorreos()
    End Sub

    Private Sub btnAbrirVistaDetalleCRCC_Click(sender As Object, e As RoutedEventArgs)
        'Dim objRegistroSeleccionado As GarantiaCRCCSeleccionada = CType(CType(sender, Button).Tag, GarantiaCRCCSeleccionada)
        'objVM.AbrirVistaDetalleCRCC(objRegistroSeleccionado)
    End Sub

    Private Sub btnAbrirVistaDetalle_Click(sender As Object, e As RoutedEventArgs)
        Dim objRegistroSeleccionado As VisorDeGarantias = CType(CType(sender, Button).Tag, VisorDeGarantias)
        objVM.AbrirVistaDetalle(objRegistroSeleccionado)
    End Sub

    Private Sub btnCerrarVistaDetalle_Click(sender As Object, e As RoutedEventArgs)
        objVM.CerrarVistaDetalle()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        objVM.Aceptar()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        objVM.Cancelar()
    End Sub

    Private Sub btnBloquear_Click(sender As Object, e As RoutedEventArgs)
        Dim objRegistroSeleccionado As VisorDeGarantias = CType(CType(sender, Button).Tag, VisorDeGarantias)
        objVM.Bloquear(objRegistroSeleccionado)
    End Sub

    Private Sub btnDesBloquear_Click(sender As Object, e As RoutedEventArgs)
        Dim objRegistroSeleccionado As VisorDeGarantias = CType(CType(sender, Button).Tag, VisorDeGarantias)
        objVM.DesBloquear(objRegistroSeleccionado)
    End Sub

    Private Sub btnDetalles_Click(sender As Object, e As RoutedEventArgs)
        objVM.CargarDetallesCliente()
    End Sub

    Private Sub btnDetallesSaldo_Click(sender As Object, e As RoutedEventArgs)
        objVM.MostrarOcultarSaldos()
    End Sub

    Private Sub btnDesbloquearSaldoBloqueado_Click(sender As Object, e As RoutedEventArgs)
        Dim objRegistroSeleccionado As SaldosBloqueados = CType(CType(sender, Button).Tag, SaldosBloqueados)
        objVM.DesbloquearSaldoBloqueado(objRegistroSeleccionado)
    End Sub

    Private Sub btnDistribuirSaldoBloqueado_Click(sender As Object, e As RoutedEventArgs)
        Dim objRegistroSeleccionado As SaldosBloqueados = CType(CType(sender, Button).Tag, SaldosBloqueados)
        objVM.DistribuirSaldoBloqueado(objRegistroSeleccionado)
    End Sub

    Private Sub btnCargarPortafolio_Click(sender As Object, e As RoutedEventArgs)
        objVM.CargarPortafolio()
    End Sub

    Private Sub btnBloquearTitulo_Click(sender As Object, e As RoutedEventArgs)
        Dim objRegistroSeleccionado As TitulosGarantia = CType(CType(sender, Button).Tag, TitulosGarantia)
        objVM.BloquearTitulo(objRegistroSeleccionado)
    End Sub

    Private Sub btnDistribuirTituloBloqueado_Click(sender As Object, e As RoutedEventArgs)
        Dim objRegistroSeleccionado As TitulosGarantia = CType(CType(sender, Button).Tag, TitulosGarantia)
        objVM.DistribuirTituloBloqueado(objRegistroSeleccionado)
    End Sub

#End Region

End Class