Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class FormaLibranzasView
    Inherits UserControl
    Dim objVMLibranzas As LibranzasViewModel
    Dim logDigitoValor As Boolean = False
    Dim logCambioValor As Boolean = False

    Public Sub New(ByVal pobjVMLibranzas As LibranzasViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMLibranzas = pobjVMLibranzas

            If Me.Resources.Contains("VMLibranzas") Then
                Me.Resources.Remove("VMLibranzas")
            End If

            Me.Resources.Add("VMLibranzas", pobjVMLibranzas)
            InitializeComponent()

            'Me.stackDistribucionComisiones.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOperacionesOtrosNegociosView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        'Me.stackDistribucionComisiones.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.95
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMLibranzas) Then
                If Not IsNothing(pobjItem) Then
                    If pstrClaseControl.ToUpper = "EMISOR" Then
                        objVMLibranzas.EncabezadoSeleccionado.intIDEmisor = CType(pobjItem.IdItem, Integer?)
                        objVMLibranzas.EncabezadoSeleccionado.strCodTipoIdentificacionEmisor = pobjItem.InfoAdicional01
                        objVMLibranzas.EncabezadoSeleccionado.strNumeroDocumentoEmisor = pobjItem.CodItem
                        objVMLibranzas.EncabezadoSeleccionado.strNombreEmisor = pobjItem.Nombre
                    ElseIf pstrClaseControl.ToUpper = "PAGADOR" Then
                        objVMLibranzas.EncabezadoSeleccionado.intIDPagador = CType(pobjItem.IdItem, Integer?)
                        objVMLibranzas.EncabezadoSeleccionado.strCodTipoIdentificacionPagador = pobjItem.InfoAdicional01
                        objVMLibranzas.EncabezadoSeleccionado.strNumeroDocumentoPagador = pobjItem.CodItem
                        objVMLibranzas.EncabezadoSeleccionado.strNombrePagador = pobjItem.Nombre
                    ElseIf pstrClaseControl.ToUpper = "CUSTODIO" Then
                        objVMLibranzas.EncabezadoSeleccionado.intIDCustodio = CType(pobjItem.IdItem, Integer?)
                        objVMLibranzas.EncabezadoSeleccionado.strCodTipoIdentificacionCustodio = pobjItem.InfoAdicional01
                        objVMLibranzas.EncabezadoSeleccionado.strNumeroDocumentoCustodio = pobjItem.CodItem
                        objVMLibranzas.EncabezadoSeleccionado.strNombreCustodio = pobjItem.Nombre
                    End If
                Else
                    If pstrClaseControl.ToUpper = "EMISOR" Then
                        ctlBuscadorEmisor.Focus()
                    ElseIf pstrClaseControl.ToUpper = "PAGADOR" Then
                        ctlBuscadorPagador.Focus()
                    ElseIf pstrClaseControl.ToUpper = "CUSTODIO" Then
                        ctlBuscadorCustodio.Focus()
                    End If
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMLibranzas) Then
                If Not IsNothing(pobjComitente) Then
                    objVMLibranzas.ComitenteSeleccionado = pobjComitente
                Else
                    ctlBuscadorClientes.Focus()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMLibranzas) Then
                Me.objVMLibranzas.ComitenteSeleccionado = Nothing
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarEmisor_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMLibranzas) Then
                objVMLibranzas.EncabezadoSeleccionado.intIDEmisor = Nothing
                objVMLibranzas.EncabezadoSeleccionado.strCodTipoIdentificacionEmisor = String.Empty
                objVMLibranzas.EncabezadoSeleccionado.strNumeroDocumentoEmisor = String.Empty
                objVMLibranzas.EncabezadoSeleccionado.strNombreEmisor = String.Empty
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEmisor_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCustodio_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMLibranzas) Then
                objVMLibranzas.EncabezadoSeleccionado.intIDCustodio = Nothing
                objVMLibranzas.EncabezadoSeleccionado.strCodTipoIdentificacionCustodio = String.Empty
                objVMLibranzas.EncabezadoSeleccionado.strNumeroDocumentoCustodio = String.Empty
                objVMLibranzas.EncabezadoSeleccionado.strNombreCustodio = String.Empty
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCustodio_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarPagador_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMLibranzas) Then
                objVMLibranzas.EncabezadoSeleccionado.intIDPagador = Nothing
                objVMLibranzas.EncabezadoSeleccionado.strCodTipoIdentificacionPagador = String.Empty
                objVMLibranzas.EncabezadoSeleccionado.strNumeroDocumentoPagador = String.Empty
                objVMLibranzas.EncabezadoSeleccionado.strNombrePagador = String.Empty
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarPagador_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub txtCalculo_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMLibranzas) Then
                If objVMLibranzas.logEditarRegistro Or objVMLibranzas.logNuevoRegistro Then
                    Dim strTipoControl As String = String.Empty

                    If TypeOf sender Is TextBox Then
                        strTipoControl = CStr(CType(sender, TextBox).Tag)
                    ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                        strTipoControl = CStr(CType(sender, A2Utilidades.A2NumericBox).Tag)
                    End If

                    If objVMLibranzas.logCalcularValores Then
                        logDigitoValor = True
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub txtCalculo_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMLibranzas) Then
                If objVMLibranzas.logEditarRegistro Or objVMLibranzas.logNuevoRegistro Then
                    If logDigitoValor Then
                        Dim strTipoControl As String = String.Empty

                        If TypeOf sender Is TextBox Then
                            strTipoControl = CStr(CType(sender, TextBox).Tag)
                        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                            strTipoControl = CStr(CType(sender, A2Utilidades.A2NumericBox).Tag)
                        End If

                        If objVMLibranzas.logCalcularValores Then
                            logCambioValor = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCalculo_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            If Not IsNothing(objVMLibranzas) Then
                If objVMLibranzas.logEditarRegistro Or objVMLibranzas.logNuevoRegistro Then
                    If logDigitoValor Then
                        Dim strTipoControl As String = String.Empty

                        strTipoControl = CStr(CType(sender, A2Utilidades.A2NumericBox).Tag)

                        If objVMLibranzas.logCalcularValores Then
                            logCambioValor = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtCalculo_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMLibranzas) Then
                If objVMLibranzas.logEditarRegistro Or objVMLibranzas.logNuevoRegistro Then
                    If logCambioValor And objVMLibranzas.logCalcularValores Then
                        Dim strTipoControl As String = String.Empty

                        If TypeOf sender Is TextBox Then
                            strTipoControl = CStr(CType(sender, TextBox).Tag)
                        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                            strTipoControl = CStr(CType(sender, A2Utilidades.A2NumericBox).Tag)
                        End If

                        logCambioValor = False
                        logDigitoValor = False
                        Await objVMLibranzas.CalcularValorRegistro()
                        'System.Web.HtmlPage.Plugin.Focus()
                        ReubicarFocoControl(sender)
                    End If

                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "txtCalculo_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ReubicarFocoControl(ByVal pobjControl As Object)
        Try
            Dim strNombreControl As String = String.Empty

            If TypeOf pobjControl Is TextBox Then
                strNombreControl = CType(pobjControl, TextBox).Name
            ElseIf TypeOf pobjControl Is A2Utilidades.A2NumericBox Then
                strNombreControl = CType(pobjControl, A2Utilidades.A2NumericBox).Name
            End If

            If strNombreControl = "txtNrocuotas" Then
                cboPeriodoCuota.Focus()
            ElseIf strNombreControl = "txtValorCreditoOriginal" Then
                dtmFechaInicioCredito.Focus()
            ElseIf strNombreControl = "txtTasaInteres" Then
                txtTasaDescuento.Focus()
            ElseIf strNombreControl = "txtTasaDescuento" Then
                ctlBuscadorEmisor.Focus()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "ReubicarFocoControl", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub BuscarControlValidacion(ByVal pstrOpcion As String)
        Try
            If Not IsNothing(Me) Then
                If TypeOf Me.FindName(pstrOpcion) Is TabItem Then
                    CType(Me.FindName(pstrOpcion), TabItem).IsSelected = True
                ElseIf TypeOf Me.FindName(pstrOpcion) Is TextBox Then
                    CType(Me.FindName(pstrOpcion), TextBox).Focus()
                ElseIf TypeOf Me.FindName(pstrOpcion) Is ComboBox Then
                    CType(Me.FindName(pstrOpcion), ComboBox).Focus()
                
                    
                ElseIf TypeOf Me.FindName(pstrOpcion) Is A2Utilidades.A2NumericBox Then
                    CType(Me.FindName(pstrOpcion), A2Utilidades.A2NumericBox).Focus()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control dentro del registro.", Me.ToString, "BuscarControlValidacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtComitente_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMLibranzas.DiccionarioHabilitarCampos(LibranzasViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorClientes.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtComitente_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtPagador_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMLibranzas.DiccionarioHabilitarCampos(LibranzasViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorPagador.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtPagador_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtEmisor_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMLibranzas.DiccionarioHabilitarCampos(LibranzasViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorEmisor.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtEmisor_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCustodio_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMLibranzas.DiccionarioHabilitarCampos(LibranzasViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorCustodio.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtCustodio_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    
End Class
