'SV20181113: Ajustes Importacion SETFX
'Santiago Alexander Vergara Orrego 

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web


Public Class ImportacionOperacionesSETFXViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Dim strTipoFiltroBusqueda As String = String.Empty

    Public objViewPrincipal As ImportacionOperacionesSETFXView


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

    Private _logInconsistencia As Boolean = True
    Public Property logInconsistencia() As Boolean
        Get
            Return _logInconsistencia
        End Get
        Set(ByVal value As Boolean)
            _logInconsistencia = value
            establecerMensajesMostrar()
            MyBase.CambioItem("logInconsistencia")
        End Set
    End Property

    Private _logInformativo As Boolean = True
    Public Property logInformativo() As String
        Get
            Return _logInformativo
        End Get
        Set(ByVal value As String)
            _logInformativo = value
            establecerMensajesMostrar()
            MyBase.CambioItem("logInformativo")
        End Set
    End Property


    Private _lstResultadoValidaciones As List(Of CPX_tblValidacionesCargaOperaciones)
    Public Property lstResultadoValidaciones() As List(Of CPX_tblValidacionesCargaOperaciones)
        Get
            Return _lstResultadoValidaciones
        End Get
        Set(ByVal value As List(Of CPX_tblValidacionesCargaOperaciones))
            _lstResultadoValidaciones = value
            MyBase.CambioItem("lstResultadoValidaciones")
        End Set
    End Property


    Private _ListaMensajes As List(Of String) = New List(Of String)
    Public Property ListaMensajes() As List(Of String)
        Get
            Return _ListaMensajes
        End Get
        Set(ByVal value As List(Of String))
            _ListaMensajes = value
            MyBase.CambioItem("ListaMensajes")
        End Set
    End Property


    ''' <summary>
    ''' Comando del botón para consultar la información
    ''' </summary>
    Private WithEvents _ImportarCmd As RelayCommand
    Public ReadOnly Property ImportarCmd() As RelayCommand
        Get
            If _ImportarCmd Is Nothing Then
                _ImportarCmd = New RelayCommand(AddressOf ImportarArchivoOperaciones)
            End If
            Return _ImportarCmd
        End Get
    End Property

    ''' <summary>
    ''' Consultar todos los datos de los formularios
    ''' SV20181113
    ''' </summary>
    Public Async Sub ImportarArchivoOperaciones()
        Try
            IsBusy = True

            ListaMensajes = New List(Of String)

            _logInconsistencia = True
            _logInformativo = True

            Dim objRespuesta1 = Await mdcProxy.CargarArchivoOperaciones_ValidarAsync(False, Program.Usuario)
            If Not IsNothing(objRespuesta1) Then
                lstResultadoValidaciones = objRespuesta1.Value
            End If

            establecerMensajesMostrar()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString() _
                                                        , "ImportarArchivoOperaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Establecer la lista de mensajes para mostrar
    ''' SV20181113
    ''' </summary>
    Public Sub establecerMensajesMostrar()
        Try

            Dim objListaMensajes As New List(Of String)

            'Lógica para recibir las respuestas y validaciones del servidor
            If lstResultadoValidaciones.Count > 0 Then
                If lstResultadoValidaciones.Where(Function(i) i.logInconsitencia = True).Count > 0 And _logInconsistencia = True Then

                    For Each li In lstResultadoValidaciones.Where(Function(i) i.logInconsitencia = True).OrderBy(Function(o) o.strTipoMensaje)
                        If li.strTipoMensaje = "INCONSISTENCIA" Then
                            objListaMensajes.Add("Inconsistencia: " & li.strMensaje)
                        Else
                            objListaMensajes.Add(String.Format("Fila: {0} - Validación: {1}", IIf(IsNothing(li.intFila) OrElse li.intFila = 0, "Desconocida", li.intFila), li.strMensaje))
                        End If
                    Next

                End If

                If _logInformativo Then
                    For Each li In lstResultadoValidaciones.Where(Function(i) i.logInconsitencia = False)
                        objListaMensajes.Add("Mensaje informativo: " & li.strMensaje & li.intIDRegistro.ToString)
                    Next
                End If

                ListaMensajes = objListaMensajes

            Else
                ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString() _
                                                         , "establecerMensajesMostrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region


End Class

