Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.WEB
Imports Telerik.Windows.Controls
Imports System.Collections.ObjectModel
Imports System.Data
Imports SpreadsheetGear

Public Class ProcesarValoracionViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public objViewPrincipal As ProcesarValoracionView




#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' JAPC20200630_C-20200440
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
                Await OrdenesDivisasCombosEspecificos("DIVISAS", Nothing, Nothing, Nothing, Nothing)
                Await OrdenesCombosEspecificos("DIVISAS", "ESPECIFICOS", Nothing, Nothing, Nothing)
                Await OrdenesCombos()
                Await ConsultarOperacionesPendientes()
                Await ConsultarOperacionesValoradas()
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

    ''' <summary>
    ''' Diccionario para almacenar valores de combos especificos
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Private _dicCombosEspecificos As Dictionary(Of String, ObservableCollection(Of ItemLista))
    Public Property dicCombosEspecificos() As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Get
            Return _dicCombosEspecificos
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of ItemLista)))
            _dicCombosEspecificos = value
            CambioItem("dicCombosEspecificos")
        End Set
    End Property

    ''' <summary>
    ''' Diccionario para almacenar valores de combos producto
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Private _dicCombosProducto As Dictionary(Of String, ObservableCollection(Of ItemLista))
    Public Property dicCombosProducto() As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Get
            Return _dicCombosProducto
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of ItemLista)))
            _dicCombosProducto = value
            CambioItem("dicCombosProducto")
        End Set
    End Property

    ''' <summary>
    ''' Diccionario para almacenar valores de combos general
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Private _dicCombosGeneral As Dictionary(Of String, ObservableCollection(Of ItemLista))
    Public Property dicCombosGeneral() As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Get
            Return _dicCombosGeneral
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of ItemLista)))
            _dicCombosGeneral = value
            CambioItem("dicCombosGeneral")
        End Set
    End Property


    ''' <summary>
    ''' Fecha de corte para consultar operaciones cerradas y pendientes ciere
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Private _FechaCorte As Date = Date.Now
    Public Property FechaCorte() As Date
        Get
            Return _FechaCorte
        End Get
        Set(ByVal value As Date)
            _FechaCorte = value
            If Not IsNothing(value) AndAlso Not IsNothing(IDMoneda) AndAlso Not IsNothing(ClasificacionNegocio) Then
                RefrescarOperaciones()
            End If
            CambioItem("FechaCorte")
        End Set
    End Property


    ''' <summary>
    ''' Moneda para consultar operaciones cerradas y pendientes ciere
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Private _IDMoneda As Integer
    Public Property IDMoneda() As Integer
        Get
            Return _IDMoneda
        End Get
        Set(ByVal value As Integer)
            _IDMoneda = value
            If Not IsNothing(value) AndAlso Not IsNothing(FechaCorte) AndAlso Not IsNothing(ClasificacionNegocio) Then
                RefrescarOperaciones()
            End If
            CambioItem("IDMoneda")
        End Set
    End Property


    ''' <summary>
    ''' Clasificacion negocio para consultar operaciones cerradas y pendientes ciere
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Private _ClasificacionNegocio As String
    Public Property ClasificacionNegocio() As String
        Get
            Return _ClasificacionNegocio
        End Get
        Set(ByVal value As String)
            _ClasificacionNegocio = value
            If Not IsNothing(value) AndAlso Not IsNothing(FechaCorte) AndAlso Not IsNothing(IDMoneda) Then
                RefrescarOperaciones()
            End If
            CambioItem("ClasificacionNegocio")
        End Set
    End Property


    ''' <summary>
    ''' Lista tipo complex para almacenar listado de operaciones pendientes por valorar a una fecha de corte
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Public _lstOperacionesPendientes As List(Of CPX_OperacionesPendientes)
    Public Property lstOperacionesPendientes() As List(Of CPX_OperacionesPendientes)
        Get
            Return _lstOperacionesPendientes
        End Get
        Set(ByVal value As List(Of CPX_OperacionesPendientes))
            _lstOperacionesPendientes = value
            CambioItem("lstOperacionesPendientes")
        End Set
    End Property


    ''' <summary>
    ''' lista tipo complex para almacenar listado de operaciones cerradas  a una fecha de corte
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Public _lstOperacionesValoracion As List(Of CPX_OperacionesValoracion)
    Public Property lstOperacionesValoracion() As List(Of CPX_OperacionesValoracion)
        Get
            Return _lstOperacionesValoracion
        End Get
        Set(ByVal value As List(Of CPX_OperacionesValoracion))
            _lstOperacionesValoracion = value
            CambioItem("lstOperacionesValoracion")
        End Set
    End Property



    ''' <summary>
    ''' Operacion pendiente cierre seleccionada
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Public _OperacionPendienteSelected As CPX_OperacionesPendientes
    Public Property OperacionPendienteSelected() As CPX_OperacionesPendientes
        Get
            Return _OperacionPendienteSelected
        End Get
        Set(ByVal value As CPX_OperacionesPendientes)
            _OperacionPendienteSelected = value
            CambioItem("OperacionPendienteSelected")
        End Set
    End Property

    ''' <summary>
    ''' Operacion cerrada seleccionada
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Public _OperacionValoracionSelected As CPX_OperacionesValoracion
    Public Property OperacionValoracionSelected() As CPX_OperacionesValoracion
        Get
            Return _OperacionValoracionSelected
        End Get
        Set(ByVal value As CPX_OperacionesValoracion)
            _OperacionValoracionSelected = value
            CambioItem("OperacionPendienteSelected")
        End Set
    End Property


    Private _HabilitarCamposForward As Boolean = False
    Public Property HabilitarCamposForward() As Boolean
        Get
            Return _HabilitarCamposForward
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposForward = value
            CambioItem("HabilitarCamposForward")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Funcion para cargar combos especificos
    ''' JAPC20200630_C-20200440
    ''' </summary>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <returns></returns>
    Public Async Function OrdenesDivisasCombosEspecificos(ByVal pstrProducto As String,
                                        ByVal pstrCondicionTexto1 As String,
                                        ByVal pstrCondicionTexto2 As String,
                                        ByVal pstrCondicionEntero1 As Integer,
                                        ByVal pstrCondicionEntero2 As Integer) As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenesDivisas))

            objRespuesta = Await mdcProxy.OrdenesDivisasCombosEspecificosAsync(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    If String.IsNullOrEmpty(pstrCondicionTexto1) Then
                        dicCombosEspecificos = clsGenerales.CargarListasDivisas(objRespuesta.Value)
                    End If
                    If dicCombosEspecificos.ContainsKey("MONEDAS") Then
                        If Not IsNothing(dicCombosEspecificos("MONEDAS").FirstOrDefault) Then
                            IDMoneda = dicCombosEspecificos("MONEDAS").FirstOrDefault.Retorno
                        End If
                    End If


                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesDivisasCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try


    End Function

    ''' <summary>
    ''' Funcion para cargar combos especificos
    ''' JAPC20200630_C-20200440
    ''' </summary>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <returns></returns>
    Public Async Function OrdenesCombosEspecificos(ByVal pstrProducto As String,
                                        ByVal pstrCondicionTexto1 As String,
                                        ByVal pstrCondicionTexto2 As String,
                                        ByVal pstrCondicionEntero1 As Integer,
                                        ByVal pstrCondicionEntero2 As Integer) As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenes))

            objRespuesta = Await mdcProxy.OrdenesCombosEspecificosAsync(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    If pstrCondicionTexto1 = "ESPECIFICOS" Then
                        dicCombosProducto = clsGenerales.CargarListas(objRespuesta.Value)
                    End If

                    If dicCombosProducto.ContainsKey("CLASIFICACIONNEGOCIO") Then
                        If Not IsNothing(dicCombosProducto("CLASIFICACIONNEGOCIO").FirstOrDefault) Then
                            ClasificacionNegocio = dicCombosProducto("CLASIFICACIONNEGOCIO").Where(Function(x) x.Retorno = "FWD").FirstOrDefault.Retorno
                        End If
                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function


    ''' <summary>
    ''' funcion para cargar combos genericos
    ''' JAPC20200630_C-20200440
    ''' </summary>
    ''' <returns></returns>
    Public Async Function OrdenesCombos() As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenes))

            objRespuesta = Await mdcProxy.OrdenesCombosAsync(Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    dicCombosGeneral = clsGenerales.CargarListas(objRespuesta.Value)
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Function

    ''' <summary>
    ''' Funcion para consultar operaciones pendientes por cerrar a una fecha de corte
    ''' JAPC20200630_C-20200440
    ''' </summary>
    ''' <returns></returns>
    Public Async Function ConsultarOperacionesPendientes() As Task
        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_OperacionesPendientes))
            If Not IsNothing(FechaCorte) AndAlso Not IsNothing(IDMoneda) AndAlso Not IsNothing(ClasificacionNegocio) Then
                objRespuesta = Await mdcProxy.OperacionesPendientes_ConsultarAsync(FechaCorte, IDMoneda, ClasificacionNegocio, Program.Usuario)
                If Not IsNothing(objRespuesta) Then
                    lstOperacionesPendientes = objRespuesta.Value
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarOperacionesPendientes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function


    ''' <summary>
    ''' Funcion para consultar operaciones cerradas a una fecha de corte
    ''' JAPC20200630_C-20200440
    ''' </summary>
    ''' <returns></returns>
    Public Async Function ConsultarOperacionesValoradas() As Task
        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_OperacionesValoracion))
            If Not IsNothing(FechaCorte) AndAlso Not IsNothing(IDMoneda) AndAlso Not IsNothing(ClasificacionNegocio) Then
                objRespuesta = Await mdcProxy.OperacionesValoradas_ConsultarAsync(FechaCorte, IDMoneda, ClasificacionNegocio, Program.Usuario)
                If Not IsNothing(objRespuesta) Then
                    lstOperacionesValoracion = objRespuesta.Value
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarOperacionesValoradas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function


    ''' <summary>
    ''' Comando del botón para iniciar el cierre de operaciones
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Private WithEvents _ProcesarCmd As RelayCommand
    Public ReadOnly Property ProcesarCmd() As RelayCommand
        Get
            If _ProcesarCmd Is Nothing Then
                _ProcesarCmd = New RelayCommand(AddressOf ConfirmarCierreOperacionesSeleccionadas)
            End If
            Return _ProcesarCmd
        End Get
    End Property



    ''' <summary>
    ''' Comando del botón para deshacer cierre de operaciones
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Private WithEvents _DeshacerCmd As RelayCommand
    Public ReadOnly Property DeshacerCmd() As RelayCommand
        Get
            If _DeshacerCmd Is Nothing Then
                _DeshacerCmd = New RelayCommand(AddressOf ConfirmarDeshacerCierreOperacionesSeleccionadas)
            End If
            Return _DeshacerCmd
        End Get
    End Property


    ''' <summary>
    ''' Modal para Confirmar Cierre de operaciones con callback de respuesta usuario
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Public Sub ConfirmarCierreOperacionesSeleccionadas()


        A2Utilidades.Mensajes.mostrarMensajePregunta((DiccionarioEtiquetasPantalla("CONFIRMACIONCIERRE").Titulo & FechaCorte.ToString()),
                              Program.TituloSistema,
                              "GEN",
                              AddressOf CerrarOperacionesSeleccionadas,
                                                  False,
                                                  "",
                                                  False,
                                                  False,
                                                  True,
                                                  True,
                                                  "GEN")



    End Sub


    ''' <summary>
    ''' Modal para Confirmar deshacer cierre de operaciones con callback de respuesta usuario
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Public Sub ConfirmarDeshacerCierreOperacionesSeleccionadas()


        A2Utilidades.Mensajes.mostrarMensajePregunta((DiccionarioEtiquetasPantalla("CONFIRMACIONDESHACER").Titulo & FechaCorte.ToString()),
                              Program.TituloSistema,
                              "GEN",
                              AddressOf DeshacerCierreOperacionesSeleccionadas,
                                                  False,
                                                  "",
                                                  False,
                                                  False,
                                                  True,
                                                  True,
                                                  "GEN")



    End Sub



    ''' <summary>
    ''' Proceso para valorar operaciones forward-divisas
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Public Async Sub CerrarOperacionesSeleccionadas(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True
            Dim objResultadoMensaje As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultadoMensaje) AndAlso objResultadoMensaje.DialogResult Then
                If Not IsNothing(FechaCorte) AndAlso Not IsNothing(IDMoneda) AndAlso Not IsNothing(ClasificacionNegocio) Then
                    Dim objRespuesta As InvokeResult(Of List(Of CPX_MensajesGenerico)) = Await mdcProxy.OrdenesDivisasValoracion_ProcesarAsync(
                                                                                FechaCorte,
                                                                                IDMoneda,
                                                                                ClasificacionNegocio,
                                                                                Program.Usuario)

                    If Not IsNothing(objRespuesta) Then
                        If Not IsNothing(objRespuesta.Value) Then
                            Dim objListaResultado As List(Of CPX_MensajesGenerico) = CType(objRespuesta.Value, List(Of CPX_MensajesGenerico))
                            Dim objListaMensajes As New List(Of ProductoValidaciones)
                            Dim intIDRegistroActualizado As Integer = -1
                            Dim dtmRegistroActualizado As Date = Now


                            If (objListaResultado.All(Function(i) i.logExitoso)) Then
                                For Each li In objListaResultado
                                    objListaMensajes.Add(New ProductoValidaciones With {
                                                     .Mensaje = li.strMensaje
                                                     })
                                Next

                                Dim strMensaje As String = ""
                                For Each m In objListaMensajes
                                    strMensaje += m.Mensaje + vbCrLf
                                Next
                                A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

                                RefrescarOperaciones()

                            Else
                                For Each li In objListaResultado
                                    objListaMensajes.Add(New ProductoValidaciones With {
                                                     .Mensaje = li.strMensaje
                                                     })
                                Next

                                Dim strMensaje As String = ""
                                For Each m In objListaMensajes
                                    strMensaje += m.Mensaje + vbCrLf
                                Next
                                A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia,)

                                IsBusy = False
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de validaciones.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                            IsBusy = False
                        End If


                    End If

                End If

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                         "CerrarOperacionesSeleccionadas", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub


    ''' <summary>
    ''' Proceso para deshacer cierre de operaciones forward-divisas
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Public Async Sub DeshacerCierreOperacionesSeleccionadas(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True
            Dim objResultadoMensaje As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultadoMensaje) AndAlso objResultadoMensaje.DialogResult Then

                If Not IsNothing(FechaCorte) AndAlso Not IsNothing(IDMoneda) AndAlso Not IsNothing(ClasificacionNegocio) Then
                    Dim objRespuesta As InvokeResult(Of List(Of CPX_MensajesGenerico)) = Await mdcProxy.OrdenesDivisasDeshacerCierre_ProcesarAsync(
                                                                               FechaCorte,
                                                                               ClasificacionNegocio,
                                                                               Program.Usuario)

                    If Not IsNothing(objRespuesta) Then
                        If Not IsNothing(objRespuesta.Value) Then
                            Dim objListaResultado As List(Of CPX_MensajesGenerico) = CType(objRespuesta.Value, List(Of CPX_MensajesGenerico))
                            Dim objListaMensajes As New List(Of ProductoValidaciones)
                            Dim intIDRegistroActualizado As Integer = -1
                            Dim dtmRegistroActualizado As Date = Now


                            If (objListaResultado.All(Function(i) i.logExitoso)) Then
                                A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

                                RefrescarOperaciones()

                            Else
                                For Each li In objListaResultado
                                    objListaMensajes.Add(New ProductoValidaciones With {
                                                     .Mensaje = li.strMensaje
                                                     })
                                Next

                                Dim strMensaje As String = ""
                                For Each m In objListaMensajes
                                    strMensaje += m.Mensaje + vbCrLf
                                Next
                                A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia,)

                                IsBusy = False
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de validaciones.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                            IsBusy = False
                        End If

                    End If

                End If

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                         "DeshacerCierreOperacionesSeleccionadas", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub

    ''' <summary>
    ''' Metodo para realizar refresh de operaciones pendiente valorar y cerradas a una fecha de corte
    ''' JAPC20200630_C-20200440
    ''' </summary>
    Public Async Sub RefrescarOperaciones()
        Try
            IsBusy = True
            Await ConsultarOperacionesPendientes()
            Await ConsultarOperacionesValoradas()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de actualización de la orden", Me.ToString(), "refrescarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub
#End Region




End Class
