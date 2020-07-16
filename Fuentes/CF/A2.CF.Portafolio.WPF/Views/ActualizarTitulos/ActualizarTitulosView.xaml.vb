Imports Telerik.Windows.Controls
'Codigo Creado
'Archivo    : EntregaDeCustodiasView.vb
'Por        : Rafael Cordero
'Creado el  : 08/17/2011 04:58:21AM
'Propiedad de Alcuadrado S.A. 2011
Imports A2.OyD.OYDServer.RIA.Web
Imports A2ComunesControl
Imports A2Utilidades.Mensajes


Partial Public Class ActualizarTitulosView
    Inherits UserControl
    Dim vm As New ActualizarTitulosViewModel

    Public Sub New()
        Me.DataContext = New ActualizarTitulosViewModel
InitializeComponent()
    End Sub

    Private Sub ActualizarTitulosView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, ActualizarTitulosViewModel)
        CType(Me.DataContext, ActualizarTitulosViewModel).NombreView = Me.ToString


    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(pstrClaseControl As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            vm._mlogBuscarCliente = False
            vm.strCodigoOyD1 = pobjComitente.CodigoOYD
            vm.CamposBusquedaSelected.CodigoCliente = vm.strCodigoOyD1
            vm._mlogBuscarCliente = True
            vm.ComitenteSeleccionadoM(pobjComitente)
            vm.CamposBusquedaSelected.NombreCliente = pobjComitente.Nombre
            vm._mlogBuscarCliente = False
            vm.ListaDetalleCustodia = Nothing
            vm._mlogBuscarCliente = True
            vm.HabilitarConsultar = True

        End If

    End Sub
    Private Sub Buscador_finalizoBusquedaClientes2(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            vm._mlogBuscarCliente2 = False
            vm.strCodigoOyD2 = pobjComitente.CodigoOYD
            vm.CamposBusquedaSelected2.CodigoCliente = vm.strCodigoOyD2
            vm._mlogBuscarCliente2 = True
            vm.ComitenteSeleccionadoM2(pobjComitente)
            vm.CamposBusquedaSelected2.NombreCliente = pobjComitente.Nombre
        End If
    End Sub

    Private Sub BuscadorEspecieListaButon_finalizoBusqueda_1(pstrClaseControl As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            vm._mlogBuscarEspecie = False
            vm.strEspecie1 = pobjEspecie.Nemotecnico
            vm.CamposBusquedaSelected.Nemotecnico = pobjEspecie.Nemotecnico
            vm.CamposBusquedaSelected.Especie = pobjEspecie.Especie
            vm.CamposBusquedaSelected.EsAccion = pobjEspecie.EsAccion
            vm._mlogBuscarEspecie = True
            vm.HabilitarConsultar = True
            If vm._mlogBuscarEspecie = True Then
                vm.ConsultarISIN()
            End If
        End If
    End Sub

    Private Sub BuscadorEspecieListaButon_finalizoBusqueda_2(pstrClaseControl As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            vm._mlogBuscarEspecie2 = False
            vm.strEspecie2 = pobjEspecie.Nemotecnico
            vm.CamposBusquedaSelected2.Nemotecnico = pobjEspecie.Nemotecnico
            vm.CamposBusquedaSelected2.Especie = pobjEspecie.Especie
            vm.CamposBusquedaSelected2.EsAccion = pobjEspecie.EsAccion
            vm._mlogBuscarEspecie2 = True
            vm.HabilitarConsultar = True
            If vm._mlogBuscarEspecie2 = True Then
                vm.ConsultarISIN()
            End If

            If vm.RealizarSplits Then
                vm.ConsultarISIN()
            End If
        End If
    End Sub
  

    Private Sub txtClienteBusqueda_KeyDown(sender As Object, e As KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtClienteBusqueda_KeyDown2(sender As Object, e As KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

    Private Sub BuscarCustodias_Click(sender As Object, e As RoutedEventArgs)
        vm.ConsultarPortafolio()
    End Sub

    Private Sub Aceptar_Click(sender As Object, e As RoutedEventArgs)
        vm.Generar()
    End Sub


    Private Sub C1NumericBox_LostFocus_1(sender As Object, e As RoutedEventArgs)
        If vm.dblFactor > 100 Or vm.dblFactor <= 0 Then
            mostrarMensaje("El factor debe estar entre 0 y 100.", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            vm.dblFactor = 1
        Else
            vm.Recalcular()
        End If
    End Sub

    Private Sub C1NumericBox_LostFocus_2(sender As Object, e As RoutedEventArgs)
        vm.Recalcular()
    End Sub

    Private Sub CodigoEspecie1_LostFocus_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub CodigoCliente2_LostFocus_1(sender As Object, e As RoutedEventArgs)
        If vm.RealizarSplits Then
            vm.ConsultarISIN()
        End If

    End Sub

    Private Sub CheckBox_Checked_1(sender As Object, e As RoutedEventArgs)
        vm.RealizarSplits = True
        If vm.RealizarSplits Then
            If Not IsNothing(vm.CamposBusquedaSelected2) Then
                If Not String.IsNullOrEmpty(vm.CamposBusquedaSelected2.Nemotecnico) Then
                    ' vm.ConsultarISIN()
                    vm.VerSplit = Visibility.Visible
                    vm.VerRecalculo = Visibility.Visible

                Else
                    vm.VerSplit = Visibility.Collapsed
                    vm.VerRecalculo = Visibility.Collapsed
                    mostrarMensaje("Para realizar proceso de Splits es necesario seleccionar valor para el campo <<A la Especie>>.", "Validación", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    vm.RealizarSplits = False
                    '   C1.Silverlight.C1MessageBox.Show("Para realizar proceso de Splits es necesario seleccionar valor para el campo ", AddressOf terminoMensaje)
                End If
            Else
                vm.VerSplit = Visibility.Collapsed
                vm.VerRecalculo = Visibility.Collapsed
                mostrarMensaje("Para realizar proceso de Splits es necesario seleccionar valor para el campo <<A la Especie>>.", "Validación", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                vm.RealizarSplits = False
            End If
        Else
            vm.VerRecalculo = Visibility.Collapsed
            vm.VerSplit = Visibility.Collapsed
        End If
       
    End Sub

    

    'Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
    '    If Not IsNothing(pobjItem) Then
    '        Select Case pstrClaseControl.ToLower

    '            Case "codigociiu" 'SLB20140325 Manejo del Buscador de Código Ciiu
    '                CType(Me.DataContext, ActualizarTitulosViewModel).CamposBusquedaSelected.
    '                CType(Me.DataContext, ActualizarTitulosViewModel).EncabezadoSeleccionado.intIDCodigoCIIU = CType(pobjItem.CodItem, Integer?)
    '                CType(Me.DataContext, ActualizarTitulosViewModel).EncabezadoSeleccionado.strDescripcionCodigoCIIU = pobjItem.Descripcion


    '        End Select
    '    End If
    'End Sub

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

    'JAEZ 20161027
    Private Sub cmbMoneda_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not IsNothing(vm.strMoneda) And Not IsNothing(vm.dtmFechaProceso) Then
            vm.ConsultarTasaConversion(vm.dtmFechaProceso, vm.strMoneda)
        End If

    End Sub

    'JAEZ 20161027
    Private Sub cmbFondo_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If Not IsNothing(vm.strFondo) Then
            vm.ConsultarCuentasFondosDestino(vm.strFondo)
        End If
    End Sub

    Private Sub CheckBox_Click(sender As Object, e As RoutedEventArgs)
        vm.Recalcular()
    End Sub


    Private Sub CantTrasladar_KeyDown(sender As Object, e As KeyEventArgs)
        vm.ContarDecimalesSelected()
    End Sub
End Class


