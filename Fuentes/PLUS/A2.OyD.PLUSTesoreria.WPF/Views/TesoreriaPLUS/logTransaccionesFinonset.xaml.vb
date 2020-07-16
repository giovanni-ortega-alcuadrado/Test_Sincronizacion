Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports System.ComponentModel
Imports C1.Silverlight.DataGrid
Imports C1.Silverlight.DataGrid.Summaries


Partial Public Class logTransaccionesFinonset
    Inherits UserControl

    Dim objVMA2Utils As A2UtilsViewModel
    Private WithEvents ObjVMLog As logTransaccionesFinonsetViewModel
    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False
    Private mstrDicCombosEspecificos As String = String.Empty
    Private strClaseCombos As String = "Plus_Tesorero"
#Region "Inicializaciones"

    Public Sub New()
        Dim objA2VM As A2UtilsViewModel
        Dim strModuloTesoreria As String = ""

        Try

            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

            mlogInicializado = True

            Me.DataContext = New logTransaccionesFinonsetViewModel
InitializeComponent()

            ObjVMLog = Me.LayoutRoot.DataContext

            'Me.DataContext = objVMOrdenesTesoreria
            AddHandler Me.Unloaded, AddressOf View_Unloaded

           

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


#End Region

    Private Sub LogTransacciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        Try
            
            ''cm.DF = df
            If ObjVMLog Is Nothing Then
                ObjVMLog = CType(Me.DataContext, logTransaccionesFinonsetViewModel) 
                'Inicia el timer cuando se cargue la pantalla 
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "Tesorero_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    
     
    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de tesorero
         
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla del tesorero.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnExportarExcel_Click(sender As Object, e As RoutedEventArgs)
        ObjVMLog.Exportar()
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        ObjVMLog.Consultar()
    End Sub
End Class
