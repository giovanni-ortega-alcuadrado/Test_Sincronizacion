Imports System.Configuration
Imports A2.OyD.Infraestructura

''' <summary>
''' Esta clase fue creada para poder extender los metodos new() de la clase oyddatacontext y poder descifrar la cadena de conexión.
''' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) los metodos new() vuelven a ser automáticamente generados y se presenta un error de que 
''' existen múltiples definiciones con firmas idénticas (‘Public Sub New()’ has multiple definitions with identical signatures).
''' LA SOLUCION es eliminar de la clase oyddatacontext todos los métodos New() y dejar los que se encuentran en esta clase.
''' </summary>
''' <remarks></remarks>
Partial Public Class OyDPLUSGarantiasDataContext



    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDPLUSGarantiasDataContext", "SubmitChanges")
        End Try
    End Sub

    'JAEZ 20161117
    Public Sub ActualizarModulo(ByVal pstrModulo As String)

        'MyBase.New(A2Utilidades.Cifrar.descifrar(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)

        Dim strConnection As String
        strConnection = ajustarConexion(A2Utilidades.Cifrar.descifrar(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), pstrModulo)
        MyBase.Connection.ConnectionString = strConnection

        OnCreated()
    End Sub

End Class