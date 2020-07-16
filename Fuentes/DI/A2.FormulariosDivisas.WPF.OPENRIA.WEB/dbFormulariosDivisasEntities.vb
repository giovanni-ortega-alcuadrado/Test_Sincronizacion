Imports System.Data.Entity
Public Class dbFormulariosDivisasEntities
	Inherits DbContext
    Public Sub New()
        'JAPC20181106
        MyBase.New(Utilidades.ConstruirConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, "res://*/dbFormulariosDivisas.csdl|res://*/dbFormulariosDivisas.ssdl|res://*/dbFormulariosDivisas.msl").ToString)
    End Sub
End Class
