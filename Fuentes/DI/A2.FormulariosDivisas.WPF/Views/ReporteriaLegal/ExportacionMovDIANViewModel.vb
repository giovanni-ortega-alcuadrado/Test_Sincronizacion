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

Public Class ExportacionMovDIANViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------    
    Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

#End Region

#Region "Constantes"

    ''' <summary>
    ''' JAPC20181009: constante para el nombre del archivo XML DIAN
    ''' </summary>
    Dim strNombreInicial = "Dmuisca_"


#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' JAPC20181009: Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True 'JAPC20181009: Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' JAPC20181009: Inicalización de acceso a datos y carga inicial de datos
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
    ''' JAPC20181009: descripcion del formato para mostrar en pantalla
    ''' </summary>
    Private _strDescripcion As String
    Public Property strDescripcion() As String
        Get
            Return _strDescripcion
        End Get
        Set(ByVal value As String)
            _strDescripcion = value
            CambioItem("strDescripcion")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: codigo OyD seleccionado en el buscador
    ''' </summary>
    Private _strCodigo As String
    Public Property strCodigo() As String
        Get
            Return _strCodigo
        End Get
        Set(ByVal value As String)
            _strCodigo = value
            CambioItem("strCodigo")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: formato legal DIAN seleccionado
    ''' </summary>
    Private _strFormato As String
    Public Property strFormato() As String
        Get
            Return _strFormato
        End Get
        Set(ByVal value As String)
            If Not IsNothing(value) Then
                _strFormato = value
                ActivarVisibilidad(_strFormato)
            End If
            CambioItem("strFormato")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20181009: lista que almacenara la consulta de movimientos formato 1062 DIAN
    ''' </summary>
    Private _lstArchivosLegalesDIAN_Formato1062 As List(Of CPX_ArchivosLegalesDIAN_Formato1062)
    Public Property lstArchivosLegalesDIAN_Formato1062() As List(Of CPX_ArchivosLegalesDIAN_Formato1062)
        Get
            Return _lstArchivosLegalesDIAN_Formato1062
        End Get
        Set(ByVal value As List(Of CPX_ArchivosLegalesDIAN_Formato1062))
            _lstArchivosLegalesDIAN_Formato1062 = value
            CambioItem("lstArchivosLegalesDIAN_Formato1062")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20181009: lista que almacenara la consulta de movimientos formato 1059 DIAN
    ''' </summary>
    Private _lstArchivosLegalesDIAN_Formato1059 As List(Of CPX_ArchivosLegalesDIAN_Formato1059)
    Public Property lstArchivosLegalesDIAN_Formato1059() As List(Of CPX_ArchivosLegalesDIAN_Formato1059)
        Get
            Return _lstArchivosLegalesDIAN_Formato1059
        End Get
        Set(ByVal value As List(Of CPX_ArchivosLegalesDIAN_Formato1059))
            _lstArchivosLegalesDIAN_Formato1059 = value
            CambioItem("lstArchivosLegalesDIAN_Formato1059")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20181009: lista que almacenara la consulta de movimientos formato 1060 DIAN
    ''' </summary>
    Private _lstArchivosLegalesDIAN_Formato1060 As List(Of CPX_ArchivosLegalesDIAN_Formato1060)
    Public Property lstArchivosLegalesDIAN_Formato1060() As List(Of CPX_ArchivosLegalesDIAN_Formato1060)
        Get
            Return _lstArchivosLegalesDIAN_Formato1060
        End Get
        Set(ByVal value As List(Of CPX_ArchivosLegalesDIAN_Formato1060))
            _lstArchivosLegalesDIAN_Formato1060 = value
            CambioItem("lstArchivosLegalesDIAN_Formato1060")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: lista que almacenara la consulta de movimientos formato 1061 DIAN
    ''' </summary>
    Private _lstArchivosLegalesDIAN_Formato1061 As List(Of CPX_ArchivosLegalesDIAN_Formato1061)
    Public Property lstArchivosLegalesDIAN_Formato1061() As List(Of CPX_ArchivosLegalesDIAN_Formato1061)
        Get
            Return _lstArchivosLegalesDIAN_Formato1061
        End Get
        Set(ByVal value As List(Of CPX_ArchivosLegalesDIAN_Formato1061))
            _lstArchivosLegalesDIAN_Formato1061 = value
            CambioItem("lstArchivosLegalesDIAN_Formato1061")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20181009: lista que almacenara la consulta de movimientos formato 1063 DIAN
    ''' </summary>
    Private _lstArchivosLegalesDIAN_Formato1063 As List(Of CPX_ArchivosLegalesDIAN_Formato1063)
    Public Property lstArchivosLegalesDIAN_Formato1063() As List(Of CPX_ArchivosLegalesDIAN_Formato1063)
        Get
            Return _lstArchivosLegalesDIAN_Formato1063
        End Get
        Set(ByVal value As List(Of CPX_ArchivosLegalesDIAN_Formato1063))
            _lstArchivosLegalesDIAN_Formato1063 = value
            CambioItem("lstArchivosLegalesDIAN_Formato1063")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20181009: lista que almacenara la consulta de movimientos formato 1064 DIAN
    ''' </summary>
    Private _lstArchivosLegalesDIAN_Formato1064 As List(Of CPX_ArchivosLegalesDIAN_Formato1064)
    Public Property lstArchivosLegalesDIAN_Formato1064() As List(Of CPX_ArchivosLegalesDIAN_Formato1064)
        Get
            Return _lstArchivosLegalesDIAN_Formato1064
        End Get
        Set(ByVal value As List(Of CPX_ArchivosLegalesDIAN_Formato1064))
            _lstArchivosLegalesDIAN_Formato1064 = value
            CambioItem("lstArchivosLegalesDIAN_Formato1064")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: lista que almacenara el resultado xml de movimientos legales independiente del formato
    ''' </summary>
    Private _lstFormatoXML As List(Of CPX_Formato_XML)
    Public Property lstFormatoXML() As List(Of CPX_Formato_XML)
        Get
            Return _lstFormatoXML
        End Get
        Set(ByVal value As List(Of CPX_Formato_XML))
            _lstFormatoXML = value
            CambioItem("lstFormatoXML")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: lista que almacenara la consulta de todos los formatos legales dian
    ''' </summary>
    Private _FormatosLegal As CPX_FormatosDianXML
    Public Property FormatosLegal() As CPX_FormatosDianXML
        Get
            Return _FormatosLegal
        End Get
        Set(ByVal value As CPX_FormatosDianXML)
            _FormatosLegal = value
            CambioItem("FormatosLegal")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: fecha inicial para la consulta
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
    ''' JAPC20181009: fecha final para la consulta
    ''' </summary>
    Private _dtmFechaFinal As Date = Now
    Public Property dtmFechaFinal() As Date
        Get
            Return _dtmFechaFinal
        End Get
        Set(ByVal value As Date)
            _dtmFechaFinal = value
            CambioItem("dtmFechaFinal")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20181009: propiedad para activar visibilidad del formato seleccionado en este caso el 1062
    ''' </summary>
    Private _logVisible1062 As Visibility = Visibility.Collapsed
    Public Property logVisible1062() As Visibility
        Get
            Return _logVisible1062
        End Get
        Set(ByVal value As Visibility)
            _logVisible1062 = value
            CambioItem("logVisible1062")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20181009: propiedad para activar visibilidad del formato seleccionado en este caso el 1059
    ''' </summary>
    Private _logVisible1059 As Visibility = Visibility.Collapsed
    Public Property logVisible1059() As Visibility
        Get
            Return _logVisible1059
        End Get
        Set(ByVal value As Visibility)
            _logVisible1059 = value
            CambioItem("logVisible1059")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: propiedad para activar visibilidad del formato seleccionado en este caso el 1060
    ''' </summary>
    Private _logVisible1060 As Visibility = Visibility.Collapsed
    Public Property logVisible1060() As Visibility
        Get
            Return _logVisible1060
        End Get
        Set(ByVal value As Visibility)
            _logVisible1060 = value
            CambioItem("logVisible1060")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: propiedad para activar visibilidad del formato seleccionado en este caso el 1061
    ''' </summary>
    Private _logVisible1061 As Visibility = Visibility.Collapsed
    Public Property logVisible1061() As Visibility
        Get
            Return _logVisible1061
        End Get
        Set(ByVal value As Visibility)
            _logVisible1061 = value
            CambioItem("logVisible1061")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: propiedad para activar visibilidad del formato seleccionado en este caso el 1063
    ''' </summary>
    Private _logVisible1063 As Visibility = Visibility.Collapsed
    Public Property logVisible1063() As Visibility
        Get
            Return _logVisible1063
        End Get
        Set(ByVal value As Visibility)
            _logVisible1063 = value
            CambioItem("logVisible1063")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20181009: propiedad para activar visibilidad del formato seleccionado en este caso el 1064
    ''' </summary>
    Private _logVisible1064 As Visibility = Visibility.Collapsed
    Public Property logVisible1064() As Visibility
        Get
            Return _logVisible1064
        End Get
        Set(ByVal value As Visibility)
            _logVisible1064 = value
            CambioItem("logVisible1064")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: propiedad para identificar si se selecciono el check box de buscar por todos
    ''' </summary>
    Private _logTodos As Boolean = True
    Public Property logTodos() As Boolean
        Get
            Return _logTodos
        End Get
        Set(ByVal value As Boolean)
            _logTodos = value
            If _logTodos = True Then
                strCodigo = String.Empty
                logBucador = False
            Else
                logBucador = True
            End If
            CambioItem("logTodos")
            CambioItem("strCodigo")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20181009: propiedad para deshabilitar edicion del buscador personas divisas
    ''' </summary>
    Private _logBucador As Boolean = False
    Public Property logBucador() As Boolean
        Get
            Return _logBucador
        End Get
        Set(ByVal value As Boolean)
            _logBucador = value
            CambioItem("logBucador")
        End Set
    End Property


#End Region


#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' JAPC20181009: metodo para consultar movimientos legales DIAN basado en formato seleccionado y rango entre fechas
    ''' </summary>
    Private Async Sub ConsultarMovimientosLegalesDIAN()
        Try
            IsBusy = True
            Dim objRespuesta = Nothing
            Dim user = Program.Usuario
            If Not IsNothing(strFormato) Then
                If Not IsNothing(dtmFechaInicial) AndAlso Not IsNothing(dtmFechaFinal) Then
                    ActivarVisibilidad(strFormato)
                    Select Case strFormato
                        Case "01062"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1062_ConsultarAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01059"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1059_ConsultarAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01060"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1060_ConsultarAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01061"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1061_ConsultarAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01063"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1063_ConsultarAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01064"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1064_ConsultarAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                    End Select

                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_FORMATODIAN"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Select Case strFormato
                        Case "01062"
                            lstArchivosLegalesDIAN_Formato1062 = objRespuesta.Value
                        Case "01059"
                            lstArchivosLegalesDIAN_Formato1059 = objRespuesta.Value
                        Case "01060"
                            lstArchivosLegalesDIAN_Formato1060 = objRespuesta.Value
                        Case "01061"
                            lstArchivosLegalesDIAN_Formato1061 = objRespuesta.Value
                        Case "01063"
                            lstArchivosLegalesDIAN_Formato1063 = objRespuesta.Value
                        Case "01064"
                            lstArchivosLegalesDIAN_Formato1064 = objRespuesta.Value
                    End Select

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarMovimientosLegalesDIAN", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20181009: metodo para exportar formato seleccionado en formato XML en la ruta seleccionada por control window  modal para ruta y parametros de formato fechas y cliente espoecifico o todos
    ''' </summary>
    ''' <param name="strRuta"></param>
    Private Async Sub ExportarMovimientosLegalesDIANXML(ByVal strRuta As String)
        Try
            IsBusy = True
            Dim newNode As Xml.XmlNode = Nothing
            Dim currNode As Xml.XmlNode = Nothing
            Dim xmlDoc As New Xml.XmlDocument
            Dim xmlDoc1 As New Xml.XmlDocument


            xmlDoc.LoadXml("<?xml version=""1.0"" encoding=""ISO-8859-1""?>" &
                                    "<mas xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""" & strFormato & ".xsd"">" &
                                    "</mas>")


            Dim objRespuesta As InvokeResult(Of List(Of CPX_Formato_XML)) = Nothing
            Dim user = Program.Usuario
            If Not IsNothing(strFormato) Then
                If Not IsNothing(dtmFechaInicial) AndAlso Not IsNothing(dtmFechaFinal) Then
                    Select Case strFormato
                        Case "01062"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1062_GenerarXMLAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01059"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1059_GenerarXMLAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01060"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1060_GenerarXMLAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01061"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1061_GenerarXMLAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01063"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1063_GenerarXMLAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                        Case "01064"
                            objRespuesta = Await mdcProxy.ReporteriaLegal_DIANF1064_GenerarXMLAsync(strCodigo, dtmFechaInicial, dtmFechaFinal, 0, 0, user)
                    End Select

                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_FORMATODIAN"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If



            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    lstFormatoXML = objRespuesta.Value
                End If
            End If


            For Each item In lstFormatoXML
                xmlDoc1.LoadXml(item.strresultado)
                newNode = xmlDoc.ImportNode(xmlDoc1.FirstChild, True)
                xmlDoc.DocumentElement.AppendChild(newNode)
            Next

            xmlDoc.Save(strRuta)

            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_EXPORTACION"), Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ExportarMovimientosLegalesDIANXML", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub


    ''' <summary>
    ''' JAPC20181009: metodo para ejecutar modal savefiledialog para elegir ruta de la exportacion dian baasdo en formato seleccionado fechas y cliente espoecifico o todos con validaciones y creacion del nmbre del archivo dinamicamente
    ''' </summary>
    Private Async Sub ExportarMovimientosLegalesDIAN()
        Try
            If IsNothing(strFormato) Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_FORMATODIAN"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim fileDialog = New System.Windows.Forms.SaveFileDialog()


            Dim objRespuesta As InvokeResult(Of CPX_FormatosDianXML) = Nothing
            Dim user = Program.Usuario
            Dim strAnoEnvio = CStr(Year(Now()))
            'Dim strConsecutivoAno = "0000000"


            objRespuesta = Await mdcProxy.ReporteriaLegal_Formatos_ConsultarAsync(strFormato, user)


            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    FormatosLegal = objRespuesta.Value
                End If
            End If


            fileDialog.FileName = strNombreInicial & FormatosLegal.strConcepto & strFormato & FormatosLegal.strVersionFormato & strAnoEnvio
            fileDialog.DefaultExt = ".xml"
            fileDialog.Filter = "XML Files|*.xml"
            Dim result = fileDialog.ShowDialog()
            Select Case result
                Case System.Windows.Forms.DialogResult.OK
                    Dim strRuta = fileDialog.FileName
                    ExportarMovimientosLegalesDIANXML(strRuta)
                    Exit Select
                Case System.Windows.Forms.DialogResult.Cancel
                    Exit Select
                Case Else
            End Select

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ExportarMovimientosLegalesDIAN", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20181009: metodo para activar visiblidad de radgridview basado en formato seleccionado
    ''' </summary>
    ''' <param name="strFormato"></param>
    Private Sub ActivarVisibilidad(ByVal strFormato As String)
        Try
            logVisible1059 = Visibility.Collapsed
            logVisible1060 = Visibility.Collapsed
            logVisible1061 = Visibility.Collapsed
            logVisible1062 = Visibility.Collapsed
            logVisible1063 = Visibility.Collapsed
            logVisible1064 = Visibility.Collapsed
            Select Case strFormato
                Case "01059"
                    logVisible1059 = Visibility.Visible
                Case "01062"
                    logVisible1062 = Visibility.Visible
                Case "01060"
                    logVisible1060 = Visibility.Visible
                Case "01061"
                    logVisible1061 = Visibility.Visible
                Case "01063"
                    logVisible1063 = Visibility.Visible
                Case "01064"
                    logVisible1064 = Visibility.Visible
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ActivarVisibilidad", Program.TituloSistema, Program.Maquina, ex)
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
                _CargarCmd = New RelayCommand(AddressOf ConsultarMovimientosLegalesDIAN)
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
                _ExportarCmd = New RelayCommand(AddressOf ExportarMovimientosLegalesDIAN)
            End If
            Return _ExportarCmd
        End Get
    End Property

#End Region

End Class
