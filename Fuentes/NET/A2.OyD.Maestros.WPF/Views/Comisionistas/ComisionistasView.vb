Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ComisionistasView.xaml.vb
'Generado el : 03/02/2011 17:36:04
'Propiedad de Alcuadrado S.A. 2010

Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class  ComisionistasView
    Inherits UserControl

#Region "Variables"    
    Private mobjVM As ComisionistasViewModel
#End Region

    Public Sub New()
        Me.DataContext = New ComisionistasViewModel
InitializeComponent()
    End Sub

    Private Sub Comisionistas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        'CType(Me.DataContext, ComisionistasViewModel).NombreView = Me.ToString
        CType(Me.DataContext, ComisionistasViewModel).NombreColeccionDetalle = Me.ToString
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
    
    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaCiudadpoblacion(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            'CType(Me.DataContext, ClientesViewModel).ClienteSelected.IDPoblacionDoc = CType(pobjItem.IdItem, Integer)
            'CType(Me.DataContext, ClientesViewModel).ClienteSelected.IDDepartamentoDoc = CType(pobjItem.InfoAdicional01, Integer)
            'CType(Me.DataContext, ClientesViewModel).ClienteSelected.IDPaisDoc = CType(pobjItem.EtiquetaCodItem, Integer)
            CType(Me.DataContext, ComisionistasViewModel).CiudadSelected.strPoblacion = pobjItem.Nombre
            CType(Me.DataContext, ComisionistasViewModel).CiudadSelected.strdepartamento = pobjItem.CodigoAuxiliar
            CType(Me.DataContext, ComisionistasViewModel).CiudadSelected.strPais = pobjItem.InfoAdicional02
            CType(Me.DataContext, ComisionistasViewModel).ComisionistaSelected.IDPais = CType(pobjItem.EtiquetaCodItem, Integer)
            CType(Me.DataContext, ComisionistasViewModel).ComisionistaSelected.IDDepartamento = CType(pobjItem.InfoAdicional01, Integer)
            CType(Me.DataContext, ComisionistasViewModel).ComisionistaSelected.IDPoblacion = CType(pobjItem.IdItem, Integer)
        End If
    End Sub

End Class


