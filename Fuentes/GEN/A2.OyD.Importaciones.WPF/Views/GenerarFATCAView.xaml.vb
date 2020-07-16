Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.Win32
Imports OpenRiaServices.DomainServices.Client
Imports System.IO

Partial Public Class GenerarFATCAView
    Inherits UserControl
    Implements INotifyPropertyChanged
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
#Region "Declaraciones"
    'Const _STR_NOMBRE_PROCESO_GUARDAR = "GeneracionFATCA"
    Const _STR_NOMBRE_PROCESO_LEER = "LEERFATCA"
    Dim dcProxy As ImportacionesDomainContext
#End Region
#Region "Metodos"
    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
        Try
            InitializeComponent()
            Me.GenerarFatca.DataContext = Me
            ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO_LEER
        Catch ex As Exception
            BI.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ImportarServiciosBolsa.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub btnGenerar_Click_1(sender As Object, e As RoutedEventArgs)
        If IsNothing(Fechadesde) Then
            A2Utilidades.Mensajes.mostrarMensaje("La fecha desde es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        If IsNothing(FechaHasta) Then
            A2Utilidades.Mensajes.mostrarMensaje("La fecha hasta es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        If Fechadesde > FechaHasta Then
            A2Utilidades.Mensajes.mostrarMensaje("Asegúrese que la fecha Desde sea menor o igual que la fecha Hasta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        IsBusy = True
        dcProxy.GuardarArchivoFATCA(Fechadesde, FechaHasta, "", Program.Usuario, "", Program.HashConexion, AddressOf Terminoguardararchivofatca, Nothing)

    End Sub
    Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles ucbtnCargar.CargarArchivo
        Dim objDialog = CType(sender, OpenFileDialog)
        Nombrearchivo = Path.GetFileName(objDialog.FileName)
        'CargarArchivo(Nombrearchivo)
        Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, GenerarFATCAView), Nombrearchivo, "")
        Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
        viewImportacion.ShowDialog()
    End Sub
    'Private Sub TerminoCrearArchivo()
    '    Try
    '        Dim cwCar As New ListarArchivosDirectorioView(_STR_NOMBRE_PROCESO_GUARDAR) 'CSTR_NOMBREPROCESO_TITULOSACTIVOS)
    '        cwCar.Show()
    '        IsBusy = False
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al levantar la ventana de visualización de los archivos", _
    '                             Me.ToString(), "TerminoCrearArchivo", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub
    Sub CargarArchivo(modulo As String, NombreArchivo As String)
        Try
            ViewImportarArchivo.IsBusy = True
            If Not IsNothing(dcProxy.RespuestaArchivoImportacions) Then
                dcProxy.RespuestaArchivoImportacions.Clear()
            End If
            dcProxy.Load(dcProxy.LeerArchivoFATCAQuery(NombreArchivo, _STR_NOMBRE_PROCESO_LEER, Program.Usuario, "\LeerFATCA.fmt", Program.HashConexion), AddressOf TerminoLeerFATCA, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo recibo.", _
                               Me.ToString(), "CargarArchivoRecibos", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub
#End Region
#Region "Propiedades"
    Private _Fechadesde As System.Nullable(Of Date)
    Public Property Fechadesde As System.Nullable(Of Date)
        Get
            Return _Fechadesde
        End Get
        Set(value As System.Nullable(Of Date))
            _Fechadesde = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fechadesde"))
        End Set
    End Property

    Private _FechaHasta As System.Nullable(Of Date)
    Public Property FechaHasta As System.Nullable(Of Date)
        Get
            Return _FechaHasta
        End Get
        Set(value As System.Nullable(Of Date))
            _FechaHasta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaHasta"))
        End Set
    End Property

    Private _Nombrearchivo As String
    Public Property Nombrearchivo As String
        Get
            Return _Nombrearchivo
        End Get
        Set(value As String)
            _Nombrearchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombrearchivo"))
        End Set
    End Property

    Private _IsBusy As Boolean
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property
    Private _ViewImportarArchivo As cwCargaArchivos
    Public Property ViewImportarArchivo() As cwCargaArchivos
        Get
            Return _ViewImportarArchivo
        End Get
        Set(ByVal value As cwCargaArchivos)
            _ViewImportarArchivo = value
        End Set
    End Property
#End Region
#Region "Asincronicos"
    Private Sub Terminoguardararchivofatca(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se realizo el proceso de generacion del archivo fatca", Me.Name, "Terminoguardararchivofatca", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensaje("El archivo se ha generado exitosamente, se encuentra en la ruta:" + obj.Value, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    TerminoCrearArchivo()
        End If
    End Sub
    Private Sub TerminoLeerFATCA(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            IsBusy = False
            If lo.HasError = False Then

                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then

                        objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False).OrderBy(Function(o) o.Tipo)
                            If li.Tipo = "C" Then
                                objListaMensajes.Add(li.Mensaje)
                            Else
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                            End If
                        Next

                        objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.IsBusy = False
                    Else
                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next
                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.IsBusy = False
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                End If
            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                ViewImportarArchivo.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            ViewImportarArchivo.IsBusy = False
        End Try
    End Sub
#End Region
End Class
