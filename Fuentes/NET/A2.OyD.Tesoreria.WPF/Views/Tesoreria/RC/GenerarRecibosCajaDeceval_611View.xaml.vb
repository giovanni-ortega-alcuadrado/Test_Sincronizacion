Imports Telerik.Windows.Controls
'Codigo Creado Por: Rafael Cordero
'Archivo: GenerarRecibosCajaDecevalView.vb
'Generado el : 08/07/2011 
'Propiedad de Alcuadrado S.A. 2011
Imports A2Utilidades.Mensajes
Imports System.Collections.ObjectModel
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports Telerik.Windows.Controls.GridView

Partial Public Class GenerarRecibosCajaDeceval_611View
    Inherits UserControl

    Dim vm As GenerarRecibosCajaDeceval_611ViewModel
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Dim objA2VM As A2UtilsViewModel
        Dim strModuloTesoreria As String = ""

        Try

            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            strClaseCombos = "Tesoreria_RecibosCaja"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            Application.Current.Resources.Add("moduloTesoreria", GenerarRecibosCajaDeceval_611ViewModel.ClasesTesoreria.RC)

            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

        Catch ex As Exception

        End Try
        Me.DataContext = New GenerarRecibosCajaDeceval_611ViewModel
        InitializeComponent()
    End Sub

    Private Sub ImportarTitulosValorizadosView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, GenerarRecibosCajaDeceval_611ViewModel)
        CType(Me.DataContext, GenerarRecibosCajaDeceval_611ViewModel).NombreView = Me.ToString
        dtpFechaRecibo.SelectedDate = Now.Date
        dtpConsignarEl.SelectedDate = Nothing
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        vm.Aceptar()
    End Sub

    Private Sub BuscadorBancos_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico) Handles BuscadorBancos.finalizoBusqueda
        If Not pobjItem Is Nothing Then
            vm.RegistroTesoreriaSeleccionado.IDBanco = pobjItem.IdItem
            vm.NombreBanco = pobjItem.Nombre
        Else
            vm.RegistroTesoreriaSeleccionado.IDBanco = Nothing
            vm.NombreBanco = Nothing
        End If
    End Sub

    Private Sub SeleccionUno(sender As Object, e As System.Windows.RoutedEventArgs)
        If Not CType(sender, CheckBox).IsChecked Then
            If chkTodos.IsChecked Then
                vm.SeleccionarTodosChecked(Nothing)
            End If
        End If
    End Sub

    Private Sub chkTodos_Checked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkTodos.Checked
        If Not vm Is Nothing Then
            If vm.SeleccionarTodos Then
                vm.SeleccionarTodosChecked(True)
            End If
        End If
    End Sub

    Private Sub chkTodos_Unchecked(sender As Object, e As System.Windows.RoutedEventArgs) Handles chkTodos.Unchecked
        If Not vm Is Nothing Then
            If Not vm.SeleccionarTodos Then
                vm.SeleccionarTodosChecked(False)
            End If
        End If
    End Sub

    Private Sub btnReporte_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, GenerarRecibosCajaDeceval_611ViewModel).EjecutarConsulta()
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "idcuentacontable"
                    CType(Me.DataContext, GenerarRecibosCajaDeceval_611ViewModel).CuentaContable = pobjItem.IdItem
                Case "lngid"
                    CType(Me.DataContext, GenerarRecibosCajaDeceval_611ViewModel).strCompaniaBanco = pobjItem.InfoAdicional03 'JCM20160216
                Case "idcentrocostos"
                    CType(Me.DataContext, GenerarRecibosCajaDeceval_611ViewModel).CentroCostos = pobjItem.IdItem

            End Select
        End If
    End Sub

    Private Sub Buscador_Cliente_GotFocus(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(sender) Then
            CType(sender, A2ComunesControl.BuscadorClienteListaButon).Agrupamiento = "id*" + CType(sender, A2ComunesControl.BuscadorClienteListaButon).Tag
        End If
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda_1(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, GenerarRecibosCajaDeceval_611ViewModel).SelectedDetalle.lngIDComitente = pobjComitente.IdComitente
            CType(Me.DataContext, GenerarRecibosCajaDeceval_611ViewModel).SelectedDetalle.Cliente = pobjComitente.Nombre
        End If
    End Sub

    Private Sub RadGridView_RowLoaded(ByVal sender As Object, ByVal e As RowLoadedEventArgs)
        Try
            Dim objItemSeleccionado As OyDTesoreria.ListaDetalleTesoreria = Nothing

            Try
                objItemSeleccionado = CType(e.Row.Item, OyDTesoreria.ListaDetalleTesoreria)
            Catch ex1 As Exception

            End Try

            If Not IsNothing(objItemSeleccionado) Then

                If Program.VerificarRecurso("HabilitarResaltoColorDeceval", "SI") Then
                    If objItemSeleccionado.NroCodigosOyDxCliente > 1 Then
                        e.Row.Background = New BrushConverter().ConvertFromString("#85FFF2")
                    Else
                        e.Row.Background = New BrushConverter().ConvertFromString("#e8e8e8")
                    End If
                Else
                    e.Row.Background = New BrushConverter().ConvertFromString("#e8e8e8")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class


