Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class FormaInmobiliariosView
    Inherits UserControl
    Dim objVMInmuebles As InmobiliariosViewModel
    Dim logDigitoValor As Boolean = False
    Dim logCambioValor As Boolean = False
    Dim strNombreControlCambio As String = String.Empty

    Public Sub New(ByVal pobjVMInmuebles As InmobiliariosViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMInmuebles = pobjVMInmuebles

            If Me.Resources.Contains("VMInmuebles") Then
                Me.Resources.Remove("VMInmuebles")
            End If

            Me.Resources.Add("VMInmuebles", pobjVMInmuebles)
            InitializeComponent()

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOperacionesOtrosNegociosView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMInmuebles) Then
                If Not IsNothing(pobjItem) Then
                    If pstrClaseControl.ToUpper = "MONEDANEGOCIACION" Then
                        objVMInmuebles.EncabezadoSeleccionado.MonedaNegociacion = pobjItem.IdItem
                        objVMInmuebles.EncabezadoSeleccionado.CodigoMonedaNegociacion = pobjItem.CodItem
                        objVMInmuebles.EncabezadoSeleccionado.NombreMonedaNegociacion = pobjItem.Nombre

                        ctlBuscadorMonedaNegociacion.Focus()
                    ElseIf pstrClaseControl.ToUpper = "DETALLECONCEPTOFILTRO" Then
                        objVMInmuebles.ConceptoFiltro = pobjItem.IdItem
                    ElseIf pstrClaseControl.ToUpper = "DETALLECONCEPTONUEVO" Then
                        objVMInmuebles.ConceptoSeleccionadoNuevoRegistro = pobjItem
                        objVMInmuebles.IDConceptoNuevoRegistro = pobjItem.IdItem
                        objVMInmuebles.ConceptoNuevoRegistro = pobjItem.Nombre
                    End If
                Else
                    If pstrClaseControl.ToUpper = "MONEDANEGOCIACION" Then
                        ctlBuscadorMonedaNegociacion.Focus()
                    End If
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMInmuebles) Then
                If Not IsNothing(pobjComitente) Then
                    objVMInmuebles.ComitenteSeleccionado = pobjComitente
                    ctrlCliente.Focus()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMInmuebles) Then
                Me.objVMInmuebles.ComitenteSeleccionado = Nothing
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub txtCalculo_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMInmuebles) Then
                If objVMInmuebles.logEditarRegistro Or objVMInmuebles.logNuevoRegistro Then
                    Dim strTipoControl As String = String.Empty

                    If TypeOf sender Is TextBox Then
                        strTipoControl = CType(sender, TextBox).Tag
                    ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                        strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag
                    End If

                    If objVMInmuebles.logCalcularValores Then
                        logDigitoValor = True
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub txtCalculo_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            If Not IsNothing(objVMInmuebles) Then
                If objVMInmuebles.logEditarRegistro Or objVMInmuebles.logNuevoRegistro And objVMInmuebles.logCalcularValores Then
                    If logDigitoValor Then
                        Dim strTipoControl As String = String.Empty

                        strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag

                        logCambioValor = True
                    Else
                        If CType(sender, A2Utilidades.A2NumericBox).Name = strNombreControlCambio And _
                            e.NewValue <> e.OldValue Then
                            logCambioValor = True
                            strNombreControlCambio = String.Empty
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCalculo_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMInmuebles) Then
                If objVMInmuebles.logEditarRegistro Or objVMInmuebles.logNuevoRegistro Then
                    If logCambioValor And objVMInmuebles.logCalcularValores Then
                        Dim strTipoControl As String = String.Empty

                        If TypeOf sender Is TextBox Then
                            strTipoControl = CType(sender, TextBox).Tag
                        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                            strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag
                        End If

                        logCambioValor = False
                        logDigitoValor = False
                        objVMInmuebles.CalcularValorRegistro()
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

    Private Sub txtMonedaNegociacion_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMInmuebles.DiccionarioHabilitarCampos(InmobiliariosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctlBuscadorMonedaNegociacion.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtMonedaNegociacion_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtPortafolio_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMInmuebles.DiccionarioHabilitarCampos(InmobiliariosViewModel.OPCIONES_HABILITARCAMPOS.HABILITARNEGOCIO.ToString) Then
                ctrlCliente.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtPortafolio_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    
    Private Sub BuscadorGenericoListaButon_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            Dim intIDDetalle As Integer = CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Tag
            If Not IsNothing(objVMInmuebles.DetalleSeleccionado) Then
                If objVMInmuebles.DetalleSeleccionado.ID <> intIDDetalle Then
                    objVMInmuebles.DetalleSeleccionado = objVMInmuebles.ListaDetalle.Where(Function(i) i.ID = intIDDetalle).First
                End If
            Else
                objVMInmuebles.DetalleSeleccionado = objVMInmuebles.ListaDetalle.Where(Function(i) i.ID = intIDDetalle).First
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "BuscadorGenericoListaButon_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtConceptoFiltro_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            ctlConceptoFiltro.AbrirBuscador()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtConceptoFiltro_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnNuevoDetalle_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVMInmuebles.Detalle_NuevoRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la acción en el detalle.", Me.ToString, "btnNuevoDetalle_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnEditarDetalle_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVMInmuebles.Detalle_EditarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la acción en el detalle.", Me.ToString, "btnEditarDetalle_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnBorrarDetalle_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVMInmuebles.Detalle_BorrarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar la acción en el detalle.", Me.ToString, "btnBorrarDetalle_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub btnConsultarDetalles_Click(sender As Object, e As RoutedEventArgs)
        Try
            If objVMInmuebles.ValidarMaximosDiasEntreFechasFiltro() Then
                Await objVMInmuebles.ConsultarDetalle(objVMInmuebles.EncabezadoSeleccionado.ID, objVMInmuebles.FechaInicialFiltro, objVMInmuebles.FechaFinalFiltro, objVMInmuebles.ConceptoFiltro)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "btnConsultarDetalles_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtConceptoNuevoRegistro_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            ctlConceptoNuevo.AbrirBuscador()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtConceptoNuevoRegistro_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlConceptoNuevo_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = objVMInmuebles.TipoMovimientoNuevo
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "ctlConceptoNuevo_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarConsulta_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVMInmuebles.FechaFinalFiltro = Nothing
            objVMInmuebles.FechaInicialFiltro = Nothing
            objVMInmuebles.ConceptoFiltro = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "btnLimpiarConsulta_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
