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


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.ConnectionString),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Data Source=A2SQLDLLO\SQL2008;Initial Catalog=dbA2Utilidades_Net_Desarrollo;Persi"& _ 
            "st Security Info=True;User ID=oydnet;Password=oydnet")>  _
        Public ReadOnly Property dbA2Utilidades_Net_DesarrolloConnectionString() As String
            Get
                Return CType(Me("dbA2Utilidades_Net_DesarrolloConnectionString"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("30")>  _
        Public ReadOnly Property Seguridad_TiempoLlamado() As String
            Get
                Return CType(Me("Seguridad_TiempoLlamado"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.ConnectionString),  _
         Global.System.Configuration.DefaultSettingValueAttribute("IOj3b0sStTzlOJEN0K07gyAkkdD4Cq4GRX47IwB+1iPrU7yig53cOJFvDRrKnQis/Wc24OL2zb2VmjMcK"& _ 
            "QxYgPNjHRBCYJE0b69aFma9sz1Q1EFfCfgta3yFlpI0D9zB84scqS+m+aUAnjy6QUp8M8SVY/cCSJPqK"& _ 
            "mk6aa/yDf4=")>  _
        Public ReadOnly Property A2RiesgosConnectionString() As String
            Get
                Return CType(Me("A2RiesgosConnectionString"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.ConnectionString),  _
         Global.System.Configuration.DefaultSettingValueAttribute("jC8q2dL5RPxf8x5hHR8RABhJcCgNLs56QxYBz3SUE6jO8s+IUajcsXdl568eoe2HoLJ2Kk1d9X436KAvU"& _ 
            "36S5BJzagDGO2BIKrRyo9b2nCqhIP3Jp7wlBy9f8XTANAtpRjWUEhUftRznWBJgutDh/d4p5MV66uxI")>  _
        Public ReadOnly Property dbOYDConnectionString() As String
            Get
                Return CType(Me("dbOYDConnectionString"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("NO")>  _
        Public ReadOnly Property Seguridad_NoValidar() As String
            Get
                Return CType(Me("Seguridad_NoValidar"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.ConnectionString),  _
         Global.System.Configuration.DefaultSettingValueAttribute("cBU135I4Fw9QYYigQF7MLtrdY+sbQJMsrJoSYtdDT24wr3kOlohFZ/ae8XQAt0QOWM6iHpaQCYbAxtPfC"& _ 
            "IgVDjgWWSJfCd+fTkHLHlGdFM51z28w5D16d7zPSQq9Ee48mME/5r+iuQY1cylm3I7NGi2bkwjtRW4nL"& _ 
            "qJWh0CC9s=")>  _
        Public ReadOnly Property dbOYDUtilidadesConnectionString() As String
            Get
                Return CType(Me("dbOYDUtilidadesConnectionString"),String)
            End Get
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.A2.Riesgos.RIA.Web.My.MySettings
            Get
                Return Global.A2.Riesgos.RIA.Web.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace