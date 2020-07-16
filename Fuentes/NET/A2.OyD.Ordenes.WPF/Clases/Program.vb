Imports Telerik.Windows.Controls
Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports System.Reflection
Imports System.Globalization
Imports A2.OyD.OYDServer.RIA.Web

#Const ES_PRUEBA = False
'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Public Class Program

    Inherits A2Utilidades.Program

    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_PARAM_RUTA_SERV_ORDENES As String = "URLServicioOrdenes"
    Friend Const NOMBRE_PARAM_RUTA_SERV_MAESTROS As String = "URLServicioMaestros"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_PARAM_RUTA_VISORSETEADOR As String = "URLVisorSeteador"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const NOMBRE_DICCIONARIO_COMBOS As String = "DiccionarioCombosA2"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS As String = "DiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"
    Friend Const NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES As String = "URLServicioImportaciones"
    Friend Const HABILITAR_RECARGA As String = "HabilitarRecargaAutomatica"
    Friend Const LAPSO_RECARGA As String = "LapsoRecargaAutomatica"
    Friend Const VELOCIDADTEXTO_TICKER As String = "VelocidadTicker"
    Friend Const VERSION_APLICACION_CLIENTE As String = "VersionAplicacionCliente"
    Friend Const NOMBRE_PARAM_RUTA_SERV_BOLSA As String = "URLServicioBolsa"
    Friend Const NOMBRE_PARAM_RUTA_URL_FILE_UPLOADS As String = "URLFileUploads"
    Friend Const NOMBRE_PARAM_RUTA_SERV_RECOMPLEMENTACION As String = "URLServicioRecomplementacion"
    Friend Const NOMBRE_PARAM_SERVIDOR_REPORTES As String = "A2VServicioRS"
    Friend Const NOMBRE_PARAM_SERVIDOR_REPORTESEJECUTION As String = "OYDPlusReporte"
    Friend Const NOMBRE_PARAM_CARPETAREPORTES As String = "A2CarpetaReportes"
    Public Const NOMBRE_PARAM_CULTURA_REPORTES As String = "A2VCultura"

#Region "Parámetros para la página del seteador"
    Friend Const ESTADO_ORDEN_PENDIENTE As String = "EstOrdenPendiente"
    Friend Const ESTADO_BOLSA_RECIBIDA As String = "EstBolsaRecibida"
    Friend Const ESTADO_BOLSA_LANZADA As String = "EstBolsaLanzada"
    Friend Const ESTADO_BOLSA_COMPLEMENTADA As String = "EstBolsaComplementada"
    Friend Const ESTADO_VISOR_POR_ENVIAR As String = "EstVisorPorEnviar"
    Friend Const ESTADO_VISOR_ENVIADA_SAE As String = "EstVisorEnviadaSAE"
    Friend Const ESTADO_VISOR_MOSTRADA_EN_VISOR As String = "EstVisorMostradaVisor"
    Friend Const ESTADO_VISOR_EN_BVC_SIN_RESPUESTA As String = "EstVisorEnBVCSinRespuesta"
    Friend Const ESTADO_VISOR_RECIBIDA_SAE As String = "EstVisorRecibidaSAE"
    Friend Const ESTADO_VISOR_DEVUELTA_SAE As String = "EstVisorDevueltaSAE"
    Friend Const ESTADO_VISOR_RECIBIDA_BVC As String = "EstVisorRecibidaBVC"
    Friend Const ESTADO_VISOR_RECIVIDA_BVC_CALZADA As String = "EstVisorRecibidaBVCCalzada"
    Friend Const ESTADO_VISOR_DEVUELTA As String = "EstVisorDevuelta"

    ' Tipos de negocio
    Friend Const TIPO_NEGOCIO_ACCIONES As String = "TipoNegocioAcciones"
    Friend Const TIPO_NEGOCIO_REPO As String = "TipoNegocioREPO"
    Friend Const TIPO_NEGOCIO_SIMULTANEAS As String = "TipoNegocioSimultaneas"
    Friend Const TIPO_NEGOCIO_RENTA_FIJA As String = "TipoNegocioRentaFija"
    Friend Const TIPO_NEGOCIO_TTV As String = "TipoNegocioTTV"
    Friend Const TIPO_NEGOCIO_ACCIONES_OF As String = "TipoNegocioAccionesOF"
    Friend Const TIPO_NEGOCIO_RENTA_FIJA_OF As String = "TipoNegocioRentaFijaOF"

    'Tipos de Orden
    Friend Const TIPO_ORDEN_DIRECTA As String = "TipoOrdenDirecta"
    Friend Const TIPO_ORDEN_INDIRECTA As String = "TipoOrdenIndirecta"

    'Ordenes de OYDPLUS
    Friend Const TIPOMERCADO_PRECIO_ESPECIE As String = "TipoMercadoPrecioEspecie"
    Friend Const TIPOMERCADO_PORLOMEJOR_PRECIO_ESPECIE As String = "TipoMercadoPorLoMejorPrecioEspecie"

    Friend Const CLASIFICACIONDEFECTO_ORDENES As String = "ClasificacionDefectoOrdenes"
    Friend Const TIPOLIMITEDEFECTO_ORDENES As String = "TipoLimiteDefectoOrdenes"
    Friend Const CONDNEGOCIACIONDEFECTO_ORDENES As String = "CondicionesNegociacionDefectoOrdenes"
    Friend Const TIPOINVERSIONDEFECTO_ORDENES As String = "TipoInversionDefectoOrdenes"
    Friend Const EJECUCIONDEFECTO_ORDENES As String = "EjecucionDefectoOrdenes"
    Friend Const DURACIONDEFECTO_ORDENES As String = "DuracionDefectoOrdenes"
    Friend Const MERCADODEFECTO_ORDENES As String = "MercadoDefectoOrdenes"
    Friend Const TIPOPRODUCTO_CUENTAPROPIA As String = "TipoProductoCuentaPropia"

    ' Comandos para el menú contextual
    Friend Const CM_MOSTRAR_EN_VISOR As String = "ComandoMostrarVisor"
    Friend Const CM_MARCAR_LANZADA As String = "ComandoMarcarComoLanzada"
    Friend Const CM_LANZAR_SAE As String = "ComandoLanzarSAE"
    Friend Const CM_RECHAZAR As String = "ComandoRechazar"

    Friend Const TOPICO_MOTIVO_RECHAZO As String = "topicoMotivoRechazo"

    ' Ruta física y url del xap del visor
    Friend Const RUTA_VISOR_XAP As String = "RutaVisorXAP"
    Friend Const URL_VISOR_XAP As String = "UrlVisorXAP"

    ' Colores normal y de estado pendiente en SAE
    Friend Const COLOR_NORMAL As String = "ColorFilaNormal"
    Friend Const COLOR_ORDEN_PENDIENTE_SAE As String = "ColorOrdenEnSAEPendiente"
    Friend Const COLOR_ORDEN_CRUZADA As String = "ColorOrdenCruzada"

    Friend Const NOMBRE_PARAM_RUTA_CARGA_ARCHIVO As String = "A2RutaFisicaGeneracion"

    Friend Const CONSULTAR_LEO_CADA As String = "ConsultarLEOCada"

#End Region

#Region "Listas Renta Fija"
    Friend Const LISTA_RF_TTV_REGRESO As String = "TTV_Regreso"
    Friend Const LISTA_RF_TTV_SALIDA As String = "TTV_Salida"
    Friend Const LISTA_RF_REPO As String = "REPO"
    Friend Const LISTA_RF_SIMULTANEA_REGRESO As String = "Simultanea_Regreso"
    Friend Const LISTA_RF_SIMULTANEA_SALIDA As String = "Simultanea_Salida"
    Friend Const LISTA_RF_SIMULTANEA_REGRESO_CRCC As String = "Simultanea_Regreso_CRCC" 'JABG 20160603
    Friend Const LISTA_RF_SIMULTANEA_SALIDA_CRCC As String = "Simultanea_Salida_CRCC"   'JABG 20160603
    Friend Const LISTA_RF_SWAP As String = "SWAP"
    Friend Const LISTA_RF_MERCADO_REPO As String = "MERCADO_Repo"
    Friend Const LISTA_RF_MERCADO_PRIMARIO As String = "MERCADO_Primario"
    Friend Const LISTA_RF_MERCADO_RENOVACION As String = "MERCADO_Renovacion"
    Friend Const LISTA_RF_MERCADO_SECUNDARIO As String = "MERCADO_Secundario"
#End Region

#Region "Acciones"

    Friend Const FECHATOMA_IGUAL_FECHASISTEMA_ACC As String = "FECHATOMA_IGUAL_FECHASISTEMA_ACC"

#End Region

    Public Shared ReadOnly Property RutaServicioUtilidadesOYD() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioNegocio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_ORDENES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_ORDENES).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_ORDENES))
                End If
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_MAESTROS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_MAESTROS))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_ORDENES))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property URLWebPageSeteador() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_VISORSETEADOR) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_VISORSETEADOR).ToString()
                Else
                    Return ("http://a2serverpro:1020/OyDServiciosRIA/OYDPLUS/DatosVisorPage.aspx?IdOrden=")
                End If
            Else
                Return ("http://a2serverpro:1020/OyDServiciosRIA/OYDPLUS/DatosVisorPage.aspx?IdOrden=")
            End If
        End Get
        '        Get
        '#If ES_PRUEBA = False Then
        '            'Dim STR_NOMBRE_PAGE_SETEADOR As String = RutaServicioNegocio.Replace("/services/A2-OyD-OYDServer-RIA-Web-OrdenesDomainService.svc", "/OyDPlus/DatosVisorPage.aspx?IdOrden=")
        '            Dim STR_NOMBRE_PAGE_SETEADOR As String = RutaServicioNegocio.Replace("/services/A2-OYD-OYDServer-RIA-Web-OrdenesDomainService.svc", "/OyDPlus/DatosVisorPage.aspx?IdOrden=")
        '            Return STR_NOMBRE_PAGE_SETEADOR
        '#Else
        '            Return "http://localhost:3857/wsSeteador/DatosVisor.aspx?IdOrden="
        '#End If
        '        End Get
    End Property

    Public Shared ReadOnly Property RF_TTV_Regreso() As String
        Get
            Return RetornarClave(LISTA_RF_TTV_REGRESO)
        End Get
    End Property

    Public Shared ReadOnly Property RF_TTV_Salida() As String
        Get
            Return RetornarClave(LISTA_RF_TTV_SALIDA)
        End Get
    End Property

    Public Shared ReadOnly Property RF_REPO() As String
        Get
            Return RetornarClave(LISTA_RF_REPO)
        End Get
    End Property

    Public Shared ReadOnly Property RF_Simulatena_Regreso() As String
        Get
            Return RetornarClave(LISTA_RF_SIMULTANEA_REGRESO)
        End Get
    End Property

    Public Shared ReadOnly Property RF_Simulatena_Salida() As String
        Get
            Return RetornarClave(LISTA_RF_SIMULTANEA_SALIDA)
        End Get
    End Property

    'JABG 20160603
    Public Shared ReadOnly Property RF_Simultanea_Regreso_CRCC() As String
        Get
            Return RetornarClave(LISTA_RF_SIMULTANEA_REGRESO_CRCC)
        End Get
    End Property

    'JABG 20160603
    Public Shared ReadOnly Property RF_Simultanea_Salida_CRCC() As String
        Get
            Return RetornarClave(LISTA_RF_SIMULTANEA_SALIDA_CRCC)
        End Get
    End Property

    Public Shared ReadOnly Property RF_SWAP() As String
        Get
            Return RetornarClave(LISTA_RF_SWAP)
        End Get
    End Property

    Public Shared ReadOnly Property RF_Mercado_Repo() As String
        Get
            Return RetornarClave(LISTA_RF_MERCADO_REPO)
        End Get
    End Property

    Public Shared ReadOnly Property RF_Mercado_Primario() As String
        Get
            Return RetornarClave(LISTA_RF_MERCADO_PRIMARIO)
        End Get
    End Property

    Public Shared ReadOnly Property RF_Mercado_Renovacion() As String
        Get
            Return RetornarClave(LISTA_RF_MERCADO_RENOVACION)
        End Get
    End Property

    Public Shared ReadOnly Property RF_Mercado_Secundario() As String
        Get
            Return RetornarClave(LISTA_RF_MERCADO_SECUNDARIO)
        End Get
    End Property

    Public Shared ReadOnly Property FECHATOMA_IGUAL_FECHASISTEMA() As String
        Get
            Return RetornarClave(FECHATOMA_IGUAL_FECHASISTEMA_ACC)
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioImportaciones() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Private Shared Function RetornarClave(ByVal pstrConstante As String) As String
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(pstrConstante) Then
                Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(pstrConstante).ToString()
            Else
                Return ("")
            End If
        Else
            Return ("")
        End If
    End Function

    Public Overloads Shared ReadOnly Property Usuario As String
        Get
            If Program.UsaUsuarioSinDomino Then
                Return Program.UsuarioSinDomino
            Else
                Return A2Utilidades.Program.Usuario
            End If
        End Get
    End Property

    Public Overloads Shared ReadOnly Property VersionApp As String
        Get
            Dim name As String = Assembly.GetExecutingAssembly().FullName()
            Dim asmName As New AssemblyName(name)
            Return String.Format("v.{0}.{1}.{2}.{3}", asmName.Version.Major, asmName.Version.Minor, asmName.Version.Build, asmName.Version.Revision)
        End Get
    End Property

    ' Indica la versión o cliente de la aplicación para determinar que opcioines cambian en las pantallas
    Public Shared ReadOnly Property VersionAplicacionCliente() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(VERSION_APLICACION_CLIENTE) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(VERSION_APLICACION_CLIENTE).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

#Region "Propiedades para estados propios del seteador"
    Public Shared ReadOnly Property ES_Estado_Orden_Pendiente() As String
        Get
            Return RetornarClave(ESTADO_ORDEN_PENDIENTE)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Bolsa_Recibida() As String
        Get
            Return RetornarClave(ESTADO_BOLSA_RECIBIDA)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Bolsa_Lanzada() As String
        Get
            Return RetornarClave(ESTADO_BOLSA_LANZADA)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Bolsa_Complementada() As String
        Get
            Return RetornarClave(ESTADO_BOLSA_COMPLEMENTADA)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Visor_Por_Enviar() As String
        Get
            Return RetornarClave(ESTADO_VISOR_POR_ENVIAR)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Visor_Enviada_SAE() As String
        Get
            Return RetornarClave(ESTADO_VISOR_ENVIADA_SAE)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Visor_Mostrada_En_Visor() As String
        Get
            Return RetornarClave(ESTADO_VISOR_MOSTRADA_EN_VISOR)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Visor_En_BVC_Sin_Respuesta() As String
        Get
            Return RetornarClave(ESTADO_VISOR_EN_BVC_SIN_RESPUESTA)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Visor_Recibida_SAE() As String
        Get
            Return RetornarClave(ESTADO_VISOR_RECIBIDA_SAE)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Visor_Devuelta_SAE() As String
        Get
            Return RetornarClave(ESTADO_VISOR_DEVUELTA_SAE)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Visor_Recibida_BVC() As String
        Get
            Return RetornarClave(ESTADO_VISOR_RECIBIDA_BVC)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Visor_Recibida_BVC_Calzada() As String
        Get
            Return RetornarClave(ESTADO_VISOR_RECIVIDA_BVC_CALZADA)
        End Get
    End Property

    Public Shared ReadOnly Property ES_Estado_Visor_Devuelta() As String
        Get
            Return RetornarClave(ESTADO_VISOR_DEVUELTA)
        End Get
    End Property

    Public Shared ReadOnly Property Recarga_Automatica_Activa() As Boolean
        Get
            Return IIf(RetornarClave(HABILITAR_RECARGA) = "1", True, False)
        End Get
    End Property

    Public Shared ReadOnly Property Par_lapso_recarga() As Integer
        Get
            Return Integer.Parse(RetornarClave(LAPSO_RECARGA))
        End Get
    End Property

    Public Shared ReadOnly Property tpc_motivo_rechazo() As String
        Get
            Return RetornarClave(TOPICO_MOTIVO_RECHAZO)
        End Get
    End Property

    Public Shared ReadOnly Property par_ruta_xap() As String
        Get
            Return RetornarClave(RUTA_VISOR_XAP)
        End Get
    End Property

    Public Shared ReadOnly Property par_url_xap() As String
        Get
            Return RetornarClave(URL_VISOR_XAP)
        End Get
    End Property

    Public Shared ReadOnly Property par_color_normal() As String
        Get
            Return RetornarClave(COLOR_NORMAL)
        End Get
    End Property

    Public Shared ReadOnly Property par_color_orden_pendiente_SAE() As String
        Get
            Return RetornarClave(COLOR_ORDEN_PENDIENTE_SAE)
        End Get
    End Property

    Public Shared ReadOnly Property par_color_cruzada() As String
        Get
            Return RetornarClave(COLOR_ORDEN_CRUZADA)
        End Get
    End Property

    Public Shared ReadOnly Property Par_Consultar_LEO_Cada() As Integer
        Get
            Return Integer.Parse(RetornarClave(CONSULTAR_LEO_CADA))
        End Get
    End Property

#End Region

#Region "Tipos de negocio"

    Public Shared ReadOnly Property TN_Acciones() As String
        Get
            Return RetornarClave(TIPO_NEGOCIO_ACCIONES)
        End Get
    End Property

    Public Shared ReadOnly Property TN_REPO() As String
        Get
            Return RetornarClave(TIPO_NEGOCIO_REPO)
        End Get
    End Property

    Public Shared ReadOnly Property TN_Simultaneas() As String
        Get
            Return RetornarClave(TIPO_NEGOCIO_SIMULTANEAS)
        End Get
    End Property

    Public Shared ReadOnly Property TN_Renta_Fija() As String
        Get
            Return RetornarClave(TIPO_NEGOCIO_RENTA_FIJA)
        End Get
    End Property

    Public Shared ReadOnly Property TN_TTV() As String
        Get
            Return RetornarClave(TIPO_NEGOCIO_TTV)
        End Get
    End Property

    Public Shared ReadOnly Property TN_Acciones_OF() As String
        Get
            Return RetornarClave(TIPO_NEGOCIO_ACCIONES_OF)
        End Get
    End Property

    Public Shared ReadOnly Property TN_Renta_Fija_OF() As String
        Get
            Return RetornarClave(TIPO_NEGOCIO_RENTA_FIJA_OF)
        End Get
    End Property


#End Region

#Region "Tipos de orden"

    Public Shared ReadOnly Property TO_Directa() As String
        Get
            Return RetornarClave(TIPO_ORDEN_DIRECTA)
        End Get
    End Property

    Public Shared ReadOnly Property TO_Indirecta() As String
        Get
            Return RetornarClave(TIPO_ORDEN_INDIRECTA)
        End Get
    End Property

#End Region

#Region "Ordenes de OYDPLUS"

    Public Shared ReadOnly Property TM_PRECIO_ESPECIE() As String
        Get
            Return RetornarClave(TIPOMERCADO_PRECIO_ESPECIE)
        End Get
    End Property

    Public Shared ReadOnly Property TM_PORLOMEJOR_PRECIO_ESPECIE() As String
        Get
            Return RetornarClave(TIPOMERCADO_PORLOMEJOR_PRECIO_ESPECIE)
        End Get
    End Property

    Public Shared ReadOnly Property CLASIFICACIONXDEFECTO_ORDEN() As String
        Get
            Return RetornarClave(CLASIFICACIONDEFECTO_ORDENES)
        End Get
    End Property

    Public Shared ReadOnly Property TIPOLIMITEXDEFECTO_ORDEN() As String
        Get
            Return RetornarClave(TIPOLIMITEDEFECTO_ORDENES)
        End Get
    End Property

    Public Shared ReadOnly Property CONDNEGOCIACIONXDEFECTO_ORDEN() As String
        Get
            Return RetornarClave(CONDNEGOCIACIONDEFECTO_ORDENES)
        End Get
    End Property

    Public Shared ReadOnly Property TIPOINVERSIONXDEFECTO_ORDEN() As String
        Get
            Return RetornarClave(TIPOINVERSIONDEFECTO_ORDENES)
        End Get
    End Property

    Public Shared ReadOnly Property EJECUCIONXDEFECTO_ORDEN() As String
        Get
            Return RetornarClave(EJECUCIONDEFECTO_ORDENES)
        End Get
    End Property

    Public Shared ReadOnly Property DURACIONXDEFECTO_ORDEN() As String
        Get
            Return RetornarClave(DURACIONDEFECTO_ORDENES)
        End Get
    End Property

    Public Shared ReadOnly Property MERCADOXDEFECTO_ORDEN() As String
        Get
            Return RetornarClave(MERCADODEFECTO_ORDENES)
        End Get
    End Property

    Public Shared ReadOnly Property TIPOPRODUCTO_CUENTAPROPIA_ORDEN() As String
        Get
            Return RetornarClave(TIPOPRODUCTO_CUENTAPROPIA)
        End Get
    End Property

    Public Shared ReadOnly Property VELOCIDADTEXTO_TICKER_ORDEN() As String
        Get
            Return RetornarClave(VELOCIDADTEXTO_TICKER)
        End Get
    End Property

#End Region


#Region "Menú contextual Seteador"

    Public Shared ReadOnly Property CMND_Mostrar_En_Visor() As String
        Get
            Return RetornarClave(CM_MOSTRAR_EN_VISOR)
        End Get
    End Property

    Public Shared ReadOnly Property CMND_Marcar_Lanzada() As String
        Get
            Return RetornarClave(CM_MARCAR_LANZADA)
        End Get
    End Property

    Public Shared ReadOnly Property CMND_Lanzar_SAE() As String
        Get
            Return RetornarClave(CM_LANZAR_SAE)
        End Get
    End Property

    Public Shared ReadOnly Property CMND_Rechazar() As String
        Get
            Return RetornarClave(CM_RECHAZAR)
        End Get
    End Property

#End Region


    Public Shared Function colorFromHex(pstrHex As String) As Color
        Try
            pstrHex = pstrHex.Replace("#", String.Empty)
            Dim r, g, b As Byte

            r = Byte.Parse(pstrHex.Substring(0, 2), NumberStyles.HexNumber)
            g = Byte.Parse(pstrHex.Substring(2, 2), NumberStyles.HexNumber)
            b = Byte.Parse(pstrHex.Substring(4, 2), NumberStyles.HexNumber)
            Return Color.FromArgb(255, r, g, b)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de asignar color desde hexadecimal", Program.TituloSistema, "colorFromHex", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Public Shared ReadOnly Property RutafisicaArchivo() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_CARGA_ARCHIVO) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_CARGA_ARCHIVO).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioNegocioBolsa() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_BOLSA) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_BOLSA).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    ''' <summary>
    ''' Ruta donde se encuentra alojada la pagina para subir los archivos al servidor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property URLFileUploads() As String
        Get
            Dim strRuta As String = RutaServicioUtilidadesOYD.Substring(0, RutaServicioUtilidadesOYD.ToLower.ToString().IndexOf("/services")) & "/Uploads.aspx"
            'strRuta = "http://a2sql2005pro:8090/Sitios%20Web/OYDServiciosRIA/Uploads.aspx"
            'strRuta = "http://a2sql2005pro:9090/OYDServiciosRIA/Uploads.aspx"
            'strRuta = "http://a2webdllo:9001/OyDServiciosRIA/Uploads.aspx"
            Return strRuta
        End Get
    End Property

    ''' <summary>
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    '''
    Friend Shared ReadOnly Property ejecutarAppSegunAmbiente() As Boolean
        Get
            Return (A2Ctl.FuncionesCompartidas.ejecutarAppSegunAmbiente())
        End Get
    End Property

    Public Shared ReadOnly Property ServidorReportes() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_SERVIDOR_REPORTES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_SERVIDOR_REPORTES).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property ServidorReportesEjecution() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_SERVIDOR_REPORTESEJECUTION) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_SERVIDOR_REPORTESEJECUTION).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property CarpetaReportes() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_CARPETAREPORTES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_CARPETAREPORTES).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property CulturaReportes() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_CULTURA_REPORTES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_CULTURA_REPORTES).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As OrdenesDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.Ordens.HasChanges Then
                For Each li In pobjProxy.Ordens
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ReceptoresOrdens.HasChanges Then
                For Each li In pobjProxy.ReceptoresOrdens
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.LiqAsociadasOrdens.HasChanges Then
                For Each li In pobjProxy.LiqAsociadasOrdens
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.InstruccionesOrdenes.HasChanges Then
                For Each li In pobjProxy.InstruccionesOrdenes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.OrdenesPagos.HasChanges Then
                For Each li In pobjProxy.OrdenesPagos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.AdicionalesOrdenes.HasChanges Then
                For Each li In pobjProxy.AdicionalesOrdenes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.Orden_MIs.HasChanges Then
                For Each li In pobjProxy.Orden_MIs
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.OrdenesOFs.HasChanges Then
                For Each li In pobjProxy.OrdenesOFs
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ReceptorOrdenesOFs.HasChanges Then
                For Each li In pobjProxy.ReceptorOrdenesOFs
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ReceptorOrdenesOFs.HasChanges Then
                For Each li In pobjProxy.ReceptorOrdenesOFs
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
        End If
    End Sub

    Public Shared Sub VerificarCambiosProxyServidorImportaciones(ByRef pobjProxy As ImportacionesDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.ImportacionLis.HasChanges Then
                For Each li In pobjProxy.ImportacionLis
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.clsChequesOyDPlus.HasChanges Then
                For Each li In pobjProxy.clsChequesOyDPlus
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.clsTransferenciaOyDPlus.HasChanges Then
                For Each li In pobjProxy.clsTransferenciaOyDPlus
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ArchivoOrdenesLeos.HasChanges Then
                For Each li In pobjProxy.ArchivoOrdenesLeos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ArchivoReposBanReps.HasChanges Then
                For Each li In pobjProxy.ArchivoReposBanReps
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
        End If
    End Sub
End Class
