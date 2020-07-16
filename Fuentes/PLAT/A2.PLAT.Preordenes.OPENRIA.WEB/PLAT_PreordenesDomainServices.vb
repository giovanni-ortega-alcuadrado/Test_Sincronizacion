
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports OpenRiaServices.DomainServices.EntityFramework
Imports A2.OyD.Infraestructura
Imports System.Threading.Tasks


'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class PLAT_PreordenesDomainServices
    Inherits DbDomainService(Of dbPLAT_PreordenesEntities)

    Public Sub New()

    End Sub

#Region "PREORDENES"

    Public Sub InserttblPreOrdenes(ByVal pCurrentItem As tblPreOrdenes)
    End Sub
    Public Sub UpdatetblPreOrdenes(ByVal pCurrentItem As tblPreOrdenes)
    End Sub
    Public Sub DeletetblPreOrdenes(ByVal pCurrentItem As tblPreOrdenes)
    End Sub

    Public Sub InserttblPreOrdenes_Portafolio(ByVal pCurrentItem As tblPreOrdenes_Portafolio)
    End Sub
    Public Sub UpdatetblPreOrdenes_Portafolio(ByVal pCurrentItem As tblPreOrdenes_Portafolio)
    End Sub
    Public Sub DeletetblPreOrdenes_Portafolio(ByVal pCurrentItem As tblPreOrdenes_Portafolio)
    End Sub

    Public Function GettblPreOrdenes() As List(Of tblPreOrdenes)
        Return New List(Of tblPreOrdenes)
    End Function

    Public Function GettblPreOrdenes_Portafolio() As List(Of tblPreOrdenes_Portafolio)
        Return New List(Of tblPreOrdenes_Portafolio)
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenes_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblPreOrdenes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenes_Filtrar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenes_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenes_Consultar(ByVal pintID As Nullable(Of Integer),
                                         ByVal pdtmFechaInversion As Nullable(Of DateTime),
                                         ByVal pdtmFechaVigencia As Nullable(Of DateTime),
                                         ByVal pintIDPersona As Nullable(Of Integer),
                                         ByVal pstrIDComitente As String,
                                         ByVal pstrTipoPreOrden As String,
                                         ByVal pstrTipoInversion As String,
                                         ByVal pintIDEntidad As Nullable(Of Integer),
                                         ByVal pstrIntencion As String,
                                         ByVal plogActivo As Nullable(Of Boolean),
                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblPreOrdenes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenes_Consultar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_Consultar(pintID, pdtmFechaInversion, pdtmFechaVigencia, pintIDPersona, pstrIDComitente, pstrTipoPreOrden, pstrTipoInversion, pintIDEntidad, pstrIntencion, plogActivo, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenes_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenes_ConsultarID(ByVal pintID As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPreOrdenes
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenes_ConsultarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_ConsultarID(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenes_ConsultarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenes_ConsultarDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPreOrdenes
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenes_ConsultarDefecto", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_ConsultarID(-1, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenes_ConsultarDefecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenes_EliminarID(ByVal pintID As Nullable(Of Integer), ByVal pstrObservaciones As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, ByVal pstrMaquina As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenes_EliminarID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_Eliminar(pintID, pstrObservaciones, strUsuario, pstrMaquina, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenes_EliminarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenes_Validar(ByVal pintID As Nullable(Of Integer),
                                       ByVal pdtmFechaInversion As Nullable(Of DateTime),
                                       ByVal pdtmFechaVigencia As Nullable(Of DateTime),
                                       ByVal pintIDPersona As Nullable(Of Integer),
                                       ByVal pstrIDComitente As String,
                                       ByVal pstrTipoPreOrden As String,
                                       ByVal pstrTipoInversion As String,
                                       ByVal pintIDEntidad As Nullable(Of Integer),
                                       ByVal pstrInstrumento As String,
                                       ByVal pstrIntencion As String,
                                       ByVal pdblValor As Nullable(Of Double),
                                       ByVal pdblPrecio As Nullable(Of Double),
                                       ByVal pdblRentabilidadMinima As Nullable(Of Double),
                                       ByVal pdblRentabilidadMaxima As Nullable(Of Double),
                                       ByVal pstrInstrucciones As String,
                                       ByVal plogActivo As Nullable(Of Boolean),
                                       ByVal pstrDetallePortafolio As String,
                                       ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, ByVal pstrMaquina As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenes_Validar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_Validar(pintID, pdtmFechaInversion, pdtmFechaVigencia, pintIDPersona, pstrIDComitente, pstrTipoPreOrden, pstrTipoInversion, pintIDEntidad, pstrInstrumento, pstrIntencion, pdblValor, pdblPrecio, pdblRentabilidadMinima, pdblRentabilidadMaxima, pstrInstrucciones, plogActivo, pstrDetallePortafolio, False, strUsuario, pstrMaquina, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenes_Validar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenesPortafolio_Consultar(ByVal pintIDPreOrden As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblClientes_Portafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenesPortafolio_Consultar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_Portafolio_Consultar(pintIDPreOrden, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenesPortafolio_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenesClientes_ConsultarPortafolio(ByVal pstrIDComitente As String, ByVal pstrTipoInversion As String, ByVal pstrInstrumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblClientes_Portafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenesClientes_ConsultarPortafolio", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_Cliente_ConsultarPortafolio(pstrIDComitente, pstrTipoInversion, pstrInstrumento, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenesClientes_ConsultarPortafolio")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenesEspecie_Entidad(ByVal pstrInstrumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CPX_tblEntidadEspecie
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenesEspecie_Entidad", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_Entidad_PorDefecto(pstrInstrumento, strUsuario, strInfoSession).ToList
            If objRetorno.Count > 0 Then
                Return objRetorno.First
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenesEspecie_Entidad")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function VisorPreOrdenes_Consultar(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblPreOrdenes_Visor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("VisorPreOrdenes_Consultar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_VisorPreOrdenes_Consultar(strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "VisorPreOrdenes_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function VisorPreOrdenes_ConsultarCruzadas(ByVal pdtmFechaInicial As DateTime, ByVal pdtmFechaFinal As DateTime, ByVal pintIDPreOrden As Nullable(Of Integer), ByVal plogSoloUsuario As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblPreOrdenes_VisorCruces)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("VisorPreOrdenes_ConsultarCruzadas", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_VisorPreOrdenes_ConsultarCruzadas(pdtmFechaInicial, pdtmFechaFinal, pintIDPreOrden, plogSoloUsuario, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "VisorPreOrdenes_ConsultarCruzadas")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function VisorPreOrdenes_Generar(ByVal pintIDPreOrdenPrincipal As Integer, ByVal pdblValorPrincipal As Double, ByVal pstrRegistrosAsociacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, ByVal pstrMaquina As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("VisorPreOrdenes_Generar", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_VisorPreOrdenes_Generar(pintIDPreOrdenPrincipal, pdblValorPrincipal, pstrRegistrosAsociacion, strUsuario, pstrMaquina, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "VisorPreOrdenes_Generar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreOrdenes_ConsultarCruzadaID(ByVal pintID As Nullable(Of Integer), ByVal pstrTipoPreOrden As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CPX_tblPreOrdenes_Cruzadas
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PreOrdenes_ConsultarCruzadaID", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_PreOrdenes_ConsultarID_Cruzada(pintID, pstrTipoPreOrden, strUsuario, strInfoSession).ToList
            Return objRetorno.First
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "PreOrdenes_ConsultarCruzadaID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function VisorPreOrdenes_AsociarOrden(ByVal pintID As Integer, ByVal pstrOrigenOrden As String, ByVal pintNroRegistro As Integer, ByVal pstrClaseRegistro As String, ByVal pstrTipoOperacionRegistro As String, ByVal pstrTipoOrigenRegistro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("VisorPreOrdenes_AsociarOrden", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_VisorPreOrdenes_AsociarOrden(pintID, pstrOrigenOrden, pintNroRegistro, pstrClaseRegistro, pstrTipoOperacionRegistro, pstrTipoOrigenRegistro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "VisorPreOrdenes_AsociarOrden")
            Return Nothing
        End Try
    End Function

    Public Function VisorPreOrdenes_VerificarAsociarOrden(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("VisorPreOrdenes_VerificarAsociarOrden", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim logRetornoParam As System.Data.Entity.Core.Objects.ObjectParameter = New System.Data.Entity.Core.Objects.ObjectParameter("plogRegistroValido", GetType(Boolean))
            DbContext.usp_VisorPreOrdenes_VerificarAsociarOrden(pintID, logRetornoParam, strUsuario, strInfoSession)
            Return CBool(logRetornoParam.Value)
        Catch ex As Exception
            ManejarError(ex, "PREORDENES", "VisorPreOrdenes_VerificarAsociarOrden")
            Return Nothing
        End Try
    End Function

#End Region

End Class

