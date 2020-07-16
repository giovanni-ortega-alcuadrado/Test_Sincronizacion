Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls

Partial Public Class CupoReceptorView
    Inherits UserControl
    Dim objViewModel As CupoReceptorXTipoNegocioViewModel
    Dim objVMA2Utils As A2UtilsViewModel
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)

            objVMA2Utils = New A2UtilsViewModel("CUPORECEPTOR", mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objVMA2Utils)

            Me.DataContext = New CupoReceptorXTipoNegocioViewModel
InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "PortafolioClienteView", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub PortafolioClienteView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
            objViewModel = CType(Me.DataContext, CupoReceptorXTipoNegocioViewModel)
            objViewModel.NombreView = Me.ToString
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "PortafolioClienteView", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtValor_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 Then
            e.Handled = True
        End If
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If

    End Sub

    Private Sub dgDetalles_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        e.Handled = True
    End Sub

    Private Sub BuscadorItemListaButon(ByVal pstrClaseControl As System.String, ByVal pobjItem AS OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(objViewModel) Then
                    If Not IsNothing(objViewModel.CupoReceptorSeleccionado) Then
                        objViewModel.InsertarDatos(pobjItem)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información del receptor", Me.ToString(), "BuscadorItemListaButon", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                objViewModel.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                objViewModel.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BuscadorItemListaButon_Buscar(ByVal pstrClaseControl As System.String, ByVal pobjItem AS OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(objViewModel) Then
                    If Not IsNothing(objViewModel.cb) Then
                        objViewModel.InsertarDatosBusqueda(pobjItem)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información del receptor", Me.ToString(), "BuscadorItemListaButon_Buscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
End Class
