' Desarrollo de control de cierre de divisas
' Ricardo Barrientos Pérez
' RABP20200601  actualiza cierre control de Divisas

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.WEB


Public Class ControlCierreDivisasViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------

    Public objViewPrincipal As ControlCierreDivisasView


#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub


    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                IsBusy = True
                consultarFechaControlCierre()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"


    Private _lstFechaControl As List(Of CPX_ControlCierreOPeracionesDivisasConsultar)
    Public Property lstFechaControl() As List(Of CPX_ControlCierreOPeracionesDivisasConsultar)
        Get
            Return _lstFechaControl
        End Get
        Set(ByVal value As List(Of CPX_ControlCierreOPeracionesDivisasConsultar))
            _lstFechaControl = value
            MyBase.CambioItem("lstFechaControl")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Proceso para la consulta de la ordenes pendintes por cumplir
    ''' </summary>
    Public Async Sub consultarFechaControlCierre()
        Try
            IsBusy = True
            Dim objRespuesta = Await mdcProxy.OrdenesDivisasControlCierre_ConsultarAsync()
            If Not IsNothing(objRespuesta) Then
                lstFechaControl = objRespuesta.Value
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                     "consultarFechaControlCierre", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub


    ''' <summary>
    ''' Comando del botón para iniciar la ejecución del cumplimiento
    ''' </summary>
    Private WithEvents _ActualizarCmd As RelayCommand
    Public ReadOnly Property ActualizarCmd() As RelayCommand
        Get
            If _ActualizarCmd Is Nothing Then
                _ActualizarCmd = New RelayCommand(AddressOf ConfirmarActualizarFechaControlSeleccionadas)
            End If
            Return _ActualizarCmd
        End Get
    End Property

    ''' <summary>
    ''' Confirmar cumplimiento de operaciones
    ''' </summary>
    Public Sub ConfirmarActualizarFechaControlSeleccionadas()

        Dim HoraMinutos As String = "", logValidoHoraMinutos As Boolean = False

        MyBase.CambioItem("lstFechaControl")

        For Each objOperacion In (From c In lstFechaControl Where Not String.IsNullOrEmpty(c.ControlDivisas))
            If HoraMinutos = "" Then
                HoraMinutos = objOperacion.HorasMinutos
            Else
                HoraMinutos = HoraMinutos & ", " & objOperacion.HorasMinutos
            End If

        Next

        If String.IsNullOrEmpty(HoraMinutos) Then
            If logValidoHoraMinutos = False Then
                A2Utilidades.Mensajes.mostrarMensaje((DiccionarioEtiquetasPantalla("VALIDACION").Titulo), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If

        Else
            A2Utilidades.Mensajes.mostrarMensajePregunta((DiccionarioEtiquetasPantalla("CONFIRMACION").Titulo & " a " & HoraMinutos),
                              Program.TituloSistema,
                              "GEN",
                              AddressOf ActualizarFechaControlSeleccionadas,
                                                  False,
                                                  "",
                                                  False,
                                                  False,
                                                  True,
                                                  True,
                                                  "GEN")
        End If


    End Sub


    ''' <summary>
    ''' Proceso para actualizar la fecha control de Divisas
    ''' </summary>
    Public Async Sub ActualizarFechaControlSeleccionadas(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True
            Dim objResultadoMensaje As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultadoMensaje) AndAlso objResultadoMensaje.DialogResult Then

                Dim objRespuesta As InvokeResult(Of List(Of CPX_ControlCierreOPeracionesDivisasActualizar))

                For Each objOperacion In lstFechaControl
                    If Not String.IsNullOrEmpty(objOperacion.HorasMinutos) Then

                        objRespuesta = Await mdcProxy.OrdenesDivisasControlCierre_ActualizarAsync(objOperacion.HorasMinutos, Program.Usuario)

                        If Not IsNothing(objRespuesta) Then

                            For Each obj In (From c In objRespuesta.Value Where c.logExitoso = True)
                                A2Utilidades.Mensajes.mostrarMensaje(obj.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia,)
                                consultarFechaControlCierre()
                                Exit Sub
                            Next
                        End If
                    End If
                Next

                A2Utilidades.Mensajes.mostrarMensaje((DiccionarioEtiquetasPantalla("ACTUALIZACIONEXITOSA").Titulo), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                consultarFechaControlCierre()
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                         "ActualizarFechaControlSeleccionadas", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub

    Public Sub RefrescarOrden()
        Try
            consultarFechaControlCierre()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de la fecha control de Divisas", Me.ToString(), "refrescarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub
#End Region



End Class