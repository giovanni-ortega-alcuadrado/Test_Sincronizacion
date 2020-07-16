Imports System.Windows
Imports A2Utilidades
Imports System.Windows.Controls
Imports Microsoft.Win32
Imports System.IO

''' <summary>
''' Clase para realizar validaciones de seguridad sobre archivos importados al sistema
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class A2OpenFileDialog

    Private Const TAMANO_MEGAS_ARCHIVO_IMPORTAR As String = "TAMANO_MEGAS_ARCHIVO_IMPORTAR"
    ''' <summary>
    ''' Tamaño maximo en megas para subir archivos en importaciones
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared ReadOnly Property TamanoArchivoImportar As Integer
        Get
            Dim intArchivo As Integer = 50 ' 50 megas por defecto
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(TAMANO_MEGAS_ARCHIVO_IMPORTAR) Then
                    intArchivo = Convert.ToInt32(CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(TAMANO_MEGAS_ARCHIVO_IMPORTAR).ToString())
                End If
            End If
            Return intArchivo
        End Get
    End Property

    ''' <summary>
    ''' Funcion que retorna el objeto validacion de seguridad en archivo
    ''' </summary>
    ''' <param name="objArchivo">Objeto OpenFileDialog</param>
    ''' <returns>Objeto ValidacionesArchivo</returns>
    ''' <remarks></remarks>
    Public Shared Function EsUnArchivoValido(ByVal objArchivo As OpenFileDialog) As ValidacionesArchivo
        Dim objRespuesta As New ValidacionesArchivo
        Try
            'valido las extensiones si existen filtros
            If objArchivo.Filter.Any() Then
                Dim lstExtensiones As List(Of String) = objArchivo.Filter.Split(Convert.ToChar("|")).Where(Function(item) item.StartsWith("*.")).ToList()
                'no existe *.* en los filtros
                If Not lstExtensiones.Where(Function(item) item.ToUpper() = "*.*").Any Then
                    Dim bitValido As Boolean
                    bitValido = lstExtensiones.Select(Function(item) item.Split(Convert.ToChar(".")).Last.ToUpper).Contains(Path.GetExtension(objArchivo.FileName).Split(Convert.ToChar(".")).Last.ToUpper)
                    If Not bitValido Then
                        objRespuesta.MensajeValidacion = String.Format("El archivo no tiene una extensión valida ({0})", String.Join(",", lstExtensiones))
                        Return objRespuesta
                    End If
                End If
            End If
            'valido el tamaño
            Dim objFileInfo = New FileInfo(objArchivo.FileName)
            Dim intArchivoMegas As Double = objFileInfo.Length / 1000000
            If intArchivoMegas > Convert.ToDouble(TamanoArchivoImportar) Then
                objRespuesta.MensajeValidacion = String.Format("El archivo no se puede procesar ya que supera el tamaño máximo en megas configurado ({0} MB)", TamanoArchivoImportar)
                Return objRespuesta
            End If
            'pasa validaciones
            objRespuesta.EsValido = True
        Catch ex As Exception
            objRespuesta.MensajeValidacion = ex.ToString
        End Try
        Return objRespuesta
    End Function


End Class

Public Class ValidacionesArchivo
    Public Property EsValido As Boolean
    Public Property MensajeValidacion As String
End Class