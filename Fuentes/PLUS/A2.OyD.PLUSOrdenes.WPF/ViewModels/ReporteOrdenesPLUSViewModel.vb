Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web


Imports System.Threading.Tasks
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Xml
Imports A2ControlMenu
Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA
Imports System.Globalization
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports A2MCCoreWPF
Imports A2OyDPLUSUtilidades
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports OpenRiaServices.DomainServices.Client

Public Class ReporteOrdenesPLUSViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Constantes"
    Private Const STR_CARPETAPROCESO As String = "ReporteOrdenesPlus"
    Private logMostrarReporte As Boolean = False '--DARM 20140613



    Private Const STR_RUTAREPORTE As String = "[SERVIDOR]/Pages/ReportViewer.aspx?[REPORTE]&pstrIDReceptor=[IDRECEPTOR]&plngComitenteDesde=[COMITENTEDESDE]&plngComitenteHasta=[COMITENTEHASTA]&pstrEspecieDesde=[ESPECIEDESDE]&pstrEspecieHasta=[ESPECIEHASTA]&pdtmDesde=[FECHADESDE]&pdtmHasta=[FECHAHASTA]&pstrUsuario=[USUARIO]&rs:Command=Render&rc:Parameters=false&rs:ParameterLanguage=[CULTURA]"
    Private Const STR_RUTAREPORTE_Trazabilidad As String = "[SERVIDOR]/Pages/ReportViewer.aspx?[REPORTE]&pstrIDReceptor=[IDRECEPTOR]&plngComitenteDesde=[COMITENTEDESDE]&plngComitenteHasta=[COMITENTEHASTA]&pstrEspecieDesde=[ESPECIEDESDE]&pstrEspecieHasta=[ESPECIEHASTA]&pdtmDesde=[FECHADESDE]&pdtmHasta=[FECHAHASTA]&pstrModulo=[MODULO]&pstrUsuario=[USUARIO]&rs:Command=Render&rc:Parameters=false&rs:ParameterLanguage=[CULTURA]"
    Private Const STR_RUTAREPORTE_Cumplimiento As String = "[SERVIDOR]/Pages/ReportViewer.aspx?[REPORTE]&pstrIDReceptor=[IDRECEPTOR]&plngComitenteDesde=[COMITENTEDESDE]&plngComitenteHasta=[COMITENTEHASTA]&pstrEspecieDesde=[ESPECIEDESDE]&pstrEspecieHasta=[ESPECIEHASTA]&pdtmDesde=[FECHADESDE]&pdtmHasta=[FECHAHASTA]&pstrModulo=[MODULO]&pstrUsuario=[USUARIO]&rs:Command=Render&rc:Parameters=false&rs:ParameterLanguage=[CULTURA]"
    Private Const STR_PARAMETROS As String = "[@pstrIDReceptor]=[IDRECEPTOR]|[@plngComitenteDesde]=[COMITENTEDESDE]|[@plngComitenteHasta]=[COMITENTEHASTA]|[@pstrEspecieDesde]=[ESPECIEDESDE]|[@pstrEspecieHasta]=[ESPECIEHASTA]|[@pdtmDesde]=[FECHADESDE]|[@pdtmHasta]=[FECHAHASTA]|[@pstrUsuario]=[USUARIO]"
    Private Const STR_PARAMETROS_TRAZA As String = "[@pstrIDReceptor]=[IDRECEPTOR]|[@plngComitenteDesde]=[COMITENTEDESDE]|[@plngComitenteHasta]=[COMITENTEHASTA]|[@pstrEspecieDesde]=[ESPECIEDESDE]|[@pstrEspecieHasta]=[ESPECIEHASTA]|[@pdtmDesde]=[FECHADESDE]|[@pdtmHasta]=[FECHAHASTA]|[@pstrModulo]=[MODULO]|[@pstrUsuario]=[USUARIO]"
    Private Const STR_PARAMETROS_CUMPLI As String = "[@pstrIDReceptor]=[IDRECEPTOR]|[@plngComitenteDesde]=[COMITENTEDESDE]|[@plngComitenteHasta]=[COMITENTEHASTA]|[@pstrEspecieDesde]=[ESPECIEDESDE]|[@pstrEspecieHasta]=[ESPECIEHASTA]|[@pdtmDesde]=[FECHADESDE]|[@pdtmHasta]=[FECHAHASTA]|[@pstrModulo]=[MODULO]|[@pstrUsuario]=[USUARIO]"

    Private Enum PARAMETROSRUTA
        SERVIDOR
        REPORTE
        IDRECEPTOR
        COMITENTEDESDE
        COMITENTEHASTA
        ESPECIEDESDE
        ESPECIEHASTA
        MODULO
        FECHADESDE
        FECHAHASTA
        USUARIO
        CULTURA
    End Enum

    Private Enum TipoRespuestaImportacion
        Exitosa
        Errores
        Inconsistencias
        Advertencias
    End Enum

    Private Enum TabGeneral
        Origen
        Importacion
    End Enum

    Private Enum ReporteInconsistencias
        Ninguno
        Activos
        Comprobantes
        CCosto
        Cuentas
        Kardex
        Compañias
        Nits
        HomologacionCCostos
        HomologacionCuentas
        ActivosDetalles
    End Enum

#End Region

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private dcProxy As OYDPLUSOrdenesDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Private mdcProxyUtilidadPLUS As OYDPLUSUtilidadesDomainContext
    Private objCulturaServidor As System.Globalization.CultureInfo
    Private NombreReporte As String = String.Empty
#End Region

#Region "Inicialización - REQUERIDO"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' 
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
                mdcProxyUtilidadPLUS = New OYDPLUSUtilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                mdcProxyUtilidadPLUS = New OYDPLUSUtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYDPLUS)))
            End If
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.OYDPLUSOrdenesDomainContext.IOYDPLUSOrdenesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            NombreReceptor = GSTR_TODOS
            CodigoReceptor = GSTR_ID_TODOS
            'CargarReceptoresUsuarioOYDPLUS("INICIO")
            CargarCombosOYDPLUS(String.Empty, String.Empty)
        Catch ex As Exception

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ReporteOrdenesPLUSViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' 



#End Region

#Region "Propiedades - REQUERIDO"
    Private _Comitente As String
    Public Property Comitente() As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            MyBase.CambioItem("Comitente")
        End Set
    End Property
    Private _Especie As String
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            MyBase.CambioItem("Especie")
        End Set
    End Property
    Private _Especie2 As String
    Public Property Especie2() As String
        Get
            Return _Especie2
        End Get
        Set(ByVal value As String)
            _Especie2 = value
            MyBase.CambioItem("Especie2")
        End Set
    End Property
    Private _FechaDesde As DateTime = Now
    Public Property FechaDesde() As DateTime
        Get
            Return _FechaDesde
        End Get
        Set(ByVal value As DateTime)
            _FechaDesde = value
            MyBase.CambioItem("FechaDesde")
        End Set
    End Property
    Private _FechaHasta As DateTime = Now
    Public Property FechaHasta() As DateTime
        Get
            Return _FechaHasta
        End Get
        Set(ByVal value As DateTime)
            _FechaHasta = value
            MyBase.CambioItem("FechaHasta")
        End Set
    End Property
    Private _NombreReceptor As String
    Public Property NombreReceptor() As String
        Get
            Return _NombreReceptor
        End Get
        Set(ByVal value As String)
            _NombreReceptor = value
            MyBase.CambioItem("NombreReceptor")
        End Set
    End Property

    Private _idReceptorCliente As String
    Public Property idReceptorCliente() As String
        Get
            Return _idReceptorCliente
        End Get
        Set(ByVal value As String)
            _idReceptorCliente = value
            MyBase.CambioItem("idReceptorCliente")
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



    Private _CodigoReceptor As String
    Public Property CodigoReceptor() As String
        Get
            Return _CodigoReceptor
        End Get
        Set(ByVal value As String)
            _CodigoReceptor = value
            BorrarCliente = True
            BorrarCliente = False
            Comitente = String.Empty
            If Not String.IsNullOrEmpty(_CodigoReceptor) Then
                HabilitarCliente = True
                idReceptorCliente = _CodigoReceptor
                If _CodigoReceptor = GSTR_ID_TODOS Then
                    idReceptorCliente = String.Empty
                End If
            End If
            MyBase.CambioItem("CodigoReceptor")
        End Set
    End Property
    Private _ListaReceptoresUsuario As List(Of OYDPLUSUtilidades.tblReceptoresUsuario)
    Public Property ListaReceptoresUsuario() As List(Of OYDPLUSUtilidades.tblReceptoresUsuario)
        Get
            Return _ListaReceptoresUsuario
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblReceptoresUsuario))
            _ListaReceptoresUsuario = value
            MyBase.CambioItem("ListaReceptoresUsuario")
        End Set
    End Property

    ''' <summary>
    ''' Descripción que presenta la información de la compañía activa para que se muestre al inicio de la forma
    ''' </summary>
    ''' 


    Private _RutaPorDefectoArchivo As String
    Public Property RutaPorDefectoArchivo() As String
        Get
            Return _RutaPorDefectoArchivo
        End Get
        Set(ByVal value As String)
            _RutaPorDefectoArchivo = value
            MyBase.CambioItem("RutaPorDefectoArchivo")
        End Set
    End Property

    Private _TabInformacionGeneral As Integer = 0
    Public Property TabInformacionGeneral() As Integer
        Get
            Return (_TabInformacionGeneral)
        End Get
        Set(ByVal value As Integer)
            _TabInformacionGeneral = value
            MyBase.CambioItem("TabInformacionGeneral")
        End Set
    End Property

    Private _TipoImportacion As String
    Public Property TipoImportacion() As String
        Get
            Return (_TipoImportacion)
        End Get
        Set(ByVal value As String)
            _TipoImportacion = value
            MyBase.CambioItem("TipoImportacion")
        End Set
    End Property

    Private _SeparadorCampos As String
    Public Property SeparadorCampos() As String
        Get
            Return (_SeparadorCampos)
        End Get
        Set(ByVal value As String)
            _SeparadorCampos = value
            MyBase.CambioItem("SeparadorCampos")
        End Set
    End Property

    Private _DiccionarioCombosOYDPlus As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
    Public Property DiccionarioCombosOYDPlus() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
        Get
            Return _DiccionarioCombosOYDPlus
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
            _DiccionarioCombosOYDPlus = value
            MyBase.CambioItem("DiccionarioCombosOYDPlus")
        End Set
    End Property
    Private _DiccionarioCombosOYDPlusCompleta As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
    Public Property DiccionarioCombosOYDPlusCompleta() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
        Get
            Return _DiccionarioCombosOYDPlusCompleta
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
            _DiccionarioCombosOYDPlusCompleta = value
            MyBase.CambioItem("DiccionarioCombosOYDPlusCompleta")
        End Set
    End Property
    Private _RutaArchivo As String
    Public Property RutaArchivo() As String
        Get
            Return (_RutaArchivo)
        End Get
        Set(ByVal value As String)
            _RutaArchivo = value
            MyBase.CambioItem("RutaArchivo")
        End Set
    End Property

    Private _NombreArchivo As String
    Public Property NombreArchivo() As String
        Get
            Return (_NombreArchivo)
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property
    Private _TipoReporte As String
    Public Property TipoReporte() As String
        Get
            Return _TipoReporte
        End Get
        Set(ByVal value As String)
            _TipoReporte = value
            If Not String.IsNullOrEmpty(_Modulo) And Not String.IsNullOrEmpty(TipoReporte) Then
                HabilitarControles = True
            End If

            MyBase.CambioItem("TipoReporte")
        End Set
    End Property


    Private _Modulo As String
    Public Property Modulo() As String
        Get
            Return _Modulo
        End Get
        Set(ByVal value As String)
            _Modulo = value
            If Not String.IsNullOrEmpty(_Modulo) And Not String.IsNullOrEmpty(TipoReporte) Then
                HabilitarControles = True
                If _Modulo <> "DI" And _Modulo <> "DE" Then
                    HabilitarEspecies = True
                Else
                    HabilitarEspecies = False
                    BorrarEspecie = True
                    Especie = String.Empty
                    Especie2 = String.Empty
                End If
            End If

            MyBase.CambioItem("HabilitarControles")
        End Set
    End Property


    Private _HabilitarControles As Boolean = False
    Public Property HabilitarControles() As Boolean
        Get
            Return _HabilitarControles
        End Get
        Set(ByVal value As Boolean)
            If Not String.IsNullOrEmpty(_Modulo) And Not String.IsNullOrEmpty(TipoReporte) Then
                _HabilitarControles = True
            Else
                _HabilitarControles = value
            End If

            MyBase.CambioItem("HabilitarControles")
        End Set
    End Property

    Private _HabilitarCliente As Boolean = False
    Public Property HabilitarCliente() As Boolean
        Get
            Return (_HabilitarCliente)
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCliente = value
            MyBase.CambioItem("HabilitarCliente")
        End Set
    End Property

    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return (_BorrarEspecie)
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            MyBase.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private _HabilitarEspecies As Boolean = False
    Public Property HabilitarEspecies() As Boolean
        Get
            Return (_HabilitarEspecies)
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEspecies = value
            MyBase.CambioItem("HabilitarEspecies")
        End Set
    End Property

#End Region

#Region "Metodos - REQUERIDO"
  
    Private Sub TerminoGenerarExcel(ByVal lo As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos))
        Try
            IsBusy = False
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.First.Exitoso Then
                        Program.VisorArchivosWeb_DescargarURL(lo.Entities.First.RutaArchivoPlano)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Se presentó un error en el momento de generar el archivo plano." & lo.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error en el momento de generar el archivo plano.", _
                                 Me.ToString(), "TerminoGenerarArchivoPlano", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub GenerarExcel()
        Try
            If ValidarParamtros() Then
                Dim strParametrosReporte As String = String.Empty
                Dim strProceso As String = String.Empty
                If TipoReporte = "RO" Then
                    strParametrosReporte = STR_PARAMETROS
                ElseIf TipoReporte = "RT" Then
                    strParametrosReporte = STR_PARAMETROS_TRAZA
                ElseIf TipoReporte = "RC" Then
                    strParametrosReporte = STR_PARAMETROS_CUMPLI
                End If

                If TipoReporte = "RO" And Modulo = "B" Then
                    strProceso = "Ordenes Bolsa Plus"
                ElseIf TipoReporte = "RO" And Modulo = "OF" Then
                    strProceso = "Ordenes Otras Firmas Plus"
                ElseIf TipoReporte = "RO" And Modulo = "DI" Then
                    strProceso = "Ordenes Divisas Plus"
                ElseIf TipoReporte = "RO" And Modulo = "DE" Then
                    strProceso = "Ordenes Derivados Plus"
                End If
                If TipoReporte = "RC" Then
                    strProceso = "Ordenes Cumplimiento Plus"
                ElseIf TipoReporte = "RT" Then
                    strProceso = "Ordenes Trazabilidad Plus"
                End If









                Dim strParametrosEnviar As String = strParametrosReporte
                Dim pstrNombreArchivo As String = strProceso & "_" & Now.ToString("yyyyMMddHHmmss")

                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.IDRECEPTOR), CodigoReceptor)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.COMITENTEDESDE), IIf(String.IsNullOrEmpty(Comitente), "T", Comitente))
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.COMITENTEHASTA), IIf(String.IsNullOrEmpty(Comitente), "T", Comitente))
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.ESPECIEDESDE), Especie)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.ESPECIEHASTA), Especie2)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.FECHADESDE), FechaDesde.ToString("yyyy-MM-dd"))
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.FECHAHASTA), FechaHasta.ToString("yyyy-MM-dd"))
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.USUARIO), Program.Usuario)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.MODULO), Modulo)

                If Not IsNothing(mdcProxyUtilidad.GenerarArchivosPlanos) Then
                    mdcProxyUtilidad.GenerarArchivosPlanos.Clear()
                End If
                IsBusy = True
                mdcProxyUtilidad.Load(mdcProxyUtilidad.GenerarArchivoPlanoQuery(STR_CARPETAPROCESO, strProceso, strParametrosEnviar, pstrNombreArchivo, ",", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarExcel, String.Empty)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó error al generar excel.", _
                        Me.ToString(), "GenerarExcel", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub
    Public Function ValidarParamtros() As Boolean
        Try
            Dim logTieneError As Boolean = False
            Dim strMensajeValidacion = String.Empty
            If String.IsNullOrEmpty(TipoReporte) Then
                strMensajeValidacion = String.Format("{0}{1} - Tipo reporte", strMensajeValidacion, vbCrLf)
                logTieneError = True
            End If
            If String.IsNullOrEmpty(Modulo) Then
                strMensajeValidacion = String.Format("{0}{1} - Modulo", strMensajeValidacion, vbCrLf)
                logTieneError = True
            End If
            If String.IsNullOrEmpty(CodigoReceptor) Then
                strMensajeValidacion = String.Format("{0}{1} - Receptor", strMensajeValidacion, vbCrLf)
                logTieneError = True
            End If
            'If String.IsNullOrEmpty(Comitente) Then
            '    strMensajeValidacion = String.Format("{0}{1} - Comitente", strMensajeValidacion, vbCrLf)
            '    logTieneError = True
            'End If

            'If Modulo = "DI" Or Modulo = "DE" Then
            '    If Not String.IsNullOrEmpty(Especie) Then
            '        strMensajeValidacion = String.Format("{0}{1} - Para el Modulo seleccionado NO aplica especies en los filtros", strMensajeValidacion, vbCrLf)
            '        logTieneError = True
            '    End If
            '    If Not String.IsNullOrEmpty(Especie2) Then
            '        strMensajeValidacion = String.Format("{0}{1} - Para el Modulo seleccionado NO aplica especies en los filtros", strMensajeValidacion, vbCrLf)
            '        logTieneError = True
            '    End If
            'End If
            If Modulo <> "DI" And Modulo <> "DE" Then
                If Not String.IsNullOrEmpty(Especie) Then
                    If String.IsNullOrEmpty(Especie2) Then
                        strMensajeValidacion = String.Format("{0}{1} - Ambas Especies deben contener Valor", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                ElseIf Not String.IsNullOrEmpty(Especie2) Then
                    If String.IsNullOrEmpty(Especie) Then
                        strMensajeValidacion = String.Format("{0}{1} - Ambas Especies deben contener Valor", strMensajeValidacion, vbCrLf)
                        logTieneError = True
                    End If
                End If
            End If

            If String.IsNullOrEmpty(Especie) Then
                Especie = " "
            End If
            If String.IsNullOrEmpty(Especie2) Then
                Especie2 = " "
            End If
            If logTieneError Then
                strMensajeValidacion = String.Format("Es necesario completar todos los datos para ejecutar el reporte :{0}{1}", vbCrLf, strMensajeValidacion)
                mostrarMensaje(strMensajeValidacion, "Reportes Ordenes Plus", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False

            Else
                Return True
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub CargarReceptoresUsuarioOYDPLUS(ByVal pstrUserState As String)
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidadPLUS.tblReceptoresUsuarios) Then
                mdcProxyUtilidadPLUS.tblReceptoresUsuarios.Clear()
            End If

            mdcProxyUtilidadPLUS.Load(mdcProxyUtilidadPLUS.OYDPLUS_ConsultarReceptoresUsuarioQuery(True, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptoresUsuario, pstrUserState)


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se  presentó error al cargar los receptores del usuario.", _
                                 Me.ToString(), "CargarReceptoresUsuario", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub VerReporte()
        Try
            If ValidarParamtros() Then


                'Construye la ruta para el link del reporte de inconsistencias
                Dim strParametrosEnviar As String = STR_RUTAREPORTE
                Dim strRutaServidor As String = Program.ServidorReportesEjecution & "/"
                Dim strRutaNombreReporte As String = Program.CarpetaReportes
                Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString
                Dim strIDTabla As String = String.Empty

                If Right(strRutaServidor, 1) = "/" Then
                    strRutaServidor = Left(strRutaServidor, Len(strRutaServidor) - 1)
                End If

                If Right(strRutaNombreReporte, 1) <> "/" Then
                    strRutaNombreReporte = strRutaNombreReporte & "/"
                End If

                If TipoReporte = "RO" Then
                    If Modulo = "B" Then
                        NombreReporte = "Ordenes OyD Bolsa Plus"
                    ElseIf Modulo = "OF" Then
                        NombreReporte = "Ordenes Otras Firmas Plus"
                    ElseIf Modulo = "DI" Then
                        NombreReporte = "Ordenes Divisas Plus"
                    ElseIf Modulo = "DE" Then
                        NombreReporte = "Ordenes Derivados Plus"
                    End If
                ElseIf TipoReporte = "RT" Then
                    strParametrosEnviar = STR_RUTAREPORTE_Trazabilidad
                    NombreReporte = "Ordenes Trazabilidad Plus"
                ElseIf TipoReporte = "RC" Then
                    strParametrosEnviar = STR_RUTAREPORTE_Cumplimiento
                    NombreReporte = "Ordenes Cumplimiento Plus"
                End If







                strRutaNombreReporte = strRutaNombreReporte & NombreReporte
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.IDRECEPTOR), CodigoReceptor)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.SERVIDOR), strRutaServidor)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.REPORTE), strRutaNombreReporte)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.COMITENTEDESDE), IIf(String.IsNullOrEmpty(Comitente), "T", Comitente))
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.COMITENTEHASTA), IIf(String.IsNullOrEmpty(Comitente), "T", Comitente))
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.ESPECIEDESDE), Especie)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.ESPECIEHASTA), Especie2)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.FECHADESDE), FechaDesde.ToString("yyyy-MM-dd"))
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.FECHAHASTA), FechaHasta.ToString("yyyy-MM-dd"))
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.USUARIO), Program.Usuario)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.MODULO), Modulo)
                strParametrosEnviar = strParametrosEnviar.Replace(ValorAReemplazar(PARAMETROSRUTA.CULTURA), Program.CulturaReportes)

                Program.VisorArchivosWeb_CargarURL(strParametrosEnviar)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al visualizar el reporte de inconsistencias.", "VerReporteInconsistencias", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSRUTA) As String
        Return String.Format("[{0}]", pintTipoCampo.ToString)
    End Function

    Public Sub CargarCombosOYDPLUS(ByVal pstrIDReceptor As String, ByVal pstrUserState As String)

        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidadPLUS.CombosReceptors) Then
                mdcProxyUtilidadPLUS.CombosReceptors.Clear()
            End If

            mdcProxyUtilidadPLUS.Load(mdcProxyUtilidadPLUS.OYDPLUS_ConsultarCombosReceptorQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYDCompleta, pstrUserState)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.", _
                                 Me.ToString(), "CargarCombosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ObtenerValoresCombos(ByVal ValoresCompletos As Boolean)
        Try
            Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
            Dim objListaCategoria As New List(Of OYDPLUSUtilidades.CombosReceptor)
            Dim objListaCategoria1 As New List(Of OYDPLUSUtilidades.CombosReceptor)

            If ValoresCompletos Then
                '************************************************************************************
                If Not IsNothing(DiccionarioCombosOYDPlusCompleta) Then
                    For Each li In DiccionarioCombosOYDPlusCompleta
                        objDiccionario.Add(li.Key, li.Value)
                    Next
                End If

                If Not IsNothing(objListaCategoria) Then
                    objListaCategoria.Clear()
                End If


            Else
                If Not IsNothing(DiccionarioCombosOYDPlus) Then
                    For Each li In DiccionarioCombosOYDPlus
                        objDiccionario.Add(li.Key, li.Value)
                    Next
                End If

                If Not IsNothing(objListaCategoria) Then
                    objListaCategoria.Clear()
                End If

            End If

            DiccionarioCombosOYDPlus = objDiccionario




        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al obtener los valores de la clasificación.", _
                                 Me.ToString(), "ObtenerValoresCombos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub
#End Region

#Region "Resultados asincronicos"

    Private Sub TerminoConsultarReceptoresUsuario(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblReceptoresUsuario))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaReceptoresUsuario = lo.Entities.ToList
                Else

                    mostrarMensaje("No hay ningun receptor configurado para el Usuario Actual Logueado.", "Reporte Ordenes", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                    IsBusy = False
                End If


            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)


                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener los receptores del usuario.", Me.ToString(), "TerminoConsultarReceptoresUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
        IsBusy = False
    End Sub
    Private Sub TerminoConsultarCombosOYDCompleta(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    IsBusy = False
                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
                    Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionarioCompleto.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    If Not IsNothing(DiccionarioCombosOYDPlus) Then
                        DiccionarioCombosOYDPlus.Clear()
                    End If

                    DiccionarioCombosOYDPlusCompleta = Nothing
                    DiccionarioCombosOYDPlusCompleta = objDiccionarioCompleto

                    ObtenerValoresCombos(True)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
        IsBusy = False
    End Sub
#End Region

End Class



