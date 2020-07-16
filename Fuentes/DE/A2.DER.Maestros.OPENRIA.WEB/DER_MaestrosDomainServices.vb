
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


'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class DER_MaestrosDomainServices
    Inherits DbDomainService(Of dbDER_MaestrosEntities)

    Public Sub New()

    End Sub

#Region "UTILIDADES"

    <Invoke(HasSideEffects:=True)>
    Public Function Utilitarios_ConsultarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblCombos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("UTILITARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Procesos_Utilitarios_CargarCombos(strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "UTILITARIOS", "Utilitarios_ConsultarCombos")
            Return Nothing
        End Try
    End Function

#End Region



#Region "INSTRUMENTOS"

#Region "Productos"

    Public Sub InserttblProductos(ByVal pCurrentItem As tblProductos)
    End Sub
    Public Sub UpdatetblProductos(ByVal pCurrentItem As tblProductos)
    End Sub
    Public Sub DeletetblProductos(ByVal pCurrentItem As tblProductos)
    End Sub
    Public Function EntidadtblProductos() As tblProductos
        Return New tblProductos
    End Function


    <Invoke(HasSideEffects:=True)>
    Public Function Productos_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblProductos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_Productos_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOS", "Productos_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Productos_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblProductos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_Productos_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOS", "Productos_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Productos_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblProductos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_Productos_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOS", "Productos_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Productos_Consultar(ByVal pstrNombreProducto As String, ByVal pintTipoProducto As Short, ByVal pintTipoCumplimiento As Integer, ByVal pintActivoSubyacente As Integer, ByVal pintEstado As Short, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblProductos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrNombreProducto = System.Web.HttpUtility.UrlDecode(pstrNombreProducto)

            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblProductos)

            objRetorno = DbContext.usp_Maestros_Productos_Consultar(pstrNombreProducto, pintTipoProducto, pintTipoCumplimiento, pintActivoSubyacente, pintEstado, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOS", "Productos_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Productos_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CLIENTES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_Productos_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOS", "Productos_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Productos_Actualizar(ByVal pintIdProducto As Nullable(Of Integer),
                                         ByVal pstrNombreProducto As String,
                                         ByVal pstrDescripcion As String,
                                         ByVal pintIdTipoProducto As Integer,
                                         ByVal pintIdActivoSubyacente As Integer,
                                         ByVal pintIdTipoCumplimiento As Integer,
                                         ByVal pintIdTipoEjercicio As Integer,
                                         ByVal pintUltimoDiaNegociacion As Integer,
                                         ByVal pnumValorNominalContrato As Decimal,
                                         ByVal pintIdTipoNegociacionPrima As Integer,
                                         ByVal pintIdEstado As Integer,
                                         ByVal pstrPrefijoProducto As String,
                                         ByVal pintNroDecimalesPrecio As Integer,
                                         ByVal pintNroDecimalesPrima As Integer,
                                         ByVal pnumTickPrecio As Nullable(Of Decimal),
                                         ByVal pintMaximoNegociacion As Nullable(Of Integer),
                                         ByVal pintMinimoRegistro As Nullable(Of Integer),
                                         ByVal pintMinimoNegociacionPolitica As Nullable(Of Integer),
                                         ByVal pintTipoContrato As Nullable(Of Integer),
                                         ByVal pnumPorcenAutoretencion As Nullable(Of Decimal),
                                         ByVal pintIdTipoLiquidacion As Nullable(Of Integer),
                                         ByVal pintIdTipoDerivado As Nullable(Of Integer),
                                         ByVal pintIdTipoCurva As Nullable(Of Integer),
                                         ByVal pintIdTipoSubyacente As Integer,
                                         ByVal pbitPrecioPorcentual As Boolean,
                                         ByVal pintIdProductoSubyacente As Nullable(Of Integer),
                                         ByVal plogSoloValidar As Nullable(Of Boolean),
                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_Productos_Validar(pintIdProducto, pstrNombreProducto, pstrDescripcion, pintIdTipoProducto,
                                                                  pintIdActivoSubyacente, pintIdTipoCumplimiento, pintIdTipoEjercicio,
                                                                  pintUltimoDiaNegociacion, pnumValorNominalContrato, pintIdTipoNegociacionPrima,
                                                                  pintIdEstado, pstrPrefijoProducto, pintNroDecimalesPrecio, pintNroDecimalesPrima,
                                                                  pnumTickPrecio, pintMaximoNegociacion, pintMinimoRegistro, pintMinimoNegociacionPolitica,
                                                                  pintTipoContrato, pnumPorcenAutoretencion, pintIdTipoLiquidacion, pintIdTipoDerivado,
                                                                  pintIdTipoCurva, pintIdTipoSubyacente, pbitPrecioPorcentual, pintIdProductoSubyacente,
                                                                  plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOS", "Productos_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Productos Vencimientos"

    Public Sub InserttblProductosVencimientos(ByVal pCurrentItem As tblProductosVencimientos)
    End Sub
    Public Sub UpdatetblProductosVencimientos(ByVal pCurrentItem As tblProductosVencimientos)
    End Sub
    Public Sub DeletetblProductosVencimientos(ByVal pCurrentItem As tblProductosVencimientos)
    End Sub
    Public Function EntidadtblProductosVencimientos() As tblProductosVencimientos
        Return New tblProductosVencimientos
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ProductosVencimientos_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblProductosVencimientos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOSVENCIMIENTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_ProductosVencimientos_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOSVENCIMIENTOS", "ProductosVencimientos_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ProductosVencimientos_Filtrar(ByVal pintIdProducto As Integer, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblProductosVencimientos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOSVENCIMIENTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_ProductosVencimientos_Filtrar(pintIdProducto, pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOSVENCIMIENTOS", "ProductosVencimiento_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ProductosVencimientos_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblProductosVencimientos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOSVENCIMIENTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_ProductosVencimientos_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOSVENCIMIENTOS", "ProductosVencimientos_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ProductosVencimientos_Consultar(ByVal pintIdProducto As Integer, ByVal pstrVencimiento As String, ByVal pstrNemotecnico As String, ByVal plogActivo As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblProductosVencimientos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOSVENCIMIENTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As List(Of tblProductosVencimientos)
            objRetorno = DbContext.usp_Maestros_ProductosVencimientos_Consultar(pintIdProducto, pstrVencimiento, pstrNemotecnico, plogActivo, pstrUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOSVENCIMIENTOS", "ProductosVencimientos_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ProductosVencimientos_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOSVENCIMIENTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_ProductosVencimientos_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOSVENCIMIENTOS", "ProductosVencimientos_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function ProductosVencimientos_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                      ByVal pintIdProducto As Nullable(Of Integer),
                                                      ByVal pstrVencimiento As String,
                                                      ByVal pstrNemotecnico As String,
                                                      ByVal pdtmFechaVencimiento As Date,
                                                      ByVal plogActivo As Boolean,
                                                      ByVal pintUltimoDiaNegPV As Integer,
                                                      ByVal pdtmFechaUltDiaNeg As Nullable(Of Date),
                                                      ByVal plogSoloValidar As Nullable(Of Boolean),
                                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRODUCTOSVENCIMIENTOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_ProductosVencimientos_Validar(pintID,
                                                                              pintIdProducto,
                                                                              pstrVencimiento,
                                                                              pstrNemotecnico,
                                                                              pdtmFechaVencimiento,
                                                                              plogActivo,
                                                                              pintUltimoDiaNegPV,
                                                                              pdtmFechaUltDiaNeg,
                                                                              plogSoloValidar,
                                                                              strUsuario,
                                                                              strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRODUCTOSVENCIMIENTOS", "ProductosVencimientos_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#End Region

#Region "DATOS TRIBUTARIOS"

#Region "Datos tributarios"

    Public Sub InserttblDatosTributarios(ByVal pCurrentItem As tblDatosTributarios)
    End Sub
    Public Sub UpdatetblDatosTributarios(ByVal pCurrentItem As tblDatosTributarios)
    End Sub
    Public Sub DeletetblDatosTributarios(ByVal pCurrentItem As tblDatosTributarios)
    End Sub
    Public Function EntidadtblDatosTributarios() As tblDatosTributarios
        Return New tblDatosTributarios
    End Function


    <Invoke(HasSideEffects:=True)>
    Public Function DatosTributarios_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblDatosTributarios
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("DATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_DatosTributarios_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DATOSTRIBUTARIOS", "DatosTributarios_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DatosTributarios_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblDatosTributarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("DATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_DatosTributarios_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DATOSTRIBUTARIOS", "DatosTributarios_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DatosTributarios_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblDatosTributarios
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("DATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_DatosTributarios_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DATOSTRIBUTARIOS", "DatosTributarios_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DatosTributarios_Consultar(ByVal pstrNombre As String, ByVal pstrDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblDatosTributarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrNombre = System.Web.HttpUtility.UrlDecode(pstrNombre)
            pstrDescripcion = System.Web.HttpUtility.UrlDecode(pstrDescripcion)

            Dim strInfoSession As String = DemeInfoSesion("DATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblDatosTributarios)

            objRetorno = DbContext.usp_Maestros_DatosTributarios_Consultar(pstrNombre, pstrDescripcion, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DATOSTRIBUTARIOS", "DatosTributarios_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DatosTributarios_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("DATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_DatosTributarios_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DATOSTRIBUTARIOS", "DatosTributarios_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DatosTributarios_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                 ByVal pstrNombre As String,
                                                 ByVal pstrDescripcion As String,
                                                 ByVal plogSoloValidar As Nullable(Of Boolean),
                                                 ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("DATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_DatosTributarios_Validar(pintID, pstrNombre, pstrDescripcion, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DATOSTRIBUTARIOS", "DatosTributarios_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Datos tributarios detalle"

    Public Sub InserttblDetalleDatosTributarios(ByVal pCurrentItem As tblDetalleDatosTributarios)
    End Sub
    Public Sub UpdatetblDetalleDatosTributarios(ByVal pCurrentItem As tblDetalleDatosTributarios)
    End Sub
    Public Sub DeletetblDetalleDatosTributarios(ByVal pCurrentItem As tblDetalleDatosTributarios)
    End Sub
    Public Function EntidadtblDetalleDatosTributarios() As tblDetalleDatosTributarios
        Return New tblDetalleDatosTributarios
    End Function


    <Invoke(HasSideEffects:=True)>
    Public Function DetalleDatosTributarios_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblDetalleDatosTributarios
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("DETALLEDATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_DetalleDatosTributarios_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DETALLEDATOSTRIBUTARIOS", "DetalleDatosTributarios_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DetalleDatosTributarios_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblDetalleDatosTributarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("DETALLEDATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_DetalleDatosTributarios_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DETALLEDATOSTRIBUTARIOS", "DetalleDatosTributarios_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DetalleDatosTributarios_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblDetalleDatosTributarios
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("DETALLEDATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_DetalleDatosTributarios_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DETALLEDATOSTRIBUTARIOS", "DetalleDatosTributarios_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DetalleDatosTributarios_Consultar(ByVal pintIdDetalleDatosTributarios As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblDetalleDatosTributarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("DETALLEDATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblDetalleDatosTributarios)

            objRetorno = DbContext.usp_Maestros_DetalleDatosTributarios_Consultar(pintIdDetalleDatosTributarios, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DETALLEDATOSTRIBUTARIOS", "DetalleDatosTributarios_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DetalleDatosTributarios_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("DETALLEDATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_DetalleDatosTributarios_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DETALLEDATOSTRIBUTARIOS", "DetalleDatosTributarios_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DetalleDatosTributarios_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                       ByVal pintIdDatosTributarios As Nullable(Of Integer),
                                                       ByVal pdtmFechaInicio As Date,
                                                       ByVal pdtmFechaFinal As Nullable(Of Date),
                                                       ByVal pnumValor As Decimal,
                                                       ByVal plogSoloValidar As Nullable(Of Boolean),
                                                       ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("DETALLEDATOSTRIBUTARIOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_DetalleDatosTributarios_Validar(pintID, pintIdDatosTributarios, pdtmFechaInicio, pdtmFechaFinal, pnumValor, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DETALLEDATOSTRIBUTARIOS", "DetalleDatosTributarios_Actualizar")
            Return Nothing
        End Try
    End Function



#End Region



#End Region

#Region "CODIGO COMPAÑIAS ENCUENTA"

    Public Sub InserttblCodComEnc(ByVal pCurrentItem As tblCodComEnc)
    End Sub
    Public Sub UpdatetblCodComEnc(ByVal pCurrentItem As tblCodComEnc)
    End Sub
    Public Sub DeletetblCodComEnc(ByVal pCurrentItem As tblCodComEnc)
    End Sub
    Public Function EntidadtblCodComEnc() As tblCodComEnc
        Return New tblCodComEnc
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CodComEnc_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblCodComEnc
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CODCOMPANIAENCUENTA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_CodCompaniaEncuenta_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CODCOMPANIAENCUENTA", "CodComEnc_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CodComEnc_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblCodComEnc)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("CODCOMPANIAENCUENTA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_CodCompaniaEncuenta_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CODCOMPANIAENCUENTA", "CodComEnc_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CodComEnc_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblCodComEnc
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CODCOMPANIAENCUENTA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_CodCompaniaEncuenta_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CODCOMPANIAENCUENTA", "CodComEnc_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CodComEnc_Consultar(ByVal pintIdPlanoContable As Integer, ByVal pstrNroDocumento As String, ByVal pintIdCuentaMultiproducto As Integer, ByVal pstrCodComEnc As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblCodComEnc)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrNroDocumento = System.Web.HttpUtility.UrlDecode(pstrNroDocumento)
            pstrCodComEnc = System.Web.HttpUtility.UrlDecode(pstrCodComEnc)

            Dim strInfoSession As String = DemeInfoSesion("CODCOMPANIAENCUENTA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblCodComEnc)

            objRetorno = DbContext.usp_Maestros_CodCompaniaEncuenta_Consultar(pintIdPlanoContable, pstrNroDocumento, pintIdCuentaMultiproducto, pstrCodComEnc, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CODCOMPANIAENCUENTA", "CodComEnc_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CodComEnc_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CODCOMPANIAENCUENTA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_CodCompaniaEncuenta_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CODCOMPANIAENCUENTA", "CodComEnc_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CodComEnc_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                 ByVal pintIdPlanoContable As Nullable(Of Integer),
                                                 ByVal pintIdCuentaMultiproducto As Integer,
                                                 ByVal pstrCodComEnc As String,
                                                 ByVal plogSoloValidar As Nullable(Of Boolean),
                                                 ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CODCOMPANIAENCUENTA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_CodCompaniaEncuenta_Validar(pintID, pintIdPlanoContable, pintIdCuentaMultiproducto, pstrCodComEnc, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CODCOMPANIAENCUENTA", "CodComEnc_Actualizar")
            Return Nothing
        End Try
    End Function


#End Region

#Region "PRECIOSDERIVADOS"

    Public Sub InserttblPreciosDerivados(ByVal pCurrentItem As tblPreciosDerivados)
    End Sub
    Public Sub UpdatetblPreciosDerivados(ByVal pCurrentItem As tblPreciosDerivados)
    End Sub
    Public Sub DeletetblPreciosDerivados(ByVal pCurrentItem As tblPreciosDerivados)
    End Sub
    Public Function EntidadtblPreciosDerivados() As tblPreciosDerivados
        Return New tblPreciosDerivados
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreciosDerivados_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPreciosDerivados
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRECIOSDERIVADOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_PreciosDerivados_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRECIOSDERIVADOS", "PreciosDerivados_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreciosDerivados_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblPreciosDerivados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("PRECIOSDERIVADOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_PreciosDerivados_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRECIOSDERIVADOS", "PreciosDerivados_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreciosDerivados_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblPreciosDerivados
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRECIOSDERIVADOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_PreciosDerivados_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRECIOSDERIVADOS", "PreciosDerivados_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreciosDerivados_Consultar(ByVal pdtmFecha As Date, ByVal pstrNemotecnico As String, ByVal pstrNemotecnicoSubyacente As String, ByVal pstrFuentePrecio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblPreciosDerivados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrNemotecnico = System.Web.HttpUtility.UrlDecode(pstrNemotecnico)
            pstrNemotecnicoSubyacente = System.Web.HttpUtility.UrlDecode(pstrNemotecnicoSubyacente)
            pstrFuentePrecio = System.Web.HttpUtility.UrlDecode(pstrFuentePrecio)

            Dim strInfoSession As String = DemeInfoSesion("PRECIOSDERIVADOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblPreciosDerivados)

            objRetorno = DbContext.usp_Maestros_PreciosDerivados_Consultar(pdtmFecha, pstrNemotecnico, pstrNemotecnicoSubyacente, pstrFuentePrecio, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRECIOSDERIVADOS", "PreciosDerivados_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreciosDerivados_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRECIOSDERIVADOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_PreciosDerivados_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRECIOSDERIVADOS", "PreciosDerivados_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function PreciosDerivados_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                 ByVal pdtmFechaRegistro As Nullable(Of Date),
                                                 ByVal pstrNemotecnico As String,
                                                 ByVal pnumStrike As Decimal,
                                                 ByVal pnumPrecio As Decimal,
                                                 ByVal plogSoloValidar As Nullable(Of Boolean),
                                                 ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("PRECIOSDERIVADOS", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_PreciosDerivados_Validar(pintID, pdtmFechaRegistro, pstrNemotecnico, pnumStrike, pnumPrecio, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "PRECIOSDERIVADOS", "PreciosDerivados_Actualizar")
            Return Nothing
        End Try
    End Function


#End Region

#Region "MENSAJE CUMPLIMIENTO POLITICA"

    Public Sub InserttblMensajesCumPolitica(ByVal pCurrentItem As tblMensajesCumPolitica)
    End Sub
    Public Sub UpdatetblMensajesCumPolitica(ByVal pCurrentItem As tblMensajesCumPolitica)
    End Sub
    Public Sub DeletetblMensajesCumPolitica(ByVal pCurrentItem As tblMensajesCumPolitica)
    End Sub
    Public Function EntidadtblMensajesCumPolitica() As tblMensajesCumPolitica
        Return New tblMensajesCumPolitica
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function MensajesCumPolitica_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblMensajesCumPolitica
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("MENSAJESCUMPOLITICA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_MensajesCumPolitica_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MENSAJESCUMPOLITICA", "MensajesCumPolitica_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function MensajesCumPolitica_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblMensajesCumPolitica)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("MENSAJESCUMPOLITICA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_MensajesCumPolitica_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MENSAJESCUMPOLITICA", "MensajesCumPolitica_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function MensajesCumPolitica_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblMensajesCumPolitica
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("MENSAJESCUMPOLITICA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_MensajesCumPolitica_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MENSAJESCUMPOLITICA", "MensajesCumPolitica_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function MensajesCumPolitica_Consultar(ByVal pstrMensaje As String, ByVal pintEstadoCumplimiento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblMensajesCumPolitica)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrMensaje = System.Web.HttpUtility.UrlDecode(pstrMensaje)

            Dim strInfoSession As String = DemeInfoSesion("MENSAJESCUMPOLITICA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblMensajesCumPolitica)

            objRetorno = DbContext.usp_Maestros_MensajesCumPolitica_Consultar(pstrMensaje, pintEstadoCumplimiento, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MENSAJESCUMPOLITICA", "MensajesCumPolitica_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function MensajesCumPolitica_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("MENSAJESCUMPOLITICA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_MensajesCumPolitica_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MENSAJESCUMPOLITICA", "MensajesCumPolitica_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function MensajesCumPolitica_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pintEstadoCumplimiento As Nullable(Of Integer),
                                                     ByVal pstrMensaje As String,
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("MENSAJESCUMPOLITICA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_MensajesCumPolitica_Validar(pintID, pintEstadoCumplimiento, pstrMensaje, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MENSAJESCUMPOLITICA", "MensajesCumPolitica_Actualizar")
            Return Nothing
        End Try
    End Function


#End Region

#Region "CUENTAS INSTRUMENTOS CRCC"

    Public Sub InserttblCuentasInstrumentos(ByVal pCurrentItem As tblCuentasInstrumentos)
    End Sub
    Public Sub UpdatetblCuentasInstrumentos(ByVal pCurrentItem As tblCuentasInstrumentos)
    End Sub
    Public Sub DeletetblCuentasInstrumentos(ByVal pCurrentItem As tblCuentasInstrumentos)
    End Sub
    Public Function EntidadtblCuentasInstrumentos() As tblCuentasInstrumentos
        Return New tblCuentasInstrumentos
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CuentasInstrumentos_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblCuentasInstrumentos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CUENTASINSTRUMENTOSCRCC", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_CuentasInstrumentosCRCC_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CUENTASINSTRUMENTOSCRCC", "CuentasInstrumentos_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CuentasInstrumentos_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblCuentasInstrumentos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("CUENTASINSTRUMENTOSCRCC", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_CuentasInstrumentosCRCC_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CUENTASINSTRUMENTOSCRCC", "CuentasInstrumentos_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CuentasInstrumentos_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblCuentasInstrumentos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CUENTASINSTRUMENTOSCRCC", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_CuentasInstrumentosCRCC_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CUENTASINSTRUMENTOSCRCC", "CuentasInstrumentos_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CuentasInstrumentos_Consultar(ByVal pintIdCuentaCRCC As Integer, ByVal pintIdProducto As Integer, ByVal pintIdCuentaCRCCReem As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblCuentasInstrumentos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim strInfoSession As String = DemeInfoSesion("CUENTASINSTRUMENTOSCRCC", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblCuentasInstrumentos)

            objRetorno = DbContext.usp_Maestros_CuentasInstrumentosCRCC_Consultar(pintIdCuentaCRCC, pintIdProducto, pintIdCuentaCRCCReem, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CUENTASINSTRUMENTOSCRCC", "CuentasInstrumentos_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CuentasInstrumentos_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CUENTASINSTRUMENTOSCRCC", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_CuentasInstrumentosCRCC_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CUENTASINSTRUMENTOSCRCC", "CuentasInstrumentos_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function CuentasInstrumentos_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pintIdCuentaCRCC As Integer,
                                                     ByVal pintIdSubcuentaCRCC As Nullable(Of Integer),
                                                     ByVal pintIdProducto As Nullable(Of Integer),
                                                     ByVal pintIdCuentaCRCCReem As Integer,
                                                     ByVal pintIdSubcuentaCRCCReem As Nullable(Of Integer),
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("CUENTASINSTRUMENTOSCRCC", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_CuentasInstrumentosCRCC_Validar(pintID, pintIdCuentaCRCC, pintIdSubcuentaCRCC, pintIdProducto, pintIdCuentaCRCCReem,
                                                                                pintIdSubcuentaCRCCReem, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CUENTASINSTRUMENTOSCRCC", "CuentasInstrumentos_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "FIRMAS GIVE-UP"

    Public Sub InserttblFirmasGiveUp(ByVal pCurrentItem As tblFirmasGiveUp)
    End Sub
    Public Sub UpdatetblFirmasGiveUp(ByVal pCurrentItem As tblFirmasGiveUp)
    End Sub
    Public Sub DeletetblFirmasGiveUp(ByVal pCurrentItem As tblFirmasGiveUp)
    End Sub
    Public Function EntidadtblFirmasGiveUp() As tblFirmasGiveUp
        Return New tblFirmasGiveUp
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FirmasGiveUp_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblFirmasGiveUp
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("FIRMASGIVEUP", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_FirmasGiveUp_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FIRMASGIVEUP", "FirmasGiveUp_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FirmasGiveUp_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblFirmasGiveUp)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("FIRMASGIVEUP", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_FirmasGiveUp_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FIRMASGIVEUP", "FirmasGiveUp_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FirmasGiveUp_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblFirmasGiveUp
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("FIRMASGIVEUP", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_FirmasGiveUp_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FIRMASGIVEUP", "FirmasGiveUp_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FirmasGiveUp_Consultar(ByVal pintIdFirmaNegocio As Integer,
                                           ByVal pstrReferenciaGiveOut As String,
                                           ByVal pbitAceptaGiveOut As Nullable(Of Boolean),
                                           ByVal pbitAceptaGiveIn As Nullable(Of Boolean),
                                           ByVal plogActiva As Nullable(Of Boolean),
                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblFirmasGiveUp)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            pstrReferenciaGiveOut = System.Web.HttpUtility.UrlDecode(pstrReferenciaGiveOut)

            Dim strInfoSession As String = DemeInfoSesion("FIRMASGIVEUP", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblFirmasGiveUp)

            objRetorno = DbContext.usp_Maestros_FirmasGiveUp_Consultar(pintIdFirmaNegocio, pstrReferenciaGiveOut, pbitAceptaGiveOut, pbitAceptaGiveIn,
                                                                       plogActiva, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FIRMASGIVEUP", "FirmasGiveUp_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FirmasGiveUps_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("FIRMASGIVEUP", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_FirmasGiveUp_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FIRMASGIVEUP", "FirmasGiveUps_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FirmasGiveUp_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pintIdFirmaNegocio As Integer,
                                                     ByVal pstrReferenciaGiveOut As String,
                                                     ByVal pbitAceptaGiveOut As Nullable(Of Boolean),
                                                     ByVal pbitAceptaGiveIn As Nullable(Of Boolean),
                                                     ByVal plogActiva As Nullable(Of Boolean),
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("FIRMASGIVEUP", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_FirmasGiveUp_Validar(pintID, pintIdFirmaNegocio, pstrReferenciaGiveOut, pbitAceptaGiveOut,
                                                                             pbitAceptaGiveIn, plogActiva, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FIRMASGIVEUP", "FirmasGiveUp_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "TIPO EJERCICIO"

    Public Sub InserttblTipoEjercicio(ByVal pCurrentItem As tblTipoEjercicio)
    End Sub
    Public Sub UpdatetblTipoEjercicio(ByVal pCurrentItem As tblTipoEjercicio)
    End Sub
    Public Sub DeletetblTipoEjercicio(ByVal pCurrentItem As tblTipoEjercicio)
    End Sub
    Public Function EntidadtblTipoEjercicio() As tblTipoEjercicio
        Return New tblTipoEjercicio
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoEjercicio_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoEjercicio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOEJERCICIO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoEjercicio_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOEJERCICIO", "TipoEjercicio_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoEjercicio_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoEjercicio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("TIPOEJERCICIO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoEjercicio_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOEJERCICIO", "TipoEjercicio_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoEjercicio_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoEjercicio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOEJERCICIO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoEjercicio_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOEJERCICIO", "TipoEjercicio_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoEjercicio_Consultar(ByVal pintIdTipoProducto As Integer,
                                           ByVal pstrNombre As String,
                                           ByVal pstrCodigo As String,
                                           ByVal pstrDescripcion As String,
                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoEjercicio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            pstrNombre = System.Web.HttpUtility.UrlDecode(pstrNombre)
            pstrCodigo = System.Web.HttpUtility.UrlDecode(pstrCodigo)
            pstrDescripcion = System.Web.HttpUtility.UrlDecode(pstrDescripcion)

            Dim strInfoSession As String = DemeInfoSesion("TIPOEJERCICIO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblTipoEjercicio)

            objRetorno = DbContext.usp_Maestros_TipoEjercicio_Consultar(pintIdTipoProducto, pstrNombre, pstrCodigo, pstrDescripcion, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOEJERCICIO", "TipoEjercicio_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoEjercicio_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOEJERCICIO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoEjercicio_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOEJERCICIO", "TipoEjercicio_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoEjercicio_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pintIdTipoProducto As Integer,
                                                     ByVal pstrCodigoTipoEjercicio As String,
                                                     ByVal pstrNombreTipoEjercicio As String,
                                                     ByVal pstrDescripcion As String,
                                                     ByVal pbitPorDefecto As Nullable(Of Boolean),
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOEJERCICIO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_TipoEjercicio_Validar(pintID, pintIdTipoProducto, pstrCodigoTipoEjercicio, pstrNombreTipoEjercicio,
                                                                      pstrDescripcion, pbitPorDefecto, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOEJERCICIO", "TipoEjercicio_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "TIPO ORDEN EJECUCION"

    Public Sub InserttblTipoOrdenEjecucion(ByVal pCurrentItem As tblTipoOrdenEjecucion)
    End Sub
    Public Sub UpdatetblTipoOrdenEjecucion(ByVal pCurrentItem As tblTipoOrdenEjecucion)
    End Sub
    Public Sub DeletetblTipoOrdenEjecucion(ByVal pCurrentItem As tblTipoOrdenEjecucion)
    End Sub
    Public Function EntidadtblTipoOrdenEjecucion() As tblTipoOrdenEjecucion
        Return New tblTipoOrdenEjecucion
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenEjecucion_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoOrdenEjecucion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENEJECUCION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenEjecucion_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENEJECUCION", "TipoOrdenEjecucion_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenEjecucion_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoOrdenEjecucion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENEJECUCION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenEjecucion_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENEJECUCION", "TipoOrdenEjecucion_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenEjecucion_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoOrdenEjecucion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENEJECUCION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenEjecucion_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENEJECUCION", "TipoOrdenEjecucion_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenEjecucion_Consultar(ByVal pstrCodigoTipoOrdenEjecucion As String,
                                           ByVal pstrNombreTipoOrdenEjecucion As String,
                                           ByVal pstrDescripcion As String,
                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoOrdenEjecucion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            pstrCodigoTipoOrdenEjecucion = System.Web.HttpUtility.UrlDecode(pstrCodigoTipoOrdenEjecucion)
            pstrNombreTipoOrdenEjecucion = System.Web.HttpUtility.UrlDecode(pstrNombreTipoOrdenEjecucion)
            pstrDescripcion = System.Web.HttpUtility.UrlDecode(pstrDescripcion)

            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENEJECUCION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblTipoOrdenEjecucion)

            objRetorno = DbContext.usp_Maestros_TipoOrdenEjecucion_Consultar(pstrCodigoTipoOrdenEjecucion, pstrNombreTipoOrdenEjecucion, pstrDescripcion, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENEJECUCION", "TipoOrdenEjecucion_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenEjecucion_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENEJECUCION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenEjecucion_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENEJECUCION", "TipoOrdenEjecucion_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenEjecucion_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pstrCodigoTipoOrdenEjecucion As String,
                                                     ByVal pstrNombreTipoOrdenEjecucion As String,
                                                     ByVal pbitPorDefecto As Nullable(Of Boolean),
                                                     ByVal pbitCantidadMinima As Nullable(Of Boolean),
                                                     ByVal pstrDescripcion As String,
                                                     ByVal pbitRequiereInstrumento As Nullable(Of Boolean),
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENEJECUCION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_TipoOrdenEjecucion_Validar(pintID, pstrCodigoTipoOrdenEjecucion, pstrNombreTipoOrdenEjecucion, pbitPorDefecto, pbitCantidadMinima,
                                                                           pstrDescripcion, pbitRequiereInstrumento, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENEJECUCION", "TipoOrdenEjecucion_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "TIPO ORDEN NATURALEZA"

    Public Sub InserttblTipoOrdenNaturaleza(ByVal pCurrentItem As tblTipoOrdenNaturaleza)
    End Sub
    Public Sub UpdatetblTipoOrdenNaturaleza(ByVal pCurrentItem As tblTipoOrdenNaturaleza)
    End Sub
    Public Sub DeletetblTipoOrdenNaturaleza(ByVal pCurrentItem As tblTipoOrdenNaturaleza)
    End Sub
    Public Function EntidadtblTipoOrdenNaturaleza() As tblTipoOrdenNaturaleza
        Return New tblTipoOrdenNaturaleza
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenNaturaleza_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoOrdenNaturaleza
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENNATURALEZA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenNaturaleza_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENNATURALEZA", "TipoOrdenNaturaleza_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenNaturaleza_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoOrdenNaturaleza)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENNATURALEZA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenNaturaleza_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENNATURALEZA", "TipoOrdenNaturaleza_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenNaturaleza_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoOrdenNaturaleza
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENNATURALEZA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenNaturaleza_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENNATURALEZA", "TipoOrdenNaturaleza_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenNaturaleza_Consultar(ByVal pstrCodigoTipoOrdenNaturaleza As String,
                                           ByVal pstrNombreTipoOrdenNaturaleza As String,
                                           ByVal pstrDescripcion As String,
                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoOrdenNaturaleza)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            pstrCodigoTipoOrdenNaturaleza = System.Web.HttpUtility.UrlDecode(pstrCodigoTipoOrdenNaturaleza)
            pstrNombreTipoOrdenNaturaleza = System.Web.HttpUtility.UrlDecode(pstrNombreTipoOrdenNaturaleza)
            pstrDescripcion = System.Web.HttpUtility.UrlDecode(pstrDescripcion)

            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENNATURALEZA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblTipoOrdenNaturaleza)

            objRetorno = DbContext.usp_Maestros_TipoOrdenNaturaleza_Consultar(pstrCodigoTipoOrdenNaturaleza, pstrNombreTipoOrdenNaturaleza, pstrDescripcion, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENNATURALEZA", "TipoOrdenNaturaleza_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenNaturaleza_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENNATURALEZA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenNaturaleza_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENNATURALEZA", "TipoOrdenNaturaleza_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenNaturaleza_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pstrCodigoTipoOrdenNaturaleza As String,
                                                     ByVal pstrNombreTipoOrdenNaturaleza As String,
                                                     ByVal pbitPorDefecto As Nullable(Of Boolean),
                                                     ByVal pbitRequierePrecio As Nullable(Of Boolean),
                                                     ByVal pbitPrecioStop As Nullable(Of Boolean),
                                                     ByVal pbitCantidadOculta As Nullable(Of Boolean),
                                                     ByVal pbitSeguirInstrumento As Nullable(Of Boolean),
                                                     ByVal pintIdTipoListaPrecio As Nullable(Of Integer),
                                                     ByVal pstrDescripcion As String,
                                                     ByVal pbitRequiereCantidad As Nullable(Of Boolean),
                                                     ByVal pbitRequierePrecioActivar As Nullable(Of Boolean),
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENNATURALEZA", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_TipoOrdenNaturaleza_Validar(pintID, pstrCodigoTipoOrdenNaturaleza, pstrNombreTipoOrdenNaturaleza, pbitPorDefecto,
                                                                            pbitRequierePrecio, pbitPrecioStop, pbitCantidadOculta, pbitSeguirInstrumento, pintIdTipoListaPrecio,
                                                                            pstrDescripcion, pbitRequiereCantidad, pbitRequierePrecioActivar, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENNATURALEZA", "TipoOrdenNaturaleza_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "TIPO ORDEN DURACION"

    Public Sub InserttblTipoOrdenDuracion(ByVal pCurrentItem As tblTipoOrdenDuracion)
    End Sub
    Public Sub UpdatetblTipoOrdenDuracion(ByVal pCurrentItem As tblTipoOrdenDuracion)
    End Sub
    Public Sub DeletetblTipoOrdenDuracion(ByVal pCurrentItem As tblTipoOrdenDuracion)
    End Sub
    Public Function EntidadtblTipoOrdenDuracion() As tblTipoOrdenDuracion
        Return New tblTipoOrdenDuracion
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenDuracion_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoOrdenDuracion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENDURACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenDuracion_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENDURACION", "TipoOrdenDuracion_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenDuracion_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoOrdenDuracion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENDURACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenDuracion_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENDURACION", "TipoOrdenDuracion_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenDuracion_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoOrdenDuracion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENDURACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenDuracion_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENDURACION", "TipoOrdenDuracion_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenDuracion_Consultar(ByVal pstrCodigoTipoOrdenDuracion As String,
                                           ByVal pstrNombreTipoOrdenDuracion As String,
                                           ByVal pstrDescripcion As String,
                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoOrdenDuracion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            pstrCodigoTipoOrdenDuracion = System.Web.HttpUtility.UrlDecode(pstrCodigoTipoOrdenDuracion)
            pstrNombreTipoOrdenDuracion = System.Web.HttpUtility.UrlDecode(pstrNombreTipoOrdenDuracion)
            pstrDescripcion = System.Web.HttpUtility.UrlDecode(pstrDescripcion)

            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENDURACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblTipoOrdenDuracion)

            objRetorno = DbContext.usp_Maestros_TipoOrdenDuracion_Consultar(pstrCodigoTipoOrdenDuracion, pstrNombreTipoOrdenDuracion, pstrDescripcion, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENDURACION", "TipoOrdenDuracion_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenDuracion_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENDURACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOrdenDuracion_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENDURACION", "TipoOrdenDuracion_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOrdenDuracion_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pstrCodigoTipoOrdenDuracion As String,
                                                     ByVal pstrNombreTipoOrdenDuracion As String,
                                                     ByVal pstrDescripcion As String,
                                                     ByVal pbitPorDefecto As Nullable(Of Boolean),
                                                     ByVal pbitRequiereFecha As Nullable(Of Boolean),
                                                     ByVal pintIdTipoListaAsociada As Nullable(Of Integer),
                                                     ByVal pstrEtiquetaLista As String,
                                                     ByVal pbitVigenciaMax As Nullable(Of Boolean),
                                                     ByVal pintIdTipoOpcionProducto As Nullable(Of Short),
                                                     ByVal pbitPermiteSeleccionarTipoSesion As Nullable(Of Boolean),
                                                     ByVal pintNroDiasVencimiento As Nullable(Of Integer),
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOORDENDURACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_TipoOrdenDuracion_Validar(pintID, pstrCodigoTipoOrdenDuracion, pstrNombreTipoOrdenDuracion, pstrDescripcion, pbitPorDefecto,
                                                                          pbitRequiereFecha, pintIdTipoListaAsociada, pstrEtiquetaLista, pbitVigenciaMax, pintIdTipoOpcionProducto,
                                                                          pbitPermiteSeleccionarTipoSesion, pintNroDiasVencimiento, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOORDENDURACION", "TipoOrdenDuracion_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "TIPO OPCION PRODUCTO"

    Public Sub InserttblTipoOpcionProducto(ByVal pCurrentItem As tblTipoOpcionProducto)
    End Sub
    Public Sub UpdatetblTipoOpcionProducto(ByVal pCurrentItem As tblTipoOpcionProducto)
    End Sub
    Public Sub DeletetblTipoOpcionProducto(ByVal pCurrentItem As tblTipoOpcionProducto)
    End Sub
    Public Function EntidadtblTipoOpcionProducto() As tblTipoOpcionProducto
        Return New tblTipoOpcionProducto
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOpcionProducto_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoOpcionProducto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOOPCIONPRODUCTO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOpcionProducto_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOOPCIONPRODUCTO", "TipoOrdenDuracion_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOpcionProducto_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoOpcionProducto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("TIPOOPCIONPRODUCTO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOpcionProducto_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOOPCIONPRODUCTO", "TipoOpcionProducto_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOpcionProducto_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoOpcionProducto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOOPCIONPRODUCTO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOpcionProducto_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOOPCIONPRODUCTO", "TipoOpcionProducto_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOpcionProducto_Consultar(ByVal pstrNombreTipoOpcion As String,
                                                   ByVal pstrCodigoTipoOpcion As String,
                                                   ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoOpcionProducto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            pstrNombreTipoOpcion = System.Web.HttpUtility.UrlDecode(pstrNombreTipoOpcion)
            pstrCodigoTipoOpcion = System.Web.HttpUtility.UrlDecode(pstrCodigoTipoOpcion)

            Dim strInfoSession As String = DemeInfoSesion("TIPOOPCIONPRODUCTO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblTipoOpcionProducto)

            objRetorno = DbContext.usp_Maestros_TipoOpcionProducto_Consultar(pstrNombreTipoOpcion, pstrCodigoTipoOpcion, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOOPCIONPRODUCTO", "TipoOpcionProducto_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOpcionProducto_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOOPCIONPRODUCTO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoOpcionProducto_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOOPCIONPRODUCTO", "TipoOpcionProducto_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoOpcionProducto_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pstrCodigoTipoOpcion As String,
                                                     ByVal pstrNombreTipoOpcion As String,
                                                     ByVal pintIdTipoProducto As Nullable(Of Integer),
                                                     ByVal pbitProductosEspeciales As Nullable(Of Boolean),
                                                     ByVal pbitPrima As Nullable(Of Boolean),
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOOPCIONPRODUCTO", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_TipoOpcionProducto_Validar(pintID, pstrCodigoTipoOpcion, pstrNombreTipoOpcion, pintIdTipoProducto,
                                                                           pbitProductosEspeciales, pbitPrima, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOOPCIONPRODUCTO", "TipoOpcionProducto_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "FINALIDAD OPERACION"

#Region "Finalidad Operacion"

    Public Sub InserttblFinalidadOperaciones(ByVal pCurrentItem As tblFinalidadOperaciones)
    End Sub
    Public Sub UpdatetblFinalidadOperaciones(ByVal pCurrentItem As tblFinalidadOperaciones)
    End Sub
    Public Sub DeletetblFinalidadOperaciones(ByVal pCurrentItem As tblFinalidadOperaciones)
    End Sub
    Public Function EntidadtblFinalidadOperaciones() As tblFinalidadOperaciones
        Return New tblFinalidadOperaciones
    End Function


    <Invoke(HasSideEffects:=True)>
    Public Function FinalidadOperaciones_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblFinalidadOperaciones
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("FINALIDADOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_FinalidadOperaciones_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FINALIDADOPERACIONES", "FinalidadOperaciones_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FinalidadOperaciones_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblFinalidadOperaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("FINALIDADOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_FinalidadOperaciones_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FINALIDADOPERACIONES", "FinalidadOperaciones_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FinalidadOperaciones_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblFinalidadOperaciones
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("FINALIDADOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_FinalidadOperaciones_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FINALIDADOPERACIONES", "FinalidadOperaciones_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FinalidadOperaciones_Consultar(ByVal pstrDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblFinalidadOperaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            pstrDescripcion = System.Web.HttpUtility.UrlDecode(pstrDescripcion)

            Dim strInfoSession As String = DemeInfoSesion("FINALIDADOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblFinalidadOperaciones)

            objRetorno = DbContext.usp_Maestros_FinalidadOperaciones_Consultar(pstrDescripcion, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FINALIDADOPERACIONES", "FinalidadOperaciones_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FinalidadOperaciones_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("FINALIDADOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_FinalidadOperaciones_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FINALIDADOPERACIONES", "FinalidadOperaciones_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function FinalidadOperaciones_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pstrDescripcion As String,
                                                     ByVal pbitPorDefecto As Nullable(Of Boolean),
                                                     ByVal pbitPermiteComplemento As Nullable(Of Boolean),
                                                     ByVal pintIdTipoLista As Nullable(Of Integer),
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("FINALIDADOPERACIONES", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_FinalidadOperaciones_Validar(pintID, pstrDescripcion, pbitPorDefecto, pbitPermiteComplemento, pintIdTipoLista, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FINALIDADOPERACIONES", "FinalidadOperaciones_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Finalidad Operacion detalle"

    Public Sub InserttblTipoFinalidad(ByVal pCurrentItem As tblTipoFinalidad)
    End Sub
    Public Sub UpdatetblTipoFinalidad(ByVal pCurrentItem As tblTipoFinalidad)
    End Sub
    Public Sub DeletetblTipoFinalidad(ByVal pCurrentItem As tblTipoFinalidad)
    End Sub
    Public Function EntidadtblTipoFinalidad() As tblTipoFinalidad
        Return New tblTipoFinalidad
    End Function


    <Invoke(HasSideEffects:=True)>
    Public Function TipoFinalidad_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoFinalidad
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOFINALIDAD", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoFinalidad_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOFINALIDAD", "TipoFinalidad_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoFinalidad_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoFinalidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("TIPOFINALIDAD", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoFinalidad_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOFINALIDAD", "TipoFinalidad_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoFinalidad_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoFinalidad
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOFINALIDAD", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoFinalidad_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOFINALIDAD", "TipoFinalidad_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoFinalidad_Consultar(ByVal pintIdFinalidadOperacion As Integer, ByVal pstrDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoFinalidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOFINALIDAD", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of tblTipoFinalidad)

            objRetorno = DbContext.usp_Maestros_TipoFinalidad_Consultar(pintIdFinalidadOperacion, pstrDescripcion, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOFINALIDAD", "TipoFinalidad_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoFinalidad_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOFINALIDAD", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoFinalidad_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOFINALIDAD", "TipoFinalidad_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoFinalidad_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                       ByVal pstrDescripcion As String,
                                                       ByVal pbitPorDefecto As Nullable(Of Boolean),
                                                       ByVal pintIdFinalidadOperacion As Nullable(Of Integer),
                                                       ByVal pintIdTipoLista As Nullable(Of Integer),
                                                       ByVal pbitSolicitarMonto As Nullable(Of Boolean),
                                                       ByVal plogSoloValidar As Nullable(Of Boolean),
                                                       ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPOFINALIDAD", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_TipoFinalidad_Validar(pintID, pstrDescripcion, pbitPorDefecto, pintIdFinalidadOperacion, pintIdTipoLista, pbitSolicitarMonto, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPOFINALIDAD", "TipoFinalidad_Actualizar")
            Return Nothing
        End Try
    End Function



#End Region


#End Region

#Region "TIPO DOCUMENTO AUTORIZACION"

    Public Sub InserttblTipoDocAutorizacion(ByVal pCurrentItem As tblTipoDocAutorizacion)
    End Sub
    Public Sub UpdatetblTipoDocAutorizacion(ByVal pCurrentItem As tblTipoDocAutorizacion)
    End Sub
    Public Sub DeletetblTipoDocAutorizacion(ByVal pCurrentItem As tblTipoDocAutorizacion)
    End Sub
    Public Function EntidadtblTipoDocAutorizaciona() As tblTipoDocAutorizacion
        Return New tblTipoDocAutorizacion
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoDocAutorizacion_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoDocAutorizacion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPODOCAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoDocAutorizacion_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPODOCAUTORIZACION", "TipoDocAutorizacion_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoDocAutorizacion_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblTipoDocAutorizacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("TIPODOCAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoDocAutorizacion_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPODOCAUTORIZACION", "TipoDocAutorizacion_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoDocAutorizacion_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblTipoDocAutorizacion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPODOCAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoDocAutorizacion_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPODOCAUTORIZACION", "TipoDocAutorizacion_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoDocAutorizacion_Consultar(ByVal pstrCodigoTipoDocumento As String, ByVal pintTipoProducto As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblTipoDocAutorizacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrCodigoTipoDocumento = System.Web.HttpUtility.UrlDecode(pstrCodigoTipoDocumento)

            Dim strInfoSession As String = DemeInfoSesion("TIPODOCAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblTipoDocAutorizacion)

            objRetorno = DbContext.usp_Maestros_TipoDocAutorizacion_Consultar(pstrCodigoTipoDocumento, pintTipoProducto, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPODOCAUTORIZACION", "TipoDocAutorizacion_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoDocAutorizacion_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPODOCAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_TipoDocAutorizacion_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPODOCAUTORIZACION", "TipoDocAutorizacion_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function TipoDocAutorizacion_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pintidTipoProducto As Nullable(Of Integer),
                                                     ByVal pstrCodigoTipoDocumento As String,
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("TIPODOCAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_TipoDocAutorizacion_Validar(pintID, pintidTipoProducto, pstrCodigoTipoDocumento, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TIPODOCAUTORIZACION", "TipoDocAutorizacion_Actualizar")
            Return Nothing
        End Try
    End Function


#End Region

#Region "NIVEL AUTORIZACION"

    Public Sub InserttblNivelAutorizacion(ByVal pCurrentItem As tblNivelAutorizacion)
    End Sub
    Public Sub UpdatetblNivelAutorizacion(ByVal pCurrentItem As tblNivelAutorizacion)
    End Sub
    Public Sub DeletetblNivelAutorizacion(ByVal pCurrentItem As tblNivelAutorizacion)
    End Sub
    Public Function EntidadtblNivelAutorizacion() As tblNivelAutorizacion
        Return New tblNivelAutorizacion
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function NivelAutorizacion_ID(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblNivelAutorizacion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("NIVELAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_NivelAutorizacion_ConsultarID(pintID, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "NIVELAUTORIZACION", "NivelAutorizacion_ID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function NivelAutorizacion_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblNivelAutorizacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion("NIVELAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_NivelAutorizacion_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "NIVELAUTORIZACION", "NivelAutorizacion_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function NivelAutorizacion_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblNivelAutorizacion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("NIVELAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_NivelAutorizacion_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "NIVELAUTORIZACION", "NivelAutorizacion_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function NivelAutorizacion_Consultar(ByVal pintIdSucursal As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblNivelAutorizacion)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("NIVELAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblNivelAutorizacion)

            objRetorno = DbContext.usp_Maestros_NivelAutorizacion_Consultar(pintIdSucursal, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "NIVELAUTORIZACION", "NivelAutorizacion_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function NivelAutorizacion_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("NIVELAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_NivelAutorizacion_Eliminar(pintID, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "NIVELAUTORIZACION", "NivelAutorizacion_Eliminar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function NivelAutorizacion_Actualizar(ByVal pintID As Nullable(Of Integer),
                                                     ByVal pintidSucursal As Nullable(Of Integer),
                                                     ByVal pbitAplicaValorInversion As Nullable(Of Boolean),
                                                     ByVal pnumRangoInicial As Nullable(Of Decimal),
                                                     ByVal pnumRangoFinal As Nullable(Of Decimal),
                                                     ByVal pintNumNivelAutorizacion As Nullable(Of Integer),
                                                     ByVal plogSoloValidar As Nullable(Of Boolean),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strInfoSession As String = DemeInfoSesion("NIVELAUTORIZACION", pstrUsuario)
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)
            objRetorno = DbContext.usp_Maestros_NivelAutorizacion_Validar(pintID, pintidSucursal, pbitAplicaValorInversion, pnumRangoInicial, pnumRangoFinal,
                                                                           pintNumNivelAutorizacion, plogSoloValidar, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "NIVELAUTORIZACION", "NivelAutorizacion_Actualizar")
            Return Nothing
        End Try
    End Function


#End Region

End Class

