Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports System.ComponentModel
Imports System
Imports System.IO
Imports SpreadsheetGear
Imports A2MCCOREWPF
Imports System.Text.RegularExpressions
Imports System.Windows.Navigation
Imports C1.Silverlight
Imports SpreadsheetGear.Windows
Imports A2.Notificaciones.Cliente
Imports GalaSoft.MvvmLight.Messaging


Partial Public Class ConfiguracionMotorCaluloView
    Inherits UserControl
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Dim _vm As MotorCalculoViewModel
    Private mlogInicializar As Boolean = True

    Private _ConsultaBorrar As Consulta
    Public Property ConsultaBorrar() As Consulta
        Get
            Return _ConsultaBorrar
        End Get
        Set(ByVal value As Consulta)
            _ConsultaBorrar = value
        End Set
    End Property

    Private _FilaConsultaBorrar As Integer
    Public Property FilaConsultaBorrar() As Integer
        Get
            Return _FilaConsultaBorrar
        End Get
        Set(ByVal value As Integer)
            _FilaConsultaBorrar = value
        End Set
    End Property

    Public Sub New()
        Try
            InitializeComponent()
            _vm = New MotorCalculoViewModel
            Me.DataContext = _vm

            Me.Name = Me.GetType.Name
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de configuración de motor de cálculo", Me.Name, "New", "ConfiguracionMotorCaluloView", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub



#Region "Eventos"

    Private Async Sub ConfiguracionMotorCaluloView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then

                mlogInicializar = False

                Me.Width = Application.Current.MainWindow.Width * 0.96

                Await Inicializar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento ParametrosRiesgos_Loaded", Me.ToString(), "ParametrosRiesgos_Loaded", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub dgMetodos_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        e.Handled = True
    End Sub

    Private Sub dgVersiones_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        e.Handled = True
    End Sub

    Private Async Sub btnNuevoVersion_Click(sender As Object, e As RoutedEventArgs)
        If _vm.MetodoSeleccionado Is Nothing Then
            mostrarMensaje("Debe seleccionar un método para crear una nueva versión", "Nueva versión", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            'valido si existen version pendientes de aprobar
            If _vm.ListaVersionesPorMetodo.Where(Function(i) Not i.Aprobado).Count > 0 Then
                mostrarMensaje("Existen versiones pendientes de aprobar, no se puede continuar!", "Nueva versión", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return
            End If
            If _vm.BKRequiereAutorizacion AndAlso _vm.ConfiguracionAutorizaciones Is Nothing Then
                Await CargarConfiguraciones()
            End If
            Dim cwNuevaVersion As NuevaVersionView = New NuevaVersionView(_vm)
            cwNuevaVersion.ShowDialog()
        End If
    End Sub

    Private Sub btnBorrarMetodo_Click(sender As Object, e As RoutedEventArgs)
        If _vm.MetodoSeleccionado Is Nothing Then
            mostrarMensaje("Debe seleccionar un método", "Borrar método", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borrará el registro seleccionado. ¿Confirma el borrado de este método?", Program.TituloSistema, "borrar_metodo", AddressOf TerminoMensajePregunta)
        End If
    End Sub

    Private Sub btnBorrarVersion_Click(sender As Object, e As RoutedEventArgs)
        If _vm.MetodoSeleccionado Is Nothing Then
            mostrarMensaje("Debe seleccionar un método", "Borrar versión", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If _vm.VersionSeleccionada Is Nothing Then
                mostrarMensaje("Debe seleccionar una versión", "Borrar versión", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borrará el registro seleccionado. ¿Confirma el borrado de esta versión?", Program.TituloSistema, "borrar_versiones", AddressOf TerminoMensajePregunta)
            End If
        End If
    End Sub

    Private Sub txtURLNotificaciones_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Space Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtURLAutorizaciones_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Space Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnGrabar_Click(sender As Object, e As RoutedEventArgs)
        Try
            'Dim listaConsultas As List(Of ConsultaValidacion) = ConsultaValidacion.ConvertirALista(rangoConsulta)
            Dim Validaciones As String = String.Empty
            Dim cont As Integer = 0
            Dim DatosValidos As Boolean = True
            If Not esUrlValida(txtURLNotificaciones.Text) Then
                Validaciones = String.Format("{0} + La url configurada para las notificaciones no es válida. {1}", Environment.NewLine, Environment.NewLine)
                DatosValidos = False
            End If
            If chkRequiereAutorizacion.IsChecked AndAlso Not esUrlValida(txtURLAutorizaciones.Text) Then
                Validaciones = String.Format("{0} + La url configurada para las autorizaciones no es válida. {1}", Environment.NewLine, Environment.NewLine)
                DatosValidos = False
            End If

            'Dim ValidacionesConsultas As String = ValidarConsultas()
            'If Not String.IsNullOrEmpty(ValidacionesConsultas) Then
            '    Validaciones = ValidacionesConsultas
            '    DatosValidos = False
            'End If

            'For Each item In listaConsultas
            '    Dim ValidacionRegistro As String = String.Empty
            '    Dim url As String = item.strURL
            '    Dim descripcion As String = item.strDescripcion
            '    Dim rango As String = item.strRango
            '    Dim escalar As Boolean = item.logEscalar
            '    Dim intervalo As String = item.intIntervalo
            '    Dim lstDependencia As List(Of String) = item.lstDependencias
            '    Dim lstMetodos As List(Of String) = item.lstMetodos

            '    If Not Int32.TryParse(intervalo.ToString(), Nothing) Then
            '        ValidacionRegistro = String.Format("{0}{1} + El intervalo debe ser numérico.", ValidacionRegistro, Environment.NewLine)
            '        DatosValidos = False
            '    End If

            '    If Not esUrlValida(url) Then
            '        ValidacionRegistro = String.Format("{0}{1} + La url configurada no es válida.", ValidacionRegistro, Environment.NewLine)
            '        DatosValidos = False
            '    End If
            '    If Not String.IsNullOrEmpty(ValidacionRegistro) Then

            '        ValidacionRegistro = String.Format("{0}{1}{2}{3}", "El Registro " & cont.ToString() & " presentó las siguientes inconsistencias: ", Environment.NewLine, ValidacionRegistro, Environment.NewLine)
            '        Validaciones = String.Format("{0}{1}{2}", Validaciones, Environment.NewLine, ValidacionRegistro)
            '    End If

            '    cont = cont + 1
            'Next

            If DatosValidos Then
                'LibroConfiguracion.Names(Program.RANGO_NOTIFICACIONES).RefersToRange.Value = txtURLNotificaciones.Text
                'LibroConfiguracion.Names(Program.RANGO_AUTORIZACIONES).RefersToRange.Value = txtURLAutorizaciones.Text
                'LibroConfiguracion.Names(Program.RANGO_REQUIEREAUTORIZACION).RefersToRange.Value = chkRequiereAutorizacion.IsChecked
                'Dim resultado = Await _vm.ActualizarRegistroLibro(LibroConfiguracion)
                'If resultado Then
                '    Await _vm.CargarLibroConfiguracion()
                '    _vm.BKRequiereAutorizacion = chkRequiereAutorizacion.IsChecked
                '    mostrarMensaje("Datos actualizados exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                'End If
            Else
                mostrarMensaje(String.Format("Por favor verifique las siguientes inconsistencias antes de guardar: {0}{1} ", Environment.NewLine, Validaciones), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al guardar los cambios del libro de configuración.", Me.ToString(), "btnGrabar_Click", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValidarRangoConsultas() As Boolean
        Dim valido As Boolean = False

        Return valido
    End Function

    'Private Function ValidarConsultas() As String
    '    Dim strMensaje As String = String.Empty

    '    Dim objMotorSL As clsCORE = New clsCORE(LibroConfiguracion, System.Globalization.CultureInfo.CurrentCulture)
    '    Dim rangoConsulta As String = objMotorSL.ObtenerValorRango(Program.RANGO_CONSULTAS, Program.TABLA, False)
    '    Dim listaConsultas As List(Of Consulta) = Consulta.ConvertirALista(rangoConsulta)

    '    For Each item In listaConsultas

    '        Dim consultasRepetidas = (From itemConsulta In listaConsultas Where itemConsulta.strURL.ToString().Trim().ToUpper() = item.strURL.ToString().Trim().ToUpper()).ToList()

    '        If consultasRepetidas IsNot Nothing Then
    '            If consultasRepetidas.Count > 1 Then
    '                strMensaje = String.Format("{0}{1} + La consulta {2} esta registrada mas de una vez en el libro.", strMensaje, Environment.NewLine, item.strURL)
    '                Exit For
    '            End If
    '        End If
    '    Next

    '    Dim URLVacio = (From item In listaConsultas Where item.strURL Is Nothing Or item.strURL = String.Empty).ToList()

    '    If URLVacio IsNot Nothing Then
    '        If URLVacio.Count > 0 Then
    '            strMensaje = String.Format("{0}{1} + El campo nombre es obligatorio.", strMensaje, Environment.NewLine)
    '        End If
    '    End If

    '    Dim DescripcionVacio = (From item In listaConsultas Where item.strDescripcion Is Nothing Or item.strDescripcion = String.Empty).ToList()

    '    If DescripcionVacio IsNot Nothing Then
    '        If DescripcionVacio.Count > 0 Then
    '            strMensaje = String.Format("{0}{1} + El campo descripción es obligatorio.", strMensaje, Environment.NewLine)
    '        End If
    '    End If

    '    Dim RangoVacio = (From item In listaConsultas Where item.strRango Is Nothing Or item.strRango = String.Empty).ToList()

    '    If RangoVacio IsNot Nothing Then
    '        If RangoVacio.Count > 0 Then
    '            strMensaje = String.Format("{0}{1} + El campo rango es obligatorio.", strMensaje, Environment.NewLine)
    '        End If
    '    End If

    '    CargarConsultas(LibroConfiguracion)

    '    Return strMensaje
    'End Function

    Private Async Sub btnDescargarVersion_Click(sender As Object, e As RoutedEventArgs)
        Try

            If _vm.MetodoSeleccionado Is Nothing Then
                mostrarMensaje("Debe seleccionar un método", "Descargar versión", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If _vm.VersionSeleccionada Is Nothing Then
                    mostrarMensaje("Debe seleccionar una versión", "Descargar versión", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    Dim sf As New SaveFileDialog()
                    sf.Filter = "Libro de Excel|*.xlsx|Libro de Excel 97-2003|*.xls"
                    sf.FileName = _vm.MetodoSeleccionado.Nombre & "." & _vm.VersionSeleccionada.Version

                    If (sf.ShowDialog) Then

                        If sf.SafeFileName IsNot String.Empty Then

                            _vm.IsBusy = True
                            Dim ArchivoExcel As IWorkbook = Await _vm.DecargarArchivoVersion()
                            ArchivoExcel.WorkbookSet.GetLock()
                            Try
                                Dim objByteFile As Byte() = ArchivoExcel.SaveToMemory(FileFormat.OpenXMLWorkbook)
                                Using fs As Stream = sf.OpenFile()
                                    fs.Write(objByteFile, 0, objByteFile.Length)
                                    fs.Close()
                                End Using
                            Finally
                                ArchivoExcel.WorkbookSet.ReleaseLock()
                            End Try
                            _vm.IsBusy = False
                            mostrarMensaje("El archivo de versión se descargó correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

                        End If
                    End If
                End If
            End If

        Catch fi As IOException
            _vm.IsBusy = False
            mostrarMensaje("El archivo no se puede subir esta siendo utilizado por otro proceso, verifique que no este abierto", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        Catch ex As Exception
            _vm.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del cuadro de dialogo de apertura",
                                 Me.ToString(), "ConfiguracionMotorCaluloView.btnDescargarVersion_Click", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnInfoVersion_Click(sender As Object, e As RoutedEventArgs)
        Dim cwInfoVersion As InfoVersionView = New InfoVersionView(_vm.VersionSeleccionada)
        cwInfoVersion.ShowDialog()
    End Sub


#End Region

#Region "Procedimientos"

    Private Async Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Select Case CType(sender, A2Utilidades.wppMensajePregunta).CodigoLlamado.ToLower
                    Case "borrar_metodo"
                        'Borrar metodo
                        Dim resultado = Await _vm.EliminarMetodo()
                        If resultado Then
                            mostrarMensaje("El registro fue eliminado exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        End If

                    Case "borrar_versiones"
                        'Borrar versión
                        Dim resultado = Await _vm.EliminarVersion()
                        If resultado Then
                            mostrarMensaje("El registro fue eliminado exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        End If
                    Case "cancelar_version"
                        Dim resultado = Await _vm.CancelarVersion(_vm.VersionSeleccionada)
                        If resultado Then
                            mostrarMensaje("La versión fue cancelada exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        End If
                    Case "seleccionar_version"
                        Dim resultado = _vm.SeleccionarVersion(_vm.VersionSeleccionada)
                        If resultado Then
                            mostrarMensaje("La versión fue cambiada exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        End If
                        'Case "borrar_consulta"
                        '    Dim LibroConfiguracion As IWorkbook = wvLibroExcel.ActiveWorkbook

                        '    LibroConfiguracion.WorkbookSet.GetLock()
                        '    Try
                        '        Dim objMotorSL As clsCORE = New clsCORE(LibroConfiguracion, System.Globalization.CultureInfo.CurrentCulture)
                        '        Dim rangoConsulta As String = objMotorSL.ObtenerValorRango(Program.RANGO_CONSULTAS, Program.TABLA, False)
                        '        Dim listaConsultas As List(Of Consulta) = Consulta.ConvertirALista(rangoConsulta)

                        '        FilaConsultaBorrar = FilaConsultaBorrar + 2
                        '        LibroConfiguracion.ActiveWorksheet.Cells("A" & FilaConsultaBorrar.ToString() & ":Z" & FilaConsultaBorrar.ToString()).EntireRow().Delete()

                        '    Finally
                        '        LibroConfiguracion.WorkbookSet.ReleaseLock()
                        '    End Try

                        '    CargarConsultas(LibroConfiguracion)
                End Select
            Else
                _vm.IsBusy = False
            End If
        Catch ex As Exception
            _vm.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "TerminoMensajePreguntaMetodo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function MostrarOcultarPropiedadesDelLibro(ByVal libro As IWorkbook, ByVal mostrar As Boolean) As IWorkbook
        Try

            libro.WorkbookSet.GetLock()
            Try
                libro.ActiveWorksheet.WindowInfo.DisplayHeadings = mostrar

                libro.WindowInfo.DisplayHorizontalScrollBar = mostrar
                libro.WindowInfo.DisplayVerticalScrollBar = mostrar
                libro.WindowInfo.DisplayWorkbookTabs = mostrar
            Finally
                libro.WorkbookSet.ReleaseLock()
            End Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al mostrar/ocultar las propiedades del libro.", Me.ToString(), "MostrarOcultarPropiedadesDelLibro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return libro
    End Function

    'Private Sub CargarConsultas(ByVal libroConfiguracion As IWorkbook)

    '    'Dim libroConfiguracion As IWorkbook = _vm.LibroConfiguracion

    '    libroConfiguracion.WorkbookSet.GetLock()
    '    Try
    '        Dim objMotorSL As clsCORE = New clsCORE(libroConfiguracion, System.Globalization.CultureInfo.CurrentCulture)
    '        wvLibroExcel.ActiveWorkbook = libroConfiguracion
    '        wvLibroExcel.DisplayReference = Program.RANGO_CONSULTAS_TITULOS
    '        Dim windowInfoEncabezados As SpreadsheetGear.IWorksheetWindowInfo = wvLibroExcel.ActiveWorksheetWindowInfo
    '        windowInfoEncabezados.DisplayHeadings = False
    '        Dim windowInfoHojaYScroball As SpreadsheetGear.IWorkbookWindowInfo = wvLibroExcel.ActiveWorkbookWindowInfo
    '        windowInfoHojaYScroball.DisplayHorizontalScrollBar = True
    '        windowInfoHojaYScroball.DisplayVerticalScrollBar = True
    '        windowInfoHojaYScroball.DisplayWorkbookTabs = False

    '        wvLibroExcel.ActiveWorksheet.Cells.Columns.AutoFit()
    '    Finally
    '        libroConfiguracion.WorkbookSet.ReleaseLock()
    '    End Try

    'End Sub

    Public Async Function CargarConfiguraciones() As Threading.Tasks.Task
        txtURLNotificaciones.Text = Await _vm.ObtenerRangoLibroConfiguracion(Program.RANGO_NOTIFICACIONES)
        Dim strRequiereAutorizacion As String = Await _vm.ObtenerRangoLibroConfiguracion(Program.RANGO_REQUIEREAUTORIZACION)

        If strRequiereAutorizacion IsNot Nothing Then
            chkRequiereAutorizacion.IsChecked = False
        Else
            If strRequiereAutorizacion.ToUpper = "SI" Then
                chkRequiereAutorizacion.IsChecked = True
            Else
                chkRequiereAutorizacion.IsChecked = False
            End If
        End If

        txtURLAutorizaciones.Text = Await _vm.ObtenerRangoLibroConfiguracion(Program.RANGO_AUTORIZACIONES)

        _vm.BKRequiereAutorizacion = chkRequiereAutorizacion.IsChecked

        If _vm.BKRequiereAutorizacion Then
            'cargo la configuracion de autorizaciones
            Await _vm.ConsultarConfiguracionAutorizaciones()
        End If
    End Function

    Public Async Function CargarBotonesTooltip() As Threading.Tasks.Task
        Try
            _vm.IsBusy = True
            Await _vm.ConsultarBotonesMC()

            _vm.IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los datos", Me.ToString(), "CargarDatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Async Function CargarDatos() As Threading.Tasks.Task
        Dim strErrorSeguimiento As String = ""
        Try
            _vm.IsBusy = True
            strErrorSeguimiento = "-Consultar Botones"
            Await _vm.ConsultarBotonesMC()
            Await _vm.ObtenerListaMetodos()
            strErrorSeguimiento = "-Consultar Lista versiones"
            Await _vm.ObtenerListaVersiones()
            strErrorSeguimiento = "-Cargar consultas"
            'CargarConsultas(_vm.LibroConfiguracion)
            strErrorSeguimiento = "-Cargar configuraciones"
            Await CargarConfiguraciones()
            _vm.IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los datos", Me.ToString(), "CargarDatos" & strErrorSeguimiento, Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Async Function Inicializar() As Threading.Tasks.Task
        Await CargarDatos()
    End Function

    Private Function esUrlValida(strUrl As String) As Boolean
        Return Regex.IsMatch(strUrl, "(((http(s?))\://){1}\S+)")
    End Function

    Private Sub dgVersiones_LoadingRow(sender As Object, e As DataGridRowEventArgs)

        '    Dim index As Integer = e.Row.GetIndex()

        '    Dim contextMenu = New ContextMenu
        '    Dim objData As clsInfoVersion = e.Row.DataContext
        '    If objData IsNot Nothing Then

        '        'si se tiene permiso de cancelar version se adiciona al menu
        '        If _vm.mnuCancelarVersionVisibility Then
        '            'si el estado es pendiente de aprobar y es el mismo usuario se permite retirar el documento
        '            If objData.Usuario = Program.Usuario AndAlso Not objData.Aprobado Then
        '                Dim mnuCancelarVersion = New MenuItem With {.Header = "Cancelar versión"}
        '                AddHandler mnuCancelarVersion.Click, AddressOf mnuCancelarVersion_Click
        '                contextMenu.Items.Add(mnuCancelarVersion)
        '            End If
        '        End If

        '        'si se tiene permiso de reasignar version se adiciona al menu
        '        If _vm.mnuSeleccionarVersionVisibility Then
        '            'si es el mismo assembly, ya esta aprobado y es diferente a la version actual
        '            If _vm.AssemblyMC = objData.Assembly AndAlso objData.Aprobado AndAlso _vm.MetodoSeleccionado.Version <> objData.Version Then
        '                Dim mnuSeleccionarVersion = New MenuItem With {.Header = "Seleccionar versión"}
        '                AddHandler mnuSeleccionarVersion.Click, AddressOf mnuSeleccionarVersion_Click
        '                contextMenu.Items.Add(mnuSeleccionarVersion)
        '            End If
        '        End If

        '    End If
        '    If contextMenu.Items.Count > 0 Then
        '        ContextMenuService.SetContextMenu(e.Row, contextMenu)
        '    Else
        '        C1ContextMenuService.SetContextMenu(e.Row, New C1ContextMenu) 'evitar el menu de silverlight
        '    End If
    End Sub

    Private Sub mnuCancelarVersion_Click(sender As Object, e As RoutedEventArgs)
        Dim objMenu As MenuItem = sender
        Dim objData As clsInfoVersion = objMenu.DataContext
        _vm.VersionSeleccionada = objData
        A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción retira el documento del sistema de autorizaciones y borrará el registro seleccionado. ¿Confirma la cancelación de esta versión?", Program.TituloSistema, "cancelar_version", AddressOf TerminoMensajePregunta)
    End Sub

    Private Sub mnuSeleccionarVersion_Click(sender As Object, e As RoutedEventArgs)
        Dim objMenu As MenuItem = sender
        Dim objData As clsInfoVersion = objMenu.DataContext
        _vm.VersionSeleccionada = objData
        A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción cambia la version actual " & _vm.MetodoSeleccionado.Version & " del metodo: " & _vm.MetodoSeleccionado.Nombre & ", por la versión: " & _vm.VersionSeleccionada.Version & ". ¿Confirma el cambio de versión?", Program.TituloSistema, "seleccionar_version", AddressOf TerminoMensajePregunta)
    End Sub

#End Region


    'Private Sub btnNuevaConsulta_Click(sender As Object, e As RoutedEventArgs)

    '    Dim validacionConsultas As String = ValidarConsultas()

    '    If String.IsNullOrEmpty(validacionConsultas) Then

    '        Dim LibroConfiguracion As IWorkbook = wvLibroExcel.ActiveWorkbook

    '        LibroConfiguracion.WorkbookSet.GetLock()
    '        Try
    '            Dim objMotorSL As clsCORE = New clsCORE(LibroConfiguracion, System.Globalization.CultureInfo.CurrentCulture)
    '            Dim rangoConsulta As String = objMotorSL.ObtenerValorRango(Program.RANGO_CONSULTAS, Program.TABLA, False)
    '            Dim listaConsultas As List(Of Consulta) = Consulta.ConvertirALista(rangoConsulta)

    '            Dim nuevaConsulta As New Consulta

    '            Dim urlPorDefecto As String = "http://"
    '            If listaConsultas.Count > 0 Then
    '                urlPorDefecto = listaConsultas(0).strURL.Substring(0, listaConsultas(0).strURL.LastIndexOf("/"))
    '            End If

    '            nuevaConsulta.strURL = urlPorDefecto
    '            listaConsultas.Add(nuevaConsulta)

    '            objMotorSL.InsertarValorEnRango(Program.RANGO_CONSULTAS, Consulta.ConvertirAArray(nuevaConsulta))
    '            objMotorSL.InsertarValorEnRango(Program.RANGO_CONSULTAS_TITULOS, Consulta.ConvertirAArray(nuevaConsulta))

    '            CargarConsultas(LibroConfiguracion)
    '        Finally
    '            LibroConfiguracion.WorkbookSet.ReleaseLock()
    '        End Try

    '    Else
    '        mostrarMensaje(String.Format("Por favor verifique las siguientes inconsistencias antes de agregar un nuevo registro: {0}{1} ", Environment.NewLine, validacionConsultas), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '    End If


    'End Sub

    'Private Sub btnBorrarConsulta_Click(sender As Object, e As RoutedEventArgs)
    '    If ConsultaBorrar IsNot Nothing Then
    '        A2Utilidades.Mensajes.mostrarMensajePregunta(String.Format("Está opción eliminará la consulta {0} del rango.", ConsultaBorrar.strURL), Program.TituloSistema, "borrar_consulta", AddressOf TerminoMensajePregunta, False, "¿Confirma el borrado de la consulta?")
    '    End If
    'End Sub

End Class
