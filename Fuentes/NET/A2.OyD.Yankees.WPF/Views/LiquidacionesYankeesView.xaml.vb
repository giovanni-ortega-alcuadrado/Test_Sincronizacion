Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2ControlMenu
Imports A2Utilidades.Mensajes
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: LiquidacionesView.xaml.vb
'Generado el : 05/30/2011 09:18:58
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class LiquidacionesYankeesView
    Inherits UserControl
    Dim p As Boolean
    Public Sub New()
        Me.DataContext = New LiquidacionesYankeesViewModel
InitializeComponent()
    End Sub
    Private Sub Liquidaciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, LiquidacionesYankeesViewModel).NombreView = Me.ToString
        ' CType(Me.DataContext, LiquidacionesYankeesViewModel).VmLiquidaciones = Me
    End Sub
    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        ' If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        'End If
    End Sub
    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        df.ValidateItem()
        'If df.ValidationSummary.HasErrors Then
        '    df.CancelEdit()
        'Else
        '    df.CommitEdit()
        'End If
    End Sub

    Private Sub ValorTotal_LostFocus(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).txtTotal_LostFocus()
    End Sub

    Private Sub TasaConversion_LostFocus(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).dblTasaConversion_LostFocus()
    End Sub


    Private Sub FechaUltPagoCupon_LostFocus(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).dtmFechaUltPagoCupon_LostFocus()
    End Sub


    Private Sub Preciolimpiocontra_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).Valor_PrecioSucio_DetalleContraparte()
    End Sub
    Private Sub ValorNominalcontra_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).ValorNominal_DetalleContraparte()
    End Sub
    Private Sub TasaSpotTRM_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).TasaSpotTRM_DetalleContraparte()
    End Sub
    Private Sub VlrTraslComision_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).SubVlrTraslComision_DetalleContraparte()
    End Sub
    Private Sub TasaConversiondetalle_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).TasaConversion_DetalleContraparte()
    End Sub

    Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).traercalculosinteresespecies()
    End Sub
    Private Sub df_LostFocusCA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        'CType(Me.DataContext, LiquidacionesYankeesViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusFA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesYankeesViewModel).dblTasaSpot_LostFocus()
    End Sub
    Private Sub df_LostFocusInt(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        'CType(Me.DataContext, LiquidacionesYankeesViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusTA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        'CType(Me.DataContext, LiquidacionesYankeesViewModel).llevarvalores()
    End Sub
    Private Sub Validaliq(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        'If CType(Me.DataContext, LiquidacionesYankeesViewModel).estadoregistro = False Then
        '    Exit Sub
        'End If
        'CType(Me.DataContext, LiquidacionesYankeesViewModel).validaliq()
    End Sub
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesYankeesViewModel).ReceptoresSelected.IDReceptor = pobjItem.IdItem
            CType(Me.DataContext, LiquidacionesYankeesViewModel).ReceptoresSelected.Nombre = pobjItem.Nombre
        End If
    End Sub
    Private Sub dgReceptoresOrdenes_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        e.Handled = True
    End Sub
    'Private Sub ValidationSummary_FocusingInvalidControl(ByVal sender As System.Object, ByVal e As System.Windows.Controls.FocusingInvalidControlEventArgs)
    '    CType(Me.DataContext, LiquidacionesYankeesViewModel).seleccionarCampoTab(e.Target.PropertyName)
    'End Sub
    Private Sub LiquidacionesBuscar_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        DirectCast(sender, System.Windows.Controls.TextBox).Focus()
    End Sub
    Private Sub BuscadorGenerico_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "Comitente"
                    'CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.lngIDComitente = pobjItem.IdComitente
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarCliente = False
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.lngIDComitente = pobjItem.CodigoOYD
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.strComitente = pobjItem.Nombre
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarCliente = True
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).buscarordenante("E")
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ReceptoresPorClientes_Consultar()
                Case "ComitenteGrid"
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarCliente = False
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.lngIDComitente = pobjItem.IdComitente
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.strComitente = pobjItem.Nombre
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarCliente = True
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).buscarordenante("D")
                    'Case "OrdenanteGrid"
                    '    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.lngIDOrdenante = pobjItem.IdComitente
                    '    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.strOrdenante = pobjItem.Nombre
            End Select
        End If
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "especies"
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarEspecie = False
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.strIDEspecie = pobjItem.IdItem
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.strNombreEspecie = pobjItem.Nombre
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).buscarisin()
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarEspecie = True
                Case "ClienteExerno"
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarClienteExterno = False
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.lngIDClienteExterno = pobjItem.IdItem
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.strClienteExterno = pobjItem.Nombre
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.strVendedor = pobjItem.Descripcion
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.lngIDDepositoExtranjero = pobjItem.EtiquetaIdItem
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.strNumeroCuenta = pobjItem.InfoAdicional01
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.strNombreTitular = pobjItem.InfoAdicional02
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarClienteExterno = True
                Case "ClienteExernoGrid" 'SLB20130906
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarClienteExterno = False
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.lngIDClienteExterno = pobjItem.IdItem
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.strClienteExterno = pobjItem.Nombre
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.strVendedor = pobjItem.Descripcion
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.lngIDDepositoExtranjero = pobjItem.EtiquetaIdItem
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.DescripcionDeposito = pobjItem.CodigoAuxiliar
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.strNumeroCuenta = pobjItem.InfoAdicional01
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.strNombreTitular = pobjItem.InfoAdicional02
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarClienteExterno = True
                Case "Ordenante"
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarOrdenante = False
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.lngIDOrdenante = pobjItem.IdItem
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.strOrdenante = pobjItem.Nombre
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarOrdenante = True
                Case "OrdenanteGrid"
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarOrdenante = False
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.lngIDOrdenante = pobjItem.IdItem
                    CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.strOrdenante = pobjItem.Nombre
                    CType(Me.DataContext, LiquidacionesYankeesViewModel)._mlogBuscarOrdenante = True
            End Select

        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se dispara el evento comitenteAsignado en el buscador de clientes (control buscador clientes lista)
    ''' </summary>
    ''' <param name="pstrClaseControl">Permite identificar el llamado</param>
    ''' <param name="pobjComitente">Datos del comitente seleccionado en el buscador</param>
    ''' <remarks></remarks>
    Private Sub BuscadorClienteListaButon_comitenteAsignado(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                Select Case pstrClaseControl.ToLower()
                    Case "idcomitentebuscar"
                        CType(Me.DataContext, LiquidacionesYankeesViewModel).cb.ComitenteSeleccionado = pobjComitente
                        CType(Me.DataContext, LiquidacionesYankeesViewModel).cb.IDComitente = pobjComitente.CodigoOYD
                        CType(Me.DataContext, LiquidacionesYankeesViewModel).CambioItem("cb")
                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el comitente seleccionado", Me.Name, "BuscadorClienteLista_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        DirectCast(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = CType(Me.DataContext, LiquidacionesYankeesViewModel).ListaLiquidacionesYankeesSelected.lngIDComitente
    End Sub

    Private Sub BuscadorGenericoListaButonGri_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        DirectCast(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = CType(Me.DataContext, LiquidacionesYankeesViewModel).ContraparteSelected.lngIDComitente
    End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub


End Class


