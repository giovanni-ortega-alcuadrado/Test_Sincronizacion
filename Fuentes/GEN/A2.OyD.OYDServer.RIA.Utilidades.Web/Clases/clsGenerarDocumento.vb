Imports System
Imports System.IO
Imports System.Web.Services.Protocols
Imports RS = A2.OyD.OYDServer.RIA.Web.MSReportingServices

Public Class clsGenerarDocumento

    Public Const REPORTE_ERROR As String = "A2ERROR: "

    ''' <summary>
    ''' Recibe los datos para ejecutar un reporte en Microsoft Reporting Services y guardar el resultado de la ejecución, si finaliza correctamente, en un archivo con el formato
    ''' indicado como parámetro.
    ''' </summary>
    ''' 
    ''' <param name="pstrUrlRS">URL del servicio web de Reporting services para ejecución de reportes.</param>
    ''' <param name="pstrReporte">Nombre del reporte que se debe ejecutar, con la ruta de acceso en Reporting Services.</param>
    ''' <param name="pstrRutaArchivo">Ruta en la cual se debe guardar el archivo generado como resultado de la ejecución del reporte. Debe existir porque el proceso no lo cra</param>
    ''' <param name="pstrPrefijoArchivo">Prefijo para incluir en el nombre el archivo</param>
    ''' <param name="pstrTipoArchivo">Tipo de archivo a generar: XLS,XLSX ==> MS Excel; DOC ==> MS Word; PDF ==> PDF; CSV ==> Texto separado por coma; HTML ==> HTML4.0; MHTM ==> MHTML; Si se recibe un tipo no válido se genera PDF.</param>
    ''' <param name="pstrParametros">Texto que contiene los parámetro necesarios para la ejecución del reporte. El formato de este texto debe ser: Parametro01=Valor|Parametro02=Valor|..|ParametroN=Valor. El nombre del parámetro debe coincidir con el nombre definido en el reporte.</param>
    ''' <param name="pstrParametrosEnNombre">Texto que contiene los valores que se deben incluir como parte del nombre del archivo. El formato de este texto debe ser: Valor01|Valor02|..|ValorN.</param>
    ''' <param name="pstrSeparador">Separador idica hasta donde va un parámetro (nombre=valor). Por defecto es "|" pero puede ser reemplazado.</param>
    ''' <param name="plogLanzarError">Si es verdadero y ocurre un error se hace un raiserror del error. Si es falso el error se retorna como salida de la función iniciando con el prefijo "A2ERROR:"</param>
    ''' 
    ''' <returns>
    ''' Retorna el nombre del archivo si se generó correctamente o un mensaje de error si falló la generación y el parámetro "plogLanzarError" es falso. 
    ''' Si se genera error y es verdadero el parámetro "plogLanzarError", se hace un raiseerror.
    ''' </returns>
    ''' 
    ''' <remarks>
    ''' Los valores de los parámetros en el nombre se separan con el caracter _. 
    ''' Si el valor de uno de los parámetros que se debe incluir en el nombre tiene alguno de los siguientes caracteres /\:*?"<>| son reemplazados con el caracter _
    ''' Si el valor de un parámetro que se incluya en el nombre del archivo tiene caracteres no soportados, diferentes a los mencionados, por el sistema operativo en el nombre de los archivos se genera un error.
    ''' </remarks>
    ''' 
    Public Function generarArchivo(ByVal pstrUrlRS As String, ByVal pstrReporte As String, ByVal pstrRutaArchivo As String, ByVal pstrPrefijoArchivo As String,
                                   ByVal pstrTipoArchivo As String, ByVal pstrParametros As String, ByVal pstrUsuario As String, Optional ByVal pstrParametrosEnNombre As String = "",
                                   Optional ByVal pstrSeparador As String = "|", Optional ByVal plogLanzarError As Boolean = False,
                                   Optional pstrEncoding As String = "") As String

        Dim logArchivoAbierto As Boolean = False
        Dim logError As Boolean = False
        Dim intPos As Integer = -1
        Dim strParametro As String = String.Empty
        Dim strValorParametro As String = String.Empty
        Dim strFormato As String = "PDF" '//Formato de archivo en el cual se debe generar
        Dim strArchivo As String = String.Empty '// Nombre del archivo que se debe generar
        Dim strRSHistoryID As String = Nothing '//  Retorna el Id de la ejecución que se generó en Reporting Services
        Dim strFuenteError As String  '// Describe los datos generales de llamado del procedimiento con los cuales se genero el error
        Dim strMsgError As String = String.Empty
        Dim objRSExec As RS.ReportExecutionServiceSoapClient = Nothing '//  Proxy para la conexión a Reporting Services
        Dim objRSServerInfo As RS.ServerInfoHeader = Nothing '//  Información retornada por el servidor de Reporting Services cuando se ejecuta el reporte
        Dim objRSExecInfo As RS.ExecutionInfo = Nothing '//  Información de la ejecución iniciada por Reporting Services para generar el reporte
        Dim objRSExecHeader As RS.ExecutionHeader = Nothing '//  Información de la ejecución iniciada por Reporting Services para generar el reporte
        Dim objRSParametro As RS.ParameterValue = Nothing
        Dim objParametrosValor As Dictionary(Of String, String) = Nothing '// Parámetros recibidos para el reporte. La clave es el nombre del parámetro.
        Dim abytReporte As Byte() = Nothing '//  Reporte serializado retornado por Reporting Services
        Dim aobjWarning As RS.Warning() = Nothing '//  Lista de warnings identificados durante la generación del reporte
        Dim astrStreamId As String() = Nothing '//  Identificador retornado por reporting services de la serialización del reporte
        Dim colRSParametros As List(Of RS.ParameterValue) = Nothing '//  Lista de parámetros que se deben pasar a reporting services para ejecutar el reporte
        Dim objFilestream As System.IO.FileStream = Nothing '// Acceso al archivo físico que se debe generar para guardar el archivo
        Dim astrParametros As String() = Nothing
        Dim astrParametroValor As String() = Nothing
        Dim astrParametrosEnNombre As String() = Nothing

        Try
            strFuenteError = "A2FUENTE: Componnete A2.Documentos; Método generarArchivo; Servidor RS: " & pstrUrlRS & "; Reporte: " & pstrReporte & "; Formato: " & pstrTipoArchivo & "; Destino: " & pstrRutaArchivo
        Catch ex As Exception
            strFuenteError = String.Empty
        End Try

        Try
            '// Validar la ruta destino
            pstrRutaArchivo = pstrRutaArchivo.Trim()
            If pstrRutaArchivo.Equals(String.Empty) Then
                '// Si no se recibe una ruta valida se genera el archivo en la carpeta "Generados" dentro de la carpeta donde está el ejecutable
                pstrRutaArchivo = CurDir() & "\Generados"

                If Not System.IO.Directory.Exists(pstrRutaArchivo) Then
                    System.IO.Directory.CreateDirectory(pstrRutaArchivo)
                End If
            End If

            '// Separar los textos recibidos con los parámetros en vectores qu permites su recorrido
            pstrParametros = pstrParametros.Trim()
            pstrParametrosEnNombre = pstrParametrosEnNombre.Trim()

            '// Extraer parámetros para el reporte
            objParametrosValor = New Dictionary(Of String, String)
            If Not pstrParametros.Equals(String.Empty) Then
                astrParametros = pstrParametros.Split(pstrSeparador.ToArray)

                For intI = 0 To astrParametros.Length - 1
                    intPos = astrParametros(intI).IndexOf("=")
                    If intPos > 0 Then
                        strParametro = astrParametros(intI).Substring(0, intPos)
                        strValorParametro = astrParametros(intI).Substring(intPos + 1)
                        objParametrosValor.Add(strParametro, strValorParametro)
                    End If
                Next intI
            End If

            '// Extraer nombre de parámetros para incluir en el nombre del archivo
            If Not pstrParametrosEnNombre.Equals(String.Empty) Then
                astrParametrosEnNombre = pstrParametrosEnNombre.Split(pstrSeparador.ToArray)
            End If

            '// Definir formato para enviar a Reporting Services indicando el tipo de reporte a generar
            strFormato = consultarFormato(pstrTipoArchivo)
        Catch ex As Exception
            logError = True
            strMsgError = "Se generó un error al recibir los parámetros para el reporte de Reporting Services."
            If plogLanzarError Then
                Throw New ApplicationException(strMsgError, ex)
            Else
                strMsgError &= vbNewLine & strFuenteError & vbNewLine & vbNewLine & "ERROR: " & ex.Message
            End If
        End Try

        If logError = False Then
            '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            '// Configurar y ejecutar el reporte
            '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Try
                '// Crear el proxy al servidor de Reporting Services
                objRSExec = New RS.ReportExecutionServiceSoapClient("ReportExecutionServiceSoap", pstrUrlRS)
            Catch ex As Exception
                logError = True
                strMsgError = "Se generó un error al establecer la conexión al servidor de Reporting Services."
                ex.Source = strFuenteError & vbNewLine & ex.Source
                If plogLanzarError Then
                    Throw New ApplicationException(strMsgError, ex)
                Else
                    strMsgError &= vbNewLine & strFuenteError & vbNewLine & vbNewLine & "ERROR: " & ex.Message
                End If
            End Try

            Try
                '// Establecer la identificación del usuario que ejecuta el reporte. RS solamente funciona con seguridad integrada, se envía el usuario que ejecuta el proceso.
                If logError = False Then
                    objRSExec.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation
                End If
            Catch ex As Exception
                logError = True

                objRSExec.Close()
                objRSExec = Nothing

                strMsgError = "Se generó un error al configurar la seguridad de acceso al servidor de Reporting Services."
                ex.Source = strFuenteError & vbNewLine & ex.Source
                If plogLanzarError Then
                    Throw New ApplicationException(strMsgError, ex)
                Else
                    strMsgError &= vbNewLine & strFuenteError & vbNewLine & vbNewLine & "ERROR: " & ex.Message
                End If
            End Try

            Try
                If logError = False Then
                    '// Cargar el reporte en Reporting Services para poder realizar su configuración y ejecución
                    objRSExecHeader = objRSExec.LoadReport(Nothing, pstrReporte, strRSHistoryID, objRSServerInfo, objRSExecInfo)

                    '// Configurar los parámetros para la ejecución del reporte
                    If objParametrosValor.Count > 0 Then
                        colRSParametros = New List(Of RS.ParameterValue)
                        For Each objParam In objParametrosValor
                            objRSParametro = New RS.ParameterValue
                            objRSParametro.Name = objParam.Key
                            objRSParametro.Value = objParam.Value
                            colRSParametros.Add(objRSParametro)
                        Next objParam

                        objRSServerInfo = objRSExec.SetExecutionParameters(objRSExecHeader, Nothing, colRSParametros.ToArray(), "", objRSExecInfo)
                    End If
                End If
            Catch ex As Exception
                logError = True

                objRSExec.Close()
                objRSExec = Nothing

                strMsgError = "Se generó un error durante la configuración del reporte para su ejecución en el servidor de Reporting Services."
                ex.Source = strFuenteError & vbNewLine & ex.Source
                If plogLanzarError Then
                    Throw New ApplicationException(strMsgError, ex)
                Else
                    strMsgError &= vbNewLine & strFuenteError & vbNewLine & vbNewLine & "ERROR: " & ex.Message
                End If
            End Try

            Try
                '// Ejecutar el reporte y recibir el resultado serializado
                If logError = False Then
                    objRSServerInfo = objRSExec.Render(objRSExecHeader, Nothing, strFormato, "", abytReporte, pstrTipoArchivo, "", pstrEncoding, aobjWarning, astrStreamId)
                End If
            Catch ex As Exception
                logError = True

                strMsgError = "Se generó un error durante la generación del reporte en el servidor de Reporting Services."
                ex.Source = strFuenteError & vbNewLine & ex.Source

                If plogLanzarError Then
                    Throw New ApplicationException(strMsgError, ex)
                Else
                    strMsgError &= vbNewLine & strFuenteError & vbNewLine & vbNewLine & "ERROR: " & ex.Message
                End If
            Finally
                If Not objRSExec Is Nothing Then objRSExec.Close()
                objRSExec = Nothing
                objRSServerInfo = Nothing
                objRSExecHeader = Nothing
                objRSExecInfo = Nothing
            End Try

            '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            '// Guardar el reporte en un archivo de aucerdo con el formato indicado en la ruta recibida
            '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Try
                If logError = False Then
                    '// Definir la ruta y nombre del archivo
                    strArchivo = pstrRutaArchivo & "\" & pstrPrefijoArchivo

                    If Not astrParametrosEnNombre Is Nothing Then
                        For intI = 0 To astrParametrosEnNombre.Length - 1
                            strArchivo &= "_" & Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(astrParametrosEnNombre(intI), "/", "_"), "\", "_"), "?", "_"), "*", "_"), """", "_"), "<", "_"), ">", "_"), "|", "_")
                        Next intI
                    End If

                    strArchivo &= "." & pstrTipoArchivo

                    '// Guardar el resultado recibido del reporte en el archivo indicado
                    objFilestream = System.IO.File.OpenWrite(strArchivo.ToLower)
                    logArchivoAbierto = True
                    objFilestream.Write(abytReporte, 0, abytReporte.Length)
                    objFilestream.Close()
                End If
            Catch ex As Exception
                logError = True
                strMsgError = "Se generó un error al guardar el reporte en la ruta indicada."

                If plogLanzarError Then
                    Throw New ApplicationException(strMsgError, ex)
                Else
                    strMsgError &= vbNewLine & strFuenteError & vbNewLine & vbNewLine & "ERROR: " & ex.Message
                End If
            Finally
                If logArchivoAbierto Then
                    objFilestream.Close()
                End If
                objFilestream = Nothing
            End Try
        End If

        If logError Then
            Return (REPORTE_ERROR & strMsgError)
        Else
            Return (strArchivo)
        End If
    End Function

    Private Function consultarFormato(ByVal pstrTipoArchivo As String) As String

        Dim strFormato As String = "PDF"

        Try
            Select Case pstrTipoArchivo.ToUpper()
                Case "CSV"
                    strFormato = "CSV"
                Case "DOC"
                    strFormato = "WORD"
                Case "XLS", "XLSX"
                    strFormato = "EXCEL"
                Case "HTML"
                    strFormato = "HTML4.0"
                Case "MHTM"
                    strFormato = "MHTML"
            End Select
        Catch ex As Exception

        End Try

        Return (strFormato)
    End Function
End Class
