Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports System.ComponentModel
Imports Telerik.Windows.Controls

Partial Public Class BuscadorCliente
    Inherits UserControl

#Region "Eventos"

    ''' <summary>
    ''' Evento que se dispara cuando se selecciona un comitente
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente buscado si es una búsqueda específica</param>
    ''' <param name="pobjComitente">Comitente seleccionado</param>
    ''' <remarks></remarks>
    Public Event comitenteAsignado(ByVal pstrIdComitente As String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
    Public Event comitenteAsignadoControlOrigen(ByVal pstrControlOrigen As String, ByVal pstrIdComitente As String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)

#End Region

#Region "Constantes"

    Private Const STR_NOMBRE_VIEW_MODEL As String = "VM"
    Private Const STR_TEXTO_ABRIR_POPUP As String = " " '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda

#End Region

#Region "Variables"

    Private mlogDigitandoFiltro As Boolean = False
    Private mlogPrimeraBusqueada As Boolean = True
    Public WithEvents mobjVM As BuscadorClienteViewModel ' Referencia al view model del control

#End Region

#Region "Propiedades"

    Private Shared IdComitenteDep As DependencyProperty = DependencyProperty.Register("IdComitente", GetType(String), GetType(BuscadorCliente), New PropertyMetadata(AddressOf CambioPropiedadDep))
    Private Shared EstadoComitenteDep As DependencyProperty = DependencyProperty.Register("EstadoComitente", GetType(BuscadorClienteViewModel.EstadosComitente), GetType(BuscadorCliente), New PropertyMetadata(AddressOf CambioPropiedadDep))
    Private Shared TipoVinculacionDep As DependencyProperty = DependencyProperty.Register("TipoVinculacion", GetType(BuscadorClienteViewModel.TiposVinculacion), GetType(BuscadorCliente), New PropertyMetadata(AddressOf CambioPropiedadDep))
    'Propiedades dependientes para el manejo del cambio de receptor, tipo negocio y tipo producto.
    Private Shared ReadOnly IDReceptorDep As DependencyProperty = DependencyProperty.Register("IDReceptor", GetType(String), GetType(BuscadorCliente), New PropertyMetadata("", New PropertyChangedCallback(AddressOf IDReceptorChanged)))
    Private Shared ReadOnly TipoNegocioDep As DependencyProperty = DependencyProperty.Register("TipoNegocio", GetType(String), GetType(BuscadorCliente), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoNegocioChanged)))
    Private Shared ReadOnly TipoProductoDep As DependencyProperty = DependencyProperty.Register("TipoProducto", GetType(String), GetType(BuscadorCliente), New PropertyMetadata("", New PropertyChangedCallback(AddressOf TipoProductoChanged)))
    Private Shared ReadOnly PerfilRiesgoDep As DependencyProperty = DependencyProperty.Register("PerfilRiesgo", GetType(String), GetType(BuscadorCliente), New PropertyMetadata("", New PropertyChangedCallback(AddressOf PerfilRiesgoChanged)))
    Private Shared ReadOnly EditandoDep As DependencyProperty = DependencyProperty.Register("Editando", GetType(Boolean), GetType(BuscadorCliente), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf EditandoChanged)))
    Private Shared ReadOnly BorrarClienteDep As DependencyProperty = DependencyProperty.Register("BorrarCliente", GetType(Boolean), GetType(BuscadorCliente), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf BorrarClienteChanged)))
    Private Shared ReadOnly ClienteBuscarDep As DependencyProperty = DependencyProperty.Register("ClienteBuscar", GetType(String), GetType(BuscadorCliente), New PropertyMetadata("", New PropertyChangedCallback(AddressOf ClienteBuscarChanged)))
    Private Shared ReadOnly IDCompaniaDep As DependencyProperty = DependencyProperty.Register("IDCompania", GetType(Nullable(Of Integer)), GetType(BuscadorCliente), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf IDCompaniaChanged)))
    Private Shared ReadOnly ConFiltroDep As DependencyProperty = DependencyProperty.Register("ConFiltro", GetType(Boolean), GetType(BuscadorCliente), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf ConFiltroChanged)))
    Private Shared ReadOnly filtroAdicional1Dep As DependencyProperty = DependencyProperty.Register("filtroAdicional1", GetType(String), GetType(BuscadorCliente), New PropertyMetadata("", New PropertyChangedCallback(AddressOf filtroAdicional1Changed)))
    Private Shared ReadOnly filtroAdicional2Dep As DependencyProperty = DependencyProperty.Register("filtroAdicional2", GetType(String), GetType(BuscadorCliente), New PropertyMetadata("", New PropertyChangedCallback(AddressOf filtroAdicional2Changed)))
    Private Shared ReadOnly filtroAdicional3Dep As DependencyProperty = DependencyProperty.Register("filtroAdicional3", GetType(String), GetType(BuscadorCliente), New PropertyMetadata("", New PropertyChangedCallback(AddressOf filtroAdicional3Changed)))
    Private Shared ReadOnly MostrarLimpiarDep As DependencyProperty = DependencyProperty.Register("MostrarLimpiar", GetType(Boolean), GetType(BuscadorCliente), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf MostrarLimpiarChanged)))

    ''' <summary>
    ''' Id del comitente que se busca si se trata de una búsqueda específica
    ''' </summary>
    Public Property IdComitente As String
        Get
            Return (CType(Me.GetValue(IdComitenteDep), String))
        End Get
        Set(ByVal value As String)
            Me.SetValue(IdComitenteDep, value)
            Me.mobjVM.IdComitente = value
        End Set
    End Property

    ''' <summary>
    ''' Estado de los comitentes que se buscan (activos, inactivos, bloqueados, todos)
    ''' </summary>
    Public Property EstadoComitente As BuscadorClienteViewModel.EstadosComitente
        Get
            Return (CType(Me.GetValue(EstadoComitenteDep), BuscadorClienteViewModel.EstadosComitente))
        End Get
        Set(ByVal value As BuscadorClienteViewModel.EstadosComitente)
            Me.SetValue(EstadoComitenteDep, value)
            Me.mobjVM.EstadoComitente = value
        End Set
    End Property

    ''' <summary>
    ''' Tipo de vinculación de los comitentes que se buscan (clientes, ordenantes, todos)
    ''' </summary>
    Public Property TipoVinculacion As BuscadorClienteViewModel.TiposVinculacion
        Get
            Return (CType(Me.GetValue(TipoVinculacionDep), BuscadorClienteViewModel.TiposVinculacion))
        End Get
        Set(ByVal value As BuscadorClienteViewModel.TiposVinculacion)
            Me.SetValue(TipoVinculacionDep, value)
            Me.mobjVM.TipoVinculacion = value
        End Set
    End Property

    Private _mstrAgrupamiento As String = String.Empty
    ''' <summary>
    ''' Permite indicar otra condición de búsqueda. No tiene una definición particular.
    ''' </summary>
    Public Property Agrupamiento As String
        Get
            Return (_mstrAgrupamiento)
        End Get
        Set(ByVal value As String)
            _mstrAgrupamiento = value
            Me.mobjVM.Agrupamiento = value
        End Set
    End Property

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
    ''' Especie seleccionada en el buscador
    ''' </summary>
    Public ReadOnly Property ComitenteActivo() As OYDUtilidades.BuscadorClientes
        Get
            Return (mobjVM.ComitenteSeleccionado)
        End Get
    End Property

    Private _mlogCargarClientesRestriccion As Boolean = False
    ''' <summary>
    ''' Indica si se muestra la opción de cargar clientes con las restricciones de oyd plus
    ''' </summary>
    Public Property CargarClientesRestriccion As Boolean
        Get
            Return (_mlogCargarClientesRestriccion)
        End Get
        Set(ByVal value As Boolean)
            _mlogCargarClientesRestriccion = value
            Me.mobjVM.CargarClientesRestricciones = value
            acbClientes.SearchText = " "
        End Set
    End Property

    Private _mlogCargarClientesTercero As Visibility = Visibility.Collapsed
    ''' <summary>
    ''' Indica si se muestra la opción de cargar clientes Terceros
    ''' </summary>
    Public Property CargarClientesTercero As Visibility
        Get
            Return (_mlogCargarClientesTercero)
        End Get
        Set(ByVal value As Visibility)
            _mlogCargarClientesTercero = value
            Me.mobjVM.MostrarClientesTercero = value
        End Set
    End Property

    Private _mlogCargarClientesXTipoProductoPerfil As Boolean = False
    ''' <summary>
    ''' Indica si se filtran los clientes x tipo de producto y perfil.
    ''' </summary>
    Public Property CargarClientesXTipoProductoPerfil As Boolean
        Get
            Return (_mlogCargarClientesXTipoProductoPerfil)
        End Get
        Set(ByVal value As Boolean)
            _mlogCargarClientesXTipoProductoPerfil = value
            Me.mobjVM.CargarClienteXTipoProductoPerfil = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si se realiza la carga de clientes dependiendo de un receptor en especifico
    ''' </summary>
    Public Property IDReceptor As String
        Get
            Return CStr(GetValue(IDReceptorDep))
        End Get
        Set(ByVal value As String)
            SetValue(IDReceptorDep, value)
        End Set
    End Property

    Private Shared Sub IDReceptorChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.IDReceptor = obj.IDReceptor
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "IDReceptorChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Indica el tipo de negocio por el cual se realizara el filtro de clientes.
    ''' </summary>
    Public Property TipoNegocio As String
        Get
            Return CStr(GetValue(TipoNegocioDep))
        End Get
        Set(ByVal value As String)
            SetValue(TipoNegocioDep, value)
        End Set
    End Property

    Private Shared Sub TipoNegocioChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.TipoNegocio = obj.TipoNegocio
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "TipoNegocioChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Indica el tipo de producto por el cual se realizara el filtro de clientes.
    ''' </summary>
    Public Property TipoProducto As String
        Get
            Return CStr(GetValue(TipoProductoDep))
        End Get
        Set(ByVal value As String)
            SetValue(TipoProductoDep, value)
        End Set
    End Property

    Private Shared Sub TipoProductoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.TipoProducto = obj.TipoProducto
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "TipoProductoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Property PerfilRiesgo As String
        Get
            Return CStr(GetValue(PerfilRiesgoDep))
        End Get
        Set(ByVal value As String)
            SetValue(PerfilRiesgoDep, value)
        End Set
    End Property

    Private Shared Sub PerfilRiesgoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.PerfilRiesgo = obj.PerfilRiesgo
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "TipoProductoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' Saber si se encuentra en modo edición para lanzar la consulta de especies.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 21 de septiembre del 2012.
    Public Property Editando As Boolean
        Get
            Return CBool(GetValue(EditandoDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(EditandoDep, value)
        End Set
    End Property

    Private Shared Sub EditandoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.Editando = obj.Editando
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "EditandoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Borrar los datos de la especie seleccionada.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 21 de septiembre del 2012.
    ''' </summary>
    Public Property BorrarCliente As Boolean
        Get
            Return CBool(GetValue(BorrarClienteDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(BorrarClienteDep, value)
        End Set
    End Property

    Private Shared Sub BorrarClienteChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.BorrarCliente) Then
                If obj.BorrarCliente Then
                    If Not IsNothing(obj.mobjVM) Then
                        If Not IsNothing(obj.mobjVM.ListaBusquedaControl) Then
                            obj.mobjVM.ListaBusquedaControl.Clear()
                        End If
                        obj.mobjVM.ItemSeleccionadoBuscador = Nothing
                        obj.acbClientes.SearchText = String.Empty
                        obj.mobjVM.ClienteTercero = False
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorEspecie", "BorrarClienteChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos del cliente seleccionado.
    ''' Desarrollado por Juan David Correa
    ''' Fecha 21 de octubre del 2012.
    ''' </summary>
    Public Property ClienteBuscar As String
        Get
            Return CStr(GetValue(ClienteBuscarDep))
        End Get
        Set(ByVal value As String)
            SetValue(ClienteBuscarDep, value)
        End Set
    End Property

    Private Shared Sub ClienteBuscarChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not String.IsNullOrEmpty(obj.ClienteBuscar) Then
                If Not IsNothing(obj.mobjVM) Then
                    obj.acbClientes.SearchText = LTrim(RTrim(obj.ClienteBuscar))
                    obj.acbClientes.IsDropDownOpen = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorClientes", "ClienteBuscarChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _mlogExcluirCodigosCompania As Boolean = False
    ''' <summary>
    ''' Indica si se muestra la opción de cargar clientes con las restricciones de oyd plus
    ''' </summary>
    Public Property ExcluirCodigosCompania As Boolean
        Get
            Return (_mlogExcluirCodigosCompania)
        End Get
        Set(ByVal value As Boolean)
            _mlogExcluirCodigosCompania = value
            Me.mobjVM.ExcluirCodigosCompania = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si se realiza la carga de clientes dependiendo de un receptor en especifico
    ''' </summary>
    Public Property IDCompania As Nullable(Of Integer)
        Get
            Return CType(GetValue(IDCompaniaDep), Nullable(Of Integer))
        End Get
        Set(ByVal value As Nullable(Of Integer))
            SetValue(IDCompaniaDep, value)
        End Set
    End Property

    Private Shared Sub IDCompaniaChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.intIDCompania = obj.IDCompania
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "IDCompaniaChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Property ConFiltro As Boolean
        Get
            Return CBool(GetValue(ConFiltroDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConFiltroDep, value)
        End Set
    End Property

    Private Shared Sub ConFiltroChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.ConFiltro = obj.ConFiltro
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "ConFiltroChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Public Property filtroAdicional1 As String
        Get
            Return CStr(GetValue(filtroAdicional1Dep))
        End Get
        Set(ByVal value As String)
            SetValue(filtroAdicional1Dep, value)
        End Set
    End Property

    Private Shared Sub filtroAdicional1Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.filtroAdicional1 = obj.filtroAdicional1
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "filtroAdicional1Changed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Public Property filtroAdicional2 As String
        Get
            Return CStr(GetValue(filtroAdicional2Dep))
        End Get
        Set(ByVal value As String)
            SetValue(filtroAdicional2Dep, value)
        End Set
    End Property

    Private Shared Sub filtroAdicional2Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.filtroAdicional2 = obj.filtroAdicional2
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "filtroAdicional2Changed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Public Property filtroAdicional3 As String
        Get
            Return CStr(GetValue(filtroAdicional3Dep))
        End Get
        Set(ByVal value As String)
            SetValue(filtroAdicional3Dep, value)
        End Set
    End Property

    Private Shared Sub filtroAdicional3Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.filtroAdicional3 = obj.filtroAdicional3
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "filtroAdicional3Changed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            Dim obj As BuscadorCliente = DirectCast(d, BuscadorCliente)

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
        Dim obj As BuscadorCliente = DirectCast(sender, BuscadorCliente)
        If Not IsNothing(args) Then
            If Not IsNothing(args.Property) Then
                If args.Property.Name = "EstadoComitente" Then
                    obj.EstadoComitente = args.NewValue
                ElseIf args.Property.Name = "TipoVinculacion" Then
                    obj.mobjVM.TipoVinculacion = args.NewValue
                End If
            End If
        End If
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

    Private Sub BuscadorCliente_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim logBuscar As Boolean = False
        Try
            If Not Program.IsDesignMode() Then
                If Me.mobjVM.Inicializado = False Then
                    '// Validar si el control ha sido inicializado es la primera ejecución para asegurar que no se vuelvan a inicializar los controles
                    '   Se valida el valor de la propiedad en el VM porque cuando la propiedad en el control ha sido asignada por Binding se 
                    '   dispara en momento diferente a cuando se asigna un valor fijo. Si se asigna un valor fijo y se deja esta instrucción
                    '   se lanzaría dos veces la asignación de la propiedad IdItem del VM 
                    If Not Me.GetValue(IdComitenteDep) Is Nothing And Me.mobjVM.IdComitente.Equals(String.Empty) Then '// En el VM la propiedad IdComitente se inicializa en ""
                        Me.mobjVM.IdComitente = Me.GetValue(IdComitenteDep).ToString
                    ElseIf Not Me.GetValue(IdComitenteDep) Is Nothing Then
                        logBuscar = True
                        Me.mobjVM.consultarComitentes()
                    End If
                    Me.grDatosClt.Visibility = Visibility.Collapsed

                    If Me.mobjVM.IdComitente.Equals(String.Empty) And Me._mlogBuscarAlIniciar And logBuscar = False Then
                        Me.mobjVM.consultarComitentes()
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

    ''' <summary>
    ''' Este evento se dispara cuando el cliente activo cambia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub acbClientes_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        Dim objSeleccion As OYDUtilidades.BuscadorClientes
        Try
            If Not IsNothing(CType(sender, RadAutoCompleteBox).SelectedItem) Then
                objSeleccion = CType(CType(CType(sender, RadAutoCompleteBox).SelectedItem, clsClientes_Buscador).ItemBusqueda, OYDUtilidades.BuscadorClientes)
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
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el cliente seleccionado.", Me.Name, "acbClientes_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Evento que se dispara cuando el usuario da clic en el botón buscar, para ejecutar una búsqueda de clientes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mlogDigitandoFiltro = False
            Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
            Me.mobjVM.consultarComitentes()
        Catch ex As Exception
            Me.cmdBuscar.IsEnabled = True
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar clientes que cumplan con el filtro indicado.", Me.Name, "cmdBuscar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta cuando el usuario modifica el texto del filtro de búsqueda (texto del autocomplete).
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub acbClientes_SearchTextChanged(sender As Object, e As EventArgs)
        Try
            If mlogDigitandoFiltro = False Then
                '// Activar el indicador que permite indicar que el usuario está digitando una nueva condición de búsqueda
                mlogDigitandoFiltro = True
                mobjVM.Buscando = False
                Me.IdComitente = String.Empty
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub acbClientes_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = Key.Tab Then
                If Not IsNothing(Me.mobjVM.ItemSeleccionadoBuscador) Then
                    If LTrim(RTrim(Me.mobjVM.ItemSeleccionadoBuscador.ItemBusqueda.IdComitente)) <> LTrim(RTrim(Me.acbClientes.SearchText)) Then
                        Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
                        mlogDigitandoFiltro = False
                        Me.mobjVM.consultarComitentes()
                    End If
                Else
                    Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
                    mlogDigitandoFiltro = False
                    Me.mobjVM.consultarComitentes()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar clientes que cumplan con el filtro indicado.", Me.Name, "acbClientes_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub acbClientes_KeyUp(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                If Not IsNothing(Me.mobjVM.ItemSeleccionadoBuscador) Then
                    If LTrim(RTrim(Me.mobjVM.ItemSeleccionadoBuscador.ItemBusqueda.IdComitente)) <> LTrim(RTrim(Me.acbClientes.SearchText)) Then
                        Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
                        mlogDigitandoFiltro = False
                        Me.mobjVM.consultarComitentes()
                    End If
                Else
                    Me.mobjVM.CondicionFiltro = Me.acbClientes.SearchText
                    mlogDigitandoFiltro = False
                    Me.mobjVM.consultarComitentes()
                End If
            ElseIf e.Key = Key.Back Then
                If Not IsNothing(e) Then
                    If Not IsNothing(e.OriginalSource) Then
                        Dim strTexto As String = CType(e.OriginalSource, System.Windows.Controls.TextBox).Text
                        If Me.acbClientes.SearchText <> strTexto Then
                            Me.acbClientes.SearchText = strTexto
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar clientes que cumplan con el filtro indicado.", Me.Name, "acbClientes_KeyUp", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se ejecuta cuando finaliza una carga de cliente a través del servicio web y es alimentada la lista del autocomplete
    ''' </summary>
    ''' <param name="plogNroComitentes">Número de clientes retornados</param>
    ''' <remarks></remarks>
    ''' 
    Private Sub mobjVM_CargaClientesCompleta(ByVal plogNroComitentes As Integer, ByVal plogBusquedaComitenteEspecifico As Boolean) Handles mobjVM.CargaComitentesCompleta

        Me.mobjVM.Activar = True
        Me.cmdBuscar.IsEnabled = True
        Me.acbClientes.IsEnabled = True

        Try
            If plogNroComitentes = 0 Then
                mostrarMensaje("No hay clientes que cumplan con la condición de búsqueda", Program.TituloSistema)
            Else
                'Modificado por Juan David Correa
                'Descripción se adiciona la condición para cargar el cliente por defecto si solo se encuentra un registro.
                If Not IsNothing(mobjVM.ListaBusquedaControl) Then
                    If mobjVM.ListaBusquedaControl.Count = 1 Then
                        mobjVM.ItemSeleccionadoBuscador = mobjVM.ListaBusquedaControl.FirstOrDefault
                        'acbClientes.SearchText = LTrim(RTrim(mobjVM.ComitenteSeleccionado.CodigoOYD))
                        acbClientes.IsDropDownOpen = False
                    Else
                        If plogBusquedaComitenteEspecifico = False Or Me._mlogBuscarAlIniciar Then
                            'If Me.acbClientes.SearchText.Trim().Equals(String.Empty) Then
                            '    Me.acbClientes.SearchText = STR_TEXTO_ABRIR_POPUP '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda - Asignar la constante y no el valor fijo " "
                            'End If
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
            mostrarErrorAplicacion(Program.Usuario, "Falló la actualización del cliente seleccionado", Me.Name, "mobjVM_CargaClientesCompleta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se dispara cuando en el ViewModel se actualiza una propiedad
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub mobjVM_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles mobjVM.PropertyChanged
        Try
            If e.PropertyName.ToLower = "comitenteseleccionado" Then
                '// Si la propiedad cambio porque el usuario está digitando un nuevo filtro no se lanza el evento porque aún no se sabe que comitente se seleccionará.
                If mlogDigitandoFiltro = False Then
                    If mobjVM.ComitenteSeleccionado Is Nothing And mobjVM.Buscando = False Then
                        '// Si está en proceso de búsqueda (true) no se lanza el evento hasta que este termine
                        RaiseEvent comitenteAsignado(mobjVM.IdComitente, Nothing)
                        RaiseEvent comitenteAsignadoControlOrigen(Me.Name, mobjVM.IdComitente, Nothing)
                    ElseIf Not mobjVM.ComitenteSeleccionado Is Nothing Then
                        RaiseEvent comitenteAsignado(mobjVM.ComitenteSeleccionado.IdComitente, mobjVM.ComitenteSeleccionado)
                        RaiseEvent comitenteAsignadoControlOrigen(Me.Name, mobjVM.ComitenteSeleccionado.IdComitente, mobjVM.ComitenteSeleccionado)
                    End If
                Else
                    If Not mobjVM.ComitenteSeleccionado Is Nothing Then
                        RaiseEvent comitenteAsignado(mobjVM.ComitenteSeleccionado.IdComitente, mobjVM.ComitenteSeleccionado)
                        RaiseEvent comitenteAsignadoControlOrigen(Me.Name, mobjVM.ComitenteSeleccionado.IdComitente, mobjVM.ComitenteSeleccionado)
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Falló la sincronización del evento cuando cambio la propiedad " & e.PropertyName.ToLower, Me.Name, "mobjVM_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
        mobjVM.ComitenteSeleccionado = Nothing
        acbClientes.SearchText = String.Empty
        mobjVM.ClienteTercero = False
        RaiseEvent comitenteAsignado(mobjVM.IdComitente, Nothing)
        RaiseEvent comitenteAsignadoControlOrigen(Me.Name, mobjVM.IdComitente, Nothing)
    End Sub

#End Region

End Class

Public Class clsFiltroBuscadorClientes
    Inherits FilteringBehavior

    Public Overrides Function FindMatchingItems(searchText As String, items As IList, escapedItems As IEnumerable(Of Object), textSearchPath As String, textSearchMode As TextSearchMode) As IEnumerable(Of Object)
        If Not String.IsNullOrEmpty(searchText) Then
            Dim results = items.OfType(Of clsClientes_Buscador).Where(Function(x) x.ItemBusqueda.Nombre.ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or IIf(IsNothing(x.ItemBusqueda.NroDocumento), "", x.ItemBusqueda.NroDocumento).ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper) _
                                                                                     Or x.ItemBusqueda.IdComitente.ToUpper(CultureInfo.InvariantCulture).Contains(searchText.ToUpper)
                                                                                     )
            Return results.Where(Function(x) Not escapedItems.Contains(x))
        Else
            Dim results = items.OfType(Of clsClientes_Buscador)
            Return results.Where(Function(x) Not escapedItems.Contains(x))
        End If
    End Function

End Class

Public Class clsClientes_Buscador
    Implements INotifyPropertyChanged

    Private _ItemBusqueda As OYDUtilidades.BuscadorClientes
    Public Property ItemBusqueda() As OYDUtilidades.BuscadorClientes
        Get
            Return _ItemBusqueda
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
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
