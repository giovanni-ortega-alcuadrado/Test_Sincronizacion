
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
Imports System.Threading.Tasks
Imports System.Transactions
Imports System.Web.Configuration
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

'Implements application logic using the OyDDataContext context.
' TODO: Add your application logic to these methods or in additional methods.
' TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
' Also consider adding roles to restrict access as appropriate.
'<RequiresAuthentication> _
<EnableClientAccess()> _
Partial Public Class OyDPLUSGarantiasDomainService
    Inherits LinqToSqlDomainService(Of OyDPLUSGarantiasDataContext)


#Region "Constructores"
    ''' <summary>
    ''' Asignar la variable time out
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

    'JAEZ 20161117 funcion para tomar el tipo de modulo desde en web.config, que nos cambia el applicationaName 20161001
    Public Function Modulos() As String
        Try
            Dim strModuloPantalla As String = String.Empty
            Dim strDelimitador As Char = CChar(",")

            Dim strModulos As String = WebConfigurationManager.AppSettings("Modulos")

            If Not String.IsNullOrEmpty(strModulos) Then

                Dim strLista() As String = strModulos.Split(strDelimitador)

                For Each UnicoModulo In strLista
                    If CBool(InStr(1, UnicoModulo, "[GARANTIAS]")) Then
                        strModuloPantalla = Right(UnicoModulo, Len(UnicoModulo) - InStrRev(UnicoModulo, "="))
                    End If
                Next

            End If

            Return strModuloPantalla
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


#End Region


#Region "General"

    ''' <summary>
    ''' Procedimiento para la carga de los combos de la pantalla de garantías
    ''' </summary>
    ''' <param name="pstrListaCombos">Si es una lista de combos específica</param>
    ''' <param name="pstrCondicionTexto">Condición varchar</param>
    ''' <param name="pintCondicionNumerica">Condición de tipo numérico</param>
    ''' <param name="pstrUsuario">Usuario que ejecuta el proceso</param>
    ''' <returns>List(Of ItemComboGarantia)</returns>
    ''' <remarks>SV20160425</remarks>
    Public Function Garantias_CargarCombos(ByVal pstrListaCombos As String, ByVal pstrCondicionTexto As String, ByVal pintCondicionNumerica As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ItemComboGarantia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.uspOyDNet_Garantias_CargarCombos(pstrListaCombos, pstrCondicionTexto, pintCondicionNumerica, pstrUsuario)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Garantias_CargarCombos")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Overrides"

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

#End Region

#Region "Visor de Garantias"
    ''' <summary>
    ''' Consultar la lista de operaciones para el visor de garantias
    ''' </summary>
    ''' <param name="pIDCliente"></param>
    ''' <param name="pIDEspecie"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Query(HasSideEffects:=True)> _
    Public Function Garantias_Operaciones_Consultar(pIDCliente As String, _
                                                    pIDEspecie As String, _
                                                    pintIDLiquidacion As Integer, _
                                                    plngParcial As Integer, _
                                                    pstrTipo As String, _
                                                    pstrClase As String, _
                                                    pdtmLiquidacion As DateTime, _
                                                    pintTipoMercado As Integer, _
                                                    pstrTipoGarantia As String, _
                                                    plogActualizarDatos As Boolean, _
                                                    plogEnviarCorreos As Boolean, _
                                                    ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of VisorDeGarantias)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            'JAEZ 20161117 Crear nueva instancia
            Me.DataContext.ActualizarModulo(Modulos())



            Dim ret = Me.DataContext.uspOyDNet_Garantias_Operaciones_Consultar(pIDCliente,
                                                                             pIDEspecie,
                                                                             pstrUsuario,
                                                                             DemeInfoSesion(pstrUsuario, "Garantias_Operaciones_Consultar"),
                                                                             ClsConstantes.GINT_ErrorPersonalizado,
                                                                             pintIDLiquidacion,
                                                                             plngParcial,
                                                                             pstrTipo,
                                                                             pstrClase,
                                                                             pdtmLiquidacion,
                                                                             pintTipoMercado,
                                                                             pstrTipoGarantia,
                                                                             plogActualizarDatos,
                                                                             plogEnviarCorreos).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Garantias_Operaciones_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Permitir editar la clase visor de garantias
    ''' </summary>
    ''' <param name="visor"></param>
    ''' <remarks></remarks>
    Public Sub UpdateVisorDeGarantias(visor As VisorDeGarantias)
        Throw New NotImplementedException
    End Sub

    ''' <summary>
    ''' Consultar los títulos del visor de garantias
    ''' </summary>
    ''' <remarks></remarks>
    <Query(HasSideEffects:=True)> _
    Public Function Garantias_Custodias_Consultar(pstrIDCliente As String, _
                                                    pintIDLiquidacion As Integer, _
                                                    plngParcial As Integer, _
                                                    pstrTipo As String, _
                                                    pstrClase As String, _
                                                    pdtmLiquidacion As DateTime, _
                                                    pstrEstadoTitulo As String, _
                                                    pintTipoMercado As Integer, _
                                                    pstrIdEspecie As String, _
                                                    pdblCantidad As Double, _
                                                    ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IEnumerable(Of TitulosGarantia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Garantias_Custodias_Consultar(pstrIDCliente,
                                                                             pintIDLiquidacion,
                                                                             plngParcial,
                                                                             pstrTipo,
                                                                             pstrClase,
                                                                             pdtmLiquidacion,
                                                                             pstrEstadoTitulo,
                                                                             pintTipoMercado,
                                                                             pstrIdEspecie,
                                                                             pdblCantidad,
                                                                             pstrUsuario,
                                                                             DemeInfoSesion(pstrUsuario, "Garantias_Custodias_Consultar"),
                                                                             ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Garantias_Custodias_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Permitir editar los titulos del visor de garantias
    ''' </summary>
    ''' <param name="TitulosGarantia"></param>
    ''' <remarks></remarks>
    Public Sub UpdateTitulosGarantia(TitulosGarantia As TitulosGarantia)
        Throw New NotImplementedException
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TitulosGarantia"></param>
    ''' <remarks></remarks>
    Public Sub InsertTitulosGarantia(TitulosGarantia As TitulosGarantia)
        Throw New NotImplementedException
    End Sub

    ''' <summary>
    ''' Consultar el saldo del cliente seleccionado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Query(HasSideEffects:=True)> _
    Public Function Garantias_SaldoCliente_Consultar(plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IEnumerable(Of SaldosCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Garantias_SaldoCliente_Consultar(plngIDComitente, DateTime.Now, pstrUsuario, DemeInfoSesion(pstrUsuario, "Garantias_SaldoCliente_Consultar"), ClsConstantes.GINT_ErrorPersonalizado)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Garantias_SaldoCliente_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Método para bloquear los títulos del visor de garantias
    ''' </summary>
    ''' <param name="plngIDRecibo"></param>
    ''' <param name="plngsecuencia"></param>
    ''' <param name="pstrMotivo"></param>
    ''' <param name="pstrNotasBloqueo"></param>
    ''' <param name="pdblCantidad"></param>
    ''' <param name="pstrAccionEjecutar"></param>
    ''' <param name="pdtmRecibo"></param>
    ''' <param name="pintIDLiquidacion"></param>
    ''' <param name="plngParcial"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pstrClase"></param>
    ''' <param name="pdtmLiquidacion"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Query(HasSideEffects:=True)> _
    Public Function Garantias_Custodias_Bloquear_Liberar(plngIDRecibo As Integer, _
                                                         plngsecuencia As Integer, _
                                                         pstrMotivo As String, _
                                                         pstrNotasBloqueo As String, _
                                                         pdblCantidad As Double, _
                                                         pstrAccionEjecutar As String, _
                                                         pdtmRecibo As DateTime, _
                                                         pintIDLiquidacion As Integer, _
                                                         plngParcial As Integer, _
                                                         pstrTipo As String, _
                                                         pstrClase As String, _
                                                         pdtmLiquidacion As DateTime, _
                                                         pintTipoMercado As Integer, _
                                                         plogSAG As Boolean, _
                                                         pstrRadicado As String, _
                                                         plogReasiganar As Boolean, _
                                                         pdblCantidadReasignar As Double, _
                                                         pintIdBloqueo As Integer, _
                                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IEnumerable(Of RespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_Garantias_Custodias_Bloquear_Liberar(plngIDRecibo,
                                                                                    plngsecuencia,
                                                                                    pstrMotivo,
                                                                                    pstrNotasBloqueo,
                                                                                    pdblCantidad,
                                                                                    pstrAccionEjecutar,
                                                                                    pdtmRecibo,
                                                                                    pintIDLiquidacion,
                                                                                    plngParcial,
                                                                                    pstrTipo,
                                                                                    pstrClase,
                                                                                    pdtmLiquidacion,
                                                                                    pintTipoMercado,
                                                                                    plogSAG,
                                                                                    pstrRadicado,
                                                                                    plogReasiganar,
                                                                                    pdblCantidadReasignar,
                                                                                    pintIdBloqueo,
                                                                                    pstrUsuario,
                                                                                    DemeInfoSesion(pstrUsuario, "Garantias_Custodias_Bloquear_Liberar"),
                                                                                    ClsConstantes.GINT_ErrorPersonalizado)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Garantias_Custodias_Bloquear_Liberar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Métodos para bloquear saldo en el visor de garantías
    ''' </summary>
    ''' <param name="plngIDComitente"></param>
    ''' <param name="pstrMotivoBloqueoSaldo"></param>
    ''' <param name="pcurValorBloqueado"></param>
    ''' <param name="pstrNaturaleza"></param>
    ''' <param name="pdtmFechaBloqueo"></param>
    ''' <param name="pstrDetalleBloqueo"></param>
    ''' <param name="pintIDLiquidacion"></param>
    ''' <param name="plngParcial"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pstrClase"></param>
    ''' <param name="pdtmLiquidacion"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Query(HasSideEffects:=True)> _
    Public Function Garantias_Saldos_Bloquear_Liberar(plngIDComitente As String, _
                                                      pstrMotivoBloqueoSaldo As String, _
                                                      pcurValorBloqueado As Decimal, _
                                                      pstrNaturaleza As String, _
                                                      pdtmFechaBloqueo As DateTime, _
                                                      pstrDetalleBloqueo As String, _
                                                      pintIDLiquidacion As Integer, _
                                                      plngParcial As Integer, _
                                                      pstrTipo As String, _
                                                      pstrClase As String, _
                                                      pdtmLiquidacion As DateTime, _
                                                      pintTipoMercado As Integer, _
                                                      plogSAG As Boolean, _
                                                      pstrRadicado As String, _
                                                      plogReasiganar As Boolean, _
                                                      pcurValorReasignar As Decimal, _
                                                      pintIDBloqueo As Integer, _
                                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IEnumerable(Of RespuestaValidaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Garantias_Saldos_Bloquear_Liberar(plngIDComitente,
                                                                                 pstrMotivoBloqueoSaldo,
                                                                                 pcurValorBloqueado,
                                                                                 pstrNaturaleza,
                                                                                 pdtmFechaBloqueo,
                                                                                 pstrDetalleBloqueo,
                                                                                 pintIDLiquidacion,
                                                                                 plngParcial,
                                                                                 pstrTipo,
                                                                                 pstrClase,
                                                                                 pdtmLiquidacion,
                                                                                 pintTipoMercado,
                                                                                 plogSAG,
                                                                                 pstrRadicado,
                                                                                 plogReasiganar,
                                                                                 pcurValorReasignar,
                                                                                 pintIDBloqueo,
                                                                                 pstrUsuario,
                                                                                 DemeInfoSesion(pstrUsuario, "Garantias_Saldos_Bloquear_Liberar"),
                                                                                 ClsConstantes.GINT_ErrorPersonalizado)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Garantias_Saldos_Bloquear_Liberar")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Función para la consulta de los saldos bloqueados en garantias para determinada liquidación 
    ''' </summary>
    ''' <param name="plngIDComitente"></param>
    ''' <param name="pintIDLiquidacion"></param>
    ''' <param name="plngParcial"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pstrClase"></param>
    ''' <param name="pdtmLiquidacion"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Collection IEnumerable(Of SaldosBloqueados)</returns>
    ''' <remarks>SV20160419</remarks>
    <Query(HasSideEffects:=True)> _
    Public Function Garantias_SaldosBloqueados_Consultar(plngIDComitente As String, _
                                                         pintIDLiquidacion As Integer, _
                                                         plngParcial As Integer, _
                                                         pstrTipo As String, _
                                                         pstrClase As String, _
                                                         pdtmLiquidacion As Date, _
                                                         pintTipoMercado As Integer, _
                                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IEnumerable(Of SaldosBloqueados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim ret = Me.DataContext.uspOyDNet_Garantias_SaldosBloqueados_Consultar(plngIDComitente,
                                                                                    pintIDLiquidacion,
                                                                                    plngParcial,
                                                                                    pstrTipo,
                                                                                    pstrClase,
                                                                                    pdtmLiquidacion,
                                                                                    pintTipoMercado,
                                                                                    pstrUsuario,
                                                                                    DemeInfoSesion(pstrUsuario, "Garantias_SaldosBloqueados_Consultar"),
                                                                                    ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Garantias_SaldosBloqueados_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="SaldosBloqueados"></param>
    ''' <remarks></remarks>
    Public Sub UpdateSaldosBloqueados(SaldosBloqueados As SaldosBloqueados)
        Throw New NotImplementedException
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="SaldosBloqueados"></param>
    ''' <remarks></remarks>
    Public Sub InsertSaldosBloqueados(SaldosBloqueados As SaldosBloqueados)
        Throw New NotImplementedException
    End Sub

#End Region

End Class