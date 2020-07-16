
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
Imports A2Utilidades.Utilidades
Imports A2.OyD.OYDServer.RIA.Web.OYDPLUSUtilidades
Imports System.Xml.Linq
Imports System.Web.UI.WebControls
Imports System.Text
Imports System.Globalization
Imports System.Threading
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

'TODO: Create methods containing your application logic.
<EnableClientAccess()> _
Public Class OYDPLUSUtilidadesDomainService
    Inherits LinqToSqlDomainService(Of OYDPLUSUtilidadesDataContext)
    Public Sub New() 'JBT20140328
        Me.DataContext.CommandTimeout = 0
    End Sub
    Dim RETORNO As Boolean
    Dim Usuario As String = ""

#Region "Comunes"


    ''' <summary>
    ''' Método para cargar los combos generales
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    <Query(HasSideEffects:=True)>
    Public Function cargarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDPLUSUtilidades.ItemComboOYDPLUS)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos("", "")
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Método para cargar combos específicos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function cargarCombosEspecificos(ByVal pstrListasCombos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OYDPLUSUtilidades.ItemComboOYDPLUS)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspA2utils_CargarCombos(pstrListasCombos, pstrUsuario)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarCombosEspecificos")
            Return Nothing
        End Try
    End Function
#End Region

#Region "OYDPLUS"

    Public Function OYDPLUS_ConsultarSaldoCliente(ByVal pstrCodigoOYD As String, ByVal pstrUsuario As String, ByVal pstrTipoProducto As String, ByVal pstrCarteraColectivaFondos As String, ByVal pintNroEncargoFondos As String, ByVal pstrInfoConexion As String) As List(Of tblSaldosCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarSaldosCliente(pstrCodigoOYD, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarSaldoCliente"), 0, pstrTipoProducto, pstrCarteraColectivaFondos, pintNroEncargoFondos)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarSaldoCliente")
            Return Nothing
        End Try
    End Function
    Public Function OYDPLUS_ConsultarSaldoBanco(ByVal pintBancoini As Integer, pintBancofin As Integer, pdtmFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblSaldosBancos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_rptSaldosBancos_PLUS(pintBancoini, pintBancofin, pdtmFecha, 1, pstrUsuario, Nothing, Nothing, Nothing)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarSaldoBanco")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarPortafolioCliente(ByVal pstrTipoNegocio As String, ByVal pstrCodigoOYD As String, ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblPortafolioCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarPortafolioCliente(pstrTipoNegocio, pstrCodigoOYD, pstrEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarPortafolioCliente"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarPortafolioCliente")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatetblPortafolioCliente(ByVal currentPortafolio As tblPortafolioCliente)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatetblPortafolioCliente")
        End Try
    End Sub
    'Consultar Liquidaciones para Complementacion Por Precio Promedio Carga MAsiva JDOL20170309
    Public Function OYDPLUS_ConsultarOrdenesBolsaAccionesPrecioPromedio(ByVal pstrOperacion As String, pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblOrdenesSAEAcciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarOrdenesBolsaReceptorAccionesPrecioPromedio(pstrOperacion, pstrEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrdenesBolsaAccionesPrecioPromedio"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarOrdenesBolsaAcciones")
            Return Nothing
        End Try
    End Function


    Public Function OYDPLUS_ConsultarOrdenesBolsaAcciones(ByVal pstrCodigoReceptor As String, ByVal pstrClaseOrden As String, ByVal pstrOperacion As String, ByVal pstrUsuario As String, ByVal pstrTipoNegocio As String, ByVal pstrLiquidacionesHabilitar As String, ByVal pstrInfoConexion As String) As List(Of tblOrdenesSAEAcciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarOrdenesBolsaReceptorAcciones(pstrCodigoReceptor, pstrClaseOrden, pstrOperacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrdenesBolsaAcciones"), 0, pstrTipoNegocio, pstrLiquidacionesHabilitar)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarOrdenesBolsaAcciones")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatetblOrdenesSAEAcciones(ByVal currentOrdenSAE As tblOrdenesSAEAcciones)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatetblOrdenesSAEAcciones")
        End Try
    End Sub

    Public Function OYDPLUS_ConsultarOrdenesBolsaRentaFija(ByVal pstrCodigoReceptor As String, ByVal pstrClaseOrden As String, ByVal pstrOperacion As String, ByVal pstrUsuario As String, ByVal pstrTipoNegocio As String, ByVal pstrLiquidacionesHabilitar As String, ByVal pstrInfoConexion As String) As List(Of tblOrdenesSAERentaFija)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarOrdenesBolsaReceptorRentaFija(pstrCodigoReceptor, pstrClaseOrden, pstrOperacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrdenesBolsaRentaFija"), 0, pstrTipoNegocio, pstrLiquidacionesHabilitar)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarOrdenesBolsaRentaFija")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatetblOrdenesSAERentaFija(ByVal currentOrdenSAE As tblOrdenesSAERentaFija)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatetblOrdenesSAERentaFija")
        End Try
    End Sub

    Public Function OYDPLUS_ConsultarOperacionesCumplir(ByVal pstrTipoNegocio As String, ByVal pstrTipoOperacion As String, ByVal pstrCodigoOYD As String, ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblOperacionesCumplir)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarOperacionesXCumplir(pstrTipoNegocio, pstrTipoOperacion, pstrCodigoOYD, pstrEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOperacionesCumplir"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarOperacionesCumplir")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatetblOperacionesCumplir(ByVal currentOrdenSAE As tblOperacionesCumplir)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatetblOperacionesCumplir")
        End Try
    End Sub

    Public Function OYDPLUS_ConsultarEstadosDocumento(ByVal pintIDNumeroUnico As Integer, ByVal pintIDDocumento As Integer, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRN_ContadorReglasAutorizacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarEstadosDocumento(pintIDNumeroUnico, pintIDDocumento, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarEstadosDocumento"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarEstadosDocumento")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatetblRN_ContadorReglasAutorizacion(ByVal currentEstado As tblRN_ContadorReglasAutorizacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatetblRN_ContadorReglasAutorizacion")
        End Try
    End Sub

    Public Function OYDPLUS_ConsultarLiquidacionesCliente(ByVal pstrCliente As String, ByVal pstrTipoOperacion As String, ByVal pdtmFechaInicial As DateTime, ByVal pdtmFechaFinal As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblLiquidacionesCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarLiquidacionesCliente(pstrCliente, pstrTipoOperacion, pdtmFechaInicial, pdtmFechaFinal, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarLiquidacionesCliente"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarLiquidacionesCliente")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatetblLiquidacionesCliente(ByVal currenrLiquidacion As tblLiquidacionesCliente)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatetblLiquidacionesCliente")
        End Try
    End Sub

    Public Sub InserttblControlProgramacion(ByVal objtblControlProgramacion As tblControlProgramacion)
    End Sub
    Public Sub UpdatetblControlProgramacion(ByVal objtblControlProgramacion As tblControlProgramacion)
    End Sub
    Public Sub DeletetblControlProgramacion(ByVal objtblControlProgramacion As tblControlProgramacion)
    End Sub

    Public Sub InserttblControlProgramacionFechas(ByVal objtblControlProgramacionFechas As tblControlProgramacionFechas)
    End Sub
    Public Sub UpdatetblControlProgramacionFechas(ByVal objtblControlProgramacionFechas As tblControlProgramacionFechas)
    End Sub
    Public Sub DeletetblControlProgramacionFechas(ByVal objtblControlProgramacionFechas As tblControlProgramacionFechas)
    End Sub

    Public Sub InserttblControlProgramacionGeneracionFechas(ByVal objtblControlProgramacionGeneracionFechas As tblControlProgramacionGeneracionFechas)
    End Sub
    Public Sub UpdatetblControlProgramacionGeneracionFechas(ByVal objtblControlProgramacionGeneracionFechas As tblControlProgramacionGeneracionFechas)
    End Sub
    Public Sub DeletetblControlProgramacionGeneracionFechas(ByVal objtblControlProgramacionGeneracionFechas As tblControlProgramacionGeneracionFechas)
    End Sub

    Public Sub InserttblControlProgramacionLog(ByVal objtblControlProgramacionLog As tblControlProgramacionLog)
    End Sub
    Public Sub UpdatetblControlProgramacionLog(ByVal objtblControlProgramacionLog As tblControlProgramacionLog)
    End Sub
    Public Sub DeletetblControlProgramacionLog(ByVal objtblControlProgramacionLog As tblControlProgramacionLog)
    End Sub

    Public Function ControlProgramacionesObtenerFechas(ByVal pstrTipoRecurrencia As String, _
                                                       ByVal plogDiariaCadaDia As Nullable(Of Boolean), _
                                                       ByVal pintDiariaDias As Nullable(Of Integer), _
                                                       ByVal pintSemanalNroSemanas As Nullable(Of Integer), _
                                                       ByVal pstrSemanalDiasSemana As String, _
                                                       ByVal plogMensualElDia As Nullable(Of Boolean), _
                                                       ByVal pintMensualCadaDias As Nullable(Of Integer),
                                                       ByVal pintMensualCadaMes As Nullable(Of Integer), _
                                                       ByVal pstrMensualDias As String, _
                                                       ByVal pstrMensualTipoDia As String, _
                                                       ByVal plogAnualElDia As Nullable(Of Boolean), _
                                                       ByVal pstrAnualMeses As String, _
                                                       ByVal pintAnualDia As Nullable(Of Integer), _
                                                       ByVal pstrAnualDias As String, _
                                                       ByVal pstrAnualTipoDia As String, _
                                                       ByVal pdtmFechaInicio As Nullable(Of DateTime), _
                                                       ByVal pstrModoFinalizacion As String, _
                                                       ByVal pintRepeticiones As Nullable(Of Integer), _
                                                       ByVal pdtmFechaFinalizacion As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlProgramacionGeneracionFechas)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ControlProgramacion_ObtenerFechas(pstrTipoRecurrencia, plogDiariaCadaDia, pintDiariaDias, pintSemanalNroSemanas, pstrSemanalDiasSemana, plogMensualElDia, pintMensualCadaDias, pintMensualCadaMes, pstrMensualDias, pstrMensualTipoDia, plogAnualElDia, pstrAnualMeses, pintAnualDia, pstrAnualDias, pstrAnualTipoDia, pdtmFechaInicio, pstrModoFinalizacion, pintRepeticiones, pdtmFechaFinalizacion)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlProgramacionesObtenerFechas")
            Return Nothing
        End Try
    End Function
    Public Function ControlProgramacionesObtenerFechasSync(ByVal pstrTipoRecurrencia As String, _
                                                       ByVal plogDiariaCadaDia As Nullable(Of Boolean), _
                                                       ByVal pintDiariaDias As Nullable(Of Integer), _
                                                       ByVal pintSemanalNroSemanas As Nullable(Of Integer), _
                                                       ByVal pstrSemanalDiasSemana As String, _
                                                       ByVal plogMensualElDia As Nullable(Of Boolean), _
                                                       ByVal pintMensualCadaDias As Nullable(Of Integer),
                                                       ByVal pintMensualCadaMes As Nullable(Of Integer), _
                                                       ByVal pstrMensualDias As String, _
                                                       ByVal pstrMensualTipoDia As String, _
                                                       ByVal plogAnualElDia As Nullable(Of Boolean), _
                                                       ByVal pstrAnualMeses As String, _
                                                       ByVal pintAnualDia As Nullable(Of Integer), _
                                                       ByVal pstrAnualDias As String, _
                                                       ByVal pstrAnualTipoDia As String, _
                                                       ByVal pdtmFechaInicio As Nullable(Of DateTime), _
                                                       ByVal pstrModoFinalizacion As String, _
                                                       ByVal pintRepeticiones As Nullable(Of Integer), _
                                                       ByVal pdtmFechaFinalizacion As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlProgramacionGeneracionFechas)
        Dim objTask As Task(Of List(Of tblControlProgramacionGeneracionFechas)) = Me.ControlProgramacionesObtenerFechasAsync(pstrTipoRecurrencia, plogDiariaCadaDia, pintDiariaDias, pintSemanalNroSemanas, pstrSemanalDiasSemana, plogMensualElDia, pintMensualCadaDias, pintMensualCadaMes, pstrMensualDias, pstrMensualTipoDia, plogAnualElDia, pstrAnualMeses, pintAnualDia, pstrAnualDias, pstrAnualTipoDia, pdtmFechaInicio, pstrModoFinalizacion, pintRepeticiones, pdtmFechaFinalizacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlProgramacionesObtenerFechasAsync(ByVal pstrTipoRecurrencia As String, _
                                                       ByVal plogDiariaCadaDia As Nullable(Of Boolean), _
                                                       ByVal pintDiariaDias As Nullable(Of Integer), _
                                                       ByVal pintSemanalNroSemanas As Nullable(Of Integer), _
                                                       ByVal pstrSemanalDiasSemana As String, _
                                                       ByVal plogMensualElDia As Nullable(Of Boolean), _
                                                       ByVal pintMensualCadaDias As Nullable(Of Integer),
                                                       ByVal pintMensualCadaMes As Nullable(Of Integer), _
                                                       ByVal pstrMensualDias As String, _
                                                       ByVal pstrMensualTipoDia As String, _
                                                       ByVal plogAnualElDia As Nullable(Of Boolean), _
                                                       ByVal pstrAnualMeses As String, _
                                                       ByVal pintAnualDia As Nullable(Of Integer), _
                                                       ByVal pstrAnualDias As String, _
                                                       ByVal pstrAnualTipoDia As String, _
                                                       ByVal pdtmFechaInicio As Nullable(Of DateTime), _
                                                       ByVal pstrModoFinalizacion As String, _
                                                       ByVal pintRepeticiones As Nullable(Of Integer), _
                                                       ByVal pdtmFechaFinalizacion As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of tblControlProgramacionGeneracionFechas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblControlProgramacionGeneracionFechas)) = New TaskCompletionSource(Of List(Of tblControlProgramacionGeneracionFechas))()
        objTaskComplete.TrySetResult(ControlProgramacionesObtenerFechas(pstrTipoRecurrencia, plogDiariaCadaDia, pintDiariaDias, pintSemanalNroSemanas, pstrSemanalDiasSemana, plogMensualElDia, pintMensualCadaDias, pintMensualCadaMes, pstrMensualDias, pstrMensualTipoDia, plogAnualElDia, pstrAnualMeses, pintAnualDia, pstrAnualDias, pstrAnualTipoDia, pdtmFechaInicio, pstrModoFinalizacion, pintRepeticiones, pdtmFechaFinalizacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlProgramacionesActualizar(ByVal pstrModulo As String, _
                                                    ByVal pintNroDocumento As Integer, _
                                                    ByVal pstrTipoRecurrencia As String, _
                                                    ByVal plogDiariaCadaDia As Nullable(Of Boolean), _
                                                    ByVal pintDiariaDias As Nullable(Of Integer), _
                                                    ByVal pintSemanalNroSemanas As Nullable(Of Integer), _
                                                    ByVal pstrSemanalDiasSemana As String, _
                                                    ByVal plogMensualElDia As Nullable(Of Boolean), _
                                                    ByVal pintMensualCadaDias As Nullable(Of Integer),
                                                    ByVal pintMensualCadaMes As Nullable(Of Integer), _
                                                    ByVal pstrMensualDias As String, _
                                                    ByVal pstrMensualTipoDia As String, _
                                                    ByVal plogAnualElDia As Nullable(Of Boolean), _
                                                    ByVal pstrAnualMeses As String, _
                                                    ByVal pintAnualDia As Nullable(Of Integer), _
                                                    ByVal pstrAnualDias As String, _
                                                    ByVal pstrAnualTipoDia As String, _
                                                    ByVal pdtmFechaInicio As Nullable(Of DateTime), _
                                                    ByVal pstrModoFinalizacion As String, _
                                                    ByVal pintRepeticiones As Nullable(Of Integer), _
                                                    ByVal pdtmFechaFinalizacion As Nullable(Of DateTime), _
                                                    ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_ControlProgramacion_Actualizar(pstrModulo, pintNroDocumento, pstrTipoRecurrencia, plogDiariaCadaDia, pintDiariaDias, pintSemanalNroSemanas, pstrSemanalDiasSemana, plogMensualElDia, pintMensualCadaDias, pintMensualCadaMes, pstrMensualDias, pstrMensualTipoDia, plogAnualElDia, pstrAnualMeses, pintAnualDia, pstrAnualDias, pstrAnualTipoDia, pdtmFechaInicio, pstrModoFinalizacion, pintRepeticiones, pdtmFechaFinalizacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlProgramacionesActualizar"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlProgramacionesActualizar")
            Return False
        End Try
    End Function
    Public Function ControlProgramacionesActualizarSync(ByVal pstrModulo As String, _
                                                    ByVal pintNroDocumento As Integer, _
                                                    ByVal pstrTipoRecurrencia As String, _
                                                    ByVal plogDiariaCadaDia As Nullable(Of Boolean), _
                                                    ByVal pintDiariaDias As Nullable(Of Integer), _
                                                    ByVal pintSemanalNroSemanas As Nullable(Of Integer), _
                                                    ByVal pstrSemanalDiasSemana As String, _
                                                    ByVal plogMensualElDia As Nullable(Of Boolean), _
                                                    ByVal pintMensualCadaDias As Nullable(Of Integer),
                                                    ByVal pintMensualCadaMes As Nullable(Of Integer), _
                                                    ByVal pstrMensualDias As String, _
                                                    ByVal pstrMensualTipoDia As String, _
                                                    ByVal plogAnualElDia As Nullable(Of Boolean), _
                                                    ByVal pstrAnualMeses As String, _
                                                    ByVal pintAnualDia As Nullable(Of Integer), _
                                                    ByVal pstrAnualDias As String, _
                                                    ByVal pstrAnualTipoDia As String, _
                                                    ByVal pdtmFechaInicio As Nullable(Of DateTime), _
                                                    ByVal pstrModoFinalizacion As String, _
                                                    ByVal pintRepeticiones As Nullable(Of Integer), _
                                                    ByVal pdtmFechaFinalizacion As Nullable(Of DateTime), _
                                                    ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Dim objTask As Task(Of Boolean) = Me.ControlProgramacionesActualizarAsync(pstrModulo, pintNroDocumento, pstrTipoRecurrencia, plogDiariaCadaDia, pintDiariaDias, pintSemanalNroSemanas, pstrSemanalDiasSemana, plogMensualElDia, pintMensualCadaDias, pintMensualCadaMes, pstrMensualDias, pstrMensualTipoDia, plogAnualElDia, pstrAnualMeses, pintAnualDia, pstrAnualDias, pstrAnualTipoDia, pdtmFechaInicio, pstrModoFinalizacion, pintRepeticiones, pdtmFechaFinalizacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlProgramacionesActualizarAsync(ByVal pstrModulo As String, _
                                                    ByVal pintNroDocumento As Integer, _
                                                    ByVal pstrTipoRecurrencia As String, _
                                                    ByVal plogDiariaCadaDia As Nullable(Of Boolean), _
                                                    ByVal pintDiariaDias As Nullable(Of Integer), _
                                                    ByVal pintSemanalNroSemanas As Nullable(Of Integer), _
                                                    ByVal pstrSemanalDiasSemana As String, _
                                                    ByVal plogMensualElDia As Nullable(Of Boolean), _
                                                    ByVal pintMensualCadaDias As Nullable(Of Integer),
                                                    ByVal pintMensualCadaMes As Nullable(Of Integer), _
                                                    ByVal pstrMensualDias As String, _
                                                    ByVal pstrMensualTipoDia As String, _
                                                    ByVal plogAnualElDia As Nullable(Of Boolean), _
                                                    ByVal pstrAnualMeses As String, _
                                                    ByVal pintAnualDia As Nullable(Of Integer), _
                                                    ByVal pstrAnualDias As String, _
                                                    ByVal pstrAnualTipoDia As String, _
                                                    ByVal pdtmFechaInicio As Nullable(Of DateTime), _
                                                    ByVal pstrModoFinalizacion As String, _
                                                    ByVal pintRepeticiones As Nullable(Of Integer), _
                                                    ByVal pdtmFechaFinalizacion As Nullable(Of DateTime), _
                                                    ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)
        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(ControlProgramacionesActualizar(pstrModulo, pintNroDocumento, pstrTipoRecurrencia, plogDiariaCadaDia, pintDiariaDias, pintSemanalNroSemanas, pstrSemanalDiasSemana, plogMensualElDia, pintMensualCadaDias, pintMensualCadaMes, pstrMensualDias, pstrMensualTipoDia, plogAnualElDia, pstrAnualMeses, pintAnualDia, pstrAnualDias, pstrAnualTipoDia, pdtmFechaInicio, pstrModoFinalizacion, pintRepeticiones, pdtmFechaFinalizacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlProgramacionesConsultar(ByVal pstrModulo As String, ByVal pintNroDocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlProgramacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ControlProgramacion_Consultar(pstrModulo, pintNroDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlProgramacionesConsultar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlProgramacionesConsultar")
            Return Nothing
        End Try
    End Function
    Public Function ControlProgramacionesConsultarSync(ByVal pstrModulo As String, ByVal pintNroDocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlProgramacion)
        Dim objTask As Task(Of List(Of tblControlProgramacion)) = Me.ControlProgramacionesConsultarAsync(pstrModulo, pintNroDocumento, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlProgramacionesConsultarAsync(ByVal pstrModulo As String, ByVal pintNroDocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of tblControlProgramacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblControlProgramacion)) = New TaskCompletionSource(Of List(Of tblControlProgramacion))()
        objTaskComplete.TrySetResult(ControlProgramacionesConsultar(pstrModulo, pintNroDocumento, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlProgramacionesConsultarFechas(ByVal pintIDControlProgramacion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlProgramacionFechas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ControlProgramacion_ConsultarFechas(pintIDControlProgramacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlProgramacionesConsultarFechas"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlProgramacionesConsultarFechas")
            Return Nothing
        End Try
    End Function
    Public Function ControlProgramacionesConsultarFechasSync(ByVal pintIDControlProgramacion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlProgramacionFechas)
        Dim objTask As Task(Of List(Of tblControlProgramacionFechas)) = Me.ControlProgramacionesConsultarFechasAsync(pintIDControlProgramacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlProgramacionesConsultarFechasAsync(ByVal pintIDControlProgramacion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of tblControlProgramacionFechas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblControlProgramacionFechas)) = New TaskCompletionSource(Of List(Of tblControlProgramacionFechas))()
        objTaskComplete.TrySetResult(ControlProgramacionesConsultarFechas(pintIDControlProgramacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlProgramacionesConsultarLog(ByVal pintIDFecha As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlProgramacionLog)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ControlProgramacion_ConsultarLog(pintIDFecha, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlProgramacionesConsultarLog"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlProgramacionesConsultarLog")
            Return Nothing
        End Try
    End Function
    Public Function ControlProgramacionesConsultarLogSync(ByVal pintIDFecha As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlProgramacionLog)
        Dim objTask As Task(Of List(Of tblControlProgramacionLog)) = Me.ControlProgramacionesConsultarLogAsync(pintIDFecha, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlProgramacionesConsultarLogAsync(ByVal pintIDFecha As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of tblControlProgramacionLog))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tblControlProgramacionLog)) = New TaskCompletionSource(Of List(Of tblControlProgramacionLog))()
        objTaskComplete.TrySetResult(ControlProgramacionesConsultarLog(pintIDFecha, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlProgramacionesInactivar(ByVal pstrModulo As String, ByVal pintNroDocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_ControlProgramacion_Inactivar(pstrModulo, pintNroDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlProgramacionesInactivar"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlProgramacionesInactivar")
            Return False
        End Try
    End Function
    Public Function ControlProgramacionesInactivarSync(ByVal pstrModulo As String, ByVal pintNroDocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Dim objTask As Task(Of Boolean) = Me.ControlProgramacionesInactivarAsync(pstrModulo, pintNroDocumento, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlProgramacionesInactivarAsync(ByVal pstrModulo As String, ByVal pintNroDocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)
        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(ControlProgramacionesInactivar(pstrModulo, pintNroDocumento, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ControlProgramacionesInactivarFechas(ByVal pstrIDFechas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_ControlProgramacion_InactivarFechas(pstrIDFechas, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlProgramacionesInactivarFechas"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlProgramacionesInactivarFechas")
            Return False
        End Try
    End Function
    Public Function ControlProgramacionesInactivarFechasSync(ByVal pstrIDFechas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Dim objTask As Task(Of Boolean) = Me.ControlProgramacionesInactivarFechasAsync(pstrIDFechas, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ControlProgramacionesInactivarFechasAsync(ByVal pstrIDFechas As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)
        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(ControlProgramacionesInactivarFechas(pstrIDFechas, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function OYDPLUS_ConsultarMensajePantalla(ByVal pstrOpcion As String, ByVal pstrIDReceptor As String, ByVal pstrIDCliente As String, ByVal pstrIDEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblMensajes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargarMensajeDinamico(pstrOpcion, pstrIDReceptor, pstrIDCliente, pstrIDEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarMensajePantalla"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarMensajePantalla")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarConfiguracionReceptor(ByVal pstrCodigoReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblConfiguracionesAdicionalesReceptor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargarConfiguracionReceptor(pstrCodigoReceptor, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarConfiguracionReceptor"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarConfiguracionReceptor")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarParametrosReceptor(ByVal pstrCodigoReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblParametrosReceptor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargarParametrosReceptor(pstrCodigoReceptor, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarConfiguracionReceptor"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarConfiguracionReceptor")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarReceptoresUsuario(ByVal plogCargarReceptoresActivos As Boolean, ByVal pstrUsuarioConsulta As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblReceptoresUsuario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargarReceptoresUsuario(plogCargarReceptoresActivos, pstrUsuarioConsulta, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarReceptoresUsuario"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarReceptoresUsuario")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarCombosReceptor(ByVal pstrReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CombosReceptor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargarCombosReceptor(pstrReceptor, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarCombosReceptor"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarCombosReceptor")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarEspeciesTipoNegocio(ByVal pstrTipoNegocio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblEspeciesXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargarEspeciesTipoNegocio(pstrTipoNegocio, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarEspeciesTipoNegocio"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarEspeciesTipoNegocio")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_TipoNegocioReceptor(ByVal pstrReceptor As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblTipoNegocioReceptor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargarTipoNegocioReceptor(pstrReceptor, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_TipoNegocioReceptor"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_TipoNegocioReceptor")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarCombosOtroSistema(ByVal pstrOpcionCargar As String, ByVal pstrSistemaOrigen As String, ByVal pstrSistemaDestino As String, ByVal pstrAccion As String, ByVal pstrIDRegistro As String, ByVal pstrInformacionAdicional As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ItemCombosSistemaExterno)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrInformacionAdicional = RetornarValorDescodificado(pstrInformacionAdicional)
            'Envio el mensaje y espero la respuesta
            Dim strResultadoXML As String = A2SBMessages.EnviarMensajeConRespuesta(Of String)(pstrSistemaOrigen, pstrSistemaDestino, pstrAccion, pstrIDRegistro, pstrInformacionAdicional)
            'retorno la respuesta xml en una entidad
            Dim strRespuestaXML As String = A2SBMessages.FormatearMensaje(strResultadoXML)
            If Not String.IsNullOrEmpty(strRespuestaXML) Then
                Dim ret = Me.DataContext.RetornarInformacionCombosSistemaExterno(pstrOpcionCargar, strRespuestaXML).ToList()
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarCombosOtroSistema")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Validar Eliminar Registro"

    Public Function ValidarEliminarRegistro(ByVal pstrTablas As String, ByVal pstrCampos As String, ByVal pstrValorCampo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidacionEliminarRegistro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ValidarEliminarRegistro(pstrTablas, pstrCampos, pstrValorCampo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarEliminarRegistro"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarEliminarRegistro")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Validar Duplicidad Registro"

    Public Function ValidarDuplicidadRegistro(ByVal pstrTablas As String, ByVal pstrCampos As String, ByVal pstrValorCampo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidacionEliminarRegistro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ValidarDuplicidadRegistro(pstrTablas, pstrCampos, pstrValorCampo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarDuplicidadRegistro"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarDuplicidadRegistro")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Obtener Fecha Servidor"

    Public Function ObtenerFechaServidor(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DateTime
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim dtmFechaServidor As DateTime?
            Me.DataContext.uspOyDNet_ObtenerFechaServidor(dtmFechaServidor, pstrUsuario, DemeInfoSesion(pstrUsuario, "ObtenerFechaServidor"), 0)
            Return CDate(dtmFechaServidor)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerFechaServidor")
            Return Nothing
        End Try
    End Function

    Public Function ObtenerFechaServidorSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DateTime
        Dim objTask As Task(Of DateTime) = Me.ObtenerFechaServidorAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ObtenerFechaServidorAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of DateTime)
        Dim objTaskComplete As TaskCompletionSource(Of DateTime) = New TaskCompletionSource(Of DateTime)()
        objTaskComplete.TrySetResult(ObtenerFechaServidor(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region
End Class

