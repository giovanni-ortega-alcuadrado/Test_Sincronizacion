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
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesBolsa
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()> _
Partial Public Class OYDPLUSOrdenesBolsaDomainService
    Inherits LinqToSqlDomainService(Of OyDPLUSOrdenesBolsaDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Ordenes"

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

#End Region

#Region "Seteador"

    Public Function ObtenerOrdenesSeteador(ByVal pstrUsuario As String, ByVal pintVista As Integer, ByVal pstrInfoConexion As String) As List(Of OrdenSeteador)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Seteador(pstrUsuario, pintVista, DemeInfoSesion(pstrUsuario, "ObtenerOrdenesSeteador"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerOrdenesSeteador")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesSeteadorEstadoActualizar(ByVal pintIdOrdenes As Integer, ByVal pstrUsuario As String, ByVal pstrEstado As String, ByVal pstrEstadoLEO As String, ByVal pstrEstadoVisor As String, ByVal pintFolderVisor As Integer, blnError As Boolean, ByVal pstrInfoConexion As String) As List(Of OrdenSeteador)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret As List(Of OrdenSeteador)
            If pstrEstado.Length > 0 Then
                ret = Me.DataContext.uspOyDNet_OrdenesSeteadorEstado_Actualizar(pintIdOrdenes, pstrUsuario, CType(pstrEstado, Char?), pstrEstadoLEO, pstrEstadoVisor, pintFolderVisor, blnError, DemeInfoSesion(pstrUsuario, "OrdenesSeteadorEstadoActualizar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Else
                ret = Me.DataContext.uspOyDNet_OrdenesSeteadorEstado_Actualizar(pintIdOrdenes, pstrUsuario, Nothing, pstrEstadoLEO, pstrEstadoVisor, pintFolderVisor, blnError, DemeInfoSesion(pstrUsuario, "OrdenesSeteadorEstadoActualizar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            End If
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesSeteadorEstadoActualizar")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesSeteadorBorrar(ByVal pintIdOrdenes As Integer, ByVal plngIDOrden As Integer, ByVal pstrUsuario As String, ByVal pintFolderVisor As Integer, ByVal pstrObservaciones As String, ByVal pstrInfoConexion As String) As List(Of OrdenSeteador)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesSeteador_Borrar(pintIdOrdenes, plngIDOrden, pstrUsuario, pintFolderVisor, pstrObservaciones, DemeInfoSesion(pstrUsuario, "OrdenesSeteadorBorrar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesSeteadorBorrar")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesSeteadorAsignarLiquidacionesProbables(ByVal pstrXMLLiquidaciones As String, pintNroOrden As Integer, ByVal pstrUsuario As String, ByVal pdtmLiquidacion As DateTime, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_LiquidacionesProbables_Ingresar(pstrXMLLiquidaciones, pintNroOrden, pdtmLiquidacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesSeteadorAsignarLiquidacionesProbables"), ClsConstantes.GINT_ErrorPersonalizado)
            Dim variable = CStr(ret)
            Return variable
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesSeteadorAsignarLiquidacionesProbables")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesSeteadorObtenerCambiosVisor(pintNumeroOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblMensajes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Obtener_Datos_Visor_Seteador_Campos_Modificados(pintNumeroOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesSeteadorObtenerCambiosVisor"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesSeteadorObtenerCambiosVisor")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesSeteadorObtenerListaTopico(pstrTopico As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblMensajes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_Seteador_Obtener_Lista_Por_Topico(pstrTopico, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesSeteadorObtenerListaTopico"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesSeteadorObtenerListaTopico")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesSeteadorObtenerLiquidacionesDisponibles(pintNumeroOrden As Integer, pstrTrader As String, pstrTipoNegocio As String, pstrTipo As Char?, pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarOrdenesLiquidacionesDisponibles(pintNumeroOrden, pstrTrader, pstrTipoNegocio, pstrTipo, pstrEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesSeteadorObtenerLiquidacionesDisponibles"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesSeteadorObtenerLiquidacionesDisponibles")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesSeteadorCalceAutomatico(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_LiquidacionesProbables_Calce_Automatico(pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesSeteadorObtenerLiquidacionesDisponibles"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesSeteadorCalceAutomatico")
            Return Nothing
        End Try
    End Function

    Public Function ValidarDisponibilidadOrden(ByVal pintIDDocumento As Integer, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EstadoFlujoOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_VerificarDisponibilidadOrden(pintIDDocumento, pstrEstado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarDisponibilidadOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarDisponibilidadOrden")
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
    Public Function enrutar_OrdenSAE_Seteador(ByVal pstrClase As String, ByVal strTipo As String, ByVal plngIDOrden As Integer, ByVal plogAnular As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenSAE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_EnrutarOrdenSAE_Desde_Seteador(pstrClase, strTipo, plngIDOrden, plogAnular, pstrUsuario, DemeInfoSesion(pstrUsuario, "enrutar_OrdenSAE"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "enrutar_OrdenSAE_Seteador")
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

            ReceptoresOrden.InfoSesion = DemeInfoSesion(ReceptoresOrden.pstrUsuarioConexion, "InsertReceptoresOrden")
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
    ''' Consulta las liquidaciones probables de la orden
    ''' </summary>
    ''' <param name="pstrClaseOrden"></param>
    ''' <param name="pstrTipoOrden"></param>
    ''' <param name="pintNroOrden"></param>
    ''' <param name="pintVersion"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Traer_LiqProbables_Orden(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, _
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

#End Region

#Region "OYDPLUS"

    Public Sub InsertOrdenOYDPlus(ByVal Orden As OrdenOYDPLUS)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Orden.pstrUsuarioConexion, Orden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenOYDPLUS.InsertOnSubmit(Orden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrdenOYDPlus")
        End Try
    End Sub

    Public Sub UpdateOrdenOYDPlus(ByVal currentOrden As OrdenOYDPLUS)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOrden.pstrUsuarioConexion, currentOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenOYDPLUS.InsertOnSubmit(currentOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrdenOYDPlus")
        End Try
    End Sub

    Public Function OYDPLUS_OrdenPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As OrdenOYDPLUS
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret As New OrdenOYDPLUS
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_OrdenPorDefecto")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function OYDPLUS_ValidarIngresoOrden(ByVal pIDOrden As Integer, ByVal plngID As Integer, ByVal pstrBolsa As String, ByVal pstrReceptor As String, ByVal pstrTipoOrden As String, _
                                                ByVal pstrTipoNegocio As String, ByVal pstrTipoProducto As String, ByVal pstrTipoOperacion As String, _
                                                ByVal pstrClase As String, ByVal pdtmOrden As System.Nullable(Of DateTime), ByVal pstrEstadoOrden As String, ByVal pstrEstadoLEO As String, ByVal pstrClasificacion As String, ByVal pstrTipoLimite As String, _
                                                ByVal pstrDuracion As String, ByVal pdtmFechaVigencia As System.Nullable(Of DateTime), ByVal pstrHoraVigencia As String, ByVal pintDiasVigencia As System.Nullable(Of Integer), ByVal pstrCondicionesNegociacion As String, ByVal pstrFormaPago As String, _
                                                ByVal pstrTipoInversion As String, ByVal pstrEjecucion As String, ByVal pstrMercado As String, ByVal pstrIDComitente As String, _
                                                ByVal pstrIDOrdenante As String, ByVal pstrUBICACIONTITULO As String, ByVal pintCuentaDeposito As System.Nullable(Of Integer), _
                                                ByVal pstrUsuarioOperador As String, ByVal pstrCanalRecepcion As String, ByVal pstrMedioVerificable As String, _
                                                ByVal pdtmFechaHoraRecepcion As System.Nullable(Of DateTime), ByVal pstrNroExtensionToma As String, ByVal pstrEspecie As String, ByVal pstrISIN As String, _
                                                ByVal pdtmFechaEmision As System.Nullable(Of DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of DateTime), ByVal plogEstandarizada As System.Nullable(Of Boolean), _
                                                ByVal pdtmFechaCumplimiento As System.Nullable(Of DateTime), ByVal pdblTasaFacial As System.Nullable(Of Double), _
                                                ByVal pstrModalidad As String, ByVal pstrIndicador As String, ByVal pdblPuntosIndicador As System.Nullable(Of Double), _
                                                ByVal plogEnPesos As System.Nullable(Of Boolean), ByVal pdblCantidad As System.Nullable(Of Double), ByVal pdblPrecio As System.Nullable(Of Double), _
                                                ByVal pdblPrecioMaximoMinimo As System.Nullable(Of Double), ByVal pdblValorCaptacionGiro As System.Nullable(Of Double), _
                                                ByVal pdblValorFuturoRepo As System.Nullable(Of Double), _
                                                ByVal pdblTasaRegistro As System.Nullable(Of Double), ByVal pdblTasaCliente As System.Nullable(Of Double), ByVal pdblTasaNominal As System.Nullable(Of Double), _
                                                ByVal pdblCastigo As System.Nullable(Of Double), ByVal pdblValorAccion As System.Nullable(Of Double), ByVal pdblComision As System.Nullable(Of Double), ByVal pdblValorComision As System.Nullable(Of Double), ByVal pdblValorOrden As System.Nullable(Of Double), ByVal pintDiasRepo As System.Nullable(Of Integer), _
                                                ByVal pintProductoValores As System.Nullable(Of Integer), ByVal pdblCostosAdicionales As System.Nullable(Of Double), ByVal pstrInstrucciones As String, ByVal pstrNotas As String, ByVal pstrReceptoresXML As String, _
                                                ByVal pstrLiquidacionesAsociadasXML As String, ByVal pstrPagosOrdenesXML As String, ByVal pstrInstruccionesOrdenesXML As String, ByVal pstrComisionesOrdenesXML As String, _
                                                ByVal pstrConfirmaciones As String, ByVal pstrConfirmacionesUsuario As String, ByVal pstrJustificaciones As String, ByVal pstrJustificacionesUsuario As String, ByVal pstrAprobaciones As String, ByVal pstrAprobacionesUsuario As String, _
                                                ByVal pintCustodia As System.Nullable(Of Integer), ByVal pintSecuenciaCustodia As System.Nullable(Of Integer), _
                                                ByVal pintDiasCumplimiento As System.Nullable(Of Integer), ByVal pstrRuedaNegocio As String, ByVal pdblPrecioLimpio As System.Nullable(Of Double), _
                                                ByVal pstrEstadoTitulo As String, ByVal pdblIvaComision As System.Nullable(Of Double), ByVal pdblValorFuturoCliente As System.Nullable(Of Double), ByVal pstrReceptoresCruzadasXML As String, ByVal plogOrdenCruzada As System.Nullable(Of Boolean), ByVal plogOrdenCruzadaCliente As System.Nullable(Of Boolean), _
                                                ByVal plogOrdenCruzadaReceptor As System.Nullable(Of Boolean), ByVal pintIDOrdenOriginal As System.Nullable(Of Integer), _
                                                ByVal plogGuardarComoPlantilla As Boolean, ByVal pstrNombrePlantilla As String, _
                                                ByVal pstrBrokenTrader As String, ByVal pstrEntidad As String, ByVal pstrEstrategia As String, _
                                                ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String,
                                                ByVal pdtmFechaConstancia As System.Nullable(Of DateTime), pdblConstancia As System.Nullable(Of Double), ByVal pstrIDComitenteADR As String,
                                                ByVal pstrReceptorToma As String, ByVal pdtmFechaSalida As Nullable(Of DateTime), ByVal pstrTipoGarantia As String,
                                                ByVal plogEliminarAsociacion As System.Nullable(Of Boolean),
                                                ByVal pstrExistePreacuerdo As String,
                                                ByVal pstrVendeTodo As String,
                                                ByVal plogExentoRetencion As System.Nullable(Of Boolean),
                                                ByVal pdblPorcentajePagoEfectivo As System.Nullable(Of Double),
                                                ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ReceptoresXML = HttpUtility.HtmlDecode(pstrReceptoresXML)
            Dim LiquidacionesAsociadasXML = HttpUtility.HtmlDecode(pstrLiquidacionesAsociadasXML)
            Dim PagosOrdenesXML = HttpUtility.HtmlDecode(pstrPagosOrdenesXML)
            Dim InstruccionesOrdenesXML = HttpUtility.HtmlDecode(pstrInstruccionesOrdenesXML)
            Dim ComisionesOrdenesXML = HttpUtility.HtmlDecode(pstrComisionesOrdenesXML)
            Dim ReceptoresOrdenesCruzadasXML = HttpUtility.HtmlDecode(pstrReceptoresCruzadasXML)
            Dim NombrePlantilla = HttpUtility.HtmlDecode(pstrNombrePlantilla)

            Dim ret = Me.DataContext.uspOyDNet_ValidarOrden(pIDOrden, plngID, pstrBolsa, pstrReceptor, pstrTipoOrden, pstrTipoNegocio, pstrTipoProducto, pstrTipoOperacion, pstrClase, _
                                                            pdtmOrden, pstrEstadoOrden, pstrEstadoLEO, pstrClasificacion, pstrTipoLimite, pstrDuracion, pdtmFechaVigencia, pstrHoraVigencia, pintDiasVigencia, pstrCondicionesNegociacion, pstrFormaPago, pstrTipoInversion, _
                                                            pstrEjecucion, pstrMercado, pstrIDComitente, pstrIDOrdenante, pstrUBICACIONTITULO, pintCuentaDeposito, pstrUsuarioOperador, _
                                                            pstrCanalRecepcion, pstrMedioVerificable, pdtmFechaHoraRecepcion, pstrNroExtensionToma, pstrEspecie, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, plogEstandarizada, pdtmFechaCumplimiento, _
                                                            pdblTasaFacial, pstrModalidad, pstrIndicador, pdblPuntosIndicador, plogEnPesos, pdblCantidad, pdblPrecio, _
                                                            pdblPrecioMaximoMinimo, pdblValorCaptacionGiro, pdblValorFuturoRepo, pdblTasaRegistro, pdblTasaCliente, pdblTasaNominal, pdblCastigo, pdblComision, pdblValorComision, pdblValorAccion, pdblValorOrden, pintDiasRepo, _
                                                            pintProductoValores, pdblCostosAdicionales, pstrInstrucciones, pstrNotas, _
                                                            ReceptoresXML, LiquidacionesAsociadasXML, PagosOrdenesXML, InstruccionesOrdenesXML, ComisionesOrdenesXML, _
                                                            pstrConfirmaciones, pstrConfirmacionesUsuario, pstrJustificaciones, pstrJustificacionesUsuario, pstrAprobaciones, pstrAprobacionesUsuario, pintCustodia, pintSecuenciaCustodia, _
                                                            pintDiasCumplimiento, pstrRuedaNegocio, pdblPrecioLimpio, pstrEstadoTitulo, pdblIvaComision, pdblValorFuturoCliente, ReceptoresOrdenesCruzadasXML, plogOrdenCruzada, plogOrdenCruzadaCliente, plogOrdenCruzadaReceptor, pintIDOrdenOriginal, _
                                                            pstrMaquina, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ValidarIngresoOrden"), 0, plogGuardarComoPlantilla, NombrePlantilla, pstrBrokenTrader, pstrEntidad, pstrEstrategia, pstrUsuarioWindows, pdtmFechaConstancia, pdblConstancia, pstrIDComitenteADR,
                                                            pstrReceptorToma, pdtmFechaSalida, pstrTipoGarantia, plogEliminarAsociacion, pstrExistePreacuerdo, pstrVendeTodo, plogExentoRetencion, pdblPorcentajePagoEfectivo)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ValidarIngresoOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_FiltrarOrden(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUS)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FiltrarOrden(pstrEstado, pstrFiltro, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_FiltrarOrden"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_FiltrarOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarOrdenesCruzadasUsuario(ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUS)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FiltrarOrden("C", String.Empty, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrdenesCruzadasUsuario"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarOrdenesCruzadasUsuario")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarOrden(ByVal pstrModulo As String, ByVal pstrEstado As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrEstadoOrden As String, ByVal pstrReceptor As String, ByVal pstrTipoOrden As String,
                                           ByVal pstrTipoNegocio As String, ByVal pstrTipoOperacion As String, ByVal pstrTipoProducto As String, ByVal pstrIDComitente As String,
                                           ByVal pstrUsuario As String, pdtmOrden As Nullable(Of DateTime), ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUS)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarOrden(pstrModulo, pstrEstado, pintNroOrden, pintVersion, pstrEstadoOrden, pstrReceptor, pstrTipoOrden, pstrTipoNegocio, pstrTipoOperacion, pstrTipoProducto, pstrIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrden"), 0, pdtmOrden).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_AnularOrdenOYDPLUS(ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrInstrucciones As String, ByVal pstrNotas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenSAE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pstrClaseOrden) Then
                Dim ret = Me.DataContext.uspOyDNet_AnularOrdenOYDPLUS(pstrClaseOrden, pstrTipoOrden, pintNroOrden, pintVersion, pstrInstrucciones, pstrNotas, True, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularOrdenOYDPLUS"), ClsConstantes.GINT_ErrorPersonalizado).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularOrdenOYDPLUS")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_CancelarOrdenOYDPLUS(ByVal plngID As Integer, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CancelarEdicionOrden(plngID, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_CancelarOrdenOYDPLUS"), ClsConstantes.GINT_ErrorPersonalizado)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CancelarOrdenOYDPLUS")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarMejorPrecioEspecie_Orden(ByVal pstrTipoOperacion As String, ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrTipoNegocio As String, ByVal pstrInfoConexion As String) As List(Of MejorPrecioEspecieOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarMejorPrecioEspecie(pstrTipoOperacion, pstrEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarMejorPrecioEspecie_Orden"), ClsConstantes.GINT_ErrorPersonalizado, pstrTipoNegocio).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarMejorPrecioEspecie_Orden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarReceptoresOrdenesPorCruzar(ByVal pintIDOrden As Integer, ByVal plogPorAprobar As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblReceptoresOrdenesPorCruzar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarReceptoresOrdenesPorCruzar(pintIDOrden, plogPorAprobar, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarReceptoresOrdenesPorCruzar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarReceptoresOrdenesPorCruzar")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarOrdenesCruzadas(ByVal pintIDOrden As Integer, ByVal plogOrdenOriginal As Boolean, ByVal pstrUsuario As String,
                                                     ByVal plogValidarCambioReceptor As Boolean,
                                                     ByVal pstrReceptorId As String, ByVal pstrInfoConexion As String) As List(Of tblOrdenesCruzadas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarOrdenesCruzadas(pintIDOrden, plogOrdenOriginal, pstrUsuario,
                                                                        DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrdenesCruzadas"),
                                                                        ClsConstantes.GINT_ErrorPersonalizado,
                                                                        plogValidarCambioReceptor,
                                                                        pstrReceptorId).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarOrdenesCruzadas")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatetblReceptoresOrdenesPorCruzar(ByVal currentOrden As tblReceptoresOrdenesPorCruzar)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatetblReceptoresOrdenesPorCruzar")
        End Try
    End Sub

    Public Function OYDPLUS_ValidarExistenciaOrdenCruzada(ByVal pstrUsuarioConsulta As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ValidarExistenciaOrdenesCruzadas(pstrUsuarioConsulta, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarExistenciaOrdenCruzada"), ClsConstantes.GINT_ErrorPersonalizado)
            If ret = 1 Then Return True Else Return False
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarExistenciaOrdenCruzada")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_OrdenesPlantillasConsultar(ByVal pstrFiltro As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUS)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesPlantillas_Consultar(pstrFiltro, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_OrdenesPlantillasConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_OrdenesPlantillasConsultar")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_OrdenesPlantillasEliminar(ByVal pstrIDEliminar As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesPlantillas_Eliminar(pstrIDEliminar, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_OrdenesPlantillasEliminar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_OrdenesPlantillasEliminar")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_OrdenesPlantillasVerificarNombre(ByVal pstrNombrePlantilla As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim logRetorno As Boolean? = False
            Me.DataContext.uspOyDNet_OrdenesPlantillas_VerificarNombre(pstrNombrePlantilla, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_OrdenesPlantillasVerificarNombre"), 0, logRetorno)

            Return CBool(logRetorno)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_OrdenesPlantillasVerificarNombre")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function OYDPLUS_CalcuarValorOrden(ByVal pstrTipoCalculo As String, ByVal pstrReceptor As String, ByVal pstrTipoOrden As String, _
                                              ByVal pstrTipoNegocio As String, ByVal pstrTipoProducto As String, ByVal pstrTipoOperacion As String, _
                                              ByVal pstrClase As String, ByVal pdtmOrden As System.Nullable(Of DateTime), ByVal pstrClasificacion As String, ByVal pstrTipoLimite As String, _
                                              ByVal pstrDuracion As String, ByVal pdtmFechaVigencia As System.Nullable(Of DateTime), ByVal pstrHoraVigencia As String, ByVal pintDiasVigencia As System.Nullable(Of Integer), ByVal pstrCondicionesNegociacion As String, ByVal pstrFormaPago As String, _
                                              ByVal pstrTipoInversion As String, ByVal pstrEjecucion As String, ByVal pstrMercado As String, ByVal pstrIDComitente As String, _
                                              ByVal pstrIDOrdenante As String, ByVal pstrUBICACIONTITULO As String, ByVal pintCuentaDeposito As System.Nullable(Of Integer), _
                                              ByVal pstrUsuarioOperador As String, ByVal pstrCanalRecepcion As String, ByVal pstrMedioVerificable As String, _
                                              ByVal pdtmFechaHoraRecepcion As System.Nullable(Of DateTime), ByVal pstrNroExtensionToma As String, ByVal pstrEspecie As String, ByVal pstrISIN As String, _
                                              ByVal pdtmFechaEmision As System.Nullable(Of DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of DateTime), ByVal plogEstandarizada As System.Nullable(Of Boolean), _
                                              ByVal pdtmFechaCumplimiento As System.Nullable(Of DateTime), ByVal pdblTasaFacial As System.Nullable(Of Double), _
                                              ByVal pstrModalidad As String, ByVal pstrIndicador As String, ByVal pdblPuntosIndicador As System.Nullable(Of Double), _
                                              ByVal plogEnPesos As System.Nullable(Of Boolean), ByVal pdblCantidad As System.Nullable(Of Double), ByVal pdblPrecio As System.Nullable(Of Double), _
                                              ByVal pdblPrecioMaximoMinimo As System.Nullable(Of Double), ByVal pdblValorCaptacionGiro As System.Nullable(Of Double), _
                                              ByVal pdblValorFuturoRepo As System.Nullable(Of Double), _
                                              ByVal pdblTasaRegistro As System.Nullable(Of Double), ByVal pdblTasaCliente As System.Nullable(Of Double), ByVal pdblTasaNominal As System.Nullable(Of Double), _
                                              ByVal pdblCastigo As System.Nullable(Of Double), ByVal pdblValorAccion As System.Nullable(Of Double), ByVal pdblComision As System.Nullable(Of Double), ByVal pdblValorComision As System.Nullable(Of Double), ByVal pdblValorOrden As System.Nullable(Of Double), ByVal pintDiasRepo As System.Nullable(Of Integer), _
                                              ByVal pintProductoValores As System.Nullable(Of Integer), ByVal pdblCostosAdicionales As System.Nullable(Of Double),
                                              ByVal pintDiasCumplimiento As System.Nullable(Of Integer),
                                              ByVal pdblIvaComision As System.Nullable(Of Double), ByVal pdblValorFuturoCliente As System.Nullable(Of Double),
                                              ByVal pstrBrokenTrader As String, ByVal pstrEntidad As String, ByVal pstrEstrategia As String, _
                                              ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String,
                                              ByVal pdtmFechaConstancia As System.Nullable(Of DateTime), pdblConstancia As System.Nullable(Of Double), ByVal pstrIDComitenteADR As String,
                                              ByVal pstrReceptorToma As String, ByVal pdtmFechaSalida As Nullable(Of DateTime), ByVal pstrTipoGarantia As String, ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUS_Calculos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CalcularValorOrden(pstrTipoCalculo, pstrReceptor, pstrTipoOrden, pstrTipoNegocio, pstrTipoProducto, pstrTipoOperacion, pstrClase,
                                                                  pdtmOrden, pstrClasificacion, pstrTipoLimite, pstrDuracion, pdtmFechaVigencia, pstrHoraVigencia, pintDiasVigencia, pstrCondicionesNegociacion, pstrFormaPago, pstrTipoInversion,
                                                                  pstrEjecucion, pstrMercado, pstrIDComitente, pstrIDOrdenante, pstrUBICACIONTITULO, pintCuentaDeposito, pstrUsuarioOperador,
                                                                  pstrCanalRecepcion, pstrMedioVerificable, pdtmFechaHoraRecepcion, pstrNroExtensionToma, pstrEspecie, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, plogEstandarizada, pdtmFechaCumplimiento,
                                                                  pdblTasaFacial, pstrModalidad, pstrIndicador, pdblPuntosIndicador, plogEnPesos, pdblCantidad, pdblPrecio,
                                                                  pdblPrecioMaximoMinimo, pdblValorCaptacionGiro, pdblValorFuturoRepo, pdblTasaRegistro, pdblTasaCliente, pdblTasaNominal, pdblCastigo, pdblComision, pdblValorComision, pdblValorAccion, pdblValorOrden, pintDiasRepo,
                                                                  pintProductoValores, pdblCostosAdicionales,
                                                                  pintDiasCumplimiento, pdblIvaComision, pdblValorFuturoCliente,
                                                                  pstrBrokenTrader, pstrEntidad, pstrEstrategia, pstrUsuarioWindows, pdtmFechaConstancia, pdblConstancia, pstrIDComitenteADR,
                                                                  pstrReceptorToma, pstrMaquina, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_CalcuarValorOrden"), 0, pdtmFechaSalida, pstrTipoGarantia)

            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_CalcuarValorOrden")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function OYDPLUS_CalcuarValorOrdenSync(ByVal pstrTipoCalculo As String, ByVal pstrReceptor As String, ByVal pstrTipoOrden As String, _
                                              ByVal pstrTipoNegocio As String, ByVal pstrTipoProducto As String, ByVal pstrTipoOperacion As String, _
                                              ByVal pstrClase As String, ByVal pdtmOrden As System.Nullable(Of DateTime), ByVal pstrClasificacion As String, ByVal pstrTipoLimite As String, _
                                              ByVal pstrDuracion As String, ByVal pdtmFechaVigencia As System.Nullable(Of DateTime), ByVal pstrHoraVigencia As String, ByVal pintDiasVigencia As System.Nullable(Of Integer), ByVal pstrCondicionesNegociacion As String, ByVal pstrFormaPago As String, _
                                              ByVal pstrTipoInversion As String, ByVal pstrEjecucion As String, ByVal pstrMercado As String, ByVal pstrIDComitente As String, _
                                              ByVal pstrIDOrdenante As String, ByVal pstrUBICACIONTITULO As String, ByVal pintCuentaDeposito As System.Nullable(Of Integer), _
                                              ByVal pstrUsuarioOperador As String, ByVal pstrCanalRecepcion As String, ByVal pstrMedioVerificable As String, _
                                              ByVal pdtmFechaHoraRecepcion As System.Nullable(Of DateTime), ByVal pstrNroExtensionToma As String, ByVal pstrEspecie As String, ByVal pstrISIN As String, _
                                              ByVal pdtmFechaEmision As System.Nullable(Of DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of DateTime), ByVal plogEstandarizada As System.Nullable(Of Boolean), _
                                              ByVal pdtmFechaCumplimiento As System.Nullable(Of DateTime), ByVal pdblTasaFacial As System.Nullable(Of Double), _
                                              ByVal pstrModalidad As String, ByVal pstrIndicador As String, ByVal pdblPuntosIndicador As System.Nullable(Of Double), _
                                              ByVal plogEnPesos As System.Nullable(Of Boolean), ByVal pdblCantidad As System.Nullable(Of Double), ByVal pdblPrecio As System.Nullable(Of Double), _
                                              ByVal pdblPrecioMaximoMinimo As System.Nullable(Of Double), ByVal pdblValorCaptacionGiro As System.Nullable(Of Double), _
                                              ByVal pdblValorFuturoRepo As System.Nullable(Of Double), _
                                              ByVal pdblTasaRegistro As System.Nullable(Of Double), ByVal pdblTasaCliente As System.Nullable(Of Double), ByVal pdblTasaNominal As System.Nullable(Of Double), _
                                              ByVal pdblCastigo As System.Nullable(Of Double), ByVal pdblValorAccion As System.Nullable(Of Double), ByVal pdblComision As System.Nullable(Of Double), ByVal pdblValorComision As System.Nullable(Of Double), ByVal pdblValorOrden As System.Nullable(Of Double), ByVal pintDiasRepo As System.Nullable(Of Integer), _
                                              ByVal pintProductoValores As System.Nullable(Of Integer), ByVal pdblCostosAdicionales As System.Nullable(Of Double),
                                              ByVal pintDiasCumplimiento As System.Nullable(Of Integer),
                                              ByVal pdblIvaComision As System.Nullable(Of Double), ByVal pdblValorFuturoCliente As System.Nullable(Of Double),
                                              ByVal pstrBrokenTrader As String, ByVal pstrEntidad As String, ByVal pstrEstrategia As String, _
                                              ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String,
                                              ByVal pdtmFechaConstancia As System.Nullable(Of DateTime), pdblConstancia As System.Nullable(Of Double), ByVal pstrIDComitenteADR As String,
                                              ByVal pstrReceptorToma As String, ByVal pdtmFechaSalida As Nullable(Of DateTime), ByVal pstrTipoGarantia As String, ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUS_Calculos)
        Dim objTask As Task(Of List(Of OrdenOYDPLUS_Calculos)) = Me.OYDPLUS_CalcuarValorOrdenAsync(pstrTipoCalculo, pstrReceptor, pstrTipoOrden, pstrTipoNegocio, pstrTipoProducto, pstrTipoOperacion, pstrClase,
                                                                  pdtmOrden, pstrClasificacion, pstrTipoLimite, pstrDuracion, pdtmFechaVigencia, pstrHoraVigencia, pintDiasVigencia, pstrCondicionesNegociacion, pstrFormaPago, pstrTipoInversion,
                                                                  pstrEjecucion, pstrMercado, pstrIDComitente, pstrIDOrdenante, pstrUBICACIONTITULO, pintCuentaDeposito, pstrUsuarioOperador,
                                                                  pstrCanalRecepcion, pstrMedioVerificable, pdtmFechaHoraRecepcion, pstrNroExtensionToma, pstrEspecie, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, plogEstandarizada, pdtmFechaCumplimiento,
                                                                  pdblTasaFacial, pstrModalidad, pstrIndicador, pdblPuntosIndicador, plogEnPesos, pdblCantidad, pdblPrecio,
                                                                  pdblPrecioMaximoMinimo, pdblValorCaptacionGiro, pdblValorFuturoRepo, pdblTasaRegistro, pdblTasaCliente, pdblTasaNominal, pdblCastigo, pdblValorAccion, pdblComision, pdblValorComision, pdblValorOrden, pintDiasRepo,
                                                                  pintProductoValores, pdblCostosAdicionales,
                                                                  pintDiasCumplimiento, pdblIvaComision, pdblValorFuturoCliente,
                                                                  pstrBrokenTrader, pstrEntidad, pstrEstrategia, pstrMaquina, pstrUsuario, pstrUsuarioWindows,
                                                                  pdtmFechaConstancia, pdblConstancia, pstrIDComitenteADR, pstrReceptorToma, pdtmFechaSalida, pstrTipoGarantia, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function OYDPLUS_CalcuarValorOrdenAsync(ByVal pstrTipoCalculo As String, ByVal pstrReceptor As String, ByVal pstrTipoOrden As String, _
                                              ByVal pstrTipoNegocio As String, ByVal pstrTipoProducto As String, ByVal pstrTipoOperacion As String, _
                                              ByVal pstrClase As String, ByVal pdtmOrden As System.Nullable(Of DateTime), ByVal pstrClasificacion As String, ByVal pstrTipoLimite As String, _
                                              ByVal pstrDuracion As String, ByVal pdtmFechaVigencia As System.Nullable(Of DateTime), ByVal pstrHoraVigencia As String, ByVal pintDiasVigencia As System.Nullable(Of Integer), ByVal pstrCondicionesNegociacion As String, ByVal pstrFormaPago As String, _
                                              ByVal pstrTipoInversion As String, ByVal pstrEjecucion As String, ByVal pstrMercado As String, ByVal pstrIDComitente As String, _
                                              ByVal pstrIDOrdenante As String, ByVal pstrUBICACIONTITULO As String, ByVal pintCuentaDeposito As System.Nullable(Of Integer), _
                                              ByVal pstrUsuarioOperador As String, ByVal pstrCanalRecepcion As String, ByVal pstrMedioVerificable As String, _
                                              ByVal pdtmFechaHoraRecepcion As System.Nullable(Of DateTime), ByVal pstrNroExtensionToma As String, ByVal pstrEspecie As String, ByVal pstrISIN As String, _
                                              ByVal pdtmFechaEmision As System.Nullable(Of DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of DateTime), ByVal plogEstandarizada As System.Nullable(Of Boolean), _
                                              ByVal pdtmFechaCumplimiento As System.Nullable(Of DateTime), ByVal pdblTasaFacial As System.Nullable(Of Double), _
                                              ByVal pstrModalidad As String, ByVal pstrIndicador As String, ByVal pdblPuntosIndicador As System.Nullable(Of Double), _
                                              ByVal plogEnPesos As System.Nullable(Of Boolean), ByVal pdblCantidad As System.Nullable(Of Double), ByVal pdblPrecio As System.Nullable(Of Double), _
                                              ByVal pdblPrecioMaximoMinimo As System.Nullable(Of Double), ByVal pdblValorCaptacionGiro As System.Nullable(Of Double), _
                                              ByVal pdblValorFuturoRepo As System.Nullable(Of Double), _
                                              ByVal pdblTasaRegistro As System.Nullable(Of Double), ByVal pdblTasaCliente As System.Nullable(Of Double), ByVal pdblTasaNominal As System.Nullable(Of Double), _
                                              ByVal pdblCastigo As System.Nullable(Of Double), ByVal pdblValorAccion As System.Nullable(Of Double), ByVal pdblComision As System.Nullable(Of Double), ByVal pdblValorComision As System.Nullable(Of Double), ByVal pdblValorOrden As System.Nullable(Of Double), ByVal pintDiasRepo As System.Nullable(Of Integer), _
                                              ByVal pintProductoValores As System.Nullable(Of Integer), ByVal pdblCostosAdicionales As System.Nullable(Of Double),
                                              ByVal pintDiasCumplimiento As System.Nullable(Of Integer),
                                              ByVal pdblIvaComision As System.Nullable(Of Double), ByVal pdblValorFuturoCliente As System.Nullable(Of Double),
                                              ByVal pstrBrokenTrader As String, ByVal pstrEntidad As String, ByVal pstrEstrategia As String, _
                                              ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String,
                                              ByVal pdtmFechaConstancia As System.Nullable(Of DateTime), pdblConstancia As System.Nullable(Of Double), ByVal pstrIDComitenteADR As String,
                                              ByVal pstrReceptorToma As String, ByVal pdtmFechaSalida As Nullable(Of DateTime), ByVal pstrTipoGarantia As String, ByVal pstrInfoConexion As String) As Task(Of List(Of OrdenOYDPLUS_Calculos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of OrdenOYDPLUS_Calculos)) = New TaskCompletionSource(Of List(Of OrdenOYDPLUS_Calculos))()
        objTaskComplete.TrySetResult(OYDPLUS_CalcuarValorOrden(pstrTipoCalculo, pstrReceptor, pstrTipoOrden, pstrTipoNegocio, pstrTipoProducto, pstrTipoOperacion, pstrClase,
                                                                  pdtmOrden, pstrClasificacion, pstrTipoLimite, pstrDuracion, pdtmFechaVigencia, pstrHoraVigencia, pintDiasVigencia, pstrCondicionesNegociacion, pstrFormaPago, pstrTipoInversion,
                                                                  pstrEjecucion, pstrMercado, pstrIDComitente, pstrIDOrdenante, pstrUBICACIONTITULO, pintCuentaDeposito, pstrUsuarioOperador,
                                                                  pstrCanalRecepcion, pstrMedioVerificable, pdtmFechaHoraRecepcion, pstrNroExtensionToma, pstrEspecie, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, plogEstandarizada, pdtmFechaCumplimiento,
                                                                  pdblTasaFacial, pstrModalidad, pstrIndicador, pdblPuntosIndicador, plogEnPesos, pdblCantidad, pdblPrecio,
                                                                  pdblPrecioMaximoMinimo, pdblValorCaptacionGiro, pdblValorFuturoRepo, pdblTasaRegistro, pdblTasaCliente, pdblTasaNominal, pdblCastigo, pdblValorAccion, pdblComision, pdblValorComision, pdblValorOrden, pintDiasRepo,
                                                                  pintProductoValores, pdblCostosAdicionales,
                                                                  pintDiasCumplimiento, pdblIvaComision, pdblValorFuturoCliente,
                                                                  pstrBrokenTrader, pstrEntidad, pstrEstrategia, pstrMaquina, pstrUsuario, pstrUsuarioWindows,
                                                                  pdtmFechaConstancia, pdblConstancia, pstrIDComitenteADR, pstrReceptorToma, pdtmFechaSalida, pstrTipoGarantia, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function OYDPLUS_ConsultarPorcentajeGarantia(ByVal pstrTipoNegocio As String, ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim dblPorcentajeGarantia As Double? = 0

            Dim ret = Me.DataContext.uspOyDNet_ConsultarPorcentajeGarantia(pstrTipoNegocio, pstrEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarPorcentajeGarantia"), 0, dblPorcentajeGarantia)

            If Not IsNothing(dblPorcentajeGarantia) Then
                Return CDbl(dblPorcentajeGarantia)
            Else
                Return 0
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarPorcentajeGarantia")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarPorcentajeGarantiaSync(ByVal pstrTipoNegocio As String, ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Dim objTask As Task(Of Double) = Me.OYDPLUS_ConsultarPorcentajeGarantiaAsync(pstrTipoNegocio, pstrEspecie, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function OYDPLUS_ConsultarPorcentajeGarantiaAsync(ByVal pstrTipoNegocio As String, ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Double)
        Dim objTaskComplete As TaskCompletionSource(Of Double) = New TaskCompletionSource(Of Double)()
        objTaskComplete.TrySetResult(OYDPLUS_ConsultarPorcentajeGarantia(pstrTipoNegocio, pstrEspecie, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Private Function OYDPLUS_ConsultarSaldoOrden(ByVal pintIDOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblSaldoOrdenSinCalzar)
        Try
            Dim ret = Me.DataContext.uspOyDNet_ConsultarSaldoOrdenSAE(pintIDOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarSaldoOrden"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarPorcentajeGarantia")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarSaldoOrdenSync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblSaldoOrdenSinCalzar)
        Dim objTask As Task(Of List(Of tblSaldoOrdenSinCalzar)) = Me.OYDPLUS_ConsultarSaldoOrdenASync(plngID, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function OYDPLUS_ConsultarSaldoOrdenASync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of tblSaldoOrdenSinCalzar))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblSaldoOrdenSinCalzar)) = New TaskCompletionSource(Of List(Of tblSaldoOrdenSinCalzar))
        objTaskComplete.TrySetResult(OYDPLUS_ConsultarSaldoOrden(plngID, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Private Function OYDPLUS_EliminarAsociacion(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            Dim plogElimino As Boolean? = False

            Dim ret = Me.DataContext.uspOyDNet_EliminarAsociacionOrdenLiqProbables(plngID, plogElimino, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_EliminarAsociacion"), 0)

            If Not IsNothing(plogElimino) Then
                Return CBool(plogElimino)
            Else
                Return False
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_EliminarAsociacion")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_EliminarAsociacionOrdenLiqProbablesSync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Dim objTask As Task(Of Boolean) = Me.OYDPLUS_EliminarAsociacionOrdenLiqProbablesAsync(plngID, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function OYDPLUS_EliminarAsociacionOrdenLiqProbablesAsync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)
        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(OYDPLUS_EliminarAsociacion(plngID, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function OYDPLUS_ConsultarEnrutamientoOrdenes(ByVal pintIDOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EnrutamientoOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarEnrutamientoOrdenSAE(pintIDOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarEnrutamientoOrdenes"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarEnrutamientoOrdenes")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarEnrutamientoOrdenesSync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EnrutamientoOrden)
        Dim objTask As Task(Of List(Of EnrutamientoOrden)) = Me.OYDPLUS_ConsultarEnrutamientoOrdenesASync(plngID, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function OYDPLUS_ConsultarEnrutamientoOrdenesASync(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EnrutamientoOrden))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EnrutamientoOrden)) = New TaskCompletionSource(Of List(Of EnrutamientoOrden))
        objTaskComplete.TrySetResult(OYDPLUS_ConsultarEnrutamientoOrdenes(plngID, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Instrucciones Orden"

    Public Sub InsertInstruccionesOrdene(ByVal currentInstruccion As InstruccionesOrdene)

    End Sub

    Public Sub UpdateInstruccionesOrdene(ByVal currentInstruccion As InstruccionesOrdene)

    End Sub

    Public Sub DeleteInstruccionesOrdene(ByVal currentInstruccion As InstruccionesOrdene)

    End Sub

    Public Function OYDPLUS_Consultar_InstruccionesOrdenes(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pintNroOrden As Integer, ByVal plngVersion As Integer, _
                                                           ByVal pstrTopico As String, ByVal pstrEstadoMakerChecker As String, ByVal pstrUsuario As String) As List(Of InstruccionesOrdene)
        Try
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_InstruccionesOrdenes_Consultar(pstrTipo, pstrClase, pintNroOrden, plngVersion, pstrEstadoMakerChecker, pstrUsuario, DemeInfoSesion(pstrUsuario, "Consultar_InstruccionesOrdenes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_InstruccionesOrdenes")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_Traer_InstruccionesClientes(ByVal plngIdComitente As String, ByVal pstrTopico As String, ByVal pstrUsuario As String) As List(Of InstruccionesOrdene)
        Try
            Dim ret = Me.DataContext.uspOyDNet_InstruccionesClientes_Consultar(plngIdComitente, pstrTopico, pstrUsuario).ToList
            If ret.Count = 0 Then
                Dim objInstrucion As List(Of InstruccionesOrdene)
                objInstrucion = OYDPLUS_Traer_InstruccionesOrdenes_PorDefecto(pstrTopico, pstrUsuario).ToList
                Return objInstrucion
            Else
                Return ret
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_InstruccionesClientes")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_Traer_InstruccionesOrdenes_PorDefecto(ByVal pstrTopico As String, ByVal pstrUsuario As String) As List(Of InstruccionesOrdene)
        Try
            Dim ret1 = Me.DataContext.uspOrdenes_ConsultarInstruccionesAcciones_OyDNet(pstrTopico, pstrUsuario).ToList
            Return ret1
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_InstruccionesClientes")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_Consultar_CuentasClientes(ByVal plngIdComitente As String, ByVal pstrUsuario As String) As List(Of CuentasClientes)
        Try
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_CuentasClientes_Consultar(plngIdComitente, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_CuentasClientes")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Carga masiva ordenes"

    <Query(HasSideEffects:=True)> _
    Public Function OYDPLUS_CargaMasivaValidarOrdenManual(ByVal pstrReceptor As String, ByVal pstrTipoOrden As String, ByVal pstrTipoNegocio As String, ByVal pstrTipoOperacion As String, ByVal pstrClasificacion As String, ByVal pstrTipoLimite As String,
                                                          ByVal pstrDuracion As String, ByVal pdtmFechaVigencia As System.Nullable(Of DateTime), ByVal pstrHoraVigencia As String, ByVal pintDiasVigencia As System.Nullable(Of Integer), ByVal pstrCondicionesNegociacion As String, ByVal pstrFormaPago As String,
                                                          ByVal pstrTipoInversion As String, ByVal pstrEjecucion As String, ByVal pstrMercado As String, ByVal pstrIDComitente As String,
                                                          ByVal pstrUBICACIONTITULO As String, ByVal pintCuentaDeposito As System.Nullable(Of Integer), ByVal pstrUsuarioOperador As String, ByVal pstrCanalRecepcion As String, ByVal pstrMedioVerificable As String,
                                                          ByVal pdtmFechaHoraRecepcion As System.Nullable(Of DateTime), ByVal pstrNroExtensionToma As String, ByVal pstrEspecie As String, ByVal pstrISIN As String,
                                                          ByVal pdtmFechaEmision As System.Nullable(Of DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of DateTime), ByVal pdtmFechaCumplimiento As System.Nullable(Of DateTime), ByVal pdblTasaFacial As System.Nullable(Of Double),
                                                          ByVal pstrModalidad As String, ByVal pstrIndicador As String, ByVal pdblPuntosIndicador As System.Nullable(Of Double), ByVal plogEnPesos As System.Nullable(Of Boolean), ByVal pdblCantidad As System.Nullable(Of Double), ByVal pdblPrecio As System.Nullable(Of Double),
                                                          ByVal pdblPrecioMaximoMinimo As System.Nullable(Of Double), ByVal pdblValorCaptacionGiro As System.Nullable(Of Double), ByVal pdblTasaRegistro As System.Nullable(Of Double), ByVal pdblTasaCliente As System.Nullable(Of Double),
                                                          ByVal pdblCastigo As System.Nullable(Of Double), ByVal pdblValorAccion As System.Nullable(Of Double), ByVal pdblComision As System.Nullable(Of Double), ByVal pdblValorComision As System.Nullable(Of Double),
                                                          ByVal pintProductoValores As System.Nullable(Of Integer), ByVal pdblCostosAdicionales As System.Nullable(Of Double), ByVal pstrInstrucciones As String, ByVal pstrNotas As String,
                                                          ByVal pstrBrokenTrader As String, ByVal pstrEntidad As String, ByVal pstrEstrategia As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String,
                                                          ByVal pstrIDComitenteADR As String, plogComplementacionPrecioPromedio As Boolean, pstrLiquidaciones As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaOrdenes_ValidarDatosOrden(pstrReceptor, pstrTipoOrden, pstrTipoNegocio, pstrTipoOperacion,
                                                                                    pstrClasificacion, pstrTipoLimite, pstrDuracion, pdtmFechaVigencia, pstrHoraVigencia, pintDiasVigencia, pstrCondicionesNegociacion, pstrFormaPago, pstrTipoInversion,
                                                                                    pstrEjecucion, pstrMercado, pstrIDComitente, pstrUBICACIONTITULO, pintCuentaDeposito, pstrUsuarioOperador,
                                                                                    pstrCanalRecepcion, pstrMedioVerificable, pdtmFechaHoraRecepcion, pstrNroExtensionToma, pstrBrokenTrader, pstrEntidad, pstrEstrategia,
                                                                                    pstrEspecie, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pdblTasaFacial, pstrModalidad, pstrIndicador, pdblPuntosIndicador, plogEnPesos, pdblCantidad, pdblPrecio,
                                                                                    pdblPrecioMaximoMinimo, pdblValorCaptacionGiro, pdblTasaRegistro, pdblTasaCliente, pdblCastigo, pdblValorAccion, pdblComision, pdblValorComision, pdtmFechaCumplimiento,
                                                                                    pstrInstrucciones, pstrNotas, pintProductoValores, pdblCostosAdicionales, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "OYDPLUS_CargaMasivaValidarOrdenManual"), 0,
                                                                                    pstrIDComitenteADR, plogComplementacionPrecioPromedio, pstrLiquidaciones)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_CargaMasivaValidarOrdenManual")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_CargaMasivaValidarHabilitarCampos(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of CamposEditablesOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaOrdenes_ValidarHabilitarCampos(pstrUsuario, pstrMaquina,
                                                                                      DemeInfoSesion(pstrUsuario, "OYDPLUS_CargaMasivaValidarHabilitarCampos"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_CargaMasivaValidarHabilitarCampos")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_CargaMasivaConsultarConsultarResultados(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaOrdenes_ConsultarResultados(pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "OYDPLUS_CargaMasivaConsultarConsultarResultados"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_CargaMasivaConsultarConsultarResultados")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_CargaMasivarConfirmar(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String,
                                                          ByVal pstrConfirmaciones As String, ByVal pstrConfirmacionesUsuario As String, ByVal pstrJustificaciones As String, ByVal pstrJustificacionesUsuario As String, ByVal pstrAprobaciones As String, ByVal pstrAprobacionesUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaOrdenes_Confirmar(pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrConfirmaciones, pstrConfirmacionesUsuario, pstrJustificaciones, pstrJustificacionesUsuario, pstrAprobaciones, pstrAprobacionesUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_CargaMasivaConsultarConfirmar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_CargaMasivaConsultarConfirmar")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_CargaMasivaConsultarCantidadProcesadas(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of CargaMasivaCantidadProcesadas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaOrdenes_ConsultarCantidadProcesados(pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "OYDPLUS_CargaMasivaConsultarCantidadProcesadas"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_CargaMasivaConsultarCantidadProcesadas")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Portafolio y rentabilidad Posición Propia"

    ''' <summary>
    ''' Consultar el portafolio de posición propia del dioa actual 
    ''' </summary>
    ''' <param name="pstrIDReceptor"></param>
    ''' <param name="pstrTipoProducto"></param>
    ''' <param name="plogTodoslosClientes"></param>
    ''' <param name="pstrIDCliente"></param>
    ''' <param name="pdtmFecha"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>Santiago Vergara - Febrero 21/2014</remarks>
    Public Function PortafolioPosicionPropiaDiaActual(ByVal pstrIDReceptor As String, ByVal pstrTipoProducto As String, ByVal plogTodoslosClientes As Boolean _
                                                      , ByVal pstrIDCliente As String, ByVal pdtmFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosPortafolioPPropia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_PortafolioPosicionPropiaConsultar(pstrIDReceptor, pstrTipoProducto, plogTodoslosClientes, pstrIDCliente, True, pdtmFecha _
                                                                       , pstrUsuario, DemeInfoSesion(pstrUsuario, "PortafolioPosicionPropiaDiaActual"),
                                                                       ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PortafolioPosicionPropiaDiaActual")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta el portafolio d eposición propia para dias siguientes
    ''' </summary>
    ''' <param name="pstrIDReceptor"></param>
    ''' <param name="pstrTipoProducto"></param>
    ''' <param name="plogTodoslosClientes"></param>
    ''' <param name="pstrIDCliente"></param>
    ''' <param name="pdtmFecha"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una lista de tipo DatosPortafolioPPropia</returns>
    ''' <remarks>Santiago Vergara - Febrero 21/2014</remarks>
    Public Function PortafolioPosicionPropiaOtrosDias(ByVal pstrIDReceptor As String, ByVal pstrTipoProducto As String, ByVal plogTodoslosClientes As Boolean _
                                                      , ByVal pstrIDCliente As String, ByVal pdtmFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosPortafolioPPropia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_PortafolioPosicionPropiaConsultar(pstrIDReceptor, pstrTipoProducto, plogTodoslosClientes, pstrIDCliente, False, pdtmFecha _
                                                                       , pstrUsuario, DemeInfoSesion(pstrUsuario, "PortafolioPosicionPropiaOtrosDias"),
                                                                       ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PortafolioPosicionPropiaOtrosDias")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta las utilidades por especie y las utilidades totales para el portafolio de posición propia
    ''' </summary>
    ''' <param name="pstrIDReceptor"></param>
    ''' <param name="pintIdEmisor"></param>
    ''' <param name="pintIdMesa"></param>
    ''' <param name="pstrEspecie"></param>
    ''' <param name="plogIncluirFechas"></param>
    ''' <param name="pdtmFechaInicial"></param>
    ''' <param name="pdtmFechaFinal"></param>
    ''' <param name="pstrTipoProducto"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una lista de tipo DatosPortafolioPPropia</returns>
    ''' <remarks>Santiago Vergara - Febrero 21/2014</remarks>
    Public Function PortafolioPosicionPropiaRentabilidad(ByVal pstrIDReceptor As String, ByVal pintIdEmisor As Integer, ByVal pintIdMesa As Integer _
                                                         , ByVal pstrEspecie As String, ByVal plogIncluirFechas As Boolean, ByVal pdtmFechaInicial As DateTime _
                                                         , ByVal pdtmFechaFinal As DateTime, ByVal pstrTipoProducto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosPortafolioPPropia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_PortafolioRentabilidadConsultar(pstrIDReceptor, pintIdEmisor, pintIdMesa, pstrEspecie, plogIncluirFechas, pdtmFechaInicial, pdtmFechaFinal, pstrTipoProducto _
                                                                       , pstrUsuario, DemeInfoSesion(pstrUsuario, "PortafolioPosicionPropiaRentabilidad"),
                                                                       ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PortafolioPosicionPropiaRentabilidad")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta el portafolio de una operación en especifico
    ''' </summary>
    ''' <param name="pstrIDReceptor"></param>
    ''' <param name="pstrTipoProducto"></param>
    ''' <param name="plogTodoslosClientes"></param>
    ''' <param name="pstrIDCliente"></param>
    ''' <param name="pdtmFecha"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una lista de tipo OperacionesPPropia</returns>
    ''' <remarks>Santiago Vergara - Febrero 18/2105</remarks>
    Public Function PortafolioPosicionPropiaOperaciones(ByVal pstrIDReceptor As String, ByVal pstrTipoProducto As String, ByVal plogTodoslosClientes As Boolean _
                                                        , ByVal pstrIDCliente As String, ByVal pdtmFecha As DateTime, ByVal pstrUsuario As String,
                                                        ByVal plogOtrosDias As Boolean, ByVal pstrIDEspecieOperacion As String,
                                                        ByVal pdtmFechaEmision As Nullable(Of DateTime), ByVal pdtmFechaVencimiento As Nullable(Of DateTime), ByVal pstrModalidad As String,
                                                        ByVal pstrIndicador As String, ByVal pdblTasaOpunto As Nullable(Of Double), ByVal pstrInfoConexion As String) As List(Of OperacionesPPropia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_PortafolioPosicionPropiaConsultarOperaciones(pstrIDReceptor, pstrTipoProducto, plogTodoslosClientes, pstrIDCliente, plogOtrosDias, pdtmFecha _
                                                                       , pstrUsuario, DemeInfoSesion(pstrUsuario, "PortafolioPosicionPropiaOperaciones"),
                                                                       ClsConstantes.GINT_ErrorPersonalizado, True, pstrIDEspecieOperacion,
                                                                       pdtmFechaEmision, pdtmFechaVencimiento, pstrModalidad, pstrIndicador, pdblTasaOpunto).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PortafolioPosicionPropiaOperaciones")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta las utilidades por especie y las utilidades totales para el portafolio de posición propia
    ''' </summary>
    ''' <param name="pstrIDReceptor"></param>
    ''' <param name="pintIdEmisor"></param>
    ''' <param name="pintIdMesa"></param>
    ''' <param name="pstrEspecie"></param>
    ''' <param name="plogIncluirFechas"></param>
    ''' <param name="pdtmFechaInicial"></param>
    ''' <param name="pdtmFechaFinal"></param>
    ''' <param name="pstrTipoProducto"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una lista de tipo OperacionesPPropia</returns>
    ''' <remarks>Santiago Vergara - Febrero 18/2015</remarks>
    Public Function PortafolioPosicionPropiaRentabilidadOperaciones(ByVal pstrIDReceptor As String, ByVal pintIdEmisor As Integer, ByVal pintIdMesa As Integer _
                                                         , ByVal pstrEspecie As String, ByVal plogIncluirFechas As Boolean, ByVal pdtmFechaInicial As DateTime _
                                                         , ByVal pdtmFechaFinal As DateTime, ByVal pstrTipoProducto As String, ByVal pstrUsuario As String _
                                                         , ByVal pdtmEmision As Nullable(Of DateTime), ByVal pdtmVencimiento As Nullable(Of DateTime), ByVal pstrModalidad As String _
                                                         , ByVal pstrIndicador As String, ByVal pdblTasaOpuntos As Nullable(Of Double), ByVal pstrInfoConexion As String) As List(Of OperacionesPPropia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_PortafolioRentabilidadConsultarOperaciones(pstrIDReceptor, pintIdEmisor, pintIdMesa, pstrEspecie, plogIncluirFechas, pdtmFechaInicial, pdtmFechaFinal, pstrTipoProducto _
                                                                       , pstrUsuario, DemeInfoSesion(pstrUsuario, "PortafolioPosicionPropiaRentabilidadOperaciones"),
                                                                       ClsConstantes.GINT_ErrorPersonalizado, True, pdtmEmision, pdtmVencimiento, pstrModalidad, pstrIndicador, pdblTasaOpuntos).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PortafolioPosicionPropiaRentabilidadOperaciones")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Registro operaciones entre receptores"

    ''' <summary>
    ''' Consulta los datos de registro de operaciones entre receptores
    ''' </summary>
    ''' <param name="pintIdOperacion"></param>
    ''' <param name="pstrClase"></param>
    ''' <param name="pstrTipoOperacion"></param>
    ''' <param name="pstrReceptorA"></param>
    ''' <param name="pstrReceptorB"></param>
    ''' <param name="pstrIdEspecie"></param>
    ''' <param name="plogTrasladoEnDinero"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una lista de tipo RegistroOperacionesPorReceptor</returns>
    ''' <remarks>Santiago Vergara - Marzo 27/2014</remarks>
    Public Function RegistroOperacionesPorReceptorConsultar(ByVal pintIdOperacion As Integer, ByVal pstrClase As String, ByVal pstrTipoOperacion As String _
                                               , ByVal pstrReceptorA As String, ByVal pstrReceptorB As String, ByVal pstrIdEspecie As String _
                                               , ByVal plogTrasladoEnDinero As Boolean, ByVal pstrUsuario As String,
                                               ByVal pdtmFechaLiquidacion As Nullable(Of DateTime), ByVal pdtmFechaCumplimiento As Nullable(Of DateTime), ByVal pstrInfoConexion As String) As List(Of RegistroOperacionesPorReceptor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RegistroOperacionesPorReceptor_Consultar(pintIdOperacion, pstrClase, pstrTipoOperacion, pstrReceptorA, pstrReceptorB _
                                                                                        , pstrIdEspecie, plogTrasladoEnDinero, pstrUsuario, DemeInfoSesion(pstrUsuario, "RegistroOperacionesPorReceptorConsultar"), ClsConstantes.GINT_ErrorPersonalizado,
                                                                                        pdtmFechaLiquidacion, pdtmFechaCumplimiento).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RegistroOperacionesPorReceptorConsultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Filtro de los datos de registro operaciones entre receptores
    ''' </summary>
    ''' <param name="pstrFiltro"></param>
    ''' <param name="plogTrasladoEnDinero"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una lista de tipo RegistroOperacionesPorReceptor</returns>
    ''' <remarks>Santiago Vergara - Marzo 27/2014</remarks>
    Public Function RegistroOperacionesPorReceptorFiltrar(ByVal pstrFiltro As String, ByVal plogTrasladoEnDinero As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RegistroOperacionesPorReceptor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RegistroOperacionesPorReceptor_Filtrar(pstrFiltro, plogTrasladoEnDinero, pstrUsuario, DemeInfoSesion(pstrUsuario, "RegistroOperacionesPorReceptorFiltrar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RegistroOperacionesPorReceptorFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerRegistroOperacionesPorReceptorPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As RegistroOperacionesPorReceptor
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New RegistroOperacionesPorReceptor
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return (e)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerRegistroOperacionesPorReceptorPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Insert de los datos de registro operaciones entre receptores
    ''' </summary>
    ''' <param name="RegistroOperacionesPorReceptor"></param>
    ''' <remarks>Giovanny Velez  - Marzo 27/2014</remarks>
    Public Sub InsertRegistroOperacionesPorReceptor(ByVal RegistroOperacionesPorReceptor As RegistroOperacionesPorReceptor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,RegistroOperacionesPorReceptor.pstrUsuarioConexion, RegistroOperacionesPorReceptor.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            RegistroOperacionesPorReceptor.InfoSesion = DemeInfoSesion(RegistroOperacionesPorReceptor.pstrUsuarioConexion, "InsertRegistroOperacionesPorReceptor")
            Me.DataContext.RegistroOperacionesPorReceptor.InsertOnSubmit(RegistroOperacionesPorReceptor)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertRegistroOperacionesPorReceptor")
        End Try
    End Sub

    ''' <summary>
    ''' Update de los datos de registro operaciones entre receptores
    ''' </summary>
    ''' <param name="currentRegistroOperacionesPorReceptor"></param>
    ''' <remarks>Giovanny Velez  - Marzo 27/2014</remarks>
    Public Sub UpdateRegistroOperacionesPorReceptor(ByVal currentRegistroOperacionesPorReceptor As RegistroOperacionesPorReceptor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentRegistroOperacionesPorReceptor.pstrUsuarioConexion, currentRegistroOperacionesPorReceptor.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentRegistroOperacionesPorReceptor.InfoSesion = DemeInfoSesion(currentRegistroOperacionesPorReceptor.pstrUsuarioConexion, "UpdateRegistroOperacionesPorReceptor")
            Me.DataContext.RegistroOperacionesPorReceptor.Attach(currentRegistroOperacionesPorReceptor, Me.ChangeSet.GetOriginal(currentRegistroOperacionesPorReceptor))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateRegistroOperacionesPorReceptor")
        End Try
    End Sub

    ''' <summary>
    ''' Delete logico de los datos de registro operaciones entre receptores
    ''' </summary>
    ''' <param name="RegistroOperacionesPorReceptor"></param>
    ''' <remarks>Giovanny Velez  - Marzo 27/2014</remarks>
    Public Sub DeleteRegistroOperacionesPorReceptor(ByVal RegistroOperacionesPorReceptor As RegistroOperacionesPorReceptor)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,RegistroOperacionesPorReceptor.pstrUsuarioConexion, RegistroOperacionesPorReceptor.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            RegistroOperacionesPorReceptor.InfoSesion = DemeInfoSesion(RegistroOperacionesPorReceptor.pstrUsuarioConexion, "DeleteRegistroOperacionesPorReceptor")
            Me.DataContext.RegistroOperacionesPorReceptor.Attach(RegistroOperacionesPorReceptor)
            Me.DataContext.RegistroOperacionesPorReceptor.DeleteOnSubmit(RegistroOperacionesPorReceptor)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteRegistroOperacionesPorReceptor")
        End Try

    End Sub

    Public Function RegistroOperacionesPorReceptorEliminar(ByVal pintIdOperacion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RegistroOperacionesPorReceptor_Eliminar(pintIdOperacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "RegistroOperacionesPorReceptorEliminar"), ClsConstantes.GINT_ErrorPersonalizado)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RegistroOperacionesPorReceptorEliminar")
            Return Nothing
        End Try
    End Function

    Public Function RegistroOperacionesPorReceptorActualizar(ByVal pintIdOperacion? As Integer, _
                                                                ByVal pstrReceptorA As String, _
                                                                ByVal pstrTipoProducto As String, _
                                                                ByVal pstrClase As String, _
                                                                ByVal pstrTipoOperacion As String, _
                                                                ByVal pstrReceptorB As String, _
                                                                ByVal plngIdCliente As String, _
                                                                ByVal pstrIdEspecie As String, _
                                                                ByVal pstrIsin As String, _
                                                                ByVal pdtmEmision As System.Nullable(Of Date), _
                                                                ByVal pdtmVencimiento As System.Nullable(Of Date), _
                                                                ByVal plngIdOrdenante As String, _
                                                                ByVal pintCuentaDeposito As System.Nullable(Of System.Int32), _
                                                                ByVal pstrUbicacionTitulo As String, _
                                                                ByVal pdblTasaFacial As System.Nullable(Of System.Double), _
                                                                ByVal pstrModalidad As String, _
                                                                ByVal pstrIndicador As String, _
                                                                ByVal pdblPuntosIndicador As System.Nullable(Of System.Double), _
                                                                ByVal pdblCantidad As System.Nullable(Of System.Double), _
                                                                ByVal pdblValorNominal As System.Nullable(Of System.Double), _
                                                                ByVal pdblValorGiro As System.Nullable(Of System.Double), _
                                                                ByVal pdblValorGiro2 As System.Nullable(Of System.Double), _
                                                                ByVal pdblValorGiroContraparte As System.Nullable(Of System.Double), _
                                                                ByVal pdblComisionVenta As System.Nullable(Of System.Double), _
                                                                ByVal pdblComisionCompra As System.Nullable(Of System.Double), _
                                                                ByVal pdblTasa As System.Nullable(Of System.Double), _
                                                                ByVal pdblAcumCupon As System.Nullable(Of System.Double), _
                                                                ByVal pdblPrecioSucio As System.Nullable(Of System.Double), _
                                                                ByVal pstrObservaciones As String, _
                                                                ByVal plogTrasladoEnDinero As Boolean, _
                                                                ByVal pstrUsuario As String, _
                                                                ByVal pdtmLiquidacion As System.Nullable(Of Date), _
                                                                ByVal pdtmCumplimiento As System.Nullable(Of Date),
                                                                ByVal pdtmFechaRegreso As System.Nullable(Of Date),
                                                                ByVal pintPlazo As System.Nullable(Of Integer), ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RegistroOperacionesPorReceptor_Actualizar(pintIdOperacion,
                                                                 pstrReceptorA,
                                                                 pstrTipoProducto,
                                                                 pstrClase,
                                                                 pstrTipoOperacion,
                                                                 pstrReceptorB,
                                                                 plngIdCliente,
                                                                 pstrIdEspecie,
                                                                 pstrIsin,
                                                                 pdtmEmision,
                                                                 pdtmVencimiento,
                                                                 plngIdOrdenante,
                                                                 pintCuentaDeposito,
                                                                 pstrUbicacionTitulo,
                                                                 pdblTasaFacial,
                                                                 pstrModalidad,
                                                                 pstrIndicador,
                                                                 pdblPuntosIndicador,
                                                                 pdblCantidad,
                                                                 pdblValorNominal,
                                                                 pdblValorGiro,
                                                                 pdblValorGiro2,
                                                                 pdblValorGiroContraparte,
                                                                 pdblComisionVenta,
                                                                 pdblComisionCompra,
                                                                 pdblTasa,
                                                                 pdblAcumCupon,
                                                                 pdblPrecioSucio,
                                                                 pstrObservaciones,
                                                                 plogTrasladoEnDinero,
                                                                 pstrUsuario,
                                                                 pdtmLiquidacion,
                                                                 pdtmCumplimiento,
                                                                 DemeInfoSesion(pstrUsuario, "RegistroOperacionesPorReceptorActualizar"),
                                                                 ClsConstantes.GINT_ErrorPersonalizado,
                                                                 pdtmFechaRegreso,
                                                                 pintPlazo)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RegistroOperacionesPorReceptorActualizar")
            Return Nothing
        End Try
    End Function

    Public Function RegistroOperacionesPorReceptor_Calcular(ByVal pstrTipoNegocio As String, ByVal pstrIDComitente As String, ByVal pdtmFechaLiquidacion As Nullable(Of DateTime), ByVal pdtmFechaCumplimiento As Nullable(Of DateTime), ByVal pstrEspecie As String, ByVal pstrISIN As String, ByVal pdtmFechaEmision As Nullable(Of DateTime), ByVal pdtmFechaVencimiento As Nullable(Of DateTime), ByVal pdblTasaFacial As Nullable(Of Double), ByVal pstrModalidad As String, ByVal pstrIndicador As String, ByVal pdblPuntosIndicador As Nullable(Of Double), ByVal pdblNominal As Nullable(Of Double), ByVal pdblPrecio As Nullable(Of Double), ByVal pdblValorGiro As Nullable(Of Double), ByVal pdblValorGiro2 As Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RegistroOperacionesPorReceptor_Calculos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RegistroOperacionesPorReceptor_Calcular(pstrTipoNegocio, pstrIDComitente, pdtmFechaLiquidacion, pdtmFechaCumplimiento, pstrEspecie, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pdblTasaFacial, pstrModalidad, pstrIndicador, pdblPuntosIndicador, pdblNominal, pdblPrecio, pdblValorGiro, pdblValorGiro2, pstrUsuario, DemeInfoSesion(pstrUsuario, "RegistroOperacionesPorReceptor_Calcular"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RegistroOperacionesPorReceptor_Calcular")
            Return Nothing
        End Try
    End Function

    Public Function RegistroOperacionesPorReceptor_CalcularSync(ByVal pstrTipoNegocio As String, ByVal pstrIDComitente As String, ByVal pdtmFechaLiquidacion As Nullable(Of DateTime), ByVal pdtmFechaCumplimiento As Nullable(Of DateTime), ByVal pstrEspecie As String, ByVal pstrISIN As String, ByVal pdtmFechaEmision As Nullable(Of DateTime), ByVal pdtmFechaVencimiento As Nullable(Of DateTime), ByVal pdblTasaFacial As Nullable(Of Double), ByVal pstrModalidad As String, ByVal pstrIndicador As String, ByVal pdblPuntosIndicador As Nullable(Of Double), ByVal pdblNominal As Nullable(Of Double), ByVal pdblPrecio As Nullable(Of Double), ByVal pdblValorGiro As Nullable(Of Double), ByVal pdblValorGiro2 As Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RegistroOperacionesPorReceptor_Calculos)
        Dim objTask As Task(Of List(Of RegistroOperacionesPorReceptor_Calculos)) = Me.RegistroOperacionesPorReceptor_CalcularAsync(pstrTipoNegocio, pstrIDComitente, pdtmFechaLiquidacion, pdtmFechaCumplimiento, pstrEspecie, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pdblTasaFacial, pstrModalidad, pstrIndicador, pdblPuntosIndicador, pdblNominal, pdblPrecio, pdblValorGiro, pdblValorGiro2, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function RegistroOperacionesPorReceptor_CalcularAsync(ByVal pstrTipoNegocio As String, ByVal pstrIDComitente As String, ByVal pdtmFechaLiquidacion As Nullable(Of DateTime), ByVal pdtmFechaCumplimiento As Nullable(Of DateTime), ByVal pstrEspecie As String, ByVal pstrISIN As String, ByVal pdtmFechaEmision As Nullable(Of DateTime), ByVal pdtmFechaVencimiento As Nullable(Of DateTime), ByVal pdblTasaFacial As Nullable(Of Double), ByVal pstrModalidad As String, ByVal pstrIndicador As String, ByVal pdblPuntosIndicador As Nullable(Of Double), ByVal pdblNominal As Nullable(Of Double), ByVal pdblPrecio As Nullable(Of Double), ByVal pdblValorGiro As Nullable(Of Double), ByVal pdblValorGiro2 As Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RegistroOperacionesPorReceptor_Calculos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RegistroOperacionesPorReceptor_Calculos)) = New TaskCompletionSource(Of List(Of RegistroOperacionesPorReceptor_Calculos))()
        objTaskComplete.TrySetResult(RegistroOperacionesPorReceptor_Calcular(pstrTipoNegocio, pstrIDComitente, pdtmFechaLiquidacion, pdtmFechaCumplimiento, pstrEspecie, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pdblTasaFacial, pstrModalidad, pstrIndicador, pdblPuntosIndicador, pdblNominal, pdblPrecio, pdblValorGiro, pdblValorGiro2, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Combinaciones Tipo Límite Órdenes Bolsa"
    Public Function CombinacionTipoLimiteConsultar(ByVal pstrUsuario As String) As List(Of CombinacionesTipoLimite)
        Try
            Dim ret = Me.DataContext.uspCombinacionesTipoLimite_Consultar(pstrUsuario, DemeInfoSesion(pstrUsuario, "CombinacionTipoLimiteConsultar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CombinacionTipoLimiteConsultar")
            Return Nothing
        End Try
    End Function
#End Region
End Class
