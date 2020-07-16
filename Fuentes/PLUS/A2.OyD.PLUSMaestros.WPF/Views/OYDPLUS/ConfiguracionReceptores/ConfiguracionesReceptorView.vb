Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: TipoNegocioView.xaml.vb
'Generado el : 11/07/2012 18:36:05
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ConfiguracionesReceptorView
    Inherits UserControl
    Dim vmConfiguracionesReceptor As ConfiguracionReceptorViewModel
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Try

            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Dim objVMA2Utils As A2UtilsViewModel
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)

            objVMA2Utils = New A2UtilsViewModel("CONFIGURACIONRECEPTOR", mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objVMA2Utils)

            Me.DataContext = New ConfiguracionReceptorViewModel
InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "ConfiguracionesReceptorView", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TipoNegocio_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        vmConfiguracionesReceptor = CType(Me.DataContext, ConfiguracionReceptorViewModel)
        vmConfiguracionesReceptor.NombreView = Me.ToString
    End Sub

    Private Sub dgDetalles_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)

        e.Handled = True
        'df.ValidateItem()
    End Sub

    Private Sub Buscador_finalizoBusquedaespeciesBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjItem) Then
            If Not IsNothing(vmConfiguracionesReceptor) Then

            End If
        End If
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(vmConfiguracionesReceptor) Then
            vmConfiguracionesReceptor.BorrarClienteBuscador()
        End If
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(vmConfiguracionesReceptor) Then
            vmConfiguracionesReceptor.BorrarEspecieBuscador()
        End If
    End Sub

    Private Sub Buscador_finalizoBusquedaitemsBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            If Not IsNothing(vmConfiguracionesReceptor) Then
                If Not IsNothing(vmConfiguracionesReceptor.ReceptoresClientesAutorizadoSelected) Then
                    vmConfiguracionesReceptor.ReceptoresClientesAutorizadoSelected.CodigoReceptorCliente = pobjItem.IdItem
                    vmConfiguracionesReceptor.ReceptoresClientesAutorizadoSelected.NombreReceptorCliente = pobjItem.Descripcion

                    vmConfiguracionesReceptor.ReceptoresClientesAutorizadoSelected.IDComitente = Nothing
                    vmConfiguracionesReceptor.ReceptoresClientesAutorizadoSelected.NombreCliente = "TODOS"

                    vmConfiguracionesReceptor.BorrarClienteBuscador()
                    vmConfiguracionesReceptor.CodigoReceptorBuscar = pobjItem.IdItem
                End If
            End If
        End If
    End Sub

    Private Sub Buscador_finalizoBusquedaclientesBusqueda(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            If Not IsNothing(vmConfiguracionesReceptor) Then
                If Not IsNothing(vmConfiguracionesReceptor.ReceptoresClientesAutorizadoSelected) Then
                    vmConfiguracionesReceptor.ReceptoresClientesAutorizadoSelected.IDComitente = pobjComitente.IdComitente
                    vmConfiguracionesReceptor.ReceptoresClientesAutorizadoSelected.NombreCliente = pobjComitente.Nombre
                End If
            End If
        End If

    End Sub

    Private Sub btnLimpiarReceptor_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(vmConfiguracionesReceptor) Then
            vmConfiguracionesReceptor.BorrarReceptorBuscador()
        End If
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If

    End Sub
    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                vmConfiguracionesReceptor.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                vmConfiguracionesReceptor.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As System.String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(vmConfiguracionesReceptor) Then
                    If Not IsNothing(vmConfiguracionesReceptor.ConceptosXReceptoSelected) Then
                        vmConfiguracionesReceptor.ConceptosXReceptoSelected.IdConcepto = pobjItem.IdItem()
                        vmConfiguracionesReceptor.ConceptosXReceptoSelected.Descripcion = pobjItem.Nombre()
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener el resultado del buscador.", Me.ToString(), "BuscadorGenerico_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
End Class


