Imports A2.OyD.Infraestructura

Partial Public Class RiesgosModelDataContext

    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.Riesgos.RIA.Web.My.MySettings.Default.A2RiesgosConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "RiesgosModelDataContext", "SubmitChanges")
        End Try
    End Sub

End Class
