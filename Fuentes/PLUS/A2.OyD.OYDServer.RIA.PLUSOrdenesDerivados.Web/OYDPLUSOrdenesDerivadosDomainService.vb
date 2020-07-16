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
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesDerivados
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()> _
Partial Public Class OYDPLUSOrdenesDerivadosDomainService
    Inherits LinqToSqlDomainService(Of OyDPLUSOrdenesDerivadosDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "OYDPLUS"

    Public Sub InsertOrdenOYDPlus(ByVal Orden As OrdenDerivados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Orden.pstrUsuarioConexion, Orden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenDerivados.InsertOnSubmit(Orden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrdenOYDPlus")
        End Try
    End Sub

    Public Sub UpdateOrdenOYDPlus(ByVal currentOrden As OrdenDerivados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOrden.pstrUsuarioConexion, currentOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenDerivados.InsertOnSubmit(currentOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrdenOYDPlus")
        End Try
    End Sub

    Public Function OYDPLUS_OrdenPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As OrdenDerivados
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret As New OrdenDerivados
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_OrdenPorDefecto")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function OYDPLUS_ValidarIngresoOrden(ByVal pintID As Integer, ByVal pintIDOrden As Integer, ByVal pintIDPreOrden As Integer, ByVal pintNroOrden As Integer,
                                                ByVal pstrFechaGenerar As String, ByVal pdtmFechaPreOrden As Nullable(Of DateTime), ByVal pstrFechaOrden As String,
                                                ByVal pstrFechaVigencia As String, ByVal pstrReceptor As String, ByVal pstrNroDocumento As String, ByVal pstrCodigoOYD As String,
                                                ByVal pintIDCuenta As Nullable(Of Integer), ByVal pintIDSubCuenta As Nullable(Of Integer),
                                                ByVal pintTipoOperacion As Nullable(Of Integer), ByVal pintTipoInstruccion As Nullable(Of Integer),
                                                ByVal pintTipoRegularOSpread As Nullable(Of Integer), ByVal pintInstrumento As Nullable(Of Integer),
                                                ByVal pintVencimientoInicial As Nullable(Of Integer), ByVal pintVencimientoFinal As Nullable(Of Integer), ByVal pnumPrecio As Nullable(Of Double), ByVal pnumCantidad As Nullable(Of Double),
                                                ByVal pnumPrima As Nullable(Of Double), ByVal pintComision As Nullable(Of Integer), ByVal pnumValorComision As Nullable(Of Double),
                                                ByVal pintFacturarComision As Nullable(Of Integer), ByVal pintEstado As Nullable(Of Integer), ByVal pintSubEstado As Nullable(Of Integer),
                                                ByVal plogRegistroEnBolsa As Nullable(Of System.Boolean), ByVal pintTipoRegistro As Nullable(Of Integer),
                                                ByVal pintContraparte As Nullable(Of Integer), ByVal pintCanal As Nullable(Of Integer),
                                                ByVal pintMedioVerificable As Nullable(Of Integer), ByVal pdtmFechaHoraToma As Nullable(Of DateTime),
                                                ByVal pstrDetalleMedioVerificable As String, ByVal pintNaturaleza As Nullable(Of Integer),
                                                ByVal pintTipoEjecucion As Nullable(Of Integer), ByVal pintDuracion As Nullable(Of Integer),
                                                ByVal pnumPrecioStop As Nullable(Of Double), ByVal pnumCantidadVisible As Nullable(Of Double),
                                                ByVal pintOtroInstrumento As Nullable(Of Integer), ByVal pintTipoPrecio As Nullable(Of Integer),
                                                ByVal pstrInstrucciones As String, ByVal pintFinalidad As Nullable(Of Integer), ByVal pintTipoCobertura As Nullable(Of Integer),
                                                ByVal pintUbicacionPosicion As Nullable(Of Integer), ByVal pstrDescripcionPosicion As String, ByVal pnumMontoCubrir As Nullable(Of Double),
                                                ByVal pintMoneda As Nullable(Of Integer), ByVal plogGiveOut As Nullable(Of Boolean),
                                                ByVal pintFirmaGiveOut As Nullable(Of Integer), ByVal pstrReferenciaGiveOut As String,
                                                ByVal pstrComentarios As String, ByVal pintDiasAvisoCumplimiento As Nullable(Of Integer),
                                                ByVal pnumPrecioSpot As Nullable(Of Double), ByVal pnumCantidadMinima As Nullable(Of Double), ByVal pstrDetalleReceptoresXML As String,
                                                ByVal pstrConfirmaciones As String, ByVal pstrConfirmacionesUsuario As String,
                                                ByVal pstrJustificaciones As String, ByVal pstrJustificacionUsuario As String, ByVal pstrAprobaciones As String,
                                                ByVal pstrAprobacionesUsuario As String, ByVal pstrMaquina As String, ByVal pstrUsuario As String,
                                                ByVal pstrUsuarioWindows As String, ByVal plogGuardarComoPlantilla As Nullable(Of Boolean),
                                                ByVal pstrNombrePlantilla As String, ByVal pstrModulo As String, ByVal pstrTipoNegocio As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim DetalleReceptoresXML = HttpUtility.HtmlDecode(pstrDetalleReceptoresXML)

            Dim ret = Me.DataContext.uspOyDNet_Derivados_ValidarOrden(pintID, pintIDOrden, pintIDPreOrden, pintNroOrden, pstrFechaGenerar, pdtmFechaPreOrden, pstrFechaOrden, pstrFechaVigencia,
                                                                      pstrReceptor, pstrNroDocumento, pstrCodigoOYD, pintIDCuenta, pintIDSubCuenta,
                                                                      pintTipoOperacion, pintTipoInstruccion, pintTipoRegularOSpread, pintInstrumento,
                                                                      pintVencimientoInicial, pintVencimientoFinal, pnumPrecio, pnumCantidad, pnumPrima, pintComision, pnumValorComision,
                                                                      pintFacturarComision, pintEstado, pintSubEstado, plogRegistroEnBolsa, pintTipoRegistro,
                                                                      pintContraparte, pintCanal, pintMedioVerificable, pdtmFechaHoraToma,
                                                                      pstrDetalleMedioVerificable, pintNaturaleza, pintTipoEjecucion, pintDuracion,
                                                                      pnumPrecioStop, pnumCantidadVisible, pintOtroInstrumento, pintTipoPrecio,
                                                                      pstrInstrucciones, pintFinalidad, pintTipoCobertura, pintUbicacionPosicion, pstrDescripcionPosicion,
                                                                      pnumMontoCubrir, pintMoneda, plogGiveOut, pintFirmaGiveOut, pstrReferenciaGiveOut,
                                                                      pstrComentarios, pintDiasAvisoCumplimiento, pnumPrecioSpot, pnumCantidadMinima, pstrDetalleReceptoresXML,
                                                                      pstrConfirmaciones, pstrConfirmacionesUsuario,
                                                                      pstrJustificaciones, pstrJustificacionUsuario, pstrAprobaciones,
                                                                      pstrAprobacionesUsuario, pstrMaquina, pstrUsuario, pstrUsuarioWindows,
                                                                      DemeInfoSesion(pstrUsuario, "OYDPLUS_ValidarIngresoOrden"), 0, plogGuardarComoPlantilla, pstrNombrePlantilla,
                                                                      pstrModulo, pstrTipoNegocio)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ValidarIngresoOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ValidarPermisosUsuario(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)("OYD", "DERIVADOS", "VALIDARPERMISOSUSUARIO", String.Empty, String.Format("<Usuario>{0}</Usuario>", pstrUsuario))
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarListaValidacion("LISTAVALIDACION", strRespuestaXML, String.Empty, String.Empty, String.Empty).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ValidarPermisosUsuario")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ValidarCamposEditablesOrden(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CamposEditablesOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)("OYD", "DERIVADOS", "CAMPOSEDITABLES", String.Empty, String.Format("<Usuario>{0}</Usuario>", pstrUsuario))
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarCamposEditables("CAMPOSEDITABLESORDEN", strRespuestaXML, String.Empty, String.Empty, String.Empty).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ValidarCamposEditablesOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ValidarEdicionOrden(ByVal pintIDOrden As Integer, ByVal pintEstado As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)("OYD", "DERIVADOS", "VALIDAREDICIONORDEN", String.Empty, String.Format("<IDOrden>{0}</IDOrden><Estado>{1}</Estado><Usuario>{2}</Usuario>", pintIDOrden, pintEstado, pstrUsuario))
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarListaValidacion("LISTAVALIDACION", strRespuestaXML, String.Empty, String.Empty, String.Empty).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ValidarEdicionOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarReceptoresUsuarioDerivados(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresBusqueda)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)("OYD", "DERIVADOS", "RECEPTORESUSUARIO", String.Empty, String.Format("<Usuario>{0}</Usuario>", pstrUsuario))
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarInformacionBusquedaReceptores("BUSQUEDARECEPTORES", strRespuestaXML, String.Empty, String.Empty, String.Empty).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_BuscarReceptoresDerivados")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_FiltrarOrden(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenDerivados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)("OYD", "DERIVADOS", "FILTRAR", String.Empty, String.Format("<Usuario>{0}</Usuario>", pstrUsuario))
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarInformacionOrdenesDerivados("ORDENESDERIVADOS", strRespuestaXML, "FILTRAR", String.Empty, String.Empty).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_FiltrarOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_FiltrarOrdenPendiente(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenDerivados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Derivados_FiltrarOrden(pstrEstado, pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_FiltrarOrden"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_FiltrarOrdenPendiente")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarOrden(ByVal pintNroOrden As Nullable(Of Integer), ByVal pintEstado As Nullable(Of Integer), ByVal pintTipoOperacion As Nullable(Of Integer), ByVal pdtmFechaInicial As Nullable(Of DateTime), ByVal pdtmFechaFinal As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenDerivados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)("OYD", "DERIVADOS", "CONSULTAR", String.Empty, String.Format("<Usuario>{0}</Usuario><NroOrden>{1}</NroOrden><Estado>{2}</Estado><TipoOperacion>{3}</TipoOperacion><FechaInicial>{4:yyyy-MM-dd 00:00:00}</FechaInicial><FechaFinal>{5:yyyy-MM-dd 23:59:59}</FechaFinal>", pstrUsuario, pintNroOrden, pintEstado, pintTipoOperacion, pdtmFechaInicial, pdtmFechaFinal))
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarInformacionOrdenesDerivados("ORDENESDERIVADOS", strRespuestaXML, "CONSULTAR", String.Empty, String.Empty).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUSDerivados_AnularOrden(ByVal plngID As Integer, ByVal pintNroOrden As Integer, ByVal pstrNotas As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Derivados_AnularOrden(plngID, pintNroOrden, pstrNotas, pstrUsuario, pstrUsuarioWindows, DemeInfoSesion(pstrUsuario, "OYDPLUSDerivados_AnularOrden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUSDerivados_AnularOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_OrdenesPlantillasConsultar(ByVal pstrFiltro As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenDerivados)
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

    Public Function OYDPLUS_ConsultarReceptoresOrden(ByVal pintIDPreOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)("OYD", "DERIVADOS", "CONSULTARRECEPTORES", String.Empty, String.Format("<IDPreOrden>{0}</IDPreOrden>", pintIDPreOrden))
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarInformacionReceptoresDerivados("RECEPTORESDERIVADOS", strRespuestaXML, String.Empty, String.Empty, String.Empty).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarReceptoresOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarReceptoresOrdenPendiente(ByVal pintIDOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Derivados_ConsultarReceptores(pintIDOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarReceptoresOrdenPendiente"), 0).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarReceptoresOrdenPendiente")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_BuscarReceptoresDerivados(ByVal pstrFiltro As String, ByVal pstrOpcionCargar As String, ByVal pstrSistemaOrigen As String, ByVal pstrSistemaDestino As String, ByVal pstrAccion As String, ByVal pstrIDRegistro As String, ByVal pstrInformacionAdicional As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresBusqueda)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrInformacionAdicional = RetornarValorDescodificado(pstrInformacionAdicional)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)(pstrSistemaOrigen, pstrSistemaDestino, pstrAccion, pstrIDRegistro, pstrInformacionAdicional)
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarInformacionBusquedaReceptores(pstrOpcionCargar, strRespuestaXML, pstrFiltro, String.Empty, String.Empty).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_BuscarReceptoresDerivados")
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
        Dim L2SDC As New OyDPLUSOrdenesDerivadosDataContext
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