Imports Telerik.Windows.Controls
Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports C1.Silverlight.DataGrid
Imports C1.Silverlight.DataGrid.Summaries

'Codigo Desarrollado por: Santiago Alexander Vergara Orrego
'Archivo: Public Class ChequesSinEntregarView.vb
'Propiedad de Alcuadrado S.A. 2013

Partial Public Class ChequesSinEntregarView
    Inherits UserControl



#Region "Variables"

    Private mlogInicializado As Boolean = False
    Dim objA2VM As A2UtilsViewModel
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

#End Region

#Region "Inicializaciones"


    Public Sub New()

        Dim objA2VM As A2UtilsViewModel
        Dim strModuloTesoreria As String = ""

        Try
            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            strClaseCombos = "Tesoreria_ComprobantesEgreso"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            Application.Current.Resources.Add("moduloTesoreria", TesoreriaViewModel.ClasesTesoreria.CE)

            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)


            mlogInicializado = True

            Me.DataContext = New ChequesSinEntregarViewModel
            InitializeComponent()

            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
            Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try


    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
    End Sub

#End Region

#Region "Eventos"

    Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, ChequesSinEntregarViewModel).ConsultarChequesSinEntregar()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, ChequesSinEntregarViewModel).GuardarChequesEntregados()
    End Sub

#End Region

End Class