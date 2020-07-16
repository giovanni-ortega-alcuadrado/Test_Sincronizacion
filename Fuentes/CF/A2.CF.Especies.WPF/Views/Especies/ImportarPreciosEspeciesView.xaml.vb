
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones


Partial Public Class ImportarPreciosEspeciesView
    Inherits UserControl
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#Region "Declaraciones"

    Dim dcProxy As ImportacionesDomainContext
    Dim objProxyUtil As UtilidadesDomainContext
    Private Const _STR_NOMBRE_PROCESO As String = "Especies"

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

    Private _FechaHoraActual As Date = Now.Date.ToShortDateString
    Public Property FechaHoraActual() As Date
        Get
            Return _FechaHoraActual
        End Get
        Set(ByVal value As Date)
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

#End Region

#Region "Inicialización"

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
            objProxyUtil = New UtilidadesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            objProxyUtil = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
        DirectCast(objProxyUtil.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

        Try
            InitializeComponent()
            LayoutRoot.DataContext = Me
            ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO
            'dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            objProxyUtil.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ImportarLiquidacionesView.New", Application.Current.ToString(), Program.Maquina, ex)
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

            If String.IsNullOrEmpty(cbArchivosASubir.Text) Then
                A2Utilidades.Mensajes.mostrarMensaje("Seleccione un archivo para subir", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                Exit Sub
            End If

            tbResultados.Text = ""
            BI.IsBusy = True
            dcProxy.Load(dcProxy.CargarArchivoPreciosEspeciesQuery(ArchivoSeleccionado.Nombre, dtpDesde.SelectedDate, _STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarArchivoLiquidaciones, "")
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
                                 Me.ToString(), "ImportarLiquidacionesView.btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.Name, "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = FormatDateTime(obj.Value, Microsoft.VisualBasic.DateFormat.ShortDate)
        End If
    End Sub

    Private Sub TerminoCargarArchivoLiquidaciones(ByVal lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario))
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
                                 Me.ToString(), "ImportarLiquidacionesView.TerminoCargarArchivoLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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
End Class
