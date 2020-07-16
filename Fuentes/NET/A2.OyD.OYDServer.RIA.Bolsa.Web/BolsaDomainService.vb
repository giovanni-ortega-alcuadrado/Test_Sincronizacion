
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
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura


'Implements application logic using the OyDDataContext context.
' TODO: Add your application logic to these methods or in additional methods.
' TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
' Also consider adding roles to restrict access as appropriate.
'<RequiresAuthentication> _
<EnableClientAccess()>
Partial Public Class BolsaDomainService
    Inherits LinqToSqlDomainService(Of OyDBolsaDatacontext)
    Dim DicCampos As New List(Of String)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Liquidaciones"

    Public Function LiquidacionesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidacione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "LiquidacionesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function LiquidacionesConsultar(ByVal pID As Integer, ByVal pIDComitente As String, ByVal pTipo As String, ByVal pClase As String, ByVal Liquidacion As DateTime, ByVal Cumplimiento As DateTime, ByVal pOrden As Integer, ByVal pAno As Integer, ByVal pParcial As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidacione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Consultar(pID, pIDComitente, pTipo, pClase, Liquidacion, Cumplimiento, pOrden, pAno, pParcial, DemeInfoSesion(pstrUsuario, "BuscarLiquidaciones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarLiquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function TraerLiquidacionePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Liquidacione
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Liquidacione
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.ID = 
            'e.Parcial = 
            'e.Tipo = 
            'e.ClaseOrden = 
            'e.IDEspecie = 
            'e.IDOrden = 
            'e.Prefijo = 
            'e.IDFactura = 
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

    Public Sub InsertLiquidacione(ByVal Liquidacione As Liquidacione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Liquidacione.pstrUsuarioConexion, Liquidacione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Liquidacione.InfoSesion = DemeInfoSesion(Liquidacione.pstrUsuarioConexion, "InsertLiquidacione")
            Me.DataContext.Liquidaciones.InsertOnSubmit(Liquidacione)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLiquidacione")
        End Try
    End Sub

    Public Sub UpdateLiquidacione(ByVal currentLiquidacione As Liquidacione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentLiquidacione.pstrUsuarioConexion, currentLiquidacione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentLiquidacione.InfoSesion = DemeInfoSesion(currentLiquidacione.pstrUsuarioConexion, "UpdateLiquidacione")
            Me.DataContext.Liquidaciones.Attach(currentLiquidacione, Me.ChangeSet.GetOriginal(currentLiquidacione))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLiquidacione")
        End Try
    End Sub

    Public Sub DeleteLiquidacione(ByVal Liquidacione As Liquidacione)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Liquidacione.pstrUsuarioConexion, Liquidacione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Eliminar( pID,  pIDComitente, DemeInfoSesion(pstrUsuario, "DeleteLiquidacione"),0).ToList
            Liquidacione.InfoSesion = DemeInfoSesion(Liquidacione.pstrUsuarioConexion, "DeleteLiquidacione")
            Me.DataContext.Liquidaciones.Attach(Liquidacione)
            Me.DataContext.Liquidaciones.DeleteOnSubmit(Liquidacione)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLiquidacione")
        End Try
    End Sub

    Public Function Traer_ReceptoresOrdenes_Liquidaciones(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(ptipo) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_ReceptoresOrdenes_Filtrar(ptipo, pclase, pIDorden, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresOrdenes_Liquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresOrdenes_Liquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function Traer_BeneficiariosOrdenes_Liquidaciones(ByVal pId As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiariosOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_BeneficiariosOrdenes_Filtrar(pId, DemeInfoSesion(pstrUsuario, "Traer_BeneficiariosOrdenes_Liquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosOrdenes_Liquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function Traer_EspeciesLiquidaciones(ByVal pId As Integer, ByVal plngParcial As Integer, ByVal pdtmFechaLiquidacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesLiquidacione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_EspeciesLiquidaciones_Filtrar(pId, plngParcial, pdtmFechaLiquidacion, DemeInfoSesion(pstrUsuario, "Traer_EspeciesLiquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_EspeciesLiquidaciones")
            Return Nothing
        End Try
    End Function

    'Public Function Traer_EspeciesLiquidaciones_P(ByVal pId As Integer, ByVal plngParcial As Integer, ByVal pdtmFechaLiquidacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of EspeciesLiquidacione)
    '    Try
    '        If Not IsNothing(pId) Then
    '            Dim ret = Me.DataContext.uspOyDNet_Bolsa_EspeciesLiquidaciones_Filtrar(pId, plngParcial, pdtmFechaLiquidacion, DemeInfoSesion(pstrUsuario, "Traer_EspeciesLiquidaciones"), 0).ToList
    '            Return ret
    '        Else
    '            Return Nothing
    '        End If
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "Traer_EspeciesLiquidaciones")
    '        Return Nothing
    '    End Try
    'End Function

    Public Function Traer_AplazamientosLiquidaciones(ByVal dtmliquidacion As DateTime, ByVal pId As Integer, ByVal pParcial As Integer, ByVal ptipo As String, ByVal pclase As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AplazamientosLiquidacione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_AplazamientosLiquidaciones_Filtrar(pId, dtmliquidacion, pParcial, ptipo, pclase, DemeInfoSesion(pstrUsuario, "Traer_AplazamientosLiquidaciones"), 0).ToList
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
                                                 ByVal plngParcial As Integer, ByVal pdtmLiquidacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CustodiasLiquidacione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(plngID) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_CustodiasLiquidaciones_Filtrar(plngIDComisionista, PlngIDSucComisionista, plngIDComitente, pstrIDEspecie, pstrTipo,
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

    Public Function TrasladarLiquidaciones(ByVal pstrClase As String, ByVal pstrUsuario As String, ByVal plngIdSucursal As Integer, ByVal pstrContacto As String, ByVal plogActualizarCostos As Boolean, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDBolsa.Comentario)
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

            Dim objTrasladar As New clsTraslados With {.gstrUser = My.User.Name}
            Dim ret = objTrasladar.FnTrasladarLiquidaciones(enClaseMercado.ToString(), pstrUsuario, plngIdSucursal, pstrContacto, plogActualizarCostos)  'JAG 20140311 se agrega el envio del parametro plogActualizarCostos

            Dim lstLineaComentario As New List(Of A2.OyD.OYDServer.RIA.Web.OyDBolsa.Comentario)

            For Each li In ret
                lstLineaComentario.Add(New A2.OyD.OYDServer.RIA.Web.OyDBolsa.Comentario With {.FechaHora = Now, .Texto = li.Mensaje})
            Next
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladarLiquidaciones")
            Return Nothing
        End Try
    End Function

    'JAG 20140311
    Public Function ValidarCostosBolsaxLiq_Consultar(ByVal pstrAccion As String, ByVal lngCantLiquidaciones As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim lngCantidad As System.Nullable(Of Integer)

            Me.DataContext.uspOyDNet_ValidarCostosBolsaxLiq_Consultar(pstrAccion, lngCantidad, pstrUsuario, DemeInfoSesion(pstrUsuario, "PrEjecutarTraslado"), 0)

            Return lngCantidad.Value

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarCostosBolsaxLiq_Consultar")
            Return Nothing
        End Try
    End Function
    'JAG 20140311

    Public Function Aplazamiento(ByVal pstrTipoAplazamiento As String, ByVal pstrAplazamiento As String, ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String _
                                 , ByVal plngIDLiquidacion As Integer, ByVal plngParcial As Integer, ByVal pdtmLiquidacion As DateTime, ByVal pdtmCumplimiento As DateTime,
                                 ByVal pstrUsuario As String, ByVal pstreRRor As String, ByVal intNroAplazamientos As System.Nullable(Of Integer), ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_Liquidaciones_Aplazamiento(pstrTipoAplazamiento, pstrAplazamiento, pstrClaseOrden, pstrTipoOrden, plngIDLiquidacion, plngParcial, pdtmLiquidacion _
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

    Public Function LiquidacionesConsultarvalidar(ByVal ptipo As String, ByVal pclase As String, ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesConsultar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesVeriOrd_OyDNet(ptipo, pclase, pID).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarLiquidacionesConsultarvalidar")
            Return Nothing
        End Try
    End Function

    Public Function LiquidacionesConsultarcantidad(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of consultarcantidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_TraerCumplimiento_OyDNet(ptipo, pclase, pIDorden, pEspecie).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarLiquidacionesConsultarcantidad")
            Return Nothing
        End Try
    End Function
    Public Function ReceptoresOrdenesliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesReceOrd_OyDNet(ptipo, pclase, pIDorden).ToList
            'ret.First.TipoOrden = ptipo
            'ret.First.ClaseOrden = pclase
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarReceptoresOrdenesliq")
            Return Nothing
        End Try
    End Function

    Public Function BeneficiariosOrdenesliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiariosOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesBeneOrd_OyDNet(ptipo, pclase, pIDorden).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBeneficiariosOrdenesliq")
            Return Nothing
        End Try
    End Function
    Public Function EspeciesOrdenesliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesLiquidacione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesEspecies_OyDNet(ptipo, pclase, pIDorden).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesOrdenesliq")
            Return Nothing
        End Try
    End Function

    Public Function Ordenesliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOrdenes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesOrdenes_OyDNet(ptipo, pclase, pIDorden).ToList
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
            Dim ret = Me.DataContext.spLiquidacionesVeriLiq_OyDNet(pID, pParcial, ptipo, pclase, pILiquidacion)
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
            Dim ret = Me.DataContext.spLiquidacionesOrdeCum_OyDNet(ptipo, pclase, pID, estado)
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
            Dim ret = Me.DataContext.spUpdOrdenes_OyDNet(ptipo, pclase, pID, plngVersion, pstrEstado, pdtmEstado, pstrUsuario)
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Actualizaordenestadocumplida")
            Return Nothing
        End Try
    End Function
    Public Function CumplimientoOrden_liq(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDOrden As Integer,
                                      ByVal pstrIDEspecie As String, ByVal pdblCantidadLiq? As Double,
                                      ByVal pdblCantidadOrden? As Double, ByVal pdblCantidadImportacion? As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_CumplimientoOrden_OyDNet(pstrTipo, pstrClase, plngIDOrden, pstrIDEspecie, pdblCantidadLiq, pdblCantidadOrden, pdblCantidadImportacion)
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
    Public Function ReceptoresOrdenesFiltrar(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_ReceptoresOrdenes_Filtrar(ptipo, pclase, pIDorden, DemeInfoSesion(pstrUsuario, "RelacionesCodBancosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoresOrdenesFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerReceptoresOrdenesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ReceptoresOrdene
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ReceptoresOrdene
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

    Public Sub InsertReceptoresOrdenes(ByVal ReceptoresOrdenes As ReceptoresOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrdenes.pstrUsuarioConexion, ReceptoresOrdenes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresOrdenes.InfoSesion = DemeInfoSesion(ReceptoresOrdenes.pstrUsuarioConexion, "InsertReceptoresOrdenes")
            ReceptoresOrdenes.Usuario = HttpContext.Current.User.Identity.Name
            Me.DataContext.ReceptoresOrdenes.InsertOnSubmit(ReceptoresOrdenes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertReceptoresOrdenes")
        End Try
    End Sub

    Public Sub UpdateReceptoresOrdenes(ByVal currentReceptoresOrdenes As ReceptoresOrdene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentReceptoresOrdenes.pstrUsuarioConexion, currentReceptoresOrdenes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentReceptoresOrdenes.InfoSesion = DemeInfoSesion(currentReceptoresOrdenes.pstrUsuarioConexion, "UpdateReceptoresOrdenes")
            currentReceptoresOrdenes.Usuario = HttpContext.Current.User.Identity.Name
            Me.DataContext.ReceptoresOrdenes.Attach(currentReceptoresOrdenes, Me.ChangeSet.GetOriginal(currentReceptoresOrdenes))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptoresOrdenes")
        End Try
    End Sub

    Public Sub DeleteReceptoresOrdenes(ByVal ReceptoresOrdenes As ReceptoresOrdene)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrdenes.pstrUsuarioConexion, ReceptoresOrdenes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresOrdenes.InfoSesion = DemeInfoSesion(ReceptoresOrdenes.pstrUsuarioConexion, "DeleteReceptoresOrdenes")
            Me.DataContext.ReceptoresOrdenes.Attach(ReceptoresOrdenes)
            Me.DataContext.ReceptoresOrdenes.DeleteOnSubmit(ReceptoresOrdenes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptoresOrdenes")
        End Try
    End Sub
#End Region

#Region "Facturas"

    Public Function FacturasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Factura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Facturas_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "FacturasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FacturasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function FacturasConsultar(ByVal pNumero As Integer, ByVal pComitente As String, ByVal dtmDocumento As DateTime?,
                                      ByVal pstrNombreComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Factura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Facturas_Consultar(pNumero, pComitente, dtmDocumento, HttpUtility.UrlDecode(pstrNombreComitente), DemeInfoSesion(pstrUsuario, "BuscarFacturas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarFacturas")
            Return Nothing
        End Try
    End Function

    Public Function TraerFacturaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Factura
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Factura
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

    Public Sub InsertFactura(ByVal Factura As Factura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Factura.pstrUsuarioConexion, Factura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Factura.InfoSesion = DemeInfoSesion(Factura.pstrUsuarioConexion, "InsertFactura")
            Me.DataContext.Facturas.InsertOnSubmit(Factura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertFactura")
        End Try
    End Sub

    Public Sub UpdateFactura(ByVal currentFactura As Factura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentFactura.pstrUsuarioConexion, currentFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentFactura.InfoSesion = DemeInfoSesion(currentFactura.pstrUsuarioConexion, "UpdateFactura")
            Me.DataContext.Facturas.Attach(currentFactura, Me.ChangeSet.GetOriginal(currentFactura))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateFactura")
        End Try
    End Sub

    'Public Sub DeleteFactura(ByVal Factura As Factura)
    '    Try
    '        'Me.DataContext.uspOyDNet_Maestros_Facturas_Eliminar( pNumero,  pComitente, DemeInfoSesion(pstrUsuario, "DeleteFactura"),0).ToList
    '        Factura.InfoSesion = DemeInfoSesion(pstrUsuario, "DeleteFactura")
    '        Me.DataContext.Facturas.Attach(Factura)
    '        Me.DataContext.Facturas.DeleteOnSubmit(Factura)
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "DeleteFactura")
    '    End Try
    'End Sub

    Public Function Traer_Liquidaciones_Factura(ByVal pID As Integer, ByVal pstrPrefijo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidacione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pID) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_Liquidaciones_Factura_Consultar(pID, pstrPrefijo, DemeInfoSesion(pstrUsuario, "Traer_Liquidaciones_Factura"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_Liquidaciones_Factura")
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
    ''' <remarks>SLB20121005</remarks>
    Public Function Consultar_TotalFactura(ByVal lngIDFactura As Integer, ByVal strPrefijo As String, ByVal curTotalFactura? As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Factura_TotalFactura(lngIDFactura, strPrefijo, curTotalFactura, DemeInfoSesion(pstrUsuario, "Consultar_TotalFactura"), 0)
            Dim varible = curTotalFactura
            Return CDbl(varible)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Consultar_TotalFactura")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Funcion para Anular las Facturas
    ''' </summary>
    ''' <param name="objFactura"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20121005</remarks>
    Public Function AnularFacturas(ByVal IDFacturas As Integer, ByVal Numero As Integer, ByVal Prefijo As String, ByVal Observaciones As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Facturas_Eliminar(IDFacturas, Numero, Prefijo, Observaciones, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularFacturas"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularFacturas")
            Return False
        End Try
    End Function

#End Region

#Region "Configuracion Facturas"
    Public Function ConfiguracionFacturasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionFacturas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Configuracion_Facturas_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "ConfiguracionFacturasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionFacturasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Sub UpdateConfiguracionFacturas(ByVal objConfiguracionFacturas As ConfiguracionFacturas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objConfiguracionFacturas.pstrUsuarioConexion, objConfiguracionFacturas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objConfiguracionFacturas.InfoSesion = DemeInfoSesion(objConfiguracionFacturas.pstrUsuarioConexion, "UpdateConfiguracionFacturas")
            Me.DataContext.ConfiguracionFacturas.Attach(objConfiguracionFacturas, Me.ChangeSet.GetOriginal(objConfiguracionFacturas))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConfiguracionFacturas")
        End Try
    End Sub
#End Region

#Region "Configuracion Mensajeria Cadena"
    Public Function MensajeriaCadenaFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionMensajeriaCadena)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Mensajeria_Cadena_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "ConfiguracionMensajeriaCadenaFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionFacturasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Sub UpdateMensajeriaCadena(ByVal objMensajeriaCadena As ConfiguracionMensajeriaCadena)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objMensajeriaCadena.pstrUsuarioConexion, objMensajeriaCadena.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objMensajeriaCadena.InfoSesion = DemeInfoSesion(objMensajeriaCadena.pstrUsuarioConexion, "UpdateMensajeriaCadena")
            Me.DataContext.ConfiguracionMensajeriaCadena.Attach(objMensajeriaCadena, Me.ChangeSet.GetOriginal(objMensajeriaCadena))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateMensajeriaCadena")
        End Try
    End Sub
#End Region


#Region "Envio a cadena Bancolombia"

    Public Sub InserttblPlanoPapeletasBolsa(ByVal Liquidacione As tblPlanoPapeletasBolsa)

    End Sub

    Public Sub UpdatetblPlanoPapeletasBolsa(ByVal currentLiquidacione As tblPlanoPapeletasBolsa)

    End Sub

    Public Sub DeletetblPlanoPapeletasBolsa(ByVal Liquidacione As tblPlanoPapeletasBolsa)

    End Sub

    Public Function generarCadenaFacturas(ByVal plngFacturaDesde As Integer, ByVal plngFacturaHasta As Integer, ByVal plngIdComitenteDesde As String, ByVal plngIdComitenteHasta As String, ByVal pstrTipoMensajeria As String, ByVal plngIdBolsa As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TmpFacturas_EnvioCadena)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_generarCadenaFacturas(plngFacturaDesde, plngFacturaHasta, plngIdComitenteDesde, plngIdComitenteHasta, pstrTipoMensajeria, plngIdBolsa, DemeInfoSesion(pstrUsuario, "generarCadenaFacturas"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "generarCadenaFacturas")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function generarCadenaPapeletas(ByVal plngIdComitenteDesde As String, ByVal plngIdComitenteHasta As String, ByVal pbitClienteAPT As Integer, ByVal pstrCadenaTexto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tbltmpPlanoPapeletas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_generarCadenaPapeletas(plngIdComitenteDesde, plngIdComitenteHasta, pbitClienteAPT, pstrCadenaTexto, DemeInfoSesion(pstrUsuario, "generarCadenaPapeletas"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "generarCadenaPapeletas")
            Return Nothing
        End Try
    End Function

    Public Function ConsultaClientes_APT_Papeletas(ByVal pstrClienteDesde As String, ByVal pstrClienteHasta As String, ByVal pintTipo As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblPlanoPapeletasBolsa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultaClientes_APT_Papeletas(pstrClienteDesde, pstrClienteHasta, pintTipo, DemeInfoSesion(pstrUsuario, "ConsultaClientes_APT_Papeletas"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultaClientes_APT_Papeletas")
            Return Nothing
        End Try
    End Function

#End Region

#Region "COMUNES"
    Public Function CargarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDBolsa.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.spA2utils_CargarCombos("")
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarCombosBolsa")
            Return Nothing
        End Try
    End Function
#End Region

#Region "ArregloUBICACIONTITULO"


    Public Function ArregloUBICACIONTITULO(ByVal pintNroLiq As Integer,
                                      ByVal pintNroParcial As Integer,
                                      ByVal pstrTipoOperacion As String,
                                      ByVal pstrClasePapel As String,
                                      ByVal pdtmFechaLiquidacion As Date,
                                      ByVal pstrUBICACIONTITULO As String,
                                      ByVal pdblNroCuenta As Double,
                                      ByVal pstrUsuario As String,
                                      ByVal pintErrorPersonalizado As Byte?, ByVal pstrInfoConexion As String) As ValidacionCustodiaLiq

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objResultado As New ValidacionCustodiaLiq
            Dim lngValor As System.Nullable(Of Integer)




            Me.DataContext.uspOyDNet_Bolsa_ArregloUBICACIONTITULO(pintNroLiq,
                                                                  pintNroParcial,
                                                                  pstrTipoOperacion,
                                                                  pstrClasePapel,
                                                                  pdtmFechaLiquidacion,
                                                                  pstrUBICACIONTITULO,
                                                                  pdblNroCuenta,
                                                                  pstrUsuario,
                                                                  "",
                                                                  pintErrorPersonalizado, lngValor)
            objResultado.intResultado = lngValor.Value
            objResultado.strTabla = ""
            Return objResultado

        Catch ex As Exception
            Return Nothing
            ManejarError(ex, Me.ToString(), "ArregloUBICACIONTITULO")
        End Try

    End Function

    Public Function CuentasDepositoCliente(ByVal plngID As System.Nullable(Of Integer),
                                           ByVal plngParcial As System.Nullable(Of Integer),
                                           ByVal pstrTipo As String,
                                           ByVal pstrClase As String,
                                           ByVal pdtmFechaLiquidacion As System.Nullable(Of Date),
                                           ByVal pstrDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasDepositoCliente)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.uspOyDNet_Bolsa_CuentasDepositoCliente(plngID, plngParcial, pstrTipo, pstrClase, pdtmFechaLiquidacion, pstrDeposito).ToList

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasDepositoCliente")
            Return Nothing
        End Try

    End Function

    Public Function TituloArregloUbicacion(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TituloArregloUbicacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Nothing

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TituloArregloUbicacion")
            Return Nothing
        End Try
    End Function

    Public Function ValidarSiLiquidacionPuedeModificarse(ByVal plngID As System.Nullable(Of Integer),
                                           ByVal plngParcial As System.Nullable(Of Integer),
                                           ByVal pstrTipo As String,
                                           ByVal pstrClase As String,
                                           ByVal pdtmFechaLiquidacion As System.Nullable(Of Date), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ValidacionCustodiaLiq

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objCustLiq As New ValidacionCustodiaLiq
            Dim lngValor As System.Nullable(Of Integer)

            Me.DataContext.uspOyDNet_Bolsa_ValidacionCustodiaLiq(plngID, plngParcial, pdtmFechaLiquidacion, pstrClase, pstrTipo, lngValor, objCustLiq.strTabla)
            objCustLiq.intResultado = lngValor.Value
            Return objCustLiq

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasDepositoCliente")
            Return Nothing
        End Try

    End Function



#End Region

#Region "FacturasBancaInv"

    Public Function FacturasBancaInvFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of FacturasBancaIn)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_FacturasBancaInv_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "FacturasBancaInvFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FacturasBancaInvFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function FacturasBancaInvConsultar(ByVal pPrefijo As String, ByVal pID As Integer, ByVal pDocumento As DateTime?, ByVal pIDComitente As String, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of FacturasBancaIn)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_FacturasBancaInv_Consultar(pPrefijo, pID, pDocumento, pIDComitente, HttpUtility.UrlDecode(pNombre), DemeInfoSesion(pstrUsuario, "FacturasBancaInvConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FacturasBancaInvConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerFacturasBancaInPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As FacturasBancaIn
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New FacturasBancaIn
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.AnoDocumento = 
            'e.Prefijo = 
            'e.ID = 
            e.Documento = Date.Now
            'e.IDComitente = 
            'e.TipoIdentificacion = 
            'e.NroIdentificacion = 
            'e.Nombre = 
            'e.Direccion = 
            'e.Telefono = 
            e.Estado = "P"
            'e.Impresiones = 
            'e.IVA = 
            'e.Retencion = 
            e.Fecha_Estado = Date.Now
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDFacturasBancaInv = 
            e.Maquina = System.Net.Dns.GetHostName()
            'e.EsExenta = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerFacturasBancaInPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertFacturasBancaIn(ByVal FacturasBancaIn As FacturasBancaIn)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,FacturasBancaIn.pstrUsuarioConexion, FacturasBancaIn.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            FacturasBancaIn.InfoSesion = DemeInfoSesion(FacturasBancaIn.pstrUsuarioConexion, "InsertFacturasBancaIn")
            Me.DataContext.FacturasBancaInv.InsertOnSubmit(FacturasBancaIn)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertFacturasBancaIn")
        End Try
    End Sub

    Public Sub UpdateFacturasBancaIn(ByVal currentFacturasBancaIn As FacturasBancaIn)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentFacturasBancaIn.pstrUsuarioConexion, currentFacturasBancaIn.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentFacturasBancaIn.InfoSesion = DemeInfoSesion(currentFacturasBancaIn.pstrUsuarioConexion, "UpdateFacturasBancaIn")
            Me.DataContext.FacturasBancaInv.Attach(currentFacturasBancaIn, Me.ChangeSet.GetOriginal(currentFacturasBancaIn))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateFacturasBancaIn")
        End Try
    End Sub

    Public Sub DeleteFacturasBancaIn(ByVal FacturasBancaIn As FacturasBancaIn)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,FacturasBancaIn.pstrUsuarioConexion, FacturasBancaIn.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Bolsa_FacturasBancaInv_Eliminar( pPrefijo,  pID,  pDocumento,  pIDComitente,  pNroIdentificacion,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteFacturasBancaIn"),0).ToList
            FacturasBancaIn.InfoSesion = DemeInfoSesion(FacturasBancaIn.pstrUsuarioConexion, "DeleteFacturasBancaIn")
            Me.DataContext.FacturasBancaInv.Attach(FacturasBancaIn)
            Me.DataContext.FacturasBancaInv.DeleteOnSubmit(FacturasBancaIn)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteFacturasBancaIn")
        End Try
    End Sub

    ''' <summary>
    ''' Funcion para Anular las Facturas
    ''' </summary>
    ''' <param name="objFactura"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20121009</remarks>
    Public Function AnularFacturasBancaInv(ByVal IDFacturasBancaInv As Integer, ByVal Numero As Integer, ByVal Prefijo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_FacturasBancaInv_Eliminar(IDFacturasBancaInv, Prefijo, Numero, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularFacturasBancaInv"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularFacturasBancaInv")
            Return False
        End Try
    End Function

    Public Function ConsultarCamposTablaFacBancaInv(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Retorno As String = ""
            Dim dblIvaComision = CampoTabla("1", "intIVA", "tblInstalacion", "1")
            Dim dblRetencion = CampoTabla("1", "intRteFuente", "tblInstalacion", "1")
            Retorno = dblIvaComision + "," + dblRetencion
            Return Retorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCamposTablaFacBancaInv")
            Return Nothing
        End Try
    End Function

#End Region

#Region "DetalleFacturasBancaInv"

    Public Function DetalleFacturasBancaInvFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleFacturasBancaIn)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_DetalleFacturasBancaInv_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "DetalleFacturasBancaInvFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleFacturasBancaInvFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DetalleFacturasBancaInvConsultar(ByVal plngAnoDocumento As Integer, ByVal pstrPrefijo As String, ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleFacturasBancaIn)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_DetalleFacturasBancaInv_Consultar(plngAnoDocumento, pstrPrefijo, plngID, DemeInfoSesion(pstrUsuario, "DetalleFacturasBancaInvConsultar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleFacturasBancaInvConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerDetalleFacturasBancaInPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DetalleFacturasBancaIn
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DetalleFacturasBancaIn
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.AnoDocumento = 
            'e.Prefijo = 
            'e.ID = 
            'e.Secuencia = 
            'e.Descripcion = 
            'e.Valor = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDDetalleFacturasBancaInv = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDetalleFacturasBancaInPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDetalleFacturasBancaIn(ByVal DetalleFacturasBancaIn As DetalleFacturasBancaIn)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DetalleFacturasBancaIn.pstrUsuarioConexion, DetalleFacturasBancaIn.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DetalleFacturasBancaIn.InfoSesion = DemeInfoSesion(DetalleFacturasBancaIn.pstrUsuarioConexion, "InsertDetalleFacturasBancaIn")
            Me.DataContext.DetalleFacturasBancaInv.InsertOnSubmit(DetalleFacturasBancaIn)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDetalleFacturasBancaIn")
        End Try
    End Sub

    Public Sub UpdateDetalleFacturasBancaIn(ByVal currentDetalleFacturasBancaIn As DetalleFacturasBancaIn)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentDetalleFacturasBancaIn.pstrUsuarioConexion, currentDetalleFacturasBancaIn.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDetalleFacturasBancaIn.InfoSesion = DemeInfoSesion(currentDetalleFacturasBancaIn.pstrUsuarioConexion, "UpdateDetalleFacturasBancaIn")
            Me.DataContext.DetalleFacturasBancaInv.Attach(currentDetalleFacturasBancaIn, Me.ChangeSet.GetOriginal(currentDetalleFacturasBancaIn))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDetalleFacturasBancaIn")
        End Try
    End Sub

    Public Sub DeleteDetalleFacturasBancaIn(ByVal DetalleFacturasBancaIn As DetalleFacturasBancaIn)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DetalleFacturasBancaIn.pstrUsuarioConexion, DetalleFacturasBancaIn.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Bolsa_DetalleFacturasBancaInv_Eliminar(DemeInfoSesion(pstrUsuario, "DeleteDetalleFacturasBancaIn"),0).ToList
            DetalleFacturasBancaIn.InfoSesion = DemeInfoSesion(DetalleFacturasBancaIn.pstrUsuarioConexion, "DeleteDetalleFacturasBancaIn")
            Me.DataContext.DetalleFacturasBancaInv.Attach(DetalleFacturasBancaIn)
            Me.DataContext.DetalleFacturasBancaInv.DeleteOnSubmit(DetalleFacturasBancaIn)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDetalleFacturasBancaIn")
        End Try
    End Sub
#End Region

#Region "GenerarOrdenDesdeLiquidacion"

    Public Function ConsultarLiquidacionesGapOrdenes(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesGapOrdenes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarLiquidacionesGapOrdenes_OyDNet().ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarLiquidacionesGapOrdenes")
            Return Nothing
        End Try
    End Function

    Public Sub UpdateLiquidacionesGapOrdenes(ByVal currentLiquidacionesGapOrdenes As LiquidacionesGapOrdenes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentLiquidacionesGapOrdenes.pstrUsuarioConexion, currentLiquidacionesGapOrdenes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.LiquidacionesGapOrdenes.InsertOnSubmit(currentLiquidacionesGapOrdenes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCustodi")
        End Try
    End Sub

    Public Function ValidarMaestroLista(ByVal pstrTipoOrden As String, ByVal pstrNegociacion As String, ByVal pstrFormaPago As String, ByVal pstrTipoInversion As String,
                                        ByVal pstrCanalRecepcion As String, ByVal pstrMedioVerificable As String, ByVal pstrEjecucion As String, ByVal pstrDuraccion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim plogValidarUsuarioRec? As Boolean
            'Dim pstrTipoOrden = "A", pstrNegociacion = "B", pstrFormaPago = "C", pstrTipoInversion = "D", pstrCanalRecepcion = "C", pstrMedioVerificable = "C", pstrEjecucion = "C", pstrDuraccion As String = "C"
            Dim ret = Me.DataContext.usp_OyDNet_ValidarMaestroLista(pstrTipoOrden, pstrNegociacion, pstrFormaPago, pstrTipoInversion, pstrCanalRecepcion,
                                                                    pstrMedioVerificable, pstrEjecucion, pstrDuraccion, plogValidarUsuarioRec)
            Dim variable As String
            variable = pstrTipoOrden + "," + pstrNegociacion + "," + pstrFormaPago + "," + pstrTipoInversion + "," + pstrCanalRecepcion + "," + pstrMedioVerificable + "," + pstrEjecucion + "," + pstrDuraccion + "," + CStr(plogValidarUsuarioRec)
            Return variable
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarMaestroLista")
            Return Nothing
        End Try

    End Function

    Public Function ValidarInfoLiquidacionGapOrdenes(ByVal pstrIDComitente As String, ByVal pstrDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrEstadoCliente, pstrIDReceptor As String
            Dim plogActivoCliente?, plogValidarOrdenante?, plogReceptorAsociado?, plogReceptorAsociadoEmp? As Boolean
            Dim ret = Me.DataContext.usp_OyDNet_ValidarInfoLiquidacionGapOrdenes(pstrIDComitente, pstrDescripcion, pstrEstadoCliente, plogActivoCliente,
                                                                                 plogValidarOrdenante, plogReceptorAsociado, plogReceptorAsociadoEmp, pstrIDReceptor)
            Dim variable As String
            variable = pstrDescripcion + "," + pstrEstadoCliente + "," + CStr(plogActivoCliente) + "," + CStr(plogValidarOrdenante) + "," + CStr(plogReceptorAsociado) + "," + CStr(plogReceptorAsociadoEmp) + "," + pstrIDReceptor
            Return variable
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarInfoLiquidacionGapOrdenes")
            Return Nothing
        End Try
    End Function

    Public Function SalvarDatos(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDLiquidacion As Integer, ByVal pdtmFechaLiquidacion As Date,
                                ByVal pdtmFechaCumplimiento As Date, ByVal plngIdComitente As String, ByVal plngParcial As Integer,
                                ByVal pstrDeposito As String, ByVal plngCuentaDeposito As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pGenerar As String
            Dim ret = Me.DataContext.spInsOrdenesLiq(pstrTipo, pstrClase, plngIDLiquidacion, pdtmFechaLiquidacion, pdtmFechaCumplimiento,
                                                     plngIdComitente, plngParcial, pstrDeposito, plngCuentaDeposito, pGenerar, pstrUsuario)
            Dim variable As String
            variable = pGenerar
            Return variable
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "SalvarDatos")
            Return Nothing
        End Try
    End Function

#End Region

#Region "PagosComisiones"

    Public Function ConsultarPagosComisiones(ByVal pstrFondo As String, ByVal pdtmFechaPago As DateTime, ByVal pstrConsecutivo As String,
                                              ByVal plngUltimoID As Integer, ByVal pstrProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PagosComisiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarPagosComisiones(pstrFondo, pdtmFechaPago, pstrConsecutivo, plngUltimoID, pstrProceso, pstrUsuario,
                                                                        DemeInfoSesion(pstrUsuario, "ConsultarPagosComisiones"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPagosComisiones")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarPagosComisionesAdmon(ByVal pstrClienteInicial As String, ByVal pstrClienteFinal As String, ByVal pstrEspecieInicial As String,
                                                 ByVal pstrEspecieFinal As String, ByVal pstrFondo As String, ByVal pdtmFechaCorte As DateTime,
                                                 ByVal plngUltimoID As Integer, ByVal pstrProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PagosComisiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarPagosComisionesAdmon(pstrClienteInicial, pstrClienteFinal, pstrEspecieInicial, pstrEspecieFinal,
                                                                             pstrFondo, pdtmFechaCorte, plngUltimoID, pstrProceso, pstrUsuario,
                                                                             DemeInfoSesion(pstrUsuario, "ConsultarPagosComisionesAdmon"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPagosComisionesAdmon")
            Return Nothing
        End Try
    End Function

    Public Function FacturarPagosComisiones(ByVal pstrXML As String, ByVal pstrTipoCobro As String, ByVal pdtmCorte As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim logRetorno As Nullable(Of Boolean)

            Dim ret = Me.DataContext.uspOyDNet_PagosComisiones(pstrXML, pstrUsuario, "", DemeInfoSesion(pstrUsuario, "FacturarPagosComisiones"), pstrTipoCobro, pdtmCorte, 0, logRetorno)

            If Not IsNothing(logRetorno) Then
                Return CBool(logRetorno)
            Else
                Return False
            End If

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FacturarPagosComisiones")
            Return Nothing
        End Try
    End Function

    'Public Sub InsertFacturasPagosRecibidos(ByVal currentFactura As PagosComisiones)
    '    Try

    '        Me.DataContext.PagosComisiones.InsertOnSubmit(currentFactura)
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "InsertFacturasPagosRecibidos")
    '    End Try
    'End Sub

    Public Sub UpdateFacturasPagosRecibidos(ByVal currentFactura As PagosComisiones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentFactura.pstrUsuarioConexion, currentFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.PagosComisiones.InsertOnSubmit(currentFactura)
            Me.DataContext.PagosComisiones.Attach(currentFactura, Me.ChangeSet.GetOriginal(currentFactura))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateFacturasPagosRecibidos")
        End Try
    End Sub

    Public Sub DeleteFacturasPagosRecibidos(ByVal currentFactura As PagosComisiones)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentFactura.pstrUsuarioConexion, currentFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.PagosComisiones.Attach(currentFactura)
            Me.DataContext.PagosComisiones.DeleteOnSubmit(currentFactura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteFacturasPagosRecibidos")
        End Try
    End Sub


#End Region

#Region "LiquidacionesOF"

    Public Function LiquidacionesOFFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_LiquidacionesOF_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "LiquidacionesOFFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionesOFFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function LiquidacionesOFConsultar(ByVal pID As Integer, ByVal pIDComitente As String, ByVal pTipo As String, ByVal pClase As String, ByVal Liquidacion As DateTime, ByVal Cumplimiento As DateTime, ByVal pOrden As Integer, ByVal pAno As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_LiquidacionesOF_Consultar(pID, pIDComitente, pTipo, pClase, Liquidacion, Cumplimiento, pOrden, pAno, DemeInfoSesion(pstrUsuario, "LiquidacionesOFConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionesOFConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerLiquidacionesOFPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As LiquidacionesOF
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New LiquidacionesOF
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.ID = 
            'e.Parcial = 
            'e.Tipo = 
            'e.ClaseOrden = 
            'e.IDEspecie = 
            'e.IDOrden = 
            'e.Prefijo = 
            'e.IDFactura = 
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
            e.IDNegocio = 0
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
            ManejarError(ex, Me.ToString(), "TraerLiquidacionesOFPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertLiquidacionesOF(ByVal LiquidacionesOF As LiquidacionesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,LiquidacionesOF.pstrUsuarioConexion, LiquidacionesOF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            LiquidacionesOF.InfoSesion = DemeInfoSesion(LiquidacionesOF.pstrUsuarioConexion, "InsertLiquidacionesOF")
            Me.DataContext.LiquidacionesOF.InsertOnSubmit(LiquidacionesOF)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLiquidacionesOF")
        End Try
    End Sub

    Public Sub UpdateLiquidacionesOF(ByVal currentLiquidacionesOF As LiquidacionesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentLiquidacionesOF.pstrUsuarioConexion, currentLiquidacionesOF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentLiquidacionesOF.InfoSesion = DemeInfoSesion(currentLiquidacionesOF.pstrUsuarioConexion, "UpdateLiquidacionesOF")
            Me.DataContext.LiquidacionesOF.Attach(currentLiquidacionesOF, Me.ChangeSet.GetOriginal(currentLiquidacionesOF))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateLiquidacionesOF")
        End Try
    End Sub

    Public Sub DeleteLiquidacionesOF(ByVal LiquidacionesOF As LiquidacionesOF)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,LiquidacionesOF.pstrUsuarioConexion, LiquidacionesOF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            LiquidacionesOF.InfoSesion = DemeInfoSesion(LiquidacionesOF.pstrUsuarioConexion, "DeleteLiquidacionesOF")
            Me.DataContext.LiquidacionesOF.Attach(LiquidacionesOF)
            Me.DataContext.LiquidacionesOF.DeleteOnSubmit(LiquidacionesOF)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLiquidacionesOF")
        End Try
    End Sub

    Public Function Traer_ReceptoresOrdenesOF_LiquidacionesOF(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(ptipo) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_ReceptoresOrdenesOF_Filtrar(ptipo, pclase, pIDorden, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresOrdenesOF_LiquidacionesOF"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresOrdenesOF_LiquidacionesOF")
            Return Nothing
        End Try
    End Function

    Public Function Traer_BeneficiariosOrdenesOF_LiquidacionesOF(ByVal pId As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiariosOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_BeneficiariosOrdenesOF_Filtrar(pId, DemeInfoSesion(pstrUsuario, "Traer_BeneficiariosOrdenesOF_LiquidacionesOF"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosOrdenesOF_LiquidacionesOF")
            Return Nothing
        End Try
    End Function

    Public Function Traer_EspeciesLiquidacionesOF(ByVal pId As Integer, ByVal plngParcial As Integer, ByVal pdtmFechaLiquidacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesLiquidacionesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_EspeciesLiquidacionesOF_Filtrar(pId, plngParcial, pdtmFechaLiquidacion, DemeInfoSesion(pstrUsuario, "Traer_EspeciesLiquidacionesOF"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_EspeciesLiquidacionesOF")
            Return Nothing
        End Try
    End Function

    Public Function Traer_AplazamientosLiquidacionesOF(ByVal dtmliquidacion As DateTime, ByVal pId As Integer, ByVal pParcial As Integer, ByVal ptipo As String, ByVal pclase As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AplazamientosLiquidacionesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_AplazamientosLiquidacionesOF_Filtrar(pId, dtmliquidacion, pParcial, ptipo, pclase, DemeInfoSesion(pstrUsuario, "Traer_AplazamientosLiquidacionesOF"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_AplazamientosLiquidacionesOF")
            Return Nothing
        End Try
    End Function

    Public Function Traer_CustodiasLiquidacionesOF(ByVal plngIDComisionista As Integer,
                                                   ByVal PlngIDSucComisionista As Integer,
                                                   ByVal plngIDComitente As String,
                                                   ByVal pstrIDEspecie As String,
                                                   ByVal pstrTipo As String,
                                                   ByVal pstrClaseOrden As String,
                                                   ByVal plngID As Integer,
                                                   ByVal plngParcial As Integer,
                                                   ByVal pdtmLiquidacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CustodiasLiquidacionesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(plngID) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_CustodiasLiquidacionesOF_Filtrar(plngIDComisionista,
                                                                                          PlngIDSucComisionista,
                                                                                          plngIDComitente,
                                                                                          pstrIDEspecie,
                                                                                          pstrTipo,
                                                                                          pstrClaseOrden,
                                                                                          plngID,
                                                                                          plngParcial,
                                                                                          pdtmLiquidacion,
                                                                                          DemeInfoSesion(pstrUsuario, "Traer_CustodiasLiquidacionesOF"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_CustodiasLiquidacionesOF")
            Return Nothing
        End Try
    End Function

    Public Function TrasladarLiquidacionesOF(ByVal pstrClase As String, ByVal pstrUsuario As String, ByVal plngIdSucursal As Integer, ByVal pstrContacto As String, ByVal plogActualizarCostos As Boolean, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDBolsa.Comentario)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim enClaseMercado As TIPOS_MERCADO

            Try
                Select Case pstrClase
                    Case Is = "A"
                        enClaseMercado = TIPOS_MERCADO.A
                    Case Is = "C"
                        enClaseMercado = TIPOS_MERCADO.C
                    Case Is = "T"
                        enClaseMercado = TIPOS_MERCADO.T
                End Select
            Catch ex As Exception
                ManejarError(ex, Me.ToString(), "CASE: TrasladarLiquidacionesOF")
            End Try



            Dim objTrasladar As New clsTraslados With {.gstrUser = My.User.Name}

            Dim ret = objTrasladar.FnTrasladarLiquidaciones_OF(enClaseMercado.ToString(), pstrUsuario, plngIdSucursal, pstrContacto, plogActualizarCostos) 'JAG 20140311 se agrega el envio del parametro plogActualizarCostos

            Dim lstLineaComentario As New List(Of A2.OyD.OYDServer.RIA.Web.OyDBolsa.Comentario)
            lstLineaComentario.Add(New A2.OyD.OYDServer.RIA.Web.OyDBolsa.Comentario With {.FechaHora = Now, .Texto = ret})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladarLiquidacionesOF")
            Return Nothing
        End Try
    End Function

    Public Function AplazamientoOF(ByVal pstrTipoAplazamiento As String,
                                   ByVal pstrAplazamiento As String,
                                   ByVal pstrClaseOrden As String,
                                   ByVal pstrTipoOrden As String,
                                   ByVal plngIDLiquidacion As Integer,
                                   ByVal plngParcial As Integer,
                                   ByVal pdtmLiquidacion As DateTime,
                                   ByVal pdtmCumplimiento As DateTime,
                                   ByVal pstrUsuario As String,
                                   ByVal pstreRRor As String,
                                   ByVal intNroAplazamientos As System.Nullable(Of Integer), ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_LiquidacionesOF_AplazamientoOF(pstrTipoAplazamiento,
                                                                               pstrAplazamiento,
                                                                               pstrClaseOrden,
                                                                               pstrTipoOrden,
                                                                               plngIDLiquidacion,
                                                                               plngParcial,
                                                                               pdtmLiquidacion,
                                                                               pdtmCumplimiento,
                                                                               pstrUsuario,
                                                                               pstreRRor,
                                                                               intNroAplazamientos,
                                                                               DemeInfoSesion(pstrUsuario, "AplazamientoOF"),
                                                                               0).ToString

            If pstreRRor = String.Empty Then
                Return intNroAplazamientos.ToString
            Else
                Return pstreRRor
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AplazamientoOF")
            Return Nothing
        End Try
    End Function

    Public Function LiquidacionesOFConsultarvalidar(ByVal ptipo As String, ByVal pclase As String, ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOFConsultar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesOFVeriOrd_OyDNet(ptipo, pclase, pID).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarLiquidacionesOFConsultarvalidar")
            Return Nothing
        End Try
    End Function

    Public Function LiquidacionesOFConsultarcantidad(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of consultarcantidadOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_TraerCumplimientoOF_OyDNet(ptipo, pclase, pIDorden, pEspecie).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarLiquidacionesOFConsultarcantidad")
            Return Nothing
        End Try
    End Function

    Public Function ReceptoresOrdenesOFliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesOFReceOrd_OyDNet(ptipo, pclase, pIDorden).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarReceptoresOrdenesOFliq")
            Return Nothing
        End Try
    End Function

    Public Function BeneficiariosOrdenesOFliq(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BeneficiariosOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesBeneOrdOF_OyDNet(ptipo, pclase, pIDorden).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBeneficiariosOrdenesOFliq")
            Return Nothing
        End Try
    End Function

    Public Function EspeciesOrdenesliqOF(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesLiquidacionesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesEspeciesOF_OyDNet(ptipo, pclase, pIDorden).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesOrdenesliq")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesliqOF(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesOrdenesOF_OyDNet(ptipo, pclase, pIDorden, DemeInfoSesion(pstrUsuario, "OrdenesliqOF"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscaOrdenesliqOF")
            Return Nothing
        End Try
    End Function

    Public Function verilifaliqOF(ByVal pID As Integer, ByVal pParcial As Integer, ByVal ptipo As String, ByVal pclase As String, ByVal pIDBolsa As Integer, ByVal pILiquidacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesVeriLiq_OyDNet_OF(pID, pParcial, ptipo, pclase, pIDBolsa, pILiquidacion)
            Return pID
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarverilifaliqOF")
            Return Nothing
        End Try
    End Function

    'Public Function verilifavalor(ByVal pidespecie As String, ByVal pdtmliquidacion As DateTime, ByVal valor As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS System.Nullable(Of Double)
    '    Try
    '        Dim ret = Me.DataContext.spValorEspecie_OyDNet(pidespecie, pdtmliquidacion, valor)
    '        Return valor
    '        Exit Function
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "Buscaverilifavalor")
    '        Return Nothing
    '    End Try
    'End Function


    'Public Function verificanombretarifa(ByVal pidespecie As String, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS String
    '    Try
    '        Dim ret = Me.DataContext.spDenominacionEspecie(pidespecie, pNombre)
    '        Return pNombre
    '        Exit Function
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "Buscaverificanombretarifa")
    '        Return Nothing
    '    End Try
    'End Function


    'Public Function verificadblIvacomision(ByVal pivacomision As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS System.Nullable(Of Double)
    '    Try
    '        Dim ret = Me.DataContext.spdblIvacomision(pivacomision)
    '        Return pivacomision
    '        Exit Function
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "BuscaverificadblIvacomision")
    '        Return Nothing
    '    End Try
    'End Function
    Public Function ActualizaordenestadoOF(ByVal ptipo As String, ByVal pclase As String, ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim estado As String = String.Empty
            Dim ret = Me.DataContext.spLiquidacionesOrdeCumOF_OyDNet(ptipo, pclase, pID, estado)
            Return estado
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizaordenestadoOF")
            Return Nothing
        End Try
    End Function

    Public Function ActualizaordenestadocumplidaOF(ByVal ptipo As String, ByVal pclase As String, ByVal pID As Integer, ByVal plngVersion As Integer, ByVal pstrEstado As String, ByVal pdtmEstado As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spUpdOrdenesOF_OyDNet(ptipo, pclase, pID, plngVersion, pstrEstado, pdtmEstado, pstrUsuario)
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizaordenestadocumplidaOF")
            Return Nothing
        End Try
    End Function
    Public Function CumplimientoOrdenOF_liq(ByVal pstrTipo As String,
                                             ByVal pstrClase As String,
                                             ByVal plngIDOrden As Integer,
                                             ByVal pstrIDEspecie As String,
                                             ByVal pdblCantidadLiq? As Double,
                                             ByVal pdblCantidadOrden? As Double,
                                             ByVal pdblCantidadImportacion? As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_CumplimientoOrdenOF_OyDNet(pstrTipo, pstrClase, plngIDOrden, pstrIDEspecie, pdblCantidadLiq, pdblCantidadOrden, pdblCantidadImportacion)
            Dim variable As String
            If IsNothing(pdblCantidadLiq) Then
                pdblCantidadLiq = 0
            End If
            If IsNothing(pdblCantidadOrden) Then
                pdblCantidadOrden = 0
            End If

            variable = CStr(pdblCantidadLiq) + "," + CStr(pdblCantidadOrden)
            Return variable
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CumplimientoOrdenOF_liq")
            Return Nothing
        End Try
    End Function

#End Region

#Region "RelacionEspeciesLocales"
    Public Function ConsultarEspeciesRelacionLocalExterior(ByVal pstrEspecies As String, ByVal pstrEspeciesExterior As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesRelacionLocalExterior)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_tblEspeciesRelacionLocalExterior_Consultar(pstrEspecies, pstrEspeciesExterior, DemeInfoSesion(pstrUsuario, "ConsultarEspeciesRelacionLocalExterior"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEspeciesRelacionLocalExterior")
            Return Nothing
        End Try
    End Function

    Public Function EspeciesRelacionLocalExteriorFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesRelacionLocalExterior)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_tblEspeciesRelacionLocalExterior_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "EspeciesRelacionLocalExteriorFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleFacturasBancaInvFiltrar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEspeciesRelacionLocalExterior(ByVal currenEspeciesRelacionLocalExterior As EspeciesRelacionLocalExterior)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currenEspeciesRelacionLocalExterior.pstrUsuarioConexion, currenEspeciesRelacionLocalExterior.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currenEspeciesRelacionLocalExterior.InfoSesion = DemeInfoSesion(currenEspeciesRelacionLocalExterior.pstrUsuarioConexion, "InsertEspeciesRelacionLocalExterior")
            currenEspeciesRelacionLocalExterior.Usuario = My.User.Name
            Me.DataContext.EspeciesRelacionLocalExterior.InsertOnSubmit(currenEspeciesRelacionLocalExterior)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEspeciesRelacionLocalExterior")
        End Try
    End Sub

    Public Sub UpdateEspeciesRelacionLocalExterior(ByVal currenEspeciesRelacionLocalExterior As EspeciesRelacionLocalExterior)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currenEspeciesRelacionLocalExterior.pstrUsuarioConexion, currenEspeciesRelacionLocalExterior.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.EspeciesRelacionLocalExterior.InsertOnSubmit(currenEspeciesRelacionLocalExterior)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEspecies")
        End Try
    End Sub

    Public Sub DeleteEspeciesRelacionLocalExterior(ByVal EspeciesRelacionLocalExterior As EspeciesRelacionLocalExterior)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesRelacionLocalExterior.pstrUsuarioConexion, EspeciesRelacionLocalExterior.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Bolsa_DetalleFacturasBancaInv_Eliminar(DemeInfoSesion(pstrUsuario, "DeleteDetalleFacturasBancaIn"),0).ToList
            EspeciesRelacionLocalExterior.InfoSesion = DemeInfoSesion(EspeciesRelacionLocalExterior.pstrUsuarioConexion, "DeleteEspeciesRelacionLocalExterior")
            EspeciesRelacionLocalExterior.Usuario = My.User.Name
            Me.DataContext.EspeciesRelacionLocalExterior.Attach(EspeciesRelacionLocalExterior)
            Me.DataContext.EspeciesRelacionLocalExterior.DeleteOnSubmit(EspeciesRelacionLocalExterior)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEspeciesRelacionLocalExterior")
        End Try
    End Sub
#End Region
#Region "Liquidaciones Bolsas Del Exterior"
    Public Function ConsultarLiquidaciones_BolsasDelExterior(ByVal pintIDLiquidacion As Integer, ByVal pdtmLiquidacion As System.Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidaciones_BolsasDelExteriorEntidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_tblLiquidaciones_BolsasDelExterior_Consultar(pintIDLiquidacion, pdtmLiquidacion, DemeInfoSesion(pstrUsuario, "ConsultarLiquidaciones_BolsasDelExterior"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarLiquidaciones_BolsasDelExterior")
            Return Nothing
        End Try
    End Function

    Public Function Liquidaciones_BolsasDelExteriorFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Liquidaciones_BolsasDelExteriorEntidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_tblLiquidaciones_BolsasDelExterior_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "EspeciesRelacionLocalExteriorFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleFacturasBancaInvFiltrar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertLiquidaciones_BolsasDelExterior(ByVal currenLiquidaciones_BolsasDelExterior As Liquidaciones_BolsasDelExteriorEntidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currenLiquidaciones_BolsasDelExterior.pstrUsuarioConexion, currenLiquidaciones_BolsasDelExterior.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currenLiquidaciones_BolsasDelExterior.InfoSesion = DemeInfoSesion(currenLiquidaciones_BolsasDelExterior.pstrUsuarioConexion, "InsertLiquidaciones_BolsasDelExterior")
            Me.DataContext.Liquidaciones_BolsasDelExteriorEntidad.InsertOnSubmit(currenLiquidaciones_BolsasDelExterior)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLiquidaciones_BolsasDelExterior")
        End Try
    End Sub

    Public Sub UpdateLiquidaciones_BolsasDelExterior(ByVal currenLiquidaciones_BolsasDelExterior As Liquidaciones_BolsasDelExteriorEntidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currenLiquidaciones_BolsasDelExterior.pstrUsuarioConexion, currenLiquidaciones_BolsasDelExterior.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currenLiquidaciones_BolsasDelExterior.InfoSesion = DemeInfoSesion(currenLiquidaciones_BolsasDelExterior.pstrUsuarioConexion, "UpdateLiquidaciones_BolsasDelExterior")
            Me.DataContext.Liquidaciones_BolsasDelExteriorEntidad.InsertOnSubmit(currenLiquidaciones_BolsasDelExterior)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Liquidaciones_BolsasDelExterior")
        End Try
    End Sub

#End Region
#Region "ReceptoresOrdenesOF"
    Public Function ReceptoresOrdenesOFFiltrar(ByVal ptipo As String, ByVal pclase As String, ByVal pIDorden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_ReceptoresOrdenesOF_Filtrar(ptipo, pclase, pIDorden, DemeInfoSesion(pstrUsuario, "ReceptoresOrdenesOFFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoresOrdenesOFFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerReceptoresOrdenesOFPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ReceptoresOrdenesOF
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ReceptoresOrdenesOF
            'e.IdComisionista = 
            'e.IdSucComisionista = 
            e.IDReceptoresOrdenesOF = -1
            'e.RelTecno = 
            'e.Actualizacion = 
            'e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReceptoresOrdenesOFPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertReceptoresOrdenesOF(ByVal ReceptoresOrdenesOF As ReceptoresOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrdenesOF.pstrUsuarioConexion, ReceptoresOrdenesOF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresOrdenesOF.InfoSesion = DemeInfoSesion(ReceptoresOrdenesOF.pstrUsuarioConexion, "InsertReceptoresOrdenesOF")
            ReceptoresOrdenesOF.Usuario = HttpContext.Current.User.Identity.Name
            Me.DataContext.ReceptoresOrdenesOF.InsertOnSubmit(ReceptoresOrdenesOF)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertReceptoresOrdenes")
        End Try
    End Sub

    Public Sub UpdateReceptoresOrdenesOF(ByVal currentReceptoresOrdenesOF As ReceptoresOrdenesOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentReceptoresOrdenesOF.pstrUsuarioConexion, currentReceptoresOrdenesOF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentReceptoresOrdenesOF.InfoSesion = DemeInfoSesion(currentReceptoresOrdenesOF.pstrUsuarioConexion, "UpdateReceptoresOrdenesOF")
            currentReceptoresOrdenesOF.Usuario = HttpContext.Current.User.Identity.Name
            Me.DataContext.ReceptoresOrdenesOF.Attach(currentReceptoresOrdenesOF, Me.ChangeSet.GetOriginal(currentReceptoresOrdenesOF))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptoresOrdenesOF")
        End Try
    End Sub

    Public Sub DeleteReceptoresOrdenesOF(ByVal ReceptoresOrdenesOF As ReceptoresOrdenesOF)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresOrdenesOF.pstrUsuarioConexion, ReceptoresOrdenesOF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresOrdenesOF.InfoSesion = DemeInfoSesion(ReceptoresOrdenesOF.pstrUsuarioConexion, "DeleteReceptoresOrdenesOF")
            Me.DataContext.ReceptoresOrdenesOF.Attach(ReceptoresOrdenesOF)
            Me.DataContext.ReceptoresOrdenesOF.DeleteOnSubmit(ReceptoresOrdenesOF)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptoresOrdenesOF")
        End Try
    End Sub
#End Region

End Class

