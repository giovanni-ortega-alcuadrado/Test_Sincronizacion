Imports System.IO
Imports System.Net
Imports System.Web
Imports Microsoft.Win32

Partial Public Class ucDescargarArchivo

    Private Shared ReadOnly _strURLArchivo As DependencyProperty = DependencyProperty.Register("URLArchivo", GetType(String), GetType(ucDescargarArchivo), New PropertyMetadata(""))
    Public Property URLArchivo As String
        Get
            Return CStr(GetValue(_strURLArchivo))
        End Get
        Set(ByVal value As String)
            SetValue(_strURLArchivo, value)
        End Set
    End Property

    Private Shared ReadOnly _strNombreArchivo As DependencyProperty = DependencyProperty.Register("NombreArchivo", GetType(String), GetType(ucDescargarArchivo), New PropertyMetadata(""))
    Public Property NombreArchivo As String
        Get
            Return CStr(GetValue(_strNombreArchivo))
        End Get
        Set(ByVal value As String)
            SetValue(_strNombreArchivo, value)
        End Set
    End Property

    Private Shared ReadOnly _strExtensionArchivo As DependencyProperty = DependencyProperty.Register("ExtensionArchivo", GetType(String), GetType(ucDescargarArchivo), New PropertyMetadata(""))
    Public Property ExtensionArchivo As String
        Get
            Return CStr(GetValue(_strExtensionArchivo))
        End Get
        Set(ByVal value As String)
            SetValue(_strExtensionArchivo, value)
        End Set
    End Property

    Private Shared ReadOnly _ColocarNombreArchivoDefecto As DependencyProperty = DependencyProperty.Register("ColocarNombreArchivoDefecto", GetType(Boolean), GetType(ucDescargarArchivo), New PropertyMetadata(False))
    Public Property ColocarNombreArchivoDefecto As Boolean
        Get
            Return CBool(GetValue(_ColocarNombreArchivoDefecto))
        End Get
        Set(ByVal value As Boolean)
            SetValue(_ColocarNombreArchivoDefecto, value)
        End Set
    End Property

    Private Shared ReadOnly _DescargarTXTPorNavegador As DependencyProperty = DependencyProperty.Register("DescargarTXTPorNavegador", GetType(Boolean), GetType(ucDescargarArchivo), New PropertyMetadata(False))
    Public Property DescargarTXTPorNavegador As Boolean
        Get
            Return CBool(GetValue(_DescargarTXTPorNavegador))
        End Get
        Set(ByVal value As Boolean)
            SetValue(_DescargarTXTPorNavegador, value)
        End Set
    End Property

    Dim objFileDialog As New SaveFileDialog()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub ucDescargarArchivo_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If Not Program.IsDesignMode() Then
                txtHipervinculo.Content = NombreArchivo
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el control.", Me.ToString(), "ucDescargarArchivo_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtHipervinculo_Click_1(sender As Object, e As RoutedEventArgs)
        DescargarArchivo(URLArchivo, NombreArchivo, ExtensionArchivo)
    End Sub

    Public Sub DescargarArchivo(ByVal pstrURLArchivo As String, ByVal pstrNombreArchivo As String, ByVal pstrExtensionArchivo As String)
        Try
            If pstrExtensionArchivo.Contains(".") Then
                pstrExtensionArchivo = pstrExtensionArchivo.Replace(".", "")
            End If

            pstrExtensionArchivo = pstrExtensionArchivo.ToLower

            If pstrExtensionArchivo = "txt" And DescargarTXTPorNavegador = False Then
                If ColocarNombreArchivoDefecto Then
                    If Not IsNothing(pstrNombreArchivo) Then
                        objFileDialog.FileName = pstrNombreArchivo
                        'objFileDialog.DefaultFileName = pstrNombreArchivo
                    End If
                End If

                objFileDialog.DefaultExt = pstrExtensionArchivo
                objFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

                If CType(objFileDialog.ShowDialog(), Boolean) Then
                    Dim webClient As New WebClient()
                    AddHandler webClient.OpenReadCompleted, AddressOf DescargaArchivoCompleta
                    webClient.Encoding = System.Text.Encoding.UTF8
                    webClient.OpenReadAsync(New Uri(pstrURLArchivo, UriKind.Absolute))
                End If
            Else
                Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString

                Program.VisorArchivosWeb_DescargarURL(pstrURLArchivo)
                'If (Application.Current.IsRunningOutOfBrowser) Then
                '    'para que cargue el visor de reportes para cuando la consola se ejecuta por fuera del browser
                '    Dim button As New MyHyperlinkButton
                '    button.NavigateUri = New Uri(pstrURLArchivo)
                '    button.TargetName = "vtnNva" & "00" & strNroVentana
                '    button.ClickMe()
                'Else
                '    If pstrExtensionArchivo = "txt" Then
                '        HtmlPage.Window.Navigate(New Uri(pstrURLArchivo), "_blank")
                '    Else
                '        HtmlPage.Window.Navigate(New Uri(pstrURLArchivo), "vtnNva" & "00" & strNroVentana, "height=550,width=750,top=25,left=25,toolbar=1,resizable=1")
                '    End If
                'End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al descargar el archivo.", "ucDescargarArchivo", "DescargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub DescargaArchivoCompleta(sender As Object, e As OpenReadCompletedEventArgs)
        Try
            If IsNothing(e.Error) Then
                Dim sr As New StreamReader(e.Result)
                Dim str As String = sr.ReadToEnd()
                Dim sw As New StreamWriter(objFileDialog.OpenFile())
                sw.Write(str)
                sw.Close()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de descargar el archivo.", "clsDescargarArchivo", "DescargaArchivoCompleta", Program.TituloSistema, Program.Maquina, e.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de descargar el archivo.", "clsDescargarArchivo", "DescargaArchivoCompleta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
