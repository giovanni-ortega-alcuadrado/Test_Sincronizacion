Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls

Partial Public Class PortafolioClienteView
    Inherits UserControl
    Dim objViewModel As PortafolioClienteXTipoNegocioViewModel
    Dim objVMA2Utils As A2UtilsViewModel
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)

            objVMA2Utils = New A2UtilsViewModel("PORTAFOLIOCLIENTE", mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objVMA2Utils)

            Me.DataContext = New PortafolioClienteXTipoNegocioViewModel
InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "PortafolioClienteView", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub PortafolioClienteView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
            cm.DF = df
            objViewModel = CType(Me.DataContext, PortafolioClienteXTipoNegocioViewModel)
            objViewModel.NombreView = Me.ToString
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "PortafolioClienteView", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            df.CancelEdit()
            If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro_Click", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        Try
            df.ValidateItem()
            If df.ValidationSummary.HasErrors Then
                df.CancelEdit()
            Else
                df.CommitEdit()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar la grabación", Me.ToString(), "cm_EventoConfirmarGrabacion", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

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
        'df.ValidateItem()
    End Sub

    Private Sub BuscadorClienteListaButon(pstrClaseControl As String, pobjComitente AS OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                If Not IsNothing(objViewModel) Then
                    If Not IsNothing(objViewModel.PortafolioClienteSeleccionado) Then
                        objViewModel.InsertarDatos(pobjComitente)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información del cliente", Me.ToString(), "BuscadorGenerico_itemAsignado_1", Program.TituloSistema, Program.Maquina, ex)
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

    Private Sub BuscadorClienteListaButon_Buscar(pstrClaseControl As String, pobjComitente AS OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                If Not IsNothing(objViewModel) Then
                    If Not IsNothing(objViewModel.cb) Then
                        objViewModel.InsertarDatosBusqueda(pobjComitente)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información del cliente", Me.ToString(), "BuscadorGenerico_itemAsignado_1", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
End Class
