Imports System.Data.Entity
Public Class dbPLAT_PreordenesEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New(Utilidades.ConstruirConexion(My.MySettings.Default.dbOYDConnectionString, "res://*/dbPLAT_Preordenes.csdl|res://*/dbPLAT_Preordenes.ssdl|res://*/dbPLAT_Preordenes.msl").ToString)
    End Sub
End Class
