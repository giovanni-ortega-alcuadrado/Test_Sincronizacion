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
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesDivisas
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()> _
Partial Public Class OYDPLUSOrdenesDivisasDomainService
    Inherits LinqToSqlDomainService(Of OyDPLUSOrdenesDivisasDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "OYDPLUS"

    Public Sub InsertOrdenDivisas(ByVal Orden As OrdenDivisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Orden.pstrUsuarioConexion, Orden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenDivisas.InsertOnSubmit(Orden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrdenDivisas")
        End Try
    End Sub

    Public Sub UpdateOrdenDivisas(ByVal currentOrden As OrdenDivisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOrden.pstrUsuarioConexion, currentOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenDivisas.InsertOnSubmit(currentOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrdenDivisas")
        End Try
    End Sub

    Public Sub InsertOrdenDivisasDetalle(ByVal OrdenDivisasDetalle As OrdenDivisasDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,OrdenDivisasDetalle.pstrUsuarioConexion, OrdenDivisasDetalle.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenDivisasDetalle.InsertOnSubmit(OrdenDivisasDetalle)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrdenDivisasDetalle")
        End Try
    End Sub

    Public Sub UpdateOrdenDivisasDetalle(ByVal currentOrdenDivisasDetalle As OrdenDivisasDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOrdenDivisasDetalle.pstrUsuarioConexion, currentOrdenDivisasDetalle.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenDivisasDetalle.InsertOnSubmit(currentOrdenDivisasDetalle)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrdenDivisasDetalle")
        End Try
    End Sub

    Public Function OrdenDivisasPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As OrdenDivisas
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret As New OrdenDivisas
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenDivisasPorDefecto")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function OYDPLUSDivisas_ValidarIngresoOrden(ByVal pintID As Integer, ByVal pintNroOrden As Integer,
                                                ByVal pstrReceptor As String, ByVal pdtmFechaOrden As DateTime,
                                                ByVal pstrTipoOperacion As String, ByVal pstrEstadoOrden As String,
                                                ByVal pstrTipoNegocio As String, ByVal pstrTipoProducto As String,
                                                ByVal lngIDComitente As String, ByVal lngIDOrdenante As String,
                                                ByVal pintCumplimiento As Nullable(Of Integer), ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                ByVal strConceptoGiro As String, ByVal pstrMoneda As String, ByVal pdblMonto As Double,
                                                ByVal pdblTasaDeCesionMesa As Double, ByVal pdblTasaCliente As Double,
                                                ByVal pdblComisionComercialVIASpread As Double, ByVal pdblComisionComercialVIAPapeleta As Double,
                                                ByVal pstrOperador As String, ByVal pdblOtrosCostos As Double, ByVal pdblValorNeto As Double,
                                                ByVal pdblTasaBruta As Double, ByVal pdblCantidadBruta As Double, ByVal pdblCantidadNeta As Double,
                                                ByVal pdblTasaDolar As Double, ByVal pdblCantidadUSD As Double, ByVal pdblIvaOtrosCostos As Double,
                                                ByVal pdblIva As Double, ByVal pdblTCPIva As Double, ByVal pdblTRM As Double,
                                                ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String,
                                                ByVal pstrConfirmaciones As String, ByVal pstrConfirmacionesUsuario As String,
                                                ByVal pstrJustificaciones As String, ByVal pstrJustificacionesUsuario As String,
                                                ByVal pstrAprobaciones As String, ByVal pstrAprobacionesUsuario As String,
                                                ByVal plogGuardarComoPlantilla As Boolean, ByVal pstrNombrePlantilla As String,
                                                ByVal pstrModulo As String, ByVal pstrXMLDetalle As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrXMLDetalle = RetornarValorDescodificado(pstrXMLDetalle)
            Dim ret = Me.DataContext.uspOyDNet_Divisas_ValidarOrden(pintID, pstrReceptor,
                                                                    pintNroOrden, pstrTipoNegocio, pstrTipoProducto,
                                                                    pstrTipoOperacion, pdtmFechaOrden, pstrEstadoOrden,
                                                                    lngIDComitente, lngIDOrdenante,
                                                                    pintCumplimiento, pdtmFechaCumplimiento, strConceptoGiro,
                                                                    pstrMoneda, pdblMonto,
                                                                    pdblTasaDeCesionMesa, pdblTasaCliente,
                                                                    pdblComisionComercialVIASpread, pdblComisionComercialVIAPapeleta,
                                                                    pstrOperador, pdblOtrosCostos, pdblValorNeto, pdblTasaBruta, pdblCantidadBruta,
                                                                    pdblCantidadNeta, pdblTasaDolar, pdblCantidadUSD, pdblIvaOtrosCostos, pdblIva, pdblTCPIva, pdblTRM,
                                                                    pstrConfirmaciones, pstrConfirmacionesUsuario,
                                                                    pstrJustificaciones, pstrJustificacionesUsuario,
                                                                    pstrAprobaciones, pstrAprobacionesUsuario,
                                                                    pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "OYDPLUS_ValidarIngresoOrdenDivisas"), ClsConstantes.GINT_ErrorPersonalizado,
                                                                    plogGuardarComoPlantilla, pstrNombrePlantilla, pstrModulo, pstrXMLDetalle)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ValidarIngresoOrdenDivisas")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUSDivisas_FiltrarOrden(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenDivisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Divisas_FiltrarOrden(pstrEstado, pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUSDivisas_FiltrarOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUSDivisas_FiltrarOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUSDivisas_ConsultarOrden(ByVal pstrEstado As String, ByVal pintNroOrden As Integer, ByVal pstrTipoProducto As String, ByVal pstrIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenDivisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Divisas_ConsultarOrden(pstrEstado, pintNroOrden, pstrTipoProducto, pstrIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUSDivisas_ConsultarOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUSDivisas_AnularOrden(ByVal plngID As Integer, ByVal pstrNotas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Divisas_AnularOrden(plngID, pstrNotas, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUSDivisas_AnularOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUSDivisas_AnularOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_OrdenesPlantillasConsultar(ByVal pstrFiltro As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenDivisas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesPlantillas_Consultar(pstrFiltro, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_OrdenesPlantillasConsultar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_OrdenesPlantillasConsultar")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_OrdenesPlantillasEliminar(ByVal pstrIDEliminar As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OrdenesPlantillas_Eliminar(pstrIDEliminar, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_OrdenesPlantillasEliminar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
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
            Me.DataContext.uspOyDNet_OrdenesPlantillas_VerificarNombre(pstrNombrePlantilla, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_OrdenesPlantillasVerificarNombre"), ClsConstantes.GINT_ErrorPersonalizado, logRetorno)

            Return CBool(logRetorno)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_OrdenesPlantillasVerificarNombre")
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

    Public Function OYDPLUS_ConsultarFechaCumplimiento(ByVal pdtmFechaOrden As DateTime, ByVal pintDiasCumplimiento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblFechaCumplimiento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)("OYD", "MERCAMSOFT", "CUMPLIMIENTO", String.Empty, String.Format("<FechaOrden>{0:yyyy-MM-dd}</FechaOrden><Cumplimiento>{1}</Cumplimiento>", pdtmFechaOrden, pintDiasCumplimiento))
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarInformacionFechaCumplimiento("FECHACUMPLIMIENTODIVISAS", strRespuestaXML).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarCombosOtroSistema")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUSDivisas_ConsultarDetalleOrden(ByVal pintIDOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenDivisasDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Divisas_DetallePreorden_Consultar(pintIDOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUSDivisas_ConsultarDetalleOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUSDivisas_ConsultarDetalleOrden")
            Return Nothing
        End Try
    End Function

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
        Dim L2SDC As New OyDPLUSOrdenesDivisasDataContext
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

End Class