Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2ControlMenu
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports System.Threading

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: LiquidacionesView.xaml.vb
'Generado el : 05/30/2011 09:18:58
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class LiquidacionesView
    Inherits UserControl
    Dim p As Boolean
    Dim logcargainicial As Boolean = True

    Public Sub New()
        Me.DataContext = New LiquidacionesViewModel
InitializeComponent()
    End Sub

    Private Sub Liquidaciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If logcargainicial Then
            logcargainicial = False
            cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
            cm.DF = df
            CType(Me.DataContext, LiquidacionesViewModel).NombreView = Me.ToString
            CType(Me.DataContext, LiquidacionesViewModel).VmLiquidaciones = Me
        Else
            If Not CType(Me.DataContext, LiquidacionesViewModel).Editando And CType(Me.DataContext, LiquidacionesViewModel).visNavegando = "Visible" Then
                CType(Me.DataContext, LiquidacionesViewModel).RefrescarForma()
            End If
        End If
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
                CType(Me.DataContext, LiquidacionesViewModel).IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                CType(Me.DataContext, LiquidacionesViewModel).IsBusy = False
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
    'Private Sub cm_EventoNuevo(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoNuevoRegistro
    '    CType(df.FindName("txtParcial"), Telerik.Windows.Controls.RadNumericUpDown).ClearValue(Telerik.Windows.Controls.RadNumericUpDown.ValueProperty)
    'End Sub

    Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusCA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusFA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusInt(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel).llevarvalores()
    End Sub
    Private Sub df_LostFocusTA(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel).llevarvalores()
    End Sub
    Private Sub Validaliq(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        If CType(Me.DataContext, LiquidacionesViewModel).estadoregistro = False Then
            Exit Sub
        End If
        CType(Me.DataContext, LiquidacionesViewModel).validaliq()
    End Sub

    Private Sub df_LostFocusVE(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel).llevarvalores()
    End Sub

    'Private Sub df_LostFocusorden(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
    '    CType(Me.DataContext, LiquidacionesViewModel).validaorden()
    'End Sub
    'Private Sub Diasvencimientolostfocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
    '    If Not IsNothing(CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.Plazo) And Not IsNothing(CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.DiasVencimiento) _
    '        And CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.DiasVencimiento > CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.Plazo Then
    '        CType(Me.DataContext, LiquidacionesViewModel).mostrarmensaje()
    '        Exit Sub
    '    End If

    '    'If Not IsNothing(CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.DiasVencimiento) Then
    '    '    CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.Vencimiento = Now
    '    'End If

    'End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesViewModel).ReceptoresOrdenesSelected.IDReceptor = pobjItem.IdItem
            CType(Me.DataContext, LiquidacionesViewModel).ReceptoresOrdenesSelected.Nombre = pobjItem.Nombre
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaPLAZA(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.IDComisionistaOtraPlaza = pobjItem.IdItem
        End If
    End Sub
    Private Sub BuscadorGenerico_finalizoBusquedaPLAZALocal(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, LiquidacionesViewModel).LiquidacioneSelected.IDComisionistaLocal = pobjItem.IdItem
        End If
    End Sub

    Private Sub dgReceptoresOrdenes_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        e.Handled = True
    End Sub

    
    Private Sub btnAplazar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel).Aplazamientos()
    End Sub

Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, LiquidacionesViewModel).Duplicar()
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
                        CType(Me.DataContext, LiquidacionesViewModel).cb.ComitenteSeleccionado = pobjComitente
                        CType(Me.DataContext, LiquidacionesViewModel).cb.IDComitente = pobjComitente.CodigoOYD
                        CType(Me.DataContext, LiquidacionesViewModel).CambioItem("cb")
                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el comitente seleccionado", Me.Name, "BuscadorClienteLista_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class


