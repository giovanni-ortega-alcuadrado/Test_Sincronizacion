Imports Telerik.Windows.Controls
Partial Public Class OrdenReciboConsultaView
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

    Public Sub New(ByVal pIdOrdenRecibo As Integer, pvmTesorero As TesoreroViewModel_OYDPLUS)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        InitializeComponent()

        Dim objNuevoOrdenRecibo As New OrdenesReciboPLUSView(pIdOrdenRecibo, pvmTesorero)

        If Not IsNothing(Me.gridOrdenRecibo.Children) Then
            Me.gridOrdenRecibo.Children.Clear()
        End If
        objNuevoOrdenRecibo.ViewPopPupEdicion = Me

        Me.gridOrdenRecibo.Children.Add(objNuevoOrdenRecibo)

        IDAnterior = pIdOrdenRecibo
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
