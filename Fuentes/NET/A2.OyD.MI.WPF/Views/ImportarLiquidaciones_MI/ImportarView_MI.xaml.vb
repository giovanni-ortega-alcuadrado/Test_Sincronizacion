
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports System.ComponentModel
Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Partial Public Class ImportarView_MI
    Inherits UserControl
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#Region "Declaraciones"
    Dim dcProxy As ImportacionesDomainContext
    'Dim dcPRoxy2 As BolsaDomainContext
    Dim dcPRoxy2 As MILADomainContext
    Dim objProxyUtil As UtilidadesDomainContext
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


    Private _FechaHoraActual As Date = Now.Date
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
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext
            dcPRoxy2 = New A2.OyD.OYDServer.RIA.Web.MILADomainContext
            objProxyUtil = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext
        Else
            dcProxy = New A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            dcPRoxy2 = New A2.OyD.OYDServer.RIA.Web.MILADomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxyUtil = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        'SLB20131105 se agrega para evitar Timeout
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
        DirectCast(dcPRoxy2.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.MILADomainContext.IMILADomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

        Try
            InitializeComponent()
            LayoutRoot.DataContext = Me
            btnMostrarCargadorArchivos.Proceso = _STR_NOMBRE_PROCESO
            'dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            objProxyUtil.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")

        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ImportarLiquidacionesView.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Private Sub VentanaCargaArchivoCerro(ByVal sender As System.Object, ByVal e As EventArgs)

    '    Try

    '        ListaArchivos.Clear()
    '        dcProxy.Archivos.Clear()
    '        dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
    '        If CType(sender, CargarArchivosView_MI).DialogResult = True And Not IsNothing(CType(sender, CargarArchivosView_MI).ArchivoSeleccionado) Then
    '            tbArchivoImportarSeleccionado.Text = CType(sender, CargarArchivosView_MI).ArchivoSeleccionado.Nombre
    '            ArchivoSeleccionado = CType(sender, CargarArchivosView_MI).ArchivoSeleccionado
    '        End If

    '    Catch ex As Exception
    '        BI.IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
    '                             Me.ToString(), "ImportarLiquidacionesView.VentanaCargaArchivoCerro", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try

    'End Sub

    'Private Sub btnMostrarCargadorArchivos_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    Try
    '        Dim cwCar As New CargarArchivosView_MI(_STR_NOMBRE_PROCESO)
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

    ''' <summary>
    ''' Este evento se encarga de Importar los registros del archivo plano que se esta cargando. En el domain service esta la logica 
    ''' para evaluar registro a registro, si cumple guardar el archivo en tblimportacionliq_MI
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB</remarks>
    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        Try

            If dtpDesde.SelectedDate > dtpHasta.SelectedDate Then
                A2Utilidades.Mensajes.mostrarMensaje("Asegúrese que la fecha Desde sea menor o igual que la fecha Hasta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(cbArchivosASubir.Text) Then
                A2Utilidades.Mensajes.mostrarMensaje("Seleccione un archivo para subir", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If FechaCierre >= dtpDesde.SelectedDate Or FechaCierre >= dtpHasta.SelectedDate Then
                A2Utilidades.Mensajes.mostrarMensaje("Ha seleccionado un rango de fecha menor a la ultima fecha de cierre, Ultima fecha de cierre: [" & FechaCierre.ToShortDateString() & "].", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            tbResultados.Text = ""
            BI.IsBusy = True
            dcProxy.Load(dcProxy.CargarArchivoLiquidaciones_MIQuery(ArchivoSeleccionado.Nombre, False, dtpDesde.SelectedDate, dtpHasta.SelectedDate, _STR_NOMBRE_PROCESO, Program.Usuario, False, Program.HashConexion), AddressOf TerminoCargarArchivoLiquidaciones, "")
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
                                 Me.ToString(), "ImportarView_MI.btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.Name, "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = FormatDateTime(obj.Value, Microsoft.VisualBasic.DateFormat.ShortDate)
        End If
    End Sub

    Private Sub TerminoCargarArchivoLiquidaciones(ByVal lo As LoadOperation(Of A2.OYD.OYDServer.RIA.Web.OyDImportaciones.LineaComentario))
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

#Region "Eliminar importación"

    ''' <summary>
    ''' Se verifica si se desea eliminar las importaciones.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB</remarks>
    Private Sub btnEliminarImportados_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnEliminarImportados.Click
        'C1.Silverlight.C1MessageBox.Show("Está seguro que desea eliminar los registros que tienen relación con órdenes y los que aún no la tienen? ", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPregunta)
        mostrarMensajePregunta("Está seguro que desea eliminar los registros que tienen relación con órdenes y los que aún no la tienen? ", _
                               Program.TituloSistema, _
                               "INFORMACIONPAGO", _
                               AddressOf TerminaPregunta, False)
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

    ''' <summary>
    ''' Si la respuesta es Yes, se procede ha eliminar todos los registros que exiten en la tabla tblImportacionLiq_MI
    ''' </summary>
    ''' <remarks>SLB</remarks>
    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                BI.IsBusy = True
                dcProxy.Load(dcProxy.EliminarImportados_MIQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoEliminarImportados, "")
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción de eliminación", _
                                 Me.ToString(), "ImportarView_MI.TerminaPregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Trasladar"

    ''' <summary>
    ''' Se verifica si se desea Trasladar la liquidaciones que ya tiene asociadas las ordenes 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB</remarks>
    Private Sub btnTrasladarLiq_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnTrasladarLiq.Click
        Me.TrasladarLiquidaciones()
    End Sub

    Private Sub TrasladarLiquidaciones()
        'C1.Silverlight.C1MessageBox.Show("¿Está seguro que desea ejecutar el proceso de traslado de las liquidaciones? ", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPreguntaTrasladoLiq)
        mostrarMensajePregunta("¿Está seguro que desea ejecutar el proceso de traslado de las liquidaciones? ", _
                               Program.TituloSistema, _
                               "TRASLADARLIQUIDACIONES", _
                               AddressOf TerminaPreguntaTrasladoLiq, False)
    End Sub

    ''' <summary>
    ''' Si la respuesta es Yes, se procede ha trasladar todas las liquidaciones que tengan asociadas una Orden
    ''' de la tabla tblImportacionLiq_MI a la tabla tblLiquidaciones_MI
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TerminaPreguntaTrasladoLiq(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                BI.IsBusy = True
                dcPRoxy2.Load(dcPRoxy2.TrasladarLiquidaciones_MIQuery("T", Program.Usuario, -1, "Contacto", Program.HashConexion), AddressOf TerminoTrasladarLiquidaciones, "")
            End If
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción de eliminación", _
                                 Me.ToString(), "ImportarLiquidacionesView.TerminaPreguntaTrasladoLiq", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTrasladarLiquidaciones(ByVal lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OyDMILA.Comentario_MI))
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



#End Region

    'Private Sub btnMostrarCargadorArchivos_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles btnMostrarCargadorArchivos.CargarArchivo
    '    ArchivoSeleccionado.Nombre = CType(sender, OpenFileDialog).FileName
    'End Sub

    Private Sub btnMostrarCargadorArchivos_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles btnMostrarCargadorArchivos.CargarArchivo
        Dim objDialog = CType(sender, OpenFileDialog)
        ArchivoSeleccionado.Nombre = Path.GetFileName(objDialog.FileName)
        If String.IsNullOrEmpty(ArchivoSeleccionado.Nombre) Then
            habilitar = False
        Else
            habilitar = True
        End If
    End Sub


End Class
