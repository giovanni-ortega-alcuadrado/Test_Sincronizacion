Imports System.Data.Entity

Partial Public Class dbOrdenesEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New(Utilidades.ConstruirConexion(My.MySettings.Default.dbOYDConnectionString, "res://*/dbOrdenes.csdl|res://*/dbOrdenes.ssdl|res://*/dbOrdenes.msl").ToString)
    End Sub
End Class
