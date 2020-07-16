Imports System.IO
Imports A2Utilidades.Utilidades
Imports System.Web
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text

Public Module Utilidades

    ''' <summary>
    ''' Tipo de separador para la generacion de archivos planos.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 24/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 24/2014 - Resultado Ok 
    ''' </history>
    Public Enum TipoSeparador
        TAB
        COMA
        PUNTOYCOMA
    End Enum

    ''' <summary>
    ''' Formato de archivo para la generacion de archivos planos.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 24/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 24/2014 - Resultado Ok 
    ''' 
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se adicionan las opciones PLANO y AMBOS para la generacion de Formatos de OyDClientes y Calculos Financieros.
    ''' Fecha            : Abril 01/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Abril 01/2014 - Resultado Ok   
    ''' 
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se adiciona la opcion EXCELVIEJO para la generacion de Formatos de OyDClientes y Calculos Financieros en la version de Excel 97.
    ''' Fecha            : Abril 21/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Abril 21/2014 - Resultado Ok       
    ''' </history>
    Public Enum TipoExportacion
        EXCEL
        CSV
        TXT
        PLANO
        AMBOS
        EXCELVIEJO
    End Enum

    Public Sub ManejarError(ByVal ex As Exception, ByVal Modulo As String, ByVal Funcion As String)
        'Si hay que generar log, entonces llamo al componente encargado de la generación de lo log
        A2Utilidades.Utilidades.registrarErrores(ex, Modulo, Funcion, False)

        Throw New MyException(ex)
    End Sub

    Public Sub ManejarError(ByVal ex As Exception, ByVal Modulo As String, ByVal Funcion As String, ByVal plogGenerarExcepcion As Boolean)
        'Si hay que generar log, entonces llamo al componente encargado de la generación de lo log
        A2Utilidades.Utilidades.registrarErrores(ex, Modulo, Funcion, False)

        If plogGenerarExcepcion Then
            Throw New MyException(ex)
        End If
    End Sub

    Public Function ObtenerCadenaConexion(ByVal pstrConexion As String) As String
        Try
            pstrConexion = A2Utilidades.Cifrar.descifrar(pstrConexion)
            pstrConexion = asignarWorkStationID(pstrConexion, clsCifradoServer.IPV4)
            Return pstrConexion
        Catch ex As Exception
            ManejarError(ex, "Utilidades", "ObtenerCadenaConexion")
            Return Nothing
        End Try
    End Function

    'JAEZ 20161001
    Public Function ajustarConexion(ByVal connection As String, ByVal pstrModulo As String) As String

        Dim strCnx As String = String.Empty
        Dim vstrDatosConexion As String()

        Try
            If Not String.IsNullOrEmpty(pstrModulo) Then

                If Microsoft.VisualBasic.InStr(1, connection, "Application name") Then

                    vstrDatosConexion = connection.Split(CChar(";"))

                    For intI = 0 To vstrDatosConexion.Count - 1
                        If Microsoft.VisualBasic.Trim(vstrDatosConexion(intI)).Substring(0, 11).ToLower.Equals("application") Then

                            vstrDatosConexion(intI) = "Application name=" & pstrModulo
                        End If

                        strCnx &= vstrDatosConexion(intI) & ";"
                    Next intI

                Else
                    strCnx = connection & ";" & "Application name=" & pstrModulo
                End If
            Else
                strCnx = connection
            End If

            strCnx = asignarWorkStationID(strCnx, clsCifradoServer.IPV4)

            Return (strCnx)
        Catch ex As Exception
            ManejarError(ex, "Utilidades", "ajustarConexion")
            Return (connection)
        End Try
    End Function

    Public Function asignarWorkStationID(ByVal pstrConexion As String, ByVal pstrWorkStationIP As String) As String
        Try
            pstrConexion = Microsoft.VisualBasic.LTrim(pstrConexion)
            If Microsoft.VisualBasic.LCase(pstrConexion).IndexOf("WorkStation".ToLower()) < 0 And Microsoft.VisualBasic.LCase(pstrConexion).IndexOf("WSD".ToLower()) < 0 Then
                pstrConexion &= Microsoft.VisualBasic.IIf(Microsoft.VisualBasic.Right(pstrConexion, 1) = ";", "", ";") & "WorkStation ID=" & pstrWorkStationIP
            End If
        Catch ex As Exception
            ManejarError(ex, "Utilidades", "asignarWorkStationID")
        End Try
        Return (pstrConexion)
    End Function

    Public Sub GrabarLog(ByVal Mensaje As String, ByVal Modulo As String, ByVal Funcion As String)
        Dim strSistema As String = "OyDServer"
        Dim strVersion As String = "1.0"
        Dim strFuente As String = "RIA"
        Dim strSufijoArchivo As String = "riaweb"
        Dim strRuta As String = ""

        A2Utilidades.Utilidades.generarLog(Mensaje, strSistema, strVersion, Modulo, Funcion, strFuente, strSufijoArchivo, strRuta)

    End Sub

    Public Function DemeInfoSesion(ByVal pstrUsuario As String, ByVal pstrModulo As String) As String
        Dim ip = clsCifradoServer.IPV4
        If pstrUsuario Is Nothing Then
            pstrUsuario = String.Empty
        End If
        Dim user As String = Microsoft.VisualBasic.IIf(pstrUsuario.Trim().Equals(String.Empty), HttpContext.Current.User.Identity.Name, pstrUsuario)
        Dim maquina = " " 'Net.Dns.GetHostEntry(ip).HostName JBT se quita esta linea para mejorar el rendimiento 
        Dim servidor = HttpContext.Current.Server.MachineName
        Dim browser = HttpContext.Current.Request.Browser.Browser & " - " & HttpContext.Current.Request.Browser.Id & " " & HttpContext.Current.Request.Browser.Version
        Dim infoCliente = <InfoSesion ip=<%= ip %>
                              usuario=<%= user %>
                              maquina=<%= maquina %>
                              browser=<%= browser %>
                              servidor=<%= servidor %>
                              modulo=<%= pstrModulo %>>
                          </InfoSesion>
        Return infoCliente.ToString
    End Function


    'ByVal pstrDomainServices As String, ByVal pstrMetodo As String, 
    Public Sub ValidacionSeguridadUsuario(ByVal pstrNoValidarSeguridad As String, ByVal pstrClase As String, ByVal pstrMetodo As String, ByVal pstrConexionBD As String, ByVal pstrUsuario As String, ByVal pstrToken As String, ByVal pstrTiempoSeguridad As String)
        Try
            Dim strTextoGenerarLog As String = String.Empty
            Dim logValidarSeguridad As Boolean = True

            If pstrNoValidarSeguridad = "SI" Then
                logValidarSeguridad = False
            End If
            Dim strRetorno As String = RealizarValidacionSeguridadUsuario(logValidarSeguridad, pstrClase, pstrMetodo, pstrConexionBD, pstrUsuario, pstrToken, pstrTiempoSeguridad, strTextoGenerarLog)

            If Not String.IsNullOrEmpty(strRetorno) Then
                If Not String.IsNullOrEmpty(strTextoGenerarLog) Then
                    Dim objException As New Exception(String.Format("{0}{1}{2}", strRetorno, Microsoft.VisualBasic.vbCrLf, strTextoGenerarLog))
                    ManejarError(objException, "Seguridad", "ValidacionSeguridadUsuario", False)
                Else
                    Dim objException As New Exception(strRetorno)
                    ManejarError(objException, "Seguridad", "ValidacionSeguridadUsuario", False)
                End If
                Throw New Exception("Seguridad invalida.")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function RealizarValidacionSeguridadUsuario(ByVal plogValidarSeguridad As Boolean, ByVal pstrClase As String, ByVal pstrMetodo As String, ByVal pstrConexionBD As String, ByVal pstrUsuario As String, ByVal pstrToken As String, ByVal pstrTiempoSeguridad As String, ByRef pstrTextoGenerarLog As String) As String
        Dim objTokenUsuario As clsTokenTiempoServer = Nothing
        Dim objTokenTiempoFecha As clsTokenTiempoFecha

        'VERIFICA SÍ SE DEBE DE VALIDAR LA SEGURIDAD

        If plogValidarSeguridad Then
            'RETORNA EL OBJECTO SERIALIZADO

            Try
                objTokenUsuario = JsonConvert.DeserializeObject(Of clsTokenTiempoServer)(pstrToken)
            Catch ex As Exception
                pstrTextoGenerarLog = String.Format("Token recibido:{0}", pstrToken)
                Return "0.Seguridad invalida."
                Exit Function
            End Try

            Try
                objTokenTiempoFecha = JsonConvert.DeserializeObject(Of clsTokenTiempoFecha)(A2Utilidades.CifrarSL.descifrar(objTokenUsuario.strHoraLlamado))
            Catch ex As Exception
                pstrTextoGenerarLog = String.Format("Hora Llamado:{0}", objTokenUsuario.strHoraLlamado)
                Return "1.Seguridad invalida."
                Exit Function
            End Try

            Try
                Dim strHashCliente As String = A2Utilidades.CifrarSL.descifrar(objTokenTiempoFecha.strHoraHash)
                If strHashCliente <> objTokenTiempoFecha.dtmHoraLlamado.GetHashCode.ToString Then
                    pstrTextoGenerarLog = String.Format("Hash Fecha Hora Cliente:{1}{0}Hash Fecha String Cliente:{2}", Microsoft.VisualBasic.vbCrLf, strHashCliente, objTokenTiempoFecha.dtmHoraLlamado.GetHashCode.ToString)
                    Return "2.Seguridad invalida."
                    Exit Function
                End If
            Catch ex As Exception
                Return "2.Seguridad invalida."
                Exit Function
            End Try

            Dim dtmActual As DateTime = Microsoft.VisualBasic.Now.ToString("yyyy-MM-dd hh:mm:ss tt")

            Dim diferencia As Double = dtmActual.Subtract(objTokenTiempoFecha.dtmHoraLlamado).TotalSeconds
            Dim intTiempoSegundosLlamado As Integer = 30

            Try
                intTiempoSegundosLlamado = CInt(pstrTiempoSeguridad)
            Catch ex As Exception
                intTiempoSegundosLlamado = 30
            End Try

            If Microsoft.VisualBasic.IIf(diferencia < 0, diferencia * -1, diferencia) > intTiempoSegundosLlamado Then
                pstrTextoGenerarLog = String.Format("Diferencia tiempo:{1}{0}Tiempo configurado:{2}", Microsoft.VisualBasic.vbCrLf, diferencia, intTiempoSegundosLlamado)
                Return "3.Seguridad invalida."
                Exit Function
            End If

            Dim strDirIP As String = clsCifradoServer.IPV4
            Dim strUsuarioServer As String = String.Empty
            Dim strUsuarioHA As String = String.Empty

            If Not HttpContext.Current.Request.ServerVariables("logon_user") Is Nothing Then
                strUsuarioServer = HttpContext.Current.Request.ServerVariables("logon_user")
                strUsuarioServer = ObtenerUsuarioSinDominio(strUsuarioServer)
                strUsuarioHA = A2Utilidades.Cifrar.cifrar(strUsuarioServer.ToLower, "@2sEG")
            ElseIf Not objTokenUsuario.strUsuarioAplicacion Is Nothing Then
                strUsuarioServer = objTokenUsuario.strUsuario
                strUsuarioServer = ObtenerUsuarioSinDominio(A2Utilidades.CifrarSL.descifrar(strUsuarioServer).ToLower)
                strUsuarioHA = A2Utilidades.Cifrar.cifrar(strUsuarioServer, "@2sEG")
            End If

            strDirIP = A2Utilidades.Cifrar.cifrar(strDirIP, "@2sEG")

            Dim objUsuarioServer As New clsUsuarioMaquinaServer With {.strDirIP = strDirIP, .strUsuario = strUsuarioHA}
            Dim strHashNuevo = A2Utilidades.Cifrar.cifrar(clsCifradoServer.CalcularHash(JsonConvert.SerializeObject(objUsuarioServer)), "@2sEG")
            pstrTextoGenerarLog = String.Format("Usuario maquina:{0}Usuario:{1}{0}IP:{2}{0}Hash:{3}{0}Usuario server:{0}Usuario:{4}{0}IP:{5}{0}Hash:{6}{0}",
                                                           Microsoft.VisualBasic.vbCrLf,
                                                           objTokenUsuario.strUsuarioAplicacion,
                                                           objTokenUsuario.strIP,
                                                           objTokenUsuario.strHash,
                                                           objUsuarioServer.strUsuario,
                                                           objUsuarioServer.strDirIP,
                                                           strHashNuevo)

            If objTokenUsuario.strIP <> objUsuarioServer.strDirIP Then
                Return "4.Seguridad invalida."
                Exit Function
            End If

            If objTokenUsuario.strUsuarioAplicacion <> objUsuarioServer.strUsuario Then
                Return "5.Seguridad invalida."
                Exit Function
            End If

            If objTokenUsuario.strHash <> strHashNuevo Then
                Return "6.Seguridad invalida."
                Exit Function
            End If

            Dim strUsuarioAplicacion As String = A2Utilidades.Cifrar.descifrar(objTokenUsuario.strUsuarioAplicacion, "@2sEG").ToLower
            strUsuarioAplicacion = ObtenerUsuarioSinDominio(strUsuarioAplicacion)
            Dim strUsuarioSinDominio As String = pstrUsuario.ToLower
            strUsuarioSinDominio = ObtenerUsuarioSinDominio(strUsuarioSinDominio)

            If strUsuarioAplicacion <> strUsuarioSinDominio Then
                pstrTextoGenerarLog = String.Format("Usuario aplicación{0}Usuario token:{1}{0}Usuario parametro:{2}", Microsoft.VisualBasic.vbCrLf, strUsuarioAplicacion, strUsuarioSinDominio)
                Return "7.Seguridad invalida."
                Exit Function
            End If

            If Not RealizarValidacionSeguridadUsuario_BaseDatos(pstrConexionBD, pstrClase, pstrMetodo, strUsuarioSinDominio) Then
                pstrTextoGenerarLog = String.Format("Permisos web services{0}Clase:{1}{0}Metodo:{2}", Microsoft.VisualBasic.vbCrLf, pstrClase, pstrMetodo)
                Return "8.Seguridad invalida."
            End If

            Return String.Empty
        Else
            Return String.Empty
        End If
    End Function

    Private Function ObtenerUsuarioSinDominio(ByVal pstrUsuario As String) As String
        Dim pstrUsuarioRetorno As String = String.Empty
        If pstrUsuario.IndexOf("\") > 0 Then
            pstrUsuarioRetorno = pstrUsuario.Substring(pstrUsuario.IndexOf("\") + 1, pstrUsuario.Length - pstrUsuario.IndexOf("\") - 1)
        Else
            pstrUsuarioRetorno = pstrUsuario
        End If
        Return pstrUsuarioRetorno.ToLower
    End Function

    Public Sub ValidarCaracteresInvalidosEnRuta(ByVal pstrConfiguracionCaracteresInvalidos As String, ByVal pstrRutaValidar As String)
        Try
            Dim objExceptionPrueba As New Exception("Entro a validar")
            registrarErrores(objExceptionPrueba, "Seguridad", "ValidacionSeguridadUsuario", False)
            If Not pstrConfiguracionCaracteresInvalidos Is Nothing Then
                Dim objCombinacionCaracteresValidar = pstrConfiguracionCaracteresInvalidos.Split("|")
                For Each li In objCombinacionCaracteresValidar
                    If pstrRutaValidar.Contains(li) Then
                        Dim objException As New Exception("0.1.Seguridad invalida caracter en ruta (" & li & ")")
                        registrarErrores(objException, "Seguridad", "ValidacionSeguridadUsuario", False)
                        Throw New Exception("0.1.Seguridad invalida.")
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function RealizarValidacionSeguridadUsuario_BaseDatos(ByVal pstrConexionBD As String, ByVal pstrClase As String, ByVal pstrMetodo As String, ByVal pstrUsuario As String) As Boolean
        Dim objListaParametros As New List(Of SqlParameter)
        objListaParametros.Add(CrearSQLParameter("pstrClase", pstrClase, SqlDbType.VarChar))
        objListaParametros.Add(CrearSQLParameter("pstrMetodo", pstrMetodo, SqlDbType.VarChar))
        objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
        objListaParametros.Add(CrearSQLParameter("pstrInfosesion", DemeInfoSesion(pstrUsuario, "RealizarValidacionSeguridadUsuario_BaseDatos"), SqlDbType.VarChar))
        objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

        Dim objTablaValidacion As DataTable = RetornarDataTable(ObtenerCadenaConexion(pstrConexionBD), "dbo.uspOyDNet_VerificarPermisosServicio", objListaParametros)
        If Not objTablaValidacion Is Nothing Then
            Dim logValidacion As Boolean = True
            Try
                logValidacion = CBool(objTablaValidacion.Rows(0)("logPermisoEnOpcion"))
            Catch ex As Exception
                ManejarError(ex, "Seguridad", "RealizarValidacionSeguridadUsuario_BaseDatos", False)
                logValidacion = True
            End Try
            Return logValidacion
        Else
            Return True
        End If
    End Function

    Public Function ObtenerIP() As String
        Return clsCifradoServer.IPV4
    End Function

    Public Function ObtenerUsuarioLogueado() As String
        Dim strUsuarioServer As String = String.Empty
        If Not HttpContext.Current.Request.ServerVariables("logon_user") Is Nothing Then
            strUsuarioServer = HttpContext.Current.Request.ServerVariables("logon_user")
        End If
        Return strUsuarioServer
    End Function

    Public Function RetornarValorDescodificado(ByVal pstrValor As String) As String
        Dim strValorNuevo As String = String.Empty
        strValorNuevo = HttpUtility.UrlDecode(pstrValor)
        Return strValorNuevo
    End Function

    ''' <summary>
    ''' Funcion para creacion de un SqlParameter.
    ''' </summary>
    ''' <param name="pstrNombre">Nombre del parametro.</param>
    ''' <param name="pobjValor">Valor del parametro.</param>
    ''' <param name="pobTipo">Tipo del parametro.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 24/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 24/2014 - Resultado Ok 
    ''' </history>
    Public Function CrearSQLParameter(ByVal pstrNombre As String, ByVal pobjValor As Object, ByVal pobTipo As SqlDbType) As SqlParameter
        Dim objParametro As New SqlParameter()
        objParametro.ParameterName = "@" & pstrNombre
        objParametro.DbType = pobTipo
        objParametro.Value = pobjValor
        Return objParametro
    End Function

    ''' <summary>
    ''' retorna un objeto DataSet con los resultados de la ejecucion de un procedimiento almacenado.
    ''' </summary>
    ''' <param name="pstrConexion">Cadena de conexion al repositorio de datos.</param>
    ''' <param name="pstrNombreProcedimiento">Nombre del procedimiento almacenado a ejecutar.</param>
    ''' <param name="pobjParametros">Parametros del procedimiento almacenado.</param>
    ''' <returns>Objeto tipo DataTable.</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan David Correa.
    ''' Descripción      : Creacion.
    ''' Fecha            : Agosto 06/2016
    ''' </history>
    Public Function RetornarDataSet(ByVal pstrConexion As String, ByVal pstrNombreProcedimiento As String, ByVal pobjParametros As List(Of SqlParameter), Optional ByVal pintMinutosTimeOut As Integer = 10) As DataSet
        Return ObtenerDataSetConsulta(pstrConexion, pstrNombreProcedimiento, pobjParametros, pintMinutosTimeOut)
    End Function

    ''' <summary>
    ''' retorna un objeto DataTable con los resultados de la ejecucion de un procedimiento almacenado.
    ''' </summary>
    ''' <param name="pstrConexion">Cadena de conexion al repositorio de datos.</param>
    ''' <param name="pstrNombreProcedimiento">Nombre del procedimiento almacenado a ejecutar.</param>
    ''' <param name="pobjParametros">Parametros del procedimiento almacenado.</param>
    ''' <returns>Objeto tipo DataTable.</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 24/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 24/2014 - Resultado Ok 
    ''' </history>
    Public Function RetornarDataTable(ByVal pstrConexion As String, ByVal pstrNombreProcedimiento As String, ByVal pobjParametros As List(Of SqlParameter), Optional ByVal pintMinutosTimeOut As Integer = 10) As DataTable
        Dim objDatos As DataTable = Nothing
        Dim objDataSet As DataSet = ObtenerDataSetConsulta(pstrConexion, pstrNombreProcedimiento, pobjParametros, pintMinutosTimeOut)
        If Not objDataSet Is Nothing Then
            objDatos = objDataSet.Tables(0)
        End If
        Return objDatos
    End Function

    Private Function ObtenerDataSetConsulta(ByVal pstrConexion As String, ByVal pstrNombreProcedimiento As String, ByVal pobjParametros As List(Of SqlParameter), Optional ByVal pintMinutosTimeOut As Integer = 10)
        Dim ds As DataSet = Nothing
        Dim objCommand As SqlCommand = Nothing

        Try
            objCommand = New SqlCommand()
            objCommand.Connection = New SqlConnection(pstrConexion)
            objCommand.CommandTimeout = 60 * pintMinutosTimeOut 'JEPM20170123
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.CommandText = pstrNombreProcedimiento

            For Each li In pobjParametros
                objCommand.Parameters.Add(li)
            Next

            If objCommand.Connection.State <> ConnectionState.Open Then
                objCommand.Connection.Open()
            End If

            ' Declaramos un DataAdapter a partir del SqlCommand y un DataSet para almacenar los datos.
            Dim da As New SqlClient.SqlDataAdapter(objCommand)
            ds = New DataSet

            ' Por último, rellenamos el DataSet
            da.Fill(ds)

            objCommand.Connection.Close()

        Catch ex As Exception
            If objCommand IsNot Nothing AndAlso objCommand.Connection IsNot Nothing Then
                If objCommand.Connection.State <> ConnectionState.Closed Then
                    objCommand.Connection.Close()
                End If
            End If
            Throw
        End Try

        Return ds
    End Function

    Public Sub ExecuteNonQuery(ByVal pstrConexion As String, ByVal pstrNombreProcedimiento As String, ByVal pobjParametros As List(Of SqlParameter), Optional ByVal pintMinutosTimeOut As Integer = 10)
        Dim ds As DataSet = Nothing
        Dim objCommand As SqlCommand = Nothing

        Try
            objCommand = New SqlCommand()
            objCommand.Connection = New SqlConnection(pstrConexion)
            objCommand.CommandTimeout = 60 * pintMinutosTimeOut 'JEPM20170123
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.CommandText = pstrNombreProcedimiento

            For Each li In pobjParametros
                objCommand.Parameters.Add(li)
            Next

            If objCommand.Connection.State <> ConnectionState.Open Then
                objCommand.Connection.Open()
            End If

            objCommand.ExecuteNonQuery()

            objCommand.Connection.Close()

        Catch ex As Exception
            If objCommand IsNot Nothing AndAlso objCommand.Connection IsNot Nothing Then
                If objCommand.Connection.State <> ConnectionState.Closed Then
                    objCommand.Connection.Close()
                End If
            End If
            Throw
        End Try
    End Sub


    ''' <summary>
    ''' Realiza la generacion de un archivo Excel.
    ''' </summary>
    ''' <param name="pobjDatos">Objeto tipo DataTable con los datos a ser vaciados al archivo.</param>
    ''' <param name="pstrRuta">Ruta donde se desea generar el archivo.</param>
    ''' <param name="pstrNombreArchivo">Nombre para el archivo.</param>
    ''' <returns>String con el nombre del archivo generado.</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 24/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 24/2014 - Resultado Ok 
    ''' </history>    
    Public Function GenerarExcel(pobjDatos As List(Of DataTable), pstrRuta As String, pstrNombreArchivo As String) As String
        Try

            Dim strExtensionNombreArchivo As String = ".xlsx"
            Dim objWorkbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook

            Dim strAgrupadorMiles As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberGroupSeparator
            Dim strSeparadorDecimal As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberDecimalSeparator
            Dim strFormatoNumerosExcel As String = "#" & strAgrupadorMiles & "##0" & strSeparadorDecimal & "00"
            Dim strFormatoFechasExcel As String = "dd/mm/yyyy"
            Dim strBackSlash As String = "\"
            Dim intHoja As Integer = 0

            Dim strNombreArchivo As String
            Dim strRutaCompletaArchivo As String

            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If

            strNombreArchivo = pstrNombreArchivo & strExtensionNombreArchivo
            strRutaCompletaArchivo = pstrRuta & strNombreArchivo

            For Each objTabla As DataTable In pobjDatos
                If pobjDatos.Count > (intHoja + 1) Then
                    objWorkbook.Worksheets.Add()
                End If

                Dim objWorkSheet As SpreadsheetGear.IWorksheet = objWorkbook.Worksheets(intHoja)
                Dim objRange As SpreadsheetGear.IRange = objWorkSheet.Cells(0, 0)

                objRange.CopyFromDataTable(objTabla, SpreadsheetGear.Data.SetDataFlags.None)

                For I As Integer = 0 To objTabla.Columns.Count - 1

                    Select Case objTabla.Columns(I).DataType
                        Case GetType(Decimal), GetType(Double)
                            objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoNumerosExcel
                        Case GetType(Date)
                            objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoFechasExcel
                    End Select
                Next

                objWorkSheet.UsedRange.Columns.AutoFit()

                objWorkSheet.UsedRange.Columns.AutoFilter()

                objWorkSheet.Cells(1, 0).EntireRow.Insert(SpreadsheetGear.InsertShiftDirection.Down)

                objWorkSheet.Cells(0, 0).EntireRow.Interior.Color = SpreadsheetGear.Color.FromArgb(51, 51, 153)

                objWorkSheet.Cells(0, 0).EntireRow.Font.Bold = True

                objWorkSheet.Cells(0, 0).EntireRow.Font.Color = SpreadsheetGear.Color.FromArgb(255, 255, 255)

                objWorkSheet.Name = objTabla.TableName

                intHoja += 1
            Next

            objWorkbook.SaveAs(strRutaCompletaArchivo, SpreadsheetGear.FileFormat.OpenXMLWorkbook)

            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function

    Public Function GenerarExcel(ByVal pobjDatos As DataTable, ByVal pstrRuta As String, ByVal pstrNombreArchivo As String) As String
        Return GenerarExcel_Local(pobjDatos, pstrRuta, pstrNombreArchivo)
    End Function

    Public Function GenerarExcel(ByVal pobjDatos As DataTable, ByVal pstrRuta As String, ByVal pstrNombreArchivo As String, ByVal pstrNombreHoja As String) As String
        Return GenerarExcel_Local(pobjDatos, pstrRuta, pstrNombreArchivo, pstrNombreHoja)
    End Function

    Public Function GenerarExcel(ByVal pobjDatos As DataTable, ByVal pstrRuta As String, ByVal pstrNombreArchivo As String, ByVal pstrNombreHoja As String, ByVal plogNombreColumnas As Boolean) As String
        Return GenerarExcel_Local(pobjDatos, pstrRuta, pstrNombreArchivo, pstrNombreHoja, plogNombreColumnas)
    End Function

    Private Function GenerarExcel_Local(ByVal pobjDatos As DataTable, ByVal pstrRuta As String, ByVal pstrNombreArchivo As String, Optional ByVal pstrNombreHoja As String = "", Optional ByVal plogNombreColumnas As Boolean = True) As String
        Try

            Dim strExtensionNombreArchivo As String = ".xlsx"
            Dim objWorkbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook
            Dim objWorkSheet As SpreadsheetGear.IWorksheet = objWorkbook.Worksheets(0)
            objWorkSheet.Name = "Sheet1"
            If Not String.IsNullOrEmpty(pstrNombreHoja) Then
                objWorkSheet.Name = pstrNombreHoja
            End If
            Dim objRange As SpreadsheetGear.IRange = objWorkSheet.Cells(0, 0)

            Dim strAgrupadorMiles As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberGroupSeparator
            Dim strSeparadorDecimal As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberDecimalSeparator
            Dim strFormatoNumerosExcel As String = "#" & strAgrupadorMiles & "##0" & strSeparadorDecimal & "00"
            Dim strFormatoFechasExcel As String = "dd/mm/yyyy"
            Dim strBackSlash As String = "\"

            Dim strNombreArchivo As String
            Dim strRutaCompletaArchivo As String

            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If

            strNombreArchivo = pstrNombreArchivo & strExtensionNombreArchivo
            strRutaCompletaArchivo = pstrRuta & strNombreArchivo

            If plogNombreColumnas Then
                objRange.CopyFromDataTable(pobjDatos, SpreadsheetGear.Data.SetDataFlags.None)

                For I As Integer = 0 To pobjDatos.Columns.Count - 1

                    Select Case pobjDatos.Columns(I).DataType
                        Case GetType(Decimal), GetType(Double)
                            objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoNumerosExcel
                        Case GetType(Date)
                            objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoFechasExcel
                    End Select

                Next

                objWorkSheet.UsedRange.Columns.AutoFit()

                objWorkSheet.UsedRange.Columns.AutoFilter()

                'objWorkSheet.Cells(1, 0).EntireRow.Insert(SpreadsheetGear.InsertShiftDirection.Down)

                objWorkSheet.Cells(0, 0).EntireRow.Interior.Color = SpreadsheetGear.Color.FromArgb(51, 51, 153)

                objWorkSheet.Cells(0, 0).EntireRow.Font.Bold = True

                objWorkSheet.Cells(0, 0).EntireRow.Font.Color = SpreadsheetGear.Color.FromArgb(255, 255, 255)
            Else
                objRange.CopyFromDataTable(pobjDatos, SpreadsheetGear.Data.SetDataFlags.NoColumnHeaders)
            End If

            objWorkbook.SaveAs(strRutaCompletaArchivo, SpreadsheetGear.FileFormat.OpenXMLWorkbook)

            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function

    ''' <summary>
    ''' Realiza la generacion de un archivo Excel en formatos antiguos como la version 97 de excel.
    ''' </summary>
    ''' <param name="pobjDatos">Objeto tipo DataTable con los datos a ser vaciados al archivo.</param>
    ''' <param name="pstrRuta">Ruta donde se desea generar el archivo.</param>
    ''' <param name="pstrNombreArchivo">Nombre para el archivo.</param>
    ''' <returns>String con el nombre del archivo generado.</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 21/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Abril 21/2014 - Resultado Ok 
    ''' </history>   
    Public Function GenerarExcelVersionesViejas(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, Optional pstrNombreHoja As String = "") As String
        Try

            Dim strExtensionNombreArchivo As String = ".xls"
            Dim objWorkbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook
            Dim objWorkSheet As SpreadsheetGear.IWorksheet = objWorkbook.Worksheets(0)
            objWorkSheet.Name = "Sheet1"
            If Not String.IsNullOrEmpty(pstrNombreHoja) Then
                objWorkSheet.Name = pstrNombreHoja
            End If
            Dim objRange As SpreadsheetGear.IRange = objWorkSheet.Cells(0, 0)

            Dim strAgrupadorMiles As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberGroupSeparator
            Dim strSeparadorDecimal As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberDecimalSeparator
            Dim strFormatoNumerosExcel As String = "#" & strAgrupadorMiles & "##0" & strSeparadorDecimal & "00"
            Dim strFormatoFechasExcel As String = "dd/mm/yyyy"
            Dim strBackSlash As String = "\"

            Dim strNombreArchivo As String
            Dim strRutaCompletaArchivo As String

            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If

            strNombreArchivo = pstrNombreArchivo & strExtensionNombreArchivo
            strRutaCompletaArchivo = pstrRuta & strNombreArchivo


            objRange.CopyFromDataTable(pobjDatos, SpreadsheetGear.Data.SetDataFlags.None)


            For I As Integer = 0 To pobjDatos.Columns.Count - 1

                Select Case pobjDatos.Columns(I).DataType
                    Case GetType(Decimal), GetType(Double)
                        objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoNumerosExcel
                    Case GetType(Date)
                        objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoFechasExcel
                End Select

            Next

            objWorkSheet.UsedRange.Columns.AutoFit()

            objWorkSheet.UsedRange.Columns.AutoFilter()

            objWorkSheet.Cells(1, 0).EntireRow.Insert(SpreadsheetGear.InsertShiftDirection.Down)

            objWorkSheet.Cells(0, 0).EntireRow.Interior.Color = SpreadsheetGear.Color.FromArgb(51, 51, 153)

            objWorkSheet.Cells(0, 0).EntireRow.Font.Bold = True

            objWorkSheet.Cells(0, 0).EntireRow.Font.Color = SpreadsheetGear.Color.FromArgb(255, 255, 255)

            objWorkbook.SaveAs(strRutaCompletaArchivo, SpreadsheetGear.FileFormat.Excel8)

            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function

    ''' <summary>
    ''' Realiza la generacion de un archivo de texto plano.
    ''' </summary>
    ''' <param name="pobjDatos">Objeto tipo DataTable con los datos a ser vaciados al archivo.</param>
    ''' <param name="pstrRuta">Ruta donde se desea generar el archivo.</param>
    ''' <param name="pstrNombreArchivo">Nombre para el archivo.</param>
    ''' <param name="pstrExtension">Extension para el archivo.</param>
    ''' <param name="pstrSeparador">Caracter separador de campos.</param>
    ''' <returns>String con el nombre del archivo generado.</returns>
    ''' <remarks>
    ''' La asignacion quemada de la extension, debe ser retirada del metodo y se debe desarrollar la funcionalidad para que dicha extension llegue desde la UI. Adicionalmente
    ''' las extensiones que sean enviadas desde la UI deben existir en los MIME Types del IIS en donde se despliegue la aplicacion de lo contrario se producira un error.
    ''' </remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 24/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Febrero 24/2014 - Resultado Ok 
    ''' 
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Se adiciona lógica para manejar la opción PLANO que se incluye para OyDClientes y Cálculos Financieros, esto permitirá generar un archivo de texto sin extensión.
    ''' Fecha            : Abril 01/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Abril 01/2014 - Resultado Ok 
    ''' 
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Se le asigna el valor txt al parametro pstrExtension para que los archivos de texto plano puedan ser visualizados en el IIS cuando se despliegue
    '''                    la aplicacion, pues sin extencion se presenta un error debido a que en IIS no encuentra el MIME Types.
    ''' Fecha            : Abril 08/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Abril 08/2014 - Resultado Ok     
    ''' </history>   
    Public Function GenerarTextoPlano(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, ByVal pstrExtension As String, pstrSeparador As String) As String
        Return GenerarTextoPlanoInterno(pobjDatos, pstrRuta, pstrNombreArchivo, pstrExtension, pstrSeparador, False, False, False, False, Nothing)
    End Function

    Public Function GenerarTextoPlano(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, ByVal pstrExtension As String, pstrSeparador As String, ByVal plogCambiarCodificacion As Boolean, ByVal pobjTipoCodificacion As Encoding) As String
        Return GenerarTextoPlanoInterno(pobjDatos, pstrRuta, pstrNombreArchivo, pstrExtension, pstrSeparador, False, False, False, plogCambiarCodificacion, pobjTipoCodificacion)
    End Function

    Public Function GenerarTextoPlano(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, ByVal pstrExtension As String, pstrSeparador As String, ByVal plogRespetarExtension As Boolean, ByVal plogPonerTitulos As Boolean, ByVal plogGenerarLineaEnBlanco As Boolean) As String
        Return GenerarTextoPlanoInterno(pobjDatos, pstrRuta, pstrNombreArchivo, pstrExtension, pstrSeparador, plogRespetarExtension, plogPonerTitulos, plogGenerarLineaEnBlanco, False, Nothing)
    End Function

    Public Function GenerarTextoPlano(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, ByVal pstrExtension As String, pstrSeparador As String, ByVal plogRespetarExtension As Boolean, ByVal plogPonerTitulos As Boolean, ByVal plogGenerarLineaEnBlanco As Boolean, ByVal plogCambiarCodificacion As Boolean, ByVal pobjTipoCodificacion As Encoding) As String
        Return GenerarTextoPlanoInterno(pobjDatos, pstrRuta, pstrNombreArchivo, pstrExtension, pstrSeparador, plogRespetarExtension, plogPonerTitulos, plogGenerarLineaEnBlanco, plogCambiarCodificacion, pobjTipoCodificacion)
    End Function

    Private Function GenerarTextoPlanoInterno(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, ByVal pstrExtension As String, pstrSeparador As String, ByVal plogRespetarExtension As Boolean, ByVal plogPonerTitulos As Boolean, ByVal plogGenerarLineaEnBlanco As Boolean, ByVal plogCambiarCodificacion As Boolean, ByVal pobjTipoCodificacion As Encoding) As String

        Try
            If plogRespetarExtension = False Then
                'Inicio JCS - Abril 08/2014 - Codigo quemado.
                pstrExtension = "txt"
                'Fin JCS - Abril 08/2014 - Codigo quemado.
            End If

            Dim strBackSlash As String = "\"
            Dim strNombreArchivo As String = String.Empty


            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If

            If pstrExtension <> TipoExportacion.PLANO.ToString And pstrExtension <> TipoExportacion.AMBOS.ToString Then       ' JCS Abril 01/2014
                If Microsoft.VisualBasic.Strings.Right(pstrNombreArchivo, 1) <> "." Then
                    pstrNombreArchivo = pstrNombreArchivo & "."
                End If
                strNombreArchivo = pstrNombreArchivo & pstrExtension
            Else
                strNombreArchivo = pstrNombreArchivo
            End If

            Dim strRutaArchivoCompleta As String = pstrRuta & strNombreArchivo '& pstrExtension

            'Nuevo objeto StreamWriter, para acceder al fichero y poder guardar las líneas  
            If plogCambiarCodificacion Then
                If pobjTipoCodificacion Is Nothing Then
                    pobjTipoCodificacion = Encoding.Default
                End If
                Using archivo As StreamWriter = New StreamWriter(strRutaArchivoCompleta, False, pobjTipoCodificacion)
                    ProcesarTextoPlanoInterno_ProcesarArchivo(archivo, pobjDatos, pstrSeparador, plogPonerTitulos, plogGenerarLineaEnBlanco)
                End Using
            Else
                Using archivo As StreamWriter = New StreamWriter(strRutaArchivoCompleta)
                    ProcesarTextoPlanoInterno_ProcesarArchivo(archivo, pobjDatos, pstrSeparador, plogPonerTitulos, plogGenerarLineaEnBlanco)
                End Using
            End If


            ' Abrir con Process.Start el archivo de texto  
            ' Process.Start(strRutaArchivoCompleta)
            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function

    Private Sub ProcesarTextoPlanoInterno_ProcesarArchivo(ByRef archivo As StreamWriter, ByVal pobjDatos As DataTable, ByVal pstrSeparador As String, ByVal plogPonerTitulos As Boolean, ByVal plogGenerarLineaEnBlanco As Boolean)
        ' variable para almacenar la línea actual del dataview  
        Dim linea As String = String.Empty

        If plogPonerTitulos Then
            With pobjDatos
                For Each col As DataColumn In .Columns
                    linea = linea & col.ColumnName & pstrSeparador
                Next
            End With

            With archivo
                .WriteLine(linea.ToString)
            End With
        End If

        If plogGenerarLineaEnBlanco Then
            With archivo
                .WriteLine(String.Empty)
            End With
        End If

        linea = String.Empty

        With pobjDatos
            ' Recorrer las filas del dataGridView  
            For fila As Integer = 0 To .Rows.Count - 1
                ' vaciar la línea  
                linea = String.Empty

                ' Recorrer la cantidad de columnas que contiene el dataGridView  
                For col As Integer = 0 To .Columns.Count - 1
                    ' Almacenar el valor de toda la fila , y cada campo separado por el delimitador  
                    linea = linea & .Rows(fila).Item(col).ToString & pstrSeparador
                Next

                ' Escribir una línea con el método WriteLine  
                With archivo
                    ' eliminar el último caracter ";" de la cadena  
                    linea = linea.Remove(linea.Length - 1).ToString
                    ' escribir la fila  
                    .WriteLine(linea.ToString)
                End With
            Next
        End With
    End Sub

    Public Function ExtraerTildesTexto(ByVal pstrTexto As String) As String
        Dim strTextoFinal As String = String.Empty
        strTextoFinal = pstrTexto.Replace("á", "a")
        strTextoFinal = strTextoFinal.Replace("é", "e")
        strTextoFinal = strTextoFinal.Replace("í", "i")
        strTextoFinal = strTextoFinal.Replace("ó", "o")
        strTextoFinal = strTextoFinal.Replace("ú", "u")
        strTextoFinal = strTextoFinal.Replace("Á", "A")
        strTextoFinal = strTextoFinal.Replace("É", "E")
        strTextoFinal = strTextoFinal.Replace("Í", "I")
        strTextoFinal = strTextoFinal.Replace("Ó", "O")
        strTextoFinal = strTextoFinal.Replace("Ú", "U")
        Return strTextoFinal
    End Function

End Module


Public Class clsTokenTiempoServer
    <JsonProperty("HO", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strHoraLlamado As String
    <JsonProperty("T", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strHash As String
    <JsonProperty("U", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strUsuarioAplicacion As String
    <JsonProperty("I", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strIP As String
    <JsonProperty("UT", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strUsuario As String
    <JsonProperty("L", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strLlamado As String
End Class

Public Class clsTokenTiempoFecha
    <JsonProperty("HO", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property dtmHoraLlamado As DateTime
    <JsonProperty("HOH", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strHoraHash As String
End Class

Public Class clsUsuarioMaquinaServer
    <JsonProperty("I", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strDirIP As String
    <JsonProperty("U", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strUsuario As String
End Class

Public Class clsCifradoServer
    Public Shared Function CalcularHash(texto As String) As String
        Dim hashMD5 = MD5.Create()
        Dim hash2 As Byte() = hashMD5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(texto))
        Dim hex = BitConverter.ToString(hash2).Replace("-", "")
        Return hex
    End Function

    Public Shared Function IPV4() As String
        Try
            Dim strIP As String = String.Empty

            strIP = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If strIP = "" Then
                strIP = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
            End If

            If strIP Is Nothing Then
                strIP = ""
            End If

            If strIP = "" Or strIP.Contains("::") Then
                Dim objListaIP = Net.Dns.GetHostEntry(HttpContext.Current.Request.UserHostAddress)
                If Not objListaIP Is Nothing Then
                    Dim listAdress = objListaIP.AddressList
                    If Not listAdress Is Nothing Then
                        For Each IPA As IPAddress In listAdress
                            If IPA.AddressFamily.ToString() = "InterNetwork" Then
                                strIP = IPA.ToString()
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If

            Return strIP
        Catch ex As Exception
            Return HttpContext.Current.Request.UserHostAddress
        End Try
    End Function

End Class