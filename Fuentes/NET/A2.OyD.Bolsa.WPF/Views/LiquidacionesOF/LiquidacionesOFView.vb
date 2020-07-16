Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2ControlMenu
Imports A2Utilidades.Mensajes
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: LiquidacionesView.xaml.vb
'Generado el : 05/30/2011 09:18:58
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class LiquidacionesOFView
    Inherits UserControl
    Dim p As Boolean

    Public Sub New()
        Me.DataContext = New LiquidacionesOFViewModel
InitializeComponent()
    End Sub

    Private Sub Liquidaciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, LiquidacionesOFViewModel).NombreView = Me.ToString
        CType(Me.DataContext, LiquidacionesOFViewModel).VmLiquidaciones = Me
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        End If
    End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                CType(Me.DataContext, LiquidacionesOFViewModel).IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                CType(Me.DataContext, LiquidacionesOFViewModel).IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
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

    Private Sub df_LostFocusPrecio(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesOFViewModel).llevarvalores()
    End Sub

    Private Sub df_LostFocusValor(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesOFViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusCA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesOFViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusFA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesOFViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusInt(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesOFViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusTA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesOFViewModel).llevarvalores()
    End Sub
    Private Sub Validaliq(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        If CType(Me.DataContext, LiquidacionesOFViewModel).estadoregistro = False Then
            Exit Sub
        End If
        CType(Me.DataContext, LiquidacionesOFViewModel).validaliq()
    End Sub


    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesOFViewModel).ReceptoresOrdenesOFSelected.IDReceptor = pobjItem.IdItem
            CType(Me.DataContext, LiquidacionesOFViewModel).ReceptoresOrdenesOFSelected.Nombre = pobjItem.Nombre
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaPLAZA(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesOFViewModel).LiquidacionesOFSelected.IDComisionistaOtraPlaza = pobjItem.IdItem
        End If
    End Sub
    Private Sub BuscadorGenerico_finalizoBusquedaPLAZALocal(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesOFViewModel).LiquidacionesOFSelected.IDComisionistaLocal = pobjItem.IdItem
        End If
    End Sub

    Private Sub dgReceptoresOrdenes_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        e.Handled = True
    End Sub

    
    Private Sub btnAplazar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesOFViewModel).Aplazamientos()
    End Sub

Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesOFViewModel).Duplicar()
    End Sub

Private Sub LiquidacionesBuscar_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        DirectCast(sender, System.Windows.Controls.TextBox).Focus()
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
                        CType(Me.DataContext, LiquidacionesOFViewModel).cb.ComitenteSeleccionado = pobjComitente
                        CType(Me.DataContext, LiquidacionesOFViewModel).cb.IDComitente = pobjComitente.CodigoOYD
                        CType(Me.DataContext, LiquidacionesOFViewModel).CambioItem("cb")
                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el comitente seleccionado", Me.Name, "BuscadorClienteLista_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
