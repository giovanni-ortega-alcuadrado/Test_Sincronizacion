'Santiago Alexander Vergara Orrego
'Octubre 31/ 2018
'SV20181031_CUMPLIMIENTOOPERACIONES 

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web


Public Class CumplimientoViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------

    Public objViewPrincipal As CumplimientoView


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
                consultarOrdenesPendientes()
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


    Private _lstOrdenesPorCumplir As List(Of CPX_OrdenesPendientesCumplir)
    Public Property lstOrdenesPorCumplir() As List(Of CPX_OrdenesPendientesCumplir)
        Get
            Return _lstOrdenesPorCumplir
        End Get
        Set(ByVal value As List(Of CPX_OrdenesPendientesCumplir))
            _lstOrdenesPorCumplir = value
            MyBase.CambioItem("lstOrdenesPorCumplir")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Proceso para la consulta de la ordenes pendintes por cumplir
    ''' </summary>
    Public Async Sub consultarOrdenesPendientes()
        Try
            IsBusy = True
            Dim objRespuesta = Await mdcProxy.Ordenes_PendientesCumplimiento_ConsultarAsync(Program.Usuario)
            If Not IsNothing(objRespuesta) Then
                lstOrdenesPorCumplir = objRespuesta.Value
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                     "consultarOrdenesPendientes", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub


    ''' <summary>
    ''' Comando del botón para iniciar la ejecución del cumplimiento
    ''' </summary>
    Private WithEvents _CumplirCmd As RelayCommand
    Public ReadOnly Property CumplirCmd() As RelayCommand
        Get
            If _CumplirCmd Is Nothing Then
                _CumplirCmd = New RelayCommand(AddressOf ConfirmarcumplimientoOperacionesSeleccionadas)
            End If
            Return _CumplirCmd
        End Get
    End Property

    ''' <summary>
    ''' Confirmar cumplimiento de operaciones
    ''' </summary>
    Public Sub ConfirmarcumplimientoOperacionesSeleccionadas()

        Dim strFolios As String = "", logValidoFOlios As Boolean = False

        For Each objOperacionSin In (From c In lstOrdenesPorCumplir Where DateDiff(DateInterval.Day, CDate(Date.Now), c.dtmVigenciaHasta.Date) < 0 And Not String.IsNullOrEmpty(c.strFolio) Select c)
            A2Utilidades.Mensajes.mostrarMensaje((DiccionarioEtiquetasPantalla("VALIDACIONVIGENCIA").Titulo & " " & objOperacionSin.intConsecutivo.ToString), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            objOperacionSin.strFolio = String.Empty
            logValidoFOlios = True
        Next

        MyBase.CambioItem("lstOrdenesPorCumplir")

        For Each objOperacion In (From c In lstOrdenesPorCumplir Where Not String.IsNullOrEmpty(c.strFolio))
            If strFolios = "" Then
                strFolios = objOperacion.strFolio
            Else
                strFolios = strFolios & ", " & objOperacion.strFolio
            End If

        Next

        If String.IsNullOrEmpty(strFolios) Then
            If logValidoFOlios = False Then
                A2Utilidades.Mensajes.mostrarMensaje((DiccionarioEtiquetasPantalla("VALIDACION").Titulo), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If

        Else
                A2Utilidades.Mensajes.mostrarMensajePregunta((DiccionarioEtiquetasPantalla("CONFIRMACION").Titulo & strFolios),
                              Program.TituloSistema,
                              "GEN",
                              AddressOf CumplirOperacionesSeleccionadas,
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
    ''' Proceso para cumplir las operaciones seleccionadas de la lista
    ''' </summary>
    Public Async Sub CumplirOperacionesSeleccionadas(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True
            Dim objResultadoMensaje As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultadoMensaje) AndAlso objResultadoMensaje.DialogResult Then

                Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesOrdenes))

                For Each objOperacion In lstOrdenesPorCumplir
                    If Not String.IsNullOrEmpty(objOperacion.strFolio) Then

                        objRespuesta = Await mdcProxy.Ordenes_Pendientes_CumplirAsync(objOperacion.intIDOrden, objOperacion.strFolio, Program.Usuario)

                        If Not IsNothing(objRespuesta) Then

                            For Each obj In (From c In objRespuesta.Value Where c.logInconsitencia = True)
                                A2Utilidades.Mensajes.mostrarMensaje(obj.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia,)
                                consultarOrdenesPendientes()
                                Exit Sub
                            Next
                        End If
                    End If
                Next

                A2Utilidades.Mensajes.mostrarMensaje((DiccionarioEtiquetasPantalla("ACTUALIZACIONEXITOSA").Titulo), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                consultarOrdenesPendientes()

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                         "CumplirOperacionesSeleccionadas", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub

    Public Sub RefrescarOrden()
        Try
            consultarOrdenesPendientes()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de actualización de la orden", Me.ToString(), "refrescarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub
#End Region

End Class
