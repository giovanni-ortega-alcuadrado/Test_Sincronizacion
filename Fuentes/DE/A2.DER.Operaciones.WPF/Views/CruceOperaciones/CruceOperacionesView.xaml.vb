Imports A2.OyD.OYDServer.RIA.WEB


Partial Public Class CruceOperacionesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases InstrumentosView y InstrumentosViewModel
    ''' Pantalla Configuración de Parámetros (Maestros)
    ''' </summary>
    ''' <remarks>(Alcuadrado S.A.) - 01 de Marzo 2018</remarks>
#Region "Variables"

    Private mobjVM As CruceOperacionesViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    ''' <history>
    ''' </history>
    Public Sub New()
        InitializeComponent()
        mobjVM = New CruceOperacionesViewModel
        Me.DataContext = mobjVM
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridTransicion = grdGridForma
                cm.GridViewRegistros = datapager1

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, CruceOperacionesViewModel)
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

    Private Async Sub btnCruceAutomatico_Click(sender As Object, e As RoutedEventArgs)
        Await mobjVM.CruceAutomatico()
    End Sub

    Private Sub btnCruceManual_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.CruceOperacionesManual()
        mobjVM.ConsultarLiquidacionesTimeSpread(mobjVM.EncabezadoSeleccionado.intID, mobjVM.EncabezadoSeleccionado.strTradeReference)
        mobjVM.MostrarBotonesCruceManual = Visibility.Visible
        mobjVM.MostrarBotonesBusqueda = Visibility.Collapsed
        mobjVM.MostrarBotonesEncabezado = Visibility.Collapsed
    End Sub

    Private Sub btnDescalzar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Descalzar()
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarRegistro()
    End Sub

    Private Sub SeleccionarUno_Checked(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.MarcarDesmarcarUno()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "SeleccionarUno_Checked", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnBusquedaAvanzada_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.Buscar()
            mobjVM.MostrarBotonesCruceManual = Visibility.Collapsed
            mobjVM.MostrarBotonesBusqueda = Visibility.Visible
            mobjVM.MostrarBotonesEncabezado = Visibility.Collapsed
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnBusquedaAvanzada_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.ConfirmarBuscar()

            mobjVM.MostrarBotonesCruceManual = Visibility.Collapsed
            mobjVM.MostrarBotonesBusqueda = Visibility.Collapsed
            mobjVM.MostrarBotonesEncabezado = Visibility.Visible

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnBuscar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnCancelarBusqueda_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.CancelarBuscar()
            mobjVM.Editando = False
            mobjVM.CambiarALista()
            mobjVM.EncabezadoSeleccionado = Nothing

            mobjVM.ConsultarEncabezado("FILTRAR", String.Empty)

            mobjVM.MostrarBotonesCruceManual = Visibility.Collapsed
            mobjVM.MostrarBotonesBusqueda = Visibility.Collapsed
            mobjVM.MostrarBotonesEncabezado = Visibility.Visible

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnCancelarBusqueda_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnGuardarCruceManual_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.GuardarCruceOperaconesManuales()

            ' mobjVM.Editando = False
            'mobjVM.CambiarALista()


            'mobjVM.MostrarBotonesCruceManual = Visibility.Collapsed
            'mobjVM.MostrarBotonesBusqueda = Visibility.Collapsed
            'mobjVM.MostrarBotonesEncabezado = Visibility.Visible

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnGuardarCruceManual_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnCancelarCruceManual_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.CancelarEditarRegistro()
            mobjVM.Editando = False
            mobjVM.CambiarALista()
            mobjVM.EncabezadoSeleccionado = Nothing

            mobjVM.ConsultarEncabezado("FILTRAR", String.Empty)

            mobjVM.MostrarBotonesCruceManual = Visibility.Collapsed
            mobjVM.MostrarBotonesBusqueda = Visibility.Collapsed
            mobjVM.MostrarBotonesEncabezado = Visibility.Visible

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnCancelarBusqueda_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub NavegarAForma(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM.EncabezadoSeleccionado) Then
            If mobjVM.EncabezadoSeleccionado.intID.ToString <> CType(sender, Button).Tag Then
                mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.intID.ToString = CType(sender, Button).Tag).First
            End If
        Else
            mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.intID.ToString = CType(sender, Button).Tag).First
        End If

        mobjVM.CambiarAForma()
    End Sub

    Private Sub ControlNotificacionInconsistencias_EventoVisualizacionErrores(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarListaInconsistencias(mobjVM.ListaInconsistenciasActualizacion, Program.TituloSistema, True)
    End Sub

    Private Sub SoloNumeros_KeyDown(sender As Object, e As KeyEventArgs)
        Try

            If e.Key >= Key.D0 AndAlso e.Key <= Key.D9 OrElse e.Key >= Key.NumPad0 AndAlso e.Key <= Key.NumPad9 Then
                e.Handled = False
            Else
                e.Handled = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "TipoSubyacente_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


End Class
