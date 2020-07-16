
Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports A2Utilidades.Mensajes
Imports System.IO
Imports A2MCCoreWPF
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones

Partial Public Class ImportarArchivoRecibos
    Inherits Window
    Implements INotifyPropertyChanged

    Dim objViewModelRecibos As OrdenesReciboViewModel_OYDPLUS

    Public Sub New()
        Try
            InitializeComponent()

            Me.DataContext = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                Me.ToString(), "ImportarArchivoRecibos.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub New(ByVal pobjViewModelRecibos As OrdenesReciboViewModel_OYDPLUS, ByVal pstrNombreArchivo As String)
        Try
            objViewModelRecibos = pobjViewModelRecibos
            NombreArchivo = pstrNombreArchivo

            InitializeComponent()

            objViewModelRecibos.ViewImportarArchivo = Me
            Me.DataContext = Me

            objViewModelRecibos.CargarArchivoRecibos(_NombreArchivo)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                Me.ToString(), "ImportarArchivoRecibos.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreArchivo"))
        End Set
    End Property

    Private _ListaMensajes As List(Of String) = New List(Of String)
    Public Property ListaMensajes() As List(Of String)
        Get
            Return _ListaMensajes
        End Get
        Set(ByVal value As List(Of String))
            _ListaMensajes = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaMensajes"))
        End Set
    End Property

    Private _ListaRespuestaImportacion As List(Of RespuestaArchivoImportacion)
    Public Property ListaRespuestaImportacion() As List(Of RespuestaArchivoImportacion)
        Get
            Return _ListaRespuestaImportacion
        End Get
        Set(ByVal value As List(Of RespuestaArchivoImportacion))
            _ListaRespuestaImportacion = value
        End Set
    End Property

    Private _ExportarListaRespuestaImportacion As Boolean = False
    Public Property ExportarListaRespuestaImportacion() As Boolean
        Get
            Return _ExportarListaRespuestaImportacion
        End Get
        Set(ByVal value As Boolean)
            _ExportarListaRespuestaImportacion = value
        End Set
    End Property

    Private _IsBusy As Boolean = False
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Private Sub btnCerrar_Click_1(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

#Region "Exportar"

    Private Sub btnExportar_Click(sender As Object, e As RoutedEventArgs)
        ExportarToCSV()
    End Sub

    ''' <summary>
    ''' Metodo para exportar datos en archivo plano excel 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportarToCSV()
        Try
            If ExportarListaRespuestaImportacion = False Then
                If ListaMensajes.Any Then
                    'preparo los datos
                    Dim objdatos As String = String.Join("#", ListaMensajes)
                    Dim objFile As New SaveFileDialog()
                    With objFile
                        .FileName = NombreArchivo & "_Resultado.csv"
                        .Filter = "Plano CSV | *.csv"
                        .DefaultExt = "csv"
                    End With
                    Dim result As System.Nullable(Of Boolean) = objFile.ShowDialog()
                    If result.HasValue AndAlso result = True Then
                        'guardo el resultado
                        Using fileStream As System.IO.Stream = objFile.OpenFile()
                            Using bookStream As MemoryStream = clsCORE.ExportaDatos(objdatos)
                                bookStream.WriteTo(fileStream)
                            End Using
                        End Using
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No hay datos para exportar", "Se presentó un problema al exportar los datos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                If Not IsNothing(_ListaRespuestaImportacion) Then
                    If _ListaRespuestaImportacion.Count > 0 Then
                        'preparo los datos
                        Dim objdatos As String = String.Empty
                        objdatos = "ID|Tipo|Exitoso|Fila|Columna|Campo|Mensaje"

                        For Each li In _ListaRespuestaImportacion
                            objdatos = String.Format("{0}#{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                                                     objdatos,
                                                     li.ID,
                                                     li.Tipo,
                                                     IIf(li.Exitoso, 1, 0),
                                                     li.Fila,
                                                     li.Columna,
                                                     li.Campo,
                                                     li.Mensaje)
                        Next

                        Dim objFile As New SaveFileDialog()
                        With objFile
                            .FileName = NombreArchivo & "_Resultado.xlsx"
                            .Filter = "Excel 2010 (*.xlsx)|*.xlsx|Excel 2003 (.xls)|*.xls"
                            .DefaultExt = "xlsx"
                        End With
                        Dim result As System.Nullable(Of Boolean) = objFile.ShowDialog()
                        If result.HasValue AndAlso result = True Then
                            'guardo el resultado
                            Using fileStream As System.IO.Stream = objFile.OpenFile()
                                Using bookStream As MemoryStream = clsCORE.ExportaDatos(objdatos, SpreadsheetGear.FileFormat.OpenXMLWorkbook)
                                    bookStream.WriteTo(fileStream)
                                End Using
                            End Using
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No hay datos para exportar", "Se presentó un problema al exportar los datos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No hay datos para exportar", "Se presentó un problema al exportar los datos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al exportar los datos", _
                               Me.ToString(), "cwCargaArchivos.ExportarToCSV", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class
