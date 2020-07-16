Imports System.Data.Entity

Partial Public Class dbDER_MaestrosEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New(Utilidades.ConstruirConexion(My.MySettings.Default.dbOYDConnectionString, "res://*/dbDER_Maestros.csdl|res://*/dbDER_Maestros.ssdl|res://*/dbDER_Maestros.msl").ToString)
    End Sub
End Class
