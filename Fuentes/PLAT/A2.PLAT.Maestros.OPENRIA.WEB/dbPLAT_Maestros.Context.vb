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

Partial Public Class dbPLAT_MaestrosEntities
    Inherits DbContext

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property tblPais() As DbSet(Of tblPais)
    Public Overridable Property tblPais_Estado() As DbSet(Of tblPais_Estado)
    Public Overridable Property tblPais_Moneda() As DbSet(Of tblPais_Moneda)
    Public Overridable Property tblParametrizacionTributaria() As DbSet(Of tblParametrizacionTributaria)

    Public Overridable Function usp_ParametrizacionTributaria_Consultar(pintID As Nullable(Of Integer), pstrNombre As String, pstrModulo As String, pstrFuncionalidad As String, pintIDPais As Nullable(Of Integer), pintIDCiudad As Nullable(Of Integer), plogActivo As Nullable(Of Boolean), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblParametrizacionTributaria)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrNombreParameter As ObjectParameter = If(pstrNombre IsNot Nothing, New ObjectParameter("pstrNombre", pstrNombre), New ObjectParameter("pstrNombre", GetType(String)))

        Dim pstrModuloParameter As ObjectParameter = If(pstrModulo IsNot Nothing, New ObjectParameter("pstrModulo", pstrModulo), New ObjectParameter("pstrModulo", GetType(String)))

        Dim pstrFuncionalidadParameter As ObjectParameter = If(pstrFuncionalidad IsNot Nothing, New ObjectParameter("pstrFuncionalidad", pstrFuncionalidad), New ObjectParameter("pstrFuncionalidad", GetType(String)))

        Dim pintIDPaisParameter As ObjectParameter = If(pintIDPais.HasValue, New ObjectParameter("pintIDPais", pintIDPais), New ObjectParameter("pintIDPais", GetType(Integer)))

        Dim pintIDCiudadParameter As ObjectParameter = If(pintIDCiudad.HasValue, New ObjectParameter("pintIDCiudad", pintIDCiudad), New ObjectParameter("pintIDCiudad", GetType(Integer)))

        Dim plogActivoParameter As ObjectParameter = If(plogActivo.HasValue, New ObjectParameter("plogActivo", plogActivo), New ObjectParameter("plogActivo", GetType(Boolean)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblParametrizacionTributaria)("usp_ParametrizacionTributaria_Consultar", pintIDParameter, pstrNombreParameter, pstrModuloParameter, pstrFuncionalidadParameter, pintIDPaisParameter, pintIDCiudadParameter, plogActivoParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_ParametrizacionTributaria_ConsultarID(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of tblParametrizacionTributaria)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of tblParametrizacionTributaria)("usp_ParametrizacionTributaria_ConsultarID", pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_ParametrizacionTributaria_ConsultarID(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String, mergeOption As MergeOption) As ObjectResult(Of tblParametrizacionTributaria)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of tblParametrizacionTributaria)("usp_ParametrizacionTributaria_ConsultarID", mergeOption, pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_ParametrizacionTributaria_Eliminar(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_ParametrizacionTributaria_Eliminar", pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_ParametrizacionTributaria_Filtrar(pstrFiltro As String, pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblParametrizacionTributaria)
        Dim pstrFiltroParameter As ObjectParameter = If(pstrFiltro IsNot Nothing, New ObjectParameter("pstrFiltro", pstrFiltro), New ObjectParameter("pstrFiltro", GetType(String)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblParametrizacionTributaria)("usp_ParametrizacionTributaria_Filtrar", pstrFiltroParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_ParametrizacionTributaria_Validar(pintID As Nullable(Of Integer), pstrNombre As String, pdblTasaImpositiva As Nullable(Of Double), pstrModulo As String, pstrFuncionalidad As String, pintIDPais As Nullable(Of Integer), pintIDCiudad As Nullable(Of Integer), plogActivo As Nullable(Of Boolean), plogSoloValidar As Nullable(Of Boolean), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrNombreParameter As ObjectParameter = If(pstrNombre IsNot Nothing, New ObjectParameter("pstrNombre", pstrNombre), New ObjectParameter("pstrNombre", GetType(String)))

        Dim pdblTasaImpositivaParameter As ObjectParameter = If(pdblTasaImpositiva.HasValue, New ObjectParameter("pdblTasaImpositiva", pdblTasaImpositiva), New ObjectParameter("pdblTasaImpositiva", GetType(Double)))

        Dim pstrModuloParameter As ObjectParameter = If(pstrModulo IsNot Nothing, New ObjectParameter("pstrModulo", pstrModulo), New ObjectParameter("pstrModulo", GetType(String)))

        Dim pstrFuncionalidadParameter As ObjectParameter = If(pstrFuncionalidad IsNot Nothing, New ObjectParameter("pstrFuncionalidad", pstrFuncionalidad), New ObjectParameter("pstrFuncionalidad", GetType(String)))

        Dim pintIDPaisParameter As ObjectParameter = If(pintIDPais.HasValue, New ObjectParameter("pintIDPais", pintIDPais), New ObjectParameter("pintIDPais", GetType(Integer)))

        Dim pintIDCiudadParameter As ObjectParameter = If(pintIDCiudad.HasValue, New ObjectParameter("pintIDCiudad", pintIDCiudad), New ObjectParameter("pintIDCiudad", GetType(Integer)))

        Dim plogActivoParameter As ObjectParameter = If(plogActivo.HasValue, New ObjectParameter("plogActivo", plogActivo), New ObjectParameter("plogActivo", GetType(Boolean)))

        Dim plogSoloValidarParameter As ObjectParameter = If(plogSoloValidar.HasValue, New ObjectParameter("plogSoloValidar", plogSoloValidar), New ObjectParameter("plogSoloValidar", GetType(Boolean)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_ParametrizacionTributaria_Validar", pintIDParameter, pstrNombreParameter, pdblTasaImpositivaParameter, pstrModuloParameter, pstrFuncionalidadParameter, pintIDPaisParameter, pintIDCiudadParameter, plogActivoParameter, plogSoloValidarParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Procesos_Utilitarios_CargarCombos(pstrProducto As String, pstrCondicionTexto1 As String, pstrCondicionTexto2 As String, pstrCondicionEntero1 As Nullable(Of Integer), pstrCondicionEntero2 As Nullable(Of Integer), plogCrearTablaTemporal As Nullable(Of Boolean), pstrUsuario As String, pstrInfosesion As String) As Integer
        Dim pstrProductoParameter As ObjectParameter = If(pstrProducto IsNot Nothing, New ObjectParameter("pstrProducto", pstrProducto), New ObjectParameter("pstrProducto", GetType(String)))

        Dim pstrCondicionTexto1Parameter As ObjectParameter = If(pstrCondicionTexto1 IsNot Nothing, New ObjectParameter("pstrCondicionTexto1", pstrCondicionTexto1), New ObjectParameter("pstrCondicionTexto1", GetType(String)))

        Dim pstrCondicionTexto2Parameter As ObjectParameter = If(pstrCondicionTexto2 IsNot Nothing, New ObjectParameter("pstrCondicionTexto2", pstrCondicionTexto2), New ObjectParameter("pstrCondicionTexto2", GetType(String)))

        Dim pstrCondicionEntero1Parameter As ObjectParameter = If(pstrCondicionEntero1.HasValue, New ObjectParameter("pstrCondicionEntero1", pstrCondicionEntero1), New ObjectParameter("pstrCondicionEntero1", GetType(Integer)))

        Dim pstrCondicionEntero2Parameter As ObjectParameter = If(pstrCondicionEntero2.HasValue, New ObjectParameter("pstrCondicionEntero2", pstrCondicionEntero2), New ObjectParameter("pstrCondicionEntero2", GetType(Integer)))

        Dim plogCrearTablaTemporalParameter As ObjectParameter = If(plogCrearTablaTemporal.HasValue, New ObjectParameter("plogCrearTablaTemporal", plogCrearTablaTemporal), New ObjectParameter("plogCrearTablaTemporal", GetType(Boolean)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction("usp_Procesos_Utilitarios_CargarCombos", pstrProductoParameter, pstrCondicionTexto1Parameter, pstrCondicionTexto2Parameter, pstrCondicionEntero1Parameter, pstrCondicionEntero2Parameter, plogCrearTablaTemporalParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Consultar(pintID As Nullable(Of Integer), pstrNombre As String, pstrCodigoISOAlfa2 As String, pstrCodigoISOAlfa3 As String, pstrCodigoISONumerico As String, plogActivo As Nullable(Of Boolean), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblPais)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrNombreParameter As ObjectParameter = If(pstrNombre IsNot Nothing, New ObjectParameter("pstrNombre", pstrNombre), New ObjectParameter("pstrNombre", GetType(String)))

        Dim pstrCodigoISOAlfa2Parameter As ObjectParameter = If(pstrCodigoISOAlfa2 IsNot Nothing, New ObjectParameter("pstrCodigoISOAlfa2", pstrCodigoISOAlfa2), New ObjectParameter("pstrCodigoISOAlfa2", GetType(String)))

        Dim pstrCodigoISOAlfa3Parameter As ObjectParameter = If(pstrCodigoISOAlfa3 IsNot Nothing, New ObjectParameter("pstrCodigoISOAlfa3", pstrCodigoISOAlfa3), New ObjectParameter("pstrCodigoISOAlfa3", GetType(String)))

        Dim pstrCodigoISONumericoParameter As ObjectParameter = If(pstrCodigoISONumerico IsNot Nothing, New ObjectParameter("pstrCodigoISONumerico", pstrCodigoISONumerico), New ObjectParameter("pstrCodigoISONumerico", GetType(String)))

        Dim plogActivoParameter As ObjectParameter = If(plogActivo.HasValue, New ObjectParameter("plogActivo", plogActivo), New ObjectParameter("plogActivo", GetType(Boolean)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblPais)("usp_Pais_Consultar", pintIDParameter, pstrNombreParameter, pstrCodigoISOAlfa2Parameter, pstrCodigoISOAlfa3Parameter, pstrCodigoISONumericoParameter, plogActivoParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_ConsultarID(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of tblPais)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of tblPais)("usp_Pais_ConsultarID", pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_ConsultarID(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String, mergeOption As MergeOption) As ObjectResult(Of tblPais)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of tblPais)("usp_Pais_ConsultarID", mergeOption, pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Eliminar(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Pais_Eliminar", pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Estado_Consultar(pintIDPais As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblPais_Estado)
        Dim pintIDPaisParameter As ObjectParameter = If(pintIDPais.HasValue, New ObjectParameter("pintIDPais", pintIDPais), New ObjectParameter("pintIDPais", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblPais_Estado)("usp_Pais_Estado_Consultar", pintIDPaisParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Estado_ConsultarID(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of tblPais_Estado)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of tblPais_Estado)("usp_Pais_Estado_ConsultarID", pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Estado_ConsultarID(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String, mergeOption As MergeOption) As ObjectResult(Of tblPais_Estado)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of tblPais_Estado)("usp_Pais_Estado_ConsultarID", mergeOption, pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Estado_Eliminar(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Pais_Estado_Eliminar", pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Estado_Validar(pintID As Nullable(Of Integer), pstrCodigo As String, pstrNombre As String, pintIDPais As Nullable(Of Integer), plogSoloValidar As Nullable(Of Boolean), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrCodigoParameter As ObjectParameter = If(pstrCodigo IsNot Nothing, New ObjectParameter("pstrCodigo", pstrCodigo), New ObjectParameter("pstrCodigo", GetType(String)))

        Dim pstrNombreParameter As ObjectParameter = If(pstrNombre IsNot Nothing, New ObjectParameter("pstrNombre", pstrNombre), New ObjectParameter("pstrNombre", GetType(String)))

        Dim pintIDPaisParameter As ObjectParameter = If(pintIDPais.HasValue, New ObjectParameter("pintIDPais", pintIDPais), New ObjectParameter("pintIDPais", GetType(Integer)))

        Dim plogSoloValidarParameter As ObjectParameter = If(plogSoloValidar.HasValue, New ObjectParameter("plogSoloValidar", plogSoloValidar), New ObjectParameter("plogSoloValidar", GetType(Boolean)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Pais_Estado_Validar", pintIDParameter, pstrCodigoParameter, pstrNombreParameter, pintIDPaisParameter, plogSoloValidarParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Filtrar(pstrFiltro As String, pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblPais)
        Dim pstrFiltroParameter As ObjectParameter = If(pstrFiltro IsNot Nothing, New ObjectParameter("pstrFiltro", pstrFiltro), New ObjectParameter("pstrFiltro", GetType(String)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblPais)("usp_Pais_Filtrar", pstrFiltroParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Moneda_Consultar(pintIDPais As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblPais_Moneda)
        Dim pintIDPaisParameter As ObjectParameter = If(pintIDPais.HasValue, New ObjectParameter("pintIDPais", pintIDPais), New ObjectParameter("pintIDPais", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblPais_Moneda)("usp_Pais_Moneda_Consultar", pintIDPaisParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Moneda_ConsultarID(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of tblPais_Moneda)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of tblPais_Moneda)("usp_Pais_Moneda_ConsultarID", pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Moneda_ConsultarID(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String, mergeOption As MergeOption) As ObjectResult(Of tblPais_Moneda)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of tblPais_Moneda)("usp_Pais_Moneda_ConsultarID", mergeOption, pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Moneda_Eliminar(pintID As Nullable(Of Integer), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Pais_Moneda_Eliminar", pintIDParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Moneda_Validar(pintID As Nullable(Of Integer), pstrCodigoMoneda As String, pintIDPais As Nullable(Of Integer), plogSoloValidar As Nullable(Of Boolean), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrCodigoMonedaParameter As ObjectParameter = If(pstrCodigoMoneda IsNot Nothing, New ObjectParameter("pstrCodigoMoneda", pstrCodigoMoneda), New ObjectParameter("pstrCodigoMoneda", GetType(String)))

        Dim pintIDPaisParameter As ObjectParameter = If(pintIDPais.HasValue, New ObjectParameter("pintIDPais", pintIDPais), New ObjectParameter("pintIDPais", GetType(Integer)))

        Dim plogSoloValidarParameter As ObjectParameter = If(plogSoloValidar.HasValue, New ObjectParameter("plogSoloValidar", plogSoloValidar), New ObjectParameter("plogSoloValidar", GetType(Boolean)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Pais_Moneda_Validar", pintIDParameter, pstrCodigoMonedaParameter, pintIDPaisParameter, plogSoloValidarParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

    Public Overridable Function usp_Pais_Validar(pintID As Nullable(Of Integer), pstrNombre As String, pstrCodigoISOAlfa2 As String, pstrCodigoISOAlfa3 As String, pstrCodigoISONumerico As String, plogActivo As Nullable(Of Boolean), plogSoloValidar As Nullable(Of Boolean), pstrUsuario As String, pstrInfosesion As String) As ObjectResult(Of CPX_tblValidacionesGenerales)
        Dim pintIDParameter As ObjectParameter = If(pintID.HasValue, New ObjectParameter("pintID", pintID), New ObjectParameter("pintID", GetType(Integer)))

        Dim pstrNombreParameter As ObjectParameter = If(pstrNombre IsNot Nothing, New ObjectParameter("pstrNombre", pstrNombre), New ObjectParameter("pstrNombre", GetType(String)))

        Dim pstrCodigoISOAlfa2Parameter As ObjectParameter = If(pstrCodigoISOAlfa2 IsNot Nothing, New ObjectParameter("pstrCodigoISOAlfa2", pstrCodigoISOAlfa2), New ObjectParameter("pstrCodigoISOAlfa2", GetType(String)))

        Dim pstrCodigoISOAlfa3Parameter As ObjectParameter = If(pstrCodigoISOAlfa3 IsNot Nothing, New ObjectParameter("pstrCodigoISOAlfa3", pstrCodigoISOAlfa3), New ObjectParameter("pstrCodigoISOAlfa3", GetType(String)))

        Dim pstrCodigoISONumericoParameter As ObjectParameter = If(pstrCodigoISONumerico IsNot Nothing, New ObjectParameter("pstrCodigoISONumerico", pstrCodigoISONumerico), New ObjectParameter("pstrCodigoISONumerico", GetType(String)))

        Dim plogActivoParameter As ObjectParameter = If(plogActivo.HasValue, New ObjectParameter("plogActivo", plogActivo), New ObjectParameter("plogActivo", GetType(Boolean)))

        Dim plogSoloValidarParameter As ObjectParameter = If(plogSoloValidar.HasValue, New ObjectParameter("plogSoloValidar", plogSoloValidar), New ObjectParameter("plogSoloValidar", GetType(Boolean)))

        Dim pstrUsuarioParameter As ObjectParameter = If(pstrUsuario IsNot Nothing, New ObjectParameter("pstrUsuario", pstrUsuario), New ObjectParameter("pstrUsuario", GetType(String)))

        Dim pstrInfosesionParameter As ObjectParameter = If(pstrInfosesion IsNot Nothing, New ObjectParameter("pstrInfosesion", pstrInfosesion), New ObjectParameter("pstrInfosesion", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of CPX_tblValidacionesGenerales)("usp_Pais_Validar", pintIDParameter, pstrNombreParameter, pstrCodigoISOAlfa2Parameter, pstrCodigoISOAlfa3Parameter, pstrCodigoISONumericoParameter, plogActivoParameter, plogSoloValidarParameter, pstrUsuarioParameter, pstrInfosesionParameter)
    End Function

End Class
