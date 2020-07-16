Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.WEB
Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.IO

Public Class OrdenesDivisasExportacionSectorRealXMLViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------    
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

#End Region

#Region "Constantes"

    ''' <summary>
    ''' RABP20190627: constante para el nombre del archivo XML 
    ''' </summary>
    Dim strNombreInicial = "trade"
#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' JAPC20181009: Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True 'JAPC20181009: Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' RABP20190627: Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        IsBusy = True
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

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' RABP20190627: lista que almacenara la consulta de las ordenes divisas del sector real '_lstArchivosLegalesDIAN_Formato1062
    ''' </summary>
    Private _lstOrdnesDivisasSectorReal As List(Of CPX_OrdenesDivisasSectorReal)
    Public Property lstOrdnesDivisasSectorReal() As List(Of CPX_OrdenesDivisasSectorReal)
        Get
            Return _lstOrdnesDivisasSectorReal
        End Get
        Set(ByVal value As List(Of CPX_OrdenesDivisasSectorReal))
            _lstOrdnesDivisasSectorReal = value
            CambioItem("lstOrdnesDivisasSectorReal")
        End Set
    End Property

    ''' <summary>
    ''' RABP20190627: lista que almacenara el resultado xml de las ordnes divisas sector real
    ''' </summary>
    Private _lstFormatoXML As ObservableCollection(Of CPX_OrdenesDivisasSectorReal_XML) = New ObservableCollection(Of CPX_OrdenesDivisasSectorReal_XML)
    Public Property lstFormatoXML() As ObservableCollection(Of CPX_OrdenesDivisasSectorReal_XML)
        Get
            Return _lstFormatoXML
        End Get
        Set(ByVal value As ObservableCollection(Of CPX_OrdenesDivisasSectorReal_XML))
            _lstFormatoXML = value
            CambioItem("lstFormatoXML")
        End Set
    End Property

    ''' <summary>
    ''' RABP20190627: lista que almacenara la consulta de todos los formatos legales dian
    ''' </summary>
    Private _FormatosLegal As CPX_OrdenesDivisasSectorReal_XML
    Public Property FormatosLegal() As CPX_OrdenesDivisasSectorReal_XML
        Get
            Return _FormatosLegal
        End Get
        Set(ByVal value As CPX_OrdenesDivisasSectorReal_XML)
            _FormatosLegal = value
            CambioItem("FormatosLegal")
        End Set
    End Property

    ''' <summary>
    ''' RABP20190627: fecha inicial para la consulta
    ''' </summary>
    Private _dtmFecha As Date = Now
    Public Property dtmFecha() As Date
        Get
            Return _dtmFecha
        End Get
        Set(ByVal value As Date)
            _dtmFecha = value
            CambioItem("dtmFecha")
        End Set
    End Property


    ''' <summary>
    ''' RABP20190702: Consecutivo para exportar los datos seleccionados
    ''' </summary>
    Private _IDConsecutivo As Integer
    Public Property IDConsecutivo() As Integer
        Get
            Return _IDConsecutivo
        End Get
        Set(ByVal value As Integer)
            _IDConsecutivo = value
            CambioItem("IDConsecutivo")
        End Set
    End Property

#End Region


#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' JAPC20181009: metodo para consultar movimientos legales DIAN basado en formato seleccionado y rango entre fechas
    ''' </summary>
    Private Async Sub ConsultarOrdenesDivisasSectorReal()
        Try
            IsBusy = True
            Dim objRespuesta = Nothing
            Dim user = Program.Usuario
            If Not IsNothing(dtmFecha) Then

                objRespuesta = Await mdcProxy.OrdenesDivisasSectorReal_ImportarAsync(dtmFecha, user)

            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("ConsultarOrdenesDivisasSectorReal"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    lstOrdnesDivisasSectorReal = objRespuesta.Value

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarMovimientosLegalesDIAN", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub


    ''' <summary>
    ''' RABP20190627: metodo para exportar ordenes divisas del sector real en formato XML en la ruta seleccionada por control window  modal para ruta y parametros de formato fechas.
    ''' </summary>
    ''' <param name="strRuta"></param>
    Private Async Sub ExportarOrdnesDivisasSectorRealXML(ByVal strRuta As String)
        Try
            IsBusy = True
            Dim xmlDoc As New Xml.XmlDocument
            Dim xmlstr As String

            Dim objRespuesta As InvokeResult(Of List(Of CPX_OrdenesDivisasSectorReal_XML)) = Nothing


            For Each objRegistro In lstOrdnesDivisasSectorReal
                If objRegistro.Enviar = True Then ' Esta seleccionado para ser importado

                    IDConsecutivo = objRegistro.id


                    Dim user = Program.Usuario
                    If Not IsNothing(dtmFecha) Then
                        objRespuesta = Await mdcProxy.OrdenesDivisasSectorReal_Importar_XMLAsync(dtmFecha, user, IDConsecutivo)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_EXPORTARXML"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    End If


                    If Not IsNothing(objRespuesta) Then
                        If Not IsNothing(objRespuesta.Value) Then
                            lstFormatoXML.Add(objRespuesta.Value.FirstOrDefault)
                        End If
                    End If
                End If
            Next


            xmlstr = "<transacciones>"

            For Each item In lstFormatoXML
                xmlstr = xmlstr + item.strresultado
            Next

            xmlstr = xmlstr + "</transacciones>"

            xmlDoc.LoadXml(xmlstr)

            xmlDoc.Save(strRuta)

            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_EXPORTACION"), Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ExportarOrdnesDivisasSectorRealXML", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub


    ''' <summary>
    ''' RABP20190627: metodo para ejecutar modal savefiledialog para elegir ruta de la exportacion de ordenes divisas del sector real.
    ''' </summary>
    Private Sub ExportarOrdenesDivisasSectorReal()
        Try

            Dim fileDialog = New System.Windows.Forms.SaveFileDialog()


            Dim objRespuesta As InvokeResult(Of CPX_OrdenesDivisasSectorReal) = Nothing
            Dim user = Program.Usuario
            Dim strAnoEnvio = CStr(Year(Now()))
            Dim dtmFecha = Now()


            fileDialog.FileName = strNombreInicial
            fileDialog.DefaultExt = ".xml"
            fileDialog.Filter = "XML Files|*.xml"
            Dim result = fileDialog.ShowDialog()
            Select Case result
                Case System.Windows.Forms.DialogResult.OK
                    Dim strRuta = fileDialog.FileName
                    ExportarOrdnesDivisasSectorRealXML(strRuta)
                    Exit Select
                Case System.Windows.Forms.DialogResult.Cancel
                    Exit Select
                Case Else
            End Select

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ExportarMovimientosLegalesDIAN", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region


#Region "Comandos"

    ''' <summary>
    ''' JAPC20181009: metodo relaycommand respuesta cuando se da click a boton Consultar
    ''' </summary>
    Private WithEvents _CargarCmd As RelayCommand
    Public ReadOnly Property CargarCmd() As RelayCommand
        Get
            If _CargarCmd Is Nothing Then
                _CargarCmd = New RelayCommand(AddressOf ConsultarOrdenesDivisasSectorReal)
            End If
            Return _CargarCmd
        End Get
    End Property


    ''' <summary>
    ''' JAPC20181009: metodo relaycommand respuesta cuando se da click a boton Exportar
    ''' </summary>
    Private WithEvents _ExportarCmd As RelayCommand
    Public ReadOnly Property ExportarCmd() As RelayCommand
        Get
            If _ExportarCmd Is Nothing Then
                _ExportarCmd = New RelayCommand(AddressOf ExportarOrdenesDivisasSectorReal)
            End If
            Return _ExportarCmd
        End Get
    End Property

#End Region

End Class
