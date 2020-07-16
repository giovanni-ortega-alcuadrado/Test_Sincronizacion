Imports Telerik.Windows.Controls
' Descripción:    View del formulario de Generar Plano ACH
' Creado por:     Sebastian Londoño Benitez
' Fecha:          Mayo 30/2013

Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports C1.Silverlight.DataGrid
Imports C1.Silverlight.DataGrid.Summaries



Partial Public Class GenerarPlanoACHView
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
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New GenerarArchivoPlanoACHViewModel
InitializeComponent()
    End Sub

#End Region

#Region "Eventos"

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "IDBanco"
                    CType(Me.DataContext, GenerarArchivoPlanoACHViewModel)._mlogBuscarBancos = False
                    CType(Me.DataContext, GenerarArchivoPlanoACHViewModel).ParametrosConsultaSelected.IDBanco = pobjItem.IdItem
                    CType(Me.DataContext, GenerarArchivoPlanoACHViewModel).buscarFormatosBanco()
                    CType(Me.DataContext, GenerarArchivoPlanoACHViewModel)._mlogBuscarBancos = True
            End Select
        End If
    End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarArchivoPlanoACHViewModel).GuardarPlanoACH()
    End Sub

    'Private Sub btnConsultarPen_Click(sender As Object, e As RoutedEventArgs)
    '    CType(Me.DataContext, GenerarArchivoPlanoACHViewModel).ConsultarPendientesTesoreraia()
    'End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarArchivoPlanoACHViewModel).SeleccionarTodos()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim objItemSeleccionado As OyDTesoreria.TesoreriaACHPendiente = CType(CType(sender, Button).Tag, OyDTesoreria.TesoreriaACHPendiente)
        CType(Me.DataContext, GenerarArchivoPlanoACHViewModel).CuentasBancariasClientes_Mostar(objItemSeleccionado)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        Dim objItemSeleccionado As OyDImportaciones.Archivo = CType(CType(sender, Button).Tag, OyDImportaciones.Archivo)
        CType(Me.DataContext, GenerarArchivoPlanoACHViewModel).BorrarArchivo(objItemSeleccionado)
    End Sub

    Private Sub CheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarArchivoPlanoACHViewModel).DesseleccionarTodos()
    End Sub

#End Region

End Class