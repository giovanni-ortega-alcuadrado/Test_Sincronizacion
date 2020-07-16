Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports System.ComponentModel
Imports Telerik.Windows.Controls

Partial Public Class BuscadorCliente
    Inherits UserControl

#Region "Eventos"

    ''' <summary>
    ''' JAPC20180926: Evento que se activa cuando se selecciona un comitente
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente buscado si es una búsqueda específica</param>
    ''' <param name="pobjComitente">Comitente seleccionado</param>
    ''' <remarks></remarks>
    Public Event personaAsignada(ByVal pstrIdComitente As String, ByVal pobjComitente As CPX_BuscadorPersonas)

#End Region

#Region "Constantes"

    Private Const STR_NOMBRE_VIEW_MODEL As String = "VM"
    Private Const STR_TEXTO_ABRIR_POPUP As String = " " '// JAPC20180926 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda

#End Region

#Region "Variables"

    Private mlogDigitandoFiltro As Boolean = False
    Private mlogPrimeraBusqueada As Boolean = True
    Public WithEvents mobjVM As BuscadorClienteViewModel ' JAPC20180926:Referencia al view model del control con eventos

#End Region

#Region "Propiedades"

    Private Shared ReadOnly RolPersonaDep As DependencyProperty = DependencyProperty.Register("RolPersona", GetType(String), GetType(BuscadorCliente), New PropertyMetadata("", New PropertyChangedCallback(AddressOf RolPersonaChanged)))


    Private _mlogVerDetalle As Boolean = False
    ''' <summary>
    ''' JAPC20180926: Indica si en la parte inferior del buscador se depliega el detalle del elemento seleccionado
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
    ''' JAPC20180926: Indica si al iniciar el control lanza una consulta de comitentes. Solamente aplica si IdComitente no se envía
    ''' </summary>
    Public Property BuscarAlIniciar As Boolean
        Get
            Return (_mlogBuscarAlIniciar)
        End Get
        Set(ByVal value As Boolean)
            _mlogBuscarAlIniciar = value
        End Set
    End Property

    '' <summary>
    '' JAPC20180926: persona seleccionada en el buscador
    '' </summary>
    Public ReadOnly Property personaActiva() As CPX_BuscadorPersonas
        Get
            Return (mobjVM.PersonaSeleccionada)
        End Get
    End Property



    ''' <summary>
    ''' JAPC20180926: rol de la persona para identificar la busqueda que debe realizar
    ''' </summary>
    ''' <returns></returns>
    Public Property RolPersona As String
        Get
            Return CStr(GetValue(RolPersonaDep))
        End Get
        Set(ByVal value As String)
            SetValue(RolPersonaDep, value)
        End Set
    End Property



    ''' <summary>
    ''' JAPC20180926: metodo para identificar cuando el rol de la persona cambie y enviarlo al view model para que cambie consulta 
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="e"></param>
    Private Shared Sub RolPersonaChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.TipoPersona = obj.RolPersona
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "RolPersonaChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


#End Region


#Region "Inicialización"

    Public Sub New()
        Try
            InitializeComponent()

            acbClientes.SearchText = ""

            mobjVM = CType(Me.LayoutRoot.Resources(STR_NOMBRE_VIEW_MODEL), BuscadorClienteViewModel)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del buscador de clientes.", Me.ToString(), "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20180926: metodo que maneja loaded del control para realizar la busqueda inicial
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BuscadorPersona_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim logBuscar As Boolean = False
        Try
            If Not Program.IsDesignMode() Then
                grDatosClt.Visibility = Visibility.Collapsed
                If Me.mobjVM.Inicializado = False Then
                    If Not Me.GetValue(RolPersonaDep) Is Nothing Then '// En el VM la propiedad IdComitente se inicializa en ""
                        Me.mobjVM.TipoPersona = Me.GetValue(RolPersonaDep).ToString
                        If Me._mlogBuscarAlIniciar Then
                            logBuscar = True
                            Me.mobjVM.consultarPersonas()
                        End If
                    End If

                    Me.mobjVM.Inicializado = True
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga inicial del buscador de clientes.", Me.ToString(), "BuscadorCliente_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos controles"

    '' <summary>
    '' JAPC20180926: Este evento se dispara cuando la persona activo cambia
    '' </summary>
    '' <param name="sender"></param>
    '' <param name="e"></param>
    '' <remarks></remarks>
    '' 
    Private Sub acbPersona_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim objSeleccion As CPX_BuscadorPersonas
        Try
            If Not IsNothing(CType(sender, RadAutoCompleteBox).SelectedItem) Then
                objSeleccion = CType(CType(CType(sender, RadAutoCompleteBox).SelectedItem, clsClientes_Buscador).ItemBusqueda, CPX_BuscadorPersonas)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la persona seleccionado.", Me.Name, "acbPersona_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' JAPC20180926: Evento que se dispara cuando el usuario da clic en el botón buscar, para ejecutar una búsqueda de personas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
            Me.mobjVM.consultarPersonas()
        Catch ex As Exception
            Me.cmdBuscar.IsEnabled = True
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar personas que cumplan con el filtro indicado.", Me.Name, "cmdBuscar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20180926: Este evento se ejecuta cuando el usuario modifica el texto del filtro de búsqueda (texto del autocomplete).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub acbPersonas_SearchTextChanged(sender As Object, e As EventArgs)
        Try
            If mlogDigitandoFiltro = False Then
                '// Activar el indicador que permite indicar que el usuario está digitando una nueva condición de búsqueda
                mlogDigitandoFiltro = True
                mobjVM.Buscando = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el cambio de texto de filtro", Me.Name, "acbPersonas_SearchTextChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20180926: Evento teclado  cuando digitan en buscador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub acbPersonas_KeyDown(sender As Object, e As KeyEventArgs)
        Try

            If Not IsNothing(Me.mobjVM.PersonaSeleccionada) Then
                If LTrim(RTrim(Me.mobjVM.PersonaSeleccionada.intID)) <> LTrim(RTrim(Me.acbClientes.SearchText)) Then 'IdComitente
                    Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
                    Me.mobjVM.consultarPersonas()
                End If
            Else
                Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
                Me.mobjVM.consultarPersonas()
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar personas que cumplan con el filtro indicado.", Me.Name, "acbPersonas_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20180926: Evento teclado  cuando digitan en buscador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub acbPersona_KeyUp(sender As Object, e As KeyEventArgs)
        Try
            If Not IsNothing(Me.mobjVM.PersonaSeleccionada) Then
                If LTrim(RTrim(Me.mobjVM.PersonaSeleccionada.intID)) <> LTrim(RTrim(Me.acbClientes.SearchText)) Then 'IdComitente
                    Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
                    Me.mobjVM.consultarPersonas()
                End If
            Else
                Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
                Me.mobjVM.consultarPersonas()
            End If

            If Not IsNothing(e) Then
                If Not IsNothing(e.OriginalSource) Then
                    Dim strTexto As String = CType(e.OriginalSource, System.Windows.Controls.TextBox).Text
                    If Me.acbClientes.SearchText <> strTexto Then
                        Me.acbClientes.SearchText = strTexto
                    End If
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar personas que cumplan con el filtro indicado.", Me.Name, "acbPersona_KeyUp", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20180926: Este evento se ejecuta cuando finaliza una carga de cliente a través del servicio web y es alimentada la lista del autocomplete
    ''' </summary>
    ''' <param name="plogNroComitentes">Número de clientes retornados</param>
    ''' <remarks></remarks>    
    Private Sub mobjVM_CargaPersonaCompleta(ByVal plogNroComitentes As Integer, ByVal plogBusquedaComitenteEspecifico As Boolean) Handles mobjVM.CargaPersonasCompleta

        Me.cmdBuscar.IsEnabled = True
        Me.acbClientes.IsEnabled = True

        Try
            If plogNroComitentes = 0 Then
                mostrarMensaje("No hay personas que cumplan con la condición de búsqueda", Program.TituloSistema)
            Else
                'Descripción se adiciona la condición para cargar el cliente por defecto si solo se encuentra un registro.
                If Not IsNothing(mobjVM.ListaBusquedaControl) Then
                    If mobjVM.ListaBusquedaControl.Count = 1 Then
                        mobjVM.ItemSeleccionadoBuscador = mobjVM.ListaBusquedaControl.FirstOrDefault
                        acbClientes.IsDropDownOpen = False
                    Else
                        If plogBusquedaComitenteEspecifico = False Or Me._mlogBuscarAlIniciar Then
                            acbClientes.Focus()
                            acbClientes.IsDropDownOpen = True
                            acbClientes.Populate(acbClientes.SearchText)
                        End If
                    End If
                End If
            End If

            '// Desactivar indicador que permite identificar que el usuario digitó una condición de filtro
            mlogDigitandoFiltro = False
        Catch ex As Exception
            mlogDigitandoFiltro = False
            mostrarErrorAplicacion(Program.Usuario, "Falló la actualización de la persona seleccionada", Me.Name, "mobjVM_CargaPersonaCompleta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' JAPC20180926: Este evento se dispara cuando en el ViewModel se actualiza una propiedad
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub mobjVM_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles mobjVM.PropertyChanged
        Try
            If e.PropertyName = "PersonaSeleccionada" Then
                If Not mobjVM.PersonaSeleccionada Is Nothing Then
                    RaiseEvent personaAsignada(mobjVM.PersonaSeleccionada.intID, mobjVM.PersonaSeleccionada)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Falló la sincronización del evento cuando cambio la propiedad " & e.PropertyName.ToLower, Me.Name, "mobjVM_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Metodos publicos"

    Public Sub BorrarPersonaSeleccionada()
        Try
            acbClientes.SearchText = String.Empty
            acbClientes.SelectedItem = Nothing
            mobjVM.PersonaSeleccionada = Nothing
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Falló la limpieza del control. ", Me.Name, "BorrarPersonaSeleccionada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class



''' <summary>
''' JAPC20180926: clase para controlador de filtros del radautocompletebox y poder realizar filtro bajo diferentes condiciones con la clase  clsClientes_Buscador
''' </summary>
Public Class clsFiltroBuscadorClientes
    Inherits FilteringBehavior

    Public Overrides Function FindMatchingItems(searchText As String, items As IList, escapedItems As IEnumerable(Of Object), textSearchPath As String, textSearchMode As TextSearchMode) As IEnumerable(Of Object)
        If Not String.IsNullOrEmpty(searchText) Then
            Dim results = items.OfType(Of clsClientes_Buscador).Where(Function(x) x.ItemBusqueda.strNombre.ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.strNroDocumento), "", x.ItemBusqueda.strNroDocumento).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.strNombre1), "", x.ItemBusqueda.strNombre1).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.strNombre2), "", x.ItemBusqueda.strNombre2).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.strApellido1), "", x.ItemBusqueda.strApellido1).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.strApellido2), "", x.ItemBusqueda.strApellido2).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.strCodigoOyD), "", x.ItemBusqueda.strCodigoOyD).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or x.ItemBusqueda.intID.Value.ToString(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) 'IdComitente
                                                                                     )
            Return results.Where(Function(x) Not escapedItems.Contains(x))
        Else
            Dim results = items.OfType(Of clsClientes_Buscador)
            Return results.Where(Function(x) Not escapedItems.Contains(x))
        End If
    End Function

End Class



''' <summary>
''' JAPC20180926: clase para manejar el item de busqueda del control y la descripcion del buscador
''' </summary>
Public Class clsClientes_Buscador
    Implements INotifyPropertyChanged

    Private _ItemBusqueda As CPX_BuscadorPersonas
    Public Property ItemBusqueda() As CPX_BuscadorPersonas
        Get
            Return _ItemBusqueda
        End Get
        Set(ByVal value As CPX_BuscadorPersonas)
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
