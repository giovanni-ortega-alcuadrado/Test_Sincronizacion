﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Data.Entity.Core.Objects
Imports System.Linq

Partial Public Class DER_OperacionesEntities
    Inherits DbContext

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property tblCRCC_CTRADES() As DbSet(Of tblCRCC_CTRADES)
    Public Overridable Property tblCRCC_CTRADETYP() As DbSet(Of tblCRCC_CTRADETYP)

    Public Overridable Function usp_Procesos_Utilitarios_CargarCombos(pstrOpcion As String, pstrTopico As String, pstrParametroAdicional1 As String, pstrParametroAdicional2 As String, pstrParametroAdicional3 As String, pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblCombos)
        Dim pstrOpcionParameter As ObjectParameter = If(pstrOpcion IsNot Nothing, New ObjectParameter("pstrOpcion", pstrOpcion), New ObjectParameter("pstrOpcion", GetType(String)))

        Dim pstrTopicoParameter As ObjectParameter = If(pstrTopico IsNot Nothing, New ObjectParameter("pstrTopico", pstrTopico), New ObjectParameter("pstrTopico", GetType(String)))

        Dim pstrParametroAdicional1Parameter As ObjectParameter = If(pstrParametroAdicional1 IsNot Nothing, New ObjectParameter("pstrParametroAdicional1", pstrParametroAdicional1), New ObjectParameter("pstrParametroAdicional1", GetType(String)))

        Dim pstrParametroAdicional2Parameter As ObjectParameter = If(pstrParametroAdicional2 IsNot Nothing, New ObjectParameter("pstrParametroAdicional2", pstrParametroAdicional2), New ObjectParameter("pstrParametroAdicional2", GetType(String)))

        Dim pstrParametroAdicional3Parameter As ObjectParameter = If(pstrParametroAdicional3 IsNot Nothing, New ObjectParameter("pstrParametroAdicional3", pstrParametroAdicional3), New ObjectParameter("pstrParametroAdicional3", GetType(String)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblCombos)("usp_Procesos_Utilitarios_CargarCombos", pstrOpcionParameter, pstrTopicoParameter, pstrParametroAdicional1Parameter, pstrParametroAdicional2Parameter, pstrParametroAdicional3Parameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Procesos_CruceOperaciones_CruceAutomatico(pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Procesos_CruceOperaciones_CruceAutomatico", pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Procesos_CruceOperaciones_DescalzarLiquidaciones(pintIdImportacion As Nullable(Of Integer), pbitCalcularCantidadPendiente As Nullable(Of Boolean), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIdImportacionParameter As ObjectParameter = If(pintIdImportacion.HasValue, New ObjectParameter("pintIdImportacion", pintIdImportacion), New ObjectParameter("pintIdImportacion", GetType(Integer)))

        Dim pbitCalcularCantidadPendienteParameter As ObjectParameter = If(pbitCalcularCantidadPendiente.HasValue, New ObjectParameter("pbitCalcularCantidadPendiente", pbitCalcularCantidadPendiente), New ObjectParameter("pbitCalcularCantidadPendiente", GetType(Boolean)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Procesos_CruceOperaciones_DescalzarLiquidaciones", pintIdImportacionParameter, pbitCalcularCantidadPendienteParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Procesos_CruceOperaciones_Consultar(pstrNemotecnico As String, pintFolioBVC As Nullable(Of Long), pintFolioCamara As Nullable(Of Integer), bitCruzada As Nullable(Of Boolean), pdtmFechaImportacion As String, pstrGestionOperaciones As String, pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblCRCC_CTRADES)
        Dim pstrNemotecnicoParameter As ObjectParameter = If(pstrNemotecnico IsNot Nothing, New ObjectParameter("pstrNemotecnico", pstrNemotecnico), New ObjectParameter("pstrNemotecnico", GetType(String)))

        Dim pintFolioBVCParameter As ObjectParameter = If(pintFolioBVC.HasValue, New ObjectParameter("pintFolioBVC", pintFolioBVC), New ObjectParameter("pintFolioBVC", GetType(Long)))

        Dim pintFolioCamaraParameter As ObjectParameter = If(pintFolioCamara.HasValue, New ObjectParameter("pintFolioCamara", pintFolioCamara), New ObjectParameter("pintFolioCamara", GetType(Integer)))

        Dim bitCruzadaParameter As ObjectParameter = If(bitCruzada.HasValue, New ObjectParameter("bitCruzada", bitCruzada), New ObjectParameter("bitCruzada", GetType(Boolean)))

        Dim pdtmFechaImportacionParameter As ObjectParameter = If(pdtmFechaImportacion IsNot Nothing, New ObjectParameter("pdtmFechaImportacion", pdtmFechaImportacion), New ObjectParameter("pdtmFechaImportacion", GetType(String)))

        Dim pstrGestionOperacionesParameter As ObjectParameter = If(pstrGestionOperaciones IsNot Nothing, New ObjectParameter("pstrGestionOperaciones", pstrGestionOperaciones), New ObjectParameter("pstrGestionOperaciones", GetType(String)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblCRCC_CTRADES)("usp_Procesos_CruceOperaciones_Consultar", pstrNemotecnicoParameter, pintFolioBVCParameter, pintFolioCamaraParameter, bitCruzadaParameter, pdtmFechaImportacionParameter, pstrGestionOperacionesParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Procesos_CruceOperaciones_Retirar(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Procesos_CruceOperaciones_Retirar", pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Procesos_CruceOperaciones_CruceManual(pintID As Nullable(Of Integer), pintTime As Nullable(Of Integer), pstrTradeReference As String, pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pintTimeParameter As ObjectParameter = If(pintTime.HasValue, New ObjectParameter("pintTime", pintTime), New ObjectParameter("pintTime", GetType(Integer)))

        Dim pstrTradeReferenceParameter As ObjectParameter = If(pstrTradeReference IsNot Nothing, New ObjectParameter("pstrTradeReference", pstrTradeReference), New ObjectParameter("pstrTradeReference", GetType(String)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Procesos_CruceOperaciones_CruceManual", pintIDParameter, pintTimeParameter, pstrTradeReferenceParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Procesos_ValidarOrdenes_Liquidacion_InicialTimeSpread(pstrIntIdImportacionesLiq As Nullable(Of Integer), pstrTradeReference As String, pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pstrIntIdImportacionesLiqParameter As ObjectParameter = If(pstrIntIdImportacionesLiq.HasValue, New ObjectParameter("pstrIntIdImportacionesLiq", pstrIntIdImportacionesLiq), New ObjectParameter("pstrIntIdImportacionesLiq", GetType(Integer)))

        Dim pstrTradeReferenceParameter As ObjectParameter = If(pstrTradeReference IsNot Nothing, New ObjectParameter("pstrTradeReference", pstrTradeReference), New ObjectParameter("pstrTradeReference", GetType(String)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Procesos_ValidarOrdenes_Liquidacion_InicialTimeSpread", pstrIntIdImportacionesLiqParameter, pstrTradeReferenceParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Procesos_CruceOperaciones_TimeSpread_Consultar(pintOrden As String, pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblCRCC_CTRADES_TimeSpread)
        Dim pintOrdenParameter As ObjectParameter = If(pintOrden IsNot Nothing, New ObjectParameter("pintOrden", pintOrden), New ObjectParameter("pintOrden", GetType(String)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblCRCC_CTRADES_TimeSpread)("usp_Procesos_CruceOperaciones_TimeSpread_Consultar", pintOrdenParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Procesos_CruceOperaciones_TipoOperacion_TimeSpread(pstrTradeType As ObjectParameter, pstrUsuario As String, pstrInfosesion As String) As Integer
        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction("usp_Procesos_CruceOperaciones_TipoOperacion_TimeSpread", pstrTradeType, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

End Class
