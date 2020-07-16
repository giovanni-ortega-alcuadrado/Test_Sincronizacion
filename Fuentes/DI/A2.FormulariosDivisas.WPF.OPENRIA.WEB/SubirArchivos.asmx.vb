Imports System.IO
Imports A2Utilidades
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web
Imports System.Web.Script.Services

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SubirArchivos
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    <ScriptMethod(UseHttpGet:=True)> _
    Public Function Subir(nomarchivo As String, usuario As String, proceso As String) As String




        'Nombre del archivo 
        Dim fileName As String = String.Empty
        Dim pstrNombreProceso As String = String.Empty
        Dim strUsuario As String = String.Empty

        Try

            If HttpContext.Current.Request.QueryString("nomarchivo") IsNot Nothing Then
                fileName = HttpContext.Current.Request.QueryString("nomarchivo")
            Else
                Return ""
            End If

            If HttpContext.Current.Request.QueryString("proceso") IsNot Nothing Then
                pstrNombreProceso = HttpContext.Current.Request.QueryString("proceso")
            Else
                Return ""
            End If

            If HttpContext.Current.Request.QueryString("usuario") IsNot Nothing Then
                strUsuario = HttpContext.Current.Request.QueryString("usuario")
            Else
                Return ""
            End If

            Dim objDatosRutas = clsClaseUploads.FnRutasImportaciones(pstrNombreProceso, strUsuario)

            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal

            If Not Directory.Exists(objDatosRutas.RutaArchivosLocal) Then
                Directory.CreateDirectory(objDatosRutas.RutaArchivosLocal)
            End If

            Dim strRutaCompleta As String = objDatosRutas.RutaArchivosLocal & "\" & fileName

            If Not Directory.Exists(objDatosRutas.RutaArchivosLocal) Then
                Directory.CreateDirectory(objDatosRutas.RutaArchivosLocal)
            End If

            Dim inputStream As Stream = HttpContext.Current.Request.InputStream
            Dim bytes As Byte() = New Byte(inputStream.Length - 1) {}
            'Guarda el archivo en el fólder ClientBin  
            Dim sw As New StreamWriter(strRutaCompleta)
            Dim bw As New BinaryWriter(sw.BaseStream)
            inputStream.Read(bytes, 0, bytes.Length)
            bw.Write(bytes)
            bw.Flush()
            bw.Close()

            Return strRutaCompleta

        Catch generatedExceptionName As Exception
            Throw
        End Try

    End Function

End Class