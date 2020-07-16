Imports System.Data.Entity

Partial Public Class dbOrdenesDivisasEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New(Utilidades.ConstruirConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, "res://*/dbOrdenesDivisas.csdl|res://*/dbOrdenesDivisas.ssdl|res://*/dbOrdenesDivisas.msl").ToString)
    End Sub
End Class
