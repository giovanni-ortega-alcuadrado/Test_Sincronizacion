'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class tblModulos
    Public Property intId As Integer
    Public Property strModulo As String
    Public Property strSubModulo As String
    Public Property strDescripcion As String
    Public Property dtmActualizacion As Nullable(Of Date)
    Public Property strUsuario As String
    Public Property strNombreConsecutivo As String

    Public Overridable Property tblModulosEstadosConfiguracion As ICollection(Of tblModulosEstadosConfiguracion) = New HashSet(Of tblModulosEstadosConfiguracion)

End Class
