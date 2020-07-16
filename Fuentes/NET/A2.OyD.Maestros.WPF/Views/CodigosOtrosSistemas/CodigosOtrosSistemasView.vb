Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: CodigosOtrosSistemasView.xaml.vb
'Generado el : 04/27/2011 15:43:42
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class CodigosOtrosSistemasView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista

    Public Sub New()
        Me.DataContext = New CodigosOtrosSistemasViewModel
InitializeComponent()
    End Sub

    Private Sub CodigosOtrosSistemas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, CodigosOtrosSistemasViewModel).NombreColeccionDetalle = Me.ToString
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
                CType(Me.DataContext, CodigosOtrosSistemasViewModel).IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                CType(Me.DataContext, CodigosOtrosSistemasViewModel).IsBusy = False
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
    

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub 
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "comitente"
                    CType(Me.DataContext, CodigosOtrosSistemasViewModel).CodigosOtrosSistemaSelected.Comitente = pobjItem.IdComitente
                    CType(Me.DataContext, CodigosOtrosSistemasViewModel).CodigosOtrosSistemaSelected.NombreComitente = pobjItem.Nombre
                    CType(Me.DataContext, CodigosOtrosSistemasViewModel).ComitenteClaseSelected.Comitente = pobjItem.Nombre
            End Select
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda_ControlBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "comitente"
                    CType(Me.DataContext, CodigosOtrosSistemasViewModel).cb.Comitente = pobjItem.IdComitente
                    CType(Me.DataContext, CodigosOtrosSistemasViewModel).cb.NombreComitente = pobjItem.Nombre
            End Select
        End If
    End Sub
End Class


