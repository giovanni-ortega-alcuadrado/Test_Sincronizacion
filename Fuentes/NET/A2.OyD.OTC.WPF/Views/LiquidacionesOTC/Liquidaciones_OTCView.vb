Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: Liquidaciones_OTCView.xaml.vb
'Generado el : 09/21/2012 11:57:23
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  Liquidaciones_OTCView
    Inherits UserControl
    Private mobjVM As Liquidaciones_OTCViewModel

    Public Sub New()
        Me.DataContext = New Liquidaciones_OTCViewModel
InitializeComponent()
    End Sub

    Private Sub Liquidaciones_OTC_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, Liquidaciones_OTCViewModel).NombreView = Me.ToString
        CType(Me.DataContext, Liquidaciones_OTCViewModel).NombreColeccionDetalle = Me.ToString
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

    Private Sub BuscadorEspecies_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            CType(Me.DataContext, Liquidaciones_OTCViewModel).ConsultodesdeBoton = False
            CType(Me.DataContext, Liquidaciones_OTCViewModel).Liquidaciones_OTSelected.IDESPECIE = pobjEspecie.Nemotecnico
            CType(Me.DataContext, Liquidaciones_OTCViewModel).Liquidaciones_OTSelected.NOMBREESPECIE = pobjEspecie.Especie
            CType(Me.DataContext, Liquidaciones_OTCViewModel).Liquidaciones_OTSelected.ESPECIE_ESACCION = IIf(pobjEspecie.EsAccion, "A", "C")
            If CType(Me.DataContext, Liquidaciones_OTCViewModel).Liquidaciones_OTSelected.ESPECIE_ESACCION = "A" Then
                CType(Me.DataContext, Liquidaciones_OTCViewModel).logHabilitaAccion = False
            Else
                CType(Me.DataContext, Liquidaciones_OTCViewModel).logHabilitaAccion = True
            End If
            CType(Me.DataContext, Liquidaciones_OTCViewModel).ConsultodesdeBoton = True
            'CType(Me.DataContext, Liquidaciones_OTCViewModel).mstrEsAccion = IIf(pobjEspecie.EsAccion, "A", "C")
        End If
    End Sub

    Private Sub BuscarCliente_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, Liquidaciones_OTCViewModel).ConsultodesdeBoton = False
            CType(Me.DataContext, Liquidaciones_OTCViewModel).Liquidaciones_OTSelected.IDCOMITENTE = pobjComitente.CodigoOYD
            CType(Me.DataContext, Liquidaciones_OTCViewModel).Liquidaciones_OTSelected.NOMBRECLIENTE = pobjComitente.Nombre
            CType(Me.DataContext, Liquidaciones_OTCViewModel).ConsultodesdeBoton = True
        End If
    End Sub

    Private Sub BuscarReceptor_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjReceptor As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjReceptor) Then
            CType(Me.DataContext, Liquidaciones_OTCViewModel).ReceptorSelected.IDReceptor = pobjReceptor.CodItem
            'SLB20130401
            If CType(Me.DataContext, Liquidaciones_OTCViewModel).ListaReceptores.Count = 1 Then
                CType(Me.DataContext, Liquidaciones_OTCViewModel).ReceptorSelected.Lider = True
                CType(Me.DataContext, Liquidaciones_OTCViewModel).ReceptorSelected.Porcentaje = 100
            End If
        End If
    End Sub

    Private Sub BuscarRepresentante_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjRepresentante As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjRepresentante) Then
            CType(Me.DataContext, Liquidaciones_OTCViewModel).ConsultodesdeBoton = False
            CType(Me.DataContext, Liquidaciones_OTCViewModel).Liquidaciones_OTSelected.IDREPRESENTANTELEGAL = pobjRepresentante.CodItem
            CType(Me.DataContext, Liquidaciones_OTCViewModel).Liquidaciones_OTSelected.NOMBREREPRESENTANTE = pobjRepresentante.Nombre
            CType(Me.DataContext, Liquidaciones_OTCViewModel).ConsultodesdeBoton = True
        End If
    End Sub

    Private Sub BuscarCliente_finalizoBusqueda_Consulta(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, Liquidaciones_OTCViewModel).ConsultodesdeBoton = True
            CType(Me.DataContext, Liquidaciones_OTCViewModel).cb.IDCOMITENTE = pobjComitente.CodigoOYD
        End If
    End Sub

    Private Sub BuscadorEspecies_finalizoBusqueda_Consulta(ByVal pstrClaseControl As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            CType(Me.DataContext, Liquidaciones_OTCViewModel).cb.IDESPECIE = pobjEspecie.Nemotecnico
        End If
    End Sub


    Private Sub CampoNumerico_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)

        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If

    End Sub

End Class


