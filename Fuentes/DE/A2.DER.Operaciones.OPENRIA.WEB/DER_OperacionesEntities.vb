Imports System.Data.Entity

Partial Public Class DER_OperacionesEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New(Utilidades.ConstruirConexion(My.MySettings.Default.dbOYDConnectionString, "res://*/DER_Operaciones.csdl|res://*/DER_Operaciones.ssdl|res://*/DER_Operaciones.msl").ToString)
    End Sub
End Class