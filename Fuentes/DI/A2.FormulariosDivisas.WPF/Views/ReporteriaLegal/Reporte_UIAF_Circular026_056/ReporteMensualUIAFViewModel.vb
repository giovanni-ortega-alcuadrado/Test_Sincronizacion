Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.IO

Public Class ReporteMensualUIAFViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------    
    Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' RABP20181017: Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True 'RABP20181017: Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' RABP20181017: Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function
#End Region

#Region "Propiedades"

    ''' <summary>
    ''' RABP20181017: fecha inicial para la consulta
    ''' </summary>
    Private _dtmFechaInicial As Date = Now
    Public Property dtmFechaInicial() As Date
        Get
            Return _dtmFechaInicial
        End Get
        Set(ByVal value As Date)
            _dtmFechaInicial = value
            CambioItem("dtmFechaInicial")
        End Set
    End Property

    ''' <summary>
    ''' RABP20181022: Para capturar la ruta de la exportación del archivo
    ''' </summary>
    Private _Topicoselected As ProductoCombos
    Public Property Topicoselected() As ProductoCombos
        Get
            Return _Topicoselected
        End Get
        Set(ByVal value As ProductoCombos)
            _Topicoselected = value
            CambioItem("Topicoselected")
        End Set
    End Property

    Private _lstTopico As List(Of ProductoCombos)
    Public Property lstTopico() As List(Of ProductoCombos)
        Get
            Return _lstTopico
        End Get
        Set(ByVal value As List(Of ProductoCombos))
            _lstTopico = value
            CambioItem("lstTopico")
        End Set
    End Property

    ''' <summary>
    ''' RABP20181017: lista que almacenara la consulta de reporte UIAF
    ''' </summary>
    Private _lstReporte_UAIF As List(Of CPX_ReporteUIAF)
    Public Property lstReporte_UAIF() As List(Of CPX_ReporteUIAF)
        Get
            Return _lstReporte_UAIF
        End Get
        Set(ByVal value As List(Of CPX_ReporteUIAF))
            _lstReporte_UAIF = value
            CambioItem("lstReporte_UAIF")
        End Set
    End Property

    ''' <summary>
    ''' RABP20181017: lista que almacenara el resultado xml de movimientos legales independiente del formato
    ''' </summary>
    Private _lstReporteUIAF_Exportar As List(Of CPX_ReporteUIAF)
    Public Property lstReporteUIAF_Exportar() As List(Of CPX_ReporteUIAF)
        Get
            Return _lstReporteUIAF_Exportar
        End Get
        Set(ByVal value As List(Of CPX_ReporteUIAF))
            _lstReporteUIAF_Exportar = value
            CambioItem("lstReporteUIAF_Exportar")
        End Set
    End Property

#End Region
#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' RABP20181017: metodo para consultar las compras y ventas para el formato UIAF
    ''' </summary>
    Private Async Sub ConsultarMovimientosReporteUIAF()
        Try
            IsBusy = True
            Dim objRespuesta = Nothing
            Dim user = Program.Usuario

            If Not IsNothing(dtmFechaInicial) Then
                objRespuesta = Await mdcProxy.ReporteriaLegal_MensualCompraVentaUIAF_ConsultarAsync(dtmFechaInicial, user)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    lstReporte_UAIF = objRespuesta.Value
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarMovimientosReporteUIAF", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub


    ''' <summary>
    ''' RABP20181017: metodo para exportar los datos a un archivo .txt reporte UIAF
    ''' </summary>
    Private Async Sub ExportarReporteUIAF()
        Try

            Dim logExportarArchivo As Boolean = False
            IsBusy = True

            Dim user = Program.Usuario

            If Not IsNothing(dtmFechaInicial) Then
                Dim objretorno = Await mdcProxy.ReporteriaLegal_MensualCompraVentaUIAF_ExportarAsync(dtmFechaInicial, IsNothing(pstrRuta), user, "")

                If Not IsNothing(objretorno) Then
                    If Not IsNothing(objretorno.Value) Then
                        If objretorno.Value.Count > 0 Then
                            If objretorno.Value.First.Exitoso Then
                                A2Utilidades.Mensajes.mostrarMensaje(objretorno.Value.First.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje(objretorno.Value.First.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If
            End If


            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_EXPORTACION"), Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ExportarReporteUIAF", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub


#End Region


#Region "Comandos"

    ''' <summary>
    ''' RABP20181017: metodo relaycommand respuesta cuando se da click a boton Consultar
    ''' </summary>
    Private WithEvents _CargarCmd As RelayCommand
    Public ReadOnly Property CargarCmd() As RelayCommand
        Get
            If _CargarCmd Is Nothing Then
                _CargarCmd = New RelayCommand(AddressOf ConsultarMovimientosReporteUIAF)
            End If
            Return _CargarCmd
        End Get
    End Property


    ''' <summary>
    ''' RABP20181017: metodo relaycommand respuesta cuando se da click a boton Exportar
    ''' </summary>
    Private WithEvents _ExportarCmd As RelayCommand
    Private pstrRuta As Object

    Public ReadOnly Property ExportarCmd() As RelayCommand
        Get
            If _ExportarCmd Is Nothing Then
                _ExportarCmd = New RelayCommand(AddressOf ExportarReporteUIAF)
            End If
            Return _ExportarCmd
        End Get
    End Property

#End Region

End Class
