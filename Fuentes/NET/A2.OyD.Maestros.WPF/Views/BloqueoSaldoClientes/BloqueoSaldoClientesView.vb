Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: BloqueoSaldoClientesView.xaml.vb
'Generado el : 04/11/2012 08:34:27
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class BloqueoSaldoClientesView
    Inherits UserControl

    '''JCM20170727
#Region "Variables"

    Private mobjVM As BloqueoSaldoClientesViewModel

#End Region


    Public Sub New()
        Me.DataContext = New BloqueoSaldoClientesViewModel
InitializeComponent()
    End Sub

    Private Sub BloqueoSaldoClientes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, BloqueoSaldoClientesViewModel).NombreView = Me.ToString

        inicializar()
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        End If
    End Sub

    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        df.ValidateItem()
        If Not IsNothing(df.ValidationSummary) Then
            If df.ValidationSummary.HasErrors Then
                df.CancelEdit()
            Else
                df.CommitEdit()
            End If
        End If
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.IDComitente = pobjItem.CodigoOYD
            CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.NombreComitente = pobjItem.Nombre
            CType(Me.DataContext, BloqueoSaldoClientesViewModel).ObtenerValorSaldoEncargo()
        End If
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes1(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, BloqueoSaldoClientesViewModel).cb.IDComitente = pobjItem.CodigoOYD
            CType(Me.DataContext, BloqueoSaldoClientesViewModel).cb.NombreComitente = pobjItem.Nombre
            CType(Me.DataContext, BloqueoSaldoClientesViewModel).CambioItem("cb")
        End If
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "compania"
                    CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.intIDCompania = pobjItem.IdItem
                    CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.strNombreCompania = pobjItem.Nombre
                    If Not IsNothing(CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.IDComitente) Then
                        CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.IDComitente = ""
                        CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.NombreComitente = ""
                    End If
                    CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.intIDEncargo = Nothing
                    CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.strDetalleEncargo = ""
                Case "encargo"
                    CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.intIDEncargo = pobjItem.IdItem
                    CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.strDetalleEncargo = pobjItem.Descripcion
                    If Not IsNothing(CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.IDComitente) Then
                        CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.IDComitente = ""
                        CType(Me.DataContext, BloqueoSaldoClientesViewModel).BloqueoSaldoClienteSelected.NombreComitente = ""
                    End If
            End Select
        End If
    End Sub

    Private Sub btnLimpiarCia_Dataform_Edicion_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.LimpiarCamposPasivo("compania")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCia_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private Sub btnLimpiarEncargos_Dataform_Edicion_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.LimpiarCamposPasivo("encargos")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEncargos_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, BloqueoSaldoClientesViewModel)
        End If
    End Sub



    Private Sub BuscadorGenericoListaButon_GotFocus(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM) Then
            If mobjVM.BloqueoSaldoClienteSelected.intIDCompania > 0 Then
                CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = mobjVM.BloqueoSaldoClienteSelected.intIDCompania
            Else
                CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = ""
            End If
        End If
    End Sub

    Private Sub BuscadorClienteListaButon_GotFocus(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM) Then
            If mobjVM.BloqueoSaldoClienteSelected.intIDEncargo > 0 Or
                    mobjVM.BloqueoSaldoClienteSelected.intIDCompania > 0 Then
                CType(sender, A2ComunesControl.BuscadorClienteListaButon).Agrupamiento = "clientescompanias" & "%%++" & mobjVM.BloqueoSaldoClienteSelected.intIDEncargo.ToString()


            Else
                CType(sender, A2ComunesControl.BuscadorClienteListaButon).Agrupamiento = ""
            End If
        End If
    End Sub

    Private Sub chkValorTotal_Checked(sender As Object, e As RoutedEventArgs)
        'mobjVM.ObtenerValorSaldoEncargo()
    End Sub


    Private Sub chkValorTotal_Unchecked(sender As Object, e As RoutedEventArgs)
        ' mobjVM.ObtenerValorSaldoEncargo()
    End Sub

End Class


