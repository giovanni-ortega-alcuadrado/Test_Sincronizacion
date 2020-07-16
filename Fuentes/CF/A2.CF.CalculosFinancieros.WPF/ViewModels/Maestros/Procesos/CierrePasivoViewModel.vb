
Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports System.Collections.ObjectModel
Imports SpreadsheetGear
Imports System.IO
Imports A2MCCoreWPF
Imports GalaSoft.MvvmLight.Threading
Imports A2.Notificaciones.Cliente
Imports A2.OyD.OYDServer.RIA
Imports A2.OyD.OYDServer.RIA.Web.CFUtilidades
Imports A2CFUtilitarios


Public Class CierrePasivoViewModel
    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged

    Dim dcProxy As CalculosFinancierosDomainContext
    Private mintLapsoRecargaAutomaticaProcesarPortafolio As Integer
    Private mstrHabilitarRecargaAutomaticaProcesarPortafolio As String

    Private logEstaValorando As Boolean

    ''' <summary>
    ''' ViewModel para la pantalla Cierre Pasivo perteneciente al proyecto de Cálculos Financieros.
    ''' </summary>
    ''' <history>
    ''' Creado por       :  (Alcuadrado S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : Marzo 04/2016
    ''' Pruebas CB       : 
    ''' </history>


#Region "Constantes"

#End Region

#Region "Variables - REQUERIDO"


    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxyUtilidades As UtilidadesDomainContext
#End Region

#Region "Inicialización - REQUERIDO"

    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        'OJO VALIDAR ESTOS ESTADOS CON EL FLUJO DE LA PANTALLA Y CARGA DE DATOS
        HabilitarBotonInforme = True
        HabilitarBotonProcesar = True
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : 
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 4/2016
    ''' Pruebas CB       : 
    ''' </history>
    Public Function inicializar() As Boolean
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                ConsultarValoresPorDefecto()
                ConsultarParametros()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _dtmFechaProceso As System.Nullable(Of System.DateTime)
    Public Property dtmFechaProceso() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaProceso
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaProceso = value
            If Not String.IsNullOrEmpty(strTipoCompania) Then
                BuscarCompanias()
            End If
            dtmFechaFinProceso = _dtmFechaProceso
            MyBase.CambioItem("dtmFechaProceso")
            MyBase.CambioItem("dtmFechaFinProceso")
        End Set
    End Property

    Private _dtmFechaFinProceso As System.Nullable(Of System.DateTime)
    Public Property dtmFechaFinProceso As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaFinProceso
        End Get
        Set(value As System.Nullable(Of System.DateTime))
            _dtmFechaFinProceso = value
            MyBase.CambioItem("dtmFechaFinProceso")
        End Set
    End Property


    Private _logCierreContinuo As Boolean
    Public Property logCierreContinuo() As Boolean
        Get
            Return _logCierreContinuo
        End Get
        Set(ByVal value As Boolean)
            _logCierreContinuo = value
            MyBase.CambioItem("logCierreContinuo")
        End Set
    End Property

    Private _logPermiteCierreContinuo As Boolean
    Public Property logPermiteCierreContinuo() As Boolean
        Get
            Return _logPermiteCierreContinuo
        End Get
        Set(ByVal value As Boolean)
            _logPermiteCierreContinuo = value
            MyBase.CambioItem("logPermiteCierreContinuo")
        End Set
    End Property

    Private _IsBusyCompanias As Boolean
    Public Property IsBusyCompanias() As Boolean
        Get
            Return _IsBusyCompanias
        End Get
        Set(ByVal value As Boolean)
            _IsBusyCompanias = value
            MyBase.CambioItem("IsBusyCompanias")
        End Set
    End Property


    Private _ListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoCompania
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoCompania = value
            MyBase.CambioItem("ListaTipoCompania")
        End Set
    End Property

    Private _strTipoCompania As String = String.Empty
    Public Property strTipoCompania() As String
        Get
            Return _strTipoCompania
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) AndAlso value <> _strTipoCompania Then
                BuscarCompanias()
            End If
            _strTipoCompania = value
            MyBase.CambioItem("strTipoCompania")
        End Set
    End Property

    Private _ListaCompanias As New ObservableCollection(Of OYDUtilidades.ItemCombo)
    Public Property ListaCompanias As ObservableCollection(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaCompanias
        End Get
        Set(ByVal value As ObservableCollection(Of OYDUtilidades.ItemCombo))
            _ListaCompanias = value
            MyBase.CambioItem("ListaCompanias")
        End Set
    End Property


    Private _ListaCompaniasSeleccionadas As New ObservableCollection(Of OYDUtilidades.ItemCombo)
    Public Property ListaCompaniasSeleccionadas As ObservableCollection(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaCompaniasSeleccionadas
        End Get
        Set(ByVal value As ObservableCollection(Of OYDUtilidades.ItemCombo))
            _ListaCompaniasSeleccionadas = value
            MyBase.CambioItem("ListaCompaniasSeleccionadas")
        End Set
    End Property

    Private _strTipoProceso As String = String.Empty
    Public Property strTipoProceso() As String
        Get
            Return _strTipoProceso
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) Then
                If value = "CUR" Then
                    logPermiteCierreContinuo = True
                Else
                    logPermiteCierreContinuo = False
                End If
            Else
                logPermiteCierreContinuo = False
            End If
            _strTipoProceso = value
            MyBase.CambioItem("strTipoProceso")
        End Set
    End Property

    Private _IndexTipoProceso As Integer
    Public Property IndexTipoProceso() As Integer
        Get
            Return _IndexTipoProceso
        End Get
        Set(ByVal value As Integer)
            _IndexTipoProceso = value
            MyBase.CambioItem("IndexTipoProceso")
        End Set
    End Property

    Private _IsBusyProcesando As Boolean = False
    Public Property IsBusyProcesando() As Boolean
        Get
            Return _IsBusyProcesando
        End Get
        Set(ByVal value As Boolean)
            _IsBusyProcesando = value
            MyBase.CambioItem("IsBusyProcesando")
        End Set
    End Property

    Private _HabilitarBotonProcesar As Boolean = True
    Public Property HabilitarBotonProcesar() As Boolean
        Get
            Return _HabilitarBotonProcesar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonProcesar = value
            MyBase.CambioItem("HabilitarBotonProcesar")
        End Set
    End Property

    Private _HabilitarBotonInforme As Boolean = True
    Public Property HabilitarBotonInforme() As Boolean
        Get
            Return _HabilitarBotonInforme
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonInforme = value
            MyBase.CambioItem("HabilitarBotonInforme")
        End Set
    End Property

    ''' <summary>
    ''' Lista de ProcesarPortafolio que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaAvanceProcesoCierrePortafolio As EntitySet(Of CFCalculosFinancieros.ProcesarPortafolio)
    Public Property ListaAvanceProcesoCierrePortafolio() As EntitySet(Of CFCalculosFinancieros.ProcesarPortafolio)
        Get
            Return _ListaAvanceProcesoCierrePortafolio
        End Get
        Set(ByVal value As EntitySet(Of CFCalculosFinancieros.ProcesarPortafolio))
            _ListaAvanceProcesoCierrePortafolioPaginada = Nothing
            _ListaAvanceProcesoCierrePortafolio = value

            MyBase.CambioItem("ListaAvanceProcesoCierrePortafolio")
            MyBase.CambioItem("ListaAvanceProcesoCierrePortafolioPaginada")
        End Set
    End Property

    Private _ListaAvanceProcesoCierrePortafolioPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de ProcesarPortafolio para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaAvanceProcesoCierrePortafolioPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaAvanceProcesoCierrePortafolio) Then
                If IsNothing(_ListaAvanceProcesoCierrePortafolioPaginada) Then
                    Dim view = New PagedCollectionView(_ListaAvanceProcesoCierrePortafolio)
                    _ListaAvanceProcesoCierrePortafolioPaginada = view
                    Return view
                Else
                    Return (_ListaAvanceProcesoCierrePortafolioPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _Encabezado As ValoracionFondo
    Public Property Encabezado() As ValoracionFondo
        Get
            Return _Encabezado
        End Get
        Set(ByVal value As ValoracionFondo)
            _Encabezado = value
            MyBase.CambioItem("Encabezado")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"



#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"


    ' ''' <summary>
    ' ''' Consulta los valores por defecto para un nuevo encabezado
    ' ''' </summary>
    Private Sub ConsultarValoresPorDefecto()
        Try
            'TODO: PENDIENTE OBTENER EL DIA HABIL PARA LA FECHA DE PROCESO
            dtmFechaProceso = Date.Now()
            dtmFechaFinProceso = Date.Now()
            IndexTipoProceso = 1 'por dfecto simulacion
            'CargaListas()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarValoresPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            'Valida la fecha de valoración
            If IsNothing(dtmFechaProceso) Then
                strMsg = String.Format("{0}{1} + La fecha del proceso es un campo requerido.", strMsg, vbCrLf)
            End If

            'Verificar que se escoja un producto
            If ListaCompaniasSeleccionadas.Count <= 0 Then
                strMsg = String.Format("{0}{1} + El producto es un campo requerido.", strMsg, vbCrLf)
            End If

            'Verificar que la Fecha inicial no sea mayor que la fecha final
            If dtmFechaProceso > dtmFechaFinProceso Then
                strMsg = String.Format("{0}{1} + La Fecha inicial no puede ser mayor que la fecha final.", strMsg, vbCrLf)
            End If

            'Verificar que se escoja un tipo de compañia
            If strTipoCompania = String.Empty Then
                strMsg = String.Format("{0}{1} + Debe selecionar un tipo de producto.", strMsg, vbCrLf)
            End If

            'Verificar que se escoja un tipo de proceso
            If strTipoProceso = String.Empty Then
                strMsg = String.Format("{0}{1} + Debe selecionar un tipo de proceso.", strMsg, vbCrLf)
            End If


            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                IsBusy = False
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Método para realizar la valoración/cierre pasivo
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ProcesarCierrePasivo()
        If Not mdcProxy Is Nothing AndAlso Not mdcProxy.ProcesarPortafolios Is Nothing Then
            mdcProxy.ProcesarPortafolios.Clear()
            ListaAvanceProcesoCierrePortafolio = mdcProxy.ProcesarPortafolios
        End If
        If strTipoProceso = "SR" Then
        EjecutarCierrePasivo(True)
        Else
        EjecutarCierrePasivo(False)
        End If
    End Sub

    ''' <summary>
    ''' Método para realizar la valoración/cierre pasivo
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConfirmarCierrePasivo(ByVal sender As Object, ByVal e As EventArgs)
        If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
            EjecutarCierrePasivo(True)
        End If
    End Sub

    ''' <summary>
    ''' Método para realizar la valoración/cierre pasivo
    ''' </summary>
    ''' <remarks></remarks>
    Private Async Sub EjecutarCierrePasivo(confirmado As Boolean)
        Try

            Dim objRet As InvokeOperation(Of String)
            Dim strMsg As String = String.Empty


            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
                DirectCast(mdcProxy.DomainClient, WebDomainClient(Of CalculosFinancierosDomainContext.ICalculosFinancierosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)
            End If

            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ValidarDatos() Then

                If strTipoProceso = "CUR" And confirmado = False Then
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Está seguro de realizar el cierre pasivo de los productos seleccionados, esta operación puede tardar varios minutos, ¿Desea continuar?", Program.TituloSistema, ValoresUserState.Actualizar.ToString, AddressOf ConfirmarCierrePasivo, False)
                    Exit Sub
                End If


                'Se habilita el IsBusyProcesando y se desahabilitan los botones de eliminar y del informe SM20150918
                IsBusyProcesando = True

                Dim xmlCompleto As String
                Dim xmlCompletoCodigos As String


                'Construir el XML de las compañías
                xmlCompleto = "<Compania>"
                If Not IsNothing(ListaCompaniasSeleccionadas) Then
                    If ListaCompaniasSeleccionadas.Count > 0 Then
                        For Each objeto In (From c In ListaCompaniasSeleccionadas)
                            xmlCompletoCodigos = "<Detalle intIDCompania=""" & objeto.ID &
                            """ logEvaluar=""" & 1 & """></Detalle>"
                            xmlCompleto = xmlCompleto & xmlCompletoCodigos


                        Next
                    End If
                End If
                xmlCompleto = xmlCompleto & "</Compania>"

                'inicio el timer para consultar el avance del proceso
                logEstaValorando = True
                ReiniciaTimer()

                'Incia el proceso de valoracion
                objRet = Await mdcProxy.ActualizarValoracionFondo(strTipoProceso, CDate(dtmFechaProceso), CDate(dtmFechaFinProceso), logCierreContinuo, xmlCompleto, Program.Usuario, Program.HashConexion).AsTask()

                logEstaValorando = False
                'consulto el avance proceso
                Await RecargarInformeProcesamiento()

                IsBusyProcesando = False
                strMsg = objRet.Value.ToString()

                If Not String.IsNullOrEmpty(strMsg) Then
                    If strMsg.StartsWith("OK|") Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg.Replace("OK|", ""), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        strTipoCompania = String.Empty
                        ListaCompanias = New ObservableCollection(Of OYDUtilidades.ItemCombo)()
                        ListaCompaniasSeleccionadas = New ObservableCollection(Of OYDUtilidades.ItemCombo)()
                    Else
                        strMsg = String.Format("{0}{1}", vbCrLf, strMsg)
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If


            End If
            IsBusy = False

        Catch ex As Exception
            logEstaValorando = False
            IsBusy = False
            IsBusyProcesando = False
            CerrarTemporizador()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al procesar la valoración", Me.ToString(), "EjecutarCierrePasivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Realiza la exportación de los resultados de valoración a Excel.
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Sub ExportarInformeValoracion()
        Try
            IsBusy = True



            Try
                If strTipoProceso = "SR" Then

                    If Await EjecutarScriptCierrePasivo(CDate(dtmFechaProceso)) Then




                        'preparo los datos
                        Dim lstData As New List(Of String) From {"Informe exportado correctamente!"}
                        Dim objdatos As String = String.Join("#", lstData)
                        'Dim objFile As New SaveFileDialog()
                        'With objFile
                        '    .FileName = "CierrePasivo_InformeValoracion_Resultado.xlsx"
                        '    .Filter = "Excel| *.xlsx"
                        '    .DefaultExt = "xlsx"
                        'End With
                        'Dim result As System.Nullable(Of Boolean) = objFile.ShowDialog()
                        'If result.HasValue AndAlso result = True Then
                        '    'guardo el resultado
                        '    Using fileStream As System.IO.Stream = objFile.OpenFile()
                        '        Using bookStream As MemoryStream = clsCORE.ExportaDatos(objdatos, FileFormat.OpenXMLWorkbook)
                        '            bookStream.WriteTo(fileStream)
                        '        End Using
                        '    End Using
                        'End If

                    End If
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al exportar los datos", _
                                   Me.ToString(), "CierrePasivoViewModel.ExportarInformeValoracion", Application.Current.ToString(), Program.Maquina, ex)
            End Try

            'Dim objRet As LoadOperation(Of GenerarArchivosPlanos)
            'Dim dcProxyUtil As UtilidadesDomainContext
            'Dim strParametros As String = STR_PARAMETROS_EXPORTAR
            'Dim dia As String = String.Empty
            'Dim mes As String = String.Empty
            'Dim ano As String = String.Empty
            'Dim strFechaValoracion As String = String.Empty

            'If Not ValidarDatos() Then Exit Sub

            'dcProxyUtil = inicializarProxyUtilidadesOYD()


            'If (_dtmFechaValoracion.Value.Day < 10) Then
            '    dia = "0" + _dtmFechaValoracion.Value.Day.ToString
            'Else
            '    dia = _dtmFechaValoracion.Value.Day.ToString
            'End If

            'If (_dtmFechaValoracion.Value.Month < 10) Then
            '    mes = "0" + _dtmFechaValoracion.Value.Month.ToString
            'Else
            '    mes = _dtmFechaValoracion.Value.Month.ToString
            'End If

            'ano = _dtmFechaValoracion.Value.Year.ToString

            'strFechaValoracion = ano + mes + dia

            'strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHAPROCESO), strFechaValoracion)
            'strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.TIPOPROCESO), strTipoProceso)

            'objRet = Await dcProxyUtil.Load(dcProxyUtil.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_PROCESO, strParametros, "InformeValoracion", "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

            'If Not objRet Is Nothing Then
            '    If objRet.HasError Then
            '        If objRet.Error Is Nothing Then
            '            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
            '        Else
            '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "ExportarInformeValoracion", Program.TituloSistema, Program.Maquina, objRet.Error)
            '        End If
            '        IsBusy = False
            '        objRet.MarkErrorAsHandled()
            '    Else
            '        If objRet.Entities.Count > 0 Then
            '            Dim objResultado = objRet.Entities.First

            '            If objResultado.Exitoso Then
            '                If (Application.Current.IsRunningOutOfBrowser) Then
            '                    Dim button As New MyHyperlinkButton
            '                    button.NavigateUri = New Uri(objResultado.RutaArchivoPlano & "?date=" & strFechaValoracion & DateTime.Now.ToString("HH:mm:ss"))
            '                    button.TargetName = "_blank"
            '                    button.ClickMe()
            '                Else
            '                    HtmlPage.Window.Navigate(New Uri(objResultado.RutaArchivoPlano & "?date=" & strFechaValoracion & DateTime.Now.ToString("HH:mm:ss")), "_blank")
            '                End If
            '            Else
            '                A2Utilidades.Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            '            End If
            '        End If
            '        IsBusy = False
            '    End If
            'End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso ExportarInformeValoracion", _
                                                             Me.ToString(), "ExportarInformeValoracion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function EjecutarScriptCierrePasivo(pdtmFechaProceso As DateTime) As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFUtilidades.MensajesProceso)
        Dim objResultado As MensajesProceso
        Dim objProxyES As UtilidadesCFDomainContext
        Dim strUsuario As String = String.Empty
        Dim objVista As EjecutarScriptResultadoView
        Dim objListaRes As List(Of A2.OyD.OYDServer.RIA.Web.CFUtilidades.MensajesProceso)

        Try
            'IsBusy = True

            ErrorForma = String.Empty

            objProxyES = inicializarProxyUtilidades()

            objProxyES.MensajesProcesos.Clear()

            strUsuario = Program.UsuarioSinDomino

            objRet = Await objProxyES.Load(objProxyES.ejecutarScriptCierrePasivoQuery(pdtmFechaProceso, strUsuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de scripts del cierre pasivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de scripts del cierre pasivo.", Me.ToString(), "EjecutarScriptCierrePasivo", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    objListaRes = objProxyES.MensajesProcesos.ToList
                    objResultado = objListaRes.First

                    If objResultado Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se obtuvo respuesta de la ejecución del script del cierre pasivo. No se puede determinar si fue o no ejecutado con éxito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        objVista = New EjecutarScriptResultadoView(objListaRes)
                        Program.Modal_OwnerMainWindowsPrincipal(objVista)
                        objVista.ShowDialog()
                    End If
                End If
            End If
            logResultado = True
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción la definción de los parámetros del script del cierre pasivo", Me.ToString(), "EjecutarScriptCierrePasivo", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False

            objProxyES = Nothing
        End Try

        Return (logResultado)


    End Function

    ''' <summary>
    ''' Realiza la exportación  del avance de procesamiento de valoración a Excel.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ExportarAvanceProcesamiento()
        Try
            IsBusy = True
            Try
                'preparo los datos
                Dim lstData As New List(Of String)
                lstData = ListaAvanceProcesoCierrePortafolio.Select(Function(i) String.Format("{0} - {1} - {2}",
                                                                                              i.strDescripcion, i.strTipo, i.dtmAvance)).ToList
                Dim objdatos As String = String.Join("#", lstData)
                Dim objFile As New SaveFileDialog()
                With objFile
                    .FileName = String.Format("CierrePasivo_{0}_AvanceProceso_{1}.xlsx", dtmFechaProceso.Value.Date.ToString("dd-MM-yyyy"), DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss.fff"))
                    .Filter = "Excel| *.xlsx"
                    .DefaultExt = "xlsx"
                End With
                Dim result As System.Nullable(Of Boolean) = objFile.ShowDialog()
                If result.HasValue AndAlso result = True Then
                    'guardo el resultado
                    Using fileStream As System.IO.Stream = objFile.OpenFile()
                        Using bookStream As MemoryStream = clsCORE.ExportaDatos(objdatos, FileFormat.OpenXMLWorkbook)
                            bookStream.WriteTo(fileStream)
                        End Using
                    End Using
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al exportar los datos", _
                                   Me.ToString(), "ExportarAvanceProcesamiento.ExportarInformeValoracion", Application.Current.ToString(), Program.Maquina, ex)
            End Try
            IsBusy = False


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso ExportarAvanceProcesamiento", _
                                                             Me.ToString(), "ExportarAvanceProcesamiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"



#End Region


#Region "Timer Refrescar pantalla"

    '''<summary>
    ''' Método utilizado para que el proceso de informe de valoración sea automático, para activarlo se neceitan dos parámetro:
    ''' mstrHabilitarRecargaAutomaticaProcesarPortafolio: indica si el proceso debe estar o no activo.
    ''' mintLapsoRecargaAutomaticaProcesarPortafolio: tiempo en segundos que tarda el proceso en reiniciarse.
    ''' </summary>
    Private _myDispatcherTimerProcesarPortafolio As System.Windows.Threading.DispatcherTimer
    Public Sub ReiniciaTimer()
        Try
            If Not String.IsNullOrEmpty(mstrHabilitarRecargaAutomaticaProcesarPortafolio) And Not IsNothing(mintLapsoRecargaAutomaticaProcesarPortafolio) Then
                If mstrHabilitarRecargaAutomaticaProcesarPortafolio = "SI" Then
                    If _myDispatcherTimerProcesarPortafolio Is Nothing Then
                        _myDispatcherTimerProcesarPortafolio = New System.Windows.Threading.DispatcherTimer
                        _myDispatcherTimerProcesarPortafolio.Interval = New TimeSpan(0, 0, 0, 0, mintLapsoRecargaAutomaticaProcesarPortafolio * 1000)
                        AddHandler _myDispatcherTimerProcesarPortafolio.Tick, AddressOf Me.Each_Tick
                    End If
                    _myDispatcherTimerProcesarPortafolio.Start()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            IsBusyProcesando = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Método utilizado para finalizar el hilo del temporizador.
    ''' </summary>
    Public Sub CerrarTemporizador()
        Try
            If Not IsNothing(_myDispatcherTimerProcesarPortafolio) Then
                _myDispatcherTimerProcesarPortafolio.Stop()
                RemoveHandler _myDispatcherTimerProcesarPortafolio.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerProcesarPortafolio = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar parar el timer.", Me.ToString, "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Recarga el grid cuando se este en un tiempo de espera sin notificaciones y sin refrescar la pantalla.
    ''' </summary>
    Private Async Sub Each_Tick(sender As Object, e As EventArgs)
        If Not mdcProxy.IsLoading Then
            If logEstaValorando Then
                Await RecargarInformeProcesamiento()
            Else
                CerrarTemporizador()
            End If
        End If
    End Sub

    Public Async Function RecargarInformeProcesamiento() As Task
        Try
            Await ConsultarInformeProcesamiento()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recargar la pantalla procesar portafolio.", Me.ToString(), "RecargarPantallaOrdenes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Public Async Function ConsultarInformeProcesamiento() As Task
        Try
            Dim objRet As LoadOperation(Of CFCalculosFinancieros.ProcesarPortafolio)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            'IsBusy = True
            mdcProxy.ProcesarPortafolios.Clear()
            objRet = Await mdcProxy.Load(mdcProxy.ConsultarAvanceCierrePasivoQuery(dtmFechaProceso.Value.Date, "Pasivo", Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    CerrarTemporizador()
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento.", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                Else
                    ListaAvanceProcesoCierrePortafolio = mdcProxy.ProcesarPortafolios
                End If
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento", Me.ToString(), "ConsultarInformeProcesamiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Métodos Privados de la pantalla"


    'Private Sub CargaListas()
    '    If Application.Current.Resources.Contains(Program.NombreListaCombos) Then
    '        If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("TIPOCOMPANIA") Then
    '            If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOCOMPANIA")) Then
    '                Dim objListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
    '                objListaTipoCompania = New List(Of OYDUtilidades.ItemCombo)(CType(Application.Current.Resources(Program.NombreListaCombos), 
    '                             Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOCOMPANIA"))
    '                objListaTipoCompania.Add(New ItemCombo() With {
    '                                                                .ID = "",
    '                                                                .Retorno = "T",
    '                                                                .Descripcion = "Todos",
    '                                                                .Categoria = "TIPOCOMPANIA"
    '                                                            })
    '                Me.ListaTipoCompania = objListaTipoCompania
    '                Me.strTipoCompania = objListaTipoCompania.LastOrDefault().ID
    '            End If
    '        End If

    '    End If
    'End Sub

#End Region

#Region "Consultar parametros y campos obligatorios"

    ''' <summary>
    ''' metodo para buscar compañias
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuscarCompanias()
        If mdcProxyUtilidades Is Nothing Then
            mdcProxyUtilidades = inicializarProxyUtilidadesOYD()
        End If
        If mdcProxyUtilidades.IsLoading Then
            A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        'TODO: traer solo la compañias que apliquen para demo se trajeron todas
        IsBusyCompanias = True
        mdcProxyUtilidades.Load(mdcProxyUtilidades.buscarItemsQuery("", "CompaniasxUsuario", "A", "", dtmFechaProceso.Value.ToString("yyyy-MM-dd"), "", Program.UsuarioSinDomino, Program.HashConexion), LoadBehavior.RefreshCurrent, AddressOf TerminoTraerCompañias, dtmFechaProceso.Value.ToShortDateString)
    End Sub

    Private Sub TerminoTraerCompañias(ByVal lo As LoadOperation(Of BuscadorGenerico))
        Try
            ListaCompanias = New ObservableCollection(Of OYDUtilidades.ItemCombo)
            If Not lo.HasError Then
                If lo.Entities.Count > 0 Then
                    'ListaCompanias = Nothing


                    'Dim debug As New clsDebug With {.strMensajeDebug = String.Format("Consulta - Fecha:{0}, Ids:{1}", lo.UserState, String.Join(",", lo.Entities.ToList.Select(Function(i) i.IdItem).ToList))}
                    'GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(Of clsDebug)(debug)

                    ListaCompanias = New ObservableCollection(Of OYDUtilidades.ItemCombo)((From objCompanias In lo.Entities.ToList
                                        Where objCompanias.Descripcion = strTipoCompania
                                     Select New ItemCombo With {.ID = objCompanias.IdItem, .Descripcion = objCompanias.InfoAdicional01}))

                    'debug = New clsDebug With {.strMensajeDebug = String.Format("Procesada - Fecha:{0}, Ids:{1}", lo.UserState, String.Join(",", ListaCompanias.Select(Function(i) i.ID).ToList))}
                    'GalaSoft.MvvmLight.Messaging.Messenger.Default.Send(Of clsDebug)(debug)

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo consultar la compañia.", _
                                    Me.ToString(), "TerminoTraerCompañias", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo consultar la compañia.", _
                                                Me.ToString(), "TerminoTraerCompañias", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusyCompanias = False
        End Try
    End Sub

    ''' <summary>
    ''' Método para consultar los parámetros para recargar y habilitar automáticamente el proceso de valoración.
    ''' </summary>
    Private Sub ConsultarParametros()
        Try
            Dim dcProxyUtil As UtilidadesDomainContext

            dcProxyUtil = inicializarProxyUtilidadesOYD()
            dcProxyUtil.Verificaparametro("LapsoRecargaAutomaticaProcesarPortafolio", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "LapsoRecargaAutomaticaProcesarPortafolio")
            dcProxyUtil.Verificaparametro("HabilitarRecargaAutomaticaProcesarPortafolio", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "HabilitarRecargaAutomaticaProcesarPortafolio")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método consultar parámetros", _
                                                             Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados en la tabla de parámetros.
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState.ToString
                Case "LapsoRecargaAutomaticaProcesarPortafolio"
                    mintLapsoRecargaAutomaticaProcesarPortafolio = CInt(lo.Value)
                Case "HabilitarRecargaAutomaticaProcesarPortafolio"
                    mstrHabilitarRecargaAutomaticaProcesarPortafolio = lo.Value.ToString
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

#End Region


End Class

