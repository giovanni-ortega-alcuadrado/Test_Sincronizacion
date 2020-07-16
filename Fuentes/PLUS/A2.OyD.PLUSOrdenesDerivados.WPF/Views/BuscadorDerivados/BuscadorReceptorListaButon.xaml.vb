Imports Telerik.Windows.Controls
Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class BuscadorReceptorListaButon
    Inherits UserControl

#Region "Eventos"

    Public Event finalizoBusqueda(ByVal pstrClaseControl As String, ByVal pobjItem As OyDPLUSOrdenesDerivados.ReceptoresBusqueda)

#End Region

#Region "Variables"

    Private WithEvents mobjBuscador As A2OYDPLUSOrdenesDerivados.BuscadorReceptorLista

#End Region

#Region "Propiedades"

    Public Shared ReadOnly AgrupamientoProperty As DependencyProperty = DependencyProperty.Register("Agrupamiento", GetType(String), GetType(BuscadorReceptorListaButon), Nothing)

    Private _mobjItem As OyDPLUSOrdenesDerivados.ReceptoresBusqueda = Nothing
    Public ReadOnly Property ItemSeleccionado As OyDPLUSOrdenesDerivados.ReceptoresBusqueda
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
    End Sub

#End Region

#Region "Eventos controles"

    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mobjBuscador = New A2OYDPLUSOrdenesDerivados.BuscadorReceptorLista(_mstrCampoBusqueda, _mstrEtiqueta)

            If _mintIzquierda > 0 And _mintSuperior > 0 Then
                mobjBuscador.Top = _mintSuperior
                mobjBuscador.Left = _mintIzquierda
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

#End Region

End Class
