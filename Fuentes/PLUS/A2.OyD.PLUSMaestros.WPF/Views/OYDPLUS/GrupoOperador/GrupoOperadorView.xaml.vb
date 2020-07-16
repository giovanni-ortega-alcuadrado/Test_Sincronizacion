Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: GruposEconomicosView.xaml.vb
'Generado el : 03/06/2012 17:14:59
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class GruposOperadoresView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorClienteListaButon

    Public Sub New()
        Me.DataContext = New GrupoOperadorViewModel
        InitializeComponent()
    End Sub

    Private Sub GruposEconomicos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        CType(Me.DataContext, GrupoOperadorViewModel).NombreView = Me.ToString
    End Sub

    Private Sub Buscador_finalizoBusquedaGrupos(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            'CType(Me.DataContext, GrupoOperadorViewModel).GrupoOperadorSelected.ComitenteLider = pobjItem.CodigoOYD
            'CType(Me.DataContext, GrupoOperadorViewModel).GrupoOperadorSelected.NombreLider = pobjItem.Nombre
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "Operador"
                    'CType(Me.DataContext, GrupoOperadorViewModel).validarNuevoDetalle(pobjItem)
            End Select
        End If
    End Sub

    Private Sub Buscador_finalizoBusquedaCliente(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "Operador"
                    CType(Me.DataContext, GrupoOperadorViewModel).cb.Operador = pobjItem.Descripcion
            End Select
        End If
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As System.String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(CType(Me.DataContext, GrupoOperadorViewModel)) Then
                    If Not IsNothing(CType(Me.DataContext, GrupoOperadorViewModel).GrupoOperadorSelected) Then
                        If Not IsNothing(CType(Me.DataContext, GrupoOperadorViewModel).GrupoOperadorSelected) Then
                            If Not IsNothing(CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected) Then
                                CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected.NombreOperador = pobjItem.Descripcion
                                CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected.Documento = pobjItem.CodItem
                                CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected.Receptor = pobjItem.Nombre
                                CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected.Login = pobjItem.IdItem
                                CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected.IDEmpleado = CInt(pobjItem.EtiquetaIdItem)
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener el resultado del buscador.", Me.ToString(), "BuscadorGenerico_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            Dim objItem As OyDPLUSMaestros.DetalleGrupoOperadores = CType(CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Tag, OyDPLUSMaestros.DetalleGrupoOperadores)
            If Not IsNothing(objItem) Then
                If IsNothing(CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected) Then
                    CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected = objItem
                Else
                    If CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected.IDGrupoOperadorDetalle <> objItem.IDGrupoOperadorDetalle Then
                        CType(Me.DataContext, GrupoOperadorViewModel).DetalleGrupoOperadorSelected = objItem
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener el resultado del buscador.", Me.ToString(), "BuscadorGenericoListaButon_GotFocus", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Cm_EventoCambiarALista(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(Me.DataContext) Then
            CType(Me.DataContext, GrupoOperadorViewModel).ConsultarGruposOperadores().GetAwaiter()
        End If
    End Sub
End Class


