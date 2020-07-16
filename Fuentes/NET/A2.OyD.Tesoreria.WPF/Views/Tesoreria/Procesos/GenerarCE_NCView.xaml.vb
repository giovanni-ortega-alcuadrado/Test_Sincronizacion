Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports C1.Silverlight.DataGrid
Imports C1.Silverlight.DataGrid.Summaries



Partial Public Class GenerarCE_NCView
    Inherits UserControl

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Dim objA2VM As A2UtilsViewModel
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

#End Region

#Region "Inicializaciones"

    Public Sub New()

        Dim strModuloTesoreria As String = ""

        Try
            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            Application.Current.Resources.Add("moduloTesoreria", TesoreriaViewModel.ClasesTesoreria.CE)

            mlogInicializado = True
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New GenerarCE_NCViewModel
InitializeComponent()
    End Sub

#End Region

#Region "Eventos"


    Private Sub BuscadorGenericoListaButon_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.TipoTesoreria Or CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.FormaPagoCE.Equals("T") Then
            Buscador_Generico.TipoItem = "cuentasbancarias"
        Else
            Buscador_Generico.TipoItem = "BancosChequeraAutomatica"
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "IDBanco"
                    'CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarBancos = False
                    CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.NroBanco = pobjItem.IdItem
                    CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.BancoTieneGMF = CBool(pobjItem.CodEstado)
                    CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.CobroGMF = CBool(pobjItem.CodEstado)
                    'JCM20160219
                    CType(Me.DataContext, GenerarCE_NCViewModel).strCompaniaBanco = pobjItem.InfoAdicional03


                    'CType(Me.DataContext, GenerarCE_NCViewModel).UltimosValoresSelected.Banco = pobjItem.Nombre
                    'CType(Me.DataContext, GenerarCE_NCViewModel).EspecificacionesCESelected.ChequeraAutomatica = pobjItem.Estado

                    'If CType(Me.DataContext, GenerarCE_NCViewModel).EspecificacionesCESelected.ChequeraAutomatica Then
                    '    CType(Me.DataContext, GenerarCE_NCViewModel).EspecificacionesCESelected.HabilitarCheque = False
                    '    CType(Me.DataContext, GenerarCE_NCViewModel).UltimosValoresSelected.NumCheque = 0
                    'Else
                    '    CType(Me.DataContext, GenerarCE_NCViewModel).EspecificacionesCESelected.HabilitarCheque = True
                    'End If
                    'CType(Me.DataContext, TesoreriaViewModel)._mlogBuscarBancos = True
                Case "IDCuentaContable"
                    CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.CuentaContable = pobjItem.IdItem
            End Select
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
            CType(Me.DataContext, GenerarCE_NCViewModel).buscarBancos(DirectCast(sender, System.Windows.Controls.TextBox).Text)
        End If
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarCE_NCViewModel).GenerarCE()
    End Sub

    Private Sub btnConsultarPen_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarCE_NCViewModel).ConsultarPagosDeceval()
    End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarCE_NCViewModel).SeleccionarTodos()
    End Sub

    Private Sub CheckBox_Unchecked(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarCE_NCViewModel).DesseleccionarTodos()
    End Sub


#End Region


    Private Sub cboConsecutivos_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If CType(Me.DataContext, GenerarCE_NCViewModel).validarCompanias = False Then
            cboConsecutivos.SelectedItem = -1
        End If
    End Sub

    Private Sub cboConsecutivosRC_SelectedItemChanged(sender As Object, e As SelectionChangedEventArgs)
        If CType(Me.DataContext, GenerarCE_NCViewModel).validarCompanias = False Then
            cboConsecutivos.SelectedItem = -1
        End If
    End Sub

    Private Sub ctlBuscadorEspeciesDemocratizadas_nemotecnicoAsignado(pstrNemotecnico As String, pstrNombreEspecie As String)
        Try
            CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.IDEspecie = pstrNemotecnico
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al recibir la respuesta del buscador.", Me.ToString, "ctlBuscadorEspeciesDemocratizadas_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As Object, e As RoutedEventArgs)
        Try
            CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.IDEspecie = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al recibir la respuesta del buscador.", Me.ToString, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlBuscadorEspeciesDemocratizadas_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            If CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.TipoEspecie = "A" Then
                ctlBuscadorEspeciesDemocratizadas.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
            ElseIf CType(Me.DataContext, GenerarCE_NCViewModel).ParametrosConsultaSelected.TipoEspecie = "C" Then
                ctlBuscadorEspeciesDemocratizadas.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
            Else
                ctlBuscadorEspeciesDemocratizadas.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al recibir la respuesta del buscador.", Me.ToString, "ctlBuscadorEspeciesDemocratizadas_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim objItemSeleccionado As OyDImportaciones.Archivo = CType(CType(sender, Button).Tag, OyDImportaciones.Archivo)
        CType(Me.DataContext, GenerarCE_NCViewModel).BorrarArchivo(objItemSeleccionado)
    End Sub

    Private Sub btnReporte_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, GenerarCE_NCViewModel).EjecutarConsulta()
    End Sub
End Class
