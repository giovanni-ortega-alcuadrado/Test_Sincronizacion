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
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesOF
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()> _
Partial Public Class OYDPLUSOrdenesOFDomainService
    Inherits LinqToSqlDomainService(Of OyDPLUSOrdenesOFDataContext)

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

            ReceptoresOrden.InfoSesion = DemeInfoSesion(strUsuario, "InsertReceptoresOrden")
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

#End Region

#Region "OYDPLUS"

    Public Sub InsertOrdenOYDPlus(ByVal Orden As OrdenOYDPLUSOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Orden.pstrUsuarioConexion, Orden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenOYDPLUSOF.InsertOnSubmit(Orden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrdenOYDPlus")
        End Try
    End Sub

    Public Sub UpdateOrdenOYDPlus(ByVal currentOrden As OrdenOYDPLUSOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOrden.pstrUsuarioConexion, currentOrden.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.OrdenOYDPLUSOF.InsertOnSubmit(currentOrden)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrdenOYDPlus")
        End Try
    End Sub

    Public Function OYDPLUS_OrdenPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As OrdenOYDPLUSOF
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret As New OrdenOYDPLUSOF
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
                                                ByVal pstrMaquina As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidaciones)
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
                                                            pstrMaquina, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ValidarIngresoOrden"), 0, plogGuardarComoPlantilla, NombrePlantilla, pstrBrokenTrader, pstrEntidad, pstrEstrategia, pstrUsuarioWindows)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ValidarIngresoOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_FiltrarOrden(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUSOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FiltrarOrden(pstrEstado, pstrFiltro, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_FiltrarOrden"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_FiltrarOrden")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarOrdenesCruzadasUsuario(ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUSOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FiltrarOrden("C", String.Empty, pstrModulo, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrdenesCruzadasUsuario"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OYDPLUS_ConsultarOrdenesCruzadasUsuario")
            Return Nothing
        End Try
    End Function

    Public Function OYDPLUS_ConsultarOrden(ByVal pstrModulo As String, ByVal pstrEstado As String, ByVal pintNroOrden As Integer, ByVal pintVersion As Integer, ByVal pstrEstadoOrden As String, ByVal pstrReceptor As String, ByVal pstrTipoOrden As String, _
                                           ByVal pstrTipoNegocio As String, ByVal pstrTipoOperacion As String, ByVal pstrTipoProducto As String, ByVal pstrIDComitente As String, _
                                           ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUSOF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarOrden(pstrModulo, pstrEstado, pintNroOrden, pintVersion, pstrEstadoOrden, pstrReceptor, pstrTipoOrden, pstrTipoNegocio, pstrTipoOperacion, pstrTipoProducto, pstrIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrden"), 0).ToList
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

    Public Function OYDPLUS_ConsultarMejorPrecioEspecie_Orden(ByVal pstrTipoOperacion As String, ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MejorPrecioEspecieOrden)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarMejorPrecioEspecie(pstrTipoOperacion, pstrEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarMejorPrecioEspecie_Orden"), ClsConstantes.GINT_ErrorPersonalizado).ToList
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

    Public Function OYDPLUS_ConsultarOrdenesCruzadas(ByVal pintIDOrden As Integer, ByVal plogOrdenOriginal As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblOrdenesCruzadas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarOrdenesCruzadas(pintIDOrden, plogOrdenOriginal, pstrUsuario, DemeInfoSesion(pstrUsuario, "OYDPLUS_ConsultarOrdenesCruzadas"), ClsConstantes.GINT_ErrorPersonalizado).ToList
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

    Public Function OYDPLUS_OrdenesPlantillasConsultar(ByVal pstrFiltro As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenOYDPLUSOF)
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
        Dim L2SDC As New OyDPLUSOrdenesOFDataContext
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