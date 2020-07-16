Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: CiudadesView.xaml.vb
'Generado el : 02/24/2011 11:45:58
'Propiedad de Alcuadrado S.A. 2010

Imports A2Utilidades.Mensajes


Partial Public Class RelacionEspeciesLocales
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New RelacionEspeciesLocalesViewModel
InitializeComponent()
    End Sub

    Private Sub RelacionEspeciesLocales_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
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

    Private Sub BuscadorEspecieListaButon_finalizoBusqueda(pstrClaseControl As System.String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                Select Case pstrClaseControl
                    Case "BuscarEspecie"
                        CType(Me.DataContext, RelacionEspeciesLocalesViewModel).cb.IdEspecie = pobjEspecie.Nemotecnico
                    Case "BuscarEspecieExterior"
                        CType(Me.DataContext, RelacionEspeciesLocalesViewModel).cb.IdEspecieExterior = pobjEspecie.Nemotecnico
                    Case "EditarEspecie"
                        If Not IsNothing(CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecieExterior) Then
                            If CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecieExterior = pobjEspecie.Especie Then
                                A2Utilidades.Mensajes.mostrarMensaje("El campo Id Especie Exterior tiene la misma especie." + vbCrLf + "Cambie la especie para poder continuar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecie = Nothing
                            Else
                                CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecie = pobjEspecie.Nemotecnico
                            End If
                        Else
                            CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecie = pobjEspecie.Nemotecnico
                        End If
                    Case "EditarEspecieExterior"
                        If Not IsNothing(CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecie) Then
                            If CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecie = pobjEspecie.Nemotecnico Then
                                A2Utilidades.Mensajes.mostrarMensaje("El campo Id Especie tiene la misma especie." + vbCrLf + "Cambie la especie para poder continuar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecieExterior = Nothing
                            Else
                                CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecieExterior = pobjEspecie.Nemotecnico
                            End If
                        Else
                            CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecieExterior = pobjEspecie.Nemotecnico
                        End If
                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            If pstrClaseControl = "EspeciesExteriorBusqueda" Then
                CType(Me.DataContext, RelacionEspeciesLocalesViewModel).cb.IdEspecieExterior = pobjItem.IdItem
            ElseIf pstrClaseControl = "EspeciesExteriorRegistro" Then
                CType(Me.DataContext, RelacionEspeciesLocalesViewModel).EspecieSelected.IdEspecieExterior = pobjItem.IdItem
            End If
        End If
    End Sub
End Class


