Imports System.Windows.Messaging
Imports System.ComponentModel
Imports System.Threading.Tasks

Partial Public Class MainPage
    Inherits UserControl

    Private Const GSTR_NOMBRE_CANAL_ORDENES As String = "A2.OYD.OrdenesSeteador.Canal"
    Private Const GSTR_NOMBRE_CANAL_VISOR As String = "A2.OYD.VisorSeteador.Canal"
    Private WithEvents orden As OrdenOyDPlus
    Private Const STR_ESTADO_ENVIANDO As String = "Enviando..."
    Private strEstado As String = String.Empty
    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        orden = New OrdenOyDPlus
        Me.DataContext = orden

        InitializeComponent()

        ' define sistema para comunicación con la página del seteador
        Dim messageReceiver As New LocalMessageReceiver(GSTR_NOMBRE_CANAL_VISOR, ReceiverNameScope.Global, LocalMessageReceiver.AnyDomain)
        messageReceiver.DisableSenderTrustCheck = True
        AddHandler messageReceiver.MessageReceived, AddressOf messageReceiver_MessageReceived

        Try
            messageReceiver.Listen()
        Catch ex As ListenFailedException
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se puede recibir mensajes." & Environment.NewLine & _
                   "Ya existe un canal con el nombre '" & GSTR_NOMBRE_CANAL_VISOR & "'.", Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        'orden.establecerPropiedades("RutaVisor*http://a2serverpro:1020/OyDServiciosRIA/OYDPLUS/DatosVisorPage.aspx?IdOrden=100|puedeLanzarSAE*False|puedeMarcarLanzada*False|puedeRechazar*False|VisibilidadNumeroReferencia*Collapsed|NumeroReferencia;0|")
        If IsNothing(orden.RutaVisor) Then enviarMensajeSeteador("Pidiendo información al setador.", "ReenviarMensaje")


    End Sub

    ''' <summary>
    ''' Procedimiento que recibe mensajes de la página del seteador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub messageReceiver_MessageReceived(sender As Object, e As MessageReceivedEventArgs)
        Try
            'e.Response = "response to " & e.Message
            'MessageBox.Show("Message: " & e.Message & Environment.NewLine & _
            '    "NameScope: " & e.NameScope.ToString() & Environment.NewLine & _
            '    "ReceiverName: " & e.ReceiverName & Environment.NewLine & _
            '    "SenderDomain: " & e.SenderDomain & Environment.NewLine & _
            '    "Response: " & e.Response)
            orden.establecerPropiedades(e.Message)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema recibiendo mensaje desde el seteador.", Me.ToString(), "messageReceiver_MessageReceived", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub enviarMensajeSeteador(strEstado As String, strComando As String)
        Try
            orden.EstadoMensaje = strEstado

            Dim messageSender = New LocalMessageSender(GSTR_NOMBRE_CANAL_ORDENES, LocalMessageSender.Global)
            AddHandler messageSender.SendCompleted, AddressOf sender_SendCompleted
            'MessageBox.Show(messageSender.ToString())
            messageSender.SendAsync(strComando, 1)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de enviar el comando " + strComando + " desde el visor", Me.ToString(), "enviarMensajeSeteador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub lanzarManual(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            enviarMensajeSeteador("La orden se ha marcado como lanzada.", "LanzarManual")
        Catch ex As Exception
            Throw New Exception("No se pudo enviar comando 'Lanzar manual' al seteador.")
        End Try
    End Sub

    Private Sub lanzarSAE(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            enviarMensajeSeteador("La orden se ha enrutado por el sistema SAE.", "LanzarSAE")
        Catch ex As Exception
            Throw New Exception("No se pudo enviar comando 'Lanzar SAE' al seteador.")
        End Try
    End Sub

    Private Sub Rechazar(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            enviarMensajeSeteador("Rechazando orden...", "Rechazar")
        Catch ex As Exception
            Throw New Exception("No se pudo enviar comando 'Rechazar' al seteador.")
        End Try
    End Sub

    Private Sub AsignarLiquidacionesProbables(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            enviarMensajeSeteador("Se ha generado la liquidación probable.", "AsignarLiquidacionesProbables")
        Catch ex As Exception
            Throw New Exception("No se pudo enviar comando 'Asignar liquidaciones probables' al seteador.")
        End Try
    End Sub

    Private Sub sender_SendCompleted(ByVal sender As Object, ByVal e As SendCompletedEventArgs)
        orden.VisibilidadEstadoMensaje = Visibility.Visible
    End Sub

    Private Sub MainPage_Unloaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            enviarMensajeSeteador("Cerrando visor.", "Cerrando")
        Catch ex As Exception
            Throw New Exception("No se pudo enviar comando 'Asignar liquidaciones probables' al seteador.")
        End Try
    End Sub
End Class
Class OrdenOyDPlus
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

    Sub establecerPropiedades(pstrPropiedades As String)
        Try
            'MessageBox.Show(pstrPropiedades)
            If pstrPropiedades.Length = 0 Then
                App.Current.MainWindow.Close()
            Else

                Dim strPropiedades As String() = pstrPropiedades.Split(CChar("|"))
                Dim strProp As String()
                For Each strPropiedad As String In strPropiedades
                    strProp = strPropiedad.Split(CChar("*"))
                    Select Case strProp(0)
                        Case "RutaVisor"
                            RutaVisor = strProp(1)
                        Case "puedeLanzarSAE"
                            puedeLanzarSAE = Boolean.Parse(strProp(1))
                        Case "puedeMarcarLanzada"
                            puedeMarcarLanzada = Boolean.Parse(strProp(1))
                        Case "puedeRechazar"
                            puedeRechazar = Boolean.Parse(strProp(1))
                        Case "VisibilidadNumeroReferencia"
                            VisibilidadNumeroReferencia = IIf(strProp(1) = "Visible", Visibility.Visible, Visibility.Collapsed)
                        Case "NumeroReferencia"
                            NumeroReferencia = Integer.Parse(strProp(1))
                    End Select
                Next
                VisibilidadEstadoMensaje = Visibility.Collapsed
                EstadoMensaje = "Enviando..."
            End If
        Catch e As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error estableciendo los datos iniciales de la forma", Me.ToString(), "establecerPropiedades", Application.Current.ToString(), Program.Maquina, e)
            'MessageBox.Show(e.Message)
        End Try
    End Sub

End Class