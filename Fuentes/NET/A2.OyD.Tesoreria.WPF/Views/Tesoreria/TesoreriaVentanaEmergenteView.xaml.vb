Imports Telerik.Windows.Controls
Partial Public Class TesoreriaVentanaEmergenteView
    Inherits Window

    Public Sub New(ByVal pstrTipoTesoreria As String,
                   Optional ByVal plngIDDocumento As Nullable(Of Integer) = Nothing,
                   Optional ByVal pstrNombreConsecutivo As String = "",
                   Optional ByVal pobjTesoreriaEmergente As TesoreriaEmergenteEncabezado = Nothing)
        Try
            InitializeComponent()

            Dim objNuevoRegistro As Object = Nothing

            If Not IsNothing(pstrTipoTesoreria) Then
                If pstrTipoTesoreria.ToUpper = "N" Then
                    objNuevoRegistro = New TesoreriaView(Me, plngIDDocumento, pstrNombreConsecutivo, pobjTesoreriaEmergente)
                    scrollVentanaEmergente.MaxHeight = 470
                    scrollVentanaEmergente.MaxWidth = 940
                ElseIf pstrTipoTesoreria.ToUpper = "CE" Then
                    objNuevoRegistro = New ComprobantesEgresoView(Me, plngIDDocumento, pstrNombreConsecutivo, pobjTesoreriaEmergente)
                    scrollVentanaEmergente.MaxHeight = 555
                    scrollVentanaEmergente.MaxWidth = 1055
                ElseIf pstrTipoTesoreria.ToUpper = "RC" Then
                    objNuevoRegistro = New TesoreriaRecibosView(Me, plngIDDocumento, pstrNombreConsecutivo, pobjTesoreriaEmergente)
                    scrollVentanaEmergente.MaxHeight = 565
                    scrollVentanaEmergente.MaxWidth = 1040
                End If
            End If

            If Not IsNothing(objNuevoRegistro) Then
                If Not IsNothing(Me.gridTesoreria.Children) Then
                    Me.gridTesoreria.Children.Clear()
                End If

                Me.gridTesoreria.Children.Add(objNuevoRegistro)
            End If

            Me.WindowState=WindowState.Maximized
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar la pantalla de Tesorería.", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _DocumentoCreado As Boolean = False
    Public Property DocumentoCreado() As Boolean
        Get
            Return _DocumentoCreado
        End Get
        Set(ByVal value As Boolean)
            _DocumentoCreado = value
        End Set
    End Property

    Private _TipoRegistroActualizado As String
    Public Property TipoRegistroActualizado() As String
        Get
            Return _TipoRegistroActualizado
        End Get
        Set(ByVal value As String)
            _TipoRegistroActualizado = value
        End Set
    End Property

    Private _FechaDocumentoActualizado As Nullable(Of DateTime)
    Public Property FechaDocumentoActualizado() As Nullable(Of DateTime)
        Get
            Return _FechaDocumentoActualizado
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaDocumentoActualizado = value
        End Set
    End Property


    Private _IDDocumentoActualizado As Nullable(Of Integer)
    Public Property IDDocumentoActualizado() As Nullable(Of Integer)
        Get
            Return _IDDocumentoActualizado
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDDocumentoActualizado = value
        End Set
    End Property

    Private _NombreConsecutivoActualizado As String
    Public Property NombreConsecutivoActualizado() As String
        Get
            Return _NombreConsecutivoActualizado
        End Get
        Set(ByVal value As String)
            _NombreConsecutivoActualizado = value
        End Set
    End Property

    Private _ValorTotalActualizado As Double
    Public Property ValorTotalActualizado() As Double
        Get
            Return _ValorTotalActualizado
        End Get
        Set(ByVal value As Double)
            _ValorTotalActualizado = value
        End Set
    End Property

End Class
