Imports Telerik.Windows.Controls


Partial Public Class CalificacionesEmisorView
    Inherits UserControl
    Private WithEvents VM As CalificacionesEmisorViewModel

    Public Sub New()
        Try
            Me.DataContext = New CalificacionesEmisorViewModel
            InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Error al inicializar la pantalla.", "CalificacionesEmisorView", "New", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Private Sub CalificacionesInversiones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            cm.GridTransicion = grdGridForma
            cm.GridViewRegistros = datapager1
            'cm.DF = df
            CType(Me.DataContext, CalificacionesEmisorViewModel).NombreColeccionDetalle = Me.ToString
            inicializar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Error al inicializar la pantalla.", "CalificacionesEmisorView", "CalificacionesInversiones_Loaded", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
        End If
    End Sub

    Private Sub DatePicker_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub
End Class
