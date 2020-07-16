Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ClienteAgrupadorView.xaml.vb
'Generado el : 03/06/2012 17:14:59
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ClienteResponsableView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorClienteListaButon

    Public Sub New()
        Me.DataContext = New ClienteResponsableViewModel
InitializeComponent()
    End Sub

    Private Sub ClienteResponsable_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        'cm.DF = df
        CType(Me.DataContext, ClienteResponsableViewModel).NombreView = Me.ToString
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, ClienteResponsableViewModel).ClienteEncabezadoSelected.IdEncabezado = pobjItem.CodigoOYD
            CType(Me.DataContext, ClienteResponsableViewModel).ClienteEncabezadoSelected.Nombre = pobjItem.Nombre
            CType(Me.DataContext, ClienteResponsableViewModel).ClienteEncabezadoSelected.nrodocumento = pobjItem.NroDocumento
            CType(Me.DataContext, ClienteResponsableViewModel).ConsultarComitente(pobjItem.CodigoOYD)
        End If
    End Sub
    Private Sub Buscador_Cliente_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'df.FindName("Buscador_Cliente").TipoVinculacion = "CA"
        'df.FindName("Buscador_Cliente").Agrupamiento = "CA," + CType(Me.DataContext, ClienteResponsableViewModel).ClienteAgrupadoSelected.TipoIdentificacion + "." + CType(Me.DataContext, ClienteResponsableViewModel).ClienteAgrupadoSelected.NroDocumento + "-"

    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaComitente(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "idcomitente"
                    CType(Me.DataContext, ClienteResponsableViewModel).cb.Comitente = pobjItem.CodigoOYD
            End Select
        End If
    End Sub
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "IDCiudadDocu"
                        CType(Me.DataContext, ClienteResponsableViewModel).ClienteResponsableSelected.IDPoblacion = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClienteResponsableViewModel).ClienteResponsableSelected.IDDepartamento = CType(pobjItem.InfoAdicional01, Integer)
                        CType(Me.DataContext, ClienteResponsableViewModel).ClienteResponsableSelected.IdPais = CType(pobjItem.EtiquetaCodItem, Integer)
                        CType(Me.DataContext, ClienteResponsableViewModel).ClienteResponsableSelected.NombreCuidad = pobjItem.Descripcion

                   
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del Buscador generico", _
                                 Me.ToString(), "BuscadorGenerico_finalizoBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
End Class


