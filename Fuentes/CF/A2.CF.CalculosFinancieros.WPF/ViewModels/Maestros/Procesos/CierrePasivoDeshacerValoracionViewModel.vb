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


Public Class CierrePasivoDeshacerValoracionViewModel
    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged

    Dim dcProxy As CalculosFinancierosDomainContext
    Private mintLapsoRecargaAutomaticaProcesarPortafolio As Integer
    Private mstrHabilitarRecargaAutomaticaProcesarPortafolio As String

    Private logEstaValorando As Boolean

    ''' <summary>
    ''' ViewModel para la pantalla Deshacer Cierre Pasivo del maestro de calculos financieros.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Juan Camilo Munera H.
    ''' Descripción      : Deschacer Cierre Pasivo
    ''' Fecha            : Agosto 26 de 2106
    ''' Pruebas CB       : 
    ''' </history>

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Declaracion de Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxyUtilidades As UtilidadesDomainContext
#End Region


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


#Region "Métodos privados del encabezado - REQUERIDOS"

    ' ''' <summary>
    ' ''' Consulta los valores por defecto para un nuevo encabezado
    ' ''' </summary>
    Private Sub ConsultarValoresPorDefecto()
        Try
            'TODO: PENDIENTE OBTENER EL DIA HABIL PARA LA FECHA DE PROCESO
            dtmFechaProceso = Date.Now()
            'CargaListas()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarValoresPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

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
            MyBase.CambioItem("dtmFechaProceso")
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

    ''' <summary>
    ''' Lista de ProcesarPortafolio que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaAvanceProcesoDeshacerCierrePortafolio As EntitySet(Of CFCalculosFinancieros.ProcesarPortafolio)
    Public Property ListaAvanceProcesoDeshacerCierrePortafolio() As EntitySet(Of CFCalculosFinancieros.ProcesarPortafolio)
        Get
            Return _ListaAvanceProcesoDeshacerCierrePortafolio
        End Get
        Set(ByVal value As EntitySet(Of CFCalculosFinancieros.ProcesarPortafolio))
            _ListaAvanceProcesoDeshacerCierrePortafolioPaginada = Nothing
            _ListaAvanceProcesoDeshacerCierrePortafolio = value

            MyBase.CambioItem("ListaAvanceProcesoDeshacerCierrePortafolio")
            MyBase.CambioItem("ListaAvanceProcesoDeshacerCierrePortafolioPaginada")
        End Set
    End Property

    Private _ListaAvanceProcesoDeshacerCierrePortafolioPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de ProcesarPortafolio para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaAvanceProcesoDeshacerCierrePortafolioPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaAvanceProcesoDeshacerCierrePortafolio) Then
                If IsNothing(_ListaAvanceProcesoDeshacerCierrePortafolioPaginada) Then
                    Dim view = New PagedCollectionView(_ListaAvanceProcesoDeshacerCierrePortafolio)
                    _ListaAvanceProcesoDeshacerCierrePortafolioPaginada = view
                    Return view
                Else
                    Return (_ListaAvanceProcesoDeshacerCierrePortafolioPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
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
        mdcProxyUtilidades.Load(mdcProxyUtilidades.buscarItemsQuery("", "CompaniasDeshacerCierre", "A", "", dtmFechaProceso.Value.ToString("yyyy-MM-dd"), "", Program.UsuarioSinDomino, Program.HashConexion), LoadBehavior.RefreshCurrent, AddressOf TerminoTraerCompañias, dtmFechaProceso.Value.ToShortDateString)
    End Sub
#End Region

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


#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Método para deschacer el cierre pasivo de portafolios
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ProcesarDeshacerCierrePasivo()
        If Not mdcProxy Is Nothing AndAlso Not mdcProxy.ProcesarPortafolios Is Nothing Then
            mdcProxy.ProcesarPortafolios.Clear()
            ListaAvanceProcesoDeshacerCierrePortafolio = mdcProxy.ProcesarPortafolios
        End If

        If ValidarDatos() Then
            A2Utilidades.Mensajes.mostrarMensajePregunta("Está seguro de deshacer el cierre pasivo de los productos seleccionados, esta operación puede tardar varios minutos, ¿Desea continuar?", Program.TituloSistema, ValoresUserState.Consultar.ToString, AddressOf ConfirmarPreguntaDeshacerCierrePasivo, False)
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' Método para realizar la validacion de compañias afectadas colateralmente en el descruce del cierre pasivo
    ''' </summary>
    ''' <remarks></remarks>
    Private Async Sub ValidarDeshacerCierrePasivo()
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

            'Se habilita el IsBusyProcesando 
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


            'Incia el proceso de validación
            objRet = Await mdcProxy.ValidarDeshacerValoracionFondo(CDate(dtmFechaProceso), xmlCompleto, Program.Usuario, Program.HashConexion).AsTask


            IsBusyProcesando = False
            strMsg = objRet.Value.ToString()

            If Not String.IsNullOrEmpty(strMsg) Then
                If strMsg.StartsWith("OK|") Then
                    EjecutarDeshacerCierrePasivo()
                Else
                    strMsg = strMsg.Replace("[#]", Environment.NewLine)
                    strMsg = String.Format("{0}{1}", vbCrLf, strMsg)
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Existen compañias diferentes a las seleccionadas que se veran afectadas por el descruce: " & vbNewLine & strMsg & vbNewLine & vbNewLine & "¿Desea continuar?", Program.TituloSistema, ValoresUserState.Ingresar.ToString, AddressOf ConfirmarPreguntaDeshacerCierrePasivo, False)

                End If
            End If


            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            IsBusyProcesando = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el descruce del cierre pasivo", Me.ToString(), "ValidarDeshacerCierrePasivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para realizar el descruce del cierre pasivo
    ''' </summary>
    ''' <remarks></remarks>
    Private Async Sub EjecutarDeshacerCierrePasivo()
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
            objRet = Await mdcProxy.ActualizarDeshacerValoracionFondo(CDate(dtmFechaProceso), xmlCompleto, Program.Usuario, Program.HashConexion).AsTask

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


            IsBusy = False

        Catch ex As Exception
            logEstaValorando = False
            IsBusy = False
            IsBusyProcesando = False
            'CerrarTemporizador()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al deshacer la valoración", Me.ToString(), "EjecutarDeshacerCierrePasivo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub


    ''' <summary>
    ''' Método para realizar la valoración/cierre pasivo
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConfirmarPreguntaDeshacerCierrePasivo(ByVal sender As Object, ByVal e As EventArgs)
        If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
            Select Case CType(sender, A2Utilidades.wppMensajePregunta).CodigoLlamado
                Case ValoresUserState.Consultar.ToString
                    ValidarDeshacerCierrePasivo()
                Case ValoresUserState.Ingresar.ToString
                    EjecutarDeshacerCierrePasivo()
            End Select
        End If
    End Sub


    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            ''Valida la fecha de valoración
            If IsNothing(dtmFechaProceso) Then
                strMsg = String.Format("{0}{1} + La fecha del proceso es un campo requerido.", strMsg, vbCrLf)
            End If

            'Verificar que se escoja un producto
            If ListaCompaniasSeleccionadas.Count <= 0 Then
                strMsg = String.Format("{0}{1} + El producto es un campo requerido.", strMsg, vbCrLf)
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


#Region "Timer Refrescar pantalla"
    '''<summary>
    ''' Método utilizado para que el proceso de informe de valoración sea automático, para activarlo se neceitan dos parámetro:
    ''' mstrHabilitarRecargaAutomaticaProcesarPortafolio: indica si el proceso debe estar o no activo.
    ''' mintLapsoRecargaAutomaticaProcesarPortafolio: tiempo en segundos que tarda el proceso en reiniciarse.
    ''' </summary>
    ''' 

    Private _myDispatcherTimerDeshacerProcesarPortafolio As System.Windows.Threading.DispatcherTimer
    Public Sub ReiniciaTimer()
        Try
            If Not String.IsNullOrEmpty(mstrHabilitarRecargaAutomaticaProcesarPortafolio) And Not IsNothing(mintLapsoRecargaAutomaticaProcesarPortafolio) Then
                If mstrHabilitarRecargaAutomaticaProcesarPortafolio = "SI" Then
                    If _myDispatcherTimerDeshacerProcesarPortafolio Is Nothing Then
                        _myDispatcherTimerDeshacerProcesarPortafolio = New System.Windows.Threading.DispatcherTimer
                        _myDispatcherTimerDeshacerProcesarPortafolio.Interval = New TimeSpan(0, 0, 0, 0, mintLapsoRecargaAutomaticaProcesarPortafolio * 1000)
                        AddHandler _myDispatcherTimerDeshacerProcesarPortafolio.Tick, AddressOf Me.Each_Tick
                    End If
                    _myDispatcherTimerDeshacerProcesarPortafolio.Start()
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
            If Not IsNothing(_myDispatcherTimerDeshacerProcesarPortafolio) Then
                _myDispatcherTimerDeshacerProcesarPortafolio.Stop()
                RemoveHandler _myDispatcherTimerDeshacerProcesarPortafolio.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerDeshacerProcesarPortafolio = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar parar el timer.", Me.ToString, "CerrarTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recargar la pantalla deshacer portafolio.", Me.ToString(), "RecargarInformeProcesamiento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            objRet = Await mdcProxy.Load(mdcProxy.ConsultarAvanceCierrePasivoQuery(dtmFechaProceso.Value.Date, "DescrucePasivo", Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    CerrarTemporizador()
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento.", Me.ToString(), "ConsultarInformeProcesamiento", Program.TituloSistema, Program.Maquina, objRet.Error)
                Else
                    ListaAvanceProcesoDeshacerCierrePortafolio = mdcProxy.ProcesarPortafolios
                End If
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el informe de procesamiento", Me.ToString(), "ConsultarInformeProcesamiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Método para consultar los parámetros para recargar y habilitar automáticamente el proceso de valoración.
    ''' </summary>
    Private Sub ConsultarParametros()
        Try
            Dim dcProxyUtil As UtilidadesDomainContext

            dcProxyUtil = inicializarProxyUtilidadesOYD()
            dcProxyUtil.Verificaparametro("LapsoRecargaAutomaticaProcesarPortafolio", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametrosDescruce, "LapsoRecargaAutomaticaProcesarPortafolio")
            dcProxyUtil.Verificaparametro("HabilitarRecargaAutomaticaProcesarPortafolio", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametrosDescruce, "HabilitarRecargaAutomaticaProcesarPortafolio")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método consultar parámetros", _
                                                             Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados en la tabla de parámetros.
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    Private Sub TerminoConsultarParametrosDescruce(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState.ToString
                Case "LapsoRecargaAutomaticaProcesarPortafolio"
                    mintLapsoRecargaAutomaticaProcesarPortafolio = CInt(lo.Value)
                Case "HabilitarRecargaAutomaticaProcesarPortafolio"
                    mstrHabilitarRecargaAutomaticaProcesarPortafolio = lo.Value.ToString
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametrosDescruce", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub


#End Region




End Class
