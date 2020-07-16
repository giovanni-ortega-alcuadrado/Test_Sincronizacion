Imports C1.WPF.FlexGrid
Imports C1.WPF.Excel
Imports System.ComponentModel
Imports System.Data
Imports Newtonsoft.Json

Public Class ControlInfoGridView
    Implements INotifyPropertyChanged
    Dim strArreglo As List(Of String)
    Dim objPlantilla As New clsPlantillaExportacion
    Dim ctxMenu = New ContextMenu()
    Dim _objDataViewControl As DataView = Nothing
    Dim _strNombreArchivoControl As String = String.Empty
    Dim _strFiltrosDefectoControl As String = String.Empty
    Dim _logCambiarSeleccionarTodos As Boolean = True

    Public Event FiltroDiseno_Modificado(ByVal pintIDRegistro As Integer, ByVal pstrNombreDiseno As String, ByVal pstrFiltroDiseno As String)
    Public Event FiltroDiseno_Borrado()

#Region "PropiedadesDependientes"

    Private Shared ReadOnly IDRegistroDep As DependencyProperty = DependencyProperty.Register("IDRegistro", GetType(Integer), GetType(ControlInfoGridView), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf IDRegistroChanged)))
    Public Property IDRegistro As Integer
        Get
            Return CInt(GetValue(IDRegistroDep))
        End Get
        Set(ByVal value As Integer)
            SetValue(IDRegistroDep, value)
        End Set
    End Property
    Private Shared Sub IDRegistroChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As ControlInfoGridView = DirectCast(d, ControlInfoGridView)
            If Not IsNothing(obj) Then
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "ControlInfoGridView", "IDRegistroChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Shared ReadOnly NombreDisenoDep As DependencyProperty = DependencyProperty.Register("NombreDiseno", GetType(String), GetType(ControlInfoGridView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf NombreDisenoChanged)))
    Public Property NombreDiseno As String
        Get
            Return CStr(GetValue(NombreDisenoDep))
        End Get
        Set(ByVal value As String)
            SetValue(NombreDisenoDep, value)
        End Set
    End Property
    Private Shared Sub NombreDisenoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As ControlInfoGridView = DirectCast(d, ControlInfoGridView)
            If Not IsNothing(obj) Then
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "ControlInfoGridView", "NombreDisenoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Shared ReadOnly FiltrosDefectoDep As DependencyProperty = DependencyProperty.Register("FiltrosDefecto", GetType(String), GetType(ControlInfoGridView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf FiltrosDefectoChanged)))
    Public Property FiltrosDefecto As String
        Get
            Return CStr(GetValue(FiltrosDefectoDep))
        End Get
        Set(ByVal value As String)
            SetValue(FiltrosDefectoDep, value)
        End Set
    End Property
    Private Shared Sub FiltrosDefectoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As ControlInfoGridView = DirectCast(d, ControlInfoGridView)
            If Not IsNothing(obj) Then
                obj._strFiltrosDefectoControl = obj.FiltrosDefecto
                obj.LeerConfiguracionDefecto()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "ControlInfoGridView", "FiltrosAplicadosChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Shared ReadOnly NombreExportacionDep As DependencyProperty = DependencyProperty.Register("NombreExportacion", GetType(String), GetType(ControlInfoGridView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf NombreExportacionChanged)))
    Public Property NombreExportacion As String
        Get
            Return CStr(GetValue(NombreExportacionDep))
        End Get
        Set(ByVal value As String)
            SetValue(NombreExportacionDep, value)
        End Set
    End Property
    Private Shared Sub NombreExportacionChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As ControlInfoGridView = DirectCast(d, ControlInfoGridView)
            If Not IsNothing(obj) Then
                obj._strNombreArchivoControl = obj.NombreExportacion
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "ControlInfoGridView", "NombreExportacionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Shared ReadOnly DataViewSPDep As DependencyProperty = DependencyProperty.Register("DataViewSP", GetType(DataView), GetType(ControlInfoGridView), New PropertyMetadata(Nothing, New PropertyChangedCallback(AddressOf DataViewSPChanged)))
    Public Property DataViewSP As DataView
        Get
            Return CType(GetValue(DataViewSPDep), DataView)
        End Get
        Set(ByVal value As DataView)
            SetValue(DataViewSPDep, value)
        End Set
    End Property
    Private Shared Sub DataViewSPChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As ControlInfoGridView = DirectCast(d, ControlInfoGridView)
            If Not IsNothing(obj) Then
                obj._objDataViewControl = obj.DataViewSP
                obj.RefrescarGrid()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "ControlInfoGridView", "DataViewSPChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _NumeroRegistrosFiltrados As String
    Public Property NumeroRegistrosFiltrados() As String
        Get
            Return _NumeroRegistrosFiltrados
        End Get
        Set(ByVal value As String)
            _NumeroRegistrosFiltrados = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroRegistrosFiltrados"))
        End Set
    End Property

    Private _NumeroRegistrosTotal As String
    Public Property NumeroRegistrosTotal() As String
        Get
            Return _NumeroRegistrosTotal
        End Get
        Set(ByVal value As String)
            _NumeroRegistrosTotal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroRegistrosTotal"))
        End Set
    End Property

    Private _listaColumnas As List(Of clsColumna) = New List(Of clsColumna)
    Public Property listaColumnas() As List(Of clsColumna)
        Get
            Return _listaColumnas
        End Get
        Set(ByVal value As List(Of clsColumna))
            _listaColumnas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("listaColumnas"))
        End Set
    End Property

#End Region

    Public Sub New()
        Try
            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.DataContext = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar la información del Grid.",
                                 Me.ToString(), "ControlInfoGridView", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Public Sub New(ByVal pobjDataViewSP As DataView, ByVal pstrFiltrosDefecto As String, ByVal pstrNombreExportacion As String)
        Try
            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.DataContext = Me

            _objDataViewControl = pobjDataViewSP
            _strFiltrosDefectoControl = pstrFiltrosDefecto
            _strNombreArchivoControl = pstrNombreExportacion

            RefrescarGrid()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar la información del Grid.",
                                 Me.ToString(), "ControlInfoGridView", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub RefrescarGrid()
        Try
            myBusyIndicator.IsBusy = True
            fg.ItemsSource = _objDataViewControl
            BorrarFiltros()
            ActualizarRegistros()
            LeerConfiguracionDefecto()
            myBusyIndicator.IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar el grid.",
                                Me.ToString(), "RefrescarGrid", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub CargarColumnas()
        Try
            listaColumnas.Clear()
            lbColumnas.ItemsSource = Nothing
            Dim logSeleccionartodos As Boolean = True

            For Each c In fg.Columns
                c.Width = GridLength.Auto

                If c.Visible = False Then
                    logSeleccionartodos = False
                End If
                listaColumnas.Add(New clsColumna With {.Nombre = c.BoundPropertyName, .Visible = c.Visible})
            Next

            fg.AutoSizeColumns(0, fg.Columns.Count - 1, 0.1)
            fg.ApplyTemplate()

            _logCambiarSeleccionarTodos = False
            chkSeleccionarTodos.IsChecked = logSeleccionartodos
            _logCambiarSeleccionarTodos = True

            ExpanderColumnas.IsExpanded = True
            lbColumnas.ItemsSource = listaColumnas
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar las columnas.",
                                 Me.ToString(), "CargarColumnas", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub C1FlexGridFilter_FilterApplied(sender As Object, e As EventArgs)
        ActualizarRegistros()
    End Sub

    Private Sub ActualizarRegistros()
        Try
            Dim cnt = (From r In fg.Rows Where r.Visible = True).Count
            Dim x = LayoutRoot
            NumeroRegistrosFiltrados = cnt.ToString("N0")
            NumeroRegistrosTotal = fg.Rows.Count.ToString("N0")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar la información.",
                                 Me.ToString(), "ActualizarRegistros", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Function CrearStringConfiguracionColumnas() As String
        Try
            objPlantilla = New clsPlantillaExportacion
            Dim f = C1FlexGridFilterService.GetFlexGridFilter(fg)
            objPlantilla.XMLConfiguracionColumnas = fg.ColumnLayout
            objPlantilla.XMLFiltros = f.FilterDefinition
            If fg.Rows.Count > 0 Then
                If Not IsNothing(fg.Rows.First.DataItem) Then
                    If Not IsNothing(fg.Rows.First.DataItem.DataView) Then
                        If Not String.IsNullOrEmpty(fg.Rows.First.DataItem.DataView.Sort) Then
                            objPlantilla.CampoOrden = fg.Rows.First.DataItem.DataView.Sort.ToString()
                            objPlantilla.CampoOrden = objPlantilla.CampoOrden.Replace("[", "").Replace("]", "")
                            objPlantilla.CampoOrden = objPlantilla.CampoOrden.Replace("DESC", "").Replace("desc", "").Replace("Desc", "")
                            objPlantilla.CampoOrden = objPlantilla.CampoOrden.Trim
                        End If
                    End If
                End If
            End If
            Return JsonConvert.SerializeObject(objPlantilla)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear la configuración de las columnas.",
                                 Me.ToString(), "CrearStringConfiguracionColumnas", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
            Return String.Empty
        End Try
    End Function

    Private Sub btnExportar_Click(sender As Object, e As RoutedEventArgs)
        Try
            myBusyIndicator.IsBusy = True
            Dim logCreoArchivo As Boolean = False

            Dim dlg = New Microsoft.Win32.SaveFileDialog()
            dlg.FileName = NombreExportacion
            dlg.DefaultExt = "xlsx"
            dlg.Filter = " Excel Workbook (*.xlsx)|*.xlsx|" + " Comma Separated Values (*.csv)|*.csv"

            If dlg.ShowDialog() = True Then
                Dim ext = System.IO.Path.GetExtension(dlg.SafeFileName).ToLower()
                Select Case ext
                    Case ".csv"
                        Mouse.OverrideCursor = Cursors.Wait
                        fg.Save(dlg.FileName, C1.WPF.FlexGrid.FileFormat.Csv, SaveOptions.SaveColumnHeaders + SaveOptions.VisibleOnly)
                        Mouse.OverrideCursor = Cursors.Arrow
                        logCreoArchivo = True
                        Exit Select
                    Case ".xlsx"
                        Mouse.OverrideCursor = Cursors.Wait
                        GuardarExcel(dlg.FileName, fg)
                        Mouse.OverrideCursor = Cursors.Arrow
                        logCreoArchivo = True
                        Exit Select
                End Select

            End If

            If logCreoArchivo Then
                Program.VisorArchivosWeb_DescargarURL(dlg.FileName())
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Ocurrio un error al momento de generar el archivo. No se obtuvo la extensión del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            myBusyIndicator.IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al exportar el archivo.",
                                 Me.ToString(), "btnExportar_Click", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub GuardarExcel(ByVal pstrNombreArchivo As String, ByVal pobjFlexgrid As C1FlexGrid)
        Try
            Dim objDataTable As DataTable = C1FlexGridToDataTable(pobjFlexgrid)
            'Dim book = New C1XLBook()
            'book.Sheets.Clear()
            'Dim xlSheet = book.Sheets.Add()
            'ExcelFilter.Save(pobjFlexgrid, xlSheet)
            'book.Save(pstrNombreArchivo, C1.WPF.Excel.FileFormat.OpenXml)
            A2.OyD.Infraestructura.Utilidades.GenerarExcel(objDataTable, pstrNombreArchivo)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar la configuración.",
                                 Me.ToString(), "SaveExcel", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Public Function C1FlexGridToDataTable(ByVal pobjFlexgrid As C1FlexGrid) As DataTable
        Dim dt As New DataTable

        Try
            Dim objColumnas As New List(Of clsColumnasFlexGrid)
            Dim intContadorColumna As Integer = 0
            Dim intContadorColumnaReal As Integer = 0

            dt.TableName = NombreExportacion

            For Each col As Column In pobjFlexgrid.Columns
                If col.Visible Then
                    objColumnas.Add(New clsColumnasFlexGrid With {.intIDColumna = intContadorColumna, .intIDColumnaReal = intContadorColumnaReal, .objColumna = col})
                    intContadorColumnaReal += 1
                End If

                intContadorColumna += 1
            Next

            If Not IsNothing(objColumnas) Then
                For Each col As clsColumnasFlexGrid In objColumnas
                    Dim objDataColumn As New DataColumn
                    objDataColumn.AllowDBNull = True
                    objDataColumn.ColumnName = col.objColumna.ColumnName
                    objDataColumn.DataType = col.objColumna.DataType

                    dt.Columns.Add(objDataColumn)
                Next

                For Each row As Row In pobjFlexgrid.Rows
                    If row.Visible Then
                        Dim NewDataRow As DataRow = dt.NewRow

                        For Each col As clsColumnasFlexGrid In objColumnas
                            If (IsNothing(row(col.intIDColumna))) Then
                                NewDataRow(col.intIDColumnaReal) = DBNull.Value
                            Else
                                NewDataRow(col.intIDColumnaReal) = row(col.intIDColumna)
                            End If
                        Next

                        dt.Rows.Add(NewDataRow)
                    End If
                Next
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar la configuración.",
                                 Me.ToString(), "C1FlexGridToDataTable", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return dt
    End Function

    Private Sub LeerConfiguracionDefecto()
        Try
            myBusyIndicator.IsBusy = True
            If Not String.IsNullOrEmpty(_strFiltrosDefectoControl) Then
                Dim jsonExportConfigString As String = String.Empty
                jsonExportConfigString = _strFiltrosDefectoControl
                objPlantilla = JsonConvert.DeserializeObject(Of clsPlantillaExportacion)(jsonExportConfigString)

                'carga las columnas
                fg.ColumnLayout = objPlantilla.XMLConfiguracionColumnas
                fg.Invalidate()

                'aplica los filtros
                Dim f = C1FlexGridFilterService.GetFlexGridFilter(fg)
                f.FilterDefinition = objPlantilla.XMLFiltros
                f.Apply()

                'aplica ordenamiento
                If Not String.IsNullOrEmpty(objPlantilla.CampoOrden) Then
                    If Not IsNothing(fg.CollectionView) Then
                        If Not IsNothing(fg.CollectionView.SortDescriptions) Then
                            fg.CollectionView.SortDescriptions.Clear()
                        End If

                        If objPlantilla.CampoOrden.ToLower.Contains("desc") Then
                            fg.CollectionView.SortDescriptions.Add(New SortDescription(objPlantilla.CampoOrden, ListSortDirection.Descending))
                        Else
                            fg.CollectionView.SortDescriptions.Add(New SortDescription(objPlantilla.CampoOrden, ListSortDirection.Ascending))
                        End If
                    End If
                Else
                    If Not IsNothing(fg.CollectionView) Then
                        If Not IsNothing(fg.CollectionView.SortDescriptions) Then
                            fg.CollectionView.SortDescriptions.Clear()
                        End If
                    End If
                End If

                CargarColumnas()
            Else
                CargarColumnas()
            End If
            myBusyIndicator.IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al leer la configuración.",
                                 Me.ToString(), "LeerConfiguracionDefecto", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub btnBorrarFiltros_Click(sender As Object, e As RoutedEventArgs)
        Try
            FiltrosDefecto = String.Empty
            BorrarFiltros()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar los filtros aplicados.",
                                 Me.ToString(), "btnBorrarFiltros_Click", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub BorrarFiltros()
        Try
            Dim f = C1FlexGridFilterService.GetFlexGridFilter(fg)
            f.FilterDefinition = Nothing
            f.Apply()

            fg.CollectionView.SortDescriptions.Clear()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar los filtros.",
                                 Me.ToString(), "BorrarFiltros", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub chkColumnas_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim cb = CType(sender, CheckBox)
            Dim columna = cb.Tag.ToString
            fg.Columns(columna).Visible = cb.IsChecked

            Verificar_SeleccionarTodos()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.",
                                 Me.ToString(), "chkColumnas_Click", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub txtFiltroColumnas_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            FiltrarColumnas(txtFiltroColumnas.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.",
                                 Me.ToString(), "txtFiltroColumnas_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub btnFiltrarColumnas_Click(sender As Object, e As RoutedEventArgs)
        Try
            FiltrarColumnas(txtFiltroColumnas.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.",
                                 Me.ToString(), "btnFiltrarColumnas_Click", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub btnQuitarFiltroColumnas_Click(sender As Object, e As RoutedEventArgs)
        Try
            txtFiltroColumnas.Text = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.",
                                 Me.ToString(), "btnQuitarFiltroColumnas_Click", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub FiltrarColumnas(ByVal pstrTextoFiltro As String)
        lbColumnas.ItemsSource = Nothing
        Dim objListaNueva As New List(Of clsColumna)
        For Each li In listaColumnas
            If String.IsNullOrEmpty(pstrTextoFiltro) Then
                objListaNueva.Add(li)
            Else
                If li.Nombre.ToLower.Contains(pstrTextoFiltro.ToLower) Then
                    objListaNueva.Add(li)
                End If
            End If
        Next

        Dim logSeleccionartodos As Boolean = True

        For Each c In objListaNueva
            If c.Visible = False Then
                logSeleccionartodos = False
            End If
        Next

        _logCambiarSeleccionarTodos = False
        chkSeleccionarTodos.IsChecked = logSeleccionartodos
        _logCambiarSeleccionarTodos = True

        lbColumnas.ItemsSource = objListaNueva
    End Sub

    Private Sub chkSeleccionarTodos_Checked(sender As Object, e As RoutedEventArgs)
        Try
            SeleccionarColumnas(True)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar las columnas.",
                                 Me.ToString(), "chkSeleccionarTodos_Checked", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub chkSeleccionarTodos_Unchecked(sender As Object, e As RoutedEventArgs)
        Try
            SeleccionarColumnas(False)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar las columnas.",
                                 Me.ToString(), "chkSeleccionarTodos_Unchecked", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub SeleccionarColumnas(ByVal plogSeleccionar As Boolean)
        If Not IsNothing(lbColumnas.ItemsSource) And _logCambiarSeleccionarTodos Then
            Dim objListaNueva As New List(Of clsColumna)
            For Each li In lbColumnas.ItemsSource
                objListaNueva.Add(li)
            Next
            lbColumnas.ItemsSource = Nothing
            For Each li In objListaNueva
                li.Visible = plogSeleccionar
                fg.Columns(li.Nombre).Visible = plogSeleccionar
            Next
            lbColumnas.ItemsSource = objListaNueva

            Verificar_SeleccionarTodos()
        End If
    End Sub

    Private Sub Verificar_SeleccionarTodos()
        If Not IsNothing(lbColumnas.ItemsSource) Then
            Dim logSeleccionartodos As Boolean = True

            For Each c In lbColumnas.ItemsSource
                If c.Visible = False Then
                    logSeleccionartodos = False
                End If
            Next

            _logCambiarSeleccionarTodos = False
            chkSeleccionarTodos.IsChecked = logSeleccionartodos
            _logCambiarSeleccionarTodos = True
        End If
    End Sub

    Private Sub txtFiltroColumnas_GotFocus(sender As Object, e As RoutedEventArgs)
        MyBase.OnGotFocus(e)
        Dim strNombreControlCambio = String.Empty
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            strNombreControlCambio = CType(sender, TextBox).Name
        End If
    End Sub

    Private Sub btnGuardarDiseno_Click(sender As Object, e As RoutedEventArgs)
        Try
            RaiseEvent FiltroDiseno_Modificado(IDRegistro, NombreDiseno, CrearStringConfiguracionColumnas())
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar las columnas.",
                                 Me.ToString(), "chkSeleccionarTodos_Checked", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Private Sub btnQuitarDiseno_Click(sender As Object, e As RoutedEventArgs)
        Try
            myBusyIndicator.IsBusy = True
            IDRegistro = 0
            NombreDiseno = String.Empty
            FiltrosDefecto = String.Empty
            BorrarFiltros()

            Dim objListaNueva As New List(Of clsColumna)
            For Each li In listaColumnas
                objListaNueva.Add(li)
            Next
            lbColumnas.ItemsSource = Nothing
            For Each li In objListaNueva
                li.Visible = True
                fg.Columns(li.Nombre).Visible = True
            Next
            lbColumnas.ItemsSource = objListaNueva

            Verificar_SeleccionarTodos()

            RaiseEvent FiltroDiseno_Borrado()
            myBusyIndicator.IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al quitar el diseño seleccionado.",
                                 Me.ToString(), "btnQuitarDiseno_Click", Application.Current.ToString(), Program.Maquina, ex)
            myBusyIndicator.IsBusy = False
        End Try
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

End Class

Public Class clsColumna
    Public Property Nombre As String
    Public Property Visible As Boolean
End Class

Public Class clsPlantillaExportacion
    Public Property XMLConfiguracionColumnas As String
    Public Property XMLFiltros As String
    Public Property CampoOrden As String
End Class