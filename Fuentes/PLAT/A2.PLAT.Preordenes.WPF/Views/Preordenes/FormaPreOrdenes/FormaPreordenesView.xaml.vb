Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class FormaPreordenesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases InstrumentosView y InstrumentosViewModel
    ''' Pantalla Configuración de Parámetros (Maestros)
    ''' </summary>
    ''' <remarks>(Alcuadrado S.A.) - 01 de Marzo 2018</remarks>
#Region "Variables"

    Private mobjVM As FormaPreordenesViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    ''' <history>
    ''' </history>
    Public Sub New()
        Try
            InitializeComponent()
            mobjVM = New FormaPreordenesViewModel
            Me.DataContext = mobjVM
            mobjVM.viewPreOrdenes = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            mobjVM = CType(Me.DataContext, FormaPreordenesViewModel)
            mobjVM.NombreView = "A2PLATPreordenes.PreordenesView"

            Await mobjVM.inicializar()
            If intIDRegistro > 0 Then
                intIDPreOrden = intIDRegistro
            End If
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
    Private Sub ControlNotificacionInconsistencias_EventoVisualizacionErrores(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarListaInconsistencias(mobjVM.ListaInconsistenciasActualizacion, Program.TituloSistema, True)
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.Encabezado_CancelarEditarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.Encabezado_ActualizarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

	Private Sub BtnNuevo_Click(sender As Object, e As RoutedEventArgs)
		Try
			mobjVM.Encabezado_NuevoRegistro()
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Async Sub CtlBuscadorCliente_personaAsignada(pstrIdComitente As String, pobjComitente As CPX_BuscadorPersonas)
        Try
            If Not IsNothing(pobjComitente) Then
                mobjVM.intIDPersona = pobjComitente.intID
                mobjVM.strIDComitente = pobjComitente.strCodigoOyD
                mobjVM.strNroDocumento = pobjComitente.strNroDocumento
                mobjVM.strNombre = pobjComitente.strNombre

                Await mobjVM.Consultar_Cliente_Portafolio()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub CtlrEspecies_especieAsignada(pstrNemotecnico As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                mobjVM.EncabezadoEdicionSeleccionado.strInstrumento = pstrNemotecnico
                mobjVM.strNombreEspecie = pobjEspecie.Especie

                Await mobjVM.Consultar_EntidadInstrumento()
                Await mobjVM.Consultar_Cliente_Portafolio()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CtlrGenericoEntidad_itemAsignado(pstrIdItem As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                mobjVM.EncabezadoEdicionSeleccionado.intIDEntidad = CInt(pobjItem.IdItem)
                mobjVM.strNroDocumentoEntidad = pobjItem.CodItem
                mobjVM.strNombreEntidad = pobjItem.Nombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub PortafolioCliente_Checked(sender As Object, e As RoutedEventArgs)
        Try
            Dim intItemSeleccionado As Integer = CInt(CType(sender, CheckBox).Tag)
            If mobjVM.ListaCliente_Portafolio.Where(Function(i) i.intID = intItemSeleccionado).Count > 0 Then
                For Each li In mobjVM.ListaCliente_Portafolio
                    If li.intID <> intItemSeleccionado Then
                        li.logSeleccionado = False
                    End If
                Next

                mobjVM.logConsultarPortafolioCliente = False
                Dim objItemSeleccionado = mobjVM.ListaCliente_Portafolio.Where(Function(i) i.intID = intItemSeleccionado).First
                mobjVM.EncabezadoEdicionSeleccionado.strInstrumento = objItemSeleccionado.strInstrumento
                mobjVM.strNombreEspecie = objItemSeleccionado.strDescripcionInstrumento
                mobjVM.EncabezadoEdicionSeleccionado.intIDEntidad = objItemSeleccionado.intIDEntidad
                mobjVM.strNroDocumentoEntidad = objItemSeleccionado.strNroDocumentoEntidad
                mobjVM.strNombreEntidad = objItemSeleccionado.strNombreEntidad
                mobjVM.EncabezadoEdicionSeleccionado.dblValor = objItemSeleccionado.dblValorNominal

                If IsNothing(mobjVM.PreOrdenes_Portafolio) Then
                    mobjVM.PreOrdenes_Portafolio = objItemSeleccionado
                End If

                mobjVM.logConsultarPortafolioCliente = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CtlrEspecies_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            If mobjVM.EncabezadoEdicionSeleccionado.strTipoInversion = "A" Then
                ctlrEspecies.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
            ElseIf mobjVM.EncabezadoEdicionSeleccionado.strTipoInversion = "C" Then
                ctlrEspecies.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
            Else
                ctlrEspecies.ClaseOrden = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnFiltrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub BtnLimpiarClienteEdicion_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.EncabezadoEdicionSeleccionado.intIDCodigoPersona = Nothing
            mobjVM.strIDComitente = String.Empty
            mobjVM.strNroDocumento = String.Empty
            mobjVM.strNombre = String.Empty

            ctlBuscadorCliente.BorrarPersonaSeleccionada()
            Await mobjVM.Consultar_Cliente_Portafolio()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnLimpiarClienteEdicion_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub BtnLimpiarEspecie_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.EncabezadoEdicionSeleccionado.strInstrumento = String.Empty
            mobjVM.strNombreEspecie = String.Empty

            If mobjVM.BorrarEspecie Then
                mobjVM.BorrarEspecie = False
            End If
            mobjVM.BorrarEspecie = True
            Await mobjVM.Consultar_Cliente_Portafolio()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BtnLimpiarEntidad_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.EncabezadoEdicionSeleccionado.intIDEntidad = Nothing
            mobjVM.strNroDocumentoEntidad = String.Empty
            mobjVM.strNombreEntidad = String.Empty

            If mobjVM.BorrarEntidad Then
                mobjVM.BorrarEntidad = False
            End If
            mobjVM.BorrarEntidad = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BtnLimpiarEntidad_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

#Region "Propiedades"

    Private Shared ReadOnly intIDRegistroDep As DependencyProperty = DependencyProperty.Register("intIDRegistro", GetType(Integer), GetType(FormaPreordenesView), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf intIDRegistro_Changed)))
    Public Property intIDRegistro As Integer
        Get
            Return CInt(GetValue(intIDRegistroDep))
        End Get
        Set(ByVal value As Integer)
            SetValue(intIDRegistroDep, value)
        End Set
    End Property
    Private Shared Sub intIDRegistro_Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As FormaPreordenesView = DirectCast(d, FormaPreordenesView)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "IDReceptorChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Shared ReadOnly intIDPreOrdenDep As DependencyProperty = DependencyProperty.Register("intIDPreOrden", GetType(Integer), GetType(FormaPreordenesView), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf intIDPreOrden_Changed)))
    Public Property intIDPreOrden As Integer
        Get
            Return CInt(GetValue(intIDPreOrdenDep))
        End Get
        Set(ByVal value As Integer)
            SetValue(intIDPreOrdenDep, value)
        End Set
    End Property
    Private Shared Sub intIDPreOrden_Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As FormaPreordenesView = DirectCast(d, FormaPreordenesView)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.intIDPreOrden = obj.intIDPreOrden
                obj.mobjVM.ConsultarEncabezadoEdicion()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "IDReceptorChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Shared ReadOnly logMostrarBotonesDep As DependencyProperty = DependencyProperty.Register("logMostrarBotones", GetType(Boolean), GetType(FormaPreordenesView), New PropertyMetadata(True, New PropertyChangedCallback(AddressOf logMostrarBotones_Changed)))
    Public Property logMostrarBotones As Boolean
        Get
            Return CBool(GetValue(logMostrarBotonesDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(logMostrarBotonesDep, value)
        End Set
    End Property
    Private Shared Sub logMostrarBotones_Changed(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As FormaPreordenesView = DirectCast(d, FormaPreordenesView)
            If Not IsNothing(obj.mobjVM) Then
                If obj.logMostrarBotones Then
                    obj.mobjVM.MostrarBotones = Visibility.Visible
                Else
                    obj.mobjVM.MostrarBotones = Visibility.Collapsed
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "IDReceptorChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
#End Region

#Region "Eventos"
    Public Event TerminoGuardarRegistro(ByVal pintIDRegistroGuardado As Integer, ByVal pobjItemGuardado As tblPreOrdenes)

	Public Sub EjecutarEventoGuardarRegistro(ByVal pintIDRegistroGuardado As Integer, ByVal pobjItemGuardado As tblPreOrdenes)
		Try
			RaiseEvent TerminoGuardarRegistro(pintIDRegistroGuardado, pobjItemGuardado)
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorCliente", "IDReceptorChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub


#End Region

End Class
