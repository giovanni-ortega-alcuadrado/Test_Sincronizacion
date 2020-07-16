Imports A2.OyD.Infraestructura

Partial Public Class OyDCitiBankDatacontext

    Private _lngIdCodificacion As Integer

    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDCitiBankDatacontext", "SubmitChanges")
        End Try
    End Sub

End Class
