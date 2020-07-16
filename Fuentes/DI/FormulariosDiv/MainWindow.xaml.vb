Imports System.ComponentModel
Imports A2Utilidades
Class MainWindow
	Implements INotifyPropertyChanged
	Dim objPantallaVisualizar As Object = Nothing
	Private mlogInicializar As Boolean = True
	Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

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

	Public Sub New()
		' This call is required by the designer.
		InitializeComponent()
		Me.DataContext = Me
		IsBusy = True
	End Sub

	Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
		Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                inicializar()

            End If
            CargarRecursoWindow()
        Catch ex As Exception

		End Try
	End Sub

    Private Sub CargarRecursoWindow()
        If Application.Current.Resources.Contains("MainWindowsPrincipalAnchoTotal") Then
            Application.Current.Resources.Remove("MainWindowsPrincipalAnchoTotal")
        End If
        If Application.Current.Resources.Contains("MainWindowsPrincipalAltoTotal") Then
            Application.Current.Resources.Remove("MainWindowsPrincipalAltoTotal")
        End If

        Application.Current.Resources.Add("MainWindowsPrincipalAnchoTotal", Me.Width)
        Application.Current.Resources.Add("MainWindowsPrincipalAltoTotal", Me.Height)

        Program.Modal_CargarWindowsPrincipal(Me)

    End Sub


    Private Async Sub inicializar()
		IsBusy = True
        'creación recursos
        Dim strArchivoConfiguracion As String = "\\a2webdllo\SitiosWebDllo\OyDPlat_WPF\OYDConsola\Reportes_OYDNet.xml"
        Dim strContenidoConfiguracion As String = Replace(LeerArchivo(strArchivoConfiguracion), """", "'")

        Await clsRecursos.Crear(Environment.GetCommandLineArgs(), strArchivoConfiguracion, strContenidoConfiguracion)
        IsBusy = False
	End Sub

	Private Sub CargarMenus(plogOpcionNuevoBorrar As Boolean)

		Dim permisosBotones As New List(Of String)
		permisosBotones.Add("Forma|Forma|Pasar a modo forma")
		permisosBotones.Add("Buscar|Buscar|Buscar un registro")
		permisosBotones.Add("Editar|Editar|Editar registro seleccionado")

		permisosBotones.Add("Aprobar|Aprobar|Aprobar modificaciones al registro")
		permisosBotones.Add("Rechazar|Rechazar|Rechazar modificaciones al registro")
		permisosBotones.Add("Version|Versión|Versión")

		If plogOpcionNuevoBorrar Then
			permisosBotones.Add("Nuevo|Nuevo|Ingresar nuevo registro")
			permisosBotones.Add("Borrar|Borrar|Borrar el registro seleccionado")

		End If
		Application.Current.Resources("A2Consola_ToolbarActivo") = permisosBotones
	End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim pstrOpcionMenu As String = CType(sender, MenuItem).Tag

        GridPrincipal.Children.Clear()
        objPantallaVisualizar = Nothing

        If pstrOpcionMenu = "Formulario1" Then
            CargarMenus(False)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario1View()

        ElseIf pstrOpcionMenu = "Formulario2" Then
            CargarMenus(False)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario2View()

        ElseIf pstrOpcionMenu = "Formulario3" Then
            CargarMenus(False)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario3View()

        ElseIf pstrOpcionMenu = "Formulario4" Then
            CargarMenus(False)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario4View()

        ElseIf pstrOpcionMenu = "Formulario5" Then
            CargarMenus(False)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario5View()

        ElseIf pstrOpcionMenu = "Formulario6" Then
            CargarMenus(True)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario6View()

        ElseIf pstrOpcionMenu = "Formulario7" Then
            CargarMenus(True)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario7View()

            'ElseIf pstrOpcionMenu = "Ordenes" Then
            '    CargarMenus(True)
            '    objPantallaVisualizar = New A2OrdenesDivisasWPF.OrdenesView()

            'ElseIf pstrOpcionMenu = "Modulos" Then
            '    CargarMenus(True)
            '    objPantallaVisualizar = New A2OrdenesDivisasWPF.ModulosView()

            'ElseIf pstrOpcionMenu = "ModuloConfiguracion" Then
            '    CargarMenus(True)
            '    objPantallaVisualizar = New A2OrdenesDivisasWPF.ModulosEstadosConfiguracionView()

            'ElseIf pstrOpcionMenu = "AjustesMesas" Then
            '    CargarMenus(True)
            '    objPantallaVisualizar = New A2OrdenesDivisasWPF.AjustesMesasView()

            'ElseIf pstrOpcionMenu = "CierreDivisas" Then
            '    CargarMenus(True)
            '    objPantallaVisualizar = New A2OrdenesDivisasWPF.CierreDivisasView()

        ElseIf pstrOpcionMenu = "DestinosInverForm" Then
            CargarMenus(True)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.DestinosInvFormulariosView()

            'reportes
        ElseIf pstrOpcionMenu = "Reporte_AjustesMesa" Then
            CargarMenus(True)

            Dim rptVisor As New A2VisorReportes.ReportViewer()
            rptVisor.cargarPordemanda = True
            rptVisor.configurarReporte("/Divisas/AjustesXMesa")

            objPantallaVisualizar = rptVisor

        ElseIf pstrOpcionMenu = "Consultar_UtilidadPerdida" Then
            CargarMenus(True)

            Dim rptVisor As New A2VisorReportes.ReportViewer()
            rptVisor.cargarPordemanda = True
            rptVisor.configurarReporte("/Divisas/ConsultarUtilidadPerdida")

            objPantallaVisualizar = rptVisor

        ElseIf pstrOpcionMenu = "Imprimir_UtilidadPerdida" Then
            CargarMenus(True)

            Dim rptVisor As New A2VisorReportes.ReportViewer()
            rptVisor.cargarPordemanda = True
            rptVisor.configurarReporte("/Divisas/ImprimirUtilidadPerdida")

            objPantallaVisualizar = rptVisor


            'JAPC20180823: se valida si el menu es constancia operaciones 
        ElseIf pstrOpcionMenu = "ConstanciaOperaciones" Then
            CargarMenus(True)

            Dim rptVisor As New A2VisorReportes.ReportViewer()
            rptVisor.cargarPordemanda = True
            rptVisor.configurarReporte("/Divisas/ConstanciaOperaciones")

            objPantallaVisualizar = rptVisor

            'JAPC20181009: Se valida si el menu es Exportación movimiento DIAN
        ElseIf pstrOpcionMenu = "ExportacionMovDIAN" Then
            CargarMenus(True)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.ExportacionMovDIANView()

        ElseIf pstrOpcionMenu = "ArchivosBancoRepublica" Then
            CargarMenus(True)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.GenerarBcoRepublicaView()

            'RABP20181010: Reporte de Spread comisión
        ElseIf pstrOpcionMenu = "SpreadComision" Then
            CargarMenus(True)

            Dim rptVisor As New A2VisorReportes.ReportViewer()
            rptVisor.cargarPordemanda = True
            rptVisor.configurarReporte("/Divisas/SpreadComision")

            objPantallaVisualizar = rptVisor

        ElseIf pstrOpcionMenu = "UIAF" Then
            CargarMenus(True)
            objPantallaVisualizar = New A2FormulariosDivisasWPF.ReporteMensualUIAFView()

            'ElseIf pstrOpcionMenu = "CargueOperaciones" Then
            '    CargarMenus(True)
            '    objPantallaVisualizar = New A2OrdenesDivisasWPF.ImportacionOperacionesSETFXView()

            'ElseIf pstrOpcionMenu = "Cumplimiento" Then
            '    CargarMenus(True)
            '    objPantallaVisualizar = New A2OrdenesDivisasWPF.CumplimientoView()

        End If

        GridPrincipal.Children.Add(objPantallaVisualizar)
    End Sub

    Private Function LeerArchivo(ByVal pstrArchivo As String) As String
        Dim strFileContents As String = String.Empty

        If My.Computer.FileSystem.FileExists(pstrArchivo) Then
            Try
                strFileContents = My.Computer.FileSystem.ReadAllText(pstrArchivo)
                Return strFileContents
            Catch ex As Exception
                Throw New Exception(String.Format("No se pudo cargar el archivo de configuración de reportes.({0}). Error: {1}", pstrArchivo, ex.Message))
                Exit Function
            End Try
        Else
            Throw New Exception(String.Format("No se pudo cargar el archivo de configuración de reportes.({0})", pstrArchivo))
            Exit Function
        End If
    End Function

End Class
