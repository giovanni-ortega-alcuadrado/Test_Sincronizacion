
'Codigo generado
'Plantilla: RIAServiceModelTemplate2010
'Archivo: A2.OyD.OTC.SLViewModel.vb
'Generado el : 09/21/2012 11:57:31
'Propiedad de Alcuadrado S.A. 2010

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
Imports A2.OyD.OYDServer.RIA.Web.OyDOTC
Imports System.Globalization
Imports System.Runtime.Serialization
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()>
Public Class OTCDomainService
    Inherits LinqToSqlDomainService(Of OyD_OTCDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Liquidaciones_OTC"

    Public Function Liquidaciones_OTCFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidaciones_OT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_Liquidaciones_OTC_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "Liquidaciones_OTCFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Liquidaciones_OTCFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function Liquidaciones_OTCConsultar(ByVal pID As Integer?, ByVal pnumeroOperacion As Integer?, ByVal pFechaOperacion As Date?,
                                            ByVal pFechaCumplimiento As Date?, ByVal pIdComitente As String, ByVal pIdEspecie As String,
                                            ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidaciones_OT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_Liquidaciones_OTC_Consultar(pID, pnumeroOperacion, pFechaOperacion, pFechaCumplimiento,
                                            pIdComitente, pIdEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "Liquidaciones_OTCConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Liquidaciones_OTCConsultar")
            Return Nothing
        End Try
    End Function

    Public Function Liquidaciones_OTCAnular(ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_Liquidaciones_OTC_Eliminar(pID, pstrUsuario, DemeInfoSesion(pstrUsuario, "Liquidaciones_OTCAnular"), 0)
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Liquidaciones_OTCAnular")
            Return Nothing
        End Try
    End Function

    Public Function TraerLiquidaciones_OTPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Liquidaciones_OT
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Liquidaciones_OT
            'e.IDCOMISIONISTA = 
            'e.IDSUCCOMISIONISTA = 
            'e.ID = 
            'e.VERSION = 0
            'e.NOMBRESISTEMA = "Citinfo"
            e.NOMBRESISTEMA = String.Empty
            'e.NUMEROOPERACION = 
            e.OPERACION = Date.Now
            e.TIPOOPERACION = "C"
            e.Mercado = "E"
            e.TIPONEGOCIACION = "C"
            e.REGISTROOPERACION = "T"
            e.TIPOPAGOOPERACION = "C"
            'e.IDESPECIE = 
            'e.CANTIDADNEGOCIADA = 
            'e.EMISION = 
            e.CUMPLIMIENTO = Date.Now
            'e.VENCIMIENTO = 
            'e.DIASALVENCIMIENTOTITULO = 
            'e.TASAINTERESNOMINAL = 
            'e.MODALIDADTASANOMINAL = 
            'e.TASAEFECTIVAANUAL = 
            'e.PRECIO = 
            'e.MONTO = 
            'e.IDREPRESENTANTELEGAL = 
            'e.IDCOMITENTE = 
            'e.NROTITULO = 
            e.INDICADOR = "0"
            'e.PUNTOSINDICADOR = 
            e.RENTAFIJA = True
            'e.PREFIJO = 
            'e.IDFACTURA = 
            'e.FACTURADA = 
            e.ESTADO = "P"
            'e.NroLote = 
            'e.ACTUALIZACION = 
            'e.USUARIO = 
            'e.NroLoteENC = 
            'e.ContabilidadENC = 
            'e.IdLiquidaciones_OTC = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerLiquidaciones_OTPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertLiquidaciones_OT(ByVal Liquidaciones_OT As Liquidaciones_OT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Liquidaciones_OT.pstrUsuarioConexion, Liquidaciones_OT.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Liquidaciones_OT.InfoSesion = DemeInfoSesion(Liquidaciones_OT.pstrUsuarioConexion, "InsertLiquidaciones_OT")
            Me.DataContext.Liquidaciones_OTC.InsertOnSubmit(Liquidaciones_OT)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLiquidaciones_OT")
        End Try
    End Sub

    Public Sub UpdateLiquidaciones_OT(ByVal currentLiquidaciones_OT As Liquidaciones_OT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentLiquidaciones_OT.pstrUsuarioConexion, currentLiquidaciones_OT.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentLiquidaciones_OT.InfoSesion = DemeInfoSesion(currentLiquidaciones_OT.pstrUsuarioConexion, "UpdateLiquidaciones_OT")
            currentLiquidaciones_OT.USUARIO = HttpContext.Current.User.Identity.Name
            Me.DataContext.Liquidaciones_OTC.Attach(currentLiquidaciones_OT, Me.ChangeSet.GetOriginal(currentLiquidaciones_OT))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLiquidaciones_OT")
        End Try
    End Sub


#End Region

#Region "ReceptoresOTC"

    Public Function Receptores_OTCConsultar(ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_ReceptoresOTC_Consultar(pID, pstrUsuario, DemeInfoSesion(pstrUsuario, "Receptores_OTCConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Receptores_OTCConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerReceptoresOTPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ReceptoresOT
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ReceptoresOT
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Id = 
            'e.IDReceptor =     
            'e.Lider = 
            'e.Porcentaje = 
            'e.Actualizacion = 
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReceptoresOTPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertReceptoresOT(ByVal ReceptoresOT As ReceptoresOT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOT.pstrUsuarioConexion, ReceptoresOT.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresOT.InfoSesion = DemeInfoSesion(ReceptoresOT.pstrUsuarioConexion, "InsertReceptoresOT")
            Me.DataContext.ReceptoresOTC.InsertOnSubmit(ReceptoresOT)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertReceptoresOT")
        End Try
    End Sub

    Public Sub UpdateReceptoresOT(ByVal currentReceptoresOT As ReceptoresOT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentReceptoresOT.pstrUsuarioConexion, currentReceptoresOT.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentReceptoresOT.Usuario = HttpContext.Current.User.Identity.Name
            currentReceptoresOT.InfoSesion = DemeInfoSesion(currentReceptoresOT.pstrUsuarioConexion, "UpdateReceptoresOT")
            Me.DataContext.ReceptoresOTC.Attach(currentReceptoresOT, Me.ChangeSet.GetOriginal(currentReceptoresOT))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptoresOT")
        End Try
    End Sub

    Public Sub DeleteReceptoresOT(ByVal ReceptoresOT As ReceptoresOT)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOT.pstrUsuarioConexion, ReceptoresOT.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresOT.Usuario = HttpContext.Current.User.Identity.Name
            ReceptoresOT.InfoSesion = DemeInfoSesion(ReceptoresOT.pstrUsuarioConexion, "DeleteReceptoresOT")
            Me.DataContext.ReceptoresOTC.Attach(ReceptoresOT)
            Me.DataContext.ReceptoresOTC.DeleteOnSubmit(ReceptoresOT)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptoresOT")
        End Try
    End Sub


#End Region

#Region "FacturasOTC"

    Public Sub InsertFactura(ByVal Factura As Factura_OTC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Factura.pstrUsuarioConexion, Factura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Factura.InfoSesion = DemeInfoSesion(Factura.pstrUsuarioConexion, "InsertFactura")
            Me.DataContext.Facturas_OTC.InsertOnSubmit(Factura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertFactura")
        End Try
    End Sub

    Public Sub UpdateFactura(ByVal currentFactura As Factura_OTC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentFactura.pstrUsuarioConexion, currentFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentFactura.InfoSesion = DemeInfoSesion(currentFactura.pstrUsuarioConexion, "UpdateFactura")
            Me.DataContext.Facturas_OTC.Attach(currentFactura, Me.ChangeSet.GetOriginal(currentFactura))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateFactura")
        End Try
    End Sub

    Public Function FacturasOTC_Consultar(ByVal plngID As Integer?, ByVal plngIDComitente As String _
                                          , ByVal pdtmDocumento As DateTime?, ByVal pstrNombreComitente As String _
                                          , ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Factura_OTC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_tblFacturasOTC_Consultar(plngID, plngIDComitente, pdtmDocumento _
                                        , pstrNombreComitente, DemeInfoSesion(pstrUsuario, "FacturasOTC_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FacturasOTC_Consultar")
            Return Nothing
        End Try
    End Function


    Public Function Consultar_TotalFactura(ByVal lngIDFactura As Integer, ByVal strPrefijo As String, ByVal curTotalFactura? As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_tblFacturasOTC_TotalFactura(lngIDFactura, strPrefijo, curTotalFactura, DemeInfoSesion(pstrUsuario, "Consultar_TotalFactura"), 0)
            Dim varible = curTotalFactura
            Return CDbl(varible)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_TotalFactura")
            Return Nothing
        End Try
    End Function

    Public Function Liquidaciones_FacturaOTC_Consultar(ByVal lngIDFactura As Integer, ByVal strPrefijo As String,
                                          ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidaciones_OT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_Liquidaciones_OTC_Factura_Consultar(lngIDFactura, strPrefijo, pstrUsuario _
                                        , DemeInfoSesion(pstrUsuario, "Liquidaciones_FacturaOTC_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Liquidaciones_FacturaOTC_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function FacturasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Factura_OTC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_FacturarOTC_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "FacturasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FacturasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function AnularFacturas(ByVal pIDFacturas As Integer, pNumero As Integer, ByVal pPrefijo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_FacturarOTC_Eliminar(pIDFacturas, pNumero, pPrefijo, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularFacturas"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularFacturas")
            Return False
        End Try
    End Function


    Public Sub DeleteFactura(ByVal Factura As Factura_OTC)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Factura.pstrUsuarioConexion, Factura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Facturas_Eliminar( pNumero,  pComitente, DemeInfoSesion(pstrUsuario, "DeleteFactura"),0).ToList
            Factura.InfoSesion = DemeInfoSesion(Factura.pstrUsuarioConexion, "DeleteFactura")
            Me.DataContext.Facturas_OTC.Attach(Factura)
            Me.DataContext.Facturas_OTC.DeleteOnSubmit(Factura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteFactura")
        End Try
    End Sub

#End Region

#Region "Constancia Operacion"

    Public Function ConsultarConstanciaOperacionReceptores_OTC(ByVal pIDOperacion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_sprptConstanciaOperacionReceptoresOTC(pIDOperacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConstanciaOperacionReceptores_OTC"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConstanciaOperacionReceptores_OTC")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConstanciaOperacion_OTC(ByVal pIdInicial As Integer, ByVal pIdFinal As Integer _
                                                     , ByVal pFechaInicial As DateTime, ByVal pFechaFinal As DateTime _
                                                     , ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidaciones_OT)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_OTC_sprptConstanciaOperacionOTC(pIdInicial, pIdFinal, pFechaInicial,
                                            pFechaFinal, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConstanciaOperacion_OTC"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConstanciaOperacion_OTC")
            Return Nothing
        End Try
    End Function

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Método encargado de consultar las liquidaciones y recibir el html para conformar las cartas de constancia operación OTC.
    ''' Fecha            : Mayo 15/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 15/2013 - Resultado Ok 
    ''' </history>
    Public Function PlantillaConstanciaOperacionOTC(ByVal pIdInicial As Integer, ByVal pIdFinal As Integer _
                                                     , ByVal pFechaInicial As DateTime, ByVal pFechaFinal As DateTime _
                                                     , ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrHTMLPlantillaOTC As String = String.Empty
            Dim ret = Me.DataContext.uspOyDNet_OTC_rptPlantillaConstanciaOperacionOTC(pIdInicial, pIdFinal, pFechaInicial,
                                            pFechaFinal, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConstanciaOperacion_OTC"), 0, pstrHTMLPlantillaOTC)
            Return pstrHTMLPlantillaOTC
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConstanciaOperacion_OTC")
            Return Nothing
        End Try
    End Function



#End Region

#Region "Liquidaciones_OTC_SEN"

    ' ''' <summary>
    ' ''' Función encarga de consultar los tipos de operaciones validas.
    ' ''' </summary>
    ' ''' <param name="bitActualizar"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function ConsultarTipoOperacion(ByVal bitActualizar As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS String
    '    Dim strTipoOperacion As String = String.Empty
    '    Try
    '        If bitActualizar Then
    '            strTipoOperacion = "ACT"
    '        Else
    '            strTipoOperacion = "VAL"
    '        End If
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "ConsultarTipoOperacion")
    '    End Try
    '    Return strTipoOperacion
    'End Function

    'Función que valida que las operaciones importadas tenga el formato correcto.
    'Santiago Vergara - Noviembre 14/2013 
    'Se modifica la logica para que cuando se genere un error validando un registro no se genere error no controlado y en vez de esto sea devuelto el error para visualizar el el log del proceso0
    Public Function ValidarOperaciones_OTC(ByVal strMetodo As String, ByVal strUsuario As String,
                                                       ByVal dtmFechaImportacion As Nullable(Of DateTime),
                                                       ByVal dtmHoraImportacion As String,
                                                       ByVal lngIDOperacion As Nullable(Of Integer),
                                                       ByVal lngIdComitente As String,
                                                       ByVal strTipo As String,
                                                       ByVal strEspecie As String,
                                                       ByVal dblCantidad As Nullable(Of Double),
                                                       ByVal lngDiasVencimiento As Nullable(Of Integer),
                                                       ByVal curEquivalente As Nullable(Of Double),
                                                       ByVal curTotal As Nullable(Of Double),
                                                       ByVal curPrecio As Nullable(Of Double),
                                                       ByVal dtmEmision As Nullable(Of DateTime),
                                                       ByVal dtmVencimiento As Nullable(Of DateTime),
                                                       ByVal dtmLiquidacion As Nullable(Of DateTime),
                                                       ByVal strTipoNegociacion As String,
                                                       ByVal strISIN As String,
                                                       ByVal dblCantidadGarantia As Nullable(Of Double),
                                                       ByVal strRueda As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tmpResultado)
        Dim lstRetorno As New List(Of tmpResultado)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            lstRetorno = Me.DataContext.uspOyDNet_OTC_Liquidaciones_SEN_Importar(
                                                        strMetodo,
                                                        strUsuario,
                                                        dtmFechaImportacion,
                                                        dtmHoraImportacion,
                                                        lngIDOperacion,
                                                        lngIdComitente,
                                                        strTipo,
                                                        strEspecie,
                                                        dblCantidad,
                                                        lngDiasVencimiento,
                                                        curEquivalente,
                                                        curTotal,
                                                        curPrecio,
                                                        dtmEmision,
                                                        dtmVencimiento,
                                                        dtmLiquidacion,
                                                        strTipoNegociacion,
                                                        strISIN,
                                                        dblCantidadGarantia,
                                                        strRueda,
                                                        DemeInfoSesion(pstrUsuario, "ConsultarOperacionesOTC_SEN"), 0).ToList
        Catch ex As Exception

            If lstRetorno.Count < 1 Then
                Dim objResultado As New tmpResultado

                With objResultado
                    .lngID = 1
                    .intResultado = -1
                    .lngIDOperacion = lngIDOperacion
                    .strMensaje = "Se presentó un problema al recibir el resultado validando la operacion (" + lngIDOperacion.ToString + "): " + ex.Message
                End With

                lstRetorno.Add(objResultado)
            End If
        End Try

        Return lstRetorno

    End Function

    ''' <summary>
    ''' Funcion encargada de retornar la cadena a partir del separador especificado.
    ''' </summary>
    ''' <param name="strArray"></param>
    ''' <param name="intPosicion"></param>
    ''' <param name="pstrSeparadorDescripcion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function RetornarCadenaSeparador(ByVal strArray As String, ByVal intPosicion As Integer, Optional ByVal pstrSeparadorDescripcion As String = " ") As String
        Dim intIndex As Integer = 0
        Dim strRetorno As String = String.Empty
        Try
            intIndex = strArray.IndexOf(pstrSeparadorDescripcion)
            If intIndex > 0 Then
                strRetorno = strArray.Substring(0, intIndex)
            End If
        Catch ex As Exception
            strRetorno = String.Empty
        End Try
        Return strRetorno
    End Function


    ''' <summary>
    ''' Función encargada de actualizar las operaciones a partir del archivo importardo
    ''' </summary>
    ''' <param name="strUsuario"></param>
    ''' <param name="dtmFechaImportacion"></param>
    ''' <param name="dtmHoraImportacion"></param>
    ''' <param name="lngIDOperacion"></param>
    ''' <param name="lngIdComitente"></param>
    ''' <param name="strTipo"></param>
    ''' <param name="strEspecie"></param>
    ''' <param name="dblCantidad"></param>
    ''' <param name="lngDiasVencimiento"></param>
    ''' <param name="curEquivalente"></param>
    ''' <param name="curTotal"></param>
    ''' <param name="curPrecio"></param>
    ''' <param name="dtmEmision"></param>
    ''' <param name="dtmVencimiento"></param>
    ''' <param name="dtmLiquidacion"></param>
    ''' <param name="strTipoNegociacion"></param>
    ''' <param name="strISIN"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ActualizarOperacionesSEN(ByVal strUsuario As String,
                                                       ByVal dtmFechaImportacion As Nullable(Of DateTime),
                                                       ByVal dtmHoraImportacion As String,
                                                       ByVal lngIDOperacion As Nullable(Of Integer),
                                                       ByVal lngIdComitente As String,
                                                       ByVal strTipo As String,
                                                       ByVal strEspecie As String,
                                                       ByVal dblCantidad As Nullable(Of Double),
                                                       ByVal lngDiasVencimiento As Nullable(Of Integer),
                                                       ByVal curEquivalente As Nullable(Of Double),
                                                       ByVal curTotal As Nullable(Of Double),
                                                       ByVal curPrecio As Nullable(Of Double),
                                                       ByVal dtmEmision As Nullable(Of DateTime),
                                                       ByVal dtmVencimiento As Nullable(Of DateTime),
                                                       ByVal dtmLiquidacion As Nullable(Of DateTime),
                                                       ByVal strTipoNegociacion As String,
                                                       ByVal strISIN As String,
                                                       ByVal dblCantidadGarantia As Nullable(Of Double),
                                                       ByVal strRueda As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblImportacionLiqSEN)
        Dim objValidacion As New List(Of tmpResultado)
        Dim lstDatosOperacion As New List(Of tblImportacionLiqSEN)
        Dim objDatos As tblImportacionLiqSEN = Nothing

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            objValidacion = Me.ValidarOperaciones_OTC("ACT", strUsuario,
                                                 dtmFechaImportacion,
                                                  dtmHoraImportacion,
                                                  lngIDOperacion,
                                                  lngIdComitente,
                                                  strTipo,
                                                  strEspecie,
                                                  dblCantidad,
                                                  lngDiasVencimiento,
                                                  curEquivalente,
                                                  curTotal,
                                                  curPrecio,
                                                  dtmEmision,
                                                 dtmVencimiento,
                                                  dtmLiquidacion,
                                                  strTipoNegociacion,
                                                  strISIN,
                                                  dblCantidadGarantia,
                                                  strRueda,
                                                  pstrUsuario, pstrInfoConexion)
            If objValidacion.Count > 0 Then

                lstDatosOperacion.Add(New tblImportacionLiqSEN With {
                                                                        .lngID = 0,
                                                                        .intResultado = objValidacion(0).intResultado,
                                                                        .ListaComentario = objValidacion(0).strMensaje.Replace("<<NUMEROOPERACION>>", CStr(lngIDOperacion))
                                                                    })

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ActualizarOperacionesSEN")
            Return Nothing
        End Try
        Return lstDatosOperacion
    End Function

    <Query(HasSideEffects:=True)>
    Public Function ActualizarOperacionesSENMasivo(ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblImportacionLiqSEN)
        Dim objValidacion As New List(Of tmpResultado)
        Dim lstDatosOperacion As New List(Of tblImportacionLiqSEN)
        Dim objDatos As tblImportacionLiqSEN = Nothing

        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objValidacion = Me.DataContext.uspOyDNet_OTC_Liquidaciones_SEN_ImportarMasivo("ACT", pstrRegistros, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarOperacionesSENMasivo"), 0).ToList

            If Not IsNothing(objValidacion) Then
                For Each li In objValidacion
                    lstDatosOperacion.Add(New tblImportacionLiqSEN With {
                                                                            .lngID = li.lngID,
                                                                            .intResultado = li.intResultado,
                                                                            .ListaComentario = li.strMensaje
                                                                        })
                Next
            End If

        Catch ex As Exception
            ManejarError(ex, Me.ToString, "ActualizarOperacionesSENMasivo")
            Return Nothing
        End Try
        Return lstDatosOperacion
    End Function

    ''' <summary>
    ''' Formatea un número que viene como string 
    ''' quita los separadores de miles 
    ''' siempre el separador decimal va a ser punto (.)
    ''' </summary>
    ''' <param name="strValor"></param>
    ''' <returns></returns>
    ''' <remarks>SantiagoVergara20130802</remarks>
    Private Function FormatearNumero(ByVal strValor As String) As String
        Dim strSimboloDecimal As String
        Dim strResultado As String
        If Not IsNothing(strValor) Then
            If strValor.Length > 3 Then
                strSimboloDecimal = strValor.Substring(strValor.Length - 3, 1)

                If strSimboloDecimal = "," Then
                    strResultado = strValor.Replace(".", "")
                    strResultado = strResultado.Replace(",", ".")
                Else
                    strResultado = strValor.Replace(",", "")
                End If
            Else
                strResultado = strValor
            End If
            Return strResultado
        Else
            Return ""
        End If
    End Function

    'Función encargada de Leer el archivo plano importado.
    'Santiago Vergara - Noviembre 14/2013 
    'Se modifica la logica de las validaciones para que no se generen errores no controlados en las conversiones
    'Se cambia la lógica de generación de un nuevo id lo cual se hacia con un número aleatorio
    'Santiago Vergara - Marzo 07/2014 - Se quita el parámetro plngIdComitente, se añade a la carga los datos de comitente, rueda y cantidad garantia
    Public Function LeerArchivoOperaciones_SEN(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblImportacionLiqSEN)
        Dim archivo As System.IO.StreamReader = Nothing
        Dim objOTCDomainService As OTCDomainService
        Dim intUltimoIdOTCSEN As Integer = 0
        Dim logDatoInconsistente As Boolean
        Dim logRegistroInconsistente As Boolean
        Dim logValorNegativo As Boolean

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploads.FnRutasImportaciones(pstrNombreProceso, pstrUsuario)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'Dim archivo As System.IO.StreamReader = Nothing
            Dim contador As Integer = 0
            Dim objResultado As New List(Of tmpResultado)
            Dim objValidacion As New List(Of tmpResultado)
            Dim lngRegbuenos As Integer = 0
            Dim lngRegErrores As Integer = 0
            Dim objDatos As tblImportacionLiqSEN = Nothing
            Dim lstDatosOperacion As New List(Of tblImportacionLiqSEN)
            Dim strFecha As String = String.Empty
            Dim prov As IFormatProvider = New CultureInfo("es-CO")
            Dim objproviderUS As IFormatProvider = New CultureInfo("en-US")
            Dim strMensajeError As String = String.Empty

            intUltimoIdOTCSEN = 0
            'Recorremos el archivo de encabezados
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyDVersiones\v12.2\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso
            archivo = New System.IO.StreamReader(directorioUsuario & "\" & pstrNombreCompletoArchivo)
            Do While archivo.Peek() <> -1

                logRegistroInconsistente = False
                contador = contador + 1
                objDatos = New tblImportacionLiqSEN
                ' objValidacion = Nothing
                Dim strLinea As String() = archivo.ReadLine().Split(CChar(vbTab))
                'objDatos.lngIdComitente = CStr(plngIdComitente)
                objDatos.strUsuario = pstrUsuario


                'Se recorre la línea para verificar los valores
                For intValor As Integer = 0 To 17
                    Try
                        logDatoInconsistente = False
                        strMensajeError = ""

                        Select Case intValor
                            Case 0 'Fecha
                                strMensajeError = "La fecha de importación tiene un formato no válido"
                                If Not IsDate(DateTime.Parse(strLinea(intValor), prov, System.Globalization.DateTimeStyles.NoCurrentDateDefault)) Then 'SLB20130528 Se adicionan las validaciones de los tipos de Datos.
                                    logDatoInconsistente = True
                                Else
                                    objDatos.dtmFechaImportacion = DateTime.Parse(strLinea(intValor), prov, System.Globalization.DateTimeStyles.NoCurrentDateDefault)
                                End If

                            Case 1 'Hora
                                strMensajeError = "La hora tiene un formato no válido"
                                objDatos.dtmHoraImportacion = String.Format("{0:HH:mm:ss}", RetornarCadenaSeparador(strLinea(intValor), 1))

                            Case 2 'Id operación (Secuencia)
                                strMensajeError = "La operación tiene un formato no válido"
                                If Not IsNumeric(FormatearNumero(strLinea(intValor))) Then
                                    logDatoInconsistente = True
                                Else
                                    objDatos.lngIDOperacion = CType(FormatearNumero(strLinea(intValor)), Nullable(Of Integer))
                                End If

                            Case 3 'Tipo
                                strMensajeError = "El Tipo tiene un formato no válido"
                                objDatos.strTipo = CType(strLinea(intValor), String)

                            Case 4 'Especie
                                strMensajeError = "La especie tiene un formato no válido"
                                objDatos.strEspecie = RetornarCadenaSeparador(strLinea(intValor), 1)

                            Case 5 'Cantidad
                                strMensajeError = "La cantidad tiene un formato no válido"
                                Dim strCantidad As String
                                strCantidad = FormatearNumero(strLinea(intValor))

                                If Not String.IsNullOrEmpty(strCantidad) Then
                                    If Not IsNumeric(strCantidad) Then
                                        logDatoInconsistente = True
                                    Else
                                        objDatos.dblCantidad = Double.Parse(strCantidad, System.Globalization.NumberStyles.AllowDecimalPoint, objproviderUS)
                                    End If
                                Else
                                    objDatos.dblCantidad = 0
                                End If

                            Case 6 'Dias Vto Título
                                strMensajeError = "Los días de vencimiento del titulo tiene un formato no válido"
                                If Not IsNumeric(FormatearNumero(strLinea(intValor))) Then
                                    logDatoInconsistente = True
                                Else
                                    objDatos.lngDiasVencimiento = CType(FormatearNumero(strLinea(intValor)), Nullable(Of Integer))
                                End If

                            Case 7 'CurEquivalente
                                strMensajeError = "El valor equivalente tiene un formato no válido"
                                Dim strEquivalente As String
                                strEquivalente = FormatearNumero(strLinea(intValor))
                                If Not IsNumeric(strEquivalente) Then
                                    logDatoInconsistente = True
                                Else
                                    If strEquivalente <> "" Then
                                        objDatos.curEquivalente = Double.Parse(strEquivalente, System.Globalization.NumberStyles.AllowDecimalPoint, objproviderUS)
                                    Else
                                        objDatos.curEquivalente = 0
                                    End If
                                End If

                            Case 8 'Total
                                strMensajeError = "El total tiene un formato no válido"
                                Dim strTotal As String

                                strTotal = FormatearNumero(strLinea(intValor))

                                If Not IsNumeric(strTotal) Then
                                    logDatoInconsistente = True
                                Else
                                    If strTotal <> "" Then
                                        objDatos.curTotal = Double.Parse(strTotal, System.Globalization.NumberStyles.AllowDecimalPoint, objproviderUS)
                                    Else
                                        objDatos.curTotal = 0
                                    End If
                                End If

                            Case 9 'Precio
                                strMensajeError = "El precio tiene un formato no válido"
                                Dim strPrecio As String

                                logValorNegativo = False

                                strPrecio = FormatearNumero(strLinea(intValor))

                                If strPrecio.Contains("-") Then
                                    logValorNegativo = True
                                    strPrecio = strPrecio.Replace("-", "")
                                End If

                                If Not IsNumeric(strPrecio) Then
                                    logDatoInconsistente = True
                                Else
                                    If strPrecio <> "" Then
                                        objDatos.curPrecio = Double.Parse(strPrecio, System.Globalization.NumberStyles.AllowDecimalPoint, objproviderUS)
                                        If logValorNegativo Then
                                            objDatos.curPrecio = objDatos.curPrecio * (-1)
                                        End If
                                    Else
                                        objDatos.curPrecio = 0
                                    End If
                                End If

                            Case 10 'Fecha de Emisión
                                strMensajeError = "La fecha de emisión tiene un formato no válido"
                                If Not IsDate(DateTime.Parse(strLinea(intValor), prov, System.Globalization.DateTimeStyles.NoCurrentDateDefault)) Then
                                    logDatoInconsistente = True
                                Else
                                    objDatos.dtmEmision = DateTime.Parse(strLinea(intValor), prov, System.Globalization.DateTimeStyles.NoCurrentDateDefault)
                                End If

                            Case 11 'Fecha de Vencimiento
                                strMensajeError = "La fecha de vencimiento tiene un formato no válido"
                                If Not IsDate(DateTime.Parse(strLinea(intValor), prov, System.Globalization.DateTimeStyles.NoCurrentDateDefault)) Then
                                    logDatoInconsistente = True
                                Else
                                    objDatos.dtmVencimiento = DateTime.Parse(strLinea(intValor), prov, System.Globalization.DateTimeStyles.NoCurrentDateDefault)
                                End If

                            Case 12 'Fecha liquidación
                                strMensajeError = "La fecha de liquidación tiene un formato no válido"
                                If Not IsDate(DateTime.Parse(strLinea(intValor), prov, System.Globalization.DateTimeStyles.NoCurrentDateDefault)) Then
                                    logDatoInconsistente = True
                                Else
                                    objDatos.dtmLiquidacion = DateTime.Parse(strLinea(intValor), prov, System.Globalization.DateTimeStyles.NoCurrentDateDefault)
                                End If

                            Case 13 'Tipo Negociación
                                strMensajeError = "El tipo de negociación tiene un formato no válido"
                                objDatos.strTipoNegociacion = CType(strLinea(intValor), String)

                            Case 14 'Isin
                                strMensajeError = "El Isin tiene un formato no válido"
                                objDatos.strISIN = CType(strLinea(intValor), String)

                            Case 15 'Rueda
                                strMensajeError = "La rueda tiene un formato no válido"
                                objDatos.strRueda = CType(strLinea(intValor), String)

                            Case 16 'Cantidad Garantía
                                strMensajeError = "La cantidad garantía tiene un formato no válido"
                                Dim strCantidadGarantia As String
                                strCantidadGarantia = FormatearNumero(strLinea(intValor))

                                If Not String.IsNullOrEmpty(strCantidadGarantia) Then
                                    If Not IsNumeric(strCantidadGarantia) Then
                                        logDatoInconsistente = True
                                    Else
                                        objDatos.dblCantidadGarantia = Double.Parse(strCantidadGarantia, System.Globalization.NumberStyles.AllowDecimalPoint, objproviderUS)
                                    End If
                                Else
                                    objDatos.dblCantidadGarantia = 0
                                End If

                            Case 17 'Comitente
                                strMensajeError = "El Comitente tiene un formato no válido"
                                objDatos.lngIdComitente = CType(strLinea(intValor), String)

                        End Select

                        If logDatoInconsistente Then
                            logRegistroInconsistente = True
                            intUltimoIdOTCSEN += 1
                            lstDatosOperacion.Add(New tblImportacionLiqSEN With {
                                                                           .intResultado = -1,
                                                                           .lngID = intUltimoIdOTCSEN,
                                                                           .ListaComentario = "Error Línea " + contador.ToString + ", Columna " + (intValor + 1).ToString + ":  " + strMensajeError})
                        End If

                    Catch ex As Exception
                        'Si se presentó un error al leer el registro
                        logRegistroInconsistente = True

                        If strMensajeError = "" Then
                            strMensajeError = "Error al leer la línea, el registro es inconsistente"
                        End If
                        intUltimoIdOTCSEN += 1
                        lstDatosOperacion.Add(New tblImportacionLiqSEN With {
                                                                       .intResultado = -1,
                                                                       .lngID = intUltimoIdOTCSEN,
                                                                       .ListaComentario = "Error Línea " + contador.ToString + ", Columna " + (intValor + 1).ToString + ":  " + strMensajeError})
                        Exit For

                    End Try


                Next

                If logRegistroInconsistente Then
                    lngRegErrores += 1

                Else
                    'Si el registro no es inconsistente se llama el proceso de validaciones de base de datos

                    objOTCDomainService = New OTCDomainService
                    'Se llama al método para hacer las validaciones de base de datos'
                    objValidacion = objOTCDomainService.ValidarOperaciones_OTC("VAL", pstrUsuario,
                                                       objDatos.dtmFechaImportacion,
                                                       objDatos.dtmHoraImportacion,
                                                       objDatos.lngIDOperacion,
                                                       objDatos.lngIdComitente,
                                                       objDatos.strTipo,
                                                       objDatos.strEspecie,
                                                       objDatos.dblCantidad,
                                                       objDatos.lngDiasVencimiento,
                                                       objDatos.curEquivalente,
                                                       objDatos.curTotal,
                                                       objDatos.curPrecio,
                                                       objDatos.dtmEmision,
                                                       objDatos.dtmVencimiento,
                                                       objDatos.dtmLiquidacion,
                                                       objDatos.strTipoNegociacion,
                                                       objDatos.strISIN,
                                                       objDatos.dblCantidadGarantia,
                                                       objDatos.strRueda,
                                                       pstrUsuario, pstrInfoConexion)

                    objOTCDomainService = Nothing


                    If objValidacion.Count = 0 Then

                        'Si no hay validaciones se añade el registro a la lista 
                        lngRegbuenos += 1
                        intUltimoIdOTCSEN += 1
                        lstDatosOperacion.Add(New tblImportacionLiqSEN With {
                                                                                 .lngID = intUltimoIdOTCSEN,
                                                                                 .dtmFechaImportacion = objDatos.dtmFechaImportacion,
                                                                                 .dtmHoraImportacion = objDatos.dtmHoraImportacion,
                                                                                 .lngIDOperacion = objDatos.lngIDOperacion,
                                                                                 .lngIdComitente = objDatos.lngIdComitente,
                                                                                 .strTipo = objDatos.strTipo,
                                                                                 .strEspecie = objDatos.strEspecie,
                                                                                 .dblCantidad = objDatos.dblCantidad,
                                                                                 .lngDiasVencimiento = objDatos.lngDiasVencimiento,
                                                                                 .curEquivalente = objDatos.curEquivalente,
                                                                                 .curTotal = objDatos.curTotal,
                                                                                 .curPrecio = objDatos.curPrecio,
                                                                                 .dtmEmision = objDatos.dtmEmision,
                                                                                 .dtmVencimiento = objDatos.dtmVencimiento,
                                                                                 .dtmLiquidacion = objDatos.dtmLiquidacion,
                                                                                 .strTipoNegociacion = objDatos.strTipoNegociacion,
                                                                                 .intResultado = 0,
                                                                                 .strISIN = objDatos.strISIN,
                                                                                 .dblCantidadGarantia = objDatos.dblCantidadGarantia,
                                                                                 .strRueda = objDatos.strRueda})
                    Else
                        lngRegErrores += 1
                        'En caso de que hayan validaciones se añaden los registyros a la lista
                        For Each objVal In objValidacion

                            Dim intNroOperacion As System.Nullable(Of Integer) = Nothing
                            Dim strMensaje As String = String.Empty

                            intNroOperacion = objDatos.lngIDOperacion
                            strMensaje = objVal.strMensaje.Replace("<<NUMEROOPERACION>>", CStr(intNroOperacion))

                            intUltimoIdOTCSEN += 1
                            lstDatosOperacion.Add(New tblImportacionLiqSEN With {
                                                                                .intResultado = -1,
                                                                                .lngID = intUltimoIdOTCSEN,
                                                                                .ListaComentario = "Error Línea " + contador.ToString + ", Operación " + CStr(intNroOperacion) + ":  " + strMensaje})

                        Next

                    End If

                End If

            Loop

            archivo.Close()

            intUltimoIdOTCSEN += 1
            lstDatosOperacion.Add(New tblImportacionLiqSEN With {.lngID = intUltimoIdOTCSEN, .intResultado = -1, .lngIDOperacion = objDatos.lngIDOperacion,
                                                                    .ListaComentario = "Total de registros Leidos: " + (contador).ToString _
                                                                    + ", Registros Importados: " + (lngRegbuenos).ToString _
                                                                    + ", Registros Inconsistentes: " + (lngRegErrores).ToString
                                                                })

            Return lstDatosOperacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "LeerArchivoOperaciones_SEN")
            Return Nothing
        Finally
            archivo.Dispose()
        End Try
    End Function

    Public Function TraerOperaciones_OTC_Defecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblImportacionLiqSEN
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New tblImportacionLiqSEN
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerOperaciones_OTC_Defecto")
            Return Nothing
        End Try
    End Function

    Public Sub ActualizarOperaciones(ByVal operacion As tblImportacionLiqSEN)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,operacion.pstrUsuarioConexion, operacion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.tblImportacionLiqSENs.InsertOnSubmit(operacion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarOperaciones")
        End Try
    End Sub

#End Region

End Class

