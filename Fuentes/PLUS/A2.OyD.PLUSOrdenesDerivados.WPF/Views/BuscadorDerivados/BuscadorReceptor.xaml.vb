Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports System.ComponentModel

Partial Public Class BuscadorReceptor
    Inherits UserControl

#Region "Eventos"

    ''' <summary>
    ''' Evento que se dispara cuando se selecciona un elemento de la lista
    ''' </summary>
    ''' <param name="pstrIdItem">Código del elemento buscado cuando se trata de una búsqueda específica</param>
    ''' <param name="pobjItem">Elemento seleccionado</param>
    ''' <remarks></remarks>
    Public Event itemAsignado(ByVal pstrIdItem As String, ByVal pobjItem As OyDPLUSOrdenesDerivados.ReceptoresBusqueda)

#End Region

#Region "Constantes"

    Private Const STR_NOMBRE_VIEW_MODEL As String = "VM"
    Private Const STR_TEXTO_ABRIR_POPUP As String = " " '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda

#End Region

#Region "Variables"

    Private mlogDigitandoFiltro As Boolean = False
    Private mlogPrimeraBusqueada As Boolean = True
    Public WithEvents mobjVM As BuscadorReceptorViewModel ' Referencia al view model del control

#End Region

#Region "Propiedades"

    Private Shared ReadOnly BorrarItemDep As DependencyProperty = DependencyProperty.Register("BorrarItem", GetType(Boolean), GetType(BuscadorReceptor), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf BorrarItemChanged)))

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
    Public ReadOnly Property ItemActivo() As OyDPLUSOrdenesDerivados.ReceptoresBusqueda
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
            Dim obj As BuscadorReceptor = DirectCast(d, BuscadorReceptor)

            If Not IsNothing(obj.BorrarItem) Then
                If obj.BorrarItem Then
                    If Not IsNothing(obj.mobjVM) Then
                        If Not IsNothing(obj.mobjVM.Items) Then
                            obj.mobjVM.Items.Clear()
                        End If
                        obj.mobjVM.ItemSeleccionado = Nothing
                        obj.acbItems.SearchText = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorReceptor", "BorrarItemChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    End Sub

#End Region

#Region "Inicialización"

    Public Sub New()
        Try
            InitializeComponent()

            acbItems.SearchText = ""

            mobjVM = CType(Me.LayoutRoot.Resources(STR_NOMBRE_VIEW_MODEL), BuscadorReceptorViewModel)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del buscador.", Me.ToString(), "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorReceptor_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim logBuscar As Boolean = False

        Try
            If Not Program.IsDesignMode() Then
                If Me.mobjVM.Inicializado = False Then
                    '// Validar si el control ha sido inicializado es la primera ejecución para asegurar que no se vuelvan a inicializar los controles
                    '   Se valida el valor de la propiedad en el VM porque cuando la propiedad en el control ha sido asignada por Binding se 
                    '   dispara en momento diferente a cuando se asigna un valor fijo. Si se asigna un valor fijo y se deja esta instrucción
                    '   se lanzaría dos veces la asignación de la propiedad IdItem del VM 
                    Me.grDatosClt.Visibility = Visibility.Collapsed

                    If Me.BuscarAlIniciar And logBuscar = False Then
                        Me.mobjVM.consultarItems()
                    End If

                    Me.mobjVM.Inicializado = True
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga inicial del buscador.", Me.ToString(), "BuscadorReceptor_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Condiciones filtro"

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
        Dim objSeleccion As OyDPLUSOrdenesDerivados.ReceptoresBusqueda
        Try
            objSeleccion = CType(CType(acbItems.SelectedItem, clsBuscarReceptor_Buscador).ItemBusqueda, OyDPLUSOrdenesDerivados.ReceptoresBusqueda)
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
            Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
            Me.mobjVM.consultarItems()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar los items que cumplan con el filtro indicado.", Me.Name, "cmdBuscar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub acbItems_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Tab Then
                If Not IsNothing(Me.mobjVM.ItemSeleccionado) Then
                    If LTrim(RTrim(Me.mobjVM.ItemSeleccionado.IDComercial)) <> LTrim(RTrim(Me.acbItems.SearchText)) Then
                        Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
                        Me.mobjVM.consultarItems()
                    End If
                Else
                    Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
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
                If Not IsNothing(Me.mobjVM.ItemSeleccionado) Then
                    If LTrim(RTrim(Me.mobjVM.ItemSeleccionado.IDComercial)) <> LTrim(RTrim(Me.acbItems.SearchText)) Then
                        Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
                        Me.mobjVM.consultarItems()
                    End If
                Else
                    Me.mobjVM.CondicionFiltro = Me.acbItems.SearchText
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
                If Not IsNothing(mobjVM.Items) Then
                    If mobjVM.Items.Count = 1 Then
                        acbItems.SearchText = LTrim(RTrim(mobjVM.Items.FirstOrDefault.IDComercial)) + "-" + LTrim(RTrim(mobjVM.Items.FirstOrDefault.Nombre))
                        mobjVM.ItemSeleccionado = mobjVM.Items.FirstOrDefault
                        acbItems.IsDropDownOpen = False
                    Else
                        If plogBusquedaItemEspecifico = False Or Me._mlogBuscarAlIniciar Then
                            If Me.acbItems.SearchText.Trim().Equals(String.Empty) Then
                                Me.acbItems.SearchText = STR_TEXTO_ABRIR_POPUP '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda - Asignar la constante y no el valor fijo " "
                            End If
                            acbItems.Focus()
                            acbItems.IsDropDownOpen = True
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
                    If Not mobjVM.ItemSeleccionado Is Nothing And mobjVM.Buscando Then
                        RaiseEvent itemAsignado(mobjVM.ItemSeleccionado.IDComercial, mobjVM.ItemSeleccionado)
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
    'Private Sub acbItems_TextChanged(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles acbItems.TextChanged
    '    Try
    '        If mlogDigitandoFiltro = False Then
    '            '// Activar el indicador que permite indicar que el usuario está digitando una nueva condición de búsqueda
    '            mlogDigitandoFiltro = True
    '            mobjVM.Buscando = False
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

#End Region


End Class

Public Class clsFiltroBuscadorReceptor
    Inherits FilteringBehavior

    Public Overrides Function FindMatchingItems(searchText As String, items As IList, escapedItems As IEnumerable(Of Object), textSearchPath As String, textSearchMode As TextSearchMode) As IEnumerable(Of Object)
        If Not String.IsNullOrEmpty(searchText) Then
            Dim results = items.OfType(Of clsBuscarReceptor_Buscador).Where(Function(x) IIf(IsNothing(x.ItemBusqueda.CodigoComercial), "", x.ItemBusqueda.CodigoComercial).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.Nombre), "", x.ItemBusqueda.Nombre).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper)
                                                                                     )
            Return results.Where(Function(x) Not escapedItems.Contains(x))
        Else
            Dim results = items.OfType(Of clsBuscarReceptor_Buscador)
            Return results.Where(Function(x) Not escapedItems.Contains(x))
        End If
    End Function

End Class

Public Class clsBuscarReceptor_Buscador
    Implements INotifyPropertyChanged

    Private _ItemBusqueda As A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesDerivados.ReceptoresBusqueda
    Public Property ItemBusqueda() As A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesDerivados.ReceptoresBusqueda
        Get
            Return _ItemBusqueda
        End Get
        Set(ByVal value As A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesDerivados.ReceptoresBusqueda)
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