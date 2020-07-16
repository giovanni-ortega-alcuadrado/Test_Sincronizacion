Imports Telerik.Windows.Controls
'Codigo Creado
'Archivo    : EntregaDeCustodiasView.vb
'Por        : Rafael Cordero
'Creado el  : 08/17/2011 04:58:21AM
'Propiedad de Alcuadrado S.A. 2011
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class EntregaDeCustodiasView
    Inherits UserControl
    Dim vm As New EntregaDeCustodiasViewModel

    Public Sub New()
        Me.DataContext = New EntregaDeCustodiasViewModel
InitializeComponent()
    End Sub

    Private Sub EntregaDeCustodiasView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, EntregaDeCustodiasViewModel)
        CType(Me.DataContext, EntregaDeCustodiasViewModel).NombreView = Me.ToString
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(pstrClaseControl As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not pobjComitente Is Nothing Then
            If Not pobjComitente.IdComitente Is Nothing Then
                vm.IdComitente = pobjComitente.IdComitente
                vm.ConsultarCustodiasComitente()
            End If
        End If
    End Sub

    Private Sub btnConsultarCustodias_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnConsultarCustodias.Click
        vm.ConsultarCustodiasComitente()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        vm.EjecutarOperaciones()
    End Sub

    Private Sub dg_ColumnReordered(sender As System.Object, e As System.Windows.Controls.DataGridColumnEventArgs)

    End Sub

    ''' <history>
    ''' Agregado por     : Jhonatan Arley Acevedo Martínez
    ''' Descripción      : Con esta lógica "Obligo" al viewModel a que se entere que la entidad ha sido modificada desde el grid para el objeto seleccionado
    ''' Fecha            : Junio 24/2016
    ''' </history>
    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        CType(sender, CheckBox).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource()
        If Not IsNothing(dg.GetBindingExpression(DataGrid.SelectedItemProperty)) Then
            dg.GetBindingExpression(DataGrid.SelectedItemProperty).UpdateSource()
        End If
    End Sub


    ''' <summary>
    ''' Buscador de ISIN para permitir buscar custodias por ISIN.
    ''' JEPM20151008
    ''' </summary>
    ''' <param name="pstrClaseControl"></param>
    ''' <param name="pobjItem"></param>
    ''' <history>
    ''' Agregado por     : Javier Eduardo Pardo Moreno
    ''' Descripción      : Buscador de ISIN 
    ''' Fecha            : Octubre 10/2015
    ''' Pruebas CB       : Javier Eduardo Pardo Moreno  - Octubre 10/2015 - Resultado Ok 
    ''' </history>
    Private Sub BuscadorGenerico_finalizoBusquedaISINFungible(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "ISINFUNGIBLE"
                        vm._mlogBuscarISINFungible = False
                        vm.ISINFungibleSeleccionada(pobjItem)
                        vm._mlogBuscarISINFungible = True
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al el Isin Fungible", _
                                 Me.ToString(), "BuscadorGenerico_finalizoBusquedaISINFungible", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Evento que dispara la lupa para limpiar los datos del ISIN (strIDEspecie, strISIN)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Javier Eduardo Pardo Moreno
    ''' Fecha        : Octubre 10/2015
    ''' Pruebas CB   : Javier Eduardo Pardo Moreno - Octubre 10/2015 - Resultado OK
    ''' </history>
    Private Sub btnLimpiarISIN_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(vm) Then
                Me.vm.strIsinFungible = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar ISIN", Me.Name, "btnLimpiarISIN_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(vm) Then
                Me.vm.IdComitente = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar Cliente", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class