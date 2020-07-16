Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class BloquearTituloView
    Inherits UserControl
    Dim vm As New BloquearTituloViewModel

    Public Sub New()
        Me.DataContext = New BloquearTituloViewModel
InitializeComponent()
    End Sub

    Private Sub BloquearTituloView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, BloquearTituloViewModel)
        CType(Me.DataContext, BloquearTituloViewModel).NombreView = Me.ToString
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, BloquearTituloViewModel)._mlogBuscarCliente = False
            CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.CodigoCliente = pobjItem.CodigoOYD
            CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.NombreCliente = pobjItem.Nombre
            CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.Nemotecnico = String.Empty
            CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.Especie = String.Empty
            CType(Me.DataContext, BloquearTituloViewModel).HabilitarBuscEspe = True
            If Not IsNothing(CType(Me.DataContext, BloquearTituloViewModel).ListaCustodiasCliente) Then
                CType(Me.DataContext, BloquearTituloViewModel).ListaCustodiasCliente.Clear()
                CType(Me.DataContext, BloquearTituloViewModel).TextoGrid = "Titulos Encontrados"
            End If
            CType(Me.DataContext, BloquearTituloViewModel)._mlogBuscarCliente = True
        End If
    End Sub

    'Private Sub Buscador_finalizoBusquedaespecies(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorEspecies)
    '    If Not IsNothing(pobjItem) Then
    '        CType(Me.DataContext, BloquearTituloViewModel)._mlogBuscarEspecie = False
    '        CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.Nemotecnico = pobjItem.Nemotecnico
    '        CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.Especie = pobjItem.Especie
    '        CType(Me.DataContext, BloquearTituloViewModel)._mlogBuscarEspecie = True
    '    End If
    'End Sub

    'Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
    '    df.ValidateItem()
    '    If df.ValidationSummary.HasErrors Then
    '        df.CancelEdit()
    '    Else
    '        df.CommitEdit()
    '        df.FindName("dgEspeciesISINFungible").EndEdit()
    '    End If
    'End Sub

    Private Sub BuscarCustodias_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, BloquearTituloViewModel).BuscarCustadias()
    End Sub

    Private Sub Aceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
                'DataGrid1.EndEditRow(True)
        'DataGrid1.EndEdit()
        CType(Me.DataContext, BloquearTituloViewModel).CambiarEstadoCustodias()
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, BloquearTituloViewModel)._mlogBuscarEspecie = False
            CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.Nemotecnico = pobjItem.IdItem
            CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.Especie = pobjItem.Nombre
            CType(Me.DataContext, BloquearTituloViewModel)._mlogBuscarEspecie = True
        End If
    End Sub

    Private Sub txtClienteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

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

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(vm.ListaCustodiaSeleccionada) Then
                If vm.ListaCustodiaSeleccionada.ObjParaBloquear Then
                    vm.dblCantidadTotal = vm.dblCantidadTotal + vm.ListaCustodiaSeleccionada.CantidadBloquear
                Else
                    vm.dblCantidadTotal = vm.dblCantidadTotal - vm.ListaCustodiaSeleccionada.CantidadBloquear
                End If

                vm.AsignarMotivoBloqueo()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar valores para el motivo de bloqueo",
                                Me.ToString(), "CheckBox_Checked", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Cantidad14dec_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(vm.ListaCustodiasCliente) Then
                vm.dblCantidadTotal = 0
                For Each li In vm.ListaCustodiasCliente
                    If li.ObjParaBloquear Then
                        vm.dblCantidadTotal = vm.dblCantidadTotal + li.CantidadBloquear
                    End If
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al totalizar los registros seleccionados.",
                    Me.ToString(), "Cantidad4dec_LostFocus", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Cantidad4dec_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(vm.ListaCustodiasCliente) Then
                vm.dblCantidadTotal = 0
                For Each li In vm.ListaCustodiasCliente
                    If li.ObjParaBloquear Then
                        vm.dblCantidadTotal = vm.dblCantidadTotal + li.CantidadBloquear
                    End If
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al totalizar los registros seleccionados.",
                    Me.ToString(), "Cantidad4dec_LostFocus", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

End Class