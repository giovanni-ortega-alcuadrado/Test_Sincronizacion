Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Globalization

'
Partial Public Class wppFrmCargarPagosAFondos_RC
    Inherits Window
    Dim vm As OrdenesReciboViewModel_OYDPLUS

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = New TesoreriaViewModel_OYDPLUS
InitializeComponent()
            Me.DataContext = vm

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppFrmCheque", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub New(ByVal pvm As OrdenesReciboViewModel_OYDPLUS)
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            vm = CType(pvm, OrdenesReciboViewModel_OYDPLUS)
            Me.DataContext = New TesoreriaViewModel_OYDPLUS
InitializeComponent()
            Me.DataContext = vm

            If Not IsNothing(vm) Then
                Me.Title = vm.TituloPestanaCargarPagosAFondos
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppFrmCargarPagosA", Me.Name, "New(Sobrecargado)", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Saliendo(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        If vm.logHayEncabezado Then
            vm.HabilitarImportacion = True
        Else
            vm.HabilitarImportacion = False
        End If
        If Not IsNothing(vm.TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected) Then
            If Not IsNothing(vm.TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID) Then
                vm.dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(vm.TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf vm.TerminoCancelarEditarRegistro, String.Empty)
            End If
        End If
    End Sub

    Private Sub ChildWindow_Closing(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles Me.Closing
        If Not IsNothing(vm.TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected) Then
            If Not IsNothing(vm.TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID) Then
                vm.dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(vm.TesoreriaordenesplusRC_Detalle_CargarPagosAFondos_selected.lngID, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf vm.TerminoCancelarEditarRegistro, String.Empty)
            End If
        End If
        vm.BuscarControlValidacion(vm.OrdenesReciboPLUSView, "TabItemCargarPagoAFondos")
    End Sub
    Private Sub ChildWindow_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded


    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrClaseControl = "conceptos" Then
                    Me.vm.IdConceptoFondos = CInt(pobjItem.IdItem)
                    Me.vm.DescripcionComboConceptoFondos = pobjItem.Descripcion
                Else
                    vm.BancoChequewpp = pobjItem.Descripcion
                    vm.IdBanco = pobjItem.IdItem
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener el resultado del buscador.", Me.Name, "New", "BuscadorGenerico_finalizoBusqueda", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub NumValorGenerar_LostFocus(sender As Object, e As RoutedEventArgs)

    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Private Sub txtConcepto_TextChanged_1(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.Caracteres)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.", _
                                 Me.ToString(), "txtConcepto_TextChanged_1", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtDescripcionConcepto_TextChanged_1(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.Caracteres)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.", _
                                 Me.ToString(), "txtConcepto_TextChanged_1", Application.Current.ToString(), Program.Maquina, ex)
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

    Private Sub Buscador_Cliente_GotFocus(sender As Object, e As RoutedEventArgs)
        If vm.IDTipoClienteCargarPagosAFondos = GSTR_CLIENTE Then
            Buscador_Cliente.Agrupamiento = "id*" + CType(Me.DataContext, OrdenesReciboViewModel_OYDPLUS).TesoreriaOrdenesPlusRC_Selected.strIDComitente
        ElseIf vm.IDTipoClienteCargarPagosAFondos = GSTR_TERCERO Then
            Buscador_Cliente.Agrupamiento = ""
        End If
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            vm.strCodigoOyDCargarPagosAFondos = pobjComitente.CodigoOYD
            If Not String.IsNullOrEmpty(vm.strCodigoOyDCargarPagosAFondos) Then
                vm.ConsultarCarterasColectivasFondos(vm.strCodigoOyDCargarPagosAFondos, False, "CARTERASCLIENTE")
            End If

        End If
    End Sub

    Private Sub TextBlockCliente_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        If vm.IDTipoClienteCargarPagosAFondos = GSTR_CLIENTE Then
            Buscador_Cliente.Agrupamiento = "id*" + CType(Me.DataContext, OrdenesReciboViewModel_OYDPLUS).TesoreriaOrdenesPlusRC_Selected.strIDComitente
        ElseIf vm.IDTipoClienteCargarPagosAFondos = GSTR_TERCERO Then
            Buscador_Cliente.Agrupamiento = ""
        End If

        Buscador_Cliente.AbrirBuscador()
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As Object, e As RoutedEventArgs)
        Try
            vm.strCodigoOyDCargarPagosAFondos = String.Empty
            vm.NroEncargoFondos = Nothing
            vm.ListaEncargosCarteraColectiva = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el texto.", _
                                Me.ToString(), "btnLimpiarCliente_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TextBlockConcepto_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorConcepto.AbrirBuscador()
    End Sub

    Private Sub btnLimpiarConcepto_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.vm.IdConceptoFondos = Nothing
            Me.vm.DescripcionComboConceptoFondos = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el texto.", _
                                 Me.ToString(), "btnLimpiarConcepto_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

End Class


