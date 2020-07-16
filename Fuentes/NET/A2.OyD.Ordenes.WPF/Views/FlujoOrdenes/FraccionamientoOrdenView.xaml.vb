Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl

Partial Public Class FraccionamientoOrdenView
    Inherits Window
    Dim objVMOrdenes As FlujoOrdenesViewModel


    Public Sub New(ByVal objViewModel As FlujoOrdenesViewModel)
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.Resources.Add("VMOrdenes", objViewModel)
            Me.DataContext = objViewModel

            objVMOrdenes = objViewModel

            InitializeComponent()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "FraccionamientoOrdenView", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub FraccionamientoOrdenView_Load(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            objVMOrdenes.viewFraccionamiento = Me
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de fraccionamiento", Me.Name, "", "FraccionamientoOrdenView_Load", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente AS OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjComitente) Then
                    If Not IsNothing(objVMOrdenes.FraccionamientoOrdenSelected) Then
                        objVMOrdenes.FraccionamientoOrdenSelected.IDComitente = pobjComitente.IdComitente
                        objVMOrdenes.FraccionamientoOrdenSelected.NroDocumento = pobjComitente.NroDocumento
                        objVMOrdenes.FraccionamientoOrdenSelected.NombreCliente = pobjComitente.NombreCodigoOYD
                        objVMOrdenes.FraccionamientoOrdenSelected.HabilitarDocumento = False
                    Else
                        objVMOrdenes.FraccionamientoOrdenSelected.HabilitarDocumento = True
                    End If
                Else
                    objVMOrdenes.FraccionamientoOrdenSelected.HabilitarDocumento = True
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorClienteListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub btnGuardar_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
               
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Private Sub txtNroDocumento_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.LetrasNumerosUnicamente)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.", _
                                 Me.ToString(), "txtConcepto_TextChanged_1", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtNombre_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.Letras)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.", _
                                 Me.ToString(), "txtConcepto_TextChanged_1", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    
    Private Sub btnNuevo_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objVMOrdenes) Then
            objVMOrdenes.NuevoRegistrFraccionamiento()
        End If
    End Sub

    Private Sub btnBorrar_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objVMOrdenes) Then
            objVMOrdenes.BorrarRegistrFraccionamiento()
        End If
    End Sub

    Private Sub btnGrabar_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objVMOrdenes) Then
            objVMOrdenes.ActualizarFraccionamiento()
        End If
    End Sub

    Private Sub btnCancelar_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
