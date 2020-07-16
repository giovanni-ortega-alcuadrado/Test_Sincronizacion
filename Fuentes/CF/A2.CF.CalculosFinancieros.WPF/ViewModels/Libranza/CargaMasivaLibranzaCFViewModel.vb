Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports A2MCCoreWPF
Imports A2.CF.CalculosFinancieros
Imports System.Threading.Tasks


Public Class CargaMasivaLibranzaCFViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New CalculosFinancierosDomainContext
                dcProxyConsulta = New CalculosFinancierosDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext
            Else
                dcProxy = New CalculosFinancierosDomainContext(New System.Uri(Program.RutaServicioCalculosFinancieros))
                dcProxyConsulta = New CalculosFinancierosDomainContext(New System.Uri(Program.RutaServicioCalculosFinancieros))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If

            'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of CalculosFinancierosDomainContext.ICalculosFinancierosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(dcProxyConsulta.DomainClient, WebDomainClient(Of CalculosFinancierosDomainContext.ICalculosFinancierosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If System.Diagnostics.Debugger.IsAttached Then

            End If

            '---------------------------------------------------------------------------------------------------------------------                        
            If Not Program.IsDesignMode() Then

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", Me.ToString(), "CargaMasivaLibranzaCFViewModel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Async Function inicializar() As Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                'Consulta los combos de la pantalla.
                Await CargarCombos()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Constantes"

    Private Const OPCION_INICIO As String = "INICIO"
    Private Const OPCION_NUEVO As String = "NUEVO"
    Private Const OPCION_EDITAR As String = "EDITAR"
    Private Const OPCION_ANULAR As String = "ANULAR"
    Private Const OPCION_DUPLICAR As String = "DUPLICAR"
    Private Const OPCION_ACTUALIZAR As String = "ACTUALIZAR"
    Public Enum TipoOpcionCargar
        ARCHIVO
        CAMPOS
        RESULTADO
    End Enum
    Private Const COLOR_DESHABILITADO As String = "#E2E2E2"
    Private Const COLOR_HABILITADO As String = "#FFFFFFFF"

#End Region

#Region "Variables"
    Private dcProxy As CalculosFinancierosDomainContext
    Private dcProxyConsulta As CalculosFinancierosDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Private dcProxyImportaciones As ImportacionesDomainContext
    Dim logConsultarCliente As Boolean = True
    Public viewCargaMasiva As Libranzas_CargaMasivaView
    Dim logRecargarResultados As Boolean = False
    Dim logCargarContenido As Boolean = True
    Dim strTemporalTipoNegocio As String = String.Empty
    Dim strTemporalTipoOperacion As String = String.Empty
    Dim logCalculoValores As Boolean = True
    Dim dtmFechaServidor As DateTime
#End Region

#Region "Propiedades"

    Private _IsbusyResultados As Boolean = False
    Public Property IsbusyResultados() As Boolean
        Get
            Return _IsbusyResultados
        End Get
        Set(ByVal value As Boolean)
            _IsbusyResultados = value
            MyBase.CambioItem("IsbusyResultados")
        End Set
    End Property

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _ComitenteSeleccionado = value
            Try
                SeleccionarCliente(_ComitenteSeleccionado)
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad del cliente seleccionado.", Me.ToString, "ComitenteSeleccionado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try

            MyBase.CambioItem("ComitenteSeleccionado")
        End Set
    End Property

    '    ''' <summary>
    '    ''' Propiedad que permite manejar las opciones del modulo del combo cboModulo
    '    ''' </summary>
    '    ''' <remarks>Por: Giovanny Velez Bolivar - 19/05/2014</remarks>
    '    Private _Modulo As String = "ORDENES"
    '    <Display(Name:="Módulo")> _
    '    Public Property Modulo() As String
    '        Get
    '            Return _Modulo
    '        End Get
    '        Set(ByVal value As String)
    '            _Modulo = value
    '            MyBase.CambioItem("Modulo")
    '        End Set
    '    End Property

    '    Private _CantidadJustificaciones As Integer = 0
    '    Public Property CantidadJustificaciones() As Integer
    '        Get
    '            Return _CantidadJustificaciones
    '        End Get
    '        Set(ByVal value As Integer)
    '            _CantidadJustificaciones = value
    '            If CantidadJustificaciones > 0 Then
    '                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
    '                    EjecutarConfirmacion()
    '                End If
    '            End If
    '        End Set
    '    End Property

    '    Private _Confirmaciones As String = String.Empty
    '    Public Property Confirmaciones() As String
    '        Get
    '            Return _Confirmaciones
    '        End Get
    '        Set(ByVal value As String)
    '            _Confirmaciones = value
    '        End Set
    '    End Property

    '    Private _ConfirmacionesUsuario As String = String.Empty
    '    Public Property ConfirmacionesUsuario() As String
    '        Get
    '            Return _ConfirmacionesUsuario
    '        End Get
    '        Set(ByVal value As String)
    '            _ConfirmacionesUsuario = value
    '        End Set
    '    End Property

    '    Private _Justificaciones As String = String.Empty
    '    Public Property Justificaciones() As String
    '        Get
    '            Return _Justificaciones
    '        End Get
    '        Set(ByVal value As String)
    '            _Justificaciones = value
    '        End Set
    '    End Property

    '    Private _JustificacionesUsuario As String = String.Empty
    '    Public Property JustificacionesUsuario() As String
    '        Get
    '            Return _JustificacionesUsuario
    '        End Get
    '        Set(ByVal value As String)
    '            _JustificacionesUsuario = value
    '        End Set
    '    End Property

    '    Private _Aprobaciones As String
    '    Public Property Aprobaciones() As String
    '        Get
    '            Return _Aprobaciones
    '        End Get
    '        Set(ByVal value As String)
    '            _Aprobaciones = value
    '        End Set
    '    End Property

    '    Private _AprobacionesUsuario As String
    '    Public Property AprobacionesUsuario() As String
    '        Get
    '            Return _AprobacionesUsuario
    '        End Get
    '        Set(ByVal value As String)
    '            _AprobacionesUsuario = value
    '        End Set
    '    End Property
    '    Private _CantidadConfirmaciones As Integer = 0
    '    Public Property CantidadConfirmaciones() As Integer
    '        Get
    '            Return _CantidadConfirmaciones
    '        End Get
    '        Set(ByVal value As Integer)
    '            _CantidadConfirmaciones = value
    '            If CantidadConfirmaciones > 0 Then
    '                If CantidadConfirmaciones = cantidadTotalConfirmacion And CantidadJustificaciones = cantidadTotalJustificacion And CantidadAprobaciones = CantidadTotalAprobaciones Then
    '                    EjecutarConfirmacion()
    '                End If
    '            End If
    '        End Set
    '    End Property
    Private _ListaResultadoValidacion As List(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion)
    Public Property ListaResultadoValidacion() As List(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion)
        Get
            Return _ListaResultadoValidacion
        End Get
        Set(ByVal value As List(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion))
            _ListaResultadoValidacion = value
            MyBase.CambioItem("ListaResultadoValidacion")
        End Set
    End Property

    Private _DiccionarioEdicionCampos As Dictionary(Of String, Boolean)
    Public Property DiccionarioEdicionCampos() As Dictionary(Of String, Boolean)
        Get
            Return _DiccionarioEdicionCampos
        End Get
        Set(ByVal value As Dictionary(Of String, Boolean))
            _DiccionarioEdicionCampos = value
            MyBase.CambioItem("DiccionarioEdicionCampos")
        End Set
    End Property

    Private _ListaComboCamposPermitidosEdicion As List(Of CFCalculosFinancieros.Libranzas_CamposEditables)
    Public Property ListaComboCamposPermitidosEdicion() As List(Of CFCalculosFinancieros.Libranzas_CamposEditables)
        Get
            Return _ListaComboCamposPermitidosEdicion
        End Get
        Set(ByVal value As List(Of CFCalculosFinancieros.Libranzas_CamposEditables))
            _ListaComboCamposPermitidosEdicion = value
            MyBase.CambioItem("ListaComboCamposPermitidosEdicion")
        End Set
    End Property

    Private _VerAtras As Visibility = Visibility.Collapsed
    Public Property VerAtras() As Visibility
        Get
            Return _VerAtras
        End Get
        Set(ByVal value As Visibility)
            _VerAtras = value
            MyBase.CambioItem("VerAtras")
        End Set
    End Property

    Private _VerGrabar As Visibility = Visibility.Visible
    Public Property VerGrabar() As Visibility
        Get
            Return _VerGrabar
        End Get
        Set(ByVal value As Visibility)
            _VerGrabar = value
            MyBase.CambioItem("VerGrabar")
        End Set
    End Property

    Private WithEvents _Libranzas_ImportacionImportadasSelected As New CFCalculosFinancieros.Libranzas
    Public Property Libranzas_ImportacionImportadasSelected() As CFCalculosFinancieros.Libranzas
        Get
            Return _Libranzas_ImportacionImportadasSelected
        End Get
        Set(ByVal value As CFCalculosFinancieros.Libranzas)
            _Libranzas_ImportacionImportadasSelected = value
            MyBase.CambioItem("Libranzas_ImportacionImportadasSelected")

            Try
                If Not IsNothing(_Libranzas_ImportacionImportadasSelected) Then
                    MyBase.CambioItem("DiccionarioCombos")
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al realizar las consultas de las libranzas.", Me.ToString(), "Libranzas_ImportacionImportadasSelected", Application.Current.ToString(), Program.Maquina, ex)
                IsBusy = False
            End Try
        End Set
    End Property

    Private _DiccionarioCombos As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))
    Public Property DiccionarioCombos() As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))
        Get
            Return _DiccionarioCombos
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos)))
            _DiccionarioCombos = value
            MyBase.CambioItem("DiccionarioCombos")
        End Set
    End Property

    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
        End Set
    End Property

    Private _MensajeCantidadProcesadas As String = String.Empty
    Public Property MensajeCantidadProcesadas() As String
        Get
            Return _MensajeCantidadProcesadas
        End Get
        Set(ByVal value As String)
            _MensajeCantidadProcesadas = value
            MyBase.CambioItem("MensajeCantidadProcesadas")
        End Set
    End Property

    Private _FondoTextoBuscadorCliente As SolidColorBrush
    Public Property FondoTextoBuscadorCliente() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorCliente
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorCliente = value
            MyBase.CambioItem("FondoTextoBuscadorCliente")
        End Set
    End Property

    Private _FondoTextoBuscadorEmisor As SolidColorBrush
    Public Property FondoTextoBuscadorEmisor() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorEmisor
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorEmisor = value
            MyBase.CambioItem("FondoTextoBuscadorEmisor")
        End Set
    End Property

    Private _FondoTextoBuscadorPagador As SolidColorBrush
    Public Property FondoTextoBuscadorPagador() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorPagador
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorPagador = value
            MyBase.CambioItem("FondoTextoBuscadorPagador")
        End Set
    End Property

    Private _FondoTextoBuscadorCustodio As SolidColorBrush
    Public Property FondoTextoBuscadorCustodio() As SolidColorBrush
        Get
            Return _FondoTextoBuscadorCustodio
        End Get
        Set(ByVal value As SolidColorBrush)
            _FondoTextoBuscadorCustodio = value
            MyBase.CambioItem("FondoTextoBuscadorCustodio")
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Async Sub SeleccionarCliente(ByVal pobjCliente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjCliente) Then
                'If logEditarRegistro Or logNuevoRegistro Then

                If Not IsNothing(_Libranzas_ImportacionImportadasSelected) Then

                    _Libranzas_ImportacionImportadasSelected.lngIDComitente = pobjCliente.IdComitente
                    _Libranzas_ImportacionImportadasSelected.strNombreCliente = pobjCliente.Nombre
                    _Libranzas_ImportacionImportadasSelected.strNumeroDocumentoCliente = pobjCliente.NroDocumento
                    _Libranzas_ImportacionImportadasSelected.strCodTipoIdentificacionCliente = pobjCliente.CodTipoIdentificacion
                    _Libranzas_ImportacionImportadasSelected.strTipoIdentificacionCliente = pobjCliente.TipoIdentificacion
                End If
                'End If
            Else
                If Not IsNothing(_Libranzas_ImportacionImportadasSelected) Then
                    _Libranzas_ImportacionImportadasSelected.lngIDComitente = String.Empty
                    _Libranzas_ImportacionImportadasSelected.strNombreCliente = String.Empty
                    _Libranzas_ImportacionImportadasSelected.strNumeroDocumentoCliente = String.Empty
                    _Libranzas_ImportacionImportadasSelected.strCodTipoIdentificacionCliente = String.Empty
                    _Libranzas_ImportacionImportadasSelected.strTipoIdentificacionCliente = String.Empty
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar los datos del cliente.", Me.ToString, "SeleccionarCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Async Function CargarCombos() As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Libranzas_Combos)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            dcProxy.Libranzas_Combos.Clear()

            objRet = Await dcProxy.Load(dcProxy.Libranzas_ConsultarCombosSyncQuery(Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consultar los combos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar los combos.", Me.ToString(), "CargarCombos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.ToList.Count > 0 Then
                        Dim strNombreCategoria As String = String.Empty
                        Dim objListaNodosCategoria As List(Of CFCalculosFinancieros.Libranzas_Combos) = Nothing
                        Dim objDiccionario As New Dictionary(Of String, List(Of CFCalculosFinancieros.Libranzas_Combos))

                        Dim listaCategorias = From lc In objRet.Entities Select lc.Topico Distinct

                        For Each li In listaCategorias
                            strNombreCategoria = li
                            objListaNodosCategoria = (From ln In objRet.Entities Where ln.Topico = strNombreCategoria).ToList
                            objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                        Next

                        DiccionarioCombos = Nothing
                        DiccionarioCombos = objDiccionario

                    End If
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consultar los combos ", Me.ToString(), "CargarCombos", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Function ValidarCamposEditables() As Boolean
        Try
            Dim logRespuesta As Boolean = False
            Dim strMensajeValidacion = String.Empty

            If Not IsNothing(DiccionarioEdicionCampos) Then
                For Each li In DiccionarioEdicionCampos()
                    If li.Key = "FechaRegistro" And li.Value = True Then
                        If IsNothing(Libranzas_ImportacionImportadasSelected.dtmFechaRegistro) Then
                            strMensajeValidacion = String.Format("{0}{1} - Fecha Registro", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "NroCredito" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(Libranzas_ImportacionImportadasSelected.strNroCredito))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Nro. Crédito", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "IDComitente" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(Libranzas_ImportacionImportadasSelected.lngIDComitente))) Or Libranzas_ImportacionImportadasSelected.lngIDComitente = "-9999999999" Then
                            strMensajeValidacion = String.Format("{0}{1} - IDComitente", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "IDEmisor" And li.Value = True Then
                        If IsNothing(Libranzas_ImportacionImportadasSelected.intIDEmisor) Or (Libranzas_ImportacionImportadasSelected.intIDEmisor = 0) Then
                            strMensajeValidacion = String.Format("{0}{1} - IDEmisor", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "FechaInicioCredito" And li.Value = True Then
                        If IsNothing(Libranzas_ImportacionImportadasSelected.dtmFechaInicioCredito) Then
                            strMensajeValidacion = String.Format("{0}{1} - Fecha Inicio Crédito", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "NroCuotas" And li.Value = True Then
                        If IsNothing(Libranzas_ImportacionImportadasSelected.intNroCuotas) Or Libranzas_ImportacionImportadasSelected.intNroCuotas = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Nro. Cuotas", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "PeriodoPago" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(Libranzas_ImportacionImportadasSelected.strPeriodoPago))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Periodo Pago", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "ValorCreditoOriginal" And li.Value = True Then
                        If IsNothing(Libranzas_ImportacionImportadasSelected.dblValorCreditoOriginal) Or Libranzas_ImportacionImportadasSelected.dblValorCreditoOriginal = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Valor Crédito Original", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "TipoPago" And li.Value = True Then
                        If String.IsNullOrEmpty(LTrim(RTrim(Libranzas_ImportacionImportadasSelected.strTipoPago))) Then
                            strMensajeValidacion = String.Format("{0}{1} - Tipo Pago", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "TasaInteresCredito" And li.Value = True Then
                        If IsNothing(Libranzas_ImportacionImportadasSelected.dblTasaInteresCredito) Or Libranzas_ImportacionImportadasSelected.dblTasaInteresCredito = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Tasa Interes Crédito", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "TasaDescuento" And li.Value = True Then
                        If IsNothing(Libranzas_ImportacionImportadasSelected.dblTasaDescuento) Or Libranzas_ImportacionImportadasSelected.dblTasaDescuento = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - Tasa Descuento", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "NroDocumentoBeneficiario" And li.Value = True Then
                        If String.IsNullOrEmpty(Libranzas_ImportacionImportadasSelected.strNroDocumentoBeneficiario) Then
                            strMensajeValidacion = String.Format("{0}{1} - Nro. Documento Beneficiario  ", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "NombreBeneficiario" And li.Value = True Then
                        If String.IsNullOrEmpty(Libranzas_ImportacionImportadasSelected.strNombreBeneficiario) Then
                            strMensajeValidacion = String.Format("{0}{1} - Nombre Beneficiario  ", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                        'ElseIf li.Key = "TipoidentificacionBeneficiario" And li.Value = True Then
                        '    If String.IsNullOrEmpty(Libranzas_ImportacionImportadasSelected.strTipoIdentificacionBeneficiario) Then
                        '        strMensajeValidacion = String.Format("{0}{1} - Tipo Identificacion Beneficiario", strMensajeValidacion, vbCrLf)
                        '        logRespuesta = True
                        '    End If
                    ElseIf li.Key = "IDPagador" And li.Value = True Then
                        If IsNothing(Libranzas_ImportacionImportadasSelected.intIDPagador) Or Libranzas_ImportacionImportadasSelected.intIDPagador = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - IDPagador  ", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    ElseIf li.Key = "IDCustodio" And li.Value = True Then
                        If IsNothing(Libranzas_ImportacionImportadasSelected.intIDCustodio) Or Libranzas_ImportacionImportadasSelected.intIDCustodio = 0 Then
                            strMensajeValidacion = String.Format("{0}{1} - IDCustodio  ", strMensajeValidacion, vbCrLf)
                            logRespuesta = True
                        End If
                    End If
                Next
            End If

            If logRespuesta Then
                strMensajeValidacion = String.Format("Es necesario completar todos los campos :{0}{1}", vbCrLf, strMensajeValidacion)
                mostrarMensaje(strMensajeValidacion, "Carga Masivas Libranzas", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'Return False
                logRespuesta = False
            Else
                'Return True
                logRespuesta = True
            End If

            Return logRespuesta
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al validar los campos", Me.ToString, "ValidarCamposEditables", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Return False
        End Try
    End Function

    '    ''' <summary>
    '    ''' Valida los campos editables de la Libranza.
    '    ''' Desarrollado por: Jeison Ramirez Pino.
    '    ''' </summary>
    Public Sub ValidarCamposEditablesLibranza(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.Libranzas_CamposEditables) Then
                dcProxyConsulta.Libranzas_CamposEditables.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.Libranzas_ValidarHabilitarCamposQuery(Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoValidarCamposEditables, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los campos editables de la Libranzas.", Me.ToString(), "ValidarCamposEditablesOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub
    Public Sub VolverAtras()
        Try
            VerAtras = Visibility.Collapsed
            VerGrabar = Visibility.Collapsed
            Libranzas_ImportacionImportadasSelected = Nothing
            CargarContenido(TipoOpcionCargar.ARCHIVO)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema.", Me.ToString(), "VolverAtras", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Public Sub GrabarCarga()
        Try
            If ValidarCamposEditables() Then
                IsBusy = True
                If Not IsNothing(dcProxy.Libranzas_RespuestaValidacions) Then
                    dcProxy.Libranzas_RespuestaValidacions.Clear()
                End If

                dcProxy.Load(dcProxy.Libranzas_ImportacionValidarManualQuery(_Libranzas_ImportacionImportadasSelected.dtmFechaRegistro,
                                                                      _Libranzas_ImportacionImportadasSelected.strNroCredito,
                                                                      _Libranzas_ImportacionImportadasSelected.lngIDComitente,
                                                                      _Libranzas_ImportacionImportadasSelected.intIDCompania,
                                                                      _Libranzas_ImportacionImportadasSelected.intIDEmisor,
                                                                      _Libranzas_ImportacionImportadasSelected.dtmFechaInicioCredito,
                                                                      _Libranzas_ImportacionImportadasSelected.dtmFechaFinCredito,
                                                                      _Libranzas_ImportacionImportadasSelected.intNroCuotas,
                                                                      _Libranzas_ImportacionImportadasSelected.strPeriodoPago,
                                                                      _Libranzas_ImportacionImportadasSelected.dblValorCreditoOriginal,
                                                                      _Libranzas_ImportacionImportadasSelected.strTipoPago,
                                                                      _Libranzas_ImportacionImportadasSelected.dblTasaInteresCredito,
                                                                      _Libranzas_ImportacionImportadasSelected.dblTasaDescuento,
                                                                      _Libranzas_ImportacionImportadasSelected.dblValorOperacion,
                                                                      _Libranzas_ImportacionImportadasSelected.dblValorCreditoActual,
                                                                      _Libranzas_ImportacionImportadasSelected.strNroDocumentoBeneficiario,
                                                                      _Libranzas_ImportacionImportadasSelected.strNombreBeneficiario,
                                                                      _Libranzas_ImportacionImportadasSelected.strTipoIdentificacionBeneficiario,
                                                                      _Libranzas_ImportacionImportadasSelected.intIDPagador,
                                                                      _Libranzas_ImportacionImportadasSelected.intIDCustodio,
                                                                      _Libranzas_ImportacionImportadasSelected.strTipoRegistro,
                                                                      Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarIngreso, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al momento de validar campos habilitados", Me.ToString(), "GrabarCarga", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Try


            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            Dim objLibranzaNueva As New CFCalculosFinancieros.Libranzas

            Libranzas_ImportacionImportadasSelected = objLibranzaNueva
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del nuevo registro", Me.ToString(), "NuevoRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    '    ''' <summary>
    '    ''' Desarrollado por Jeison Ramirez Pino.
    '    ''' Se crea un metodo para ubicarse en la opción o campo que se encuentra vacio.
    '    '''
    '    ''' </summary>
    '    ''' <param name="pViewLibranzas"></param>
    '    ''' <param name="pstrOpcion"></param>
    '    ''' <remarks></remarks>
    Public Sub BuscarControlValidacion(ByVal pViewLibranzas As Libranzas_CargaMasivaCamposView, ByVal pstrOpcion As String)
        Try
            If Not IsNothing(pViewLibranzas) Then
                If TypeOf pViewLibranzas.FindName(pstrOpcion) Is TabItem Then
                    CType(pViewLibranzas.FindName(pstrOpcion), TabItem).IsSelected = True
                ElseIf TypeOf pViewLibranzas.FindName(pstrOpcion) Is TextBox Then
                    CType(pViewLibranzas.FindName(pstrOpcion), TextBox).Focus()
                ElseIf TypeOf pViewLibranzas.FindName(pstrOpcion) Is ComboBox Then
                    CType(pViewLibranzas.FindName(pstrOpcion), ComboBox).Focus()


                ElseIf TypeOf pViewLibranzas.FindName(pstrOpcion) Is A2Utilidades.A2NumericBox Then
                    CType(pViewLibranzas.FindName(pstrOpcion), A2Utilidades.A2NumericBox).Focus()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control dentro de las libranzas.", Me.ToString, "BuscarControlValidacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub CargarResultadosImportacion()
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.Libranzas_RespuestaValidacions) Then
                dcProxyConsulta.Libranzas_RespuestaValidacions.Clear()
            End If
            ListaResultadoValidacion = Nothing
            dcProxyConsulta.Load(dcProxyConsulta.Libranzas_ImportacionConsultarResultadosQuery(Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarResultado, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al validar los campos editables de la orden.", Me.ToString(), "ValidarCamposEditablesOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub CargarContenido(ByVal pobjTipoOpcionCargar As TipoOpcionCargar)
        Try
            If Not IsNothing(viewCargaMasiva.gridContenido.Children) Then
                viewCargaMasiva.gridContenido.Children.Clear()
            End If

            logRecargarResultados = False
            pararTemporizador()

            If pobjTipoOpcionCargar = TipoOpcionCargar.ARCHIVO Then

                Dim objContenidoCargar As Libranzas_CargaMasivaImportarArchivoView = New Libranzas_CargaMasivaImportarArchivoView(Me)
                NombreArchivo = String.Empty
                VerGrabar = Visibility.Collapsed
                viewCargaMasiva.gridContenido.Children.Add(objContenidoCargar)

            ElseIf pobjTipoOpcionCargar = TipoOpcionCargar.CAMPOS Then

                NuevoRegistro()
                Dim objContenidoCargar As Libranzas_CargaMasivaCamposView = New Libranzas_CargaMasivaCamposView(Me)
                viewCargaMasiva.gridContenido.Children.Add(objContenidoCargar)
                ValidarCamposEditablesLibranza()

            ElseIf pobjTipoOpcionCargar = TipoOpcionCargar.RESULTADO Then

                VerAtras = Visibility.Collapsed
                VerGrabar = Visibility.Collapsed

                Dim objContenidoCargar As Libranzas_CargaMasivaImportarResultadoView = New Libranzas_CargaMasivaImportarResultadoView(Me)
                logRecargarResultados = True
                viewCargaMasiva.gridContenido.Children.Add(objContenidoCargar)

            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al cargar el contenido.", Me.ToString(), "CargarContenido", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub ImportarArchivo()
        Try
            If String.IsNullOrEmpty(_NombreArchivo) Then
                mostrarMensaje("Debe de seleccionar un archivo para la importación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            End If

            dcProxyImportaciones.Load(dcProxyImportaciones.CargarArchivoImportacionLibranzasQuery(NombreArchivo, "ImpLibranzasMA", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoCargarArchivo, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al importar el archivo.", Me.ToString(), "ImportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ConfirmarValoresResultado()
        Try
            logRecargarResultados = False

            If Not IsNothing(ListaResultadoValidacion) Then
                If ListaResultadoValidacion.Count > 0 Then
                    '------------------------------------------------
                    Dim logConsultaListaJustificacion As Boolean = False
                    Dim logError As Boolean = False
                    Dim strMensajeExitoso As String = "Las libranzas se actualizaron correctamente."
                    Dim strMensajeError As String = "Las libranzas no se pudo actualizar."
                    Dim logEsHtml As Boolean = False
                    Dim strMensajeDetallesHtml As String = String.Empty
                    Dim strMensajeRetornoHtml As String = String.Empty
                    '------------------------------------------------
                    CargarContenido(TipoOpcionCargar.ARCHIVO)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar los resultados.", Me.ToString(), "ConfirmarValoresResultado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = True
        End Try
    End Sub

    Public Sub EjecutarConfirmacion()
        Try
            IsBusy = True
            CargarContenido(TipoOpcionCargar.ARCHIVO)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la confirmacion.", Me.ToString(), "EjecutarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = True
            IsBusy = False
        End Try
    End Sub

    Public Sub ConsultarCantidadProcesadas(Optional ByVal pstrUserState As String = "")
        Try
            IsBusy = True

            If Not IsNothing(dcProxy.Libranzas_CantidadProcesadas) Then
                dcProxy.Libranzas_CantidadProcesadas.Clear()
            End If

            dcProxy.Load(dcProxy.Libranzas_ImportacionCantidadProcesadasQuery(Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarCantidadProcesadas, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la cantidad procesadas.", Me.ToString(), "ConsultarCantidadProcesadas", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = True
            IsBusy = False
        End Try
    End Sub

    Public Sub ExportarExcel()
        Try
            IsBusy = True

            Dim strParametrosEnviar As String = String.Format("[@pstrUsuario]={0}|[@pstrUsuarioWindows]={1}|[@pstrMaquina]={2}", Program.Usuario, Program.UsuarioWindows, Program.Maquina)
            Dim strNombreArchivo = String.Format("ImportacionMasivaLibranzas_{0:yyyyMMddHHmmss}", Now)

            If Not IsNothing(mdcProxyUtilidad.GenerarArchivosPlanos) Then
                mdcProxyUtilidad.GenerarArchivosPlanos.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.GenerarArchivoPlanoQuery("ImpLibranzasMA", "ImpLibranzasMA", strParametrosEnviar, strNombreArchivo, "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarArchivoPlanoFondos, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la cantidad procesadas.", Me.ToString(), "ConsultarCantidadProcesadas", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            logRecargarResultados = True
            IsBusy = False
        End Try
    End Sub

    Public Sub CambiarColorFondoTextoBuscador()
        Try
            Dim colorFondo As Color

            If DiccionarioEdicionCampos("IDComitente") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadorCliente = New SolidColorBrush(colorFondo)
            '----------------------------------------------------------
            If DiccionarioEdicionCampos("IDEmisor") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadorEmisor = New SolidColorBrush(colorFondo)
            '----------------------------------------------------------
            If DiccionarioEdicionCampos("IDCustodio") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadorCustodio = New SolidColorBrush(colorFondo)
            '----------------------------------------------------------
            If DiccionarioEdicionCampos("IDPagador") Then
                colorFondo = Program.colorFromHex(COLOR_HABILITADO)
            Else
                colorFondo = Program.colorFromHex(COLOR_DESHABILITADO)
            End If
            FondoTextoBuscadorPagador = New SolidColorBrush(colorFondo)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el color de fondo del texto.", Me.ToString(), "CambiarColorFondoTextoBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarResultado(ByVal lo As LoadOperation(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion))
        Try
            If lo.HasError = False Then
                ListaResultadoValidacion = lo.Entities.ToList

                IsBusy = False
                logRecargarResultados = True
                ReiniciaTimer()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoConsultarResultado", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoConsultarResultado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub
    Private Sub TerminoConsultarCantidadProcesadas(ByVal lo As LoadOperation(Of CFCalculosFinancieros.Libranzas_CantidadProcesadas))
        Try
            If lo.HasError = False Then
                Dim objCantidadProcesadas As CFCalculosFinancieros.Libranzas_CantidadProcesadas = Nothing
                objCantidadProcesadas = lo.Entities.First

                MensajeCantidadProcesadas = String.Format("Cantidad de total de registros a procesar {1}{0}Cantidad de registros procesados {2}{0}Cantidad de registros pendientes por procesar {3}", vbCrLf, objCantidadProcesadas.CantidadTotalProcesar, objCantidadProcesadas.CantidadProcesados, objCantidadProcesadas.CantidadFaltantes)
                If objCantidadProcesadas.CantidadFaltantes > 0 Then
                    IsbusyResultados = True
                Else
                    IsbusyResultados = False
                End If

                If objCantidadProcesadas.ProcesoActivo Then
                    IsbusyResultados = True
                Else
                    IsbusyResultados = False
                End If

                CargarResultadosImportacion()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoEjecutarConfirmacion", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la consulta.", Me.ToString(), "TerminoEjecutarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
        IsBusy = False
    End Sub
    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper

                        Case "PREGUNTARCONFIRMACION"
                            'ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.CONFIRMACION, objResultado)
                        Case "PREGUNTARAPROBACION"
                            'ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.APROBACION, objResultado)
                        Case "PREGUNTARJUSTIFICACION"
                            'ValidarMensajesMostrarUsuario(TIPOMENSAJEUSUARIO.JUSTIFICACION, objResultado)
                        Case "INICIOPROCESOCONRESULTADOS"
                            If objResultado.DialogResult Then
                                logCargarContenido = False


                                CargarContenido(TipoOpcionCargar.RESULTADO)

                                logCargarContenido = True
                            Else
                                IsBusy = False
                                IsbusyResultados = False
                            End If
                    End Select
                End If
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje pregunta.", Me.ToString(), "TerminoMensajePregunta", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoMensajeResultadoAsincronico(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado = CType(sender, A2Utilidades.wcpMensajes)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper
                        Case "RESULTADOIMPORTACION"
                            ConsultarCantidadProcesadas(String.Empty)
                    End Select
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje asincronico.", Me.ToString(), "TerminoMensajeResultadoAsincronico", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoValidarIngreso(ByVal lo As LoadOperation(Of CFCalculosFinancieros.Libranzas_RespuestaValidacion))
        Try
            If lo.HasError = False Then

                ListaResultadoValidacion = lo.Entities.ToList

                If ListaResultadoValidacion.Count > 0 Then
                    Dim logExitoso As Boolean = False
                    Dim strMensajeExitoso As String = "Se realizo el proceso de carga exitosamente"
                    Dim strMensajeError As String = "El proceso de carga no se realizo"
                    Dim logError As Boolean = False

                    For Each li In ListaResultadoValidacion
                        If li.Exitoso Then
                            logExitoso = True
                            strMensajeExitoso = String.Format("{0}{1} {2}", strMensajeExitoso, vbCrLf, li.Mensaje)
                        ElseIf li.DetieneIngreso Then
                            logError = True
                            logExitoso = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)
                        Else
                            logError = True
                            logExitoso = False
                            strMensajeError = String.Format("{0}{1} -> {2}", strMensajeError, vbCrLf, li.Mensaje)

                        End If
                    Next

                    If logExitoso = True And logError = False Then
                        strMensajeExitoso = strMensajeExitoso.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeExitoso = strMensajeExitoso.Replace("--", String.Format("{0}", vbCrLf))
                        mostrarMensajeResultadoAsincronico(strMensajeExitoso, "Carga Masiva Libranzas", AddressOf TerminoMensajeResultadoAsincronico, "RESULTADOIMPORTACION", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        CargarContenido(TipoOpcionCargar.RESULTADO)
                    ElseIf logError Then
                        strMensajeError = strMensajeError.Replace("++", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("|", String.Format("{0}   -> ", vbCrLf))
                        strMensajeError = strMensajeError.Replace("--", String.Format("{0}", vbCrLf))

                        mostrarMensaje(strMensajeError, "Carga Masiva Libranzas", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                End If
            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngreso", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de las validación.", Me.ToString(), "TerminoValidarIngreso", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)

            IsBusy = False
        End Try
    End Sub
    Private Sub TerminoValidarCamposEditables(ByVal lo As LoadOperation(Of CFCalculosFinancieros.Libranzas_CamposEditables))
        Try
            If lo.HasError = False Then
                ListaComboCamposPermitidosEdicion = lo.Entities.ToList

                Dim objDiccionarioCamposEditables As New Dictionary(Of String, Boolean)

                For Each li In ListaComboCamposPermitidosEdicion
                    If Not objDiccionarioCamposEditables.ContainsKey(li.NombreCampo) Then
                        objDiccionarioCamposEditables.Add(li.NombreCampo, li.PermiteEditar)
                    End If
                Next

                DiccionarioEdicionCampos = objDiccionarioCamposEditables

                CambiarColorFondoTextoBuscador()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar los campos editables.", Me.ToString(), "TerminoValidarCamposEditables", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar los campos editables.", Me.ToString(), "TerminoValidarCamposEditables", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub
    Private Sub TerminoCargarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If lo.HasError = False Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim ViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                Dim logContinuar As Boolean = False

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    Dim objTipo As String = String.Empty

                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
                        objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")
                    End If

                    For Each li In objListaRespuesta.OrderBy(Function(o) o.Tipo)
                        If objTipo <> li.Tipo And li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                            objTipo = li.Tipo
                            objListaMensajes.Add(String.Format("Hoja {0}", li.Tipo))
                        ElseIf li.Tipo = "REGISTROSIMPORTADOS" Then
                            If li.Columna > 0 Then
                                logContinuar = True
                            End If
                        End If

                        If li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                            objListaMensajes.Add(String.Format("Fila: {0} - Campo {1} - Validación: {2}", li.Fila, li.Campo, li.Mensaje))
                        Else
                            objListaMensajes.Add(li.Mensaje)
                        End If
                    Next

                    ViewImportarArchivo.ListaMensajes = objListaMensajes
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo libranzas masivas"
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo libranzas masivas"
                End If

                Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
                ViewImportarArchivo.ShowDialog()

                If logContinuar Then
                    VerAtras = Visibility.Visible
                    CargarContenido(TipoOpcionCargar.CAMPOS)
                    VerGrabar = Visibility.Visible
                Else
                    VerGrabar = Visibility.Collapsed
                End If
                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de libranzas.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de libranzas.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub
    Private Sub TerminoGenerarArchivoPlanoFondos(ByVal lo As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.First.Exitoso Then
                        Program.VisorArchivosWeb_DescargarURL(lo.Entities.First.RutaArchivoPlano)

                        IsBusy = False
                    Else
                        mostrarMensaje(lo.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                Else
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "TerminoGenerarArchivoPlanoFondos", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "TerminoGenerarArchivoPlanoFondos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Timer Refrescar pantalla"

    Private _myDispatcherTimerOrdenes As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

    Public Sub ReiniciaTimer()
        Try
            If _myDispatcherTimerOrdenes Is Nothing Then
                _myDispatcherTimerOrdenes = New System.Windows.Threading.DispatcherTimer
                _myDispatcherTimerOrdenes.Interval = New TimeSpan(0, 0, 15)
                AddHandler _myDispatcherTimerOrdenes.Tick, AddressOf Me.Each_Tick
            End If
            _myDispatcherTimerOrdenes.Start()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' Para hilo del temporizador
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub pararTemporizador()
        Try
            If Not IsNothing(_myDispatcherTimerOrdenes) Then
                _myDispatcherTimerOrdenes.Stop()
                RemoveHandler _myDispatcherTimerOrdenes.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerOrdenes = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar parar el timer.", Me.ToString, "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Each_Tick(sender As Object, e As EventArgs)
        'Recarga la pantalla cuando se este en un tiempo de espera sin notificaciones y sin refrescar la pantalla.
        If logRecargarResultados Then
            ConsultarCantidadProcesadas(String.Empty)
        End If
    End Sub

#End Region

    Private Sub _Libranzas_ImportacionImportadasSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _Libranzas_ImportacionImportadasSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()

            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_Libranzas_ImportacionImportadasSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

End Class