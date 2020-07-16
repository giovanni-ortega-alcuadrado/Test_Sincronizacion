Imports System.Data.SqlClient
Imports System.Net
Imports System.Web.Http
Imports A2.OyD.Infraestructura

Public Class A2_UtilidadesController
    Inherits ApiController

    ' GET api/Scripts_EjecutarConsulta
    <HttpGet()>
    Public Function Scripts_EjecutarConsulta(ByVal pintIdCompania As Nullable(Of Integer),
                                             ByVal pintIDScript As Integer,
                                             ByVal pstrNombreScript As String,
                                             ByVal pstrParametros As String,
                                             ByVal pstrUsuario As String,
                                             ByVal pstrMaquina As String,
                                             ByVal pstrInfoConexion As String) As DataSet
        Try
            Dim strCadenaConexion As String = System.Configuration.ConfigurationManager.ConnectionStrings("A2.OyD.OYDServer.RIA.Web.My.MySettings.dbOYDConnectionString").ConnectionString
            ValidacionSeguridadUsuario(My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, strCadenaConexion, pstrUsuario, pstrInfoConexion, My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strProcedimiento As String = "[dbo].[uspEjecutarScripts_EjecutarProcedimiento]"
            Dim strConexionVlrs As String = String.Empty
            Dim strSeparadorVlrs As String = String.Empty
            Dim strResultado As String = String.Empty
            Dim intLongitudVlrs As Integer = 0

            Dim strConexion As String = ObtenerCadenaConexion(strCadenaConexion)

            strConexionVlrs = strCadenaConexion
            leerDatosConexion(strSeparadorVlrs, strConexionVlrs, intLongitudVlrs)

            If pstrParametros Is Nothing Then
                pstrParametros = String.Empty
            End If

            Dim objListaParametros As New List(Of SqlParameter)
            objListaParametros.Add(CrearSQLParameter("pintIdCompania", pintIdCompania, SqlDbType.Int))
            objListaParametros.Add(CrearSQLParameter("pintIdScript", pintIDScript, SqlDbType.Int))
            objListaParametros.Add(CrearSQLParameter("pstrNombreScript", pstrNombreScript, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrParametros", pstrParametros, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrValoresArchivo", strConexionVlrs, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrSeparadorValores", strSeparadorVlrs, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintLongitud", intLongitudVlrs, SqlDbType.Int))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "Scripts_EjecutarConsulta"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProcedimiento, objListaParametros, 60)
            Return objDatosConsulta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Scripts_EjecutarConsulta")
            Return Nothing
        End Try
    End Function

    Private Sub leerDatosConexion(ByRef pstrSeparador As String, ByRef pstrConexion As String, ByRef pintLongitud As Integer)
        Dim intPos As Integer = -1
        Dim intPosFin As Integer = -1
        Dim intMaxPos As Integer = -1
        Dim intLongPwd As Integer = 0
        Dim dblValor As Double
        Dim vntVector As String() = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}

        Try
            Dim strCadenaConexion As String = System.Configuration.ConfigurationManager.ConnectionStrings("A2.OyD.OYDServer.RIA.Web.My.MySettings.dbOYDConnectionString").ConnectionString
            pstrConexion = A2Utilidades.Cifrar.descifrar(strCadenaConexion)

            intMaxPos = vntVector.Length

            Randomize()
            dblValor = Rnd()
            pintLongitud = CInt(CDbl(Right((dblValor - CInt(dblValor)).ToString, 2)) Mod intMaxPos)
            pstrSeparador = vntVector(pintLongitud)
            dblValor = Rnd()
            pintLongitud = CInt(CDbl(Right((dblValor - CInt(dblValor)).ToString, 2)) Mod intMaxPos)
            pstrSeparador &= vntVector(pintLongitud)
            dblValor = Rnd()
            pintLongitud = CInt(CDbl(Right((dblValor - CInt(dblValor)).ToString, 2)) Mod intMaxPos)
            pstrSeparador &= vntVector(pintLongitud)
            pintLongitud = 3

            intPos = pstrConexion.ToLower.IndexOf("password")
            If intPos < 0 Then
                intPos = pstrConexion.ToLower.IndexOf("pwd")
            End If

            If intPos >= 0 Then
                intPos = pstrConexion.IndexOf("=", intPos) + 1
                If intPos >= 0 Then
                    intPosFin = pstrConexion.IndexOf(";", intPos)
                    If intPosFin >= 0 Then
                        intLongPwd = pstrConexion.IndexOf(";", intPos) - intPos
                    Else
                        intLongPwd = pstrConexion.Length - intPos
                    End If
                    pstrConexion = pstrConexion.Substring(intPos, intLongPwd)
                Else
                    pstrConexion = String.Empty
                End If
            Else
                pstrConexion = String.Empty
            End If

            pstrConexion = A2Utilidades.Cifrar.cifrar(pstrConexion, pstrSeparador)
        Catch ex As Exception
            Throw New ApplicationException("Se presentó un error al extraer la información de conectividad necesaria para generar el archivo solicitado." & vbNewLine & vbNewLine & ex.Message)
        End Try

    End Sub


End Class
