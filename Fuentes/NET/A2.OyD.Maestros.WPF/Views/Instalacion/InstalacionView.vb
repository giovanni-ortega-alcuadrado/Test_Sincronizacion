Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: InstalacionView.xaml.vb
'Generado el : 04/28/2011 11:33:03
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OYD.OYDServer.RIA.Web

Partial Public Class  InstalacionView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New InstalacionViewModel
InitializeComponent()
    End Sub

    Private Sub Instalacion_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, InstalacionViewModel).NombreColeccionDetalle = Me.ToString
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
                CType(Me.DataContext, InstalacionViewModel).IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                CType(Me.DataContext, InstalacionViewModel).IsBusy = False
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

    '

    Private Sub BuscadorGenerico_finalizoBusquedaD(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, InstalacionViewModel).InstalacioSelected.CtaContableContraparteNotasCxC = pobjItem.IdItem
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "CCostoContraparte"
                    CType(Me.DataContext, InstalacionViewModel).InstalacioSelected.CCostoContraparte = pobjItem.IdItem
                Case "CCosto"
                    CType(Me.DataContext, InstalacionViewModel).InstalacioSelected.CCosto = pobjItem.IdItem
            End Select
        End If
    End Sub

    'SLB20140516 Manejo por separado de la selección de la cuenta contable
    Private Sub BuscadorGenerico_finalizoBusquedaCtaContable(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, InstalacionViewModel).InstalacioSelected.CtaContable = pobjItem.IdItem
        End If
    End Sub

    'SLB20140516 Manejo por separado de la selección de la cuenta contable
    Private Sub BuscadorGenerico_finalizoBusquedaCtaContableContraparte(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, InstalacionViewModel).InstalacioSelected.CtaContableContraparte = pobjItem.IdItem
        End If
    End Sub

    ''' <summary>
    ''' Manejo de la cuenta contable de clientes
    ''' </summary>
    ''' <param name="pstrClaseControl"></param>
    ''' <param name="pobjItem"></param>
    ''' <remarks>SLB20140516</remarks>
    Private Sub BuscadorGenerico_finalizoBusquedaCta(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, InstalacionViewModel).InstalacioSelected.CtaContableClientes = pobjItem.IdItem
        End If
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub
    Private Sub Consultarcontraparteotc(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, InstalacionViewModel).llenarcontraparteotc()
    End Sub

End Class


