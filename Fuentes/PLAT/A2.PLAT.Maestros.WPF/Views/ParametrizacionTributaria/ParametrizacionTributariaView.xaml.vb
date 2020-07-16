Imports A2.OyD.OYDServer.RIA.WEB


Partial Public Class ParametrizacionTributariaView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases InstrumentosView y InstrumentosViewModel
    ''' Pantalla Configuración de Parámetros (Maestros)
    ''' </summary>
    ''' <remarks>(Alcuadrado S.A.) - 01 de Marzo 2018</remarks>
#Region "Variables"

    Private mobjVM As ParametrizacionTributariaViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    ''' <history>
    ''' </history>
    Public Sub New()
        Try
            InitializeComponent()
            mobjVM = New ParametrizacionTributariaViewModel
            Me.DataContext = mobjVM
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
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
            mobjVM = CType(Me.DataContext, ParametrizacionTributariaViewModel)
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

    Private Sub BuscadorGenerico_FinalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "paisbusqueda"
                        If Not IsNothing(mobjVM) Then
                            mobjVM.cb.intIDPais = CInt(pobjItem.IdItem)
                            mobjVM.cb.strDescripcionPais = String.Format("{0} - {1}", pobjItem.IdItem, pobjItem.Nombre)
                            mobjVM.cb.intIDCiudad = Nothing
                            mobjVM.cb.strDescripcionCiudad = String.Empty
                        End If
                    Case "ciudadbusqueda"
                        If Not IsNothing(mobjVM) Then
                            mobjVM.cb.intIDCiudad = CInt(pobjItem.IdItem)
                            mobjVM.cb.strDescripcionCiudad = String.Format("{0} - {1}", pobjItem.IdItem, pobjItem.Nombre)
                        End If
                    Case "pais"
                        If Not IsNothing(mobjVM) Then
                            mobjVM.EncabezadoEdicionSeleccionado.intIDPais = CInt(pobjItem.IdItem)
                            mobjVM.strDescripcionPais = String.Format("{0} - {1}", pobjItem.IdItem, pobjItem.Nombre)
                            mobjVM.EncabezadoEdicionSeleccionado.intIDCiudad = Nothing
                            mobjVM.strDescripcionCiudad = String.Empty
                        End If
                    Case "ciudad"
                        If Not IsNothing(mobjVM) Then
                            mobjVM.EncabezadoEdicionSeleccionado.intIDCiudad = CInt(pobjItem.IdItem)
                            mobjVM.strDescripcionCiudad = String.Format("{0} - {1}", pobjItem.IdItem, pobjItem.Nombre)
                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BuscadorGenerico_FinalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ControlNotificacionInconsistencias_EventoVisualizacionErrores(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarListaInconsistencias(mobjVM.ListaInconsistenciasActualizacion, Program.TituloSistema, True)
    End Sub

    Private Sub btnLimpiarPaisBusqueda_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                mobjVM.cb.intIDPais = Nothing
                mobjVM.cb.strDescripcionPais = String.Empty
                mobjVM.cb.intIDCiudad = Nothing
                mobjVM.cb.strDescripcionCiudad = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnLimpiarPaisBusqueda_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCiudadBusqueda_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                mobjVM.cb.intIDCiudad = Nothing
                mobjVM.cb.strDescripcionCiudad = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnLimpiarCiudadBusqueda_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarPais_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                mobjVM.EncabezadoEdicionSeleccionado.intIDPais = Nothing
                mobjVM.strDescripcionPais = String.Empty
                mobjVM.EncabezadoEdicionSeleccionado.intIDCiudad = Nothing
                mobjVM.strDescripcionCiudad = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnLimpiarPais_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCiudad_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                mobjVM.EncabezadoEdicionSeleccionado.intIDCiudad = Nothing
                mobjVM.strDescripcionCiudad = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnLimpiarCiudad_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoCiudad_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                If IsNothing(mobjVM.EncabezadoEdicionSeleccionado.intIDPais) Then
                    CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Condicion1 = String.Empty
                Else
                    CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Condicion1 = mobjVM.EncabezadoEdicionSeleccionado.intIDPais
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BuscadorGenericoCiudad_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoBusquedaCiudad_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                If IsNothing(mobjVM.cb.intIDPais) Then
                    CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Condicion1 = String.Empty
                Else
                    CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Condicion1 = mobjVM.cb.intIDPais
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "BuscadorGenericoBusquedaCiudad_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                mobjVM.DuplicarRegistro()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "btnDuplicar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
