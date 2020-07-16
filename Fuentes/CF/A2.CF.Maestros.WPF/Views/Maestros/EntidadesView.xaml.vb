Imports Telerik.Windows.Controls

Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web



Partial Public Class EntidadesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases EntidadesView y EntidadesViewModel
    ''' Pantalla Entidades (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 21 de Febrero 2014</remarks>
#Region "Variables"

    Private mobjVM As EntidadesViewModel
    Private mlogInicializar As Boolean = True ' CCM20130827 - Cristian Ciceri Muñetón - Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Me.DataContext = New EntidadesViewModel
InitializeComponent()
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, EntidadesViewModel)
            mobjVM.NombreView = Me.ToString

            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)

            Await mobjVM.inicializar()
        End If
    End Sub

#End Region

#Region "Eventos de controles"
    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                mobjVM.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                mobjVM.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
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
#End Region

#Region "Manejadores error"

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

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "ciudades"
                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.lngIDPoblacion = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.strDescripcionPoblacion = pobjItem.Nombre
                    'CType(Me.DataContext, EntidadesViewModel).CiudadesClaseSelected.Ciudad = pobjItem.Nombre

                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.lngIDDepartamento = CType(pobjItem.InfoAdicional01, Integer)
                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.strDescripcionDepartamento = pobjItem.CodigoAuxiliar

                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.lngIDPais = CType(pobjItem.EtiquetaCodItem, Integer)
                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.strDescripcionPais = pobjItem.InfoAdicional02

                Case "codigociiu" 'SLB20140325 Manejo del Buscador de Código Ciiu
                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.intIDCodigoCIIU = CType(pobjItem.CodItem, Integer?)
                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.strDescripcionCodigoCIIU = pobjItem.Descripcion

                Case "claseinversion" 'JP20150716 Jorge Peña Clase para combo Clase Inversión según si se marca el CheckBox Cartera Colectiva
                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.strClaseInversion = pobjItem.IdItem
                    'If IsNothing(CType(Me.DataContext, EntidadesViewModel).ClaseInversionSeleccionada) Then
                    '    CType(Me.DataContext, EntidadesViewModel).ClaseInversionSeleccionada = New ClaseInversion() With {.strcodigo = pobjItem.IdItem}
                    'End If
                    CType(Me.DataContext, EntidadesViewModel).EncabezadoSeleccionado.strDescripcionClaseInversion = pobjItem.Descripcion

            End Select
        End If
    End Sub

    Private Sub txtNit_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Me.mobjVM.CalcularDigitoNIT()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al perder el foco del nit.", _
                                 Me.ToString(), "txtNit_LostFocus", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Notifica que la propiedad lngIDPortafolio cambió sin tener que perder el foco.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtNit_TextChanged(sender As Object, e As TextChangedEventArgs)
        CType(sender, TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource()
    End Sub

    Private Sub txtDigitoVerificacion_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.Numeros)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        End If
                    End If
                Else
                    If Not IsNothing(Me.mobjVM.EncabezadoSeleccionado) Then
                        Me.mobjVM.EncabezadoSeleccionado.intDigitoVerificacion = Nothing
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el dígito de verificación.", _
                                 Me.ToString(), "TextBox_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtCodigoComisionista_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.Numeros)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        End If
                    End If
                Else
                    If Not IsNothing(Me.mobjVM.EncabezadoSeleccionado) Then
                        Me.mobjVM.EncabezadoSeleccionado.intIDComisionista = Nothing
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el código comisionista.", _
                                 Me.ToString(), "TextBox_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Descripción:   Se comento la asignacion a los combos ya que afectaban los demas registros
    ''' Responsable:   Yessid Andres Paniagua Pabon (AlCuadrado)
    ''' Fecha:         Enero 2016/26
    ''' ID del cambio: YAPP20160126
    ''' </summary>
    Private Sub ComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            Me.mobjVM.ConsultarListaCalificaiones()
            'YAPP20160126
            'Me.cboCalificacionLarga.SelectedValue = Nothing
            'Me.cboCalificacionCorta.SelectedValue = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la calificadora.", _
                                Me.ToString(), "ComboBox_SelectionChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Descripción:   Llamado a el filtro de listado de grupos
    ''' Responsable:   Yessid Andres Paniagua Pabon (AlCuadrado)
    ''' Fecha:         Diciembre 2015/28
    ''' ID del cambio: YAPP<YYYYMMDD>
    ''' </summary>
    Private Sub ComboBox_SelectionChanged_1(sender As Object, e As SelectionChangedEventArgs)
        Try
            Me.mobjVM.ConsultarListaSubGrupos()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la calificadora.", _
                                Me.ToString(), "ComboBox_SelectionChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

End Class

