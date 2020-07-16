'Ricardo Barrientos Perez
'Diciembre 03/ 2018
'RABP20181203 _APROBACIONPREORDENES 

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.WEB
Imports Telerik.Windows.Controls
Imports System.Collections.ObjectModel

Public Class AprobacionViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------

    Public objViewPrincipal As AprobacionView

    'JAPC20200210 (C-20190368) variable para almacenar modal de desaprobacion orden
    Dim window As RadWindow = New RadWindow()


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
                ConsultarOrdenesEstadoPreorden()
                traerCorreos()
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


    Private _lstOrdenesParaAprobar As List(Of CPX_AprobacionPreordenesConsultar)
    Public Property lstOrdenesParaAprobar() As List(Of CPX_AprobacionPreordenesConsultar)
        Get
            Return _lstOrdenesParaAprobar
        End Get
        Set(ByVal value As List(Of CPX_AprobacionPreordenesConsultar))
            _lstOrdenesParaAprobar = value
            MyBase.CambioItem("lstOrdenesParaAprobar")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20200211 C-20190368 propiedad para identificar orden a devolver
    ''' </summary>
    Private _OrdenSelected As CPX_AprobacionPreordenesConsultar
    Public Property OrdenSelected() As CPX_AprobacionPreordenesConsultar
        Get
            Return _OrdenSelected
        End Get
        Set(ByVal value As CPX_AprobacionPreordenesConsultar)
            _OrdenSelected = value
        End Set
    End Property


    ''' <summary>
    ''' JAPC20200211 C-20190368 propiedad para identificar observacion de devolucion
    ''' </summary>
    Private _Observacion As String
    Public Property Observacion() As String
        Get
            Return _Observacion
        End Get
        Set(ByVal value As String)
            _Observacion = value
        End Set
    End Property


    Private _ListaCorreos As List(Of clx_CorreosPersonas)
    Public Property ListaCorreos() As List(Of clx_CorreosPersonas)
        Get
            Return _ListaCorreos
        End Get
        Set(ByVal value As List(Of clx_CorreosPersonas))
            _ListaCorreos = value
            MyBase.CambioItem("ListaCorreos ")
        End Set
    End Property


    Private _ListaCorreosSelected As ObservableCollection(Of clx_CorreosPersonas) = New ObservableCollection(Of clx_CorreosPersonas)
    Public Property ListaCorreosSelected() As ObservableCollection(Of clx_CorreosPersonas)
        Get
            Return _ListaCorreosSelected
        End Get
        Set(ByVal value As ObservableCollection(Of clx_CorreosPersonas))
            If Not IsNothing(value) Then
                _ListaCorreosSelected = value
                MyBase.CambioItem("ListaCorreosSelected ")
            End If
        End Set
    End Property


    Private _logAprobar As Boolean = False
    Public Property logAprobar() As Boolean
        Get
            Return _logAprobar
        End Get
        Set(ByVal value As Boolean)
            _logAprobar = value
            MyBase.CambioItem("logAprobar ")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Proceso para la consulta de la ordenes en estado preorden
    ''' </summary>
    Public Async Sub ConsultarOrdenesEstadoPreorden()
        Try
            IsBusy = True
            Dim objRespuesta = Await mdcProxy.Ordenes_AprobacionPreordenes_ConsultarAsync(Program.Usuario)
            If Not IsNothing(objRespuesta) Then
                lstOrdenesParaAprobar = objRespuesta.Value
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
                _CumplirCmd = New RelayCommand(AddressOf ConfirmarAprobacionOrdenesSeleccionadas)
            End If
            Return _CumplirCmd
        End Get
    End Property

    ''' <summary>
    ''' JAPC20200210 (C-20190368): Comando del botón para iniciar la ejecución de desaprobacion
    ''' </summary>
    Private WithEvents _AceptarDesabrobacionCmd As RelayCommand
    Public ReadOnly Property AceptarDesabrobacionCmd() As RelayCommand
        Get
            If _AceptarDesabrobacionCmd Is Nothing Then
                _AceptarDesabrobacionCmd = New RelayCommand(AddressOf ConfirmarDesaprobacionOrdenesSeleccionadas)
            End If
            Return _AceptarDesabrobacionCmd
        End Get
    End Property


    ''' <summary>
    ''' JAPC20200210 (C-20190368): Comando del botón para cancelar la ejecución de desaprobacion
    ''' </summary>
    Private WithEvents _CancelarDesabrobacionCmd As RelayCommand
    Public ReadOnly Property CancelarDesabrobacionCmd() As RelayCommand
        Get
            If _CancelarDesabrobacionCmd Is Nothing Then
                _CancelarDesabrobacionCmd = New RelayCommand(AddressOf CancelarDesaprobacionOrdenesSeleccionadas)
            End If
            Return _CancelarDesabrobacionCmd
        End Get
    End Property


    ''' <summary>
    ''' JAPC20200210 (C-20190368): Comando del botón para cancelar la ejecución de desaprobacion
    ''' </summary>
    Public Sub CancelarDesaprobacionOrdenesSeleccionadas()
        Try
            window.Close()
            ListaCorreosSelected.RemoveAll()
            Observacion = Nothing

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Error. CancelarDesaprobacionOrdenesSeleccionadas:",
                                                     Me.ToString(), "CancelarDesaprobacionOrdenesSeleccionadas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub



    ''' <summary>
    ''' JAPC20200210 (C-20190368):Comando del botón para iniciar la ejecución de la desaprobación de la orden
    ''' </summary>
    Private WithEvents _DesaprobarCmd As RelayCommand
    Public ReadOnly Property DesaprobarCmd() As RelayCommand
        Get
            If _DesaprobarCmd Is Nothing Then
                _DesaprobarCmd = New RelayCommand(AddressOf DesaprobacionOrdenModal)
            End If
            Return _DesaprobarCmd
        End Get
    End Property


    ''' <summary>
    ''' JAPC20200210 (C-20190368): metodo para cargar los correos de las personas y terceros
    ''' </summary>
    Private Async Sub traerCorreos()
        Try
            Dim z = Await mdcProxy.GetCorreosReceptores_ConsultarAsync()
            ListaCorreos = z.Value

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Error. traer Correos:",
                                                     Me.ToString(), "traerCorreos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub



    ''' <summary>
    ''' JAPC20200210 (C-20190368): Confirmar desaprobracion orden seleccionada
    ''' </summary>
    Public Sub ConfirmarDesaprobacionOrdenesSeleccionadas()

        Dim logConsecutivoSeleccionado As Boolean = False, intConsecutivo As String = ""



        MyBase.CambioItem("lstOrdenesParaAprobar")

        'DiccionarioEtiquetasPantalla("CONFIRMACIONDESAPROBACION").Titulo
        A2Utilidades.Mensajes.mostrarMensajePregunta("Esta seguro que desea devolver la orden:" & String.Format(intConsecutivo),
                              Program.TituloSistema,
                              "GEN",
                              AddressOf DesaprobarOrdenSeleccionada,
                                                  False,
                                                  "",
                                                  False,
                                                  False,
                                                  True,
                                                  True,
                                                  "GEN")



    End Sub

    ''' <summary>
    ''' JAPC20200210 (C-20190368) : Proceso para desabrobar la orden seleccionada
    ''' </summary>
    Public Sub DesaprobarOrdenSeleccionada(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True
            Dim objResultadoMensaje As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultadoMensaje) AndAlso objResultadoMensaje.DialogResult Then


                Dim objEnvio As New clsEnvioCorreo

                Dim strCorreos As String = ""
                Dim strTipo As String = ""

                Dim strCorreoReceptor As String = ""

                strCorreoReceptor = (From i In ListaCorreos Where i.ID = OrdenSelected.strIDReceptor Select i.strEmail).FirstOrDefault()

                For Each item In ListaCorreosSelected
                    strCorreos = strCorreos & item.strEmail & ";"
                Next

                If Not IsNothing(strCorreoReceptor) Then
                    strCorreos = strCorreos & strCorreoReceptor
                Else
                    '(DiccionarioEtiquetasPantalla("RECEPTORSINEMAIL").Titulo)
                    A2Utilidades.Mensajes.mostrarMensaje("El receptor de la orden no tiene email configurado" & " ID Receptor: " & OrdenSelected.strIDReceptor, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)

                    If Not IsNothing(strCorreos) Then
                        strCorreos = strCorreos.Substring(0, strCorreos.Length - 1)
                    End If
                End If

                'If Not IsNothing(strCorreos) Then
                '    strCorreos = strCorreos.Substring(0, strCorreos.Length - 1)
                'End If


                If OrdenSelected.strTipo = "C" Then
                    strTipo = "Compra"
                ElseIf OrdenSelected.strTipo = "V" Then
                    strTipo = "Venta"
                Else
                    strTipo = "Sin Tipo"
                End If

                'DiccionarioEtiquetasPantalla("EMAILDESAPROBACION").Titulo
                objEnvio.PrepararCorreoEvento("Desaprobacion orden divisas",
                                              strCorreos,
                                              OrdenSelected.strReceptorNombre,
                                              OrdenSelected.intConsecutivo,
                                              strTipo,
                                              OrdenSelected.strUsuarioCreacion,
                                              Program.Usuario(),
                                              Observacion,
                                              OrdenSelected.intIDComitente,
                                              OrdenSelected.dblCantidad,
                                              OrdenSelected.dblPrecio,
                                              Date.Now,
                                              OrdenSelected.dtmCreacion
                                                )

                '(DiccionarioEtiquetasPantalla("ACTUALIZACIONEXITOSADESABROBACION").Titulo)
                A2Utilidades.Mensajes.mostrarMensaje("Se desaprobó y notifico estado de la orden exitosamente", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                ConsultarOrdenesEstadoPreorden()

                window.Close()
                ListaCorreosSelected.RemoveAll()
                Observacion = Nothing
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                         "AprobarOperacionesSeleccionadas", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub



    ''' <summary>
    ''' Confirmar cumplimiento de operaciones
    ''' </summary>
    Public Sub ConfirmarAprobacionOrdenesSeleccionadas()

        Dim logConsecutivoSeleccionado As Boolean = False, intConsecutivo As String = ""

        'For Each objOperacionSin In (From c In lstOrdenesParaAprobar Where DateDiff(DateInterval.Day, CDate(Date.Now.ToString("dd/MM/yyyy")), c.dtmVigenciaHasta.Value) < 0 And (c.Aprobar = True) Select c)
        For Each objOperacionSin In (From c In lstOrdenesParaAprobar Where DateDiff(DateInterval.Day, CDate(Date.Now), c.dtmVigenciaHasta.Value) < 0 And (c.Aprobar = True) Select c)
            A2Utilidades.Mensajes.mostrarMensaje((DiccionarioEtiquetasPantalla("VALIDACIONVIGENCIA").Titulo & " " & objOperacionSin.intConsecutivo.ToString), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            objOperacionSin.Aprobar = False
        Next

        MyBase.CambioItem("lstOrdenesParaAprobar")

        For Each objOperacion In (From c In lstOrdenesParaAprobar Where Not String.IsNullOrEmpty(c.intConsecutivo))
            If objOperacion.Aprobar = True Then
                'intConsecutivo = objOperacion.intConsecutivo
                intConsecutivo = intConsecutivo & ", " & String.Format(objOperacion.intConsecutivo)
                logConsecutivoSeleccionado = True
            End If

        Next

        If logConsecutivoSeleccionado = False Then

            A2Utilidades.Mensajes.mostrarMensaje((DiccionarioEtiquetasPantalla("VALIDACION").Titulo), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)

        Else
            A2Utilidades.Mensajes.mostrarMensajePregunta((DiccionarioEtiquetasPantalla("CONFIRMACION").Titulo & String.Format(intConsecutivo)),
                              Program.TituloSistema,
                              "GEN",
                              AddressOf AprobarOperacionesSeleccionadas,
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
    Public Async Sub AprobarOperacionesSeleccionadas(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True
            Dim objResultadoMensaje As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultadoMensaje) AndAlso objResultadoMensaje.DialogResult Then

                Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesOrdenes))

                For Each objOperacion In lstOrdenesParaAprobar
                    If objOperacion.Aprobar = True Then

                        objRespuesta = Await mdcProxy.Ordenes_AprobacionPreordenes_AprobarAsync(objOperacion.intIDOrden, objOperacion.dtmEstado, Program.Usuario)

                        If Not IsNothing(objRespuesta) Then

                            For Each obj In (From c In objRespuesta.Value Where c.logInconsitencia = True)
                                A2Utilidades.Mensajes.mostrarMensaje(obj.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia,)
                                ConsultarOrdenesEstadoPreorden()
                                Exit Sub
                            Next
                        End If
                    End If
                Next

                A2Utilidades.Mensajes.mostrarMensaje((DiccionarioEtiquetasPantalla("ACTUALIZACIONEXITOSA").Titulo), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                ConsultarOrdenesEstadoPreorden()

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                         "AprobarOperacionesSeleccionadas", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub

    Public Sub RefrescarOrden()
        Try
            ConsultarOrdenesEstadoPreorden()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de actualización de la orden", Me.ToString(), "refrescarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub
#End Region


#Region "Modales"


    ''' <summary>
    ''' JAPC20200210 (C-20190368) Metodo para invocar modal para desaprobacion de la orden
    ''' </summary>
    ''' <remarks>JAPC20200210 (C-20190368)</remarks>
    Public Sub DesaprobacionOrdenModal()
        Try

            'If Not IsNothing(OrdenSelected) Then
            '    If (OrdenSelected.logDesaprobar = False) Then
            '        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_DESABROBACION_SINLOGDESAPROBACION"), "consultarOrdenesPendientes", wppMensajes.TiposMensaje.Advertencia)
            '        Exit Sub
            '    End If
            'Else
            '    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_DESABROBACION_SINORDEN"), "consultarOrdenesPendientes", wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If

            If Not IsNothing(OrdenSelected) Then
                If (OrdenSelected.logDesaprobar = False) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe marcar la orden como desaprobada para realizar este proceso", " ", wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la orden a desaprobar", " ", wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            window.Owner = Application.Current.MainWindow
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen
            window.ResizeMode = ResizeMode.NoResize
            window.DataContext = Me


            window.Header = " "

            Dim contentWidth = 600
            Dim contentHeight = 290
            window.CanClose = False
            window.IsEnabled = True


            Dim objSimulador = New DesaprobacionModalView()


            window.Content = objSimulador

            window.Width = contentWidth
            window.Height = contentHeight
            window.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                         "DesaprobacionOrdenModal", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

#End Region
End Class
