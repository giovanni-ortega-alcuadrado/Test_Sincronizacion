Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls

Partial Public Class ClientesRelacionadosView
    Inherits UserControl
    Dim objViewModel As ClientesRelacionadosViewModel

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            'objVMA2Utils = New A2UtilsViewModel("RELACIONCLIENTE", "CLIENTESRELACIONADOS")
            'Me.Resources.Add("A2VM", objVMA2Utils)

            Me.DataContext = New ClientesRelacionadosViewModel
InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "ClientesRelacionadosView", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ClientesRelacionadosView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
            objViewModel = CType(Me.DataContext, ClientesRelacionadosViewModel)
            objViewModel.NombreView = Me.ToString
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "ClientesRelacionadosView_Loaded", Program.TituloSistema, Program.Maquina, ex)
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

    Private Sub BuscadorGenerico_itemAsignado_1(pstrIdItem As String, pobjItem AS OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(objViewModel) Then
                    If Not IsNothing(objViewModel.ClienteSeleccionado) Then
                        objViewModel.InsertarDatos(pobjItem)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información del cliente", Me.ToString(), "BuscadorGenerico_itemAsignado_1", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda_1(pstrClaseControl As String, pobjItem AS OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(objViewModel) Then
                    If Not IsNothing(objViewModel.ClienteRelacionadoSelected) Then
                        objViewModel.InsertarDatosNuevoRegistro(pobjItem)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información del cliente", Me.ToString(), "BuscadorGenerico_itemAsignado_1", Program.TituloSistema, Program.Maquina, ex)
        End Try
        
    End Sub
End Class
