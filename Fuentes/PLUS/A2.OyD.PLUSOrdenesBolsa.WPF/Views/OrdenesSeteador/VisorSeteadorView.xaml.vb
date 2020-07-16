Imports Telerik.Windows.Controls
Imports System.Xml
Imports System.Xml.Linq
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesBolsa
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class VisorSeteadorView
    Inherits Window

    Private WithEvents _mobjVM As OrdenSeteadorViewModel
    Private WithEvents orden As OrdenOyDPlusVisorSeteador
    Private strEstado As String = String.Empty

    Public Sub New(ByVal pobjVieModelSeteador As OrdenSeteadorViewModel)
        Try
            orden = New OrdenOyDPlusVisorSeteador
            InitializeComponent()
            _mobjVM = pobjVieModelSeteador

            visor.Source = New Uri(Program.URLWebPageSeteadorSinOrden)

            Me.DataContext = orden

            _mobjVM.IsBusySeteador = False
            _mobjVM.pobjViewVisorSeteadorView = Me
            _mobjVM.OrdenEnvisor = True
            _mobjVM.enviarMensajeApp()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "No se puede inicializar el formulario del visor del seteador", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub VisorSeteadorView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            Dim intIzquierda As Double = 990
            Me.Height = 730
            Me.Width = 300
            Me.Top = 0

            If Application.Current.Resources.Contains("MainWindowsPrincipalAnchoTotal") Then
                Dim intAnchoTotal = CType(Application.Current.Resources("MainWindowsPrincipalAnchoTotal"), Double)
                If Not intAnchoTotal.Equals(Double.NaN) Then
                    intIzquierda = intAnchoTotal - 310
                End If
            End If

            If Application.Current.Resources.Contains("MainWindowsPrincipalAltoTotal") Then
                Dim intAltoTotal = CType(Application.Current.Resources("MainWindowsPrincipalAltoTotal"), Double)
                If Not intAltoTotal.Equals(Double.NaN) Then
                    Me.Height = intAltoTotal - 10
                End If
            End If

            Me.Left = intIzquierda
            Me.Topmost = True
            If Not IsNothing(Me.Owner) Then
                Me.Owner.IsEnabled = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "No se puede inicializar el formulario del visor del seteador", Me.Name, "VisorSeteadorView_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub EstablecerPropiedades(ByVal pstrRutaVisorSeteador As String, ByVal plogPuedeLanzarSAE As Boolean, ByVal plogPuedeMarcarLanzada As Boolean, ByVal plogPuedeRechazar As Boolean, ByVal pobjVisibilidadNroReferencia As Visibility, ByVal pstrNroReferencia As String)
        Try
            orden.establecerPropiedades(pstrRutaVisorSeteador, plogPuedeRechazar, plogPuedeMarcarLanzada, plogPuedeRechazar, pobjVisibilidadNroReferencia, pstrNroReferencia)
            visor.Source = New Uri(orden.RutaVisor)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al establecer las propiedades.", Me.Name, "EstablecerPropiedades", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub lanzarManual(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim objpuedeLanzarSAE As Boolean = orden.puedeLanzarSAE
            Dim objpuedeMarcarLanzada As Boolean = orden.puedeMarcarLanzada
            Dim objpuedeRechazar As Boolean = orden.puedeRechazar
            orden.puedeLanzarSAE = False
            orden.puedeMarcarLanzada = False
            orden.puedeRechazar = False

            _mobjVM.marcarLanzada(Nothing)
            _mobjVM.OrdenSeteadorSelected = _mobjVM.ListaOrdenes.Item(0)

            orden.puedeLanzarSAE = objpuedeLanzarSAE
            orden.puedeMarcarLanzada = objpuedeMarcarLanzada
            orden.puedeRechazar = objpuedeRechazar
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al ejecutar la acción.", Me.Name, "lanzarManual", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub lanzarSAE(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim objpuedeLanzarSAE As Boolean = orden.puedeLanzarSAE
            Dim objpuedeMarcarLanzada As Boolean = orden.puedeMarcarLanzada
            Dim objpuedeRechazar As Boolean = orden.puedeRechazar
            orden.puedeLanzarSAE = False
            orden.puedeMarcarLanzada = False
            orden.puedeRechazar = False

            _mobjVM.enviarPorSAE(Nothing)
            _mobjVM.EnviarMensajeCliente(MensajeNotificacion.AccionEjecutada.EnviadaSAE.ToString(), OrdenSeteadorViewModel.STR_MENSAJE_CONSOLA_ORDEN_ENVIADA_SAE)
            _mobjVM.MostrarPopupVisor(Nothing)

            orden.puedeLanzarSAE = objpuedeLanzarSAE
            orden.puedeMarcarLanzada = objpuedeMarcarLanzada
            orden.puedeRechazar = objpuedeRechazar
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al ejecutar la acción.", Me.Name, "lanzarSAE", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Rechazar(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim objpuedeLanzarSAE As Boolean = orden.puedeLanzarSAE
            Dim objpuedeMarcarLanzada As Boolean = orden.puedeMarcarLanzada
            Dim objpuedeRechazar As Boolean = orden.puedeRechazar
            orden.puedeLanzarSAE = False
            orden.puedeMarcarLanzada = False
            orden.puedeRechazar = False

            _mobjVM.rechazarOrden(Nothing)

            orden.puedeLanzarSAE = objpuedeLanzarSAE
            orden.puedeMarcarLanzada = objpuedeMarcarLanzada
            orden.puedeRechazar = objpuedeRechazar
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al ejecutar la acción.", Me.Name, "Rechazar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub AsignarLiquidacionesProbables(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            _mobjVM.cargarLiquidaciones()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al ejecutar la acción.", Me.Name, "AsignarLiquidacionesProbables", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub MainPage_Unloaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            _mobjVM.ActualizarBloqueoOrden()
            _mobjVM.pobjViewVisorSeteadorView = Nothing
            _mobjVM.OrdenEnvisor = False
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al ejecutar la acción.", Me.Name, "MainPage_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class

Class OrdenOyDPlusVisorSeteador
    Implements INotifyPropertyChanged

    Private _rutaVisor As String
    Public Property RutaVisor() As String
        Get
            Return _rutaVisor
        End Get
        Set(ByVal value As String)
            _rutaVisor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RutaVisor"))
        End Set
    End Property

    Private _puedeLanzarSAE As Boolean
    ''' <summary>
    ''' Habilia/deshabilita botón Lanzar SAE
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property puedeLanzarSAE() As Boolean
        Get
            Return _puedeLanzarSAE
        End Get
        Set(ByVal value As Boolean)
            _puedeLanzarSAE = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("puedeLanzarSAE"))
        End Set
    End Property

    Private _puedeMarcarLanzada As Boolean
    ''' <summary>
    ''' Habilia/deshabilita botón Marcar como lanzada (Lanzar)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property puedeMarcarLanzada() As Boolean
        Get
            Return _puedeMarcarLanzada
        End Get
        Set(ByVal value As Boolean)
            _puedeMarcarLanzada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("puedeMarcarLanzada"))
        End Set
    End Property

    Private _puedeRechazar As Boolean

    ''' <summary>
    ''' Habilia/deshabilita botón Rechazar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property puedeRechazar() As Boolean
        Get
            Return _puedeRechazar
        End Get
        Set(ByVal value As Boolean)
            _puedeRechazar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("puedeRechazar"))
        End Set
    End Property

    Private _visibilidadNroRef As Visibility
    ''' <summary>
    ''' Indica si existe número de referencia para acciones y expone propiedad visible para mostrar o colapsar objetos en la vista
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VisibilidadNumeroReferencia() As Visibility
        Get
            Return _visibilidadNroRef
        End Get
        Set(ByVal value As Visibility)
            _visibilidadNroRef = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisibilidadNumeroReferencia"))
        End Set
    End Property

    Private _nroReferencia As Integer
    ''' <summary>
    ''' Número de referencia para acciones
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NumeroReferencia() As Integer
        Get
            Return _nroReferencia
        End Get
        Set(ByVal value As Integer)
            _nroReferencia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroReferencia"))
        End Set
    End Property

    Private _strEstadoMensaje As String
    ''' <summary>
    ''' Estado de mensaje recibido por la página del seteador
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EstadoMensaje() As String
        Get
            Return _strEstadoMensaje
        End Get
        Set(ByVal value As String)
            _strEstadoMensaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EstadoMensaje"))
        End Set
    End Property

    Private _visibilidadEstadoMensaje As Visibility
    ''' <summary>
    ''' Visibilidad del estado del mensaje recibido por la página del seteador
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VisibilidadEstadoMensaje() As Visibility
        Get
            Return _visibilidadEstadoMensaje
        End Get
        Set(ByVal value As Visibility)
            _visibilidadEstadoMensaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisibilidadEstadoMensaje"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Sub establecerPropiedades(ByVal pstrRutaVisorSeteador As String, ByVal plogPuedeLanzarSAE As Boolean, ByVal plogPuedeMarcarLanzada As Boolean, ByVal plogPuedeRechazar As Boolean, ByVal pobjVisibilidadNroReferencia As Visibility, ByVal pstrNroReferencia As String)
        Try
            RutaVisor = pstrRutaVisorSeteador
            puedeLanzarSAE = plogPuedeLanzarSAE
            puedeMarcarLanzada = plogPuedeMarcarLanzada
            puedeRechazar = plogPuedeRechazar
            VisibilidadNumeroReferencia = pobjVisibilidadNroReferencia
            NumeroReferencia = pstrNroReferencia
        Catch e As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error estableciendo los datos iniciales de la forma", Me.ToString(), "establecerPropiedades", Application.Current.ToString(), Program.Maquina, e)
            'MessageBox.Show(e.Message)
        End Try
    End Sub
End Class