Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.ComponentModel
Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports Microsoft.Win32
Imports System.IO

Partial Public Class ImportarCompensacionView
    Inherits UserControl
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#Region "Declaraciones"
    Dim dcProxy As ImportacionesDomainContext
    Dim objProxyUtil As UtilidadesDomainContext
    Private Const _STR_NOMBRE_PROCESO As String = "LiquidacionesCompensacion"
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
    Private _Acciones As Boolean = False
    Public Property Acciones() As Boolean
        Get
            Return _Acciones
        End Get
        Set(ByVal value As Boolean)
            _Acciones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Acciones"))
        End Set
    End Property
    Private _Crediticio As Boolean = False
    Public Property Crediticio() As Boolean
        Get
            Return _Crediticio
        End Get
        Set(ByVal value As Boolean)
            _Crediticio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Crediticio"))
        End Set
    End Property
    Private _Ambos As Boolean = True
    Public Property Ambos() As Boolean
        Get
            Return _Ambos
        End Get
        Set(ByVal value As Boolean)
            _Ambos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ambos"))
        End Set
    End Property


#End Region

#Region "Procedimientos"

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
            objProxyUtil = New UtilidadesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxyUtil = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)

        Try
            InitializeComponent()
            LayoutRoot.DataContext = Me
            ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO
            'dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            objProxyUtil.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportarLiquidacionesView.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Private Sub VentanaCargaArchivoCerro(sender As System.Object, e As EventArgs)

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


            'La fecha de cierre no se puede validar en compensacion porque solo es consulta de datos
            'If FechaCierre >= dtpDesde.SelectedDate Or FechaCierre >= dtpHasta.SelectedDate Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Ha seleccionado un rango de fecha menor a la ultima fecha de cierre, Ultima fecha de cierre: [" & FechaCierre.ToShortDateString() & "].", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
            '    Exit Sub
            'End If

            tbResultados.Text = ""
            BI.IsBusy = True
            dcProxy.Load(dcProxy.CargarArchivoLiquidacionesCompensacionQuery(ArchivoSeleccionado.Nombre, False, dtpDesde.SelectedDate, dtpHasta.SelectedDate, _STR_NOMBRE_PROCESO, Program.Usuario, Acciones, Crediticio, Ambos, Program.HashConexion), AddressOf TerminoCargarArchivoLiquidaciones, "")
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
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

    Private Sub TerminoCargarArchivoLiquidaciones(ByVal lo As LoadOperation(Of OyDImportaciones.LineaComentario))
        Try
            Dim sb As New StringBuilder
            BI.IsBusy = False
            If lo.HasError = False Then
                For Each comentario In lo.Entities
                    sb.AppendLine(comentario.FechaHora.ToString & "  " & comentario.Texto)
                Next
                tbResultados.Text = sb.ToString
                'ArchivoSeleccionado.Nombre = String.Empty
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportarLiquidacionesView.TerminoCargarArchivoLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eliminar importación"

    Private Sub btnEliminarImportados_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnEliminarImportados.Click
        'C1.Silverlight.C1MessageBox.Show("Está seguro que desea eliminar los registros que tienen relación con órdenes y los que aún no la tienen? ", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, AddressOf TerminaPregunta)
        mostrarMensajePregunta("Está seguro que desea eliminar todos los registros que tiene hasta el momento importados? ",
                               Program.TituloSistema,
                               "ELIMINARIMPORTADOS",
                               AddressOf TerminaPregunta, False)
    End Sub

    Private Sub TerminoEliminarImportados(ByVal lo As LoadOperation(Of OyDImportaciones.LineaComentario))
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción de eliminación",
                                 Me.ToString(), "ImportarLiquidacionesView.TerminoEliminarImportados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                BI.IsBusy = True
                dcProxy.Load(dcProxy.EliminarImportadosCompensacionQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoEliminarImportados, "")
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción de eliminación",
                                 Me.ToString(), "ImportarLiquidacionesView.TerminaPregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Trasladar"

    'Private Sub btnTrasladarLiq_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnTrasladarLiq.Click
    '    Me.TrasladarLiquidaciones()
    'End Sub

    'Private Sub TrasladarLiquidaciones()
    '    'C1.Silverlight.C1MessageBox.Show("¿Está seguro que desea ejecutar el proceso de traslado de las liquidaciones? ", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, AddressOf TerminaPreguntaTrasladoLiq)
    '    mostrarMensajePregunta("¿Está seguro que desea ejecutar el proceso de traslado de las liquidaciones? ", _
    '                           Program.TituloSistema, _
    '                           "TRASLADARLIQUIDACIONES", _
    '                           AddressOf TerminaPreguntaTrasladoLiq, False)
    'End Sub

    'Private Sub TerminoTrasladarLiquidaciones(ByVal lo As LoadOperation(Of OyDBolsa.Comentario))
    '    Try
    '        Dim sb As New StringBuilder
    '        BI.IsBusy = False
    '        If lo.HasError = False Then
    '            For Each comentario In lo.Entities
    '                sb.AppendLine(comentario.FechaHora.ToString & "  " & comentario.Texto)
    '            Next
    '            tbResultados.Text = sb.ToString
    '        End If
    '    Catch ex As Exception
    '        BI.IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar el traslado de las liquidaciones", _
    '                             Me.ToString(), "ImportarLiquidacionesView.TerminoTrasladarLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

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
