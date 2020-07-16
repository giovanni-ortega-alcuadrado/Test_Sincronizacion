'Codigo Creado Por: Rafael Cordero
'Archivo: ImportacionesDomainService.vb
'Generado el : 07/12/2011 
'Propiedad de Alcuadrado S.A. 2011

Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Linq
Imports System.Linq
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Text
Imports System.Web.SessionState
Imports System.Configuration
Imports A2Utilidades.Cifrar
Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports System.Xml
Imports A2.OyD.OYDServer.RIA.Web.clsProcesosArchivo
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()>
Public Class ImportacionesDomainService
    Inherits LinqToSqlDomainService(Of OyD_ImportacionesDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

    Public Const CSTR_NOMBREPROCESO_RECIBOSDECAJA = "RECIBOSDECAJA"
    Private RegBuenos As Long
    Dim RETORNO As Boolean
    Private Shared SEPARATOR_FORMAT_CVS As String = System.Globalization.CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator

    Public Enum FormatoImportacion
        EspecieVencimiento
        IsinAnna
    End Enum

    Public Function GetArchivos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.Archivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.Archivos
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetArchivos")
            Return Nothing
        End Try
    End Function

    Public _objTitulosValorizados As New EntitySet(Of ListaTitulosValorizados)
    Public _objResultados As New List(Of String)
    Private _lstLineaComentario As New List(Of LineaComentario)
    Private glngIDComisionista As Long = 0
    Private mLogANNA As Boolean = False
    Public Function CargarArchivoServiciosBolsa(ByVal pstrNombreCompletoArchivo As String,
                                                   ByVal pdtmDesde As DateTime, ByVal pdtmHasta As DateTime, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal ReemplazaValor As Boolean, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.ComentarioServicioBolsa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal

            Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
            Dim ret = objImportar.EjecutaprocesoServicioBolsa(directorioUsuario & "\" & pstrNombreCompletoArchivo, pdtmDesde, pdtmHasta, ReemplazaValor, pstrUsuario)
            If Not IsNothing(ret) Then
                Dim li As New List(Of ComentarioServicioBolsa)
                li.Add(New ComentarioServicioBolsa With {.idConsecutivo = -1, .inconsistencia = ret})
                Return li
            End If
            Dim list = Me.DataContext.usp_ValidarLiquidacionServicioBolsa_OyDNet(pdtmDesde, pdtmHasta, ReemplazaValor).ToList
            Return list

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoServiciosBolsa")
            Return Nothing
        End Try

    End Function
    Public Function actualizaServiciosBolsa(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim registros As System.Nullable(Of Integer)
            Dim e = Me.DataContext.usp_ActualizarServiciosBolsa_OyDNet(registros)
            Return registros
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "actualizaServiciosBolsa")
            Return Nothing
        End Try

    End Function
    Public Function TransladarServiciosBolsaLog(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.usp_TransladarServiciosBolsaLog
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransladarServiciosBolsaLog")
            Return Nothing
        End Try
    End Function

    ''' <history>
    ''' Modificado por   : Jhon bayron torres pelaez
    ''' Descripción      : guarda el archivo FATCA
    ''' Fecha            : Abril 10/2014
    ''' Pruebas CB       : Jhon bayron torres pelaez- Abril 10/2014 - Resultado Ok 
    ''' </history>
    Public Function GuardarArchivoFATCA(ByVal pdtmFechadesde As DateTime _
                                       , ByVal pdtmFechahasta As DateTime, ByVal pstrNombreArchivo As String, ByVal pstrUsuario As String, ByVal pstrRuta As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim lista As New List(Of String)
            Dim ret = Me.DataContext.uspOyDNet_GenerarArchivoFATCA(pdtmFechadesde, pdtmFechahasta, pstrNombreArchivo, pstrRuta, pstrUsuario, DemeInfoSesion(pstrUsuario, "GuardarArchivoFATCA"), 0)
            For Each li In ret
                lista.Add(li.strResultado)
            Next
            Dim archivo = pstrNombreArchivo.Replace(":", "_")
            Dim resultado = GuardarArchivoOtraruta(pstrRuta, pstrUsuario, archivo, lista, True, pstrInfoConexion)
            If resultado Then
                Return pstrRuta
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GuardarArchivoFATCA")
            Return Nothing
        End Try
    End Function
    ''' <history>
    ''' Modificado por   : Jhon bayron torres pelaez
    ''' Descripción      : lee el archivo FATCA core
    ''' Fecha            : Abril 10/2014
    ''' Pruebas CB       : Jhon bayron torres pelaez- Abril 10/2014 - Resultado Ok 
    ''' </history>
    Public Function LeerArchivoFATCA(ByVal pstrNombreCompletoArchivo As String _
                                            , ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrArchivoFormato As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Dim objStringordenes As StringBuilder = Nothing

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}


            Dim ret = Me.DataContext.uspLeerArchivoFATCA(pstrUsuario.Replace(".", "_") & objDatosRutas.NombreProceso & "\" & pstrNombreCompletoArchivo, pstrArchivoFormato, pstrUsuario, DemeInfoSesion(pstrUsuario, "LeerArchivoFATCA"), 0).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LeerArchivoFATCA")
            Return Nothing
        End Try
    End Function

    ''' <history>
    ''' Modificado por   : Giovanny Velez Bolivar
    ''' Descripción      : lee el archivo RC
    ''' Fecha            : Noviembre 18/2014
    ''' Pruebas CB       : Giovanny Velez Bolivar - Noviembre 18/2014 - Resultado Ok 
    ''' </history>
    Public Function LeerArchivoRC(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String,
                                  ByVal pstrNombreProcesoRespuesta As String, ByVal pstrUsuario As String,
                                  ByVal pintCodigoBanco As Integer, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_RC_Carga_Generar(pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso & "\" & pstrNombreCompletoArchivo,
                                                                          pstrUsuario.Replace(".", "_") & "\" & pstrNombreProcesoRespuesta & "\" & pstrNombreCompletoArchivo,
                                                                          pstrUsuario, pintCodigoBanco, DemeInfoSesion(pstrUsuario, "LeerArchivoRC"), 0).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LeerArchivoRC")
            Return Nothing
        End Try
    End Function

#Region "Upload Files"

    Public Function TraerArchivosDirectorio(ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.Archivo)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ValidarCaracteresInvalidosEnRuta(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_CaracteresInvalidosCarpeta, pstrNombreProceso)
            ValidarCaracteresInvalidosEnRuta(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_CaracteresInvalidosCarpeta, pstrUsuario)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            If Not Directory.Exists(objDatosRutas.RutaArchivosLocal) Then
                Directory.CreateDirectory(objDatosRutas.RutaArchivosLocal)
            End If

            Dim files As ReadOnlyCollection(Of String)
            Dim archivos As New List(Of OyDImportaciones.Archivo)
            files = My.Computer.FileSystem.GetFiles(objDatosRutas.RutaArchivosLocal, FileIO.SearchOption.SearchTopLevelOnly, "*.*")

            For I = 0 To files.Count - 1
                Dim f = files(I)
                Dim fileData As FileInfo = My.Computer.FileSystem.GetFileInfo(f)

                archivos.Add(New OyDImportaciones.Archivo With {.Ruta = f, .Nombre = fileData.Name, .Extension = fileData.Extension, .KBytes = CInt(fileData.Length / 1024),
                                               .FechaHora = fileData.LastWriteTime, .RutaWeb = objDatosRutas.RutaCompartidaOWeb() & fileData.Name})
            Next

            If CBool(My.Settings.HabilitaDebbugImportar) Then
                archivos.Add(New OyDImportaciones.Archivo With {.Ruta = objDatosRutas.MensajeDebbug, .Nombre = objDatosRutas.MensajeDebbug, .Extension = objDatosRutas.MensajeDebbug, .KBytes = 0,
                                                   .FechaHora = Now.Date, .RutaWeb = objDatosRutas.RutaCompartidaOWeb() & objDatosRutas.MensajeDebbug})
            End If

            Return archivos.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerArchivosDirectorio")
            Return Nothing
        End Try

    End Function

    Public Function GuardarArchivo(ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal NombreArchivo As String, ByVal Lineas As IEnumerable(Of String), ByVal UtilizaSaltoLinea As Boolean, ByVal pstrInfoConexion As String) As Boolean
        Dim archivoServidor As FileStream = Nothing
        Dim writer As StreamWriter = Nothing

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
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
            ManejarError(ex, Me.ToString(), "GuardarArchivo")
            Return False
        End Try
    End Function

    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Este método se invoca desde Guardar_ArchivoServidorWord
    ''' Fecha            : Junio 12/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 12/2013 - Resultado Ok 
    ''' </history>
    Public Function GuardarArchivoWord(ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal NombreArchivo As String, ByVal Lineas As String, ByVal UtilizaSaltoLinea As Boolean, ByVal pstrInfoConexion As String) As Boolean
        Dim archivoServidor As FileStream = Nothing
        Dim writer As StreamWriter = Nothing

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            If Not System.IO.Directory.Exists(objDatosRutas.RutaArchivosLocal) Then
                System.IO.Directory.CreateDirectory(objDatosRutas.RutaArchivosLocal)
            End If

            archivoServidor = File.Create(objDatosRutas.RutaArchivosLocal + "\" + NombreArchivo)
            writer = New StreamWriter(archivoServidor, Encoding.GetEncoding(1252))

            writer.Write(Lineas)

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
            ManejarError(ex, Me.ToString(), "GuardarArchivo")
            Return False
        End Try
    End Function

    Public Sub BorrarArchivo(ByVal ArchivoABorrar As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String)
        Dim directorioBorrar As String = ""

        Try

            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            directorioBorrar = objDatosRutas.RutaArchivosLocal

            GrabarLog("Comenzando a borrar archivo" & ArchivoABorrar, My.Application.Info.AssemblyName, "BorrarArchivo")

            If Not IsNothing(ArchivoABorrar) Then
                Dim rutaArchivos = directorioBorrar
                Dim fileExists = My.Computer.FileSystem.FileExists(rutaArchivos & "\" & ArchivoABorrar)
                If fileExists Then
                    My.Computer.FileSystem.DeleteFile(rutaArchivos & "\" & ArchivoABorrar, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                End If
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BorrarArchivo")
        End Try
    End Sub

    Public Function GuardarArchivoOtraruta(ByVal pstrRuta As String, ByVal pstrUsuario As String, ByVal NombreArchivo As String, ByVal Lineas As IEnumerable(Of String), ByVal UtilizaSaltoLinea As Boolean, ByVal pstrInfoConexion As String) As Boolean
        Dim archivoServidor As FileStream = Nothing
        Dim writer As StreamWriter = Nothing

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Dim objDatosRutas = FnRutasImportaciones(pstrNombreProceso, pstrUsuario)

            If Not System.IO.Directory.Exists(pstrRuta) Then
                System.IO.Directory.CreateDirectory(pstrRuta)
            End If

            archivoServidor = File.Create(pstrRuta + "\" + NombreArchivo)
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
            ManejarError(ex, Me.ToString(), "GuardarArchivoOtraruta")
            Return False
        End Try
    End Function

#End Region

#Region "ImportacionLiq_Mi"

    Public Function EliminarImportados_MI(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
        Dim MensajeError As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objImportar As New clsImportarLiquidaciones_MI With {.gstrUser = My.User.Name}
            MensajeError = objImportar.BorrarEncabezados_MI()

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = MensajeError})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarImportados_MI")
            Return Nothing
        End Try

    End Function





    Public Function CargarArchivoLiquidaciones_MI(ByVal pstrNombreCompletoArchivoLiquidaciones As String,
                                          ByVal pblnEstructuraActual As Boolean, ByVal pdtmDesde As DateTime, ByVal pdtmHasta As DateTime, ByVal pstrNombreProceso As String,
                                          ByVal pstrUsuario As String, ByVal FirmaExtrangera As Boolean, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal

            Dim objImportar As New clsImportarLiquidaciones_MI With {.gstrUser = My.User.Name}
            Dim ret = objImportar.EvaluarLineaColombia_MI(directorioUsuario & "\" & pstrNombreCompletoArchivoLiquidaciones, pblnEstructuraActual, pdtmDesde, pdtmHasta, FirmaExtrangera, pstrUsuario)

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = ret})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoLiquidaciones_MI")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Importar Precios Especies"

    ''' <summary>
    ''' Función que permite leer un archivo con los Precios de las Especies
    ''' </summary>
    ''' <param name="pstrNombreCompletoArchivoPreciosEspecies"></param>
    ''' <param name="pdtmDesde"></param>
    ''' <param name="pstrNombreProceso"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna un string con la respuesta de la lectura del archivo</returns>
    ''' <remarks>SLB20130320</remarks>
    Public Function CargarArchivoPreciosEspecies(ByVal pstrNombreCompletoArchivoPreciosEspecies As String,
                                            ByVal pdtmDesde As DateTime, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyD.Net\V1.1.1\VS2010\A2.OyD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
            Dim ret = objImportar.EvaluarPreciosEspecie(directorioUsuario & "\" & pstrNombreCompletoArchivoPreciosEspecies, pdtmDesde, pstrUsuario)

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = ret})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoPreciosEspecies")
            Return Nothing
        End Try

    End Function

#End Region

#Region "ImportacionLiq"

    Public Function CargarArchivoLiquidaciones(ByVal pstrNombreCompletoArchivoLiquidaciones As String,
                                              ByVal pblnEstructuraActual As Boolean, ByVal pdtmDesde As DateTime, ByVal pdtmHasta As DateTime,
                                              ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pdtmFechaCierre As DateTime, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyDVersiones\v12.2\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
            Dim ret = objImportar.EvaluarLineaColombia(directorioUsuario & "\" & pstrNombreCompletoArchivoLiquidaciones, pblnEstructuraActual, pdtmDesde, pdtmHasta, pstrUsuario, pdtmFechaCierre)

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = ret})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoLiquidaciones")
            Return Nothing
        End Try

    End Function

    Public Function EliminarImportados(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
        Dim MensajeError As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
            MensajeError = objImportar.BorrarEncabezados()

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = MensajeError})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarImportados")
            Return Nothing
        End Try

    End Function

    Public Function EliminarImportadosLiq(ByVal Respuesta As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
        Dim MensajeError As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
            Dim ret = Me.DataContext.usp_LIQUIDACIONES_EliminarActualizarImportacion(Respuesta, pstrUsuario, DemeInfoSesion(pstrUsuario, "EliminarImportadosLiq"), 0).ToList

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = MensajeError})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarImportadosLiq")
            Return Nothing
        End Try
    End Function

    Public Function EspecieExiste(ByVal pstrIdEspecie As String, ByVal pintIdBolsa As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Dim MensajeError As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)


            Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
            MensajeError = objImportar.BorrarEncabezados()

            Return Nothing

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarImportados")
            Return Nothing
        End Try

    End Function

    Public Function ImportacionLiqFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ImportacionLi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Importaciones_ImportacionLiq_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "ImportacionLiqFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ImportacionLiqFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ImportacionLiqConsultar(ByVal pID As Integer, ByVal parcial As Integer, ByVal pLiquidacion? As DateTime, ByVal Comitente As String, ByVal especie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ImportacionLi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Importaciones_ImportacionLiq_Consultar(pID, parcial, pLiquidacion, Comitente, especie, DemeInfoSesion(pstrUsuario, "BuscarImportacionLiq"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarImportacionLiq")
            Return Nothing
        End Try
    End Function

    Public Function TraerImportacionLiPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ImportacionLi
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ImportacionLi
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = 0
            e.Parcial = 0
            e.Tipo = String.Empty
            e.ClaseOrden = String.Empty
            'e.IDEspecie = 
            'e.IDOrden = 
            'e.IDComitente = 
            'e.IDOrdenante = 
            e.IDBolsa = 0
            'e.IDRueda = 
            'e.ValBolsa = 
            'e.TasaDescuento = 
            'e.TasaCompraVende = 
            'e.Modalidad = 
            'e.IndicadorEconomico = 
            'e.PuntosIndicador = 
            'e.Plazo = 
            e.Liquidacion = Now
            'e.Cumplimiento = 
            'e.Emision = 
            'e.Vencimiento = 
            'e.OtraPlaza = 
            'e.Plaza = 
            'e.IDComisionistaLocal = 
            'e.IDComisionistaOtraPlaza = 
            'e.IDCiudadOtraPlaza = 
            'e.TasaEfectiva = 
            'e.Cantidad = 
            'e.Precio = 
            'e.Transaccion = 
            'e.SubTotalLiq = 
            'e.TotalLiq = 
            'e.Comision = 
            'e.Retencion = 
            'e.Intereses = 
            'e.ValorIva = 
            'e.DiasIntereses = 
            'e.FactorComisionPactada = 
            'e.Mercado = 
            'e.NroTitulo = 
            'e.IDCiudadExpTitulo = 
            'e.PlazoOriginal = 
            'e.Aplazamiento = 
            'e.VersionPapeleta = 
            'e.EmisionOriginal = 
            'e.VencimientoOriginal = 
            'e.Impresiones = 
            'e.FormaPago = 
            'e.CtrlImpPapeleta = 
            'e.DiasVencimiento = 
            'e.PosicionPropia = 
            'e.Transaccion = 
            'e.TipoOperacion = 
            'e.DiasContado = 
            'e.Ordinaria = 
            'e.ObjetoOrdenExtraordinaria = 
            'e.NumPadre = 
            'e.ParcialPadre = 
            'e.OperacionPadre = 
            'e.DiasTramo = 
            'e.Vendido = 
            'e.Vendido = 
            'e.ValorTraslado = 
            'e.ValorBrutoCompraVencida = 
            'e.AutoRetenedor = 
            'e.Sujeto = 
            'e.PcRenEfecCompraRet = 
            'e.PcRenEfecVendeRet = 
            'e.Reinversion = 
            'e.Swap = 
            'e.NroSwap = 
            'e.Certificacion = 
            'e.DescuentoAcumula = 
            'e.PctRendimiento = 
            'e.FechaCompraVencido = 
            'e.PrecioCompraVencido = 
            'e.ConstanciaEnajenacion = 
            'e.RepoTitulo = 
            'e.ServBolsaVble = 
            'e.ServBolsaFijo = 
            'e.Traslado = 
            'e.UBICACIONTITULO = 
            'e.TipoIdentificacion = 
            'e.NroDocumento = 
            'e.ValorEntregaContraPago = 
            'e.AquienSeEnviaRetencion = 
            'e.IDBaseDias = 
            'e.TipoDeOferta = 
            'e.HoraGrabacion = 
            'e.OrigenOperacion = 
            'e.CodigoOperadorCompra = 
            'e.CodigoOperadorVende = 
            'e.IdentificacionRemate = 
            'e.ModalidaOperacion = 
            'e.IndicadorPrecio = 
            'e.PeriodoExdividendo = 
            'e.PlazoOperacionRepo = 
            'e.ValorCaptacionRepo = 
            'e.VolumenCompraRepo = 
            'e.PrecioNetoFraccion = 
            'e.VolumenNetoFraccion = 
            'e.CodigoContactoComercial = 
            'e.NroFraccionOperacion = 
            'e.IdentificacionPatrimonio1 = 
            'e.TipoidentificacionCliente2 = 
            'e.NitCliente2 = 
            'e.IdentificacionPatrimonio2 = 
            'e.TipoIdentificacionCliente3 = 
            'e.NitCliente3 = 
            'e.IdentificacionPatrimonio3 = 
            'e.IndicadorOperacion = 
            'e.BaseRetencion = 
            'e.PorcRetencion = 
            'e.BaseRetencionTranslado = 
            'e.PorcRetencionTranslado = 
            'e.PorcIvaComision = 
            'e.IndicadorAcciones = 
            'e.OperacionNegociada = 
            'e.FechaConstancia = 
            'e.ValorConstancia = 
            'e.GeneraConstancia = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.CodigoIntermediario = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerImportacionLiPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertImportacionLi(ByVal ImportacionLi As ImportacionLi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, ImportacionLi.pstrUsuarioConexion, ImportacionLi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ImportacionLi.InfoSesion = DemeInfoSesion(ImportacionLi.pstrUsuarioConexion, "InsertImportacionLi")
            Me.DataContext.ImportacionLiq.InsertOnSubmit(ImportacionLi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertImportacionLi")
        End Try
    End Sub

    Public Sub UpdateImportacionLi(ByVal currentImportacionLi As ImportacionLi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentImportacionLi.pstrUsuarioConexion, currentImportacionLi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentImportacionLi.InfoSesion = DemeInfoSesion(currentImportacionLi.pstrUsuarioConexion, "UpdateImportacionLi")
            Me.DataContext.ImportacionLiq.Attach(currentImportacionLi, Me.ChangeSet.GetOriginal(currentImportacionLi))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateImportacionLi")
        End Try
    End Sub

    Public Function GetListaTitulos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaTitulosValorizados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.ListaTitulosValorizados.ToList()
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetListaTitulos")
            Return Nothing
        End Try
    End Function

    'Public Function ActualizarImportacion(ByVal pdtmLiquidacion As System.Nullable(Of Date), _
    '                                      ByVal pstrTipo As String, _
    '                                      ByVal pstrClase As String, _
    '                                      ByVal plngID As System.Nullable(Of Integer), _
    '                                      ByVal plngParcial As System.Nullable(Of Integer), _
    '                                      ByVal plngIDOrden As System.Nullable(Of Integer), _
    '                                      ByVal pstrUsuario As String, _
    '                                      ByVal lngidBolsa As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)

    '    Dim ret = Me.DataContext.uspOyDNet_Importaciones_ActualizarImportaciones(pdtmLiquidacion, _
    '                                            pstrTipo, _
    '                                            pstrClase, _
    '                                            plngID, _
    '                                            plngParcial, _
    '                                            plngIDOrden, _
    '                                            pstrUsuario, _
    '                                            lngidBolsa, _
    '                                            DemeInfoSesion(pstrUsuario, "ActualizarImportacion"), _
    '                                           ClsConstantes.GINT_ErrorPersonalizado)


    '    Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
    '    lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = CStr(ret)})
    '    Return lstLineaComentario

    'End Function


    Public Sub DeleteImportacionLi(ByVal ImportacionLi As ImportacionLi)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ImportacionLi.pstrUsuarioConexion, ImportacionLi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Importaciones_ImportacionLiq_Eliminar( pID,  pTipo,  pClaseOrden,  pIDOrden,  pIDBolsa,  pLiquidacion,  pCumplimiento, DemeInfoSesion(pstrUsuario, "DeleteImportacionLi"),0).ToList
            ImportacionLi.InfoSesion = DemeInfoSesion(ImportacionLi.pstrUsuarioConexion, "DeleteImportacionLi")
            Me.DataContext.ImportacionLiq.Attach(ImportacionLi)
            Me.DataContext.ImportacionLiq.DeleteOnSubmit(ImportacionLi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteImportacionLi")
        End Try
    End Sub

    Public Function VerificarOrdenLiq(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDOrden As Integer,
                                      ByVal pstrIDEspecie As String, ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of VerificarOrdenLiq)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_verificarOrdenLiq_OyDNet(pstrTipo, pstrClase, plngIDOrden, pstrIDEspecie, pstrNroDocumento).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarOrdenLiq")
            Return Nothing
        End Try
    End Function

    Public Function CumplimientoOrden(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDOrden As Integer,
                                      ByVal pstrIDEspecie As String, ByVal pdblCantidadLiq? As Double,
                                      ByVal pdblCantidadOrden? As Double, ByVal pdblCantidadImportacion? As Double, ByVal plogEnPesos? As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String ' As Dictionary(Of Integer, Decimal)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Jorge Andres Bedoya 20150506 Se adiciono el parametro plogEnPesos 
            Dim ret = Me.DataContext.sp_CumplimientoOrden_OyDNet(pstrTipo, pstrClase, plngIDOrden, pstrIDEspecie, pdblCantidadLiq, pdblCantidadOrden, pdblCantidadImportacion, plogEnPesos)
            Dim variable As String
            variable = CStr(pdblCantidadLiq) + "," + CStr(pdblCantidadOrden) + "," + CStr(pdblCantidadImportacion) + "," + CStr(IIf(CBool(plogEnPesos = True), 1, 0))
            Return variable
            'Dim lista As New Dictionary(Of Integer, Decimal)
            'lista.Add(1, CDec(pdblCantidadLiq))
            'lista.Add(2, CDec(pdblCantidadOrden))
            'Return lista
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarOrdenLiq")
            Return Nothing
        End Try
    End Function

    Public Function PatrimonioTecnico(ByVal plngIDComitente As String, ByVal pdtmCorte As Date, ByVal pdblOtrosValores As Decimal, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PatrimonioTecnico)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspValorFuturoRepoConsultar_OyDNet(plngIDComitente, pdtmCorte, pdblOtrosValores).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarOrdenLiq")
            Return Nothing
        End Try
    End Function

    Public Function verificarLiqExistente(ByVal plngIDLiquidacion As Integer, ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pdtmLiquidacion As Date,
                                      ByVal pstrIDEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of verificarLiqExistente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_verificarLiqExistente_OyDNet(plngIDLiquidacion, pstrTipo, pstrClase, pdtmLiquidacion, pstrIDEspecie).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "verificarLiqExistente")
            Return Nothing
        End Try
    End Function
#End Region
#Region "ImportarCompensacion"
    Public Function CargarArchivoLiquidacionesCompensacion(ByVal pstrNombreCompletoArchivoLiquidaciones As String,
                                             ByVal pblnEstructuraActual As Boolean, ByVal pdtmDesde As DateTime, ByVal pdtmHasta As DateTime, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pbAcciones As Boolean, ByVal pbCrediticio As Boolean, ByVal pbAmbos As Boolean, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyDVersiones\v12\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
            Dim ret = objImportar.EvaluarLineaColombiaCompensacion(directorioUsuario & "\" & pstrNombreCompletoArchivoLiquidaciones, pblnEstructuraActual, pdtmDesde, pdtmHasta, pstrUsuario, pbAcciones, pbCrediticio, pbAmbos)

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = ret})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoLiquidacionesCompensacion")
            Return Nothing
        End Try

    End Function
    Public Function EliminarImportadosCompensacion(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario)
        Dim MensajeError As String = ""
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objImportar As New clsImportarLiquidaciones With {.gstrUser = My.User.Name}
            MensajeError = objImportar.BorrarEncabezadosCompensacion()

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = MensajeError})
            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarImportados")
            Return Nothing
        End Try

    End Function
#End Region
#Region "Impotar Titulos Valorizados"

    Public Function CargarArchivoValorizacionTitulos(ByVal pstrNombreCompletoArchivoValorizacion As String,
                                               ByVal pblnEstructuraActual As Boolean, ByVal pdtmDesde As DateTime, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pintTipoFormato As Integer, ByVal pstrInfoConexion As String) As List(Of ListaTitulosValorizados)

        Dim objReader As System.IO.StreamReader = Nothing

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim strdatos As String = String.Empty
            Dim intCual As Integer = 0

            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal

            Dim intEncabezado1 = FreeFile()

            objReader = New System.IO.StreamReader(pstrNombreCompletoArchivoValorizacion)

            Dim ret = Me.DataContext.uspOyDNet_Importaciones_ValorizacionTitulosBorrar(pdtmDesde, DemeInfoSesion(pstrUsuario, "CargarArchivoValorizacionTitulos"), ClsConstantes.GINT_ErrorPersonalizado)

            RegBuenos = 0
            _objTitulosValorizados.Clear()
            'Recorremos el archivo de encabezados
            Do While objReader.Peek() <> -1
                strdatos = objReader.ReadLine()
                If Not LeerLineaArchivoValoracion(intCual, strdatos, pdtmDesde, pstrUsuario, CType(pintTipoFormato, FormatoImportacion), pstrInfoConexion) Then
                    Exit Do
                End If
                intCual = intCual + 1
            Loop

            objReader.Close()

            'MsgBox("Grabación exitosa (" & CStr(intCual) & " registros)", CType(MsgBoxStyle.ApplicationModal + MsgBoxStyle.Information, MsgBoxStyle), "Proceso exitoso")
            Return _objTitulosValorizados.ToList()

        Catch ex As Exception
            If Not objReader Is Nothing Then objReader.Close()
            ManejarError(ex, Me.ToString(), "CargarArchivoValorizacionTitulos")
            Return Nothing
        Finally
            objReader.Dispose()
        End Try

    End Function

    Private Function LeerLineaArchivoValoracion(ByVal pintCual As Integer, ByVal pstrDatos As String, ByVal pDateDesde As DateTime, ByVal pstrUsuario As String, ByVal pTipoFormato As FormatoImportacion, ByVal pstrInfoConexion As String) As Boolean
        Dim intSecuencia As Integer = pintCual

        Dim intEncabezado As Integer = 0
        Dim lngSecuencia As Long = 0
        Dim strSecuencia As String = String.Empty
        Dim dtmFecha As Date = Nothing
        Dim dtmFechaVencimiento As System.Nullable(Of Date)
        Dim strIDEspecie As String = String.Empty
        Dim dblValor As Double = 0
        Dim intCual As Integer = 0
        Dim RegBuenos As Integer = 0
        Dim strDatos As String = String.Empty
        Dim PosicionGuion As Byte = 0
        Dim strFechaVencimiento As String = String.Empty
        Dim strFungible As String = String.Empty
        Dim lngFungible As Long = 0
        Dim logArchivoFungible As Boolean = Nothing
        Dim logTransaccion As Boolean = Nothing
        Dim strIsinANNA As String = String.Empty
        Dim bolRetorno As Boolean = True
        Dim strNombreCampoValidar As String = String.Empty
        Dim strIDEspecieOisinAnna As String = String.Empty

        Try

            strDatos = pstrDatos

            If Not IsNumeric(Right(Mid$(strDatos, 1, 5), 5)) Then
                'MsgBox("La secuencia es un campo numerico, requerido ")
                Throw New Exception("La secuencia es un campo numerico, requerido ")
                Return False
                'Exit Function
            End If
            lngSecuencia = CLng(Right(Mid$(strDatos, 1, 5), 5)) 'Secuencia

            logArchivoFungible = Len(strDatos) = 50 'LogFungible

            dtmFecha = DateSerial(CInt(Mid$(strDatos, 6, 4)), CInt(Mid$(strDatos, 10, 2)), CInt(Mid$(strDatos, 12, 2)))
            If Not IsDate(dtmFecha) Then
                'Call LogImportacion(pintCual, dtmLiquidacion, "La fecha de valorizacion tiene un formato no válido")
                'If logTransaccion Then gdb.RollbackTrans()
                'MsgBox("La fecha de valorizacion tiene un formato no válido")
                Throw New Exception("La fecha de valorizacion tiene un formato no válido")
                Return False
                'Exit Function
            End If

            If pDateDesde.ToShortDateString() <> dtmFecha.ToShortDateString() Then
                Dim strMsg As String = "La fecha de valorizacion del archivo " & dtmFecha.ToShortDateString() & " es diferente de la fecha enviada como parametro " & pDateDesde.ToShortDateString() & " Por favor verifique"
                'MsgBox(strMsg, MsgBoxStyle.SystemModal, "Información")
                Throw New ArgumentException(strMsg)
                Return False
                'Exit Function
            End If

            Dim LongEspecie As Integer
            If logArchivoFungible Then
                LongEspecie = 20
            Else ''Fecha de vencimiento
                LongEspecie = 18
            End If

            lngFungible = 0
            strIDEspecie = Mid$(strDatos, 14, LongEspecie) 'Extrae el nemo del título y su fecha de vencimiento, en formato aammdd (ambos se encuentran separados por un guión (-))
            'La última versión del archivo viene como Nemo-Fungible. Pero, por razones de actualización de versión, la Firma necesita cargar el archivo que viene como Nemo-Fecha Vencimiento (CGA, 26.Agosto.09)
            PosicionGuion = CByte(InStr(1, strIDEspecie, "-"))

            If PosicionGuion <> 0 Then
                'strFechaVencimiento = Mid$(strIDEspecie, PosicionGuion + 1, 6)

                'CGA, 26.Agosto.09 <------
                If logArchivoFungible Then
                    strFungible = Mid$(strIDEspecie, PosicionGuion + 1, 6)
                    lngFungible = CLng(strFungible)
                    strFechaVencimiento = ""
                Else
                    strFungible = ""
                    strFechaVencimiento = Mid$(strIDEspecie, PosicionGuion + 1, 6)
                    If Trim(strFechaVencimiento) <> "" Then
                        dtmFechaVencimiento = DateSerial(CInt(Mid$(strFechaVencimiento, 1, 2)), CInt(Mid$(strFechaVencimiento, 3, 2)), CInt(Mid$(strFechaVencimiento, 5, 2)))
                    Else
                        dtmFechaVencimiento = Nothing
                    End If
                End If
                '------>
                strIDEspecie = Mid$(strIDEspecie, 1, PosicionGuion - 1)
                strIsinANNA = ""

            Else

                strIDEspecie = Trim(strIDEspecie)

            End If
            '-------->

            If pTipoFormato = FormatoImportacion.IsinAnna Then
                strIsinANNA = Trim(strIDEspecie)
                strIDEspecie = ""
                strFechaVencimiento = ""
                strNombreCampoValidar = "strIsinANNA"
                strIDEspecieOisinAnna = strIsinANNA
            ElseIf pTipoFormato = FormatoImportacion.EspecieVencimiento Then
                strIDEspecieOisinAnna = Trim(strIDEspecie)
                strNombreCampoValidar = "strIDEspecie"
            End If

            dblValor = CDbl(Mid$(strDatos, 33, 17))  '+CGA, 5/Oct/09

            Dim objImportar As ListaTitulosValorizados = Nothing

            If logArchivoFungible Then
                objImportar = New ListaTitulosValorizados With {.Secuencia = CStr(lngSecuencia),
                                                                   .Fvalorizacion = Mid$(dtmFecha.ToString(), 1, 10),
                                                                   .IdEspecie = strIDEspecie,
                                                                   .Fungible = CStr(lngFungible),
                                                                   .FechaVencimiento = "",
                                                                   .ValorEspecie = CType(dblValor, Decimal?),
                                                                   .Registro_Completo = strDatos,
                                                                   .IsinAnna = strIsinANNA}


            Else
                objImportar = New ListaTitulosValorizados With {.Secuencia = CStr(lngSecuencia),
                                                                .Fvalorizacion = Mid$(dtmFecha.ToString(), 1, 10),
                                                                .IdEspecie = strIDEspecie,
                                                                .Fungible = "",
                                                                .FechaVencimiento = Mid$(dtmFechaVencimiento.ToString(), 1, 10),
                                                                .ValorEspecie = CType(dblValor, Decimal?),
                                                                .Registro_Completo = strDatos,
                                                                .IsinAnna = strIsinANNA}

            End If






            _objTitulosValorizados.Add(objImportar)

            If Trim(strFechaVencimiento) = "" Then
                strSecuencia = ExisteRegistro("select lngIdSecuencia from tbltitulosvalorizados where convert(varchar(10),dtmfechavalorizacion,111) = '" & Format(dtmFecha, "yyyy/MM/dd") & "'" & " And ltrim(rtrim(" & strNombreCampoValidar & ")) = '" & strIDEspecieOisinAnna & "'")
            Else
                strSecuencia = ExisteRegistro("select lngIdSecuencia from tbltitulosvalorizados where convert(varchar(10),dtmfechavalorizacion,111) = '" & Format(dtmFecha, "yyyy/MM/dd") & "'" & " And ltrim(rtrim(" & strNombreCampoValidar & ")) = '" & strIDEspecieOisinAnna & "'" & " And convert(varchar(10),dtmFechaVencimiento,111) = '" & Format(dtmFechaVencimiento, "yyyy/MM/dd") & "'")
            End If

            If Trim(strSecuencia) <> "" Then

                Dim strMsg1 As String = String.Empty

                If String.IsNullOrEmpty(strFechaVencimiento.Trim()) Then
                    strMsg1 = String.Concat(" y fecha de vencimiento ", Format(dtmFechaVencimiento, "yyyy/MM/dd").ToString())
                Else
                    strMsg1 = ""
                End If

                'MsgBox("El registro formado por Secuencia " & strSecuencia & ", Especie " & strIDEspecie & ", Fecha valoración " & Format(dtmFecha, "yyyy/MM/dd") & strMsg1 & "se encuentra repetido." & vbCrLf & "Debe corregirlo para subir el archivo.", CType(Global.Microsoft.VisualBasic.MsgBoxStyle.SystemModal + MsgBoxStyle.OkOnly, MsgBoxStyle), "Error el importar el archivo")
                Throw New Exception("El registro formado por Secuencia " & strSecuencia & ", Especie " & strIDEspecie & ", Fecha valoración " & Format(dtmFecha, "yyyy/MM/dd") & strMsg1 & "se encuentra repetido." & vbCrLf & "Debe corregirlo para subir el archivo.")
                Return False
                'Exit Function

            Else

                With objImportar
                    Dim ret = Me.DataContext.uspOyDNet_Importaciones_ValorizarTitulos_Ingresar(CType(lngSecuencia, Integer?), dtmFecha, CType(dblValor, Decimal?), strIDEspecie, pstrUsuario, strDatos, dtmFechaVencimiento, strIsinANNA, DemeInfoSesion(pstrUsuario, "LeerLineaArchivoValoracionTitulo"), ClsConstantes.GINT_ErrorPersonalizado)
                End With

                RegBuenos = RegBuenos + 1

            End If
            Return bolRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoValoricacionTitulos")
            Return False
        End Try

    End Function

    Private Function ExisteRegistro(ByVal pstrSQL As String) As String
        Dim resultado As Integer = Me.DataContext.ExecuteQuery(Of TblTitulosValorizado)(pstrSQL).Count
        Dim retorno As String = String.Empty
        If resultado > 0 Then
            retorno = resultado.ToString()
        End If
        Return retorno
    End Function

#End Region
#Region "ImportarDatosDeceval"

    'Constantes para indicar el tipo de filtro
    Private Const ALGUNOS_REGISTROS As Integer = 1
    Private Const TODOS_LOS_REGISTROS As Integer = 2
    Private Const REGISTRO_INICIAL As Integer = 3

    'Constante para indicar el Codigo de Comicol
    Private Const Comicol As Integer = 23


    'Constantes para los mensajes de error

    Private Const MSGERROR_INGRESAR_NOMBREARCHIVO_DETALLE As String = "Debe ingresar el nombre del archivo"
    Private Const MSGERROR_ENCABEZADO As String = "   1. Archivo de Deceval : "
    Private Const MSGERROR_ENCABEZADODCV As String = "   1. Archivo de DCV : "
    Private Const MSGERROR_INICIO_IMPORTACION As String = "Inicio de la importación"
    Private Const MSGERROR_TOTAL_LEIDOS As String = "   Total de Registros Leidos  "
    Private Const MSGERROR_TOTAL_IMPORTADOS As String = "   Total de Registros Importados   "
    Private Const MSGERROR_TOTAL_REGISTROS_ISA As String = "   Total de Registros Borrados de ISA  "
    Private Const MSGERROR_IMPORTACION As String = "Importación de Deceval"
    Private Const MSGERROR_ABRIR_ARCHIVO As String = "Error tratando de abrir el archivo"
    Private Const MSGERROR_FIN_IMPORTACION As String = "Fin de la Importación"

    'Constantes para los mensajes de error de las conversiones
    Private Const MSGERROR_CONVERSION_FUNGIBLE As String = "Error de conversión del 'Fungible'"
    Private Const MSGERROR_CONVERSION_TIPODERECHO As String = "Error de conversión del 'Tipo de Derecho'"
    Private Const MSGERROR_CONVERSION_CUENTAINVERSIONISTA As String = "Error de conversión de la 'Cuenta del Inversionista'"
    Private Const MSGERROR_CONVERSION_SALDOCONTABLE As String = "Error de conversión del  'Saldo Contable'"
    Private Const MSGERROR_CONVERSION_CobroDividendos As String = "Error de conversión del  'Cobro Dividendos'"
    Private Const MSGERROR_CONVERSION_CobroDividendosAcciones As String = "Error de conversión del  'Cobro Dividendos Acciones'"
    Private Const MSGERROR_CONVERSION_CobroCapital As String = "Error de conversión del  'Cobro Capital'"
    Private Const MSGERROR_CONVERSION_CobroRendimiento As String = "Error de conversión del  'Cobro Rendimiento'"
    Private Const MSGERROR_CONVERSION_Reinversiones As String = "Error de conversión del  'Cobro Reinversiones'"
    Private Const MSGERROR_CONVERSION_RecaudoCapital As String = "Error de conversión del  'Recaudo Capital'"
    Private Const MSGERROR_CONVERSION_RecaudoDividendosAcciones As String = "Error de conversión del  'Recaudo Dividendos Acciones'"
    Private Const MSGERROR_CONVERSION_RecaudoRendimientos As String = "Error de conversión del  'Recaudo Rendimientos'"
    Private Const MSGERROR_CONVERSION_RetencionFuente As String = "Error de conversión del  'Retencion Fuente'"
    Private Const MSGERROR_CONVERSION_Enajenacion As String = "Error de conversión del  'Enajenacion'"
    Private Const MSGERROR_CONVERSION_TieneAdmonValores As String = "Error de conversión de  'Tiene o No Admon en Deceval'"
    Private Const MSGERROR_CONVERSION_ISINNOEXISTE As String = "Error : El ISIN NO existe para ninguna especie"
    Private Const MSGERROR_CONVERSION_CUENTANOEXISTE As String = "Error : La cuenta de Deceval no Existe para ningun Cliente "
    Private Const MSGERROR_CONVERSION_CLIENTEINACTIVO As String = "Error : El cliente está inactivo "
    Private RegDeISA As Long = 0

    Private Const MSGERROR_CONVERSION_PagoCUD As String = "Error de conversión del dato  'Pago CUD'"
    Private Const MSGERROR_CONVERSION_PagoPDI As String = "Error de conversión del dato  'Pago PDI'"
    Private Const MSGERROR_CONVERSION_CobroCheque As String = "Error de conversión del dato  'Cobro Cheque'"
    Private Const MSGERROR_CONVERSION_CobroConsignacion As String = "Error de conversión del dato  'Cobro Consignación'"
    Private Const MSGERROR_CONVERSION_Gravamen As String = "Error de conversión del dato  'Gravamen'"
    Private Const MSGERROR_CONVERSION_NroCuentaBancaria As String = "Error de conversión del dato  'Número Cuenta Bancaria'"


    Private Const MSGERROR_CONVERSION_OtrosImpuestos As String = "Error de conversión del dato  'Otros Impuestos'"
    Private Const MSGERROR_CONVERSION_MontoProrrogaAutomatica As String = "Error de conversión del dato  'Monto Prorroga Automatica'"
    Private Const MSGERROR_CONVERSION_MontoProrrogaConvenida As String = "Error de conversión del dato  'Monto Prorroga Convenida'"
    Private Const MSGERROR_CONVERSION_RendimientoAdicional As String = "Error de conversión del dato  'Rendimiento Adicional'"
    Private Const MSGERROR_CONVERSION_ComplementoReinversion As String = "Error de conversión del dato  'Complemento Reinversion'"
    Private Const MSGERROR_CONVERSION_TotalCobroPesos As String = "Error de conversión del dato  'Total Cobro Pesos'"
    Private Const MSGERROR_CONVERSION_TotalPagoPesos As String = "Error de conversión del dato  'Total Pago Pesos'"


    '****************************************************************************************************************************
    'Modificado por Víctor M. Arango M. - 20130722
    'Caso de soporte 6695
    'Se adicionaron dos nuevos campos a la estructura:Impuesto Ica e Impuesto Cree
    Private Const MSGERROR_CONVERSION_ImpuestoICA As String = "Error de conversión del dato  'Impuesto Ica'"
    Private Const MSGERROR_CONVERSION_ImpuestoCREE As String = "Error de conversión del dato  'Impuesto Cree'"
    Private Const MSGERROR_CONVERSION_ImpuestoAdicional As String = "Error de conversión del dato  'Impuesto Adicional'"
    '****************************************************************************************************************************


    Public Function CargarArchivoDeceval(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LineaComentario)

        Dim objReader As System.IO.StreamReader = Nothing

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strdatos As String = String.Empty
            Dim intCual As Integer = 0
            If Not _lstLineaComentario Is Nothing Then _lstLineaComentario.Clear()
            glngIDComisionista = CLng(CampoTabla("1", "lngIDComisionista", "tblInstalacion", "1"))

            Call LogImportacionLocal(MSGERROR_INICIO_IMPORTACION)
            Call LogImportacionLocal(MSGERROR_ENCABEZADO)
            Call LogImportacionLocal("      " & pstrNombreCompletoArchivo & "  ")

            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal

            Dim intEncabezado1 = FreeFile()
            'objReader = New System.IO.StreamReader(pstrNombreCompletoArchivo)
            objReader = New System.IO.StreamReader(directorioUsuario & "\" & pstrNombreCompletoArchivo)

            intCual = 1
            RegBuenos = 0
            RegDeISA = 0
            _objResultados.Clear()
            'Recorremos el archivo de encabezados
            Do While objReader.Peek() <> -1
                strdatos = objReader.ReadLine()
                If Not ExtraerDatosDeceval(intCual, strdatos, pstrUsuario, pstrInfoConexion) Then
                    Exit Do
                End If
                intCual = intCual + 1
            Loop

            objReader.Close()

            Call LogImportacionLocal(MSGERROR_TOTAL_LEIDOS & CStr(intCual - 1))
            Call LogImportacionLocal(MSGERROR_TOTAL_IMPORTADOS & CStr(RegBuenos))

            Call LogImportacionLocal("Grabación exitosa (" & CStr(RegBuenos) & " registros)")


            If glngIDComisionista = Comicol Then
                Call LogImportacionLocal(MSGERROR_TOTAL_REGISTROS_ISA & CStr(RegDeISA))
            End If

            Return _lstLineaComentario

        Catch ex As Exception
            If Not objReader Is Nothing Then objReader.Close()
            ManejarError(ex, Me.ToString(), "CargarArchivoDeceval")
            Return Nothing
        Finally
            objReader.Dispose()
        End Try
    End Function
    Public Function CargarArchivoDCV(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LineaComentario)

        Dim objReader As System.IO.StreamReader = Nothing

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim strdatos As String = String.Empty
            Dim intCual As Integer = 0
            If Not _lstLineaComentario Is Nothing Then _lstLineaComentario.Clear()

            glngIDComisionista = CLng(CampoTabla("1", "lngIDComisionista", "tblInstalacion", "1"))

            Call LogImportacionLocal(MSGERROR_INICIO_IMPORTACION)
            Call LogImportacionLocal(MSGERROR_ENCABEZADODCV)
            Call LogImportacionLocal("      " & pstrNombreCompletoArchivo & "  ")

            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal

            Dim intEncabezado1 = FreeFile()
            ' objReader = New System.IO.StreamReader(pstrNombreCompletoArchivo)
            objReader = New System.IO.StreamReader(directorioUsuario & "\" & pstrNombreCompletoArchivo)

            intCual = 1
            RegBuenos = 0
            RegDeISA = 0
            _objResultados.Clear()
            'Recorremos el archivo de encabezados
            Do While objReader.Peek() <> -1
                strdatos = objReader.ReadLine()
                If Not ExtraerDatosDCV(intCual, strdatos, pstrUsuario, pstrInfoConexion) Then
                    Exit Do
                End If
                intCual = intCual + 1
            Loop

            objReader.Close()

            Call LogImportacionLocal(MSGERROR_TOTAL_LEIDOS & CStr(intCual - 1))
            Call LogImportacionLocal(MSGERROR_TOTAL_IMPORTADOS & CStr(RegBuenos))

            Call LogImportacionLocal("Grabación exitosa (" & CStr(RegBuenos) & " registros)")


            If glngIDComisionista = Comicol Then
                Call LogImportacionLocal(MSGERROR_TOTAL_REGISTROS_ISA & CStr(RegDeISA))
            End If

            Return _lstLineaComentario

        Catch ex As Exception
            If Not objReader Is Nothing Then objReader.Close()
            ManejarError(ex, Me.ToString(), "CargarArchivoDCV")
            Return Nothing
        Finally
            objReader.Dispose()
        End Try
    End Function


    ''' <summary>
    ''' A partir de una hilera de caracteres separa en variables los datos del detalle de una Liquidación de acciones
    ''' </summary>
    ''' <param name="pintCual">Número de la Linea a ser evaluada en el archivo texto.</param>
    ''' <param name="pstrDato">Hilera de caracteres donde vienen datos de una liquidación.</param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' El proceso se debe realizar con previo conocimiento de las posiciones donde empiezan y donde terminan el valor de cada variable.
    ''' La linea del cambio se identifica asi: JCS - Marzo 15/2013 Modificacion
    ''' </remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto. JCS
    ''' Descripción      : Se modifica la sobrecarga al metodo CampoTabla(strISIN, " Top 1 strIDEspecie ", "tblEspeciesISIN", "strISIN") en la verificacion de que el ISIN exista
    '''                    en OyD, para que no envie el " Top 1 strIDEspecie " quemado al procedimiento almacenado llamado uspOyDNet_Utilidades_CampoTabla pues este tuvo que ser 
    '''                    modificado para otro requisito y al enviarle el valor en mencion esta retornando un error. En lugar de ese valor se utiliza FirstOrDefault() para 
    '''                    obtener la primera fila del set de datos que retorne la ejecucion del procedimiento almacenado.
    ''' Fecha            : Marzo 15/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 15/2013 - Resultado Ok 
    ''' </history>
    '''   ''' <history>
    ''' Modificado por   : Jhon bayron torres JBT
    ''' Descripción      : Se modifica la sobrecarga al metodo CampoTabla(strISIN, " strIDEspecie ", "tblEspeciesISIN", "strISIN") en la verificacion de que el ISIN exista
    '''                    en OyD, para que no utilize la funcion FirstOrDefault.
    ''' Fecha            : Marzo 15/2013
    ''' Pruebas CB       : Jhon bayron torres JBT - Mayo 23/2013 - Resultado Ok 
    ''' </history>
    Private Function ExtraerDatosDeceval(ByVal pintCual As Integer, ByVal pstrDato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean

        Dim bolRetorno As Boolean = True
        Dim bolPermitaGrabar As Boolean = True

        Dim strCodEmisor As String,
        strISIN As String,
        lngFungible As System.Nullable(Of Integer),
        lngTipoDerecho As System.Nullable(Of Integer),
        strFechaVencimiento As String,
        lngCuentaInversionista As System.Nullable(Of Integer),
        strTipoDocumento As String,
        strNroIdSolicitante As String,
        strNombre As String,
        curSaldoContable As System.Nullable(Of Decimal),
        curCobroDividendos As System.Nullable(Of Decimal),
        curCobroDividendosAcciones As System.Nullable(Of Decimal),
        curCobroCapital As System.Nullable(Of Decimal),
        curCobroRendimiento As System.Nullable(Of Decimal),
        curReinversiones As System.Nullable(Of Decimal),
        curRecaudoCapital As System.Nullable(Of Decimal),
        curRecaudoDividendosAcciones As System.Nullable(Of Decimal),
        curRecaudoRendimientos As System.Nullable(Of Decimal),
        curRetencionFuente As System.Nullable(Of Decimal),
        curEnajenacion As System.Nullable(Of Decimal),
        strTieneAdmon As String = String.Empty,
        strNroPreimpreso As String,
        strIDEspecie As String = String.Empty,
        strUsuario As String = pstrUsuario,
        infosesion As String = DemeInfoSesion(pstrUsuario, "ExtraerDatosDeceval"),
        intErrorPersonalizado As System.Nullable(Of Byte) = ClsConstantes.GINT_ErrorPersonalizado

        Dim strCodigoCliente As String = String.Empty

        'SLB20131007 Campos del archivo deceval del generico.
        Dim curPagoCUD As Double
        Dim curPagoPDI As Double
        Dim curCobroCheque As Double
        Dim curCobroConsignacion As Double
        Dim curGravamen As Double
        Dim strBanco As String
        Dim curNroCuentaBancaria As Double

        Dim strEstadoPagoPDI As String
        Dim curOtrosImpuestos As Double
        Dim strFechaConsignacion As String
        Dim strEstadoProrroga As String
        Dim curMontoProrrogaAutomatica As Double
        Dim curMontoProrrogaConvenida As Double
        Dim curRendimientoAdicional As Double
        Dim curComplementoReinversion As Double
        Dim curTotalCobroPesos As Double
        Dim curTotalPagoPesos As Double

        '********************************************************************************************************
        'Modificado por Víctor M. Arango M. - 20130722
        'Caso de soporte 6695
        'Se adicionaron dos nuevos campos a la estructura:Impuesto Ica e Impuesto Cree
        Dim curImpuestoICA As Double
        Dim curImpuestoCREE As Double
        '********************************************************************************************************

        'Se verifican los tipos de datos proporcionados
        strCodEmisor = UCase(Trim(Mid$(pstrDato, 10, 5)))
        strISIN = UCase(Trim(Mid$(pstrDato, 15, 12)))

        'Los ISINES de ISA no deben ser subidos SOLO PARA COMICOL
        If glngIDComisionista = Comicol And
           (Trim(strISIN) = "COI15AP00019" Or Trim(strISIN) = "COI15AP00027") Then
            RegDeISA = RegDeISA + 1
            bolPermitaGrabar = False
            'Exit Function
        End If

        'Jorge Arango 2010/02/08
        'Cambio Por ANNA, en el caso que este activo el fungible será 0, sino seguira como antes
        If mLogANNA Then
            lngFungible = 0
        Else
            If Not IsNumeric(Mid$(pstrDato, 27, 10)) Then 'Fungible
                Call LogImportacionLocal(pintCual, Mid$(pstrDato, 27, 10), MSGERROR_CONVERSION_FUNGIBLE)
                bolPermitaGrabar = False
                Return True
                'Exit Function
            Else
                lngFungible = CType(CLng(Mid$(pstrDato, 27, 10)), Integer?)
            End If
        End If
        'Fin Jorge A

        If Not IsNumeric(Mid$(pstrDato, 37, 2)) Then 'Tipo Derecho
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 37, 2), MSGERROR_CONVERSION_TIPODERECHO)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            lngTipoDerecho = CType(Mid$(pstrDato, 37, 2), Integer?)
        End If

        strFechaVencimiento = UCase(Trim(Mid$(pstrDato, 39, 8)))

        If Not IsNumeric(Mid$(pstrDato, 47, 8)) Then 'Cuenta Inversionista
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 47, 8), MSGERROR_CONVERSION_CUENTAINVERSIONISTA)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            lngCuentaInversionista = CType(Mid$(pstrDato, 47, 8), Integer?)
        End If

        strTipoDocumento = UCase(Trim(Mid$(pstrDato, 55, 3)))
        strNroIdSolicitante = UCase(Trim(Mid$(pstrDato, 58, 15)))
        strNombre = UCase(Trim(Mid$(pstrDato, 73, 50)))

        If Not IsNumeric(Mid$(pstrDato, 123, 20)) Then 'Saldo Contable
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 123, 20), MSGERROR_CONVERSION_SALDOCONTABLE)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curSaldoContable = CType(CDbl(Mid$(pstrDato, 123, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 143, 20)) Then 'Cobro Dividendos
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 143, 20), MSGERROR_CONVERSION_CobroDividendos)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curCobroDividendos = CType(CDbl(Mid$(pstrDato, 143, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 163, 20)) Then 'Cobro Dividendos en Acciones
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 163, 20), MSGERROR_CONVERSION_CobroDividendosAcciones)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curCobroDividendosAcciones = CType(CDbl(Mid$(pstrDato, 163, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 183, 20)) Then 'Cobro Capital
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 183, 20), MSGERROR_CONVERSION_CobroCapital)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curCobroCapital = CType(CDbl(Mid$(pstrDato, 183, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 203, 20)) Then 'Cobro Rendimientos
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 203, 20), MSGERROR_CONVERSION_CobroRendimiento)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curCobroRendimiento = CType(CDbl(Mid$(pstrDato, 203, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 223, 20)) Then 'Reinversiones
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 223, 20), MSGERROR_CONVERSION_Reinversiones)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curReinversiones = CType(CDbl(Mid$(pstrDato, 223, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 243, 20)) Then 'Recaudo Capital
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 243, 20), MSGERROR_CONVERSION_RecaudoCapital)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curRecaudoCapital = CType(CDbl(Mid$(pstrDato, 243, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 263, 20)) Then 'Recaudo Dividendos Acciones
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 263, 20), MSGERROR_CONVERSION_RecaudoDividendosAcciones)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curRecaudoDividendosAcciones = CType(CDbl(Mid$(pstrDato, 263, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 283, 20)) Then 'Recaudo Rendimientos
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 283, 20), MSGERROR_CONVERSION_RecaudoRendimientos)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curRecaudoRendimientos = CType(CDbl(Mid$(pstrDato, 283, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 303, 20)) Then 'Retencion en la Fuente
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 303, 20), MSGERROR_CONVERSION_RetencionFuente)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curRetencionFuente = CType(CDbl(Mid$(pstrDato, 303, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 323, 20)) Then 'Enajenacion
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 323, 20), MSGERROR_CONVERSION_Enajenacion)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curEnajenacion = CType(CDbl(Mid$(pstrDato, 323, 20)) / 10000, Decimal?)
        End If

        If Not IsNumeric(Mid$(pstrDato, 343, 2)) Then 'Tiene Admon Valores
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 323, 20), MSGERROR_CONVERSION_TieneAdmonValores)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            If CLng(Mid$(pstrDato, 343, 2)) = 1 Then
                strTieneAdmon = "SI"
            Else
                strTieneAdmon = "NO"
            End If
        End If

        strNroPreimpreso = UCase(Trim(Mid$(pstrDato, 345, 10)))

        'SLB20131007 Inicio Adicion Campos archivo del Generico. 
        If Not IsNumeric(Mid$(pstrDato, 385, 20)) Then 'Pago CUD
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 385, 20), MSGERROR_CONVERSION_PagoCUD)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curPagoCUD = CType(CDbl(Mid$(pstrDato, 385, 20)) / 10000, Double)
            'curPagoCUD = CDbl(Mid$(pstrDato, 385, 20) / 10000)
        End If

        If Not IsNumeric(Mid$(pstrDato, 405, 20)) Then 'Pago PDI
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 405, 20), MSGERROR_CONVERSION_PagoPDI)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curPagoPDI = CType(CDbl(Mid$(pstrDato, 405, 20)) / 10000, Double)
            'curPagoPDI = CDbl(Mid$(pstrDato, 405, 20) / 10000)
        End If

        If Not IsNumeric(Mid$(pstrDato, 425, 20)) Then 'Cobro Cheque
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 425, 20), MSGERROR_CONVERSION_CobroCheque)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curCobroCheque = CType(CDbl(Mid$(pstrDato, 425, 20)) / 10000, Double)
            'curCobroCheque = CDbl(Mid$(pstrDato, 425, 20) / 10000)
        End If

        If Not IsNumeric(Mid$(pstrDato, 445, 20)) Then 'Cobro Consignación
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 445, 20), MSGERROR_CONVERSION_CobroConsignacion)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            'Santiago Vergara - Agosto 05/2014 - Se corrige la lectura del curCobroConsignacion que iniciaba en 455 y debe se 455
            curCobroConsignacion = CType(CDbl(Mid$(pstrDato, 445, 20)) / 10000, Double)
            'curCobroConsignacion = CDbl(Mid$(pstrDato, 445, 20) / 10000)
        End If

        If Not IsNumeric(Mid$(pstrDato, 465, 20)) Then 'Gravamen
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 465, 20), MSGERROR_CONVERSION_Gravamen)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curGravamen = CType(CDbl(Mid$(pstrDato, 465, 20)) / 10000, Double)
            'curGravamen = CDbl(Mid$(pstrDato, 465, 20) / 10000)
        End If

        strBanco = UCase(Trim(Mid$(pstrDato, 485, 15)))     'Nombre del Banco

        If Not IsNumeric(Mid$(pstrDato, 500, 20)) Then 'Número Cuenta Bancaria
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 500, 20), MSGERROR_CONVERSION_NroCuentaBancaria)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curNroCuentaBancaria = CType(CDbl(Mid$(pstrDato, 500, 20)) / 10000, Double)
            'curNroCuentaBancaria = CDbl(Mid$(pstrDato, 500, 20))
        End If

        strEstadoPagoPDI = UCase(Trim(Mid$(pstrDato, 520, 30)))     'Estado Pago PDI

        '****************************************************************************************************************************
        'Modificado por Víctor M. Arango M. - 20130722
        'Caso de soporte 6695
        'Se adicionaron dos nuevos campos a la estructura:Impuesto Ica e Impuesto Cree

        If (Len(pstrDato) = 767) Then
            If Not IsNumeric(Mid$(pstrDato, 550, 20)) Then 'Impuesto ICA
                Call LogImportacionLocal(pintCual, Mid$(pstrDato, 550, 20), MSGERROR_CONVERSION_ImpuestoICA)
                bolPermitaGrabar = False
                Return True
                'Exit Function
            Else
                curImpuestoICA = CType(CDbl(Mid$(pstrDato, 550, 20)) / 10000, Double)
                'curImpuestoICA = CDbl(Mid$(pstrDato, 550, 20) / 10000)
            End If

            If Not IsNumeric(Mid$(pstrDato, 570, 20)) Then 'Impuesto CREE
                Call LogImportacionLocal(pintCual, Mid$(pstrDato, 570, 20), MSGERROR_CONVERSION_ImpuestoCREE)
                bolPermitaGrabar = False
                Return True
                'Exit Function
            Else
                curImpuestoCREE = CType(CDbl(Mid$(pstrDato, 570, 20)) / 10000, Double)
                'curImpuestoCREE = CDbl(Mid$(pstrDato, 570, 20) / 10000)
            End If
        Else
            curImpuestoICA = 0
            curImpuestoCREE = 0
        End If
        '****************************************************************************************************************************

        If Not IsNumeric(Mid$(pstrDato, 590, 20)) Then '
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 590, 20), MSGERROR_CONVERSION_OtrosImpuestos)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curOtrosImpuestos = CType(CDbl(Mid$(pstrDato, 590, 20)) / 10000, Double)
            'curOtrosImpuestos = CDbl(Mid$(pstrDato, 590, 20) / 10000)
        End If

        strFechaConsignacion = UCase(Trim(Mid$(pstrDato, 610, 8)))

        strEstadoProrroga = UCase(Trim(Mid$(pstrDato, 618, 30)))

        If Not IsNumeric(Mid$(pstrDato, 648, 20)) Then '
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 648, 20), MSGERROR_CONVERSION_MontoProrrogaAutomatica)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curMontoProrrogaAutomatica = CType(CDbl(Mid$(pstrDato, 648, 20)) / 10000, Double)
            'curMontoProrrogaAutomatica = CDbl(Mid$(pstrDato, 648, 20) / 10000)
        End If

        If Not IsNumeric(Mid$(pstrDato, 668, 20)) Then '
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 668, 20), MSGERROR_CONVERSION_MontoProrrogaConvenida)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curMontoProrrogaConvenida = CType(CDbl(Mid$(pstrDato, 668, 20)) / 10000, Double)
            'curMontoProrrogaConvenida = CDbl(Mid$(pstrDato, 668, 20) / 10000)
        End If

        If Not IsNumeric(Mid$(pstrDato, 688, 20)) Then '
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 688, 20), MSGERROR_CONVERSION_RendimientoAdicional)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curRendimientoAdicional = CType(CDbl(Mid$(pstrDato, 688, 20)) / 10000, Double)
            'curRendimientoAdicional = CDbl(Mid$(pstrDato, 688, 20) / 10000)
        End If

        If Not IsNumeric(Mid$(pstrDato, 708, 20)) Then '
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 708, 20), MSGERROR_CONVERSION_ComplementoReinversion)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curComplementoReinversion = CType(CDbl(Mid$(pstrDato, 708, 20)) / 10000, Double)
            'curComplementoReinversion = CDbl(Mid$(pstrDato, 708, 20) / 10000)
        End If

        If Not IsNumeric(Mid$(pstrDato, 728, 20)) Then '
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 728, 20), MSGERROR_CONVERSION_TotalCobroPesos)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curTotalCobroPesos = CType(CDbl(Mid$(pstrDato, 728, 20)) / 10000, Double)
            'curTotalCobroPesos = CDbl(Mid$(pstrDato, 728, 20) / 10000)
        End If

        If Not IsNumeric(Mid$(pstrDato, 748, 20)) Then '
            Call LogImportacionLocal(pintCual, Mid$(pstrDato, 748, 20), MSGERROR_CONVERSION_TotalPagoPesos)
            bolPermitaGrabar = False
            Return True
            'Exit Function
        Else
            curTotalPagoPesos = CType(CDbl(Mid$(pstrDato, 748, 20)) / 10000, Double)
            'curTotalPagoPesos = CDbl(Mid$(pstrDato, 748, 20) / 10000)
        End If
        'SLB20131007 Fin Adicion Campos archivo del Generico. 

        ''Verifico si la Cuenta de Deceval si pertenece a algun Cliente en OyD y selecciono el Codigo
        strCodigoCliente = CampoTabla(lngCuentaInversionista.ToString(), "lngIDComitente", "tblCuentasDecevalPorAgrupador", "lngidCuentaDeceval")
        If Trim(strCodigoCliente) = "" Then
            Call LogImportacionLocal(pintCual, CStr(IIf(lngCuentaInversionista Is Nothing, "", lngCuentaInversionista)), MSGERROR_CONVERSION_CUENTANOEXISTE)
            bolPermitaGrabar = False
            Return True
            Exit Function
        Else
            If CampoTabla("BLOQUEOSCLIENTEPAGODIVIDENDOS", "strvalor", "tblparametros", "strparametro") = "SI" Then
                If CDbl(CampoTabla(strCodigoCliente, "logActivo", "tblClientes", "lngID")) = 0 Then
                    Call LogImportacionLocal(pintCual, strCodigoCliente, MSGERROR_CONVERSION_CLIENTEINACTIVO)
                    bolPermitaGrabar = False
                    Return True
                    Exit Function
                End If
            End If
        End If


        'Verifico si el ISIN existe en OyD y saco la primera especie que tenga el ISIN

        'JCS - Marzo 15/2013 Modificacion
        'strIDEspecie = CampoTabla(strISIN, " Top 1 strIDEspecie ", "tblEspeciesISIN", "strISIN")
        'strIDEspecie = CampoTabla(strISIN, " strIDEspecie ", "tblEspeciesISIN", "strISIN").FirstOrDefault
        'FIN JCS - Marzo 15/2013 Modificacion
        'JBT - Mayo 23/2013
        strIDEspecie = CampoTabla(strISIN, " strIDEspecie ", "tblEspeciesISIN", "strISIN")
        'fin JBT - Mayo 23/2013 Modificacion
        If strIDEspecie = "" Then
            Call LogImportacionLocal(pintCual, strISIN, MSGERROR_CONVERSION_ISINNOEXISTE)
            bolPermitaGrabar = False
            Return True
            Exit Function
        End If

        'Cambio agregado Miercoles 14 de Mayo de 2008 JUANGUI
        If curCobroDividendosAcciones > 0 Then
            Call LogImportacionLocal(pintCual, CStr(curCobroDividendosAcciones), "El proceso No importa Abonos realizados en Cantidad de Acciones")
            bolPermitaGrabar = False
            'Exit Function
        End If

        If bolPermitaGrabar Then

            'Grabo el Registro en la Tabla De Pagos de Deceval
            Me.DataContext.uspOyDNet_Importaciones_GrabarRegistroDeceval(strCodEmisor,
                                                                        strISIN,
                                                                        lngFungible,
                                                                        lngTipoDerecho,
                                                                        strFechaVencimiento,
                                                                        lngCuentaInversionista,
                                                                        strTipoDocumento,
                                                                        strNroIdSolicitante,
                                                                        strNombre,
                                                                        curSaldoContable,
                                                                        curCobroDividendos,
                                                                        curCobroDividendosAcciones,
                                                                        curCobroCapital,
                                                                        curCobroRendimiento,
                                                                        curReinversiones,
                                                                        curRecaudoCapital,
                                                                        curRecaudoDividendosAcciones,
                                                                        curRecaudoRendimientos,
                                                                        curRetencionFuente,
                                                                        curEnajenacion,
                                                                        strTieneAdmon,
                                                                        strNroPreimpreso,
                                                                        strIDEspecie,
                                                                        curPagoCUD,
                                                                        curPagoPDI,
                                                                        curCobroCheque,
                                                                        curCobroConsignacion,
                                                                        curGravamen,
                                                                        strBanco,
                                                                        curNroCuentaBancaria,
                                                                        strEstadoPagoPDI,
                                                                        curOtrosImpuestos,
                                                                        strFechaConsignacion,
                                                                        strEstadoProrroga,
                                                                        curMontoProrrogaAutomatica,
                                                                        curMontoProrrogaConvenida,
                                                                        curRendimientoAdicional,
                                                                        curComplementoReinversion,
            curTotalCobroPesos,
            curTotalPagoPesos,
            curImpuestoICA,
            curImpuestoCREE,
                                                                        pstrUsuario,
                                                                        infosesion,
                                                                        intErrorPersonalizado)

            RegBuenos = RegBuenos + 1
        End If

        'Exit_ExtraerDatosDeceval:
        '        Exit Function

        'Err_ExtraerDatosDeceval:
        '        LogErrores(Me.Name & ".ExtraerDatosDeceval", Err.Number, Err.Description)
        '        Resume Exit_ExtraerDatosDeceval
        Return bolRetorno
    End Function
    ''' <summary>
    ''' Metodo por el cual se lee el archivo DCV
    ''' </summary>
    ''' <param name="pintCual">Número de la Linea a ser evaluada en el archivo texto.</param>
    ''' <param name="pstrDato">Hilera de caracteres donde vienen datos de una liquidación.</param>
    ''' <param name="pstrUsuario"></param>
    Private Function ExtraerDatosDCV(ByVal pintCual As Integer, ByVal pstrDato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean

        Dim bolRetorno As Boolean = True
        Dim bytCiudadTitulo As Byte
        Dim strNombreTituloPagado As String = String.Empty
        Dim strFechaPago As String = String.Empty
        Dim lngNroContratoTercero As Integer
        Dim strNumeroExpedicion As String = String.Empty
        Dim strNroDocumento As String
        Dim strCodigoCliente As String
        Dim bytEntidadBeneficiariaPago As Byte
        Dim strBeneficiarioPago As String = String.Empty
        Dim dblTasaEfectivaAnual As Decimal
        Dim dblValorCosto As Decimal
        Dim dblValorSaldo As Decimal
        Dim dblValorCapitalizado As Decimal
        Dim dblValorCapital As Decimal
        Dim dblValorIntereses As Decimal
        Dim dblValorReinvertido As Decimal
        Dim dblValorAmortizado As Decimal
        Dim dblValorDescuento As Decimal
        Dim dblValorDiferenciaEnCambio As Decimal
        Dim dblValorReteFuenteIntereses As Decimal
        Dim dblValorReteFuenteDiferenciaEnCambio As Decimal
        Dim dblValorReteFuenteCapital As Decimal
        Dim dblValorPagoOtraMoneda As Decimal
        Dim lngNroOperacionPesos As Integer
        Dim lngNroOperacionOtraMoneda As Integer
        Dim lngNroOperacionCapitalizacion As Integer
        Dim lngNroOperacionReinversion As Integer
        Dim intCodigoIntermediario As Integer
        Dim intEntidadIntermediario As Byte
        Dim strUsuario As String = pstrUsuario
        Dim infosesion As String = DemeInfoSesion(pstrUsuario, "ExtraerDatosDCV")
        Dim intErrorPersonalizado As System.Nullable(Of Byte) = ClsConstantes.GINT_ErrorPersonalizado


        Dim CamposRegistro = pstrDato.Split(CChar("!"))
        'bytCiudadTitulo
        If Not IsNumeric(CamposRegistro(0)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(0) = "", "Vacío", CamposRegistro(0))), "La Ciudad del título no es un número")
            Return True
            Exit Function
        Else
            bytCiudadTitulo = CByte(CamposRegistro(0))
        End If

        'strNombreTituloPagado
        If CamposRegistro(1) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Nombre del título pagado> se encuentra vacío")
        Else
            If CamposRegistro(1).Contains("""") Then 'Pregunta si la cadena tiene, por lo menos, una comilla doble
                strNombreTituloPagado = Mid(CamposRegistro(1), 2, Len(CamposRegistro(1)) - 2)
            End If
        End If


        'Fecha Pago
        If CamposRegistro(2) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Fecha de Pago> se encuentra vacío")
        Else
            strFechaPago = CStr(CamposRegistro(2))
        End If


        'lngNroContratoTercero
        If Not IsNumeric(CamposRegistro(3)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(3) = "", "Vacío", CamposRegistro(3))), "El número de contrato del tercero, no es numérico")
            Return True
            Exit Function
        Else
            lngNroContratoTercero = CInt(CamposRegistro(3))
        End If

        'strNumeroExpedicion
        If CamposRegistro(4) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Nro. de expedición> se encuentra vacío")
        Else
            If CamposRegistro(4).Contains("""") Then 'Pregunta si la cadena tiene, por lo menos, una comilla doble
                strNumeroExpedicion = Mid(CamposRegistro(4), 2, Len(CamposRegistro(4)) - 2)
            End If
        End If


        'strNroDocumento
        If CamposRegistro(5) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Nro. de Documento (del Cliente)> se encuentra vacío")
        Else
            strNroDocumento = CamposRegistro(5)
        End If
        ''''''''''''''''''''''''''''''''''''''''''''
        'busca el documento asi sea nit con numero de verificacion
        Dim e = Me.DataContext.uspCodigoClientePorDocumento_OyDNet(strCodigoCliente, strNroDocumento)
        ''''''''''''''''''''''''''''''''''''''''''''
        'strCodigoCliente = CampoTabla(strNroDocumento, "lngID", "tblClientes", "strNroDocumento")

        If Trim(strCodigoCliente) = "" Then
            Call LogImportacionLocal(pintCual, strNroDocumento, "No existe el cliente con el número de documento especificado")
            Return True
            Exit Function
        Else
            If CampoTabla("BLOQUEOSCLIENTEPAGODIVIDENDOS", "strvalor", "tblparametros", "strparametro") = "SI" Then
                If CDbl(CampoTabla(strCodigoCliente, "logActivo", "tblClientes", "lngID")) = 0 Or CampoTabla(strCodigoCliente, "strEstadoCliente", "tblClientes", "lngId") = "B" Then 'Jairo Gonzalez 2007/05/10
                    Call LogImportacionLocal(pintCual, strNroDocumento, MSGERROR_CONVERSION_CLIENTEINACTIVO)
                    Return True
                    Exit Function
                End If
            End If

        End If

        'bytEntidadBeneficiariaPago
        If Not IsNumeric(CamposRegistro(6)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(6) = "", "Vacío", CamposRegistro(6))), "La entidad beneficiaria del pago, no es numérica")
            Return True
            Exit Function
        Else
            bytEntidadBeneficiariaPago = CByte(CamposRegistro(6))
        End If

        'strBeneficiarioPago
        If CamposRegistro(7) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Beneficiario Pago> se encuentra vacío")
        Else
            If CamposRegistro(7).Contains("""") Then 'Pregunta si la cadena tiene, por lo menos, una comilla doble
                strBeneficiarioPago = Mid(CamposRegistro(7), 2, Len(CamposRegistro(7)) - 2)
            End If
        End If


        'dblTasaEfectivaAnual
        If Not IsNumeric(CamposRegistro(8)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(8) = "", "Vacío", CamposRegistro(8))), "La tasa efectiva anual, no es numérica")
            Return True
            Exit Function
        Else
            dblTasaEfectivaAnual = CDec(CamposRegistro(8))
        End If

        'dblValorCosto
        If Not IsNumeric(CamposRegistro(9)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(9) = "", "Vacío", CamposRegistro(9))), "El valor del costo no es numérico")
            Return True
            Exit Function
        Else
            dblValorCosto = CDec(CamposRegistro(9))
        End If

        'dblValorSaldo
        If Not IsNumeric(CamposRegistro(10)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(10) = "", "Vacío", CamposRegistro(10))), "El valor del Saldo no es numérico")
            Return True
            Exit Function
        Else
            dblValorSaldo = CDec(CamposRegistro(10))
        End If

        'dblValorCapitalizado
        If Not IsNumeric(CamposRegistro(11)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(11) = "", "Vacío", CamposRegistro(11))), "El valor capitalizado no es numérico")
            Return True
            Exit Function
        Else
            dblValorCapitalizado = CDec(CamposRegistro(11))
        End If

        'dblValorCapital
        If Not IsNumeric(CamposRegistro(12)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(12) = "", "Vacío", CamposRegistro(12))), "El valor del capital no es numérico")
            Return True
            Exit Function
        Else
            dblValorCapital = CDec(CamposRegistro(12))
        End If

        'dblValorIntereses
        If Not IsNumeric(CamposRegistro(13)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(13) = "", "Vacío", CamposRegistro(13))), "El valor de los intereses no es numérico")
            Return True
            Exit Function
        Else
            dblValorIntereses = CDec(CamposRegistro(13))
        End If

        'dblValorReinvertido
        If Not IsNumeric(CamposRegistro(14)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(14) = "", "Vacío", CamposRegistro(14))), "El valor reinvertido no es numérico")
            Return True
            Exit Function
        Else
            dblValorReinvertido = CDec(CamposRegistro(14))
        End If

        'dblValorAmortizado
        If Not IsNumeric(CamposRegistro(15)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(15) = "", "Vacío", CamposRegistro(15))), "El valor amortizado no es numérico")
            Return True
            Exit Function
        Else
            dblValorAmortizado = CDec(CamposRegistro(15))
        End If

        'dblValorDescuento
        If Not IsNumeric(CamposRegistro(16)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(16) = "", "Vacío", CamposRegistro(16))), "El valor de descuento no es numérico")
            Return True
            Exit Function
        Else
            dblValorDescuento = CDec(CamposRegistro(16))
        End If

        'dblValorDiferenciaEnCambio
        If Not IsNumeric(CamposRegistro(17)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(17) = "", "Vacío", CamposRegistro(17))), "El valor de la diferencia en cambio no es numérico")
            Return True
            Exit Function
        Else
            dblValorDiferenciaEnCambio = CDec(CamposRegistro(17))
        End If


        'dblValorReteFuenteIntereses
        If Not IsNumeric(CamposRegistro(18)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(18) = "", "Vacío", CamposRegistro(18))), "El valor de la Retefuente no es numérico")
            Return True
            Exit Function
        Else
            dblValorReteFuenteIntereses = CDec(CamposRegistro(18))
        End If

        'dblValorReteFuenteDiferenciaEnCambio
        If Not IsNumeric(CamposRegistro(19)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(19) = "", "Vacío", CamposRegistro(19))), "El valor <Valor ReteFuente Diferencia en Cambio> no es numérico")
            Return True
            Exit Function
        Else
            dblValorReteFuenteDiferenciaEnCambio = CDec(CamposRegistro(19))
        End If

        'dblValorReteFuenteCapital
        If Not IsNumeric(CamposRegistro(20)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(20) = "", "Vacío", CamposRegistro(20))), "El valor <Valor ReteFuente Capital> no es numérico")
            Return True
            Exit Function
        Else
            dblValorReteFuenteCapital = CDec(CamposRegistro(20))
        End If


        'Valor Pago Otra moneda
        If Not IsNumeric(CamposRegistro(21)) Then
            Call LogImportacionLocal(pintCual, CStr(IIf(CamposRegistro(21) = "", "Vacío", CamposRegistro(21))), "El valor <Valor Pago Otra Moneda> no es numérico")
            Return True
            Exit Function
        Else
            dblValorPagoOtraMoneda = CDec(CamposRegistro(21))
        End If

        'lngNroOperacionPesos
        If CamposRegistro(22) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Nro. operación Pesos> se encuentra vacío")
        Else
            If CamposRegistro(22).Contains("""") Then 'Pregunta si la cadena tiene, por lo menos, una comilla doble
                lngNroOperacionPesos = CInt(Mid(CamposRegistro(22), 2, Len(CamposRegistro(22)) - 2))
            End If
        End If

        'Nro. Operación Otra moneda
        If CamposRegistro(23) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Nro. operación Otra moneda> se encuentra vacío")
        Else
            If CamposRegistro(23).Contains("""") Then 'Pregunta si la cadena tiene, por lo menos, una comilla doble
                lngNroOperacionOtraMoneda = CInt(Mid(CamposRegistro(23), 2, Len(CamposRegistro(23)) - 2))
            End If
        End If

        'lngNroOperacionCapitalizacion
        If CamposRegistro(24) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Nro. operación Capitalización> se encuentra vacío")
        Else
            If CamposRegistro(24).Contains("""") Then 'Pregunta si la cadena tiene, por lo menos, una comilla doble
                lngNroOperacionCapitalizacion = CInt(Mid(CamposRegistro(24), 2, Len(CamposRegistro(24)) - 2))
            End If
        End If


        'Nro. Operación Reinversión.
        If CamposRegistro(25) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Nro. operación Reinversión> se encuentra vacío")
        Else
            If CamposRegistro(25).Contains("""") Then 'Pregunta si la cadena tiene, por lo menos, una comilla doble
                lngNroOperacionReinversion = CInt(Mid(CamposRegistro(25), 2, Len(CamposRegistro(25)) - 2))
            End If
        End If


        'intCodigoIntermediario
        If CamposRegistro(26) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Código Intermediario> se encuentra vacío")
        Else
            If CamposRegistro(26).Contains("""") Then 'Pregunta si la cadena tiene, por lo menos, una comilla doble
                intCodigoIntermediario = CInt(Mid(CamposRegistro(26), 2, Len(CamposRegistro(26)) - 2))
            End If
        End If


        'intEntidadIntermediario
        If CamposRegistro(27) = "" Then
            Call LogImportacionLocal(pintCual, "", "El campo <Entidad Intermediario> se encuentra vacío")
        Else
            If CamposRegistro(27).Contains("""") Then 'Pregunta si la cadena tiene, por lo menos, una comilla doble
                intEntidadIntermediario = CByte(Mid(CamposRegistro(27), 2, Len(CamposRegistro(27)) - 2))
                intEntidadIntermediario = CByte(CamposRegistro(27))
            End If
        End If


        'Grabo el Registro en la Tabla De Pagos de Deceval
        Me.DataContext.usp_tblPagosDCV_Insertar_OyDNet(bytCiudadTitulo,
                                                                    strNombreTituloPagado,
                                                                    strFechaPago,
                                                                    lngNroContratoTercero,
                                                                    strNumeroExpedicion,
                                                                    strNroDocumento,
                                                                    strCodigoCliente,
                                                                    bytEntidadBeneficiariaPago,
                                                                    strBeneficiarioPago,
                                                                    dblTasaEfectivaAnual,
                                                                    dblValorCosto,
                                                                    dblValorSaldo,
                                                                    dblValorCapitalizado,
                                                                    dblValorCapital,
                                                                    dblValorIntereses,
                                                                    dblValorReinvertido,
                                                                    dblValorAmortizado,
                                                                    dblValorDescuento,
                                                                    dblValorDiferenciaEnCambio,
                                                                    dblValorReteFuenteIntereses,
                                                                    dblValorReteFuenteDiferenciaEnCambio,
                                                                    dblValorReteFuenteCapital,
                                                                    dblValorPagoOtraMoneda,
                                                                    lngNroOperacionPesos,
                                                                    lngNroOperacionOtraMoneda,
                                                                    lngNroOperacionCapitalizacion, lngNroOperacionReinversion, CType(intCodigoIntermediario, Short?), intEntidadIntermediario, pstrUsuario, infosesion, intErrorPersonalizado)

        RegBuenos = RegBuenos + 1
        'Exit_ExtraerDatosDeceval:
        '        Exit Function

        'Err_ExtraerDatosDeceval:
        '        LogErrores(Me.Name & ".ExtraerDatosDeceval", Err.Number, Err.Description)
        '        Resume Exit_ExtraerDatosDeceval
        Return bolRetorno
    End Function
    Public Function NroDeRegistrosDeceval(ByVal pbolBorrar As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LineaComentario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim strRespuesta As String = String.Empty
            Dim resultado As Integer = 0

            If pbolBorrar Then
                resultado = CInt(Me.DataContext.uspOyDNet_Importaciones_DatosDeceval_Borrar(pstrUsuario, DemeInfoSesion(pstrUsuario, "NroDeRegistrosDeceval"), ClsConstantes.GINT_ErrorPersonalizado))
                strRespuesta = "BORRADO"
            Else
                resultado = CInt(Me.DataContext.ufnOyDNet_Importaciones_NroRegistrosDeceval())
                If resultado > 0 Then
                    strRespuesta = "Existen datos Pendientes para hacer Recibos de Caja, " & vbCrLf & "Desea borrarlos y cargar este nuevo archivo ? "
                Else
                    strRespuesta = "OK"
                End If
            End If

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = strRespuesta})

            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoValoricacionTitulos")
            Return Nothing
        End Try
    End Function
    Public Function NroDeRegistrosDCV(ByVal pbolBorrar As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LineaComentario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim strRespuesta As String = String.Empty
            Dim resultado As Integer = 0

            If pbolBorrar Then
                resultado = CInt(Me.DataContext.uspOyDNet_Importaciones_DatosDCV_Borrar(pstrUsuario, DemeInfoSesion(pstrUsuario, "NroDeRegistrosDCV"), ClsConstantes.GINT_ErrorPersonalizado))
                strRespuesta = "BORRADO"
            Else
                resultado = CInt(Me.DataContext.ufnOyDNet_Importaciones_NroRegistrosDCV())
                If resultado > 0 Then
                    strRespuesta = "Existen datos Pendientes para hacer Recibos de Caja, " & vbCrLf & "Desea borrarlos y cargar este nuevo archivo ? "
                Else
                    strRespuesta = "OK"
                End If
            End If

            Dim lstLineaComentario As New List(Of OyDImportaciones.LineaComentario)
            lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = strRespuesta})

            Return lstLineaComentario

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoValoricacionTitulos")
            Return Nothing
        End Try
    End Function

    Sub LogImportacionLocal(ByVal pintLinea As Integer, ByVal pstrDato As String, ByVal pstrMensaje As String)
        LogImportacionLocal(" Línea " & pintLinea & ": " & vbTab & pstrMensaje & vbTab & " (Dato: " & pstrDato & ")")
    End Sub

    Private Sub LogImportacionLocal(ByVal pstrTexto As String)
        _lstLineaComentario.Add(New OyDImportaciones.LineaComentario With {.FechaHora = Now, .Texto = pstrTexto})
    End Sub
    Public Function ConsultaListaPagosDCV(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TblpagosDCV)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.usp_tblPagosDCV_Consultar_OyDNet().ToList()
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultaListaPagosDCV")
            Return Nothing
        End Try
    End Function
    <Update()>
    Public Sub UpdateConsultapagosdcv(ByVal currentpagosdcv As TblpagosDCV)
        Try
            Throw New NotImplementedException("No se puede modificar")
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConsultapagosdcv")
        End Try
    End Sub
#End Region

#Region "Propiedades"
    Private _Listarecibodecaja As List(Of Reporterecibodecaja)
    Public Property Listarecibodecaja(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Reporterecibodecaja)
        Get
            Return _Listarecibodecaja
        End Get
        Set(ByVal value As List(Of Reporterecibodecaja))
            _Listarecibodecaja = value
        End Set
    End Property
#End Region
#Region "Traer_Recibosdecaja"
    'DomainServices
    'Public Function Traer_Recibosdecaja(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS System.Nullable(Of Boolean)


    '    Try
    '        Dim ret = Me.DataContext.SpConsultarPagosDecevalOyDNet() '.ToList

    '        RETORNO = False
    '        Usuario = pstrUsuario
    '        Listarecibodecaja = ret.ToList
    '        Dim grid As New DataGrid
    '        grid.AutoGenerateColumns = False
    '        CrearColumnasrecibo(grid)
    '        Dim strMensaje As String = "csv"
    '        exportDataGrid(grid, strMensaje.ToUpper())
    '        If RETORNO = True Then
    '            Return True
    '        Else
    '            Return Nothing
    '        End If


    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "Traer_Reporterecibosdecaja")
    '        Return Nothing
    '    End Try
    'End Function

    'Private Sub CrearColumnasrecibo(ByVal pgrid As DataGrid)


    '    '<--
    '    Dim Especie As New BoundColumn
    '    Especie.HeaderText = "Especie"
    '    pgrid.Columns.Add(Especie)

    '    Dim VlrAPagar As New BoundColumn
    '    VlrAPagar.HeaderText = "Valor A Pagar"
    '    pgrid.Columns.Add(VlrAPagar)

    '    Dim Cliente As New BoundColumn
    '    Cliente.HeaderText = "Cliente"
    '    pgrid.Columns.Add(Cliente)

    '    Dim DocumentoDCVAL As New BoundColumn
    '    DocumentoDCVAL.HeaderText = "Documento DECEVAL"
    '    pgrid.Columns.Add(DocumentoDCVAL)

    '    Dim Sucursal As New BoundColumn
    '    Sucursal.HeaderText = "Sucursal"
    '    pgrid.Columns.Add(Sucursal)

    '    Dim Receptor As New BoundColumn
    '    Receptor.HeaderText = "Receptor"
    '    pgrid.Columns.Add(Receptor)

    '    Dim lngIDComitente As New BoundColumn
    '    lngIDComitente.HeaderText = "IDComitente"
    '    pgrid.Columns.Add(lngIDComitente)

    '    Dim Detalle As New BoundColumn
    '    Detalle.HeaderText = "Detalle"
    '    pgrid.Columns.Add(Detalle)

    '    Dim Emisor As New BoundColumn
    '    Emisor.HeaderText = "Emisor"
    '    pgrid.Columns.Add(Emisor)

    '    Dim strISIN As New BoundColumn
    '    strISIN.HeaderText = "ISIN"
    '    pgrid.Columns.Add(strISIN)

    '    Dim lngFungible As New BoundColumn
    '    lngFungible.HeaderText = "Fungible"
    '    pgrid.Columns.Add(lngFungible)

    '    Dim lngCuentaInversionista As New BoundColumn
    '    lngCuentaInversionista.HeaderText = "Cuenta Inversionista"
    '    pgrid.Columns.Add(lngCuentaInversionista)

    '    Dim strIDEspecie As New BoundColumn
    '    strIDEspecie.HeaderText = "IDEspecie"
    '    pgrid.Columns.Add(strIDEspecie)

    '    Dim lngIdEmisor As New BoundColumn
    '    lngIdEmisor.HeaderText = "IdEmisor"
    '    pgrid.Columns.Add(lngIdEmisor)

    '    Dim Tipo As New BoundColumn
    '    Tipo.HeaderText = "Tipo"
    '    pgrid.Columns.Add(Tipo)

    '    Dim CodSucursal As New BoundColumn
    '    CodSucursal.HeaderText = "Codigo Sucursal"
    '    pgrid.Columns.Add(CodSucursal)

    '    Dim strIDReceptor As New BoundColumn
    '    strIDReceptor.HeaderText = "IDReceptor"
    '    pgrid.Columns.Add(strIDReceptor)

    '    Dim DocumentoOyD As New BoundColumn
    '    DocumentoOyD.HeaderText = "Documento OyD"
    '    pgrid.Columns.Add(DocumentoOyD)

    '    Dim CurSaldoContable As New BoundColumn
    '    CurSaldoContable.HeaderText = "Saldo Contable"
    '    pgrid.Columns.Add(CurSaldoContable)

    '    Dim curRecaudoRendimientos As New BoundColumn
    '    curRecaudoRendimientos.HeaderText = "Recaudo Rendimientos"
    '    pgrid.Columns.Add(curRecaudoRendimientos)

    '    Dim curRecaudoCapital As New BoundColumn
    '    curRecaudoCapital.HeaderText = "Recaudo Capital"
    '    pgrid.Columns.Add(curRecaudoCapital)

    '    Dim curRetencionFuente As New BoundColumn
    '    curRetencionFuente.HeaderText = "Retencion Fuente"
    '    pgrid.Columns.Add(curRetencionFuente)
    '    '-->


    'End Sub


    'Private Sub exportDataGrid(ByVal dGrid As DataGrid, ByVal strFormat As String)

    '    Dim strBuilder As New StringBuilder()
    '    Dim strLineas As New List(Of String)

    '    If IsNothing(dGrid) Then Return
    '    Dim lstFields As List(Of String) = New List(Of String)()
    '    'If dGrid.HeaderStyle= Then  'es para saber si es la columna de el header y q le de el formato
    '    For Each dgcol As DataGridColumn In dGrid.Columns
    '        lstFields.Add(formatField(dgcol.HeaderText.ToString(), strFormat, True))
    '    Next
    '    buildStringOfRow(strBuilder, lstFields, strFormat)
    '    strLineas.Add(strBuilder.ToString())
    '    'End If
    '    dGrid.DataSource = Listarecibodecaja

    '    For a = 0 To Listarecibodecaja.Count - 1
    '        lstFields.Clear()
    '        strBuilder.Clear()
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).Especie), String.Empty, Listarecibodecaja(a).Especie)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).VlrAPagar), String.Empty, Listarecibodecaja(a).VlrAPagar)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).Cliente), String.Empty, Listarecibodecaja(a).Cliente)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).DocumentoDCVAL), String.Empty, Listarecibodecaja(a).DocumentoDCVAL)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).Sucursal), String.Empty, Listarecibodecaja(a).Sucursal)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).Receptor), String.Empty, Listarecibodecaja(a).Receptor)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).lngIDComitente), String.Empty, Listarecibodecaja(a).lngIDComitente)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).Detalle), String.Empty, Listarecibodecaja(a).Detalle)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).Emisor), String.Empty, Listarecibodecaja(a).Emisor)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).strISIN), String.Empty, Listarecibodecaja(a).strISIN)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).lngFungible), String.Empty, Listarecibodecaja(a).lngFungible)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).lngCuentaInversionista), String.Empty, Listarecibodecaja(a).lngCuentaInversionista)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).strIDEspecie), String.Empty, Listarecibodecaja(a).strIDEspecie)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).lngIdEmisor), String.Empty, Listarecibodecaja(a).lngIdEmisor)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).Tipo), String.Empty, Listarecibodecaja(a).Tipo)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).CodSucursal), String.Empty, Listarecibodecaja(a).CodSucursal)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).strIDReceptor), String.Empty, Listarecibodecaja(a).strIDReceptor)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).DocumentoOyD), String.Empty, Listarecibodecaja(a).DocumentoOyD)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).curSaldoContable), String.Empty, Listarecibodecaja(a).curSaldoContable)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).curRecaudoRendimientos), String.Empty, Listarecibodecaja(a).curRecaudoRendimientos)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).curRecaudoCapital), String.Empty, Listarecibodecaja(a).curRecaudoCapital)), strFormat, False))
    '        lstFields.Add(formatField(CStr(IIf(IsNothing(Listarecibodecaja(a).curRetencionFuente), String.Empty, Listarecibodecaja(a).curRetencionFuente)), strFormat, False))

    '        buildStringOfRow(strBuilder, lstFields, strFormat)
    '        strLineas.Add(strBuilder.ToString())
    '    Next

    '    RETORNO = Guardar_ArchivoServidor(CSTR_NOMBREPROCESO_RECIBOSDECAJA, Usuario, String.Format("ReporteRecibosDeCaja{0}.csv", Now.ToString("yyyy-mm-dd")), strLineas)

    'End Sub

    'Private Shared Function formatField(ByVal data As String, ByVal format As String, ByVal encabezado As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS String
    '    Select Case format
    '        Case "XML"
    '            Return String.Format("<Cell><Data ss:Type=""String" & """>{0}</Data></Cell>", data)
    '        Case "CSV"
    '            If encabezado = True Then
    '                Return String.Format("""   {0}   """, data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
    '            Else
    '                Return String.Format("""{0}""", data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
    '            End If
    '    End Select
    '    Return data

    'End Function

    'Private Shared Sub buildStringOfRow(ByVal strBuilder As StringBuilder, ByVal lstFields As List(Of String), ByVal strFormat As String)
    '    Select Case strFormat
    '        Case "XML"
    '            strBuilder.AppendLine("<Row>")
    '            strBuilder.AppendLine(String.Join("" & vbCrLf & "", lstFields.ToArray()))
    '            strBuilder.AppendLine("</Row>")
    '            ' break;
    '        Case "CSV"
    '            strBuilder.AppendLine(String.Join(SEPARATOR_FORMAT_CVS, lstFields.ToArray()))
    '            ' break;

    '    End Select

    'End Sub

    Public Function Guardar_ArchivoServidor(ByVal NombreProceso As String, ByVal pstrusuario As String, ByVal NombreArchivo As String, ByVal Lista As List(Of String), ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrusuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim importaciones As New ImportacionesDomainService
            importaciones.GuardarArchivo(NombreProceso, pstrusuario, NombreArchivo, Lista, False, pstrInfoConexion)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Guardar_ArchivoServidor")
            Return False
        End Try
    End Function

    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Este método recibe un parametro string el cual contiene el html con toda la estructura de datos para visualizarlo en formato .doc
    ''' Fecha            : Junio 12/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 12/2013 - Resultado Ok 
    ''' </history>
    Public Function Guardar_ArchivoServidorWord(ByVal NombreProceso As String, ByVal pstrusuario As String, ByVal NombreArchivo As String, ByVal Lista As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrusuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim importaciones As New ImportacionesDomainService
            importaciones.GuardarArchivoWord(NombreProceso, pstrusuario, NombreArchivo, Lista, False, pstrInfoConexion)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Guardar_ArchivoServidor")
            Return False
        End Try
    End Function

#End Region
#Region "Inhabilitados"
    Public Sub InsertItemComboClientesInhabilitados(ByVal objItemCombo As ClientesInhabilitados)
    End Sub

    Public Sub UpdateItemComboClientesInhabilitados(ByVal objItemCombo As ClientesInhabilitados)
    End Sub

    Public Sub DeleteItemComboClientesInhabilitados(ByVal objItemCombo As ClientesInhabilitados)
    End Sub

    Public Function ObtenerURLClientesInhabilitados(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strURLClientes As String
            Try
                strURLClientes = My.Settings.URLXMLClientesInhabilitados
            Catch ex As Exception
                strURLClientes = "http://www.treasury.gov/ofac/downloads/sdn.xml"
            End Try

            Return strURLClientes
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerURLClientesInhabilitados")
            Return Nothing
        End Try

    End Function


    Public Function ClientesInhablitados(ByVal RutaArchivo As String,
                                         ByVal pstrNombreProceso As String,
                                         ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesInhabilitados)
        Dim logFalloLecturaURLBolsa As Boolean = False

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim objRutasInsertados As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones("ClientesInhabilitadosInsertados", pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            logFalloLecturaURLBolsa = True

            Dim objListaRespuesta = New clsProcesosArchivo().ValidarClientesInhabilitados(RutaArchivo).ToList

            logFalloLecturaURLBolsa = False

            If Not IsNothing(objListaRespuesta) Then
                Dim objListaRespuesta2 = (From I In objListaRespuesta Where I.NumeroDocumento.Length <= 25).Distinct.ToList
                objListaRespuesta = objListaRespuesta2

                Dim objDataTable = New DataTable()
                objDataTable.Columns.Add("Tipo Identificacion", GetType(String))
                objDataTable.Columns.Add("Numero Documento", GetType(String))
                objDataTable.Columns.Add("Nombre Completo", GetType(String))
                If Not IsNothing(objListaRespuesta) Then
                    For Each objItem In objListaRespuesta
                        objDataTable.Rows.Add(objItem.TipoIdentificacion, objItem.NumeroDocumento, objItem.NombreCompleto)
                    Next
                End If

                Dim objDataTableCarga = New DataTable()
                objDataTableCarga.Columns.Add("Nombre Completo", GetType(String))
                objDataTableCarga.Columns.Add("Numero Documento", GetType(String))
                objDataTableCarga.Columns.Add("Tipo Identificacion", GetType(String))
                If Not IsNothing(objListaRespuesta) Then
                    For Each objItem In objListaRespuesta
                        If String.IsNullOrEmpty(objItem.TipoIdentificacion) Then
                            objItem.TipoIdentificacion = "0"
                        End If

                        objDataTableCarga.Rows.Add(objItem.NombreCompleto, objItem.NumeroDocumento, objItem.TipoIdentificacion)
                    Next
                End If

                Dim objDataTableInsertados = New DataTable()
                objDataTableInsertados.Columns.Add("Nombre existe OyD", GetType(String))
                objDataTableInsertados.Columns.Add("Numero Documento existe OyD", GetType(String))
                objDataTableInsertados.Columns.Add("Tipo Documento existe OyD", GetType(String))
                objDataTableInsertados.Columns.Add("Nombre inserto OyD", GetType(String))
                objDataTableInsertados.Columns.Add("Numero Documento inserto OyD", GetType(String))
                objDataTableInsertados.Columns.Add("Tipo Documento inserto OyD", GetType(String))

                Dim listTblInhabilitados = (From I In Me.DataContext.tblInhabilitados Select I.lngnrodocumento).ToList

                If Not IsNothing(objListaRespuesta) Then
                    For Each objItem In objListaRespuesta
                        If listTblInhabilitados.Contains(objItem.NumeroDocumento.ToString) Then
                            objDataTableInsertados.Rows.Add(objItem.NombreCompleto, objItem.NumeroDocumento, objItem.TipoIdentificacion, "", "", "")
                        Else
                            objDataTableInsertados.Rows.Add("", "", "", objItem.NombreCompleto, objItem.NumeroDocumento, objItem.TipoIdentificacion)
                        End If

                    Next
                End If


                Dim strArchivo = GenerarExcel(objDataTable, objRutas.RutaArchivosLocal, "ClientesInhabilitados_" & Now.ToString("yyyyMMddhhmmss"))
                Dim strArchivoInsertados = GenerarExcel(objDataTableInsertados, objRutasInsertados.RutaArchivosLocal, "ClientesInhabilitadosInsertados_" & Now.ToString("yyyyMMddhhmmss"))
                Dim strArchivoPlanoCarga = GenerarTextoPlano(objDataTableCarga, objRutasInsertados.RutaArchivosLocal, "ClientesInhabilitadosCarga_" & Now.ToString("yyyyMMddhhmmss"), "txt", ";")

                If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                    objRutas.RutaWeb = objRutas.RutaWeb & "/"
                End If

                If objRutasInsertados.RutaWeb.Substring(objRutasInsertados.RutaWeb.Length - 2, 1) <> "/" Then
                    objRutasInsertados.RutaWeb = objRutasInsertados.RutaWeb & "/"
                End If

                If Not IsNothing(objListaRespuesta) Then
                    If objListaRespuesta.Count > 0 Then
                        objListaRespuesta.First().RutaArchivo = objRutas.RutaCompartidaOWeb() & strArchivo
                        objListaRespuesta.First().RutaArchivoInsertado = objRutasInsertados.RutaCompartidaOWeb() & strArchivoInsertados
                        objListaRespuesta.First().RutaArchivoCarga = objRutasInsertados.RutaArchivoProceso & "\" & strArchivoPlanoCarga
                    End If
                End If


                Return objListaRespuesta
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If logFalloLecturaURLBolsa Then
                ManejarError(ex, Me.ToString(), "ClientesInhablitados", False)
                Throw New Exception("ERRORLECTURAURL:|Ocurrió un error al intentar acceder a la URL (" & RutaArchivo & "). Por favor informar al administrador para que sea revisado el log de errores del servicio y sea solucionado el problema.")
                Return Nothing
            Else
                ManejarError(ex, Me.ToString(), "ClientesInhablitados")
                Return Nothing
            End If
        End Try
    End Function

    Public Function CargarArchivoImportacionInhabilitados(ByVal pstrNombreCompletoArchivo As String,
                                             ByVal pstrNombreProceso As String,
                                             ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesInhabilitados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim objDatosRutasInsertados = clsClaseUploadsUnificado.FnRutasImportaciones("ClientesInhabilitadosInsertados", pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Dim directorioUsuarioInsertados = objDatosRutasInsertados.RutaArchivosLocal


            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}
            Dim objListaRespuesta As List(Of ClientesInhabilitados)
            objListaRespuesta = objImportar.RecorrerArchivoExcelInhabilitados(directorioUsuario & "\", pstrNombreCompletoArchivo)

            Dim objListaRespuesta2 = (From I In objListaRespuesta Where Not String.IsNullOrEmpty(I.NumeroDocumento) And I.NumeroDocumento.Length <= 40).Distinct.ToList

            objListaRespuesta = objListaRespuesta2

            Dim objDataTable = New DataTable()
            objDataTable.Columns.Add("Tipo Identificacion", GetType(String))
            objDataTable.Columns.Add("Numero Documento", GetType(String))
            objDataTable.Columns.Add("Nombre Completo", GetType(String))
            If Not IsNothing(objListaRespuesta) Then
                For Each objItem In objListaRespuesta
                    If String.IsNullOrEmpty(objItem.TipoIdentificacion) Then
                        objItem.TipoIdentificacion = "0"
                    End If
                    objDataTable.Rows.Add(objItem.TipoIdentificacion, objItem.NumeroDocumento, objItem.NombreCompleto)
                Next
            End If

            Dim objDataTableCarga = New DataTable()
            objDataTableCarga.Columns.Add("Nombre Completo", GetType(String))
            objDataTableCarga.Columns.Add("Numero Documento", GetType(String))
            objDataTableCarga.Columns.Add("Tipo Identificacion", GetType(String))
            If Not IsNothing(objListaRespuesta) Then
                For Each objItem In objListaRespuesta
                    objDataTableCarga.Rows.Add(objItem.NombreCompleto, objItem.NumeroDocumento, objItem.TipoIdentificacion)
                Next
            End If

            Dim objDataTableInsertados = New DataTable()
            objDataTableInsertados.Columns.Add("Nombre existe OyD", GetType(String))
            objDataTableInsertados.Columns.Add("Numero Documento existe OyD", GetType(String))
            objDataTableInsertados.Columns.Add("Tipo Documento existe OyD", GetType(String))
            objDataTableInsertados.Columns.Add("Nombre inserto OyD", GetType(String))
            objDataTableInsertados.Columns.Add("Numero Documento inserto OyD", GetType(String))
            objDataTableInsertados.Columns.Add("Tipo Documento inserto OyD", GetType(String))
            Dim listTblInhabilitados = (From I In Me.DataContext.tblInhabilitados Select I.lngnrodocumento).ToList

            If Not IsNothing(objListaRespuesta) Then
                For Each objItem In objListaRespuesta
                    If listTblInhabilitados.Contains(objItem.NumeroDocumento.ToString) Then
                        'If (From i In listTblInhabilitados Where i.lngnrodocumento = objItem.NumeroDocumento).Count > 0 Then
                        objDataTableInsertados.Rows.Add(objItem.NombreCompleto, objItem.NumeroDocumento, objItem.TipoIdentificacion, "", "", "")
                    Else
                        objDataTableInsertados.Rows.Add("", "", "", objItem.NombreCompleto, objItem.NumeroDocumento, objItem.TipoIdentificacion)
                    End If

                Next
            End If

            Dim strArchivo = GenerarExcel(objDataTable, objDatosRutas.RutaArchivosLocal, "ClientesInhabilitados_" & Now.ToString("yyyyMMddhhmmss"))
            Dim strArchivoInsertados = GenerarExcel(objDataTableInsertados, objDatosRutasInsertados.RutaArchivosLocal, "ClientesInhabilitadosInsertados_" & Now.ToString("yyyyMMddhhmmss"))
            Dim strArchivoPlanoCarga = GenerarTextoPlano(objDataTableCarga, objDatosRutasInsertados.RutaArchivosLocal, "ClientesInhabilitadosCarga_" & Now.ToString("yyyyMMddhhmmss"), "txt", ";")


            If objDatosRutas.RutaWeb.Substring(objDatosRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objDatosRutas.RutaWeb = objDatosRutas.RutaWeb & "/"
            End If

            If objDatosRutasInsertados.RutaWeb.Substring(objDatosRutasInsertados.RutaWeb.Length - 2, 1) <> "/" Then
                objDatosRutasInsertados.RutaWeb = objDatosRutasInsertados.RutaWeb & "/"
            End If

            If Not IsNothing(objListaRespuesta) Then
                If objListaRespuesta.Count > 0 Then
                    objListaRespuesta.First().RutaArchivo = objDatosRutas.RutaCompartidaOWeb() & strArchivo
                    objListaRespuesta.First().RutaArchivoInsertado = objDatosRutasInsertados.RutaCompartidaOWeb() & strArchivoInsertados
                    objListaRespuesta.First().RutaArchivoCarga = objDatosRutasInsertados.RutaArchivoProceso & "\" & strArchivoPlanoCarga
                End If
            End If

            Return objListaRespuesta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionInhabilitados")
            Return Nothing
        End Try
    End Function


    Public Function ConsultarRutaFormatos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strRutaFormatos = (From e In Me.DataContext.tblParametros Where e.strParametro = "RUTA_FORMATO_CARGA_ARCHIVOS" Select e.strValor).FirstOrDefault.ToString

            Return strRutaFormatos
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarRutaFormatos")
            Return Nothing
        End Try

    End Function
#End Region
#Region "OyD Plus"

    Public Function CargarArchivoImportacionChequeOyDPlus(ByVal pstrNombreCompletoArchivo As String,
                                              ByVal pblnEstructuraActual As Boolean, ByVal pdtmDesde As DateTime, ByVal pdtmHasta As DateTime, ByVal pstrNombreProceso As String,
                                              ByVal pstrUsuario As String, ByVal FirmaExtrangera As Boolean, ByVal pstrInfoConexion As String) As List(Of clsChequesOyDPlus)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyDVersiones\Desarrollo\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}
            Dim ret As List(Of clsChequesOyDPlus)
            ret = objImportar.RecorrerArchivoExcelCheque(directorioUsuario & "\", pstrNombreCompletoArchivo)


            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionChequeOyDPlus")
            Return Nothing
        End Try
    End Function

    Public Function CargarArchivoImportacionTransferenciaOyDPlus(ByVal pstrNombreCompletoArchivo As String,
                                              ByVal pblnEstructuraActual As Boolean, ByVal pdtmDesde As DateTime, ByVal pdtmHasta As DateTime, ByVal pstrNombreProceso As String,
                                              ByVal pstrUsuario As String, ByVal FirmaExtrangera As Boolean, ByVal pstrInfoConexion As String) As List(Of clsTransferenciaOyDPlus)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)

            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyDVersiones\Desarrollo\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}
            Dim ret As List(Of clsTransferenciaOyDPlus)
            ret = objImportar.RecorrerArchivoExcelTransferencia(directorioUsuario & "\", pstrNombreCompletoArchivo)

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionTransferenciaOyDPlus")
            Return Nothing
        End Try
    End Function

    Public Function CargarArchivoImportacionRecibosOyDPlus(ByVal pstrNombreCompletoArchivo As String, pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String, Optional ByVal strIDComitente As String = "", Optional ByVal strTipoProducto As String = "") As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyDVersiones\Desarrollo\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim objResultado As Dictionary(Of String, Object)

            objResultado = objImportar.RecorrerArchivoExcelRecibos(directorioUsuario & "\", pstrNombreCompletoArchivo)

            Dim objListaRespuesta As New List(Of RespuestaArchivoImportacion)

            If Not IsNothing(objResultado) Then
                Dim objStringPagos As String = String.Empty
                Dim objStringCheques As String = String.Empty
                Dim objStringTransferencia As String = String.Empty
                Dim objStringConsignacion As String = String.Empty
                Dim logPasaValidacion As Boolean = True
                Dim objListaMensajes As New List(Of String)

                If objResultado.ContainsKey("STRINGPAGOS") Then
                    objStringPagos = CStr(objResultado("STRINGPAGOS"))
                End If

                If objResultado.ContainsKey("STRINGCHEQUES") Then
                    objStringCheques = CStr(objResultado("STRINGCHEQUES"))
                End If

                If objResultado.ContainsKey("STRINGTRANSFERENCIA") Then
                    objStringTransferencia = CStr(objResultado("STRINGTRANSFERENCIA"))
                End If

                If objResultado.ContainsKey("STRINGCONSIGNACION") Then
                    objStringConsignacion = CStr(objResultado("STRINGCONSIGNACION"))
                End If

                Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesRecibo_ValidarArchivo(objStringPagos, objStringCheques, objStringTransferencia, objStringConsignacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "CargarArchivoImportacionRecibosOyDPlus"), 0, strIDComitente, strTipoProducto)
                objListaRespuesta = ret.ToList
            Else
                objListaRespuesta.Add(New RespuestaArchivoImportacion With {.Columna = 0,
                                                                            .Exitoso = False,
                                                                            .Fila = 0,
                                                                            .ID = 1,
                                                                            .Mensaje = "El archivo no coincide con una estructura valida o no contiene registros.",
                                                                            .NroProceso = 0,
                                                                            .Tipo = "Estructura"})
            End If

            Return objListaRespuesta

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionRecibosOyDPlus")
            Return Nothing
        End Try
    End Function

    Public Function CargarArchivoImportacionFondos(ByVal pstrNombreCompletoArchivo As String, pstrNombreProceso As String, ByVal strTipoImportacion As String, ByVal plogBorrarRegistros As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyDVersiones\Desarrollo\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim objResultado As String

            If strTipoImportacion = "F" Then
                objResultado = objImportar.RecorrerArchivoExcel(directorioUsuario & "\", pstrNombreCompletoArchivo, clsProcesosArchivo.NroColumnasFondos)
            Else
                objResultado = objImportar.RecorrerArchivoExcel(directorioUsuario & "\", pstrNombreCompletoArchivo, clsProcesosArchivo.NroColumnasFondosClientes)
            End If

            Dim objListaRespuesta As New List(Of RespuestaArchivoImportacion)

            If Not String.IsNullOrEmpty(objResultado) Then
                Dim ret = Me.DataContext.uspOyDNet_InsertarFondosClientes(objResultado, strTipoImportacion, plogBorrarRegistros, pstrUsuario, DemeInfoSesion(pstrUsuario, "CargarArchivoImportacionFondos"), 0)
                objListaRespuesta = ret.ToList
            Else
                objListaRespuesta.Add(New RespuestaArchivoImportacion With {.Columna = 0,
                                                                            .Exitoso = False,
                                                                            .Fila = 0,
                                                                            .ID = 1,
                                                                            .Mensaje = "El archivo no coincide con una estructura valida o no contiene registros.",
                                                                            .NroProceso = 0,
                                                                            .Tipo = "Estructura"})
            End If

            Return objListaRespuesta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionFondos")
            Return Nothing
        End Try
    End Function

    Public Function CargarArchivoImportacionOrdenesMasivas(ByVal pstrNombreCompletoArchivo As String, pstrNombreProceso As String,
                                                           ByVal pstrTipoNegocio As String, ByVal pstrTipoOperacion As String, ByVal pstrUsuario As String,
                                                           ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, plogComplementacionPrecioPromedio As Boolean, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OYDVersiones\Desarrollo\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim objResultado As clsResultadoCargaMasiva

            objResultado = objImportar.RecorrerArchivoExcelColumnasDinamicas(directorioUsuario & "\", pstrNombreCompletoArchivo, pstrUsuario, pstrUsuarioWindows, pstrMaquina)

            Dim objListaRespuesta As New List(Of RespuestaArchivoImportacion)

            If Not IsNothing(objResultado) Then
                Dim ret = Me.DataContext.uspOyDNet_CargaMasivaOrdenes_LeerArchivo(pstrTipoNegocio, pstrTipoOperacion, objResultado.Campos, objResultado.Valores, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "CargarArchivoImportacionOrdenesMasivas"), 0, plogComplementacionPrecioPromedio)
                objListaRespuesta = ret.ToList
            Else
                objListaRespuesta.Add(New RespuestaArchivoImportacion With {.Columna = 0,
                                                                            .Exitoso = False,
                                                                            .Fila = 0,
                                                                            .ID = 1,
                                                                            .Mensaje = "El archivo no coincide con una estructura valida o no contiene registros.",
                                                                            .NroProceso = 0,
                                                                            .Tipo = "Estructura"})
            End If

            Return objListaRespuesta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionOrdenesMasivas")
            Return Nothing
        End Try
    End Function

    Public Function CargarArchivoImportacionLibranzas(ByVal pstrNombreCompletoArchivo As String, pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OYDVersiones\Desarrollo\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim objResultado As clsResultadoCargaMasiva

            objResultado = objImportar.RecorrerArchivoExcelColumnasDinamicas(directorioUsuario & "\", pstrNombreCompletoArchivo, pstrUsuario, pstrMaquina)

            Dim objListaRespuesta As New List(Of RespuestaArchivoImportacion)

            If Not IsNothing(objResultado) Then
                Dim ret = Me.DataContext.uspLibranzas_ImportacionLeer(objResultado.Campos, objResultado.Valores, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "CargarArchivoImportacionLibranzas"), 0)
                objListaRespuesta = ret.ToList
            Else
                objListaRespuesta.Add(New RespuestaArchivoImportacion With {.Columna = 0,
                                                                            .Exitoso = False,
                                                                            .Fila = 0,
                                                                            .ID = 1,
                                                                            .Mensaje = "El archivo no coincide con una estructura valida o no contiene registros.",
                                                                            .NroProceso = 0,
                                                                            .Tipo = "Estructura"})
            End If

            Return objListaRespuesta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionLibranzas")
            Return Nothing
        End Try
    End Function

    Public Function ObtenerValoresArchivoImportacion(ByVal pdblNroProceso As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of InformacionArchivoRecibos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaOrdenesRecibo_ConsultarArchivo(pdblNroProceso, pstrUsuario, DemeInfoSesion(pstrUsuario, "ObtenerValoresArchivoImportacion"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerValoresArchivoImportacion")
            Return Nothing
        End Try
    End Function

    Public Sub DeleteclsChequesOyDPlus(ByVal objclsChequeOyDPlus As clsChequesOyDPlus)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objclsChequeOyDPlus.pstrUsuarioConexion, objclsChequeOyDPlus.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Importaciones_ImportacionLiq_Eliminar( pID,  pTipo,  pClaseOrden,  pIDOrden,  pIDBolsa,  pLiquidacion,  pCumplimiento, DemeInfoSesion(pstrUsuario, "DeleteImportacionLi"),0).ToList

            Me.DataContext.clsCheques.Attach(objclsChequeOyDPlus)
            Me.DataContext.clsCheques.DeleteOnSubmit(objclsChequeOyDPlus)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteclsChequesOyDPlus")
        End Try
    End Sub

    Public Sub UpdateclsChequesOyDPlus(ByVal objclsChequeOyD As clsChequesOyDPlus)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objclsChequeOyD.pstrUsuarioConexion, objclsChequeOyD.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.clsCheques.Attach(objclsChequeOyD, Me.ChangeSet.GetOriginal(objclsChequeOyD))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateclsChequesOyDPlus")
        End Try

    End Sub

    Public Sub DeleteclsTransferenciaOyDPlus(ByVal objclsTransferenciaOyDPlus As clsTransferenciaOyDPlus)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objclsTransferenciaOyDPlus.pstrUsuarioConexion, objclsTransferenciaOyDPlus.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.clsTransferencia.Attach(objclsTransferenciaOyDPlus)
            Me.DataContext.clsTransferencia.DeleteOnSubmit(objclsTransferenciaOyDPlus)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteclsTransferenciaOyDPlus")
        End Try
    End Sub

    Public Sub UpdateclsTransferenciaOyDPlus(ByVal objclsTransferenciaOyD As clsTransferenciaOyDPlus)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objclsTransferenciaOyD.pstrUsuarioConexion, objclsTransferenciaOyD.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.clsTransferencia.Attach(objclsTransferenciaOyD, Me.ChangeSet.GetOriginal(objclsTransferenciaOyD))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateclsTransferenciaOyDPlus")
        End Try

    End Sub

#End Region

#Region "Importacion CF.ConfiguracionArbitraje"
    Public Function ConfiguracionArbitraje_Importar(ByVal pstrNombreProceso As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivoProceso

            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionArbitraje_Importar(directorioUsuario & "\" & pstrNombreCompletoArchivo, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "ConfiguracionArbitraje_Importar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionArbitraje_Importar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Importar Ordenes LEO"
    Public Function CargarArchivoImportacionLEO(ByVal pstrNombreCompletoArchivo As String, pstrNombreProceso As String, ByVal pstrUsuario As String, pstrRutaFisicaGeneracion As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyD.Net\V1.1.1\VS2010\A2.OyD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso
            'directorioUsuario = "\\A2WEBDLLO\SitiosWebDllo\OyD\OyD.Net\OYDServiciosRIA\Uploads\edgar_munoz\OrdenesLEO"

            Dim objStringordenes As StringBuilder = Nothing

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            objStringordenes = objImportar.RecorrerArchivoExcelLEO(directorioUsuario & "\", pstrNombreCompletoArchivo)
            Dim xmlLista As System.Xml.Linq.XElement

            Try
                xmlLista = System.Xml.Linq.XElement.Parse(objStringordenes.ToString())
                objStringordenes.Clear()
            Catch ex As Exception
                Dim resp As RespuestaArchivoImportacion = New RespuestaArchivoImportacion() With {.Columna = 0,
                    .Fila = 0,
                    .ID = 0,
                    .Tipo = String.Empty,
                    .Mensaje = "Estructura no válida para el archivo, el archivo de tener la siguiente estructura:" + vbNewLine +
                    "Clase | Código del cliente | Tipo | Usuario | Cantidad | Deposito | Cuenta del depósito | tipo clasificacion | objeto clasificacion | Especie | Femision | Fvencimiento | Modalidad | TasaFacial | Comisión"}
                Dim lsresp As New List(Of RespuestaArchivoImportacion)
                lsresp.Add(resp)
                Return lsresp
            End Try

            'Dim objListaRespuesta As New List(Of RespuestaArchivoImportacion)

            Dim strRutaFisicaCargaArchivo As String = pstrRutaFisicaGeneracion & "\" & pstrUsuario.Replace(".", "_") & objDatosRutas.NombreProceso & "\" & pstrNombreCompletoArchivo
            'Dim strRutaFisicaCargaArchivo As String = "\\A2WEBDLLO\SitiosWebDllo\OyD\OyD.Net\OYDServiciosRIA\Uploads\edgar_munoz\OrdenesLEO\ordenesRV4.csv"

            Dim ret = Me.DataContext.usp_validarlistasOrdenes_Consultar(strRutaFisicaCargaArchivo, pstrUsuario).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionLEO")
            Return Nothing
        End Try
    End Function

    Public Function ObtenerValoresArchivoLEO(ByVal pdblNroProceso As System.Nullable(Of Double), ByVal pstrUsuario As String, pintUltimoRegistro As Integer, ByVal pstrInfoConexion As String) As List(Of ArchivoOrdenesLeo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarlistasOrdenes_Archivo(pdblNroProceso, pstrUsuario, pintUltimoRegistro)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerValoresArchivoImportacion")
            Return Nothing
        End Try
    End Function

    Public Sub UpdateArchivoOrdenesLeo(ByVal currentArchivoOrdenesLeo As ArchivoOrdenesLeo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentArchivoOrdenesLeo.pstrUsuarioConexion, currentArchivoOrdenesLeo.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.ArchivoOrdenesLeos.Attach(currentArchivoOrdenesLeo, Me.ChangeSet.GetOriginal(currentArchivoOrdenesLeo))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateSucursale")
        End Try
    End Sub

    Public Function GenerarOrdenesLeo(ByVal pdblNroProceso As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ordenesLEO_lanzarJob(pstrUsuario, pdblNroProceso)
            Return ret.ToString
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarOrdenesLeo")
            Return Nothing
        End Try
    End Function

    Public Function VerificarGeneracionOrdenesLEO(ByVal pdblNroProceso As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objListaRespuesta As New List(Of RespuestaArchivoImportacion)
            Dim ret = Me.DataContext.usp_ordenesLEO_verificar_archivo(pstrUsuario, pdblNroProceso)
            objListaRespuesta = ret.ToList
            Return objListaRespuesta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerValoresArchivoImportacion")
            Return Nothing
        End Try
    End Function

    Public Function cargarObjetosClasificacion(pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ObjetoClasificacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ordenesLEO_ListaObjetoClasificacion(pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "cargarObejtosClasificacion")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Amortizaciones ISIN"
    ''' <summary>
    ''' Metodo para cargar amortizaciones en base a archivo .csv seleccionado -- EOMC -- 08/08/2013
    ''' </summary>
    ''' <param name="pintIdISINFungible"></param>
    ''' <param name="pdtmFEmision"></param>
    ''' <param name="pdtmFVencimiento"></param>
    ''' <param name="pstrNombreCompletoArchivo"></param>
    ''' <param name="pstrNombreProceso"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CargarAmortizacionesISIN(ByVal pintIdISINFungible As Integer, pdtmFEmision As DateTime, pdtmFVencimiento As DateTime, ByVal pxmlAmortizaciones As String, ByVal pstrNombreCompletoArchivo As String, pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Dim objStringordenes As StringBuilder = Nothing

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim ret = Me.DataContext.uspOyDNet_Maestros_EspeciesISIN_Importar_Amortizaciones(pintIdISINFungible, pdtmFEmision, pdtmFVencimiento, pstrUsuario, pxmlAmortizaciones, pstrUsuario.Replace(".", "_") & objDatosRutas.NombreProceso & "\" & pstrNombreCompletoArchivo, DemeInfoSesion(pstrUsuario, "ImportacionLiqFiltrar"), 0).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarAmortizacionesISIN")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Importaciones genéricas"
    ''' <summary>
    ''' Metodo consulta de tabla tblCargasArchivos que contiene las definiciones para importación de archivos -- EOMC -- 08/08/2013
    ''' </summary>
    ''' <param name="pstrSistema"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function archivosCargaGenericaConsultar(pstrSistema As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CargasArchivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim resp = Me.DataContext.usptblCargasArchivos_Consultar(pstrSistema, pstrUsuario, DemeInfoSesion(pstrUsuario, "archivosCargaGenericaConsultar"), 0).ToList()
            Return resp
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "archivosCargaGenericaConsultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Metodo de proceso que identifica las diferentes definiciones para carga de archivo y ejecuta el correspondiente según parámetros -- EOMC -- 08/08/2013
    ''' </summary>
    ''' <param name="pstrModulo"></param>
    ''' <param name="pstrNombreCompletoArchivo"></param>
    ''' <param name="pstrArchivoFormato"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="pintLineasARemover"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CargarArchivoImportaciones(pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, pstrArchivoFormato As String, ByVal pstrUsuario As String, pintLineasARemover As Integer, paccion As String, plngidconcepto As Integer, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrModulo, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Dim objStringordenes As StringBuilder = Nothing

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            If pintLineasARemover > 0 Then
                objImportar.EliminarFilas(directorioUsuario, pstrNombreCompletoArchivo, pintLineasARemover)
            End If

            Dim ret = Me.DataContext.uspOyDNet_CargasArchivos_Ejecutar_procedimiento(pstrModulo, pstrUsuario.Replace(".", "_") & objDatosRutas.NombreProceso & "\" & pstrNombreCompletoArchivo, pstrArchivoFormato, pstrUsuario, paccion, plngidconcepto, DemeInfoSesion(pstrUsuario, "ImportacionLiqFiltrar"), 0).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportaciones")
            Return Nothing
        End Try
    End Function

    Public Function CargaMasivaDetalleTesoreria_SubirArchivo(pstrNombreProceso As String, ByVal pstrTipodocumento As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivoProceso

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}



            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaDetalleTesoreria_SubirArchivo(pstrTipodocumento, pstrUsuario, pstrUsuarioWindows, directorioUsuario & "\" & pstrNombreCompletoArchivo, pstrMaquina, DemeInfoSesion(pstrUsuario, "CargaMasivaDetalleTesoreria_SubirArchivo"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargaMasivaDetalleTesoreria_SubirArchivo")
            Return Nothing
        End Try
    End Function

    Public Function CargaCECarteraColectivas_SubirArchivo(pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivoProceso

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim ret = Me.DataContext.uspOyDNet_CargaCECarteraColectivas_SubirArchivo(pstrUsuario, pstrUsuarioWindows, directorioUsuario & "\" & pstrNombreCompletoArchivo, pstrMaquina, DemeInfoSesion(pstrUsuario, "CargaMasivaDetalleTesoreria_SubirArchivo"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargaCECarteraColectivas_SubirArchivo")
            Return Nothing
        End Try
    End Function

    Public Function RecaudosXReferencia_Importar(ByVal pstrNombreProceso As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivoProceso

            Dim ret = Me.DataContext.uspOyDNet_RecaudosXReferencia_Importar(directorioUsuario & "\" & pstrNombreCompletoArchivo, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "RecaudosXReferencia_Importar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RecaudosXReferencia_Importar")
            Return Nothing
        End Try
    End Function


    'Public Function CargarArchivoImportacionesParametros(pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, pstrArchivoFormato As String, ByVal pstrUsuario As String, pintLineasARemover As Integer, parametros As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of RespuestaArchivoImportacion)

    '    Try
    '        Dim objDatosRutas = clsClaseUploads.FnRutasImportaciones(pstrModulo, pstrUsuario)
    '        Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
    '        Dim objStringordenes As StringBuilder = Nothing

    '        Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

    '        If pintLineasARemover > 0 Then
    '            objImportar.EliminarFilas(directorioUsuario, pstrNombreCompletoArchivo, pintLineasARemover)
    '        End If

    '        Dim ret = Me.DataContext.uspOyDNet_CargasArchivos_Ejecutar_procedimiento(pstrModulo, pstrUsuario.Replace(".", "_") & objDatosRutas.NombreProceso & "\" & pstrNombreCompletoArchivo, pstrArchivoFormato, pstrUsuario, DemeInfoSesion(pstrUsuario, "ImportacionLiqFiltrar"), 0).ToList

    '        Return ret

    '        'Dim dt As New DataTable()
    '        'Dim line As String = Nothing
    '        'Dim i As Integer = 0

    '        'Using sr As StreamReader = File.OpenText("c:\temp\table1.csv")
    '        '    line = sr.ReadLine()
    '        '    Do While line IsNot Nothing
    '        '        Dim data( ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS String = line.Split(","c)
    '        '        If data.Length > 0 Then
    '        '            If i = 0 Then
    '        '                For Each item In data
    '        '                    dt.Columns.Add(New DataColumn())
    '        '                Next item
    '        '                i += 1
    '        '            End If
    '        '            Dim row As DataRow = dt.NewRow()
    '        '            row.ItemArray = data
    '        '            dt.Rows.Add(row)
    '        '        End If
    '        '        line = sr.ReadLine()
    '        '    Loop
    '        'End Using

    '        'Using cn As New SqlConnection(ConfigurationManager.ConnectionStrings()
    '        '    cn.Open()
    '        '    Using copy As New SqlBulkCopy(cn)
    '        '        copy.ColumnMappings.Add(0, 0)
    '        '        copy.ColumnMappings.Add(1, 1)
    '        '        copy.ColumnMappings.Add(2, 2)
    '        '        copy.ColumnMappings.Add(3, 3)
    '        '        copy.ColumnMappings.Add(4, 4)
    '        '        copy.DestinationTableName = "Censis"
    '        '        copy.WriteToServer(dt)
    '        '    End Using
    '        'End Using

    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "CargarArchivoImportaciones")
    '        Return Nothing
    '    End Try
    'End Function
#End Region

#Region "Importación flujo de ordenes"

    Public Function FlujoOrdenes_CargarArchivoRespuesta(ByVal pstrNombreCompletoArchivo As String, pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            'directorioUsuario = "D:\Desarrollo\Alcuadrado\OyDVersiones\Desarrollo\Fuentes\A2.OYD.OYDServer\A2.OyD.OYDServer.Services\Uploads\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim objResultado As String

            objResultado = objImportar.RecorrerArchivoExcelFlujoOrdenes(directorioUsuario & "\", pstrNombreCompletoArchivo)

            Dim objListaRespuesta As New List(Of RespuestaArchivoImportacion)

            If Not String.IsNullOrEmpty(objResultado) Then
                Dim ret = Me.DataContext.uspOyDNet_FlujosOrden_RespuestaFraccionamiento(objResultado, pstrUsuario, DemeInfoSesion(pstrUsuario, "FlujoOrdenes_CargarArchivoRespuesta"), 0)
                objListaRespuesta = ret.ToList
            Else
                objListaRespuesta.Add(New RespuestaArchivoImportacion With {.Columna = 0,
                                                                            .Exitoso = False,
                                                                            .Fila = 0,
                                                                            .ID = 1,
                                                                            .Mensaje = "El archivo no coincide con una estructura valida o no contiene registros.",
                                                                            .NroProceso = 0,
                                                                            .Tipo = "Estructura"})
            End If

            Return objListaRespuesta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FlujoOrdenes_CargarArchivoRespuesta")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Importar BanRep"
    Public Function CargarArchivoImportacionBanRep(ByVal pstrNombreCompletoArchivo As String, pstrNombreProceso As String, ByVal pstrUsuario As String, pstrRutaFisicaGeneracion As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacionOTC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = pstrRutaFisicaGeneracion & "\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso & "\"

            Dim objStringordenes As StringBuilder = Nothing
            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            objStringordenes = objImportar.RecorrerArchivoExcelBanRep(directorioUsuario, pstrNombreCompletoArchivo)
            Dim xmlLista As System.Xml.Linq.XElement

            Try
                xmlLista = System.Xml.Linq.XElement.Parse(objStringordenes.ToString())
                objStringordenes.Clear()
            Catch ex As Exception
                Dim resp As RespuestaArchivoImportacionOTC = New RespuestaArchivoImportacionOTC() With {.Columna = 0,
                    .Fila = 0,
                    .ID = 0,
                    .Tipo = String.Empty,
                    .Mensaje = "Estructura no válida para el archivo, el archivo debe tener la siguiente estructura:" + vbNewLine +
                    "Cod.Titulo | Fecha Liquidación | Nro Emisión | Tipo | Nemotécnico | ISIN | Precio | Tasa Efectiva | Valor Nominal | Valor Operación | Nro. de Oferta | Valor de Restitución | Fecha Restitución"}
                Dim lsresp As New List(Of RespuestaArchivoImportacionOTC)
                lsresp.Add(resp)
                Return lsresp
            End Try

            'Dim objListaRespuesta As New List(Of RespuestaArchivoImportacion)

            Dim strRutaFisicaCargaArchivo As String = pstrRutaFisicaGeneracion & "\" & pstrUsuario.Replace(".", "_") & "\" & pstrNombreProceso & "\" & pstrNombreCompletoArchivo
            'Dim strRutaFisicaCargaArchivo As String = "\\A2WEBDLLO\SitiosWebDllo\OyD\OyD.Net\OYDServiciosRIA\Uploads\edgar_munoz\OrdenesLEO\ordenesRV4.csv"

            Dim ret = Me.DataContext.usp_ValidarResposBanRep(strRutaFisicaCargaArchivo, pstrUsuario).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionLEO")
            Return Nothing
        End Try

    End Function

    Public Function ObtenerValoresArchivoReposBanRep(ByVal pdblNroProceso As Integer, ByVal pstrUsuario As String, pintUltimoRegistro As Integer, ByVal pstrInfoConexion As String) As List(Of ArchivoReposBanRep)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarReposBanRep_Archivo(pdblNroProceso, pstrUsuario, pintUltimoRegistro)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerValoresArchivoRepos")
            Return Nothing
        End Try
    End Function

    Public Function GenerarLiquidacionesRepos(ByVal pdblNroProceso As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_LiquidacionesRepos_lanzarJob(pstrUsuario, pdblNroProceso)
            Return ret.ToString
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarLiquidacionesRepos")
            Return Nothing
        End Try
    End Function

    Public Function VerificarGeneracionLiquidacionesRepos(ByVal pdblNroProceso As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacionOTC)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_LiquidacionesRepos_verificar_archivo(pstrUsuario, pdblNroProceso).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarGeneracionLiquidacionesRepos")
            Return Nothing
        End Try
    End Function

    Public Sub UpdateArchivoResposBanRep(ByVal currentArchivoReposBanRep As ArchivoReposBanRep)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, currentArchivoReposBanRep.pstrUsuarioConexion, currentArchivoReposBanRep.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.ArchivoReposBanReps.Attach(currentArchivoReposBanRep, Me.ChangeSet.GetOriginal(currentArchivoReposBanRep))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateArchivoResposBanRep")
        End Try
    End Sub

#End Region

#Region "Titulos"

#Region "Métodos asincrónicos"

    Public Function ClientesActivosNoBloqueados_Consultar(ByVal plngIDCuentaDeposito As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesActivosNoBloqueados)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_Titulos_ClientesActivosNoBloqueados_Consultar(plngIDCuentaDeposito, pstrDeposito).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClientesActivosNoBloqueados_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function MovimientosArchivoCambio_Procesar(ByVal plngLinea As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pstrEstadoEntrada As String, ByVal pstrEstadoSalida As String, ByVal pstrTipoMov As String, ByVal pvarOpcional1 As String, ByVal pvarOpcional2 As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Titulos_MovimientosArchivoCambio_Procesar(plngLinea, plngIDComitente, pstrEstadoEntrada, pstrEstadoSalida, pstrTipoMov, pvarOpcional1, pvarOpcional2, pstrUsuario, DemeInfoSesion(pstrUsuario, "MovimientosArchivoCambio_Procesar"), 0).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MovimientosArchivoCambio_Procesar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function ClientesActivosNoBloqueados_ConsultarSync(ByVal plngIDCuentaDeposito As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClientesActivosNoBloqueados)
        Dim objTask As Task(Of List(Of ClientesActivosNoBloqueados)) = Me.ClientesActivosNoBloqueados_ConsultarAsync(plngIDCuentaDeposito, pstrDeposito, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ClientesActivosNoBloqueados_ConsultarAsync(ByVal plngIDCuentaDeposito As System.Nullable(Of Integer), ByVal pstrDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ClientesActivosNoBloqueados))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ClientesActivosNoBloqueados)) = New TaskCompletionSource(Of List(Of ClientesActivosNoBloqueados))()
        objTaskComplete.TrySetResult(ClientesActivosNoBloqueados_Consultar(plngIDCuentaDeposito, pstrDeposito, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function MovimientosArchivoCambio_ProcesarSync(ByVal plngLinea As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pstrEstadoEntrada As String, ByVal pstrEstadoSalida As String, ByVal pstrTipoMov As String, ByVal pvarOpcional1 As String, ByVal pvarOpcional2 As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Dim objTask As Task(Of List(Of RespuestaArchivoImportacion)) = Me.MovimientosArchivoCambio_ProcesarAsync(plngLinea, plngIDComitente, pstrEstadoEntrada, pstrEstadoSalida, pstrTipoMov, pvarOpcional1, pvarOpcional2, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function MovimientosArchivoCambio_ProcesarAsync(ByVal plngLinea As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pstrEstadoEntrada As String, ByVal pstrEstadoSalida As String, ByVal pstrTipoMov As String, ByVal pvarOpcional1 As String, ByVal pvarOpcional2 As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaArchivoImportacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaArchivoImportacion)) = New TaskCompletionSource(Of List(Of RespuestaArchivoImportacion))()
        objTaskComplete.TrySetResult(MovimientosArchivoCambio_Procesar(plngLinea, plngIDComitente, pstrEstadoEntrada, pstrEstadoSalida, pstrTipoMov, pvarOpcional1, pvarOpcional2, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Carga Masiva tesoreria"
    Public Function CargarArchivoImportacionTesoreriaMasiva(ByVal pstrTipoSeparadorFormato As String, ByVal pstrNombreCompletoArchivo As String, pstrNombreProceso As String, ByVal pstrTipoDocumento As String, ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Dim strRutaCompletaArchivoImportar As String = String.Empty
            Dim strRutaCompletaArchivoFMT As String = String.Empty
            Dim strEncabezadoTexto As String
            Dim strRutaFMT As String

            strEncabezadoTexto = ObtenerEncabezadoArchivo(pstrTipoSeparadorFormato, directorioUsuario & "\" & pstrNombreCompletoArchivo, pstrUsuario, pstrInfoConexion)
            strRutaFMT = CrearFMTDinamico(pstrTipoSeparadorFormato, directorioUsuario & "\", pstrUsuario, strEncabezadoTexto, pstrInfoConexion)

            If Right(objDatosRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivoImportar = objDatosRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
                strRutaCompletaArchivoFMT = objDatosRutas.RutaArchivoProceso & "\" & strRutaFMT
            Else
                strRutaCompletaArchivoImportar = objDatosRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
                strRutaCompletaArchivoFMT = objDatosRutas.RutaArchivoProceso & strRutaFMT
            End If

            Dim objListaRespuesta As New List(Of RespuestaArchivoImportacion)

            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaTesoreria_LeerArchivo(pstrTipoDocumento, pstrNombreConsecutivo, strEncabezadoTexto, strRutaCompletaArchivoImportar, strRutaCompletaArchivoFMT, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "CargarArchivoImportacionTesoreriaMasiva"), 0)
            objListaRespuesta = ret.ToList

            BorrarFMTDinamico(strRutaCompletaArchivoFMT)

            Return objListaRespuesta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoImportacionTesoreriaMasiva")
            Return Nothing
        End Try
    End Function

    Private Function ObtenerEncabezadoArchivo(ByVal pstrTipoSeparadorFormato As String, ByVal pstrRutaCompletaArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objStream As New StreamReader(pstrRutaCompletaArchivo)
        Dim strPrimeraLinea As String
        Dim strStrColumnasSinEspacios As String = String.Empty
        Dim strSeparador As String = String.Empty

        If Not String.IsNullOrEmpty(pstrTipoSeparadorFormato) Then
            If pstrTipoSeparadorFormato.ToUpper = "TAB" Then
                strSeparador = Chr(9)
            Else
                strSeparador = ","
            End If
        Else
            strSeparador = ","
        End If

        strPrimeraLinea = objStream.ReadLine()

        For Each objColumna In strPrimeraLinea.Split(CChar(strSeparador))
            If String.IsNullOrEmpty(strStrColumnasSinEspacios) Then
                strStrColumnasSinEspacios = Trim(objColumna)
            Else
                strStrColumnasSinEspacios = strStrColumnasSinEspacios & "," & Trim(objColumna)
            End If
        Next

        objStream.Close()
        objStream.Dispose()
        Return strStrColumnasSinEspacios
    End Function

    Private Function CrearFMTDinamico(ByVal pstrTipoSeparadorFormato As String, ByVal pstrDirectorioUsuario As String, ByVal pstrUsuario As String, ByVal pstrPrimeraFila As String, ByVal pstrInfoConexion As String) As String
        Dim strNombreFMT As String = "FMT" & "_" & pstrUsuario & "_" & Now.ToString("yyyyMMddhhmmss") & ".fmt"
        Dim strRutaCompletaFMT As String = pstrDirectorioUsuario & strNombreFMT
        Dim objColumnasRegistros As String() = pstrPrimeraFila.Split(CChar(","))
        Dim intContador As Integer = 0
        Dim strSeparador As String = String.Empty

        If Not String.IsNullOrEmpty(pstrTipoSeparadorFormato) Then
            If pstrTipoSeparadorFormato.ToUpper = "TAB" Then
                strSeparador = "\t"
            Else
                strSeparador = ","
            End If
        Else
            strSeparador = ","
        End If


        If File.Exists(strRutaCompletaFMT) Then
            File.Delete(strRutaCompletaFMT)
        End If
        Dim objStream As FileStream = File.Create(strRutaCompletaFMT)
        AdicionarTexto(objStream, "10.0")
        AdicionarTexto(objStream, CStr(objColumnasRegistros.Count))
        intContador = 1

        For Each strColumna In objColumnasRegistros
            AdicionarTexto(objStream, String.Format("{0}		SQLCHAR		0		8000		""{1}""			{2}			{3}						""""",
                                                    intContador,
                                                    IIf(intContador = objColumnasRegistros.Count, "\r\n", strSeparador),
                                                    intContador + 1,
                                                    Trim(strColumna))
                        )
            intContador += 1
        Next
        objStream.Close()

        Return strNombreFMT
    End Function

    Private Function BorrarFMTDinamico(ByVal pstrDirectorioUsuario As String) As Boolean
        Try
            If File.Exists(pstrDirectorioUsuario) Then
                File.Delete(pstrDirectorioUsuario)
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Shared Sub AdicionarTexto(ByVal fs As FileStream, ByVal value As String)
        value = value & Environment.NewLine
        Dim info As Byte() = New UTF8Encoding(True).GetBytes(value)
        fs.Write(info, 0, info.Length)
    End Sub

#End Region

#Region "Derechos patrimoniales DER245"

    Public Function CargarArchivoDecevalDER245(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacionDER245)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_DerechosPatrimonialesDER245_Importar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacionDER245)
            Dim strRutaCompletaArchivos As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)

            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
            Else
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
            End If

            objListaParametros.Add(CrearSQLParameter("pstrRutaCompletaArchivo", strRutaCompletaArchivos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "GenerarArchivoPlano"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim strArchivo As String = String.Empty
            Dim intContador As Integer = 1

            strArchivo = GenerarExcel(objDatosConsulta, objRutas.RutaArchivosLocal, "ResultadoDER245_" & Now.ToString("yyyyMMddhhmmss"))

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            For Each objRow As DataRow In objDatosConsulta.Rows
                Dim objItem As New RespuestaArchivoImportacionDER245
                If Not IsNothing(objRow("ValorInconsistente")) And Not IsDBNull(objRow("ValorInconsistente")) Then
                    objItem.ValorInconsistente = CStr(objRow("ValorInconsistente"))
                End If
                If Not IsNothing(objRow("ColumnaInconsistente")) And Not IsDBNull(objRow("ColumnaInconsistente")) Then
                    objItem.ColumnaInconsistente = CStr(objRow("ColumnaInconsistente"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.Mensaje = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("NroDocumento")) And Not IsDBNull(objRow("NroDocumento")) Then
                    objItem.NroDocumento = CStr(objRow("NroDocumento"))
                End If
                If Not IsNothing(objRow("CuentaInversionista")) And Not IsDBNull(objRow("CuentaInversionista")) Then
                    objItem.CuentaInversionista = CStr(objRow("CuentaInversionista"))
                End If

                objItem.ID = intContador
                objItem.URLArchivo = objRutas.RutaCompartidaOWeb() & strArchivo

                intContador += 1
                objListaRetorno.Add(objItem)
            Next


            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarArchivoDecevalDER245")
            Return Nothing
        End Try


    End Function

    Public Function ConsultarResultadoDER245(ByVal pstrTipoConsulta As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_DerechosPatrimonialesDER245_Consultar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)

            Dim objListaParametros As New List(Of SqlParameter)
            objListaParametros.Add(CrearSQLParameter("pstrTipoConsulta", pstrTipoConsulta, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "ConsultarResultadoDER245"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim strArchivo As String = String.Empty

            If pstrTipoConsulta = "A" Then
                strArchivo = GenerarExcel(objDatosConsulta, objRutas.RutaArchivosLocal, "ResultadoAccionesDER245_" & Now.ToString("yyyyMMddhhmmss"))
            Else
                strArchivo = GenerarExcel(objDatosConsulta, objRutas.RutaArchivosLocal, "ResultadoRentaFijaDER245_" & Now.ToString("yyyyMMddhhmmss"))
            End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 1,
                                                                      .Mensaje = "El archivo se generó exitosamente",
                                                                      .Tipo = "",
                                                                      .URLArchivo = objRutas.RutaCompartidaOWeb() & strArchivo})

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarResultadoDER245")
            Return Nothing
        End Try


    End Function


#End Region

#Region "Generar Ajustes automaticos"

    Public Function Carga_AjustesAutomaticos(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrConsecutivo As String, ByVal pstrTipoAjuste As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_GenerarAjustesAutomaticos_Importar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strRutaCompletaArchivos As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)

            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
            Else
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
            End If

            objListaParametros.Add(CrearSQLParameter("pstrRutaCompletaArchivo", strRutaCompletaArchivos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrNombreConsecutivo", pstrConsecutivo, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrTipoAjuste", pstrTipoAjuste, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "Carga_AjustesAutomaticos"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Rows
                Dim objItem As New RespuestaArchivoImportacion

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.Mensaje = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Campo")) And Not IsDBNull(objRow("Campo")) Then
                    objItem.Campo = CStr(objRow("Campo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Carga_AjustesAutomaticos")
            Return Nothing
        End Try


    End Function

#End Region

    ''' <history>
    ''' Modificado por   : Daniel Esteban Marin
    ''' Descripción      : Cuentas Activas Confianza(importacion)
    ''' Fecha            : julio 07/2016
    ''' </history>
#Region "Carteras Cuentas Activas Confianza"
    Public Function CuentasActivasConfianza_Procesar(ByVal plogCambioRutaDefectoArchivo As Boolean, ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = Nothing
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_CuentasActivasConfianza_Procesar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strRutaCompletaArchivos As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)
            Dim logTomarRutaCompleta As Boolean = False

            If plogCambioRutaDefectoArchivo Then
                objRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
                If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                    strRutaCompletaArchivos = objRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
                Else
                    strRutaCompletaArchivos = objRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
                End If
                logTomarRutaCompleta = False
            Else
                logTomarRutaCompleta = True
                strRutaCompletaArchivos = pstrNombreCompletoArchivo
            End If

            objListaParametros.Add(CrearSQLParameter("plogTomarRutaCompleta", logTomarRutaCompleta, SqlDbType.Bit))
            objListaParametros.Add(CrearSQLParameter("pstrRutaCompletaArchivo", strRutaCompletaArchivos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSession", DemeInfoSesion(pstrUsuario, "CuentasActivasConfianza_Procesar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Rows
                Dim objItem As New RespuestaArchivoImportacion

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.Mensaje = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Campo")) And Not IsDBNull(objRow("Campo")) Then
                    objItem.Campo = CStr(objRow("Campo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasActivasConfianza_Procesar")
            Return Nothing
        End Try


    End Function
#End Region

#Region "Recibos recaudo"

    Public Function RecibosRecaudo_Exportar(ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[usp_ExportarRecibosRecaudo_OYDNET]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strNombreArchivo As String = String.Empty
            Dim strRutaArchivoGeneracion As String = String.Empty

            Dim objListaParametros As New List(Of SqlParameter)
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "ConsultarResultadoDER245"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                If Not IsNothing(objRow("NombreArchivo")) And Not IsDBNull(objRow("NombreArchivo")) Then
                    strNombreArchivo = CStr(objRow("NombreArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivo")) And Not IsDBNull(objRow("RutaArchivo")) Then
                    strRutaArchivoGeneracion = CStr(objRow("RutaArchivo"))
                End If
            Next

            Dim strArchivo As String = String.Empty

            strArchivo = GenerarExcel(objDatosConsulta.Tables(1), strRutaArchivoGeneracion, strNombreArchivo)

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                     .Columna = 0,
                                                                     .Exitoso = True,
                                                                     .Fila = 1,
                                                                     .ID = 2,
                                                                     .Mensaje = "El archivo se generó en la ruta " & strRutaArchivoGeneracion & strArchivo,
                                                                     .Tipo = ""})

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RecibosRecaudo_Exportar")
            Return Nothing
        End Try
    End Function

    Public Function RecibosRecaudo_Procesar(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = Nothing
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_RecibosRecaudo_Procesar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strRutaCompletaArchivos As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)

            objRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
            Else
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
            End If

            objListaParametros.Add(CrearSQLParameter("pstrRutaCompletaArchivo", strRutaCompletaArchivos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfosesion", DemeInfoSesion(pstrUsuario, "CuentasActivasConfianza_Procesar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Rows
                Dim objItem As New RespuestaArchivoImportacion

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.Mensaje = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Campo")) And Not IsDBNull(objRow("Campo")) Then
                    objItem.Campo = CStr(objRow("Campo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasActivasConfianza_Procesar")
            Return Nothing
        End Try


    End Function

#End Region

#Region "Transferencia Pagos masivos"

    Public Function TransferenciaPagosMasivos_Importar(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = Nothing
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_TransferenciaPagosMasivos_Importar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strRutaCompletaArchivos As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)

            objRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
            Else
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
            End If

            objListaParametros.Add(CrearSQLParameter("pstrRutaCompletaArchivo", strRutaCompletaArchivos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "TransferenciaPagosMasivos_Importar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Rows
                Dim objItem As New RespuestaArchivoImportacion

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.Mensaje = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Campo")) And Not IsDBNull(objRow("Campo")) Then
                    objItem.Campo = CStr(objRow("Campo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciaPagosMasivos_Importar")
            Return Nothing
        End Try
    End Function

    Public Function TransferenciaPagosMasivos_Exportar(ByVal plngIDDocumento As Integer, ByVal pstrNombreConsecutivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_TransferenciaPagosMasivos_Exportar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strNombreArchivo As String = String.Empty
            Dim strRutaArchivoGeneracion As String = String.Empty
            Dim strRutaBackupGeneracion As String = String.Empty
            Dim strArchivoRutaGeneracion As String = String.Empty
            Dim strArchivoRutaBackup As String = String.Empty


            Dim objListaParametros As New List(Of SqlParameter)
            objListaParametros.Add(CrearSQLParameter("plngIDDocumento", plngIDDocumento, SqlDbType.Int))
            objListaParametros.Add(CrearSQLParameter("pstrNombreConsecutivo", pstrNombreConsecutivo, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "TransferenciaPagosMasivos_Exportar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                If Not IsNothing(objRow("NombreArchivo")) And Not IsDBNull(objRow("NombreArchivo")) Then
                    strNombreArchivo = CStr(objRow("NombreArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivo")) And Not IsDBNull(objRow("RutaArchivo")) Then
                    strRutaArchivoGeneracion = CStr(objRow("RutaArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivoBackup")) And Not IsDBNull(objRow("RutaArchivoBackup")) Then
                    strRutaBackupGeneracion = CStr(objRow("RutaArchivoBackup"))
                End If
            Next

            If Not String.IsNullOrEmpty(strRutaArchivoGeneracion) Then
                If Not Directory.Exists(strRutaArchivoGeneracion) Then
                    Directory.CreateDirectory(strRutaArchivoGeneracion)
                End If

                'JFSB20180839 Se ajusta la codificación del archivo a generar para que sea ASCII
                strArchivoRutaGeneracion = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaArchivoGeneracion, strNombreArchivo & "_" & Now.ToString("yyyyMMdd"), ".txt", String.Empty, True, Encoding.ASCII)
            End If

            If Not String.IsNullOrEmpty(strRutaBackupGeneracion) Then
                If Not Directory.Exists(strRutaBackupGeneracion) Then
                    Directory.CreateDirectory(strRutaBackupGeneracion)
                End If

                'JFSB20180839 Se ajusta la codificación del archivo a generar para que sea ASCII
                strArchivoRutaBackup = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaBackupGeneracion, strNombreArchivo & "_" & Now.ToString("yyyyMMdd"), ".txt", String.Empty, True, Encoding.ASCII)
            End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            If Right(strRutaArchivoGeneracion, 1) <> "\" Then
                strRutaArchivoGeneracion = strRutaArchivoGeneracion & "\"
            End If

            If Right(strRutaBackupGeneracion, 1) <> "\" Then
                strRutaBackupGeneracion = strRutaBackupGeneracion & "\"
            End If

            If Not String.IsNullOrEmpty(strArchivoRutaGeneracion) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 2,
                                                                      .Mensaje = "El archivo se generó en la ruta " & strRutaArchivoGeneracion & strArchivoRutaGeneracion,
                                                                      .Tipo = ""})
            End If

            If Not String.IsNullOrEmpty(strArchivoRutaBackup) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 3,
                                                                      .Mensaje = "El backup del archivo se generó en la ruta " & strRutaBackupGeneracion & strArchivoRutaBackup,
                                                                      .Tipo = ""})
            End If

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciaPagosMasivos_Exportar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Transacciones carteras colectivas"

    Public Function TransaccionesCarterasColectivas_Exportar(ByVal pstrRutaArchivoGeneracion As String, ByVal pstrNombreArchivo As String, ByVal pdtmFechaProceso As DateTime, ByVal pstrNombreConsecutivo As String, ByVal pstrCuentaBancaria As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_TransaccionesCarterasColectivas_Exportar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)

            Dim objListaParametros As New List(Of SqlParameter)
            Dim objParametroFechaProceso As New SqlParameter()
            objParametroFechaProceso.ParameterName = "@" & "pdtmFechaProceso"
            objParametroFechaProceso.DbType = DbType.DateTime
            objParametroFechaProceso.Value = pdtmFechaProceso

            objListaParametros.Add(objParametroFechaProceso)
            objListaParametros.Add(CrearSQLParameter("pstrNombreConsecutivo", pstrNombreConsecutivo, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrCuentaBancaria", pstrCuentaBancaria, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "TransaccionesCarterasColectivas_Exportar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim strArchivoRutaGeneracion As String = String.Empty

            If Not String.IsNullOrEmpty(pstrRutaArchivoGeneracion) Then
                If Not Directory.Exists(pstrRutaArchivoGeneracion) Then
                    Directory.CreateDirectory(pstrRutaArchivoGeneracion)
                End If

                'JFSB20180839 Se ajusta la codificación del archivo a generar para que sea ASCII
                strArchivoRutaGeneracion = GenerarTextoPlano(objDatosConsulta, pstrRutaArchivoGeneracion, pstrNombreArchivo, Nothing, String.Empty, True, False, False)
            End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            If Right(pstrRutaArchivoGeneracion, 1) <> "\" Then
                pstrRutaArchivoGeneracion = pstrRutaArchivoGeneracion & "\"
            End If

            If Not String.IsNullOrEmpty(pstrRutaArchivoGeneracion) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 2,
                                                                      .Mensaje = "Se generó en la ruta " & pstrRutaArchivoGeneracion & strArchivoRutaGeneracion,
                                                                      .Tipo = ""})
            End If

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransaccionesCarterasColectivas_Exportar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Transferencias carteras colectivas"

    Public Function TransferenciasCarterasColectivas_Exportar(ByVal pstrRutaArchivoGeneracion As String, ByVal pstrNombreArchivo As String, ByVal pdtmFechaProceso As DateTime, ByVal pstrNombreConsecutivo As String, ByVal pstrCuentaBancaria As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_TransferenciasCarterasColectivas_Exportar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)

            Dim objListaParametros As New List(Of SqlParameter)
            Dim objParametroFechaProceso As New SqlParameter()
            objParametroFechaProceso.ParameterName = "@" & "pdtmFechaProceso"
            objParametroFechaProceso.DbType = DbType.DateTime
            objParametroFechaProceso.Value = pdtmFechaProceso

            objListaParametros.Add(objParametroFechaProceso)
            objListaParametros.Add(CrearSQLParameter("pstrNombreConsecutivo", pstrNombreConsecutivo, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrCuentaBancaria", pstrCuentaBancaria, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "TransferenciasCarterasColectivas_Exportar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim strArchivoRutaGeneracion As String = String.Empty

            If Not String.IsNullOrEmpty(pstrRutaArchivoGeneracion) Then
                If Not Directory.Exists(pstrRutaArchivoGeneracion) Then
                    Directory.CreateDirectory(pstrRutaArchivoGeneracion)
                End If

                strArchivoRutaGeneracion = GenerarTextoPlano(objDatosConsulta, pstrRutaArchivoGeneracion, pstrNombreArchivo, ".txt", String.Empty)
            End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            If Right(pstrRutaArchivoGeneracion, 1) <> "\" Then
                pstrRutaArchivoGeneracion = pstrRutaArchivoGeneracion & "\"
            End If

            If Not String.IsNullOrEmpty(pstrRutaArchivoGeneracion) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 2,
                                                                      .Mensaje = "Se generó en la ruta " & pstrRutaArchivoGeneracion & strArchivoRutaGeneracion,
                                                                      .Tipo = ""})
            End If

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciasCarterasColectivas_Exportar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Transferencias a cuentas no inscritas"

    Public Function TransferenciasACuentasNoInscritas_Exportar(ByVal pdtmFechaProceso As DateTime, ByVal pstrTipoBanco As String, ByVal pstrCuentaBancaria As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_TransferenciaCuentasNoInscritas_Exportar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strNombreArchivo As String = String.Empty
            Dim strRutaArchivoGeneracion As String = String.Empty
            Dim strRutaBackupGeneracion As String = String.Empty
            Dim strArchivoRutaGeneracion As String = String.Empty
            Dim strArchivoRutaBackup As String = String.Empty

            Dim objListaParametros As New List(Of SqlParameter)
            Dim objParametroFechaProceso As New SqlParameter()
            objParametroFechaProceso.ParameterName = "@" & "pdtmFechaProceso"
            objParametroFechaProceso.DbType = DbType.DateTime
            objParametroFechaProceso.Value = pdtmFechaProceso

            objListaParametros.Add(objParametroFechaProceso)
            objListaParametros.Add(CrearSQLParameter("pstrTipoBanco", pstrTipoBanco, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrCuentaBancaria", pstrCuentaBancaria, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "TransferenciasCarterasColectivas_Exportar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                If Not IsNothing(objRow("NombreArchivo")) And Not IsDBNull(objRow("NombreArchivo")) Then
                    strNombreArchivo = CStr(objRow("NombreArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivo")) And Not IsDBNull(objRow("RutaArchivo")) Then
                    strRutaArchivoGeneracion = CStr(objRow("RutaArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivoBackup")) And Not IsDBNull(objRow("RutaArchivoBackup")) Then
                    strRutaBackupGeneracion = CStr(objRow("RutaArchivoBackup"))
                End If
            Next

            If Not String.IsNullOrEmpty(strRutaArchivoGeneracion) Then
                If Not Directory.Exists(strRutaArchivoGeneracion) Then
                    Directory.CreateDirectory(strRutaArchivoGeneracion)
                End If

                'JFSB20180839 Se ajusta la codificación del archivo a generar para que sea ASCII
                strArchivoRutaGeneracion = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaArchivoGeneracion, strNombreArchivo & "_" & Now.ToString("yyyyMMdd"), ".txt", String.Empty, True, Encoding.ASCII)
            End If

            If Not String.IsNullOrEmpty(strRutaBackupGeneracion) Then
                If Not Directory.Exists(strRutaBackupGeneracion) Then
                    Directory.CreateDirectory(strRutaBackupGeneracion)
                End If

                'JFSB20180839 Se ajusta la codificación del archivo a generar para que sea ASCII
                strArchivoRutaBackup = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaBackupGeneracion, strNombreArchivo & "_" & Now.ToString("yyyyMMdd"), ".txt", String.Empty, True, Encoding.ASCII)
            End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            If Right(strRutaArchivoGeneracion, 1) <> "\" Then
                strRutaArchivoGeneracion = strRutaArchivoGeneracion & "\"
            End If

            If Right(strRutaBackupGeneracion, 1) <> "\" Then
                strRutaBackupGeneracion = strRutaBackupGeneracion & "\"
            End If

            If Not String.IsNullOrEmpty(strRutaArchivoGeneracion) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 2,
                                                                      .Mensaje = "Se generó en la ruta " & strRutaArchivoGeneracion & strArchivoRutaGeneracion,
                                                                      .Tipo = ""})
            End If

            If Not String.IsNullOrEmpty(strArchivoRutaBackup) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 3,
                                                                      .Mensaje = "El backup del archivo se generó en la ruta " & strRutaBackupGeneracion & strArchivoRutaBackup,
                                                                      .Tipo = ""})
            End If

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciasCarterasColectivas_Exportar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Generar auditoria recaudos"

    Public Function GenerarAuditoriaRecaudos_GenerarTotales(ByVal pstrNombreCompletoArchivo As String, ByVal pstrRutaArchivoGeneracion As String, ByVal pstrNombreArchivoGeneracion As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_GenerarAuditoriaRecaudos_Totales]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim objListaParametros As New List(Of SqlParameter)
            Dim strRutaCompletaArchivos As String = String.Empty

            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
            Else
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
            End If

            objListaParametros.Add(CrearSQLParameter("pstrRutaCompletaArchivo", strRutaCompletaArchivos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "GenerarAuditoriaRecaudos_GenerarTotales"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                Dim objItem As New RespuestaArchivoImportacion

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.Mensaje = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Campo")) And Not IsDBNull(objRow("Campo")) Then
                    objItem.Campo = CStr(objRow("Campo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            Dim strArchivo As String = String.Empty
            Dim strArchivoRutaGeneracion As String = String.Empty

            strArchivo = GenerarTextoPlano(objDatosConsulta.Tables(1), objRutas.RutaArchivosLocal, pstrNombreArchivoGeneracion & Now.ToString("yyyyMMddhhmmss"), "csv", ",", True, True, True)

            'If Not String.IsNullOrEmpty(pstrRutaArchivoGeneracion) Then
            '    If Not Directory.Exists(pstrRutaArchivoGeneracion) Then
            '        Directory.CreateDirectory(pstrRutaArchivoGeneracion)
            '    End If

            '    strArchivoRutaGeneracion = GenerarTextoPlano(objDatosConsulta.Tables(1), pstrRutaArchivoGeneracion, pstrNombreArchivoGeneracion, ".txt", String.Empty)
            'End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            'If Right(pstrRutaArchivoGeneracion, 1) <> "\" Then
            '    pstrRutaArchivoGeneracion = pstrRutaArchivoGeneracion & "\"
            'End If

            objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = intContador,
                                                                      .Mensaje = "El archivo se generó exitosamente",
                                                                      .Tipo = "ARCHIVO",
                                                                      .URLArchivo = objRutas.RutaCompartidaOWeb() & strArchivo})

            'intContador += 1
            'If Not String.IsNullOrEmpty(pstrRutaArchivoGeneracion) Then
            '    objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
            '                                                          .Columna = 0,
            '                                                          .Exitoso = True,
            '                                                          .Fila = 1,
            '                                                          .ID = intContador,
            '                                                          .Mensaje = "Se genero en la ruta " & pstrRutaArchivoGeneracion & strArchivoRutaGeneracion,
            '                                                          .Tipo = ""})
            'End If

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarAuditoriaRecaudos_GenerarTotales")
            Return Nothing
        End Try
    End Function

    Public Function GenerarAuditoriaRecaudos_GenerarSaldosBancos(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_GenerarAuditoriaRecaudos_Saldos]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim objListaParametros As New List(Of SqlParameter)
            Dim strRutaCompletaArchivos As String = String.Empty

            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
            Else
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
            End If

            objListaParametros.Add(CrearSQLParameter("pstrRutaCompletaArchivo", strRutaCompletaArchivos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "GenerarAuditoriaRecaudos_GenerarTotales"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                Dim objItem As New RespuestaArchivoImportacion

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.Mensaje = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Campo")) And Not IsDBNull(objRow("Campo")) Then
                    objItem.Campo = CStr(objRow("Campo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            Dim strArchivo As String = String.Empty
            Dim strArchivoRutaGeneracion As String = String.Empty
            Dim objTablasExcel As New List(Of DataTable)

            objDatosConsulta.Tables(1).TableName = "Cuentas Centralizadas"
            objDatosConsulta.Tables(2).TableName = "Cuentas NO Centralizadoras"

            objTablasExcel.Add(objDatosConsulta.Tables(1))
            objTablasExcel.Add(objDatosConsulta.Tables(2))

            strArchivo = GenerarExcel(objTablasExcel, objRutas.RutaArchivosLocal, "GenerarAuditoriaRecaudos" & Now.ToString("yyyyMMddhhmmss"))

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 1,
                                                                      .Mensaje = "El archivo se generó exitosamente",
                                                                      .Tipo = "ARCHIVO",
                                                                      .URLArchivo = objRutas.RutaCompartidaOWeb() & strArchivo})

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarAuditoriaRecaudos_GenerarSaldosBancos")
            Return Nothing
        End Try
    End Function

#End Region

#Region "PagosPorRedBanco"

    Public Function PagosPorRedBanco_Exportar(ByVal pstrCuentaBancaria As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_PagosPorRedBanco_Exportar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strNombreArchivo As String = String.Empty
            Dim strRutaArchivoGeneracion As String = String.Empty
            Dim strRutaBackupGeneracion As String = String.Empty
            Dim strArchivoRutaGeneracion As String = String.Empty
            Dim strArchivoRutaBackup As String = String.Empty

            Dim objListaParametros As New List(Of SqlParameter)
            Dim objParametroFechaProceso As New SqlParameter()
            objParametroFechaProceso.ParameterName = "@" & "pdtmFechaProceso"
            objParametroFechaProceso.DbType = DbType.DateTime
            objParametroFechaProceso.Value = pdtmFechaProceso


            objListaParametros.Add(CrearSQLParameter("pstrCuentaBancaria", pstrCuentaBancaria, SqlDbType.VarChar))
            objListaParametros.Add(objParametroFechaProceso)
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "PagosPorRedBanco_Exportar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                If Not IsNothing(objRow("NombreArchivo")) And Not IsDBNull(objRow("NombreArchivo")) Then
                    strNombreArchivo = CStr(objRow("NombreArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivo")) And Not IsDBNull(objRow("RutaArchivo")) Then
                    strRutaArchivoGeneracion = CStr(objRow("RutaArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivoBackup")) And Not IsDBNull(objRow("RutaArchivoBackup")) Then
                    strRutaBackupGeneracion = CStr(objRow("RutaArchivoBackup"))
                End If
            Next

            If Not String.IsNullOrEmpty(strRutaArchivoGeneracion) Then
                If Not Directory.Exists(strRutaArchivoGeneracion) Then
                    Directory.CreateDirectory(strRutaArchivoGeneracion)
                End If

                'JFSB20180839 Se ajusta la codificación del archivo a generar para que sea ASCII
                strArchivoRutaGeneracion = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaArchivoGeneracion, strNombreArchivo & "_" & Now.ToString("yyyyMMdd"), ".txt", String.Empty, True, Encoding.ASCII)
            End If

            If Not String.IsNullOrEmpty(strRutaBackupGeneracion) Then
                If Not Directory.Exists(strRutaBackupGeneracion) Then
                    Directory.CreateDirectory(strRutaBackupGeneracion)
                End If

                'JFSB20180839 Se ajusta la codificación del archivo a generar para que sea ASCII
                strArchivoRutaBackup = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaBackupGeneracion, strNombreArchivo & "_" & Now.ToString("yyyyMMdd"), ".txt", String.Empty, True, Encoding.ASCII)
            End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            If Right(strRutaArchivoGeneracion, 1) <> "\" Then
                strRutaArchivoGeneracion = strRutaArchivoGeneracion & "\"
            End If

            If Right(strRutaBackupGeneracion, 1) <> "\" Then
                strRutaBackupGeneracion = strRutaBackupGeneracion & "\"
            End If

            If Not String.IsNullOrEmpty(strArchivoRutaGeneracion) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 2,
                                                                      .Mensaje = "El archivo de transferencias se generó en la ruta: " & strRutaArchivoGeneracion & strArchivoRutaGeneracion,
                                                                      .Tipo = ""})
            End If

            If Not String.IsNullOrEmpty(strArchivoRutaBackup) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 3,
                                                                      .Mensaje = "El backup del archivo de transferencias se generó en la ruta: " & strRutaBackupGeneracion & strArchivoRutaBackup,
                                                                      .Tipo = ""})
            End If

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PagosPorRedBanco_Exportar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Traslados internos tesoreria"

    Public Function TrasladosTesoreria_Importar(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = Nothing
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_TrasladosTesoreria_Importar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strRutaCompletaArchivos As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)

            objRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
            Else
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
            End If

            objListaParametros.Add(CrearSQLParameter("pstrRutaCompletaArchivo", strRutaCompletaArchivos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "TrasladosTesoreria_Importar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Rows
                Dim objItem As New RespuestaArchivoImportacion

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.Mensaje = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladosTesoreria_Importar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Utilidades"

    Public Function VerificarRutaFisicaServidor(ByVal pstrRutaFisica As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not String.IsNullOrEmpty(pstrRutaFisica) Then
                If Not Directory.Exists(pstrRutaFisica) Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PagosPorRedBanco_Exportar")
            Return False
        End Try
    End Function
    Public Function VerificarRutaFisicaServidorSync(ByVal pstrRutaFisica As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Dim objTask As Task(Of Boolean) = Me.VerificarRutaFisicaServidorAsync(pstrRutaFisica, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function VerificarRutaFisicaServidorAsync(ByVal pstrRutaFisica As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Boolean)
        Dim objTaskComplete As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
        objTaskComplete.TrySetResult(VerificarRutaFisicaServidor(pstrRutaFisica, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region


    ''' <summary>
    ''' Descripción:     Proceso para importar archivo txt y exportar excel desde la pantalla "Generar vencimientos Deceval"
    ''' Responsable:     Catalina Dávila (IOSoft S.A.)
    ''' Fecha:           17 de Septiembre/2016
    ''' </summary>
    ''' <param name="pstrNombreCompletoArchivo"></param>
    ''' <param name="pstrNombreProceso"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="pstrMaquina"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
#Region "GenerarVencimientosDeceval"

    Public Function GenerarVencimientosDeceval_Importar(ByVal pstrNombreCompletoArchivo As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = Nothing
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_GenerarVencimientosDeceval_Importar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strRutaCompletaArchivos As String = String.Empty
            Dim objListaParametros As New List(Of SqlParameter)

            objRutas = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            If Right(objRutas.RutaArchivoProceso, 1) <> "\" Then
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & "\" & pstrNombreCompletoArchivo
            Else
                strRutaCompletaArchivos = objRutas.RutaArchivoProceso & pstrNombreCompletoArchivo
            End If

            objListaParametros.Add(CrearSQLParameter("pstrRutaCompletaArchivo", strRutaCompletaArchivos, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "GenerarVencimientosDeceval_Importar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim intContador As Integer = 1

            For Each objRow As DataRow In objDatosConsulta.Rows
                Dim objItem As New RespuestaArchivoImportacion

                If Not IsNothing(objRow("Columna")) And Not IsDBNull(objRow("Columna")) Then
                    objItem.Columna = CInt(objRow("Columna"))
                End If
                If Not IsNothing(objRow("Fila")) And Not IsDBNull(objRow("Fila")) Then
                    objItem.Fila = CInt(objRow("Fila"))
                End If
                If Not IsNothing(objRow("Mensaje")) And Not IsDBNull(objRow("Mensaje")) Then
                    objItem.Mensaje = CStr(objRow("Mensaje"))
                End If
                If Not IsNothing(objRow("Tipo")) And Not IsDBNull(objRow("Tipo")) Then
                    objItem.Tipo = CStr(objRow("Tipo"))
                End If
                If Not IsNothing(objRow("Exitoso")) And Not IsDBNull(objRow("Exitoso")) Then
                    objItem.Exitoso = CBool(objRow("Exitoso"))
                End If

                objItem.ID = intContador

                intContador += 1
                objListaRetorno.Add(objItem)
            Next

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarVencimientosDeceval_Importar")
            Return Nothing
        End Try
    End Function

    Public Function GenerarVencimientosDeceval_Exportar(ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_GenerarVencimientosDeceval_Exportar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)

            Dim objListaParametros As New List(Of SqlParameter)
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "GenerarVencimientosDeceval_Exportar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataTable = RetornarDataTable(strConexion, strProceso, objListaParametros)
            Dim strArchivo As String = String.Empty

            strArchivo = GenerarExcel(objDatosConsulta, objRutas.RutaArchivosLocal, "VencimientosDeceval_" & Now.ToString("yyyyMMddhhmmss"))

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 1,
                                                                      .Mensaje = "El archivo se generó exitosamente",
                                                                      .Tipo = "",
                                                                      .URLArchivo = objRutas.RutaCompartidaOWeb() & strArchivo})

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarVencimientosDeceval_Exportar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Transferencia electronica"

    <Query(HasSideEffects:=True)>
    Public Function TransferenciasElectronica_ACH_Exportar(ByVal pstrRegistrosGenerar As String, ByVal pdtmFechaProceso As DateTime, ByVal pstrTipoBanco As String, ByVal pstrCuentaBancaria As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRutas As RutasArchivosUnificado = clsClaseUploadsUnificado.FnRutasImportaciones(pstrNombreProceso, pstrUsuario, My.Settings.DirectorioArchivosUpload, My.Settings.DirectorioCompartidoUploads)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_TransferenciaElectronicaACH_Exportar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim objListaRetorno As New List(Of RespuestaArchivoImportacion)
            Dim strNombreArchivo As String = String.Empty
            Dim strRutaArchivoGeneracion As String = String.Empty
            Dim strRutaBackupGeneracion As String = String.Empty
            Dim strArchivoRutaGeneracion As String = String.Empty
            Dim strArchivoRutaBackup As String = String.Empty

            pstrRegistrosGenerar = HttpUtility.UrlDecode(pstrRegistrosGenerar)


            Dim objListaParametros As New List(Of SqlParameter)
            Dim objParametroFechaProceso As New SqlParameter()
            objParametroFechaProceso.ParameterName = "@" & "pdtmFechaProceso"
            objParametroFechaProceso.DbType = DbType.DateTime
            objParametroFechaProceso.Value = pdtmFechaProceso

            objListaParametros.Add(objParametroFechaProceso)
            objListaParametros.Add(CrearSQLParameter("pstrRegistrosGenerar", pstrRegistrosGenerar, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrTipoBanco", pstrTipoBanco, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrCuentaBancaria", pstrCuentaBancaria, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "TransferenciasElectronica_ACH_Exportar"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                If Not IsNothing(objRow("NombreArchivo")) And Not IsDBNull(objRow("NombreArchivo")) Then
                    strNombreArchivo = CStr(objRow("NombreArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivo")) And Not IsDBNull(objRow("RutaArchivo")) Then
                    strRutaArchivoGeneracion = CStr(objRow("RutaArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivoBackup")) And Not IsDBNull(objRow("RutaArchivoBackup")) Then
                    strRutaBackupGeneracion = CStr(objRow("RutaArchivoBackup"))
                End If
            Next

            If Not String.IsNullOrEmpty(strRutaArchivoGeneracion) Then
                If Not Directory.Exists(strRutaArchivoGeneracion) Then
                    Directory.CreateDirectory(strRutaArchivoGeneracion)
                End If

                'JFSB20180839 Se ajusta la codificación del archivo a generar para que sea ASCII
                strArchivoRutaGeneracion = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaArchivoGeneracion, strNombreArchivo & "_" & Now.ToString("yyyyMMdd"), ".txt", String.Empty, True, Encoding.ASCII)
            End If

            If Not String.IsNullOrEmpty(strRutaBackupGeneracion) Then
                If Not Directory.Exists(strRutaBackupGeneracion) Then
                    Directory.CreateDirectory(strRutaBackupGeneracion)
                End If

                'JFSB20180839 Se ajusta la codificación del archivo a generar para que sea ASCII
                strArchivoRutaBackup = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaBackupGeneracion, strNombreArchivo & "_" & Now.ToString("yyyyMMdd"), ".txt", String.Empty, True, Encoding.ASCII)
            End If

            If objRutas.RutaWeb.Substring(objRutas.RutaWeb.Length - 2, 1) <> "/" Then
                objRutas.RutaWeb = objRutas.RutaWeb & "/"
            End If

            If Right(strRutaArchivoGeneracion, 1) <> "\" Then
                strRutaArchivoGeneracion = strRutaArchivoGeneracion & "\"
            End If

            If Right(strRutaBackupGeneracion, 1) <> "\" Then
                strRutaBackupGeneracion = strRutaBackupGeneracion & "\"
            End If

            If Not String.IsNullOrEmpty(strRutaArchivoGeneracion) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 2,
                                                                      .Mensaje = "Se generó en la ruta " & strRutaArchivoGeneracion & strArchivoRutaGeneracion,
                                                                      .Tipo = ""})
            End If

            If Not String.IsNullOrEmpty(strArchivoRutaBackup) Then
                objListaRetorno.Add(New RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 3,
                                                                      .Mensaje = "El backup del archivo se generó en la ruta " & strRutaBackupGeneracion & strArchivoRutaBackup,
                                                                      .Tipo = ""})
            End If

            Return objListaRetorno

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciasElectronica_ACH_Exportar")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function TransferenciasElectronica_ACH_ExportarSync(ByVal pstrRegistrosGenerar As String, ByVal pdtmFechaProceso As DateTime, ByVal pstrTipoBanco As String, ByVal pstrCuentaBancaria As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Dim objTask As Task(Of List(Of RespuestaArchivoImportacion)) = Me.TransferenciasElectronica_ACH_ExportarAsync(pstrRegistrosGenerar, pdtmFechaProceso, pstrTipoBanco, pstrCuentaBancaria, pstrNombreProceso, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function TransferenciasElectronica_ACH_ExportarAsync(ByVal pstrRegistrosGenerar As String, ByVal pdtmFechaProceso As DateTime, ByVal pstrTipoBanco As String, ByVal pstrCuentaBancaria As String, ByVal pstrNombreProceso As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaArchivoImportacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaArchivoImportacion)) = New TaskCompletionSource(Of List(Of RespuestaArchivoImportacion))()
        objTaskComplete.TrySetResult(TransferenciasElectronica_ACH_Exportar(pstrRegistrosGenerar, pdtmFechaProceso, pstrTipoBanco, pstrCuentaBancaria, pstrNombreProceso, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

End Class