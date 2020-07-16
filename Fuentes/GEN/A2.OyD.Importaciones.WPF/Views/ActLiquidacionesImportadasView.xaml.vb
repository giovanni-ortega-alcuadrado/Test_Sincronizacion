'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ImportacionLiqView.xaml.vb
'Generado el : 07/19/2011 09:26:12
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web
Imports A2ComunesControl

Partial Public Class ActLiquidacionesImportadasView
    Inherits UserControl
    Dim logcargainicial As Boolean = True
    Dim objViewModel As ActLiquidacionesImportadasViewModel

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub ImportacionLiq_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        objViewModel = CType(Me.DataContext, ActLiquidacionesImportadasViewModel)

        If logcargainicial Then
            logcargainicial = False
            cm.GridTransicion = grdGridForma
            cm.GridViewRegistros = datapager1
            cm.DF = df
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel).NombreView = Me.ToString
        Else
            If Not CType(Me.DataContext, ActLiquidacionesImportadasViewModel).Editando And CType(Me.DataContext, ActLiquidacionesImportadasViewModel).visNavegando = "Visible" Then
                CType(Me.DataContext, ActLiquidacionesImportadasViewModel).Autorefresh()
            End If
        End If
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        df.ValidationSummary.DataContext = Nothing
    End Sub

    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        df.ValidateItem()
        If df.ValidationSummary.HasErrors Then
            df.CancelEdit()
        Else
            df.CommitEdit()
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaorden(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ImportacionLiSelected.IDOrden = CType(pobjItem.CodItem, Integer)
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ordenselec.Cantidad = pobjItem.InfoAdicional01
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ordenselec.Comitente = pobjItem.InfoAdicional02
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ordenselec.Nombre = pobjItem.Clasificacion
            'CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ClienteSelected.IDDepartamentoDoc = CType(pobjItem.InfoAdicional01, Integer)
            'CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ClienteSelected.IDPaisDoc = CType(pobjItem.EtiquetaCodItem, Integer)
            'CType(Me.DataContext, ActLiquidacionesImportadasViewModel).CiudadSelectedDoc.strPoblaciondoc = pobjItem.Nombre
            'CType(Me.DataContext, ActLiquidacionesImportadasViewModel).CiudadSelectedDoc.strdepartamentoDoc = pobjItem.CodigoAuxiliar
            'CType(Me.DataContext, ActLiquidacionesImportadasViewModel).CiudadSelectedDoc.strPaisDoc = pobjItem.InfoAdicional02
        End If
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'df.FindName("BuscadorOrdenes").EstadoItem = CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ImportacionLiSelected.TipoOrden
        CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ImportacionLiSelected.ClaseOrden & "," &
            CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ImportacionLiSelected.Tipo & "." & CType(Me.DataContext, ActLiquidacionesImportadasViewModel).ImportacionLiSelected.IDEspecie
    End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objViewModel.ImportacionLiSelected) Then
            If objViewModel.ImportacionLiSelected.ID.ToString <> CType(sender, Button).Tag Then
                objViewModel.ImportacionLiSelected = objViewModel.ListaImportacionLiq.Where(Function(i) i.ID.ToString = CType(sender, Button).Tag).First
            End If
        Else
            objViewModel.ImportacionLiSelected = objViewModel.ListaImportacionLiq.Where(Function(i) i.ID.ToString = CType(sender, Button).Tag).First
        End If

        objViewModel.CambiarAForma()
    End Sub
End Class


