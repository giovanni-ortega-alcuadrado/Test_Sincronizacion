Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class GenerarNotasContablesFondosView
    Inherits UserControl
    Dim objVMGenerarNotas As GenerarNotasContablesFondosViewModel
    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = New GenerarNotasContablesFondosViewModel
InitializeComponent()
            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "OperacionesOtrosNegociosView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub GenerarNotasContablesFondosView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado = False Then
                objVMGenerarNotas = CType(Me.DataContext, GenerarNotasContablesFondosViewModel)
                objVMGenerarNotas.viewGenerarNotas = Me

                'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla

                mlogInicializado = True
                objVMGenerarNotas.inicializar()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de otros negocios", Me.Name, "", "GenerarNotasContablesFondosView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMGenerarNotas) Then
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
            'Para el timer de ordenes
            If Not IsNothing(objVMGenerarNotas) Then
                If Not IsNothing(pobjItem) Then
                    If pstrClaseControl = "Compania" Then
                        objVMGenerarNotas.IDCompania = pobjItem.IdItem
                        objVMGenerarNotas.NombreCompania = pobjItem.Nombre

                        objVMGenerarNotas.LimpiarCampos("COMPANIA")
                        objVMGenerarNotas.ConsultarCombosCompania()
                    ElseIf pstrClaseControl = "IDBancoCR" Then
                        objVMGenerarNotas.IDBancoCR = pobjItem.IdItem
                        objVMGenerarNotas.NombreBancoCR = pobjItem.Nombre
                    ElseIf pstrClaseControl = "IDBancoDB" Then
                        objVMGenerarNotas.IDBancoDB = pobjItem.IdItem
                        objVMGenerarNotas.NombreBancoDB = pobjItem.Nombre
                    ElseIf pstrClaseControl = "Nit" Then
                        objVMGenerarNotas.Nit = pobjItem.CodItem
                    ElseIf pstrClaseControl = "CentrosCosto" Then
                        objVMGenerarNotas.CentroCostos = pobjItem.IdItem
                    ElseIf pstrClaseControl = "CodigoOYD" Then
                        objVMGenerarNotas.CodigoOYD = pobjItem.IdItem
                        If objVMGenerarNotas.HabilitarSeleccionNit Then
                            objVMGenerarNotas.Nit = pobjItem.InfoAdicional03
                        End If
                        objVMGenerarNotas.ConsultarCombosEncargos()
                    ElseIf pstrClaseControl = "Concepto" Then
                        objVMGenerarNotas.IDConcepto = pobjItem.IdItem
                        If pobjItem.InfoAdicional07 <> GenerarNotasContablesFondosViewModel.TipoMvtoTesoreria.BB.ToString Then
                            objVMGenerarNotas.CuentaContableDB = pobjItem.InfoAdicional03
                            objVMGenerarNotas.CuentaContableCR = pobjItem.InfoAdicional04
                        Else
                            objVMGenerarNotas.CuentaContableDB = String.Empty
                            objVMGenerarNotas.CuentaContableCR = String.Empty
                        End If
                        
                        objVMGenerarNotas.TipoMovimiento = pobjItem.InfoAdicional07
                        objVMGenerarNotas.DescripcionTipoMovimiento = pobjItem.InfoAdicional08
                        objVMGenerarNotas.Retencion = pobjItem.InfoAdicional09
                        objVMGenerarNotas.DescripcionRetencion = pobjItem.InfoAdicional10
                        objVMGenerarNotas.DetalleConcepto = pobjItem.Nombre
                        'objVMGenerarNotas.Nit = pobjItem.InfoAdicional02

                        If pobjItem.InfoAdicional11 = "1" Then
                            objVMGenerarNotas.ManejaDiferido = True
                        Else
                            objVMGenerarNotas.ManejaDiferido = False
                        End If

                        objVMGenerarNotas.TipoMovimientoDiferido = pobjItem.InfoAdicional12
                        objVMGenerarNotas.DescripcionTipoMovimientoDiferido = pobjItem.InfoAdicional13

                        If pobjItem.InfoAdicional12 <> GenerarNotasContablesFondosViewModel.TipoMvtoTesoreria.BB.ToString Then
                            objVMGenerarNotas.CuentaContableCRDiferido = pobjItem.InfoAdicional14
                            objVMGenerarNotas.CuentaContableDBDiferido = pobjItem.InfoAdicional15
                        Else
                            objVMGenerarNotas.CuentaContableCRDiferido = String.Empty
                            objVMGenerarNotas.CuentaContableDBDiferido = String.Empty
                        End If

                        objVMGenerarNotas.TipoNitDB = pobjItem.InfoAdicional16
                        objVMGenerarNotas.TipoNitCR = pobjItem.InfoAdicional17

                        If objVMGenerarNotas.TipoNitDB = "TE" Or objVMGenerarNotas.TipoNitCR = "TE" Then
                            objVMGenerarNotas.HabilitarSeleccionNit = True
                        Else
                            objVMGenerarNotas.HabilitarSeleccionNit = False
                            objVMGenerarNotas.Nit = String.Empty
                        End If

                        objVMGenerarNotas.ManejaNotaCRDB = IIf(pobjItem.InfoAdicional18 = "1", True, False)
                        objVMGenerarNotas.IDNotaCRDB = pobjItem.InfoAdicional19
                        objVMGenerarNotas.DescripcionNotaCRDB = pobjItem.InfoAdicional20

                        CantidadMaximaParaTextoDigitado(pobjItem.Nombre)
                        objVMGenerarNotas.VerificarHabilitarBancos()
                        objVMGenerarNotas.VerificarHabilitarCodigoOYD()
                        objVMGenerarNotas.VerificarConceptoVSConsecutvio()
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información de la compañia.", Me.Name, "txtCompania_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub CantidadMaximaParaTextoDigitado(ByVal pstrDetalleConcepto As String)
        Try
            If Not String.IsNullOrEmpty(pstrDetalleConcepto) Then
                If Len(pstrDetalleConcepto) >= 77 Then
                    txtConcepto.MaxLength = 0
                Else
                    txtConcepto.MaxLength = 77 - Len(pstrDetalleConcepto)
                End If
            Else
                txtConcepto.MaxLength = 77
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al digitar el concepto.", _
                                               Me.ToString(), "CantidadMaximaParaTextoDigitado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtCompania_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMGenerarNotas) Then
                If CType(sender, A2Utilidades.A2NumericBox).Value > 0 Then
                    objVMGenerarNotas.ConsultarDatosCompania(CType(sender, A2Utilidades.A2NumericBox).Value.ToString)
                Else
                    objVMGenerarNotas.IDCompania = Nothing
                    objVMGenerarNotas.NombreCompania = String.Empty
                    objVMGenerarNotas.LimpiarCampos("COMPANIA")
                    objVMGenerarNotas.ConsultarCombosCompania()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información de la compañia.", Me.Name, "txtCompania_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtBancoCR_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMGenerarNotas) Then
                If CType(sender, A2Utilidades.A2NumericBox).Value > 0 Then
                    objVMGenerarNotas.ConsultarDatosBancoCR(CType(sender, A2Utilidades.A2NumericBox).Value.ToString)
                Else
                    objVMGenerarNotas.IDBancoCR = Nothing
                    objVMGenerarNotas.NombreBancoCR = String.Empty
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información.", Me.Name, "txtBancoCR_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtBancoDB_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMGenerarNotas) Then
                If CType(sender, A2Utilidades.A2NumericBox).Value > 0 Then
                    objVMGenerarNotas.ConsultarDatosBancoDB(CType(sender, A2Utilidades.A2NumericBox).Value.ToString)
                Else
                    objVMGenerarNotas.IDBancoDB = Nothing
                    objVMGenerarNotas.NombreBancoDB = String.Empty
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información.", Me.Name, "txtBancoCR_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtNIT_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMGenerarNotas) Then
                If Not String.IsNullOrEmpty(CType(sender, TextBox).Text) Then
                    objVMGenerarNotas.ConsultarDatosNit(CType(sender, TextBox).Text)
                Else
                    objVMGenerarNotas.Nit = Nothing
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información.", Me.Name, "txtNIT_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCentroCostos_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMGenerarNotas) Then
                If Not String.IsNullOrEmpty(CType(sender, TextBox).Text) Then
                    objVMGenerarNotas.ConsultarDatosCentroCostos(CType(sender, TextBox).Text)
                Else
                    objVMGenerarNotas.CentroCostos = Nothing
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información.", Me.Name, "txtCentroCostos_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtIDConcepto_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMGenerarNotas) Then
                If CType(sender, A2Utilidades.A2NumericBox).Value > 0 Then
                    objVMGenerarNotas.ConsultarDatosConcepto(CType(sender, A2Utilidades.A2NumericBox).Value.ToString)
                Else
                    objVMGenerarNotas.IDConcepto = Nothing
                    objVMGenerarNotas.CuentaContableDB = String.Empty
                    objVMGenerarNotas.CuentaContableCR = String.Empty
                    objVMGenerarNotas.TipoMovimiento = String.Empty
                    objVMGenerarNotas.DescripcionTipoMovimiento = String.Empty
                    objVMGenerarNotas.Retencion = String.Empty
                    objVMGenerarNotas.DescripcionRetencion = String.Empty
                    objVMGenerarNotas.DetalleConcepto = String.Empty
                    objVMGenerarNotas.viewGenerarNotas.CantidadMaximaParaTextoDigitado(String.Empty)
                    objVMGenerarNotas.VerificarHabilitarBancos(False)
                    objVMGenerarNotas.TipoNitDB = String.Empty
                    objVMGenerarNotas.TipoNitCR = String.Empty
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información.", Me.Name, "txtIDConcepto_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ConfigurarSubirDocumento()
        Try
            ctlSubirArchivo.URLServicioDocumentos = objVMGenerarNotas.URLServicioDocumento
            ctlSubirArchivo.Aplicacion = Program.Aplicacion
            ctlSubirArchivo.Version = Program.VersionAplicacion
            ctlSubirArchivo.SoloCapturarRutaArchivo = "True"
            ctlSubirArchivo.AnchoNombreArchivo = "200"
            ctlSubirArchivo.Modulo = "NF"
            ctlSubirArchivo.Tema = "NotasFondos"
            ctlSubirArchivo.Subtema = ""

            ctlSubirArchivo.TagsBusqueda = "Carta, autorización, documento"
            ctlSubirArchivo.Titulo = "Documento adjunto para la nota contable fondos"
            ctlSubirArchivo.Descripcion = "Servicio documentos Notas Fondos"
            ctlSubirArchivo.UsuarioActivo = Program.Usuario
            ctlSubirArchivo.FiltroArchivos = "Todos los archivos (*.*)|*.*|Texto (*.txt)|*.txt|Excel 2010 (*.xlsx)|*.xlsx|Word 2010 (*.docx)|*.docx|Excel 2003 (.xls)|*.xls|Word 2003 (*.doc)|*.doc"
            ctlSubirArchivo.MostrarLog = "False"
            ctlSubirArchivo.TituloSistema = "Notas fondos"
            ctlSubirArchivo.TextoBotonSubirArchivo = "Cargar archivo"

            Dim logMensaje As Boolean = False
            If Not String.IsNullOrEmpty(Program.MostrarMensajeLog) Then
                If Program.MostrarMensajeLog = "1" Then
                    logMensaje = True
                End If
            End If

            ctlSubirArchivo.MostrarMensajeFinalizacion = logMensaje
            ctlSubirArchivo.MostrarNombreArchivo = True
            ctlSubirArchivo.MostrarLog = logMensaje
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al configurar el subir documentos.", Me.Name, "ConfigurarSubirDocumento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlSubirArchivo_finalizoTransmisionArchivo(pstrArchivo As String, pstrRuta As String) Handles ctlSubirArchivo.finalizoTransmisionArchivo

    End Sub

    Private Sub ctlSubirArchivo_finalizoSeleccionArchivo(ByVal pstrArchivo As String, ByVal pstrRuta As String, ByVal pabytArchivo As Byte()) Handles ctlSubirArchivo.finalizoSeleccionArchivo
        Try
            objVMGenerarNotas.mstrArchivo = pstrArchivo
            objVMGenerarNotas.mstrRuta = pstrRuta

            If String.IsNullOrEmpty(pstrRuta) Then
                '/ Si no se recibe la ruta del archivo se guarda el arreglo de bytes que tiene el contenido del archivo.
                objVMGenerarNotas.mabytArchivo = pabytArchivo
            Else
                '/ Si se recibe la ruta del archivo no se guarda el arreglo de bytes a menos que sea requerido porque consume más memoria y puede ser innecesario su almacenamiento.
                objVMGenerarNotas.mabytArchivo = Nothing
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener la información del archivo.", Me.Name, "ctlSubirArchivo_finalizoSeleccionArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCompania_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.LimpiarCompania()
    End Sub

    Private Sub btnLimpiarCodigoOYD_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.LimpiarCodigoOYOD()
    End Sub

    Private Sub btnLimpiarConcepto_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.LimpiarConcepto()
    End Sub

    Private Sub btnLimpiarBancoCR_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.LimpiarBancoCR()
    End Sub

    Private Sub btnLimpiarBancoDB_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.LimpiarBancoDB()
    End Sub

    Private Sub btnLimpiarNit_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.LimpiarNit()
    End Sub

    Private Sub btnLimpiarCentroCosto_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.LimpiarCentroCosto()
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.GenerarNotaContable()
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.LimpiarInformacion()
    End Sub

    Private Sub btnConsultarPendientes_Click(sender As Object, e As RoutedEventArgs)
        objVMGenerarNotas.AbrirConsultarDocumentosPendientes()
    End Sub

    Private Sub txtCodigoOYD_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMGenerarNotas) Then
                If CType(sender, A2Utilidades.A2NumericBox).Value > 0 Then
                    objVMGenerarNotas.ConsultarDatosCodigoOYD(CType(sender, A2Utilidades.A2NumericBox).Value.ToString)
                Else
                    objVMGenerarNotas.LimpiarCodigoOYOD()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información.", Me.Name, "txtCodigoOYD_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
