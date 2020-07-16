Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports C1.Silverlight.DataGrid
Imports C1.Silverlight.DataGrid.Summaries



Partial Public Class GenerarAutomaticamenteNotasView
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

        Me.DataContext = New GenerarAutomaticamenteNotasViewModel
InitializeComponent()
    End Sub

#End Region

#Region "Eventos"

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "IDBancoRegistro"
                    'CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarBancos = False
                    CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ValoresSelected.RegistroBanco = pobjItem.IdItem
                Case "IDBancoContraParte"
                    CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ValoresSelected.ContraParteBanco = pobjItem.IdItem
                Case "CCostoRegistro"
                    CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ValoresSelected.RegistroCCostos = pobjItem.IdItem
                Case "CCostoContraParte"
                    CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ValoresSelected.ContraParteCCostos = pobjItem.IdItem
                Case "IDCuentaContableRegistro"
                    CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ValoresSelected.RegistroCuentaContable = pobjItem.IdItem
                Case "IDCuentaContableContraParte"
                    CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ValoresSelected.ContraParteCuentaContable = pobjItem.IdItem
            End Select
        End If
    End Sub

    Private Sub Buscar_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ValoresSelected.RegistroCliente = pobjComitente.IdComitente
        End If
    End Sub

    Private Sub Buscar_finalizoBusquedaContraParte(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ValoresSelected.ContraParteCliente = pobjComitente.IdComitente
        End If
    End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' Evento para la busqueda del Banco.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB20130611</remarks>
    Private Sub txtBanco_LostFocus(sender As Object, e As RoutedEventArgs)
        If Not String.IsNullOrEmpty(DirectCast(sender, System.Windows.Controls.TextBox).Text) Then
            CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).buscarBancos(DirectCast(sender, System.Windows.Controls.TextBox).Text)
        End If
    End Sub

    ''' <summary>
    ''' Evento para la busqueda del Centro de Costos.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB20130611</remarks>
    Private Sub txtCCosto_LostFocus(sender As Object, e As RoutedEventArgs)
        If Not String.IsNullOrEmpty(DirectCast(sender, System.Windows.Controls.TextBox).Text) Then
            CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).buscarCentroCostos(DirectCast(sender, System.Windows.Controls.TextBox).Text)
        End If
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).GenerarNC()
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ConsultarPendientesTesoreraia()
    End Sub

    'Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
    '    CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).SeleccionarTodos()
    'End Sub

    'Private Sub CheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
    '    CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).DesseleccionarTodos()
    'End Sub

    Private Sub ListBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ModificacionParametros()
    End Sub
    Private Sub ListBox_SelectionChangedContraParte(sender As Object, e As SelectionChangedEventArgs)
        CType(Me.DataContext, GenerarAutomaticamenteNotasViewModel).ModificacionParametrosContraParte()
    End Sub

    

#End Region


End Class