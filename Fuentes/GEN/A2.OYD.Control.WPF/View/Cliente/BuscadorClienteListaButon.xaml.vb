Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class BuscadorClienteListaButon
    Inherits UserControl

#Region "Eventos"

    Public Event finalizoBusqueda(ByVal pstrClaseControl As String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)

#End Region

#Region "Variables"

    Private WithEvents mobjBuscador As BuscadorClienteLista

#End Region

#Region "Propiedades"

    Private Shared ReadOnly IDReceptorDep As DependencyProperty = DependencyProperty.Register("IDReceptor", GetType(String), GetType(BuscadorClienteListaButon), New PropertyMetadata(""))
    Private Shared ReadOnly TipoNegocioDep As DependencyProperty = DependencyProperty.Register("TipoNegocio", GetType(String), GetType(BuscadorClienteListaButon), New PropertyMetadata(""))
    Private Shared ReadOnly TipoProductoDep As DependencyProperty = DependencyProperty.Register("TipoProducto", GetType(String), GetType(BuscadorClienteListaButon), New PropertyMetadata(""))
    Private Shared ReadOnly PerfilRiesgoDep As DependencyProperty = DependencyProperty.Register("PerfilRiesgo", GetType(String), GetType(BuscadorClienteListaButon), New PropertyMetadata(""))
    Private Shared ReadOnly IDCompaniaDep As DependencyProperty = DependencyProperty.Register("IDCompania", GetType(Nullable(Of Integer)), GetType(BuscadorClienteListaButon), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf IDCompaniaChanged)))
    Private Shared ReadOnly ConFiltroDep As DependencyProperty = DependencyProperty.Register("conFiltro", GetType(Boolean), GetType(BuscadorClienteListaButon), New PropertyMetadata(False))
    Private Shared ReadOnly FiltroAdicional1Dep As DependencyProperty = DependencyProperty.Register("filtroAdicional1", GetType(String), GetType(BuscadorClienteListaButon), New PropertyMetadata(""))
    Private Shared ReadOnly FiltroAdicional2Dep As DependencyProperty = DependencyProperty.Register("filtroAdicional2", GetType(String), GetType(BuscadorClienteListaButon), New PropertyMetadata(""))
    Private Shared ReadOnly FiltroAdicional3Dep As DependencyProperty = DependencyProperty.Register("filtroAdicional3", GetType(String), GetType(BuscadorClienteListaButon), New PropertyMetadata(""))

    Private _mobjComitente As OYDUtilidades.BuscadorClientes = Nothing
    Public ReadOnly Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_mobjComitente)
        End Get
    End Property

    Private _mstrCampoBusqueda As String = String.Empty
    Public Property CampoBusqueda As String
        Get
            Return (_mstrCampoBusqueda)
        End Get
        Set(ByVal value As String)
            _mstrCampoBusqueda = value
        End Set
    End Property

    Private _mintEstado As BuscadorClienteViewModel.EstadosComitente = BuscadorClienteViewModel.EstadosComitente.T
    Public Property EstadoComitente As BuscadorClienteViewModel.EstadosComitente
        Get
            Return (_mintEstado)
        End Get
        Set(ByVal value As BuscadorClienteViewModel.EstadosComitente)
            _mintEstado = value
        End Set
    End Property

    Private _mintTipoVinculacion As BuscadorClienteViewModel.TiposVinculacion = BuscadorClienteViewModel.TiposVinculacion.T
    Public Property TipoVinculacion As BuscadorClienteViewModel.TiposVinculacion
        Get
            Return (_mintTipoVinculacion)
        End Get
        Set(ByVal value As BuscadorClienteViewModel.TiposVinculacion)
            _mintTipoVinculacion = value
        End Set
    End Property

    Private _mstrAgrupamiento As String = String.Empty
    Public Property Agrupamiento As String
        Get
            Return (_mstrAgrupamiento)
        End Get
        Set(ByVal value As String)
            _mstrAgrupamiento = value
        End Set
    End Property

    Private _mstrEtiqueta As String = String.Empty
    Public Property Etiqueta As String
        Get
            Return (_mstrEtiqueta)
        End Get
        Set(ByVal value As String)
            _mstrEtiqueta = value
        End Set
    End Property

    Private _mintIzquierda As Integer = 0
    Public Property Izquierda As Integer
        Get
            Return (_mintIzquierda)
        End Get
        Set(ByVal value As Integer)
            _mintIzquierda = value
        End Set
    End Property

    Private _mintSuperior As Integer = 0
    Public Property Superior As Integer
        Get
            Return (_mintSuperior)
        End Get
        Set(ByVal value As Integer)
            _mintSuperior = value
        End Set
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

    Public Property ConFiltro As Boolean
        Get
            Return CBool(GetValue(ConFiltroDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(ConFiltroDep, value)
        End Set
    End Property

    Public Property FiltroAdicional1 As String
        Get
            Return CStr(GetValue(FiltroAdicional1Dep))
        End Get
        Set(ByVal value As String)
            SetValue(FiltroAdicional1Dep, value)
        End Set
    End Property
    Public Property FiltroAdicional2 As String
        Get
            Return CStr(GetValue(FiltroAdicional2Dep))
        End Get
        Set(ByVal value As String)
            SetValue(FiltroAdicional2Dep, value)
        End Set
    End Property
    Public Property FiltroAdicional3 As String
        Get
            Return CStr(GetValue(FiltroAdicional3Dep))
        End Get
        Set(ByVal value As String)
            SetValue(FiltroAdicional3Dep, value)
        End Set
    End Property

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

    Public Property PerfilRiesgo As String
        Get
            Return CStr(GetValue(PerfilRiesgoDep))
        End Get
        Set(ByVal value As String)
            SetValue(PerfilRiesgoDep, value)
        End Set
    End Property

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
            
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "IDCompaniaChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


#End Region

#Region "Inicialización"

    Public Sub New()
        InitializeComponent()
    End Sub

#End Region

#Region "Eventos controles"

    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If Me._mlogCargarClientesRestriccion Then
                mobjBuscador = New BuscadorClienteLista(_mstrCampoBusqueda, _mstrEtiqueta, _mintEstado, _mintTipoVinculacion, _mstrAgrupamiento, _mlogCargarClientesRestriccion, _mlogCargarClientesTercero, _mlogCargarClientesXTipoProductoPerfil, IDReceptor, TipoNegocio, TipoProducto, PerfilRiesgo, _mlogExcluirCodigosCompania, IDCompania, ConFiltro, FiltroAdicional1, FiltroAdicional2, FiltroAdicional3)
            Else
                mobjBuscador = New BuscadorClienteLista(_mstrCampoBusqueda, _mstrEtiqueta, _mintEstado, _mintTipoVinculacion, _mstrAgrupamiento, _mlogExcluirCodigosCompania, IDCompania, ConFiltro, FiltroAdicional1, FiltroAdicional2, FiltroAdicional3)
            End If

            If _mintIzquierda > 0 And _mintSuperior > 0 Then
                mobjBuscador.Top = _mintSuperior
                mobjBuscador.Left = _mintIzquierda
            Else
                'mobjBuscador.CenterOnScreen()
            End If
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscador)
            mobjBuscador.ShowDialog()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al buscar un cliente", Me.Name, "Button_Click_buscar_cliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub mobjBuscador_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscador.Closed
        Try
            Me._mobjComitente = mobjBuscador.ComitenteSeleccionado
            RaiseEvent finalizoBusqueda(_mstrCampoBusqueda, mobjBuscador.ComitenteSeleccionado)
        Catch ex As Exception
            Me._mobjComitente = Nothing
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al cerrar el buscador de clientes", Me.Name, "mobjBuscador_Closed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub AbrirBuscador()
        Try
            If Me._mlogCargarClientesRestriccion Then
                mobjBuscador = New BuscadorClienteLista(_mstrCampoBusqueda, _mstrEtiqueta, _mintEstado, _mintTipoVinculacion, _mstrAgrupamiento, _mlogCargarClientesRestriccion, _mlogCargarClientesTercero, _mlogCargarClientesXTipoProductoPerfil, IDReceptor, TipoNegocio, TipoProducto, PerfilRiesgo, _mlogExcluirCodigosCompania, IDCompania, ConFiltro, FiltroAdicional1, FiltroAdicional2, FiltroAdicional3)
            Else
                mobjBuscador = New BuscadorClienteLista(_mstrCampoBusqueda, _mstrEtiqueta, _mintEstado, _mintTipoVinculacion, _mstrAgrupamiento, _mlogExcluirCodigosCompania, IDCompania, ConFiltro, FiltroAdicional1, FiltroAdicional2, FiltroAdicional3)
            End If

            If _mintIzquierda > 0 And _mintSuperior > 0 Then
                mobjBuscador.Top = _mintSuperior
                mobjBuscador.Left = _mintIzquierda
            Else
                'mobjBuscador.CenterOnScreen()
            End If
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscador)
            mobjBuscador.ShowDialog()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al buscar un cliente", Me.Name, "AbrirBuscador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
