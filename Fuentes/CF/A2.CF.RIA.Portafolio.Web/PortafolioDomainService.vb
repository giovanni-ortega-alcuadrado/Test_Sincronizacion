
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
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio
Imports System.Web.UI.WebControls
Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports System.Threading
Imports System.Transactions
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura


'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class PortafolioDomainService
    Inherits LinqToSqlDomainService(Of CFPortafolioDatacontext)
    Public Const CSTR_NOMBREPROCESO_TITULOSACTIVOS = "TITULOSACTIVOS"
    'Private Shared SEPARATOR_FORMAT_CVS As String = System.Globalization.CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator
    Dim RETORNO As Boolean
    Dim Usuario As String = ""

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

    Public Overrides Function Submit(ByVal changeSet As OpenRiaServices.DomainServices.Server.ChangeSet) As Boolean
        Dim result As Boolean

        Using tx = New TransactionScope(
                TransactionScopeOption.Required,
                New TransactionOptions With {.IsolationLevel = IsolationLevel.ReadCommitted})

            result = MyBase.Submit(changeSet)
            If (Not Me.ChangeSet.HasError) Then
                tx.Complete()
            End If
        End Using
        Return result
    End Function

#Region "Custodia"

    Public Function CustodiaFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.Custodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_Custodia_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "CustodiaFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CustodiaFiltrar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Realiza una consulta de custodias basado en los diferentes criterios que se pasan como paramnetro.
    ''' </summary>
    ''' <param name="Filtro"></param>
    ''' <param name="pIdRecibo">Id del recibo</param>
    ''' <param name="pComitente">Codigo del cliente</param>
    ''' <param name="pdtmRecibo">Fecha del Recibo</param>
    ''' <returns>List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.Custodia)</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se incluye el parametro ByVal pdtmRecibo As Date, para que se filtre por Fecha del Recibo.
    ''' Fecha            : Febrero 20/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 20/2013 - Resultado Ok 
    ''' </history>
    Public Function CustodiaConsultar(ByVal Filtro As Integer, ByVal pIdRecibo As Integer, ByVal pComitente As String, ByVal pdtmRecibo As System.Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.Custodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_Custodia_Consultar(Filtro, pIdRecibo, pComitente, pdtmRecibo, DemeInfoSesion(pstrUsuario, "BuscarCustodia"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCustodia")
            Return Nothing
        End Try
    End Function

    Public Function TraerCustodiPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As A2.OyD.OYDServer.RIA.Web.CFPortafolio.Custodia
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New A2.OyD.OYDServer.RIA.Web.CFPortafolio.Custodia
            'e.IdComisionista = 
            'e.IdSucComisionista = 
            e.IdRecibo = -1
            'e.Comitente = 
            'e.TipoIdentificacion = 
            'e.NroDocumento = 
            'e.Nombre = 
            'e.Telefono1 = 
            'e.Direccion = 
            e.Recibo = Now.Date
            'e.Estado = 
            e.Fecha_Estado = Now.Date
            'e.ConceptoAnulacion = 
            e.Notas = String.Empty
            'e.NroLote = 
            e.Elaboracion = Now.Date
            'e.Actualizacion = 
            'e.Usuario = HttpContext.Current.User.Identity.Name
            ' e.IDCustodia =
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCustodiPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCustodi(ByVal Custodi As A2.OyD.OYDServer.RIA.Web.CFPortafolio.Custodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Custodi.pstrUsuarioConexion, Custodi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Custodi.InfoSesion = DemeInfoSesion(Custodi.pstrUsuarioConexion, "InsertCustodi")
            Me.DataContext.Custodias.InsertOnSubmit(Custodi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCustodi")
        End Try
    End Sub

    Public Sub UpdateCustodi(ByVal currentCustodi As A2.OyD.OYDServer.RIA.Web.CFPortafolio.Custodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCustodi.pstrUsuarioConexion, currentCustodi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If (currentCustodi.ConceptoAnulacion) <> String.Empty Then
                currentCustodi.InfoSesion = DemeInfoSesion(currentCustodi.pstrUsuarioConexion, "UpdateCustodi")
                Me.DataContext.Custodias.Attach(currentCustodi)
                Me.DataContext.Custodias.DeleteOnSubmit(currentCustodi)

            ElseIf Not IsNothing(currentCustodi.DescripcionEstado) Then
                If currentCustodi.DescripcionEstado.Equals("Retiro") Then
                    currentCustodi.InfoSesion = DemeInfoSesion(currentCustodi.pstrUsuarioConexion, "UpdateCustodi")
                    Me.DataContext.Custodias.Attach(currentCustodi)
                    Me.DataContext.Custodias.DeleteOnSubmit(currentCustodi)
                Else
                    currentCustodi.InfoSesion = DemeInfoSesion(currentCustodi.pstrUsuarioConexion, "UpdateCustodi")
                    Me.DataContext.Custodias.Attach(currentCustodi, Me.ChangeSet.GetOriginal(currentCustodi))
                End If
            Else
                currentCustodi.InfoSesion = DemeInfoSesion(currentCustodi.pstrUsuarioConexion, "UpdateCustodi")
                Me.DataContext.Custodias.Attach(currentCustodi, Me.ChangeSet.GetOriginal(currentCustodi))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCustodi")
        End Try
    End Sub

    Public Sub DeleteCustodi(ByVal Custodi As A2.OyD.OYDServer.RIA.Web.CFPortafolio.Custodia)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Custodi.pstrUsuarioConexion, Custodi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Custodia_Eliminar( pIdRecibo,  pComitente, DemeInfoSesion(pstrUsuario, "DeleteCustodi"),0).ToList
            Custodi.InfoSesion = DemeInfoSesion(Custodi.pstrUsuarioConexion, "DeleteCustodi")
            Me.DataContext.Custodias.Attach(Custodi)
            Me.DataContext.Custodias.DeleteOnSubmit(Custodi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCustodi")
        End Try
    End Sub

    Public Function Traer_DetalleCustodias_Custodi(ByVal Filtro As Integer, ByVal pIdRecibo As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.DetalleCustodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pIdRecibo) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_DetalleCustodias_Custodi_Consultar(Filtro, pIdRecibo, DemeInfoSesion(pstrUsuario, "Traer_DetalleCustodias_Custodi"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_DetalleCustodias_Custodi")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Operacion para consultar los datos del beneficiario de un tiulo de acuerdo a un numero de recibo de titulo.
    ''' </summary>
    ''' <param name="plngIdRecibo">numero de recibo de titulo.</param>
    ''' <returns>Lista del tipo List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.BeneficiariosCustodia)</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creado.
    ''' Fecha            : Febrero 26/2013
    ''' Pruebas Negocio  : No se le han hecho pruebas de caja blanca. 
    ''' </history> 
    Public Function Traer_BeneficiariosCustodias_Custodi(ByVal plngIdRecibo As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.BeneficiariosCustodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(plngIdRecibo) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_BeneficiariosCustodia_Consultar(plngIdRecibo, DemeInfoSesion(pstrUsuario, "Traer_BeneficiariosCustodias_Custodi"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosCustodias_Custodi")
            Return Nothing
        End Try
    End Function

#End Region

#Region "DetalleCustodias"

    Public Function DetalleCustodiasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.DetalleCustodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_DetalleCustodias_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "DetalleCustodiasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleCustodiasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DetalleCustodiasConsultar(ByVal pIdRecibo As Integer, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.DetalleCustodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_DetalleCustodias_Consultar(pIdRecibo, pComitente, DemeInfoSesion(pstrUsuario, "BuscarDetalleCustodias"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarDetalleCustodias")
            Return Nothing
        End Try
    End Function

    Public Function TraerDetalleCustodiaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As A2.OyD.OYDServer.RIA.Web.CFPortafolio.DetalleCustodia
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New A2.OyD.OYDServer.RIA.Web.CFPortafolio.DetalleCustodia
            'e.IdComisionista = 
            'e.IdSucComisionista = 
            'e.IdRecibo = 
            'e.Secuencia = 
            'e.Comitente = 
            'e.IdEspecie = 
            'e.NroTitulo = 
            'e.RentaVariable = 
            'e.IndicadorEconomico = 
            'e.PuntosIndicador = 
            'e.DiasVencimiento = 
            'e.Modalidad = 
            'e.Emision = 
            'e.Vencimiento = 
            'e.Cantidad = 
            'e.Fondo = 
            'e.TasaInteres = 
            'e.NroRefFondo = 
            'e.Retencion = 
            'e.TasaRetencion = 
            'e.ValorRetencion = 
            'e.PorcRendimiento = 
            'e.IdAgenteRetenedor = 
            'e.EstadoActual = 
            e.ObjVenta = True
            e.ObjRenovReinv = True
            e.ObjCobroIntDiv = True
            e.ObjSuscripcion = True
            e.ObjCancelacion = True
            e.Notas = String.Empty
            'e.Sellado = 
            'e.IdCuentaDeceval = 
            'e.ISIN = 
            'e.Fungible = 
            'e.TipoValor = 
            'e.FechasPagoRendimientos = 
            'e.IDDepositoExtranjero = 
            'e.IDCustodio = 
            'e.TitularCustodio = 
            'e.Reinversion = 
            'e.IDLiquidacion = 
            'e.Parcial = 
            'e.TipoLiquidacion = 
            'e.ClaseLiquidacion = 
            'e.Liquidacion = 
            'e.TotalLiq = 
            'e.TasaCompraVende = 
            'e.CumplimientoTitulo = 
            'e.TasaDescuento = 
            'e.MotivoBloqueo = 
            'e.NotasBloqueo = 
            'e.EstadoEntrada = 
            'e.EstadoSalida = 
            'e.CargadoArchivo = 
            'e.Actualizacion = 
            'e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDDetalleCustodias = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDetalleCustodiaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDetalleCustodia(ByVal DetalleCustodia As A2.OyD.OYDServer.RIA.Web.CFPortafolio.DetalleCustodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DetalleCustodia.pstrUsuarioConexion, DetalleCustodia.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DetalleCustodia.InfoSesion = DemeInfoSesion(DetalleCustodia.pstrUsuarioConexion, "InsertDetalleCustodia")
            Me.DataContext.DetalleCustodias.InsertOnSubmit(DetalleCustodia)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDetalleCustodia")
        End Try
    End Sub

    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se modifica el método para enviar los parámetros MotivoBloqueo y NotasBloqueo en NULL cuando se actualiza el registro.
    ''' Fecha            : Abril 09/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 09/2013 - Resultado Ok 
    ''' </history>
    Public Sub UpdateDetalleCustodia(ByVal currentDetalleCustodia As A2.OyD.OYDServer.RIA.Web.CFPortafolio.DetalleCustodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentDetalleCustodia.pstrUsuarioConexion, currentDetalleCustodia.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentDetalleCustodia.DescripcionEstado) Then
                If currentDetalleCustodia.DescripcionEstado.Equals("Retiro") Then
                    currentDetalleCustodia.InfoSesion = DemeInfoSesion(currentDetalleCustodia.pstrUsuarioConexion, "UpdateDetalleCustodia")
                    currentDetalleCustodia.MotivoBloqueo = Nothing
                    currentDetalleCustodia.NotasBloqueo = Nothing
                    Me.DataContext.DetalleCustodias.Attach(currentDetalleCustodia)
                    Me.DataContext.DetalleCustodias.DeleteOnSubmit(currentDetalleCustodia)
                Else
                    currentDetalleCustodia.InfoSesion = DemeInfoSesion(currentDetalleCustodia.pstrUsuarioConexion, "UpdateDetalleCustodia")
                    currentDetalleCustodia.MotivoBloqueo = Nothing
                    currentDetalleCustodia.NotasBloqueo = Nothing
                    Me.DataContext.DetalleCustodias.Attach(currentDetalleCustodia, Me.ChangeSet.GetOriginal(currentDetalleCustodia))
                End If
            Else
                currentDetalleCustodia.InfoSesion = DemeInfoSesion(currentDetalleCustodia.pstrUsuarioConexion, "UpdateDetalleCustodia")
                currentDetalleCustodia.MotivoBloqueo = Nothing
                currentDetalleCustodia.NotasBloqueo = Nothing
                Me.DataContext.DetalleCustodias.Attach(currentDetalleCustodia, Me.ChangeSet.GetOriginal(currentDetalleCustodia))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDetalleCustodia")
        End Try
    End Sub

    Public Sub DeleteDetalleCustodia(ByVal DetalleCustodia As A2.OyD.OYDServer.RIA.Web.CFPortafolio.DetalleCustodia)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DetalleCustodia.pstrUsuarioConexion, DetalleCustodia.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_DetalleCustodias_Eliminar( pIdRecibo,  pComitente, DemeInfoSesion(pstrUsuario, "DeleteDetalleCustodia"),0).ToList
            DetalleCustodia.InfoSesion = DemeInfoSesion(DetalleCustodia.pstrUsuarioConexion, "DeleteDetalleCustodia")
            Me.DataContext.DetalleCustodias.Attach(DetalleCustodia)
            Me.DataContext.DetalleCustodias.DeleteOnSubmit(DetalleCustodia)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDetalleCustodia")
        End Try
    End Sub

    Public Function Validar_CuentasDEC(ByVal plngidCuentaDeceval As Integer?, ByVal plngIdComitente As String, ByVal pstrFondo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.spValidarCuentasDEC_OyDNet(plngidCuentaDeceval, plngIdComitente, pstrFondo)
            If IsNothing(plngidCuentaDeceval) Or plngidCuentaDeceval = 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Validar_CuentasDEC")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Descripción:    Se agrega la función "Validar_TipoInversion" para validar que el tipo de inversión ingresado en el detalle
    '''                 de la custodia corresponda al mismo tipo de inversión que existe en la orden siempre y cuando esta orden exista.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          14 de Abirl
    ''' Pruebas CB:     Jorge Peña - 14 de Abirl - Resultado Ok 
    ''' </summary>
    ''' <param name="pstrTipoInversion"></param>
    ''' <param name="plngIDLiquidacion"></param>
    ''' <param name="plngParcial"></param>
    ''' <param name="pstrTipoLiquidacion"></param>
    ''' <param name="pstrClaseLiquidacion"></param>
    ''' <param name="pdtmLiquidacion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Validar_TipoInversion(ByVal pstrTipoInversion As String, ByVal plngIDLiquidacion As Integer, ByVal plngParcial As Integer, ByVal pstrTipoLiquidacion As String, ByVal pstrClaseLiquidacion As String, ByVal pdtmLiquidacion As Date, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Bolsa_DetalleCustodias_TipoInversion_Validar(pstrTipoInversion, plngIDLiquidacion, plngParcial, pstrTipoLiquidacion, pstrClaseLiquidacion, pdtmLiquidacion, DemeInfoSesion(pstrUsuario, "Validar_TipoInversion"), 0)
            Return pstrTipoInversion
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Validar_TipoInversion")
            Return Nothing
        End Try
    End Function

#End Region

#Region "CaracteristicasTitulos"

    Public Function CaracteristicasTitulosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.CaracteristicasTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_CaracteristicasTitulos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "CaracteristicasTitulosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CaracteristicasTitulosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CaracteristicasTitulosConsultar(ByVal pIdRecibo As Integer, ByVal pComitente As String, ByVal pstrEspecie As String, ByVal strNrotitulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.CaracteristicasTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_CaracteristicasTitulos_Consultar(pIdRecibo, pComitente, pstrEspecie, strNrotitulo, DemeInfoSesion(pstrUsuario, "CaracteristicasTitulosConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CaracteristicasTitulosConsultar")
            Return Nothing
        End Try
    End Function

    'Public Function TraerCustodiPorDefecto( ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS A2.OyD.OYDServer.RIA.Web.CFPortafolio.CaracteristicasTitulos
    '    Try
    '        Dim e As New A2.OyD.OYDServer.RIA.Web.CFPortafolio.CaracteristicasTitulos
    '        'e.IdComisionista = 
    '        'e.IdSucComisionista = 
    '        e.IdRecibo = -1
    '        'e.Comitente = 
    '        'e.TipoIdentificacion = 
    '        'e.NroDocumento = 
    '        'e.Nombre = 
    '        'e.Telefono1 = 
    '        'e.Direccion = 
    '        e.Recibo = Now.Date
    '        'e.Estado = 
    '        e.Fecha_Estado = Now.Date
    '        'e.ConceptoAnulacion = 
    '        'e.Notas = 
    '        'e.NroLote = 
    '        e.Elaboracion = Now.Date
    '        'e.Actualizacion = 
    '        e.Usuario = HttpContext.Current.User.Identity.Name
    '        ' e.IDCustodia =
    '        Return e
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "TraerCustodiPorDefecto")
    '        Return Nothing
    '    End Try
    'End Function

    Public Sub InsertCaracteristicasTitulos(ByVal CaracteristicasTitulos As A2.OyD.OYDServer.RIA.Web.CFPortafolio.CaracteristicasTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CaracteristicasTitulos.pstrUsuarioConexion, CaracteristicasTitulos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CaracteristicasTitulos.InfoSesion = DemeInfoSesion(CaracteristicasTitulos.pstrUsuarioConexion, "InsertCaracteristicasTitulos")
            Me.DataContext.CaracteristicasTitulos.InsertOnSubmit(CaracteristicasTitulos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCaracteristicasTitulos")
        End Try
    End Sub

    Public Sub UpdatecurrentCaracteristicasTitulos(ByVal currentCaracteristicasTitulos As A2.OyD.OYDServer.RIA.Web.CFPortafolio.CaracteristicasTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCaracteristicasTitulos.pstrUsuarioConexion, currentCaracteristicasTitulos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCaracteristicasTitulos.InfoSesion = DemeInfoSesion(currentCaracteristicasTitulos.pstrUsuarioConexion, "UpdatecurrentCaracteristicasTitulos")
            Me.DataContext.CaracteristicasTitulos.Attach(currentCaracteristicasTitulos, Me.ChangeSet.GetOriginal(currentCaracteristicasTitulos))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatecurrentCaracteristicasTitulos")
        End Try
    End Sub

    Public Sub DeleteCaracteristicasTitulos(ByVal CaracteristicasTitulos As A2.OyD.OYDServer.RIA.Web.CFPortafolio.CaracteristicasTitulos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CaracteristicasTitulos.pstrUsuarioConexion, CaracteristicasTitulos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Custodia_Eliminar( pIdRecibo,  pComitente, DemeInfoSesion(pstrUsuario, "DeleteCustodi"),0).ToList
            CaracteristicasTitulos.InfoSesion = DemeInfoSesion(CaracteristicasTitulos.pstrUsuarioConexion, "DeleteCaracteristicasTitulos")
            Me.DataContext.CaracteristicasTitulos.Attach(CaracteristicasTitulos)
            Me.DataContext.CaracteristicasTitulos.DeleteOnSubmit(CaracteristicasTitulos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCaracteristicasTitulos")
        End Try
    End Sub

    Public Function TraerISINConsultar(ByVal pISIN As String, ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.ListadoISIN)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TraerISIN(pISIN, pstrEspecie, DemeInfoSesion(pstrUsuario, "TraerISINConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerISINConsultar")
            Return Nothing
        End Try
    End Function

    ''' <history>
    ''' Modificado por   : Juan Carlos Soto Cruz.
    ''' Descripción      : Se adiciona el parametro strFondo.
    ''' Fecha            : Abril 03/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Abril 03/2013 - Resultado Ok 
    ''' </history> 
    Public Function TraerCuentasConsultar(ByVal pIdCuentaDeceval As Integer, ByVal pComitente As String, ByVal strFondo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.ListadoCuenta)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TraerCuentasDEC(pIdCuentaDeceval, pComitente, strFondo, DemeInfoSesion(pstrUsuario, "TraerCuentasConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCuentasConsultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Operacion para obtener las liquidaciones del cliente.
    ''' </summary>
    ''' <param name="plngIdComitente">Id del cliente</param>
    ''' <param name="pstrIDEspecie">Id de la Especie</param>
    ''' <returns>List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.LiquidacionesCliente)</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por   : Juan Carlos Soto Cruz.
    ''' Descripción  : Se modifica el método UpdateCustodiasObtenerTitulo para validar que las custodias que se van a actualizar son las que el usuario seleccionó en la columna actualizar estado.
    ''' Fecha        : Marzo 07/2013
    ''' Pruebas CB   : Juan Carlos Soto Cruz - Marzo 07/2013 - Resultado Ok 
    ''' </history> 
    Public Function TraerLiquidacionesClienteConsultar(ByVal plngIdComitente As String, ByVal pstrIDEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.LiquidacionesCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_CaracteristicasTitulos_ConsultarLiquidacionesCliente(plngIdComitente, pstrIDEspecie, DemeInfoSesion(pstrUsuario, "TraerLiquidacionesClienteConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerLiquidacionesClienteConsultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Operacion para ejecutar las validaciones del modulo de Caracteristicas Titulos.
    ''' </summary>
    ''' <param name="pstrTopico">Topico</param>
    ''' <param name="pbolCodigo">TRUE Indica consultar la descripcion, FALSE Indica consultar el retorno</param>
    ''' <param name="pstrValor">Valor del retorno o de la descripcion.</param>
    ''' <param name="plngIdCuentaDeceval">Id de la cuenta DECEVAL</param>
    ''' <param name="plngIdComitente">Id del Comitente</param>
    ''' <param name="pstrISIN">ISIN</param>
    ''' <param name="pstrEspecie">Especie</param>
    ''' <returns>
    ''' List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.ValidacionesCaracteristica)
    ''' </returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por   : Juan Carlos Soto Cruz.
    ''' Descripción  : Creacion.
    ''' Fecha        : Marzo 19/2013
    ''' Pruebas CB   : Juan Carlos Soto Cruz - Marzo 19/2013 - Resultado Ok 
    ''' </history> 
    Public Function ValidacionesCaracteristicasConsultar(ByVal pstrTopico As String,
                                                ByVal pbolCodigo As Boolean,
                                                ByVal pstrValor As String,
                                                ByVal plngIdCuentaDeceval As Integer,
                                                ByVal plngIdComitente As String,
                                                ByVal pstrISIN As String,
                                                ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.ValidacionesCaracteristica)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_CaracteristicasTitulos_ConsultarTipoDocumento(pstrTopico, pbolCodigo, pstrValor, plngIdCuentaDeceval, plngIdComitente,
                                                                                                   pstrISIN, pstrEspecie, DemeInfoSesion(pstrUsuario, "ValidacionesCaracteristicasConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidacionesCaracteristicasConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Entrega de Custodias"

    Public Function ExistenCustodiasClientePendientesPorAprobar(ByVal plngIdComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Boolean)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRetorno = Me.DataContext.uspOyDNet_Custodias_ExistenCustodiasClientePendientesPorAprobar(plngIdComitente,
                                                                                           pstrUsuario,
                                                                                           DemeInfoSesion(pstrUsuario, "ExistenCustodiasClientePendientesPorAprobar"),
                                                                                           ClsConstantes.GINT_ErrorPersonalizado).ToList()

            If Not objRetorno Is Nothing Then
                If objRetorno.Count >= 1 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ExistenCustodiasClientePendientesPorAprobar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta las custodias para entregar por Comitente o ISIN
    ''' </summary>
    ''' <param name="plngIdComitente">Código OyD</param>
    ''' <param name="pstrISIN">ISIN Fungible</param>
    ''' <param name="pstrUsuario">Usuario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TraerCustodiasParaEntregaComitente(ByVal plngIdComitente As String, ByVal pstrISIN As String,
                                                        ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListadoCustodiasEntrega)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Custodias_EntregaCustodias_Consultar(plngIdComitente, pstrISIN, pstrUsuario, DemeInfoSesion(pstrUsuario, "TraerCustodiasParaEntregaComitente"), ClsConstantes.GINT_ErrorPersonalizado).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCustodiasParaEntregaComitente")
            Return Nothing
        End Try

    End Function

    Public Function EntregaCustodias_Procesar(ByVal plngIDRecibo As System.Nullable(Of Integer),
                                                ByVal plngSecuencia As System.Nullable(Of Integer),
                                                ByVal pdtmDocumento As System.Nullable(Of Date),
                                                ByVal pstrNotas As String,
                                                ByVal pstrEstadoSalida As String,
                                                ByVal pdblCantidadDevolver As System.Nullable(Of Double),
                                                ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)

        Dim intRegistrosAfectados As System.Nullable(Of Integer) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Me.DataContext.uspOyDNet_EntregaCustodias_Procesar(plngIDRecibo,
                                                                plngSecuencia,
                                                                pdtmDocumento,
                                                                pstrNotas,
                                                                pstrEstadoSalida,
                                                                pdblCantidadDevolver,
                                                                pstrUsuario,
                                                                DemeInfoSesion(pstrUsuario, "GrabarMovimientoEntregaCustodia"),
                                                                ClsConstantes.GINT_ErrorPersonalizado,
                                                                intRegistrosAfectados)
            Return intRegistrosAfectados

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EntregaCustodias_Procesar")
            Return intRegistrosAfectados
        End Try

    End Function

    Public Function ActualizarDetalleCustodias(ByVal plngIDRecibo As System.Nullable(Of Integer),
                                                ByVal plngSecuencia As System.Nullable(Of Integer),
                                                ByVal pstrEstado As String,
                                                ByVal pstrNotas As String,
                                                ByVal pstrEstadoSalida As String,
                                                ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)
        Dim intNroRegistrosAfectados As System.Nullable(Of Integer) = Nothing

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Custodias_ActualizarDetalleCustodia(plngIDRecibo,
                                                                         plngSecuencia,
                                                                         pstrEstado,
                                                                         pstrNotas,
                                                                         pstrEstadoSalida,
                                                                         pstrUsuario,
                                                                         DemeInfoSesion(pstrUsuario, "ActualizarDetalleCustodias"),
                                                                         ClsConstantes.GINT_ErrorPersonalizado,
                                                                         intNroRegistrosAfectados)


            Return intNroRegistrosAfectados
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarDetalleCustodias")
            Return Nothing
        End Try


    End Function

    Public Function InsCustodias(ByVal plngIDRecibo As System.Nullable(Of Integer),
                                    ByVal pstrEstado As String,
                                    ByVal pdtmActualizacion As System.Nullable(Of Date),
                                    ByVal pstrNotas As String,
                                    ByVal plngIdComitente As String,
                                    ByVal pstrTipoIdentificacion As String,
                                    ByVal plngNroDocumento As System.Nullable(Of Decimal),
                                    ByVal pstrNombre As String,
                                    ByVal pstrTelefono1 As String,
                                    ByVal pstrDireccion As String,
                                    ByVal pdtmRecibo As System.Nullable(Of Date),
                                    ByVal pdtmEstado As System.Nullable(Of Date),
                                    ByVal pstrConceptoAnulacion As String,
                                    ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim intResultado As System.Nullable(Of Integer) = Nothing

            intResultado = Me.DataContext.uspOyDNet_Custodias_InsCustodiasOyD(plngIDRecibo,
                                                                pstrEstado,
                                                                pdtmActualizacion,
                                                                pstrNotas,
                                                                plngIdComitente,
                                                                pstrTipoIdentificacion,
                                                                plngNroDocumento,
                                                                pstrNombre,
                                                                pstrTelefono1,
                                                                pstrDireccion,
                                                                pdtmRecibo,
                                                                pdtmEstado,
                                                                pstrConceptoAnulacion,
                                                                pstrUsuario,
                                                                DemeInfoSesion(pstrUsuario, "InsCustodias"),
                                                                ClsConstantes.GINT_ErrorPersonalizado)

            Return intResultado
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsCustodias")
            Return Nothing
        End Try
    End Function

    Public Function InsDetalleCustodias(ByVal plngIDRecibo As System.Nullable(Of Integer),
                                        ByVal plngSecuencia As System.Nullable(Of Integer),
                                        ByVal pdtmActualizacion As System.Nullable(Of Date),
                                        ByVal pstrNotas As String,
                                        ByVal plngIdComitente As String,
                                        ByVal pstrIdEspecie As String,
                                        ByVal pstrNroTitulo As String,
                                        ByVal plogRentaVariable As System.Nullable(Of Boolean),
                                        ByVal pstrIndicadorEconomico As String,
                                        ByVal pdblPuntosIndicador As System.Nullable(Of Double),
                                        ByVal plngDiasVencimiento As System.Nullable(Of Integer),
                                        ByVal pstrModalidad As String,
                                        ByVal pdtmEmision As System.Nullable(Of Date),
                                        ByVal pdtmVencimiento As System.Nullable(Of Date),
                                        ByVal pdblCantidad As System.Nullable(Of Double),
                                        ByVal pstrFondo As String,
                                        ByVal pdblTasaInteres As System.Nullable(Of Double),
                                        ByVal pstrNroRefFondo As String,
                                        ByVal pdtmRetencion As System.Nullable(Of Date),
                                        ByVal pdblTasaRetencion As System.Nullable(Of Double),
                                        ByVal pdblValorRetencion As System.Nullable(Of Double),
                                        ByVal pdblPorcRendimiento As System.Nullable(Of Double),
                                        ByVal plngIdAgenteRetenedor As System.Nullable(Of Integer),
                                        ByVal pstrEstadoActual As String,
                                        ByVal plogObjVenta As System.Nullable(Of Boolean),
                                        ByVal plogObjCobroIntDiv As System.Nullable(Of Boolean),
                                        ByVal plogObjRenovReinv As System.Nullable(Of Boolean),
                                        ByVal plogObjSuscripcion As System.Nullable(Of Boolean),
                                        ByVal plogObjCancelacion As System.Nullable(Of Boolean),
                                        ByVal pdtmSellado As System.Nullable(Of Date),
                                        ByVal plngIdCuentaDeceval As System.Nullable(Of Integer),
                                        ByVal pstrISIN As String,
                                        ByVal plngFungible As System.Nullable(Of Integer),
                                        ByVal pstrTipoValor As System.Nullable(Of Char),
                                        ByVal pstrFechasPagoRendimientos As String,
                                        ByVal plngIDDepositoExtranjero As System.Nullable(Of Integer),
                                        ByVal plngIDCustodio As System.Nullable(Of Integer),
                                        ByVal pstrTitularCustodio As String,
                                        ByVal pstrReinversion As String,
                                        ByVal plngIDLiquidacion As System.Nullable(Of Integer),
                                        ByVal plngParcial As System.Nullable(Of Integer),
                                        ByVal pstrClase As String,
                                        ByVal pstrTipo As String,
                                        ByVal dtmLiquidacion As System.Nullable(Of Date),
                                        ByVal pcurTotalLiq As System.Nullable(Of Double),
                                        ByVal pdblTasaCompraVende As System.Nullable(Of Double),
                                        ByVal pdtmCumplimientoTitulo As System.Nullable(Of Date),
                                        ByVal pdblTasaDescuento As System.Nullable(Of Double),
                                        ByVal pstrMotivoBloqueo As String,
                                        ByVal pcurPrecio As System.Nullable(Of Double),
                                        ByVal plngidReciboAnterior As System.Nullable(Of Integer),
                                        ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.uspOyDNet_Custodias_InsDetalleCustodiasOyD(plngIDRecibo,
                                                                        plngSecuencia,
                                                                        pdtmActualizacion,
                                                                        pstrNotas,
                                                                        plngIdComitente,
                                                                        pstrIdEspecie,
                                                                        pstrNroTitulo,
                                                                        plogRentaVariable,
                                                                        pstrIndicadorEconomico,
                                                                        pdblPuntosIndicador,
                                                                        plngDiasVencimiento,
                                                                        pstrModalidad,
                                                                        pdtmEmision,
                                                                        pdtmVencimiento,
                                                                        pdblCantidad,
                                                                        pstrFondo,
                                                                        pdblTasaInteres,
                                                                        pstrNroRefFondo,
                                                                        pdtmRetencion,
                                                                        pdblTasaRetencion,
                                                                        pdblValorRetencion,
                                                                        pdblPorcRendimiento,
                                                                        plngIdAgenteRetenedor,
                                                                        pstrEstadoActual,
                                                                        plogObjVenta,
                                                                        plogObjCobroIntDiv,
                                                                        plogObjRenovReinv,
                                                                        plogObjSuscripcion,
                                                                        plogObjCancelacion,
                                                                        pdtmSellado,
                                                                        plngIdCuentaDeceval,
                                                                        pstrISIN,
                                                                        plngFungible,
                                                                        pstrTipoValor,
                                                                        pstrFechasPagoRendimientos,
                                                                        plngIDDepositoExtranjero,
                                                                        plngIDCustodio,
                                                                        pstrTitularCustodio,
                                                                        pstrReinversion,
                                                                        plngIDLiquidacion,
                                                                        plngParcial,
                                                                        pstrClase,
                                                                        pstrTipo,
                                                                        dtmLiquidacion,
                                                                        pcurTotalLiq,
                                                                        pdblTasaCompraVende,
                                                                        pdtmCumplimientoTitulo,
                                                                        pdblTasaDescuento,
                                                                        pstrMotivoBloqueo,
                                                                        pcurPrecio,
                                                                        plngidReciboAnterior,
                                                                        pstrUsuario,
                                                                        DemeInfoSesion(pstrUsuario, "InsDetalleCustodias"),
                                                                        ClsConstantes.GINT_ErrorPersonalizado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsDetalleCustodias")
            Return Nothing
        End Try
    End Function


    Public Function ActualizaConsecutivo(ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim intRetorno As System.Nullable(Of Integer) = Nothing
            Me.DataContext.uspOyDNet_Custodias_ActualizaConsecutivo(pstrNombreConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizaConsecutivo"), ClsConstantes.GINT_ErrorPersonalizado, intRetorno)
            Return intRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizaConsecutivo")
            Return Nothing
        End Try
    End Function

    Public Function TraerCiudadInstalacion(ByVal pstrCiudad As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.spTraerBolsa_OyDNet(pstrCiudad)
            Return pstrCiudad
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCiudadInstalacion")
            Return Nothing
        End Try

    End Function

#End Region

#Region "Actualizar Estado de Custodia"

    ''' <summary>
    ''' Operacion encargada de consultar las custodias a las cuales les sera actualizado el estado.
    ''' </summary>
    ''' <param name="IdComitente">Codigo del comitente.</param>
    ''' <param name="IdComitenteFinal"></param>
    ''' <param name="estadoInicial"></param>
    ''' <param name="estadofinal"></param>
    ''' <param name="Usuario"></param>
    ''' <returns>List(Of CustodiasObtenerTitulos)</returns>
    ''' <remarks>
    ''' Nombre	        :	CustodiasConsultarTitulos
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 29/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 29/2011 - Resultado Ok
    ''' </remarks>
    Public Function CustodiasConsultarTitulos(ByVal IdComitente As String, ByVal IdComitenteFinal As String, ByVal estadoInicial As String,
                                       ByVal estadofinal As String,
                                       ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CustodiasObtenerTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Custodias_ConsultarTitulos(IdComitente, IdComitenteFinal, estadoInicial, estadofinal, DemeInfoSesion(pstrUsuario, "CustodiasConsultarTitulos"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CustodiasConsultarTitulos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Operacion encargada de ejecutar el proceso de Actualizacion de estados de custodias.
    ''' </summary>
    ''' <param name="currentCustodiasObtenerTitulo"></param>
    ''' <remarks>
    ''' Nombre	        :	UpdateCustodiasObtenerTitulo
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 29/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 29/2011 - Resultado Ok    
    ''' </remarks>
    ''' 
    ''' Se modifica el método UpdateCustodiasObtenerTitulo
    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' ID               : 000001
    ''' Descripción      : Se modifica el método UpdateCustodiasObtenerTitulo para validar que las custodias que se van a actualizar son las que el usuario seleccionó en la columna actualizar estado.
    ''' Fecha            : Febrero 21/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Febrero 25/2013 - Resultado Ok 
    ''' </history> 
    ''' 
    Public Sub UpdateCustodiasObtenerTitulo(ByVal currentCustodiasObtenerTitulo As CustodiasObtenerTitulos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCustodiasObtenerTitulo.pstrUsuarioConexion, currentCustodiasObtenerTitulo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If currentCustodiasObtenerTitulo.ActualizarEstado.Value = True Then
                currentCustodiasObtenerTitulo.InfoSesion = DemeInfoSesion(currentCustodiasObtenerTitulo.pstrUsuarioConexion, "UpdateCustodiasObtenerTitulo")
                Me.DataContext.CustodiasObtenerTitulos.Attach(currentCustodiasObtenerTitulo, Me.ChangeSet.GetOriginal(currentCustodiasObtenerTitulo))
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCustodiasObtenerTitulo")
        End Try
    End Sub

#End Region

    Private _ListaTitulos As List(Of ReporteExcelTitulo)
    Public Property ListaTitulos() As List(Of ReporteExcelTitulo)
        Get
            Return _ListaTitulos
        End Get
        Set(ByVal value As List(Of ReporteExcelTitulo))
            _ListaTitulos = value
        End Set
    End Property
#Region "ReporteExcelTitulos"
    Public Function Traer_ReporteExcelTitulos(ByVal pstrCodigoClienteInicio As String,
                                              ByVal pstrCodigoClienteFin As String,
                                              ByVal pstrCodigoEspecieInicio As String,
                                              ByVal pstrCodigoEspecieFin As String,
                                              ByVal pdtmFechaCorte As DateTime,
                                              ByVal pstrEstadoTitulo As String,
                                              ByVal pstrConcepto As String,
                                              ByVal pstrSucursalReceptor As Integer,
                                              ByVal pstrIDReceptor As String,
                                              ByVal pstrDeposito As String,
                                              ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Boolean)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_rptCustodiasGeneral_TitulosActivos_OyDNet(pstrCodigoClienteInicio, pstrCodigoClienteFin,
                                                                                   pstrCodigoEspecieInicio, pstrCodigoEspecieFin,
                                                                                   pdtmFechaCorte, pstrEstadoTitulo, pstrConcepto,
                                                                                   pstrSucursalReceptor, pstrIDReceptor, pstrDeposito,
                                                                                   pstrUsuario, DemeInfoSesion(pstrUsuario, "ExcepcionesRDIPFiltrar"), 0).ToList
            RETORNO = False
            Usuario = pstrUsuario
            ListaTitulos = ret.ToList
            Dim grid As New DataGrid
            grid.AutoGenerateColumns = False
            CrearColumnasTitulos(grid)
            Dim strMensaje As String = "csv"
            exportDataGrid(grid, strMensaje.ToUpper())
            If RETORNO = True Then
                Return True
            Else
                Return Nothing
            End If


        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReporteExcelTitulos")
            Return Nothing
        End Try
    End Function
    Private Sub CrearColumnasTitulos(ByVal pgrid As DataGrid)
        Dim FechaCorte As New BoundColumn
        FechaCorte.HeaderText = "Fecha de Corte"
        pgrid.Columns.Add(FechaCorte)

        Dim CodigoCliente As New BoundColumn
        CodigoCliente.HeaderText = "Codigo Cliente"
        pgrid.Columns.Add(CodigoCliente)

        Dim NombreCliente As New BoundColumn
        NombreCliente.HeaderText = "Nombre Cliente"
        pgrid.Columns.Add(NombreCliente)

        Dim NemotecnicoEspecie As New BoundColumn
        NemotecnicoEspecie.HeaderText = "Nemotecnico Especie"
        pgrid.Columns.Add(NemotecnicoEspecie)

        Dim NombreEspecie As New BoundColumn
        NombreEspecie.HeaderText = "Nombre Especie"
        pgrid.Columns.Add(NombreEspecie)

        Dim ValorNomina As New BoundColumn
        ValorNomina.HeaderText = "Valor Nominal Titulo"
        pgrid.Columns.Add(ValorNomina)

        Dim NroRecibo As New BoundColumn
        NroRecibo.HeaderText = "Nro Recibo Custodia"
        pgrid.Columns.Add(NroRecibo)

        Dim NroFila As New BoundColumn
        NroFila.HeaderText = "Nro Fila Custodia"
        pgrid.Columns.Add(NroFila)

        Dim CodigoDeposito As New BoundColumn
        CodigoDeposito.HeaderText = "Codigo Deposito Valores"
        pgrid.Columns.Add(CodigoDeposito)

        Dim NombreDeposito As New BoundColumn
        NombreDeposito.HeaderText = "Nombre Deposito de Valores"
        pgrid.Columns.Add(NombreDeposito)

        Dim ISIN As New BoundColumn
        ISIN.HeaderText = "ISIN"
        pgrid.Columns.Add(ISIN)

        Dim CuentaDeposito As New BoundColumn
        CuentaDeposito.HeaderText = "Cuenta Deposito"
        pgrid.Columns.Add(CuentaDeposito)

        Dim ConceptoBloqueo As New BoundColumn
        ConceptoBloqueo.HeaderText = "Concepto Bloqueo"
        pgrid.Columns.Add(ConceptoBloqueo)

        Dim CodigoSucursal As New BoundColumn
        CodigoSucursal.HeaderText = "Codigo Sucursal Receptor"
        pgrid.Columns.Add(CodigoSucursal)

        Dim NombreSucursal As New BoundColumn
        NombreSucursal.HeaderText = "Nombre Sucursal Receptor"
        pgrid.Columns.Add(NombreSucursal)

        Dim CodigoReceptor As New BoundColumn
        CodigoReceptor.HeaderText = "Codigo Receptor Lider Cliente"
        pgrid.Columns.Add(CodigoReceptor)

        Dim NombreReceptor As New BoundColumn
        NombreReceptor.HeaderText = "Nombre Receptor Lider Cliente"
        pgrid.Columns.Add(NombreReceptor)

        Dim NroLiquidacion As New BoundColumn
        NroLiquidacion.HeaderText = "Nro Liquidacion"
        pgrid.Columns.Add(NroLiquidacion)

        Dim NroParcial As New BoundColumn
        NroParcial.HeaderText = "Nro Parcial"
        pgrid.Columns.Add(NroParcial)

        Dim FechaElaboracion As New BoundColumn
        FechaElaboracion.HeaderText = "Fecha Elaboracion Liquidacion"
        pgrid.Columns.Add(FechaElaboracion)

        Dim TipoLiquidacion As New BoundColumn
        TipoLiquidacion.HeaderText = "Tipo Liquidacion"
        pgrid.Columns.Add(TipoLiquidacion)

        Dim ClaseLiquidacion As New BoundColumn
        ClaseLiquidacion.HeaderText = "Clase Liquidacion"
        pgrid.Columns.Add(ClaseLiquidacion)

        Dim Periodicidad As New BoundColumn
        Periodicidad.HeaderText = "Periodicidad"
        pgrid.Columns.Add(Periodicidad)

        Dim TasaEmision As New BoundColumn
        TasaEmision.HeaderText = "Tasa Emision"
        pgrid.Columns.Add(TasaEmision)

        Dim IndicadorEconomico As New BoundColumn
        IndicadorEconomico.HeaderText = "Indicador Economico"
        pgrid.Columns.Add(IndicadorEconomico)

        Dim Puntos As New BoundColumn
        Puntos.HeaderText = "Puntos indicador"
        pgrid.Columns.Add(Puntos)

        Dim Emision As New BoundColumn
        Emision.HeaderText = "Fecha Emision"
        pgrid.Columns.Add(Emision)

        Dim Vencimiento As New BoundColumn
        Vencimiento.HeaderText = "Fecha Vencimiento"
        pgrid.Columns.Add(Vencimiento)

        Dim VPNMercado As New BoundColumn
        VPNMercado.HeaderText = "VPN_Mercado_Alianz"
        pgrid.Columns.Add(VPNMercado)

        'Dim VPNMercadoTotal As New BoundColumn
        'VPNMercadoTotal.HeaderText = "Total (VPN  X  V/r nominal)"
        'pgrid.Columns.Add(VPNMercadoTotal)

        Dim NroTitulo As New BoundColumn
        NroTitulo.HeaderText = "NroTitulo"
        pgrid.Columns.Add(NroTitulo)

        Dim TIRVPN As New BoundColumn
        TIRVPN.HeaderText = "TIRVPN"
        pgrid.Columns.Add(TIRVPN)

        Dim VlrLineal As New BoundColumn
        VlrLineal.HeaderText = "VlrLineal"
        pgrid.Columns.Add(VlrLineal)

        Dim TIROriginal As New BoundColumn
        TIROriginal.HeaderText = "TIROriginal"
        pgrid.Columns.Add(TIROriginal)

        Dim TIRActual As New BoundColumn
        TIRActual.HeaderText = "TIRActual"
        pgrid.Columns.Add(TIRActual)

        Dim Spread As New BoundColumn
        Spread.HeaderText = "Spread"
        pgrid.Columns.Add(Spread)

        Dim TIRSpread As New BoundColumn
        TIRSpread.HeaderText = "TIRSpread"
        pgrid.Columns.Add(TIRSpread)

        Dim ValorValoracionOyD As New BoundColumn
        ValorValoracionOyD.HeaderText = "Valor Valoracion OyD"
        pgrid.Columns.Add(ValorValoracionOyD)

        Dim Recibo As New BoundColumn
        Recibo.HeaderText = "Fecha Recibo"
        pgrid.Columns.Add(Recibo)

        Dim Elaboracion As New BoundColumn
        Elaboracion.HeaderText = "Fecha Elaboracion"
        pgrid.Columns.Add(Elaboracion)

        Dim NroDocumento As New BoundColumn
        NroDocumento.HeaderText = "Nro Documento"
        pgrid.Columns.Add(NroDocumento)

        Dim TasaCompra As New BoundColumn
        TasaCompra.HeaderText = "Tasa Compra"
        pgrid.Columns.Add(TasaCompra)

        Dim TasaEfectiva As New BoundColumn
        TasaEfectiva.HeaderText = "TasaEfectiva"
        pgrid.Columns.Add(TasaEfectiva)

        Dim Precio As New BoundColumn
        Precio.HeaderText = "Precio"
        pgrid.Columns.Add(Precio)

        Dim Liquidacion As New BoundColumn
        Liquidacion.HeaderText = "Liquidacion"
        pgrid.Columns.Add(Liquidacion)

        Dim Transaccion As New BoundColumn
        Transaccion.HeaderText = "Transaccion"
        pgrid.Columns.Add(Transaccion)

        Dim tipoDeOferta As New BoundColumn
        tipoDeOferta.HeaderText = "Tipo de Oferta"
        pgrid.Columns.Add(tipoDeOferta)

        Dim DescripcionTipoDeOferta As New BoundColumn
        DescripcionTipoDeOferta.HeaderText = "Descripcion Tipo de Oferta"
        pgrid.Columns.Add(DescripcionTipoDeOferta)

        Dim NombreIndicador As New BoundColumn
        NombreIndicador.HeaderText = "Nombre Indicador"
        pgrid.Columns.Add(NombreIndicador)


    End Sub
    Private Sub exportDataGrid(ByVal dGrid As DataGrid, ByVal strFormat As String)

        Dim strBuilder As New StringBuilder()
        Dim strLineas As New List(Of String)

        If IsNothing(dGrid) Then Return
        Dim lstFields As List(Of String) = New List(Of String)()

        'SLB20131030 Manejo del separador ","
        lstFields.Add(formatField("sep=,", strFormat, False))
        buildStringOfRow(strBuilder, lstFields, strFormat)
        strLineas.Add(strBuilder.ToString())
        lstFields.Clear()
        strBuilder.Clear()

        'If dGrid.HeaderStyle= Then  'es para saber si es la columna de el header y q le de el formato
        For Each dgcol As DataGridColumn In dGrid.Columns
            lstFields.Add(formatField(dgcol.HeaderText.ToString(), strFormat, True))
        Next
        buildStringOfRow(strBuilder, lstFields, strFormat)
        strLineas.Add(strBuilder.ToString())
        'End If
        dGrid.DataSource = ListaTitulos

        For a = 0 To ListaTitulos.Count - 1
            lstFields.Clear()
            strBuilder.Clear()
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).FechaCorte), String.Empty, ListaTitulos(a).FechaCorte)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).CodigoCliente), String.Empty, ListaTitulos(a).CodigoCliente)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NombreCliente), String.Empty, ListaTitulos(a).NombreCliente)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NemotecnicoEspecie), String.Empty, ListaTitulos(a).NemotecnicoEspecie)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NombreEspecie), String.Empty, ListaTitulos(a).NombreEspecie)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).ValorNomina), String.Empty, ListaTitulos(a).ValorNomina)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NroRecibo), String.Empty, ListaTitulos(a).NroRecibo)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NroFila), String.Empty, ListaTitulos(a).NroFila)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).CodigoDeposito), String.Empty, ListaTitulos(a).CodigoDeposito)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NombreDeposito), String.Empty, ListaTitulos(a).NombreDeposito)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).ISIN), String.Empty, ListaTitulos(a).ISIN)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).CuentaDeposito), String.Empty, ListaTitulos(a).CuentaDeposito)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).ConceptoBloqueo), String.Empty, ListaTitulos(a).ConceptoBloqueo)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).CodigoSucursal), String.Empty, ListaTitulos(a).CodigoSucursal)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NombreSucursal), String.Empty, ListaTitulos(a).NombreSucursal)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).CodigoReceptor), String.Empty, ListaTitulos(a).CodigoReceptor)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).CodigoReceptor), String.Empty, ListaTitulos(a).CodigoReceptor)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NombreReceptor), String.Empty, ListaTitulos(a).NombreReceptor)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NroLiquidacion), String.Empty, ListaTitulos(a).NroLiquidacion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NroParcial), String.Empty, ListaTitulos(a).NroParcial)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).FechaElaboracion), String.Empty, ListaTitulos(a).FechaElaboracion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).TipoLiquidacion), String.Empty, ListaTitulos(a).TipoLiquidacion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).ClaseLiquidacion), String.Empty, ListaTitulos(a).ClaseLiquidacion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Periodicidad), String.Empty, ListaTitulos(a).Periodicidad)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).TasaEmision), String.Empty, ListaTitulos(a).TasaEmision)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).IndicadorEconomico), String.Empty, ListaTitulos(a).IndicadorEconomico)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Puntos), String.Empty, ListaTitulos(a).Puntos)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Emision), String.Empty, ListaTitulos(a).Emision)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Vencimiento), String.Empty, ListaTitulos(a).Vencimiento)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).VPNMercado), String.Empty, ListaTitulos(a).VPNMercado)), strFormat, False))
            'lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).VPNMercadoTotal), String.Empty, ListaTitulos(a).VPNMercadoTotal)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NroTitulo), String.Empty, ListaTitulos(a).NroTitulo)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).TIRVPN), String.Empty, ListaTitulos(a).TIRVPN)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).VlrLineal), String.Empty, ListaTitulos(a).VlrLineal)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).TIROriginal), String.Empty, ListaTitulos(a).TIROriginal)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).TIRActual), String.Empty, ListaTitulos(a).TIRActual)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Spread), String.Empty, ListaTitulos(a).Spread)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).TIRSpread), String.Empty, ListaTitulos(a).TIRSpread)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).ValorValoracionOyD), String.Empty, ListaTitulos(a).ValorValoracionOyD)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Recibo), String.Empty, ListaTitulos(a).Recibo)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Elaboracion), String.Empty, ListaTitulos(a).Elaboracion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NroDocumento), String.Empty, ListaTitulos(a).NroDocumento)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).TasaCompra), String.Empty, ListaTitulos(a).TasaCompra)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).TasaEfectiva), String.Empty, ListaTitulos(a).TasaEfectiva)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Precio), String.Empty, ListaTitulos(a).Precio)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Liquidacion), String.Empty, ListaTitulos(a).Liquidacion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).Transaccion), String.Empty, ListaTitulos(a).Transaccion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).tipoDeOferta), String.Empty, ListaTitulos(a).tipoDeOferta)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).DescripcionTipoDeOferta), String.Empty, ListaTitulos(a).DescripcionTipoDeOferta)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTitulos(a).NombreIndicador), String.Empty, ListaTitulos(a).NombreIndicador)), strFormat, False))


            buildStringOfRow(strBuilder, lstFields, strFormat)
            strLineas.Add(strBuilder.ToString())
        Next

        RETORNO = Guardar_ArchivoServidor(CSTR_NOMBREPROCESO_TITULOSACTIVOS, Usuario, String.Format("ReporteTitulosActivos{0}.csv", Now.ToString("yyyy-mm-dd")), strLineas)

    End Sub
    Private Shared Function formatField(ByVal data As String, ByVal format As String, ByVal encabezado As Boolean) As String
        Select Case format
            Case "XML"
                Return String.Format("<Cell><Data ss:Type=""String" & """>{0}</Data></Cell>", data)
            Case "CSV"
                If encabezado = True Then
                    Return String.Format("""   {0}   """, data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                Else
                    Return String.Format("""{0}""", data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                End If
        End Select
        Return data

    End Function
    Private Shared Sub buildStringOfRow(ByVal strBuilder As StringBuilder, ByVal lstFields As List(Of String), ByVal strFormat As String)
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
        Select Case strFormat
            Case "XML"
                strBuilder.AppendLine("<Row>")
                strBuilder.AppendLine(String.Join("" & vbCrLf & "", lstFields.ToArray()))
                strBuilder.AppendLine("</Row>")
            Case "CSV"
                strBuilder.AppendLine(String.Join(CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator, lstFields.ToArray()))
                'strBuilder.AppendLine(String.Join(SEPARATOR_FORMAT_CVS, lstFields.ToArray()))
        End Select

    End Sub
    Public Function Guardar_ArchivoServidor(ByVal NombreProceso As String, ByVal pstrUsuario As String, ByVal NombreArchivo As String, ByVal Lista As List(Of String)) As Boolean
        Try
            GuardarArchivo(NombreProceso, Usuario, NombreArchivo, Lista, False)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Guardar_ArchivoServidor")
            Return False
        End Try
    End Function
#End Region

    Public Function CargarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFPortafolio.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.spA2utils_CargarCombos("")
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarCombos")
            Return Nothing
        End Try
    End Function


    'Private Sub OnCreated()
    '    'Dim channelFactoryProperty As PropertyInfo = Me.DataContext.[GetType]().GetProperty("ChannelFactory")

    '    'If IsNothing(channelFactoryProperty) Then
    '    '    Throw New InvalidOperationException("There is no 'ChannelFactory' property on the  DomainClient.")
    '    'End If

    '    'Dim factory As channel = DirectCast(channelFactoryProperty.GetValue(Me.DataContex, Nothing), ChannelFactory)

    '    Me.DataContext.CommandTimeout = 6000

    '    'If Not DesignerProperties.GetIsInDesignMode(Application.Current.RootVisual) Then
    '    '    DirectCast(DomainClient, WebDomainClient(Of IMyAppDataServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)
    '    'End If

    'End Sub

#Region "Bloquear Titulos"

    Public Function ConsultarCustodiasCliente(ByVal plngIdComitente As String, ByVal strIdEspecie As String, ByVal pstrEstadoTitulo As String, ByVal pstrISINFungible As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CustodiasCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCustodias_Bloquear_Leer_OyDNet(plngIdComitente, strIdEspecie, pstrEstadoTitulo, pstrISINFungible).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCustodiasCliente")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCustodi(ByVal Custodi As CustodiasCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Custodi.pstrUsuarioConexion, Custodi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.CustodiasCliente.InsertOnSubmit(Custodi)
            'Me.DataContext.uspCustodias_Bloquear_Liberar(Custodi.Custodia, Custodi.CantidadBloquear)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCustodi")
        End Try
    End Sub

    Public Function BloqueoCustodias_Procesar(ByVal plngIDRecibo As System.Nullable(Of Integer),
                                                ByVal plngSecuencia As System.Nullable(Of Integer),
                                                ByVal pstrMotivoBloqueo As String,
                                                ByVal pstrNotasBloqueo As String,
                                                ByVal pdblCantidad As System.Nullable(Of Double),
                                                ByVal pstrAccionAEjecutar As String,
                                                ByVal pstrUsuario As String,
                                                ByVal pdtmRecibo As System.Nullable(Of Date), ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspCustodias_Bloquear_Liberar(plngIDRecibo,
                                                         plngSecuencia,
                                                         pstrMotivoBloqueo,
                                                         pstrNotasBloqueo,
                                                         pdblCantidad,
                                                         pstrAccionAEjecutar,
                                                         pstrUsuario,
                                                         pdtmRecibo
                                                         )


        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EntregaCustodias_Procesar")
        End Try

    End Function


    Public Sub UpdateCustodi(ByVal currentCustodi As CustodiasCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCustodi.pstrUsuarioConexion, currentCustodi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            currentCustodi.pstrAccionAEjecutar = CStr(IIf(currentCustodi.EstadoActual = "Bloqueado", "L", "B"))
            currentCustodi.pstrUsuario = HttpContext.Current.User.Identity.Name
            'currentCustodi.pdtmRecibo = Now.Date
            Me.DataContext.CustodiasCliente.InsertOnSubmit(currentCustodi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCustodi")
        End Try
    End Sub

#End Region
#Region "FraccionarCustodias"
    Public Function ConsultarCustodiasClienteFraccionar(ByVal pdtmHasta As DateTime, ByVal plngIdComitente As String, ByVal strIdEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CustodiasFraccionar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCustodias_Activas_Fraccionamiento_Leer_OyDNet(pdtmHasta, plngIdComitente, strIdEspecie).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCustodiasClienteFraccionar")
            Return Nothing
        End Try
    End Function
    Public Sub UpdateCustodiFraccionar(ByVal CustodiFraccionar As CustodiasFraccionar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CustodiFraccionar.pstrUsuarioConexion, CustodiFraccionar.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CustodiFraccionar.InfoSesion = DemeInfoSesion(CustodiFraccionar.pstrUsuarioConexion, "UpdateCustodiFraccionar")
            Me.DataContext.tblCustodiasFraccionar.Attach(CustodiFraccionar, Me.ChangeSet.GetOriginal(CustodiFraccionar))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCustodiFraccionar")
        End Try
    End Sub

#End Region

#Region "SaldarPagosDECEVAL"

    Public Function SaldarPagosDECEVALFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of SaldarPagosDECEVA)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Custodias_SaldarPagosDECEVAL_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "SaldarPagosDECEVALFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "SaldarPagosDECEVALFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerSaldarPagosDECEVAPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As SaldarPagosDECEVA
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New SaldarPagosDECEVA
            'e.Tesoreria = 
            e.FechaUno = Date.Now
            e.FechaDos = Date.Now
            'e.ConsecutivoUno = 
            'e.Numero = 
            e.haElaboracion = Date.Now
            'e.ConsecutivoDos = 
            'e.Banco = 
            'e.CuentaContable = 
            'e.IDSaldarPagosDECEVAL = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerSaldarPagosDECEVAPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertSaldarPagosDECEVA(ByVal SaldarPagosDECEVA As SaldarPagosDECEVA)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,SaldarPagosDECEVA.pstrUsuarioConexion, SaldarPagosDECEVA.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            SaldarPagosDECEVA.InfoSesion = DemeInfoSesion(SaldarPagosDECEVA.pstrUsuarioConexion, "InsertSaldarPagosDECEVA")
            Me.DataContext.SaldarPagosDECEVAL.InsertOnSubmit(SaldarPagosDECEVA)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertSaldarPagosDECEVA")
        End Try
    End Sub

    Public Sub UpdateSaldarPagosDECEVA(ByVal currentSaldarPagosDECEVA As SaldarPagosDECEVA)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentSaldarPagosDECEVA.pstrUsuarioConexion, currentSaldarPagosDECEVA.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentSaldarPagosDECEVA.InfoSesion = DemeInfoSesion(currentSaldarPagosDECEVA.pstrUsuarioConexion, "UpdateSaldarPagosDECEVA")
            Me.DataContext.SaldarPagosDECEVAL.Attach(currentSaldarPagosDECEVA, Me.ChangeSet.GetOriginal(currentSaldarPagosDECEVA))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateSaldarPagosDECEVA")
        End Try
    End Sub

    Public Sub DeleteSaldarPagosDECEVA(ByVal SaldarPagosDECEVA As SaldarPagosDECEVA)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,SaldarPagosDECEVA.pstrUsuarioConexion, SaldarPagosDECEVA.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Custodias_SaldarPagosDECEVAL_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteSaldarPagosDECEVA"),0).ToList
            SaldarPagosDECEVA.InfoSesion = DemeInfoSesion(SaldarPagosDECEVA.pstrUsuarioConexion, "DeleteSaldarPagosDECEVA")
            Me.DataContext.SaldarPagosDECEVAL.Attach(SaldarPagosDECEVA)
            Me.DataContext.SaldarPagosDECEVAL.DeleteOnSubmit(SaldarPagosDECEVA)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteSaldarPagosDECEVA")
        End Try
    End Sub
#End Region
#Region "Actualizar Titulos"
    Public Function ConsultarPortafolio(ByVal pstrEspecie As String, plngIdComitente As String, pstrUsuario As String, pstrISINFungible As String, pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrInfoConexion As String) As List(Of DetalleCustodia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Consulta_TBLDetalleCustodia_OyDNet(pstrEspecie, plngIdComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarPortafolio"), 0, pstrISINFungible, pdtmFechaProceso).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPortafolio")
            Return Nothing
        End Try
    End Function
    Public Function ActualizarTitulos(ByVal pstrRegistrosDetalle As String, plngIdComitente As String, plogSplit As Boolean, pstrUsuario As String, pintIDEstadosConceptoTitulos As Int32, pstrTipoRedondeo As String, pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspActualizarTitulos_OyDNet(pstrRegistrosDetalle, plngIdComitente, plogSplit, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarTitulos"), 0, pintIDEstadosConceptoTitulos, pstrTipoRedondeo, pdtmFechaProceso)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarTitulos")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Consultar Tasaconversion"
    Public Function ConsultarTasaConversion(ByVal pdtmFechaProceso As Date, pstrMoneda As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pdblTasoConversion As Double? = 0
            Me.DataContext.uspCalculosFinancieros_ConsultarTasaConversion(pdblTasoConversion, pdtmFechaProceso, pstrMoneda, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarTasaConversion"), 0)
            Return CDbl(pdblTasoConversion)
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarTasaConversion")
            Return Nothing
        End Try
    End Function

#End Region

End Class





