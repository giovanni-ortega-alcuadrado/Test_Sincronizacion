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



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0"),  _
 Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
Partial Friend NotInheritable Class MySettings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
    
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
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
     Global.System.Configuration.DefaultSettingValueAttribute("<style>   h1   {font-weight: normal;font-style: oblique;font-size: 100%;}    h4  "& _ 
        " {font-weight: normal;font-size: 110%;}    h3   {font-weight: normal;font-size: "& _ 
        "110%;}    th    {font-weight: normal;font-size: 100%;} </style>   <H4>Señor(a): "& _ 
        "[NOMBREASESOR] </H4>  <H4>La orden  con número [NUMEROORDEN] de Divisas, tipo [T"& _ 
        "IPO] y creada por el usuario [NOMBREUSUARIO] </H4>  <H4>Ha sido devuelta por el "& _ 
        "usuario [NOMBREUSUARIODEVOLUCION] con la siguiente nota:</H4>  <H4>[OBSERVACION]"& _ 
        "</H4>  <H4>Datos de la orden</H4>  <table border=""1"">  <tr>  <th align=""left"">Co"& _ 
        "mitente</th>  <th> [CLIENTE] </th>     </tr>  <tr>  <th align=""left"">Cantidad</t"& _ 
        "h>  <th> [CANTIDAD]  </th>     </tr>  <tr>  <th align=""left"">Precio</th>  <th> ["& _ 
        "PRECIO]  </th>     </tr>  <tr>  <th align=""left"">Fecha</th>  <th> [FECHA]  </th>"& _ 
        "     </tr>  <tr>  <th align=""left"">Hora creación</th>  <th> [HORACREACION]  </th"& _ 
        ">     </tr>  </table>  <H1>  Nota: No responda este correo, ha sido generado por"& _ 
        " un servicio automático de notificaciones.</H1>")>  _
    Public ReadOnly Property EMAIL_MENSAJE_EVENTO() As String
        Get
            Return CType(Me("EMAIL_MENSAJE_EVENTO"),String)
        End Get
    End Property
End Class

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.A2OrdenesDivisasWPF.MySettings
            Get
                Return Global.A2OrdenesDivisasWPF.MySettings.Default
            End Get
        End Property
    End Module
End Namespace