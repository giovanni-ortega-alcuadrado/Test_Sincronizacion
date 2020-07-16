Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: TipoProductoXEspecieView.xaml.vb
'Generado el : 12/10/2012 09:15:14
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class TipoProductoXEspecieView
    Inherits UserControl
    Dim vmTipoProducto As TipoProductoXEspecieViewModel

    Public Sub New()

        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Me.DataContext = New TipoProductoXEspecieViewModel
InitializeComponent()

    End Sub

    Private Sub TipoProductoXEspecie_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        vmTipoProducto = CType(Me.DataContext, TipoProductoXEspecieViewModel)
        CType(Me.DataContext, TipoProductoXEspecieViewModel).NombreView = Me.ToString

    End Sub

    Private Sub Buscador_finalizoBusquedanemotecnicoBusqueda(ByVal pstrNemotecnico As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            If Not IsNothing(vmTipoProducto) Then
                If Not IsNothing(vmTipoProducto.TipoProductoXEspeciSelected) Then
                    vmTipoProducto.TipoProductoXEspeciSelected.IDEspecie = pobjEspecie.Nemotecnico
                End If
            End If
        End If
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(vmTipoProducto) Then
            vmTipoProducto.BorrarEspecieBuscador()
        End If
    End Sub

    Private Sub dgDetalles_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)

        e.Handled = True
        'df.ValidateItem()
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub
    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                vmTipoProducto.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                vmTipoProducto.IsBusy = False
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


