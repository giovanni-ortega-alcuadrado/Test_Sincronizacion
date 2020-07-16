Imports Telerik.Windows.Controls
Partial Public Class OrdenGiroConsultaView
    Inherits Window

    Private _IDNuevo As Integer
    Public Property IDNuevo() As Integer
        Get
            Return _IDNuevo
        End Get
        Set(ByVal value As Integer)
            _IDNuevo = value
        End Set
    End Property

    Private _IDAnterior As Integer
    Public Property IDAnterior() As Integer
        Get
            Return _IDAnterior
        End Get
        Set(ByVal value As Integer)
            _IDAnterior = value
        End Set
    End Property

#Region "Inicializacion"

    Public Sub New(ByVal pIdOrdenGiro As Integer, ByVal plogEditarOrden As Boolean, ByVal plogPendientePorAprobar As Boolean, ByVal objViewModelTesorero As TesoreroViewModel_OYDPLUS)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        InitializeComponent()

        Dim objNuevoOrdenGiro As New ComprobantesEgresoPLUSView(pIdOrdenGiro, plogEditarOrden, plogPendientePorAprobar, objViewModelTesorero)

        If Not IsNothing(Me.gridOrdenGiro.Children) Then
            Me.gridOrdenGiro.Children.Clear()
        End If
        objNuevoOrdenGiro.ViewPopPupEdicion = Me
        Me.gridOrdenGiro.Children.Add(objNuevoOrdenGiro)
        IDAnterior = pIdOrdenGiro
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

    Public Sub CerrarConIDArchivo(ByVal IdNuevo As Integer)
        Me.IDNuevo = IdNuevo
        Me.DialogResult = True
    End Sub
End Class
