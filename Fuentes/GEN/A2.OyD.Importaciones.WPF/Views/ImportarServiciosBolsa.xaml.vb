Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.ComponentModel
Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.Win32
Imports System.IO

Partial Public Class ImportarServiciosBolsa
    Inherits UserControl
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#Region "Inicialización"

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
            objProxyUtil = New UtilidadesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxyUtil = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
        Try
            InitializeComponent()
            LayoutRoot.DataContext = Me
            'dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            objProxyUtil.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
            ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ImportarServiciosBolsa.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Declaraciones"

    Dim dcProxy As ImportacionesDomainContext
    Dim objProxyUtil As UtilidadesDomainContext
    Private Const _STR_NOMBRE_PROCESO As String = "ServiciosBolsa"
    Dim sb As New StringBuilder

#End Region

#Region "Propiedades"

    Private _ListaArchivos As List(Of OyDImportaciones.Archivo)
    Public Property ListaArchivos() As List(Of OyDImportaciones.Archivo)
        Get
            Return _ListaArchivos
        End Get
        Set(ByVal value As List(Of OyDImportaciones.Archivo))
            _ListaArchivos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaArchivos"))
            ArchivoSeleccionado = _ListaArchivos.FirstOrDefault
        End Set
    End Property

    Private _ArchivoSeleccionado As New OyDImportaciones.Archivo
    Public Property ArchivoSeleccionado() As OyDImportaciones.Archivo
        Get
            Return _ArchivoSeleccionado
        End Get
        Set(ByVal value As OyDImportaciones.Archivo)
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


    Public ReadOnly Property FechaHoraActual() As Date
        Get
            Return Now.Date.ToShortDateString
        End Get
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

    Private _chkReemplazar As Boolean = False
    Public Property chkReemplazar() As Boolean
        Get
            Return _chkReemplazar
        End Get
        Set(ByVal value As Boolean)
            _chkReemplazar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("chkReemplazar"))
        End Set
    End Property


#End Region

#Region "Metodos"

    'Private Sub VentanaCargaArchivoCerro(ByVal sender As System.Object, ByVal e As EventArgs)
    '    Try
    '        ListaArchivos.Clear()
    '        dcProxy.Archivos.Clear()
    '        dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
    '        If CType(sender, CargarArchivosView).DialogResult = True And Not IsNothing(CType(sender, CargarArchivosView).ArchivoSeleccionado) Then
    '            tbArchivoImportarSeleccionado.Text = CType(sender, CargarArchivosView).ArchivoSeleccionado.Nombre
    '            ArchivoSeleccionado = CType(sender, CargarArchivosView).ArchivoSeleccionado
    '        End If

    '    Catch ex As Exception
    '        BI.IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
    '                             Me.ToString(), "VentanaCargaArchivoCerro", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    Private Sub CerroVentana()
        'If cfe.DialogResult = True Then
        'Dim ce As New ViewClientes_Exento
        'ce.Comitente = cfe.txtFiltro.Text
        'ce.Nombre = cfe.nombre
        'vm.ViewClientesExentoSelected = ce
        'vm.ViewClientesExentoSelected.Comitente = cfe.txtFiltro.Text
        'vm.ViewClientesExentoSelected.Nombre = cfe.nombre
        'vm.CambioViewClientesExentoSelected()
        'End If
    End Sub

    'Private Sub btnMostrarCargadorArchivos_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    Try
    '        Dim cwCar As New CargarArchivosView(_STR_NOMBRE_PROCESO)
    '        AddHandler cwCar.Closed, AddressOf VentanaCargaArchivoCerro
    '        cwCar.Show()

    '    Catch ex As Exception
    '        BI.IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
    '                             Me.ToString(), "btnMostrarCargadorArchivos_Click", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

#End Region

#Region "Importar archivo"

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        Try
            If dtpDesde.SelectedDate > dtpHasta.SelectedDate Then
                A2Utilidades.Mensajes.mostrarMensaje("Asegúrese que la fecha Desde sea menor o igual que la fecha Hasta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            'If cbArchivosASubir.SelectedIndex = -1 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Seleccione un archivo para subir", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If
            If String.IsNullOrEmpty(cbArchivosASubir.Text) Then
                A2Utilidades.Mensajes.mostrarMensaje("Seleccione un archivo para subir", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            BI.IsBusy = True
            dcProxy.TransladarServiciosBolsaLog(Program.Usuario, Program.HashConexion, AddressOf Terminotraladarservicioslog, Nothing)
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
                                 Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.Name, "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = FormatDateTime(obj.Value, Microsoft.VisualBasic.DateFormat.ShortDate)
        End If
    End Sub

    Private Sub TerminoCargarArchivoLiquidaciones(ByVal lo As LoadOperation(Of OyDImportaciones.ComentarioServicioBolsa))
        Try
            BI.IsBusy = False
            If lo.HasError = False Then
                sb.Clear()
                If dcProxy.ComentarioServicioBolsas.First.idConsecutivo = -1 Then
                    A2Utilidades.Mensajes.mostrarMensaje(dcProxy.ComentarioServicioBolsas.First.inconsistencia.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                For Each comentario In dcProxy.ComentarioServicioBolsas
                    sb.AppendLine(comentario.inconsistencia)
                Next
                dcProxy.actualizaServiciosBolsa(Program.Usuario, Program.HashConexion, AddressOf TerminoActualizarserviciosbolsa, Nothing)
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "TerminoCargarArchivoLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoActualizarserviciosbolsa(ByVal lo As InvokeOperation(Of System.Nullable(Of Integer)))
        Try
            If lo.HasError = False Then
                Dim cfe As New LogImportacion
                AddHandler cfe.Closed, AddressOf CerroVentana
                Program.Modal_OwnerMainWindowsPrincipal(cfe)
                cfe.texto = String.Empty
                cfe.texto = sb.ToString
                cfe.texto = cfe.texto & "Registros actualizados: " & lo.Value.ToString
                cfe.ShowDialog()
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "TerminoActualizarserviciosbolsa", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Terminotraladarservicioslog(ByVal lo As InvokeOperation(Of Integer))
        If lo.HasError = False Then
            dcProxy.ComentarioServicioBolsas.Clear()
            dcProxy.Load(dcProxy.CargarArchivoServiciosBolsaQuery(ArchivoSeleccionado.Nombre, dtpDesde.SelectedDate, dtpHasta.SelectedDate, _STR_NOMBRE_PROCESO, Program.Usuario, chkReemplazar, Program.HashConexion), AddressOf TerminoCargarArchivoLiquidaciones, "")
        Else
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "Terminotraladarservicioslog", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

#End Region

#Region "Eventos"

    Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles ucbtnCargar.CargarArchivo
        Dim objDialog = CType(sender, OpenFileDialog)
        ArchivoSeleccionado.Nombre = Path.GetFileName(objDialog.FileName)
        If String.IsNullOrEmpty(ArchivoSeleccionado.Nombre) Then
            habilitar = False
        Else
            habilitar = True
        End If
    End Sub

#End Region

End Class
