Imports A2Utilidades.Mensajes
Imports System.ComponentModel
Imports System
Imports System.IO
Imports A2MCCOREWPF
Imports SpreadsheetGear
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports Microsoft.VisualBasic
Imports Microsoft.Win32

Partial Public Class NuevaVersionView
    Inherits Window

    Public _vm As MotorCalculoViewModel
    Dim archivo As Stream

    Private _NuevaVersion As IWorkbook
    Public Property NuevaVersion As IWorkbook
        Get
            Return _NuevaVersion
        End Get
        Set(ByVal value As IWorkbook)
            _NuevaVersion = value
        End Set
    End Property

    Public Sub New(vm As MotorCalculoViewModel)
        Try
            InitializeComponent()
            _vm = vm
            Me.DataContext = _vm

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de nueva versión", Me.Name, "New", "NuevaVersionView", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub NuevaVersionView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            Me.DataContext = _vm
            _vm.TituloMetodo = _vm.MetodoSeleccionado.Nombre

            If _vm.ConfiguracionAutorizaciones IsNot Nothing Then
                Dim index As Integer = -1
                Dim cont As Integer = 0
                For Each item In _vm.ConfiguracionAutorizaciones.Prioridades
                    If _vm.PrioridadPorDefecto.IDPrioridad = item.IDPrioridad Then
                        index = cont
                        Exit For
                    End If
                    cont = cont + 1
                Next

                If index <> -1 Then
                    cbxPrioridades.SelectedIndex = index
                End If
            End If

            If _vm.NuevaVersion Is Nothing Then
                _vm.NuevaVersion = New clsInfoVersion()
            End If

            If _vm.ConfiguracionAutorizaciones IsNot Nothing Then
                _vm.NuevaVersion.Aprobado = False
                _vm.NuevaVersion.Autorizador = String.Empty
            Else
                _vm.NuevaVersion.Aprobado = True
                _vm.NuevaVersion.Autorizador = Program.Usuario
            End If

            _vm.NuevaVersion.FechaInicial = Now
            _vm.NuevaVersion.FechaFinal = Now
            _vm.NuevaVersion.Metodo = _vm.MetodoSeleccionado.Nombre
            _vm.NuevaVersion.Observaciones = String.Empty
            _vm.NuevaVersion.Usuario = Program.Usuario
            _vm.NuevaVersion.Assembly = _vm.AssemblyMC

            If Not IsNothing(_vm.ListaVersionesPorMetodo) Then
                If _vm.ListaVersionesPorMetodo.Count > 0 Then
                    Dim objProximo As clsInfoVersion = (From item In _vm.ListaVersionesPorMetodo
                                                        Where item.Aprobado
                                                        Select item Order By item.Version Descending).FirstOrDefault()

                    If IsNothing(objProximo) Then
                        _vm.NuevaVersion.Version = "1"
                    Else
                        _vm.NuevaVersion.Version = CStr(CInt(objProximo.Version) + 1)
                    End If
                Else
                    _vm.NuevaVersion.Version = "1"
                End If
            Else
                _vm.NuevaVersion.Version = "1"
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de nueva versión", Me.Name, "NuevaVersionView_Loaded", "NuevaVersionView", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As RoutedEventArgs)
        Try

            Dim fu As OpenFileDialog = New OpenFileDialog

            fu.Filter = "Libro de Excel|*.xlsx|Libro de Excel 97-2003|*.xls"

            fu.Multiselect = False
            If (fu.ShowDialog) Then

                txtURLArchivo.Text = fu.FileName
                Dim workbookSet As SpreadsheetGear.IWorkbookSet = SpreadsheetGear.Factory.GetWorkbookSet(System.Globalization.CultureInfo.CurrentCulture)

                Using fileStream As System.IO.Stream = fu.OpenFile
                    _vm.NuevoLibroVersion = workbookSet.Workbooks.OpenFromStream(fileStream)
                    fileStream.Close()
                End Using

            End If

        Catch fi As IOException
            _vm.IsBusy = False
            MensajesControl("El archivo no se puede subir esta siendo utilizado por otro proceso, verifique que no este abierto")
            txtURLArchivo.Text = String.Empty
        Catch ex As Exception
            _vm.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del cuadro de dialogo de apertura",
                                 Me.ToString(), "NuevaVersionView.btnSeleccionar_Click", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub MensajesControl(pstrmensaje As String)
        mostrarMensaje(pstrmensaje, Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub

    Private Function ValidarLibro() As Boolean

        Dim libroValido As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            If String.IsNullOrEmpty(c1Notas.Text) Then
                strMsg = String.Format("{0}{1} + Debe especificar las observaciones.", strMsg, Environment.NewLine)
                libroValido = False
            End If

            If String.IsNullOrEmpty(txtURLArchivo.Text) Then
                strMsg = String.Format("{0}{1} + Debe de seleccionar un archivo .xlsx.", strMsg, Environment.NewLine)
                libroValido = False
            End If

            If _vm.MetodoSeleccionado.Online Then

                If _vm.NuevoLibroVersion IsNot Nothing Then

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_ACCIONES) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango TBL_ACCIONES.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_LISTA_ALERTAS) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango A2_LISTA_ALERTAS.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_LISTA_TIPO_ALERTA) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango A2_LISTA_TIPOALERTA.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_LISTA_BOLEANA) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango A2_LISTABOLEANA.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_LISTA_PRESENTACION) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango A2_LISTAPRESENTACION.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_BANDAS) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango A2_TBL_BANDAS.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_COMPONENTES) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango A2_TBL_COMPONENTES.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_POSICIONES) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango A2_TBL_COMPONENTES_PROPIEDADES.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_POSICIONES) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango A2_TBL_PARAMETROS_CONFIGURACION.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_PARAMETROS_CONFIGURACION) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango A2_TBL_PARAMETROS_CONFIGURACION.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_PARAMETROS_VISUALIZACION) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango TBL_PARAMETROS_VISUALIZACION.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                    If _vm.NuevoLibroVersion.Names(Program.RANGO_PARAMETROS_CONSULTA) Is Nothing Then
                        strMsg = String.Format("{0}{1} + El libro no contiene el rango TBL_PARAMETROS_CONSULTA.", strMsg, Environment.NewLine)
                        libroValido = False
                    End If

                End If

            End If

            'valido autorizaciones
            If _vm.BKRequiereAutorizacion Then
                If txtEmailResponsable.Text.Length = 0 Then
                    strMsg = String.Format("{0}{1} + Por favor especifique el email del responsable para notificar los cambios de estado del documento", strMsg, Environment.NewLine)
                    libroValido = False
                Else
                    'email valido
                    Dim regMail As Regex = New Regex(Program.STR_EMAIL_PATRON, RegexOptions.IgnoreCase Or RegexOptions.Singleline)
                    If Not regMail.IsMatch(txtEmailResponsable.Text) Then
                        strMsg = String.Format("{0}{1} + Por favor especifique un email valido", strMsg, Environment.NewLine)
                        libroValido = False
                    End If
                End If
                If cbxPrioridades.SelectedItem Is Nothing Then
                    strMsg = String.Format("{0}{1} + Por favor seleccione una prioridad", strMsg, Environment.NewLine)
                    libroValido = False
                End If
                If c1Notas.Text.Contains("#") Then
                    strMsg = String.Format("{0}{1} + La nota de la versión no debe contener el caracter #", strMsg, Environment.NewLine)
                    libroValido = False
                End If
            End If

            If Not libroValido Then
                mostrarMensaje(String.Format("El archivo presentó los siguientes errores: {0}{1} ", Environment.NewLine, strMsg), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If


        Catch ex As Exception
            _vm.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del cuadro de dialogo de apertura",
                                 Me.ToString(), "NuevoVersionView.ValidarLibro", Program.TituloSistema, Program.Maquina, ex)
            libroValido = False
        End Try

        Return libroValido

    End Function

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles btnCancelar.Click
        Me.DialogResult = False
    End Sub

    Private Async Sub btnGrabar_Click(sender As Object, e As RoutedEventArgs) Handles btnGrabar.Click
        Await AdicionarVersion()
    End Sub

    Private Async Function AdicionarVersion() As Threading.Tasks.Task
        Try

            If ValidarLibro() Then

                If _vm.MetodoSeleccionado.Online Then
                    If Not _vm.ValidarRangos(_vm.MetodoSeleccionado.Nombre.Split(".").Last, _vm.NuevoLibroVersion) Then
                        mostrarMensaje(String.Format("Por favor verifique las siguientes inconsistencias antes de guardar: {0}{1} ", Environment.NewLine, "El nombre del riesgo configurado en el rango TBL_PARAMETROS_VISUALIZACION no coincide con el nombre del método"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Function
                    End If
                End If

                _vm.IsBusy = True

                'calculo el numero de version
                Dim intVersion As Integer

                Dim lstVersiones As List(Of clsInfoVersion) = Await _vm.ObtenerVersionesMetodoEspecifico(_vm.MetodoSeleccionado.Nombre.ToString().Trim.ToUpper())

                'Filtro las versiones por método, consulto el número de la versión y lo aumento
                If lstVersiones IsNot Nothing Then
                    Dim lstVersionesXMetodo = (From item In lstVersiones).ToList()
                    If lstVersionesXMetodo IsNot Nothing Then
                        Dim existe = (From item In lstVersionesXMetodo Order By item.Version Descending).FirstOrDefault()
                        If existe IsNot Nothing Then
                            intVersion = existe.Version + 1
                        Else
                            intVersion = 1
                        End If
                    Else
                        intVersion = 1
                    End If
                End If

                'establezco valores
                Dim Nota As String = c1Notas.Text.ToString()

                If Nota.ToString().Contains("#") Then

                    While Nota.ToString().Contains("#")
                        Dim index As Integer = Nota.ToString().IndexOf("#")

                        If index <> -1 Then
                            Dim colorHex As String = Nota.ToString().Substring(index, 7)

                            colorHex = colorHex.Replace("#", String.Empty)
                            Dim r As Byte = CByte(Convert.ToUInt32(colorHex.Substring(0, 2), 16))
                            Dim g As Byte = CByte(Convert.ToUInt32(colorHex.Substring(2, 2), 16))
                            Dim b As Byte = CByte(Convert.ToUInt32(colorHex.Substring(4, 2), 16))

                            Dim color As String = String.Format("rgb({0},{1},{2})", r, g, b)

                            Nota = Nota.ToString().Replace("#" & colorHex, color)
                        End If
                    End While

                    _vm.NuevaVersion.Observaciones = Nota
                Else
                    _vm.NuevaVersion.Observaciones = c1Notas.Text.ToString()
                End If

                Dim dtFechaInicial As DateTime = DateTime.Now.AddMilliseconds(-DateTime.Now.Millisecond)
                If _vm.BKRequiereAutorizacion Then
                    'Subo el archivo temporal
                    Dim strKeyAutorizaciones As String = String.Format("{0}_{1}", _vm.MetodoSeleccionado.Nombre, intVersion)
                    Dim strUrl As String = Await _vm.objMotorSL.RegistrarArchivoAutorizacionesTaskAsync(strKeyAutorizaciones, _vm.NuevoLibroVersion)
                    If String.IsNullOrEmpty(strUrl) Then
                        _vm.IsBusy = False
                        MensajesControl("Ocurrió un problema al crear el archivo para la revisión el sistema de autorizaciones")
                        Return
                    End If
                    'creo el objeto
                    Dim objDoc As New AutorizacionesIngresoDocumento
                    With objDoc
                        .TipoDocumento = _vm.ConfiguracionAutorizaciones.IDDocumento
                        .NroDocumento = _vm.MetodoSeleccionado.Nombre
                        .NroDetalleDocumento = strKeyAutorizaciones
                        .VersionDocumento = intVersion
                        .URLDocumentoSistemaFuente = strUrl.Trim("""")
                        .InformacionDocumento = "<documento><encabezado infodocumento=""Versión subida desde la consola de administración del motor cálculos""/></documento>"
                        .EmailsInformacion = _vm.ConfiguracionAutorizaciones.EmailInformacionDefecto
                        .EmailResponsable = txtEmailResponsable.Text.Trim
                        .IDNivelAtribucion = _vm.ConfiguracionAutorizaciones.IDNivelAprobacion
                        .IDPrioridad = cbxPrioridades.SelectedValue
                        .FechaVigencia = DateTime.Now.Date.AddDays(_vm.ConfiguracionAutorizaciones.DiasVigencia)
                        .NombreResponsable = Program.NombreUsuario
                        .ComentarioResponsable = c1Notas.Text
                        .EliminarDocumento = False
                        .InsertarRetornoEnTemporal = False
                        .NombreTablaTemporal = String.Empty
                    End With
                    Dim objResultado As AutorizacionesDocumento = Await _vm.objMotorSL.RegistrarDocumentoAutorizacionesTaskAsync(objDoc)
                    If objResultado Is Nothing Then
                        _vm.IsBusy = False
                        MensajesControl("Ocurrió un problema en la comunicación con el sistema de autorizaciones")
                        Return
                    Else
                        If objResultado.Codigo <> 0 Then
                            _vm.IsBusy = False
                            MensajesControl("Ocurrió un problema al registrar el documento en el sistema de autorizaciones: " & objResultado.Mensaje)
                            Return
                        End If
                    End If
                End If

                Dim resultado As Boolean = Await _vm.AdicionarVersion(Newtonsoft.Json.JsonConvert.SerializeObject(_vm.NuevaVersion), dtFechaInicial)
                If resultado Then
                    Me.Close()
                End If

            End If
        Catch fi As IOException
            _vm.IsBusy = False
            MensajesControl("El archivo no se puede subir esta siendo utilizado por otro proceso, verifique que no este abierto")
        Catch ex As Exception
            _vm.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del cuadro de dialogo de apertura",
                                 Me.ToString(), "NuevaVersionView.AdicionarVersion", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Private Sub txtEmailResponsable_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Key = Key.Space Then
            e.Handled = True
        End If
    End Sub
End Class
