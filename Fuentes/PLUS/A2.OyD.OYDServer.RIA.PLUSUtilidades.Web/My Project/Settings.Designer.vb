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
         Global.System.Configuration.DefaultSettingValueAttribute("Uploads")>  _
        Public ReadOnly Property DirectorioArchivosUpload() As String
            Get
                Return CType(Me("DirectorioArchivosUpload"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public ReadOnly Property HabilitaDebbugImportar() As String
            Get
                Return CType(Me("HabilitaDebbugImportar"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.ConnectionString),  _
         Global.System.Configuration.DefaultSettingValueAttribute("IrNzhqza18F+0iixDfKd1BjQpBMDaOTvQ24yesbQ3NkzVMxditcDQGfl2fK9Z3/gHZXJzPUjZhfvjWv6g"& _ 
            "aX9X61GDllS/25ocqrf+S95lzYkO2LzYOVAFFQrEZ0P5sLmO11CO5r4quCg6Y31I6JiDjD/ZvEKbD/od"& _ 
            "nQin7xbQw0tdGg7RKTo0Kwt+Rp83QkrqlwBHPGLmDI=")>  _
        Public ReadOnly Property dbOYDConnectionString() As String
            Get
                Return CType(Me("dbOYDConnectionString"),String)
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
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("300")>  _
        Public ReadOnly Property TimeUPUtilidades() As String
            Get
                Return CType(Me("TimeUPUtilidades"),String)
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
         Global.System.Configuration.DefaultSettingValueAttribute("NO")>  _
        Public ReadOnly Property Seguridad_NoValidar() As String
            Get
                Return CType(Me("Seguridad_NoValidar"),String)
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
        Friend ReadOnly Property Settings() As Global.A2.OyD.OYDServer.RIA.Web.My.MySettings
            Get
                Return Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
