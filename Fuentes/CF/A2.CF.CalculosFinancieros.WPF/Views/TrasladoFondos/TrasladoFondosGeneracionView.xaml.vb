Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl

Partial Public Class TrasladoFondosGeneracionView
    Inherits Window

#Region "Variables"

    Private mobjVM As TrasladoFondosViewModel
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista
    Dim objBuscadorConceptos As A2ComunesControl.BuscarConceptoTesoreria

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para la DetalleModalCalculadoraView (aplica los estilos a la pantalla), 
    ''' esta muestra el resultado de la calculadora financiera básica
    ''' </summary>
    ''' <param name="pmobjVM">Requiere el ViewModel para asociarlo a la ventana y consultar el resultado</param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Public Sub New(ByVal pmobjVM As TrasladoFondosViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        mobjVM = pmobjVM
        Me.DataContext = mobjVM

        InitializeComponent()

    End Sub

#End Region

#Region "Métodos para control de eventos"

    Private Async Sub Aceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If mobjVM.ValidarDatosRequeridosGeneracion() Then
                If Await mobjVM.ConfirmarGeneracionTraslados() Then
                    Me.DialogResult = True
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación.", Me.ToString(), "Aceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación.", Me.ToString(), "btnCancelar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region


    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(mobjVM) Then
                If Not IsNothing(pobjItem) Then
                    Select Case pstrClaseControl
                        Case "BANCORC_FIRMA"
                            mobjVM.BANCORC_FIRMA = pobjItem.IdItem
                            mobjVM.DESCRIPCION_BANCORC_FIRMA = pobjItem.Descripcion
                        Case "BANCORC_COMPANIA"
                            mobjVM.BANCORC_COMPANIA = pobjItem.IdItem
                            mobjVM.DESCRIPCION_BANCORC_COMPANIA = pobjItem.Descripcion
                        Case "BANCONC_COMPANIA"
                            mobjVM.BANCONC_COMPANIA = pobjItem.IdItem
                            mobjVM.DESCRIPCION_BANCONC_COMPANIA = pobjItem.Descripcion
                        Case "BANCOCE_FIRMA"
                            mobjVM.BANCOCE_FIRMA = pobjItem.IdItem
                            mobjVM.DESCRIPCION_BANCOCE_FIRMA = pobjItem.Descripcion
                        Case "BANCOCE_COMPANIA"
                            mobjVM.BANCOCE_COMPANIA = pobjItem.IdItem
                            mobjVM.DESCRIPCION_BANCOCE_COMPANIA = pobjItem.Descripcion
                        Case "BANCONC_COMPANIA"
                            mobjVM.BANCONC_COMPANIA = pobjItem.IdItem
                            mobjVM.DESCRIPCION_BANCONC_COMPANIA = pobjItem.Descripcion
                    End Select
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del buscador.", Me.ToString(), "BuscadorGenericoListaButon_finalizoBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnIDCONCEPTORC_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Dim intIDConcepto As Integer? = Nothing
                If mobjVM.DiccionarioHabilitarCampos.ContainsKey("CONSECUTIVORC_FIRMA") Then
                    If mobjVM.DiccionarioHabilitarCampos("CONSECUTIVORC_FIRMA") = Visibility.Visible Then
                        If String.IsNullOrEmpty(mobjVM.CONSECUTIVORC_FIRMA) Then
                            mostrarMensaje("Debe de seleccionar el consecutivo para consultar los conceptos de caja.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            If Not String.IsNullOrEmpty(mobjVM.IDCONCEPTORC) Then
                                intIDConcepto = CType(mobjVM.IDCONCEPTORC, Integer?)
                            End If
                            objBuscadorConceptos = New A2ComunesControl.BuscarConceptoTesoreria("RC", mobjVM.CONSECUTIVORC_FIRMA, mobjVM.IDCompaniaFirma, intIDConcepto, mobjVM.DESCRIPCION_IDCONCEPTORC)
                            AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConcepto_RC
                            Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                            objBuscadorConceptos.ShowDialog()

                        End If
                    End If
                End If

                If mobjVM.DiccionarioHabilitarCampos.ContainsKey("CONSECUTIVORC_COMPANIA") Then
                    If mobjVM.DiccionarioHabilitarCampos("CONSECUTIVORC_COMPANIA") = Visibility.Visible Then
                        If String.IsNullOrEmpty(mobjVM.CONSECUTIVORC_COMPANIA) Then
                            mostrarMensaje("Debe de seleccionar el consecutivo para consultar los conceptos de caja.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            If Not String.IsNullOrEmpty(mobjVM.IDCONCEPTORC) Then
                                intIDConcepto = CType(mobjVM.IDCONCEPTORC, Integer?)
                            End If
                            objBuscadorConceptos = New A2ComunesControl.BuscarConceptoTesoreria("RC", mobjVM.CONSECUTIVORC_COMPANIA, mobjVM.IDCompania, intIDConcepto, mobjVM.DESCRIPCION_IDCONCEPTORC)
                            AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConcepto_RC
                            Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                            objBuscadorConceptos.ShowDialog()

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del buscador.", Me.ToString(), "btnIDCONCEPTORC_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnIDCONCEPTOCE_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Dim intIDConcepto As Integer? = Nothing
                If mobjVM.DiccionarioHabilitarCampos.ContainsKey("CONSECUTIVOCE_FIRMA") Then
                    If mobjVM.DiccionarioHabilitarCampos("CONSECUTIVOCE_FIRMA") = Visibility.Visible Then
                        If String.IsNullOrEmpty(mobjVM.CONSECUTIVOCE_FIRMA) Then
                            mostrarMensaje("Debe de seleccionar el consecutivo para consultar los conceptos de egreso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            If Not String.IsNullOrEmpty(mobjVM.IDCONCEPTOCE) Then
                                intIDConcepto = CType(mobjVM.IDCONCEPTOCE, Integer?)
                            End If
                            objBuscadorConceptos = New A2ComunesControl.BuscarConceptoTesoreria("CE", mobjVM.CONSECUTIVOCE_FIRMA, mobjVM.IDCompaniaFirma, intIDConcepto, mobjVM.DESCRIPCION_IDCONCEPTOCE)
                            AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConcepto_CE
                            Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                            objBuscadorConceptos.ShowDialog()

                        End If
                    End If
                End If

                If mobjVM.DiccionarioHabilitarCampos.ContainsKey("CONSECUTIVOCE_COMPANIA") Then
                    If mobjVM.DiccionarioHabilitarCampos("CONSECUTIVOCE_COMPANIA") = Visibility.Visible Then
                        If String.IsNullOrEmpty(mobjVM.CONSECUTIVOCE_COMPANIA) Then
                            mostrarMensaje("Debe de seleccionar el consecutivo para consultar los conceptos de egreso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            If Not String.IsNullOrEmpty(mobjVM.IDCONCEPTOCE) Then
                                intIDConcepto = CType(mobjVM.IDCONCEPTOCE, Integer?)
                            End If
                            objBuscadorConceptos = New A2ComunesControl.BuscarConceptoTesoreria("CE", mobjVM.CONSECUTIVOCE_COMPANIA, mobjVM.IDCompania, intIDConcepto, mobjVM.DESCRIPCION_IDCONCEPTOCE)
                            AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConcepto_CE
                            Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                            objBuscadorConceptos.ShowDialog()

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del buscador.", Me.ToString(), "btnIDCONCEPTOCE_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnIDCONCEPTONC_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Dim intIDConcepto As Integer? = Nothing

                If mobjVM.DiccionarioHabilitarCampos.ContainsKey("CONSECUTIVONC_FIRMA") Then
                    If mobjVM.DiccionarioHabilitarCampos("CONSECUTIVONC_FIRMA") = Visibility.Visible Then
                        If String.IsNullOrEmpty(mobjVM.CONSECUTIVONC_FIRMA) Then
                            mostrarMensaje("Debe de seleccionar el consecutivo para consultar los conceptos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            If Not String.IsNullOrEmpty(mobjVM.IDCONCEPTONC) Then
                                intIDConcepto = CType(mobjVM.IDCONCEPTONC, Integer?)
                            End If
                            objBuscadorConceptos = New A2ComunesControl.BuscarConceptoTesoreria("N", mobjVM.CONSECUTIVONC_FIRMA, mobjVM.IDCompaniaFirma, intIDConcepto, mobjVM.DESCRIPCION_IDCONCEPTONC, False, False, False, True, True, False)
                            AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConcepto_NC
                            Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                            objBuscadorConceptos.ShowDialog()

                        End If
                    End If
                End If

                If mobjVM.DiccionarioHabilitarCampos.ContainsKey("CONSECUTIVONC_COMPANIA") Then
                    If mobjVM.DiccionarioHabilitarCampos("CONSECUTIVONC_COMPANIA") = Visibility.Visible Then
                        If String.IsNullOrEmpty(mobjVM.CONSECUTIVONC_COMPANIA) Then
                            mostrarMensaje("Debe de seleccionar el consecutivo para consultar los conceptos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Else
                            If Not String.IsNullOrEmpty(mobjVM.IDCONCEPTONC) Then
                                intIDConcepto = CType(mobjVM.IDCONCEPTONC, Integer?)
                            End If
                            objBuscadorConceptos = New A2ComunesControl.BuscarConceptoTesoreria("N", mobjVM.CONSECUTIVONC_COMPANIA, mobjVM.IDCompania, intIDConcepto, mobjVM.DESCRIPCION_IDCONCEPTONC, False, False, False, True, True, False)
                            AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConcepto_NC
                            Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
                            objBuscadorConceptos.ShowDialog()

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del buscador.", Me.ToString(), "btnIDCONCEPTOCE_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCENTROCOSTO_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("CentrosCosto", "Centros de costo", "CentrosCosto", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, "", "", "")
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
            mobjBuscadorLst.ShowDialog()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar del centro de costos.", Me.ToString(), "btnCENTROCOSTO_Click", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoSeleccionarConcepto_RC(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2ComunesControl.BuscarConceptoTesoreria = CType(sender, A2ComunesControl.BuscarConceptoTesoreria)
            If CBool(objResultado.DialogResult) Then
                mobjVM.IDCONCEPTORC = CStr(objResultado.IDConcepto)
                mobjVM.DESCRIPCION_IDCONCEPTORC = objResultado.DetalleConceptoCompleto
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el concepto.", Me.ToString(), "TerminoSeleccionarConcepto_RC", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoSeleccionarConcepto_CE(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2ComunesControl.BuscarConceptoTesoreria = CType(sender, A2ComunesControl.BuscarConceptoTesoreria)
            If CBool(objResultado.DialogResult) Then
                mobjVM.IDCONCEPTOCE = CStr(objResultado.IDConcepto)
                mobjVM.DESCRIPCION_IDCONCEPTOCE = objResultado.DetalleConceptoCompleto
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el concepto.", Me.ToString(), "TerminoSeleccionarConcepto_CE", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoSeleccionarConcepto_NC(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2ComunesControl.BuscarConceptoTesoreria = CType(sender, A2ComunesControl.BuscarConceptoTesoreria)
            If CBool(objResultado.DialogResult) Then
                mobjVM.IDCONCEPTONC = CStr(objResultado.IDConcepto)
                mobjVM.DESCRIPCION_IDCONCEPTONC = objResultado.DetalleConceptoCompleto
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el concepto.", Me.ToString(), "TerminoSeleccionarConcepto_CE", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
        Try
            If Not mobjBuscadorLst.ItemSeleccionado Is Nothing Then
                Select Case mobjBuscadorLst.CampoBusqueda
                    Case "CentrosCosto"
                        mobjVM.CENTROCOSTO = mobjBuscadorLst.ItemSeleccionado.IdItem
                        mobjVM.DESCRIPCION_CENTROCOSTO = mobjBuscadorLst.ItemSeleccionado.Nombre
                    Case Else
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la información del centro de costo", Me.ToString(), "mobjBuscadorLst_Closed", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub



End Class
