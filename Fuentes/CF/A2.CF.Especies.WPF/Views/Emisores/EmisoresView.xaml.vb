Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: EmisoresView.xaml.vb
'Generado el : 04/19/2011 10:28:38
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class EmisoresView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista

    Public Sub New()
        Me.DataContext = New EmisoresViewModel
InitializeComponent()
    End Sub

    Private Sub Emisores_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, EmisoresViewModel).NombreColeccionDetalle = Me.ToString
        inicializar()
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
        End If
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
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "ciudades"
                    CType(Me.DataContext, EmisoresViewModel).EmisoreSelected.IDPoblacion = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, EmisoresViewModel).CiudadesClaseSelected.Ciudad = pobjItem.Nombre

                    CType(Me.DataContext, EmisoresViewModel).EmisoreSelected.IDDepartamento = CType(pobjItem.InfoAdicional01, Integer)
                    CType(Me.DataContext, EmisoresViewModel).CiudadesClaseSelected.Departamento = pobjItem.CodigoAuxiliar

                    CType(Me.DataContext, EmisoresViewModel).EmisoreSelected.IDPais = CType(pobjItem.EtiquetaCodItem, Integer)
                    CType(Me.DataContext, EmisoresViewModel).CiudadesClaseSelected.Pais = pobjItem.InfoAdicional02
                Case "codigociiu" 'SLB20140325 Manejo del Buscador de Código Ciiu
                    CType(Me.DataContext, EmisoresViewModel).EmisoreSelected.intIdCodigoCIIU = pobjItem.CodItem
                    CType(Me.DataContext, EmisoresViewModel).EmisoreSelected.strDescripcionCodigoCiiu = pobjItem.Descripcion
            End Select
        End If
    End Sub

End Class


