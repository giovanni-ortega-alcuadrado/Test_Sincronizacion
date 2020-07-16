
Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports System.ComponentModel
Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports System.IO
Imports A2MCCOREWPF

Partial Public Class ImportarLiquidacionesView
    Inherits UserControl
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Dim Eliminarimportacion As EliminarImportacionLiqView

#Region "Declaraciones"
    Dim dcProxy As ImportacionesDomainContext
    Dim dcPRoxy2 As BolsaDomainContext
    Dim objProxyUtil As UtilidadesDomainContext
    Dim pstrMesesActualizaCostos As String 'JAG 20140312

    Private Const _STR_NOMBRE_PROCESO As String = "Liquidaciones"
#End Region

#Region "Propiedades"

    Private _ListaArchivos As List(Of Archivo)
    Public Property ListaArchivos() As List(Of Archivo)
        Get
            Return _ListaArchivos
        End Get
        Set(ByVal value As List(Of Archivo))
            _ListaArchivos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaArchivos"))
            ArchivoSeleccionado = _ListaArchivos.FirstOrDefault
        End Set
    End Property

    Private _ArchivoSeleccionado As New Archivo
    Public Property ArchivoSeleccionado() As Archivo
        Get
            Return _ArchivoSeleccionado
        End Get
        Set(ByVal value As Archivo)
            If Not IsNothing(value) Then
                _ArchivoSeleccionado = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ArchivoSeleccionado"))
            End If
        End Set
    End Property


    Private _dtmFechaCierre As Date
    Public Property FechaCierre() As Date
        Get
            Return _dtmFechaCierre
        End Get
        Set(ByVal value As Date)
            If Not IsNothing(value) Then
                _dtmFechaCierre = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaCierre"))
            End If
        End Set
    End Property


    Private _FechaHoraActual As DateTime = Now.Date
    Public Property FechaHoraActual() As DateTime
        Get
            Return _FechaHoraActual
        End Get
        Set(ByVal value As DateTime)
            _FechaHoraActual = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaHoraActual"))
        End Set
    End Property

    'Private Sub TerminoTraerArchivosAdjuntos(ByVal lo As LoadOperation(Of Archivo))
    '    If IsNothing(lo.Error) Then
    '        ListaArchivos = dcProxy.Archivos.ToList
    '        If ListaArchivos.Count > 0 Then
    '            habilitar = True
    '        Else
    '            habilitar = False
    '        End If
    '    Else
    '        MessageBox.Show(lo.Error.Message)
    '    End If
    'End Sub


    Private _habilitar As Boolean = False
    Public Property habilitar() As Boolean
        Get
            Return _habilitar
        End Get
        Set(ByVal value As Boolean)
            _habilitar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("habilitar"))
        End Set
    End Property

    'JFSB 20160804 Lista para la exportacion a excel
    Private _ListaMensajes As List(Of String) = New List(Of String)
    Public Property ListaMensajes() As List(Of String)
        Get
            Return _ListaMensajes
        End Get
        Set(ByVal value As List(Of String))
            _ListaMensajes = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaMensajes"))
        End Set
    End Property

    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreArchivo"))
        End Set
    End Property

#End Region

#Region "Inicialización"

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext
            dcPRoxy2 = New A2.OyD.OYDServer.RIA.Web.BolsaDomainContext
            objProxyUtil = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext
        Else
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            dcPRoxy2 = New A2.OyD.OYDServer.RIA.Web.BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxyUtil = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 600)
        DirectCast(dcPRoxy2.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.BolsaDomainContext.IBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 600)

        Try
            InitializeComponent()
            LayoutRoot.DataContext = Me
            ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO
            'dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            ConsultarFechaCierreSistema("")
            objProxyUtil.Verificaparametro("MESESACTUALIZACOSTOS", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "MESESACTUALIZACOSTOS") 'JAG 20140312
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ImportarLiquidacionesView.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConsultarFechaCierreSistema(Optional ByVal pstrUserState As String = "")
        Try
            objProxyUtil.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre del sistema.", _
                               Me.ToString(), "ConsultarFechaCierreSistema", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Private Sub VentanaCargaArchivoCerro(sender As System.Object, e As EventArgs)

    '    Try

    '        ListaArchivos.Clear()
    '        dcProxy.Archivos.Clear()
    '        dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
    '        If CType(sender, CargarArchivosView).DialogResult = True And Not IsNothing(CType(sender, CargarArchivosView).ArchivoSeleccionado) Then
    '            tbArchivoImportarSeleccionado.Text = CType(sender, CargarArchivosView).ArchivoSeleccionado.Nombre
    '            ArchivoSeleccionado = CType(sender, CargarArchivosView).ArchivoSeleccionado
    '        End If

    '    Catch ex As Exception
    '        BI.IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
    '                             Me.ToString(), "ImportarLiquidacionesView.VentanaCargaArchivoCerro", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try

    'End Sub

    'Private Sub btnMostrarCargadorArchivos_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
    '    Try
    '        Dim cwCar As New CargarArchivosView(_STR_NOMBRE_PROCESO)
    '        AddHandler cwCar.Closed, AddressOf VentanaCargaArchivoCerro
    '        cwCar.Show()
    '    Catch ex As Exception
    '        BI.IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
    '                             Me.ToString(), "ImportarLiquidacionesView.btnMostrarCargadorArchivos_Click", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

#End Region

#Region "Importar archivo"

    Private Sub btnAceptar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        Try

            If dtpDesde.SelectedDate > dtpHasta.SelectedDate Then
                A2Utilidades.Mensajes.mostrarMensaje("Asegúrese que la fecha Desde sea menor o igual que la fecha Hasta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                Exit Sub
            End If

            If String.IsNullOrEmpty(cbArchivosASubir.Text) Then
                A2Utilidades.Mensajes.mostrarMensaje("Seleccione un archivo para subir", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                Exit Sub
            End If

            ConsultarFechaCierreSistema("IMPORTAR")
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
                                 Me.ToString(), "ImportarLiquidacionesView.btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarImportacionLiquidaciones()
        Try
            If FechaCierre >= dtpDesde.SelectedDate Or FechaCierre >= dtpHasta.SelectedDate Then
                A2Utilidades.Mensajes.mostrarMensaje("Ha seleccionado un rango de fecha menor a la ultima fecha de cierre, Ultima fecha de cierre: [" & FechaCierre.ToShortDateString() & "].", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            tbResultados.Text = ""
            BI.IsBusy = True
            dcProxy.Load(dcProxy.CargarArchivoLiquidacionesQuery(ArchivoSeleccionado.Nombre, False, dtpDesde.SelectedDate, dtpHasta.SelectedDate, _STR_NOMBRE_PROCESO, Program.Usuario, FechaCierre, Program.HashConexion), AddressOf TerminoCargarArchivoLiquidaciones, "")
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
                                 Me.ToString(), "ContinuarImportacionLiquidaciones.btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.Name, "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = FormatDateTime(obj.Value, Microsoft.VisualBasic.DateFormat.ShortDate)
            If obj.UserState = "IMPORTAR" Then
                ContinuarImportacionLiquidaciones()
            End If
        End If
    End Sub

    Private Sub TerminoCargarArchivoLiquidaciones(ByVal lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario))
        Try
            Dim sb As New StringBuilder
            BI.IsBusy = False
            Dim objListaMensaje As New List(Of String)
            Dim strMensaje() As String

            If lo.HasError = False Then
                For Each comentario In lo.Entities.ToList
                    sb.AppendLine(comentario.FechaHora.ToString & "  " & comentario.Texto)

                    strMensaje = comentario.Texto.Split(vbCrLf)

                    For Each mensaje In strMensaje  'JFSB 20160826 Se agrega ciclo para separar las inconsistencias
                        objListaMensaje.Add(mensaje.Replace(",", "").Replace(vbLf, String.Empty))
                    Next
                Next

                tbResultados.Text = sb.ToString
                ListaMensajes = objListaMensaje     'JFSB 20160804 Se agregan comentarios a la lista de mensajes

            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
                                 Me.ToString(), "ImportarLiquidacionesView.TerminoCargarArchivoLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eliminar importación"

    Private Sub btnEliminarImportados_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnEliminarImportados.Click
        'C1.Silverlight.C1MessageBox.Show("Está seguro que desea eliminar los registros que tienen relación con órdenes y los que aún no la tienen? ", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPregunta)
        ' mostrarMensajePregunta("Está seguro que desea eliminar los registros que tienen relación con órdenes y los que aún no la tienen? ", _
        '      Program.TituloSistema, _
        ' "ELIMINARIMPORTADOS", _
        'AddressOf TerminaPregunta, False)

        Eliminarimportacion = New EliminarImportacionLiqView()
        'JFSB 20170621 Se pone seleccionado todos por defecto
        Eliminarimportacion.logtodos = True
        AddHandler Eliminarimportacion.Closed, AddressOf TerminaPregunta
        Program.Modal_OwnerMainWindowsPrincipal(Eliminarimportacion)
        Eliminarimportacion.ShowDialog()
        Exit Sub
    End Sub

    Private Sub TerminoEliminarImportados(ByVal lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario))
        Try
            Dim sb As New StringBuilder
            BI.IsBusy = False
            If lo.HasError = False Then
                For Each comentario In lo.Entities
                    sb.AppendLine(comentario.FechaHora.ToString & "  " & comentario.Texto)
                Next
                tbResultados.Text = sb.ToString
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción de eliminación", _
                                 Me.ToString(), "ImportarLiquidacionesView.TerminoEliminarImportados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As EliminarImportacionLiqView = CType(sender, EliminarImportacionLiqView)

            If objResultado.DialogResult Then
                BI.IsBusy = True
                Dim Respuesta As String
                Respuesta = objResultado.Respuesta

                dcProxy.Load(dcProxy.EliminarImportadosLiqQuery(Respuesta, Program.Usuario, Program.HashConexion), AddressOf TerminoEliminarImportados, "")
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción de eliminación", _
                                 Me.ToString(), "ImportarLiquidacionesView.TerminaPregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaPreguntaTrasladoLiq(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim lngCantidad As Integer

            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                'JAG 20140311
                'dcPRoxy2.ValidarCostosBolsaxLiq_Consultar("C", lngCantidad, Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaValidarCostos, Nothing)
                dcPRoxy2.ValidarCostosBolsaxLiq_Consultar("C", lngCantidad, Program.Usuario, Program.HashConexion, AddressOf TerminoValidarCostos, lngCantidad)
                'JAG 20140311
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el traslado de las liquidaciones", _
                                 Me.ToString(), "ImportarLiquidacionesView.TerminaPreguntaTrasladoLiq", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Trasladar"

    Private Sub btnTrasladarLiq_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnTrasladarLiq.Click
        Me.TrasladarLiquidaciones()
    End Sub

    Private Sub TrasladarLiquidaciones()
        'C1.Silverlight.C1MessageBox.Show("¿Está seguzro que desea ejecutar el proceso de traslado de las liquidaciones? ", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPreguntaTrasladoLiq)
        mostrarMensajePregunta("¿Está seguro que desea ejecutar el proceso de traslado de las liquidaciones? ", _
                               Program.TituloSistema, _
                               "TRASLADARLIQUIDACIONES", _
                               AddressOf TerminaPreguntaTrasladoLiq, False)
    End Sub

    Private Sub TerminoTrasladarLiquidaciones(ByVal lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OyDBolsa.Comentario))
        Try
            Dim sb As New StringBuilder
            BI.IsBusy = False
            If lo.HasError = False Then
                For Each comentario In lo.Entities
                    sb.AppendLine(comentario.FechaHora.ToString & "  " & comentario.Texto)
                Next
                tbResultados.Text = sb.ToString
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el traslado de las liquidaciones", _
                                 Me.ToString(), "ImportarLiquidacionesView.TerminoTrasladarLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'JAG 20140312
    Private Sub Terminotraerparametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el parametro", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            Select Case obj.UserState
                Case "MESESACTUALIZACOSTOS"
                    pstrMesesActualizaCostos = obj.Value
            End Select
        End If
    End Sub
    'JAG 20140312

    'JAG 20140311
    Private Sub TerminoValidarCostos(ByVal lo As InvokeOperation(Of Integer))
        Try
            If lo.Value > 0 Then
                mostrarMensajePregunta("Existen " & lo.Value & " operaciones que tienen los costos de bolsa desactualizados con más de " & pstrMesesActualizaCostos & " meses. Desea actualizar estos costos con este valor?", _
                                       Program.TituloSistema, _
                                       "TRASLADARLIQUIDACIONES", _
                                       AddressOf TerminaPreguntaValidarCostos, False)
            Else
                BI.IsBusy = True
                dcPRoxy2.Load(dcPRoxy2.TrasladarLiquidacionesQuery("T", Program.Usuario, -1, "Contacto", False, Program.HashConexion), AddressOf TerminoTrasladarLiquidaciones, "") 'JAG 20140311 se agrega el envio del parametro plogActualizarCostos
            End If

        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el traslado de las liquidaciones", _
                                 Me.ToString(), "ImportarLiquidacionesView.TerminoValidarCostos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    'JAG 20140311

    'JAG 20140311
    Private Sub TerminaPreguntaValidarCostos(ByVal sender As Object, ByVal e As EventArgs)
        Try
            BI.IsBusy = True

            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                dcPRoxy2.Load(dcPRoxy2.TrasladarLiquidacionesQuery("T", Program.Usuario, -1, "Contacto", True, Program.HashConexion), AddressOf TerminoTrasladarLiquidaciones, "") 'JAG 20140311 se agrega el envio del parametro plogActualizarCostos
            Else
                dcPRoxy2.Load(dcPRoxy2.TrasladarLiquidacionesQuery("T", Program.Usuario, -1, "Contacto", False, Program.HashConexion), AddressOf TerminoTrasladarLiquidaciones, "") 'JAG 20140311 se agrega el envio del parametro plogActualizarCostos
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el traslado de las liquidaciones", _
                                 Me.ToString(), "ImportarLiquidacionesView.TerminaPreguntaTrasladoLiq", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    'JAG 20140311

#End Region


    Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles ucbtnCargar.CargarArchivo
        Dim objDialog = CType(sender, OpenFileDialog)
        ArchivoSeleccionado.Nombre = Path.GetFileName(objDialog.FileName)
        If String.IsNullOrEmpty(ArchivoSeleccionado.Nombre) Then
            habilitar = False
        Else
            habilitar = True
        End If
    End Sub

#Region "Exportar"

    ''' <summary>
    ''' JFSB 20160804 Sección para el proceso de exportar a CSV
    ''' </summary>
    ''' <remarks></remarks>
    ''' 

    Private Sub btnExportar_Click(sender As Object, e As RoutedEventArgs)
        ExportarToCSV()
    End Sub

    ''' <summary>
    ''' Metodo para exportar datos en archivo plano excel 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportarToCSV()
        Try
            If ListaMensajes.Any Then
                'preparo los datos
                Dim objdatos As String = String.Join("#", ListaMensajes)
                Dim objFile As New SaveFileDialog()
                With objFile
                    .FileName = NombreArchivo.Split(CChar(".")).First() & "_Resultado.csv"
                    .Filter = "Plano CSV | *.csv"
                    .DefaultExt = "csv"
                End With
                Dim result As System.Nullable(Of Boolean) = objFile.ShowDialog()
                If result.HasValue AndAlso result = True Then
                    'guardo el resultado
                    Using fileStream As System.IO.Stream = objFile.OpenFile()
                        Using bookStream As MemoryStream = clsCORE.ExportaDatos(objdatos)
                            bookStream.WriteTo(fileStream)
                        End Using
                    End Using
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No hay datos para exportar", "Se presentó un problema al exportar los datos", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al exportar los datos", _
                               Me.ToString(), "cwCargaArchivos.ExportarToCSV", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region
    
End Class
