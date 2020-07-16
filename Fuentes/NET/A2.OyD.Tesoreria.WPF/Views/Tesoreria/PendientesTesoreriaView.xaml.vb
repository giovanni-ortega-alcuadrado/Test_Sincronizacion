Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: PendientesTesoreriaView.xaml.vb
'Generado el : 07/30/2012 14:53:14
'Propiedad de Alcuadrado S.A. 2010

Imports A2Utilidades.Mensajes


Partial Public Class PendientesTesoreriaView
    Inherits UserControl
    Private mobjVM As PendientesTesoreriaViewModel
    Dim logcargainicial As Boolean = True

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Dim objA2VM As A2UtilsViewModel
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

#End Region

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

            Me.DataContext = New PendientesTesoreriaViewModel
            Me.Resources.Add("ViewModelPrincipal", Me.DataContext)
            InitializeComponent()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub PendientesTesoreria_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado Then
                mlogInicializado = False
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
                cm.DF = df
                mobjVM = CType(Me.DataContext, PendientesTesoreriaViewModel)
                mobjVM.NombreView = Me.ToString
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "PendientesTesoreria_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    End Sub

    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
    End Sub

    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        mobjVM.Aprobando()
    End Sub

    Private Sub cmdConsultar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.CargarGrid()
    End Sub
End Class


