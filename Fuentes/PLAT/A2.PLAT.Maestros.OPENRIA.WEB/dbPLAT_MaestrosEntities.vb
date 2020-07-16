Imports System.Data.Entity

Partial Public Class dbPLAT_MaestrosEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New(Utilidades.ConstruirConexion(My.MySettings.Default.dbOYDConnectionString, "res://*/dbPLAT_Maestros.csdl|res://*/dbPLAT_Maestros.ssdl|res://*/dbPLAT_Maestros.msl").ToString)
    End Sub
End Class
