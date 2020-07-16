Imports Telerik.Windows.Controls
Imports A2ComunesControl
Imports A2.OYD.OYDServer.RIA.Web



Partial Public Class CuentasFondosView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases CuentasFondosView y CuentasFondosViewModel
    ''' Pantalla ChoquesTasasInteres (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 21 de Febrero 2014</remarks>
#Region "Variables"

    Private mobjVM As CuentasFondosViewModel
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

        Me.DataContext = New CuentasFondosViewModel
        Me.Resources.Add("ViewModelPrincipal", Me.DataContext)
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
            mobjVM = CType(Me.DataContext, CuentasFondosViewModel)
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

    'OYDUtilidades
    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "ClienteAgrupador_Dataform_Edicion"
                        Me.mobjVM.EncabezadoSeleccionado.strTipoIdComitente = pobjItem.CodigoAuxiliar
                        Me.mobjVM.EncabezadoSeleccionado.strNroDocumento = pobjItem.IdItem
                        Me.mobjVM.EncabezadoSeleccionado.strNombre = pobjItem.Nombre
                        Me.mobjVM.EncabezadoSeleccionado.lngidComitenteLider = pobjItem.InfoAdicional02
                    Case "ClienteAgrupador_Dataform_Busqueda"
                        Me.mobjVM.cb.strTipoIdComitente = pobjItem.CodigoAuxiliar
                        Me.mobjVM.cb.strNroDocumento = pobjItem.IdItem
                        Me.mobjVM.cb.strNombre = pobjItem.Nombre
                    Case "Beneficiario_Dataform_Edicion"
                        If pobjItem.InfoAdicional01 = "PRIMER_BENEFICIARIO" Then
                            Me.mobjVM.EncabezadoSeleccionado.strTipoIdBenef1 = pobjItem.CodigoAuxiliar
                            Me.mobjVM.EncabezadoSeleccionado.lngNroDocBenef1 = pobjItem.descripcion
                            Me.mobjVM.EncabezadoSeleccionado.strPrimerBeneficiario = pobjItem.descripcion & " - " & pobjItem.Nombre
                        Else
                            Me.mobjVM.EncabezadoSeleccionado.strTipoIdBenef2 = pobjItem.CodigoAuxiliar
                            Me.mobjVM.EncabezadoSeleccionado.lngNroDocBenef2 = pobjItem.descripcion
                            Me.mobjVM.EncabezadoSeleccionado.strSegundoBeneficiario = pobjItem.descripcion & " - " & pobjItem.Nombre
                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al buscar el cliente.", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'SV20180406_AJUSTEFILTRO
    Private Sub BuscadorClienteListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                Me.mobjVM.cb.strCodigoOyD = pobjComitente.IdComitente
                Me.mobjVM.cb.strNombreCodigoOyD = pobjComitente.Nombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el cliente.", _
                                                Me.ToString(), "BuscadorClienteListaButon_finalizoBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Select Case CType(sender, Button).Name
                    Case "btnLimpiar_Dataform_Busqueda"
                        Me.mobjVM.cb.strTipoIdComitente = String.Empty
                        Me.mobjVM.cb.strNroDocumento = String.Empty
                        Me.mobjVM.cb.strNombre = String.Empty
                    Case "btnLimpiar_Dataform_Edicion"
                        Me.mobjVM.EncabezadoSeleccionado.strTipoIdComitente = String.Empty
                        Me.mobjVM.EncabezadoSeleccionado.strNroDocumento = String.Empty
                        Me.mobjVM.EncabezadoSeleccionado.strNombre = String.Empty
                    Case "btnLimpiar_PrimerBeneficiario_Dataform_Edicion"
                        Me.mobjVM.EncabezadoSeleccionado.strTipoIdBenef1 = Nothing
                        Me.mobjVM.EncabezadoSeleccionado.lngNroDocBenef1 = Nothing
                        Me.mobjVM.EncabezadoSeleccionado.strPrimerBeneficiario = Nothing
                    Case "btnLimpiar_SegundoBeneficiario_Dataform_Edicion"
                        Me.mobjVM.EncabezadoSeleccionado.strTipoIdBenef2 = Nothing
                        Me.mobjVM.EncabezadoSeleccionado.lngNroDocBenef2 = Nothing
                        Me.mobjVM.EncabezadoSeleccionado.strSegundoBeneficiario = Nothing
                    Case "btnLimpiar_Dataform_BusquedaCodigoOyD" 'SV20180406_AJUSTEFILTRO
                        Me.mobjVM.cb.strCodigoOyD = String.Empty
                        Me.mobjVM.cb.strNombreCodigoOyD = String.Empty
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar los controles.", Me.Name, "btnLimpiar_Dataform_Busqueda_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Async Sub lngidCuentaDeceval_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(Me.mobjVM.EncabezadoSeleccionado.strNroDocumento) And Not IsNothing(Me.mobjVM.EncabezadoSeleccionado.lngidCuentaDeceval) And Not IsNothing(Me.mobjVM.EncabezadoSeleccionado.strDeposito) Then
                Await Me.mobjVM.ConsultarDetalle(Me.mobjVM.EncabezadoSeleccionado.strNroDocumento, Me.mobjVM.EncabezadoSeleccionado.strTipoIdComitente, Me.mobjVM.EncabezadoSeleccionado.lngidCuentaDeceval, Me.mobjVM.EncabezadoSeleccionado.strDeposito)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al perder el foco del control.", Me.Name, "lngidCuentaDeceval_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

End Class