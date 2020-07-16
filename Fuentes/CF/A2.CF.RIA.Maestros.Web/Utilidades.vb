Imports System.IO
Imports A2Utilidades.Utilidades
Imports System.Web
Imports System.Data.SqlClient

Module Utilidades

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
    Friend Enum TipoSeparador
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
    Friend Enum TipoExportacion
        EXCEL
        CSV
        TXT
        PLANO
        AMBOS
        EXCELVIEJO
    End Enum

    Public Sub ManejarError(ByVal ex As Exception, ByVal Modulo As String, ByVal Funcion As String)
        'Si hay que generar log, entonces llamo al componente encargado de la generación de lo log
        A2Utilidades.Utilidades.registrarErrores(ex, Modulo, Funcion)
    End Sub



    Public Sub GrabarLog(ByVal Mensaje As String, ByVal Modulo As String, ByVal Funcion As String)
        Dim strSistema As String = "OyDServer"
        Dim strVersion As String = "1.0"
        Dim strFuente As String = "RIA"
        Dim strSufijoArchivo As String = "riaweb"
        Dim strRuta As String = ""

        A2Utilidades.Utilidades.generarLog(Mensaje, strSistema, strVersion, Modulo, Funcion, strFuente, strSufijoArchivo, strRuta)

    End Sub

    Public Function DemeInfoSesion(ByVal pstrModulo As String, Optional ByVal pstrUsuario As String = "") As String
        Dim ip = HttpContext.Current.Request.UserHostAddress
        If pstrUsuario Is Nothing Then
            pstrUsuario = String.Empty
        End If
        Dim user = IIf(pstrUsuario.Trim().Equals(String.Empty), HttpContext.Current.User.Identity.Name, pstrUsuario)
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

    'Public Function CampoTabla(ByVal pstrIdCondicion As String, ByVal pstrCampo As String, ByVal pstrTabla As String, ByVal pstrCondicion As String) As String
    '    Dim dcUtil As New MaestrosCalculosFinancierosDBML
    '    Dim strRetorno As String = String.Empty

    '    Dim obj = dcUtil.uspOyDNet_Utilidades_CampoTabla(pstrIdCondicion, pstrCampo, pstrTabla, pstrCondicion).FirstOrDefault()

    '    If Not obj Is Nothing Then
    '        strRetorno = obj.Resultado.ToString()
    '    End If

    '    Return strRetorno
    'End Function

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
    Friend Function CrearSQLParameter(ByVal pstrNombre As String, ByVal pobjValor As Object, ByVal pobTipo As SqlDbType) As SqlParameter
        Dim objParametro As New SqlParameter()
        objParametro.ParameterName = "@" & pstrNombre
        objParametro.DbType = pobTipo
        objParametro.Value = pobjValor
        Return objParametro
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
    Friend Function RetornarDataTable(ByVal pstrConexion As String, ByVal pstrNombreProcedimiento As String, ByVal pobjParametros As List(Of SqlParameter)) As DataTable

        Dim objDatos As DataTable = Nothing
        Dim objCommand As SqlCommand = Nothing
        Dim objDataReader As SqlDataReader

        Try
            objCommand = New SqlCommand()
            objCommand.Connection = New SqlConnection(pstrConexion)
            objCommand.CommandTimeout = 300
            objCommand.CommandType = CommandType.StoredProcedure
            objCommand.CommandText = pstrNombreProcedimiento

            For Each li In pobjParametros
                objCommand.Parameters.Add(li)
            Next

            If objCommand.Connection.State <> ConnectionState.Open Then
                objCommand.Connection.Open()
            End If

            objDataReader = objCommand.ExecuteReader

            objDatos = New DataTable()

            objDatos.Load(objDataReader)

            objDataReader.Close()

        Catch ex As Exception
            If objCommand IsNot Nothing AndAlso objCommand.Connection IsNot Nothing Then
                If objCommand.Connection.State <> ConnectionState.Closed Then
                    objCommand.Connection.Close()
                End If
            End If
            Throw
        End Try

        Return objDatos

    End Function

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
    Friend Function GenerarExcel(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String) As String
        Try

            Dim strExtensionNombreArchivo As String = ".xlsx"
            Dim objWorkbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook
            Dim objWorkSheet As SpreadsheetGear.IWorksheet = objWorkbook.Worksheets(0)
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
    Friend Function GenerarExcelVersionesViejas(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String) As String
        Try

            Dim strExtensionNombreArchivo As String = ".xls"
            Dim objWorkbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook
            Dim objWorkSheet As SpreadsheetGear.IWorksheet = objWorkbook.Worksheets(0)
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
    Friend Function GenerarTextoPlano(pobjDatos As DataTable, pstrRuta As String, pstrNombreArchivo As String, ByVal pstrExtension As String, pstrSeparador As String) As String

        Try
            'Inicio JCS - Abril 08/2014 - Codigo quemado.
            pstrExtension = "txt"
            'Fin JCS - Abril 08/2014 - Codigo quemado.

            Dim strBackSlash As String = "\"
            Dim strNombreArchivo As String = String.Empty


            If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                pstrRuta = pstrRuta & strBackSlash
            End If


            If pstrExtension <> TipoExportacion.PLANO.ToString And pstrExtension <> TipoExportacion.AMBOS.ToString Then       ' JCS Abril 01/2014
                If Right(pstrNombreArchivo, 1) <> "." Then
                    pstrNombreArchivo = pstrNombreArchivo & "."
                End If
                strNombreArchivo = pstrNombreArchivo & pstrExtension
            Else
                strNombreArchivo = pstrNombreArchivo
            End If

            Dim strRutaArchivoCompleta As String = pstrRuta & strNombreArchivo '& pstrExtension

            'Nuevo objeto StreamWriter, para acceder al fichero y poder guardar las líneas  
            Using archivo As StreamWriter = New StreamWriter(strRutaArchivoCompleta)

                ' variable para almacenar la línea actual del dataview  
                Dim linea As String = String.Empty

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
            End Using

            ' Abrir con Process.Start el archivo de texto  
            ' Process.Start(strRutaArchivoCompleta)
            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function

End Module
