Imports System.ComponentModel
Imports A2Utilidades.Mensajes
Imports System.IO
Imports A2.MC.Core
Imports Microsoft.Win32

Partial Public Class ResultadoGenericoImportaciones
    Inherits Window
    Implements INotifyPropertyChanged

    Public Sub New()
        Try
            InitializeComponent()

            Me.DataContext = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                Me.ToString(), "ImportarArchivoRecibos.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub New(ByVal pstrNombreArchivo As String)
        Try
            NombreArchivo = pstrNombreArchivo

            InitializeComponent()

            Me.DataContext = Me
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

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

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
            If ListaMensajes.Any Then
                'preparo los datos
                Dim objdatos As String = String.Join("#", ListaMensajes)
                Dim objFile As New SaveFileDialog()
                With objFile
                    .FileName = NombreArchivo.Split(CChar(".")).First() & "_Resultado.csv"
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
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al exportar los datos", _
                               Me.ToString(), "cwCargaArchivos.ExportarToCSV", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class
