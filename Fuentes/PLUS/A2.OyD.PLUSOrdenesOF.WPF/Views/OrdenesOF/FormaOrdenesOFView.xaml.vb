Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class FormaOrdenesOFView
    Inherits UserControl
    Dim objVMOrdenes As OrdenesOYDPLUSOFViewModel
    Dim logCambioValor As Boolean = False

    Public Sub New(ByVal pobjVMOrdenes As OrdenesOYDPLUSOFViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMOrdenes = pobjVMOrdenes

            If Me.Resources.Contains("VMOrdenes") Then
                Me.Resources.Remove("VMOrdenes")
            End If

            Me.Resources.Add("VMOrdenes", objVMOrdenes)

            InitializeComponent()
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
            Me.stackDistribucionComisiones.Width = Application.Current.MainWindow.ActualWidth * 0.94
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOrdenesView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.stackDistribucionComisiones.Width = Application.Current.MainWindow.ActualWidth * 0.94
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.actualizarItemOrden(pstrClaseControl, pobjItem)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda_CruzadaCon(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMOrdenes) Then
                If Not IsNothing(pobjItem) Then

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

    Private Sub txtCalculo_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            If Not IsNothing(objVMOrdenes) Then
                If objVMOrdenes.logEditarRegistro Or objVMOrdenes.logNuevoRegistro Then
                    Dim strTipoControl As String = String.Empty

                    strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag

                    logCambioValor = True
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtCalculo_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Dim strTipoControl As String = String.Empty

                If TypeOf sender Is TextBox Then
                    strTipoControl = CType(sender, TextBox).Tag
                ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                    strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag
                End If

                If logCambioValor Then
                    Await objVMOrdenes.CalcularValorOrden(objVMOrdenes.OrdenOYDPLUSSelected)
                    logCambioValor = False
                End If

            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "txtCalculo_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
