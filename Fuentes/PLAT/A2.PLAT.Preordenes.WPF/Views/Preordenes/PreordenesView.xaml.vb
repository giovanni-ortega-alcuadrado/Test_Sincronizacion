Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class PreordenesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases InstrumentosView y InstrumentosViewModel
    ''' Pantalla Configuración de Parámetros (Maestros)
    ''' </summary>
    ''' <remarks>(Alcuadrado S.A.) - 01 de Marzo 2018</remarks>
#Region "Variables"

    Private mobjVM As PreordenesViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    ''' <history>
    ''' </history>
    Public Sub New()
        Try
            InitializeComponent()
            mobjVM = New PreordenesViewModel
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
            mobjVM = CType(Me.DataContext, PreordenesViewModel)
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

    Private Sub BtnLimpiarCliente_Click(sender As Object, e As RoutedEventArgs)
        Try
            ctlBuscadorClienteBusqueda.BorrarPersonaSeleccionada()

            mobjVM.cb.intIDCodigoPersona = Nothing
            mobjVM.cb.strIDComitente = String.Empty
            mobjVM.cb.strNroDocumento = String.Empty
            mobjVM.cb.strNombre = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BtnConsultar_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.Encabezado_Buscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnConsultar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BtnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.PrepararNuevaBusqueda()
            mobjVM.Encabezado_QuitarFiltro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnLimpiar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TxtFiltrar_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                mobjVM.FiltroUsuario = CType(sender, A2TextBox).Text
                mobjVM.Encabezado_Filtrar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "TxtFiltrar_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BtnLimpiarFiltro_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.Encabezado_QuitarFiltro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnLimpiarFiltro_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TxtBusqueda_ID_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                mobjVM.cb.intID = CInt(CType(sender, A2TextBox).Text)
                mobjVM.Encabezado_Buscar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "TxtBusqueda_ID_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BtnFiltrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.Encabezado_Filtrar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Busqueda_BuscadorCliente_personaAsignada(pstrIdComitente As String, pobjComitente As CPX_BuscadorPersonas)
        Try
            If Not IsNothing(pobjComitente) Then
                mobjVM.cb.intIDCodigoPersona = pobjComitente.intID
                mobjVM.cb.strIDComitente = pobjComitente.strCodigoOyD
                mobjVM.cb.strNroDocumento = pobjComitente.strNroDocumento
                mobjVM.cb.strNombre = pobjComitente.strNombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

	Private Sub FormaPreordenesView_TerminoGuardarRegistro(pintIDRegistroGuardado As Integer, pobjItemGuardado As tblPreOrdenes)
		Try
			mobjVM.Encabezado_ActualizarRegistro(pintIDRegistroGuardado)
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub btnEliminar_Click(sender As Object, e As RoutedEventArgs)
		Try
			Dim intRegistro As Integer = CInt(CType(sender, Button).Tag)

			If Not IsNothing(mobjVM.ListaEncabezado) Then
				If mobjVM.ListaEncabezado.Where(Function(i) i.intID = intRegistro).Count > 0 Then
					mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.intID = intRegistro).First
				End If
			End If

			mobjVM.Encabezado_BorrarRegistro()
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnEliminar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub
End Class
