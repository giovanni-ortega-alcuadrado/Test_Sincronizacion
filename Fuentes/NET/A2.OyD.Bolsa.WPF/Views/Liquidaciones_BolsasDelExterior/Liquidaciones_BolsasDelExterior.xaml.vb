Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes


Partial Public Class Liquidaciones_BolsasDelExterior
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New Liquidaciones_BolsasDelExteriorViewModel
InitializeComponent()
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

    Private Sub Liquidaciones_BolsasDelExterior_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
    End Sub

    Private Sub BuscadorEspecieListaButon_finalizoBusqueda(pstrClaseControl As System.String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                Select Case pstrClaseControl
                    Case "BuscarEspecie"
                        CType(Me.DataContext, Liquidaciones_BolsasDelExteriorViewModel).LiquidacionSelected.IdEspecie = pobjEspecie.Especie
                        CType(Me.DataContext, Liquidaciones_BolsasDelExteriorViewModel).NombreEspecie = pobjEspecie.Nemotecnico
                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As System.String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "BuscarEspecie"
                        CType(Me.DataContext, Liquidaciones_BolsasDelExteriorViewModel).LiquidacionSelected.IdEspecie = pobjItem.IdItem
                        CType(Me.DataContext, Liquidaciones_BolsasDelExteriorViewModel).NombreEspecie = pobjItem.IdItem
                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class