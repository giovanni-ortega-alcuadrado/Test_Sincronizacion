Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OYD.OYDServer.RIA.Web


Partial Public Class IdentificacionAportesView
    Inherits UserControl
    Dim objVMIdentificacionAportes As IdentificacionAportesViewModel
    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False
    Dim logMostrarMensajeCuentas As Boolean = True

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = New IdentificacionAportesViewModel
InitializeComponent()

            'If Me.Resources.Contains("VMIdentificacionAportes") Then
            '    Me.Resources.Remove("VMIdentificacionAportes")
            'End If

            'Me.Resources.Add("VMIdentificacionAportes", objVMIdentificacionAportes)

            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "OperacionesOtrosNegociosView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub IdentificacionAportesView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado = False Then
                objVMIdentificacionAportes = CType(Me.DataContext, IdentificacionAportesViewModel)

                'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla

                mlogInicializado = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de otros negocios", Me.Name, "", "OtrosNegocios_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMIdentificacionAportes) Then
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de operaciones.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMIdentificacionAportes) Then
                If Not IsNothing(pobjItem) Then
                    If pstrClaseControl = "Compania" Then
                        objVMIdentificacionAportes.IDCompania = CInt(pobjItem.IdItem)
                        objVMIdentificacionAportes.NombreCompania = pobjItem.Nombre
                    ElseIf pstrClaseControl = "IDBanco" Then
                        objVMIdentificacionAportes.IDBanco = CInt(pobjItem.IdItem)
                        objVMIdentificacionAportes.NombreBanco = pobjItem.Nombre
                    ElseIf pstrClaseControl = "CentrosCosto" Then
                        objVMIdentificacionAportes.CentroCostos = pobjItem.IdItem
                    ElseIf pstrClaseControl = "NroEncargo" Then
                        objVMIdentificacionAportes.NroEncargo = pobjItem.CodItem
                        If Not IsNothing(objVMIdentificacionAportes.ListaInformacionMostrar) Then
                            For Each li In objVMIdentificacionAportes.ListaInformacionMostrar
                                If li.logSeleccionado And li.strIDComitente = objVMIdentificacionAportes.IDComitente And String.IsNullOrEmpty(li.strNroEncargo) Then
                                    li.strNroEncargo = pobjItem.CodItem
                                End If
                            Next
                        End If
                    ElseIf pstrClaseControl = "NroEncargoDetalle" Then
                        objVMIdentificacionAportes.InformacionMostrarSeleccionado.strNroEncargo = pobjItem.CodItem
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información de la compañia.", Me.Name, "txtCompania_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                If pstrClaseControl = "IDComitentePrincipal" Then
                    objVMIdentificacionAportes.IDComitente = pobjComitente.IdComitente
                    If Not IsNothing(objVMIdentificacionAportes.ListaInformacionMostrar) Then
                        For Each li In objVMIdentificacionAportes.ListaInformacionMostrar
                            If li.logSeleccionado And String.IsNullOrEmpty(li.strIDComitente) Then
                                li.strIDComitente = pobjComitente.IdComitente
                            End If
                        Next
                    End If
                Else
                    If Not IsNothing(objVMIdentificacionAportes.InformacionMostrarSeleccionado) Then
                        objVMIdentificacionAportes.InformacionMostrarSeleccionado.strIDComitente = pobjComitente.IdComitente
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información del comitente.", Me.Name, "BuscadorClienteListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCompania_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMIdentificacionAportes) Then
                If CType(sender, A2Utilidades.A2NumericBox).Value > 0 Then
                    objVMIdentificacionAportes.ConsultarDatosCompania(CType(sender, A2Utilidades.A2NumericBox).Value.ToString)
                Else
                    objVMIdentificacionAportes.IDCompania = Nothing
                    objVMIdentificacionAportes.NombreCompania = String.Empty
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información de la compañia.", Me.Name, "txtCompania_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtValor_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMIdentificacionAportes) Then
                'objVMIdentificacionAportes.ConsultarDatosCompania(CType(sender, TextBox).Text)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información de la compañia.", Me.Name, "txtCompania_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnConceptoCE_Click(sender As Object, e As RoutedEventArgs)
        Try
            If String.IsNullOrEmpty(objVMIdentificacionAportes.NombreConsecutivo) Then
                mostrarMensaje("Debe de seleccionar el Consecutivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim objBuscadorConceptos As New A2ComunesControl.BuscarConceptoTesoreria("N",
                                                                                        objVMIdentificacionAportes.NombreConsecutivo,
                                                                                        objVMIdentificacionAportes.IDCompania,
                                                                                        objVMIdentificacionAportes.IDConcepto,
                                                                                        objVMIdentificacionAportes.DescripcionConcepto,
                                                                                        False, False, False, True, False, True)
            AddHandler objBuscadorConceptos.Closed, AddressOf TerminoSeleccionarConceptoCE
            Program.Modal_OwnerMainWindowsPrincipal(objBuscadorConceptos)
            objBuscadorConceptos.ShowDialog()


            If logMostrarMensajeCuentas Then
                mostrarMensaje("Recuerde que el tipo movimiento del concepto debe de mover dos cuentas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                logMostrarMensajeCuentas = False
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar los conceptos.", Me.Name, "btnConceptoCE_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoSeleccionarConceptoCE(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2ComunesControl.BuscarConceptoTesoreria = CType(sender, A2ComunesControl.BuscarConceptoTesoreria)
            If CBool(objResultado.DialogResult) Then
                If objResultado.IDTipoMovimiento = "BABA" Or objResultado.IDTipoMovimiento = "BAP" _
                    Or objResultado.IDTipoMovimiento = "PP" Then
                    objVMIdentificacionAportes.IDConcepto = CInt(objResultado.IDConcepto)
                    objVMIdentificacionAportes.DetalleConcepto = objResultado.DetalleConcepto
                    objVMIdentificacionAportes.DescripcionConcepto = objResultado.DetalleConceptoCompleto
                Else
                    mostrarMensaje("El tipo movimiento del concepto debe de mover dos cuentas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar si se refrescan los datos de las órdenes en pantalla", Me.ToString(), "validarRefrescarOrdenes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtBancoCR_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMIdentificacionAportes) Then
                If CType(sender, A2Utilidades.A2NumericBox).Value > 0 Then
                    objVMIdentificacionAportes.ConsultarDatosBanco(CType(sender, A2Utilidades.A2NumericBox).Value.ToString)
                Else
                    objVMIdentificacionAportes.IDBanco = Nothing
                    objVMIdentificacionAportes.NombreBanco = String.Empty
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información del banco.", Me.Name, "txtBancoCR_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtComitenteEncabezado_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMIdentificacionAportes) Then
                If Not String.IsNullOrEmpty(CType(sender, TextBox).Text) Then
                    objVMIdentificacionAportes.ConsultarDatosComitente(CType(sender, TextBox).Text)
                Else
                    objVMIdentificacionAportes.IDComitente = Nothing
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información el comitente.", Me.Name, "txtComitenteEncabezado_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtComitenteDetalle_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMIdentificacionAportes) Then
                If Not String.IsNullOrEmpty(CType(sender, TextBox).Text) Then
                    objVMIdentificacionAportes.ConsultarDatosComitente(CType(sender, TextBox).Text, CInt(CType(sender, TextBox).Tag))
                Else
                    If objVMIdentificacionAportes.ListaInformacionMostrar.Where(Function(i) i.intID = CInt(CType(sender, TextBox).Tag)).Count > 0 Then
                        objVMIdentificacionAportes.ListaInformacionMostrar.Where(Function(i) i.intID = CInt(CType(sender, TextBox).Tag)).First.strIDComitente = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información el comitente.", Me.Name, "txtComitenteEncabezado_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCentroCostos_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMIdentificacionAportes) Then
                If Not String.IsNullOrEmpty(CType(sender, TextBox).Text) Then
                    objVMIdentificacionAportes.ConsultarDatosCentroCosto(CType(sender, TextBox).Text)
                Else
                    objVMIdentificacionAportes.CentroCostos = String.Empty
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información del centro de costo.", Me.Name, "txtCentroCostos_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtNroEncargo_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMIdentificacionAportes) Then
                If Not String.IsNullOrEmpty(CType(sender, TextBox).Text) Then
                    objVMIdentificacionAportes.ConsultarDatosEncargo(CType(sender, TextBox).Text)
                Else
                    objVMIdentificacionAportes.NroEncargo = Nothing
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información el encargo.", Me.Name, "txtNroEncargo_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCompania_Click(sender As Object, e As RoutedEventArgs)
        objVMIdentificacionAportes.LimpiarCompania()
    End Sub

Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        objVMIdentificacionAportes.ConsultarPendientesGenerar()
    End Sub

Private Sub btnLimpiarBanco_Click(sender As Object, e As RoutedEventArgs)
        objVMIdentificacionAportes.LimpiarBanco()
    End Sub

Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        objVMIdentificacionAportes.ConsultarDatos()
    End Sub

Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        objVMIdentificacionAportes.Generar()
    End Sub

Private Sub txtNroEncargoDetalle_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMIdentificacionAportes) Then
                If Not String.IsNullOrEmpty(CType(sender, TextBox).Text) Then
                    objVMIdentificacionAportes.ConsultarDatosEncargo(CType(sender, TextBox).Text, objVMIdentificacionAportes.InformacionMostrarSeleccionado.strIDComitente, objVMIdentificacionAportes.InformacionMostrarSeleccionado.intID)
                Else
                    If objVMIdentificacionAportes.ListaInformacionMostrar.Where(Function(i) i.intID = CInt(CType(sender, TextBox).Tag)).Count > 0 Then
                        objVMIdentificacionAportes.ListaInformacionMostrar.Where(Function(i) i.intID = CInt(CType(sender, TextBox).Tag)).First.strNroEncargo = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información el encargo.", Me.Name, "txtNroEncargoDetalle_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
