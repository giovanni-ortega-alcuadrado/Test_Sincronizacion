Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.ComponentModel
Imports System.Collections.ObjectModel

Partial Public Class ucListMultiselectedCombobox
    Inherits UserControl
    Implements INotifyPropertyChanged

    Private logCambiarSeleccionados As Boolean = True

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Sub New()
        Try
            InitializeComponent()
            Me.DataContext = Me
            'Valores iniciales
            logCambiarSeleccionados = False
            TextoBotonMarcarTodos = "Marcar todos"
            TextoBotonDesmarcarTodos = "Desmarcar todos"
            TextoSeleccionarItems = "Seleccione los items de la lista..."
            AnchoPopup = 350
            EstableceMarcas(SeleccionarTodosPorDefecto)
            EstableceValorTextoSeleccionados()
            logCambiarSeleccionados = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al abrir el selector.", "ucListMultiselectedCombobox", "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

#Region "Propiedades"

    Shared _TextoItemsSeleccionados As String
    Public Property TextoItemsSeleccionados As String
        Get
            Return _TextoItemsSeleccionados
        End Get
        Set(value As String)
            _TextoItemsSeleccionados = value
            txtHeader.Text = value
            cboItems.Header = value
        End Set
    End Property

    Private Property _TextoBotonMarcarTodos As String
    Public Property TextoBotonMarcarTodos As String
        Get
            Return _TextoBotonMarcarTodos
        End Get
        Set(ByVal value As String)
            _TextoBotonMarcarTodos = value
            btnMarcarTodos.Content = value
        End Set
    End Property

    Private Property _TextoBotonDesmarcarTodos As String
    Public Property TextoBotonDesmarcarTodos As String
        Get
            Return _TextoBotonDesmarcarTodos
        End Get
        Set(ByVal value As String)
            _TextoBotonDesmarcarTodos = value
            btnDesmarcarTodos.Content = value
        End Set
    End Property

    Private Property _TextoSeleccionarItems As String
    Public Property TextoSeleccionarItems As String
        Get
            Return _TextoSeleccionarItems
        End Get
        Set(ByVal value As String)
            _TextoSeleccionarItems = value
        End Set
    End Property

    Private Property _SeleccionarTodosPorDefecto As Boolean
    Public Property SeleccionarTodosPorDefecto As Boolean
        Get
            Return _SeleccionarTodosPorDefecto
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodosPorDefecto = value
            EstableceMarcas(value)
            EstableceValorTextoSeleccionados()
        End Set
    End Property

    Public Property _AnchoPopup As Integer
    Public Property AnchoPopup As Integer
        Get
            Return _AnchoPopup
        End Get
        Set(ByVal value As Integer)
            _AnchoPopup = value
            BorderAncho.Width = value
        End Set
    End Property

#End Region

#Region "Dependientes"

    Public Shared ReadOnly ItemsListaProperty As DependencyProperty = DependencyProperty.Register("ItemsLista",
                                                                                     GetType(ObservableCollection(Of ItemCombo)),
                                                                                     GetType(ucListMultiselectedCombobox),
                                                                                     New PropertyMetadata(New ObservableCollection(Of ItemCombo),
                                                                                                          New PropertyChangedCallback(AddressOf OnChangeItemsLista)))
    Public Property ItemsLista As ObservableCollection(Of ItemCombo)
        Get
            Return CType(GetValue(ItemsListaProperty), ObservableCollection(Of ItemCombo))
        End Get
        Set(ByVal value As ObservableCollection(Of ItemCombo))
            SetValue(ItemsListaProperty, value)
        End Set
    End Property

    Shared Sub OnChangeItemsLista(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim control = CType(d, ucListMultiselectedCombobox)
        If control IsNot Nothing Then
            control.lstItems.ItemsSource = control.ItemsLista
            control.ItemsListaSeleccionados = New ObservableCollection(Of ItemCombo)
            If control.ItemsLista IsNot Nothing Then
                control.MapeoDatasource()
            End If
            control.EstableceValorTextoSeleccionados()
        End If
    End Sub

    Public Shared ReadOnly ItemsListaSeleccionadosProperty As DependencyProperty = DependencyProperty.Register("ItemsListaSeleccionados",
                                                                                    GetType(ObservableCollection(Of ItemCombo)),
                                                                                    GetType(ucListMultiselectedCombobox),
                                                                                    New PropertyMetadata(New ObservableCollection(Of ItemCombo),
                                                                                                          New PropertyChangedCallback(AddressOf OnChangeItemsListaSeleccionados)))

    Public Property ItemsListaSeleccionados As ObservableCollection(Of ItemCombo)
        Get
            Return CType(GetValue(ItemsListaSeleccionadosProperty), ObservableCollection(Of ItemCombo))
        End Get
        Set(value As ObservableCollection(Of ItemCombo))
            SetValue(ItemsListaSeleccionadosProperty, value)
        End Set
    End Property
    Shared Sub OnChangeItemsListaSeleccionados(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
        Dim control = CType(d, ucListMultiselectedCombobox)
        If control IsNot Nothing Then
            If control.logCambiarSeleccionados Then
                If control.ItemsListaSeleccionados IsNot Nothing Then
                    control.EstableceSeleccionados(control.ItemsListaSeleccionados)
                End If
            End If
        End If
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub btnMarcarTodos_Click(sender As Object, e As RoutedEventArgs)
        EstableceMarcas(True)
    End Sub

    Private Sub btnDesmarcarTodos_Click(sender As Object, e As RoutedEventArgs)
        EstableceMarcas(False)
    End Sub

    ''' <summary>
    ''' Metodo para la funcionalidad de marcar y desmarcar todos
    ''' </summary>
    ''' <param name="bitTodos"></param>
    ''' <remarks></remarks>
    Private Sub EstableceMarcas(ByVal bitTodos As Boolean)
        If ItemsLista IsNot Nothing Then
            For Each item In ItemsLista
                item.Retorno = bitTodos.ToString
            Next
            ItemsListaSeleccionados = New ObservableCollection(Of ItemCombo)(ItemsLista.Where(Function(i) i.Retorno = Boolean.TrueString).ToList)
        Else
            ItemsListaSeleccionados = New ObservableCollection(Of ItemCombo)
        End If
    End Sub

    Private Function FindFirstElementInVisualTree(Of T As DependencyObject)(parentElement As DependencyObject) As T
        Dim count = VisualTreeHelper.GetChildrenCount(parentElement)
        If count = 0 Then
            Return Nothing
        End If
        For i As Integer = 0 To count - 1
            Dim child = VisualTreeHelper.GetChild(parentElement, i)

            If child IsNot Nothing AndAlso TypeOf child Is T Then
                Return DirectCast(child, T)
            Else
                Dim result = FindFirstElementInVisualTree(Of T)(child)
                If result IsNot Nothing Then
                    Return result

                End If
            End If
        Next
        Return Nothing
    End Function


    ''' <summary>
    ''' Metodo para checkear los seleccionados
    ''' </summary>
    ''' <param name="lstSeleccionados"></param>
    ''' <remarks></remarks>
    Private Sub EstableceSeleccionados(lstSeleccionados As ObservableCollection(Of ItemCombo))
        Try
            Dim itemList As ItemCombo
            For Each item In lstSeleccionados
                itemList = ItemsLista.Where(Function(i) i.ID = item.ID).FirstOrDefault
                If itemList IsNot Nothing Then
                    itemList.Retorno = Boolean.TrueString
                End If
            Next
        Catch
        End Try
    End Sub

    Private Sub cboItems_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        cboItems.IsDropDownOpen = Not cboItems.IsDropDownOpen
    End Sub

    Private Sub chkSelected_Checked(sender As Object, e As RoutedEventArgs)
        Try
            CType(sender, CheckBox).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource()
            EstableceValorTextoSeleccionados()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub chkSelected_Unchecked(sender As Object, e As RoutedEventArgs)
        Try
            CType(sender, CheckBox).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource()
            EstableceValorTextoSeleccionados()
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Metodo creado para establecer los seleccionados
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EstableceValorTextoSeleccionados()
        Dim strSeleccionados As String = String.Empty
        Dim intSeleccionados As Integer = 0
        If Not IsNothing(ItemsLista) Then
            For Each item In ItemsLista
                If item.Retorno = Boolean.TrueString Then
                    If String.IsNullOrEmpty(strSeleccionados) Then
                        strSeleccionados = CType(item, ItemCombo).Descripcion
                    Else
                        strSeleccionados += ", " + CType(item, ItemCombo).Descripcion
                    End If
                    intSeleccionados += 1
                End If
            Next
        End If
        'valido el texto de los seleccionados
        If String.IsNullOrEmpty(strSeleccionados) Then
            TextoItemsSeleccionados = TextoSeleccionarItems
        ElseIf (intSeleccionados = ItemsLista.Count) Then
            TextoItemsSeleccionados = "(Todos)"
        Else
            TextoItemsSeleccionados = strSeleccionados
        End If
        logCambiarSeleccionados = False
        'actualizo el listado de seleccionados
        ItemsListaSeleccionados = New ObservableCollection(Of ItemCombo)(ItemsLista.Where(Function(i) i.Retorno = Boolean.TrueString).ToList)
        logCambiarSeleccionados = True
    End Sub

    Private Sub MapeoDatasource()
        If Not IsNothing(ItemsLista) Then
            For Each item In ItemsLista
                item.Retorno = SeleccionarTodosPorDefecto.ToString
            Next
        End If
    End Sub

#End Region

End Class
