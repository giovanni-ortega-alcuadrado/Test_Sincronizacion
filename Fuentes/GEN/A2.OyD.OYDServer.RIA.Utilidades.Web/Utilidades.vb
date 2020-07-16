Imports System.IO
Imports A2Utilidades.Utilidades
Imports System.Web
Imports System.Data.SqlClient
Imports A2.OyD.Infraestructura
Imports System.Text




Module Utilidades

    Public Function CampoTabla(ByVal pstrIdCondicion As String, ByVal pstrCampo As String, ByVal pstrTabla As String, ByVal pstrCondicion As String) As String
        Dim dcUtil As New OYDUtilidadesDataContext
        Dim strRetorno As String = String.Empty

        Dim obj = dcUtil.uspOyDNet_Utilidades_CampoTabla(pstrIdCondicion, pstrCampo, pstrTabla, pstrCondicion).FirstOrDefault()

        If Not obj Is Nothing Then
            strRetorno = obj.Resultado.ToString()
        End If

        Return strRetorno
    End Function

    Public Function GuardarArchivo(ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal NombreArchivo As String, ByVal Lineas As IEnumerable(Of String), ByVal UtilizaSaltoLinea As Boolean) As Boolean
        Dim archivoServidor As FileStream = Nothing
        Dim writer As StreamWriter = Nothing

        Try
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            If Not System.IO.Directory.Exists(objDatosRutas.RutaArchivosLocal) Then
                System.IO.Directory.CreateDirectory(objDatosRutas.RutaArchivosLocal)
            End If

            archivoServidor = File.Create(objDatosRutas.RutaArchivosLocal + "\" + NombreArchivo)
            writer = New StreamWriter(archivoServidor, Encoding.GetEncoding(1252))

            For Each linea As String In Lineas
                If UtilizaSaltoLinea Then
                    writer.WriteLine(linea)
                Else
                    writer.Write(linea)
                End If
            Next

            writer.Close()
            archivoServidor.Close()

            Return True
        Catch ex As Exception
            If Not archivoServidor Is Nothing Then
                archivoServidor.Close()
            End If
            If Not writer Is Nothing Then
                writer.Close()
            End If
            ManejarError(ex, "Tesoreria", "GuardarArchivo")
            Return False
        End Try
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
End Module
