Imports System.Configuration
Imports System.Data.Linq.Mapping
Imports System.Data.Linq
Imports System.Reflection
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

Partial Public Class OyDPLUSOrdenesDivisasDataContext

    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDPLUSOrdenesDivisasDataContext", "SubmitChanges")
        End Try
    End Sub

End Class



