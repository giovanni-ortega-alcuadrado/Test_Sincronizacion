Imports System.IO
Imports System.Reflection
Imports System.Web.Mvc
Imports A2.OyD.Infraestructura


Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        Return View()
    End Function

    Function VersionAssembly() As ActionResult
        Dim objListaAssembly As New List(Of clsInformacionAssembly)

        Try
            Dim strRutaDirectorioAssembly As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin")
            Dim objArchivos() As String = Directory.GetFiles(strRutaDirectorioAssembly)
            If Not IsNothing(objArchivos) Then
                For Each Archivo In objArchivos
                    Dim objInfoArchivoCompleta = New FileInfo(Archivo)
                    If objInfoArchivoCompleta.Name.ToLower.Contains("a2") And objInfoArchivoCompleta.Extension.ToLower = ".dll" Then
                        Dim objInfoArchivo As Assembly = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(Archivo))

                        If Not IsNothing(objInfoArchivo) Then
                            objListaAssembly.Add(New clsInformacionAssembly With {.pstrNombreArchivo = objInfoArchivoCompleta.Name,
                                                 .pstrRutaArchivo = Archivo,
                                                 .pstrVersionArchivo = objInfoArchivo.GetName().Version.ToString})
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            ManejarError(ex, "HomeController", "VersionAssembly", False)
        End Try
        Return View(objListaAssembly)
    End Function

    Private Function ObtenerNombreArchivo() As String

    End Function
End Class
