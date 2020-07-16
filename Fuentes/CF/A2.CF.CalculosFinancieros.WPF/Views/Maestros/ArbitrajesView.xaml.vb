Imports Telerik.Windows.Controls

Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web



Partial Public Class ArbitrajesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases ArbitrajesView y ArbitrajesViewModel
    ''' Pantalla ChoquesTasasInteres (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 21 de Febrero 2014</remarks>
#Region "Variables"

    Private mobjVM As ArbitrajesViewModel
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

        Me.DataContext = New ArbitrajesViewModel
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
            mobjVM = CType(Me.DataContext, ArbitrajesViewModel)
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

    Private Sub BuscadorGenerico_finalizo_Dataform_Edicion(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "SubyacenteCB"
                    Me.mobjVM.EncabezadoSeleccionado.strIDEspecie = pobjItem.Nombre
                Case "CruceReplicaCB"
                    Me.mobjVM.strDatosOperacionCruce = pobjItem.Descripcion
                    Me.mobjVM.intIDLiquidacionesCruce = CInt(pobjItem.IdItem)
                    Me.mobjVM.strOrigenCruce = pobjItem.InfoAdicional01
                    Me.mobjVM.strIDOperacionCruce = pobjItem.InfoAdicional02
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Notifica que la propiedad lngIDPortafolio cambió sin tener que perder el foco.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtPortafolio_TextChanged(sender As Object, e As TextChangedEventArgs)
        CType(sender, TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource()
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.EncabezadoSeleccionado.strIDEspecie = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                Select Case pstrClaseControl
                    Case "PortafolioADR"
                        Me.mobjVM.lngIDPortafolioADR = pobjComitente.CodigoOYD
                        Me.mobjVM.strNombrePortafolioADR = pobjComitente.Nombre
                    Case "PortafolioReplica"
                        Me.mobjVM.lngIDPortafolioReplica = pobjComitente.CodigoOYD
                        Me.mobjVM.strNombrePortafolioReplica = pobjComitente.Nombre
                    Case "PortafolioTraslado"
                        Me.mobjVM.lngIDPortafolioTraslado = pobjComitente.CodigoOYD
                        Me.mobjVM.strNombrePortafolioTraslado = pobjComitente.Nombre
                End Select
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM) Then
            Select Case CType(sender, Button).Name
                Case "btnLimpiarPortafolioADR"
                    Me.mobjVM.lngIDPortafolioADR = Nothing
                    Me.mobjVM.strNombrePortafolioADR = Nothing
                Case "btnLimpiarPortafolioReplica"
                    Me.mobjVM.lngIDPortafolioReplica = Nothing
                    Me.mobjVM.strNombrePortafolioReplica = Nothing
                Case "btnLimpiarPortafolioTraslado"
                    Me.mobjVM.lngIDPortafolioTraslado = Nothing
                    Me.mobjVM.strNombrePortafolioTraslado = Nothing
                Case "btnLimpiarCuentaDepositoReplica"
                    Me.mobjVM.strIDCuentaDepositoReplicaBuscador = Nothing
                    Me.mobjVM.strDepositoReplicaBuscador = Nothing
                Case "btnLimpiarCuentaDepositotraslado"
                    Me.mobjVM.strIDCuentaDepositoTrasladoBuscador = Nothing
                    Me.mobjVM.strDepositoTrasladoBuscador = Nothing
                Case "btnLimpiarCruceFiltro"
                    Me.mobjVM.strIDOperacionCruce = Nothing
                    Me.mobjVM.strDatosOperacionCruce = Nothing

            End Select
        End If
    End Sub

    Private Async Sub txtPortafolio_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Select Case CType(sender, TextBox).Name
                    Case "txtPortafolioADR"
                        Await Me.mobjVM.ConsultarDatosPortafolio(txtPortafolioADR.Text, "PortafolioADR")
                    Case "txtPortafolioReplica"
                        Await Me.mobjVM.ConsultarDatosPortafolio(txtPortafolioReplica.Text, "PortafolioReplica")
                    Case "txtPortafolioTraslado"
                        Await Me.mobjVM.ConsultarDatosPortafolio(txtPortafolioTraslado.Text, "PortafolioTraslado")
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenerico_finalizo_Dataform_Busqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "SubyacenteBusqueda"
                    mobjVM.cb.strIDEspecie = pobjItem.Nombre
            End Select
        End If
    End Sub

    Private Sub btnLimpiarEspecie_Dataform_Busqueda_Click(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM) Then
            Select Case CType(sender, Button).Name
                Case "btnLimpiarEspecie"
                    mobjVM.cb.strIDEspecie = Nothing
            End Select
        End If
    End Sub

    Private Sub cmbDepositoReplica_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        mobjVM.strIDCuentaDepositoReplicaBuscador = Nothing
    End Sub

    Private Sub BuscadorCuentasDepositoReplica_finalizoBusqueda(pintCuentaDeposito As Integer, pobjCuentaDeposito As OYDUtilidades.BuscadorCuentasDeposito)
        If Not IsNothing(pobjCuentaDeposito) Then
            mobjVM.strDepositoReplicaBuscador = pobjCuentaDeposito.Deposito
            mobjVM.strIDCuentaDepositoReplicaBuscador = CStr(pobjCuentaDeposito.NroCuentaDeposito)
            mobjVM.strNombreDepositoReplicaBuscador = pobjCuentaDeposito.NombreDeposito

        End If
    End Sub

    Private Sub cmbDepositoTraslado_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        mobjVM.strIDCuentaDepositoTrasladoBuscador = Nothing
    End Sub

    Private Sub BuscadorCuentasDepositoTraslado_finalizoBusqueda(pintCuentaDeposito As Integer, pobjCuentaDeposito As OYDUtilidades.BuscadorCuentasDeposito)
        If Not IsNothing(pobjCuentaDeposito) Then
            mobjVM.strDepositoTrasladoBuscador = pobjCuentaDeposito.Deposito
            mobjVM.strIDCuentaDepositoTrasladoBuscador = CStr(pobjCuentaDeposito.NroCuentaDeposito)
            mobjVM.strNombreDepositoTrasladoBuscador = pobjCuentaDeposito.NombreDeposito
        End If
    End Sub

#End Region

#Region "Manejadores error"

    Private Sub btnBuscar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BuscarOperaciones()
    End Sub

Private Sub btnReplicarSeleccionados_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ReplicarSeleccionados()
    End Sub

Private Sub btnEditar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarDetalle()
    End Sub

Private Sub btnNuevo_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.IngresarDetalle()
    End Sub

Private Sub btnBorrar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarDetalle()
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



