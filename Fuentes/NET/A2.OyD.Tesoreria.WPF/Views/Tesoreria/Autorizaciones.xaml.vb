Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports System.ComponentModel
Imports A2Utilidades.Recursos
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Net
Imports System.ServiceModel
Imports System.Net.Security

Partial Public Class Autorizaciones
    Inherits Window
    Implements INotifyPropertyChanged
    Dim dcProxy As TesoreriaDomainContext
    Private mobjServicio As A2Utilidades.Servicios.SeguridadApp.SeguridadAppClient
    Dim LOGTIENEPERMISO As Boolean
    Public autorizapago As Boolean
    Const GSTR_CLAVE_CIFRADO As String = "A2"
    Dim _mlogSeguridadIntegrada As Boolean = False

    Public Sub New()
        InitializeComponent()
        Me.Control.DataContext = Me
        'SLB20130125 Si se esta trabajando con seguridad integrada no se puede ingresar ni clave ni contraseña
        If CType(Application.Current.Resources(A2Utilidades.Recursos.RecursosApp.A2Consola_UsuarioActivo.ToString()), Usuario).SeguridadIntegrada Then
            _mlogSeguridadIntegrada = True
            validausuario.usuario = Program.UsuarioWindows
            validausuario.clave = String.Empty
            Me.txtUsuario.IsEnabled = False
            Me.txtClave.IsEnabled = False
        Else
            _mlogSeguridadIntegrada = False
            Me.txtUsuario.IsEnabled = True
            Me.txtClave.IsEnabled = True
        End If
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext()
            Else
                dcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Autorizacion.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Try
            If Not _mlogSeguridadIntegrada Then
                If Not IsNothing(validausuario.usuario) Then
                    If Me.validausuario.usuario.Trim.Equals(String.Empty) Then
                        Mensajes.mostrarMensaje("El nombre de usuario es requerido", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Else

                    Mensajes.mostrarMensaje("El nombre de usuario es requerido", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Not IsNothing(validausuario.clave) Then
                    If Me.validausuario.clave.Trim.Equals(String.Empty) Then
                        Mensajes.mostrarMensaje("La contraseña del usuario es requerida", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Else
                    Mensajes.mostrarMensaje("La contraseña del usuario es requerida", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                isbusy = True
                dcProxy.ValidarPermisos(validausuario.usuario, "", LOGTIENEPERMISO,Program.Usuario, Program.HashConexion, AddressOf TerminoTraervalidar, Nothing)
            Else
                isbusy = True
                dcProxy.ValidarPermisos(Program.Usuario, "", LOGTIENEPERMISO,Program.Usuario, Program.HashConexion, AddressOf TerminoTraervalidar, Nothing)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                Me.ToString(), "Autorizacion.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private _validausuario As autoriza = New autoriza
    Public Property validausuario As autoriza
        Get
            Return _validausuario
        End Get
        Set(ByVal value As autoriza)
            _validausuario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("validausuario"))
        End Set
    End Property

    Private Sub TerminoTraervalidar(ByVal obj As InvokeOperation(Of Boolean))
        Try
            If obj.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminoTraervalidar", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
                isbusy = False
            Else
                If Not IsNothing(obj.Value) Then
                    If (obj.Value) = False Then
                        A2Utilidades.Mensajes.mostrarMensaje("El usuario no está autorizado para realizar la operación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        autorizapago = False
                        isbusy = False
                        Exit Sub
                    Else
                        'validar usuario sisi esta bien 
                        Dim objRespuestaServicio = mobjServicio.validarUsuario(CifrarSL.cifrar(Me.validausuario.usuario), CifrarSL.cifrar(Me.validausuario.clave, GSTR_CLAVE_CIFRADO), True, Program.Servidor, Program.Maquina, Program.DireccionIP, Program.Browser)
                        terminovalidarusuario(objRespuestaServicio)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de validar el consecutivo", _
                    Me.ToString(), "TerminoTraervalidar", Application.Current.ToString(), Program.Maquina, ex)
            isbusy = False
        End Try
    End Sub

    Private Sub terminovalidarusuario(ByVal e As A2Utilidades.Servicios.SeguridadApp.ValidarUsuario)
        Dim logExitoso As Boolean = False

        Try
            If Mensajes.verificarResultadoServicio(e, Program.Usuario, Me.ToString(), "validarUsuarioCompleted", Program.TituloSistema, Program.Maquina, Program.RutaServicioLog) Then
                If e.LstDatosValidarUsuario.Count > 0 Then
                    If e.LstDatosValidarUsuario.First.Exitoso Then
                        autorizapago = True
                    Else
                        Mensajes.mostrarMensaje("No se tiene la información suficiente del usuario para realizar su validación en el sistema.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Personalizado)
                        autorizapago = False
                    End If
                Else
                    If Not e.LstDatosValidarUsuario.First.MsgPassword Then

                        Mensajes.mostrarMensaje(e.LstDatosValidarUsuario.First.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Personalizado)
                        autorizapago = False
                    End If
                End If
            End If
        Catch ex As Exception
            autorizapago = False
            isbusy = False
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para verificar el resultado de la validación del usuario", Me.ToString(), "inicializarServicios", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
        Me.DialogResult = True
        isbusy = False
        Me.Close()

    End Sub
    'Private Function validarUsuario() As Boolean
    '    Dim logRes As Boolean = False

    '    Try
    '        If Application.Current.Resources.Contains(A2Utilidades.Recursos.RecursosApp.A2Consola_UsuarioActivo.ToString()) Then


    '            If CType(Application.Current.Resources(A2Utilidades.Recursos.RecursosApp.A2Consola_UsuarioActivo.ToString()), Usuario).SeguridadIntegrada Then
    '                ' mobjLogin.SeguridadIntegrada = True
    '                validausuario.usuario = Program.Usuario
    '            Else
    '                ' mobjLogin.SeguridadIntegrada = False

    '            End If

    '            'mobjLogin.ShowModal()

    '            logRes = True
    '            'Else
    '            '    cargarVentanaDocumento(STR_VENTANA_INICIO, "/Views/AccesoDenegado.xaml", False, Nothing)
    '            '    Mensajes.mostrarMensaje("No se tiene la información necesaria para validar el usuario", Program.TituloSistema, wppMensajes.TiposMensaje.Personalizado)
    '        End If
    '    Catch ex As Exception
    '        Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la validación del usuario", Me.Name, "validarUsuario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '    End Try

    '    Return (logRes)
    'End Function

    Private Sub Autorizaciones(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            '-- Inicializar servicios
            inicializarServicios()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control de acceso.", Me.ToString(), "wppLogin_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private Sub inicializarServicios()
        Try
            Dim logEsHttps As Boolean = False
            If Not String.IsNullOrEmpty(Program.URLServicioSeguridad) Then
                If Strings.Left(Program.URLServicioSeguridad, 5).ToLower = "https" Then
                    logEsHttps = True
                End If
            End If

            If logEsHttps Then
                ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(Function() True)
            End If

            Dim objBinding As BasicHttpBinding = New BasicHttpBinding()
            If logEsHttps Then
                objBinding.Security.Mode = BasicHttpSecurityMode.Transport
            Else
                objBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly
            End If
            objBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows
            objBinding.SendTimeout = New TimeSpan(0, 10, 0)
            objBinding.ReceiveTimeout = New TimeSpan(0, 10, 0)
            objBinding.MaxBufferSize = 2147483647
            objBinding.MaxReceivedMessageSize = 2147483647
            Dim objEndPoint As EndpointAddress = New EndpointAddress(Program.URLServicioSeguridad)

            mobjServicio = New A2Utilidades.Servicios.SeguridadApp.SeguridadAppClient(objBinding, objEndPoint)
            mobjServicio.InnerChannel.OperationTimeout = New TimeSpan(0, 30, 0)

        Catch ex As Exception

            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización de los servicios web", Me.ToString(), "inicializarServicios", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private _isbusy As Boolean
    Public Property isbusy As Boolean
        Get
            Return _isbusy
        End Get
        Set(ByVal value As Boolean)
            _isbusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("isbusy"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

'Comentario
Public Class autoriza
    Implements INotifyPropertyChanged
    Private _usuario As String
    Public Property usuario As String
        Get
            Return _usuario
        End Get
        Set(ByVal value As String)
            _usuario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("usuario"))
        End Set
    End Property

    Private _clave As String
    Public Property clave As String
        Get
            Return _clave
        End Get
        Set(ByVal value As String)
            _clave = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("clave"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

