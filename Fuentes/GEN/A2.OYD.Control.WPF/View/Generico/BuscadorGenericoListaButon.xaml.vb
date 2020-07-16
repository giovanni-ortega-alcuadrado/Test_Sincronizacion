Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class BuscadorGenericoListaButon
    Inherits UserControl

#Region "Eventos"

    Public Event finalizoBusqueda(ByVal pstrClaseControl As String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)

#End Region

#Region "Variables"

    Private WithEvents mobjBuscador As BuscadorGenericoLista

#End Region

#Region "Propiedades"

    Public Shared ReadOnly AgrupamientoProperty As DependencyProperty = DependencyProperty.Register("Agrupamiento", GetType(String), GetType(BuscadorGenericoListaButon), Nothing)

    'SV20160203
    Public Shared ReadOnly Condicion1Property As DependencyProperty = DependencyProperty.Register("Condicion1", GetType(String), GetType(BuscadorGenericoListaButon), Nothing)

    Public Shared ReadOnly Condicion2Property As DependencyProperty = DependencyProperty.Register("Condicion2", GetType(String), GetType(BuscadorGenericoListaButon), Nothing)

    Private _mobjItem As OYDUtilidades.BuscadorGenerico = Nothing
    Public ReadOnly Property ItemSeleccionado As OYDUtilidades.BuscadorGenerico
        Get
            Return (_mobjItem)
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

    Private _mintEstadoItem As BuscadorGenericoViewModel.EstadosItem = BuscadorGenericoViewModel.EstadosItem.T
    Public Property EstadoItem As BuscadorGenericoViewModel.EstadosItem
        Get
            Return (_mintEstadoItem)
        End Get
        Set(ByVal value As BuscadorGenericoViewModel.EstadosItem)
            _mintEstadoItem = value
        End Set
    End Property

    Private _mstrTipoItem As String = String.Empty
    Public Property TipoItem As String
        Get
            Return (_mstrTipoItem)
        End Get
        Set(ByVal value As String)
            _mstrTipoItem = value
        End Set
    End Property

    'Private _mstrAgrupamiento As String = String.Empty
    'Public Property Agrupamiento As String
    '    Get
    '        Return (_mstrAgrupamiento)
    '    End Get
    '    Set(ByVal value As String)
    '        _mstrAgrupamiento = value
    '    End Set
    'End Property

    Public Property Agrupamiento As String
        Get
            Return CStr(GetValue(AgrupamientoProperty))
        End Get
        Set(ByVal value As String)
            SetValue(AgrupamientoProperty, value)
        End Set
    End Property

    'SV20160203
    Public Property Condicion1 As String
        Get
            Return CStr(GetValue(Condicion1Property))
        End Get
        Set(ByVal value As String)
            SetValue(Condicion1Property, value)
        End Set
    End Property

    'SV20160203
    Public Property Condicion2 As String
        Get
            Return CStr(GetValue(Condicion2Property))
        End Get
        Set(ByVal value As String)
            SetValue(Condicion2Property, value)
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

#End Region

#Region "Callback"

    ''' <summary>
    ''' Procedimiento de Call back que se lanza cuando alguna de las dependency properties se modifica
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Shared Sub CambioPropiedadDep(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)

    End Sub

#End Region

#Region "Inicialización"

    Public Sub New()
        InitializeComponent()
        Me.Agrupamiento = String.Empty
        Me.Condicion1 = String.Empty
        Me.Condicion2 = String.Empty
    End Sub

#End Region

#Region "Eventos controles"


    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mobjBuscador = New BuscadorGenericoLista(_mstrCampoBusqueda, _mstrEtiqueta, _mstrTipoItem, _mintEstadoItem, Agrupamiento, Condicion1, Condicion2)

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
            Me._mobjItem = mobjBuscador.ItemSeleccionado
            RaiseEvent finalizoBusqueda(_mstrCampoBusqueda, mobjBuscador.ItemSeleccionado)
        Catch ex As Exception
            Me._mobjItem = Nothing
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al cerrar el buscador de clientes", Me.Name, "mobjBuscador_Closed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub AbrirBuscador()
        Try
            mobjBuscador = New BuscadorGenericoLista(_mstrCampoBusqueda, _mstrEtiqueta, _mstrTipoItem, _mintEstadoItem, Agrupamiento, Condicion1, Condicion2)

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
