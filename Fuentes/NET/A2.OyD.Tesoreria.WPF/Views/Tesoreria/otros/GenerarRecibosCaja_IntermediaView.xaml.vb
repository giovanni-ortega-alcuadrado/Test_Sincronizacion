Imports Telerik.Windows.Controls



Partial Public Class GenerarRecibosCaja_IntermediaView
    Inherits UserControl


    ''' <summary>
    ''' Eventos creados para la comunicación con las clases GenerarRecibosCaja_IntermediaView y GenerarRecibosCaja_IntermediaViewModel
    ''' Pantalla Procesar Portafolio (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 21 de Febrero 2014</remarks>
#Region "Variables"

    Private mobjVM As GenerarRecibosCaja_IntermediaViewModel
    Private mlogInicializar As Boolean = True ' CCM20130827 - Cristian Ciceri Muñetón - Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.DataContext = New GenerarRecibosCaja_IntermediaViewModel
InitializeComponent()
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, GenerarRecibosCaja_IntermediaViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.ViewGenerarRecibosCaja_Intermedia = Me

            'Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

            mobjVM.inicializar()
        End If
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

    Private Sub dgListaRC_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            'CType(sender, DataGrid).GetBindingExpression(DataGrid.SelectedItemProperty).UpdateSource()

            If Not IsNothing(mobjVM.DetalleSeleccionado) Then
                If Not mobjVM.DetalleSeleccionado.logGenerar Then
                    If mobjVM.DetalleSeleccionado.strEnTesoreria.ToUpper.Contains("SI") Then

                        mobjVM.DetalleSeleccionado.logGenerar = False

                        A2Utilidades.Mensajes.mostrarMensaje("La Orden nro " + mobjVM.DetalleSeleccionado.numero_orden + " se encuentra en tesoreria", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar un registro.", Me.Name, "dgListaRC_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Manejadores error"

    Private Sub btnConsultarDocumentos_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ConsultarDocumentos()
    End Sub

    Private Sub GenerarRC_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.GenerarRC()
    End Sub

    Private Sub dgGrid_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            ' Control de error del bindding del grid
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


