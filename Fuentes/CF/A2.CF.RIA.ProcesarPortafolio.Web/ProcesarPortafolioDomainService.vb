Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.CFProcesarPortafolios
Imports System.Text
Imports System.Web
Imports System.Web.UI.Page
Imports System.Web.Configuration
Imports System.Data.SqlClient
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

''' <summary>
''' DomainServices para los maestros Calificadoras, EspeciesClaseInversion, CalificacionesCalificadora, PreciosEspecies,
''' Indicadores y ProcesarPortafolio, pertenecientes al proyecto Cálculos Financieros.
''' </summary>
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Febrero 21/2014 
''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok
''' <remarks></remarks>

<EnableClientAccess()>
Partial Public Class ProcesarPortafoliosDomainService
    Inherits LinqToSqlDomainService(Of ProcesarPortafoliosDBML)


    'JAEZ funcion para tomar el tipo de modulo desde en web.config, que nos cambia el applicationaName 20161001
    Public Function Modulos() As String
        Try
            Dim strModuloPantalla As String = String.Empty
            Dim strDelimitador As Char = CChar(",")

            Dim strModulos As String = WebConfigurationManager.AppSettings("Modulos")

            If Not String.IsNullOrEmpty(strModulos) Then

                Dim strLista() As String = strModulos.Split(strDelimitador)

                For Each UnicoModulo In strLista
                    If CBool(InStr(1, UnicoModulo, "[OYDVALORAR]")) Then
                        strModuloPantalla = Right(UnicoModulo, Len(UnicoModulo) - InStrRev(UnicoModulo, "="))
                    End If
                Next

            End If

            Return strModuloPantalla
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

    <Query(HasSideEffects:=True)>
    Public Function ListaRetorno() As List(Of ProcesarUtilidadesCustodias)
        Return New List(Of ProcesarUtilidadesCustodias)
    End Function

#Region "ProcesarPortafolio"

#Region "Métodos modelo para activar funcionalidad RIA"
    ''' <summary>
    ''' Este metodo vacío se implementa para que el sistema permita modificar las entidades desde el grid de Cobro de Utilidades
    ''' </summary>
    ''' <param name="currentProcesarUtilidadesCustodias">Objeto tipo ProcesarUtilidadesCustodias</param>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Este metodo vacío se implementa para que el sistema permita modificar las entidades desde el grid de Cobro de Utilidades
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Public Sub UpdateProcesarUtilidadesCustodias(ByVal currentProcesarUtilidadesCustodias As ProcesarUtilidadesCustodias)

    End Sub
#End Region

#Region "Métodos asincrónicos"

    ''' <summary>
    ''' Este procedimiento inicia la valoración
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Objeto de tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Objeto de tipo String</param>
    ''' <param name="plngIDComitente">Objeto de tipo String</param>
    ''' <param name="pstrTipoPortafolio">Objeto de tipo String</param>
    ''' <param name="pstrTipoProceso">Objeto de tipo String</param>
    ''' <param name="pstrUsuario">Objeto de tipo String</param>
    ''' <returns>Retorna un cadena validando si existen Cobro de Utilidades Pendientes</returns>
    ''' <history>
    ''' Modificado por  : Germán Arbey González Osorio
    ''' Descripción     : Este procedimiento inicia la valoración
    ''' Fecha           : Marzo 31/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    ''' <history>
    ''' Modificado por  : Javier Eduardo Pardo Moreno
    ''' Descripción     : Se agrega el parámetro plogContabilizar
    ''' Fecha           : Septiembre 29/2015
    ''' Pruebas CB      : Javier Eduardo Pardo Moreno - Septiembre 29/2015 - Resultado OK
    ''' </history>
    Public Function ProcesarPortafolio(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, ByVal plogContabilizar As Boolean, ByVal pstrUsuario As String, ByVal plogIniciarJobValoracion As System.Nullable(Of System.Boolean), ByVal plogEliminarDatosResultadoMotor As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            'JAEZ Crear nueva instancia 20161003
            Me.DataContext.ActualizarModulo(Modulos())

            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_ValoracionPortafolio(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, plogContabilizar, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarProcesarPortafolio"), 0, pstrMsgValidacion, plogIniciarJobValoracion, plogEliminarDatosResultadoMotor)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarProcesarPortafolio")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Este método valida si existen cobros de utilidades pendientes para posteriormente leventar una ventana con dichos registros
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro de tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro de tipo String</param>
    ''' <param name="plngIDComitente">Parámetro de tipo String</param>
    ''' <param name="pstrUsuario">Parámetro de tipo String</param>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Este método valida si existen cobros de utilidades pendientes para posteriormente leventar una ventana con dichos registros
    ''' Fecha           : Marzo 31/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Public Function ValidarCobroUtilidadesPendientes(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim plogUtilidad As Boolean = False
            Me.DataContext.uspCalculosFinancieros_ValidarCobrosPendientes(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarProcesarPortafolio"), 0, plogUtilidad)
            Return plogUtilidad
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarCobroUtilidadesPendientes")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarAvanceProcesamiento(pdtmFechaProcesoInicial As Nullable(Of Date), pdtmFechaProcesoFinal As Nullable(Of Date), pstrModulo As String, ByVal pstrTipoPortafolio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProcesarPortafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_AvanceProcesamiento(pdtmFechaProcesoInicial, pdtmFechaProcesoFinal, pstrModulo, pstrTipoPortafolio, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarAvanceProcesamiento"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarAvanceProcesamiento")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función que permite consultar la tabla de utilidades por flujo, vencimiento y entre otras antes de calculos financieros
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro de tipo Datetime</param>
    ''' <param name="pstrIdEspecie">Parámetro de tipo String</param>
    ''' <param name="plngIDComitente">Parámetro de tipo String</param>
    ''' <param name="pstrTipoCompania">Parámetro de tipo String</param>
    ''' <param name="pstrUsuario">Parámetro de tipo String</param>
    ''' <returns>Retorna un conjunto de registros de tipo "ProcesarUtilidadesCustodias"</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Descripción  : Este método se invoca desde uspCalculosFinancieros_UtilidadesCustodiasConsultar
    ''' Fecha        : Marzo 31/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Marzo 31/2014 - Resultado OK
    ''' </history>
    ''' <history>
    ''' Creado por   : Javier Eduardo Pardo Moreno
    ''' Descripción  : Se añade el parámetro pstrTipoCompania
    ''' Fecha        : Septiembre 07/2015
    ''' Pruebas CB   : Javier Eduardo Pardo Moreno - Septiembre 07/2015 - Resultado OK
    ''' </history>
    ''' <history>
    ''' Creado por   : Javier Eduardo Pardo Moreno
    ''' Descripción  : Se añade el parámetro pstrEstado
    ''' Fecha        : Septiembre 09/2015
    ''' Pruebas CB   : Javier Eduardo Pardo Moreno - Septiembre 09/2015 - Resultado OK
    ''' </history>
    Public Function UtilidadesCustodiasConsultar(pdtmFechaValoracion As Nullable(Of DateTime),
                                                 pstrIdEspecie As String,
                                                 plngIDComitente As String,
                                                 pstrTipoCompania As String,
                                                 pstrEstado As String,
                                                 ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProcesarUtilidadesCustodias)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_UtilidadesCustodiasConsultar(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoCompania, pstrEstado, pstrUsuario, DemeInfoSesion(pstrUsuario, "UtilidadesCustodiasConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UtilidadesCustodiasConsultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Este metodo permite actualizar los registros de los cobros de utilidades
    ''' </summary>
    ''' <param name="pxmlCobroUtilidades">Parámetro tipo String que contiene todos los registros de cobros pendientes </param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <param name="pdtmFechaValoracion">Parámetro tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro tipo String</param>
    ''' <param name="plngIDComitente">Parámetro tipo String</param>
    ''' <param name="pstrTipoPortafolio">Parámetro tipo String</param>
    ''' <param name="pstrTipoProceso">Parámetro tipo String</param>
    ''' <returns>Retorna un entero indicando si el proceso es exitoso</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Este metodo permite actualizar los registros de los cobros de utilidades
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Public Function UtilidadesCustodiasActualizar(ByVal pxmlCobroUtilidades As String,
                                                  ByVal pstrUsuario As String,
                                                  ByVal pdtmFechaValoracion As Nullable(Of Date),
                                                  ByVal pstrIdEspecie As String,
                                                  ByVal plngIDComitente As String,
                                                  ByVal pstrTipoPortafolio As String,
                                                  ByVal pstrTipoProceso As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_UtilidadesCustodiasActualizar(pxmlCobroUtilidades,
                                                                                          pstrUsuario,
                                                                                          DemeInfoSesion(pstrUsuario, "UtilidadesCustodiasActualizar"),
                                                                                          0,
                                                                                          pdtmFechaValoracion,
                                                                                          pstrIdEspecie,
                                                                                          plngIDComitente,
                                                                                          pstrTipoPortafolio,
                                                                                          pstrTipoProceso,
                                                                                          "")
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UtilidadesCustodiasActualizar")
            Return Nothing
        End Try
    End Function

    Public Function EliminarCalculos(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, plogEliminarCierreTodosLosPortafolios As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProcesarPortafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_EliminarCalculos(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, plogEliminarCierreTodosLosPortafolios, pstrUsuario, DemeInfoSesion(pstrUsuario, "EliminarCalculos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarCalculos")
            Return Nothing
        End Try
    End Function

    Public Function ValidarOperacionesPendientes(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, pstrReconstruir As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProcesarPortafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ValidarOperacionesPendientes(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, pstrReconstruir, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarOperacionesPendientes"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarOperacionesPendientes")
            Return Nothing
        End Try
    End Function

    Public Function ValidarFechaCierrePortafolio(pdtmFechaValoracion As System.Nullable(Of System.DateTime), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, ByVal plogCerrarPortafolios As System.Nullable(Of System.Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_ValidarFechaCierrePortafolio(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, plogCerrarPortafolios, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarFechaCierrePortafolio"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarFechaCierrePortafolio")
            Return Nothing
        End Try
    End Function

    Public Function CerrarFechaCierrePortafolio(pdtmFechaValoracion As Nullable(Of Date), ByVal pstrTipoPortafolio As String, plngIDComitente As String, ByVal plogConfirmarCerrarPortafolio As System.Nullable(Of System.Boolean), ByVal plogCerrarPortafolios As System.Nullable(Of System.Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProcesarPortafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CerrarFechaCierrePortafolio(pdtmFechaValoracion, pstrTipoPortafolio, plngIDComitente, plogConfirmarCerrarPortafolio, plogCerrarPortafolios, pstrUsuario, DemeInfoSesion(pstrUsuario, "CerrarFechaCierrePortafolio"), 0).ToList()
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CerrarFechaCierrePortafolio")
            Return Nothing
        End Try
    End Function

    'Asincronico
    Public Function ValidarParametro_EliminarCierreTodosLosPortafolios(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, plogEliminarValoracionesPosteriores As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_ValidarParametro_EliminarCierreTodosLosPortafolios(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, plogEliminarValoracionesPosteriores, pstrUsuario, DemeInfoSesion(pstrUsuario, "DevolverFechaCierreCF"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DevolverFechaCierreCF")
            Return Nothing
        End Try
    End Function

    Public Function ValidarFechaValoracionInferior(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspCalculosFinancieros_ValidarFechaValoracionInferior(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarFechaValoracionInferior"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarFechaValoracionInferior")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    ''' <summary>
    ''' Permite realizar una espera al método ProcesarPortafolioAsync, implementando una metodología síncrona
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro tipo String</param>
    ''' <param name="plngIDComitente">Parámetro tipo String</param>
    ''' <param name="pstrTipoPortafolio">Parámetro tipo String</param>
    ''' <param name="pstrTipoProceso">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna un objeto Task de tipo String</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Permite realizar una espera al método ProcesarPortafolioAsync, implementando una metodología síncrona
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Public Function ProcesarPortafolioSync(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, ByVal plogContabilizar As Boolean, ByVal pstrUsuario As String, ByVal plogIniciarJobValoracion As System.Nullable(Of System.Boolean), ByVal plogEliminarDatosResultadoMotor As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ProcesarPortafolioAsync(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, plogContabilizar, pstrUsuario, plogIniciarJobValoracion, plogEliminarDatosResultadoMotor, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    ''' <summary>
    ''' Realiza un llamado al método ProcesarPortafolio
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro tipo String</param>
    ''' <param name="plngIDComitente">Parámetro tipo String</param>
    ''' <param name="pstrTipoPortafolio">Parámetro tipo String</param>
    ''' <param name="pstrTipoProceso">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna un objeto Task de tipo String</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Realiza un llamado al método ProcesarPortafolio
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private Function ProcesarPortafolioAsync(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, ByVal plogContabilizar As Boolean, ByVal pstrUsuario As String, ByVal plogIniciarJobValoracion As System.Nullable(Of System.Boolean), ByVal plogEliminarDatosResultadoMotor As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ProcesarPortafolio(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, plogContabilizar, pstrUsuario, plogIniciarJobValoracion, plogEliminarDatosResultadoMotor, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Permite realizar una espera al método ValidarCobroUtilidadesPendientesAsync, implementando una metodología síncrona
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro tipo String</param>
    ''' <param name="plngIDComitente">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna un objeto Task de tipo String</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Permite realizar una espera al método ValidarCobroUtilidadesPendientesAsync, implementando una metodología síncrona
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Public Function ValidarCobroUtilidadesPendientesSync(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Dim objTask As Task(Of Boolean) = Me.ValidarCobroUtilidadesPendientesAsync(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    ''' <summary>
    ''' Realiza un llamado al método ValidarCobroUtilidadesPendientes
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro tipo String</param>
    ''' <param name="plngIDComitente">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna un objeto Task de tipo String</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Realiza un llamado al método ValidarCobroUtilidadesPendientes
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private Function ValidarCobroUtilidadesPendientesAsync(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)
        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(ValidarCobroUtilidadesPendientes(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ValidarParametro_EliminarCierreTodosLosPortafoliosSync(ByVal pdtmFechaValoracion As Nullable(Of Date), ByVal pstrIdEspecie As String, ByVal plngIDComitente As String, ByVal pstrTipoPortafolio As String, ByVal pstrTipoProceso As String, ByVal plogEliminarValoracionesPosteriores As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ValidarParametro_EliminarCierreTodosLosPortafoliosAsync(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, plogEliminarValoracionesPosteriores, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ValidarParametro_EliminarCierreTodosLosPortafoliosAsync(ByVal pdtmFechaValoracion As Nullable(Of Date), ByVal pstrIdEspecie As String, ByVal plngIDComitente As String, ByVal pstrTipoPortafolio As String, ByVal pstrTipoProceso As String, ByVal plogEliminarValoracionesPosteriores As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ValidarParametro_EliminarCierreTodosLosPortafolios(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, plogEliminarValoracionesPosteriores, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Private Function ValidarFechaValoracionInferiorAsync(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ValidarFechaValoracionInferior(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ValidarFechaValoracionInferiorSync(pdtmFechaValoracion As Nullable(Of Date), pstrIdEspecie As String, plngIDComitente As String, pstrTipoPortafolio As String, pstrTipoProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ValidarFechaValoracionInferiorAsync(pdtmFechaValoracion, pstrIdEspecie, plngIDComitente, pstrTipoPortafolio, pstrTipoProceso, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

#End Region

#End Region

#Region "Portafolios"

#Region "Métodos asincrónicos"
    Public Function ConsultarDatosPortafolio(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosPortafolios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_DatosPortafolio_Consultar(plngIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "DatosPortafolio"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DatosPortafolio")
            Return Nothing
        End Try
    End Function
#End Region

    Public Function ConsultarDatosPortafolioSync(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosPortafolios)
        Dim objTask As Task(Of List(Of DatosPortafolios)) = Me.ConsultarDatosPortafoliosAsync(plngIDComitente, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarDatosPortafoliosAsync(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DatosPortafolios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DatosPortafolios)) = New TaskCompletionSource(Of List(Of DatosPortafolios))()
        objTaskComplete.TrySetResult(ConsultarDatosPortafolio(plngIDComitente, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

End Class

Public Class RutasArchivos
    Public Property NombreProceso As String
    Public Property RutaWeb As String
    Public Property RutaArchivosLocal As String
    Public Property MensajeDebbug As String
    Public Property RutaArchivosUpload As String
    Public Property RutaArchivoUploadProceso As String
    Public Property RutaArchivoProceso As String
End Class
