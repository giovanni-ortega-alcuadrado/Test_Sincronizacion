Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls

Partial Public Class PagosPorRedBancoView
    Inherits UserControl


    ''' <summary>
    ''' Eventos creados para la comunicación con las clases PagosPorRedBancoView y PagosPorRedBancoViewModel
    ''' Pantalla Pagos por red banco
    ''' </summary>
    ''' <remarks>Catalina Davila (IOSoft S.A.) - 7 de Agosto/2016</remarks>
#Region "Variables"

    Private mobjVM As PagosPorRedBancoViewModel
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
            Me.DataContext = New PagosPorRedBancoViewModel
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
            mobjVM = CType(Me.DataContext, PagosPorRedBancoViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.ViewPagosPorRedBanco = Me

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

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem AS OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "CuentasCB"
                        CType(Me.DataContext, PagosPorRedBancoViewModel).strNroCuenta = pobjItem.IdItem
                        CType(Me.DataContext, PagosPorRedBancoViewModel).strNombreCuenta = pobjItem.Nombre
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al finalizar la búsqueda de cuentas.", Me.Name, "BuscadorGenerico_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub btnLimpiarCuentas_Click(sender As Object, e As RoutedEventArgs)
        Try
            CType(Me.DataContext, PagosPorRedBancoViewModel).strNroCuenta = String.Empty
            CType(Me.DataContext, PagosPorRedBancoViewModel).strNombreCuenta = String.Empty
            CType(Me.DataContext, PagosPorRedBancoViewModel).dblCantidad = 0
            CType(Me.DataContext, PagosPorRedBancoViewModel).dblTotal = 0
            CType(Me.DataContext, PagosPorRedBancoViewModel).ListaDetalle = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los datos.", Me.Name, "btnLimpiarCuentas_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub dpFechaProceso_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsDate(CType(Me.DataContext, PagosPorRedBancoViewModel).dtmFechaProceso) Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de proceso es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                CType(Me.DataContext, PagosPorRedBancoViewModel).dtmFechaProceso = Date.Now.ToShortDateString
                Exit Sub
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al perder el foco.", Me.Name, "dpFechaProceso_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Manejadores error"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Aceptar()
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


