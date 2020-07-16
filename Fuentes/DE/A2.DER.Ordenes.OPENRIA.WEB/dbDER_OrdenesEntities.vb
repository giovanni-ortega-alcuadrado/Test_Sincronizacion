Imports System.Data.Entity

Partial Public Class dbDER_OrdenesEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New(Utilidades.ConstruirConexion(My.MySettings.Default.dbOYDConnectionString, "res://*/dbDER_Ordenes.csdl|res://*/dbDER_Ordenes.ssdl|res://*/dbDER_Ordenes.msl").ToString)
    End Sub
End Class