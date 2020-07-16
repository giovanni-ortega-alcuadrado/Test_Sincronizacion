
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On
Imports A2.OyD.OYDServer.RIA.Web.OyDYankees
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
Imports System.Data.SqlClient
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura


'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class YankeesDomainService
    Inherits LinqToSqlDomainService(Of OYD_YankeesDataContext)
    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Liquidaciones Yankees"

    Public Function LiquidacionesConsultarYankees(ByVal plngID As Integer, ByVal pdtmRegistro As System.Nullable(Of DateTime), ByVal pdtmCumplimiento As System.Nullable(Of DateTime), ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblLiquidaciones_YANKEE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarLiquidacionesYankees(plngID, pdtmRegistro, pdtmCumplimiento, plngIDComitente, DemeInfoSesion(pstrUsuario, "BuscarLiquidacionesConsultarYankees"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarLiquidacionesConsultarYankees")
            Return Nothing
        End Try
    End Function
    Public Function LiquidacionesFiltrarYankees(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblLiquidaciones_YANKEE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FiltrarLiquidacionesYankees(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "LiquidacionesFiltrarYankees"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionesFiltrarYankees")
            Return Nothing
        End Try
    End Function
    Public Sub InsertLiquidacione_Yankees(ByVal LiquidacioneYankees As tblLiquidaciones_YANKEE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,LiquidacioneYankees.pstrUsuarioConexion, LiquidacioneYankees.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            LiquidacioneYankees.InfoSesion = DemeInfoSesion(LiquidacioneYankees.pstrUsuarioConexion, "InsertLiquidacione_Yankees")
            Me.DataContext.tblLiquidaciones_YANKEEs.InsertOnSubmit(LiquidacioneYankees)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLiquidacione_Yankees")
        End Try
    End Sub
    Public Sub UpdateLiquidacione_Yankees(ByVal currentLiquidacioneYankees As tblLiquidaciones_YANKEE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentLiquidacioneYankees.pstrUsuarioConexion, currentLiquidacioneYankees.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentLiquidacioneYankees.InfoSesion = DemeInfoSesion(currentLiquidacioneYankees.pstrUsuarioConexion, "UpdateLiquidacione_Yankees")
            Me.DataContext.tblLiquidaciones_YANKEEs.Attach(currentLiquidacioneYankees, Me.ChangeSet.GetOriginal(currentLiquidacioneYankees))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLiquidacione_Yankees")
        End Try
    End Sub
    Public Function LiquidacionesConsultarYankeesContraparte(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblDetalleLiquida_YANKEE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarLiquidacionesYankeesContraparte(plngID, DemeInfoSesion(pstrUsuario, "LiquidacionesConsultarYankeesContraparte"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionesConsultarYankeesContraparte")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función para consultar los receptores asociados al comitente.
    ''' </summary>
    ''' <param name="IDY"></param>
    ''' <param name="pstrIDComitente"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una Lista de ReceptoresYankees</returns>
    ''' <remarks>SLB20130910</remarks>
    Public Function ReceptoresPorClientes_Consultar(ByVal IDY As Integer, ByVal pstrIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objListaReceptores As New List(Of ReceptoresYankees)
            Dim ret = Me.DataContext.uspOyDNet_Ordenes_ReceptoresCliente_Consultar(String.Empty, String.Empty, pstrIDComitente, pstrUsuario,
                                                                               DemeInfoSesion(pstrUsuario, "ReceptoresPorClientes_Consultar"), 0).ToList

            If ret.Count > 0 Then
                For Each objLista In ret
                    objListaReceptores.Add(New ReceptoresYankees With {.IDY = IDY, .IDReceptor = objLista.IDReceptor, .Lider = objLista.Lider,
                                                                       .Porcentaje = CDbl(IIf(objLista.Lider, 100.0, 0.0)), .Usuario = "", .IDReceptoresYankees = CInt(objLista.IDReceptorOrden),
                                                                       .InfoSesion = objLista.InfoSesion, .Nombre = objLista.Nombre})

                Next
            End If

            Return objListaReceptores
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoresPorClientes_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function ReceptoresYankeesConsultar(ByVal plngIDOperacion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_LiquidacionesReceYankees(plngIDOperacion, DemeInfoSesion(pstrUsuario, "ReceptoresYankeesConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoresYankeesConsultar")
            Return Nothing
        End Try
    End Function
    Public Function ConsultaCalculoEspecie(ByVal pstrIDEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.Usp_ConsBaseCalculoEspecies_LiquidacionesYankees_OyDNet(pstrIDEspecie).ToList
            If ret.Count = 0 Then
                Return String.Empty
            Else
                Return ret.FirstOrDefault.strDescripcion
            End If
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoresYankeesConsultar")
            Return Nothing
        End Try
    End Function
    Public Sub InsertReceptore_Yankees(ByVal ReceptoreYankees As ReceptoresYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoreYankees.pstrUsuarioConexion, ReceptoreYankees.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoreYankees.InfoSesion = DemeInfoSesion(ReceptoreYankees.pstrUsuarioConexion, "ReceptoreYankees")
            Me.DataContext.ReceptoresYankees.InsertOnSubmit(ReceptoreYankees)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoreYankees")
        End Try
    End Sub
    Public Sub UpdateReceptore_Yankees(ByVal currentReceptoreYankees As ReceptoresYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentReceptoreYankees.pstrUsuarioConexion, currentReceptoreYankees.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentReceptoreYankees.InfoSesion = DemeInfoSesion(currentReceptoreYankees.pstrUsuarioConexion, "UpdateReceptore_Yankees")
            Me.DataContext.ReceptoresYankees.Attach(currentReceptoreYankees, Me.ChangeSet.GetOriginal(currentReceptoreYankees))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptore_Yankees")
        End Try
    End Sub
    Public Sub DeleteReceptore_Yankees(ByVal ReceptoresYankee As ReceptoresYankees)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresYankee.pstrUsuarioConexion, ReceptoresYankee.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Eliminar( pID,  pIDComitente, DemeInfoSesion(pstrUsuario, "DeleteLiquidacione"),0).ToList
            ReceptoresYankee.InfoSesion = DemeInfoSesion(ReceptoresYankee.pstrUsuarioConexion, "DeleteReceptore_Yankees")
            Me.DataContext.ReceptoresYankees.Attach(ReceptoresYankee)
            Me.DataContext.ReceptoresYankees.DeleteOnSubmit(ReceptoresYankee)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptore_Yankees")
        End Try
    End Sub
    Public Sub InsertContraparte_Yankees(ByVal ContraparteYankees As tblDetalleLiquida_YANKEE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ContraparteYankees.pstrUsuarioConexion, ContraparteYankees.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ContraparteYankees.InfoSesion = DemeInfoSesion(ContraparteYankees.pstrUsuarioConexion, "InsertContraparte_Yankees")
            Me.DataContext.tblDetalleLiquida_YANKEEs.InsertOnSubmit(ContraparteYankees)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertContraparte_Yankees")
        End Try
    End Sub
    Public Sub UpdateContraparte_Yankees(ByVal currentContraparteYankees As tblDetalleLiquida_YANKEE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentContraparteYankees.pstrUsuarioConexion, currentContraparteYankees.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentContraparteYankees.InfoSesion = DemeInfoSesion(currentContraparteYankees.pstrUsuarioConexion, "UpdateContraparte_Yankees")
            Me.DataContext.tblDetalleLiquida_YANKEEs.Attach(currentContraparteYankees, Me.ChangeSet.GetOriginal(currentContraparteYankees))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateContraparte_Yankees")
        End Try
    End Sub
    Public Sub DeleteContraparte_Yankees(ByVal ContraparteYankee As tblDetalleLiquida_YANKEE)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ContraparteYankee.pstrUsuarioConexion, ContraparteYankee.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Eliminar( pID,  pIDComitente, DemeInfoSesion(pstrUsuario, "DeleteLiquidacione"),0).ToList
            ContraparteYankee.InfoSesion = DemeInfoSesion(ContraparteYankee.pstrUsuarioConexion, "DeleteContraparte_Yankees")
            Me.DataContext.tblDetalleLiquida_YANKEEs.Attach(ContraparteYankee)
            Me.DataContext.tblDetalleLiquida_YANKEEs.DeleteOnSubmit(ContraparteYankee)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteContraparte_Yankees")
        End Try
    End Sub


    Public Function LiquidacionesEnviarAdmon(ByVal pdtmFechaIni As DateTime, ByVal pdtmFechaFin As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidaciones_Yankee_Admon)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesEnviarAdmon_OyDNet(pdtmFechaIni, pdtmFechaFin, DemeInfoSesion(pstrUsuario, "LiquidacionesEnviarAdmon"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionesEnviarAdmon")
            Return Nothing
        End Try
    End Function

    Public Function EnviarLiquidacionesCrear(plngIdRecibo As Integer, plngIdComitente As String, pstrTipoIdentificacion As String, plngNroDocumento As Decimal, pstrNombre As String,
            pstrTelefono1 As String, pstrDireccion As String, pdtmRecibo As Date?, pstrEstado As String, pdtmEstado As Date?, pstrConceptoAnulacion As String, pstrNotas As String,
            pdtmActualizacion As Date?, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim resp As Integer
            resp = Me.DataContext.spInsCustodiasOyDNet(plngIdRecibo, plngIdComitente, pstrTipoIdentificacion, plngNroDocumento, pstrNombre, pstrTelefono1, pstrDireccion,
                    pdtmRecibo, pstrEstado, pdtmEstado, pstrConceptoAnulacion, pstrNotas, pdtmActualizacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "EnviarLiquidacionesCrear"), 0)
            Return resp
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EnviarLiquidacionesCrear")
            Return -1
        End Try
    End Function

    Public Function IngresarDetalleCustodiaYankee(plngIdRecibo As Integer, plngidSecuencia As Integer, plngIdComitente As String, pstrIdEspecie As String, pstrNroTitulo As String,
            pstrTipoIdentificacion As String, pstrModalidad As String, pdtmEmision As Date, pdtmVencimiento As Date, pdblCantidad As Double, plngNroDocumento As Integer, pstrISIN As String,
            pstrUsuario As String, plngIDDepositoExtranjero As Integer, pcurTotalLiq As Double, pdtmCumplimientoTitulo As Date, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim resp As Integer
            resp = Me.DataContext.spInsDetalleCustodiasOyDNet(plngIdRecibo, plngidSecuencia, plngIdComitente, pstrIdEspecie, pstrNroTitulo, False, Nothing, 0, 0, pstrModalidad,
                pdtmEmision, pdtmVencimiento, pdblCantidad, "X", 0, Nothing, Nothing, 0, 0, 0, 0, String.Empty, True, True, True, True, True, String.Empty, Nothing, pstrUsuario,
                Nothing, Nothing, pstrISIN, Nothing, Nothing, Nothing, plngIDDepositoExtranjero, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                pcurTotalLiq, Nothing, pdtmCumplimientoTitulo, Nothing, DemeInfoSesion(pstrUsuario, "IngresarDetalleCustodiaYankee"), 0)
            Return resp
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "IngresarDetalleCustodiaYankee")
            Return -1
        End Try
    End Function

    Public Function Custodia_Beneficiario_Yankee_Modificar(plngidSecuencia As Integer, plngIDNroCustodia As Integer, plngID As Integer, plngIdComitente As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim resp As Integer
            resp = Me.DataContext.uspOyDNet_Custodia_Beneficiario_Yankee(plngidSecuencia, plngIDNroCustodia, plngID, plngIdComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "Custodia_Beneficiario_Yankee_Modificar"), 0)
            Return resp
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Custodia_Beneficiario_Yankee_Modificar")
            Return -1
        End Try
    End Function


#End Region

#Region "FacturasYankees"

    Public Function FacturasFiltrarYankees(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of FacturaYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FacturasYankees_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "FacturasFiltrarYankees"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FacturasFiltrarYankees")
            Return Nothing
        End Try
    End Function

    Public Function FacturasConsultarYankees(ByVal pNumero As Integer, ByVal pComitente As String, ByVal dtmDocumento As DateTime?,
                                      ByVal pstrNombreComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of FacturaYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FacturasYankees_Consultar(pNumero, pComitente, dtmDocumento, pstrNombreComitente, DemeInfoSesion(pstrUsuario, "BuscarFacturasYankees"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarFacturasYankees")
            Return Nothing
        End Try
    End Function

    Public Function TraerFacturaPorDefectoYankees(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As FacturaYankees
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New FacturaYankees
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Prefijo = 
            'e.Numero = 
            'e.Comitente = 
            'e.Documento = 
            'e.Estado = 
            'e.Estado = 
            'e.Impresiones = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDCodigoResolucion = 
            'e.IDfacturas = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerFacturaPorDefectoYankees")
            Return Nothing
        End Try
    End Function

    Public Sub InsertFacturaYankees(ByVal FacturaYankees As FacturaYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,FacturaYankees.pstrUsuarioConexion, FacturaYankees.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            FacturaYankees.InfoSesion = DemeInfoSesion(FacturaYankees.pstrUsuarioConexion, "InsertFacturaYankees")
            Me.DataContext.FacturasYankees.InsertOnSubmit(FacturaYankees)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertFactura")
        End Try
    End Sub

    Public Sub UpdateFacturaYankees(ByVal currentFacturaYankees As FacturaYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentFacturaYankees.pstrUsuarioConexion, currentFacturaYankees.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentFacturaYankees.InfoSesion = DemeInfoSesion(currentFacturaYankees.pstrUsuarioConexion, "UpdateFacturaYankees")
            Me.DataContext.FacturasYankees.Attach(currentFacturaYankees, Me.ChangeSet.GetOriginal(currentFacturaYankees))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateFacturaYankees")
        End Try
    End Sub
    Public Function Traer_Liquidaciones_FacturaYankees(ByVal pID As Integer, ByVal pstrPrefijo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblLiquidaciones_YANKEE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pID) Then
                Dim ret = Me.DataContext.uspOyDNet_Liquidaciones_FacturaYankees_Consultar(pID, pstrPrefijo, DemeInfoSesion(pstrUsuario, "Traer_Liquidaciones_FacturaYankees"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_Liquidaciones_FacturaYankees")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Funcion que consulta el Total de las Facturas seleccionada (A Favor o A Cargo)
    ''' </summary>
    ''' <param name="lngIDFactura"></param>
    ''' <param name="strPrefijo"></param>
    ''' <param name="curTotalFactura"></param>
    ''' <returns></returns>
    ''' <remarks>JBT20130618</remarks>
    Public Function Consultar_TotalFacturaYankees(ByVal lngIDFactura As Integer, ByVal strPrefijo As String, ByVal curTotalFactura? As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FacturaYankees_TotalFactura(lngIDFactura, strPrefijo, curTotalFactura, DemeInfoSesion(pstrUsuario, "Consultar_TotalFacturaYankees"), 0)
            Dim varible = curTotalFactura
            Return CDbl(varible)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_TotalFacturaYankees")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Funcion para Anular las Facturas
    ''' </summary>
    ''' <param name="objFactura"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>JBT20130618</remarks>
    Public Function AnularFacturasYankees(ByVal IDFacturas As Integer, ByVal Numero As Integer, ByVal Prefijo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FacturasYankees_Eliminar(IDFacturas, Numero, Prefijo, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularFacturasYankees"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularFacturasYankees")
            Return False
        End Try
    End Function

#End Region

#Region "Confirmación Operación Yankees"

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Método encargado de recibir el html para conformar las cartas de confirmación operación Yankees.
    ''' Fecha            : Junio 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 17/2013 - Resultado Ok 
    ''' </history>
    Public Function PlantillaConfirmacionOperacionYankees(ByVal pIdInicial As Integer, ByVal pIdFinal As Integer _
                                                     , ByVal pFechaInicial As DateTime, ByVal pFechaFinal As DateTime _
                                                     , ByVal pstrTipoOperacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrHTMLPlantillaYankees As String = String.Empty
            Dim ret = Me.DataContext.UspConfirmacionOperacion(pIdInicial, pIdFinal, pFechaInicial,
                                            pFechaFinal, pstrTipoOperacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConstanciaOperacion_OTC"), 0, pstrHTMLPlantillaYankees)
            Return pstrHTMLPlantillaYankees
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConstanciaOperacion_OTC")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Certificado Operación Yankees"

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Método encargado de recibir el html para conformar los certificados de operación Yankees.
    ''' Fecha            : Junio 20/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 20/2013 - Resultado Ok 
    ''' </history>
    Public Function PlantillaCertificadoCustodiaYankees(ByVal pIdInicial As Integer, ByVal pIdFinal As Integer _
                                                     , ByVal pFechaInicial As DateTime, ByVal pFechaFinal As DateTime _
                                                     , ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrHTMLPlantillaYankees As String = String.Empty
            Dim ret = Me.DataContext.uspOyDNet_CertificadoCustodiaYankees(pIdInicial, pIdFinal, pFechaInicial,
                                            pFechaFinal, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConstanciaOperacion_OTC"), 0, pstrHTMLPlantillaYankees)
            Return pstrHTMLPlantillaYankees
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConstanciaOperacion_OTC")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Notas Yankees"

    Public Function NotasYankeesFiltrar(pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of NotasYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_NotasYankees_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "NotasYankeesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "NotasYankeesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function NotasYankeesFiltrarDetalle(pstrNombreConsecutivo As String, plngIDDocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of NotasYankeesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_NotasYankeesDetalle_Filtrar(pstrNombreConsecutivo, plngIDDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "NotasYankeesFiltrarDetalle"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "NotasYankeesFiltrarDetalle")
            Return Nothing
        End Try
    End Function
    Public Sub InsertNotasYankees(ByVal obj As OyDYankees.NotasYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.ActualizarNotas(CType(Nothing, String))

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertNotasYankees")
        End Try
    End Sub
    Public Sub UpdateNotasYankees(ByVal obj As OyDYankees.NotasYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.ActualizarNotas(CType(Nothing, String))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateNotasYankees")
        End Try
    End Sub

    Public Sub InsertNotasYankeesDetalle(ByVal obj As OyDYankees.NotasYankeesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.ActualizarNotas(CType(Nothing, String))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertNotasYankeesDetalle")
        End Try
    End Sub

    Public Sub UpdateNotasYankeesDetalle(ByVal obj As OyDYankees.NotasYankeesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.ActualizarNotas(CType(Nothing, String))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateNotasYankeesDetalle")
        End Try
    End Sub
    Public Function NotasYankeesActualizar(plngIDDocumento As Integer, pstrNombreConsecutivo As String, pdtmDocumento As DateTime, pstrEstado As String, pstrOpcion As String, pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_NotasYankees_Actualizar(plngIDDocumento, pstrNombreConsecutivo, pdtmDocumento, pstrEstado, pstrOpcion, pstrRegistros, pstrUsuario, DemeInfoSesion(pstrUsuario, "NotasYankeesActualizar"), 0)
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "NotasYankeesFiltrarDetalle")
            Return 0
        End Try
    End Function
    Public Function NotasYankeesEliminar(plngIDDocumento As Integer, pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_NotasYankees_Eliminar(plngIDDocumento, pstrNombreConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "NotasYankeesEliminar"), 0)
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "NotasYankeesEliminar")
            Return 0
        End Try
    End Function

    Public Function NotasYankeesConsultar(plngIDDocumento As Integer?, pdtmDocumento As DateTime?, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of NotasYankees)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_NotasYankees_Consultar(plngIDDocumento, pdtmDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "NotasYankeesConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "NotasYankeesConsultar")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Función para verificar si el un documento de Tesoreria yankees cambio de esta P a I
    ''' </summary>
    ''' <param name="plngIDDocumento">Nro Documento</param>
    ''' <param name="pstrNombreConsecutivo">Consecutivo</param>
    ''' <param name="pstrTipo">Tipo</param>
    ''' <param name="pstrUsuario">Usuario</param>
    ''' <returns></returns>
    ''' <remarks>JDO20130626</remarks>
    Public Function Verificar_EstadoImpresion(ByVal plngIDDocumento As Integer, pstrNombreConsecutivo As String, pstrTipo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaYankees_VerificarEstadoImpresion(plngIDDocumento, pstrNombreConsecutivo, pstrTipo, pstrUsuario, DemeInfoSesion(pstrUsuario, "Verificar_EstadoImpresion"), 0)
            Return CBool(ret)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Verificar_EstadoImpresion")
            Return False
        End Try
    End Function
#End Region

End Class

