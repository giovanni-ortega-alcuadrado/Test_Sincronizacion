Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Globalization

'
Partial Public Class wppFrmConsignaciones_RC
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
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppFrmTransferencia", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            ctlBuscadorCuentasBancariasDestino.TipoItem = vm.TipoCuentasBancarias
            If vm.logEsFondosOYD Then
                ctlBuscadorCuentasBancariasDestino.Agrupamiento = "cartera"
                ctlBuscadorCuentasBancariasDestino.Condicion1 = vm.CarteraColectivaFondos
            Else
                ctlBuscadorCuentasBancariasDestino.Agrupamiento = "firma"
                ctlBuscadorCuentasBancariasDestino.Condicion1 = String.Empty
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppFrmTransferencia", Me.Name, "New(Sobrecargado)", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private Sub Saliendo(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        vm.HabilitarImportacion = True
        If Not IsNothing(vm.TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected) Then
            If Not IsNothing(vm.TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID) Then
                vm.dcProxy.OYDPLUS_CancelarOrdenOYDPLUS(vm.TesoreriaOrdenesPlusRC_Detalle_Transferencias_Selected.lngID, GSTR_OTD, Program.Usuario, Program.HashConexion, AddressOf vm.TerminoCancelarEditarRegistro, String.Empty)
            End If
        End If

    End Sub

    Private Sub NumValorGenerar_LostFocus_1(sender As System.Object, e As System.Windows.RoutedEventArgs)


    End Sub



    Private Sub ChildWindow_Closing(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles Me.Closing

        vm.BuscarControlValidacion(vm.OrdenesReciboPLUSView, "tabItemConsignacion")
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As System.String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then

                vm.strDescripcionBancoConsignacion = pobjItem.Descripcion
                vm.lngCodigoBancoConsignacionWpp = pobjItem.IdItem
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener la cuenta bancaria.", Me.Name, "BuscadorGenerico_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private Sub ChildWindow_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded


    End Sub



    Private Sub BuscadorGenericoDestino_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If vm.TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
                    vm.strDescripcionCuentaConsignacion = pobjItem.Descripcion
                Else
                    vm.strDescripcionCuentaConsignacion = pobjItem.IdItem + " - " + pobjItem.Nombre
                End If

                vm.strCuentaConsignacion = pobjItem.IdItem
                vm.strTipoCuentaConsignacion = pobjItem.Nombre
                If InStr(pobjItem.InfoAdicional02.ToUpper, "CORRIENTE") > 0 Then
                    vm.strTipoCuentaConsignacion = "C"

                ElseIf InStr(pobjItem.InfoAdicional02.ToUpper, "AHORRO") > 0 Then
                    vm.strTipoCuentaConsignacion = "A"
                End If
                'For Each li In Split(pobjItem.Descripcion, "-")
                '    If vm.IsNumeric(li) Then
                '        vm.lngCodigoBancoDestinoConsignacionWpp = Integer.Parse(RTrim(LTrim(li)))
                '    End If
                'Next
                vm.lngCodigoBancoDestinoConsignacionWpp = pobjItem.IdItem
                If String.IsNullOrEmpty(pobjItem.CodItem) Then
                    mostrarMensaje("¡Validar el registro ya que este no tiene un número de cuenta!", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener la cuenta destino.", Me.Name, "BuscadorGenericoDestino_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlBuscadorCuentasBancariasDestino_GotFocus_1(sender As Object, e As RoutedEventArgs)
        If vm.TesoreriaOrdenesPlusRC_Selected.ValorTipoProducto = GSTR_FONDOS_TIPOPRODUCTO Then
            ctlBuscadorCuentasBancariasDestino.TipoItem = "cuentasbancariascartera"
        Else
            ctlBuscadorCuentasBancariasDestino.TipoItem = "cuentasbancariasPLUS"
        End If
    End Sub

    Private Sub ctlSubirArchivo_finalizoSeleccionArchivo(ByVal pstrArchivo As String, ByVal pstrRuta As String, ByVal pabytArchivo As Byte()) Handles ctlSubirArchivo.finalizoSeleccionArchivo
        Try
            vm.mstrArchivoConsignacion = pstrArchivo
            vm.mstrRutaConsignacion = pstrRuta

            If String.IsNullOrEmpty(pstrRuta) Then
                '/ Si no se recibe la ruta del archivo se guarda el arreglo de bytes que tiene el contenido del archivo.
                vm.mabytArchivoConsignacion = pabytArchivo
            Else
                '/ Si se recibe la ruta del archivo no se guarda el arreglo de bytes a menos que sea requerido porque consume más memoria y puede ser innecesario su almacenamiento.
                vm.mabytArchivoConsignacion = Nothing
            End If

            vm.TextoCampoSubirArchivoDetalle(False, pstrArchivo)

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener la información del archivo.", Me.Name, "ctlSubirArchivo_finalizoSeleccionArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private Sub ctlSubirArchivo_finalizoTransmisionArchivo(pstrArchivo As String, pstrRuta As String) Handles ctlSubirArchivo.finalizoTransmisionArchivo
        'vm.TraerOrdenes("TERMINOGUARDARNUEVO")
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If

    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus(sender As Object, e As RoutedEventArgs)
        ctlBuscadorCuentasBancariasDestino.TipoItem = vm.TipoCuentasBancarias
    End Sub

    Private Sub TextBlockBanco_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorCuentasBancariasDestino.AbrirBuscador()
    End Sub

    Private Sub btnLimpiarBanco_Click(sender As Object, e As RoutedEventArgs)
        Try
            vm.strDescripcionCuentaConsignacion = String.Empty
            vm.strCuentaConsignacion = String.Empty
            vm.strTipoCuentaConsignacion = String.Empty
            vm.strTipoCuentaConsignacion = String.Empty
            vm.lngCodigoBancoDestinoConsignacionWpp = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el texto.", _
                                 Me.ToString(), "btnLimpiarConcepto_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TextBlockBanco1_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorBancoNacional.AbrirBuscador()
    End Sub

    Private Sub btnLimpiarBanco1_Click(sender As Object, e As RoutedEventArgs)
        Try
            vm.strDescripcionBancoConsignacion = String.Empty
            vm.lngCodigoBancoConsignacionWpp = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el texto.", _
                                 Me.ToString(), "btnLimpiarBanco1_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

End Class


