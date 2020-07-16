Imports System.IO
Imports A2Utilidades.Utilidades
Imports System.Web
Imports System.Data.SqlClient
Imports System.Text

Module Utilidades

    Friend Function ObtenerCadenaConexionUtilidades() As String
        Return A2Utilidades.Cifrar.descifrar(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDUtilidadesConnectionString)
    End Function

    Friend Function ObtenerTimeUpUtilidades() As Integer
        Return CInt(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.TimeUPUtilidades)
    End Function

End Module
