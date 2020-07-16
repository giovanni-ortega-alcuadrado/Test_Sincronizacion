Imports System.Windows.Navigation
Imports System.Reflection
Imports Microsoft.VisualBasic
Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports A2Utilidades.Recursos.RecursosApp
Imports GalaSoft.MvvmLight.Messaging
Imports System.Threading.Tasks
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports GalaSoft.MvvmLight.Command
Imports A2.Notificaciones.Cliente
Imports System.IO

Partial Public Class A2Consola
    Inherits UserControl
    Implements INotifyPropertyChanged

#Region "Eventos"
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

#End Region

#Region "Constantes"

    Private Const STR_VENTANA_INICIO As String = "Inicio"
    Private Const STR_VENTANA_CERRARSESSION As String = "Cerrar sesión"

    ''' ----------------------------------------------------------------------------------------
    ''' Constantes para el llamado a ejecutable de títulos
    Private Const MSTR_CLAVE_INICIAL As String = "OYDNET_"
    Private Const MSTR_CLAVE_FINAL As String = "_TITULOS2008"

    Private STR_TOPICO_CONSOLA As String = "CONSOLA"


#End Region

#Region "Variables"

    '''------------------------------------------------------------------------------------------------------------------------
    '''------------------------------------------------------------------------------------------------------------------------
    '''-- DECLARACIÓN DE VARIABLES
    '''------------------------------------------------------------------------------------------------------------------------
    '''------------------------------------------------------------------------------------------------------------------------
    ''' 
    Private WithEvents mobjLogin As A2Utilidades.wppLogin
    Private WithEvents mobjTimer As System.Windows.Threading.DispatcherTimer

    Private mintNroVentanas As Integer = 0 '-- Nro. de ventanas que están abiertas en la consola
    Private mintNroVentanasAbiertas As Integer = 0 '-- Nro. de ventanas abiertas en la consola durante la sesión sin importar si se han cerrado o no
    Private mlogMostrarMensajeLog As Boolean = False

    '''------------------------------------------------------------------------------------------------------------------------
    ''' -- Variables para propiedades
    '''------------------------------------------------------------------------------------------------------------------------
    ''' 
    Private _mstrVentanaActiva As String
    Private _mintVentanaActiva As Integer
    Private _mintAltoEncabezado As Integer
    Private strMensaje As String

    'CLIENTE NOTIFICACIONES SIGNALR
    Private objClienteNotificaciones As clsClienteSignalR

    'VARIABLES PROPIEDADES 
    Private _VisibilidadMensajesConsola As Visibility = Windows.Visibility.Collapsed
    Private _VisibilidadNotificaciones As Visibility = Windows.Visibility.Collapsed
    Private _VisibilidadDebug As Visibility = Windows.Visibility.Collapsed
    Private _VisibilidadLinkNotificaciones As Visibility = Windows.Visibility.Collapsed
    Private _VisibilidadLinkDebug As Visibility = Windows.Visibility.Collapsed
    Private mlogCargoAssembliesWPF As Boolean = False
    Private mlogCargoAssembliesRIA As Boolean = False
    Private plogSetearColorStackPanel As Boolean = True

#End Region

#Region "Propiedades"

    Private _TabsConsola As New ObservableCollection(Of clsTabItemConsola)
    Public Property TabsConsola() As ObservableCollection(Of clsTabItemConsola)
        Get
            Return _TabsConsola
        End Get
        Set(ByVal value As ObservableCollection(Of clsTabItemConsola))
            _TabsConsola = value
            CambioPropiedad("TabsConsola")
        End Set
    End Property

    Private _mlogProcesando As Boolean = False
    Public Property Procesando As Boolean
        Get
            Return (_mlogProcesando)
        End Get
        Set(ByVal value As Boolean)
            _mlogProcesando = value
        End Set
    End Property

    Private _mlogConsultandoParamConsola As Boolean = False '-- CCM20131025 - Indica si el sistema está  consultando (se está ejecutando el método de consulta) los parámetros de consola en el servicio RIA de la aplicación activa
    Public ReadOnly Property ConsultandoParamConsola As Boolean
        Get
            Return (_mlogConsultandoParamConsola)
        End Get
    End Property


    Public Property DebugCollection As ObservableCollection(Of String)

    Public Property LogNotificaciones As ObservableCollection(Of String)

    Public Property ListaMensajesConsola As ObservableCollection(Of NotificacionConsola)

    Public Property VisibilidadMensajesConsola As Visibility
        Get
            Return _VisibilidadMensajesConsola
        End Get
        Set(value As Visibility)
            If _VisibilidadMensajesConsola <> value Then
                _VisibilidadMensajesConsola = value
                CambioPropiedad("VisibilidadMensajesConsola")
            End If
        End Set
    End Property

    Public Property VisibilidadNotificaciones As Visibility
        Get
            Return _VisibilidadNotificaciones
        End Get
        Set(value As Visibility)
            If _VisibilidadNotificaciones <> value Then
                _VisibilidadNotificaciones = value
                CambioPropiedad("VisibilidadNotificaciones")
            End If
        End Set
    End Property

    Public Property VisibilidadDebug As Visibility
        Get
            Return _VisibilidadDebug
        End Get
        Set(value As Visibility)
            If _VisibilidadDebug <> value Then
                _VisibilidadDebug = value
                CambioPropiedad("VisibilidadDebug")
            End If
        End Set
    End Property

    Public Property VisibilidadLinkNotificaciones As Visibility
        Get
            Return _VisibilidadLinkNotificaciones
        End Get
        Set(value As Visibility)
            If _VisibilidadLinkNotificaciones <> value Then
                _VisibilidadLinkNotificaciones = value
                CambioPropiedad("VisibilidadLinkNotificaciones")
            End If
        End Set
    End Property

    Public Property VisibilidadLinkDebug As Visibility
        Get
            Return _VisibilidadLinkDebug
        End Get
        Set(value As Visibility)
            If _VisibilidadLinkDebug <> value Then
                _VisibilidadLinkDebug = value
                CambioPropiedad("VisibilidadLinkDebug")
            End If
        End Set
    End Property


#End Region

#Region "Inicialización"

    Public Sub New()
        Try
            InitializeComponent()

            If Application.Current.Resources.Contains(RecursosApp.A2Consola_MostrarLog.ToString()) Then
                mlogMostrarMensajeLog = CBool(IIf(Application.Current.Resources(RecursosApp.A2Consola_MostrarLog.ToString()).ToString = "1", True, False))
            End If

            Me.txtAplicacionActiva.Text = Program.TituloSistema

            '// Datos de la barra de estado
            Me.txtFecha.Text = String.Format("{0}, {1}", Now().ToLongDateString(), Now.ToShortTimeString())
            Me.txtLoginActivo.Text = Program.Usuario

            Dim strVersionConsola As String = System.Reflection.Assembly.GetCallingAssembly().FullName().ToLower
            Dim intPosVersion As Integer = strVersionConsola.IndexOf("version") + 8 ' 8 es la longitud de la palabra versión más uno del espacio en blanco después de esta palabra
            strVersionConsola = strVersionConsola.Substring(intPosVersion, strVersionConsola.IndexOf(" ", intPosVersion) - intPosVersion - 1)

            If Application.Current.Resources.Contains(RecursosApp.A2Consola_VersionLocal.ToString()) Then
                strVersionConsola = Application.Current.Resources(RecursosApp.A2Consola_VersionLocal.ToString())
            ElseIf strVersionConsola.Equals(String.Empty) Then
                strVersionConsola = Program.ConsolaVersion
            End If

            Me.txtConsola.Text = Program.NOMBRE_CONSOLA & ", versión " & strVersionConsola

            If validarUsuario() Then
                Messenger.Default.Register(Of String)(Me, AddressOf capturarEventosRegistradosStr)
            End If

            mobjTimer = New System.Windows.Threading.DispatcherTimer() With {.Interval = New TimeSpan(0, 0, 20)}
            mobjTimer.Start()

            'Inicializa la conexión al servidor de notificaciones.
            InicializarNotificacionesSinalR()

            'Inicia messenger mvvmlight
            Messenger.Default.Register(Of clsNotificacionCliente)(Me, AddressOf EnviarNotificacion)
            Messenger.Default.Register(Of clsDebug)(Me, AddressOf RegistrarDebug)

            'Inicialización de comandos.
            AddHandler Me.KeyDown, AddressOf Consola_KeyDown

            DebugCollection = New ObservableCollection(Of String)

            LogNotificaciones = New ObservableCollection(Of String)

            ListaMensajesConsola = New ObservableCollection(Of NotificacionConsola)

            'Se asigna el datacontext a la vista.
            Me.DataContext = Me
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consola de aplicaciones", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub A2Consola_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

    End Sub

#End Region

#Region "Métodos privados"

    Private Function validarUsuario() As Boolean
        Dim logRes As Boolean = False

        Try
            If Application.Current.Resources.Contains(RecursosApp.A2Consola_UsuarioActivo.ToString()) Then
                mobjLogin = New A2Utilidades.wppLogin

                If CType(Application.Current.Resources(RecursosApp.A2Consola_UsuarioActivo.ToString()), Usuario).SeguridadIntegrada Then
                    mobjLogin.SeguridadIntegrada = True
                    mobjLogin.logModificarLogin = False
                    mobjLogin.Login = Program.Usuario
                    mobjLogin.logModificarLogin = True
                Else
                    mobjLogin.SeguridadIntegrada = False

                    If glogSugerirLogin Then
                        mobjLogin.logModificarLogin = False
                        mobjLogin.Login = eliminarDominio(Program.Usuario)
                        mobjLogin.logModificarLogin = True
                    Else
                        mobjLogin.logModificarLogin = False
                        mobjLogin.Login = ""
                        mobjLogin.logModificarLogin = True
                    End If
                End If

                Program.Modal_OwnerMainWindowsPrincipal(mobjLogin)
                mobjLogin.ShowDialog()

                logRes = True
            Else
                cargarVentanaDocumento(STR_VENTANA_INICIO, "/Views/AccesoDenegado.xaml", False, Nothing)
                Mensajes.mostrarMensaje("No se tiene la información necesaria para validar el usuario", Program.TituloSistema, wppMensajes.TiposMensaje.Personalizado)
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la validación del usuario", Me.Name, "validarUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Return (logRes)
    End Function

    ''' <summary>
    ''' Ejecuta los métodos del control menú que cargan los datos de acuerdo con los permisos del usuario
    ''' </summary>
    ''' <remarks>Si la consola está en forma multiaplicación se cargan primero las aplicaciones autorizadas. Si está en monoaplicación carga el menú de la aplicación indicada</remarks>
    ''' 
    Private Sub CargarMenu()
        Dim vstrDatosApp As String()
        Dim strApp As String = ""
        Try
            Me.ctlMenuTop.MultiAplicacion = Program.Multiaplicacion

            If ctlMenuTop.MultiAplicacion Then
                Me.ctlMenuTop.consultarAplicacionesUsuario("", "")
            Else
                If Application.Current.Resources.Contains(GSTR_RECURSO_MONOAPLICACION) Then
                    strApp = Application.Current.Resources(GSTR_RECURSO_MONOAPLICACION).ToString
                Else
                    strApp = "" & Program.SeparadorListas & "" & Program.SeparadorListas & ""
                End If
                vstrDatosApp = strApp.Split(CChar(Program.SeparadorListas))
                Me.ctlMenuTop.consultarAplicacionesUsuario(vstrDatosApp(0), vstrDatosApp(1), vstrDatosApp(2))
                'Me.ctlMenuTop.consultarMenuAplicacion()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Falló la carga de las opciones del menú para el usuario " & Program.Usuario, Me.Name, "CargarMenu", Program.TITULO_CONSOLA, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub configurarTabInicio()
        Try
            cargarVentanaDocumento(STR_VENTANA_INICIO, "/Views/Inicio.xaml", False, Nothing)
            'VisualStateManager.GoToState(lnkInicio, "ActiveLink", True)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo configurar la pestaña de inicio.", Me.Name, "configurarTabInicio", Program.TITULO_CONSOLA, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Adiciona una nueva ventana (tab) a la consola.
    ''' Modificado por Juan David Correa - JDCP.
    ''' Se modifica la carga del control por las pestañas de component ONE.
    ''' Fecha 02 de Julio del 2013
    ''' </summary>
    ''' <param name="pstrTitulo">Título que aparecerá en el tab adicionado</param>
    ''' <param name="pstrURL">URL de la opción que debe ser cargada en la nueva ventana</param>
    ''' <param name="plogVentanaNueva">Indica si se debe cargar en una nueva ventana o si debe verificarse que ya exista otra ventana con el mismo nombre para actualizar el contenido</param>
    ''' <param name="pobjOpcion">Datos de la opción que se cargará en la nueva ventana</param>
    ''' <returns>Retorna el nombre de la ventana adicionada o activada</returns>
    ''' <remarks>Si el usuario tiene el máximo número de ventanas abiertas no se abre una nueva y se informa al usuario</remarks>
    ''' 
    Friend Function cargarVentanaDocumento(ByVal pstrTitulo As String, ByVal pstrURL As String, ByVal plogVentanaNueva As Boolean, ByVal pobjOpcion As Menu) As String
        Dim logVentanaCargada As Boolean = False
        Dim strNomVentana As String = String.Empty
        Dim strMsg As String = String.Empty
        'JDCP
        '***************************************************************************
        'Dim objVentana As Divelements.SandDock.DocumentWindow = Nothing
        Dim objVentanaTab As System.Windows.Controls.TabItem = Nothing
        '***************************************************************************

        Try
            If pstrURL.Trim().Length = 0 Then
                Return (String.Empty)
            Else
                If Program.ValidarVersionAssemblies() Then
                    If VerificarAssemblies() = False Then
                        Return (String.Empty)
                    End If
                End If
            End If

            If mintNroVentanas <= CamtidadMaximaVentanas() Then
                '-- Se valida que el URL coience con '/' porque solamente se soportan URL relativos
                pstrURL = pstrURL.Trim()
                If pstrURL.Substring(0, 1) <> "/" Then
                    pstrURL = "/" & pstrURL
                End If

                If plogVentanaNueva = False Then
                    ' CCM201308: Validar si una ventana con el mismo tema ya está cargada
                    If pstrTitulo.Equals(STR_VENTANA_INICIO) Or pstrTitulo.Equals(STR_VENTANA_CERRARSESSION) Then
                        If ddcDocumentosTabControl.Items.Count > 0 Then
                            ' La primera ventana contiene la ventana de inicio. No se deja cerrar.
                            objVentanaTab = CType(ddcDocumentosTabControl.Items(0), System.Windows.Controls.TabItem)
                        Else
                            plogVentanaNueva = True
                        End If
                    Else
                        For Each objVen In ddcDocumentosTabControl.Items
                            If objVen.GetType.FullName = "System.Windows.Controls.TabItem" Then
                                objVentanaTab = CType(objVen, System.Windows.Controls.TabItem)
                                If CType(objVentanaTab.Header, clsTabItemConsola).Header.ToString().ToLower = pstrTitulo Then
                                    logVentanaCargada = True
                                    Exit For
                                End If
                            End If
                        Next

                        If logVentanaCargada = False Then
                            plogVentanaNueva = True
                        End If
                    End If
                End If

                If plogVentanaNueva Then
                    '-- Se incrementa aca porque se inicializa en cero y se manejan ventanas desde 1 hasta CamtidadMaximaVentanas()
                    mintNroVentanas += 1
                    mintNroVentanasAbiertas += 1
                    strNomVentana = GSTR_PREF_NOM_VENTANA & String.Format("000", mintNroVentanasAbiertas)

                    'JDCP
                    '***************************************************************************
                    objVentanaTab = New System.Windows.Controls.TabItem
                    objVentanaTab.Tag = pobjOpcion
                    '***************************************************************************

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '' TEMPORAL POR MANEJO DE REPORTES DE OYD
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim intPosParam As Integer = 0
                    Dim intPosIniValor As Integer = 0
                    Dim strNomRecurso As String = ""
                    Dim strValRecurso As String = ""

                    If Not pobjOpcion Is Nothing Then
                        If Not pobjOpcion.Tipo Is Nothing Then
                            Select Case pobjOpcion.Tipo.ToUpper
                                Case TipoOpcionMenu.G.ToString
                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    '' TEMPORAL POR MANEJO DEL VISOR DE REPORTES 
                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                                    ' ATENCIÓN:
                                    ' ========
                                    ' Este tipo se utiliza para identificar los URL que traen parámetros y adicionarlo a un recurso global. 
                                    ' Esto deberá ser mejorado modificando inicialmente el visor para que el parámetro no sea tomado de los
                                    ' recursos de la aplicación y así lograr independencia de la consola y las Aplicaciones.
                                    intPosParam = pstrURL.IndexOf("?")
                                    If intPosParam > 0 Then
                                        intPosIniValor = pstrURL.IndexOf("=", intPosParam)
                                        If intPosIniValor > 0 Then
                                            strNomRecurso = pstrURL.Substring(intPosParam + 1, intPosIniValor - intPosParam - 1) ' Se corta entre el ? y el =
                                            strValRecurso = pstrURL.Substring(intPosIniValor + 1) ' Se corta desde el caracter siguiente al =
                                            If Application.Current.Resources.Contains(strNomRecurso) Then
                                                Application.Current.Resources.Remove(strNomRecurso)
                                            End If
                                            Application.Current.Resources.Add(strNomRecurso, strValRecurso)
                                            pstrURL = pstrURL.Substring(0, intPosParam) ' Se corta hasta el caracter anterior al ?

                                            'Modificado por Juan David Correa
                                            'Se obtiene el nombre del reporte sin la ruta, solamente el nombre del reporte
                                            If strNomRecurso = Program.GSTR_NOMBRERECURSO_RPTS Then
                                                Dim strNombreReporte As String = String.Empty

                                                If strValRecurso.Contains("/") And Not String.IsNullOrEmpty(strValRecurso) Then
                                                    For Each li In strValRecurso.Split(CChar("/"))
                                                        strNombreReporte = li
                                                    Next
                                                Else
                                                    strNombreReporte = strValRecurso
                                                End If

                                                If Application.Current.Resources.Contains(Program.GSTR_NOMBREPARAMETRO_RPTS) Then
                                                    Application.Current.Resources.Remove(Program.GSTR_NOMBREPARAMETRO_RPTS)
                                                End If
                                                Application.Current.Resources.Add(Program.GSTR_NOMBREPARAMETRO_RPTS, strNombreReporte)
                                            End If
                                        End If
                                    End If

                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                    '' FIN TEMPORAL POR MANEJO DEL VISOR DE REPORTES 
                                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                Case TipoOpcionMenu.B.ToString
                                    A2Utilidades.Mensajes.mostrarMensaje("Esta opción solamente está disponible cuando el aplicativo se ejecuta dentro del Explorador de Internet.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                    Exit Try
                                Case TipoOpcionMenu.I.ToString
                                    Dim strApp As String = A2Utilidades.CifrarSL.cifrar(Program.Aplicacion)
                                    Dim strVerApp As String = A2Utilidades.CifrarSL.cifrar(Program.VersionAplicacion)
                                    Dim strUsuario As String = A2Utilidades.CifrarSL.cifrar(Program.Usuario)
                                    Dim strPasswordUsr As String = A2Utilidades.CifrarSL.cifrar(Program.Password)
                                    Dim strClaveVerif As String = A2Utilidades.CifrarSL.cifrar(MSTR_CLAVE_INICIAL & Year(Now()).ToString() & Microsoft.VisualBasic.Right("00" & Month(Now()).ToString(), 2) & Microsoft.VisualBasic.Right("00" & Day(Now()).ToString(), 2) & MSTR_CLAVE_FINAL)
                                    Dim strEjecutable As String = pobjOpcion.Hipervinculo & " " & strClaveVerif & " " & strUsuario & " " & strPasswordUsr & " " & strApp & " " & strVerApp

                                    Using wScript = CreateObject("WScript.Shell")

                                        wScript.Run(strEjecutable, 1, True)

                                    End Using

                                    Exit Try

                                Case TipoOpcionMenu.H.ToString
                                    If pstrURL.ToString().StartsWith("/") Then
                                        pstrURL = pstrURL.ToString().Substring(1, pstrURL.ToString.Length - 1).ToString()
                                    End If

                                    LoadURLBrowserDefault(pstrURL, plogVentanaNueva)
                            End Select
                        End If
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '' FIN TEMPORAL POR MANEJO DE REPORTES DE OYD
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    'JDCP
                    '***************************************************************************
                    'OBTENER EL QUERYSTRING DE LAS VISTAS
                    If Application.Current.Resources.Contains("QueryString") Then
                        Application.Current.Resources.Remove("QueryString")
                    End If
                    Dim intPosQuery As Integer = pstrURL.LastIndexOf("?")
                    Dim strQuery As String = String.Empty
                    If intPosQuery > 0 Then
                        strQuery = pstrURL.Substring(intPosQuery + 1)
                    End If
                    Application.Current.Resources.Add("QueryString", strQuery)

                    objVentanaTab.HorizontalAlignment = Windows.HorizontalAlignment.Stretch
                    objVentanaTab.HorizontalContentAlignment = Windows.HorizontalAlignment.Stretch
                    objVentanaTab.VerticalAlignment = Windows.VerticalAlignment.Stretch
                    objVentanaTab.VerticalContentAlignment = Windows.VerticalAlignment.Stretch

                    objVentanaTab.Background = New SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0)) 'Transparente

                    objVentanaTab.Name = strNomVentana
                    'objVentanaTab.Header = pstrTitulo
                    objVentanaTab.AllowDrop = False

                    Dim pstrTituloAlterno As String = pstrTitulo
                    If plogSetearColorStackPanel Then
                        'verifica sí se debe de cambiar el color de la consola, esto se utilizara cuando tengan varias consolas instaladas en la maquina
                        If Not String.IsNullOrEmpty(A2ConsolaWPF.MySettings.Default.ColorFondoTabPrincipal) Then
                            If A2ConsolaWPF.MySettings.Default.ColorFondoTabPrincipal = "ROJO" Then
                                Me.stackTabControl.Background = Brushes.Red
                            ElseIf A2ConsolaWPF.MySettings.Default.ColorFondoTabPrincipal = "VERDE" Then
                                Me.stackTabControl.Background = Brushes.Green
                            ElseIf A2ConsolaWPF.MySettings.Default.ColorFondoTabPrincipal = "CAFE" Then
                                Me.stackTabControl.Background = Brushes.Brown
                            End If
                        End If

                        If Not String.IsNullOrEmpty(A2ConsolaWPF.MySettings.Default.TextoFondoTabPrincipal) Then
                            pstrTituloAlterno = A2ConsolaWPF.MySettings.Default.TextoFondoTabPrincipal
                        End If

                        plogSetearColorStackPanel = False
                    End If

                    objVentanaTab.Header = TabConsola_AddItem(pstrTituloAlterno, pstrURL, objVentanaTab)
                    objVentanaTab.HeaderTemplate = CType(Application.Current.Resources("TabItemConsolaTemplate"), DataTemplate)

                    ddcDocumentosTabControl.Items.Add(objVentanaTab)

                    Task.Run(Function()
                                 Application.Current.Dispatcher.Invoke(
                                            Function()
                                                Try
                                                    objVentanaTab.Content = New upgNavegacion With {.urlNavegacionRelativa = pstrURL}
                                                    objVentanaTab.IsSelected = True
                                                Catch ex As Exception
                                                End Try

                                                Return True
                                            End Function
                                            )
                                 Return True
                             End Function)

                    '***************************************************************************
                Else
                    'JDCP
                    '***************************************************************************
                    'strNomVentana = objVentana.Name
                    'objVentana.Content = New upgNavegacion With {.urlNavegacionRelativa = pstrURL}
                    If pstrTitulo = STR_VENTANA_INICIO Or pstrTitulo = STR_VENTANA_CERRARSESSION Then
                        objVentanaTab = CType(ddcDocumentosTabControl.Items(0), System.Windows.Controls.TabItem)
                    End If

                    If Not IsNothing(objVentanaTab) Then
                        Task.Run(Function()
                                     Application.Current.Dispatcher.Invoke(
                                                Function()
                                                    Try
                                                        objVentanaTab.Content = New upgNavegacion With {.urlNavegacionRelativa = pstrURL}
                                                        objVentanaTab.IsSelected = True
                                                    Catch ex As Exception
                                                    End Try

                                                    Return True
                                                End Function
                                                )
                                     Return True
                                 End Function)
                    End If
                    '***************************************************************************
                End If

                If Not pobjOpcion Is Nothing Then
                    If pobjOpcion.Tipo.Equals(TipoOpcionMenu.H.ToString) Or pobjOpcion.Tipo.Equals(TipoOpcionMenu.I.ToString) Then
                        ' Cerrar ventana luego de iniciar el hipervínculo

                        'PENDIENTE PROBAR SI FUNCIONA CORRECTAMNETE AL ELIMINAR
                        Me.ddcDocumentosTabControl.Items.Remove(objVentanaTab)
                    Else
                        objVentanaTab.IsSelected = True
                    End If
                ElseIf pstrTitulo = STR_VENTANA_INICIO Then
                    'objVentanaTab.CanUserClose = False
                    objVentanaTab.IsSelected = True
                ElseIf pstrTitulo = STR_VENTANA_CERRARSESSION Then
                    objVentanaTab.IsSelected = True
                End If
            Else
                Mensajes.mostrarMensaje("Tiene " & CamtidadMaximaVentanas().ToString() & " ventanas abiertas. No puede abrir más de este número, por favor cierre alguna que no esté utilizando.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                'MessageBox.Show("Tiene " & MINT_MAX_VENTANAS.ToString() & " ventanas abiertas. No puede abrir más de este número, por favor cierre alguna que no esté utilizando.")
            End If
        Catch ex As Exception
            If ex.Message.Contains("HRESULT: 0x80070002") Or ex.Message.Contains("Error 0x1709") Or ex.Message.Contains("El sistema no puede hallar el archivo especificado") Then
                A2Utilidades.Mensajes.mostrarMensaje("Verifique que tenga instalado el aplicativo de OyD Windows, con su administrador de aplicaciones.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            Else
                Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al cargar la opción seleccionada", Me.Name, "cargarVentanaDocumento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            End If
        End Try

        Return (strNomVentana)
    End Function

    '   CCM201308 - Cristian Ciceri Muñetón
    ''' <summary>
    ''' Captura los eventos generados mediante MvvmLight (GalaSoft.MvvmLight.Messaging).
    ''' Permite la ejecución de acciones generales sobre la consola de acuerdo a los eventos lanzados
    ''' por los controles de usuario que se cargan dinámicamente.
    ''' </summary>
    ''' <param name="pstrEvento">Datos del evento recibido. 
    ''' Se recibe un texto con formato [Separador][NombreEvento][Separador][NamespaceEliminar][Separador][Aplicacion][Separador][Version][Separador][TipoVentana]</param>
    ''' <history>
    ''' Código:      GAG20150709
    ''' Fecha:       2015/07/09
    ''' Responsable: Germán Arbey González Osorio
    ''' Descripción: Se controla la variable "pstrEvento" cuando este vacío, esto debido a que genera un error en la pantalla "ExportarFormatosView.xaml".
    '''              Esta es una solución temporal y debe validarse con el personal de Arquitectura
    ''' </history>
    Private Sub capturarEventosRegistradosStr(ByVal pstrEvento As String)

        If pstrEvento <> "" Then

            Const EVENTO As Byte = 1
            Const NAME_SPACE As Byte = 2
            Const APLICACION As Byte = 3
            Const VERSION As Byte = 4
            Const TIPO_VENTANA As Byte = 5

            Dim objVentanaTab As System.Windows.Controls.TabItem
            Dim aobjVentanasEliminar As System.Windows.Controls.TabItem()
            Dim objOpcion As A2Utilidades.Menu
            Dim objContenido As Object
            Dim astrParam As String()
            Dim strSeparador As Char
            Dim strAplicacion As String = Me.txtAplicacionActiva.Text
            Dim strVersion As String = String.Empty
            Dim strNamespace As String
            Dim strTipoVentana As String
            Dim strNamespaceLink As String
            Dim strTipoVentanaLink As String
            Dim intPos As Integer
            Dim intNroVentanas As Integer

            Try
                strSeparador = pstrEvento.Chars(0)
                astrParam = pstrEvento.Split(strSeparador)

                Select Case astrParam(EVENTO).ToLower
                    Case "CerrarVentanasAplicacion".ToLower()
                        strNamespace = astrParam(NAME_SPACE).ToLower
                        strAplicacion = astrParam(APLICACION).ToLower
                        strVersion = astrParam(VERSION).ToLower
                        strTipoVentana = astrParam(TIPO_VENTANA).ToLower
                        intNroVentanas = 0

                        ReDim aobjVentanasEliminar(0)

                        For Each objVen In ddcDocumentosTabControl.Items
                            If objVen.GetType.FullName = "System.Windows.Controls.TabItem" Then
                                objVentanaTab = CType(objVen, System.Windows.Controls.TabItem)
                                objOpcion = CType(objVentanaTab.Tag, A2Utilidades.Menu)
                                If Not objOpcion Is Nothing Then
                                    If objOpcion.Aplicacion.ToLower.Equals(strAplicacion.ToLower) And objOpcion.Version.ToLower.Equals(strVersion.ToLower) Then
                                        ' No se toma el slash (/) inicial que debe venir al inicio del hipervínculo para que se cargue el control dinámicamente
                                        If objOpcion.Hipervinculo.Substring(0, 1).Equals("/") Then
                                            intPos = 1
                                        Else
                                            intPos = 0
                                        End If
                                        strNamespaceLink = objOpcion.Hipervinculo.Substring(intPos, strNamespace.Length).ToLower
                                        objContenido = objVentanaTab.Content.Content.children.item(0).Content.Content
                                        If objContenido Is Nothing Then
                                            strTipoVentanaLink = String.Empty
                                        Else
                                            strTipoVentanaLink = objContenido.GetType.FullName.ToLower
                                        End If

                                        If strNamespaceLink.Equals(strNamespace) And Not strTipoVentanaLink.Equals(strTipoVentana) Then
                                            ReDim Preserve aobjVentanasEliminar(intNroVentanas)
                                            aobjVentanasEliminar(aobjVentanasEliminar.Length - 1) = objVentanaTab
                                            intNroVentanas += 1
                                        End If
                                    End If
                                End If
                            End If
                        Next

                        For intI As Integer = 0 To aobjVentanasEliminar.Length - 1
                            If Not aobjVentanasEliminar(intI) Is Nothing Then
                                ddcDocumentosTabControl.Items.Remove(aobjVentanasEliminar(intI))
                            End If
                        Next intI
                End Select
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, String.Format("No fue posible cerrar todas las ventanas que están abiertas para la aplicación {0}-{1}. Por favor cierrelas manualmente antes de continuar.", strAplicacion, strVersion), Me.Name, "capturarEventosRegistradosStr", Program.TITULO_CONSOLA, Program.Maquina, ex, Program.RutaServicioLog)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Consultar en el servicio RIA y en la base de datos los parámetros necesarios para la aplicación activa
    ''' </summary>
    ''' 
    Private Async Sub actualizarParametrosApp()

        Dim objApp As A2Utilidades.Aplicacion = Nothing
        Dim logResultado As Boolean = False

        'strAssembly = objApp.Parametros.Item(pstrClave_RefParametros).ToString
        'objParam = Assembly.Load(strAssembly).CreateInstance(strAssembly)
        'objParam = System.Reflection.Assembly.GetExecutingAssembly.CreateInstance(strAssembly)
        Try
            objApp = CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString), A2Utilidades.Aplicaciones).obtenerAplicacion(Program.Aplicacion, Program.VersionAplicacion, Program.Division)

            'If objApp.Parametros.ContainsKey(GSTR_CLAVE_REF_COMPONENTE_APP) Then
            _mlogConsultandoParamConsola = True

            If Program.Aplicacion.ToLower() = "Contabilidad".ToLower() Then ' TEMPORAL - Se debe cambiar a una invocación automática
                'Dim objCtl As A2.Encuenta.Cont.Controles.SL.ParametrosConsola

                'objCtl = New A2.Encuenta.Cont.Controles.SL.ParametrosConsola

                'logResultado = Await objCtl.actualizarParametrosConsola
                'ElseIf Program.Aplicacion.ToLower() = "OYD Server".ToLower() Then
            Else
                Dim objCtl As A2ComunesControl.ParametrosConsola

                objCtl = New A2ComunesControl.ParametrosConsola

                'Se utiliza para desarrollo, falta implementar el codigo para recargar recursos de etiquetas, etc de base de datos
                clsRecursosVisuales.CrearRecursosMensajes(objApp)

                logResultado = Await objCtl.actualizarParametrosConsola
                mlogCargoAssembliesRIA = True
                VerificarBajarBusyConsola()

                If logResultado Then
                    logResultado = Await objCtl.Plataforma_ConsultarCombosGenericos()
                End If
            End If

            _mlogConsultandoParamConsola = False

            If logResultado Then
                lnkVerVersion.IsEnabled = True
            End If
            'End If
        Catch ex As Exception
            Me.bsyConsola.Visibility = Windows.Visibility.Collapsed
            Throw New Exception("Se generó un problema durante la obtención de los datos de la versión de la aplicación.", ex)
        End Try
    End Sub

    Private Function CamtidadMaximaVentanas() As Integer
        Dim intCantidadMaximaVentanas As Integer = MINT_MAX_VENTANAS

        If Application.Current.Resources.Contains("CANTIDADMAXIMAVENTANAS") Then
            intCantidadMaximaVentanas = CInt(Application.Current.Resources("CANTIDADMAXIMAVENTANAS"))
        End If

        Return intCantidadMaximaVentanas
    End Function

#End Region

#Region "Eventos controles"

    ''' <summary>
    ''' Evento que se ejecuta cuando se cambia la aplicación activa en la consola
    ''' </summary>
    ''' <param name="pobjAplicacion">Datos básicos de la aplicación seleccionada</param>
    ''' 
    Private Sub ctlMenuTop_CambioAplicacion(ByVal pobjAplicacion As Menu) Handles ctlMenuTop.CambioAplicacion
        Try
            If Not Me.txtAplicacionActiva.Text.Equals(Program.Aplicacion) Then
                lnkVerVersion.IsEnabled = False
                Me.txtAplicacionActiva.Text = Program.Aplicacion
                _mlogConsultandoParamConsola = False
            End If
        Catch ex As Exception
            Me.txtAplicacionActiva.Text = String.Empty
        End Try
    End Sub

    Private Sub ctlMenuTop_CargoParametrosAplicacion(ByVal plogExitoso As Boolean, ByVal pstrMensajeError As String) Handles ctlMenuTop.CargoParametrosAplicacion
        Try
            If plogExitoso = False Then
                Mensajes.mostrarMensaje(pstrMensajeError, Program.ConsolaTitulo, wppMensajes.TiposMensaje.Advertencia)
            End If

            If _mlogConsultandoParamConsola = False Then
                If Application.Current.Resources.Contains(A2Consola_AppActiva.ToString) Then
                    ' Consultar los parámetro de la aplicación para la consola que se retorna desde el servicio RIA y base de datos
                    Task.Run(Function()
                                 Application.Current.Dispatcher.Invoke(
                                            Function()
                                                Try
                                                    Dim objApp As A2Utilidades.Aplicacion = CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString), A2Utilidades.Aplicaciones).obtenerAplicacion(Program.Aplicacion, Program.VersionAplicacion, Program.Division)
                                                    If Not IsNothing(objApp) Then
                                                        If Not IsNothing(objApp.ListaDLLClienteValidacion) Then
                                                            Dim strVersionCliente As String = String.Empty

                                                            For Each objItemValidar In objApp.ListaDLLClienteValidacion

                                                                Try
                                                                    Dim strRutaArchivo As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, objItemValidar.Nombre)
                                                                    Dim objInfoArchivo As Assembly = System.Reflection.Assembly.LoadFile(strRutaArchivo)

                                                                    If Not IsNothing(objInfoArchivo) Then
                                                                        strVersionCliente = objInfoArchivo.GetName().Version.ToString
                                                                    Else
                                                                        strVersionCliente = objItemValidar.Version
                                                                    End If
                                                                Catch ex As Exception
                                                                    strVersionCliente = objItemValidar.Version
                                                                End Try

                                                                Program.Assembly_ActualizarVersionCliente(objItemValidar, strVersionCliente)
                                                            Next
                                                        End If
                                                    End If

                                                    mlogCargoAssembliesWPF = True
                                                    VerificarBajarBusyConsola()
                                                Catch ex As Exception
                                                    mlogCargoAssembliesWPF = True
                                                    Me.bsyConsola.Visibility = Windows.Visibility.Collapsed
                                                End Try

                                                Return True
                                            End Function
                                            )
                                 Return True
                             End Function)

                    actualizarParametrosApp()
                Else
                    Mensajes.mostrarMensaje(String.Format("No se pudo obtener la información necesaria de la versión de la aplicación {1}, versión {2} desde el servicio RIA.", Program.Aplicacion, Program.VersionAplicacion), Program.ConsolaTitulo, wppMensajes.TiposMensaje.Errores)
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, String.Format("Se generó un error al obtener la información necesaria de la versión de la aplicación {1}, versión {2} desde el servicio RIA.", Program.Aplicacion, Program.VersionAplicacion), Me.Name, "CambioAplicacion", Program.ConsolaTitulo, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ctlMenuTop_ProcesandoAccion(ByVal plogProcesando As Boolean) Handles ctlMenuTop.ProcesandoAccion
        Me.Procesando = plogProcesando

        If mlogMostrarMensajeLog = False Then
            If plogProcesando Then
                Me.bsyConsola.Visibility = Windows.Visibility.Visible
                ctlMenuTop.IsEnabled = False
                lnkInicio.IsEnabled = False
            Else
                ctlMenuTop.IsEnabled = True
                lnkInicio.IsEnabled = True
            End If
        End If
    End Sub

    Private Sub VerificarBajarBusyConsola()
        If mlogCargoAssembliesWPF And mlogCargoAssembliesRIA Then
            Me.bsyConsola.Visibility = Windows.Visibility.Collapsed
            If VerificarAssemblies() = False Then
                cargarVentanaDocumento(STR_VENTANA_INICIO, "/Views/AccesoDenegado.xaml", False, Nothing)
            End If
        End If
    End Sub

    Private Function VerificarAssemblies() As Boolean
        Dim logRetorno As Boolean = True

        If mlogCargoAssembliesWPF And mlogCargoAssembliesRIA Then
            Try
                Dim objApp As A2Utilidades.Aplicacion = CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString), A2Utilidades.Aplicaciones).obtenerAplicacion(Program.Aplicacion, Program.VersionAplicacion, Program.Division)

                If Program.Assembly_VerificarVersiones(objApp.ListaDLLClienteValidacion) = False Then
                    logRetorno = False
                Else
                    If Program.Assembly_VerificarVersiones(objApp.ListaDLLRIAValidacion) = False Then
                        logRetorno = False
                    End If
                End If

                If logRetorno = False Then
                    If Debugger.IsAttached = False Then
                        Dim strMsg As String = String.Empty
                        strMsg = "Existen módulos desactualizados del sistema que no permiten su funcionamiento. "
                        strMsg &= vbNewLine & "Cierre y vuelva a cargar la aplicación, " &
                                    "si aún no se actualiza elimine los archivos temporales y vuelva a ejecutar la aplicación." & vbNewLine & vbNewLine &
                                    "Si el problema persiste por favor comuníquese con el administrador del sistema o con el área de soporte." & vbNewLine

                        Mensajes.mostrarMensaje(strMsg, Program.ConsolaTitulo, wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Catch ex As Exception
                logRetorno = False
                Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al verificar los assemblies.", Me.Name, "VerificarAssemblies", Program.ConsolaTitulo, Program.Maquina, ex)
            End Try
        End If

        Return logRetorno
    End Function


    ''' <summary>
    ''' Se ejecuta cuando el usuario ha seleccionado alguna opción del menú
    ''' </summary>
    ''' <param name="pobjOpcion">Datos de la opción seleccionada por el usuario</param>
    ''' <remarks></remarks>
    Private Sub ctlMenuTop_OpcionSeleccionada(ByVal pobjOpcion As A2Utilidades.Menu) Handles ctlMenuTop.OpcionSeleccionada
        Try
            cargarVentanaDocumento(pobjOpcion.Descripcion, pobjOpcion.Hipervinculo, True, pobjOpcion)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Falló el acceso a la opción seleccionada", Me.Name, "ctlMenuTop_OpcionSeleccionada", Program.TITULO_CONSOLA, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub lnkInicio_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        configurarTabInicio()
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el link "Cerrar sesión". Elimina la información de las aplicaciones de la consola y oculta el menú
    ''' </summary>
    ''' <param name="sender">Objeto que genera el evento</param>
    ''' <param name="e">Parámetros generados por el evento</param>
    ''' <remarks></remarks>
    ''' 
    Private Sub lnkCerrarSesion_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim intI As Integer
        Dim intApp As Integer
        Dim objApps As A2Utilidades.Aplicaciones
        Dim objApp As A2Utilidades.Aplicacion
        'Dim objVentana As Divelements.SandDock.DocumentWindow = Nothing
        Dim objVentana As System.Windows.Controls.TabItem = Nothing

        Try
            'JDCP
            '***************************************************************************
            '-- Cerrar tabs activos
            'For Each objVentana In Me.dstContenido.GetAllWindows()
            '    objVentana.Close()
            'Next
            Dim intTotalColumnas As Integer

            intTotalColumnas = Me.ddcDocumentosTabControl.Items.Count

            If intTotalColumnas > 1 Then
                intTotalColumnas = intTotalColumnas - 1
                For i = intTotalColumnas To 1 Step -1
                    Me.ddcDocumentosTabControl.Items.RemoveAt(i)
                Next
            End If
            '***************************************************************************

            '-- Ocultar menú
            Me.ctlMenuTop.Visibility = Windows.Visibility.Collapsed

            '-- Eliminar aplicaciones registradas en la consola
            If Application.Current.Resources.Contains(RecursosApp.A2Consola_Aplicaciones.ToString) Then
                objApps = CType(Application.Current.Resources(RecursosApp.A2Consola_Aplicaciones.ToString), A2Utilidades.Aplicaciones)
                intI = objApps.LstAplicaciones.Count
                intApp = 0
                Do While intI > 0
                    objApp = objApps.LstAplicaciones.ElementAt(intApp).Value
                    If objApp.Nombre.ToLower().Contains(Program.NOMBRE_CONSOLA.ToLower()) Then
                        '-- Se mantiene la aplicación correspondiente a la consola
                        intApp += 1
                    Else
                        objApps.LstAplicaciones.Remove(objApps.LstAplicaciones.ElementAt(intApp).Key)
                    End If
                    intI -= 1
                Loop
            End If

            '-- Ocultar link a Inicio y cierre de sesión
            Me.lnkInicio.Visibility = Visibility.Collapsed
            Me.txtInicialNombreUsuario.Visibility = Visibility.Collapsed
            Me.lnkVerVersion.Visibility = Visibility.Collapsed

            '-- Mostrar mensaje de fin de sesión
            cargarVentanaDocumento(STR_VENTANA_CERRARSESSION, "/Views/FinSesion.xaml", False, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo finalizar el proceso de cierre de sesión", Me.Name, "lnkCerrarSesion_Click", Program.TITULO_CONSOLA, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando la pantalla de login se cierra
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub mobjLogin_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjLogin.Closed
        If mobjLogin.LoginValido Then
            CargarMenu()

            '// Nombre del usuario que aparece en el encabezado
            Me.txtInicialNombreUsuario.Text = Program.NombreUsuario

            '// Datos de la barra de estado
            Me.txtLoginActivo.Text = Program.Usuario

            configurarTabInicio()
            Me.txtInicialNombreUsuario.Visibility = Windows.Visibility.Visible
        Else
            cargarVentanaDocumento(STR_VENTANA_INICIO, "/Views/AccesoDenegado.xaml", False, Nothing)
        End If
    End Sub

    Private Sub mobjTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjTimer.Tick
        Try
            Me.txtFecha.Text = String.Format("{0}, {1}", Now().ToLongDateString(), Now.ToShortTimeString())
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lnkVerVersion_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim objWin = New AcercaDe
            Program.Modal_OwnerMainWindowsPrincipal(objWin)
            objWin.ShowDialog()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub cmdNotificacion_OnClick()

        If VisibilidadNotificaciones = Windows.Visibility.Collapsed Then
            VisibilidadNotificaciones = Windows.Visibility.Visible
            VisibilidadMensajesConsola = Windows.Visibility.Collapsed
            VisibilidadDebug = Windows.Visibility.Collapsed
        Else
            VisibilidadNotificaciones = Windows.Visibility.Collapsed
        End If

    End Sub

    Public Sub cmdMensajes_OnClick()

        If VisibilidadMensajesConsola = Windows.Visibility.Collapsed Then
            VisibilidadNotificaciones = Windows.Visibility.Collapsed
            VisibilidadMensajesConsola = Windows.Visibility.Visible
            VisibilidadDebug = Windows.Visibility.Collapsed
        Else
            VisibilidadMensajesConsola = Windows.Visibility.Collapsed
        End If

    End Sub

    Public Sub cmdDebug_OnClick()

        If VisibilidadDebug = Windows.Visibility.Collapsed Then
            VisibilidadNotificaciones = Windows.Visibility.Collapsed
            VisibilidadMensajesConsola = Windows.Visibility.Collapsed
            VisibilidadDebug = Windows.Visibility.Visible
        Else
            VisibilidadDebug = Windows.Visibility.Collapsed
        End If

    End Sub

    Private Sub Consola_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)

        If e.Key = Key.N And Keyboard.Modifiers = ModifierKeys.Control + ModifierKeys.Alt Then
            'MUESTRA EL LINK DE LAS NOTIFICACIONES.
            VisibilidadLinkNotificaciones = Windows.Visibility.Visible
        ElseIf e.Key = Key.D And Keyboard.Modifiers = ModifierKeys.Control + ModifierKeys.Alt Then
            'MUESTRA EL LINK DEL DEBUG.
            VisibilidadLinkDebug = Windows.Visibility.Visible
        End If

    End Sub


#End Region

#Region "Manejadores de eventos ventanas"

    ''' <summary>
    ''' Permite identificar cuando una ventana cambia de foco. Se puede identificar la ventana nueva y la anterior.
    ''' </summary>
    ''' <remarks>Mediante el objeto sender</remarks>
    Private Sub ddcDocumentosTabControl_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim objVentana As System.Windows.Controls.TabItem = Nothing
        Dim objOpcion As Menu

        Try
            If Not e Is Nothing Then
                If e.AddedItems.Count > 0 Then
                    ' Nueva ventana seleccionada
                    If TypeOf e.AddedItems.Item(0) Is System.Windows.Controls.TabItem Then
                        objVentana = CType(e.AddedItems.Item(0), System.Windows.Controls.TabItem)
                        If Not IsNothing(objVentana) Then
                            If TypeOf objVentana.Header Is clsTabItemConsola Then
                                Dim prueba = e.AddedItems
                                _mstrVentanaActiva = objVentana.Name

                                If objVentana.Tag Is Nothing Then
                                    Dim strTextoTabInicio As String = "Inicio"
                                    If Not String.IsNullOrEmpty(A2ConsolaWPF.MySettings.Default.TextoFondoTabPrincipal) Then
                                        strTextoTabInicio = A2ConsolaWPF.MySettings.Default.TextoFondoTabPrincipal
                                    End If

                                    If Not objVentana.Header.Equals(String.Empty) And Not objVentana.Header.Equals(strTextoTabInicio) Then
                                        'MessageBox.Show(objVentana.Header.ToString)
                                        ' Se envía un texto con formato  [Separador][NombreEvento][Separador][NamespaceEliminar][Separador][Aplicacion][Separador][Version][Separador][TipoVentana], donde
                                        ' [Separador]: |
                                        ' [NombreEvento] : CerrarVentanasAplicacion
                                        ' [NamespaceEliminar] : A2.OyDNet
                                        ' [Aplicacion] : Program.Aplicacion
                                        ' [Version] : Program.VersionAplicacion
                                        ' [TipoControl] : Me.GetType.FullName

                                        strMensaje = String.Format("{0}{1}{0}{2}{0}{3}{0}{4}{0}{5}", "|", objVentana.Header, "A2.OyDNet", Program.Aplicacion, Program.VersionAplicacion, Me.GetType.FullName)
                                        Messenger.Default.Send(strMensaje)

                                    End If
                                    Me.txtAplicacionActiva.Text = Program.TITULO_CONSOLA
                                Else
                                    objOpcion = CType(objVentana.Tag, Menu)

                                    '// TEMPORAL mientras se estructuran los toolbar
                                    Dim strToolbar As String
                                    If objOpcion.Toolbar.Trim.Equals(String.Empty) Then
                                        strToolbar = Recursos.ToolbarEstandar
                                    Else
                                        strToolbar = objOpcion.Toolbar
                                    End If
                                    '// FIN TEMPORAL

                                    '// Siempre se cambia la aplicación para garantizar que la aplicación activa corresponda a la opción cargada en la ventana que se activa
                                    Program.cambiarAplicacionActiva(objOpcion.Aplicacion, objOpcion.Version, objOpcion.Division, False, "")
                                    Me.txtAplicacionActiva.Text = Program.Aplicacion
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, String.Format("Se presentó un problema al activar la pestaña de la opción seleccionada (pestaña {0})", _mstrVentanaActiva), Me.Name, "dstContenido_LastActiveWindowChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


#End Region

#Region "Implementado por Rafael Cordero"

    'CORREC_CITI_SV_2014
    'Implementado por Rafael Cordero - Marzo 16 de 2012
    Private Sub LoadURLBrowserDefault(pstrUri As String, plogTargetNueva As Boolean)
        'se Modifica Para correr fuera del browser
        Program.VisorArchivosWeb_CargarURL(pstrUri)
    End Sub


#End Region

    Public Function TabConsola_AddItem(ByVal pHearder As String, ByVal pURLOrigen As String, ByVal pobTabItemActual As System.Windows.Controls.TabItem) As clsTabItemConsola
        Dim newTabItem As clsTabItemConsola = New clsTabItemConsola(Me, pobTabItemActual)
        newTabItem.Header = pHearder
        newTabItem.UrlOrigen = pURLOrigen
        newTabItem.IsSelected = True
        Return newTabItem
    End Function

    Public Sub TabConsola_RemoveItem(ByVal ptabItem As clsTabItemConsola)
        Try
            TabConsola_EliminarTabItem(ptabItem.TabItem)

            Dim objNotificacionCliente As New clsNotificacionCliente()
            Dim objNotificacion As New clsNotificacion()
            objNotificacion.dtmFechaEnvio = Now
            objNotificacion.intConsecutivo = 1
            objNotificacion.strIdConexion = String.Empty
            objNotificacion.strInfoMensaje = ptabItem.UrlOrigen
            objNotificacion.strMaquina = Program.Maquina
            objNotificacion.strMensajeConsola = String.Empty
            objNotificacion.strTipoMensaje = "A2CONSOLA_CERRARVENTANA"
            objNotificacion.strTopicos = "NOTIFICACIONPROPIA"
            objNotificacion.strUsuario = Program.Usuario
            objNotificacion.strUsuariosNotificacion = Program.Usuario
            objNotificacion.strRecibirNotificacionPropia = "SI"

            objNotificacionCliente.objInfoNotificacion = objNotificacion

            EnviarNotificacion(objNotificacionCliente)
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al remover la pestaña.", Me.Name, "TabConsola_RemoveItem", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TabConsola_EliminarTabItem(ByVal pobjTabItem As System.Windows.Controls.TabItem)
        '// Disminuir el número de ventanas cargas
        Try
            mintNroVentanas -= 1

            RemoveHandler CType(CType(pobjTabItem, System.Windows.Controls.TabItem).Content, upgNavegacion).wfrContenido.NavigationFailed, AddressOf CType(CType(pobjTabItem, System.Windows.Controls.TabItem).Content, upgNavegacion).wfrContenido_NavigationFailed
            CType(CType(pobjTabItem, System.Windows.Controls.TabItem).Content, upgNavegacion).wfrContenido = Nothing
            CType(pobjTabItem, System.Windows.Controls.TabItem).Content = Nothing
            GC.Collect()

            ddcDocumentosTabControl.Items.Remove(pobjTabItem)

            'lanza la notificación de la ventana que se esta cerrando
        Catch ex As Exception
            mintNroVentanas = 0
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al remover la pestaña.", Me.Name, "TabConsola_EliminarTabItem", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#Region "Comunicación Notificaciones SignalR"

    ''' <summary>
    ''' Inicializa los datos de Notificaciones 
    ''' </summary>
    ''' <remarks>Abril 07/2014</remarks>
    Private Sub InicializarNotificacionesSinalR()
        Try

            objClienteNotificaciones = New clsClienteSignalR(Program.URLNotificaciones, True)

            objClienteNotificaciones.logFiltrarMensajesPropios = True

            AddHandler objClienteNotificaciones.LlegoMensaje, AddressOf LlegoMensajeSignalR
            AddHandler objClienteNotificaciones.OcurrioError, AddressOf ocurrioErrorSignalR
            AddHandler objClienteNotificaciones.ConexionExitosa, AddressOf ConexionExitosa
            AddHandler objClienteNotificaciones.EstadoCambio, AddressOf CambioEstado

        Catch ex As Exception
            Debug(String.Format("Se presentó un error mientras inicializaba la conexión con el servidor de notificaciones. {0}", ex.Message))
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la ejecución del proceso de inicialización del servidor de notificaciones SignalR",
            '                                             Me.Name, "InicializarNotificacionesSinalR", Program.TITULO_CONSOLA, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' Recibe el mensaje de la notificación
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Abril 07/2014</remarks>
    Private Sub LlegoMensajeSignalR(sender As Object, e As EventArgs)
        Dim objInfoNotificacion As clsNotificacion

        Try
            Application.Current.Dispatcher.Invoke(
                                            Function()
                                                Try
                                                    objInfoNotificacion = DirectCast(sender, clsNotificacion)
                                                    LogNotificaciones.Add(objInfoNotificacion.ToString)
                                                    If Not String.IsNullOrWhiteSpace(objInfoNotificacion.strMensajeConsola) Then

                                                        ListaMensajesConsola.Insert(0, New NotificacionConsola With {.Texto = objInfoNotificacion.strMensajeConsola, .Fecha = Now})

                                                        VisibilidadMensajesConsola = Windows.Visibility.Visible
                                                        'DockMensajes.

                                                    End If

                                                    If STR_TOPICO_CONSOLA = objInfoNotificacion.strTopicos Then
                                                        'NOTIFICACION PARA LA CONSOLA.


                                                    Else
                                                        'SE NOTIFICA A LAS DEMAS PANTALLAS

                                                        Messenger.Default.Send(objInfoNotificacion)
                                                    End If
                                                Catch ex As Exception
                                                    Debug(String.Format("Se presentó un problema al recibir el mensaje de notificación. {0}", ex.Message))
                                                End Try

                                                Return True
                                            End Function
                                            )

        Catch ex As Exception
            Application.Current.Dispatcher.Invoke(
                                            Function()
                                                Try
                                                    Debug(String.Format("Se presentó un problema al recibir el mensaje de notificación. {0}", ex.Message))
                                                Catch ex1 As Exception

                                                End Try
                                                Return True
                                            End Function
                                            )
        End Try
    End Sub


    Private Sub ocurrioErrorSignalR(sender As Object, e As EventArgs)
        Application.Current.Dispatcher.Invoke(
                                            Function()
                                                Try
                                                    Debug(String.Format("Se presentó con la comunicación al servicio de  notificación. {0}", DirectCast(sender, Exception).Message))
                                                Catch ex1 As Exception

                                                End Try
                                                Return True
                                            End Function
                                            )
    End Sub

    Private Sub ConexionExitosa(sender As Object, e As EventArgs)
        Application.Current.Dispatcher.Invoke(
                                            Function()
                                                Try
                                                    Debug("Conexión al servicio de notificaciones exitosa.")
                                                Catch ex1 As Exception

                                                End Try
                                                Return True
                                            End Function
                                            )
    End Sub

    Private Sub CambioEstado(sender As Object, e As EventArgs)
        Application.Current.Dispatcher.Invoke(
                                            Function()
                                                Try
                                                    If Not IsNothing(sender) Then
                                                        Debug(String.Format("Cambio de estado de la notificación. {0}", CType(sender, Microsoft.AspNet.SignalR.Client.StateChange).NewState.ToString))
                                                    Else
                                                        Debug("Cambio de estado de la notificación. Sin estado")
                                                    End If
                                                Catch ex1 As Exception

                                                End Try
                                                Return True
                                            End Function
                                            )
    End Sub

    Private Sub EnviarNotificacion(pobjInfoNotificacionCliente As clsNotificacionCliente)
        Try

            If objClienteNotificaciones IsNot Nothing Then

                objClienteNotificaciones.EnviarNotificacion(pobjInfoNotificacionCliente.objInfoNotificacion)

            End If

        Catch ex As Exception
            Debug(String.Format("Se presentó un error al enviar la notificación. {0}", ex.Message))
            'CONTROL DE ERRORES
        End Try

    End Sub

#End Region

#Region "Debug"

    Private Sub RegistrarDebug(pobjDebug As clsDebug)

        If pobjDebug IsNot Nothing Then

            Debug(pobjDebug.strMensajeDebug)

        End If

    End Sub

    Private Sub Debug(pstrMensaje As String)

        If Not String.IsNullOrWhiteSpace(pstrMensaje) Then

            DebugCollection.Add(pstrMensaje)

        End If

    End Sub

#End Region

#Region "Notify Property"

    Private Sub CambioPropiedad(ByVal pstrNombrePropiedad As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(pstrNombrePropiedad))
    End Sub

#End Region

#Region "Animación"

    Private Sub AnimacionLista_Completed(sender As Object, e As EventArgs)
        VisibilidadMensajesConsola = Windows.Visibility.Collapsed
    End Sub

    Private Sub lnkAlcuadrado_Click(sender As Object, e As RoutedEventArgs)
        Dim strUrlVinculo As String = CType(sender, Button).Tag
        System.Diagnostics.Process.Start(strUrlVinculo)
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        cmdDebug_OnClick()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        cmdMensajes_OnClick()
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        cmdNotificacion_OnClick()
    End Sub

#End Region

End Class

