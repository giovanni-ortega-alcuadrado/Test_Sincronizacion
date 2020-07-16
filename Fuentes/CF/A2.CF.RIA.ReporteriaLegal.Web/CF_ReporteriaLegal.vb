Imports System.Data.SqlClient
Imports System.Data.Linq
Imports System.Reflection
Imports A2.OyD.OYDServer.RIA.Web.CFReporteriaLegal
Imports A2.OyD.Infraestructura

Partial Public Class ReporteriaLegalDBML
    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegalDBML", "SubmitChanges")
        End Try
    End Sub
End Class
