Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
'Imports A2Utilidades.Mensajes
'Imports Microsoft.VisualBasic.CompilerServices

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ExcepcionesRDIPView.xaml.vb
'Generado el : 08/09/2011 13:22:05
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ReporteExcelTitulosActivos
    Inherits UserControl
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Dim objA2VM As A2UtilsViewModel
        'Dim strModuloTesoreria As String = ""

        strClaseCombos = "CitiBank_ReporteExcelTitulos"
        mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)

        Try
            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

            Me.DataContext = New ReporteExcelTitulosViewModel
InitializeComponent()
        Catch ex As Exception
            'mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                     Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'SLB20140512 No limpiar los parametros de consulta.
    'Private Sub ReporteExcelTitulosActivos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
    '    CType(Me.DataContext, ReporteExcelTitulosViewModel).Limpiar()
    'End Sub

#Region "Buscadores ExcepcionesRDIP"
    Private Sub Buscar_finalizoBusquedaComitenteDesde(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, ReporteExcelTitulosViewModel).cb.IDComitenteDesde = pobjComitente.IdComitente
        End If
    End Sub

    Private Sub Buscar_finalizoBusquedaComitenteHasta(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, ReporteExcelTitulosViewModel).cb.IDComitenteHasta = pobjComitente.IdComitente
        End If
    End Sub

    Private Sub Buscar_finalizoBusquedaEspecieDesde(ByVal pstrClaseControl As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            CType(Me.DataContext, ReporteExcelTitulosViewModel).cb.IDEspecieDesde = pobjEspecie.Nemotecnico
        End If
    End Sub

    Private Sub Buscar_finalizoBusquedaEspecieHasta(ByVal pstrClaseControl As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            CType(Me.DataContext, ReporteExcelTitulosViewModel).cb.IDEspecieHasta = pobjEspecie.Nemotecnico
        End If
    End Sub
#End Region

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ReporteExcelTitulosViewModel).Limpiar()
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'CType(Me.DataContext, ReporteExcelTitulosViewModel).FormatoExcel()
        CType(Me.DataContext, ReporteExcelTitulosViewModel).EjecutarConsulta()
    End Sub

    Private Sub cmbEstado_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
        CType(Me.DataContext, ReporteExcelTitulosViewModel).HabilitarTipoBloqueo()
    End Sub
End Class


