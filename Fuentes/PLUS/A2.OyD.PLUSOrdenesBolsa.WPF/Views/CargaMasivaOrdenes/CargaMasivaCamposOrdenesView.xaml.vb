Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class CargaMasivaCamposOrdenesView
    Inherits UserControl
    Dim objVMOrdenes As CargaMasivaOrdenesOYDPLUSViewModel
    Private mlogInicializado As Boolean = False
    Dim objVMA2Utils As A2UtilsViewModel
    Public Sub New(ByVal pobjVMOrdenes As CargaMasivaOrdenesOYDPLUSViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMOrdenes = pobjVMOrdenes

            If Me.Resources.Contains("VMOrdenes") Then
                Me.Resources.Remove("VMOrdenes")
            End If

            Me.Resources.Add("VMOrdenes", objVMOrdenes)
            InitializeComponent()

            AddHandler Me.SizeChanged, AddressOf CambioDePantalla


        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOrdenesView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMOrdenes) Then
                'Me.objVMOrdenes.actualizarItemOrden(pstrClaseControl, pobjItem)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda_CruzadaCon(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjItem) Then
                    'objVMOrdenes.ObtenerReceptorCruzada(pobjItem.IdItem, pobjItem.Nombre)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.ComitenteSeleccionadoOYDPLUS = pobjComitente
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitenteAsignadoADR(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjComitente) Then
                    Me.objVMOrdenes.OrdenOYDPLUSSelected.IDComitenteADR = pobjComitente.IdComitente
                    Me.objVMOrdenes.OrdenOYDPLUSSelected.TipoIdentificacionADR = pobjComitente.TipoIdentificacion
                    Me.objVMOrdenes.OrdenOYDPLUSSelected.NroDocumentoADR = pobjComitente.NroDocumento
                    Me.objVMOrdenes.OrdenOYDPLUSSelected.NombreClienteADR = pobjComitente.Nombre
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_nemotecnicoAsignado(pstrNemotecnico As System.String, pstrNombreNemotecnico As System.String)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(objVMOrdenes.OrdenOYDPLUSSelected) Then
                    objVMOrdenes.OrdenOYDPLUSSelected.Especie = pstrNemotecnico
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_especieAsignada(pstrNemotecnico As System.String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjEspecie) Then
                    Me.objVMOrdenes.NemotecnicoSeleccionadoOYDPLUS = pobjEspecie
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_especieAsignada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.ComitenteSeleccionadoOYDPLUS = Nothing
                If Me.objVMOrdenes.BorrarCliente Then
                    Me.objVMOrdenes.BorrarCliente = False
                End If
                Me.objVMOrdenes.BorrarCliente = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarClienteADR_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.OrdenOYDPLUSSelected.IDComitenteADR = Nothing
                Me.objVMOrdenes.OrdenOYDPLUSSelected.TipoIdentificacionADR = String.Empty
                Me.objVMOrdenes.OrdenOYDPLUSSelected.NroDocumentoADR = String.Empty
                Me.objVMOrdenes.OrdenOYDPLUSSelected.NombreClienteADR = String.Empty
                If Me.objVMOrdenes.BorrarClienteADR Then
                    Me.objVMOrdenes.BorrarClienteADR = False
                End If
                Me.objVMOrdenes.BorrarClienteADR = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.NemotecnicoSeleccionadoOYDPLUS = Nothing
                If Me.objVMOrdenes.BorrarEspecie Then
                    Me.objVMOrdenes.BorrarEspecie = False
                End If
                Me.objVMOrdenes.BorrarEspecie = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub
    Private Sub Ordenes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            '  If mlogInicializado = False Then
            '    If Not IsNothing(objVMA2Utils) Then
            '        cm.GridTransicion = grdGridForma


            '        objVMOrdenes = CType(Me.DataContext, OrdenesOYDPLUSViewModel)

            '        If Not IsNothing(pobjModulo) Then
            '            txtTituloOrdenes.Text = pobjModulo.TituloVistaModulo
            '            objVMOrdenes.DiccionarioBotonesOrdenes = pobjModulo.CamposControlMenu
            '            objVMOrdenes.Modulo = pobjModulo.Modulo

            '            If objVMOrdenes.DiccionarioBotonesOrdenes.ContainsKey("AbrirPlantillas") Then
            '                objVMOrdenes.HabilitarAbrirPlantillas = True
            '            Else
            '                objVMOrdenes.HabilitarAbrirPlantillas = False
            '            End If
            '            If objVMOrdenes.DiccionarioBotonesOrdenes.ContainsKey("GenerarPlantilla") Then
            '                objVMOrdenes.HabilitarGenerarPlantillas = True
            '            Else
            '                objVMOrdenes.HabilitarGenerarPlantillas = False
            '            End If
            '            If objVMOrdenes.DiccionarioBotonesOrdenes.ContainsKey("Duplicar") Then
            '                objVMOrdenes.HabilitarDuplicar = True
            '            Else
            '                objVMOrdenes.HabilitarDuplicar = False
            '            End If
            '        Else
            '            objVMOrdenes.DiccionarioBotonesOrdenes = objVMOrdenes.DicBotonesMenuVM
            '            objVMOrdenes.HabilitarAbrirPlantillas = True
            '            objVMOrdenes.HabilitarGenerarPlantillas = True
            '            objVMOrdenes.HabilitarDuplicar = True
            '        End If

            '        objVMOrdenes.ListaCombosEsp = mstrDicCombosEspecificos
            '        objVMOrdenes.ViewOrdenesOYDPLUS = Me
            '        objVMOrdenes.visNavegando = "Collapse"
            '        'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla

            '        'Inicia el timer de ordenes
            '        If Not IsNothing(objVMOrdenes) Then
            '            objVMOrdenes.ReiniciaTimer()
            '        End If
            '        'scrollEdicion.MaxWidth = Application.Current.MainWindow.ActualWidth - 100
            '    End If

            '    mlogInicializado = True
            'End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "Ordenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub txtPrecio_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(objVMOrdenes.OrdenOYDPLUSSelected) Then
                    If objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_SIMULTANEA Or objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objVMOrdenes.TIPONEGOCIO_RENTAFIJA Then
                        If objVMOrdenes.DiccionarioEdicionCamposOYDPLUS("PrecioConGarantia") Then
                            objVMOrdenes.OrdenOYDPLUSSelected.PrecioMaximoMinimo = objVMOrdenes.OrdenOYDPLUSSelected.Precio
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del precio.", Me.Name, "txtPrecio_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
