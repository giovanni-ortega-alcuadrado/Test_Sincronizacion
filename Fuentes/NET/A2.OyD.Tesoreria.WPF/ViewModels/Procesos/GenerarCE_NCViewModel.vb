Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Sebastian Londoño Benitez
'Archivo: Public Class GenerarCE_NCViewModel.vb
'Propiedad de Alcuadrado S.A. 2013

Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports A2Utilidades.Mensajes
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Threading.Tasks
Imports A2ComunesControl
Imports A2ComunesImportaciones
Imports System.Web
Imports System.Collections.ObjectModel

Public Class GenerarCE_NCViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As TesoreriaDomainContext
    Dim dcProxy1 As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim dcArchivosProxy As ImportacionesDomainContext
    Dim FechaCierre As DateTime
    Dim _mstrValidacion_NroIdentificacion_ACH As String = "NO"
    Private Const BYT_FORMAPAGOCHEQUE As String = "C"
    Private Const BYT_FORMAPAGOTRANSFERENCIA As String = "T"
    Private Const BYT_FORMAPAGO_PAGOXREDBANCO As String = "B"
    Private Const BYT_FORMAPAGO_TRANSFERENCIACARTERASCOLECTIVAS As String = "L"
    Public Const NOTA_DEBITO As String = "ND"
    Public Const NOTA_CREDITO As String = "NC"
    Private Const BYT_TIPOCARTERA_RENTALIQUIDEZ As String = "1"

    Dim mstrNombreConsecutivoNota As String
    Dim mstrctacontableMGF, mstrccostosMGF, mstrctacontablecontrapMGF, mstrccostocontrapMGF As String
    Dim mdblGMFInferior, msnGMF, dblvalortotalConGMF As Double
    Dim strservidor, strbasedatos, strowner, strcia, strcuenta, mstrCtacontraparteNotasCXC As String
    Dim mstrCtaContableTrasladoDB, mstrCtaContableTrasladoCR, mstrTipoNotasCxC As String
    Dim mstrCtaContableClientes As String
    Dim strReportar As String = String.Empty
    Public Const CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO = "ArchivoPlanoACH_DECEVAL"
    Dim strFormato As String
    Dim strBorrar As String = String.Empty

    Dim NroRegistros As Integer
    Dim Mensaje As String
    Dim NroAccionesGen As Integer
    Dim NroRentaGen As Integer

    Dim strGMFDividendos As String 'JAG 20141015
    Dim logUtilizaCuentasTrasladoGMF As Boolean = False
    Dim logUtilizaCuentasNotasCobroGMF As Boolean = False
    Dim logUtilizaTransferenciaACHBancolombia As Boolean = False
    Dim logUtilizaPasivaA2 As Boolean = False
    Dim objServicios As A2VisorReportes.A2.Visor.Servicios.GeneralesClient
    Public Const CSTR_NOMBREPROCESO_CSVREPORTE = "GeneracionCSVReportes"
    Dim logGMFCOMPENSACION_VB As Boolean = False

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New TesoreriaDomainContext()
            dcProxy1 = New TesoreriaDomainContext()
            objProxy = New UtilidadesDomainContext()
            dcArchivosProxy = New ImportacionesDomainContext()
        Else
            dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            dcArchivosProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 3600) 'DEMC20191002 SE AUMENTA EL TIMEOUT A 60 MINUTOS.

        clsEstablecerComunicacionVisorReportes.ServicioGenerales(objServicios, Application.Current.Resources("A2VServicioParam").ToString())

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.VerificarGMFQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoverificarGMF, Nothing)
                dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
                objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")

                'JCM20160219
                'Valoreselected = ParametrosConsultaSelected

                If Not IsNothing(dcProxy.ItemCombos) Then
                    objProxy.ItemCombos.Clear()
                End If

                'JCM20160219
                objProxy.Load(objProxy.cargarCombosEspecificosQuery("PagosDeceval", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCombosPagosDeceval, Nothing)
                objProxy.Verificaparametro("PAGOSDECEVAL_GMF_DIVIDENDOS", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "PAGOSDECEVAL_GMF_DIVIDENDOS") 'JAG 20141015
                objProxy.Verificaparametro("GMFCOMPENSACION_VB", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "GMFCOMPENSACION_VB") 'JAG 20141015
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "LiquidacionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Metodos"

    ''' <summary>
    ''' Método que consulta los documentos de Pagos de Deceval.
    ''' </summary>
    ''' <remarks>SLB20130830</remarks>
    Public Sub ConsultarPagosDeceval(Optional ByVal strAccion As String = " ")
        Try
            IsBusy = True
            dcProxy.PagosDECEVALs.Clear()
            dcProxy.Load(dcProxy.ConsultarPagosDecevalQuery(_ParametrosConsultaSelected.FechaDesde, _ParametrosConsultaSelected.FechaHasta,
                                                            IIf(_ParametrosConsultaSelected.NombreConsecutivoRC = "-1", Nothing, _ParametrosConsultaSelected.NombreConsecutivoRC),
                                                            _ParametrosConsultaSelected.NroRC, _ParametrosConsultaSelected.TipoTesoreria, _ParametrosConsultaSelected.FormaPagoCE,
                                                            Program.Usuario, _ParametrosConsultaSelected.Democratizados, _ParametrosConsultaSelected.IDEspecie,
                                                            _ParametrosConsultaSelected.TipoCartera, _ParametrosConsultaSelected.TipoACH, _ParametrosConsultaSelected.TipoCheque,
                                                            _ParametrosConsultaSelected.TipoEspecie, _ParametrosConsultaSelected.logExento, Program.HashConexion), AddressOf TerminoConsultarPagosDeceval, strAccion)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ConsultarUltimosValores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub EjecutarConsulta()
        Try
            'SLB20131108 Se genera el informe desde el Visor de Reportes
            IsBusy = True

            Dim strParametros As String
            Dim strExento As String

            'JFSB 20170221 Se controla el registro del campo exento
            If IsNothing(_ParametrosConsultaSelected.logExento) Then
                strExento = "NULL"
            ElseIf _ParametrosConsultaSelected.logExento = True Then
                strExento = "1"
            Else
                strExento = "0"
            End If

            strParametros = " '@pdtmConsecutivoRC'**'" & IIf(_ParametrosConsultaSelected.NombreConsecutivoRC = "-1", Nothing, _ParametrosConsultaSelected.NombreConsecutivoRC) & "'**'STRING'|" _
                             & " '@intNroRC'**'" & IIf(_ParametrosConsultaSelected.NroRC Is Nothing, "NULL", _ParametrosConsultaSelected.NroRC) & "'**'INTEGER'|" _
                             & " '@plogDividendos'**'" & IIf(_ParametrosConsultaSelected.TipoTesoreria = True, 1, 0) & "'**'BOOLEAN'|" _
                             & " '@pstrFormaPago'**'" & _ParametrosConsultaSelected.FormaPagoCE & "'**'STRING'|" _
                             & " '@pstrUsuario'**'" & Program.Usuario & "'**'STRING'|" _
                             & " '@plogDemocratizados'**'" & IIf(_ParametrosConsultaSelected.Democratizados = True, 1, 0) & "'**'BOOLEAN'|" _
                             & " '@pstrIDEspecie'**'" & IIf(_ParametrosConsultaSelected.IDEspecie Is Nothing, "NULL", _ParametrosConsultaSelected.IDEspecie) & "'**'STRING'|" _
                             & " '@pstrTipoCartera'**'" & IIf(_ParametrosConsultaSelected.TipoCartera Is Nothing, "NULL", _ParametrosConsultaSelected.TipoCartera) & "'**'STRING'|" _
                             & " '@pstrTipoBanco'**'" & IIf(_ParametrosConsultaSelected.TipoACH Is Nothing, "NULL", _ParametrosConsultaSelected.TipoACH) & "'**'STRING'|" _
                             & " '@pstrTipoCheque'**'" & IIf(_ParametrosConsultaSelected.TipoCheque Is Nothing, "NULL", _ParametrosConsultaSelected.TipoCheque) & "'**'STRING'|" _
                             & " '@pstrTipoEspecie'**'" & _ParametrosConsultaSelected.TipoEspecie & "'**'STRING'|" _
                             & " '@pstrOpcion'**'EXCEL'**'STRING'|" _
                             & " '@plogExento'**'" & strExento & "'**'BOOLEAN'|" _
                             & " '@pstrFechaElaboracionRCDesde'**'" & _ParametrosConsultaSelected.FechaDesde.ToString("yyyy-MM-dd") & "'**'STRING'|" _
                             & " '@pstrFechaElaboracionRCHasta'**'" & _ParametrosConsultaSelected.FechaHasta.ToString("yyyy-MM-dd") & "'**'STRING'|"


            Dim objRepuestaServicio = objServicios.GenerarArchivo("Saldar pagos DECEVAL", Program.Usuario, Application.Current.Resources("A2RutaFisicaGeneracion").ToString, strParametros, "usp_OyD_DetalleReciboCaja_PagosDECEVAL_Consultar_OyDNet", Program.Maquina, A2VisorReportes.A2.Visor.Servicios.TipoArchivo.EXCEL.EXCEL, "Saldar pagos DECEVAL", Program.HashConexion("Saldar pagos DECEVAL"))
            TerminoGenerarArchivo(objRepuestaServicio)

        Catch ex As Exception
            IsBusy = False      ' JFSB 20170221 Se controla la habilitación de la pantalla
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al ejecutar la consulta.",
                                 Me.ToString(), "ReporteRecibosdeCajaViewModel.EjecutarConsulta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoGenerarArchivo(ByVal e As A2VisorReportes.A2.Visor.Servicios.RespuestaArchivo)
        Try
            IsBusy = False
            If e.EjecucionExitosa Then
                TerminoCrearArchivo()
            Else
                Dim ex1 As New Exception(e.Mensaje)
                A2Utilidades.Mensajes.mostrarMensaje("Se presentó un error en el momento de generar el archivo plano." & ex1.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error en el momento de generar el archivo plano.",
                                 Me.ToString(), "TerminoGenerarArchivoPlano", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCrearArchivo()
        Try
            Dim cwCar As New A2ComunesImportaciones.ListarArchivosDirectorioView(CSTR_NOMBREPROCESO_CSVREPORTE) '(CSTR_NOMBREPROCESO_RECIBOSDECAJA)
            Program.Modal_OwnerMainWindowsPrincipal(cwCar)
            cwCar.ShowDialog()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al levantar la ventana de visualización de los archivos",
                                 Me.ToString(), "TerminoCrearArchivo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    ''' <summary>
    ''' Método generaración los Comprobantes de Egreso o Notas de Tesorería.
    ''' </summary>
    ''' <remarks>SLB20130902</remarks>
    Public Async Sub GenerarCE()
        Try
            If validaFechaCierre("Actualizar") Then
                IsBusy = True
                If Await Validaciones() Then
                    mostrarMensajePregunta("Está seguro de generar " & IIf(_ParametrosConsultaSelected.TipoTesoreria, "Comprobante de Egreso", "Notas de Tesorería"),
                                            Program.TituloSistema,
                                            "ULTIMOSVALORES",
                                            AddressOf TerminoPregunta, True, "¿Generar?")
                End If
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método de las validaciones para generar los Comprobantes de Egreso o Notas de Tesorería.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Async Function Validaciones() As Task(Of Boolean)
        Dim logElegir As Boolean
        Dim logEsRentaLiquidez As Boolean = False
        Dim logEsTransferenciaElectronica As Boolean = False

        If _ParametrosConsultaSelected.MostrarCamposACH = Visibility.Visible And _ParametrosConsultaSelected.FormaPagoCE = BYT_FORMAPAGO_TRANSFERENCIACARTERASCOLECTIVAS Then
            logEsRentaLiquidez = True
        End If

        If _ParametrosConsultaSelected.FormaPagoCE = BYT_FORMAPAGOTRANSFERENCIA Then
            logEsTransferenciaElectronica = True
        End If

        Dim logValidaciones As Boolean = True

        If _ParametrosConsultaSelected.TipoTesoreria Then
            If String.IsNullOrEmpty(_ParametrosConsultaSelected.FormaPagoCE) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe elegir la forma de pago de los comprobantes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidaciones = False
                Return logValidaciones
            End If
        End If

        If String.IsNullOrEmpty(_ParametrosConsultaSelected.NombreConsecutivo) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe elegir un consecutivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidaciones = False
            Return logValidaciones
        End If

        If IsNothing(_ParametrosConsultaSelected.NroBanco) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe elegir un banco.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidaciones = False
            Return logValidaciones
        End If

        If String.IsNullOrEmpty(_ParametrosConsultaSelected.CuentaContable) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe elegir la cuenta contable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidaciones = False
            Return logValidaciones
        End If

        If _ParametrosConsultaSelected.TipoTesoreria Then
            If _ParametrosConsultaSelected.FormaPagoCE <> "C" And _ParametrosConsultaSelected.MostrarCamposACH = Visibility.Visible Then

                If _ParametrosConsultaSelected.HabilitarSeleccionACH Then
                    If String.IsNullOrEmpty(_ParametrosConsultaSelected.FormatoACH) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un formato ACH.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logValidaciones = False
                        Return logValidaciones
                    End If
                End If

                If _ParametrosConsultaSelected.HabilitarSeleccionRutaArchivoPlano Then
                    If String.IsNullOrEmpty(_ParametrosConsultaSelected.RutaArchivoPlano) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe definir el nombre del archivo de ACH.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logValidaciones = False
                        Return logValidaciones
                    End If

                    If Not _ParametrosConsultaSelected._mlogValidoNombrePlano Then
                        A2Utilidades.Mensajes.mostrarMensaje("El nombre del archivo plano posee caractares no válidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logValidaciones = False
                        Return logValidaciones
                    End If
                End If

            ElseIf _ParametrosConsultaSelected.FormaPagoCE = "C" And String.IsNullOrEmpty(_ParametrosConsultaSelected.TipoCheque) Then
                A2Utilidades.Mensajes.mostrarMensaje("Para la forma de pago Cheque debe de seleccionar el tipo de cheque.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidaciones = False
                Return logValidaciones
            End If
        End If

        If _ParametrosConsultaSelected.CobroGMF Then

            If Not ValidarParametrosInstalacionGMF() Then
                logValidaciones = False
                Return logValidaciones
            End If

            'JAG 20141023
            If _ParametrosConsultaSelected.FormaPagoCE = "T" And strGMFDividendos = "SI" Then

                If Not _ParametrosConsultaSelected.BancoTieneGMF Then
                    A2Utilidades.Mensajes.mostrarMensaje("El banco no tiene cobro GMF y se pueden generar comprobantes con cobro de GMF.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _ParametrosConsultaSelected.CobroGMF = False
                    logValidaciones = False
                    Return logValidaciones
                End If
                'JAG 20141023
            ElseIf Not _ParametrosConsultaSelected.BancoTieneGMF Then
                A2Utilidades.Mensajes.mostrarMensaje("El banco no tiene cobro GMF.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                _ParametrosConsultaSelected.CobroGMF = False
                logValidaciones = False
                Return logValidaciones
            End If

        End If

        If IsNothing(ListaPagosDECEVAL) Then
            A2Utilidades.Mensajes.mostrarMensaje("No existen abonos de DECEVAL consultados.  Cambie los filtros de Consulta que le permitan generar " & IIf(_ParametrosConsultaSelected.TipoTesoreria, "Comprobantes de Egreso", "Notas de Tesorería") & ".", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidaciones = False
            Return logValidaciones
        End If

        If (Not IsNothing(ListaPagosDECEVAL)) AndAlso ListaPagosDECEVAL.Count > 0 Then
            For Each objLista In ListaPagosDECEVAL
                If objLista.Seleccionado Then
                    logElegir = True
                    Exit For
                End If
            Next
            If Not logElegir Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe elegir mínimo un registro para generar " & IIf(_ParametrosConsultaSelected.TipoTesoreria, "Comprobantes de Egreso", "Notas de Tesorería") & ".", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidaciones = False
                Return logValidaciones
            End If
        Else
            A2Utilidades.Mensajes.mostrarMensaje("No existen abonos de DECEVAL consultados.  Cambie los filtros de Consulta que le permitan generar " & IIf(_ParametrosConsultaSelected.TipoTesoreria, "Comprobantes de Egreso", "Notas de Tesorería") & ".", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logValidaciones = False
            Return logValidaciones
        End If

        If logEsRentaLiquidez Then
            Dim strRutaArchivoLiquidez = Await ConsultarParametro("RUTAARCHIVO_ACHLIQUIDEZ")

            If Await VerificarRutaServidor(strRutaArchivoLiquidez) = False Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La ruta del parámetro " & "RUTAARCHIVO_ACHLIQUIDEZ" & " no existe o no se tiene acceso. Por favor configure el parámetro por una ruta válida para realizar la generación de los archivos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidaciones = False
                Return logValidaciones
            End If

            strRutaArchivoLiquidez = Await ConsultarParametro("BACKUP_ARCHIVOSPAGOS_PAB")

            If Await VerificarRutaServidor(strRutaArchivoLiquidez) = False Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La ruta del parámetro " & "BACKUP_ARCHIVOSPAGOS_PAB" & " no existe o no se tiene acceso. Por favor configure el parámetro por una ruta válida para realizar la generación de los archivos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidaciones = False
                Return logValidaciones
            End If
        ElseIf logEsTransferenciaElectronica And logGMFCOMPENSACION_VB Then
            Dim strRutaArchivoTransferencia = Await ConsultarParametro("RUTAARCHIVO_TRANSFERENCIA_ELECTRONICA")
            If Await VerificarRutaServidor(strRutaArchivoTransferencia) = False Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La ruta del parámetro " & "RUTAARCHIVO_TRANSFERENCIA_ELECTRONICA" & " no existe o no se tiene acceso. Por favor configure el parámetro por una ruta válida para realizar la generación de los archivos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidaciones = False
                Return logValidaciones
            End If

            Dim strRutaArchivoPab = Await ConsultarParametro("BACKUP_ARCHIVOSPAGOS_PAB")

            If Await VerificarRutaServidor(strRutaArchivoPab) = False Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La ruta del parámetro " & "BACKUP_ARCHIVOSPAGOS_PAB" & " no existe o no se tiene acceso. Por favor configure el parámetro por una ruta válida para realizar la generación de los archivos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logValidaciones = False
                Return logValidaciones
            End If
        End If

        Return logValidaciones

    End Function

    Public Async Function ConsultarParametro(ByVal pstrParametro As String) As Task(Of String)
        Dim strValorParametro As String = String.Empty

        Try
            Dim objRet As InvokeOperation(Of String)

            objRet = Await objProxy.VerificaparametroSync(pstrParametro, Program.Usuario, Program.HashConexion).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el valor.", Me.ToString(), "ConsultarParametro", Program.TituloSistema, Program.Maquina, objRet.Error)
                Else
                    strValorParametro = objRet.Value
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el valor.", Me.ToString(), "ConsultarParametro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return strValorParametro
    End Function

    Public Async Function VerificarRutaServidor(ByVal pstrRutaFisica As String) As Task(Of Boolean)
        Dim logExisteRutaFisica As Boolean = False

        Try
            Dim objRet As InvokeOperation(Of Boolean)

            objRet = Await dcArchivosProxy.VerificarRutaFisicaServidorSync(pstrRutaFisica, Program.Usuario, Program.HashConexion).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar y consultar los registros.", Me.ToString(), "VerificarRutaServidor", Program.TituloSistema, Program.Maquina, objRet.Error)
                Else
                    logExisteRutaFisica = objRet.Value
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el valor.", Me.ToString(), "VerificarRutaServidor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return logExisteRutaFisica
    End Function

    'JCM20160219
    Public Function validarCompanias() As Boolean
        validarCompanias = True

        If logUtilizaPasivaA2 Then
            If Not String.IsNullOrEmpty(_strCompaniaConsecutivoComprobanteNota) Then
                If _strCompaniaConsecutivo <> _strCompaniaConsecutivoComprobanteNota Then
                    If _ParametrosConsultaSelected.TipoTesoreria Then

                        _ParametrosConsultaSelected.NombreConsecutivo = String.Empty
                        _ParametrosConsultaSelected.NroBanco = Nothing
                        _strCompaniaConsecutivoComprobanteNota = String.Empty
                        strCompaniaBanco = String.Empty

                        A2Utilidades.Mensajes.mostrarMensaje("La compañia asociada al consecutivo del recibo no es igual a la compañia asociada al consecutivo del egreso", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                        validarCompanias = False
                        Return validarCompanias
                    Else

                        _ParametrosConsultaSelected.NombreConsecutivo = String.Empty
                        _ParametrosConsultaSelected.NroBanco = Nothing
                        _strCompaniaConsecutivoComprobanteNota = String.Empty
                        strCompaniaBanco = String.Empty

                        A2Utilidades.Mensajes.mostrarMensaje("La compañia asociada al consecutivo del recibo no es igual a la compañia asociada al consecutivo de la nota", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)


                        validarCompanias = False
                        Return validarCompanias
                    End If

                End If
            End If
        End If

    End Function

    ''' <summary>
    ''' Se valida los parametros del GMF, si falta alguno no se puede generar el GMF
    ''' </summary>
    ''' <returns>True - Existen todos los parametros</returns>
    ''' <remarks>SLB20130306</remarks>
    Private Function ValidarParametrosInstalacionGMF() As Boolean
        ValidarParametrosInstalacionGMF = False

        If mstrNombreConsecutivoNota = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado el nombre del consecutivo de la nota GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrctacontableMGF = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrccostosMGF = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado el centro de costos para el GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrctacontablecontrapMGF = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable de la contraparte para el GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If mstrccostocontrapMGF = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado el centro de costos de la contraparte para el GMF en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If logUtilizaCuentasNotasCobroGMF Then
            If mstrTipoNotasCxC = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado el tipo de Notas CxC en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If
            If mstrCtacontraparteNotasCXC = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable de la contraparte para Notas CxC en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If
        End If

        If logUtilizaCuentasTrasladoGMF Then
            If mstrCtaContableTrasladoDB = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable de traslado DB en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            If mstrCtaContableTrasladoCR = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado la cuenta contable de traslado CR en Instalación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If
        End If

        ValidarParametrosInstalacionGMF = True
    End Function

    Private Async Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then

            strBorrar = String.Empty
            strReportar = String.Empty
            If Await GenerarDocumentosTesoreria() Then
                Dim mostrarMensaje As New LogImportacion
                Program.Modal_OwnerMainWindowsPrincipal(mostrarMensaje)
                mostrarMensaje.texto = String.Empty
                mostrarMensaje.texto = strReportar
                mostrarMensaje.ShowDialog()
                logSeleccionarTodos = False
            End If
        End If
    End Sub

    Private Async Function GenerarDocumentosTesoreria() As Task(Of Boolean)
        Dim objRetorno As Boolean = True
        Try
            IsBusy = True
            Dim objResultGenerarDeceval As LoadOperation(Of OyDTesoreria.GeneracionCENCDeceval)
            Dim objResultGenerarAHC As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion)
            Dim strRegistrosEnviar As String = String.Empty
            Dim strCadena As String = String.Empty
            Dim strDocumentosGenerados As String = String.Empty
            Dim objResultInteger As InvokeOperation(Of Integer)
            Dim objResultBoolean As InvokeOperation(Of Boolean)
            Dim logEsRentaLiquidez As Boolean
            Dim logEsTransferenciaElectronica As Boolean = False

            For Each objLista In ListaPagosDECEVAL
                If objLista.Seleccionado Then
                    If String.IsNullOrEmpty(strRegistrosEnviar) Then
                        strRegistrosEnviar = String.Format("{0}**{1}**{2}", objLista.NroRecibo, objLista.NombreConsecutivo, objLista.Secuencia)
                    Else
                        strRegistrosEnviar = String.Format("{0}|{1}**{2}**{3}", strRegistrosEnviar, objLista.NroRecibo, objLista.NombreConsecutivo, objLista.Secuencia)
                    End If
                End If
            Next

            strRegistrosEnviar = System.Web.HttpUtility.UrlEncode(strRegistrosEnviar)

            dcProxy.GeneracionCENCDecevals.Clear()

            objResultGenerarDeceval = Await dcProxy.Load(dcProxy.GenerarComprobanteONotaDebitoDecevalSyncQuery(strRegistrosEnviar,
                                                                                                               IIf(_ParametrosConsultaSelected.MostrarCamposACH = Visibility.Visible, True, False),
                                                                                                               _ParametrosConsultaSelected.FormaPagoCE,
                                                                                                               _ParametrosConsultaSelected.TipoTesoreria,
                                                                                                               _ParametrosConsultaSelected.NombreConsecutivo,
                                                                                                               _ParametrosConsultaSelected.NroBanco,
                                                                                                               _ParametrosConsultaSelected.FechaElaboracion,
                                                                                                               _ParametrosConsultaSelected.TipoCheque,
                                                                                                               _ParametrosConsultaSelected.CobroGMF,
                                                                                                               _ParametrosConsultaSelected.CuentaContable,
                                                                                                               _ParametrosConsultaSelected.logExento,
                                                                                                               Program.Usuario,
                                                                                                               Program.Maquina,
                                                                                                               Program.HashConexion)).AsTask
            If objResultGenerarDeceval.HasError Then
                If objResultGenerarDeceval.Error Is Nothing Then
                    A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "InsertarCEDetalle", Program.TituloSistema, Program.Maquina, objResultGenerarDeceval.Error)
                End If
                objResultGenerarDeceval.MarkErrorAsHandled()
                Return False
            Else
                Dim objRespuestaGeneracion = objResultGenerarDeceval.Entities.ToList

                For Each li In objRespuestaGeneracion
                    If String.IsNullOrEmpty(strReportar) Then
                        strReportar = li.strMensaje
                    Else
                        strReportar = strReportar & vbCr & li.strMensaje
                    End If

                    If String.IsNullOrEmpty(strCadena) Then
                        If Not String.IsNullOrEmpty(li.strDatosACH) Then
                            strCadena = li.strDatosACH
                        End If
                    End If

                    If String.IsNullOrEmpty(strDocumentosGenerados) Then
                        If Not String.IsNullOrEmpty(li.strDocumentosGenerados) Then
                            strDocumentosGenerados = strDocumentosGenerados + li.strDocumentosGenerados
                        End If
                    End If
                Next
            End If

            If _ParametrosConsultaSelected.MostrarCamposACH = Visibility.Visible And _ParametrosConsultaSelected.FormaPagoCE = BYT_FORMAPAGO_TRANSFERENCIACARTERASCOLECTIVAS Then
                logEsRentaLiquidez = True
            End If

            If _ParametrosConsultaSelected.FormaPagoCE = BYT_FORMAPAGOTRANSFERENCIA Then
                logEsTransferenciaElectronica = True
            End If

            If Not String.IsNullOrEmpty(strDocumentosGenerados) Then
                strDocumentosGenerados = System.Web.HttpUtility.UrlEncode(strDocumentosGenerados)

                If logEsRentaLiquidez Then
                    If _ParametrosConsultaSelected.TipoCartera = "1" Then
                        strReportar = strReportar & vbCr & "Inicio generar plano liquidez"
                    Else
                        strReportar = strReportar & vbCr & "Inicio generar plano fiducuenta"
                    End If

                    DescripcionFormato()
                    dcProxy.GeneracionCENCDecevals.Clear()
                    objResultGenerarDeceval = Await dcProxy.Load(dcProxy.GenerarArchivoCarteraLiquidez_ACHSyncQuery(strDocumentosGenerados, _ParametrosConsultaSelected.TipoCartera, CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, _ParametrosConsultaSelected.RutaArchivoPlano, Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask()

                    If objResultGenerarDeceval.HasError Then
                        If objResultGenerarDeceval.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "GenerarArchivoCarteraLiquidez", Program.TituloSistema, Program.Maquina, objResultGenerarDeceval.Error)
                        End If
                        objResultGenerarDeceval.MarkErrorAsHandled()
                        Return False
                    Else
                        strReportar = strReportar & vbCr & objResultGenerarDeceval.Entities.First.strMensaje
                        _ParametrosConsultaSelected.RutaArchivoPlano = String.Empty
                        dcArchivosProxy.Archivos.Clear()
                        dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
                    End If
                ElseIf logEsTransferenciaElectronica And logGMFCOMPENSACION_VB Then
                    strReportar = strReportar & vbCr & "Inicio generar plano transferencia electrónica"
                    dcArchivosProxy.RespuestaArchivoImportacions.Clear()
                    objResultGenerarAHC = Await dcArchivosProxy.Load(dcArchivosProxy.TransferenciasElectronica_ACH_ExportarSyncQuery(strDocumentosGenerados, Now.Date, String.Empty, String.Empty, "TransferenciaElectronicaACH", Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask()

                    If objResultGenerarAHC.HasError Then
                        If objResultGenerarAHC.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "GenerarArchivoCarteraLiquidez", Program.TituloSistema, Program.Maquina, objResultGenerarAHC.Error)
                        End If
                        objResultGenerarAHC.MarkErrorAsHandled()
                        Return False
                    Else
                        For Each li In objResultGenerarAHC.Entities
                            strReportar = strReportar & vbCr & li.Mensaje
                        Next

                        dcArchivosProxy.Archivos.Clear()
                        dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
                    End If
                Else
                    If _ParametrosConsultaSelected.MostrarCamposACH = Visibility.Visible And _ParametrosConsultaSelected.FormaPagoCE <> "C" Then
                        strCadena = Replace(strCadena, "Ñ", "N")
                        strCadena = Replace(strCadena, "Á", "A")
                        strCadena = Replace(strCadena, "É", "E")
                        strCadena = Replace(strCadena, "Í", "I")
                        strCadena = Replace(strCadena, "Ó", "O")
                        strCadena = Replace(strCadena, "Ú", "U")
                        strCadena = Replace(strCadena, "Ü", "U")

                        'Descripción:   Se agregan caracteres especiales porque en faltan algunos por validar
                        'Creado Por:    Jorge Peña (Alcuadrado S.A.)
                        'Fecha:         2 de Junio 2011
                        '            strCadena = Replace(strCadena, "¿", "")
                        '            strCadena = Replace(strCadena, "¡", "")
                        '            strCadena = Replace(strCadena, "¢", "")
                        '
                        '            'Caracteres especiales ASCII del 192 al 255

                        strCadena = Replace(strCadena, "À", "A")
                        strCadena = Replace(strCadena, "Â", "A")
                        strCadena = Replace(strCadena, "Ã", "A")
                        strCadena = Replace(strCadena, "Ä", "A")
                        strCadena = Replace(strCadena, "Å", "A")
                        strCadena = Replace(strCadena, "Æ", "")
                        strCadena = Replace(strCadena, "Ç", "C")
                        strCadena = Replace(strCadena, "È", "E")
                        strCadena = Replace(strCadena, "Ê", "E")
                        strCadena = Replace(strCadena, "Ë", "E")
                        strCadena = Replace(strCadena, "Ì", "I")
                        strCadena = Replace(strCadena, "Î", "I")
                        strCadena = Replace(strCadena, "Ï", "I")
                        strCadena = Replace(strCadena, "Ð", "")
                        strCadena = Replace(strCadena, "Ò", "O")
                        strCadena = Replace(strCadena, "Ô", "O")
                        strCadena = Replace(strCadena, "Õ", "O")
                        strCadena = Replace(strCadena, "Ö", "O")
                        strCadena = Replace(strCadena, "¥", "")
                        strCadena = Replace(strCadena, "×", "")
                        strCadena = Replace(strCadena, "Ø", "")
                        strCadena = Replace(strCadena, "Ù", "U")
                        strCadena = Replace(strCadena, "Û", "U")
                        strCadena = Replace(strCadena, "Ü", "U")
                        strCadena = Replace(strCadena, "Ý", "Y")
                        strCadena = Replace(strCadena, "Þ", "")
                        strCadena = Replace(strCadena, "ß", "")
                        strCadena = Replace(strCadena, "à", "a")
                        strCadena = Replace(strCadena, "â", "a")
                        strCadena = Replace(strCadena, "á", "a")
                        strCadena = Replace(strCadena, "ã", "a")
                        strCadena = Replace(strCadena, "ä", "a")
                        strCadena = Replace(strCadena, "å", "a")
                        strCadena = Replace(strCadena, "æ", "")
                        strCadena = Replace(strCadena, "ç", "c")
                        strCadena = Replace(strCadena, "è", "e")
                        strCadena = Replace(strCadena, "é", "e")
                        strCadena = Replace(strCadena, "ê", "e")
                        strCadena = Replace(strCadena, "ë", "e")
                        strCadena = Replace(strCadena, "ì", "i")
                        strCadena = Replace(strCadena, "í", "i")
                        strCadena = Replace(strCadena, "î", "i")
                        strCadena = Replace(strCadena, "ï", "i")
                        strCadena = Replace(strCadena, "ð", "")
                        strCadena = Replace(strCadena, "ò", "o")
                        strCadena = Replace(strCadena, "ó", "o")
                        strCadena = Replace(strCadena, "ô", "o")
                        strCadena = Replace(strCadena, "õ", "o")
                        strCadena = Replace(strCadena, "ö", "o")
                        strCadena = Replace(strCadena, "÷", "")
                        strCadena = Replace(strCadena, "ø", "")
                        strCadena = Replace(strCadena, "ù", "u")
                        strCadena = Replace(strCadena, "ú", "u")
                        strCadena = Replace(strCadena, "û", "u")
                        strCadena = Replace(strCadena, "ü", "u")
                        strCadena = Replace(strCadena, "ý", "y")
                        strCadena = Replace(strCadena, "þ", "")
                        strCadena = Replace(strCadena, "ÿ", "")
                        strCadena = Replace(strCadena, "ñ", "n")

                        strCadena = Replace(strCadena, "`", "")
                        strCadena = Replace(strCadena, "´", "")
                        strCadena = Replace(strCadena, ".", "")
                        strCadena = Replace(strCadena, "&", "")
                        strCadena = Replace(strCadena, "*", "")
                        strCadena = Replace(strCadena, "-", "")
                        strCadena = Replace(strCadena, ",", "")
                        strCadena = Replace(strCadena, "(", "")
                        strCadena = Replace(strCadena, ")", "")
                        strCadena = Replace(strCadena, "\", "")
                        strCadena = Replace(strCadena, "'", "")
                        strCadena = Replace(strCadena, "?", "")
                        strCadena = "<root> " & strCadena & "</root> "

                        Dim strCadena1 As String
                        Dim strCadena2 As String
                        Dim strCadena3 As String
                        Dim strCadena4 As String
                        Dim strCadena5 As String
                        Dim strCadena6 As String
                        Dim strCadena7 As String
                        Dim strCadena8 As String
                        Dim strCadena9 As String
                        Dim strCadena10 As String
                        Dim strCadena11 As String
                        Dim strCadena12 As String
                        Dim strCadena13 As String
                        Dim strCadena14 As String
                        Dim strCadena15 As String

                        strCadena1 = ""
                        strCadena2 = ""
                        strCadena3 = ""
                        strCadena4 = ""
                        strCadena5 = ""
                        strCadena6 = ""
                        strCadena7 = ""
                        strCadena8 = ""
                        strCadena9 = ""
                        strCadena10 = ""
                        strCadena11 = ""
                        strCadena12 = ""
                        strCadena13 = ""
                        strCadena14 = ""
                        strCadena15 = ""

                        strCadena1 = Mid(strCadena, 1, 200000)
                        strCadena2 = Mid(strCadena, 200001, 200000)
                        strCadena3 = Mid(strCadena, 400001, 200000)
                        strCadena4 = Mid(strCadena, 600001, 200000)
                        strCadena5 = Mid(strCadena, 800001, 200000)
                        strCadena6 = Mid(strCadena, 1000001, 200000)
                        strCadena7 = Mid(strCadena, 1200001, 200000)
                        strCadena8 = Mid(strCadena, 1400001, 200000)
                        strCadena9 = Mid(strCadena, 1600001, 200000)
                        strCadena10 = Mid(strCadena, 1800001, 200000)
                        strCadena11 = Mid(strCadena, 2000001, 200000)
                        strCadena12 = Mid(strCadena, 2200001, 200000)
                        strCadena13 = Mid(strCadena, 2400001, 200000)
                        strCadena14 = Mid(strCadena, 2600001, 200000)
                        strCadena15 = Mid(strCadena, 2800001, 200000)

                        objResultInteger = Await dcProxy.PlanosACHCadena_Insertar(strCadena1, strCadena2, strCadena3, strCadena4, strCadena5, strCadena6,
                                                                                  strCadena7, strCadena8, strCadena9, strCadena10, strCadena11, strCadena12,
                                                                                  strCadena13, strCadena14, strCadena15, Program.Usuario, Program.HashConexion).AsTask

                        If objResultInteger.HasError Then
                            If objResultInteger.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            Else
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "InsertarCEDetalle", Program.TituloSistema, Program.Maquina, objResultInteger.Error)
                            End If
                            objResultInteger.MarkErrorAsHandled()
                            Return False
                        End If

                        objResultInteger = Await dcProxy.PlanosACHCadena_Generar(_ParametrosConsultaSelected.FormatoACH, Nothing, Program.Usuario, Program.HashConexion).AsTask

                        If objResultInteger.HasError Then
                            If objResultInteger.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            Else
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "InsertarCEDetalle", Program.TituloSistema, Program.Maquina, objResultInteger.Error)
                            End If
                            objResultInteger.MarkErrorAsHandled()
                            Return False
                        End If

                        DescripcionFormato()
                        _ParametrosConsultaSelected.RutaArchivoPlano = _ParametrosConsultaSelected.RutaArchivoPlano & ".csv"
                        strReportar = strReportar & vbCr & "Inicio generar plano"

                        objResultBoolean = Await dcProxy.GenerarArchivoPlanoACH(_ParametrosConsultaSelected.FormatoACH, _ParametrosConsultaSelected.NroBanco, strFormato, _ParametrosConsultaSelected.RutaArchivoPlano,
                                                                                CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion).AsTask()

                        If objResultBoolean.HasError Then
                            If objResultBoolean.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            Else
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "InsertarCEDetalle", Program.TituloSistema, Program.Maquina, objResultBoolean.Error)
                            End If
                            objResultBoolean.MarkErrorAsHandled()
                            Return False
                        Else
                            strReportar = strReportar & vbCr & "Fin generar plano"
                            strReportar = strReportar & vbCr & "Se exportó correctamente el archivo ACH, revise los archivos generados en la parte inferior."
                            _ParametrosConsultaSelected.RutaArchivoPlano = String.Empty
                            dcArchivosProxy.Archivos.Clear()
                            dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
                        End If
                    End If
                End If
            End If

            ConsultarPagosDeceval()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "GenerarDocumentosTesoreria", Application.Current.ToString(), Program.Maquina, ex)
            objRetorno = False
        Finally
            IsBusy = False
        End Try
        Return objRetorno
    End Function

    ''' <summary>
    ''' Devuelve la descripcion del formato seleccionado.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub DescripcionFormato()
        If Not String.IsNullOrEmpty(_ParametrosConsultaSelected.FormatoACH) Then
            If DiccionarioCombosPagos.ContainsKey("FORMATOACH") Then
                If DiccionarioCombosPagos("FORMATOACH").Where(Function(i) i.ID = _ParametrosConsultaSelected.FormatoACH).Count > 0 Then
                    strFormato = (From dc In DiccionarioCombosPagos("FORMATOACH") Where dc.ID = _ParametrosConsultaSelected.FormatoACH Select dc.Descripcion).FirstOrDefault
                Else
                    strFormato = String.Empty
                End If
            Else
                strFormato = String.Empty
            End If
        Else
            strFormato = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' Valida la Fecha de Cierre del Sistema
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>SLB20121005</remarks>
    Private Function validaFechaCierre(ByVal pstrAccion As String) As Boolean
        validaFechaCierre = True
        'If Format(CType(_UltimosValoresSelected.Documento, Date).Date, "yyyy/MM/dd") <= Format(FechaCierre, "yyyy/MM/dd") Then 'Intentan registrar un documento con fecha inferior a la fecha de cierre registrada en tblInstalacion
        '    If Format(FechaCierre, "yyyy/mm/dd") <> "1900/01/01" Then
        '        Select Case pstrAccion
        '            Case "Actualizar"
        '                A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & CType(_UltimosValoresSelected.Documento, Date).Date.ToLongDateString & ") no puede ser ingresada o modificada porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '        End Select
        '        validaFechaCierre = False
        '    End If
        'End If
        Return validaFechaCierre
    End Function

    ''' <summary>
    ''' Método para seleccionar el total de los registros de los pendientes de tesorería.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub SeleccionarTodos()
        If Not IsNothing(_ListaPagosDECEVAL) Then
            For Each led In ListaPagosDECEVAL
                led.Seleccionado = True
            Next
        End If
    End Sub

    ''' <summary>
    ''' Método para des seleccionar el total de los registros de los pendientes de tesorería.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub DesseleccionarTodos()
        If Not IsNothing(_ListaPagosDECEVAL) Then
            For Each led In ListaPagosDECEVAL
                led.Seleccionado = False
            Next
        End If
    End Sub

    ''' <summary>
    ''' Buscar los datos del banco seleccionado en el encabezado y en el detalle de Tesoreria
    ''' </summary>
    ''' <param name="plngIdBanco">Codigo del banco el cual se va a realizar la búsqueda</param>
    ''' <remarks>SLB20130312</remarks>
    Friend Sub buscarBancos(Optional ByVal plngIdBanco As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery("cuentasbancarias", plngIdBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, "cuentasbancarias")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    'Public Sub NavegarArchivo(ByVal archivo As OyDImportaciones.Archivo)
    '    If Not IsNothing(archivo) Then
    '        If (Application.Current.IsRunningOutOfBrowser) Then
    '            'para que cargue el visor de reportes para cuando la consola se ejecuta por fuera del browser
    '            Dim button As New MyHyperlinkButton
    '            button.NavigateUri = New Uri(archivo.RutaWeb)
    '            button.TargetName = "_blank"
    '            button.ClickMe()
    '        Else
    '            HtmlPage.Window.Navigate(New Uri(archivo.RutaWeb), "_blank")
    '        End If
    '    End If
    'End Sub

    Public Sub BorrarArchivo(ByVal archivo As OyDImportaciones.Archivo)
        If Not IsNothing(archivo) Then
            'If MessageBox.Show("¿Está seguro de borrar este archivo?", "Borrar", MessageBoxButton.OKCancel) Then
            dcArchivosProxy.BorrarArchivo(archivo.Nombre, CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarArchivo, Nothing)
            'End If
        End If
    End Sub

#End Region

#Region "MetodosAsincronicos"

    ''' <summary>
    ''' Método encargado de recibir la lista de las Pagos Deceval.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130830</remarks>
    Private Sub TerminoConsultarPagosDeceval(ByVal lo As LoadOperation(Of OyDTesoreria.PagosDECEVAL))
        IsBusy = False
        If Not lo.HasError Then
            ListaPagosDECEVAL = dcProxy.PagosDECEVALs

            If dcProxy.PagosDECEVALs.Count = 0 Then
                If Not lo.UserState = "Grabar" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontraron abonos Deceval.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los pendientes de tesorería.",
                     Me.ToString(), "TerminoConsultarPendientesTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = obj.Value
        End If
    End Sub

    Private Sub TerminoverificarGMF(ByVal lo As LoadOperation(Of OyDTesoreria.Instalacion))
        Try
            If Not lo.HasError Then
                mstrNombreConsecutivoNota = dcProxy.Instalacions.First.STRTIPO
                mstrctacontableMGF = dcProxy.Instalacions.First.STRCTACONTABLE
                mstrccostosMGF = dcProxy.Instalacions.First.strCCosto
                mstrctacontablecontrapMGF = dcProxy.Instalacions.First.STRCTACONTABLECONTRAPARTE
                mstrccostocontrapMGF = dcProxy.Instalacions.First.STRCCOSTOCONTRAPARTE
                mstrCtacontraparteNotasCXC = dcProxy.Instalacions.First.strCtaContableContraparteNotasCxC
                msnGMF = dcProxy.Instalacions.First.INTGMS
                mdblGMFInferior = dcProxy.Instalacions.First.DBLGMFINFERIOR
                If mdblGMFInferior = 0 Then
                    mdblGMFInferior = msnGMF
                End If
                'strservidor = dcProxy.Instalacions.First.strServidor
                'strbasedatos = dcProxy.Instalacions.First.strBaseDatos
                'strowner = dcProxy.Instalacions.First.strOwner
                'strcia = dcProxy.Instalacions.First.lngCompania
                'strcuenta = dcProxy.Instalacions.First.strCtaContableClientes
                mstrCtaContableClientes = dcProxy.Instalacions.First.strCtaContableClientes
                'logValidacuentasuperval = dcProxy.Instalacions.First.logValidaCuentaSuperVal
                mstrCtaContableTrasladoDB = dcProxy.Instalacions.First.strCtaContableTrasladoDB
                mstrCtaContableTrasladoCR = dcProxy.Instalacions.First.strCtaContableTrasladoCR
                mstrTipoNotasCxC = dcProxy.Instalacions.First.strtipoNotasCxC
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en VerificarGMF",
                                                 Me.ToString(), "TerminoverificarGMF", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en VerificarGMF",
                                 Me.ToString(), "TerminoverificarGMF", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'JCM20160217
    Private Sub TerminoTraerCombosPagosDeceval(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                Dim objListaCombos As ObservableCollection(Of OYDUtilidades.ItemCombo) = Nothing
                Dim objListaNodosCategoria As ObservableCollection(Of OYDUtilidades.ItemCombo) = Nothing
                Dim dicListaCombos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)) = Nothing
                Dim strNombreCategoria As String = String.Empty

                objListaCombos = New ObservableCollection(Of OYDUtilidades.ItemCombo)(lo.Entities)
                If objListaCombos.Count > 0 Then
                    Dim listaCategorias = From lc In objListaCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada
                    dicListaCombos = New Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
                    For Each NombreCategoria As String In listaCategorias
                        strNombreCategoria = NombreCategoria
                        objListaNodosCategoria = New ObservableCollection(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria))
                        dicListaCombos.Add(NombreCategoria, objListaNodosCategoria)
                    Next
                End If

                DiccionarioCombosPagos = dicListaCombos

                If DiccionarioCombosPagos.ContainsKey("O_PARAM_CONFIG_TESO") Then
                    If DiccionarioCombosPagos("O_PARAM_CONFIG_TESO").Where(Function(obj) obj.ID = "SOLODIVIDENDOS_PAGOSDCVL_CE").Count > 0 Then
                        If DiccionarioCombosPagos("O_PARAM_CONFIG_TESO").Where(Function(obj) obj.ID = "SOLODIVIDENDOS_PAGOSDCVL_CE").FirstOrDefault.Descripcion.Equals("SI") Then
                            _ParametrosConsultaSelected.MostrarCamposACH = Visibility.Visible
                        End If
                    End If

                    If DiccionarioCombosPagos("O_PARAM_CONFIG_TESO").Where(Function(obj) obj.ID = "VALIDACION_NROIDENTIFICACION_ACH").Count > 0 Then
                        If DiccionarioCombosPagos("O_PARAM_CONFIG_TESO").Where(Function(obj) obj.ID = "VALIDACION_NROIDENTIFICACION_ACH").FirstOrDefault.Descripcion.Equals("SI") Then
                            _mstrValidacion_NroIdentificacion_ACH = "SI"
                        End If
                    End If
                End If

                _ParametrosConsultaSelected.TipoEspecie = "A"

                objProxy.ItemCombos.Clear()
                objProxy.Load(objProxy.cargarCombosCondicionalQuery("CONSECUTIVOS_DEMOCRATIZADOS", "NO", 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosRecibos, Nothing)
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivosEgresos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivosEgresos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    'JCM20160217
    Private Sub TerminoTraerConsecutivosRecibos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                _ParametrosConsultaSelected.NombreConsecutivoRC = String.Empty

                If objProxy.ItemCombos.Count > 0 Then
                    listaConsecutivoRecibos = objProxy.ItemCombos.Where(Function(y) y.Categoria = "CAJA").ToList
                Else
                    listaConsecutivoRecibos = Nothing
                End If

                objProxy.ItemCombos.Clear()
                objProxy.Load(objProxy.cargarCombosEspecificosQuery("Tesoreria_ComprobantesEgreso", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosEgresos, Nothing)
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivosRecibos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivosRecibos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'JCM20160217
    Private Sub TerminoTraerConsecutivosEgresos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If objProxy.ItemCombos.Count > 0 Then
                    dicListaCombos = objProxy.ItemCombos.Where(Function(y) y.Categoria = "NombreConsecutivoCE").ToList
                Else
                    dicListaCombos = Nothing
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivosEgresos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivosEgresos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub


    'JCM20160217
    Private Sub TerminoTraerConsecutivosNotas(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If objProxy.ItemCombos.Count > 0 Then
                    dicListaCombos = objProxy.ItemCombos.Where(Function(y) y.Categoria = "ConsecutivosTesoreriaNotas").ToList
                Else
                    dicListaCombos = Nothing
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivosNotas", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivosNotas", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub



    ''' <summary>
    ''' Método que recibe la respuesta del borrado de los encabezados y detalles previamente insertados.
    ''' </summary>
    ''' <remarks>SLB20130422</remarks>
    Private Sub TerminoBorrarTesoreria(ByVal lo As InvokeOperation(Of Integer))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar los documentos de tesorería insertados.",
                     Me.ToString(), "TerminoBorrarTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    ''' <summary>
    ''' Método recibe la respuesta si el banco existe o no
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>20130201</remarks>
    Private Sub TerminoTraerCuentasBancarias(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "cuentasbancarias"
                        If lo.Entities.ToList.Count > 0 Then
                            If lo.Entities.First.InfoAdicional02.Equals("1") Then
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos",
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(),
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoBorrarArchivo(ByVal e As InvokeOperation)
        If Not IsNothing(e.Error) Then
            MessageBox.Show(e.Error.Message)
            e.MarkErrorAsHandled()
        End If
        dcArchivosProxy.Archivos.Clear()
        dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
    End Sub

    Private Sub TerminoTraerArchivos(ByVal lo As LoadOperation(Of OyDImportaciones.Archivo))
        Try
            If Not lo.HasError Then
                ListaArchivosGuardados = dcArchivosProxy.Archivos.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos",
                                                 Me.ToString(), "TerminoTraerArchivos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos",
                                                             Me.ToString(), "TerminoTraerArchivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    'JAG 20141015
    Private Sub Terminotraerparametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el parametro", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            Select Case obj.UserState
                Case "PAGOSDECEVAL_GMF_DIVIDENDOS"
                    strGMFDividendos = obj.Value
                    objProxy.Verificaparametro("PAGOSDECEVAL_GENERARRECIBOSANULADOS", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "PAGOSDECEVAL_GENERARRECIBOSANULADOS") 'JAG 20141015
                Case "PAGOSDECEVAL_GENERARRECIBOSANULADOS"
                    If obj.Value = "SI" Then
                        MostrarCamposDemocratizados = Visibility.Visible
                    Else
                        MostrarCamposDemocratizados = Visibility.Collapsed
                    End If
                    objProxy.Verificaparametro("UTILIZARCUENTASTRASLADOGMF", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "UTILIZARCUENTASTRASLADOGMF")
                Case "UTILIZARCUENTASTRASLADOGMF"
                    If obj.Value = "SI" Then
                        logUtilizaCuentasTrasladoGMF = True
                    Else
                        logUtilizaCuentasTrasladoGMF = False
                    End If
                    objProxy.Verificaparametro("NOTAS_COBRO_GMF", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "NOTAS_COBRO_GMF")
                Case "NOTAS_COBRO_GMF"
                    If obj.Value = "SI" Then
                        logUtilizaCuentasNotasCobroGMF = True
                    Else
                        logUtilizaCuentasNotasCobroGMF = False
                    End If
                    objProxy.Verificaparametro("TRANSFERENCIASACH_BANCOLOMBIA", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "TRANSFERENCIASACH_BANCOLOMBIA")
                Case "TRANSFERENCIASACH_BANCOLOMBIA"
                    If obj.Value = "SI" Then
                        logUtilizaTransferenciaACHBancolombia = True
                        _ParametrosConsultaSelected.logExento = True
                        'MostrarExento = Visibility.Visible
                    Else
                        logUtilizaTransferenciaACHBancolombia = False
                        _ParametrosConsultaSelected.logExento = Nothing
                        'MostrarExento = Visibility.Collapsed

                    End If
                    objProxy.Verificaparametro("CF_UTILIZA_PASIVA_A2", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "CF_UTILIZA_PASIVA_A2")
                Case "CF_UTILIZA_PASIVA_A2"
                    If obj.Value = "SI" Then
                        logUtilizaPasivaA2 = True
                    Else
                        logUtilizaPasivaA2 = False
                    End If
                Case "GMFCOMPENSACION_VB"
                    If obj.Value = "SI" Then
                        logGMFCOMPENSACION_VB = True
                    Else
                        logGMFCOMPENSACION_VB = False
                    End If
            End Select
        End If
    End Sub
    'JAG 20141015

#End Region

#Region "Propiedades"

    Private _DiccionarioCombosPagos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombosPagos() As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombosPagos
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombosPagos = value
            MyBase.CambioItem("DiccionarioCombosPagos")
        End Set
    End Property

    Private _ListaPagosDECEVAL As EntitySet(Of OyDTesoreria.PagosDECEVAL)
    Public Property ListaPagosDECEVAL As EntitySet(Of OyDTesoreria.PagosDECEVAL)
        Get
            Return _ListaPagosDECEVAL
        End Get
        Set(ByVal value As EntitySet(Of OyDTesoreria.PagosDECEVAL))
            _ListaPagosDECEVAL = value
            MyBase.CambioItem("ListaPagosDECEVAL")
            MyBase.CambioItem("ListaPagosDECEVALPaged")
        End Set
    End Property

    Public ReadOnly Property ListaPagosDECEVALPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPagosDECEVAL) Then
                Dim view = New PagedCollectionView(_ListaPagosDECEVAL)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    'JCM20160219
    Private WithEvents _Valoreselected As New ParametrosConsultaCE_NC
    Public Property Valoreselected As ParametrosConsultaCE_NC
        Get
            Return _Valoreselected
        End Get
        Set(ByVal value As ParametrosConsultaCE_NC)
            _Valoreselected = value
            MyBase.CambioItem("Valoreselected")
        End Set
    End Property


    Private WithEvents _ParametrosConsultaSelected As New ParametrosConsultaCE_NC
    Public Property ParametrosConsultaSelected As ParametrosConsultaCE_NC
        Get
            Return _ParametrosConsultaSelected
        End Get
        Set(ByVal value As ParametrosConsultaCE_NC)
            _ParametrosConsultaSelected = value
            MyBase.CambioItem("ParametrosConsultaSelected")
        End Set
    End Property

    Private _dicListaCombos As List(Of OYDUtilidades.ItemCombo) = Nothing
    Public Property dicListaCombos As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _dicListaCombos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _dicListaCombos = value
            MyBase.CambioItem("dicListaCombos")
        End Set
    End Property

    Private _listaConsecutivoRecibos As List(Of OYDUtilidades.ItemCombo) = Nothing
    Public Property listaConsecutivoRecibos As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listaConsecutivoRecibos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaConsecutivoRecibos = value
            MyBase.CambioItem("listaConsecutivoRecibos")
        End Set
    End Property



    Public _ListaArchivosGuardados As List(Of OyDImportaciones.Archivo)
    Public Property ListaArchivosGuardados As List(Of OyDImportaciones.Archivo)
        Get
            Return _ListaArchivosGuardados
        End Get
        Set(ByVal value As List(Of OyDImportaciones.Archivo))
            _ListaArchivosGuardados = value
            CambioItem("ListaArchivosGuardados")
            CambioItem("ListaArchivosGuardadosPaged")
        End Set
    End Property

    'JCM20160219
    ' Compañía asociada al consecutivo seleccionado en el encabezado
    Private _strCompaniaConsecutivo As String = String.Empty
    Public Property strCompaniaConsecutivo() As String
        Get
            Return _strCompaniaConsecutivo
        End Get
        Set(ByVal value As String)
            _strCompaniaConsecutivo = value
            MyBase.CambioItem("strCompaniaConsecutivo")
        End Set
    End Property

    'JCM20160219
    ' Compañía asociada al consecutivo seleccionado en el encabezado
    Private _strCompaniaConsecutivoComprobanteNota As String = String.Empty
    Public Property strCompaniaConsecutivoComprobanteNota() As String
        Get
            Return _strCompaniaConsecutivoComprobanteNota
        End Get
        Set(ByVal value As String)
            _strCompaniaConsecutivoComprobanteNota = value
            MyBase.CambioItem("strCompaniaConsecutivoComprobanteNota")
        End Set
    End Property

    Private _HabilitaTipoCruce As Boolean = False
    Public Property HabilitaTipoCruce() As Boolean
        Get
            Return _HabilitaTipoCruce
        End Get
        Set(ByVal value As Boolean)
            _HabilitaTipoCruce = value
            MyBase.CambioItem("HabilitaTipoCruce")
        End Set
    End Property

    Private _MostrarCamposDemocratizados As Visibility = Visibility.Collapsed
    Public Property MostrarCamposDemocratizados() As Visibility
        Get
            Return _MostrarCamposDemocratizados
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposDemocratizados = value
            MyBase.CambioItem("MostrarCamposDemocratizados")
        End Set
    End Property
    Private _MostrarTipoCartera As Visibility = Visibility.Collapsed
    Public Property MostrarTipoCartera() As Visibility
        Get
            Return _MostrarTipoCartera
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoCartera = value
            MyBase.CambioItem("MostrarTipoCartera")
        End Set
    End Property
    Private _MostrarTipoBanco As Visibility = Visibility.Collapsed
    Public Property MostrarTipoBanco() As Visibility
        Get
            Return _MostrarTipoBanco
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoBanco = value
            MyBase.CambioItem("MostrarTipoBanco")
        End Set
    End Property
    Private _MostrarExento As Visibility = Visibility.Collapsed
    Public Property MostrarExento() As Visibility
        Get
            Return _MostrarExento
        End Get
        Set(ByVal value As Visibility)
            _MostrarExento = value
            MyBase.CambioItem("MostrarExento")
        End Set
    End Property

    Private _HabilitarExento As Boolean
    Public Property HabilitarExento() As Boolean
        Get
            Return _HabilitarExento
        End Get
        Set(ByVal value As Boolean)
            _HabilitarExento = value
            MyBase.CambioItem("HabilitarExento")
        End Set
    End Property

    Private _HabilitarGenerar As Boolean = True
    Public Property HabilitarGenerar() As Boolean
        Get
            Return _HabilitarGenerar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarGenerar = value
            MyBase.CambioItem("HabilitarGenerar")
        End Set
    End Property

    Private _logSeleccionarTodos As Boolean
    Public Property logSeleccionarTodos() As Boolean
        Get
            Return _logSeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _logSeleccionarTodos = value
            MyBase.CambioItem("logSeleccionarTodos")
        End Set
    End Property

    Private _MostrarTipoCheque As Visibility = Visibility.Collapsed
    Public Property MostrarTipoCheque() As Visibility
        Get
            Return _MostrarTipoCheque
        End Get
        Set(ByVal value As Visibility)
            _MostrarTipoCheque = value
            MyBase.CambioItem("MostrarTipoCheque")
        End Set
    End Property


    Public Property strCompaniaBanco As String = String.Empty
#End Region

#Region "PropertyChanged"

    Private Sub _ParametrosConsultaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ParametrosConsultaSelected.PropertyChanged
        Try
            If e.PropertyName = "TipoTesoreria" Then

                ListaPagosDECEVAL = Nothing
                logSeleccionarTodos = False

                If _ParametrosConsultaSelected.TipoTesoreria Then

                    'JCM20160219
                    objProxy.Load(objProxy.cargarCombosEspecificosQuery("Tesoreria_ComprobantesEgreso", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosEgresos, Nothing)
                    '_ParametrosConsultaSelected.FormaPagoCE = String.Empty
                    _ParametrosConsultaSelected.NombreConsecutivo = String.Empty
                    _ParametrosConsultaSelected.NroBanco = Nothing
                    _ParametrosConsultaSelected.CuentaContable = String.Empty
                    _ParametrosConsultaSelected.FormatoACH = String.Empty
                    _ParametrosConsultaSelected.OcultarCampos = Visibility.Visible

                Else
                    'JCM20160219
                    objProxy.Load(objProxy.cargarCombosEspecificosQuery("Tesoreria_Notas", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosNotas, Nothing)

                    '_ParametrosConsultaSelected.FormaPagoCE = String.Empty
                    _ParametrosConsultaSelected.NombreConsecutivo = String.Empty
                    _ParametrosConsultaSelected.NroBanco = Nothing
                    _ParametrosConsultaSelected.CuentaContable = String.Empty
                    _ParametrosConsultaSelected.FormatoACH = String.Empty
                    _ParametrosConsultaSelected.OcultarCampos = Visibility.Collapsed
                End If
                Exit Sub
                'JCM20160219
            ElseIf e.PropertyName = "NombreConsecutivoRC" Then
                ListaPagosDECEVAL = Nothing
                logSeleccionarTodos = False

                If Not String.IsNullOrEmpty(_ParametrosConsultaSelected.NombreConsecutivoRC) Then
                    If Not IsNothing(listaConsecutivoRecibos) Then
                        strCompaniaConsecutivo = (From c In listaConsecutivoRecibos Where c.ID = _ParametrosConsultaSelected.NombreConsecutivoRC Select c).FirstOrDefault.Retorno
                    Else
                        strCompaniaConsecutivo = String.Empty
                    End If
                Else
                    strCompaniaConsecutivo = String.Empty
                End If
                'JCM20160219
            ElseIf e.PropertyName = "NombreConsecutivo" Then
                If Not String.IsNullOrEmpty(_ParametrosConsultaSelected.NombreConsecutivo) Then
                    If Not IsNothing(dicListaCombos) Then
                        strCompaniaConsecutivoComprobanteNota = (From c In dicListaCombos Where c.ID = _ParametrosConsultaSelected.NombreConsecutivo Select c).FirstOrDefault.Retorno
                    Else
                        strCompaniaConsecutivoComprobanteNota = String.Empty
                    End If
                Else
                    strCompaniaConsecutivoComprobanteNota = String.Empty
                End If

                _ParametrosConsultaSelected.NroBanco = Nothing
                _ParametrosConsultaSelected.BancoTieneGMF = False
                _ParametrosConsultaSelected.CobroGMF = False
                strCompaniaBanco = String.Empty

                'JCM20160219
            ElseIf e.PropertyName = "FormaPagoCE" Then
                ListaPagosDECEVAL = Nothing
                logSeleccionarTodos = False
                'If _ParametrosConsultaSelected.FormaPagoCE = "T" And strGMFDividendos = "SI" Then
                '    _ParametrosConsultaSelected.CobroGMF = True
                'Else
                '    _ParametrosConsultaSelected.CobroGMF = False
                'End If
                If _ParametrosConsultaSelected.FormaPagoCE = "C" Then
                    HabilitaTipoCruce = True
                Else
                    HabilitaTipoCruce = False
                    _ParametrosConsultaSelected.TipoCheque = String.Empty
                End If

                _ParametrosConsultaSelected.TipoCartera = Nothing
                _ParametrosConsultaSelected.TipoACH = Nothing

                If _ParametrosConsultaSelected.FormaPagoCE = BYT_FORMAPAGO_TRANSFERENCIACARTERASCOLECTIVAS Then
                    MostrarTipoCartera = Visibility.Visible
                    MostrarTipoBanco = Visibility.Collapsed
                    MostrarTipoCheque = Visibility.Collapsed
                    If logUtilizaTransferenciaACHBancolombia Then
                        _ParametrosConsultaSelected.TipoCartera = "1"
                        MostrarExento = Visibility.Collapsed
                        _ParametrosConsultaSelected.logExento = Nothing
                    End If
                    _ParametrosConsultaSelected.HabilitarSeleccionACH = False
                    _ParametrosConsultaSelected.HabilitarSeleccionRutaArchivoPlano = True
                ElseIf _ParametrosConsultaSelected.FormaPagoCE = BYT_FORMAPAGOTRANSFERENCIA Then
                    If logUtilizaTransferenciaACHBancolombia Then
                        MostrarTipoCartera = Visibility.Collapsed
                        MostrarTipoBanco = Visibility.Visible
                        MostrarTipoCheque = Visibility.Collapsed
                        If logUtilizaTransferenciaACHBancolombia Then
                            HabilitarExento = True
                            MostrarExento = Visibility.Visible
                            _ParametrosConsultaSelected.logExento = True
                        End If

                        _ParametrosConsultaSelected.HabilitarSeleccionACH = False
                        _ParametrosConsultaSelected.HabilitarSeleccionRutaArchivoPlano = False
                        _ParametrosConsultaSelected.TipoACH = "1"
                    Else
                        MostrarTipoCartera = Visibility.Collapsed
                        MostrarTipoBanco = Visibility.Collapsed
                        MostrarTipoCheque = Visibility.Collapsed
                        If logUtilizaTransferenciaACHBancolombia Then
                            MostrarExento = Visibility.Collapsed
                            _ParametrosConsultaSelected.logExento = Nothing
                        End If

                        _ParametrosConsultaSelected.HabilitarSeleccionACH = True
                        _ParametrosConsultaSelected.HabilitarSeleccionRutaArchivoPlano = True
                    End If
                ElseIf _ParametrosConsultaSelected.FormaPagoCE = BYT_FORMAPAGOCHEQUE Then
                    MostrarTipoCartera = Visibility.Collapsed
                    MostrarTipoBanco = Visibility.Collapsed
                    MostrarTipoCheque = Visibility.Visible
                    If logUtilizaTransferenciaACHBancolombia Then
                        MostrarExento = Visibility.Collapsed
                        _ParametrosConsultaSelected.logExento = Nothing
                    End If
                    _ParametrosConsultaSelected.HabilitarSeleccionACH = True
                    _ParametrosConsultaSelected.HabilitarSeleccionRutaArchivoPlano = True
                    _ParametrosConsultaSelected.TipoCheque = "N"
                Else
                    MostrarTipoCartera = Visibility.Collapsed
                    MostrarTipoBanco = Visibility.Collapsed
                    MostrarTipoCheque = Visibility.Collapsed
                    If logUtilizaTransferenciaACHBancolombia Then
                        MostrarExento = Visibility.Collapsed
                        _ParametrosConsultaSelected.logExento = Nothing
                    End If
                    _ParametrosConsultaSelected.HabilitarSeleccionACH = True
                    _ParametrosConsultaSelected.HabilitarSeleccionRutaArchivoPlano = True

                End If
            ElseIf e.PropertyName = "Democratizados" Then
                ListaPagosDECEVAL = Nothing
                logSeleccionarTodos = False
                If Not IsNothing(_ParametrosConsultaSelected.Democratizados) Then
                    If Not IsNothing(dcProxy.ItemCombos) Then
                        objProxy.ItemCombos.Clear()
                    End If
                    objProxy.Load(objProxy.cargarCombosCondicionalQuery("CONSECUTIVOS_DEMOCRATIZADOS", IIf(_ParametrosConsultaSelected.Democratizados, "SI", "NO"), 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosRecibos, Nothing)
                End If
            ElseIf e.PropertyName = "TipoCartera" Then
                ListaPagosDECEVAL = Nothing
                logSeleccionarTodos = False
                If _ParametrosConsultaSelected.TipoCartera = "1" Then
                    HabilitarExento = False
                    MostrarExento = Visibility.Collapsed
                    _ParametrosConsultaSelected.logExento = Nothing
                    HabilitarGenerar = False
                Else
                    MostrarExento = Visibility.Visible
                    HabilitarExento = False
                    HabilitarGenerar = True
                    _ParametrosConsultaSelected.logExento = False
                End If
            ElseIf e.PropertyName = "TipoACH" Then
                ListaPagosDECEVAL = Nothing
                logSeleccionarTodos = False
            ElseIf e.PropertyName = "FechaDesde" Then
                ListaPagosDECEVAL = Nothing
                logSeleccionarTodos = False
            ElseIf e.PropertyName = "FechaHasta" Then
                ListaPagosDECEVAL = Nothing
                logSeleccionarTodos = False
                'ElseIf e.PropertyName = "logExento" Then
                'If MostrarTipoCartera = Visibility.Visible Then
                '    If _ParametrosConsultaSelected.logExento Then
                '        _ParametrosConsultaSelected.TipoCartera = "1"
                '    Else
                '        _ParametrosConsultaSelected.TipoCartera = "2"
                '    End If
                'End If
            End If
            'JAG 20141020

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_ParametrosConsultaSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region


End Class


''' <summary>
''' Clase para el manejo de los parametros de consulta de los CE pendientes.
''' </summary>
''' <remarks>SLB201300830</remarks>
Public Class ParametrosConsultaCE_NC
    Implements INotifyPropertyChanged

    Private _FechaDesde As Date = Now.Date
    Public Property FechaDesde As Date
        Get
            Return _FechaDesde
        End Get
        Set(ByVal value As Date)
            _FechaDesde = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaDesde"))
        End Set
    End Property

    Private _FechaHasta As Date = Now.Date
    Public Property FechaHasta As Date
        Get
            Return _FechaHasta
        End Get
        Set(ByVal value As Date)
            _FechaHasta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaHasta"))
        End Set
    End Property

    Private _NombreConsecutivoRC As String = "-1"
    Public Property NombreConsecutivoRC As String
        Get
            Return _NombreConsecutivoRC
        End Get
        Set(ByVal value As String)
            _NombreConsecutivoRC = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreConsecutivoRC"))
        End Set
    End Property

    Private _NroRC As Integer?
    Public Property NroRC As Integer?
        Get
            Return _NroRC
        End Get
        Set(ByVal value As Integer?)
            _NroRC = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroRC"))
        End Set
    End Property

    Private _FormaPagoCE As String = String.Empty
    Public Property FormaPagoCE As String
        Get
            Return _FormaPagoCE
        End Get
        Set(ByVal value As String)
            _FormaPagoCE = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FormaPagoCE"))
        End Set
    End Property

    Private _Democratizados As Boolean
    Public Property Democratizados() As Boolean
        Get
            Return _Democratizados
        End Get
        Set(ByVal value As Boolean)
            _Democratizados = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Democratizados"))
        End Set
    End Property

    Private _logExento As Nullable(Of Boolean)
    Public Property logExento() As Nullable(Of Boolean)
        Get
            Return _logExento
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            _logExento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logExento"))
        End Set
    End Property

    Private _IDEspecie As String
    Public Property IDEspecie() As String
        Get
            Return _IDEspecie
        End Get
        Set(ByVal value As String)
            _IDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDEspecie"))
        End Set
    End Property

    Private _TipoCartera As String
    Public Property TipoCartera() As String
        Get
            Return _TipoCartera
        End Get
        Set(ByVal value As String)
            _TipoCartera = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoCartera"))
        End Set
    End Property

    Private _TipoACH As String
    Public Property TipoACH() As String
        Get
            Return _TipoACH
        End Get
        Set(ByVal value As String)
            _TipoACH = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoACH"))
        End Set
    End Property


    Private _TipoTesoreria As Boolean = True
    ''' <summary>
    ''' Si es True es Comprobante de Egreso y si es False es una Nota de Tesorería
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>SLB20130830</remarks>
    Public Property TipoTesoreria As Boolean
        Get
            Return _TipoTesoreria
        End Get
        Set(ByVal value As Boolean)
            _TipoTesoreria = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoTesoreria"))
        End Set
    End Property

    Private _FormatoACH As String
    Public Property FormatoACH As String
        Get
            Return _FormatoACH
        End Get
        Set(ByVal value As String)
            _FormatoACH = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FormatoACH"))
        End Set
    End Property

    Private _NombreConsecutivo As String
    Public Property NombreConsecutivo As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            _NombreConsecutivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreConsecutivo"))
        End Set
    End Property

    Private _FechaElaboracion As Date = Now.Date
    Public Property FechaElaboracion As Date
        Get
            Return _FechaElaboracion
        End Get
        Set(ByVal value As Date)
            _FechaElaboracion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaElaboracion"))
        End Set
    End Property

    Private _NroBanco As Integer?
    Public Property NroBanco As Integer?
        Get
            Return _NroBanco
        End Get
        Set(ByVal value As Integer?)
            _NroBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroBanco"))
        End Set
    End Property

    Private _BancoTieneGMF As Boolean
    Public Property BancoTieneGMF As Boolean
        Get
            Return _BancoTieneGMF
        End Get
        Set(ByVal value As Boolean)
            _BancoTieneGMF = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("BancoTieneGMF"))
        End Set
    End Property

    Private _CobroGMF As Boolean = False
    Public Property CobroGMF As Boolean
        Get
            Return _CobroGMF
        End Get
        Set(ByVal value As Boolean)
            _CobroGMF = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CobroGMF"))
        End Set
    End Property

    Private _CuentaContable As String
    Public Property CuentaContable As String
        Get
            Return _CuentaContable
        End Get
        Set(ByVal value As String)
            _CuentaContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaContable"))
        End Set
    End Property

    Private _TipoCheque As String
    Public Property TipoCheque As String
        Get
            Return _TipoCheque
        End Get
        Set(ByVal value As String)
            _TipoCheque = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoCheque"))
        End Set
    End Property

    Private _OcultarCampos As Visibility = Visibility.Visible
    Public Property OcultarCampos As Visibility
        Get
            Return _OcultarCampos
        End Get
        Set(ByVal value As Visibility)
            _OcultarCampos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OcultarCampos"))
        End Set
    End Property

    Private _MostrarCamposACH As Visibility = Visibility.Collapsed
    Public Property MostrarCamposACH As Visibility
        Get
            Return _MostrarCamposACH
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposACH = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarCamposACH"))
        End Set
    End Property

    Private _HabilitarSeleccionACH As Boolean = True
    Public Property HabilitarSeleccionACH() As Boolean
        Get
            Return _HabilitarSeleccionACH
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionACH = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarSeleccionACH"))
        End Set
    End Property

    Private _HabilitarSeleccionRutaArchivoPlano As Boolean = True
    Public Property HabilitarSeleccionRutaArchivoPlano() As Boolean
        Get
            Return _HabilitarSeleccionRutaArchivoPlano
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionRutaArchivoPlano = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarSeleccionRutaArchivoPlano"))
        End Set
    End Property


    Public Property _mlogValidoNombrePlano As Boolean = True

    Private _RutaArchivoPlano As String
    Public Property RutaArchivoPlano As String
        Get
            Return _RutaArchivoPlano
        End Get
        Set(ByVal value As String)
            _RutaArchivoPlano = value
            If Not String.IsNullOrEmpty(_RutaArchivoPlano) Then
                If Not Regex.IsMatch(_RutaArchivoPlano, "^[a-z A-ZÑ 0-9 á-ú ._-]*$") Then
                    A2Utilidades.Mensajes.mostrarMensaje("El nombre del archivo plano posee caractares no válidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _mlogValidoNombrePlano = False
                Else
                    _mlogValidoNombrePlano = True
                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RutaArchivoPlano"))
        End Set
    End Property

    Private _TipoEspecie As String
    Public Property TipoEspecie As String
        Get
            Return _TipoEspecie
        End Get
        Set(ByVal value As String)
            _TipoEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoEspecie"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class


