Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports System.ComponentModel
Imports Telerik.Windows.Controls

Partial Public Class BuscadorGenerico
    Inherits UserControl

#Region "Eventos"

    ''' <summary>
    ''' Evento que se dispara cuando se selecciona un elemento de la lista
    ''' </summary>
    ''' <param name="pstrIdItem">Código del elemento buscado cuando se trata de una búsqueda específica</param>
    ''' <param name="pobjItem">Elemento seleccionado</param>
    ''' <remarks></remarks>
    Public Event itemAsignado(ByVal pstrIdItem As String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
    Public Event itemAsignadoControlOrigen(ByVal pstrControlOrigen As String, ByVal pstrIdItem As String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)

#End Region

#Region "Constantes"

    Private Const STR_NOMBRE_VIEW_MODEL As String = "VM"
    Private Const STR_TEXTO_ABRIR_POPUP As String = " " '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda

#End Region

#Region "Variables"

    Private mlogDigitandoFiltro As Boolean = False
    Private mlogPrimeraBusqueada As Boolean = True
    Public WithEvents mobjVM As BuscadorGenericoViewModel ' Referencia al view model del control
#End Region

#Region "Propiedades"

    Private Shared IdItemDep As DependencyProperty = DependencyProperty.Register("IdItem", GetType(String), GetType(BuscadorGenerico), New PropertyMetadata(AddressOf CambioPropiedadDep))
    Private Shared EstadoItemDep As DependencyProperty = DependencyProperty.Register("EstadoItem", GetType(BuscadorGenericoViewModel.EstadosItem), GetType(BuscadorGenerico), New PropertyMetadata(AddressOf CambioPropiedadDep))
    Private Shared ReadOnly AgrupacionDep As DependencyProperty = DependencyProperty.Register("Agrupamiento", GetType(String), GetType(BuscadorGenerico), New PropertyMetadata("", New PropertyChangedCallback(AddressOf AgrupamientoChanged)))
    Private Shared ReadOnly Condicion1Dep As DependencyProperty = DependencyProperty.Register("Condicion1", GetType(String), GetType(BuscadorGenerico), New PropertyMetadata("", New PropertyChangedCallback(AddressOf Condicion1Changed)))
    Private Shared ReadOnly Condicion2Dep As DependencyProperty = DependencyProperty.Register("Condicion2", GetType(String), GetType(BuscadorGenerico), New PropertyMetadata("", New PropertyChangedCallback(AddressOf Condicion2Changed)))
    Private Shared ReadOnly BorrarItemDep As DependencyProperty = DependencyProperty.Register("BorrarItem", GetType(Boolean), GetType(BuscadorGenerico), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf BorrarItemChanged)))
    Private Shared ReadOnly MostrarLimpiarDep As DependencyProperty = DependencyProperty.Register("MostrarLimpiar", GetType(Boolean), GetType(BuscadorGenerico), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf MostrarLimpiarChanged)))

    ''' <summary>
    ''' Código del elemento que se desea buscar cuando es necesaria una búsqueda de un elemento específico
    ''' </summary>
    Public Property IdItem As String
        Get
            Return (Me.GetValue(IdItemDep).ToString)
        End Get
        Set(ByVal value As String)
            Me.SetValue(IdItemDep, value)
            Me.mobjVM.IdItem = value
        End Set
    End Property

    ''' <summary>
    ''' Estado de los elementos que se desea buscar (activos, inactivos, todos)
    ''' </summary>
    Public Property EstadoItem As BuscadorGenericoViewModel.EstadosItem
        Get
            Return (CType(Me.GetValue(EstadoItemDep), BuscadorGenericoViewModel.EstadosItem))
        End Get
        Set(ByVal value As BuscadorGenericoViewModel.EstadosItem)
            Me.SetValue(EstadoItemDep, value)
            Me.mobjVM.EstadoItem = value
        End Set
    End Property

    Private _mstrTipoItem As String = String.Empty
    ''' <summary>
    ''' Tipo de elementos que se desea buscar (por ejemplo ciudades, pasises, receptores, etc.)
    ''' </summary>
    Public Property TipoItem As String
        Get
            Return (_mstrTipoItem)
        End Get
        Set(ByVal value As String)
            _mstrTipoItem = value
            Me.mobjVM.TipoItem = value
        End Set
    End Property

    'Private _mstrAgrupamiento As String = String.Empty
    ''' <summary>
    ''' Permite indicar otra condición de búsqueda. No tiene una definición particular.
    ''' </summary>
    Public Property Agrupamiento As String
        Get
            Return CStr(GetValue(AgrupacionDep))
        End Get
        Set(ByVal value As String)
            SetValue(AgrupacionDep, value)
        End Set
    End Property

    Private Shared Sub AgrupamientoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorGenerico = DirectCast(d, BuscadorGenerico)

            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.Agrupamiento = obj.Agrupamiento
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorGenerico", "AgrupamientoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <remarks>SV20160203</remarks>
    Public Property Condicion1 As String
        Get
            Return CStr(GetValue(Condicion1Dep))
        End Get
        Set(ByVal value As String)
            SetValue(Condicion1Dep, value)
        End Set
    End Property

    Private Shared Sub Condicion1Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorGenerico = DirectCast(d, BuscadorGenerico)

            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.Condicion1 = obj.Condicion1
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorGenerico", "Condicion1Changed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <remarks>SV20160203</remarks>
    Public Property Condicion2 As String
        Get
            Return CStr(GetValue(Condicion2Dep))
        End Get
        Set(ByVal value As String)
            SetValue(Condicion2Dep, value)
        End Set
    End Property

    Private Shared Sub Condicion2Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorGenerico = DirectCast(d, BuscadorGenerico)

            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.Condicion2 = obj.Condicion2
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorGenerico", "Condicion2Changed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _mlogVerDetalle As Boolean = False
    ''' <summary>
    ''' Indica si en la parte inferior del buscador se depliega el detalle del elemento seleccionado
    ''' </summary>
    Public Property VerDetalle As Boolean
        Get
            Return (_mlogVerDetalle)
        End Get
        Set(ByVal value As Boolean)
            _mlogVerDetalle = value
        End Set
    End Property

    Private _mlogBuscarAlIniciar As Boolean = False
    ''' <summary>
    ''' Indica si al iniciar el control lanza una consulta de comitentes. Solamente aplica si IdComitente no se envía
    ''' </summary>
    Public Property BuscarAlIniciar As Boolean
        Get
            Return (_mlogBuscarAlIniciar)
        End Get
        Set(ByVal value As Boolean)
            _mlogBuscarAlIniciar = value
        End Set
    End Property

    ''' <summary>
    ''' Elemento seleccionado en el buscador
    ''' </summary>
    Public ReadOnly Property ItemActivo() As OYDUtilidades.BuscadorGenerico
        Get
            Return (mobjVM.ItemSeleccionado)
        End Get
    End Property

    ''' <summary>
    ''' Borrar los datos del item seleccionado.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 21 de septiembre del 2012.
    ''' </summary>
    Public Property BorrarItem As Boolean
        Get
            Return CBool(GetValue(BorrarItemDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(BorrarItemDep, value)
        End Set
    End Property

    Private Shared Sub BorrarItemChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorGenerico = DirectCast(d, BuscadorGenerico)

            If Not IsNothing(obj.BorrarItem) Then
                If obj.BorrarItem Then
                    If Not IsNothing(obj.mobjVM) Then
                        If Not IsNothing(obj.mobjVM.ListaBusquedaControl) Then
                            obj.mobjVM.ListaBusquedaControl.Clear()
                        End If
                        obj.mobjVM.ItemSeleccionadoBuscador = Nothing
                        obj.acbItems.SearchText = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorGenerico", "BorrarItemChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Property MostrarLimpiar As Boolean
        Get
            Return CBool(GetValue(MostrarLimpiarDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(MostrarLimpiarDep, value)
        End Set
    End Property

    Private Shared Sub MostrarLimpiarChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorGenerico = DirectCast(d, BuscadorGenerico)

            If Not IsNothing(obj.mobjVM) Then
                If obj.MostrarLimpiar Then
                    obj.cmbLimpiar.Visibility = Visibility.Visible
                Else
                    obj.cmbLimpiar.Visibility = Visibility.Collapsed
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorGenerico", "BorrarItemChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Callback"

    ''' <summary>
    ''' Procedimiento de Call back que se lanza cuando alguna de las dependency properties se modifica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Shared Sub CambioPropiedadDep(ByVal sender As Object, ByVal args As DependencyPropertyChangedEventArgs)
        Dim obj As BuscadorGenerico = DirectCast(sender, BuscadorGenerico)
        If Not IsNothing(args) Then
            If Not IsNothing(args.Property) Then
                If args.Property.Name = "EstadoItem" Then
                    obj.EstadoItem = args.NewValue
                End If
            End If
        End If
    End Sub

#End Region

#Region "Inicialización"

    Public Sub New()
        Try
            InitializeComponent()

            acbItems.SearchText = ""

            mobjVM = CType(Me.LayoutRoot.Resources(STR_NOMBRE_VIEW_MODEL), BuscadorGenericoViewModel)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del buscador.", Me.ToString(), "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenerico_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim logBuscar As Boolean = False

        Try
            If Not Program.IsDesignMode() Then
                If Me.mobjVM.Inicializado = False Then
                    '// Validar si el control ha sido inicializado es la primera ejecución para asegurar que no se vuelvan a inicializar los controles
                    '   Se valida el valor de la propiedad en el VM porque cuando la propiedad en el control ha sido asignada por Binding se 
                    '   dispara en momento diferente a cuando se asigna un valor fijo. Si se asigna un valor fijo y se deja esta instrucción
                    '   se lanzaría dos veces la asignación de la propiedad IdItem del VM 
                    If Not Me.GetValue(IdItemDep) Is Nothing And Me.mobjVM.Buscando = False Then '// Esta propiedad se inicializa si se está buscando un Item
                        Me.mobjVM.IdItem = Me.GetValue(IdItemDep).ToString
                    ElseIf Not Me.GetValue(IdItemDep) Is Nothing Then
                        logBuscar = True
                        Me.mobjVM.consultarItems()
                    End If

                    Me.grDatosClt.Visibility = Visibility.Collapsed

                    If Me.mobjVM.IdItem.Equals(String.Empty) And Me.BuscarAlIniciar And logBuscar = False Then
                        Me.mobjVM.consultarItems()
                    End If

                    Me.mobjVM.Inicializado = True
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga inicial del buscador.", Me.ToString(), "BuscadorGenerico_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos controles"

    ''' <summary>
    ''' Este evento se dispara cuando el item activo cambia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub acbGenerico_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim objSeleccion As OYDUtilidades.BuscadorGenerico
        Try
            If Not IsNothing(CType(sender, RadAutoCompleteBox).SelectedItem) Then
                objSeleccion = CType(CType(CType(sender, RadAutoCompleteBox).SelectedItem, clsGenerico_Buscador).ItemBusqueda, OYDUtilidades.BuscadorGenerico)
                If objSeleccion Is Nothing Then
                    Me.grDatosClt.Visibility = Visibility.Collapsed
                Else
                    If Me.VerDetalle Then
                        Me.grDatosClt.Visibility = Visibility.Visible
                    End If

                    '// Desactivar indicador que permite identificar que el usuario digitó una condición de filtro
                    '   Solamente se modifica cuando entra por el else porque si se seleccionó una opción de la lista del autocomplete nunca puede ser nothing
                    '   porque debe ser un elemento de la lista.
                    mlogDigitandoFiltro = False
                End If
            Else
                Me.grDatosClt.Visibility = Visibility.Collapsed
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el item seleccionado.", Me.Name, "acbGenerico_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Evento que se dispara cuando el usuario da clic en el botón buscar, para ejecutar una búsqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mlogDigitandoFiltro = False
            Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
            Me.mobjVM.consultarItems()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar los items que cumplan con el filtro indicado.", Me.Name, "cmdBuscar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub acbItems_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Tab Then
                If Not IsNothing(Me.mobjVM.ItemSeleccionadoBuscador) Then
                    If LTrim(RTrim(Me.mobjVM.ItemSeleccionadoBuscador.ItemBusqueda.IdItem)) <> LTrim(RTrim(Me.acbItems.SearchText)) Then
                        Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
                        mlogDigitandoFiltro = False
                        Me.mobjVM.consultarItems()
                    End If
                Else
                    Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
                    mlogDigitandoFiltro = False
                    Me.mobjVM.consultarItems()
                End If
            End If
        Catch ex As Exception
            Me.cmdBuscar.IsEnabled = True
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar los items que cumplan con el filtro indicado.", Me.Name, "acbItems_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub acbItems_KeyUp(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                If Not IsNothing(Me.mobjVM.ItemSeleccionadoBuscador) Then
                    If LTrim(RTrim(Me.mobjVM.ItemSeleccionadoBuscador.ItemBusqueda.IdItem)) <> LTrim(RTrim(Me.acbItems.SearchText)) Then
                        Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
                        mlogDigitandoFiltro = False
                        Me.mobjVM.consultarItems()
                    End If
                Else
                    Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
                    mlogDigitandoFiltro = False
                    Me.mobjVM.consultarItems()
                End If
            ElseIf e.Key = Key.Back Then
                If Not IsNothing(e) Then
                    If Not IsNothing(e.OriginalSource) Then
                        Dim strTexto As String = CType(e.OriginalSource, System.Windows.Controls.TextBox).Text
                        If Me.acbItems.SearchText <> strTexto Then
                            Me.acbItems.SearchText = strTexto
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Me.cmdBuscar.IsEnabled = True
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar los items que cumplan con el filtro indicado.", Me.Name, "acbItems_KeyUp", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta cuando finaliza una carga de items a través del servicio web y es alimentada la lista del autocomplete
    ''' </summary>
    ''' <param name="plogNroItems">Número de items retornados</param>
    ''' <remarks></remarks>
    ''' 
    Private Sub mobjVM_CargaItemsCompleta(ByVal plogNroItems As Integer, ByVal plogBusquedaItemEspecifico As Boolean) Handles mobjVM.CargaItemsCompleta

        Me.cmdBuscar.IsEnabled = True
        Me.acbItems.IsEnabled = True

        Try
            If plogNroItems = 0 Then
                mostrarMensaje("No hay items que cumplan con la condición de búsqueda", Program.TituloSistema)
            Else
                If Not IsNothing(mobjVM.ListaBusquedaControl) Then
                    If mobjVM.ListaBusquedaControl.Count = 1 Then
                        'acbItems.SearchText = LTrim(RTrim(mobjVM.Items.FirstOrDefault.IdItem)) + "-" + LTrim(RTrim(mobjVM.Items.FirstOrDefault.Nombre))
                        mobjVM.ItemSeleccionadoBuscador = mobjVM.ListaBusquedaControl.FirstOrDefault
                        acbItems.IsDropDownOpen = False
                    Else
                        If plogBusquedaItemEspecifico = False Or Me._mlogBuscarAlIniciar Then
                            'If Me.acbItems.SearchText.Trim().Equals(String.Empty) Then
                            '    Me.acbItems.SearchText = STR_TEXTO_ABRIR_POPUP '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda - Asignar la constante y no el valor fijo " "
                            'End If
                            acbItems.Focus()
                            acbItems.IsDropDownOpen = True
                            acbItems.Populate(acbItems.SearchText)
                        End If
                    End If
                End If
            End If

            '// Desactivar indicador que permite identificar que el usuario digitó una condición de filtro
            mlogDigitandoFiltro = False
        Catch ex As Exception
            mlogDigitandoFiltro = False
            mostrarErrorAplicacion(Program.Usuario, "Falló la actualización del item seleccionado", Me.Name, "mobjVM_CargaItemsCompleta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se dispara cuando en el ViewModel se actualiza una propiedad
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub mobjVM_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles mobjVM.PropertyChanged
        Try
            If e.PropertyName.ToLower = "itemseleccionado" Then
                '// Si la propiedad cambio porque el usuario está digitando un nuevo filtro no se lanza el evento porque aún no se sabe que item se seleccionará.
                If mlogDigitandoFiltro = False Then
                    If mobjVM.ItemSeleccionado Is Nothing And mobjVM.Buscando = False Then
                        '// Si está en proceso de búsqueda (true) no se lanza el evento hasta que este termine
                        RaiseEvent itemAsignado(mobjVM.IdItem, Nothing)
                        RaiseEvent itemAsignadoControlOrigen(Me.Name, mobjVM.IdItem, Nothing)
                    ElseIf Not mobjVM.ItemSeleccionado Is Nothing Then
                        RaiseEvent itemAsignado(mobjVM.ItemSeleccionado.IdItem, mobjVM.ItemSeleccionado)
                        RaiseEvent itemAsignadoControlOrigen(Me.Name, mobjVM.ItemSeleccionado.IdItem, mobjVM.ItemSeleccionado)
                    End If
                Else
                    If Not mobjVM.ItemSeleccionado Is Nothing Then
                        RaiseEvent itemAsignado(mobjVM.ItemSeleccionado.IdItem, mobjVM.ItemSeleccionado)
                        RaiseEvent itemAsignadoControlOrigen(Me.Name, mobjVM.ItemSeleccionado.IdItem, mobjVM.ItemSeleccionado)
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Falló la sincronización del evento cuando cambio la propiedad " & e.PropertyName.ToLower, Me.Name, "mobjVM_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta cuando el usuario modifica el texto del filtro de búsqueda (texto del autocomplete).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub acbItems_TextChanged(sender As Object, e As EventArgs) Handles acbItems.SearchTextChanged
        Try
            If mlogDigitandoFiltro = False Then
                '// Activar el indicador que permite indicar que el usuario está digitando una nueva condición de búsqueda
                mlogDigitandoFiltro = True
                mobjVM.Buscando = False
                Me.IdItem = String.Empty
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CmbLimpiar_Click(sender As Object, e As RoutedEventArgs)
        Try
            LimpiarRegistro()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Falló la limpieza de los valores.", Me.Name, "CmbLimpiar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Metodos"
    Public Sub Limpiar()
        Try
            LimpiarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorGenerico", "BorrarItemChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub LimpiarRegistro()
        mobjVM.ListaBusquedaControl = Nothing
        mobjVM.ItemSeleccionadoBuscador = Nothing
        mobjVM.ItemSeleccionado = Nothing
        acbItems.SearchText = String.Empty
        RaiseEvent itemAsignado(mobjVM.IdItem, Nothing)
        RaiseEvent itemAsignadoControlOrigen(Me.Name, mobjVM.IdItem, Nothing)
    End Sub

    Private Sub AcbItems_KeyDown_1(sender As Object, e As KeyEventArgs)

    End Sub

#End Region

End Class

Public Class clsFiltroBuscadorGenerico
    Inherits FilteringBehavior

    Public Overrides Function FindMatchingItems(searchText As String, items As IList, escapedItems As IEnumerable(Of Object), textSearchPath As String, textSearchMode As TextSearchMode) As IEnumerable(Of Object)
        If Not String.IsNullOrEmpty(searchText) Then
            Dim results = items.OfType(Of clsGenerico_Buscador).Where(Function(x) IIf(IsNothing(x.ItemBusqueda.Clasificacion), "", x.ItemBusqueda.Clasificacion).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.Descripcion), "", x.ItemBusqueda.Descripcion).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.Nombre), "", x.ItemBusqueda.Nombre).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.IdItem), "", x.ItemBusqueda.IdItem).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper)
                                                                                     )
            Return results.Where(Function(x) Not escapedItems.Contains(x))
        Else
            Dim results = items.OfType(Of clsGenerico_Buscador)
            Return results.Where(Function(x) Not escapedItems.Contains(x))
        End If
    End Function

End Class

Public Class clsGenerico_Buscador
    Implements INotifyPropertyChanged

    Private _ItemBusqueda As OYDUtilidades.BuscadorGenerico
    Public Property ItemBusqueda() As OYDUtilidades.BuscadorGenerico
        Get
            Return _ItemBusqueda
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorGenerico)
            _ItemBusqueda = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ItemBusqueda"))
        End Set
    End Property

    Private _DescripcionBuscador As String
    Public Property DescripcionBuscador() As String
        Get
            Return _DescripcionBuscador
        End Get
        Set(ByVal value As String)
            _DescripcionBuscador = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionBuscador"))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
