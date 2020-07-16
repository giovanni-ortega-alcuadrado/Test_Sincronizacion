Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: TipoNegocioView.xaml.vb
'Generado el : 11/07/2012 18:36:05
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class TipoNegocioView
    Inherits UserControl
    Dim vmTipoNegocio As TipoNegocioViewModel
    Dim objVMA2Utils As A2UtilsViewModel
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Try

            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)

            objVMA2Utils = New A2UtilsViewModel("TIPONEGOCIO", mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objVMA2Utils)

            Me.DataContext = New TipoNegocioViewModel
InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "TipoNegocio", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TipoNegocio_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        vmTipoNegocio = CType(Me.DataContext, TipoNegocioViewModel)
        vmTipoNegocio.NombreView = Me.ToString
        If Not IsNothing(vmTipoNegocio) Then
            If Not IsNothing(objVMA2Utils) Then
                vmTipoNegocio.DiccionarioCombosCompletos = objVMA2Utils.DiccionarioCombosEspecificos
            End If

            vmTipoNegocio.Inicializar()
        End If
    End Sub

    Private Sub dgDetalles_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)

        e.Handled = True
        'df.ValidateItem()
    End Sub

    Private Sub Buscador_finalizoBusquedaespeciesBusqueda(ByVal pstrNemotecnico As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            If Not IsNothing(vmTipoNegocio) Then
                If Not IsNothing(vmTipoNegocio.TipoNegocioXEspeciSelected) Then
                    vmTipoNegocio.TipoNegocioXEspeciSelected.IDEspecie = pobjEspecie.Nemotecnico
                End If
            End If
        End If
    End Sub


    Private Sub Buscador_finalizoBusquedaclientesBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            If Not IsNothing(vmTipoNegocio) Then
                If Not IsNothing(vmTipoNegocio.TipoNegocioXTipoProductSelected) Then
                    vmTipoNegocio.TipoNegocioXTipoProductSelected.IDComitente = pobjItem.IdComitente
                    vmTipoNegocio.TipoNegocioXTipoProductSelected.Nombre = pobjItem.Nombre
                End If
            End If
        End If
    End Sub

    Private Sub Buscador_finalizoBusquedaclientesBusquedaDistribucion(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            If Not IsNothing(vmTipoNegocio) Then
                If Not IsNothing(vmTipoNegocio.TipoNegocioXDistribucionSelected) Then
                    vmTipoNegocio.TipoNegocioXDistribucionSelected.CodigoOYD = pobjItem.IdComitente
                    vmTipoNegocio.TipoNegocioXDistribucionSelected.NombreCodigoOYD = pobjItem.Nombre
                End If
            End If
        End If
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(vmTipoNegocio) Then
            vmTipoNegocio.BorrarClienteBuscador()
        End If
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(vmTipoNegocio) Then
            vmTipoNegocio.BorrarEspecieBuscador()
        End If
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub
    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                vmTipoNegocio.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                vmTipoNegocio.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If

    End Sub

End Class


