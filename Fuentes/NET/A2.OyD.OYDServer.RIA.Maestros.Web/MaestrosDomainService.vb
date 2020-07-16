
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
Partial Public Class MaestrosDomainService
    Inherits LinqToSqlDomainService(Of OyDDataContext)

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

#Region "JUAN CARLOS SOTO MAESTROS"

#Region "V1.0.0"

#Region "Sucursales"

    Public Function SucursalesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Sucursale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Sucursales_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "SucursalesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "SucursalesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function SucursalesConsultar(ByVal pIDSucursal As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Sucursale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Sucursales_Consultar(pIDSucursal, pNombre, DemeInfoSesion(pstrUsuario, "BuscarSucursales"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarSucursales")
            Return Nothing
        End Try
    End Function

    Public Function TraerSucursalePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Sucursale
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Sucursale
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IDSucursal = -1
            'e.Nombre = 
            'e.PorcentajePatrimonioTecnico =
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDSuc = 
            Return (e)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerSucursalePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertSucursale(ByVal Sucursale As Sucursale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Sucursale.pstrUsuarioConexion, Sucursale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Sucursale.InfoSesion = DemeInfoSesion(Sucursale.pstrUsuarioConexion, "InsertSucursale")
            Me.DataContext.Sucursales.InsertOnSubmit(Sucursale)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertSucursale")
        End Try
    End Sub

    Public Sub UpdateSucursale(ByVal currentSucursale As Sucursale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentSucursale.pstrUsuarioConexion, currentSucursale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentSucursale.InfoSesion = DemeInfoSesion(currentSucursale.pstrUsuarioConexion, "UpdateSucursale")
            Me.DataContext.Sucursales.Attach(currentSucursale, Me.ChangeSet.GetOriginal(currentSucursale))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateSucursale")
        End Try
    End Sub

    Public Sub DeleteSucursale(ByVal Sucursale As Sucursale)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Sucursale.pstrUsuarioConexion, Sucursale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Sucursales_Eliminar( pIDSucursal,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteSucursale"),0).ToList
            Sucursale.InfoSesion = DemeInfoSesion(Sucursale.pstrUsuarioConexion, "DeleteSucursale")
            Me.DataContext.Sucursales.Attach(Sucursale)
            Me.DataContext.Sucursales.DeleteOnSubmit(Sucursale)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteSucursale")
        End Try
    End Sub

#End Region
#Region "TiposEntidad"

    Public Function TiposEntidadFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposEntida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TiposEntidad_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "TiposEntidadFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TiposEntidadFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TiposEntidadConsultar(ByVal pIDTipoEntidad As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposEntida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TiposEntidad_Consultar(pIDTipoEntidad, pNombre, DemeInfoSesion(pstrUsuario, "BuscarTiposEntidad"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarTiposEntidad")
            Return Nothing
        End Try
    End Function

    Public Function TraerTiposEntidaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TiposEntida
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TiposEntida
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IDTipoEntidad = -1
            'e.Nombre = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IdTipoEntidadI = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTiposEntidaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTiposEntida(ByVal TiposEntida As TiposEntida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, TiposEntida.pstrUsuarioConexion, TiposEntida.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TiposEntida.InfoSesion = DemeInfoSesion(TiposEntida.pstrUsuarioConexion, "InsertTiposEntida")
            Me.DataContext.TiposEntidad.InsertOnSubmit(TiposEntida)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTiposEntida")
        End Try
    End Sub

    Public Sub UpdateTiposEntida(ByVal currentTiposEntida As TiposEntida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentTiposEntida.pstrUsuarioConexion, currentTiposEntida.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTiposEntida.InfoSesion = DemeInfoSesion(currentTiposEntida.pstrUsuarioConexion, "UpdateTiposEntida")
            Me.DataContext.TiposEntidad.Attach(currentTiposEntida, Me.ChangeSet.GetOriginal(currentTiposEntida))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTiposEntida")
        End Try
    End Sub

    Public Sub DeleteTiposEntida(ByVal TiposEntida As TiposEntida)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TiposEntida.pstrUsuarioConexion, TiposEntida.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_TiposEntidad_Eliminar( pIDTipoEntidad,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteTiposEntida"),0).ToList
            TiposEntida.InfoSesion = DemeInfoSesion(TiposEntida.pstrUsuarioConexion, "DeleteTiposEntida")
            Me.DataContext.TiposEntidad.Attach(TiposEntida)
            Me.DataContext.TiposEntidad.DeleteOnSubmit(TiposEntida)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTiposEntida")
        End Try
    End Sub
#End Region
#Region "ProductosValores"

    Public Function ProductosValoresFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProductosValore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ProductosValores_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ProductosValoresFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ProductosValoresFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ProductosValoresConsultar(ByVal pIDTipoProducto As Integer, ByVal pDescripcion As String, ByVal pOrden As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ProductosValore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ProductosValores_Consultar(pIDTipoProducto, pDescripcion, pOrden, DemeInfoSesion(pstrUsuario, "BuscarProductosValores"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarProductosValores")
            Return Nothing
        End Try
    End Function

    Public Function TraerProductosValorePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ProductosValore
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ProductosValore
            e.IDTipoProducto = -1
            'e.Descripcion = 
            'e.Orden = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            'e.IdProductoValores = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerProductosValorePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertProductosValore(ByVal ProductosValore As ProductosValore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ProductosValore.pstrUsuarioConexion, ProductosValore.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ProductosValore.InfoSesion = DemeInfoSesion(ProductosValore.pstrUsuarioConexion, "InsertProductosValore")
            Me.DataContext.ProductosValores.InsertOnSubmit(ProductosValore)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertProductosValore")
        End Try
    End Sub

    Public Sub UpdateProductosValore(ByVal currentProductosValore As ProductosValore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentProductosValore.pstrUsuarioConexion, currentProductosValore.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentProductosValore.InfoSesion = DemeInfoSesion(currentProductosValore.pstrUsuarioConexion, "UpdateProductosValore")
            Me.DataContext.ProductosValores.Attach(currentProductosValore, Me.ChangeSet.GetOriginal(currentProductosValore))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateProductosValore")
        End Try
    End Sub

    Public Sub DeleteProductosValore(ByVal ProductosValore As ProductosValore)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ProductosValore.pstrUsuarioConexion, ProductosValore.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ProductosValores_Eliminar( pIDTipoProducto,  pDescripcion,  pOrden, DemeInfoSesion(pstrUsuario, "DeleteProductosValore"),0).ToList
            ProductosValore.InfoSesion = DemeInfoSesion(ProductosValore.pstrUsuarioConexion, "DeleteProductosValore")
            Me.DataContext.ProductosValores.Attach(ProductosValore)
            Me.DataContext.ProductosValores.DeleteOnSubmit(ProductosValore)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteProductosValore")
        End Try
    End Sub
#End Region
#Region "ConceptosInactividad"

    Public Function ConceptosInactividadFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptosInactivida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosInactividad_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ConceptosInactividadFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosInactividadFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosInactividadConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptosInactivida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosInactividad_Consultar(pID, pNombre, DemeInfoSesion(pstrUsuario, "BuscarConceptosInactividad"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarConceptosInactividad")
            Return Nothing
        End Try
    End Function

    Public Function TraerConceptosInactividaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConceptosInactivida
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ConceptosInactivida
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = -1
            e.Actividad = True
            'e.Nombre = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IdConceptoInactividad = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerConceptosInactividaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConceptosInactivida(ByVal ConceptosInactivida As ConceptosInactivida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ConceptosInactivida.pstrUsuarioConexion, ConceptosInactivida.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ConceptosInactivida.InfoSesion = DemeInfoSesion(ConceptosInactivida.pstrUsuarioConexion, "InsertConceptosInactivida")
            Me.DataContext.ConceptosInactividad.InsertOnSubmit(ConceptosInactivida)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConceptosInactivida")
        End Try
    End Sub

    Public Sub UpdateConceptosInactivida(ByVal currentConceptosInactivida As ConceptosInactivida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentConceptosInactivida.pstrUsuarioConexion, currentConceptosInactivida.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConceptosInactivida.InfoSesion = DemeInfoSesion(currentConceptosInactivida.pstrUsuarioConexion, "UpdateConceptosInactivida")
            Me.DataContext.ConceptosInactividad.Attach(currentConceptosInactivida, Me.ChangeSet.GetOriginal(currentConceptosInactivida))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConceptosInactivida")
        End Try
    End Sub

    Public Sub DeleteConceptosInactivida(ByVal ConceptosInactivida As ConceptosInactivida)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConceptosInactivida.pstrUsuarioConexion, ConceptosInactivida.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConceptosInactividad_Eliminar( pID,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteConceptosInactivida"),0).ToList
            ConceptosInactivida.InfoSesion = DemeInfoSesion(ConceptosInactivida.pstrUsuarioConexion, "DeleteConceptosInactivida")
            Me.DataContext.ConceptosInactividad.Attach(ConceptosInactivida)
            Me.DataContext.ConceptosInactividad.DeleteOnSubmit(ConceptosInactivida)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConceptosInactivida")
        End Try
    End Sub

    'Public Function EliminarResolucionesFacturas(ByVal pIDCodigoResolucion As Integer, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS String
    '    Try
    '        Dim ret = Me.DataContext.uspOyDNet_Maestros_ResolucionesFacturas_Eliminar(pIDCodigoResolucion, mensaje, DemeInfoSesion(pstrUsuario, "EliminarResolucionesFacturas"), 0)
    '        Return mensaje
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "EliminarResolucionesFacturas")
    '        Return Nothing
    '    End Try
    'End Function

    Public Function EliminarConceptosInactividad(ByVal pintIdConceptosInactividad As Short, ByVal strMsg As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosInactividad_Eliminar(pintIdConceptosInactividad, strMsg, DemeInfoSesion(pstrUsuario, "EliminarConceptosInactividad"), 0)
            Return strMsg
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarConceptosInactividad")
            Return Nothing
        End Try
    End Function
#End Region

#End Region

#Region "V1.0.1"

#Region "Bolsas"

    Public Function BolsasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Bolsa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Bolsas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "BolsasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BolsasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function BolsasConsultar(ByVal pIdBolsa As Integer, ByVal pNombre As String, ByVal plngIDPoblacion As Integer, ByVal plngNroDocumento As Decimal, ByVal plogMercadoIntegrado? As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Bolsa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Bolsas_Consultar(pIdBolsa, pNombre, plngIDPoblacion, plngNroDocumento, plogMercadoIntegrado, DemeInfoSesion(pstrUsuario, "BuscarBolsas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBolsas")
            Return Nothing
        End Try
    End Function

    Public Function TraerBolsaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Bolsa
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Bolsa
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IdBolsa = -1
            'e.Nombre = 
            'e.IDPoblacion = 
            'e.NroDocumento = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.MercadoIntegrado = 
            'e.Activa = CType(1, Boolean?)
            'e.IDBolsa = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerBolsaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertBolsa(ByVal Bolsa As Bolsa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Bolsa.pstrUsuarioConexion, Bolsa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Bolsa.InfoSesion = DemeInfoSesion(Bolsa.pstrUsuarioConexion, "InsertBolsa")
            Me.DataContext.Bolsas.InsertOnSubmit(Bolsa)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertBolsa")
        End Try
    End Sub

    Public Sub UpdateBolsa(ByVal currentBolsa As Bolsa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentBolsa.pstrUsuarioConexion, currentBolsa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentBolsa.InfoSesion = DemeInfoSesion(currentBolsa.pstrUsuarioConexion, "UpdateBolsa")
            Me.DataContext.Bolsas.Attach(currentBolsa, Me.ChangeSet.GetOriginal(currentBolsa))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateBolsa")
        End Try
    End Sub

    Public Sub DeleteBolsa(ByVal Bolsa As Bolsa)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Bolsa.pstrUsuarioConexion, Bolsa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Bolsas_Eliminar( pIdBolsa,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteBolsa"),0).ToList
            Bolsa.InfoSesion = DemeInfoSesion(Bolsa.pstrUsuarioConexion, "DeleteBolsa")
            Me.DataContext.Bolsas.Attach(Bolsa)
            Me.DataContext.Bolsas.DeleteOnSubmit(Bolsa)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteBolsa")
        End Try
    End Sub

    Public Function EliminarBolsa(ByVal Bolsa As Integer, ByVal nombre As String, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Bolsas_Eliminar(Bolsa, nombre, mensaje, DemeInfoSesion(pstrUsuario, "EliminarBolsa"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarBolsa")
            Return Nothing
        End Try
    End Function
#End Region
#Region "BancosNacionales"

    Public Function BancosNacionalesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BancosNacionale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BancosNacionales_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "BancosNacionalesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BancosNacionalesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function BancosNacionalesConsultar(ByVal pId As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BancosNacionale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BancosNacionales_Consultar(pId, pNombre, DemeInfoSesion(pstrUsuario, "BuscarBancosNacionales"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBancosNacionales")
            Return Nothing
        End Try
    End Function

    Public Function TraerBancosNacionalePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As BancosNacionale
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New BancosNacionale
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.Id = -1
            'e.CodACH = 
            'e.Nombre = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDBancoNacional = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerBancosNacionalePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertBancosNacionale(ByVal BancosNacionale As BancosNacionale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, BancosNacionale.pstrUsuarioConexion, BancosNacionale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            BancosNacionale.InfoSesion = DemeInfoSesion(BancosNacionale.pstrUsuarioConexion, "InsertBancosNacionale")
            Me.DataContext.BancosNacionales.InsertOnSubmit(BancosNacionale)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertBancosNacionale")
        End Try
    End Sub

    Public Sub UpdateBancosNacionale(ByVal currentBancosNacionale As BancosNacionale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentBancosNacionale.pstrUsuarioConexion, currentBancosNacionale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentBancosNacionale.InfoSesion = DemeInfoSesion(currentBancosNacionale.pstrUsuarioConexion, "UpdateBancosNacionale")
            Me.DataContext.BancosNacionales.Attach(currentBancosNacionale, Me.ChangeSet.GetOriginal(currentBancosNacionale))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateBancosNacionale")
        End Try
    End Sub

    Public Sub DeleteBancosNacionale(ByVal BancosNacionale As BancosNacionale)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,BancosNacionale.pstrUsuarioConexion, BancosNacionale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_BancosNacionales_Eliminar( pId,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteBancosNacionale"),0).ToList
            BancosNacionale.InfoSesion = DemeInfoSesion(BancosNacionale.pstrUsuarioConexion, "DeleteBancosNacionale")
            Me.DataContext.BancosNacionales.Attach(BancosNacionale)
            Me.DataContext.BancosNacionales.DeleteOnSubmit(BancosNacionale)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteBancosNacionale")
        End Try
    End Sub

    Public Function Traer_RelacionesCodBancos_BancosNacionale(ByVal pId As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RelacionesCodBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_RelacionesCodBancosBancosNacionale_Consultar(pId, DemeInfoSesion(pstrUsuario, "Traer_RelacionesCodBancos_BancosNacionale"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_RelacionesCodBancos_BancosNacionale")
            Return Nothing
        End Try
    End Function

    Public Function EliminarBancosNacionales(ByVal pintIDBancoNacional As Integer, ByVal pstrUsuario As String, ByVal strMsg As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BancosNacionales_Eliminar(pintIDBancoNacional, pstrUsuario, strMsg, DemeInfoSesion(pstrUsuario, "EliminarBancosNacionales"), 0)
            Return strMsg
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarBancosNacionales")
            Return Nothing
        End Try
    End Function
#End Region
#Region "RelacionesCodBancos"

    Public Function RelacionesCodBancosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RelacionesCodBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_RelacionesCodBancos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "RelacionesCodBancosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RelacionesCodBancosFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerRelacionesCodBancoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As RelacionesCodBanco
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New RelacionesCodBanco
            'e.IdComisionista = 
            'e.IdSucComisionista = 
            e.IdCodBanco = -1
            e.intIDRelacionCodBanco = -1
            'e.RelTecno = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerRelacionesCodBancoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertRelacionesCodBanco(ByVal RelacionesCodBanco As RelacionesCodBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, RelacionesCodBanco.pstrUsuarioConexion, RelacionesCodBanco.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            RelacionesCodBanco.InfoSesion = DemeInfoSesion(RelacionesCodBanco.pstrUsuarioConexion, "InsertRelacionesCodBanco")
            RelacionesCodBanco.Usuario = HttpContext.Current.User.Identity.Name
            Me.DataContext.RelacionesCodBancos.InsertOnSubmit(RelacionesCodBanco)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertRelacionesCodBanco")
        End Try
    End Sub

    Public Sub UpdateRelacionesCodBanco(ByVal currentRelacionesCodBanco As RelacionesCodBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentRelacionesCodBanco.pstrUsuarioConexion, currentRelacionesCodBanco.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentRelacionesCodBanco.InfoSesion = DemeInfoSesion(currentRelacionesCodBanco.pstrUsuarioConexion, "UpdateRelacionesCodBanco")
            currentRelacionesCodBanco.Usuario = HttpContext.Current.User.Identity.Name
            Me.DataContext.RelacionesCodBancos.Attach(currentRelacionesCodBanco, Me.ChangeSet.GetOriginal(currentRelacionesCodBanco))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateRelacionesCodBanco")
        End Try
    End Sub

    Public Sub DeleteRelacionesCodBanco(ByVal RelacionesCodBanco As RelacionesCodBanco)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,RelacionesCodBanco.pstrUsuarioConexion, RelacionesCodBanco.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_RelacionesCodBancos_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteRelacionesCodBanco"),0).ToList
            RelacionesCodBanco.InfoSesion = DemeInfoSesion(RelacionesCodBanco.pstrUsuarioConexion, "DeleteRelacionesCodBanco")
            Me.DataContext.RelacionesCodBancos.Attach(RelacionesCodBanco)
            Me.DataContext.RelacionesCodBancos.DeleteOnSubmit(RelacionesCodBanco)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteRelacionesCodBanco")
        End Try
    End Sub
#End Region
#Region "Clasificaciones"

    Public Function ClasificacionesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Clasificacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Clasificaciones_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ClasificacionesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClasificacionesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ClasificacionesConsultar(ByVal pCódigo As Integer, ByVal pNombre As String, ByVal pstrAplicaA As String,
                                             ByVal plogEsGrupo As Boolean, ByVal plogEsSector As Boolean, ByVal plngIDPerteneceA As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Clasificacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Clasificaciones_Consultar(pCódigo, pNombre, pstrAplicaA, plogEsGrupo, plogEsSector, plngIDPerteneceA, DemeInfoSesion(pstrUsuario, "BuscarClasificaciones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClasificaciones")
            Return Nothing
        End Try
    End Function

    Public Function TraerClasificacionPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Clasificacion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Clasificacion
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.Código = -1
            'e.Nombre = 
            'e.EsGrupo = 
            'e.EsSector = 
            'e.IDPerteneceA = 
            'e.AplicaA = 
            'e.Nemo = 
            'e.Actualizacion = 
            'e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDClasificacion = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClasificacionPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertClasificacion(ByVal Clasificacion As Clasificacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Clasificacion.pstrUsuarioConexion, Clasificacion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Clasificacion.InfoSesion = DemeInfoSesion(Clasificacion.pstrUsuarioConexion, "InsertClasificacion")
            Me.DataContext.Clasificaciones.InsertOnSubmit(Clasificacion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClasificacion")
        End Try
    End Sub

    Public Sub UpdateClasificacion(ByVal currentClasificacion As Clasificacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClasificacion.pstrUsuarioConexion, currentClasificacion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClasificacion.InfoSesion = DemeInfoSesion(currentClasificacion.pstrUsuarioConexion, "UpdateClasificacion")
            Me.DataContext.Clasificaciones.Attach(currentClasificacion, Me.ChangeSet.GetOriginal(currentClasificacion))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClasificacion")
        End Try
    End Sub

    Public Sub DeleteClasificacion(ByVal Clasificacion As Clasificacion)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Clasificacion.pstrUsuarioConexion, Clasificacion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Clasificaciones_Eliminar( pCódigo,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteClasificacion"),0).ToList
            Clasificacion.InfoSesion = DemeInfoSesion(Clasificacion.pstrUsuarioConexion, "DeleteClasificacion")
            Me.DataContext.Clasificaciones.Attach(Clasificacion)
            Me.DataContext.Clasificaciones.DeleteOnSubmit(Clasificacion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClasificacion")
        End Try
    End Sub

    Public Function EliminarClasificacion(ByVal pintIDClasificacion As Integer, ByVal strMsg As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Clasificaciones_Eliminar(pintIDClasificacion, strMsg, DemeInfoSesion(pstrUsuario, "EliminarBancosNacionales"), 0)
            Return strMsg
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarClasificación")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Comisionistas"

    Public Function ComisionistasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Comisionista)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Comisionistas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ComisionistasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ComisionistasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ComisionistasConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pstrRepresentanteLegal As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Comisionista)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Comisionistas_Consultar(pID, pNombre, pstrRepresentanteLegal, DemeInfoSesion(pstrUsuario, "BuscarComisionistas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarComisionistas")
            Return Nothing
        End Try
    End Function

    Public Function TraerComisionistaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Comisionista
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Comisionista
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IdBolsa = 4
            e.ID = -1
            'e.NroDocumento = 
            'e.Nombre = 
            'e.RepresentanteLegal = 
            'e.Telefono1 = 
            'e.Telefono2 = 
            'e.Fax1 = 
            'e.Fax2 = 
            'e.Direccion = 
            'e.Internet = 
            'e.EMail = 
            'e.IDPoblacion = 586
            'e.IDDepartamento = 
            'e.IDPais = 
            'e.Notas = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDComisionista = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerComisionistaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertComisionista(ByVal Comisionista As Comisionista)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Comisionista.pstrUsuarioConexion, Comisionista.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Comisionista.InfoSesion = DemeInfoSesion(Comisionista.pstrUsuarioConexion, "InsertComisionista")
            Me.DataContext.Comisionistas.InsertOnSubmit(Comisionista)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertComisionista")
        End Try
    End Sub

    Public Sub UpdateComisionista(ByVal currentComisionista As Comisionista)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentComisionista.pstrUsuarioConexion, currentComisionista.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentComisionista.InfoSesion = DemeInfoSesion(currentComisionista.pstrUsuarioConexion, "UpdateComisionista")
            Me.DataContext.Comisionistas.Attach(currentComisionista, Me.ChangeSet.GetOriginal(currentComisionista))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateComisionista")
        End Try
    End Sub

    Public Sub DeleteComisionista(ByVal Comisionista As Comisionista)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Comisionista.pstrUsuarioConexion, Comisionista.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Comisionistas_Eliminar( pID,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteComisionista"),0).ToList
            Comisionista.InfoSesion = DemeInfoSesion(Comisionista.pstrUsuarioConexion, "DeleteComisionista")
            Me.DataContext.Comisionistas.Attach(Comisionista)
            Me.DataContext.Comisionistas.DeleteOnSubmit(Comisionista)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteComisionista")
        End Try
    End Sub

    Public Function EliminarComisionista(ByVal pID As Integer, ByVal strMsg As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Comisionistas_Eliminar(pID, strMsg, DemeInfoSesion(pstrUsuario, "Eliminarcomisionista"), 0)
            Return strMsg
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarComisionista")
            Return Nothing
        End Try
    End Function
#End Region
#Region "ConsecutivosDocumentos"

    Public Function ConsecutivosDocumentosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsecutivosDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConsecutivosDocumentos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ConsecutivosDocumentosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsecutivosDocumentosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ConsecutivosDocumentosConsultar(ByVal pDocumento As String, ByVal pNombreConsecutivo As String, ByVal pDescripcion As String, ByVal pCuentaContable As Boolean, ByVal pCuentaContable1 As String, ByVal pPermiteCliente As String, ByVal pTipoCuenta As String, ByVal psucursalConciliacion As String, ByVal pIdSucursalSuvalor As Integer, ByVal pConcepto As Boolean, ByVal pComprobanteContable As String, ByVal pIncluidoEnExtractoBanco As Boolean, ByVal pIncluidoEnExtractoCliente As Boolean, ByVal pIdMoneda As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsecutivosDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConsecutivosDocumentos_Consultar(pDocumento, pNombreConsecutivo, pDescripcion, pCuentaContable, pCuentaContable1, pPermiteCliente, pTipoCuenta, psucursalConciliacion, pIdSucursalSuvalor, pConcepto, pComprobanteContable, pIncluidoEnExtractoBanco, pIncluidoEnExtractoCliente, pIdMoneda, DemeInfoSesion(pstrUsuario, "BuscarConsecutivosDocumentos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarConsecutivosDocumentos")
            Return Nothing
        End Try
    End Function

    Public Function BuscadorConsecutivosDocumentos(ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsecutivosDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Buscador_ConsecutivosDocumentos(pstrNombreConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscadorConsecutivosDocumentos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscadorConsecutivosDocumentos")
            Return Nothing
        End Try
    End Function

    Public Function TraerConsecutivosDocumentoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConsecutivosDocumento
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ConsecutivosDocumento

            Dim strSQL = "SELECT lngId as strCampo FROM tblMonedas WHERE strDescripcion = @p0 "
            Dim valor = fnEjecutarQuerySQL(strSQL, "PESOS COLOMBIANOS")
            If Not String.IsNullOrEmpty(valor) Then
                e.IdMoneda = CInt(valor)
            End If

            Dim strSQLNroCompanias = "SELECT COUNT(1) as strCampo FROM cf.tblCompanias "
            Dim NroCompañias = fnEjecutarQuerySQL(strSQLNroCompanias)
            If CInt(NroCompañias) = 1 Then
                Dim strSQLCodigoCompania = "SELECT intIDCompania as strCampo FROM cf.tblCompanias "
                Dim intIDCompañia = fnEjecutarQuerySQL(strSQLCodigoCompania)
                e.Compania = CInt(intIDCompañia)

                Dim strSQLNombreCompania = "SELECT strNombre as strCampo FROM cf.tblCompanias "
                Dim strNombreCompañia = fnEjecutarQuerySQL(strSQLNombreCompania)
                e.NombreCompania = strNombreCompañia
            Else
                e.Compania = 0
                e.NombreCompania = String.Empty
            End If

            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Documento = 
            'e.NombreConsecutivo = 
            'e.Descripcion = 
            'e.Cliente = 
            'e.CuentaContable = 
            'e.CuentaContable = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.PermiteCliente = 
            'e.TipoCuenta = 
            'e.IdTarifa = 
            'e.Signo = 
            'e.sucursalConciliacion = 
            'e.IdSucursalSuvalor = 
            'e.Concepto = 
            'e.ComprobanteContable = 
            'e.IncluidoEnExtractoBanco = 
            'e.IdMoneda = 
            'e.IDConsecutivoDocumento = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerConsecutivosDocumentoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConsecutivosDocumento(ByVal ConsecutivosDocumento As ConsecutivosDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ConsecutivosDocumento.pstrUsuarioConexion, ConsecutivosDocumento.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ConsecutivosDocumento.InfoSesion = DemeInfoSesion(ConsecutivosDocumento.pstrUsuarioConexion, "InsertConsecutivosDocumento")
            Me.DataContext.ConsecutivosDocumentos.InsertOnSubmit(ConsecutivosDocumento)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConsecutivosDocumento")
        End Try
    End Sub

    Public Sub UpdateConsecutivosDocumento(ByVal currentConsecutivosDocumento As ConsecutivosDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentConsecutivosDocumento.pstrUsuarioConexion, currentConsecutivosDocumento.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConsecutivosDocumento.InfoSesion = DemeInfoSesion(currentConsecutivosDocumento.pstrUsuarioConexion, "UpdateConsecutivosDocumento")
            Me.DataContext.ConsecutivosDocumentos.Attach(currentConsecutivosDocumento, Me.ChangeSet.GetOriginal(currentConsecutivosDocumento))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConsecutivosDocumento")
        End Try
    End Sub

    Public Sub DeleteConsecutivosDocumento(ByVal ConsecutivosDocumento As ConsecutivosDocumento)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConsecutivosDocumento.pstrUsuarioConexion, ConsecutivosDocumento.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConsecutivosDocumentos_Eliminar( pDocumento,  pNombreConsecutivo,  pDescripcion,  pCuentaContable,  pCuentaContable,  pPermiteCliente,  pTipoCuenta,  psucursalConciliacion,  pIdSucursalSuvalor,  pConcepto,  pComprobanteContable,  pIncluidoEnExtractoBanco,  pIdMoneda, DemeInfoSesion(pstrUsuario, "DeleteConsecutivosDocumento"),0).ToList
            ConsecutivosDocumento.InfoSesion = DemeInfoSesion(ConsecutivosDocumento.pstrUsuarioConexion, "DeleteConsecutivosDocumento")
            Me.DataContext.ConsecutivosDocumentos.Attach(ConsecutivosDocumento)
            Me.DataContext.ConsecutivosDocumentos.DeleteOnSubmit(ConsecutivosDocumento)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConsecutivosDocumento")
        End Try
    End Sub
#End Region
#Region "CuentasContablesOyD"

    Public Function CuentasContablesOyDFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasContablesOy)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Tipo As String = ""
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CuentasContables_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "CuentasContablesOyDFiltrar"), 0, Tipo).ToList
            ret.Add(New CuentasContablesOy With {.actualizacion = Now, .DctoAsociado = String.Empty, .ID = String.Empty, .IDComisionista = 0, .IDCuentaContable = -1,
                                                 .IDSucComisionista = 0, .InfoSesion = String.Empty, .Msg = Tipo, .Naturaleza = "", .Nombre = "", .Usuario = ""
                                                })
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasContablesOyDFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CuentasContablesOyDConsultar(ByVal pID As String, ByVal pNombre As String, ByVal pNaturaleza As String, ByVal pDctoAsociado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasContablesOy)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CuentasContables_Consultar(pID, pNombre, pNaturaleza, pDctoAsociado, DemeInfoSesion(pstrUsuario, "BuscarCuentasContablesOyD"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCuentasContablesOyD")
            Return Nothing
        End Try
    End Function

    Public Function TraerCuentasContablesOyPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CuentasContablesOy
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New CuentasContablesOy
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.ID = "-1"
            'e.Nombre = 
            'e.Naturaleza = 
            'e.DctoAsociado = 
            'e.actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDCuentaContable = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCuentasContablesOyPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCuentasContablesOy(ByVal CuentasContablesOy As CuentasContablesOy)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, CuentasContablesOy.pstrUsuarioConexion, CuentasContablesOy.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CuentasContablesOy.InfoSesion = DemeInfoSesion(CuentasContablesOy.pstrUsuarioConexion, "InsertCuentasContablesOy")
            Me.DataContext.CuentasContablesOyD.InsertOnSubmit(CuentasContablesOy)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCuentasContablesOy")
        End Try
    End Sub

    Public Sub UpdateCuentasContablesOy(ByVal currentCuentasContablesOy As CuentasContablesOy)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCuentasContablesOy.pstrUsuarioConexion, currentCuentasContablesOy.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCuentasContablesOy.InfoSesion = DemeInfoSesion(currentCuentasContablesOy.pstrUsuarioConexion, "UpdateCuentasContablesOy")
            Me.DataContext.CuentasContablesOyD.Attach(currentCuentasContablesOy, Me.ChangeSet.GetOriginal(currentCuentasContablesOy))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCuentasContablesOy")
        End Try
    End Sub

    Public Sub DeleteCuentasContablesOy(ByVal CuentasContablesOy As CuentasContablesOy)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CuentasContablesOy.pstrUsuarioConexion, CuentasContablesOy.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If CuentasContablesOy.IDCuentaContable = -1 Then
                Exit Sub
            End If
            'Me.DataContext.uspOyDNet_Maestros_CuentasContablesOyD_Eliminar( pID,  pNombre,  pNaturaleza,  pDctoAsociado, DemeInfoSesion(pstrUsuario, "DeleteCuentasContablesOy"),0).ToList
            CuentasContablesOy.InfoSesion = DemeInfoSesion(CuentasContablesOy.pstrUsuarioConexion, "DeleteCuentasContablesOy")
            Me.DataContext.CuentasContablesOyD.Attach(CuentasContablesOy)
            Me.DataContext.CuentasContablesOyD.DeleteOnSubmit(CuentasContablesOy)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCuentasContablesOy")
        End Try
    End Sub

    Public Function EliminarCuentasContables(ByVal pintIDCuentaContable As Integer, ByVal strMsg As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CuentasContables_Eliminar(pintIDCuentaContable, strMsg, DemeInfoSesion(pstrUsuario, "EliminarCuentasContables"), 0)
            Return strMsg
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarCuentasContables")
            Return Nothing
        End Try
    End Function

#End Region

#End Region

#Region "V1.0.2"


#Region "DiasNoHabiles"

    Public Function DiasNoHabilesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DiasNoHabile)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DiasNoHabiles_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "DiasNoHabilesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DiasNoHabilesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DiasNoHabilesConsultar(ByVal pNoHabil As DateTime?, ByVal pActivo As Boolean, ByVal plngIdPais As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DiasNoHabile)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DiasNoHabiles_Consultar(pNoHabil, pActivo, plngIdPais, DemeInfoSesion(pstrUsuario, "BuscarDiasNoHabiles"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarDiasNoHabiles")
            Return Nothing
        End Try
    End Function

    Public Function TraerDiasNoHabilePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DiasNoHabile
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DiasNoHabile
            e.IdPais = ValorPorDefectoDiasNoHabile(pstrUsuario, pstrInfoConexion)
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.NoHabil = DateTime.Now
            e.Activo = True
            'e.Activo = 
            'e.Inactivo = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IdPais = 0
            e.IDDiaNoHabil = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDiasNoHabilePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDiasNoHabile(ByVal DiasNoHabile As DiasNoHabile)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, DiasNoHabile.pstrUsuarioConexion, DiasNoHabile.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DiasNoHabile.InfoSesion = DemeInfoSesion(DiasNoHabile.pstrUsuarioConexion, "InsertDiasNoHabile")
            Me.DataContext.DiasNoHabiles.InsertOnSubmit(DiasNoHabile)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDiasNoHabile")
        End Try
    End Sub

    Public Sub UpdateDiasNoHabile(ByVal currentDiasNoHabile As DiasNoHabile)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentDiasNoHabile.pstrUsuarioConexion, currentDiasNoHabile.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDiasNoHabile.InfoSesion = DemeInfoSesion(currentDiasNoHabile.pstrUsuarioConexion, "UpdateDiasNoHabile")
            Me.DataContext.DiasNoHabiles.Attach(currentDiasNoHabile, Me.ChangeSet.GetOriginal(currentDiasNoHabile))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDiasNoHabile")
        End Try
    End Sub

    Public Sub DeleteDiasNoHabile(ByVal DiasNoHabile As DiasNoHabile)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DiasNoHabile.pstrUsuarioConexion, DiasNoHabile.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_DiasNoHabiles_Eliminar( pNoHabil,  pActivo, DemeInfoSesion(pstrUsuario, "DeleteDiasNoHabile"),0).ToList
            DiasNoHabile.InfoSesion = DemeInfoSesion(DiasNoHabile.pstrUsuarioConexion, "DeleteDiasNoHabile")
            Me.DataContext.DiasNoHabiles.Attach(DiasNoHabile)
            Me.DataContext.DiasNoHabiles.DeleteOnSubmit(DiasNoHabile)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDiasNoHabile")
        End Try
    End Sub

    Public Function ValorPorDefectoDiasNoHabile(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DiasNoHabiles_ValoresPorDefecto(DemeInfoSesion(pstrUsuario, "ValorPorDefectoDiasNoHabiles"), 0)
            Return CInt(ret)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValorPorDefectoDiasNoHabile")
            Return -1
        End Try
    End Function

#End Region

#Region "TipoPersonaPorDcto"

    Public Function TipoPersonaPorDctoFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoPersonaPorDct)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoPersonaPorDcto_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "TipoPersonaPorDctoFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoPersonaPorDctoFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerTipoPersonaPorDctPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TipoPersonaPorDct
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TipoPersonaPorDct
            e.ID = -1
            'e.TipoIdentificacion = 
            'e.IDTipoPersona = 
            'e.menored = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTipoPersonaPorDctPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTipoPersonaPorDct(ByVal TipoPersonaPorDct As TipoPersonaPorDct)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, TipoPersonaPorDct.pstrUsuarioConexion, TipoPersonaPorDct.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TipoPersonaPorDct.InfoSesion = DemeInfoSesion(TipoPersonaPorDct.pstrUsuarioConexion, "InsertTipoPersonaPorDct")
            Me.DataContext.TipoPersonaPorDcto.InsertOnSubmit(TipoPersonaPorDct)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTipoPersonaPorDct")
        End Try
    End Sub

    Public Sub UpdateTipoPersonaPorDct(ByVal currentTipoPersonaPorDct As TipoPersonaPorDct)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentTipoPersonaPorDct.pstrUsuarioConexion, currentTipoPersonaPorDct.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTipoPersonaPorDct.InfoSesion = DemeInfoSesion(currentTipoPersonaPorDct.pstrUsuarioConexion, "UpdateTipoPersonaPorDct")
            Me.DataContext.TipoPersonaPorDcto.Attach(currentTipoPersonaPorDct, Me.ChangeSet.GetOriginal(currentTipoPersonaPorDct))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTipoPersonaPorDct")
        End Try
    End Sub

    Public Sub DeleteTipoPersonaPorDct(ByVal TipoPersonaPorDct As TipoPersonaPorDct)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoPersonaPorDct.pstrUsuarioConexion, TipoPersonaPorDct.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_TipoPersonaPorDcto_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteTipoPersonaPorDct"),0).ToList
            TipoPersonaPorDct.InfoSesion = DemeInfoSesion(TipoPersonaPorDct.pstrUsuarioConexion, "DeleteTipoPersonaPorDct")
            Me.DataContext.TipoPersonaPorDcto.Attach(TipoPersonaPorDct)
            Me.DataContext.TipoPersonaPorDcto.DeleteOnSubmit(TipoPersonaPorDct)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTipoPersonaPorDct")
        End Try
    End Sub
#End Region
#Region "Mesas"

    Public Function MesasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Mesa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Mesas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "MesasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MesasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function MesasConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Mesa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Mesas_Consultar(pID, pNombre, DemeInfoSesion(pstrUsuario, "BuscarMesas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarMesas")
            Return Nothing
        End Try
    End Function

    Public Function TraerMesaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Mesa
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Mesa
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = -1
            'e.Nombre = 
            'e.Ccostos = 
            'e.CuentaContable = 
            'e.IdPoblacion = 1
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IdMesa = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerMesaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertMesa(ByVal Mesa As Mesa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Mesa.pstrUsuarioConexion, Mesa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Mesa.InfoSesion = DemeInfoSesion(Mesa.pstrUsuarioConexion, "InsertMesa")
            Me.DataContext.Mesas.InsertOnSubmit(Mesa)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertMesa")
        End Try
    End Sub

    Public Sub UpdateMesa(ByVal currentMesa As Mesa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentMesa.pstrUsuarioConexion, currentMesa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentMesa.InfoSesion = DemeInfoSesion(currentMesa.pstrUsuarioConexion, "UpdateMesa")
            Me.DataContext.Mesas.Attach(currentMesa, Me.ChangeSet.GetOriginal(currentMesa))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateMesa")
        End Try
    End Sub

    Public Sub DeleteMesa(ByVal Mesa As Mesa)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Mesa.pstrUsuarioConexion, Mesa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Mesas_Eliminar( pID,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteMesa"),0).ToList
            Mesa.InfoSesion = DemeInfoSesion(Mesa.pstrUsuarioConexion, "DeleteMesa")
            Me.DataContext.Mesas.Attach(Mesa)
            Me.DataContext.Mesas.DeleteOnSubmit(Mesa)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteMesa")
        End Try
    End Sub

    Public Function EliminarMesa(ByVal pID As Integer, ByVal strMsg As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Mesas_Eliminar(pID, strMsg, DemeInfoSesion(pstrUsuario, "EliminarPaises"), 0)
            Return strMsg
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarPaises")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Instalacion"

    Public Function InstalacionFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Instalacio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Instalacion_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "InstalacionFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InstalacionFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerInstalacioPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Instalacio
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Instalacio
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.IDBolsa = 
            'e.IdPoblacion = 
            'e.Linea1 = 
            'e.Enca11 = 
            'e.Enca21 = 
            'e.Enca31 = 
            'e.Enca41 = 
            'e.Enca12 = 
            'e.Enca22 = 
            'e.Enca32 = 
            'e.Enca42 = 
            'e.Observacion1 = 
            'e.Observacion2 = 
            'e.ClientesAutomatico = 
            'e.ClientesCedula = 
            'e.EncaFac = 
            'e.EncaFacBca = 
            'e.EncaTit = 
            'e.EncaEgr = 
            'e.EncaCaj = 
            'e.EncaNot = 
            'e.EncaExt = 
            'e.RCLineas = 
            'e.CELineas = 
            'e.NCLineas = 
            'e.TITLineas = 
            'e.FacLineas = 
            'e.FacBcaLineas = 
            'e.EXTLineas = 
            'e.Receptores = 
            'e.FechaOrden = 
            'e.Usuario = 
            'e.ValSobregiroCE = 
            'e.Resolucion = 
            'e.IvaComision = 
            'e.NombreCuenta = 
            'e.SerBolsaFijo = 
            'e.SerBolsaFijoAcciones = 
            'e.TopeSerBolsaFijoAcciones = 
            'e.EncaCus = 
            'e.CusLineas = 
            'e.UsuarioEntregas = 
            'e.UsuarioRecibido = 
            'e.UsuarioCustodia = 
            'e.UsuarioSobrantes = 
            'e.IVA = 
            'e.RteFuente = 
            'e.NitComisionista = 
            'e.Servidor = 
            'e.BaseDatos = 
            'e.Owner = 
            'e.ServidorBus = 
            'e.BaseDatosBus = 
            'e.OwnerBus = 
            'e.Compania = 
            'e.DepositoExtranjero = 
            'e.CustodioLocal = 
            'e.IdContraparteOTC = 
            'e.ValorContrato = 
            'e.CodigoIMC = 
            'e.ReteIva = 
            'e.ValorInicial = 
            'e.GMS = 
            'e.CargarReceptorCliente = 
            'e.Cierre = 
            'e.UltimaVersion = 
            'e.TasaInicial = 
            'e.AplazarOTC = 
            'e.CuentasBancarias = 
            'e.RepresentanteLegal = 
            'e.FechaLimite = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.DiaSemana = 
            'e.PorcentajeGarantia = 
            'e.TarifaRteFuente = 
            'e.ImpDocTesoreria = 
            'e.TipoEntidad = 
            'e.CodigoEntidad = 
            'e.RteComision = 
            'e.RteICA = 
            'e.CodigoEntidadUIAF = 
            'e.TipoEntidadUIAF = 
            'e.ValidaCuentaSuperVal = 
            'e.ValSobregiroNC = 
            'e.Tipo = 
            'e.CtaContable = 
            'e.CCosto = 
            'e.CtaContableContraparte = 
            'e.CCostoContraparte = 
            'e.ReporteExtractoClientePedirRangos = 
            'e.CtaContableClientes = 
            'e.URL = 
            'e.Path = 
            'e.Ordenantes = 
            'e.ReceptorSuc = 
            'e.ClientesAgrupados = 
            'e.PathActualiza = 
            'e.DatosFinancieros = 
            'e.ConceptoDetalleTesoreriaManual = 
            'e.NroUsu = 
            'e.ServidorNacional = 
            'e.Compania = 
            'e.CompaniaM = 
            'e.Multicuenta = 
            'e.MaximoValor = 
            'e.DefensorCliente = 
            'e.UrlReportesBus = 
            'e.RutaReportesBus = 
            'e.GMFInferior = 
            'e.CtaContableContraparteNotasCxC = 
            'e.tipoNotasCxC = 
            'e.IDInstalacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerInstalacioPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertInstalacio(ByVal Instalacio As Instalacio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Instalacio.pstrUsuarioConexion, Instalacio.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Instalacio.InfoSesion = DemeInfoSesion(Instalacio.pstrUsuarioConexion, "InsertInstalacio")
            Me.DataContext.Instalacion.InsertOnSubmit(Instalacio)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertInstalacio")
        End Try
    End Sub

    Public Sub UpdateInstalacio(ByVal currentInstalacio As Instalacio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentInstalacio.pstrUsuarioConexion, currentInstalacio.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentInstalacio.InfoSesion = DemeInfoSesion(currentInstalacio.pstrUsuarioConexion, "UpdateInstalacio")
            Me.DataContext.Instalacion.Attach(currentInstalacio, Me.ChangeSet.GetOriginal(currentInstalacio))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateInstalacio")
        End Try
    End Sub

    Public Sub DeleteInstalacio(ByVal Instalacio As Instalacio)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Instalacio.pstrUsuarioConexion, Instalacio.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Instalacion_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteInstalacio"),0).ToList
            Instalacio.InfoSesion = DemeInfoSesion(Instalacio.pstrUsuarioConexion, "DeleteInstalacio")
            Me.DataContext.Instalacion.Attach(Instalacio)
            Me.DataContext.Instalacion.DeleteOnSubmit(Instalacio)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteInstalacio")
        End Try
    End Sub
#End Region

#End Region

#End Region

#Region "JAQUELINE RESTREPO C. MAESTROS"

#Region "V1.0.0"

#Region "Custodio"

    Public Function CustodioFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Custodi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Custodio_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "CustodioFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CustodioFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CustodioConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Custodi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Custodio_Consultar(pID, pNombre, DemeInfoSesion(pstrUsuario, "BuscarCustodio"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCustodio")
            Return Nothing
        End Try
    End Function

    Public Function TraerCustodiPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Custodi
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Custodi
            e.ID = -1
            'e.Nombre = 
            'e.Local = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCustodiPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCustodi(ByVal Custodi As Custodi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Custodi.pstrUsuarioConexion, Custodi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Custodi.InfoSesion = DemeInfoSesion(Custodi.pstrUsuarioConexion, "InsertCustodi")
            Me.DataContext.Custodio.InsertOnSubmit(Custodi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCustodi")
        End Try
    End Sub

    Public Sub UpdateCustodi(ByVal currentCustodi As Custodi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCustodi.pstrUsuarioConexion, currentCustodi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCustodi.InfoSesion = DemeInfoSesion(currentCustodi.pstrUsuarioConexion, "UpdateCustodi")
            Me.DataContext.Custodio.Attach(currentCustodi, Me.ChangeSet.GetOriginal(currentCustodi))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCustodi")
        End Try
    End Sub

    Public Sub DeleteCustodi(ByVal Custodi As Custodi)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Custodi.pstrUsuarioConexion, Custodi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Custodio_Eliminar( pID,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteCustodi"),0).ToList
            Custodi.InfoSesion = DemeInfoSesion(Custodi.pstrUsuarioConexion, "DeleteCustodi")
            Me.DataContext.Custodio.Attach(Custodi)
            Me.DataContext.Custodio.DeleteOnSubmit(Custodi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCustodi")
        End Try
    End Sub
#End Region

#Region "Profesiones"

    Public Function ProfesionesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Profesione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Profesiones_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ProfesionesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ProfesionesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ProfesionesConsultar(ByVal pCodigoProfesion As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Profesione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Profesiones_Consultar(pCodigoProfesion, pNombre, DemeInfoSesion(pstrUsuario, "BuscarProfesiones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarProfesiones")
            Return Nothing
        End Try
    End Function

    Public Function TraerProfesionePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Profesione
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Profesione
            'e.IDComisionista = -1
            'e.IDSucComisionista = -1
            e.CodigoProfesion = -1
            'e.Nombre = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDProfesion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerProfesionePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertProfesione(ByVal Profesione As Profesione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Profesione.pstrUsuarioConexion, Profesione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Profesione.InfoSesion = DemeInfoSesion(Profesione.pstrUsuarioConexion, "InsertProfesione")
            Me.DataContext.Profesiones.InsertOnSubmit(Profesione)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertProfesione")
        End Try
    End Sub

    Public Sub UpdateProfesione(ByVal currentProfesione As Profesione)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentProfesione.pstrUsuarioConexion, currentProfesione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentProfesione.InfoSesion = DemeInfoSesion(currentProfesione.pstrUsuarioConexion, "UpdateProfesione")
            Me.DataContext.Profesiones.Attach(currentProfesione, Me.ChangeSet.GetOriginal(currentProfesione))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateProfesione")
        End Try
    End Sub

    Public Sub DeleteProfesione(ByVal Profesione As Profesione)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Profesione.pstrUsuarioConexion, Profesione.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Profesiones_Eliminar( pCodigoProfesion,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteProfesione"),0).ToList
            Profesione.InfoSesion = DemeInfoSesion(Profesione.pstrUsuarioConexion, "DeleteProfesione")
            Me.DataContext.Profesiones.Attach(Profesione)
            Me.DataContext.Profesiones.DeleteOnSubmit(Profesione)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteProfesione")
        End Try
    End Sub
#End Region

#Region "Codigos_CIIU"

    Public Function Codigos_CIIUFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Codigos_CII)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Codigos_CIIU_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "Codigos_CIIUFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Codigos_CIIUFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function Codigos_CIIUConsultar(ByVal pCodigo As String, ByVal pDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Codigos_CII)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Codigos_CIIU_Consultar(pCodigo, pDescripcion, DemeInfoSesion(pstrUsuario, "BuscarCodigos_CIIU"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCodigos_CIIU")
            Return Nothing
        End Try
    End Function

    Public Function TraerCodigos_CIIPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Codigos_CII
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Codigos_CII
            'e.Codigo = 
            'e.Descripcion = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.ClasificacionCIIU=
            'e.IDCodigoCIIU = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCodigos_CIIPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCodigos_CII(ByVal Codigos_CII As Codigos_CII)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Codigos_CII.pstrUsuarioConexion, Codigos_CII.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Codigos_CII.InfoSesion = DemeInfoSesion(Codigos_CII.pstrUsuarioConexion, "InsertCodigos_CII")
            Me.DataContext.Codigos_CIIU.InsertOnSubmit(Codigos_CII)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCodigos_CII")
        End Try
    End Sub

    Public Sub UpdateCodigos_CII(ByVal currentCodigos_CII As Codigos_CII)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCodigos_CII.pstrUsuarioConexion, currentCodigos_CII.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCodigos_CII.InfoSesion = DemeInfoSesion(currentCodigos_CII.pstrUsuarioConexion, "UpdateCodigos_CII")
            Me.DataContext.Codigos_CIIU.Attach(currentCodigos_CII, Me.ChangeSet.GetOriginal(currentCodigos_CII))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCodigos_CII")
        End Try
    End Sub

    Public Sub DeleteCodigos_CII(ByVal Codigos_CII As Codigos_CII)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Codigos_CII.pstrUsuarioConexion, Codigos_CII.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Codigos_CIIU_Eliminar( pCodigo,  pDescripcion, DemeInfoSesion(pstrUsuario, "DeleteCodigos_CII"),0).ToList
            Codigos_CII.InfoSesion = DemeInfoSesion(Codigos_CII.pstrUsuarioConexion, "DeleteCodigos_CII")
            Me.DataContext.Codigos_CIIU.Attach(Codigos_CII)
            Me.DataContext.Codigos_CIIU.DeleteOnSubmit(Codigos_CII)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCodigos_CII")
        End Try
    End Sub
#End Region

#Region "DoctosRequeridos"

    Public Function DoctosRequeridosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DoctosRequerido)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DoctosRequeridos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "DoctosRequeridosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DoctosRequeridosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DoctosRequeridosConsultar(ByVal pCodigoDocto As String, ByVal pNombreDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DoctosRequerido)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DoctosRequeridos_Consultar(pCodigoDocto, pNombreDocumento, DemeInfoSesion(pstrUsuario, "BuscarDoctosRequeridos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarDoctosRequeridos")
            Return Nothing
        End Try
    End Function

    Public Function TraerDoctosRequeridoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DoctosRequerido
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DoctosRequerido
            'e.IDDocumento = 
            'e.CodigoDocto = 
            'e.NombreDocumento = 
            'e.Requerido = 
            'e.FechaIniVigencia = 
            'e.FechaFinVigencia = 
            e.DocuActivo = True
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDoctosRequeridoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDoctosRequerido(ByVal DoctosRequerido As DoctosRequerido)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, DoctosRequerido.pstrUsuarioConexion, DoctosRequerido.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DoctosRequerido.InfoSesion = DemeInfoSesion(DoctosRequerido.pstrUsuarioConexion, "InsertDoctosRequerido")
            Me.DataContext.DoctosRequeridos.InsertOnSubmit(DoctosRequerido)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDoctosRequerido")
        End Try
    End Sub

    Public Sub UpdateDoctosRequerido(ByVal currentDoctosRequerido As DoctosRequerido)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentDoctosRequerido.pstrUsuarioConexion, currentDoctosRequerido.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDoctosRequerido.InfoSesion = DemeInfoSesion(currentDoctosRequerido.pstrUsuarioConexion, "UpdateDoctosRequerido")
            Me.DataContext.DoctosRequeridos.Attach(currentDoctosRequerido, Me.ChangeSet.GetOriginal(currentDoctosRequerido))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDoctosRequerido")
        End Try
    End Sub

    Public Sub DeleteDoctosRequerido(ByVal DoctosRequerido As DoctosRequerido)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DoctosRequerido.pstrUsuarioConexion, DoctosRequerido.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_DoctosRequeridos_Eliminar( pCodigoDocto,  pNombreDocumento, DemeInfoSesion(pstrUsuario, "DeleteDoctosRequerido"),0).ToList
            DoctosRequerido.InfoSesion = DemeInfoSesion(DoctosRequerido.pstrUsuarioConexion, "DeleteDoctosRequerido")
            Me.DataContext.DoctosRequeridos.Attach(DoctosRequerido)
            Me.DataContext.DoctosRequeridos.DeleteOnSubmit(DoctosRequerido)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDoctosRequerido")
        End Try
    End Sub

    ''' <summary>
    ''' Descripción:     Consulta los documentos requeridos del clientes
    ''' </summary> 
    ''' <param name="plngID">Codigo del cliente</param>
    ''' <param name="pstrTipoIdentificacion">Tipo de persona</param>
    ''' <returns>Lista de DoctosRequerido</returns>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 8 de Octubre 2013</remarks>
    Public Function DocumentosRecibidosConsultar(ByVal plngID As String, ByVal pstrTipoIdentificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DoctosRequerido)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spCargarDocumentosRequeridos_OyDNet(plngID, pstrTipoIdentificacion, DemeInfoSesion(pstrUsuario, "DocumentosRecibidosConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DocumentosRecibidosConsultar")
            Return Nothing
        End Try
    End Function




#End Region

#Region "TipoReferencias"

    Public Function TipoReferenciasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoReferencia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoReferencias_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "TipoReferenciasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TipoReferenciasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TipoReferenciasConsultar(ByVal pIDCodigoRetorno As String, ByVal pDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TipoReferencia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoReferencias_Consultar(pIDCodigoRetorno, pDescripcion, DemeInfoSesion(pstrUsuario, "BuscarTipoReferencias"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarTipoReferencias")
            Return Nothing
        End Try
    End Function

    Public Function TraerTipoReferenciaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TipoReferencia
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TipoReferencia
            'e.IDComisionista = -1
            'e.IDSucComisionista = -1
            e.IDCodigo = -1
            'e.IDCodigoRetorno = 
            'e.Descripcion = 
            'e.tipoClasificacion = 
            'e.Formulario1 = 
            'e.Formulario2 = 
            'e.Formulario3 = 
            'e.Formulario4 = 
            'e.Formulario5 = 
            'e.CalculaIVA = 
            'e.Mensajes = 
            'e.CalculaRetencion = 
            'e.CantidadNegociada = 
            'e.NroMesesDctoTransporte = 
            'e.Consecutivo = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTipoReferenciaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTipoReferencia(ByVal TipoReferencia As TipoReferencia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, TipoReferencia.pstrUsuarioConexion, TipoReferencia.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TipoReferencia.InfoSesion = DemeInfoSesion(TipoReferencia.pstrUsuarioConexion, "InsertTipoReferencia")
            Me.DataContext.TipoReferencias.InsertOnSubmit(TipoReferencia)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTipoReferencia")
        End Try
    End Sub

    Public Sub UpdateTipoReferencia(ByVal currentTipoReferencia As TipoReferencia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentTipoReferencia.pstrUsuarioConexion, currentTipoReferencia.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTipoReferencia.InfoSesion = DemeInfoSesion(currentTipoReferencia.pstrUsuarioConexion, "UpdateTipoReferencia")
            Me.DataContext.TipoReferencias.Attach(currentTipoReferencia, Me.ChangeSet.GetOriginal(currentTipoReferencia))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTipoReferencia")
        End Try
    End Sub

    Public Sub DeleteTipoReferencia(ByVal TipoReferencia As TipoReferencia)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TipoReferencia.pstrUsuarioConexion, TipoReferencia.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_TipoReferencias_Eliminar( pIDCodigoRetorno,  pDescripcion, DemeInfoSesion(pstrUsuario, "DeleteTipoReferencia"),0).ToList
            TipoReferencia.InfoSesion = DemeInfoSesion(TipoReferencia.pstrUsuarioConexion, "DeleteTipoReferencia")
            Me.DataContext.TipoReferencias.Attach(TipoReferencia)
            Me.DataContext.TipoReferencias.DeleteOnSubmit(TipoReferencia)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTipoReferencia")
        End Try
    End Sub

    Public Function EliminarTipoReferencias(ByVal pID As Integer, ByVal pUsuario As String, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TipoReferencias_Eliminar(pID, pUsuario, mensaje, DemeInfoSesion(pstrUsuario, "EliminarTipoReferencias"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarTipoReferencias")
            Return Nothing
        End Try
    End Function
#End Region

#End Region

#Region "V1.0.1"

#Region "ClientesExternos"

    Public Function ClientesExternosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesExterno)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesExternos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ClientesExternosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesExternosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ClientesExternosConsultar(ByVal pID As String, ByVal pNombre As String, ByVal pVendedor As String, ByVal pIDDepositoExtranjero As Integer, ByVal pNumeroCuenta As String, ByVal pNombreTitular As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesExterno)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesExternos_Consultar(pID, pNombre, pVendedor, pIDDepositoExtranjero, pNumeroCuenta, pNombreTitular, DemeInfoSesion(pstrUsuario, "BuscarClientesExternos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClientesExternos")
            Return Nothing
        End Try
    End Function

    Public Function TraerClientesExternoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ClientesExterno
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ClientesExterno
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = "              -1"
            'e.Nombre = 
            'e.Vendedor = 
            e.IDDepositoExtranjero = 0
            'e.NumeroCuenta = 
            'e.NombreTitular = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDClienteExt = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesExternoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertClientesExterno(ByVal ClientesExterno As ClientesExterno)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesExterno.pstrUsuarioConexion, ClientesExterno.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesExterno.InfoSesion = DemeInfoSesion(ClientesExterno.pstrUsuarioConexion, "InsertClientesExterno")
            Me.DataContext.ClientesExternos.InsertOnSubmit(ClientesExterno)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClientesExterno")
        End Try
    End Sub

    Public Sub UpdateClientesExterno(ByVal currentClientesExterno As ClientesExterno)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesExterno.pstrUsuarioConexion, currentClientesExterno.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClientesExterno.InfoSesion = DemeInfoSesion(currentClientesExterno.pstrUsuarioConexion, "UpdateClientesExterno")
            Me.DataContext.ClientesExternos.Attach(currentClientesExterno, Me.ChangeSet.GetOriginal(currentClientesExterno))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClientesExterno")
        End Try
    End Sub

    Public Sub DeleteClientesExterno(ByVal ClientesExterno As ClientesExterno)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesExterno.pstrUsuarioConexion, ClientesExterno.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ClientesExternos_Eliminar( pID,  pNombre,  pVendedor,  pIDDepositoExtranjero,  pNumeroCuenta,  pNombreTitular, DemeInfoSesion(pstrUsuario, "DeleteClientesExterno"),0).ToList
            ClientesExterno.InfoSesion = DemeInfoSesion(ClientesExterno.pstrUsuarioConexion, "DeleteClientesExterno")
            Me.DataContext.ClientesExternos.Attach(ClientesExterno)
            Me.DataContext.ClientesExternos.DeleteOnSubmit(ClientesExterno)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesExterno")
        End Try
    End Sub
#End Region
#Region "ConceptosTesoreria"

    Public Function ConceptosTesoreriaFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptosTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosTesoreria_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ConceptosTesoreriaFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosTesoreriaFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosTesoreriaConsultar(ByVal pIDConcepto As Integer, ByVal pDetalle As String, ByVal pAplicaA As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptosTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosTesoreria_Consultar(pIDConcepto, pDetalle, pAplicaA, DemeInfoSesion(pstrUsuario, "BuscarConceptosTesoreria"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarConceptosTesoreria")
            Return Nothing
        End Try
    End Function

    Public Function TraerConceptosTesoreriPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConceptosTesoreri
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ConceptosTesoreri
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IDConcepto = -1
            'e.Detalle = 
            'e.AplicaA = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.CuentaContable = 
            e.Activo = True
            'e.ParametroContable = 
            'e.CuentaContableAux = 
            'e.NitTercero = 
            'e.ManejaCliente =
            'e.TipoMovimientoTesoreria =
            'e.Retencion =
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerConceptosTesoreriPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConceptosTesoreri(ByVal ConceptosTesoreri As ConceptosTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ConceptosTesoreri.pstrUsuarioConexion, ConceptosTesoreri.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ConceptosTesoreri.InfoSesion = DemeInfoSesion(ConceptosTesoreri.pstrUsuarioConexion, "InsertConceptosTesoreri")
            Me.DataContext.ConceptosTesoreria.InsertOnSubmit(ConceptosTesoreri)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConceptosTesoreri")
        End Try
    End Sub

    Public Sub UpdateConceptosTesoreri(ByVal currentConceptosTesoreri As ConceptosTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentConceptosTesoreri.pstrUsuarioConexion, currentConceptosTesoreri.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConceptosTesoreri.InfoSesion = DemeInfoSesion(currentConceptosTesoreri.pstrUsuarioConexion, "UpdateConceptosTesoreri")
            Me.DataContext.ConceptosTesoreria.Attach(currentConceptosTesoreri, Me.ChangeSet.GetOriginal(currentConceptosTesoreri))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConceptosTesoreri")
        End Try
    End Sub

    Public Sub DeleteConceptosTesoreri(ByVal ConceptosTesoreri As ConceptosTesoreri)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConceptosTesoreri.pstrUsuarioConexion, ConceptosTesoreri.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConceptosTesoreria_Eliminar( pIDConcepto,  pDetalle,  pAplicaA, DemeInfoSesion(pstrUsuario, "DeleteConceptosTesoreri"),0).ToList
            ConceptosTesoreri.InfoSesion = DemeInfoSesion(ConceptosTesoreri.pstrUsuarioConexion, "DeleteConceptosTesoreri")
            Me.DataContext.ConceptosTesoreria.Attach(ConceptosTesoreri)
            Me.DataContext.ConceptosTesoreria.DeleteOnSubmit(ConceptosTesoreri)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConceptosTesoreri")
        End Try
    End Sub
#End Region
#Region "Consecutivos"

    Public Function ConsecutivosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Consecutivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Consecutivos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ConsecutivosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsecutivosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ConsecutivosConsultar(ByVal pIDOwner As String, ByVal pNombreConsecutivo As String, ByVal pDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Consecutivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Consecutivos_Consultar(pIDOwner, pNombreConsecutivo, pDescripcion, DemeInfoSesion(pstrUsuario, "BuscarConsecutivos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarConsecutivos")
            Return Nothing
        End Try
    End Function

    Public Function TraerConsecutivoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Consecutivo
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Consecutivo
            e.IDComisionista = -1
            e.IDSucComisionista = -1
            'e.IDOwner = 
            'e.NombreConsecutivo = 
            'e.Descripcion = 
            'e.Minimo = 
            'e.Maximo = 
            'e.Actual = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.CodContabilidad = 
            e.Activo = True
            'e.IdConsecutivos = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerConsecutivoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConsecutivo(ByVal Consecutivo As Consecutivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Consecutivo.pstrUsuarioConexion, Consecutivo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Consecutivo.InfoSesion = DemeInfoSesion(Consecutivo.pstrUsuarioConexion, "InsertConsecutivo")
            Me.DataContext.Consecutivos.InsertOnSubmit(Consecutivo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConsecutivo")
        End Try
    End Sub

    Public Sub UpdateConsecutivo(ByVal currentConsecutivo As Consecutivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentConsecutivo.pstrUsuarioConexion, currentConsecutivo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConsecutivo.InfoSesion = DemeInfoSesion(currentConsecutivo.pstrUsuarioConexion, "UpdateConsecutivo")
            Me.DataContext.Consecutivos.Attach(currentConsecutivo, Me.ChangeSet.GetOriginal(currentConsecutivo))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConsecutivo")
        End Try
    End Sub

    Public Sub DeleteConsecutivo(ByVal Consecutivo As Consecutivo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Consecutivo.pstrUsuarioConexion, Consecutivo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Consecutivos_Eliminar( pIDOwner,  pNombreConsecutivo,  pDescripcion, DemeInfoSesion(pstrUsuario, "DeleteConsecutivo"),0).ToList
            Consecutivo.InfoSesion = DemeInfoSesion(Consecutivo.pstrUsuarioConexion, "DeleteConsecutivo")
            Me.DataContext.Consecutivos.Attach(Consecutivo)
            Me.DataContext.Consecutivos.DeleteOnSubmit(Consecutivo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConsecutivo")
        End Try
    End Sub
#End Region

#Region "DepositosExtranjeros"

    Public Function DepositosExtranjerosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DepositosExtranjero)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DepositosExtranjeros_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "DepositosExtranjerosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DepositosExtranjerosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DepositosExtranjerosConsultar(ByVal pIDdeposito As Integer, ByVal pNombre As String, ByVal pIDPais As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DepositosExtranjero)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DepositosExtranjeros_Consultar(pIDdeposito, pNombre, pIDPais, DemeInfoSesion(pstrUsuario, "BuscarDepositosExtranjeros"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarDepositosExtranjeros")
            Return Nothing
        End Try
    End Function

    Public Function TraerDepositosExtranjeroPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DepositosExtranjero
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DepositosExtranjero
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IDdeposito = -1
            'e.Nombre = 
            'e.IDPais = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDepositosExtranjeroPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDepositosExtranjero(ByVal DepositosExtranjero As DepositosExtranjero)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, DepositosExtranjero.pstrUsuarioConexion, DepositosExtranjero.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DepositosExtranjero.InfoSesion = DemeInfoSesion(DepositosExtranjero.pstrUsuarioConexion, "InsertDepositosExtranjero")
            Me.DataContext.DepositosExtranjeros.InsertOnSubmit(DepositosExtranjero)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDepositosExtranjero")
        End Try
    End Sub

    Public Sub UpdateDepositosExtranjero(ByVal currentDepositosExtranjero As DepositosExtranjero)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentDepositosExtranjero.pstrUsuarioConexion, currentDepositosExtranjero.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDepositosExtranjero.InfoSesion = DemeInfoSesion(currentDepositosExtranjero.pstrUsuarioConexion, "UpdateDepositosExtranjero")
            Me.DataContext.DepositosExtranjeros.Attach(currentDepositosExtranjero, Me.ChangeSet.GetOriginal(currentDepositosExtranjero))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDepositosExtranjero")
        End Try
    End Sub

    Public Sub DeleteDepositosExtranjero(ByVal DepositosExtranjero As DepositosExtranjero)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DepositosExtranjero.pstrUsuarioConexion, DepositosExtranjero.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_DepositosExtranjeros_Eliminar( pIDdeposito,  pNombre,  pIDPais, DemeInfoSesion(pstrUsuario, "DeleteDepositosExtranjero"),0).ToList
            DepositosExtranjero.InfoSesion = DemeInfoSesion(DepositosExtranjero.pstrUsuarioConexion, "DeleteDepositosExtranjero")
            Me.DataContext.DepositosExtranjeros.Attach(DepositosExtranjero)
            Me.DataContext.DepositosExtranjeros.DeleteOnSubmit(DepositosExtranjero)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDepositosExtranjero")
        End Try
    End Sub

    Public Function EliminarDepositoExtranjero(ByVal pID As Integer, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DepositosExtranjeros_Eliminar(pID, mensaje, DemeInfoSesion(pstrUsuario, "EliminarDepositoExtranjero"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString, "EliminarDepositoExtranjero")
            Return Nothing
        End Try
    End Function
#End Region
#Region "PrefijosFacturas"

    Public Function PrefijosFacturasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PrefijosFactura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_PrefijosFacturas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "PrefijosFacturasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PrefijosFacturasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function PrefijosFacturasConsultar(ByVal pPrefijo As String, ByVal pDescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PrefijosFactura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_PrefijosFacturas_Consultar(pPrefijo, pDescripcion, DemeInfoSesion(pstrUsuario, "BuscarPrefijosFacturas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarPrefijosFacturas")
            Return Nothing
        End Try
    End Function

    Public Function TraerPrefijosFacturaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PrefijosFactura
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New PrefijosFactura
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Prefijo = 
            'e.NombreConsecutivo = 
            'e.Descripcion = 
            'e.Tipo = 
            'e.NombreCuenta =                                                                          
            'e.TextoResolucion = 
            'e.IntervaloRes = 
            'e.ResponsabilidadIVA = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.FechaVencimiento = Now
            e.Alarma = False
            e.IDCodigoResolucion = -1
            'e.IDPrefijoFacturas = 
            e.SucursalAplica = 1
            'e.Resolucion = 
            'e.AnoRes = 
            'CFMA20172510
            e.FechaDesde = Now
            e.FechaHasta = Now
            'CFMA20172510
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerPrefijosFacturaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertPrefijosFactura(ByVal PrefijosFactura As PrefijosFactura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, PrefijosFactura.pstrUsuarioConexion, PrefijosFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            PrefijosFactura.InfoSesion = DemeInfoSesion(PrefijosFactura.pstrUsuarioConexion, "InsertPrefijosFactura")
            Me.DataContext.PrefijosFacturas.InsertOnSubmit(PrefijosFactura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPrefijosFactura")
        End Try
    End Sub

    Public Sub UpdatePrefijosFactura(ByVal currentPrefijosFactura As PrefijosFactura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentPrefijosFactura.pstrUsuarioConexion, currentPrefijosFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentPrefijosFactura.InfoSesion = DemeInfoSesion(currentPrefijosFactura.pstrUsuarioConexion, "UpdatePrefijosFactura")
            Me.DataContext.PrefijosFacturas.Attach(currentPrefijosFactura, Me.ChangeSet.GetOriginal(currentPrefijosFactura))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePrefijosFactura")
        End Try
    End Sub

    Public Sub DeletePrefijosFactura(ByVal PrefijosFactura As PrefijosFactura)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,PrefijosFactura.pstrUsuarioConexion, PrefijosFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_PrefijosFacturas_Eliminar( pPrefijo,  pDescripcion, DemeInfoSesion(pstrUsuario, "DeletePrefijosFactura"),0).ToList
            PrefijosFactura.InfoSesion = DemeInfoSesion(PrefijosFactura.pstrUsuarioConexion, "DeletePrefijosFactura")
            Me.DataContext.PrefijosFacturas.Attach(PrefijosFactura)
            Me.DataContext.PrefijosFacturas.DeleteOnSubmit(PrefijosFactura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePrefijosFactura")
        End Try
    End Sub
#End Region
#Region "Empleados"

    Public Function EmpleadosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Empleado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Empleados_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "EmpleadosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EmpleadosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function EmpleadosConsultar(ByVal pIDEmpleado As Integer, ByVal pNombre As String, ByVal pNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Empleado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Empleados_Consultar(pIDEmpleado, pNombre, pNroDocumento, DemeInfoSesion(pstrUsuario, "BuscarEmpleados"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEmpleados")
            Return Nothing
        End Try
    End Function

    Public Function TraerEmpleadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Empleado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Empleado
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IDEmpleado = -1
            'e.Nombre = 
            'e.NroDocumento = 
            'e.IDReceptor = 
            'e.Login = 
            'e.IDCargo = 
            'e.AccesoOperadorBolsa = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.Activo = True
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEmpleadoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEmpleado(ByVal Empleado As Empleado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Empleado.pstrUsuarioConexion, Empleado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Empleado.InfoSesion = DemeInfoSesion(Empleado.pstrUsuarioConexion, "InsertEmpleado")
            Me.DataContext.Empleados.InsertOnSubmit(Empleado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEmpleado")
        End Try
    End Sub

    Public Sub UpdateEmpleado(ByVal currentEmpleado As Empleado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentEmpleado.pstrUsuarioConexion, currentEmpleado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEmpleado.InfoSesion = DemeInfoSesion(currentEmpleado.pstrUsuarioConexion, "UpdateEmpleado")
            Me.DataContext.Empleados.Attach(currentEmpleado, Me.ChangeSet.GetOriginal(currentEmpleado))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEmpleado")
        End Try
    End Sub

    Public Sub DeleteEmpleado(ByVal Empleado As Empleado)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Empleado.pstrUsuarioConexion, Empleado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Empleados_Eliminar( pIDEmpleado,  pNombre,  pNroDocumento, DemeInfoSesion(pstrUsuario, "DeleteEmpleado"),0).ToList
            Empleado.InfoSesion = DemeInfoSesion(Empleado.pstrUsuarioConexion, "DeleteEmpleado")
            Me.DataContext.Empleados.Attach(Empleado)
            Me.DataContext.Empleados.DeleteOnSubmit(Empleado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEmpleado")
        End Try
    End Sub

    Public Function CambiarEstadoEmpleado(ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Maestros_Empleados_Eliminar(plngID, DemeInfoSesion(pstrUsuario, "CambiarEstadoEmpleado"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CambiarEstadoEmpleado")
            Return False
        End Try
    End Function

#End Region
#Region "Inhabilitados"

    Public Function InhabilitadosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Inhabilitado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Inhabilitados_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "InhabilitadosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InhabilitadosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function InhabilitadosConsultar(ByVal pnrodocumento As String, ByVal pnombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Inhabilitado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Inhabilitados_Consultar(pnrodocumento, pnombre, DemeInfoSesion(pstrUsuario, "BuscarInhabilitados"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarInhabilitados")
            Return Nothing
        End Try
    End Function

    Public Function TraerInhabilitadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Inhabilitado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Inhabilitado
            'e.idcomisionista = 
            'e.idsuccomisionista = 
            'e.tipoidentificacion = 
            'e.nrodocumento = 
            'e.nombre = 
            'e.idconcepto = 
            e.ingreso = Now
            'e.actualizacion = 
            e.usuario = HttpContext.Current.User.Identity.Name
            'e.IDInhabilitado = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerInhabilitadoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertInhabilitado(ByVal Inhabilitado As Inhabilitado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Inhabilitado.pstrUsuarioConexion, Inhabilitado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Inhabilitado.InfoSesion = DemeInfoSesion(Inhabilitado.pstrUsuarioConexion, "InsertInhabilitado")
            Me.DataContext.Inhabilitados.InsertOnSubmit(Inhabilitado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertInhabilitado")
        End Try
    End Sub

    Public Sub UpdateInhabilitado(ByVal currentInhabilitado As Inhabilitado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentInhabilitado.pstrUsuarioConexion, currentInhabilitado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentInhabilitado.InfoSesion = DemeInfoSesion(currentInhabilitado.pstrUsuarioConexion, "UpdateInhabilitado")
            Me.DataContext.Inhabilitados.Attach(currentInhabilitado, Me.ChangeSet.GetOriginal(currentInhabilitado))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateInhabilitado")
        End Try
    End Sub

    Public Sub DeleteInhabilitado(ByVal Inhabilitado As Inhabilitado)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Inhabilitado.pstrUsuarioConexion, Inhabilitado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Inhabilitados_Eliminar( pnrodocumento,  pnombre, DemeInfoSesion(pstrUsuario, "DeleteInhabilitado"),0).ToList
            Inhabilitado.InfoSesion = DemeInfoSesion(Inhabilitado.pstrUsuarioConexion, "DeleteInhabilitado")
            Me.DataContext.Inhabilitados.Attach(Inhabilitado)
            Me.DataContext.Inhabilitados.DeleteOnSubmit(Inhabilitado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteInhabilitado")
        End Try
    End Sub
#End Region

#End Region

#End Region

#Region "JHON BAYRON TORRES MAESTROS"

#Region "V1.0.1"

#Region "UsuariosSucursales"

    Public Function UsuariosSucursalesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of UsuariosSucursale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_UsuariosSucursales_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "UsuariosSucursalesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UsuariosSucursalesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function UsuariosSucursalesConsultar(ByVal pUsuario As String, ByVal pReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of UsuariosSucursale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_UsuariosSucursales_Consultar(pUsuario, pReceptor, DemeInfoSesion(pstrUsuario, "BuscarUsuariosSucursales"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarUsuariosSucursales")
            Return Nothing
        End Try
    End Function

    Public Function TraerUsuariosSucursalePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As UsuariosSucursale
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New UsuariosSucursale
            'e.IDComisionista = 
            'e.IDSucComisionista 
            'e.Nombre_Usuario = 
            'e.Receptor = 
            'e.IDSucursal = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDUsuariosSucursales = -1
            e.Prioridad = 0
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerUsuariosSucursalePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertUsuariosSucursale(ByVal UsuariosSucursale As UsuariosSucursale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, UsuariosSucursale.pstrUsuarioConexion, UsuariosSucursale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            UsuariosSucursale.InfoSesion = DemeInfoSesion(UsuariosSucursale.pstrUsuarioConexion, "InsertUsuariosSucursale")
            Me.DataContext.UsuariosSucursales.InsertOnSubmit(UsuariosSucursale)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertUsuariosSucursale")
        End Try
    End Sub

    Public Sub UpdateUsuariosSucursale(ByVal currentUsuariosSucursale As UsuariosSucursale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentUsuariosSucursale.pstrUsuarioConexion, currentUsuariosSucursale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentUsuariosSucursale.InfoSesion = DemeInfoSesion(currentUsuariosSucursale.pstrUsuarioConexion, "UpdateUsuariosSucursale")
            Me.DataContext.UsuariosSucursales.Attach(currentUsuariosSucursale, Me.ChangeSet.GetOriginal(currentUsuariosSucursale))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateUsuariosSucursale")
        End Try
    End Sub

    Public Sub DeleteUsuariosSucursale(ByVal UsuariosSucursale As UsuariosSucursale)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,UsuariosSucursale.pstrUsuarioConexion, UsuariosSucursale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_UsuariosSucursales_Eliminar( pUsuario,  pReceptor, DemeInfoSesion(pstrUsuario, "DeleteUsuariosSucursale"),0).ToList
            UsuariosSucursale.InfoSesion = DemeInfoSesion(UsuariosSucursale.pstrUsuarioConexion, "DeleteUsuariosSucursale")
            Me.DataContext.UsuariosSucursales.Attach(UsuariosSucursale)
            Me.DataContext.UsuariosSucursales.DeleteOnSubmit(UsuariosSucursale)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteUsuariosSucursale")
        End Try
    End Sub
#End Region
#Region "ClientesFondosPensiones"


    Public Function ViewClientes_ExentoConsultar(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ViewClientes_Exento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesFondosPensiones_Consultar(pComitente, DemeInfoSesion(pstrUsuario, "BuscarViewClientes_Exentos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarViewClientes_Exentos")
            Return Nothing
        End Try
    End Function

    Public Function TraerClientesFondosPensionePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ClientesFondosPensione
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ClientesFondosPensione
            'e.Comitente = 
            'e.IDClientesfondospensiones = 
            'e.NroDocumento = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesFondosPensionePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertViewClientes_Exento(ByVal ViewClientes_Exento As ViewClientes_Exento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ViewClientes_Exento.pstrUsuarioConexion, ViewClientes_Exento.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ViewClientes_Exento.InfoSesion = DemeInfoSesion(ViewClientes_Exento.pstrUsuarioConexion, "InsertViewClientes_Exento")
            Me.DataContext.ViewClientes_Exentos.InsertOnSubmit(ViewClientes_Exento)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertViewClientes_Exento")
        End Try
    End Sub


    Public Sub UpdateViewClientes_Exento(ByVal currentViewClientes_Exento As ViewClientes_Exento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentViewClientes_Exento.pstrUsuarioConexion, currentViewClientes_Exento.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentViewClientes_Exento.InfoSesion = DemeInfoSesion(currentViewClientes_Exento.pstrUsuarioConexion, "UpdateViewClientes_Exento")
            Me.DataContext.ViewClientes_Exentos.Attach(currentViewClientes_Exento, Me.ChangeSet.GetOriginal(currentViewClientes_Exento))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateViewClientes_Exento")
        End Try
    End Sub


    Public Sub DeleteViewClientes_Exento(ByVal ViewClientes_Exento As ViewClientes_Exento)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ViewClientes_Exento.pstrUsuarioConexion, ViewClientes_Exento.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ClientesFondosPensiones_Eliminar( pComitente, DemeInfoSesion(pstrUsuario, "DeleteClientesFondosPensione"),0).ToList
            ViewClientes_Exento.InfoSesion = DemeInfoSesion(ViewClientes_Exento.pstrUsuarioConexion, "DeleteClientesFondosPensione")
            Me.DataContext.ViewClientes_Exentos.Attach(ViewClientes_Exento)
            Me.DataContext.ViewClientes_Exentos.DeleteOnSubmit(ViewClientes_Exento)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteViewClientes_Exento")
        End Try
    End Sub
    Public Function Clientes_ExentosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ViewClientes_Exento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim reto = Me.DataContext.uspOyDNet_Maestros_ClientesFondosPensiones_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "Clientes_ExentosFiltrar"), 0).ToList
            Return reto
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Clientes_ExentosFiltrar")
            Return Nothing
        End Try
    End Function
    Public Function TraerClientes_ExentoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ViewClientes_Exento
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ViewClientes_Exento
            'e.Comitente = 
            'e.IDClientesfondospensiones = 
            'e.NroDocumento = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientes_ExentoPorDefecto")
            Return Nothing
        End Try
    End Function

#Region "ViewClientes_Exento_Consultar Builder"

    Public Function GetViewClientes_Exentos_Consultars(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ViewClientes_Exentos_Consultar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesFondosPensiones_Consultar_Builder(pComitente, DemeInfoSesion(pstrUsuario, "BuscarViewClientes_Exentos_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarViewClientes_Exentos_Consultar")
            Return Nothing
        End Try
    End Function


#End Region
#End Region



#End Region
#Region "V1.0.2"

#Region "ConsecutivosUsuarios"

    Public Function ConsecutivosUsuariosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsecutivosUsuario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConsecutivosUsuarios_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ConsecutivosUsuariosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsecutivosUsuariosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ConsecutivosUsuariosConsultar(ByVal pUsuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsecutivosUsuario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConsecutivosUsuarios_Consultar(pUsuario, DemeInfoSesion(pstrUsuario, "BuscarConsecutivosUsuarios"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarConsecutivosUsuarios")
            Return Nothing
        End Try
    End Function

    Public Function TraerConsecutivosUsuarioPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConsecutivosUsuario
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ConsecutivosUsuario
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Usuario_Consecutivo = 
            'e.Nombre_Consecutivo = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDConsecutivosUsuarios = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerConsecutivosUsuarioPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConsecutivosUsuario(ByVal ConsecutivosUsuario As ConsecutivosUsuario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ConsecutivosUsuario.pstrUsuarioConexion, ConsecutivosUsuario.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ConsecutivosUsuario.InfoSesion = DemeInfoSesion(ConsecutivosUsuario.pstrUsuarioConexion, "InsertConsecutivosUsuario")
            Me.DataContext.ConsecutivosUsuarios.InsertOnSubmit(ConsecutivosUsuario)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConsecutivosUsuario")
        End Try
    End Sub

    Public Sub UpdateConsecutivosUsuario(ByVal currentConsecutivosUsuario As ConsecutivosUsuario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentConsecutivosUsuario.pstrUsuarioConexion, currentConsecutivosUsuario.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConsecutivosUsuario.InfoSesion = DemeInfoSesion(currentConsecutivosUsuario.pstrUsuarioConexion, "UpdateConsecutivosUsuario")
            Me.DataContext.ConsecutivosUsuarios.Attach(currentConsecutivosUsuario, Me.ChangeSet.GetOriginal(currentConsecutivosUsuario))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConsecutivosUsuario")
        End Try
    End Sub

    Public Sub DeleteConsecutivosUsuario(ByVal ConsecutivosUsuario As ConsecutivosUsuario)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConsecutivosUsuario.pstrUsuarioConexion, ConsecutivosUsuario.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConsecutivosUsuarios_Eliminar( pUsuario, DemeInfoSesion(pstrUsuario, "DeleteConsecutivosUsuario"),0).ToList
            ConsecutivosUsuario.InfoSesion = DemeInfoSesion(ConsecutivosUsuario.pstrUsuarioConexion, "DeleteConsecutivosUsuario")
            Me.DataContext.ConsecutivosUsuarios.Attach(ConsecutivosUsuario)
            Me.DataContext.ConsecutivosUsuarios.DeleteOnSubmit(ConsecutivosUsuario)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConsecutivosUsuario")
        End Try
    End Sub
    Public Function llenarconsecudisponibles(ByVal pstrconsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaUsuario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspOyDNet_Maestros_ConsecutivosUsuarios_Disponiblesconsecutivo(pstrconsecutivo, DemeInfoSesion(pstrUsuario, "Buscarllenarconsecudisponibles"), 0).ToList
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Buscarllenarconsecudisponibles")
            Return Nothing
        End Try
    End Function

    Public Function llenarusuariosinpermiso(ByVal pstrusuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of ListaUsuario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspOyDNet_Maestros_ConsecutivosUsuarios_DisponiblesUsuarios(pstrusuario, DemeInfoSesion(pstrUsuario, "Buscarllenarusuariosinpermiso"), 0).ToList
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Buscarllenarusuariosinpermiso")
            Return Nothing
        End Try
    End Function
#End Region
#Region "ConceptosConsecutivos"

    Public Function ConceptosConsecutivosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptosConsecutivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosConsecutivos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ConceptosConsecutivosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosConsecutivosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosConsecutivosConsultar(ByVal pConsecutivo As String, ByVal pDetalleConcepto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptosConsecutivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosConsecutivos_Consultar(pConsecutivo, pDetalleConcepto, DemeInfoSesion(pstrUsuario, "BuscarConceptosConsecutivos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarConceptosConsecutivos")
            Return Nothing
        End Try
    End Function

    Public Function TraerConceptosConsecutivoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConceptosConsecutivo
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ConceptosConsecutivo
            'e.IDComisionista = 
            'e.IdSucComisionista = 
            'e.Consecutivo = 
            'e.Concepto = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDConceptosconsecutivos = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerConceptosConsecutivoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConceptosConsecutivo(ByVal ConceptosConsecutivo As ConceptosConsecutivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ConceptosConsecutivo.pstrUsuarioConexion, ConceptosConsecutivo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ConceptosConsecutivo.InfoSesion = DemeInfoSesion(ConceptosConsecutivo.pstrUsuarioConexion, "InsertConceptosConsecutivo")
            Me.DataContext.ConceptosConsecutivos.InsertOnSubmit(ConceptosConsecutivo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConceptosConsecutivo")
        End Try
    End Sub

    Public Sub UpdateConceptosConsecutivo(ByVal currentConceptosConsecutivo As ConceptosConsecutivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentConceptosConsecutivo.pstrUsuarioConexion, currentConceptosConsecutivo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConceptosConsecutivo.InfoSesion = DemeInfoSesion(currentConceptosConsecutivo.pstrUsuarioConexion, "UpdateConceptosConsecutivo")
            Me.DataContext.ConceptosConsecutivos.Attach(currentConceptosConsecutivo, Me.ChangeSet.GetOriginal(currentConceptosConsecutivo))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConceptosConsecutivo")
        End Try
    End Sub

    Public Sub DeleteConceptosConsecutivo(ByVal ConceptosConsecutivo As ConceptosConsecutivo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConceptosConsecutivo.pstrUsuarioConexion, ConceptosConsecutivo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConceptosConsecutivos_Eliminar(pConsecutivo, pConcepto, DemeInfoSesion(pstrUsuario, "DeleteConceptosConsecutivo"), 0).ToList()
            ConceptosConsecutivo.InfoSesion = DemeInfoSesion(ConceptosConsecutivo.pstrUsuarioConexion, "DeleteConceptosConsecutivo")
            Me.DataContext.ConceptosConsecutivos.Attach(ConceptosConsecutivo)
            Me.DataContext.ConceptosConsecutivos.DeleteOnSubmit(ConceptosConsecutivo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConceptosConsecutivo")
        End Try
    End Sub
    Public Function llenarlistconsecutivodisponibles(ByVal pstrConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LlenarlistaConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspOyDNet_Maestros_ConceptosConsecutivos_Disponiblesconsecutivo(pstrConsecutivo, DemeInfoSesion(pstrUsuario, "Buscarllenarlistconsecutivodisponibles"), 0).ToList
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Buscarllenarlistconsecutivodisponibles")
            Return Nothing
        End Try
    End Function
    Public Function llenarlistconceptodisponibles(ByVal intlngIDconcepto As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LlenarlistaConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspOyDNet_Maestros_ConceptosConsecutivos_Disponiblesconcepto(intlngIDconcepto, DemeInfoSesion(pstrUsuario, "Buscarllenarlistconceptodisponibles"), 0).ToList
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Buscarllenarlistconceptodisponibles")
            Return Nothing
        End Try
    End Function




#End Region
#Region "Tarifas"

    Public Function TarifasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Tarifa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Tarifas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "TarifasFiltrar"), 0).ToList
            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TarifasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TarifasConsultar(ByVal filtro As Byte, ByVal pID As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Tarifa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Tarifas_Consultar(filtro, pID, RetornarValorDescodificado(pNombre), DemeInfoSesion(pstrUsuario, "BuscarTarifas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarTarifas")
            Return Nothing
        End Try
    End Function

    Public Function TraerTarifaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Tarifa
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Tarifa
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = -1
            'e.Nombre = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            'e.Valor = 
            'e.Simbolo = 
            ' e.IDTarifas 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTarifaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTarifa(ByVal Tarifa As Tarifa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Tarifa.pstrUsuarioConexion, Tarifa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Tarifa.InfoSesion = DemeInfoSesion(Tarifa.pstrUsuarioConexion, "InsertTarifa")
            Me.DataContext.Tarifas.InsertOnSubmit(Tarifa)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTarifa")
        End Try
    End Sub

    Public Sub UpdateTarifa(ByVal currentTarifa As Tarifa)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentTarifa.pstrUsuarioConexion, currentTarifa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentTarifa.Estado) Then
                If currentTarifa.Estado.Equals("Retiro") Then
                    currentTarifa.InfoSesion = DemeInfoSesion(currentTarifa.pstrUsuarioConexion, "UpdateTarifa")
                    Me.DataContext.Tarifas.Attach(currentTarifa)
                    Me.DataContext.Tarifas.DeleteOnSubmit(currentTarifa)
                Else
                    currentTarifa.InfoSesion = DemeInfoSesion(currentTarifa.pstrUsuarioConexion, "UpdateTarifa")
                    Me.DataContext.Tarifas.Attach(currentTarifa, Me.ChangeSet.GetOriginal(currentTarifa))
                End If
            Else
                currentTarifa.InfoSesion = DemeInfoSesion(currentTarifa.pstrUsuarioConexion, "UpdateTarifa")
                Me.DataContext.Tarifas.Attach(currentTarifa, Me.ChangeSet.GetOriginal(currentTarifa))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTarifa")
        End Try
    End Sub

    Public Sub DeleteTarifa(ByVal Tarifa As Tarifa)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Tarifa.pstrUsuarioConexion, Tarifa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Tarifas_Eliminar( pID,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteTarifa"),0).ToList
            Tarifa.InfoSesion = DemeInfoSesion(Tarifa.pstrUsuarioConexion, "DeleteTarifa")
            Me.DataContext.Tarifas.Attach(Tarifa)
            Me.DataContext.Tarifas.DeleteOnSubmit(Tarifa)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTarifa")
        End Try
    End Sub

    Public Function EliminarTarifas(ByVal Aprobacion As Byte, ByVal pNombre As String, ByVal pUsuario As String, ByVal pID As Integer, ByVal mensaje As String, ByVal pIDTarifas As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Tarifas_Eliminar(Aprobacion, pNombre, pUsuario, pID, mensaje, pIDTarifas, DemeInfoSesion(pstrUsuario, "EliminarPaises"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarPaises")
            Return Nothing
        End Try
    End Function

    Public Function Traer_DetalleTarifas_Tarifa(ByVal Filtro As Integer, ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleTarifa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pID) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_DetalleTarifas_Tarifa_Consultar(Filtro, pID, DemeInfoSesion(pstrUsuario, "Traer_DetalleTarifas_Tarifa"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_DetalleTarifas_Tarifa")
        End Try
        Return Nothing
    End Function
#End Region
#Region "DetalleTarifas"

    Public Function DetalleTarifasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleTarifa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DetalleTarifas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "DetalleTarifasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleTarifasFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerDetalleTarifaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DetalleTarifa
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DetalleTarifa
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Codigo 
            'e.FechaValor = Now
            'e.Valor = 
            'e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            e.IDDetalleTarifas = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDetalleTarifaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDetalleTarifa(ByVal DetalleTarifa As DetalleTarifa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, DetalleTarifa.pstrUsuarioConexion, DetalleTarifa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DetalleTarifa.InfoSesion = DemeInfoSesion(DetalleTarifa.pstrUsuarioConexion, "InsertDetalleTarifa")
            Me.DataContext.DetalleTarifas.InsertOnSubmit(DetalleTarifa)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDetalleTarifa")
        End Try
    End Sub

    Public Sub UpdateDetalleTarifa(ByVal currentDetalleTarifa As DetalleTarifa)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentDetalleTarifa.pstrUsuarioConexion, currentDetalleTarifa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentDetalleTarifa.Estado) Then
                If currentDetalleTarifa.Estado.Equals("Retiro") Then
                    currentDetalleTarifa.InfoSesion = DemeInfoSesion(currentDetalleTarifa.pstrUsuarioConexion, "UpdateDetalleTarifa")
                    Me.DataContext.DetalleTarifas.Attach(currentDetalleTarifa)
                    Me.DataContext.DetalleTarifas.DeleteOnSubmit(currentDetalleTarifa)
                Else
                    currentDetalleTarifa.InfoSesion = DemeInfoSesion(currentDetalleTarifa.pstrUsuarioConexion, "UpdateDetalleTarifa")
                    Me.DataContext.DetalleTarifas.Attach(currentDetalleTarifa, Me.ChangeSet.GetOriginal(currentDetalleTarifa))
                End If
            Else
                currentDetalleTarifa.InfoSesion = DemeInfoSesion(currentDetalleTarifa.pstrUsuarioConexion, "UpdateDetalleTarifa")
                Me.DataContext.DetalleTarifas.Attach(currentDetalleTarifa, Me.ChangeSet.GetOriginal(currentDetalleTarifa))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDetalleTarifa")
        End Try
    End Sub

    Public Sub DeleteDetalleTarifa(ByVal DetalleTarifa As DetalleTarifa)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DetalleTarifa.pstrUsuarioConexion, DetalleTarifa.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_DetalleTarifas_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteDetalleTarifa"),0).ToList
            DetalleTarifa.InfoSesion = DemeInfoSesion(DetalleTarifa.pstrUsuarioConexion, "DeleteDetalleTarifa")
            Me.DataContext.DetalleTarifas.Attach(DetalleTarifa)
            Me.DataContext.DetalleTarifas.DeleteOnSubmit(DetalleTarifa)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDetalleTarifa")
        End Try
    End Sub
#End Region
#Region "Monedas"

    Public Function MonedasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Moneda)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Monedas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "MonedasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MonedasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function MonedasConsultar(ByVal pCodigo As Integer, ByVal strCodigo As String, ByVal strdescripcion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Moneda)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Monedas_Consultar(pCodigo, strCodigo, strdescripcion, DemeInfoSesion(pstrUsuario, "BuscarMonedas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarMonedas")
            Return Nothing
        End Try
    End Function

    Public Function TraerMonedaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Moneda
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Moneda
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.Codigo = -1
            'e.Codigo_internacional = 
            'e.Descripcion = 
            'e.Convercion_Dolar = 
            'e.Nro_Decimales = 
            'e.Dias_Cumplimiento = 
            'e.ValorBase_IVA = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            'e.Mercado_Integrado = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerMonedaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertMoneda(ByVal Moneda As Moneda)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Moneda.pstrUsuarioConexion, Moneda.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Moneda.InfoSesion = DemeInfoSesion(Moneda.pstrUsuarioConexion, "InsertMoneda")
            Me.DataContext.Monedas.InsertOnSubmit(Moneda)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertMoneda")
        End Try
    End Sub

    Public Sub UpdateMoneda(ByVal currentMoneda As Moneda)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentMoneda.pstrUsuarioConexion, currentMoneda.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentMoneda.InfoSesion = DemeInfoSesion(currentMoneda.pstrUsuarioConexion, "UpdateMoneda")
            Me.DataContext.Monedas.Attach(currentMoneda, Me.ChangeSet.GetOriginal(currentMoneda))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateMoneda")
        End Try
    End Sub

    Public Sub DeleteMoneda(ByVal Moneda As Moneda)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Moneda.pstrUsuarioConexion, Moneda.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Monedas_Eliminar( pCodigo, DemeInfoSesion(pstrUsuario, "DeleteMoneda"),0).ToList
            Moneda.InfoSesion = DemeInfoSesion(Moneda.pstrUsuarioConexion, "DeleteMoneda")
            Me.DataContext.Monedas.Attach(Moneda)
            Me.DataContext.Monedas.DeleteOnSubmit(Moneda)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteMoneda")
        End Try
    End Sub

    Public Function ValidarMoneda(ByVal pintIdMoneda As Integer, ByVal pstrCodigoMoneda As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            Dim pstrMsgValidacion As String = ""
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgVal = Me.DataContext.uspOyDNet_Validar_Monedas_Validar(pintIdMoneda, pstrCodigoMoneda, pstrMsgValidacion, DemeInfoSesion(pstrUsuario, "ValidarMoneda"), 0)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarMoneda")
            Return Nothing
        End Try
    End Function

    Public Function EliminarMonedas(ByVal pCodigo As Integer, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Monedas_Eliminar(pCodigo, mensaje, DemeInfoSesion(pstrUsuario, "EliminarMonedas"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarMonedas")
            Return Nothing
        End Try
    End Function

    Public Function Traer_MonedaValor_Moneda(ByVal pCodigo As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MonedaValo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pCodigo) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_MonedaValor_Moneda_Consultar(pCodigo, DemeInfoSesion(pstrUsuario, "Traer_MonedaValor_Moneda"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_MonedaValor_Moneda")
        End Try
        Return Nothing
    End Function
#End Region
#Region "MonedaValor"

    Public Function MonedaValorFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MonedaValo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_MonedaValor_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "MonedaValorFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MonedaValorFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function MonedaValorConsultar(ByVal pIDMonedavalor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MonedaValo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_MonedaValor_Moneda_Consultar(pIDMonedavalor, DemeInfoSesion(pstrUsuario, "BuscarMonedaValor"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarMonedaValor")
            Return Nothing
        End Try
    End Function

    Public Function TraerMonedaValoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As MonedaValo
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New MonedaValo
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Codigo = 
            'e.FechaValor = 
            'e.Valor_Moneda_Local = 
            e.Base_IVA_Diario = 0
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            e.IDMonedavalor = -1
            'e.NroRegistro = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerMonedaValoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertMonedaValo(ByVal MonedaValo As MonedaValo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, MonedaValo.pstrUsuarioConexion, MonedaValo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            MonedaValo.InfoSesion = DemeInfoSesion(MonedaValo.pstrUsuarioConexion, "InsertMonedaValo")
            Me.DataContext.MonedaValos.InsertOnSubmit(MonedaValo)


        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertMonedaValo")
        End Try
    End Sub

    Public Sub UpdateMonedaValo(ByVal currentMonedaValo As MonedaValo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentMonedaValo.pstrUsuarioConexion, currentMonedaValo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentMonedaValo.InfoSesion = DemeInfoSesion(currentMonedaValo.pstrUsuarioConexion, "UpdateMonedaValo")
            Me.DataContext.MonedaValos.Attach(currentMonedaValo, Me.ChangeSet.GetOriginal(currentMonedaValo))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateMonedaValo")
        End Try
    End Sub

    Public Sub DeleteMonedaValo(ByVal MonedaValo As MonedaValo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,MonedaValo.pstrUsuarioConexion, MonedaValo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_MonedaValor_Eliminar( pIDMonedavalor, DemeInfoSesion(pstrUsuario, "DeleteMonedaValo"),0).ToList
            MonedaValo.InfoSesion = DemeInfoSesion(MonedaValo.pstrUsuarioConexion, "DeleteMonedaValo")
            Me.DataContext.MonedaValos.Attach(MonedaValo)
            Me.DataContext.MonedaValos.DeleteOnSubmit(MonedaValo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteMonedaValo")
        End Try
    End Sub
#End Region
#Region "Receptores"

    Public Function ReceptoresFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Receptore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Receptores_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ReceptoresFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoresFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ReceptoresConsultar(ByVal pCodigo As String, ByVal pidsucural As Integer, ByVal pidmesa As Integer, ByVal Nombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Receptore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Receptores_Consultar(pidsucural, pidmesa, Nombre, pCodigo, DemeInfoSesion(pstrUsuario, "BuscarReceptores"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarReceptores")
            Return Nothing
        End Try
    End Function
    Public Function ReceptoresConsultarEliminar(ByVal pstrID As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsultaExiste)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_Maestros_Receptores_ConsultarEliminar(pstrID, DemeInfoSesion(pstrUsuario, "BuscarReceptores"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarReceptores")
            Return Nothing
        End Try
    End Function

    Public Function TraerReceptorePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Receptore
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Receptore
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Sucursal = 
            'e.Codigo = 
            'e.Nombre = 
            'e.Activo = 
            'e.Tipo = 
            e.Estado = Now
            'e.Centro_costos = 
            'e.Login = 
            'e.Lider_Mesa = 
            'e.Codigo_Mesa = 
            'e.Numero_Documento = 
            'e.E_Mail = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.IdOficina = Nothing
            'e.IdReceptor = 
            'e.NumTrader = 
            'e.CodSetFX = 
            'e.RepresentanteLegalOtrosNegocios = 
            e.IdReceptores = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReceptorePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertReceptore(ByVal Receptore As Receptore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Receptore.pstrUsuarioConexion, Receptore.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Receptore.InfoSesion = DemeInfoSesion(Receptore.pstrUsuarioConexion, "InsertReceptore")
            Me.DataContext.Receptores.InsertOnSubmit(Receptore)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertReceptore")
        End Try
    End Sub

    Public Sub UpdateReceptore(ByVal currentReceptore As Receptore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentReceptore.pstrUsuarioConexion, currentReceptore.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentReceptore.InfoSesion = DemeInfoSesion(currentReceptore.pstrUsuarioConexion, "UpdateReceptore")
            Me.DataContext.Receptores.Attach(currentReceptore, Me.ChangeSet.GetOriginal(currentReceptore))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptore")
        End Try
    End Sub

    Public Sub DeleteReceptore(ByVal Receptore As Receptore)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Receptore.pstrUsuarioConexion, Receptore.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim consultaexis = Me.DataContext.uspOyDNet_Maestros_Receptores_ConsultarEliminar(Receptore.Codigo, DemeInfoSesion(Receptore.pstrUsuarioConexion, "BuscarReceptores"), 0).ToList

            For Each a In consultaexis
                If a.Existe = 0 Then
                    Receptore.Accion = CType("R", Char?)
                ElseIf (a.Existe = 1) Then
                    Receptore.Accion = CType("A", Char?)

                End If
                Exit For
            Next
            'Me.DataContext.uspOyDNet_Maestros_Receptores_Eliminar(pCodigo, DemeInfoSesion(pstrUsuario, "DeleteReceptore"), 0).ToList()
            Receptore.InfoSesion = DemeInfoSesion(Receptore.pstrUsuarioConexion, "DeleteReceptore")
            Me.DataContext.Receptores.Attach(Receptore)
            Me.DataContext.Receptores.DeleteOnSubmit(Receptore)

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptore")
        End Try
    End Sub

    Public Function Traer_ReceptoresSistemas_Receptore(ByVal pCodigo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pCodigo) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_ReceptoresSistemas_Receptore_Consultar(pCodigo, DemeInfoSesion(pstrUsuario, "Traer_ReceptoresSistemas_Receptore"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReceptoresSistemas_Receptore")
        End Try
        Return Nothing
    End Function
#End Region
#Region "ReceptoresSistemas"

    Public Function ReceptoresSistemasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ReceptoresSistemas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ReceptoresSistemasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ReceptoresSistemasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ReceptoresSistemasConsultar(ByVal pCodigo As String, ByVal pCodigo_Sistema As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ReceptoresSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ReceptoresSistemas_Consultar(pCodigo, pCodigo_Sistema, DemeInfoSesion(pstrUsuario, "BuscarReceptoresSistemas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarReceptoresSistemas")
            Return Nothing
        End Try
    End Function

    Public Function TraerReceptoresSistemaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ReceptoresSistema
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ReceptoresSistema
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Codigo = 
            'e.Codigo_Sistema = 
            'e.Valor_Sistema = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDReceptoresSistemas = 
            e.MontoLimite = 0
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerReceptoresSistemaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertReceptoresSistema(ByVal ReceptoresSistema As ReceptoresSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ReceptoresSistema.pstrUsuarioConexion, ReceptoresSistema.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ReceptoresSistema.InfoSesion = DemeInfoSesion(ReceptoresSistema.pstrUsuarioConexion, "InsertReceptoresSistema")
            Me.DataContext.ReceptoresSistemas.InsertOnSubmit(ReceptoresSistema)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertReceptoresSistema")
        End Try
    End Sub

    Public Sub UpdateReceptoresSistema(ByVal currentReceptoresSistema As ReceptoresSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentReceptoresSistema.pstrUsuarioConexion, currentReceptoresSistema.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentReceptoresSistema.InfoSesion = DemeInfoSesion(currentReceptoresSistema.pstrUsuarioConexion, "UpdateReceptoresSistema")
            Me.DataContext.ReceptoresSistemas.Attach(currentReceptoresSistema, Me.ChangeSet.GetOriginal(currentReceptoresSistema))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateReceptoresSistema")
        End Try
    End Sub

    Public Sub DeleteReceptoresSistema(ByVal ReceptoresSistema As ReceptoresSistema)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ReceptoresSistema.pstrUsuarioConexion, ReceptoresSistema.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ReceptoresSistemas_Eliminar( pCodigo,  pCodigo_Sistema, DemeInfoSesion(pstrUsuario, "DeleteReceptoresSistema"),0).ToList
            ReceptoresSistema.InfoSesion = DemeInfoSesion(ReceptoresSistema.pstrUsuarioConexion, "DeleteReceptoresSistema")
            Me.DataContext.ReceptoresSistemas.Attach(ReceptoresSistema)
            Me.DataContext.ReceptoresSistemas.DeleteOnSubmit(ReceptoresSistema)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteReceptoresSistema")
        End Try
    End Sub
#End Region
#Region "UsuariosFechaCierre"

    Public Function UsuariosFechaCierreFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of UsuariosFechaCierr)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_UsuariosFechaCierre_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "UsuariosFechaCierreFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UsuariosFechaCierreFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function UsuariosFechaCierreConsultar(ByVal pNombre_Usuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of UsuariosFechaCierr)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_UsuariosFechaCierre_Consultar(pNombre_Usuario, DemeInfoSesion(pstrUsuario, "BuscarUsuariosFechaCierre"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarUsuariosFechaCierre")
            Return Nothing
        End Try
    End Function

    Public Function TraerUsuariosFechaCierrPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As UsuariosFechaCierr
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New UsuariosFechaCierr
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Nombre_Usuario = 
            'e.Modulo = 
            e.Fecha_Cierre = Now
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.ID = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerUsuariosFechaCierrPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertUsuariosFechaCierr(ByVal UsuariosFechaCierr As UsuariosFechaCierr)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, UsuariosFechaCierr.pstrUsuarioConexion, UsuariosFechaCierr.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            UsuariosFechaCierr.InfoSesion = DemeInfoSesion(UsuariosFechaCierr.pstrUsuarioConexion, "InsertUsuariosFechaCierr")
            Me.DataContext.UsuariosFechaCierre.InsertOnSubmit(UsuariosFechaCierr)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertUsuariosFechaCierr")
        End Try
    End Sub

    Public Sub UpdateUsuariosFechaCierr(ByVal currentUsuariosFechaCierr As UsuariosFechaCierr)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentUsuariosFechaCierr.pstrUsuarioConexion, currentUsuariosFechaCierr.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentUsuariosFechaCierr.InfoSesion = DemeInfoSesion(currentUsuariosFechaCierr.pstrUsuarioConexion, "UpdateUsuariosFechaCierr")
            Me.DataContext.UsuariosFechaCierre.Attach(currentUsuariosFechaCierr, Me.ChangeSet.GetOriginal(currentUsuariosFechaCierr))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateUsuariosFechaCierr")
        End Try
    End Sub

    Public Sub DeleteUsuariosFechaCierr(ByVal UsuariosFechaCierr As UsuariosFechaCierr)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,UsuariosFechaCierr.pstrUsuarioConexion, UsuariosFechaCierr.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_UsuariosFechaCierre_Eliminar( pNombre_Usuario, DemeInfoSesion(pstrUsuario, "DeleteUsuariosFechaCierr"),0).ToList
            UsuariosFechaCierr.InfoSesion = DemeInfoSesion(UsuariosFechaCierr.pstrUsuarioConexion, "DeleteUsuariosFechaCierr")
            Me.DataContext.UsuariosFechaCierre.Attach(UsuariosFechaCierr)
            Me.DataContext.UsuariosFechaCierre.DeleteOnSubmit(UsuariosFechaCierr)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteUsuariosFechaCierr")
        End Try
    End Sub
#End Region
#Region "CodigosOtrosSistemas"

    Public Function CodigosOtrosSistemasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosOtrosSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodigosOtrosSistemas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "CodigosOtrosSistemasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CodigosOtrosSistemasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CodigosOtrosSistemasConsultar(ByVal pComitente As String, ByVal pCodigoSistema As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosOtrosSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodigosOtrosSistemas_Consultar(pComitente, pCodigoSistema, DemeInfoSesion(pstrUsuario, "BuscarCodigosOtrosSistemas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCodigosOtrosSistemas")
            Return Nothing
        End Try
    End Function

    Public Function TraerCodigosOtrosSistemaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CodigosOtrosSistema
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New CodigosOtrosSistema
            'e.Id = 
            'e.IdComisionista = 
            'e.IdSucComisionista = 
            'e.Comitente = 
            'e.Sistema = 
            'e.CodigoSistema = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCodigosOtrosSistemaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCodigosOtrosSistema(ByVal CodigosOtrosSistema As CodigosOtrosSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, CodigosOtrosSistema.pstrUsuarioConexion, CodigosOtrosSistema.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CodigosOtrosSistema.InfoSesion = DemeInfoSesion(CodigosOtrosSistema.pstrUsuarioConexion, "InsertCodigosOtrosSistema")
            Me.DataContext.CodigosOtrosSistemas.InsertOnSubmit(CodigosOtrosSistema)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCodigosOtrosSistema")
        End Try
    End Sub

    Public Sub UpdateCodigosOtrosSistema(ByVal currentCodigosOtrosSistema As CodigosOtrosSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCodigosOtrosSistema.pstrUsuarioConexion, currentCodigosOtrosSistema.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCodigosOtrosSistema.InfoSesion = DemeInfoSesion(currentCodigosOtrosSistema.pstrUsuarioConexion, "UpdateCodigosOtrosSistema")
            Me.DataContext.CodigosOtrosSistemas.Attach(currentCodigosOtrosSistema, Me.ChangeSet.GetOriginal(currentCodigosOtrosSistema))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCodigosOtrosSistema")
        End Try
    End Sub

    Public Sub DeleteCodigosOtrosSistema(ByVal CodigosOtrosSistema As CodigosOtrosSistema)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CodigosOtrosSistema.pstrUsuarioConexion, CodigosOtrosSistema.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_CodigosOtrosSistemas_Eliminar( pComitente,  pCodigoSistema, DemeInfoSesion(pstrUsuario, "DeleteCodigosOtrosSistema"),0).ToList
            CodigosOtrosSistema.InfoSesion = DemeInfoSesion(CodigosOtrosSistema.pstrUsuarioConexion, "DeleteCodigosOtrosSistema")
            Me.DataContext.CodigosOtrosSistemas.Attach(CodigosOtrosSistema)
            Me.DataContext.CodigosOtrosSistemas.DeleteOnSubmit(CodigosOtrosSistema)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCodigosOtrosSistema")
        End Try
    End Sub

    Public Function EliminarCodigoOtrosSistemas(ByVal plngId As Integer, ByVal plngIDComitente As String, ByVal pstrSistema As String, ByVal pstrCodigoSistema As String, ByVal pstrUsuario As String, ByVal strMsg As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CodigosOtrosSistemas_Eliminar(plngId, plngIDComitente, pstrSistema, pstrCodigoSistema, pstrUsuario, strMsg, DemeInfoSesion(pstrUsuario, "EliminarPaises"), 0)
            Return strMsg
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarPaises")
            Return Nothing
        End Try
    End Function
#End Region
#Region "CuentasDecevalPorAgrupador"

    Public Function CuentasDecevalPorAgrupadorFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasDecevalPorAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "CuentasDecevalPorAgrupadorFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasDecevalPorAgrupadorFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CuentasDecevalPorAgrupadorConsultar(ByVal pComitente As String, ByVal cuentadeceval As Integer, ByVal strDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasDecevalPorAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Consultar(pComitente, cuentadeceval, strDeposito, DemeInfoSesion(pstrUsuario, "BuscarCuentasDecevalPorAgrupador"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCuentasDecevalPorAgrupador")
            Return Nothing
        End Try
    End Function

    Public Function TraerCuentasDecevalPorAgrupadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CuentasDecevalPorAgrupado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New CuentasDecevalPorAgrupado
            'e.IdComisionista = 
            'e.IdSucComisionista = 
            'e.TipoIdComitente = 
            'e.NroDocumento = 
            'e.Comitente = 
            'e.CuentaDeceval = 
            'e.Conector1 = 
            'e.TipoIdBenef1 = 
            'e.NroDocBenef1 = 
            'e.Conector2 = 
            'e.TipoIdBenef2 = 
            'e.NroDocBenef2 = 
            'e.Deposito = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.CuentaPorCliente = True
            'e.ntermedia = 
            'e.CuentaPrincipalDCV = 
            'e.IDCuentasDeceval = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCuentasDecevalPorAgrupadoPorDefecto")
            Return Nothing
        End Try
    End Function
    Public Function TraerCuentasDecevalPorAgrupado_Deceval(ByVal pComitente As String, ByVal tipoID As Char, ByVal NroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasDeceval)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pComitente) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Deceval_Consultar(pComitente, tipoID, NroDocumento, DemeInfoSesion(pstrUsuario, "TraerCuentasDecevalPorAgrupado_Deceval"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCuentasDecevalPorAgrupado_Deceval")
            Return Nothing
        End Try

    End Function
    Public Function TraerBeneficiarios(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaBeneficiarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Consultar_Beneficiarios(pComitente, DemeInfoSesion(pstrUsuario, "TraerBeneficiarios"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerBeneficiarios")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCuentasDecevalPorAgrupado(ByVal CuentasDecevalPorAgrupado As CuentasDecevalPorAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, CuentasDecevalPorAgrupado.pstrUsuarioConexion, CuentasDecevalPorAgrupado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CuentasDecevalPorAgrupado.InfoSesion = DemeInfoSesion(CuentasDecevalPorAgrupado.pstrUsuarioConexion, "InsertCuentasDecevalPorAgrupado")
            Me.DataContext.CuentasDecevalPorAgrupador.InsertOnSubmit(CuentasDecevalPorAgrupado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCuentasDecevalPorAgrupado")
        End Try
    End Sub

    'Public Sub InsertUnificacuenta(ByVal ConsultaNombre As ConsultaNombre)
    '    Try
    '        ConsultaNombre.InfoSesion = DemeInfoSesion(pstrUsuario, "Unificacuenta")
    '        Me.DataContext.ConsultaNombres.InsertOnSubmit(ConsultaNombre)
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "Unificacuenta")
    '    End Try
    'End Sub
    Public Function UnificarCuenta(ByVal IdRetira As Integer, ByVal pstrAccion As Char, ByVal IdUnifica As Integer, ByVal pstrDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConsultaNombre
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Unificar(IdRetira, pstrAccion, IdUnifica, pstrDeposito, DemeInfoSesion(pstrUsuario, "ConsultaNombre"), 0)
            Return Nothing
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UnificarCuenta")
            Return Nothing
        End Try

    End Function
    Public Function TraerUnificarCuenta(ByVal IdRetira As Integer, ByVal pstrAccion As Char, ByVal pstrDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsultaNombre)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Consultar_Unificar(IdRetira, pstrAccion, pstrDeposito, DemeInfoSesion(pstrUsuario, "ConsultaNombre"), 0).ToList
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UnificarCuenta")
            Return Nothing
        End Try

    End Function
    Public Sub UpdateCuentasDecevalPorAgrupado(ByVal currentCuentasDecevalPorAgrupado As CuentasDecevalPorAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCuentasDecevalPorAgrupado.pstrUsuarioConexion, currentCuentasDecevalPorAgrupado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCuentasDecevalPorAgrupado.InfoSesion = DemeInfoSesion(currentCuentasDecevalPorAgrupado.pstrUsuarioConexion, "UpdateCuentasDecevalPorAgrupado")
            Me.DataContext.CuentasDecevalPorAgrupador.Attach(currentCuentasDecevalPorAgrupado, Me.ChangeSet.GetOriginal(currentCuentasDecevalPorAgrupado))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCuentasDecevalPorAgrupado")
        End Try
    End Sub

    Public Sub DeleteCuentasDecevalPorAgrupado(ByVal CuentasDecevalPorAgrupado As CuentasDecevalPorAgrupado)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CuentasDecevalPorAgrupado.pstrUsuarioConexion, CuentasDecevalPorAgrupado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Eliminar( pComitente, DemeInfoSesion(pstrUsuario, "DeleteCuentasDecevalPorAgrupado"),0).ToList
            CuentasDecevalPorAgrupado.InfoSesion = DemeInfoSesion(CuentasDecevalPorAgrupado.pstrUsuarioConexion, "DeleteCuentasDecevalPorAgrupado")
            Me.DataContext.CuentasDecevalPorAgrupador.Attach(CuentasDecevalPorAgrupado)
            Me.DataContext.CuentasDecevalPorAgrupador.DeleteOnSubmit(CuentasDecevalPorAgrupado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCuentasDecevalPorAgrupado")
        End Try
    End Sub
    Public Function EliminarCuentasDecevalPorAgrupador(ByVal comitente As String, ByVal cuentadeceval As Integer, ByVal idencuentadeceval As Integer, ByVal deposito As String, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Eliminar( pComitente, DemeInfoSesion(pstrUsuario, "DeleteCuentasDecevalPorAgrupado"),0).ToList
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Eliminar(comitente, cuentadeceval, idencuentadeceval, deposito, mensaje, DemeInfoSesion(pstrUsuario, "EliminarCuentasDecevalPorAgrupador"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarCuentasDecevalPorAgrupador")
            Return Nothing
        End Try
    End Function



#End Region
#Region "Bancos_BancosNacionalesRelacionados"

    Public Function Bancos_BancosNacionalesRelacionadosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Bancos_BancosNacionalesRelacionado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Bancos_BancosNacionalesRelacionados_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "Bancos_BancosNacionalesRelacionadosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Bancos_BancosNacionalesRelacionadosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function Bancos_BancosNacionalesRelacionadosConsultar(ByVal pIDBanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Bancos_BancosNacionalesRelacionado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Bancos_BancosNacionalesRelacionados_Consultar(pIDBanco, DemeInfoSesion(pstrUsuario, "BuscarBancos_BancosNacionalesRelacionados"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBancos_BancosNacionalesRelacionados")
            Return Nothing
        End Try
    End Function

    Public Function TraerBancos_BancosNacionalesRelacionadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Bancos_BancosNacionalesRelacionado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Bancos_BancosNacionalesRelacionado
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.IDBanco = 
            'e.IdBancoNacional = 
            'e.Actualizacion = 
            'e.Usuario = 
            e.IDBancosNacionalesR = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerBancos_BancosNacionalesRelacionadoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertBancos_BancosNacionalesRelacionado(ByVal Bancos_BancosNacionalesRelacionado As Bancos_BancosNacionalesRelacionado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Bancos_BancosNacionalesRelacionado.pstrUsuarioConexion, Bancos_BancosNacionalesRelacionado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Bancos_BancosNacionalesRelacionado.InfoSesion = DemeInfoSesion(Bancos_BancosNacionalesRelacionado.pstrUsuarioConexion, "InsertBancos_BancosNacionalesRelacionado")
            Me.DataContext.Bancos_BancosNacionalesRelacionados.InsertOnSubmit(Bancos_BancosNacionalesRelacionado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertBancos_BancosNacionalesRelacionado")
        End Try
    End Sub

    Public Sub UpdateBancos_BancosNacionalesRelacionado(ByVal currentBancos_BancosNacionalesRelacionado As Bancos_BancosNacionalesRelacionado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentBancos_BancosNacionalesRelacionado.pstrUsuarioConexion, currentBancos_BancosNacionalesRelacionado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentBancos_BancosNacionalesRelacionado.InfoSesion = DemeInfoSesion(currentBancos_BancosNacionalesRelacionado.pstrUsuarioConexion, "UpdateBancos_BancosNacionalesRelacionado")
            Me.DataContext.Bancos_BancosNacionalesRelacionados.Attach(currentBancos_BancosNacionalesRelacionado, Me.ChangeSet.GetOriginal(currentBancos_BancosNacionalesRelacionado))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateBancos_BancosNacionalesRelacionado")
        End Try
    End Sub

    Public Sub DeleteBancos_BancosNacionalesRelacionado(ByVal Bancos_BancosNacionalesRelacionado As Bancos_BancosNacionalesRelacionado)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Bancos_BancosNacionalesRelacionado.pstrUsuarioConexion, Bancos_BancosNacionalesRelacionado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Bancos_BancosNacionalesRelacionados_Eliminar( pIDBanco, DemeInfoSesion(pstrUsuario, "DeleteBancos_BancosNacionalesRelacionado"),0).ToList
            Bancos_BancosNacionalesRelacionado.InfoSesion = DemeInfoSesion(Bancos_BancosNacionalesRelacionado.pstrUsuarioConexion, "DeleteBancos_BancosNacionalesRelacionado")
            Me.DataContext.Bancos_BancosNacionalesRelacionados.Attach(Bancos_BancosNacionalesRelacionado)
            Me.DataContext.Bancos_BancosNacionalesRelacionados.DeleteOnSubmit(Bancos_BancosNacionalesRelacionado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteBancos_BancosNacionalesRelacionado")
        End Try
    End Sub

    Public Function llenarBancosrelacionadosasociados(ByVal banco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaBancosRelacionadosAsociados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspOyDNet_Maestros_BancosRelacionados_Asociados(banco).ToList
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarllenarBancosrelacionadosasociados")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Llenarcombocontraparteotc"
    Public Function LLenarcontraparteotc(ByVal pstrnitfirma As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLLenarCboContraparteOTC_OyDNet(pstrnitfirma).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LLenarcontraparteotc")
            Return Nothing
        End Try
    End Function

#End Region
#Region "Oficinas"

    Public Function OficinasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Oficinas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Oficinas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "OficinasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OficinasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function OficinasConsultar(ByVal plngOficina As Integer, ByVal pstrNomOficina As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Oficinas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Oficinas_Consultar(plngOficina, pstrNomOficina, DemeInfoSesion(pstrUsuario, "BuscarOficinas"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarOficinas")
            Return Nothing
        End Try
    End Function

    Public Function TraerOficinaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Oficinas
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Oficinas
            'e.IdOficina = -1
            'e.NomOficina = ""
            'e.NomOficina = ""
            'e.IdSucursal = -1
            'e.Codigo_internacional = 
            'e.Descripcion = 
            'e.Convercion_Dolar = 
            'e.Nro_Decimales = 
            'e.Dias_Cumplimiento = 
            'e.ValorBase_IVA = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            'e.Mercado_Integrado = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerOficinaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertOficina(ByVal Oficina As Oficinas)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Oficina.pstrUsuarioConexion, Oficina.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Oficina.InfoSesion = DemeInfoSesion(Oficina.pstrUsuarioConexion, "InsertOficina")

            Me.DataContext.Oficinas.InsertOnSubmit(Oficina)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOficina")
        End Try
    End Sub

    Public Sub UpdateOficina(ByVal CodigoOficina As Oficinas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, CodigoOficina.pstrUsuarioConexion, CodigoOficina.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CodigoOficina.InfoSesion = DemeInfoSesion(CodigoOficina.pstrUsuarioConexion, "UpdateOficina")
            Me.DataContext.Oficinas.Attach(CodigoOficina, Me.ChangeSet.GetOriginal(CodigoOficina))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOficina")
        End Try
    End Sub

    Public Sub DeleteOficina(ByVal CodigoOficina As Oficinas)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CodigoOficina.pstrUsuarioConexion, CodigoOficina.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'DARM 20180516 SE QUITAN EL PARAMETRO STRACCION
            Dim consultaexis = Me.DataContext.uspOyDNet_Maestros_Oficina_Eliminar(CInt(CodigoOficina.IdOficina), CodigoOficina.Usuario, DemeInfoSesion(CodigoOficina.pstrUsuarioConexion, "BuscarOficinas"), 0)


            'For Each a In consultaexis
            '    If a.Existe = 0 Then

            '        Oficinas.Accion = CType("R", Char?)
            '    ElseIf (a.Existe = 1) Then
            '        Oficinas.Accion = CType("A", Char?)

            '    End If
            '    Exit For
            'Next
            'DARM 20180516 SE QUITAN LAS SIGUIENTES LINEAS QUE IMPIDEN LA ELIMINACION DE UNA OFICINA E INTENTA HACERLO DOS VECES
            'CodigoOficina.InfoSesion = DemeInfoSesion(pstrUsuario, "DeleteOficina")
            'Me.DataContext.Oficinas.Attach(CodigoOficina)
            'Me.DataContext.Oficinas.DeleteOnSubmit(CodigoOficina)

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOficina")
        End Try
    End Sub

    'RBP20160729_Consulta para los centros de costos
    Public Function ConsultarCentroCostos(ByVal plngID As Integer, ByVal pstrID As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tmpCCostos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spConsultarCCosto_ENCUENTA(plngID, pstrID).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCentroCostos")
            Return Nothing
        End Try
    End Function
#End Region

    Public Function VerificaParametros(ByVal parametro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim retorno As String = String.Empty
            Dim ret = Me.DataContext.usp_Valida4porlmil_OyDNet(parametro, retorno)
            Return retorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificaParametros")
            Return Nothing
        End Try
    End Function
    '#Region "AjustesBancarios"
    '    Public Function AjustesBancariosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of AjustesBancario)
    '        Try
    '            Dim ret = Me.DataContext.uspOyDNet_AjustesBancarios_Filtrar(pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "AjustesBancariosFiltrar"), 0).ToList
    '            Return ret
    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "AjustesBancariosFiltrar")
    '            Return Nothing
    '        End Try
    '    End Function
    '    Public Function AjustesBancariosConsultar(ByVal pstrTipo As String, ByVal pstrNombreConsecutivo As String, ByVal plngIDDocumento As Integer, ByVal pdtmDocumento As DateTime, ByVal plngIDBanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of AjustesBancario)
    '        Try
    '            Dim ret = Me.DataContext.uspOyDNet_AjustesBancarios_Consultar(pstrTipo, pstrNombreConsecutivo, plngIDDocumento, pdtmDocumento, plngIDBanco, DemeInfoSesion(pstrUsuario, "AjustesBancariosConsultar"), 0).ToList
    '            Return ret
    '            Exit Function
    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "AjustesBancariosConsultar")
    '            Return Nothing
    '        End Try
    '    End Function
    '    Public Function Traer_AjustesBancariosPorDefecto( ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS AjustesBancario
    '        Try
    '            Dim e As New AjustesBancario
    '            'e.IDComisionista = 
    '            'e.IDSucComisionista = 
    '            'e.IDBanco = 
    '            'e.IdBancoNacional = 
    '            'e.Actualizacion = 
    '            'e.Usuario = 
    '            e.IDTesoreria = -1
    '            Return e
    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "Traer_AjustesBancariosPorDefecto")
    '            Return Nothing
    '        End Try
    '    End Function
    '    Public Sub Insert_AjustesBancarios(ByVal AjustesBancarios As AjustesBancario)
    '        Try
    '            AjustesBancarios.InfoSesion = DemeInfoSesion(pstrUsuario, "Insert_AjustesBancarios")
    '            Me.DataContext.AjustesBancario.InsertOnSubmit(AjustesBancarios)
    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "Insert_AjustesBancarios")
    '        End Try
    '    End Sub

    '    Public Sub Update_AjustesBancarios(ByVal currentAjustesBancarios As AjustesBancario)
    '        Try
    '            currentAjustesBancarios.InfoSesion = DemeInfoSesion(pstrUsuario, "Update_AjustesBancarios")
    '            Me.DataContext.AjustesBancario.Attach(currentAjustesBancarios, Me.ChangeSet.GetOriginal(currentAjustesBancarios))
    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "Update_AjustesBancarios")
    '        End Try
    '    End Sub

    '#End Region
#Region "Ordenes"

    Public Function OrdenesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Ordene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Ordenes_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "OrdenesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesConsultar(ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Ordene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Ordenes_Consultar(pID, DemeInfoSesion(pstrUsuario, "BuscarOrdenes"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarOrdenes")
            Return Nothing
        End Try
    End Function

    Public Function TraerOrdenePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Ordene
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Ordene
            'e.IDComisionista = 
            'e.IdSucComisionista = 
            'e.Tipo = 
            'e.Clase = 
            'e.ID = 
            'e.Version = 
            'e.Ordinaria = 
            'e.Objeto = 
            'e.Repo = 
            'e.Renovacion = 
            'e.IDComitente = 
            'e.IDOrdenante = 
            'e.ComisionPactada = 
            'e.CondicionesNegociacion = 
            'e.TipoLimite = 
            'e.FormaPago = 
            'e.Orden = 
            'e.VigenciaHasta = 
            'e.Instrucciones = 
            'e.Notas = 
            'e.Estado = 
            'e.Estado = 
            'e.Sistema = 
            'e.UBICACIONTITULO = 
            'e.TipoInversion = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDPreliquidacion = 
            'e.IDProducto = 
            'e.CostoAdicionalesOrden = 
            'e.IdBolsa = 
            'e.UsuarioIngreso = 
            'e.NegocioEspecial = 
            'e.Eca = 
            'e.OrdenEscrito = 
            'e.ConsecutivoSwap = 
            'e.Ejecucion = 
            'e.Duracion = 
            'e.CantidadMinima = 
            'e.PrecioStop = 
            'e.CantidadVisible = 
            'e.HoraVigencia = 
            'e.EstadoLEO = 
            'e.UsuarioOperador = 
            'e.CanalRecepcion = 
            'e.MedioVerificable = 
            'e.FechaHoraRecepcion = 
            'e.SitioIngreso = 
            'e.Seteada = 
            'e.Folio = 
            'e.TipoOrdenPreOrdenes = 
            'e.NroOrdenPreOrdenes = 
            'e.Impresion = 
            'e.Impresiones = 
            'e.PreordenDetalle = 
            'e.EstadoOrdenBus = 
            'e.IDOrdenes = 
            'e.IpOrigen = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerOrdenePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertOrdene(ByVal Ordene As Ordene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Ordene.pstrUsuarioConexion, Ordene.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Ordene.InfoSesion = DemeInfoSesion(Ordene.pstrUsuarioConexion, "InsertOrdene")
            Me.DataContext.Ordenes.InsertOnSubmit(Ordene)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOrdene")
        End Try
    End Sub

    Public Sub UpdateOrdene(ByVal currentOrdene As Ordene)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentOrdene.pstrUsuarioConexion, currentOrdene.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentOrdene.InfoSesion = DemeInfoSesion(currentOrdene.pstrUsuarioConexion, "UpdateOrdene")
            Me.DataContext.Ordenes.Attach(currentOrdene, Me.ChangeSet.GetOriginal(currentOrdene))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOrdene")
        End Try
    End Sub

    Public Sub DeleteOrdene(ByVal Ordene As Ordene)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Ordene.pstrUsuarioConexion, Ordene.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Ordenes_Eliminar( pID, DemeInfoSesion(pstrUsuario, "DeleteOrdene"),0).ToList
            Ordene.InfoSesion = DemeInfoSesion(Ordene.pstrUsuarioConexion, "DeleteOrdene")
            Me.DataContext.Ordenes.Attach(Ordene)
            Me.DataContext.Ordenes.DeleteOnSubmit(Ordene)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOrdene")
        End Try
    End Sub
#End Region


#End Region
#End Region

#Region "JEISON RAMIREZ"
#Region "ResolucionesFacturas"

    Public Function ResolucionesFacturasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ResolucionesFactura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ResolucionesFacturas_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ResolucionesFacturasFiltrar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ResolucionesFacturasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ResolucionesFacturasConsultar(ByVal pNumeroResolucion As Int64, ByVal pstrDescripcionResolucion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ResolucionesFactura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ResolucionesFacturas_Consultar(pNumeroResolucion, pstrDescripcionResolucion, DemeInfoSesion(pstrUsuario, "BuscarResolucionesFacturas"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarResolucionesFacturas")
            Return Nothing
        End Try
    End Function

    Public Function TraerResolucionesFacturaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ResolucionesFactura
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ResolucionesFactura
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.IDCodigoResolucion = 
            'e.NumeroResolucion = 
            'e.DescripcionResolucion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerResolucionesFacturaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertResolucionesFactura(ByVal ResolucionesFactura As ResolucionesFactura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ResolucionesFactura.pstrUsuarioConexion, ResolucionesFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ResolucionesFactura.InfoSesion = DemeInfoSesion(ResolucionesFactura.pstrUsuarioConexion, "InsertResolucionesFactura")
            Me.DataContext.ResolucionesFacturas.InsertOnSubmit(ResolucionesFactura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertResolucionesFactura")
        End Try
    End Sub

    Public Sub UpdateResolucionesFactura(ByVal currentResolucionesFactura As ResolucionesFactura)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentResolucionesFactura.pstrUsuarioConexion, currentResolucionesFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentResolucionesFactura.InfoSesion = DemeInfoSesion(currentResolucionesFactura.pstrUsuarioConexion, "UpdateResolucionesFactura")
            Me.DataContext.ResolucionesFacturas.Attach(currentResolucionesFactura, Me.ChangeSet.GetOriginal(currentResolucionesFactura))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateResolucionesFactura")
        End Try
    End Sub

    Public Sub DeleteResolucionesFactura(ByVal ResolucionesFactura As ResolucionesFactura)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ResolucionesFactura.pstrUsuarioConexion, ResolucionesFactura.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ResolucionesFacturas_Eliminar( pNumeroResolucion, DemeInfoSesion(pstrUsuario, "DeleteResolucionesFactura"),0).ToList
            ResolucionesFactura.InfoSesion = DemeInfoSesion(ResolucionesFactura.pstrUsuarioConexion, "DeleteResolucionesFactura")
            Me.DataContext.ResolucionesFacturas.Attach(ResolucionesFactura)
            Me.DataContext.ResolucionesFacturas.DeleteOnSubmit(ResolucionesFactura)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteResolucionesFactura")
        End Try
    End Sub

    Public Function EliminarResolucionesFacturas(ByVal pIDCodigoResolucion As Integer, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ResolucionesFacturas_Eliminar(pIDCodigoResolucion, mensaje, DemeInfoSesion(pstrUsuario, "EliminarResolucionesFacturas"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarResolucionesFacturas")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Cuentas Bancarias"


    Public Function BancosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Banco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Cuentasbancarias_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "BancosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BancosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function BancosConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pNombreSucursal As String, ByVal pIDCia As Integer, ByVal pstrNroCuenta As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Banco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Cuentasbancarias_Consultar(pID, pNombre, pNombreSucursal, pIDCia, DemeInfoSesion(pstrUsuario, "BuscarBancos"), 0, pstrNroCuenta).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBancos")
            Return Nothing
        End Try
    End Function

    Public Function TraerBancoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Banco
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Banco

            Dim strSQL = "SELECT lngId as strCampo FROM tblMonedas WHERE strDescripcion = @p0 "
            Dim valor = fnEjecutarQuerySQL(strSQL, "PESOS COLOMBIANOS")
            If Not String.IsNullOrEmpty(valor) Then
                e.IdMoneda = CInt(valor)
            End If

            Dim strSQLNroCompanias = "SELECT COUNT(1) as strCampo FROM cf.tblCompanias "
            Dim NroCompañias = fnEjecutarQuerySQL(strSQLNroCompanias)
            If CInt(NroCompañias) = 1 Then
                Dim strSQLCodigoCompania = "SELECT intIDCompania as strCampo FROM cf.tblCompanias "
                Dim intIDCompañia = fnEjecutarQuerySQL(strSQLCodigoCompania)
                e.Compania = CInt(intIDCompañia)

                Dim strSQLNombreCompania = "SELECT strNombre as strCampo FROM cf.tblCompanias "
                Dim strNombreCompañia = fnEjecutarQuerySQL(strSQLNombreCompania)
                e.NombreCompania = strNombreCompañia
            Else
                e.Compania = 0
                e.NombreCompania = String.Empty
            End If
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.ID = 
            'e.Nombre = 
            'e.NombreSucursal = 
            'e.NroCuenta = 
            'e.Telefono1 = 
            'e.Telefono2 = 
            'e.Fax1 = 
            'e.Fax2 = 
            'e.Direccion = 
            'e.Internet = 
            'e.ChequeraAutomatica = 
            'e.NombreConsecutivo = 
            'e.IDPoblacion = 
            'e.IDDepartamento = 
            'e.IDPais = 
            'e.NombreGerente = 
            'e.NombreCajero = 
            'e.NombrePortero = 
            e.Creacion = Now.Date
            e.IDOwner = "S"
            'e.IdCuentaCtble = 
            e.CtaActiva = True
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Reporte = 
            'e.CobroGMF = 
            'e.TipoCta = 
            'e.TipoCuenta = 
            e.IdCodBanco = Nothing
            e.IDBanco = -1
            'e.IdMoneda =
            'e.IDFormato =
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerBancoPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función que permite ejecutar Querys en SQL Server con o sin parametros.
    ''' </summary>
    ''' <param name="strSQL"></param>
    ''' <param name="strParametro"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130604</remarks>
    Private Function fnEjecutarQuerySQL(ByVal strSQL As String, Optional strParametro As Object = Nothing) As String
        Dim L2SDC As New OyDDataContext
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

    Public Sub InsertBanco(ByVal Banco As Banco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Banco.pstrUsuarioConexion, Banco.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Banco.InfoSesion = DemeInfoSesion(Banco.pstrUsuarioConexion, "InsertBanco")
            Me.DataContext.CuentasBancarias.InsertOnSubmit(Banco)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertBanco")
        End Try
    End Sub

    Public Sub UpdateBanco(ByVal currentBanco As Banco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentBanco.pstrUsuarioConexion, currentBanco.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentBanco.InfoSesion = DemeInfoSesion(currentBanco.pstrUsuarioConexion, "UpdateBanco")
            Me.DataContext.CuentasBancarias.Attach(currentBanco, Me.ChangeSet.GetOriginal(currentBanco))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateBanco")
        End Try
    End Sub

    Public Sub DeleteBanco(ByVal Banco As Banco)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Banco.pstrUsuarioConexion, Banco.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros__Cuentasbancarias_Eliminar( pID,  pNombre,  pNombreSucursal, DemeInfoSesion(pstrUsuario, "DeleteBanco"),0).ToList
            Banco.InfoSesion = DemeInfoSesion(Banco.pstrUsuarioConexion, "DeleteBanco")
            Me.DataContext.CuentasBancarias.Attach(Banco)
            Me.DataContext.CuentasBancarias.DeleteOnSubmit(Banco)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteBanco")
        End Try
    End Sub

    Public Function EliminarCuentasBancarias(ByVal strMsg As String, ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Cuentasbancarias_Eliminar(strMsg, pID, DemeInfoSesion(pstrUsuario, "EliminarCuentasBancarias"), 0)
            Return strMsg
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarCuentasBancarias")
            Return Nothing
        End Try
    End Function
#End Region

#Region "SaldosxMes"


    Public Function SaldosBancoMesConsultar(ByVal pintBanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of SaldosBancoMes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_SaldosBancoMes_Consultar(pintBanco, DemeInfoSesion(pstrUsuario, "BuscarSaldosBancoMes"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarSaldosBancoMes")
            Return Nothing
        End Try
    End Function



#End Region

#Region "MovimientosBancos"


    Public Function MovimientosBancosConsultar(ByVal plngIDBanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of MovimientosBancos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_MovimientosBancos_Consultar(plngIDBanco, DemeInfoSesion(pstrUsuario, "BuscarMovimientosBancos"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarMovimientosBancos")
            Return Nothing
        End Try
    End Function


#End Region

#Region "ConfiguraciónLEO"
    Public Function ConfigLEOFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfigLE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConfigLEO_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ConfigLEOFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfigLEOFiltrar")
            Return Nothing
        End Try
    End Function
    Public Function TraerConfigLEPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfigLE
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ConfigLE
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = -1
            'e.Nombre = 
            'e.NombreSQL = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.NombreDescSQL = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerConfigLEPorDefecto")
            Return Nothing
        End Try
    End Function
    Public Sub InsertConfigLE(ByVal ConfigLE As ConfigLE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ConfigLE.pstrUsuarioConexion, ConfigLE.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ConfigLE.InfoSesion = DemeInfoSesion(ConfigLE.pstrUsuarioConexion, "InsertConfigLE")
            Me.DataContext.ConfigLEO.InsertOnSubmit(ConfigLE)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConfigLE")
        End Try
    End Sub
    Public Sub UpdateConfigLE(ByVal currentConfigLE As ConfigLE)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentConfigLE.pstrUsuarioConexion, currentConfigLE.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConfigLE.InfoSesion = DemeInfoSesion(currentConfigLE.pstrUsuarioConexion, "UpdateConfigLE")
            Me.DataContext.ConfigLEO.Attach(currentConfigLE, Me.ChangeSet.GetOriginal(currentConfigLE))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConfigLE")
        End Try
    End Sub
    Public Sub DeleteConfigLE(ByVal ConfigLE As ConfigLE)
        Try
            ConfigLE.Index = 0
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ConfigLE.pstrUsuarioConexion, ConfigLE.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConfigLEO_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteConfigLE"),0).ToList
            ConfigLE.InfoSesion = DemeInfoSesion(ConfigLE.pstrUsuarioConexion, "DeleteConfigLE")
            Me.DataContext.ConfigLEO.Attach(ConfigLE)
            Me.DataContext.ConfigLEO.DeleteOnSubmit(ConfigLE)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConfigLE")
        End Try
    End Sub
    Public Function ConfigLEOFiltrarDisponibles(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaConfigLEO)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConfigLEO_Filtrar_Disponible(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ConfigLEOFiltrarDisponible"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfigLEOFiltrarDisponible")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para actualizar los registros de LEOS
    ''' </summary>
    ''' <param name="plogNuevo"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>Santiago Vergara - Marzo 13/2014</remarks>
    Public Function InsertarLEOS(ByVal plogNuevo As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_LEO_Insertar(plogNuevo, pstrUsuario, DemeInfoSesion(pstrUsuario, "InsertarLEOS"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarLEOS")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Actualizar Estado Leo"
    Public Function OrdenesLEO_Consultar(ByVal pstrOperado As String, ByVal plngIDLLEO As Integer, ByVal pdtmFechaOrden As Date, ByVal pstrIDReceptor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListadoActualizarLEO)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_OrdenesLEO_Consultar(pstrOperado, pdtmFechaOrden, plngIDLLEO, pstrIDReceptor, DemeInfoSesion(pstrUsuario, "OrdenesLEO_Consultar"), ClsConstantes.GINT_ErrorPersonalizado).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesLEO_Consultar")
            Return Nothing
        End Try

    End Function

    Public Function OrdenesLEO_DatosOrden(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal pstrDatoReceptor As String, ByVal pstrLEO As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            Dim STR_DETALLE As String = ""
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_OrdenesLEO_DatosOrden(pstrTipo, pstrClase, plngID, pstrDatoReceptor, pstrLEO, STR_DETALLE, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesLEO_DatosOrden"), ClsConstantes.GINT_ErrorPersonalizado)
            Return STR_DETALLE
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesLEO_DatosOrden")
            Return Nothing
        End Try
    End Function


    Public Function OrdenesLEO_TipoOrden(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            Dim pstrTipoLEO As String = ""
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_OrdenesLEO_TipoOrden(pstrTipoLEO, pstrUsuario, DemeInfoSesion(pstrUsuario, "OrdenesLEO_TipoOrden"), ClsConstantes.GINT_ErrorPersonalizado)
            Return pstrTipoLEO
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesLEO_TipoOrden")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesEstadoLEO_Actualizar(ByVal plngIDOrden As Integer, ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pstrEstadoLEO As String, pChkUndo As Boolean, ByVal pdtmFechaEstadoLEO As Date, ByVal pstrUsuarioLEO As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_OrdenesEstadoLEO_Actualizar(plngIDOrden, pstrTipo, pstrClase, pstrEstadoLEO, pChkUndo, pdtmFechaEstadoLEO, pstrUsuarioLEO, DemeInfoSesion(pstrUsuario, "OrdenesLEO_Consultar"), ClsConstantes.GINT_ErrorPersonalizado)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesEstadoLEO_Actualizar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConsultarOrdenesLEO(ByVal currentListadoActualizarLEO As ListadoActualizarLEO)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConsultarOrdenesLEO")
        End Try
    End Sub

    Public Sub UpdateConsultarOrdenesLEO(ByVal ListadoActualizarLEO As ListadoActualizarLEO)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConsultarOrdenesLEO")
        End Try
    End Sub

    Public Sub DeleteConsultarOrdenesLEO(ByVal ListadoActualizarLEO As ListadoActualizarLEO)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConsecutivosVsUsuarios")
        End Try
    End Sub


#End Region


#Region "Usuarios_Seleccionar"

    Public Function Usuarios_SeleccionarFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Usuarios_Selecciona)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Usuarios_Seleccionar_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "Usuarios_SeleccionarFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Usuarios_SeleccionarFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function Usuarios_SeleccionarConsultar(ByVal pId As Integer, ByVal pLogin As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Usuarios_Selecciona)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Usuarios_Seleccionar_Consultar(pId, pLogin, DemeInfoSesion(pstrUsuario, "BuscarUsuarios_Seleccionar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarUsuarios_Seleccionar")
            Return Nothing
        End Try
    End Function

    Public Function TraerUsuarios_SeleccionaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Usuarios_Selecciona
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Usuarios_Selecciona
            'e.Id = -1
            'e.Login = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerUsuarios_SeleccionaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertUsuarios_Selecciona(ByVal Usuarios_Selecciona As Usuarios_Selecciona)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Usuarios_Selecciona.pstrUsuarioConexion, Usuarios_Selecciona.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Usuarios_Selecciona.InfoSesion = DemeInfoSesion(Usuarios_Selecciona.pstrUsuarioConexion, "InsertUsuarios_Selecciona")
            Me.DataContext.Usuarios_Seleccionar.InsertOnSubmit(Usuarios_Selecciona)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertUsuarios_Selecciona")
        End Try
    End Sub

    Public Sub UpdateUsuarios_Selecciona(ByVal currentUsuarios_Selecciona As Usuarios_Selecciona)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentUsuarios_Selecciona.pstrUsuarioConexion, currentUsuarios_Selecciona.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentUsuarios_Selecciona.InfoSesion = DemeInfoSesion(currentUsuarios_Selecciona.pstrUsuarioConexion, "UpdateUsuarios_Selecciona")
            Me.DataContext.Usuarios_Seleccionar.Attach(currentUsuarios_Selecciona, Me.ChangeSet.GetOriginal(currentUsuarios_Selecciona))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateUsuarios_Selecciona")
        End Try
    End Sub

    Public Sub DeleteUsuarios_Selecciona(ByVal Usuarios_Selecciona As Usuarios_Selecciona)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Usuarios_Selecciona.pstrUsuarioConexion, Usuarios_Selecciona.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Usuarios_Seleccionar_Eliminar( pId,  pLogin, DemeInfoSesion(pstrUsuario, "DeleteUsuarios_Selecciona"),0).ToList
            Usuarios_Selecciona.InfoSesion = DemeInfoSesion(Usuarios_Selecciona.pstrUsuarioConexion, "DeleteUsuarios_Selecciona")
            Me.DataContext.Usuarios_Seleccionar.Attach(Usuarios_Selecciona)
            Me.DataContext.Usuarios_Seleccionar.DeleteOnSubmit(Usuarios_Selecciona)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteUsuarios_Selecciona")
        End Try
    End Sub
#End Region

#Region "ClienteAgrupador"

    Public Function ClienteAgrupadorFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClienteAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClienteAgrupador_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ClienteAgrupadorFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClienteAgrupadorFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ClienteAgrupadorConsultar(ByVal pNroDocumento As String, ByVal pNombreAgrupador As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClienteAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClienteAgrupador_Consultar(pNroDocumento, pNombreAgrupador, DemeInfoSesion(pstrUsuario, "BuscarClienteAgrupador"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClienteAgrupador")
            Return Nothing
        End Try
    End Function

    Public Function TraerClienteAgrupadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ClienteAgrupado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ClienteAgrupado
            'e.idComisionista = 
            'e.idSucComisionista = 
            'e.IDAgrupador = 
            'e.NroDocumento = 
            'e.TipoIdentificacion = 
            'e.idComitenteLider = 
            'e.NombreAgrupador = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Actualizacion = 
            'e.IDClienteAgrupador=
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClienteAgrupadoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertClienteAgrupado(ByVal ClienteAgrupado As ClienteAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClienteAgrupado.pstrUsuarioConexion, ClienteAgrupado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClienteAgrupado.InfoSesion = DemeInfoSesion(ClienteAgrupado.pstrUsuarioConexion, "InsertClienteAgrupado")
            Me.DataContext.ClienteAgrupador.InsertOnSubmit(ClienteAgrupado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClienteAgrupado")
        End Try
    End Sub

    Public Sub UpdateClienteAgrupado(ByVal currentClienteAgrupado As ClienteAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClienteAgrupado.pstrUsuarioConexion, currentClienteAgrupado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClienteAgrupado.InfoSesion = DemeInfoSesion(currentClienteAgrupado.pstrUsuarioConexion, "UpdateClienteAgrupado")
            Me.DataContext.ClienteAgrupador.Attach(currentClienteAgrupado, Me.ChangeSet.GetOriginal(currentClienteAgrupado))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClienteAgrupado")
        End Try
    End Sub

    Public Sub DeleteClienteAgrupado(ByVal ClienteAgrupado As ClienteAgrupado)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClienteAgrupado.pstrUsuarioConexion, ClienteAgrupado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ClienteAgrupador_Eliminar( pNroDocumento,  pNombreAgrupador, DemeInfoSesion(pstrUsuario, "DeleteClienteAgrupado"),0).ToList
            ClienteAgrupado.InfoSesion = DemeInfoSesion(ClienteAgrupado.pstrUsuarioConexion, "DeleteClienteAgrupado")
            Me.DataContext.ClienteAgrupador.Attach(ClienteAgrupado)
            Me.DataContext.ClienteAgrupador.DeleteOnSubmit(ClienteAgrupado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClienteAgrupado")
        End Try
    End Sub
#End Region

#Region "DetalleClienteAgrupador"

    Public Function DetalleClienteAgrupadorFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleClienteAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DetalleClienteAgrupador_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "DetalleClienteAgrupadorFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleClienteAgrupadorFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DetalleClienteAgrupadorConsultar(ByVal pId As String, ByVal pNombre As String, ByVal pDireccionEnvio As String, ByVal pidReceptor As String, ByVal pIDSucCliente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleClienteAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DetalleClienteAgrupador_Consultar(pId, pNombre, pDireccionEnvio, pidReceptor, pIDSucCliente, DemeInfoSesion(pstrUsuario, "BuscarDetalleClienteAgrupador"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarDetalleClienteAgrupador")
            Return Nothing
        End Try
    End Function

    Public Function TraerDetalleClienteAgrupadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DetalleClienteAgrupado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DetalleClienteAgrupado
            'e.Id = 
            'e.Nombre = 
            'e.DireccionEnvio = 
            'e.idReceptor = 
            'e.IDSucCliente = 
            e.IDDetalleClienteAgrupador = -1
            Return (e)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDetalleClienteAgrupadoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDetalleClienteAgrupado(ByVal DetalleClienteAgrupado As DetalleClienteAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, DetalleClienteAgrupado.pstrUsuarioConexion, DetalleClienteAgrupado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DetalleClienteAgrupado.InfoSesion = DemeInfoSesion(DetalleClienteAgrupado.pstrUsuarioConexion, "InsertDetalleClienteAgrupado")
            Me.DataContext.DetalleClienteAgrupador.InsertOnSubmit(DetalleClienteAgrupado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDetalleClienteAgrupado")
        End Try
    End Sub

    Public Sub UpdateDetalleClienteAgrupado(ByVal currentDetalleClienteAgrupado As DetalleClienteAgrupado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentDetalleClienteAgrupado.pstrUsuarioConexion, currentDetalleClienteAgrupado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDetalleClienteAgrupado.InfoSesion = DemeInfoSesion(currentDetalleClienteAgrupado.pstrUsuarioConexion, "UpdateDetalleClienteAgrupado")
            Me.DataContext.DetalleClienteAgrupador.Attach(currentDetalleClienteAgrupado, Me.ChangeSet.GetOriginal(currentDetalleClienteAgrupado))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDetalleClienteAgrupado")
        End Try
    End Sub

    Public Sub DeleteDetalleClienteAgrupado(ByVal DetalleClienteAgrupado As DetalleClienteAgrupado)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DetalleClienteAgrupado.pstrUsuarioConexion, DetalleClienteAgrupado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_DetalleClienteAgrupador_Eliminar( pId,  pNombre,  pDireccionEnvio,  pidReceptor,  pIDSucCliente, DemeInfoSesion(pstrUsuario, "DeleteDetalleClienteAgrupado"),0).ToList
            DetalleClienteAgrupado.InfoSesion = DemeInfoSesion(DetalleClienteAgrupado.pstrUsuarioConexion, "DeleteDetalleClienteAgrupado")
            Me.DataContext.DetalleClienteAgrupador.Attach(DetalleClienteAgrupado)
            Me.DataContext.DetalleClienteAgrupador.DeleteOnSubmit(DetalleClienteAgrupado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDetalleClienteAgrupado")
        End Try
    End Sub
#End Region

#Region "ClasificacionesCiiu"

    Public Function ClasificacionesCiiuFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClasificacionesCii)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClasificacionesCiiu_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ClasificacionesCiiuFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClasificacionesCiiuFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ClasificacionesCiiuConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pTipo As Integer, ByVal pIDPerteneceA As Integer, ByVal pIDClasificacionCiiu As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClasificacionesCii)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClasificacionesCiiu_Consultar(pID, pNombre, pTipo, pIDPerteneceA, DemeInfoSesion(pstrUsuario, "BuscarClasificacionesCiiu"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClasificacionesCiiu")
            Return Nothing
        End Try
    End Function

    Public Function TraerClasificacionesCiiPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ClasificacionesCii
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ClasificacionesCii
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.ID = 
            'e.Nombre = 
            'e.Tipo = Nothing
            'e.IDPerteneceA = Nothing
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDClasificacionCiiu = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClasificacionesCiiPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertClasificacionesCii(ByVal ClasificacionesCii As ClasificacionesCii)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClasificacionesCii.pstrUsuarioConexion, ClasificacionesCii.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClasificacionesCii.InfoSesion = DemeInfoSesion(ClasificacionesCii.pstrUsuarioConexion, "InsertClasificacionesCii")
            Me.DataContext.ClasificacionesCiiu.InsertOnSubmit(ClasificacionesCii)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClasificacionesCii")
        End Try
    End Sub

    Public Sub UpdateClasificacionesCii(ByVal currentClasificacionesCii As ClasificacionesCii)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClasificacionesCii.pstrUsuarioConexion, currentClasificacionesCii.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClasificacionesCii.InfoSesion = DemeInfoSesion(currentClasificacionesCii.pstrUsuarioConexion, "UpdateClasificacionesCii")
            Me.DataContext.ClasificacionesCiiu.Attach(currentClasificacionesCii, Me.ChangeSet.GetOriginal(currentClasificacionesCii))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClasificacionesCii")
        End Try
    End Sub

    Public Sub DeleteClasificacionesCii(ByVal ClasificacionesCii As ClasificacionesCii)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClasificacionesCii.pstrUsuarioConexion, ClasificacionesCii.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ClasificacionesCiiu_Eliminar( pID,  pNombre,  pTipo,  pIDPerteneceA,  pIDClasificacionCiiu, DemeInfoSesion(pstrUsuario, "DeleteClasificacionesCii"),0).ToList
            ClasificacionesCii.InfoSesion = DemeInfoSesion(ClasificacionesCii.pstrUsuarioConexion, "DeleteClasificacionesCii")
            Me.DataContext.ClasificacionesCiiu.Attach(ClasificacionesCii)
            Me.DataContext.ClasificacionesCiiu.DeleteOnSubmit(ClasificacionesCii)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClasificacionesCii")
        End Try
    End Sub

    Public Function ClasificacionesCiiuConsultarCombo(ByVal pTipo As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ClasificacionesCIIUPorTipo_Consultar(pTipo).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCombo")
            Return Nothing
        End Try
    End Function
#End Region

#Region "BolsaCostos"

    Public Function BolsaCostosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BolsaCosto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BolsaCostos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "BolsaCostosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BolsaCostosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function BolsaCostosConsultar(ByVal pId As Integer, ByVal pstrNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BolsaCosto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BolsaCostos_Consultar(pId, pstrNombre, DemeInfoSesion(pstrUsuario, "BuscarBolsaCostos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBolsaCostos")
            Return Nothing
        End Try
    End Function

    Public Function TraerBolsaCostoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As BolsaCosto
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New BolsaCosto
            'e.IDBolsaCostos = 
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Id = 
            'e.Descripcion = 
            'e.PorcentajeCosto = 
            'e.CostoPesos = 
            'e.Actualizado = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.Actualizacion = Date.Now
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerBolsaCostoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertBolsaCosto(ByVal BolsaCosto As BolsaCosto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, BolsaCosto.pstrUsuarioConexion, BolsaCosto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            BolsaCosto.InfoSesion = DemeInfoSesion(BolsaCosto.pstrUsuarioConexion, "InsertBolsaCosto")
            Me.DataContext.BolsaCostos.InsertOnSubmit(BolsaCosto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertBolsaCosto")
        End Try
    End Sub

    Public Sub UpdateBolsaCosto(ByVal currentBolsaCosto As BolsaCosto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentBolsaCosto.pstrUsuarioConexion, currentBolsaCosto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentBolsaCosto.InfoSesion = DemeInfoSesion(currentBolsaCosto.pstrUsuarioConexion, "UpdateBolsaCosto")
            Me.DataContext.BolsaCostos.Attach(currentBolsaCosto, Me.ChangeSet.GetOriginal(currentBolsaCosto))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateBolsaCosto")
        End Try
    End Sub

    Public Sub DeleteBolsaCosto(ByVal BolsaCosto As BolsaCosto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,BolsaCosto.pstrUsuarioConexion, BolsaCosto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_BolsaCostos_Eliminar( pId, DemeInfoSesion(pstrUsuario, "DeleteBolsaCosto"),0).ToList
            BolsaCosto.InfoSesion = DemeInfoSesion(BolsaCosto.pstrUsuarioConexion, "DeleteBolsaCosto")
            Me.DataContext.BolsaCostos.Attach(BolsaCosto)
            Me.DataContext.BolsaCostos.DeleteOnSubmit(BolsaCosto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteBolsaCosto")
        End Try
    End Sub
#End Region
#End Region

#Region "SEBASTIAN LONDOÑO B."

#Region "BloqueoSaldoClientes"

    Public Function BloqueoSaldoClientesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BloqueoSaldoCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BloqueoSaldoClientes_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "BloqueoSaldoClientesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BloqueoSaldoClientesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function BloqueoSaldoClientesConsultar(ByVal pIDComitente As String, ByVal pNaturaleza As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BloqueoSaldoCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BloqueoSaldoClientes_Consultar(pIDComitente, pNaturaleza, DemeInfoSesion(pstrUsuario, "BuscarBloqueoSaldoClientes"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBloqueoSaldoClientes")
            Return Nothing
        End Try
    End Function

    Public Function TraerBloqueoSaldoClientePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As BloqueoSaldoCliente
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New BloqueoSaldoCliente
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.IdRegistro = 
            'e.IDComitente = 
            'e.MotivoBloqueoSaldo = 
            'e.ValorBloqueado = 
            'e.Naturaleza = 
            e.FechaBloqueo = Date.Now
            'e.DetalleBloqueo = 
            e.Actualizacion = Date.Now
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerBloqueoSaldoClientePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertBloqueoSaldoCliente(ByVal BloqueoSaldoCliente As BloqueoSaldoCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, BloqueoSaldoCliente.pstrUsuarioConexion, BloqueoSaldoCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            BloqueoSaldoCliente.InfoSesion = DemeInfoSesion(BloqueoSaldoCliente.pstrUsuarioConexion, "InsertBloqueoSaldoCliente")
            Me.DataContext.BloqueoSaldoClientes.InsertOnSubmit(BloqueoSaldoCliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertBloqueoSaldoCliente")
        End Try
    End Sub

    Public Sub UpdateBloqueoSaldoCliente(ByVal currentBloqueoSaldoCliente As BloqueoSaldoCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentBloqueoSaldoCliente.pstrUsuarioConexion, currentBloqueoSaldoCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentBloqueoSaldoCliente.InfoSesion = DemeInfoSesion(currentBloqueoSaldoCliente.pstrUsuarioConexion, "UpdateBloqueoSaldoCliente")
            Me.DataContext.BloqueoSaldoClientes.Attach(currentBloqueoSaldoCliente, Me.ChangeSet.GetOriginal(currentBloqueoSaldoCliente))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateBloqueoSaldoCliente")
        End Try
    End Sub

    Public Sub DeleteBloqueoSaldoCliente(ByVal BloqueoSaldoCliente As BloqueoSaldoCliente)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,BloqueoSaldoCliente.pstrUsuarioConexion, BloqueoSaldoCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_BloqueoSaldoClientes_Eliminar( pIDComitente,  pNaturaleza, DemeInfoSesion(pstrUsuario, "DeleteBloqueoSaldoCliente"),0).ToList
            BloqueoSaldoCliente.InfoSesion = DemeInfoSesion(BloqueoSaldoCliente.pstrUsuarioConexion, "DeleteBloqueoSaldoCliente")
            Me.DataContext.BloqueoSaldoClientes.Attach(BloqueoSaldoCliente)
            Me.DataContext.BloqueoSaldoClientes.DeleteOnSubmit(BloqueoSaldoCliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteBloqueoSaldoCliente")
        End Try
    End Sub
    Public Function ValidarSaldoBloqueo(ByVal pIDComitente As String, ByVal pintIDEncargo As Integer, ByVal pdtmFecha As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Decimal
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim p1 As Nullable(Of Decimal)
            Dim ret = Me.DataContext.usp_ConsultarSaldoBloqueoRecursosclientes_OyDNet(p1, pIDComitente, pintIDEncargo, pdtmFecha, DemeInfoSesion(pstrUsuario, "BuscarBloqueoSaldoClientes"), 0)
            Return CDec(p1)
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarSaldoBloqueo")
            Return Nothing
        End Try
    End Function
    Public Function ValidarSaldoEncargo(ByVal pintIDEncargo As System.Nullable(Of Integer), ByVal pdtmFecha As System.Nullable(Of DateTime), ByVal pstrUsuario As String) As Double
        Try
            Dim pdblSaldoEncargo As Nullable(Of Double) = 0
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BloqueoSaldoClientes_ObtenerSaldoEncargo(pdblSaldoEncargo, pintIDEncargo, pdtmFecha, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarSaldoEncargo"), 0)
            Return CDbl(pdblSaldoEncargo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarSaldoEncargo")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Destino Inversion"

    Public Function DestinoInversionFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DestinoInversion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DestinoInversion_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "DestinoInversionFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DestinoInversionFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DestinoInversionConsultar(ByVal lngIDDestino? As Integer, ByVal StrNombreDestino As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DestinoInversion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DestinoInversion_Consultar(lngIDDestino, StrNombreDestino, DemeInfoSesion(pstrUsuario, "DestinoInversionConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DestinoInversionConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerDestinoInversionPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DestinoInversion
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DestinoInversion
            'e.IDClasesEspecies = -1
            e.IDDestinoInversion = -1
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClasesEspeciePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDestinoInversion(ByVal DestinoInversion As DestinoInversion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, DestinoInversion.pstrUsuarioConexion, DestinoInversion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DestinoInversion.InfoSesion = DemeInfoSesion(DestinoInversion.pstrUsuarioConexion, "InsertDestinoInversion")
            Me.DataContext.DestinoInversion.InsertOnSubmit(DestinoInversion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDestinoInversion")
        End Try
    End Sub

    Public Sub UpdateDestinoInversion(ByVal currentDestinoInversion As DestinoInversion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentDestinoInversion.pstrUsuarioConexion, currentDestinoInversion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentDestinoInversion.InfoSesion = DemeInfoSesion(currentDestinoInversion.pstrUsuarioConexion, "UpdateDestinoInversion")
            Me.DataContext.DestinoInversion.Attach(currentDestinoInversion, Me.ChangeSet.GetOriginal(currentDestinoInversion))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDestinoInversion")
        End Try
    End Sub

    Public Sub DeleteDestinoInversion(ByVal DestinoInversion As DestinoInversion)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DestinoInversion.pstrUsuarioConexion, DestinoInversion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ClasesEspecies_Eliminar( pIDClaseEspecie,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteClasesEspecie"),0).ToList
            DestinoInversion.InfoSesion = DemeInfoSesion(DestinoInversion.pstrUsuarioConexion, "DeleteDestinoInversion")
            Me.DataContext.DestinoInversion.Attach(DestinoInversion)
            Me.DataContext.DestinoInversion.DeleteOnSubmit(DestinoInversion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDestinoInversion")
        End Try
    End Sub


#End Region

#Region "ComisionesBroker"

    Public Function ComisionesBrokerFiltrar(ByVal pstrFiltro As String, ByVal pstrFuente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ComisionesBroker)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ComisionesBroker_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrFuente, DemeInfoSesion(pstrUsuario, "ComisionesBrokerFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ComisionesBrokerFiltrar")
            Return Nothing
        End Try
    End Function


    Public Sub InsertComisionesBroker(ByVal ComisionesBroker As ComisionesBroker)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ComisionesBroker.pstrUsuarioConexion, ComisionesBroker.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ComisionesBroker.InfoSesion = DemeInfoSesion(ComisionesBroker.pstrUsuarioConexion, "ComisionesBroker")
            Me.DataContext.ComisionesBroker.InsertOnSubmit(ComisionesBroker)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertComisionesBroker")
        End Try
    End Sub

    Public Sub UpdateComisionesBroker(ByVal currentComisionesBroker As ComisionesBroker)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentComisionesBroker.pstrUsuarioConexion, currentComisionesBroker.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentComisionesBroker.InfoSesion = DemeInfoSesion(currentComisionesBroker.pstrUsuarioConexion, "ComisionesBroker")
            Me.DataContext.ComisionesBroker.Attach(currentComisionesBroker, Me.ChangeSet.GetOriginal(currentComisionesBroker))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateComisionesBroker")
        End Try
    End Sub

#End Region

#End Region

#Region "SANTIAGO VERGARA"

#Region "ComisionEspecies"

    Public Function ComisionEspeciesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ComisionEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ComisionEspecies_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ComisionEspeciesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ComisionEspeciesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ComisionEspeciesConsultar(ByVal pIDEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ComisionEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ComisionEspecies_Consultar(pIDEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarComisionEspecies"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarComisionEspecies")
            Return Nothing
        End Try
    End Function

    Public Function TraerComisionEspeciePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ComisionEspecie
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ComisionEspecie
            'e.ID = 
            'e.IDEspecie = 
            e.Comision = 0
            e.PorcentajeComision = 0
            'e.Usuario = 
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerComisionEspeciePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertComisionEspecie(ByVal ComisionEspecie As ComisionEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ComisionEspecie.pstrUsuarioConexion, ComisionEspecie.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ComisionEspecie.InfoSesion = DemeInfoSesion(ComisionEspecie.pstrUsuarioConexion, "InsertComisionEspecie")
            Me.DataContext.ComisionEspecies.InsertOnSubmit(ComisionEspecie)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertComisionEspecie")
        End Try
    End Sub

    Public Sub UpdateComisionEspecie(ByVal currentComisionEspecie As ComisionEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentComisionEspecie.pstrUsuarioConexion, currentComisionEspecie.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentComisionEspecie.InfoSesion = DemeInfoSesion(currentComisionEspecie.pstrUsuarioConexion, "UpdateComisionEspecie")
            currentComisionEspecie.Usuario = HttpContext.Current.User.Identity.Name
            Me.DataContext.ComisionEspecies.Attach(currentComisionEspecie, Me.ChangeSet.GetOriginal(currentComisionEspecie))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateComisionEspecie")
        End Try
    End Sub

    Public Sub DeleteComisionEspecie(ByVal ComisionEspecie As ComisionEspecie)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ComisionEspecie.pstrUsuarioConexion, ComisionEspecie.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ComisionEspecies_Eliminar( pIDEspecie, DemeInfoSesion(pstrUsuario, "DeleteComisionEspecie"),0).ToList
            ComisionEspecie.InfoSesion = DemeInfoSesion(ComisionEspecie.pstrUsuarioConexion, "DeleteComisionEspecie")
            Me.DataContext.ComisionEspecies.Attach(ComisionEspecie)
            Me.DataContext.ComisionEspecies.DeleteOnSubmit(ComisionEspecie)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteComisionEspecie")
        End Try
    End Sub
#End Region

#End Region

#Region "COMUNES"
    Public Function GetAuditorias(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of Auditoria)
        Dim objResultado As IQueryable(Of Auditoria) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objResultado = Me.DataContext.Auditorias
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetAuditorias")
        End Try
        Return objResultado
    End Function

    Public Function CargarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.spA2utils_CargarCombos("")
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarCombose")
            Return Nothing
        End Try
    End Function

#Region "Validar Eliminar Registro"

    Public Function ValidarEliminarRegistro(ByVal pstrTablas As String, ByVal pstrCampos As String, ByVal pstrValorCampo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidacionEliminarRegistro)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
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
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ValidarDuplicidadRegistro(pstrTablas, pstrCampos, pstrValorCampo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarDuplicidadRegistro"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarDuplicidadRegistro")
            Return Nothing
        End Try
    End Function

#End Region

#End Region

#Region "ClienteGruposEconómicos"

    Public Function GruposEconomicosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of GrupoEconomicos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesGruposEconomicos_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "GruposEconomicosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GruposEconomicosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function GruposEconomicosConsultar(pidGrupoEconomico As Integer, ByVal pNombreGrupo As String, ByVal idComitenteLider As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of GrupoEconomicos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesGruposEconomicos_Consultar(pidGrupoEconomico, pNombreGrupo, idComitenteLider, DemeInfoSesion(pstrUsuario, "GruposEconomicosConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GruposEconomicosConsultar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertGruposEconomicos(ByVal currentGrupoEconomico As GrupoEconomicos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentGrupoEconomico.pstrUsuarioConexion, currentGrupoEconomico.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentGrupoEconomico.InfoSesion = DemeInfoSesion(currentGrupoEconomico.pstrUsuarioConexion, "InsertGruposEconomicos")
            Me.DataContext.GrupoEconomicos.InsertOnSubmit(currentGrupoEconomico)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertGruposEconomicos")
        End Try
    End Sub

    Public Sub UpdateGruposEconomicos(ByVal currentGrupoEconomico As GrupoEconomicos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentGrupoEconomico.pstrUsuarioConexion, currentGrupoEconomico.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentGrupoEconomico.InfoSesion = DemeInfoSesion(currentGrupoEconomico.pstrUsuarioConexion, "UpdateGruposEconomicos")
            Me.DataContext.GrupoEconomicos.Attach(currentGrupoEconomico, Me.ChangeSet.GetOriginal(currentGrupoEconomico))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateGruposEconomicos")
        End Try
    End Sub

    Public Sub DeleteGruposEconomicos(ByVal GrupoEconomico As GrupoEconomicos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,GrupoEconomico.pstrUsuarioConexion, GrupoEconomico.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ClienteAgrupador_Eliminar( pNroDocumento,  pNombreAgrupador, DemeInfoSesion(pstrUsuario, "DeleteClienteAgrupado"),0).ToList
            GrupoEconomico.InfoSesion = DemeInfoSesion(GrupoEconomico.pstrUsuarioConexion, "DeleteGruposEconomicos")
            Me.DataContext.GrupoEconomicos.Attach(GrupoEconomico)
            Me.DataContext.GrupoEconomicos.DeleteOnSubmit(GrupoEconomico)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteGruposEconomicos")
        End Try
    End Sub
#End Region

#Region "DetalleClienteGruposEconómicos"

    Public Function DetalleGruposEconomicosConsultar(ByVal plngIDGrupoEconommico As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleGrupoEconomico)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DetalleGruposEconomicos_Consultar(plngIDGrupoEconommico, pstrUsuario, DemeInfoSesion(pstrUsuario, "DetalleGruposEconomicosConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleGruposEconomicosConsultar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDetalleGruposEconomicos(ByVal currentDetalleGrupoEconomico As DetalleGrupoEconomico)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDetalleGruposEconomicos")
        End Try
    End Sub

    Public Sub UpdateDetalleGruposEconomicos(ByVal DetalleGrupoEconomico As DetalleGrupoEconomico)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateGruposEconomicos")
        End Try
    End Sub

    Public Sub DeleteDetalleGruposEconomicos(ByVal DetalleGrupoEconomico As DetalleGrupoEconomico)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDetalleGruposEconomicos")
        End Try
    End Sub
#End Region

#Region "Clasificacion Riesgo"
    'Jorge Andres Bedoya 2014/01/23

    Public Function ClasificacionRiesgoFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClasificacionRiesgo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClasificacionRiesgos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ClasificacionRiesgoFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClasificacionRiesgoFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ClasificacionRiesgoConsultar(ByVal pintIdClasificacionRiesgo As Integer, pintIdTipoClasificacionRiesgo As Integer, ByVal pstrPrefijo As String, ByVal pstrDetalle As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClasificacionRiesgo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClasificacionRiesgos_Consultar(pintIdClasificacionRiesgo, pintIdTipoClasificacionRiesgo, pstrPrefijo, pstrDetalle, DemeInfoSesion(pstrUsuario, "BuscarClasificacionRiesgo"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClasificacionRiesgo")
            Return Nothing
        End Try
    End Function

    Public Function TraerClasificacionRiesgoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ClasificacionRiesgo
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ClasificacionRiesgo
            e.IdClasificacionRiesgo = 0
            e.IdTipoClasificacionRiesgo = 0
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClasificacionRiesgoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertClasificacionRiesgo(ByVal ClasificacionRiesgo As ClasificacionRiesgo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClasificacionRiesgo.pstrUsuarioConexion, ClasificacionRiesgo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClasificacionRiesgo.InfoSesion = DemeInfoSesion(ClasificacionRiesgo.pstrUsuarioConexion, "InsertClasificacionRiesgo")
            Me.DataContext.ClasificacionRiesgos.InsertOnSubmit(ClasificacionRiesgo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClasificacionRiesgo")
        End Try
    End Sub

    Public Sub UpdateClasificacionRiesgo(ByVal currentClasificacionRiesgo As ClasificacionRiesgo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClasificacionRiesgo.pstrUsuarioConexion, currentClasificacionRiesgo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClasificacionRiesgo.InfoSesion = DemeInfoSesion(currentClasificacionRiesgo.pstrUsuarioConexion, "UpdateClasificacionRiesgo")
            Me.DataContext.ClasificacionRiesgos.Attach(currentClasificacionRiesgo, Me.ChangeSet.GetOriginal(currentClasificacionRiesgo))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClasificacionRiesgo")
        End Try
    End Sub

    Public Sub DeleteClasificacionRiesgo(ByVal ClasificacionRiesgo As ClasificacionRiesgo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClasificacionRiesgo.pstrUsuarioConexion, ClasificacionRiesgo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Ciudades_Eliminar( pIDCodigo,  pNombre,  pCodigoDANE, DemeInfoSesion(pstrUsuario, "DeleteCiudade"),0).ToList
            ClasificacionRiesgo.InfoSesion = DemeInfoSesion(ClasificacionRiesgo.pstrUsuarioConexion, "DeleteClasificacionRiesgo")
            Me.DataContext.ClasificacionRiesgos.Attach(ClasificacionRiesgo)
            Me.DataContext.ClasificacionRiesgos.DeleteOnSubmit(ClasificacionRiesgo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClasificacionRiesgo")
        End Try
    End Sub

    Public Function EliminarClasificacionRiesgo(ByVal pintIdClasificacionRiesgo As Integer, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClasificacionRiesgos_Eliminar(pintIdClasificacionRiesgo, mensaje, DemeInfoSesion(pstrUsuario, "EliminarClasificacionRiesgo"))
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminaRClasificacionRiesgo")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Clasificacion Riesgo tipo cliente"
    'Jorge Andres Bedoya 2014/03/07

    Public Function ClasificacionRiesgoTipoClienteFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClasificacionRiesgoTipoCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_MaestrosClasificacionTipoCliente_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ClasificacionRiesgoTipoClienteFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClasificacionRiesgoTipoClienteFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ClasificacionRiesgoTipoClienteConsultar(ByVal pintIdCodigo As Integer, ByVal pstrClasificacionTipoCliente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClasificacionRiesgoTipoCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_MaestrosClasificacionTipoCliente_Consultar(pintIdCodigo, pstrClasificacionTipoCliente, DemeInfoSesion(pstrUsuario, "BuscarClasificacionRiesgo"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClasificacionRiesgoTipoCliente")
            Return Nothing
        End Try
    End Function

    Public Function TraerClasificacionRiesgoTipoClientePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ClasificacionRiesgoTipoCliente
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ClasificacionRiesgoTipoCliente
            e.Codigo = 0
            e.ClasificacionTipoCliente = ""
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClasificacionRiesgoTipoClientePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertClasificacionRiesgoTipoCliente(ByVal ClasificacionRiesgoTipoCliente As ClasificacionRiesgoTipoCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClasificacionRiesgoTipoCliente.pstrUsuarioConexion, ClasificacionRiesgoTipoCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClasificacionRiesgoTipoCliente.InfoSesion = DemeInfoSesion(ClasificacionRiesgoTipoCliente.pstrUsuarioConexion, "InsertClasificacionRiesgoTipoCliente")
            Me.DataContext.ClasificacionTipoCliente.InsertOnSubmit(ClasificacionRiesgoTipoCliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClasificacionRiesgoTipoCliente")
        End Try
    End Sub

    Public Sub UpdateClasificacionRiesgoTipoCliente(ByVal currentClasificacionRiesgoTipoCliente As ClasificacionRiesgoTipoCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClasificacionRiesgoTipoCliente.pstrUsuarioConexion, currentClasificacionRiesgoTipoCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClasificacionRiesgoTipoCliente.InfoSesion = DemeInfoSesion(currentClasificacionRiesgoTipoCliente.pstrUsuarioConexion, "UpdateClasificacionRiesgoTipoCliente")
            Me.DataContext.ClasificacionTipoCliente.Attach(currentClasificacionRiesgoTipoCliente, Me.ChangeSet.GetOriginal(currentClasificacionRiesgoTipoCliente))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClasificacionRiesgoTipoCliente")
        End Try
    End Sub

    Public Sub DeleteClasificacionRiesgoTipoCliente(ByVal ClasificacionRiesgoTipoCliente As ClasificacionRiesgoTipoCliente)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClasificacionRiesgoTipoCliente.pstrUsuarioConexion, ClasificacionRiesgoTipoCliente.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Ciudades_Eliminar( pIDCodigo,  pNombre,  pCodigoDANE, DemeInfoSesion(pstrUsuario, "DeleteCiudade"),0).ToList
            ClasificacionRiesgoTipoCliente.InfoSesion = DemeInfoSesion(ClasificacionRiesgoTipoCliente.pstrUsuarioConexion, "DeleteClasificacionRiesgoTipoCliente")
            Me.DataContext.ClasificacionTipoCliente.Attach(ClasificacionRiesgoTipoCliente)
            Me.DataContext.ClasificacionTipoCliente.DeleteOnSubmit(ClasificacionRiesgoTipoCliente)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClasificacionRiesgoTipoCliente")
        End Try
    End Sub

    Public Function EliminarClasificacionRiesgoTipoCliente(ByVal pintIdCodigo As Integer, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_MaestrosClasificacionTipoCliente_Eliminar(pintIdCodigo, mensaje, DemeInfoSesion(pstrUsuario, "EliminarClasificacionRiesgoTipoCliente"))
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarClasificacionRiesgoTipoCliente")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Carteras colectivas clientes GMF"
    'Jorge Andres Bedoya 2014/10/21

    Public Function CarterasColectivasClientesGMFFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CarterasColectivasClientesGMF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_MaestrosCarterasColectivasGMF_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "CarterasColectivasClientesGMFFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CarterasColectivasClientesGMFFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CarterasColectivasClientesGMFConsultar(ByVal pstrTipoIdentificacion As String, ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CarterasColectivasClientesGMF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_MaestrosCarterasColectivasGMF_Consultar(pstrTipoIdentificacion, pstrNroDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "CarterasColectivasClientesGMFConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CarterasColectivasClientesGMFConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerCarterasColectivasClientesGMFPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CarterasColectivasClientesGMF
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New CarterasColectivasClientesGMF
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCarterasColectivasClientesGMFPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCarterasColectivasClientesGMF(ByVal CarterasColectivasClientesGMF As CarterasColectivasClientesGMF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, CarterasColectivasClientesGMF.pstrUsuarioConexion, CarterasColectivasClientesGMF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CarterasColectivasClientesGMF.InfoSesion = DemeInfoSesion(CarterasColectivasClientesGMF.pstrUsuarioConexion, "InsertCarterasColectivasClientesGMF")
            Me.DataContext.CarterasColectivasClientesGMF.InsertOnSubmit(CarterasColectivasClientesGMF)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCarterasColectivasClientesGMF")
        End Try
    End Sub

    Public Sub UpdateCarterasColectivasClientesGMF(ByVal currentCarterasColectivasClientesGMF As CarterasColectivasClientesGMF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCarterasColectivasClientesGMF.pstrUsuarioConexion, currentCarterasColectivasClientesGMF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCarterasColectivasClientesGMF.InfoSesion = DemeInfoSesion(currentCarterasColectivasClientesGMF.pstrUsuarioConexion, "UpdateCarterasColectivasClientesGMF")
            Me.DataContext.CarterasColectivasClientesGMF.Attach(currentCarterasColectivasClientesGMF, Me.ChangeSet.GetOriginal(currentCarterasColectivasClientesGMF))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCarterasColectivasClientesGMF")
        End Try
    End Sub

    Public Sub DeleteCarterasColectivasClientesGMF(ByVal CarterasColectivasClientesGMF As CarterasColectivasClientesGMF)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CarterasColectivasClientesGMF.pstrUsuarioConexion, CarterasColectivasClientesGMF.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CarterasColectivasClientesGMF.InfoSesion = DemeInfoSesion(CarterasColectivasClientesGMF.pstrUsuarioConexion, "DeleteCarterasColectivasClientesGMF")
            Me.DataContext.CarterasColectivasClientesGMF.Attach(CarterasColectivasClientesGMF)
            Me.DataContext.CarterasColectivasClientesGMF.DeleteOnSubmit(CarterasColectivasClientesGMF)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCarterasColectivasClientesGMF")
        End Try
    End Sub

#End Region

#Region "JULIAN DAVID RINCON HENAO"

#Region "PerfilesRiesgo"

    Public Function PerfilesRiesgoFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PerfilesRiesgo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_PerfilesRiesgo_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "PerfilesRiesgoFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PerfilesRiesgoFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function PerfilesRiesgoConsultar(ByVal pintIDPerfilRiesgo As Integer, ByVal plngIDTipoPerfil As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PerfilesRiesgo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_PerfilesRiesgo_Consultar(pintIDPerfilRiesgo, plngIDTipoPerfil, DemeInfoSesion(pstrUsuario, "BuscarPerfilesRiesgo"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarPerfilesRiesgo")
            Return Nothing
        End Try
    End Function

    Public Function TraerPerfilesRiesgoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PerfilesRiesgo
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New PerfilesRiesgo
            e.PerfilRiesgo = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerPerfilesRiesgoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertPerfilesRiesgo(ByVal objPerfil As PerfilesRiesgo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objPerfil.pstrUsuarioConexion, objPerfil.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objPerfil.Infosesion = DemeInfoSesion(objPerfil.pstrUsuarioConexion, "InsertPerfilesRiesgo")
            Me.DataContext.PerfilesRiesgos.InsertOnSubmit(objPerfil)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPerfilesRiesgo")
        End Try
    End Sub

    Public Sub UpdatePerfilesRiesgo(ByVal objPerfil As PerfilesRiesgo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objPerfil.pstrUsuarioConexion, objPerfil.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objPerfil.Infosesion = DemeInfoSesion(objPerfil.pstrUsuarioConexion, "UpdatePerfilesRiesgo")
            Me.DataContext.PerfilesRiesgos.Attach(objPerfil, Me.ChangeSet.GetOriginal(objPerfil))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePerfilesRiesgo")
        End Try
    End Sub

    Public Sub DeletePerfilesRiesgo(ByVal PerfilesRiesgo As PerfilesRiesgo)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,PerfilesRiesgo.pstrUsuarioConexion, PerfilesRiesgo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            PerfilesRiesgo.Infosesion = DemeInfoSesion(PerfilesRiesgo.pstrUsuarioConexion, "DeletePerfilesRiesgo")
            Me.DataContext.PerfilesRiesgos.Attach(PerfilesRiesgo)
            Me.DataContext.PerfilesRiesgos.DeleteOnSubmit(PerfilesRiesgo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePerfilesRiesgo")
        End Try
    End Sub
#End Region

#End Region

#Region "DECEVAL"
    'Carlos Andres Toro 2015/02/13
    Public Function EmpleadoSistemaConsultar(ByVal pintIDEmpleado As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EmpleadoSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_EmpleadosSistemas_Consultar(pintIDEmpleado, pstrUsuario, DemeInfoSesion(pstrUsuario, "EmpleadoSistemaConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EmpleadoSistemaConsultar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEmpleadoSistema(ByVal objEmpleadoSistema As EmpleadoSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objEmpleadoSistema.pstrUsuarioConexion, objEmpleadoSistema.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objEmpleadoSistema.InfoSesion = DemeInfoSesion(objEmpleadoSistema.pstrUsuarioConexion, "InsertEmpleadoSistema")
            Me.DataContext.EmpleadosSistemas.InsertOnSubmit(objEmpleadoSistema)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEmpleadoSistema")
        End Try
    End Sub

    Public Sub UpdateEmpleadoSistema(ByVal objEmpleadoSistema As EmpleadoSistema)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objEmpleadoSistema.pstrUsuarioConexion, objEmpleadoSistema.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objEmpleadoSistema.InfoSesion = DemeInfoSesion(objEmpleadoSistema.pstrUsuarioConexion, "UpdateEmpleadoSistema")
            Me.DataContext.EmpleadosSistemas.Attach(objEmpleadoSistema, Me.ChangeSet.GetOriginal(objEmpleadoSistema))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEmpleadoSistema")
        End Try
    End Sub

    Public Sub DeleteEmpleadoSistema(ByVal objEmpleadoSistema As EmpleadoSistema)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objEmpleadoSistema.pstrUsuarioConexion, objEmpleadoSistema.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objEmpleadoSistema.InfoSesion = DemeInfoSesion(objEmpleadoSistema.pstrUsuarioConexion, "DeleteEmpleadoSistema")
            Me.DataContext.EmpleadosSistemas.Attach(objEmpleadoSistema)
            Me.DataContext.EmpleadosSistemas.DeleteOnSubmit(objEmpleadoSistema)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEmpleadoSistema")
        End Try
    End Sub

#End Region

#Region "PermisoExportacionFormato"

#Region "Métodos modelo para activar funcionalidad RIA"
    ''' <summary>
    ''' Este metodo vacío se implementa para que el sistema permita seleccionar los checkbox desde el grid
    ''' </summary>
    ''' <param name="currentPermisosFormatosExportar">Objeto tipo PermisosFormatosExportar</param>
    ''' <history>
    ''' Creado por      : Jhonatan Arley Acevedo
    ''' Descripción     : Este metodo vacío se implementa para que el sistema permita seleccionar los checkbox desde el grid
    ''' Fecha           : Abril 13/2015
    ''' Pruebas CB      : Jhonatan Arley Acevedo - Abril 13/2015 - Resultado OK
    ''' </history>
    Public Sub UpdatePermisosFormatosExportar(ByVal currentPermisosFormatosExportar As PermisosFormatosExportar)

    End Sub
#End Region

    Public Function PermisoExportacionFormatoConsultar(pstrUsuario As String, plogActivo As Boolean, pstrSistema As String, ByVal pstrTipoBusqueda As String, ByVal pstrObjetoBusqueda As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of PermisosFormatosExportar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FormatosXUsuarios_Consultar(pstrUsuario, plogActivo, pstrSistema, pstrTipoBusqueda, pstrObjetoBusqueda).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PermisoExportacionFormatoConsultar")
            Return Nothing
        End Try
    End Function

    Public Function UsuariosPermisosConsultar(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of UsuariosPermisosFormatos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspA2Utils_ConsultarUsuarios().ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UsuariosPermisosFormatos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Este método permite actualizar los registros de los permisos asignados a un usuario
    ''' </summary>
    ''' <param name="pxmlPermisosAsignados">Parámetro tipo String que contiene todos los registros de permisos a asignar (o quitar)</param>
    ''' <param name="pstrUsuarioActualizacion">Parámetro tipo String</param>
    ''' <param name="plogAsignar">Parámetro tipo Booleano</param>
    ''' <param name="pstrInfosesion">Parámetro tipo String</param>
    ''' <param name="pintErrorPersonalizado">Parámetro tipo String</param>
    ''' <param name="pstrMaquina">Parámetro tipo String</param>
    ''' <param name="pstrSistema">Parámetro tipo String</param>
    ''' <returns>Retorna un entero indicando si el proceso es exitoso</returns>
    ''' <history>
    ''' Creado por      : Jhonatan Arley Acevedo
    ''' Descripción     : Este método permite actualizar los registros de los permisos asignados a un usuario
    ''' Fecha           : Abril 09/2015
    ''' Pruebas CB      : Jhonatan Arley Acevedo - Abril 09/2015 - Resultado OK
    ''' </history>
    <Query(HasSideEffects:=True)>
    Public Function PermisoExportacionFormatoActualizar(ByVal pxmlPermisosAsignados As String,
                                                        ByVal plogAsignar As Boolean,
                                                        ByVal pstrUsuario As String,
                                                        ByVal pstrUsuarioActualizacion As String,
                                                        ByVal pstrMaquina As String,
                                                        ByVal pstrSistema As String,
                                                        ByVal pstrTipoBusqueda As String,
                                                        ByVal pstrObjetoBusqueda As String, ByVal pstrInfoConexion As String) As List(Of PermisosFormatosExportar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioActualizacion, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_FormatosXUsuarios_Grabar(pxmlPermisosAsignados,
                                                                        plogAsignar,
                                                                        pstrUsuario,
                                                                        pstrUsuarioActualizacion,
                                                                        DemeInfoSesion(pstrUsuario, "PermisoExportacionFormatoActualizar"),
                                                                        0,
                                                                        "",
                                                                        pstrMaquina,
                                                                        pstrSistema,
                                                                        pstrTipoBusqueda,
                                                                        pstrObjetoBusqueda).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PermisoExportacionFormatoActualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Plantillas"

    Public Function PlantillasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Plantilla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Plantillas_Filtrar(Filtro, DemeInfoSesion(pstrUsuario, "PlantillasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PlantillasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function PlantillasConsultar(ByVal pintID As Integer, pstrCodigo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Plantilla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If pintID = 0 Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_Plantilla_Consultar(Nothing, pstrCodigo, DemeInfoSesion(pstrUsuario, "PlantillasConsultar"), 0).ToList
                Return ret
            Else
                Dim ret = Me.DataContext.uspOyDNet_Maestros_Plantilla_Consultar(pintID, pstrCodigo, DemeInfoSesion(pstrUsuario, "PlantillasConsultar"), 0).ToList
                Return ret
            End If
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PlantillasConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerPlantillaPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Plantilla
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Plantilla
            e.intID = -1
            Return (e)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerPlantillaPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertPlantilla(ByVal Plantilla As Plantilla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, Plantilla.pstrUsuarioConexion, Plantilla.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Plantilla.InfoSesion = DemeInfoSesion(Plantilla.pstrUsuarioConexion, "InsertPlantilla")
            Me.DataContext.Plantillas.InsertOnSubmit(Plantilla)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPlantilla")
        End Try
    End Sub

    Public Sub UpdatePlantilla(ByVal currentPlantilla As Plantilla)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentPlantilla.pstrUsuarioConexion, currentPlantilla.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentPlantilla.InfoSesion = DemeInfoSesion(currentPlantilla.pstrUsuarioConexion, "UpdatePlantilla")
            Me.DataContext.Plantillas.Attach(currentPlantilla, Me.ChangeSet.GetOriginal(currentPlantilla))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePlantilla")
        End Try
    End Sub

    Public Sub DeletePlantilla(ByVal Plantilla As Plantilla)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Plantilla.pstrUsuarioConexion, Plantilla.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Sucursales_Eliminar( pIDSucursal,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteSucursale"),0).ToList
            Plantilla.InfoSesion = DemeInfoSesion(Plantilla.pstrUsuarioConexion, "DeletePlantilla")
            Me.DataContext.Plantillas.Attach(Plantilla)
            Me.DataContext.Plantillas.DeleteOnSubmit(Plantilla)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePlantilla")
        End Try
    End Sub

    Public Function MetapalabrasConsultar(pstrSistema As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblMetapalabras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspListaMetapalabras(pstrSistema, DemeInfoSesion(pstrUsuario, "MetapalabrasConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MetapalabrasConsultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "PlantillaBanco"

    Public Function PlantillaBancoFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PlantillaBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Filtro = HttpUtility.UrlDecode(pstrFiltro)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_PlantillaBanco_Filtrar(Filtro, DemeInfoSesion(pstrUsuario, "PlantillaBancoFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PlantillaBancoFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function PlantillaBancoConsultar(ByVal pintID As Integer, pintIdBanco As Integer, pintIdPlantilla As Integer, pstrDescripcion As String, pstrExtension As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PlantillaBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Descripcion = HttpUtility.UrlDecode(pstrDescripcion)
            Dim Extension = HttpUtility.UrlDecode(pstrExtension)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_PlantillaBanco_Consultar(pintID, pintIdBanco, pintIdPlantilla, Descripcion, Extension, DemeInfoSesion(pstrUsuario, "PlantillaBancoConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PlantillaBancoConsultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerPlantillaBancoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PlantillaBanco
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New PlantillaBanco
            e.Id = -1
            Return (e)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerPlantillaBancoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertPlantillaBanco(ByVal PlantillaBancos As PlantillaBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, PlantillaBancos.pstrUsuarioConexion, PlantillaBancos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            PlantillaBancos.InfoSesion = DemeInfoSesion(PlantillaBancos.pstrUsuarioConexion, "InsertPlantillaBanco")
            Me.DataContext.PlantillaBancos.InsertOnSubmit(PlantillaBancos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPlantillaBanco")
        End Try
    End Sub

    Public Sub UpdatePlantillaBanco(ByVal currentPlantillaBanco As PlantillaBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentPlantillaBanco.pstrUsuarioConexion, currentPlantillaBanco.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentPlantillaBanco.InfoSesion = DemeInfoSesion(currentPlantillaBanco.pstrUsuarioConexion, "UpdatePlantillaBanco")
            Me.DataContext.PlantillaBancos.Attach(currentPlantillaBanco, Me.ChangeSet.GetOriginal(currentPlantillaBanco))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePlantillaBanco")
        End Try
    End Sub

    Public Sub DeletePlantillaBanco(ByVal PlantillaBanco As PlantillaBanco)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,PlantillaBanco.pstrUsuarioConexion, PlantillaBanco.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            PlantillaBanco.InfoSesion = DemeInfoSesion(PlantillaBanco.pstrUsuarioConexion, "DeletePlantillaBanco")
            Me.DataContext.PlantillaBancos.Attach(PlantillaBanco)
            Me.DataContext.PlantillaBancos.DeleteOnSubmit(PlantillaBanco)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePlantillaBanco")
        End Try
    End Sub

#End Region

#Region "Configuracion Receptores"
#Region "Metodos Asincronico"

    Private Function ConfiguracionReceptoresFiltrar(ByVal pstrfiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionReceptores)
        Try
            Dim ret = Me.DataContext.uspOyDNet_Receptores_Filtrar(pstrfiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarReceptores"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionReceptoresFiltrar")
            Return Nothing
        End Try
    End Function

    Private Function ConfiguracionReceptoresConsultar(ByVal pCodigo As String, ByVal pidsucural As Integer, ByVal pidmesa As Integer, ByVal Nombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionReceptores)
        Try
            Dim ret = Me.DataContext.uspOyDNet_Receptores_Consultar(pidsucural, pidmesa, Nombre, pCodigo, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarReceptores"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionReceptoresConsultar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConfiguracionReceptores(ByVal configuracionReceptores As ConfiguracionReceptores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, configuracionReceptores.pstrUsuarioConexion, configuracionReceptores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            configuracionReceptores.InfoSesion = DemeInfoSesion(configuracionReceptores.pstrUsuarioConexion, "InsertConfiguracionReceptores")
            Me.DataContext.ConfiguracionReceptores.InsertOnSubmit(configuracionReceptores)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConfiguracionReceptores")
        End Try
    End Sub

    Public Sub UpdateConfiguracionReceptores(ByVal currentConfiguracionReceptores As ConfiguracionReceptores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentConfiguracionReceptores.pstrUsuarioConexion, currentConfiguracionReceptores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConfiguracionReceptores.InfoSesion = DemeInfoSesion(currentConfiguracionReceptores.pstrUsuarioConexion, "UpdateConfiguracionReceptores")
            Me.DataContext.ConfiguracionReceptores.Attach(currentConfiguracionReceptores, Me.ChangeSet.GetOriginal(currentConfiguracionReceptores))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConfiguracionReceptores")
        End Try
    End Sub

    Public Sub DeleteConfiguracionReceptores(ByVal configuracionReceptores As ConfiguracionReceptores)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,configuracionReceptores.pstrUsuarioConexion, configuracionReceptores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim consultaexis = Me.DataContext.uspOyDNet_Maestros_Receptores_ConsultarEliminar(configuracionReceptores.Codigo, DemeInfoSesion(configuracionReceptores.pstrUsuarioConexion, "BuscarReceptores"), 0).ToList

            For Each a In consultaexis
                If a.Existe = 0 Then
                    configuracionReceptores.Accion = CType("R", Char?)
                ElseIf (a.Existe = 1) Then
                    configuracionReceptores.Accion = CType("A", Char?)
                End If
                Exit For
            Next

            configuracionReceptores.InfoSesion = DemeInfoSesion(configuracionReceptores.pstrUsuarioConexion, "DeleteReceptore")
            Me.DataContext.ConfiguracionReceptores.Attach(configuracionReceptores)
            Me.DataContext.ConfiguracionReceptores.DeleteOnSubmit(configuracionReceptores)

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConfiguracionReceptores")
        End Try
    End Sub
#End Region
#Region "Metodos Sincronicos"
    ''' <summary>
    ''' Permite realizar una espera al método ProcesarPortafolioAsync, implementando una metodología síncrona
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro tipo String</param>
    ''' <param name="plngIDComitente">Parámetro tipo String</param>
    ''' <param name="pstrTipoPortafolio">Parámetro tipo String</param>
    ''' <param name="pstrTipoProceso">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna un objeto Task de tipo String</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Permite realizar una espera al método ProcesarPortafolioAsync, implementando una metodología síncrona
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Public Function ConfiguracionReceptoresFiltrarSync(ByVal pstrfiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionReceptores)
        Dim objTask As Task(Of List(Of ConfiguracionReceptores)) = Me.ConfiguracionReceptoresFiltrarAsync(pstrfiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    ''' <summary>
    ''' Realiza un llamado al método ProcesarPortafolio
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro tipo String</param>
    ''' <param name="plngIDComitente">Parámetro tipo String</param>
    ''' <param name="pstrTipoPortafolio">Parámetro tipo String</param>
    ''' <param name="pstrTipoProceso">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna un objeto Task de tipo String</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Realiza un llamado al método ProcesarPortafolio
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private Function ConfiguracionReceptoresFiltrarAsync(ByVal pstrfiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionReceptores))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionReceptores)) = New TaskCompletionSource(Of List(Of ConfiguracionReceptores))()
        objTaskComplete.TrySetResult(ConfiguracionReceptoresFiltrar(pstrfiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Permite realizar una espera al método ProcesarPortafolioAsync, implementando una metodología síncrona
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro tipo String</param>
    ''' <param name="plngIDComitente">Parámetro tipo String</param>
    ''' <param name="pstrTipoPortafolio">Parámetro tipo String</param>
    ''' <param name="pstrTipoProceso">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna un objeto Task de tipo String</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Permite realizar una espera al método ProcesarPortafolioAsync, implementando una metodología síncrona
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Public Function ConfiguracionReceptoresConsultarSync(ByVal pCodigo As String, ByVal pidsucural As Integer, ByVal pidmesa As Integer, ByVal Nombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionReceptores)
        Dim objTask As Task(Of List(Of ConfiguracionReceptores)) = Me.ConfiguracionReceptoresConsultarAsync(pCodigo, pidsucural, pidmesa, Nombre, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    ''' <summary>
    ''' Realiza un llamado al método ProcesarPortafolio
    ''' </summary>
    ''' <param name="pdtmFechaValoracion">Parámetro tipo Nullable(Of Date)</param>
    ''' <param name="pstrIdEspecie">Parámetro tipo String</param>
    ''' <param name="plngIDComitente">Parámetro tipo String</param>
    ''' <param name="pstrTipoPortafolio">Parámetro tipo String</param>
    ''' <param name="pstrTipoProceso">Parámetro tipo String</param>
    ''' <param name="pstrUsuario">Parámetro tipo String</param>
    ''' <returns>Retorna un objeto Task de tipo String</returns>
    ''' <history>
    ''' Creado por      : Germán Arbey González Osorio
    ''' Descripción     : Realiza un llamado al método ProcesarPortafolio
    ''' Fecha           : Abril 11/2014
    ''' Pruebas CB      : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private Function ConfiguracionReceptoresConsultarAsync(ByVal pCodigo As String, ByVal pidsucural As Integer, ByVal pidmesa As Integer, ByVal Nombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionReceptores))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionReceptores)) = New TaskCompletionSource(Of List(Of ConfiguracionReceptores))()
        objTaskComplete.TrySetResult(ConfiguracionReceptoresConsultar(pCodigo, pidsucural, pidmesa, Nombre, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function
#End Region
#End Region

#Region "Paises"

    Public Function PaisesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Paise)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Paises_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "PaisesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PaisesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function PaisesConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Paise)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Paises_Consultar(pID, pNombre, DemeInfoSesion(pstrUsuario, "BuscarPaises"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarPaises")
            Return Nothing
        End Try
    End Function

    Public Function TraerPaisePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Paise
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Paise
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = -1
            'e.Nombre = 
            'e.Codigo_ISO = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDPais = 
            'e.CodigoDane = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerPaisePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertPaise(ByVal Paise As Paise)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPaise")
        End Try
    End Sub

    Public Sub UpdatePaise(ByVal currentPaise As Paise)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePaise")
        End Try
    End Sub

    Public Sub DeletePaise(ByVal Paise As Paise)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePaise")
        End Try
    End Sub

    Public Function Traer_Departamentos_Paise(ByVal pIDPais As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Departamento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pIDPais) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_DepartamentosPaise_Consultar(pIDPais, DemeInfoSesion(pstrUsuario, "Traer_Departamentos_Paise"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_Departamentos_Paise")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Departamentos"

    Public Function DepartamentosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Departamento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Departamentos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "DepartamentosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DepartamentosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function DepartamentosConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Departamento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Departamentos_Consultar(pID, pNombre, DemeInfoSesion(pstrUsuario, "BuscarDepartamentos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarDepartamentos")
            Return Nothing
        End Try
    End Function

    Public Function TraerDepartamentoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Departamento
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Departamento
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.IDPais = 
            e.ID = -1
            'e.Nombre = 
            'e.Codigo_DaneDEPTO = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IDDepartamento = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDepartamentoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDepartamento(ByVal Departamento As Departamento)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDepartamento")
        End Try
    End Sub

    Public Sub UpdateDepartamento(ByVal currentDepartamento As Departamento)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDepartamento")
        End Try
    End Sub

    Public Sub DeleteDepartamento(ByVal Departamento As Departamento)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDepartamento")
        End Try
    End Sub
#End Region

#Region "Ciudades"

    Public Function CiudadesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Ciudade)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Ciudades_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "CiudadesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CiudadesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CiudadesConsultar(ByVal pIDCodigo As Integer, ByVal pNombre As String, ByVal pCodigoDANE As String, ByVal pIDdepartamento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Ciudade)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Ciudades_Consultar(pIDCodigo, pNombre, pCodigoDANE, pIDdepartamento, DemeInfoSesion(pstrUsuario, "BuscarCiudades"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarCiudades")
            Return Nothing
        End Try
    End Function

    Public Function TraerCiudadePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Ciudade
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Ciudade
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.IDCodigo = 0
            'e.Nombre = 
            'e.EsCapital = 
            e.IDdepartamento = 0
            'e.CodigoDANE = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDCiudad = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCiudadePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCiudade(ByVal Ciudade As Ciudade)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCiudade")
        End Try
    End Sub

    Public Sub UpdateCiudade(ByVal currentCiudade As Ciudade)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCiudade")
        End Try
    End Sub

    Public Sub DeleteCiudade(ByVal Ciudade As Ciudade)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCiudade")
        End Try
    End Sub
#End Region

    'JCM20160226
#Region "Bancos Tasas Rendimientos"
    Public Function BancosTasasRendimientosConsultar(ByVal pintIDBanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of BancosTasasRendimientos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_BancosTasasRendimientos_Consultar(pintIDBanco, pstrUsuario, DemeInfoSesion(pstrUsuario, "BuscarBancosTasasRendimientos"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBancosTasasRendimientos")
            Return Nothing
        End Try
    End Function


    Public Function TraerBancosTasasRendimientosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As BancosTasasRendimientos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New BancosTasasRendimientos
            'e.ID = 
            'e.IDEspecie = 
            e.dblValorInicial = 0
            e.dblValorFinal = 0
            e.dblTasaRendimiento = 0
            'e.Usuario = 
            'e.Actualizacion = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerBancosTasasRendimientosPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertBancosTasasRendimientos(ByVal currentBancosTasasRendimientos As BancosTasasRendimientos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertBancosTasasRendimientos")
        End Try
    End Sub

    Public Sub UpdateBancosTasasRendimientos(ByVal BancosTasasRendimientos As BancosTasasRendimientos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateBancosTasasRendimientos")
        End Try
    End Sub

    Public Sub DeleteBancosTasasRendimientos(ByVal BancosTasasRendimientos As BancosTasasRendimientos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteBancosTasasRendimientos")
        End Try
    End Sub


    'Public Sub InsertBancosTasasRendimientos(ByVal BancosTasasRendimientos As BancosTasasRendimientos)
    '    Try

    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "BancosTasasRendimientos")
    '    End Try
    'End Sub


    ' JFSB 20160825 Se habilita el metodo para poder editar los registros
    'Public Sub UpdateBancosTasasRendimientos(ByVal currentBancosTasasRendimientos As BancosTasasRendimientos)
    '    Try

    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "UpdateBancosTasasRendimientos")
    '    End Try
    'End Sub

    '    Public Sub DeleteBancosTasasRendimientos(ByVal BancosTasasRendimientos As BancosTasasRendimientos)
    '        Try

    '        Catch ex As Exception
    '            ManejarError(ex, Me.ToString(), "DeleteBancosTasasRendimientos")
    '        End Try
    '    End Sub


#End Region

    'JWSJ20160505
#Region "ConsecutivosVsUsuarios"

    <Query(HasSideEffects:=True)>
    Public Function ConsecutivosVsUsuariosFiltrar(pstrModulo As String, pintIDCompanias As Integer, pstrUsuarioProceso As String, pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsecutivosVsUsuarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConsecutivosVsUsuarios_Filtrar(pstrModulo, pintIDCompanias, pstrUsuarioProceso, pstrNombreConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsecutivosVsUsuariosFiltrar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsecutivosVsUsuariosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ConsecutivosVsUsuariosActualizar(ByVal pstrModulo As String, ByVal pxmlConseutivosXUsuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConsecutivosVsUsuarios_Actualizar(pstrModulo, pxmlConseutivosXUsuario, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarConsecutivosVsUsuarios"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarConsecutivosVsUsuarios")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConsecutivosVsUsuarios(ByVal currentConsecutivosVsUsuarios As ConsecutivosVsUsuarios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConsecutivosVsUsuarios")
        End Try
    End Sub

    Public Sub UpdateConsecutivosVsUsuarios(ByVal ConsecutivosVsUsuarios As ConsecutivosVsUsuarios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConsecutivosVsUsuarios")
        End Try
    End Sub

    Public Sub DeleteConsecutivosVsUsuarios(ByVal ConsecutivosVsUsuarios As ConsecutivosVsUsuarios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConsecutivosVsUsuarios")
        End Try
    End Sub



#Region "Métodos sincrónicos"
    Public Function ConsecutivosVsUsuariosActualizarSync(ByVal pstrModulo As String, ByVal pxmlConseutivosXUsuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ConsecutivosVsUsuariosActualizarAsync(pstrModulo, pxmlConseutivosXUsuario, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function ConsecutivosVsUsuariosActualizarAsync(ByVal pstrModulo As String, ByVal pxmlConseutivosXUsuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ConsecutivosVsUsuariosActualizar(pstrModulo, pxmlConseutivosXUsuario, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function



#End Region

    '<Query(HasSideEffects:=True)> _
    'Public Function ConsecutivosVsUsuariosActualizar(ByVal pstrModulo As String,
    '                                                    ByVal pxmlConseutivosXUsuario As String,
    '                                                    ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of ConsecutivosVsUsuarios)
    '    Try
    '        Dim ret = Me.DataContext.uspOyDNet_Maestros_ConsecutivosVsUsuarios_Actualizar(pstrModulo, pxmlConseutivosXUsuario, pstrUsuario, _
    '                                                                                    DemeInfoSesion(pstrUsuario, "ConsecutivosVsUsuariosActualizar"), _
    '                                                                                    0).ToList

    '        Return ret
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "ConsecutivosVsUsuariosActualizar")
    '        Return Nothing
    '    End Try
    'End Function


#End Region


#Region "ConceptosVSConsecutivos"
    <Query(HasSideEffects:=True)>
    Public Function ConceptosVSConsecutivosFiltrar(plngIDConcepto As Integer, pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptosVSConsecutivos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosVSConsecutivos_Filtrar(plngIDConcepto, pstrNombreConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosVSConsecutivosFiltrar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosVSConsecutivosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConcepto(ByVal pintConcepto As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ValidarConceptosTesoreria(pintConcepto, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarConsecutivosVsUsuarios"), 0)
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarConsecutivosVsUsuarios")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosVsConsecutivosActualizar(ByVal pstrModulo As String, ByVal pxmlConceptosXConsecutivos As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ConceptosVsConsecutivos_Actualizar(pstrModulo, pxmlConceptosXConsecutivos, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarConsecutivosVsUsuarios"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarConsecutivosVsUsuarios")
            Return Nothing
        End Try
    End Function

    Public Sub InsertConceptosVsConsecutivos(ByVal currentConceptosVSConsecutivos As ConceptosVSConsecutivos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConsecutivosVsUsuarios")
        End Try
    End Sub

    Public Sub UpdateConceptosVsConsecutivos(ByVal ConceptosVSConsecutivos As ConceptosVSConsecutivos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConsecutivosVsUsuarios")
        End Try
    End Sub

    Public Sub DeleteConceptosVSConsecutivos(ByVal ConceptosVSConsecutivos As ConceptosVSConsecutivos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConsecutivosVsUsuarios")
        End Try
    End Sub
#End Region

#Region "ConsecutivosEquivalencias"
    Public Function TesoreriaConsecutivosEquivalenciasFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaConsecutivosEquivalencias)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TesoreriaConsecutivosEquivalencias_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "TesoreriaConsecutivosEquivalenciasFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaConsecutivosEquivalenciasFiltrar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTesoreriaConsecutivosEquivalencias(ByVal TesoreriaConsecutivosEquivalencias As TesoreriaConsecutivosEquivalencias)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, TesoreriaConsecutivosEquivalencias.pstrUsuarioConexion, TesoreriaConsecutivosEquivalencias.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TesoreriaConsecutivosEquivalencias.InfoSesion = DemeInfoSesion(TesoreriaConsecutivosEquivalencias.pstrUsuarioConexion, "InsertTesoreriaConsecutivosEquivalencias")
            Me.DataContext.TesoreriaConsecutivosEquivalencias.InsertOnSubmit(TesoreriaConsecutivosEquivalencias)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaConsecutivosEquivalencias")
        End Try
    End Sub

    Public Sub UpdateTesoreriaConsecutivosEquivalencias(ByVal currentTesoreriaConsecutivosEquivalencias As TesoreriaConsecutivosEquivalencias)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentTesoreriaConsecutivosEquivalencias.pstrUsuarioConexion, currentTesoreriaConsecutivosEquivalencias.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTesoreriaConsecutivosEquivalencias.InfoSesion = DemeInfoSesion(currentTesoreriaConsecutivosEquivalencias.pstrUsuarioConexion, "UpdateTesoreriaConsecutivosEquivalencias")
            Me.DataContext.TesoreriaConsecutivosEquivalencias.Attach(currentTesoreriaConsecutivosEquivalencias, Me.ChangeSet.GetOriginal(currentTesoreriaConsecutivosEquivalencias))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaConsecutivosEquivalencias")
        End Try
    End Sub

    Public Sub DeleteTesoreriaConsecutivosEquivalencias(ByVal TesoreriaConsecutivosEquivalencias As TesoreriaConsecutivosEquivalencias)
        Try
            ' ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TesoreriaConsecutivosEquivalencias.pstrUsuarioConexion, TesoreriaConsecutivosEquivalencias.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConsecutivosDocumentos_Eliminar( pDocumento,  pNombreConsecutivo,  pDescripcion,  pCuentaContable,  pCuentaContable,  pPermiteCliente,  pTipoCuenta,  psucursalConciliacion,  pIdSucursalSuvalor,  pConcepto,  pComprobanteContable,  pIncluidoEnExtractoBanco,  pIdMoneda, DemeInfoSesion(pstrUsuario, "DeleteConsecutivosDocumento"),0).ToList
            TesoreriaConsecutivosEquivalencias.InfoSesion = DemeInfoSesion(TesoreriaConsecutivosEquivalencias.pstrUsuarioConexion, "DeleteTesoreriaConsecutivosEquivalencias")
            Me.DataContext.TesoreriaConsecutivosEquivalencias.Attach(TesoreriaConsecutivosEquivalencias)
            Me.DataContext.TesoreriaConsecutivosEquivalencias.DeleteOnSubmit(TesoreriaConsecutivosEquivalencias)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaConsecutivosEquivalencias")
        End Try
    End Sub

#End Region

#Region "Tipos cuentas recaudadoras"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertTiposCuentasRecaudadoras(ByVal objTiposCuentasRecaudadoras As TiposCuentasRecaudadoras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objTiposCuentasRecaudadoras.pstrUsuarioConexion, objTiposCuentasRecaudadoras.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTiposCuentasRecaudadoras.InfoSession = DemeInfoSesion(objTiposCuentasRecaudadoras.pstrUsuarioConexion, "InsertTiposCuentasRecaudadoras")
            Me.DataContext.TiposCuentasRecaudadoras.InsertOnSubmit(objTiposCuentasRecaudadoras)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTiposCuentasRecaudadoras")
        End Try
    End Sub
    Public Sub UpdateTiposCuentasRecaudadoras(ByVal currentTiposCuentasRecaudadoras As TiposCuentasRecaudadoras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentTiposCuentasRecaudadoras.pstrUsuarioConexion, currentTiposCuentasRecaudadoras.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTiposCuentasRecaudadoras.InfoSession = DemeInfoSesion(currentTiposCuentasRecaudadoras.pstrUsuarioConexion, "UpdateTiposCuentasRecaudadoras")
            Me.DataContext.TiposCuentasRecaudadoras.Attach(currentTiposCuentasRecaudadoras, Me.ChangeSet.GetOriginal(currentTiposCuentasRecaudadoras))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTiposCuentasRecaudadoras")
        End Try
    End Sub
    Public Sub DeleteTiposCuentasRecaudadoras(ByVal objTiposCuentasRecaudadoras As TiposCuentasRecaudadoras)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objTiposCuentasRecaudadoras.pstrUsuarioConexion, objTiposCuentasRecaudadoras.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objTiposCuentasRecaudadoras.InfoSession = DemeInfoSesion(objTiposCuentasRecaudadoras.pstrUsuarioConexion, "DeleteTiposCuentasRecaudadoras")
            Me.DataContext.TiposCuentasRecaudadoras.Attach(objTiposCuentasRecaudadoras)
            Me.DataContext.TiposCuentasRecaudadoras.DeleteOnSubmit(objTiposCuentasRecaudadoras)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTiposCuentasRecaudadoras")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarTiposCuentasRecaudadoras(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposCuentasRecaudadoras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TiposCuentasRecaudadoras_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarTiposCuentasRecaudadoras"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarTiposCuentasRecaudadoras")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarTiposCuentasRecaudadoras(ByVal pintCodigo As Nullable(Of Integer), ByVal pstrTipoCuenta As String, ByVal pstrTipoReciboCaja As String, ByVal pstrRegistrarCheques As String, ByVal pstrManejoComisiones As String, ByVal pstrManejoTraslado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposCuentasRecaudadoras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TiposCuentasRecaudadoras_Consultar(String.Empty, pintCodigo, pstrTipoCuenta, pstrTipoReciboCaja, pstrRegistrarCheques, pstrManejoComisiones, pstrManejoTraslado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarTiposCuentasRecaudadoras"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarTiposCuentasRecaudadoras")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarTiposCuentasRecaudadorasDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TiposCuentasRecaudadoras
        Dim objDefecto As TiposCuentasRecaudadoras = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_TiposCuentasRecaudadoras_Consultar("DEFAULT", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarTiposCuentasRecaudadorasDefecto"), 0).ToList
            If ret.Count > 0 Then
                objDefecto = ret.FirstOrDefault
            End If
            Return objDefecto
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarTiposCuentasRecaudadorasDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarTiposCuentasRecaudadorasSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposCuentasRecaudadoras)
        Dim objTask As Task(Of List(Of TiposCuentasRecaudadoras)) = Me.FiltrarTiposCuentasRecaudadorasAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarTiposCuentasRecaudadorasAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TiposCuentasRecaudadoras))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TiposCuentasRecaudadoras)) = New TaskCompletionSource(Of List(Of TiposCuentasRecaudadoras))()
        objTaskComplete.TrySetResult(FiltrarTiposCuentasRecaudadoras(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarTiposCuentasRecaudadorasSync(ByVal pintCodigo As Nullable(Of Integer), ByVal pstrTipoCuenta As String, ByVal pstrTipoReciboCaja As String, ByVal pstrRegistrarCheques As String, ByVal pstrManejoComisiones As String, ByVal pstrManejoTraslado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposCuentasRecaudadoras)
        Dim objTask As Task(Of List(Of TiposCuentasRecaudadoras)) = Me.ConsultarTiposCuentasRecaudadorasAsync(pintCodigo, pstrTipoCuenta, pstrTipoReciboCaja, pstrRegistrarCheques, pstrManejoComisiones, pstrManejoTraslado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarTiposCuentasRecaudadorasAsync(ByVal pintCodigo As Nullable(Of Integer), ByVal pstrTipoCuenta As String, ByVal pstrTipoReciboCaja As String, ByVal pstrRegistrarCheques As String, ByVal pstrManejoComisiones As String, ByVal pstrManejoTraslado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TiposCuentasRecaudadoras))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TiposCuentasRecaudadoras)) = New TaskCompletionSource(Of List(Of TiposCuentasRecaudadoras))()
        objTaskComplete.TrySetResult(ConsultarTiposCuentasRecaudadoras(pintCodigo, pstrTipoCuenta, pstrTipoReciboCaja, pstrRegistrarCheques, pstrManejoComisiones, pstrManejoTraslado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarTiposCuentasRecaudadorasDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TiposCuentasRecaudadoras
        Dim objTask As Task(Of TiposCuentasRecaudadoras) = Me.ConsultarTiposCuentasRecaudadorasDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarTiposCuentasRecaudadorasDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of TiposCuentasRecaudadoras)
        Dim objTaskComplete As TaskCompletionSource(Of TiposCuentasRecaudadoras) = New TaskCompletionSource(Of TiposCuentasRecaudadoras)()
        objTaskComplete.TrySetResult(ConsultarTiposCuentasRecaudadorasDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CuentasBancarias por concepto"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCuentasBancariasPorConcepto(ByVal objCuentasBancariasPorConcepto As CuentasBancariasPorConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objCuentasBancariasPorConcepto.pstrUsuarioConexion, objCuentasBancariasPorConcepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCuentasBancariasPorConcepto.InfoSession = DemeInfoSesion(objCuentasBancariasPorConcepto.pstrUsuarioConexion, "InsertCuentasBancariasPorConcepto")
            Me.DataContext.CuentasBancariasPorConcepto.InsertOnSubmit(objCuentasBancariasPorConcepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCuentasBancariasPorConcepto")
        End Try
    End Sub
    Public Sub UpdateCuentasBancariasPorConcepto(ByVal currentCuentasBancariasPorConcepto As CuentasBancariasPorConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCuentasBancariasPorConcepto.pstrUsuarioConexion, currentCuentasBancariasPorConcepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCuentasBancariasPorConcepto.InfoSession = DemeInfoSesion(currentCuentasBancariasPorConcepto.pstrUsuarioConexion, "UpdateCuentasBancariasPorConcepto")
            Me.DataContext.CuentasBancariasPorConcepto.Attach(currentCuentasBancariasPorConcepto, Me.ChangeSet.GetOriginal(currentCuentasBancariasPorConcepto))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCuentasBancariasPorConcepto")
        End Try
    End Sub
    Public Sub DeleteCuentasBancariasPorConcepto(ByVal objCuentasBancariasPorConcepto As CuentasBancariasPorConcepto)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCuentasBancariasPorConcepto.pstrUsuarioConexion, objCuentasBancariasPorConcepto.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCuentasBancariasPorConcepto.InfoSession = DemeInfoSesion(objCuentasBancariasPorConcepto.pstrUsuarioConexion, "DeleteCuentasBancariasPorConcepto")
            Me.DataContext.CuentasBancariasPorConcepto.Attach(objCuentasBancariasPorConcepto)
            Me.DataContext.CuentasBancariasPorConcepto.DeleteOnSubmit(objCuentasBancariasPorConcepto)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCuentasBancariasPorConcepto")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarCuentasBancariasPorConcepto(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasBancariasPorConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_CuentasbancariasPorConcepto_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "FiltrarCuentasBancariasPorConcepto"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarCuentasBancariasPorConcepto")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarCuentasBancariasPorConcepto(ByVal plngIdCuentaBancaria As Nullable(Of Integer), ByVal pstrCuentaContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasBancariasPorConcepto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_CuentasbancariasPorConcepto_Consultar(Convert.ToInt32(plngIdCuentaBancaria), pstrCuentaContable, DemeInfoSesion(pstrUsuario, "ConsultarCuentasBancariasPorConcepto"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCuentasBancariasPorConcepto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarCuentasBancariasPorConceptoSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasBancariasPorConcepto)
        Dim objTask As Task(Of List(Of CuentasBancariasPorConcepto)) = Me.FiltrarCuentasBancariasPorConceptoAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarCuentasBancariasPorConceptoAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CuentasBancariasPorConcepto))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CuentasBancariasPorConcepto)) = New TaskCompletionSource(Of List(Of CuentasBancariasPorConcepto))()
        objTaskComplete.TrySetResult(FiltrarCuentasBancariasPorConcepto(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCuentasBancariasPorConceptoSync(ByVal plngIdCuentaBancaria As Nullable(Of Integer), ByVal pstrCuentaContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasBancariasPorConcepto)
        Dim objTask As Task(Of List(Of CuentasBancariasPorConcepto)) = Me.ConsultarCuentasBancariasPorConceptoAsync(plngIdCuentaBancaria, pstrCuentaContable, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCuentasBancariasPorConceptoAsync(ByVal plngIdCuentaBancaria As Nullable(Of Integer), ByVal pstrCuentaContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CuentasBancariasPorConcepto))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CuentasBancariasPorConcepto)) = New TaskCompletionSource(Of List(Of CuentasBancariasPorConcepto))()
        objTaskComplete.TrySetResult(ConsultarCuentasBancariasPorConcepto(plngIdCuentaBancaria, pstrCuentaContable, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CuentasCRCC"
    <Query(HasSideEffects:=False)>
    Public Function CuentasCRCC_Filtrar(pstrfiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasCRCC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_CuentasCRCC_Filtrar(RetornarValorDescodificado(pstrfiltro), DemeInfoSesion(pstrUsuario, "CuentasCRCC_Filtrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasCRCC_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function CuentasCRCC_Consultar(ByVal pstrCuenta As String, ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasCRCC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Clientes_CuentasCRCC_Consultar(pstrCuenta, pComitente, DemeInfoSesion(pstrUsuario, "CuentasCRCC_Filtrar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasCRCC_Filtrar")
            Return Nothing
        End Try
    End Function



    Public Sub InsertCuentasCRCC(ByVal CuentaCRCC As CuentasCRCC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, CuentaCRCC.pstrUsuarioConexion, CuentaCRCC.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CuentaCRCC.InfoSesion = DemeInfoSesion(CuentaCRCC.pstrUsuarioConexion, "CuentaCRCC")
            Me.DataContext.CuentasCRCC.InsertOnSubmit(CuentaCRCC)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentaCRCC")
        End Try
    End Sub

    Public Sub UpdateCuentasCRCC(ByVal currentCuentaCRCC As CuentasCRCC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCuentaCRCC.pstrUsuarioConexion, currentCuentaCRCC.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCuentaCRCC.InfoSesion = DemeInfoSesion(currentCuentaCRCC.pstrUsuarioConexion, "UpdateCuentasCRCC")
            Me.DataContext.CuentasCRCC.Attach(currentCuentaCRCC, Me.ChangeSet.GetOriginal(currentCuentaCRCC))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCuentasCRCC")
        End Try
    End Sub

    Public Sub DeleteCuentasCRCC(ByVal CuentaCRCC As CuentasCRCC)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CuentaCRCC.pstrUsuarioConexion, CuentaCRCC.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CuentaCRCC.InfoSesion = DemeInfoSesion(CuentaCRCC.pstrUsuarioConexion, "DeleteCuentasCRCC")
            Me.DataContext.CuentasCRCC.Attach(CuentaCRCC)
            Me.DataContext.CuentasCRCC.DeleteOnSubmit(CuentaCRCC)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCuentasCRCC")
        End Try
    End Sub

    Public Function EliminarCuentasCRCC(ByVal pintIDtblCuentasCRCC As Integer, ByVal mensaje As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_CuentasDecevalPorAgrupador_Eliminar( pComitente, DemeInfoSesion(pstrUsuario, "DeleteCuentasDecevalPorAgrupado"),0).ToList
            Dim ret = Me.DataContext.uspOyDNet_Clientes_CuentasCRCC_Eliminar(pintIDtblCuentasCRCC, mensaje, DemeInfoSesion(pstrUsuario, "EliminarCuentasCRCC"), 0)
            Return mensaje
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarCuentasCRCC")
            Return Nothing
        End Try
    End Function

    Public Function TraerCuentasCRCCPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CuentasCRCC
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New CuentasCRCC
            'e.IDComitente = ""
            'e.CuentaCRCC = ""
            'e.TipoDeOferta = ""
            'e.IdSucComisionista = 
            'e.TipoIdComitente = 
            'e.NroDocumento = 
            'e.Comitente = 
            'e.CuentaDeceval = 
            'e.Conector1 = 
            'e.TipoIdBenef1 = 
            'e.NroDocBenef1 = 
            'e.Conector2 = 
            'e.TipoIdBenef2 = 
            'e.NroDocBenef2 = 
            'e.Deposito = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.ntermedia = 
            'e.CuentaPrincipalDCV = 
            'e.IDCuentasDeceval = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCuentasCRCCPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Codigos ajustes"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCodigosAjustes(ByVal objCodigosAjustes As CodigosAjustes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objCodigosAjustes.pstrUsuarioConexion, objCodigosAjustes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCodigosAjustes.InfoSesion = DemeInfoSesion(objCodigosAjustes.pstrUsuarioConexion, "InsertCodigosAjustes")
            Me.DataContext.CodigosAjustes.InsertOnSubmit(objCodigosAjustes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCodigosAjustes")
        End Try
    End Sub
    Public Sub UpdateCodigosAjustes(ByVal currentCodigosAjustes As CodigosAjustes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCodigosAjustes.pstrUsuarioConexion, currentCodigosAjustes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCodigosAjustes.InfoSesion = DemeInfoSesion(currentCodigosAjustes.pstrUsuarioConexion, "UpdateCodigosAjustes")
            Me.DataContext.CodigosAjustes.Attach(currentCodigosAjustes, Me.ChangeSet.GetOriginal(currentCodigosAjustes))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCodigosAjustes")
        End Try
    End Sub
    Public Sub DeleteCodigosAjustes(ByVal objCodigosAjustes As CodigosAjustes)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCodigosAjustes.pstrUsuarioConexion, objCodigosAjustes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCodigosAjustes.InfoSesion = DemeInfoSesion(objCodigosAjustes.pstrUsuarioConexion, "DeleteCodigosAjustes")
            Me.DataContext.CodigosAjustes.Attach(objCodigosAjustes)
            Me.DataContext.CodigosAjustes.DeleteOnSubmit(objCodigosAjustes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCodigosAjustes")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarCodigosAjustes(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosAjustes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_CodigosAjustes_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "FiltrarCodigosAjustes"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarCodigosAjustes")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarCodigosAjustes(ByVal pstrCodTransaccion As Nullable(Of Double), ByVal pstrDescripcion As String, ByVal pstrIdOwner As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosAjustes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_CodigosAjustes_Consultar(Convert.ToInt32(pstrCodTransaccion), pstrDescripcion, pstrIdOwner, DemeInfoSesion(pstrUsuario, "ConsultarCodigosAjustes"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCodigosAjustes")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarCodigosAjustesSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosAjustes)
        Dim objTask As Task(Of List(Of CodigosAjustes)) = Me.FiltrarCodigosAjustesAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarCodigosAjustesAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CodigosAjustes))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CodigosAjustes)) = New TaskCompletionSource(Of List(Of CodigosAjustes))()
        objTaskComplete.TrySetResult(FiltrarCodigosAjustes(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCodigosAjustesSync(ByVal pstrCodTransaccion As Nullable(Of Double), ByVal pstrDescripcion As String, ByVal pstrIdOwner As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosAjustes)
        Dim objTask As Task(Of List(Of CodigosAjustes)) = Me.ConsultarCodigosAjustesAsync(pstrCodTransaccion, pstrDescripcion, pstrIdOwner, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCodigosAjustesAsync(ByVal pstrCodTransaccion As Nullable(Of Double), ByVal pstrDescripcion As String, ByVal pstrIdOwner As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CodigosAjustes))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CodigosAjustes)) = New TaskCompletionSource(Of List(Of CodigosAjustes))()
        objTaskComplete.TrySetResult(ConsultarCodigosAjustes(pstrCodTransaccion, pstrDescripcion, pstrIdOwner, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Clientes Responsables"

    Public Sub InsertClientesResponsables(ByVal ClientesResponsables As Clientes_Responsables)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesResponsables.pstrUsuarioConexion, ClientesResponsables.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesResponsables.InfoSesion = DemeInfoSesion(ClientesResponsables.pstrUsuarioConexion, "Clientes_Responsables")
            Me.DataContext.Clientes_Responsables.InsertOnSubmit(ClientesResponsables)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Clientes_Responsables")
        End Try
    End Sub

    Public Sub UpdateClientesResponsables(ByVal currentClientesResponsables As Clientes_Responsables)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesResponsables.pstrUsuarioConexion, currentClientesResponsables.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClientesResponsables.InfoSesion = DemeInfoSesion(currentClientesResponsables.pstrUsuarioConexion, "Clientes_Responsables")
            Me.DataContext.Clientes_Responsables.Attach(currentClientesResponsables, Me.ChangeSet.GetOriginal(currentClientesResponsables))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Clientes_Responsables")
        End Try
    End Sub

    Public Sub DeleteClientesResponsables(ByVal ClientesResponsables As Clientes_Responsables)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClientesResponsables.pstrUsuarioConexion, ClientesResponsables.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesResponsables.InfoSesion = DemeInfoSesion(ClientesResponsables.pstrUsuarioConexion, "DeleteClientesResponsables")
            Me.DataContext.Clientes_Responsables.Attach(ClientesResponsables)
            Me.DataContext.Clientes_Responsables.DeleteOnSubmit(ClientesResponsables)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesResponsables")
        End Try
    End Sub
    <Query(HasSideEffects:=False)>
    Public Function ClientesResponsables_Filtrar(pstrfiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Clientes_Responsables)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesResponsables_Filtrar(RetornarValorDescodificado(pstrfiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesResponsables_Filtrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesResponsables_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function ClientesResponsables_Consultar(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Clientes_Responsables)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesResponsables_Consultar(pComitente, DemeInfoSesion(pstrUsuario, "ClientesResponsables_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesResponsables_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerClientesResponsablesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Clientes_Responsables
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Clientes_Responsables
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesResponsablesPorDefecto")
            Return Nothing
        End Try
    End Function


    Public Function ConsultarClienteResponsable_comitente(ByVal plngComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spClientes_Responsables_Consultar_comitente(plngComitente, False, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarClienteResponsable_comitente"), ClsConstantes.GINT_ErrorPersonalizado)
            Return CBool(ret)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarClienteResponsable_comitente")
            Return Nothing
        End Try
    End Function

#End Region

#Region "encabezado Responsables"
    Public Function TraerEncabezadoResponsablePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EncabezadoResponsable
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New EncabezadoResponsable
            'e.Id = 
            'e.Nombre = 
            'e.DireccionEnvio = 
            'e.idReceptor = 
            'e.IDSucCliente = 
            e.IDEncabezadoResponsable = -1
            Return (e)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEncabezadoResponsablePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEncabezadoResponsable(ByVal EncabezadoResponsable As EncabezadoResponsable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, EncabezadoResponsable.pstrUsuarioConexion, EncabezadoResponsable.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            EncabezadoResponsable.InfoSesion = DemeInfoSesion(EncabezadoResponsable.pstrUsuarioConexion, "InsertDetalleClienteAgrupado")
            Me.DataContext.EncabezadoResponsable.InsertOnSubmit(EncabezadoResponsable)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDetalleEncabezadoResponsable")
        End Try
    End Sub

    Public Sub UpdateEncabezadoResponsable(ByVal currentEncabezadoResponsable As EncabezadoResponsable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentEncabezadoResponsable.pstrUsuarioConexion, currentEncabezadoResponsable.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEncabezadoResponsable.InfoSesion = DemeInfoSesion(currentEncabezadoResponsable.pstrUsuarioConexion, "UpdateEncabezadoResponsable")
            Me.DataContext.EncabezadoResponsable.Attach(currentEncabezadoResponsable, Me.ChangeSet.GetOriginal(currentEncabezadoResponsable))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEncabezadoResponsable")
        End Try
    End Sub

    Public Sub DeleteEncabezadoResponsable(ByVal EncabezadoResponsable As EncabezadoResponsable)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EncabezadoResponsable.pstrUsuarioConexion, EncabezadoResponsable.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            EncabezadoResponsable.InfoSesion = DemeInfoSesion(EncabezadoResponsable.pstrUsuarioConexion, "DeleteEncabezadoResponsable")
            Me.DataContext.EncabezadoResponsable.Attach(EncabezadoResponsable)
            Me.DataContext.EncabezadoResponsable.DeleteOnSubmit(EncabezadoResponsable)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEncabezadoResponsable")
        End Try
    End Sub

    Public Function EncabezadoClienteResponsableConsultar(ByVal pId As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EncabezadoResponsable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_EncabezadoCResponsable_Consultar(pId, DemeInfoSesion(pstrUsuario, "EncabezadoClienteResponsableConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EncabezadoClienteResponsableConsultar")
            Return Nothing
        End Try
    End Function

    Public Function EncabezadoCResponsable_Filtrar(pstrfiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EncabezadoResponsable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_EncabezadoCResponsable_Filtrar(RetornarValorDescodificado(pstrfiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "EncabezadoCResponsable_Filtrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EncabezadoCResponsable_Filtrar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Clientes relacionados"
    Public Sub InsertClientesRelacionados(ByVal ClientesRelacionados As Clientes_Relacionados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ClientesRelacionados.pstrUsuarioConexion, ClientesRelacionados.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesRelacionados.strXml_Detalles = RetornarValorDescodificado(ClientesRelacionados.strXml_Detalles)
            ClientesRelacionados.InfoSesion = DemeInfoSesion(ClientesRelacionados.pstrUsuarioConexion, "Clientes_Relacionados")
            Me.DataContext.tblClientes_Relacionados.InsertOnSubmit(ClientesRelacionados)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Clientes_Relacionados")
        End Try
    End Sub

    Public Sub UpdateClientesRelacionados(ByVal currentClientesRelacionados As Clientes_Relacionados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentClientesRelacionados.pstrUsuarioConexion, currentClientesRelacionados.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClientesRelacionados.strXml_Detalles = RetornarValorDescodificado(currentClientesRelacionados.strXml_Detalles)
            currentClientesRelacionados.InfoSesion = DemeInfoSesion(currentClientesRelacionados.pstrUsuarioConexion, "Clientes_Relacionados")
            Me.DataContext.tblClientes_Relacionados.Attach(currentClientesRelacionados, Me.ChangeSet.GetOriginal(currentClientesRelacionados))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Clientes_Relacionados")
        End Try
    End Sub

    Public Sub DeleteClientesRelacionados(ByVal ClientesRelacionados As Clientes_Relacionados)
        Try
            ' ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDUtilidadesConnectionString, currentClientesRelacionados.pstrUsuarioConexion, currentClientesRelacionados.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClientesRelacionados.strXml_Detalles = RetornarValorDescodificado(ClientesRelacionados.strXml_Detalles)
            ClientesRelacionados.InfoSesion = DemeInfoSesion(ClientesRelacionados.pstrUsuarioConexion, "DeleteClientesRelacionados")
            Me.DataContext.tblClientes_Relacionados.Attach(ClientesRelacionados)
            Me.DataContext.tblClientes_Relacionados.DeleteOnSubmit(ClientesRelacionados)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClientesRelacionados")
        End Try
    End Sub

    Public Function ClientesRelacionados_Consultar(ByVal pComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Clientes_Relacionados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesRelacionados_Consultar(pComitente, DemeInfoSesion(pstrUsuario, "ClientesRelacionados_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesResponsables_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function TraerClientesRelacionadosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Clientes_Relacionados
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Clientes_Relacionados
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClientesRelacionadosPorDefecto")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=False)>
    Public Function ClientesRelacionados_Filtrar(pstrfiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Clientes_Relacionados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClientesRelacionados_Filtrar(RetornarValorDescodificado(pstrfiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ClientesRelacionados_Filtrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesRelacionados_Filtrar")
            Return Nothing
        End Try
    End Function


    Public Function ConsultaClientesRelacionados(ByVal plngComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spOyDNet_ClientesRelacionados_validar(plngComitente, False, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultaClientesRelacionados"), ClsConstantes.GINT_ErrorPersonalizado)
            Return CBool(ret)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultaClientesRelacionados")
            Return Nothing
        End Try
    End Function

    Public Function ConsultaClientesTP(ByVal plngComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spOyDNet_ClientesRelacionados_ConsultarTP(plngComitente, False, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultaClientesRelacionados"), ClsConstantes.GINT_ErrorPersonalizado)
            Return CBool(ret)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultaClientesRelacionados")
            Return Nothing
        End Try
    End Function
#End Region

#Region "encabezado Relacionado"
    Public Function TraerEncabezadoRelacionadoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EncabezadoRelacionado
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New EncabezadoRelacionado
            'e.Nombre
            'e.Nombre = 
            'e.DireccionEnvio = 
            'e.idReceptor = 
            'e.IDSucCliente = 
            e.IDEncabezadoRelacionado = -1
            Return (e)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEncabezadoRelacionadoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEncabezadoRelacionado(ByVal EncabezadoRelacionado As EncabezadoRelacionado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, EncabezadoRelacionado.pstrUsuarioConexion, EncabezadoRelacionado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            EncabezadoRelacionado.InfoSesion = DemeInfoSesion(EncabezadoRelacionado.pstrUsuarioConexion, "InsertEncabezadoRelacionado")
            Me.DataContext.EncabezadoRelacionado.InsertOnSubmit(EncabezadoRelacionado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDetalleEncabezadoResponsable")
        End Try
    End Sub

    Public Sub UpdateEncabezadoRelacionado(ByVal currentEncabezadoRelacionado As EncabezadoRelacionado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentEncabezadoRelacionado.pstrUsuarioConexion, currentEncabezadoRelacionado.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEncabezadoRelacionado.InfoSesion = DemeInfoSesion(currentEncabezadoRelacionado.pstrUsuarioConexion, "UpdateEncabezadoResponsable")
            Me.DataContext.EncabezadoRelacionado.Attach(currentEncabezadoRelacionado, Me.ChangeSet.GetOriginal(currentEncabezadoRelacionado))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEncabezadoRelacionado")
        End Try
    End Sub

    Public Sub DeleteEncabezadoRelacionado(ByVal EncabezadoRelacionado As EncabezadoRelacionado)
        Try
            'ValidacionSeguridadUsuario(EncabezadoResponsable.pstrUsuarioConexion, EncabezadoResponsable.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            EncabezadoRelacionado.InfoSesion = DemeInfoSesion(EncabezadoRelacionado.pstrUsuarioConexion, "DeleteEncabezadoRelacionado")
            Me.DataContext.EncabezadoRelacionado.Attach(EncabezadoRelacionado)
            Me.DataContext.EncabezadoRelacionado.DeleteOnSubmit(EncabezadoRelacionado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEncabezadoRelacionado")
        End Try
    End Sub

    Public Function EncabezadoClienteRelacionadoConsultar(ByVal pId As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EncabezadoRelacionado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_EncabezadoCRelacionado_Consultar(pId, DemeInfoSesion(pstrUsuario, "EncabezadoClienteRelacionadoConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EncabezadoClienteRelacionadoConsultar")
            Return Nothing
        End Try
    End Function

    Public Function EncabezadoCRelacionadoFiltrar(pstrfiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EncabezadoRelacionado)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_EncabezadoCRelacionado_Filtrar(RetornarValorDescodificado(pstrfiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "EncabezadoCRelacionadoFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EncabezadoCRelacionadoFiltrar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Codigos transaccion"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertCodigosTransaccion(ByVal objCodigosTransaccion As CodigosTransaccion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objCodigosTransaccion.pstrUsuarioConexion, objCodigosTransaccion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCodigosTransaccion.infosesion = DemeInfoSesion(objCodigosTransaccion.pstrUsuarioConexion, "InsertCodigosTransaccion")
            Me.DataContext.CodigosTransaccion.InsertOnSubmit(objCodigosTransaccion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCodigosTransaccion")
        End Try
    End Sub
    Public Sub UpdateCodigosTransaccion(ByVal currentCodigosTransaccion As CodigosTransaccion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentCodigosTransaccion.pstrUsuarioConexion, currentCodigosTransaccion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCodigosTransaccion.infosesion = DemeInfoSesion(currentCodigosTransaccion.pstrUsuarioConexion, "UpdateCodigosTransaccion")
            Me.DataContext.CodigosTransaccion.Attach(currentCodigosTransaccion, Me.ChangeSet.GetOriginal(currentCodigosTransaccion))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCodigosTransaccion")
        End Try
    End Sub
    Public Sub DeleteCodigosTransaccion(ByVal objCodigosTransaccion As CodigosTransaccion)
        Try
            objCodigosTransaccion.infosesion = DemeInfoSesion(objCodigosTransaccion.pstrUsuarioConexion, "DeleteCodigosTransaccion")
            Me.DataContext.CodigosTransaccion.Attach(objCodigosTransaccion)
            Me.DataContext.CodigosTransaccion.DeleteOnSubmit(objCodigosTransaccion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCodigosTransaccion")
        End Try
    End Sub
#End Region
#Region "Metodos asicronicos"
    Public Function FiltrarCodigosTransaccion(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosTransaccion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CodigosTransaccion_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "FiltrarCodigosTransaccion"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarCodigosTransaccion")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarCodigosTransaccion(ByVal plngCodigo As Integer, ByVal pstrTransaccion As String, ByVal pstrDetalleRC As String, ByVal pstrTipoTransaccion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosTransaccion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CodigosTransaccion_Consultar(plngCodigo, pstrTransaccion, pstrDetalleRC, pstrTipoTransaccion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCodigosTransaccion"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCodigosTransaccion")
            Return Nothing
        End Try
    End Function
#End Region
#Region "Metodos sincronicos"
    Public Function FiltrarCodigosTransaccionSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosTransaccion)
        Dim objTask As Task(Of List(Of CodigosTransaccion)) = Me.FiltrarCodigosTransaccionAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarCodigosTransaccionAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CodigosTransaccion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CodigosTransaccion)) = New TaskCompletionSource(Of List(Of CodigosTransaccion))()
        objTaskComplete.TrySetResult(FiltrarCodigosTransaccion(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCodigosTransaccionSync(ByVal plngCodigo As Integer, ByVal pstrTransaccion As String, ByVal pstrDetalleRC As String, ByVal pstrTipoTransaccion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodigosTransaccion)
        Dim objTask As Task(Of List(Of CodigosTransaccion)) = Me.ConsultarCodigosTransaccionAsync(plngCodigo, pstrTransaccion, pstrDetalleRC, pstrTipoTransaccion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCodigosTransaccionAsync(ByVal plngCodigo As Integer, ByVal pstrTransaccion As String, ByVal pstrDetalleRC As String, ByVal pstrTipoTransaccion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of CodigosTransaccion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of CodigosTransaccion)) = New TaskCompletionSource(Of List(Of CodigosTransaccion))()
        objTaskComplete.TrySetResult(ConsultarCodigosTransaccion(plngCodigo, pstrTransaccion, pstrDetalleRC, pstrTipoTransaccion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function
#End Region
#End Region

End Class