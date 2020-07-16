Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.OyD.OYDServer.RIA.Web
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Linq
Imports System.Linq
Imports System.IO
Imports System.Collections.ObjectModel
Imports System.Web
Imports System.Configuration
Imports A2Utilidades.Cifrar
Imports A2.OyD.OYDServer.RIA.Web.OyDOrdenes
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()>
Partial Public Class OrdenesDomainService
    Inherits LinqToSqlDomainService(Of OyDOrdenesDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Ordenes"

    Public Function OrdenesFiltrar(ByVal pstrClase As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Orden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_Filtrar(pstrClase, pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesFiltrar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesConsultar(ByVal pstrClase As String, ByVal pstrTipo As String, ByVal pintNroOrden As System.Nullable(Of Integer), ByVal pintVersion As System.Nullable(Of Integer),
                                     ByVal pstrIDComitente As String, ByVal pstrIDOrdenante As String, ByVal pdtmOrden As System.Nullable(Of Date), ByVal pstrFormaPago As String,
                                     ByVal pstrObjeto As String, ByVal pstrCondicionesNegociacion As String, ByVal pstrTipoInversion As String,
                                     ByVal pstrCanalRecepcion As String, ByVal pstrMedioVerificable As String, ByVal pdtmFechaHoraRecepcion As System.Nullable(Of Date),
                                     ByVal pstrTipoLimite As String, ByVal pdtmVigenciaHasta As System.Nullable(Of Date),
                                     ByVal pstrEstado As String, ByVal pstrEstadoOrdenBus As String, ByVal pstrEstadoMakerChecker As String, ByVal pstrAccionMakerChecker As String,
                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Orden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_Consultar(pstrClase, pstrTipo, pintNroOrden, pintVersion, pstrIDComitente, pstrIDOrdenante, pdtmOrden, pstrFormaPago,
                                      pstrObjeto, pstrCondicionesNegociacion, pstrTipoInversion, pstrCanalRecepcion, pstrMedioVerificable, pdtmFechaHoraRecepcion,
                                      pstrTipoLimite, pdtmVigenciaHasta, pstrEstado, pstrEstadoOrdenBus, pstrEstadoMakerChecker, pstrAccionMakerChecker, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarOrdenes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarOrdenes")
            Return Nothing
        End Try
    End Function

    Public Function TraerOrdenPorDefecto(ByVal pstrClaseOrden As String, ByVal pstrUsuario As String, ByVal pstrTipoOrden As String, ByVal pstrInfoConexion As String) As Orden
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Orden
            Dim objVig As List(Of ValidarFecha)

            '/ Buscar la fecha de vigencia por defecto para las nuevas órdenes. Se toma fecha de elaboración por defecto la fecha actual y una vigencia de 5 días
            objVig = CalcularDiasHabiles("vencimiento_orden", Now(), Nothing, 5, pstrUsuario, pstrInfoConexion).ToList

            e.Clase = pstrClaseOrden
            If Not pstrTipoOrden.Trim.Equals(String.Empty) Then
                e.Tipo = pstrTipoOrden
            End If
            e.Version = 0
            e.Estado = "P"
            e.Ordinaria = True
            e.Objeto = ""
            e.CondicionesNegociacion = "C"
            e.TipoLimite = "M"
            e.Ejecucion = "N"
            e.EstadoLEO = "R"
            e.Duracion = "I"
            e.TipoInversion = "N"
            e.Instrucciones = ""
            e.Notas = ""
            e.FechaOrden = Now()
            If IsNothing(objVig) Then
                e.VigenciaHasta = DateAdd(DateInterval.Day, 4, Now()) '/ Se suman 4 días para incluir la fecha actual
            Else
                If objVig.Count > 0 Then
                    e.VigenciaHasta = objVig.First.FechaFinal
                Else
                    e.VigenciaHasta = DateAdd(DateInterval.Day, 4, Now())
                End If
            End If
            e.IdBolsa = 4
            e.Repo = False
            e.TieneOrdenRelacionada = False

            e.FormaPago = Nothing
            e.IDComitente = Nothing
            e.IDOrdenante = Nothing
            e.Nemotecnico = Nothing
            e.UBICACIONTITULO = Nothing
            e.CuentaDeposito = Nothing
            e.FormaPago = Nothing
            e.MedioVerificable = Nothing
            e.CanalRecepcion = Nothing
            e.UsuarioOperador = Nothing
            e.NroExtensionToma = Nothing
            e.IdReceptorToma = Nothing
            e.FechaRecepcion = Nothing

            e.ComisionPactada = 0
            e.EnPesos = False
            e.DividendoCompra = False

            e.Eca = False
            e.OrdenEscrito = Nothing
            e.CostoAdicionalesOrden = 0
            e.SitioIngreso = Nothing
            e.DescripcionOrden = "..."
            e.NombreCliente = "..."
            e.AccionMakerAndChecker = "I"
            e.EstadoMakerChecker = "PA"
            e.IDPreliquidacion = 0
            e.Renovacion = False
            e.IpOrigen = Nothing
            e.NegocioEspecial = Nothing

            e.ReceptoresOrdenes = Nothing
            e.LiqAsociadasOrdenes = Nothing
            e.BeneficiariosOrdenes = Nothing
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerOrdenPorDefecto")
            Return Nothing
        End Try
    End Function


    Public Sub InsertOrden(ByVal Orden As Orden)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Orden.pstrUsuarioConexion, Orden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not Orden Is Nothing Then
                strUsuario = Orden.Usuario
            End If

            Orden.InfoSesion = DemeInfoSesion(strUsuario, "InsertOrden")


            'JWSJ - decode HTML
            Orden.ReceptoresXML = HttpUtility.HtmlDecode(Orden.ReceptoresXML)
            Orden.LiqAsociadasXML = HttpUtility.HtmlDecode(Orden.LiqAsociadasXML)
            Orden.PagosOrdenesXML = HttpUtility.HtmlDecode(Orden.PagosOrdenesXML)
            Orden.InstruccionesOrdenesXML = HttpUtility.HtmlDecode(Orden.InstruccionesOrdenesXML)
            Orden.ComisionesOrdenesXML = HttpUtility.HtmlDecode(Orden.ComisionesOrdenesXML)


            If A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.LogOrdenes Then
                A2Utilidades.Utilidades.generarLogTxt(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), "Inicia InsertOrden", strUsuario, "Orden No. " & Convert.ToString(Orden.NroOrden), "Receptores:", Orden.ReceptoresXML, "Log_OrdenesOyD", "")
            End If


            Me.DataContext.Ordenes.InsertOnSubmit(Orden)

            If A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.LogOrdenes Then
                A2Utilidades.Utilidades.generarLogTxt(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), "Termina InsertOrden", strUsuario, "Orden No. " & Convert.ToString(Orden.NroOrden), "Receptores:", Orden.ReceptoresXML, "Log_OrdenesOyD", "")
            End If

        Catch ex As Exception

            A2Utilidades.Utilidades.generarLogTxt(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), "Exception InsertOrden", strUsuario, "Orden No. " & Convert.ToString(Orden.NroOrden), "Receptores: " & Orden.ReceptoresXML, ex.ToString(), "Log_OrdenesOyD", "")

            ManejarError(ex, Me.ToString(), "InsertOrden")
        End Try

    End Sub

    Public Sub UpdateOrden(ByVal currentOrden As Orden)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOrden.pstrUsuarioConexion, currentOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not currentOrden Is Nothing Then
                strUsuario = currentOrden.Usuario
            End If

            currentOrden.InfoSesion = DemeInfoSesion(strUsuario, "UpdateOrden")

            'JWSJ - decode HTML
            currentOrden.ReceptoresXML = HttpUtility.HtmlDecode(currentOrden.ReceptoresXML)
            currentOrden.LiqAsociadasXML = HttpUtility.HtmlDecode(currentOrden.LiqAsociadasXML)
            currentOrden.PagosOrdenesXML = HttpUtility.HtmlDecode(currentOrden.PagosOrdenesXML)
            currentOrden.InstruccionesOrdenesXML = HttpUtility.HtmlDecode(currentOrden.InstruccionesOrdenesXML)
            currentOrden.ComisionesOrdenesXML = HttpUtility.HtmlDecode(currentOrden.ComisionesOrdenesXML)

            If A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.LogOrdenes Then
                A2Utilidades.Utilidades.generarLogTxt(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), "Inicia UpdateOrden", strUsuario, "Orden No." & Convert.ToString(currentOrden.NroOrden), "Receptores:", currentOrden.ReceptoresXML, "Log_OrdenesOyD", "")
            End If

            Me.DataContext.Ordenes.InsertOnSubmit(currentOrden)

            If A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.LogOrdenes Then
                A2Utilidades.Utilidades.generarLogTxt(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), "Termina UpdateOrden", strUsuario, "Orden No." & Convert.ToString(currentOrden.NroOrden), "Receptores:", currentOrden.ReceptoresXML, "Log_OrdenesOyD", "")
            End If

        Catch ex As Exception

            A2Utilidades.Utilidades.generarLogTxt(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), "Exception UpdateOrden", strUsuario, "Orden No. " & Convert.ToString(currentOrden.NroOrden), "Receptores: " & currentOrden.ReceptoresXML, ex.ToString(), "Log_OrdenesOyD", "")

            ManejarError(ex, Me.ToString(), "UpdateOrden")
        End Try

    End Sub




    Public Function AnularOrden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrInstrucciones As String, ByVal pstrNotas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenSAE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_Anular(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrInstrucciones, pstrNotas, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularOrden")
            Return Nothing
        End Try
    End Function

    'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se añade el tipo de negocio para saber si la orden es de bolsa u otras firmas
    Public Function Traer_BeneficiariosOrdenes_Orden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUsuario As String, ByVal pstrTipoNegocio As String, ByVal pstrInfoConexion As String) As List(Of BeneficiariosOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_BeneficiariosOrdenes_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_BeneficiariosOrdenes_Orden"), ClsConstantes.GINT_ErrorPersonalizado, pstrTipoNegocio).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosOrdenes_Orden")
            Return Nothing
        End Try
    End Function

    Public Function Traer_BeneficiariosOrdenes_Cliente(ByVal plngIdComitente As String, ByVal pintNroCuenta As Integer, ByVal pstrDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiariosOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_BeneficiariosCuentaDep_Consultar(plngIdComitente, pintNroCuenta, pstrDeposito).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosOrdenes_Cliente")
            Return Nothing
        End Try
    End Function

    'Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se añade el tipo de negocio para saber si la orden es de bolsa u otras firmas
    Public Function Traer_ReceptoresOrdenes_Orden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUsuario As String, ByVal pstrEstadoMakerChecker As String, ByVal pstrTipoNegocio As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_ReceptoresOrdenes_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrUsuario, pstrEstadoMakerChecker, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresOrdenes_Orden"), ClsConstantes.GINT_ErrorPersonalizado, pstrTipoNegocio).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresOrdenes_Orden")
            Return Nothing
        End Try
    End Function

    Public Function Verificar_ReceptoresOrdenes_Orden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_ReceptoresOrdenes_Verificar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Verificar_ReceptoresOrdenes_Orden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Verificar_ReceptoresOrdenes_Orden")
            Return Nothing
        End Try
    End Function

    Public Function AutorizarOrden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal plogAprobar As Boolean, ByVal pintClasificacionRiesgoEspecie As Integer, ByVal pintPerfilInversionista As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenSAE)
        Dim intIdOrden As Integer = 1
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_Autorizar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, plogAprobar, pintClasificacionRiesgoEspecie, pintPerfilInversionista, intIdOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "Autorizar_Orden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Autorizar_Orden")
            Return Nothing
        End Try
    End Function

    Public Function ValidarOrden_Repo(ByVal pintNroOrden As Integer, ByVal plngIdComitente As String, ByVal pdtmFechaOrden As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ValidarOrden_Repo(pintNroOrden, plngIdComitente, pdtmFechaOrden, False, pstrUsuario)
            Dim variable = CStr(ret)
            Return variable
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarOrden_Repo")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta las liquidaciones probables de la orden
    ''' </summary>
    ''' <param name="pstrClaseOrden"></param>
    ''' <param name="pstrTipoOrden"></param>
    ''' <param name="pintNroOrden"></param>
    ''' <param name="pintVersion"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Traer_LiqProbables_Orden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer,
                                              ByVal pstrEstadoMakerChecker As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiqAsociadasOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_OrdenesLiqAsociadas_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrEstadoMakerChecker,
                                                                                         pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_LiqProbables_Orden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_LiqProbables_Orden")
            Return Nothing
        End Try
    End Function

    Public Function Traer_CuentasDepositoPago(ByVal plngIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasDepositoPago)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_CuentasDepositoPago(plngIdComitente, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_CuentasDepositoPago")
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Función para realizar las validaciones de Órdenes de Bolsa
    ''' </summary>
    ''' <param name="pIDOrden"></param>
    ''' <param name="plngID"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pstrClase"></param>
    ''' <param name="pstrIDComitente"></param>
    ''' <param name="pdtmOrden"></param>
    ''' <param name="plogOrdinaria"></param>
    ''' <param name="pstrObjeto"></param>
    ''' <param name="pdblCantidad"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna las validaciones pendientes</returns>
    ''' <remarks>SLB20130614</remarks>
    <Query(HasSideEffects:=True)>
    Public Function ValidarIngresoOrden_Bolsa(ByVal pIDOrden As Integer, ByVal plngID As Integer, ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pstrIDComitente As String,
                                              ByVal pdtmOrden As System.Nullable(Of DateTime), ByVal plogOrdinaria As Boolean, ByVal pstrObjeto As String, ByVal pdblCantidad As System.Nullable(Of Double),
                                              ByVal pstrIDEspecie As String, ByVal pstrUBICACIONTITULO As String, ByVal pdtmEmision As System.Nullable(Of Date), ByVal pdtmVencimiento As System.Nullable(Of Date),
                                              ByVal pstrModalidad As String, ByVal pdblTasaInicial As System.Nullable(Of Double), ByVal pstrIndicadorEconomico As String, ByVal pdblPuntosIndicador As System.Nullable(Of Double),
                                              ByVal pdblValorFuturoRepo As System.Nullable(Of Double), ByVal pstrCondicionesNegociacion As String, ByVal pdtmFechaCumplimiento As System.Nullable(Of DateTime), ByVal pstrUsuario As String,
                                              ByVal pdtmFechaVigencia As System.Nullable(Of DateTime), ByVal pstrInfoConexion As String,
                                              Optional ByVal pstrTipoLimite As String = "", Optional ByVal pstrEjecucion As String = "", Optional ByVal pstrDuracion As String = "") As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            If A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.LogOrdenes Then
                A2Utilidades.Utilidades.generarLogTxt(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), "Inicia ValidarIngresoOrden_Bolsa", pstrUsuario, "Order No.", Convert.ToString(plngID), "", "Log_OrdenesOyD", "")
            End If

            Dim ret = Me.DataContext.uspOyDNet_ValidarOrden_Bolsa(pIDOrden, plngID, pstrTipo, pstrClase, pstrIDComitente, pdtmOrden, plogOrdinaria, pstrObjeto, pdblCantidad,
                                                                  pstrIDEspecie, pstrUBICACIONTITULO, pdtmEmision, pdtmVencimiento, pstrModalidad, pdblTasaInicial,
                                                                  pstrIndicadorEconomico, pdblPuntosIndicador, pdblValorFuturoRepo, pstrCondicionesNegociacion,
                                                                  pdtmFechaCumplimiento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarIngresoOrden_Bolsa"), 0, pdtmFechaVigencia, pstrTipoLimite, pstrEjecucion, pstrDuracion)

            If A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.LogOrdenes Then
                A2Utilidades.Utilidades.generarLogTxt(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), "Termina ValidarIngresoOrden_Bolsa", pstrUsuario, "Orden No.", Convert.ToString(plngID), "", "Log_OrdenesOyD", "")
            End If

            Return ret.ToList
        Catch ex As Exception

            A2Utilidades.Utilidades.generarLogTxt(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"), "Exception ValidarIngresoOrden_Bolsa", pstrUsuario, "Orden No.", Convert.ToString(plngID), ex.ToString(), "Log_OrdenesOyD", "")

            ManejarError(ex, Me.ToString(), "ValidarIngresoOrden_Bolsa")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCuidadSeteo(ByVal pstrIDReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strSQL = "Select CONVERT(VARCHAR,S.lngID) + '+' + S.strNombre As strCampo FROM tblSucursales S INNER JOIN tblReceptores R ON R.lngSucReceptor = S.lngId WHERE R.strId = @p0 "
            Dim valor = fnEjecutarQuerySQL(strSQL, pstrIDReceptor)
            Return CStr(valor)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCuidadSeteo")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarClaseEspecie(ByVal pstrNemotecnico As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sptblOrdenes_ClaseEspecie_Consultar(pstrNemotecnico, False, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarClaseEspecie"), ClsConstantes.GINT_ErrorPersonalizado)
            Return CBool(ret)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarClaseEspecie")
            Return Nothing
        End Try
    End Function

#End Region

#Region "ReceptoresOrdenes"

    Public Function TraerReceptoresOrdenPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ReceptoresOrden
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ReceptoresOrden

            e.IDReceptor = Nothing
            e.Lider = False
            e.Porcentaje = 0

            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReceptoresOrdenPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertReceptoresOrden(ByVal ReceptoresOrden As ReceptoresOrden)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrden.pstrUsuarioConexion, ReceptoresOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not ReceptoresOrden Is Nothing Then
                strUsuario = ReceptoresOrden.Usuario
            End If

            ReceptoresOrden.InfoSesion = DemeInfoSesion(strUsuario, "InsertReceptoresOrden")
            Me.DataContext.ReceptoresOrdenes.InsertOnSubmit(ReceptoresOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertReceptoresOrden")
        End Try
    End Sub

    Public Sub UpdateReceptoresOrden(ByVal currentReceptoresOrden As ReceptoresOrden)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentReceptoresOrden.pstrUsuarioConexion, currentReceptoresOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not currentReceptoresOrden Is Nothing Then
                strUsuario = currentReceptoresOrden.Usuario
            End If

            currentReceptoresOrden.InfoSesion = DemeInfoSesion(strUsuario, "UpdateReceptoresOrden")
            Me.DataContext.ReceptoresOrdenes.Attach(currentReceptoresOrden, Me.ChangeSet.GetOriginal(currentReceptoresOrden))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptoresOrden")
        End Try
    End Sub

    Public Sub DeleteReceptoresOrden(ByVal ReceptoresOrden As ReceptoresOrden)
        Dim strUsuario As String = ""
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrden.pstrUsuarioConexion, ReceptoresOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not ReceptoresOrden Is Nothing Then
                strUsuario = ReceptoresOrden.Usuario
            End If

            ReceptoresOrden.InfoSesion = DemeInfoSesion(strUsuario, "DeleteReceptoresOrden")
            Me.DataContext.ReceptoresOrdenes.Attach(ReceptoresOrden)
            Me.DataContext.ReceptoresOrdenes.DeleteOnSubmit(ReceptoresOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptoresOrden")
        End Try
    End Sub
#End Region

#Region "LiqProbablesOrdenes"

    Public Function TraerLiqProbablesOrdenPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As LiqAsociadasOrden
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New LiqAsociadasOrden

            'e.IDReceptor = Nothing
            'e.Lider = False
            'e.Porcentaje = 0

            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerLiqProbablesOrdenPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertLiqProbablesOrden(ByVal LiqProbablesOrden As LiqAsociadasOrden)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,LiqProbablesOrden.pstrUsuarioConexion, LiqProbablesOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not LiqProbablesOrden Is Nothing Then
                strUsuario = LiqProbablesOrden.Usuario
            End If

            LiqProbablesOrden.InfoSesion = DemeInfoSesion(strUsuario, "InsertLiqProbablesOrden")
            Me.DataContext.LiqAsociadasOrdenes.InsertOnSubmit(LiqProbablesOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLiqProbablesOrden")
        End Try
    End Sub

    Public Sub UpdateLiqProbablesOrden(ByVal currentLiqProbablesOrden As LiqAsociadasOrden)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentLiqProbablesOrden.pstrUsuarioConexion, currentLiqProbablesOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not currentLiqProbablesOrden Is Nothing Then
                strUsuario = currentLiqProbablesOrden.Usuario
            End If

            currentLiqProbablesOrden.InfoSesion = DemeInfoSesion(strUsuario, "UpdateLiqProbablesOrden")
            Me.DataContext.LiqAsociadasOrdenes.Attach(currentLiqProbablesOrden, Me.ChangeSet.GetOriginal(currentLiqProbablesOrden))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLiqProbablesOrden")
        End Try
    End Sub

    Public Sub DeleteLiqProbablesOrden(ByVal LiqProbablesOrden As LiqAsociadasOrden)
        Dim strUsuario As String = ""
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,LiqProbablesOrden.pstrUsuarioConexion, LiqProbablesOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not LiqProbablesOrden Is Nothing Then
                strUsuario = LiqProbablesOrden.Usuario
            End If

            LiqProbablesOrden.InfoSesion = DemeInfoSesion(strUsuario, "DeleteLiqProbablesOrden")
            Me.DataContext.LiqAsociadasOrdenes.Attach(LiqProbablesOrden)
            Me.DataContext.LiqAsociadasOrdenes.DeleteOnSubmit(LiqProbablesOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLiqProbablesOrden")
        End Try
    End Sub
#End Region

#Region "InstruccionesOrdenes"

    Public Function Consultar_CuentasClientes(ByVal plngIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasClientes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_CuentasClientes_Consultar(plngIdComitente, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_CuentasClientes")
            Return Nothing
        End Try
    End Function

    Public Function Consultar_InstruccionesOrdenes(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pintNroOrden As Integer, ByVal plngVersion As Integer,
                                                   ByVal pstrTopico As String, ByVal pstrEstadoMakerChecker As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of InstruccionesOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_InstruccionesOrdenes_Consultar(pstrTipo, pstrClase, pintNroOrden, plngVersion, pstrEstadoMakerChecker, pstrUsuario, DemeInfoSesion(pstrUsuario, "Consultar_InstruccionesOrdenes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            If ret.Count = 0 Then
                Dim objInstrucion As List(Of InstruccionesOrdene)
                objInstrucion = Traer_InstruccionesOrdenes_PorDefecto(pstrTopico, pstrUsuario, pstrInfoConexion).ToList
                Return objInstrucion
            Else
                Return ret
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_InstruccionesOrdenes")
            Return Nothing
        End Try
    End Function

    Public Function Traer_InstruccionesClientes(ByVal plngIdComitente As String, ByVal pstrTopico As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of InstruccionesOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_InstruccionesClientes_Consultar(plngIdComitente, pstrTopico, pstrUsuario).ToList
            If ret.Count = 0 Then
                Dim objInstrucion As List(Of InstruccionesOrdene)
                objInstrucion = Traer_InstruccionesOrdenes_PorDefecto(pstrTopico, pstrUsuario, pstrInfoConexion).ToList
                Return objInstrucion
            Else
                Return ret
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_InstruccionesClientes")
            Return Nothing
        End Try
    End Function

    Public Function Actualizar_InstruccionesOrdenes(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pintNroOrden As Integer, ByVal plngVersion As Integer,
                                                   ByVal plngIdBolsa As Integer, ByVal pstrInstruccionesOrdenesXML As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Instrucciones_Actualizar(pstrTipo, pstrClase, pintNroOrden, plngVersion, plngIdBolsa, pstrInstruccionesOrdenesXML, pstrUsuario, DemeInfoSesion(pstrUsuario, "Actualizar_InstruccionesOrdenes"), ClsConstantes.GINT_ErrorPersonalizado)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Actualizar_InstruccionesOrdenes")
            Return False
        End Try
    End Function

    Public Function Traer_InstruccionesOrdenes_PorDefecto(ByVal pstrTopico As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of InstruccionesOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret1 = Me.DataContext.uspOrdenes_ConsultarInstruccionesAcciones_OyDNet(pstrTopico, pstrUsuario).ToList
            Return ret1
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_InstruccionesClientes")
            Return Nothing
        End Try
    End Function


    Public Function TraerInstruccionesOrdenePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As InstruccionesOrdene
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New InstruccionesOrdene
            'e.IDComisionista = 
            'e.IdSucComisionista = 
            'e.Tipo = 
            'e.Clase = 
            'e.ID = 
            'e.Version = 
            'e.IdBolsa = 
            'e.Retorno = 
            'e.Instruccion = 
            'e.Cuenta = 
            'e.Valor = 
            'e.Seleccionado = 
            'e.Cumplido = 
            'e.Aprobado = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.IDInstruccionesOrdenes = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerInstruccionesOrdenePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertInstruccionesOrdene(ByVal InstruccionesOrdene As InstruccionesOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,InstruccionesOrdene.pstrUsuarioConexion, InstruccionesOrdene.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            InstruccionesOrdene.InfoSesion = DemeInfoSesion(InstruccionesOrdene.pstrUsuarioConexion, "InsertInstruccionesOrdene")
            Me.DataContext.InstruccionesOrdenes.InsertOnSubmit(InstruccionesOrdene)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertInstruccionesOrdene")
        End Try
    End Sub

    Public Sub UpdateInstruccionesOrdene(ByVal currentInstruccionesOrdene As InstruccionesOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentInstruccionesOrdene.pstrUsuarioConexion, currentInstruccionesOrdene.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentInstruccionesOrdene.InfoSesion = DemeInfoSesion(currentInstruccionesOrdene.pstrUsuarioConexion, "UpdateInstruccionesOrdene")
            Me.DataContext.InstruccionesOrdenes.Attach(currentInstruccionesOrdene, Me.ChangeSet.GetOriginal(currentInstruccionesOrdene))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateInstruccionesOrdene")
        End Try
    End Sub

    Public Sub DeleteInstruccionesOrdene(ByVal InstruccionesOrdene As InstruccionesOrdene)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,InstruccionesOrdene.pstrUsuarioConexion, InstruccionesOrdene.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Ordenes_InstruccionesOrdenes_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteInstruccionesOrdene"),0).ToList
            InstruccionesOrdene.InfoSesion = DemeInfoSesion(InstruccionesOrdene.pstrUsuarioConexion, "DeleteInstruccionesOrdene")
            Me.DataContext.InstruccionesOrdenes.Attach(InstruccionesOrdene)
            Me.DataContext.InstruccionesOrdenes.DeleteOnSubmit(InstruccionesOrdene)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteInstruccionesOrdene")
        End Try
    End Sub
#End Region

#Region "OrdenesPagos"

    Public Function OrdenesPagosConsultar(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pintNroOrden As Integer, ByVal plngVersion As Integer,
                                           ByVal pstrEstadoMakerChecker As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenesPago)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_OrdenesPagos_Consultar(pstrTipo, pstrClase, pintNroOrden, plngVersion, pstrEstadoMakerChecker, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesPagosFiltrar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesPagosFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerOrdenesPagoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As OrdenesPago
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New OrdenesPago
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.TipoOrden = 
            'e.ClaseOrden = 
            'e.IDOrden = 
            'e.Version = 
            'e.IdBolsa = 
            'e.FormaPago = 
            'e.CumpPesos = 
            'e.CumpTitulo = 
            'e.CumpPesos = 
            'e.CumpTitulo = 
            'e.DeposPesos = 
            'e.CtaSebraPesos = 
            'e.DeposTitulo = 
            'e.CtaTitulo = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.IDOrdenesPagos = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerOrdenesPagoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertOrdenesPago(ByVal OrdenesPago As OrdenesPago)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,OrdenesPago.pstrUsuarioConexion, OrdenesPago.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            OrdenesPago.InfoSesion = DemeInfoSesion(OrdenesPago.pstrUsuarioConexion, "InsertOrdenesPago")
            Me.DataContext.OrdenesPagos.InsertOnSubmit(OrdenesPago)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrdenesPago")
        End Try
    End Sub

    Public Sub UpdateOrdenesPago(ByVal currentOrdenesPago As OrdenesPago)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOrdenesPago.pstrUsuarioConexion, currentOrdenesPago.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentOrdenesPago.InfoSesion = DemeInfoSesion(currentOrdenesPago.pstrUsuarioConexion, "UpdateOrdenesPago")
            Me.DataContext.OrdenesPagos.Attach(currentOrdenesPago, Me.ChangeSet.GetOriginal(currentOrdenesPago))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrdenesPago")
        End Try
    End Sub

    Public Sub DeleteOrdenesPago(ByVal OrdenesPago As OrdenesPago)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,OrdenesPago.pstrUsuarioConexion, OrdenesPago.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Ordenes_OrdenesPagos_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteOrdenesPago"),0).ToList
            OrdenesPago.InfoSesion = DemeInfoSesion(OrdenesPago.pstrUsuarioConexion, "DeleteOrdenesPago")
            Me.DataContext.OrdenesPagos.Attach(OrdenesPago)
            Me.DataContext.OrdenesPagos.DeleteOnSubmit(OrdenesPago)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOrdenesPago")
        End Try
    End Sub
#End Region

#Region "AdicionalesOrdenes"

    Public Function Consultar_ComisionesOrdenes(ByVal plngIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Comisiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Comisiones_Consultar(plngIdComitente, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_ComisionesOrdenes")
            Return Nothing
        End Try
    End Function


    Public Function Consultar_AdicionalesOrdenes(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pintNroOrden As Integer, ByVal plngVersion As Integer,
                                                 ByVal pstrEstadoMakerChecker As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AdicionalesOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_AdicionalesOrdenes_Consultar(pstrTipo, pstrClase, pintNroOrden, plngVersion, pstrEstadoMakerChecker, pstrUsuario,
                                                                                    DemeInfoSesion(pstrUsuario, "Consultar_InstruccionesOrdenes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_AdicionalesOrdenes")
            Return Nothing
        End Try
    End Function


    Public Function TraerAdicionalesOrdenePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As AdicionalesOrdene
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New AdicionalesOrdene
            'e.IDComisionista = 
            'e.IdSucComisionista = 
            'e.Tipo = 
            'e.Clase = 
            'e.ID = 
            'e.Version = 
            'e.IdBolsa = 
            'e.IdComision = 
            'e.PorcCompra = 
            'e.PorcVenta = 
            'e.PorcOtro = 
            'e.IdOperacion = 
            'e.ComisionSugerida = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.IDAdicionalesOrdenes = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerAdicionalesOrdenePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertAdicionalesOrdene(ByVal AdicionalesOrdene As AdicionalesOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,AdicionalesOrdene.pstrUsuarioConexion, AdicionalesOrdene.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            AdicionalesOrdene.InfoSesion = DemeInfoSesion(AdicionalesOrdene.pstrUsuarioConexion, "InsertAdicionalesOrdene")
            Me.DataContext.AdicionalesOrdenes.InsertOnSubmit(AdicionalesOrdene)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertAdicionalesOrdene")
        End Try
    End Sub

    Public Sub UpdateAdicionalesOrdene(ByVal currentAdicionalesOrdene As AdicionalesOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentAdicionalesOrdene.pstrUsuarioConexion, currentAdicionalesOrdene.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentAdicionalesOrdene.InfoSesion = DemeInfoSesion(currentAdicionalesOrdene.pstrUsuarioConexion, "UpdateAdicionalesOrdene")
            Me.DataContext.AdicionalesOrdenes.Attach(currentAdicionalesOrdene, Me.ChangeSet.GetOriginal(currentAdicionalesOrdene))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateAdicionalesOrdene")
        End Try
    End Sub

    Public Sub DeleteAdicionalesOrdene(ByVal AdicionalesOrdene As AdicionalesOrdene)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,AdicionalesOrdene.pstrUsuarioConexion, AdicionalesOrdene.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Ordenes_AdicionalesOrdenes_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteAdicionalesOrdene"),0).ToList
            AdicionalesOrdene.InfoSesion = DemeInfoSesion(AdicionalesOrdene.pstrUsuarioConexion, "DeleteAdicionalesOrdene")
            Me.DataContext.AdicionalesOrdenes.Attach(AdicionalesOrdene)
            Me.DataContext.AdicionalesOrdenes.DeleteOnSubmit(AdicionalesOrdene)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteAdicionalesOrdene")
        End Try
    End Sub

#End Region

#Region "Procedimientos adicionales"

    ''' <summary>
    ''' Verificar el estado y condiciones de la orden para saber si es o no modificable
    ''' </summary>
    ''' <param name="pstrClaseOrden"></param>
    ''' <param name="pstrTipoOrden"></param>
    ''' <param name="pintNroOrden"></param>
    ''' <param name="pintVersion"></param>
    ''' <param name="pstrModulo">Módulo de OYD al cual está asociado órdenes</param>
    ''' <param name="pstrUsuario">Usuario activo</param>
    ''' <returns>Consulta con datos que indican si se puede o no modificar la orden</returns>
    ''' <remarks></remarks>
    ''' Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se añade el tipo de negocio para saber si la orden es de bolsa u otras firmas
    Public Function verificarOrdenModificable(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrModulo As String, ByVal pstrAccion As String, ByVal pstrUsuario As String, ByVal pstrTipoNegocio As String, ByVal pstrInfoConexion As String) As List(Of OrdenModificable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_VerificarEstadoOrden_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrModulo, pstrAccion, pstrUsuario, DemeInfoSesion(pstrUsuario, "verificarOrdenModificable"), ClsConstantes.GINT_ErrorPersonalizado, pstrTipoNegocio).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "verificarOrdenModificable")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarSaldoOrden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pcurSaldo As Double? = 0
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_SaldoOrden_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pcurSaldo, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarSaldoOrden"), ClsConstantes.GINT_ErrorPersonalizado)
            Return CDbl(pcurSaldo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarSaldoOrden")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta los recpetores del comitente cuando se ingresa una nueva orden o se cambia el comitente de la oirden
    ''' </summary>
    ''' <param name="pstrClaseOrden"></param>
    ''' <param name="pstrTipoOrden"></param>
    ''' <param name="pstrIdComitente"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Traer_ReceptoresOrdenes_Cliente(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pstrIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrIdComitente) AndAlso Not pstrIdComitente.Trim().Equals(String.Empty) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_ReceptoresCliente_Consultar(pstrClaseOrden, pstrTipoOrden, pstrIdComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresOrdenes_Cliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresOrdenes_Cliente")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta los recpetores del comitente cuando se ingresa una nueva orden o se cambia el comitente de la oirden
    ''' </summary>
    ''' <param name="pstrClaseOrden">Indica si la orden es de acciones o renta fija</param>
    ''' <param name="pstrTipoOrden">Indica si la orden es de compra o venta</param>
    ''' <param name="pstrIdComitente">Código del comitente seleccionado</param>
    ''' <param name="pstrUsuario">Usuario activo</param>
    ''' <returns>Returna los receptores que se pueden adicionar a la orden según la configuración en OYD</returns>
    ''' 
    Public Function Traer_Receptores_Cliente(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pstrIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrIdComitente) AndAlso Not pstrIdComitente.Trim().Equals(String.Empty) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_ReceptoresCliente_Consultar(pstrClaseOrden, pstrTipoOrden, pstrIdComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_Receptores_Cliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_Receptores_Cliente")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Verifica si una fecha específica corresponde o no a un día hábil
    ''' </summary>
    ''' <param name="pdtmFecha">Fecha que debe ser validada</param>
    ''' <param name="pstrUsuario">Usuario activo</param>
    ''' <returns></returns>
    ''' 
    Public Function VerificarFechaHabil(ByVal pdtmFecha As Date, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidarFecha)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_VerificarDiaHabil(pdtmFecha).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Verificar_FechaHabil")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Calcula los días de diferencia entre dos fechas o la fecha final a partir de una fecha inicial y un número de días
    ''' En ambos casos solamente se tienen en cuenta días hábiles
    ''' </summary>
    ''' <param name="pstrTipoCalculo">Indica el tipo de cálculo para encontrar el resultado</param>
    ''' <param name="pdtmFechaInicial">Fecha desde la cual se inicia a calcular</param>
    ''' <param name="pdtmFechaFinal">Fecha hasta la cual se calcula o fecha que se calcula. Para clacularlar enviar Nothing.</param>
    ''' <param name="pintNroDias">Nro. de días claculados o días para calcular la fecha final. Si la fecha final es Nothin se usan los días para calcularla.</param>
    ''' <param name="pstrUsuario">Usuario activo</param>
    ''' <returns></returns>
    ''' 
    Public Function CalcularDiasHabiles(ByVal pstrTipoCalculo As String, ByVal pdtmFechaInicial As System.Nullable(Of Date), ByVal pdtmFechaFinal As System.Nullable(Of Date), ByVal pintNroDias As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidarFecha)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_CalcularDiasOrden(pstrTipoCalculo, pdtmFechaInicial, pdtmFechaFinal, pintNroDias, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Calcular_DiasHabiles")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consultar los datos de la orden en SAE
    ''' </summary>
    ''' <param name="pstrClase">Clase de la orden (renta fija, acciones)</param>
    ''' <param name="strTipo">Tipo de la orden (compra, venta, ...)</param>
    ''' <param name="plngIDOrden">Número de la orden</param>
    ''' <param name="pstrUsuario">Usuario activo</param>
    ''' <returns></returns>
    ''' 
    Public Function ConsultarOrdenSAE(ByVal pstrClase As String, ByVal strTipo As String, ByVal plngIDOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenSAE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_ConsultarOrdenSAE(pstrClase, strTipo, plngIDOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "Consultar_OrdenSAE"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_OrdenSAE")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consultar los datos de la orden en SAE
    ''' </summary>
    ''' <param name="pstrClase">Clase de la orden (renta fija, acciones)</param>
    ''' <param name="strTipo">Tipo de la orden (compra, venta, ...)</param>
    ''' <param name="plngIDOrden">Número de la orden</param>
    ''' <param name="pstrUsuario">Usuario activo</param>
    ''' <returns></returns>
    ''' 
    Public Function enrutar_OrdenSAE(ByVal pstrClase As String, ByVal strTipo As String, ByVal plngIDOrden As Integer, ByVal plogAnular As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenSAE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_EnrutarOrdenSAE(pstrClase, strTipo, plngIDOrden, plogAnular, pstrUsuario, DemeInfoSesion(pstrUsuario, "enrutar_OrdenSAE"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "enrutar_OrdenSAE")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consultar los datos de la orden en SAE
    ''' </summary>
    ''' <param name="pstrClase">Clase de la orden (renta fija, acciones)</param>
    ''' <param name="strTipo">Tipo de la orden (compra, venta, ...)</param>
    ''' <param name="pintNroOrden">Número de la orden</param>
    ''' <param name="pstrUsuario">Usuario activo</param>
    ''' <returns></returns>
    ''' 
    '''Santiago Alexander Vergara Orrego - Mayo 15/2014 - Se añade el tipo de negocio para saber si la orden es de bolsa u otras firmas
    Public Function ConsultarLiquidacionesAsociadasOrden(ByVal pstrClase As String, ByVal strTipo As String, ByVal pintNroOrden As Integer, ByVal pstrUsuario As String, ByVal pstrTipoNegocio As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_LiquidacionesAsociadas(pstrClase, strTipo, pintNroOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarLiquidacionesAsociadasOrden"), ClsConstantes.GINT_ErrorPersonalizado, pstrTipoNegocio).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarLiquidacionesAsociadasOrden")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Se valida si el usuario de la aplicación tiene permisos para duplicar una orden
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidarUsuario_DuplicarOrden(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_DuplicarOrden(pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarUsuario_DuplicarOrden"), ClsConstantes.GINT_ErrorPersonalizado)
            Return CBool(ret)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarUsuario_DuplicarOrden")
            Return False
        End Try
    End Function

#End Region

#Region "Ordenes_MILA"

    Public Function OrdenesFiltrar_MI(ByVal pstrClase As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Orden_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_MI_Filtrar(pstrClase, pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesFiltrar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesConsultar_MI(ByVal pstrClase As String, ByVal pstrTipo As String, ByVal pintNroOrden As System.Nullable(Of Integer), ByVal pintVersion As System.Nullable(Of Integer),
                                     ByVal pstrIDComitente As String, ByVal pstrIDOrdenante As String, ByVal pdtmOrden As System.Nullable(Of Date), ByVal pstrFormaPago As String,
                                     ByVal pstrObjeto As String, ByVal pstrCondicionesNegociacion As String, ByVal pstrTipoInversion As String,
                                     ByVal pstrCanalRecepcion As String, ByVal pstrMedioVerificable As String, ByVal pdtmFechaHoraRecepcion As System.Nullable(Of Date),
                                     ByVal pstrTipoLimite As String, ByVal pdtmVigenciaHasta As System.Nullable(Of Date),
                                     ByVal pstrEstado As String, ByVal pstrEstadoOrdenBus As String, ByVal pstrEstadoMakerChecker As String, ByVal pstrAccionMakerChecker As String,
                                     ByVal plngIdBolsa As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Orden_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_MI_Consultar(pstrClase, pstrTipo, pintNroOrden, pintVersion, pstrIDComitente, pstrIDOrdenante, pdtmOrden, pstrFormaPago,
                                      pstrObjeto, pstrCondicionesNegociacion, pstrTipoInversion, pstrCanalRecepcion, pstrMedioVerificable, pdtmFechaHoraRecepcion,
                                      pstrTipoLimite, pdtmVigenciaHasta, pstrEstado, pstrEstadoOrdenBus, pstrEstadoMakerChecker, pstrAccionMakerChecker, plngIdBolsa, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarOrdenes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarOrdenes")
            Return Nothing
        End Try
    End Function

    Public Function Traer_ReceptoresOrdenes_MI_Orden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_ReceptoresOrdenes_MI_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresOrdenes_MI_Orden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresOrdenes_MI_Orden")
            Return Nothing
        End Try
    End Function

    Public Function Verificar_ReceptoresOrdenes_MI_Orden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_ReceptoresOrdenes_MI_Verificar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Verificar_ReceptoresOrdenes_MI_Orden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Verificar_ReceptoresOrdenes_MI_Orden")
            Return Nothing
        End Try
    End Function

    Public Function Traer_BeneficiariosOrdenes_MI_Orden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiariosOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_BeneficiariosOrdenes_MI_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_BeneficiariosOrdenes_Orden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosOrdenes_Orden")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Consulta los recpetores del comitente cuando se ingresa una nueva orden o se cambia el comitente de la oirden
    ''' </summary>
    ''' <param name="pstrClaseOrden"></param>
    ''' <param name="pstrTipoOrden"></param>
    ''' <param name="pstrIdComitente"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Traer_ReceptoresOrdenes_MI_Cliente(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pstrIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrIdComitente) AndAlso Not pstrIdComitente.Trim().Equals(String.Empty) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_ReceptoresCliente_Consultar(pstrClaseOrden, pstrTipoOrden, pstrIdComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresOrdenes_Cliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresOrdenes_Cliente")
            Return Nothing
        End Try
    End Function

    Public Function TraerOrden_MIPorDefecto(ByVal pstrClaseOrden As String, ByVal pstrUsuario As String,
                                         ByVal pstrTipoOrden As String, ByVal pstrInfoConexion As String) As Orden_MI
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Orden_MI
            Dim objVig As List(Of ValidarFecha)
            Dim objEsHabil As List(Of ValidarDiaHabil)

            '/ Buscar la fecha de vigencia por defecto para las nuevas órdenes. Se toma fecha de elaboración por defecto la fecha actual y una vigencia de 5 días
            objVig = CalcularDiasHabiles("vencimiento_orden", Now(), Nothing, 5, pstrUsuario, pstrInfoConexion).ToList

            e.Clase = pstrClaseOrden
            If Not pstrTipoOrden.Trim.Equals(String.Empty) Then
                e.Tipo = pstrTipoOrden
            End If
            e.Version = 0
            e.Estado = "P"
            e.Ordinaria = True
            e.Objeto = ""
            e.CondicionesNegociacion = "C"
            e.TipoLimite = "M"
            e.Ejecucion = "N"
            e.EstadoLEO = "R"
            e.Duracion = "I"
            e.TipoInversion = "N"
            e.Instrucciones = ""
            e.Notas = ""
            e.FechaOrden = Now()
            If IsNothing(objVig) Then
                e.VigenciaHasta = DateAdd(DateInterval.Day, 4, Now()) '/ Se suman 4 días para incluir la fecha actual
            Else
                If objVig.Count > 0 Then
                    e.VigenciaHasta = objVig.First.FechaFinal
                Else
                    e.VigenciaHasta = DateAdd(DateInterval.Day, 4, Now())
                End If
            End If
            objEsHabil = ValidarDiaHabil(Now.Date, pstrUsuario, pstrInfoConexion).ToList

            e.IdBolsa = 0
            e.Repo = False
            e.TieneOrdenRelacionada = False

            e.FormaPago = Nothing
            e.IDComitente = Nothing
            e.IDOrdenante = Nothing
            e.Nemotecnico = Nothing
            e.UBICACIONTITULO = Nothing
            e.CuentaDeposito = Nothing
            e.FormaPago = Nothing
            e.MedioVerificable = Nothing
            e.CanalRecepcion = Nothing
            e.UsuarioOperador = Nothing
            e.NroExtensionToma = Nothing
            e.IdReceptorToma = Nothing
            e.FechaRecepcion = Nothing

            If Not IsNothing(objEsHabil) Then
                e.EsHabil = objEsHabil.First.EsDiaHabil
                If Not e.EsHabil Then
                    e.FechaOrden = CDate(objEsHabil.First.MenorFechaHabilOrden)
                End If
            End If

            e.Eca = False
            e.OrdenEscrito = Nothing
            e.CostoAdicionalesOrden = 0
            e.SitioIngreso = Nothing
            e.DescripcionOrden = "..."
            e.NombreCliente = "..."
            e.AccionMakerAndChecker = "I"
            e.EstadoMakerChecker = "PA"
            e.IDPreliquidacion = 0
            e.Renovacion = False
            e.IpOrigen = Nothing
            e.NegocioEspecial = Nothing
            e.Moneda = 0

            'e.ReceptoresOrdenes = Nothing
            'e.LiqAsociadasOrdenes = Nothing
            'e.BeneficiariosOrdenes = Nothing

            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerOrden_MIPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertOrden(ByVal Orden As Orden_MI)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Orden.pstrUsuarioConexion, Orden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not Orden Is Nothing Then
                strUsuario = Orden.Usuario
            End If

            Orden.InfoSesion = DemeInfoSesion(strUsuario, "InsertOrden")
            Me.DataContext.Ordenes_MI.InsertOnSubmit(Orden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrden")
        End Try
    End Sub

    Public Sub UpdateOrden(ByVal currentOrden As Orden_MI)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOrden.pstrUsuarioConexion, currentOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not currentOrden Is Nothing Then
                strUsuario = currentOrden.Usuario
            End If

            currentOrden.InfoSesion = DemeInfoSesion(strUsuario, "UpdateOrden")
            Me.DataContext.Ordenes_MI.InsertOnSubmit(currentOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrden")
        End Try
    End Sub

    Public Function AutorizarOrden_MI(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal plogAprobar As Boolean, ByVal pintClasificacionRiesgoEspecie As Integer, ByVal pintPerfilInversionista As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenSAE)
        Dim intIdOrden As Integer = 1
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_MI_Autorizar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, plogAprobar, pintClasificacionRiesgoEspecie, pintPerfilInversionista, intIdOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "Autorizar_Orden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Autorizar_Orden")
            Return Nothing
        End Try
    End Function

    Public Function AnularOrden_MI(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrInstrucciones As String, ByVal pstrNotas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenSAE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_MI_Anular(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrInstrucciones, pstrNotas, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularOrden")
            Return Nothing
        End Try
    End Function


#Region "Procedimientos adicionales mila"

    ''' <summary>
    ''' Verificar el estado y condiciones de la orden para saber si es o no modificable
    ''' </summary>
    ''' <param name="pstrClaseOrden"></param>
    ''' <param name="pstrTipoOrden"></param>
    ''' <param name="pintNroOrden"></param>
    ''' <param name="pintVersion"></param>
    ''' <param name="pstrModulo">Módulo de OYD al cual está asociado órdenes</param>
    ''' <param name="pstrUsuario">Usuario activo</param>
    ''' <returns>Consulta con datos que indican si se puede o no modificar la orden</returns>
    ''' <remarks></remarks>
    Public Function verificarOrdenModificable_MI(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrModulo As String, ByVal pstrAccion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenModificable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_VerificarEstadoOrden_MI_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrModulo, pstrAccion, pstrUsuario, DemeInfoSesion(pstrUsuario, "verificarOrdenModificable"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "verificarOrdenModificable")
            Return Nothing
        End Try
    End Function

    Public Function ValidarDiaHabil(ByVal pdtmFechaOrden As Date, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidarDiaHabil)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_CalcularDiasHabiles(pdtmFechaOrden, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarDiaHabil")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Consultar las liquidaciones asociadas a la Orden de MILA
    ''' </summary>
    ''' <param name="pstrClase">Clase de la orden (renta fija, acciones)</param>
    ''' <param name="strTipo">Tipo de la orden (compra, venta, ...)</param>
    ''' <param name="pintNroOrden">Número de la orden</param>
    ''' <param name="pstrUsuario">Usuario activo</param>
    ''' <returns></returns>
    ''' 
    Public Function ConsultarLiquidacionesAsociadasOrden_MI(ByVal pstrClase As String, ByVal strTipo As String, ByVal pintNroOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Ordenes_MI_LiquidacionesAsociadas(pstrClase, strTipo, pintNroOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarLiquidacionesAsociadasOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarLiquidacionesAsociadasOrden")
            Return Nothing
        End Try
    End Function


#End Region

#End Region

#Region "Modulo OF"
#Region "Ordenes"

    Public Function OrdenesOFFiltrar(ByVal pstrClase As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_OrdenesOF_Filtrar(pstrClase, pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesOFFiltrar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesOFFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesOFConsultar(ByVal pstrClase As String, ByVal pstrTipo As String, ByVal pintNroOrden As System.Nullable(Of Integer), ByVal pintVersion As System.Nullable(Of Integer),
                                     ByVal pstrIDComitente As String, ByVal pstrIDOrdenante As String, ByVal pdtmOrden As System.Nullable(Of Date), ByVal pstrFormaPago As String,
                                     ByVal pstrObjeto As String, ByVal pstrCondicionesNegociacion As String, ByVal pstrTipoInversion As String,
                                     ByVal pstrTipoLimite As String, ByVal pdtmVigenciaHasta As System.Nullable(Of Date),
                                     ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_OrdenesOF_Consultar(pstrClase, pstrTipo, pintNroOrden, pintVersion, pstrIDComitente, pstrIDOrdenante, pdtmOrden, pstrFormaPago,
                                      pstrObjeto, pstrCondicionesNegociacion, pstrTipoInversion, pstrTipoLimite, pdtmVigenciaHasta, pstrEstado, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarOrdenes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesOFConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerOrdenesOFPorDefecto(ByVal pstrClaseOrden As String, ByVal pstrUsuario As String, ByVal pstrTipoOrden As String, ByVal pstrInfoConexion As String) As OrdenesOF
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New OrdenesOF
            Dim objVig As List(Of ValidarFecha)

            '/ Buscar la fecha de vigencia por defecto para las nuevas órdenes. Se toma fecha de elaboración por defecto la fecha actual y una vigencia de 5 días
            objVig = CalcularDiasHabiles("vencimiento_orden", Now(), Nothing, 5, pstrUsuario, pstrInfoConexion).ToList

            e.Clase = pstrClaseOrden
            If Not pstrTipoOrden.Trim.Equals(String.Empty) Then
                e.Tipo = pstrTipoOrden
            End If
            e.Version = 0
            e.Estado = "P"
            e.Ordinaria = True
            e.Objeto = ""
            e.CondicionesNegociacion = "C"
            e.TipoLimite = "M"
            e.TipoInversion = "N"
            e.Instrucciones = ""
            e.Notas = ""
            e.FechaOrden = Now()
            If IsNothing(objVig) Then
                e.VigenciaHasta = DateAdd(DateInterval.Day, 4, Now()) '/ Se suman 4 días para incluir la fecha actual
            Else
                If objVig.Count > 0 Then
                    e.VigenciaHasta = objVig.First.FechaFinal
                Else
                    e.VigenciaHasta = DateAdd(DateInterval.Day, 4, Now())
                End If
            End If
            e.Repo = False

            e.FormaPago = Nothing
            e.IDComitente = Nothing
            e.IDOrdenante = Nothing
            e.Nemotecnico = Nothing
            e.UBICACIONTITULO = Nothing
            e.CuentaDeposito = Nothing
            e.FormaPago = Nothing
            e.DescripcionOrden = "..."
            e.NombreCliente = "..."
            e.IDPreliquidacion = 0
            e.EnPesos = False
            e.Renovacion = False
            e.DividendoCompra = False
            e.ComisionPactada = 0

            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerOrdenesOFPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertOrdenOF(ByVal OrdenesOF As OrdenesOF)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,OrdenesOF.pstrUsuarioConexion, OrdenesOF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not OrdenesOF Is Nothing Then
                strUsuario = OrdenesOF.Usuario
            End If

            OrdenesOF.InfoSesion = DemeInfoSesion(strUsuario, "InsertOrdenOF")
            Me.DataContext.OrdenesOF.InsertOnSubmit(OrdenesOF)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrdenOF")
        End Try
    End Sub

    Public Sub UpdateOrdenOF(ByVal currentOrdenesOF As OrdenesOF)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOrdenesOF.pstrUsuarioConexion, currentOrdenesOF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not currentOrdenesOF Is Nothing Then
                strUsuario = currentOrdenesOF.Usuario
            End If

            currentOrdenesOF.InfoSesion = DemeInfoSesion(strUsuario, "UpdateOrdenOF")
            Me.DataContext.OrdenesOF.InsertOnSubmit(currentOrdenesOF)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrdenOF")
        End Try
    End Sub

    Public Function AnularOrdenesOF(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrInstrucciones As String, ByVal pstrNotas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenSAE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_OrdenesOF_Anular(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrInstrucciones, pstrNotas, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularOrdenesOF")
            Return Nothing
        End Try
    End Function

    Public Function Traer_BeneficiariosOrdenes_OrdenesOF(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiarioOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_BeneficiariosOrdenesOF_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_BeneficiariosOrdenes_OrdenesOF"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosOrdenes_OrdenesOF")
            Return Nothing
        End Try
    End Function

    Public Function Traer_BeneficiariosOrdenesOF_Cliente(ByVal plngIdComitente As String, ByVal pintNroCuenta As Integer, ByVal pstrDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiarioOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_BeneficiariosCuentaDep_Consultar(plngIdComitente, pintNroCuenta, pstrDeposito).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosOrdenesOF_Cliente")
            Return Nothing
        End Try
    End Function

    Public Function Traer_ReceptoresOrdenesOF_OrdenesOF(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptorOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_ReceptoresOrdenesOF_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresOrdenesOF_OrdenesOF"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresOrdenesOF_OrdenesOF")
            Return Nothing
        End Try
    End Function

    Public Function Verificar_ReceptoresOrdenesOF_Orden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptorOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_ReceptoresOrdenesOF_Verificar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Verificar_ReceptoresOrdenes_Orden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Verificar_ReceptoresOrdenesOF_Orden")
            Return Nothing
        End Try
    End Function

    Public Function Traer_ReceptoresOrdenesOF_Cliente(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pstrIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptorOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrIdComitente) AndAlso Not pstrIdComitente.Trim().Equals(String.Empty) Then
                Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_ReceptoresCliente_Consultar(pstrClaseOrden, pstrTipoOrden, pstrIdComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresOrdenes_Cliente"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresOrdenesOF_Cliente")
            Return Nothing
        End Try
    End Function

    Public Function TraerReceptorOrdenesOFPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ReceptorOrdenesOF
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ReceptorOrdenesOF

            e.IDReceptor = Nothing
            e.Lider = False
            e.Porcentaje = 0

            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReceptoresOrdenPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertReceptorOrdenesOF(ByVal ReceptoresOrden As ReceptorOrdenesOF)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrden.pstrUsuarioConexion, ReceptoresOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not ReceptoresOrden Is Nothing Then
                strUsuario = ReceptoresOrden.Usuario
            End If

            ReceptoresOrden.InfoSesion = DemeInfoSesion(strUsuario, "InsertReceptoresOrden")
            Me.DataContext.ReceptoresOrdenOF.InsertOnSubmit(ReceptoresOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertReceptoresOrden")
        End Try
    End Sub

    Public Sub UpdateReceptorOrdenesOF(ByVal currentReceptoresOrden As ReceptorOrdenesOF)
        Dim strUsuario As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentReceptoresOrden.pstrUsuarioConexion, currentReceptoresOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not currentReceptoresOrden Is Nothing Then
                strUsuario = currentReceptoresOrden.Usuario
            End If

            currentReceptoresOrden.InfoSesion = DemeInfoSesion(strUsuario, "UpdateReceptoresOrden")
            Me.DataContext.ReceptoresOrdenOF.Attach(currentReceptoresOrden, Me.ChangeSet.GetOriginal(currentReceptoresOrden))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptoresOrden")
        End Try
    End Sub

    Public Sub DeleteReceptorOrdenesOF(ByVal ReceptoresOrden As ReceptorOrdenesOF)
        Dim strUsuario As String = ""
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrden.pstrUsuarioConexion, ReceptoresOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not ReceptoresOrden Is Nothing Then
                strUsuario = ReceptoresOrden.Usuario
            End If

            ReceptoresOrden.InfoSesion = DemeInfoSesion(strUsuario, "DeleteReceptoresOrden")
            Me.DataContext.ReceptoresOrdenOF.Attach(ReceptoresOrden)
            Me.DataContext.ReceptoresOrdenOF.DeleteOnSubmit(ReceptoresOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptoresOrden")
        End Try
    End Sub

    Public Function ConsultarLiquidacionesAsociadasOrdenOF(ByVal pstrClase As String, ByVal strTipo As String, ByVal pintNroOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOrdenOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_OrdenesOF_LiquidacionesAsociadas(pstrClase, strTipo, pintNroOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarLiquidacionesAsociadasOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarLiquidacionesAsociadasOrdenOF")
            Return Nothing
        End Try
    End Function

    Public Function verificarOrdenModificableOF(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrModulo As String, ByVal pstrAccion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenOFModificable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesOF_VerificarEstadoOrdenOF_Consultar(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrModulo, pstrAccion, pstrUsuario, DemeInfoSesion(pstrUsuario, "verificarOrdenModificableOF"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "verificarOrdenModificableOF")
            Return Nothing
        End Try
    End Function

#End Region

    '#Region "ReceptoresOrdenes"

    '    Public Function TraerReceptoresOrdenPorDefecto( ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS ReceptoresOrden
    '        Try
    '            Dim e As New ReceptoresOrden

    '            e.IDReceptor = Nothing
    '            e.Lider = False
    '            e.Porcentaje = 0

    '            Return e
    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "TraerReceptoresOrdenPorDefecto")
    '            Return Nothing
    '        End Try
    '    End Function

    '    Public Sub InsertReceptoresOrden(ByVal ReceptoresOrden As ReceptoresOrden)
    '        Dim strUsuario As String = ""
    '        Try
    '            If Not ReceptoresOrden Is Nothing Then
    '                strUsuario = ReceptoresOrden.Usuario
    '            End If

    '            ReceptoresOrden.InfoSesion = DemeInfoSesion(pstrUsuario, "InsertReceptoresOrden", strUsuario)
    '            Me.DataContext.ReceptoresOrdenes.InsertOnSubmit(ReceptoresOrden)
    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "InsertReceptoresOrden")
    '        End Try
    '    End Sub

    '    Public Sub UpdateReceptoresOrden(ByVal currentReceptoresOrden As ReceptoresOrden)
    '        Dim strUsuario As String = ""
    '        Try
    '            If Not currentReceptoresOrden Is Nothing Then
    '                strUsuario = currentReceptoresOrden.Usuario
    '            End If

    '            currentReceptoresOrden.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateReceptoresOrden", strUsuario)
    '            Me.DataContext.ReceptoresOrdenes.Attach(currentReceptoresOrden, Me.ChangeSet.GetOriginal(currentReceptoresOrden))
    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "UpdateReceptoresOrden")
    '        End Try
    '    End Sub

    '    Public Sub DeleteReceptoresOrden(ByVal ReceptoresOrden As ReceptoresOrden)
    '        Dim strUsuario As String = ""
    '        Try
    '            If Not ReceptoresOrden Is Nothing Then
    '                strUsuario = ReceptoresOrden.Usuario
    '            End If

    '            ReceptoresOrden.InfoSesion = DemeInfoSesion(pstrUsuario, "DeleteReceptoresOrden", strUsuario)
    '            Me.DataContext.ReceptoresOrdenes.Attach(ReceptoresOrden)
    '            Me.DataContext.ReceptoresOrdenes.DeleteOnSubmit(ReceptoresOrden)
    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "DeleteReceptoresOrden")
    '        End Try
    '    End Sub
    '#End Region
#End Region

#Region "Función Generica"

    ''' <summary>
    ''' Función que permite ejecutar Querys en SQL Server con o sin parametros.
    ''' </summary>
    ''' <param name="strSQL"></param>
    ''' <param name="strParametro"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130604</remarks>
    Private Function fnEjecutarQuerySQL(ByVal strSQL As String, Optional strParametro As Object = Nothing) As String
        Dim L2SDC As New OyDOrdenesDataContext
        Dim result As String = String.Empty

        If IsNothing(strParametro) Then
            Dim res = L2SDC.ExecuteQuery(Of Resultado)(strSQL).FirstOrDefault
            If Not IsNothing(res) Then
                result = CStr(res.strCampo)
            End If
        Else
            Dim res = L2SDC.ExecuteQuery(Of Resultado)(strSQL, strParametro).FirstOrDefault
            If Not IsNothing(res) Then
                result = CStr(res.strCampo)
            End If
        End If

        Return result
    End Function

    Public Class Resultado
        Public strCampo As Object
    End Class

#End Region

#Region "Re-complementacion"

    'TODO:
    ' Consider constraining the results of your query method.  If you need additional input you can
    ' add parameters to this method or create additional query methods with different names.
    Public Function GetTblDistribucions(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of tblDistribucion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.tblDistribucions
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetTblDistribucions")
            Return Nothing
        End Try
    End Function

    'TODO:
    ' Consider constraining the results of your query method.  If you need additional input you can
    ' add parameters to this method or create additional query methods with different names.
    Public Function GetTblDistribucionClientes(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of tblDistribucionCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.tblDistribucionClientes
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetTblDistribucionClientes")
            Return Nothing
        End Try
    End Function

    'TODO:
    ' Consider constraining the results of your query method.  If you need additional input you can
    ' add parameters to this method or create additional query methods with different names.
    Public Function GetTblDistribucionPrecios(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of tblDistribucionPrecio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.tblDistribucionPrecios
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetTblDistribucionPrecios")
            Return Nothing
        End Try
    End Function

    'TODO:
    ' Consider constraining the results of your query method.  If you need additional input you can
    ' add parameters to this method or create additional query methods with different names.
    Public Function GetTblResultadoCalculoRecomplementacions(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of tblResultadoCalculoRecomplementacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.tblResultadoCalculoRecomplementacions
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetTblResultadoCalculoRecomplementacions")
            Return Nothing
        End Try
    End Function

    'TODO:
    ' Consider constraining the results of your query method.  If you need additional input you can
    ' add parameters to this method or create additional query methods with different names.
    Public Function GetTblResultadoCargaRecomplementacions(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of tblResultadoCargaRecomplementacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.tblResultadoCargaRecomplementacions
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetTblResultadoCargaRecomplementacions")
            Return Nothing
        End Try
    End Function

    'TODO:
    ' Consider constraining the results of your query method.  If you need additional input you can
    ' add parameters to this method or create additional query methods with different names.
    Public Function GetTblValidacions(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of tblValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.tblValidacions
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetTblValidacions")
            Return Nothing
        End Try
    End Function

    'TODO:
    ' Consider constraining the results of your query method.  If you need additional input you can
    ' add parameters to this method or create additional query methods with different names.
    Public Function GetTblValidacionProcesos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of tblValidacionProceso)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.tblValidacionProcesos
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetTblValidacionProcesos")
            Return Nothing
        End Try
    End Function

    Public Function CargarArchivoRecomplementacion(ByVal pstrRuta As String, ByVal pstrNombreArchivo As String, ByVal pintIdOrden As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblResultadoCargaRecomplementacion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'ADICIONAR INFOSESION.
            Dim ret = Me.DataContext.uspOyDNet_CargarArchivoRecomplementacion(pstrRuta, pstrNombreArchivo, pintIdOrden, "")

            Dim objListaValidaciones As List(Of tblValidacion) = ret.GetResult(Of tblValidacion)().ToList
            Dim objListaDistribucion As List(Of tblDistribucion) = ret.GetResult(Of tblDistribucion)().ToList


            Dim objRespuesta As tblResultadoCargaRecomplementacion = New tblResultadoCargaRecomplementacion

            objRespuesta.intID = 1

            If objListaDistribucion IsNot Nothing Then
                objRespuesta.tblDistribucions = New EntitySet(Of tblDistribucion)
                objRespuesta.tblDistribucions.AddRange(objListaDistribucion)
            End If

            If objListaValidaciones IsNot Nothing Then
                objRespuesta.tblValidacions = New EntitySet(Of tblValidacion)
                objRespuesta.tblValidacions.AddRange(objListaValidaciones)
            End If

            Return objRespuesta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoRecomplementacion")
            Return Nothing
        End Try
    End Function

    Public Function CalcularDistribucionRecomplementacion(ByVal pstrRuta As String, ByVal pstrNombreArchivo As String, ByVal pintIdOrden As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblResultadoCalculoRecomplementacion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'ADICIONAR INFOSESION.
            Dim ret = Me.DataContext.uspOyDNet_CalcularRecomplementacion(pstrRuta, pstrNombreArchivo, pintIdOrden, "")

            Dim objDistribucionCliente As List(Of tblDistribucionCliente) = ret.GetResult(Of tblDistribucionCliente)().ToList
            Dim objDistribucionPrecio As List(Of tblDistribucionPrecio) = ret.GetResult(Of tblDistribucionPrecio)().ToList
            Dim objValidaciones As List(Of tblValidacionProceso) = ret.GetResult(Of tblValidacionProceso)().ToList

            Dim objRespuesta As tblResultadoCalculoRecomplementacion = New tblResultadoCalculoRecomplementacion

            objRespuesta.intID = 1

            If objDistribucionCliente IsNot Nothing Then
                objRespuesta.tblDistribucionClientes = New EntitySet(Of tblDistribucionCliente)
                objRespuesta.tblDistribucionClientes.AddRange(objDistribucionCliente)
            End If

            If objDistribucionPrecio IsNot Nothing Then
                objRespuesta.tblDistribucionPrecios = New EntitySet(Of tblDistribucionPrecio)
                objRespuesta.tblDistribucionPrecios.AddRange(objDistribucionPrecio)
            End If

            If objValidaciones IsNot Nothing Then
                objRespuesta.tblValidacionProcesos = New EntitySet(Of tblValidacionProceso)
                objRespuesta.tblValidacionProcesos.AddRange(objValidaciones)
            End If

            Return objRespuesta

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalcularDistribucionRecomplementacion")
            Return Nothing
        End Try

    End Function

    Public Function ActualizarRecomplementacion(ByVal pstrRuta As String, ByVal pstrNombreArchivo As String, ByVal pintIdOrden As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim ret = Me.DataContext.uspOyDNet_ActualizarRecomplementacion(pstrRuta, pstrNombreArchivo, pintIdOrden, "")

            Return True

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarRecomplementacion")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Flujo de Ordenes"
    Public Sub InsertFlujoOrdenes(ByVal objFlujoOrdenes As FlujoOrdenes)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertFlujoOrdenes")
        End Try
    End Sub

    Public Sub UpdateFlujoOrdenes(ByVal objFlujoOrdenes As FlujoOrdenes)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateFlujoOrdenes")
        End Try
    End Sub

    Public Sub DeleteFlujoOrdenes(ByVal objFlujoOrdenes As FlujoOrdenes)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteFlujoOrdenes")
        End Try
    End Sub

    Public Sub InsertOrdenesFraccionamiento(ByVal objOrdenesFraccionamiento As OrdenesFraccionamiento)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrdenesFraccionamiento")
        End Try
    End Sub

    Public Sub UpdateOrdenesFraccionamiento(ByVal objOrdenesFraccionamiento As OrdenesFraccionamiento)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrdenesFraccionamiento")
        End Try
    End Sub

    Public Sub DeleteOrdenesFraccionamiento(ByVal objOrdenesFraccionamiento As OrdenesFraccionamiento)
        Try
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOrdenesFraccionamiento")
        End Try
    End Sub

    Public Function FlujoOrdenes_Consultar(ByVal pstrBrokerTrader As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of FlujoOrdenes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FlujosOrden_Consultar(pstrBrokerTrader, pstrUsuario, DemeInfoSesion(pstrUsuario, "FlujoOrdenes_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FlujoOrdenes_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function FlujoOrdenes_ConsultarFraccionamiento(ByVal pintIDOrden As Integer, ByVal pstrBrokerTrader As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenesFraccionamiento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FlujosOrden_ConsultarFraccionamiento(pintIDOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "FlujoOrdenes_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FlujoOrdenes_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function FlujoOrdenes_ActualizarTradeConfirmation(ByVal pstrRegistrosActualizar As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FlujosOrden_ActualizarTradeConfirmation(pstrRegistrosActualizar, pstrUsuario, DemeInfoSesion(pstrUsuario, "FlujoOrdenes_ActualizarTradeConfirmation"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FlujoOrdenes_ActualizarTradeConfirmation")
            Return Nothing
        End Try
    End Function

    Public Function FlujoOrdenes_ActualizarFraccionamiento(ByVal pstrRegistrosActualizar As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FlujosOrden_ActualizarFraccionamiento(pstrRegistrosActualizar, pstrUsuario, DemeInfoSesion(pstrUsuario, "FlujoOrdenes_ActualizarFraccionamiento"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FlujoOrdenes_ActualizarFraccionamiento")
            Return Nothing
        End Try
    End Function

    Public Function FlujoOrdenes_ActualizarFraccionamientoOrden(ByVal pintIDOrden As Integer, ByVal pstrRegistrosActualizar As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FlujosOrden_ActualizarFraccionamientoOrden(pintIDOrden, pstrRegistrosActualizar, pstrUsuario, DemeInfoSesion(pstrUsuario, "FlujoOrdenes_ActualizarFraccionamientoOrden"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FlujoOrdenes_ActualizarFraccionamientoOrden")
            Return Nothing
        End Try
    End Function

    Public Function FlujoOrdenes_DeshacerOrden(ByVal pstrRegistrosActualizar As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FlujosOrden_DeshacerOrden(pstrRegistrosActualizar, pstrUsuario, DemeInfoSesion(pstrUsuario, "FlujoOrdenes_DeshacerOrden"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FlujoOrdenes_DeshacerOrden")
            Return Nothing
        End Try
    End Function

    Public Function FlujoOrdenes_ActualizarPreMatched(ByVal pstrRegistrosActualizar As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FlujosOrden_ActualizarPreMatched(pstrRegistrosActualizar, pstrUsuario, DemeInfoSesion(pstrUsuario, "uspOyDNet_FlujosOrden_ActualizarPreMatched"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "uspOyDNet_FlujosOrden_ActualizarPreMatched")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Trazabilidad Bancolombia"
    Public Function TraerClientesTrazabilidad(ByVal plngIdOrden As Integer, ByVal pstrClaseOrden As String, ByVal pstrTipo As String, ByVal pstrcambio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Trazabilidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spOrdenesConsultarTrazabilidad(plngIdOrden, pstrClaseOrden, pstrTipo, pstrcambio).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesTrazabilidad")
            Return Nothing
        End Try
    End Function
#End Region

End Class