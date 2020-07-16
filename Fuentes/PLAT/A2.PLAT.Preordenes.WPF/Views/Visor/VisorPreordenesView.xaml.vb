Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class VisorPreordenesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases InstrumentosView y InstrumentosViewModel
    ''' Pantalla Configuración de Parámetros (Maestros)
    ''' </summary>
    ''' <remarks>(Alcuadrado S.A.) - 01 de Marzo 2018</remarks>
#Region "Variables"

    Private mobjVM As VisorPreordenesViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    ''' <history>
    ''' </history>
    Public Sub New()
        Try
            InitializeComponent()
            mobjVM = New VisorPreordenesViewModel
            Me.DataContext = mobjVM

            If Not IsNothing(Application.Current.MainWindow) Then
                AddHandler Application.Current.MainWindow.SizeChanged, AddressOf CambioDePantalla
            End If

            Me.Width = Application.Current.MainWindow.ActualWidth * 0.96
            mobjVM.viewPreOrdenes = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Me.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "CambioDePantalla", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False

                inicializar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, VisorPreordenesViewModel)
            mobjVM.NombreView = Me.ToString

            Await mobjVM.inicializar()
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
            ElseIf TypeOf sender Is Telerik.Windows.Controls.RadNumericUpDown Then
                CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Select(0, CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Manejadores error"

    Private Sub dgGrid_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            ' Control de error del bindding del grid
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

    Private Sub ControlNotificacionInconsistencias_EventoVisualizacionErrores(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarListaInconsistencias(mobjVM.ListaInconsistenciasActualizacion, Program.TituloSistema, True)
    End Sub

    Private Sub BtnLimpiarFiltro_Click_1(sender As Object, e As RoutedEventArgs)
        mobjVM.PrepararNuevaBusqueda()
        mobjVM.ConsultarPreOrdenesCruzadas()
    End Sub

    Private Sub BtnFiltrar_Click_1(sender As Object, e As RoutedEventArgs)
        mobjVM.ConsultarPreOrdenesCruzadas()
    End Sub

    Private Sub BtnCruzar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.GenerarCrucesPreordenes()
    End Sub

    Private Sub BtnRefrescarCompras_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.RecargarCompras()
    End Sub

    Private Sub BtnRefrescarVentas_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.RecargarVentas()
    End Sub

    Private Sub BtnConsultar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.RecargarTodo()
    End Sub

    Private Sub Compras_Checked(sender As Object, e As RoutedEventArgs)
        mobjVM.SeleccionarCompra(CInt(CType(sender, CheckBox).Tag))
    End Sub

    Private Sub Compras_UnChecked(sender As Object, e As RoutedEventArgs)
        mobjVM.RecalcularTotales()
    End Sub

    Private Sub Ventas_Checked(sender As Object, e As RoutedEventArgs)
        mobjVM.SeleccionarVenta(CInt(CType(sender, CheckBox).Tag))
    End Sub

    Private Sub Ventas_UnChecked(sender As Object, e As RoutedEventArgs)
        mobjVM.RecalcularTotales()
    End Sub

    Private Sub VerDetalleRegistro_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.VerDetalleRegistro(CInt(CType(sender, Button).Tag))
    End Sub

    Private Sub TxtBusqueda_ID_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                mobjVM.ConsultarPreOrdenesCruzadas()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "TxtBusqueda_ID_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CrearOrden_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.VerificarCreacionOrden(CInt(CType(sender, Button).Tag))
    End Sub
End Class
