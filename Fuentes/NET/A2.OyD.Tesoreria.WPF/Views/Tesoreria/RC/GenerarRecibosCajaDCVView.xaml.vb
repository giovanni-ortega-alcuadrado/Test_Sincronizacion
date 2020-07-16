Imports Telerik.Windows.Controls
'Codigo Creado Por: Rafael Cordero
'Archivo: GenerarRecibosCajaDCVView.vb
'Generado el : 08/07/2011 
'Propiedad de Alcuadrado S.A. 2011
Imports A2Utilidades.Mensajes
Imports System.Collections.ObjectModel
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices



Partial Public Class GenerarRecibosCajaDCVView
    Inherits UserControl

    Dim vm As GenerarRecibosCajaDCVViewModel
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
            Application.Current.Resources.Add("moduloTesoreria", GenerarRecibosCajaDecevalViewModel.ClasesTesoreria.RC)

            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

        Catch ex As Exception

        End Try
        Me.DataContext = New GenerarRecibosCajaDCVViewModel
InitializeComponent()
    End Sub

    Private Sub ImportarTitulosValorizadosView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, GenerarRecibosCajaDCVViewModel)
        CType(Me.DataContext, GenerarRecibosCajaDCVViewModel).NombreView = Me.ToString
        dtpFechaRecibo.SelectedDate = Now.Date
        dtpConsignarEl.SelectedDate = Nothing
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        vm.Aceptar()
    End Sub

    Private Sub cboNombreSucursal_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs) Handles cboNombreSucursal.SelectionChanged
        vm.NombreSucursal_Click(CType(sender, ComboBox).SelectedItem.descripcion)
    End Sub

    Private Sub BuscadorBancos_finalizoBusqueda(pstrClaseControl As String, pobjItem AS OYDUtilidades.BuscadorGenerico) Handles BuscadorBancos.finalizoBusqueda
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

    Private Sub btnReporte_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, GenerarRecibosCajaDCVViewModel).EjecutarConsulta()
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "idcuentacontable"
                    CType(Me.DataContext, GenerarRecibosCajaDCVViewModel).CuentaContable = pobjItem.IdItem
                Case "lngid"
                    CType(Me.DataContext, GenerarRecibosCajaDCVViewModel).strCompaniaBanco = pobjItem.InfoAdicional03 'JCM20160217
            End Select
        End If
    End Sub
End Class


