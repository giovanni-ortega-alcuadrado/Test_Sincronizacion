Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class CargaMasivaTeImportar_RecibosView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista
    Dim objVMTesoreria As CargaMasivaTesoreriaViewModel

    Public Sub New(ByVal pobjVMTesoreria As CargaMasivaTesoreriaViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMTesoreria = pobjVMTesoreria

            If Me.Resources.Contains("VMTesoreria") Then
                Me.Resources.Remove("VMTesoreria")
            End If

            Me.Resources.Add("VMTesoreria", objVMTesoreria)
            InitializeComponent()

            objVMTesoreria.ConsultarCantidadProcesadas(String.Empty)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "CargaMasivaTeImportar_RecibosView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub


#Region "Eventos"

    Private Sub txtComitente_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            ctlBuscadorClientes.AbrirBuscador()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtComitente_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtComitenteD_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMTesoreria.DiccionarioEdicionCampos("IdComitente") Then
                ctlBuscadorClientesD.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtComitenteD_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCuentaContable_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMTesoreria.DiccionarioEdicionCampos("CuentaContable") Then
                ctlBuscadorCuentaContable.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtCuentaContable_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtNIT_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMTesoreria.DiccionarioEdicionCampos("Nit") Then
                ctlBuscadorNIT.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtNIT_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCentroCosto_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMTesoreria.DiccionarioEdicionCampos("CentroCostos") Then
                ctlBuscadorCentroCosto.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtCentroCosto_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BancoConsignacion_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMTesoreria.DiccionarioEdicionCampos("IdBanco") Then
                ctlBuscadorCheque.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "BancoConsignacion_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Concepto_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMTesoreria.DiccionarioEdicionCampos("IdConcepto") Then
                If Not IsNothing(objVMTesoreria.ImportacionDetalleTeSelected) Then
                    Dim objBuscadorConceptos As New A2ComunesControl.BuscarConceptoTesoreria("RC",
                                                                                                objVMTesoreria.ConsecutivoSeleccionado,
                                                                                                objVMTesoreria.IDCompaniaConsecutivo,
                                                                                                objVMTesoreria.ImportacionDetalleTeSelected.IDConcepto,
                                                                                                objVMTesoreria.ImportacionDetalleTeSelected.Detalle,
                                                                                                False,
                                                                                                False,
                                                                                                False,
                                                                                                False,
                                                                                                True)
                    AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConcepto
                    Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                    objBuscadorConceptos.ShowDialog()

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "BancoConsignacion_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub



    ''' <summary>
    ''' Evento para el manejo de los conceptos de Tesorería.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB20130711</remarks>
    Private Sub Button_Click_BuscadorListaConceptos(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesoreria.ImportacionDetalleTeSelected) Then
                Dim objBuscadorConceptos As New A2ComunesControl.BuscarConceptoTesoreria("RC",
                                                                                            objVMTesoreria.ConsecutivoSeleccionado,
                                                                                            objVMTesoreria.IDCompaniaConsecutivo,
                                                                                            objVMTesoreria.ImportacionDetalleTeSelected.IDConcepto,
                                                                                            objVMTesoreria.ImportacionDetalleTeSelected.Detalle,
                                                                                            False,
                                                                                            False,
                                                                                            False,
                                                                                            False,
                                                                                            True)
                AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConcepto
                Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                objBuscadorConceptos.ShowDialog()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al levantar el buscador de concepto", Me.Name, "Button_Click_BuscadorListaConceptos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub TerminoSeleccionarConcepto(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2ComunesControl.BuscarConceptoTesoreria = CType(sender, A2ComunesControl.BuscarConceptoTesoreria)
            If objResultado.DialogResult Then
                objVMTesoreria.ImportacionDetalleTeSelected.IDConcepto = objResultado.IDConcepto
                If objVMTesoreria.DiccionarioEdicionCampos("Detalle") Then
                    objVMTesoreria.ImportacionDetalleTeSelected.Detalle = objResultado.DetalleConceptoCompleto
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar si se refrescan los datos de las órdenes en pantalla", Me.ToString(), "validarRefrescarOrdenes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region
#Region "Buscadores Tesoreria"

    Private Sub Buscar_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then

                objVMTesoreria.ImportacionEncabezadoTeSelected.IdComitente = pobjComitente.IdComitente
                If objVMTesoreria.DiccionarioEdicionCampos("Nombre") Then
                    objVMTesoreria.ImportacionEncabezadoTeSelected.Nombre = pobjComitente.Nombre
                End If
                If objVMTesoreria.DiccionarioEdicionCampos("TipoIdentificacion") Then
                    objVMTesoreria.ImportacionEncabezadoTeSelected.TipoIdentificacion = pobjComitente.CodTipoIdentificacion
                End If
                If objVMTesoreria.DiccionarioEdicionCampos("NroDocumento") Then
                    objVMTesoreria.ImportacionEncabezadoTeSelected.NroDocumento = pobjComitente.NroDocumento
                End If

                objVMTesoreria.ConsultarCuentasContablesCliente(pobjComitente.IdComitente)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener el resultado del comitente.", Me.ToString(), "Buscar_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Buscar_finalizoBusquedaD(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                objVMTesoreria.ImportacionDetalleTeSelected.IDComitente = pobjComitente.IdComitente
                objVMTesoreria.ImportacionDetalleTeSelected.Nombre = pobjComitente.Nombre
                If objVMTesoreria.DiccionarioEdicionCampos("Nit") Then
                    If pobjComitente.TipoIdentificacion = "N" Then
                        objVMTesoreria.ImportacionDetalleTeSelected.NIT = pobjComitente.NroDocumento.Substring(0, pobjComitente.NroDocumento.Length - 1)
                    Else
                        objVMTesoreria.ImportacionDetalleTeSelected.NIT = pobjComitente.NroDocumento
                    End If
                End If
                If objVMTesoreria.DiccionarioEdicionCampos("CuentaContable") Then
                    If String.IsNullOrEmpty(objVMTesoreria.ImportacionDetalleTeSelected.IDCuentaContable) Then
                        objVMTesoreria.ImportacionDetalleTeSelected.IDCuentaContable = objVMTesoreria.strCuentaContableClientes
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener el resultado del comitente.", Me.ToString(), "Buscar_finalizoBusquedaD", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "IDBanco"
                    objVMTesoreria.ImportacionEncabezadoTeSelected.IDBanco = pobjItem.IdItem
                    objVMTesoreria.ImportacionEncabezadoTeSelected.NombreBco = pobjItem.Nombre
                    objVMTesoreria.ImportacionEncabezadoTeSelected.ChequeraAutomatica = pobjItem.Estado
                    objVMTesoreria.ImportacionEncabezadoTeSelected.CompaniaBanco = pobjItem.InfoAdicional03 'SV20160203
                Case "IDBancoCheque"
                    objVMTesoreria.ImportacionDetalleChequesTeSelected.BancoConsignacion = pobjItem.IdItem
                Case "CentrosCosto"
                    objVMTesoreria.ImportacionDetalleTeSelected.CentroCosto = pobjItem.IdItem
            End Select
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaBancoDestino(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            objVMTesoreria.ImportacionEncabezadoTeSelected.lngBancoConsignacion = pobjItem.IdItem
            objVMTesoreria.ImportacionEncabezadoTeSelected.strBancoDestino = pobjItem.Nombre
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaD(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            objVMTesoreria.ImportacionDetalleTeSelected.IDCuentaContable = pobjItem.IdItem
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaNITS(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            objVMTesoreria.ImportacionDetalleTeSelected.NIT = pobjItem.CodItem
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaSB(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            objVMTesoreria.ImportacionEncabezadoTeSelected.IdSucursalBancaria = pobjItem.IdItem
            objVMTesoreria.ImportacionEncabezadoTeSelected.SucursalBancaria = pobjItem.Nombre
        End If
    End Sub

    Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
        If Not mobjBuscadorLst.ItemSeleccionado Is Nothing Then
            Select Case mobjBuscadorLst.CampoBusqueda.ToLower
                Case "conceptoteso"
                    objVMTesoreria.ImportacionDetalleTeSelected.IDConcepto = mobjBuscadorLst.ItemSeleccionado.IdItem
                    objVMTesoreria.ImportacionDetalleTeSelected.Detalle = mobjBuscadorLst.ItemSeleccionado.Nombre
                Case "nombresucursal"
                    CType(Me.DataContext, TesoreriaViewModel).ChequeTesoreriSelected.SucursalesBancolombia = mobjBuscadorLst.ItemSeleccionado.IdItem
                Case Else
            End Select
        End If
    End Sub

#End Region

End Class
