Imports System.Deployment.Application
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Security
Imports System.Security.Principal
Imports System.Web
Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports Telerik.Windows.Controls

Class MainWindow

    Private logHabilitarTestSeguridad As Boolean = False
    Private strUsuarioClienteTestSeguridad As String = String.Empty
    Private strIPClienteTestSeguridad As String = String.Empty
    Private pstrOpcionCargarMainWindow As String = String.Empty

    Public Sub New()
        Try

            'StyleManager.ApplicationTheme = New Telerik.Windows.Controls.TransparentTheme()
            'StyleManager.ApplicationTheme = New Telerik.Windows.Controls.Office_BlueTheme(
            'StyleManager.ApplicationTheme = New Telerik.Windows.Controls.Windows7Theme()
            'StyleManager.ApplicationTheme = New Telerik.Windows.Controls.SummerTheme()
            StyleManager.ApplicationTheme = New Telerik.Windows.Controls.Windows8Theme()


            '===PALETA DE COLORES===

            'StrongColor
            '-Titulos de columnas
            '-Fondo de opciones de menú
            '-Fondo fila hover
            '-Etiquetas 

            'BasicColor
            '-Fondo fila seleccionada
            '-Borde menú        '

            'MainColor
            '-Fondo de datagrid
            '-Fondo pestañas
            '-Fondo menú

            'MarkerColor
            '-Fuente del menú
            '-Fuente de la pestaña
            '-Fuente textos por fuera de los controles
            '-Fuente título de grupos
            '-Fuente de textos en datagrid



            Windows8Palette.Palette.AccentColor = Color.FromRgb(46, 106, 170)       'azul principal
            Windows8Palette.Palette.BasicColor = Color.FromRgb(190, 190, 190)       'gris medio
            Windows8Palette.Palette.MainColor = Color.FromRgb(250, 250, 250)        'gris claro
            Windows8Palette.Palette.MarkerColor = Color.FromRgb(50, 50, 50)         'gris fuerte
            Windows8Palette.Palette.StrongColor = Color.FromRgb(46, 106, 170)       'azul principal
            'Windows8Palette.Palette.AccentColor




            ' This call is required by the designer.
            InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al inicializar la consola de ejecución de aplicaciones.", Me.ToString(), "MainWindow_New", "", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            CargarRecursoWindow()
            InicializarAplicacion()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al inicializar la consola de ejecución de aplicaciones.", Me.ToString(), "MainWindow_Loaded", "", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub MainWindow_UnLoaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            Application.Current.Shutdown()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al inicializar la consola de ejecución de aplicaciones.", Me.ToString(), "MainWindow_Loaded", "", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub InicializarAplicacion()
        Dim logExito As Boolean = True
        Dim intPosAppActiva As Byte = 0
        Dim strMultiaplicacion As String = String.Empty
        Dim strAplicacion As String = String.Empty
        Dim strVersion As String = String.Empty
        Dim strDivision As String = String.Empty
        Dim strURLServicioLog As String = String.Empty
        Dim strMsgError As String = String.Empty
        Dim objUsr As A2Utilidades.Usuario
        Dim objApps As A2Utilidades.Aplicaciones
        Dim objInitParam As New Dictionary(Of String, String)
        Dim logAbrirTestSeguridad As Boolean = False
        Dim strContenidoConfiguracion As String = String.Empty
        Dim strArchivoConfiguracion As String = String.Empty
        Dim strConfiguracionServiciosOffLine As String = String.Empty

        Try
            strArchivoConfiguracion = A2ConsolaWPF.MySettings.Default.A2VArchivoConfigRpts
            strConfiguracionServiciosOffLine = A2ConsolaWPF.MySettings.Default.ServiciosOffline

            If Not String.IsNullOrEmpty(strConfiguracionServiciosOffLine) Then
                If Strings.Left(strArchivoConfiguracion, 4).ToLower = "http" Or Strings.Left(strArchivoConfiguracion, 5).ToLower = "https" Then
                    Dim strNombreArchivoConfiguracion As String = strArchivoConfiguracion
                    For Each nombre In strArchivoConfiguracion.Split("/")
                        If Not (String.IsNullOrEmpty(nombre)) Then
                            strArchivoConfiguracion = nombre
                        End If
                    Next
                End If
            End If

            strContenidoConfiguracion = Replace(LeerArchivo(strConfiguracionServiciosOffLine, strArchivoConfiguracion), """", "'")

            '// Recuperar la configuración para continuar con el procesamiento
            Application.Current.Resources.Add(RecursosApp.A2Consola_OffLine.ToString(), strConfiguracionServiciosOffLine)
            objInitParam.Add(RecursosApp.A2Consola_OffLine.ToString(), strConfiguracionServiciosOffLine)
            objInitParam.Add("USS", A2ConsolaWPF.MySettings.Default.URLServicioSeguridad)
            objInitParam.Add("USSAPI", A2ConsolaWPF.MySettings.Default.URLServicioSeguridadApi)
            objInitParam.Add("USSAPID", A2ConsolaWPF.MySettings.Default.URLServicioSeguridadApi_DirectorioVirtual)

            If String.IsNullOrEmpty(A2ConsolaWPF.MySettings.Default.URLServicioLog) Then
                objInitParam.Add("APL", A2ConsolaWPF.MySettings.Default.URLServicioSeguridad)
            Else
                objInitParam.Add("APL", A2ConsolaWPF.MySettings.Default.URLServicioLog)
            End If
            objInitParam.Add("UI", A2ConsolaWPF.MySettings.Default.SeguridadIntegrada)
            objInitParam.Add("APC", A2ConsolaWPF.MySettings.Default.Aplicacion)
            objInitParam.Add("APV", A2ConsolaWPF.MySettings.Default.Version)
            objInitParam.Add("ML", A2ConsolaWPF.MySettings.Default.MostrarSeguimiento)
            objInitParam.Add("APD", String.Empty)
            objInitParam.Add("UL", WindowsIdentity.GetCurrent().Name)
            objInitParam.Add("UN", WindowsIdentity.GetCurrent().Name)
            objInitParam.Add("UW", WindowsIdentity.GetCurrent().Name)
            objInitParam.Add("UM", Environment.MachineName)
            objInitParam.Add("SL", A2ConsolaWPF.MySettings.Default.SugerirLogin)
            objInitParam.Add("USN", A2ConsolaWPF.MySettings.Default.UrlNotificaciones)
            objInitParam.Add("UMC", A2ConsolaWPF.MySettings.Default.UrlMotorCalculos)
            objInitParam.Add("US_SIN_DOMINIO", A2ConsolaWPF.MySettings.Default.US_SIN_DOMINIO)
            objInitParam.Add("ConsolaVer", A2ConsolaWPF.MySettings.Default.ConsolaVer)
            objInitParam.Add("MA", A2ConsolaWPF.MySettings.Default.Multiaplicacion)
            objInitParam.Add("A2VXMLConfigRpts", strContenidoConfiguracion)


            Try
                If Debugger.IsAttached = False Then
                    If Not IsNothing(ApplicationDeployment.CurrentDeployment) Then
                        Dim objVersionConsola As System.Version = ApplicationDeployment.CurrentDeployment.CurrentVersion

                        If Not IsNothing(objVersionConsola) Then
                            Dim strVersionConsolaLocal As String = objVersionConsola.Major.ToString() + "." + objVersionConsola.Minor.ToString() + "." + objVersionConsola.Build.ToString() + "." + objVersionConsola.Revision.ToString()
                            objInitParam.Add(RecursosApp.A2Consola_VersionLocal.ToString(), strVersionConsolaLocal)
                            Application.Current.Resources.Add(RecursosApp.A2Consola_VersionLocal.ToString(), objInitParam(RecursosApp.A2Consola_VersionLocal.ToString()))
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try

            'inicialaizarOutOfBrowserDebug(objInitParam)
            If objInitParam.ContainsKey(Recursos.ParametrosConsola.USS.ToString()) Then 'USS: URLServicioSeguridad
                Application.Current.Resources.Add(RecursosApp.A2Consola_ServicioSeguridad.ToString(), objInitParam(Recursos.ParametrosConsola.USS.ToString()))
            Else
                logExito = False
                strMsgError = "No se tiene la configuración del servicio de seguridad de Alcuadrado S.A."
            End If

            If objInitParam.ContainsKey(Recursos.ParametrosConsola.USSAPI.ToString()) Then 'USS: URLServicioSeguridadAPI
                Application.Current.Resources.Add(RecursosApp.A2Consola_ServicioSeguridadAPI.ToString(), objInitParam(Recursos.ParametrosConsola.USSAPI.ToString()))
            Else
                logExito = False
                strMsgError = "No se tiene la configuración del servicio de seguridad api de Alcuadrado S.A."
            End If

            If objInitParam.ContainsKey(Recursos.ParametrosConsola.USSAPID.ToString()) Then 'USS: URLServicioSeguridadAPI DIRECTORIO
                Application.Current.Resources.Add(RecursosApp.A2Consola_APIDirectorioVirtual.ToString(), objInitParam(Recursos.ParametrosConsola.USSAPID.ToString()))
            Else
                logExito = False
                strMsgError = "No se tiene la configuración del directorio de seguridad api de Alcuadrado S.A."
            End If

            If logExito Then
                ' -- Validar si se recibió el parámetro que indica si la consola debe trabajar o no en modo multiaplicación
                If objInitParam.ContainsKey(Recursos.ParametrosConsola.MA.ToString()) Then
                    strMultiaplicacion = objInitParam(Recursos.ParametrosConsola.MA.ToString())
                    If strMultiaplicacion.Trim() = "0" Then
                        Program.Multiaplicacion = False
                    Else
                        Program.Multiaplicacion = True
                    End If
                Else
                    Program.Multiaplicacion = True
                End If

                If objInitParam.ContainsKey(Recursos.ParametrosAplicacion.APL.ToString()) Then 'APL: URLServicioLOG
                    strURLServicioLog = objInitParam(Recursos.ParametrosAplicacion.APL.ToString())
                    Application.Current.Resources.Add(RecursosApp.A2Consola_RutaLog.ToString(), strURLServicioLog)
                End If

                '-- Validar si se recibió el pará metro que indica si se muestra un mensaje de log mientras carga la aplicación.
                '   Se activa para ayudar a depurar la carga del sistema en las estaciones cliente
                If objInitParam.ContainsKey(Recursos.ParametrosConsola.ML.ToString()) Then
                    Application.Current.Resources.Add(RecursosApp.A2Consola_MostrarLog.ToString(), objInitParam(Recursos.ParametrosConsola.ML.ToString()))
                End If

                '-- Validar si se recibió el parámetro que indica si se sugiere el nombre de usuario cuando la seguridad no es integrada.
                If objInitParam.ContainsKey(Program.PARAM_SUGERIR_LOGIN) Then
                    If objInitParam(Program.PARAM_SUGERIR_LOGIN) = "1" Then
                        glogSugerirLogin = True
                    Else
                        glogSugerirLogin = False
                    End If
                End If

                '-- Guardar los datos del usuario activo
                objUsr = New A2Utilidades.Usuario(CType(objInitParam, Dictionary(Of String, String)))
                Application.Current.Resources.Add(RecursosApp.A2Consola_UsuarioActivo.ToString(), objUsr)
                objUsr = Nothing

                '-- Guardar los datos de la consola
                If objInitParam.ContainsKey(Recursos.ParametrosConsola.ConsolaVer.ToString()) Then
                    Program.VERSION_CONSOLA = objInitParam(Recursos.ParametrosConsola.ConsolaVer.ToString())
                End If

                Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_ConsolaTitulo.ToString(), Program.TITULO_CONSOLA)
                Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_ConsolaVersion.ToString(), Program.VERSION_CONSOLA)

                If objInitParam.ContainsKey(Program.USA_USUARIO_SIN_DOMINIO) Then
                    If objInitParam(Program.USA_USUARIO_SIN_DOMINIO) = "1" Then
                        glogUsaUsuarioSinDominio = True
                    Else
                        glogUsaUsuarioSinDominio = False
                    End If
                Else
                    glogUsaUsuarioSinDominio = False
                End If

                Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_UsaUsuarioSinDominio.ToString(), glogUsaUsuarioSinDominio)

                If objInitParam.ContainsKey(Program.MOSTRARDETALLEERROR_USUARIO) Then
                    If objInitParam(Program.MOSTRARDETALLEERROR_USUARIO) = "1" Or objInitParam(Program.MOSTRARDETALLEERROR_USUARIO) = "SI" Then
                        glogMostrarDetalleErrorUsuario = True
                    Else
                        glogMostrarDetalleErrorUsuario = False
                    End If
                Else
                    glogMostrarDetalleErrorUsuario = False
                End If

                Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_MostrarDetalleError.ToString(), glogMostrarDetalleErrorUsuario)

                ' Crear lista de aplicaciones
                objApps = New A2Utilidades.Aplicaciones

                ' Adicionar la consola como primera aplicación en la colección de aplicaciones
                objApps.adicionarAplicacion(0, Program.NOMBRE_CONSOLA, Program.VERSION_CONSOLA, Program.TITULO_CONSOLA, Program.TITULO_CONSOLA, CType(objInitParam, Dictionary(Of String, String)), Nothing, True, Recursos.ToolbarEstandar, "", strURLServicioLog, "", "")
                '-- Guardar en los recursos de la aplicación los datos de las aplicaciones definidas
                Application.Current.Resources.Add(RecursosApp.A2Consola_Aplicaciones.ToString(), objApps)
                '-- Cargar la consola como aplicación activa
                Application.Current.Resources.Add(RecursosApp.A2Consola_AppActiva.ToString(), objApps.LstAplicaciones.Values.ElementAt(0))
                '-- Guardar el toolabar activo
                Application.Current.Resources.Add(RecursosApp.A2Consola_ToolbarActivo.ToString(), objApps.LstAplicaciones.Values.ElementAt(0).obtenerToolbarTexto(Recursos.ToolbarEstandar))

                Application.Current.Resources.Add("CANTIDADMAXIMAVENTANAS", A2ConsolaWPF.MySettings.Default.CantidadVentanasPermitidas)

                objApps = Nothing

                ' Validar si la consola funcionará en modo multiaplicación o monoaplicación. Si el parámetro strMultiaplicacion es diferente de cero se asume multiaplicación.
                If strMultiaplicacion = "0" Then
                    ' Verificar si se recibió el nombre y versión de la aplicación que se trabajará en la consola
                    If Not objInitParam(ParametrosAplicacion.APC.ToString()) Is Nothing Then
                        strAplicacion = objInitParam(ParametrosAplicacion.APC.ToString())
                    End If

                    If Not objInitParam(ParametrosAplicacion.APV.ToString()) Is Nothing Then
                        strVersion = objInitParam(ParametrosAplicacion.APV.ToString())
                    End If

                    If Not objInitParam(ParametrosAplicacion.APD.ToString()) Is Nothing Then
                        strDivision = objInitParam(ParametrosAplicacion.APD.ToString())
                    End If

                    Application.Current.Resources.Add(GSTR_RECURSO_MONOAPLICACION, strAplicacion & Program.SeparadorListas & strVersion & Program.SeparadorListas & strDivision)
                End If

                'Obtiene el nombre de la maquina y la graba como un recurso principal
                If objInitParam.ContainsKey(Program.GSTR_MAQUINAUSUARIO) Then
                    Application.Current.Resources.Add(Program.GSTR_MAQUINAUSUARIO, objInitParam(Program.GSTR_MAQUINAUSUARIO))
                End If

                '''' TEMPORAL para el visor de reportes
                If objInitParam.ContainsKey("A2VXMLConfigRpts") Then 'XML de configuración
                    Application.Current.Resources.Add("A2VXMLConfigRpts", objInitParam("A2VXMLConfigRpts"))
                End If
                '''' FIN TEMPORAL para el visor de reportes

                'URL Para el servidor de notificaciones
                If objInitParam.ContainsKey(Recursos.ParametrosConsola.USN.ToString()) Then 'USN: URLNotificaciones
                    Application.Current.Resources.Add(RecursosApp.A2Consola_Notificaciones.ToString(), objInitParam(Recursos.ParametrosConsola.USN.ToString()))
                End If

                'URL Para el servidor de Motor de Calculos
                If objInitParam.ContainsKey(Recursos.ParametrosConsola.UMC.ToString()) Then 'UMC: UrlMotorCalculos
                    Application.Current.Resources.Add(RecursosApp.A2Consola_MotordeCalculos.ToString(), objInitParam(Recursos.ParametrosConsola.UMC.ToString()))
                End If

                If objInitParam.ContainsKey("TESTSEGURIDAD") Then
                    If objInitParam("TESTSEGURIDAD") = "SI" Then
                        logAbrirTestSeguridad = True
                    End If
                End If

                'Me.RootVisual = New MainPage
                If logAbrirTestSeguridad Then
                    logHabilitarTestSeguridad = True
                    strUsuarioClienteTestSeguridad = objInitParam("HAUSUSUARIO")
                    strIPClienteTestSeguridad = objInitParam("HAIPCLIENTE")
                End If

                'Seguridad
                If objInitParam.ContainsKey(Recursos.ParametrosConsola.CIFK.ToString()) Then 'CIFK:
                    Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_CIFK.ToString(), objInitParam(Recursos.ParametrosConsola.CIFK.ToString()))
                End If
                If objInitParam.ContainsKey(Recursos.ParametrosConsola.CIFKC.ToString()) Then 'CIFKC:
                    Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_CIFKC.ToString(), objInitParam(Recursos.ParametrosConsola.CIFKC.ToString()))
                End If
                If objInitParam.ContainsKey(Recursos.ParametrosConsola.CIFSK.ToString()) Then 'CIFSK:
                    Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_CIFSK.ToString(), objInitParam(Recursos.ParametrosConsola.CIFSK.ToString()))
                End If
                If objInitParam.ContainsKey(Recursos.ParametrosConsola.CIFP.ToString()) Then 'CIFP:
                    Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_CIFP.ToString(), objInitParam(Recursos.ParametrosConsola.CIFP.ToString()))
                End If

                'Crear recurso para saber sí se debe de validar versión de los assemblies.
                If A2ConsolaWPF.MySettings.Default.ValidarVersionAssemblies = "1" Then
                    Application.Current.Resources.Add("A2Consola_ValidarVersionAssemblies", True)
                Else
                    Application.Current.Resources.Add("A2Consola_ValidarVersionAssemblies", False)
                End If

                CargarPantalla(New A2Consola)
            End If

            If logExito = False Then
                CargarPantalla(New Inicio)
                If Not String.IsNullOrEmpty(strMsgError) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsgError, Program.TITULO_CONSOLA, wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            CargarPantalla(New Inicio)
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al inicializar la consola de ejecución de aplicaciones.", Me.ToString(), "Application_Startup", "", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CargarRecursoWindow()
        If Application.Current.Resources.Contains("MainWindowsPrincipalAnchoTotal") Then
            Application.Current.Resources.Remove("MainWindowsPrincipalAnchoTotal")
        End If
        If Application.Current.Resources.Contains("MainWindowsPrincipalAltoTotal") Then
            Application.Current.Resources.Remove("MainWindowsPrincipalAltoTotal")
        End If

        Application.Current.Resources.Add("MainWindowsPrincipalAnchoTotal", Me.ActualWidth)
        Application.Current.Resources.Add("MainWindowsPrincipalAltoTotal", Me.ActualHeight)

        Program.Modal_CargarWindowsPrincipal(Me)
    End Sub

    Private Sub CargarPantalla(ByVal objPantallaCargar As Object)
        Me.LayoutRoot.Children.Clear()
        Me.LayoutRoot.Children.Add(objPantallaCargar)
    End Sub

    Private Function LeerArchivo(ByVal pstrRutaArchivo As String, ByVal pstrArchivo As String) As String
        If (Not String.IsNullOrEmpty(pstrRutaArchivo)) Then
            Return Program.LeerArchivoFisico(pstrRutaArchivo, pstrArchivo)
        Else
            Return Program.DescargarArchivoHTTP(pstrArchivo)
        End If
    End Function
End Class
