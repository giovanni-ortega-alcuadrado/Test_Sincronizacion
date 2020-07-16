Imports A2.OyD.OYDServer.RIA.WEB
Imports Telerik.Windows.Controls

Partial Public Class OrdenesDerivadosFormView
    Inherits Window

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases OrdenesFormView y OrdenesFormView
    ''' Pantalla Configuración de Parámetros (Maestros)
    ''' </summary>
    ''' <remarks>(Alcuadrado S.A.) - 17 de Abril 2018</remarks>
#Region "Variables"

    Private mobjVM As OrdenesDerivadosViewModel
    Private mlogInicializar As Boolean = True
    Private mobjPaginador As RadDataPager = Nothing


    Private strTipoProducto As String = ""
    Private strTipoOpcionProducto As String = ""

#End Region

#Region "Inicializacion"
    ''' <history>
    ''' </history>
    Public Sub New(ByVal pobjViewModel As OrdenesDerivadosViewModel)
        InitializeComponent()
        mobjVM = pobjViewModel
        Me.DataContext = mobjVM
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridViewRegistros = mobjVM.viewListaPrincipal.datapager1
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            ElseIf TypeOf sender Is Telerik.Windows.Controls.RadNumericUpDown Then
                CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Select(0, CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As RoutedEventArgs)
        If Await mobjVM.ActualizarRegistroDetalle() Then
            mobjVM.Editando = False
            Me.DialogResult = True
        End If
    End Sub

    Private Async Sub btnGuardarContinuar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.logCambiarEncabezadoSeleccionado = False
        If Await mobjVM.ActualizarRegistroDetalle() Then
            mobjVM.CrearNuevoDetalle(True)
        End If
        mobjVM.logCambiarEncabezadoSeleccionado = True
    End Sub

    Private Async Sub btnGuardarContinuarNuevo_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.logCambiarEncabezadoSeleccionado = False
        If Await mobjVM.ActualizarRegistroDetalle() Then
            mobjVM.CrearNuevoDetalle()
        End If
        mobjVM.logCambiarEncabezadoSeleccionado = True
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Editando = False
        mobjVM.CancelarRegistroDetalle()
    End Sub

    Private Sub btnNuevoRegistro_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.NuevoRegistroDetalle()
    End Sub

    Private Sub btnEditarRegistro_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.EditarRegistroDetalle()
    End Sub

    Private Sub btnBorrarRegistro_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarRegistroDetalle()
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

    Private Sub TipoOpcionProducto_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try

            Dim ojbComboBox As ComboBox = CType(sender, ComboBox)

            Dim IntTipoOpcionProducto As String = ojbComboBox.SelectedValue

            If Not IsNothing(IntTipoOpcionProducto) Then
                strTipoOpcionProducto = mobjVM.DiccionarioCombosPantalla("TIPOOPCIONPRODUCTO").Where(Function(i) i.ID = IntTipoOpcionProducto).First.Descripcion
            Else
                strTipoOpcionProducto = ""
            End If


            If strTipoProducto.ToUpper = "FUTURO" And strTipoOpcionProducto.ToUpper = "TIME SPREAD" Then
                mobjVM.Habilitar_VencimientoFinal = True
                mobjVM.MostrarCamposSpread = Visibility.Visible
                mobjVM.MostrarCamposPrecio = Visibility.Collapsed
            Else
                mobjVM.Habilitar_VencimientoFinal = False
                mobjVM.MostrarCamposSpread = Visibility.Collapsed
                mobjVM.MostrarCamposPrecio = Visibility.Visible
                If Not IsNothing(mobjVM.EncabezadoEdicionSeleccionado) Then
                    mobjVM.EncabezadoEdicionSeleccionado.intIdProductoEspecial = -1
                End If
            End If

            If strTipoProducto.ToUpper = "OPCION" And (strTipoOpcionProducto.ToUpper = "CALL" Or strTipoOpcionProducto.ToUpper = "PUT") Then
                mobjVM.Habilitar_TipoPrima = True
                If Not IsNothing(mobjVM.EncabezadoEdicionSeleccionado) Then
                    mobjVM.EncabezadoEdicionSeleccionado.intIdTipoPrima = mobjVM.DiccionarioCombosPantalla("TIPOPRIMA").Where(Function(i) i.Descripcion.ToUpper = "PRECIO").First.ID
                End If
            Else
                mobjVM.Habilitar_TipoPrima = False
                mobjVM.Habilitar_PrecioPrima = False
                mobjVM.Habilitar_Prima = False
                If Not IsNothing(mobjVM.EncabezadoEdicionSeleccionado) Then
                    mobjVM.EncabezadoEdicionSeleccionado.intIdTipoPrima = -1
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "TipoOpcionProducto_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TipoProducto_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try

            Dim ojbComboBox As ComboBox = CType(sender, ComboBox)

            Dim IntIdTipoProducto As String = ojbComboBox.SelectedValue

            If Not IsNothing(IntIdTipoProducto) Then
                strTipoProducto = mobjVM.DiccionarioCombosPantalla("TIPOPRODUCTO").Where(Function(i) i.ID = IntIdTipoProducto).First.Descripcion
            Else
                strTipoProducto = ""
            End If

            If strTipoProducto.ToUpper = "FUTURO" And strTipoOpcionProducto.ToUpper = "TIME SPREAD" Then
                mobjVM.Habilitar_VencimientoFinal = True
                mobjVM.MostrarCamposSpread = Visibility.Visible
                mobjVM.MostrarCamposPrecio = Visibility.Collapsed
            Else
                mobjVM.Habilitar_VencimientoFinal = False
                mobjVM.MostrarCamposSpread = Visibility.Collapsed
                mobjVM.MostrarCamposPrecio = Visibility.Visible
                If Not IsNothing(mobjVM.EncabezadoEdicionSeleccionado) Then
                    mobjVM.EncabezadoEdicionSeleccionado.intIdProductoEspecial = -1
                End If
            End If

            If strTipoProducto.ToUpper = "OPCION" And (strTipoOpcionProducto.ToUpper = "CALL" Or strTipoOpcionProducto.ToUpper = "PUT") Then
                mobjVM.Habilitar_TipoPrima = True
                If Not IsNothing(mobjVM.EncabezadoEdicionSeleccionado) Then
                    mobjVM.EncabezadoEdicionSeleccionado.intIdTipoPrima = mobjVM.DiccionarioCombosPantalla("TIPOPRIMA").Where(Function(i) i.Descripcion.ToUpper = "PRECIO").First.ID
                End If
            Else
                mobjVM.Habilitar_TipoPrima = False
                mobjVM.Habilitar_PrecioPrima = False
                mobjVM.Habilitar_Prima = False
                If Not IsNothing(mobjVM.EncabezadoEdicionSeleccionado) Then
                    mobjVM.EncabezadoEdicionSeleccionado.intIdTipoPrima = -1
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "TipoProducto_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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


    Private Sub ControlNotificacionInconsistencias_EventoVisualizacionErrores(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarListaInconsistencias(mobjVM.ListaInconsistenciasActualizacion, Program.TituloSistema, True)
    End Sub



#End Region

End Class
