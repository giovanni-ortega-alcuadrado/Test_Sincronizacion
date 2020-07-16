Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class FormaAjustesManualesView
    Inherits UserControl
    Dim objVMAjustes As AjustesManualesViewModel
    Dim logDigitoValor As Boolean = False
    Dim logCambioValor As Boolean = False
    Dim strNombreControlCambio As String = String.Empty

    Public Sub New(ByVal pobjVMAjustes As AjustesManualesViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMAjustes = pobjVMAjustes

            If Me.Resources.Contains("VMAjustes") Then
                Me.Resources.Remove("VMAjustes")
            End If

            Me.Resources.Add("VMAjustes", pobjVMAjustes)
            InitializeComponent()

            Me.stackMovimientos.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOperacionesOtrosNegociosView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.stackMovimientos.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.95
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMAjustes) Then
                If Not IsNothing(pobjItem) Then
                    If pstrClaseControl.ToUpper = "MONEDA" Then
                        objVMAjustes.EncabezadoSeleccionado.IDMoneda = CType(pobjItem.IdItem, Integer?)
                        objVMAjustes.EncabezadoSeleccionado.CodMoneda = pobjItem.CodItem
                        objVMAjustes.EncabezadoSeleccionado.DescripcionMoneda = pobjItem.Nombre

                        cboTipoAjuste.Focus()
                    ElseIf pstrClaseControl.ToUpper = "EVENTOCONTABLE" Then
                        objVMAjustes.EncabezadoSeleccionado.IDEventoContable = CType(pobjItem.IdItem, Integer?)
                        objVMAjustes.EncabezadoSeleccionado.CodEventoContable = pobjItem.CodItem
                        objVMAjustes.EncabezadoSeleccionado.EventoContable = pobjItem.Nombre

                        ctlBuscadorClaseContable.Focus()
                    ElseIf pstrClaseControl.ToUpper = "CLASECONTABLE" Then
                        objVMAjustes.EncabezadoSeleccionado.ClaseContable = pobjItem.IdItem
                        objVMAjustes.EncabezadoSeleccionado.DescripcionClaseContable = pobjItem.Nombre

                        cboTipoInversion.Focus()
                    ElseIf pstrClaseControl.ToUpper = "CONCEPTOCONTABLE" Then
                        objVMAjustes.DetalleSeleccionado.IDConceptoContable = CInt(pobjItem.IdItem)
                        objVMAjustes.DetalleSeleccionado.ConceptoContable = pobjItem.CodItem
                        objVMAjustes.DetalleSeleccionado.DescripcionConceptoContable = pobjItem.Nombre
                    ElseIf pstrClaseControl.ToUpper = "CONCEPTOCONTABLEEVENTO" Then
                        objVMAjustes.DetalleSeleccionado.CuentaContable = pobjItem.IdItem
                    End If
                Else
                    If pstrClaseControl.ToUpper = "MONEDA" Then
                        ctlBuscadorMoneda.Focus()
                    ElseIf pstrClaseControl.ToUpper = "EVENTOCONTABLE" Then
                        ctlBuscadorEventoContable.Focus()
                    ElseIf pstrClaseControl.ToUpper = "CLASECONTABLE" Then
                        ctlBuscadorClaseContable.Focus()
                    ElseIf pstrClaseControl.ToUpper = "CONCEPTOCONTABLE" Then

                    ElseIf pstrClaseControl.ToUpper = "CONCEPTOCONTABLEEVENTO" Then

                    End If
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMAjustes) Then
                If Not IsNothing(pobjComitente) Then
                    objVMAjustes.ComitenteSeleccionado = pobjComitente
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        strNombreControlCambio = String.Empty

        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            strNombreControlCambio = CType(sender, TextBox).Name
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
            strNombreControlCambio = CType(sender, A2Utilidades.A2NumericBox).Name
        End If
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

    Private Sub txtIDComitente_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMAjustes.DiccionarioHabilitarCampos(AjustesManualesViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorCliente.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control.", Me.ToString, "txtIDComitente_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtMoneda_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMAjustes.DiccionarioHabilitarCampos(AjustesManualesViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMoneda.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control.", Me.ToString, "txtMoneda_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtEvento_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMAjustes.DiccionarioHabilitarCampos(AjustesManualesViewModel.OPCIONES_HABILITARCAMPOS.HABILITAREVENTO.ToString) Then
                ctlBuscadorEventoContable.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control.", Me.ToString, "txtEvento_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtClaseContable_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMAjustes.DiccionarioHabilitarCampos(AjustesManualesViewModel.OPCIONES_HABILITARCAMPOS.HABILITAREVENTO.ToString) Then
                ctlBuscadorClaseContable.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control.", Me.ToString, "txtEvento_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            If (objVMAjustes.DetalleSeleccionado.ID <> CInt(CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Tag)) Then
                objVMAjustes.DetalleSeleccionado = objVMAjustes.ListaDetalle.Where(Function(i) i.ID = CInt(CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Tag)).First
            End If

            If objVMAjustes.EncabezadoSeleccionado.TipoComprobante = objVMAjustes.TIPOCOMPROBANTE_BASADOENEVENTO Then
                CType(sender, A2ComunesControl.BuscadorGenericoListaButon).TipoItem = "CONCEPTOS_EVENTOCONTABLE"
                If Not IsNothing(objVMAjustes.EncabezadoSeleccionado.IDEventoContable) Then
                    CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = CStr(objVMAjustes.EncabezadoSeleccionado.IDEventoContable)
                End If
            Else
                CType(sender, A2ComunesControl.BuscadorGenericoListaButon).TipoItem = "CONCEPTO"
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control.", Me.ToString, "BuscadorGenericoListaButon_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus_1(sender As Object, e As RoutedEventArgs)
        Try
            If (objVMAjustes.DetalleSeleccionado.ID <> CInt(CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Tag)) Then
                objVMAjustes.DetalleSeleccionado = objVMAjustes.ListaDetalle.Where(Function(i) i.ID = CInt(CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Tag)).First
            End If

            Dim strEventoContableBuscar As String = String.Format("{0},{1},{2},{3},{4}",
                                                                     objVMAjustes.EncabezadoSeleccionado.IDEventoContable,
                                                                     objVMAjustes.DetalleSeleccionado.ConceptoContable,
                                                                     objVMAjustes.DetalleSeleccionado.Naturaleza,
                                                                     objVMAjustes.EncabezadoSeleccionado.NormaContable,
                                                                     objVMAjustes.EncabezadoSeleccionado.TipoInversion)

            CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = strEventoContableBuscar
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control.", Me.ToString, "BuscadorGenericoListaButon_GotFocus_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCuentaContable_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Try
            If (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D9) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D8) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D7) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D6) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D5) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D4) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D3) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D2) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D1) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D0) Then
                e.Handled = True
            Else
                If (((e.Key >= Key.D0 And e.Key <= Key.D9) Or (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) Or e.Key = Key.Back Or e.Key = Key.Tab)) Then
                    e.Handled = False
                Else
                    e.Handled = True
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el codigo ingresado.", _
                                Me.ToString(), "txtComitente_KeyDown", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
End Class
