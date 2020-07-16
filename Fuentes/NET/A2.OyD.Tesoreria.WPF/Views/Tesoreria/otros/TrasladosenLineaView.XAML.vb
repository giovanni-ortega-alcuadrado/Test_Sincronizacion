Imports Telerik.Windows.Controls

Partial Public Class TrasladosenLineaView
    Inherits Window

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases TrasladosenLineaView y GenerarRecibosRecaudosViewModel
    ''' </summary>
    ''' <remarks>Jeison Ramírez Pino.(IOSoft) - 01 de Julio 2016</remarks>
#Region "Variables"
    Private mobjVM As GenerarRecibosRecaudoViewModel
    Private mlogInicializar As Boolean = True
#End Region

#Region "Inicializacion"
    Public Sub New(ByVal pobjVM As GenerarRecibosRecaudoViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            mobjVM = pobjVM

            If Me.Resources.Contains("VM") Then
                Me.Resources.Remove("VM")
            End If

            Me.Resources.Add("VM", mobjVM)
            Me.DataContext = mobjVM

            InitializeComponent()

            mobjVM.ConsultarTraslados()
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try
    End Sub

#End Region

#Region "Eventos de controles"

    Private Sub seleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            ' Seleccionar el texto del control en el cual el usuario se ubicó
            MyBase.OnGotFocus(e)

            If TypeOf sender Is TextBox Then
                CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.EliminarTraslado()
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.AceptarTraslado()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
    
#End Region

#Region "Manejadores error"
    Private Sub dgGrid_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema para presentar los datos del detalle.", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga de los datos", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region

   
End Class