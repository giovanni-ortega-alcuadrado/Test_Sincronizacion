Imports System.Data.SqlClient
Imports System.Data.Linq
Imports System.Reflection
Imports A2.OyD.OYDServer.RIA.Web.CFProcesarPortafolios
Imports A2.OyD.Infraestructura

Partial Public Class ProcesarPortafoliosDBML
    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "CalculosFinancierosDBML", "SubmitChanges")
        End Try
    End Sub

    'JAEZ 20161001
    Public Sub ActualizarModulo(ByVal pstrModulo As String)

        'MyBase.New(A2Utilidades.Cifrar.descifrar(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)

        Dim strConnection As String
        strConnection = ajustarConexion(A2Utilidades.Cifrar.descifrar(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), pstrModulo)
        MyBase.Connection.ConnectionString = strConnection

        OnCreated()
    End Sub

End Class
