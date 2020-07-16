
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
Imports A2.OyD.OYDServer.RIA.Web.OyDMILA
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

'Implements application logic using the OyD_ImportacionesDataContext context.
' TODO: Add your application logic to these methods or in additional methods.
' TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
' Also consider adding roles to restrict access as appropriate.
'<RequiresAuthentication> _
<EnableClientAccess()>
Public Class MILADomainService
    Inherits LinqToSqlDomainService(Of OyDMILADatacontext)

    ''' <summary>
    ''' Asignar la variable time out
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Liquidaciones"

    Public Function LiquidacionesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Filtrar_MI(pstrFiltro, DemeInfoSesion(pstrUsuario, "LiquidacionesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function LiquidacionesConsultar(ByVal pID As Integer, ByVal pIDComitente As String, ByVal pTipo As String, ByVal pClase As String, ByVal Liquidacion As DateTime, ByVal Cumplimiento As DateTime, ByVal pOrden As Integer, ByVal pParcial As Integer, ByVal pordenante As String, ByVal pAno As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Consultar_MI(pID, pIDComitente, pTipo, pClase, Liquidacion, Cumplimiento, pOrden, pParcial, pordenante, pAno, DemeInfoSesion(pstrUsuario, "BuscarLiquidaciones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarLiquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function TraerLiquidacionePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Liquidacione_MI
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Liquidacione_MI
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.ID = 
            'e.Parcial = 
            'e.Tipo = 
            'e.ClaseOrden = 
            'e.IDEspecie = 
            'e.IDOrden = 
            'e.Prefijo = 
            e.IDFactura = Nothing
            'e.Facturada = 
            'e.IDComitente = 
            'e.IDOrdenante = 
            'e.IDBolsa = 
            'e.ValBolsa = 
            'e.IDRueda = 
            'e.TasaDescuento = 
            'e.TasaCompraVende = 
            'e.Modalidad = 
            'e.IndicadorEconomico = 
            'e.PuntosIndicador = 
            'e.Plazo = 
            'e.Liquidacion = 
            'e.Cumplimiento = 
            'e.Emision = 
            'e.Vencimiento = 
            'e.OtraPlaza = 
            'e.Plaza = 
            'e.IDComisionistaLocal = 
            'e.IDComisionistaOtraPlaza = 
            'e.IDCiudadOtraPlaza = 
            'e.TasaEfectiva = 
            'e.Cantidad = 
            'e.Precio = 
            'e.Transaccion = 
            'e.SubTotalLiq = 
            'e.TotalLiq = 
            'e.Comision = 
            'e.Retencion = 
            'e.Intereses = 
            'e.ValorIva = 
            'e.DiasIntereses = 
            'e.FactorComisionPactada = 
            'e.Mercado = 
            'e.NroTitulo = 
            'e.IDCiudadExpTitulo = 
            'e.PlazoOriginal = 
            'e.Aplazamiento = 
            'e.VersionPapeleta = 
            'e.EmisionOriginal = 
            'e.VencimientoOriginal = 
            'e.Impresiones = 
            'e.FormaPago = 
            'e.CtrlImpPapeleta = 
            'e.DiasVencimiento = 
            'e.PosicionPropia = 
            'e.Transaccion_cur = 
            'e.TipoOperacion = 
            'e.DiasContado = 
            'e.Ordinaria = 
            'e.ObjetoOrdenExtraordinaria = 
            'e.NumPadre = 
            'e.ParcialPadre = 
            'e.OperacionPadre = 
            'e.DiasTramo = 
            'e.Vendido = 
            'e.Vendido_fecha = 
            'e.Manual = 
            'e.ValorTraslado = 
            'e.ValorBrutoCompraVencida = 
            'e.AutoRetenedor = 
            'e.Sujeto = 
            'e.PcRenEfecCompraRet = 
            'e.PcRenEfecVendeRet = 
            'e.Reinversion = 
            'e.Swap = 
            'e.NroSwap = 
            'e.Certificacion = 
            'e.DescuentoAcumula = 
            'e.PctRendimiento = 
            'e.FechaCompraVencido = 
            'e.PrecioCompraVencido = 
            'e.ConstanciaEnajenacion = 
            'e.RepoTitulo = 
            'e.ServBolsaVble = 
            'e.ServBolsaFijo = 
            'e.Traslado = 
            'e.UBICACIONTITULO = 
            'e.HoraGrabacion = 
            'e.OrigenOperacion = 
            'e.CodigoOperadorCompra = 
            'e.CodigoOperadorVende = 
            'e.IdentificacionRemate = 
            'e.ModalidaOperacion = 
            'e.IndicadorPrecio = 
            'e.PeriodoExdividendo = 
            'e.PlazoOperacionRepo = 
            'e.ValorCaptacionRepo = 
            'e.VolumenCompraRepo = 
            'e.PrecioNetoFraccion = 
            'e.VolumenNetoFraccion = 
            'e.CodigoContactoComercial = 
            'e.NroFraccionOperacion = 
            'e.IdentificacionPatrimonio1 = 
            'e.TipoidentificacionCliente2 = 
            'e.NitCliente2 = 
            'e.IdentificacionPatrimonio2 = 
            'e.TipoIdentificacionCliente3 = 
            'e.NitCliente3 = 
            'e.IdentificacionPatrimonio3 = 
            'e.IndicadorOperacion = 
            'e.BaseRetencion = 
            'e.PorcRetencion = 
            'e.BaseRetencionTranslado = 
            'e.PorcRetencionTranslado = 
            'e.PorcIvaComision = 
            'e.IndicadorAcciones = 
            'e.OperacionNegociada = 
            'e.FechaConstancia = 
            'e.ValorConstancia = 
            'e.GeneraConstancia = 
            'e.Cargado = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.CumplimientoTitulo = 
            'e.NroLote = 
            'e.ValorEntregaContraPago = 
            'e.AquienSeEnviaRetencion = 
            'e.IDBaseDias = 
            'e.TipoDeOferta = 
            'e.NroLoteENC = 
            'e.ContabilidadENC = 
            'e.IDLiquidaciones = 
            'e.CodigoIntermediario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerLiquidacionePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertLiquidacione(ByVal Liquidacione As Liquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Liquidacione.pstrUsuarioConexion, Liquidacione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Liquidacione.InfoSesion = DemeInfoSesion(Liquidacione.pstrUsuarioConexion, "InsertLiquidacione")
            Me.DataContext.Liquidaciones_MI.InsertOnSubmit(Liquidacione)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLiquidacione")
        End Try
    End Sub

    Public Sub UpdateLiquidacione(ByVal currentLiquidacione As Liquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentLiquidacione.pstrUsuarioConexion, currentLiquidacione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentLiquidacione.InfoSesion = DemeInfoSesion(currentLiquidacione.pstrUsuarioConexion, "UpdateLiquidacione")
            Me.DataContext.Liquidaciones_MI.Attach(currentLiquidacione, Me.ChangeSet.GetOriginal(currentLiquidacione))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLiquidacione")
        End Try
    End Sub

    Public Sub DeleteLiquidacione(ByVal Liquidacione As Liquidacione_MI)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Liquidacione.pstrUsuarioConexion, Liquidacione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Eliminar( pID,  pIDComitente, DemeInfoSesion(pstrUsuario, "DeleteLiquidacione"),0).ToList
            Liquidacione.InfoSesion = DemeInfoSesion(Liquidacione.pstrUsuarioConexion, "DeleteLiquidacione")
            Me.DataContext.Liquidaciones_MI.Attach(Liquidacione)
            Me.DataContext.Liquidaciones_MI.DeleteOnSubmit(Liquidacione)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLiquidacione")
        End Try
    End Sub

    Public Function Traer_ReceptoresOrdenes_Liquidaciones(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrdene_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(ptipo) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_ReceptoresOrdenes_Filtrar_MI(ptipo, pclase, pIDorden, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresOrdenes_Liquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresOrdenes_Liquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function Traer_BeneficiariosOrdenes_Liquidaciones(ByVal pId As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiariosOrdene_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_BeneficiariosOrdenes_Filtrar_MI(pId, DemeInfoSesion(pstrUsuario, "Traer_BeneficiariosOrdenes_Liquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosOrdenes_Liquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function Traer_EspeciesLiquidaciones(ByVal pId As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesLiquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_EspeciesLiquidaciones_Filtrar_MI(pId, DemeInfoSesion(pstrUsuario, "Traer_EspeciesLiquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_EspeciesLiquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function Traer_AplazamientosLiquidaciones(ByVal dtmliquidacion As DateTime, ByVal pId As Integer, ByVal pParcial As Integer, ByVal ptipo As String, ByVal pclase As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AplazamientosLiquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_AplazamientosLiquidaciones_Filtrar_MI(pId, dtmliquidacion, pParcial, ptipo, pclase, DemeInfoSesion(pstrUsuario, "Traer_AplazamientosLiquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_AplazamientosLiquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function Traer_CustodiasLiquidaciones(ByVal plngIDComisionista As Integer, ByVal PlngIDSucComisionista As Integer, ByVal plngIDComitente As String,
                                                 ByVal pstrIDEspecie As String, ByVal pstrTipo As String, ByVal pstrClaseOrden As String, ByVal plngID As Integer,
                                                 ByVal plngParcial As Integer, ByVal pdtmLiquidacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CustodiasLiquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(plngID) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_CustodiasLiquidaciones_Filtrar_MI(plngIDComisionista, PlngIDSucComisionista, plngIDComitente, pstrIDEspecie, pstrTipo,
                                                                                        pstrClaseOrden, plngID, plngParcial, pdtmLiquidacion,
                                                                                        DemeInfoSesion(pstrUsuario, "Traer_CustodiasLiquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_CustodiasLiquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function Aplazamiento(ByVal pstrTipoAplazamiento As String, ByVal pstrAplazamiento As String, ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String _
                                 , ByVal plngIDLiquidacion As Integer, ByVal plngParcial As Integer, ByVal pdtmLiquidacion As DateTime, ByVal pdtmCumplimiento As DateTime,
                                 ByVal pstrUsuario As String, ByVal pstreRRor As String, ByVal intNroAplazamientos As System.Nullable(Of Integer), ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_Liquidaciones_Aplazamiento_MI(pstrTipoAplazamiento, pstrAplazamiento, pstrClaseOrden, pstrTipoOrden, plngIDLiquidacion, plngParcial, pdtmLiquidacion _
              , pdtmCumplimiento, pstrUsuario, pstreRRor, intNroAplazamientos, DemeInfoSesion(pstrUsuario, "Aplazamiento"), 0).ToString

            If pstreRRor = String.Empty Then
                Return intNroAplazamientos.ToString
            Else
                Return pstreRRor
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Aplazamiento")
            Return Nothing
        End Try
    End Function

    Public Function LiquidacionesConsultarvalidar(ByVal ptipo As String, ByVal pclase As String, ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesConsultar_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesVeriOrd_OyDNet_MI(ptipo, pclase, pID).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarLiquidacionesConsultarvalidar")
            Return Nothing
        End Try
    End Function

    Public Function LiquidacionesConsultarcantidad(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of consultarcantidad_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_TraerCumplimiento_OyDNet_MI(ptipo, pclase, pIDorden, pEspecie).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarLiquidacionesConsultarcantidad")
            Return Nothing
        End Try
    End Function
    Public Function ReceptoresOrdenesliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrdene_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesReceOrd_OyDNet_MI(ptipo, pclase, pIDorden).ToList
            'ret.First.TipoOrden = ptipo
            'ret.First.ClaseOrden = pclase
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarReceptoresOrdenesliq")
            Return Nothing
        End Try
    End Function

    Public Function BeneficiariosOrdenesliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiariosOrdene_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesBeneOrd_OyDNet_MI(ptipo, pclase, pIDorden).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBeneficiariosOrdenesliq")
            Return Nothing
        End Try
    End Function
    Public Function EspeciesOrdenesliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesLiquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesEspecies_OyDNet_MI(ptipo, pclase, pIDorden).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesOrdenesliq")
            Return Nothing
        End Try
    End Function

    Public Function Ordenesliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOrdenes_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesOrdenes_OyDNet_MI(ptipo, pclase, pIDorden).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarOrdenesliq")
            Return Nothing
        End Try
    End Function

    Public Function verilifaliq(ByVal pID As Integer, ByVal pParcial As Integer, ByVal ptipo As String, ByVal pclase As String, ByVal pILiquidacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesVeriLiq_OyDNet_MI(pID, pParcial, ptipo, pclase, pILiquidacion)
            Return pID
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Buscarverilifaliq")
            Return Nothing
        End Try
    End Function

    Public Function verilifavalor(ByVal pidespecie As String, ByVal pdtmliquidacion As DateTime, ByVal valor As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spValorEspecie_OyDNet(pidespecie, pdtmliquidacion, valor)
            Return valor
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Buscaverilifavalor")
            Return Nothing
        End Try
    End Function


    Public Function verificanombretarifa(ByVal pidespecie As String, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spDenominacionEspecie(pidespecie, pNombre)
            Return pNombre
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Buscaverificanombretarifa")
            Return Nothing
        End Try
    End Function


    Public Function verificadblIvacomision(ByVal pivacomision As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spdblIvacomision(pivacomision)
            Return pivacomision
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscaverificadblIvacomision")
            Return Nothing
        End Try
    End Function
    Public Function Actualizaordenestado(ByVal ptipo As String, ByVal pclase As String, ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim estado As String = String.Empty
            Dim ret = Me.DataContext.spLiquidacionesOrdeCum_OyDNet_MI(ptipo, pclase, pID, estado)
            Return estado
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Actualizaordenestado")
            Return Nothing
        End Try
    End Function

    Public Function Actualizaordenestadocumplida(ByVal ptipo As String, ByVal pclase As String, ByVal pID As Integer, ByVal plngVersion As Integer, ByVal pstrEstado As String, ByVal pdtmEstado As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spUpdOrdenes_OyDNet_MI(ptipo, pclase, pID, plngVersion, pstrEstado, pdtmEstado, pstrUsuario)
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Actualizaordenestadocumplida")
            Return Nothing
        End Try
    End Function

    Public Function CalculaCostoMonedaOrigen(ByVal plngIDBolsa As Integer, ByVal dblCostosOrigen As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spOrdenes_MI_CalcularValorCostoMonedaOrigen_OyDNet_MI(plngIDBolsa, dblCostosOrigen)
            Return dblCostosOrigen
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalculaCostoMonedaOrigen")
            Return Nothing
        End Try
    End Function


    Public Function CalculaValorNeto(ByVal Tipo As String, ByVal ValorTotalMonOrigen As Double, ByVal ValorCostosMonOrigen As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacion_MI_ValorNetoMonedaOrigen_Consultar_OyDNet_MI(Tipo, ValorTotalMonOrigen, ValorCostosMonOrigen)
            Return ValorCostosMonOrigen
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalculaValorNeto")
            Return Nothing
        End Try
    End Function


    Public Function CalculaValorBruto(ByVal plngIDOrden As Integer, ByVal pstrClase As String, ByVal pstrTipo As String, ByVal pcurValor As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacion_MI_TasasConversion_Consultar_OyDNet_MI(plngIDOrden, pstrClase, pstrTipo, pcurValor)
            Return pcurValor
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalculaValorBruto")
            Return Nothing
        End Try
    End Function

    Public Function Calculacomisionpesos(ByVal plngIDOrden As Integer, ByVal pstrClase As String, ByVal pstrTipo As String, ByVal pcurValorbruto As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacion_MI_ComisionPesos_Consultar_OyDnet_MI(plngIDOrden, pstrClase, pstrTipo, pcurValorbruto)
            Return pcurValorbruto
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Calculacomisionpesos")
            Return Nothing
        End Try
    End Function

    Public Function CalculaIvacomisionpesos(ByVal pcurValor As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacion_MI_IVAComisionPesos_Consultar_OyDNet_MI(pcurValor)
            Return pcurValor
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalculaIvacomisionpesos")
            Return Nothing
        End Try
    End Function


    Public Function Calculacostospesos(ByVal plngIDOrden As Integer, ByVal pstrClase As String, ByVal pstrTipo As String, ByVal pcurValor As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacion_MI_CostosPesos_Consultar_OyDNet_MI(plngIDOrden, pstrClase, pstrTipo, pcurValor)
            Return pcurValor
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Calculacostospesos")
            Return Nothing
        End Try
    End Function
    Public Function Calculavalornetopesos(ByVal pstrTipo As String, ByVal pdblValorBruto As Double, ByVal pdblValorComisionPesos As Double, ByVal pdblValorIVAComisionPesos As Double, ByVal pcurValorCostosPesos As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacion_MI_ValorNetoPesos_Consultar_OyDNet_MI(pstrTipo, pdblValorBruto, pdblValorComisionPesos, pdblValorIVAComisionPesos, pcurValorCostosPesos)
            Return pcurValorCostosPesos
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Calculavalornetopesos")
            Return Nothing
        End Try
    End Function
    Public Function ActualizaEspecieOrdenes(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDOrden As Integer, ByVal plngVersion As Integer, ByVal pstrIDEspecie As String, ByVal pdtmCumplimiento As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spUpdEspeciesOrdenes_MI_OyDNet_MI(pstrTipo, pstrClase, plngIDOrden, plngVersion, pstrIDEspecie, pdtmCumplimiento, pstrUsuario)
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizaEspecieOrdenes")
            Return Nothing
        End Try
    End Function

    Public Function CumplimientoOrden_liq(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDOrden As Integer,
                                      ByVal pstrIDEspecie As String, ByVal pdblCantidadLiq? As Double,
                                      ByVal pdblCantidadOrden? As Double, ByVal pdblCantidadImportacion? As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_CumplimientoOrden_MI_OyDNet(pstrTipo, pstrClase, plngIDOrden, pstrIDEspecie, pdblCantidadLiq, pdblCantidadOrden, pdblCantidadImportacion)
            Dim variable As String
            variable = CStr(pdblCantidadLiq) + "," + CStr(pdblCantidadOrden)
            Return variable
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CumplimientoOrden_liq")
            Return Nothing
        End Try
    End Function



#End Region

#Region "ReceptoresOrdenes"
    Public Function ReceptoresOrdenesFiltrar(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrdene_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_ReceptoresOrdenes_Filtrar_MI(ptipo, pclase, pIDorden, DemeInfoSesion(pstrUsuario, "RelacionesCodBancosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoresOrdenesFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerReceptoresOrdenesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ReceptoresOrdene_MI
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ReceptoresOrdene_MI
            'e.IdComisionista = 
            'e.IdSucComisionista = 
            e.IDReceptoresOrdenes = -1
            'e.RelTecno = 
            'e.Actualizacion = 
            'e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReceptoresOrdenesPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertReceptoresOrdenes(ByVal ReceptoresOrdenes As ReceptoresOrdene_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrdenes.pstrUsuarioConexion, ReceptoresOrdenes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresOrdenes.InfoSesion = DemeInfoSesion(ReceptoresOrdenes.pstrUsuarioConexion, "InsertReceptoresOrdenes")
            'ReceptoresOrdenes.Usuario = HttpContext.Current.User.Identity.Name
            Me.DataContext.ReceptoresOrdenes_MI.InsertOnSubmit(ReceptoresOrdenes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertReceptoresOrdenes")
        End Try
    End Sub

    Public Sub UpdateReceptoresOrdenes(ByVal currentReceptoresOrdenes As ReceptoresOrdene_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentReceptoresOrdenes.pstrUsuarioConexion, currentReceptoresOrdenes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentReceptoresOrdenes.InfoSesion = DemeInfoSesion(currentReceptoresOrdenes.pstrUsuarioConexion, "UpdateReceptoresOrdenes")
            currentReceptoresOrdenes.Usuario = HttpContext.Current.User.Identity.Name
            Me.DataContext.ReceptoresOrdenes_MI.Attach(currentReceptoresOrdenes, Me.ChangeSet.GetOriginal(currentReceptoresOrdenes))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptoresOrdenes")
        End Try
    End Sub

    Public Sub DeleteReceptoresOrdenes(ByVal ReceptoresOrdenes As ReceptoresOrdene_MI)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrdenes.pstrUsuarioConexion, ReceptoresOrdenes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresOrdenes.InfoSesion = DemeInfoSesion(ReceptoresOrdenes.pstrUsuarioConexion, "DeleteReceptoresOrdenes")
            Me.DataContext.ReceptoresOrdenes_MI.Attach(ReceptoresOrdenes)
            Me.DataContext.ReceptoresOrdenes_MI.DeleteOnSubmit(ReceptoresOrdenes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptoresOrdenes")
        End Try
    End Sub
#End Region

#Region "Facturas"

    Public Function FacturasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Factura_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Facturas_MI_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "FacturasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FacturasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function FacturasConsultar(ByVal pNumero As Integer, ByVal pComitente As String, ByVal dtmDocumento As DateTime?,
                                      ByVal pstrNombreComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Factura_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Facturas_MI_Consultar(pNumero, pComitente, dtmDocumento, pstrNombreComitente, DemeInfoSesion(pstrUsuario, "BuscarFacturas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarFacturas")
            Return Nothing
        End Try
    End Function

    Public Function TraerFacturaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Factura_MI
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Factura_MI
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
            ManejarError(ex, Me.ToString(), "TraerFacturaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertFactura(ByVal Factura As Factura_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Factura.pstrUsuarioConexion, Factura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Factura.InfoSesion = DemeInfoSesion(Factura.pstrUsuarioConexion, "InsertFactura")
            Me.DataContext.Facturas_MI.InsertOnSubmit(Factura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertFactura")
        End Try
    End Sub

    Public Sub UpdateFactura(ByVal currentFactura As Factura_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentFactura.pstrUsuarioConexion, currentFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentFactura.InfoSesion = DemeInfoSesion(currentFactura.pstrUsuarioConexion, "UpdateFactura")
            Me.DataContext.Facturas_MI.Attach(currentFactura, Me.ChangeSet.GetOriginal(currentFactura))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateFactura")
        End Try
    End Sub

    Public Sub DeleteFactura(ByVal Factura As Factura_MI)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Factura.pstrUsuarioConexion, Factura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Facturas_Eliminar( pNumero,  pComitente, DemeInfoSesion(pstrUsuario, "DeleteFactura"),0).ToList
            Factura.InfoSesion = DemeInfoSesion(Factura.pstrUsuarioConexion, "DeleteFactura")
            Me.DataContext.Facturas_MI.Attach(Factura)
            Me.DataContext.Facturas_MI.DeleteOnSubmit(Factura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteFactura")
        End Try
    End Sub

    Public Function Traer_Liquidaciones_Factura(ByVal pID As Integer, ByVal pstrPrefijo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pID) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Factura_MI_Consultar(pID, pstrPrefijo, DemeInfoSesion(pstrUsuario, "Traer_Liquidaciones_Factura"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_Liquidaciones_Factura")
            Return Nothing
        End Try
    End Function

    Public Function Consultar_TotalFactura(ByVal lngIDFactura As Integer, ByVal strPrefijo As String, ByVal curTotalFactura? As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Factura_MI_TotalFactura(lngIDFactura, strPrefijo, curTotalFactura, DemeInfoSesion(pstrUsuario, "Consultar_TotalFactura"), 0)
            Dim varible = curTotalFactura
            Return CDbl(varible)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_TotalFactura")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Funcion para Anular las Facturas de MILA
    ''' </summary>
    ''' <param name="objFactura"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20121016</remarks>
    Public Function AnularFacturas(ByVal IDFacturas As Integer, ByVal Numero As Integer, ByVal Prefijo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Facturas_MI_Eliminar(IDFacturas, Numero, Prefijo, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularFacturas"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularFacturas")
            Return False
        End Try
    End Function

#End Region

#Region "ImportacionLiq_MI"

    'Public Function CargarArchivoLiquidaciones(ByVal pstrNombreCompletoArchivoLiquidaciones As String,
    '                                          ByVal pblnEstructuraActual As Boolean, ByVal pdtmDesde As DateTime, ByVal pdtmHasta As DateTime, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
    '    Try

    '        Dim objDatosRutas = FnRutasImportaciones(pstrNombreProceso, pstrUsuario)

    '        Dim directorioUsuario = objDatosRutas.RutaArchivosLocal

    '        Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
    '        Dim ret = objImportar.EvaluarLineaColombia(directorioUsuario & "\" & pstrNombreCompletoArchivoLiquidaciones, pblnEstructuraActual, pdtmDesde, pdtmHasta)

    '        Dim lstLineaComentario As New List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
    '        lstLineaComentario.Add(New A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = ret})
    '        Return lstLineaComentario

    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "CargarArchivoLiquidaciones")
    '        Return Nothing
    '    End Try

    'End Function

    'Public Function EliminarImportados( ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
    '    Dim MensajeError As String = ""
    '    Try
    '        Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
    '        MensajeError = objImportar.BorrarEncabezados()

    '        Dim lstLineaComentario As New List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
    '        lstLineaComentario.Add(New A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = MensajeError})
    '        Return lstLineaComentario

    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "EliminarImportados")
    '        Return Nothing
    '    End Try

    'End Function

    'Public Function EspecieExiste(ByVal pstrIdEspecie As String, ByVal pintIdBolsa As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS Boolean
    '    Dim MensajeError As String = ""
    '    Try
    '        Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
    '        MensajeError = objImportar.BorrarEncabezados()
    '        Return Nothing
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "EliminarImportados")
    '        Return Nothing
    '    End Try
    'End Function

    Public Function ImportacionLiqFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ImportacionLiq_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Importaciones_ImportacionLiq_MI_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "ImportacionLiqFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ImportacionLiqFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ImportacionLiqConsultar(ByVal pID As Integer, ByVal parcial As Integer, ByVal pLiquidacion? As DateTime,
                                            ByVal Comitente As String, ByVal especie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ImportacionLiq_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Importaciones_ImportacionLiq_MI_Consultar(pID, parcial, pLiquidacion, Comitente, especie, DemeInfoSesion(pstrUsuario, "BuscarImportacionLiq"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarImportacionLiq")
            Return Nothing
        End Try
    End Function

    Public Function TraerImportacionLiPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ImportacionLiq_MI
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ImportacionLiq_MI
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = 0
            e.Parcial = 0
            e.Tipo = String.Empty
            e.ClaseOrden = String.Empty
            'e.IDEspecie = 
            'e.IDOrden = 
            'e.IDComitente = 
            'e.IDOrdenante = 
            e.IDBolsa = 0
            'e.IDRueda = 
            'e.ValBolsa = 
            'e.TasaDescuento = 
            'e.TasaCompraVende = 
            'e.Modalidad = 
            'e.IndicadorEconomico = 
            'e.PuntosIndicador = 
            'e.Plazo = 
            e.Liquidacion = Now
            'e.Cumplimiento = 
            'e.Emision = 
            'e.Vencimiento = 
            'e.OtraPlaza = 
            'e.Plaza = 
            'e.IDComisionistaLocal = 
            'e.IDComisionistaOtraPlaza = 
            'e.IDCiudadOtraPlaza = 
            'e.TasaEfectiva = 
            'e.Cantidad = 
            'e.Precio = 
            'e.Transaccion = 
            'e.SubTotalLiq = 
            'e.TotalLiq = 
            'e.Comision = 
            'e.Retencion = 
            'e.Intereses = 
            'e.ValorIva = 
            'e.DiasIntereses = 
            'e.FactorComisionPactada = 
            'e.Mercado = 
            'e.NroTitulo = 
            'e.IDCiudadExpTitulo = 
            'e.PlazoOriginal = 
            'e.Aplazamiento = 
            'e.VersionPapeleta = 
            'e.EmisionOriginal = 
            'e.VencimientoOriginal = 
            'e.Impresiones = 
            'e.FormaPago = 
            'e.CtrlImpPapeleta = 
            'e.DiasVencimiento = 
            'e.PosicionPropia = 
            'e.Transaccion = 
            'e.TipoOperacion = 
            'e.DiasContado = 
            'e.Ordinaria = 
            'e.ObjetoOrdenExtraordinaria = 
            'e.NumPadre = 
            'e.ParcialPadre = 
            'e.OperacionPadre = 
            'e.DiasTramo = 
            'e.Vendido = 
            'e.Vendido = 
            'e.ValorTraslado = 
            'e.ValorBrutoCompraVencida = 
            'e.AutoRetenedor = 
            'e.Sujeto = 
            'e.PcRenEfecCompraRet = 
            'e.PcRenEfecVendeRet = 
            'e.Reinversion = 
            'e.Swap = 
            'e.NroSwap = 
            'e.Certificacion = 
            'e.DescuentoAcumula = 
            'e.PctRendimiento = 
            'e.FechaCompraVencido = 
            'e.PrecioCompraVencido = 
            'e.ConstanciaEnajenacion = 
            'e.RepoTitulo = 
            'e.ServBolsaVble = 
            'e.ServBolsaFijo = 
            'e.Traslado = 
            'e.UBICACIONTITULO = 
            'e.TipoIdentificacion = 
            'e.NroDocumento = 
            'e.ValorEntregaContraPago = 
            'e.AquienSeEnviaRetencion = 
            'e.IDBaseDias = 
            'e.TipoDeOferta = 
            'e.HoraGrabacion = 
            'e.OrigenOperacion = 
            'e.CodigoOperadorCompra = 
            'e.CodigoOperadorVende = 
            'e.IdentificacionRemate = 
            'e.ModalidaOperacion = 
            'e.IndicadorPrecio = 
            'e.PeriodoExdividendo = 
            'e.PlazoOperacionRepo = 
            'e.ValorCaptacionRepo = 
            'e.VolumenCompraRepo = 
            'e.PrecioNetoFraccion = 
            'e.VolumenNetoFraccion = 
            'e.CodigoContactoComercial = 
            'e.NroFraccionOperacion = 
            'e.IdentificacionPatrimonio1 = 
            'e.TipoidentificacionCliente2 = 
            'e.NitCliente2 = 
            'e.IdentificacionPatrimonio2 = 
            'e.TipoIdentificacionCliente3 = 
            'e.NitCliente3 = 
            'e.IdentificacionPatrimonio3 = 
            'e.IndicadorOperacion = 
            'e.BaseRetencion = 
            'e.PorcRetencion = 
            'e.BaseRetencionTranslado = 
            'e.PorcRetencionTranslado = 
            'e.PorcIvaComision = 
            'e.IndicadorAcciones = 
            'e.OperacionNegociada = 
            'e.FechaConstancia = 
            'e.ValorConstancia = 
            'e.GeneraConstancia = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.CodigoIntermediario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerImportacionLiPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertImportacionLi(ByVal ImportacionLi As ImportacionLiq_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ImportacionLi.pstrUsuarioConexion, ImportacionLi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ImportacionLi.InfoSesion = DemeInfoSesion(ImportacionLi.pstrUsuarioConexion, "InsertImportacionLi")
            Me.DataContext.ImportacionLiq_MI.InsertOnSubmit(ImportacionLi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertImportacionLi")
        End Try
    End Sub

    Public Sub UpdateImportacionLi(ByVal currentImportacionLi As ImportacionLiq_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentImportacionLi.pstrUsuarioConexion, currentImportacionLi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentImportacionLi.InfoSesion = DemeInfoSesion(currentImportacionLi.pstrUsuarioConexion, "UpdateImportacionLi")
            Me.DataContext.ImportacionLiq_MI.Attach(currentImportacionLi, Me.ChangeSet.GetOriginal(currentImportacionLi))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateImportacionLi")
        End Try
    End Sub

    'Public Function GetListaTitulos( ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of ListaTitulosValorizados)
    '    Return Me.DataContext.ListaTitulosValorizados.ToList()
    'End Function

    Public Sub DeleteImportacionLi(ByVal ImportacionLi As ImportacionLiq_MI)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ImportacionLi.pstrUsuarioConexion, ImportacionLi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Importaciones_ImportacionLiq_Eliminar( pID,  pTipo,  pClaseOrden,  pIDOrden,  pIDBolsa,  pLiquidacion,  pCumplimiento, DemeInfoSesion(pstrUsuario, "DeleteImportacionLi"),0).ToList
            ImportacionLi.InfoSesion = DemeInfoSesion(ImportacionLi.pstrUsuarioConexion, "DeleteImportacionLi")
            Me.DataContext.ImportacionLiq_MI.Attach(ImportacionLi)
            Me.DataContext.ImportacionLiq_MI.DeleteOnSubmit(ImportacionLi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteImportacionLi")
        End Try
    End Sub

    Public Function VerificarOrdenLiq(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDOrden As Integer,
                                      ByVal pstrIDEspecie As String, ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of VerificarOrdenLiq_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_verificarOrdenLiq_MI_OyDNet(pstrTipo, pstrClase, plngIDOrden, pstrIDEspecie, pstrNroDocumento).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarOrdenLiq")
            Return Nothing
        End Try
    End Function

    Public Function CumplimientoOrden(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDOrden As Integer,
                                      ByVal pstrIDEspecie As String, ByVal pdblCantidadLiq? As Double,
                                      ByVal pdblCantidadOrden? As Double, ByVal pdblCantidadImportacion? As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_CumplimientoOrden_MI_OyDNet(pstrTipo, pstrClase, plngIDOrden, pstrIDEspecie, pdblCantidadLiq, pdblCantidadOrden, pdblCantidadImportacion)
            Dim variable As String
            variable = CStr(pdblCantidadLiq) + "," + CStr(pdblCantidadOrden) + "," + CStr(pdblCantidadImportacion)
            Return variable
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarOrdenLiq")
            Return Nothing
        End Try
    End Function

    'Public Function PatrimonioTecnico(ByVal plngIDComitente As String, ByVal pdtmCorte As Date, ByVal pdblOtrosValores As Decimal, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of PatrimonioTecnico)
    '    Try
    '        Dim ret = Me.DataContext.uspValorFuturoRepoConsultar_OyDNet(plngIDComitente, pdtmCorte, pdblOtrosValores).ToList
    '        Return ret
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "VerificarOrdenLiq")
    '        Return Nothing
    '    End Try
    'End Function

#End Region

#Region "Reliquidaciones"
    Public Function ReLiquidacionesConsultar(ByVal pIDComitente As String, ByVal pOpcionFecha As Integer, ByVal pFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidacione_MI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_MI_ReLiquidaciones_MI_Consultar(pIDComitente, pOpcionFecha, pFecha, DemeInfoSesion(pstrUsuario, "BuscarLiquidaciones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarReLiquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function ReLiquidacionesActualizar(ByVal pdblComisionPesos As Double,
                                              ByVal pdblComisionPorcentaje As Double,
                                              ByVal dblTasaDolares As Double,
                                              ByVal dblTasaPesos As Double,
                                              ByVal plngIDLiquidacion As Integer,
                                              ByVal plngParcial As Integer,
                                              ByVal pstrTipo As String,
                                              ByVal pstrClaseOrden As String,
                                              ByVal pstrIDEspecie As String,
                                              ByVal plngIDOrden As Integer,
                                              ByVal plngIDComitente As String,
                                              ByVal pcurTransaccion As Double,
                                              ByVal plngIDMoneda As Integer,
                                              ByVal dblCostosOrigen As Double,
                                              ByVal plngIDTipoFecha As Integer,
                                              ByVal pdtmFecha As DateTime,
                                              ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_MI_Liquidaciones_MI_Actualizar(pdblComisionPesos,
                                                                              pdblComisionPorcentaje,
                                                                              dblTasaDolares,
                                                                              dblTasaPesos,
                                                                              plngIDLiquidacion,
                                                                              plngParcial,
                                                                              pstrTipo,
                                                                              pstrClaseOrden,
                                                                              pstrIDEspecie,
                                                                              plngIDOrden,
                                                                              plngIDComitente,
                                                                              pcurTransaccion,
                                                                              plngIDMoneda,
                                                                              dblCostosOrigen,
                                                                              plngIDTipoFecha,
                                                                              pdtmFecha,
                                                                              pstrUsuario,
                                                                              DemeInfoSesion(pstrUsuario, "BuscarLiquidaciones"),
                                                                              0)
            Return pstrUsuario
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarReLiquidaciones")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Trasladar Liquidaciones MI"

    Public Function TrasladarLiquidaciones_MI(ByVal pstrClase As String, ByVal pstrUsuario As String, ByVal plngIdSucursal As Integer, ByVal pstrContacto As String, ByVal pstrInfoConexion As String) As List(Of Comentario_MI)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim enClaseMercado As TIPOS_MERCADO

            If pstrClase = "A" Then
                enClaseMercado = TIPOS_MERCADO.A
            ElseIf pstrClase = "C" Then
                enClaseMercado = TIPOS_MERCADO.C
            ElseIf pstrClase = "T" Then
                enClaseMercado = TIPOS_MERCADO.T
            End If

            Dim objTrasladar As New clsTraslados_MI With {.gstrUser = My.User.Name}
            Dim ret = objTrasladar.FnTrasladarLiquidaciones_MI(enClaseMercado.ToString(), pstrUsuario, plngIdSucursal, pstrContacto)

            Dim lstLineaComentario As New List(Of Comentario_MI)
            lstLineaComentario.Add(New Comentario_MI With {.FechaHora = Now, .Texto = ret})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladarLiquidaciones_MI")
            Return Nothing
        End Try
    End Function

#End Region



End Class

