﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Reflection


<Global.System.Data.Linq.Mapping.DatabaseAttribute(Name:="dbA2Utilidades_Net_Desarrollo")>  _
Partial Public Class RiesgosModelDataContext
	Inherits System.Data.Linq.DataContext
	
	Private Shared mappingSource As System.Data.Linq.Mapping.MappingSource = New AttributeMappingSource()
	
  #Region "Extensibility Method Definitions"
  Partial Private Sub OnCreated()
  End Sub
  Partial Private Sub InsertItemCombo(instance As ItemCombo)
    End Sub
  Partial Private Sub UpdateItemCombo(instance As ItemCombo)
    End Sub
  Partial Private Sub DeleteItemCombo(instance As ItemCombo)
    End Sub
  Partial Private Sub InsertAlertas(instance As Alertas)
    End Sub
  Partial Private Sub UpdateAlertas(instance As Alertas)
    End Sub
  Partial Private Sub DeleteAlertas(instance As Alertas)
    End Sub
  Partial Private Sub InsertToolbarsPorAplicacion(instance As ToolbarsPorAplicacion)
    End Sub
  Partial Private Sub UpdateToolbarsPorAplicacion(instance As ToolbarsPorAplicacion)
    End Sub
  Partial Private Sub DeleteToolbarsPorAplicacion(instance As ToolbarsPorAplicacion)
    End Sub
  #End Region
	
	Public Sub New(ByVal connection As String)
		MyBase.New(connection, mappingSource)
		OnCreated
	End Sub
	
	Public Sub New(ByVal connection As System.Data.IDbConnection)
		MyBase.New(connection, mappingSource)
		OnCreated
	End Sub
	
	Public Sub New(ByVal connection As String, ByVal mappingSource As System.Data.Linq.Mapping.MappingSource)
		MyBase.New(connection, mappingSource)
		OnCreated
	End Sub
	
	Public Sub New(ByVal connection As System.Data.IDbConnection, ByVal mappingSource As System.Data.Linq.Mapping.MappingSource)
		MyBase.New(connection, mappingSource)
		OnCreated
	End Sub
	
	Public ReadOnly Property ItemCombos() As System.Data.Linq.Table(Of ItemCombo)
		Get
			Return Me.GetTable(Of ItemCombo)
		End Get
	End Property
	
	Public ReadOnly Property Consultas() As System.Data.Linq.Table(Of Consultas)
		Get
			Return Me.GetTable(Of Consultas)
		End Get
	End Property
	
	Public ReadOnly Property Alertas() As System.Data.Linq.Table(Of Alertas)
		Get
			Return Me.GetTable(Of Alertas)
		End Get
	End Property
	
	Public ReadOnly Property ToolbarsPorAplicacions() As System.Data.Linq.Table(Of ToolbarsPorAplicacion)
		Get
			Return Me.GetTable(Of ToolbarsPorAplicacion)
		End Get
	End Property

    <Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="MC.usp_MC_Consultas_Actualizar")>
    Public Function usp_MC_Consultas_Actualizar(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int")> ByRef pintIDConsultas As System.Nullable(Of Integer), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Varchar(100) NOT NULL")> ByVal pstrConsulta As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Varchar(400) NOT NULL")> ByVal pstrProcedimiento As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrUsuario As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrInfoSesion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(8000)")> ByRef pstrMsgValidacion As String) As Integer
		Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod,MethodInfo), pintIDConsultas, pstrConsulta, pstrProcedimiento, pstrUsuario, pstrInfoSesion, pintErrorPersonalizado, pstrMsgValidacion)
		pintIDConsultas = CType(result.GetParameterValue(0),System.Nullable(Of Integer))
		pstrMsgValidacion = CType(result.GetParameterValue(6),String)
		Return CType(result.ReturnValue,Integer)
	End Function
	
	<Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="MC.usp_MC_Consultas_Eliminar")>  _
	Public Function usp_MC_Consultas_Eliminar(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int")> ByVal pintIDConsultas As System.Nullable(Of Integer), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(100)")> ByVal pstrUsuario As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrInfoSesion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="varchar(8000)")> ByRef pstrMsgValidacion As String) As Integer
		Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod,MethodInfo), pintIDConsultas, pstrUsuario, pstrInfoSesion, pintErrorPersonalizado, pstrMsgValidacion)
		pstrMsgValidacion = CType(result.GetParameterValue(4),String)
		Return CType(result.ReturnValue,Integer)
	End Function
	
	<Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="MC.usp_MC_Consultas_Filtrar")>  _
	Public Function usp_MC_Consultas_Filtrar(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(30)")> ByVal pstrFiltro As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(100)")> ByVal pstrUsuario As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrInfoSesion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte)) As ISingleResult(Of Consultas)
		Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod,MethodInfo), pstrFiltro, pstrUsuario, pstrInfoSesion, pintErrorPersonalizado)
		Return CType(result.ReturnValue,ISingleResult(Of Consultas))
	End Function
	
	<Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="MC.usp_MC_Consultas_Consultar")>  _
	Public Function usp_MC_Consultas_Consultar(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(20)")> ByVal pstrAccion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Varchar(100) NOT NULL")> ByVal pstrConsulta As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Varchar(400) NOT NULL")> ByVal pstrProcedimiento As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(100)")> ByVal pstrUsuario As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrInfoSesion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte)) As ISingleResult(Of Consultas)
		Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod,MethodInfo), pstrAccion, pstrConsulta, pstrProcedimiento, pstrUsuario, pstrInfoSesion, pintErrorPersonalizado)
		Return CType(result.ReturnValue,ISingleResult(Of Consultas))
	End Function
	
	<Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="MC.usp_MC_Alertas_Consultar")>  _
	Public Function usp_MC_Alertas_Consultar(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="DateTime")> ByVal pdtmFecha As System.Nullable(Of Date), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrUsuario As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrInfoSesion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte)) As ISingleResult(Of Alertas)
		Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod,MethodInfo), pdtmFecha, pstrUsuario, pstrInfoSesion, pintErrorPersonalizado)
		Return CType(result.ReturnValue,ISingleResult(Of Alertas))
	End Function
	
	<Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="dbo.uspA2CA_consultarToolbarsUsuario")>  _
	Public Function uspA2CA_consultarToolbarsUsuario( _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrAplicacion As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(20)")> ByVal pstrVersion As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(80)")> ByVal pstrUsuario As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(50)")> ByVal pstrClave As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(5000)")> ByVal pstrToolbar As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(5)")> ByVal pstrSeparador As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Bit")> ByVal plogRetornarPredeterminada As System.Nullable(Of Boolean),  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(50)")> ByVal pstrServidor As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(50)")> ByVal pstrMaquina As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(50)")> ByVal pstrDirIPMaquina As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(250)")> ByVal pstrBrowser As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int")> ByVal pintIdSesion As System.Nullable(Of Integer),  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(40)")> ByVal pstrDivision As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(80)")> ByVal pstrUsrImpersonalizado As String,  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte),  _
				<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Bit")> ByVal plogRegistrarLogin As System.Nullable(Of Boolean)) As ISingleResult(Of ToolbarsPorAplicacion)
		Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod,MethodInfo), pstrAplicacion, pstrVersion, pstrUsuario, pstrClave, pstrToolbar, pstrSeparador, plogRetornarPredeterminada, pstrServidor, pstrMaquina, pstrDirIPMaquina, pstrBrowser, pintIdSesion, pstrDivision, pstrUsrImpersonalizado, pintErrorPersonalizado, plogRegistrarLogin)
		Return CType(result.ReturnValue,ISingleResult(Of ToolbarsPorAplicacion))
	End Function
	
	<Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="dbo.uspA2utils_CargarCombos")>  _
	Public Function uspA2utils_CargarCombos(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(500)")> ByVal pstrListaNombreCombos As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrUsuario As String) As ISingleResult(Of ItemCombo)
		Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod,MethodInfo), pstrListaNombreCombos, pstrUsuario)
		Return CType(result.ReturnValue,ISingleResult(Of ItemCombo))
	End Function
	
	<Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="MC.usp_MC_Riesgo_Insertar")>  _
	Public Function usp_MC_Riesgo_Insertar(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrAplicacion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrVersion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(50)")> ByVal pstrTitulo As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(30)")> ByVal pstrNombre As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(30)")> ByVal pstrTooltip As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(500)")> ByVal pstrRoles As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(255)")> ByVal pstrDescripcion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(30)")> ByVal pstrNombrePadre As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(25)")> ByVal pstrNombreTipoObjeto As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrUsuario As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrInfoSesion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(8000)")> ByRef pstrMsgValidacion As String) As ISingleResult(Of usp_MC_Riesgo_InsertarResult)
		Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod,MethodInfo), pstrAplicacion, pstrVersion, pstrTitulo, pstrNombre, pstrTooltip, pstrRoles, pstrDescripcion, pstrNombrePadre, pstrNombreTipoObjeto, pstrUsuario, pstrInfoSesion, pintErrorPersonalizado, pstrMsgValidacion)
		pstrMsgValidacion = CType(result.GetParameterValue(12),String)
		Return CType(result.ReturnValue,ISingleResult(Of usp_MC_Riesgo_InsertarResult))
	End Function
	
	<Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="MC.usp_MC_Riesgo_Eliminar")>  _
	Public Function usp_MC_Riesgo_Eliminar(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrAplicacion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrVersion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(50)")> ByVal pstrTitulo As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrUsuario As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrInfoSesion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(8000)")> ByRef pstrMsgValidacion As String) As Integer
		Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod,MethodInfo), pstrAplicacion, pstrVersion, pstrTitulo, pstrUsuario, pstrInfoSesion, pintErrorPersonalizado, pstrMsgValidacion)
		pstrMsgValidacion = CType(result.GetParameterValue(6),String)
		Return CType(result.ReturnValue,Integer)
	End Function
End Class

<Global.System.Data.Linq.Mapping.TableAttribute(Name:="")>  _
Partial Public Class ItemCombo
	Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	
	Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)
	
	Private _Categoria As String
	
	Private _ID As String
	
	Private _Descripcion As String
	
	Private _intID As System.Nullable(Of System.Int32)
	
	Private _Retorno As String
	
    #Region "Extensibility Method Definitions"
    Partial Private Sub OnLoaded()
    End Sub
    Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
    End Sub
    Partial Private Sub OnCreated()
    End Sub
    Partial Private Sub OnCategoriaChanging(value As String)
    End Sub
    Partial Private Sub OnCategoriaChanged()
    End Sub
    Partial Private Sub OnIDChanging(value As String)
    End Sub
    Partial Private Sub OnIDChanged()
    End Sub
    Partial Private Sub OnDescripcionChanging(value As String)
    End Sub
    Partial Private Sub OnDescripcionChanged()
    End Sub
    Partial Private Sub OnintIDChanging(value As System.Nullable(Of System.Int32))
    End Sub
    Partial Private Sub OnintIDChanged()
    End Sub
    Partial Private Sub OnRetornoChanging(value As String)
    End Sub
    Partial Private Sub OnRetornoChanged()
    End Sub
    #End Region
	
	Public Sub New()
		MyBase.New
		OnCreated
	End Sub
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Categoria", CanBeNull:=false, IsPrimaryKey:=true)>  _
	Public Property Categoria() As String
		Get
			Return Me._Categoria
		End Get
		Set
			If (String.Equals(Me._Categoria, value) = false) Then
				Me.OnCategoriaChanging(value)
				Me.SendPropertyChanging
				Me._Categoria = value
				Me.SendPropertyChanged("Categoria")
				Me.OnCategoriaChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_ID", CanBeNull:=false, IsPrimaryKey:=true)>  _
	Public Property ID() As String
		Get
			Return Me._ID
		End Get
		Set
			If (String.Equals(Me._ID, value) = false) Then
				Me.OnIDChanging(value)
				Me.SendPropertyChanging
				Me._ID = value
				Me.SendPropertyChanged("ID")
				Me.OnIDChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Descripcion", CanBeNull:=false)>  _
	Public Property Descripcion() As String
		Get
			Return Me._Descripcion
		End Get
		Set
			If (String.Equals(Me._Descripcion, value) = false) Then
				Me.OnDescripcionChanging(value)
				Me.SendPropertyChanging
				Me._Descripcion = value
				Me.SendPropertyChanged("Descripcion")
				Me.OnDescripcionChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intID", CanBeNull:=true)>  _
	Public Property intID() As System.Nullable(Of System.Int32)
		Get
			Return Me._intID
		End Get
		Set
			If (Object.Equals(Me._intID, value) = false) Then
				Me.OnintIDChanging(value)
				Me.SendPropertyChanging
				Me._intID = value
				Me.SendPropertyChanged("intID")
				Me.OnintIDChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Retorno")>  _
	Public Property Retorno() As String
		Get
			Return Me._Retorno
		End Get
		Set
			If (String.Equals(Me._Retorno, value) = false) Then
				Me.OnRetornoChanging(value)
				Me.SendPropertyChanging
				Me._Retorno = value
				Me.SendPropertyChanged("Retorno")
				Me.OnRetornoChanged
			End If
		End Set
	End Property
	
	Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging
	
	Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
	
	Protected Overridable Sub SendPropertyChanging()
		If ((Me.PropertyChangingEvent Is Nothing)  _
					= false) Then
			RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
		End If
	End Sub
	
	Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
		If ((Me.PropertyChangedEvent Is Nothing)  _
					= false) Then
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End If
	End Sub
End Class

<Global.System.Data.Linq.Mapping.TableAttribute(Name:="MC.tblConsultas")>  _
Partial Public Class Consultas
	Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	
	Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)
	
	Private _intIDConsultas As Integer
	
	Private _strConsulta As String
	
	Private _strProcedimiento As String
	
	Private _dtmActualizacion As System.Nullable(of System.DateTime)
	
	Private _strUsuario As String
	
	Private _strInfoSesion As String
	
	Private _strMsgValidacion As String
	
	Private _pstrUsuarioConexion As String
	
	Private _pstrInfoConexion As String
	
    #Region "Extensibility Method Definitions"
    Partial Private Sub OnLoaded()
    End Sub
    Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
    End Sub
    Partial Private Sub OnCreated()
    End Sub
    Partial Private Sub OnintIDConsultasChanging(value As Integer)
    End Sub
    Partial Private Sub OnintIDConsultasChanged()
    End Sub
    Partial Private Sub OnstrConsultaChanging(value As String)
    End Sub
    Partial Private Sub OnstrConsultaChanged()
    End Sub
    Partial Private Sub OnstrProcedimientoChanging(value As String)
    End Sub
    Partial Private Sub OnstrProcedimientoChanged()
    End Sub
    Partial Private Sub OndtmActualizacionChanging(value As System.Nullable(of System.DateTime))
    End Sub
    Partial Private Sub OndtmActualizacionChanged()
    End Sub
    Partial Private Sub OnstrUsuarioChanging(value As String)
    End Sub
    Partial Private Sub OnstrUsuarioChanged()
    End Sub
    Partial Private Sub OnstrInfoSesionChanging(value As String)
    End Sub
    Partial Private Sub OnstrInfoSesionChanged()
    End Sub
    Partial Private Sub OnstrMsgValidacionChanging(value As String)
    End Sub
    Partial Private Sub OnstrMsgValidacionChanged()
    End Sub
    Partial Private Sub OnpstrUsuarioConexionChanging(value As String)
    End Sub
    Partial Private Sub OnpstrUsuarioConexionChanged()
    End Sub
    Partial Private Sub OnpstrInfoConexionChanging(value As String)
    End Sub
    Partial Private Sub OnpstrInfoConexionChanged()
    End Sub
    #End Region
	
	Public Sub New()
		MyBase.New
		OnCreated
	End Sub
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intIDConsultas", AutoSync:=AutoSync.OnInsert, DbType:="Int NOT NULL IDENTITY", IsPrimaryKey:=true, IsDbGenerated:=true)>  _
	Public Property intIDConsultas() As Integer
		Get
			Return Me._intIDConsultas
		End Get
		Set
			If ((Me._intIDConsultas = value)  _
						= false) Then
				Me.OnintIDConsultasChanging(value)
				Me.SendPropertyChanging
				Me._intIDConsultas = value
				Me.SendPropertyChanged("intIDConsultas")
				Me.OnintIDConsultasChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strConsulta", DbType:="Varchar(100) NOT NULL")>  _
	Public Property strConsulta() As String
		Get
			Return Me._strConsulta
		End Get
		Set
			If (String.Equals(Me._strConsulta, value) = false) Then
				Me.OnstrConsultaChanging(value)
				Me.SendPropertyChanging
				Me._strConsulta = value
				Me.SendPropertyChanged("strConsulta")
				Me.OnstrConsultaChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strProcedimiento", DbType:="Varchar(400) NOT NULL")>  _
	Public Property strProcedimiento() As String
		Get
			Return Me._strProcedimiento
		End Get
		Set
			If (String.Equals(Me._strProcedimiento, value) = false) Then
				Me.OnstrProcedimientoChanging(value)
				Me.SendPropertyChanging
				Me._strProcedimiento = value
				Me.SendPropertyChanged("strProcedimiento")
				Me.OnstrProcedimientoChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_dtmActualizacion", DbType:="Datetime NOT NULL", CanBeNull:=false)>  _
	Public Property dtmActualizacion() As System.Nullable(of System.DateTime)
		Get
			Return Me._dtmActualizacion
		End Get
		Set
			If (Object.Equals(Me._dtmActualizacion, value) = false) Then
				Me.OndtmActualizacionChanging(value)
				Me.SendPropertyChanging
				Me._dtmActualizacion = value
				Me.SendPropertyChanged("dtmActualizacion")
				Me.OndtmActualizacionChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strUsuario", DbType:="Varchar(60) NOT NULL")>  _
	Public Property strUsuario() As String
		Get
			Return Me._strUsuario
		End Get
		Set
			If (String.Equals(Me._strUsuario, value) = false) Then
				Me.OnstrUsuarioChanging(value)
				Me.SendPropertyChanging
				Me._strUsuario = value
				Me.SendPropertyChanged("strUsuario")
				Me.OnstrUsuarioChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strInfoSesion", DbType:="Varchar(max) NULL")>  _
	Public Property strInfoSesion() As String
		Get
			Return Me._strInfoSesion
		End Get
		Set
			If (String.Equals(Me._strInfoSesion, value) = false) Then
				Me.OnstrInfoSesionChanging(value)
				Me.SendPropertyChanging
				Me._strInfoSesion = value
				Me.SendPropertyChanged("strInfoSesion")
				Me.OnstrInfoSesionChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strMsgValidacion", DbType:="Varchar(8000)")>  _
	Public Property strMsgValidacion() As String
		Get
			Return Me._strMsgValidacion
		End Get
		Set
			If (String.Equals(Me._strMsgValidacion, value) = false) Then
				Me.OnstrMsgValidacionChanging(value)
				Me.SendPropertyChanging
				Me._strMsgValidacion = value
				Me.SendPropertyChanged("strMsgValidacion")
				Me.OnstrMsgValidacionChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_pstrUsuarioConexion", DbType:="Varchar(100)")>  _
	Public Property pstrUsuarioConexion() As String
		Get
			Return Me._pstrUsuarioConexion
		End Get
		Set
			If (String.Equals(Me._pstrUsuarioConexion, value) = false) Then
				Me.OnpstrUsuarioConexionChanging(value)
				Me.SendPropertyChanging
				Me._pstrUsuarioConexion = value
				Me.SendPropertyChanged("pstrUsuarioConexion")
				Me.OnpstrUsuarioConexionChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_pstrInfoConexion", DbType:="Varchar(8000)")>  _
	Public Property pstrInfoConexion() As String
		Get
			Return Me._pstrInfoConexion
		End Get
		Set
			If (String.Equals(Me._pstrInfoConexion, value) = false) Then
				Me.OnpstrInfoConexionChanging(value)
				Me.SendPropertyChanging
				Me._pstrInfoConexion = value
				Me.SendPropertyChanged("pstrInfoConexion")
				Me.OnpstrInfoConexionChanged
			End If
		End Set
	End Property
	
	Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging
	
	Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
	
	Protected Overridable Sub SendPropertyChanging()
		If ((Me.PropertyChangingEvent Is Nothing)  _
					= false) Then
			RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
		End If
	End Sub
	
	Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
		If ((Me.PropertyChangedEvent Is Nothing)  _
					= false) Then
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End If
	End Sub
End Class

<Global.System.Data.Linq.Mapping.TableAttribute(Name:="MC.tblAlertas")>  _
Partial Public Class Alertas
	Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	
	Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)
	
	Private _intIDAlertas As Integer
	
	Private _strMetodo As String
	
	Private _strConsulta As String
	
	Private _strValorAnterior As String
	
	Private _strValorNuevo As String
	
	Private _strTipoAlerta As String
	
	Private _strDestinatarios As String
	
	Private _strAlerta As String
	
	Private _dtmActualizacion As Date
	
	Private _strUsuario As String
	
    #Region "Extensibility Method Definitions"
    Partial Private Sub OnLoaded()
    End Sub
    Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
    End Sub
    Partial Private Sub OnCreated()
    End Sub
    Partial Private Sub OnintIDAlertasChanging(value As Integer)
    End Sub
    Partial Private Sub OnintIDAlertasChanged()
    End Sub
    Partial Private Sub OnstrMetodoChanging(value As String)
    End Sub
    Partial Private Sub OnstrMetodoChanged()
    End Sub
    Partial Private Sub OnstrConsultaChanging(value As String)
    End Sub
    Partial Private Sub OnstrConsultaChanged()
    End Sub
    Partial Private Sub OnstrValorAnteriorChanging(value As String)
    End Sub
    Partial Private Sub OnstrValorAnteriorChanged()
    End Sub
    Partial Private Sub OnstrValorNuevoChanging(value As String)
    End Sub
    Partial Private Sub OnstrValorNuevoChanged()
    End Sub
    Partial Private Sub OnstrTipoAlertaChanging(value As String)
    End Sub
    Partial Private Sub OnstrTipoAlertaChanged()
    End Sub
    Partial Private Sub OnstrDestinatariosChanging(value As String)
    End Sub
    Partial Private Sub OnstrDestinatariosChanged()
    End Sub
    Partial Private Sub OnstrAlertaChanging(value As String)
    End Sub
    Partial Private Sub OnstrAlertaChanged()
    End Sub
    Partial Private Sub OndtmActualizacionChanging(value As Date)
    End Sub
    Partial Private Sub OndtmActualizacionChanged()
    End Sub
    Partial Private Sub OnstrUsuarioChanging(value As String)
    End Sub
    Partial Private Sub OnstrUsuarioChanged()
    End Sub
    #End Region
	
	Public Sub New()
		MyBase.New
		OnCreated
	End Sub
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_intIDAlertas", AutoSync:=AutoSync.OnInsert, DbType:="Int NOT NULL IDENTITY", IsPrimaryKey:=true, IsDbGenerated:=true)>  _
	Public Property intIDAlertas() As Integer
		Get
			Return Me._intIDAlertas
		End Get
		Set
			If ((Me._intIDAlertas = value)  _
						= false) Then
				Me.OnintIDAlertasChanging(value)
				Me.SendPropertyChanging
				Me._intIDAlertas = value
				Me.SendPropertyChanged("intIDAlertas")
				Me.OnintIDAlertasChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strMetodo", DbType:="VarChar(100) NOT NULL", CanBeNull:=false)>  _
	Public Property strMetodo() As String
		Get
			Return Me._strMetodo
		End Get
		Set
			If (String.Equals(Me._strMetodo, value) = false) Then
				Me.OnstrMetodoChanging(value)
				Me.SendPropertyChanging
				Me._strMetodo = value
				Me.SendPropertyChanged("strMetodo")
				Me.OnstrMetodoChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strConsulta", DbType:="VarChar(100) NOT NULL", CanBeNull:=false)>  _
	Public Property strConsulta() As String
		Get
			Return Me._strConsulta
		End Get
		Set
			If (String.Equals(Me._strConsulta, value) = false) Then
				Me.OnstrConsultaChanging(value)
				Me.SendPropertyChanging
				Me._strConsulta = value
				Me.SendPropertyChanged("strConsulta")
				Me.OnstrConsultaChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strValorAnterior", DbType:="VarChar(400) NOT NULL", CanBeNull:=false)>  _
	Public Property strValorAnterior() As String
		Get
			Return Me._strValorAnterior
		End Get
		Set
			If (String.Equals(Me._strValorAnterior, value) = false) Then
				Me.OnstrValorAnteriorChanging(value)
				Me.SendPropertyChanging
				Me._strValorAnterior = value
				Me.SendPropertyChanged("strValorAnterior")
				Me.OnstrValorAnteriorChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strValorNuevo", DbType:="VarChar(400) NOT NULL", CanBeNull:=false)>  _
	Public Property strValorNuevo() As String
		Get
			Return Me._strValorNuevo
		End Get
		Set
			If (String.Equals(Me._strValorNuevo, value) = false) Then
				Me.OnstrValorNuevoChanging(value)
				Me.SendPropertyChanging
				Me._strValorNuevo = value
				Me.SendPropertyChanged("strValorNuevo")
				Me.OnstrValorNuevoChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strTipoAlerta", DbType:="VarChar(60) NOT NULL", CanBeNull:=false)>  _
	Public Property strTipoAlerta() As String
		Get
			Return Me._strTipoAlerta
		End Get
		Set
			If (String.Equals(Me._strTipoAlerta, value) = false) Then
				Me.OnstrTipoAlertaChanging(value)
				Me.SendPropertyChanging
				Me._strTipoAlerta = value
				Me.SendPropertyChanged("strTipoAlerta")
				Me.OnstrTipoAlertaChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strDestinatarios", DbType:="VarChar(400) NOT NULL", CanBeNull:=false)>  _
	Public Property strDestinatarios() As String
		Get
			Return Me._strDestinatarios
		End Get
		Set
			If (String.Equals(Me._strDestinatarios, value) = false) Then
				Me.OnstrDestinatariosChanging(value)
				Me.SendPropertyChanging
				Me._strDestinatarios = value
				Me.SendPropertyChanged("strDestinatarios")
				Me.OnstrDestinatariosChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strAlerta", DbType:="VarChar(MAX) NOT NULL", CanBeNull:=false)>  _
	Public Property strAlerta() As String
		Get
			Return Me._strAlerta
		End Get
		Set
			If (String.Equals(Me._strAlerta, value) = false) Then
				Me.OnstrAlertaChanging(value)
				Me.SendPropertyChanging
				Me._strAlerta = value
				Me.SendPropertyChanged("strAlerta")
				Me.OnstrAlertaChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_dtmActualizacion", DbType:="DateTime NOT NULL")>  _
	Public Property dtmActualizacion() As Date
		Get
			Return Me._dtmActualizacion
		End Get
		Set
			If ((Me._dtmActualizacion = value)  _
						= false) Then
				Me.OndtmActualizacionChanging(value)
				Me.SendPropertyChanging
				Me._dtmActualizacion = value
				Me.SendPropertyChanged("dtmActualizacion")
				Me.OndtmActualizacionChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strUsuario", DbType:="VarChar(60) NOT NULL", CanBeNull:=false)>  _
	Public Property strUsuario() As String
		Get
			Return Me._strUsuario
		End Get
		Set
			If (String.Equals(Me._strUsuario, value) = false) Then
				Me.OnstrUsuarioChanging(value)
				Me.SendPropertyChanging
				Me._strUsuario = value
				Me.SendPropertyChanged("strUsuario")
				Me.OnstrUsuarioChanged
			End If
		End Set
	End Property
	
	Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging
	
	Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
	
	Protected Overridable Sub SendPropertyChanging()
		If ((Me.PropertyChangingEvent Is Nothing)  _
					= false) Then
			RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
		End If
	End Sub
	
	Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
		If ((Me.PropertyChangedEvent Is Nothing)  _
					= false) Then
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End If
	End Sub
End Class

<Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.tblToolbarsPorAplicacion")>  _
Partial Public Class ToolbarsPorAplicacion
	Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	
	Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)
	
	Private _lngId As Integer
	
	Private _strNombreBoton As String
	
	Private _strToolTip As String
	
    #Region "Extensibility Method Definitions"
    Partial Private Sub OnLoaded()
    End Sub
    Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
    End Sub
    Partial Private Sub OnCreated()
    End Sub
    Partial Private Sub OnlngIdChanging(value As Integer)
    End Sub
    Partial Private Sub OnlngIdChanged()
    End Sub
    Partial Private Sub OnstrNombreBotonChanging(value As String)
    End Sub
    Partial Private Sub OnstrNombreBotonChanged()
    End Sub
    Partial Private Sub OnstrToolTipChanging(value As String)
    End Sub
    Partial Private Sub OnstrToolTipChanged()
    End Sub
    #End Region
	
	Public Sub New()
		MyBase.New
		OnCreated
	End Sub
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Name:="intId", Storage:="_lngId", DbType:="Int NOT NULL IDENTITY", IsPrimaryKey:=true, UpdateCheck:=UpdateCheck.Never)>  _
	Public Property lngId() As Integer
		Get
			Return Me._lngId
		End Get
		Set
			If ((Me._lngId = value)  _
						= false) Then
				Me.OnlngIdChanging(value)
				Me.SendPropertyChanging
				Me._lngId = value
				Me.SendPropertyChanged("lngId")
				Me.OnlngIdChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strNombreBoton", DbType:="VarChar(30) NOT NULL", CanBeNull:=false)>  _
	Public Property strNombreBoton() As String
		Get
			Return Me._strNombreBoton
		End Get
		Set
			If (String.Equals(Me._strNombreBoton, value) = false) Then
				Me.OnstrNombreBotonChanging(value)
				Me.SendPropertyChanging
				Me._strNombreBoton = value
				Me.SendPropertyChanged("strNombreBoton")
				Me.OnstrNombreBotonChanged
			End If
		End Set
	End Property
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_strToolTip", DbType:="VarChar(30)")>  _
	Public Property strToolTip() As String
		Get
			Return Me._strToolTip
		End Get
		Set
			If (String.Equals(Me._strToolTip, value) = false) Then
				Me.OnstrToolTipChanging(value)
				Me.SendPropertyChanging
				Me._strToolTip = value
				Me.SendPropertyChanged("strToolTip")
				Me.OnstrToolTipChanged
			End If
		End Set
	End Property
	
	Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging
	
	Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
	
	Protected Overridable Sub SendPropertyChanging()
		If ((Me.PropertyChangingEvent Is Nothing)  _
					= false) Then
			RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
		End If
	End Sub
	
	Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
		If ((Me.PropertyChangedEvent Is Nothing)  _
					= false) Then
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
		End If
	End Sub
End Class

Partial Public Class usp_MC_Riesgo_InsertarResult
	
	Private _lngIdMenu As System.Nullable(Of Integer)
	
	Public Sub New()
		MyBase.New
	End Sub
	
	<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_lngIdMenu", DbType:="Int")>  _
	Public Property lngIdMenu() As System.Nullable(Of Integer)
		Get
			Return Me._lngIdMenu
		End Get
		Set
			If (Me._lngIdMenu.Equals(value) = false) Then
				Me._lngIdMenu = value
			End If
		End Set
	End Property
End Class
