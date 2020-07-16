Imports System.Data.Entity

Partial Public Class dbPersonasEntities
    Inherits DbContext

    ''' <summary>
    ''' JAPC20200507:Comentario constructor
    ''' </summary>
    Public Sub New()
        'JAPC20181106

        MyBase.New(Utilidades.ConstruirConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, "res://*/dbPersonas.csdl|res://*/dbPersonas.ssdl|res://*/dbPersonas.msl").ToString)


    End Sub

End Class
