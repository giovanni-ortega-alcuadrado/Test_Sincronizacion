
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
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestros
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
Partial Public Class OyDPLUSMaestrosDomainService
    Inherits LinqToSqlDomainService(Of OyDPLUSMaestrosDataContext)

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

#Region "COMUNES"
    <Query(HasSideEffects:=True)>
    Public Function GetAuditorias(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of Auditoria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objResultado As IQueryable(Of Auditoria) = Nothing
            Try
                objResultado = Me.DataContext.Auditorias
            Catch ex As Exception
                ManejarError(ex, Me.ToString(), "GetAuditorias")
            End Try
            Return objResultado
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetAuditorias")
            Return Nothing
        End Try

    End Function
#End Region

#Region "MAESTROS OYDPLUS"

#Region "Maestro Mensajes"

    Public Function TraerMensajePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Mensaje
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Mensaje
            'e.IDMensaje = 
            e.CodigoMensaje = String.Empty
            e.Mensaje = String.Empty
            e.MensajeConReempl = String.Empty
            'e.Usuario = 
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerMensajePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertMensaje(ByVal Mensaje As Mensaje)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Mensaje.pstrUsuarioConexion, Mensaje.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Mensaje.InfoSesion = DemeInfoSesion(Mensaje.pstrUsuarioConexion, "InsertMensaje")
            Me.DataContext.Mensajes.InsertOnSubmit(Mensaje)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertMensaje")
        End Try
    End Sub

    Public Sub UpdateMensaje(ByVal currentMensaje As Mensaje)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentMensaje.pstrUsuarioConexion, currentMensaje.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentMensaje.InfoSesion = DemeInfoSesion(currentMensaje.pstrUsuarioConexion, "UpdateMensaje")
            Me.DataContext.Mensajes.Attach(currentMensaje, Me.ChangeSet.GetOriginal(currentMensaje))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateMensaje")
        End Try
    End Sub

    Public Sub DeleteMensaje(ByVal Mensaje As Mensaje)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Mensaje.pstrUsuarioConexion, Mensaje.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Mensajes_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteMensaje"),0).ToList
            Mensaje.InfoSesion = DemeInfoSesion(Mensaje.pstrUsuarioConexion, "DeleteMensaje")
            Me.DataContext.Mensajes.Attach(Mensaje)
            Me.DataContext.Mensajes.DeleteOnSubmit(Mensaje)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteMensaje")
        End Try
    End Sub

    Public Function MensajesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Mensaje)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Mensajes_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "MensajesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MensajesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function MensajesConsultar(ByVal pstrCodigoMensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Mensaje)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Mensajes_Consultar(RetornarValorDescodificado(pstrCodigoMensaje), pstrUsuario, DemeInfoSesion(pstrUsuario, "MensajesConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MensajesConsultar")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Maestro Reglas"

    Public Function TraerReglaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Regla
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Regla
            'e.IDRegla = 
            e.CodigoRegla = String.Empty
            e.DescripcionRegla = String.Empty
            'e.IDTipo = 
            'e.Usuario = 
            'e.Actualizacion = 
            e.NombreCorto = String.Empty
            e.Prioridad = 0
            e.CodigoTipoRegla = "DETIENEINGRESONOCONTINUA"
            e.CausasJustificacion = String.Empty
            e.NombreProcedimiento = String.Empty
            e.Parametros = String.Empty
            e.Modulo = String.Empty
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReglaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertRegla(ByVal Regla As Regla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Regla.pstrUsuarioConexion, Regla.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Regla.InfoSesion = DemeInfoSesion(Regla.pstrUsuarioConexion, "InsertRegla")
            Me.DataContext.Reglas.InsertOnSubmit(Regla)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertRegla")
        End Try
    End Sub

    Public Sub UpdateRegla(ByVal currentRegla As Regla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentRegla.pstrUsuarioConexion, currentRegla.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentRegla.InfoSesion = DemeInfoSesion(currentRegla.pstrUsuarioConexion, "UpdateRegla")
            Me.DataContext.Reglas.Attach(currentRegla, Me.ChangeSet.GetOriginal(currentRegla))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateRegla")
        End Try
    End Sub

    Public Sub DeleteRegla(ByVal Regla As Regla)
        Try
            ' ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Regla.pstrUsuarioConexion, Regla.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Reglas_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteRegla"),0).ToList
            Regla.InfoSesion = DemeInfoSesion(Regla.pstrUsuarioConexion, "DeleteRegla")
            Me.DataContext.Reglas.Attach(Regla)
            Me.DataContext.Reglas.DeleteOnSubmit(Regla)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteRegla")
        End Try
    End Sub

    Public Function ReglasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Regla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Reglas_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "ReglasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReglasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ReglasConsultar(ByVal pstrCodigoRegla As String, ByVal pstrNombreCorto As String, ByVal plogMotorCalculos As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Regla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim CodigoRegla = HttpUtility.UrlDecode(pstrCodigoRegla)
            Dim NombreCorto = HttpUtility.UrlDecode(pstrNombreCorto)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Reglas_Consultar(CodigoRegla, NombreCorto, plogMotorCalculos, pstrUsuario, DemeInfoSesion(pstrUsuario, "ReglasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReglasConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Maestro Bancos nacionales fondos"

    Public Function TraerBancosNacionalesFondosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As BancosNacionalesFondos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New BancosNacionalesFondos
            e.NombreBanco = String.Empty
            e.NombreTipoCuenta = String.Empty
            e.NombreTipoDocumentoTitular = String.Empty
            e.NombreTitular = String.Empty
            e.NroCuenta = String.Empty
            e.NroDocumentoTitular = String.Empty
            e.TipoCuenta = String.Empty
            e.TipoDocumentoTitular = String.Empty
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReglaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertBancosNacionalesFondos(ByVal objBancoNacionalFondo As BancosNacionalesFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objBancoNacionalFondo.pstrUsuarioConexion, objBancoNacionalFondo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objBancoNacionalFondo.InfoSesion = DemeInfoSesion(objBancoNacionalFondo.pstrUsuarioConexion, "InsertBancosNacionalesFondos")
            Me.DataContext.BancosNacionalesFondos.InsertOnSubmit(objBancoNacionalFondo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertBancosNacionalesFondos")
        End Try
    End Sub

    Public Sub UpdateBancosNacionalesFondos(ByVal objBancoNacionalFondo As BancosNacionalesFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objBancoNacionalFondo.pstrUsuarioConexion, objBancoNacionalFondo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objBancoNacionalFondo.InfoSesion = DemeInfoSesion(objBancoNacionalFondo.pstrUsuarioConexion, "UpdateBancosNacionalesFondos")
            Me.DataContext.BancosNacionalesFondos.Attach(objBancoNacionalFondo, Me.ChangeSet.GetOriginal(objBancoNacionalFondo))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateBancosNacionalesFondos")
        End Try
    End Sub

    Public Sub DeleteBancosNacionalesFondos(ByVal objBancoNacionalFondo As BancosNacionalesFondos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objBancoNacionalFondo.pstrUsuarioConexion, objBancoNacionalFondo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Reglas_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteRegla"),0).ToList
            objBancoNacionalFondo.InfoSesion = DemeInfoSesion(objBancoNacionalFondo.pstrUsuarioConexion, "DeleteBancosNacionalesFondos")
            Me.DataContext.BancosNacionalesFondos.Attach(objBancoNacionalFondo)
            Me.DataContext.BancosNacionalesFondos.DeleteOnSubmit(objBancoNacionalFondo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteBancosNacionalesFondos")
        End Try
    End Sub

    Public Function BancosNacionalesFondosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BancosNacionalesFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BancosNacionalesFondos_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "BancosNacionalesFondosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BancosNacionalesFondosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function BancosNacionalesFondosConsultar(ByVal pintIDBanco As Nullable(Of Integer), ByVal pstrNroCuenta As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BancosNacionalesFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrNroCuenta = HttpUtility.UrlDecode(pstrNroCuenta)
            pstrNroDocumento = HttpUtility.UrlDecode(pstrNroDocumento)
            pstrNombre = HttpUtility.UrlDecode(pstrNombre)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BancosNacionalesFondos_Consultar(pintIDBanco, pstrNroCuenta, pstrNroDocumento, pstrNombre, pstrUsuario, DemeInfoSesion(pstrUsuario, "BancosNacionalesFondosConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BancosNacionalesFondosConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Maestro Alias Especies"

    Public Sub UpdateAliasEspecie(ByVal currentAliasEspecie As AliasEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentAliasEspecie.pstrUsuarioConexion, currentAliasEspecie.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentAliasEspecie.InfoSesion = DemeInfoSesion(currentAliasEspecie.pstrUsuarioConexion, "UpdateAliasEspecie")
            Me.DataContext.AliasEspecies.Attach(currentAliasEspecie, Me.ChangeSet.GetOriginal(currentAliasEspecie))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateAliasEspecie")
        End Try
    End Sub

    ''' <summary>
    ''' Filtro de los registros para configuración del alias
    ''' </summary>
    ''' <param name="pstrFiltro"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AliasEspecieFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AliasEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_AliasEspecies_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "AliasEspecieFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AliasEspecieFiltrar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de los registros para configuración del alias
    ''' </summary>
    ''' <param name="pintIDEspecies"></param>
    ''' <param name="pstrNemotecnico"></param>
    ''' <param name="pstrNombre"></param>
    ''' <param name="pstrAlias"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AliasEspecieConsultar(ByVal pintIDEspecies As Integer, ByVal pstrNemotecnico As String, ByVal pstrNombre As String, ByVal pstrAlias As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AliasEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strNemotecnicoDec = HttpUtility.UrlDecode(pstrNemotecnico)
            Dim strNombreDec = HttpUtility.UrlDecode(pstrNombre)
            Dim strAliasDec = HttpUtility.UrlDecode(pstrAlias)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_AliasEspecies_Consultar(pintIDEspecies, strNemotecnicoDec, strNombreDec, strAliasDec, pstrUsuario, DemeInfoSesion(pstrUsuario, "AliasEspecieConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AliasEspecieConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Maestros Mensajes Reglas"

    Public Function TraerMensajesReglaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As MensajesRegla
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New MensajesRegla
            e.IDRegla = Nothing
            e.IDMensaje = Nothing
            e.IDTipoMensaje = 0
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerMensajesReglaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertMensajesRegla(ByVal MensajesRegla As MensajesRegla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,MensajesRegla.pstrUsuarioConexion, MensajesRegla.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            MensajesRegla.InfoSesion = DemeInfoSesion(MensajesRegla.pstrUsuarioConexion, "InsertMensajesRegla")
            Me.DataContext.MensajesReglas.InsertOnSubmit(MensajesRegla)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertMensajesRegla")
        End Try
    End Sub

    Public Sub UpdateMensajesRegla(ByVal currentMensajesRegla As MensajesRegla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentMensajesRegla.pstrUsuarioConexion, currentMensajesRegla.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentMensajesRegla.InfoSesion = DemeInfoSesion(currentMensajesRegla.pstrUsuarioConexion, "UpdateMensajesRegla")
            Me.DataContext.MensajesReglas.Attach(currentMensajesRegla, Me.ChangeSet.GetOriginal(currentMensajesRegla))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateMensajesRegla")
        End Try
    End Sub

    Public Sub DeleteMensajesRegla(ByVal MensajesRegla As MensajesRegla)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,MensajesRegla.pstrUsuarioConexion, MensajesRegla.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_MensajesReglas_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteMensajesRegla"),0).ToList
            MensajesRegla.InfoSesion = DemeInfoSesion(MensajesRegla.pstrUsuarioConexion, "DeleteMensajesRegla")
            Me.DataContext.MensajesReglas.Attach(MensajesRegla)
            Me.DataContext.MensajesReglas.DeleteOnSubmit(MensajesRegla)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteMensajesRegla")
        End Try
    End Sub

    Public Function MensajesReglasConsultar(ByVal pintIDRegla As System.Nullable(Of Integer), ByVal pintIDMensaje As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MensajesRegla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_MensajesReglas_Consultar(pintIDRegla, pintIDMensaje, pstrUsuario, DemeInfoSesion(pstrUsuario, "MensajesReglasConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MensajesReglasConsultar")
            Return Nothing
        End Try
    End Function

    Public Function MensajesReglasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MensajesRegla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro As String = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_MensajesReglas_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "MensajesReglasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MensajesReglasFiltrar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "TipoNegocio"

    Public Function TipoNegocioFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoNegocio_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoNegocioFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoNegocioFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TipoNegocioConsultar(ByVal pCodigo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Codigo = HttpUtility.HtmlDecode(pCodigo)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoNegocio_Consultar(Codigo, pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoNegocioConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoNegocioConsultar")
            Return Nothing
        End Try
    End Function
    Public Function TipoNegocioActualizar(ByVal pintID As Integer, ByVal pstrCodigo As String, ByVal pdblPorcentajeMinComision As Double, ByVal pstrUsuario As String) As List(Of OyDPLUSMaestros.TipoNegocio)
        Try
            Dim Codigo = HttpUtility.HtmlDecode(pstrCodigo)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoNegocio_Actualizar(pintID, pstrCodigo, pdblPorcentajeMinComision, pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoNegocioActualizar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoNegocioActualizar")
            Return Nothing
        End Try
        Return Nothing
    End Function
    Public Sub InsertTipoNegocio(ByVal Regla As TipoNegocio)
    End Sub

    Public Sub UpdateTipoNegocio(ByVal currentRegla As TipoNegocio)
    End Sub

    Public Sub DeleteTipoNegocio(ByVal Regla As TipoNegocio)
    End Sub
    Public Function TraerTipoNegociPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TipoNegocio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TipoNegocio
            'e.ID = 
            'e.Codigo = 
            'e.Descripcion = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.ConfiguracionMenu = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTipoNegociPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "CertificacionXTipoNegocio"

    Public Function TraerCertificacionXTipoNegociPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CertificacionXTipoNegocio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New CertificacionXTipoNegocio
            'e.ID = 
            'e.CodigoCertificacion = 
            'e.CodigoTipoNegocio = 
            'e.Actualizacion = 
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCertificacionXTipoNegociPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCertificacionXTipoNegocio(ByVal CertificacionXTipoNegoci As CertificacionXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CertificacionXTipoNegoci.pstrUsuarioConexion, CertificacionXTipoNegoci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CertificacionXTipoNegoci.InfoSesion = DemeInfoSesion(CertificacionXTipoNegoci.pstrUsuarioConexion, "InsertCertificacionXTipoNegocio")
            Me.DataContext.CertificacionXTipoNegocio.InsertOnSubmit(CertificacionXTipoNegoci)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCertificacionXTipoNegocio")
        End Try
    End Sub

    Public Sub UpdateCertificacionXTipoNegocio(ByVal currentCertificacionXTipoNegoci As CertificacionXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCertificacionXTipoNegoci.pstrUsuarioConexion, currentCertificacionXTipoNegoci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCertificacionXTipoNegoci.InfoSesion = DemeInfoSesion(currentCertificacionXTipoNegoci.pstrUsuarioConexion, "UpdateCertificacionXTipoNegocio")
            Me.DataContext.CertificacionXTipoNegocio.Attach(currentCertificacionXTipoNegoci, Me.ChangeSet.GetOriginal(currentCertificacionXTipoNegoci))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCertificacionXTipoNegocio")
        End Try
    End Sub

    Public Sub DeleteCertificacionXTipoNegocio(ByVal CertificacionXTipoNegoci As CertificacionXTipoNegocio)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CertificacionXTipoNegoci.pstrUsuarioConexion, CertificacionXTipoNegoci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_CertificacionXTipoNegocio_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteCertificacionXTipoNegoci"),0).ToList
            CertificacionXTipoNegoci.InfoSesion = DemeInfoSesion(CertificacionXTipoNegoci.pstrUsuarioConexion, "DeleteCertificacionXTipoNegocio")
            Me.DataContext.CertificacionXTipoNegocio.Attach(CertificacionXTipoNegoci)
            Me.DataContext.CertificacionXTipoNegocio.DeleteOnSubmit(CertificacionXTipoNegoci)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCertificacionXTipoNegocio")
        End Try
    End Sub

    Public Function CertificacionXTipoNegocioConsultar(ByVal pstrTipoNegocio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CertificacionXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CertificacionXTipoNegocio_Consultar(pstrTipoNegocio, pstrUsuario, DemeInfoSesion(pstrUsuario, "CertificacionXTipoNegocioConsultar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CertificacionXTipoNegocioConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "DocumentoXTipoNegocio"


    Public Function TraerDocumentoXTipoNegociPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DocumentoXTipoNegocio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DocumentoXTipoNegocio
            'e.ID = 
            e.TipoNegocio = Nothing
            e.CodDocumento = 0
            'e.Actualizacion = 
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDocumentoXTipoNegociPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDocumentoXTipoNegocio(ByVal DocumentoXTipoNegoci As DocumentoXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DocumentoXTipoNegoci.pstrUsuarioConexion, DocumentoXTipoNegoci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DocumentoXTipoNegoci.InfoSesion = DemeInfoSesion(DocumentoXTipoNegoci.pstrUsuarioConexion, "InsertDocumentoXTipoNegocio")
            Me.DataContext.DocumentoXTipoNegocio.InsertOnSubmit(DocumentoXTipoNegoci)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDocumentoXTipoNegocio")
        End Try
    End Sub

    Public Sub UpdateDocumentoXTipoNegocio(ByVal currentDocumentoXTipoNegoci As DocumentoXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentDocumentoXTipoNegoci.pstrUsuarioConexion, currentDocumentoXTipoNegoci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDocumentoXTipoNegoci.InfoSesion = DemeInfoSesion(currentDocumentoXTipoNegoci.pstrUsuarioConexion, "UpdateDocumentoXTipoNegocio")
            Me.DataContext.DocumentoXTipoNegocio.Attach(currentDocumentoXTipoNegoci, Me.ChangeSet.GetOriginal(currentDocumentoXTipoNegoci))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDocumentoXTipoNegocio")
        End Try
    End Sub

    Public Sub DeleteDocumentoXTipoNegocio(ByVal DocumentoXTipoNegoci As DocumentoXTipoNegocio)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DocumentoXTipoNegoci.pstrUsuarioConexion, DocumentoXTipoNegoci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_DocumentoXTipoNegocio_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteDocumentoXTipoNegoci"),0).ToList
            DocumentoXTipoNegoci.InfoSesion = DemeInfoSesion(DocumentoXTipoNegoci.pstrUsuarioConexion, "DeleteDocumentoXTipoNegocio")
            Me.DataContext.DocumentoXTipoNegocio.Attach(DocumentoXTipoNegoci)
            Me.DataContext.DocumentoXTipoNegocio.DeleteOnSubmit(DocumentoXTipoNegoci)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDocumentoXTipoNegocio")
        End Try
    End Sub

    Public Function DocumentoXTipoNegocioConsultar(ByVal pstrTipoNegocio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DocumentoXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DocumentoXTipoNegocio_Consultar(pstrTipoNegocio, pstrUsuario, DemeInfoSesion(pstrUsuario, "DocumentoXTipoNegocioConsultar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DocumentoXTipoNegocioConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "TipoNegocioXEspecie"

    Public Function TraerTipoNegocioXEspeciPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TipoNegocioXEspecie
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TipoNegocioXEspecie
            'e.ID = 
            e.TipoNegocio = Nothing
            e.IDEspecie = "TODAS"
            e.ManejaISIN = False
            e.MaxValorCantidad = 0
            e.PermiteNegociar = True
            'e.Actualizacion = 
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTipoNegocioXEspeciPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTipoNegocioXEspecie(ByVal TipoNegocioXEspeci As TipoNegocioXEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoNegocioXEspeci.pstrUsuarioConexion, TipoNegocioXEspeci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TipoNegocioXEspeci.InfoSesion = DemeInfoSesion(TipoNegocioXEspeci.pstrUsuarioConexion, "InsertTipoNegocioXEspecie")
            Me.DataContext.TipoNegocioXEspecie.InsertOnSubmit(TipoNegocioXEspeci)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTipoNegocioXEspecie")
        End Try
    End Sub

    Public Sub UpdateTipoNegocioXEspecie(ByVal currentTipoNegocioXEspeci As TipoNegocioXEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTipoNegocioXEspeci.pstrUsuarioConexion, currentTipoNegocioXEspeci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTipoNegocioXEspeci.InfoSesion = DemeInfoSesion(currentTipoNegocioXEspeci.pstrUsuarioConexion, "UpdateTipoNegocioXEspecie")
            Me.DataContext.TipoNegocioXEspecie.Attach(currentTipoNegocioXEspeci, Me.ChangeSet.GetOriginal(currentTipoNegocioXEspeci))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTipoNegocioXEspecie")
        End Try
    End Sub

    Public Sub DeleteTipoNegocioXEspecie(ByVal TipoNegocioXEspeci As TipoNegocioXEspecie)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoNegocioXEspeci.pstrUsuarioConexion, TipoNegocioXEspeci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_TipoNegocioXEspecie_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteTipoNegocioXEspeci"),0).ToList
            TipoNegocioXEspeci.InfoSesion = DemeInfoSesion(TipoNegocioXEspeci.pstrUsuarioConexion, "DeleteTipoNegocioXEspecie")
            Me.DataContext.TipoNegocioXEspecie.Attach(TipoNegocioXEspeci)
            Me.DataContext.TipoNegocioXEspecie.DeleteOnSubmit(TipoNegocioXEspeci)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTipoNegocioXEspecie")
        End Try
    End Sub

    Public Function TipoNegocioXEspecieConsultar(ByVal pstrTipoNegocio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoNegocioXEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoNegocioXEspecie_Consultar(pstrTipoNegocio, pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoNegocioXEspecieConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoNegocioXEspecieConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "TipoNegocioXTipoProducto"

    Public Function TraerTipoNegocioXTipoProductPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TipoNegocioXTipoProducto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TipoNegocioXTipoProducto
            'e.ID = 
            e.TipoNegocio = Nothing
            e.TipoProducto = Nothing
            e.IDComitente = Nothing
            e.Nombre = "TODOS"
            e.Perfil = String.Empty
            e.PermiteNegociar = True
            'e.Actualizacion = 
            'e.ValorMaxNegociacion = 
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTipoNegocioXTipoProductPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTipoNegocioXTipoProducto(ByVal TipoNegocioXTipoProduct As TipoNegocioXTipoProducto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoNegocioXTipoProduct.pstrUsuarioConexion, TipoNegocioXTipoProduct.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TipoNegocioXTipoProduct.InfoSesion = DemeInfoSesion(TipoNegocioXTipoProduct.pstrUsuarioConexion, "InsertTipoNegocioXTipoProducto")
            Me.DataContext.TipoNegocioXTipoProducto.InsertOnSubmit(TipoNegocioXTipoProduct)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTipoNegocioXTipoProducto")
        End Try
    End Sub

    Public Sub UpdateTipoNegocioXTipoProducto(ByVal currentTipoNegocioXTipoProduct As TipoNegocioXTipoProducto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTipoNegocioXTipoProduct.pstrUsuarioConexion, currentTipoNegocioXTipoProduct.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTipoNegocioXTipoProduct.InfoSesion = DemeInfoSesion(currentTipoNegocioXTipoProduct.pstrUsuarioConexion, "UpdateTipoNegocioXTipoProducto")
            Me.DataContext.TipoNegocioXTipoProducto.Attach(currentTipoNegocioXTipoProduct, Me.ChangeSet.GetOriginal(currentTipoNegocioXTipoProduct))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTipoNegocioXTipoProducto")
        End Try
    End Sub

    Public Sub DeleteTipoNegocioXTipoProducto(ByVal TipoNegocioXTipoProduct As TipoNegocioXTipoProducto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoNegocioXTipoProduct.pstrUsuarioConexion, TipoNegocioXTipoProduct.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_TipoNegocioXTipoProducto_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteTipoNegocioXTipoProduct"),0).ToList
            TipoNegocioXTipoProduct.InfoSesion = DemeInfoSesion(TipoNegocioXTipoProduct.pstrUsuarioConexion, "DeleteTipoNegocioXTipoProducto")
            Me.DataContext.TipoNegocioXTipoProducto.Attach(TipoNegocioXTipoProduct)
            Me.DataContext.TipoNegocioXTipoProducto.DeleteOnSubmit(TipoNegocioXTipoProduct)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTipoNegocioXTipoProducto")
        End Try
    End Sub

    Public Function TipoNegocioXTipoProductoConsultar(ByVal pstrTipoNegocio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoNegocioXTipoProducto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoNegocioXTipoProducto_Consultar(pstrTipoNegocio, pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoNegocioXTipoProductoConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoNegocioXTipoProductoConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Distribución comisión x tipo negocio"

    Public Function DistribucionPortafolioConsultar(ByVal pstrTipoNegocio As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DistribucionComisionXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOYDNet_Maestros_DistribucionComisionXTipoNegocio_Consultar(pstrTipoNegocio, pstrUsuario, DemeInfoSesion(pstrUsuario, "DistribucionPortafolioConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DistribucionPortafolioConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerDistribucionPortafolioPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DistribucionComisionXTipoNegocio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DistribucionComisionXTipoNegocio
            'e.Idcosto = 
            'e.Valor = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDistribucionPortafolioPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDistribucionComisionXTipoNegocio(ByVal Distribucion As DistribucionComisionXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Distribucion.pstrUsuarioConexion, Distribucion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Distribucion.InfoSesion = DemeInfoSesion(Distribucion.pstrUsuarioConexion, "InsertDistribucionComisionXTipoNegocio")
            Me.DataContext.DistribucionComisionXTipoNegocio.InsertOnSubmit(Distribucion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDistribucionComisionXTipoNegocio")
        End Try
    End Sub

    Public Sub UpdateDistribucionComisionXTipoNegocio(ByVal currentDistribucion As DistribucionComisionXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentDistribucion.pstrUsuarioConexion, currentDistribucion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDistribucion.InfoSesion = DemeInfoSesion(currentDistribucion.pstrUsuarioConexion, "UpdateDistribucionComisionXTipoNegocio")
            Me.DataContext.DistribucionComisionXTipoNegocio.Attach(currentDistribucion, Me.ChangeSet.GetOriginal(currentDistribucion))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDistribucionComisionXTipoNegocio")
        End Try
    End Sub

    Public Sub DeleteDistribucionComisionXTipoNegocio(ByVal Distribucion As DistribucionComisionXTipoNegocio)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Distribucion.pstrUsuarioConexion, Distribucion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.dbo.uspOyDNet_Costos_Eliminar( pCodigoFormapago,  pCodigoTipoCheque,  pCodigoTipoCruce, DemeInfoSesion(pstrUsuario, "DeleteCosto"),0).ToList
            Distribucion.InfoSesion = DemeInfoSesion(Distribucion.pstrUsuarioConexion, "DeleteDistribucionComisionXTipoNegocio")
            Me.DataContext.DistribucionComisionXTipoNegocio.Attach(Distribucion)
            Me.DataContext.DistribucionComisionXTipoNegocio.DeleteOnSubmit(Distribucion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDistribucionComisionXTipoNegocio")
        End Try
    End Sub
#End Region

#Region "CertificacionesXReceptor"

    Public Function CertificacionesXReceptorConsultar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CertificacionesXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CertificacionesXReceptor_Consultar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "CertificacionesXReceptorConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CertificacionesXReceptorConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerCertificacionesXReceptoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CertificacionesXRecepto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New CertificacionesXRecepto
            'e.ID = 
            e.CodigoReceptor = Nothing
            e.CodigoCertificacion = Nothing
            e.FechaInicial = Now
            e.FechaFinal = DateAdd(DateInterval.Month, 1, Now)
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCertificacionesXReceptoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCertificacionesXRecepto(ByVal CertificacionesXRecepto As CertificacionesXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CertificacionesXRecepto.pstrUsuarioConexion, CertificacionesXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CertificacionesXRecepto.InfoSesion = DemeInfoSesion(CertificacionesXRecepto.pstrUsuarioConexion, "InsertCertificacionesXRecepto")
            Me.DataContext.CertificacionesXReceptor.InsertOnSubmit(CertificacionesXRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCertificacionesXRecepto")
        End Try
    End Sub

    Public Sub UpdateCertificacionesXRecepto(ByVal currentCertificacionesXRecepto As CertificacionesXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCertificacionesXRecepto.pstrUsuarioConexion, currentCertificacionesXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCertificacionesXRecepto.InfoSesion = DemeInfoSesion(currentCertificacionesXRecepto.pstrUsuarioConexion, "UpdateCertificacionesXRecepto")
            Me.DataContext.CertificacionesXReceptor.Attach(currentCertificacionesXRecepto, Me.ChangeSet.GetOriginal(currentCertificacionesXRecepto))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCertificacionesXRecepto")
        End Try
    End Sub

    Public Sub DeleteCertificacionesXRecepto(ByVal CertificacionesXRecepto As CertificacionesXRecepto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CertificacionesXRecepto.pstrUsuarioConexion, CertificacionesXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_CertificacionesXReceptor_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteCertificacionesXRecepto"),0).ToList
            CertificacionesXRecepto.InfoSesion = DemeInfoSesion(CertificacionesXRecepto.pstrUsuarioConexion, "DeleteCertificacionesXRecepto")
            Me.DataContext.CertificacionesXReceptor.Attach(CertificacionesXRecepto)
            Me.DataContext.CertificacionesXReceptor.DeleteOnSubmit(CertificacionesXRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCertificacionesXRecepto")
        End Try
    End Sub

#End Region

#Region "ConceptosXReceptor"

    Public Function ConceptosXReceptorConsultar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptosXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosXReceptor_Consultar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosXReceptorConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosXReceptorConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerConceptosXReceptoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConceptosXRecepto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ConceptosXRecepto
            'e.ID = 
            e.CodigoReceptor = Nothing
            e.IdConcepto = Nothing
            e.Prioridad = 0
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerConceptosXReceptoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConceptosXRecepto(ByVal ConceptosXRecepto As ConceptosXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConceptosXRecepto.pstrUsuarioConexion, ConceptosXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ConceptosXRecepto.InfoSesion = DemeInfoSesion(ConceptosXRecepto.pstrUsuarioConexion, "InsertConceptosXRecepto")
            Me.DataContext.ConceptosXReceptor.InsertOnSubmit(ConceptosXRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConceptosXRecepto")
        End Try
    End Sub

    Public Sub UpdateConceptosXRecepto(ByVal currentConceptosXRecepto As ConceptosXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentConceptosXRecepto.pstrUsuarioConexion, currentConceptosXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConceptosXRecepto.InfoSesion = DemeInfoSesion(currentConceptosXRecepto.pstrUsuarioConexion, "UpdateConceptosXRecepto")
            Me.DataContext.ConceptosXReceptor.Attach(currentConceptosXRecepto, Me.ChangeSet.GetOriginal(currentConceptosXRecepto))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConceptosXRecepto")
        End Try
    End Sub

    Public Sub DeleteConceptosXRecepto(ByVal ConceptosXRecepto As ConceptosXRecepto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConceptosXRecepto.pstrUsuarioConexion, ConceptosXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConceptosXReceptor_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteConceptosXRecepto"),0).ToList
            ConceptosXRecepto.InfoSesion = DemeInfoSesion(ConceptosXRecepto.pstrUsuarioConexion, "DeleteConceptosXRecepto")
            Me.DataContext.ConceptosXReceptor.Attach(ConceptosXRecepto)
            Me.DataContext.ConceptosXReceptor.DeleteOnSubmit(ConceptosXRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConceptosXRecepto")
        End Try
    End Sub

#End Region

#Region "ConfiguracionesAdicionalesReceptor"

    Public Function ConfiguracionesAdicionalesReceptorFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionesAdicionalesRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConfiguracionesAdicionalesReceptor_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConfiguracionesAdicionalesReceptorFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionesAdicionalesReceptorFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ConfiguracionesAdicionalesReceptorConsultar(ByVal pstrReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionesAdicionalesRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.HtmlDecode(pstrReceptor)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConfiguracionesAdicionalesReceptor_Consultar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConfiguracionesAdicionalesReceptorConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionesAdicionalesReceptorConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerConfiguracionesAdicionalesReceptoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionesAdicionalesRecepto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ConfiguracionesAdicionalesRecepto
            e.CodigoReceptor = String.Empty
            e.NombreReceptor = String.Empty
            e.NombreTipoOrden = String.Empty
            e.NombreTipoOrdenDefecto = String.Empty
            e.TipoOrden = Nothing
            e.ExtensionDefecto = String.Empty
            e.TipoOrdenDefecto = String.Empty
            e.EnrutaBus = False
            'e.Actualizacion = 
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerConfiguracionesAdicionalesReceptoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConfiguracionesAdicionalesRecepto(ByVal ConfiguracionesAdicionalesRecepto As ConfiguracionesAdicionalesRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConfiguracionesAdicionalesRecepto.pstrUsuarioConexion, ConfiguracionesAdicionalesRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ConfiguracionesAdicionalesRecepto.InfoSesion = DemeInfoSesion(ConfiguracionesAdicionalesRecepto.pstrUsuarioConexion, "InsertConfiguracionesAdicionalesRecepto")
            Me.DataContext.ConfiguracionesAdicionalesReceptor.InsertOnSubmit(ConfiguracionesAdicionalesRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConfiguracionesAdicionalesRecepto")
        End Try
    End Sub

    Public Sub UpdateConfiguracionesAdicionalesRecepto(ByVal currentConfiguracionesAdicionalesRecepto As ConfiguracionesAdicionalesRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentConfiguracionesAdicionalesRecepto.pstrUsuarioConexion, currentConfiguracionesAdicionalesRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConfiguracionesAdicionalesRecepto.InfoSesion = DemeInfoSesion(currentConfiguracionesAdicionalesRecepto.pstrUsuarioConexion, "UpdateConfiguracionesAdicionalesRecepto")
            Me.DataContext.ConfiguracionesAdicionalesReceptor.Attach(currentConfiguracionesAdicionalesRecepto, Me.ChangeSet.GetOriginal(currentConfiguracionesAdicionalesRecepto))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConfiguracionesAdicionalesRecepto")
        End Try
    End Sub

    Public Sub DeleteConfiguracionesAdicionalesRecepto(ByVal ConfiguracionesAdicionalesRecepto As ConfiguracionesAdicionalesRecepto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConfiguracionesAdicionalesRecepto.pstrUsuarioConexion, ConfiguracionesAdicionalesRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConfiguracionesAdicionalesReceptor_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteConfiguracionesAdicionalesRecepto"),0).ToList
            ConfiguracionesAdicionalesRecepto.InfoSesion = DemeInfoSesion(ConfiguracionesAdicionalesRecepto.pstrUsuarioConexion, "DeleteConfiguracionesAdicionalesRecepto")
            Me.DataContext.ConfiguracionesAdicionalesReceptor.Attach(ConfiguracionesAdicionalesRecepto)
            Me.DataContext.ConfiguracionesAdicionalesReceptor.DeleteOnSubmit(ConfiguracionesAdicionalesRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConfiguracionesAdicionalesRecepto")
        End Try
    End Sub

#End Region

#Region "ParametrosReceptor"

    Public Function ParametrosReceptorConsultar(ByVal pstrCodigoReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ParametrosRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ParametrosReceptor_Consultar(pstrCodigoReceptor, pstrUsuario, DemeInfoSesion(pstrUsuario, "ParametrosReceptorConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ParametrosReceptorConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerParametrosReceptoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ParametrosRecepto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ParametrosRecepto
            'e.ID = 
            e.CodigoReceptor = Nothing
            e.Topico = Nothing
            e.Valor = Nothing
            e.Prioridad = 0
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerParametrosReceptoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertParametrosRecepto(ByVal ParametrosRecepto As ParametrosRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ParametrosRecepto.pstrUsuarioConexion, ParametrosRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ParametrosRecepto.InfoSesion = DemeInfoSesion(ParametrosRecepto.pstrUsuarioConexion, "InsertParametrosRecepto")
            Me.DataContext.ParametrosReceptor.InsertOnSubmit(ParametrosRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertParametrosRecepto")
        End Try
    End Sub

    Public Sub UpdateParametrosRecepto(ByVal currentParametrosRecepto As ParametrosRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentParametrosRecepto.pstrUsuarioConexion, currentParametrosRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentParametrosRecepto.InfoSesion = DemeInfoSesion(currentParametrosRecepto.pstrUsuarioConexion, "UpdateParametrosRecepto")
            Me.DataContext.ParametrosReceptor.Attach(currentParametrosRecepto, Me.ChangeSet.GetOriginal(currentParametrosRecepto))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateParametrosRecepto")
        End Try
    End Sub

    Public Sub DeleteParametrosRecepto(ByVal ParametrosRecepto As ParametrosRecepto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ParametrosRecepto.pstrUsuarioConexion, ParametrosRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ParametrosReceptor_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteParametrosRecepto"),0).ToList
            ParametrosRecepto.InfoSesion = DemeInfoSesion(ParametrosRecepto.pstrUsuarioConexion, "DeleteParametrosRecepto")
            Me.DataContext.ParametrosReceptor.Attach(ParametrosRecepto)
            Me.DataContext.ParametrosReceptor.DeleteOnSubmit(ParametrosRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteParametrosRecepto")
        End Try
    End Sub

#End Region

#Region "ReceptoresClientesAutorizados"
#Region "Receptor Activo"
    Public Function ReceptorActivoConsultar(ByVal pstrReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptorActivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ReceptorActivo_Consultar(pstrReceptor, pstrUsuario, DemeInfoSesion(pstrUsuario, "ReceptorActivoConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptorActivoConsultar")
            Return Nothing
        End Try
    End Function
#End Region
    Public Function ReceptoresClientesAutorizadosConsultar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresClientesAutorizado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ReceptoresClientesAutorizados_Consultar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ReceptoresClientesAutorizadosConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoresClientesAutorizadosConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerReceptoresClientesAutorizadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ReceptoresClientesAutorizado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ReceptoresClientesAutorizado
            'e.ID = 
            e.CodigoReceptorCliente = Nothing
            e.NombreReceptorCliente = Nothing
            e.CodigoReceptorTercero = Nothing
            e.NombreReceptorTercero = Nothing
            e.IDComitente = Nothing
            e.NombreCliente = Nothing
            e.FechaInicial = Now
            e.FechaFinal = DateAdd(DateInterval.Month, 1, Now)
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReceptoresClientesAutorizadoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertReceptoresClientesAutorizado(ByVal ReceptoresClientesAutorizado As ReceptoresClientesAutorizado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresClientesAutorizado.pstrUsuarioConexion, ReceptoresClientesAutorizado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresClientesAutorizado.InfoSesion = DemeInfoSesion(ReceptoresClientesAutorizado.pstrUsuarioConexion, "InsertReceptoresClientesAutorizado")
            Me.DataContext.ReceptoresClientesAutorizados.InsertOnSubmit(ReceptoresClientesAutorizado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertReceptoresClientesAutorizado")
        End Try
    End Sub

    Public Sub UpdateReceptoresClientesAutorizado(ByVal currentReceptoresClientesAutorizado As ReceptoresClientesAutorizado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentReceptoresClientesAutorizado.pstrUsuarioConexion, currentReceptoresClientesAutorizado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentReceptoresClientesAutorizado.InfoSesion = DemeInfoSesion(currentReceptoresClientesAutorizado.pstrUsuarioConexion, "UpdateReceptoresClientesAutorizado")
            Me.DataContext.ReceptoresClientesAutorizados.Attach(currentReceptoresClientesAutorizado, Me.ChangeSet.GetOriginal(currentReceptoresClientesAutorizado))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptoresClientesAutorizado")
        End Try
    End Sub

    Public Sub DeleteReceptoresClientesAutorizado(ByVal ReceptoresClientesAutorizado As ReceptoresClientesAutorizado)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresClientesAutorizado.pstrUsuarioConexion, ReceptoresClientesAutorizado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ReceptoresClientesAutorizados_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteReceptoresClientesAutorizado"),0).ToList
            ReceptoresClientesAutorizado.InfoSesion = DemeInfoSesion(ReceptoresClientesAutorizado.pstrUsuarioConexion, "DeleteReceptoresClientesAutorizado")
            Me.DataContext.ReceptoresClientesAutorizados.Attach(ReceptoresClientesAutorizado)
            Me.DataContext.ReceptoresClientesAutorizados.DeleteOnSubmit(ReceptoresClientesAutorizado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptoresClientesAutorizado")
        End Try
    End Sub

#End Region

#Region "TipoNegocioXReceptor"

    Public Function TipoNegocioXReceptorConsultar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoNegocioXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoNegocioXReceptor_Consultar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoNegocioXReceptorConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoNegocioXReceptorConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerTipoNegocioXReceptoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TipoNegocioXRecepto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TipoNegocioXRecepto
            'e.ID = 
            e.CodigoReceptor = Nothing
            e.CodigoTipoNegocio = Nothing
            e.ValorMaxNegociacion = 0
            e.Prioridad = 0
            e.PorcentajeComision = 0
            e.ValorComision = 0
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTipoNegocioXReceptoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTipoNegocioXRecepto(ByVal TipoNegocioXRecepto As TipoNegocioXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoNegocioXRecepto.pstrUsuarioConexion, TipoNegocioXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TipoNegocioXRecepto.InfoSesion = DemeInfoSesion(TipoNegocioXRecepto.pstrUsuarioConexion, "InsertTipoNegocioXRecepto")
            Me.DataContext.TipoNegocioXReceptor.InsertOnSubmit(TipoNegocioXRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTipoNegocioXRecepto")
        End Try
    End Sub

    Public Sub UpdateTipoNegocioXRecepto(ByVal currentTipoNegocioXRecepto As TipoNegocioXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTipoNegocioXRecepto.pstrUsuarioConexion, currentTipoNegocioXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTipoNegocioXRecepto.InfoSesion = DemeInfoSesion(currentTipoNegocioXRecepto.pstrUsuarioConexion, "UpdateTipoNegocioXRecepto")
            Me.DataContext.TipoNegocioXReceptor.Attach(currentTipoNegocioXRecepto, Me.ChangeSet.GetOriginal(currentTipoNegocioXRecepto))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTipoNegocioXRecepto")
        End Try
    End Sub

    Public Sub DeleteTipoNegocioXRecepto(ByVal TipoNegocioXRecepto As TipoNegocioXRecepto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoNegocioXRecepto.pstrUsuarioConexion, TipoNegocioXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_TipoNegocioXReceptor_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteTipoNegocioXRecepto"),0).ToList
            TipoNegocioXRecepto.InfoSesion = DemeInfoSesion(TipoNegocioXRecepto.pstrUsuarioConexion, "DeleteTipoNegocioXRecepto")
            Me.DataContext.TipoNegocioXReceptor.Attach(TipoNegocioXRecepto)
            Me.DataContext.TipoNegocioXReceptor.DeleteOnSubmit(TipoNegocioXRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTipoNegocioXRecepto")
        End Try
    End Sub

#End Region

#Region "TipoProductoXReceptor"

    Public Function TipoProductoXReceptorConsultar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoProductoXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoProductoXReceptor_Consultar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoProductoXReceptorConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoProductoXReceptorConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerTipoProductoXReceptoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TipoProductoXRecepto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TipoProductoXRecepto
            'e.ID = 
            e.CodigoReceptor = Nothing
            e.TipoProducto = Nothing
            e.Prioridad = 0
            'e.Actualizacion = 
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTipoProductoXReceptoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTipoProductoXRecepto(ByVal TipoProductoXRecepto As TipoProductoXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoProductoXRecepto.pstrUsuarioConexion, TipoProductoXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TipoProductoXRecepto.InfoSesion = DemeInfoSesion(TipoProductoXRecepto.pstrUsuarioConexion, "InsertTipoProductoXRecepto")
            Me.DataContext.TipoProductoXReceptor.InsertOnSubmit(TipoProductoXRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTipoProductoXRecepto")
        End Try
    End Sub

    Public Sub UpdateTipoProductoXRecepto(ByVal currentTipoProductoXRecepto As TipoProductoXRecepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTipoProductoXRecepto.pstrUsuarioConexion, currentTipoProductoXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTipoProductoXRecepto.InfoSesion = DemeInfoSesion(currentTipoProductoXRecepto.pstrUsuarioConexion, "UpdateTipoProductoXRecepto")
            Me.DataContext.TipoProductoXReceptor.Attach(currentTipoProductoXRecepto, Me.ChangeSet.GetOriginal(currentTipoProductoXRecepto))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTipoProductoXRecepto")
        End Try
    End Sub

    Public Sub DeleteTipoProductoXRecepto(ByVal TipoProductoXRecepto As TipoProductoXRecepto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoProductoXRecepto.pstrUsuarioConexion, TipoProductoXRecepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_TipoProductoXReceptor_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteTipoProductoXRecepto"),0).ToList
            TipoProductoXRecepto.InfoSesion = DemeInfoSesion(TipoProductoXRecepto.pstrUsuarioConexion, "DeleteTipoProductoXRecepto")
            Me.DataContext.TipoProductoXReceptor.Attach(TipoProductoXRecepto)
            Me.DataContext.TipoProductoXReceptor.DeleteOnSubmit(TipoProductoXRecepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTipoProductoXRecepto")
        End Try
    End Sub

#End Region

#Region "Clientes Receptor"

    Public Function ClientesReceptorConsultar(ByVal pstrReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesReceptor)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ClientesReceptor_Consultar(pstrReceptor, pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesReceptorConsultar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesReceptorConsultar")
            Return Nothing
        End Try

    End Function

#End Region

#Region "Configuracion Niveles de Atribución"

    Public Function RN_ConfiguracionNivelAtribucionFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RN_ConfiguracionNivelAtribucio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro As String = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_RN_ConfiguracionNivelAtribucion_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "RN_ConfiguracionNivelAtribucionFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RN_ConfiguracionNivelAtribucionFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function RN_ConfiguracionNivelAtribucionConsultar(ByVal pintRegla As System.Nullable(Of Integer),
                                                             ByVal pintTipoDocumento As System.Nullable(Of Integer),
                                                             ByVal pintNivelAtribucion As System.Nullable(Of Integer),
                                                             ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RN_ConfiguracionNivelAtribucio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_RN_ConfiguracionNivelAtribucion_Consultar(pintRegla, pintTipoDocumento, pintNivelAtribucion, pstrUsuario, DemeInfoSesion(pstrUsuario, "RN_ConfiguracionNivelAtribucionConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RN_ConfiguracionNivelAtribucionConsultar")
            Return Nothing
        End Try
    End Function


    Public Function TraerRN_ConfiguracionNivelAtribucioPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As RN_ConfiguracionNivelAtribucio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New RN_ConfiguracionNivelAtribucio
            'e.ID = 
            e.Regla = Nothing
            e.TipoDocumento = Nothing
            e.NivelAprobacion = Nothing
            'e.Usuario = 
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerRN_ConfiguracionNivelAtribucioPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertRN_ConfiguracionNivelAtribucio(ByVal RN_ConfiguracionNivelAtribucio As RN_ConfiguracionNivelAtribucio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,RN_ConfiguracionNivelAtribucio.pstrUsuarioConexion, RN_ConfiguracionNivelAtribucio.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            RN_ConfiguracionNivelAtribucio.InfoSesion = DemeInfoSesion(RN_ConfiguracionNivelAtribucio.pstrUsuarioConexion, "InsertRN_ConfiguracionNivelAtribucio")
            Me.DataContext.RN_ConfiguracionNivelAtribucion.InsertOnSubmit(RN_ConfiguracionNivelAtribucio)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertRN_ConfiguracionNivelAtribucio")
        End Try
    End Sub

    Public Sub UpdateRN_ConfiguracionNivelAtribucio(ByVal currentRN_ConfiguracionNivelAtribucio As RN_ConfiguracionNivelAtribucio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentRN_ConfiguracionNivelAtribucio.pstrUsuarioConexion, currentRN_ConfiguracionNivelAtribucio.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentRN_ConfiguracionNivelAtribucio.InfoSesion = DemeInfoSesion(currentRN_ConfiguracionNivelAtribucio.pstrUsuarioConexion, "UpdateRN_ConfiguracionNivelAtribucio")
            Me.DataContext.RN_ConfiguracionNivelAtribucion.Attach(currentRN_ConfiguracionNivelAtribucio, Me.ChangeSet.GetOriginal(currentRN_ConfiguracionNivelAtribucio))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateRN_ConfiguracionNivelAtribucio")
        End Try
    End Sub

    Public Sub DeleteRN_ConfiguracionNivelAtribucio(ByVal RN_ConfiguracionNivelAtribucio As RN_ConfiguracionNivelAtribucio)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,RN_ConfiguracionNivelAtribucio.pstrUsuarioConexion, RN_ConfiguracionNivelAtribucio.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_RN_ConfiguracionNivelAtribucion_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteRN_ConfiguracionNivelAtribucio"),0).ToList
            RN_ConfiguracionNivelAtribucio.InfoSesion = DemeInfoSesion(RN_ConfiguracionNivelAtribucio.pstrUsuarioConexion, "DeleteRN_ConfiguracionNivelAtribucio")
            Me.DataContext.RN_ConfiguracionNivelAtribucion.Attach(RN_ConfiguracionNivelAtribucio)
            Me.DataContext.RN_ConfiguracionNivelAtribucion.DeleteOnSubmit(RN_ConfiguracionNivelAtribucio)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteRN_ConfiguracionNivelAtribucio")
        End Try
    End Sub

    Public Function RN_TiposDocumentoConsultar(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RN_TiposDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_RN_TiposDocumento_Consultar(pstrUsuario, DemeInfoSesion(pstrUsuario, "RN_TiposDocumentoConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RN_TiposDocumentoConsultar")
            Return Nothing
        End Try
    End Function

    Public Function RN_NivelesAtribucionConsultar(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RN_NivelesAtribucion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_RN_NivelesAtribucion_Consultar(pstrUsuario, DemeInfoSesion(pstrUsuario, "RN_NivelesAtribucionConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RN_NivelesAtribucionConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Costos"

    Public Function CostosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Costo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Costos_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "CostosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CostosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CostosConsultar(ByVal pCodigoFormapago As String, ByVal pCodigoTipoCheque As String, ByVal pCodigoTipoCruce As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Costo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Costos_Consultar(pCodigoFormapago, pCodigoTipoCheque, pCodigoTipoCruce, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarCostos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCostos")
            Return Nothing
        End Try
    End Function

    Public Function TraerCostoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Costo
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Costo
            'e.Idcosto = 
            'e.CodigoFormapago = 
            'e.CodigoTipoCheque = 
            'e.CodigoTipoCruce = 
            'e.Valor = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCostoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCosto(ByVal Costo As Costo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Costo.pstrUsuarioConexion, Costo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Costo.InfoSesion = DemeInfoSesion(Costo.pstrUsuarioConexion, "InsertCosto")
            Me.DataContext.Costos.InsertOnSubmit(Costo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCosto")
        End Try
    End Sub

    Public Sub UpdateCosto(ByVal currentCosto As Costo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCosto.pstrUsuarioConexion, currentCosto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCosto.InfoSesion = DemeInfoSesion(currentCosto.pstrUsuarioConexion, "UpdateCosto")
            Me.DataContext.Costos.Attach(currentCosto, Me.ChangeSet.GetOriginal(currentCosto))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCosto")
        End Try
    End Sub

    Public Sub DeleteCosto(ByVal Costo As Costo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Costo.pstrUsuarioConexion, Costo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.dbo.uspOyDNet_Costos_Eliminar( pCodigoFormapago,  pCodigoTipoCheque,  pCodigoTipoCruce, DemeInfoSesion(pstrUsuario, "DeleteCosto"),0).ToList
            Costo.InfoSesion = DemeInfoSesion(Costo.pstrUsuarioConexion, "DeleteCosto")
            Me.DataContext.Costos.Attach(Costo)
            Me.DataContext.Costos.DeleteOnSubmit(Costo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCosto")
        End Try
    End Sub
#End Region

#Region "Tipo Producto x Especie"

    Public Function TipoProductoFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoProducto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoProducto_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoProductoFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoProductoFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TipoProductoConsultar(ByVal pstrTipoProducto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoProducto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim TipoProducto = HttpUtility.UrlDecode(pstrTipoProducto)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoProducto_Consultar(TipoProducto, pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoProductoConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoProductoConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TipoProductoXEspecieConsultar(ByVal pCodigoTipoProducto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoProductoXEspeci)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoProductoXEspecie_Consultar(pCodigoTipoProducto, pstrUsuario, DemeInfoSesion(pstrUsuario, "TipoProductoXEspecieConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoProductoXEspecieConsultar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTipoProductoXEspeci(ByVal TipoProductoXEspeci As TipoProductoXEspeci)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoProductoXEspeci.pstrUsuarioConexion, TipoProductoXEspeci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TipoProductoXEspeci.InfoSesion = DemeInfoSesion(TipoProductoXEspeci.pstrUsuarioConexion, "InsertTipoProductoXEspeci")
            Me.DataContext.TipoProductoXEspecie.InsertOnSubmit(TipoProductoXEspeci)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTipoProductoXEspeci")
        End Try
    End Sub

    Public Sub UpdateTipoProductoXEspeci(ByVal currentTipoProductoXEspeci As TipoProductoXEspeci)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTipoProductoXEspeci.pstrUsuarioConexion, currentTipoProductoXEspeci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTipoProductoXEspeci.InfoSesion = DemeInfoSesion(currentTipoProductoXEspeci.pstrUsuarioConexion, "UpdateTipoProductoXEspeci")
            Me.DataContext.TipoProductoXEspecie.Attach(currentTipoProductoXEspeci, Me.ChangeSet.GetOriginal(currentTipoProductoXEspeci))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTipoProductoXEspeci")
        End Try
    End Sub

    Public Sub DeleteTipoProductoXEspeci(ByVal TipoProductoXEspeci As TipoProductoXEspeci)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoProductoXEspeci.pstrUsuarioConexion, TipoProductoXEspeci.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_TipoProductoXEspecie_Eliminar( pCodigoTipoNegocio, DemeInfoSesion(pstrUsuario, "DeleteTipoProductoXEspeci"),0).ToList
            TipoProductoXEspeci.InfoSesion = DemeInfoSesion(TipoProductoXEspeci.pstrUsuarioConexion, "DeleteTipoProductoXEspeci")
            Me.DataContext.TipoProductoXEspecie.Attach(TipoProductoXEspeci)
            Me.DataContext.TipoProductoXEspecie.DeleteOnSubmit(TipoProductoXEspeci)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTipoProductoXEspeci")
        End Try
    End Sub

    Public Function TraerTipoProductoXEspeciPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TipoProductoXEspeci
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TipoProductoXEspeci
            'e.ID = 
            'e.CodigoTipoNegocio = 
            e.IDEspecie = "TODAS"
            e.ValorMaxNegociacion = 0
            e.PermiteNegociar = True
            'e.Actualizacion = 
            'e.Usuario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTipoProductoXEspeciPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Control Horarios"

    Public Function ControlHorarioFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlHorario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ControlHorarios_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "CostosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlHorarioFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ControlHorarioConsultar(ByVal pModulo As String, ByVal pTipoNegocio As String, ByVal pTipoOrden As String, ByVal pTipoProducto As String, ByVal pInstruccion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ControlHorario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ControlHorarios_Consultar(pModulo, pTipoNegocio, pTipoOrden, pTipoProducto, pInstruccion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlHorarioConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlHorarioConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerControlHorarioPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ControlHorario
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ControlHorario
            'e.Idcosto = 
            e.TipoNegocio = "TOD"
            e.TipoOrden = "TOD"
            e.TipoProducto = "TOD"
            e.Instruccion = "TOD"
            'e.Valor = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerControlHorarioPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertControlHorario(ByVal Control As ControlHorario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Control.pstrUsuarioConexion, Control.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Control.InfoSesion = DemeInfoSesion(Control.pstrUsuarioConexion, "InsertControlHorario")
            Me.DataContext.ControlHorarios.InsertOnSubmit(Control)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertControlHorario")
        End Try
    End Sub

    Public Sub UpdateControlHorario(ByVal currentControl As ControlHorario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentControl.pstrUsuarioConexion, currentControl.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentControl.InfoSesion = DemeInfoSesion(currentControl.pstrUsuarioConexion, "UpdateControlHorario")
            Me.DataContext.ControlHorarios.Attach(currentControl, Me.ChangeSet.GetOriginal(currentControl))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateControlHorario")
        End Try
    End Sub

    Public Sub DeleteControlHorario(ByVal Control As ControlHorario)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Control.pstrUsuarioConexion, Control.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.dbo.uspOyDNet_Costos_Eliminar( pCodigoFormapago,  pCodigoTipoCheque,  pCodigoTipoCruce, DemeInfoSesion(pstrUsuario, "DeleteCosto"),0).ToList
            Control.InfoSesion = DemeInfoSesion(Control.pstrUsuarioConexion, "DeleteControlHorario")
            Me.DataContext.ControlHorarios.Attach(Control)
            Me.DataContext.ControlHorarios.DeleteOnSubmit(Control)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteControlHorario")
        End Try
    End Sub
#End Region

#Region "Clientes relacionados"

    Public Function TraerClienteRelacionadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ClientesRelacionadosEncabezado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ClientesRelacionadosEncabezado
            'e.IDMensaje = 
            'e.Usuario = 
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerMensajePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertClientesRelacionadosEncabezado(ByVal currentClientePrincipal As ClientesRelacionadosEncabezado)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesRelacionadosEncabezado")
        End Try
    End Sub

    Public Sub UpdateClientesRelacionadosEncabezado(ByVal currentClientePrincipal As ClientesRelacionadosEncabezado)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesRelacionadosEncabezado")
        End Try
    End Sub

    Public Sub DeleteClientesRelacionadosEncabezado(ByVal currentClientePrincipal As ClientesRelacionadosEncabezado)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesRelacionadosEncabezado")
        End Try
    End Sub

    Public Sub InsertClientesRelacionados(ByVal ClienteRelacionado As ClientesRelacionados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClienteRelacionado.pstrUsuarioConexion, ClienteRelacionado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClienteRelacionado.InfoSesion = DemeInfoSesion(ClienteRelacionado.pstrUsuarioConexion, "InsertClientesRelacionados")
            Me.DataContext.ClientesRelacionados.InsertOnSubmit(ClienteRelacionado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesRelacionados")
        End Try
    End Sub

    Public Sub UpdateClientesRelacionados(ByVal currentClienteRelacionado As ClientesRelacionados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentClienteRelacionado.pstrUsuarioConexion, currentClienteRelacionado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClienteRelacionado.InfoSesion = DemeInfoSesion(currentClienteRelacionado.pstrUsuarioConexion, "UpdateClientesRelacionados")
            Me.DataContext.ClientesRelacionados.Attach(currentClienteRelacionado, Me.ChangeSet.GetOriginal(currentClienteRelacionado))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesRelacionados")
        End Try
    End Sub

    Public Sub DeleteClientesRelacionados(ByVal ClienteRelacionado As ClientesRelacionados)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClienteRelacionado.pstrUsuarioConexion, ClienteRelacionado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClienteRelacionado.InfoSesion = DemeInfoSesion(ClienteRelacionado.pstrUsuarioConexion, "DeleteClientesRelacionados")
            Me.DataContext.ClientesRelacionados.Attach(ClienteRelacionado)
            Me.DataContext.ClientesRelacionados.DeleteOnSubmit(ClienteRelacionado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesRelacionados")
        End Try
    End Sub

    Public Function ClientesRelacionadosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesRelacionadosEncabezado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesRelacionados_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesRelacionadosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesRelacionadosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ClientesRelacionadosConsultar(ByVal pstrTipoIdentificacion As String, ByVal pstrNroDocumento As String, ByVal pstrNombre As String, ByVal pstrUsuario As String, pstrTipoRelacion As String, ByVal pstrInfoConexion As String) As List(Of ClientesRelacionadosEncabezado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim NroDocumento = HttpUtility.UrlDecode(pstrNroDocumento)
            Dim Nombre = HttpUtility.UrlDecode(pstrNombre)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesRelacionados_Consultar(pstrTipoIdentificacion, NroDocumento, Nombre, pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesRelacionadosConsultar"), 0, pstrTipoRelacion).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesRelacionadosConsultar")
            Return Nothing
        End Try
    End Function

    Public Function ClientesRelacionadosConsultar_Relaciones(ByVal pstrTipoIdentificacion As String, ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesRelacionados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesRelacionados_ConsultarRelaciones(pstrTipoIdentificacion, pstrNroDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesRelacionadosConsultar_Relaciones"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesRelacionadosConsultar_Relaciones")
            Return Nothing
        End Try
    End Function

    Public Function ClientesRelacionadosValidar_Relaciones(ByVal pstrTipoIdentificacion As String, ByVal pstrNroDocumento As String, ByVal pstrRelaciones As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidacionClientesRelacionados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesRelacionados_ValidarRelaciones(pstrTipoIdentificacion, pstrNroDocumento, pstrRelaciones, pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesRelacionadosValidar_Relaciones"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesRelacionadosValidar_Relaciones")
            Return Nothing
        End Try
    End Function

    Public Function ClientesRelacionadosEliminar(ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Maestros_ClientesRelacionados_Eliminar(pstrNroDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesRelacionadosEliminar"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesRelacionadosEliminar")
            Return False
        End Try
    End Function

#End Region

#Region "Portafolio x tipo de negocio"

    Public Function TraerPortalioClienteXTipoNegocioPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PortalioClienteXTipoNegocio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New PortalioClienteXTipoNegocio
            'e.IDMensaje = 
            'e.Usuario = 
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerPortalioClienteXTipoNegocioPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatePortalioClienteXTipoNegocioPrincipal(ByVal currentClientePrincipal As PortalioClienteXTipoNegocioPrincipal)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePortalioClienteXTipoNegocioPrincipal")
        End Try
    End Sub

    Public Sub InsertPortalioClienteXTipoNegocio(ByVal PortafolioCliente As PortalioClienteXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,PortafolioCliente.pstrUsuarioConexion, PortafolioCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            PortafolioCliente.InfoSesion = DemeInfoSesion(PortafolioCliente.pstrUsuarioConexion, "InsertPortalioClienteXTipoNegocio")
            Me.DataContext.PortalioClienteXTipoNegocio.InsertOnSubmit(PortafolioCliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPortalioClienteXTipoNegocio")
        End Try
    End Sub

    Public Sub UpdatePortalioClienteXTipoNegocio(ByVal currentPortafolioCliente As PortalioClienteXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentPortafolioCliente.pstrUsuarioConexion, currentPortafolioCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentPortafolioCliente.InfoSesion = DemeInfoSesion(currentPortafolioCliente.pstrUsuarioConexion, "UpdatePortalioClienteXTipoNegocio")
            Me.DataContext.PortalioClienteXTipoNegocio.Attach(currentPortafolioCliente, Me.ChangeSet.GetOriginal(currentPortafolioCliente))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePortalioClienteXTipoNegocio")
        End Try
    End Sub

    Public Sub DeletePortalioClienteXTipoNegocio(ByVal PortafolioCliente As PortalioClienteXTipoNegocio)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,PortafolioCliente.pstrUsuarioConexion, PortafolioCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Mensajes_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteMensaje"),0).ToList
            PortafolioCliente.InfoSesion = DemeInfoSesion(PortafolioCliente.pstrUsuarioConexion, "DeletePortalioClienteXTipoNegocio")
            Me.DataContext.PortalioClienteXTipoNegocio.Attach(PortafolioCliente)
            Me.DataContext.PortalioClienteXTipoNegocio.DeleteOnSubmit(PortafolioCliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePortalioClienteXTipoNegocio")
        End Try
    End Sub

    Public Function PortafolioClientesXTipoNegocioFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PortalioClienteXTipoNegocioPrincipal)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOYDNet_Maestros_PortafolioClienteXTipoNegocio_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "PortafolioClientesXTipoNegocioFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PortafolioClientesXTipoNegocioFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function PortafolioClientesXTipoNegocioConsultar(ByVal pstrTipoProducto As String, ByVal pstrPerfilRiesgo As String, ByVal pstrCodigoOYD As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PortalioClienteXTipoNegocioPrincipal)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim TipoProducto = HttpUtility.UrlDecode(pstrTipoProducto)
            Dim PerfilRiesgo = HttpUtility.UrlDecode(pstrPerfilRiesgo)
            Dim CodigoOYD = HttpUtility.HtmlDecode(pstrCodigoOYD)
            Dim ret = Me.DataContext.uspOYDNet_Maestros_PortafolioClienteXTipoNegocio_Consultar(TipoProducto, PerfilRiesgo, CodigoOYD, pstrUsuario, DemeInfoSesion(pstrUsuario, "PortafolioClientesXTipoNegocioConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PortafolioClientesXTipoNegocioConsultar")
            Return Nothing
        End Try
    End Function

    Public Function PortafolioClientesXTipoNegocioConsultar_Relaciones(ByVal pstrTipoProducto As String, ByVal pstrPerfilRiesgo As String, ByVal pstrCodigoOYD As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PortalioClienteXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOYDNet_Maestros_PortafolioClienteXTipoNegocio_ConsultarRelaciones(pstrTipoProducto, pstrPerfilRiesgo, pstrCodigoOYD, pstrUsuario, DemeInfoSesion(pstrUsuario, "PortafolioClientesXTipoNegocioConsultar_Relaciones"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PortafolioClientesXTipoNegocioConsultar_Relaciones")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Cupo receptor x tipo de negocio"

    Public Function TraerCupoReceptorXTipoNegocioPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CupoReceptorXTipoNegocio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New CupoReceptorXTipoNegocio
            'e.IDMensaje = 
            'e.Usuario = 
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCupoReceptorXTipoNegocioPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub UpdateCupoReceptorXTipoNegocioPrincipal(ByVal currentClientePrincipal As CupoReceptorXTipoNegocioPrincipal)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCupoReceptorXTipoNegocioPrincipal")
        End Try
    End Sub

    Public Sub InsertPortalioClienteXTipoNegocio(ByVal CupoReceptor As CupoReceptorXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CupoReceptor.pstrUsuarioConexion, CupoReceptor.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CupoReceptor.InfoSesion = DemeInfoSesion(CupoReceptor.pstrUsuarioConexion, "InsertPortalioClienteXTipoNegocio")
            Me.DataContext.CupoReceptorXTipoNegocio.InsertOnSubmit(CupoReceptor)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPortalioClienteXTipoNegocio")
        End Try
    End Sub

    Public Sub UpdatePortalioClienteXTipoNegocio(ByVal currentCupoReceptor As CupoReceptorXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCupoReceptor.pstrUsuarioConexion, currentCupoReceptor.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCupoReceptor.InfoSesion = DemeInfoSesion(currentCupoReceptor.pstrUsuarioConexion, "UpdatePortalioClienteXTipoNegocio")
            Me.DataContext.CupoReceptorXTipoNegocio.Attach(currentCupoReceptor, Me.ChangeSet.GetOriginal(currentCupoReceptor))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePortalioClienteXTipoNegocio")
        End Try
    End Sub

    Public Sub DeletePortalioClienteXTipoNegocio(ByVal CupoReceptor As CupoReceptorXTipoNegocio)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CupoReceptor.pstrUsuarioConexion, CupoReceptor.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Mensajes_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteMensaje"),0).ToList
            CupoReceptor.InfoSesion = DemeInfoSesion(CupoReceptor.pstrUsuarioConexion, "DeletePortalioClienteXTipoNegocio")
            Me.DataContext.CupoReceptorXTipoNegocio.Attach(CupoReceptor)
            Me.DataContext.CupoReceptorXTipoNegocio.DeleteOnSubmit(CupoReceptor)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePortalioClienteXTipoNegocio")
        End Try
    End Sub

    Public Function CupoReceptorXTipoNegocioFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CupoReceptorXTipoNegocioPrincipal)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOYDNet_Maestros_CupoReceptorXTipoNegocio_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "CupoReceptorXTipoNegocioFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CupoReceptorXTipoNegocioFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CupoReceptorXTipoNegocioConsultar(ByVal pintIDSucursal As Integer, ByVal pintIDMesa As Integer, ByVal pstrReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CupoReceptorXTipoNegocioPrincipal)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOYDNet_Maestros_CupoReceptorXTipoNegocio_Consultar(pintIDSucursal, pintIDMesa, pstrReceptor, pstrUsuario, DemeInfoSesion(pstrUsuario, "CupoReceptorXTipoNegocioConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CupoReceptorXTipoNegocioConsultar")
            Return Nothing
        End Try
    End Function

    Public Function CupoReceptorXTipoNegocioConsultar_Relaciones(ByVal pintIDSucursal As Integer, ByVal pintIDMesa As Integer, ByVal pstrReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CupoReceptorXTipoNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOYDNet_Maestros_CupoReceptorXTipoNegocio_ConsultarRelaciones(pintIDSucursal, pintIDMesa, pstrReceptor, pstrUsuario, DemeInfoSesion(pstrUsuario, "CupoReceptorXTipoNegocioConsultar_Relaciones"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CupoReceptorXTipoNegocioConsultar_Relaciones")
            Return Nothing
        End Try
    End Function

#End Region

    Public Function ConceptosTesoreriaFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptosTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosTesoreria_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ConceptosTesoreriaFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosTesoreriaFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DoctosRequeridosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DoctosRequerido)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DoctosRequeridos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "DoctosRequeridosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DoctosRequeridosFiltrar")
            Return Nothing
        End Try
    End Function
#Region "PreciosTick"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertPreciosTick(ByVal newPreciosTick As PreciosTick)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newPreciosTick.pstrUsuarioConexion, newPreciosTick.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newPreciosTick.strInfoSesion = DemeInfoSesion(newPreciosTick.pstrUsuarioConexion, "InsertPreciosTick")
            Me.DataContext.PreciosTick.InsertOnSubmit(newPreciosTick)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPreciosTick")
        End Try
    End Sub


    Public Sub UpdatePreciosTick(ByVal currentPreciosTick As PreciosTick)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentPreciosTick.pstrUsuarioConexion, currentPreciosTick.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentPreciosTick.strInfoSesion = DemeInfoSesion(currentPreciosTick.pstrUsuarioConexion, "UpdatePreciosTick")
            Me.DataContext.PreciosTick.Attach(currentPreciosTick, Me.ChangeSet.GetOriginal(currentPreciosTick))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePreciosTick")
        End Try
    End Sub

    Public Sub DeletePreciosTick(ByVal retirarPreciosTick As PreciosTick)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,retirarPreciosTick.pstrUsuarioConexion, retirarPreciosTick.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            retirarPreciosTick.strInfoSesion = DemeInfoSesion(retirarPreciosTick.pstrUsuarioConexion, "retirarPreciosTick")
            Me.DataContext.PreciosTick.Attach(retirarPreciosTick)
            Me.DataContext.PreciosTick.DeleteOnSubmit(retirarPreciosTick)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePreciosTick")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function PreciosTick_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreciosTick)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRetorno = Me.DataContext.uspOyDNet_Maestros_PreciosTick_Filtrar(pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "PreciosTick_Filtrar"), ClsConstantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PreciosTick_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function PreciosTick_Consultar(ByVal pdblPrecioInicial As Double, ByVal pdblPrecioFinal As Double, ByVal pdblMultiplos As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreciosTick)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_PreciosTick_Consultar(String.Empty, pdblPrecioInicial, pdblPrecioFinal, pdblMultiplos, pstrUsuario, DemeInfoSesion(pstrUsuario, "PreciosTick_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PreciosTick_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function PreciosTick_ConsultarPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PreciosTick
        Dim objPreciosTick As PreciosTick = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_PreciosTick_Consultar(ClsConstantes.CONSULTAR_DATOS_POR_DEFECTO, 0, 0, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "PreciosTick_ConsultarPorDefecto"), ClsConstantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objPreciosTick = ret.FirstOrDefault
            End If
            Return objPreciosTick
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PreciosTick_ConsultarPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function PreciosTick_FiltrarSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreciosTick)
        Dim objTask As Task(Of List(Of PreciosTick)) = Me.PreciosTick_FiltrarAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function PreciosTick_FiltrarAsync(ByVal pstrFiltro As String, pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of PreciosTick))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of PreciosTick)) = New TaskCompletionSource(Of List(Of PreciosTick))()
        objTaskComplete.TrySetResult(PreciosTick_Filtrar(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function PreciosTick_ConsultarSync(ByVal pdblPrecioInicial As Double, ByVal pdblPrecioFinal As Double, ByVal pdblMultiplos As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreciosTick)
        Dim objTask As Task(Of List(Of PreciosTick)) = Me.PreciosTick_ConsultarAsync(pdblPrecioInicial, pdblPrecioFinal, pdblMultiplos, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function PreciosTick_ConsultarAsync(ByVal pdblPrecioInicial As Double, ByVal pdblPrecioFinal As Double, ByVal pdblMultiplos As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of PreciosTick))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of PreciosTick)) = New TaskCompletionSource(Of List(Of PreciosTick))()
        objTaskComplete.TrySetResult(PreciosTick_Consultar(pdblPrecioInicial, pdblPrecioFinal, pdblMultiplos, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function PreciosTick_ConsultarPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PreciosTick
        Dim objTask As Task(Of PreciosTick) = Me.PreciosTick_ConsultarPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function PreciosTick_ConsultarPorDefectoAsync(pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of PreciosTick)
        Dim objTaskComplete As TaskCompletionSource(Of PreciosTick) = New TaskCompletionSource(Of PreciosTick)()
        objTaskComplete.TrySetResult(PreciosTick_ConsultarPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region
#Region "Control Horario Fondos"
    Public Function ControlHorarioFondosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlHorarioFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ControlHorariosFondos_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlHorarioFondosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlHorarioFondosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ControlHorarioFondosConsultar(ByVal pstrCodigoFondo As String, ByVal pstrTipoMovimiento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblControlHorarioFondos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ControlHorariosFondos_Consultar(pstrCodigoFondo, pstrTipoMovimiento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ControlHorarioFondosConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ControlHorarioFondosConsultar")
            Return Nothing
        End Try
    End Function

#End Region
#Region "Forma Pagos Fondos"

    Public Function FormasPagosFondosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblFondosFormasPagos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_FondosFormasPagos_Filtrar(Filtro, pstrUsuario, DemeInfoSesion(pstrUsuario, "FormasPagosFondosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FormasPagosFondosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function FormasPagosFondosConsultar(ByVal pstrDescripcion As String, ByVal pstrTipoMovimiento As String, pstrTipoTransaccion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblFondosFormasPagos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_Maestros_FondosFormasPagos_Consultar(pstrDescripcion, pstrTipoMovimiento, pstrTipoTransaccion, pstrUsuario, DemeInfoSesion(pstrUsuario, "FormasPagosFondosConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FormasPagosFondosConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerControlHorarioFondosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As tblControlHorarioFondos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New tblControlHorarioFondos
            'e.Idcosto = 
            e.CodigoFondo = ""
            e.HoraFin = "23:00"
            e.HoraInicio = "18:00"

            'e.Valor = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerControlHorarioFondosPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region
#Region "Grupo Operador"

    Public Function GrupoOperadorConsultar(ByVal strNombre As String, ByVal strUsuario As String, ByVal strFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of GrupoOperadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspGrupoOperador_Consultar(strNombre, strUsuario, DemeInfoSesion(pstrUsuario, "GrupoOperadorConsultar"), 0, strFiltro)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrupoOperadorConsultar")
            Return Nothing
        End Try
    End Function

    Public Function GrupoOperadorFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of GrupoOperadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspGrupoOperador_Consultar("", "", DemeInfoSesion(pstrUsuario, "GrupoOperadorFiltrar"), 0, pstrFiltro)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrupoOperadorFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function GrupoOperadorModificar(ByVal plngIDGrupoOperador As Integer, ByVal pstrNombreGrupo As String, ByVal pstrDetalle As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDPLUSMaestros.GrupoOperadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspGrupoOperador_Modificar(plngIDGrupoOperador, pstrNombreGrupo, pstrDetalle, pstrUsuario, DemeInfoSesion(pstrUsuario, "GrupoOperadorModificar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrupoOperadorModificar")
            Return Nothing
        End Try
    End Function

    Public Function GrupoOperadorEliminar(ByVal plngIDGrupoOperador As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDPLUSMaestros.GrupoOperadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspGrupoOperador_Eliminar(plngIDGrupoOperador, pstrUsuario, DemeInfoSesion(pstrUsuario, "GrupoOperadorEliminar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrupoOperadorEliminar")
            Return Nothing
        End Try
    End Function

#Region "Detalle Grupo Operador"
    Public Sub UpdateDetalleGrupoOperadores(ByVal DetalleGrupoOperadores As DetalleGrupoOperadores)

    End Sub

    Public Function DetalleGruposOperadoresrConsultar(ByVal lngIDGrupoOperador As Int32, ByVal strUsuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleGrupoOperadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspDetalleGruposOperadores_Consultar(lngIDGrupoOperador, strUsuario, DemeInfoSesion(pstrUsuario, "DetalleGruposOperadoresrConsultar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleGruposOperadoresrConsultar")
            Return Nothing
        End Try
    End Function

    Public Function DetalleGruposOperadoresrEliminar(ByVal lngIDGrupoOperador As Int32, ByVal strUsuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleGrupoOperadores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspDetalleGruposOperadores_Eliminar(lngIDGrupoOperador, strUsuario, DemeInfoSesion(pstrUsuario, "DetalleGruposOperadoresrEliminar"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleGruposOperadoresrEliminar")
            Return Nothing
        End Try
    End Function

#End Region
#End Region

#End Region

End Class