Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports System.Reflection
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
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const NOMBRE_DICCIONARIO_COMBOS As String = "DiccionarioCombosA2"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS As String = "DiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"
    Friend Const HABILITAR_RECARGA As String = "HabilitarRecargaAutomatica"
    Friend Const LAPSO_RECARGA As String = "LapsoRecargaAutomatica"
    Public Const CLAVE_PARAM_EJECUTAR_SEGUN_AMB As String = "EJECUTAR_APP_SEGUN_AMBIENTE"

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
#End Region

#Region "Listas Renta Fija"
    Friend Const LISTA_RF_TTV_REGRESO As String = "TTV_Regreso"
    Friend Const LISTA_RF_TTV_SALIDA As String = "TTV_Salida"
    Friend Const LISTA_RF_REPO As String = "REPO"
    Friend Const LISTA_RF_SIMULTANEA_REGRESO As String = "Simultanea_Regreso"
    Friend Const LISTA_RF_SIMULTANEA_SALIDA As String = "Simultanea_Salida"
    Friend Const LISTA_RF_SWAP As String = "SWAP"
    Friend Const LISTA_RF_MERCADO_REPO As String = "MERCADO_Repo"
    Friend Const LISTA_RF_MERCADO_PRIMARIO As String = "MERCADO_Primario"
    Friend Const LISTA_RF_MERCADO_RENOVACION As String = "MERCADO_Renovacion"
    Friend Const LISTA_RF_MERCADO_SECUNDARIO As String = "MERCADO_Secundario"
#End Region


    Public Shared ReadOnly Property RutaServicioUtilidadesOYD() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioNegocio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_ORDENES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_ORDENES).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property URLWebPageSeteador() As String
        Get
#If ES_PRUEBA = False Then
            Dim STR_NOMBRE_PAGE_SETEADOR As String = RutaServicioNegocio.Replace("/Services/A2-OyD-OYDServer-RIA-Web-OrdenesDomainService.svc", "/OyDPlus/DatosVisorPage.aspx?IdOrden=")
            Return STR_NOMBRE_PAGE_SETEADOR
#Else
            Return "http://localhost:3857/wsSeteador/DatosVisor.aspx?IdOrden="
#End If
        End Get
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
            Return Program.UsuarioSinDomino
        End Get
    End Property

    Public Overloads Shared ReadOnly Property VersionApp As String
        Get
            Dim name As String = Assembly.GetExecutingAssembly().FullName()
            Dim asmName As New AssemblyName(name)
            Return String.Format("v.{0}.{1}.{2}.{3}", asmName.Version.Major, asmName.Version.Minor, asmName.Version.Build, asmName.Version.Revision)
        End Get
    End Property

    ''' <summary>
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    ''' 
    Public Shared Function ejecutarAppSegunAmbiente() As Boolean
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(CLAVE_PARAM_EJECUTAR_SEGUN_AMB) Then
                Return CType(CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(CLAVE_PARAM_EJECUTAR_SEGUN_AMB), Boolean)
            Else
                Return (True)
            End If
        Else
            Return (True)
        End If
    End Function

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
End Class
